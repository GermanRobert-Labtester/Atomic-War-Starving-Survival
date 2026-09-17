# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 32)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 497–512

## Outcome

Batch 32 moves ASHFALL from a durable colony to a self-governing civilizational
network. It combines generational husbandry, diplomacy, ceramics, ecological
water treatment, archive infrastructure, aquaponics, maritime telemetry,
precision manufacturing, and the final Renaissance milestone.

The implementation must extend these repository authorities:

- **Generations and legitimacy:** `GenerationalSuccessionEngine`,
  `CrossingArbitrationSystem`, `WarlordDoctrineSystem`, `FinalWishSystem`, and
  typed `IEventBus` contracts.
- **Food and materials:** `GreenhouseSystem`, `NeedsSystem`, `RecipeCatalog`,
  `GoodsCatalog`, `CraftingSystem`, `SilentFoundrySystem`, and the shared water
  and power authorities.
- **World knowledge and hazards:** `JournalSystem`, `ResearchSystem`,
  `LocationLayoutSystem`, `WeatherSystem`, `ShelterHazardLoop`, and
  `StealthDiveInstance`.
- **Continuity and ending:** `SaveChecksum`, `EpilogueMatrixRuntime`, the
  generational/archive contracts, and an authoritative domain-mastery
  validator.

The roadmap’s `CenturySeed`, `GenerationalSuccessionSystem`,
`District8Accords`, and `PowerGridPanel` names are integration concepts, not
permission to create parallel systems. Use the actual generational engine,
validated agreement IDs, and typed power-generation transactions. The typo
`StonewapeKilnPanel.cs` in the source roadmap is corrected here to
`StonewareKilnPanel.cs`.

## Batch entry gates

1. **Batch state and persistence:** Add `Batch32State` and
   `batch32_save.json`, register it with the existing save host, and implement
   explicit state capture/restore, migrations, events, and deterministic IDs.
2. **Generation-safe progression:** Pedigrees, treaties, archive documents,
   domain flags, and monuments use stable references to people, settlements,
   species, and authored data. No completion flag is inferred from UI state.
3. **Resource and safety accounting:** Water, fish, crops, clay, fuel, glass,
   silk, labor, oxygen, ecological capacity, and power settle through typed
   transactions. Treatment systems never bypass potable-water validation.
4. **Environmental uncertainty:** Seismic, ocean, wetland, hydro, and buoy
   systems expose sampling confidence, failure modes, maintenance, and weather
   dependencies. “Infinite”, “zero footprint”, and “fully self-sufficient” are
   not unconditional state mutations.
5. **Diplomacy and consent:** Envoy security, prisoner/hostage context, animal
   welfare, indigenous knowledge provenance, genetic privacy, and treaty
   ratification are explicit records and event outcomes.
6. **Endgame integrity:** The Renaissance milestone reads a validated snapshot
   of ten domain authorities, ratifies an auditable charter, and writes an
   idempotent epilogue state through the checksummed save path.

## Delivery order

1. **497, 498, 500:** Establish generational, diplomatic, and knowledge
   provenance contracts.
2. **499, 503, 504, 509:** Build ceramic, water-polishing, archive, and glass
   infrastructure for later production and research.
3. **505, 506, 507, 508:** Add closed-loop food, luxury trade, ocean telemetry,
   and underground power with measured resource flows.
4. **501, 502, 510, 511:** Add seismic safety and cultural/precision works.
5. **512:** Validate all ten domains, sign the New World Charter, and trigger the
   final milestone only after replay and migration tests pass.

## Shared implementation contract

`Batch32State` owns feature registrations, cross-feature references, schema
version, completion proofs, and migration hooks. Feature systems own their
simulation state; the aggregate does not calculate production, combat, or
research itself. `SaveLoadHostSession` must include the new store in the normal
checksummed envelope and per-store manifest hash.

Every feature receives: a Core command/state boundary, catalog data under
`Assets/StreamingAssets/Data/`, a thin Godot presenter, deterministic tests,
state round-trip tests, invalid-input tests, and an acceptance fixture that
replays the user-facing “Done when” outcome without hard-coded UI state.

## Step integration matrix

### [497] Animal Husbandry Selective Breeding Program & Genetic Registers

**Core integration:** Add `LivestockBreedingSystem` over
`GenerationalSuccessionEngine`, `GreenhouseSystem` feed/crop contracts,
`NeedsSystem`, `DutyRosterSystem`, animal health, disease, and goods output.
Record animal identity, lineage, traits, inbreeding, welfare, selection, and
generation snapshots.

**Smallest vertical slice:** Register two healthy animals, create a permitted
breeding plan, advance one generation, record offspring traits, and update milk
or wool output after a measured production interval.

**Acceptance gate:** Five generations compare against the founding-herd baseline
with a configured breeding model. Doubling milk and tripling wool are bounded
fixture outcomes that require feed, herd health, sample size, and welfare; no
unbounded permanent multiplier or hidden cloning is allowed.

**UI:** `LivestockBreedingPanel.cs` shows pedigrees, trait matrix, health and
welfare, breeding eligibility, generation timeline, and yield comparison.

### [498] Diplomatic Embassy System & Foreign Faction Envoy Exchanges

**Core integration:** Add `EmbassyDiplomacySystem` over
`CrossingArbitrationSystem`, `WarlordDoctrineSystem`, `FinalWishSystem`, and
`IEventBus`. Model embassy site security, envoy identity, agenda, clauses,
trust, concessions, ratification, breach, and treaty expiration.

**Smallest vertical slice:** Receive one envoy, negotiate one trade or
non-aggression clause, obtain valid authority signatures, ratify it, and emit a
treaty event consumed by trade/defense systems.

**Acceptance gate:** A former enemy becomes a military ally only after both
parties satisfy clause requirements and the arbitration result is persisted.
Embassy safety, bad-faith choices, expiry, and breach produce explicit outcomes;
the UI cannot set faction trust or alliance directly.

**UI:** `EmbassyDiplomacyPanel.cs` presents reception, agenda, clause tree,
trust, concessions, signatures, and ratified treaty terms.

### [499] Hand-Thrown Stoneware Pottery Kiln & Functional Ceramics

**Core integration:** Add `StonewareKilnSystem` over
`SilentFoundrySystem`, `CraftingSystem`, `ResearchSystem`, clay/grog/glaze
goods, fuel, kiln atmosphere, and quality inspection.

**Smallest vertical slice:** Throw one vessel, fire it through a controlled
1,280°C schedule, inspect vitrification/porosity, and register it as a beaker
or insulator if it passes its category tolerance.

**Acceptance gate:** Two hundred chemical beakers and electrical insulators
require separate valid batches, clay/fuel inputs, and defect rates. Ceramics
reduce breakage risk but do not remove all glass use or all breakage. The UI and
data use `StonewareKilnPanel.cs`.

**UI:** `StonewareKilnPanel.cs` handles throwing, pyrometer schedule, atmosphere,
kiln defects, category quality, and production inventory.

### [500] Indigenous Plant Ethnobotany Archive & Lost Knowledge Revival

**Core integration:** Add `EthnobotanyArchiveSystem` over `JournalSystem`,
`GreenhouseSystem`, `ExpeditionSystem`, `ResearchSystem`, and consent/provenance
records. Separate specimen facts, elder testimony, community permissions, and
validated recipe/medicine unlocks.

**Smallest vertical slice:** Conduct a consented interview, collect one plant
specimen with location/provenance, mount it, and submit one candidate recipe for
research review.

**Acceptance gate:** Two hundred species are counted only when identity,
location, specimen, contributor attribution, and permissions are complete. The
30 medicine and 15 recipe outcomes are catalogued discoveries with evidence and
review status, not automatic appropriation or free unlocks.

**UI:** `EthnobotanyArchivePanel.cs` shows interview consent, transcript,
specimen chain of custody, community attribution, and review/unlock state.

### [501] Seismic Early Warning Sensor Network & Tremor Evacuation

**Core integration:** Add `SeismicEarlyWarningSystem` over `WeatherSystem`,
`LocationLayoutSystem`, `ShelterHazardLoop`, station health, synchronized clocks,
P/S arrival models, structural zones, and evacuation duties.

**Smallest vertical slice:** Register three stations, detect a synthetic P-wave,
triangulate an event, estimate S-wave arrival, and issue an evacuation command
to one tunnel zone.

**Acceptance gate:** Twenty-to-thirty seconds of warning is calculated from
station geometry and event depth, not displayed as a fixed countdown. A
magnitude 6.2 fixture gives 25 seconds and zero casualties only when evacuation
capacity and response duties are sufficient; false positives and station faults
remain possible and are saved.

**UI:** `SeismicWarningPanel.cs` displays station map, wave arrivals, event
confidence, evacuation zones, alarms, and structural response.

### [502] Hand-Laid Sanctuary Grand Hall Marble Opus Sectile Floor

**Core integration:** Add `OpusSectileSystem` over `LocationLayoutSystem`,
`GenerationalSuccessionEngine`, `DutyRosterSystem`, stone goods, labor safety,
design schema, repair, and cultural-event state.

**Smallest vertical slice:** Cut and place one validated tessera, persist its
pattern coordinate and material, repair a damaged piece, and advance ceremony
progress.

**Acceptance gate:** Completion requires full pattern coverage, material
inventory, tolerances, labor, and inspection. The “Surrounded by Beauty” trait is
an authored, bounded morale effect granted once by a ceremony event; it cannot
be repeatedly farmed by reopening the panel.

**UI:** `OpusSectileFloorPanel.cs` provides pattern selection, cut tolerances,
placement grid, material palette, defects, labor, and ceremony state.

### [503] Managed Wetlands Aquifer Recharge & Biofilter Polishing Beds

**Core integration:** Add `ManagedWetlandSystem` over the water/brine authority,
`WeatherSystem`, `LocationLayoutSystem`, treatment plant, hydrology, reed/cattail
ecology, groundwater monitoring, and pathogen/chemical assays.

**Smallest vertical slice:** Commission one wetland cell, run a measured flow,
sample influent/effluent, calculate retention and BOD/nutrient removal, and log
aquifer recharge.

**Acceptance gate:** Ten thousand litres/day reaches near-drinking quality only
after configured assays pass. Recharge is tracked separately from potable
storage; the wetland cannot bypass disinfection or declare water safe from a UI
label. Flooding, drought, clogging, pathogens, and maintenance are modeled.

**UI:** `ManagedWetlandPanel.cs` shows cell maps, flow/HRT, BOD and nutrient
removal, assay confidence, recharge volume, and maintenance alerts.

### [504] Convict Labor Grand Archive Library Building & Stacks

**Core integration:** Add `ArchiveLibrarySystem` over `JournalSystem`,
`DutyRosterSystem`, `LocationLayoutSystem`, `ResearchSystem`, fire protection,
accessibility, catalog indexing, conservation, and archive storage.

**Smallest vertical slice:** Build one stack, catalogue one document, store it
in a protected vault, retrieve it through the index, and record its conservation
state.

**Acceptance gate:** Ten thousand documents require unique catalog IDs, valid
provenance, storage capacity, and conservation records. A +25% research effect
is a single configured modifier with an explicit source and cap; it must not
stack invisibly with existing archive/research bonuses.

**UI:** `ArchiveLibraryPanel.cs` handles stack construction, card catalog,
vault access, document search, accessibility, and researcher modifiers.

### [505] Aquaponics Tilapia & Murray Cod Intensive Raceways

**Core integration:** Add `AquaponicsSystem` over `GreenhouseSystem`,
`NeedsSystem`, water treatment, fish stock, feed, oxygen, temperature, disease,
effluent nutrient balances, and crop beds.

**Smallest vertical slice:** Stock one raceway, maintain dissolved oxygen, feed
the fish, route measured effluent to a crop bed, and harvest one fish/vegetable
batch.

**Acceptance gate:** The 50 kg fish and 30 kg vegetable daily target requires
stock biomass, feed, oxygen, water, crop area, energy, and disease control. The
system is closed-loop in nutrient reuse, not zero-input; mortality, water loss,
and contamination can reduce output.

**UI:** `FishRacewayPanel.cs` shows stock, biomass, oxygen, feed, disease,
effluent nutrients, crop demand, and actual harvest.

### [506] Silk Moth Sericulture & Hand-Reeled Bombyx Silk Textile

**Core integration:** Add `SericultureSystem` over `GreenhouseSystem`,
`NeedsSystem`, `CraftingSystem`, trade goods, and diplomatic arbitration.
Model mulberry feed, silkworm health, temperature, cocoon yield, reeling loss,
fiber quality, labor, and gift provenance.

**Smallest vertical slice:** Grow mulberry feed, raise one cohort, harvest and
reel cocoons, grade ten metres of cloth, and submit it as a diplomatic gift.

**Acceptance gate:** Ten metres of qualified silk can improve a treaty only
through the embassy/arbitration contract. The item has real labor and feed cost,
and quality, damage, and delivery conditions affect gift value.

**UI:** `SericulturePanel.cs` shows mulberry supply, cohort health, cocoon
harvest, reeling, cloth quality, and trade/gift assignment.

### [507] Autonomous Solar-Powered Buoy Network & Ocean Weather Station

**Core integration:** Add `OceanBuoyNetworkSystem` over `WeatherSystem`,
`RadioHostSession`, `StealthDiveInstance`, maritime routes, solar/battery state,
buoy health, sensor calibration, and forecast assimilation.

**Smallest vertical slice:** Deploy one buoy, log wind/wave/temperature, transmit
one packet, and route the observation into a storm forecast.

**Acceptance gate:** A Category-5 storm receives a three-day warning only when
buoy coverage, battery, radio links, calibration, and forecast confidence meet
the fixture. Loss, drift, communications outage, and storm damage are possible;
the network cannot promise 72 hours for every storm.

**UI:** `OceanBuoyNetworkPanel.cs` presents deployment routes, buoy health,
solar charge, sensor charts, radio status, and forecast uncertainty.

### [508] Underground River Hydroelectric Dam & Siphon Turbines

**Core integration:** Add `UndergroundHydroSystem` over
`LocationLayoutSystem`, surveyed river geometry and flow, water/ecology,
structural hazard, turbine maintenance, and the shared power authority.

**Smallest vertical slice:** Survey a river section, build a safe intake and
dam, commission one siphon turbine, and settle output from head, flow, and
efficiency.

**Acceptance gate:** One megawatt is a capacity fixture available only at the
measured flow/head and grid connection. Dam safety, sediment, drought, flooding,
ecological flow, cavern stability, and maintenance can reduce output. “Energy
self-sufficient” is a scenario score after demand and reserve checks, not a
literal infinite source.

**UI:** `UndergroundHydroPanel.cs` shows flow/head, dam progress, ecology,
turbine efficiency, structural alarms, and actual grid contribution.

### [509] Glassblowing Studio & Scientific Apparatus Fabrication

**Core integration:** Add `GlassblowingSystem` over `SilentFoundrySystem`,
`ResearchSystem`, `CraftingSystem`, borosilicate goods, torch/annealing state,
calibration, cleaning, and breakage.

**Smallest vertical slice:** Form and anneal one flask, calibrate its volume,
inspect it, and run one chemistry recipe that consumes it.

**Acceptance gate:** A complete apparatus set unlocks ten synthesis pathways only
when the individual vessels meet category tolerances. Thermal shock, porosity,
operator skill, and breakage remain modeled; the studio improves availability
but does not make glass indestructible.

**UI:** `GlassblowingStudioPanel.cs` covers torch settings, annealing, calibration,
inspection, breakage, and apparatus inventory.

### [510] Mechanical Barrel Organ Street Music & Colony Festivity Engine

**Core integration:** Add `BarrelOrganSystem` over `NeedsSystem`,
`DutyRosterSystem`, `CraftingSystem`, `SoundManager`, festival scheduling,
attendee eligibility, maintenance, and cultural journal records.

**Smallest vertical slice:** Assemble one cylinder, tune bellows/pipe ranks,
schedule a festival performance, and record attendance and morale change.

**Acceptance gate:** The Founders’ Day event receives a bounded morale score
based on audience, novelty, venue, maintenance, and competing events. “Highest
single-day morale boost” is valid only if the game records a comparable event
score; no unconditional maximum is granted.

**UI:** `BarrelOrganPanel.cs` provides cylinder selection, bellows pressure,
rank tabs, performer duty, festival schedule, and event results.

### [511] Precision Optical Comparator & Gear Tooth Profile Inspection

**Core integration:** Add `OpticalComparatorSystem` over
`SilentFoundrySystem`, `ShelterOperationsPanel` as an adapter, metrology
calibration, gear/tool catalogs, templates, and manufacturing quality records.

**Smallest vertical slice:** Calibrate the 50x comparator, inspect one gear
profile against a validated template, and issue a certificate with tolerances.

**Acceptance gate:** Class-6 AGMA certification requires calibrated optics,
template provenance, tooth/profile measurements, runout, and operator skill.
All workshop gears may be certified only after all are inspected; the result
unlocks gearbox assembly through a quality contract rather than a global flag.

**UI:** `OpticalComparatorPanel.cs` shows calibration, workpiece, overlay,
measurements, defects, and certification history.

### [512] Grand Civilizational Renaissance Milestone & New World Charter

**Core integration:** Add `RenaissanceMilestoneSystem` over
`GenerationalSuccessionEngine`, `EpilogueMatrixRuntime`, `SaveChecksum`,
`JournalSystem`, and ten domain validators. The system consumes a versioned
domain snapshot and publishes one idempotent `RenaissanceAchieved` event.

**Smallest vertical slice:** Validate one domain, draft charter clauses, collect
authorized signatures, save the snapshot, reload it, and confirm that the
milestone remains pending until all ten domains pass.

**Acceptance gate:** All ten domains must be independently valid, current, and
non-corrupt; the charter must contain versioned clauses, signatories, and date.
The final cinematic/epilogue is triggered once, survives save/load, rejects a
malformed checksum or stale domain snapshot, and cannot be replay-farmed.

**UI:** `RenaissanceMilestonePanel.cs` displays domain arcs, validator evidence,
charter clauses, signature ceremony, final tally, and epilogue state.

## Data, UI, and test deliverables

- Add `batch32_features.json`, livestock trait/pedigree schemas, diplomatic
  clauses, ethnobotany provenance, clay/ceramic quality, seismic stations,
  wetland treatment thresholds, archive schemas, aquaponics species, silk
  recipes, buoy sensors, underground hydrology, glass apparatus, organ music,
  comparator tolerances, and ten domain validators under
  `Assets/StreamingAssets/Data/`.
- Add `src/UI/LivestockBreedingPanel.cs`, `EmbassyDiplomacyPanel.cs`,
  `StonewareKilnPanel.cs`, `EthnobotanyArchivePanel.cs`,
  `SeismicWarningPanel.cs`, `OpusSectileFloorPanel.cs`,
  `ManagedWetlandPanel.cs`, `ArchiveLibraryPanel.cs`, `FishRacewayPanel.cs`,
  `SericulturePanel.cs`, `OceanBuoyNetworkPanel.cs`,
  `UndergroundHydroPanel.cs`, `GlassblowingStudioPanel.cs`,
  `BarrelOrganPanel.cs`, `OpticalComparatorPanel.cs`, and
  `RenaissanceMilestonePanel.cs` as presentation/wiring only.
- Add Core tests for generation baselines, treaty idempotence, consent and
  provenance, mass/resource conservation, water-quality rejection, sensor
  uncertainty, maintenance failure, quality certificates, and endgame replay.
  Add Godot tests for every panel’s command path, disabled/invalid states,
  navigation, and layout at supported resolutions.
- Run the repository verification gates from `AGENTS.md`: Core build/test,
  `Ashfall.csproj` build, all four Godot headless self-tests, and
  `./scripts/ci/godot-asset-gate.sh`.

## Batch completion definition

Batch 32 is complete when all 16 systems are catalog-validated, deterministic,
round-trip safe, and reachable through Godot UI; diplomatic, ecological,
archival, production, and generational outcomes are journaled; and the ten
domain validators can produce a checksummed New World Charter and one-time
Grand Renaissance epilogue without bypassing the repository’s Core/host
architecture.
