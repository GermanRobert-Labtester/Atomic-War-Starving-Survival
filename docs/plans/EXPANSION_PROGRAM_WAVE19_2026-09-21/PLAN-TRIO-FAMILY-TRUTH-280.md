# PLAN-TRIO-FAMILY-TRUTH-280 — Crafting, Journal & Disease Support Files

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-RECIPE-REACHABILITY-TRUTH-125, PLAN-JOURNAL (existing owner), PLAN-QUARANTINE-STRAIN-TRUTH-241.
**Non-goals:** no new content; this audit groups three small families with one
method.

## 1. Outcome
Three small families lack plan coverage — **18 files total**:
`Crafting/` (6: `ChemicalSynthesisCatalog`, `CraftContext`,
`PharmaRecipeCatalogLoader`, `RelicCatalogLoader`, `TrapRecipeIntegrity`,
`RoboticsSystem`), `Journal/` (6: `JournalCorpus`, `JournalEntry`,
`JournalVoice`, `JournalVoiceProseCatalog`, `KnowledgeBase`, `RiskBiasTrait`),
`Disease/` (6: `ContainmentCapability`, `DiseaseTriage`,
`IDiseaseOutbreakSource`, `PathogenStrainCatalog`, `PathogenStrainSave`, a
demo). Each is a data/type layer beneath existing owners — a contiguous audit
keeps it reviewable.

| Deliverable | Detail |
|---|---|
| Crafting data | catalogs/loaders resolve; `TrapRecipeIntegrity` is invoked by Plan 125's checks; `RoboticsSystem` boundary with Plan 79 stated |
| Journal support | corpus/voice/prose catalogs resolve; knowledge base is read-only for other owners |
| Disease support | triage/capability types feed Plan 241; strain save round-trips; `IDiseaseOutbreakSource` has one implementer |
| Test presence | fixtures for each family's data layer |
| Dead report | support files with no consumer are reported |

## 2. Evidence
- 18 basenames across the three directories absent from every plan body (Wave 19 file-level audit).
- Plans 125/241 and the journal owner are the consumers; this plan verifies their data.
- Plan 79 owns robotics content the RoboticsSystem boundary references.

## 3. Packages
- **TRF-280A** crafting data resolution + integrity invocation.
- **TRF-280B** journal catalog resolution + read-only check.
- **TRF-280C** disease support wiring + strain save round-trip.
- **TRF-280D** fixtures + dead report.

## 4. Acceptance & verification
- All three families resolve to consumers; integrity checks run; saves round-trip.
- `bash scripts/run_test.sh` on the three regions.

## 5. Risks
Three-family scope → packages are per family; each is independently reviewable.
Boundary with 79/125 → stated in the claim before edits.

---

## 6. Expanded census (27 files in scope · 7,406 lines · 0 still unmentioned)

Scope: files under `Assets/Ashfall.Core/Crafting, Journal, Disease/`. The plan's premise was the family **at authoring time**; later
plans have since referenced some files, so the table marks each row
`still unmentioned` or `since-mentioned`. Both are in scope for this plan: the
since-mentioned files are re-verified for owner discipline, and the still-
unmentioned ones are the original audit target. Class distribution:
Support 10 · System 8 · Catalog 4 · Loader 3 · Demo 1 · Save 1.

| File | Lines | Class | Banned | Empty catches | Capture/Restore | Status |
|---|---:|---|---:|---:|---:|---|
| `ChemicalSynthesisCatalog.cs` | 199 | Catalog | 0 | 0 | 0 | since-mentioned |
| `ChemicalSynthesisSystem.cs` | 321 | System | 0 | 0 | 2 | since-mentioned |
| `CraftContext.cs` | 29 | Support | 0 | 0 | 0 | since-mentioned |
| `CraftingSystem.cs` | 496 | System | 0 | 0 | 2 | since-mentioned |
| `PharmaRecipeCatalogLoader.cs` | 107 | Loader | 0 | 0 | 0 | since-mentioned |
| `RecipeCatalogLoader.cs` | 179 | Loader | 0 | 0 | 0 | since-mentioned |
| `RelicCatalogLoader.cs` | 109 | Loader | 0 | 0 | 0 | since-mentioned |
| `RoboticsSystem.cs` | 367 | System | 0 | 0 | 2 | since-mentioned |
| `TrapRecipeIntegrity.cs` | 81 | Support | 0 | 0 | 0 | since-mentioned |
| `ContainmentCapability.cs` | 50 | Support | 0 | 0 | 0 | since-mentioned |
| `DiseaseCatalog.cs` | 632 | Catalog | 0 | 0 | 0 | since-mentioned |
| `DiseaseHeadlessDemo.cs` | 490 | Demo | 0 | 0 | 11 | since-mentioned |
| `DiseaseQuarantineCoordinator.cs` | 376 | System | 0 | 0 | 0 | since-mentioned |
| `DiseaseSystem.cs` | 1707 | System | 1 | 0 | 3 | since-mentioned |
| `DiseaseTriage.cs` | 263 | Support | 0 | 0 | 0 | since-mentioned |
| `IDiseaseOutbreakSource.cs` | 80 | Support | 0 | 0 | 0 | since-mentioned |
| `PathogenStrainCatalog.cs` | 71 | Catalog | 0 | 0 | 0 | since-mentioned |
| `PathogenStrainSave.cs` | 113 | Save | 0 | 0 | 0 | since-mentioned |
| `PathogenStrainSystem.cs` | 361 | System | 0 | 0 | 2 | since-mentioned |
| `JournalCorpus.cs` | 348 | Support | 0 | 0 | 0 | since-mentioned |
| `JournalEntry.cs` | 34 | Support | 0 | 0 | 0 | since-mentioned |
| `JournalSystem.cs` | 484 | System | 0 | 0 | 4 | since-mentioned |
| `JournalVoice.cs` | 59 | Support | 0 | 0 | 0 | since-mentioned |
| `JournalVoiceProseCatalog.cs` | 150 | Catalog | 0 | 0 | 0 | since-mentioned |
| `KnowledgeBase.cs` | 152 | Support | 0 | 0 | 2 | since-mentioned |
| `ProceduralEulogyEngine.cs` | 105 | System | 0 | 0 | 2 | since-mentioned |
| `RiskBiasTrait.cs` | 43 | Support | 0 | 0 | 0 | since-mentioned |

**Census totals:** 1 banned nondeterministic references · 0 empty-catch sites · 9 files with capture/restore methods.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `faction_war_journal.json` | array[26] |
| `journal_entries_expansion_05.json` | object[2 keys] |
| `journal_voice_prose.json` | object[2 keys] |
| `disease_catalog.json` | object[5 keys] |
| `awl_saddle_stitch_journals.json` | array[7] |
| `dweller_psychological_journals.json` | array[8] |

**State surfaces (capture/restore present):**

- `ChemicalSynthesisSystem.cs`
- `CraftingSystem.cs`
- `RoboticsSystem.cs`
- `DiseaseHeadlessDemo.cs`
- `DiseaseSystem.cs`
- `PathogenStrainSystem.cs`
- `JournalSystem.cs`
- `KnowledgeBase.cs`
- `ProceduralEulogyEngine.cs`

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/Crafting/` |
| Files referenced by tests | 245 name references across the test tree |
| Determinism scan | 1 banned references to fix or justify |
| Failure scan | 0 empty-catch sites to route through Plan 35's rules |
| Drift | 27 of 27 files became plan-referenced since authoring — re-verify their owners |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census first (section 6) — classify every file in scope; no edits.
2. Still-unmentioned files: the original audit target — consumer or ownerless verdict.
3. Since-mentioned files: confirm the new plan's claim actually owns them; no double ownership.
4. Catalogs and loaders: justify or report inert.
5. Systems and saves: one owner per state; keys per Plan 1 Appendix Q.
6. Regression: focused region plus this census regenerated.

## 10. Acceptance matrix

| File class | Acceptance |
|---|---|
| Catalog | resolves through a loader; malformed fixture fails typed with the field named |
| Loader | valid/invalid fixture pair; unknown id names the field |
| DTO/Type | round-trip or consume-only proof; no orphan type |
| Save | capture/restore round-trip; key per Plan 1 Appendix Q |
| System | one owner per state; no parallel store |
| Demo | resolves to an existing verb or is retired |
| Support | consumed by a system or reported ownerless |

**Non-goals unchanged:** this expansion adds census and verification detail; it
does not widen the plan's scope or create new authorities.

---

## 12. Cross-plan coupling

This is a family-survey plan; the domain set is the plan's own `.cs` enumeration
(27 files). Other plans referencing those names: **18**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-PANDEMIC-PUBLIC-HEALTH-47` | 9 |
| `PLAN-QUARANTINE-STRAIN-TRUTH-241` | 9 |
| `PLAN-HOST-EVENT-ARCHIVE-91` | 5 |
| `PLAN-CRAFT-QUALITY-TRUTH-112` | 5 |
| `PLAN-RECIPE-REACHABILITY-TRUTH-125` | 4 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 3 |
| `PLAN-VERTICAL-CULTURE-04` | 2 |
| `EVIDENCE` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `TRF-280A` | `CraftingSystem.cs`, `TrapRecipeIntegrity.cs` |
| `TRF-280B` | `JournalVoiceProseCatalog.cs`, `ChemicalSynthesisCatalog.cs`, `DiseaseCatalog.cs` |
| `TRF-280C` | `DiseaseCatalog.cs`, `DiseaseHeadlessDemo.cs`, `DiseaseQuarantineCoordinator.cs` |
| `TRF-280D` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 27. Host files: **61** · Test files: **126** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 61 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Disease/DiseaseHostSession.cs`, `src/Foundry/SilentFoundryHostSession.cs`, `src/Host/ArchiveDeskHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 126 | `Ashfall.Core.Tests/AbyssalAnomaliesCatalogTests.cs`, `Ashfall.Core.Tests/AbyssalAnomaliesRuntimeActivationTests.cs`, `Ashfall.Core.Tests/ArchitectureHardeningCrossPlanIntegrationTests.cs`, `Ashfall.Core.Tests/ArchiveDeskSystemTests.cs`, `Ashfall.Core.Tests/ArchiveInksCatalogTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/narrative/journal_entries_batch_1.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **11** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `chlor_alkali_synthesis` |
| `crafting` |
| `disease` |
| `journal` |
| `knowledge` |
| `pathogen_strains` |
| `procedural_narrative` |
| `robotics` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **9** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--chemical-dependency-save-selftest` |
| `--data-integrity-selftest` |
| `--disease-expansion-selftest` |
| `--disease-selftest` |
| `--ice-road-tick-demo` |
| `--journal-save-selftest` |
| `--journal-selftest` |
| `--journal-uitest` |
| `--journal-weather-panel-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **21**.

| Event | First declaration |
|---|---|
| `OnBlightOutbreak` | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` |
| `OnCraftCompleted` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnCraftStarted` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnCraftingPenaltyChanged` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnDependencyRisk` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |
| `OnEntryAdded` | `Assets/Ashfall.Core/Journal/JournalSystem.cs` |
| `OnEntryRead` | `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs` |
| `OnEulogySpoken` | `Assets/Ashfall.Core/Journal/ProceduralEulogyEngine.cs` |
| `OnFrostbiteRisk` | `Assets/Ashfall.Core/ShelterThermalSystem.cs` |
| `OnJournalTriggered` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnLeaderBreakRisk` | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/chlor_alkali_synthesis_catalog.json` |
| `Assets/StreamingAssets/Data/disease_catalog.json` |
| `Assets/StreamingAssets/Data/documents/vel_triage_log_names.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_war_journal.json` |
| `Assets/StreamingAssets/Data/journal_entries_expansion_05.json` |
| `Assets/StreamingAssets/Data/journal_voice_prose.json` |
| `Assets/StreamingAssets/Data/mineral_acid_synthesis_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/chef_recipe_development.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (4 files, 25 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Crafting` | 1 | 11 |
| `Voice` | 3 | 14 |

**Verdict:** 25 cases sit under matching regions — run those first (`Crafting`, `Voice`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **47**
(12 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Host/ChemicalDependencyHostSession.cs` |
| `src/Host/ChemicalDependencySaveSelfTest.cs` |
| `src/Host/ChemicalDependencySaveStore.cs` |
| `src/Host/ChemicalReconHostSession.cs` |
| `src/Host/ChemicalReconSaveStore.cs` |
| `src/Host/ChemicalSynthesisHostSession.cs` |
| `src/Host/ChemicalSynthesisSaveStore.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/CraftingHostSession.cs` |
| `src/Host/CraftingSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **11**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `chlor_alkali_synthesis` | no |
| `crafting` | no |
| `disease` | no |
| `journal` | no |
| `knowledge` | no |
| `pathogen_strains` | no |
| `procedural_narrative` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `aquaponics_disease` |
| `disease` |
| `mineral_chemical` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **23**
(CODEX_ONLY 7, GAMEPLAY_CONSUMED 11, OPTIONAL 2, UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `chlor_alkali_synthesis_catalog.json` | UNRESOLVED |
| `disease_catalog.json` | GAMEPLAY_CONSUMED |
| `documents/vel_triage_log_names.json` | OPTIONAL |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_war_journal.json` | GAMEPLAY_CONSUMED |
| `journal_entries_expansion_05.json` | OPTIONAL |
| `journal_voice_prose.json` | GAMEPLAY_CONSUMED |

**Verdict:** 3 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** hub · **Coupling (incoming plans):** 18
**Surface:** save sections 11 (laddered 0) · RNG streams 3 · host files 15 · catalogs 22 · test regions 2 · flags 9

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-TRIO-FAMILY-TRUTH-280
wave: 19
status: PROPOSED — foreman claim required
packages: TRF-280A, TRF-280B, TRF-280C, TRF-280D
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Host/ChemicalDependencyHostSession.cs  # §19 candidate host surface
  - src/Host/ChemicalDependencySaveSelfTest.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/chemical_dependency_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/chemical_syntheses.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Crafting/
  - godot --headless --path . -- --chemical-dependency-save-selftest
dependencies:
  - coordinate: 18 other plan(s) name these artifacts (§12)
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
