# PLAN-TRANSPORT-EXPEDITION-30 — Route Network, Rail, Air & Fleet Logistics

**Wave:** 3 (2026-09-21) · **Kind:** EXPANSION
**Status:** PROPOSED — not a claim. Foreman claim required.
**Depends on:** PLAN-ORPHAN-SEAL-01 Wave 8, PLAN-MARITIME-DEEPWATER-27,
PLAN-DETERMINISM-REPLAY-13.
**Expanded appendix:** [`PLAN-TRANSPORT-EXPEDITION-30_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-TRANSPORT-EXPEDITION-30_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's transport & expedition
systems (5 authorities), each mapped to its parent-plan mechanic row.

**Non-goals:** no second map authority, no vehicle physics sim, no real
vehicle/military hardware data.

---

## 1. Outcome

Make movement a strategic layer: choose and maintain **modes** (road, rail,
water, air), run **convoys**, supply **outposts**, and keep a **fleet** alive.
The corpus is rich: `ModalTravelDispatchEngine`, `RailwaySystem` +
`RailwayInterlockEngine` + `RailLogisticsCatalog` + `RailGrindingEngine` +
`RailTrackMaintenanceEngine` + `rail_network.json`,
`AviationSystem` + `AerialReconWindowEngine` + `CargoAirdropSystem` +
`RadarEcmCatalog`, `VehicleCustomizationSystem` + `vehicle_modules.json` +
`vehicle_armor_grades.json` (CF-P6), `ArmoredCrawlerExpeditionSystem` +
modules, `AmphibiousDraisineEngine`, `RunFlatTireEngine`, `ColonySystem` +
`colony_blueprints.json`, `OutpostSettlementSystem` + `outposts.json`,
`TravelingCaravanSystem` + `caravans.json` + `caravan_trade_routes.json`,
`MineClearingFlailEngine`, and `ReconTelemetrySystem`.

Player loop: **plan a route → choose a mode → load and escort → maintain the
line → extend the network → hold the far end**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Modal choice | `ModalTravelDispatchEngine`, `WastelandMapSystem` | pick road/rail/water/air | time, cost, risk, cargo limits |
| Rail ops | `RailwaySystem`, `RailwayInterlockEngine`, `RailGrindingEngine`, `RailLogisticsCatalog` | schedule trains, maintain track | throughput, derail risk, blockages |
| Aviation | `AviationSystem`, `AerialReconWindowEngine`, `CargoAirdropSystem` | recon, airdrop, ferry | weather windows, drop scatter, detection |
| Fleet | `VehicleCustomizationSystem`, modules/armor, `RunFlatTireEngine` | fit, armor, repair, fuel | speed/cargo/defense, breakdowns |
| Outposts | `ColonySystem`, `OutpostSettlementSystem` | establish, garrison, supply | capacity, starvation, overrun risk |
| Convoys | `TravelingCaravanSystem` | escort, trade, refuse | delivery, piracy losses, standing |
| Hazard ops | `MineClearingFlailEngine`, `AmphibiousDraisineEngine` | clear/ford | new routes opened |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Host-unreachable authorities | `ModalTravelDispatchEngine`, `AerialReconWindowEngine`, `RailwayInterlockEngine`, `RailTrackMaintenanceEngine`, `ColonySystem`, `VehicleCustomizationSystem` + others |
| Data | 16 transport catalogs (`rail_*`, `vehicle*`, `caravans`, `outposts`, `colony_blueprints`, `armored_crawler_modules`, `rerailing_equipment_catalog`, `wasteland_map_v1`, `weather_route_gates`) |
| Prior seals | Plan 32 graph travel/fog, Plan 50 decoration seam, `--vehicle-garage-selftest` 19/19, Plan 58 outposts, Plan 160 colony, CF-P6 armor grades pending |
| Baselines | map distance + Unknown-fog gate is live; caravans expand hops via `PlanRoute` |
| Selftests | `--caravan-selftest`, `--vehicle-garage-selftest`, `--expeditions-selftest` |

---

## 3. Packages

### TR-30A — Modal travel
- `ModalTravelDispatchEngine` scores modes per route from distance, weather,
  infrastructure condition, vehicle availability, and cargo; preview shows
  time/cost/risk per mode.
- **Acceptance:** every mode obeys the canonical map graph and vehicle/
  inventory state; no hidden teleport; forecast gates respected.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/World/` + focused.

### TR-30B — Rail operations
- Bind rail network + interlock scheduling + grinding maintenance + logistics
  catalog to a `rail` day owner: throughput, track condition, blockage events,
  convoy slots.
- **Acceptance:** throughput derives from condition and schedule; derail/block
  events are recoverable; no parallel cargo store.
- **Verify:** `--trains-selftest` (or existing rail probe) + focused suites.

### TR-30C — Aviation and airdrop
- `AviationSystem` + recon windows + airdrop + radar/ECM: flights only in
  windows; drops scatter by wind; recon reveals map intel; ECM affects enemy
  detection.
- **Acceptance:** windows visible in forecast; drop content resolved through
  canonical loot; aircraft state persisted in the vehicle owner.
- **Verify:** aviation focused suite + `--world-selftest`.

### TR-30D — Fleet: modules, armor, maintenance
- `VehicleCustomizationSystem` modules + CF-P6 armor grades + condition wear +
  fuel consumption; amphibious and run-flat capabilities gate route choice.
- **Acceptance:** module install/remove costs items; armor grade changes damage
  outcome; breakdowns recoverable; one vehicle state authority.
- **Verify:** `--vehicle-garage-selftest` + focused modules tests.

### TR-30E — Outposts and colony supply
- `OutpostSettlementSystem` + `ColonySystem`: establish with materials, assign
  garrison, run supply lines, daily consumption, starvation/overrun risk,
  abandonment.
- **Acceptance:** no population/inventory duplication; starvation is visible
  before the failure; abandoned outposts return surviving garrison.
- **Verify:** Plan 58/160 focused suites (12/12 precedent) + caravan supply tests.

### TR-30F — Convoys, escorts and piracy
- `TravelingCaravanSystem` convoys carry cargo with escort assignment; piracy
  risk derives from faction/territory state; losses route through insurance/
  standing rather than silent deletion.
- **Acceptance:** convoy outcome deterministic per seed; escort changes odds
  band; losses itemized; trade ledger canonical.
- **Verify:** caravan + faction focused suites.

### TR-30G — Content volumes
- +6 rail lines, +8 vehicle modules, +4 aircraft, +6 outpost blueprints
  (extend authored 4), +10 convoy events, +6 route hazards; fictional only.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Transport micro-management | one dispatch screen; auto-resolve with chosen policy |
| Mode imbalance (air trivializes map) | weather windows, fuel, detection, cargo limits |
| Convoy loss feels arbitrary | visible risk band + escort + itemized recovery |
| Outpost sprawl | authored blueprints + supply viability gate (food/water per day) |

## 5. Verification

```bash
godot --headless --path . -- --caravan-selftest
godot --headless --path . -- --vehicle-garage-selftest
godot --headless --path . -- --expeditions-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/
bash scripts/run_test.sh Ashfall.Core.Tests/World/
```

---

## 6. Expanded census (24 files · 7,767 lines)

Scope: `Assets/Ashfall.Core/Expeditions/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 3 · Demo 1 · Loader 3 · Support 6 · System 11

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ArmoredCrawlerExpeditionSystem.cs` | 402 | System | — | 0 | 0 | 2 |
| `DraisineRerailingSystem.cs` | 255 | System | — | 0 | 0 | 3 |
| `ExpeditionAggregate.cs` | 116 | Support | — | 0 | 0 | 1 |
| `ExpeditionCatalogLoader.cs` | 130 | Loader | — | 0 | 0 | 0 |
| `ExpeditionEncounterBridge.cs` | 355 | Support | — | 0 | 0 | 0 |
| `ExpeditionHeadlessDemo.cs` | 120 | Demo | — | 0 | 0 | 5 |
| `ExpeditionLootReferenceResolver.cs` | 93 | Support | — | 0 | 0 | 0 |
| `ExpeditionLootValidator.cs` | 115 | Support | — | 0 | 0 | 0 |
| `ExpeditionNavalSystem.cs` | 280 | System | — | 0 | 0 | 0 |
| `ExpeditionSystem.cs` | 1580 | System | — | 0 | 0 | 5 |
| `ExpeditionTravelStretch.cs` | 54 | Support | — | 0 | 0 | 0 |
| `RailGrindingCatalogLoader.cs` | 128 | Loader | — | 0 | 0 | 0 |
| `RailGrindingEngine.cs` | 336 | System | — | 0 | 0 | 2 |
| `RailLogisticsCatalog.cs` | 59 | Catalog | — | 0 | 0 | 0 |
| `RailwayInterlockEngine.cs` | 730 | System | — | 1 | 0 | 11 |
| `RailwaySystem.cs` | 831 | System | — | 0 | 0 | 3 |
| `TravelEncounterCombatBinder.cs` | 57 | Support | — | 0 | 0 | 0 |
| `VehicleArmorGradeCatalog.cs` | 155 | Catalog | — | 0 | 0 | 0 |
| `VehicleGarageCatalog.cs` | 62 | Catalog | — | 0 | 0 | 0 |
| `VehicleGarageSystem.cs` | 865 | System | — | 0 | 0 | 2 |
| `RailTrackMaintenanceEngine.cs` | 243 | System | — | 0 | 0 | 0 |
| `VehicleCustomizationSystem.cs` | 435 | System | **yes** | 0 | 0 | 2 |
| `WaystationCatalogLoader.cs` | 165 | Loader | — | 0 | 0 | 0 |
| `WaystationNetworkSystem.cs` | 201 | System | — | 0 | 0 | 2 |

**Totals:** 1 banned refs · 0 empty catches · 11 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `anomalous_expedition_encounters.json` | object[2 keys] |
| `rail_grinding_catalog.json` | object[4 keys] |
| `rail_logistics_catalog.json` | object[2 keys] |
| `rail_network.json` | object[4 keys] |
| `railway_interlock_catalog.json` | object[6 keys] |
| `rerailing_equipment_catalog.json` | object[2 keys] |

**State surfaces:** `ArmoredCrawlerExpeditionSystem.cs`, `DraisineRerailingSystem.cs`, `ExpeditionAggregate.cs`, `ExpeditionHeadlessDemo.cs`, `ExpeditionSystem.cs`, `RailGrindingEngine.cs`, `RailwayInterlockEngine.cs`, `RailwaySystem.cs`, `VehicleGarageSystem.cs`, `VehicleCustomizationSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Expeditions/` |
| Test references | 100 name references across the test tree |
| Determinism | 1 banned refs to fix or justify |
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

Computed across 45 domain files: **59 type-reference edges**.

| File | Lines | In-degree | Out-degree |
|---|---:|---:|---:|
| `ExpeditionSystem.cs` | 1580 | 19 | 1 |
| `VehicleGarageSystem.cs` | 865 | 1 | 7 |
| `RailwaySystem.cs` | 831 | 5 | 3 |
| `RailwayInterlockEngine.cs` | 730 | 0 | 1 |
| `ChemicalReconEngine.cs` | 593 | 0 | 0 |
| `ColonySystem.cs` | 517 | 0 | 1 |
| `AmphibiousDraisineEngine.cs` | 516 | 0 | 2 |
| `AviationSystem.cs` | 462 | 0 | 0 |
| `VehicleCustomizationSystem.cs` | 435 | 0 | 0 |
| `ArmoredCrawlerExpeditionSystem.cs` | 402 | 0 | 2 |
| `DiscoveryConsequenceSystem.cs` | 390 | 1 | 1 |
| `ExpeditionEncounterBridge.cs` | 355 | 1 | 2 |

**Highest-coupling files (in×2 + out):**

- `ExpeditionSystem.cs` — in 19, out 1
- `RailwaySystem.cs` — in 5, out 3
- `ReconTelemetryState.cs` — in 5, out 1
- `ReconTelemetrySystem.cs` — in 2, out 6
- `VehicleGarageSystem.cs` — in 1, out 7
- `ReconTelemetryCatalog.cs` — in 4, out 0
- `VehicleArmorGradeCatalog.cs` — in 4, out 0
- `ExpeditionHeadlessDemo.cs` — in 0, out 7
- `ExpeditionLootReferenceResolver.cs` — in 3, out 0
- `ExpeditionLootValidator.cs` — in 0, out 5

**Ordering implication:** wire in-dependent files first (high in-degree, low
out-degree), then the terminal consumers. A file with many outgoing edges is a
dependency: it should be sealed or verified before its dependents claim work.

---

## 12. Cross-plan coupling

Domain files: 24. Other plans referencing their names: **14**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-EXPEDITION-FAMILY-TRUTH-269` | 20 |
| `PLAN-EXPEDITION-VEHICLE-TRUTH-219` | 14 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 4 |
| `PLAN-ORPHAN-SEAL-01` | 2 |
| `EVIDENCE` | 2 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 2 |
| `PLAN-WAYSTATION-NETWORK-TRUTH-153` | 2 |
| `PLAN-CORE-ONLY-REGISTRY-11` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `TR-30A` | `ExpeditionTravelStretch.cs`, `TravelEncounterCombatBinder.cs` |
| `TR-30B` | no name match — resolve at claim time |
| `TR-30C` | no name match — resolve at claim time |
| `TR-30D` | `ArmoredCrawlerExpeditionSystem.cs`, `VehicleArmorGradeCatalog.cs`, `RailTrackMaintenanceEngine.cs` |
| `TR-30E` | no name match — resolve at claim time |
| `TR-30F` | no name match — resolve at claim time |
| `TR-30G` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 32. Host files: **56** · Test files: **82** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 56 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Audio/ExpansionAudioBridge.cs`, `src/Host/AmphibiousDraisineHostSession.cs`, `src/Host/ChemicalReconHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 82 | `Ashfall.Core.Tests/Campaign/Plans50_53_SharedIntegrationTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagshipB70_B73Tests.cs`, `Ashfall.Core.Tests/ContentDeepChainGateTests.cs`, `Ashfall.Core.Tests/ContractorRosterSystemTests.cs`, `Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/slice_seven_days.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **22** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `amphibious_draisine` |
| `armored_crawlers` |
| `aviation` |
| `caravan_trade_network` |
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `collectible_discovery` |
| `combat` |
| `draisine_recovery` |
| `encounter_choice` |
| `expedition` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **17** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--amphibious-draisine-selftest` |
| `--chemical-dependency-save-selftest` |
| `--combat-breaching-selftest` |
| `--combat-selftest` |
| `--expedition-encounter-bridge-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--expedition-playtest-selftest` |
| `--expedition-selftest` |
| `--ice-road-tick-demo` |
| `--patrol-encounter-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **32**.

| Event | First declaration |
|---|---|
| `OnCampEncounterResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEncounterSurfaced` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnColonyDied` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnColonyStressed` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnColonySwarming` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnCombatPenaltyChanged` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnCombatPerkEarned` | `Assets/Ashfall.Core/Combat/CombatPerks.cs` |
| `OnConsequenceApplied` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnConsequenceDispatched` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnEncounterArrived` | `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs` |
| `OnEncounterEnded` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/amphibious_draisine_catalog.json` |
| `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` |
| `Assets/StreamingAssets/Data/armored_crawler_modules.json` |
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/colony_blueprints.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/discovery_consequences.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/narrative/armored_cockroach_hive_logs.json` |
| `Assets/StreamingAssets/Data/narrative/armored_locomotive_manifests.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **4** (14 files, 120 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Combat` | 10 | 84 |
| `NarrativeConsequence` | 1 | 20 |
| `Rail` | 1 | 5 |
| `Telemetry` | 2 | 11 |

**Verdict:** 120 cases sit under matching regions — run those first (`Combat`, `NarrativeConsequence`, `Rail`, `Telemetry`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **66**
(19 of them panels/HUD).

| Host file |
|---|
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/ArmoredCrawlerSaveStore.cs` |
| `src/Host/AviationSaveStore.cs` |
| `src/Host/ChemicalDependencyHostSession.cs` |
| `src/Host/ChemicalDependencySaveSelfTest.cs` |
| `src/Host/ChemicalDependencySaveStore.cs` |
| `src/Host/ChemicalReconHostSession.cs` |
| `src/Host/ChemicalReconSaveStore.cs` |
| `src/Host/ChemicalSynthesisHostSession.cs` |
| `src/Host/ChemicalSynthesisSaveStore.cs` |
| `src/Host/CollectibleDiscoverySaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **22**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `amphibious_draisine` | no |
| `armored_crawlers` | no |
| `aviation` | no |
| `caravan_trade_network` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `collectible_discovery` | no |
| `combat` | no |
| `draisine_recovery` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **5**.

| Stream |
|---|
| `amphibious_draisine` |
| `combat` |
| `expedition` |
| `mineral_chemical` |
| `route_engineering_rail_grinding` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **26**
(CODEX_ONLY 13, GAMEPLAY_CONSUMED 6, UNRESOLVED 7).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `armored_crawler_modules.json` | UNRESOLVED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `narrative/armored_cockroach_hive_logs.json` | CODEX_ONLY |
| `narrative/armored_locomotive_manifests.json` | CODEX_ONLY |
| `narrative/bunker_maintenance_glitches.json` | CODEX_ONLY |
| `narrative/bunker_maintenance_logs_batch_2.json` | CODEX_ONLY |

**Verdict:** 7 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 14
**Surface:** save sections 22 (laddered 0) · RNG streams 5 · host files 17 · catalogs 22 · test regions 4 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-TRANSPORT-EXPEDITION-30
wave: —
status: PROPOSED — foreman claim required
packages: TR-30A, TR-30B, TR-30C, TR-30D, TR-30E, TR-30F, TR-30G
claim paths:
  - src/Host/AmphibiousDraisineHostSession.cs  # §19 candidate host surface
  - src/Host/AmphibiousDraisineSaveStore.cs  # §19 candidate host surface
  - src/Host/ArmoredCrawlerSaveStore.cs  # §19 candidate host surface
  - src/Host/AviationSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/amphibious_draisine_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/anomalous_expedition_encounters.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Combat/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 14 other plan(s) name these artifacts (§12)
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
