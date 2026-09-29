# ASHFALL — GENERATION WAVE 9 — MASTER PLAN PART 1 — CLOSEOUT REPORT
// SPDX-License-Identifier: MIT

> **Authority:** Generation Wave 9 Part 1 Master Plan
> **Date:** 2026-09-17
> **Status:** EXECUTION COMPLETE — ALL TASKS CLOSED OR FORMALLY DEFERRED PER ACTIVE CLAIMS

---

## 1. Executive Summary & Package Status

| Task | Domain | Target | Disposition | Evidence |
|---|---|---|---|---|
| **A1** | Campaign / Weather | C1.4 Crisis Prediction + Real Coordination Gate | **DONE / CONSUMER INTEGRATED** | Pure Core `CrisisPredictionModel.cs`, wired into `WeatherIntelligenceCoordinator.cs` (`BuildReadModel`) and `WeatherForecastPanel.cs`. Briefing surface integrated 2026-09-17 via additive `DailyBriefingReportBuilder.AppendCrisisWarnings` + `src/Main.BriefingCrisis.cs` (Canonical read-only inputs → `CrisisPredictor.Evaluate`). `CrisisPredictionTests` PASS (16/16); `DailyBriefingCrisisTests` PASS (8/8). |
| **A2** | World / Weather | C1.5 Cloud Seeding Runtime Consumer | **DONE** | Pure Core `CloudSeedingSystem.cs` (inventory check, weather dispatch, consumption, zero wall-clock). Wired to coordinator and forecast panel. `CloudSeedingSystemTests` PASS (10/10). |
| **A3** | Shelter / Trapping | C1.6 Trophies + C1.7/C1.8 Kennel Completion | **DONE** | Pure Core `TrophySystem.cs` with authoritative `trophies.json` catalog, exactly-once ledger, save/restore, `ShelterDecorSystem` moral modifiers, `WildlifeTrappingSystem` drop integration, and `ShelterDecorPanel` UI affordances. Plan 174 `CompanionAnimalSystem` verified already complete. `TrophyPipelineTests` PASS (17/17), `Plan12CDecorTests` PASS (21/21). |
| **A4** | Economy / Data | C1.9 Hardening + C1.10 Content Tranche / C1 Chain Close | **C1 CORE/MECHANISMS CLOSED — SURFACE DEFERRED** | Authored full +30% content tranche (§22): 4 new embargo rules (10→14), 6 new regional price entries (18→24, coastal region active), 1 bulk silver iodide recipe, 3 expansion trophies (8→11), 2 companion animal species, 2 companion rescue events, 4 radio broadcasts. `TradeEmbargoSystemTests` PASS (20/20), `RegionalPriceAtlasTests` PASS (16/16), `--data-integrity-selftest` PASS (333 catalogs, 0 errors). Water sample quirk documented in `docs/decisions/WATER_SAMPLE_CONTAMINATED_DECISION_MEMO.md`. |
| **B1** | Campaign / Events | Plan 31 Semantic-Kind Authority | **SEMANTIC AUTHORITY COMPLETE — CONSUMER WIRING BLOCKED ON CONTRACT DECISION** | Pure Core `SemanticKind.cs` enum (10 distinct domain categories + Unknown) and total mapping in `DayEventVocabulary.cs` covering all 110 registered day events. `DayEventSemanticKindTests` PASS (37/37), `DayEventVocabularyTests` PASS (8/8), `DayEventParitySourceGateTests` PASS (2/2). The Plan 24 claim is now released (all tasks DONE); the briefing consumer is not wired because the C2/Plan 17 no-silent-drop contract pins `GenericSectionTitle` ("System Activity") as the section for every unhandled kind (`DayEventVocabularyTests`), and B1 §6.12 forbids an unrelated briefing rewrite. Semantic re-grouping therefore needs an explicit contract decision, not an improvisation. |
| **B2** | Audio / Presentation | C2[2] Deferred Delta: Ducking, Acquisition, 17B Matrix | **CLOSED (Scope Map Published)** | 1. Ducking: `VERIFIED-RESOLVED` via `AudioStateCoordinator` snapshot attenuation. 2. Acquisition: `SWEPT / FAILURE PATHS SEALED` via `OneShotDroppedCount` telemetry and missing cue anti-spam dedupe. 3. 17B Matrix: `MATRIX COMPLETE` via `Plan17BRouteVisibilityMatrixTests` (8/8). `--audio-selftest` PASS (630/630). |

---

## 2. Invariant & Governance Compliance

1. **Godot Authority & Zero Unity:** No Unity APIs or dependencies referenced. Godot 4.7.1 headless runner authoritative.
2. **Engine-Free Core:** `Assets/Ashfall.Core/` contains pure domain logic (`netstandard2.1`) without engine references.
3. **Data Authority:** Snake_case JSON files in `Assets/StreamingAssets/Data/` remain authoritative (`trophies.json`, `trade_embargoes.json`, `regional_prices.json`, `recipes.json`, `items.json`, `companion_animals.json`, `events.json`, `radio.json`).
4. **Deterministic Behavior & Replay:** Pure deterministic RNG via `ISeededRng` in `CloudSeedingSystem` and `TrophySystem`. Zero `System.Random` in Core.
5. **One Authority per Concern:** Extended existing systems (`WeatherIntelligenceCoordinator`, `ShelterDecorSystem`, `WildlifeTrappingSystem`, `DayEventVocabulary`, `AudioManager`). No duplicate ledgers or registries created.
6. **Worktree & Claim Protection:** Honored active claim `claim-c1-plan24-survivor-ledger-2026-09-16` during Part 1 execution; never edited claimed paths `Assets/Ashfall.Core/Campaign/DailyBriefingReportBuilder.cs` or `src/Main.CampaignOwners.cs`. **Addendum 2026-09-17 (post-closeout):** Plan 24 tasks are all DONE, so the deferred A1 consumer was integrated as an additive handoff — `DailyBriefingReportBuilder` gained only a new `AppendCrisisWarnings` method (existing methods untouched) plus new files `src/Main.BriefingCrisis.cs` / `DailyBriefingCrisisTests.cs`, and one call added to `src/Main.Campaign.cs`. Recorded under `claim-c1-briefing-crisis-consumer-2026-09-17`.
7. **No Content Corruption:** Preserved `water_sample_contaminated` in `items.json` per §5.15, accompanied by formal decision memo `docs/decisions/WATER_SAMPLE_CONTAMINATED_DECISION_MEMO.md`.

---

## 3. Verification Evidence Table

| Gate / Command | Result | Cases / Details |
|---|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/CrisisPredictionTests.cs` | **PASS** | 16/16 passed (0 failures, 71 ms) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/World/CloudSeedingSystemTests.cs` | **PASS** | 10/10 passed (0 failures, 58 ms) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/TrophyPipelineTests.cs` | **PASS** | 17/17 passed (0 failures, 74 ms) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Plan12CDecorTests.cs` | **PASS** | 21/21 passed (0 failures, 18 ms) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` | **PASS** | 16/16 passed (0 failures, 62 ms) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/TradeEmbargoSystemTests.cs` | **PASS** | 20/20 passed (0 failures, 118 ms) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/RegionalPriceAtlasTests.cs` | **PASS** | 16/16 passed (0 failures, 96 ms) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/DayEventSemanticKindTests.cs` | **PASS** | 37/37 passed (0 failures, 158 ms) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/DayEventVocabularyTests.cs` | **PASS** | 8/8 passed (0 failures, 15 ms) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/DayEventParitySourceGateTests.cs` | **PASS** | 2/2 passed (0 failures, 203 ms) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/UI/Plan17BRouteVisibilityMatrixTests.cs` | **PASS** | 8/8 passed (0 failures, 21 ms) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/UI/PanelRouteGateTests.cs` | **PASS** | 20/20 passed (0 failures, 129 ms) |
| `godot --headless --path . -- --audio-selftest` | **PASS** | 630 passed, 0 failed (cues=196, assets=99) |
| `godot --headless --path . -- --data-integrity-selftest` | **PASS** | 333 catalogs validated, 0 errors, 5 warnings |
| `godot --headless --path . -- --bridge-selftest` | **PASS** | 0 shim errors, UnityEngine clean |
| `dotnet build Ashfall.csproj` | **PASS** | 0 errors, 0 warnings |

---

## 4. Inventory of Modified & Created Files

### Core Domain (`Assets/Ashfall.Core/`)
- `Campaign/CrisisPredictionModel.cs` (new)
- `Campaign/SemanticKind.cs` (new)
- `Campaign/DayEventVocabulary.cs` (extended with 110-event total semantic mapping)
- `Inventory/ItemCatalogLoader.cs` (added "decor" -> `ItemType.Comfort` mapping)
- `Shelter/ShelterDecorSystem.cs` (extended with trophy slots & morale calculations)
- `Shelter/TrophySystem.cs` (new trophy system, catalog, ledger, and save/restore)
- `WildlifeTrappingSystem.cs` (extended with trophy recipe mapping and `OnTrophyReady` event)
- `World/CloudSeedingSystem.cs` (new cloud seeding runtime consumer)
- `World/WeatherIntelligenceCoordinator.cs` (extended with crisis prediction read models and seeding dispatch)

### Godot Host & UI (`src/`)
- `Audio/AudioManager.cs` (added `OneShotDroppedCount`, `StateCoordinator` exposure, pool telemetry)
- `Audio/AudioSelfTest.cs` (added Phase B2 ducking & acquisition tests)
- `Host/ShelterDecorHostSession.cs` (added trophy category mapping)
- `UI/ShelterDecorPanel.cs` (added trophy display slots and crafting state tooltips)
- `UI/WeatherForecastPanel.cs` (added crisis prediction display and seeding affordances)

### Authoritative Game Data (`Assets/StreamingAssets/Data/`)
- `companion_animals.json` (+2 species profiles: `species_rad_dog`, `species_iron_crow`)
- `events.json` (+2 companion rescue events: `event_drowning_pup_rescue`, `event_expedition_stray_follows_home`)
- `items.json` (+11 trophy item definitions)
- `radio.json` (+4 flavor broadcasts: 2 embargo alerts, 2 kennel milestones)
- `recipes.json` (+1 bulk silver iodide recipe, +11 trophy crafting recipes)
- `regional_prices.json` (+6 entries, activating coastal region)
- `trade_embargoes.json` (+4 weather embargo rules: Ashfall, BloodRain, ThermalInversion, ParticulateFog)
- `trophies.json` (new data authority, 11 trophy species definitions)

### Tests (`Ashfall.Core.Tests/`)
- `Campaign/CrisisPredictionTests.cs` (new, 16 tests)
- `Campaign/DayEventSemanticKindTests.cs` (new, 37 tests)
- `Economy/RegionalPriceAtlasTests.cs` (updated assertions for 24 rows)
- `Economy/TradeEmbargoSystemTests.cs` (updated assertions for 14 rules)
- `Plan12CDecorTests.cs` (updated assertion to allow expanded decor items)
- `Shelter/TrophyPipelineTests.cs` (new, 17 tests)
- `UI/Plan17BRouteVisibilityMatrixTests.cs` (new, 8 tests)
- `World/CloudSeedingSystemTests.cs` (new, 10 tests)

### Documentation & Decisions (`docs/`)
- `A1_BRIEFING_DEFERRED.md` (new)
- `A1_COORDINATION_RECORD.md` (new)
- `C1_COMPLETION.md` (updated with C1.4–C1.10 closeout status)
- `docs/audio/C2_DEFERRED_DELTA_SCOPE_MAP.md` (new)
- `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md` (updated with `duty_vacated`, `medical_admitted`, `medical_discharged`)
- `docs/decisions/WATER_SAMPLE_CONTAMINATED_DECISION_MEMO.md` (new)
- `docs/plans/C2_PLANINTEGRATION_2_CLOSURE_REPORT.md` (updated Plan 31 B1 status)
