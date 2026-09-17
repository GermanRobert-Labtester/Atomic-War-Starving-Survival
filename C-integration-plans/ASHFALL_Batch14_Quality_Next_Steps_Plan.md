# ASHFALL: Batch 14 Integration Plan

**Steps:** 209–224<br>
**Generated:** 2026-08-19<br>
**Status:** Execution-ready, architecture-aligned<br>
**Host:** Godot 4.7+ (.NET 8 C#)<br>
**Core target:** .NET Standard 2.1

## 1. Integration decision

Batch 14 is a content batch, not Expansion 14. Preserve the canonical
12-entry `ExpansionSuite` and implement these features as Core state machines,
typed host sessions, and Godot presentation.

The batch depends on the shared power, inventory, nutrition, radio, location,
threat, and history contracts established by earlier batches. Existing live
authorities include `BrineWaterSystem`, `SilentFoundrySystem`,
`GreenhouseSystem`, `WeatherSystem`, `RadioHostSession`, `CrossingArbitrationSystem`,
`MarketSystem`, `DiseaseSystem`, `TacticalCombatSystem`, `ResearchSystem`,
`JournalSystem`, `CaregivingSystem`, and `CenturySeed`.

Several requested names are not active authorities: `DynamicEconomySystem`,
`PowerGridPanel`, and `ShelterHazardLoop` are not Core state owners, and
`MemorialLedgerPanel` does not yet exist. Use `MarketSystem`, the shared
`PowerGridSystem`, a host hazard adapter, and a new Core memorial ledger. No
Godot node may own fuel, power, combat, disease, crop, treaty, or history
state.

Absolute claims are converted to measurable conditions. Fuel caverns are not
“immune” to attack, surge protection does not mean zero damage, communications
are not literally un-interceptable, and permanent morale/yield effects require
explicit data and maintenance rules.

## 2. Shared foundation and persistence

### 2.1 Batch save

Add `Batch14State` and `Batch14SaveStore` at
`user://saves/slot-N/batch14_save.json`. Use a checksummed versioned envelope,
`AtomicFileWriter`, explicit migrations, deep-copy capture/restore, and strict
rejection of malformed current envelopes. `SaveLoadHostSession` will include
the file through its existing `_save.json` discovery and manifest hashing.

New feature state belongs here unless an existing authoritative store is
deliberately migrated. Do not extend an existing state and create a second
Batch 14 writer for the same fields. Record all machine condition, resource
reservations, faults, modifiers, and historical outcomes.

### 2.2 Shared prerequisites

Before feature slices, verify these cross-batch contracts:

- `PowerGridSystem` supports large transient loads, storage, priority,
  generator/turbine adapters, faults, surge events, and outage recovery;
- `InventoryLedger` handles liters, kilograms, components, durable tools,
  ammunition, fuel quality, and by-products without unit ambiguity;
- `WeatherSystem` publishes lightning, wind, fog, ash, radon, and storm events;
- `LocationLayoutSystem` and `TacticalCombatSystem` support sectors, choke
  points, sensor contacts, mine/obstacle hazards, and response encounters;
- `ResearchSystem`, `JournalSystem`, `GreenhouseSystem`, `NeedsSystem`,
  `RadioHostSession`, `DiseaseSystem`, and `CenturySeed` expose typed effect
  and event ports;
- `CampaignHistoryLedger` from Batch 13 accepts construction, disaster,
  combat, culture, and memorial records.

Extend `CatalogIntegrityValidator` for every new item, machine, location,
recipe, disease, threat, signal, effect, and history ID. All new data uses
`snake_case` IDs and `schema_version`.

## 3. Ordered vertical slices

1. **Foundation:** Batch 14 save, high-load power, unit-safe inventory,
   weather/hazard events, and `--batch14-selftest` scaffolding.
2. **Fuel and metallurgy:** 209 cavern storage, 210 arc furnace, 217 bronze
   bushings, and 220 diamond lapping.
3. **Clinical and culinary:** 211 skin flaps and 213 acetator, reusing Batch 11
   skin reconstruction and pickling/recipe contracts.
4. **Grid and perimeter protection:** 212 surge arresters, 219 microwave
   radar, and 216 claymore field resolution.
5. **Trade and agriculture:** 214 customs, 215 strawberries, and 218 almanac.
6. **Geological and communications:** 221 downhole logging and 223 meteor
   burst, after Batch 13 survey/radio infrastructure is stable.
7. **Memory and monument:** 222 magic lantern and 224 memorial/flame system,
   after structured survivor death/history events are available.

## 4. Step-by-step integration contracts

### [209] Salt-Cavern Hydrocarbon Storage — `CavernStorageSystem`

- Use `BrineWaterSystem`/geological location data for dissolution, cavern
  volume, salt roof/floor, sonar survey, brine disposal, seal integrity,
  injection manifold, pressure, temperature, fuel quality, and leak detection.
- Store refined diesel/fuel oil as typed inventory with batch provenance and
  safety class. Power and fuel systems consume the reservoir through a
  controlled withdrawal port; panels cannot create liters.
- `CavernStoragePanel` shows geometry, injection, reserve, pressure, and seal
  telemetry. Acceptance stores 50,000 liters in the authored cavern and keeps
  it below the configured loss/pressure thresholds; it remains vulnerable to
  geological failure, sabotage, and maintenance neglect.

### [210] Electric Arc Furnace — `ElectricArcFurnaceSystem`

- Extend `SilentFoundrySystem` with graphite electrode wear, arc strike,
  transformer/power load, melt bath, alloy charge, slag, optical composition,
  refractory damage, operator exposure, and tap/cooling state.
- Connect large transient loads to `PowerGridSystem`; a geothermal turbine is
  an energy source adapter, not a direct panel-controlled power pool.
- `ElectricArcPanel` controls electrode height, arc power, bath inspection,
  and tap. Acceptance produces the authored 300 kg surgical stainless batch
  only when power quality, charge, and safety interlocks pass.

### [211] Rotational Skin Flap Surgery — `SkinFlapSystem`

- Extend the Batch 11 medical reconstruction boundary with wound depth,
  exposed structures, flap donor site, vascular pedicle, rotation geometry,
  sutures, perfusion, necrosis, infection, rehabilitation, and mobility.
- Connect to `CombatTraumaSystem`, medical save state, and survivor effects;
  preserve a single injury/health authority. No modal may mark a limb saved
  without a valid perfusion and recovery outcome.
- `SkinFlapSurgeryModal` submits mapping, rotation, and postoperative care
  commands. Acceptance preserves the authored scout leg when the procedure
  passes, while failures remain medically visible and deterministic.

### [212] Lightning Surge Arresters — `SurgeProtectionSystem`

- Add surface line sectors, grounding impedance, MOV health, spark-gap state,
  surge energy, lightning strike events, EMP coupling, protected equipment,
  and maintenance replacement.
- Integrate `WeatherSystem`, `PowerGridSystem`, and radio/computer equipment
  protection ports. “Zero electrical damage” is a fixture for a correctly
  grounded direct strike, not a universal immunity flag.
- `SurgeArresterPanel` renders discharge, block health, strikes, and protected
  loads. Acceptance diverts the authored mast strike while preserving arrestor
  wear and recording the event in history.

### [213] Vinegar Acetator — `VinegarAcetatorSystem`

- Track cider batch, alcohol/acetic conversion, beechwood packing, aeration,
  recirculation, temperature, acidity, contamination, capacity, and yield.
- Use `RecipeCatalog`, `GoodsCatalog`, and medical/crafting reagent tags. A
  culinary vinegar and a disinfectant reagent must remain distinct items with
  appropriate safety metadata.
- `AcetatorPanel` controls aeration and recirculation. Acceptance converts the
  authored 50-liter cider fixture to 50 liters at the configured 5% acidity in
  48 hours when airflow and temperature conditions pass; conversion loss is
  possible outside the fixture.

### [214] Highway Customs Checkpoint — `CustomsCheckpointSystem`

- Use `CrossingArbitrationSystem`, `MarketSystem`, `DiseaseSystem`, and
  location/route data for caravan manifests, tariffs, inspection, quarantine,
  contraband, treatment, release, and faction trust.
- Do not add a `DynamicEconomySystem` duplicate. Tariffs post through the
  existing market/ledger authority and disease cases use `DiseaseCatalog` and
  quarantine protocols; add a scarlet-fever definition only if the catalog
  schema and treatment data support it.
- `CustomsPostPanel` shows cargo, fever screening, tax, quarantine, and seizure
  records. Acceptance collects the authored 50-scrap daily tariff under valid
  traffic and quarantines the authored infected merchant without bypassing
  disease transmission or due-process outcomes.

### [215] Vertical Strawberry Towers — `StrawberryTowerSystem`

- Extend `GreenhouseSystem` with tower capacity, drip nutrients, water,
  lighting/power, pollination, temperature, blight, flowering, harvest, and
  fruit-quality state.
- Use `RecipeCatalog` for fruit leather/pie outputs and `NeedsSystem` for
  nutrition and morale. The +20 dining effect is sourced, bounded, and tied to
  a successful shared meal; scurvy relief comes from the dietary ledger.
- `StrawberryTowerPanel` manages towers and harvest. Acceptance converts 10 kg
  valid strawberries into the authored pie recipe and applies the configured
  dining morale event without a global UI-only buff.

### [216] Command-Detonated Claymore Fields — `ClaymoreMinefieldSystem`

- Use `LocationLayoutSystem`, `TacticalCombatSystem`, and
  `WarlordDoctrineSystem` for gully placement, field-wire state, arming,
  command authority, directional cone, target classification, fragmentation,
  unexploded ordnance, and post-raid recovery.
- The minefield is a combat encounter modifier, not a direct kill button.
  Model friend/foe uncertainty, line-of-fire, ammunition, and misfire/failure
  states in Core.
- `ClaymoreMinefieldPanel` controls placement, arming, and fire command.
  Acceptance resolves the authored assault squad into the configured morale
  break/retreat outcome when the directional engagement succeeds.

### [217] Centrifugal Bronze Bushings — `BronzeCentrifugalSystem`

- Track phosphor-bronze composition, mold RPM, pour timing, centrifugal
  density, cooling, bore/clearance, inspection, and machine installation.
- Connect to `SilentFoundrySystem` and `ResearchSystem`; bushing compatibility
  must be checked by the turbine/lathe machine adapter.
- `BronzeCentrifugalPanel` renders RPM, pour, density, and stock. Acceptance
  installs a valid turbine bushing and applies the configured +15% efficiency
  modifier while preserving wear and maintenance behavior.

### [218] Copperplate Almanac — `AlmanacSystem`

- Track copperplate design, press, ink/paper, lunar/solar calendar data,
  planting windows, crop/season references, and edition distribution.
- Use `JournalSystem`, `GreenhouseSystem`, `CraftingSystem`, and `SimClock`.
  Treat moon-phase scheduling as a data-authored calendar signal; it must not
  override weather, light, water, or crop biology.
- `AlmanacPrintModal` handles engraving and publication. Acceptance applies a
  configured +10% harvest modifier only to crops scheduled under a valid
  edition and only for its authored year/season, rather than permanently to
  every greenhouse.

### [219] Doppler Microwave Perimeter Radar — `MicrowaveRadarSystem`

- Add radar sector, frequency, Doppler shift, target speed/classification,
  fog/ash/radon attenuation, false returns, power, searchlight handoff, and
  perimeter alarm state.
- Connect to `PowerGridSystem`, `LocationLayoutSystem`, and the existing
  hazard/combat response adapter. The radar detects contacts; it does not
  decide combat outcomes.
- `MicrowaveRadarPanel` displays velocity, sector, confidence, and tracking.
  Acceptance detects the authored 500 m technical truck under configured
  visibility and creates a searchlight/sentry dispatch event.

### [220] Diamond Tool Lapping — `DiamondLappingSystem`

- Track lap material, grit grade, rotational speed, pressure, thermal load,
  flatness measurement, optical interference fringes, tool geometry, and
  inspection results.
- Integrate with `SilentFoundrySystem`/`ResearchSystem` durable-tool records.
  “Atom-thin” and “zero defect” become named metrology tiers with tolerances,
  rework, and breakage.
- `DiamondLappingPanel` controls grit and test passes. Acceptance produces
  scalpels within the authored edge/flatness tolerance and reduces the clinic
  tool’s tissue-trauma modifier; it does not guarantee perfect surgery.

### [221] Downhole Seismic Logging — `DownholeLoggingSystem`

- Extend Batch 13 geological drilling/survey state with sonde type, cable depth,
  P/S velocities, density interpretation, water table, fault, cavern signal,
  winch wear, and borehole stability.
- Use `LocationLayoutSystem` and `ResearchSystem`; a detected chamber must be
  an authored geological target with an excavation/safety prerequisite.
- `DownholeLoggingPanel` controls depth and logs. Acceptance identifies the
  authored dry cavern 50 m below Sub-Level 3 and creates a safe excavation
  candidate only after stability validation.

### [222] Magic Lantern Story Hour — `MagicLanternSystem`

- Track slide set, lamp fuel, wick/heat, projector condition, narration,
  audience, age suitability, fire safety, attendance, and history entry.
- Use `CaregivingSystem`, `JournalSystem`, `SurvivorNeedsState`, and the shared
  morale effect port. Happiness changes must be bounded and sourced.
- `MagicLanternModal` manages lamp, slides, narration, and audience. Acceptance
  records “Ancient Cities of the World” and applies the authored +20 colony
  happiness event after a safe, attended show.

### [223] Meteor-Burst Scatter Radio — `MeteorBurstSystem`

- Bridge `RadioHostSession`, `ResearchSystem`, and `LocationLayoutSystem` with
  meteor-trail windows, ionization strength, burst frequency, packet buffer,
  relay identity, encryption metadata, jamming, loss, and retransmission.
- A meteor burst is difficult to sustain and can be missed or intercepted;
  do not model it as un-interceptable or permanently jam-proof.
- `MeteorBurstPanel` controls buffer, timing, relay, and receipt. Acceptance
  delivers the authored encrypted treaty to the 1,500 km enclave during a
  valid atmospheric window and records delivery/verification state.

### [224] Memorial Courtyard and Perpetual Flame — `MemorialLedgerSystem` + `PerpetualFlameSystem`

- Add a structured memorial ledger fed from survivor death events, final wishes,
  journal records, and `CenturySeed` lineage. Deduplicate identities and retain
  cause, day, generation, location, and nameplate status.
- Track courtyard construction, granite/tablet capacity, gas supply, valve,
  flame uptime, wind protection, maintenance, wreaths, and inspection. The
  flame is “perpetual” only while its supply and maintenance contract remains
  valid.
- `PerpetualFlamePanel` renders ledger/nameplates, valve state, flame health,
  and wreath placements. Acceptance ignites the monument when all authored
  names are engraved and applies the data-defined `indomitable_spirit` trait
  through the normal colony-effect system; it must not create an uncapped
  permanent morale aura or omit fallen survivors from the ledger.

## 5. Data, host, and UI deliverables

Add validated data for cavern geology, fuel batches, arc-furnace alloys,
medical flap cases, lightning/surge curves, vinegar recipes, checkpoint
tariffs and disease definitions, strawberry crops/pies, minefield encounters,
bronze bearing specs, almanac editions, microwave contacts, metrology tiers,
downhole targets, lantern slides, meteor relays, and memorial traits.

Create these presentation views with typed read models and command results:

`CavernStoragePanel`, `ElectricArcPanel`, `SkinFlapSurgeryModal`,
`SurgeArresterPanel`, `AcetatorPanel`, `CustomsPostPanel`,
`StrawberryTowerPanel`, `ClaymoreMinefieldPanel`, `BronzeCentrifugalPanel`,
`AlmanacPrintModal`, `MicrowaveRadarPanel`, `DiamondLappingPanel`,
`DownholeLoggingPanel`, `MagicLanternModal`, `MeteorBurstPanel`, and
`PerpetualFlamePanel`.

Every panel needs loading/empty/error states, accessible controls, save/load
rehydration, state-change refresh, and no hard-coded inventory, damage,
tariff, disease, crop, morale, or memorial data.

## 6. Verification and exit criteria

Core tests must cover deterministic replay, capture/restore, migration,
malformed-envelope rejection, unit-safe inventories, high-load power,
geological seal loss, surge faults, medical complications, disease/quarantine,
combat/minefield outcomes, sensor attenuation, communication loss, and all 16
acceptance fixtures.

History tests must confirm construction, disaster, combat, cultural, and
memorial events enter `CampaignHistoryLedger` once with stable causal IDs.
Catalog tests must reject unknown machines, locations, diseases, recipes,
effects, signal relays, survivor IDs, and unsafe permanent modifiers.

Add host self-tests for `--batch14-selftest` and `--batch14-ui-selftest`, then
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

Batch 14 is ready when all 16 fixtures pass from clean and migrated saves,
replay identically with the same seed, survive save/load during active
industrial, medical, combat, radio, and memorial operations, and publish
their outcomes to the structured campaign history without UI-only effects.
