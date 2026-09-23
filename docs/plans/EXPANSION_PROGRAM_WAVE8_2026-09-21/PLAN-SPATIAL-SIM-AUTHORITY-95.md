# PLAN-SPATIAL-SIM-AUTHORITY-95 — Place, Territory & Route Authority Map

**Wave 8 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-TEMPORAL-AUTHORITY-33, PLAN-TRANSPORT-EXPEDITION-30, PLAN-REFERENCE-INTEGRITY-34.
**Implementation scaffold:** [`PLAN-SPATIAL-SIM-AUTHORITY-95_APPENDIX-A_SCAFFOLD.md`](PLAN-SPATIAL-SIM-AUTHORITY-95_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-WAYSTATION-NETWORK-TRUTH-153` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new map format, no second coordinate system, no renderer work.

## 1. Outcome
`Assets/Ashfall.Core/World/` owns a dense cluster of place-shaped authorities:
`RouteRegionTopology`, `PatrolTerritoryAuthority`, `FactionTerritoryCatalog`,
`SettlementCatalog`, `LivingMapRouteProjection`, `MapRouteHazardEvaluator`,
`DamagedMapSystem`, `RouteInfrastructureSystem`, `RouteGateContextResolver`,
`DebtRouteAccessResolver`, `RouteAvailability*`, plus five host-unreachable
engines (`ModalTravelDispatchEngine`, `NightWatchPatrolReadinessEngine`,
`StormForecastReadinessEngine`, `WeatherForecastReliabilityEngine`,
`WildlifeHarvestQuotaEngine`). Nothing states which of these owns a given
spatial fact — position, region membership, route availability, territory
claim — and nothing tests that placement is seed-stable.

| Deliverable | Detail |
|---|---|
| Authority map | one row per spatial fact (site coords, region membership, route edges, territory claim, hazard cell) naming its single owner |
| Grid/cell semantics | what a map cell means, which authority mutates it, how the save restores it (section named) |
| Placement determinism | same seed → same site/route/territory placement across two runs, checksummed |
| Orphan linkage | the five `World/` orphan engines each point at their consumer in this map (Plan 1 Appendix A wiring) |
| Read-model rule | `LivingMapRouteProjection`/`DamagedMapSystem` are presentations of authority state, never inputs to simulation |

## 2. Evidence
- `Assets/Ashfall.Core/World/` file list (this plan's premise; the package re-verifies each file's role).
- Plan 1 Appendix A: the five `World/` engines are host-unreachable.
- Plan 34 Appendix A: `loc_` 117 ids across 59 files — the reference space for place ids.
- `SaveSectionRegistry.All`: world/settlement/route-related sections (e.g. `expansion_hub`, `deep_well`, `holdfast`) show persistence already exists per system; this plan does not add a store.

## 3. Packages
- **SPA-95A** authority map doc + row-per-fact table (verified against source).
- **SPA-95B** grid/cell semantics doc with the owning save section per mutable field.
- **SPA-95C** placement determinism test: two same-seed runs compare placed-site sets and route edges.
- **SPA-95D** orphan linkage notes for the five engines (feeds Plan 1 packages).
- **SPA-95E** read-model guard: a test/scan that projection types are not referenced from simulation paths.

## 4. Acceptance & verification
- Every spatial fact in the map names exactly one owner; no fact has two.
- Placement test: identical placements and checksums across two runs.
- Read-model guard passes; a deliberate projection-into-simulation injection would fail it.
- `bash scripts/run_test.sh Ashfall.Core.Tests/World/`.

## 5. Risks
Map becomes a de-facto second registry → the map is documentation; facts live
in their named authorities. Orphan wiring races Plan 1 → this plan only records
linkage; the seal packages own the wiring.

---

## 6. Expanded census (12 files · 2,120 lines)

Scope: `Assets/Ashfall.Core/World/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 3 · Support 8 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `DebtRouteAccessResolver.cs` | 35 | Support | — | 0 | 0 | 0 |
| `FactionTerritoryCatalog.cs` | 155 | Catalog | — | 0 | 0 | 0 |
| `LivingMapRouteProjection.cs` | 51 | Support | — | 0 | 0 | 0 |
| `MapRouteHazardEvaluator.cs` | 96 | Support | — | 0 | 0 | 0 |
| `PatrolTerritoryAuthority.cs` | 259 | Support | **yes** | 0 | 0 | 2 |
| `RouteAvailabilityKind.cs` | 33 | Support | — | 0 | 0 | 0 |
| `RouteAvailabilityPresentation.cs` | 25 | Support | — | 0 | 0 | 0 |
| `RouteGateContextResolver.cs` | 292 | Support | — | 0 | 0 | 0 |
| `RouteInfrastructureSystem.cs` | 373 | System | — | 0 | 0 | 3 |
| `RouteRegionTopology.cs` | 149 | Support | **yes** | 0 | 0 | 0 |
| `SettlementCatalog.cs` | 464 | Catalog | — | 0 | 0 | 2 |
| `WeatherRouteGateCatalog.cs` | 188 | Catalog | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `faction_territory.json` | object[4 keys] |
| `wasteland_settlement_npcs.json` | object[3 keys] |
| `caravan_trade_routes.json` | object[2 keys] |
| `weather_route_gates.json` | object[2 keys] |
| `settlements.json` | object[3 keys] |
| `expedition_route_waypoint_notes_batch_2.json` | object[4 keys] |

**State surfaces:** `PatrolTerritoryAuthority.cs`, `RouteInfrastructureSystem.cs`, `SettlementCatalog.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/World/` |
| Test references | 20 name references across the test tree |
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

## 11. Tier-2: intra-domain reference graph

Computed across 13 domain files: **4 type-reference edges**.

| File | Lines | In-degree | Out-degree |
|---|---:|---:|---:|
| `SettlementCatalog.cs` | 464 | 0 | 0 |
| `RouteInfrastructureSystem.cs` | 373 | 0 | 0 |
| `RouteGateContextResolver.cs` | 292 | 1 | 0 |
| `NightWatchPatrolReadinessEngine.cs` | 279 | 0 | 2 |
| `PatrolTerritoryAuthority.cs` | 259 | 2 | 0 |
| `WeatherRouteGateCatalog.cs` | 188 | 0 | 0 |
| `FactionTerritoryCatalog.cs` | 155 | 0 | 0 |
| `RouteRegionTopology.cs` | 149 | 0 | 0 |
| `MapRouteHazardEvaluator.cs` | 96 | 0 | 0 |
| `LivingMapRouteProjection.cs` | 51 | 0 | 0 |

**Highest-coupling files (in×2 + out):**

- `PatrolTerritoryAuthority.cs` — in 2, out 0
- `NightWatchPatrolReadinessEngine.cs` — in 0, out 2
- `RouteAvailabilityKind.cs` — in 1, out 0
- `RouteGateContextResolver.cs` — in 1, out 0
- `DebtRouteAccessResolver.cs` — in 0, out 1
- `RouteAvailabilityPresentation.cs` — in 0, out 1
- `FactionTerritoryCatalog.cs` — in 0, out 0
- `LivingMapRouteProjection.cs` — in 0, out 0

**Ordering implication:** high in-degree files are depended upon — verify or seal
them first. High out-degree files are consumers whose claims should land after
their dependencies; a file with both is the domain's hub and needs its own
bounded package.

---

## 12. Cross-plan coupling

Domain files: 13. Other plans referencing their names: **10**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-WORLD-FAMILY-TRUTH-267` | 3 |
| `PLAN-BASE-DEFENSE-RAIDS-61` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-UNBLOCK-03` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-WEATHER-ATMOSPHERE-28` | 1 |
| `PLAN-WARLORDS-DIPLOMACY-29` | 1 |
| `PLAN-ASYLUM-REFUGEES-85` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SPA-95A` | `PatrolTerritoryAuthority.cs` |
| `SPA-95B` | no name match — resolve at claim time |
| `SPA-95C` | `DebtRouteAccessResolver.cs`, `LivingMapRouteProjection.cs`, `MapRouteHazardEvaluator.cs` |
| `SPA-95D` | no name match — resolve at claim time |
| `SPA-95E` | `LivingMapRouteProjection.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 13. Host files: **4** · Test files: **20** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/HostCli.WastelandInhabitants.cs`, `src/Host/MineClearingFlailHostSession.cs`, `src/Host/RailGrindingHostSession.cs`, `src/Main.Plans146_149.cs` |
| Tests (`Ashfall.Core.Tests/`) | 20 | `Ashfall.Core.Tests/Expeditions/ExpeditionTravelStretchTests.cs`, `Ashfall.Core.Tests/Expeditions/ForcePassageTests.cs`, `Ashfall.Core.Tests/Expeditions/MineClearingFlailEngineTests.cs`, `Ashfall.Core.Tests/Expeditions/RailGrindingEngineTests.cs`, `Ashfall.Core.Tests/Integration/Plans146_149IntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `anomaly_hazard` |
| `faction_espionage` |
| `infrastructure` |
| `route_infrastructure` |
| `settlement_defenses` |
| `settlement_politics` |
| `weather_hardening` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **9** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--deep-coast-route-selftest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--journal-weather-panel-selftest` |
| `--ledger-debt-selftest` |
| `--patrol-encounter-selftest` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |
| `--weather-save-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **11**.

| Event | First declaration |
|---|---|
| `OnAccessSoftened` | `Assets/Ashfall.Core/VouchAccessSystem.cs` |
| `OnCampNightSegmentResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnHazardWarning` | `Assets/Ashfall.Core/VentilationSystem.cs` |
| `OnOverlayAccessChanged` | `Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs` |
| `OnRegionChanged` | `Assets/Ashfall.Core/Muster/LongWalkSystem.cs` |
| `OnTerritoryChanged` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnWeatherChanged` | `Assets/Ashfall.Core/World/WeatherSystem.cs` |
| `OnWeatherFrontArrived` | `Assets/Ashfall.Core/Weather/WeatherCascadeSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_territory.json` |
| `Assets/StreamingAssets/Data/faction_war_communiques.json` |
| `Assets/StreamingAssets/Data/faction_war_dialogue.json` |
| `Assets/StreamingAssets/Data/faction_war_events.json` |
| `Assets/StreamingAssets/Data/faction_war_journal.json` |
| `Assets/StreamingAssets/Data/faction_war_location_overrides.json` |
| `Assets/StreamingAssets/Data/faction_war_radio.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (3 files, 16 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Presentation` | 1 | 5 |
| `Weather` | 2 | 11 |

**Verdict:** 16 cases sit under matching regions — run those first (`Presentation`, `Weather`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **30**
(13 of them panels/HUD).

| Host file |
|---|
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ExcavationHazardSaveStore.cs` |
| `src/Host/FactionBranchHostSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HostCli.FactionCommuniqueSelfTests.cs` |
| `src/Host/RouteInfrastructureSaveStore.cs` |
| `src/Host/WeatherHardeningHostSession.cs` |
| `src/Host/WeatherHardeningSaveStore.cs` |
| `src/Host/WeatherHostSession.cs` |
| `src/Host/WeatherSaveSelfTest.cs` |
| `src/Host/WeatherSaveStore.cs` |
| `src/Main.DebtCredit.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **7**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `anomaly_hazard` | no |
| `faction_espionage` | no |
| `infrastructure` | no |
| `route_infrastructure` | no |
| `settlement_defenses` | no |
| `settlement_politics` | no |
| `weather_hardening` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **5**.

| Stream |
|---|
| `anomaly_hazard` |
| `black_market_debt_event` |
| `route_engineering_mine_flail` |
| `route_engineering_rail_grinding` |
| `weather` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **35**
(CODEX_ONLY 12, GAMEPLAY_CONSUMED 16, UNRESOLVED 7).

| Catalog | Classification |
|---|---|
| `excavation_hazard_mitigation.json` | GAMEPLAY_CONSUMED |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_territory.json` | UNRESOLVED |
| `faction_war_communiques.json` | GAMEPLAY_CONSUMED |
| `faction_war_dialogue.json` | GAMEPLAY_CONSUMED |
| `faction_war_events.json` | GAMEPLAY_CONSUMED |
| `faction_war_journal.json` | GAMEPLAY_CONSUMED |
| `faction_war_location_overrides.json` | GAMEPLAY_CONSUMED |
| `faction_war_radio.json` | GAMEPLAY_CONSUMED |

**Verdict:** 7 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **4**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_honored_debt` |
| `flag_repaired_infrastructure` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 10
**Surface:** save sections 7 (laddered 0) · RNG streams 5 · host files 21 · catalogs 22 · test regions 2 · flags 9

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SPATIAL-SIM-AUTHORITY-95
wave: 8
status: PROPOSED — foreman claim required
packages: SPA-95A, SPA-95B, SPA-95C, SPA-95D, SPA-95E
claim paths:
  - src/Host/AnomalyHazardSaveStore.cs  # §19 candidate host surface
  - src/Host/ExcavationHazardSaveStore.cs  # §19 candidate host surface
  - src/Host/FactionBranchHostSession.cs  # §19 candidate host surface
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/excavation_hazard_mitigation.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/faction_combat_thresholds.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Presentation/
  - godot --headless --path . -- --deep-coast-route-selftest
dependencies:
  - coordinate: 10 other plan(s) name these artifacts (§12)
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
