# PLAN-THERMAL-EXPOSURE-TRUTH-117 — Body Temperature, Clothing Layers & Shelter Heating

**Wave 10 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-WEATHER-ATMOSPHERE-28, PLAN-SHELTER-CAPACITY-AUTHORITY-103, PLAN-ORPHAN-SEAL packages for both clothing engines.
**Non-goals:** no weather simulation (Plan 28 owns the source), no new clothing
catalog, no panel-side temperature math.

## 1. Outcome
Two host-unreachable authorities own the clothing half —
`Inventory/ClothingWarmthSystem.cs` and `Textiles/GarmentLayeringThermalEngine.cs`
(Plan 1 Appendices A/H/K) — while Plan 28 owns weather and Plan 103 owns
occupancy. No contract states how ambient temperature, clothing layers, and
heating combine into a body-temperature effect, so warmth is either ignored or
applied twice.

| Deliverable | Detail |
|---|---|
| Thermal model | inputs: ambient (Plan 28), layers (clothing engines), heating (shelter state); one owner per input, one output (exposure band) |
| Layering rules | layer order/coverage from garment data; wet/damaged modifiers documented |
| Heating truth | a heated room raises the effective ambient per built state; no separate "warmth" counter |
| Exposure effects | cold/heat bands drive documented consequences (rest quality hand-off to Plan 103, injury risk to Plan 124) |
| No double count | a test proves shelter heating + clothing do not apply the same modifier twice |

## 2. Evidence
- Plan 1 Appendix A/H/K: `ClothingWarmthSystem` (Inventory/), `GarmentLayeringThermalEngine` (Textiles/) host-unreachable; K lists their public members.
- Plan 28 owns ambient weather; Plan 103 owns occupancy/sleep quality consuming the result.
- `SaveSectionRegistry`: `survivors` and shelter-family sections exist; thermal state rides them (no new section without Plan 87 policy).

## 3. Packages
- **TET-117A** thermal model doc + input/owner table.
- **TET-117B** layering rules + wet/damage modifier tests.
- **TET-117C** heating contribution from built shelter state + no-double-count test.
- **TET-117D** exposure band consequences wired to their named owners.
- **TET-117E** save round-trip of exposure state (or explicit default behavior).

## 4. Acceptance & verification
- Same ambient, same layers, same shelter → same band across runs (seed-stable, no wall clock).
- Removing a layer changes the band; adding heating changes it; both together are not double-applied.
- `bash scripts/run_test.sh` on the inventory/needs regions touched.

## 5. Risks
Model sprawl → three inputs, one output, documented; the double-count test is the guard.
Overlap with Plan 103 → sleep quality is 103's output; this plan only supplies the band.

---

## 6. Expanded census (2 files · 620 lines)

Scope: `Assets/Ashfall.Core/Inventory/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ClothingWarmthSystem.cs` | 390 | System | **yes** | 0 | 0 | 2 |
| `GarmentLayeringThermalEngine.cs` | 230 | System | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `thermal_gear.json` | object[2 keys] |
| `geothermal_drilling_depths.json` | object[2 keys] |
| `geothermal_strata_catalog.json` | object[2 keys] |
| `borosilicate_sight_glass_thermal_shock.json` | array[8] |
| `geothermal_borehole_logs.json` | array[7] |
| `geothermal_steam_vent_diagnostics.json` | array[7] |

**State surfaces:** `ClothingWarmthSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Inventory/` |
| Test references | 2 name references across the test tree |
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

Domain method: plan-body artifact list.
Governed artifacts: 10. Other plans referencing them: **11**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-GEOTHERMAL-PLANT-TRUTH-191` | 4 |
| `PLAN-GEOTHERMAL-AQUIFER-TRUTH-260` | 4 |
| `EVIDENCE` | 2 |
| `PLAN-SHELTER-ARCHITECTURE-40` | 2 |
| `PLAN-DEEP-STRATA-83` | 2 |
| `PLAN-INDUSTRY-AUTOMATION-45` | 1 |
| `PLAN-WATER-AGRICULTURE-46` | 1 |
| `PLAN-INVENTORY-CONSERVATION-93` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `ClothingWarmthSystem.cs` |
| `GarmentLayeringThermalEngine.cs` |
| `Inventory/ClothingWarmthSystem.cs` |
| `Textiles/GarmentLayeringThermalEngine.cs` |
| `borosilicate_sight_glass_thermal_shock.json` |
| `geothermal_borehole_logs.json` |
| `geothermal_drilling_depths.json` |
| `geothermal_steam_vent_diagnostics.json` |
| `geothermal_strata_catalog.json` |
| `thermal_gear.json` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `TET-117A` | `GarmentLayeringThermalEngine.cs`, `Textiles/GarmentLayeringThermalEngine.cs`, `borosilicate_sight_glass_thermal_shock.json` |
| `TET-117B` | `GarmentLayeringThermalEngine.cs`, `Textiles/GarmentLayeringThermalEngine.cs` |
| `TET-117C` | no name match — resolve at claim time |
| `TET-117D` | no name match — resolve at claim time |
| `TET-117E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 8. Host files: **0** · Test files: **4** · Data files: **2**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/AbyssalAnomaliesRuntimeActivationTests.cs`, `Ashfall.Core.Tests/Inventory/ClothingWarmthSystemTests.cs`, `Ashfall.Core.Tests/Narrative/HydroGeologyDiscoveryTests.cs`, `Ashfall.Core.Tests/Textiles/GarmentLayeringThermalEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 2 | `Assets/StreamingAssets/Data/narrative_discovery_manifest.json`, `Assets/StreamingAssets/Data/thermal_gear.json` |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names)

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **8** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `geothermal_aquifer` |
| `geothermal_orc` |
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |
| `shelter_thermal` |
| `thermal` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **5** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--glass-orchard-selftest` |
| `--narrative-selftest` |
| `--salt-steam-selftest` |
| `--selftest-manifest` |
| `--test-manifest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **14**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnExposureEnded` | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` |
| `OnExposureStarted` | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` |
| `OnHeavyMetalExposure` | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnPathogenExposure` | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` |
| `OnRadiationExposure` | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` |
| `OnShockExpired` | `Assets/Ashfall.Core/Economy/MarketSystem.cs` |
| `OnShockStarted` | `Assets/Ashfall.Core/Economy/MarketSystem.cs` |
| `OnStageNarrativeEmitted` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |
| `OnSteamTrip` | `Assets/Ashfall.Core/BrineWaterSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/narrative/borosilicate_sight_glass_thermal_shock.json` |
| `Assets/StreamingAssets/Data/narrative/invar_pendulum_thermal_expansion.json` |
| `Assets/StreamingAssets/Data/thermal_gear.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **3**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/ShelterThermalHostSession.cs` |
| `src/Host/ShelterThermalSaveStore.cs` |
| `src/UI/ShelterThermalPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `shelter_thermal` | no |
| `thermal` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **3**
(CODEX_ONLY 2, GAMEPLAY_CONSUMED 1).

| Catalog | Classification |
|---|---|
| `narrative/borosilicate_sight_glass_thermal_shock.json` | CODEX_ONLY |
| `narrative/invar_pendulum_thermal_expansion.json` | CODEX_ONLY |
| `thermal_gear.json` | GAMEPLAY_CONSUMED |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

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
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 3 · catalogs 6 · test regions 0 · flags 5

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-THERMAL-EXPOSURE-TRUTH-117
wave: 10
status: PROPOSED — foreman claim required
packages: TET-117A, TET-117B, TET-117C, TET-117D, TET-117E
claim paths:
  - src/Host/ShelterThermalHostSession.cs  # §19 candidate host surface
  - src/Host/ShelterThermalSaveStore.cs  # §19 candidate host surface
  - src/UI/ShelterThermalPanel.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/borosilicate_sight_glass_thermal_shock.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/invar_pendulum_thermal_expansion.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --glass-orchard-selftest
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
