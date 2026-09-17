# ASHFALL: Batch 11 Integration Plan

**Steps:** 161–176<br>
**Generated:** 2026-08-19<br>
**Status:** Execution-ready, architecture-aligned<br>
**Host:** Godot 4.7+ (.NET 8 C#)<br>
**Core target:** .NET Standard 2.1

## 1. Integration decision

Batch 11 is another content batch, not Expansion 13 or 14. Preserve the
canonical 12-entry `ExpansionSuite`. The batch adds deterministic Core
systems, typed host sessions, and Godot presentation around existing
authorities. It must not introduce a legacy engine, generated editor project,
Godot references in Core, or simulation code in UI nodes.

Important existing anchors:

- `BrineWaterSystem` already owns geothermal/brine processing and salt trade;
  existing foundry-accord data contains iodine exchange references.
- `SilentFoundrySystem` owns foundry production, maintenance, products, and
  treaties; it is the integration point for coke, cranes, and bearings.
- `GreenhouseSystem`, `RecipeCatalog`, `GoodsCatalog`, and `NeedsSystem` are
  the food/nutrition boundaries.
- `WeatherSystem`, `RadioHostSession`, `StealthDiveInstance`,
  `LocationLayoutSystem`, `ResearchSystem`, `JournalSystem`,
  `CrossingArbitrationSystem`, `TacticalCombatSystem`, and
  `WarlordDoctrineSystem` already exist and must be reused.
- `CatalogIntegrityValidator` is the authority for data references and
  ranges. `Main.cs` is a host/wiring boundary, not a scenario simulation
  engine.
- There is no complete power-grid authority, VLF runtime, scenario editor,
  challenge-style mod loader, or general skin-graft/smokehouse runtime yet.
  Batch 10’s shared `PowerGridSystem` and nutrition ledger are prerequisites
  for the dependent Batch 11 features.

Absolute roadmap language is converted into measurable, authored outcomes:
feathering reduces damage rather than guaranteeing peak generation, a
smokehouse produces long-life food rather than literally never-spoiling food,
and defensive systems create threat-resolution advantages rather than
“complete” or “impenetrable” security.

## 2. Shared foundation and save boundary

### 2.1 Batch 11 state

Add a checksummed, versioned Core `Batch11State` and
`Batch11SaveStore` at `user://saves/slot-N/batch11_save.json`. Its sub-states
cover active Batch 11 jobs, treatment cases, industrial batches, field
deployments, communications, and scenario-editor metadata. Use
`SaveChecksum`, `AtomicFileWriter`, explicit migrations, and malformed-current
envelope rejection. `SaveLoadHostSession` will include the store in the slot
manifest through its existing `*_save.json` convention.

Do not duplicate state already owned by the Expansion Hub. New features use
typed adapters to existing systems; if a future implementation moves a
sub-state into `SilentFoundryState`, `GreenhouseState`, or `RadioSaveState`,
perform a deliberate schema migration rather than keeping two writers.

Installed user scenarios are not campaign state. Store packages under
`user://mods/` and keep the enabled-mod registry separate from campaign saves.

### 2.2 Required shared contracts

Batch 11 depends on these contracts, implemented once and reused:

- measured units and conversion rules for mass, temperature, pressure,
  voltage/current, energy, distance, depth, and signal frequency;
- inventory reservation/by-product accounting;
- operator skill, maintenance, contamination, and deterministic failure;
- bounded nutrition, immunity, morale, work, and navigation modifiers with
  source, duration, and stacking policy;
- location/line-of-sight, faction stance/treaty, threat, radio signal, and
  geological sample references;
- power loads through Batch 10’s `PowerGridSystem`, including outage events.

Extend `CatalogIntegrityValidator` for all IDs, references, schema versions,
unit/range checks, and uniqueness constraints before feature UI is built.

## 3. Dependency order

1. **Foundation:** Batch 10 save/power/nutrition contracts, data schemas,
   deterministic failure/result types, and `--batch11-selftest` scaffolding.
2. **Industrial chemistry:** 161 iodine ponds, 162 coke ovens, 169 gantry
   crane, and 172 Babbitt bearings, all using foundry work orders.
3. **Food and medicine:** 163 skin grafts, 165 smokehouse, and 167 spirulina,
   using shared needs and power contracts.
4. **Weather and defense:** 164 anemometer, 168 sniper hide, and 171 electric
   fence, with weather, combat, and hazard outcomes.
5. **Diplomacy and culture:** 166 summit feast and 170 woodblock printing,
   with faction stance, journal, and bounded morale effects.
6. **Survey and communications:** 173 core drilling, 174 stereoscope, and 175
   VLF beacon, with location, research, radio, and maritime adapters.
7. **Community authoring:** 176 scenario editor and mod packaging, after the
   catalog and narrative contracts are stable.

Steps 164, 167, and 171 cannot be considered production-ready until the
Batch 10 power authority exists. Step 176 cannot ship until package validation
and sandbox rules pass security tests.

## 4. Step-by-step integration contracts

### [161] Geothermal Iodine Ponds — `IodineEvaporationSystem`

- Extend the `BrineWaterSystem` boundary with pond terraces, sluice flow,
  temperature, salinity, evaporation, iodine separation, condenser trays,
  contamination, and batch yields.
- Use `SilentFoundrySystem`/catalog recipes for salt, iodine reagent, and any
  potassium-iodide medicine. Treaty exchange remains a faction/economy
  outcome, not a second brine inventory.
- `IodinePondsPanel` shows flows, evaporation, condenser status, and medical
  yields. Acceptance uses authored inputs to produce 100 kg salt and five
  reagent bottles; missing heat, brine, or condenser capacity must fail
  transparently.

### [162] Refractory Coke Ovens — `CokeOvenSystem`

- Add foundry coke-oven state for coal reservation, oxygen exclusion,
  carbonization temperature, oven-battery latches, tar/ammonia capture, gas
  loss, fire, and product quality.
- Register metallurgical coke and chemical by-products in foundry data. The
  1,000°C, 1-ton, 700 kg, and 50-liter values are configuration fixtures, not
  hard-coded universal conversion constants.
- `CokeOvenPanel` submits a work order and renders pyrometer, condenser, and
  yield state. Acceptance requires coke quality to unlock the authored steel
  recipes through `SilentFoundrySystem`.

### [163] Skin Grafting — `SkinGraftSystem`

- Add deterministic treatment cases for burn/radiation skin injury, donor
  eligibility, dermatome harvest, mesh expansion, wound-bed preparation,
  dressing changes, vascularization, rejection, infection, scarring, and
  recovery.
- Connect through the existing medical/trauma/needs effect boundary; do not
  create a second survivor-health writer or directly set crafting speed.
- `SkinGraftModal` exposes case inspection and procedure commands. A
  successful graft removes the configured dexterity/work penalty; “100% speed”
  is achieved only when the authored recovery state says so.

### [164] Weather Mast Anemometer — `WindTelemetrySystem`

- Extend `WeatherSystem` with deterministic wind vector, cup tachometer,
  direction vane, gusts, telemetry faults, and turbine controller state.
- Connect feathering/braking to Batch 10’s `PowerGridSystem`. Feathering must
  reduce rotor damage and may reduce output according to the authored power
  curve; it cannot both guarantee peak power and remove storm risk.
- `AnemometerPanel` renders wind, direction, thresholds, brake state, and kW.
  Acceptance covers an over-80-km/h storm with prevented rotor damage and the
  configured safe output.

### [165] Cold Smokehouse — `SmokehouseSystem`

- Track meat batch, cure salt/nitrite inputs, wood species, smoke temperature,
  humidity, density, exposure time, contamination, quality, and shelf-life.
- Use `RecipeCatalog`, `NeedsSystem`, and `GoodsCatalog`; output must be a
  distinct preserved ration with an authored expiration/stability rule.
- `SmokehousePanel` handles wood, hooks, smoke, and stock. Acceptance turns
  50 valid fresh rations into 50 preserved rations that remain stable for the
  configured multi-year pantry interval, subject to storage conditions.

### [166] Diplomatic Summit Feast — `DiplomaticFeastSystem`

- Use `CrossingArbitrationSystem`, `MusterHostSession`, faction IDs, treaty
  records, and `RecipeCatalog` to model invitations, menu quality, seating,
  service, ambassador stance, toast choices, and negotiation outcomes.
- Align the Foundry Guild identity with existing faction/catalog data; do not
  invent a parallel “Grandmaster” authority in the UI.
- `DiplomaticFeastModal` prepares a menu and submits negotiation choices. A
  five-star feast creates the authored negotiation opportunity; the permanent
  mutual-defense pact still requires faction stance, treaty terms, and a
  successful arbitration result.

### [167] Spirulina Bioreactors — `SpirulinaSystem`

- Add reactor tubes, culture density, light/power, CO2, nutrients, temperature,
  contamination, centrifuge harvest, and powder inventory.
- Connect nutrition to `GreenhouseSystem` and `NeedsSystem`, and power to the
  shared grid. The +30 immune-resistance result is a bounded, data-authored
  nutrition modifier, not a direct immunity override.
- `SpirulinaPanel` manages bubbling, CO2, and harvest. Acceptance requires a
  valid harvest to add supplement powder and apply the configured immune/
  nutrition effect with source and expiry.

### [168] Ridge-Line Sniper Hide — `SniperHideSystem`

- Use `LocationLayoutSystem`, `TacticalCombatSystem`, and
  `WarlordDoctrineSystem` for hide placement, concealment, marksman assignment,
  optics, wind, visibility, ammunition, detection, and ambush resolution.
- A scout-party elimination is a deterministic combat outcome based on the
  authored encounter, skill, weapon, and conditions; the hide cannot bypass
  combat or directly delete a faction patrol.
- `SniperHidePanel` renders elevation, windage, camouflage, and engagement
  eligibility. Acceptance stores the resulting prevention/intelligence event
  when the configured ambush succeeds.

### [169] Foundry Gantry Crane — `GantryCraneSystem`

- Extend foundry work orders with bridge/trolley/hoist positions, rated load,
  cable wear, operator authorization, ladle temperature/content, power,
  interlocks, spills, and fault recovery.
- Integrate with `SilentFoundrySystem` material quantities and safety events.
  The crane reduces handling injury risk; it does not claim zero injury risk.
- `GantryCranePanel` controls axes, hook, and ladle tilt. Acceptance requires
  the configured 5-ton pour to pass load, operator, temperature, and safety
  checks, then produce a valid ingot.

### [170] Woodblock Printing — `WoodblockPrintSystem`

- Track block design, carving progress, ink, paper, press, print run,
  placement, wear, and authored poster records.
- Use `JournalSystem` for historical scripts/artifacts and `NeedsSystem` or
  `CaregivingSystem` for bounded morale/safety modifiers. Replace the claimed
  permanent +10 aura with a sourced effect that decays or requires upkeep.
- `WoodblockPrintModal` supports templates, brayer, press, placement, and
  preview. Acceptance applies the configured morale/safety effect after the
  poster is printed and placed in a valid residential/work area.

### [171] Electrified Perimeter — `ElectricFenceSystem`

- Add sector topology, pulse voltage/current, transformer, capacitor charge,
  ground/insulation, faults, maintenance, power load, and threat contact
  resolution.
- Connect to `PowerGridSystem` and the existing hazard/combat boundary. Use
  data-authored deterrence and injury consequences; do not guarantee an
  impenetrable or universally non-lethal barrier.
- `ElectricFencePanel` shows sector faults, pulse state, charge, and load.
  Acceptance repels the authored mutant-wolf event without ammunition when
  power, grounding, and maintenance are valid.

### [172] Babbitt Bearings — `BabbittBearingSystem`

- Extend foundry/research with alloy composition, melt temperature, shell
  preparation, tinning, mold/pour defects, clearance, break-in, wear, and
  machine compatibility.
- Apply friction and fuel effects through a machine-condition adapter; never
  edit generator fuel consumption from the panel. A 15% reduction is a
  configured result for a correctly fitted, calibrated bearing.
- `BabbittBearingPanel` drives crucible, shell, mold, and inspection states.
  Acceptance installs a valid bearing on the diesel generator and reduces its
  condition-based fuel modifier while preserving maintenance history.

### [173] Deep Core Drilling — `CoreDrillingSystem`

- Use `LocationLayoutSystem`, `ResearchSystem`, `GeologicalStrataCatalog`, and
  hydrogeology data for borehole location, depth, drill wear, diamond bits,
  water/power, core recovery, sample trays, and assay results.
- A 1,000 m result requires an authored drill site, adequate tooling, and
  deterministic progress; discoveries add catalogued veins and mining
  locations rather than promising unlimited wealth.
- `CoreSamplePanel` controls drilling and assay. Acceptance discovers the
  authored native-copper vein and creates the corresponding validated shaft
  location.

### [174] Aerial Stereoscope — `StereoscopeSystem`

- Track paired photo IDs, film condition, lens alignment, focus, relief
  confidence, clues, and revealed map coordinates.
- Integrate with `LocationLayoutSystem`, `ResearchSystem`, and the existing
  cartography/location catalogs. “Vault 17 Depot” must be a data-authored
  location with validated prerequisites, not a UI-only marker.
- `StereoscopeModal` handles alignment and inspection. Acceptance reveals the
  authored depot when the pair is aligned within tolerance and the evidence is
  readable.

### [175] VLF Submarine Radio — `VlfBeaconSystem`

- Bridge `RadioHostSession` with `StealthDiveInstance`/maritime state. Track
  frequency, antenna length, transmit power, grounding, sea-water attenuation,
  transponder identity, signal quality, and recall expiry.
- Make 20 kHz and 100 m data-authored operating fixtures. Power, antenna,
  water conditions, and receiver state determine whether the recall arrives.
- `VlfBeaconPanel` controls tuning, antenna, monitoring, and recall. Acceptance
  delivers the authored warning to a submerged diver at 100 m and records the
  event in the radio/maritime history.

### [176] Scenario Editor and Mod Packaging — `ScenarioEditorSystem`

- Implement Core validation and preview services around
  `CatalogIntegrityValidator` and `NarrativeEncounterSystem`; `Main.cs` only
  registers the resulting mod with the host.
- Version 1 packages contain namespaced JSON for survivors, items, traits,
  encounters, recipes, and starting conditions plus a manifest. IDs use a
  `mod_<slug>__<id>` namespace, references are local or explicitly canonical,
  and every file has `schema_version`.
- Load/export only from `user://mods/`; never mutate
  `Assets/StreamingAssets/Data/` at runtime. Reject path traversal, executable
  code, Godot resources, duplicate IDs, unknown fields outside the schema,
  oversized payloads, invalid references, and nondeterministic script hooks.
- `ScenarioEditorPanel` provides a node-based narrative editor, schema
  validation, starting conditions, preview simulation, and export. A package
  is playable only after validation, deterministic preview, and clean install;
  export should produce a manifest plus packaged JSON (with optional host-side
  archive creation), not generated C# or GDScript.
- Acceptance: authoring a scenario, validating it, exporting it, installing it
  under `user://mods/`, and selecting it from the campaign menu produces the
  same result after restart without modifying canonical data.

## 5. Data, host, and UI deliverables

Add validated data for brine chemistry, coke recipes, medical treatment cases,
wind curves, smokehouse recipes, faction/treaty terms, spirulina nutrition,
combat hides, crane capacities, print effects, perimeter sectors, bearing
specifications, geological sites, photo pairs, VLF channels, and scenario
schemas. Use existing catalog IDs wherever possible.

Create these presentation views, each backed by a typed read model and command
result:

`IodinePondsPanel`, `CokeOvenPanel`, `SkinGraftModal`, `AnemometerPanel`,
`SmokehousePanel`, `DiplomaticFeastModal`, `SpirulinaPanel`,
`SniperHidePanel`, `GantryCranePanel`, `WoodblockPrintModal`,
`ElectricFencePanel`, `BabbittBearingPanel`, `CoreSamplePanel`,
`StereoscopeModal`, `VlfBeaconPanel`, and `ScenarioEditorPanel`.

All panels need loading/empty/error states, accessible controls, deterministic
refresh on state-change events, and no hard-coded inventory, morale, combat,
or treaty results.

## 6. Verification and exit criteria

Core tests must cover deterministic replay, capture/restore, migration,
malformed-envelope rejection, units and ranges, by-product accounting,
contamination/failure, power outages, medical complications, combat outcomes,
signal attenuation, and all 16 acceptance fixtures.

Scenario/mod tests additionally require namespace isolation, schema and
cross-reference validation, path traversal rejection, executable-content
rejection, size limits, deterministic preview, clean install, restart
survival, and proof that canonical data remains unchanged.

Add host self-tests for `--batch11-selftest` and `--batch11-ui-selftest`, then
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

Batch 11 is ready when every acceptance fixture passes from a clean save,
replays identically with the same seed, survives save/load at active job
boundaries, respects existing system authorities, and the scenario editor can
export and play a validated mod without changing canonical content.
