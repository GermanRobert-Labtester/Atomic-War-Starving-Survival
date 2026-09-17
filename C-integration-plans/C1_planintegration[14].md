# C1 — Flagship Integration Plan [14]: The Content Acceptance Pipeline — Nothing Authors Itself Anymore

> **Output:** `C1_planintegration[14].md`
>
> **Source baseline:** Plan 45 — The Content Acceptance Pipeline: Nothing Authors Itself Anymore
>
> **Wave:** Continuity Wave 7 — *Content on Rails & the Measurement Layer*
>
> **Depends on:** Plan 36A port-contract enforcement; Plan 27A authority-backed fixture fidelity; Plan 27C runtime evidence; Plan 40B canonical tags; Plan 25A/25C keyed text and overlay conventions; Plan 29B canon/document truth; Plan 31 semantic event evidence.
>
> **Mandatory execution order:** 36A → 45A → 45B → 45C.
>
> **Wave sequencing constraint:** 45A must land before any queued content expansion wave (136, 141, 142, 145–160). New authored content must not be allowed to increase the orphan bucket while 45B is trying to eliminate it.
>
> **Primary architectural rule:** content acceptance is not “file exists” or “scanner names a consumer.” A gameplay family is accepted only when real runtime evidence reaches `EFFECT_PRODUCED`; narrative/codex families are accepted at `SELECTED` when player reachability is the intended terminal state.
>
> **Primary process rule:** every exemption is temporary, owned, dated, and mechanically enforced.
>
> **Guardrails:** no fake wiring to make metrics green; no content written solely to satisfy acceptance numbers; no silent deletion of authored prose; no new bespoke loader that bypasses shared catalog patterns; no hand-edits to generated catalog/index documents.

---

# 0. Mission

Six continuity waves have repeatedly found the same failure pattern:

```text
content authored
→ system exists
→ scanner or docs claim ownership
→ no real loader/query/selection/effect path
→ content remains unreachable
```

Wave 1 fixed examples.
Wave 6 fixed examples.

Plan 45 changes the system that allows those examples to recur.

The repository already has most enforcement rails:

- `ContentUtilizationScanner`;
- runtime collection from real campaign boots;
- `CatalogIntegrityValidator`;
- port-contract gates;
- `SaveSectionRegistry`;
- panel liveness;
- content utilization stages;
- generated registry/index artifacts;
- keyed text;
- canonical tags.

What is missing is a single acceptance contract.

The target lifecycle is:

```text
AUTHOR
  │
  ▼
scaffold / family definition
  │
  ▼
DISCOVERED
  │
  ▼
LOADED
  │
  ▼
REGISTERED
  │
  ▼
QUERIED
  │
  ▼
SELECTED
  │
  ▼
EFFECT_PRODUCED
  │
  ▼
ACCEPTED
```

For narrative/codex-only families:

```text
DISCOVERED
→ LOADED
→ REGISTERED
→ QUERIED
→ SELECTED
→ ACCEPTED
```

The distinction is explicit by family type.

The content pipeline should make this structurally true:

```text
NEW FAMILY
  │
  ├── entity/schema
  ├── loader
  ├── system/owner hook
  ├── effect applier or player-facing selection
  ├── section/route
  ├── tests
  └── acceptance evidence
       │
       ▼
fast static gate
       │
       ▼
Tier-2 runtime gate
       │
       ▼
merge
```

If a family cannot complete the ladder, there are only three valid outcomes:

```text
WIRE
DELETE / ARCHIVE
TEMPORARY EXEMPTION WITH OWNER + EXPIRY + DUE DATE
```

No permanent “we know this is dead” bucket.

---

# 1. Source-Evidence Interpretation

## 1.1 The current dead-content bucket is still large

The source baseline reports:
- 29 unconsumed non-narrative catalogs;
- 452 definitions.

The exact count must be remeasured from the current artifact before implementation, but the scale is sufficient to justify a standing pipeline.

## 1.2 Current metrics are structurally blind

The source reports:
- many definitions stuck at `DISCOVERED`;
- almost no `EFFECT_PRODUCED`;
- zero or near-zero `SELECTED`, `DESERIALIZED`, `REGISTERED`;
- candidate consumer tables being mistaken for evidence.

Therefore 45A must fix measurement before 45B uses it.

## 1.3 Root-array catalogs are undercounted

Files such as:
- `cassette_sets.json`;
- `guilt_sources.json`;
- `confession_secrets.json`;
- `damaged_map_zones.json`;
- `final_wishes.json`
report zero definitions because shape counting assumes object roots.

This must be corrected before baseline acceptance.

## 1.4 “Named consumer” is not runtime proof

A scanner lookup table saying:

```text
this file belongs to MemorialSystem
```

is only candidate evidence.

Acceptance requires:
- actual source call/dependency;
- runtime observation;
- final stage reached.

## 1.5 Exemptions currently do not expire mechanically

An `ExpiryCondition` string is not enforcement.

45A must make due dates and expiry conditions executable gate inputs.

## 1.6 The next content waves depend on this

Queued plans will add more entries to catalogs currently in the dead bucket.

If the gate lands late, Wave 7 recreates the same backlog it is meant to eliminate.

---

# 2. Non-Negotiable Content Invariants

## INV-45.1 — One acceptance ladder

All catalog families use the same stage vocabulary.

## INV-45.2 — Gameplay content terminates at `EFFECT_PRODUCED`

A gameplay definition is not accepted at:
- discovered;
- loaded;
- registered;
- queried;
- selected.

It must mutate or materially affect runtime state.

## INV-45.3 — Narrative/codex families may terminate at `SELECTED`

Only when player-visible reachability is the intended effect.

The family type must explicitly declare this.

## INV-45.4 — Runtime evidence beats self-attestation

Handwritten scanner consumer tables never count as final proof.

## INV-45.5 — Definition counts are shape-correct

Object-root and array-root catalogs are both counted accurately.

## INV-45.6 — Exemptions are temporary contracts

Every exemption requires:
- owner;
- rationale;
- tracking ticket;
- expiry condition;
- due date.

## INV-45.7 — Expired exemptions fail the gate

No comment-only expiry.

## INV-45.8 — No silent deletion

Removed authored prose is archived with:
- source filename;
- reason;
- replacement/supersession if any.

## INV-45.9 — Generated documents remain generated

Do not hand-edit:
- catalog registry;
- docs index;
- generated acceptance artifacts.

## INV-45.10 — New loaders must prove a consumer path

A new loader without acceptance evidence fails.

## INV-45.11 — Family reporting, not file spam

Gate output should summarize at a family level.

## INV-45.12 — The gate must prove it can fail

An intentional orphan fixture is mandatory.

---

# 3. Definition of Done

Plan 45 closes only when:

- acceptance stages are documented precisely;
- family types define required terminal stage;
- root-array catalogs count correctly;
- scanner candidate-consumer tables no longer count as runtime proof;
- source evidence and runtime evidence are both required where appropriate;
- `exempt_no_source_evidence` no longer acts as a permanent bypass;
- every exemption has owner/rationale/ticket/expiry/due date;
- expired exemptions fail;
- one monotonic utilization baseline exists;
- baseline regression fails without reviewed update;
- static acceptance gate runs in fast tier;
- runtime acceptance gate runs in Tier 2;
- an intentionally orphaned family trips the gate;
- report output is family-oriented and actionable;
- all current dead-content families are remeasured from current artifact;
- each current family is classified as wire / repurpose / delete / temporary exempt;
- deletion preserves authored prose in archive where appropriate;
- duplicate `_expansion` generations are consolidated or governed;
- text families use keyed/overlay rails;
- item semantics use canonical tags;
- gameplay effects use shared effect/consume/modifier/event authorities;
- newly wired families pass integrity tiers;
- before/after utilization metrics are published;
- content registry/atlas rows are regenerated;
- deleted files have no references and boot still succeeds;
- a one-command family scaffold exists;
- scaffold emits the standard family artifacts and acceptance TODOs;
- validator/gate errors tell the author exactly which stage is missing;
- authors can run a filtered family acceptance check quickly;
- worked examples use a genuinely wired family;
- agent/data-generation skills point to the pipeline;
- naming/version policy is explicit;
- ID reservation/validation is automated;
- migration notes are machine-visible;
- family-type terminal-stage policy is documented;
- new-loader-without-evidence is review/gate failure;
- acceptance cycle time is measured over subsequent content waves.

---

# 4. Phase P0 — Rebuild the Current Truth

## P0.1 Capture repository baseline

Record:

```text
commit SHA
branch
dirty paths
content-utilization artifact path
catalog count
definition count
stage distribution
runtime evidence count
static evidence count
exemption count
expired exemptions
root-array catalogs
generated registry status
generated docs index status
```

---

## P0.2 Regenerate utilization artifact

Do not rely on stale `DATA_GAP_AUDIT.md`.

Run the current scanner/runtime collector.

Capture fresh:

```text
family
catalog files
definition count
consumer systems
stage
evidence tier
exemption
runtime hits
effect hits
```

---

## P0.3 Recompute dead bucket

Definition:

```text
non-narrative gameplay/content family
AND no accepted terminal stage
AND not valid temporary exemption
```

Publish current count.

---

## P0.4 Build acceptance-family inventory

Create:

`docs/content/CONTENT_FAMILY_ACCEPTANCE_MATRIX.md`

Columns:

```text
family
files
type
definition_count
required_terminal_stage
current_stage
candidate_consumer
source_evidence
runtime_evidence
effect evidence
exemption
owner
due
disposition
status
```

---

## P0.5 Reproduce blind root-array count

Fixture:
- root-array JSON with three defs.

Pre-fix expected:
- zero or incorrect count.

Post-fix:
- 3.

---

## P0.6 Reproduce false consumer evidence

Fixture:
- scanner mapping names a consumer class;
- no source call;
- no runtime evidence.

Expected post-fix:
- NOT ACCEPTED.

---

# TASK 45A — Define and Enforce Acceptance

# 45A.0 Goal

Make acceptance a buildable contract, not a convention.

---

## 45A.1 Publish acceptance ladder

Create:

`docs/content/ACCEPTANCE.md`

Canonical stages:

```text
DISCOVERED
LOADED
REGISTERED
QUERIED
SELECTED
EFFECT_PRODUCED
```

Do not maintain a second vocabulary.

---

## 45A.2 Define `DISCOVERED`

Proof:
- file found by catalog discovery;
- family identified;
- definition count known.

This is existence only.

---

## 45A.3 Define `LOADED`

Proof:
- shipped data path opens/parses file;
- loader reports family;
- no synthetic fixture substitution.

---

## 45A.4 Define `REGISTERED`

Proof:
- definitions enter runtime registry/system catalog;
- stable IDs present;
- duplicates rejected.

---

## 45A.5 Define `QUERIED`

Proof:
- real runtime consumer requests the family/definition set.

Capture:
- system/owner;
- query arguments;
- day/context if useful.

---

## 45A.6 Define `SELECTED`

Proof:
- specific definition chosen for presentation/use.

Examples:
- journal entry rendered;
- encounter selected;
- atmosphere line chosen;
- memorial line shown.

---

## 45A.7 Define `EFFECT_PRODUCED`

Proof:
- selected definition causes a real game effect.

Examples:
- inventory change;
- needs/morale modifier;
- quest state;
- world state;
- unlock;
- condition;
- event consequence.

Rendered text alone is not `EFFECT_PRODUCED`.

---

## 45A.8 Family types

Define:

```text
GAMEPLAY
NARRATIVE
CODEX
COSMETIC_PRESENTATION
INFRASTRUCTURE
TEST_FIXTURE
```

Use minimal set.

---

## 45A.9 Required terminal stage by type

Recommended:

```text
GAMEPLAY → EFFECT_PRODUCED
NARRATIVE → SELECTED
CODEX → SELECTED
COSMETIC_PRESENTATION → SELECTED
INFRASTRUCTURE → REGISTERED or QUERIED depending contract
TEST_FIXTURE → excluded from gameplay acceptance
```

Document exact rationale.

---

## 45A.10 Root-array shape support

Update definition counting to handle:

```text
{ "schema_version": ..., "definitions": [...] }
[ {...}, {...} ]
```

Do not exempt array roots merely because no schema wrapper exists.

---

## 45A.11 Root-array migration policy

Long-term:
- migrate arrays to versioned object root where feasible;
or:
- explicitly support array-root legacy format with migration marker.

Do not silently count then leave schema governance undefined.

---

## 45A.12 Schema-version rule

If array-root legacy remains:
- define explicit exception type;
- no blanket exemption.

---

## 45A.13 Candidate-consumer evidence

Scanner tables become:

```text
candidateConsumerSystems
```

not accepted `consumerSystems` unless corroborated.

---

## 45A.14 Source evidence rule

A consumer counts as source-linked only when:
- loader/catalog injected or resolved;
- consumer method actually calls/querys it;
- source scanner can find the dependency/call.

Class-name mention alone is insufficient.

---

## 45A.15 Runtime evidence rule

Runtime collector must observe:
- load/register/query/select/effect transitions.

Prefer real campaign boot/journey from Plan 27C.

---

## 45A.16 Evidence merge

Per family:

```text
static candidate
+ source call
+ runtime observation
→ accepted stage
```

Stage may never exceed observed evidence.

---

## 45A.17 Exemption schema

Every exemption:

```text
id
family/path
owner
rationale
tracking_ticket
expiry_condition
due_date
terminal_stage_override if any
```

---

## 45A.18 Ban permanent `exempt_no_source_evidence`

Migrate every current use to:
- wire;
- delete;
- temporary dated exemption.

---

## 45A.19 Due-date enforcement

Gate compares:
- current date;
- exemption due date.

Expired:
- fail.

No grace unless explicit reviewed extension.

---

## 45A.20 Expiry-condition enforcement

Where condition is machine-checkable:

```text
"When EchoSystem is implemented and wired"
```

translate to:
- source/gate condition;
- automatically revoke exemption when true.

---

## 45A.21 Exemption extension

An extension requires:
- new due date;
- reason;
- owner;
- reviewed change.

Track extension count.

---

## 45A.22 Baseline artifact

Use:

`artifacts/content-utilization-baseline.json`

Per family:

```text
definition_count
required_stage
current_stage
selected_count
effect_count
exemption_count
```

---

## 45A.23 Monotonic regression rule

Accepted family may not regress from:
- effect → selected;
- selected → queried;
- etc.

Regression fails.

---

## 45A.24 Definition-count change rule

If definition count increases:
- new defs must reach required stage or valid temporary exemption in same change.

No bucket growth.

---

## 45A.25 Improvement baseline update

When stage improves:
- gate reports improvement;
- reviewed baseline update records it.

Do not require manual baseline change just to see improvement if tooling can generate proposal/diff.

---

## 45A.26 Static gate

Create:

`scripts/ci/content-acceptance-gate.sh`

Fast checks:
- schema/counting;
- source evidence;
- exemption validity;
- baseline shape;
- terminal-stage policy metadata;
- generated-registry consistency.

---

## 45A.27 Runtime gate

Tier 2:
- boot real campaign;
- collect runtime stage evidence;
- compare to baseline;
- assert required families hit target.

---

## 45A.28 Filtered family mode

Support:

```bash
content-acceptance-gate.sh --family memorials
```

or current argument style.

---

## 45A.29 Family report

Good output:

```text
memorials
  27 definitions
  required: EFFECT_PRODUCED
  current: LOADED
  missing: SELECTED, EFFECT_PRODUCED
  source candidate: MemorialSystem
  runtime consumer: none
  action: wire consumer or add dated exemption
```

---

## 45A.30 No giant row dump

Detailed file rows may be artifact output.

Console summary remains family-level.

---

## 45A.31 Gate failure instructions

Every failure names:
- family;
- missing stage;
- expected action;
- relevant file/system.

---

## 45A.32 Intentional orphan fixture

Add test family:

```text
orphan_fixture_family
```

Discovered + Loaded only.

Assert:
- acceptance gate fails.

---

## 45A.33 Intentional expired exemption fixture

Assert:
- expired due date fails.

---

## 45A.34 Intentional false-consumer fixture

Scanner candidate without runtime proof.

Assert:
- family remains unaccepted.

---

## 45A.35 Generated content policy integration

Plan 29's `CONTENT_POLICY.md` should include:
- acceptance state;
- exemption due date;
- required terminal stage.

Generated from authority.

---

## 45A.36 Agent rulebook integration

`AGENTS.md` references:

```text
Before adding content:
read docs/content/ACCEPTANCE.md
```

Do not duplicate full policy.

---

## 45A.37 Content plan Step 0

Every content expansion plan begins:

```text
Step 0:
identify target family
identify current consumer
identify required terminal stage
prove consumer seam exists before authoring
```

---

## 45A.38 Five-artifact pattern

For a new family, require where applicable:

1. entity/schema;
2. loader;
3. system/owner hook;
4. effect applier / selection consumer;
5. section/route/player surface.

Tests are additional mandatory proof.

---

## 45A.39 No pre-authoring without consumer seam

A plan that proposes 100 new definitions before a consumer exists should be blocked by process/gate.

---

## 45A.40 CI manifest registration

Register:
- static gate fast tier;
- runtime gate Tier 2.

---

## 45A.41 PR summary

Show:

```text
families improved
families regressed
new definitions
new exemptions
expired exemptions
selected delta
effect delta
```

---

## 45A.42 45A acceptance metrics

Record:

```text
families
definitions
accepted gameplay
accepted narrative
exemptions
expired exemptions
candidate-only consumers
runtime-proven consumers
root-array counts fixed
```

### 45A DoD

New dead content becomes a build failure, and exemptions cannot become permanent by accident.

---

# TASK 45B — Clear the Remaining Unconsumed Bucket

# 45B.0 Goal

Re-measure the current dead bucket and reduce it to:

```text
0 unresolved families
```

where every family is:
- accepted;
- deleted/archived;
- or temporarily exempt with enforced due date.

---

## 45B.1 Re-verify current list

Do not trust stale docs.

Generate from fresh utilization artifact.

---

## 45B.2 Group by family, not filename

Examples:
- environmental atmosphere;
- medical texts;
- audio logs;
- journal entries;
- memorial text;
- echoes;
- narrative encounters;
- questlines;
- moral-choice stubs;
- trade scenarios;
- identity/deep-lore enrichment;
- wall carvings;
- confessions;
- final wishes;
- damaged map zones.

Recompute exact current families.

---

## 45B.3 Disposition taxonomy

Every family gets exactly one:

```text
WIRE
REPURPOSE_TEXT
DELETE_ARCHIVE
TEMP_EXEMPT
ALREADY_ACCEPTED
```

---

## 45B.4 Named implementation task

Every `WIRE` family maps to:
- existing plan/task;
- or a new bounded task.

No vague “future”.

---

## 45B.5 Environmental atmosphere

Source suggests mapping to Plan 49A.

Target likely:
- player-reachable environmental overlay text;
- `SELECTED` terminal stage if pure presentation.

Do not fabricate gameplay effects.

---

## 45B.6 Medical texts

Map to:
- MedicalWard;
- Autopsy;
- DiagnosisKnowledgeStore;
or actual current consumers.

Decide family type:
- narrative/knowledge;
- gameplay.

Use honest terminal stage.

---

## 45B.7 Audio logs / journal / memorial text

Wire through existing:
- JournalSystem;
- memorial pipeline;
- audio-log playback surfaces.

Terminal stage:
- `SELECTED` if player playback is the intended effect.

---

## 45B.8 Narrative encounters / arc events

Use current:
- NarrativeEncounter;
- quest/event;
- world encounter rails.

Gameplay consequences must reach effect stage if choices mutate state.

---

## 45B.9 Questline files

Reconcile with Plan 18A/18B and current loader.

Do not wire stale duplicate quest files if canonical quest authority already supersedes them.

---

## 45B.10 Moral-choice stubs

Either:
- map to real MoralChoice system;
- or delete/archive.

A stub file is not content just because it contains prose.

---

## 45B.11 Trade screen scenarios

If they are test scenarios:
- move to test fixtures;
- remove from production catalog registry.

---

## 45B.12 Root-array families

Now counted accurately:
- classify each;
- migrate shape if needed;
- apply real disposition.

---

## 45B.13 Preserve authored prose

On delete:
- move source snapshot/text to `docs/archive/content/`;
- add archival manifest entry;
- reason;
- superseded-by if applicable.

---

## 45B.14 Archive does not count as runtime content

Archived files:
- excluded from production discovery;
- clearly historical.

---

## 45B.15 Duplicate suffix families

Audit:

```text
base
*_expansion
*_expansion_05
...
```

For the same concept.

---

## 45B.16 Consolidation decision

Per duplicate family:
- merge into canonical file;
- rename by semantic family;
- retain version in schema/migration history.

---

## 45B.17 Migration note

Create:

```text
old files
new canonical file
ID changes
loader changes
compatibility
```

---

## 45B.18 ID preservation

Prefer preserving stable IDs during file merge.

If ID changes:
- migration map;
- reference validation.

---

## 45B.19 Text rails

Repurposed text uses:
- Plan 25A localization keys;
- Plan 25C overlays;
- no inline C# strings.

---

## 45B.20 Tags rails

Item/content categories use Plan 40B tags.

No string-ID parsing.

---

## 45B.21 Effect rails

Gameplay content uses:
- Plan 24B modifier stack;
- Plan 22A consume;
- Plan 35 delivery;
- Plan 31 event kinds;
- existing quest/world authorities.

No family-specific ad hoc effect engine.

---

## 45B.22 Reference integrity

Per family validate:
- referenced IDs exist;
- tags exist;
- min/max day ordering;
- route IDs live;
- localization keys exist;
- effect types registered.

---

## 45B.23 Definition uniqueness

No duplicate IDs across merged families.

---

## 45B.24 Runtime family test

For every wired family:
- real authority-backed boot;
- query;
- selection;
- effect if gameplay.

---

## 45B.25 Deletion reference test

For every deleted production file:
- source search shows no required reference;
- catalog registry regenerated;
- boot succeeds.

---

## 45B.26 Exemption list reduction

Every completed family removes its exemption.

Track:
- before;
- after.

---

## 45B.27 Exemption reason quality

Remaining exemptions must describe:
- why acceptance cannot occur now;
- what exact condition ends exemption.

---

## 45B.28 Publish before/after table

Per family:

```text
definitions
before stage
after stage
selected delta
effect delta
disposition
exemption removed?
```

---

## 45B.29 Update `DATA_GAP_AUDIT`

Regenerate/reconcile.

Mark stale prior claims historical.

---

## 45B.30 Update generated registry

Run generator only.

No hand edits.

---

## 45B.31 Update canon claims

Plan 29B:
- family graduated;
- source/runtime proof;
- archive/deletion.

---

## 45B.32 Content-utilization baseline

Update after sweep.

---

## 45B.33 Bucket-empty assertion

The final report should say:

```text
unresolved non-narrative families: 0
```

Temporary exemptions are separate, counted explicitly.

---

## 45B.34 If bucket cannot reach zero

Do not hide residuals.

For each:
- owner;
- due;
- blocker;
- terminal target.

---

## 45B.35 45B metrics

Publish:

```text
families wired
definitions wired
families selected
families effect-producing
definitions archived/deleted
exemptions removed
exemptions remaining
duplicate families merged
```

### 45B DoD

The current unconsumed bucket is empty as an unresolved category: every surviving family is reachable or temporarily, mechanically exempt.

---

# TASK 45C — Make the Right Path the Easy Path

# 45C.0 Goal

Reduce authoring friction enough that contributors and agents naturally stay inside the acceptance rails.

---

## 45C.1 Create family scaffold

Script:

`scripts/content/scaffold_family.py`

Inputs:

```text
family name
family type
ID prefix
target system/owner
route/section
```

---

## 45C.2 Scaffold outputs

Where applicable generate skeletons for:

1. catalog/entity;
2. loader;
3. system/owner hook;
4. effect/selection consumer;
5. section/route binding;
6. tests;
7. acceptance TODO/manifest entry.

---

## 45C.3 Schema version

New catalog skeleton includes:
- `schema_version`;
- migration note field/pattern if current project uses one.

---

## 45C.4 ID prefix validation

Scaffold queries:
- `CatalogIntegrityRules`;
- master ID registry.

Reject invalid prefix.

---

## 45C.5 ID reservation

Provide deterministic reservation flow.

Avoid:
- hand-chosen colliding IDs.

---

## 45C.6 Family-type template

Different scaffold template for:
- gameplay;
- narrative;
- codex/presentation;
- infrastructure.

Do not generate meaningless effect applier for pure text.

---

## 45C.7 Acceptance TODO

Generated checklist:

```text
[ ] DISCOVERED
[ ] LOADED
[ ] REGISTERED
[ ] QUERIED
[ ] SELECTED
[ ] EFFECT_PRODUCED (if required)
```

---

## 45C.8 Validator failure messages

Change opaque errors into:

```text
<family> is LOADED but not SELECTED.
Next:
- add runtime consumer in <candidate system>
- or classify family terminal stage
- or add dated exemption
```

---

## 45C.9 Loader failure diagnostics

Catalog loaders report:
- family;
- file;
- schema;
- exact failing field/path;
- expected action.

---

## 45C.10 Single-family verify command

Example:

```bash
godot --headless --path . -- \
  --content-utilization-selftest \
  --family echoes
```

or current equivalent.

---

## 45C.11 Fast local loop

Target:
- seconds;
- not full CI.

---

## 45C.12 Worked example

Use a recently wired family, ideally echoes.

Document:
- catalog;
- loader;
- owner;
- selection;
- effect/terminal stage;
- test;
- runtime evidence.

---

## 45C.13 Skill alignment

Update:
- `ashfall-data-add`;
- `ashfall-expansion-data-gen`;
- `ashfall-write`;
- `ashfall-expand`.

Every skill:
- links acceptance doc;
- identifies terminal stage;
- checks consumer seam before authoring.

---

## 45C.14 No skill-side parallel policy

Skills do not invent separate acceptance steps.

One pipeline.

---

## 45C.15 Naming policy

Decide:
- ban generational suffixes like `_expansion_05`;
or:
- define strict semantics.

Recommended:
- semantic per-family filenames;
- version in schema.

---

## 45C.16 Rename migration

When consolidating:
- preserve stable ID;
- loader migration note;
- archive old filename mapping.

---

## 45C.17 Migration notes as data

Where catalog shape changes:
- loader/schema includes migration metadata or generated migration registry.

---

## 45C.18 Family-type acceptance table

Document:

| Type | Required stage |
|---|---|
| Gameplay | EFFECT_PRODUCED |
| Narrative | SELECTED |
| Codex | SELECTED |
| Cosmetic/presentation | SELECTED |
| Infrastructure | REGISTERED/QUERIED as defined |

No hidden exceptions.

---

## 45C.19 New-loader review rule

If diff adds `*CatalogLoader.cs` or equivalent:
- acceptance evidence manifest/test required.

Static gate can detect.

---

## 45C.20 New-catalog review rule

If diff adds production catalog:
- family metadata;
- type;
- terminal stage;
- owner;
- consumer evidence or temporary exemption.

---

## 45C.21 Acceptance manifest

If useful, create machine-readable:

`docs/content/ACCEPTANCE_FAMILIES.json`

Generated/authoritative fields:

```text
family
type
files
required_stage
owner
consumer
route
```

Avoid maintaining duplicate facts manually if scanner can derive them.

---

## 45C.22 Scaffold dry run

Generate throwaway family.

Prove:
- scaffold passes structure;
- runtime acceptance remains missing until consumer wired.

---

## 45C.23 Scaffold successful run

Wire throwaway test family fully.

Prove:
- all stages pass.

Then remove test family cleanly.

---

## 45C.24 Intentional fail after success

Break:
- consumer;
or:
- effect.

Assert gate describes exact missing stage.

---

## 45C.25 Generated docs integration

`docs/INDEX.md` and catalog registry discover:
- acceptance docs;
- generated family status.

No hand edit.

---

## 45C.26 `verify-fast` gate selector

Support:

```bash
bash scripts/ci/verify-fast.sh --gate content_acceptance
```

if gate runner architecture permits.

---

## 45C.27 PR author workflow

Document:

```text
scaffold
→ implement
→ single-family verify
→ data integrity
→ fast gate
→ Tier-2 runtime evidence
→ merge
```

---

## 45C.28 Agent workflow

Agent prompt/rule:

```text
Never generate the next batch of definitions until the previous family reaches its required acceptance stage.
```

---

## 45C.29 Cycle-time metric

For next three content waves record:

```text
family
created_at
first loaded
first selected
first effect
accepted_at
```

---

## 45C.30 Ergonomic target

Measure:
- commands required;
- average failure iterations;
- time to acceptance.

The goal is not bureaucracy.

The goal is making correct wiring cheaper than bypassing it.

---

## 45C.31 Documentation

Add:
- quick start;
- worked example;
- failure examples;
- exemption process;
- family-type table.

---

## 45C.32 45C acceptance metrics

Track:
- scaffold usage;
- average cycle time;
- new families accepted first PR;
- new exemptions;
- gate-disable attempts/overrides if visible.

### 45C DoD

A contributor can create a compliant family from one command, validate it locally, and understand exactly what is missing when the gate fails.

---

# 5. Cross-Task Dependency Graph

```text
36A port contract
      │
      ▼
45A acceptance contract
      │
      ▼
45B dead-content sweep
      │
      ▼
45C authoring ergonomics
```

Supporting:

```text
27A fixture fidelity ─────► real data
27C runtime evidence ─────► selected/effect proof
40B tags ─────────────────► canonical item/content classification
25A/25C keys/overlays ────► text families
29B canon claims ─────────► docs/registry truth
31 events ────────────────► gameplay effect visibility
```

Future waves:

```text
136 / 141 / 142 / 145–160
            │
            ▼
     must start at 45A Step 0
```

---

# 6. Acceptance Stage State Machine

```text
DISCOVERED
   │
   ▼
LOADED
   │
   ▼
REGISTERED
   │
   ▼
QUERIED
   │
   ▼
SELECTED
   │
   ├── narrative/codex ACCEPTED
   │
   ▼
EFFECT_PRODUCED
   │
   └── gameplay ACCEPTED
```

A family may not skip stages in evidence reporting.

---

# 7. Evidence Model

Per stage capture:

```text
family
definition_id
stage
source
consumer
runtime journey
day/context
effect identity
```

Do not store huge payloads.

---

# 8. Exemption Lifecycle

```text
created
→ valid
→ approaching due
→ expired
→ gate fails
→ wire/delete/extend
```

Extensions are visible history.

---

# 9. Root-Shape Contract

Definition counting must support:
- versioned object catalogs;
- legacy root arrays;
- nested definition containers if explicitly registered.

Unknown shape:
- integrity failure.

---

# 10. Family-Type Contract

Family type is not inferred purely from filename.

It is declared or derived from canonical registry.

Reason:
- "medical_texts" may be presentation;
- "moral_choice" may be gameplay.

---

# 11. Deletion/Archive Contract

Delete runtime file only after:

```text
references = 0
registry updated
boot passes
archive copy/reason created if authored prose
```

No orphan references.

---

# 12. Merge/Consolidation Contract

For duplicate family generations:

```text
inventory definitions
dedupe IDs
choose canonical filename
preserve references
migrate loaders
archive old names
regenerate registry
run acceptance
```

---

# 13. Runtime Acceptance Journey

A representative journey should prove multiple family types:

```text
boot
→ atmosphere line selected
→ medical text selected
→ encounter chosen
→ gameplay choice mutates state
→ memorial text selected
→ journal entry selected
```

This yields:
- narrative SELECTED;
- gameplay EFFECT_PRODUCED.

---

# 14. Baseline Governance

Baseline is not a ceiling.

It is:
- regression floor;
- current truth snapshot.

Improvements should ratchet.

---

# 15. PR Diff Semantics

If PR adds:
- 10 definitions;
- only 8 accepted;
- 2 unexempted;

gate fails.

If 2 have valid temporary exemption:
- passes with visible debt count.

---

# 16. Content Debt Budget

Preferred:
- new unresolved debt = 0.

Temporary exemptions:
- explicit;
- dated.

Track count over time.

---

# 17. Failure Injection Matrix

## N45.1 Root-array with 5 defs
Expected: count 5.

## N45.2 Scanner table names consumer but runtime never queries
Expected: not accepted.

## N45.3 Gameplay family reaches SELECTED only
Expected: fail required terminal stage.

## N45.4 Narrative family reaches SELECTED
Expected: accepted.

## N45.5 Expired exemption
Expected: gate fails.

## N45.6 New definition added without runtime evidence
Expected: baseline/delta gate fails.

## N45.7 Deleted file still referenced
Expected: reference-integrity fail.

## N45.8 Generated registry hand-edited
Expected: generator check fails.

## N45.9 New loader no acceptance metadata
Expected: static gate fails.

## N45.10 Throwaway orphan scaffold
Expected: gate fails with instructions.

---

# 18. Test Pyramid

## Tier 1 — Scanner/unit
- shapes;
- stage transitions;
- exemption expiry;
- family type;
- terminal stage.

## Tier 2 — Static gate
- source evidence;
- registry;
- loader/change detection.

## Tier 3 — Runtime
- real boot;
- query;
- selection;
- effect.

## Tier 4 — Sweep
- per-family acceptance;
- deletion reference tests.

## Tier 5 — Ergonomic dry run
- scaffold;
- fail;
- fix;
- pass.

---

# 19. CI Tiering

## Fast
- root shape;
- schema;
- source evidence;
- exemption;
- baseline metadata;
- generated registry;
- new-loader policy.

## Tier 2
- runtime collector;
- selected/effect stages;
- journey.

Keep local fast verification usable.

---

# 20. Metrics Dashboard

Publish per wave:

```text
total families
total definitions
gameplay accepted
narrative accepted
unresolved
temporary exempt
expired exempt
SELECTED
EFFECT_PRODUCED
runtime-proven consumers
candidate-only consumers
```

---

# 21. Content Family Report Example

```text
environmental_atmosphere
  defs: 152
  type: narrative/presentation
  required: SELECTED
  current: LOADED
  candidate consumer: WeatherSystem / StartingLevel
  runtime evidence: none
  disposition: Plan 49A
  due: <date>
```

---

# 22. Error Message Quality Standard

Every gate error answers:

1. What family failed?
2. What stage is current?
3. What stage is required?
4. Why is current proof insufficient?
5. What file/system should be changed?
6. What command verifies the fix?

---

# 23. Generated Artifact Set

Create/update:

```text
docs/content/ACCEPTANCE.md
docs/content/CONTENT_FAMILY_ACCEPTANCE_MATRIX.md
artifacts/content-utilization-baseline.json
scripts/ci/content-acceptance-gate.sh
Ashfall.Core.Tests/ContentAcceptanceTests.cs
scripts/content/scaffold_family.py
docs/archive/content/...
docs/data/CATALOG_REGISTRY.md (generated)
docs/INDEX.md (generated)
```

Possible machine registry:
`docs/content/ACCEPTANCE_FAMILIES.json`

only if it avoids duplicated truth.

---

# 24. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --content-utilization-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/generate-catalog-registry.py --check
bash scripts/ci/verify-fast.sh
```

Also:
- intentional orphan fixture;
- expired exemption fixture;
- filtered family run;
- scaffold dry run.

---

# 25. Recommended Commit Breakdown

```text
45A-1 current utilization remeasure + acceptance matrix
45A-2 root-array counting + shape tests
45A-3 acceptance stage contract
45A-4 source/runtime evidence separation
45A-5 exemption schema + expiry enforcement
45A-6 baseline regression gate
45A-7 static/runtime CI split
45A-8 failure fixtures + docs + AGENTS integration

45B-1 remeasure + family dispositions
45B-2 environmental/medical families
45B-3 journal/audio/memorial text families
45B-4 encounter/arc/quest families
45B-5 stubs/test-scenario deletion/archive
45B-6 duplicate suffix consolidation
45B-7 reference/integrity/content tests
45B-8 final baseline/registry/canon update

45C-1 scaffold script
45C-2 family templates / ID reservation
45C-3 actionable validator messages
45C-4 filtered verify command
45C-5 worked example
45C-6 agent-skill alignment
45C-7 naming/migration policy
45C-8 dry-run + cycle-time instrumentation
```

---

# 26. Risk Register

## R45.1 Gate blocks too much existing content

Mitigation:
- baseline current state;
- dated exemptions;
- no permanent bypass.

## R45.2 Teams fake effects to reach green

Mitigation:
- family terminal-stage policy;
- runtime journey review;
- prohibit fake wiring.

## R45.3 Deletion loses valuable prose

Mitigation:
- archive authored content;
- reason manifest.

## R45.4 Consolidation breaks IDs

Mitigation:
- preserve IDs;
- migration map;
- integrity tests.

## R45.5 Runtime gate is slow

Mitigation:
- family filter;
- Tier 2;
- small deterministic journeys.

## R45.6 Exemptions become a new backlog

Mitigation:
- due dates;
- expiry failures;
- dashboard count.

## R45.7 Scaffold becomes another stale generator

Mitigation:
- generated tests;
- use canonical rules/registries;
- align agent skills.

## R45.8 Family classification is abused

Mitigation:
- review required terminal stage;
- gameplay families cannot masquerade as narrative.

---

# 27. Acceptance Checklist

## 45A — Acceptance contract

- [ ] current artifact remeasured
- [ ] dead bucket count regenerated
- [ ] family acceptance matrix created
- [ ] root-array failure reproduced
- [ ] false-consumer failure reproduced
- [ ] ACCEPTANCE.md written
- [ ] DISCOVERED defined
- [ ] LOADED defined
- [ ] REGISTERED defined
- [ ] QUERIED defined
- [ ] SELECTED defined
- [ ] EFFECT_PRODUCED defined
- [ ] family types defined
- [ ] terminal stage by family type
- [ ] root-array counting fixed
- [ ] legacy-array policy defined
- [ ] schema rule corrected
- [ ] candidate-consumer semantics
- [ ] real source evidence required
- [ ] runtime evidence required
- [ ] evidence merge implemented
- [ ] exemption schema expanded
- [ ] permanent no-source exemption removed
- [ ] due-date enforcement
- [ ] expiry-condition enforcement
- [ ] extension policy
- [ ] baseline artifact per family
- [ ] monotonic regression gate
- [ ] definition-count delta gate
- [ ] improvement update workflow
- [ ] static acceptance gate
- [ ] runtime acceptance gate
- [ ] filtered family mode
- [ ] family-level reporting
- [ ] no giant console dump
- [ ] actionable error messages
- [ ] orphan fixture fails
- [ ] expired exemption fixture fails
- [ ] false-consumer fixture fails
- [ ] content policy generated integration
- [ ] AGENTS references acceptance
- [ ] content plans use Step 0
- [ ] five-artifact pattern documented
- [ ] pre-authoring consumer seam required
- [ ] CI manifest registration
- [ ] PR summary
- [ ] metrics recorded

## 45B — Sweep

- [ ] dead list regenerated
- [ ] families grouped
- [ ] every family has one disposition
- [ ] every WIRE family has named task
- [ ] atmosphere disposition
- [ ] medical-text disposition
- [ ] audio/journal/memorial disposition
- [ ] encounter/arc disposition
- [ ] questline disposition
- [ ] moral-choice stub disposition
- [ ] trade scenario disposition
- [ ] root-array dispositions
- [ ] deleted prose archived
- [ ] archive excluded from runtime discovery
- [ ] duplicate suffix families inventoried
- [ ] canonical files chosen
- [ ] migration notes
- [ ] IDs preserved/mapped
- [ ] text uses keyed/overlay rails
- [ ] items use canonical tags
- [ ] effects use shared authorities
- [ ] reference integrity
- [ ] definition uniqueness
- [ ] runtime family tests
- [ ] deletion reference tests
- [ ] exemptions removed as families graduate
- [ ] remaining exemptions have precise blockers
- [ ] before/after table
- [ ] DATA_GAP_AUDIT reconciled
- [ ] catalog registry regenerated
- [ ] canon claims updated
- [ ] baseline updated
- [ ] unresolved bucket = 0
- [ ] residual temporary exemptions explicit
- [ ] metrics published

## 45C — Ergonomics

- [ ] scaffold command created
- [ ] scaffold inputs minimal
- [ ] five family artifacts generated where applicable
- [ ] schema version included
- [ ] ID validation
- [ ] ID reservation
- [ ] family-type templates
- [ ] acceptance TODO list
- [ ] validator messages actionable
- [ ] loader diagnostics actionable
- [ ] single-family verify command
- [ ] local loop fast
- [ ] real worked example
- [ ] data-add skill aligned
- [ ] expansion-data-gen skill aligned
- [ ] write/expand skills aligned
- [ ] no skill-side parallel policy
- [ ] naming policy
- [ ] rename migration
- [ ] migration notes machine-visible
- [ ] family-type table
- [ ] new-loader review gate
- [ ] new-catalog review gate
- [ ] acceptance manifest only if non-duplicative
- [ ] scaffold orphan dry run
- [ ] scaffold successful run
- [ ] intentional fail after success
- [ ] generated docs integration
- [ ] verify-fast gate selector where supported
- [ ] PR workflow documented
- [ ] agent workflow documented
- [ ] cycle-time metric
- [ ] ergonomics measurement
- [ ] quick-start docs
- [ ] metrics recorded

---

# 28. Ship / No-Ship Gate

**SHIP** only if:

```text
acceptance_ladders == 1
AND gameplay_terminal_stage == EFFECT_PRODUCED
AND narrative_terminal_stage == SELECTED
AND root_array_definition_count_errors == 0
AND candidate_consumer_self_attestation_accepted == false
AND exemptions_without_owner == 0
AND exemptions_without_due_date == 0
AND expired_exemptions == 0
AND unresolved_dead_content_families == 0
AND content_regressions_from_baseline == 0
AND intentional_orphan_fixture_fails_gate == true
AND runtime_acceptance_gate == pass
AND generated_catalog_registry_drift == 0
AND deleted_runtime_files_with_live_refs == 0
AND archived_authored_prose_has_reason == true
AND new_loader_without_acceptance_evidence_fails == true
AND scaffold_family_dry_run == pass
AND filtered_family_verification == pass
AND data_integrity_selftest == pass
AND content_utilization_selftest == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 29. Implementer Handoff

1. Re-measure current utilization before touching the backlog.
2. Fix root-array counting before setting a baseline.
3. Define the acceptance ladder once.
4. Separate candidate consumer hints from source and runtime evidence.
5. Require gameplay content to reach `EFFECT_PRODUCED`.
6. Let narrative/codex content terminate honestly at `SELECTED`.
7. Make every exemption owned, dated, expiring, and gate-enforced.
8. Prove the gate can fail with an orphan fixture.
9. Land 45A before any queued content expansion.
10. Reclassify every current dead family from fresh evidence.
11. Wire through existing rails; do not add bespoke family frameworks.
12. Archive authored prose rather than silently deleting it.
13. Consolidate duplicate suffix generations.
14. Regenerate catalog/canon docs after the sweep.
15. Keep residual debt explicit and dated.
16. Build scaffolding only after the contract is stable.
17. Make errors tell contributors what exact stage is missing.
18. Align every content-generating agent skill to this pipeline.
19. Track acceptance cycle time over the next content waves.
20. Close only when “wrote it and forgot it” is mechanically prevented.

---

# 30. Final Outcome

When this plan is complete, ASHFALL stops treating content reachability as an audit project.

Every new family has a defined path from file to player. The repository knows whether it was discovered, loaded, registered, queried, selected, and whether it produced a real effect. Gameplay content cannot claim success merely because a class name appears in a scanner table. Narrative content does not need fake gameplay effects; it only needs to prove that a player can actually reach it.

The existing orphan bucket is then cleared under the same rules. Families that belong in the game are wired. Pure presentation text is honestly classified and selected through the keyed overlay system. Test scenarios and abandoned stubs leave production. Duplicate `_expansion_05` generations collapse into findable canonical families. Authored prose that is removed is archived with a reason rather than disappearing in a cleanup commit.

Most importantly, the process changes. New loaders arrive with consumers. Exemptions expire. Baselines ratchet. CI detects regressions. A contributor can scaffold a compliant family, run one focused command, and receive an error that says exactly what is missing.

The result is not another content audit.

It is the end of content archaeology as a normal development task.
