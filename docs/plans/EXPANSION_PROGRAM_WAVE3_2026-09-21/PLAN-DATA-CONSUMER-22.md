# PLAN-DATA-CONSUMER-22 — Field-Level & Row-Level Consumption Sealing

**Wave:** 3 (2026-09-21) · **Kind:** GAP SEALING
**Status:** PROPOSED — not a claim. Foreman claim required.
**Depends on:** PLAN-DATA-AUTHORITY-14 (catalog lifecycle), PLAN-INTEGRATION-KIT-02.
**Non-goals:** no catalog rewrite, no field deletion without a consumer check,
no new authoring format.

---

## 1. Outcome

`--content-utilization-selftest` audits **catalogs**; it cannot see that a
catalog is loaded every day while half its fields are never read. The ledger
already records three field-level bugs of this exact class:

- `passive_decay_bonus_permille` authored but ignored; a hardcoded `-200`
  blower value shipped instead (sealed 2026-09-20);
- `water_sample_contaminated` declared equipable with no equip path
  (`DEC-09`, sealed by making it `isEquipable: false`);
- `difficultyPresetId` stamped into completion history only after the chronicle
  authority seal (`DEBT-PLAN34-…`).

This plan extends consumption truth from the catalog level to the **field and
row level**.

Deliverables:

1. a generated **field-read inventory**: every JSON-bound DTO property, its
   readers, and its producer;
2. triage of unread fields: wire a consumer, document as authoring-only, or
   retire;
3. a **row-reachability audit**: catalog rows that no gameplay path can select
   (unreachable prerequisites, dead gates, orphan foreign keys);
4. a **gate** so new fields cannot ship without a reader or an explicit
   authoring tag.

---

## 2. Evidence

| Fact | Value | Source |
|---|---:|---|
| Data catalogs | 703 | audit 2026-09-21 |
| Content-utilization classes | catalog-level only (`GAMEPLAY_CONSUMED`/`OPTIONAL`/`UNRESOLVED`) | `artifacts/content-utilization-baseline.json` |
| Field-level defects already sealed | 3 named cases | INTEGRATION_PLANS / `KNOWN_DEBT` |
| DTO convention | snake_case public fields + `JsonPropertyName`, loader-populated | loaders (`*CatalogLoader.cs`) |
| Catalog registry | ~605 rows | `docs/data/CATALOG_REGISTRY.md` |
| Data-integrity gate | shape/schema/reference checks | `--data-integrity-selftest` |
| Vehicle armor grades | catalog exists, host-pending (CF-P6) | Wave 1 program |
| Difficulty scalars | partial binding (XP-01) | `docs/plans/CF_XP01_…` |

**Method (proposed):** for each catalog DTO, extract public members; search
Core+src for reads (`x.Field`, `entry.Field`, destructuring) excluding the DTO
declaration and the loader assignment; emit a report with file:line for every
reader. Fields with zero readers are candidates. This is static, fast, and can
run as a gate.

---

## 3. Packages

### DC-22A — Field-read inventory
- Generate `docs/data/FIELD_CONSUMPTION.md`:
  `catalog | DTO type | field | readers (file:line) | producer | status`.
- Status: `CONSUMED`, `AUTHORING_ONLY` (tagged in code), `UNREAD`,
  `WRITE_ONLY` (deserialized only).
- **Acceptance:** report covers every `JsonPropertyName`-bearing DTO in
  `Assets/Ashfall.Core` and `src/Host`; deterministic output.
- **Verify:** `python3 scripts/ci/generate-field-consumption.py --check`.

### DC-22B — Triage the unread set
- For each `UNREAD`/`WRITE_ONLY` field: wire the canonical consumer (the
  hardcoded-value precedent applies: if a constant exists where the field
  belongs, replace the constant), mark `AUTHORING_ONLY` with a doc comment,
  or delete the field and its JSON rows.
- **Acceptance:** zero unclassified fields; each wiring carries a focused test
  proving the authored value moves the outcome.
- **Verify:** focused suites per catalog family (economy, medical, weather,
  survivors, world).

### DC-22C — Row-reachability audit
- For each catalog with selection logic, prove every row is selectable:
  prerequisites resolvable, gate flags reachable, foreign keys exist, `min/max`
  day windows overlap the campaign, weights > 0, no row shadowed by an earlier
  match.
- Output: `docs/data/ROW_REACHABILITY.md` with the unreachable rows and the
  reason (`prerequisite_cycle`, `gate_never_true`, `window_never_open`,
  `shadowed`, `fk_missing`).
- **Acceptance:** zero `fk_missing`; other classes triaged (open the window,
  fix the gate, delete the row, or document why unreachable is intentional).
- **Verify:** extended data-integrity rules + per-family focused tests.

### DC-22D — Gates
- `generate-field-consumption.py --check` fails on new unread fields without a
  tag; the row-reachability report is regenerated in the data CI job.
- **Acceptance:** a field added with no reader fails with the field name and
  catalog; an unreachable row fails with its id and reason.
- **Verify:** `agent-fast-verify.py`.

### DC-22E — Named follow-through
- Land the three live examples as the pilot:
  `difficultyPresetId` completion (XP-01), vehicle armor grades (CF-P6), and
  one dose-ledger field audited by the new report.
- **Acceptance:** pilot proves the report catches a real gap before the fix
  and clears after.

---

## 4. Risk register

| Risk | Mitigation |
|---|---|
| Static readers miss reflection/serialization use | DTOs are loaded by explicit loaders; the report lists producer and all textual readers; allowlist with reason |
| Deletion of a field breaks legacy saves | fields are additive; deletion only after a save-compat check |
| Row audit flags intentionally unreachable content | status allows `intentional` with a reason and owner |
| Report too large to read | per-family sections + counts; failures printed first |

## 5. Verification

```bash
python3 scripts/ci/generate-field-consumption.py --check
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/
bash scripts/run_test.sh Ashfall.Core.Tests/World/
```

---

## 6. Expanded census (bespoke: catalog consumption baseline)

This plan closes consumer gaps, so the census reads
`artifacts/content-utilization-baseline.json` directly. (Corrected: the first
pass guessed artifact keys; the real keys are `catalogClassifications`,
`knownOrphans`, `exemptedCatalogs`.)

| Metric | Value |
|---|---:|
| Classified catalogs | 538 |
| Known orphans | 11 |
| Exempted catalogs | 12 |
| Generated from commit | `unset` |

**Classification counts:**

| Class | Catalogs |
|---|---:|
| CODEX_ONLY | 279 |
| GAMEPLAY_CONSUMED | 162 |
| UNRESOLVED | 73 |
| OPTIONAL | 24 |

**Known orphans (first 15):** `audio_logs_expansion_05.json`, `cassette_sets.json`, `confession_secrets.json`, `final_wishes.json`, `guilt_sources.json`, `item_degradation.json`, `memorials_expansion_05.json`, `narrative_encounters_expansion.json`, `phantom_heirlooms.json`, `trade_screen_scenarios.json`, `wall_carving_templates.json`

## 7. Expanded surface: consumer contract

| Rule | Detail |
|---|---|
| Reachability | a catalog is consumed when a host path reads it and a player can see the effect |
| Codex-only | a legitimate class only when a codex surface renders it (Plan 110) |
| Unresolved | triaged: wire, reclassify, or retire |
| No silent default | a missing consumer must not fall back silently |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Baseline regeneration | re-run the utilization artifact; deltas reported |
| Consumer proof | one fixture per newly wired catalog |
| Unresolved triage | each row has a decision recorded |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Freeze the baseline counts (this section).
2. Triage UNRESOLVED rows and known orphans.
3. Wire or reclassify top consumers.
4. Regression: baseline delta per batch.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Catalog | classification current; no unexplained UNRESOLVED |
| Consumer | host path + visible effect proven |
| Codex-only | rendered by a codex surface |
| Baseline | regenerates without unexplained drift |

**Non-goals unchanged:** this expansion adds census and verification detail.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 16. Other plans referencing them: **25**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-DATA-AUTHORITY-14` | 2 |
| `PLAN-NARRATIVE-GRAPH-18` | 2 |
| `PLAN-SHELTER-ARCHITECTURE-40` | 2 |
| `PLAN-ANOMALY-PHANTOM-63` | 2 |
| `PLAN-INTEGRATION-KIT-02` | 1 |
| `PLAN-LAUNCH-FACE-06` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-CORE-ONLY-REGISTRY-11` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `agent-fast-verify.py` |
| `artifacts/content-utilization-baseline.json` |
| `audio_logs_expansion_05.json` |
| `cassette_sets.json` |
| `confession_secrets.json` |
| `docs/data/CATALOG_REGISTRY.md` |
| `docs/data/FIELD_CONSUMPTION.md` |
| `docs/data/ROW_REACHABILITY.md` |
| `final_wishes.json` |
| `guilt_sources.json` |
| `item_degradation.json` |
| `memorials_expansion_05.json` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `DC-22A` | `docs/data/FIELD_CONSUMPTION.md` |
| `DC-22B` | no name match — resolve at claim time |
| `DC-22C` | `docs/data/ROW_REACHABILITY.md` |
| `DC-22D` | no name match — resolve at claim time |
| `DC-22E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 16. Host files: **3** · Test files: **12** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/EquipmentConditionHostSession.cs`, `src/Host/Phase0HostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 12 | `Ashfall.Core.Tests/Campaign/Plan47_65ModAgencyIntegrationTests.cs`, `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs`, `Ashfall.Core.Tests/Content/ContentOrphanCertificationEngineTests.cs`, `Ashfall.Core.Tests/Economy/TradeScreenScenarioCatalogTests.cs`, `Ashfall.Core.Tests/Equipment/EquipmentConditionCatalogTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/trade_screen_scenarios.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **11** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caravan_trade_network` |
| `encounters` |
| `equipment` |
| `equipment_condition` |
| `field_guide` |
| `holdfast_trade` |
| `narrative` |
| `narrative_questlines` |
| `phantom_memory` |
| `procedural_narrative` |
| `travel_encounters` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **5** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--audio-selftest` |
| `--audio-test` |
| `--holdfast-trade-save-selftest` |
| `--memorial-wall-selftest` |
| `--narrative-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **24**.

| Event | First declaration |
|---|---|
| `OnBaselineCorrected` | `Assets/Ashfall.Core/CohortSystem.cs` |
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnConditionChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnConditionStarted` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnConditionStopped` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnDeviceConditionChanged` | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` |
| `OnEquipmentChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnEquipmentDamaged` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnFinalWishCompleted` | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` |
| `OnFinalWishFailed` | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` |
| `OnFinalWishStepCompleted` | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` |
| `OnGuiltInsomniaCritical` | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/backstory_templates.json` |
| `Assets/StreamingAssets/Data/breaching_equipment_catalog.json` |
| `Assets/StreamingAssets/Data/caravan_trade_routes.json` |
| `Assets/StreamingAssets/Data/cassette_sets.json` |
| `Assets/StreamingAssets/Data/cohort_tuning.json` |
| `Assets/StreamingAssets/Data/communication_templates.json` |
| `Assets/StreamingAssets/Data/confession_secrets.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **8** (140 files, 1196 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Communication` | 3 | 17 |
| `Equipment` | 1 | 4 |
| `Integration` | 16 | 74 |
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |
| `Shelter` | 87 | 754 |

**Verdict:** 1196 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Communication`, `Equipment`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **209**
(19 of them panels/HUD).

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
| `src/Disease/DiseaseHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **35**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `anomaly_hazard` | no |
| `caravan` | no |
| `caravan_trade_network` | no |
| `communication` | no |
| `disease` | no |
| `encounters` | no |
| `equipment` | no |
| `equipment_condition` | no |
| `expanded_shelter` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **12**.

| Stream |
|---|
| `acoustic_detection` |
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `anomaly_hazard` |
| `aquaponics_disease` |
| `black_market_debt_event` |
| `disease` |
| `expedition` |
| `metrology_calibration_drift` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **330**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 25, OPTIONAL 15, UNRESOLVED 11).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `cassette_sets.json` | OPTIONAL |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `confession_secrets.json` | OPTIONAL |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |

**Verdict:** 11 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** hub · **Coupling (incoming plans):** 25
**Surface:** save sections 35 (laddered 0) · RNG streams 12 · host files 22 · catalogs 22 · test regions 8 · flags 5

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DATA-CONSUMER-22
wave: —
status: PROPOSED — foreman claim required
packages: DC-22A, DC-22B, DC-22C, DC-22D, DC-22E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/anomalous_expedition_encounters.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --audio-selftest
dependencies:
  - coordinate: 25 other plan(s) name these artifacts (§12)
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
