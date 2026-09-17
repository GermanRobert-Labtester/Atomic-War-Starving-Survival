---
PLAN_ID: E1
PLAN_FAMILY: planintegration
STATUS: READY_FOR_EXECUTION
SOURCE_PLAN: "Plan 53 — Ambition Audit & Expansion Intake"
SEQUENCE_FILENAME: "E1_planintegration.md"
NEXT_FILENAMES:
  - "E1_planintegration[2].md"
  - "E1_planintegration[3].md"
CATEGORY: PROCESS+LINK+GOVERNANCE
PRIMARY_INTENT: "Convert plan sprawl into an evidence-gated, machine-readable, execution-oriented roadmap."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
NO_HISTORY_DELETION: true
RUNTIME_RISK: LOW
DOCUMENTATION_RISK: MEDIUM
GOVERNANCE_RISK: HIGH
---

# E1 — Flagship Plan Integration: Ambition Governance, Rails Readiness, and Evidence-Gated Execution

> **Naming sequence:** this is `E1_planintegration.md`. The next files in this exact sequence are
> `E1_planintegration[2].md`, `E1_planintegration[3].md`, `E1_planintegration[4].md`, and so on.
> The bracketed sequence number belongs immediately before `.md`.

## 0. Mission

This plan converts the Plan 53 ambition-audit concept into an implementation-grade integration programme.
The objective is not merely to write three governance documents. The objective is to make plan governance,
premise freshness, duplicate detection, rails readiness, roadmap triage, UI-authority validation, and plan
intake executable properties of the repository.

The source audit establishes the central problem: the project has accumulated multiple plan namespaces,
large plan/doc counts, no universal status markers, stale premises, overlapping capability proposals,
hundreds of unconsumed effect catalogs, and UI surfaces that can exist without authoritative runtime
backing. The source therefore proposes three core moves: make the backlog machine-readable, audit ambition
against explicit pillars, and enforce an intake gate for new work. E1 takes those moves and expands them
into a sequenced programme with migration strategy, schemas, test matrices, CI adoption, rollback rules,
metrics, ownership, and follow-on execution tasks.

E1 is deliberately biased toward **LINK**, **PROCESS**, and **PRESENTATION-AUTHORITY** work rather than
new systems. A new system is only justified when the register and rails analysis prove that no suitable
authority already exists and that the new authority can be owned, saved, surfaced, and tested.

## 1. Source Baseline and Non-Negotiable Facts

The source plan's verified baseline is treated as an input contract for this plan:

- `Next-steps-plans/` contains a large mixed continuity/expansion backlog.
- `piagentsplans/` contains a second large plan namespace.
- `docs/` contains a third large documentation body.
- Existing plan files do not universally declare `STATUS` or `PREMISE_VERIFIED_AT`.
- Stale plan premises have already been found repeatedly across prior waves.
- Expansion proposals can duplicate capabilities already present in radio, triangulation, gossip,
  memory, needs, governance, and related systems.
- A large number of catalogs can produce effects without any verified consumer.
- Prior UI audits demonstrate the danger of static or routed panels existing before real Core/host
  authority exists.
- The repository lacks one concise design-pillar document capable of rejecting scope.
- Prior governance work already proposed roadmap/index/gate infrastructure and should be reused rather
  than replaced.
- No plan is deleted merely because it is stale, merged, dropped, or superseded. Historical evidence is
  retained.

These are not optional context notes. They define the acceptance surface for E1.

## 2. Programme Outcomes

E1 is complete only when all of the following are true:

1. Every in-scope plan is represented in one generated plan register.
2. Every active plan has machine-readable status and premise metadata.
3. Referenced files and capability claims can be freshness-checked automatically.
4. Duplicate or overlapping plans are surfaced as clusters before new implementation begins.
5. ASHFALL has a short, falsifiable design-pillar document.
6. Every backlog item is triaged into an explicit execution bin.
7. Every `MERGE` decision names the live authority to extend.
8. Every `DROP` or `NOT_NOW` decision contains evidence and a reversible decision record.
9. Rails readiness is generated from authoritative status rather than manually duplicated prose.
10. New system plans cannot pass intake without a duplicate search, rails declaration, acceptance target,
    and named metric.
11. New UI surfaces cannot pass intake without naming an authority, save ownership, and mutating action.
12. Plan-register and intake validation run in CI with a staged adoption path.
13. The roadmap has a small capped `NOW` band and an explicit sequencing model.
14. Archive/retirement rules prevent completed work from being rediscovered as unfinished.
15. Concurrent authors have a collision policy for numbering and folder ownership.
16. Quarterly re-audit can be performed from generated data rather than re-reading hundreds of files.
17. Follow-on integration work is expressed as rails tickets rather than parallel duplicate systems.
18. All existing runtime build/test/selftest gates remain green.

## 3. Programme Guardrails

- Do not introduce gameplay systems while implementing E1.
- Do not silently renumber historical plans.
- Do not delete superseded plans.
- Do not infer plan completion from filename or age alone.
- Do not mark a premise verified unless a commit SHA or equivalent immutable reference is recorded.
- Do not call a capability duplicate merely because names are similar; require code/registry/evidence.
- Do not let `MERGE` become a euphemism for unbounded scope growth.
- Do not let the register become an alternative project-management database with duplicated truth.
- Do not introduce a panel-intake rule that cannot be mechanically checked.
- Do not gate the repository on a new checker until a baseline report and migration window exist.
- Do not weaken existing engine invariants to make metadata migration easier.
- Do not require authors to fill fields that can be generated deterministically.
- Do not copy status into multiple independently editable tables.
- Do not use vague acceptance states such as `mostly done`, `probably live`, or `appears connected`.
- Prefer evidence classes: `CODE`, `DATA`, `TEST`, `RUNTIME`, `UI`, `DOC`, `COMMIT`, `DECISION`.
- Prefer negative capability: a plan must be able to say "not now" and remain valid.

## 4. Execution Order

The default dependency chain is:

`29A docs gates → E1A baseline → E1B schema/register → E1C metadata migration → E1D freshness verifier
→ E1E overlap clusters → E1F pillars/rubric → E1G ambition audit → E1H rails → E1I intake checker
→ E1J UI authority gate → E1K numbering/archive/co-author rules → E1L CI rollout → E1M roadmap publication
→ E1N metrics/review → E1O independent audit → E1P closure`

Do not execute the ambition audit before the register and freshness scan exist. The purpose of the register is
to convert a 200+ document reading problem into an evidence-ranked decision problem.


---

## E1A — Preflight, repository freeze, and baseline evidence capture

**Goal:** Create a reproducible snapshot of the plan/doc landscape before any metadata migration changes the corpus.

**Primary files/surfaces:** `scripts/ci/`, `docs/roadmap/`, `Next-steps-plans/`, `piagentsplans/`, `docs/`, CI manifests, current wave indexes.

### Required substeps

1. Record `HEAD`, branch, dirty state, toolchain versions, Godot version, .NET SDK version, Python version, and the exact UTC timestamp used for the baseline.
2. Enumerate all candidate plan documents using explicit include patterns. Write the patterns into a machine-readable config so the register generator does not hard-code folder assumptions.
3. Count plans by namespace, numbering family, extension, and apparent numeric range. Preserve the raw file list as a baseline artifact.
4. Enumerate markdown documentation outside plan folders and classify obvious roadmap, design, debug, audit, index, and generated files without mutating them.
5. Run existing docs/index/rulebook gates before E1 changes. Capture which failures predate E1 so the programme does not claim regressions it inherited.
6. Run build, unit-test, integrity-selftest, bridge-selftest, and fast verification. Store command, exit code, duration, and abbreviated failure output.
7. Create `docs/roadmap/e1/E1_BASELINE.md` with immutable baseline facts and links to generated reports.
8. Create a compact JSON baseline (`docs/roadmap/e1/e1_baseline.json`) for tests and later metric deltas.
9. Explicitly list known source-plan evidence that must remain explainable after migration: multi-namespace numbering, stale premises, overlap clusters, zero-consumer catalogs, and fake-affordance findings.
10. Mark baseline data as generated/read-only where appropriate so later authors do not manually repair counts.
11. Define the baseline exclusion list for archived vendor docs, generated third-party docs, packages, cache folders, and anything not treated as project planning authority.
12. Add a smoke test that proves the baseline enumerator returns a stable sorted list across two consecutive runs.

### Implementation contract

- E1A must be executable independently once its declared prerequisites are green.
- Changes must preserve historical plan identity and must not introduce gameplay behavior as a side effect.
- Generated outputs must be deterministic, reviewable in diffs, and reproducible locally.
- Ambiguity must be surfaced as a structured warning or review queue; it must not be silently guessed.
- Every status-changing action must leave evidence that can be linked from the plan register.
- Any new author burden introduced by this task must have a concrete repository benefit and a testable rule.

### Failure modes to actively probe

- A checker reports a problem but cannot identify the exact plan/path/field responsible.
- Generated Markdown becomes a second editable source of truth and drifts from structured data.
- A legacy plan is accidentally treated as executable because metadata is missing.
- Historical links break because identity is tied only to the current filename.
- An apparently useful automation creates false confidence by inferring architectural facts from names.
- A CI rule is enabled before the corpus is migrated, producing permanent warning blindness.
- An exception/waiver has no owner or expiry and becomes an invisible permanent bypass.

### Verification

- [ ] Baseline file list is byte-stable on unchanged HEAD.
- [ ] Counts in Markdown and JSON agree.
- [ ] Existing red gates are clearly marked inherited rather than caused by E1.
- [ ] No runtime source file is modified by E1A.

### Definition of Done

A reproducible pre-migration snapshot exists, commands are captured, and every later E1 metric can be expressed as a delta from this baseline.

---

## E1B — Canonical plan metadata schema and generated register

**Goal:** Make plan state queryable without opening each document and establish one generated register as the discovery surface.

**Primary files/surfaces:** new `scripts/ci/generate-plan-register.py`, new `docs/roadmap/PLAN_REGISTER.md`, schema/config files, tests.

### Required substeps

1. Define a canonical metadata schema containing at least `PLAN_ID`, `STATUS`, `CATEGORY`, `WAVE`, `PREMISE_VERIFIED_AT`, `SUPERSEDES`, `SUPERSEDED_BY`, `OWNER`, `RAILS_REQUIRED`, `METRIC_MOVED`, `ACCEPTANCE_TIER`, and `SOURCE_AUTHORITY`.
2. Specify enumerations for status (`PROPOSED`, `PREMISE_STALE`, `READY`, `IN_PROGRESS`, `BLOCKED`, `DONE`, `MERGED`, `NOT_NOW`, `DROPPED`, `SUPERSEDED`, `ARCHIVED`) and category (`SYSTEM`, `LINK`, `CONTENT`, `PRESENTATION`, `PROCESS`).
3. Separate author-owned fields from generated fields. File existence, SHA age, duplicate clusters, and referenced-path checks must be generated rather than hand-entered.
4. Create a parser tolerant of legacy Markdown without front matter so migration can be staged. Legacy files must surface as `METADATA_MISSING`, not crash the generator.
5. Normalize plan IDs without renaming files. Retain the original path and filename as historical identity.
6. Generate `PLAN_REGISTER.md` sorted by execution relevance: active status, blocked status, stale premise, then archive/history.
7. Generate a machine-readable sibling such as `PLAN_REGISTER.json` for CI/tests. Markdown is presentation; JSON is the integration substrate.
8. Include columns for status, category, wave, premise SHA, premise age, reference health, rails, metric, overlap cluster, and path.
9. Add `--check`, `--write`, `--json`, and `--explain <plan-id>` modes. `--check` must be deterministic and side-effect free.
10. Make errors actionable: include file path, field, invalid value, allowed values, and suggested remediation.
11. Add a schema-version field so future metadata changes can be migrated deliberately rather than silently.
12. Add fixtures for legacy plan, valid active plan, stale reference, superseded plan, circular supersedence, unknown rail, and invalid category.
13. Document the register contract in `docs/roadmap/README.md` and point to it from agent instructions without copying the full schema into every rulebook.

### Implementation contract

- E1B must be executable independently once its declared prerequisites are green.
- Changes must preserve historical plan identity and must not introduce gameplay behavior as a side effect.
- Generated outputs must be deterministic, reviewable in diffs, and reproducible locally.
- Ambiguity must be surfaced as a structured warning or review queue; it must not be silently guessed.
- Every status-changing action must leave evidence that can be linked from the plan register.
- Any new author burden introduced by this task must have a concrete repository benefit and a testable rule.

### Failure modes to actively probe

- A checker reports a problem but cannot identify the exact plan/path/field responsible.
- Generated Markdown becomes a second editable source of truth and drifts from structured data.
- A legacy plan is accidentally treated as executable because metadata is missing.
- Historical links break because identity is tied only to the current filename.
- An apparently useful automation creates false confidence by inferring architectural facts from names.
- A CI rule is enabled before the corpus is migrated, producing permanent warning blindness.
- An exception/waiver has no owner or expiry and becomes an invisible permanent bypass.

### Verification

- [ ] Idempotent generation.
- [ ] Stable sort order independent of filesystem enumeration order.
- [ ] Legacy file produces a structured warning.
- [ ] Invalid enum fails with precise diagnostics.
- [ ] Supersedence cycles are detected.
- [ ] `--check` exits non-zero on drift and zero on an exact generated state.

### Definition of Done

One generated register can answer what plans exist, their state, their freshness, their rails, and where to open them.

---

## E1C — Corpus-wide metadata migration without rewriting history

**Goal:** Add canonical metadata to legacy plan files while preserving content, numbering, links, and authorship history.

**Primary files/surfaces:** all in-scope `Plan_*.md`, selected piagents plans, indexes, migration reports.

### Required substeps

1. Build a migration script that can insert front matter without reformatting the Markdown body. Preserve line endings and body bytes wherever practical.
2. Classify fields by confidence. High-confidence fields may be inferred mechanically from filename/path/index; low-confidence fields are emitted as TODOs in a migration report, not guessed.
3. Infer `PLAN_ID` from filename only when unambiguous. Otherwise create a stable synthetic legacy ID and require review.
4. Infer broad category from known directories and explicit headings only as a provisional value marked `INFERRED: true` until reviewed.
5. Populate `PREMISE_VERIFIED_AT` only when the plan already names an immutable commit or when an explicit verification pass is performed.
6. Do not infer `DONE` from words such as 'implemented' in narrative text. Completion requires evidence or existing accepted status.
7. Create `docs/roadmap/e1/E1_METADATA_MIGRATION_REPORT.md` listing migrated, ambiguous, malformed, and manually reviewed files.
8. Run migration in dry-run mode first and diff the corpus. Refuse any transformation that changes non-front-matter body text unexpectedly.
9. Batch manual review by ambiguity class rather than by folder: identity ambiguity, status ambiguity, category ambiguity, rails ambiguity, supersedence ambiguity.
10. Add explicit `LEGACY_SOURCE_PATH` if a plan has a historical path likely to move to archive later.
11. Retain dropped/superseded content in place until the archive policy is active; migration is not the archival step.
12. After review, regenerate the plan register and compare count parity with the baseline enumerator.
13. Create a one-time migration checksum report proving no plan disappeared.

### Implementation contract

- E1C must be executable independently once its declared prerequisites are green.
- Changes must preserve historical plan identity and must not introduce gameplay behavior as a side effect.
- Generated outputs must be deterministic, reviewable in diffs, and reproducible locally.
- Ambiguity must be surfaced as a structured warning or review queue; it must not be silently guessed.
- Every status-changing action must leave evidence that can be linked from the plan register.
- Any new author burden introduced by this task must have a concrete repository benefit and a testable rule.

### Failure modes to actively probe

- A checker reports a problem but cannot identify the exact plan/path/field responsible.
- Generated Markdown becomes a second editable source of truth and drifts from structured data.
- A legacy plan is accidentally treated as executable because metadata is missing.
- Historical links break because identity is tied only to the current filename.
- An apparently useful automation creates false confidence by inferring architectural facts from names.
- A CI rule is enabled before the corpus is migrated, producing permanent warning blindness.
- An exception/waiver has no owner or expiry and becomes an invisible permanent bypass.

### Verification

- [ ] Body hashes are unchanged after metadata insertion except at the front-matter boundary.
- [ ] All baseline plan paths are represented after migration.
- [ ] No plan silently becomes `DONE`.
- [ ] Dry-run and write mode produce the same proposed changes.
- [ ] Migration can be re-run safely.

### Definition of Done

Every active/in-scope plan is machine-readable, ambiguous metadata is explicit, and historical content remains intact.

---

## E1D — Premise freshness, dead-reference, and capability-claim verification

**Goal:** Turn stale-premise detection from an occasional audit technique into a reusable verification service shared by plan and capability checks.

**Primary files/surfaces:** `scripts/ci/verify-capability-claims.py`, register generator shared modules, fixtures, docs.

### Required substeps

1. Extract path/reference parsing into a shared library consumed by both the register generator and capability-claim verifier.
2. Recognize repository-relative paths, path:line references, glob-like plan references, and named plan IDs without treating prose punctuation as part of the path.
3. Classify reference state as `OK`, `MISSING_PATH`, `MISSING_LINE_RANGE`, `RENAMED_CANDIDATE`, `AMBIGUOUS`, or `EXTERNAL`.
4. Compare `PREMISE_VERIFIED_AT` against current HEAD age without assuming age alone makes a premise false. Age is a review signal, not proof of staleness.
5. Define premise-stale rules: missing authority path, invalid referenced symbol when symbol evidence is available, explicit contradiction from a newer accepted decision, or manual review verdict.
6. Allow a plan to remain old but valid when its referenced authority and invariants still hold; avoid churn based solely on dates.
7. Create a structured evidence record for every stale verdict so the ambition audit can cite the machine output.
8. Provide `--plan <id>`, `--changed-since <sha>`, and `--all-active` modes to support local and CI workflows.
9. Add rename heuristics only as suggestions. Never automatically rewrite a plan reference because a similarly named file exists.
10. Integrate source-plan examples as regression fixtures: absent historical files, moved docs, stale numerical claims, and duplicate capability assertions.
11. Add severity levels: error for active plan pointing to absent required authority, warning for old-but-valid verification SHA, info for archived history.
12. Generate a compact `PREMISE_HEALTH.md` report with counts and highest-risk stale plans.
13. Ensure the checker never imports the game runtime or mutates content; it is static repository analysis.

### Implementation contract

- E1D must be executable independently once its declared prerequisites are green.
- Changes must preserve historical plan identity and must not introduce gameplay behavior as a side effect.
- Generated outputs must be deterministic, reviewable in diffs, and reproducible locally.
- Ambiguity must be surfaced as a structured warning or review queue; it must not be silently guessed.
- Every status-changing action must leave evidence that can be linked from the plan register.
- Any new author burden introduced by this task must have a concrete repository benefit and a testable rule.

### Failure modes to actively probe

- A checker reports a problem but cannot identify the exact plan/path/field responsible.
- Generated Markdown becomes a second editable source of truth and drifts from structured data.
- A legacy plan is accidentally treated as executable because metadata is missing.
- Historical links break because identity is tied only to the current filename.
- An apparently useful automation creates false confidence by inferring architectural facts from names.
- A CI rule is enabled before the corpus is migrated, producing permanent warning blindness.
- An exception/waiver has no owner or expiry and becomes an invisible permanent bypass.

### Verification

- [ ] Deleted-file fixture is flagged.
- [ ] Moved-file candidate is suggested but not auto-fixed.
- [ ] Old SHA with valid paths remains warning rather than automatic failure.
- [ ] Archived stale references do not block CI.
- [ ] Active required-authority missing path can block CI once rollout reaches enforcement.

### Definition of Done

Premise freshness is evidence-backed, explainable, and reusable by register, intake, and quarterly audit workflows.

---

## E1E — Duplicate-topic clustering and live-capability mapping

**Goal:** Find where proposed work overlaps existing systems and convert overlap into integration opportunities rather than parallel authorities.

**Primary files/surfaces:** register JSON, code/registry indexes, `docs/CURRENT_AUTHORITY.md`, new `docs/roadmap/CAPABILITY_CLUSTERS.md`.

### Required substeps

1. Define a capability vocabulary aligned to actual repository domains: information flow, radio, triangulation, gossip, memory, needs, disease, weather, expedition, governance, identity, trade, shelter, power, relations, journal, UI, save, etc.
2. Extract plan-topic signals from title, metadata, referenced files, rails, and explicitly named systems. Do not rely on embeddings alone for final decisions.
3. Build a code authority index from class/system names, registries, host wiring, save sections, and known runtime entry points.
4. Generate candidate overlap clusters with confidence and evidence paths.
5. Require a human or second-tool review before a candidate cluster can cause `MERGED` or `NOT_NOW` status.
6. For each confirmed cluster, name the existing authorities that should be extended and the integration seams that are currently missing.
7. Distinguish semantic overlap from implementation duplicate: two plans may discuss memory but target different ownership or lifecycle concerns.
8. Create explicit cluster records for the source-plan examples: information-flow, per-NPC memory, governance, needs cascade, and food pipeline where evidence supports them.
9. For every cluster, ask whether the correct next action is `LINK`, `CONTENT`, `PRESENTATION`, or genuinely `SYSTEM`.
10. Create a `duplicate_search_receipt` format containing queries, files inspected, systems found, and reviewer conclusion.
11. Feed cluster IDs into the plan register so future intake can check whether a new plan enters a known overlap domain.
12. Do not delete duplicate plans; cross-link them with `SUPERSEDES`/`SUPERSEDED_BY` or `MERGED_INTO` metadata.
13. Publish a short 'extension before invention' decision rule in the roadmap docs.

### Implementation contract

- E1E must be executable independently once its declared prerequisites are green.
- Changes must preserve historical plan identity and must not introduce gameplay behavior as a side effect.
- Generated outputs must be deterministic, reviewable in diffs, and reproducible locally.
- Ambiguity must be surfaced as a structured warning or review queue; it must not be silently guessed.
- Every status-changing action must leave evidence that can be linked from the plan register.
- Any new author burden introduced by this task must have a concrete repository benefit and a testable rule.

### Failure modes to actively probe

- A checker reports a problem but cannot identify the exact plan/path/field responsible.
- Generated Markdown becomes a second editable source of truth and drifts from structured data.
- A legacy plan is accidentally treated as executable because metadata is missing.
- Historical links break because identity is tied only to the current filename.
- An apparently useful automation creates false confidence by inferring architectural facts from names.
- A CI rule is enabled before the corpus is migrated, producing permanent warning blindness.
- An exception/waiver has no owner or expiry and becomes an invisible permanent bypass.

### Verification

- [ ] Known synthetic duplicates cluster together.
- [ ] Two similarly named but semantically distinct fixtures remain separate.
- [ ] Cluster records contain evidence paths.
- [ ] No plan status changes automatically from clustering alone.

### Definition of Done

The backlog can identify where new scope should become a rails/integration ticket against live authorities.

---

## E1F — Design pillars and falsifiable ambition rubric

**Goal:** Create the minimum design authority needed to accept or reject scope without relying on personal preference.

**Primary files/surfaces:** new `docs/design/PILLARS.md`, new `docs/roadmap/AMBITION_RUBRIC.md`, decision-record templates.

### Required substeps

1. Draft four or five short pillars that describe player-facing truths, not implementation technologies.
2. Each pillar must be falsifiable: a proposed feature should be able to contradict it in a way reviewers can explain.
3. Keep examples subordinate to the pillar. The pillar file must remain short enough to be read before planning.
4. Include explicit non-goals: no 3D conversion, no unrelated genre layer, no live-service requirements, no procedurally generated open world, no dialogue-tree platform merely for its own sake, and no code-mod platform unless later re-approved.
5. Define the ambition rubric using source-plan questions: connect versus accumulate, rails readiness, fake-console risk, day-2 player impact, and blocking/dependency effect.
6. Add scoring dimensions for authority ownership, save ownership, testability, content demand, presentation burden, migration burden, and metric moved.
7. Use a weighted score only for prioritization support; never let arithmetic override a hard blocker such as missing authority or contradicted pillar.
8. Define hard-stop rules: duplicate live authority without extension rationale; UI with no mutating authority; new save type with no migration owner; content wave with no acceptance route.
9. Define positive fast-path signals: closes a known link gap, converts zero-consumer data into gameplay, removes fake affordance, increases runtime evidence, or retires duplicate authority.
10. Create a one-page decision record template capturing decision, evidence, alternatives, reversibility, and reviewer.
11. Run the pillars against at least ten known backlog examples and document ambiguous cases. If everything scores similarly, the rubric is too vague.
12. Have a second tool review pillars without seeing the desired outcomes; ask whether the text can actually reject features.
13. Version the pillars and require decision records for substantive changes.

### Implementation contract

- E1F must be executable independently once its declared prerequisites are green.
- Changes must preserve historical plan identity and must not introduce gameplay behavior as a side effect.
- Generated outputs must be deterministic, reviewable in diffs, and reproducible locally.
- Ambiguity must be surfaced as a structured warning or review queue; it must not be silently guessed.
- Every status-changing action must leave evidence that can be linked from the plan register.
- Any new author burden introduced by this task must have a concrete repository benefit and a testable rule.

### Failure modes to actively probe

- A checker reports a problem but cannot identify the exact plan/path/field responsible.
- Generated Markdown becomes a second editable source of truth and drifts from structured data.
- A legacy plan is accidentally treated as executable because metadata is missing.
- Historical links break because identity is tied only to the current filename.
- An apparently useful automation creates false confidence by inferring architectural facts from names.
- A CI rule is enabled before the corpus is migrated, producing permanent warning blindness.
- An exception/waiver has no owner or expiry and becomes an invisible permanent bypass.

### Verification

- [ ] Rubric can clearly distinguish a LINK repair from a speculative SYSTEM addition.
- [ ] At least one plausible plan is rejected or deferred by each hard-stop rule fixture.
- [ ] Pillars remain under a deliberately small line/word budget.
- [ ] Decision records are linkable from the register.

### Definition of Done

Scope decisions can be defended using project principles plus repository evidence rather than intuition.

---

## E1G — Whole-backlog ambition audit and ordered triage

**Goal:** Apply the register, freshness report, clusters, and rubric to produce a small executable roadmap plus explicit deferrals.

**Primary files/surfaces:** new `docs/roadmap/AMBITION_AUDIT.md`, `ROADMAP.md`, register metadata updates, decision records.

### Required substeps

1. Score the generated register first; open full documents only for high-priority, contested, stale, or high-overlap cases.
2. Triage every active candidate into `NOW`, `NEXT`, `MERGE`, `NOT_NOW`, `DROP`, or `NEEDS_PREMISE_REVIEW`.
3. Keep `NOW` deliberately small. Set an explicit WIP cap and publish it in the roadmap.
4. Every `MERGE` verdict must name an existing live authority or accepted plan that receives the work.
5. Every `DROP` verdict must cite a pillar, contradiction, duplicate, or cost/benefit reason and remain reversible through a decision record.
6. Every `NOT_NOW` verdict must name the condition under which it may return: rail completion, acceptance tier, runtime evidence, budget, or roadmap milestone.
7. Every `NEEDS_PREMISE_REVIEW` item remains non-executable until its source assumptions are revalidated.
8. Sequence `NEXT` by dependency and metric impact, not numeric plan order.
9. Convert overlap clusters into consolidated rails tickets with bounded scope and explicit existing systems to extend.
10. Identify plans that are primarily content but are blocked by missing consumers; move them behind the appropriate acceptance/consumer rail.
11. Identify plans that are primarily presentation but are blocked by missing Core/host authority; move them behind E1J or authority completion.
12. Identify plans already implemented in substance; require completion evidence and archive rather than reimplementation.
13. Publish a one-page `ROADMAP.md` with waves, target outcomes, metrics, and links. Keep detailed reasoning in the audit document.
14. Run a second-tool blind review using register + pillars + evidence summaries, compare disagreements, and record adjudication.

### Implementation contract

- E1G must be executable independently once its declared prerequisites are green.
- Changes must preserve historical plan identity and must not introduce gameplay behavior as a side effect.
- Generated outputs must be deterministic, reviewable in diffs, and reproducible locally.
- Ambiguity must be surfaced as a structured warning or review queue; it must not be silently guessed.
- Every status-changing action must leave evidence that can be linked from the plan register.
- Any new author burden introduced by this task must have a concrete repository benefit and a testable rule.

### Failure modes to actively probe

- A checker reports a problem but cannot identify the exact plan/path/field responsible.
- Generated Markdown becomes a second editable source of truth and drifts from structured data.
- A legacy plan is accidentally treated as executable because metadata is missing.
- Historical links break because identity is tied only to the current filename.
- An apparently useful automation creates false confidence by inferring architectural facts from names.
- A CI rule is enabled before the corpus is migrated, producing permanent warning blindness.
- An exception/waiver has no owner or expiry and becomes an invisible permanent bypass.

### Verification

- [ ] Every active plan has exactly one triage state.
- [ ] No `MERGE` lacks a target authority.
- [ ] No `NOT_NOW` lacks a return condition.
- [ ] No `NOW` item is blocked by an unmet hard rail.
- [ ] WIP cap is machine-checkable from register metadata.

### Definition of Done

The repository has one ordered roadmap and an explicit, evidence-backed not-now surface; no active plan remains in limbo.

---

## E1H — Rails readiness registry and dependency truth

**Goal:** Make cross-cutting prerequisites visible and generated so plans cannot claim readiness while their foundational rails are absent.

**Primary files/surfaces:** new `docs/roadmap/RAILS.md`, `RAILS.json`, plan metadata, register generator.

### Required substeps

1. Define rail IDs for graph/topology, intel, identity, voice, policy, relation outcomes, seasons, commitments, acceptance ladder, port contract, save/migration, UI authority, metrics, and other accepted cross-cutting prerequisites.
2. Each rail record must name its authoritative plan/system, status, evidence, consumer classes, and blocking semantics.
3. Generate rail status from authoritative plan metadata where possible. Do not hand-edit the Markdown table.
4. Allow composite rail states when a foundational plan is done but runtime acceptance is partial; represent this with explicit readiness levels rather than prose.
5. Define `NOT_STARTED`, `IN_FLIGHT`, `CODE_READY`, `RUNTIME_VERIFIED`, `PRESENTED`, and `DONE` readiness semantics if the project needs more resolution than a binary state.
6. Map each active plan's `RAILS_REQUIRED` entries to the registry and flag unknown rail IDs.
7. Add a dependency-cycle detector so two plans cannot both claim the other as an unmet prerequisite without explicit break strategy.
8. Expose rail consumers in the generated report to show blast radius and help sequencing.
9. Add a 'missing rail but accepted spike' exception that requires a timebox and decision output.
10. Make intake validation compare the plan's required readiness level against the rail's actual readiness, not merely string equality.
11. Include the source-plan named rails as initial records and preserve their associated plan references.
12. Use rails status in the roadmap sort so downstream content does not leapfrog its consumer/authority.
13. Document who is allowed to change a rail's authoritative source and how that change is reviewed.

### Implementation contract

- E1H must be executable independently once its declared prerequisites are green.
- Changes must preserve historical plan identity and must not introduce gameplay behavior as a side effect.
- Generated outputs must be deterministic, reviewable in diffs, and reproducible locally.
- Ambiguity must be surfaced as a structured warning or review queue; it must not be silently guessed.
- Every status-changing action must leave evidence that can be linked from the plan register.
- Any new author burden introduced by this task must have a concrete repository benefit and a testable rule.

### Failure modes to actively probe

- A checker reports a problem but cannot identify the exact plan/path/field responsible.
- Generated Markdown becomes a second editable source of truth and drifts from structured data.
- A legacy plan is accidentally treated as executable because metadata is missing.
- Historical links break because identity is tied only to the current filename.
- An apparently useful automation creates false confidence by inferring architectural facts from names.
- A CI rule is enabled before the corpus is migrated, producing permanent warning blindness.
- An exception/waiver has no owner or expiry and becomes an invisible permanent bypass.

### Verification

- [ ] Unknown rail fails validation.
- [ ] Dependency cycles are detected.
- [ ] Insufficient readiness blocks an executable plan.
- [ ] Spike exception requires expiry/decision fields.
- [ ] Generated Markdown and JSON remain synchronized.

### Definition of Done

Prerequisite readiness is a queryable repository property and becomes a real scheduling constraint.

---

## E1I — Plan intake form and executable intake checker

**Goal:** Prevent future plan sprawl by requiring evidence before SYSTEM, PRESENTATION, or new-resource work can become executable.

**Primary files/surfaces:** new `docs/roadmap/INTAKE.md`, new `scripts/ci/plan-intake-check.sh` or Python equivalent, schema tests.

### Required substeps

1. Define intake fields directly in front matter or a structured `INTAKE` block: pillars touched, category, rails, duplicate-search receipt, authority, save owner, fake-console risk, day-2 player change, metric moved, acceptance tier, rollback plan, and reviewer.
2. Define lightweight fast lanes for `PROCESS`, small `LINK`, and documentation hygiene work while retaining minimum identity/status/evidence fields.
3. Require full intake for `SYSTEM`, new `PRESENTATION` surfaces, save-schema changes, new resource types, and broad content waves.
4. Reject executable status if required rails are below the declared readiness threshold unless a named prerequisite or spike exception exists.
5. Reject a SYSTEM plan lacking a duplicate-search receipt.
6. Reject a panel plan lacking Core/host authority, read model, mutating action, and save ownership where state is persisted.
7. Reject a content plan lacking an acceptance route or consumer.
8. Require a named metric moved. Allow qualitative metrics only if the measuring method is specified.
9. Require a rollback/disable path for cross-cutting system changes before implementation begins.
10. Provide `--explain` output that prints each failed rule and how to fix it.
11. Support checking one plan locally and all changed plans in CI.
12. Use git diff to scope PR enforcement after full corpus migration; do not force every historical archived plan through modern intake.
13. Create fixtures for missing rail, missing duplicate receipt, panel without authority, content without consumer, valid LINK fast lane, valid spike, and valid SYSTEM plan.
14. Document examples of valid and invalid intake blocks using real project terminology.

### Implementation contract

- E1I must be executable independently once its declared prerequisites are green.
- Changes must preserve historical plan identity and must not introduce gameplay behavior as a side effect.
- Generated outputs must be deterministic, reviewable in diffs, and reproducible locally.
- Ambiguity must be surfaced as a structured warning or review queue; it must not be silently guessed.
- Every status-changing action must leave evidence that can be linked from the plan register.
- Any new author burden introduced by this task must have a concrete repository benefit and a testable rule.

### Failure modes to actively probe

- A checker reports a problem but cannot identify the exact plan/path/field responsible.
- Generated Markdown becomes a second editable source of truth and drifts from structured data.
- A legacy plan is accidentally treated as executable because metadata is missing.
- Historical links break because identity is tied only to the current filename.
- An apparently useful automation creates false confidence by inferring architectural facts from names.
- A CI rule is enabled before the corpus is migrated, producing permanent warning blindness.
- An exception/waiver has no owner or expiry and becomes an invisible permanent bypass.

### Verification

- [ ] Each required rejection fixture fails for the intended reason.
- [ ] Fast-lane LINK fixture passes without irrelevant bureaucracy.
- [ ] Changed-plan mode ignores untouched archived history.
- [ ] Checker output is deterministic and human-readable.

### Definition of Done

A new plan cannot become executable by default; it must prove its place in the existing architecture and roadmap.

---

## E1J — Presentation-authority contract and fake-affordance prevention

**Goal:** Move the documented fake-console lesson into plan-time and CI-time validation for future UI work.

**Primary files/surfaces:** intake schema, UI authority registry, docs, relevant host/composition documentation.

### Required substeps

1. Define the minimum authority contract for a gameplay panel: authoritative system/host, read model, mutating command/action, persistence owner if stateful, error/loading/disabled semantics, and evidence test.
2. Create a lightweight UI authority registry or generated view sourced from existing composition/registry data where practical.
3. For every active PRESENTATION plan, require a named authority and consumer/command path.
4. Mark read-only diagnostic/debug panels explicitly so they are not forced to invent mutating actions.
5. Define a fake-affordance smell list: button with no authoritative command, local UI-only state standing in for gameplay state, duplicated validation, static placeholder data, host bypass, save-unaware mutation.
6. Add a plan-intake rule that fails new interactive panels missing the authority contract.
7. Add optional static checks for known registration patterns if the repository has stable conventions; keep them warning-only until false-positive rate is understood.
8. Back-audit the highest-risk existing planned panels and convert failures into LINK/remediation tickets rather than broad redesigns.
9. Require one integration test or runtime selftest per authoritative mutation path for new panels.
10. Require keyboard/accessibility concerns to remain presentation requirements but not substitutes for authority wiring.
11. Document explicit exceptions: purely informational codex, debug-only views, static legal/about pages, and non-gameplay menus.
12. Track the metric `interactive_panels_with_verified_authority / interactive_panels_total` and use it in quarterly review.

### Implementation contract

- E1J must be executable independently once its declared prerequisites are green.
- Changes must preserve historical plan identity and must not introduce gameplay behavior as a side effect.
- Generated outputs must be deterministic, reviewable in diffs, and reproducible locally.
- Ambiguity must be surfaced as a structured warning or review queue; it must not be silently guessed.
- Every status-changing action must leave evidence that can be linked from the plan register.
- Any new author burden introduced by this task must have a concrete repository benefit and a testable rule.

### Failure modes to actively probe

- A checker reports a problem but cannot identify the exact plan/path/field responsible.
- Generated Markdown becomes a second editable source of truth and drifts from structured data.
- A legacy plan is accidentally treated as executable because metadata is missing.
- Historical links break because identity is tied only to the current filename.
- An apparently useful automation creates false confidence by inferring architectural facts from names.
- A CI rule is enabled before the corpus is migrated, producing permanent warning blindness.
- An exception/waiver has no owner or expiry and becomes an invisible permanent bypass.

### Verification

- [ ] Panel-without-authority intake fixture fails.
- [ ] Read-only diagnostic panel fixture passes with declared exception.
- [ ] Authority registry can be generated deterministically.
- [ ] At least one integration fixture proves UI command reaches the named authority.

### Definition of Done

Future UI cannot become 'finished' while detached from gameplay authority, and the failure is caught before implementation.

---

## E1K — Numbering, concurrent authorship, retirement, and archive discipline

**Goal:** Stop plan collisions and resurrection of finished work without rewriting the project's historical numbering.

**Primary files/surfaces:** `docs/roadmap/README.md`, `WAVE_LEDGER.md`, archive folders, agent rulebook pointers.

### Required substeps

1. Publish the historical numbering policy: preserve continuity plans below their established range, preserve expansion numbers, do not renumber history solely for neatness.
2. Define the E-series integration-plan filename sequence separately: `E1_planintegration.md`, then `E1_planintegration[2].md`, `E1_planintegration[3].md`, etc.
3. Define ownership for concurrent plan authors: reservation mechanism, folder responsibility, collision resolution, and who can retire or supersede another author's active plan.
4. Prefer stable plan IDs in metadata over filenames for cross-links so future archival moves do not break semantic references.
5. Create an archive policy: only `DONE`, `SUPERSEDED`, `MERGED`, `DROPPED`, or explicitly historical items can move; archive moves preserve metadata and completion/decision evidence.
6. Require `COMPLETED_AT` and `COMPLETION_EVIDENCE` before `DONE` can archive.
7. Create archive indexes generated from metadata so historical work remains searchable.
8. Update references through stable IDs or generated link mapping when files move; do not mass-edit prose blindly.
9. Define duplicate-filename behavior for non-E-series files and reject accidental overwrites in tooling.
10. Document branch/PR collision guidance for two agents creating plans concurrently.
11. Add a register warning when two active files claim the same `PLAN_ID`.
12. Add a stale-active warning for plans untouched beyond a threshold without status review; warning only, not auto-archive.
13. Point `AGENTS.md` and synchronized rulebooks to the single policy location instead of duplicating the policy body.

### Implementation contract

- E1K must be executable independently once its declared prerequisites are green.
- Changes must preserve historical plan identity and must not introduce gameplay behavior as a side effect.
- Generated outputs must be deterministic, reviewable in diffs, and reproducible locally.
- Ambiguity must be surfaced as a structured warning or review queue; it must not be silently guessed.
- Every status-changing action must leave evidence that can be linked from the plan register.
- Any new author burden introduced by this task must have a concrete repository benefit and a testable rule.

### Failure modes to actively probe

- A checker reports a problem but cannot identify the exact plan/path/field responsible.
- Generated Markdown becomes a second editable source of truth and drifts from structured data.
- A legacy plan is accidentally treated as executable because metadata is missing.
- Historical links break because identity is tied only to the current filename.
- An apparently useful automation creates false confidence by inferring architectural facts from names.
- A CI rule is enabled before the corpus is migrated, producing permanent warning blindness.
- An exception/waiver has no owner or expiry and becomes an invisible permanent bypass.

### Verification

- [ ] Duplicate active PLAN_ID fails.
- [ ] Archive move preserves plan identity.
- [ ] E-series filename examples validate against documented sequence.
- [ ] Historical numbering remains unchanged.

### Definition of Done

Plan identity, sequencing, authorship, and retirement are explicit enough that parallel agents cannot silently collide or restart completed work.

---

## E1L — CI rollout, enforcement tiers, and developer ergonomics

**Goal:** Adopt governance checks without turning an inherited red tree into permanent noise or blocking all work at once.

**Primary files/surfaces:** CI manifest/workflows, scripts, docs, fast verification.

### Required substeps

1. Introduce new checks in report-only mode first: register drift, metadata completeness, premise health, intake compliance on changed plans, rails validity, duplicate IDs.
2. Capture warning counts in CI artifacts and compare against the E1 baseline.
3. Define rollout tiers: Tier 0 local/report, Tier 1 changed-file enforcement, Tier 2 full active-plan enforcement, Tier 3 quality thresholds.
4. Gate only new/changed executable plans during the first enforcement stage so legacy debt is visible but not paralyzing.
5. Require zero new metadata debt: warning count may remain non-zero initially, but PRs cannot increase it without an accepted exception.
6. Integrate register `--check` into the normal fast verification path once the generated files are stable.
7. Keep runtime build/test/selftests independent so governance-script failures cannot mask gameplay regressions.
8. Add concise CI summaries with direct commands to reproduce failures locally.
9. Cache or optimize scanners if full-corpus checks materially slow the developer loop; target seconds, not minutes.
10. Ensure all scripts work on the project's supported developer OS and CI environment; avoid shell-only assumptions if cross-platform use matters.
11. Version test fixtures with the scripts and avoid tests depending on the live plan count.
12. Create a temporary waiver format with owner, reason, expiry, and decision record. Expired waivers fail.
13. Promote tiers only after one clean wave under the previous tier and documented false-positive review.

### Implementation contract

- E1L must be executable independently once its declared prerequisites are green.
- Changes must preserve historical plan identity and must not introduce gameplay behavior as a side effect.
- Generated outputs must be deterministic, reviewable in diffs, and reproducible locally.
- Ambiguity must be surfaced as a structured warning or review queue; it must not be silently guessed.
- Every status-changing action must leave evidence that can be linked from the plan register.
- Any new author burden introduced by this task must have a concrete repository benefit and a testable rule.

### Failure modes to actively probe

- A checker reports a problem but cannot identify the exact plan/path/field responsible.
- Generated Markdown becomes a second editable source of truth and drifts from structured data.
- A legacy plan is accidentally treated as executable because metadata is missing.
- Historical links break because identity is tied only to the current filename.
- An apparently useful automation creates false confidence by inferring architectural facts from names.
- A CI rule is enabled before the corpus is migrated, producing permanent warning blindness.
- An exception/waiver has no owner or expiry and becomes an invisible permanent bypass.

### Verification

- [ ] Changed-plan enforcement catches a newly invalid plan.
- [ ] Legacy untouched invalid fixture can remain warning during staged rollout.
- [ ] Waiver expiry is enforced.
- [ ] Fast verification reports governance and runtime results separately.

### Definition of Done

Governance checks are real CI gates with a controlled adoption path and acceptable developer feedback time.

---

## E1M — Roadmap publication and execution handoff

**Goal:** Transform audit output into a roadmap that agents can execute without reopening the entire governance debate on every task.

**Primary files/surfaces:** `docs/roadmap/ROADMAP.md`, wave indexes, plan register, decision records.

### Required substeps

1. Publish a one-page roadmap containing only `NOW`, near-term `NEXT`, prerequisite rails, and metric deltas.
2. Link each roadmap item to its stable plan ID and authoritative detailed plan; do not duplicate full implementation prose.
3. Assign each `NOW` item an owner or agent role, start condition, exit condition, and evidence bundle.
4. Attach the rail dependencies and acceptance tier to every roadmap item.
5. Add explicit freeze rules: no new SYSTEM item may enter `NOW` if the WIP cap is full.
6. Define replacement rules: an urgent item can displace an existing NOW item only through a short decision record identifying what is paused.
7. Publish a `NOT_NOW` index grouped by return condition so deferred ideas remain discoverable without competing for execution attention.
8. Publish `MERGE` clusters as rails tickets and identify the first integration seam for each.
9. Publish `NEEDS_PREMISE_REVIEW` as an evidence queue separate from implementation.
10. Link metrics from each roadmap wave to the measurement source defined in E1N.
11. Ensure the roadmap can be regenerated or validated against register statuses to avoid hand-maintained drift.
12. Create a standard handoff block agents must fill at completion: changed authorities, tests added, metrics moved, docs updated, follow-up plan IDs.

### Implementation contract

- E1M must be executable independently once its declared prerequisites are green.
- Changes must preserve historical plan identity and must not introduce gameplay behavior as a side effect.
- Generated outputs must be deterministic, reviewable in diffs, and reproducible locally.
- Ambiguity must be surfaced as a structured warning or review queue; it must not be silently guessed.
- Every status-changing action must leave evidence that can be linked from the plan register.
- Any new author burden introduced by this task must have a concrete repository benefit and a testable rule.

### Failure modes to actively probe

- A checker reports a problem but cannot identify the exact plan/path/field responsible.
- Generated Markdown becomes a second editable source of truth and drifts from structured data.
- A legacy plan is accidentally treated as executable because metadata is missing.
- Historical links break because identity is tied only to the current filename.
- An apparently useful automation creates false confidence by inferring architectural facts from names.
- A CI rule is enabled before the corpus is migrated, producing permanent warning blindness.
- An exception/waiver has no owner or expiry and becomes an invisible permanent bypass.

### Verification

- [ ] Roadmap NOW count respects WIP cap.
- [ ] Every NOW entry resolves to a registered plan.
- [ ] No blocked rail appears as satisfied.
- [ ] Manual roadmap edits that conflict with register state are caught.

### Definition of Done

Agents receive a bounded execution queue whose dependencies and acceptance evidence are already decided.

---

## E1N — Metrics, deltas, and quarterly governance health

**Goal:** Measure whether E1 reduces disconnected breadth rather than merely adding process documents.

**Primary files/surfaces:** new `docs/roadmap/GOVERNANCE_METRICS.md`, generated metrics JSON, quarterly reports.

### Required substeps

1. Define baseline metrics from the source audit and E1A: active plans, metadata completeness, stale premises, overlap clusters, zero-consumer catalogs, executable plans blocked by rails, fake-affordance risk, and roadmap WIP.
2. Add ratio metrics: LINK-to-SYSTEM plan ratio, plans with runtime evidence, plans with named metric, panels with verified authority, and completed-to-started work per wave.
3. Distinguish leading indicators (intake quality, rail readiness, premise freshness) from outcome indicators (consumer coverage, runtime acceptance, reduced duplicate authority).
4. Generate metrics from repository data wherever possible; avoid manual quarterly counting.
5. Set directional targets rather than arbitrary perfection. Example: zero new zero-consumer content; zero new panels without authority; declining stale-premise count.
6. Track the age distribution of active premises and the count of plans waiting on the same rail to reveal systemic bottlenecks.
7. Track `MERGE` conversion outcomes: how many duplicate proposals became links versus being left as documents.
8. Track archive throughput so the active plan folder does not grow without bound.
9. Track decision reversals to detect an overly rigid rubric; some reversals are healthy if evidence changed.
10. Publish a quarterly snapshot with deltas from baseline and previous quarter.
11. Require the quarterly review to choose at most a small number of process changes; governance itself must not grow without intake.
12. Include one explicit metric for governance cost, such as median intake completion time or CI checker duration.
13. Trigger a process simplification review if governance cost rises while disconnected-work metrics do not improve.

### Implementation contract

- E1N must be executable independently once its declared prerequisites are green.
- Changes must preserve historical plan identity and must not introduce gameplay behavior as a side effect.
- Generated outputs must be deterministic, reviewable in diffs, and reproducible locally.
- Ambiguity must be surfaced as a structured warning or review queue; it must not be silently guessed.
- Every status-changing action must leave evidence that can be linked from the plan register.
- Any new author burden introduced by this task must have a concrete repository benefit and a testable rule.

### Failure modes to actively probe

- A checker reports a problem but cannot identify the exact plan/path/field responsible.
- Generated Markdown becomes a second editable source of truth and drifts from structured data.
- A legacy plan is accidentally treated as executable because metadata is missing.
- Historical links break because identity is tied only to the current filename.
- An apparently useful automation creates false confidence by inferring architectural facts from names.
- A CI rule is enabled before the corpus is migrated, producing permanent warning blindness.
- An exception/waiver has no owner or expiry and becomes an invisible permanent bypass.

### Verification

- [ ] Metrics generator is deterministic.
- [ ] Ratios handle zero denominators.
- [ ] Quarterly snapshot references immutable baseline values.
- [ ] Metrics do not require reading arbitrary prose.

### Definition of Done

The project can show whether plan governance is producing more connected, accepted, finished work.

---

## E1O — Independent cross-tool review and adversarial audit

**Goal:** Test whether E1's decisions survive review by another model/tool that receives evidence but not the preferred conclusion.

**Primary files/surfaces:** review prompt template, audit records, decision log.

### Required substeps

1. Create a minimal review packet: plan register slice, pillars, rails, code authority evidence, and candidate decision. Exclude persuasive narrative from the original author.
2. Ask the reviewer to classify the plan independently and list missing evidence, duplicate authorities, and risk of false deferral.
3. Require reviewers to cite repository evidence rather than generate architectural claims from model knowledge.
4. Compare primary and secondary verdicts in a structured table.
5. Escalate disagreements on `DROP`, `MERGE`, or new `SYSTEM` creation; low-impact ordering disagreements need not block.
6. Record adjudication and the reason one verdict prevailed.
7. Use a small stratified sample after initial full audit: high-cost SYSTEM, high-overlap cluster, UI panel, content wave, and apparently completed legacy plan.
8. Measure reviewer agreement rate over time; very high agreement can mean the rubric is clear, while very low agreement means evidence/rules are underspecified.
9. Do not allow the second tool to modify plan statuses directly. It is a reviewer, not the authority.
10. Archive review packets with the decision record so future contributors can understand why scope was constrained.
11. Re-run adversarial review when pillars or intake hard-stop rules materially change.

### Implementation contract

- E1O must be executable independently once its declared prerequisites are green.
- Changes must preserve historical plan identity and must not introduce gameplay behavior as a side effect.
- Generated outputs must be deterministic, reviewable in diffs, and reproducible locally.
- Ambiguity must be surfaced as a structured warning or review queue; it must not be silently guessed.
- Every status-changing action must leave evidence that can be linked from the plan register.
- Any new author burden introduced by this task must have a concrete repository benefit and a testable rule.

### Failure modes to actively probe

- A checker reports a problem but cannot identify the exact plan/path/field responsible.
- Generated Markdown becomes a second editable source of truth and drifts from structured data.
- A legacy plan is accidentally treated as executable because metadata is missing.
- Historical links break because identity is tied only to the current filename.
- An apparently useful automation creates false confidence by inferring architectural facts from names.
- A CI rule is enabled before the corpus is migrated, producing permanent warning blindness.
- An exception/waiver has no owner or expiry and becomes an invisible permanent bypass.

### Verification

- [ ] Review packet contains no hidden conclusion field.
- [ ] Every reviewer claim intended to influence status includes evidence.
- [ ] Status mutation still requires the primary governed workflow.

### Definition of Done

High-impact scope decisions have independent evidence review and recorded adjudication.

---

## E1P — Programme closure, ownership transfer, and anti-bureaucracy check

**Goal:** Close E1 as an implementation programme and prevent the governance layer from becoming a self-perpetuating project.

**Primary files/surfaces:** E1 completion report, roadmap, archived migration artifacts, agent docs.

### Required substeps

1. Run every E1 verification command from a clean checkout or clean working tree.
2. Confirm all generated docs are in sync and all enforcement tiers intended for the release are active.
3. Confirm the active plan register has no unclassified items.
4. Confirm the roadmap WIP cap is respected and every NOW item has a ready rail state.
5. Confirm E-series filename policy is documented and examples include `[2]` and `[3]` placement immediately before `.md`.
6. Archive one-time migration-only scripts or clearly mark them non-routine if they should not become permanent maintenance surface.
7. Count permanent E1 scripts, docs, configs, and tests. If the governance footprint is materially larger than necessary, simplify before closure.
8. Write `docs/roadmap/e1/E1_COMPLETION.md` with baseline-to-final metric deltas, residual debt, accepted waivers, and follow-on plan IDs.
9. Transfer ongoing ownership to roadmap/register maintainers rather than an E1-specific role.
10. Add a rule that future process additions must themselves pass intake when they impose new author obligations.
11. Schedule the first quarterly ambition review from the repository's normal planning cadence, not from a separate E1 calendar.
12. Ensure no runtime behavior changed inadvertently by comparing baseline and final gameplay test/selftest outcomes.
13. Mark E1 `DONE` only after completion evidence is linked in metadata and the register is regenerated.

### Implementation contract

- E1P must be executable independently once its declared prerequisites are green.
- Changes must preserve historical plan identity and must not introduce gameplay behavior as a side effect.
- Generated outputs must be deterministic, reviewable in diffs, and reproducible locally.
- Ambiguity must be surfaced as a structured warning or review queue; it must not be silently guessed.
- Every status-changing action must leave evidence that can be linked from the plan register.
- Any new author burden introduced by this task must have a concrete repository benefit and a testable rule.

### Failure modes to actively probe

- A checker reports a problem but cannot identify the exact plan/path/field responsible.
- Generated Markdown becomes a second editable source of truth and drifts from structured data.
- A legacy plan is accidentally treated as executable because metadata is missing.
- Historical links break because identity is tied only to the current filename.
- An apparently useful automation creates false confidence by inferring architectural facts from names.
- A CI rule is enabled before the corpus is migrated, producing permanent warning blindness.
- An exception/waiver has no owner or expiry and becomes an invisible permanent bypass.

### Verification

- [ ] All baseline runtime verification remains green.
- [ ] No unclassified active plan remains.
- [ ] No permanent migration warning is left unexplained.
- [ ] Completion report contains metric deltas and residual debt.

### Definition of Done

E1 exits cleanly, leaves a small maintainable governance substrate, and hands the project back to normal execution.

---

# 5. Immediate Follow-On Task Queue After E1

The next tasks are not invitations to add more parallel systems. They are the execution work created by the
audit. Each task below should either consume an existing authority, close a measured link gap, or retire
stale scope.

## E2 — Register-Backed Roadmap Execution Wave

**Intent:** execute only the top `NOW` band produced by E1G, with one integration ticket at a time.

### Substeps

1. Freeze the initial `NOW` band and record its plan IDs.
2. For each item, verify rails at the exact readiness level required.
3. Re-run premise verification immediately before implementation.
4. Confirm the duplicate-search receipt is still valid at current HEAD.
5. Identify authority owner, save owner, presentation surface, and acceptance test.
6. Implement the smallest vertical slice that reaches runtime evidence.
7. Add tests before expanding content volume.
8. Measure the declared metric delta.
9. Complete the handoff block.
10. Mark the plan `DONE` only when acceptance evidence exists.
11. Archive/retire according to E1K.
12. Pull the next item only when the WIP cap permits.

**Exit condition:** at least one full NOW item completes from plan intake through runtime evidence and archive
without bypassing the governance flow.

## E3 — Merge-Cluster Rails Consolidation

**Intent:** turn confirmed duplicate-topic clusters into bounded integration work against live systems.

### Candidate cluster pattern

- Information-flow proposals → extend radio/triangulation/gossip/weather-intelligence authorities.
- Per-NPC memory proposals → extend existing location/phantom/standing memory authorities where semantics fit.
- Governance proposals → extend existing leadership/register/arbitration surfaces where they already own state.
- Needs cascade proposals → connect existing needs/social/health systems instead of creating a parallel needs authority.
- Food pipeline proposals → connect producers, inventory, spoilage/consumption, and presentation before adding new food abstractions.

### Substeps

1. Open the cluster evidence, not every plan in the cluster.
2. Name the canonical live authorities.
3. List missing seams separately from missing features.
4. Create one LINK ticket per seam.
5. Define data contracts at the seam.
6. Define save and replay behavior.
7. Add an integration test reproducing the disconnected current behavior.
8. Implement the link.
9. Prove the existing capability now satisfies the deferred proposal's player-facing intent where applicable.
10. Mark source plans `MERGED`/`SUPERSEDED` with evidence.
11. Re-run duplicate clustering and verify cluster pressure decreases.
12. Record metric change.

**Exit condition:** duplicate scope becomes fewer authorities with more consumers, not merely fewer documents.

## E4 — Premise Remediation Sweep

**Intent:** resolve the highest-risk stale-premise queue generated by E1D.

### Priority order

1. Active NOW/NEXT plans with missing required authority paths.
2. Plans with numerical premises that materially affect architecture.
3. Plans whose named systems were renamed/replaced.
4. Plans whose data/catalog counts affect implementation scope.
5. Plans blocked only because premise freshness is unknown.
6. Historical/archived references last.

### Substeps

- Reproduce each stale claim against current HEAD.
- Replace claims only when current evidence supports the replacement.
- If the original premise is false, re-score the plan rather than simply updating the number.
- If a new live capability now exists, route the item to MERGE/LINK.
- If the feature no longer has a valid player-facing reason, route to NOT_NOW/DROP with decision evidence.
- Update `PREMISE_VERIFIED_AT`.
- Add regression coverage for recurring stale-claim patterns.
- Regenerate register and premise-health report.

**Exit condition:** no executable plan is stale at start of implementation.

## E5 — Zero-Consumer-to-Runtime Conversion Wave

**Intent:** directly attack the source audit's most important breadth-without-connection metric.

### Substeps

1. Rank zero-consumer catalogs by player relevance, existing authority proximity, and content volume.
2. Select a small batch with a clear consumer path.
3. Identify the intended producer and consumer contract.
4. Refuse catalogs that need an entirely new system unless the intake rubric independently approves it.
5. Wire one path end-to-end.
6. Add deterministic tests proving the catalog entry is consumed.
7. Add runtime/selftest evidence.
8. Add or repair presentation only after authority/consumer wiring exists.
9. Measure effect-produced/consumer coverage delta.
10. Repeat until the selected batch is accepted.
11. Re-rank rather than blindly continuing by filename.
12. Stop if integration cost reveals a missing rail; create the rail ticket instead.

**Exit condition:** the project demonstrates a repeatable method for converting authored breadth into accepted gameplay.

## E6 — Interactive Panel Authority Hardening

**Intent:** eliminate or quarantine interactive surfaces that cannot identify their gameplay authority.

### Substeps

1. Inventory interactive panels/screens/routes.
2. Map each to authority, read model, mutation, save owner, and acceptance evidence.
3. Mark read-only/debug exceptions.
4. Flag missing mappings.
5. Prioritize high-player-frequency surfaces.
6. Write one failing integration test per selected detached panel.
7. Wire to existing authority if available.
8. Remove duplicated UI-side business logic.
9. Ensure error/disabled states reflect authority response rather than local guesses.
10. Add keyboard/accessibility verification without confusing it with authority verification.
11. Update panel-authority metric.
12. Retire obsolete static consoles rather than preserving them as dead routes.

**Exit condition:** selected high-risk panels are either genuinely wired or explicitly non-gameplay.

## E7 — Acceptance-Ladder Enforcement Across Content Waves

**Intent:** ensure authored content is not counted as implemented merely because it exists in JSON or code.

### Substeps

1. Define/confirm acceptance levels from source existence through runtime consumption and presentation.
2. Tag representative catalogs with current acceptance evidence.
3. Require content plans to declare target tier.
4. Add checks preventing high-volume content waves from entering NOW when consumer rails are absent.
5. Create a runtime evidence harness for representative entries.
6. Surface acceptance status in the register or linked content report.
7. Measure movement between tiers per wave.
8. Block 'done' status for plans that only authored data when their target tier is runtime/presented.
9. Archive plans that achieved their declared tier.
10. Keep deeper tiers as separate explicit work rather than silently inflating scope.

**Exit condition:** content completion language is tied to evidence and target tier.

## E8 — Quarterly Ambition and Governance Review

**Intent:** keep E1 alive as a lightweight discipline without turning it into a permanent programme.

### Agenda

1. Regenerate register and metrics.
2. Compare stale-premise counts.
3. Compare LINK:SYSTEM ratio.
4. Compare zero-consumer coverage.
5. Compare panel-authority coverage.
6. Review rails bottlenecks.
7. Review WIP cap violations.
8. Review waivers and expired exceptions.
9. Review MERGE cluster outcomes.
10. Sample NOT_NOW return conditions.
11. Review decision reversals.
12. Measure governance cost.
13. Simplify any rule that does not correlate with better execution evidence.
14. Re-rank the roadmap.
15. Admit new scope only through normal intake.

**Exit condition:** roadmap changes are evidence-based, deferred scope is reconsidered only when conditions changed,
and process overhead stays bounded.


---

# 6. Data Contracts

## 6.1 Suggested Plan Front Matter

```yaml
PLAN_ID: "53C"
TITLE: "Intake policy"
STATUS: READY
CATEGORY: PROCESS
WAVE: 8
PREMISE_VERIFIED_AT: "ccac926e"
PREMISE_VERIFIED_DATE: "YYYY-MM-DD"
OWNER: "role-or-agent"
PILLARS:
  - scarcity-information
RAILS_REQUIRED:
  - id: acceptance_ladder
    readiness: CODE_READY
  - id: port_contract
    readiness: RUNTIME_VERIFIED
METRIC_MOVED:
  - plan_intake_compliance
ACCEPTANCE_TIER: "PROCESS_ENFORCED"
SOURCE_AUTHORITY:
  - "docs/roadmap/INTAKE.md"
SUPERSEDES: []
SUPERSEDED_BY: []
MERGED_INTO: null
COMPLETION_EVIDENCE: []
INTAKE:
  duplicate_search_receipt: "docs/roadmap/receipts/..."
  authority: null
  save_owner: null
  day2_change: "Prevents non-ready work entering implementation."
  rollback: "Disable changed-plan CI gate and retain report-only output."
```

The exact schema may differ, but the contract must preserve four properties: stable identity, explicit state,
evidence references, and machine-validatable dependencies.

## 6.2 Suggested Rails Record

```yaml
id: acceptance_ladder
name: Acceptance Ladder
authority_plan: "45A"
state: RUNTIME_VERIFIED
evidence:
  - type: TEST
    path: "..."
consumers:
  - "content-intake"
  - "plan-register"
blocks:
  - "high-volume-content-wave"
```

## 6.3 Duplicate Search Receipt

A duplicate search receipt should be intentionally boring and reproducible:

```yaml
queries:
  - "rumour information gossip radio triangulation"
  - "LocationMemorySystem PhantomMemory standing_record_memory"
inspected:
  - "Assets/Ashfall.Core/..."
  - "src/Host/..."
  - "docs/CURRENT_AUTHORITY.md"
found_authorities:
  - "SignalTriangulationSystem"
  - "MoralChoiceGossipRuntime"
decision: "MERGE_AS_LINK"
reviewer: "..."
verified_at: "<sha>"
```

The receipt does not prove a duplicate by itself. It proves the author looked before inventing.

---

# 7. State-Machine Rules for Plans

A plan should move through a constrained state machine rather than arbitrary labels.

```text
PROPOSED
  ├─> NEEDS_PREMISE_REVIEW
  ├─> NOT_NOW
  ├─> DROPPED
  └─> READY
        ├─> BLOCKED
        └─> IN_PROGRESS
              ├─> BLOCKED
              ├─> SUPERSEDED
              ├─> MERGED
              └─> DONE
                    └─> ARCHIVED
```

Additional rules:

- `DROPPED` is a decision, not deletion.
- `MERGED` must identify a target authority/plan.
- `SUPERSEDED` must identify the successor.
- `DONE` requires completion evidence.
- `ARCHIVED` is storage state after a terminal decision.
- `NEEDS_PREMISE_REVIEW` cannot be executable.
- `BLOCKED` must name the blocker.
- A return from `NOT_NOW` requires its recorded return condition to become true or a new decision record.

---

# 8. Verification Command Matrix

The source plan already defines a strong command set. E1 retains that intent and expands the expectations
around each command.

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
python3 scripts/ci/generate-plan-register.py --check
bash scripts/ci/plan-intake-check.sh <sample-plan>
python3 scripts/ci/verify-capability-claims.py --check
bash scripts/ci/doc-link-gate.sh
bash scripts/ci/sync-agent-rulebooks.py --check
bash scripts/ci/verify-fast.sh
```

For each command, capture:

- exit code;
- wall-clock duration;
- whether failure predates E1;
- whether output is deterministic;
- whether the command mutates files;
- reproduction command;
- owner for remediation.

The complete verification pass must not stop after the first governance failure if the harness can safely
continue. Runtime regressions and governance drift are different failure classes and should both be visible.

---

# 9. Test Matrix

## 9.1 Register tests

- valid plan;
- legacy plan;
- missing metadata;
- duplicate ID;
- unknown status;
- unknown category;
- circular supersedence;
- archived stale plan;
- active stale plan;
- stable ordering;
- generated drift;
- line-ending variation;
- UTF-8 headings;
- bracketed filenames such as `E1_planintegration[2].md`.

## 9.2 Premise tests

- path exists;
- path absent;
- line range absent;
- renamed candidate;
- old SHA but valid premise;
- active plan with invalid authority;
- external reference;
- ambiguous textual pseudo-path;
- archived historical dead link;
- explicit contradiction from accepted decision.

## 9.3 Intake tests

- SYSTEM without duplicate receipt;
- SYSTEM with valid receipt;
- panel without authority;
- panel with authority but no mutation;
- read-only diagnostic exception;
- content without consumer;
- LINK fast lane;
- PROCESS fast lane;
- missing rail;
- insufficient rail readiness;
- valid spike;
- expired spike;
- missing metric;
- missing rollback for cross-cutting change;
- waiver with expiry;
- expired waiver.

## 9.4 Roadmap tests

- NOW exceeds cap;
- NOW item missing plan ID;
- NOW item blocked by rail;
- MERGE missing target;
- NOT_NOW missing return condition;
- DONE missing evidence;
- archive contains non-terminal active item;
- roadmap/register drift.

---

# 10. Rollout Strategy

## Stage 0 — Observe

Generate reports only. Do not block. Measure baseline warnings and script runtime.

## Stage 1 — Protect New Work

Gate only new or changed executable plans. Legacy debt remains visible but cannot increase.

## Stage 2 — Migrate Active Work

Require full metadata and premise health for `NOW` and `NEXT`.

## Stage 3 — Enforce Rails and Intake

Block new SYSTEM/PRESENTATION/content waves that fail hard-stop rules.

## Stage 4 — Full Active-Plan Governance

Require all active plans to be registered, classifiable, and freshness-reviewed.

## Stage 5 — Optimization

Reduce checker latency, simplify fields, remove dead waivers, and tune false-positive rules.

Promotion between stages requires evidence from the preceding stage. Do not jump directly to full enforcement
because a script exists.

---

# 11. Rollback and Recovery

E1 is process-heavy but runtime-light. That makes rollback straightforward if designed correctly.

1. Every generated file can be regenerated from plan metadata.
2. CI gates can be downgraded from blocking to report-only without deleting metadata.
3. Metadata migration should be reversible from version control because plan bodies are preserved.
4. Archive moves are reversible because stable IDs and original paths are recorded.
5. Pillar/rubric changes require decision records; they are not rewritten silently.
6. A faulty duplicate-cluster heuristic cannot change statuses automatically, limiting blast radius.
7. A faulty premise scanner can be disabled independently from runtime tests.
8. A faulty intake rule can be waived temporarily with expiry while preserving the failing evidence.
9. No gameplay save migration should be required by E1 itself.
10. No runtime authority should be created merely to satisfy governance metadata.

---

# 12. Risk Register

| Risk | Probability | Impact | Mitigation |
|---|---:|---:|---|
| Metadata migration causes huge noisy diffs | High | Medium | body-preserving insertion, batch review, dedicated commit |
| Register becomes manually edited truth | Medium | High | generated banner + `--check` |
| Overlap clustering creates false duplicates | Medium | High | evidence + human review; no auto-status changes |
| Pillars are too vague to reject anything | Medium | High | falsifiability tests + adversarial review |
| Intake becomes bureaucracy | Medium | High | fast lanes, generated fields, governance-cost metric |
| CI blocks legacy debt all at once | High | Medium | staged changed-file rollout |
| Archive breaks links | Medium | Medium | stable IDs and generated link mapping |
| Concurrent agents collide | High | Medium | reservation/ownership rules |
| Old premise is treated as false solely due to age | Medium | Medium | age = warning; contradiction requires evidence |
| MERGE hides real missing capability | Medium | High | name exact authority and missing seam |
| UI gate forces mutations onto read-only screens | Low | Medium | explicit read-only/debug exceptions |
| Waivers become permanent | Medium | High | owner + expiry + CI enforcement |
| Governance code outgrows benefit | Medium | High | anti-bureaucracy closure check |

---

# 13. Suggested Evidence Classes

Use a compact evidence vocabulary:

- `CODE`: authoritative implementation path/symbol.
- `DATA`: catalog/JSON/resource definition.
- `TEST`: deterministic unit/integration test.
- `RUNTIME`: selftest, headless run, gameplay trace, or verified runtime event.
- `UI`: presented player-facing behavior.
- `SAVE`: persistence round-trip or migration evidence.
- `DOC`: accepted design/roadmap authority.
- `COMMIT`: immutable revision reference.
- `DECISION`: signed/recorded scope decision.
- `METRIC`: generated quantitative outcome.

A plan can cite multiple evidence classes. `DOC` alone should not prove runtime completion.

---

# 14. Definition of “Connected”

A capability is not considered connected merely because all participating files exist.

For E1 planning purposes, a connected gameplay capability should satisfy the relevant subset of:

1. authoritative producer exists;
2. authoritative consumer exists;
3. data contract between them is explicit;
4. host/composition wiring reaches both;
5. error/unavailable behavior is defined;
6. save/load ownership exists if state persists;
7. deterministic test proves the interaction;
8. runtime/selftest evidence proves registration/wiring;
9. player presentation exists if the feature is player-facing;
10. the declared metric reflects the connection.

This definition is intentionally stronger than “compiled” and weaker than “fully polished.” It is an
integration acceptance concept.

---

# 15. Definition of “New System”

A plan should be treated as a new SYSTEM if it introduces a new durable gameplay authority with its own
state/lifecycle/rules, even when the file name says “manager,” “bridge,” “coordinator,” or “service.”

A LINK does not become a SYSTEM merely because it needs a small adapter. The review should ask:

- Does the new code own novel durable state?
- Does it define rules another authority could reasonably own?
- Does it require a new save section?
- Does it create a parallel source of truth?
- Would deleting it remove a gameplay concept, or only a connection?

If the answer is “connection only,” prefer LINK.

---

# 16. Definition of “Done”

For governance/process plans:

- implementation exists;
- deterministic tests exist;
- generated outputs are in sync;
- CI enforcement level is declared;
- docs point to the canonical source;
- baseline delta is measured;
- completion evidence is linked.

For integration/gameplay plans governed by E1:

- premise is fresh;
- rails are ready;
- authority is named;
- consumer path exists;
- save behavior is verified where applicable;
- tests pass;
- runtime evidence exists;
- player presentation is verified if in scope;
- metric moved or the failure to move it is explained;
- handoff block is complete.

---

# 17. Handoff Template for Every Follow-On Execution Plan

```markdown
## Handoff

- Plan ID:
- HEAD / completion commit:
- Status:
- Authority extended:
- New authority introduced (if any):
- Rails consumed:
- Files changed:
- Save schema changed: yes/no
- Tests added:
- Runtime/selftest evidence:
- UI/presentation evidence:
- Metric before:
- Metric after:
- Known residual gaps:
- Plans merged/superseded:
- New follow-on plan IDs:
- Decision records:
```

Agents should not write a narrative victory lap in place of this block. The point is to make the next
session able to continue from evidence.

---

# 18. E1 Completion Checklist

- [ ] E1A baseline captured.
- [ ] E1B canonical schema and register implemented.
- [ ] E1C active corpus metadata migrated.
- [ ] E1D premise verifier integrated.
- [ ] E1E overlap clusters reviewed.
- [ ] E1F pillars and rubric accepted.
- [ ] E1G whole backlog triaged.
- [ ] E1H rails generated.
- [ ] E1I intake checker active.
- [ ] E1J panel-authority rules active.
- [ ] E1K numbering/archive/co-author policy published.
- [ ] E1L CI rollout reaches agreed enforcement tier.
- [ ] E1M roadmap published and WIP-capped.
- [ ] E1N metrics baseline and quarterly report format exist.
- [ ] E1O independent review completed.
- [ ] E1P completion report written.
- [ ] Build/test/selftests remain green.
- [ ] No active executable plan has unknown status.
- [ ] No new SYSTEM plan lacks duplicate-search evidence.
- [ ] No new interactive gameplay panel lacks named authority.
- [ ] No MERGE item lacks a target.
- [ ] No NOT_NOW item lacks a return condition.
- [ ] No DONE item lacks completion evidence.
- [ ] No waiver lacks an owner and expiry.
- [ ] `E1_planintegration[2].md` is reserved as the next sequence name.
- [ ] `E1_planintegration[3].md` is reserved as the following sequence name.

---

# 19. Final Execution Directive

E1 should be executed as a repository integration programme, not as a documentation cleanup. The decisive
test is whether future work becomes harder to start when it is disconnected and easier to finish when it
extends a live authority.

The source audit's warning is the governing constraint: ASHFALL's planning volume has outpaced its ability
to prove connection, freshness, and acceptance. The remedy is not less ambition; it is ambition under
rails. The register tells the project what exists. Premise verification tells it what is still true.
Capability clustering tells it what already exists in pieces. Pillars tell it what belongs. Rails tell it
what is ready. Intake tells it what may start. The roadmap tells it what is next. Runtime evidence tells it
what is actually finished.

Once those functions are working, expansion becomes safer because every new plan has to enter through a
known architecture rather than around one.
