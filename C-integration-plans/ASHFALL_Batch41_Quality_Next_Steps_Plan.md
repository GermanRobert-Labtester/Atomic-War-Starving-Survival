# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 41)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 641–656

## Outcome

Batch 41 adds hospice, archival cartography, cheese affinage, analogue fire control, circus arts, ceramics, silvopasture, civic heraldry, tide prediction, natural dyes, field medicine, woodwinds, perimeter sensing, distributed wind, district heating, and long-term planning.

Reuse:

- **MedicalSystem**, **FinalWishSystem**, NeedsSystem, GenerationalSuccessionEngine, and the psychological-care contract own hospice, grief, dignity, and rehabilitation.
- **JournalSystem**, CraftingSystem, LocationLayoutSystem, and archive state own atlases, heraldry, calligraphy-adjacent records, and maps.
- **RecipeCatalog/data**, GreenhouseSystem, GoodsCatalog, and NeedsSystem own cheese, dyes, silvopasture, and food.
- **SilentFoundrySystem**, TacticalCombatSystem, WarlordDoctrineSystem, ResearchSystem, and the metrology contract own fire control, pottery, sensors, and equipment.
- **StealthDiveInstance**, surface-vessel state, WeatherSystem, LocationLayoutSystem, and RadioHostSession own tide and maritime operations.
- **DutyRosterSystem**, DiseaseSystem, and the shared power/thermal contract own field training, epidemiology, VAWT, and district heating.
- **CrossingArbitrationSystem**, governance, and GenerationalSuccessionEngine own civic heraldry and plan approval.
- **EpilogueMatrix** and SaveChecksum own the long-range plan milestone and presentation.

## Batch entry gates

1. **Hospice gate:** define symptom comfort, consent, advance wishes, family/vigil, privacy, death record, grief, and palliative staff.
2. **Archive/cartography gate:** define map version, source, scale, leather/paper materials, update, access, and diplomatic gift value.
3. **Food/affinage gate:** define temperature/humidity, cultures, contamination, wheel turning, aging, yield, and trade.
4. **Fire-control gate:** define measurement inputs, ballistic model, wind/target motion, analogue error, crew, and safe use.
5. **Performance/craft gate:** define training, consent, rigging, injury, sound, materials, firing, and accessibility.
6. **Silvopasture gate:** define land layers, animal rotation, tree growth, shade, fertility, carrying capacity, and disease.
7. **Civic identity gate:** connect heraldry to democratic governance, document use, amendments, and generational records.
8. **Tide/dye/medicine gate:** define harmonic uncertainty, dye chemistry, field-trauma competence, and resource use.
9. **Sensor/power gate:** unify PIR, wind, VAWT, district heat, and environment telemetry.
10. **Urban planning gate:** define zoning, capacity, phases, approvals, revision, and conflict with current layout.

## Delivery order

1. **641, 642, 643:** establish hospice and heritage/food preservation.
2. **644, 645, 646:** add fire-control, performance, and pottery.
3. **647, 648, 649:** add silvopasture, heraldry, and tide prediction.
4. **650, 651, 652:** add dyes, field trauma, and woodwinds.
5. **653, 654, 655:** extend sensing, wind energy, and district heat.
6. **656:** finish with the revision-controlled master plan.

## Step integration matrix

### [641] Hospice and Palliative Care — HospicePalliativeCarePanel

**Core integration:** Add a hospice/comfort-care state over MedicalSystem, FinalWishSystem, NeedsSystem, GenerationalSuccessionEngine, psychological care, family/vigil, symptom management, and death/grief records.

**Smallest vertical slice:** Enroll one consenting terminally ill survivor, assign a care team, manage one symptom plan/final wish, and record a dignified death or care transition.

**Acceptance gate:** Prognosis uncertainty, pain/nausea/breathlessness, medication, consent, advance wishes, family, privacy, spiritual preference, nurse capacity, death record, and grief are persisted. Hospice improves comfort and grief processing; it cannot guarantee all deaths are attended or reduce grief by a fixed 70%.

### [642] Leatherbound Atlas Edition — LeatherboundAtlasPanel

**Core integration:** Extend JournalSystem, CraftingSystem, LocationLayoutSystem, leather/paper/ink/gold, chart versions, cartographic sources, bookbinding, archive, and diplomatic gifting.

**Smallest vertical slice:** Compile one chart set, bind one edition, attach sources/version, and present it in one negotiation.

**Acceptance gate:** Map scale, source date, uncertainty, leather/paper, signatures, binding, correction, condition, owner, access, and gift response are saved. Prestige comes from provenance and faction context; it cannot automatically become the most impactful gesture.

### [643] Cheese Affinage Cave — CheeseAffinageCavePanel

**Core integration:** Extend RecipeCatalog/data, GreenhouseSystem, NeedsSystem, GoodsCatalog, cave facility, cultures, humidity/temperature, wheel rotation, rind, aging, storage, and trade.

**Smallest vertical slice:** Start one cheese wheel, inoculate/turn it, age through one interval, inspect it, and settle food/trade outcome.

**Acceptance gate:** Milk/plant input, culture, salt, cave temperature/humidity, airflow, rind, contamination, aging, wheel mass, spoilage, and quality are authoritative. Twelve months and premium value require valid time and test results; blue mold is not success by default.

### [644] Mechanical Fire-Control Computer — FireControlComputerPanel

**Core integration:** Extend TacticalCombatSystem, SilentFoundrySystem, WarlordDoctrineSystem, measurement devices, analogue computation, artillery, weather/wind, target motion, crew, and ammunition.

**Smallest vertical slice:** Feed one range/bearing/wind observation into the computer, calculate a solution with error bounds, fire one test shot, and compare impact.

**Acceptance gate:** Drum calibration, integrator friction, range, bearing, wind, target movement, projectile, crew, elevation, azimuth, computation error, misfire, and correction are saved. First-round accuracy is a scenario result; bracketing remains possible.

### [645] Circus Arts Troupe — CircusTroupePanel

**Core integration:** Extend NeedsSystem, DutyRosterSystem, GenerationalSuccessionEngine, cultural-event state, training, rigging, safety, venue, and audience.

**Smallest vertical slice:** Train one act, inspect apparatus, obtain participant consent/clearance, perform safely, and record audience response.

**Acceptance gate:** Age, consent, skill, fatigue, injury risk, rigging, weather/venue, accessibility, rehearsal, and audience are persisted. A performance can be a high morale event; it cannot be the greatest event by fiat or expose children to unmodeled risk.

### [646] Salt-Glazed Stoneware — SaltGlazedPotteryPanel

**Core integration:** Extend SilentFoundrySystem/CraftingSystem, clay, kiln, salt, firing profile, thermal shock, glaze/texture, vessel recipes, NeedsSystem, and trade.

**Smallest vertical slice:** Throw one vessel, fire it through a salt-glaze cycle, inspect it, and use it in one food/storage task.

**Acceptance gate:** Clay, moisture, wall thickness, kiln temperature, salt charge, atmosphere, firing time, warping, cracks, food safety, capacity, and fuel are saved. A complete Mead Hall set requires actual pieces and quality checks.

### [647] Silvopasture Agroforestry — SilvopasturePanel

**Core integration:** Add a layered land-use state over GreenhouseSystem, LocationLayoutSystem, NeedsSystem, livestock, orchard/timber, paddock rotation, soil fertility, shade, water, and disease.

**Smallest vertical slice:** Plant or register one tree row, assign one paddock rotation, graze animals under it, and measure fruit/timber/manure/animal outputs.

**Acceptance gate:** Tree age, canopy, shade, species compatibility, animal load, forage, rotation, water, disease, soil, fruit, timber, and regeneration are persisted. Triple productivity is a tested land-use result, not an unconditional multiplier.

### [648] Civic Flag and Heraldry — HeraldryDesignPanel

**Core integration:** Extend CrossingArbitrationSystem, NeedsSystem, GenerationalSuccessionEngine, Constitution/governance, design submissions, democratic vote, official document/building use, and archive.

**Smallest vertical slice:** Submit designs, validate electorate, vote, publish the winning flag/motto, and attach it to one official record or building.

**Acceptance gate:** Submission authorship, symbols/tinctures, cultural permissions, electorate, quorum, ballot, tie/recount, version, use, dissent, and generational archive are saved. Civic pride is a bounded identity effect; no permanent global morale trait from a design screen.

### [649] Mechanical Tide Prediction Machine — TidePredictionPanel

**Core integration:** Extend StealthDiveInstance/surface vessels, WeatherSystem, LocationLayoutSystem, SimClock, tide constituents, harmonic machine, coastal routes, and chart versions.

**Smallest vertical slice:** Calibrate constituent amplitudes/phases for one site, print a prediction interval, and use it to schedule one harbor operation.

**Acceptance gate:** Site, epoch, constituents, amplitude/phase, machine drift, weather/storm surge, river flow, observation reference, and uncertainty are persisted. Twelve-month tables are useful forecasts, not exact guarantees under abnormal storms.

### [650] Natural Wool Dye Library — NaturalDyeLibraryPanel

**Core integration:** Extend GreenhouseSystem, NeedsSystem, CraftingSystem, plant/mineral mordant inputs, wool, dye extraction, pH, colorfastness, swatches, and textile recipes.

**Smallest vertical slice:** Prepare one mordant/dye bath, dye one yarn sample, measure colorfastness, and add it to the library.

**Acceptance gate:** Plant mass, mordant, bath temperature/pH, fiber, time, color, wash/light fastness, contamination, waste, and sample provenance are saved. Twenty-four colors require distinct stable recipes and samples, not palette labels.

### [651] Field Surgery Kit and Wilderness Trauma Training — FieldSurgeryTrainingPanel

**Core integration:** Extend MedicalSystem, CombatTraumaSystem, DutyRosterSystem, expedition roster, kit inventory, training modules, competency, trauma scenarios, and field-treatment outcomes.

**Smallest vertical slice:** Assemble one kit, train and assess one scout, then resolve one field injury using the competency record.

**Acceptance gate:** Kit contents, sterility, tourniquet/wound-packing/airway/needle-decompression skill, scenario, time-to-care, supplies, fatigue, and referral are persisted. A 60% prevention reduction requires baseline comparison and actual trained coverage.

### [652] Reed Woodwind Ensemble — WoodwindEnsemblePanel

**Core integration:** Extend CraftingSystem, NeedsSystem, DutyRosterSystem, SoundManager, instrument geometry, reeds, tuning, rehearsal, venue, and archive.

**Smallest vertical slice:** Craft/tune one woodwind, assign a player, rehearse a short piece, and perform it in a valid room.

**Acceptance gate:** Bore, tone holes, reed, material, tuning, player skill, fatigue, audio asset, acoustics, and audience are saved. The first concert is a cultural record and bounded event, not an automatic maximum morale event.

### [653] PIR Perimeter Security — InfraredMotionSensorPanel

**Core integration:** Extend the Batch 15/21/40 sensor and night-watch state over LocationLayoutSystem, TacticalCombatSystem, power, sentry routing, animal/human classification, weather, occlusion, and alerts.

**Smallest vertical slice:** Install one sector, detect one moving contact, classify with confidence, route an alert, and resolve guard response.

**Acceptance gate:** Sensor angle/range, ambient temperature, foliage, occlusion, battery/power, false positives, animal/human uncertainty, alert latency, and maintenance are persisted. Zero undetected intrusion is not an acceptable universal guarantee.

### [654] Vertical-Axis Wind Turbines — VawTurbinePanel

**Core integration:** Add generation assets over WeatherSystem, LocationLayoutSystem, shared power/grid, ventilation exhaust, turbine condition, inverter/storage, noise, and maintenance.

**Smallest vertical slice:** Install one VAWT at a validated site, simulate wind/exhaust flow, produce measured power, and settle it into the grid/storage.

**Acceptance gate:** Wind speed/direction, turbine curve, cut-in/cut-out, torque, exhaust flow, losses, inverter, storage, structure, noise, and maintenance are authoritative. Two hundred kilowatts requires enough installed capacity and measured wind; “no fuel cost” does not mean no service cost.

### [655] Geothermal District Heating — DistrictHeatingPanel

**Core integration:** Add a district-heat network over shared power/thermal/water, LocationLayoutSystem, NeedsSystem, geothermal source, insulated mains, heat exchangers, balancing valves, buildings, leaks, and maintenance.

**Smallest vertical slice:** Connect one source to one building, balance a heat exchanger, simulate a day/night cycle, and compare demand with individual heating.

**Acceptance gate:** Source flow/temperature, pipe pressure, insulation, exchanger efficiency, building load, outdoor weather, leak, maintenance, and fallback heating are persisted. Coverage and savings are measured; 100% buildings and 55% savings are not guaranteed by a map overlay.

### [656] Grand Architectural Master Plan — UrbanMasterPlanPanel

**Core integration:** Add a revision-controlled plan over LocationLayoutSystem, GenerationalSuccessionEngine, ResearchSystem, JournalSystem, infrastructure capacity, zoning, land rights, utilities, and governance approval.

**Smallest vertical slice:** Draft zones for one district, validate capacity/conflicts, approve a phase, and use it to guide one construction job.

**Acceptance gate:** Zone geometry, land rights, population projection, utility capacity, transport, hazards, green space, phase, approval, exceptions, revision, and conflict resolution are saved. A plan influences future jobs; it cannot permanently grant +20% efficiency or erase future constraints without an implemented planning modifier.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes with stable ordinal ordering; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact reservation and settlement for medicine, documents, leather, cheese, metal, powder, clay, salt, dyes, textiles, sensors, power, heat, water, food, animals, instruments, and construction stock.
- Explicit interruption, cancellation, failure, spoilage, contamination, leakage, drift, injury, false alarm, outage, maintenance, stale-data, and governance outcomes.
- Canonical snake_case IDs and ordinal validation through the data-integrity rules.

Use explicit units: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, flow_m3_s, humidity_fraction, pH, redox_mv, speed_kmh, signal_db, confidence_fraction, condition_fraction, and population_count.

## Data authority and content work

Extend existing catalogs where possible. Candidate new authority files, only if current catalogs cannot express the domain, are:

- hospice_care_defs.json
- atlas_binding_defs.json
- cheese_affinage_defs.json
- fire_control_defs.json
- circus_act_defs.json
- pottery_firing_defs.json
- silvopasture_defs.json
- heraldry_rules.json
- tide_machine_defs.json
- natural_dye_defs.json
- field_trauma_defs.json
- woodwind_defs.json
- pir_sensor_defs.json
- vawt_defs.json
- district_heating_defs.json
- urban_plan_defs.json

Every new or edited authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and loader tests.

## Godot host and UI work

Build these panels only after their typed host contracts exist:

HospicePalliativeCarePanel, LeatherboundAtlasPanel, CheeseAffinageCavePanel, FireControlComputerPanel, CircusTroupePanel, SaltGlazedPotteryPanel, SilvopasturePanel, HeraldryDesignPanel, TidePredictionPanel, NaturalDyeLibraryPanel, FieldSurgeryTrainingPanel, WoodwindEnsemblePanel, InfraredMotionSensorPanel, VawTurbinePanel, DistrictHeatingPanel, and UrbanMasterPlanPanel.

Each panel must bind to typed host sessions/read models, expose loading/empty/error states, show units and confidence where relevant, and issue validation-returning commands. No Bind(object), placeholder arrays, direct Core mutation, hard-coded success, or unsupported 3D-only dependency is permitted. New dirty stores must be wired through Main.Setup, Main.Save, and Main.Flush.

## Verification plan

Run focused tests after each vertical slice, then:

~~~
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --asset-registry-selftest
godot --headless --path . -- --playable-shell-selftest
godot --headless --path . -- --ui-layout-selftest
godot --headless --path . -- --save-slots-selftest
./scripts/ci/godot-asset-gate.sh
./scripts/ci/no-legacy-residue.sh
~~~

Add focused coverage for hospice consent/symptoms/grief, atlas sources/binding, cheese aging/contamination, fire-control error, performance safety, pottery firing, silvopasture yields, heraldry vote, tide uncertainty, dye fastness, field competence, PIR false positives, VAWT output, district heat flow, urban-plan revisions, and clean/tampered/missing-checksum/legacy/future-version/atomic-write saves.

## Definition of done

Each Step 641–656 must be implemented, explicitly deferred with a dependency, or rejected as unsupported. Implemented steps require Core ownership, typed Godot UI, schema-valid data, deterministic and checksummed persistence, dirty-store flushing, tunable claims, and the complete Godot-only verification pipeline.
