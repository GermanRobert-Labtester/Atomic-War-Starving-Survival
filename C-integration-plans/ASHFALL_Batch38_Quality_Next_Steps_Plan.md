# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 38)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 593–608

## Outcome

Batch 38 completes a social and scientific arc through midwifery, puppet arts,
companion animals, amber science, mechanical statistics, mushroom medicine,
postal exchange, sailing, local markets, natural history, geothermal
horticulture, folklore, near-space observation, natural construction, and a
museum.

The batch uses these existing authorities:

- The medical host boundary, `NeedsSystem`, `DiseaseSystem`, and
  `GenerationalSuccessionEngine` own birth, child, companion-animal, and
  population outcomes. `MedicalSystem` and `CenturySeed` are roadmap concepts,
  not new duplicate classes.
- `JournalSystem`, `ResearchSystem`, `CraftingSystem`, `DutyRosterSystem`, and
  the archive/library contract own cultural, statistical, scientific, and
  museum records.
- `CrossingArbitrationSystem`, `LocationLayoutSystem`, maritime
  `StealthDiveInstance`, `MarketSystem`, and `RadioHostSession` own diplomacy,
  postal routes, regatta logistics, exchange, and communications.
- `GreenhouseSystem`, recipe/goods catalogs, thermal infrastructure, and the
  shared power authority own food, medicine inputs, and greenhouse heat.

No panel may write an alliance, mortality result, stress modifier, market
balance, museum milestone, or epilogue directly.

## Batch entry gates

1. Add `Batch38State` and `batch38_save.json` with explicit migrations, normal
   checksummed persistence, `CaptureState`/`RestoreState`, and state-change
   events.
2. Persist consent, clinical evidence, animal welfare, scientific provenance,
   postal custody, vessel safety, credit backing, cultural authorship, and
   environmental conditions.
3. Replace absolute claims—zero maternal mortality, virtually no loneliness,
   ancient DNA, order-of-magnitude savings, permanent trade, and museum-wide
   wonder—with measured cohort, evidence, or authored milestone fixtures.
4. Use `ISeededRng` for birth complications, signal candidates, animal
   behaviour, amber finds, market demand, race performance, weather, and
   cultural events. Never use `System.Random` or `Guid.NewGuid()`.
5. Add snake_case, schema-versioned data under `Assets/StreamingAssets/Data/`;
   catalog validation must reject duplicate IDs, invalid references, unsafe
   ranges, and incomplete provenance.

## Delivery order

1. **593–595:** Establish birth-care, performance, and companion-animal
   welfare foundations.
2. **596–599:** Add amber science, jewellery, statistics, and mushroom clinical
   research with evidence records.
3. **600–602:** Build postal, regatta, and local-market contracts.
4. **603–607:** Add the natural-history atlas, geothermal crops, folklore,
   balloon science, and thatched construction.
5. **608:** Curate the museum from validated specimens, records, instruments,
   and exhibits, then issue the cultural milestone.

## Shared implementation contract

`Batch38State` owns feature registration, cross-feature references, schema
version, and migrations. Each feature receives a Core command/state boundary,
thin Godot presentation, catalog data, deterministic replay and round-trip
tests, invalid-input/failure tests, and a user-facing acceptance fixture.
Resource flows use atomic inventory transactions; research discoveries carry
source and confidence; cultural modifiers are bounded and idempotent.

## Step integration matrix

### [593] Post-War Midwifery Guild & Dedicated Obstetric Birthing Suite

**Core integration:** Add `MidwiferySystem` over the medical host,
`GenerationalSuccessionEngine`, `NeedsSystem`, pregnancy/birth records, midwife
training, room/equipment capacity, neonatal resuscitation, consent, and
postpartum follow-up.

**Smallest vertical slice:** Train one birth attendant, prepare one suite,
record a consented labour with a partograph, deliver one birth, and complete
maternal/neonatal follow-up.

**Acceptance gate:** Twenty consecutive births with zero maternal deaths is an
authored safe-care fixture requiring staffing, equipment, blood, risk profile,
referrals, and correct monitoring. Complications, prematurity, haemorrhage,
infection, and consent changes remain modeled; the system cannot guarantee zero
mortality for every future birth.

**UI:** `MidwiferyBirthingPanel.cs` presents room setup, competency, labour
chart, supplies, resuscitation, and follow-up records.

### [594] Hand-Carved Wooden Puppet Theatre & Shadow Play Tradition

**Core integration:** Add `PuppetTheatreSystem` over `NeedsSystem`,
`JournalSystem`, `DutyRosterSystem`, `CraftingSystem`, child safeguarding,
script/puppet provenance, venue capacity, and cultural-event state.

**Smallest vertical slice:** Carve one puppet, author a safe short play, assign
performers, stage it, record audience attendance, and archive the script.

**Acceptance gate:** The Puppet Arts tradition is established by a completed,
safe performance with authored material, performers, venue, and audience. Child
participation requires guardianship/safeguarding; morale is a bounded event
effect and not a universal “massive” value.

**UI:** `PuppetTheatrePanel.cs` handles carving, jointing, costume, scripts,
performers, safety, audience, and journal entry.

### [595] Colony Companion Animal Programme & Pet Domestication Registry

**Core integration:** Add `CompanionAnimalSystem` over `NeedsSystem`,
`DutyRosterSystem`, animal health/veterinary care, shelter capacity, pet identity,
vaccination, behaviour, welfare, and animal-assisted visit scheduling.

**Smallest vertical slice:** Register one suitable animal, complete health and
vaccination checks, assign a guardian, and conduct one supervised therapy visit.

**Acceptance gate:** Fifty pets require valid registrations, welfare/capacity,
health records, guardian duties, and care supplies. A -20% average stress result
is a measured cohort effect; chronic loneliness is not “virtually eliminated”
without a defined population, baseline, duration, and confidence interval.

**UI:** `PetRegistryPanel.cs` displays identity, health, guardian, welfare,
visits, and stress-response evidence.

### [596] Baltic Amber Fossil Resin Excavation & Palaeontology Study

**Core integration:** Add `AmberPalaeontologySystem` over `ExpeditionSystem`,
`ResearchSystem`, `JournalSystem`, coastal geology, excavation provenance,
sample custody, polishing, microscopy, and evidence review.

**Smallest vertical slice:** Survey an amber bed, recover one nodule without
losing context, polish and inspect it, and submit an inclusion record.

**Acceptance gate:** An ancient insect unlocks the ancient-DNA research tree only
after specimen authenticity, age/context, preservation, contamination controls,
and research review pass. The result is a research opportunity, not automatic
recoverable ancient DNA or resurrection.

**UI:** `AmberPalaeontologyPanel.cs` shows survey/sieve, nodule custody,
polishing, microscope view, specimen metadata, and research review.

### [597] Artisan Baltic Amber & Gemstone Jewellery Workshop

**Core integration:** Add `GemstoneJewellerySystem` over `CraftingSystem`,
`NeedsSystem`, `CrossingArbitrationSystem`, trade goods, lapidary quality,
precious-metal inventory, provenance, valuation, and diplomatic gift events.

**Smallest vertical slice:** Cut and polish one amber, fabricate a silver bezel,
assemble a necklace, grade it, and offer it through a treaty negotiation.

**Acceptance gate:** An alliance outcome requires both the item’s quality/value
and a valid arbitration result with clauses, signatures, and faction trust. The
necklace cannot replace 20 tonnes of grain by a hard-coded barter conversion;
market valuation and negotiation leverage are data-driven and uncertain.

**UI:** `GemstoneJewelleryPanel.cs` presents faceting, bezel fabrication,
polishing, quality, provenance, value, and gift negotiation.

### [598] Mechanical Burroughs-Style Adding Machine & Statistical Bureau

**Core integration:** Add `StatisticalBureauSystem` over `ResearchSystem`,
`DutyRosterSystem`, `CrossingArbitrationSystem`, population/economy/resource
ledgers, deterministic arithmetic, report provenance, and governance review.

**Smallest vertical slice:** Tabulate one month of resource data, audit the
totals, produce a regression/efficiency report, and submit one approved action.

**Acceptance gate:** The first report identifies three inefficiencies only when
the source ledgers, formulas, period, and audit trail are valid. A 15% annual
expenditure saving requires an implemented action and before/after comparison;
the adding machine cannot create savings by report text alone.

**UI:** `StatisticalBureauPanel.cs` displays machine operation, source ledgers,
tables, formulas, report confidence, and approved actions.

### [599] Medicinal Mushroom Immunotherapy Clinic & Beta-Glucan Therapy

**Core integration:** Add `MushroomImmunotherapySystem` over the medical host,
`GreenhouseSystem`, `ResearchSystem`, recipe/compound catalog, contamination,
purity, dose, patient eligibility, radiation/immune state, and follow-up.

**Smallest vertical slice:** Grow/obtain one species, extract and assay
beta-glucan, formulate a controlled preparation, and monitor one eligible
patient through treatment.

**Acceptance gate:** Thirty patients count only with baseline immune markers,
purity/dose, consent, protocol, response, adverse events, and follow-up.
“Normal immune function” is an evidence threshold for a configured cohort;
immunotherapy cannot eliminate all infection vulnerability or replace urgent
clinical care.

**UI:** `MushroomImmunotherapyPanel.cs` shows source species, extraction,
chromatography purity, protocol, patient eligibility, and response.

### [600] Colony Postal Service, Letter Stamp Press & Exchange Network

**Core integration:** Add `PostalServiceSystem` over
`CrossingArbitrationSystem`, `DutyRosterSystem`, `LocationLayoutSystem`, postal
routes, stamps/goods, rider/relay duties, message custody, privacy, and
inter-settlement relationship events.

**Smallest vertical slice:** Engrave one stamp, register a letter, route it to a
second settlement, deliver it, and record relationship/official correspondence
effects.

**Acceptance gate:** Alliance bonuses require a delivered, authenticated
exchange and a faction event outcome. Lost mail, route hazards, censorship/
privacy rules, weather, and rider capacity are modeled; a sorting-office launch
does not imply delivery.

**UI:** `PostalServicePanel.cs` presents stamp die, sorting, route, custody,
delivery, correspondence, and relationship history.

### [601] Competitive Long-Distance Sailing Regatta & Cup Race Event

**Core integration:** Add `SailingRegattaSystem` over `StealthDiveInstance`,
`NeedsSystem`, `LocationLayoutSystem`, `WeatherSystem`, vessel condition,
crew roles, safety, buoy waypoints, allied invitations, and cultural/faction
events.

**Smallest vertical slice:** Register two vessels, validate a course, simulate
one weather leg, update positions, and complete a safe finish or rescue.

**Acceptance gate:** Five allied vessels and a colony win require authenticated
entries, seaworthy vessels, crew, weather window, course completion, and race
rules. Wind, damage, navigation error, fatigue, and rescue can change results;
the regatta cannot force the colony barque to win.

**UI:** `SailingRegattaPanel.cs` displays course buoys, wind angle, vessel state,
crew, rankings, safety, and post-race relations.

### [602] Cooperative Colony Credit & Barter Exchange Marketplace

**Core integration:** Add `LocalExchangeMarketSystem` over
`CrossingArbitrationSystem`, `NeedsSystem`, `DutyRosterSystem`, `MarketSystem`,
`GoodsCatalog`, credit backing, offers, wants, skills, settlement, fraud, and
price discovery.

**Smallest vertical slice:** Create one backed credit voucher, post an offer and
request, match them, settle goods/services, and record both parties’ balances.

**Acceptance gate:** One hundred first-month transactions require unique
participants, valid inventory/service claims, backed credit, settlement, and
fraud/expiry checks. Economic activity cannot create goods from listings or
grant an unbacked currency balance.

**UI:** `BarterMarketPanel.cs` shows listings, requests, vouchers, matching,
settlement, prices, disputes, and activity metrics.

### [603] Hand-Illustrated Illuminated Scientific Atlas & Natural History

**Core integration:** Add `ScientificAtlasSystem` over `JournalSystem`,
`ResearchSystem`, `NeedsSystem`, specimen/observation catalogs, artist/naturalist
provenance, plate production, taxonomy, and archive storage.

**Smallest vertical slice:** Create one validated specimen observation, paint and
annotate one plate, cite its source, and publish it to the atlas.

**Acceptance gate:** Five hundred plates require unique observations, taxonomy,
artist/naturalist attribution, source confidence, conservation, and archive
placement. “Most treasured” is a library/cultural ranking based on recorded
visits and significance, not a hard-coded title.

**UI:** `IlluminatedAtlasPanel.cs` presents specimen plates, taxonomy, annotations,
gold leaf, citations, review, and publication.

### [604] Geothermal Greenhouse Heating & Year-Round Tropical Crops

**Core integration:** Add `GeothermalGreenhouseHeatingSystem` over
`GreenhouseSystem`, thermal/power authority, `NeedsSystem`, geothermal flow,
heat exchangers, soil/air temperatures, crop varieties, water, and maintenance.

**Smallest vertical slice:** Connect one heat loop, maintain a bounded winter
temperature, grow one tropical crop, and settle water/nutrient/heat demand.

**Acceptance gate:** Banana and citrus production requires suitable temperature,
light, water, nutrients, pollination, disease control, geothermal capacity, and
pipe integrity. Nuclear winter operation is a scenario result; tropical crops
cannot be grown from heat alone or without resource cost.

**UI:** `GeothermalGreenhousePanel.cs` shows heat flow, exchanger condition,
soil/air probes, crop requirements, power/water, and harvest.

### [605] Post-War Folklore Collection & Oral Mythology Preservation

**Core integration:** Add `FolkloreCollectionSystem` over `JournalSystem`,
`NeedsSystem`, `GenerationalSuccessionEngine`, oral consent, narrator identity,
transcription, dialect/variant records, classification, illustration, and
publication.

**Smallest vertical slice:** Record one consented tale, transcribe it, preserve
variant/source metadata, illustrate or annotate it, and publish one pamphlet.

**Acceptance gate:** Fifty tales require distinct source records, consent,
transcription, attribution, variant handling, and archive placement. The
Mythology Canon is a reviewed cultural collection; it cannot erase contested
versions or grant identity strength from unverified text alone.

**UI:** `FolkloreCollectionPanel.cs` presents interviews, transcription,
variants, themes, attribution, illustration, and publication.

### [606] High-Altitude Balloon Launched Micro-Satellite & Ionosphere Study

**Core integration:** Add `StratosphericBalloonSystem` over
`RadioHostSession`, `ResearchSystem`, `WeatherSystem`, balloon/payload goods,
telemetry, altitude/temperature/pressure, cosmic-ray/UV instruments, recovery,
and antenna design research.

**Smallest vertical slice:** Launch one payload, maintain a telemetry link,
collect an ionospheric profile, recover or lose the package, and submit data for
research review.

**Acceptance gate:** A 35 km payload requires lift calculation, pressure/thermal
survival, instrument calibration, signal coverage, flight path, and data
quality. It is a balloon platform, not a satellite; superior antenna research
unlocks only after validated profiles and repeatability.

**UI:** `StratosBalloonPanel.cs` shows lift/helium, payload, telemetry,
altitude/profile, recovery, and research quality.

### [607] Traditional Thatched Roof Construction & Biomass Insulation

**Core integration:** Add `ThatchedRoofSystem` over
`LocationLayoutSystem`, `NeedsSystem`, `DutyRosterSystem`, reed/straw goods,
weather exposure, fire safety, roof geometry, maintenance, and thermal state.

**Smallest vertical slice:** Harvest and bundle thatch, install one roof section,
inspect ridge/weatherproofing, and measure thermal/fire effects.

**Acceptance gate:** A 30-year roof life is a material/maintenance tier requiring
dryness, pitch, ridge quality, pests, firebreaks, and weather. A -50% heating
effect is scoped to eligible exterior structures and measured against a baseline;
thatch cannot be treated as fireproof or maintenance-free.

**UI:** `ThatchedRoofPanel.cs` presents materials, bundle pressing, fixing,
ridge cap, weather/fire inspection, thermal effect, and maintenance.

### [608] Grand Colony Museum of Natural History & Science Exhibits

**Core integration:** Add `NaturalHistoryMuseumSystem` over `JournalSystem`,
`ResearchSystem`, `LocationLayoutSystem`, `DutyRosterSystem`, archive/specimen
records, exhibit provenance, building capacity, curation, accessibility, and
visitor events.

**Smallest vertical slice:** Curate one specimen/instrument, write its label and
source, install it in a case, and admit a visitor cohort.

**Acceptance gate:** Twenty exhibits require valid specimens, provenance,
condition, labels, cases, safety, accessibility, and curator duty. The inaugural
visit and “Curiosity and Wonder” trait are one-time, scoped cultural events;
visitor attendance and educational impact are recorded rather than assumed.

**UI:** `NaturalHistoryMuseumPanel.cs` handles cases, labels, provenance,
collection wings, accessibility, visitor flow, and cultural outcome.

## Data, UI, and test deliverables

- Add `batch38_features.json`, midwifery protocols, theatre/cultural events,
  pet registry and welfare data, amber geology/specimen types, jewellery grades,
  statistical formulas, mushroom compounds, postal routes, regatta rules,
  market/credit policies, atlas plate schemas, greenhouse thermal profiles,
  folklore consent/variants, balloon payloads, thatch specifications, and
  museum exhibit definitions under `Assets/StreamingAssets/Data/`.
- Add thin presenters: `MidwiferyBirthingPanel.cs`, `PuppetTheatrePanel.cs`,
  `PetRegistryPanel.cs`, `AmberPalaeontologyPanel.cs`,
  `GemstoneJewelleryPanel.cs`, `StatisticalBureauPanel.cs`,
  `MushroomImmunotherapyPanel.cs`, `PostalServicePanel.cs`,
  `SailingRegattaPanel.cs`, `BarterMarketPanel.cs`,
  `IlluminatedAtlasPanel.cs`, `GeothermalGreenhousePanel.cs`,
  `FolkloreCollectionPanel.cs`, `StratosBalloonPanel.cs`,
  `ThatchedRoofPanel.cs`, and `NaturalHistoryMuseumPanel.cs`.
- Test clinical/animal consent, signal and specimen provenance, market backing,
  route custody, race safety, thermal/resource balances, cultural attribution,
  exhibit idempotence, deterministic replay, and state round-trip. Add Godot
  command-path, disabled-state, navigation, and layout tests.
- Run the Core build/test, Godot project build, four headless self-tests, and
  `./scripts/ci/godot-asset-gate.sh` required by `AGENTS.md`.

## Batch completion definition

Batch 38 is complete when all 16 systems are catalog-validated,
deterministically replayable, round-trip safe, and reachable through Godot UI;
birth, welfare, science, trade, postal, maritime, food, folklore, and museum
records are auditable; and the museum milestone is generated from real
specimens, exhibits, and visitor history.
