# PLAN-DATA-AUTHORITY-14 — Catalog Lifecycle, Consumption Proof & Schema Normalization

**Program:** ASHFALL Expansion & Integration Program — Wave 2 (2026-09-21)
**Status:** PROPOSED — not a claim. Foreman claim required.
**Owner role on execution:** Integrator (catalog registry policy) with Builders
per catalog family.
**Depends on:** PLAN-INTEGRATION-KIT-02 (ledger-truth gate), PLAN-ORPHAN-SEAL-01
(catalog consumers arrive with the wiring), PLAN-CORE-ONLY-REGISTRY-11 (retired
catalogs land there).
**Expanded appendix:** [`PLAN-DATA-AUTHORITY-14_APPENDIX-A_CATALOG_CLASSIFICATION.md`](PLAN-DATA-AUTHORITY-14_APPENDIX-A_CATALOG_CLASSIFICATION.md)
— the classification inventory for **538 catalogs**: GAMEPLAY_CONSUMED 162 ·
CODEX_ONLY 279 · OPTIONAL 24 · **UNRESOLVED 73** — the UNRESOLVED rows are the
DA-14A triage queue, with a suggested action per class.
**Non-goals:** no second data store, no DB, no splitting an authority file into
per-system caches, no hand edits of generated catalogs.

---

## 1. Outcome

703 JSON files are the game's authored authority. Some are consumed every day;
some are `UNRESOLVED` by the content-utilization baseline; some belong to
systems that are not wired at all. This plan makes catalog state explicit and
keeps it that way:

1. **every catalog has a lifecycle class** — `GAMEPLAY_CONSUMED`, `PRESENTATION`,
   `OPTIONAL`, `AUTHORING_ONLY`, `RETIRED` — with a named loader and consumer;
2. **schema hygiene** — schema_version present, snake_case keys, no unknown
   top-level keys, enforced by the existing validators;
3. **consumption proof** — the catalog registry row names the live consumer, not
   just the file;
4. **large-file discipline** — validation and load budgets for the biggest
   files, with no game-logic forks;
5. **retired data** — rows whose systems are retired are deleted with evidence
   and listed in the Core-only registry.

**Non-goal example:** `items.json` (390 KB) stays the single item authority; it
is never split per category.

---

## 2. Premise evidence (2026-09-21)

| Fact | Value | Source |
|---|---:|---|
| JSON files under `Assets/StreamingAssets/Data` | 703 | `find … -name '*.json'` |
| Subdirectories | `documents/`, `narrative/`, `whitelists/` | `find -mindepth 1 -maxdepth 1 -type d` |
| Files over 100 KB | 21 | `find … -size +100k` |
| Largest | `items.json` 390 KB, `moral_choice_quests_branching.json` 340 KB, `item_description_texts.json` 273 KB, `events.json` 241 KB | size listing |
| Content-utilization classes observed | `GAMEPLAY_CONSUMED`, `UNRESOLVED`, `OPTIONAL` | `artifacts/content-utilization-baseline.json` |
| `UNRESOLVED` examples | `acoustic_triangulation_catalog.json`, `anomalous_expedition_encounters.json`, `armored_crawler_modules.json`, `audio_cues.json`, `ballistic_shield_catalog.json`, `belief_movements.json` | same |
| Data-integrity selftest | walks 318+ catalogs | INTEGRATION_PLANS evidence |
| Catalog registry | ~605 rows, `--check` gate | `docs/ci/CI_GATE_MANIFEST.json` |
| Path policy | `CatalogPath` routing, forbidden-path gate | `scripts/ci` gates |
| Schema survey artifact | `artifacts/snake-case-survey.json` | artifacts listing |
| Deep-chain artifact | `artifacts/content-utilization-deep-chain.json` | artifacts listing |

---

## 3. Authority map

| Concern | Extend (do not create) |
|---|---|
| Loading | `CatalogFileSystem`, `CatalogPath.CreateFileIOForDataDir`, per-catalog `*CatalogLoader` |
| Validation | `CatalogIntegrityValidator` + `CatalogIntegrityRules/Checkers`; `--data-integrity-selftest` |
| Registry | `docs/data/CATALOG_REGISTRY.md` (generated) + `generate-catalog-registry.py` |
| Consumption | `--content-utilization-selftest`, baseline + deep-chain artifacts |
| Schema | schema envelope pattern (schema_version int, snake_case, nullable DTOs, collected errors) |
| Prohibited | new `res://assets/StreamingAssets/Data` literals; bypassing `CatalogPath` |

---

## 4. Packages

### DA-14A — UNRESOLVED triage
- For each `UNRESOLVED` catalog: find its loader and its consumer in current
  source. Classify:
  - `GAMEPLAY_CONSUMED` — live consumer exists (fix the baseline classifier or
    the path wiring if it was mis-detected);
  - `OPTIONAL` — loaded but not required, with a documented trigger;
  - `AUTHORING_ONLY` — used by tools/tests, never at runtime;
  - `RETIRED` — no loader or no consumer → delete + Core-only registry row.
- **Acceptance:** zero `UNRESOLVED` after the sweep; each row cites the loader
  file and the consumer call site.
- **Verify:** `godot --headless --path . -- --content-utilization-selftest`;
  `python3 scripts/ci/generate-catalog-registry.py --check`.

### DA-14B — Schema normalization and migration notes
- Run the snake-case survey; for each non-conforming key: rename with a
  JsonPropertyName alias or a one-time migration note (never both silently).
- Enforce: integer `schema_version`, known top-level keys (reject unknown with
  collected errors), no `null` arrays where the loader assumes `new List<>`.
- **Acceptance:** survey delta reported; validator rules added for unknown
  top-level keys; all loaders still tolerate missing optional fields.
- **Verify:** `--data-integrity-selftest`; focused catalog tests per family.

### DA-14C — Consumer proof in the registry
- Extend `generate-catalog-registry.py` rows to include: loader class, consumer
  owner (system or host session), and lifecycle class. `--check` fails when a
  row lacks a consumer.
- **Acceptance:** registry row count unchanged in shape but every row has a
  consumer; the gate catches a catalog added without a consumer (the exact way
  orphan catalogs appear).
- **Verify:** `python3 scripts/ci/generate-catalog-registry.py --check`.

### DA-14D — Large-file budgets
- Measure load + validate time and allocation for the 21 files >100 KB; set a
  per-file budget; index-heavy lookups must be dictionary-built once at load,
  not scanned per query.
- **Acceptance:** budgets recorded in `docs/data/DATA_PERFORMANCE_BUDGET.md`;
  a regression probe fails when load time exceeds budget by a margin;
  no per-frame catalog re-reads.
- **Verify:** `godot --headless --path . -- --runtime-scale-selftest` (extend
  with a data-load phase) + focused catalog tests.

### DA-14E — Authoring pipeline
- Document the only path for new content: author JSON → `--data-integrity-selftest`
  → consumer test → content-utilization class → registry row. Include the
  `whitelists/` and `documents/` subdirectories.
- **Acceptance:** a new catalog cannot merge without a registry row and a
  classified consumption state (CI).
- **Verify:** `python3 scripts/ci/generate-catalog-registry.py --check`;
  `godot --headless --path . -- --content-utilization-selftest`.

### DA-14F — Retired-system data cleanup
- For the 5 dead authorities (PLAN-ORPHAN-SEAL-01 Wave 1) and any retired
  system: delete the catalogs that only they load, or re-home the data to the
  superseding authority (e.g. powder metallurgy data → `SilentFoundrySystem`
  only if it becomes the consumer).
- **Acceptance:** no catalog without a loader-consumer pair; git history keeps
  the deleted content; `KNOWN_DEBT` row records the retirement.
- **Verify:** `find` diff + data-integrity + catalog registry gates.

---

## 5. Risk register

| Risk | Mitigation |
|---|---|
| "UNRESOLVED" is a classification gap, not orphan data | DA-14A fixes the classifier first; only true orphans are deleted |
| Re-keying JSON breaks authored prose in tests | migrations are additive aliases; tests updated in the same package |
| Large-file budgets become perf gates that flake | budgets measured on median of 5, margin-bounded |
| Registry gate blocks in-flight data work | gate is warn-only until K5 (PLAN-INTEGRATION-KIT-02) then enforcing |

## 6. Verification summary

```bash
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
python3 scripts/ci/generate-catalog-registry.py --check
python3 scripts/ci/generate-docs-index.py --check
```

## 7. Change control

Data files are authored authority: no runtime system may write them; no
duplicate authority per system; a catalog deletion requires the same evidence
standard as a code retirement (PLAN-CORE-ONLY-REGISTRY-11 §4D).

---

## 6. Expanded census (bespoke: data authority surface)

This plan governs the data tree, so the census counts files, version fields,
classification, and schema artifacts.

| Metric | Value |
|---|---:|
| JSON files under `Data/` | 703 |
| With `schema_version` | 703 |
| Structural schema files | 1 |
| Classification counts | CODEX_ONLY 279, GAMEPLAY_CONSUMED 162, UNRESOLVED 73, OPTIONAL 24 |

## 7. Expanded surface: authority contract

| Rule | Detail |
|---|---|
| Single authority | JSON is authoritative; no duplicated mutable state in code |
| Consumers | every catalog has a named loader/consumer (Plan 22) |
| Versioning | `schema_version` consistent; bumps documented |
| Structure | snake_case ids; schema where the catalog class warrants (Plan 90) |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Census delta | file/version counts compared per change |
| Consumer census | Plan 22's classification regenerated |
| Schema coverage | Plan 90's first-wave list |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section).
2. Consumer/schema coverage triage.
3. Version-field normalization.
4. Regression: census + classification regenerated.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Catalog | consumer named; version field consistent |
| Classification | current; no unexplained UNRESOLVED |
| Schema | present where required; drift gate green |
| Code | no duplicated mutable authority |

**Non-goals unchanged:** this expansion adds census and verification detail.

---

## 12. Cross-plan coupling

This plan governs artifacts rather than a `.cs` domain; the domain set is the
plan's own backticked artifact list (18 files). Other plans referencing
those artifacts: **53**.

**Incoming plan edges (top 8):**

| Plan | Artifact mentions |
|---|---:|
| `PLAN-NARRATIVE-GRAPH-18` | 3 |
| `PLAN-LAUNCH-FACE-06` | 2 |
| `PLAN-RELEASE-OPS-20` | 2 |
| `PLAN-DATA-CONSUMER-22` | 2 |
| `PLAN-BELIEF-IDEOLOGY-36` | 2 |
| `PLAN-SIGNALS-REMOTE-SENSING-49` | 2 |
| `PLAN-COMBAT-DEPTH-62` | 2 |
| `PLAN-AUDIO-MIX-AUTHORITY-97` | 2 |

**Artifacts (first 12):**

| Artifact |
|---|
| `PLAN-DATA-AUTHORITY-14_APPENDIX-A_CATALOG_CLASSIFICATION.md` |
| `acoustic_triangulation_catalog.json` |
| `anomalous_expedition_encounters.json` |
| `armored_crawler_modules.json` |
| `artifacts/content-utilization-baseline.json` |
| `artifacts/content-utilization-deep-chain.json` |
| `artifacts/snake-case-survey.json` |
| `audio_cues.json` |
| `ballistic_shield_catalog.json` |
| `belief_movements.json` |
| `docs/ci/CI_GATE_MANIFEST.json` |
| `docs/data/CATALOG_REGISTRY.md` |

**Package → candidate artifacts (heuristic by name overlap):**

| Package | Candidate artifacts |
|---|---|
| `DA-14A` | no name match — resolve at claim time |
| `DA-14B` | no name match — resolve at claim time |
| `DA-14C` | `docs/data/CATALOG_REGISTRY.md`, `generate-catalog-registry.py` |
| `DA-14D` | no name match — resolve at claim time |
| `DA-14E` | no name match — resolve at claim time |
| `DA-14F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; artifacts are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 18. Host files: **155** · Test files: **267** · Data files: **263**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 155 | `src/Audio/AudioCueCatalog.cs`, `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioManager.cs`, `src/Dose/DoseRegisterSurface.cs`, `src/Economy/TradeScreenGodotPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 267 | `Ashfall.Core.Tests/ArchitectureHardeningCrossPlanIntegrationTests.cs`, `Ashfall.Core.Tests/ArchiveInksCatalogTests.cs`, `Ashfall.Core.Tests/Audio/Plan67CassetteSetsTests.cs`, `Ashfall.Core.Tests/AudioEventIntegrationTests.cs`, `Ashfall.Core.Tests/BalancePowerEconomyTests.cs` |
| Data (`StreamingAssets/Data/`) | 263 | `Assets/StreamingAssets/Data/agriculture_items.json`, `Assets/StreamingAssets/Data/antigravity_survivor_fields.json`, `Assets/StreamingAssets/Data/audio_logs_expansion_05.json`, `Assets/StreamingAssets/Data/black_flotilla_items.json`, `Assets/StreamingAssets/Data/cassette_sets.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **35** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `agriculture` |
| `archive_desk` |
| `armored_crawlers` |
| `ballistic_shield` |
| `black_market` |
| `black_projects_archive` |
| `caravan_trade_network` |
| `deep_well` |
| `dose_ledger` |
| `dynamic_quests` |
| `economy` |
| `encounter_choice` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **30** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--agriculture-selftest` |
| `--audio-selftest` |
| `--audio-test` |
| `--black-flotilla-selftest` |
| `--deep-coast-host-selftest` |
| `--deep-coast-playthrough` |
| `--deep-coast-route-selftest` |
| `--deep-coast-selftest` |
| `--dose-ledger-selftest` |
| `--dose-uitest` |
| `--economy-selftest` |
| `--economy-uitest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **26**.

| Event | First declaration |
|---|---|
| `MoralChoiceSystem` | `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs` |
| `OnArchiveChanged` | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` |
| `OnBaselineCorrected` | `Assets/Ashfall.Core/CohortSystem.cs` |
| `OnCaseCompleted` | `Assets/Ashfall.Core/AutopsySystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnDoseChanged` | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` |
| `OnDoseCorrected` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnEconomyChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnEventRaised` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnExpeditionCompleted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionFailed` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionStarted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json` |
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` |
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/archive_categories.json` |
| `Assets/StreamingAssets/Data/archive_inks.json` |
| `Assets/StreamingAssets/Data/armored_crawler_modules.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/autopsy_procedures.json` |
| `Assets/StreamingAssets/Data/ballistic_shield_catalog.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **13** (198 files, 1542 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Combat` | 10 | 84 |
| `Economy` | 41 | 329 |
| `Events` | 1 | 6 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `MoralChoice` | 4 | 33 |
| `Performance` | 10 | 47 |

**Verdict:** 1542 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Balance`, `Combat`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **325**
(230 of them panels/HUD).

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
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Dose/DoseRegisterSurface.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **56**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `archive_desk` | no |
| `armored_crawlers` | no |
| `autopsy` | no |
| `ballistic_shield` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `caravan_trade_network` | no |
| `combat` | no |
| `deep_well` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **17**.

| Stream |
|---|
| `acoustic_detection` |
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `combat` |
| `cupola_foundry` |
| `deep_coast` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **178**
(CODEX_ONLY 82, GAMEPLAY_CONSUMED 58, OPTIONAL 12, UNRESOLVED 26).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `armored_crawler_modules.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `belief_movements.json` | UNRESOLVED |

**Verdict:** 26 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_expelled_survivor` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** governance · **Coupling (incoming plans):** 0
**Surface:** save sections 56 (laddered 1) · RNG streams 17 · host files 24 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DATA-AUTHORITY-14
wave: —
status: PROPOSED — foreman claim required
packages: DA-14A, DA-14B, DA-14C, DA-14D, DA-14E, DA-14F
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --agriculture-selftest
dependencies:
  - touches 1 versioned save ladder(s) — extend, never fork
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
