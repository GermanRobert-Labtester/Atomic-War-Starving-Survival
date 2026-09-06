# PLANS 78–81 UI SPEC — Google Stitch Handoff (Decon Airlock · Geodetic Survey · Kinetic Storage · Chemical Recon)

> **Purpose:** Antigravity (or any agent) can hand this document to
> **`google-stitch`** and generate the four missing Plans 78–81 UI panels with
> no further code archaeology. Stitch output is a **design proposal** —
> implementation lands in Godot 4.7+ C# through the existing UI helper layer
> and must be reconciled with the tokens below (project MCP rule: Stitch
> output never replaces the runtime theme or data authority).
>
> All host APIs and Core state listed here **exist and are tested** (Plans
> 78–81 Waves 1–5; see `docs/plans/PLANS_78_81_FLAGSHIP_CLOSEOUT.md`).

---

## 1. Hard constraints (apply to every screen)

| Constraint | Value |
|---|---|
| Engine | Godot 4.7+ C#, `gl_compatibility`, fixed **1920×1080** |
| Fonts | **BarlowCondensed** (headings/labels), **ShareTechMono** (values/numerals) |
| Mood | Cold, exhausted, phosphor-terminal post-atomic shelter. Engineering readouts, not gauges-for-gauges'-sake. No glassmorphism, no decorative gradients |
| Surfaces | Backdrop `#090B0C`, Surface `#050709`, Card `#090B0D`, Selected `#282319`, Hover `#1D2228` |
| Ink/palette | Primary accent `#D3AA62` (Warm), highlight `#F4C875` (Hot), text `#C7DCD0` (Pale), muted `#938F84`, dim `#7E827A` |
| Semantic | Critical `#FF4D4D`, Warning/Hazard `#FF6B35`, Success `#5CD670`, Info `#6EA3A8`, Radiation `#D9A026`, Amber `#D4A35A` |
| Components | Reuse: `AshfallStatusRail` (metric cards), `AshfallDataGrid` (rows), `AshfallUiHelpers.MakeDataRow/MakeButton/MakeSectionHeader/MakeSmall/MakeSeparator` |
| Feedback | Every action surfaces its outcome line via the host session's `LastEvent` (single event strip — no toasts) |
| Accessibility | Never color-only. Every critical state pairs color with a text label (exact strings per panel below). Hit targets ≥ 90×30. No raw item IDs — displayName only |
| Panel standard | Every action row shows `state → blocker → cost → consequence`: current state, why it's blocked if it is, resource cost, and what happens if run |

---

## 2. Panel 1 — `DeconAirlockPanel` (Plan 78)

**Binds:** `DecontaminationHostSession` (`System:` `DecontaminationSystem`).
**Distinct from** the pre-existing `UltrasonicDecontaminationAirlockPanel` stub — do not merge them.

### Already exists (do NOT regenerate)
`src/UI/DecontaminationPanel.cs`: legacy queue flow — `Enqueue`, `ProcessQueue`, `CompleteCycle(safeRelease)`.

### Plan 78 surface to generate
- **Status rail (7 cards):** Active Stage (`currentStageId` display name) · Stage Ticks (`stageTicksRemaining`) · Surface Contamination (`surfaceContamination` %) · Radiometric Gate (`radiometricGateReading` mSv/h) · Effluent Tank (`effluentTankVolume`/`effluentTankCapacity` L) · Filter Life (`effluentFilterRemainingLiters` L or `NO FILTER`) · Inner Door (`CanOpenInnerDoor` → `OPEN`/`LOCKED`)
- **Stage progress strip:** protocol stage chain as ordered segments; current segment fills by ticks. `DeconStageResult.stageComplete/cycleComplete` drives transitions.
- **Protocol picker:** one row per `Protocols` entry — `display_name`, total water L, chelator units, duration ticks, `interlock_threshold_mSv_per_h`. Route: `StartProtocolCycle(protocolId, survivorId, gearId, surfaceContamination)`.
- **EFFLUENT block:** tank fill bar (contamination tinted `#D9A026`), TREAT button (blocked reasons: `empty_tank`, `filter_exhausted`), INSTALL FILTER button (consumes `item_lead_lined_effluent_filter`).
- **GEAR DISPOSAL block:** list contamination-over-threshold gear (`ShouldDisposeGear`), DISPOSE button (consumes `item_sealed_waste_bin`), disposed-ledger count.
- **MANUAL OVERRIDE:** separate red-zoned button, `#FF4D4D` border, confirm copy: "Clears the occupant regardless of radiometric reading. Contamination enters the shelter air. This is logged."
- **Hard text labels (with color, never color-only):** `INNER DOOR LOCKED` · `CONTAMINATION ABOVE LIMIT` · `REWASH REQUIRED` · `MANUAL OVERRIDE ENGAGED`

### GAP register (host API → UI)

| # | Gap | Host API | State surfaced |
|---|---|---|---|
| D-1 | Protocol picker | `System.StartProtocolCycle(protocolId, survivorId, gearId, surfaceContamination, operatorSkill)` | `System.Protocols` |
| D-2 | Stage progress strip | `System.TickActiveStage()` → `DeconStageResult` | case `currentStageIndex/totalStages`, `stageTicksRemaining` |
| D-3 | Effluent block | `System.TreatEffluent()`, `System.InstallEffluentFilter()` | `effluentTank*`, `effluentFilter*` |
| D-4 | Gear disposal | `System.DisposeContaminatedGear(gearId)`, `System.ShouldDisposeGear(level)` | `disposedGearIds`, disposal threshold |
| D-5 | Manual override | `System.EngageManualOverride()` | `manualOverrideEngaged`, `overrideLog` |
| D-6 | Interlock banner | `System.CanOpenInnerDoor()`, `System.InnerDoorFailureReason()` | failure reason string |

---

## 3. Panel 2 — `GeodeticSurveyPanel` (Plan 79)

**Binds:** `GeodeticSurveyHostSession` (`System:` `GeodeticSurveyEngine`).
**Rule:** the crosshair/vernier is **visual only** — Core returns measured values; the panel performs no trig.

### Surface to generate
- **Status rail (5 cards):** Monuments (`Monuments.Count` active) · Observations (`observations.Count`) · Triangles Resolved (`resolvedTriangles.Count`) · Network Accuracy (`NetworkAccuracy` %) · Routes Unlocked (`UnlockedShortcuts.Count`)
- **Monument ledger:** `AshfallDataGrid` rows per `SurveyPointDef` — displayName, point type, elevation m, baseline quality, status (`MONUMENT` / `—` / `DAMAGED`), ESTABLISH button (blocked reason `missing_items` shows required item displayNames).
- **Observation block:** FROM monument picker → TO point picker → weather condition → OBSERVE button. Result row: horizontal angle °, vertical angle °, uncertainty ±°. Uncertainty renders as `±X.XX°` text next to a spread bar.
- **Triangle resolution:** three-point picker + RESOLVE button; resolved triangle rows show accuracy and day. Duplicate resolve is idempotent — render `ALREADY RESOLVED` rather than disabling.
- **Route knowledge:** unlocked shortcut rows (`UnlockedShortcuts`) with the establishing triangle; surveyed-corridor capability chips: drift reduction %, speed bonus %.
- **Critical text labels:** `MONUMENT DESTROYED` · `BASELINE TOO SHORT` · `NOT RESOLVED`

### GAP register

| # | Gap | Host API | State surfaced |
|---|---|---|---|
| G-1 | Establish monument | `EstablishMonument(surveyPointId, day, consumeItems)` | `System.FindPoint(id).construction_required_items` (displayNames) |
| G-2 | Observe angles | `System.Observe(fromPointId, targetPointId, weather, skill)` | observation rows |
| G-3 | Resolve triangle | `System.TryResolveTriangle(a, b, c)` | `ResolvedTriangle` |
| G-4 | Network + routes | `System.MarkCorridorSurveyed(corridorId)`, `GetDriftReduction`, `GetSpeedBonus` | `NetworkAccuracy`, `UnlockedShortcuts` |
| G-5 | Monument damage | read-only display | monument `integrity`, `isActive` |

---

## 4. Panel 3 — `KineticStoragePanel` (Plan 80)

**Binds:** `KineticStorageHostSession` (`System:` `KineticStorageSystem`).
**Rule:** the emergency brake is routed through the host command — the UI never writes rotor state directly.

### Surface to generate
- **Status rail (7 cards):** RPM (`rotorRpm`, mono font, large) · Stored Energy (`JoulesToKwh(storedEnergyJ)` kWh) · Vacuum (`vacuumPressureTorr` scientific notation) · Bearing Temp (`bearingTemperatureC` °C) · Charge/Discharge (`activeChargeKw`/`activeDischargeKw` kW) · Containment (`containmentHealth` %) · Maintenance (`daysSinceMaintenance`/interval d)
- **Rotor dial:** 0 → `max_safe_rpm_ratio × max_rpm` arc; redline segment beyond safe ratio labeled `OVERSPEED`.
- **Charge/Discharge controls:** charge power slider (≤ `max_charge_kw`) and discharge request; blocked states rendered as text (`VACUUM TOO POOR`, `BEARINGS TOO HOT`, `BRAKE ENGAGED`, `OFFLINE`).
- **Surge log:** last surge event rows — surge displayName, delivered kW vs requested.
- **Black start block:** stored-energy gauge vs `min_stored_energy_kwh`; BLACK START button; failure copy: "Insufficient inertia for starter burst."
- **MAINTENANCE block:** interval countdown, required parts (displayNames), PERFORM MAINTENANCE button; emergency-brake state with `EMERGENCY BRAKE ENGAGED` label and a RELEASE via maintenance route.
- **Hard text labels:** `OVERSPEED` · `VACUUM LOSS` · `BEARING OVERHEAT` · `CONTAINMENT COMPROMISED` · `EMERGENCY BRAKE ENGAGED` · `ROTOR FAILURE`

### GAP register

| # | Gap | Host API | State surfaced |
|---|---|---|---|
| K-1 | Install flywheel | `Install(flywheelClassId, roomId, day, consumeItems)` | `System.Catalog.flywheel_classes` (displayNames, class stats) |
| K-2 | Bring online / brake | `BringOnline(instanceId)`; brake via `System.PerformMaintenance` | `isOnline`, `emergencyBrakeEngaged` |
| K-3 | Charge / discharge | `System.Charge(id, kW, s)`, `System.Discharge(id, kW, s)` | `storedEnergyJ`, `activeChargeKw/DischargeKw` |
| K-4 | Surge response | `System.HandleSurge(instanceId, surgeId)` | `Catalog.surge_events` displayNames |
| K-5 | Black start | `System.TryBlackStart(instanceId)` | `Catalog.black_start.min_stored_energy_kwh` |
| K-6 | Maintenance | `PerformMaintenance(instanceId, day, consumeItems)` | `daysSinceMaintenance`, required items |

---

## 5. Panel 4 — `ChemicalReconPanel` (Plan 81)

**Binds:** `ChemicalReconHostSession` (`System:` `ChemicalReconEngine`).

### Surface to generate
- **Status rail (6 cards):** Battery (`detectorBatteryRemaining`/charge ticks) · Sensor Band (`activeSensorBand`) · Hazards Known (`discoveredHazardIds.Count`) · Observations (`hazardObservations.Count`) · Samples (`collectedSamples.Count`, undelivered count) · Safe Corridors (`safeCorridorIds.Count`)
- **Detector readout:** large normalized-level bar (0–1) with band annotation; hazard class + `SafeExposureBand` as text tier (`SAFE`/`CAUTION`/`DANGER`/`CRITICAL`); recommended filter category chip. Location node picker + band picker (low/medium/wide) + SCAN button.
- **Observation ledger:** `AshfallDataGrid` rows — location, hazard displayName, discovery state (`UNKNOWN`/`SUSPECTED`/`IDENTIFIED`/`QUANTIFIED`), confidence %, last confirmed day.
- **Sample flow:** COLLECT SAMPLE (consumes `item_hermetic_sample_ampoule`; blocked `no_ampoule`/`sample_limit`), sample rows with quality %, DELIVER TO LAB button → `DeliverToLab` rendered as `DELIVERED`/`PENDING`.
- **Corridor block:** MAP SAFE CORRIDOR button per observed location (blocked copy: "Insufficient confidence — rescan."), corridor rows with `SAFE` badge.
- **Hard text labels:** `UNKNOWN HAZARD` · `FILTER INCOMPATIBLE` · `FILTER BREAKTHROUGH` · `CRITICAL EXPOSURE` · `BATTERY DEPLETED`

### GAP register

| # | Gap | Host API | State surfaced |
|---|---|---|---|
| C-1 | Scan | `Scan(locationNodeId, band, skill)` → `ChemicalDetectionResult` | result fields |
| C-2 | Band switch | `System.SetSensorBand(band)` | `Catalog.detector_equipment.detector_bands` |
| C-3 | Samples | `CollectSample(hazardId, locationNodeId, skill, consumeItem)`, `System.DeliverSampleToLab(sampleId)` | `collectedSamples` |
| C-4 | Corridors | `System.TryDiscoverSafeCorridor(corridorId, locationNodeId, skill)` | `safeCorridorIds` |
| C-5 | Battery | `RechargeBattery()` | `detectorBatteryRemaining` |
| C-6 | Filter projection | `System.CalculateFilterConsumption(...)`, `System.IsFilterBreakthrough(...)` | per-hazard load rate |

---

## 6. Stitch prompt skeletons (paste-ready)

> For each panel, give Stitch the constraint table from §1 plus the skeleton.

**DeconAirlockPanel**
```
Design a 1920x1080 shelter airlock decontamination terminal. Cold phosphor-terminal
style, dark surfaces (#090B0C), warm amber accents (#D3AA62). Top: 7-card status
rail (STAGE, TICKS, SURFACE %, GATE mSv/h, TANK L, FILTER, INNER DOOR). Middle:
horizontal 4-stage protocol progress strip (coarse strip → chemical wash →
pressure rinse → radiometric gate) with tick fill. Below left: protocol picker
table (4 rows: name, water L, chelator, duration, threshold). Below right: two
stacked blocks — EFFLUENT (tank fill bar, TREAT, INSTALL FILTER) and GEAR
DISPOSAL (ledger count, DISPOSE). Bottom: single red-bordered MANUAL OVERRIDE
button with warning copy, one-line event strip above it. Critical states render
as text banners: INNER DOOR LOCKED / CONTAMINATION ABOVE LIMIT / REWASH REQUIRED.
```

**GeodeticSurveyPanel**
```
Design a 1920x1080 survey office terminal. Same cold phosphor style. Top: 5-card
rail (MONUMENTS, OBSERVATIONS, TRIANGLES, NETWORK ACCURACY %, ROUTES). Left
half: monument ledger grid (16 rows: name, type, elevation, quality, status,
ESTABLISH button). Right half stacked: observation block (from/to/weather
pickers, OBSERVE button, result row "H 214.7° V +12.3° ±0.08°") and triangle
resolver (three point dropdowns, RESOLVE, resolved rows with accuracy) and a
route-knowledge list (unlocked shortcuts + drift/speed chips). A thin decorative
crosshair/reticle may sit behind the observation block but all readouts are
text-first. No UI-side math.
```

**KineticStoragePanel**
```
Design a 1920x1080 flywheel energy terminal. Same style, heavier industrial
feel. Top: 7-card rail (RPM large mono, ENERGY kWh, VACUUM torr, BEARING °C,
CHARGE/DISCHARGE kW, CONTAINMENT %, MAINTENANCE d). Center: horizontal rotor
dial 0→safe-max RPM with redline segment labeled OVERSPEED. Left: charge slider
+ discharge button + blocked-state text lines. Right stacked: surge log rows,
black-start gauge vs required kWh with BLACK START button, maintenance block
(interval countdown, parts, PERFORM MAINTENANCE). Critical banners as text:
OVERSPEED / VACUUM LOSS / BEARING OVERHEAT / CONTAINMENT COMPROMISED / ROTOR
FAILURE. Emergency brake state is a latched text banner, not a toggle.
```

**ChemicalReconPanel**
```
Design a 1920x1080 atmospheric reconnaissance terminal. Same style. Top: 6-card
rail (BATTERY, BAND, HAZARDS KNOWN, OBSERVATIONS, SAMPLES, SAFE CORRIDORS).
Left: large detector readout — 0..1 normalized level bar, hazard class line,
exposure tier as text (SAFE/CAUTION/DANGER/CRITICAL), recommended filter chip,
location + band pickers, SCAN button. Right top: observation ledger grid
(location, hazard, state ladder SUSPECTED→IDENTIFIED→QUANTIFIED, confidence %,
day). Right bottom stacked: sample flow (COLLECT, quality %, DELIVER TO LAB,
PENDING/DELIVERED) and safe-corridor block (MAP SAFE CORRIDOR with blocked
copy, corridor rows with SAFE badge). Text banners: UNKNOWN HAZARD / FILTER
INCOMPATIBLE / FILTER BREAKTHROUGH / CRITICAL EXPOSURE / BATTERY DEPLETED.
```

## 7. Tone anchors for state copy

- Blocked, factual, no hedging: "Inner door locked — radiometric gate above limit."
- Costs stated plainly: "Treat effluent — recovers ~15% as process water. Sludge remains."
- Danger is logistical, not dramatic: "Filter breakthrough imminent. Swap or exit."
- Override copy owns the consequence: "You are choosing to bring this inside."

## 8. Reconciliation rules (implementation contract)

1. Implement through `AshfallStatusRail` / `AshfallDataGrid` / `AshfallUiHelpers.*` — no custom theme forks.
2. Bind only to the host sessions listed above; the panel performs no domain math (esp. no trig in `GeodeticSurveyPanel`).
3. New action verbs route through the `Main` action-switch pattern (`Main.HandleGreenhouseAction` precedent); `LastEvent` stays the single feedback strip.
4. When a panel gains a real Core binding, **remove its row from the `AGENTS.md` "Missing UI panels" table in the same commit**.
5. Raw IDs never render — resolve displayNames from the four catalogs.
