# Plan 29 — One Truth: Documentation, Canon, and the Instructions Agents Actually Read

> **Wave:** Continuity Wave 3 — *Ship It Intact* (closing plan)
> **Depends on:** results of Waves 1–3 (this plan is where their claims get recorded).
> **Run 29A immediately** — three critical CI gates are red *right now*.
>
> **Theme:** ASHFALL's truth is spread over 13 agent rulebooks, 119 `docs/*.md`, 14 root
> `*.md`, 4 planning folders with three numbering schemes, a 93 KB "unified master execution plan",
> and a canon registry that overstates capabilities. The gates that police this are **currently
> failing**, and the instruction file every agent obeys tells them to wire expansions into a class
> that does not exist. When the record is wrong, good agents confidently do the wrong work — which
> is exactly how Waves 1 and 2's findings stayed invisible.

---

## Evidence Inventory (gates re-run by me at `ccac926e` + working tree)

### 🔴 Three critical fast-tier gates are failing right now

| Gate (`docs/ci/CI_GATE_MANIFEST.json`, 46 gates / 45 fast / all `critical=True`) | Command | Result |
|---|---|---|
| `agent_rulebooks_sync` | `python3 scripts/ci/sync-agent-rulebooks.py --check` | **FAIL — exit 1**: "12 client rulebook(s) drifted from canonical AGENTS.md": `CLAUDE.md`, `CODEX.md`, `CRUSH.md`, `GOOSE.md`, `QWEN.md`, `VIBE.md`, `MIMOCODE.md`, `OPENSETUP.md`, `ANTIGRAVITY.md`, `.clinerules`, `.cursorrules`, `.windsurfrules` |
| `docs_index_drift` | `python3 scripts/ci/generate-docs-index.py --check` | **FAIL**: `docs/INDEX.md` is out of sync with repository markdown |
| `agent_skills_catalog_drift` | `python3 scripts/ci/generate-agent-skills-catalog.py --check` | **FAIL**: `docs/agents/AGENT_SKILLS_INDEX.md` out of date |
| `doc_link_portability` | `bash scripts/ci/doc-link-gate.sh` | PASS (1,173 files, portable relative links) |
| `triad-drift-gate` | `bash scripts/ci/triad-drift-gate.sh` | PASS (sections ↔ Setup/Save, exemptions documented) |
| `warning-baseline-gate` | `bash scripts/ci/warning-baseline-gate.sh` | PASS (0 warnings, all targets) |

### The instruction layer is internally inconsistent

| # | Fact | Evidence |
|---|---|---|
| 1 | **`GEMINI.md` contains another client's rules** | it is 4,802 bytes beginning `# Antigravity Agent Rules — ASHFALL Project`, while `ANTIGRAVITY.md` (35,818 B) is an AGENTS.md copy — the two crossed at some point |
| 2 | …and it is **outside the sync contract** | `sync-agent-rulebooks.py`'s docstring lists 12 client files; `GEMINI.md` appears in none of them and never in that file. A drifted stub is therefore invisible to the gate that exists to catch it |
| 3 | **`AGENTS.md` tells agents to wire into a class that doesn't exist** | `AGENTS.md:236` "**Phase 4** — wire into `GameBootstrap`…"; `:240` `GameBootstrap.Phase0Expansion.cs` stubs; `:241` "`GameBootstrap` is a 1225-line god object across 82 partial files". `find . -name "GameBootstrap*"` → **no files**. `docs/ASHFALL_CODE_INDEX.md:181` confirms it was Unity wiring, "NOT to be ported; each subsystem host in Godot replaces a slice". Also referenced in `docs/CI.md:97` and `docs/GODOT_MIGRATION_STATUS.md:62` |
| 4 | **H7 describes a file that isn't there** | `AGENTS.md` H7: "`src/Main.cs` … ~6.5k-line file", "31 Setup / 24 Save + SaveAll / 17 Flush". Reality: `src/Main.cs` = **80 lines**; `src/Main*.cs` = **56 files / 14,361 lines**; counts are **72 / 69 / 26**; the canon registry §26.4 claims a third set (**7,014 lines, 38/30/18**) |
| 5 | Resolved issues still advertised as open | **H5** "Utility AI forked — Core vs Godot host (`src/UtilityAI/`)": `Assets/Ashfall.Core/UtilityAI/` has the 4 real files, `src/UtilityAI/` has only `UtilityAiPanel.cs`. **H11** "`JournalSystem` … still untested": `JournalSystemTests.cs` + `JournalSystemCoreBehaviorTests.cs` exist |
| 6 | Canon registry false positive | §26.3 asserts nondeterminism at `AirlockSecuritySystem.cs:80` via `GetHashCode()` — `grep` in that file: no match |
| 7 | Other audits carry stale rows | `docs/data/DATA_GAP_AUDIT.md` lists `questline_master.json` as an orphan (loaded at `Main.Application.cs:392`); `docs/audio/SILENCE_AUDIT.md` §4.7 claims no death event (`NeedsSystem.cs:79 OnDied`); `Next-steps-plans/Plan_14_…md` claims the weather forecast is never surfaced (`src/UI/WeatherForecastPanel.cs` is routed) |
| 8 | Planning truth is four folders deep | `Next-steps-plans/` (14, 15–19, 20–24, 25–29, 131–138 — three numbering schemes, two active authors), `piagentsplans/` (00–129 + README), `docs/plans/` (1 file), root `ASHFALL_UNIFIED_MASTER_EXECUTION_PLAN.md` (93 KB), plus `sources.md` (50 KB) marked historical |
| 9 | The index that should help is the one out of date | `docs/CURRENT_AUTHORITY.md` (71 lines) is the designated navigation map and lists `REPO_REVIEW_REPORT.md` / `COMPREHENSIVE_GAME_AUDIT.md` as historical — but neither file exists in the repo anymore, and the index itself is not what `docs/INDEX.md` (stale) generates from |
| 10 | Canon decisions live in code, not docs | the 272 codex-only `narrative/` catalogs are declared non-gameplay by a C# exemption object with a rationale string and ticket (`Assets/Ashfall.Core/Content/ContentExemption.cs`), and `echoes.json` is "deferred" by an `ExpiryCondition` field — policy that no document states |
| 11 | Working tree is the largest divergence | **95 uncommitted paths** on `main`, which is also why gate results differ between machines and agents |

**Reading:** agents trust these files. `AGENTS.md` is loaded into every session in this workspace —
including the instruction to wire into a nonexistent class. Fixing the record is not
administrative; it is the cheapest way to stop generating the same findings a third time.

---

## Task 29A — Make the record green, then keep it green

**Goal:** clear the three red gates, repair the rulebook layer (including the client the gate
ignores), and make gate-red a state that cannot persist for more than one commit.

**Files:** the 13 rulebook files, `scripts/ci/sync-agent-rulebooks.py`,
`docs/INDEX.md` (+ generator), `docs/agents/AGENT_SKILLS_INDEX.md` (+ generator),
`docs/CURRENT_AUTHORITY.md`, `AGENTS.md`, `docs/ci/CI_GATE_MANIFEST.json`,
`.github/workflows/ci.yml`.

### Substeps

1. **Run the whole fast tier locally first** (`bash scripts/ci/verify-fast.sh`) and capture the
   complete failing list — I verified three failures by hand; the honest count comes from the
   runner, and the diff-vs-working-tree cases matter (95 uncommitted paths).
2. **Land the 95 uncommitted paths** as reviewable commits grouped by concern (audio, disease,
   memorial, docs, plans) before touching rulebooks, so a regeneration diff isn't tangled with
   feature work. One system per commit, per the project's own rule.
3. **Synchronise the rulebooks** with the provided generator (`python3
   scripts/ci/sync-agent-rulebooks.py`) and re-run `--check` to zero.
4. **Fix the `GEMINI.md` crossing**: restore it to a real AGENTS.md-derived copy (it currently
   carries Antigravity's token-budget rules) and **add it to the sync list** in
   `sync-agent-rulebooks.py` — a gate that covers 12 of 13 clients is a gate with a hole.
5. **Enumerate every client rulebook from one place**: make the script discover files from a list
   constant shared with the `ashfall-agents-sync` skill, so a new client can't be added to the repo
   and omitted from the contract again.
6. **Regenerate the docs index and skills catalog**, then re-run their two `--check` gates.
7. **Rewrite `docs/CURRENT_AUTHORITY.md`** so every row points at a file that exists, and add the
   three Wave-1/2/3 gap registers and the plan-wave index to the navigation table. Delete the rows
   for `REPO_REVIEW_REPORT.md` / `COMPREHENSIVE_GAME_AUDIT.md` or restore the files; a navigation
   map with dead destinations is worse than none.
8. **Add a "generated, never hand-edited" marker** to each generated doc (`docs/INDEX.md`,
   `docs/agents/AGENT_SKILLS_INDEX.md`, `docs/data/CATALOG_REGISTRY.md`,
   `docs/cli/HOST_CLI_COMMAND_CATALOG.md`, `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`,
   `docs/architecture/*`) in the style `HOST_CLI_COMMAND_CATALOG.md` already uses, plus a
   generator-command line — the majority of drift cases are a human editing the output.
9. **Fail the PR, not the schedule**: confirm the three failing gates are `critical=True` *and*
   actually enforced in `.github/workflows/ci.yml` — a manifest that isn't executed by the workflow
   is a wish list; if the workflow doesn't run the gate list, wire it to `run-gates.py --tier fast`.
10. **Add a drift budget**: allow-list *no* rulebook drift and require an explicit exception entry
    with owner + reason for any generated file that legitimately lags, mirroring Wave 1's 15C
    "exemptions may only shrink" ratchet.
11. **Record what "green" means**: paste the passing gate list into
    `docs/CI.md`'s status table with its date, so the next agent doesn't re-derive it.
12. **Tests**: no new tests needed; instead add the check that `--check` modes exit non-zero when
    output differs (spot-verify by mutating one generated file in a scratch copy).
13. **Run the checklist** + `verify-fast.sh` → expect all fast gates green.

**DoD:** zero red gates on `main`, and a client rulebook can no longer exist outside the sync list.

---

## Task 29B — Canon reconciliation: verify every capability claim against source

**Goal:** replace "the registry says it's live" with a claim backed by a file:line, so future
audits start from a document they can trust — including the parts of `AGENTS.md` this audit
disproved.

**Files:** `AGENTS.md` (+ all rulebook copies), `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md`,
`docs/GODOT_MIGRATION_STATUS.md`, `docs/data/DATA_GAP_AUDIT.md`,
`docs/audio/SILENCE_AUDIT.md`, `docs/ASHFALL_CODE_INDEX.md`,
`docs/ASHFALL_IMPLEMENTATION_GAP_AUDIT.md` (if present), new
`scripts/ci/verify-capability-claims.py`, `docs/ci/CI_GATE_MANIFEST.json`.

### Substeps

1. **Extract every factual claim** of the form "`file:line` — state" from the audit/registry set
   into a machine-readable list (`docs/architecture/CLAIMS.json`). This is the input to the whole
   task and the thing that makes re-verification cheap forever.
2. **Classify each claim**: `TRUE / STALE / PARTLY-TRUE / UNVERIFIABLE / FALSE`, with the file:line
   that decides it. Start from the six already disproved (H5, H11, `AirlockSecuritySystem.cs:80`,
   `questline_master.json`, `SILENCE_AUDIT` §4.7, `AGENTS.md` GameBootstrap/H7) as worked examples.
3. **Rewrite `AGENTS.md`'s workflow to the real code path**: expansion Phase 4 is
   "`src/Main.<Domain>.cs` triad + `SaveSectionRegistry` + `_campaignDay.Register` + (Wave 3)
   `SubsystemManifest`" — never `GameBootstrap`. This is the single most consequential edit in the
   plan because it is loaded into every agent session.
4. **Update H7 with generated numbers** (56 partials / 14,361 lines / 72-69-26), and add one
   sentence saying those numbers come from the generator, so nobody re-derives them by hand again.
5. **Retire or move the dead claims** into `docs/ARCHIVE_INDEX.md` with a "superseded by"
   pointer — never delete a resolved history, but never let it read as current.
6. **Encode the canon decisions that live only in code**: generate a table from
   `ContentExemption.cs` (path, classification, owner, rationale, `ExpiryCondition`) into
   `docs/data/CONTENT_POLICY.md`, so "272 catalogs are intentionally codex-only" is a stated
   project decision rather than a C# comment.
7. **Give every capability an evidence pointer**: the registry's matrix column "Runtime Confidence"
   must cite a test name or a gate id; entries that cite nothing get downgraded to `UNVERIFIED`.
   This is the doc-side twin of 15C's "presence ≠ liveness".
8. **Reconcile the three panel counts** that appear in prose (164 UI files / 135 routes / 30 live
   consoles / 30 snapshot targets) into one generated table, and fix the places that say 134 or
   52 (both appear in `docs/ui/*` history).
9. **Add `scripts/ci/verify-capability-claims.py --check`** as a gate: fails when a cited file:line
   no longer exists or a claim's status is contradicted by source. Keep it Tier-2 with a graceful
   "N claims need re-verification" summary so it can't be defeated by volume.
10. **Timestamp every audit doc** with the commit it was generated against (a
    `VersionReportContractTests`-style convention already exists for version output) — an undated
    audit is a liability, since it will eventually be read as current.
11. **Mark superseded plans explicitly**: Wave 1's index already flags `Plan_14`'s stale premise;
    add a `STATUS:` line (active / superseded by X / executed) to each plan file and have the docs
    index surface it.
12. **Fix the tone/content guards**: `AGENTS.md`'s "no real countries/wars/people" rule is enforced
    by `DataRuleComplianceTests` — confirm the test name is cited in the rulebook so the constraint
    is discoverable from the instruction layer, not only from the code.
13. **Run the checklist** + both docs gates + the new claims gate.

**DoD:** every capability claim in the docs either cites evidence or is labelled unverified, and
`AGENTS.md` describes the codebase that exists.

---

## Task 29C — The plan layer: one index, one numbering scheme, one definition of "done"

**Goal:** make the roadmap navigable enough that an agent's first act is reading current truth
instead of re-auditing it — the failure mode that produced 40+ overlapping planning documents.

**Files:** `Next-steps-plans/` (+ index), `piagentsplans/` (README + 130 files), `docs/plans/`,
`ASHFALL_UNIFIED_MASTER_EXECUTION_PLAN.md`, `docs/ARCHIVE_INDEX.md`, `docs/INDEX.md`
(generator), new `docs/roadmap/README.md`, `docs/roadmap/WAVE_LEDGER.md`,
`scripts/ci/generate-docs-index.py` (status awareness).

### Substeps

1. **Publish one numbering policy** and honour it: `<100` = continuity/hardening waves (14, 15–19,
   20–24, 25–29…), `≥100` = expansion waves (131–138), and `piagentsplans/00–129` = the historical
   evidence-backed backlog. Record it in `docs/roadmap/README.md`; retro-label nothing.
2. **Create `docs/roadmap/WAVE_LEDGER.md`**: one row per wave — id, date, author/tool, premise
   verified y/n, tasks, state (`proposed / in-flight / executed / superseded`), and the gates that
   prove completion.
3. **Give every plan file a front-matter status block** (`STATUS`, `PREMISE_VERIFIED_AT <sha>`,
   `SUPERSEDES`, `SUPERSEDED_BY`) and teach `generate-docs-index.py` to render it — so the index
   tells the truth about which plan is current without a human maintaining a list.
4. **Re-verify premises before execution** (this wave has three examples of plans written against
   stale facts: `Plan_14`'s forecast claim, the parallel `Plan_131` "zero information-flow
   capability exists" claim vs the live radio/triangulation/`WeatherIntelligenceCoordinator`/gossip
   pieces, and `DATA_GAP_AUDIT`'s orphan list). Make "premise check" step 0 of every task template
   and cite the `ashfall-analyze`/`ashfall-scan` skills as the means.
5. **Deduplicate the master plan**: `ASHFALL_UNIFIED_MASTER_EXECUTION_PLAN.md` (93 KB) against the
   wave indexes — keep the smaller, verifiable wave registers as authority and mark the monolith
   historical (or reduce it to a link page). Same treatment for `sources.md` (50 KB, already
   historical).
6. **Define "done" once per deliverable class** so claims stop inflating: a *system* is done at
   Core + save + owner + panel-live + tests; *content* at EFFECT_PRODUCED + reachability; *UI* at
   routed + bound + snapshot; *build* at export boot smoke. Put this table in
   `docs/roadmap/README.md` and link from `AGENTS.md`.
7. **Retire completed plans visibly**: move executed plans to `docs/archive/plans/` with a
   completion note and the commit that landed them — an unbounded plan folder is how the next agent
   starts work that's already shipped.
8. **Cross-link waves to gaps**: each wave index lists which numbered gaps it retires; each gap
   links its evidence. The `Wave1/2/3_Continuity_Audit_INDEX.md` trio is the template.
9. **Instrument staleness**: a gate (or a Tier-3 report) that flags any plan/audit doc citing a
   file that no longer exists or an unresolved `STATUS: in-flight` older than N days.
10. **Decide the co-authorship protocol**: two agents are writing plans into `Next-steps-plans/`
    concurrently (this wave observed 131–138 appear mid-session). Record the convention — folder
    ownership, numbering, and renumbering rules — in `docs/roadmap/README.md`, before an
    accidental id collision makes two plans claim the same number.
11. **Agent-facing entry point**: shorten the first-read path to
    `AGENTS.md → docs/CURRENT_AUTHORITY.md → docs/roadmap/README.md → wave index`, and make
    `docs/INDEX.md` the last resort rather than the first.
12. **Run the docs generator twice** to prove idempotency (`--check` after a plain run must be
    clean), then re-run every gate in the checklist.
13. **Close Wave 3** by updating `docs/CI.md`'s status line and the wave ledger states, and record
    the three waves' metrics deltas (`EFFECT_PRODUCED`, live panels, derived ending, exposure
    sources, watts, coverage %, gate count) in one table.

**DoD:** reading current truth is one hop, "done" has one definition per class, and a stale plan
says so on its first line.

---

## Cross-Task Dependencies

```
29A (green gates + rulebook repair)  ── immediate, independent ──► everything downstream
29A step 3/5 (sync) ──► 29B (AGENTS.md edits propagate to 13 copies)
29B (claims → evidence) ──► 29C (wave ledger cites them)
28A (subsystem manifest) ──► 29B step 3 (Phase-4 instruction can finally name a real seam)
27B/26B (new gates) ──► 29A step 9 (workflow must actually run the gate list)
```

**Execution order:** 29A **today** (red critical gates + the crossed rulebook are a live defect),
then 29B, and run 29C last so the index reflects verified claims. 29A step 3 must precede any
`AGENTS.md` content edit from other plans — otherwise regeneration overwrites it, or worse,
silently restores stale text into 13 files.

---

## Verification Checklist (per task)

```
1. bash scripts/ci/verify-fast.sh                                # expect: all fast gates green
2. python3 scripts/ci/sync-agent-rulebooks.py --check            # 0 drift, 13 files covered
3. python3 scripts/ci/generate-docs-index.py --check
4. python3 scripts/ci/generate-agent-skills-catalog.py --check
5. bash scripts/ci/doc-link-gate.sh
6. python3 scripts/ci/verify-capability-claims.py --check        # (29B)
7. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj      # docs work must not break tests
8. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
9. godot --headless --path . -- --data-integrity-selftest        # 0 errors
10. godot --headless --path . -- --bridge-selftest               # exits 0
```

---

## Estimated Effort & Risk

| Task | Files | New gates | Difficulty | Regression risk |
|---|---|---|---|---|
| 29A | 13 rulebooks + 2 generated docs + workflow | 0 (repair existing 3) | Low | **LOW** (docs/gates only — but regenerate, don't hand-edit) |
| 29B | ~8 audit docs + `AGENTS.md` + 1 script | 1 (claims) | Medium | LOW (never delete history; supersede it) |
| 29C | plan layer + index generator | 1 (staleness, optional) | Low–Med | LOW |

**Guardrails:** never hand-edit a generated file; never delete a resolved history (move and mark
it); no new master plan document — the wave indexes replace them; keep `AGENTS.md`'s non-negotiable
engine rules byte-identical while repairing the stale rows (the Godot-authoritative,
`dotnet`+`godot --headless`, Core-truth, no-Unity rules are the parts agents must never lose); and
do not weaken a gate to make it pass — three of them are telling the truth about an untended layer.
