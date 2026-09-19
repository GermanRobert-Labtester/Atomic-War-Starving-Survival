# Plan 53 / E1 — Ambition Governance & Expansion Intake: Integration Plan

> **Package:** `E1` / Plan 53 — "Ambition Governance & Intake" (census row 114, `docs/plans/UNCLAIMED_CORPUS_CENSUS.md`)
> **Census status at authoring:** `READY-UNCLAIMED` (Wave 10 Part 2, Task F1 classification; prerequisite Plan 29 `SEALED` 2026-09-17 under `claim-wave10-part1-b2-plan29-one-truth-2026-09-17`)
> **Source documents (authoritative, read in full):**
> 1. `Next-steps-plans/shipped_to_chat/Plan_53_Ambition_Audit_Expansion_Intake.md` — the original audit concept (Tasks 53A/53B/53C, premise re-verified there at `ccac926e`)
> 2. `C-integration-plans/E1_planintegration.md` — the corpus adaptation (front matter `PLAN_ID: E1`, `STATUS: READY_FOR_EXECUTION`, `CATEGORY: PROCESS+LINK+GOVERNANCE`, `RUNTIME_RISK: LOW`, `GOVERNANCE_RISK: HIGH`, `NO_HISTORY_DELETION: true`), which expands the concept into the 16-item programme **E1A–E1P** plus follow-on queue E2–E8
> **Plan type:** governance/process capability. No gameplay mechanic, no Core simulation change, no Godot scene or panel, no game data catalog, no save section. Deliverables are authored metadata, generated registers, and static Python CI tooling under `scripts/ci/`.
> **Author of this document:** ashfall-plan role (planning only — zero production change in this deliverable)

---

# 1. Objective

Convert ASHFALL's accumulated plan corpus — three numbering namespaces, no universal status markers, demonstrably stale premises, and repeated duplicate-capability proposals — into an evidence-gated, machine-readable, execution-oriented governance pipeline, exactly as scoped by the E1 corpus document's sixteen sub-items E1A–E1P.

Concretely, at the end of this package the repository must be able to answer, from generated data rather than tribal knowledge:

1. **What exists** — one generated plan register (`docs/roadmap/PLAN_REGISTER.md` + `PLAN_REGISTER.json`) covering every in-scope plan in all three namespaces with stable identity, status, category, premise freshness, rails, and overlap cluster.
2. **What is still true** — an automated premise/dead-reference verifier that flags any plan whose cited `file[:line]` or capability evidence no longer exists, sharing one reference parser with the existing Plan 29 claims verifier.
3. **What already exists in pieces** — reviewed duplicate-topic clusters mapping proposed scope onto live Core/host authorities, so new work becomes extension tickets instead of parallel systems.
4. **What belongs** — a short, falsifiable `docs/design/PILLARS.md` plus a published ambition rubric, applied to the whole backlog in one recorded triage (`NOW` / `NEXT` / `MERGE` / `NOT_NOW` / `DROP` / `NEEDS_PREMISE_REVIEW`).
5. **What is ready** — a generated rails readiness registry (`docs/roadmap/RAILS.md` + `rails.json`) whose states derive from authoritative plan metadata, never hand-maintained prose.
6. **What may start** — an executable intake checker (`scripts/ci/plan-intake-check.py` + `.sh` wrapper) that refuses `SYSTEM`/panel/resource additions lacking a duplicate-search receipt, ready rails, named authority, metric, and acceptance target.
7. **What is next** — a one-page `docs/roadmap/ROADMAP.md` with a hard-capped `NOW` band, dependency-ordered, validated against the register.

**Bounded outcome:** the sixteen E1 sub-items delivered in dependency order with the staged CI adoption path (report-only → changed-file enforcement → full enforcement), zero runtime behavior change, and all pre-existing gates green at every phase boundary.

**Non-goals (explicit, binding):**

- No gameplay system, panel, resource type, catalog, or save schema is created by this package (E1 §3 guardrail: "Do not introduce gameplay systems while implementing E1").
- No plan file is deleted; superseded/dropped/merged plans are marked, never erased (`NO_HISTORY_DELETION: true`).
- No historical plan is renumbered; numbering reconciliation is a published policy, not a rename wave.
- No second project-management database: the register is generated from front matter; it is never an independently editable source of truth.
- No player-facing surface of any kind (a player-facing "wishlist" would be a separate, out-of-scope stretch proposal requiring its own intake — see §22).
- Follow-on execution queue E2–E8 (register-backed execution wave, merge-cluster consolidation, premise remediation, zero-consumer conversion, panel authority hardening, acceptance-ladder enforcement, quarterly review) is **specified but not executed** by this package.

---

# 2. Current Reality

All facts below were re-verified against the working tree on 2026-09-19, per project rule 7 (a plan or audit name is not proof of current state). Where the 2026-09-19 measurement contradicts the source plan's `ccac926e`-era numbers, the current number governs and the old number is retained as historical premise.

## 2.1 Corpus shape at HEAD (re-measured)

| Namespace | Source-plan claim (@ `ccac926e`) | Re-measured 2026-09-19 | Front matter with `PLAN_ID:` |
|---|---|---|---|
| `Next-steps-plans/` `Plan_*.md` | 115 | **230** | **0 / 230** |
| `piagentsplans/` `*.md` | 132 | **134** | **0 / 134** |
| `C-integration-plans/` `*.md` | (census: 131 `*planintegration*` files) | **245** total `.md` (131 are the census-scoped corpus chain files) | **29 / 245** — the E1/C1/C2/D1 flagship chain heads carry YAML front matter already (e.g. `E1_planintegration.md` itself) |
| `docs/` `*.md` | 119 | **1,786** | n/a (not plan authority) |

Consequences that shape this plan:

- The source premise "no `STATUS:`/`PREMISE_VERIFIED_AT` front matter anywhere" is **partially stale**: the 29 corpus chain files already carry front matter. Metadata migration (E1C) therefore has a two-tier scope — corpus files need schema *completion* (their existing keys preserved), while `Next-steps-plans/` and `piagentsplans/` need full insertion.
- The counts have drifted upward since the source audit, which is itself evidence for the intake problem and a mandate for E1A: all E1 metrics must be expressed as deltas from a *fresh* baseline captured at execution time, never quoted from the source document.
- `docs/` is indexed by the existing `generate-docs-index.py` (2,473 documents per Wave 10 Part 2 closeout) — plan governance must not build a second docs index.

## 2.2 Governance machinery already sealed by Plan 29 (the collision surface)

Plan 29 / C1[7] "One Truth" is `SEALED` (2026-09-17, `docs/plans/wave10_part1/B2_PLAN29_IMPLEMENTATION_LOG.md`). Its live deliverables are the exact machinery Plan 53 must **feed, not duplicate**:

| Asset | Path | Role | Verified state |
|---|---|---|---|
| Roadmap governance spec | `docs/roadmap/README.md` | Flow-of-truth pipeline, numbering policy (`<100` continuity, `>=100` expansion, `piagentsplans/00–129` historical-only), collision/reservation rules, binding Definitions of Done (§3.1 System, §3.2 Content, §3.3 UI, §3.4 Build, §3.5 Plan) | Live; cited by `AGENTS.md` |
| Wave ledger | `docs/roadmap/WAVE_LEDGER.md` | Waves 1–10 status table (`executed`/`in-flight`/`proposed`/`superseded`/`historical`) | Live; last row still lists Wave 10 Part 2 as `proposed` although its closeout is `COMPLETE` — a real, current example of hand-maintained status drift that E1's generated-status rule is designed to eliminate |
| Capability claims registry | `docs/architecture/CLAIMS.json` | 24 machine-verified capability claims with `claim_id`, `status` (`TRUE`/`PARTLY_TRUE`/`STALE`/`UNVERIFIABLE`/`FALSE`/`SUPERSEDED`), `confidence` (`PROVEN_RUNTIME`…`UNVERIFIED`), typed evidence paths, test paths, gate IDs | Live; 24/24 valid at last gate run |
| Claims verifier | `scripts/ci/verify-capability-claims.py` | `--check` gate validating schema, evidence path existence, test path existence, gate IDs against the CI manifest | Live; registered gate `capability_claims` family |
| CI gate manifest + runner | `docs/ci/CI_GATE_MANIFEST.json` (53 gates), `scripts/ci/run-gates.py`, `scripts/ci/verify-fast.sh` | Fast-tier gate execution with JSON report + failure artifact | Live; 48/48 fast gates PASS at Wave 10 Part 2 closeout |
| Docs index | `scripts/ci/generate-docs-index.py` → `docs/INDEX.md` | Deterministic generated index with `--check` drift gate | Live |
| Rulebook sync | `scripts/ci/sync-agent-rulebooks.py` | 13 client rulebooks byte-derived from `AGENTS.md` | Live; gate `agent_rulebooks_sync` |
| Corpus duplicate detector | `scripts/ci/detect-corpus-duplicates.py` | Advisory C1/C2 identity-collision reporter vs. historical closeouts; explicitly human-adjudicated, census-recorded | Live (Wave 11 Part 2 B5 tooling) |
| Census | `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` | 131-file corpus inventory, 73-edge DAG, per-row status; row 114 is this package | Live |
| Decision register | `docs/governance/DECISION_REGISTER.md` | 20 standing decisions with terminal verdict vocabulary `SIGNED` / `DECLINED` / `DEFERRED-WITH-CONDITION` / `RETIRED`, plus owner, condition, evidence, recheck trigger | Live; reviewed at each wave closeout |
| Ledgers | `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `KNOWN_DEBT.md`, `TEST_POLICY.md` | Queue authority, path claims, debt, test selection | Live |

**None of Plan 53's own deliverables exist yet** (verified by directory listing 2026-09-19): no `scripts/ci/generate-plan-register.py`, no `scripts/ci/plan-intake-check.*`, no `docs/roadmap/PLAN_REGISTER.*`, `INTAKE.md`, `RAILS.*`, `ROADMAP.md`, `AMBITION_AUDIT.md`, `AMBITION_RUBRIC.md`, `PREMISE_HEALTH.md`, `CAPABILITY_CLUSTERS.md`, `GOVERNANCE_METRICS.*`, no `docs/roadmap/e1/`, no `docs/design/PILLARS.md`. `docs/roadmap/` contains exactly `README.md` and `WAVE_LEDGER.md`.

## 2.3 The Plan 53 identity collision (must be recorded, not "fixed")

The number 53 currently names **three different subjects** in the repo:

1. `docs/PLANS_50_53_AUTHORITY_MAP.md` — "Plan 53: Acoustic Director" (`ShelterAcousticDirector`, `shelter_audio_cues.json`), implemented.
2. `docs/PLANS_51_54_INTEGRATION_REPORT.md` — "Plan 53" = economy price explanation (`MarketSystem.ExplainPrice`), implemented.
3. `Next-steps-plans/shipped_to_chat/Plan_53_Ambition_Audit_Expansion_Intake.md` + corpus `E1` — **this** package (governance), `READY-UNCLAIMED`.

The census already adjudicated the governance chain's canonical identity (`E1` → "Plan 53 — Ambition Audit & Expansion Intake", row 114). This package does **not** rename or renumber anything (roadmap README §2: "No Silent Renumbering"); it records the collision in the register as a `numbering_drift` finding, exactly as the census protocol does for the other 39 known collisions. The register's stable-`PLAN_ID` identity model (metadata, not filename) is the structural fix; the historical filenames remain untouched.

## 2.4 Active claims touching shared governance paths (race check per rule 6)

From `WORKTREE_OWNERSHIP.md` at authoring time:

- `claim-xp-wave1-difficulty-2026-09-18` (ACTIVE) holds `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`, and its own decision packet.
- `claim-wave11-part2-execution-2026-09-18` (ACTIVE) holds `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`, `KNOWN_DEBT.md`, `docs/governance/DECISION_REGISTER.md`, and the census.

Therefore every edit this package needs to a shared governance file (`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, census row 114, `CI_GATE_MANIFEST.json`, `CLAIMS.json`, `docs/roadmap/README.md`, `AGENTS.md`) is an **integrator-mediated handoff**, sequenced at phase boundaries when those claims are `DONE`/released — never a concurrent builder edit. All package-owned paths (new files under `scripts/ci/`, `docs/roadmap/`, `docs/design/`, fixtures, tests) are disjoint from every active claim.

## 2.5 Verification baseline at authoring

Wave 10 Part 2 closeout evidence: fast tier 48/48 PASS; `verify-capability-claims.py --check` 24/24; docs index 2,473 documents clean. Subsequent waves (11 Parts 1–2) kept gates green per their closeouts. E1A will re-run and re-record this as the formal pre-migration baseline, distinguishing inherited-red from E1-caused-red before any new gate exists.

---

# 3. Required Delta

From the current reality to the objective, the minimum complete delta is:

1. **Metadata delta:** a canonical plan front-matter schema (schema-versioned), present on every in-scope plan file across all three namespaces, with author-owned fields separated from generated fields, inserted without touching plan bodies.
2. **Generation delta:** one register generator producing `PLAN_REGISTER.md` (presentation) + `PLAN_REGISTER.json` (substrate) with `--check`/`--write`/`--json`/`--explain`/`--self-test`, deterministic and side-effect-free in check mode.
3. **Freshness delta:** a shared reference parser classifying every `file[:line]` citation as `OK` / `MISSING_PATH` / `MISSING_LINE_RANGE` / `RENAMED_CANDIDATE` / `AMBIGUOUS` / `EXTERNAL`, consumed by the register, the intake checker, and (via a flagged shared-seam migration) the Plan 29 claims verifier.
4. **Judgement delta:** `docs/design/PILLARS.md` + `docs/roadmap/AMBITION_RUBRIC.md`, then one recorded whole-backlog triage (`docs/roadmap/AMBITION_AUDIT.md`) whose verdicts cite evidence and name deciders, and whose `MERGE` verdicts name the live authority to extend.
5. **Readiness delta:** a rails registry (authored rows, generated table) with explicit readiness levels, unknown-rail rejection, and dependency-cycle detection.
6. **Enforcement delta:** an intake checker that can refuse (missing rails, missing duplicate-search receipt, panel-without-authority, missing metric, expired spike/waiver), introduced through the five-stage rollout (report-only → full active-plan governance) so an inherited-red corpus never becomes permanent CI noise.
7. **Ledger delta:** census row 114 status progression (`READY-UNCLAIMED` → claimed → `SEALED`), a claim row in `WORKTREE_OWNERSHIP.md`, a package entry in `INTEGRATION_PLANS.md`, one additive `CLAIMS.json` capability claim at closure, and a `WAVE_LEDGER.md` row — all integrator-mediated.
8. **Zero gameplay delta:** no change under `Assets/Ashfall.Core/`, `src/`, `Assets/StreamingAssets/Data/`, `project.godot`, or any save store. Proven per phase by the existing build/selftest gates, not by assertion.

---

# 4. Evidence

Primary evidence anchors for every design decision in this plan (all paths repo-relative, verified present 2026-09-19 unless marked historical):

| # | Evidence | What it proves for this plan |
|---|---|---|
| E-1 | `Next-steps-plans/shipped_to_chat/Plan_53_Ambition_Audit_Expansion_Intake.md` (full text: Tasks 53A/53B/53C, 12/13/13 substeps, cross-task dependency `29A → 53A → 53B → 53C`, guardrails) | The concept scope: register, audit, intake; "vaccine, not another feature"; the four-bin triage; the five-question rubric; the named rails list; the anti-bureaucracy guardrail |
| E-2 | `C-integration-plans/E1_planintegration.md` (front matter + §0–§19: E1A–E1P, data contracts §6, state machine §7, command matrix §8, test matrix §9, rollout §10, rollback §11, risk register §12, evidence classes §13, definitions §14–16, handoff §17, checklist §18) | The authoritative decomposition this plan implements; its 18 programme outcomes (§2) become this plan's acceptance surface |
| E-3 | `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` row 114 + §head | Package status `READY-UNCLAIMED`; prerequisite Plan 29 sealed; "Full governance programme (E1A–E1P); standalone roadmap track"; standard claim protocol applies (no special foreman signature named) |
| E-4 | `docs/plans/wave10_part2/WAVE10_PART2_CLOSEOUT.md` §2 Task F1 | The classification event: E1 = Plan 53, category `PROCESS+LINK+GOVERNANCE`, census row updated, chain heads unblocked |
| E-5 | `docs/roadmap/README.md` §2–§4 | Numbering/collision rules this package must obey; binding DoD classes this package's own "Plan Done" (§3.5) must satisfy; §4 names `CLAIMS.json` + verifier as the capability-claim authority (so the register must not become a second one) |
| E-6 | `docs/roadmap/WAVE_LEDGER.md` | Wave status taxonomy; the stale `proposed` row for Wave 10 Part 2 is a live drift specimen motivating generated-over-authored status |
| E-7 | `docs/architecture/CLAIMS.json` + `scripts/ci/verify-capability-claims.py` (read in full) | The exact CI-gate pattern to mirror: schema enums, duplicate-ID rejection, on-disk evidence existence checks, manifest gate-ID validation, `--check` exit semantics, human-readable report block. E1's shared reference parser is designed against this file's evidence model |
| E-8 | `docs/ci/CI_GATE_MANIFEST.json` (53 gates; fields `gate_id`/`name`/`category`/`command`/`timeout_seconds`/`expected_summary`/`classification`/`critical`) + `scripts/ci/verify-fast.sh` → `run-gates.py --tier fast` | The registration contract for the two new gates; supports `critical` flag → report-only staging is expressible today without runner changes |
| E-9 | `docs/governance/DECISION_REGISTER.md` (20 decisions, §2 verdict definitions, §3 cadence) | The verdict vocabulary (`SIGNED`/`DECLINED`/`DEFERRED-WITH-CONDITION`/`RETIRED`) the triage lifecycle mirrors; `DEFERRED-WITH-CONDITION` already requires "named blocking condition, execution package, and recheck trigger. Never indefinite." — the model for `NOT_NOW` return conditions |
| E-10 | `scripts/ci/detect-corpus-duplicates.py` (read) | Existing advisory-only duplicate detection with human adjudication into the census; E1E's clustering extends this pattern (confidence + evidence paths + review-required) rather than inventing auto-adjudication |
| E-11 | `TEST_POLICY.md` (read in full) | Test-selection rules honoured in §18: focused targets, `scripts/run_test.sh` 180 s cap, new tests only for new contracts, no full-suite default |
| E-12 | `WORKTREE_OWNERSHIP.md` active claims (XP-WAVE1, WAVE11-PART2) | The shared-governance-path race surface handled in §2.4 and §7 |
| E-13 | `docs/PLANS_50_53_AUTHORITY_MAP.md` + `docs/PLANS_51_54_INTEGRATION_REPORT.md` | The Plan 53 numbering collision recorded in §2.3; proof that filename/number identity is unreliable and stable `PLAN_ID` metadata is required |
| E-14 | Re-measured counts (2026-09-19): 230 / 134 / 245 / 1,786; 29 corpus files with `PLAN_ID:` front matter; 0 in the two legacy namespaces | §2.1; supersedes the source plan's `ccac926e` numbers as execution baseline input |
| E-15 | `docs/debug/10LOOP_player_ui_ux_BUG_AUDIT.md` BUG-UI-002 (cited by source plan) | The fake-affordance failure mode (30 routed consoles before authority existed) that E1J's panel-authority intake rule operationalizes at plan time |
| E-16 | `AGENTS.md` "ACTIVE QUEUE" item 6 | This package is listed as available, unexecuted, no new foreman signature needed; entry gate is the standard census claim protocol |

**Premise-verdict on the source documents:** current-valid in structure (E1A–E1P decomposition, contracts, state machine, rollout, risks all still applicable and unimplemented); partially stale in numbers (E-14) and in the "no front matter anywhere" claim (29 corpus files carry it). Both staleness findings are absorbed by E1A's fresh-baseline mandate and E1C's two-tier migration design.

---

# 5. Existing Extension Seams

EXTEND-over-DUPLICATE mapping: every E1 function attaches to an existing owner. Nothing in this list is re-created.

| E1 need | Existing seam to extend | How it is consumed |
|---|---|---|
| Capability truth ("what systems exist and are proven") | `docs/architecture/CLAIMS.json` + `verify-capability-claims.py` (Plan 29B) | E1E clustering and the intake duplicate-search receipt *query* the claims registry and the architecture map; the register links `claim_id`s, never re-asserts capability truth |
| Numbering, collision, reservation policy | `docs/roadmap/README.md` §2 (Plan 29C) | E1K publishes its E-series/folder-ownership rules as *additive subsections* here; the register's `numbering_drift` finding type implements §2's collision-handling rule mechanically |
| Wave status truth | `docs/roadmap/WAVE_LEDGER.md` (Plan 29C) | E1M's roadmap links wave IDs; E1N metrics read the ledger; the ledger stays hand-authored for waves (its scope) while plan-level status becomes generated (register's scope) — boundary documented in README to prevent overlap |
| Corpus identity/classification | `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` + `detect-corpus-duplicates.py` | Register ingests corpus front matter (the 29 files that have it) and census statuses as `SOURCE_AUTHORITY` evidence; `detect-corpus-duplicates.py` output feeds E1E as one candidate-cluster source |
| Decision truth | `docs/governance/DECISION_REGISTER.md` | Triage verdicts that bind scope (`DROP`, `NOT_NOW`, portfolio `MERGE`s) are recorded as decision-register entries by the integrator; the audit document cites `DEC-xx` IDs; verdict vocabulary is mirrored, not redefined (§9) |
| Queue and claims | `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md` | Standard rows added by foreman/integrator at claim and seal; intake checker's `OWNER` field cross-references claim IDs |
| CI execution | `docs/ci/CI_GATE_MANIFEST.json` + `run-gates.py` + `verify-fast.sh` | Two new gate rows (register drift; intake check) added with `classification: fast`, `critical: false` during rollout Stages 0–1, promoted to `critical: true` at Stage 2+ |
| Docs index | `generate-docs-index.py` | New `docs/roadmap/*` and `docs/design/PILLARS.md` are picked up by the canonical generator; its `--check` proves no index drift; no E1 code touches it |
| Rulebooks | `sync-agent-rulebooks.py` | Exactly one additive pointer line in `AGENTS.md` (task workflow → `docs/roadmap/INTAKE.md`), landed at rollout Stage 1, then the canonical sync regenerates all 13 clients; engine-invariant sections untouched byte-for-byte |
| Test runner | `scripts/run_test.sh` | All xUnit contract tests run through it; Python fixture self-tests run as CI gates (not via dotnet) |
| Architecture truth | `scripts/ci/generate-architecture-map.py` → `docs/architecture/ARCHITECTURE_TEST_MAP.md` | E1E's code-authority index consumes the generated map's node list as its system-name source of record |

Explicit non-seams (things that must NOT be built): a second capability registry, a second docs index, a second decision log, a second numbering policy document, a database/server/service, any in-game surface.

---

# 6. Proposed Architecture

## 6.1 Layer model

Three layers, mirroring the project's data/code separation discipline (authored authority → generated substrate → presentation):

```text
AUTHORED AUTHORITY (hand-maintained, minimal, reviewable)
  Plan file front matter (YAML block at head of each in-scope plan .md)
  docs/design/PILLARS.md                      (4–5 falsifiable pillars + non-goals)
  docs/roadmap/AMBITION_RUBRIC.md             (scoring + hard-stop rules)
  docs/roadmap/INTAKE.md                      (the policy, human text)
  docs/roadmap/rails.registry.json            (authored rail definitions ONLY)
  docs/roadmap/e1/ (baseline inputs, migration review queues, decision records)

GENERATED SUBSTRATE (machine-written, --check enforced, never hand-edited)
  docs/roadmap/PLAN_REGISTER.json             (integration substrate)
  docs/roadmap/rails.json                     (resolved rail readiness)
  docs/roadmap/governance_metrics.json

GENERATED PRESENTATION (Markdown views of the substrate)
  docs/roadmap/PLAN_REGISTER.md
  docs/roadmap/RAILS.md
  docs/roadmap/PREMISE_HEALTH.md
  docs/roadmap/CAPABILITY_CLUSTERS.md         (generated candidates + recorded human verdicts)
  docs/roadmap/ROADMAP.md                     (validated against register; may carry a small authored header)
  docs/roadmap/GOVERNANCE_METRICS.md

TOOLING (static analysis only; never imports game runtime; never mutates outside --write)
  scripts/ci/plan_corpus_lib.py               (shared: front-matter parse, schema validate, reference parse/classify)
  scripts/ci/generate-plan-register.py        (register + freshness + rails resolution + metrics)
  scripts/ci/plan-intake-check.py             (intake validation; the enforcement gate)
  scripts/ci/plan-intake-check.sh             (thin wrapper, symmetry with other gates)
  scripts/ci/migrate-plan-metadata.py         (one-time metadata insertion; archived after E1C)
  scripts/ci/fixtures/plan_governance/        (versioned fixture corpus for --self-test)

TESTS
  scripts' --self-test modes (Python, fixture-driven, run as one fast-tier CI gate)
  Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs
      (xUnit static contract tests over the GENERATED JSON artifacts only —
       schema_version, enum conformance, no duplicate PLAN_ID, WIP cap, rail
       reference integrity — mirroring how PortContractGateTests validates
       generated policy JSON without executing the generator)
```

Every generated file carries the standard banner (`<!-- GENERATED BY scripts/ci/generate-plan-register.py — DO NOT EDIT -->` / JSON `"generated_by"` field), matching the existing `CLAIMS.json`/`INDEX.md` convention.

## 6.2 Identity model

- Canonical identity is the `PLAN_ID` front-matter value (e.g. `E1`, `C1[10]`, `53A`, `131`), never the filename. The register indexes `plan_id → {path, namespace, historical_filenames[]}`.
- Filenames and paths are preserved byte-for-byte; archival moves (E1K) record `LEGACY_SOURCE_PATH` so links survive.
- Duplicate active `PLAN_ID` = register error (E1K substep 11). The known Plan 53 three-way collision (§2.3) is represented as one register row per real subject with distinct `PLAN_ID`s (`53-ACOUSTIC` historical, `53-ECONOMY` historical, `E1` active) and a `numbering_drift` note — demonstrating the mechanism on a live case without renaming anything.

## 6.3 Why Python static tooling (and not Core C#)

The deliverable is repository-meta tooling. Project rule 2 keeps `Assets/Ashfall.Core/` engine-free and domain-pure; governance scanning is neither domain logic nor engine code, and the entire existing governance toolchain (`verify-capability-claims.py`, `generate-docs-index.py`, `detect-corpus-duplicates.py`, `sync-agent-rulebooks.py`) is Python under `scripts/ci/`. Placing the register/intake logic in C# would force the test suite to shell out to Python anyway and would put a non-gameplay authority inside the domain assembly — both violations. xUnit's role is confined to *static contract assertions over generated artifacts*, consistent with existing gate tests (`PortContractGateTests`, `CatalogPathForbiddenGateTests`).

## 6.4 The sixteen sub-items, enumerated and designed

The corpus document's E1A–E1P are the mandatory decomposition (census row 114 records the exact chain). Each sub-item below gets its own current-reality / delta / design / acceptance treatment. (Phase mapping and ordering are in §19.)

---

### E1A — Preflight, repository freeze, and baseline evidence capture

- **Current reality:** No baseline artifact exists. Counts re-measured for this plan (§2.1) are ad-hoc; the docs/wave gates' current pass state is known from closeouts but not captured as an immutable pre-migration snapshot.
- **Required delta:** A reproducible snapshot: HEAD SHA, branch, dirty state, toolchain versions (dotnet SDK, Godot, Python), UTC timestamp; explicit include-pattern config; raw file lists per namespace; pre-existing gate results (so E1 never claims or inherits credit/blame incorrectly).
- **Design:** `migrate-plan-metadata.py enumerate` (or a small `--baseline` mode of the register generator) writes `docs/roadmap/e1/E1_BASELINE.md` (human) + `docs/roadmap/e1/e1_baseline.json` (machine). Include patterns live in a versioned config (`scripts/ci/plan_governance_config.json`) so folder assumptions are never hard-coded. Baseline file lists must be byte-stable across two consecutive runs on unchanged HEAD (sorted, LF, UTF-8, no timestamps inside lists). The config also carries the exclusion list (archived vendor docs, caches, `docs/archive/`, generated outputs).
- **Acceptance:** baseline file list byte-stable on re-run; MD and JSON counts agree; inherited gate failures (if any) marked `inherited: true` with the failing command; zero runtime source files touched; smoke test proves stable enumeration ordering.

### E1B — Canonical plan metadata schema and generated register

- **Current reality:** 29/245 corpus files have front matter; 0/230 `Next-steps-plans` and 0/134 `piagentsplans` files do. No schema, no register, no generator.
- **Required delta:** The schema (§10.1), the parser with legacy tolerance (missing front matter → structured `METADATA_MISSING` warning, never a crash), the generator, both output artifacts, four modes.
- **Design:** `generate-plan-register.py` builds the register sorted by execution relevance (active → blocked → stale → archived), with per-row generated fields: reference health, premise-SHA age (days, computed from git, labelled review-signal-only), rails resolution summary, overlap cluster ID, path. `PLAN_REGISTER.json` is the substrate consumed by CI/tests; the `.md` is presentation only. Errors are actionable: `path: PLAN_ID field STATUS invalid value 'mostly done' (allowed: PROPOSED, PREMISE_STALE, READY, IN_PROGRESS, BLOCKED, DONE, MERGED, NOT_NOW, DROPPED, SUPERSEDED, ARCHIVED)`. Schema carries `REGISTER_SCHEMA_VERSION` for deliberate future migration. `--check` is deterministic and side-effect-free; `--write` regenerates; `--json` prints substrate; `--explain <plan-id>` prints one plan's parsed record, generated fields, and every validation verdict with reasons.
- **Acceptance:** idempotent generation (write→check → exit 0); stable sort independent of filesystem order; legacy fixture → structured warning; invalid enum → precise diagnostic naming file/field/value/allowed; supersedence cycle detected and reported with the cycle members; `--check` non-zero on drift, zero when clean.

### E1C — Corpus-wide metadata migration without rewriting history

- **Current reality:** Two-tier corpus (§2.1): corpus chain needs schema completion; legacy namespaces need full insertion. No migration tooling.
- **Required delta:** Body-preserving front-matter insertion across all in-scope plans, with confidence-tiered field inference and an explicit human review queue for anything ambiguous.
- **Design:** `migrate-plan-metadata.py` (one-time; marked non-routine and archived after this phase per E1P substep 6). Dry-run by default; `--write` requires the dry-run diff to have been reviewed. Insertion never reformats the Markdown body; line endings preserved; body SHA-256 before/after compared and reported (`E1_METADATA_MIGRATION_REPORT.md` + JSON). Inference rules: `PLAN_ID` from filename only when unambiguous, else stable synthetic `LEGACY-xx` + review queue; `CATEGORY` only from explicit headings/directory with `INFERRED: true` until reviewed; `PREMISE_VERIFIED_AT` only when the plan already names an immutable SHA or a verification pass is performed — never inferred; `STATUS` never inferred to `DONE` from prose ("implemented" in narrative text is not evidence — corpus guardrail). Post-migration: register regenerated, count parity vs. E1A baseline proven, one-time checksum report proves no plan disappeared.
- **Acceptance:** body hashes unchanged outside the front-matter boundary; all baseline paths represented; zero silent `DONE`; dry-run == write proposals; re-running the migrator is a no-op.

### E1D — Premise freshness, dead-reference, and capability-claim verification

- **Current reality:** `verify-capability-claims.py` does evidence-path existence checks for 24 curated claims, but no plan-corpus reference checking exists; stale premises have been found repeatedly by hand (source-plan evidence table rows 4–6: H5, H11, the "261 map nodes" claim, `ExpansionEnrichmentCatalog` unread, `AGENTS.md` describing the retired Unity tree).
- **Required delta:** One shared reference parser/classifier, consumed by register generation (per-row reference health), intake (authority path must exist), and the claims verifier (shared-seam migration).
- **Design:** `plan_corpus_lib.py::classify_reference()` handles repo-relative paths, `path:line` and `path:line-line`, glob-like plan references, named `PLAN_ID`s, and strips prose punctuation. Verdicts: `OK`, `MISSING_PATH`, `MISSING_LINE_RANGE`, `RENAMED_CANDIDATE` (suggestion only — never auto-rewrite), `AMBIGUOUS`, `EXTERNAL`. Premise-stale rules are evidence-based, not date-based: missing authority path, invalid referenced symbol where symbol evidence exists, contradiction by a newer accepted decision, or recorded manual verdict. Old-but-valid SHA = `warning`, never failure. Severity model: active plan → absent required authority = `error`; old SHA with live paths = `warning`; archived-history dead links = `info`, never CI-blocking. Modes: `--plan <id>`, `--changed-since <sha>`, `--all-active`. Output: `PREMISE_HEALTH.md` + machine verdicts embedded in `PLAN_REGISTER.json`. **Shared-seam flag:** migrating `verify-capability-claims.py` onto the shared parser touches a Plan 29-sealed artifact; it is scheduled as its own sub-step with byte-identical report-parity proven on the current 24 claims before and after, and is integrator-mediated. Until that sub-step lands, the verifier keeps working unmodified (the shared lib is additive).
- **Acceptance:** deleted-file fixture flagged `MISSING_PATH`; moved-file fixture yields `RENAMED_CANDIDATE` suggestion with no auto-fix; old-SHA-valid-paths stays `warning`; archived dead links `info`-only; checker never imports game code, never mutates content; source-plan historical staleness examples converted into regression fixtures.

### E1E — Duplicate-topic clustering and live-capability mapping

- **Current reality:** `detect-corpus-duplicates.py` covers only C1/C2-vs-closeout identity collisions, advisory-only. CLAIMS.json (24 claims) + the architecture map are the live-capability truth. The source plan documents the failure mode: rumour networks proposed while radio tuner + `SignalTriangulationSystem` + `WeatherIntelligenceCoordinator` + `MoralChoiceGossipRuntime` are live; per-NPC memory proposed while `LocationMemorySystem` + `PhantomMemory` + `standing_record_memory.json` exist.
- **Required delta:** Candidate overlap clusters across the whole register, each with confidence + evidence paths, feeding human/second-tool review, and producing `MERGE`-candidate rails tickets — never automatic status changes.
- **Design:** A capability vocabulary aligned to real domains (information-flow, radio, triangulation, gossip, memory, needs, disease, weather, expedition, governance, identity, trade, shelter, power, relations, journal, UI, save). Signals: title, `CATEGORY`, referenced files, rails, explicitly named systems — deterministic signals only; no embedding-based auto-decisions (E1E substep 2: "Do not rely on embeddings alone for final decisions"). The code-authority index is built from the generated architecture map node list + CLAIMS.json evidence paths + save-section registry names — existing generated truth, not a new scan of everything. Output `CAPABILITY_CLUSTERS.md`: each cluster lists member plans, candidate live authorities (with `claim_id`/path evidence), confidence, and a `review_status: PENDING` until a human/second-tool verdict is recorded in the companion JSON. Cluster IDs feed back into the register so intake can warn "new plan enters known cluster `information-flow`". The `duplicate_search_receipt` format (§10.4) is defined here and consumed by E1I. Semantic-overlap vs implementation-duplicate is a required reviewer question (two memory plans may target different ownership/lifecycle concerns).
- **Acceptance:** synthetic duplicate fixtures cluster; similarly-named-but-distinct fixtures stay separate; every cluster record carries evidence paths; no plan status changes from clustering alone (verified by test); the five source-plan example clusters are resolved explicitly (with evidence) or recorded as not-supported-by-current-evidence.

### E1F — Design pillars and falsifiable ambition rubric

- **Current reality:** No design-pillar doc exists (source evidence row 8: `docs/` has only visual design rules). Scope decisions are currently unarguable from principle. `DESIGN.md` governs visual tone only.
- **Required delta:** `docs/design/PILLARS.md` (4–5 short falsifiable pillars + explicit non-goals) and `docs/roadmap/AMBITION_RUBRIC.md` (the five source questions + scoring dimensions + hard stops + fast paths).
- **Design:** Pillars are player-facing truths, not technologies; the source plan's drafts are the starting point: *scarcity is information before it is inventory*; *people are resources and obligations*; *the shelter is a machine that must be maintained*; *every decision has a witness*. Each pillar ships with a falsification example ("feature X contradicts this because…"). Non-goals written down: no 3D conversion, no new genre layer, no procedurally generated open world, no live-service, no dialogue-tree platform for its own sake, no code-mod platform unless re-approved (generalizing Wave 7's 47C non-goals; consistent with DEC-10/DEC-12 declined precedents). Rubric: the five source questions (connect-vs-accumulate; rails readiness; fake-console risk; day-2 player change; what it blocks) plus scoring dimensions (authority ownership, save ownership, testability, content demand, presentation burden, migration burden, metric moved). Weighted score supports prioritization only; hard stops override arithmetic: duplicate live authority without extension rationale; UI with no mutating authority; new save type with no migration owner; content wave with no acceptance route. Fast-path positives: closes a known link gap; converts zero-consumer data into gameplay; removes a fake affordance; increases runtime evidence; retires duplicate authority. Decision-record template: decision, evidence, alternatives, reversibility, reviewer — one page, linkable from register and decision register.
- **Acceptance:** rubric demonstrably separates a LINK repair from a speculative SYSTEM addition on fixtures; each hard-stop rule rejects/defers at least one plausible fixture plan; pillars fit a deliberately small word budget (target ≤ 600 words — short enough to read before planning); a second tool reviews pillars blind and confirms they can reject; pillars are versioned and changes require decision records.

### E1G — Whole-backlog ambition audit and ordered triage

- **Current reality:** The census classifies the 131-file corpus chain, but the 230+134 legacy plans have no triage state; no ordered cross-namespace roadmap exists.
- **Required delta:** Every active candidate triaged into exactly one bin — `NOW`, `NEXT`, `MERGE`, `NOT_NOW`, `DROP`, `NEEDS_PREMISE_REVIEW` — with the invariants below, recorded in `docs/roadmap/AMBITION_AUDIT.md`, and folded back into register metadata.
- **Design:** Score the register table first; open full documents only for top/contested/stale/high-overlap bands (247+ documents are not a reading assignment — source 53B substep 3). `NOW` is hard-capped (WIP cap value recorded in `plan_governance_config.json`, machine-checked by the register). Every `MERGE` names the target live authority or accepted plan; every `DROP` cites a pillar/contradiction/duplicate/cost reason + decider and lands in `DECISION_REGISTER.md` as `DECLINED`; every `NOT_NOW` names its return condition and lands as `DEFERRED-WITH-CONDITION` (register vocabulary mirrored, §9); every `NEEDS_PREMISE_REVIEW` maps to plan `STATUS: PREMISE_STALE` and is non-executable until re-verified. `NEXT` is sequenced by dependency and metric impact, not plan number. Overlap clusters become consolidated rails tickets (information-flow, per-NPC memory, governance, needs cascade, food pipeline — one ticket each naming the live systems to extend). Plans already implemented in substance require completion evidence then archive, never reimplementation. Second-tool blind review (register + pillars + evidence summaries, no narrative) with disagreement adjudication recorded (feeds E1O).
- **Acceptance:** every active plan has exactly one triage state (register-enforced); no `MERGE` without target; no `NOT_NOW` without return condition; no `NOW` item blocked by an unmet hard rail; WIP cap machine-checkable; audit doc records evidence per verdict.

### E1H — Rails readiness registry and dependency truth

- **Current reality:** The source plan names ten rails: graph (32A), intel (33), identity (40A), voice (42), policy (43B), relations outcomes (44A), seasons (38A), commitments (38C), acceptance ladder (45A), port contract (36A). Verified statuses at 2026-09-19 (from census rows): 38 **SEALED** (Wave 11 A1), 43 **SEALED** (A3), 45 **SEALED** (A4), 36A/36B sealed with 36C queued (Wave 11 Part 2 B4), 33 **SEALED** (Wave 10 Part 2 B4), 32 **PARTIALLY-SEALED** (graph-native travel unsealed). Identity (40A), voice (42), relations outcomes (44A): not verified in this planning pass — their census rows remain `AUDIT-PENDING`/unread here; E1A/E1H must resolve them from current evidence before the registry ships. This is recorded as an execution prerequisite, not assumed.
- **Required delta:** Authored rail definitions (`rails.registry.json`) → generated resolved states (`rails.json` + `RAILS.md`), with readiness levels finer than binary where needed.
- **Design:** Readiness levels: `NOT_STARTED` / `IN_FLIGHT` / `CODE_READY` / `RUNTIME_VERIFIED` / `PRESENTED` / `DONE` (corpus §E1H substep 5). Rail status is *resolved from authoritative plan metadata* (the register) — the Markdown table is generated, never hand-edited. Rail record: `id`, `name`, `authority_plan`, `state`, `evidence[]` (typed per E1 §13 evidence classes), `consumers[]`, `blocks[]`. Validators: unknown `RAILS_REQUIRED` ID → intake/register error; dependency-cycle detector across plans and rails; composite readiness when a plan is code-done but runtime-partial; spike exception (missing rail) requires timebox + decision output + expiry; intake compares *required readiness level vs actual level*, not string equality. The ten source rails seed the registry with their census-derived statuses; rail-consumer lists expose blast radius for sequencing.
- **Acceptance:** unknown rail fails validation; cycle detected (fixture A⇄B); insufficient readiness blocks an executable plan; spike exception missing expiry fails; generated `.md` and `.json` synchronized under `--check`.

### E1I — Plan intake form and executable intake checker

- **Current reality:** Nothing prevents a new plan from appearing without evidence; the two legacy namespaces grew by exactly that route. No `INTAKE.md`, no checker.
- **Required delta:** The intake policy document plus `plan-intake-check.py` (with `.sh` wrapper) that can refuse, with fast lanes that keep hygiene work cheap.
- **Design:** Intake fields live in front matter (`INTAKE:` block, §10.2) — form-as-metadata, not prose. Full intake required for: `SYSTEM`, new `PRESENTATION` surfaces, save-schema changes, new resource types, broad content waves. Fast lane (identity/status/evidence minimum only) for `PROCESS`, small `LINK`, docs hygiene. Rejection rules (each with fixture): missing required rail or readiness below threshold without named prerequisite/spike; `SYSTEM` without `duplicate_search_receipt`; panel without authority/read-model/mutating-action (+ save owner when stateful); content without consumer or acceptance route; missing `METRIC_MOVED`; cross-cutting change without rollback path. `--explain` prints each failed rule and its fix. Scoping: local single-plan mode; CI mode checks *changed* plans via git diff after migration — historical archived plans are never force-modernized (E1I substep 12).
- **Acceptance:** every rejection fixture fails for the intended rule (and only that rule); fast-lane LINK fixture passes; changed-plan mode ignores untouched archived history; output deterministic and human-readable; the gate demonstrably exits non-zero on a crafted invalid plan and zero on a valid one.

### E1J — Presentation-authority contract and fake-affordance prevention

- **Current reality:** BUG-UI-002 (30 routed consoles, 5,186 lines, registered before Core/host authority existed) is documented in-repo; roadmap README §3.3 already defines "UI Done" (live route, read-model binding, keyboard/a11y, lifecycle) as a binding DoD — E1J moves the *authority* half of that DoD to plan time.
- **Required delta:** A minimum authority contract for interactive panels, enforced at intake; a generated panel-authority coverage metric; back-audit of highest-risk planned panels into LINK tickets.
- **Design:** Contract per interactive gameplay panel: authoritative system/host session, read model, mutating command/action, persistence owner if stateful, error/loading/disabled semantics, evidence test (one integration test or runtime selftest per mutation path). Registry: generated view sourced from the existing `PanelRegistryBootstrap`/player-surface manifest data where practical — not a new hand-maintained table. Read-only diagnostic/debug panels declare `panel_kind: read_only` and are exempt from the mutating-action rule (E1J substep 4 — no forced fake mutations). Fake-affordance smell list (button with no authoritative command; UI-local state standing in for gameplay state; duplicated validation; static placeholder data; host bypass; save-unaware mutation) published in `INTAKE.md`. Static code checks for registration patterns stay `warning`-only until false-positive rate is measured (substep 7). Metric: `interactive_panels_with_verified_authority / interactive_panels_total`, fed to E1N.
- **Acceptance:** panel-without-authority fixture rejected; read-only diagnostic fixture passes with declared exception; authority registry generates deterministically; at least one integration fixture proves a UI command reaches the named authority; exceptions list is explicit (codex, debug views, legal/about, non-gameplay menus).

### E1K — Numbering, concurrent authorship, retirement, and archive discipline

- **Current reality:** README §2 covers number ranges, reservation, no-silent-renumbering, and collision classification. Unaddressed today: E-series filename sequence mechanics, multi-author folder ownership, when/how files physically move to `docs/archive/plans/`, and duplicate active `PLAN_ID` detection.
- **Required delta:** Additive policy subsections in `docs/roadmap/README.md` + mechanical enforcement in the register generator.
- **Design:** Published rules: historical numbers preserved (no renumbering — the Plan 53 collision stays a *recorded* drift); E-series sequence `E1_planintegration.md`, `E1_planintegration[2].md`, … with the bracket immediately before `.md` (corpus naming contract); concurrent-authors protocol (number reservation in `INTEGRATION_PLANS.md` + path claim in `WORKTREE_OWNERSHIP.md` *before drafting* — extends README §2 rule 1 from implementation to drafting; collision → subject matching → census row, per existing precedent); stable-`PLAN_ID` cross-links so archive moves don't break semantics; archive policy: only `DONE`/`SUPERSEDED`/`MERGED`/`DROPPED`/explicitly historical items move, with `COMPLETED_AT` + `COMPLETION_EVIDENCE` required for `DONE`, metadata preserved, `LEGACY_SOURCE_PATH` recorded, generated archive index; duplicate-filename overwrite rejected by tooling; duplicate active `PLAN_ID` → register error; stale-active (untouched beyond threshold without status review) → warning only, never auto-archive. `AGENTS.md`/rulebooks carry a pointer to the single policy location, never a copied body.
- **Acceptance:** duplicate-active-ID fixture fails; archive-move fixture preserves identity (ID + legacy path); E-series filename examples (including `[2]`, `[3]`) validate; historical numbering byte-unchanged (E1A baseline comparison).

### E1L — CI rollout, enforcement tiers, and developer ergonomics

- **Current reality:** The manifest supports `classification` and `critical` per gate (E-8), so report-only staging needs no runner changes. Fast tier is green (48/48 at last closeout).
- **Required delta:** Two new gates introduced report-only, then promoted through the corpus rollout Stages 0–5 with measurable warning counts and waiver discipline.
- **Design:** New manifest rows: `plan_register_drift` (`python3 scripts/ci/generate-plan-register.py --check`, timeout 60 s) and `plan_intake_check` (`bash scripts/ci/plan-intake-check.sh --changed`, timeout 60 s), plus `plan_governance_selftest` (`generate-plan-register.py --self-test && plan-intake-check.py --self-test`, timeout 120 s) — all `classification: fast`, `critical: false` at Stage 0–1. Rollout tiers (E1L): Tier 0 local/report; Tier 1 changed-file enforcement (`critical: true` for `plan_intake_check` only); Tier 2 full active-plan enforcement (register drift goes `critical`); Tier 3 quality thresholds (warning-count ratchet: PRs may not increase metadata-debt warning count without an accepted waiver). Waiver format: `owner`, `reason`, `expiry` (date), `decision_record` link; expired waiver = gate failure (mechanical). Governance and runtime results reported separately (governance red must never mask a gameplay regression — E1 §8). Performance target: full-corpus scan in seconds; fixture tests versioned with scripts, never dependent on live plan counts.
- **Acceptance:** changed-plan enforcement catches a newly-invalid fixture plan; legacy untouched invalid fixture remains warning during staged rollout; waiver expiry enforced by test; `verify-fast.sh` output keeps governance and runtime results separable; tier promotion requires one clean wave + documented false-positive review at the previous tier.

### E1M — Roadmap publication and execution handoff

- **Current reality:** `INTEGRATION_PLANS.md` is the live execution ledger (current batch: XP Expansion W1); `WAVE_LEDGER.md` indexes waves. No one-page cross-namespace roadmap with a capped NOW band exists.
- **Required delta:** `docs/roadmap/ROADMAP.md` — one page, register-validated, containing only `NOW`, near-term `NEXT`, prerequisite rails, and metric deltas; plus the standard handoff block (E1 §17) adopted for follow-on execution.
- **Design:** Roadmap rows link stable `PLAN_ID`s and authoritative detail docs — no duplicated implementation prose (it is a *view*). Each `NOW` item carries owner/role, start condition, exit condition, evidence bundle, rails, acceptance tier. Freeze rule: no new `SYSTEM` enters `NOW` while the cap is full; displacement requires a short decision record naming what is paused. `NOT_NOW` index grouped by return condition; `MERGE` clusters published as rails tickets with first integration seam named; `NEEDS_PREMISE_REVIEW` published as an evidence queue. The register validates the roadmap (`ROADMAP.md` NOW-count ≤ cap; every NOW entry resolves to a registered plan; no blocked rail shown satisfied; hand edits conflicting with register state fail `--check`). Boundary with existing ledgers: `INTEGRATION_PLANS.md` remains the *execution* authority for claimed packages; `ROADMAP.md` is the *ambition sequencing* view — README gains two sentences fixing this boundary so the roadmap never becomes a second queue.
- **Acceptance:** NOW count respects cap under `--check`; every NOW entry resolves; blocked rails cannot appear satisfied; deliberate register/roadmap drift fixture is caught.

### E1N — Metrics, deltas, and quarterly governance health

- **Current reality:** Zero baseline metrics exist; the source plan's breadth-without-connection numbers (4 `EFFECT_PRODUCED` catalogs vs 300 zero-consumer catalogs; 29/452 non-narrative consumer coverage) are historical measurements, to be re-measured, not quoted.
- **Required delta:** `governance_metrics.json` + `GOVERNANCE_METRICS.md`, generated from repository data; quarterly snapshot format; directional targets.
- **Design:** Baseline metrics from E1A: active plans, metadata completeness %, stale premises, overlap clusters, zero-consumer catalogs (via existing content-utilization evidence — measured by the existing gates, not a new scanner), executable plans blocked by rails, panel-authority coverage (E1J), roadmap WIP. Ratio metrics: LINK:SYSTEM plan ratio; plans with runtime evidence; plans with named metric; completed-to-started per wave. Leading (intake quality, rail readiness, premise freshness) vs outcome (consumer coverage, runtime acceptance, duplicate-authority reduction) indicators separated. Directional targets only: zero *new* zero-consumer content; zero *new* panels without authority; declining stale-premise count. Bottleneck telemetry: premise age distribution; count of plans waiting per rail; `MERGE` conversion outcomes; archive throughput; decision reversals (rubric-rigidity signal). **Governance-cost metric** (median intake completion time; checker duration) with an explicit simplification trigger: if governance cost rises while disconnected-work metrics do not improve, a process-simplification review is mandatory (anti-bureaucracy, E1P). Quarterly review chooses at most a small number of process changes — governance itself passes intake when it adds author obligations (E1P substep 10).
- **Acceptance:** metrics generator deterministic (two runs → identical JSON); ratios handle zero denominators; quarterly snapshot references immutable E1A baseline values; no metric requires reading arbitrary prose.

### E1O — Independent cross-tool review and adversarial audit

- **Current reality:** The repo already practices cross-tool QA (census adjudication, second-tool bin review in 53B substep 12). No standing review-packet format exists.
- **Required delta:** A minimal review-packet template + adjudication records for high-impact verdicts; reviewer has no status-write power.
- **Design:** Packet = register slice + pillars + rails + code-authority evidence + candidate decision, with persuasive narrative stripped (no hidden conclusion field — verified). Reviewer classifies independently, must cite repository evidence, lists missing evidence / duplicate authorities / false-deferral risk. Verdicts compared in a structured table; disagreements on `DROP`/`MERGE`/new `SYSTEM` escalate to a recorded adjudication; ordering-only disagreements don't block. Stratified sample after the full audit: high-cost SYSTEM, high-overlap cluster, UI panel, content wave, apparently-complete legacy plan. Reviewer agreement rate tracked over time (very low → rules underspecified; suspiciously high → check rubber-stamping). Packets archived with the decision record. Re-run when pillars or hard-stop rules materially change.
- **Acceptance:** packet contains no conclusion field; every status-influencing reviewer claim cites evidence; status mutation still requires the primary governed workflow (reviewer is not an authority).

### E1P — Programme closure, ownership transfer, and anti-bureaucracy check

- **Current reality:** n/a (terminal phase).
- **Required delta:** Clean exit: all verifications from a clean tree, enforcement tiers at their declared level, footprint audit, completion report, ownership handed to roadmap/register maintainers, E1 marked `DONE` with linked evidence.
- **Design:** Re-run the full command matrix (§18.4) from a clean checkout; confirm generated docs in sync and no unclassified active plan remains; confirm WIP cap + NOW rail readiness; archive or mark non-routine the one-time migration script; **count the permanent footprint** (scripts, docs, configs, tests) — if materially larger than necessary, simplify before closure (the source plan's closing warning: "if intake becomes a second bureaucracy, the next audit will be about this plan"); write `docs/roadmap/e1/E1_COMPLETION.md` with baseline→final metric deltas, residual debt, accepted waivers, follow-on plan IDs (E2–E8 queue references); transfer ownership to the standing foreman/integrator roles (no permanent E1-specific role); schedule the first quarterly review inside the normal planning cadence; prove zero runtime drift by comparing baseline vs final gameplay gate outcomes; flip census row 114 to `SEALED` (integrator), add the `WAVE_LEDGER.md` row, and append exactly one capability claim to `CLAIMS.json` (`gov_plan_register_integrity`, evidence = the two gates + contract tests).
- **Acceptance:** all baseline runtime verification green; no unclassified active plan; no unexplained permanent migration warning; completion report contains metric deltas + residual debt; `E1_planintegration[2].md` / `[3].md` sequence names recorded as reserved in the census/README per E1K.

---

# 7. Ownership Matrix

Claim strategy: one package claim row (`claim-e1-plan53-ambition-governance-<date>`) written by the foreman/integrator in `WORKTREE_OWNERSHIP.md` at Phase 0. Package-owned paths are all-new and disjoint from every active claim (§2.4). Shared governance paths are touched only at the named handoff points.

| Path / surface | Relationship | Owner during execution | When touched |
|---|---|---|---|
| `scripts/ci/plan_corpus_lib.py`, `generate-plan-register.py`, `plan-intake-check.py`, `plan-intake-check.sh`, `migrate-plan-metadata.py` | **New, package-owned** | E1 builder | Phases 1–3, 8 |
| `scripts/ci/plan_governance_config.json`, `scripts/ci/fixtures/plan_governance/**` | **New, package-owned** | E1 builder | Phases 1–3, 8 |
| `docs/roadmap/PLAN_REGISTER.{md,json}`, `RAILS.md`, `rails.json`, `rails.registry.json`, `PREMISE_HEALTH.md`, `CAPABILITY_CLUSTERS.md`, `ROADMAP.md`, `AMBITION_AUDIT.md`, `AMBITION_RUBRIC.md`, `INTAKE.md`, `GOVERNANCE_METRICS.{md,json}` | **New, package-owned** (generated ones are regenerated, never hand-edited, after handoff) | E1 builder → roadmap maintainer at E1P | Phases 1–13 |
| `docs/roadmap/e1/**` (baseline, migration report, review queues, decision records, completion) | **New, package-owned** | E1 builder | Phases 0–14 |
| `docs/design/PILLARS.md` | **New, package-owned** (new `docs/design/` file; existing files untouched) | E1 builder | Phase 5 |
| `Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs` | **New, package-owned** (static contract tests only; no Core production code) | E1 builder | Phases 2, 7, 8, 11 |
| Plan files' front matter (all three namespaces) | **Modified, package-owned during Phase 2 only**, body bytes preserved; per-file diff review | E1 builder | Phase 2 (migration) |
| `docs/ci/CI_GATE_MANIFEST.json` | **Shared — integrator seam** | Integrator | Phase 10 (three additive rows) |
| `docs/roadmap/README.md` | **Shared — Plan 29 artifact, integrator seam** | Integrator, additive subsections only | Phases 9, 11 |
| `AGENTS.md` + 13 rulebooks | **Shared — integrator seam**; exactly one pointer line; rulebooks regenerated by canonical sync, never hand-edited | Integrator | Phase 10 (Stage 1 promotion) |
| `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` (row 114), `KNOWN_DEBT.md` | **Shared — foreman/integrator ledgers** | Foreman/integrator | Phases 0, 14 |
| `docs/governance/DECISION_REGISTER.md` | **Shared — integrator seam**; triage `DROP`/`NOT_NOW`/portfolio verdicts appended as `DEC-xx` rows | Integrator | Phases 6, 13 |
| `docs/architecture/CLAIMS.json` | **Shared — Plan 29 artifact, integrator seam**; one additive claim at closure | Integrator | Phase 14 |
| `scripts/ci/verify-capability-claims.py` | **Shared — Plan 29 artifact**; optional shared-parser migration with byte-parity proof, or untouched | Integrator | Phase 3 tail (flagged sub-step) |
| `docs/roadmap/WAVE_LEDGER.md` | **Shared**; one wave row at closure | Integrator | Phase 14 |
| `docs/INDEX.md` | **Generated**; regenerated via canonical `generate-docs-index.py` at package close | Integrator | Phase 14 |
| `Assets/Ashfall.Core/**`, `src/**`, `Assets/StreamingAssets/Data/**`, `project.godot`, save stores | **Untouched — hard exclusion** | n/a | never |

---

# 8. Data Flow

The intake funnel — how a future expansion ambition becomes governed execution work. Every arrow is either a human authoring act or a named script; there is no mutable state outside front matter and authored registries.

```text
NEW AMBITION (any author/agent)
   │  writes plan .md with front matter + INTAKE block (§10.1/§10.2)
   ▼
plan-intake-check.py  ──► parses via plan_corpus_lib ──► validates schema/enums
   │                                                       rails (vs rails.json)
   │                                                       duplicate-search receipt presence
   │                                                       panel-authority contract (E1J)
   │                                                       metric/acceptance/rollback presence
   ▼
ACCEPTED as STATUS: READY (or PROPOSED / spike-with-expiry)        REJECTED: diagnostics list
   │                                                                  (file:field:rule:fix)
   ▼
generate-plan-register.py ingests ALL plan front matter
   │  + classifies every file[:line] reference (E1D verdicts)
   │  + resolves PREMISE_VERIFIED_AT age vs HEAD (warning-only)
   │  + assigns overlap cluster IDs (E1E candidate clusters)
   │  + resolves RAILS_REQUIRED against rails.registry.json (E1H)
   ▼
PLAN_REGISTER.json (substrate) ──► PLAN_REGISTER.md (presentation)
   │                            ──► PREMISE_HEALTH.md
   │                            ──► CAPABILITY_CLUSTERS.md ──► human/second-tool review
   │                            ──► rails.json + RAILS.md        (verdicts recorded in JSON)
   │                            ──► governance_metrics.json ──► GOVERNANCE_METRICS.md
   ▼
AMBITION AUDIT (E1G, human + rubric, reading the register not the corpus)
   │  triage verdicts: NOW / NEXT / MERGE / NOT_NOW / DROP / NEEDS_PREMISE_REVIEW
   │  binding verdicts ──► DECISION_REGISTER.md rows (integrator)
   ▼
ROADMAP.md (generated-validated one-pager; NOW ≤ WIP cap; rails resolved)
   │
   ▼
INTEGRATION_PLANS.md / WORKTREE_OWNERSHIP.md  (existing execution ledger + claims —
   │                                            unchanged in role; roadmap feeds it, never replaces it)
   ▼
CI: plan_register_drift --check · plan_intake_check --changed · plan_governance_selftest
   (report-only → staged enforcement per E1L)
```

Feedback loops (all deliberate): cluster review verdicts → register metadata; audit verdicts → plan `STATUS`; rail state changes (when authority plans seal) → rails.json on next generation → intake outcomes change accordingly; quarterly metrics → at-most-small process changes, themselves admitted through intake.

---

# 9. State Model

There is no runtime state. The state model governs **plan lifecycle metadata only**, and it deliberately mirrors vocabularies that already exist so no new semantic drift is introduced.

## 9.1 Plan lifecycle (canonical `STATUS` enum — from E1B/E1 §7, reconciled)

```text
PROPOSED ──► NEEDS_PREMISE_REVIEW (audit bin; materializes as STATUS: PREMISE_STALE)
   │  ──► NOT_NOW .................. requires RETURN_CONDITION (decision: DEFERRED-WITH-CONDITION)
   │  ──► DROPPED .................. requires evidence + decider (decision: DECLINED)
   ▼
READY ──► BLOCKED ................. requires named blocker (rail/plan/signature)
   │  ──► IN_PROGRESS ──► BLOCKED
   │                 ──► SUPERSEDED . requires SUPERSEDED_BY (decision: RETIRED for the old scope)
   │                 ──► MERGED ..... requires MERGED_INTO live authority/plan (decision: RETIRED/duplicate)
   │                 ──► DONE ....... requires COMPLETED_AT + COMPLETION_EVIDENCE (register §3.5 Plan-Done)
   │                                   ──► ARCHIVED (storage state; terminal; requires DONE evidence)
```

Canonical `STATUS` values (stored in front matter): `PROPOSED`, `PREMISE_STALE`, `READY`, `IN_PROGRESS`, `BLOCKED`, `DONE`, `MERGED`, `NOT_NOW`, `DROPPED`, `SUPERSEDED`, `ARCHIVED`. Triage bins (`NOW`/`NEXT`) are **roadmap placement**, recorded in `ROADMAP.md` + the audit document, not `STATUS` values — this keeps lifecycle state and prioritization orthogonal.

## 9.2 Verdict-vocabulary mapping to `docs/governance/DECISION_REGISTER.md`

The audit's binding outcomes reuse the standing register's terminal verdicts exactly (§2 of that document), so scope decisions live in one place with one vocabulary:

| Triage outcome | Decision-register verdict | Mandatory content (mirrors DEC register columns) |
|---|---|---|
| Admitted to `NOW` band | `SIGNED` (execution authorization) | owner, execution package, evidence, recheck trigger |
| `DROP` | `DECLINED` | pillar/contradiction/duplicate/cost reason, decider, evidence |
| `NOT_NOW` | `DEFERRED-WITH-CONDITION` | named return condition (rail completion / acceptance tier / runtime evidence / budget / milestone), execution package candidate, recheck trigger — "never indefinite" |
| `MERGE` / `SUPERSEDED` | `RETIRED` (for the absorbed scope) | target live authority/plan, evidence that the target satisfies intent |
| `NEEDS_PREMISE_REVIEW` | (no decision row; non-executable queue) | stale evidence list from E1D |

## 9.3 Rail readiness states

`NOT_STARTED` → `IN_FLIGHT` → `CODE_READY` → `RUNTIME_VERIFIED` → `PRESENTED` → `DONE`. Readiness is *resolved* from the authority plan's `STATUS` + evidence classes (E1 §13: `CODE`, `DATA`, `TEST`, `RUNTIME`, `UI`, `SAVE`, `DOC`, `COMMIT`, `DECISION`, `METRIC`), never hand-typed into the generated table. Intake compares required level vs resolved level (e.g. a plan requiring `acceptance_ladder: RUNTIME_VERIFIED` fails while that rail resolves `IN_FLIGHT`).

## 9.4 Exception lifecycles (both expire mechanically)

- **Spike:** accepted with `SPIKE: {timebox_days, decision_due}`; past `decision_due` with no decision record → intake failure. A spike converts to a full intake record or closes; it never silently becomes a shipped system (source 53C substep 8).
- **Waiver:** `{owner, reason, expiry, decision_record}`; past `expiry` → gate failure (E1L substep 12). Waivers are counted in governance metrics; a growing waiver count is a leading health signal.

## 9.5 Single-writer rules

- `STATUS`/identity/intent fields: authored by humans in front matter (single editable location).
- Reference health, premise age, cluster IDs, rail resolution, metrics: **generated only** — a hand edit to a generated field or generated file is a `--check` failure by construction (regeneration overwrites).
- No field may exist in two editable places (corpus guardrail: "Do not copy status into multiple independently editable tables"). The census keeps its historical classification role; the register links it via `SOURCE_AUTHORITY` rather than copying its verdicts into plan front matter.

---

# 10. API/Contracts

## 10.1 Proposal-record schema (plan front matter, `PLAN_SCHEMA_VERSION: 1`)

```yaml
PLAN_SCHEMA_VERSION: 1
PLAN_ID: "E1"                      # string, unique among ACTIVE plans, stable, never reused
TITLE: "Ambition Governance & Intake"
STATUS: READY                      # enum, §9.1
CATEGORY: PROCESS                  # SYSTEM | LINK | CONTENT | PRESENTATION | PROCESS
WAVE: 8                            # int or null
NAMESPACE: corpus                  # corpus | next_steps | piagents (generated-normalizable)
PREMISE_VERIFIED_AT: "ccac926e"    # immutable SHA or null; null => never verified
PREMISE_VERIFIED_DATE: "2026-09-19"# ISO date or null
OWNER: "foreman"                   # role or claim id
PILLARS: ["scarcity-information"]  # ids from PILLARS.md; [] allowed for PROCESS
RAILS_REQUIRED:                    # list of {id, readiness}; [] allowed
  - {id: acceptance_ladder, readiness: RUNTIME_VERIFIED}
METRIC_MOVED: ["plan_intake_compliance"]   # >=1 for full-intake categories
ACCEPTANCE_TIER: "PROCESS_ENFORCED"        # free-form tier id per Plan 45 ladder
SOURCE_AUTHORITY: ["docs/roadmap/INTAKE.md"]  # evidence pointers
SUPERSEDES: []                     # list of PLAN_ID
SUPERSEDED_BY: []                  # list of PLAN_ID
MERGED_INTO: null                  # PLAN_ID or null
COMPLETED_AT: null                 # ISO date; required iff STATUS: DONE
COMPLETION_EVIDENCE: []            # paths/SHAs; required iff STATUS: DONE
LEGACY_SOURCE_PATH: null           # recorded before any archive move
INFERRED: false                    # true until a human reviews machine-inferred fields
INTAKE: { ... }                    # §10.2; required for full-intake categories
```

**Validation rules (each is a named, testable rule ID):** `R-SCHEMA-VERSION` (known version or structured `SCHEMA_TOO_NEW` error); `R-ID-PRESENT`; `R-ID-UNIQUE-ACTIVE` (duplicate active `PLAN_ID` → error); `R-STATUS-ENUM`; `R-CATEGORY-ENUM`; `R-PREMISE-SHA-FORMAT` (hex, 7–40 chars, resolvable via `git cat-file` when repo available, else `EXTERNAL`); `R-SUPERSEDE-ACYCLIC` (cycle → error naming members); `R-DONE-EVIDENCE` (`DONE` without `COMPLETED_AT`+`COMPLETION_EVIDENCE` → error); `R-ARCHIVE-TERMINAL` (only terminal statuses may carry archive location); `R-RAILS-KNOWN` (every `id` resolves in `rails.registry.json`); `R-RAILS-READY` (resolved readiness ≥ required, else named prerequisite or unexpired spike); `R-NOTNOW-RETURN` (`NOT_NOW` without return condition in audit record → error); `R-MERGE-TARGET` (`MERGED` without `MERGED_INTO` → error); `R-OWNER-PRESENT`; `R-METRIC-PRESENT` (full-intake categories); `R-GENERATED-FIELD-IMMUTABLE` (hand-edited generated fields detected by regeneration diff).

## 10.2 `INTAKE` block (full-intake categories: SYSTEM, new PRESENTATION, save-schema change, new resource type, broad content wave)

```yaml
INTAKE:
  pillars_touched: ["scarcity-information"]
  duplicate_search_receipt: "docs/roadmap/e1/receipts/2026-09-19-information-flow.md"  # §10.4; SYSTEM: required
  authority: "Assets/Ashfall.Core/.../ExistingSystem.cs"   # panel/new-surface plans: must exist on disk
  read_model: "ExistingReadModel"            # panel plans: required
  mutating_action: "ExistingSystem.DoThing"  # panel plans: required unless panel_kind: read_only
  panel_kind: interactive                    # interactive | read_only
  save_owner: "ExistingSaveStore"            # required iff the surface persists state
  fake_console_risk: "low"                   # low|medium|high + one-line justification
  day2_change: "What the player does differently on day 2"  # required, non-empty
  metric_moved: ["zero_consumer_catalogs"]
  acceptance_tier: "RUNTIME_VERIFIED"
  rollback: "Feature flag / revert plan"     # required for cross-cutting changes
  reviewer: "role-or-agent"
  spike: null                                # or {timebox_days: N, decision_due: "YYYY-MM-DD"}
```

Fast-lane minimum (PROCESS, small LINK, docs hygiene): identity fields + `STATUS` + evidence pointer + `metric_moved` optional. Ambiguity defaults to the heavy form (source 53C substep 5: "Ambiguity is where policies die").

## 10.3 Rail record (`rails.registry.json`, authored; `rails.json` resolved/generated)

```json
{
  "schema_version": "1.0.0",
  "rails": [
    {"id": "acceptance_ladder", "name": "Content Acceptance Ladder",
     "authority_plan": "C1[14]", "state": "DONE",
     "evidence": [{"type": "TEST", "path": "docs/plans/wave11_part1/A4_PLAN45_IMPLEMENTATION_LOG.md"}],
     "consumers": ["content-intake", "plan-register"],
     "blocks": ["high-volume-content-wave"]}
  ]
}
```

Validation: `RAIL-ID-UNIQUE`; `authority_plan` resolves in register; `state` ∈ readiness enum; evidence paths exist on disk; consumer/block lists reference known IDs or plan IDs; cycle detection over `blocks`/`authority_plan` edges.

## 10.4 Duplicate-search receipt (authored artifact; "intentionally boring and reproducible" — E1 §6.3)

```yaml
receipt_schema_version: 1
plan_id: "NEW-PLAN-ID"
queries: ["rumour information gossip radio triangulation", "LocationMemorySystem PhantomMemory standing_record_memory"]
inspected: ["docs/architecture/CLAIMS.json", "docs/architecture/ARCHITECTURE_TEST_MAP.md", "docs/CURRENT_AUTHORITY.md", "Assets/Ashfall.Core/...", "src/Host/..."]
found_authorities: ["SignalTriangulationSystem", "MoralChoiceGossipRuntime"]
known_clusters_entered: ["information-flow"]     # from CAPABILITY_CLUSTERS.md; may be []
decision: "MERGE_AS_LINK | NEW_AUTHORITY_JUSTIFIED | NO_OVERLAP_FOUND"
justification: "one paragraph citing evidence"
reviewer: "role-or-agent"
verified_at: "<sha>"                              # must resolve in git history
```

The receipt proves the author *looked*; it does not auto-prove a duplicate. Intake validates: file exists, all `found_authorities` resolve as symbols/paths or are explicitly marked `not_found`, `verified_at` parses, `decision` ∈ enum.

## 10.5 Tool CLI contracts

**`generate-plan-register.py`** — modes: `--check` (exit 0 iff working tree matches generated output and zero validation errors; side-effect-free; deterministic), `--write` (regenerate all artifacts), `--json` (print substrate to stdout), `--explain <plan-id>` (parsed record + generated fields + every verdict with reasons), `--self-test` (run fixture suite; exit non-zero on any failure), `--changed-since <sha>` (scope reference re-checks), `--config <path>` (default `scripts/ci/plan_governance_config.json`). Exit codes: `0` clean, `1` validation/drift failure, `2` usage/environment error. Diagnostics format (one per line, machine-parseable): `<path>: front_matter.<FIELD>: <RULE_ID>: <message> (allowed: ...; fix: ...)`. Output determinism: sorted keys everywhere, `\n` line endings, UTF-8, no wall-clock timestamps in any check-mode output (the only timestamped artifact is the E1A baseline, where the timestamp is the payload).

**`plan-intake-check.py`** — positional plan paths, or `--all-active`, or `--changed` (git diff vs merge-base; post-migration CI mode), `--explain`, `--self-test`, `--tier <0-3>` (report/changed/active/threshold). `.sh` wrapper resolves repo root and forwards, matching existing gate-script shape. Refusal messages name the rule, the missing field, and the fix (e.g. `R-PANEL-AUTHORITY: INTAKE.mutating_action required for panel_kind: interactive; set panel_kind: read_only for diagnostic panels`).

**`migrate-plan-metadata.py`** — default `--dry-run` (writes proposed diff + report only); `--write` performs insertion; `--report <path>`; refuses to run twice over an already-migrated file (idempotency via `PLAN_SCHEMA_VERSION` presence); exits non-zero if any body byte outside the front-matter boundary would change.

## 10.6 Register substrate (`PLAN_REGISTER.json`) contract

Top level: `schema_version`, `generated_by`, `corpus_config_version`, `counts {total, by_status, by_category, by_namespace}`, `wip_cap`, `rows[]`. Row: all author-owned fields verbatim + generated block `{reference_health: [{ref, verdict, detail}], premise_age_days, cluster_id, cluster_confidence, rails_resolution: [{id, required, resolved, ok}], numbering_drift: bool, legacy: bool}`. Consumed by: `plan-intake-check.py`, the xUnit contract tests, E1N metrics. Markdown is derived from this JSON; any divergence is a `--check` failure.

---

# 11. Data Changes

No gameplay data changes: `Assets/StreamingAssets/Data/` is untouched; no catalog, schema_version, or JSON authority is added or modified; `CatalogIntegrityValidator` and `--data-integrity-selftest` are run only as regression witnesses.

The only "data" this package authors is repository-governance metadata: YAML front matter in plan files (Phase 2), `rails.registry.json`, `plan_governance_config.json`, fixtures, receipts, and the `docs/roadmap/e1/` artifacts. All generated JSON artifacts declare `schema_version` from day one so future governance-schema changes migrate deliberately (E1B substep 11) rather than silently.

Front-matter migration volume expectation (from §2.1): ~230 `Next-steps-plans` + ~134 `piagentsplans` files get full insertion; 29 corpus files get schema completion (existing keys preserved verbatim — their `PLAN_ID`/`STATUS` values are already meaningful, e.g. `E1`'s `READY_FOR_EXECUTION` maps to canonical `READY` with the original string retained in the migration report's mapping table).

---

# 12. Save/Load

Not applicable — this package introduces no gameplay state, no save section, no save store, and no migration; per E1 §11.9 "No gameplay save migration should be required by E1 itself". The only persistence-adjacent concern is negative: the golden-save fixtures and round-trip gates (Plan 27B sealed machinery) must remain byte-green throughout, serving as proof that governance work leaked nothing into the runtime.

---

# 13. Determinism

Not applicable to gameplay — no Core simulation code, no RNG, no `ISeededRng` stream, no day-advance owner is added (Invariant 4 untouched). The determinism requirement that *does* bind this package is **tool-output determinism**: register generation, intake checking, and metrics must be pure functions of (corpus bytes, config, git metadata at HEAD) — stable sort orders independent of filesystem enumeration, no `dict`-iteration-order leaks (explicit key sorting), no wall-clock reads in check mode, no hash-randomization effects (`PYTHONHASHSEED`-independent by construction via sorted iteration), and byte-identical `--write` output across two consecutive runs on unchanged input. This is tested by the idempotency and two-run byte-stability fixtures (§18).

---

# 14. System/Event Wiring

No Core events, host sessions, day owners, or panel routes are involved — the package's "wiring" is registration of its gates into the existing CI fabric and its pointers into existing governance documents:

1. `docs/ci/CI_GATE_MANIFEST.json` — three additive rows (`plan_register_drift`, `plan_intake_check`, `plan_governance_selftest`) at Phase 10, `critical: false` → promoted per E1L tiers (integrator seam).
2. `AGENTS.md` — one pointer line routing plan authoring through `docs/roadmap/INTAKE.md`; 13 rulebooks regenerated via `sync-agent-rulebooks.py` (integrator seam, Stage 1).
3. `docs/roadmap/README.md` — additive subsections (register contract pointer, E-series naming, roadmap-vs-ledger boundary) (integrator seam).
4. Census row 114 status flips + `WAVE_LEDGER.md` wave row + one additive `CLAIMS.json` claim at closure (integrator seams).
5. `docs/INDEX.md` regeneration via the canonical generator at package close.

No new event bus, registry, or modality manager is created; the register *reads* existing registries (claims, architecture map, census) and never writes to them except through the named integrator seams above.

---

# 15. Godot Integration

Not applicable — no scene, node, panel, autoload, export preset, or `project.godot` change exists in this package; there is no in-game surface for plan governance by design (a player-facing proposal surface is explicitly out of scope, §22). Godot enters only as a regression witness: `godot --headless --path . -- --data-integrity-selftest` and `--bridge-selftest` run at each phase boundary and in the E1A/E1P baselines to prove the runtime is byte-unaffected (15 FPS default per policy if any interactive session were ever needed — none is).

---

# 16. Narrative/Content Integration

No narrative content is authored. The single tone-sensitive deliverable is `docs/design/PILLARS.md`, which must stay inside the project's established voice (AGENTS.md: restrained, human, fictional; no real countries/wars/people) and must describe player-facing truths rather than technologies (E1F substep 1). The source plan's draft pillars (*scarcity is information before it is inventory*; *people are resources and obligations*; *the shelter is a machine that must be maintained*; *every decision has a witness*) are consistent with `DESIGN.md`'s scarcity-and-utility identity and are the starting drafts, subject to the falsifiability acceptance bar and the second-tool blind review (E1F substeps 11–13). The written non-goals (no 3D, no genre layer, no open world, no live-service, no dialogue-tree platform, no code mods) generalize already-recorded project decisions (DEC-10, DEC-12, Plan 47C) rather than inventing new taste.

---

# 17. Failure Modes

## 17.1 Malformed / broken intake record — full analysis

The intake record is the highest-traffic artifact this package creates; its failure modes are enumerated with detection point, severity, and remediation contract. Every row has a fixture (§18).

| # | Malformation | Detection point | Severity / behaviour | Diagnostic contract |
|---|---|---|---|---|
| F-01 | No front-matter delimiters at all (legacy plan) | `plan_corpus_lib` parser | `METADATA_MISSING` structured warning; plan registers as `legacy: true`, never crashes the generator, never blocks CI during Stages 0–1 | `path: R-FRONT-MATTER-MISSING: no YAML front matter found; run migrate-plan-metadata.py --dry-run` |
| F-02 | Unparseable YAML (tab indentation, unbalanced quotes, duplicate keys) | parser | Hard error for that file only; other files still processed; aggregate report at end | `path: line N: R-YAML-PARSE: <parser message>` |
| F-03 | `PLAN_SCHEMA_VERSION` newer than tooling | schema validator | `SCHEMA_TOO_NEW` error with "upgrade tooling or migrate deliberately" — never silently best-effort parsed | names found vs supported versions |
| F-04 | Missing `PLAN_ID` | schema validator | Error; synthetic `LEGACY-xx` only via migration review queue, never auto-assigned at intake | `R-ID-PRESENT` + fix |
| F-05 | Duplicate active `PLAN_ID` | register (cross-file) | Error naming both paths; archive/historical duplicates exempt | `R-ID-UNIQUE-ACTIVE: 'E1' also active at <path>` |
| F-06 | Invalid `STATUS` / `CATEGORY` value (`mostly done`, `In Progress`) | schema validator | Error with the allowed enum printed; suggestion for nearest valid value | `R-STATUS-ENUM ... (allowed: ...)` |
| F-07 | `RAILS_REQUIRED` references unknown rail ID | rails resolver | Error; typo candidate suggested by edit distance as a hint only | `R-RAILS-KNOWN: 'accpetance_ladder' unknown (did you mean 'acceptance_ladder'?)` |
| F-08 | Rail readiness below requirement, no prerequisite/spike | intake checker | Refusal (post-enforcement) / warning (report-only) | `R-RAILS-READY: acceptance_ladder requires RUNTIME_VERIFIED, resolves IN_FLIGHT` |
| F-09 | `SYSTEM` plan without `duplicate_search_receipt` | intake checker | Refusal; the single most important rejection (kills breadth-without-connection at the door) | `R-DUP-RECEIPT: CATEGORY: SYSTEM requires INTAKE.duplicate_search_receipt` |
| F-10 | Receipt `verified_at` SHA not in git history | intake checker | Error (`git cat-file -e` failure) — receipts cannot cite imaginary states | `R-RECEIPT-SHA: <sha> not resolvable` |
| F-11 | Interactive panel plan without authority/read-model/mutating action | intake checker (E1J) | Refusal; the BUG-UI-002 rule | `R-PANEL-AUTHORITY` naming the missing field(s) |
| F-12 | Named authority path does not exist on disk | reference classifier (shared lib) | Error for active plans; `info` for archived | `MISSING_PATH` verdict with the path |
| F-13 | `save_owner` named but plan claims no persistence (or vice versa) | intake checker | Error — contradiction must be resolved by the author, not the tool | `R-SAVE-CONSISTENCY` |
| F-14 | Full-intake category missing `METRIC_MOVED` / `day2_change` / `rollback` | intake checker | Refusal naming each missing field | `R-METRIC-PRESENT` / `R-DAY2` / `R-ROLLBACK` |
| F-15 | Expired spike (`decision_due` passed, no decision record) | intake checker | Failure with the due date and the outstanding decision requirement | `R-SPIKE-EXPIRED` |
| F-16 | Expired waiver | gate | Failure (E1L); owner + original reason reprinted | `R-WAIVER-EXPIRED` |
| F-17 | Supersedence cycle (A supersedes B, B supersedes A) | register | Error naming cycle members; statuses unchanged | `R-SUPERSEDE-ACYCLIC: cycle: A -> B -> A` |
| F-18 | `DONE` without `COMPLETED_AT`/`COMPLETION_EVIDENCE` | register | Error (completion is evidence-gated, not claim-gated) | `R-DONE-EVIDENCE` |
| F-19 | Hand-edited generated file (`PLAN_REGISTER.md` etc.) | `--check` | Drift failure; regeneration overwrites; generated banner names the owning script | `drift: docs/roadmap/PLAN_REGISTER.md (regenerate: python3 scripts/ci/generate-plan-register.py --write)` |
| F-20 | Fast-lane category carrying a half-filled `INTAKE` block | intake checker | Error — ambiguity resolves to the heavy form; author must complete or drop the block | `R-INTAKE-AMBIGUOUS` |
| F-21 | CRLF / non-UTF-8 / BOM in front matter | parser | Normalized on parse; `--check` reports encoding drift as warning (never rewrites body bytes) | `R-ENCODING` |
| F-22 | Bracketed sequence filenames (`E1_planintegration[2].md`) | enumerator/config | Parsed literally; glob config must not treat `[]` as a character class; validated by fixture | enumeration warning if unmatched |
| F-23 | Front-matter delimiter line (`---`) appearing inside body content | parser | First `---…---` pair at file head only is front matter; later delimiters are body; body-hash check in migration proves no body mutation | migration report row |

## 17.2 Programme-level failure modes (from E1's repeated contract list — each mapped to its structural mitigation)

1. **Checker can't localize a problem** → diagnostics contract (`path: field: RULE_ID: message (fix:)`) enforced by fixture tests asserting message shape.
2. **Generated Markdown becomes a second truth** → banner + `--check` drift gate + JSON substrate is the only CI-consumed form.
3. **Legacy plan treated as executable because metadata is missing** → `legacy: true` rows are non-executable by definition; intake refuses them with the migration pointer.
4. **Identity tied to filename breaks history** → stable `PLAN_ID` + `LEGACY_SOURCE_PATH` + generated link mapping (E1K).
5. **Automation infers architecture from names** → generated fields limited to mechanical facts (file existence, dates, enumeration); capability claims require `CLAIMS.json`/evidence; clustering emits *candidates* only.
6. **CI enabled before migration → permanent warning blindness** → staged rollout with ratchet (warnings may not *increase*), not instant full enforcement.
7. **Waiver without owner/expiry** → schema-enforced, expiry mechanically fails.
8. **MERGE hides a genuinely missing capability** → every `MERGE` must name the target authority *and* the missing seam (E1G substep 4/9); seams become LINK tickets.
9. **Rubric too vague to reject** → falsifiability acceptance (E1F): each hard stop must reject a fixture; blind second-tool review of pillars.
10. **Governance becomes the bureaucracy it audits** → footprint count + governance-cost metric + simplification trigger (E1N/E1P); process additions must themselves pass intake.

---

# 18. Test Strategy

Follows `TEST_POLICY.md`: focused targets only; new tests exist because new contracts exist (schema, register, intake, rails, roadmap validation); no full-suite runs by default; xUnit via `bash scripts/run_test.sh <target>` (180 s cap); Python fixture suites run as a CI gate, not through dotnet.

## 18.1 Python fixture self-tests (`--self-test`, gate `plan_governance_selftest`)

Versioned fixture corpus under `scripts/ci/fixtures/plan_governance/` (mini plan files, rails registry, receipts, git-mock SHAs). Fixtures never depend on live plan counts (E1L substep 11). Enumerated cases (from E1 §9 test matrix, completed):

**Register (14):** valid plan; legacy plan without front matter; missing-metadata warning shape; duplicate active `PLAN_ID`; unknown status enum; unknown category enum; circular supersedence; archived stale plan (info-severity references); active stale plan (error-severity); stable ordering under shuffled filesystem enumeration; generated-drift detection; CRLF/LF variation; UTF-8 headings; bracketed filename `E1_planintegration[2].md` enumeration.

**Premise/reference (10):** path exists; path absent (`MISSING_PATH`); line range beyond EOF (`MISSING_LINE_RANGE`); renamed candidate suggested-not-applied; old SHA + valid paths stays warning; active plan with invalid required authority errors; `EXTERNAL` reference classification; ambiguous prose pseudo-path (`AMBIGUOUS`); archived historical dead link non-blocking; contradiction-by-newer-decision verdict from a recorded decision fixture.

**Intake (16):** SYSTEM without receipt → reject; SYSTEM with valid receipt → pass; panel without authority → reject; panel with authority but no mutation → reject; read-only diagnostic exception → pass; content without consumer → reject; LINK fast lane → pass; PROCESS fast lane → pass; missing rail → reject; insufficient rail readiness → reject; valid spike → pass; expired spike → reject; missing metric → reject; cross-cutting change missing rollback → reject; waiver with future expiry → pass; expired waiver → reject.

**Roadmap/register-integration (8):** NOW exceeds WIP cap → fail; NOW entry missing plan ID → fail; NOW item blocked by unresolved rail → fail; MERGE without target → fail; NOT_NOW without return condition → fail; DONE without evidence → fail; archive containing non-terminal active item → fail; deliberate roadmap/register drift → caught.

**Tool determinism (4):** two consecutive `--write` runs byte-identical; `--check` side-effect-free (tree hash before == after); `PYTHONHASHSEED` variation produces identical output; metrics JSON deterministic with zero-denominator ratios.

Total fixture cases ≈ 52, run in seconds — inside the focused-run budget.

## 18.2 xUnit contract tests (new file `Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs`)

Static assertions over the **generated artifacts** (engine-free, sub-second, mirroring `PortContractGateTests`' validation of generated policy JSON): `PLAN_REGISTER.json` parses and carries `schema_version`; `counts.total == rows.Length`; statuses/categories within enums; zero duplicate active `PLAN_ID`s; every `rails_resolution.id` exists in `rails.json`; `rails.json` states within readiness enum; roadmap NOW count ≤ `wip_cap`; every NOW entry resolves to a register row; `CLAIMS.json` unaffected (24+1 claims valid via existing verifier); register JSON↔MD consistency spot-check (row count). ≈ 12–15 cases. Run alone first (new-file rule), then as the focused target for later phases. **Note on naming:** the source plan anticipated `Ashfall.Core.Tests/PlanRegisterTests.cs`; this plan deliberately places the suite in `Tooling/` beside the repo's other gate-contract tests and scopes it to generated-artifact contracts, because executing Python from xUnit would add a fragile cross-runtime dependency the current suite does not have — the Python logic itself is covered by §18.1's fixtures. If the integrator prefers in-suite Python invocation, the fallback is one thin smoke test asserting `generate-plan-register.py --self-test` exits 0; the contract tests remain the primary gate.

## 18.3 Regression witnesses (per phase boundary, not per subtask)

`dotnet build Ashfall.csproj` (0 errors); `godot --headless --path . -- --data-integrity-selftest`; `--bridge-selftest`; `python3 scripts/ci/verify-capability-claims.py --check`; `bash scripts/ci/doc-link-gate.sh`; `python3 scripts/ci/generate-docs-index.py --check` (after any docs addition); `bash scripts/ci/sync-agent-rulebooks.py --check` (after the AGENTS.md pointer). Full `verify-fast.sh` at Phases 0, 10, and 14 (foreman-visible windows), not as a per-edit default.

## 18.4 Verification command matrix (closure set, E1 §8 retained)

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
python3 scripts/ci/generate-plan-register.py --self-test
python3 scripts/ci/generate-plan-register.py --check
python3 scripts/ci/plan-intake-check.py --self-test
bash scripts/ci/plan-intake-check.sh scripts/ci/fixtures/plan_governance/intake/invalid_system_no_receipt.md   # must refuse
python3 scripts/ci/verify-capability-claims.py --check
bash scripts/ci/doc-link-gate.sh
bash scripts/ci/sync-agent-rulebooks.py --check
bash scripts/ci/verify-fast.sh
```

Each command's exit code, duration, inherited-vs-caused failure class, determinism, mutability, and reproduction line are captured into `docs/roadmap/e1/E1_COMPLETION.md` (E1 §8 capture contract). Governance failures and runtime failures are reported as separate classes; the matrix does not stop at the first governance failure when safe to continue.

---

# 19. Dependency-Ordered Phases

Execution order obeys the corpus chain (`29A docs gates → E1A → E1B → E1C → E1D → E1E → E1F → E1G → E1H → E1I → E1J → E1K → E1L → E1M → E1N → E1O → E1P`) and the source rule: never hand-score the backlog before the register and freshness scan exist. Phase gates are explicit; a phase starts only when its predecessor's exit checks pass.

| Phase | Delivers | Depends on | Key files | Exit verification (focused) |
|---|---|---|---|---|
| **P0 — Claim & premise re-audit** | Foreman/integrator claim row; this plan's premises re-checked at execution HEAD (corpus counts, Plan 29 seal, gate baseline re-run); stale-premise corrections written into `docs/roadmap/e1/` | Queue item available (AGENTS.md active queue #6); no active claim on package paths | `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md` (integrator) | Claim row exists; premise-corrections doc lists any drift since 2026-09-19 authoring |
| **P1 — Baseline (E1A)** | `plan_governance_config.json`, baseline enumerator, `e1/E1_BASELINE.md` + `e1_baseline.json`, inherited-red record | P0 | new tooling + config | Byte-stable double enumeration; MD/JSON count parity; zero runtime files touched; baseline gate results captured |
| **P2 — Schema & register (E1B)** | Schema v1, `plan_corpus_lib.py`, `generate-plan-register.py` (all modes), fixtures, `PLAN_REGISTER.{md,json}` first generation, contract tests file | P1 | as §7 | Register self-test 14 cases + determinism 4; `--check`/`--write` round-trip; `PlanGovernanceContractTests.cs` runs alone green (new-file rule) |
| **P3 — Metadata migration (E1C)** | `migrate-plan-metadata.py`; dry-run diff; batch review queues; `--write` across the corpus; `E1_METADATA_MIGRATION_REPORT.md`; checksum parity proof | P2 | migrator + all plan files' front matter | Body hashes unchanged; baseline path parity; zero inferred `DONE`; re-run no-op; register regenerates clean over migrated corpus |
| **P4 — Freshness verifier (E1D)** | Shared reference classifier in lib; register reference-health columns; `PREMISE_HEALTH.md`; source-plan staleness examples as regression fixtures; **flagged:** `verify-capability-claims.py` shared-parser migration (byte-parity proof, integrator) or explicit deferral note | P3 (needs populated metadata) | lib, generator, (shared seam) | Premise fixture 10 cases; severity model honoured; verifier parity proven on 24 claims if migrated |
| **P5 — Overlap clusters (E1E)** | Capability vocabulary; code-authority index (from architecture map + CLAIMS.json + save-section registry); `CAPABILITY_CLUSTERS.md` candidates; receipt format live; the five source-plan example clusters resolved with evidence or recorded unsupported | P4 | generator, clusters artifact | Cluster fixtures (synthetic duplicates cluster; near-name distincts stay separate); no auto status change (test); receipt schema validated |
| **P6 — Pillars & rubric (E1F)** | `docs/design/PILLARS.md` (≤ word budget, falsifiable, non-goals), `docs/roadmap/AMBITION_RUBRIC.md`, decision-record template, ten known-backlog scoring walkthroughs, second-tool blind review recorded | P2 (register for examples); independent of P4/P5 | two authored docs | Hard-stop fixtures each reject ≥1 plausible plan; LINK-vs-SYSTEM separation demonstrated; pillars review recorded |
| **P7 — Ambition audit (E1G)** | `docs/roadmap/AMBITION_AUDIT.md`; every active plan triaged; WIP cap set in config; `MERGE` clusters → rails tickets; binding verdicts → `DECISION_REGISTER.md` rows (integrator); register metadata updated | P4, P5, P6 | audit doc, register, decision register (shared) | Triage invariants enforced by register (`R-*`); WIP cap machine-checked; second-tool disagreement adjudications recorded |
| **P8 — Rails registry (E1H)** | `rails.registry.json` (10 source rails + census-derived states, identity/voice/relations resolved from current evidence), generated `rails.json` + `RAILS.md`, cycle detector, spike-exception schema | P7 (authority-plan references must resolve) | rails files, lib | Rails fixtures: unknown ID, cycle, insufficient readiness, spike expiry, MD/JSON sync |
| **P9 — Intake checker & panel contract (E1I + E1J)** | `docs/roadmap/INTAKE.md`; `plan-intake-check.py` + `.sh`; panel-authority contract + read-only exceptions; fake-affordance smell list; back-audit of highest-risk planned panels → LINK tickets; panel-authority metric feed | P8 (rails resolution), P5 (receipts) | checker, policy doc | Intake fixture 16 cases; panel fixtures (rejection, read-only pass, authority-resolves); `--explain` output contract |
| **P10 — Numbering/archive/co-author (E1K) + CI rollout start (E1L Stages 0–1)** | README additive subsections (integrator); duplicate-ID and archive validators in generator; three gate rows in manifest `critical: false` (integrator); `AGENTS.md` pointer + rulebook resync (integrator); warning-count baseline vs E1A | P9 | README, manifest, AGENTS.md (shared seams) | E1K fixtures (duplicate ID, archive identity, E-series filenames); gates run report-only; `verify-fast.sh` green with new gates non-critical; rulebook sync `--check` green |
| **P11 — Roadmap publication (E1M)** | `docs/roadmap/ROADMAP.md` (capped NOW, dependency-ordered NEXT, NOT_NOW index by return condition, MERGE tickets, premise-review queue); handoff template adopted; roadmap/register cross-validation live | P7, P8, P10 | ROADMAP.md, generator validators | Roadmap fixtures (8); boundary sentences in README; ledger unchanged in role |
| **P12 — Metrics (E1N)** | `governance_metrics.json` + `GOVERNANCE_METRICS.md`; baseline deltas; quarterly snapshot format; governance-cost metric + simplification trigger | P1 baseline, P7–P11 data sources | metrics generator mode | Determinism + zero-denominator fixtures; every metric generated from repo data (no prose counting) |
| **P13 — Independent review (E1O)** | Review-packet template; stratified sample reviews; adjudication records archived with decision rows | P7 (verdicts exist) | `docs/roadmap/e1/reviews/` | Packet has no conclusion field; reviewer evidence-only rule checked; status mutation path unchanged |
| **P14 — Enforcement promotion & closure (E1L Stages 2–3 + E1P)** | Tier promotion after one clean wave + false-positive review; footprint count; `E1_COMPLETION.md`; census row 114 → `SEALED`; WAVE_LEDGER row; one additive `CLAIMS.json` claim; ownership transferred; `docs/INDEX.md` regenerated; migrator archived | P10–P13 | completion doc + shared seams | Full §18.4 matrix from a clean tree; baseline-vs-final runtime parity; no unclassified active plan; no unexplained permanent warning |

Parallelization note: P6 may overlap P3–P5 (different files, no dependency); P13's template may be drafted during P7. Everything else is strictly ordered. Each phase ends with the §18.3 regression witnesses, not the full suite.

---

# 20. File Impact Map

Every planned change, with its reason. "New" files do not exist today (verified §2.2). Shared-seam rows name the mediating owner.

| File | Change | Reason |
|---|---|---|
| `scripts/ci/plan_governance_config.json` | New | Include/exclude patterns, WIP cap, namespace config — folder assumptions must not be hard-coded (E1A.2) |
| `scripts/ci/plan_corpus_lib.py` | New | Shared front-matter parser, schema validator, reference classifier — one parser for register + intake (+ claims verifier later) (E1D.1) |
| `scripts/ci/generate-plan-register.py` | New | Register/freshness/rails/metrics generator with `--check/--write/--json/--explain/--self-test` (E1B/D/H/N) |
| `scripts/ci/plan-intake-check.py` | New | Intake enforcement logic (E1I) |
| `scripts/ci/plan-intake-check.sh` | New | Gate-shaped wrapper (source 53C names the `.sh`; symmetry with existing gates) |
| `scripts/ci/migrate-plan-metadata.py` | New (archived after P3) | Body-preserving front-matter insertion; one-time tool, never a permanent surface (E1C, E1P.6) |
| `scripts/ci/fixtures/plan_governance/**` | New | ≈52 versioned fixtures for all self-tests; independent of live corpus (E1L.11) |
| `docs/roadmap/PLAN_REGISTER.md` / `.json` | New (generated) | The discovery surface + substrate (E1B) |
| `docs/roadmap/PREMISE_HEALTH.md` | New (generated) | Stale-premise queue with counts and risk order (E1D.12) |
| `docs/roadmap/CAPABILITY_CLUSTERS.md` (+ verdict JSON) | New (generated + reviewed) | Overlap clusters with evidence; human verdicts recorded (E1E) |
| `docs/roadmap/rails.registry.json` / `rails.json` / `RAILS.md` | New (authored + generated) | Rail definitions authored once; readiness resolved/generated (E1H) |
| `docs/roadmap/INTAKE.md` | New | The intake policy humans read (E1I) |
| `docs/roadmap/AMBITION_RUBRIC.md` | New | Scoring + hard stops + fast paths (E1F) |
| `docs/roadmap/AMBITION_AUDIT.md` | New | The recorded whole-backlog triage (E1G) |
| `docs/roadmap/ROADMAP.md` | New (generated-validated) | One-page capped execution view (E1M) |
| `docs/roadmap/GOVERNANCE_METRICS.md` / `governance_metrics.json` | New (generated) | Deltas vs baseline; quarterly format (E1N) |
| `docs/roadmap/e1/E1_BASELINE.md`, `e1_baseline.json`, `E1_METADATA_MIGRATION_REPORT.md`, receipts/, reviews/, `E1_COMPLETION.md` | New | Baseline, migration proof, receipts, adjudications, closure (E1A/C/G/O/P) |
| `docs/design/PILLARS.md` | New | The falsifiable design authority (E1F); placed under existing `docs/design/` |
| `Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs` | New | Static contract tests over generated JSON (§18.2); no Core production change |
| All in-scope `Plan_*.md` / corpus / piagents files | Front matter inserted/completed; bodies byte-preserved | Machine-readable status without history rewrite (E1C) |
| `docs/ci/CI_GATE_MANIFEST.json` | 3 additive rows (shared seam, integrator) | Gate registration (E1L) |
| `docs/roadmap/README.md` | Additive subsections (shared seam, integrator) | Register contract pointer; E-series naming; roadmap/ledger boundary (E1K/M) |
| `AGENTS.md` (+13 rulebooks via canonical sync) | One pointer line (shared seam, integrator) | Route plan authoring through INTAKE.md (53C.9); engine invariants byte-identical |
| `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md` | Claim + package rows (foreman/integrator) | Standard coordination (rule 6) |
| `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` row 114 | Status flips at claim/seal (integrator) | Census protocol |
| `docs/governance/DECISION_REGISTER.md` | New `DEC-xx` rows for binding triage verdicts (integrator) | Scope decisions live in the standing register (§9.2) |
| `docs/architecture/CLAIMS.json` | One additive claim at closure (integrator) | `gov_plan_register_integrity` capability evidence (E1P) |
| `docs/roadmap/WAVE_LEDGER.md` | One wave row (integrator) | Historical index completeness |
| `scripts/ci/verify-capability-claims.py` | Optional shared-parser migration with parity proof, or untouched (integrator decision) | E1D shared-detector intent without risking a sealed gate |
| `docs/INDEX.md` | Regenerated by canonical generator (integrator) | Index drift gate |
| **Untouched (hard exclusions):** `Assets/Ashfall.Core/**`, `src/**`, `Assets/StreamingAssets/Data/**`, `project.godot`, all save stores, all gameplay tests, `KNOWN_DEBT.md` content rows (no new debt invented by this package), `docs/CURRENT_AUTHORITY.md` body (pointer-only change if the integrator wants the register listed) | — | Governance tooling must not perturb runtime authority (rules 1–5) |

---

# 21. Risks

Ranked by (probability × impact), each with the structural mitigation designed into §6–§19. Extends E1 §12's register with the current-reality findings of this plan.

| Risk | P | I | Mitigation |
|---|---|---|---|
| Metadata migration creates a giant noisy diff across ~600 files | High | Medium | Body-preserving insertion, dry-run review, one dedicated commit, checksum parity proof; review batched by ambiguity class not by folder (E1C.9) |
| Register drifts into a hand-edited second truth | Medium | High | Generated banner + `--check` drift gate + JSON-only CI consumption (F-19) |
| Overlap clustering flags false duplicates | Medium | High | Candidates-only; evidence paths mandatory; human/second-tool verdict required; zero auto status changes (E1E.5) |
| Pillars too vague to reject scope | Medium | High | Falsifiability acceptance + blind second-tool review + hard-stop fixtures (E1F.11–12) |
| Intake becomes a second bureaucracy | Medium | High | Fast lanes, generated fields, governance-cost metric with simplification trigger, footprint audit at closure (E1N.12–13, E1P.7) |
| Full enforcement on an unmigrated corpus = permanent CI red | High | Medium | Staged rollout; changed-file gating first; warning-count ratchet; waivers with expiry (E1L) |
| Archive moves break historical links | Medium | Medium | Stable `PLAN_ID`, `LEGACY_SOURCE_PATH`, generated link mapping (E1K.4,8) |
| Concurrent plan authors collide | High | Medium | Reservation-before-drafting extension of README §2; duplicate active `PLAN_ID` is a register error (E1K.3,11) |
| Premise age treated as falsity | Medium | Medium | Age is a warning signal only; staleness requires evidence classes (E1D.4–6) |
| `MERGE` verdicts hide genuinely missing capability | Medium | High | Target authority + missing seam mandatory; seams become LINK tickets (E1G.4,9) |
| UI gate forces fake mutations onto read-only screens | Low | Medium | `panel_kind: read_only` declared exceptions (E1J.4,11) |
| Waivers become permanent bypass | Medium | High | Owner + expiry + mechanical failure on expiry (F-16) |
| Governance code outgrows benefit | Medium | High | Closure footprint count; migrator archived; anti-bureaucracy guardrail from source plan quoted into DoD (E1P) |
| Plan 29 sealed artifacts (`verify-capability-claims.py`, README) are perturbed by shared-lib migration | Low | High | Migration is optional/flagged, byte-parity-proven on the 24 existing claims, integrator-mediated; default is "untouched" (E1D design note) |
| Shared governance files race with ACTIVE XP-WAVE1 / WAVE11-PART2 claims | Medium | Medium | All shared edits sequenced at phase boundaries through the integrator; package paths disjoint (§2.4, §7) |
| The three-way Plan 53 identity collision confuses future agents about which "Plan 53" this is | Medium | Low | Recorded as the register's first `numbering_drift` case; census row 114 is the canonical pointer; no renames (§2.3) |

---

# 22. Out of Scope

1. **Any gameplay mechanic, panel, scene, catalog, or save schema** — including any player-facing "wishlist"/proposal surface. If such a surface is ever wanted, it is a separate proposal that must itself pass the intake this package builds (the source plan scopes E1A–E1P to repository governance; nothing in them requires an in-game surface).
2. **Execution of the follow-on queue E2–E8** (register-backed NOW-band execution, merge-cluster consolidation, premise remediation sweep, zero-consumer conversion, panel authority hardening, acceptance-ladder enforcement, quarterly review operations). This package *creates the machinery and the first triage*; it does not execute the resulting implementation tickets.
3. **Renumbering or deleting any historical plan** — collisions are recorded as `numbering_drift`; `DROP`/`MERGE`/`SUPERSEDED` are marks, never erasures.
4. **Re-adjudicating the census's 122 `AUDIT-PENDING` rows beyond what E1G triage requires** — the register ingests and re-presents; per-clause corpus re-audits remain the census's own tranche-2 work.
5. **Gameplay-facing docs restructure** (`docs/CURRENT_AUTHORITY.md` rewrite, docs taxonomy work) — pointer-level edits only.
6. **Embedding/LLM-based automatic duplicate adjudication** — clustering is deterministic-signal candidates plus human/second-tool review.
7. **Changing the runtime test suites' scope or the fast-tier runtime gates** — new gates are additive and initially non-critical.
8. **Fixing the pre-existing `WAVE_LEDGER.md` stale `proposed` row as a drive-by** — it is recorded as drift evidence; its correction rides the closure-phase ledger update by the integrator, keeping this package's diff out of shared files mid-flight.
9. **Migrating `piagentsplans/` content into active status** — that namespace stays historical-reference per README §2; it receives metadata and registration, not promotion.

---

# 23. Rollback Strategy

This package is process-heavy and runtime-light; rollback is correspondingly simple and is designed in, per E1 §11:

1. **Generated artifacts:** every generated file regenerates from front matter + config; deleting them and re-running `--write` reproduces the exact state. No generated file is ever a sole source of truth.
2. **CI gates:** any new gate downgrades from blocking to report-only by flipping `critical` in the manifest — metadata and tooling stay intact. A faulty rule is waived with an expiry while evidence is preserved (never silently disabled).
3. **Metadata migration:** plan bodies are byte-preserved (hash-proven), and front-matter insertion is version-control-revertible per file; the migration report enumerates every touched file for surgical revert.
4. **Archive moves:** reversible via recorded `LEGACY_SOURCE_PATH` + stable `PLAN_ID`.
5. **Pillars/rubric changes:** decision-record gated; never silently rewritten.
6. **Blast-radius limiters by construction:** clustering cannot change statuses; the premise scanner is independent of runtime tests; intake rules are individually waivable; no gameplay save migration exists to roll back; no runtime authority was created to satisfy metadata.
7. **Whole-package abort:** removing the three manifest gate rows + the new `scripts/ci` and `docs/roadmap` artifacts returns the repository to its pre-package governance state; front matter left behind is inert metadata that breaks nothing (parser-tolerant by design).

---

# 24. Definition of Done

This package is a **Plan-class deliverable** under the binding roadmap README §3.5 ("Plan Done"), applied to governance tooling, plus the E1 programme outcomes (§1). All of the following must hold; "compile-green is not proof of runtime integration" applies here as "script-exists is not proof of enforcement".

- [ ] Every acceptance clause of E1A–E1P (§6.4 per-item acceptance bullets) verified against source, with zero "probably done" items.
- [ ] Register covers 100% of in-scope plans; zero unclassified active plans; `--check` exit 0 on a clean tree.
- [ ] Premise verifier live with severity model; source-plan staleness examples pinned as regression fixtures.
- [ ] Overlap clusters reviewed; the five source-plan example clusters resolved with evidence or recorded as unsupported.
- [ ] Pillars + rubric pass falsifiability acceptance and blind second-tool review.
- [ ] Whole backlog triaged; every `MERGE` names a target authority; every `NOT_NOW` names a return condition; every `DROP` cites evidence + decider and exists as a `DECISION_REGISTER.md` row; WIP cap machine-enforced.
- [ ] Rails generated from authoritative metadata; unknown-rail and cycle rejection proven; identity(40A)/voice(42)/relations(44A) rail states resolved from current evidence, not assumed.
- [ ] Intake checker refuses the enumerated rejection fixtures and only for the intended rules; panel-authority contract enforced with read-only exceptions.
- [ ] Numbering/E-series/co-author/archive policy published additively in README; duplicate active `PLAN_ID` fails; archive identity preservation proven.
- [ ] CI rollout at its declared tier with zero unexplained permanent warnings; waiver expiry mechanically enforced; governance vs runtime results separable in fast-tier output.
- [ ] Roadmap published, register-validated, NOW-capped; handoff template adopted.
- [ ] Metrics deterministic, baseline-referenced, zero-denominator-safe; governance-cost metric present with simplification trigger.
- [ ] Independent review packets archived; adjudications recorded.
- [ ] `E1_COMPLETION.md` contains baseline→final metric deltas, residual debt, accepted waivers, and follow-on plan IDs; footprint counted; migrator archived.
- [ ] Implementation log written (this package's phases logged in `docs/roadmap/e1/`); terminal status set in `INTEGRATION_PLANS.md` + `WORKTREE_OWNERSHIP.md`; census row 114 `SEALED`; `WAVE_LEDGER.md` row; one additive `CLAIMS.json` claim green under `verify-capability-claims.py --check`.
- [ ] All pre-existing gates green at closure (§18.4 matrix from a clean tree), with inherited-vs-caused failure accounting.
- [ ] No gameplay source, data, save, or UI file modified at any point (proven by phase-boundary diffs against the E1A baseline).
- [ ] Anti-bureaucracy check passed: permanent footprint = 4 scripts + 1 config + fixtures + 1 test file + the authored docs; anything beyond is justified or removed.
- [ ] `E1_planintegration[2].md` and `[3].md` recorded as reserved sequence names.

---

# 25. Implementation Handoff

**Package:** `E1` / Plan 53 — Ambition Governance & Intake. **Category:** PROCESS+LINK+GOVERNANCE. **Runtime risk:** LOW. **Governance risk:** HIGH (handled by staged rollout + integrator-mediated shared seams). **Entry gate:** standard census claim protocol (no special foreman signature named in the census); Phase P0 re-audits this plan's premises at execution HEAD before any file is created.

**What the implementing builder needs to know in one paragraph:** everything this package builds is static Python tooling plus generated Markdown/JSON plus front-matter metadata; it never touches the runtime; its design centre is *extend, never duplicate* — the register consumes CLAIMS.json, the census, the wave ledger, the architecture map, and the decision register rather than re-authoring any of them; enforcement arrives in stages so a 600-file legacy corpus never turns CI permanently red; and the programme is forbidden from growing past its own intake form — the closure phase audits the footprint.

## MUST PRESERVE

- Godot-authoritative / engine-free Core / JSON-data-authoritative / deterministic-RNG / one-authority-per-concern invariants — this package adds zero runtime surface, so preservation means *proving* zero drift per phase (§18.3 witnesses), not merely asserting it.
- Historical plan identity: filenames, numbers, and bodies byte-preserved; the three-way Plan 53 collision recorded as drift, never "fixed" by renaming (§2.3).
- Plan 29's sealed machinery (`CLAIMS.json`, `verify-capability-claims.py`, `docs/roadmap/README.md`, `WAVE_LEDGER.md`) as the sole owners of their concerns; `verify-capability-claims.py` changes only via the flagged parity-proven migration.
- `DECISION_REGISTER.md`'s verdict vocabulary and cadence; triage verdicts map onto it, never redefine it (§9.2).
- Existing ledgers' roles: `INTEGRATION_PLANS.md` (execution queue), `WORKTREE_OWNERSHIP.md` (claims), `KNOWN_DEBT.md` (debt) — the roadmap feeds them, never replaces them.
- Active claims' paths (XP-WAVE1, WAVE11-PART2): all shared governance edits are integrator handoffs at phase boundaries.
- The 29 corpus files' existing front-matter values during migration (mapping table, not overwrite).

## MUST ADD

- `scripts/ci/plan_governance_config.json`, `plan_corpus_lib.py`, `generate-plan-register.py`, `plan-intake-check.py`, `plan-intake-check.sh`, `migrate-plan-metadata.py`, `fixtures/plan_governance/**` (package-owned tooling).
- Generated artifacts: `PLAN_REGISTER.{md,json}`, `PREMISE_HEALTH.md`, `CAPABILITY_CLUSTERS.md`, `rails.json` + `RAILS.md`, `ROADMAP.md`, `GOVERNANCE_METRICS.{md,json}`.
- Authored artifacts: `rails.registry.json`, `INTAKE.md`, `AMBITION_RUBRIC.md`, `AMBITION_AUDIT.md`, `docs/design/PILLARS.md`, `docs/roadmap/e1/**`.
- `Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs` (static contract tests, §18.2) and the ≈52-case fixture self-test suites (§18.1).
- Front matter on every in-scope plan across all three namespaces (body-preserving).
- Three CI manifest gate rows (integrator), one `AGENTS.md` pointer line + rulebook resync (integrator), README additive subsections (integrator), decision rows for binding verdicts (integrator), closure ledger updates: census 114 `SEALED`, wave row, one `CLAIMS.json` claim (integrator).

## MUST NOT DO

- No gameplay code, data, panel, scene, save section, RNG stream, or `project.godot` change — under any phase, for any reason.
- No deleting or renumbering plans; no inferring `DONE` from prose; no inferring `PREMISE_VERIFIED_AT` without an immutable SHA or an actual verification pass.
- No second capability registry, docs index, decision log, or numbering policy; no mutable state outside front matter and authored registries; no hand-edited generated files.
- No auto-adjudication from clustering or embeddings; no status changes without the governed workflow; reviewer tools never write status.
- No full-enforcement-on-day-one; no gating legacy archived history through modern intake; no permanent or ownerless waivers; no warnings without `path: field: RULE_ID: fix` diagnostics.
- No Unity anything; no `System.Random` anywhere (including tooling decisions that affect output — tool determinism via sorted iteration); no process additions that don't themselves pass intake.
- No mass-formatting of shared areas; no unrelated refactors; no edits to shared governance files outside the named phase-boundary handoffs.

## VERIFY WITH

- Per phase: the phase's focused fixtures/tests (§18.1–18.2 via `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs` and the scripts' `--self-test` modes) plus the §18.3 regression witnesses.
- Stage proofs: `python3 scripts/ci/generate-plan-register.py --check` (drift), `bash scripts/ci/plan-intake-check.sh <crafted-invalid-fixture>` (refusal behaviour), waiver/spike expiry fixtures, roadmap-cap fixtures.
- Closure: the full §18.4 command matrix from a clean tree, including `bash scripts/ci/verify-fast.sh`, `godot --headless --path . -- --data-integrity-selftest`, `--bridge-selftest`, `verify-capability-claims.py --check`, `doc-link-gate.sh`, `sync-agent-rulebooks.py --check`, `generate-docs-index.py --check` — with runtime and governance results reported as separate failure classes.

## FIRST SAFE IMPLEMENTATION STEP

Phase P0 + P1 in one bounded move: have the foreman/integrator write the claim row (package-owned paths only, per §7), then run the E1A baseline — record HEAD/dirty-state/toolchain versions, author `scripts/ci/plan_governance_config.json` with explicit include/exclude patterns, enumerate the three namespaces twice, and prove byte-stable counts + count parity between `docs/roadmap/e1/E1_BASELINE.md` and `e1_baseline.json`, while capturing the current fast-tier gate results as the inherited-red baseline. This touches no plan file, no shared ledger beyond the claim row, and no runtime path; it produces the immutable yardstick every later phase's metrics, migration parity, and completion deltas are measured against, and it is fully reversible by deleting `docs/roadmap/e1/`.

---

*End of plan. Source authority: `Next-steps-plans/shipped_to_chat/Plan_53_Ambition_Audit_Expansion_Intake.md` + `C-integration-plans/E1_planintegration.md`; governance authority: `docs/roadmap/README.md`, `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` row 114, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`. This document plans; it does not implement.*
