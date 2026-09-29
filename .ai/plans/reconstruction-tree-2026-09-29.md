# Feature / Task Plan: The Reconstruction Tree — research and rebuild lost knowledge

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit)

> Prose companion: `docs/expansions/expansion_reconstruction_tree_plan.md`. Family index: `docs/expansions/expansion_new_ways_to_play_index.md`.
> Not a claim. Existing research completes and stays known exactly as today unless a node is flagged bearer-dependent.

> **Editorial polish (prose pass):** sections **0**, **1b** and **12** are narrative texture only. No
> authority, claimed path, decision, acceptance criterion or verification step changes. Sample lines
> are content candidates for `reconstruction_lines.json` rows; they belong in data, never in code.

---

## 0. Prologue — What We Knew

> *"A technique is not stored in a book. A book is only the place a technique agreed to wait."*

Research in this game is currently permanent: unlock a node, keep it forever, and 19 read sites
quietly return `true` for the rest of the campaign. That is how a *game* works. It is not how a
*shelter* works.

The Reconstruction Tree asks a single uncomfortable question and answers it mechanically: what
happens when the last person who could do a thing stops being able to do it? There are three kinds
of **bearer** — a person, a page, and a practice — and a node is only as secure as its least
survivable holder. When the second-to-last bearer dies, the node goes Fragile. When the grace
period runs out, it goes Lapsed. And the capability check — for the first time in the campaign —
returns **false**.

**Tone & register.** Scholarly, elegiac, precise. The vocabulary is the workshop and the library:
*discipline, bearer, page-hold, fragment, trial, relearn*. Prose should sound like a librarian who
has realised the catalogue is shorter than the shelves. Never write nostalgia. Write inventory.

**Mystery & texture.** The ~24 authored **lost nodes** are knowledge that was already gone before
the campaign began. Fragments for them surface through existing seams — archive, salvage, dive,
elder, radio letter — which means the plan never *places* history, it only finds residue. DEC-RT-05
is the plan's kindness: a lapsed node never deletes its unlock record. The shelter forgets how to
do a thing; it never forgets that it once could.

## 1. Goal & Outcome

> *Design intent: loss here is opt-in and reversible. That is what makes it bearable to play and
> unbearable to think about.*

- **Goal:** Add **opt-in** knowledge fragility on top of the research authority: derive *bearers* (people, pages, practice) for flagged nodes; mark them Secure/Fragile/Lapsed; add ~24 authored **lost nodes** with fragment sets; let a **reconstruction project** (fragments + reconstructor + trial) rebuild them; make relearning of a lapsed node faster.
- **Outcome (observable):** on a fixed seed a flagged node goes Secure → Fragile when its last-but-one bearer dies, → Lapsed after the grace period; the capability check for that node returns false; fragments recovered through an existing source make a reconstruction startable; a trial resolves deterministically (success/lesson/failed/catastrophic); the node returns; save/load preserves all states.
- **Non-Goals:** no second research system; no lapse for unflagged nodes; no new currency; no new save section; no new routed panel; no change to unlock bridge semantics; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff lists untouched shared paths.

## 1b. Texture, Mystery & Voice

**Three kinds of bearer is the plan's whole philosophy.**

A **person** dies. A **page** burns. A **practice** lapses from disuse. These are three different
rhythms of forgetting and RT-P1 models all three without ever storing a copy of anyone's skill
state (§11). Give each one a distinct visual signature in the Knowledge Ledger: a person is a name,
a page is a place, a practice is a date.

**Fragments are not quest items.**

RT-P3 stores `{ fragmentId, nodeId, foundDay, source }` and reuses the existing acquisition source
enum. A fragment has no weight, no value, no description. It is a *citation*. That austerity is
deliberate: the player is assembling a proof, not a hoard.

**What the player is never told.**

- What the 24 lost nodes *were* before they were lost. They are authored as fragment sets and
  prerequisites. Their pre-collapse history is not written and must not be.
- Whether a trial will fail. RT-P5 rolls success / lesson / failed / catastrophic with a seeded fork
  and reuses the tech-salvage **catastrophic chance** field. The number is visible; the outcome is
  not.
- Who the last bearer was. `KnowledgeBearerProjector` derives holds from skills, pages and practice
  stamps. It returns a status, never a name.
- Why grace is two seasons (DEC-RT-03). The period is authored. It is not a rule of nature.

**Voice — sample fragments (content candidates for `reconstruction_lines.json`).**

> "Node 14, Fragile. One bearer left and he is not teaching. The grace period is two seasons. I have
> started counting seasons differently."

> "Fragments: 3 of 5. The fourth came from a dive and the fifth from a letter, which tells you what
> kind of shelter this is now."

> "Trial: lesson. We failed and we learned exactly one thing. The ledger records which thing."

> "Lapsed. We still have the unlock record. It says we could. It does not say we can."

**Design texture beats.**

- **Opt-in per node (`bearer_dependent`) is non-negotiable.** Unflagged nodes must never lapse. The
  player chooses where fragility lives.
- **Ship-dark parity (§6.2) is the plan's honesty test.** No flags → identical capability read sites.
  Nothing here is imposed.
- **A lesson is a bounded bonus, not a reroll (RT-P5).** Failure teaches exactly one thing and the
  ledger says which. That is the plan's most humane mechanic.
- **Teaching is the point.** RT-P6 lets a taught apprentice secure a Fragile node within one tick.
  The Tree is not a scavenger hunt; it is an argument for schools.

---

## 2. Evidence table (verified 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | 62 nodes, 31 roots, 6 categories, max 18 days; fields `id, display_name, category, description, days_to_complete, prerequisites, breakthrough_item`. | `research_knowledge.json` | LIVE |
| E2 | 30 unlocks across 6 types. | `research_unlocks.json`; `Research/ResearchUnlockBridge.cs` | LIVE |
| E3 | `ResearchSystem`: one active research, eligibility, available/locked/completed, points, blueprint progress, save; section `research`. | `Research/ResearchSystem.cs`; `Save/SaveSectionRegistry.cs` L138 | LIVE |
| E4 | Capability = `IsManualUnlocked`; 19 read sites; no revocation. | grep | LIVE / GAP |
| E5 | Acquisition sources enum + metadata record. | `Research/KnowledgeAcquisitionSource.cs` | LIVE |
| E6 | Tech salvage: complexity, success and **catastrophic** chance, preservation value. | `Research/TechSalvageCatalog.cs`; `tech_salvage.json` | LIVE |
| E7 | Prewar archives (decryption projects), library manuals, cultural tomes, archive desk. | `Research/PrewarArchiveDecryptionSystem.cs`; `prewar_archives.json`; `library_manuals.json`; sections `prewar_archives`, `library_study`, `archive_desk` | LIVE |
| E8 | Skill decay-to-dormant; bunker-wide stop perk. | `Survivors/SkillProgressionSystem.cs` L56, L71, L240–257 | LIVE |
| E9 | Education/apprenticeship: teacher+subject, sessions, graduation, shelter knowledge counter, transcription yield. | `Education/SurvivorEducationSystem.cs`, `ApprenticeshipCurriculumEngine.cs` | LIVE |
| E10 | Survivor death legacy owner. | `Survivors/SurvivorDeathLegacySystem.cs` | LIVE |
| E11 | Mapping node → skill discipline/manual; the "practice" signal source; per-node bearer query surface. | not yet located | **VERIFY (P0)** |
| E12 | Panels: `ResearchPanel`, `ResearchAtlasPanel`, `LibraryStudyPanel`, `ArchiveDeskPanel`. | `src/UI/` | LIVE |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Nodes, prerequisites, completion | `ResearchSystem` | new **lost** nodes and edges as additive catalog rows — **DEC-RT-01** |
| Capability check | `ResearchSystem.HasCapability` | an optional *lapse overlay* consulted for flagged nodes only |
| Skills | `SkillProgressionSystem` | read-only bearer source |
| Pages | archive desk / library / tomes | read-only page-hold source |
| Practice | — | a small recent-use stamp per flagged node |
| Death/teaching | death legacy; education/apprenticeship | read-only |
| Fragments | — | `FragmentLedger` (pure), nested in the `research` DTO — **DEC-RT-02** |
| Project execution | `ResearchSystem` | reconstruction is a project *variant* through the same start/tick/complete path |
| Salvage chance model | `TechSalvageCatalog` | reused *field* for trial catastrophic chance |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Research/KnowledgeBearerProjector.cs` (new, pure), `Research/FragmentLedger.cs` (new, pure), `Research/ReconstructionTrial.cs` (new, pure, seeded), `Research/ResearchSystem.cs` (additive lapse overlay + nested state), `Research/ResearchKnowledgeDef.cs` (additive optional fields: `bearer_dependent`, `lost`, `fragment_set_id`), `CatalogIntegrityValidator.cs` (`INT`), `Random/` stream id (`INT`)
**Data:** `research_knowledge.json` (additive rows/fields — owner-file edit, `INT`), `knowledge_fragments.json` (new), `reconstruction_lines.json`
**Host:** `src/Host/ResearchHostSession.cs`, one day-owner registration (`src/Main.CampaignOwners.cs`, `INT`), sources bridging (archive desk/dive/radio) via existing seams
**Presentation:** `src/UI/ResearchAtlasPanel.cs` (Knowledge Ledger view; DEC-RT-06)
**Tests:** `Ashfall.Core.Tests/Research/KnowledgeBearerProjectorTests.cs`, `FragmentLedgerTests.cs`, `ReconstructionTrialTests.cs`, `Ashfall.Core.Tests/Save/ReconstructionTreeSaveTests.cs`; extend existing research tests

## 5. Packages

### RT-P0 — Premise audit (Auditor; read-only)
- Close E11: for each of the 20 candidate flagged nodes, name its skill discipline, manual (if any), and a *practice* signal; enumerate all 19 capability read sites and classify "safe to return false"; confirm `ResearchSystem` can take an additive overlay without changing `HasCapability` for unflagged nodes; foreman signs DEC-RT-01…10.
- **Accept:** every flagged node has a bearer mapping or is dropped from v1.

### RT-P1 — Bearer projector (Core, pure)
- Derive holds per node from skills (above dormant), pages (archive/library), practice stamps; status Secure/Fragile/Lapsed with a grace period (proposed two seasons) stored as `fragileSinceDay`.
- **Accept:** table-driven; no writes to skill/manual authorities; deterministic.

### RT-P2 — Lapse overlay (Core)
- Flagged and Lapsed → `HasCapability` false; unflagged → unchanged; **Lapsed never removes completion/unlock records** (reversible).
- **Accept:** ship-dark parity: no flags → `IsManualUnlocked`/`HasCapability` outputs identical on a saved corpus; round-trip preserves overlay.

### RT-P3 — Fragment ledger (Core, pure + nested DTO)
- Fragments `{ fragmentId, nodeId, foundDay, source }` in the research DTO; sources routed from existing acquisition seams (archive, salvage, dive, elder, radio letter).
- **Accept:** duplicate fragment ignored; source enum reused; old saves load empty.

### RT-P4 — Lost nodes & tree depth (data)
- 24 lost nodes, 30 edges, 20 bearer flags as additive rows; integrity validator verifies prerequisites/fragment sets/unlock targets.
- **Accept:** validator passes; no existing node id or prerequisite edited; every lost node has ≥3 fragments reachable from an existing source.

### RT-P5 — Reconstruction project & trial (Core + host)
- Start needs *n of m* fragments + a reconstructor with the discipline; ends in a seeded trial: success / lesson / failed / catastrophic (catastrophic chance from the salvage field). A *lesson* = bounded bonus to the next attempt, stored in the nested DTO.
- **Accept:** same seed → same outcome; failed trial keeps fragments; catastrophic applies exactly one authored consequence; relearning a lapsed node uses reduced days but never below a floor.

### RT-P6 — Teaching links (read-only bridges)
- Education/apprenticeship completion and classroom hours count as person-holds; transcription counts as page-holds. No writes to those systems.
- **Accept:** a taught apprentice moves a Fragile node to Secure within one tick; verified with a fixed roster.

### RT-P7 — Knowledge Ledger view (Presentation)
- A tab/section in `ResearchAtlasPanel`: node status, bearers, missing fragments. Focus/back preserved.
- **Accept:** presenter tests; panel holds no authority.

### RT-P8 — Content waves W1–W4 & governance close.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no flagged nodes → research selftests and capability read sites unchanged.
3. Determinism: identical trial outcomes on replay.
4. Save round-trip with a node Fragile, one Lapsed, and a project in progress.
5. No writes to skills, manuals, education, or death legacy.
6. Every lapse and every Fragile transition has a journal line.

## 7. Cross-plan boundaries
- **The Drowned Coast:** dive finds are a fragment source through the existing acquisition seam.
- **Radio Free Ashfall:** a mailbag letter may be a fragment; classroom hour counts as teaching (read-only).
- **Year Two — Generations:** apprentices are the primary *person* hold; the plan reads the apprenticeship roster and never writes it.
- **The Plague Year:** pathogen containment and pharmacology nodes are prime bearer-dependent candidates.
- **The Quiet War:** fragments are contested intelligence (trade/steal); custody stays with the ledger.
- **Shelter Governance:** the heritage/archive bloc's influence is read as a teaching multiplier (data), not authority.
- **Crews and Companions:** expedition finds route into the fragment source; no party writes.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-RT-01 | Lost nodes live in `research_knowledge.json` as additive rows (one owner file). | architecture | Yes |
| DEC-RT-02 | Fragment ledger + lesson bonuses nest in the `research` DTO. | architecture | Yes |
| DEC-RT-03 | Lapse is opt-in per node (`bearer_dependent`); grace period two seasons. | design | Yes |
| DEC-RT-04 | Parallel reconstruction: bounded benches inside `ResearchSystem` vs single project + parallel fragment gathering. | architecture | Decide in P0 (prefer the latter) |
| DEC-RT-05 | Lapsed never deletes unlock records. | compatibility | Yes |
| DEC-RT-06 | Extend `ResearchAtlasPanel`; no new routed panel. | UI | Yes |
| DEC-RT-07 | Trial catastrophic chance reuses the tech-salvage field. | design | Yes |
| DEC-RT-08 | Practice signal is a recent-use stamp, not a new activity system. | architecture | Decide in P0 |
| DEC-RT-09 | Relearning floor (never below N days). | tuning | Yes |
| DEC-RT-10 | No existing node id/prerequisite is edited. | compatibility | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Fragment`, `Bearer`, `Lapsed`, `Reconstruct`)
- [ ] Premise re-verified (Rule 7); ledger files re-read; no overlapping live claim
- [ ] Signed decisions in hand

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing research/unlock/skill-decay/education tests (list from P0 selector)
- [ ] Research selftests (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: the overlay cannot be limited to flagged nodes; any of the 19 read sites cannot tolerate a false capability; bearers would need stored copies of skill/manual state; parallel projects require a second research owner; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered** — not gaps, not TODOs, not deferred work. They
keep the lost knowledge larger than the tree that meters it. Any future plan that answers one must
name the signed decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| RT-OM-1 | Who was the last bearer? | `KnowledgeBearerProjector` returns a status, never a name. Naming a person would turn a loss into a quest. | Never — a rule, not a gap. |
| RT-OM-2 | What were the 24 lost nodes before they were lost? | RT-P4 authors fragment sets and prerequisites only. Their pre-collapse history is deliberately un-written. | A canon owner, if lost-node provenance is ever authored. |
| RT-OM-3 | Why does a practice lapse? | DEC-RT-08 makes the practice signal a recent-use stamp, not a new activity system. Its decay is a mechanism with no stated cause. | Never — texture by omission. |
| RT-OM-4 | What does a *catastrophic* trial destroy? | RT-P5 applies exactly one authored consequence and reuses the tech-salvage chance field. The consequence is content, not doctrine. | Content waves, individually signed. |
| RT-OM-5 | Why is grace two seasons? | DEC-RT-03 authors the period. It is not offered as a rule of nature and must not be read as one. | Never — locked by decision. |
| RT-OM-6 | Were the lost nodes ever known to anyone alive? | Fragments surface through existing seams and are never placed. The plan finds residue and declines to say whose. | Never — the artefact is the atmosphere. |
