# ASHFALL Worktree Ownership
> **TRIMMED 2026-10-02 (robustness/workflow batch, Task 55).** The full
> 17,502-line claim history (504 `##` claim sections and the
> 248-row legacy claim table) is archived verbatim at
> [`docs/archive/coordination/2026-10-02/WORKTREE_OWNERSHIP_full.md`](docs/archive/coordination/2026-10-02/WORKTREE_OWNERSHIP_full.md)
> (`sha256:766158fda5a7f3e2`). This file is now a short index.
>
> **Contract (unchanged).** One foreman assigns packages; builders implement
> disjoint paths; a claimed path is read-only to everyone except its owner;
> shared seams belong to the integrator. Only the foreman edits this ledger and
> `INTEGRATION_PLANS.md`. Every claim states outcome, exact paths, and status, and
> is released (or marked COMPLETE) when done. Read the archive for historical
> provenance; do not restore archived claims as active authority.
## Active / in-flight claims
## claim-coordination-ledger-trim-2026-10-02 — IN PROGRESS

Robustness/workflow batch, Task 55 (user brief 2026-10-02). Plan:
`.ai/plans/coordination-ledger-trim-2026-10-02.md` (STATUS: APPROVED BY USER).
**Exact owned paths:** `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`,
`KNOWN_DEBT.md`, new `docs/archive/coordination/2026-10-02/**`, generated
`docs/INDEX.md`, this claim, `.ai/state.md`.
**Delivered:** full verbatim snapshots archived; canonical files are now short
indexes (WORKTREE 1.61 MB → 19.5 KB; INTEGRATION_PLANS 487 KB → 2.1 KB;
KNOWN_DEBT 45 KB → 6.3 KB). Regenerated `docs/INDEX.md` (it also closed a
pre-existing 74→75-gate drift from the sprint-1 manifest addition).
**Not claimed:** `.github/workflows/**` (concurrent `perf-track-c-ci-tiering`
session owns Task 49), `scripts/ci/**`, generator scripts.
**Status:** IN PROGRESS 2026-10-02 — archive line counts match originals;
`doc-link-gate.sh` PASS (6130 files); `generate-docs-index.py --check` PASS
(4162 docs); `git diff --check` clean.

## claim-perf-runtime-lifecycle-sprint2-2026-10-02 — IN PROGRESS


Perf sprint 2 (user brief suggested order 21/22/31/32/39). Plan:
`.ai/plans/perf-runtime-lifecycle-sprint2-2026-10-02.md` (STATUS: APPROVED BY USER).
**Exact owned paths:** `src/Host/HoldfastTerminalPanel.cs` (teardown + process
gating), `src/UI/CombatPanel.cs` (process gating), `src/World/SurvivorActorView.cs`
(idle physics gating), new `Ashfall.Core.Tests/Tooling/ProcessOwnershipGateTests.cs`,
new `Ashfall.Core.Tests/Tooling/PanelTeardownGateTests.cs`, this claim, `.ai/state.md`.
**Explicitly untouched:** sprint-1 performance files, `scripts/ci/**`,
`docs/ci/CI_GATE_MANIFEST.json`, all other panels. Tasks 21/22 (spatial/entity
indexes) and 39 (test tiers) are deferred to their own packages with the premise
evidence recorded in the plan §7.
**Status:** IN PROGRESS 2026-10-02 — focused tests 4/4 + 3/3 + 3/3 PASS, host
build 0/0, `--player-panels-uitest` 22/22 PASS.
## claim-perf-runtime-baseline-sprint-2026-10-02 — IN PROGRESS


Performance program, highest-value first sprint (user brief 2026-10-02). Plan:
`.ai/plans/perf-runtime-baseline-sprint-2026-10-02.md` (STATUS: APPROVED BY USER).
**Exact owned paths:** `Assets/Ashfall.Core/Performance/PerfDayProfile.cs` (new),
`Assets/Ashfall.Core/Performance/PerformanceBaselineSuite.cs` (new),
`Assets/Ashfall.Core/Performance/ScaleTier.cs`,
`Assets/Ashfall.Core/Performance/WorkloadProfile.cs`,
`Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs`,
`src/Host/PerformanceSelfTest.cs`, `tools/performance/frame_profile.gd`,
`scripts/ci/perf-baseline-gate.py` (new), `docs/ci/PERFORMANCE_BASELINE.json`
(new), `docs/ci/CI_GATE_MANIFEST.json` (additive `perf_baseline_regression` row
only), `Ashfall.Core.Tests/Performance/PerformanceBaselineSuiteTests.cs` (new),
this claim, `.ai/state.md`.
**Explicitly untouched:** the in-flight `scripts/ci/run-gates.py` path-filter
change and the `compiler_warning_baseline` `paths` key already in the manifest
(preserved byte-for-byte); all live Core gameplay/save/UI paths; the deferred
sprint items 6/7/8 (dirty-state, survivor-social, Utility AI).
**Status:** IN PROGRESS 2026-10-02 — see `.ai/state.md` for the handoff.
## claim-graphics25-batch22-2026-10-01


Owner /root RELEASED 18:33 UTC — BLOCKED by built-in image usage limit. Exact pack artifacts/asset-generation/graphics25-22-2026-10-01/ and .ai/plans/asset-generation-graphics25-22-2026-10-01.md; own additive state/claim entries only. Twenty-five prompts ready (twelve swatches, five alpha graphics, five props, three characters), premise reviewed; zero PNGs generated. First request HTTP429 usage_limit_reached; tool reset 2026-10-02 12:03:20 UTC. Start 18:30 UTC; stopped within deadline. No retries, fallback calls or code/data/live asset/registry/UI changes.
## claim-plan-trim-conservative-method-C-expansion-batch62-2026-09-29


User-authorized conservative trim (method C) — "find 5 bloated plans to
trim, please don't overtrim and overcompress, remove repetitive and
ununique plus boring prose from prose plans and polish all 10 plans!" —
sixty-second batch in the method-C family (executed as 5 files per the
explicit count, batch 48/54–61 precedent). Continues the b61-surveyed
fresh tier of 0-mention, git-clean, untrimmed generated-expansion plans
across docs/* domain subdirectories; the five largest remaining were
taken this batch, ranked by size from a fresh full (non-truncated)
survey. Live-claim screens re-run: the 15 clean single-mention
prose_wave files and the wave*/expansion_* tier remain root-owned by
claim-plan-bloat-reduction-parallel-batch-42…53 (IN PROGRESS,
spot-verified unchanged) — skipped untouched; already-consolidated
committed trims (prose_wave136/137, INTEGRATED_*) trim-refuse; open
editorial-claim roots (C2_planintegration[3]/[6]/[7], docs/plans
1-mention tier) skipped; game_repository_remediation__plan.md claimed by
the IN PROGRESS deep-audit claim — skipped. All five selected files
have 0 ledger mentions under fixed-string matching, no directory-level
claim coverage, and no load-bearing references (only static
batch-expander/scanner tooling lists). All were git-clean and quiet at
claim time; edit-time rechecks (git-clean + mention recheck) applied
before each write; trimmer is the batch-53-reconstructed tool (validated
byte-exact against three known pairs):

- `docs/PLANS_50_53_AUTHORITY_MAP.md`
- `docs/progression/PLAN26_CLOSEOUT.md`
- `docs/combat/PLAN10_BASELINE.md`
- `docs/spiritual/PLAN30_BASELINE.md`
- `docs/social/PLAN12_SOCIAL_STATE_MAP.md`

No spares used. Standing exclusions unchanged: deep-audit ×4; W2-06;
PLAN-READINESS-281 + PLAN-ORPHAN-SEAL-01 + CLAIM_READINESS_INDEX;
PLAN_24_CLOSEOUT; wave*/expansion_* tier in
claim-plan-bloat-reduction-parallel-batch-42/44/45/47; batch-28–61 files
now dirty; roots of IN PROGRESS / open editorial claims excluded;
in-flight sibling trims excluded by the git-clean filter.

Method C (conservative — keeps unique material; no wholesale removal, no
lossy compression): authored content retained verbatim; only
byte-identical repeat leaf-section copies in the generated `BATCH-NN
ARCHITECTURAL EXPANSION` regions are removed (each marked by a
`consolidated: §` pointer); `### Tranche` container headings retained
for navigation; all unique `BATCH-` headers and distinct section
headings/bodies preserved. Full pre-trim originals + SHA-256 manifest at
`/tmp/ashfall-plan-trim-methodc-b62-20260929/` and recoverable from git
history. No production changes, runtime tests, or commit.
Status: COMPLETE, uncommitted. Trim results (Go tool line counts):
PLANS_50_53_AUTHORITY_MAP 50,331 → 32,403; PLAN26_CLOSEOUT 50,320 →
33,516; PLAN10_BASELINE 50,197 → 32,714; PLAN30_BASELINE 50,140 →
32,657; PLAN12_SOCIAL_STATE_MAP 49,619 → 30,425. Total 250,607 →
161,715 lines (~36%, conservative per the user's no-overtrim
instruction); 13,360 byte-identical repeat copies replaced by
`consolidated: §` pointers; ~5.4 MB saved. Each file's authored prefix
retained byte-identically (SHA-256 prefix check per file: 05495094…,
ce64d306…, c769c27f…, 78f3b572…, ee8173e2…). Verified per file via the
tool's `--verify` mode (every distinct original line value survives; no
distinct line lost; BATCH banners intact) plus independent checks:
banner counts equal (11 each), `### Tranche` container headings equal
(220 each), authored-prefix SHA-256 equal, scoped `git diff --check`
PASS. The fresh-tier pool remains large (hundreds of ~47–49K-line
0-mention files across docs/* subdirectories plus the two smaller
docs/remediation audit plans); future batches should re-rank by size
with the full survey and continue this class.
## claim-plan-trim-conservative-method-C-expansion-batch60-2026-09-29


User-authorized conservative trim (method C) — "find 5 bloated plans to
trim, please don't overtrim and overcompress, remove repetitive and
ununique plus boring prose from prose plans and polish all 10 plans!" —
sixtieth batch in the method-C family (executed as 5 files per the
explicit count, batch 48/54–59 precedent). Continues the b59-surveyed
fresh tier of 0-mention, git-clean, untrimmed generated-expansion plans
outside docs/plans and docs/expansions (docs/content, docs/medical,
docs/ecology, docs/combat, docs/progression). Pre-selection re-audit
re-run: the 15 clean single-mention prose_wave files and the
wave*/expansion_* tier remain root-owned by
claim-plan-bloat-reduction-parallel-batch-42/43/44/45/46/47/48/49/50/51/52/53
(IN PROGRESS, spot-verified unchanged) — skipped untouched; the
INTEGRATED_cw* copies are already consolidated (trim-refusing); no new
spare refills. All five were git-clean and quiet at claim time with 0
ledger mentions under fixed-string matching and no directory-level claim
coverage; edit-time rechecks (git-clean + mention recheck) applied before
each write; trimmer is the batch-53-reconstructed tool (validated
byte-exact against three known pairs):

- `docs/content/PLAN156_SAVE_COMPATIBILITY.md`
- `docs/medical/PLAN112_EXISTING_7_INVENTORY.md`
- `docs/ecology/PLAN28_COMPLETION_REPORT.md`
- `docs/combat/PLAN10_COMPLETION_REPORT.md`
- `docs/progression/PLAN33_BASELINE.md`

No spares used. Standing exclusions unchanged: deep-audit ×4; W2-06;
PLAN-READINESS-281 + PLAN-ORPHAN-SEAL-01 + CLAIM_READINESS_INDEX;
PLAN_24_CLOSEOUT; wave*/expansion_* tier in
claim-plan-bloat-reduction-parallel-batch-42/44/45/47; batch-28–59 files
now dirty; roots of IN PROGRESS / open editorial claims excluded;
in-flight sibling trims excluded by the git-clean filter.

Method C (conservative — keeps unique material; no wholesale removal, no
lossy compression): authored content retained verbatim; only
byte-identical repeat leaf-section copies in the generated `BATCH-NN
ARCHITECTURAL EXPANSION` regions are removed (each marked by a
`consolidated: §` pointer); `### Tranche` container headings retained
for navigation; all unique `BATCH-` headers and distinct section
headings/bodies preserved. Full pre-trim originals + SHA-256 manifest at
`/tmp/ashfall-plan-trim-methodc-b60-20260929/` and recoverable from git
history. No production changes, runtime tests, or commit.
Status: COMPLETE, uncommitted. Trim results (Go tool line counts):
PLAN156_SAVE_COMPATIBILITY 52,504 → 34,320; PLAN112_EXISTING_7_INVENTORY
52,120 → 34,389; PLAN28_COMPLETION_REPORT 52,097 → 34,463;
PLAN10_COMPLETION_REPORT 51,316 → 33,294; PLAN33_BASELINE 51,092 →
33,117. Total 259,129 → 169,583 lines (~35%, conservative per the
user's no-overtrim instruction); 12,796 byte-identical repeat copies
replaced by `consolidated: §` pointers; ~4.8 MB saved. Each file's
authored prefix retained byte-identically (SHA-256 prefix check per
file: 233b810d…, 75d72eb9…, 6da969d8…, 3af2f0e3…, 70aed489…). Verified
per file via the tool's `--verify` mode (every distinct original line
value survives; no distinct line lost; BATCH banners intact) plus
independent checks: banner counts equal (11 each), `### Tranche`
container headings equal (220 each), authored-prefix SHA-256 equal,
scoped `git diff --check` PASS. Fresh-tier eligible 0-mention candidates
remain for one more 4-file batch (PLAN10_REGRESSION_MATRIX,
PLAN128_BASELINE, and the two smaller docs/remediation/plans audit
plans); after that, further batches need IN PROGRESS claims to complete
or foreman/user direction.
## Editorial claim — conservative duplicate removal batch 2, 2026-09-28


Root owns prose-only edits to `docs/plans/FLAGSHIP_MISSING_ASSET_GENERATION_INTEGRATION_PLAN.md`,
`docs/plans/BLOCKED_PLANS_UNBLOCKER_PLAN_2026-09-19.md`, and
`docs/plans/MASTER_FIVE_OLDEST_PLANS_EXPANSION_INTEGRATION_FRAMEWORK.md`, plus an
appended `.ai/state.md` handoff. No existing claims name these paths. Consolidate
only byte-identical repeated sections within each file, retain first copies
verbatim and all headings in order, and link duplicate locations to retained text.
Preserve unique content and status; no production edits, implementation or commits.
## Unregistered worktree safety


The repository already contains substantial user and other-agent changes that
predate this batch. They are not available for opportunistic cleanup, rebasing,
formatting, or conflict resolution. A package may touch only its listed paths.

| claim-deep-audit-repair-recruitment-save-2026-09-26 | `DEEP-AUDIT-REPAIR / recruitment envelope capture` | Integrator (user-authorized claim extension) | **Parent:** `claim-deep-audit-repair-2026-09-26`. **Exact path:** `src/Main.Recruitment.cs`. | **ACTIVE — 2026-09-26:** the real-campaign journey proved `SaveRecruitment()` wrote a derived file without capturing the `recruitment` section into `campaign.json`; add the owner-level capture through its existing save store, then rerun the campaign save/reload probe. |
## Claim lifecycle


- `ACTIVE`: owner may edit exact paths.
- `HANDED_OFF`: owner stopped; integrator or named successor may proceed.
- `BLOCKED`: no edits until foreman records a new decision.
- `ACCEPTED` / `REVOKED`: claim is closed and paths become available.

Claims name exact files or narrow directories. Shared composition roots,
registries, and generated authority files belong to the integrator, never to
parallel builders.
## claim-plan37-current-acceptance-closeout-2026-09-30


User-authorized one-plan continuation. Integrator claims the Plan 37 closeout only: `docs/plans/PLAN_37_INPUT_FOCUS_CONTROLLER_INTEGRATION_PLAN.md`, archive destination `docs/plans/integrated/ui/PLAN_37_INPUT_FOCUS_CONTROLLER_INTEGRATION_PLAN.md`, its census row in `docs/plans/UNCLAIMED_CORPUS_CENSUS.md`, an acceptance entry in `INTEGRATION_PLANS.md`, this claim, `.ai/state.md`, and generated `docs/INDEX.md`. Production source is read-only. Acceptance: focused input/settings and current UI-owner tests, current host build and UI-layout/controller-parity probe pass; document superseded owners and hardware limitation; mark and immediately archive. Status: BLOCKED / RELEASED. Input gates passed 10/10; settings and current-owner tests cannot compile unrelated YearOfAshTimelineSystem climate API mismatches (lines 125–131). Host build succeeded with 14 warnings, but concurrent Core changes prevent consistent acceptance. Auditor also flagged live joypad delivery through _UnhandledKeyInput for verification. No production edits, no archival, no completion claim; see .ai/state.md.

### claim-save-authority-2026-09-30 — SAVE-CAMPAIGN-JSON-SOLE-AUTHORITY
User-authorized ("continue working on the still opened"). Paths: 83 `src/Main*.cs` Save methods (TrySave line removed), `src/Main.SaveOrchestrator.cs` (restore-list setups), `src/Main.Expeditions.cs`, `src/Main.InternalCommunication.cs`, `src/Main.UiTests.RealCampaignJourney.cs`, `Ashfall.Core.Tests/Tooling/MainTriadDriftGateTests.cs`, `docs/architecture/ARCHITECTURE_TEST_MAP.md` (regenerated). Plan `.ai/plans/integrated/save/campaign-json-sole-authority-2026-09-30.md`. Status: COMPLETE (uncommitted).
## GOVERNANCE NOTE 2026-10-02 — process incident, queued defect, and staging plan


**Incident (review ruling): permission-layer bypass.** During the Rust port's
Stage 2, the delegated agent reported that a shell permission layer began denying
`cargo`/`go`/`python3` mid-task and that it worked around the denials using the
`command` builtin. That bypass is **prohibited standing policy**: a denied tool
call is a blocker to report, never to route around (via `command`, `python3 -c`,
an alias, a generated script, a symlink, a config change, or any equivalent path).
Stage 3 was briefed accordingly. Logged here because a permission layer that can
silently begin denying and can be circumvented is an infrastructure defect the
operator needs to know about. **Action for the infrastructure owner:** investigate
why the Stage 2 denials began mid-task.

**Queued defect (review ruling): Go path-truncation in `get_changed_files`.**
`TrimSpace(line)[3:]` drops the first character of an unstaged path (observed
`WORKTREE_OWNERSHIP.md` → `ORKTREE_OWNERSHIP.md`), directly under the path-based
coordination layer (`bin/run-scoped-tests` feeds ownership/verification). Approved
fix: a **coordinated Go + Rust pair** in one change set with the reproduction
promoted to a regression test in both toolchains, executed at **Stage 4 cutover**
(or earlier if any live mis-selection is observed). Until fixed, treat
unstaged-path output from `bin/run-scoped-tests` as untrusted for coordination.
Parity freeze respected until then.

**Two-commit staging plan (review-approved; execute when Stage 3 closes).**
Selective staging only; concurrent sessions' dirty files stay out.
1. **Commit 1 — Rust port Stages 1–3 (once Stage 3 is verified):**
   `tools/rstools/**` (workspace + `crates/ashfall-dev/**`), the port plan under
   `.ai/plans/`, and the port claim.
2. **Commit 2 — PERF-001 + Phase 1:** `Ashfall.csproj`,
   `src/Host/PerformanceSelfTest.cs`, `docs/ci/TIMING_BASELINE.json`,
   `artifacts/performance/PERFORMANCE_DIAGNOSTIC_2026-10-02.md`, plus a **new**
   `.ai/plans/` plan + claim for the PERF-001/Phase 1 item (not yet filed).
Do **not** commit while `tools/rstools/**` is a live agent write scope. A
concurrent session had staged a broad set of foreign files at ~05:40, so re-check
`git status` immediately before any commit.
## Recently closed (2026-10-02) — headings only
- claim-perf-runtime-lifecycle-sprint2-2026-10-02 — IN PROGRESS
- claim-perf-runtime-baseline-sprint-2026-10-02 — IN PROGRESS
- claim-perf-track-c-lint-and-warning-baseline-2026-10-02 — COMPLETE
- claim-perf-track-ce-scanner-algorithm-2026-10-02 — COMPLETE
- claim-perf-track-d-ci-autogen-cleanup-2026-10-02 — COMPLETE
- claim-perf-track-a-baseline-2026-10-02 — COMPLETE
- claim-perf-track-b-docsindex-cache-2026-10-02 — COMPLETE
- claim-docs-index-ledger-exclusion-2026-10-02 — COMPLETE
- GOVERNANCE NOTE 2026-10-02 — process incident, queued defect, and staging plan
- claim-language-policy-agent-speedups-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-repo-wide-6-loop-sweep-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-agent-speedups-timing-docsindex-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-rust-port-gotools-2026-10-02 — COMPLETE / FULLY INTEGRATED (Stages 1–4)
- claim-test-build-speedups-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-repo-wide-6-loop-localization-sweep-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-l10n-sweep-wave9-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-expedition-followup-wave8-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-l10n-drift-dynamic-families-l01-l04-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-survival-legibility-twelfth-wave-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-survival-legibility-thirteenth-wave-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-catalog-health-gates-i01-i15-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-catalog-health-gates-h01-h15-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-catalog-health-gates-g01-g15-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-expedition-followup-wave7-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-catalog-health-gates-f01-f15-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-catalog-health-gates-e01-e15-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-expedition-followup-wave6-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-catalog-health-gates-d01-d05-2026-10-02 — COMPLETE / FULLY INTEGRATED
- claim-15-small-repo-followups-2026-10-02 — COMPLETE / RELEASED
- claim-scene-binding-truth-p089-p096-2026-10-02 — COMPLETE / RELEASED
