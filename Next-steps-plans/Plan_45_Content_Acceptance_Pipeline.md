# Plan 45 — The Content Acceptance Pipeline: Nothing Authors Itself Anymore

> **Wave:** Continuity Wave 7 — *Content on Rails & the Measurement Layer*
> (Plans 45–49; predecessors [W1](Wave1_Continuity_Audit_INDEX.md)–[W6](Wave6_Continuity_Audit_INDEX.md).)
> **Depends on:** 36A (port contract), 27A (fixture fidelity), 27C (runtime evidence), 40B (tags),
> 25A/25C (keyed text).
>
> **Theme:** six audit waves produced the same finding in six costumes — content authored, systems
> built, and never connected. Waves 1 and 6 each *cleared a bucket* of it. This wave stops refilling
> the bucket: one acceptance pipeline that any new definition must pass, so "wrote it and forgot it"
> becomes structurally impossible — and one final sweep of the 452 definitions still sitting
> unconsumed today.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | The dead-content bucket is still 29 catalogs / 452 definitions | `artifacts/content-utilization.json` → catalogs with `consumerSystems = []` and a non-`narrative/` path: **29 catalogs, 452 defs** — led by `environmental_atmosphere_expansion.json` (152), `medical_texts.json` (83), `environmental_texts_expansion_05.json` (36), `audio_logs_expansion_05.json` (30), `narrative_encounters_expansion.json` (29), `journal_entries_expansion_05.json` (28), `memorials_expansion_05.json` (27), `echoes.json` (23), `narrative_arc_events.json` (15), `moral_choice_quest_stubs.json` (10), `narrative_questlines.json` (4), `dynamic_questlines.json` (2), `trade_texts.json` (4), `trade_screen_scenarios.json` (3), plus root-array-shape files counted as 0 (`cassette_sets`, `confession_secrets`, `damaged_map_zones`, `final_wishes`, `antigravity_survivor_fields`, `deep_lore_survivor_fields`, `wall_carving_templates`) |
| 2 | The gate cannot see most of it | stage distribution `DISCOVERED 271 · QUERIED 133 · EFFECT_PRODUCED 4 · SELECTED 0 · DESERIALIZED 0 · REGISTERED 0`; evidence tiers `STATIC 402 / RUNTIME 9`; the gate's own summary prints `Actionable Priorities: 0,0,0,0,0` |
| 3 | "Named consumer" is not evidence | 26 catalogs sit in `exemptionId: exempt_no_source_evidence` — the scanner's own lookup tables (`ContentUtilizationScanner.cs:229,350,418,516,674,788`) *assert* a consumer class, and the class never references the file. Wave 1's 18B step 1 is the correction; it needs to become a standing pipeline, not a one-off |
| 4 | Root-array files are counted as empty | `cassette_sets.json`, `guilt_sources.json`, `confession_secrets.json`, `damaged_map_zones.json`, `final_wishes.json` report `definitionCount = 0` — invisible to both the utilisation gate and the `schema_version` presence rule (`CatalogIntegrityValidatorTests` exempts bare-array roots) |
| 5 | Exemptions are permanent by default | `ContentExemption.cs` entries have `Owner/Rationale/TrackingTicket` and, in one case, an `ExpiryCondition` (`exempt_echoes_future`: *"When EchoSystem is implemented and wired"*) — expiry is a comment, not a check |
| 6 | Waves 1/6 cleared parts, not the process | 18A (echoes), 18B (retire the bucket), 40A (identity fields), 40B (tags), 41A/B (carvings, confessions, epitaphs) each fix rows; none of them makes the *next* dead family impossible |
| 7 | The content waves are queued behind this | parallel plans 136 (hunting/cooking), 141 (research unlocks), 142 (clothing/warmth), 145 (endings), 146 (radiation economy), 148–160 add entries to catalogs whose consumer status is exactly what's in row 1 — authoring now, before the pipeline, recreates the backlog |
| 8 | The rails exist to demand proof | `--content-utilization-selftest` with runtime collection (27C), `CatalogIntegrityValidator`'s five tiers, `SaveSectionRegistry`, `PanelRegistry` + 15C's liveness gate, 36A's port contract — every verification surface this pipeline needs is either live or scheduled one step earlier |

---

## Task 45A — Define and enforce acceptance for every new definition

**Goal:** a content PR is *not done* until the definition is loaded, queried, selected, and
effect-producing under a runtime boot — or explicitly removed.

**Files:** `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`,
`src/Host/ContentUtilizationRuntimeCollector.cs`, `Assets/Ashfall.Core/Content/ContentExemption.cs`,
`Assets/Ashfall.Core/CatalogIntegrityValidator.cs` (+ `CatalogIntegrityRules.cs`,
`CatalogIntegrityCheckers.cs`), `artifacts/content-utilization-baseline.json`,
new `docs/content/ACCEPTANCE.md`, new `scripts/ci/content-acceptance-gate.sh`,
`Ashfall.Core.Tests/ContentAcceptanceTests.cs`.

### Substeps

1. **Write the five-stage acceptance ladder** in one page: *Discovered → Loaded → Registered →
   Selected → Effect produced*, each with the exact runtime signal that proves it (a load counter, a
   registry key, a query with args, a state mutation) — the vocabulary already exists in the
   scanner; the *contract* does not.
2. **Make `Effect produced` the definition of done** for gameplay families; `Selected` is the
   minimum for narrative/codex families (text a player can actually reach is a legitimate end state —
   the difference is documented, not implied).
3. **Fix the counting blind spots first** (row 4): root-array catalogs must be counted, not exempted,
   or the ladder passes on files with zero visible definitions.
4. **Turn `exempt_no_source_evidence` into a failure with three exits** — wire, delete, or file with
   `Owner + Rationale + ExpiryCondition + due date`; add expiry *enforcement* (an expired exemption
   fails the gate).
5. **Reject self-attestation** (row 3): the scanner's hand-written consumer tables count as
   *candidate* evidence only; a consumer must appear in source (declaration + call) **and** in a
   runtime observation from a real campaign boot (27C step 1).
6. **One baseline, monotonic**: `artifacts/content-utilization-baseline.json` records per-family
   stage counts; the gate fails on regression and requires a reviewed bump on improvement.
7. **Add the pre-merge check to the authoring workflow**: `docs/content/ACCEPTANCE.md` is the
   checklist every content plan (including the parallel waves) references at step 0 — no new
   catalog, item, quest, echo, line, or policy is created before its consumer exists, matching the
   project's one-component-at-a-time discipline.
8. **Scaffold on the rails, not around them**: the acceptance doc names the five artefacts a family
   needs (entity, loader, system/owner, effect applier, section+route) — generalising Wave 1's 18A
   step 14 pattern doc.
9. **Gate tier**: fast-tier gate for the static half (source evidence), Tier-2 nightly for the
   runtime half (needs a boot), so `verify-fast.sh` stays usable.
10. **Prove the gate can fail**: an intentionally orphaned fixture catalog must trip it
    (the discipline repeated in 15C, 26B, 27A, 36A — a gate that has never failed is a rumour).
11. **Report per family, not per file**: the output should say *"memorials: 27 defs, Loaded, no
    Effect"* rather than a 411-row table nobody reads.
12. **Run the checklist** + `--data-integrity-selftest` + `--content-utilization-selftest`.

**DoD:** new dead content is a build failure, and every exemption has an owner and an expiry date.

---

## Task 45B — Clear the last 452 definitions

**Goal:** finish what Wave 1's 18B started — every remaining unconsumed non-narrative catalog wired
or deleted, using the acceptance ladder as the arbiter.

**Files:** the 29 catalogs in evidence row 1, their intended consumers (`WeatherSystem`/`StartingLevel`,
`MedicalWardSystem`/`AutopsySystem`/`DiagnosisKnowledgeStore`, `NarrativeEncounter*`,
`JournalSystem`, `MemorialSystem`, `TradeScreenPresenter`, `QuestlineSystem`,
`MoralChoiceSystem`), `docs/data/DATA_GAP_AUDIT.md`, `docs/data/CATALOG_REGISTRY.md`,
`piagentsplans/` batch roadmaps.

### Substeps

1. **Re-verify before acting** — `DATA_GAP_AUDIT.md` is stale (`questline_master.json` is loaded at
   `Main.Application.cs:392`); regenerate the dead list from the current artifact rather than from
   the doc (Wave 3's 29B rule, applied to content).
2. **Sort into the three dispositions** (wire / repurpose as text / delete) with a named plan task
   per wire — `environmental_atmosphere_expansion` (152) → Plan 49A; `medical_texts` (83) → 49B;
   audio logs + journal entries + memorials (85) → 49C; encounters + arc events (44) → 49D; questline
   files (14 + 2) → 18A/18B; `moral_choice_quest_stubs` (10) → 15A or delete.
3. **Delete without ceremony where deletion is right**: `trade_screen_scenarios.json` (test
   scenarios masquerading as content) moves to test fixtures or goes; stubs with no intent go.
4. **Repurpose the text families** with the 25C overlay pattern: an environmental-atmosphere line is
   *flavour rendered at a place in a state*, not a gameplay effect — the ladder's `Selected` tier is
   the honest target, and saying so prevents fake wiring to satisfy a metric.
5. **Kill duplicate families while you're here**: `*_expansion_05`, `*_expansion`, and base files
   describing the same concept (five generations of "expansion" suffixes) get merged or renamed with
   a migration note, or the next author cannot find the right file.
6. **Preserve the writing**: nothing authored is discarded silently — deletions land in
   `docs/archive/content/` with the reason (a project that spent this much on prose shouldn't lose it
   to a cleanup commit).
7. **Consumer-side checks**: the wiring must go through the rails — text through 25A keys, items
   through 40B tags, effects through 24B stacks and 22A consumption, events through 31A kinds — not
   through a bespoke loader per family.
8. **Content-integrity tiers** for each newly wired family (reference resolution, no duplicates,
   minDay/maxDay ordering, defined tags).
9. **Re-measure and publish**: before/after per family — defs wired, defs deleted, `EFFECT_PRODUCED`
   and `SELECTED` counts, and the size of the exemption list.
10. **Update the atlas/registry** rows the sweep invalidates (29B), including the
    "Codex-Only" policy decision if any family graduates.
11. **Tests**: one acceptance-ladder test per wired family, a deletion test proving no reference
    remains, and a regression test that a deleted file's absence doesn't break a boot.
12. **Run the checklist** + both content gates.

**DoD:** the unconsumed bucket is empty or explicitly, datedly exempt — and every surviving line of
prose is reachable by a player or archived with a reason.

---

## Task 45C — Authoring ergonomics: make the right thing the easy thing

**Goal:** reduce friction on the rails so contributors and agents stop routing around them —
scaffolding, validation messages that say what to do next, and one command to prove a family.

**Files:** new `scripts/content/scaffold_family.py`, `docs/content/ACCEPTANCE.md`,
`CatalogIntegrityValidator` error text, `ContentUtilizationScanner` report text,
`Ashfall.Core/*CatalogLoader.cs` failure diagnostics, `docs/INDEX.md` (generated),
`scripts/ci/verify-fast.sh --gate content_acceptance`, `piagentsplans/README.md`,
`.agents/skills/ashfall-data-add` + `ashfall-expansion-data-gen` (align, don't fork).

### Substeps

1. **One scaffold command** that emits the five artefacts of a family (entity, loader, system or
   owner hook, effect applier, section+route) with the id prefix, `schema_version`, tests, and an
   acceptance TODO list — the same "don't make humans remember the triad" idea as 28A.
2. **Fail with instructions**: validator/gate messages must name the missing stage and the file to
   create ("`memorials_expansion_05.json` is Loaded but never Selected — add a consumer in X or add
   an exemption with an owner and expiry"), because opaque gate text is how people learn to disable
   gates.
3. **Single verification command** for authors: `--content-utilization-selftest` filtered to one
   family, so the loop is seconds, not a CI run.
4. **Template docs from the rails**: a filled-in "add a family" worked example using an actual
   wired family (echoes from 18A is the freshest) rather than an abstract spec.
5. **Align the skills**: `ashfall-data-add`, `ashfall-expansion-data-gen`, `ashfall-write`,
   `ashfall-expand` all generate content today; each must reference this pipeline so the tool
   layer and the gate layer agree (the `AGENTS.md`-vs-reality drift Wave 3 documented is exactly
   what this step prevents).
6. **Naming policy**: define what `_expansion_05` means, or ban suffix-generations outright and
   prefer per-family files with a version field (row 5 of 45B).
7. **Id discipline automation**: new ids must come from the master list; the scaffold should
   reserve/validate against `CatalogIntegrityRules` prefixes rather than trusting the author.
8. **Migration notes as data**: when a family's shape changes, the loader records a migration note
   (25C's pattern) so a future sweep can tell what happened without archaeology.
9. **Coverage of the ladder by family type**: gameplay, narrative, cosmetic, and infrastructure each
   get a documented required stage, so the gate doesn't demand the impossible from a whitelist file.
10. **Adopt-by-default rule**: any content PR that adds a *new* loader without the ladder's evidence
    fails review — codify in `docs/content/ACCEPTANCE.md` and reference from `AGENTS.md`.
11. **Measure the ergonomics**: cycle time from "idea" to "accepted family" over the next three
    content waves, reported in the wave ledger (49C closes the loop).
12. **Run the checklist** and one end-to-end dry run of the scaffold producing a throwaway family
    that passes and then fails the gate on purpose.

**DoD:** a contributor can do the right thing in one command, and the failure messages tell them
what's missing.

---

## Cross-Task Dependencies

```
36A (port contract) ──► 45A step 5 (source + runtime evidence) ──► 45B (the sweep) ──► 45C (ergonomics)
27A/27C (fidelity, runtime evidence) ──► 45A steps 1–5
40B (tags) ──► 45B step 7 · 25A/25C (keys/overlays) ──► 45B step 4
18A/18B (W1) ──► 45B step 2's plan mapping · 49A–49D consume the cleared families
```

**Execution order:** 36A → 45A → 45B → 45C, and **45A before any queued content wave**
(136/141/142/145–160), because content authored against the old metrics inflates the bucket 45B is
about to empty.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --content-utilization-selftest   # stage counts published
7. bash scripts/ci/content-acceptance-gate.sh                    # (45A)
8. bash scripts/ci/generate-catalog-registry.py --check
9. intentional-orphan fixture trips the gate (45A step 10)
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 45A | 2 | 1 | baseline | 8–12 | Medium | LOW (ships with a dated exemption set) |
| 45B | 4–6 | 4–6 | **29 files** | 12–18 | High (volume) | MEDIUM (deletions must be reference-checked) |
| 45C | 0 | 1 | 0 | 3–5 | Low–Med | LOW |

**Guardrails:** no new content authored to make a metric greener (the whole point); no fake wiring to
satisfy a stage (declare the honest target family instead); no silent deletion of prose; no new
loader that skips the shared catalog pattern; and never edit `docs/data/CATALOG_REGISTRY.md` or
`docs/INDEX.md` by hand — they are generated.
