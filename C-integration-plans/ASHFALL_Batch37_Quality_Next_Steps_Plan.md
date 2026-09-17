# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 37)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 577–592

## Outcome

Batch 37 strengthens youth civic participation, micro-energy, communal craft,
child welfare, book conservation, cold storage, internal logistics, strategic
recreation, memorial art, ventilation efficiency, food production, science
training, grid storage, and genealogy.

The implementation extends:

- `GenerationalSuccessionEngine`, `DutyRosterSystem`,
  `CrossingArbitrationSystem`, `FinalWishSystem`, and `NeedsSystem` for youth
  governance, foster care, education, sport, and social welfare.
- `JournalSystem`, `CraftingSystem`, `ResearchSystem`, and the archive/library
  contracts for books, monuments, tournaments, and genealogy.
- `LocationLayoutSystem`, `WeatherSystem`, `ShelterHazardLoop`,
  `GreenhouseSystem`, and animal/water contracts for buildings, ventilation,
  mushrooms, crayfish, and cold storage.
- The shared power authority for piezo harvesting, heat recovery, and pumped
  storage; `PowerGridPanel` remains a view/command adapter.

The roadmap’s `CenturySeed` and `GenerationalSuccessionSystem` names resolve to
the current generational engine. New state must use typed contracts rather than
UI-owned flags or direct global multipliers.

## Batch entry gates

1. Add `Batch37State` and `batch37_save.json` with checksummed persistence,
   explicit migrations, `CaptureState`/`RestoreState`, and state-change events.
2. Define child participation, foster consent and welfare, animal/ecological
   safeguards, competition safety, document provenance, and lineage confidence.
3. Model finite inputs and operating costs for piezo tiles, refrigeration,
   pneumatic tubes, mushroom labs, crayfish ponds, ventilation, and pumped
   storage. “Maintenance-free”, “perfect stability”, and “eliminates all” are
   not default outcomes.
4. Use `ISeededRng` for tournament performance, animal survival, fungal strains,
   weather, defects, and event outcomes. Never use `System.Random` or
   `Guid.NewGuid()` for simulation outcomes.
5. Add schema-versioned, snake_case catalog data under
   `Assets/StreamingAssets/Data/` and validate all references, ranges, and
   uniqueness before enabling a feature.

## Delivery order

1. **577–581:** Establish youth governance, micro-power, craft ceremony,
   foster welfare, and conservation/archive foundations.
2. **582–584:** Add ice-house logistics, pneumatic delivery, and strategic
   recreation with measurable outcomes.
3. **585–588:** Add memorial art, ventilation, ranged sport, and aquatic food.
4. **589–591:** Add mushroom research, surveying education, and pumped storage.
5. **592:** Publish the genealogy scroll from validated generational and
   archival records, then issue the dynasty milestone.

## Shared implementation contract

`Batch37State` owns feature IDs, references, schema version, and migrations;
individual Core systems own simulation state. Every step receives a thin
`src/UI/` presenter, catalog data, deterministic replay and state round-trip
tests, failure-path tests, and an acceptance fixture. Panels cannot directly
change children, lineages, power output, tactical accuracy, crop yield, or
ending state.

## Step integration matrix

### [577] Youth Parliament & Coming-of-Age Civic Education Programme

**Core integration:** Add `YouthParliamentSystem` over
`GenerationalSuccessionEngine`, `DutyRosterSystem`, `CrossingArbitrationSystem`,
education/age eligibility, safeguarding, delegate selection, resolution
records, and adult-council observation/ratification.

**Smallest vertical slice:** Enroll eligible 14–17-year-olds, elect delegates,
draft one library resolution, hold a supervised vote, and route it to adult
review.

**Acceptance gate:** Youth ballots are advisory until the adult council ratifies
them through the governance authority. Consent, safeguarding, quorum,
abstention, and age eligibility are persisted. The first library resolution
must contain a valid proposal, vote record, and adult ratification event.

**UI:** `YouthParliamentPanel.cs` presents delegate rosters, forums, safeguards,
vote tally, adult review, and civic-learning evidence.

### [578] Piezoelectric Floor Tiles & Vibration Energy Harvesting Grid

**Core integration:** Add `PiezoHarvestingSystem` over the shared power
authority, `LocationLayoutSystem`, foot-traffic schedules, machinery vibration,
tile condition, rectifiers, accumulator storage, and maintenance.

**Smallest vertical slice:** Install one tile array, measure footfall/vibration,
rectify and store output, then settle its contribution to a low-power load.

**Acceptance gate:** Five kW is a rated scenario result requiring tile area,
traffic, machinery duty, conversion efficiency, wiring, and maintenance. Output
is supplemental and intermittent; tile fatigue, low occupancy, and electrical
losses prevent a constant free-power claim.

**UI:** `PiezoHarvestingPanel.cs` shows coverage, vibration density, voltage,
accumulator charge, condition, and actual grid contribution.

### [579] Hand-Thrown Raku Pottery Firing Ceremony & Smoke Patina

**Core integration:** Add `RakuCeremonySystem` over `CraftingSystem`,
`NeedsSystem`, `DutyRosterSystem`, kiln/ceramic state, fuel, reduction chamber,
artist identity, audience, and cultural events.

**Smallest vertical slice:** Throw one bowl, fire it, execute a supervised
reduction pull, record the patina/defects, and archive the ceremony outcome.

**Acceptance gate:** Attendance and morale are calculated from venue capacity,
safety, audience, novelty, and event quality. Unpredictable smoke patina is a
seeded quality result; one firing cannot guarantee a perfect bowl or unlimited
communal morale.

**UI:** `RakuFiringPanel.cs` presents kiln temperature, extraction safety,
reduction chamber, patina inspection, audience, and ceremony record.

### [580] Post-War Orphan Adoption Registry & Foster Family Programme

**Core integration:** Add `FosterCareSystem` over
`GenerationalSuccessionEngine`, `NeedsSystem`, `FinalWishSystem`,
`DutyRosterSystem`, child identity, guardian consent, family capacity, welfare
reviews, support stipends, and development records.

**Smallest vertical slice:** Register one unaccompanied child, screen one foster
family, obtain consent/approval, place the child, and complete a welfare review.

**Acceptance gate:** “All orphans placed” requires every active case to have a
validated placement, guardian/legal record, capacity, safeguarding check, and
successful review. Failed matches, disruption, grief, stipends, and child
preferences remain possible; the registry cannot erase vulnerability by flag.

**UI:** `OrphanFosterCarePanel.cs` displays child records, matching factors,
consent, capacity, review calendar, stipends, and development.

### [581] Coptic Bookbinding Studio & Hand-Sewn Book Conservation

**Core integration:** Add `BookbindingConservationSystem` over
`JournalSystem`, `CraftingSystem`, `ResearchSystem`, archive/library storage,
leather/thread/paper goods, document condition, deacidification, and recovery
provenance.

**Smallest vertical slice:** Inspect one damaged book, choose a conservation
plan, repair/rebind it, update condition, and make it searchable again.

**Acceptance gate:** Two hundred technical books count only when each has a
source ID, condition record, intervention log, restored content, and safe
storage. Conservation can fail or alter an artefact; restored knowledge must be
reviewed before it becomes a research unlock.

**UI:** `CopticBindingStudioPanel.cs` presents sewing, cover tooling,
deacidification, tissue repair, condition, and archive retrieval.

### [582] Subterranean Ice Harvest, Ice House & Year-Round Cold Storage

**Core integration:** Add `IceHouseSystem` over the medical host, `NeedsSystem`,
`LocationLayoutSystem`, `WeatherSystem`, ice inventory, sawdust insulation,
thermal exchange, cold-chain items, and spoilage.

**Smallest vertical slice:** Harvest winter ice, store it in one insulated cell,
log temperature, and preserve one vaccine/blood/food lot through a warm interval.

**Acceptance gate:** Two degrees Celsius through summer requires ice mass,
insulation quality, ambient conditions, loading schedule, thermometers, and
maintenance. Blood and vaccine validity depends on cold-chain history; ice can
run out, melt, or be contaminated.

**UI:** `IceHousePanel.cs` shows harvest, packing, cell temperature, ice reserve,
cold-chain lots, and spoilage alerts.

### [583] Pneumatic Tube Mail & Document Delivery Network

**Core integration:** Add `PneumaticTubeSystem` over
`LocationLayoutSystem`, `RadioHostSession` only as a message/event adapter,
`DutyRosterSystem`, item custody, pressure stations, route topology, and
delivery hazards.

**Smallest vertical slice:** Register two stations, dispatch a medicine vial,
track canister pressure and route, and complete an authenticated delivery.

**Acceptance gate:** Thirty seconds is an eligible-route target requiring
pressure, distance, station health, canister mass, and no blockage. Contents,
sender, recipient, custody, breakage, pressure loss, and emergency priority are
saved; the tube is not instantaneous and cannot transport arbitrary items.

**UI:** `PneumaticTubePanel.cs` presents stations, canister contents, route,
pressure, queue, custody, and delivery time.

### [584] Colony Chess League, Strategy Game Tournament & Tactical Academy

**Core integration:** Add `ChessLeagueSystem` over `NeedsSystem`,
`DutyRosterSystem`, `TacticalCombatSystem`, player skill, tournament brackets,
annotation records, and commander training evidence.

**Smallest vertical slice:** Register players, run a seeded game, annotate one
position, and award a tactical lesson to an eligible commander.

**Acceptance gate:** One year of league play requires completed games, opponent
strength, attendance, fatigue, and training transfer. A -30% planning-error
effect is a cohort modifier with a comparison baseline; chess study improves
decision quality without guaranteeing battlefield outcomes.

**UI:** `ChessLeaguePanel.cs` shows bracket, boards, annotations, patterns,
player ratings, and commander training transfer.

### [585] Artisan Bronze Age-Style Lost-Wax Sculpture & Memorial Statuary

**Core integration:** Add `BronzeStatuarySystem` over `SilentFoundrySystem`,
`CraftingSystem`, `LocationLayoutSystem`, memorial/history records, wax/plaster/
bronze goods, casting defects, plinth construction, and dedication events.

**Smallest vertical slice:** Model one approved hero, form a wax master, cast and
inspect the bronze, install a plinth, and dedicate the monument.

**Acceptance gate:** The “Inspired by Legacy” bonus is granted once from a valid
hero record, quote/provenance, casting inspection, location, and dedication.
Metal loss, porosity, miscasts, labour, and material availability remain part of
the process.

**UI:** `BronzeStatuaryPanel.cs` covers wax armature, mould, pour, risers,
inspection, plinth, inscription, and dedication.

### [586] Mechanical Ventilation Heat Recovery Unit & Air-to-Air Exchanger

**Core integration:** Add `HeatRecoveryVentilationSystem` over the shared power
and thermal authority, `WeatherSystem`, `LocationLayoutSystem`, air quality,
airflow balance, frost protection, filters, and maintenance.

**Smallest vertical slice:** Install one exchanger, measure supply/exhaust
temperatures and airflow, run a winter interval, and settle heat savings and
air-quality state.

**Acceptance gate:** Eighty-five percent is rated exchanger effectiveness, not
guaranteed seasonal savings. A -40% heating-demand result requires coverage,
airflow, outdoor temperature, frost cycles, filter condition, and demand
baseline; heat recovery cannot mix contaminants or eliminate ventilation load.

**UI:** `HeatRecoveryVentPanel.cs` shows airflow, temperatures, effectiveness,
frost bypass, filters, air quality, and actual heat/power impact.

### [587] Competitive Archery, Javelin & Sling Olympic Sports Trials

**Core integration:** Add `RangedSportsSystem` over `NeedsSystem`,
`DutyRosterSystem`, `TacticalCombatSystem`, equipment safety, event rules,
training transfer, and military call-up eligibility.

**Smallest vertical slice:** Run one safe event, score a competitor, record
coaching and fatigue, and award a validated precision certification.

**Acceptance gate:** Fifty precision archers/slingers require qualifying scores,
repeated performance, equipment safety, age/health eligibility, and current
training. Sports bonuses affect defined accuracy skills, not automatic combat
victory or zero injury.

**UI:** `RangedSportsPanel.cs` displays score rings, distances, groupings,
officials, fatigue, certifications, and call-up pool.

### [588] Freshwater Crayfish Aquaculture & Crustacean Fishery

**Core integration:** Add `CrayfishAquacultureSystem` over aquatic water
quality, `GreenhouseSystem`/food production, `NeedsSystem`, feed, stocking,
dissolved oxygen, disease, harvest, and ecological containment.

**Smallest vertical slice:** Commission one pond, stock a measured cohort,
maintain water quality, feed it, and harvest one batch.

**Acceptance gate:** Thirty kg/week requires pond capacity, biomass, feed,
oxygen, temperature, survival, and water exchange. Escape, disease, seasonal
growth, and harvest depletion are modeled; “self-breeding with little input” is
not a free protein source.

**UI:** `CrayfishPondPanel.cs` shows pond state, density, oxygen, feed, disease,
growth, harvest, and containment.

### [589] Underground Mushroom Spawn Laboratory & Mycelium Research

**Core integration:** Add `MushroomSpawnLabSystem` over `GreenhouseSystem`,
`ResearchSystem`, medical/sterile-lab boundaries, culture inventory, strain
provenance, contamination, radiation exposure, and yield trials.

**Smallest vertical slice:** Isolate one culture, test contamination, produce
spawn, inoculate a controlled cavern batch, and compare yield with a baseline.

**Acceptance gate:** A threefold yield increase requires replicated trials,
strain identity, environmental equivalence, contamination checks, and
harvest-mass accounting. Radiation resistance is a measured trait, not a label;
failed cultures and unsafe spores remain possible.

**UI:** `SpawnLabPanel.cs` presents sterile hood, agar plates, strain lineage,
spawn jars, assay results, and cavern trial comparison.

### [590] Colony Surveying School & Cadastral Cartography Diploma

**Core integration:** Add `SurveyingSchoolSystem` over `DutyRosterSystem`,
`GenerationalSuccessionEngine`, `ResearchSystem`, expedition/location survey
state, instruments, examinations, licenses, and cadastral dispute records.

**Smallest vertical slice:** Teach one class, complete a field traverse and
exam, issue one license, and use the survey in an infrastructure boundary.

**Acceptance gate:** Ten licensed surveyors require practical and written
assessments, instrument calibration, ethics, and license validity. Boundary
disputes and misalignment become less likely through evidence and review; they
cannot be globally eliminated by awarding diplomas.

**UI:** `SurveyingSchoolPanel.cs` displays instruments, lessons, field work,
exams, licenses, and cadastral confidence.

### [591] Hydroelectric Pumped-Storage Reversible Turbine Reservoir

**Core integration:** Add `PumpedStorageSystem` over the shared power
authority, `LocationLayoutSystem`, water inventory, upper/lower reservoirs,
head/flow, reversible pump-turbine, round-trip efficiency, and grid frequency.

**Smallest vertical slice:** Fill an upper reservoir from surplus power, switch
to generation at peak demand, and settle losses, water level, and frequency
response.

**Acceptance gate:** Eighty percent is a configured round-trip efficiency subject
to head, pump/turbine condition, friction, leakage, and dispatch. “Perfect grid
stability” is achieved only for a defined demand fixture with reserves; drought,
capacity limits, faults, and competing loads remain possible.

**UI:** `PumpedStoragePanel.cs` shows reservoir levels, mode, head, efficiency,
dispatch, frequency response, and maintenance.

### [592] Grand Founders’ Illuminated Genealogy Scroll & Dynasty Archive

**Core integration:** Add `GenealogyScrollSystem` over
`GenerationalSuccessionEngine`, `JournalSystem`, archive records, consent,
lineage confidence, manuscript materials, `SaveChecksum`, and the campaign
history/epilogue layer.

**Smallest vertical slice:** Render one validated family branch, illuminate and
annotate it, archive the source references, and display uncertainty for missing
or disputed parentage.

**Acceptance gate:** The 100-year scroll is complete only when every available
lineage is represented with source/provenance and unknown branches are marked,
not fabricated. The Dynasty Archive milestone is idempotent, checksummed, and
requires conservation and publication records.

**UI:** `GenealogyScrollPanel.cs` presents branches, sources, confidence,
annotations, gilding, conservation, and publication ceremony.

## Data, UI, and test deliverables

- Add `batch37_features.json`, youth eligibility and safeguards, piezo tile
  specs, raku rules, foster/case schemas, book conservation methods, cold-chain
  thresholds, tube routes, chess ratings, memorial records, HRV parameters,
  sports qualification, crayfish species, spawn trial data, survey curricula,
  pumped-storage limits, and genealogy provenance under
  `Assets/StreamingAssets/Data/`.
- Add thin presenters: `YouthParliamentPanel.cs`, `PiezoHarvestingPanel.cs`,
  `RakuFiringPanel.cs`, `OrphanFosterCarePanel.cs`,
  `CopticBindingStudioPanel.cs`, `IceHousePanel.cs`, `PneumaticTubePanel.cs`,
  `ChessLeaguePanel.cs`, `BronzeStatuaryPanel.cs`,
  `HeatRecoveryVentPanel.cs`, `RangedSportsPanel.cs`,
  `CrayfishPondPanel.cs`, `SpawnLabPanel.cs`, `SurveyingSchoolPanel.cs`,
  `PumpedStoragePanel.cs`, and `GenealogyScrollPanel.cs`.
- Test child consent/safety, welfare placement, conservation provenance,
  temperature/cold-chain, delivery timing, tournament replay, animal ecology,
  sensor drift, energy losses, and lineage gaps. Add Godot command-path,
  disabled-state, navigation, and layout tests.
- Run the Core build/test, Godot project build, four headless self-tests, and
  `./scripts/ci/godot-asset-gate.sh` required by `AGENTS.md`.

## Batch completion definition

Batch 37 is complete when all 16 systems are catalog-validated,
deterministically replayable, round-trip safe, and reachable through Godot UI;
child welfare, archive, ecological, logistics, sports, and energy outcomes are
auditable; and the genealogy milestone reflects actual lineage evidence without
inventing missing history.
