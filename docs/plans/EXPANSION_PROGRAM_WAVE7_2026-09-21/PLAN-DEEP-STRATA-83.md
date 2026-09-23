# PLAN-DEEP-STRATA-83 — Caverns, Mines, Shafts & Subterranean Settlement

**Wave 7 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SHELTER-ARCHITECTURE-40, PLAN-WATER-AGRICULTURE-46,
PLAN-ENERGY-NUCLEAR-48.
**Implementation scaffold:** [`PLAN-DEEP-STRATA-83_APPENDIX-A_SCAFFOLD.md`](PLAN-DEEP-STRATA-83_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-AQUIFER-MONITORING-TRUTH-164` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no 3D cave simulation; `WastelandMapSystem` and the shelter grid
remain canonical.

## Outcome
The underground is authored: `Subterranean/` (systems + save), `SubterraneanSystem`,
`ExcavationSystem`/catalog, `ExcavationHazardSystem` (methane + flood producers
sealed), `SubterraneanSubsidenceEngine` (orphan), `DeepWellSystem`,
`District8DeepCoastSystem` (deep coast), `geothermal_strata_catalog.json`,
`geothermal_drilling_depths.json`, `excavation_sites.json` (8 stratified
sites), `excavation_hazard_mitigation.json`, salt mine, crystalline/cryo vault
familiy. This plan makes depth a **frontier with its own economy and hazards**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Shafts & levels | shelter expansion + excavation | dig, shore, lift | new rooms, ore, risk |
| Strata | strata catalog | survey, choose seam | material yields, hazards |
| Hazards | methane/flood/subsidence owners | ventilate, pump, reinforce | incidents, evacuation |
| Mining | ore/resource work | mine, haul, process | stone, salt, metals |
| Deep water | deep well/aquifer | tap, treat | yield, contamination |
| Geothermal | drilling depths | drill, build plant | heat/power, seismic risk |
| Hidden spaces | deep-lore sites | explore | relics, anomalies, lore |
| Settlement | deep habitat rooms | inhabit, supply | fallback shelter, morale |

## Evidence
- Core: `Subterranean/` + `SubterraneanSystem`, `ExcavationSystem`, `Excavation/ExcavationHazardSystem.cs` (sealed producers/test 14/14), `Excavation/SubterraneanSubsidenceEngine.cs` (orphan, 9 tests), `DeepWellSystem`, `SaltMineExtractionSystem`, `World/GroundPenetratingRadarEngine`.
- Data: `excavation_sites.json`, `excavation_hazard_mitigation.json`, `geothermal_strata_catalog.json`, `geothermal_drilling_depths.json`, `deep_lore_locations.json`.
- Sealed prior: Plan 37 excavation sites (10/10), Plan 56 seismic/methane (6/6), Plan 156 vertical shafts, Plan 189 deep well.
- Contracts: hazards via canonical systems; stability < 40% alert; no parallel map.

## Packages
- **DS-83A** shaft/level model: depth bands, shoring requirements, lift logistics, stability.
- **DS-83B** strata survey+extraction: yields from authored strata; GPR reveals seams.
- **DS-83C** hazard cycle: methane accumulation, flooding, subsidence with mitigation protocols; incidents typed.
- **DS-83D** deep water/geothermal: yield vs hazard trade; plant builds heat/power.
- **DS-83E** hidden sites: deep-lore exploration outcomes (relics, anomalies, records).
- **DS-83F** deep habitat: shelter rooms underground with thermal/air trade-offs (Plan 40 ties).
- **DS-83G** content volumes: +6 strata, +6 sites, +8 hazards, +6 mitigation protocols; abstract/fictional.

## Acceptance & verification
- Depth economics are real (output vs energy/risk); hazard incidents recoverable; determinism.
- `bash scripts/run_test.sh` excavation/seismic suites; `git status` untouched; instrumentation: reachability re-run for `SubterraneanSubsidenceEngine`.

## Risks
Deep mining dominates the economy → energy/water/risk costs and finite seams; depth gated by tech (Plan 38).

---

## 6. Expanded census (12 files · 2,806 lines)

Scope: `Assets/Ashfall.Core/Subterranean/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 2 · DTO/Type 1 · Demo 1 · Loader 2 · Save 1 · System 5

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ExcavationCatalogLoader.cs` | 273 | Loader | — | 0 | 0 | 0 |
| `ExcavationHazardSystem.cs` | 483 | System | — | 0 | 0 | 2 |
| `SubterraneanSubsidenceEngine.cs` | 299 | System | — | 0 | 0 | 0 |
| `GeothermalAquiferHeadlessDemo.cs` | 119 | Demo | — | 0 | 0 | 1 |
| `GeothermalAquiferState.cs` | 27 | DTO/Type | — | 0 | 0 | 0 |
| `GeothermalAquiferSystem.cs` | 303 | System | — | 0 | 0 | 2 |
| `GeothermalCatalog.cs` | 58 | Catalog | — | 0 | 0 | 0 |
| `GeothermalCatalogLoader.cs` | 34 | Loader | — | 0 | 0 | 0 |
| `GeothermalOrcSystem.cs` | 556 | System | — | 0 | 0 | 2 |
| `SubterraneanSave.cs` | 134 | Save | — | 0 | 0 | 0 |
| `SubterraneanSystem.cs` | 440 | System | **yes** | 0 | 0 | 2 |
| `SubterraneanZoneCatalog.cs` | 80 | Catalog | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 5 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `excavation_hazard_mitigation.json` | object[2 keys] |
| `excavation_sites.json` | object[2 keys] |
| `geothermal_drilling_depths.json` | object[2 keys] |
| `geothermal_strata_catalog.json` | object[2 keys] |
| `subterranean_zones.json` | object[3 keys] |
| `geological_strata_logs.json` | object[3 keys] |

**State surfaces:** `ExcavationHazardSystem.cs`, `GeothermalAquiferHeadlessDemo.cs`, `GeothermalAquiferSystem.cs`, `GeothermalOrcSystem.cs`, `SubterraneanSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 26 name references across the test tree |
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

Domain files: 14. Other plans referencing their names: **12**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-GEOTHERMAL-PLANT-TRUTH-191` | 6 |
| `PLAN-GEOTHERMAL-AQUIFER-TRUTH-260` | 6 |
| `PLAN-SHELTER-FAMILY-TRUTH-265` | 5 |
| `PLAN-ENERGY-NUCLEAR-48` | 3 |
| `PLAN-AQUIFER-MONITORING-TRUTH-164` | 3 |
| `PLAN-SHELTER-ARCHITECTURE-40` | 2 |
| `PLAN-SEISMIC-DYNAMICS-TRUTH-193` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `DS-83A` | no name match — resolve at claim time |
| `DS-83B` | no name match — resolve at claim time |
| `DS-83C` | `ExcavationHazardSystem.cs`, `SubterraneanSubsidenceEngine.cs` |
| `DS-83D` | `ExcavationHazardSystem.cs`, `GeothermalAquiferHeadlessDemo.cs`, `GeothermalAquiferState.cs` |
| `DS-83E` | no name match — resolve at claim time |
| `DS-83F` | `GeothermalAquiferHeadlessDemo.cs`, `GeothermalAquiferState.cs`, `GeothermalAquiferSystem.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 12; intra-domain edges: **4**; isolated files:
**7**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `GeothermalAquiferHeadlessDemo` | `GeothermalAquiferSystem` |
| `GeothermalAquiferState` | `GeothermalAquiferSystem` |
| `GeothermalAquiferSystem` | `GeothermalAquiferState` |
| `SubterraneanZoneCatalog` | `SubterraneanSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `GeothermalAquiferSystem` | 2 |
| `GeothermalAquiferState` | 1 |
| `SubterraneanSystem` | 1 |
| `ExcavationCatalogLoader` | 0 |
| `ExcavationHazardSystem` | 0 |
| `GeothermalAquiferHeadlessDemo` | 0 |
| `GeothermalCatalog` | 0 |
| `GeothermalCatalogLoader` | 0 |
| `GeothermalOrcSystem` | 0 |
| `SubterraneanSave` | 0 |

**Class split:** hub 2 · sink 1 · source 2 · isolated 7.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 12. Host files: **12** · Test files: **17** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 12 | `src/Audio/AudioSelfTest.cs`, `src/Audio/ShelterOperationsAudioBridge.cs`, `src/Host/GeothermalAquiferHostSession.cs`, `src/Host/GeothermalAquiferSaveStore.cs`, `src/Host/HostCli.WorldExploration.cs` |
| Tests (`Ashfall.Core.Tests/`) | 17 | `Ashfall.Core.Tests/Balance/ShelterOperationsBalanceSim.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs`, `Ashfall.Core.Tests/Combat/Plan10_11CombatExplorationIntegrationTests.cs`, `Ashfall.Core.Tests/Excavation/SubterraneanSubsidenceEngineTests.cs`, `Ashfall.Core.Tests/Flagship11/SubterraneanSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `anomaly_hazard` |
| `deep_well` |
| `excavation` |
| `excavation_hazards` |
| `geothermal_aquifer` |
| `geothermal_orc` |
| `subterranean` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--deep-coast-host-selftest` |
| `--deep-coast-playthrough` |
| `--deep-coast-route-selftest` |
| `--deep-coast-selftest` |
| `--ice-road-tick-demo` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **3**.

| Event | First declaration |
|---|---|
| `OnExcavationChanged` | `Assets/Ashfall.Core/ExcavationSystem.cs` |
| `OnHazardWarning` | `Assets/Ashfall.Core/VentilationSystem.cs` |
| `OnSmokeZoneChanged` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json` |
| `Assets/StreamingAssets/Data/excavation_sites.json` |
| `Assets/StreamingAssets/Data/geothermal_drilling_depths.json` |
| `Assets/StreamingAssets/Data/geothermal_strata_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/canyon_mudflow_hazard_reports.json` |
| `Assets/StreamingAssets/Data/narrative/deep_lore_texts.json` |
| `Assets/StreamingAssets/Data/narrative/geological_strata_logs.json` |
| `Assets/StreamingAssets/Data/narrative/geothermal_borehole_logs.json` |
| `Assets/StreamingAssets/Data/narrative/geothermal_steam_vent_diagnostics.json` |
| `Assets/StreamingAssets/Data/narrative/geothermal_steam_well_logs.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (1 files, 5 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Excavation` | 1 | 5 |

**Verdict:** 5 cases sit under matching regions — run those first (`Excavation`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **29**
(10 of them panels/HUD).

| Host file |
|---|
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/DeepCoastHostSession.cs` |
| `src/Host/DeepWellHostSession.cs` |
| `src/Host/DeepWellSaveStore.cs` |
| `src/Host/ExcavationHazardSaveStore.cs` |
| `src/Host/ExcavationHostSession.cs` |
| `src/Host/ExcavationSaveStore.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/GeothermalAquiferHostSession.cs` |
| `src/Host/GeothermalAquiferSaveStore.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **7**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `anomaly_hazard` | no |
| `deep_well` | no |
| `excavation` | no |
| `excavation_hazards` | no |
| `geothermal_aquifer` | no |
| `geothermal_orc` | no |
| `subterranean` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `anomaly_hazard` |
| `deep_coast` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **12**
(CODEX_ONLY 6, GAMEPLAY_CONSUMED 2, OPTIONAL 1, UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `excavation_hazard_mitigation.json` | GAMEPLAY_CONSUMED |
| `excavation_sites.json` | UNRESOLVED |
| `geothermal_drilling_depths.json` | UNRESOLVED |
| `narrative/canyon_mudflow_hazard_reports.json` | CODEX_ONLY |
| `narrative/deep_lore_texts.json` | CODEX_ONLY |
| `narrative/geological_strata_logs.json` | CODEX_ONLY |
| `narrative/geothermal_borehole_logs.json` | CODEX_ONLY |
| `narrative/geothermal_steam_vent_diagnostics.json` | CODEX_ONLY |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 12
**Surface:** save sections 7 (laddered 0) · RNG streams 2 · host files 14 · catalogs 22 · test regions 1 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DEEP-STRATA-83
wave: 7
status: PROPOSED — foreman claim required
packages: DS-83A, DS-83B, DS-83C, DS-83D, DS-83E, DS-83F, DS-83G
claim paths:
  - src/Host/AnomalyHazardSaveStore.cs  # §19 candidate host surface
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/DeepCoastHostSession.cs  # §19 candidate host surface
  - src/Host/DeepWellHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/deep_lore_locations.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/deep_lore_survivor_fields.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Excavation/
  - godot --headless --path . -- --deep-coast-host-selftest
dependencies:
  - coordinate: 12 other plan(s) name these artifacts (§12)
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
