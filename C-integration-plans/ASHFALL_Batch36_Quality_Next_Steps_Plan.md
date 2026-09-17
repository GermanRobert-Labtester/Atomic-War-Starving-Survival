# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 36)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 561–576

## Outcome

Batch 36 adds animal medicine, passive astronomy, guild progression, ceramics,
mass-casualty readiness, photography, constitutional governance, sanitary waste
management, typography, evidence-based apitherapy, wildlife restoration,
tapestry, solar mineral processing, EMP resilience, tidal energy, and a unified
energy plan.

Use the repository’s real boundaries:

- The medical host, `CombatTraumaSystem`, `NeedsSystem`, disease, and animal-care
  contracts own clinical and veterinary outcomes; `MedicalSystem` in the source
  roadmap is a conceptual boundary, not a class to duplicate.
- `DutyRosterSystem`, `GenerationalSuccessionEngine`, `ResearchSystem`,
  `CraftingSystem`, `SilentFoundrySystem`, and `JournalSystem` own guilds,
  production, education, and cultural history.
- `CrossingArbitrationSystem`, `FinalWishSystem`, and typed `IEventBus`
  contracts own governance decisions and treaty consequences.
- `WeatherSystem`, `LocationLayoutSystem`, `ShelterHazardLoop`, maritime state,
  and the shared power authority own environmental risk and energy.

`District8Accords`, `CenturySeed`, and `PowerGridPanel` are roadmap concepts
adapted through validated agreement IDs, `GenerationalSuccessionEngine`, and
typed power transactions. Panels remain presenters only.

## Batch entry gates

1. Add `Batch36State` and `batch36_save.json` with explicit migrations,
   `CaptureState`/`RestoreState`, state-change events, and normal checksummed
   save registration.
2. Separate human, animal, ecological, clinical, and governance eligibility
   from UI visibility. Consent, welfare, medical evidence, referendum authority,
   and waste/environmental monitoring must be persisted.
3. Convert absolute claims—zero preventable deaths, zero contamination,
   self-sustaining ecosystems, total requirements, and permanent energy
   independence—into measured scenario fixtures with failure paths.
4. Use `ISeededRng` for wildlife, signal candidates, kiln defects, clinical
   response, weather, and production variation; never use `System.Random` or
   `Guid.NewGuid()` for simulation outcomes.
5. Add snake_case, schema-versioned catalog data under
   `Assets/StreamingAssets/Data/` and run reference/range/uniqueness validation
   before exposing controls.

## Delivery order

1. **561–563:** Establish animal health, astronomy evidence, and guild/rank
   progression.
2. **564–568:** Build ceramic, medical surge, archival image, governance, and
   sanitary-waste foundations.
3. **569–572:** Extend typography, clinical natural compounds, wildlife
   restoration, and textile culture.
4. **573–575:** Add solar mineral processing, EMP resilience, and tidal power.
5. **576:** Publish the energy atlas from authoritative generation, storage,
   demand, and reserve snapshots.

## Shared implementation contract

`Batch36State` owns feature IDs, cross-batch references, schema version, and
migration hooks. Each feature receives a Core command/state boundary, catalog
data, a thin `src/UI/` panel, deterministic replay and round-trip tests,
resource conservation tests, and an acceptance fixture. No panel writes a
medical result, faction stance, power value, contamination certificate, or
ending flag directly.

## Step integration matrix

### [561] Veterinary Surgery Suite & Colony Animal Hospital

**Core integration:** Add `VeterinaryCareSystem` over animal identity/health,
`DutyRosterSystem`, `NeedsSystem`, disease/vaccination, livestock breeding,
inventory, and the medical sterilization boundary. Keep animal cases separate
from human patient records while sharing typed treatment primitives.

**Smallest vertical slice:** Admit one injured breeding animal, triage it,
sterilize instruments, perform a permitted procedure, vaccinate it, and update
health and breeding eligibility.

**Acceptance gate:** Ten animals are counted as saved only with a before/after
health record, treatment resource settlement, welfare checks, and follow-up.
Mortality reduction is a cohort statistic; surgery cannot guarantee survival or
erase genetic, anaesthesia, infection, or recovery risk.

**UI:** `VeterinarySuitePanel.cs` shows animal records, triage, dosage,
sterilization, procedure progress, vaccination, and follow-up.

### [562] Amateur Radio Astronomy & Passive SETI Listening Programme

**Core integration:** Add `PassiveSignalSearchSystem` over `RadioHostSession`,
`ResearchSystem`, `JournalSystem`, dish pointing, FFT observations, candidate
provenance, verification, and contact-event arbitration.

**Smallest vertical slice:** Point at one catalogued sky window, record a
narrowband candidate, repeat it under a second observation, and submit it for
verification.

**Acceptance gate:** A repeating signal triggers “Contact” only after frequency,
timing, sky position, instrument calibration, interference checks, and
independent repetition pass the evidence threshold. A candidate can be noise,
local interference, or a natural source; opening the listener does not create a
civilisation.

**UI:** `SETIListeningPanel.cs` presents pointing, waterfall, candidate queue,
verification evidence, and event-chain status.

### [563] Colony Trade Guild Apprenticeship System & Journeyman Ranks

**Core integration:** Add `TradeGuildSystem` over `DutyRosterSystem`,
`GenerationalSuccessionEngine`, `ResearchSystem`, `CraftingSystem`, skill
progression, guild charters, mentor capacity, assessments, and certificates.

**Smallest vertical slice:** Found one guild, enroll an apprentice, complete a
mentor work order, submit a masterpiece, and award a validated Journeyman rank.

**Acceptance gate:** Six Guild Masters require six trade-specific curricula,
attendance, safety, mastery assessments, and mentor capacity. Tier-5 quality
unlocks only for certified trades and cannot be granted by a ceremony button or
stacked without a named quality modifier.

**UI:** `TradeGuildPanel.cs` displays charters, mentor assignments, rank ladder,
work orders, assessments, and certificates.

### [564] Artisan Terracotta Tile Kiln & Encaustic Floor Tile Manufacture

**Core integration:** Add `TerracottaTileSystem` over `SilentFoundrySystem`,
`DutyRosterSystem`, `LocationLayoutSystem`, clay/slip/fuel goods, kiln schedules,
pattern design, installation, inspection, and beauty state.

**Smallest vertical slice:** Press, fire, inspect, and install one tile in a
validated corridor pattern, then update coverage and maintenance.

**Acceptance gate:** Sanctuary-wide completion requires tile counts, material
mass, firing quality, installation coverage, slip defects, and safe labour.
Maximum beauty is a scoped location score after inspection, not an unconditional
global aesthetic value.

**UI:** `TerracottaTileKilnPanel.cs` handles moulds, slip patterns, pyrometer
ramps, defects, installation, and corridor coverage.

### [565] Mass Casualty Surge Capacity & Field Hospital Tent City

**Core integration:** Add `MassCasualtySurgeSystem` over the medical host,
`CombatTraumaSystem`, `DutyRosterSystem`, `ShelterHazardLoop`, cots, blood,
sterile supplies, triage, operating capacity, and evacuation/aftercare.

**Smallest vertical slice:** Train one paramedic cohort, deploy a tent module,
triage a synthetic casualty wave, consume supplies, and hand off survivors to
definitive care.

**Acceptance gate:** Two hundred cots and 50 paramedics are capacity records,
not automatic readiness. Treating 150 casualties with zero preventable deaths is
an authored disaster fixture requiring staffing, stock, triage timing, and
available surgery; avoidable death, overload, infection, and blood shortage are
still possible in other events.

**UI:** `MassCasualtyHospitalPanel.cs` shows deployment, cots, staff, blood,
triage queues, operating rooms, and aftercare outcomes.

### [566] Mechanical Pinhole Camera Obscura Studio & Portrait Sessions

**Core integration:** Add `CameraObscuraSystem` over `JournalSystem`,
`DutyRosterSystem`, `CraftingSystem`, plate/emulsion goods, subject consent,
exposure, development, image provenance, and archive storage.

**Smallest vertical slice:** Schedule a consented portrait, expose one plate,
develop it, grade the image, and archive the negative and print together.

**Acceptance gate:** The founders’ portrait archive requires a complete subject
roster, consent, image identity, process metadata, and storage. Platinum-palladium
quality is a material/processing tier; “millennia” is an archival durability
claim requiring conservation, not a guaranteed lifespan.

**UI:** `CameraObscuraPanel.cs` covers focus, emulsion, exposure, contact print,
subject consent, image grading, and archive placement.

### [567] Constitutional Monarchy Election & Crowned Sovereign Charter

**Core integration:** Add `GovernanceReferendumSystem` over
`CrossingArbitrationSystem`, the existing constitution assembly boundary,
`FinalWishSystem`, `IEventBus`, electorate eligibility, ballot secrecy, quorum,
charter clauses, and succession rules.

**Smallest vertical slice:** Publish both governance options, collect a valid
referendum, validate quorum, record the result, and emit the corresponding
governance transition event.

**Acceptance gate:** The result is determined from eligible ballots, audit data,
quorum, and tie/invalid-ballot rules. The monarchy/republic branch changes
future governance event availability only after ratification; a coronation
ceremony cannot overwrite constitution or faction state directly.

**UI:** `MonarchyReferendumPanel.cs` displays debate material, eligibility,
ballots, audit, quorum, result, charter, and transition consequences.

### [568] Geosynthetic Clay Liner Sanitary Landfill & Leachate Treatment

**Core integration:** Add `SanitaryLandfillSystem` over
`LocationLayoutSystem`, `WeatherSystem`, water/groundwater contamination,
leachate collection, liner inspection, landfill gas, waste streams, and hazard
monitoring. Agreement data can supply materials but does not own landfill state.

**Smallest vertical slice:** Construct one lined cell, route leachate to
treatment, monitor groundwater, and place one approved waste stream.

**Acceptance gate:** “Zero groundwater contamination” is a certificate issued
only after liner integrity, monitoring wells, rainfall, leachate treatment, and
sampling pass. Liner damage, overflow, gas, illegal waste, and extreme weather
remain failure paths; uncontrolled dumping is reduced by verified coverage, not
erased by a toggle.

**UI:** `SanitaryLandfillPanel.cs` presents cells, liner layers, pipes, leachate,
gas, groundwater samples, capacity, and maintenance.

### [569] Hand-Stamped Letterpress Typography & Typeface Design

**Core integration:** Add `LetterpressTypographySystem` over `JournalSystem`,
`DutyRosterSystem`, `CraftingSystem`, type inventory, alloy casting, composing,
document provenance, and constitutional/archive records.

**Smallest vertical slice:** Cut one glyph set, cast type, compose a page,
print it, inspect alignment/ink, and attach the page to a document edition.

**Acceptance gate:** The Constitution edition requires complete page coverage,
type inventory, print quality, authorizing record, and archival placement. A
prestige milestone is a one-time document event and cannot alter constitutional
content or fabricate an edition without materials.

**UI:** `LetterpressTypographyPanel.cs` shows punch/counter-punch work, casting,
composition, press quality, edition metadata, and archive status.

### [570] Apitherapy Bee Venom Acupuncture & Anti-Inflammatory Clinic

**Core integration:** Add `ApitherapySystem` over the medical host,
`NeedsSystem`, `GreenhouseSystem`/apiary, treatment evidence, allergy risk,
venom dose, consent, and follow-up.

**Smallest vertical slice:** Screen one patient, obtain consent, administer a
controlled protocol or purified preparation, monitor response, and record an
adverse-event path.

**Acceptance gate:** Twenty elders count only when eligible cases show a
documented improvement under the configured evidence tier. Allergy, anaphylaxis,
infection, dose error, and contraindications are modeled; the clinic cannot
promise universal arthritis or radiation-inflammation relief.

**UI:** `ApitherapyClinicPanel.cs` presents eligibility, points, concentration,
consent, monitoring, response, and adverse events.

### [571] Post-War Species Reintroduction Programme & Wildlife Corridors

**Core integration:** Add `SpeciesReintroductionSystem` over
`LocationLayoutSystem`, `WeatherSystem`, habitat/food/water, animal welfare,
radio collars, predator-prey ecology, and research observations.

**Smallest vertical slice:** Assess one corridor, release one approved animal,
track it, record survival and prey indicators, and update habitat status.

**Acceptance gate:** Self-sustaining wolf/elk populations require viable
population size, food, habitat, disease controls, corridor connectivity, and
multi-season evidence. Reintroduction may fail, cause conflict, or require
recapture; “at no cost” is not a valid ecological assumption.

**UI:** `SpeciesReintroductionPanel.cs` shows sites, welfare checks, collar
tracking, population balance, conflict, and recovery confidence.

### [572] Mechanical Difference Loom & Complex Tapestry Weaving

**Core integration:** Add `TapestryLoomSystem` over `CraftingSystem`,
`DutyRosterSystem`, `NeedsSystem`, wool/dye goods, Jacquard card programs,
pattern provenance, and cultural exhibition.

**Smallest vertical slice:** Program one pattern, warp a bounded section, weave,
inspect, and archive a narrative panel.

**Acceptance gate:** A ten-metre tapestry requires thread mass, dye, loom
capacity, worker time, pattern completeness, and quality inspection. Its
“celebrated artwork” status is a recorded cultural ranking, not a guaranteed
global maximum.

**UI:** `TapestryLoomPanel.cs` displays warp cards, preview, materials, dye,
progress, defects, and exhibition ranking.

### [573] Solar Salt Evaporation Pans & Chemical Mineral Harvesting

**Core integration:** Add `SolarMineralPanSystem` over brine/water chemistry,
`WeatherSystem`, `LocationLayoutSystem`, goods, crystallization assays, and
research review. The roadmap’s accord reference is represented by validated
trade/input agreements only.

**Smallest vertical slice:** Fill one pan, measure evaporation and salinity,
harvest one fraction, assay it, and separate the next fraction if purity passes.

**Acceptance gate:** Lithium, potassium, and magnesium outputs require brine
composition, evaporation area, weather, contamination control, labour, and
separation yields. Solar heat reduces energy cost but does not make the process
free or guarantee the colony’s entire chemical demand.

**UI:** `SolarEvaporationPanPanel.cs` shows pan conditions, salinity stages,
crystallization, assay purity, harvest, and stockpiles.

### [574] Emergency EMP-Hardened Communications Faraday Vault

**Core integration:** Add `FaradayVaultSystem` over `RadioHostSession`, shared
power, `LocationLayoutSystem`, EMP hazard, shielding materials, penetrations,
surge protection, equipment inventory, and restoration logs.

**Smallest vertical slice:** Build one vault wall, install a cable penetration,
test shielding with a calibrated pulse, and restore one protected radio.

**Acceptance gate:** MIL-STD-461-style protection is a configured test profile
requiring measured dB attenuation, seams, filters, grounding, equipment state,
and test frequency. A failed penetration or unprotected accessory can still
damage equipment; the vault cannot guarantee protection from every EMP.

**UI:** `FaradayVaultPanel.cs` presents mesh coverage, penetrations, filters,
grounding, test procedure, protected equipment, and restoration time.

### [575] Tidal Power Barrage & Reversible Turbine Generator Array

**Core integration:** Add `TidalBarrageSystem` over the shared power authority,
`StealthDiveInstance`, `LocationLayoutSystem`, coastal hydrology, tides, marine
ecology, sluice/turbine condition, and grid demand.

**Smallest vertical slice:** Survey an estuary, commission one sluice/turbine,
run flood and ebb cycles, and settle generated power after head/flow/efficiency.

**Acceptance gate:** Two MW is a rated capacity requiring tide range, basin
volume, flow, turbine availability, ecological flow, and grid connection. The
system has outage, silt, storm, fish-passage, corrosion, and maintenance states;
it does not generate forever or erase demand variability.

**UI:** `TidalBarragePanel.cs` shows tide phase, gates, flow direction, turbine
condition, environmental constraints, and actual grid output.

### [576] Grand Unified Colony Energy Atlas & Civilizational Energy Plan

**Core integration:** Add `EnergyAtlasSystem` over the shared power authority,
`ResearchSystem`, `LocationLayoutSystem`, `JournalSystem`, all generation and
storage contracts, demand forecasts, fuel reserve, and history snapshots.

**Smallest vertical slice:** Import current generation/load/storage records,
project one season, identify one reserve gap, and publish a versioned atlas.

**Acceptance gate:** Renewable independence is certified only when every load,
reserve margin, storage duration, seasonal profile, maintenance outage, and
fuel fallback is accounted for. A 50-year plan is an authored projection with
uncertainty, not a permanent elimination of fuel scarcity.

**UI:** `EnergyAtlasPanel.cs` displays source stack, demand curves, storage,
reserve gaps, maintenance scenarios, and certification evidence.

## Data, UI, and test deliverables

- Add `batch36_features.json`, veterinary protocols, SETI candidate classes,
  guild curricula, tile/ceramic recipes, mass-casualty capacities, photographic
  media, governance ballot schemas, landfill thresholds, typography data,
  apitherapy evidence, wildlife corridors, tapestry patterns, brine fractions,
  EMP test profiles, tidal parameters, and energy-atlas validators under
  `Assets/StreamingAssets/Data/`.
- Add thin presenters: `VeterinarySuitePanel.cs`, `SETIListeningPanel.cs`,
  `TradeGuildPanel.cs`, `TerracottaTileKilnPanel.cs`,
  `MassCasualtyHospitalPanel.cs`, `CameraObscuraPanel.cs`,
  `MonarchyReferendumPanel.cs`, `SanitaryLandfillPanel.cs`,
  `LetterpressTypographyPanel.cs`, `ApitherapyClinicPanel.cs`,
  `SpeciesReintroductionPanel.cs`, `TapestryLoomPanel.cs`,
  `SolarEvaporationPanPanel.cs`, `FaradayVaultPanel.cs`,
  `TidalBarragePanel.cs`, and `EnergyAtlasPanel.cs`.
- Test clinical/animal consent, signal false positives, guild certification,
  resource/quality conservation, ballot audit, groundwater contamination,
  ecological uncertainty, EMP failures, tidal cycles, and atlas projection
  replay. Add Godot command-path, disabled-state, navigation, and layout tests.
- Run the Core build/test, Godot project build, four headless self-tests, and
  `./scripts/ci/godot-asset-gate.sh` required by `AGENTS.md`.

## Batch completion definition

Batch 36 is complete when all 16 systems are catalog-validated,
deterministically replayable, round-trip safe, and reachable through Godot UI;
clinical, environmental, governance, security, power, and cultural records are
auditable; and the Energy Atlas is generated from authoritative state rather
than hard-coded capacity claims.
