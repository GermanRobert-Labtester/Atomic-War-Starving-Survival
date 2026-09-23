# PLAN-SHELTER-ARCHITECTURE-40 — Rooms, Air, Heat, Water & Structural Integrity

**Wave:** 4 (2026-09-21) · **Kind:** EXPANSION
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-VERTICAL-BODY-INDUSTRY-05, PLAN-ORPHAN-SEAL-01 Wave 2,
PLAN-UI-SURFACE-15.
**Expanded appendix:** [`PLAN-SHELTER-ARCHITECTURE-40_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-SHELTER-ARCHITECTURE-40_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's shelter/excavation/water
systems, each mapped to its parent-plan mechanic row.
**Non-goals:** no 3D building game, no second power/water authority, no
free-form geometry editor.

---

## 1. Outcome

The bunker is the game's main character, and its authored architecture is
almost complete: `shelter_rooms.json` (23 rooms), `shelter_construction.json`
(6 blueprints + 4 upgrades), `shelter_insulation_catalog.json`,
`shelter_shielding.json`, `shelter_components.json`, `shelter_room_identities.json`,
`shelter_machine_identities.json`, `fluid_infrastructure.json`, `power_grid.json`,
`power_subgrid_nodes.json`, `geothermal_strata_catalog.json`,
`geothermal_drilling_depths.json`, plus the live `ShelterThermalSystem`,
`VentilationSystem`, `AirlockSecuritySystem`, `PowerGridSystem`,
`FluidInfrastructure`, `SubterraneanSystem`, `SumpFloodingSystem`, and the
host-unreachable `ShelterExpansionSystem` (11 tests), `ShelterMaintenanceSystem`,
`ShelterRoomCatalog` and `ShelterAssignmentSystem`.

This plan turns the room list into a **designed machine**: adjacency, zoning,
air, heat, water, power and wear all visible and consequential.

Player loop: **review the layout → expand or renovate → balance air/heat/power/
water → maintain → survive the failure of one node**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Layout & expansion | `ShelterExpansionSystem`, `shelter_construction.json`, `ShelterRoomCatalog` | build, excavate, renovate | capacity, adjacency, stability |
| Assignment & zoning | `ShelterAssignmentSystem`, room tags | zone and assign | proximity effects, access |
| Thermal | `ShelterThermalSystem`, insulation catalog | insulate, heat, zone | temperature, fuel demand |
| Air | `VentilationSystem`, airlocks, scrubbers | balance flow, filter | air quality, contamination |
| Water & fluids | `FluidInfrastructure`, sump, brine, geothermal | plumb, drain, drill | supply, flooding, depth |
| Power | `PowerGridSystem`, subgrids | set priorities | brownouts, critical loads |
| Structure | `ShelterMaintenanceSystem` | repair, reinforce | condition, stability alerts |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Data | `shelter_rooms.json` (23 rooms/12 kinds), `shelter_construction.json`, `shelter_insulation_catalog.json`, `shelter_shielding.json`, `shelter_components.json`, `fluid_infrastructure.json`, `power_grid.json`, `power_subgrid_nodes.json`, `geothermal_strata_catalog.json`, `geothermal_drilling_depths.json` |
| Core live | `ShelterThermalSystem`, `VentilationSystem`, `AirlockSecuritySystem`, `PowerGridSystem`, `SubterraneanSystem`, `SumpFloodingSystem` |
| Host-unreachable | `ShelterExpansionSystem` (5 tests), `ShelterMaintenanceSystem` (5), `ShelterRoomCatalog`/`ShelterAssignmentSystem` |
| Sealed prior | Plan 41 room catalog (9/9 + 14/14 + 11/11), Plan 156 expansion, Plan 29 room identity, Plan 210 sanitation rooms, Plan 50 asset families |
| Contracts | stability < 40% alert; containment/energy bounds from Plan 156 |

---

## 3. Packages

### SH-40A — Layout, adjacency and expansion
- `ShelterExpansionSystem` operates on the canonical room list (no parallel
  grid): cells, collisions, adjacency bonuses/penalties, vertical shafts, and
  renovation. Panels show the plan view.
- **Acceptance:** construction consumes materials through inventory;
  adjacency modifies function (e.g. clinic near quarters); stability alerts at
  the authored threshold; no room duplication.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/` +
  `--shelter-expansion-selftest` (new).

### SH-40B — Thermal zoning and insulation
- Insulation and shielding rows become room-level upgrades; zoning lets the
  player heat living space and let cold rooms freeze (food/water trade-offs);
  heating fuel demand follows the thermal authority.
- **Acceptance:** temperature per room explainable; fuel cost visible; frozen
  rooms affect preservation; no second thermal model.
- **Verify:** `--shelter-thermal-selftest` + focused.

### SH-40C — Air, ventilation and airlocks
- Ventilation flow, filtration state, CO₂/contamination, and airlock
  decontamination cycles; filter replacement is a consumable with breakthrough
  routing (the sealed `OnFilterBreakthrough` path).
- **Acceptance:** air quality degrades realistically and recovers with
  maintenance; no duplicate contamination store; airlocks gate access and
  decon time.
- **Verify:** `--airlock-selftest` + focused.

### SH-40D — Fluids, drainage and geothermal depth
- Water network nodes, pressure, drainage and sump behaviour, brine/geothermal
  wells by depth; drilling unlocks deeper strata with hazard (subsidence)
  consequences from `SubterraneanSubsidenceEngine`.
- **Acceptance:** one fluid authority; flooding recoverable; geothermal output
  gated by depth/hazard; no duplicate water store.
- **Verify:** `--deep-well-selftest` + `--subterranean-selftest`.

### SH-40E — Power subgrids and priorities
- Subgrid nodes let the player isolate circuits (life support, clinic,
  workshop, greenhouse, lighting) and set priorities; load shedding follows
  the canonical grid plus the Power Board from PLAN-VERTICAL-BODY-INDUSTRY-05.
- **Acceptance:** brownout order matches priorities; critical loads never
  silently dropped; no second grid.
- **Verify:** `--power-grid-selftest` + plan 0.5 focused.

### SH-40F — Structure, wear and renovation
- `ShelterMaintenanceSystem` tracks condition per room/component; wear from
  use, weather and activity; renovation restores condition and can change
  function; condition gates performance (Plan 29 identity tells).
- **Acceptance:** wear is visible before failure; repair uses materials/labour;
  no instant full repair; condition persists.
- **Verify:** focused maintenance suite + `--shelter-selftest`.

### SH-40G — Content volumes
- +8 construction blueprints, +6 insulation/shielding rows, +6 fluid nodes,
  +4 geothermal strata, +8 maintenance tasks; consumer-bound and validated.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Architectural complexity overwhelms players | single plan-view panel, defaults auto-managed, alerts only on thresholds |
| System interactions magnify failures | authored bands, staged failure warnings, recovery paths |
| Overlap with industry vertical | power/thermal owners named once; Power Board is the only player policy surface |
| Save growth from per-room state | state rides shelter sections; bounded logs; matrix gate |

## 5. Verification

```bash
godot --headless --path . -- --shelter-selftest
godot --headless --path . -- --shelter-thermal-selftest
godot --headless --path . -- --power-grid-selftest
godot --headless --path . -- --deep-well-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/
godot --headless --path . -- --data-integrity-selftest
```

---

## 6. Expanded census (2 files · 964 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SubterraneanSubsidenceEngine.cs` | 299 | System | **yes** | 0 | 0 | 0 |
| `ShelterExpansionSystem.cs` | 665 | System | **yes** | 3 | 0 | 2 |

**Totals:** 3 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `audio_logs_expansion_05.json` | object[2 keys] |
| `environmental_atmosphere_expansion.json` | object[3 keys] |
| `environmental_texts_expansion_05.json` | object[2 keys] |
| `journal_entries_expansion_05.json` | object[2 keys] |
| `locations_expansion3.json` | object[2 keys] |
| `memorials_expansion_05.json` | object[2 keys] |

**State surfaces:** `ShelterExpansionSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 2 name references across the test tree |
| Determinism | 3 banned refs to fix or justify |
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

Domain files: 4. Other plans referencing their names: **8**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-DEEP-STRATA-83` | 3 |
| `PLAN-ORPHAN-SEAL-01` | 2 |
| `EVIDENCE` | 2 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 1 |
| `PLAN-WATER-AGRICULTURE-46` | 1 |
| `PLAN-ENERGY-NUCLEAR-48` | 1 |
| `PLAN-SIGNALS-REMOTE-SENSING-49` | 1 |
| `PLAN-CRISIS-DISASTER-RESPONSE-80` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SH-40A` | `ShelterExpansionSystem.cs` |
| `SH-40B` | no name match — resolve at claim time |
| `SH-40C` | no name match — resolve at claim time |
| `SH-40D` | no name match — resolve at claim time |
| `SH-40E` | no name match — resolve at claim time |
| `SH-40F` | no name match — resolve at claim time |
| `SH-40G` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 21. Host files: **16** · Test files: **20** · Data files: **6**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 16 | `src/Audio/AudioCueCatalog.cs`, `src/Host/AssetCoverageScanner.cs`, `src/Host/AssetRegistry.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/HostCli.PanelTests.cs` |
| Tests (`Ashfall.Core.Tests/`) | 20 | `Ashfall.Core.Tests/Campaign/DailyBriefingCadenceTests.cs`, `Ashfall.Core.Tests/Content/ContentOrphanCertificationEngineTests.cs`, `Ashfall.Core.Tests/Content/Plan49ContentAtmosphereIntegrationTests.cs`, `Ashfall.Core.Tests/District8DeepCoastTests.cs`, `Ashfall.Core.Tests/Excavation/SubterraneanSubsidenceEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 6 | `Assets/StreamingAssets/Data/environmental_atmosphere_expansion.json`, `Assets/StreamingAssets/Data/sanitation_facilities.json`, `Assets/StreamingAssets/Data/shelter_construction.json`, `Assets/StreamingAssets/Data/shelter_machine_identities.json`, `Assets/StreamingAssets/Data/shelter_room_identities.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **28** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `daily_briefing` |
| `deep_well` |
| `expanded_shelter` |
| `fluid_logistics` |
| `geothermal_aquifer` |
| `geothermal_orc` |
| `infrastructure` |
| `journal` |
| `power_grid` |
| `power_subgrids` |
| `route_infrastructure` |
| `sanitation` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **32** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--asset-coverage-report` |
| `--asset-registry-selftest` |
| `--atmosphere-selftest` |
| `--audio-selftest` |
| `--audio-test` |
| `--deep-coast-host-selftest` |
| `--deep-coast-playthrough` |
| `--deep-coast-route-selftest` |
| `--deep-coast-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--holdfast-briefing` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **7**.

| Event | First declaration |
|---|---|
| `OnEnvironmentalCrisisTriggered` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnJournalTriggered` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnRoomEntered` | `Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs` |
| `OnRoomUnlocked` | `Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json` |
| `Assets/StreamingAssets/Data/shelter_audio_cues.json` |
| `Assets/StreamingAssets/Data/shelter_celebrations.json` |
| `Assets/StreamingAssets/Data/shelter_components.json` |
| `Assets/StreamingAssets/Data/shelter_construction.json` |
| `Assets/StreamingAssets/Data/shelter_governance_blocs.json` |
| `Assets/StreamingAssets/Data/shelter_insulation_catalog.json` |
| `Assets/StreamingAssets/Data/shelter_machine_identities.json` |
| `Assets/StreamingAssets/Data/shelter_origins.json` |
| `Assets/StreamingAssets/Data/shelter_room_identities.json` |
| `Assets/StreamingAssets/Data/shelter_rooms.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (87 files, 754 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Shelter` | 87 | 754 |

**Verdict:** 754 cases sit under matching regions — run those first (`Shelter`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **53**
(13 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/ShelterAssignmentHostSession.cs` |
| `src/Host/ShelterAtmosphereHostSession.cs` |
| `src/Host/ShelterAtmosphereSaveStore.cs` |
| `src/Host/ShelterAtmosphereSelfTest.cs` |
| `src/Host/ShelterBarterSaveStore.cs` |
| `src/Host/ShelterDecorHostSession.cs` |
| `src/Host/ShelterDecorSaveStore.cs` |
| `src/Host/ShelterDecorSelfTest.cs` |
| `src/Host/ShelterEspionageSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **16**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `expanded_shelter` | no |
| `shelter` | no |
| `shelter_assignment` | no |
| `shelter_atmosphere` | no |
| `shelter_barter` | no |
| `shelter_decor` | no |
| `shelter_fire` | no |
| `shelter_noise` | no |
| `shelter_prisoners` | no |
| `shelter_reputation` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **8**
(CODEX_ONLY 2, GAMEPLAY_CONSUMED 2, UNRESOLVED 4).

| Catalog | Classification |
|---|---|
| `narrative/shelter_notices_expansion.json` | CODEX_ONLY |
| `narrative/shelter_songs_expansion.json` | CODEX_ONLY |
| `shelter_machine_identities.json` | UNRESOLVED |
| `shelter_room_identities.json` | UNRESOLVED |
| `shelter_rooms.json` | UNRESOLVED |
| `shelter_schedules.json` | GAMEPLAY_CONSUMED |
| `shelter_social_events.json` | GAMEPLAY_CONSUMED |
| `subterranean_zones.json` | UNRESOLVED |

**Verdict:** 4 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 8
**Surface:** save sections 16 (laddered 0) · RNG streams 1 · host files 13 · catalogs 20 · test regions 1 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SHELTER-ARCHITECTURE-40
wave: —
status: PROPOSED — foreman claim required
packages: SH-40A, SH-40B, SH-40C, SH-40D, SH-40E, SH-40F, SH-40G
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Host/ShelterAssignmentHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/
  - godot --headless --path . -- --asset-coverage-report
dependencies:
  - coordinate: 8 other plan(s) name these artifacts (§12)
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
