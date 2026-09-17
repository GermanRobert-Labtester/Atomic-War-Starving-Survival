# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 31)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 481–496

## Outcome

Batch 31 adds the human, ecological, and archival layer that turns a functioning
colony into a humane civilization. The work is ordered around four durable
authorities:

- **Human welfare and governance:** `FinalWishSystem`, the medical host
  boundary, `NeedsSystem`, `DutyRosterSystem`, `CrossingArbitrationSystem`,
  `JournalSystem`, and typed event contracts.
- **Production and food:** `SilentFoundrySystem`, `GreenhouseSystem`, recipe
  and goods catalogs, `CraftingSystem`, and the shared power/utility contract.
- **Field science:** `WeatherSystem`, `LocationLayoutSystem`,
  `ExpeditionSystem`, `ResearchSystem`, and `RadioHostSession`.
- **Continuity:** `GenerationalSuccessionEngine`, archival records, and the
  checksummed save pipeline.

The roadmap names `MedicalSystem`, `CenturySeed`, `District8Accords`, and
`PowerGridPanel` as conceptual integration points. In this repository they are
adapted as follows: medical gameplay goes through the existing medical host and
trauma/needs contracts; Century Seed uses `GenerationalSuccessionEngine`; trade
accords use validated agreement IDs; and power production/consumption uses a
typed shared power authority rather than a UI panel. No new duplicate authority
is introduced.

## Batch entry gates

1. **Records and consent:** Add a `Batch31State` aggregate and
   `batch31_save.json` store. Every stateful feature implements
   `CaptureState`/`RestoreState`, raises state-change events, and persists stable
   IDs, provenance, and versioned timestamps.
2. **Welfare safety:** Grief, accessibility, ethics, medical compounds, and
   cryogenic preservation must model consent, eligibility, capacity, risk, and
   recovery. UI panels are presenters and command dispatchers only.
3. **Resource accounting:** Candles, oil, mushrooms, medicines, electroforms,
   heat, water, power, and archive materials settle through catalogued goods and
   atomic inventory transactions. “Zero input”, “perpetual”, and “flawless” are
   quality states or balance targets, never free resources.
4. **Exploration evidence:** Cave rivers, bird migration, pulsar timing, and
   semaphore routes require position, uncertainty, environmental conditions, and
   survey provenance before unlocking a map or global modifier.
5. **Data authority:** Add snake_case catalog entries under
   `Assets/StreamingAssets/Data/` with `schema_version`; validate references,
   ranges, uniqueness, and feature IDs before the UI can display them.
6. **Determinism:** Use `ISeededRng` for capture, weather, production defects,
   and ecology outcomes. Never use `System.Random` or `Guid.NewGuid()` for
   simulation outcomes.

## Delivery order

1. **481, 482, 487:** Establish bereavement, accessibility, and ethics records;
   these define the human and event contracts used by later culture systems.
2. **483, 485, 486, 489, 490, 491:** Add timekeeping, food, lighting, precision
   production, heating, and medical crop pipelines.
3. **484, 488:** Add bird ecology and cave exploration with environmental evidence
   and water-source validation.
4. **492, 493, 494, 495:** Add power, timing, cryobank, and resilient field
   communication integrations.
5. **496:** Complete the durable writing-material and archive workflow, then run
   the batch-wide migration and replay tests.

## Shared implementation contract

Create one `Batch31State`/store facade that owns feature IDs, schema version,
cross-feature references, and migration registration. It may reference typed
system interfaces, but it must not contain simulation logic that belongs in Core.
The host registers the store with `SaveLoadHostSession`; the normal checksummed
envelope and SHA-256 slot manifest therefore cover the batch automatically.

Each feature gets a Core state/command boundary, a thin Godot panel, catalog
data, and tests for: deterministic replay, state round-trip, invalid input,
resource settlement, event emission, and UI-to-command wiring. New panels belong
under `src/UI/`; no panel may mutate a save or inventory directly.

## Step integration matrix

### [481] Colony Grief Counselling, Funeral Rites & Bereavement Archive

**Core integration:** Add `BereavementSystem` over `FinalWishSystem`, the
medical host boundary, `NeedsSystem`, and `GenerationalSuccessionEngine`. Store
deceased identity, relationship graph, grief cases, counsellor duty, rite type,
attendance, consent, and the Book of the Departed in `JournalSystem`-compatible
records.

**Smallest vertical slice:** Resolve one death, schedule a private counselling
session and a collective rite, record attendance and archive entry, then apply a
bounded grief-recovery event to eligible survivors.

**Acceptance gate:** A mass-casualty event creates one case per affected survivor,
prevents duplicate rites, survives save/load, and emits an auditable ceremony
record. The “no morale spiral” outcome is a tested recovery threshold for the
configured event, not a hard-coded immunity to grief.

**UI:** `BereavedCounsellorPanel.cs` displays caseload, consent, ceremony
schedule, Book of the Departed, and recovery status.

### [482] Tactile Braille Press & Accessibility Library

**Core integration:** Add `BrailleAccessibilitySystem` over `JournalSystem`,
recipe/map document sources, `DutyRosterSystem`, survivor impairment state, and
`NeedsSystem`. Use a versioned six-dot translation table and accessibility
document IDs; do not store translated text as an unvalidated UI string.

**Smallest vertical slice:** Select one survival guide, translate and emboss it,
assign it to an eligible vision-impaired survivor, and expose one independent
crafting or navigation task.

**Acceptance gate:** Translation, press wear, paper/plate inventory, reader
eligibility, assistive access, and task completion are deterministic and saved.
Three survivors gain independent work/navigation only after receiving valid
guides and accommodations; the feature does not silently remove impairment.

**UI:** `BrailleEmbossPanel.cs` provides document selection, dot-cell preview,
press controls, production defects, and distribution records.

### [483] Mechanical Watchmaking Guild & Lever Escapement Movements

**Core integration:** Add `WatchmakingGuildSystem` over
`SilentFoundrySystem`, `DutyRosterSystem`, `CraftingSystem`, and the shared time
service. Model components, jewel/pivot quality, escapement regulation, drift,
maintenance, guild skill, and watch assignment to expedition schedules.

**Smallest vertical slice:** Train one watchmaker, assemble and regulate one
watch, record its drift certificate, and use it to coordinate two scheduled
expedition actions.

**Acceptance gate:** Fifty watches require 50 valid component sets and work
orders. Timing bonuses depend on calibration and maintenance; a watch cannot
rewrite `SimClock` or guarantee perfect coordination when drift or weather
invalidates the schedule.

**UI:** `WatchmakingGuildPanel.cs` shows work orders, escapement inspection,
poising, drift certificates, and assignment status.

### [484] Migratory Bird Banding Station & Avian Population Ecology

**Core integration:** Add `BirdBandingSystem` over `WeatherSystem`,
`LocationLayoutSystem`, seasonal time, species catalog data, and
`JournalSystem`. Band identifiers are deterministic issued IDs, not GUID-based
simulation outcomes.

**Smallest vertical slice:** Capture, ethically band, release, and later recover
one bird; record species, season, location, condition, and weather.

**Acceptance gate:** Net coverage, animal welfare, species identification,
banding loss, migration season, recovery probability, and observation confidence
are saved. Five hundred recoveries across three seasons complete the atlas only
when the records meet the configured species and geography thresholds.

**UI:** `BirdBandingPanel.cs` presents net layout, band fitting, field guide,
recovery records, and atlas confidence.

### [485] Underground Cavern Mushroom Farm & Bioluminescent Varieties

**Core integration:** Add `CavernMushroomSystem` over `GreenhouseSystem`,
`NeedsSystem`, `LocationLayoutSystem`, water, substrate goods, ventilation,
humidity/CO2/temperature, contamination, and the shared power authority.

**Smallest vertical slice:** Validate one cavern chamber, inoculate one rack,
advance its growth cycle, harvest edible mushrooms, and record a contamination
check and food transaction.

**Acceptance gate:** The farm consumes substrate, water, ventilation capacity,
and any required energy; “zero surface resources” means low surface dependence,
not free production. The 200 kg/week target is a configured capacity requiring
enough racks and validated environmental conditions. Bioluminescence is a
visual/quality property and never changes food safety by itself.

**UI:** `CavernMushroomFarmPanel.cs` shows chamber conditions, rack cycles,
substrate, contamination, glow intensity, and harvest yields.

### [486] Artisanal Tallow Chandlery, Beeswax Candles & Lantern Oil Press

**Core integration:** Add `ChandlerySystem` over `RecipeCatalog`, `GoodsCatalog`,
`NeedsSystem`, livestock by-products, fuel, and light-source state. Agreement
IDs may provide trade inputs, but `District8Accords` is not a gameplay class.

**Smallest vertical slice:** Render tallow, make one candle and one oil batch,
register burn duration and light output, then consume them during a power outage.

**Acceptance gate:** Fat, wax, wick, seed, heat, and labour settle through
recipes. Five hundred candles and 50 litres/month are production targets based on
input and duty capacity; light sources add fire risk, emissions, and maintenance.

**UI:** `ChandleryPanel.cs` covers rendering, dipping/moulding, oil pressing,
inventory, burn duration, and emergency lighting coverage.

### [487] Philosophical Debate Hall, Ethics Council & Moral Dilemma System

**Core integration:** Add `EthicsCouncilSystem` over
`CrossingArbitrationSystem`, `FinalWishSystem`, `DutyRosterSystem`, and
`IEventBus`. Model council membership, eligibility, dilemma provenance, quorum,
votes, abstention, legitimacy, public record, and typed consequence events.

**Smallest vertical slice:** Present the first ration dilemma, collect a valid
quorum vote, publish the result, and route one consequence to ration policy and
one to social trust.

**Acceptance gate:** Identical seed and state reproduce the vote and outcome;
missing quorum, coercion, or invalid council membership rejects the decision.
Consequences are explicit events consumed by owning systems, not a UI-side
mutation of many unrelated stats. The decision is preserved in the journal.

**UI:** `EthicsCouncilPanel.cs` displays dilemma text, eligibility, vote tally,
quorum, consequence preview, and permanent decision record.

### [488] Caving Speleology Expedition & Underground River Discovery

**Core integration:** Add `SpeleologySystem` over `ExpeditionSystem`,
`LocationLayoutSystem`, `JournalSystem`, geology/water quality, hazard, and
navigation state. Cave chambers and rivers become map entities with survey
confidence, flow, contamination, and access risk.

**Smallest vertical slice:** Survey one cave traverse, measure a river segment,
collect a sample, validate water quality, and unlock a safe map route.

**Acceptance gate:** A 500-metre river discovery requires connected surveyed
segments and valid sample records. “Pristine” water is a tested quality result,
not an automatic label; the river can become a source only after treatment,
capacity, hazards, and ecological impact are resolved.

**UI:** `SpeleologyPanel.cs` provides traverse instruments, flow/sample readings,
hazard state, journal entries, and discovered chamber maps.

### [489] Electrochemical Copper Electroforming & Micro-Sculpture Replication

**Core integration:** Add `ElectroformingSystem` over `SilentFoundrySystem`,
`ResearchSystem`, `CraftingSystem`, bath chemistry, mandrel inventory, current
density, deposition thickness, defects, and inspection.

**Smallest vertical slice:** Prepare a wax master, run one bath, strip and inspect
the shell, and register it as an investment-casting pattern.

**Acceptance gate:** Bath composition, current, time, shell mass, adhesion,
voids, and waste are authoritative. “Flawless” means the shell passes the
configured inspection tier. The turbine impeller pattern unlocks casting only
after dimensional verification.

**UI:** `ElectroformingPanel.cs` shows bath chemistry, current density,
thickness, defect inspection, and pattern inventory.

### [490] Convict Labor Roman-Style Underground Hypocaust Floor Heating

**Core integration:** Add `HypocaustSystem` over `DutyRosterSystem`,
`LocationLayoutSystem`, `NeedsSystem`, fuel/thermal state, fire safety, and air
quality. Labour assignment must respect the existing prisoner/care rules.

**Smallest vertical slice:** Build one heated floor segment, route hot gas,
measure temperature and air quality, and simulate one winter interval.

**Acceptance gate:** Dormitory comfort improves only within covered rooms and
available fuel. Carbon monoxide, fire, maintenance, and worker safety checks are
persisted. The deep-winter acceptance target is met for all equipped dorms, not
for uncovered colony spaces, and only under safe air-quality thresholds.

**UI:** `HypocaustHeatingPanel.cs` presents tile construction, channel routing,
temperature, fuel, air quality, and coverage.

### [491] Hydroponic Vertical Tulsi Holy Basil & Ayurvedic Medicine Chest

**Core integration:** Add `TulsiMedicineSystem` over `GreenhouseSystem`,
`RecipeCatalog`, medical treatment, disease state, and `NeedsSystem`. Treat
plant compounds as catalogued preparations with efficacy, contraindication, and
interaction data.

**Smallest vertical slice:** Grow and harvest tulsi, distil one compound,
formulate a medicine chest, and apply it to one eligible condition under medical
supervision.

**Acceptance gate:** Compound potency, dose, expiry, contraindications, and
evidence tier are saved. The -40% sickness-days target is a configured,
eligible expedition effect measured over a cohort; tulsi cannot silently cure
all diseases or replace clinical treatment.

**UI:** `TulsiMedicinePanel.cs` shows crop state, distillation, formulation,
stock, evidence, and patient eligibility.

### [492] Micro-Hydroelectric Pelton Wheel High-Head Water Turbines

**Core integration:** Add `PeltonHydroSystem` over `LocationLayoutSystem`,
surveyed water flow/head, turbine maintenance, and the shared power authority.
Power is produced through a typed generation transaction and consumed by the
grid; `PowerGridPanel` only displays and commands it.

**Smallest vertical slice:** Commission one nozzle, measure head and flow, run a
+load interval, and settle generated power after efficiency and maintenance loss.

**Acceptance gate:** The 500 kW target is available only when the mountain
spring has the required flow/head and the generator is connected. Dry-season
flow, icing, silt, mechanical wear, and grid curtailment are modeled; the
feature cannot write an unconditional 500 kW value.

**UI:** `PeltonWheelPanel.cs` shows head/flow, nozzle state, RPM, efficiency,
maintenance, and actual grid contribution.

### [493] Astronomical Radio Telescope & Pulsar Timing Navigation

**Core integration:** Add `PulsarTimingSystem` over `RadioHostSession`,
`ResearchSystem`, `LocationLayoutSystem`, dish tracking, a versioned pulsar
catalog, dispersion correction, clock uncertainty, and navigation consumers.

**Smallest vertical slice:** Track one pulsar, acquire a pulse train, calculate
an uncertainty-bounded time solution, and improve one expedition position fix.

**Acceptance gate:** Three pulsars produce a universal time estimate only after
signal quality, geometry, and clock uncertainty pass thresholds. The system
does not overwrite the simulation clock or claim nanosecond accuracy without an
authored error model; navigation receives a correction with provenance.

**UI:** `RadioTelescopePanel.cs` displays dish tracking, signal quality,
dispersion, timing uncertainty, and navigation correction.

### [494] Arctic Lichen Cryoprotectant Biochemistry & Cryogenic Preservation

**Core integration:** Add `LichenCryobankSystem` over `ResearchSystem`, medical
host workflows, `GenerationalSuccessionEngine`, consent/provenance, controlled
cooling, liquid-nitrogen inventory, sample viability, and thaw records.

**Smallest vertical slice:** Collect an approved lichen sample, extract and assay
the cryoprotectant, archive one human/animal/plant sample with consent, and
perform a simulated viability check.

**Acceptance gate:** The 1,000-sample target counts unique, provenance-complete
records across the three sample classes. Cooling rate, storage temperature,
LN2 consumption, contamination, privacy, consent, and thaw viability are saved;
the bank is not an automatic population-recovery guarantee.

**UI:** `LichenCryoprotectPanel.cs` shows assay, loading protocol, cooling,
storage, consent, viability, and genetic-diversity indexes.

### [495] Mechanical Telegraph Optical Semaphore Tower Network

**Core integration:** Add `SemaphoreNetworkSystem` over
`LocationLayoutSystem`, line-of-sight/weather, `RadioHostSession` as a
communications adapter, and `CrossingArbitrationSystem` for authenticated
messages. Model tower condition, shutters, codebook, relay queue, visibility,
and delay.

**Smallest vertical slice:** Register two visible towers, encode and relay one
message, then deliver it with measured shutter/relay delay.

**Acceptance gate:** A raid warning arrives in under two minutes only for a
validated route, trained operators, adequate visibility, and healthy towers.
The network is low-tech and line-of-sight; it is not instantaneous and does not
claim the speed of light as gameplay transport.

**UI:** `SemaphoreTowerPanel.cs` presents tower maps, code wheel, operator duty,
visibility, queue, and delivery status.

### [496] Woven Reed Papyrus & Handmade Vellum Parchment Production

**Core integration:** Add `PapyrusVellumSystem` over `RecipeCatalog`,
`JournalSystem`, `CraftingSystem`, reed/skin/lime goods, archival quality,
conservation, and the library document store.

**Smallest vertical slice:** Process reeds into one sheet and one skin into
vellum, write a journal record, assign durability, and place it in validated
archive storage.

**Acceptance gate:** Fibre/skin mass, lime, drying time, writing quality,
acid/temperature/humidity exposure, and preservation treatment are saved.
“Thousands of years” is a durability tier requiring conservation and storage;
the Constitution and journals are archived only when the records pass integrity
and provenance validation.

**UI:** `PapyrusVellumPanel.cs` handles reed splitting, lamination, vellum
stretching, quality, writing, and archival placement.

## Data, UI, and test deliverables

- Add `batch31_features.json`, recipes/goods entries, species and cave survey
  schemas, accessibility tables, ethics dilemmas, medical compound records,
  pulsar/semaphore catalogs, and archive-material definitions under
  `Assets/StreamingAssets/Data/`.
- Add `src/UI/BereavedCounsellorPanel.cs`, `BrailleEmbossPanel.cs`,
  `WatchmakingGuildPanel.cs`, `BirdBandingPanel.cs`,
  `CavernMushroomFarmPanel.cs`, `ChandleryPanel.cs`, `EthicsCouncilPanel.cs`,
  `SpeleologyPanel.cs`, `ElectroformingPanel.cs`, `HypocaustHeatingPanel.cs`,
  `TulsiMedicinePanel.cs`, `PeltonWheelPanel.cs`, `RadioTelescopePanel.cs`,
  `LichenCryoprotectPanel.cs`, `SemaphoreTowerPanel.cs`, and
  `PapyrusVellumPanel.cs` as thin presenters.
- Add Core tests for deterministic replay, state capture/restore, malformed
  data rejection, inventory conservation, consent/eligibility, environmental
  uncertainty, and idempotent completion. Add Godot tests for panel commands,
  navigation, disabled states, and responsive layout.
- Run the repository verification gates from `AGENTS.md`: Core build/test,
  `Ashfall.csproj` build, all four Godot headless self-tests, and
  `./scripts/ci/godot-asset-gate.sh`.

## Batch completion definition

Batch 31 is complete when all 16 feature contracts are catalog-validated,
deterministically replayable, round-trip safe, and reachable through Godot UI;
the Book of the Departed, accessibility records, ethics decisions, survey
evidence, research discoveries, semaphore messages, and archival manuscripts
are visible in the journal/history layer; and the full required verification
suite passes without legacy-engine dependencies.
