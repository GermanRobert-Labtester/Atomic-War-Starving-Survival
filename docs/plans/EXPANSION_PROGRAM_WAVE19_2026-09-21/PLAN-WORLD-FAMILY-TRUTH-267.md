# PLAN-WORLD-FAMILY-TRUTH-267 — Map, Hazard & Knowledge-Gate Data

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SPATIAL-SIM-AUTHORITY-95, PLAN-DISCOVERY-STATE-108, PLAN-WORLD-EVOLUTION-TRUTH-227.
**Non-goals:** no map format change; the family is audited for gate and catalog
wiring.

## 1. Outcome
**24 `World/` files** are referenced by no plan: catalogs
(`GroundPenetratingRadarCatalog`, `SeasonalEventCatalog`, `AnomalyHazardCatalog`,
`WastelandMapCatalogLoader`), route-presentation types
(`RouteAvailabilityKind`, `RouteAvailabilityPresentation`), and
`TravelGraphKnowledgeGate`. The knowledge gate is the interesting one — travel
legality by knowledge state (Plan 108) must have exactly one implementation.

| Deliverable | Detail |
|---|---|
| Gate audit | `TravelGraphKnowledgeGate` is the sole travel-legality gate; duplicates reported |
| Catalog→system map | each catalog ↔ its engine (Plans 49/83/227) |
| Route presentation | kinds/presentations bound to Plan 95's authority state (read models) |
| Anomaly/seasonal data | routed to their systems or flagged |
| Test presence | gate and catalog fixtures |

## 2. Evidence
- 24 `World/` basenames absent from every plan body (Wave 19 file-level audit).
- Plans 95/108/227 own the facts; this plan verifies the data/gate layer.
- Plan 1 Appendix A lists `World/` orphan engines already being sealed — boundary noted.

## 3. Packages
- **WOF-267A** gate audit + duplicate report.
- **WOF-267B** catalog→system map.
- **WOF-267C** route presentation binding tests.
- **WOF-267D** fixtures for gate/catalogs.

## 4. Acceptance & verification
- One travel-legality gate; catalogs resolve to engines; presentations read authority state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/World/`.

## 5. Risks
Gate duplication → audit with a named duplicate report.
Presentation drift → binding tests assert read-only behavior.

---

## 6. Expanded census (15 family files · 1,956 lines)

Scope: files under `Assets/Ashfall.Core/World/` whose basename is referenced
by no plan body (the Wave 19 family definition). Class distribution: Support 8 · Catalog 3 · Loader 2 · DTO/Type 1 · Demo 1.

| File | Lines | Class | Banned refs | Empty catches | Capture/Restore |
|---|---:|---|---:|---:|---:|
| `WeatherGate.cs` | 94 | Support | 0 | 0 | 0 |
| `WeatherGateCatalog.cs` | 194 | Catalog | 0 | 0 | 0 |
| `WeatherGateCatalogLoader.cs` | 193 | Loader | 0 | 0 | 0 |
| `WeatherGateContextEvaluator.cs` | 228 | Support | 0 | 0 | 0 |
| `WeatherGateContextModifier.cs` | 45 | Support | 0 | 0 | 0 |
| `WeatherGateEvaluationContext.cs` | 49 | Support | 0 | 0 | 0 |
| `WeatherGateEvaluator.cs` | 438 | Support | 0 | 0 | 0 |
| `WeatherGateFile.cs` | 45 | Support | 0 | 0 | 0 |
| `WeatherGateRadioHooks.cs` | 207 | Support | 0 | 0 | 0 |
| `WeatherGateResult.cs` | 35 | Support | 0 | 0 | 0 |
| `WeatherHardeningCatalog.cs` | 65 | Catalog | 0 | 0 | 0 |
| `WeatherHardeningCatalogLoader.cs` | 34 | Loader | 0 | 0 | 0 |
| `WeatherHardeningState.cs` | 39 | DTO/Type | 0 | 0 | 0 |
| `WeatherRouteGateCatalog.cs` | 188 | Catalog | 0 | 0 | 0 |
| `WorldHeadlessDemo.cs` | 102 | Demo | 0 | 0 | 7 |

**Census totals:** 0 banned nondeterministic references · 0 empty-catch sites · 1 files with capture/restore methods.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `world_evolution_events.json` | object[2 keys] |
| `world_evolution_seeds.json` | object[9 keys] |
| `world_history.json` | array[79] |
| `world_history_expansion.json` | array[39] |

**State surfaces (capture/restore present):**

- `WorldHeadlessDemo.cs`

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/World/` |
| Family files referenced by tests | 48 name references across the test tree |
| Determinism scan | 0 banned references to fix or justify |
| Failure scan | 0 empty-catch sites to route through Plan 35's rules |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census first (section 6) — classify every family file; no edits in this step.
2. Catalogs and loaders: prove a consumer or report the file as inert.
3. DTO/Type files: round-trip or consume-only proof; unknown values fail typed.
4. System files: confirm the single owner per state; remove duplicated stores.
5. Demo/tooling files: resolve to a real CLI verb or retire (Plan 86 pattern).
6. Regression: focused region plus this family census regenerated.

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
(15 files). Other plans referencing those names: **3**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-WEATHER-ATMOSPHERE-28` | 14 |
| `PLAN-WEATHER-INTELLIGENCE-TRUTH-218` | 3 |
| `PLAN-SPATIAL-SIM-AUTHORITY-95` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `WOF-267A` | no name match — resolve at claim time |
| `WOF-267B` | `WeatherGateCatalog.cs`, `WeatherGateCatalogLoader.cs`, `WeatherHardeningCatalog.cs` |
| `WOF-267C` | `WeatherRouteGateCatalog.cs` |
| `WOF-267D` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 15; intra-domain edges: **17**; isolated files:
**4**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `WeatherGateCatalog` | `WeatherGate` |
| `WeatherGateCatalog` | `WeatherGateCatalogLoader` |
| `WeatherGateCatalogLoader` | `WeatherGateCatalog` |
| `WeatherGateCatalogLoader` | `WeatherGateEvaluator` |
| `WeatherGateCatalogLoader` | `WeatherGateFile` |
| `WeatherGateContextEvaluator` | `WeatherGate` |
| `WeatherGateContextEvaluator` | `WeatherGateContextModifier` |
| `WeatherGateContextEvaluator` | `WeatherGateEvaluationContext` |
| `WeatherGateContextEvaluator` | `WeatherGateEvaluator` |
| `WeatherGateEvaluationContext` | `WeatherGateContextEvaluator` |
| `WeatherGateEvaluator` | `WeatherGate` |
| `WeatherGateEvaluator` | `WeatherGateCatalog` |
| `WeatherGateFile` | `WeatherGate` |
| `WeatherGateFile` | `WeatherGateCatalog` |
| `WeatherGateFile` | `WeatherGateCatalogLoader` |
| `WeatherGateRadioHooks` | `WeatherGateEvaluator` |
| `WeatherHardeningCatalogLoader` | `WeatherHardeningCatalog` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `WeatherGate` | 4 |
| `WeatherGateCatalog` | 3 |
| `WeatherGateEvaluator` | 3 |
| `WeatherGateCatalogLoader` | 2 |
| `WeatherGateContextEvaluator` | 1 |
| `WeatherGateContextModifier` | 1 |
| `WeatherGateEvaluationContext` | 1 |
| `WeatherGateFile` | 1 |
| `WeatherHardeningCatalog` | 1 |
| `WeatherGateRadioHooks` | 0 |

**Class split:** hub 6 · sink 3 · source 2 · isolated 4.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 15. Host files: **5** · Test files: **15** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Host/HostCli.PanelTests.cs`, `src/Host/WeatherHardeningHostSession.cs`, `src/Host/WeatherHardeningSaveStore.cs`, `src/Host/WorldHostSession.cs`, `src/Main.ShelterInfrastructure.cs` |
| Tests (`Ashfall.Core.Tests/`) | 15 | `Ashfall.Core.Tests/Expeditions/ForcePassageTests.cs`, `Ashfall.Core.Tests/World/Plan100_48MoralReactionsWeatherGatesIntegrationTests.cs`, `Ashfall.Core.Tests/World/WeatherGateAuditSimulator.cs`, `Ashfall.Core.Tests/World/WeatherGateBalanceAuditTests.cs`, `Ashfall.Core.Tests/World/WeatherGateCatalogIntegrityTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **5** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `radio` |
| `radio_program_production` |
| `radio_station` |
| `route_infrastructure` |
| `weather_hardening` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **6** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--deep-coast-route-selftest` |
| `--ice-road-tick-demo` |
| `--journal-weather-panel-selftest` |
| `--radio-catalog-selftest` |
| `--radio-selftest` |
| `--weather-save-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **3**.

| Event | First declaration |
|---|---|
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnWeatherChanged` | `Assets/Ashfall.Core/World/WeatherSystem.cs` |
| `OnWeatherFrontArrived` | `Assets/Ashfall.Core/Weather/WeatherCascadeSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_war_radio.json` |
| `Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_route_waypoint_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/radio_broadcast_rundowns.json` |
| `Assets/StreamingAssets/Data/narrative/radio_mysteries_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/radio_scriptbook.json` |
| `Assets/StreamingAssets/Data/narrative/radio_scripts_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/radio_transcripts_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/radio_transcripts_batch_3.json` |
| `Assets/StreamingAssets/Data/narrative/scavenger_expedition_route_notes.json` |
| `Assets/StreamingAssets/Data/narrative/weather_almanac_expansion.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (49 files, 365 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Radio` | 47 | 354 |
| `Weather` | 2 | 11 |

**Verdict:** 365 cases sit under matching regions — run those first (`Radio`, `Weather`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **31**
(10 of them panels/HUD).

| Host file |
|---|
| `src/Host/CoreDemoSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/GodotFileIO.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/RadioCatalogSelfTest.cs` |
| `src/Host/RadioHostSession.cs` |
| `src/Host/RadioProgramProductionHostSession.cs` |
| `src/Host/RadioProgramProductionSaveStore.cs` |
| `src/Host/RadioSaveStore.cs` |
| `src/Host/RadioStationSaveStore.cs` |
| `src/Host/RouteInfrastructureSaveStore.cs` |
| `src/Host/WeatherHardeningHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **5**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `radio` | no |
| `radio_program_production` | no |
| `radio_station` | no |
| `route_infrastructure` | no |
| `weather_hardening` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **4**.

| Stream |
|---|
| `radio` |
| `route_engineering_mine_flail` |
| `route_engineering_rail_grinding` |
| `weather` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **22**
(CODEX_ONLY 10, GAMEPLAY_CONSUMED 10, UNRESOLVED 2).

| Catalog | Classification |
|---|---|
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_war_radio.json` | GAMEPLAY_CONSUMED |
| `narrative/blast_gate_mechanical_audits.json` | CODEX_ONLY |
| `narrative/expedition_route_waypoint_notes_batch_2.json` | CODEX_ONLY |
| `narrative/radio_broadcast_rundowns.json` | CODEX_ONLY |
| `narrative/radio_mysteries_expansion.json` | CODEX_ONLY |
| `narrative/radio_scriptbook.json` | CODEX_ONLY |
| `narrative/radio_scripts_expansion.json` | CODEX_ONLY |
| `narrative/radio_transcripts_batch_2.json` | CODEX_ONLY |
| `narrative/radio_transcripts_batch_3.json` | CODEX_ONLY |

**Verdict:** 2 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 5 (laddered 0) · RNG streams 4 · host files 16 · catalogs 22 · test regions 2 · flags 6

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-WORLD-FAMILY-TRUTH-267
wave: 19
status: PROPOSED — foreman claim required
packages: WOF-267A, WOF-267B, WOF-267C, WOF-267D
claim paths:
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - src/Host/GodotFileIO.cs  # §19 candidate host surface
  - src/Host/LoaderWiringSelfTest.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/faction_radio_corpus.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/faction_war_radio.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Radio/
  - godot --headless --path . -- --deep-coast-route-selftest
dependencies:
  - coordinate: 3 other plan(s) name these artifacts (§12)
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
