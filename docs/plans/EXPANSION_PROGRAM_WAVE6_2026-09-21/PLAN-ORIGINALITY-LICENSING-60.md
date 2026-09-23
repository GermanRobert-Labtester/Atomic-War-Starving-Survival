# PLAN-ORIGINALITY-LICENSING-60 — Provenance, Attribution & Copy Audit

**Wave 6 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ASSET-PIPELINE-19, PLAN-NARRATIVE-GRAPH-18.
**Non-goals:** legal advice; this is a repository hygiene and provenance
programme. It does not remove assets without human review.

## Outcome
`AGENTS.md` forbids copied art, text, UI layouts, and real-world references, and
the repo has an SPDX header check (`license-header-check.sh --strict`, 1,867+
files) plus provenance fields in the asset registry. But there is no single
**provenance ledger** covering assets, fonts, audio, prose, and generated
content, and no copy-detection pass. This plan adds both.

| Deliverable | Detail |
|---|---|
| Provenance ledger | one table per asset family: original / generated (recipe + tool) / migrated (source + license) |
| Copied-text audit | near-duplicate detection inside the repo (prose hygiene) and an external-text review checklist for writers |
| Fonts/audio | licenses recorded (e.g. Barlow Condensed family) with redistribution terms; cues list origin |
| UI layout originality | checklist that panels are original layouts, not copied from other games (tone rule) |
| Generated assets | recipe pinning (ties to PLAN-ASSET-PIPELINE-19 AP-19E) with tool/version/seed |
| Compliance gate | every shipped binary asset has a provenance row and a license field; new assets without one fail |

## Evidence
- `assets/fonts/BarlowCondensed-*.ttf` (3+ weights) — license must be recorded.
- Asset registry: 355 assets / 6 families with `source_model_provenance` support; `strict_mode: false`.
- Audio catalogs (4), 3,858 LFS objects.
- Prose catalogs: `item_description_texts.json` (273 KB), `medical_texts.json` (222 KB), narrative batches.
- `license-header-check.sh --strict` covers code; `sources.md` (50 KB) is an uncertified reference file.
- Tone rules: fictional, restrained, no real countries/wars/people, no copied art/text/layouts.

## Packages
- **OL-60A** provenance ledger generated for all shipped families; missing rows listed as findings.
- **OL-60B** license fields: fonts, audio, any third-party data; redistribution-compatible or replaced.
- **OL-60C** prose duplication pass: internal near-duplicate report + external-originality checklist for authors.
- **OL-60D** UI originality checklist applied to panels; copied-layout suspicion escalated to a human review, never auto-fixed.
- **OL-60E** compliance gate: no shipped asset without provenance; no generated asset without a recipe.

## Acceptance & verification
- 100% provenance rows for shipped assets; licenses recorded; gates green.
- `python3 scripts/ci/generate-asset-registry.py --check`; provenance gate; `bash scripts/ci/license-header-check.sh --strict`; prose hygiene from Plan 18.

## Risks
False copy suspicion → review by a human; findings are labelled "review required", never auto-deleted.

---

## 6. Expanded census (bespoke: asset & license surface)

This plan governs originality and licensing, so the census covers the asset
tree and license files rather than source filenames.

| Metric | Value |
|---|---:|
| Asset files in indexed roots | 8293 |
| Distinct extensions | 23 |
| License files at root | LICENSE |
| Third-party notices file | absent |

**Top extensions:**

| Extension | Count |
|---|---:|
| `.import` | 4075 |
| `.png` | 2069 |
| `.jpg` | 1666 |
| `.wav` | 194 |
| `.mp3` | 83 |
| `.html` | 60 |
| `.svg` | 47 |
| `.tscn` | 26 |
| `(none)` | 16 |
| `.ogg` | 9 |
| `.json` | 8 |
| `.txt` | 8 |

## 7. Expanded surface: originality contract

| Rule | Detail |
|---|---|
| Provenance | every shipped asset traces to an original or licensed source |
| No copied art/text/UI | layouts and prose are original; no real-world references |
| Notice file | third-party components named with their license |
| Generated assets | produced with the project's own tooling or licensed generators |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Asset inventory | the census above, re-run after each asset batch |
| License check | every non-original item appears in the notice file |
| Originality scan | no real country/person/brand strings in player-facing data |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Asset census (this section) and provenance classification.
2. Notice-file completion for any non-original component.
3. Originality scan over player-facing text.
4. Regression: census + notice check per asset batch.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Asset | has provenance (original or licensed + notice row) |
| Text/UI | original; originality scan clean |
| Notice | complete for every third-party component |
| Census | re-runs without unexplained deltas |

**Non-goals unchanged:** this expansion adds census and verification detail; it makes no licensing determination.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 4. Other plans referencing them: **16**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-NARRATIVE-GRAPH-18` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-INTEGRATION-KIT-02` | 1 |
| `PLAN-UNBLOCK-03` | 1 |
| `PLAN-LAUNCH-FACE-06` | 1 |
| `PLAN-DATA-AUTHORITY-14` | 1 |
| `PLAN-RUNTIME-PERF-16` | 1 |
| `PLAN-RELEASE-OPS-20` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `AGENTS.md` |
| `item_description_texts.json` |
| `medical_texts.json` |
| `sources.md` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `OL-60A` | no name match — resolve at claim time |
| `OL-60B` | no name match — resolve at claim time |
| `OL-60C` | no name match — resolve at claim time |
| `OL-60D` | no name match — resolve at claim time |
| `OL-60E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 4. Host files: **21** · Test files: **29** · Data files: **22**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 21 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Host/ChemicalReconHostSession.cs`, `src/Host/CoreDemoSession.cs`, `src/Host/GeodeticSurveyHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 29 | `Ashfall.Core.Tests/Collectibles/CollectibleContentUtilizationTests.cs`, `Ashfall.Core.Tests/Content/Plan49ContentAtmosphereIntegrationTests.cs`, `Ashfall.Core.Tests/DataRuleComplianceTests.cs`, `Ashfall.Core.Tests/DescriptiveTextsTests.cs`, `Ashfall.Core.Tests/FactionWarChainRunnerTests.cs` |
| Data (`StreamingAssets/Data/`) | 22 | `Assets/StreamingAssets/Data/faction_war_communiques.json`, `Assets/StreamingAssets/Data/field_guide.json`, `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`, `Assets/StreamingAssets/Data/item_description_texts.json`, `Assets/StreamingAssets/Data/items.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **16** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `collectible_discovery` |
| `faction_espionage` |
| `field_guide` |
| `foundry` |
| `geodetic_survey` |
| `host_event` |
| `medical` |
| `medical_pipeline` |
| `medical_ward` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **13** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--atmosphere-selftest` |
| `--audio-selftest` |
| `--audio-test` |
| `--chemical-dependency-save-selftest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--ice-road-tick-demo` |
| `--medical-selftest` |
| `--medical-ward-save-selftest` |
| `--shelter-atmosphere-selftest` |
| `--silent-foundry-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **14**.

| Event | First declaration |
|---|---|
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnEventRaised` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnItemAdded` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnItemDegraded` | `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` |
| `OnItemRemoved` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnMedicalProcessingCompleted` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnTreatyDeliveryAccepted` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnTreatyDeliveryMissed` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnTreatyQuotaMet` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/atmosphere_profiles.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/black_flotilla_items.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (232 files, 1897 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Combat` | 10 | 84 |
| `Economy` | 41 | 329 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Inventory` | 15 | 114 |
| `Medical` | 49 | 435 |
| `Shelter` | 87 | 754 |

**Verdict:** 1897 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Combat`, `Economy`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **240**
(29 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/AudioCueCatalog.cs` |
| `src/Audio/AudioEventBridge.cs` |
| `src/Audio/AudioManager.cs` |
| `src/Audio/AudioSelfTest.cs` |
| `src/Audio/AudioSettings.cs` |
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ExpansionAudioBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Economy/EconomyMarketPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **41**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `collectible_discovery` | no |
| `combat` | no |
| `crossing` | no |
| `disease` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **17**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `combat` |
| `cupola_foundry` |
| `disease` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **144**
(CODEX_ONLY 82, GAMEPLAY_CONSUMED 43, OPTIONAL 6, UNRESOLVED 13).

| Catalog | Classification |
|---|---|
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |

**Verdict:** 13 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_broke_treaty` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** hub · **Coupling (incoming plans):** 16
**Surface:** save sections 41 (laddered 0) · RNG streams 17 · host files 25 · catalogs 22 · test regions 9 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ORIGINALITY-LICENSING-60
wave: 6
status: PROPOSED — foreman claim required
packages: OL-60A, OL-60B, OL-60C, OL-60D, OL-60E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/atmosphere_profiles.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 16 other plan(s) name these artifacts (§12)
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
| verification | **no** |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** author or confirm: verification.
