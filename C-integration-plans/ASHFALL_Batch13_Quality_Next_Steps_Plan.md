# ASHFALL: Batch 13 Integration Plan

**Steps:** 193–208<br>
**Generated:** 2026-08-19<br>
**Status:** Execution-ready, architecture-aligned<br>
**Host:** Godot 4.7+ (.NET 8 C#)<br>
**Core target:** .NET Standard 2.1

## 1. Integration decision

Batch 13 is a content batch, not Expansion 13. Keep `ExpansionSuite` at its
canonical 12 entries. Implement the batch as deterministic Core systems with
typed host sessions and Godot presentation.

The roadmap references `District8Accords`, `DynamicEconomySystem`,
`PowerGridPanel`, and `ShelterHazardLoop`. Those names are not the current
authorities. Use the existing `District8DeepCoastSystem` plus the treaty IDs in
`Assets/StreamingAssets/Data/foundry_accords.json`, `MarketSystem` rather than
`DynamicEconomySystem`, Batch 10’s shared power contract, and the host hazard
loop only as an adapter. Core remains free of Godot APIs and engine JSON.

Existing authorities to preserve:

- `BrineWaterSystem` owns brine/membrane state and salt-trade unlocks.
- `SilentFoundrySystem` owns foundry heat, products, maintenance, quality,
  incidents, and treaty compliance.
- `GreenhouseSystem`, `RecipeCatalog`, `GoodsCatalog`, and `NeedsSystem` own
  agricultural production, recipes, inventory products, and survivor effects.
- `WeatherSystem`, `RadioHostSession`, `LocationLayoutSystem`,
  `ResearchSystem`, `JournalSystem`, `CrossingArbitrationSystem`,
  `MusterHostSession`, `DiseaseSystem`, `TacticalCombatSystem`, and
  `WarlordDoctrineSystem` remain the integration boundaries.
- `CenturySeed`/generational state and the existing journal are inputs to a
  new structured campaign-history projection; neither is replaced by UI code.

The roadmap’s absolute results are converted into authored, measurable
fixtures. “Infinite fertilizer,” “flawless navigation,” “uninterceptable
radio,” “decimates,” and “unbroken 100-year history” are not unconditional
engine guarantees.

## 2. Shared foundation and persistence

### 2.1 Batch save

Add `Batch13State` and `Batch13SaveStore` at
`user://saves/slot-N/batch13_save.json`. Use a checksummed versioned envelope,
`AtomicFileWriter`, explicit migrations, deep-copy capture/restore, and
rejection of malformed current envelopes. `SaveLoadHostSession` already
includes files ending in `_save.json` and hashes them in the slot manifest.

New mutable feature state belongs in this batch store by default. If a feature
is later folded into `SilentFoundryState`, `GreenhouseState`, `RadioSaveState`,
or another existing store, make a deliberate schema migration and remove the
duplicate writer. Do not add parallel panel-owned JSON.

### 2.2 Shared contracts

Before feature work, implement or consume the shared contracts introduced by
Batches 10–12:

- `PowerGridSystem`: `power_kw`, `energy_kwh`, storage, priority loads,
  faults, and outage events;
- measured units and bounded conversion rules for mass, temperature, pressure,
  distance, frequency, signal strength, and energy;
- inventory reservations, by-products, quality, contamination, maintenance,
  and operator-skill results;
- sourced, bounded survivor effects for health, nutrition, morale, XP,
  navigation, work speed, and equipment runtime;
- location/line-of-sight, faction/treaty, threat/contact, geological sample,
  and radio-signal references.

Extend `CatalogIntegrityValidator` for every new ID, reference, unit/range,
schema-version, and uniqueness rule. New data under
`Assets/StreamingAssets/Data/` uses `snake_case` IDs.

### 2.3 Structured campaign history

Create a Core `CampaignHistoryLedger` with typed records for treaty, battle,
birth, death, disaster, research, construction, and leadership events. Existing
systems publish records through an event adapter; `JournalSystem` remains the
human-readable projection and bounded journal UI.

The ledger must support sequence number, day/year, source system, actor IDs,
location, causal parent, visibility, and branch/alternate-outcome metadata.
Migration can import available historical journal/ledger data, but missing
history must be marked as a gap rather than silently fabricated. Step 208
renders this ledger and `CenturySeed` generations; it does not infer a complete
century from the current day counter.

## 3. Ordered vertical slices

1. **Foundation:** Batch 13 save, units, catalog schemas, power/material
   contracts, history ledger, and `--batch13-selftest` scaffolding.
2. **Industrial chemistry:** 193 lithium, 194 open-hearth, 201 ductile iron,
   and 204 carbide inserts through foundry work orders.
3. **Food and medicine:** 195 airway treatment, 197 pickling, and 199 Azolla
   through medical, recipe, needs, greenhouse, and power ports.
4. **Sensing and field security:** 196 infrasound, 200 caltrops, 203 tripwires,
   and 205 geoelectrical surveys.
5. **Diplomacy, navigation, training:** 198 exchange, 202 astrolabe, and 206
   filmstrip instruction.
6. **Continental intelligence:** 207 OTH radar after radio/weather contacts
   and long-range location data are stable.
7. **History projection:** 208 last, after all event publishers and save
   migration behavior are fixed.

## 4. Step-by-step integration contracts

### [193] Geothermal Lithium Refining — `LithiumRefiningSystem`

- Extend the `BrineWaterSystem`/foundry material boundary with brine feed,
  acid precipitation, impurity removal, carbonate yield, cathode coating,
  separator/winding, cell quality, pack assembly, and hazardous-waste outputs.
- Use the foundry treaty ID `treaty_brine_pipe_and_iodine_exchange` for
  exchange obligations; do not create a second District 8 accords authority.
- Represent battery packs as validated item IDs with capacity, charge,
  temperature, degradation, and compatible equipment tags. Apply the scout
  runtime modifier through equipment loadouts, not a global battery cheat.
- `LithiumRefiningPanel` controls tanks, rollers, winding, and QC. Acceptance
  requires authored brine/acid inputs to produce 10 valid rechargeable packs
  accepted by the expedition armory.

### [194] Regenerative Open-Hearth Furnace — `OpenHearthSystem`

- Extend the `SilentFoundrySystem` work-order boundary with hearth charge,
  refractory condition, checker-chamber heat recovery, flue reversal,
  pyrometry, slag, alloy chemistry, emissions, and operator exposure.
- Register rusty scrap, slag, recovered steel, and specialty alloy products in
  foundry data. The 500 kg to 450 kg conversion is a fixture, not a universal
  hard-coded yield.
- `OpenHearthPanel` renders valves, temperature, slag, and quality. Acceptance
  requires a valid high-tensile output to unlock the authored structural/tool
  recipe and records fuel/maintenance effects.

### [195] Emergency Tracheotomy — `AirwayTreatmentSystem`

- Build on the existing medical host and respiratory degeneration path, with
  typed airway obstruction, inhalation exposure, oxygenation, airway device,
  bronchoscopy, suction, bleeding, infection, and recovery state.
- Connect to `CombatTraumaSystem` and the shared survivor effect port. Do not
  set health, oxygen, or work capacity directly from the modal.
- `TracheotomyModal` submits triage and procedure commands. Success saves the
  authored scout case only when equipment, skill, time, and patient state pass;
  failure can leave a persistent airway or infection complication.

### [196] Infrasound Array — `InfrasoundArraySystem`

- Add sensor stations, calibration, frequency band `0.01–20 Hz`, local noise,
  wind/temperature attenuation, arrival times, azimuth triangulation, source
  classification, and warning records.
- Integrate event sources from `WeatherSystem` and `RadioHostSession` through
  deterministic contact adapters. A 300-mile explosion is a data-authored
  distance/signal fixture, not an unconditional global detector.
- `InfrasoundArrayPanel` displays waveform, azimuth, intensity, confidence,
  and siren state. Acceptance creates a fallout-preparation warning only when
  the configured signal is detectable and triangulated.

### [197] Pickling Cellar — `PicklingCellarSystem`

- Track vinegar mother, lactic culture, crock, brine salinity/pH, temperature,
  oxygen exposure, contamination, batch mass, storage, and shelf-life.
- Use `RecipeCatalog`, `GoodsCatalog`, and `NeedsSystem`; outputs are distinct
  sauerkraut, cucumber, mushroom, and egg ration IDs with nutrition and
  probiotic metadata.
- `PicklingCellarPanel` manages crocks and feed timers. Acceptance converts
  the authored 100 kg cabbage fixture into 20 valid crocks and applies vitamin
  C through the normal dietary ledger. Two-year stability requires the
  configured storage conditions.

### [198] Hostage Exchange DMZ — `HostageExchangeSystem`

- Use `CrossingArbitrationSystem`, `MusterHostSession`, and
  `NarrativeEncounterSystem` for location security, prisoner identity,
  parity, leverage, escorts, deadlines, and faction response.
- Reuse Batch 10 prisoner/citizenship provenance; never collapse a hostage,
  prisoner, and citizen into one boolean. DMZ security can fail, stall, or
  escalate to combat.
- `PrisonerExchangeModal` shows parity, leverage, tension, and safe-release
  conditions. Acceptance returns the authored surgeon only after a valid
  bilateral exchange and records the treaty/relationship result.

### [199] Azolla Bio-Fertilizer — `AzollaSystem`

- Extend the greenhouse boundary with basin water, fern biomass, nitrogen
  fixation, light/power, contamination, harvest, composting, soil nitrogen,
  and plot application.
- Apply fertility through the existing `GreenhouseSystem` soil state or an
  explicit adapter, never through a panel-local crop-yield multiplier. Cap
  fertility and define depletion/contamination; no infinite free resource.
- `AzollaBasinPanel` manages skim, compost, and soil application. Acceptance
  raises the authored plot to 100% fertility and applies the configured +25%
  yield modifier for the valid fertility window.

### [200] Hardened Caltrops — `CaltropDefenseSystem`

- Use `LocationLayoutSystem`, `TacticalCombatSystem`, and
  `WarlordDoctrineSystem` for craft batch, deployment zone, visibility,
  vehicle class, tire/track vulnerability, recovery, and attack resolution.
- Caltrops create a static obstacle/threat modifier; they do not directly
  damage every actor or guarantee convoy immobilization.
- `CaltropDefensePanel` manages stock, pass placement, and reports. Acceptance
  resolves the authored motorized convoy with enough immobilized vehicles to
  force the configured dismount/retreat outcome.

### [201] Ductile Iron Casting — `DuctileIronSystem`

- Extend foundry metallurgy with melt chemistry, magnesium inoculation timing,
  nodularity inspection, mold quality, thermal treatment, fatigue rating,
  machining allowance, and component compatibility.
- Use `ResearchSystem` for the metallurgy unlock and `SilentFoundrySystem` for
  the work order. The crankshaft must be an item/component accepted by the
  locomotive repair contract.
- `DuctileIronPanel` renders ladle reaction, inspection, and yield. Acceptance
  installs a valid crankshaft and returns the existing armored locomotive to
  service without bypassing engine condition state.

### [202] Brass Astrolabe — `AstrolabeSystem`

- Track brass stock, engraving precision, rete/alidade calibration, star
  catalogue, observer skill, ash-cloud visibility, and expedition equipment
  compatibility.
- Integrate with `ExpeditionSystem`, `ResearchSystem`, and `CraftingSystem`.
  Navigation speed is a night-only, visibility-gated modifier with a source
  and duration; it is not flawless navigation.
- `AstrolabeCraftModal` handles engraving and alignment. Acceptance applies
  the configured +20% night navigation speed to an equipped expedition under
  valid star-sight conditions.

### [203] Infrared Tripwires — `LaserTripwireSystem`

- Add perimeter sectors, beam endpoints, alignment tolerance, receiver power,
  continuity, fog/ash attenuation, false alarms, tamper state, and dispatch
  events. Connect power through `PowerGridSystem` and the host hazard adapter.
- `LaserTripwirePanel` shows alignment and breach sectors; it does not invoke
  combat directly. Sentry response is routed through the tactical encounter
  authority.
- Acceptance requires an authored infiltrator to break a valid beam, create a
  silent command-center alarm, and make the sentry squad eligible for an
  ambush resolution.

### [204] Tungsten Carbide Inserts — `CarbideSinteringSystem`

- Model powder/binder reservation, compaction, vacuum/temperature profile,
  press wear, hardness inspection, edge geometry, and tool-life degradation.
- Register inserts as durable tooling with compatible machine tags. “Never
  dull” becomes a high tool-life tier with wear and breakage.
- `CarbideSinteringPanel` controls dies, kiln, and hardness test. Acceptance
  produces 10 valid inserts and applies the authored machining-speed modifier
  to compatible lathes/mills, with condition-based falloff.

### [205] Geoelectrical Survey — `GeoelectricalSurveySystem`

- Add electrode array geometry, current pulse, apparent resistivity,
  chargeability, contact resistance, weather/noise, line placement, and
  interpretation confidence.
- Integrate `LocationLayoutSystem`, `ResearchSystem`, and geological deposit
  data. An anomaly becomes a mine target only after the configured confidence
  and follow-up validation; survey noise and false positives are possible.
- `GeoelectricalSurveyPanel` renders spacing, pseudosection, contours, and
  target map. Acceptance discovers the authored Sector 7 copper-sulfide
  deposit and creates a validated prospect location.

### [206] Filmstrip Training — `FilmstripTrainingSystem`

- Track filmstrip condition, projector/lamp state, lesson frames, instructor,
  attendance, comprehension, and apprentice XP events.
- Use `ResearchSystem`, `DutyRosterSystem`, and `JournalSystem`. XP is awarded
  to eligible attendees through the normal skill system, not broadcast to all
  medics regardless of attendance.
- `FilmstripProjectorModal` controls focus, advance, lesson, and classroom.
  Acceptance gives the authored +15 medical XP to attending apprentice medics
  after the emergency-amputation lesson completes.

### [207] OTH Ionospheric Radar — `OthRadarSystem`

- Add HF pulse schedule, frequency, ionospheric state, skip distance,
  Doppler return, antenna/power, noise, contact confidence, and track
  projection. Integrate `RadioHostSession`, `WeatherSystem`, and
  `LocationLayoutSystem`.
- Long-range contacts require authored geometry, propagation, and target
  emissions. The 800 km / four-day warning is an acceptance fixture when the
  configured conditions pass, not a universal range or guaranteed warning.
- `OthRadarPanel` renders tuner, skip map, tracks, and alerts. Acceptance
  creates a raid-preparation event with four days of lead time for the authored
  armored-vehicle mobilization.

### [208] Campaign Timeline — `CampaignHistoryLedger` + `TimelineProjectionSystem`

- Publish structured events from `JournalSystem`, `CenturySeed`, treaties,
  battles, deaths, births, disasters, research, construction, and expedition
  outcomes. Keep causal links and branch IDs so alternate choices remain
  visible without rewriting history.
- Persist the ledger in the checksummed Batch 13 save or a deliberately
  migrated history store. Export chronicles through Core `IJsonSerializer`/
  `IFileIO` to user-controlled paths; Godot only selects the destination.
- `CampaignTimelinePanel` provides horizontal navigation, filters, event
  cards, dynasty portraits from validated survivor IDs, treaty/battle nodes,
  branch expansion, and export status.
- Acceptance renders a clean campaign’s Day 1–Year 100 history without gaps;
  legacy saves show explicit provenance/gap markers until their history can be
  reconstructed from real records.

## 5. Data, host, and UI deliverables

Add validated data for brine chemistry, battery components, foundry heat
curves, medical airway cases, infrasound sources, fermentation recipes,
hostage/faction terms, greenhouse soil effects, vehicle vulnerabilities,
metallurgy, star visibility, perimeter sectors, tooling, geological anomalies,
training lessons, OTH contacts, and timeline event schemas.

Create these presentation views with typed read models and command results:

`LithiumRefiningPanel`, `OpenHearthPanel`, `TracheotomyModal`,
`InfrasoundArrayPanel`, `PicklingCellarPanel`, `PrisonerExchangeModal`,
`AzollaBasinPanel`, `CaltropDefensePanel`, `DuctileIronPanel`,
`AstrolabeCraftModal`, `LaserTripwirePanel`, `CarbideSinteringPanel`,
`GeoelectricalSurveyPanel`, `FilmstripProjectorModal`, `OthRadarPanel`, and
`CampaignTimelinePanel`.

Panels must be headless-safe, accessible, idempotently refreshable from
state-change events, and free of hard-coded inventory, XP, combat, treaty,
timeline, or morale state.

## 6. Verification and exit criteria

Core tests must cover deterministic replay, capture/restore, migration,
malformed-envelope rejection, units/ranges, by-products, contamination,
maintenance, sensor noise, medical complications, faction escalation,
navigation/weather modifiers, and all 16 acceptance fixtures.

History tests must verify stable event sequence, causal/branch references,
deduplication, legacy-gap marking, export round-trip, and save/load of a
100-year fixture. Catalog tests must reject unknown IDs, duplicate event IDs,
invalid locations, unsafe modifiers, and out-of-range physical values.

Add host self-tests for `--batch13-selftest` and `--batch13-ui-selftest`, then
run the repository gates:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --asset-registry-selftest
godot --headless --path . -- --playable-shell-selftest
godot --headless --path . -- --ui-layout-selftest
./scripts/ci/godot-asset-gate.sh
```

Batch 13 is ready when all 16 fixtures pass from clean and migrated saves,
replay identically with the same seed, survive save/load at active job and
timeline boundaries, and the campaign visualizer is derived from structured
history rather than UI-generated claims.
