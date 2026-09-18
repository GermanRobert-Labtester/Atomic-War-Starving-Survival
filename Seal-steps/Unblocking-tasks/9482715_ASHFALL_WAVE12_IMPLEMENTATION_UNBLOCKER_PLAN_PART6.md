# ASHFALL — GENERATION WAVE 12 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 6
// SPDX-License-Identifier: MIT

**Document role:** execution-grade Part 6 master plan for Generation Wave 12, derived from the verified post-Wave 12 Part 5 unclaimed corpus ledger and dependency DAG.

**Purpose:** convert the eight highest-priority unsealed deep progression, subversive motive, atmospheric pressure, and perimeter defense plans beyond the Wave 12 Part 5 frontier into an execution-grade unblocking package. Each task provides 20 procedural execution instructions and structured mini-tasks with 4 additional mini-substeps each, eliminating guesswork for coding agents and foremen.

**Wave 12 Part 6 Premise:** Parts 1 through 5 established the complete operational presentation, mechanics, decrees, informational agency, and macro-social life of ASHFALL. Part 6 delivers the **culmination of survival depth and late-campaign continuity**: pre-war codex reconstruction and master blueprints, captive rehabilitation with escape conspiracies, seasonal food smoking and winter nutrition buffers, dynamic campaign epilogue branching, survivor hidden agendas and betrayal investigations, deep weather systemic cascades, contested shelter defense with airlock screening, and layered clothing warmth with wetness insulation.

**Part 6 Execution Set (Exactly 8 Tasks):**
1. **Task K1 — C1[45] Technological Legacy: Pre-War Codex Reconstruction & Master Blueprints (Plan 45-T5)**
2. **Task K2 — C1[45] Captive Dynamics: Rehabilitation, Labor Unions & Escape Conspiracies (Plan 45-T6)**
3. **Task K3 — C1[45] Nutritional Security: Seasonal Food Preservation, Smoking & Winter Buffers (Plan 45-T7)**
4. **Task K4 — C1[45] Campaign Chronicle: Dynamic Epilogue Branching & Legacy Timeline (Plan 45-T8)**
5. **Task L1 — C2[26] Subversive Motives: Hidden Agendas, Clue Tracking & Betrayal Arcs (Plan 132A/132B/132C)**
6. **Task L2 — C2[27] Atmospheric Stress: Weather Deep Gameplay Cascade & Structural Attrition (Plan 135A/135B/135C)**
7. **Task L3 — C2[28] Contested Perimeter: Shelter Defense, Threat Waves & Hatch Screening (Plan 138A/138B/138C)**
8. **Task L4 — C2[29] Thermal Protection: Layered Clothing Warmth, Wetness & Cold Gear (Plan 142A/142B/142C)**

---

# 0. OPERATING CONTRACT

## 0.1 Allowed Terminal States

Every task in Wave 12 Part 6 must reach one of these terminal states:

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

## TASK K1 — C1[45] Technological Legacy: Pre-War Codex Reconstruction & Master Blueprints (Plan 45-T5)

- **Source Plan:** `C-integration-plans/C1_planintegration[45].md` (Task 5)
- **Blocker Class:** ARCHIVE RESEARCH / BLUEPRINT INTEGRITY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Research/`, `Assets/Ashfall.Core/Crafting/`
- **Target Subsystem:** Codex fragment reassembly, master technological blueprints, multi-specialist research assignment, foundry project unlocks

### 20 Procedural Substeps:
1. Review `ResearchSystem.cs`, `CraftingSystem.cs`, and `PrewarArchiveDecryptionSystem.cs` to map technology unlock boundaries.
2. Enforce core invariant: blueprints unlock recipe eligibility in canonical research trees; archive systems never mutate JSON recipe files directly.
3. Claim `Assets/Ashfall.Core/Research/PrewarCodexCoordinator.cs` and `Assets/Ashfall.Core/Research/MasterBlueprint.cs`.
4. Define `CodexFragmentCategory` enum: GeothermalEngineering, HydroponicGeneticMod, GaussBallistics, NuclearMedicine.
5. Implement fragment reassembly logic requiring multiple distinct salvaged technical folios to synthesize one complete Master Blueprint.
6. Connect reassembly projects to the advanced research laboratory room requiring multi-specialist engineering and science staffing.
7. Integrate specialist labor contributions from `DutyRosterSystem` accumulating research work-effort points per shift.
8. Wire unlocked master blueprints to eligibility flags in `FoundryProductionCatalog.cs` and `WorkshopRecipeCatalog.cs`.
9. Prevent duplicate discovery exploits by assigning persistent unique UUIDs to every salvaged archive folio item instance.
10. Route wasteland faction awareness of unlocked blueprints through `Plan131RumorCoordinator` rather than magical global omniscience.
11. Implement high-value blueprint espionage: hostile faction infiltrators may attempt to copy or steal unsealed technical documents.
12. Author `master_blueprints.json` in `Assets/StreamingAssets/Data/` declaring endgame technological wonders with `schema_version: 1`.
13. Enforce schema integrity validation in `CatalogIntegrityValidator.cs` confirming all blueprint rewards point to valid recipe IDs.
14. Ensure master blueprints can be traded with allied advanced factions via `HoldfastTradeSession` for exorbitant resource reserves.
15. Save and restore codex reconstruction progress, assembled blueprints, and unlocked technology tiers inside `ResearchSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Research/PrewarCodexTests.cs` verifying fragment reassembly rules.
17. Verify deterministic seed isolation ensuring identical research teams produce identical breakthrough timing.
18. Validate that tech tree inspect cards render master blueprints with authentic pre-war engineering schematics.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/research/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### K1.1 Fragment Schema & Synthesis Verification
- (a) Define `MasterBlueprintDefinition` schema declaring required folio count, research tier, and unlock tags.
- (b) Author schema-validated technical fragments in `codex_fragments.json` distributed across high-tier wasteland loot tables.
- (c) Provide validation ensuring all fragment combinations cleanly resolve into authored blueprint outputs without loose ends.
- (d) Write unit tests verifying that attempting synthesis with incomplete fragments fails with descriptive missing-folio feedback.

#### K1.2 Multi-Specialist Research Coordination
- (a) Author labor aggregation contract allowing multiple assigned researchers to pool science and engineering skill points.
- (b) Scale daily reconstruction progress by assigned room equipment condition and power grid stability.
- (c) Apply research acceleration bonuses when lead researchers hold certified engineering master ranks.
- (d) Author tests validating that laboratory brownouts or power grid shutdowns halt codex reconstruction progress immediately.

#### K1.3 Canonical Recipe & Construction Eligibility
- (a) Implement `IsBlueprintUnlocked(BlueprintId id)` query method in `PrewarCodexCoordinator`.
- (b) Connect blueprint unlock query directly into recipe craftability checks in `CraftingSystem` and `FoundrySystem`.
- (c) Verify that newly unlocked advanced construction projects appear in shelter building menus without restart.
- (d) Author tests proving that locked blueprints strictly block fabrication of advanced plasma coils and geothermal pumps.

#### K1.4 Faction Espionage & Trade Mechanics
- (a) Emit `MasterBlueprintSynthesizedEvent` across the semantic event bus when a high-tier codex completes.
- (b) Generate diplomatic inquiry events from tech-seeking factions offering alliances or demanding immediate tribute.
- (c) Connect physical blueprint storage lockers in `ShelterRoomSystem` to infiltration theft risk checks.
- (d) Author characterization tests confirming that trading a blueprint duplicate yields massive faction reputation gains.

---

## TASK K2 — C1[45] Captive Dynamics: Rehabilitation, Labor Unions & Escape Conspiracies (Plan 45-T6)

- **Source Plan:** `C-integration-plans/C1_planintegration[45].md` (Task 6)
- **Blocker Class:** CAPTIVE CUSTODY / CONSPIRACY RUNTIME GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Shelter/`, `Assets/Ashfall.Core/Survivors/`
- **Target Subsystem:** Prisoner custody conditions, escape cabal formation, vocational training, loyalty oath citizenship transitions

### 20 Procedural Substeps:
1. Review `ShelterPrisonerSystem.cs`, `AirlockSecuritySystem.cs`, and `DutyRosterSystem.cs` to locate captive custody seams.
2. Enforce core invariant: captive systems manage custody, security, and rehabilitation; citizen status routes strictly through survivor identity.
3. Claim `Assets/Ashfall.Core/Shelter/CaptiveRehabilitationCoordinator.cs` and `Assets/Ashfall.Core/Shelter/EscapeConspiracy.cs`.
4. Define `CaptiveStatus` enum: Detained, SupervisedLabor, RehabilitationCandidate, OathSwornCitizen, HostileInfiltrator.
5. Track detention conditions (cell comfort, daily rations, guard presence, medical care) computing captive resentment.
6. Implement escape conspiracy formation: captives held under brutal conditions with low guard security organize covert escape cabals.
7. Model covert escape planning stages: ToolHoarding, LockPickAcquisition, SentryRoutineStudy, CoordinatedBreakout.
8. Wire active guard duty assignments in `DutyRosterSystem` to conspiracy detection checks, uncovering hidden tools before outbreaks.
9. Implement a structured vocational rehabilitation track allowing trustworthy captives to perform supervised shelter labor.
10. Accumulate rehabilitation loyalty points through fair treatment, medical healing, and voluntary work participation.
11. Build the Loyalty Oath ceremony allowing rehabilitated captives with loyalty above verified thresholds to swear citizenship.
12. Ensure swearing citizenship atomically updates survivor identity records, removing prisoner flags and unlocking full rights.
13. Wire escaped captives to external faction networks: escaping prisoners return to their home faction, leaking shelter layout intelligence.
14. Author `captive_policies.json` in `Assets/StreamingAssets/Data/` declaring ethical treatment guidelines with `schema_version: 1`.
15. Save and restore captive custody states, conspiracy progress, and rehabilitation loyalty within `ShelterSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Shelter/CaptiveRehabilitationTests.cs` verifying breakout detection mechanics.
17. Verify deterministic seed isolation ensuring identical guard patrols produce identical conspiracy discovery chances.
18. Validate that prisoner cell UI cards render captive morale, conspiracy suspicion, and rehabilitation progress bars cleanly.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and strict netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/shelter/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### K2.1 Custody Condition & Resentment Tracking
- (a) Monitor daily food rations, water quality, and cell overcrowding experienced by detained captives.
- (b) Calculate daily resentment delta: starvation and brutality accelerate resentment; humane medical treatment lowers it.
- (c) Enforce detention housing constraints in `ShelterRoomSystem` requiring dedicated detention cells with security doors.
- (d) Write unit tests verifying that humane treatment prevents resentment from crossing radicalization thresholds.

#### K2.2 Escape Conspiracy State Machine
- (a) Model conspiracy lifecycle: `SimmeringDiscontent -> ToolHoarding -> BreakoutPlanning -> Execution`.
- (b) Check shelter security score against captive conspiracy resolve to determine daily discovery risks.
- (c) Emit `EscapePlotUncoveredEvent` across the semantic event bus when alert guards detect contraband lockpicks.
- (d) Author tests validating that unaddressed conspiracies trigger coordinated armed jailbreaks during night cycles.

#### K2.3 Vocational Rehabilitation & Supervised Shifts
- (a) Provide player toggle assigning low-risk captives to supervised agricultural or maintenance work squads.
- (b) Require dedicated guard supervision during labor shifts, pulling active guard staffing from the duty roster.
- (c) Award daily rehabilitation points for productive labor shifts completed without incident.
- (d) Author tests proving that unsupervised labor shifts dramatically increase captive escape and sabotage opportunities.

#### K2.4 Loyalty Oath & Citizenship Transition
- (a) Establish qualification checks: require 90+ days without infractions, zero active conspiracies, and high rehabilitation score.
- (b) Author `SwearCitizenshipOathAction` executing atomic transition from captive status to full shelter survivor citizen.
- (c) Integrate newly naturalized citizens into general duty assignment and room allocation systems.
- (d) Author characterization tests confirming that former captives retain vocational skills acquired during rehabilitation.

---

## TASK K3 — C1[45] Nutritional Security: Seasonal Food Preservation, Smoking & Winter Buffers (Plan 45-T7)

- **Source Plan:** `C-integration-plans/C1_planintegration[45].md` (Task 7)
- **Blocker Class:** FOOD SPOILAGE / WINTER SURVIVAL BUFFER GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Inventory/`, `Assets/Ashfall.Core/Needs/`
- **Target Subsystem:** Food smoking chambers, salt-curing, root cellar insulation, spoilage decay curves, varied dietary immunity

### 20 Procedural Substeps:
1. Review `Inventory.cs`, `FoodRationingCoordinator.cs`, and `NeedsSystem.cs` to map food preservation boundaries.
2. Enforce core invariant: food preservation manages shelf-life and processing; temperature and nutrition route through canonical owners.
3. Claim `Assets/Ashfall.Core/Inventory/FoodPreservationCoordinator.cs` and `Assets/Ashfall.Core/Inventory/PreservedFoodRecord.cs`.
4. Define `PreservationMethod` enum: RawFresh, SmokedWoodfire, SaltCured, DesiccatedJerky, CannedSealed, FrozenDeepCold.
5. Model food spoilage curves where raw meats and vegetables rot within 3–5 days unless preserved or stored in sub-zero chillers.
6. Designate smokehouse and salting workbench stations in `ShelterRoomSystem` consuming wood fuel, salt, and raw foodstuffs.
7. Implement root cellar storage rooms providing passive cooling that doubles raw food shelf-life during temperate seasons.
8. Wire winter sub-zero temperatures from `YearOfAshSystem` and `WeatherSystem` to natural outdoor food freezing buffers.
9. Connect dietary variety (consuming fresh, smoked, and pickled items) to immune system vitality in `DiseaseSystem`.
10. Enforce nutritional deficiencies: surviving exclusively on desiccated jerky and salt rations for 30+ days generates scurvy debuffs.
11. Wire spoiled food batches to compost recycling recipes, converting rotten biomass into fertilizer for hydroponic bays.
12. Implement spoilage odor hazards: rotting food stores generate foul air quality penalties in `ShelterAtmosphereCoordinator`.
13. Author `preserved_food_definitions.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce catalog integrity validation confirming all preserved foodstuffs declare valid caloric and hydration ratings.
15. Save and restore food batch expiration timestamps and smoking station work orders inside `InventorySaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Inventory/FoodPreservationTests.cs` verifying shelf-life progression.
17. Run 120-day winter simulation tests validating that preserved food stocks sustain the shelter through extended blizzards.
18. Validate that inventory stack tooltips display remaining fresh days and preservation method badges clearly.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/inventory/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### K3.1 Preservation Profiles & Spoilage Curves
- (a) Define `FoodPreservationProfile` schema declaring shelf-life multipliers and fuel costs for each preservation technique.
- (b) Implement daily spoilage tick advancing batch decay based on ambient room temperature and air humidity.
- (c) Provide auto-conversion transforming expired food stacks into `SpoiledFoodWaste` items.
- (d) Write unit tests verifying that canned sealed rations remain non-perishable across 400-day simulation runs.

#### K3.2 Smokehouse & Salting Workshop Execution
- (a) Author crafting work orders in `FoodPreservationCoordinator` converting raw fish and game into smoked jerky.
- (b) Validate recipe resource consumption ensuring wood logs and rock salt are deducted from inventory during processing.
- (c) Advance smoking batches over 24-hour curing cycles requiring assigned cook labor in `DutyRosterSystem`.
- (d) Author tests proving that power outages or fuel shortages cleanly pause smokehouse curing cycles without item loss.

#### K3.3 Thermal Cellar & Sub-Zero Freezing
- (a) Query real-time room temperature from `ShelterRoomSystem` and outdoor environmental temperatures from `WeatherSystem`.
- (b) Apply zero-spoilage freeze flags when ambient food storage temperatures fall below 0°C (32°F).
- (c) Calculate rapid thaw spoilage when sudden spring heatwaves warm uninsulated storage rooms.
- (d) Author tests validating that natural winter freezes protect food stocks without requiring electric power.

#### K3.4 Nutritional Variety & Deficiency Health
- (a) Track 14-day rolling dietary intake history per survivor recording distinct food groups consumed.
- (b) Apply nutritional variety bonus reducing disease contraction risks when survivors consume balanced diets.
- (c) Trigger scurvy and micronutrient exhaustion afflictions when diets lack fresh produce for 30 consecutive days.
- (d) Author characterization tests confirming that introducing preserved pickled vegetables clears nutritional deficiencies.

---

## TASK K4 — C1[45] Campaign Chronicle: Dynamic Epilogue Branching & Legacy Timeline (Plan 45-T8)

- **Source Plan:** `C-integration-plans/C1_planintegration[45].md` (Task 8)
- **Blocker Class:** EPILOGUE SCORING / HISTORICAL RETENTION GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Campaign/`, `Assets/Ashfall.Core/Narrative/`
- **Target Subsystem:** Composite legacy score projection, survivor epitaph generation, multi-era timeline chronicle, NG+ inheritance seed

### 20 Procedural Substeps:
1. Review `CampaignCalendar.cs`, `JournalSystem.cs`, and `MoralChoiceSystem.cs` to map historical milestone records.
2. Enforce core invariant: epilogue is a derived chronicle projection; it derives legacy from canonical facts without mutating play state.
3. Claim `Assets/Ashfall.Core/Campaign/CampaignEpilogueEngine.cs` and `Assets/Ashfall.Core/Campaign/LegacyChronicle.cs`.
4. Define `EpilogueBranchKind` enum: WastelandCapital, ReclusiveArchive, FallenBastion, NomadicExodus, ScientificUtopia.
5. Synthesize composite shelter legacy ratings based on population survival, tech unlocked, moral standing, and faction peace.
6. Generate personalized survivor epitaphs summarizing each survivor's life trajectory, highest skill, and ultimate fate.
7. Build a chronological timeline player presenting key historical milestones (Founding, Pandemics, Sieges, Alliances, Victorious Years).
8. Connect completed campaign state to New Game Plus legacy seed generation, creating bounded inheritance profiles.
9. Allow NG+ inheritance of one preserved master blueprint, one cultural tradition, and one legacy survivor lineage trait.
10. Ensure epilogue generation uses deterministic templating, assembling localized authored prose without generative hallucination.
11. Export the completed campaign chronicle as a persistent, human-readable markdown artifact in `campaign_history/`.
12. Wire campaign victory and defeat conditions directly into the epilogue trigger, preventing abrupt unceremonious game-over screens.
13. Author `epilogue_narrative_matrix.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce catalog integrity validation ensuring all epilogue branches map to valid localized narrative passages.
15. Save and restore epilogue records, legacy seeds, and historical chronicle entries cleanly inside `ChronicleSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Campaign/CampaignEpilogueTests.cs` verifying branch selection algorithms.
17. Verify deterministic seed isolation ensuring identical campaign histories produce bitwise-identical epilogue texts.
18. Validate that epilogue UI screens provide smooth timeline scrolling, survivor portrait displays, and audio crescendo cues.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/campaign/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### K4.1 Composite Legacy Scoring Algorithm
- (a) Author `LegacyRatingCalculator` aggregating weighted metrics: SurvivorCount, TechnologyTier, FactionTrust, ReserveStability.
- (b) Implement normalization formula mapping raw campaign achievements into standardized historical legacy ranks (Grade S to F).
- (c) Provide penalty deductions for excessive executions, unresolved epidemics, and reliance on brutal authoritarian decrees.
- (d) Write unit tests verifying that pristine humanitarian runs achieve high legacy grades without score inflation.

#### K4.2 Dynamic Narrative Branch Evaluation
- (a) Match campaign historical event facts against preconditions declared in `epilogue_narrative_matrix.json`.
- (b) Determine macro wasteland epilogue: did the shelter re-civilize the valley, become a closed enclave, or succumb to ash?
- (c) Assemble localized paragraph passages describing the fate of neighboring factions and wasteland trade routes.
- (d) Author tests proving that contradictory epilogue branches (e.g. Total Victory vs Extinction) cannot trigger simultaneously.

#### K4.3 Survivor Fate & Epitaph Generation
- (a) Enumerate all survivors who lived or perished during the campaign from `SurvivorLedger`.
- (b) Generate individualized narrative sentences detailing post-campaign retirement, leadership roles, or heroic burials.
- (c) Highlight notable achievements: longest expedition traveled, most surgeries performed, children raised in the shelter.
- (d) Author characterization tests validating that every survivor receives a respectful, context-accurate historical epitaph.

#### K4.4 New Game Plus Legacy Seed Generation
- (a) Extract player-selected inheritance relics: one Master Blueprint, one heirloom keepsake, and one starting supply cache.
- (b) Author compact, versioned `LegacyProfileSeed` struct serialized independently from prior save states.
- (c) Validate that loading an NG+ legacy profile applies bounded starting bonuses without corrupting normal campaign rules.
- (d) Verify through test simulation that starting an NG+ run with a legacy seed preserves full determinism.

---

## TASK L1 — C2[26] Subversive Motives: Hidden Agendas, Clue Tracking & Betrayal Arcs (Plan 132A/132B/132C)

- **Source Plan:** `C-integration-plans/C2_planintegration[26].md` (Plan 132)
- **Blocker Class:** SECRET MOTIVE / BETRAYAL INVESTIGATION GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Survivors/`, `Assets/Ashfall.Core/Relations/`
- **Target Subsystem:** Persistent hidden agenda layers, behavioral anomalies, player investigation/confrontation, betrayal crisis resolution

### 20 Procedural Substeps:
1. Review `SurvivorTraits.cs`, `SurvivorRelationsSystem.cs`, and `IdeologicalFrictionSystem.cs` to locate trust seams.
2. Enforce core invariant: agendas track secret motives and behavioral clues; they do not replace canonical faction standing or relations.
3. Claim `Assets/Ashfall.Core/Survivors/HiddenAgendaCoordinator.cs` and `Assets/Ashfall.Core/Survivors/SurvivorAgenda.cs`.
4. Define `AgendaType` enum: FactionSympathizer, ResourceThief, CovertSaboteur, CultZealot, SecretDefector, GuardianAngel.
5. Assign hidden agendas deterministically based on survivor background tags, recruitment origin, and campaign seed.
6. Guard against omniscient leaks: hidden agenda fields must remain completely masked from player inspection until unmasked by evidence.
7. Generate behavioral clues during daily routines (e.g. found near radio at 3 AM, unlogged medicine consumption, whispered rumors).
8. Implement an investigation action allowing trusted survivors or detectives to discreetly observe suspects or search bunks.
9. Accumulate objective evidence tokens (Fingerprints, StashedContraband, SecretFrequencies) validating player suspicions.
10. Build a confrontation dialogue mechanic allowing leadership to question suspects with evidence, demanding confession or reform.
11. Implement betrayal crises: unaddressed sabotage agendas trigger electrical fires, airlock tampering, or supply theft during raids.
12. Allow positive hidden agendas (Guardian Angel): secret protectors anonymously perform repairs or comfort depressed orphans.
13. Wire confession and reconciliation outcomes to `MoralChoiceSystem` granting opportunities for redemption or banishment.
14. Author `hidden_agenda_templates.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
15. Enforce catalog integrity validation ensuring all agenda triggers link to valid event vocabulary IDs.
16. Save and restore hidden agenda assignments, clue logs, and investigation progress cleanly within `SurvivorsSaveSection`.
17. Author unit tests in `Ashfall.Core.Tests/Survivors/HiddenAgendaTests.cs` verifying clue emission rules.
18. Verify deterministic seed isolation ensuring identical campaigns assign identical agendas to identical survivor recruits.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/survivors/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### L1.1 Agenda Assignment & Masked State
- (a) Define `SurvivorAgendaRecord` struct storing AgendaType, TargetFaction, SuspicionLevel (0–100), and IsUnmasked.
- (b) Assign agendas upon survivor recruitment using seeded RNG weighted by recruit origin (e.g. ex-raider has higher theft chance).
- (c) Provide strict UI masking ensuring suspect survivor inspect cards display normal character sheets without hints.
- (d) Write unit tests verifying that agenda state serializes cleanly without exposing plain-text leaks in UI models.

#### L1.2 Behavioral Anomaly & Clue Generation
- (a) Intercept daily routine execution in `SurvivorRoutineCoordinator` to emit subtle behavioral anomaly events.
- (b) Generate physical clue items (unaccounted footprints in storage, tampered wiring) during sabotage preparations.
- (c) Record witnessed clues in `ClueRegistry` attached to the suspect's dossier without immediately confirming guilt.
- (d) Author tests validating that high-perception roommates have significantly higher chances of noticing nocturnal anomalies.

#### L1.3 Investigation & Bunk Search Actions
- (a) Provide player command to assign an investigator to shadow a suspect survivor over a 3-day work period.
- (b) Author discrete bunk inspection action searching personal belongings for stashed contraband or foreign faction coins.
- (c) Calculate search success odds based on investigator tradecraft vs suspect concealment skill.
- (d) Author tests proving that searching innocent survivors without cause inflicts heavy morale and trust penalties.

#### L1.4 Confrontation & Resolution Branching
- (a) Build confrontation dialog interface presenting gathered evidence tokens to the suspect.
- (b) Provide multiple resolution pathways: Forgiveness & Oath, Supervised Parole, Exile Banishment, Formal Execution.
- (c) Route resolution consequences to `SurvivorRelationsSystem` and `FactionStanceEngine` based on chosen action.
- (d) Author characterization tests confirming that peaceful reform transforms hostile agendas into fierce shelter loyalty.

---

## TASK L2 — C2[27] Atmospheric Stress: Weather Deep Gameplay Cascade & Structural Attrition (Plan 135A/135B/135C)

- **Source Plan:** `C-integration-plans/C2_planintegration[27].md` (Plan 135)
- **Blocker Class:** WEATHER INTEGRATION / STRUCTURAL CASCADE GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Weather/`, `Assets/Ashfall.Core/Shelter/`
- **Target Subsystem:** Weather projection into thermal load, sump flooding, airlock freeze, expedition route closures, mental-health weather dread

### 20 Procedural Substeps:
1. Review `WeatherSystem.cs`, `VentilationSystem.cs`, and `ShelterRoomSystem.cs` to map environmental cascade boundaries.
2. Enforce core invariant: weather projects physical pressure into existing systems; it does not duplicate shelter simulation state.
3. Claim `Assets/Ashfall.Core/Weather/WeatherCascadeCoordinator.cs` and `Assets/Ashfall.Core/Weather/EnvironmentalPressure.cs`.
4. Define `WeatherSeverityTier` (Light, Moderate, Severe, Extreme, Cataclysmic) scaling with ongoing nuclear winter phases.
5. Project severe blizzards into exterior airlock freeze hazards, requiring mechanical defrosting or de-icing maintenance shifts.
6. Project torrential radioactive rain into sump pump flooding load in `SumpFloodingSystem`, threatening lower machine sublevels.
7. Project ash storms and toxic dust plumes into accelerated air filter degradation in `VentilationSystem`.
8. Project extreme cold waves into thermal heating electrical load, triggering brownouts in overloaded power grids.
9. Connect prolonged severe weather (blizzards lasting >5 days) to seasonal expedition travel route closures on the world map.
10. Route howling weather soundscapes and seismic rumbles to survivor mental health, elevating weather dread and insomnia.
11. Implement predictive telegraphing: weather towers and barometers provide 48-hour advance warnings of incoming atmospheric fronts.
12. Ensure catastrophic storms offer actionable player preparation choices (boarding storm shutters, stocking fuel, recalling teams).
13. Author `weather_cascade_matrix.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce catalog integrity validation confirming all weather event types link to valid atmospheric particle presets.
15. Save and restore active storm pressure vectors, structural wear accumulation, and forecast queues inside `WeatherSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Weather/WeatherCascadeTests.cs` verifying cross-system pressure dispatches.
17. Verify deterministic seed isolation ensuring identical campaign weather seeds generate identical structural stress points.
18. Validate that weather HUD widgets render incoming storm fronts with clear time-to-impact countdowns and severity warnings.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/weather/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### L2.1 Environmental Pressure Projection Contract
- (a) Define `WeatherPressureVector` capturing ThermalDelta, InfiltrationDustLoad, WaterRunoffRate, and WindShear.
- (b) Author calculation engine transforming current `WeatherKind` and wind speed into concrete pressure values.
- (c) Provide read-only projection interfaces consumed by ventilation, heating, plumbing, and airlock systems.
- (d) Write unit tests verifying that clear calm weather generates exactly 0.0 baseline environmental pressure.

#### L2.2 Shelter Sublevel Inundation & Sump Load
- (a) Route torrential rain runoff directly to `SumpFloodingSystem` as gallons-per-minute water inflow.
- (b) Calculate sump pump electrical demand scaling with incoming runoff volume to maintain dry sublevels.
- (c) Trigger sublevel room flooding when runoff inflow exceeds total operational pump discharge capacity.
- (d) Author tests validating that maintaining backup diesel sump pumps prevents lower workshop flooding during hurricanes.

#### L2.3 Ventilation Dust Clogging & Filter Attrition
- (a) Apply particulate dust load to exterior air intake scrubbers during radioactive ash fallout storms.
- (b) Accelerate intake filter condition wear in `VentilationSystem` proportional to storm dust density.
- (c) Trigger toxic dust bypass into shelter interior air supplies when intake filters deteriorate past zero condition.
- (d) Author tests proving that sealing intake dampers during peak ash storms protects internal air quality at the cost of oxygen.

#### L2.4 World Route Impassability & Expedition Recall
- (a) Map extreme blizzard and flood conditions to travel cost multipliers on wasteland topological road edges.
- (b) Mark high-elevation mountain pass nodes as impassable when snowpack depths exceed critical thresholds.
- (c) Emit emergency recall recommendations to active wasteland expeditions caught within incoming storm paths.
- (d) Author characterization tests confirming that stranded expeditions suffer severe hypothermia if caught unprepared.

---

## TASK L3 — C2[28] Contested Perimeter: Shelter Defense, Threat Waves & Hatch Screening (Plan 138A/138B/138C)

- **Source Plan:** `C-integration-plans/C2_planintegration[28].md` (Plan 138)
- **Blocker Class:** SHELTER PERIMETER / VISITOR SCREENING GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Combat/`, `Assets/Ashfall.Core/Airlock/`
- **Target Subsystem:** Defense-readiness projection, dynamic raid threat calculation, airlock visitor quarantine/screening, breach defense combat

### 20 Procedural Substeps:
1. Review `AirlockSecuritySystem.cs`, `TacticalCombatSystem.cs`, and `DutyRosterSystem.cs` to map perimeter defense hooks.
2. Enforce core invariant: defense readiness coordinates threats and screening; combat resolution routes strictly through tactical combat.
3. Claim `Assets/Ashfall.Core/Combat/ShelterDefenseCoordinator.cs` and `Assets/Ashfall.Core/Airlock/VisitorScreeningStation.cs`.
4. Define `PerimeterThreatLevel` enum: Peaceful, Guarded, HeightenedAlert, ImminentAssault, BreachInProgress.
5. Calculate composite shelter `DefenseReadiness` aggregating armed guard sentries, perimeter barricades, searchlights, and automated turrets.
6. Implement dynamic raid threat generation scaled by shelter wealth, food reserves, visibility notoriety, and faction hostility.
7. Build structured visitor intake protocols at the exterior hatch: DecontaminationScrub, WeaponSurrender, MedicalInspection, Interrogation.
8. Wire visitor screening checks to the discovery of hidden infections in `DiseaseSystem` and concealed contraband in `Inventory`.
9. Handle visitor intake disposition choices: GrantTemporarySanctuary, AdmitAsResident, BarterAtAirlock, RefuseEntry, DetainSuspect.
10. Connect perimeter breaches to `TacticalCombatSystem` instantiating tactical skirmishes in the airlock and entrance vestibule.
11. Implement defensive fortification upgrades (reinforced blast doors, firing embrasures, barbwire aprons) in `ShelterRoomSystem`.
12. Ensure defeated raid waves leave salvageable weaponry, ammunition, and captive prisoners at the outer perimeter gate.
13. Wire civilian evacuation alarms moving unarmed children and sick survivors to deep fortified bunker retreats during assaults.
14. Author `perimeter_defense_upgrades.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
15. Enforce catalog integrity validation ensuring all fortification items map to valid construction and repair recipes.
16. Save and restore perimeter defense condition, turret ammo counts, and screening queue records within `AirlockSaveSection`.
17. Author unit tests in `Ashfall.Core.Tests/Combat/ShelterDefenseTests.cs` verifying threat calculation formulas.
18. Verify deterministic seed isolation ensuring identical raid attacks produce identical combat entry placements.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/combat/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### L3.1 Defense Readiness & Threat Projection
- (a) Author `DefenseReadinessCalculator` summing sentry marksmanship skills, weapon tiers, and barricade hitpoints.
- (b) Calculate raid probability modified by shelter notoriety, stockpiled food wealth, and active faction wars.
- (c) Provide clear UI readiness badges displaying defensive score vs estimated regional bandit threat tiers.
- (d) Write unit tests verifying that manning all sentry posts significantly lowers enemy surprise assault chances.

#### L3.2 Hatch Screening & Visitor Quarantine
- (a) Model visitor screening pipeline executing automated decontamination showers and physical body frisks.
- (b) Detect concealed weapons and stolen tech items using sentry perception checks against visitor tradecraft.
- (c) Screen incoming refugees for latent radiation sickness or contagious bacterial infections before door opening.
- (d) Author tests proving that admitting unscreened plague victims directly into common dorms triggers shelter epidemics.

#### L3.3 Barricade Integrity & Turret Fire Support
- (a) Attach structural hitpoints to entrance blast doors and sandbag barricades in `ShelterRoomSystem`.
- (b) Wire automated perimeter turrets to electrical power grids and physical ammunition stores in inventory.
- (c) Deduct ammunition rounds dynamically during defensive engagements, disabling turrets when magazines empty.
- (d) Author tests validating that fortified blast doors absorb initial explosive breaches, buying time for sentry deployment.

#### L3.4 Vestibule Breach & Retreat Coordination
- (a) Transition battle state to InteriorVestibuleSkirmish when outer blast doors suffer structural breach.
- (b) Route non-combatant survivors to designated secure bunker rooms via `SurvivorRoutineCoordinator`.
- (c) Process defensive melee and firearm combat using canonical rules in `TacticalCombatSystem`.
- (d) Author characterization tests confirming that repelling an assault triggers morale surges and prisoner capture opportunities.

---

## TASK L4 — C2[29] Thermal Protection: Layered Clothing Warmth, Wetness & Cold Gear (Plan 142A/142B/142C)

- **Source Plan:** `C-integration-plans/C2_planintegration[29].md` (Plan 142)
- **Blocker Class:** EQUIPMENT INSULATION / WETNESS THERMAL GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Inventory/`, `Assets/Ashfall.Core/Needs/`
- **Target Subsystem:** Clothing warmth insulation values, wetness immersion penalties, layered outerwear slots, hypothermia prevention

### 20 Procedural Substeps:
1. Review `Inventory.cs`, `ItemInstance.cs`, and `NeedsSystem.cs` to locate survivor warmth and equipment handling.
2. Enforce core invariant: clothing warmth contributes to survivor warmth calculation; it does not duplicate equipment durability.
3. Claim `Assets/Ashfall.Core/Inventory/ClothingWarmthCoordinator.cs` and `Assets/Ashfall.Core/Inventory/ThermalInsulation.cs`.
4. Define `ClothingLayer` enum: Undergarment, ThermalBase, ProtectiveMid, HeavyOuterwear, Headwear, Handwear, Footwear.
5. Author authored insulation values in `clothing_thermal_properties.json` for coats, parkas, thermal underwear, and boots.
6. Calculate aggregate survivor insulation summing equipped garments across distinct non-conflicting equipment slots.
7. Implement wetness tracking: exposure to rain, snow, or sump flooding accumulates survivor clothing moisture (0.0 to 1.0).
8. Apply moisture degradation: soaked clothing loses up to 80% of its thermal insulation value and accelerates hypothermia.
9. Wire body drying mechanics: standing near shelter heating stoves or radiators gradually reduces clothing moisture.
10. Integrate clothing condition wear: ripped or deteriorated garments suffer proportional reductions in thermal protection.
11. Wire effective warmth retention directly into hourly body temperature and warmth decay calculations in `NeedsSystem`.
12. Prevent layering exploits by enforcing strict slot rules: survivors cannot equip multiple heavy parkas simultaneously.
13. Author nuclear-winter extreme cold gear (FurLinedParkas, HeatedVests, InsulatedMukluks) with standard `schema_version: 1`.
14. Enforce catalog integrity validation confirming all clothing items declare valid layer classifications and thermal tags.
15. Save and restore survivor clothing moisture values and thermal state cleanly within `InventorySaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Inventory/ClothingWarmthTests.cs` verifying insulation and moisture calculations.
17. Verify deterministic seed isolation ensuring identical weather exposure produces identical clothing wetting rates.
18. Validate that equipment inspect cards display thermal warmth ratings (CLO values) and current moisture percentages.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/inventory/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### L4.1 Thermal Insulation Schema & Layer Slots
- (a) Define `ClothingThermalRecord` struct storing `InsulationRating` (CLO 0.1 to 4.0), `WaterResistance`, and `LayerSlot`.
- (b) Author schema-validated thermal data in `clothing_thermal_properties.json` covering standard wasteland apparel.
- (c) Provide layering validation ensuring each survivor equips at most one garment per designated layer slot.
- (d) Write unit tests verifying that all authored clothing entries load cleanly without missing thermal attributes.

#### L4.2 Wetness Accumulation & Thermal Degradation
- (a) Model moisture absorption rate driven by precipitation intensity from `WeatherSystem` and swimming/flooding immersion.
- (b) Calculate thermal conductivity penalty: wet garments dramatically conduct body heat away into freezing ambient air.
- (c) Emit `SurvivorSoakedEvent` when moisture surpasses 0.75, warning the player of imminent hypothermia risks.
- (d) Author tests validating that wool and treated waterproof synthetic coats resist moisture significantly longer than cotton.

#### L4.3 Shelter Stoves & Garment Drying Cycles
- (a) Detect survivor proximity to functional heating stoves, fireplaces, and radiators in `ShelterRoomSystem`.
- (b) Apply daily or hourly moisture evaporation ticks, returning wet clothing to dry 상태 over 2 to 4 hours of rest.
- (c) Enable drying racks in laundry and boiler rooms allowing off-duty survivors to hang wet gear overnight.
- (d) Author tests proving that sleeping in dry thermal clothes completely halts overnight hypothermia progression.

#### L4.4 Needs Integration & Hypothermia Prevention
- (a) Feed effective net insulation into hourly warmth decay formulas in `NeedsSystem`.
- (b) Neutralize sub-zero environmental temperature debuffs when total equipped insulation exceeds required regional CLO thresholds.
- (c) Trigger frostbite and shivering affliction states when uninsulated survivors travel through freezing blizzards.
- (d) Author characterization tests confirming that properly equipped expedition scouts survive deep arctic winter treks without warmth failure.

---

# 2. QUALITY GATE & VERIFICATION MATRIX

| Gate ID | Target System | Focused Verification Command | Passing Criterion |
| :--- | :--- | :--- | :--- |
| **QG-K1** | Pre-War Codex & Master Blueprints | `bash scripts/run_test.sh Ashfall.Core.Tests/Research/PrewarCodexTests.cs` | 100% pass; fragment synthesis and recipe unlocking verified |
| **QG-K2** | Captive Dynamics & Conspiracies | `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/CaptiveRehabilitationTests.cs` | 100% pass; escape plots, custody, and oath transitions green |
| **QG-K3** | Food Preservation & Winter Buffers | `bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/FoodPreservationTests.cs` | 100% pass; shelf-life decay and smokehouse recipes verified |
| **QG-K4** | Campaign Epilogue & Legacy | `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/CampaignEpilogueTests.cs` | 100% pass; dynamic narrative branches and NG+ seeds green |
| **QG-L1** | Subversive Agendas & Betrayal | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/HiddenAgendaTests.cs` | 100% pass; clue generation, investigation, and confession green |
| **QG-L2** | Weather Atmospheric Cascades | `bash scripts/run_test.sh Ashfall.Core.Tests/Weather/WeatherCascadeTests.cs` | 100% pass; pressure vectors, flooding, and filter wear verified |
| **QG-L3** | Shelter Defense & Screening | `bash scripts/run_test.sh Ashfall.Core.Tests/Combat/ShelterDefenseTests.cs` | 100% pass; readiness scores, screening queues, and breach green |
| **QG-L4** | Clothing Warmth & Wetness | `bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/ClothingWarmthTests.cs` | 100% pass; insulation stacking, moisture decay, and drying green |
| **QG-INT** | Catalog Schema & Integrity | `dotnet test --filter "FullyQualifiedName~CatalogIntegrity"` | 100% pass; all newly added JSON catalogs pass schema |
| **QG-BLD** | Engine-Free Compilation | `dotnet build Ashfall.csproj` | 0 Errors, 0 Warnings across entire C# solution |

---

# 3. HANDOFF & CLOSURE CHECKLIST

- [ ] All 8 tasks (K1–K4, L1–L4) have documented terminal states with evidence recorded in commit history.
- [ ] No Unity dependencies, shims, or references were added to any files.
- [ ] `Assets/Ashfall.Core/` remains strictly engine-free (`netstandard2.1`).
- [ ] All authored JSON data contains `schema_version: 1` and adheres to snake_case field naming.
- [ ] No `System.Random` or unseeded `Guid.NewGuid()` calls exist in deterministic Core logic.
- [ ] No duplicate managers, shadow registries, or parallel save stores were introduced.
- [ ] File claims in `WORKTREE_OWNERSHIP.md` are audited and cleared upon task completion.
- [ ] Focused verification passes for all modified subsystems using `scripts/run_test.sh`.
- [ ] Build succeeds with 0 Errors and 0 Warnings across the entire repository.
