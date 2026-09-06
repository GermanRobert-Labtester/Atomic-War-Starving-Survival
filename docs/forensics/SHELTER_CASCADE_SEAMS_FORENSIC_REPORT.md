# SHELTER CASCADE SEAMS — FORENSIC REPORT

**Scope:** Three residual seams from the B42–B45 reconnaissance:
(1) EMP event → PowerGridSystem coupling, (2) power→water→greenhouse→medical cascade wiring,
(3) catalog-value-driven simulation proof for `power_grid.json` and `crop_strains.json`.

**Method:** Read-only source/data/test trace. No code or data was modified.
Verification commands executed: targeted `dotnet test` runs (PowerGridCatalogTests filter),
JSON inspection, cross-tree greps with call-site reads.

**Date:** 2026-09-06 session. Evidence current as of this working tree.

---

# 1. Target

Verify whether the shelter behaves as one connected simulation across the three seams:

- **S1** — Does any EMP/orbital event resolve into power-grid surge/breaker/battery damage?
- **S2** — Do power outages cascade into water treatment, greenhouse growth, and medical capacity through explicit, deterministic interfaces?
- **S3** — Do edits to `power_grid.json` and `crop_strains.json` actually change runtime simulation?

# 2. Executive Finding

1. **S1 = CONFIRMED GAP.** EMP exists only as weather/narrative/audio. No code path connects any EMP source to `PowerGridSystem` breaker/battery/fuse state. `OrbitalHarrowTelemetrySystem` computes a `PowerGridDisruption` value that **no campaign consumer reads**.
2. **S2 = MOSTLY LIVE, with one CRITICAL defect.** Power cascades into greenhouse (Plan 162 agriculture), weather hardening, sump, schedule, and foundry are real. But the fluid-logistics power feed is **hardwired to 0 every campaign day** due to a room-ID mismatch, water treatment has **no power gating at all**, and medical/quarantine have **zero power coupling** despite an authored `fx_clinic_off` failure effect.
3. **S3 = FAILS for power, PASSES for crops.** `power_grid.json` is **never parsed at runtime** — the host ships a hardcoded 4-room copy of a 6-room catalog. The test that validates the JSON is **quarantined out of compilation** (`<Compile Remove>`), and it disagrees with the data (asserts 18 rooms vs. actual 6). `crop_strains.json` is genuinely data-driven.

# 3. Evidence Summary

| # | Finding | Classification | Severity |
|---|---|---|---|
| F1 | No EMP → grid coupling of any kind | Confirmed Gap | HIGH |
| F2 | `OrbitalImpactReport.PowerGridDisruption` has zero campaign consumers | PORTED_NOT_WIRED | MEDIUM |
| F3 | Fluid logistics receives `power=0f` every day (room-ID mismatch) | LIVE defect | **CRITICAL** |
| F4 | `WaterTreatmentSystem` has no power gating | PARTIAL | HIGH |
| F5 | Medical ward / pipeline / quarantine have no power coupling | PARTIAL | HIGH |
| F6 | `FailureEffectId` authored in JSON, dropped in host, consumed nowhere | DATA_ONLY | MEDIUM |
| F7 | `power_grid.json` not loaded at runtime; hardcoded drift | DATA_ONLY / Trap-7 | **CRITICAL** |
| F8 | `PowerGridCatalogTests.cs` quarantined; asserts 18 rooms vs. 6 in data | Masked drift | HIGH |
| F9 | `crop_strains.json` runtime-loaded and consumed | LIVE_CORE | — (healthy) |
| F10 | Greenhouse power seam (Plan 162) genuinely wired | LIVE | — (healthy) |

# 4. Architecture Placement

- Power authority: `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` (Core, single authority).
- Host binding: `src/Host/PowerGridHostSession.cs` (construction, save, tick RNG).
- Day pipeline: `src/Main.CampaignOwners.cs` `IDayAdvanceOwner` owners (PowerGridDayOwner, GreenhouseFoundryDayOwner, Plan168FluidDayOwner, …).
- Agriculture power seam: `src/Main.Plans162_165.cs` (`BuildAgricultureEnvironment`).
- Fluid network: `Assets/Ashfall.Core/Shelter/FluidLogisticsSystem.cs` + `src/Host/FluidLogisticsHostSession.cs` + `src/Main.Plans166_169.cs`.
- Water treatment: `Assets/Ashfall.Core/WaterTreatmentSystem.cs` + `src/Main.ExpandedShelterSystems.cs`.
- Medical: `Assets/Ashfall.Core/Medical/*`, quarantine: `Assets/Ashfall.Core/Disease/*`.

# 5. Current Implementation — S1 (EMP)

- `WeatherKind.EMPStorm` exists (`Assets/Ashfall.Core/WeatherKind.cs`).
- Narrative event `weather_emp_storm_radio_blackout` requires weather `EMPStorm`
  (`Assets/StreamingAssets/Data/events.json:2436`). Body text only; no machine effects.
- EMPStorm consumers found: `CrisisPresentationCoordinator` (presentation),
  `WeatherGateEvaluator` (route-gate labels), `IWeatherSeverityProvider`,
  `WildlifeTrappingSystem`, audio bridges. **None touch `PowerGridSystem`.**
- `WeatherSystem.cs` contains no grid/breaker/power coupling.
- `PowerGridHostSession` takes no weather dependency at construction or tick.

Evidence:
- `grep -r "EMPStorm"` across Core/src → no `PowerGrid*` hit.
- `Assets/Ashfall.Core/World/WeatherSystem.cs` — no breaker/power references.

**Conclusion:** an EMP storm can be running while the grid reports STABLE. Nothing fuses, trips, or sheds.

# 6. Current Implementation — S1 (OrbitalHarrow)

- `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs:204` computes
  `powerDisruption = anyBreached ? Math.Min(100f, totalDamage * 2.5f) : 0f`.
- Stored on `OrbitalImpactReport.PowerGridDisruption` (line 62) and emitted via
  `OnImpactDetailed` (line ~251).
- **Sole subscriber:** `src/Host/HostCli.DynamicWorld.cs:119` — a headless selftest assertion
  (`Check(impactReport != null, ...)`). It reads existence, not the disruption value.
- No campaign day-owner, host session, or Core system consumes the field.

Evidence: `grep -rn "PowerGridDisruption|powerDisruption"` → only the emitting file.

# 7. Current Implementation — S2 (cascade consumers, verified LIVE)

| Consumer | Coupling | Evidence |
|---|---|---|
| Advanced agriculture (Plan 162) | `IsRoomPowered("room_greenhouse")` → `LightingAvailabilityPermille` (1000/0) | `src/Main.Plans162_165.cs:101-113` |
| Weather hardening | `powerGranted(zoneId) => _powerGrid?.IsRoomPowered(zoneId)` | `Assets/Ashfall.Core/World/WeatherHardeningSystem.cs:197-209` |
| Sump dewatering | `IsRoomPowered(nodeId)` gates centrifuge | `Assets/Ashfall.Core/SumpFloodingSystem.cs:240` |
| Shelter schedule | `IsBrownout` halves lighting demand | `Assets/Ashfall.Core/ShelterScheduleSystem.cs:186-191` |
| Silent foundry | room-power gate on foundry/workshop | `src/Foundry/SilentFoundryHostSession.cs:57,493` |
| Workshop UI | `IsRoomPowered("room_workshop")` | `src/Main.World.cs:289` |

The legacy greenhouse path (`Main.CampaignOwners.cs:219`,
`_greenhouse.TickDay(day, growLightHours: 6f, ...)`) is only a fallback when
Plan 162 agriculture is absent; the live path derives lighting from power.

# 8. Current Implementation — S2 (water & medical, the broken half)

- **F3 (CRITICAL):** `src/Main.Plans166_169.cs:121`:
  `float power = _powerGrid?.System?.IsRoomPowered("room_water_treatment") == false ? 0f : 1f;`
  `PowerGridSystem.IsRoomPowered` returns **false for unknown rooms**
  (`Assets/Ashfall.Core/Shelter/PowerGridSystem.cs:72-78`: `FindRoom` → null → `return false`).
  `room_water_treatment` exists in **neither** `power_grid.json` (which defines
  `room_water_pump`) nor the host's hardcoded 4-room default. The grid always exists
  in campaign (`SetupPowerGrid`, `src/Main.World.cs:359-367`), so
  `Plan168FluidDayOwner` ticks `FluidLogisticsSystem` with `powerAvailability01 = 0f`
  **every day**. Pumps never run via the campaign pipeline.
  Evidence: `src/Host/FluidLogisticsHostSession.cs:41-47` passes the value straight to `System.Tick`.
- **F4:** `WaterTreatmentSystem.TickDay` (`src/Main.ExpandedShelterSystems.cs:329`) has no
  power parameter and no grid reference. `room_water_pump`'s authored
  `fx_water_pressure_drop` has no consumer.
- **F5:** `MedicalWardSystem`, `MedicalPipelineCoordinator`, `DiseaseQuarantineCoordinator`,
  `DiseaseSystem` contain **zero** references to `PowerGridSystem`/`IsRoomPowered`/`IsBrownout`.
  `room_clinic` (120 W, `fx_clinic_off`) powers nothing and nothing observes its failure.
- **F6:** `FailureEffectId` is carried by `PowerGridRoom`
  (`Assets/Ashfall.Core/Shelter/PowerGridSystem.cs:307` — "semantic id the host looks up")
  and persisted (`PowerGridSave.cs:32`), but no host code looks it up. Authored IDs:
  `fx_filtration_off`, `fx_clinic_off`, `fx_water_pressure_drop`, `fx_grow_lights_off`,
  `fx_foundry_standstill`, `fx_lighting_dim` — zero consumers.

# 9. Current Implementation — S3 (catalog consumption)

- **F7 (CRITICAL — Trap 7 realized):** `src/Host/PowerGridHostSession.cs:119-126`:
  ```
  private static PowerGridJson LoadGridJson()
  {
      // Default rooms match the authoritative power_grid.json catalog ...
      return DefaultGrid();
  }
  ```
  `DefaultGrid()` (lines 128-146) hardcodes **4 rooms** and drops all `FailureEffectId`s.
  The authoritative catalog has **6 rooms**
  (`room_water_pump` 100 W critical, `room_lighting_main` 80 W low are missing at runtime).
  Draw watts for the 4 shared rooms happen to match today, but the file is decorative:
  editing it changes nothing. `ContentUtilizationScanner` maps
  `power_grid.json → "PowerGridSystem"` (`Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:335,498,713,925`)
  — a name-adjacency utilization check, not proof of consumption, so the scanner cannot catch this.
- **F8:** `Ashfall.Core.Tests/Shelter/PowerGridCatalogTests.cs` validates the JSON —
  but is excluded from compilation: `<Compile Remove="Shelter/PowerGridCatalogTests.cs" />`
  (`Ashfall.Core.Tests/Ashfall.Core.Tests.csproj:44`). Runtime check confirmed:
  `dotnet test --filter FullyQualifiedName~PowerGridCatalogTests` →
  "No test matches the given testcase filter". The quarantined file asserts
  `Catalog_HasExactly18Rooms` while the data contains 6 — the test and the catalog
  disagree, and the conflict was masked by removing the test instead of reconciling.
- **F9 (healthy):** `crop_strains.json` IS runtime-loaded:
  `CropStrainCatalogLoader.Load(_dataDir, fileIO, json)` (`src/Main.Plans162_165.cs:40`),
  consumed by AgricultureSystem (Plan 162). Strains must resolve seeds through the
  canonical `GreenhouseExpansionCatalog.CropCatalog` (`Assets/Ashfall.Core/Farming/CropStrainCatalog.cs` header contract).
  `fluid_infrastructure.json` is likewise loaded via
  `FluidLogisticsHostSession.Create(_dataDir, system)` (`src/Main.Plans166_169.cs:65`).
- `PowerGridDeterminismTests` (`Ashfall.Core.Tests/PowerGridDeterminismTests.cs`) proves
  seed determinism and numerical safety of the grid math — it does not, and cannot,
  prove catalog consumption.

# 10. State Ownership

- Power state: `PowerGridState` owned by `PowerGridSystem`, captured/restored via
  `PowerGridSave` envelope, section `"power_grid"` (`src/Main.World.cs:373-389`,
  `SaveSectionRegistry` entry). Healthy.
- Fluid state: `FluidLogisticsState` via `FluidLogisticsSaveStore`. Healthy.
- No ownership duplication found across the three seams. The defects are wiring/data,
  not authority forks.

# 11. Save/Load

- Power grid, fluid logistics, and agriculture all persist through the campaign
  envelope (`CaptureSection`, `SaveSectionRegistry`). No gap introduced by this audit.

# 12. Determinism

- All cascade couplings observed are deterministic reads of grid state
  (`IsRoomPowered`, `IsBrownout`, `NetWatts`) inside fixed day-owner phases.
- No `System.Random`, wall-clock, or unordered iteration introduced by the seams audited.
- `Plan168FluidDayOwner`'s constant `power=0f` is deterministic — it is a correctness
  bug, not a determinism bug.

# 13. UI/Player Feedback

- `src/UI/PowerGridPanel.cs` binds snapshots, breaker toggles, priorities, fuel (live-bound).
- `src/UI/StatusPanel.cs` surfaces brownout state.
- Audio: `ShelterAudioController` reacts to `OnPowerChanged` / brownout.
- **Player-facing lie risk:** the fluid network and water treatment fail/degrade for
  reasons (zero power) the power panel will never show, because the panel reports a
  grid whose `room_water_pump` breaker is closed and healthy. The UI-13-style
  "actionable reason" standard (flagship plan §13) is violated by F3/F4 invisibly.

# 14. Tests & Verification

Executed during this audit:
- `dotnet test --filter FullyQualifiedName~PowerGridCatalogTests` → **no test matched**
  (quarantined; build log confirmed exclusion at csproj line 44).
- JSON inspection: 6 rooms confirmed via parse; test file asserts 18.

Existing (compiled) coverage relevant to the seams:
- `PowerGridDeterminismTests` — seed determinism, NaN/negative bounds, divergence bounds.
- `Plan168FluidLogisticsTests`, `WaterTreatment*Tests` — Core-level fluid/water math
  (do not cover the host power argument).
- `GreenhouseSystemTests`, `GreenhouseCropExpansionTests`, `Medical/` suites — domain-internal.
- **Gap:** no test covers `TickPlan168Fluid`'s power derivation or any
  power→water/medical host cascade (Layer-3 host binding tests absent for this seam).

# 15. Duplicates / Legacy / Forks

- None found within the three seams. The hardcoded `DefaultGrid()` is a **stale
  snapshot** of the catalog, not a competing authority — but it functionally acts as
  one because it is the only thing the runtime reads.

# 16. Existing Extension Seams (for the later planner)

- `PowerGridRoom.FailureEffectId` — authored, persisted, carried on the room object.
  A host consequence dispatcher (like the existing `SumpFloodingSystem` adapter
  pattern) can subscribe to `OnPowerChanged` and translate these IDs without touching Core.
- `PowerGridHostSession.CreateDefault` — single place to swap hardcoded defaults for
  a real catalog parse (the class already has the JSON DTO types private to it).
- `FluidLogisticsSystem.Tick(day, temp, powerAvailability01)` — the power parameter
  already exists; only the host argument is wrong.
- `BuildAgricultureEnvironment` — the proven pattern for deriving a domain snapshot
  from grid state; the same shape applies to a future medical ward snapshot.
- `OrbitalImpactReport.OnImpactDetailed` — existing typed event; an EMP/orbital →
  grid surge adapter can subscribe here or at `WeatherSystem` EMPStorm transitions.

# 17. Confirmed Gaps (re-searched)

1. **G1 (CRITICAL):** Fluid logistics receives `power=0f` every campaign day
   (`room_water_treatment` unknown-room ID mismatch). Fix seam:
   `src/Main.Plans166_169.cs:121` — resolve the correct room ID
   (`room_water_pump`) through the grid, not a literal.
2. **G2 (CRITICAL):** `power_grid.json` is not runtime data. Host `DefaultGrid()`
   is stale (4 vs 6 rooms, no failure IDs). Fix seam:
   `PowerGridHostSession.LoadGridJson` should parse the catalog file.
3. **G3 (HIGH):** Quarantined failing test `PowerGridCatalogTests` masks catalog drift
   (18 vs 6 rooms). Must be reconciled (either restore catalog breadth or fix the
   assertion) and un-quarantined.
4. **G4 (HIGH):** No EMP/orbital → grid coupling. `PowerGridDisruption` telemetry
   dead-ends; `EMPStorm` weather never touches breakers/battery.
5. **G5 (HIGH):** Water treatment and medical/quarantine have no power dependency,
   despite `room_clinic`/`fx_clinic_off` authoring. Medical procedures and quarantine
   ventilation are power-inviolate today.
6. **G6 (MEDIUM):** `FailureEffectId` authored, persisted, never consumed —
   the "failure consequence" vocabulary is decorative.
7. **G7 (LOW):** Legacy greenhouse fallback hardcodes `growLightHours: 6f`
   (`Main.CampaignOwners.cs:219`, `ExpansionHostSession.cs:427`) — acceptable as a
   fallback but should be documented as such.

# 18. Risks

| ID | Risk | Class | Evidence |
|---|---|---|---|
| R1 | Editing `power_grid.json` (e.g., balancing clinic draw) silently does nothing; a future balancer will trust the authority and ship wrong gameplay | CRITICAL (data-authority illusion) | F7 |
| R2 | Fluid network permanently depowered → water logistics gameplay (Plan 168) is dead or degenerate in every campaign; player-facing cause invisible | CRITICAL (runtime correctness) | F3, §13 |
| R3 | Catalog/test drift masked by csproj quarantine — the same pattern AGENTS.md documents for other quarantined files; may hide more disagreements | HIGH | F8 |
| R4 | EMP flagship scenario (§18 of the flagship plan) cannot occur: hazard, presentation and audio all fire, grid never notices | HIGH | F1, F2 |
| R5 | Clinic/quarantine "power loss" consequences promised by catalog failure IDs do not exist; medical capacity is unaffected by any outage | HIGH | F5, F6 |
| R6 | Utilization scanner's name-adjacency mapping provides false confidence that the power catalog is consumed | MEDIUM | F7 |

# 19. Constraints for Planning

- `PowerGridSystem` (Core) must remain the single power authority; fixes belong in the
  host session, day owners, and data — not a second grid model.
- The room-ID vocabulary must be reconciled once (`room_water_pump` vs
  `room_water_treatment`) and referenced from the catalog, never literals.
- Any EMP coupling should be deterministic, bounded, and persist post-event damage
  (flagship plan §4.2 Step 42.9 semantics) — attach via existing typed events
  (`OnImpactDetailed`, weather transitions), not string topics.
- Un-quarantining `PowerGridCatalogTests` requires first deciding the intended room
  count (6 shipped vs 18 asserted) — that is a design decision, not a mechanical fix.
- Medical power coupling should follow the `BuildAgricultureEnvironment` snapshot
  pattern to avoid reaching into breaker internals.

# 20. Confidence & Unknowns

- **High confidence:** F1–F9 (all verified by direct source reads and executed test runs).
- **Unknown 1:** Whether any path other than `Plan168FluidDayOwner` ticks
  `FluidLogisticsSystem` in campaign (search found none; only HostCli selftests).
- **Unknown 2:** The intended scope of the 18-room catalog assertion (was a larger
  catalog planned and trimmed, or is the test aspirational?). Git history not
  inspected in this read-only pass.
- **Unknown 3:** Whether UI-13/UI-16 lifecycle findings interact with the
  fluid-logistics panel's display of the zero-power state (panel route was not audited).
- **Not audited here:** save-envelope integrity of the affected sections
  (covered by existing suite), scene lint, full-suite regression.

---

**Audit compliance:** read-only; no code, data, or test files were modified.
