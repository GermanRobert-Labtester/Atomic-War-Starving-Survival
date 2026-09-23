# PLAN-CARTOGRAPHY-LANDMARKS-70 — Map Knowledge, Landmarks, Navigation & Naming

**Wave 6 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED · **Depends on:**
PLAN-TRANSPORT-EXPEDITION-30, PLAN-SIGNALS-REMOTE-SENSING-49.
**Implementation scaffold:** [`PLAN-CARTOGRAPHY-LANDMARKS-70_APPENDIX-A_SCAFFOLD.md`](PLAN-CARTOGRAPHY-LANDMARKS-70_APPENDIX-A_SCAFFOLD.md) — source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no procedural world generation change; no second map authority
(`WastelandMapSystem` remains canonical).

## Outcome
The map already persists knowledge (`WastelandMapSystem.Nodes`/`Knowledge`,
fog/Unknown gating, `PlanRoute`, `CartographySystem.ProjectCanonicalMap`, 8
regions, 15+ `loc_*` stubs, `DamagedMapSystem`, `LandmarkDegradationSystem`,
`LocationEvolutionSystem`, `FieldGuideCatalog`, `GeodeticSurveyEngine`). This
plan makes map knowledge a **player skill and a shared artifact**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Survey | `CartographySystem` (Plan 163) | survey a region | detail, accuracy, hazards |
| Landmarks | map nodes + degradation | find, name, mark | navigation, story, resupply |
| Damaged maps | `DamagedMapSystem` | piece together fragments | reveals, dead ends |
| Evolution | `LocationEvolutionSystem` | revisit | changed sites, new risks |
| Naming | survivor/player | name a site | identity, chronicle entries |
| Sharing | journal/codex/broadsheet | publish a route | NPCs use it; reputation |
| Navigation tools | compass/kit/cartography | equip, craft | survey speed/accuracy |
| Geodesy | `GeodeticSurveyEngine` | control points | map accuracy, route fixing |

## Evidence
- Core: `Exploration/CartographySystem.cs` (5 tests + host projection), `World/WastelandMapSystem`, `World/DamagedMap*`, `World/LandmarkDegradationSystem`, `World/LocationEvolutionSystem`, `World/GeodeticSurveyEngine`, `Narrative/FieldGuideCatalog`.
- Data: `map_regions.json` (8 regions), `wasteland_map_v1.json`, `damaged_map_zones.json`, `locations.json` (canonical `loc_*`), `deep_lore_locations.json` (25).
- Sealed prior: Plan 32 graph travel/fog, Plan 163 cartography, Plan 139 InSAR (feeds survey), `DEBT-PLAN32-MAP-ORPHANS` (loader gate).
- Contracts: map is canonical; projections are read-only; no duplicate region graph.

## Packages
- **CL-70A** survey depth: regions gain an accuracy/coverage value affecting route risk and encounter discovery.
- **CL-70B** landmarks: authored landmark nodes with functions (resupply, shelter, signal, story) and degradation over time.
- **CL-70C** damaged-map assembly: fragment puzzles reveal real nodes; false leads are authored, not random.
- **CL-70D** location evolution: sites change with war/weather/ecology; revisits differ deterministically.
- **CL-70E** naming/notation: player names appear on the map and in chronicle/archive entries.
- **CL-70F** sharing: publishing routes improves NPC/caravan behavior and standing; misdirection has consequences.
- **CL-70G** content volumes: +10 landmarks, +8 map fragments, +6 survey events, +6 evolution profiles; fictional.

## Acceptance & verification
- Survey accuracy measurably changes outcomes; no unreachable nodes (loader gate); determinism.
- `godot --headless --path . -- --cartography-selftest`; `bash scripts/run_test.sh` map suites; `--data-integrity-selftest`.

## Risks
Map busywork → surveying is optional and rewarding; auto-notes reduce bookkeeping.

---

## 6. Expanded census (2 files · 667 lines)

Scope: `Assets/Ashfall.Core/World/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CartographySystem.cs` | 468 | System | — | 0 | 0 | 2 |
| `FieldGuideCatalog.cs` | 199 | Catalog | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `CartographySystem.cs`, `FieldGuideCatalog.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/World/` |
| Test references | 5 name references across the test tree |
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

Domain files: 9. Other plans referencing their names: **11**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-DISCOVERY-STATE-108` | 4 |
| `EVIDENCE` | 3 |
| `PLAN-SPATIAL-SIM-AUTHORITY-95` | 3 |
| `PLAN-ECOLOGY-WILDLIFE-26` | 2 |
| `PLAN-UNBLOCK-03` | 1 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 1 |
| `PLAN-WEATHER-ATMOSPHERE-28` | 1 |
| `PLAN-TRANSPORT-EXPEDITION-30` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CL-70A` | `LivingMapRouteProjection.cs`, `MapRouteHazardEvaluator.cs` |
| `CL-70B` | no name match — resolve at claim time |
| `CL-70C` | `DamagedMapCatalog.cs`, `DamagedMapSystem.cs` |
| `CL-70D` | `WeatherAtmosphereMap.cs` |
| `CL-70E` | no name match — resolve at claim time |
| `CL-70F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 7; intra-domain edges: **1**; isolated files:
**5**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `DamagedMapCatalog` | `DamagedMapSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `DamagedMapSystem` | 1 |
| `CartographySystem` | 0 |
| `DamagedMapCatalog` | 0 |
| `FieldGuideCatalog` | 0 |
| `LivingMapRouteProjection` | 0 |
| `MapRouteHazardEvaluator` | 0 |
| `WeatherAtmosphereMap` | 0 |

**Class split:** hub 0 · sink 1 · source 1 · isolated 5.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 7. Host files: **8** · Test files: **11** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 8 | `src/Host/CodexHostSession.cs`, `src/Host/ExpeditionHostSession.cs`, `src/Host/HostCli.WastelandInhabitants.cs`, `src/Host/WorldHostSession.cs`, `src/Main.Codex.cs` |
| Tests (`Ashfall.Core.Tests/`) | 11 | `Ashfall.Core.Tests/Codex/CodexProjectionTests.cs`, `Ashfall.Core.Tests/Exploration/CartographySystemTests.cs`, `Ashfall.Core.Tests/Exploration/Plan163CartographyIntegrationTests.cs`, `Ashfall.Core.Tests/FieldGuidePersistenceTests.cs`, `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **5** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `anomaly_hazard` |
| `field_guide` |
| `route_infrastructure` |
| `shelter_atmosphere` |
| `weather_hardening` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **8** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--atmosphere-selftest` |
| `--deep-coast-route-selftest` |
| `--gpr-cartography-selftest` |
| `--journal-weather-panel-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |
| `--weather-save-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnEquipmentDamaged` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnHazardWarning` | `Assets/Ashfall.Core/VentilationSystem.cs` |
| `OnToolDamaged` | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` |
| `OnWeatherChanged` | `Assets/Ashfall.Core/World/WeatherSystem.cs` |
| `OnWeatherFrontArrived` | `Assets/Ashfall.Core/Weather/WeatherCascadeSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/atmosphere_profiles.json` |
| `Assets/StreamingAssets/Data/damaged_map_zones.json` |
| `Assets/StreamingAssets/Data/environmental_atmosphere_expansion.json` |
| `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json` |
| `Assets/StreamingAssets/Data/field_guide.json` |
| `Assets/StreamingAssets/Data/narrative/canyon_mudflow_hazard_reports.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_field_reports.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_field_reports_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_route_waypoint_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/faction_field_documents.json` |
| `Assets/StreamingAssets/Data/narrative/field_reports_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/scavenger_expedition_route_notes.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (2 files, 11 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Weather` | 2 | 11 |

**Verdict:** 11 cases sit under matching regions — run those first (`Weather`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **21**
(7 of them panels/HUD).

| Host file |
|---|
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ExcavationHazardSaveStore.cs` |
| `src/Host/FieldGuideSaveStore.cs` |
| `src/Host/HostCli.Cartography.cs` |
| `src/Host/RouteInfrastructureSaveStore.cs` |
| `src/Host/ShelterAtmosphereHostSession.cs` |
| `src/Host/ShelterAtmosphereSaveStore.cs` |
| `src/Host/ShelterAtmosphereSelfTest.cs` |
| `src/Host/WeatherHardeningHostSession.cs` |
| `src/Host/WeatherHardeningSaveStore.cs` |
| `src/Host/WeatherHostSession.cs` |
| `src/Host/WeatherSaveSelfTest.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **5**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `anomaly_hazard` | no |
| `field_guide` | no |
| `route_infrastructure` | no |
| `shelter_atmosphere` | no |
| `weather_hardening` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **4**.

| Stream |
|---|
| `anomaly_hazard` |
| `route_engineering_mine_flail` |
| `route_engineering_rail_grinding` |
| `weather` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **16**
(CODEX_ONLY 9, GAMEPLAY_CONSUMED 5, UNRESOLVED 2).

| Catalog | Classification |
|---|---|
| `damaged_map_zones.json` | GAMEPLAY_CONSUMED |
| `environmental_atmosphere_expansion.json` | GAMEPLAY_CONSUMED |
| `excavation_hazard_mitigation.json` | GAMEPLAY_CONSUMED |
| `field_guide.json` | UNRESOLVED |
| `narrative/canyon_mudflow_hazard_reports.json` | CODEX_ONLY |
| `narrative/expedition_field_reports.json` | CODEX_ONLY |
| `narrative/expedition_field_reports_batch_2.json` | CODEX_ONLY |
| `narrative/expedition_route_waypoint_notes_batch_2.json` | CODEX_ONLY |
| `narrative/faction_field_documents.json` | CODEX_ONLY |
| `narrative/field_reports_expansion.json` | CODEX_ONLY |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 11
**Surface:** save sections 5 (laddered 0) · RNG streams 4 · host files 16 · catalogs 22 · test regions 1 · flags 8

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CARTOGRAPHY-LANDMARKS-70
wave: 6
status: PROPOSED — foreman claim required
packages: CL-70A, CL-70B, CL-70C, CL-70D, CL-70E, CL-70F, CL-70G
claim paths:
  - src/Host/AnomalyHazardSaveStore.cs  # §19 candidate host surface
  - src/Host/ExcavationHazardSaveStore.cs  # §19 candidate host surface
  - src/Host/FieldGuideSaveStore.cs  # §19 candidate host surface
  - src/Host/HostCli.Cartography.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/atmosphere_profiles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/damaged_map_zones.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Weather/
  - godot --headless --path . -- --atmosphere-selftest
dependencies:
  - coordinate: 11 other plan(s) name these artifacts (§12)
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
