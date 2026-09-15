# C2 — Flagship Integration Plan [4]: Environmental Exposure, Shelter Shielding, and Weather Consequences

> **Deliverable:** `C2_planintegration[4].md`
> **Source scope:** Plan 20 — *Exposure Is Environmental: Weather, Zone, and Position Drive Dose*
> **Wave:** Continuity Wave 2 — *The Bunker Machine* (Plans 20–24)
> **Primary objective:** replace hardcoded radiation inputs with a deterministic, data-authored exposure model where survivor position, zone contamination, shelter shielding, weather, gear, and bunker condition jointly determine dose.
> **Execution order:** **20A → 20B → 20C**
> **Wave-1 integration:** consume the semantic event vocabulary established by Plan 17A/Plan 31 so new radiation causality is legible to the player.
> **Downstream dependency:** Plan 21 gear protection becomes meaningful only after exposure itself is environmental.
> **Highest regression risk:** expedition dose must not be counted twice after position-aware environmental exposure lands.
> **Scope discipline:** no replacement `RadiationSystem`, no alternate dose formula, no new panel, no new weather state, no new audio family. Reuse `ComputeExposurePerHour`, existing weather/radiation/shelter systems, existing routes, existing audio catalog, and existing day-event plumbing.

---

# 0. Executive Intent

ASHFALL already contains most of the machinery required to make radiation physically meaningful:

- a generalized `RadiationSystem`,
- `ComputeEffectiveAmbient(...)`,
- `ComputeExposurePerHour(...)`,
- an injectable `ShelterRadQuery`,
- shelter attenuation,
- weather states,
- expedition location IDs,
- contamination data in world/location authority,
- filtration/ventilation/decontamination/airlock systems,
- radiation UI routes,
- dosimeter/history presentation,
- day-event reporting,
- geiger/audio infrastructure.

The integration failure is upstream input continuity.

Today, environmental exposure is effectively reduced to a hardcoded survivor-ID branch. The result is that:

- expedition members can remain on shelter-like ambient exposure while physically far away,
- sector contamination is not load-bearing,
- weather does not modify outdoor dose,
- indoor/outdoor status does not matter,
- ventilation and filtration do not change exposure,
- decontamination does not alter the physical dose environment,
- bunker maintenance and radiation survival function as disconnected loops.

This plan fixes the physical causal chain in three layers:

```text
20A — Where is the survivor?
      position + location contamination + weather
      → ambient exposure source

20B — What stands between them and the contamination?
      shelter geometry + filtration + ventilation + airlock + decon + radon/sump state
      → interior radiation / shielding

20C — What is the sky doing, and can the player act on it?
      weather effects
      → dose + travel + traps + caravans + thermal load + forecast decisions
```

The flagship player-facing outcome is:

> **Radiation dose becomes explainable from real world state: where the survivor is, what the zone is like, what the weather is doing, what protection they have, and how well the bunker is maintained. The same inputs are visible in UI, persist across save/load, replay deterministically, and feed the daily briefing.**

---

# 1. Source Truth

The source plan identifies the current critical defect:

- `ZoneRadLevel` has a single host writer.
- That writer uses a literal branch based on survivor ID.
- shelter shielding is derived only from a narrow ceiling attenuation input.
- weather has no consumed radiation multiplier.
- location contamination is not consumed by exposure.
- expeditions already know location IDs but survivor exposure does not follow them.
- indoor/outdoor position is not represented as authoritative survivor state.

The source also notes that Core already has the general exposure math, including a better path via `ShelterRadQuery`.

This means the correct implementation strategy is:

```text
replace hardcoded host inputs
→ do not replace Core radiation math
```

---

# 2. Program-Level Success Criteria

C2[4] is complete only when all of these questions have a deterministic answer.

## 2.1 Where is each survivor?

Every survivor has one authoritative location state.

## 2.2 What is the ambient contamination there?

The answer comes from world/location data authority, not survivor identity.

## 2.3 Is the survivor inside or exposed to outside conditions?

Position semantics determine whether shelter shielding/interior radiation applies.

## 2.4 What is the weather doing to outdoor dose?

Weather modifiers come from data and apply consistently.

## 2.5 What bunker conditions change interior radiation?

Shielding, filtration, ventilation, airlock, decontamination, radon, flooding, and structural degradation compose through one shielding/interior model.

## 2.6 Can the player understand the number?

Radiation UI exposes an input breakdown rather than only the final dose.

## 2.7 Can the player act on the cause?

Forecast, bunker maintenance, filters, decon, travel timing, traps, caravans, and heating decisions all use the same environmental weather model.

---

# 3. Architectural Invariants

## 3.1 One dose formula

Keep:

```csharp
RadiationSystem.ComputeExposurePerHour(...)
```

as the canonical dose formula.

Do not add:

- `EnvironmentalDoseSystem`,
- `OutdoorDoseCalculator` with a parallel final dose formula,
- expedition-only dose arithmetic that competes with RadiationSystem,
- panel-side dose calculations.

## 3.2 One exposure-source resolver

Position + zone/world state must resolve through one authoritative exposure-source seam.

This resolver answers:

```text
what ambient radiation source applies here?
```

It does not own:

- gear protection,
- final dose accumulation,
- ARS progression,
- UI formatting.

## 3.3 One shelter shielding/interior model

All bunker protective contributors compose through a single model.

Do not maintain:

- one number for the radiation system,
- a second number for shelter UI,
- a third number for the forecast,
- a fourth flag for “air filter bad.”

## 3.4 Data is authority

Contamination, weather-effect multipliers, shielding curves, filter curves, decon duration, and similar balance parameters belong in authored data.

No hardcoded survivor ID semantics.

No giant `switch (WeatherKind)` containing balance numbers if a weather effects catalog can own them.

## 3.5 Position is explicit state

Do not infer survivor location from:

- roster display strings,
- current panel,
- survivor ID,
- expedition text labels.

## 3.6 Deterministic replay

Any stochastic environmental behavior must use existing seeded RNG infrastructure.

Avoid:

- `System.Random`,
- wall-clock dependence,
- dictionary iteration as ordering,
- non-stable hash decisions.

## 3.7 No double dose

There must be exactly one effective environmental exposure accumulation path for a deployed survivor.

Any existing expedition dose additions must be audited and either:

- removed,
- converted to the canonical environmental source,
- or proven to represent a distinct effect not counted by the new path.

## 3.8 UI observes Core/host truth

Radiation detail and history surfaces display the actual inputs used in calculation.

No separately recomputed “estimated” breakdown unless explicitly labeled and sourced from the same model.

---

# 4. Dependency Graph

```text
20A — Position + ambient source
 │
 ├──────────────► 20B — Shelter shielding/interior radiation
 │                       │
 │                       └──────────► 20C — Weather decision effects
 │
 ├──────────────► radiation UI breakdown
 │
 ├──────────────► expedition exposure
 │
 └──────────────► Plan 21 gear protection

Plan 17A / Plan 31 semantic event vocabulary
 ├──────────────► 20B day-event attribution
 └──────────────► 20C weather/forecast consequence reporting

Plan 17C audio
 └──────────────► geiger intensity / weather alert parity
```

Required order:

```text
20A → 20B → 20C
```

Do not invert the order.

Composing weather and shielding on top of a constant zone value would produce a more elaborate constant, not environmental exposure.

---

# 5. Baseline Capture

Before editing, record the exact repository state.

## 5.1 Mandatory commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Also run current seeded radiation/balance probes if they already exist.

## 5.2 Record the current exposure pipeline

Document:

- all writers of `ZoneRadLevel`,
- all calls to `Expose(...)`,
- all calls to `ComputeExposurePerHour(...)`,
- all expedition-specific dose additions,
- all shelter attenuation inputs,
- all radiation persistence sections,
- current radiation UI read model,
- current location contamination data shape,
- current weather data shape,
- current expedition phase/location state,
- current survivor save shape.

## 5.3 Baseline metrics

Record:

- shelter survivor dose/day under current default,
- “Mikhail/outside” dose/day under current branch,
- expedition member dose/day,
- same survivor under clear/storm if weather currently has no effect,
- same survivor before/after filter state if currently no effect,
- same survivor before/after airlock state if currently no effect.

The point is to establish the broken invariants before repair.

---

# 6. Workstream 20A — Position and Ambient Exposure Source

## 6.1 Objective

Replace the literal `2f / 40f` host behavior with an explicit environmental source:

```text
survivor position
+ location/sector contamination
+ weather
→ ambient zone rate
```

The final radiation formula remains in `RadiationSystem`.

---

# 7. 20A Phase A — Tests First

Create a failing integration test proving the current defect.

Scenario:

```text
Campaign A:
survivor remains in shelter

Campaign B:
same survivor is deployed to a hot sector

All other inputs identical.
Advance equal simulated time.
```

Expected post-fix:

```text
lifetime dose A != lifetime dose B
```

Current expected failure:

```text
dose curves remain identical or reflect the hardcoded survivor branch instead of position
```

Add a second test proving survivor identity must not determine zone rate:

```text
two survivors at same location
+ same gear
+ same shielding/context
→ same ambient input
```

except where survivor-specific gear/state legitimately differs.

---

# 8. 20A Phase B — Survivor Location State

## 8.1 Add explicit Core location fact

Create or reuse a Core type equivalent to:

```csharp
SurvivorLocation
{
    kind: Shelter | Surface | Expedition | Node,
    locationId,
    sectorId
}
```

Use repository naming conventions and existing enums if available.

## 8.2 Ownership

The systems that already know movement must update location state.

Expected ownership points:

- campaign/new game initialization,
- expedition dispatch,
- expedition travel/node transition,
- expedition return,
- explicit surface/shelter transitions if they exist elsewhere.

## 8.3 Default

Most survivors begin at:

```text
Shelter
```

unless authored starting state says otherwise.

## 8.4 No identity hacks

If `survivor_gunner_mikhail` must begin outside:

- represent it in authored starting state,
- resolve it through the same location model,
- delete ID-specific radiation branching.

---

# 9. 20A Phase C — ExposureSourceResolver

## 9.1 Responsibility

Create an engine-agnostic Core resolver whose responsibility is:

```text
position + world/environment inputs
→ ambient source result
```

Suggested output shape:

```csharp
ExposureSourceResult
{
    SurvivorLocation Position;
    string? LocationId;
    string? SectorId;
    float BaseAmbient;
    float WeatherMultiplier;
    float EffectiveOutdoorAmbient;
}
```

Use the repository's actual naming conventions.

## 9.2 Purity

The resolver should be pure for ordinary resolution.

Input:

- survivor location,
- world/location contamination read model,
- weather effects read model,
- day/context if necessary.

Output:

- source/breakdown.

It must not mutate survivor dose.

## 9.3 Stochastic elements

Avoid stochastic radiation source resolution unless genuinely authored.

If randomness is necessary:

- use `ISeededRng`,
- fork through established campaign stream IDs,
- test paired-seed replay.

---

# 10. 20A Phase D — Contamination Data Authority

## 10.1 Read existing data first

Inspect:

- `locations.json`,
- sector catalogs,
- world history/world-state catalogs,
- any contamination/hazard fields already authored.

Do not invent a duplicate contamination field if one already exists.

## 10.2 If field exists

Add it to:

- DTO,
- loader,
- validator,
- runtime read model.

## 10.3 If field does not exist

Add a data-authored field with:

- snake_case,
- schema version discipline,
- validated ranges,
- stable location/sector IDs.

Example concept only:

```json
{
  "ambient_radiation": 12.5
}
```

Do not assume this exact field name.

## 10.4 Validation

Reject:

- negative contamination,
- NaN/infinite,
- unresolved location/sector references,
- missing required environmental values where the model expects them.

---

# 11. 20A Phase E — Weather Outdoor Modifier

## 11.1 Requirement

Weather contributes a multiplier to outdoor ambient radiation.

## 11.2 Data ownership

The multiplier belongs in weather-authored data.

Do not embed balance values in:

```csharp
switch (WeatherKind)
```

## 11.3 Compatibility

20A may initially consume a narrow weather-radiation field.

20C will generalize this into the complete `WeatherEffects` record.

Avoid creating a temporary field shape that 20C must later discard.

Preferred approach:

- define the reusable weather effects schema once,
- 20A consumes only `outdoor_dose_multiplier`,
- 20C later populates/consumes the remaining fields.

---

# 12. 20A Phase F — Populate ShelterRadQuery

The source identifies the existing better path:

```text
RadiationSystem
→ context.ShelterRadQuery(zone)
```

Use it.

For survivors in shelter/indoor context:

```text
ambient outdoor source
→ ShelterShieldingModel / interim interior query
→ interior ambient
→ RadiationSystem
```

20A may initially wire existing shelter attenuation if 20B is not landed yet.

Do not block 20A on the full 20B shielding model.

But structure the seam so 20B can replace the internals without changing callers.

---

# 13. 20A Phase G — Expedition Exposure

## 13.1 Position follows expedition

A deployed survivor's location must resolve to:

- current expedition node,
- current location ID,
- current sector ID,
- or the best existing canonical expedition position state.

## 13.2 Audit current dose additions

Search all expedition code for:

- dose,
- radiation,
- contamination,
- exposure,
- rad hazard.

Create a table:

| Existing expedition dose path | Meaning | Keep? | Replace? | Risk |
|---|---|---|---|---|

## 13.3 Exactly-once rule

Environmental ambient dose should not be added both:

- in generic survivor exposure,
- and in expedition tick code.

If expedition code models a distinct acute event:

```text
e.g. reactor breach / rad burst
```

that may remain, but must be semantically separate from continuous ambient exposure.

## 13.4 Regression test

Create a no-double-count test:

```text
known hot location
→ expected ambient exposure from resolver
→ deployed survivor dose increase equals canonical formula
→ no second generic expedition radiation term
```

---

# 14. 20A Phase H — Delete the Literal Branch

Remove the host code that derives zone from survivor ID.

Replace with the resolver seam.

Desired direction:

```text
SurvivorsHostSession
→ current survivor location
→ ExposureSourceResolver
→ RadiationSystem exposure context
```

No production code should contain:

```text
if survivor == Mikhail then high radiation
```

for environmental exposure.

Add a source-level regression test or grep gate if practical.

---

# 15. 20A Phase I — Position Persistence

Persist location state in the canonical survivor/campaign save section.

Requirements:

- shelter survivor reloads in shelter,
- expedition member reloads in expedition at correct node,
- returned survivor reloads in shelter,
- no fallback resets deployed survivors to shelter.

## 15.1 Migration

If save DTO changes:

- version the contract,
- provide old-save default,
- do not replay movement events during restore.

Old saves should default safely using existing authoritative expedition state where available.

---

# 16. 20A Phase J — Player-Facing Radiation Breakdown

Reuse:

- `radiation_detail`,
- `radiation_history`,
- dosimeter pen readout.

Display the actual calculation inputs.

Minimum conceptual breakdown:

```text
Location / zone ambient
Weather modifier
Shelter/interior shielding if applicable
Gear protection
Effective exposure rate
Accumulated dose
```

Do not expose implementation noise.

## 16.1 Explainability rule

The UI values must come from the same read model/result used by calculation.

No duplicated math.

## 16.2 Position label

Show meaningful source context:

```text
Shelter
Surface — Sector X
Expedition — Location Y
```

using localization/data names rather than raw IDs where possible.

---

# 17. 20A Balance Gate

Run a seeded 60-day sweep before and after.

At minimum test:

1. intact shelter,
2. degraded shelter,
3. outside clear weather,
4. outside fallout storm,
5. low contamination sector,
6. high contamination sector,
7. expedition through multiple zones.

## 17.1 Acceptance principles

- intact shelter remains survivable,
- outdoor storm is dangerous,
- hot-sector expedition meaningfully increases dose,
- no ordinary early-game storm instantly kills a healthy survivor without other compounding failures,
- environmental dose is not trivial,
- exposure differences are visible but recoverable.

Record dose/day curves.

---

# 18. 20A Test Matrix

- same survivor, shelter vs hot sector → different dose,
- two survivors same location → same ambient source before gear,
- all location kinds resolve,
- unknown location fails safely,
- weather multiplier applied,
- shelter query used for indoor position,
- expedition node changes exposure,
- expedition return restores shelter exposure,
- no double-count,
- location save/load round-trip,
- paired-seed replay,
- UI breakdown matches computation result.

---

# 19. 20A Definition of Done

- [ ] hardcoded survivor-ID environmental branch removed,
- [ ] survivor location explicit,
- [ ] location state owned by real movement systems,
- [ ] contamination comes from data authority,
- [ ] outdoor weather modifier consumed,
- [ ] shelter query populated,
- [ ] expedition position affects dose,
- [ ] double-count regression blocked,
- [ ] position persists,
- [ ] UI shows calculation breakdown,
- [ ] 60-day dose curves recorded,
- [ ] deterministic replay green.

---

# 20. Workstream 20B — Shelter Shielding and Interior Radiation

## 20.1 Objective

Make bunker maintenance the dominant controllable lever on interior radiation.

Inputs already simulated should finally affect a single physical shielding/interior-radiation model.

Expected contributors:

- wall/ceiling attenuation,
- filtration,
- ventilation,
- decontamination,
- airlock/hatch state,
- radon migration,
- sump flooding,
- structural degradation.

---

# 21. 20B Phase A — Shielding Capability Inventory

Read and document every existing state source.

Create a matrix:

| Contributor | Existing system | Existing state | Persisted? | Current radiation effect | New model role |
|---|---|---|---:|---:|---|
| ceiling | shelter | attenuation | yes/no | partial | base shielding |
| walls | shelter | integrity | yes/no | none/partial | shielding |
| filter | starting/shelter | condition | yes/no | none | airborne reduction |
| ventilation | atmosphere | state | yes/no | none | air exchange |
| decon | decontamination | cycle state | yes/no | none | interior contamination |
| airlock | security | sealed/open | yes/no | none | ingress |
| radon | migration system | level | yes/no | none | interior source |
| flooding | sump | state | yes/no | none | contamination transport |

Use actual repository facts.

---

# 22. 20B Phase B — ShelterShieldingModel

Create one Core model.

## 22.1 Outputs

It should provide both:

- an interior radiation query/value suitable for `ShelterRadQuery`,
- a shielding fallback/value suitable for existing callers that require shielding.

## 22.2 Composition

Document precedence.

Example conceptual sequence:

```text
outdoor ambient
→ structural attenuation
→ ingress modifier (airlock/vent)
→ filtration
→ internal contamination sources (radon/flooding)
→ decon reduction
→ interior ambient
```

The actual formula must reflect existing model semantics and balance.

## 22.3 One arithmetic path

The UI and radiation system must use the same result.

No independent “shielding percent” display formula.

---

# 23. 20B Phase C — Filter Consumption and Clogging

## 23.1 Real resource behavior

HEPA/air filters should degrade based on authored inputs such as:

- dust/weather load,
- contamination exposure,
- runtime hours.

## 23.2 Deterministic curve

Use data-authored parameters.

No random clogging unless already designed.

## 23.3 Same-day consequence

Replacing a clogged filter must measurably reduce interior exposure within the same day/tick cadence supported by the simulation.

## 23.4 Alert parity

Tie existing `shelter_air_filter` warning to the same numeric threshold used by the model.

Do not maintain a separate warning flag that can drift from the actual filter state.

---

# 24. 20B Phase D — Decontamination

Decon must alter physical interior contamination.

## 24.1 Requirements

- data-defined effectiveness,
- data-defined duration/window,
- diminishing returns on repeated cycles if authored,
- consumes existing water/consumables,
- persists active decon state if it spans time.

## 24.2 No magic reset

Avoid:

```text
decon button
→ radiation = 0
```

Use bounded reduction according to data.

## 24.3 Observable

Radiation breakdown should show decon contribution while active.

---

# 25. 20B Phase E — Airlock and Ventilation Coupling

During bad weather:

```text
unsealed hatch / poor ventilation state
→ greater contamination ingress
→ higher interior radiation
```

During benign conditions:

- effect may be smaller,
- but must remain physically consistent.

## 25.1 Real state only

Read canonical:

- airlock seal state,
- ventilation state,
- weather state.

No UI-local approximation.

---

# 26. 20B Phase F — Radon Migration and Sump Flooding

These systems already simulate relevant physical conditions.

Feed them into the shielding/interior model.

## 26.1 Radon

Treat radon as an internal source term or appropriate existing radiation category.

Do not incorrectly multiply radon by outdoor shielding.

## 26.2 Flooding

If flooding transports contamination:

- feed the contamination contribution into the interior model,
- ensure decon/drain actions can reduce it where existing mechanics support that.

## 26.3 Persistence

Restore these contributors before recomputing current interior state on load.

---

# 27. 20B Phase G — Data-Author Shielding Curves

Add or extend:

```text
shelter_shielding.json
```

or the repository's canonical shelter-radiation data file.

Include only parameters that truly need authoring.

Potential categories:

- structural attenuation scaling,
- filter efficiency curve,
- clog thresholds,
- ventilation ingress multipliers,
- airlock ingress multiplier,
- decon reduction/duration,
- radon contribution scaling,
- flooding contamination scaling.

## 27.1 Validation

Use `CatalogIntegrityValidator`.

Check:

- ranges,
- IDs,
- non-negative values,
- bounded multipliers,
- schema version,
- no unresolved referenced item/system IDs.

---

# 28. 20B Phase H — Shelter UI Continuity

Update existing shelter/radiation surfaces.

Display:

```text
Composite shielding / interior protection
Weakest contributor
Active filter condition
Ventilation state
Airlock state
Decon active/inactive
Major internal contamination contributor
```

Do not overload the panel with raw coefficients.

## 28.1 Weakest-link language

Reuse existing “weakest ceiling” style where appropriate.

Player-facing goal:

```text
Why is interior radiation high?
```

should have a clear answer.

---

# 29. 20B Phase I — Day Events

Emit semantic events through the established event vocabulary.

Source-proposed concepts:

- `shielding_degraded`,
- `filter_replaced`,
- `decon_cycle`,
- `hatch_unsealed`.

If Plan 31 renamed/standardized kinds, use the canonical names.

Do not invent raw strings in 20B.

## 29.1 Causality

A briefing should be able to say:

```text
Filter clogged
→ interior shielding worsened
→ dose rose
```

without reverse-engineering state snapshots.

---

# 30. 20B Balance Gate

Sweep:

```text
storm frequency × filter supply × shelter integrity × decon resources
```

Focus on recoverability.

Reject:

- unavoidable death spirals,
- one filter failure causing instant lethal dose,
- decon so strong it trivializes bunker maintenance,
- ventilation/airlock effects too small to matter,
- filter consumption so fast that normal resource flow cannot support it.

Record curves and thresholds.

---

# 31. 20B Test Matrix

- shielding composition precedence,
- intact vs damaged structure,
- clean vs clogged filter,
- replace filter lowers exposure,
- ventilation active vs stalled,
- airlock sealed vs open,
- storm amplifies ingress,
- decon reduces interior contamination,
- decon duration expires deterministically,
- repeated decon diminishing returns if authored,
- radon contribution included,
- sump/flood contribution included,
- save/load filter state,
- save/load decon state,
- UI weakest contributor matches model,
- end-to-end filter replacement lowers next-day dose,
- day events emitted once.

---

# 32. 20B Definition of Done

- [ ] single shelter shielding/interior model exists,
- [ ] all major bunker radiation contributors feed it,
- [ ] filter degradation is real,
- [ ] warning threshold matches numeric model,
- [ ] decon changes interior radiation,
- [ ] airlock/ventilation couple to weather,
- [ ] radon/flooding feed the model,
- [ ] parameters data-authored,
- [ ] shelter UI exposes cause,
- [ ] semantic day events emitted,
- [ ] save/load stable,
- [ ] balance sweep recoverable.

---

# 33. Workstream 20C — Weather as a Decision System

## 33.1 Objective

No authored `WeatherKind` should remain purely decorative.

Each meaningful state must alter at least one player-relevant decision.

The forecast becomes preparation infrastructure, not flavor text.

---

# 34. 20C Phase A — Weather Consumer Inventory

Enumerate all current `WeatherKind` values.

For each, document current consumers.

Matrix:

| Weather kind | Audio | Forecast text | Radiation | Travel | Traps | Caravan | Thermal | Visibility | Other |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|

Rows with only audio/text are the main mechanical work list.

Use actual current enum count; the source reports 22.

---

# 35. 20C Phase B — WeatherEffects Data Record

Create or extend a data-authored effects record.

Conceptual fields:

```text
outdoor_dose_multiplier
travel_speed_modifier
travel_risk_modifier
trap_yield_modifier
caravan_availability_modifier
thermal_load_modifier
visibility_modifier
forecast_reliability_modifier
```

Do not assume all must be numeric multipliers; choose semantics appropriate to existing systems.

## 35.1 No silent defaults

Every `WeatherKind` must have an explicit effects entry.

The table-driven test should fail if a new enum value appears without data.

---

# 36. 20C Phase C — Expedition Dispatch

`ExpeditionSystem.Estimate` already provides travel/fuel/capacity/risk.

Add weather to the estimate.

## 36.1 Forecast-aware estimate

The dispatch surface should show the estimate for the planned departure period.

Player decision:

```text
go now
vs
wait for better weather
```

## 36.2 Consistency

The estimate and actual expedition runtime must consume the same weather effects source.

No UI-only risk formula.

---

# 37. 20C Phase D — Forecast Reliability

The source indicates weather station accuracy, calibration, and a 3-day horizon already exist.

Surface reliability visibly.

## 37.1 Required display

Show:

- forecast state,
- confidence/reliability,
- calibration quality where relevant,
- whether data is stale/uncertain if supported.

## 37.2 Imperfect but fair

A low-quality forecast may be wrong.

But the player must be able to understand why reliability was poor.

Avoid hidden arbitrary failure.

---

# 38. 20C Phase E — Costly but Recoverable Forecast Error

If a storm is missed:

- outdoor dose may rise,
- travel risk may rise,
- shelter load may rise.

But early/mid-game forecast error should not be an unavoidable instant-death event.

Use balance tests.

---

# 39. 20C Phase F — Radio and Briefing Warnings

Reuse:

- radio broadcast machinery,
- `weather_alert`,
- semantic day-event/briefing layer.

Differentiate:

```text
radio predicted storm
station predicted storm
station missed storm
unexpected storm
```

where existing data supports the distinction.

Do not create a new notification subsystem.

---

# 40. 20C Phase G — Trapping and Foraging

Use the same `WeatherEffects` source.

Examples:

- storm reduces trapping efficiency,
- certain conditions improve specific yields,
- visibility affects foraging/search.

Do not duplicate weather tables inside wildlife systems.

Add table-driven integration tests.

---

# 41. 20C Phase H — Caravan Availability

Use the same weather effects source for caravan accessibility/availability.

Keep shape compatible with future embargo/trade systems.

Do not mix:

- geopolitical embargo,
- physical weather inaccessibility,

into one opaque boolean.

They may combine at a higher availability decision.

---

# 42. 20C Phase I — Thermal Coupling

Cold weather should increase heating/fuel pressure through the already-simulated thermal system.

Expose this in:

- warmth/heating,
- power/fuel,
- briefing.

Avoid a second direct fuel drain if `ShelterThermalSystem` already owns the actual consumption.

---

# 43. 20C Phase J — Audio Parity

Map weather states lacking dedicated transition cues to existing cue families.

No new production audio batch here.

Rules:

- reuse existing beds by semantic family,
- no missing state produces silence if a transition cue is expected,
- do not duplicate audio if generic family already plays.

Coordinate with Plan 17C's exactly-once/alert concurrency rules.

---

# 44. 20C Phase K — Weather Mechanical Coverage Gate

Add a table-driven test:

```text
for every WeatherKind
→ WeatherEffects entry exists
→ at least one mechanical effect is non-identity
```

Potential exception:

If a truly neutral weather state is intentionally mechanically neutral, classify it explicitly rather than silently using identity values.

The source objective is that no weather state remains purely cosmetic.

---

# 45. 20C Phase L — Forecast UI and Map Integration

Update existing:

- `WeatherForecastPanel`,
- `MapPanel`.

Show actionable effects:

- radiation risk,
- expedition risk/speed,
- visibility,
- thermal pressure,
- trapping impact,
- caravan access,
- reliability.

Do not dump every raw multiplier.

Present decisions.

Example conceptual text:

```text
Tomorrow: Fallout Storm
Outdoor dose: severe
Travel: +35% risk
Visibility: poor
Forecast confidence: 72%
Recommended: delay nonessential expedition
```

Use existing localization/style conventions.

---

# 46. 20C Test Matrix

For every weather state:

- effects record exists,
- at least one mechanical effect non-identity,
- outdoor dose modifier valid,
- travel estimate matches runtime,
- forecast reliability visible,
- weather error remains recoverable in balance fixture,
- radio/briefing warnings map correctly,
- trapping uses shared table,
- caravan uses shared table,
- thermal coupling reaches existing fuel/heat path,
- audio mapping exists where required,
- save/load does not corrupt weather effects,
- same seed produces same weather/effects sequence.

---

# 47. 20C Definition of Done

- [ ] all weather kinds inventoried,
- [ ] `WeatherEffects` data authority exists,
- [ ] every weather kind has explicit entry,
- [ ] outdoor dose uses it,
- [ ] expedition estimate/runtime uses it,
- [ ] forecast reliability visible,
- [ ] missed forecasts costly but recoverable,
- [ ] radio/briefing warnings integrated,
- [ ] trapping/foraging uses it,
- [ ] caravans use it,
- [ ] thermal system consumes it,
- [ ] weather audio parity preserved,
- [ ] no state purely cosmetic,
- [ ] deterministic coverage green.

---

# 48. Integrated Exposure Pipeline

Final intended flow:

```text
SurvivorLocation
    │
    ▼
ExposureSourceResolver
    ├─ location/sector contamination
    └─ WeatherEffects.outdoor_dose
    │
    ▼
Outdoor ambient
    │
    ├────────────── if shelter/indoors ──────────────┐
    │                                                │
    │                                     ShelterShieldingModel
    │                                      ├─ structure
    │                                      ├─ filtration
    │                                      ├─ ventilation
    │                                      ├─ airlock
    │                                      ├─ decon
    │                                      ├─ radon
    │                                      └─ flooding
    │                                                │
    │                                                ▼
    │                                         interior ambient
    │                                                │
    └─────────────────────────────┬──────────────────┘
                                  ▼
                           worn gear protection
                                  │
                                  ▼
                      RadiationSystem.ComputeExposurePerHour
                                  │
                                  ▼
                              Expose(...)
                                  │
        ┌─────────────────────────┼────────────────────────────┐
        ▼                         ▼                            ▼
      dose                    ARS / triage                dose ledger
        │
        ├──────────────► radiation detail/history UI
        │
        ├──────────────► geiger/audio intensity
        │
        └──────────────► semantic day events / briefing
```

---

# 49. Exactly-Once Exposure Ownership

Create an explicit ownership note in code/docs.

## Continuous ambient exposure

Owned by:

```text
RadiationSystem exposure tick using ExposureSourceResolver + shielding + gear
```

## Acute radiation events

Examples:

- breach,
- contamination burst,
- reactor incident.

These may add discrete dose only if semantically distinct.

Every acute-dose producer must be documented.

## Expedition travel

Must not separately add the same ambient dose already handled by survivor environmental exposure.

---

# 50. Save/Load Contract

At minimum persist:

- survivor position/location,
- filter condition,
- decon state/window,
- any shelter shielding state not already persisted,
- weather system state as already required,
- exposure-relevant world state.

## 50.1 Restore order

Restore authoritative state before recalculating current exposure.

Suggested order:

```text
world/weather
→ shelter systems
→ survivor/expedition position
→ radiation state
→ UI read models
```

Use actual composition architecture.

## 50.2 No restore side effects

Loading must not:

- add exposure for elapsed wall-clock time,
- replay decon action events,
- reapply filter replacement,
- duplicate expedition dose,
- fire false weather transition alerts.

---

# 51. Determinism Contract

Paired same-seed campaigns with the same player actions must produce identical:

- weather sequence,
- survivor positions,
- exposure sources,
- shielding values,
- dose curves,
- forecast outcomes where forecast uncertainty is seeded,
- day-event attribution.

Add a high-level replay fixture.

---

# 52. UI Continuity Contract

Player-facing surfaces must answer:

## Radiation detail

```text
Where am I?
How contaminated is it?
What is weather doing?
What is the shelter doing?
What is gear doing?
What is my current exposure rate?
```

## Shelter

```text
What is my effective shielding?
What is the weakest contributor?
What can I fix?
```

## Forecast

```text
What will weather change?
How certain is that?
Should I travel, trap, trade, or conserve fuel?
```

## Briefing

```text
Why did dose rise?
What changed?
What action can I take?
```

---

# 53. Semantic Event Integration

Use canonical event kinds from Plan 31/17A.

Potential semantic categories:

- location exposure increased,
- shielding degraded,
- filter clogged/replaced,
- decon started/expired,
- hatch unsealed,
- weather hazard active,
- expedition entered hot zone,
- indoor radiation rose,
- forecast miss.

Do not invent ad-hoc strings if canonical vocabulary exists.

Each actionable event should carry enough entity identity for routing.

---

# 54. Audio Integration

Coordinate with Plan 17C.

## Radiation

- geiger intensity may follow effective exposure,
- explicit start/stop lifecycle remains authoritative,
- no audio computation changes dose.

## Weather

- alert cues use existing catalog,
- concurrency/ducking rules apply,
- family reuse for missing weather-specific cues.

## Shelter

- filter/ventilation warnings should reflect the same numeric model used by shielding.

---

# 55. Balance Program

Run seeded 60-day sweeps.

At minimum vary:

```text
zone contamination
× weather severity
× expedition exposure
× shelter integrity
× filter supply
× ventilation state
× airlock state
× decon resources
× gear
```

Do not brute-force every Cartesian combination if tooling is expensive.

Use representative stratified scenarios.

## 55.1 Required outputs

Record:

- average dose/day,
- p50/p95 dose/day,
- cumulative 60-day dose,
- ARS threshold crossings,
- survivor mortality attributable to radiation,
- filter consumption rate,
- recovery after maintenance action,
- forecast error outcomes,
- expedition dose contribution.

## 55.2 Acceptance

Environmental radiation should:

- matter,
- be explainable,
- reward preparation,
- punish reckless exposure,
- remain recoverable through correct systems,
- avoid unavoidable early death spirals.

---

# 56. Performance Considerations

The resolver may execute frequently.

Requirements:

- no repeated JSON parsing,
- weather effects preloaded,
- location contamination indexed,
- O(1)-average lookup by location/sector ID,
- shielding calculation avoids heavy allocations,
- UI breakdown reuses already-computed result where practical.

Add a benchmark only if current profiling shows the exposure path is materially hot.

Do not prematurely add caching that risks stale environmental state.

---

# 57. Failure Modes and Corrective Actions

## 57.1 Expedition dose doubles

Cause:

- existing expedition dose + new ambient path.

Fix:

- identify ownership,
- remove duplicate continuous term,
- keep only distinct acute events.

## 57.2 Survivor reloads into shelter while expedition is active

Cause:

- location persistence not integrated with expedition restore.

Fix:

- derive/persist authoritative deployment position consistently.

## 57.3 Weather multiplier differs between UI estimate and runtime

Cause:

- duplicated formula/table.

Fix:

- both consume `WeatherEffects`.

## 57.4 Shelter UI says “good shielding” but dose is high

Cause:

- UI and radiation system use different arithmetic.

Fix:

- display shared `ShelterShieldingModel` result.

## 57.5 Filter warning fires at wrong threshold

Cause:

- separate flag or hardcoded UI threshold.

Fix:

- warning reads same numeric condition used by model.

## 57.6 Decon applies twice after load

Cause:

- restore replays action rather than restoring state.

Fix:

- restore active state/window without action side effects.

## 57.7 Weather state has identity mechanics

Cause:

- missing effects data or silent default.

Fix:

- fail table-driven gate.

## 57.8 Forecast feels arbitrary

Cause:

- reliability hidden.

Fix:

- expose confidence/calibration and seed deterministically.

## 57.9 Audio affects replay checksum

Critical.

Fix before merge.

---

# 58. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| expedition dose double-count | High | High | audit all dose producers + integration test |
| save migration loses position | Medium | High | versioned DTO + expedition restore test |
| weather schema churn between 20A/20C | Medium | Medium | define reusable `WeatherEffects` shape early |
| shelter model duplicates existing math | Medium | High | one shared model output |
| filter curve too punitive | Medium | Medium | 60-day sweep |
| decon trivializes exposure | Medium | Medium | diminishing/bounded data curve |
| route/UI values drift from runtime | Medium | High | shared read model |
| unknown location gives zero dose silently | Medium | High | fail-safe validation |
| weather effect added but no player observable | Medium | Medium | forecast/briefing visibility requirement |
| audio double-fire | Medium | Low–Med | Plan 17C ownership rules |
| hot path allocations | Low–Med | Medium | profile, indexed data |
| new data fields unvalidated | Medium | Medium | integrity validator |

---

# 59. Commit Strategy

## Commit C2[4].1 — Baseline and failing exposure tests

- document current hardcoded branch,
- failing shelter vs hot-sector test,
- expedition dose producer inventory.

## Commit C2[4].2 — Survivor location state

- Core location fact,
- expedition dispatch/return ownership,
- persistence scaffolding.

## Commit C2[4].3 — ExposureSourceResolver + contamination data

- data DTO/loader,
- resolver,
- tests.

## Commit C2[4].4 — Weather outdoor multiplier + shelter query seam

- reusable weather effects schema start,
- `ShelterRadQuery` population.

## Commit C2[4].5 — Expedition integration + literal removal

- position-aware exposure,
- no-double-count gate,
- remove survivor-ID branch.

## Commit C2[4].6 — Radiation UI breakdown + balance probe

### Gate: 20A complete

## Commit C2[4].7 — ShelterShieldingModel

- structural/filter/vent/airlock inputs,
- shared output.

## Commit C2[4].8 — Filter + decon lifecycle

- data curves,
- persistence,
- warnings.

## Commit C2[4].9 — Radon/flooding integration + shelter UI

- event emission,
- snapshot.

## Commit C2[4].10 — Shielding balance closure

### Gate: 20B complete

## Commit C2[4].11 — WeatherEffects full table

- all WeatherKind rows,
- integrity coverage.

## Commit C2[4].12 — Expedition/forecast integration

- estimates,
- reliability.

## Commit C2[4].13 — traps/caravans/thermal integration

- shared effects record.

## Commit C2[4].14 — weather radio/briefing/audio parity

- alerts,
- semantic events,
- family cue mapping.

## Commit C2[4].15 — table-driven mechanical coverage + final balance

### Gate: 20C complete

## Commit C2[4].16 — integrated 60-day replay closure

- save/load,
- deterministic replay,
- full verification.

---

# 60. Verification Checklist

Run per substantial workstream and at final closure.

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Also run:

```text
ashfall-balance-sim
ashfall-seed-replay
```

using current repository invocations.

Required manual/diagnostic scenarios:

- shelter,
- hot surface sector,
- expedition,
- storm,
- filter replacement,
- decon,
- airlock open/closed,
- save/load during expedition,
- save/load during decon,
- forecast decision.

---

# 61. Flagship Definition of Done

## 20A — Position and zone

- [ ] explicit survivor location exists,
- [ ] expedition updates it,
- [ ] contamination is data-authored/consumed,
- [ ] weather modifies outdoor ambient,
- [ ] shelter query populated,
- [ ] hardcoded survivor branch deleted,
- [ ] expedition ambient dose not double-counted,
- [ ] location persists,
- [ ] radiation UI shows breakdown,
- [ ] 60-day curves viable,
- [ ] deterministic replay passes.

## 20B — Shelter

- [ ] one shielding/interior model,
- [ ] structure contributes,
- [ ] filters contribute,
- [ ] filters clog/consume deterministically,
- [ ] replacing filter lowers exposure,
- [ ] ventilation contributes,
- [ ] airlock contributes,
- [ ] decon contributes,
- [ ] radon contributes,
- [ ] flooding contributes,
- [ ] shielding curves data-authored,
- [ ] warning thresholds numeric and shared,
- [ ] shelter UI shows weakest contributor,
- [ ] day events explain degradation,
- [ ] save/load stable,
- [ ] balance recoverable.

## 20C — Weather

- [ ] all WeatherKind values inventoried,
- [ ] all have explicit effects records,
- [ ] all have at least one mechanical effect,
- [ ] expedition estimates consume weather,
- [ ] actual travel consumes same weather source,
- [ ] forecast reliability visible,
- [ ] missed forecasts costly but recoverable,
- [ ] radio/briefing warning integrated,
- [ ] traps/foraging consume weather,
- [ ] caravans consume weather,
- [ ] thermal pressure consumes weather,
- [ ] audio parity complete without new family,
- [ ] no silent default,
- [ ] deterministic replay passes.

## Cross-system

- [ ] one exposure formula remains,
- [ ] one ambient resolver remains,
- [ ] one shelter shielding model remains,
- [ ] UI displays canonical values,
- [ ] audio is observational,
- [ ] semantic events use canonical vocabulary,
- [ ] full verification green.

---

# 62. Closure Report Template

```markdown
## C2[4] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:
- Working tree:

### Baseline Exposure Audit
- ZoneRadLevel writers:
- Expose call sites:
- Expedition dose paths:
- Shelter dose/day:
- Hot-sector dose/day:
- Weather influence before:
- Filter influence before:

### 20A — Position + Ambient
- SurvivorLocation:
- ExposureSourceResolver:
- Contamination authority:
- Weather outdoor multiplier:
- ShelterRadQuery:
- Expedition integration:
- Double-count result:
- Literal branch removed:
- Save/load:
- UI breakdown:
- 60-day curves:
- Seed replay:
- Result:

### 20B — Shielding
- Structural:
- Filter:
- Ventilation:
- Airlock:
- Decon:
- Radon:
- Flooding:
- Data curves:
- UI weakest contributor:
- Day events:
- Save/load:
- Balance:
- Result:

### 20C — Weather
- WeatherKind count:
- Effects catalog:
- Mechanical coverage:
- Expedition estimate:
- Forecast reliability:
- Radio/briefing:
- Trapping:
- Caravan:
- Thermal:
- Audio parity:
- Table-driven gate:
- Balance:
- Result:

### Full Verification
- Core tests build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Triad drift:
- Verify fast:
- Balance sim:
- Seed replay:

### Remaining Debt
- Radiation:
- Shelter:
- Weather:
- Expedition:
- Forecast:
- Plan 21 handoff:
```

---

# 63. Final Execution Directive

Implement Plan 20 as an input-continuity repair.

Do not redesign radiation.

The final physical chain must be:

```text
where the survivor is
+ how contaminated that place is
+ what the weather is doing
+ whether shelter protection applies
+ how healthy the shelter protection actually is
+ what gear the survivor wears
→ canonical RadiationSystem exposure
→ dose
→ visible consequences
```

The work is not complete when:

- a new resolver class exists,
- the forecast shows numbers,
- filters have a condition meter,
- a storm plays audio,
- expedition risk changes.

It is complete only when all these surfaces consume the same authoritative environmental state.

The most important regression rule is explicit:

> **After 20A lands, there must be one and only one continuous ambient-radiation contribution for a deployed survivor. Audit and test expedition dose ownership before merge.**

And the most important player-facing rule is equally explicit:

> **Every unexpected dose increase must be explainable from current location, weather, shielding, gear, and recorded environmental events.**

---

# 64. Execution Checkpoint — 2026-09-15 (20A package)

Premise corrections against §1 (full evidence: `C2_PLANINTEGRATION_4_BASELINE.md`):
the hardcoded survivor-ID branch, missing position state, missing contamination
consumption, and missing weather consumption were **already repaired** in current
source and are gated by `EnvironmentalExposureMatrixTests`. Executed remainder:

- **G3:** `OnWeatherGateForced` consumer (acute `ForceRadDose` via radiation
  owner, never ambient) — silent-drop defect closed. 4/4 tests.
- **G1:** `weather_effects.json` data authority; `OutdoorRadModifier` +
  `ForecastRadModifier` share the bound table; legacy constants preserved
  unbound; validator enforces 22/22 explicit rows. 10/10 tests.
  Balance alignment: Ashfall outdoor dose 0→45 (matches the forecast the
  player already sees).
- **G2:** `ShelterRadQuery` seam semantics pinned (precedence, no double-count,
  gear subtraction, clamp). Host keeps byte-identical fallback math until 20B's
  `ShelterShieldingModel`. 5/5 tests.
- **G4:** expedition `camp.radiationExposure` confirmed inert — exactly-once
  ambient ownership by `RadiationSystem` holds; retirement deferred to 20B.

Deferred: §16 UI breakdown parity extension, §17 60-day seeded balance sweep.

---

# 65. Checkpoint — 20A closure (2026-09-15, second pass)

- **§16 UI breakdown parity closed.** `RadiationSystem.ComputeEffectiveRate` is now the one
  effective-rate resolver (tick + read model); Core `ExposureBreakdown`; host
  `GetExposureBreakdown`; `RadiationDetailPanel` shows the canonical line per survivor
  (position, zone, weather/fallout/anomaly, shielding, gear → effective rate, dose,
  lifetime). Source gates: panel must not carry dose math. 15/15.
- **§17 balance evidence recorded.** 9/9 seeded sweep + `docs/balance/BALANCE_SIM_radiation_exposure_20A.md`.
  Key findings: F1 surface/expedition dose saturates the acute scale in hours (pre-existing
  tuning question, proposals only); F3 ceiling attenuation dominates; F4 intact shelter is
  exactly zero (20B needs internal sources for a maintenance floor).
- **20A status: COMPLETE** against the revised scope in `C2_PLANINTEGRATION_4_BASELINE.md`.
  20B (ShelterShieldingModel + filter/decon/airlock/radon/flooding) is the next claim.

---

# 66. Checkpoint — 20B core (2026-09-15)

One shelter shielding/interior-radiation model landed behind the 20A-pinned
`ShelterRadQuery` seam (callers unchanged):

- **Data authority:** `shelter_shielding.json` (baseline 2.0 + 13 bounded
  coefficients), validated through `CatalogIntegrityValidator`.
- **Model:** `ShelterShieldingModel` — composition = baseline bleed ×
  ingress(filter clog + duct breach + saturation + recirculation + airlock
  seal/incident, weather amplifies existing defects only) + internal sources
  (radon + flooding + shelter contamination) with bounded decon reduction;
  caps authored (`max_ingress_multiplier`, `max_interior_rad_rate`).
- **Wiring:** resolver `ShelterInteriorRadQuery` → `ExposureBreakdown` →
  `RadiationSystem` (one path); host binds every canonical owner lazily
  (MaterialShieldingSystem, StartingLevel air filter + radon, VentilationSystem,
  AirlockSecuritySystem door state/incidents, SumpFloodingSystem worst room,
  DecontaminationSystem state). Unbound ⇒ nominal ⇒ legacy byte-identical.
- **No new save fields:** the model is a pure function of already-persisted
  owner state (filter/vent/airlock/decon/radon/sump saves unchanged).
- **Tests:** 21 focused (composition, caps, NaN safety, purity, catalog ranges,
  seam end-to-end, warning-parity + canonical-owner source gates); Shelter
  573/573; Radiation 72/72; data-integrity PASS; host build 0.
- **Recorded deltas:** intact shelter now has the shelter-live radon floor
  (0.06 mSv/h at 12 Bq/m³) instead of exactly zero; maintenance defects raise
  interior ingress; active decon halves internal sources. Structure term is
  ceiling-only (no general shelter-condition owner exists — documented).

**Remaining 20B:** §29 semantic day events, §28 weakest-contributor UI line,
§30 60-day shielding balance sweep, save/load regression pins.

---

# 67. Checkpoint — 20B complete (2026-09-15, second pass)

§28–§30 + save pins landed on top of the §66 core:

- **§29 day events:** `ShelterFacilitiesDayOwner` transition detection emits
  `shelter_filter_degraded` (band + %), `shelter_decon_started` /
  `shelter_decon_completed`, `shelter_hatch_unsealed` (door state); the briefing
  builder renders tailored Warnings / new "Shelter" section text; parity matrix
  current; source gate 2/2. Filter *replacement* stays on the existing
  maintenance-command feedback path (no day-tick observer exists — documented).
- **§28 UI:** contributor breakdown (`GetBreakdown`) names the largest single
  mover; radiation detail Protection section shows interior rate + weakest
  contributor + decon flag from the same arithmetic path as dose.
- **§30 sweep:** 12 focused cases + balance addendum (F5–F8): weather only acts
  through defects; maintenance ordering measured; decon bounded; intact radon
  floor 1.44 mSv/day.
- **Save pins:** no new save section; model is pure over persisted owner state
  (owner round-trip coverage existing); paired-run fingerprint pins determinism.

**20B status: COMPLETE.** Controls: Shelter 588/588 · Campaign 107/107 ·
Radiation 72/72 · parity 2/2 · host build 0 · data-integrity PASS · panel
lifecycle PASS · triad PASS.

**Next:** 20C (weather as a decision system — §33–§47), which consumes the
`weather_effects.json` authority created in 20A (currently only
`outdoor_rad_modifier` is populated; the remaining fields are 20C's scope).
