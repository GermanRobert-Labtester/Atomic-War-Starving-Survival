# ASHFALL: Batch 17 Integration Plan

**Steps:** 257–272<br>
**Generated:** 2026-08-19<br>
**Status:** Execution-ready, architecture-aligned<br>
**Host:** Godot 4.7+ (.NET 8 C#)<br>
**Core target:** .NET Standard 2.1

## 1. Integration decision

Batch 17 is a content batch, not Expansion 17. Keep `ExpansionSuite` at its
canonical 12 entries. Add deterministic Core systems, typed host sessions, and
Godot-only presentation while preserving one authority for each resource,
survivor, combat, radio, and historical state.

The roadmap names `District8Accords`, `MedicalSystem`, `PowerGridPanel`, and
`ShelterHazardLoop`. They are not current Core authorities. Use
`BrineWaterSystem` plus the treaty IDs in `foundry_accords.json`, the existing
`MedicalHostSession`/`RespiratoryDegenerationSystem`/`CombatTraumaSystem`/
`NeedsSystem` boundary, Batch 10’s shared `PowerGridSystem`, and a host hazard
adapter. Do not create a second economy, medical, or power writer.

The current Century Seed authority is the Core
`GenerationalSuccessionEngine` (despite its existing legacy folder name). Batch
17 must use a typed facade/migration around that authority rather than add a
parallel `GenerationalSuccessionSystem`.

Absolute roadmap claims become configured, measurable outcomes. Soap does not
guarantee zero infections, acoustic defense does not guarantee zero breaches,
and a permanent skill aura is represented as a sourced, versioned modifier
with explicit stacking and maintenance policy.

## 2. Shared foundation and persistence

### 2.1 Batch save

Add `Batch17State` and `Batch17SaveStore` at
`user://saves/slot-N/batch17_save.json`. Use a checksummed versioned envelope,
`AtomicFileWriter`, explicit migrations, deep-copy capture/restore, and strict
rejection of malformed current envelopes. `SaveLoadHostSession` already
discovers `_save.json` stores and records their SHA-256 in the slot manifest.

New mutable Batch 17 state belongs in this store by default. If a feature is
folded into `SilentFoundryState`, `GreenhouseState`, radio state, or the
existing Century Seed save, migrate that store and remove the duplicate batch
writer. Panels never serialize their own state.

### 2.2 Shared contracts

Before feature work, verify the shared contracts from earlier batches:

- unit-safe inventory for `kg`, `L`, `m`, `m²`, `m³`, `kW`, `kWh`, pressure,
  frequency, dose, and skill XP;
- power loads, storage, transient peaks, faults, and priority through
  `PowerGridSystem`;
- machine work orders with operator skill, maintenance, contamination,
  quality, by-products, and deterministic failure reasons;
- survivor treatment/effect ports through `NeedsSystem`, medical/trauma, and
  the active respiratory authority;
- greenhouse nutrient, vitamin, deficiency, and soil-fertility effects;
- location/line-of-sight, obstacle, combat, faction, radio-signal, and
  geological-survey references;
- `CampaignHistoryLedger` from Batch 13 for construction, treatment, combat,
  training, research, and monument records.

Extend `CatalogIntegrityValidator` for all new IDs, references, ranges,
permanent-modifier rules, and `schema_version` values. New data belongs under
`Assets/StreamingAssets/Data/` with `snake_case` IDs.

## 3. Ordered vertical slices

1. **Foundation:** Batch 17 save, units, power/material contracts, medical
   effect ports, history events, and `--batch17-selftest` scaffolding.
2. **Industrial chemistry and steel:** 257, 258, 265, and 268 through the
   brine/foundry/research work-order boundary.
3. **Clinical, food, and sanitation:** 259, 261, 263, and 270 through medical,
   recipe, greenhouse, needs, and duty contracts.
4. **Perimeter and field sensing:** 264, 267, 269, and 271 through location,
   tactical, power, weather, and radio adapters.
5. **Navigation and training:** 262 and 266 with duty, expedition, research,
   and skill progression.
6. **Founders monument:** 272 after the structured history and Century Seed
   facade are authoritative.

## 4. Step-by-step integration contracts

### [257] Potash Soft Soap — `PotashSoapSystem`

- Extend `BrineWaterSystem`/foundry chemistry with potassium-rich brine feed,
  electrolysis, KOH concentration, tallow rendering, saponification,
  contamination, caustic handling, batch quality, and storage.
- Use treaty ID `treaty_brine_pipe_and_iodine_exchange` for any District 8
  obligation; do not create a `District8Accords` duplicate. Register liquid
  soap as a validated hygiene/medical-scrub item with safe handling tags.
- `PotashSoapPanel` controls tanks, kettle, stir, and carboy output. Acceptance
  produces the authored 50-liter batch and raises the colony hygiene score to
  its configured cap only while supply, laundry, and clinical usage remain
  valid; it does not make skin infections impossible.

### [258] Continuous Billet Caster and Rolling Mill — `RollingMillSystem`

- Extend `SilentFoundrySystem` with tundish, mold cooling, billet continuity,
  roll-stand reductions, motor load, water flow, flying shear, scale loss,
  straightness, and product quality.
- Register rebar, angle, I-beam, and structural stock with measured length,
  cross-section, grade, and batch provenance. A five-ton heat to 200 m of rebar
  is an authored recipe fixture, not a universal mass conversion.
- `RollingMillPanel` renders temperature, water, pass schedule, shears, and
  stock. Acceptance creates valid rebar accepted by the reinforced-concrete
  blast-vault construction recipe.

### [259] Thoracic Surgery and Lobectomy — `ThoracicSurgerySystem`

- Build on the actual medical/trauma boundary with hemothorax, pneumothorax,
  empyema, lung injury, airway/oxygenation, blood loss, anesthesia, thoracotomy,
  lobe resection, chest tube drainage, infection, and recovery state.
- Route health changes through `NeedsSystem`/medical effect ports and the
  existing respiratory system. Do not create a parallel `MedicalSystem` or
  set a survivor to “cured” from a modal.
- `ThoracicSurgeryModal` submits triage, resection, and drainage commands.
  Acceptance resolves the authored tension-hemothorax case when equipment,
  time, surgeon skill, and blood-loss conditions pass; complications remain
  deterministic and saveable.

### [260] Cascade Aerosol Impactors — `CascadeImpactorSystem`

- Add sampler stages, nozzle flow, particle-size bins `0.5–10 µm`, slide
  loading, weather exposure, dose/activity, alpha spectrum, isotope
  classification, and sample contamination.
- Integrate `WeatherSystem` with the existing radiation/dosimetry authority.
  Isotope IDs such as plutonium, cesium, and strontium must be catalogued; a
  UI graph cannot invent a radiation classification.
- `CascadeImpactorPanel` shows stage flow, slides, counts, and hazard. Acceptance
  detects the authored inhalable high-activity profile and publishes a warning
  that applies the configured dual-cartridge P100 respirator requirement to
  outdoor work through the needs/hazard system.

### [261] Continuous Acetator Automation — `ContinuousAcetatorSystem`

- Extend the Batch 14 acetator boundary with solenoid valves, thermostat,
  heating load, culture health, continuous outflow, storage, cleaning, and
  manual-override/fault state.
- Use `RecipeCatalog`, `GoodsCatalog`, and the shared power contract. Automated
  production still consumes electricity, culture, feedstock, capacity, and
  maintenance; it is not free labor or free energy.
- `ContinuousAcetatorPanel` controls set points and displays daily acid
  totals. Acceptance produces the configured 20 liters of 5% vinegar per day
  without worker assignment while the machine remains powered and maintained.

### [262] Prisoner Vocational Guild — `VocationalApprenticeshipSystem`

- Extend Batch 10 rehabilitation/citizenship state with guild, mentor,
  apprentice skill track, attendance, safety, milestone project, assessment,
  parole compliance, and graduation/citizenship review.
- Use `DutyRosterSystem`, `CaregivingSystem`, `SurvivorNeedsState`, and the
  shared skill ledger. A certificate is an earned credential with provenance,
  not a direct skill overwrite.
- `GuildApprenticePanel` assigns mentors, projects, and reviews. Acceptance
  graduates the authored blacksmithing parole prisoner into the configured
  master-smith tier and completes citizenship only when legal and conduct
  requirements pass.

### [263] Sweet Potato Towers — `SweetPotatoTowerSystem`

- Extend `GreenhouseSystem` with aeroponic tower geometry, misting, water and
  nutrient load, light/power, vine harvest, tuber growth, blight, contamination,
  and dual-output inventory.
- Apply vitamin A, calories, and deficiency relief through `NeedsSystem`; do
  not erase all deficiencies from a single harvest. Yield and quality depend on
  water, power, growth, and contamination.
- `SweetPotatoTowerPanel` manages misting and harvest. Acceptance creates the
  authored 40 kg tuber and 15 kg greens fixture and removes only the
  deficiencies covered by the resulting dietary ledger entry.

### [264] Czech Hedgehogs — `CzechHedgehogSystem`

- Use `LocationLayoutSystem`, `TacticalCombatSystem`, and
  `WarlordDoctrineSystem` for I-beam material, cutting/welding, barrier
  topology, anchoring, wire, vehicle mass, ramming, bypass, and dismount
  outcomes.
- Obstacles create a tactical movement/vehicle modifier; they do not
  guarantee that every convoy stops or prevent flanking, removal, or artillery.
- `CzechHedgehogPanel` controls fabrication and road placement. Acceptance
  resolves the authored ten-unit highway deployment as a convoy halt/dismount
  event when the route, anchoring, and vehicle conditions pass.

### [265] Vertical Gear Shaper — `GearShaperSystem`

- Extend `SilentFoundrySystem`/`ResearchSystem` precision tooling with ram
  stroke, indexing, cutter wear, internal spline geometry, keyway tolerance,
  lubrication, operator skill, and inspection.
- Register spline couplers as compatible durable components. The mining-hoist
  repair path must validate tooth count, material, torque rating, and fit.
- `GearShaperPanel` controls stroke and indexing. Acceptance produces and
  installs an in-tolerance coupler, returning the existing Sub-Level 4 hoist
  to service through its normal machine-condition state.

### [266] Solar Transit Chronometer — `SolarTransitSystem`

- Track engraved instrument quality, meridian wire alignment, solar noon,
  observer latitude/longitude calculation, cloud/ash visibility, time sync,
  and map-coordinate confidence.
- Use `JournalSystem`, `ExpeditionSystem`, `CraftingSystem`, and the existing
  `SimClock` boundary. A navigation result must be a confidence-bearing map
  observation, not “flawless” precision.
- `SolarTransitModal` handles alignment and calculation. Acceptance reveals
  the authored hidden mountain-pass coordinate and adds the shortcut only when
  clear-sky, calibration, and observation conditions pass.

### [267] Acoustic Pest Denial — `AcousticPestControlSystem`

- Add vent sectors, emitter frequency `20–100 kHz`, sweep pattern, power load,
  baffles, structural coupling, animal response, habituation, false alarms,
  and maintenance.
- Connect to `PowerGridSystem` and the host hazard/duct adapter. “Zero duct
  breach” becomes a monitored risk threshold with fatigue, coverage gaps, and
  alternative infestation paths.
- `BatAcousticJammerPanel` controls frequency/coverage and reports. Acceptance
  repels the authored bat swarm from the air-conditioning shafts without
  granting universal pest immunity.

### [268] Flint Prism Spectrometer — `PrismSpectrometerSystem`

- Track flint-glass prism quality, 60-degree angle, slit width, collimation,
  calibration, light source, emission lines, sample preparation, and isotope/
  element confidence.
- Integrate with `SilentFoundrySystem` and `ResearchSystem`. An identified
  titanium sample unlocks the authored aerospace recipe only after spectral
  confidence and catalog research prerequisites pass.
- `PrismSpectrometerPanel` shows slit, prism rotation, spectrum, and assay.
  Acceptance identifies the authored titanium-rich scrap and creates a valid
  research unlock rather than directly changing all alloy recipes.

### [269] Cross-Hole Seismic Tomography — `CrossHoleTomographySystem`

- Extend geological survey state with borehole pair geometry, sparker source,
  geophone arrays, pulse timing, velocity inversion, fracture zones, void
  confidence, and excavation stability.
- Use `LocationLayoutSystem` and `ResearchSystem`; the solid-bedrock result
  must become a validated excavation zone with a safety margin, not a direct
  room creation.
- `CrossHoleTomographyPanel` controls pulses and inversion. Acceptance maps
  the authored generator-cavern path and clears it for excavation only after
  geology and construction checks pass.

### [270] Cryotome Histology — `HistologySystem`

- Add biopsy provenance, freezing, section thickness `1–10 µm`, stain,
  microscope quality, slide interpretation, pathogen/malignancy confidence,
  and treatment recommendation.
- Use `MedicalHostSession`, `DutyRosterSystem`, `JournalSystem`,
  `DiseaseSystem`, and the shared medical evidence port. A pathology result
  must not prescribe an antibiotic that is absent or invalid for the catalogued
  organism.
- `CryotomeHistologyModal` controls sectioning, staining, and review.
  Acceptance rules out malignancy in the authored biopsy, identifies the
  configured lung infection, and creates a targeted treatment plan through the
  disease/medical authority.

### [271] Whistler VLF Space Weather — `WhistlerVlfSystem`

- Add loop antenna, VLF band, lightning-whistler capture, dispersion curve,
  plasma-density estimate, radiation-belt index, solar-storm forecast,
  confidence, and radio-blackout warning.
- Integrate `RadioHostSession`, `ResearchSystem`, and `WeatherSystem` with
  deterministic storm events. A 12-hour warning requires a detectable
  whistler signature and valid model confidence.
- `WhistlerVlfPanel` displays spectrogram, density, radiation, and warning.
  Acceptance creates the authored geomagnetic-storm alert 12 hours before the
  configured blackout and feeds it into the radio/weather event timeline.

### [272] Hall of Founders — `FoundersHallSystem`

- Use the existing Century Seed authority through a typed facade and the
  `CampaignHistoryLedger`/memorial ledger from earlier batches. Validate five
  founder identities, death/leadership provenance, material, sculpting,
  inscription, rotunda construction, and dedication state.
- `FoundersHallPanel` renders a 2D/2.5D diorama, founder selection, quote
  validation, and dedication status. The panel never invents portraits or
  quotes.
- Apply `legacy_of_the_pioneers` as a data-defined future-generation skill
  modifier with source, stacking, and save version. Acceptance requires the
  original five founders’ validated statues and dedication, then grants the
  configured +15% future-generation skill effect without uncapped aura stacking.

## 5. Data, host, and UI deliverables

Add validated data for brine chemistry, soap/hygiene effects, rolling stock,
medical thoracic cases, aerosol isotope profiles, acetator automation,
vocational guilds, greenhouse tower crops, vehicle obstacles, machine tooling,
astronomical observations, pest species, spectral lines, borehole pairs,
histology diagnoses, space-weather fixtures, founder identities, and legacy
modifiers. Narrative files can provide flavor, but runtime catalogs remain the
authority.

Create these presentation views with typed read models and command results:

`PotashSoapPanel`, `RollingMillPanel`, `ThoracicSurgeryModal`,
`CascadeImpactorPanel`, `ContinuousAcetatorPanel`, `GuildApprenticePanel`,
`SweetPotatoTowerPanel`, `CzechHedgehogPanel`, `GearShaperPanel`,
`SolarTransitModal`, `BatAcousticJammerPanel`, `PrismSpectrometerPanel`,
`CrossHoleTomographyPanel`, `CryotomeHistologyModal`, `WhistlerVlfPanel`, and
`FoundersHallPanel`.

Every panel needs loading/empty/error states, accessible controls, save/load
rehydration, deterministic refresh from state-change events, and no hard-coded
inventory, health, hygiene, combat, research, or monument state.

## 6. Verification and exit criteria

Core tests must cover deterministic replay, capture/restore, migration,
malformed-envelope rejection, unit-safe chemistry, power faults, machine
quality, medical complications, greenhouse deficiency effects, obstacle and
combat outcomes, sensor noise, radio forecast confidence, and all 16
acceptance fixtures.

History tests must confirm treatment, training, construction, combat, research,
founder, and dedication events enter `CampaignHistoryLedger` exactly once with
stable actor/source IDs. Catalog tests must reject unknown materials, diseases,
recipes, machines, founder IDs, effects, and unsafe permanent modifiers.

Add host self-tests for `--batch17-selftest` and `--batch17-ui-selftest`, then
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

Batch 17 is ready when all 16 fixtures pass from clean and migrated saves,
replay identically with the same seed, survive save/load at active industrial,
medical, field, and historical boundaries, and every claimed effect is sourced
from a Core authority rather than a Godot panel.
