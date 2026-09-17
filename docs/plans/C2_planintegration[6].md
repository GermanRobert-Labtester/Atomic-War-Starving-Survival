# C2 — Flagship Integration Plan [6]: Power as a Dependency, Player Load Shedding, and Recoverable Failure Cascades

> **Deliverable:** `docs/plans/C2_planintegration[6].md`
> **Source scope:** Plan 23 — *Power Is a Dependency: The Bunker Runs on Watts*
> **Wave:** Continuity Wave 2 — *The Bunker Machine* (Plans 20–24)
> **Primary objective:** make `PowerGridSystem` the single source of truth for bunker
> electricity, close the remaining consumers that still hold an independent opinion
> about whether electricity exists, expose deficit/shedding as a deliberate player
> decision, and compose existing failures into deterministic, attributable,
> recoverable cascades.
> **Core execution order:** **23A → 23B → 23C**
> **Prerequisite sequencing:** **after 22B** (kitchen/cellar preservation seam) and
> coordinated with **20B** (one interior atmosphere/shielding model). Plan 22C
> pharmaceutical cold chain and Plan 24 people/heat/light/sleep effects are
> consumers, never duplicates.
> **Downstream:** Plan 24C consumes blackout/brownout as a human stress event.
> **Highest regression risk:** day-one default deficit balance and the existing
> four-value priority enum / save compatibility. See §5.4 and §D-1/§D-2.
> **Scope discipline:** no second grid, no new tier vocabulary, no fake powered
> dependencies, no scripted disaster cutscenes, no consumer-private `isPowered`
> authority, no per-frame hydraulic/thermal recomputation, no direct duplicate
> power math in panels.

---

> ### PREMISE-CORRECTION BANNER — READ BEFORE PLANNING IMPLEMENTATION
>
> The source Plan 23 diagnosis ("most bunker systems behave as though electricity
> is decorative") is **no longer accurate**. Current source shows the grid is already
> consumed by a large set of systems, the priority allocator already sheds, the
> player panel already toggles breakers and priorities, and several private flags
> have already been removed or are derived caches.
>
> The work is therefore **not** "connect a disconnected bunker". It is a smaller,
> sharper **continuity repair**: fix the remaining un-gated loads, remove the two
> genuine private authorities, make every power *read* allocation-aware rather than
> global-brownout-aware, add attribution/recovery surfaces, and build the
> fact-reading cascade layer. Full evidence is in §1 and §6; every stale premise
> from the source document is listed there with `file:line`.
>
> Executing the source document literally would duplicate systems that already
> exist (`PowerGridPanel` room controls, `ComputeAllocation`, `RegisterLoadRoom`,
> `CrisisPresentationCoordinator`) and "fix" premises that are already correct.

---

# 0. Executive Intent

ASHFALL already has a capable bunker electricity simulator.

`PowerGridSystem` already models, in deterministic Core:

- generation plus runtime generation contributions,
- battery reserve / capacity / depth of discharge,
- authorised fuel burn (with a fuel-starvation partial-output state),
- room breakers, per-room tripped state, per-room priority,
- deterministic priority allocation with a served prefix / shed suffix,
- a sustainable-battery discharge term (`reserve / 24 h`),
- brownout hours, brownout begin/end edges,
- saturated overload breaker trips,
- EMP/orbital surge trips and battery drain,
- generator condition wear, maintenance, battery-bank install, coated-part install,
- typed `PowerGridEvent` emissions and a per-day `PowerGridTickSummary`.

The continuity problem is narrower and more specific than the source assumed:

1. A small number of loads are **not gated at all** (mechanical ventilation/
   filtration; heating circulation; salt mine; library manuals).
2. Two systems hold a **genuine private power authority** that the grid never
   feeds (`SaltMineExtractionSystem.isPowered`, `PneumaticDispatchSystem.Blackout`).
3. Several consumers read a **global brownout** flag instead of the room
   allocation, so a *brownout with a served room* is treated as a total outage
   (`FoodPreservationSystem`, `HydroponicBiomeSystem`, bionic charger).
4. One consumer reads a **room id that does not exist in any grid catalog**, so it
   is permanently unpowered (`room_ward_clinical`).
5. Automatic shedding is real and deterministic but **not attributed** as
   automatic in the briefing, and there is no aggregate demand/supply/deficit read
   model beyond the panel.
6. There is no fact-driven **cascade** authority that composes existing failures
   with warning + off-ramp; `CrisisPresentationCoordinator` is presentation-only.

The final physical/economic chain remains:

```text
generation + fuel + battery
→ available watts
→ player priorities / authored fallback priorities
→ room power (served/shed)
→ consumer throughput / failure behavior
→ shelter consequences
→ human consequences
→ briefing / audio / UI attribution
```

Three compositional stages, unchanged in intent but re-scoped to current source:

1. **23A — One power authority.** Close the un-gated loads, delete the two real
   private authorities, convert every global-`IsBrownout` read to
   `IsRoomServed`-class reads, fix the dead room-id, publish the load table.
2. **23B — Player load shedding.** Keep the existing room/priority controls,
   add the authored criticality/tier data mapping, automatic-vs-player
   attribution, battery runtime forecast from the canonical estimator, sump
   rising-water clock, breaker recovery cost, and fuel-claim legibility.
3. **23C — Failure cascades.** Add a Core `CascadeCoordinator` that reads existing
   facts and evaluates data-authored rules with a minimum warning window and at
   least one legal off-ramp; route consequences into existing owners; feed the
   existing crisis presentation + briefing + audio.

The flagship outcome:

> **When the generator cannot carry the bunker, the player can explain exactly
> what lost power, whether they or the grid shed it, what that broke, what is at
> risk next, and what action can recover the situation.**

---

# 1. Current Source Truth (verified, not assumed)

All references are current source at the time of writing. This section is the
authority for every premise correction and is the contract the implementation
packages must not contradict without new evidence.

## 1.1 The grid authority already exists and is already authoritative

| Fact | Authority | Evidence |
|---|---|---|
| Generation + contributions | `PowerGridSystem.GenerationWatts` | `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` |
| Battery reserve/capacity | `PowerGridSystem.BatteryReserveWh` / `BatteryCapacityWh` | same |
| Per-room allocation (served/shed) | `ComputeAllocation(...)` (private), surfaced via `PowerGridTickSummary.ServedRoomIds` / `ShedRoomIds` and `IsRoomServed` | same |
| Room powered (legacy global) | `IsRoomPowered` | same |
| Dynamic load registration | `RegisterLoadRoom(PowerGridRoom)` | same |
| Deterministic tier order | `EffectivePriority`; tier descending, then `RoomId` ordinal | same |
| Emergency preset | `ApplyBrownoutShedPreset()` / `ApplyCatalogDefaultPriorities()` | same |
| Surge | `ApplySurgeDay(day, severity)` (per-day deduped) | same |
| Generator condition | `GeneratorCondition`, `GeneratorOutputFactor`, `PerformGeneratorMaintenance` | same |
| Battery build chain | `TryInstallBatteryBank`, `MaxInstalledBatteryBanks = 4`, `BatteryBankCapacityWh = 1000` | same |
| Save envelope | `PowerGridState` + `PowerGridSave` + `src/Host/PowerGridSaveStore.cs` section `power_grid` | `Assets/Ashfall.Core/Shelter/PowerGridSave.cs`, `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs:93` |

The catalog pin: `Assets/StreamingAssets/Data/power_grid.json` (schema 1) defines
**18 rooms**. `ShelterPowerGridCatalogLoader.Validate` enforces schema, unique ids,
non-negative draw and a known priority string; unknown priorities warn. The
fallback (`FallbackDefault()`) is kept in lockstep with the catalog.

## 1.2 The priority vocabulary is 4-valued, not 5-tier

```csharp
public enum PowerGridRoomPriority { Disabled = 0, Low = 1, Standard = 2, Critical = 3 }
```

Catalog string mapping is `critical|standard|low|disabled`
(`ShelterPowerGridCatalog.cs:147`). **There is no life-support / pumping / medical /
production / comfort tier vocabulary anywhere in Core, data, or docs.** The source
plan's "explicit five-tier model" is a design proposal, not an existing contract.
See **Decision D-1**.

## 1.3 Most consumers ALREADY consult the grid

| Consumer | What it reads | Evidence |
|---|---|---|
| Deep Well | `IsRoomServed("room_deep_well_pump")` + dynamic load reg | `Assets/Ashfall.Core/DeepWellSystem.cs:183,207` |
| Atmospheric Condenser | `IsRoomServed("room_water_condenser")` + dynamic load reg | `Assets/Ashfall.Core/AtmosphericCondenserSystem.cs:202,229` |
| Sump Flooding pumps | `IsRoomServed(nodeId)` per node; pump failure incident on shed | `Assets/Ashfall.Core/SumpFloodingSystem.cs:257,427,552` |
| Sanitation facilities | `RoomPowerProvider` feed | `Assets/Ashfall.Core/Shelter/SanitationSystem.cs:203`; `Plan210SanitationPowerFeedTests` |
| Water Treatment | `room_water_pump` via host → `TickDay(day, power01)` | `src/Main.ExpandedShelterSystems.cs:356`; `WaterTreatmentSystem.cs:645` |
| Greenhouse / Agriculture | `IsRoomPowered("room_greenhouse")` → grow-light hours | `src/Main.Plans162_165.cs:107`; `AgricultureSystem.cs:734` |
| Hydroponics biome | host lambda `!IsBrownout` (global) | `src/Main.AdvancedShelterSystems.cs:344`; `HydroponicBiomeSystem.cs:297` |
| Food preservation / cellar | host `!IsBrownout` (global) + storage-bay °C projection | `src/Main.Plans62_65.cs:155`; `FoodPreservationSystem.cs:83` |
| Ventilation electrostatic stage | `IsRoomPowered(stage.roomId)` | `Assets/Ashfall.Core/VentilationSystem.cs:475` |
| Bio-fermentation | `IsRoomPowered` callback | `src/Main.Plans126_129.cs:57`; `BioFermentationEngine.cs:166` |
| Weather hardening | per-zone `IsRoomPowered` | `Assets/Ashfall.Core/World/WeatherHardeningSystem.cs:199` |
| Shelter schedule | `IsBrownout` | `Assets/Ashfall.Core/Shelter/ShelterScheduleSystem.cs:247,342` |
| Silent Foundry | `room_foundry` or `room_workshop` | `src/Foundry/SilentFoundryHostSession.cs:90` |
| Cryo vault | `room_cryo_vault` | `src/Main.PlansB68_B69.cs:136` |
| Medical pipeline (clinic) | `IsRoomPowered("room_clinic")` lambda | `src/Main.Medical.cs:114` |
| Disease isolation | `IsRoomPowered("room_ward_quarantine")` | `src/Main.Medical.cs:458` |
| Armory / munitions | `IsRoomServed("room_armory_munitions")` | `src/Main.Plans162_165.cs:386` |
| Comms array | host `SetPowerState(gridPowered, watts)` | `src/Main.Plans198_201.cs:286` |
| Audio ambience/generator/vent | `IsBrownout`, `OnTickSummary` | `src/Audio/ShelterAudioController.cs:92,110,127,162` |

**Conclusion:** the "only a tiny number of systems consult it" premise is false.
The correct framing is: *the remaining un-gated loads and the global-vs-room read
mismatch* are the continuity defects.

## 1.4 Genuine remaining private power authorities

| System | Private state | Fed by grid? | Assessment |
|---|---|---|---|
| `SaltMineExtractionSystem` | `SaltMineState.isPowered = true`; `TickDaily` early-returns when false (`Foundry/SaltMineExtractionSystem.cs:49,152`) | **No production caller of a setter exists** | **Real defect:** the mine runs at full output forever regardless of grid. Also has a private `powerDraw` field computed but never charged to the grid. |
| `PneumaticDispatchSystem` | `PneumaticDispatchState.Blackout`; `IsPowered => !Blackout && blowerCondition > 0` (`Shelter/PneumaticDispatchSystem.cs:146,221,397`) | Only a UI debug toggle (`src/Main.Plans74_77.cs:371`) | **Real defect:** tube network never blackouts from the grid. |

## 1.5 Derived caches (acceptable) vs dead data (defect)

- `HydroponicBiomeRack.isPowered` (`HydroponicBiomeSystem.cs:19,307,315`) is a
  **derived cache** of the grid lambda — acceptable, but it currently derives from
  the global brownout, not from the rack's room allocation.
- `CommsArrayState.IsPowered` (`World/CommsArraySystem.cs:77,142`) is host-fed from
  the grid — acceptable; verify the host derivation is allocation-aware.
- `LibraryManualDefinition.requiresPower` (`LibraryStudySystem.cs:53`) is authored
  `requires_power` data present in `library_manuals.json` (24 rows, 10 `false`) but
  **read by nothing** — dead authored data. Study jobs run during a blackout.
- `ReactiveAmbienceEvaluator.HasPower` (`Audio/ReactiveAmbienceEvaluator.cs:11`) is
  presentation input — not a gameplay authority.

## 1.6 Confirmed defects found during reconnaissance

1. **Dead room id.** `src/Main.Bionics.cs:76` gates the implant charger on
   `IsRoomPowered("room_ward_clinical")`. `room_ward_clinical` is in **no** catalog
   and is never registered as a dynamic load, so `IsRoomPowered` returns `false`
   permanently. **Bionic implant charging is always unavailable.** (Correct target
   is `room_ward_quarantine` or `room_clinic`.) This is the §60.4-class silent
   failure in the source plan.
2. **Mechanical ventilation is not power-gated.** `VentilationSystem.TickDay` runs
   filter saturation, duct integrity, CO/smoke and gas state regardless of
   `room_air_filtration`; only the Plan-72 electrostatic stage checks power
   (`VentilationSystem.cs:468-475`). Air handling is therefore half a load.
3. **Heating circulation is not wired.** `ShelterThermalSystem.TickDay(day)` takes
   no power and `SetGeneratorWasteHeat(generatorKw, pumpActive)` has **no production
   caller** (only tests). `circulationPumpActive` is never driven, so generator
   waste-heat recovery never happens in the running game and `room_heating` is a
   decorative draw.
4. **Food preservation uses the global brownout.** A brownout where
   `room_storage_bay`/`room_cryo_vault` is *served* is treated as a full outage;
   conversely the cryogenic branch keys on `UnpoweredDays`, not on the cryo room.
5. **Salt mine uncoupled** and **tube network uncoupled** (see §1.4).
6. **No automatic-shed attribution.** `PowerGridTickSummary` exposes
   `ShedRoomIds`, but no day event distinguishes *player shed* from *grid shed*, and
   the briefing does not name either.
7. **No canonical battery-runtime estimator.** `computeAllocation` derives
   sustainable discharge internally; there is no public read model a panel can call
   without re-deriving arithmetic (the §60.9 failure mode).
8. **Room-id drift precedent.** `src/Main.Plans166_169.cs:177` documents a prior
   `room_water_treatment` mismatch that "matched no grid room" — the class of bug
   is known, not unique to bionics.

## 1.7 The cascade layer does not exist, but an adjacent presenter does

`Assets/Ashfall.Core/UI/CrisisPresentationCoordinator.cs` already aggregates
power / disease / weather / shielding / fire / sump / radiation / fate into a
`CrisisPresentationSnapshot` with severity, cause text and metrics. It is a
**presentation aggregator**, not a rule evaluator: it has no warning windows, no
off-ramps, no progression, no attribution chain, and no persistence. 23C must build
the risk authority *behind* it and feed it, not replace it.

## 1.8 The panel already provides most of 23B

`src/UI/PowerGridPanel.cs` already renders generation/condition, draw/net,
battery/banks, fuel, brownout/deficit status, per-room served/shed with reason
tone, per-room priority pickers, ON/OFF breakers, install-battery-bank,
service-generator, emergency shed preset, restore defaults, and fuel add. It does
**not** render battery runtime, fuel runway, automatic-vs-player attribution,
sump clock, breaker reset cost, or a cascade/strain readout.

## 1.9 Measured default grid load (balance premise)

From `power_grid.json` (before any external generation contributions):

| Tier | Rooms | Draw |
|---|---:|---:|
| critical | 6 | 910 W |
| standard | 8 | 1330 W |
| low | 4 | 670 W |
| **total intent draw** | **18** | **2910 W** |

Supply at start: base generation **800 W** + sustainable battery discharge
(4000 Wh / 24 h) ≈ **166.7 W** ≈ **966.7 W**. Therefore the *default catalog alone*
is ~3.0× oversubscribed; the served prefix is the 6 critical rooms (910 W) and every
standard/low room is shed on day one, with the battery draining toward brownout.
Fuel burn is 19.2 units/day at base rating (100 units ≈ 5.2 days). This is a
**balance decision**, not a bug, but it must be measured and either accepted as
intended pressure or rebalanced. See **Decision D-3**.

---

# 2. Program-Level Success Criteria

C2[6] is complete only if all of the following are true.

## 2.1 Every meaningful powered function declares its dependency
For each candidate subsystem: room, draw, failure behavior, grace window,
restart/recovery cost. Systems with no meaningful consequence are classified
`DECORATIVE` and left alone (§6.3).

## 2.2 One authority answers whether a room is powered
No private `isPowered`/`Blackout` state remains for bunker electrical availability.
Derived caches must be fed from `IsRoomServed`-class reads.

## 2.3 Total draw can exceed supply, and the deficit is measurable
Already true (§1.9); the deficit must be exposed as a first-class read model, not
recomputed by panels.

## 2.4 The player can prioritize
Already partially true via breakers/priorities/preset. The player must be able to
shed by authored criticality class and by room, reversibly.

## 2.5 Automatic shedding remains accountable
When the grid sheds, the briefing says the grid did it; when the player does it,
it says the player did.

## 2.6 Power failures propagate physically
Refrigeration warms, water treatment slows/stops, mechanical filtration degrades,
heating delivery falls, pumps stop, medical services constrain — each through its
owning system, never through grid arithmetic.

## 2.7 Cascades are emergent, not scripted
Rules evaluate facts already owned by subsystems. No authored cutscene forces a
sequence.

## 2.8 Every cascade is recoverable
Every reachable stage has warning time, at least one legal intervention, and an
off-ramp. Static integrity validation rejects reachable rule sets with no off-ramp.

---

# 3. Architectural Invariants

## 3.1 One `PowerGridSystem`
No second grid, no per-system power manager, no isolated room battery, no hidden
power boolean.

## 3.2 Consumers use one narrow contract
Introduce a small Core abstraction (working name `IPowerConsumer`, final shape
TBD per §8.1). The consumer declares what it needs; the grid/host decides delivery.
Consumers must remain testable with a stub allocation state.

## 3.3 Prefer a delivered-power input over direct grid internals
Where a system only needs availability, feed a boolean/percentage value
(`WaterTreatmentSystem.TickDay(day, power01)` is the existing good pattern) rather
than injecting the whole grid. Where a system must distinguish served-vs-shed, it
reads the allocation-aware query or the tick summary.

## 3.4 One registration point
All dynamic consumer registration lives in one composition seam
(`SetupPowerConsumers()` or the existing per-system registration callsite), with
deterministic order and fail-loud duplicate detection.

## 3.5 No fake dependencies
If a system's output genuinely does not depend on electricity, do not wire it.
`room_lighting_main`, `room_common_mess_hall`, `room_radio_tuner`, and similar
catalog rows must be classified `DECORATIVE` or given real effects — never left as
a draw with no consequence.

## 3.6 One atmosphere model
Filtration/ventilation power effects feed the Plan 20B shielding/atmosphere model.
The grid never adds radiation dose.

## 3.7 One refrigeration model
Power loss modifies the 22B preservation/cellar dynamics. No spoilage timer in the
grid. `FoodPreservationSystem` remains the spoilage authority; `CryoVaultSystem`
remains the cryo-sample authority — they are separate by existing design and must
stay separate.

## 3.8 One human consequence path
Lighting/heat/schedule effects feed Plan 24 systems. No morale/sleep consequence in
grid or cascade code.

## 3.9 Cascades read facts; they do not own them
`CascadeCoordinator` observes brownout, sump level, temperature, filtration, fire,
hatch, outbreak, contamination and lighting facts. It never writes subsystem
fields; it calls existing actions/events or only records risk progression.

## 3.10 Deterministic ordering
Registration, shedding, cascade evaluation and attribution order is stable:
priority tier, then ordinal id; rules evaluated in ordinal rule-id order; no
dictionary-order or wall-clock dependence.

---

# 4. Dependency Graph

```text
20B — interior atmosphere / shielding
        ▲
        │  23A feeds ventilation/filtration state into the 20B model
        │
22B — cellar / preservation (host-projected storage °C)
        ▲
        │  23A makes room power allocation-aware for storage/cryo
        │
22C — pharmaceutical cold chain
        ▲
        │  23A gates storage capability, not medicine logic
        │
24 — people / heat / light / sleep
        ▲
        │  23A/23B feed human consequences
        │
23A — one authority
        │
        ▼
23B — player shedding + attribution + read models
        │
        ▼
23C — fact-driven recoverable cascades
        │
        ▼
24C — blackout/brownout as a human stress event
```

Required sequencing:

```text
22B  →  23A  →  23B  →  23C
```

Coordinate 23A with 20B rather than duplicating atmosphere logic.

---

# 5. Baseline Capture

## 5.1 Mandatory verification commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --audio-selftest
godot --headless --path . -- --panel-bind-lifecycle-selftest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Focused power baselines (per `TEST_POLICY.md`, one region at a time):

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridPhase2AllocationTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridPhase5MaintenanceTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridSurgeTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Plan210SanitationPowerFeedTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/ShelterScheduleIntegrationTests.cs
```

## 5.2 Recorded baseline (current)

| Fact | Value | Source |
|---|---|---|
| Catalog rooms | 18 | `power_grid.json` |
| Tier split | critical 6 / standard 8 / low 4 | computed |
| Total intent draw | 2910 W | computed |
| Base generation default | 800 W | `power_grid.json` |
| Battery capacity default | 4000 Wh (+1000/bank ×4 max) | catalog + `PowerGridSystem.cs` |
| Sustainable discharge term | reserve / 24 h ≈ 166.7 W | `ComputeAllocation` |
| Default fuel | 100 units (≈5.2 days at base) | catalog + burn formula |
| Generator wear | 0.25/day while burning; derate <50 | `PowerGridSystem.cs` |
| Breaker overload trip | brownout ≥4 h, 10%/room/tick | `TickDay` |
| Surge | EMP severity 0.6; drain 0.15×capacity×severity | catalog + `ApplySurgeDay` |
| Priority enum | Disabled/Low/Standard/Critical | `PowerGridSystem.cs` |
| Save sections | `power_grid`, `power_subgrids`, `nuclear_core_lifecycle`, `kinetic_storage`, `solar_concentrator`, `geothermal_orc`, `food_preservation` | `SaveSectionRegistry.cs` |
| Day-advance 30d median | **0.737 s** (min 0.663, p95 2.86) | `artifacts/runtime-scale-results.json` |
| Day-advance 180d median | 4.627 s | same |
| Day-advance 360d median | 11.236 s | same |
| 30d alloc bytes | 363,056 | same |
| Day owner phase | `power_grid` **phase 1**; consumers phase 2/3 | `Main.CampaignOwners.cs:22` |

## 5.3 Baseline behavior probes to record before packages

- `TickDay` with a healthy generator: exact `PowerGridTickSummary` for day 1
  (Generation/Draw/Served/Unserved/ShedRoomIds).
- A forced brownout: verify `BrownoutBegan` fires once and `IsRoomServed` still
  serves critical rooms while the battery lasts.
- Save → mutate → restore: verify no replayed brownout/breaker event.
- Audio: `--audio-selftest` power transitions.

## 5.4 Open baseline questions (record answers before coding)

- **D-3:** Is the day-one 2910 W vs ~967 W supply intended? If yes, every fresh
  campaign opens in permanent shedding and standard/low rooms never run. If not,
  either raise base generation, lower draws, or author a start-up subset.
- **D-2:** Does adding a 5-value tier require a save-compatible migration, or can
  it be authored as a `criticality_class` string mapping many-to-one onto the
  existing 4-value enum with display labels derived from data? (Preferred:
  mapping, no enum change, no save bump.)

---

# 6. Power Load & Consumer Inventory

## 6.1 The load table (contract)

| Consumer | Room | Draw W | Mode | Criticality | Failure without power | Grace | Restart cost | Existing authority |
|---|---|---:|---|---|---|---|---|---|
| Air filtration (mechanical) | `room_air_filtration` | 180 | continuous | critical | filtration effectiveness falls; 20B interior model degrades | data-authored | service | **un-gated — fix** |
| Clinic (diagnostics/procedures) | `room_clinic` | 120 | on-demand | medical | reservation blocked with reason | 0 | none | `Main.Medical.cs:114` |
| Quarantine isolation | `room_ward_quarantine` | 90 | continuous | medical | isolation power check fails; disease containment degrades | data | none | `Main.Medical.cs:458` |
| Water pump | `room_water_pump` | 100 | continuous | pumping | treatment throughput pauses | data | none | `Main.ExpandedShelterSystems.cs:356` |
| Water filtration | `room_water_filtration` | 140 | continuous | pumping | **currently unread — wire to treatment stage** | data | filter media | `room_water_filtration` catalog row |
| Cryo vault | `room_cryo_vault` | 280 | continuous | critical | sample viability degrades (B69) | thermal mass | none | `Main.PlansB68_B69.cs:136` |
| Greenhouse grow lights | `room_greenhouse` | 160 | daily window | production | grow-light hours → growth/yield fall | data | none | `Main.Plans162_165.cs:107` |
| Electric heating | `room_heating` | 240 | continuous | comfort | circulation/waste-heat delivery falls | thermal mass | part | **un-wired — fix** |
| Kitchen | `room_kitchen` | 150 | on-demand | production | prep/service constrained | 0 | none | verify `KitchenNutritionSystem` seam |
| Airlock decontamination | `room_airlock` | 130 | on-demand | production | decon cycle unavailable | 0 | none | `DefenseSystem`/airlock path |
| Radio tuner | `room_radio_tuner` | 90 | on-demand | production | radio operations constrained | 0 | none | verify |
| Laboratory research | `room_laboratory_research` | 260 | on-demand | production | research jobs constrained | 0 | none | verify |
| Precision workshop | `room_workshop_precision` | 240 | on-demand | production | metrology drift uncorrected | 0 | none | `Main.PlansB86_B89` |
| Armory & munitions | `room_armory_munitions` | 60 | on-demand | production | service/press unavailable | 0 | none | `Main.Plans162_165.cs:386` |
| Foundry | `room_foundry` | 220 | on-demand | production | forging standstill | 0 | none | `SilentFoundryHostSession.cs:90` |
| Main lighting | `room_lighting_main` | 80 | nightly | comfort | **DECORATIVE unless Plan 24 defines a consequence** | — | — | classify |
| Workshop | `room_workshop` | 300 | on-demand | production | crafting constrained | 0 | none | `SilentFoundryHostSession.cs:90` |
| Common mess hall | `room_common_mess_hall` | 70 | on-demand | comfort | **DECORATIVE unless meal ambience is authored** | — | — | classify |
| Salt mine | `room_deep_strata`/own | own `powerDraw` | daily | production | `TickDaily` early-returns | 0 | none | **private flag — fix** |
| Pneumatic tube network | own | own | continuous | production | network unpressurised | 0 | blower part | **private flag — fix** |
| Library study | `room_library`/study area | 0 (authored per manual) | daily | knowledge | powered-only manuals unreadable | 0 | none | **dead data — wire** |
| Bionic charger | `room_ward_clinical` (**bad id**) | 0 | on-demand | medical | charger always unavailable | 0 | none | **bug — fix to real room** |
| Deep well pump | `room_deep_well_pump` (dynamic) | system-owned | continuous | pumping | intake silent | 0 | none | `DeepWellSystem` |
| Condenser array | `room_water_condenser` (dynamic) | system-owned | continuous | pumping | condensate silent | 0 | none | `AtmosphericCondenserSystem` |
| Sump pumps | `sump_node_*` (dynamic) | system-owned | continuous | pumping | water rises; pump-failure incident | none (real clock) | pump part | `SumpFloodingSystem` |
| Comms array | host-fed | own | on-demand | production | array unpowered | 0 | none | verify host derivation |
| Hydroponic racks | `room_greenhouse` | derived | continuous | production | racks unpowered | 0 | none | `HydroponicBiomeSystem` |

`DECORATIVE` entries must either be given a real Plan-24 consequence or explicitly
reclassified and excluded from the reaction graph. Do not leave a watt with no effect.

## 6.2 Failure-class requirement

Every row must declare one of: `LIFE_SUPPORT`, `PUMPING`, `MEDICAL`, `PRODUCTION`,
`COMFORT`, `DECORATIVE`. This is an authored classification that maps onto the
existing 4-value simulation enum (see D-1/D-2); it is **not** a new simulation
priority and must not be a second display-order source.

## 6.3 Dynamic loads (already registered) vs catalog rooms

`RegisterLoadRoom` is idempotent and catalog rooms take precedence
(`PowerGridSystem.cs:470`). Dynamic loads are re-registered after their owning
system restores, because the grid's room list is not saved. New dynamic consumers
must follow the same pattern and the same `Setup`/`Save`/`Flush` discipline.

---

# 7. Workstream 23A — One Power Authority

## 7.1 Objective
Close un-gated loads, delete real private authorities, convert global reads to
allocation-aware reads, fix the dead room id, and publish the load contract.

## 7.2 23A Phase A — Publish the contract first
Commit `docs/systems/POWER_LOAD_CONSUMER_CONTRACT.md` containing §6.1's table,
the failure classes, and the registration map. This is the artifact that prevents
re-introducing a private flag.

## 7.3 23A Phase B — Consumer abstraction (minimum shape)
Per §3.2/§3.3: prefer the existing delivered-value pattern where sufficient. Only
introduce `IPowerConsumer` where a system needs loss/restoration callbacks with
grace/restart state. Do **not** inject `PowerGridSystem` into systems that only need
a boolean. Tests must use a stub.

Suggested shape:

```csharp
public interface IPowerConsumer
{
    string ConsumerId { get; }   // stable, unique
    string RoomId { get; }
    float DrawWatts { get; }
    void OnPowerLost(int day);
    void OnPowerRestored(int day);
}
```

If chosen, registration must reject duplicate `ConsumerId`/`RoomId` loudly and
deterministically.

## 7.4 23A Phase C — Central registration
One host composition method registers consumers, assigns rooms/draws from data,
establishes ordinal ordering, and fails loudly on duplicates. This protects
Setup/Save/Flush parity and auditability. (Existing per-system registration may be
folded into this seam, not duplicated.)

## 7.5 23A Phase D — Remove private power flags
1. **Salt mine:** remove `isPowered` as an independent authority; feed the mine from
   `IsRoomServed(<mine room>)`; charge `powerDraw` to the grid (via a registered
   load) or explicitly document why the mine is off-grid and classify it.
2. **Pneumatic dispatch:** replace `_state.Blackout` authority with a grid-fed
   `SetBlackout(bool)` derived from the tube network's room/power, or register the
   blower as a load and gate on served state. Keep a `BlackoutSafe` pipe
   classification (`PneumaticDispatchSystem.cs:86`) — that is a physical property,
   not a power authority.

Equivalence test first: old powered behavior == new grid-powered behavior for the
same on/off sequence. Then delete the independent state.

## 7.6 23A Phase E — Greenhouse & grow lamps
The greenhouse already consumes `room_greenhouse` (§1.3). Verify the *authored
light curve* is the single source of grow-light-to-yield effect
(`GreenhouseSystem.TickDay(currentDay, growLightHours, ashContaminationRate)`), that
`AgricultureSystem` derives hours from power and not from a second table, and that
no doc-comment promises a power constraint that is not implemented. No greenhouse-
specific power penalty may be invented.

## 7.7 23A Phase F — Water treatment
- Map treatment stages to powered/passive (`WaterTreatmentSystem.cs:645-670`
  already pauses treatment at `power01 <= 0`).
- Decide the canonical power room: currently `room_water_pump`; `room_water_filtration`
  is a separate critical catalog room with no reader (§1.6). Either bind treatment
  to both rooms with authored weights, or collapse to one room and reconcile the
  catalog. Document the choice.
- Preserve the passive/manual survival path; blackout must not equal instant thirst.
- Surface "treatment reduced because the plant is unpowered" in the water panel and
  the briefing.

## 7.8 23A Phase G — Air filtration & ventilation
- Gate the **mechanical** ventilation/filtration effect on `room_air_filtration`
  served state (currently un-gated, §1.6).
- Route filtration/ventilation state into the Plan 20B atmosphere/shielding model.
  The grid adds no dose; the atmosphere model computes consequences.
- Data-author a residual-function grace window for filters/ducts.
- Keep the electrostatic stage's existing `IsRoomPowered(stage.roomId)` gate.

## 7.9 23A Phase H — Cold storage
- 22B provides cellar/preservation; `FoodPreservationSystem` owns spoilage and
  already accepts `SetPowerStatus` + `SetStorageTemperatureC`.
- Replace the host's global `!IsBrownout` (`Main.Plans62_65.cs:155`) with
  allocation-aware reads for the storage room (and the cryo tier on
  `room_cryo_vault`).
- Thermal mass: the existing `StorageTempShelfLifeFactor` + host-projected
  storage °C already model slow warm/cool; extend with a data-defined grace.
- Power return resumes refrigeration; already-spoiled food does not recover.

## 7.10 23A Phase I — Medical ward
- Clinic and quarantine already consult the grid (§1.3). Verify the pipeline's
  power check **fails reservations explicitly with a power reason** rather than
  silently queuing or dropping them.
- Fix the dead `room_ward_clinical` charger gate to a real room and test it.
- 22C cold chain: the grid changes storage capability; medicine logic is unchanged.

## 7.11 23A Phase J — Heating
- Wire `room_heating`: fans/pumps/electrical circulation require power.
- Wire `SetGeneratorWasteHeat(generatorWatts, pumpActive)` to the running game
  (currently test-only, §1.6), with `pumpActive` derived from served
  `room_heating` power — this creates the intended "fuel heat without circulation
  power is wasted" conflict.
- Separate combustible stock (fuel) from electricity; never conflate them.

## 7.12 23A Phase K — Total draw and deficit read model
Expose a Core read model consumed by panel and briefing:

```text
generation W, demand W, available W (generation + sustainable battery discharge),
deficit W, served W, unserved W, shed tier/rooms, forecast runtime
```

Panels must not re-derive any of these.

## 7.13 23A Phase L — Data-author loads
Extend `power_grid.json` (additive) and/or a consumer catalog with:

```text
consumer_id, room_id, draw_watts, criticality_class,
grace_hours, restart_cost, shed_priority
```

Validate unique ids, valid room ids, non-negative draw, valid class, valid
item/resource ids, deterministic ordering. Extend the existing loader/integrity
hooks; do not add a second validator.

## 7.14 23A Tests
Minimum (focused, one file or one directory at a time):
registration determinism; duplicate registration fails; per-consumer loss and
restoration behavior; private-flag equivalence; greenhouse light/yield dependency;
water throughput dependency; mechanical filtration dependency; refrigeration
(cellar) dependency; medical reservation failure reason; heating/waste-heat
dependency; total draw/deficit equality with `TickDay`; save round-trip of
consumer-specific persistent state; brownout ordering determinism; dead-room-id
regression (charger powered when the real ward is served).

## 7.15 23A Definition of Done
- [ ] load contract published; DECORATIVE rows explicitly identified
- [ ] one consumer abstraction (only where needed); one registration seam
- [ ] salt-mine + tube-network private flags removed
- [ ] dead `room_ward_clinical` fixed and tested
- [ ] mechanical filtration gated; heating circulation gated; generator waste heat wired
- [ ] water treatment stage mapping decided and documented
- [ ] food preservation allocation-aware (storage + cryo)
- [ ] medical reservations surface power reason; 22C seam respected
- [ ] deficit/read model exposed; panels consume it
- [ ] loads data-authored and validated; deterministic ordering
- [ ] save/load stable; no direct duplicate power models

---

# 8. Workstream 23B — Load Shedding as a Player Decision

## 8.1 Objective
The existing grid screen becomes a complete decision screen: room and class
shedding, honest automatic fallback, battery strategy, sump clock, breaker cost,
fuel-claim legibility — all rendering canonical Core numbers.

## 8.2 23B Phase A — Criticality class data (no new enum)
Author the `criticality_class` vocabulary (`life_support`, `pumping`, `medical`,
`production`, `comfort`) in data and map it onto the existing 4-value simulation
enum. Display order must derive from the same mapping — never a second hardcoded
order. If a true 5-value simulation tier is required, that is **Decision D-2** and
must include a save-compatibility plan before implementation.

## 8.3 23B Phase B — Player room/class controls
Already present: breaker toggle, priority picker, emergency preset, restore
defaults (`PowerGridPanel.cs`). Add class-level shed/restore that maps to authored
`criticality_class`, and an action cost where the design requires duty — routed
through Plan 24 labor/roster authority, never a power-specific labor counter.
Shedding must be reversible unless a hardware failure prevents it.

## 8.4 23B Phase C — Automatic fallback shedding + attribution
`ComputeAllocation` already sheds a deterministic suffix. Add:
- a semantic distinction in the day-event/briefing layer between grid-automatic
  shed and player-initiated shed;
- briefing copy that names *who* shed and *what* was shed.

Example semantics: `The grid shed workshop power automatically.` vs
`You shut down the workshop.`

## 8.5 23B Phase D — Sump consequence legibility
`SumpFloodingSystem` already owns level, inflow, pump served state, flooding and
contamination consequences. Add a read model: current level, rate of rise,
estimated time to flood/damage threshold — computed by the same Core path as the
runtime. No duplicate flood logic. Surface in the sump panel and the briefing.

## 8.6 23B Phase E — Schedule consequences
`ShelterScheduleSystem` already reads `IsBrownout`. Route unpowered-period effects
on work/study/rest into Plan 24 systems. No morale penalty inside power code.

## 8.7 23B Phase F — Battery strategy
Expose capacity, discharge rate, recharge rate, depth-of-discharge wear and
condition (only if a real wear authority exists or is approved). Add a canonical
**estimated runtime at current load** computation on `PowerGridSystem` (or a Core
read model built from it) and render it. No panel-side arithmetic.

## 8.8 23B Phase G — Brownout presentation
Brownout must be visible on existing surfaces: power panel, room status, lighting
accents, and audio. Coordinate with Plan 17C for generator/vent loops, alert
ducking and exactly-once cues. `ShelterAudioController` already follows brownout
and `OnTickSummary`; extend rather than fork.

## 8.9 23B Phase H — Breaker recovery cost
After an overload trip, reset requires a shift/action and (where designed) an
authored part/material through the existing Plan 22 consumption authority
(`machine_oil` is already the generator/subgrid service item). No free reset spam.

## 8.10 23B Phase I — Fuel chain legibility
One surface exposes competing fuel claims (generator, vehicles, foundry, heating).
Do not move those systems into the grid; show their consumption/claims so the
conflict is visible.

## 8.11 23B Phase J — Generator repair/expansion
Only wire expansion controls where real authorities exist (nuclear, geothermal ORC,
SOFC, solar concentrator, kinetic storage, battery banks, coated parts — all
already publish generation or install through existing sessions). Apply the
"authority first, buttons second" rule. No fake expansion interactions.

## 8.12 23B UI read model fields
`generation, battery state, total demand, deficit, current shed class,
room allocations, room criticality class, grace timers, predicted battery runtime,
fuel runway, breaker status`. Reuse `PowerGridPanel` style and routes; no new console.

## 8.13 23B Tests
manual shed room; shed by class; restore room; automatic shed priority;
automatic-vs-player attribution; sump clock matches runtime; battery endurance and
recharge; breaker trip + reset cost; save/load priorities and battery state;
snapshot each class state; audio state parity (exactly-once transitions).

## 8.14 23B Balance program
Sweep `winter severity × storm frequency × fuel supply × battery capacity × load
demand × player shedding strategy`. Acceptance: blackout is dangerous; early
blackout is recoverable; prioritization matters; battery is valuable; fuel scarcity
creates tradeoffs; one mistake is not automatic campaign death. Resolve **D-3**
(the day-one structural deficit) as part of this program.

## 8.15 23B Definition of Done
- [ ] criticality classes authored and mapped (D-2 decision recorded)
- [ ] per-room and per-class shedding player-controlled; reversible
- [ ] automatic fallback attributable and distinguishable from player action
- [ ] sump consequence/clock visible from canonical math
- [ ] schedule consequences feed Plan 24 (no power-owned morale)
- [ ] battery runtime forecast canonical; no panel arithmetic
- [ ] brownout visually/audio legible; exactly-once
- [ ] breaker recovery costs resources/labor
- [ ] fuel competition visible
- [ ] generator expansion only where authoritative
- [ ] balance sweep passes, D-3 resolved

---

# 9. Workstream 23C — Recoverable Failure Cascades

## 9.1 Objective
Compose existing subsystem failures into emergent situations without scripting
them, so the player can trace:

```text
brownout → pump loss → rising sump → damaged infrastructure → contamination → illness
```

and still have a legal path to recover.

## 9.2 23C Phase A — Define input facts
Inventory existing facts and their owners (all already exist — no new state):

| Fact | Owner | Read surface |
|---|---|---|
| sustained brownout / deficit | `PowerGridSystem` | `IsBrownout`, `OnTickSummary` |
| breaker trip / surge | `PowerGridSystem` | `PowerGridEvent` |
| sump level / flooded | `SumpFloodingSystem` | `State.nodes`, `OnIncident` |
| room temperature | `ShelterThermalSystem` | `State.rooms` |
| filtration state | `VentilationSystem`/`StartingLevelSystem` | state + band |
| radiation / interior shielding | `RadiationSystem` + 20B model | `ExposureEnvironment` |
| hatch/unseal | `AirlockSecuritySystem` | `State.doorState` |
| fire | `ShelterFireHazardSystem` | incidents |
| outbreak | `DiseaseSystem` | active outbreaks |
| contamination | `SumpFloodingSystem`, inventory | state |
| lighting loss | `room_lighting_main` served state | read model |
| pump served state | `PowerGridSystem` | `IsRoomServed` |

## 9.3 23C Phase B — `CascadeCoordinator`
Create `Assets/Ashfall.Core/Shelter/CascadeCoordinator.cs` (or equivalent). It
evaluates existing facts against data-authored rules, holds rule progress /
warning / escalation / attribution state only (and only when persistence is
needed), and never rewrites another subsystem's fields. It must not replace
`CrisisPresentationCoordinator`; it should feed it and the briefing.

## 9.4 23C Phase C — Data-authored cascade rules
Reuse the existing condition/flag machinery where it can express the rule; do not
invent a second expression language. Conceptual shape:

```json
{
  "id": "cascade_brownout_winter_pipe_risk",
  "when": [ ...existing condition grammar... ],
  "warning_hours": 24,
  "next_effect": "...",
  "off_ramps": [ "...", "..." ]
}
```

Example semantic chains: brownout + winter + heating dependency → thermal drop;
thermal drop + rising sump → pipe burst risk; ventilation down + fallout storm →
interior dose pressure; cold + darkness + fatigue → maintenance-incident risk.

## 9.5 23C Phase D — Recovery off-ramps
Every stage declares at least one recovery path (restore power, manual pump, shed
another load, consume a part, assign labor, seal hatch, decontaminate, treat
outbreak). Integrity validation fails a reachable cascade with no legal off-ramp
where statically determinable.

## 9.6 23C Phase E — Warning windows
Every severe cascade authors a minimum warning period, escalation period and
threshold. Reject zero-warning catastrophic chains unless explicitly reviewed and
exceptional. The player must see risk before irreversible loss.

## 9.7 23C Phase F — Human consequences
Route to existing systems: cold → sleep quality / morale (Plan 24); contaminated
water → disease vectors (`DiseaseSystem`); darkness + fatigue → incident risk;
unresolved power stress → Plan 24C. **No `PowerStressSystem`.**

## 9.8 23C Phase G — Attributable briefing chain
Emit one semantic line per stage so the player can trace backwards:

```text
Power shed from pumping.
Sump level rose.
Cold storage warmed.
Water quality fell.
Two survivors became ill.
```

Use the Plan 31/17 vocabulary (`DayEventVocabulary`, parity matrix, "no silent
drops"). Add explicit cases rather than relying on the generic renderer for
player-critical power/cascade facts.

## 9.9 23C Phase H — Shelter "strain" readout
Add a compact readout to an existing shelter/power surface showing active
stressors, the next likely consequence and warning time, using the same rule
evaluator as simulation. No second forecast system.

## 9.10 23C Phase I — Performance budget
Evaluate daily over already-computed state; never per frame or per UI refresh.
Current day-advance 30d median is 0.737 s. Acceptance: cascade evaluation stays
within the day-advance budget, no allocation churn, deterministic fixed order.

## 9.11 23C Phase J — Determinism
Rules evaluate in stable ordinal order. Any probability uses `ISeededRng` with a
campaign stream id; same seed → same cascade path. No dictionary-order or
wall-clock dependence.

## 9.12 23C Phase K — Worst-winter scenario
Seeded integration scenario: winter + repeated storms + constrained fuel + high
load + one subsystem failure. Assert multiple cascading pressures occur, warning
stages appear, at least one legal recovery action remains, and the campaign is not
forced into unavoidable loss.

## 9.13 23C Reachability QA
Every rule must be reachable in a test or seeded playthrough, observable, and
recoverable. No dead rules. Docs record trigger, stage sequence, off-ramp and
affected systems.

## 9.14 23C Tests
Per cascade: trigger, warning, escalation, effect, off-ramp, recovery. Global:
no off-ramp-less reachable cascade; deterministic order; same-seed replay;
worst-winter legal action; performance budget; save/load mid-cascade; briefing
attribution.

## 9.15 23C Definition of Done
- [ ] coordinator reads existing facts only; never writes subsystem fields
- [ ] cascades data-authored; no scripted sequences
- [ ] every cascade has an off-ramp; integrity gate rejects otherwise
- [ ] minimum warning enforced
- [ ] human consequences routed to existing systems (no `PowerStressSystem`)
- [ ] briefing traces stages; parity matrix updated
- [ ] strain readout uses the canonical evaluator
- [ ] daily performance within budget; stable allocation
- [ ] deterministic replay; save/load mid-cascade stable
- [ ] worst-winter remains recoverable; all cascades reachable in QA

---

# 10. Integrated Power Pipeline

```text
Fuel / generator / battery / external generation
        │  (nuclear, ORC, SOFC, solar, kinetic, RTG, coated parts)
        ▼
PowerGridSystem
        ├─ generation  ├─ battery  ├─ room allocation (served/shed)
        ├─ shed state  ├─ deficit  ─ breaker / surge / generator condition
        │
        ▼
Central consumer registration (one seam)
        ├─ water treatment        ├─ mechanical ventilation/filtration
        ├─ greenhouse/agriculture ├─ preservation/cellar + cryo vault
        ├─ clinic / quarantine    ├─ heating circulation + waste heat
        ├─ sump pumps             ├─ foundry/extraction/armory
        └─ other legitimate loads
        │
        ▼
Subsystem-specific effects (owning systems only)
        ├─ throughput  ├─ temperature  ├─ contamination  ├─ spoilage
        ├─ reservation availability    ├─ schedule disruption  └─ flooding
        │
        ▼
CascadeCoordinator  (reads facts → evaluates authored rules → warning/off-ramp → stages)
        │
        ▼
Briefing / UI / audio / human consequences (existing owners + Plan 24)
```

---

# 11. Ownership Matrix (publish at closure)

| Fact | Authority |
|---|---|
| available generation (base + external) | `PowerGridSystem` |
| battery charge | `PowerGridSystem` |
| room power / served-shed | `PowerGridSystem` |
| water throughput | `WaterTreatmentSystem` |
| indoor atmosphere / shielding | Plan 20B model |
| refrigeration / cellar spoilage | `FoodPreservationSystem` |
| cryo samples | `CryoVaultSystem` |
| pharma storage | 22C (`DoseLedgerSystem` / treatment authority) |
| shelter temperature | `ShelterThermalSystem` |
| sump level / flooding | `SumpFloodingSystem` |
| disease | `DiseaseSystem` |
| human schedule / stress | Plan 24 systems |
| cascade risk / warning state | `CascadeCoordinator` |
| cascade source facts | owning subsystem (never mirrored) |

The coordinator never replaces these owners.

---

# 12. Exactly-One-Authority Rules

- **Power availability:** only `PowerGridSystem`.
- **Consumer functional state:** each subsystem owns it; it may cache a derived
  power input.
- **Cascades:** the coordinator owns rule progress, warning/escalation state and
  attribution metadata only where persistence is needed. It does not mirror every
  subsystem value.
- **Presentation:** panels and the crisis presenter render canonical numbers; they
  never re-derive.

---

# 13. Save / Load Contract

Persist only where necessary:
power grid state, battery state, room priorities/overrides, breaker/surge
incidents, consumer-specific degradation already owned by each subsystem, and
cascade warning/escalation state if it spans days.

## 13.1 Restore order (conceptual, using actual composition)
```text
resource/fuel → power grid → consumers → environmental systems → cascade coordinator → UI
```

## 13.2 No replay side effects
Loading must not re-trigger power-lost callbacks as new incidents unless a state
transition actually occurs, consume restart parts, duplicate cascade stage events,
or replay historical brownout audio. `PowerGridSystem.RestoreState` already
re-seeds brownout and source-degradation latches (`PowerGridSystem.cs`); new
cascade state must do the same.

## 13.3 Save sections touched
- existing: `power_grid`, `power_subgrids`, generation-source sections, `food_preservation`;
- new only if cascade state spans days: one cascade section through the existing
  registry (`SaveSectionRegistry.cs`), never a parallel store.

---

# 14. Semantic Event Integration

Use canonical kinds and the parity matrix. Candidate categories:

```text
power_deficit, power_shed_automatic, power_shed_player, power_breaker_tripped,
power_room_lost, power_room_restored, sump_risk, preservation_degraded,
filtration_degraded, cascade_warning, cascade_escalation, cascade_recovered
```

Do not invent raw strings outside the semantic-kind authority. Add explicit
`DailyBriefingReportBuilder` cases (following the 17A-S no-silent-drop pattern)
rather than relying on the generic fallback for player-critical power facts.
`power_ticked` stays an internal heartbeat.

---

# 15. Audio Integration

Existing `ShelterAudioController` already tracks generator running, brownout, and
ventilation loops (`ShelterAudioController.cs:92-174`). Coordinate with Plan 17C:

- generator loop follows live generation/fuel,
- ventilation loop follows actual power/ventilation state,
- brownout/overload/cascade-warning cues obey concurrency, ducking, and
  exactly-once rules (`ShelterPowerRestore`, `GeneratorBrownoutFlicker`, etc.).

Audio remains observational; no audio→gameplay edge.

---

# 16. UI Integration

Reuse existing power/shelter surfaces. Required player answers:

**Power panel**
```text
How much power do I have?  How much is demanded?
What is shed, and who shed it?  How long will the battery last?
What does restoring this room cost?
```

**Shelter surface**
```text
What failure is approaching next, and how long do I have?
```

**Briefing**
```text
What did I shut down?  What did the grid shut down?  What broke because of it?
```

No new console. Preserve keyboard/controller close/back, focus, contrast, and
refresh/disposal lifecycle.

---

# 17. Balance Program

Seeded sweeps across `storm frequency × winter severity × fuel supply × generator
capacity × battery capacity × consumer load × player shedding strategy`. Track
blackout hours, breaker trips, sump flood events, cold-storage failures, filtration
failures, treatment interruptions, cascade frequency, recovery success, cascade-
attributable deaths, fuel runway.

Acceptance: power matters; prioritization matters; blackouts are bad; early
blackouts are recoverable; late-game poor planning can become severe; no arbitrary
unwinnable spiral. **D-3 must be resolved here.**

---

# 18. Performance Program

Measure day advance before, after 23A, after 23C. Check consumer registration has
no per-day allocations, cascade evaluation is O(rules), no per-frame evaluation,
and lookups are by id. Current baselines: 30d median 0.737 s; 180d 4.627 s; 360d
11.236 s; 30d alloc 363,056 B. Record results in `artifacts/runtime-scale-results.json`
if repository workflow expects it.

---

# 19. Failure Modes and Corrective Actions

| Mode | Cause | Fix |
|---|---|---|
| Water runs during blackout | plant not gated, or fallback bypasses grid | route powered stages through consumer/allocated state |
| Filtration stops but dose unchanged | duplicate atmosphere model | feed 20B model; no local radiation math |
| Refrigerator warms instantly | no thermal-mass grace | use 22B storage dynamics |
| Medical action silently fails | reservation pipeline hides power prerequisite | explicit unavailable reason |
| Private `isPowered` returns | subsystem reintroduces local authority | source/ownership gate + contract doc |
| Automatic shed feels like player choice | attribution missing | semantic actor/source metadata |
| Cascade has no escape | rule has no legal off-ramp | reject rule / add off-ramp |
| Cascade evaluator writes subsystem fields | coordinator became god-object | call existing actions/events; evaluate risk only |
| Power UI predicts different runtime | panel-side arithmetic | shared Core estimate |
| Charger permanently off | dead room id | fix to real room + regression test |

---

# 20. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| too many fake dependencies | Medium | Medium | DECORATIVE classification |
| one system keeps a private power flag | Medium | High | source sweep + ownership doc |
| 20B atmosphere duplication | Medium | High | single model agreement |
| refrigeration integration before 22B | High if mis-sequenced | Medium | prerequisite gate |
| load order nondeterministic | Low–Med | Medium | stable registration/ordering |
| auto shed feels arbitrary | Medium | Medium | visible priorities + attribution |
| battery/spread too strong or weak | Medium | Medium | balance sweep; D-3 |
| cascades become scripted | Medium | High | fact-driven rules only |
| cascade death spiral | Medium | Critical | off-ramp + warning tests |
| day-advance regression | Medium | Medium | daily-only evaluator + budget |
| alert spam | Medium | Low–Med | 17C concurrency rules |
| save/load replays incidents | Medium | Medium | restore-side-effect tests |
| 4→5 tier save churn | Medium | High | D-2 mapping decision before code |

---

# 21. Package / Commit Strategy

| Package | Scope | Gate |
|---|---|---|
| `C2-6-23A-CONTRACT` | load table doc, DECORATIVE classification, baseline capture | contract published |
| `C2-6-23A-PRIVATE-FLAGS` | salt-mine + pneumatic flags removed; equivalence tests | 23A-D |
| `C2-6-23A-DEFECT-FIXES` | dead room id; mechanical filtration gate; heating/waste-heat; water stage mapping | 23A E–J |
| `C2-6-23A-ALLOCATION` | preservation/Hydroponic/medical reads allocation-aware; deficit read model; data-authored loads | 23A K–L |
| `C2-6-23A-CLOSURE` | tests + save round-trip + determinism | **23A complete** |
| `C2-6-23B-CLASSES` | criticality_class data mapping (D-2) + class controls | 23B A–B |
| `C2-6-23B-ATTRIBUTION` | automatic-vs-player shed events + briefing | 23B C |
| `C2-6-23B-READ-MODELS` | sump clock, battery runtime, fuel runway, deficit UI | 23B D–F |
| `C2-6-23B-PRESENTATION` | brownout UI/audio, breaker reset cost, snapshots | 23B G–I |
| `C2-6-23B-BALANCE` | sweep + D-3 resolution | **23B complete** |
| `C2-6-23C-COORDINATOR` | `CascadeCoordinator` + rule schema + input facts; deterministic order | 23C A–C |
| `C2-6-23C-VALIDATION` | off-ramp + warning-floor integrity tests | 23C D–E |
| `C2-6-23C-CONSEQUENCES` | human-consequence seams + briefing attribution | 23C F–G |
| `C2-6-23C-STRAIN-PERF` | strain readout + performance budget | 23C H–I |
| `C2-6-23C-WORST-WINTER` | seeded scenario + reachability QA + docs | **23C complete** |
| `C2-6-CLOSURE` | save/load, replay, verify-fast, balance/perf report | full pipeline |

Follow the repository cadence: one owned system per package, host/save follows
Core, and update `INTEGRATION_PLANS.md` / `WORKTREE_OWNERSHIP.md` only as foreman
or named integrator.

---

# 22. Verification Checklist

Per major workstream and at final closure:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --audio-selftest
godot --headless --path . -- --panel-bind-lifecycle-selftest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Plus: balance sweep, day-advance performance probe, a seeded worst-winter cascade
scenario, and `docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md`.

---

# 23. Flagship Definition of Done

## 23A — One authority
- [ ] load contract complete; DECORATIVE rows identified
- [ ] one consumer abstraction; one registration seam; deterministic order
- [ ] no private power authority remains (salt mine, pneumatic network)
- [ ] dead `room_ward_clinical` fixed
- [ ] mechanical filtration, heating circulation, generator waste heat gated
- [ ] water treatment, preservation/cryo, greenhouse, medical allocation-aware
- [ ] total demand vs supply + deficit visible; loads data-authored/validated
- [ ] save/load stable; no direct duplicate power models

## 23B — Player shedding
- [ ] criticality classes authored and mapped (D-2 recorded)
- [ ] room + class controls player-driven and reversible
- [ ] auto-shed only fallback, attributed distinctly from player shed
- [ ] sump risk/clock visible; schedule consequences feed Plan 24
- [ ] battery strategy/runtime canonical; brownout visible in UI+audio
- [ ] breaker recovery costs; fuel competition legible
- [ ] expansion only where authoritative; balance recoverable (D-3 resolved)

## 23C — Cascades
- [ ] coordinator reads existing facts only; rules data-authored; no scripts
- [ ] every cascade has an off-ramp; every severe cascade warns
- [ ] human consequences route to existing systems (no new stress system)
- [ ] semantic stages recorded; strain readout uses canonical evaluator
- [ ] daily performance within budget; same-seed deterministic
- [ ] worst-winter retains a legal recovery action; save/load mid-cascade stable

## Cross-system
- [ ] one `PowerGridSystem`; one atmosphere model; one preservation model
- [ ] one human consequence path; no fake dependencies
- [ ] no consumer-specific power authority; full verification green

---

# 24. Closure Report Template

```markdown
## C2[6] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:
- Working tree:

### Baseline
- Grid rooms / tier split:
- Total modeled draw / generation / battery:
- Day-one deficit:
- Private power flags:
- Dead room ids:
- Day-advance 30d median:
- Audio selftest:

### 23A — One Authority
- Load contract:
- Consumer interface:
- Registration seam:
- Private flags removed:
- Defect fixes (charger / filtration / heating / water stage):
- Allocation-aware reads:
- Total draw/deficit read model:
- Save/load:
- Determinism:
- Result:

### 23B — Load Shedding
- Criticality classes (D-2):
- Room/class controls:
- Automatic shed + attribution:
- Sump clock:
- Schedule seam:
- Battery runtime:
- Brownout UI/audio:
- Breaker recovery:
- Fuel chain:
- Balance (D-3):
- Result:

### 23C — Cascades
- Coordinator:
- Rule catalog:
- Off-ramp validator:
- Warning floor:
- Human consequences:
- Briefing:
- Strain readout:
- Performance:
- Worst-winter:
- Determinism:
- Save/load:
- Result:

### Full Verification
- Core build / tests:
- Host build:
- Data integrity / bridge / audio / panel lifecycle:
- Triad drift / verify fast:
- Balance sim / day-advance perf:

### Final Metrics
- Powered consumers:
- DECORATIVE exclusions:
- Private flags remaining:
- Avg blackout hours:
- Breaker trips:
- Cascade count:
- Unrecoverable cascade count:
- Day-advance delta:

### Remaining Debt
- 20B:
- 22B / 22C:
- 24:
- Power:
- Cascades:
```

---

# 25. Final Execution Directive

Implement Plan 23 as a **dependency-continuity repair**, not a new grid and not a
cosmetic wiring pass.

Do not add fake electrical requirements to make the system look interconnected.
Do not re-build anything already present (§1). Do not fix a premise that is already
true.

The critical chain is:

```text
declare real loads
→ register them once
→ make grid availability authoritative (allocation-aware)
→ expose deficit
→ let the player shed load (by room and class)
→ attribute automatic fallback
→ let existing systems fail honestly
→ compose those failures into recoverable cascades
```

The work is not complete when the power panel shows watt numbers, consumers
implement an interface, rooms can be toggled, or a cascade rule file exists.

It is complete when the bunker behaves like one machine:

```text
the generator loses capacity
→ the player decides what goes dark
→ those rooms actually lose function
→ secondary systems react
→ the briefing explains the chain, and who caused each step
→ the player still has a meaningful recovery path
```

The strongest architectural rule:

> **Every watt is accounted for by one power authority, and no subsystem keeps an
> independent opinion about whether electricity exists.**

The strongest gameplay rule:

> **Every severe power cascade gives the player warning, attribution, and at least
> one real way out.**

---

# Appendix A — Premise Corrections (source Plan 23 → current source)

| Source Plan 23 claim | Current reality | Action |
|---|---|---|
| "water treatment continues without power" | gated via `room_water_pump` host feed | verify stage mapping only |
| "air filtration and ventilation continue during blackout" | **partly true**: mechanical tick un-gated; electrostatic stage gated | gate mechanical filtration |
| "greenhouse grow lamps do not draw watts" | **false**: `room_greenhouse` load + `AgricultureSystem` hours | verify one curve source |
| "refrigeration does not depend on the grid" | host feeds `SetPowerStatus` from global brownout | make allocation-aware |
| "medical procedures ignore power" | clinic + quarantine power checks exist | verify honest failure reason |
| "heating pumps/fans ignore the grid" | **true**: `TickDay(day)`, waste-heat setter unrouted | wire it |
| "some systems carry their own private power booleans" | true for salt mine + pneumatic network; hydroponic is a derived cache | remove the two authorities |
| "sump is already a real power consumer" | true and allocation-aware | add clock/read model |
| "generator audio already reacts to live grid state" | true (`ShelterAudioController`) | extend to cascade cues |
| "five-tier vocabulary" | **does not exist**; authority is 4-valued | Decision D-1/D-2 |
| "total draw can exceed supply" | true, and day-one default is ~3× oversubscribed | Decision D-3 |
| "`LibraryStudySystem.requiresPower`" is a private flag | it is authored per-manual data, currently unread | wire as a real gate (or mark dead) |
| "cascade coordinator does not exist" | true; `CrisisPresentationCoordinator` is presentation-only | build behind it |
| "player load shedding not exposed" | **false**: breakers, priorities, preset, per-room served/shed exist | add classes, attribution, read models |

# Appendix B — Open Decisions (require foreman/user sign-off)

- **D-1 — Tier vocabulary.** The source five-tier vocabulary (life support /
  pumping / medical / production / comfort) does not exist. Choose: (a) authored
  `criticality_class` mapped onto the existing 4-value enum with data-derived
  display order (no save change — recommended), or (b) extend the enum with a save
  migration plan.
- **D-2 — Save compatibility for tiers.** Only relevant if D-1(b); must land with
  an envelope version and old-save defaults before any consumer uses it.
- **D-3 — Day-one structural deficit.** 2910 W demand vs ~967 W supply means the
  default campaign opens in permanent shedding. Decide intended pressure vs
  rebalance (generation, draws, or a start-up load subset) during 23B balance.
- **D-4 — Water room authority.** Decide whether treatment binds to
  `room_water_pump`, `room_water_filtration`, or both with authored weights, and
  reconcile the unused catalog row.
- **D-5 — Salt-mine and tube-network power route.** Decide their canonical room /
  dynamic-load id and whether their `powerDraw` is charged to the grid or the
  systems are reclassified.
- **D-6 — Cascade rule expression.** Decide whether the existing condition/flag
  grammar can express cascade rules or a minimal additive schema is required.
- **D-7 — `room_ward_clinical` correction target.** Choose the canonical room
  (`room_ward_quarantine` preferred for consistency with the disease isolation
  check) and pin it with a regression test.
---

# 26. Execution Checkpoint — 23A continuity repairs (2026-09-15)

Status: **23A partially implemented and verified at Core+host+panel scope.**
Package scope was chosen to avoid the live `C1-PLAN22-ONE-FOOD-AUTHORITY` claim
(`KitchenNutritionSystem`, `Inventory`, kitchen panels) and the in-flight
`DailyBriefingReportBuilder` / `Main.CampaignOwners` shared edits owned by other
agents. No shared/claimed path was touched.

## Implemented packages

| Item | Change | Path |
|---|---|---|
| Dead room id | bionic charger gate `room_ward_clinical` → `room_ward_quarantine` | `src/Main.Bionics.cs` |
| Mechanical filtration load | `TickDay(..., bool mechanicalPowerAvailable)` gates forced exhaust + filter efficiency (default true = legacy) | `Assets/Ashfall.Core/VentilationSystem.cs` |
| Filtration host feed | passes `IsRoomServed("room_air_filtration")` | `src/Main.ExpandedShelterSystems.cs` |
| Heating circulation | generator waste heat + circulation pump now driven by served `room_heating` | `src/Main.ExpandedShelterSystems.cs` |
| Water filtration load | treatment spans `room_water_pump` + `room_water_filtration`, allocation-aware, partial on one bus | `src/Main.ExpandedShelterSystems.cs` |
| Hydroponic power | global `!IsBrownout` → `IsRoomServed("room_greenhouse")` | `src/Main.AdvancedShelterSystems.cs` |
| Food preservation power | global `!IsBrownout` → `IsRoomServed("room_kitchen")` (cellar/refrigeration bus) | `src/Main.Plans62_65.cs` |
| Pneumatic network authority | blackout now derived from served foundry/workshop bus each tick | `src/Main.Plans74_77.cs` |
| Salt mine authority | `SetPower` now fed from served foundry/workshop bus | `src/Foundry/SilentFoundryHostSession.cs` |
| Library authored power | dead `requires_power` data now gates `StartStudy` + progress | `Assets/Ashfall.Core/LibraryStudySystem.cs`, `src/Main.ShelterBatch3.cs` |
| Comms array power | global `!IsBrownout` → `IsRoomServed("room_radio_tuner")` | `src/Main.Plans198_201.cs` |
| Deficit/runtime read model | `AvailableSupplyWatts`, `DeficitWatts`, `EstimatedRuntimeHours`, `EstimatedFuelRunwayDays` (canonical, no panel arithmetic) | `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` |
| Panel rendering | deficit/supply/battery-ETA/fuel-runway line | `src/UI/PowerGridPanel.cs` |

## Decisions taken

- **D-4 (water authority):** treatment binds to both `room_water_pump` and
  `room_water_filtration` (filtration is now load-bearing; one served bus = 0.5).
- **D-5 (mine/tube bus):** both share the foundry/workshop electrical bus
  (`room_foundry` ∨ `room_workshop`), fed from the canonical allocation.
- **D-7 (charger room):** `room_ward_quarantine` (consistent with the disease
  isolation check).
- **Abstraction decision:** no new `IPowerConsumer` interface introduced. Every
  repaired consumer uses the existing delivered-value/provider pattern
  (`IsRoomServed` / `TickDay(..., power)`), which is the minimum shape the plan
  permits. A god-object interface would add no behaviour.

## Verification (all green)

- `dotnet build Ashfall.Core/Ashfall.Core.csproj` — 0 warnings, 0 errors
- `dotnet build Ashfall.csproj` — 0 errors
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Plan23APowerContinuityTests.cs` — **8/8** (new)
- Adjacent Core regressions: `LibraryStudySystemTests` 8/8 ·
  `PowerGridSystemTests` 20/20 · `PowerGridPhase2AllocationTests` 14/14 ·
  `PowerGridPhase5MaintenanceTests` 9/9 · `VentilationSystemTests` 7/7 ·
  `VentilationElectrostaticIntegrationTests` 5/5 ·
  `ShelterThermalThermodynamicsPlan57Tests` 8/8 · `ShelterThermalSystemTests` 13/13 ·
  `WaterTreatmentSumpBridgeTests` 4/4 · `Phase3WaterIntegrationTests` 10/10 ·
  `DeepWellSystemTests` 10/10
- `godot --headless --path . -- --data-integrity-selftest` — PASS (330 catalogs, 0 errors)
- `godot --headless --path . -- --bridge-selftest` — PASS
- `godot --headless --path . -- --panel-bind-lifecycle-selftest` — PASS (17/17)

## Not yet implemented (remaining plan)

- **23A remaining:** `room_water_filtration`/pump data reconciliation; medical
  reservation reason is already implemented (`clinic_no_power` — verified, no
  change needed); data-authored consumer catalog + integrity validation;
  `SetupPowerConsumers()` central registration (deferred — existing per-system
  registration is deterministic).
- **23B:** criticality-class data mapping (D-1/D-2); automatic-vs-player shed
  attribution in the briefing (blocked by shared
  `DailyBriefingReportBuilder`/`Main.CampaignOwners` edits owned by another
  agent); sump rising-water clock; breaker reset cost; fuel-claim screen;
  class-state snapshots; balance sweep and D-3 resolution.
- **23C:** `CascadeCoordinator` + data-authored rules + off-ramp/warning
  integrity gate + strain readout + worst-winter scenario. Not started.
- **D-1/D-2/D-3/D-6** remain open decisions requiring foreman sign-off (see
  Appendix B).

## Remaining debt introduced

- The pneumatic panel still exposes a manual "TOGGLE BLACKOUT" action whose
  mutation is overwritten by the grid each tick; the button should be relabelled
  or removed in a UI package.
- `room_kitchen` is used as the preservation power bus because
  `room_storage_bay` is not a grid room; a dedicated cold-storage room/catalog
  row is the durable fix (needs data decision).

---

# 27. Execution Checkpoint — 23B + 23C (2026-09-15)

Status: **23B implemented at Core+host+panel scope; 23C implemented at
Core+host+briefing scope.** All focused/new tests and all Godot selftests that
touch this work are green. One unrelated drift gate (`save_store_matrix_drift`)
is stale from a concurrent agent's in-flight Plan 22 save work — no save store
was added by this plan.

## 23B — implemented

| Item | Change | Path |
|---|---|---|
| Breaker trip recovery (defect) | overload/surge-tripped rooms were **unrecoverable** — `ToggleBreaker` never cleared a trip. Added `PowerGridHostSession.ClearTripped`, a panel `RESET` action, and a host route that consumes `machine_oil` before commit. | `src/Host/PowerGridHostSession.cs`, `src/UI/PowerGridPanel.cs`, `src/Main.World.cs` |
| Player-shed attribution | breaker-off by the player is recorded and drained by the power day owner as `power_shed_player`. | `src/Main.World.cs` |
| Automatic-shed attribution | `PowerGridTickSummary.ShedRoomIds` / `HasCriticalDeficit` / `BrownoutBegan` / `BrownoutEnded` become `power_shed_automatic`, `power_critical_deficit`, `power_brownout_began`, `power_brownout_restored` day events. | `src/Main.CampaignOwners.cs` |
| Briefing copy | "The grid shed X automatically" vs "You shut down X", life-support deficit warning, brownout began/restored. | `Assets/Ashfall.Core/Campaign/DailyBriefingReportBuilder.cs` |
| Sump rising-water clock | `lastNetLevelChangeCmPerDay` + `SumpRiskSnapshot` (`GetRisk`: level, threshold, rate, hours-to-threshold) from the same numbers the sim applied; host accessor + panel `RISING … FLOOD RISK ~N h` line. | `Assets/Ashfall.Core/SumpFloodingSystem.cs`, `src/Host/SumpFloodingHostSession.cs`, `src/UI/SumpFloodingPanel.cs` |
| Panel read model | already added in §26 (SUPPLY / DEFICIT / BATTERY ETA / FUEL RUNWAY). | `src/UI/PowerGridPanel.cs` |

Decisions: automatic shedding stays a Core allocation fallback and is now
attributed, not player-impersonating. Breaker reset costs one `machine_oil`
(the canonical maintenance item) — no free reset.

## 23C — implemented

| Item | Change | Path |
|---|---|---|
| Rule catalog + integrity | `CascadeRule`, `CascadeRuleCatalog`, `CascadeRuleCatalogLoader` with `Validate`: duplicate/empty ids, unknown condition/off-ramp keys, and a **hard off-ramp requirement + warning floor** — a reachable cascade with no recovery path is rejected. | `Assets/Ashfall.Core/Shelter/CascadeRuleCatalog.cs` |
| Coordinator | `CascadeCoordinator` evaluates facts against authored rules in ordinal id order, reports active stressors + next consequence + warning hours, and emits exactly-once `cascade_warning` / `cascade_recovered` transitions (first tick seeds silently — no reload replay). No RNG, no subsystem writes. | `Assets/Ashfall.Core/Shelter/CascadeCoordinator.cs` |
| Authored rules | 5 rules (brownout→pump→flood, cold storage warm, filtration down in fallout, heating circulation lost, darkness incident), each with effect tags and off-ramps. | `Assets/StreamingAssets/Data/cascade_rules.json` |
| Integrity pipeline | `requires_all` / `off_ramps` / `effect_tags` declared pure vocabulary so the data gate validates them structurally rather than treating `manual_pump` as a `manual_*` catalog id. | `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` |
| Host facts + attribution | `Main.Cascade.cs` builds facts from the grid/sump/thermal/food/disease/fire/airlock owners, ticks the coordinator from the power day owner, and journals each transition. | `src/Main.Cascade.cs`, `src/Main.CampaignOwners.cs` |
| Briefing | `cascade_warning` → warning line; `cascade_recovered` → resolution line. | `Assets/Ashfall.Core/Campaign/DailyBriefingReportBuilder.cs` |
| Parity matrix | 7 new rows (5 power + 2 cascade) keep the no-silent-drop gate green. | `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md` |

Decisions: the cascade authority is a **pure risk/attribution layer** — it owns
no gameplay state and needs no save section, because the physical consequences
stay with the subsystems that already model them (sump flooding, spoilage,
dose, thermal). This avoids a parallel authority and a save migration. If
day-spanning escalation state is later required, the coordinator already exposes
the transition seams to persist.

## Verification (23B/23C)

- `dotnet build Ashfall.Core` — 0 warnings, 0 errors
- `dotnet build Ashfall.csproj` — 0 warnings, 0 errors
- New: `Plan23BPowerDecisionTests` **6/6** · `CascadeCoordinatorTests` **9/9**
- Adjacent: `SumpFloodingSystemTests` 39/39 · `WaterTreatmentSumpBridgeTests` 4/4 ·
  `PowerGridSurgeTests` 14/14 · `Plan210SanitationPowerFeedTests` 3/3 ·
  `ShelterScheduleIntegrationTests` 3/3 · `AtmosphericCondenserSystemTests` 9/9 ·
  `DayEventParitySourceGateTests` 2/2 · `DailyBriefingReportBuilderTests` 12/12 ·
  `DayEventVocabularyTests` 8/8
- Godot selftests: `--data-integrity-selftest` PASS (331 catalogs, 0 errors) ·
  `--bridge-selftest` PASS · `--panel-bind-lifecycle-selftest` PASS (17/17) ·
  `--audio-selftest` PASS (624/624) · `--content-utilization-selftest` PASS
- Drift gates individually re-run green: `architecture_map_drift`,
  `docs_index_drift`, `catalog_registry_drift` (regenerated, 615 catalogs),
  `persistent_filename_registry`, `core_systems_catalog_drift`,
  `ui_panel_catalog_drift`, `expansions_catalog_drift`, `audio_catalog_drift`,
  `agent_skills_catalog_drift`, `selftest_manifest_drift`, `asset_registry`

## Remaining debt / blockers

- **`save_store_matrix_drift`** fails in `verify-fast.sh` — the matrix doc was
  already modified by the concurrent `C1-PLAN22-ONE-FOOD-AUTHORITY` agent and
  their in-flight save changes make it stale. This plan added **no** save store
  or section; the owning agent should run
  `bash scripts/ci/generate-save-store-matrix.sh`.
- 23A data-authored consumer catalog + `SetupPowerConsumers()` central
  registration remain deferred (existing per-system registration is
  deterministic; see §26).
- Criticality-class data mapping (D-1/D-2) and the balance sweep / D-3 remain
  open decisions from Appendix B.
- The cascade rules are mechanism-first: no authoring beyond the five seeded
  rules yet, and no dedicated strain panel (the read model and briefing lines
  ship; a compact strain surface is a presentation package).
