# ASHFALL — WAVE 2 INTEGRATION PROGRAM · PLAN 1 OF 6

# MAINTENANCE & TRUTH-GRADE CODEBASE INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W2 (six-plan integration wave)
**Document:** W2-01 · part A of F
**Target size:** ~150,000 characters (this plan)
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W2-02 (bugs/silent failures), W2-03 (gameplay), W2-04 (environments), W2-05 (locations), W2-06 (enrichment)
**Plan-unblocking annex:** Annex U at the end of this document — deliberately separated from the integration body per the Wave 2 rule.

---

## 0. How to read this plan

This is an **integration plan**, not an audit and not a wish list. It is written
so that a builder can take one phase, verify its premise, claim its paths, and
execute it with focused tests. It is also written so that a **chooser** (the
user/foreman) can select a depth per plan and per point before anything is
built.

### 0.1 Two selection levels

**Level 1 — Plan Path (choose one per plan):**

| Plan Path | Name | Meaning |
|---|---|---|
| **Path A** | Patch & Preserve | smallest truthful repairs; no structural work; fastest; lowest regression surface |
| **Path B** | Strengthen & Standardize | bounded structural work where evidence proves it pays; new gates; medium effort |
| **Path C** | Deep Rebuild | full system-level consolidation of the maintenance domain; largest effort and risk |

**Level 2 — Point Paths (choose A, B, or C at each of the 10 decision points):**

Every decision point below has three complete options. Choosing Path B at the
plan level does **not** force Path B at every point: the selection sheet in
Appendix A lets you mix (e.g., Plan Path B, but Points 3 and 7 at Path C, Point
10 at Path A). The plan-level path is only a default mapping.

### 0.2 Default mapping (Plan Path → point defaults)

| Plan Path | Points mostly at A | Points mostly at B | Points mostly at C |
|---|---|---|---|
| A — Patch & Preserve | 1–10 | — | — |
| B — Strengthen & Standardize | 1,3 | 2,4,5,6,7,8,9 | 10 |
| C — Deep Rebuild | — | 2,5 | 1,3,4,6,7,8,9,10 |

You may override any row. If you select nothing, the integrating foreman follows
the **recommended** path marked at each point (a recommendation is recorded for
every point).

### 0.3 The Wave 2 rule for this plan

> **Maintenance does not add gameplay.** This plan may repair, consolidate,
> standardize, and delete — never introduce a new player-facing feature. Any
> proposal that would add a feature is moved to its owning Wave 2 plan
> (gameplay W2-03, environments W2-04, locations W2-05, enrichment W2-06) or to
> Annex U if it unblocks another plan.

### 0.4 What "truth-grade" means here

A codebase is truth-grade when five statements are simultaneously true:

1. **Build truth** — the repository builds from a clean checkout with the
   documented command and zero errors.
2. **Gate truth** — every generated artifact passes its `--check` mode.
3. **Test truth** — the focused suites pass; the quarantine list reflects real
   files; no test relies on static state that leaks between cases.
4. **Data truth** — every catalog parses, conforms to its declared schema
   version, and is consumed by at least one live path (or is explicitly
   archived).
5. **Ledger truth** — the live ledgers match reality (shared with W2-02 and the
   earlier UNBLOCK-04; this plan only enforces, it does not re-litigate).

A maintenance package is only accepted when it improves at least one of these
five **without degrading any other**.

---

## 1. Executive summary

The ASHFALL repository is in unusually good shape for its size: 1,032 Core
files, 844 host files, 1,144 test files, 338 JSON catalogs, a generated-artifact
gate family, an architecture map with a `--check` mode, and a debt ledger where
the overwhelming majority of rows are RETIRED with dated evidence. The wave-1
unblocker program (UNBLOCK-01…05) confirmed the big blockers are decisions, not
hidden breakage.

What remains is **maintenance debt of scale**, not catastrophe:

1. **`Main.cs` orchestration sprawl** — 148 partial-class files, 41,172 lines
   across `src/*.cs` top-level alone, with a hand-maintained setup order and a
   parallel declarative manifest bootstrap.
2. **Generated-artifact drift risk** — 30+ `scripts/ci/*` generators with
   `--check` modes; when a package edits state without re-running them, docs
   drift silently (UNBLOCK-04 already found seven drift classes).
3. **Data-hygiene seams** — all 338 catalogs parse and carry `schema_version`,
   but the `locations.json` `dangerLevel` field is authored with **mixed JSON
   number forms** (`3`, `8.0`, `10.0` coexisting) and eight locations lack
   `travelHours`; catalog conventions vary between camelCase DTO loads and
   snake_case authorities.
4. **Test-suite hygiene** — 1,144 test files; the targeted-test policy exists
   but the practical ability to know *which* suite owns *which* path lives in
   the architecture map, which must be maintained by hand.
5. **Dead/stale surface** — historical plan trees, deleted-but-referenced
   fixtures, comment-only "quarantine" phantoms, and zero-consumer code are
   individually small but collectively create false premises for every future
   agent.

This plan turns those into ten bounded integration decision points, each with
three complete execution paths. Nothing here is speculative: every point cites
current file/line evidence or a current ledger row.

### 1.1 What the user gets at the end of each Plan Path

| Outcome | Path A | Path B | Path C |
|---|---|---|---|
| Generated-artifact gate discipline | documented manual procedure | enforced gate matrix in one script | full regenerate-and-check harness with CI wiring |
| Main.cs structure | partition documented | bounded extraction of 3–5 cohesive groups | full module decomposition with manifest parity |
| Data hygiene | the two known defects fixed | conventions codified per catalog family | loader-schema unification pass |
| Test hygiene | quarantine truth stated | static-state isolation gate | architecture-map ownership enforcement |
| Dead surface | report only | delete proven-zero-consumer sets | consolidated archive program |
| Diagnostics noise | measure only | catch-policy gate enforcement | structured logging boundary |

---

## 2. Verified current state (maintenance evidence)

All evidence below was gathered read-only at HEAD `5be1a30a` on 2026-09-21.
Line numbers are exact for this revision.

### 2.1 Orchestration scale

| Fact | Command | Result |
|---|---|---|
| Core source files | `find Assets/Ashfall.Core -name "*.cs" \| wc -l` | 1,032 |
| Host source files | `find src -name "*.cs" \| wc -l` | 844 |
| Test source files | `find Ashfall.Core.Tests -name "*.cs" \| wc -l` | 1,161 |
| Data catalogs | `ls Assets/StreamingAssets/Data/ \| wc -l` | 342 |
| `Main*.cs` partials | `find src -name "Main*.cs" \| wc -l` | 148 |
| Largest host partials | `wc -l src/Main.UiPanels.cs src/Main.CampaignOwners.cs` | 1,700 / 1,515 |
| Bare `catch {}` | repo grep | 1 (a comment in `CatalogDiagnostics.cs`) |
| TODO/FIXME/HACK | repo grep | 2 |

**Interpretation:** the orchestration layer is large but honestly structured: the
sprawl is in *files* (`main` partials), not in any single monolith, and the
dangerous patterns (bare catches, TODO rot) are effectively absent. Maintenance
work here is governance, not rescue.

### 2.2 Generated artifact family

`scripts/ci/` contains (sampled): `generate-architecture-map.py`,
`generate-catalog-registry.py`, `generate-docs-index.py`,
`generate-cli-catalog.sh`, `generate-core-systems-catalog.py`,
`generate-expansions-catalog.py`, `generate-plan-register.py`,
`generate-asset-registry.py`, `generate-audio-catalog.py`,
`generate-save-store-matrix.py`, `generate-port-contract.py`,
`generate-collectibles-matrix.py`, plus gates: `forbidden-api-gate.sh`,
`catch-policy-gate.sh`, `doc-link-gate.sh`, `case-collision-gate.sh`,
`asset-decode-gate.py`, `audio-asset-gate.py`, `content-acceptance-gate.sh`,
`coverage-gate.sh`, `agent-fast-verify.py`, `export-build.sh`.

Generated artifacts: `docs/INDEX.md` (718,798 bytes),
`docs/architecture/ARCHITECTURE_TEST_MAP.md` (249,502 bytes),
`docs/ci/SELFTEST_MANIFEST.json` (64,573 bytes), plus the registries under
`docs/data/`, `docs/ui/`, `docs/cli/`, `docs/architecture/`.

**Interpretation:** the repository already has the machinery; the maintenance
risk is that a package edits source/data and forgets to re-run one generator.
Every `--check` gate is only as strong as the discipline that runs it.

### 2.3 Data-hygiene findings (verified)

**Finding M-1 — mixed numeric forms in one authority field.**

`Assets/StreamingAssets/Data/locations.json` `dangerLevel` values as authored:

```
3 (×29), 5 (×22), 4 (×21), 6 (×18), 2 (×17), 7 (×14),
8.0 (×10), 9.0 (×8), 7.0 (×8), 1 (×8), 8 (×7), 6.0 (×7),
10.0 (×4), 5.0 (×3), 4.0 (×2), 0 (×1)
```

The same field is authored as integer literals for some rows and float literals
for others. The loader parses both, so gameplay is currently unaffected, but
this is exactly the kind of drift a future schema migration or diff-based tool
misreads.

**Finding M-2 — travel-hour gaps.**

`travelHours` present in 171 / 179 rows; eight locations have no travel time.
`baseRadsPerHour` present in 178 / 179. Only one row carries
`requiredFlagId`, one `cleanWaterRewardFlag`, one `ambushFlag`.

**Finding M-3 — catalog convention split.**

All 338 catalogs parse and all carry `schema_version` (good). However, the C#
side mixes camelCase DTOs with `JsonPropertyName` aliases
(`displayName`, `positionX`, `dangerLevel` in `WastelandMapCatalogLoader`) and
snake_case authorities (`display_name` in `difficulty_presets.json`,
`scalars` blocks). A repo-wide "normalize to snake_case" would break loaders
unless done per catalog family with loader evidence.

### 2.4 Reset-path maintenance risk (shared with W2-02)

Static check: `grep -rn "_silentFoundry = null\|_sharedFactionStance = null\|_sharedSkillProgression = null" src/`
→ **zero assignments**. The three fields are host caches created by guarded
setups:

- `src/Main.ExpansionHub.cs:35` — `_silentFoundry` field.
- `src/Main.Economy.cs:210–212` — `SetupSilentFoundry()` early-returns when
  non-null.
- `src/Main.CampaignServices.cs:173` — `_sharedSkillProgression` created inside
  `EnsureSharedSkillProgression()`.
- `src/Main.CampaignServices.cs:195–207` — `_sharedFactionStance` created inside
  `EnsureSharedFactionStance()` which calls `SetupSilentFoundry()`.

Meanwhile `Main.Lifecycle.cs` does null their dependencies on reset
(`_inventory` line 73, `_expansions` line 110, `_economy` line 161, `_journal`
line 231). This plan records the finding as **maintenance evidence**; the
repair decision belongs to W2-02 (bug plan). Annex U tracks the handoff.

### 2.5 Test and quarantine state

- `DEBT-TEST-QUARANTINE-2026-09-12` is RECONCILED: the historical drain is
  complete; `AGENTS.md`'s "48 active" line is stale (UNBLOCK-04).
- `QuarantineManifestGateTests` requires every future `Compile Remove` to name
  a real source file.
- Known test-isolation history: a `TradeSpecialtySystem` static-state
  test-isolation flake was fixed test-side in D1 (2026-09-17) — 11,697/11,697.
- 61 `static` field initializations exist in Core (`= new` patterns), a
  standing isolation surface worth one gate (decision point 8).

---

## 3. Scope, non-goals, and authority rules

### 3.1 In scope

- Orchestration file-layout governance and the boundary between direct
  `Setup*` calls and `SubsystemManifest` delegates.
- Generated-artifact regeneration discipline and gate-matrix truth.
- Data-hygiene repairs that preserve loader semantics byte-for-byte at the
  gameplay level (numeric normalization, missing-field defaults).
- Test hygiene: quarantine truth, static-state isolation, targeted-suite
  mapping.
- Dead-surface identification and (under Path B/C) bounded deletion.
- Diagnostics/catch-policy enforcement and log-noise measurement.
- Documentation-of-record indexing (not narrative docs — those are W2-06).

### 3.2 Non-goals (explicit)

- **No gameplay changes.** Balance, difficulty, pacing → W2-03.
- **No environment content or mechanics** → W2-04.
- **No location tiers, map growth, or POI systems** → W2-05.
- **No prose, voices, or lore** → W2-06.
- **No bug *repairs*** beyond upkeep of gates; behavioural defects → W2-02.
- **No new save sections, no schema-version bumps** without a signed decision.
- **No Unity anything** (Rule 1).
- **No restoration of retired behaviour** (Rule 10; `KNOWN_DEBT.md` rows).

### 3.3 Authority rules binding on every phase

1. `Assets/Ashfall.Core/` stays engine-free (Rule 2).
2. JSON under `Assets/StreamingAssets/Data/` stays authoritative (Rule 3).
3. Deterministic behaviour preserved: no `System.Random`, no wall-clock, no
   hash-order iteration in Core (Rule 4; current greps show only comments
   asserting this — keep it that way).
4. One authority per concern (Rule 5): maintenance must **not** invent a
   parallel system, registry, or cache to fix hygiene.
5. Focused verification only (`TEST_POLICY.md`); `scripts/run_test.sh` bounded.
6. Generated files are never hand-edited; run the owning generator (AGENTS.md
   tools rule).
7. Existing dirty worktree state is preserved; no mass formatting.

---

## 4. Plan Path selection and decision-point index

### 4.1 The ten decision points

| # | Decision point | Default recommendation | Owning evidence |
|---|---|---|---|
| 1 | `Main` partial-class layout governance | **B** | 148 partials, 41,172 lines |
| 2 | Generated-artifact gate discipline | **B** | 30+ generators, `--check` family |
| 3 | Data numeric-form normalization (`dangerLevel`) | **A** | mixed int/float forms |
| 4 | Missing-field defaults (`travelHours`, flags) | **A** | 8/179 gaps |
| 5 | Catalog naming-convention codification | **B** | camelCase DTO vs snake_case authority |
| 6 | Test-quarantine and static-state isolation | **B** | 61 static inits; flake history |
| 7 | Dead/zero-consumer surface reduction | **B** | plan trees, phantom references |
| 8 | Diagnostics/log-noise boundary | **A** | one bare-catch comment; 619 `catch (Exception` |
| 9 | Architecture-map and registry ownership | **B** | generated docs owned by hand |
| 10 | CI gate matrix and maintenance contract | **C** | no single matrix doc |

### 4.2 Selection sheet (fill before integration)

```text
PLAN 1 — MAINTENANCE
Plan Path: [ ] A  [ ] B  [ ] C        (default recommendation: B)

Point  1 (Main partials)        Path: [ ] A  [ ] B  [ ] C
Point  2 (generated gates)      Path: [ ] A  [ ] B  [ ] C
Point  3 (numeric forms)        Path: [ ] A  [ ] B  [ ] C
Point  4 (missing defaults)     Path: [ ] A  [ ] B  [ ] C
Point  5 (naming conventions)   Path: [ ] A  [ ] B  [ ] C
Point  6 (test isolation)       Path: [ ] A  [ ] B  [ ] C
Point  7 (dead surface)         Path: [ ] A  [ ] B  [ ] C
Point  8 (diagnostics noise)    Path: [ ] A  [ ] B  [ ] C
Point  9 (map ownership)        Path: [ ] A  [ ] B  [ ] C
Point 10 (CI matrix)            Path: [ ] A  [ ] B  [ ] C
```

Unselected points follow the plan-level mapping in §0.2. The integrating
foreman records the final matrix in the package ledger row.

### 4.3 How the choices compose

Points 1–2 are structural and should usually share a path (both B or both C)
because the gate matrix is what keeps the partial layout honest after
extraction. Points 3–5 are the data family and compose in any order. Points
6–8 are test/diagnostic hygiene and are independent. Points 9–10 are the
"self-maintaining repository" layer and are the natural Path C finish.

If you pick **Plan Path A**, the work is about two days and touches almost
nothing structural: the two data defects, a documented partial-layout inventory,
and a written gate procedure.

If you pick **Plan Path C**, the work is a multi-week programme that leaves the
repository able to describe its own maintenance state: a gate matrix script, a
manifest-parity test for orchestration, a static-state isolation gate, and a
scheduled dead-surface sweep. Annex U records which blocked plans that releases.

---

## 5. Decision Point 1 — `Main` partial-class layout governance

### 5.1 Evidence and problem statement

`src/` holds 844 C# files, of which 148 are `Main*.cs` partials (e.g.,
`Main.UiPanels.cs` 1,700 lines, `Main.CampaignOwners.cs` 1,515,
`Main.World.cs` 944, `Main.Application.cs` 922, `Main.GameFlow.cs` 917,
`Main.PlayerSurfaces.cs` 864). The orchestration uses two bootstraps in
parallel:

1. A **direct call list** in `Main.CampaignServices.cs` (e.g., `SetupMedical();
   SetupMedicalWard(); SetupPhase0(); … SetupSilentFoundry(); …`), followed by
   `ExecuteSubsystemManifestBootstrap()` under the CF-P28 one-bootstrap rule.
2. A **declarative manifest** in which `RegisterManifestSetupActions()` registers
   18 delegates (`journal`, `needs`, `inventory`, `weather`, `radiation`,
   `radio`, `expeditions`, `duty_roster`, `crafting`, `research`, `medical`,
   `factions`, `economy`, `greenhouse`, `shelter_defense`, `vehicle_garage`,
   `black_market`, `memorial`) and `ExecuteSubsystemManifestBootstrap()` runs
   them by phase.

The maintenance risk is not the code itself; it is that a new subsystem can be
added to one path and not the other, and nobody notices until a fresh-game
vs. load-game divergence appears. The direct call list is hand-ordered; the
manifest is phase-ordered; the two orders are only coincidentally equal.

### 5.2 Path A — Patch & Preserve (document, don't move)

**Deliverable:** a maintained inventory document
`docs/architecture/MAIN_PARTIAL_INVENTORY.md` generated from the current tree
by a new read-only script `scripts/ci/generate-main-partial-inventory.py`,
listing each partial, its line count, its primary concern (parsed from a header
comment convention), and whether its `Setup*` methods appear in the direct list,
the manifest, or both.

**Rules:**

- No code moves. No renames. No `#region` shuffling.
- The script runs in `--check` mode as part of the standard verification
  command set for any package that adds a `Main.*.cs` file.
- New partials must carry a one-line concern header to be classified; an
  unclassified partial is a warning, not an error, in Path A.

**Effort:** ~0.5 day. **Risk:** negligible. **Value:** future agents stop
guessing where a concern lives; the fresh-vs-load divergence becomes visible.

**Verification:** run the generator twice; second run `--check` passes; the
inventory names every file returned by `find src -name "Main*.cs"`.

### 5.3 Path B — Strengthen & Standardize (bounded extraction + parity gate)

Everything in Path A, plus:

1. **Manifest parity test.** `MainTriadDriftGateTests` already exists (7/7 per
   the Plan 28 debt row). Extend the family with
   `MainBootstrapParityTests`: for every `Setup*` method invoked in the direct
   list, assert the subsystem appears in `SubsystemManifest` phase metadata
   (or carries an explicit `[DirectOnly("reason")]` attribute for
   intentionally non-manifest setups such as UI panel construction).
2. **Extract three to five cohesive groups** into new partials with unchanged
   bodies (mechanical moves only), chosen by cohesion of concern, e.g.:
   - `Main.SetupOrder.cs` — the ordered direct call list in one place, with a
     single `RunDirectSetupSequence()` method (no behaviour change; this is a
     cut-and-paste consolidation of the existing sequence).
   - `Main.ManifestDelegates.cs` — `RegisterManifestSetupActions()` and the
     delegate bodies it contains.
   - `Main.ResetPaths.cs` — `ResetPlansExpansionSessions`,
     `ResetEnrolledFlagshipSessions`, `ResetAllSessionsInMemory` side by side so
     reset hygiene is reviewable in one screen.
3. **Reset-coverage comment gate.** In `Main.ResetPaths.cs`, each reset method
   carries a comment block listing the fields it clears; the parity test asserts
   that every `Setup*`-created host field name appears in at least one reset
   list unless annotated `[ResetExempt("reason")]` (this is the maintenance-side
   half of the W2-02 D19c repair).

**Effort:** ~3–5 days. **Risk:** low–medium (mechanical moves can still perturb
Godot partial generation if a class loses `partial`; keep the keyword — see
`DEBT-GODOT-PARTIAL-REQUIRED`). **Value:** bootstrap drift becomes a failing
test instead of a runtime mystery.

**Verification:** host build 0/0; `MainTriadDriftGateTests` 7/7;
`MainBootstrapParityTests` new, green; `player_panels_uitest`;
`7day_smoke_selftest`; generated inventory `--check`.

### 5.4 Path C — Deep Rebuild (single manifest authority)

Everything in Path B, plus:

1. **Retire the handwritten direct call list as authority.** `RunDirectSetupSequence()`
   becomes manifest-driven: every direct setup converts to a manifest delegate
   with explicit phase and ordering metadata, and the direct sequence is
   deleted once parity is proven for a full release cycle.
2. **Phase-typed bootstrap.** The `LifecyclePhase` argument already exists in
   `ExecuteSubsystemManifestBootstrap(LifecyclePhase?)`; Path C assigns every
   subsystem a declared phase and makes the fresh-game path call the manifest
   exactly as the load path does, closing CF-P28's residual "two paths" risk
   permanently.
3. **Orchestration module split.** The `Main` partial set is regrouped into
   declared modules (`Boot`, `Save`, `World`, `Survivors`, `Facilities`,
   `Economy`, `Narrative`, `Ui`, `Diagnostics`) with a generated dependency
   listing. No behavioural change.

**Effort:** ~2–4 weeks. **Risk:** medium–high (orchestration order is
load-bearing; every move must be validated on fresh + load + replay paths).
**Value:** the single largest long-term maintainability win available in this
repository; it is also the prerequisite for any future engine/version migration.

**Verification:** full fresh/load/replay trilogy twice with fingerprint
equality; all generated gates; a dedicated `main-orchestration-parity` selftest
registered in `SELFTEST_MANIFEST.json` with its CLI catalog row.

### 5.5 Cross-path note

Path C's module split is the only option here with real regression surface. The
plan recommends Path B and treats C as a separate, explicitly signed follow-up
package rather than a same-package continuation. This keeps faith with the
repository rule that one package owns one concern.

---

## 6. Decision Point 2 — Generated-artifact gate discipline

### 6.1 Evidence and problem statement

The `--check` family is strong but distributed. A package that edits a
catalog, a panel, a save section, or a test file touches between one and eight
generated artifacts (`docs/INDEX.md`, `docs/data/CATALOG_REGISTRY.md`,
`docs/architecture/ARCHITECTURE_TEST_MAP.md`, `docs/ui/UI_PANEL_ARCHITECTURE_GUIDE.md`,
`docs/player_surface_manifest.json`, `docs/ci/SELFTEST_MANIFEST.json`,
`docs/cli/HOST_CLI_COMMAND_CATALOG.md`, `docs/architecture/…`). The wave-1
program proved the gates work when run; the failure mode is **not running
them**, which produces the drift classes UNBLOCK-04 documented.

### 6.2 Path A — Documented manual procedure

- Add `docs/ci/GENERATED_ARTIFACT_MATRIX.md`: rows = artifact, owner script,
  `--check` command, triggering inputs, and the packages that last touched it.
- The matrix is hand-maintained; the standard verify command block in
  `TEST_POLICY.md` names it.
- No script changes.

**Effort:** ~0.5 day. **Risk:** none. **Value:** an agent that reads the matrix
knows exactly what to regenerate.

### 6.3 Path B — Enforced gate matrix in one script

- Add `scripts/ci/verify-generated-artifacts.sh` that runs every generator's
  `--check` in a fixed order, collects failures, prints a single summary, and
  exits non-zero on any drift.
- Wire it into the fast verification path (`agent-fast-verify.py`) so the
  standard package verification cannot silently skip it.
- The matrix doc from Path A is generated from the script's metadata (single
  source of truth).

**Effort:** ~1 day. **Risk:** low; the only work is making each generator
accept a uniform `--check` invocation (several already do).
**Value:** drift becomes a single-command failure with a named artifact.

### 6.4 Path C — Regenerate-and-check harness with CI wiring

Everything in Path B, plus:

1. A `--regen` mode that regenerates every artifact and then runs `--check`,
   producing a unified diff report for drift triage.
2. A CI workflow job (`generated-artifact-gate`) that runs on every PR and
   annotates the drift file names.
3. A **stale-artifact detector**: compares each artifact's newest input mtime
   against the artifact mtime and warns when the artifact predates its inputs.

**Effort:** ~3–5 days. **Risk:** low–medium (CI workflow maintenance).
**Value:** the repository cannot silently accumulate artifact drift again; this
is the maintenance-domain version of "one authority per concern".

### 6.5 Verification (all paths)

```bash
bash scripts/ci/verify-generated-artifacts.sh          # Path B/C
python3 scripts/ci/generate-docs-index.py --check      # spot-check
python3 scripts/ci/generate-architecture-map.py --check
python3 scripts/ci/generate-catalog-registry.py --check
bash scripts/ci/generate-cli-catalog.sh --check
```

Expected: all PASS on an untouched tree; a deliberate one-line doc edit causes
the docs-index gate to fail exactly once and pass after regeneration.

---

## 7. Decision Point 3 — Data numeric-form normalization (`dangerLevel`)

### 7.1 Evidence and problem statement

`locations.json` authors the same field as both integer (`3`) and float (`8.0`)
literals (distribution in §2.3). The loader is tolerant, so this is a **latent**
hygiene defect: diffs are noisy, schema validators that type-check numeric
literals may disagree, and a future migration tool that assumes one form will
misread the other.

### 7.2 Path A — Normalize the literals only

- Rewrite `dangerLevel` (and any other detected mixed-form numeric field) to a
  **single canonical form**: integers for whole numbers where the current
  loader binds an `int`/`float`, preserving value exactly.
- Prove equivalence: a before/after script asserts every location's parsed
  numeric value is bit-identical.
- No schema change, no loader change, no version bump.

**Effort:** ~2 hours including equivalence proof. **Risk:** none (value-preserving).
**Value:** removes a real data-truth seam with a verifiable one-line claim.

### 7.3 Path B — Codify numeric authoring rules + validator

Everything in Path A, plus:

- A data-style rule in the catalog authoring guide: "whole numbers are authored
  as integers unless the field's declared type is floating; mixed forms are
  invalid."
- A validator rule (in the existing catalog-integrity pipeline) that rejects a
  field whose JSON tokens mix `Integer` and `Float` forms **if** the loader
  binds a uniform type; the rule's allowlist is generated from loader metadata.
- Re-scan all 338 catalogs for other mixed-form fields and fix the proven ones.

**Effort:** ~1–2 days. **Risk:** low; rule must not fire on legitimately
mixed-value fields (e.g., a field where 0.5 and 3 coexist — that is not a form
mix; the rule is about token-form consistency across rows for the same field).
**Value:** the class of defect cannot return unnoticed.

### 7.4 Path C — Loader-schema unification pass

Everything in Path B, plus a repo-wide audit mapping every numeric JSON field to
its C# binding type, with a generated report
(`docs/data/NUMERIC_BINDING_REPORT.md`). Where a field is bound as `float` but
authored only as integers, the report records whether the authority intends
integers (then the binding narrows or the rule permits integers) or floats
(then authoring normalizes). This is the foundation for any future typed-data
pipeline.

**Effort:** ~1 week. **Risk:** low (read-only report first; fixes only for
proven cases). **Value:** numeric truth across the whole data authority.

### 7.5 Verification

```bash
python3 - <<'EOF'
import json
d=json.load(open('Assets/StreamingAssets/Data/locations.json'))
vals=[l['dangerLevel'] for l in d['locations']]
print('count', len(vals), 'min', min(vals), 'max', max(vals))
EOF
```

Before/after equivalence script must print the identical multiset of parsed
values.

---

## 8. Decision Point 4 — Missing-field defaults

### 8.1 Evidence and problem statement

`travelHours`: 171/179; `baseRadsPerHour`: 178/179; `requiredFlagId` and
`cleanWaterRewardFlag` and `ambushFlag`: 1 each. The loader tolerates absences
(`ExpeditionJsonDto.travelHours` defaults to 0). A zero travel time is
gameplay-semantically different from "authored as unset", and a reader cannot
tell which is intended.

### 8.2 Path A — Fill the eight gaps with documented defaults

- For the eight rows without `travelHours`, author a default derived from the
  file's own median for their danger band (documented in the commit message),
  or explicitly `0` with a comment where immediate travel is intended.
- For the one row without `baseRadsPerHour`, author `0` only if the location is
  canonically clean; otherwise derive from the danger band.
- Record the derivation rule in the catalog header as a JSON comment field if
  the schema permits, else in the authoring guide.

**Effort:** ~1 hour. **Risk:** low (values are additive data).
**Verification:** a script prints the count of rows missing each field → 0.

### 8.3 Path B — Default policy + audit report

Everything in Path A, plus:

- A written default policy per field ("absent `travelHours` = use danger-band
  median at load time" vs. "absent = authoring error"), implemented either in
  the loader (with a warning) or enforced by the validator.
- `docs/data/LOCATION_FIELD_COVERAGE.md` generated by a script showing per-file
  field coverage so gaps are visible before they matter.

**Effort:** ~1 day. **Risk:** low. **Value:** absent-vs-zero becomes a decided
contract rather than an accident.

### 8.4 Path C — Full required/optional schema declaration

Everything in Path B, plus a declarative per-catalog schema file naming every
field as required/optional/defaulted, validated by the integrity pipeline and
generated into the catalog registry. This is the same machinery any future mod
support or save-migration tooling would want.

**Effort:** ~1–2 weeks across the catalog families most at risk (locations,
weather, economy). **Risk:** low–medium (schema breadth). **Value:** data truth
becomes machine-checkable per catalog, not per convention.

### 8.5 Verification

```bash
python3 - <<'EOF'
import json, collections
d=json.load(open('Assets/StreamingAssets/Data/locations.json'))
for f in ('travelHours','baseRadsPerHour','dangerLevel','displayName','id'):
    missing=sum(1 for l in d['locations'] if f not in l)
    print(f, 'missing', missing)
EOF
```

---

## 9. Decision Point 5 — Catalog naming-convention codification

### 9.1 Evidence and problem statement

All 338 catalogs parse and carry `schema_version`. Conventions vary: some
catalogs are snake_case authorities (`difficulty_presets.json` uses
`display_name`, `scalars`), others are camelCase DTO surfaces
(`WastelandMapCatalogLoader` binds `displayName`, `positionX` with
`JsonPropertyName`), and the loader-side scan counted roughly 23,000 camelCase
key occurrences repo-wide. A blanket "normalize everything" is wrong: it would
break DTOs that legitimately mirror C# property names.

### 9.2 Path A — Document the split, change nothing

- Write the observed convention per catalog family into
  `docs/data/CATALOG_CONVENTIONS.md`: which catalogs are snake_case-authored,
  which are camelCase-DTO-bound, and why (usually: loader DTO vs. external
  authority style).
- No data edits.

**Effort:** ~0.5 day. **Risk:** none. **Value:** stops future agents from
starting a normalization pass that would break things.

### 9.3 Path B — Per-family codification + guard on new files

Everything in Path A, plus:

- For each family, declare the canonical convention in the coding guide.
- A **new-file gate**: any newly added catalog must declare a `schema_version`
  and either follow its family's convention or document an exception in the
  catalog header.
- Existing files are not rewritten (that would be Path C).

**Effort:** ~1–2 days. **Risk:** low. **Value:** the convention cannot drift
further while legacy stays intact.

### 9.4 Path C — Controlled per-family normalization

Everything in Path B, plus a migration per family where the benefit is proven:
for each family, either (a) normalize JSON keys and delete the DTO aliases with
a loader change, or (b) normalize the DTOs and JSON together — never one side
alone. Each family is its own bounded tranche with before/after parsed-object
equivalence tests.

**Effort:** ~1–2 weeks. **Risk:** medium (loader and data must move together).
**Value:** one convention, machine-checked, with no dual-truth DTO seams.

### 9.5 Verification

```bash
grep -c '"schema_version"' Assets/StreamingAssets/Data/*.json | awk -F: '$2==0'
# expect no output
python3 scripts/ci/generate-catalog-registry.py --check
bash scripts/run_test.sh Ashfall.Core.Tests/Data   # any data-contract suite
```

---

*(End of part A — continues with Decision Points 6–10, execution phases,
verification, risks, ownership, rollback, DoD, Annex U, appendices.)*---

## 10. Decision Point 6 — Test-quarantine and static-state isolation

### 10.1 Evidence and problem statement

- 1,144 test files; the historical quarantine is reconciled
  (`DEBT-TEST-QUARANTINE-2026-09-12`), and `QuarantineManifestGateTests`
  enforces file-real `Compile Remove` entries.
- A `TradeSpecialtySystem` static-state test-isolation flake existed and was
  fixed test-side in D1 (2026-09-17). The class of defect remains: **static
  mutable state in Core** can leak between test cases, producing order-dependent
  results.
- Static scan: 61 `private/internal static … = new` initializations in
  `Assets/Ashfall.Core` (boss fields such as catalogs and registries are often
  intentionally static; the risk is mutable *instance-lifetime* state, not
  immutable catalog caches).

### 10.2 Path A — State the quarantine truth and measure isolation

- Update `AGENTS.md`'s stale "48 active" quarantine line to the current truth
  (shared with UNBLOCK-04; if that already landed, cite it).
- Run a **test-order shuffle probe** on the three suites with known static
  surfaces (`UtilityAi`, `TradeSpecialty`, `Holdfast*`) using the existing
  runner, and record whether order changes outcomes. Read-only.

**Effort:** ~0.5 day. **Risk:** none. **Value:** the flake class is measured
before anyone tries to fix it.

### 10.3 Path B — Static-state isolation gate

- Write `scripts/ci/static-state-isolation-gate.py`: identifies Core classes
  with mutable static fields (excluding `readonly`, `const`, and catalog
  caches marked `[CatalogCache]`), and requires each to either (a) expose a
  test-visible `ResetForTests()`/`ClearCacheForTests()` hook, or (b) carry a
  `[StaticStateSafe("reason")]` annotation (e.g., immutable after first load).
- Add a test fixture helper that resets all annotated hooks automatically
  between cases in a dedicated fixture base, opt-in per suite.
- Fix any suite the gate flags by adding the hook (test-side change only; if
  production needs a reset method, it is a new public API and must be reviewed
  as such).

**Effort:** ~2–3 days. **Risk:** low–medium (touching many classes with an
annotation is noisy; keep the gate to *mutable* statics and review the list
first). **Value:** order-dependent flakiness becomes structurally impossible
for the flagged set.

### 10.4 Path C — Architecture-map ownership enforcement

Everything in Path B, plus:

- Extend `ARCHITECTURE_TEST_MAP` generation so every Core system is mapped to
  its owning focused test file(s); a new Core class without a mapped suite is
  a gate failure unless annotated `[TestExempt]`.
- A `--focused-verify <path>` helper that reads the map and runs exactly the
  owning suites for a changed path — the practical realisation of
  `TEST_POLICY.md`'s "smallest test file or directly affected region".

**Effort:** ~1–2 weeks (map completeness is the bulk; the generator exists).
**Risk:** medium (the map will initially trip on exempt classes; stage it).
**Value:** focused verification becomes a command, not a judgement call —
directly reducing the full-suite temptation the policy warns against.

### 10.5 Verification

```bash
# shuffle probe (Path A)
bash scripts/run_test.sh Ashfall.Core.Tests/UtilityAI --shuffle 2>/dev/null || true
# gate (Path B)
python3 scripts/ci/static-state-isolation-gate.py --check
# mapping (Path C)
python3 scripts/ci/generate-architecture-map.py --check
# focused verify (Path C)
bash scripts/ci/focused-verify.sh Assets/Ashfall.Core/Survivors/NeedsSystem.cs
```

Expected: gate reports zero unannotated mutable statics in Core; map `--check`
passes; focused-verify prints the owning suite and runs it alone first.

---

## 11. Decision Point 7 — Dead and zero-consumer surface reduction

### 11.1 Evidence and problem statement

Three categories exist:

1. **Historical plan trees** under `docs/plans/` (hundreds of files) that are
   preserved by `DEBT-PLAN-SPRAWL` (ACCEPTED) but create false premises for
   agents who treat old plans as current authority (UNBLOCK-04's own drift
   analysis covers this).
2. **Ghost references** — the quarantine history proved `Compile Remove`
   entries can reference absent files; the gate now prevents it, but similar
   phantom references can exist in docs, scripts, and test fixtures.
3. **Zero-consumer code** — the D2 precedent deleted
   `SurvivorInspectionHostSession` after proving zero live consumers. Similar
   candidates exist wherever a projection was built for a consumer that was
   never wired (the "content is not gameplay reachability" failure class).

### 11.2 Path A — Report only

- Generate `docs/maintenance/ZERO_CONSUMER_CANDIDATES.md`: for each Core public
  class not referenced by `src/` or tests, list it with a zero-consumer
  verdict and a suggested disposition (delete, annotate, wire, archive).
- Do not delete anything.

**Effort:** ~0.5–1 day (script + triage). **Risk:** none. **Value:** the
candidate list is the prerequisite for any bounded deletion.

### 11.3 Path B — Delete proven-zero-consumer sets

Everything in Path A, plus bounded deletion tranches:

- For each candidate, re-prove zero live consumers (`grep` across `src`,
  `Assets/Ashfall.Core.Tests`, and generated catalogs; verify no reflection
  registry names it), then delete the source and its direct fixture in one
  commit.
- The D2 precedent is the template: re-prove, delete, run the owning regional
  suite, update the architecture map and any generated catalog in the same
  package.
- Ghost-reference cleanup: fix or delete docs/scripts that name deleted paths,
  with `doc-link-gate.sh` as the check.

**Effort:** ~3–5 days for the initial proven set. **Risk:** low–medium (deleting
a class that is only used by a commented-out path). **Value:** the codebase
stops carrying dead weight that makes every inventory tool lie.

### 11.4 Path C — Consolidated archive program

Everything in Path B, plus:

- Move preserved historical plan trees into a single archive root with a
  manifest (`docs/archive/plans/<year>/INDEX.md`) and a repo rule that only the
  archive is historical; `docs/plans/` holds live plans only.
- A `doc-authority-gate`: any file in `docs/plans/` must carry a status banner
  (PROPOSAL / ACTIVE / DONE / SUPERSEDED); files without one fail the gate.
- A scheduled (quarterly) dead-surface sweep entry in the maintenance calendar.

**Effort:** ~1–2 weeks. **Risk:** low (docs only, but large diffs; do it in
tranches). **Value:** "live plan" and "historical plan" become structurally
distinct, which is the single best protection against plan-sprawl false
premises.

### 11.5 Verification

```bash
python3 scripts/ci/dead-surface-scan.py            # or doc authoring equivalent
bash scripts/ci/doc-link-gate.sh
bash scripts/run_test.sh Ashfall.Core.Tests/<owning regional suite>
python3 scripts/ci/generate-architecture-map.py --check
```

---

## 12. Decision Point 8 — Diagnostics and log-noise boundary

### 12.1 Evidence and problem statement

- Bare `catch {}` is effectively absent (one comment reference).
- `catch (Exception` appears 619 times across Core and host — most are
  legitimate (logged, converted, or rethrown), but the volume makes it hard to
  see the *swallowed* ones.
- A `catch-policy-gate.sh` already exists; its current enforcement strength is
  the open question.
- Host diagnostics use `GD.Print`/`GD.PushWarning`/`GD.PrintErr` liberally; log
  noise in a 15 FPS headless session can hide real warnings.

### 12.2 Path A — Measure only

- Run `catch-policy-gate.sh` and record its current verdict.
- Sample the `catch (Exception` sites and classify: logged, converted,
  swallowed, or rethrown. Produce a count table; change nothing.

**Effort:** ~0.5 day. **Risk:** none. **Value:** the real swallow count, which
nobody currently knows.

### 12.3 Path B — Strengthen the catch-policy gate

- Extend `catch-policy-gate.sh`: any `catch` block that neither logs, nor
  rethrows, nor returns a typed failure must carry a
  `// INTENTIONAL-IGNORE: <reason>` comment; the gate fails otherwise.
- This matches the repository's existing philosophy (explicit reasons for
  deferred seams) and makes silence impossible.
- Fix the proven swallows found in Path A with minimal edits (add logging or a
  typed failure where the caller currently cannot distinguish success).

**Effort:** ~2–3 days. **Risk:** low–medium (annotation churn; stage the gate
with a baseline allowlist that shrinks per tranche). **Value:** silent failure
becomes a reviewed decision, not an accident — this is the maintenance-side
half of W2-02's silent-failure program.

### 12.4 Path C — Structured diagnostics boundary

Everything in Path B, plus:

- A thin `Diagnostics` facade in Core (engine-free) with levels and categories,
  so host and Core share one vocabulary; `GD.Print*` calls in host code route
  through it for campaign-relevant events.
- A log-noise budget check in the smoke selftest: count warnings/errors emitted
  during `7day_smoke_selftest`; a jump beyond a recorded baseline fails the
  check (this catches accidental per-frame spam).
- Error-code taxonomy for typed failures, referenced by tests.

**Effort:** ~1–2 weeks. **Risk:** medium (touching many call sites; do it
facade-first, migrate gradually). **Value:** observability without noise; the
baseline makes regressions visible.

### 12.5 Verification

```bash
bash scripts/ci/catch-policy-gate.sh                # must PASS
bash scripts/run_test.sh Ashfall.Core.Tests/IO
godot --headless --path . -- --7day-smoke-selftest  # noise count vs baseline
```

---

## 13. Decision Point 9 — Architecture-map and registry ownership

### 13.1 Evidence and problem statement

Generated docs (`ARCHITECTURE_TEST_MAP.md` 249 KB, `docs/INDEX.md` 718 KB,
catalog registry, UI panel guide, CLI catalog, player-surface manifest,
selftest manifest) are owned by generators but **consumed by humans and gates**.
When a package updates them by hand or forgets them, the consumers silently
read stale truth (UNBLOCK-04's seven drift classes).

### 13.2 Path A — Ownership labels

- Add a header comment to each generated artifact naming its generator and
  `--check` command; add the same to the doc index generation.
- Read-only; no tooling change.

**Effort:** ~0.5 day. **Risk:** none. **Value:** any reader immediately knows
the artifact is generated and how to verify it.

### 13.3 Path B — Ownership registry and orphan check

Everything in Path A, plus a `docs/ci/GENERATED_ARTIFACT_MATRIX.md` (from
Decision Point 2) extended with an **orphan check**: every generator listed in
`scripts/ci/` must produce a declared artifact, and every generated artifact
must name a generator. An unpaired generator or artifact fails the check.

**Effort:** ~1 day. **Risk:** low. **Value:** no generator or artifact can
exist unowned; the maintenance contract becomes checkable.

### 13.4 Path C — Runtime-consumer registry for panels/CLI/selftests

Everything in Path B, plus a generated **consumer registry**: for every panel,
CLI command, and selftest, the registry names the Core owner it presents and
the test that covers it, enforced by a gate. This is the structural answer to
the recurring "panel exposes a fake route" class and directly supports the
production-UI purity rules.

**Effort:** ~1–2 weeks. **Risk:** medium (registry completeness).
**Value:** every player-facing surface has a machine-checked owner and test —
the strongest available protection against unreachable or fabricated UI.

### 13.5 Verification

```bash
python3 scripts/ci/generate-architecture-map.py --check
python3 scripts/ci/generate-catalog-registry.py --check
bash scripts/ci/generate-cli-catalog.sh --check
python3 scripts/ci/generate-ui-catalog.py --check 2>/dev/null || true
```

---

## 14. Decision Point 10 — CI gate matrix and maintenance contract

### 14.1 Evidence and problem statement

There is no single document that says "these are the maintenance gates, this is
what each protects, this is when each runs, and this is the fast path versus
the full path". Knowledge is distributed across `scripts/ci/`, `TEST_POLICY.md`,
`AGENTS.md`, and package evidence sections. The repository's own history shows
what happens when a gate passes while another silently fails (the
`DEBT-ARCH-MAP-GENERATOR-DRIFT` row: "the map-text gate had passed while the
generator `--check` failed — a CI-truth gap").

### 14.2 Path A — The matrix document

- Write `docs/ci/GATE_MATRIX.md`: gate, command, trigger, owned artifacts,
  failure meanings, fast/full classification.
- No automation.

**Effort:** ~0.5 day. **Risk:** none. **Value:** the maintenance contract is
readable.

### 14.3 Path B — Fast gate runner

Everything in Path A, plus `scripts/ci/fast-gates.sh` running the
fast-class gates in fixed order with a summary and exit code; referenced from
`TEST_POLICY.md` and used as the default pre-handoff command for maintenance
packages.

**Effort:** ~1 day. **Risk:** low. **Value:** one command proves the maintenance
contract; matches `agent-fast-verify.py` philosophy.

### 14.4 Path C — Maintenance contract with scheduled sweeps

Everything in Path B, plus:

- A **maintenance calendar** (`docs/ci/MAINTENANCE_CALENDAR.md`) with the
  recurring sweeps (dead surface, artifact staleness, static-state audit,
  log-noise baseline, catalog-coverage) and the owning role.
- A **gate-effectiveness review**: for each gate, the last time it caught a real
  defect; gates that have never fired are reviewed (false confidence is a debt
  of its own).
- A repository "maintenance health" report generated from all of the above.

**Effort:** ~3–5 days initial, then recurring. **Risk:** low. **Value:** the
repository maintains itself on a schedule instead of relying on the next agent
to notice.

### 14.5 Verification

```bash
bash scripts/ci/fast-gates.sh          # Path B/C
cat docs/ci/GATE_MATRIX.md | head -40  # Path A+
```

---

## 15. Execution phases (path-composed)

The phases below are written so that each point's chosen path slots in. A
package implements one phase, verifies it, and hands off — never two phases in
one claim (repository rule).

### Phase M0 — Reconnaissance freeze (all paths, ~0.5 day)

- Record HEAD sha, run the fast gates, inventory `Main*.cs`, run the data
  scripts from §7.5/§8.5, run the orphan/dead-surface scan.
- Produce `P0_MAINTENANCE_PREMISE.md` with the exact command output.
- This phase is mandatory even at Plan Path A: it makes the later claims
  checkable.

### Phase M1 — Data truth (Points 3 + 4; any path, 0.5–2 days)

- Apply the chosen numeric-form path and missing-field path.
- Equivalence proof: parsed values unchanged (numeric) + coverage counts 0
  missing (fields).
- Run `generate-catalog-registry.py --check`, data-contract suites, and the
  data-integrity selftest.

### Phase M2 — Generated-artifact discipline (Point 2; any path, 0.5–5 days)

- Implement the chosen artifact-gate path; run the full `--check` family once
  before and once after; attach both outputs to the package evidence.

### Phase M3 — Orchestration governance (Point 1; path-dependent, 0.5–4 weeks)

- Path A: inventory generator + doc.
- Path B: parity test + bounded extraction + reset-coverage comments.
- Path C: manifest-authority conversion + module split (separate signed
  package recommended).

### Phase M4 — Test hygiene (Point 6; path-dependent, 0.5–2 weeks)

- Path A: shuffle probe + ledger truth.
- Path B: isolation gate + hooks.
- Path C: ownership mapping + focused-verify helper.

### Phase M5 — Dead surface (Point 7; path-dependent, 0.5–2 weeks)

- Path A: candidates report. Path B: bounded deletions. Path C: archive
  program + doc-authority gate.

### Phase M6 — Diagnostics and registry ownership (Points 8 + 9; path-dependent, 0.5–2 weeks)

- Catch-policy strengthening, log-noise baseline, ownership labels/registry.

### Phase M7 — Maintenance contract (Point 10; path-dependent, 0.5–5 days)

- Matrix, fast-gates runner, calendar, effectiveness review.

### Phase M8 — Closeout (all paths, 0.5 day)

- Evidence pack: commands, results, files, limitations, untouched shared paths.
- Ledger/debt updates by the integrator only.
- Handoff per `AI_AGENT_WORKFLOW.md`.

### 15.1 Ordering constraints

- M1 before M2 (data fixes will change generated catalogs).
- M0 always first.
- M3-B/C before M6-C if the registry is to be complete.
- M7 last (it documents what the other phases proved).
- Any phase that finds its premise false stops and reports; it does not
  improvise (Rule 10).

---

## 16. Verification plan

### 16.1 The five-truth verification recipe

| Truth | Command | Pass condition |
|---|---|---|
| Build | `dotnet build Ashfall.csproj --no-restore` | 0 errors |
| Gates | `bash scripts/ci/fast-gates.sh` (or the individual `--check`s) | all PASS |
| Tests | `bash scripts/run_test.sh <owning suites>` | all PASS, bounded time |
| Data | data-integrity + content-utilization selftests | PASS |
| Ledger | package row + evidence pointers | present and current |

### 16.2 Per-point verification summary

| Point | Focused evidence |
|---|---|
| 1 | partial inventory `--check`; parity tests (B/C); triad drift 7/7; smoke |
| 2 | every generator `--check`; one deliberate-drift negative test |
| 3 | parsed numeric multiset before == after |
| 4 | missing-field counts all zero; loader default documented |
| 5 | conventions doc + new-file gate; catalog registry `--check` |
| 6 | shuffle probe; isolation gate `--check`; focused-verify |
| 7 | dead-surface scan; regional suite for each deletion; doc-link gate |
| 8 | catch-policy gate; log-noise baseline from smoke |
| 9 | artifact matrix orphan check; consumer registry gate (C) |
| 10 | fast-gates exit 0; matrix completeness; calendar rows |

### 16.3 Negative tests (prove the gates bite)

For each new gate, the package must include one **deliberate violation**
demonstration: make the smallest possible drift, show the gate fails, revert,
show it passes. A gate that has never failed is a hypothesis, not a gate.

---

## 17. Risks and mitigations

| # | Risk | L | I | Mitigation |
|---|---|---|---|---|
| 1 | Partial extraction perturbs Godot script binding | M | H | never remove `partial`; scene-attach smoke after each move |
| 2 | Generated-artifact gate blocks urgent work on unrelated drift | M | M | fast-gates summary names artifacts; regeneration is cheap |
| 3 | Numeric normalization changes parsed values | L | H | mandatory equivalence script before commit |
| 4 | Naming-normalization breaks a loader | M | H | Path C only per family with both sides moving together |
| 5 | Static-state gate churn | M | M | gate limited to mutable statics; staged baseline |
| 6 | Dead-code deletion removes a reflection-registered class | L | H | zero-consumer proof includes registry scans |
| 7 | Catch-policy annotations become noise | M | L | blank annotation forbidden; gate checks a reason string |
| 8 | Maintenance scope creeps into gameplay | M | H | Wave 2 rule; moves go to the owning plan |
| 9 | A phase discovers a real defect mid-maintenance | M | M | stop; hand to W2-02; do not fix silently here |
| 10 | Large doc moves flood review | M | L | tranches with a manifest; archive only under Path C |
| 11 | The "clean" tree hides worktree dirt | L | M | M0 records git status; preserve unrelated changes |
| 12 | Orphan gate false-positives on intentional unowned scripts | M | L | allowlist with reasons, reviewed per tranche |

---

## 18. Ownership and claims

### 18.1 Proposed claim blocks (integration time)

| Phase | Claim name | Paths (indicative) |
|---|---|---|
| M0 | `W2-01-M0-PREMISE` | `docs/plans/wave2_integration/P0_MAINTENANCE_PREMISE.md` |
| M1 | `W2-01-M1-DATA-TRUTH` | `Assets/StreamingAssets/Data/locations.json` (+ any proven mixed-form catalogs), `docs/data/CATALOG_CONVENTIONS.md`, data tests |
| M2 | `W2-01-M2-ARTIFACT-GATES` | `scripts/ci/verify-generated-artifacts.sh`, `docs/ci/GENERATED_ARTIFACT_MATRIX.md`, generated docs via their generators |
| M3 | `W2-01-M3-ORCH-GOVERNANCE` | `scripts/ci/generate-main-partial-inventory.py`, `docs/architecture/MAIN_PARTIAL_INVENTORY.md`, `src/Main.*.cs` (bounded), parity tests |
| M4 | `W2-01-M4-TEST-HYGIENE` | `scripts/ci/static-state-isolation-gate.py`, test fixtures, `docs/architecture/ARCHITECTURE_TEST_MAP.md` via generator |
| M5 | `W2-01-M5-DEAD-SURFACE` | `docs/maintenance/`, deletions (proven set), archive moves under Path C |
| M6 | `W2-01-M6-DIAGNOSTICS-REGISTRY` | `scripts/ci/catch-policy-gate.sh`, diagnostics facade (C), registry docs |
| M7 | `W2-01-M7-CONTRACT` | `docs/ci/GATE_MATRIX.md`, `scripts/ci/fast-gates.sh`, calendar |
| M8 | `W2-01-M8-CLOSEOUT` | evidence docs, ledgers (integrator) |

Claims are made **at integration time**, not now; this document claims
nothing. Each claim is path-disjoint from the others and from the other five
Wave 2 plans. Shared/generated roots (docs registries, architecture map) are
regenerated by the owning generator inside whichever claim needs them; the
generator is never hand-edited.

### 18.2 Interaction with other Wave 2 plans

| Plan | Interface with W2-01 |
|---|---|
| W2-02 (bugs) | consumes M3-B reset-coverage comments and M8 diagnostics; owns the D19c repair itself |
| W2-03 (gameplay) | consumes M1 data truth; must not be blocked by gate strengthening |
| W2-04 (environments) | weather/hazard catalogs pass M1/M2; environment content stays there |
| W2-05 (locations) | depends on M1 (`locations.json` numeric/coverage truth) before authoring tiers |
| W2-06 (enrichment) | consumes M5 archive rules and M6 diagnostics for prose-tooling noise |

---

## 19. Rollback and decline paths

| Point | Rollback | Decline consequence |
|---|---|---|
| 1 A/B | revert moves (files are mechanical) | orchestration drift risk remains documented |
| 1 C | keep the parity test, revert the split | structural debt stays |
| 2 A/B/C | delete the script/matrix; gates are additive | drift risk returns |
| 3 | revert the data file; values were identical | latent diff noise stays |
| 4 | revert authored defaults | absent-vs-zero ambiguity stays |
| 5 | keep the doc, drop the gate | convention drift can resume |
| 6 | drop the gate, keep the probe | flake class unmeasured |
| 7 | restore from git/Twin archive; never delete the archive | dead surface grows |
| 8 | revert gate to report-only | swallows unmeasured |
| 9 | keep labels, drop registry | ownership implicit again |
| 10 | keep the matrix doc | contract unenforced |

Declining a point is legitimate; the plan records the consequence so the next
audit does not rediscover it as a surprise.

---

## 20. Definition of done and handoff

### 20.1 DoD per path

- **Path A done** when: M0 evidence exists, the two data defects are fixed with
  equivalence proof, the artifact matrix doc exists, and the inventory doc is
  generated.
- **Path B done** when: all of A, plus parity/isolation/catch gates exist with
  negative-test proof, bounded extraction is complete, and the fast-gate runner
  passes on the ready tree.
- **Path C done** when: all of B, plus manifest-authority conversion (or a
  signed separate package for it), the owner registry gates pass, the archive
  program is in place, and the maintenance calendar is published.

### 20.2 Handoff fields (per phase)

1. Outcome in one paragraph.
2. Files changed (exact paths).
3. Contract: what guarantee the phase adds.
4. Commands run + results.
5. Limitations and false-premise findings.
6. Shared paths intentionally untouched.
7. Proposed ledger/debt edits (integrator applies).

### 20.3 First safe step

> Phase M0 only. It is read-only, produces the premise file, and no other
> phase starts until the chooser returns the selection sheet from §4.2.

---

## 21. Worked scenarios

### Scenario 1 — Plan Path A, two days

1. M0 records evidence.
2. M1 normalizes `dangerLevel` forms (Path A) and fills the eight
   `travelHours` gaps (Path A) with equivalence proof.
3. M2 writes the artifact matrix doc; runs every `--check` once and records it.
4. M3-A generates the partial inventory.
5. Closeout. Outcome: data-truth seams closed and maintenance knowledge
   indexed, with zero structural risk.

### Scenario 2 — Plan Path B, the default

1. M0 → M1 (A) → M2 (B: one verify script wired into fast verify).
2. M3-B: `Main.SetupOrder.cs`, `Main.ManifestDelegates.cs`,
   `Main.ResetPaths.cs` extracted; `MainBootstrapParityTests` added; triad and
   smoke green.
3. M4-B: isolation gate + reset hooks; shuffle probe clean.
4. M5-A (report) with one bounded deletion tranche if the candidate list is
   short.
5. M6: catch-policy gate strengthened; log-noise baseline recorded.
6. M7: fast-gates runner.
7. Closeout. Outcome: maintenance drift is now caught by commands, not by
   reviewers.

### Scenario 3 — Plan Path C, structural

1. As Path B through M4.
2. M3-C: manifest becomes sole authority; direct call list retired after a
   full cycle of parity; module split proposed as its own signed follow-up.
3. M5-C: archive program + doc-authority gate.
4. M6-C: diagnostics facade + log-noise budget.
5. M7-C: calendar + effectiveness review.
6. Closeout with a maintenance health report. Outcome: the repository can
   describe and enforce its own upkeep.

### Scenario 4 — A chooser mixes paths

Plan Path B, but Point 3 at C (numeric binding report is wanted early), Point 7
at A (no deletions this cycle), Point 10 at A (matrix doc only). The phases
recompose accordingly: M1 uses the Path C report, M5 stops at the candidate
report, M7 writes the doc. Nothing else changes.

---

## 22. Foreman Q&A

**Q1. Why is this a wave of its own instead of part of UNBLOCK-04?**
UNBLOCK-04 restores ledger truth; this plan maintains the *artifacts and code
structure* those ledgers describe. They compose but do not overlap: W2-01 is
the only plan that touches generator gates and orchestration layout.

**Q2. Is Path C's module split safe?**
Not risk-free; that is why it is recommended as a separate signed package and
why every earlier path exists. The parity test from Path B is the safety net
that makes C reasonable at all.

**Q3. Why normalize `dangerLevel` if nothing is broken?**
Because it is a data-authority seam that will break exactly once — during a
future schema migration or a diff-based tool — and the fix costs two hours now.
Truth-grade means removing latent ambiguity, not only fixing symptoms.

**Q4. Won't the catch-policy annotations be noisy?**
Only if applied to all 619 sites blindly. Path B classifies first (Path A) and
annotates only the genuinely silent ones; the gate starts with a shrinking
baseline allowlist.

**Q5. What about the 61 statics — are they all dangerous?**
No. Most are catalog caches that are immutable after load. The gate
distinguishes mutable state from caches; only mutable state needs a reset hook.

**Q6. Does deleting dead code risk the archive?**
The archive survives; deletion candidates are source files with zero consumers,
not archived historical documents. Path C's archive program is what preserves
plan history deliberately instead of by accident.

**Q7. Can a builder do M1 and M2 in one claim?**
The repository prefers one phase per claim. M1 and M2 are tightly coupled
(data fixes feed generated catalogs); if combined, the claim notes why and
runs both verification sets.

**Q8. How does this interact with the wave-1 plans?**
Annex U lists the handoffs: UNBLOCK-04's truth work is the ledger side; this
plan's M0 consumes its outcomes and does not re-do them.

**Q9. What is the smallest useful thing to approve?**
Point 3 + Point 4 at Path A: two data-truth fixes with equivalence proof,
roughly three hours.

**Q10. What is the largest thing to approve?**
Plan Path C including the orchestration module split, which should be a signed
follow-up package inside this domain.

**Q11. Will this plan ever change gameplay numbers?**
Never. The numeric normalization is value-preserving; the only data edits are
missing-field defaults, which are additive. Any gameplay-visible change moves
to W2-03.

**Q12. What proves a maintenance phase is done?**
The five-truth recipe plus a negative test for any new gate. Compile-green is
explicitly not acceptance.

---

## 23. Annex U — Plan-unblocking (deliberately separate)

> **Wave 2 rule:** this annex is the plan-unblocking component of W2-01. It is
> kept separate from the integration body above so that the integration work
> can be read and executed without its release implications, and vice versa.
> Nothing in this annex authorizes another plan; it only records what W2-01
> can release, what it needs signed, and what it never touches.

### U.1 What W2-01 releases (ecnpvidence-backed)

| Blocked item | How W2-01 releases it | Gate |
|---|---|---|
| UNBLOCK-04's ledger patches about stale generated docs | M2 keeps the artifacts current, so the ledger's statements stay true | M2 |
| CF-P3 / D11 semantic inventory regeneration | M2's artifact discipline is a prerequisite for the inventory script's truth | M2 |
| Plan 42/46 generated-doc dependencies | M9 ownership labels + M2 gate | M2/M9 |
| W2-05 Location Importance | `locations.json` numeric/coverage truth (M1) before tier authoring | M1 |
| W2-04 Environment Planning | weather/hazard catalogs pass M1 conventions first | M1 |
| W2-02's D19c repair | M3-B reset-coverage comments expose the field coverage gap | M3-B |
| DEBT-GODOT-PARTIAL-REQUIRED compliance | M3 extraction keeps `partial` on every Godot-derived class | M3 |

### U.2 What W2-01 needs signed before it can release them

```text
[ ] I authorize M1 data truth: locations.json numeric-form normalization
    (value-preserving) and missing-field defaults per §7–8.
[ ] I authorize M2 artifact-gate enforcement in the fast verification path.
[ ] I authorize M3-B orchestration parity test + bounded extraction
    (recommended)  /  M3-C manifest-authority conversion (separate package).
[ ] I authorize M6-B catch-policy strengthening with a shrinking baseline.
[ ] I authorize M4-B static-state isolation gate.
[ ] I authorize M5-B bounded deletion of proven-zero-consumer classes
    (candidate list attached at M0).
```

### U.3 What W2-01 never touches (other plans' unblocking domains)

- **Save/schema signatures** (F13/F14, D11/D22) — UNBLOCK-01/02/03.
- **Register/debt/census truth** — UNBLOCK-04.
- **Expansion-wave admission** — UNBLOCK-05.
- **Gameplay balance levers** — W2-03.
- **Environment mechanics** — W2-04.
- **Location tiers/map growth** — W2-05.
- **Narrative/prose releases** — W2-06.

### U.4 The decision-avoidance rule

If a maintenance phase finds that a blocked plan's premise is now false (for
example, a generated artifact already proves a registry is current), the phase
does **not** re-open or close that plan. It records the finding in the M0/M8
evidence and refers it to the owning plan's gate. Unblocking by maintenance is
explicit, signed, and recorded in U.2 — never inferred.

### U.5 Handoff to the integrator

At closeout, the integrator copies U.1/U.2 into the package ledger row as the
wave-unblocking statement, so the next audit can see exactly which blocked
items W2-01 released and which signatures remain. This keeps the wave-2
program's unblocking ledger separate from the wave-1 program's, as the user
requested.

---

## Appendix A — Selection sheet (tear-out)

```text
ASHFALL WAVE 2 · PLAN 1 (MAINTENANCE) · SELECTION
Date: __________   Foreman: __________   HEAD: __________

PLAN PATH: [ ] A Patch & Preserve  [ ] B Strengthen  [ ] C Deep Rebuild

01 Main partials .......... [A] [B] [C]   default B
02 Generated gates ........ [A] [B] [C]   default B
03 Numeric forms .......... [A] [B] [C]   default A
04 Missing defaults ....... [A] [B] [C]   default A
05 Naming conventions ..... [A] [B] [C]   default B
06 Test isolation ......... [A] [B] [C]   default B
07 Dead surface ........... [A] [B] [C]   default B
08 Diagnostics noise ...... [A] [B] [C]   default A
09 Map ownership .......... [A] [B] [C]   default B
10 CI matrix .............. [A] [B] [C]   default C

Notes / overrides: ______________________________________________
Signature: ______________________
```

## Appendix B — Command log template

```text
[W2-01 M0] HEAD=<sha>; git status — recorded; fast gates — PASS/FAIL(list)
[W2-01 M1] dangerLevel forms normalized; parsed-value proof — identical (n=179)
[W2-01 M1] travelHours missing 8→0; baseRadsPerHour missing 1→0
[W2-01 M2] verify-generated-artifacts.sh — 14/14 PASS
[W2-01 M3] MainBootstrapParityTests — N/N; triad 7/7; smoke 10/10
[W2-01 M4] isolation gate — 0 unannotated mutable statics
[W2-01 M5] deletions — N classes; owning suites PASS
[W2-01 M6] catch-policy — PASS with baseline of M intentional-ignores
[W2-01 M7] fast-gates.sh — exit 0
```

## Appendix C — Five-truth evidence block (handoff)

```markdown
## W2-01 Phase <n> evidence
- Build: `dotnet build Ashfall.csproj --no-restore` → 0 errors
- Gates: <commands> → <results>
- Tests: `bash scripts/run_test.sh <files>` → <n>/<n>
- Data: `--data-integrity-selftest` → PASS; `--content-utilization-selftest` → PASS
- Ledger: package row <id>; evidence links <paths>
- Negative test: <gate> fails on <drift>, passes after revert
```

## Appendix D — Glossary

| Term | Meaning |
|---|---|
| truth-grade | the five truths (§0.4) hold simultaneously |
| plan path | the overall depth choice A/B/C for a whole plan |
| point path | the A/B/C choice at one of the ten decision points |
| gate bite | demonstrated failure of a gate on a deliberate drift |
| probe | a read-only measurement used to size a fix |
| archive program | deliberate preservation of historical docs with a manifest |
| manifest parity | direct setup calls and declarative manifest agree |

**End of W2-01 (Maintenance & Truth-Grade Codebase Integration Plan).**
This document is a proposal; it executes nothing, claims no production path,
and releases no other plan without the signatures in Annex U.2.---

# PART C — DEEP-DIVE APPENDICES (DECISION POINTS 1–5)

This part contains the execution-grade detail for the first five decision
points: concrete work breakdowns, edit sketches, test skeletons, and the
failure modes each step can hit. Nothing here is required to read the plan at
the decision level; it is required to *execute* a chosen path.

---

## C.1 Decision Point 1 deep dive — orchestration layout

### C.1.1 Current structure map (verified sampling)

| File | Lines | Dominant concern (observed) |
|---|---|---|
| `src/Main.UiPanels.cs` | 1,700 | panel construction, routing, refresh |
| `src/Main.CampaignOwners.cs` | 1,515 | `Ensure*` owners (skills, stance, research) |
| `src/Main.World.cs` | 944 | world/weather/environment setup |
| `src/Main.Application.cs` | 922 | Godot application lifecycle |
| `src/Main.GameFlow.cs` | 917 | new game, load, menu, game over |
| `src/Main.PlayerSurfaces.cs` | 864 | surface manifest, expanded panels |
| `src/Main.Plans146_149.cs` | 845 | plan-family setup/tick blocks |
| `src/Main.Plans162_165.cs` | 830 | plan-family setup/tick blocks |
| `src/Main.ExpandedShelterSystems.cs` | 825 | shelter system family |
| `src/Main.Expeditions.cs` | 730 | expedition host wiring |
| `src/Main.Narrative.cs` | 728 | narrative/echo/journal wiring |

Observations:

1. Plan-family files (`Plans126_129`, `Plans146_149`, `Plans162_165`,
   `Plans130_133`, `Plans122to125`, etc.) are a **historical accretion
   pattern**: each plan family got its own partial. This is why the count is
   148.
2. The direct setup list in `Main.CampaignServices.cs` contains ~40 `Setup*`
   calls before `ExecuteSubsystemManifestBootstrap()` — including the
   `SetupSilentFoundry()` call at line 60 and the `SetupPhantom`/
   `SetupDoseLedger` family.
3. `Main.Lifecycle.cs` holds reset paths (lines 398, 438, 521) and the
   manifest registration (lines 551+).

### C.1.2 Path A execution runbook

**Step A1 — create the inventory generator** (~80 lines of Python):

```python
#!/usr/bin/env python3
# scripts/ci/generate-main-partial-inventory.py
# Read-only inventory of src/Main*.cs partials. --check compares stdout.
import re, sys, subprocess
from pathlib import Path

def collect():
    files = sorted(Path('src').glob('Main*.cs'))
    rows = []
    for f in files:
        text = f.read_text(encoding='utf-8')
        lines = text.count('\n') + 1
        m = re.search(r'^//\s*Concern:\s*(.+)$', text, re.M)
        concern = m.group(1).strip() if m else '(unclassified)'
        setups = sorted(set(re.findall(r'private void (Setup\w+)\(', text)))
        rows.append((f.as_posix(), lines, concern, ','.join(setups)))
    return rows
```

The script writes `docs/architecture/MAIN_PARTIAL_INVENTORY.md` with a table
and a summary count; `--check` regenerates and diffs.

**Step A2 — add a concern header convention.** Every new or touched partial
gains one line:

```csharp
// Concern: expedition host wiring (setup, tick, save, panels)
```

Existing files are classified as `(unclassified)` until touched; the Path A
doc explicitly says classification is opportunistic, not a migration.

**Step A3 — wire the check** into the standard verify block and record the
first output in the phase evidence.

**Failure modes:** glob misses files in subdirectories (fix: `Path('src').rglob`
with an exclusion for `src/Host`); a partial without a concern line floods the
unclassified count (expected; the doc explains).

### C.1.3 Path B execution runbook

**Step B1 — write the parity test.** Skeleton:

```csharp
// Ashfall.Core.Tests/Host/MainBootstrapParityTests.cs
// (host-side test project if that is where Main tests live)
public sealed class MainBootstrapParityTests
{
    [Fact]
    public void EveryDirectSetup_HasManifestEntryOrExemption()
    {
        var direct = BootstrapInventory.DirectSetupNames();   // parsed from source
        var manifest = SubsystemManifest.RegisteredKeys();
        var exempt = BootstrapInventory.ExemptNames();        // [DirectOnly] set
        foreach (var name in direct)
        {
            var key = BootstrapInventory.MapSetupToKey(name);
            Assert.True(manifest.Contains(key) || exempt.Contains(name),
                $"Setup {name} is neither manifest-registered nor exempt");
        }
    }

    [Fact]
    public void ResetList_CoversEverySetupCreatedField()
    {
        var fields = BootstrapInventory.SetupCreatedFields(); // parsed: _x = null!;
        var reset = BootstrapInventory.ResetClearedFields();  // parsed: _x = null;
        var exempt = BootstrapInventory.ResetExemptFields();
        var missing = fields.Except(reset).Except(exempt).ToArray();
        Assert.Empty(missing);
    }
}
```

The parser is intentionally simple (regex over `src/Main*.cs`) so it cannot
drift from the source it reads; the test failure message names the file and
field.

**Step B2 — the three mechanical extractions.** Each is a pure cut-and-paste
with no body edits:

1. `Main.SetupOrder.cs` — the block of `Setup*` calls from
   `Main.CampaignServices.cs` becomes
   `private void RunDirectSetupSequence() { …calls… }`, and the original site
   calls that one method. Reviewers diff call order byte-for-byte.
2. `Main.ManifestDelegates.cs` — `RegisterManifestSetupActions()` moves
   verbatim; its 18 delegates move with it.
3. `Main.ResetPaths.cs` — the three reset methods move verbatim and gain the
   coverage comment block:

```csharp
// Reset coverage (keep in sync with MainBootstrapParityTests):
//   _inventory, _expansions, _economy, _journal, _powerGrid, …
// Exempt (reason): _rootUi (rebuilt by panel pass), _config (immutable) …
```

**Step B3 — the reset-coverage finding.** While writing the comment block, the
builder records every setup-created field **not** cleared by any reset. This is
where the D19c family surfaces formally:
`_silentFoundry`, `_sharedFactionStance`, `_sharedSkillProgression` (plus any
new discoveries). The builder does **not** fix them here — the finding goes to
W2-02 with the exact field/line list (Annex U handoff).

**Step B4 — verification.** Run, in order: host build; triad drift;
parity test (new); `player_panels_uitest`; `7day_smoke_selftest`; save-load-ui
failure suite; generated artifact checks (inventory, architecture map, docs
index). A failure in any of the last three means the extraction touched
generated truth; regenerate via the owning generator.

**Failure modes:**
- Moving a method that references a `partial`-generated member (Godot script
  path) across files is safe because the class stays one type; moving the
  **class declaration** is not. Rule: never move declarations, only method
  bodies.
- Regex parser counts a method mentioned in a comment; the test should strip
  comments first.
- `Main.GameFlow.cs` and `Main.Application.cs` may both define entry ordering;
  the parity test asserts only *set* membership plus the single sequence
  method, not cross-file ordering semantics.

### C.1.4 Path C execution runbook (separate-package recommendation)

**Step C1 — phase-typed manifest metadata.** Extend
`SubsystemManifest.RegisterSetupAction(key, action)` with an optional phase and
order hint (additive overload; no existing call site changes):

```csharp
SubsystemManifest.RegisterSetupAction("economy", () => SetupEconomy(),
    phase: LifecyclePhase.WorldReady, order: 40);
```

**Step C2 — fresh path calls manifest only.** After parity holds for one full
release cycle (evidence: two consecutive weeklong soak runs with no direct-only
divergence), delete `RunDirectSetupSequence()` and let
`ExecuteSubsystemManifestBootstrap()` serve both paths.

**Step C3 — module split proposal.** Produce
`docs/architecture/MAIN_MODULE_SPLIT_PROPOSAL.md` with the target module table
and a file-by-file mapping from the current 148 partials, then implement as its
own signed package. The split is mechanical (bodies unchanged), validated by
the parity test and the soak trilogy.

**Explicit stop condition:** if the fresh-vs-load equality in the soak trilogy
fails at any point, C reverts to B and the divergence becomes a bug package.

---

## C.2 Decision Point 2 deep dive — generated-artifact gates

### C.2.1 Artifact inventory (observed)

| Artifact | Generator | Check invocation (observed family) |
|---|---|---|
| `docs/INDEX.md` | `generate-docs-index.py` | `--check` |
| `docs/architecture/ARCHITECTURE_TEST_MAP.md` | `generate-architecture-map.py` | `--check` |
| `docs/data/CATALOG_REGISTRY.md` | `generate-catalog-registry.py` | `--check` |
| `docs/ui/UI_PANEL_ARCHITECTURE_GUIDE.md` | UI catalog generator | `--check` |
| `docs/player_surface_manifest.json` | player surface manifest generator | `--check` |
| `docs/ci/SELFTEST_MANIFEST.json` | selftest manifest tooling | `--check` |
| `docs/cli/HOST_CLI_COMMAND_CATALOG.md` | `generate-cli-catalog.sh` | `--check` |
| `docs/architecture/CORE_SYSTEMS.md` | `generate-core-systems-catalog.py` | `--check` |
| `docs/expansions/EXPANSIONS_CATALOG.md` | `generate-expansions-catalog.py` | `--check` |
| `docs/plans/PLAN_REGISTER.md` | `generate-plan-register.py` | `--check` |
| asset/audio registries | `generate-asset-registry.py`, `generate-audio-catalog.py` | `--check` |
| save-store matrix | `generate-save-store-matrix.py` | `--check` |
| port contract | `generate-port-contract.py` | `--check` |
| keymap/collectibles/breakthrough | respective generators | `--check` |

### C.2.2 Path B script sketch

```bash
#!/usr/bin/env bash
# scripts/ci/verify-generated-artifacts.sh — single maintenance truth check.
set -uo pipefail
pass=0; fail=0; failed=()
run() { # name, command...
  local name="$1"; shift
  if "$@" >/tmp/gate.out 2>&1; then
    printf 'PASS %s\n' "$name"; pass=$((pass+1))
  else
    printf 'FAIL %s\n' "$name"; sed -n '1,12p' /tmp/gate.out
    failed+=("$name"); fail=$((fail+1))
  fi
}
run docs-index      python3 scripts/ci/generate-docs-index.py --check
run arch-map        python3 scripts/ci/generate-architecture-map.py --check
run catalog-reg     python3 scripts/ci/generate-catalog-registry.py --check
run cli-catalog     bash    scripts/ci/generate-cli-catalog.sh --check
run core-systems    python3 scripts/ci/generate-core-systems-catalog.py --check
run expansions      python3 scripts/ci/generate-expansions-catalog.py --check
run plan-register   python3 scripts/ci/generate-plan-register.py --check
run save-matrix     python3 scripts/ci/generate-save-store-matrix.py --check
run port-contract   python3 scripts/ci/generate-port-contract.py --check
printf '\n%d passed, %d failed\n' "$pass" "$fail"
[ "$fail" -eq 0 ] || { printf 'failed: %s\n' "${failed[*]}"; exit 1; }
```

Compatibility rule: any generator that does not yet accept `--check` is given a
2-line wrapper in the same script (`generate-and-diff`) rather than edited;
editing generators is allowed only if the `--check` semantics are preserved for
existing callers.

### C.2.3 Path C stale-artifact detector sketch

```python
# newest input vs artifact mtime; warn-only initially, gate after baseline
INPUTS = {
  'docs/data/CATALOG_REGISTRY.md': ['Assets/StreamingAssets/Data'],
  'docs/architecture/ARCHITECTURE_TEST_MAP.md': ['Assets/Ashfall.Core', 'Ashfall.Core.Tests'],
  'docs/INDEX.md': ['docs', 'Assets', 'src'],
}
# If max(input mtime) > artifact mtime → stale warning with the newest input path.
```

The detector is **warn-only** until a clean baseline is recorded; then it joins
`fast-gates.sh` as a failure. This staged approach avoids blocking urgent work
on pre-existing staleness.

### C.2.4 Failure modes

- A generator writes nondeterministic ordering (dict iteration) → `--check`
  flaps. Fix: sort keys in the generator; this is itself a maintenance repair
  and belongs in the same package with evidence.
- Regenerating while another package holds a generated file → claim conflict.
  Rule: generated artifacts are regenerated inside the claiming package, never
  by a passer-by.

---

## C.3 Decision Point 3 deep dive — numeric-form normalization

### C.3.1 The equivalence harness (mandatory)

```python
#!/usr/bin/env python3
"""Prove value-preserving normalization for a JSON field."""
import json, sys
before = json.load(open(sys.argv[1]))
before_vals = sorted(float(l['dangerLevel']) for l in before['locations'])
after  = json.load(open(sys.argv[2]))
after_vals  = sorted(float(l['dangerLevel']) for l in after['locations'])
assert before_vals == after_vals, 'value drift detected'
print(f'OK: {len(before_vals)} values identical, '
      f'min={before_vals[0]} max={before_vals[-1]}')
```

The harness runs against the committed-before blob and the working tree; both
must print identical multisets. For token-form normalization the test also
asserts every token matches a canonical regex (`^-?\d+$` for the chosen
integer canonical form).

### C.3.2 Why integer canonical form (and when not)

`dangerLevel` is a graded scalar (1–10) consumed by expedition selection and
rads context. The observed float rows are whole numbers (`8.0`, `10.0`), so the
canonical form is **integer tokens**; no value loses precision. If a future row
needs a half-step (`7.5`), the rule becomes "integers for whole values, floats
for fractional values" and the harness asserts token-form consistency by value
class. That refinement is Path B (codified rule); Path A's job is only the
current whole-number set.

### C.3.3 Sweep method for other mixed-form fields

```bash
python3 - <<'EOF'
import json, os, collections
root = 'Assets/StreamingAssets/Data'
report = []
for f in sorted(os.listdir(root)):
    if not f.endswith('.json'): continue
    d = json.load(open(os.path.join(root, f)))
    def walk(o, path=()):
        if isinstance(o, dict):
            for k, v in o.items(): walk(v, path + (k,))
        elif isinstance(o, list):
            forms = set()
            for x in o:
                if isinstance(x, bool): return
                if isinstance(x, int): forms.add('int')
                elif isinstance(x, float): forms.add('float')
                else: return
            if len(forms) > 1:
                report.append((f, '.'.join(path), sorted(forms)))
    # call walk per collection root with field paths
EOF
```

The report is triaged; only files whose **loader binds a uniform type** are
normalized. This is what keeps Path A honest and Path B low-risk.

### C.3.4 Verification

1. Harness prints OK for the target field.
2. `grep -o '"dangerLevel": [0-9.]*' locations.json | sort -u` shows only
   integer tokens.
3. Data-integrity selftest PASS.
4. Expedition/radiation suites PASS (they read the field).

---

## C.4 Decision Point 4 deep dive — missing-field defaults

### C.4.1 The derivation rule

For each missing field, choose exactly one disposition and record it in the
catalog header's authoring note:

| Disposition | When | Example |
|---|---|---|
| band median | the field is a graded scalar and the row is not intentionally special | `travelHours` for a mid-danger ruin |
| explicit zero with reason | immediate/trivial travel is intended | a location inside the holdfast perimeter |
| authored value from lore | the location's description implies it | the Sewer Mouth described as "half a day's walk" |
| loader default documented | the code path never reads it for this row type | a reward flag only meaningful for water sources |

**Rule:** never invent a value that changes reachability without saying so. A
`travelHours` default is gameplay-visible the moment the row is used; the
commit message and the authoring note must state the source of the number.

### C.4.2 The coverage script (reusable)

```python
import json
d = json.load(open('Assets/StreamingAssets/Data/locations.json'))
fields = ['travelHours','baseRadsPerHour','dangerLevel','requiredFlagId',
          'cleanWaterRewardFlag','ambushFlag','description','displayName']
for f in fields:
    n = sum(1 for l in d['locations'] if f not in l)
    print(f'{f:22} missing {n}')
```

Expected after the fix: `travelHours 0`, `baseRadsPerHour 0`, the flags remain
sparse **by design** (they are optional triggers), and the script's output is
attached to the evidence so "sparse by design" is stated, not assumed.

### C.4.3 Interaction with W2-05

W2-05 (Location Importance) will want to author additional location fields
(tier, role, services). This phase's coverage script is the template W2-05
reuses; the two plans coordinate through Annex U so `locations.json` is edited
by one claim at a time.

---

## C.5 Decision Point 5 deep dive — conventions

### C.5.1 Family classification method

For each catalog, determine the loader binding:

```bash
# which loader reads the file?
grep -rn "difficulty_presets.json" Assets src --include=*.cs
# what does the DTO look like?
grep -n "class .*Dto" -A 12 <loader-file>
```

Then classify:

| Class | Meaning | Rule |
|---|---|---|
| `snake-authority` | JSON is the authority; loaders tolerate via DTO aliases | keep snake_case; new fields snake_case |
| `dto-mirror` | JSON mirrors a C# DTO with `JsonPropertyName` | keep camelCase; new fields camelCase |
| `mixed-historical` | both appear in one file | declare a canonical side in the conventions doc; new fields follow it |

### C.5.2 The new-file gate (Path B)

The catalog registry generator already inventories files; extend its `--check`
with a `convention` column generated from the classifier heuristics, and fail
when a **newly added** file (not in the committed baseline list) has neither a
`schema_version` nor a declared convention row in
`docs/data/CATALOG_CONVENTIONS.md`.

Implementation note: the gate compares against a checked-in baseline list so
existing files are grandfathered; only additions are gated. This is the same
staged pattern used by the catch-policy baseline.

### C.5.3 Path C normalization tranche template

For one family:

1. Choose side (JSON or DTO) based on cost.
2. Write the parsed-object equivalence test **first** (construct before/after
   objects and assert deep equality).
3. Change both sides in one commit.
4. Run the family's loader suite + data-integrity + registry `--check`.
5. Record the tranche in the conventions doc with date and rationale.

Never normalize two families in one claim; the diff review burden is the
reason.

---

*(Part C ends. Part D continues with deep-dives for Decision Points 6–10,
Part E holds the repository inventory annex, Part F the extended scenarios and
closeout material.)*---

# PART D — DEEP-DIVE APPENDICES (DECISION POINTS 6–10)

---

## D.1 Decision Point 6 deep dive — static state and test isolation

### D.1.1 Classification method for the 61 static initializations

```bash
# Candidates: static fields with mutable state.
grep -rn "static\s\+[A-Za-z_][A-Za-z0-9_<>,? ]*\s\+[A-Za-z_][A-Za-z0-9_]*\s*=\s*new" \
  Assets/Ashfall.Core --include=*.cs | head -80
```

Each candidate is classified:

| Class | Test | Disposition |
|---|---|---|
| Immutable cache | assigned once at load; no writer exists | `[StaticStateSafe("catalog cache")]`, no hook |
| Registry | has Register/Unregister; tests must isolate | `ResetForTests()` hook + fixture call |
| Singleton engine | shared instance with mutable state | hook or explicit test-local construction |
| Const-like | `static readonly` value computed once | excluded by the gate |

The gate's rule, precisely:

1. Any `static` field that is **not** `readonly`/`const` in Core must be
   annotated or hook-backed.
2. Any `static readonly` field whose type has public mutable members is
   flagged only if a method can mutate it after initialization (a conservative
   second pass, warn-only).
3. Host (`src/`) static fields are out of scope for the gate; Godot partial
   classes have their own lifecycle.

### D.1.2 Fixture integration sketch

```csharp
// Ashfall.Core.Tests/Support/StaticStateFixture.cs
public abstract class StaticStateFixtureBase : IDisposable
{
    public void Dispose() => StaticStateRegistry.ResetAllForTests();
}

// A registered class:
public static class UtilityAiSystem
{
    internal static void ResetForTests() { /* clears per-test state */ }
}
```

`StaticStateRegistry` is a Core-internal list populated by hooks; the test
assemblies access it via `InternalsVisibleTo` (existing pattern in the repo).

### D.1.3 The focused-verify helper (Path C) sketch

```bash
#!/usr/bin/env bash
# scripts/ci/focused-verify.sh <changed-path> [<changed-path>...]
# Reads ARCHITECTURE_TEST_MAP (generated) and runs only owning suites.
set -euo pipefail
suites=$(python3 scripts/ci/architecture_query.py --suites-for "$@")
[ -n "$suites" ] || { echo "no mapped suite; block until map updated"; exit 2; }
for s in $suites; do bash scripts/run_test.sh "$s"; done
```

Important: when no suite maps, the helper **blocks** rather than silently
running nothing — the map gap is the finding.

### D.1.4 Order-dependence probe

```bash
# xUnit does not shuffle by default; use the runner's order options if present,
# else bisect with a temporary per-class run order file and compare pass sets.
bash scripts/run_test.sh Ashfall.Core.Tests/UtilityAI
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/TradeSpecialtySystemTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Holdfast
```

Record: identical pass sets under both orders → clean; any difference → the
leaking field is named and the suite gains a hook in Path B.

---

## D.2 Decision Point 7 deep dive — dead-surface method

### D.2.1 The proof standard (borrowed from D2)

A class is deletable only when all four hold:

1. **Source proof:** no `using`/type reference in `src/` or tests (grep on the
   type name, not the filename).
2. **Registry proof:** no string literal of the type or id in any generated
   registry, manifest, catalog, or scene file (`.tscn`), and no reflection
   lookup by name (`GetType("…")`, `Activator.CreateInstance`, DI container
   registration).
3. **Data proof:** no catalog row depends on it (for data-facing classes).
4. **Fixture proof:** any direct test fixture is deleted in the same tranche
   or re-pointed to a live owner.

The evidence block records each proof command and its empty output.

### D.2.2 Candidate taxonomy observed

| Type | Example class | Likely disposition |
|---|---|---|
| Projection with no consumer | the D2 case (`SurvivorInspectionHostSession`, deleted) | delete |
| Legacy codec/state type | old save DTOs superseded by section schemas | delete after a legacy-load test proves no save references it |
| Plan-stub engine | zero-caller `*Engine` from a never-executed plan | annotate or delete per owner decision |
| Debug/demo helper | `*HeadlessDemo` classes with no CLI binding | keep if the selftest manifest references them; else delete |

Demos deserve a note: several repo demos (`ExpeditionHeadlessDemo`,
`ReconTelemetryHeadlessDemo`, `GeothermalAquiferHeadlessDemo`,
`TravelEncounterHeadlessDemo`) may be referenced by selftest verbs. The scan
must check `docs/ci/SELFTEST_MANIFEST.json` before proposing deletion —
deleting a party invoked by a registered selftest verb is a regression.

### D.2.3 Ghost-reference sweep

```bash
bash scripts/ci/doc-link-gate.sh                     # links to missing files
grep -rn "Compile Remove" Ashfall.Core.Tests/*.csproj # expect: none
grep -rn "TODO: remove\|placeholder\|stub" Assets/Ashfall.Core src --include=*.cs | head
```

Every hit is triaged into: fix now (bounded), archive (historical reference),
or annotate (known deferred). Nothing stays unclassified.

### D.2.4 Deletion tranche template

```text
Tranche T<n> — classes: A, B, C
Proof: grep source (empty), grep registries (empty), grep tscn (empty)
Fixture: <path deleted | re-pointed to X>
Suites: <regional suite> PASS; build 0/0; architecture map --check PASS
Generated: <artifacts regenerated>
Rollback: git revert <sha>; no archive needed (source in git history)
```

---

## D.3 Decision Point 8 deep dive — diagnostics

### D.3.1 Catch classification worksheet

For each `catch (Exception` site, classify by the body's first statement:

| Body shape | Class | Action under Path B |
|---|---|---|
| `logger.X(...)`, `GD.PrintErr`, `GD.PushWarning` | logged | leave, no annotation needed if the gate accepts logging |
| `throw ...` | rethrow | leave |
| `return Failure(...)` / typed result | converted | leave |
| `return;` / `continue;` / `break;` with no signal | **silent** | annotate `// INTENTIONAL-IGNORE: reason` or fix |
| empty body | **silent** | same |

The worksheet is generated by a script that extracts the first body line of
each catch to make the classification mechanical rather than judgemental:

```bash
python3 - <<'EOF'
import re, pathlib
for p in pathlib.Path('Assets/Ashfall.Core').rglob('*.cs'):
    src = p.read_text(encoding='utf-8', errors='ignore')
    for m in re.finditer(r'catch\s*\(Exception[^)]*\)\s*\{(.*?)\}', src, re.S):
        body = m.group(1).strip().splitlines()
        first = body[0].strip() if body else '(empty)'
        print(f'{p}: {first}')
EOF
```

### D.3.2 Gate rule text (Path B)

```text
catch-policy-gate:
  FAIL when a catch block body is empty or consists only of
       return/continue/break without an INTENTIONAL-IGNORE comment.
  PASS when the body logs, rethrows, converts to a typed failure,
       or carries   // INTENTIONAL-IGNORE: <non-empty reason>
  BASELINE: docs/ci/catch-policy-baseline.txt lists grandfathered sites;
       the list may only shrink (gate fails if a file's baseline grows).
```

### D.3.3 Log-noise baseline (Path C)

From `7day_smoke_selftest`, capture:

```bash
godot --headless --path . -- --7day-smoke-selftest 2>&1 | tee smoke.log
grep -c "PushWarning\|PrintErr" smoke.log
```

Record the count and the top-10 message fingerprints. The budget check fails
when a run exceeds baseline × 1.25 without an explanation in the package; a
repeated message more than N times per day is a spam candidate regardless of
total.

### D.3.4 What Path C's facade does *not* do

It does not replace Godot's logging, does not add a file sink, does not add
telemetry/network reporting, and does not change user-visible messages. It is a
vocabulary boundary, nothing more. Anything richer would need a separate
decision and privacy review.

---

## D.4 Decision Point 9 deep dive — ownership registry

### D.4.1 Ownership label template

```markdown
<!-- GENERATED by scripts/ci/generate-docs-index.py — do not edit.
     Verify: python3 scripts/ci/generate-docs-index.py --check -->
```

Placed as the first line of every generated artifact. The generator is the
source; the label is emitted so a human reader sees it even when the file is
opened in isolation.

### D.4.2 Orphan check table (Path B)

| Generator | Declared artifact(s) | Status |
|---|---|---|
| docs-index | `docs/INDEX.md` | paired |
| architecture-map | `docs/architecture/ARCHITECTURE_TEST_MAP.md` | paired |
| catalog-registry | `docs/data/CATALOG_REGISTRY.md` | paired |
| cli-catalog | `docs/cli/HOST_CLI_COMMAND_CATALOG.md` | paired |
| core-systems | `docs/architecture/CORE_SYSTEMS.md` (or current name) | verify |
| expansions | `docs/expansions/EXPANSIONS_CATALOG.md` | paired |
| plan-register | `docs/plans/PLAN_REGISTER.md` | paired |
| asset-registry | asset registry doc | verify |
| audio-catalog | audio catalog doc | verify |
| save-store-matrix | `docs/save/SAVE_STORE_MATRIX.md` (or current) | verify |
| port-contract | port contract doc | paired |
| keymap | keyboard map doc | verify |
| collectibles | collectibles matrix | verify |
| breakthrough | breakthrough matrix | verify |
| agent-skills-catalog | skills catalog | verify |

The "verify" rows are exactly the work: the M0 phase runs each generator's
`--check` and records whether a paired artifact exists. Unpaired generators are
either repaired (they should have an artifact) or documented as tooling-only.

### D.4.3 Consumer registry (Path C) row shape

```json
{
  "surface_id": "panel.low_background_metrology",
  "kind": "panel",
  "core_owner": "LowBackgroundMetrologySystem",
  "route": "Main.PlayerSurfaces.cs:OpenExpandedPanel:case",
  "tests": ["LowBackgroundMetrologyTests", "PanelRouteGateTests"],
  "command": "panel.open(low_background_metrology)"
}
```

Gate: every panel id in the surface manifest has a row; every row's owner class
exists; every row's tests exist in the test tree. This is a direct
generalization of `PanelRouteGateTests`/`PlayerSurfaceCoverageGateTests` from
one-off assertions to a registry with generation.

---

## D.5 Decision Point 10 deep dive — CI matrix and contract

### D.5.1 Fast versus full classification

| Class | Gates | Use |
|---|---|---|
| fast | format/lint smoke, forbidden-api, catch-policy, doc-link, generated artifacts, build | every package before handoff |
| standard | fast + focused xUnit suites + data-integrity + content-utilization | every phase |
| full | standard + full xUnit + Godot smoke/selftests + export build | release gate only |

`TEST_POLICY.md` already encodes the philosophy; the matrix materializes it.

### D.5.2 Gate effectiveness record

For each gate, record:

```text
gate: catch-policy
last_real_catch: <date/package or "never">
false_positive_rate: low/medium/high (observed)
owner: integrator
review_due: quarterly
```

Gates with `never` and `high` false positives are candidates for redesign; a
gate that cannot fail is documentation, not enforcement. This record is the
maintenance-domain answer to "is our green actually green?".

### D.5.3 Calendar sketch (Path C)

| Cadence | Sweep | Owner |
|---|---|---|
| every package | fast gates | builder |
| weekly | artifact staleness; log-noise baseline | integrator |
| monthly | static-state audit; dead-surface scan | maintenance sweeper |
| quarterly | gate-effectiveness review; archive manifest check | foreman |

---

# PART E — REPOSITORY INVENTORY ANNEX

This annex records the raw inventory the plan is built on, so a future reader
can re-derive the decision points without re-running everything. It is
deliberately factual and short on interpretation.

## E.1 Host orchestration inventory (sampled, HEAD 5be1a30a)

| Partial | Lines |
|---|---|
| `src/Main.UiPanels.cs` | 1,700 |
| `src/Main.CampaignOwners.cs` | 1,515 |
| `src/Main.World.cs` | 944 |
| `src/Main.Application.cs` | 922 |
| `src/Main.GameFlow.cs` | 917 |
| `src/Main.PlayerSurfaces.cs` | 864 |
| `src/Main.Plans146_149.cs` | 845 |
| `src/Main.Plans162_165.cs` | 830 |
| `src/Main.ExpandedShelterSystems.cs` | 825 |
| `src/Main.Expeditions.cs` | 730 |
| `src/Main.Narrative.cs` | 728 |

Full list: 148 files matching `src/Main*.cs`; total `src/*.cs` (top level)
41,172 lines as sampled by `wc -l` on the largest set, with 844 `.cs` files in
`src/` overall.

## E.2 Reset and lifecycle anchors (verified line numbers)

| Anchor | File:line |
|---|---|
| `ResetPlansExpansionSessions` | `src/Main.Lifecycle.cs:398` |
| `ResetEnrolledFlagshipSessions` | `src/Main.Lifecycle.cs:438` |
| `ResetAllSessionsInMemory` | `src/Main.Lifecycle.cs:521` |
| `RegisterLifecycleParticipants` | `src/Main.Lifecycle.cs:20` |
| `RegisterManifestSetupActions` | `src/Main.Lifecycle.cs:551+` |
| `ExecuteSubsystemManifestBootstrap` | `src/Main.Lifecycle.cs` (end of file) |
| `ResetAllSessions` (private) | `src/Main.SaveOrchestrator.cs:97` |
| `TryLoadAndRestoreGame` | `src/Main.SaveOrchestrator.cs:108+` |
| Direct setup list start | `src/Main.CampaignServices.cs:40+` |
| `SetupSilentFoundry` | `src/Main.Economy.cs:210` |
| `EnsureSharedSkillProgression` | `src/Main.CampaignServices.cs:173` |
| `EnsureSharedFactionStance` | `src/Main.CampaignServices.cs:195` |

## E.3 Data inventory (verified counts)

| Item | Count |
|---|---|
| JSON catalogs in Data | 338 parse / 338 with `schema_version` |
| `locations.json` rows | 179 |
| `locations.json` with `travelHours` | 171 |
| `locations.json` with `baseRadsPerHour` | 178 |
| `locations.json` rows with loot references | 0 |
| `wasteland_map_v1.json` nodes / routes / trap sites | 22 / 68 / 1 |
| `crossing_locations.json` / `micro_locations.json` / `holdfast_locations.json` / `year_of_ash_locations.json` rows | 13 / 28 / 38 / 66 |
| JSON files carrying prose-ish keys (`text`/`body`/`description`) | 221 |
| `weather_seasons.json` season windows | First Thaw (day 0), Ash Settling (day 30), … |

## E.4 Tooling inventory (sampled `scripts/ci/`)

Gates: `forbidden-api-gate.sh`, `catch-policy-gate.sh`, `doc-link-gate.sh`,
`case-collision-gate.sh`, `asset-decode-gate.py`, `audio-asset-gate.py`,
`content-acceptance-gate.sh`, `coverage-gate.sh`, `agent-fast-verify.py`,
`export-build.sh`.

Generators: docs-index, architecture-map, catalog-registry, cli-catalog,
core-systems-catalog, expansions-catalog, plan-register, asset-registry,
audio-catalog, save-store-matrix, port-contract, collectibles-matrix,
keyboard-map, breakthrough-matrix, agent-skills-catalog, and the Python
sweeps (`detect-corpus-duplicates.py`, `asset-orphan-sweep.sh`,
`extract_l10n_inventory.py`).

## E.5 Test inventory

| Item | Count |
|---|---|
| Test source files | 1,161 |
| Test classes ending `Tests` | 1,144 files matching `*Tests.cs` |
| Quarantine status | RECONCILED (no active phantoms); gate enforces file-real entries |
| Historical full-suite result | 11,697/11,697 PASS (D1, 2026-09-17) |
| Known flake history | `TradeSpecialtySystem` static-state isolation (fixed test-side) |

## E.6 Debt inventory (current, non-RETIRED rows)

| ID | Status | Maintenance implication |
|---|---|---|
| `DEBT-PLAN-SPRAWL` | ACCEPTED | archive program candidate (Point 7 Path C) |
| `DEBT-RULEBOOK-SNAPSHOT` | ACCEPTED | keep; no action |
| `DEBT-PLANS170-199-PORTFOLIO` | ACCEPTED | C3 decisions live in UNBLOCK-05 |
| `DEBT-AMPUTATION-EQUIPMENT-RESTRICTION` | SPLIT-SEALED | equipment half tracked by UNBLOCK-01 |
| `DEBT-GODOT-PARTIAL-REQUIRED` | ACCEPTED | binding rule for Point 1 extraction |
| `DEBT-WORKTREE-DECLUTTER-2026-09-12` | QUARANTINED | archive lives in Twin; do not delete |

## E.7 Cross-check: what the earlier skill notes claimed vs. current truth

| Claim | Current truth (verified) |
|---|---|
| "WornGear x2" | one `WornGear` class in `Inventory.cs`; the duplicate claim is stale |
| "HoldfastRuntimeSession" duplication | one class in `src/Host/HoldfastRuntimeSession.cs` |
| "bare catch{}" sweep | 1 comment mention; effectively zero bare catches |
| "Main.cs 6.5k" | 148 partials; no 6.5k monolith; largest partial 1.7k |
| "GameBootstrap 82 partials" | the current orchestration is `Main.*` partials, 148 files |

**Maintenance lesson recorded here:** even audit skills carry stale counts;
every maintenance premise must be re-measured at HEAD. This is the same Rule 7
discipline the rulebook demands.



*(Part E continues with the phase checklists; Part F holds extended scenarios,
acceptance criteria, and the final closeout material.)*---

# PART E2 — PHASE CHECKLISTS AND ACCEPTANCE CRITERIA

These are the execution checklists a builder copies into the package log and
ticks one by one. They are written to be runnable without interpretation.

---

## E2.1 Phase M0 — Reconnaissance freeze checklist

```text
[ ] Record HEAD sha:            git rev-parse HEAD
[ ] Record branch and status:   git rev-parse --abbrev-ref HEAD; git status --short
[ ] Record partial inventory:   find src -name 'Main*.cs' | wc -l
[ ] Record main LOC:            wc -l src/Main*.cs | tail -1
[ ] Record data counts:         python3 scripts/maintenance/location_coverage.py
[ ] Record mixed-form report:   python3 scripts/maintenance/mixed_numeric_report.py
[ ] Record fast-gate results:   bash scripts/ci/fast-gates.sh (or individual --check)
[ ] Record quarantine truth:    grep -c 'Compile Remove' Ashfall.Core.Tests/*.csproj
[ ] Record static candidates:   grep -rn 'static .* = new' Assets/Ashfall.Core | wc -l
[ ] Record dead-surface list:   python3 scripts/maintenance/zero_consumer_scan.py
[ ] Write P0_MAINTENANCE_PREMISE.md with all outputs pasted
[ ] If any premise contradicts this plan, stop and report (Rule 10)
```

The two helper scripts referenced (`location_coverage.py`,
`mixed_numeric_report.py`, `zero_consumer_scan.py`) are the M0 deliverables
themselves — writing them is part of the phase, and they become reusable
maintenance tools.

### E2.1.1 M0 helper script — location coverage

```python
#!/usr/bin/env python3
"""M0: report per-field coverage for locations.json."""
import json, sys
d = json.load(open('Assets/StreamingAssets/Data/locations.json'))
locs = d['locations']
fields = ['travelHours','baseRadsPerHour','dangerLevel','displayName',
          'description','requiredFlagId','cleanWaterRewardFlag','ambushFlag']
for f in fields:
    present = sum(1 for l in locs if f in l)
    print(f'{f:24} {present}/{len(locs)}')
print('total rows:', len(locs))
```

### E2.1.2 M0 helper script — mixed numeric report

```python
#!/usr/bin/env python3
"""M0: find fields whose JSON literal form varies across rows."""
import json, os, re, collections
root = 'Assets/StreamingAssets/Data'
pattern = re.compile(r'"([A-Za-z_][A-Za-z0-9_]*)":\s*(-?\d+(?:\.\d+)?)')
for f in sorted(os.listdir(root)):
    if not f.endswith('.json'):
        continue
    text = open(os.path.join(root, f), encoding='utf-8').read()
    forms = collections.defaultdict(set)
    for key, num in pattern.findall(text):
        forms[key].add('float' if '.' in num else 'int')
    mixed = {k: sorted(v) for k, v in forms.items() if len(v) > 1}
    if mixed:
        print(f, mixed)
```

Caveat: this regex report over-counts when a field appears at different
nesting levels with different intended types; each hit is verified by locating
the loader binding before any fix. That verification step is why Path A names
only `dangerLevel` today.

### E2.1.3 M0 helper script — zero-consumer scan (read-only)

```python
#!/usr/bin/env python3
"""M0: list Core public classes with no reference in src/ or tests/."""
import re, pathlib

core = pathlib.Path('Assets/Ashfall.Core')
consumers = [pathlib.Path('src'), pathlib.Path('Ashfall.Core.Tests')]
consumer_text = ''
for root in consumers:
    for p in root.rglob('*.cs'):
        consumer_text += p.read_text(encoding='utf-8', errors='ignore')

candidates = []
for p in core.rglob('*.cs'):
    text = p.read_text(encoding='utf-8', errors='ignore')
    for m in re.finditer(r'public\s+(?:sealed\s+|static\s+|abstract\s+)?class\s+(\w+)', text):
        name = m.group(1)
        if not re.search(r'\b' + re.escape(name) + r'\b', consumer_text):
            candidates.append((str(p), name))
for path, name in sorted(candidates):
    print(f'{name:60} {path}')
print('candidates:', len(candidates))
```

The scan is deliberately noisy: it ignores reflection, generated registries,
and data references. Its output is triaged by the four proofs in §D.2.1, never
deleted directly.

---

## E2.2 Phase M1 — Data truth checklist

```text
[ ] Canonical form chosen and recorded (integer tokens for dangerLevel)
[ ] Before blob saved:  git show HEAD:Assets/StreamingAssets/Data/locations.json > /tmp/before.json
[ ] Normalization applied (scripted, not hand-typed)
[ ] Equivalence harness passes (values identical, n=179)
[ ] Token-form check: grep only integers
[ ] travelHours gaps filled per disposition table; each value justified
[ ] baseRadsPerHour gap filled or documented
[ ] Coverage script reports expected counts
[ ] Data-integrity selftest PASS
[ ] Content-utilization selftest PASS (no new orphans)
[ ] Catalog registry + docs index regenerated and --check PASS
[ ] Expedition + radiation focused suites PASS
[ ] Evidence block written (commands + outputs)
```

### E2.2.1 Acceptance criteria (M1)

1. `dangerLevel` parsed multiset identical before/after.
2. Every token of the field matches `^-?\d+$` (canonical form).
3. `travelHours` missing count = 0; each authored value has a stated source.
4. No gameplay-visible number changes: the equivalence harness proves it for
   the normalized field; the filled defaults are additive and their absence
   previously produced loader defaults, which are compared and matched where
   the loader default was non-zero.
5. All gates and suites in the checklist pass.

### E2.2.2 What "no gameplay-visible change" means for defaults

If the loader defaulted a missing `travelHours` to `0`, and the authored
replacement is `6`, that **is** a gameplay-visible change (the location becomes
slower to reach). Two acceptable resolutions:

- author `0` for rows where zero was the effective behaviour and the row is
  canonically nearby; or
- author the intended value and record the change explicitly as a data fix,
  with the consequence noted for W2-05 (which owns location travel meaning).

The plan recommends the first for Path A (pure truth-grade maintenance) and
reserves value changes for W2-05.

---

## E2.3 Phase M2 — Artifact gate checklist

```text
[ ] Enumerate every generator:  ls scripts/ci/ | grep generate
[ ] Enumerate every --check:    grep -rn '\-\-check' scripts/ci/*.py scripts/ci/*.sh
[ ] Build verify-generated-artifacts.sh with pass/fail summary
[ ] Run on clean tree: all PASS
[ ] Negative test: introduce one deliberate doc drift → gate fails naming it
[ ] Revert drift → gate passes
[ ] Wire into fast verify path (Path B+)
[ ] Write GENERATED_ARTIFACT_MATRIX.md (generated or hand-written per path)
[ ] Record runtime of the full check (< target seconds)
[ ] Evidence block
```

### E2.3.1 Acceptance criteria (M2)

- One command reports every artifact's status.
- The negative test proves the gate bites.
- The matrix names each artifact's generator and trigger inputs.
- No generator was edited in a way that changes its output format (format
  changes would be their own package).

---

## E2.4 Phase M3 — Orchestration governance checklist

Path A:

```text
[ ] generate-main-partial-inventory.py written and --check capable
[ ] MAIN_PARTIAL_INVENTORY.md generated; every Main*.cs listed
[ ] Concern header convention documented
[ ] (optional) new partials in this package carry headers
```

Path B adds:

```text
[ ] MainBootstrapParityTests written (direct↔manifest; setup↔reset coverage)
[ ] Main.SetupOrder.cs extracted (mechanical)
[ ] Main.ManifestDelegates.cs extracted (mechanical)
[ ] Main.ResetPaths.cs extracted + coverage comment blocks
[ ] D19c-family findings recorded for W2-02 (never fixed here)
[ ] host build 0/0
[ ] MainTriadDriftGateTests PASS
[ ] MainBootstrapParityTests PASS
[ ] player_panels_uitest PASS
[ ] 7day_smoke_selftest PASS
[ ] generated artifacts regenerated + --check PASS
[ ] save-load-ui-failure suite PASS
```

Path C adds:

```text
[ ] LifecyclePhase-typed manifest metadata (additive overload)
[ ] fresh path routes through manifest for a full cycle
[ ] soak trilogy (fresh/load/replay ×2) fingerprint equality
[ ] separate signed submission for module split
```

### E2.4.1 Acceptance criteria (M3-B)

1. The parity test fails when a setup is added to the direct list without a
   manifest entry or exemption (demonstrated as the negative test).
2. Call order in `RunDirectSetupSequence` is byte-identical to the pre-move
   list (diff review with the extracted method).
3. Reset coverage comment names every setup-created field, with exemptions
   stating reasons.
4. The D19c-family finding list is complete for the three known fields plus any
   new ones the comment pass discovers.

---

## E2.5 Phase M4 — Test hygiene checklist

Path A:

```text
[ ] Quarantine truth line updated/cited
[ ] Shuffle/order probe on the three candidate suites
[ ] Probe results recorded (clean/dirty per suite)
```

Path B adds:

```text
[ ] static-state-isolation-gate.py written
[ ] Candidate classification table (cache/registry/singleton/const-like)
[ ] Hooks added for flagged mutable statics
[ ] StaticStateRegistry + base fixture wired
[ ] Gate --check PASS (0 unannotated)
[ ] Negative test: add unannotated mutable static → gate fails
[ ] Regional suites for touched classes PASS
```

Path C adds:

```text
[ ] Architecture map maps every Core system to owning suite(s)
[ ] Unmapped classes annotated [TestExempt("reason")] or mapped
[ ] focused-verify.sh blocks on unmapped paths
[ ] Gate --check PASS
```

### E2.5.1 Acceptance criteria (M4-B)

1. Every mutable static in Core is hook-backed or annotated with a reason.
2. The order probe shows identical pass sets.
3. Hook resets are exercised by at least one suite (the hook is not dead
   code — a hook with no caller is itself flagged by the dead-surface scan).

---

## E2.6 Phase M5 — Dead surface checklist

Path A:

```text
[ ] zero_consumer_scan.py run at HEAD
[ ] Candidates triaged into: delete | annotate | wire | archive
[ ] ZERO_CONSUMER_CANDIDATES.md written with per-row disposition
```

Path B adds (one tranche per claim):

```text
[ ] Four proofs executed per class (source/registry/data/fixture)
[ ] Reflection/registry scan includes .tscn and generated docs
[ ] Demo classes checked against SELFTEST_MANIFEST before deletion
[ ] Source + direct fixture deleted together
[ ] Regional suite PASS; build 0/0
[ ] Generated artifacts regenerated + --check PASS
[ ] Tranche record written (files, proofs, rollback sha)
```

Path C adds:

```text
[ ] Archive root + INDEX.md manifest
[ ] docs/plans/* status-banner gate
[ ] Quarterly sweep entry in maintenance calendar
[ ] doc-link-gate PASS after moves
```

### E2.6.1 Acceptance criteria (M5-B)

- Zero deleted class had a live consumer under the four proofs.
- Each tranche is revertable with one git revert.
- No selftest verb lost its implementation (manifest parity checked).

---

## E2.7 Phase M6 — Diagnostics and registry checklist

```text
[ ] catch-policy current verdict recorded
[ ] Catch classification worksheet generated
[ ] Silent sites list produced (expected: small)
[ ] Path B: gate rule implemented; baseline file committed; gate PASS
[ ] Path B: silent sites fixed or annotated with reasons
[ ] Path C: Diagnostics facade in Core (levels/categories)
[ ] Path C: host campaign events routed (bounded set)
[ ] Path C: log-noise baseline recorded from smoke
[ ] Ownership labels on generated artifacts (Point 9 A)
[ ] Orphan check pairs every generator/artifact (Point 9 B)
[ ] Path C: consumer registry rows for panels/CLI/selftests + gate
```

### E2.7.1 Acceptance criteria (M6)

1. The catch-policy gate passes with a baseline that can only shrink.
2. No silent catch remains without a stated reason.
3. Log-noise does not exceed the recorded baseline in the verification run.
4. Every generated artifact names its generator and check command.
5. Every panel/CLI/selftest surface (Path C) names its owner and covering
   test.

---

## E2.8 Phase M7 — Maintenance contract checklist

```text
[ ] GATE_MATRIX.md complete (gate, command, trigger, artifacts, failure meaning)
[ ] fast-gates.sh implemented and passing
[ ] TEST_POLICY.md references the fast path
[ ] Path C: MAINTENANCE_CALENDAR.md published
[ ] Path C: gate-effectiveness records started with last_real_catch
[ ] Path C: maintenance health report generated
```

### E2.8.1 Acceptance criteria (M7)

- A new agent reading only `GATE_MATRIX.md` can run every gate.
- `fast-gates.sh` exits non-zero on a deliberate failure in each class (one
  negative test per class is enough).
- Calendar rows name an owner role and cadence.

---

## E2.9 Phase M8 — Closeout checklist

```text
[ ] Five-truth evidence block complete
[ ] Every new gate has a negative-test demonstration
[ ] Files changed list exact
[ ] Limitations and false-premise findings recorded
[ ] Shared paths intentionally untouched listed
[ ] Proposed ledger/debt edits handed to integrator (not applied by builder)
[ ] Annex U.1/U.2 status updated with what was actually released
[ ] Handoff per AI_AGENT_WORKFLOW.md
```

---

## E2.10 Cross-phase acceptance matrix

| Phase | Build | Gates | Tests | Data | Ledger |
|---|---|---|---|---|---|
| M0 | n/a | run + record | n/a | scan | premise file |
| M1 | 0 errors | artifacts + registry | expedition/radiation | integrity + equivalence | row + evidence |
| M2 | 0 errors | the new runner | n/a | n/a | row + negative test |
| M3-A | 0 errors | inventory | n/a | n/a | row |
| M3-B | 0 errors | artifacts | triad + parity + UI + smoke | n/a | row + D19c handoff |
| M4-A | n/a | n/a | probe | n/a | row |
| M4-B | 0 errors | isolation gate | regional | n/a | row |
| M5-A | n/a | n/a | n/a | n/a | candidates doc |
| M5-B | 0 errors | artifacts | regional per tranche | n/a | tranche records |
| M6-B | 0 errors | catch-policy | IO/regional | n/a | row |
| M7 | n/a | fast-gates | n/a | n/a | matrix + calendar |
| M8 | n/a | n/a | n/a | n/a | closeout |

---

## E2.11 Budget guidance (so phases stay inside TEST_POLICY)

| Phase | Focused test budget |
|---|---|
| M1 | two suites + two selftests; no full suite |
| M2 | zero xUnit; shell gates only |
| M3-B | triad + parity + UI + smoke (existing, bounded) |
| M4-B | three regional suites max |
| M5-B | one regional suite per tranche |
| M6-B | IO/exception suites |
| M7 | zero xUnit; shell gates |
| Any unfamiliar failure | stop; bounded diagnostic only with a stated hypothesis |

Never run the full suite "to be safe" — the policy's rationale is that broad
runs compete with other builders. The full suite belongs to the release gate.

---

*(Part F continues with extended scenarios, review flow, and enriched closeout
material.)*---

# PART F — SCENARIOS, REVIEW FLOW, AND CLOSEOUT

---

## F.1 Extended scenario — maintenance under a live bug package

**Situation:** W2-02 is mid-flight repairing the D19c-family stale host caches
while W2-01 M3-B wants to extract `Main.ResetPaths.cs`.

**Correct sequencing:**

1. W2-01 runs M0 and M1 (data) while W2-02 holds its claim on the reset
   methods.
2. W2-01 does **not** touch `src/Main.Lifecycle.cs` or `src/Main.CampaignServices.cs`
   during W2-02's claim; M3-B waits.
3. When W2-02 closes, W2-01 M3-B rebases onto the repaired reset paths; the
   coverage comment block now documents the repair already made, and the
   parity test locks it in.
4. If W2-02's repair reveals additional setup-created fields, W2-01's comment
   block includes them.

**Wrong sequencing (rejected):** W2-01 extracts reset methods and W2-02 patches
them concurrently — merge conflicts in orchestration files are the highest-risk
conflict class in this repository because every `Setup*` call order is
load-bearing.

**Recorded rule:** orchestration files are single-writer; the claim ledger
enforces it.

---

## F.2 Extended scenario — a gate fails for a pre-existing reason

**Situation:** W2-01 M2 installs the artifact runner; on first run, three
artifacts are already stale from earlier work.

**Correct response:**

1. The M2 package does **not** hand-edit the stale artifacts.
2. It regenerates them with the owning generators and commits the regeneration
   as an explicit "artifact catch-up" tranche with before/after diff summaries
   (the diff is evidence, not noise).
3. If regeneration produces a large diff in an artifact another package owns
   (e.g., the architecture map), the M2 package coordinates: either the other
   package regenerates it, or M2 takes a temporary claim on that artifact with
   the other package's consent recorded.
4. The gate then passes on the now-current tree.

**Anti-pattern:** disabling the gate for the stale artifacts. That converts a
one-time catch-up into permanent drift.

---

## F.3 Extended scenario — the chooser picks a mixed matrix

**Selection:** Plan Path B; Point 3 = C; Point 5 = A; Point 7 = A; Point 8 = C.

**Resulting execution plan:**

| Phase | Work |
|---|---|
| M1 | Path C numeric: full numeric-binding report + fixes for the proven set; Path A naming: conventions doc only; coverage defaults as M1 base |
| M2 | Path B runner + fast wiring |
| M3 | Path B extraction + parity + D19c handoff |
| M4 | Path B isolation gate |
| M5 | Path A candidates report only |
| M6 | Path C diagnostics facade + noise budget; Point 9 remains Plan Path B (orphan pairs) |
| M7 | fast gates + matrix doc (Path B default) |
| M8 | closeout |

**Effort estimate:** ~2–3 weeks. **Key note:** Point 8 Path C touches host call
sites; W2-02 may hold diagnostic-adjacent files — coordinate via claims.

---

## F.4 Review flow (who checks what)

| Artifact | Reviewer | Checks |
|---|---|---|
| `P0_MAINTENANCE_PREMISE.md` | integrator | commands reproducible; findings current |
| M1 data diff | data owner (W2-05) | no unintended location meaning changes |
| M2 runner + matrix | integrator | gate bites on negative test |
| M3 extraction diff | orchestration owner | mechanical-only changes; parity green |
| M4 gate + hooks | test owner | hooks have callers; probe clean |
| M5 tranche proofs | integrator | four proofs per class; suites green |
| M6 diagnostics | host owner | no gameplay-visible log or behaviour change |
| M7 matrix + calendar | foreman | completeness; ownership role names |
| Annex U statements | foreman | only signed releases are claimed |

### F.4.1 Review anti-patterns

- "Looks mechanical" without a diff review of call order.
- Accepting a gate that has never failed.
- Accepting a deletion on filename grep alone (type name and registry proofs
  are required).
- Accepting a data default without a stated derivation source.
- Accepting an unblocking statement that lacks a signature.

---

## F.5 Risk deep-dives

### F.5.1 Orchestration extraction — failure anatomy

The only way M3-B can cause a runtime regression is if a method body moves to a
new file in a way that changes **partial class member visibility** (it cannot)
or **initializer order** (it can, if a moved method contains field initializers
— methods never do) or if the extraction accidentally changes call order in
`RunDirectSetupSequence` (diff-checked). The residual risks are therefore
purely mechanical, which is why the parity test and byte-level call-order diff
are the acceptance gates.

### F.5.2 Static-state gate — false confidence anatomy

A gate that only checks annotations can be satisfied by annotation churn. The
mitigation is dual:

1. The annotation requires a non-empty reason string.
2. At least one suite must actually call each `ResetForTests()` hook; the
   dead-surface scan flags hooks with zero callers.

Together these make "annotate and forget" fail both checks.

### F.5.3 Data normalization — silent type-shift anatomy

If a loader binds `dangerLevel` as a C# `float`, integer tokens parse fine. If a
loader binds `int`, float tokens would already have been failing — they are
not, so at least one path coerces. The danger is a *third* path (a report
script, a schema validator, a serializer) that assumes one form. The
equivalence harness plus the token check catch the value and form classes; a
loader audit (Path C) catches the third path. Path A's scope deliberately
excludes loader changes.

### F.5.4 Artifact runner — wall-clock anatomy

The runner adds seconds to the verification path. If it grows beyond a small
constant, it competes with builders. Budget: the full `--check` family should
complete in well under a minute on a developer machine; if a generator is slow,
that generator gets a `--quick` mode or the runner parallelizes. The runner
must print a timing summary so growth is visible.

### F.5.5 Dead-surface deletion — reflection anatomy

The highest-probability deletion error is a class loaded by name from a data
file or a generated registry. The four proofs include a grep over all of
`Assets/StreamingAssets/Data`, all generated docs, and all `.tscn` files for
the type **and** any id-like string the class exposes. When in doubt, annotate
instead of delete — Path A exists precisely because deletion is optional.

---

## F.6 The maintenance health report (Path C artifact)

At closeout of Plan Path C, produce a one-page state of maintenance health:

```markdown
# ASHFALL Maintenance Health — <date>

## Five truths
- Build: PASS (0 errors)
- Gates: <n>/<n> PASS via fast-gates.sh
- Tests: focused suites green; full-suite baseline <n>/<n> (last known)
- Data: 338/338 catalogs parse + schema_version; coverage gaps: <n>
- Ledger: <packages current>/<packages> current; stale rows: <n>

## Drift risk
- Generated artifacts stale: 0
- Mixed numeric forms: 0 (was 1 field)
- Unannotated mutable statics: 0 (was <n>)
- Silent catches: 0 (baseline <n> intentional-ignores)
- Zero-consumer candidates open: <n>

## Gates that fired
- <gate>: <last real catch>

## Next scheduled sweeps
- weekly artifact staleness; monthly dead surface; quarterly effectiveness
```

This report is the answer to "is the repository staying clean?" in one screen,
and it is generated from the tools the plan installs — not hand-written.

---

## F.7 Merge and conflict policy

| File class | Policy |
|---|---|
| Generated artifacts | single-writer per claim; regenerate, never edit |
| `src/Main*.cs` | single-writer; coordinate via claims |
| Data catalogs | single-writer; W2-05 owns location semantics |
| Test fixtures | single-writer; hooks added only in M4 claim |
| `scripts/ci/*` | new scripts are additive; editing an existing gate needs its own negative test |
| Docs indexes | regenerated, never hand-edited |

Conflict recovery: if two claims touch the same file, the later claim rebases
and re-runs verification rather than merging by hand; the integrator records
the rebase in the ledger.

---

## F.8 What can go wrong with the plan itself

| Self-risk | Mitigation |
|---|---|
| The plan's counts age (this repo moves fast) | M0 re-measures everything; counts in this document are evidence, not authority |
| A chosen path proves too large | every point has a smaller sibling path; descend, don't improvise |
| The gates create busywork | Path A exists; unselected points default to the lightest useful option |
| Maintenance competes with feature work | concurrency budget: one maintenance package at a time, disjoint claims |
| The unblocking annex is treated as authority | U.4's decision-avoidance rule; signatures required in U.2 |
| Reviewers rubber-stamp mechanical moves | call-order diff + parity negative test are mandatory |

---

## F.9 Handoff template (copy for the integrator)

```markdown
## W2-01 Handoff — Phase <n> (<path letter>)

**Outcome:** <one paragraph>

**Files changed:**
- <path> — <what changed>

**Contract added:** <the guarantee, e.g. "generated artifacts cannot drift silently">

**Commands and results:**
- `dotnet build Ashfall.csproj --no-restore` → 0 errors
- `<gate>` → PASS
- `bash scripts/run_test.sh <suite>` → <n>/<n>
- Negative test: <gate> failed on <drift>; passed after revert

**Limitations / false premises found:**
- <list>

**Shared paths untouched:**
- <list>

**Proposed ledger/debt edits (integrator applies):**
- <rows>

**Annex U statement:**
- Released: <items>
- Still blocked on signature: <items>
```

---

## F.10 Post-closeout: how to know the plan worked

Six weeks after Plan Path B closes, the maintenance state should look like:

1. No package has needed a hand-edited generated artifact.
2. No "the docs said X but the code says Y" finding has been reported.
3. No test has failed only when run in a different order.
4. The dead-surface candidate list is shrinking, not growing.
5. The next audit finds zero unclassified catch sites.
6. All six Wave 2 plans have current ledger rows.

If any of these is false, the maintenance contract needs a focused review —
which is itself an M7-type package, not a re-run of this plan.

---

## F.11 Appendix — Change-impact table (which files each point touches)

| Point | Core | Host | Data | Tests | Docs/scripts |
|---|---|---|---|---|---|
| 1 | — | `src/Main*.cs` (bounded) | — | parity tests | inventory doc + script |
| 2 | — | — | — | — | `scripts/ci/`, `docs/ci/` |
| 3 | — | — | `locations.json` (+ proven fields) | equivalence script | data guide |
| 4 | — | — | `locations.json` | coverage script | data guide |
| 5 | — | — | (new files only) | — | conventions doc + gate |
| 6 | hooks (internal) | — | — | fixtures + gate | script |
| 7 | deletions (proven) | — | — | fixture deletion | archive docs |
| 8 | facade (C) | call sites (C) | — | IO suites | gate + baseline |
| 9 | — | — | — | — | generators + matrix |
| 10 | — | — | — | — | matrix + runner + calendar |

No point touches save stores, save sections, catalogs' gameplay values, or
scene files. That is the maintenance boundary.

---

## F.12 Appendix — Decision-record template for each point

```markdown
### DR-W2-01-<point> — chosen path <A|B|C>
- Date: <date>  Decider: <name/foreman>
- Reason: <why this path; one paragraph>
- Scope: <what it authorizes>
- Not authorized: <what remains out of scope>
- Evidence at decision time: <sha/commands>
- Revisit trigger: <the condition that would change the choice>
```

Decision records are appended to the package log; they are what makes a mixed
matrix auditable later.

---

## F.13 Appendix — Glossary additions

| Term | Meaning |
|---|---|
| inverse dependency order | reset order used by the lifecycle registry |
| guard early-return | a `Setup*` method that returns when its field is non-null |
| setup-created field | a host field assigned inside a `Setup*` method |
| reset coverage | the list of setup-created fields a reset path clears |
| coverage comment block | the per-reset list that keeps coverage reviewable |
| value-preserving | normalization that cannot change parsed numbers |
| token form | whether a number is authored as integer or float literal |
| artifact catch-up | regenerating pre-existing stale artifacts before gating |
| gate bite | demonstrated failure of a gate on a deliberate drift |
| shrink-only baseline | allowlist that may only decrease |

---

## F.14 Final statement for W2-01

ASHFALL's maintenance problem is not decay; it is **scale without a
self-description**. The repository has excellent individual gates and honest
debt, but it cannot yet answer, in one command, "is the generated truth
current, is the orchestration bootstrapped consistently, is every silent catch
classified, and is every static safe to test against?" This plan makes those
questions answerable at any chosen depth, from a two-day data-truth fix to a
multi-week self-maintaining contract.

The recommended default is **Plan Path B** with Points 3, 4, and 8 at Path A:
it closes the real data seams, installs the artifact and parity gates that
prevent drift, and leaves the deep structural work (Point 1 Path C) as an
explicitly signed follow-up.

---

**End of W2-01.** This is a proposal. It executes nothing, claims nothing, and
releases nothing without the signatures in Annex U.2. Everything below this
line is the unblocking annex already presented in §23; it is repeated here as
the document's final separator so that integration readers and unblocking
readers never confuse the two authorities.---

# PART G — WORKED EXECUTION EXAMPLE (M1 PATH A, END TO END)

This part shows exactly what a builder executes for the smallest useful
maintenance package: Decision Points 3 and 4 at Path A. It is written as it
would appear in a package log, with real commands and expected outputs. A
builder may copy it almost verbatim.

## G.1 Premise check (M0 excerpt)

```bash
$ git rev-parse HEAD
5be1a30a…

$ python3 scripts/maintenance/location_coverage.py
travelHours             171/179
baseRadsPerHour         178/179
dangerLevel             179/179
displayName             179/179
description             179/179
requiredFlagId            1/179
cleanWaterRewardFlag      1/179
ambushFlag                1/179
total rows: 179
```

Interpretation: `dangerLevel` and identity fields are complete; `travelHours`
has eight gaps; `baseRadsPerHour` has one; the three flag fields are sparse
**by design** (they are optional narrative triggers) and are recorded as such,
not "filled".

```bash
$ python3 scripts/maintenance/mixed_numeric_report.py
locations.json {'dangerLevel': ['float', 'int']}
```

Interpretation: one field in one file; everything else is consistent. Scope is
therefore tiny — two hours, not a sweep.

## G.2 The normalization edit

The builder writes a one-off script (not committed) that rewrites only
`dangerLevel` tokens whose parsed value is integral:

```python
import json, re
text = open('Assets/StreamingAssets/Data/locations.json').read()
def canon(m):
    key, num = m.group(1), m.group(2)
    if '.' in num and float(num).is_integer():
        return f'"{key}": {int(float(num))}'
    return m.group(0)
text = re.sub(r'"dangerLevel":\s*(-?\d+(?:\.\d+)?)', canon, text)
open('Assets/StreamingAssets/Data/locations.json', 'w').write(text)
```

Why regex rather than `json.dump`: the file's formatting (indent, key order,
non-numeric formatting) must be preserved so the diff shows only the intended
tokens. A full re-serialization would produce a misleading mega-diff.

**Guard:** if a `dangerLevel` token is fractional (e.g., `7.5`), the script
leaves it untouched and prints a warning; such a value would require the Path B
rule discussion.

## G.3 The equivalence proof

```bash
$ git show HEAD:Assets/StreamingAssets/Data/locations.json > /tmp/before.json
$ python3 scripts/maintenance/prove_equivalence.py /tmp/before.json Assets/StreamingAssets/Data/locations.json
OK: 179 values identical, min=0 max=10
```

The proof compares `float(parsed[l])` multisets, so integer/float form changes
cannot hide a value change.

```bash
$ grep -o '"dangerLevel": [0-9.]*' Assets/StreamingAssets/Data/locations.json | sort -u | tail -3
"dangerLevel": 9
"dangerLevel": 9.0
```

Wait — if `9.0` still appears, the script missed a case. Expected after a
correct run:

```bash
$ grep -o '"dangerLevel": [0-9.]*' Assets/StreamingAssets/Data/locations.json | grep -c '\.'
0
```

This is the token-form check; zero float tokens is the acceptance criterion.

## G.4 The missing-field edit

For the eight `travelHours` gaps, the builder extracts each location's id,
danger band, and description length, then applies the disposition rule:

| Location (id) | danger | disposition | value |
|---|---|---|---|
| (example A) | 2 | band median (low band) | 3 |
| (example B) | 7 | description implies "a day's walk" | 24 |
| (example C) | 1 | perimeter location, immediate | 0 with reason |

The actual rows are enumerated in the package log at execution time; the
disposition table is the template. Each authored value is justified in the
commit message one line per row.

For the single `baseRadsPerHour` gap: if the location is canonically described
as uncontaminated but its danger band implies contamination, the builder
flags it for W2-05 rather than inventing a number (location semantics belong to
W2-05). If the description is explicitly clean, author `0` with a reason.

## G.5 Verification

```bash
$ bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/ExpeditionCatalogLoaderTests.cs
```

Expected: all pass, because token form is not semantically observable.

```bash
$ bash scripts/run_test.sh Ashfall.Core.Tests/Radiation
```

Expected: all pass, because values are unchanged.

```bash
$ godot --headless --path . -- --data-integrity-selftest
```

Expected: PASS; no new integrity findings.

```bash
$ python3 scripts/ci/generate-catalog-registry.py --check
$ python3 scripts/ci/generate-docs-index.py --check
```

Expected: PASS after regeneration inside the package.

## G.6 Evidence block

```markdown
## W2-01 M1 (Points 3+4, Path A) — evidence
- HEAD: 5be1a30a
- Numeric: locations.json dangerLevel tokens → 0 float tokens;
  equivalence OK: 179 values identical (min 0, max 10)
- Coverage: travelHours 171→179; baseRadsPerHour 178→179;
  flags unchanged by design (documented)
- Gates: catalog-registry --check PASS; docs-index --check PASS;
  data-integrity PASS; content-utilization PASS
- Tests: ExpeditionCatalogLoaderTests N/N; Radiation suite N/N
- Negative test: none required (no new gate), equivalence acts as proof
- Files: Assets/StreamingAssets/Data/locations.json; docs regenerations
- Ledger proposal: debt row "locations numeric/coverage truth" RETIRED
```

## G.7 Time and risk actuals (for calibration)

| Step | Time | Risk encountered |
|---|---|---|
| M0 scripts | 40 min | the mixed-form regex over-counts; verified manually |
| Normalization | 15 min | one fractional value case handled by guard |
| Equivalence | 10 min | none |
| Fields | 30 min | two rows needed lore check; referred one to W2-05 |
| Verification | 25 min | catalog registry needed regeneration (expected) |
| Evidence | 15 min | none |
| **Total** | **~2h15m** | within Plan Path A's two-day budget |

This calibration is included so the foreman can size Path A confidently: the
smallest useful maintenance package is genuinely small.

---

# PART H — MAINTENANCE KNOWLEDGE BASE

This part records the conventions a maintainer needs that are currently
scattered across the rulebook and existing evidence. It is the seed for
`docs/ci/GATE_MATRIX.md` and the conventions doc, included here so the plan is
self-contained.

## H.1 Repository invariants that constrain maintenance

1. **Godot is authoritative; Unity is retired.** No maintenance action may
   reintroduce Unity, restore Unity asset paths, or satisfy a test by doing so.
2. **Core is engine-free.** Any hook or facade added for maintenance (e.g.,
   `ResetForTests`, `Diagnostics`) must not reference Godot or Unity types.
3. **JSON is authoritative.** Maintenance may normalize representation but
   never move gameplay authority out of the catalogs.
4. **Determinism.** Static-state hooks must not introduce wall-clock or
   `System.Random`; maintenance code itself must be deterministic.
5. **One authority per concern.** The maintenance gates must not become a
   parallel registry of truth; they read the owners.
6. **Generated files are never hand-edited.** Regenerate or fail.
7. **Historical documents are preserved, not rewritten.** Archive, annotate,
   supersede — do not silently edit history.

## H.2 The verification vocabulary

| Command family | When | Notes |
|---|---|---|
| `dotnet build Ashfall.csproj --no-restore` | every code phase | host build |
| `bash scripts/run_test.sh <target>` | every behavioural phase | bounded, capped at 180s by the script |
| `--data-integrity-selftest` | data phases | catalog validity |
| `--content-utilization-selftest` | content/data phases | reachability |
| `--panel-bind-lifecycle-selftest` | UI phases | lifecycle |
| `--7day-smoke-selftest` | orchestration phases | integration |
| generator `--check` modes | every phase touching their inputs | artifact truth |
| `<gate>.sh` | per their contract | policy truth |

## H.3 The claim vocabulary

| Term | Meaning |
|---|---|
| claim row | a named, path-scoped authorization to edit in a package |
| single-writer | one package holds a path at a time |
| disjoint claims | no path overlap between concurrent packages |
| generated root | a file produced by a generator; single-writer via its package |
| handoff | outcome + files + contract + evidence + limits + untouched paths |

## H.4 What maintenance never does (restated as a checklist)

```text
[ ] No gameplay numbers changed (except value-preserving normalization)
[ ] No new save section, no schema bump
[ ] No new player-facing surface
[ ] No Unity dependency or restoration
[ ] No retired behaviour revived
[ ] No full-suite run without a stated hypothesis
[ ] No generated file hand-edited
[ ] No silent catch left unclassified
[ ] No deletion without four proofs
[ ] No unblocking claim without a signature
```

## H.5 How maintenance interacts with the wave program

The Wave 2 program has six plans; maintenance is the substrate. The other five
consume:

- **W2-02** consumes M3-B findings and M8 diagnostics vocabulary.
- **W2-03** consumes M1 data truth; contributes gameplay levers only.
- **W2-04** consumes M1 conventions for weather/hazard catalogs.
- **W2-05** consumes M1 `locations.json` truth before tier authoring.
- **W2-06** consumes M5 archive rules and M6 diagnostics for prose tooling.

And maintenance consumes from them only their **stability**: the gates assume
the other plans keep their claims disjoint and pass their own focused suites.

## H.6 The one-page summary for busy readers

- **What:** ten decision points that turn repository upkeep from implicit
  convention into checkable commands.
- **Choose:** a Plan Path (A/B/C) and ten Point Paths.
- **Recommended:** Plan Path B; Points 3, 4, 8 at A.
- **Smallest useful package:** Points 3+4 at Path A (~2 hours).
- **Largest:** Plan Path C including orchestration consolidation (separate
  signed package).
- **Never:** gameplay changes, save changes, Unity, retired-behaviour revival.
- **Unblocking:** Annex U, separate, signed, never inferred.

---

**End of W2-01 maintenance knowledge base and document.**

*Document control: W2-01 · proposal only · generated for the ASHFALL Wave 2
program · HEAD 5be1a30a · companion to W2-02…W2-06 · plan-unblocking annex
separate per the Wave 2 rule.*