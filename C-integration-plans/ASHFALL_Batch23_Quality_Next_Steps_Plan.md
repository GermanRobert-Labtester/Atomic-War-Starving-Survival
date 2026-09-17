# ASHFALL: Batch 23 Integration Plan

**Steps:** 353–368<br>
**Generated:** 2026-08-19<br>
**Status:** Execution-ready, architecture-aligned<br>
**Host:** Godot 4.7+ (.NET 8 C#)<br>
**Core target:** .NET Standard 2.1

## 1. Integration decision

Batch 23 is a content batch, not Expansion 23. Preserve `ExpansionSuite` at
its canonical 12 entries. Implement the batch as deterministic Core systems,
typed host sessions, validated data, and Godot presentation only.

Use the actual current authorities:

- `BrineWaterSystem` plus treaty IDs in `foundry_accords.json` for geothermal
  brine obligations; there is no active `District8Accords` class.
- `SilentFoundrySystem` for production, maintenance, quality, and machine
  work orders.
- `RadiationSystem`, `WeatherSystem`, `GreenhouseSystem`, `ResearchSystem`,
  `RadioHostSession`, `MarketSystem`, `JournalSystem`, `LocationLayoutSystem`,
  `TacticalCombatSystem`, and `NeedsSystem` for their existing domains.
- `MedicalHostSession`, `RespiratoryDegenerationSystem`,
  `CombatTraumaSystem`, and `SurvivorNeedsState` for clinical state; there is
  no general `MedicalSystem` authority to duplicate.
- `GenerationalSuccessionEngine` and `EpilogueMatrixRuntime` through typed
  facades; do not create a parallel `GenerationalSuccessionSystem`.

The roadmap’s absolute claims are converted into configured, measurable
outcomes. Dosimetry beads warn but do not replace dose simulation; aqueducts
have flow and contamination failure; combat obstacles create encounter
advantages rather than guaranteed kills; and permanent-looking morale or
endgame effects require explicit provenance and stacking rules.

## 2. Shared foundation and persistence

### 2.1 Batch save

Add `Batch23State` and `Batch23SaveStore` at
`user://saves/slot-N/batch23_save.json`. Use a checksummed versioned envelope,
`AtomicFileWriter`, explicit migrations, deep-copy capture/restore, and strict
rejection of malformed current envelopes. `SaveLoadHostSession` discovers
`_save.json` files and records them in the slot manifest.

New mutable state belongs in this store by default. If an implementation moves
state into `RadiationSystem`, `WeatherSystem`, `GreenhouseState`,
`SilentFoundryState`, `RadioSaveState`, or the Century Seed save, perform a
deliberate migration and remove the duplicate writer. Panels never own state.

### 2.2 Shared contracts

Verify or extend the shared contracts from prior batches before feature work:

- unit-safe inventory for beads, pigments, fasteners, oxygen, air volume,
  water, crops, wire, light, frequencies, and data packets;
- `PowerGridSystem` loads, storage, transient peaks, faults, and zero-power
  versus powered-device semantics;
- measured observations with units, sample provenance, confidence, calibration,
  uncertainty, and source event;
- medical treatment/effect ports through the actual Medical/Trauma/Needs path;
- greenhouse vitamin, respiratory, soil, and crop effects through `NeedsSystem`;
- location, water route, obstacle, combat, sensor, radio, geophysical, and
  history event references;
- campaign history publication for construction, treatment, discovery,
  research, culture, combat, and final-campaign events.

Extend `CatalogIntegrityValidator` for every new item, recipe, machine,
location, disease/effect, isotope, signal, survey target, and endgame event.
All new JSON under `Assets/StreamingAssets/Data/` uses `snake_case` IDs and
`schema_version`.

## 3. Ordered vertical slices

1. **Foundation:** Batch 23 save, units, observation contracts, power,
   radiation/weather adapters, and `--batch23-selftest` scaffolding.
2. **Industrial and optical:** 353, 354, 361, and 364 through brine, foundry,
   research, and needs/illumination ports.
3. **Medical, food, and culture:** 355, 357, 359, and 366 through medical,
   recipe, greenhouse, journal, and research authorities.
4. **Water and defensive field:** 356, 358, 360, 362, 363, and 365 through
   weather, radiation, location, tactical, geophysics, and power contracts.
5. **Radio and endgame:** 367 and 368 after meteor-radio and Century Seed/
   Epilogue history contracts are stable.

## 4. Step-by-step integration contracts

### [353] Cobalt Dosimetry Beads — `CobaltDosimetrySystem`

- Extend the brine/foundry chemistry boundary with cobalt mineral feed,
  pigment precipitation, glass composition, bead batch, calibration curve,
  absorbed-dose response, color state, and replacement/annealing.
- Connect dose interpretation to `RadiationSystem`; beads are a visual,
  approximate measurement with saturation, fading, and calibration error, not
  a second radiation ledger.
- `CobaltDosimetryGlassPanel` controls kiln, bead fabrication, calibration, and
  assignment. Acceptance fabricates 100 validated beads and equips all dwellers
  with a live color warning for authored chronic-radiation zones.

### [354] Cold-Header Fasteners — `ColdHeaderFastenerSystem`

- Extend `SilentFoundrySystem` with wire feed, cutoff, two-blow forming,
  heading die wear, thread rolling, hardness, quality inspection, lubrication,
  barrel stock, and production faults.
- Register bolts, rivets, and armor fasteners with dimensions, grade, and
  construction compatibility. “Infinite” stock is replaced by throughput and
  material reservations.
- `ColdHeaderFastenerPanel` renders wire feed, dies, threads, and stock.
  Acceptance produces the authored 10,000 hardened bolts and applies the
  configured +30% construction throughput only to eligible work orders while
  stock lasts.

### [355] Hyperbaric Oxygen Therapy — `HbotTreatmentSystem`

- Add chamber capacity, pressure, oxygen purity, patient schedule, exposure
  time, decompression, fire risk, equipment condition, and patient response.
- Use `MedicalHostSession`, `RespiratoryDegenerationSystem`,
  `CombatTraumaSystem`, and `SurvivorNeedsState` through a single treatment
  effect port. HBOT can support wounds or poisoning; it cannot bypass
  diagnosis, necrosis, infection, or contraindications.
- `HbotChamberModal` controls patient, pressure, oxygen, and dive cycle.
  Acceptance resolves the authored gangrenous-wound case with limb salvage
  and restored mobility only when the treatment and recovery checks pass.

### [356] TSP Air Sampler — `TspAerosolSamplerSystem`

- Track sampler flow `1.5 m³/min`, filter identity, loading mass, pump wear,
  weather, ash composition, concentration units, clogging forecast, and filter
  replacement/backwash events.
- Integrate `WeatherSystem` and `RadiationSystem`; total particulate mass is
  distinct from isotope dose and must not overwrite the radiological state.
- `TspAerosolSamplerPanel` shows airflow, filter mass, concentration, and
  toxicity warnings. Acceptance detects the authored extreme ash surge and
  schedules a filter backwash before the configured air-system stall threshold.

### [357] Vintage Tasting Parlor — `VintageTastingSystem`

- Use `RecipeCatalog`, `NeedsSystem`, `CenturySeed` time progression, and
  faction/trade authorities for cellar inventory, age, provenance, tasting
  notes, hosting, audience, cultural prestige, and diplomatic interest.
- A vintage cannot make colony happiness automatically maximum or summon an
  elite faction without an authored invitation/stance/quality path.
- `VintageTastingRoomPanel` manages bottles, tasting, notes, and guest seating.
  Acceptance records the tasting event, applies a bounded cultural morale
  modifier, and creates the configured diplomatic contact opportunity.

### [358] Gravity Siphon Aqueduct — `SiphonAqueductSystem`

- Model survey grade, alpine intake, snowmelt seasonality, weir, 10 km route,
  inverted siphon pressure, masonry loss, pipe integrity, sediment, water
  quality, irrigation canals, and repair access.
- Use `DutyRosterSystem`, `CaregivingSystem`, `LocationLayoutSystem`,
  `BrineWaterSystem`/water inventory, and the location/crop water adapter. Do
  not treat “millions of liters” or pristine water as an unconditional source.
- `SiphonAqueductPanel` controls grade, intake, valves, and irrigation. The
  acceptance fixture delivers 50,000 L/hour and irrigates 100 hectares only
  during the authored snowmelt/quality conditions.

### [359] Mustard Towers and Medicinal Plasters — `MustardTowerSystem`

- Extend `GreenhouseSystem` with vertical tower, growth, seed harvest, water,
  light/power, contamination, glucosinolate concentration, and leaf/seed
  outputs.
- Use `RecipeCatalog` and the medical effect port for mustard plaster; model
  topical warming/irritation and contraindications. Do not label a mustard
  plaster as a universal antibiotic or cure.
- `MustardTowerPanel` controls growth, seed milling, and compounding.
  Acceptance applies the configured congestion-relief effect to 10 valid
  patients and resolves winter bronchitis only if disease and recovery states
  permit it.

### [360] Concealed Low-Wire Aprons — `LowWireApronSystem`

- Use `LocationLayoutSystem`, `TacticalCombatSystem`, and
  `WarlordDoctrineSystem` for stake placement, concealment, brush coverage,
  wire integrity, crawl detection, friendly access, crossfire eligibility, and
  response timing.
- An entanglement creates a movement/detection advantage; sentry fire remains
  a combat resolution with visibility, ammunition, and identification checks.
- `LowWireApronPanel` manages placement and reports. Acceptance resolves the
  authored infiltration squad as entangled and makes sentry neutralization
  eligible when the defensive arc and rules of engagement pass.

### [361] Thread Grinder and Ball Screws — `ThreadGrindingSystem`

- Extend `SilentFoundrySystem`/`ResearchSystem` precision tooling with lead,
  pitch, wheel dressing, thermal drift, profile projection, ball-track finish,
  preload, backlash, and wear.
- Register ball screws with micrometer-scale tolerance as compatible machine
  components; a one-micrometer result is an inspected tier, not guaranteed
  accuracy on every machine.
- `ThreadGrinderPanel` controls setup, dressing, grinding, and inspection.
  Acceptance installs an in-tolerance ball screw and raises the lathe’s
  positioning accuracy to the authored 1 µm tier.

### [362] Meridian Circle Geodesy — `MeridianCircleSystem`

- Track engraved ring, vernier scale, star catalogue, reticle alignment,
  refraction, ash/cloud visibility, observer skill, declination, azimuth, and
  map confidence.
- Use `JournalSystem`, `ExpeditionSystem`, `CraftingSystem`, and the existing
  `SimClock`/astronomy boundary. A survey yields a confidence-bearing map
  observation, not a flawless continent map.
- `MeridianRingModal` renders engraving, transit, calculations, and survey
  overlay. Acceptance creates the authored geodetic map segment and reveals
  validated hidden features after observation/error checks pass.

### [363] Cave-Scorpion Disruptors — `ScorpionDisruptorSystem`

- Add foundation crevices, resonator frequency `25–55 kHz`, power, bedrock
  coverage, attenuation, structural resonance, animal response, habituation,
  and sting/encounter events.
- Connect to `PowerGridSystem`, `LocationLayoutSystem`, and the host hazard
  adapter. “Zero casualties” is an acceptance result for the authored event,
  not a universal safety rating.
- `ScorpionDisruptorPanel` controls coverage and frequency. Acceptance clears
  the authored Sub-Level 4 scorpion encounter while retaining maintenance,
  power, and alternate-threat state.

### [364] TIR Daylight Pipes — `TirLightPipeSystem`

- Model heliostat tracking, surface weather, fused-silica prism quality,
  conduit alignment, total-internal-reflection loss, fire/heat exposure,
  depth, dormitory lumens, and maintenance.
- Use `SilentFoundrySystem`/`ResearchSystem` for optical components and
  `NeedsSystem` for light/mental-health effects. “Zero power” means no
  electrical generation load, not no maintenance or weather dependency.
- `TirLightPipePanel` shows heliostat, prism, conduit, and lumens. Acceptance
  delivers the authored 500 lumens to Sub-Level 3 and removes the configured
  depression debuff only while daylight and alignment remain valid.

### [365] Spectral Induced Polarization — `SipGeophysicsSystem`

- Add multi-frequency excitation `0.001 Hz–1 kHz`, current, phase/amplitude,
  electrode geometry, Cole-Cole parameters, contact resistance, noise,
  inversion confidence, and mineral classification.
- Use `LocationLayoutSystem` and `ResearchSystem`; copper sulfide, pyrite, and
  graphite signatures are catalogued observations and require follow-up assay.
- `SipGeophysicsPanel` renders frequency curves, relaxation parameters,
  anomalies, and verified targets. Acceptance confirms the authored
  chalcopyrite body and rejects the barren pyrite zone at the configured
  confidence threshold.

### [366] Laser Flow Cytometry — `FlowCytometerSystem`

- Track sheath fluid, microfluidic flow, laser `488 nm`, scatter channels,
  fluorescence, droplet sorting, calibration beads, sample provenance,
  lymphocyte damage markers, and result confidence.
- Use `ResearchSystem`, `MedicalHostSession`, `DutyRosterSystem`, and the
  medical evidence port. A cellular finding must create a diagnosis/monitoring
  result; it cannot prescribe unvalidated cytokine therapy directly.
- `FlowCytometerModal` controls alignment, sample, plots, and sort. Acceptance
  detects the authored early marrow-damage profile and creates an immediate
  preventive cytokine-treatment option through validated medical data.

### [367] 256-Carrier OFDM Meteor Radio — `OfdmMeteorSystem`

- Extend the meteor-burst radio contract with 256 carriers, cyclic prefix,
  channel estimate, multipath delay, coding, packet buffer, voice frames,
  ionization window, relay identity, and loss/retransmission.
- Use `RadioHostSession`, `ResearchSystem`, and `LocationLayoutSystem`. Voice
  is a packetized content result with latency/loss, not a guaranteed live
  phone call.
- `OfdmMeteorPanel` renders subcarriers, guard interval, voice buffer, and
  telemetry. Acceptance delivers an intelligible 10-second message across the
  authored 1,800 km route during a valid meteor window and records receipt.

### [368] Sanctuary Grand Organ — `SanctuaryOrganSystem`

- Add organ construction, pipe rank inventory, lead-tin alloy quality, manuals,
  drawstops, windchest pressure, blower load, tuning, repertoire, performer,
  audience, and acoustic event state.
- Integrate `GenerationalSuccessionEngine`, `SoundManager`,
  `EpilogueMatrixRuntime`, `JournalSystem`, and the campaign history ledger
  through typed adapters. The organ does not end a campaign by editing a UI
  boolean.
- `SanctuaryPipeOrganPanel` renders the 4-manual console, rank controls,
  windchest, and performance. Acceptance requires a completed 3,000-pipe
  instrument, valid wind/tuning state, and the authored grand-finale event to
  close the 100-year campaign through the epilogue authority.

## 5. Data, host, and UI deliverables

Add validated data for cobalt pigments/bead curves, fastener grades,
hyperbaric treatment cases, ash/TSP profiles, vintages and faction contacts,
aqueduct routes and water quality, mustard/plaster effects, defensive sectors,
thread-grinding tolerances, star observations, cave species, optical pipes,
SIP mineral signatures, cytometry markers, OFDM meteor routes, organ ranks,
repertoire, and endgame events. Existing narrative files are flavor/evidence
only unless promoted through catalog validation.

Create these presentation views with typed read models and command results:

`CobaltDosimetryGlassPanel`, `ColdHeaderFastenerPanel`, `HbotChamberModal`,
`TspAerosolSamplerPanel`, `VintageTastingRoomPanel`, `SiphonAqueductPanel`,
`MustardTowerPanel`, `LowWireApronPanel`, `ThreadGrinderPanel`,
`MeridianRingModal`, `ScorpionDisruptorPanel`, `TirLightPipePanel`,
`SipGeophysicsPanel`, `FlowCytometerModal`, `OfdmMeteorPanel`, and
`SanctuaryPipeOrganPanel`.

Every panel needs loading/empty/error states, accessible controls, save/load
rehydration, deterministic state-change refresh, and no hard-coded inventory,
health, radiation, combat, signal, history, or endgame state.

## 6. Verification and exit criteria

Core tests must cover deterministic replay, capture/restore, migration,
malformed-envelope rejection, unit conversions, calibration uncertainty,
dosimetry saturation, fastener quality, hyperbaric contraindications, TSP
clogging, water contamination, crop/medical effects, combat entanglement,
optical losses, geophysical inversion noise, cytometry provenance, meteor
packet loss, organ tuning, and all 16 acceptance fixtures.

History tests must confirm scientific observations, construction, treatments,
combat, communications, cultural events, and the final organ performance are
published once with stable source/actor IDs. Catalog tests must reject unknown
isotopes, mineral signatures, medical therapies, recipes, locations, radio
relays, organ ranks, and unsafe permanent modifiers.

Add host self-tests for `--batch23-selftest` and `--batch23-ui-selftest`, then
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

Batch 23 is ready when all 16 fixtures pass from clean and migrated saves,
replay identically with the same seed, survive active sampling/treatment/
production/radio/endgame save boundaries, and every result is traceable to a
validated Core authority.
