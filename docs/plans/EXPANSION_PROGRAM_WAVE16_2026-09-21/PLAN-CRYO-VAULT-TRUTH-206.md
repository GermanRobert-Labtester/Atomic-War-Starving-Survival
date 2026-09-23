# PLAN-CRYO-VAULT-TRUTH-206 — Cold Storage, Power Dependency & Sample Integrity

**Wave 16 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-PRESERVATION-TRUTH-118, PLAN-ENERGY-NUCLEAR-48, PLAN-HEIRLOOM-PHANTOM-TRUTH-149.
**Non-goals:** no spoilage curves (Plan 118), no grid model (Plan 48), no
genetics (Plan 81).

## 1. Outcome
`Shelter/CryoVaultSystem.cs` (**554 lines**) is reachable and unaddressed: a
powered vault preserving biological samples. Preservation (Plan 118) covers
food; this is the **long-term biological** store whose integrity depends on the
grid — and whose failure is silent unless stated.

| Deliverable | Detail |
|---|---|
| Vault model | slots with contents (samples by class), temperature band, and a power draw from Plan 48 |
| Integrity rule | temperature excursion degrades contents per a documented curve; degradation is visible before loss |
| Power coupling | a brownout draws on the grid; a backup/power path is an explicit build, not an assumption |
| Access rules | who may withdraw a sample by role (Plan 141); withdrawal is recorded |
| Save truth | contents and temperature state restore; a load never re-rolls degradation |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/CryoVaultSystem.cs` (554 lines; unaddressed — Wave 16 audit).
- Plan 118 owns preservation of consumables; sample preservation is the sibling with a power dependency.
- Plan 149's heirlooms and Plan 81's hereditary content may reference samples — boundaries stated.
- Plan 48 owns grid balance the draw enters.

## 3. Packages
- **CVT-206A** vault model + contents/slot table.
- **CVT-206B** integrity curve + excursion fixtures.
- **CVT-206C** power draw in Plan 48's balance + backup build path.
- **CVT-206D** access/withdrawal tests per role.
- **CVT-206E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- An excursion degrades exactly per curve and warns before loss.
- Draw appears in grid balance; no backup means loss on outage.
- Save/load preserves contents and temperature state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Silent sample loss → warning before loss and a record of the excursion.
Power assumption → the backup path is an explicit build with a cost.

---

## 6. Expanded census (1 files · 554 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CryoVaultSystem.cs` | 554 | System | **yes** | 0 | 0 | 3 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `cryo_cultivars.json` | object[4 keys] |
| `cryogenic_air_separation.json` | object[3 keys] |
| `architect_vault_audits.json` | array[7] |
| `cryo_germplasm_viability_audits.json` | array[8] |
| `cryo_seed_ampoule_logs.json` | array[8] |
| `cryopod_failure_logs.json` | array[8] |

**State surfaces:** `CryoVaultSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 3 name references across the test tree |
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

Domain files: 1. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-PRESERVATION-TRUTH-118` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CVT-206A` | `CryoVaultSystem.cs` |
| `CVT-206B` | no name match — resolve at claim time |
| `CVT-206C` | no name match — resolve at claim time |
| `CVT-206D` | no name match — resolve at claim time |
| `CVT-206E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 7. Host files: **3** · Test files: **5** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/CryogenicAirSeparationHostSession.cs`, `src/Main.Plans94_97.cs`, `src/Main.PlansB68_B69.cs` |
| Tests (`Ashfall.Core.Tests/`) | 5 | `Ashfall.Core.Tests/AbyssalAnomaliesRuntimeActivationTests.cs`, `Ashfall.Core.Tests/Integration/PlansB66ToB69CrossSystemTests.cs`, `Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `Ashfall.Core.Tests/Shelter/CryoVaultB69Tests.cs`, `Ashfall.Core.Tests/Shelter/PowerGridCatalogTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **9** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `cryo_vault` |
| `cryogenic_air_separation` |
| `narrative` |
| `narrative_questlines` |
| `power_grid` |
| `power_subgrids` |
| `procedural_narrative` |
| `sofc_power` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **13** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--narrative-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--real-main-journey-selftest` |
| `--save-load-failure-selftest` |
| `--save-load-failure-uitest` |
| `--save-load-ui-failure-selftest` |
| `--save-store-checksum-selftest` |
| `--save-store-checksums-selftest` |
| `--selftest-manifest` |
| `--sofc-power-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **6**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnDrillFailure` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnPumpFailure` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnStageNarrativeEmitted` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/anomalies.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/cryo_cultivars.json` |
| `Assets/StreamingAssets/Data/cryogenic_air_separation.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (112 files, 919 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Economy` | 41 | 329 |
| `Factions` | 10 | 72 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |
| `Quests` | 4 | 25 |

**Verdict:** 919 cases sit under matching regions — run those first (`Audio`, `Balance`, `Economy`, `Factions`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **494**
(22 of them panels/HUD).

| Host file |
|---|
| `src/Disease/DiseaseHostSession.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/AmputationSaveStore.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ApprenticeshipSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **35**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `amputation` | no |
| `anomaly_hazard` | no |
| `apprenticeship` | no |
| `collectible_discovery` | no |
| `crossing` | no |
| `cryo_vault` | no |
| `cryogenic_air_separation` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **14**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `anomaly_hazard` |
| `aquaponics_disease` |
| `cupola_foundry` |
| `deep_coast` |
| `disease` |
| `foundry` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **351**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 51, OPTIONAL 6, UNRESOLVED 15).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |

**Verdict:** 15 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_become_warlord` |
| `flag_expelled_survivor` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 35 (laddered 0) · RNG streams 14 · host files 24 · catalogs 22 · test regions 9 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CRYO-VAULT-TRUTH-206
wave: 16
status: PROPOSED — foreman claim required
packages: CVT-206A, CVT-206B, CVT-206C, CVT-206D, CVT-206E
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/anomalies.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --narrative-selftest
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
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
