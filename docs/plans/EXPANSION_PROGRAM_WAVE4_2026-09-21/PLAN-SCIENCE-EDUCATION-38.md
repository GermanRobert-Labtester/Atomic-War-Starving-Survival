# PLAN-SCIENCE-EDUCATION-38 — Schoolroom, Archive, Salvage Science & Pharma

**Wave:** 4 (2026-09-21) · **Kind:** EXPANSION
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-ORPHAN-SEAL-01 Wave 4, PLAN-UNBLOCK-03 U1 (research
unlocks), PLAN-VERTICAL-BODY-INDUSTRY-05.
**Expanded appendix:** [`PLAN-SCIENCE-EDUCATION-38_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-SCIENCE-EDUCATION-38_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's science & education
systems (2 authorities), each mapped to its parent-plan mechanic row.

**Non-goals:** no real-world weapons research, no explicit pharmaceutical
recipes, no second research authority (`ResearchSystem` remains canonical).

---

## 1. Outcome

Research is a 56-node DAG with an unlock bridge; education, archives, salvage
science and pharma laboratory are authored but idle
(`ApprenticeshipCurriculumEngine`, `SurvivorEducationSystem`,
`LibraryStudySystem`, `PrewarArchiveDecryptionSystem`, `TechSalvageCatalog`,
`ArchaeologySystem`, `WorkshopReverseEngineeringSystem`,
`ResearchKnowledgeCatalogLoader`, `ResearchUnlockBridge`). This plan makes
knowledge a **place, a person, and a process** in the shelter.

Player loop: **teach → study → salvage → decrypt → research → publish → unlock**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Schoolroom | `SurvivorEducationSystem`, `education_curriculum.json` | assign teacher/students | proficiency, graduation |
| Apprenticeship | `ApprenticeshipCurriculumEngine` | pair master/apprentice | faster skill tiers, trade specialties |
| Library | `LibraryStudySystem`, `library_manuals.json` | study manuals | knowledge points, insights |
| Archives | `PrewarArchiveDecryptionSystem`, `PrewarArchiveCatalog` | decrypt fragments | breakthrough clues |
| Salvage science | `TechSalvageCatalog`, `WorkshopReverseEngineeringSystem` | analyse relics/tech | recipes, schematics |
| Research | `ResearchSystem`, `ResearchKnowledgeCatalogLoader` | run project | node unlock |
| Pharma | `PharmaLabSystem`, `pharma_recipes.json` | synthesise, test | medicine supply, dependency risk |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Core files | `Research/` (10), `Education/` (2, host-unreachable), `LibraryStudySystem`, `ArchaeologySystem`, `PharmaLabSystem`, `WorkshopReverseEngineeringSystem`, `AutopsySystem`, `PrewarArchive*` |
| Data | `research_knowledge.json` (56 nodes), `research_unlocks.json`, `education_curriculum.json`, `library_manuals.json`, `pharma_recipes.json`, `autopsy_procedures.json` |
| Sealed prior | Plan 34 (research externalisation), Plan 141 (unlock bridge), Plan 154 education (Core), Plan 87 relic recipes, Plan 213 metallurgy |
| Unlock contract | `ResearchUnlockBridge` binds to `ResearchSystem.OnResearchCompleted` (no duplicate tracking) |
| Facilities | schoolroom/library/lab room tags exist in `shelter_rooms.json` |

---

## 3. Packages

### SC-38A — Schoolroom and curriculum
- Bind `SurvivorEducationSystem` + `education_curriculum.json` to a schoolroom
  room and a teaching duty; sessions are daily, deterministic, with the
  parent-child and facility bonuses already authored (+20% / +10%).
- **Acceptance:** graduation is a real state with an effect; no duplicate skill
  store; teacher load bounded; children/adolescent stages respected.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Education/`.

### SC-38B — Apprenticeship and mastery
- `ApprenticeshipCurriculumEngine` consumes the duty roster: master/apprentice
  pairing, tier progression, and trade-specialty affinity. Uses the existing
  skill progression owner.
- **Acceptance:** mastery tiers are monotonic and persisted; pairing changes
  measured speed; no parallel XP ledger.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Apprenticeship*`.

### SC-38C — Library and archive decryption
- `LibraryStudySystem` converts manuals into insights under quiet-room
  conditions; `PrewarArchiveDecryptionSystem` decrypts archive fragments over
  days, producing research clues and narrative discoveries.
- **Acceptance:** study has a time/cost and a cap; decryption output feeds the
  research DAG only; journal entries for discoveries.
- **Verify:** research + narrative focused suites.

### SC-38D — Salvage science and reverse engineering
- `TechSalvageCatalog` + `WorkshopReverseEngineeringSystem`: analyse recovered
  tech/relics → salvage knowledge → schematics or repair knowledge; failure
  damages the item.
- **Acceptance:** every schematic resolves to a real recipe; reverse
  engineering is deterministic; no free unlocks.
- **Verify:** workshop/relic focused suites.

### SC-38E — Pharma lab and dependency
- `PharmaLabSystem` + `pharma_recipes.json` produce medical supplies; clinical
  testing gates efficacy; dependency risk routes through
  `ChemicalDependencySystem`.
- **Acceptance:** no real drug recipes (abstract compounds only); dependency
  and tolerance use the canonical medical owner; supply chain consumes
  greenhouse/chemistry inputs.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/`.

### SC-38F — Knowledge diffusion
- Shelter knowledge pool: discoveries move from an individual to the shelter
  (library records, teaching, publications), affecting future study speed and
  recipe availability; loss of a key survivor costs knowledge.
- **Acceptance:** pool changes are explainable; no duplicate research state;
  a death can lose an unshared insight.

### SC-38G — Content volumes
- +12 curriculum subjects, +8 manuals, +10 archive fragments, +8 salvage
  schematics, +10 pharma compounds; fictional/abstract; consumer-bound.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Education becomes free skill inflation | per-day caps, teacher load, graduation gates |
| Pharma reads as real pharmacology | abstract compound names; no dosage/recipe realism |
| Research becomes idle-game waiting | parallel projects limited by staff/facility; salvage shortcuts |
| Knowledge diffusion breaks saves | pool rides existing research/education sections |

## 5. Verification

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Research/
bash scripts/run_test.sh Ashfall.Core.Tests/Education/
bash scripts/run_test.sh Ashfall.Core.Tests/Progression/
godot --headless --path . -- --research-selftest
godot --headless --path . -- --data-integrity-selftest
```

---

## 6. Expanded census (2 files · 798 lines)

Scope: `Assets/Ashfall.Core/Education/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ApprenticeshipCurriculumEngine.cs` | 219 | System | **yes** | 0 | 0 | 0 |
| `SurvivorEducationSystem.cs` | 579 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `apprenticeship_catalog.json` | object[2 keys] |
| `education_curriculum.json` | object[4 keys] |
| `education_session_records.json` | array[20] |

**State surfaces:** `SurvivorEducationSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Education/` |
| Test references | 2 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain files: 2. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 2 |
| `EVIDENCE` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SC-38A` | `ApprenticeshipCurriculumEngine.cs` |
| `SC-38B` | `ApprenticeshipCurriculumEngine.cs` |
| `SC-38C` | no name match — resolve at claim time |
| `SC-38D` | no name match — resolve at claim time |
| `SC-38E` | no name match — resolve at claim time |
| `SC-38F` | no name match — resolve at claim time |
| `SC-38G` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 12. Host files: **10** · Test files: **14** · Data files: **8**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 10 | `src/Host/AutopsyHostSession.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/CraftingHostSession.cs`, `src/Host/HostCli.SelfTests.cs`, `src/Host/LibraryStudyHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 14 | `Ashfall.Core.Tests/BodyMind/Plan27BodyMindTests.cs`, `Ashfall.Core.Tests/ContentDeepChainGateTests.cs`, `Ashfall.Core.Tests/Education/ApprenticeshipCurriculumEngineTests.cs`, `Ashfall.Core.Tests/Education/Plan154EducationIntegrationTests.cs`, `Ashfall.Core.Tests/Integration/FullCampaign30DayShelterPlaythroughTests.cs` |
| Data (`StreamingAssets/Data/`) | 8 | `Assets/StreamingAssets/Data/autopsy_procedures.json`, `Assets/StreamingAssets/Data/education_curriculum.json`, `Assets/StreamingAssets/Data/library_manuals.json`, `Assets/StreamingAssets/Data/narrative/conflict_mediation_records.json`, `Assets/StreamingAssets/Data/research_knowledge.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **28** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `apprenticeship` |
| `autopsy` |
| `campaign` |
| `campaign_day` |
| `crafting` |
| `deep_well` |
| `expanded_shelter` |
| `knowledge` |
| `library_study` |
| `research` |
| `shelter` |
| `shelter_assignment` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **21** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--deep-coast-host-selftest` |
| `--deep-coast-playthrough` |
| `--deep-coast-route-selftest` |
| `--deep-coast-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |
| `--research-catalog-selftest` |
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-decor-selftest` |
| `--shelter-hazard-loop-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **13**.

| Event | First declaration |
|---|---|
| `OnAutopsyChanged` | `Assets/Ashfall.Core/AutopsySystem.cs` |
| `OnConflictResolved` | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` |
| `OnConflictStarted` | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` |
| `OnCraftingPenaltyChanged` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnLastSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnPharmaStateChanged` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |
| `OnSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` |
| `OnSurvivorExposed` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnSurvivorFate` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/apprenticeship_catalog.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/education_curriculum.json` |
| `Assets/StreamingAssets/Data/expansion_survivor_fields.json` |
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/survivor_letters_lost_kin.json` |
| `Assets/StreamingAssets/Data/narrative/survivor_profiles_expansion.json` |
| `Assets/StreamingAssets/Data/starting_survivor_cohorts.json` |
| `Assets/StreamingAssets/Data/survivor_life_stages.json` |
| `Assets/StreamingAssets/Data/survivor_roles.json` |
| `Assets/StreamingAssets/Data/survivor_voice_lines.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (2 files, 11 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Education` | 2 | 11 |

**Verdict:** 11 cases sit under matching regions — run those first (`Education`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **20**
(5 of them panels/HUD).

| Host file |
|---|
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ApprenticeshipSaveStore.cs` |
| `src/Host/SurvivorDeathLegacyHostSession.cs` |
| `src/Host/SurvivorDeathLegacySaveStore.cs` |
| `src/Host/SurvivorDeathLegacySelfTest.cs` |
| `src/Host/SurvivorFateSaveStore.cs` |
| `src/Host/SurvivorMentalHealthSaveStore.cs` |
| `src/Host/SurvivorRelationsHostSession.cs` |
| `src/Host/SurvivorRelationsSaveStore.cs` |
| `src/Host/SurvivorSocialSaveStore.cs` |
| `src/Main.SurvivorDeathLegacy.cs` |
| `src/Main.SurvivorFate.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **5**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `apprenticeship` | no |
| `survivor_fate` | no |
| `survivor_mental_health` | no |
| `survivor_relations` | no |
| `survivor_social` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **6**
(CODEX_ONLY 3, GAMEPLAY_CONSUMED 1, OPTIONAL 2).

| Catalog | Classification |
|---|---|
| `antigravity_survivor_fields.json` | OPTIONAL |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `expansion_survivor_fields.json` | GAMEPLAY_CONSUMED |
| `narrative/education_session_records.json` | CODEX_ONLY |
| `narrative/survivor_letters_lost_kin.json` | CODEX_ONLY |
| `narrative/survivor_profiles_expansion.json` | CODEX_ONLY |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_expelled_survivor` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 5 (laddered 0) · RNG streams 0 · host files 13 · catalogs 18 · test regions 1 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SCIENCE-EDUCATION-38
wave: —
status: PROPOSED — foreman claim required
packages: SC-38A, SC-38B, SC-38C, SC-38D, SC-38E, SC-38F, SC-38G
claim paths:
  - src/Host/ApprenticeshipHostSession.cs  # §19 candidate host surface
  - src/Host/ApprenticeshipSaveStore.cs  # §19 candidate host surface
  - src/Host/SurvivorDeathLegacyHostSession.cs  # §19 candidate host surface
  - src/Host/SurvivorDeathLegacySaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/apprenticeship_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Education/
  - godot --headless --path . -- --campaign-journey-selftest
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
