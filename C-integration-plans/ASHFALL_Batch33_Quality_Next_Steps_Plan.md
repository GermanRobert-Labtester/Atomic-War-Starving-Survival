# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 33)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 513–528

## Outcome

Batch 33 expands communal culture, maritime construction, linguistic heritage, aerial survey, cooperative finance, food traditions, leather, textile arts, astronomy, paleontology, mutual aid, thermal design, athletics, and stained glass.

The batch should reuse existing Core contracts rather than turn every cultural panel into an isolated global morale switch:

- **JournalSystem**, SoundManager, cultural-event state, and **GenerationalSuccessionEngine** own authored works, performances, oral records, and long-lived identity.
- **StealthDiveInstance**, the surface-vessel extension, **LocationLayoutSystem**, and **CrossingArbitrationSystem** own maritime routes, coastal construction, diplomacy, and trade.
- **GreenhouseSystem**, recipe data, **GoodsCatalog**, and **CraftingSystem** own crops, dyes, tannery inputs, food, pearls, and textiles.
- **SilentFoundrySystem** owns leather tooling support, ironwork, glass components, and other production jobs.
- **MarketSystem**, the credit/ledger contract, constitution/governance state, and **SaveChecksum** own cooperative finance and accountable transfers.
- **ExpeditionSystem**, **WeatherSystem**, ResearchSystem, and the shared survey contract own kite mapping, fossils, orrery observations, and hydrographic records.
- **NeedsSystem**, **DutyRosterSystem**, and CaregivingSystem consume bounded social, comfort, health, and event effects.

The supplied IEventBus reference is an integration seam, not permission to make a second event architecture. Use typed state-change events and the host’s existing save orchestration.

## Batch entry gates

1. **Culture and consent:** define authored work, performers, audience, oral-history ownership, language-community consent, privacy, translation, and generational transfer.
2. **Maritime vessel gate:** reuse the surface-vessel contract from earlier batches for ferro-cement hulls, wind, crew, cargo, coastal weather, maintenance, and tactical risk.
3. **Survey gate:** extend kite aerial photography and photogrammetry with camera calibration, overlap, wind, scale, terrain, uncertainty, and 2D map tiles.
4. **Cooperative finance gate:** define member accounts, deposits, loans, collateral, interest, governance, defaults, dividends, audit, and atomic ledger settlement.
5. **Food/craft gate:** define mead, leather, dyes, pearls, textiles, tanning, pH/redox, contamination, labor, and exact material conversion.
6. **Governance and mutual-aid gate:** connect credit union and relief fund to the Living Constitution/legal rules; no hidden discretionary transfers.
7. **Scientific instrument gate:** define orrery/calendar accuracy, fossil/sample chain, and hydrographic evidence with confidence.
8. **Infrastructure/thermal gate:** establish shared thermal and water utility contracts before Trombe walls or artesian fountains.
9. **Education/athletics gate:** define participant eligibility, consent, injury, fatigue, age, safety, and bounded benefits.
10. **Visual 2D gate:** theatre, stained glass, survey mosaics, and architecture use Godot 2D scenes/read models; no unsupported 3D dependency.

## Delivery order

1. **513, 515:** stabilize live performance and linguistic/archive contracts.
2. **514, 516:** extend surface maritime and aerial survey state.
3. **517, 524:** add cooperative finance and mutual-aid governance.
4. **518, 519, 520, 521:** add feast, leather, dye, and pearl production/trade.
5. **522, 523:** add orrery and paleontology research.
6. **525, 526:** extend architectural materials and passive thermal infrastructure.
7. **527, 528:** complete athletics and stained-glass cultural spaces.

## Step integration matrix

### [513] Colony Theatre Company — TheatreCompanyPanel

**Core integration:** Extend the existing cultural-event/script state over NeedsSystem, DutyRosterSystem, JournalSystem, SoundManager, survivor participation, venue capacity, and the psychological-care contract.

**Smallest vertical slice:** Draft or select one original play, cast/rehearse available participants, perform it in a validated venue, archive the script, and record attendance and effect.

**Acceptance gate:** Script provenance, performers, rehearsal time, fatigue, venue, seating, audience, content tags, accessibility, audio, and morale/stress response are persisted. A founding play can grant a bounded 30-day effect to attending or eligible survivors; no colony-wide trait is applied without the effect pipeline.

### [514] Ferro-Cement Patrol Boat — FerroCementBoatPanel

**Core integration:** Extend StealthDiveInstance/surface-vessel state over LocationLayoutSystem, TacticalCombatSystem, cement, steel mesh, hull curing, waterproofing, crew, fuel, cargo, and coastal routes.

**Smallest vertical slice:** Build one hull section, cure and inspect it, launch one boat, and complete one patrol route with a weather/encounter result.

**Acceptance gate:** Mesh geometry, mortar composition, curing time, cracks, buoyancy, displacement, engine/sail condition, crew, stores, weather, draft, repair, and weapon status are saved. Three boats and elimination of pirate raids are not automatic consequences of one construction job.

### [515] Indigenous Language and Oral-History Archive — LinguisticArchivePanel

**Core integration:** Add a community-controlled linguistic record over JournalSystem, DutyRosterSystem, GenerationalSuccessionEngine, audio/text media, translator/elder roles, consent, and archive storage.

**Smallest vertical slice:** Record one consenting speaker, transcribe one narrative, annotate vocabulary/place-name context, and preserve the source audio and translation provenance.

**Acceptance gate:** Speaker consent, ownership, access permissions, dialect identity, phoneme confidence, translation uncertainty, date, recorder, storage, and community review are persisted. The archive must not force disclosure or flatten distinct dialects. Three languages and fifty narratives require actual reviewed records.

### [516] Kite Aerial Photography and Topographic Survey — KiteAerialSurveyPanel

**Core integration:** Extend ExpeditionSystem, LocationLayoutSystem, WeatherSystem, photogrammetry, camera calibration, kite/winch equipment, image sets, and map-tile discovery.

**Smallest vertical slice:** Launch one kite under valid wind, capture an overlapping image strip, solve a small mosaic, and add one confidence-rated terrain tile.

**Acceptance gate:** Kite lift, line tension, payload, wind, shutter interval, camera calibration, overlap, scale, terrain, image quality, recovery, and uncertainty are saved. A 100 km² survey and three valleys require actual tile coverage and evidence; no map reveals hidden sites by UI choice.

### [517] Cooperative Credit Union — CreditUnionPanel

**Core integration:** Add a member-owned financial state over MarketSystem, CrossingArbitrationSystem, DutyRosterSystem, ConstitutionAssemblyPanel/governance, account ledger, loan contracts, project financing, audit, and SaveChecksum.

**Smallest vertical slice:** Open member accounts, accept a deposit, approve one collateralized work loan through a recorded vote, disburse funds, and settle one repayment/dividend event.

**Acceptance gate:** Member eligibility, balance, deposit, interest, collateral, approval quorum, default, reserve, project escrow, audit, inflation/price basis, and atomic transfers are persisted. A cistern project can finish faster only if the financed project consumes funds and the construction schedule reflects it.

### [518] Mead Hall and Feasting Tradition — MeadHallPanel

**Core integration:** Extend the solera/mead/food and cultural-event contracts over NeedsSystem, RecipeCatalog/data, GenerationalSuccessionEngine, venue capacity, SoundManager, and social-cohesion effects.

**Smallest vertical slice:** Plan and serve one feast, assign cooks/servers/skalds, record menu/attendance, perform one story, and apply a bounded social result.

**Acceptance gate:** Food, beverage, seating, labor, alcohol safety, participation, dietary needs, story provenance, fatigue, and post-event effects are saved. Ten consecutive feasts create a milestone only when ten valid events occurred; a permanent +15 social effect must be a named, bounded rule.

### [519] Oak-Bark Pit Tannery — PitTanneryPanel

**Core integration:** Extend SilentFoundrySystem/CraftingSystem, DutyRosterSystem, hide inventory, stream/water, oak bark, lime, tannin liquor, leather quality, boots/armor/saddlery recipes, and environmental waste.

**Smallest vertical slice:** Prepare one hide, run one stage of a multi-month tan, inspect liquor/quality, and finish one leather side.

**Acceptance gate:** Hide mass, species, lime, bark, water, pH, tannin strength, pit time, temperature, contamination, labor, waste, thickness, grain, and drying are authoritative. Fifty sides and complete scout equipment require actual hides, time, and tailoring/armor capacity.

### [520] Indigo Vat and Block Printing — IndigoDyePrintPanel

**Core integration:** Extend GreenhouseSystem, CraftingSystem, textile inventory, indigo crop, fermentation, pH/redox, vat condition, carved blocks, dye repeats, and cultural artifact records.

**Smallest vertical slice:** Ferment one vat, carve one block, print a measured textile batch, and register a pattern/provenance record.

**Acceptance gate:** Plant mass, vat chemistry, oxidation, temperature, pH/redox, cloth area, block condition, dye depth, rinse water, labor, and colorfastness are saved. Patterned clothing can create bounded identity/comfort effects; no permanent trait is assigned to every dweller without an explicit effect.

### [521] Freshwater Pearl Mussel Aquaculture — PearlAquaculturePanel

**Core integration:** Add a freshwater aquaculture state over GreenhouseSystem or a shared aquatic-production authority, LocationLayoutSystem, stream flow/water quality, mussel stock, nucleation, CraftingSystem, MarketSystem, and CrossingArbitrationSystem.

**Smallest vertical slice:** Establish one pen, introduce a valid mussel cohort, perform one nucleation operation, advance growth, and harvest or reject one pearl batch.

**Acceptance gate:** Water flow, temperature, pH, contamination, mussel survival, bead condition, operator skill, growth time, shell damage, pearl quality, and trade value are persisted. A diplomatic gift may improve a negotiation; it cannot guarantee an alliance or replace faction terms.

### [522] Clockwork Orrery — OrreryPanel

**Core integration:** Extend JournalSystem, ResearchSystem, CraftingSystem, SimClock, celestial/planetary data, mechanical calibration, eclipse prediction, and navigation consumers.

**Smallest vertical slice:** Build and calibrate one orrery subset, predict a catalogued celestial event, compare it to the simulated calendar, and archive the result/error.

**Acceptance gate:** Gear ratios, period data, epoch, calibration, mechanical drift, date, observer, and event uncertainty are saved. A predicted eclipse three years ahead requires an authoritative celestial model; the orrery cannot create a scientific bonus from visual motion alone.

### [523] Fossil Excavation and Paleontology Lab — PalaeontologyLabPanel

**Core integration:** Add a specimen/research state over ExpeditionSystem, ResearchSystem, LocationLayoutSystem, sample inventory, stratigraphic layer, micro-CT asset/read model, taxonomy, and geological knowledge.

**Smallest vertical slice:** Excavate one specimen from a valid layer, preserve it, classify it with confidence, and add one stratigraphic record.

**Acceptance gate:** Location, layer, extraction damage, specimen identity, preparation, scan condition, taxonomy, dating method, uncertainty, researcher, and archive provenance are persisted. Twenty species and a complete local column require actual samples and ordered strata; no fabricated fossils.

### [524] Cooperative Mutual-Aid Society — MutualAidSocietyPanel

**Core integration:** Add a relief-fund state over CrossingArbitrationSystem, NeedsSystem, FinalWishSystem, GenerationalSuccessionEngine, Constitution/governance, member ledger, eligibility rules, and emergency events.

**Smallest vertical slice:** Register members, collect one contribution cycle, trigger one validated hardship claim, and issue an auditable relief payment.

**Acceptance gate:** Membership, contribution, reserve, claim cause, beneficiary, review, priority, fraud/duplicate prevention, payment, family/household links, and fund solvency are saved. Support prevents or reduces hardship when funds exist; five families cannot be promised if the reserve cannot cover claims.

### [525] Wrought-Iron Architectural Work — DecorativeIronworksPanel

**Core integration:** Extend SilentFoundrySystem, CraftingSystem, DutyRosterSystem, LocationLayoutSystem, architectural asset records, iron stock, forge condition, installation, and beauty/read-model scoring.

**Smallest vertical slice:** Forge one gate or candelabra, inspect joints/finish, install it in a valid space, and record contributor/provenance.

**Acceptance gate:** Iron mass, heat, tooling, scrollwork, joints, rivets, surface finish, condition, installation, accessibility, and maintenance are authoritative. Beauty is a typed room/cultural read model; maximum architecture score is not a direct button effect.

### [526] Passive Solar Trombe Wall — TrombeWallPanel

**Core integration:** Add a passive thermal facility state over LocationLayoutSystem, shared power/thermal contract, glazing, air gap, wall mass, WeatherSystem, ShelterOperationsPanel, NeedsSystem warmth, and maintenance.

**Smallest vertical slice:** Install one wall segment, simulate one day/night cycle, and update corridor temperature/heating demand.

**Acceptance gate:** Orientation, glazing, gap, wall material, solar irradiance, outdoor temperature, ventilation, heat capacity, leakage, shading, maintenance, and room load are saved. Passive heating reduces demand under valid sunlight; it cannot guarantee 18°C through every nuclear-winter night or reduce all heating by 30%.

### [527] Colony Olympiad — OlympiadFestivalPanel

**Core integration:** Add a scheduled athletic-event state over NeedsSystem, DutyRosterSystem, GenerationalSuccessionEngine, venue, participant age/health, equipment, injury, audience, and cultural effects.

**Smallest vertical slice:** Register eligible competitors, run one safe event, record results/medals, and apply bounded vitality/morale effects.

**Acceptance gate:** Eligibility, consent, training, fatigue, medical clearance, event rules, equipment, injury, accessibility, audience, and awards are persisted. Fifty competitors and a permanent Athletic Spirit effect require actual participation and a named progression rule.

### [528] Mosaic Stained-Glass Studio — StainedGlassStudioPanel

**Core integration:** Extend the stained-glass/monument contract over LocationLayoutSystem, CraftingSystem, glass/lead/tin, kiln, soldering, design provenance, installation, lighting, NeedsSystem comfort, and 2D scene presentation.

**Smallest vertical slice:** Cut, lead, solder, and install one panel, register its artist/pattern, and update one room’s light/mood read model.

**Acceptance gate:** Glass pieces, color, lead came, solder, breakage, kiln, labor, frame, weather, maintenance, light angle, and accessibility are authoritative. “Most beloved interior” is measured through attendance/comfort events, not a hard-coded quality flag.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes with stable ordinal ordering; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact reservation and settlement for food, alcohol, hides, bark, water, dyes, cloth, metals, glass, stone, pearls, account funds, relief funds, survey film, and construction materials.
- Explicit interruption, cancellation, failure, spoilage, contamination, leakage, ballot invalidation, default, injury, maintenance, stale-data, and negotiation outcomes.
- Canonical snake_case IDs and ordinal validation through the data-integrity rules.

Use explicit units: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, flow_m3_s, pH, redox_mv, vote_count, quorum_fraction, balance_units, interest_fraction, confidence_fraction, condition_fraction, and population_count.

## Data authority and content work

Extend existing catalogs where possible. Candidate new authority files, only if current catalogs cannot express the domain, are:

- theatre_production_defs.json
- surface_vessel_defs.json
- linguistic_archive_defs.json
- kite_survey_profiles.json
- cooperative_finance_rules.json
- feast_event_defs.json
- tannery_process_defs.json
- dye_vat_defs.json
- pearl_aquaculture_defs.json
- celestial_orrery_defs.json
- fossil_specimen_defs.json
- mutual_aid_rules.json
- architectural_ironwork_defs.json
- passive_thermal_defs.json
- olympiad_event_defs.json
- stained_glass_defs.json

Every new or edited authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and loader tests. Languages, records, laws, accounts, products, species, instruments, materials, effects, and cultural milestones must resolve through the canonical registry.

## Godot host and UI work

Build these panels only after their typed host contracts exist:

TheatreCompanyPanel, FerroCementBoatPanel, LinguisticArchivePanel, KiteAerialSurveyPanel, CreditUnionPanel, MeadHallPanel, PitTanneryPanel, IndigoDyePrintPanel, PearlAquaculturePanel, OrreryPanel, PalaeontologyLabPanel, MutualAidSocietyPanel, DecorativeIronworksPanel, TrombeWallPanel, OlympiadFestivalPanel, and StainedGlassStudioPanel.

Each panel must bind to a typed host session/read model, expose loading/empty/error states, show units and confidence where relevant, and issue commands that return validation results. No Bind(object), placeholder arrays, direct Core mutation, hard-coded success, or unsupported 3D-only dependency is permitted. New dirty stores must be wired through Main.Setup, Main.Save, and Main.Flush using the shared atomic writer.

## Verification plan

Run focused tests after each vertical slice, then the full Godot-only acceptance set:

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

Add focused coverage for:

- performance participation/attendance/script provenance, vessel hull/curing/weather/cargo, linguistic consent/transcription/translation, and kite wind/calibration/mosaic uncertainty;
- account deposits/loans/collateral/defaults/dividends/audit, feast food/alcohol/attendance/effects, tanning mass/chemistry/quality, dye vat chemistry/cloth coverage, and pearl survival/quality/trade;
- orrery calibration/prediction error, fossil stratigraphy/specimen provenance, mutual-aid solvency/claims, ironwork installation/beauty, Trombe thermal simulation, and Olympiad safety/results;
- stained-glass material conservation, installation/light read models, 2D presentation, cultural records, and all consumer effects;
- clean, tampered, missing-checksum, legacy, future-version, interruption, and atomic-write saves.

## Definition of done

Each Step 513–528 must be documented as implemented, explicitly deferred with a dependency, or rejected as unsupported by the current simulation model. An implemented step requires:

1. Core owns the behavior and state.
2. Godot exposes a typed, non-placeholder workflow.
3. Data is schema-versioned, canonical, and reference-valid.
4. Save/load, migration, checksum, atomicity, determinism, and dirty-store flushing are covered.
5. Cultural, economic, maritime, ecological, legal, industrial, thermal, and social claims are backed by tunable data and failure modes.
6. The full Godot-only verification pipeline passes without Unity dependencies.
