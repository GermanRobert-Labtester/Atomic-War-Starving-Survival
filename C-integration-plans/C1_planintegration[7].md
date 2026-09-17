# C1 — Flagship Integration Plan [7]: One Truth — Rulebooks, Canon, Roadmap Governance & Documentation Integrity

> **Output:** `C1_planintegration[7].md`
>
> **Source baseline:** Plan 29 — One Truth: Documentation, Canon, and the Instructions Agents Actually Read
>
> **Wave:** Continuity Wave 3 — *Ship It Intact* (closing plan)
>
> **Depends on:** verified outputs of Waves 1–3. Plan 29 records their truth and prevents future agents from reintroducing already-solved defects through stale instructions.
>
> **Immediate priority:** execute 29A first. The source baseline reports three critical fast-tier documentation/rulebook gates failing and one client rulebook outside the sync contract.
>
> **Mandatory execution order:** 29A → 29B → 29C.
>
> **Primary principle:** one canonical instruction source, one evidence-backed capability registry, one roadmap index, one definition of done per deliverable class.
>
> **Guardrails:** never weaken a gate to make it pass; never hand-edit generated outputs; never delete useful historical evidence—archive and supersede it; do not create another master-plan monolith; preserve the project's engine/runtime invariants while repairing stale claims.

---

# 0. Mission

ASHFALL's technical continuity depends on more than code correctness.

Agents, developers, CI, and future planning sessions operate from a documentation layer that tells them:

- which runtime architecture is authoritative;
- which systems are live;
- where integration happens;
- which known issues remain;
- which plans are current;
- what "done" means;
- which gates prove that a claim is true.

The source baseline identifies a high-leverage failure mode: the record itself has drifted.

Examples include:
- client rulebooks no longer matching canonical instructions;
- `GEMINI.md` containing another client's rules;
- documentation instructing agents to wire into `GameBootstrap`, even though the Godot architecture no longer uses that class;
- stale counts for `Main.cs` and its partials;
- already-resolved issues still listed as open;
- stale orphan-content and audio-gap claims;
- multiple plan folders and numbering schemes with no single status ledger;
- generated indices out of sync;
- canon policy existing only in C# exemption objects rather than discoverable documentation.

This is not clerical cleanup.

A stale instruction layer is a defect generator.

The final governance model must be:

```text
                         ┌──────────────────────┐
                         │      AGENTS.md       │
                         │ canonical rulebook   │
                         └──────────┬───────────┘
                                    │
                     sync generator │
                                    ▼
        ┌─────────────────────────────────────────────┐
        │ CLIENT RULEBOOK COPIES — BYTE/SECTION SYNC │
        └─────────────────────────────────────────────┘
                                    │
                                    ▼
                    docs/CURRENT_AUTHORITY.md
                                    │
                                    ▼
                    docs/roadmap/README.md
                                    │
                                    ▼
                    docs/roadmap/WAVE_LEDGER.md
                                    │
                                    ▼
                     current wave / active plan
```

And capability truth must flow as:

```text
SOURCE / TEST / GATE
        │
        ▼
docs/architecture/CLAIMS.json
        │
        ▼
capability claim status
TRUE / PARTIAL / STALE / UNVERIFIED / FALSE
        │
        ▼
canon registry / audits / rulebooks
```

Generated documentation must flow as:

```text
SOURCE DATA / MANIFEST
        │
        ▼
GENERATOR
        │
        ▼
GENERATED DOC
        │
        ▼
--check gate
        │
        ▼
CI
```

No generated output should be maintained as a parallel handwritten authority.

---

# 1. Source-Evidence Interpretation

## 1.1 Three critical fast-tier gates are reported red

The source plan reports failures in:
- agent rulebook sync;
- docs index drift;
- agent skills catalog drift.

Therefore 29A is not optional cleanup; it is current CI repair.

## 1.2 `GEMINI.md` is outside the existing sync contract

A sync gate that covers only 12 of 13 client rulebooks is structurally incomplete.

29A must:
- repair `GEMINI.md`;
- add it to the canonical client list;
- ensure future clients cannot appear outside the contract.

## 1.3 The canonical rulebook contains stale architecture instructions

The strongest example is the instruction to wire expansions into a nonexistent `GameBootstrap`.

If every agent session reads that instruction, the documentation actively generates wrong code.

29B therefore treats `AGENTS.md` as executable architecture policy.

## 1.4 Capability claims lack machine-verifiable evidence

A registry saying "implemented" is not sufficient.

A capability claim must point to:
- source;
- test;
- runtime evidence;
- CI gate;
or explicitly remain `UNVERIFIED`.

## 1.5 Planning information is fragmented

Multiple folders, numbering schemes, and a large unified master plan create ambiguity over:
- which plan is current;
- which was superseded;
- which was executed;
- which premise was stale.

29C converts plans into indexed state rather than prose archaeology.

---

# 2. Non-Negotiable Documentation Invariants

## INV-29.1 — `AGENTS.md` is the canonical agent rulebook

Client-specific rulebooks may add explicitly allowed client overlays only if the sync system supports that model.

They may not silently diverge from canonical architecture rules.

## INV-29.2 — Every client rulebook is inside the sync contract

A client instruction file cannot exist in the repository without being:
- listed/discovered by the rulebook sync system;
- checked in CI;
- assigned a sync policy.

## INV-29.3 — Generated docs are never hand-edited

Generated documents contain:
- explicit generated marker;
- generator command;
- source authority.

Any human edit to generated output without source/generator change should fail `--check`.

## INV-29.4 — Current architecture claims require evidence

Every claim in the current canon/audit layer must be:
- evidenced;
- marked partial;
- marked stale;
- marked unverifiable;
- or marked false.

No unsupported "LIVE" language.

## INV-29.5 — Dead history is archived, not erased

Resolved/stale documents may be moved to archive with:
- superseded-by pointer;
- last verified commit;
- historical status.

Do not let archived claims read as current.

## INV-29.6 — Roadmap status is machine-visible

Every active plan has a machine-readable status block.

The docs index should surface it.

## INV-29.7 — Plan numbering has one published policy

New plans follow one documented scheme.

Historical plans are not renumbered.

## INV-29.8 — "Done" is defined per deliverable class

A system, UI, content package, build, and plan are not complete under the same proof.

Completion definitions belong in one canonical roadmap document.

## INV-29.9 — Gate manifests must correspond to executed workflow

A gate listed as critical must actually run in CI.

Manifest-only gates are not enforcement.

## INV-29.10 — Staleness is observable

Docs/plans that:
- cite missing files;
- remain in-flight too long;
- refer to obsolete architecture;
should be surfaced automatically.

---

# 3. Definition of Done

Plan 29 closes only when all are true:

- `verify-fast.sh` reports all fast-tier gates green;
- all agent rulebooks are synchronized;
- `GEMINI.md` is correct and inside the sync contract;
- client rulebook discovery/list ownership is centralized;
- docs index and skills index regenerate cleanly;
- generated docs contain a "generated, do not hand-edit" marker and generator command;
- critical gate manifest entries are actually executed by workflow;
- `docs/CURRENT_AUTHORITY.md` contains only valid destinations;
- `AGENTS.md` describes the actual Godot composition path;
- stale `GameBootstrap` instructions are removed from current docs;
- stale H5/H7/H11-style claims are reconciled against current source;
- canon capability claims are extracted to a machine-readable claims registry;
- every current claim has evidence or `UNVERIFIED` status;
- code-only content exemption policy is surfaced in documentation;
- panel/count metrics are generated from source rather than prose;
- plan files expose status/premise/supersession metadata;
- one roadmap README defines numbering and completion policy;
- one wave ledger indexes active/historical waves;
- executed/superseded plans are clearly marked or archived;
- stale plan/audit references can be detected automatically;
- agent first-read path is short and explicit;
- Wave 3 closes with current metrics and green CI.

---

# 4. Phase P0 — Freeze Current Documentation/Gate State

## P0.1 Run the actual fast tier

Before modifying documentation:

```bash
bash scripts/ci/verify-fast.sh
```

Capture:
- gate count;
- pass count;
- fail count;
- exact failing gate IDs;
- command;
- exit code;
- relevant diff/output.

Do not assume only the three source-plan failures remain.

---

## P0.2 Record working-tree state

Capture:

```text
git rev-parse HEAD
git branch --show-current
git status --porcelain
dirty path count
```

If the tree remains large:
- group prior feature work;
- land or isolate reviewable commits before generated-document regeneration.

Do not mix unrelated feature changes with rulebook sync output.

---

## P0.3 Inventory rulebooks

Create table:

```text
file
client
exists
expected canonical source
sync mode
currently in sync?
special overlay?
included in sync script?
```

Expected candidates from source:
- `AGENTS.md`
- `CLAUDE.md`
- `CODEX.md`
- `CRUSH.md`
- `GOOSE.md`
- `QWEN.md`
- `VIBE.md`
- `MIMOCODE.md`
- `OPENSETUP.md`
- `ANTIGRAVITY.md`
- `GEMINI.md`
- `.clinerules`
- `.cursorrules`
- `.windsurfrules`

Recheck actual current repository.

---

## P0.4 Inventory generated docs

Search for:
- generator script;
- generated output;
- existing generated marker;
- `--check` support;
- CI gate.

Create table:

```text
generated_doc
generator
source_inputs
marker_present
check_gate
critical?
workflow_executed?
```

Minimum source-plan set:
- `docs/INDEX.md`;
- `docs/agents/AGENT_SKILLS_INDEX.md`;
- `docs/data/CATALOG_REGISTRY.md`;
- `docs/cli/HOST_CLI_COMMAND_CATALOG.md`;
- `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`;
- generated architecture docs.

---

## P0.5 Inventory current documentation layers

Classify:

### Current authority
- `AGENTS.md`
- `docs/CURRENT_AUTHORITY.md`
- current architecture docs
- canon registry

### Generated indices
- `docs/INDEX.md`
- skills index
- catalogs

### Audits
- data gap
- audio silence
- implementation gap
- continuity audits

### Roadmap
- `Next-steps-plans/`
- `piagentsplans/`
- `docs/plans/`
- root unified master plan
- historical sources

### Archive
- existing archive indices/folders

---

# TASK 29A — Make the Record Green and Keep It Green

# 29A.0 Goal

Clear active documentation/rulebook drift and make future drift fail immediately.

---

## 29A.1 Land or isolate unrelated dirty work

If working tree contains broad feature modifications:
- separate by concern;
- commit coherent completed work;
- do not regenerate docs over unstable feature state.

Suggested grouping:
- audio;
- disease;
- memorial;
- docs;
- plans;
- generated artifacts.

The exact groups follow actual tree.

---

## 29A.2 Re-run rulebook sync in write mode

Run:

```bash
python3 scripts/ci/sync-agent-rulebooks.py
```

Then:

```bash
python3 scripts/ci/sync-agent-rulebooks.py --check
```

Expected:
- zero drift for every managed client file.

---

## 29A.3 Repair `GEMINI.md`

Inspect current content.

Requirements:
- restore canonical AGENTS-derived rule content;
- remove accidental Antigravity-only rules unless they belong in a documented Gemini overlay;
- include correct client heading only if sync policy supports headings.

Do not simply copy another client's file blindly.

---

## 29A.4 Add `GEMINI.md` to sync contract

Modify sync system so Gemini is governed.

Acceptance:
- mutate Gemini copy in scratch test;
- `--check` fails;
- regeneration restores it.

---

## 29A.5 Centralize client rulebook list

Create one authoritative list/constant.

Possible:

```python
CLIENT_RULEBOOKS = [
    ...
]
```

The same list should be reusable by:
- `sync-agent-rulebooks.py`;
- agent sync skill/tooling;
- tests.

Avoid duplicate client lists.

---

## 29A.6 Future-client omission gate

Add test:

```text
repository contains recognized rulebook-like client file
AND file not listed in sync contract
→ fail
```

Recognition may use:
- allowlisted filename patterns;
- explicit agent-client registry.

Avoid matching arbitrary markdown files.

---

## 29A.7 Regenerate docs index

Run generator normally.

Then:

```bash
python3 scripts/ci/generate-docs-index.py --check
```

Expected clean.

---

## 29A.8 Regenerate agent skills index

Run generator normally.

Then:

```bash
python3 scripts/ci/generate-agent-skills-catalog.py --check
```

Expected clean.

---

## 29A.9 Repair `CURRENT_AUTHORITY`

Every row must resolve to a current file.

For missing historical documents:
- remove row from current navigation;
- or restore only if the file genuinely remains current.

Do not keep dead links for nostalgia.

---

## 29A.10 Add continuity-wave registers

`CURRENT_AUTHORITY` should link:
- Wave 1 gap/audit register;
- Wave 2 gap/audit register;
- Wave 3 gap/audit register;
- roadmap wave ledger;
- current plan index.

Use actual current filenames.

---

## 29A.11 Add generated markers

Each generated doc should start with a stable header, e.g.:

```markdown
<!-- GENERATED FILE — DO NOT EDIT DIRECTLY -->
<!-- Source: ... -->
<!-- Regenerate: python3 scripts/... -->
```

or repository house style.

The marker should include:
- source;
- command;
- whether CI checks drift.

---

## 29A.12 Generator idempotence

For every generator touched:

```text
run
hash outputs
run again
hash outputs
assert unchanged
```

Then run `--check`.

No timestamp-only churn in generated output unless intentionally normalized.

---

## 29A.13 Verify workflow executes critical gates

Read:
- `docs/ci/CI_GATE_MANIFEST.json`;
- `.github/workflows/ci.yml`;
- gate runner scripts.

For every `critical=True` gate:
- identify workflow job;
- identify invocation path.

If manifest is not authoritative:
- wire CI to `run-gates.py --tier fast` or current canonical runner.

---

## 29A.14 Gate manifest/workflow consistency test

Add test:

```text
every critical fast-tier manifest gate
→ reachable from CI workflow
```

Avoid manual duplication.

---

## 29A.15 No rulebook drift budget

Policy:
- zero rulebook drift.

If a client legitimately needs a difference:
- use explicit overlay mechanism;
- owner;
- reason;
- bounded scope;
- test.

Do not create a freeform exception that permits entire-file drift.

---

## 29A.16 Generated-doc drift exceptions

If any generated output legitimately lags:
- explicit exception file/registry;
- owner;
- reason;
- expiry condition.

Target:
- exceptions shrink.

---

## 29A.17 Record green status in CI docs

Update `docs/CI.md` status table with:
- date;
- commit;
- fast gate count;
- pass count;
- critical count.

Use generated data where possible.

---

## 29A.18 Test `--check` can fail

For each repaired gate family:
- mutate scratch copy/output;
- run `--check`;
- assert non-zero.

Do not modify tracked production file during test.

---

## 29A.19 Full fast-tier rerun

Run:

```bash
bash scripts/ci/verify-fast.sh
```

Expected:
- all fast gates green.

### 29A DoD

Zero red fast-tier gates and no client rulebook can exist outside the synchronization contract.

---

# TASK 29B — Canon Reconciliation and Evidence-Backed Claims

# 29B.0 Goal

Make current documentation trustworthy enough that a future audit can begin from it instead of re-verifying the entire repository.

---

## 29B.1 Define capability claim schema

Create:

`docs/architecture/CLAIMS.json`

Suggested record:

```json
{
  "claim_id": "agent_h7_main_shape",
  "document": "AGENTS.md",
  "anchor": "H7",
  "claim": "...",
  "status": "TRUE",
  "evidence": [
    {
      "type": "source",
      "path": "src/Main.cs",
      "symbol": null
    }
  ],
  "tests": [],
  "gates": [],
  "verified_at_commit": "...",
  "notes": ""
}
```

---

## 29B.2 Supported statuses

Use exactly:

- `TRUE`
- `PARTLY_TRUE`
- `STALE`
- `UNVERIFIABLE`
- `FALSE`
- `SUPERSEDED`

Avoid proliferating status vocabulary.

---

## 29B.3 Extract claims

Initial documents:
- `AGENTS.md`;
- canon registry;
- Godot migration status;
- data gap audit;
- audio silence audit;
- code index;
- implementation gap audit if present.

Focus first on objective claims:
- file exists;
- symbol exists;
- route exists;
- system wired;
- test exists;
- line/count metrics;
- architecture owner.

---

## 29B.4 Start with already disproved claims

Reverify:
- Utility AI fork;
- JournalSystem test claim;
- Airlock `GetHashCode`;
- `questline_master.json` orphan claim;
- no death event claim;
- `GameBootstrap`;
- `Main.cs` size/count claim.

Do not trust source-plan values blindly; recompute current truth.

---

## 29B.5 Rewrite AGENTS Phase 4

Replace nonexistent Unity-era wiring instructions with actual Godot composition path.

Canonical pattern should mention current:
- `src/Main.<Domain>.cs` composition;
- Setup/Save/Flush triad where applicable;
- `SaveSectionRegistry`;
- `_campaignDay.Register`;
- `SubsystemManifest` if Plan 28 has landed.

Do not prescribe symbols absent from current source.

---

## 29B.6 Preserve engine invariants

While rewriting rulebooks, preserve byte-equivalent or semantically identical non-negotiables:

- Godot authoritative;
- Core engine-free;
- `dotnet` + `godot --headless` validation;
- no Unity reintroduction;
- current data-authority rules.

Add tests/diff review for these sections.

---

## 29B.7 Generate Main architecture metrics

Add script/helper that computes:
- `src/Main.cs` line count;
- `src/Main*.cs` file count;
- total lines;
- Setup method count;
- Save method count;
- Flush method count.

Use generated values in docs.

Do not hand-maintain numbers.

---

## 29B.8 Replace H7 prose with generated metrics

H7 should:
- cite generated report;
- avoid stale inline numbers where possible;
- describe architecture conceptually.

If numbers remain:
- auto-insert from generator.

---

## 29B.9 Archive resolved claims

When claim becomes stale/resolved:
- mark status;
- move historical explanation to archive if verbose;
- add `superseded_by`;
- keep current docs concise.

Do not erase useful history.

---

## 29B.10 Generate content policy doc from code authority

Create:

`docs/data/CONTENT_POLICY.md`

Source:
- `ContentExemption.cs`;
- exemption registry;
- `ExpiryCondition`;
- ticket/owner/rationale.

Generated columns:

```text
path/category
classification
owner
rationale
expiry_condition
ticket
```

This makes codex-only/deferred policy discoverable.

---

## 29B.11 Content policy drift gate

If content exemption code changes:
- generated policy must update;
- `--check` fails otherwise.

---

## 29B.12 Capability evidence requirements

Every current capability row in canon registry gets:

```text
Runtime Confidence
Evidence
```

Evidence can be:
- test name;
- gate ID;
- journey name;
- runtime evidence artifact;
- source + direct integration proof.

If none:
- `UNVERIFIED`.

---

## 29B.13 Confidence levels

Suggested:

```text
PROVEN_RUNTIME
PROVEN_INTEGRATION
PROVEN_UNIT
SOURCE_ONLY
UNVERIFIED
```

Do not equate source presence with runtime proof.

---

## 29B.14 Panel count reconciliation

Generate one table:

```text
UI source files
registered descriptors
player-navigable live routes
prototype/shelved routes
snapshot-covered live routes
```

Source each from actual registry/files/snapshot manifest.

Use this table in docs.

---

## 29B.15 Remove conflicting panel counts from prose

Replace historical counts with:
- generated table link;
- or explicitly historical dated note.

---

## 29B.16 Capability claim verifier

Create:

`scripts/ci/verify-capability-claims.py`

Capabilities:
- parse `CLAIMS.json`;
- verify paths exist;
- verify optional symbols/patterns;
- detect stale line anchors where used;
- validate required evidence fields;
- produce summary.

---

## 29B.17 Avoid brittle exact lines

Prefer:
- symbol;
- path;
- regex signature;
- test/gate ID.

Exact line numbers may remain in historical evidence but should not be sole verification key.

---

## 29B.18 Tier-2 claim gate

Add to CI manifest as Tier 2.

Output:

```text
claims verified
claims stale
claims unverified
claims false
claims requiring manual review
```

Fail when:
- current TRUE claim evidence no longer resolves;
- required current docs contain stale claim;
- status missing.

---

## 29B.19 Manual-reverification path

Some claims cannot be fully machine-verified.

Allow:
- `UNVERIFIABLE` or manual-review status;
- reviewer;
- verification commit/date.

Do not pretend heuristic script proves semantics it cannot.

---

## 29B.20 Timestamp audit docs

Add header:

```text
Verified against: <commit>
Verified at: <date>
Status: current / historical / superseded
```

Prefer generator where possible.

---

## 29B.21 Plan status headers

Each plan gets front matter:

```text
STATUS: proposed | in-flight | executed | superseded | historical
PREMISE_VERIFIED_AT: <sha>
SUPERSEDES:
SUPERSEDED_BY:
EXECUTED_AT:
```

29C formalizes this across plan layer.

---

## 29B.22 Rule enforcement discoverability

For important rules in `AGENTS.md`, cite enforcing test/gate.

Example:
- no real countries/wars/people;
- test/gate name.

This makes policy auditable.

---

## 29B.23 Reconcile stale audits comprehensively

Do not only fix source-plan examples.

For each current audit:
- compare claims registry;
- update statuses;
- archive superseded sections.

---

## 29B.24 Claims gate selftest

Use scratch claim:
- missing file;
- wrong symbol;
- stale TRUE claim.

Assert verifier fails with clear reason.

---

## 29B.25 29B acceptance metrics

Record:

```text
claims total
TRUE
PARTLY_TRUE
STALE
UNVERIFIABLE
FALSE
SUPERSEDED
capabilities with runtime evidence
capabilities source-only
dead current-doc references
conflicting generated counts
```

### 29B DoD

Every current capability claim either points to evidence or clearly declares that it is not yet verified.

---

# TASK 29C — One Roadmap Index, One Numbering Policy, One Definition of Done

# 29C.0 Goal

Make a new agent's first action reading current truth, not reconstructing the roadmap from dozens of overlapping plan files.

---

## 29C.1 Publish numbering policy

Create:

`docs/roadmap/README.md`

Policy from source:

```text
<100
  continuity / hardening waves
  e.g. 14, 15–19, 20–24, 25–29

>=100
  expansion waves
  e.g. 131–138

piagentsplans/00–129
  historical evidence-backed backlog
```

Do not renumber historical files.

---

## 29C.2 Define plan ID collision rules

When two agents plan concurrently:
- reserve ID/range;
- record author/tool;
- no silent renumber after publication;
- if collision occurs, keep one canonical ID and mark the other superseded/alias.

Document exact procedure.

---

## 29C.3 Create wave ledger

Create:

`docs/roadmap/WAVE_LEDGER.md`

Columns:

```text
wave_id
title
date
author/tool
premise_verified
premise_commit
tasks
status
supersedes
superseded_by
completion_gates
execution_commit
notes
```

---

## 29C.4 Status vocabulary

Use:

- `proposed`
- `in-flight`
- `executed`
- `superseded`
- `historical`

Avoid extra synonyms like "done", "complete-ish", "paused" unless represented as notes.

---

## 29C.5 Add front-matter status to plan files

For each current plan:

```text
STATUS:
PREMISE_VERIFIED_AT:
SUPERSEDES:
SUPERSEDED_BY:
EXECUTED_AT:
WAVE:
```

Generator parses this.

---

## 29C.6 Premise verification step 0

Every future plan template begins:

```text
Step 0 — Re-verify premise against current source.
```

Required outputs:
- commit SHA;
- current evidence;
- stale assumptions found.

If premise false:
- mark plan superseded or rewrite before execution.

---

## 29C.7 Link analysis skills

Roadmap README should tell agents to use current:
- analysis/scan skill;
- repository audit skill;
- equivalent current tooling.

Do not cite obsolete skill names if they do not exist.

---

## 29C.8 Deduplicate unified master plan

Compare root monolith against wave ledgers.

Preferred outcome:
- mark master plan historical;
- reduce to navigation/summary;
- do not maintain duplicate detailed task authority.

---

## 29C.9 Historical `sources.md`

If already historical:
- move/archive or clearly label;
- link current source registry;
- avoid current docs treating it as authority.

---

## 29C.10 Define "done" once

In roadmap README add:

### System done
Must have:
- Core implementation;
- campaign owner/composition;
- persistence if stateful;
- day-loop registration if time-based;
- player-live surface if player-facing;
- tests;
- runtime evidence where applicable.

### Content done
Must have:
- loaded/registered;
- reachable;
- selected by real runtime;
- `EFFECT_PRODUCED`;
- data integrity.

### UI done
Must have:
- live route;
- campaign authority binding;
- action/read behavior;
- keyboard/accessibility;
- snapshot for live route.

### Build done
Must have:
- export succeeds;
- artifact boots;
- packaged data resolves;
- golden save load smoke.

### Plan done
Must have:
- acceptance checklist;
- required gates green;
- status set `executed`;
- execution commit;
- supersession/update of roadmap.

---

## 29C.11 Link done definitions from AGENTS

Do not duplicate full table into all rulebooks.

Rulebook says:
- "Completion definitions: docs/roadmap/README.md".

Sync propagates link.

---

## 29C.12 Archive executed plans

Move executed plans to:
`docs/archive/plans/`
or current archive convention.

Include:
- completion note;
- execution commit;
- original plan ID;
- superseded-by if applicable.

If physical move would break too many links:
- first support redirect/index aliases;
- then migrate.

---

## 29C.13 Preserve discoverability after archive

Docs index must list:
- current plans;
- executed archive;
- superseded history.

A move must not make old links silently disappear.

---

## 29C.14 Cross-link waves to gaps

Wave ledger entry lists:
- gap IDs closed;
- evidence register;
- acceptance gates.

Gap audit lists:
- plan/task closing it;
- status.

---

## 29C.15 Staleness scanner

Add script/report:

```text
plan/audit references missing file
status in-flight older than threshold
premise verification commit unavailable
superseded plan missing pointer
executed plan still in active folder
```

Tier:
- report first;
- gate for hard failures;
- age threshold warning may remain Tier 3.

---

## 29C.16 Missing-file reference gate

Hard fail for current docs referencing nonexistent current files unless:
- historical block;
- archive link;
- explicit external reference.

---

## 29C.17 In-flight age policy

Set configurable threshold.

Example:
- 30/60/90 days according to project cadence.

Old in-flight plan:
- warning;
- requires reaffirm/reverify;
- not automatically marked stale.

---

## 29C.18 Co-authorship protocol

Document:
- folder ownership;
- plan ID reservation;
- branch/lane expectation;
- merge resolution;
- supersession.

No two active plans should claim same ID.

---

## 29C.19 First-read path

Canonical onboarding:

```text
AGENTS.md
   ↓
docs/CURRENT_AUTHORITY.md
   ↓
docs/roadmap/README.md
   ↓
docs/roadmap/WAVE_LEDGER.md
   ↓
current wave index
```

`docs/INDEX.md` becomes broad catalog, not first architecture authority.

---

## 29C.20 Docs index status awareness

Update generator to surface:
- status;
- wave;
- premise commit;
- superseded marker.

Current plans appear before historical.

---

## 29C.21 Docs index categories

Recommended:
- Current Authority
- Active Roadmap
- Current Architecture
- Generated Catalogs
- Audits
- Historical/Archive

Avoid flat 100+ file wall.

---

## 29C.22 Generator idempotence

Run:

```text
generate
--check
generate again
hash same
```

For docs index and roadmap tables.

---

## 29C.23 Wave 3 closure

Update:
- Wave 3 status to executed;
- task statuses;
- CI status;
- current authority;
- archive pointers.

Only after required implementation lands.

---

## 29C.24 Cross-wave metric table

Create one table:

```text
metric
pre-Wave1
post-Wave1
post-Wave2
post-Wave3
```

Candidate metrics:
- `EFFECT_PRODUCED`;
- live panel count;
- derived ending;
- exposure sources;
- watts/power consumers;
- protected coverage slices;
- gate count;
- red fast gates;
- stale capability claims.

Use current measured values.

---

## 29C.25 29C acceptance metrics

Record:

```text
active plan folders
plan files with status metadata
plans without premise SHA
plans marked in-flight beyond threshold
duplicate plan IDs
executed plans still active
dead current-doc references
roadmap hops from AGENTS to active plan
```

### 29C DoD

Current truth is reachable in one short navigation path, plan status is visible on first read, and completion criteria are defined once.

---

# 5. Cross-Task Dependency Graph

```text
          29A — GREEN RECORD
           │
           ├── rulebook sync
           ├── generated docs
           └── CI execution
           │
           ▼
       29B — VERIFIED CANON
           │
           ├── AGENTS truth
           ├── capability claims
           ├── evidence registry
           └── content policy
           │
           ▼
       29C — ROADMAP TRUTH
           │
           ├── numbering
           ├── statuses
           ├── wave ledger
           ├── done definitions
           └── staleness
```

Dependencies from other plans:

```text
28A SubsystemManifest ─────► 29B actual Phase-4 instructions
27B / 26B gates ───────────► 29A workflow execution check
15C liveness concepts ─────► 29B evidence requirements
Waves 1–3 outputs ─────────► 29C closure metrics
```

---

# 6. Documentation Authority Hierarchy

Publish and enforce:

```text
1. Executable source + tests + runtime gates
2. AGENTS.md engine/architecture rules
3. docs/CURRENT_AUTHORITY.md
4. generated architecture/catalog docs
5. docs/roadmap/WAVE_LEDGER.md
6. current audits
7. active plans
8. historical plans/audits
```

A lower layer cannot override a higher layer.

If plan contradicts source:
- source wins;
- plan premise must be reverified.

---

# 7. Generated-Document Contract

Every generated doc must specify:

```text
GENERATED
source authority
generator command
check command
last generated commit if useful
```

Generator tests:
- deterministic output;
- sorted stable order;
- no machine-specific path;
- no timestamp churn unless normalized.

---

# 8. Rulebook Sync Contract

Recommended architecture:

```text
AGENTS.md
  │
  ├── canonical shared section
  │
  └── optional client overlay registry
           │
           ▼
sync-agent-rulebooks.py
           │
           ▼
client outputs
```

If overlays are not needed:
- exact canonical copies are simpler.

Avoid manually maintained per-client architecture instructions.

---

# 9. Capability Claim Evidence Model

A capability may be:

### Source-only
Class exists.

### Unit-proven
Behavior tested in isolation.

### Integration-proven
Host/campaign wiring tested.

### Runtime-proven
Real journey/selftest observed effect.

### Ship-proven
Exported artifact boot/runtime proves it.

Canon registry should expose this confidence.

---

# 10. Claim Failure Injection

## N29.1 Claim cites deleted file
Expected: claims gate fails.

## N29.2 TRUE claim loses test/gate evidence
Expected: downgraded or gate failure.

## N29.3 `GEMINI.md` drifts
Expected: sync check fails.

## N29.4 New client rulebook added outside registry
Expected: omission gate fails.

## N29.5 Generated docs hand-edited
Expected: generator `--check` fails.

## N29.6 Critical manifest gate removed from CI workflow
Expected: manifest/workflow consistency test fails.

## N29.7 Active plan has no status
Expected: roadmap/index gate fails.

## N29.8 Two active plans share same ID
Expected: roadmap gate fails.

## N29.9 Current doc cites nonexistent current file
Expected: doc/reference gate fails.

## N29.10 Superseded plan lacks pointer
Expected: roadmap lint warning/failure.

---

# 11. CI Tiering

## Fast
- rulebook sync;
- docs index drift;
- skills catalog drift;
- doc links;
- generated-doc checks;
- roadmap syntax/status checks.

## Tier 2
- capability claims verification;
- broader current-doc source resolution;
- evidence pointer validation.

## Tier 3/report
- age/staleness warnings;
- long-running audit recertification.

Do not overload fast tier with expensive semantic scanning.

---

# 12. Documentation Tests

Add tests for:

- generator idempotence;
- `--check` non-zero on drift;
- all current paths resolve;
- all client rulebooks registered;
- current plan statuses valid;
- no duplicate active plan IDs;
- claims schema valid;
- current TRUE claims have evidence.

Use current repository test/tool conventions.

---

# 13. Plan Status Front Matter

Recommended block:

```markdown
> **STATUS:** in-flight
> **WAVE:** 3
> **PREMISE_VERIFIED_AT:** ccac926e
> **SUPERSEDES:** —
> **SUPERSEDED_BY:** —
> **EXECUTED_AT:** —
```

Generator must parse predictable syntax.

Do not rely on prose words buried in body.

---

# 14. Plan Premise Reverification Protocol

Before execution:

1. read current commit;
2. rerun the evidence commands;
3. mark each premise:
   - confirmed;
   - changed;
   - false;
4. update plan status;
5. execute only confirmed task scope.

If >30% of premises changed:
- rewrite/supersede plan rather than patching endlessly.

---

# 15. Archive Policy

Archive contains:
- executed plans;
- superseded audits;
- historical master plans;
- obsolete architecture guides.

Each archived file should include:

```text
HISTORICAL
superseded_by
last_current_at
reason archived
```

Current docs should not link historical files without historical label.

---

# 16. Roadmap Navigation Policy

The roadmap README should answer in <1 minute:

- What wave are we in?
- What is active?
- What is executed?
- What is blocked?
- What comes next?
- Which gates prove completion?
- Which numbering range do I use?

If it cannot, roadmap is too complex.

---

# 17. "Done" Gate Matrix

| Deliverable | Required proof |
|---|---|
| Core system | build + unit behavior + owner + save if stateful |
| Runtime subsystem | Core + composition + day owner + integration |
| UI | live route + campaign bind + interaction + accessibility + snapshot |
| Content | loaded + registered + selected + effect produced |
| Save | round-trip + migration + checksum/integrity |
| Build | export + boot + data resolution + load smoke |
| Plan | acceptance checklist + gates + executed status + commit |
| Audit | verified commit + claim statuses + source evidence |

No plan may invent its own weaker definition.

---

# 18. Metrics to Publish at Wave Close

Minimum:

```text
fast gates passed / total
rulebooks synced / total
current-doc dead references
claims verified / total
claims runtime-proven
stale claims
active plans
executed plans
duplicate plan IDs
plan premise verification coverage
live panels
EFFECT_PRODUCED
coverage protected-slice deltas
export smoke status
```

---

# 19. Recommended Commit Breakdown

```text
29A-1 capture fast-tier failures + working-tree cleanup
29A-2 rulebook regeneration
29A-3 GEMINI repair + 13-client sync registry
29A-4 docs index / skills index regeneration
29A-5 generated markers + generator idempotence
29A-6 workflow/manifest consistency + green status

29B-1 claims schema + extraction
29B-2 AGENTS architecture repair
29B-3 generated Main architecture metrics
29B-4 audit/canon reconciliation
29B-5 content policy generation
29B-6 capability confidence/evidence
29B-7 claims verifier + Tier-2 gate

29C-1 roadmap README + numbering policy
29C-2 wave ledger
29C-3 plan front-matter + index generator support
29C-4 done-definition table
29C-5 archive/supersession pass
29C-6 staleness/duplicate-plan gate
29C-7 Wave-3 closure metrics + final docs regeneration
```

---

# 20. Risk Register

## R29.1 Generated-doc hand-edit churn

Mitigation:
- markers;
- source-first changes;
- `--check`.

## R29.2 Rulebook sync destroys valid client-specific instructions

Mitigation:
- explicit overlay model if required;
- diff review;
- canonical sections immutable.

## R29.3 Claims verifier becomes brittle

Mitigation:
- symbols over exact lines;
- manual-review status;
- Tier 2.

## R29.4 Archiving breaks links

Mitigation:
- index redirects;
- link gate;
- move in controlled pass.

## R29.5 Roadmap migration causes ID confusion

Mitigation:
- no historical renumbering;
- explicit aliases/supersession.

## R29.6 Staleness gate becomes noisy

Mitigation:
- hard fail only missing current files/invalid statuses;
- age warnings Tier 3 initially.

## R29.7 AGENTS edit accidentally loses engine invariants

Mitigation:
- invariant section diff test;
- rulebook sync after canonical edit.

## R29.8 Working-tree regeneration mixes unrelated work

Mitigation:
- clean/group commits before regeneration.

---

# 21. Verification Commands

Run after each task and at closure:

```bash
bash scripts/ci/verify-fast.sh
python3 scripts/ci/sync-agent-rulebooks.py --check
python3 scripts/ci/generate-docs-index.py --check
python3 scripts/ci/generate-agent-skills-catalog.py --check
bash scripts/ci/doc-link-gate.sh
python3 scripts/ci/verify-capability-claims.py --check
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

Additional roadmap checks when implemented:

```bash
python3 scripts/ci/verify-roadmap-status.py --check
python3 scripts/ci/verify-current-doc-references.py --check
```

Use actual final script names.

---

# 22. Final Acceptance Checklist

## 29A — Green record

- [ ] full fast-tier baseline captured
- [ ] unrelated dirty work grouped/isolated
- [ ] canonical rulebook regeneration run
- [ ] 12 previous drifted clients clean
- [ ] GEMINI.md corrected
- [ ] GEMINI.md added to sync contract
- [ ] client list centralized
- [ ] omitted-client gate added
- [ ] docs index regenerated
- [ ] skills catalog regenerated
- [ ] CURRENT_AUTHORITY dead links removed
- [ ] continuity wave registers added
- [ ] generated-doc markers added
- [ ] generator command shown in generated docs
- [ ] generators idempotent
- [ ] critical manifest gates map to workflow
- [ ] manifest/workflow consistency test added
- [ ] rulebook drift budget zero
- [ ] generated drift exceptions explicit/expiring
- [ ] docs/CI green status updated
- [ ] `--check` failure behavior proven
- [ ] verify-fast all green

## 29B — Canon

- [ ] CLAIMS.json schema established
- [ ] claim statuses standardized
- [ ] current claim set extracted
- [ ] known stale claims reverified
- [ ] AGENTS Phase 4 rewritten to actual Godot composition path
- [ ] engine invariants preserved
- [ ] Main architecture metrics generated
- [ ] H7 corrected from generated source
- [ ] resolved claims archived/superseded
- [ ] CONTENT_POLICY generated from exemption authority
- [ ] content-policy drift gate exists
- [ ] canon capability rows include evidence
- [ ] confidence levels normalized
- [ ] panel counts generated
- [ ] conflicting prose counts reconciled
- [ ] verify-capability-claims.py implemented
- [ ] verifier prefers symbols over brittle lines
- [ ] Tier-2 claim gate registered
- [ ] manual verification status supported
- [ ] audit docs carry verification commit/date
- [ ] plan status headers prepared
- [ ] rulebook constraints cite enforcing tests/gates
- [ ] full stale-audit reconciliation done
- [ ] claims gate failure test exists
- [ ] metrics recorded

## 29C — Roadmap

- [ ] numbering policy published
- [ ] plan collision protocol published
- [ ] WAVE_LEDGER created
- [ ] status vocabulary fixed
- [ ] plan front matter added
- [ ] premise-verification step 0 standardized
- [ ] analysis/scan tooling linked
- [ ] unified master plan reduced/archived
- [ ] historical sources marked
- [ ] one done-definition table created
- [ ] AGENTS links to done definitions
- [ ] executed plans archived or clearly marked
- [ ] archive remains discoverable
- [ ] waves cross-link to gap registers
- [ ] staleness scanner added
- [ ] current missing-file references hard-fail
- [ ] in-flight age policy defined
- [ ] co-authorship protocol defined
- [ ] first-read path documented
- [ ] docs index surfaces plan status
- [ ] docs index categories improved
- [ ] generators idempotent
- [ ] Wave-3 closure status updated
- [ ] cross-wave metrics table recorded
- [ ] roadmap metrics recorded

---

# 23. Ship / No-Ship Gate

**SHIP** only if:

```text
fast_critical_gates_red == 0
AND client_rulebooks_synced == 100_percent
AND unregistered_client_rulebooks == 0
AND generated_doc_drift == 0
AND critical_manifest_gates_executed_by_ci == 100_percent
AND current_authority_dead_links == 0
AND current_AGENTS_architecture_claims_match_source == true
AND current_capability_claims_without_status == 0
AND TRUE_claims_without_evidence == 0
AND conflicting_generated_counts == 0
AND active_plans_without_status == 0
AND duplicate_active_plan_ids == 0
AND current_plan_premises_without_verified_commit == 0
AND current_docs_missing_file_refs == 0
AND roadmap_first_read_path_defined == true
AND completion_definitions_canonical == true
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 24. Implementer Handoff

1. Run 29A before editing canonical instructions.
2. Fix rulebook synchronization before propagating any new AGENTS content.
3. Repair `GEMINI.md` and place it inside the same contract as every other client.
4. Never hand-edit generated docs.
5. Verify CI actually executes every critical manifest gate.
6. Treat `AGENTS.md` as architecture code: stale instructions are production defects.
7. Generate volatile metrics rather than writing them into prose.
8. Give every current capability claim evidence or an unverified label.
9. Surface code-only content policy in generated documentation.
10. Preserve resolved history in archive, but remove it from current authority.
11. Publish one roadmap numbering policy and never retro-renumber history.
12. Make premise re-verification Step 0 for every future plan.
13. Define "done" once per deliverable class.
14. Keep active roadmap small; archive executed work visibly.
15. Make current truth reachable from AGENTS in a few hops.
16. Close Wave 3 only after regenerated docs, claims, roadmap, and all fast gates are green.

---

# 25. Final Outcome

When this plan is complete, ASHFALL has one coherent record of itself.

Every agent begins from the same canonical rulebook. Client instruction files cannot silently drift. Generated documentation identifies itself and is rebuilt from source rather than edited by hand. CI proves that the gate manifest is not aspirational.

The canon registry no longer treats presence as capability. Current claims point to source, tests, runtime evidence, or gates. Unverified claims say so. Resolved defects remain available historically but no longer masquerade as active work. Code-only policy—such as intentional content exemptions—is visible to the people and agents making decisions.

The roadmap stops being a pile of numbered markdown files. One numbering policy, one wave ledger, one status vocabulary, one premise-verification protocol, and one definition of done per deliverable class make current work obvious.

The result is not more documentation. It is a smaller, executable truth layer that prevents good agents from confidently implementing yesterday's architecture.
