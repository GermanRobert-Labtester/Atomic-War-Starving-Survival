# ASHFALL — GENERATION WAVE 12 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 7
// SPDX-License-Identifier: MIT

**Document role:** execution-grade Part 7 master plan for Generation Wave 12, derived from the verified post-Wave 12 Part 6 unclaimed corpus ledger and dependency DAG.

**Purpose:** convert the eight highest-priority unsealed socio-economic, pedagogical, subterranean, and communications depth plans beyond the Wave 12 Part 6 frontier into an execution-grade unblocking package. Each task provides 20 procedural execution instructions and structured mini-tasks with 4 additional mini-substeps each, eliminating guesswork for coding agents and foremen.

**Wave 12 Part 7 Premise:** Parts 1 through 6 established presentation, core mechanics, decrees, informational warfare, social life, and late-game progression. Part 7 delivers the **human civilizational and territorial infrastructure**: contamination stigma and radiation trade economics, romance and family lineage continuity, survivor education and apprenticeships, disaster emergency response protocols, shelter institutional archives and memorialization, wasteland cartography and fog of war, underground tunnel exploration networks, and shelter radio program production with audience response loops.

**Part 7 Execution Set (Exactly 8 Tasks):**
1. **Task M1 — C2[30] Contamination Stigma: Radiation Economics, Trade Discounts & Social Trust (Plan 146A/146B/146C)**
2. **Task M2 — C2[31] Intimate Bonds: Romance, Courtship, Family Lineage & Bereavement (Plan 150A/150B/150C)**
3. **Task M3 — C2[32] Generational Pedagogy: Survivor Education, Apprenticeship & Vocational Skill Transfer (Plan 154A/154B/154C)**
4. **Task M4 — C2[33] Crisis Resilience: Acute Disaster Protocols, Containment & Recovery Continuity (Plan 158A/158B/158C)**
5. **Task N1 — C2[34] Institutional Memory: Shelter Historical Archive, Cataloged Relics & Searchable Chronicle (Plan 162A/162B/162C)**
6. **Task N2 — C2[35] Wasteland Cartography: Survey Quality, Fog of War, Topographic Discovery & Map Trading (Plan 163A/163B/163C)**
7. **Task N3 — C2[36] Subterranean Highway: Underground Tunnel Exploration, Mine Collapse & Deep Route Hazards (Plan 167A/167B/167C)**
8. **Task N4 — C2[37] Diegetic Broadcast: Shelter Radio Production, DJ Host Workflows & Wasteland Response (Plan 173A/173B/173C)**

---

# 0. OPERATING CONTRACT

## 0.1 Allowed Terminal States

Every task in Wave 12 Part 7 must reach one of these terminal states:

- **IMPLEMENTED** — Missing mechanism, consumer, contract, or surface is authored, wired to canonical owners, and verified green across all required test gates.
- **DECIDED-DEFERRED** — Product/architecture authority requires a signature; an implementation-selective decision memo is signed and recorded, leaving the remainder explicitly deferred without claiming false completion.
- **RETIRED** — An obsolete or duplicate file/shim/mechanism is deleted and unregistered with reference proof and architecture map regeneration.
- **VERIFIED-RESOLVED** — Re-verification at repository `HEAD` proves the blocker was already sealed by concurrent work; evidence is recorded and redundant implementation is skipped.
- **ROUTED-REPAIR** — Investigation exposes a genuine production defect outside the task's bounded scope; an isolated repair package with reproducible characterization test is registered.

## 0.2 Non-Negotiable Hard Rules

1. **Godot is authoritative; Unity is retired.** No Unity dependencies, shims, or references may be added.
2. **Core stays engine-free.** `Assets/Ashfall.Core/` (`netstandard2.1`) must never reference Godot or engine types.
3. **JSON data is authoritative.** Authoritative data resides in `Assets/StreamingAssets/Data/` with valid schema policy.
4. **Preserve determinism and persistence.** No `System.Random` or unseeded wall-clock RNG in Core domain logic.
5. **One authority per concern.** Never create duplicate registries, parallel save stores, or shadow managers.
6. **Claims before edits.** Check and record file path claims in `WORKTREE_OWNERSHIP.md` before touching code.
7. **Substeps are instructions, not tasks.** The 20 substeps per task represent ordered procedural instructions.
8. **Mini-tasks require 4 mini-substeps.** Any mini-task (e.g. `.1`, `.2`) must contain exactly 4 subsequent execution instructions.
9. **Focused testing first.** Use `scripts/run_test.sh` for bounded xUnit runs; do not run broad suites unprompted.
10. **Zero warning tolerance.** Production code edits must maintain a 0-error, 0-warning baseline on build.

---

# 1. DETAILED TASK SPECIFICATIONS

## TASK M1 — C2[30] Contamination Stigma: Radiation Economics, Trade Discounts & Social Trust (Plan 146A/146B/146C)

- **Source Plan:** `C-integration-plans/C2_planintegration[30].md` (Plan 146)
- **Blocker Class:** RADIATION SOCIAL BRIDGE / CONTAMINATED TRADE GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Radiation/`, `Assets/Ashfall.Core/Economy/`
- **Target Subsystem:** Contaminated item valuation, buyer dosimeter inspection, irradiated survivor social stigma, cleansing certs

### 20 Procedural Substeps:
1. Review `RadiationSystem.cs`, `DoseLedgerSystem.cs`, `MarketSystem.cs`, and `HoldfastTradeSession.cs` to map contamination seams.
2. Enforce core invariant: radiation social effects project canonical physical dose; never maintain duplicate price or standing ledgers.
3. Claim `Assets/Ashfall.Core/Radiation/RadiationSocialBridgeCoordinator.cs` and `Assets/Ashfall.Core/Radiation/ContaminationTradeFilter.cs`.
4. Define `ContaminationTier` enum: Clean, TraceExposure, HotIrradiated, LethalLuminescent.
5. Implement buyer dosimeter inspection: merchants scan incoming trade items, rejecting or heavily discounting hot goods.
6. Apply economic valuation penalties: contaminated food suffers an 80% price penalty; contaminated medical supplies are refused outright.
7. Author decontamination certification mechanics: running items through the ultrasonic decontaminator restores commercial valuation.
8. Model irradiated survivor social stigma: survivors with high acute rad poisoning suffer social avoidance in mess halls.
9. Connect survivor radiation levels to diplomatic friction: visiting envoys demand that irradiated leaders undergo decontamination scrubs.
10. Prevent fraudulent trade exploits: attempting to sell masked hot items without disclosing rads triggers merchant boycott events.
11. Author data-driven contamination pricing curves in `contamination_economic_rules.json` with standard `schema_version: 1`.
12. Enforce schema integrity validation in `CatalogIntegrityValidator.cs` confirming all item categories declare contamination tolerances.
13. Wire rad-cleansing certificates to `DecontaminationSystem`, allowing certified clean goods to bypass buyer inspection fees.
14. Ensure radiation stigma penalties clear completely once survivors undergo successful chelation and medical treatment.
15. Save and restore trade boycott records and merchant trust ratings cleanly inside `EconomySaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Radiation/RadiationSocialBridgeTests.cs` verifying price deduction formulas.
17. Verify deterministic seed isolation ensuring identical dosimeter rolls yield identical merchant price adjustments.
18. Validate that trade UI clearly displays buyer dosimeter readings, rad warning icons, and net valuation penalties.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/economy/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### M1.1 Item Contamination & Valuation Degradation
- (a) Author `ContaminationPriceCalculator` computing valuation discounts based on micro-Sievert surface counts.
- (b) Enforce strict merchant refusal rules for irradiated baby formula, clean water, and sterile bandages.
- (c) Provide non-destructive Geiger counter inspection API querying canonical item contamination state.
- (d) Write unit tests verifying that pristine clean items suffer zero valuation deductions during trade sessions.

#### M1.2 Buyer Inspection & Fraud Discovery
- (a) Model merchant inspection thoroughness based on merchant faction technical level (e.g. Scavengers vs Enclave).
- (b) Detect player attempts to disguise hot items using lead-lined containers or counterfeit clean stamps.
- (c) Trigger immediate merchant hostility, trade embargoes, and reputation penalties upon fraud discovery.
- (d) Author tests validating that honest disclosure of radioactive scrap allows salvage sales at fair raw-metal rates.

#### M1.3 Irradiated Survivor Stigma & Cohort Friction
- (a) Query cumulative rem dose from `DoseLedgerSystem` to establish survivor ambient radiation emission.
- (b) Apply social distance penalties: healthy survivors avoid sharing bunk rooms with glowing irradiated patients.
- (c) Connect irradiated leadership status to diplomatic hesitation in `FactionDiplomacyCoordinator`.
- (d) Author tests proving that decontaminating survivors immediately eliminates social friction in communal spaces.

#### M1.4 Chelation Certification & Decontamination Receipts
- (a) Generate `DecontaminationCertificate` items upon completing full medical autoclave and scrub cycles.
- (b) Allow bulk presentation of cleansing certificates to traveling caravans to unlock premium export prices.
- (c) Track certificate validity periods ensuring old papers cannot be reused on newly scavenged wasteland scrap.
- (d) Author characterization tests confirming that certified clean water crates command premium market exchange rates.

---

## TASK M2 — C2[31] Intimate Bonds: Romance, Courtship, Family Lineage & Bereavement (Plan 150A/150B/150C)

- **Source Plan:** `C-integration-plans/C2_planintegration[31].md` (Plan 150)
- **Blocker Class:** ROMANCE LIFECYCLE / FAMILY SOCIAL CONTINUITY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Relations/`, `Assets/Ashfall.Core/Survivors/`
- **Target Subsystem:** Consent-based courtship progression, cohabitation bunking, shared family bereavement, lineage trait inheritance

### 20 Procedural Substeps:
1. Review `SurvivorRelationsSystem.cs`, `CohortSystem.cs`, and `CaregivingSystem.cs` to map long-form relationship seams.
2. Enforce core invariant: romance tracks lifecycle stages and mutual consent; it does not replace canonical affinity scores.
3. Claim `Assets/Ashfall.Core/Relations/RomanceLifecycleCoordinator.cs` and `Assets/Ashfall.Core/Relations/FamilyUnitRecord.cs`.
4. Define `RomanceStage` enum: Unacquainted, DevelopingAttraction, MutualCourtship, CommittedPartners, Estranged.
5. Implement deterministic attraction compatibility based on shared personality traits, complementary values, and age proximity.
6. Build a consent-based courtship progression requiring mutual positive interactions and zero active grievances.
7. Allow committed partners to cohabit in designated double-bunk quarters in `ShelterRoomSystem`, boosting overnight stress relief.
8. Wire family unit formation linking married partners and their biological or adopted children in `GenerationalLineageExtension`.
9. Implement family duty preference: parents prefer work shifts aligned with their partners to coordinate childcare duties.
10. Connect shared bereavement: the death of a partner or child inflicts profound grief, requiring compassionate leave and counseling.
11. Build a lineage trait inheritance model where children deterministically inherit predispositions from both parents.
12. Ensure partnership breakups or estrangements route through `InterpersonalConflictCoordinator` as structured divorce events.
13. Author `romance_compatibility_rules.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce catalog integrity validation confirming all lineage trait references exist in the canonical trait database.
15. Save and restore romance stages, wedding anniversaries, and family tree links cleanly inside `RelationsSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Relations/RomanceLifecycleTests.cs` verifying courtship progression rules.
17. Verify deterministic seed isolation ensuring identical survivor pairings develop identical courtship timelines.
18. Validate that survivor inspect cards display partner portraits, relationship badges, and family lineage trees cleanly.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/survivors/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### M2.1 Attraction Compatibility & Courtship Logic
- (a) Author `AttractionCompatibilityCalculator` evaluating ideological harmony, mutual respect, and age brackets.
- (b) Advance courtship stages during leisure hours spent sharing meals or recreational spaces in the shelter.
- (c) Provide voluntary player blessing or private survivor initiative enabling formal partnership commitments.
- (d) Write unit tests verifying that survivors with active grievances or ideological contempt reject courtship advances.

#### M2.2 Cohabitation & Shared Household Buffs
- (a) Assign committed couples to double-bed residential quarters in `ShelterRoomSystem`.
- (b) Apply passive comfort and morale restoration bonuses when couples rest together in private quarters.
- (c) Enable shared storage lockers allowing partners to freely share personal keepsakes and clothing items.
- (d) Author tests validating that forced separation of couples into remote dorms generates relationship strain.

#### M2.3 Family Lineage & Trait Inheritance
- (a) Track nuclear and blended family units linking parents, children, and legal guardians in a unified graph.
- (b) Implement genetic and cultural trait inheritance blending parental physical endurance and intellectual curiosities.
- (c) Coordinate parental supervision during child schooling shifts in `CaregivingSystem`.
- (d) Author tests proving that orphan adoption creates valid parental bonds with full inheritance rights.

#### M2.4 Bereavement Grief & Compassionate Protocols
- (a) Intercept partner and child mortality events emitted by `SurvivorMortalityCoordinator`.
- (b) Apply acute bereavement grief debuffs imposing severe temporary work capacity penalties on surviving partners.
- (c) Provide leadership option to grant 7-day bereavement leave, preventing stress breakdowns during mourning.
- (d) Author characterization tests confirming that counseling sessions gradually alleviate bereavement depression.

---

## TASK M3 — C2[32] Generational Pedagogy: Survivor Education, Apprenticeship & Vocational Skill Transfer (Plan 154A/154B/154C)

- **Source Plan:** `C-integration-plans/C2_planintegration[32].md` (Plan 154)
- **Blocker Class:** KNOWLEDGE TRANSFER / APPRENTICESHIP CONTINUITY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Skills/`, `Assets/Ashfall.Core/Shelter/`
- **Target Subsystem:** Classroom schooling for youths, master-apprentice workbench pairings, technical literacy, curriculum planning

### 20 Procedural Substeps:
1. Review `SkillProgressionSystem.cs`, `DutyRosterSystem.cs`, and `SurvivorLifecycle.cs` to map pedagogy boundaries.
2. Enforce core invariant: education transfers knowledge through canonical skill systems; it does not bypass skill requirements.
3. Claim `Assets/Ashfall.Core/Skills/SurvivorEducationCoordinator.cs` and `Assets/Ashfall.Core/Skills/ApprenticeshipContract.cs`.
4. Define `EducationPhase` enum: EarlyLiteracy, GeneralWastelandSurvival, VocationalApprenticeship, AdvancedResearchSpecialization.
5. Designate classroom and schoolroom facilities in `ShelterRoomSystem` providing study desks, chalkboards, and library shelves.
6. Implement daily curriculum scheduling in `DutyRosterSystem` dedicating morning shifts to child education.
7. Connect assigned teachers to classroom instruction, scaling student learning speed by the teacher's intellectual skills.
8. Build a one-on-one vocational apprenticeship contract binding an adolescent apprentice to a master craftsman or surgeon.
9. Advance vocational skill experience during shared workbench shifts at the machine shop, forge, or medical clinic.
10. Model technical literacy: illiterate survivors learn basic reading, enabling them to decipher equipment repair manuals.
11. Implement textbook and educational supply consumption, deducting chalk, paper, and pre-war primers from inventory.
12. Author graduation milestones: upon reaching adulthood, educated youths enter the adult workforce with baseline Tier 2 skills.
13. Wire neglected education to developmental penalties: children forced into manual labor grow up with stunted technical potential.
14. Author `education_curricula.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
15. Enforce catalog integrity validation confirming all curriculum paths map to valid skill identifiers in `skills.json`.
16. Save and restore apprenticeship bindings, student grade progress, and active classroom rosters inside `SkillsSaveSection`.
17. Author unit tests in `Ashfall.Core.Tests/Skills/SurvivorEducationTests.cs` verifying learning rate calculations.
18. Verify deterministic seed isolation ensuring identical schooling regimens produce identical student skill distributions.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/skills/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### M3.1 Classroom Infrastructure & Literacy
- (a) Author `ClassroomFacilityRecord` tracking student capacity, desk condition, and library book quality.
- (b) Advance child literacy points daily, transitioning illiterate children to basic and advanced reading comprehension.
- (c) Unlock the ability to read technical lore documents and repair schematics upon achieving basic literacy.
- (d) Write unit tests verifying that unpowered, freezing classrooms severely penalize daily student learning progress.

#### M3.2 Teacher Assignment & Curriculum Planning
- (a) Provide player interface to select active educational curricula (e.g., Agricultural Focus vs Mechanical Engineering).
- (b) Assign qualified adult survivors to teacher roles in `DutyRosterSystem`, pulling from Science and Medicine pools.
- (c) Calculate teaching effectiveness scaled by teacher patience traits and pedagogical certifications.
- (d) Author tests validating that highly skilled teachers accelerate student learning by up to 2.5x baseline rates.

#### M3.3 Master-Apprentice Workbench Contracts
- (a) Author `ApprenticeshipContract` binding an adolescent student to a senior master craftsman (Skill Level 4+).
- (b) Schedule paired duty shifts where master and apprentice work side-by-side at the same fabrication bench.
- (c) Transfer practical vocational skill points on every successful craft or medical procedure completed together.
- (d) Author tests proving that apprentices paired with abusive or hostile masters suffer morale collapse and learn slowly.

#### M3.4 Graduation & Generational Workforce Entry
- (a) Trigger graduation ceremony upon survivor transition from Youth to Adult life stage in `Plan176SurvivorAgingCoordinator`.
- (b) Grant starting skill proficiencies and vocational certification based on cumulative educational achievements.
- (c) Automatically enroll graduated adults into the general labor pool with immediate suitability for skilled trades.
- (d) Author characterization tests confirming that educated generations maintain higher shelter industrial productivity.

---

## TASK M4 — C2[33] Crisis Resilience: Acute Disaster Protocols, Containment & Recovery Continuity (Plan 158A/158B/158C)

- **Source Plan:** `C-integration-plans/C2_planintegration[33].md` (Plan 158)
- **Blocker Class:** DISASTER RESPONSE / POST-CRISIS RECOVERY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Events/`, `Assets/Ashfall.Core/Shelter/`
- **Target Subsystem:** Disaster response protocols, automated triage routing, secondary damage containment, recovery milestones

### 20 Procedural Substeps:
1. Review `EmergencyAlertCoordinator.cs`, `DutyRosterSystem.cs`, and `ShelterRoomSystem.cs` to map disaster management seams.
2. Enforce core invariant: disaster response coordinates protocols and labor shifts; it does not simulate primary hazards.
3. Claim `Assets/Ashfall.Core/Events/DisasterResponseCoordinator.cs` and `Assets/Ashfall.Core/Events/DisasterProtocol.cs`.
4. Define `DisasterKind` enum: StructuralCollapse, ToxicAirLeak, SublevelFlood, ReactorMeltdown, PandemoniumFire.
5. Author pre-configured response protocols (ContainmentLockdown, EmergencyEvacuation, FireSuppressionSweep, MedicalTriage).
6. Implement atomic protocol activation: triggering a protocol automatically reallocates duty shifts to priority emergency tasks.
7. Connect FireSuppressionSweep to fire extinguisher inventory, dispatching firefighters directly to burning sectors.
8. Connect ContainmentLockdown to airlock door controls, hermetically sealing breached rooms to prevent gas/water spread.
9. Route injured survivors directly to the emergency triage room in `MedicalTriageCoordinator` with trauma prioritization.
10. Calculate secondary damage containment: effective emergency response prevents localized fires from spreading across hallways.
11. Implement post-disaster recovery milestones tracking rubble clearance, structural re-bracing, and electrical re-wiring.
12. Provide emergency resource rationing overrides: during disasters, auxiliary battery reserves are prioritized for life support.
13. Author `disaster_protocols.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce catalog integrity validation confirming all response protocols link to valid duty roles and room equipment tags.
15. Save and restore active disaster state machines, response protocol assignments, and triage queues inside `EventsSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Events/DisasterResponseTests.cs` verifying protocol execution flows.
17. Verify deterministic seed isolation ensuring identical disaster responses produce identical secondary containment rates.
18. Validate that emergency UI displays clear protocol toggle buttons, affected room heatmaps, and casualty status counts.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/events/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### M4.1 Protocol Registry & Pre-Configuration
- (a) Define `DisasterProtocol` schema declaring affected duty roles, required tool items, and airlock door states.
- (b) Author default protocols in `disaster_protocols.json` with localized titles and operation descriptions.
- (c) Allow players to customize protocol rosters during peacetime to prepare specialized emergency response squads.
- (d) Write unit tests verifying that all authored protocols declare valid duty role and item requirement identifiers.

#### M4.2 Atomic Protocol Activation & Reallocation
- (a) Provide one-click protocol activation API immediately overriding standard peacetime duty assignments.
- (b) Mobilize off-duty and sleeping emergency squad members, waking them immediately to respond to alarms.
- (c) Equip required emergency gear (gas masks, fire axes, hazmat suits) from nearest communal lockers.
- (d) Author tests validating that activating FireSuppression immediately deploys firefighters to burning room nodes.

#### M4.3 Secondary Damage Containment
- (a) Track hazard spillover probabilities between adjacent connected room grid nodes in `ShelterRoomSystem`.
- (b) Apply physical containment boundaries: closed airtight blast doors stop 100% of smoke and floodwater spread.
- (c) Calculate structural re-bracing times to prevent secondary cave-ins while survivors evacuate damaged sectors.
- (d) Author tests proving that uncontained fires spread to adjacent rooms within 3 hours if containment protocols fail.

#### M4.4 Post-Crisis Triage & Recovery Tracking
- (a) Direct walking wounded and incapacitated survivors to designated medical triage stations.
- (b) Track post-disaster recovery tasks: de-watering flooded rooms, clearing concrete rubble, replacing blown fuses.
- (c) Return survivors to normal daily schedules automatically once all active disaster hazards are marked resolved.
- (d) Author characterization tests confirming that disciplined protocol execution dramatically reduces total casualties.

---

## TASK N1 — C2[34] Institutional Memory: Shelter Historical Archive, Cataloged Relics & Searchable Chronicle (Plan 162A/162B/162C)

- **Source Plan:** `C-integration-plans/C2_planintegration[34].md` (Plan 162)
- **Blocker Class:** ARCHIVE CONTINUITY / INSTITUTIONAL MEMORY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Campaign/`, `Assets/Ashfall.Core/Narrative/`
- **Target Subsystem:** Historical event indexing, museum exhibit displays, survivor oral history recordings, searchable vault logs

### 20 Procedural Substeps:
1. Review `JournalSystem.cs`, `MemorialSystem.cs`, and `CampaignCalendar.cs` to map historical documentation hooks.
2. Enforce core invariant: archive indexes canonical history and displays relics; it does not fabricate past events.
3. Claim `Assets/Ashfall.Core/Narrative/ShelterArchiveCoordinator.cs` and `Assets/Ashfall.Core/Narrative/HistoricalRelicRecord.cs`.
4. Define `ArchiveEntryCategory` enum: VaultFounding, MajorCatastrophe, HeroicSacrifice, ScientificBreakthrough, FactionTreaty.
5. Implement automatic event indexing logging every notable victory, pandemic, leader transition, and expedition return.
6. Designate archive and museum gallery rooms in `ShelterRoomSystem` with display cases, display pedestals, and recording booths.
7. Connect cataloged historical relics (faded pre-war flags, shattered robot core, treaty quills) to display cases.
8. Wire visitor and resident museum viewing to cultural morale buffs, reinforcing shelter identity and shared purpose.
9. Implement survivor oral history recordings: elderly survivors record memoirs in the archive before passing away.
10. Build a full-text searchable chronicle search interface allowing players to filter past events by survivor, year, or faction.
11. Connect archived technological breakthroughs to historical research reference bonuses, speeding up future related tech.
12. Model archive physical preservation: maintaining dry, climate-controlled archive rooms prevents paper document decay.
13. Author `archive_relic_definitions.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce catalog integrity validation confirming all historical relics reference valid item identifiers.
15. Save and restore historical chronicle indexes, relic display assignments, and audio recordings inside `ChronicleSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Narrative/ShelterArchiveTests.cs` verifying event indexing accuracy.
17. Verify deterministic seed isolation ensuring identical campaign journeys produce identical historical archive logs.
18. Validate that archive UI screens provide fast text searching, chronological sorting, and rich multimedia summaries.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/narrative/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### N1.1 Event Indexing & Chronicle Architecture
- (a) Author `HistoricalEventRecord` struct capturing CampaignDay, Category, InvolvedActors, and LocalizedSummaryKey.
- (b) Listen to semantic day events across the event bus, automatically capturing significant campaign milestones.
- (c) Provide structured tagging system allowing entries to be queried by topic (e.g. `War`, `Epidemic`, `Founding`).
- (d) Write unit tests verifying that trivial daily chores are excluded while major milestones are accurately indexed.

#### N1.2 Museum Exhibits & Relic Curating
- (a) Create `ExhibitDisplayPedestal` schema binding physical inventory relics to room display coordinates.
- (b) Calculate aesthetic and cultural inspiration values granted to survivors visiting the museum room.
- (c) Prevent display relics from being accidentally recycled or bartered while placed in active exhibit cases.
- (d) Author tests validating that curated pre-war relics generate significant cultural morale bonuses.

#### N1.3 Oral Histories & Elder Memoirs
- (a) Author interview action allowing youth or scribes to record oral histories from elderly survivors (Age 60+).
- (b) Generate unique memoir audio log entries capturing personal perspectives on the pre-war collapse.
- (c) Preserve oral history transcripts permanently in the archive even after the elder survivor passes away.
- (d) Author tests proving that recording memoirs preserves elder wisdom bonuses for future generations.

#### N1.4 Searchable UI & Preservation Mechanics
- (a) Build fast in-memory search index filtering hundreds of historical entries by keyword, year, or actor ID.
- (b) Implement document degradation checks in damp, flooded archives, requiring moisture control to preserve records.
- (c) Provide export functionality outputting the shelter's chronicle to clean Markdown text files.
- (d) Author characterization tests confirming that search queries return accurate, deterministic result sets.

---

## TASK N2 — C2[35] Wasteland Cartography: Survey Quality, Fog of War, Topographic Discovery & Map Trading (Plan 163A/163B/163C)

- **Source Plan:** `C-integration-plans/C2_planintegration[35].md` (Plan 163)
- **Blocker Class:** SURVEY QUALITY / CARTOGRAPHIC DISCOVERY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Expeditions/`, `Assets/Ashfall.Core/World/`
- **Target Subsystem:** Map accuracy tiers, fog-of-war revelation, geographic triangulation, cartographic survey sales to merchants

### 20 Procedural Substeps:
1. Review `ExpeditionSystem.cs`, `WorldMapTopology.cs`, and `ProceduralScavengeSystem.cs` to map world exploration seams.
2. Enforce core invariant: cartography tracks knowledge of world nodes; it does not alter underlying physical map topography.
3. Claim `Assets/Ashfall.Core/World/WastelandCartographyCoordinator.cs` and `Assets/Ashfall.Core/World/MapSurveyRecord.cs`.
4. Define `SurveyQuality` enum: TerraIncognita, RumoredApproximate, ReconnoiteredRough, SurveyedPrecise, FullyMapped.
5. Implement dynamic fog-of-war revelation: expeditions marching through sectors unveil adjacent nodes and travel routes.
6. Connect surveyor survival skills and optical equipment (Binoculars, Theodolites) to accelerated survey quality upgrades.
7. Model survey inaccuracy: approximate nodes may display incorrect resource estimates or underestimate radiation hazards.
8. Wire precise mapping to travel efficiency: traversing FullyMapped routes reduces travel time and ambush chances by 20%.
9. Build cartographic surveying work orders allowing expeditions to survey unexplored valleys, creating physical map items.
10. Enable cartographic map trading: players can purchase pre-war tactical maps or sell authored surveys to visiting merchants.
11. Handle topographic landmark discovery: mapping radio towers, bridges, and mountain passes unlocks regional navigation bonuses.
12. Ensure map knowledge persists across expeditions and campaigns, saving discovered node states in `WorldSaveSection`.
13. Author `cartography_parameters.json` in `Assets/StreamingAssets/Data/` declaring survey tiers with `schema_version: 1`.
14. Enforce catalog integrity validation confirming all surveyable landmarks exist in the canonical `world_locations.json`.
15. Save and restore survey quality tiers, fog-of-war bitmasks, and authored map items inside `WorldSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/World/WastelandCartographyTests.cs` verifying survey progression.
17. Verify deterministic seed isolation ensuring identical scouting journeys produce identical map revelation bounds.
18. Validate that wasteland world map UI renders fog of war with smooth topographic parchment shading and clear icons.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/world/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### N2.1 Fog of War & Discovery Mechanics
- (a) Model per-node `SurveyQuality` tracking player knowledge of each location on `WorldMapTopology`.
- (b) Implement sightline calculation revealing nodes within visual radius of expedition routes based on terrain elevation.
- (c) Transition nodes from `TerraIncognita` to `ReconnoiteredRough` upon first expedition transit.
- (d) Write unit tests verifying that unvisited distant nodes remain concealed beneath fog of war.

#### N2.2 Survey Quality & Resource Accuracy
- (a) Model information uncertainty: rough surveys show generic hazard categories; precise surveys reveal exact Sievert levels.
- (b) Calculate travel route speed bonuses awarded when traversing fully triangulated roads.
- (c) Reduce raider ambush probabilities along well-mapped corridors by pre-identifying choke points.
- (d) Author tests validating that high-quality surveys prevent expeditions from wandering into unpredicted radiation pockets.

#### N2.3 Physical Map Items & Cartographic Production
- (a) Author crafting recipe in `CraftingSystem` allowing skilled cartographers to draw `RegionalSurveyMap` items.
- (b) Consume blank paper and ink supplies to generate tradeable physical maps of explored sectors.
- (c) Enable map consumption: reading a purchased map item instantly upgrades target region survey quality.
- (d) Author tests proving that selling survey maps to merchants yields significant barter wealth.

#### N2.4 Landmark Triangulation & Strategic Vistas
- (a) Designate high-elevation mountain peaks and pre-war broadcast towers as strategic survey landmarks.
- (b) Grant massive regional map revelation when an expedition successfully summits and surveys a landmark node.
- (c) Bind triangulation bonuses to radio navigation aids in `VerdictRadioSystem`.
- (d) Author characterization tests confirming that scouting landmarks accelerates overall wasteland exploration.

---

## TASK N3 — C2[36] Subterranean Highway: Underground Tunnel Network, Mine Collapse & Deep Route Hazards (Plan 167A/167B/167C)

- **Source Plan:** `C-integration-plans/C2_planintegration[36].md` (Plan 167)
- **Blocker Class:** SUBTERRANEAN EXPLORATION / TUNNEL NETWORK GAP
- **Canonical Owner:** `Assets/Ashfall.Core/World/`, `Assets/Ashfall.Core/Expeditions/`
- **Target Subsystem:** Underground tunnel topology, subterranean cave-in hazards, toxic damp air, alternative storm-safe travel

### 20 Procedural Substeps:
1. Review `WorldMapTopology.cs`, `ExpeditionSystem.cs`, and `ShelterRoomSystem.cs` to map subterranean route seams.
2. Enforce core invariant: tunnel network provides alternative topological travel edges; it does not duplicate world coordinates.
3. Claim `Assets/Ashfall.Core/World/UndergroundTunnelCoordinator.cs` and `Assets/Ashfall.Core/World/TunnelSectorRecord.cs`.
4. Define `TunnelSectorKind` enum: MetroTube, MineAdit, DrainageSewer, NaturalCavern, SealedMilitaryBunker.
5. Author subterranean route connections in `subterranean_routes.json` linking shelter basement airlocks to distant wasteland ruins.
6. Connect tunnel travel to weather immunity: traveling underground completely shields expeditions from surface blizzards and ash fallout.
7. Model deep subterranean hazards: toxic damp air (chokedamp), cave-in collapses, flooded sumps, and blind predator nests.
8. Require specialized subterranean equipment (MiningHelmets, GasCanaryCages, ShoringTimbers, SubmersiblePumps) for passage.
9. Implement tunnel clearance operations: excavating blocked mine shafts requires assigned heavy labor and explosive demolition charges.
10. Wire subterranean route maintenance: unmaintained timber shoring suffers decay, risking catastrophic cave-ins over time.
11. Build covert subterranean infiltration routes allowing strike teams to bypass hostile surface faction checkpoints undetected.
12. Ensure tunnel sectors can be permanently sealed using detonation charges to defend the shelter from underground monstrosities.
13. Author `tunnel_hazard_definitions.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce catalog integrity validation confirming all subterranean routes connect valid shelter rooms to world map nodes.
15. Save and restore tunnel clearance progress, structural shoring integrity, and discovered shortcuts inside `WorldSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/World/UndergroundTunnelTests.cs` verifying weather immunity mechanics.
17. Verify deterministic seed isolation ensuring identical tunneling expeditions encounter identical structural obstacles.
18. Validate that world map UI provides a toggleable subterranean layer view displaying tunnel networks beneath surface roads.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/world/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### N3.1 Subterranean Topology & Surface Connections
- (a) Define `TunnelSectorRecord` struct linking origin shelter sublevels to destination surface ruins.
- (b) Author schema-validated subterranean graph in `subterranean_routes.json` with distinct route lengths and elevations.
- (c) Provide route availability checks ensuring collapsed sectors block transit until cleared by excavation teams.
- (d) Write unit tests verifying that tunnel routes correctly map to valid physical entrance and exit coordinates.

#### N3.2 Surface Weather Immunity & Environmental Protection
- (a) Evaluate surface weather conditions during expedition pathfinding in `ExpeditionSystem`.
- (b) Grant 100% immunity to surface blizzards, acid rain, and fallout clouds while traveling along tunnel edges.
- (c) Maintain constant subterranean temperatures (10°C / 50°F), protecting travelers from lethal winter freezes.
- (d) Author tests validating that winter expeditions safely reach distant outposts through tunnels during cataclysmic storms.

#### N3.3 Subterranean Hazards & Equipment Checks
- (a) Model atmospheric asphyxiation hazards (methane pockets, oxygen depletion) requiring breathing apparatus.
- (b) Implement cave-in instability checks: high seismic activity or explosive usage increases rockfall damage risks.
- (c) Connect miners' canary cages and gas detectors in inventory to early hazard detection warnings.
- (d) Author tests proving that traveling without illumination or gas masks in abandoned mines results in severe casualties.

#### N3.4 Clearance Excavation & Structural Shoring
- (a) Author clearance work orders requiring mining picks, dynamite, and engineering labor to reopen collapsed sectors.
- (b) Track structural timber shoring condition, requiring periodic lumber maintenance to prevent secondary collapses.
- (c) Provide emergency tunnel demolition command allowing defenders to collapse routes behind retreating teams.
- (d) Author characterization tests confirming that collapsed tunnels successfully block hostile underground incursions.

---

## TASK N4 — C2[37] Diegetic Broadcast: Shelter Radio Production, DJ Host Workflows & Wasteland Response (Plan 173A/173B/173C)

- **Source Plan:** `C-integration-plans/C2_planintegration[37].md` (Plan 173)
- **Blocker Class:** RADIO PROGRAMMING / AUDIENCE FEEDBACK GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Narrative/`, `Assets/Ashfall.Core/Audio/`
- **Target Subsystem:** Shelter radio program scheduling, presenter DJ workflows, music/speech balancing, listener feedback dilemmas

### 20 Procedural Substeps:
1. Review `VerdictRadioSystem.cs`, `TurntableSystem.cs`, and `PowerGridSystem.cs` to map broadcast infrastructure boundaries.
2. Enforce core invariant: radio production coordinates programming and wasteland responses; it does not duplicate radio transmission physics.
3. Claim `Assets/Ashfall.Core/Narrative/ShelterRadioProductionCoordinator.cs` and `Assets/Ashfall.Core/Narrative/RadioBroadcastSchedule.cs`.
4. Define `BroadcastContentType` enum: FolkMusic, SurvivalAdvice, WastelandNews, PhilosophicalMonologue, DistressRelay.
5. Designate radio studio broadcast booths in `ShelterRoomSystem` requiring functional microphones, transmitter power, and antenna towers.
6. Assign shelter survivors to radio host / DJ roles in `DutyRosterSystem`, evaluating personality charisma and speech traits.
7. Implement daily broadcast scheduling allowing players to balance music, news commentary, and weather forecasts.
8. Wire active shelter broadcasts to regional listener reception, generating positive wasteland reputation and listener letters.
9. Connect survivor DJ commentary to shelter morale: entertaining, hopeful hosts grant modest daily mood buffs to all radio listeners.
10. Model wasteland audience response dilemmas: broadcasts provoke fan visits, merchant requests, or angry warlord threats.
11. Implement host burnout: continuous daily broadcasting without breaks strains host vocal cords, requiring periodic host rotation.
12. Integrate emergency broadcast alerts: breaking news of incoming raids or radiation storms automatically interrupts scheduled shows.
13. Author `radio_program_templates.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce catalog integrity validation confirming all broadcast tracks reference valid audio asset cues and localized speech keys.
15. Save and restore broadcast schedules, listener feedback queues, and DJ host ratings inside `NarrativeSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Narrative/ShelterRadioProductionTests.cs` verifying program scheduling mechanics.
17. Verify deterministic seed isolation ensuring identical broadcast playlists generate identical listener feedback pools.
18. Validate that radio studio UI displays audio VU meters, active playlist cues, listener frequency maps, and mail logs.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/narrative/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### N4.1 Studio Infrastructure & DJ Host Assignment
- (a) Author `RadioStudioRecord` binding broadcasting booth equipment, power requirements, and transmitter wattage.
- (b) Evaluate survivor charisma, humor, and articulation traits to calculate presenter broadcast appeal scores.
- (c) Assign daily DJ shifts in `DutyRosterSystem` ensuring radio stations remain active during peak evening hours.
- (d) Write unit tests verifying that unpowered radio studios immediately cut off active transmissions.

#### N4.2 Playlist Composition & Genre Balance
- (a) Author `BroadcastSchedule` schema organizing 24-hour programming blocks into Music, News, Advice, and Silence.
- (b) Bind music blocks to recovered vinyl albums in `TurntableSystem`, broadcasting authentic audio tracks.
- (c) Calculate listener satisfaction scaling with playlist variety; repeating the same record continuously generates listener boredom.
- (d) Author tests validating that broadcasting accurate weather forecasts boosts listener trust across regional settlements.

#### N4.3 Wasteland Audience Response & Mail Dilemmas
- (a) Intercept listener reception radius driven by antenna tower height and atmospheric solar flare activity.
- (b) Generate postal feedback encounters delivered by nomadic traders containing tips, fan gifts, or hostile warnings.
- (c) Route audience requests to dynamic quest generation (e.g., "Play that pre-war lullaby for our dying elder").
- (d) Author tests proving that fulfilling listener requests unlocks exclusive barter discounts with remote settlements.

#### N4.4 Host Fatigue & Emergency Overrides
- (a) Track host vocal strain and mental fatigue, requiring rest days to prevent chronic hoarseness debuffs.
- (b) Implement emergency interrupt system pre-empting music programming with sirens and civil defense warnings.
- (c) Restore scheduled programming automatically once active emergency alerts are cleared.
- (d) Author characterization tests confirming that charismatic hosts uplift domestic shelter morale during dark winter nights.

---

# 2. QUALITY GATE & VERIFICATION MATRIX

| Gate ID | Target System | Focused Verification Command | Passing Criterion |
| :--- | :--- | :--- | :--- |
| **QG-M1** | Contamination Stigma & Trade | `bash scripts/run_test.sh Ashfall.Core.Tests/Radiation/RadiationSocialBridgeTests.cs` | 100% pass; dosimeter checks and pricing discounts verified |
| **QG-M2** | Romance & Generational Lineage | `bash scripts/run_test.sh Ashfall.Core.Tests/Relations/RomanceLifecycleTests.cs` | 100% pass; courtship progression and inheritance green |
| **QG-M3** | Survivor Education & Apprentices | `bash scripts/run_test.sh Ashfall.Core.Tests/Skills/SurvivorEducationTests.cs` | 100% pass; classroom literacy and apprentice pairing verified |
| **QG-M4** | Acute Disaster Protocols | `bash scripts/run_test.sh Ashfall.Core.Tests/Events/DisasterResponseTests.cs` | 100% pass; atomic protocols and containment verified |
| **QG-N1** | Shelter Archive & Chronicle | `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/ShelterArchiveTests.cs` | 100% pass; event indexing, relic displays, and search green |
| **QG-N2** | Wasteland Cartography | `bash scripts/run_test.sh Ashfall.Core.Tests/World/WastelandCartographyTests.cs` | 100% pass; fog of war and survey quality verified |
| **QG-N3** | Subterranean Tunnel Highway | `bash scripts/run_test.sh Ashfall.Core.Tests/World/UndergroundTunnelTests.cs` | 100% pass; weather immunity and cave-in hazards green |
| **QG-N4** | Shelter Radio Production | `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/ShelterRadioProductionTests.cs` | 100% pass; DJ workflows, playlists, and audience loops green |
| **QG-INT** | Catalog Schema & Integrity | `dotnet test --filter "FullyQualifiedName~CatalogIntegrity"` | 100% pass; all newly added JSON catalogs pass schema |
| **QG-BLD** | Engine-Free Compilation | `dotnet build Ashfall.csproj` | 0 Errors, 0 Warnings across entire C# solution |

---

# 3. HANDOFF & CLOSURE CHECKLIST

- [ ] All 8 tasks (M1–M4, N1–N4) have documented terminal states with evidence recorded in commit history.
- [ ] No Unity dependencies, shims, or references were added to any files.
- [ ] `Assets/Ashfall.Core/` remains strictly engine-free (`netstandard2.1`).
- [ ] All authored JSON data contains `schema_version: 1` and adheres to snake_case field naming.
- [ ] No `System.Random` or unseeded `Guid.NewGuid()` calls exist in deterministic Core logic.
- [ ] No duplicate managers, shadow registries, or parallel save stores were introduced.
- [ ] File claims in `WORKTREE_OWNERSHIP.md` are audited and cleared upon task completion.
- [ ] Focused verification passes for all modified subsystems using `scripts/run_test.sh`.
- [ ] Build succeeds with 0 Errors and 0 Warnings across the entire repository.
