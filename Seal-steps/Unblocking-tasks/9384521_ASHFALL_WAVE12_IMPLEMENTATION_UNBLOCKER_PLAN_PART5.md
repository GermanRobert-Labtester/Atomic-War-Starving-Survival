# ASHFALL — GENERATION WAVE 12 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 5
// SPDX-License-Identifier: MIT

**Document role:** execution-grade Part 5 master plan for Generation Wave 12, derived from the verified post-Wave 12 Part 4 unclaimed corpus ledger and dependency DAG.

**Purpose:** convert the eight highest-priority unsealed human-dynamic, social-friction, environmental-character, and end-to-end campaign soak plans beyond the Wave 12 Part 4 frontier into an execution-grade unblocking package. Each task provides 20 procedural execution instructions and structured mini-tasks with 4 additional mini-substeps each, eliminating guesswork for coding agents and foremen.

**Wave 12 Part 5 Premise:** Parts 1 through 4 stabilized presentation, game mechanics, governance decrees, and informational agency. Part 5 attacks the **macro-social and environmental life of the shelter and wasteland**: seasonal human migration and refugee columns, interpersonal survivor friction and mediation, external shelter notoriety and wasteland standing, material keepsake attachment and inheritance, physical exercise adaptation and deconditioning, sensory shelter atmosphere and ambiance, trauma catharsis with acoustic direction, and micro-location encounter UX backed by a 120-day deterministic campaign fuzz certification.

**Part 5 Execution Set (Exactly 8 Tasks):**
1. **Task I1 — C1[37] Seasonal Human Dynamics: Refugee Flows, Trader Circuits & Faction Relocation (Plan 199A/199B/199C)**
2. **Task I2 — C1[38] Interpersonal Friction: Grievance Claims, Mediation & Social Reconciliation (Plan 202A/202B/202C)**
3. **Task I3 — C1[39] Wasteland Standing: Shelter Reputation, External Notoriety & Diplomatic Perception (Plan 207A/207B/207C)**
4. **Task I4 — C1[40] Material Attachment: Survivor Keepsakes, Personal Ownership & Inheritance (Plan 210A/210B/210C)**
5. **Task J1 — C1[41] Physical Conditioning: Survivor Exercise, Training Adaptation & Deconditioning (Plan 216A/216B/216C)**
6. **Task J2 — C1[42] Environmental Character: Shelter Atmosphere, Sensory Ambiance & Read-Model (Plan 220A/220B/220C)**
7. **Task J3 — C1[43] Psychological Catharsis: Mental-Health UI, Acoustic Director & Archive Decryption (Plan 220/52/53/62)**
8. **Task J4 — C1[44] Wasteland Encounter UX: Micro-Location UI, Turntable Broadcast & 120-Day Fuzz (Plan 224/49/02-09)**

---

# 0. OPERATING CONTRACT

## 0.1 Allowed Terminal States

Every task in Wave 12 Part 5 must reach one of these terminal states:

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

## TASK I1 — C1[37] Seasonal Human Dynamics: Refugee Flows, Trader Circuits & Faction Relocation (Plan 199A/199B/199C)

- **Source Plan:** `C-integration-plans/C1_planintegration[37].md` (Plan 199)
- **Blocker Class:** SEASONAL WORLD DRIFT / REFUGEE FLOW GAP
- **Canonical Owner:** `Assets/Ashfall.Core/World/`, `Assets/Ashfall.Core/Caravans/`
- **Target Subsystem:** Seasonal human migration episodes, refugee intake triggers, nomadic merchant circuits, world route traversal

### 20 Procedural Substeps:
1. Review `WildlifeMigrationSystem.cs`, `TravelingCaravanSystem.cs`, and `WeatherSystem.cs` to map seasonal simulation hooks.
2. Enforce core invariant: migration owns group travel progression and arrival timing; it does not duplicate settlement population or caravan stock.
3. Claim `Assets/Ashfall.Core/World/SeasonalMigrationCoordinator.cs` and `Assets/Ashfall.Core/World/MigrationEpisode.cs`.
4. Define `MigrationGroupType` enum: RefugeeColumn, NomadicTraderCircuit, FactionRelocation, SeasonalPilgrims.
5. Author `migration_routes.json` declaring seasonal corridors across canonical `WorldMapTopology` nodes.
6. Connect migration activation triggers to seasonal transitions in `CampaignCalendar.cs` and nuclear winter thresholds.
7. Implement group route traversal calculating daily progression based on terrain roughness, weather attrition, and radiation storms.
8. Wire refugee group arrival at the shelter to `AirlockSecuritySystem` triggering structured intake dilemmas.
9. Connect nomadic merchant arrivals directly to `HoldfastTradeSession` enabling temporary rare goods markets.
10. Route faction operational relocation to `FactionBranchCoordinator` shifting regional patrol density and outpost control.
11. Implement route interception mechanics allowing expeditions to encounter migrating columns in the wasteland.
12. Ensure migration groups consume food and sustain casualties during extreme blizzards without spawning duplicate survivor entities.
13. Author data-driven migration templates in `migration_templates.json` with standard `schema_version: 1`.
14. Enforce schema validation in `CatalogIntegrityValidator.cs` confirming all route endpoints link to valid location IDs.
15. Save and restore active migration episodes and waypoint progress cleanly inside `WorldSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/World/SeasonalMigrationTests.cs` verifying seasonal trigger timing.
17. Run multi-year headless simulations proving that seasonal migration corridors cycle deterministically without entity leaks.
18. Verify that completed migrations retire group records cleanly without leaving orphaned references on the world map.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/world/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### I1.1 Corridor Definition & Topology Binding
- (a) Define `MigrationCorridor` struct mapping ordered sequences of `LocationId` nodes on the canonical wasteland map.
- (b) Author schema-validated migration paths in `migration_routes.json` representing summer and winter travel passes.
- (c) Validate that all corridor nodes exist and maintain traversable travel connections in `WorldMapTopology`.
- (d) Write unit tests verifying that corrupted or circular corridors are rejected during catalog integrity checks.

#### I1.2 Seasonal Trigger & Episode Scheduling
- (a) Monitor seasonal boundary events emitted by `CampaignCalendar` to initiate seasonal migration cycles.
- (b) Evaluate nuclear-winter temperature thresholds in `WeatherSystem` to trigger emergency southward refugee flights.
- (c) Instantiate `MigrationEpisode` records assigning deterministic group size, supply reserves, and destination.
- (d) Author tests verifying that unseasonal mild weather suppresses emergency refugee flight episodes.

#### I1.3 Group Route Traversal & Hazard Attrition
- (a) Calculate daily movement steps along active corridors modified by seasonal mud, snowpack, or fallout plumes.
- (b) Apply attrition checks reducing migrant group supply counters during passage through high-hazard nodes.
- (c) Emit `MigrationInterceptionPossibleEvent` when player expedition paths intersect active migration coordinates.
- (d) Author tests proving that migrant groups unable to reach shelter before extreme blizzard onset suffer dissolution.

#### I1.4 Arrival Dispatch & Downstream Seam Handoff
- (a) Dispatch completed refugee journeys to `AirlockSecuritySystem` as pending intake admission choices.
- (b) Trigger scheduled trading sessions in `TravelingCaravanSystem` upon nomadic merchant arrival at shelter gates.
- (c) Update regional faction influence in `FactionBranchCoordinator` when faction relocation columns reach their fortresses.
- (d) Verify through test harness that arrival handoffs cleanly terminate the migration episode without lingering timers.

---

## TASK I2 — C1[38] Interpersonal Friction: Grievance Claims, Mediation & Social Reconciliation (Plan 202A/202B/202C)

- **Source Plan:** `C-integration-plans/C1_planintegration[38].md` (Plan 202)
- **Blocker Class:** INTERPERSONAL CONFLICT / SOCIAL VOLATILITY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Survivors/`, `Assets/Ashfall.Core/Relations/`
- **Target Subsystem:** Non-ideological dispute episodes, grievance tracking, mediation settlements, roommate/workload friction

### 20 Procedural Substeps:
1. Review `IdeologicalFrictionSystem.cs`, `SurvivorRelationsSystem.cs`, and `SurvivorTraits.cs` to locate social conflict seams.
2. Enforce core invariant: conflict owns grievance claims and escalation episodes; it does not duplicate affinity scores or survivor traits.
3. Claim `Assets/Ashfall.Core/Relations/InterpersonalConflictCoordinator.cs` and `Assets/Ashfall.Core/Relations/GrievanceClaim.cs`.
4. Define `GrievanceCategory` enum: UnequalRations, RoommateNoise, WorkloadDispute, BrokenPromise, PersonalSlight, TheftAccusation.
5. Implement fact-driven grievance generation requiring an objective triggering event (e.g., missed meal, forced overtime, cramped bunk).
6. Bind grievance claims to concrete participant survivor IDs, timestamping creation day and perceived severity.
7. Construct an escalation state machine transitioning disputes from Simmering to VerbalArgument, PublicConfrontation, and PhysicalBrawl.
8. Enforce trait modulation where volatile traits accelerate escalation while diplomatic traits expand de-escalation windows.
9. Build a mediation intervention system allowing high-Charisma survivors or the shelter leader to preside over dispute hearings.
10. Provide structured resolution outcomes: FormalApology, MaterialRestitution, ShiftSeparation, BunkReassignment, OfficialSanction.
11. Route successful resolutions to `SurvivorRelationsSystem` restoring lost affinity and clearing active grievance claims.
12. Route unmediated physical brawls to `SurvivorInjuryCoordinator` instantiating minor contusions or lacerations via canonical medical paths.
13. Author `grievance_templates.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce that grievances decay naturally over 60 campaign days if survivors are physically separated in different rooms.
15. Serialize active dispute episodes and pending grievances within `RelationsSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Relations/InterpersonalConflictTests.cs` verifying escalation thresholds.
17. Verify deterministic seed isolation ensuring identical shelter social stressors produce identical dispute resolutions.
18. Validate that dispute notifications present legible cause-and-effect explanations without journal clutter.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and strict null safety.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/survivors/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### I2.1 Fact-Driven Grievance Extraction
- (a) Intercept food allocation events in `FoodRationingCoordinator` to detect survivors skipped during meal distribution.
- (b) Query `DutyRosterSystem` for consecutive double-shift assignments creating severe workload disparity.
- (c) Monitor room assignment changes in `ShelterRoomSystem` detecting incompatible roommate personality pairings.
- (d) Write unit tests verifying that grievances generate only when objective triggering facts occur.

#### I2.2 Dispute Escalation State Machine
- (a) Model dispute progression: `Simmering -> VerbalArgument -> Confrontation -> PhysicalBrawl`.
- (b) Calculate daily escalation probabilities influenced by survivor stress levels and room overcrowding.
- (c) Apply dampening factors when mutual friends or high-reputation survivors share the common room.
- (d) Author tests validating that calm, well-fed survivors rarely escalate disputes past minor verbal arguments.

#### I2.3 Leadership Mediation & Restitution Protocols
- (a) Provide leadership command interface allowing players to assign a mediator to active dispute episodes.
- (b) Calculate mediation success rates based on mediator negotiation skill and both parties' respect for authority.
- (c) Enforce restitution agreements transferring apology items or assigning compensatory work shifts.
- (d) Author tests proving that successful mediation clears active grievances and restores baseline social affinity.

#### I2.4 Reconciliation & Spatial Separation Decay
- (a) Track daily physical separation: if disputing survivors occupy different rooms/shifts, apply natural decay.
- (b) Implement natural expiration removing grievances older than 60 campaign days if no new offenses occur.
- (c) Emit `GrievanceReconciledEvent` across the semantic event bus when lingering resentment fully dissipates.
- (d) Verify through test simulation that spatial separation effectively prevents simmering disputes from flaring into violence.

---

## TASK I3 — C1[39] Wasteland Standing: Shelter Reputation, External Notoriety & Diplomatic Perception (Plan 207A/207B/207C)

- **Source Plan:** `C-integration-plans/C1_planintegration[39].md` (Plan 207)
- **Blocker Class:** REPUTATION PERCEPTION / EXTERNAL IDENTITY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Factions/`, `Assets/Ashfall.Core/Campaign/`
- **Target Subsystem:** Shelter notoriety vectors, wasteland external perception, visitor reception, trade posture

### 20 Procedural Substeps:
1. Review `FactionStanceEngine.cs`, `AirlockSecuritySystem.cs`, and `HoldfastTradeSession.cs` to map external perception boundaries.
2. Enforce core invariant: reputation owns external perception and public notoriety; it does not replace faction trust or moral choice truth.
3. Claim `Assets/Ashfall.Core/Factions/ShelterReputationCoordinator.cs` and `Assets/Ashfall.Core/Factions/ReputationVector.cs`.
4. Define public reputation dimensions: SanctuaryHospitality, MilitaryDeterrence, CommercialFairness, RuthlessPragmatism, TechProwess.
5. Implement an external information horizon: deeds only affect reputation when witnesses, traders, or radio broadcasts escape to report them.
6. Model `WastelandNotoriety` as an awareness scalar (0.0 to 1.0) dictating how widely the shelter's deeds are recognized.
7. Wire high `SanctuaryHospitality` to increased refugee arrivals and humanitarian trade caravan visits in `SeasonalMigrationSystem`.
8. Wire high `MilitaryDeterrence` to reduced bandit raid frequency and increased demands for tribute rather than assault.
9. Wire high `CommercialFairness` to preferential barter exchange ratios and exclusive blueprint offerings in `MarketSystem`.
10. Allow malicious reputation distortion via enemy faction radio smear campaigns integrated with `PropagandaCoordinator`.
11. Implement reputation evidence decay where routine commercial transactions fade while legendary defense stands endure for decades.
12. Ensure public reputation tags display on the world map and inspect panels as diegetic titles (e.g., "The Iron Bastion", "Haven of Mercy").
13. Author `reputation_dimensions.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce integrity checks in `CatalogIntegrityValidator.cs` confirming all reputation tags reference valid localized strings.
15. Save and restore reputation vectors, evidence records, and notoriety scores cleanly inside `FactionsSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Factions/ShelterReputationTests.cs` verifying notoriety progression.
17. Run 400-day simulation tests validating that reputation tags stabilize deterministically without runaway inflation.
18. Verify that secret atrocities committed inside sealed bunkers with zero surviving witnesses generate zero external reputation.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/factions/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### I3.1 Perception Dimensions & Notoriety Model
- (a) Author `ReputationVector` struct capturing typed dimension scores bounded between -100.0 and +100.0.
- (b) Implement notoriety progression scaling from 0.0 (Unknown Hermit Vault) to 1.0 (Legendary Wasteland Capital).
- (c) Provide dimension classification mapping numeric scores to descriptive titles (e.g., Ruthless Pragmatism > 70 = "The Cold Reapers").
- (d) Write unit tests verifying that reputation updates maintain strict mathematical bounds and zero NaN leaks.

#### I3.2 External Information Propagation Horizon
- (a) Implement witness verification checking whether neutral NPCs, surviving raiders, or traders witnessed shelter actions.
- (b) Route information transmission through caravan arrivals and radio broadcasts in `VerdictRadioSystem`.
- (c) Suppress reputation changes for internal bunker executions or events occurring behind locked airlocks with no survivors.
- (d) Author characterization tests confirming that actions taken without external witnesses produce zero reputation drift.

#### I3.3 Visitor & Trade Posture Calibration
- (a) Connect high Hospitality reputation to increased recruitment applicant pools in `RecruitmentSystem`.
- (b) Apply Commercial Fairness reputation bonuses reducing merchant trade margins in `HoldfastTradeSession`.
- (c) Apply Military Deterrence thresholds causing weak scavenger gangs to flee rather than attack shelter outposts.
- (d) Author tests validating that high notoriety magnifies both positive alliances and hostile faction jealousy.

#### I3.4 Reputation Decay & Historical Monumentality
- (a) Implement differentiated decay rates: minor barter events decay in 15 days; defended siege battles endure for 300 days.
- (b) Build reputation stabilization logic preventing rapid oscillating flips between benevolent and ruthless alignments.
- (c) Emit `ReputationMilestoneAchievedEvent` across the semantic event bus when major reputation titles are unlocked.
- (d) Verify through test simulation that long-term reputation records persist accurately across multi-generational runs.

---

## TASK I4 — C1[40] Material Attachment: Survivor Keepsakes, Personal Ownership & Inheritance (Plan 210A/210B/210C)

- **Source Plan:** `C-integration-plans/C1_planintegration[40].md` (Plan 210)
- **Blocker Class:** PERSONAL PROPERTY / SENTIMENTAL CONTINUITY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Inventory/`, `Assets/Ashfall.Core/Survivors/`
- **Target Subsystem:** Sentimental item binding, private trinket lockers, bereavement heirlooms, gift exchanges

### 20 Procedural Substeps:
1. Review `Inventory.cs`, `ItemInstance.cs`, and `EquipmentConditionSystem.cs` to map item ownership boundaries.
2. Enforce core invariant: personal belongings represent ownership metadata over canonical item instances; no shadow inventory container.
3. Claim `Assets/Ashfall.Core/Inventory/PersonalBelongingsCoordinator.cs` and `Assets/Ashfall.Core/Inventory/BelongingRecord.cs`.
4. Define `SentimentalSignificance` enum: FamilyHeirloom, FallenComradeTrinket, VictoryTrophy, RomanticGift, ChildhoodKeepsake.
5. Attach `BelongingRecord` to canonical item instance UUIDs storing OwnerSurvivorId, AcquisitionDay, and EmotionalAttachment (1–10).
6. Implement automatic protection flags preventing automated crafting teardown or merchant barter of claimed keepsakes.
7. Allow survivors to voluntarily designate favorite weapons, tools, or clothing items through sustained usage.
8. Wire possession of sentimental items to stress recovery bonuses during daily sleep in `NeedsSystem`.
9. Implement bereavement inheritance protocols: when a survivor dies, claimed keepsakes pass to designated kin or bonded partners.
10. Handle emergency requisition: players may override personal ownership to scrap or equip a keepsake, triggering grief and morale penalties.
11. Build a gift-giving mechanic allowing survivors with high relationship affinity to exchange sentimental items.
12. Wire stolen or confiscated keepsakes to the grievance generation engine in `InterpersonalConflictCoordinator`.
13. Author `sentimental_items.json` declaring unique authored relics with standard `schema_version: 1`.
14. Enforce that narrative artifacts (letters, faded photographs, lockets) occupy lightweight non-stackable item slots.
15. Serialize belonging records and sentimental links within `InventorySaveSection` with full reference integrity.
16. Author unit tests in `Ashfall.Core.Tests/Inventory/PersonalBelongingsTests.cs` verifying inheritance dispatch.
17. Verify deterministic seed isolation ensuring identical gift exchanges occur under identical relationship states.
18. Validate that survivor inspect UI clearly highlights personal belongings with a sentimental ribbon badge.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and strict null safety.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/inventory/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### I4.1 Ownership Claim & Protection Flags
- (a) Map `BelongingRecord` to existing item UUIDs in `Inventory` without duplicating item base statistics.
- (b) Enforce barter lock: merchant screens disallow trading claimed keepsakes unless player explicitly confirms unlock.
- (c) Enforce crafting lock: recycling and dismantling benches reject claimed belongings as salvage ingredients.
- (d) Write unit tests verifying that claimed keepsakes cannot be auto-consumed or auto-sold by batch operations.

#### I4.2 Sentimental Significance & Stress Relief
- (a) Calculate emotional attachment values (1 to 10) scaling with item age, survivor survival triumphs, and romance bonds.
- (b) Apply passive stress reduction ticks during overnight rest when a survivor sleeps with their favored keepsake in bunk storage.
- (c) Amplify attachment when a keepsake is carried during successful defense of the shelter or harrowing expeditions.
- (d) Author tests validating that losing a prized keepsake (durability zero or stolen) triggers acute grief stress spikes.

#### I4.3 Bereavement Inheritance & Estate Disposition
- (a) Intercept survivor death events emitted by `SurvivorMortalityCoordinator`.
- (b) Evaluate estate priority: transfer belongings first to spouse/partner, second to adult children, third to closest friend.
- (c) Transfer unclaimed estate belongings to the shelter memorial display or communal storage with legacy tags.
- (d) Author characterization tests confirming that inheritance transfers execute seamlessly without duplicating item entities.

#### I4.4 Requisition Friction & Gift Exchanges
- (a) Implement emergency player requisition command allowing confiscation of survivor belongings during crises.
- (b) Calculate relationship and trust penalties inflicted upon leadership when prized possessions are forcibly stripped.
- (c) Enable peer gift-giving actions when survivor affinity surpasses +80, granting mutual morale boosts.
- (d) Verify through test harness that voluntary gifts cleanly update owner UUID references without item loss.

---

## TASK J1 — C1[41] Physical Conditioning: Survivor Exercise, Training Adaptation & Deconditioning (Plan 216A/216B/216C)

- **Source Plan:** `C-integration-plans/C1_planintegration[41].md` (Plan 216)
- **Blocker Class:** PHYSIOLOGICAL ADAPTATION / PHYSICAL CAPACITY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Survivors/`, `Assets/Ashfall.Core/Needs/`
- **Target Subsystem:** Physical workout regimens, muscle hypertrophy/endurance adaptation, sedentary atrophy, gym facility binding

### 20 Procedural Substeps:
1. Review `SurvivorLifecycle.cs`, `NeedsSystem.cs`, and `DutyRosterSystem.cs` to map physical capability boundaries.
2. Enforce core invariant: conditioning is a bounded, trainable capability contribution; it does not replace traits, age, or health stats.
3. Claim `Assets/Ashfall.Core/Survivors/PhysicalConditioningCoordinator.cs` and `Assets/Ashfall.Core/Survivors/ConditioningProfile.cs`.
4. Define `ConditioningMetric` facets: CardiovascularEndurance, MuscularStrength, CoreFlexibility, LoadBearingStamina.
5. Establish progressive overload formulas where conditioning metrics improve only when survivors endure sustained physical load.
6. Connect hard manual labor (quarrying, rubble clearing, hauling, distance scouting) to natural conditioning maintenance.
7. Designate gym and training facility rooms in `ShelterRoomSystem` providing structured workout stations.
8. Implement deconditioning atrophy where sedentary or bedridden survivors lose unmaintained conditioning points after 14 days of inactivity.
9. Wire conditioning strength and stamina bonuses into carrying capacity in `Inventory` and march speed in `ExpeditionSystem`.
10. Wire physical exhaustion and recovery debt into `NeedsSystem` ensuring heavy workouts increase caloric and hydration consumption.
11. Implement overtraining hazards: exercising while malnourished, exhausted, or diseased generates strain injury observations.
12. Bound maximum conditioning gains by biological age brackets established in `Plan176SurvivorAgingCoordinator`.
13. Author `exercise_parameters.json` in `Assets/StreamingAssets/Data/` declaring metabolic rates and trainability caps.
14. Ensure all authored conditioning definitions declare `schema_version: 1` and pass `CatalogIntegrityValidator.cs`.
15. Save and restore conditioning metrics and recovery debt within `SurvivorsSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Survivors/PhysicalConditioningTests.cs` verifying adaptation curves.
17. Run 180-day headless simulations proving that conditioning levels plateau smoothly without infinite stat creep.
18. Validate that survivor inspect cards display physical conditioning tiers with clear progress bars and atrophy warnings.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/survivors/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### J1.1 Conditioning Metrics & Adaptation Curves
- (a) Define `ConditioningProfile` value record capturing Cardio, Strength, and Stamina on a 0.0 to 100.0 scale.
- (b) Implement diminishing returns formula ensuring progress slows logarithmically as survivors approach peak conditioning.
- (c) Provide baseline conditioning initialization derived from survivor starting background tags (e.g. Scavenger vs Clerk).
- (d) Write unit tests verifying that identical workout regimens produce deterministic, bounded adaptation curves.

#### J1.2 Labor & Facility Training Integration
- (a) Map physical duty shifts (mining, construction, hauling) to passive conditioning exercise point generation.
- (b) Connect specialized training room equipment (barbells, pull-up bars) to accelerated targeted muscle conditioning.
- (c) Integrate athletic trainer mentorship allowing certified physical instructors to boost trainee workout efficiency.
- (d) Author tests proving that survivors assigned to desk jobs or clinic beds generate zero passive physical exercise points.

#### J1.3 Inactivity Deconditioning & Age Caps
- (a) Implement inactivity timer evaluating consecutive days without vigorous labor or structured workouts.
- (b) Apply gradual deconditioning decay at 0.5 points per day once inactivity exceeds the 14-day grace period.
- (c) Apply age-specific ceiling multipliers from `Plan176` preventing elderly survivors from exceeding youthful physical caps.
- (d) Author tests validating that injured survivors placed on bedrest experience gradual, realistic muscle atrophy.

#### J1.4 Overtraining Fatigue & Caloric Demand
- (a) Calculate workout caloric and hydration surcharges routed directly to `NeedsSystem` metabolic consumption ticks.
- (b) Accumulate `RecoveryDebt` tracking physical exhaustion and requiring compensatory sleep hours to dissipate.
- (c) Trigger overtraining strain injuries in `SurvivorInjuryCoordinator` when workouts occur under acute sleep deprivation.
- (d) Verify through test simulation that survivors cannot indefinitely grind workouts without adequate food and rest.

---

## TASK J2 — C1[42] Environmental Character: Shelter Atmosphere, Sensory Ambiance & Read-Model (Plan 220A/220B/220C)

- **Source Plan:** `C-integration-plans/C1_planintegration[42].md` (Plan 220)
- **Blocker Class:** AMBIENT ENVIRONMENT / SENSORY HABITABILITY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Shelter/`, `src/UI/`
- **Target Subsystem:** Room acoustic dampening, lighting warmth, olfactory/cleanliness scores, aesthetic psychological buffers

### 20 Procedural Substeps:
1. Review `VentilationSystem.cs`, `ShelterRoomSystem.cs`, and `ShelterFireHazardSystem.cs` to locate environmental sensors.
2. Enforce core invariant: atmosphere is a derived presentation read-model; it projects physical facts without simulating a duplicate environment.
3. Claim `Assets/Ashfall.Core/Shelter/ShelterAtmosphereCoordinator.cs` and `Assets/Ashfall.Core/Shelter/AtmosphereReadModel.cs`.
4. Define `AtmosphereFacet` values: AirPurity, AcousticQuiet, ThermalComfort, IlluminationQuality, AestheticTidiness.
5. Aggregate raw physical outputs from power, ventilation, plumbing, and heating systems into normalized 0.0–1.0 facet scores.
6. Implement descriptive pattern matching classifying rooms into dynamic profiles (e.g. Industrial, LivedIn, Claustrophobic, Hospitable).
7. Wire atmosphere summaries into `src/UI/HoldfastSpatialPresenter.cs` driving room ambient lighting tint and dust particle density.
8. Connect acoustic quiet scores to background noise audio bus parameters in `ShelterAcousticDirector.cs`.
9. Route aesthetic tidiness and comfort scores into sleep recovery multipliers in `NeedsSystem` and psychological stress floors.
10. Ensure catastrophic events (fires, toxic gas leaks, sump flooding) immediately override atmosphere profiles with EmergencyAlert visuals.
11. Implement room decoration bonuses: installing crafted tapestries, rugs, plants, or bookshelves elevates aesthetic facet ratings.
12. Provide player-facing atmosphere breakdown tooltips explaining exact underlying causes (e.g., "-20% Air Quality from clogged filter").
13. Author `atmosphere_profiles.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce catalog integrity validation ensuring all profile descriptors link to valid localized text strings.
15. Save and restore room decoration placements and custom aesthetic furnishings inside `ShelterSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Shelter/ShelterAtmosphereTests.cs` verifying facet normalization.
17. Verify deterministic read-model evaluation ensuring identical physical sensor feeds produce identical atmosphere tags.
18. Validate that atmosphere recomputations execute exclusively on room state change events, avoiding per-frame updates.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/shelter/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### J2.1 Sensor Normalization & Facet Composition
- (a) Poll real-time ventilation CFM, temperature readings, and radiation background to compute `AirPurity` and `ThermalComfort`.
- (b) Read electrical grid switchboard state in `PowerGridSystem` to establish room `IlluminationQuality`.
- (c) Read active machinery sound emitter levels to compute normalized `AcousticQuiet` scores.
- (d) Write unit tests verifying that zero air filter airflow correctly drops `AirPurity` to hazardous levels.

#### J2.2 Dynamic Atmosphere Profile Matching
- (a) Implement multi-tag classifier evaluating facet combinations against authored profile definitions.
- (b) Support composite profile tags allowing rooms to be simultaneously classified as `Industrial + Crowded + Tense`.
- (c) Calculate room habitability index summarizing overall psychological comfort for occupants.
- (d) Author tests validating that repairing broken ceiling lights shifts room profile from `Grim` to `Functional`.

#### J2.3 Sensory Presentation & Audio/Visual Binding
- (a) Emit `RoomAtmosphereChangedEvent` when composite profile classifications transition.
- (b) Map illumination quality to Godot CanvasModulate light color gradients in `HoldfastSpatialPresenter`.
- (c) Bind acoustic quiet scores to low-pass filter cutoff frequencies on the room ambiance audio bus.
- (d) Author tests proving that audio bus parameters update deterministically without creating memory leaks.

#### J2.4 Furnishing Buffs & Diagnostic Tooltips
- (a) Parse decorative furniture items placed in room grid slots, summing comfort and aesthetic bonuses.
- (b) Apply modest sleep fatigue restoration bonuses to survivors resting in high-aesthetic bedrooms.
- (c) Generate structured tooltip data strings detailing positive and negative contributing factors for player inspection.
- (d) Verify through test harness that removing a decorative painting immediately updates room aesthetic scores.

---

## TASK J3 — C1[43] Psychological Catharsis: Mental-Health UI, Acoustic Director & Archive Decryption (Plan 220/52/53/62)

- **Source Plan:** `C-integration-plans/C1_planintegration[43].md` (Plan 220/52/53/62)
- **Blocker Class:** TRAUMA RESOLUTION / ACOUSTIC DIRECTOR GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Survivors/`, `Assets/Ashfall.Core/Audio/`
- **Target Subsystem:** Trauma catharsis sessions, dynamic acoustic soundscapes, deep-strata encrypted holotape decoding

### 20 Procedural Substeps:
1. Review `SurvivorMentalHealthSystem.cs`, `AudioManager.cs`, and `ArchiveDecryptionPanel.cs` to locate integration seams.
2. Enforce core invariant: UI submits intents; audio presentation never mutates simulation; archive decryption commits through research authority.
3. Claim `Assets/Ashfall.Core/Survivors/TraumaCatharsisCoordinator.cs` and `Assets/Ashfall.Core/Audio/ShelterAcousticDirector.cs`.
4. Define `CatharsisMethod` enum: GroupMemorialSession, PersonalConfession, CreativeJournaling, SolitaryContemplation.
5. Connect catharsis sessions to counseling rooms or quiet chapels in `ShelterRoomSystem` requiring assigned counselor labor.
6. Implement trauma resolution logic: successful catharsis lowers chronic stress floors without erasing underlying character memory.
7. Build `ShelterAcousticDirector.cs` as a headless-safe audio intent emitter selecting soundscapes based on room atmosphere facets.
8. Connect acoustic director mix buses to Godot audio runtime via thin adapter, isolating sound playback from domain logic.
9. Wire deep-strata archive decryption jobs to `ResearchSystem` consuming chemical solvents and computing cycles over time.
10. Author `ArchiveDecryptionPanel.cs` as a specialized UI presenter rendering encrypted terminal screens and waveform minigames.
11. Ensure decoded archive records unlock canonical lore entries in `JournalSystem` and rare industrial blueprint recipes.
12. Require medical authority validation before offering pharmaceutical sedation for acute psychological breakdowns.
13. Author `catharsis_activities.json` and `encrypted_archives.json` under `Assets/StreamingAssets/Data/` with `schema_version: 1`.
14. Enforce catalog integrity validation confirming all decoded archives map to valid research tree unlock flags.
15. Save and restore active catharsis sessions and archive decryption progress cleanly within `CampaignSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Survivors/TraumaCatharsisTests.cs` verifying trauma resolution mechanics.
17. Verify deterministic seed isolation ensuring identical counselor traits produce identical psychological recovery outcomes.
18. Validate that audio intent events execute safely in headless mode with zero null reference exceptions or sound bus dependencies.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/audio/` and `docs/survivors/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### J3.1 Trauma Catharsis & Counseling Seams
- (a) Author `CatharsisSession` class binding patient survivor, counselor survivor, and chosen therapeutic method.
- (b) Validate counselor qualifications in `SkillProgressionSystem` ensuring medicine or leadership competence.
- (c) Calculate stress floor reduction percentage bounded by trauma severity and patient-counselor rapport.
- (d) Write unit tests verifying that catharsis sessions fail if interrupted by shelter combat breaches or alarms.

#### J3.2 Headless-Safe Acoustic Intent Director
- (a) Implement `ShelterAcousticDirector` in Core emitting pure data `AudioIntentEvent` objects across event bus.
- (b) Author Godot host adapter in `src/Audio/GodotAudioAdapter.cs` translating data intents into runtime bus changes.
- (c) Ensure complete absence of engine audio dependencies inside `Assets/Ashfall.Core/` assemblies.
- (d) Author tests proving that audio intent generation functions identically in headless CI test runs.

#### J3.3 Deep-Strata Archive Decryption Pipeline
- (a) Model encrypted holotape items requiring specialized terminal equipment and solvent reagents to decode.
- (b) Advance decryption progress during research shifts in `ResearchSystem` scaled by survivor electronics skill.
- (c) Emit `ArchiveDecryptedEvent` unlocking pre-war technological blueprints and historical audio logs in `JournalSystem`.
- (d) Author tests validating that corrupted holotapes yield fragmentary lore without crashing the decryption pipeline.

#### J3.4 Crisis Sedation & Medical Governance
- (a) Provide emergency medical sedation action on survivor mental health inspect UI for violent psychotic breaks.
- (b) Validate pharmaceutical supply availability in `Inventory` ensuring required sedative doses are in stock.
- (c) Apply temporary physical paralysis and long-term grogginess debuffs following emergency chemical sedation.
- (d) Author characterization tests confirming that sedation prevents violent outbursts while imposing significant recovery delays.

---

## TASK J4 — C1[44] Wasteland Encounter UX: Micro-Location UI, Turntable Broadcast & 120-Day Fuzz (Plan 224/49/02-09)

- **Source Plan:** `C-integration-plans/C1_planintegration[44].md` (Plan 224/49/02-09)
- **Blocker Class:** EXPEDITION ENCOUNTER UX / CAMPAIGN SOAK INTEGRITY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Expeditions/`, `Ashfall.Core.Tests/`
- **Target Subsystem:** Micro-location inspection cards, vinyl turntable audio recovery, war-caravan trading loop, 120-day headless soak

### 20 Procedural Substeps:
1. Review `ExpeditionSystem.cs`, `TurntableSystem.cs`, and `TravelingCaravanSystem.cs` to inventory late-stage integration boundaries.
2. Enforce core invariant: encounter UX displays precomputed odds; turntable playback reflects item wear; fuzz harness perturbs without cheating.
3. Claim `src/UI/MicroLocationEncounterCard.cs`, `Assets/Ashfall.Core/Audio/TurntableBroadcastCoordinator.cs`, and fuzz suite.
4. Implement `MicroLocationEncounterCard.cs` rendering risk levels, weather hazards, and canonical loot probabilities without client-side rolls.
5. Wire failed encounter consequences directly to `SurvivorMentalHealthSystem` emitting traumatic exposure events.
6. Connect recovered vinyl records to `TurntableSystem` allowing shelter-wide music broadcasts during evening leisure hours.
7. Track vinyl record groove wear through `EquipmentConditionSystem`, causing gradual audio distortion and needle skips over playbacks.
8. Build a unified war-economy caravan orchestrator composing vehicle armor, armed guards, contraband cargo, and faction route tolls.
9. Author `120DayCampaignFuzzHarness.cs` in test assembly simulating 120 continuous days of aggressive randomized player inputs.
10. Assert strict determinism in the fuzz harness: identical seed + identical input script = bitwise-identical save serialization at Day 120.
11. Enforce memory stability: verify that 120-day soak runs show zero memory leaks, unbounded entity collections, or handle leaks.
12. Validate that vehicle cargo trailers increase physical hauling volume rather than magically duplicating encounter loot tables.
13. Author `micro_locations.json` and `turntable_records.json` under `Assets/StreamingAssets/Data/` with `schema_version: 1`.
14. Enforce catalog integrity validation ensuring all micro-location nodes exist on the canonical wasteland map graph.
15. Save and restore turntable playback state and vinyl collection inventories cleanly inside `ShelterSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Expeditions/MicroLocationTests.cs` verifying precomputed probability parity.
17. Execute the 120-day headless fuzz benchmark and assert that simulation execution completes within established performance budgets.
18. Validate that caravan missions properly route combat damage to vehicle parts in `VehicleMaintenanceCoordinator`.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with complete fuzz benchmark logs, test evidence, and updated documentation in `docs/expeditions/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### J4.1 Micro-Location Encounter Card & Precomputed Odds
- (a) Author `MicroLocationEncounterCard.cs` presenting tactical reconnaissance data for explored sub-locations.
- (b) Bind risk and success odds directly to canonical simulation projections in `ExpeditionEncounterResolver`.
- (c) Display clear prerequisite equipment badges (e.g., Gas Mask required, Crowbar recommended) on action cards.
- (d) Write unit tests verifying that UI card presentation never recalculates or diverges from Core odds calculations.

#### J4.2 Turntable Vinyl Playback & Groove Degradation
- (a) Implement turntable playback scheduling enabling survivors to listen to recovered pre-war musical records.
- (b) Emit acoustic comfort broadcasts across common rooms, granting modest shelter-wide stress alleviation.
- (c) Apply physical condition wear ticks to active vinyl item instances on each completed album playback.
- (d) Author tests proving that worn-out vinyl records (condition 0) become unplayable until restored with vinyl cleaning solvent.

#### J4.3 War-Economy Caravan Mission Orchestration
- (a) Build caravan mission state machine coordinating vehicle convoy dispatch, waypoint progression, and return.
- (b) Integrate armed escort combat defense against wasteland raider ambushes along high-risk transit corridors.
- (c) Process contraband border inspections and toll payments when entering militarized faction protectorates.
- (d) Author characterization tests confirming that damaged caravan vehicles return to shelter with authentic repair requirements.

#### J4.4 120-Day Headless Fuzz & Deterministic Soak Gate
- (a) Implement automated soak runner simulating 120 consecutive campaign days with stochastic player decision inputs.
- (b) Periodically save and reload game state at Days 30, 60, 90, and 120, asserting bitwise state checksum parity.
- (c) Monitor GC heap allocations and object counts across the 120-day run to verify strict absence of memory leaks.
- (d) Assert that the full 120-day fuzz execution completes cleanly within the 180-second headless CI execution cap.

---

# 2. QUALITY GATE & VERIFICATION MATRIX

| Gate ID | Target System | Focused Verification Command | Passing Criterion |
| :--- | :--- | :--- | :--- |
| **QG-I1** | Seasonal Human Dynamics | `bash scripts/run_test.sh Ashfall.Core.Tests/World/SeasonalMigrationTests.cs` | 100% pass; seasonal corridors and refugee intake verified |
| **QG-I2** | Interpersonal Friction | `bash scripts/run_test.sh Ashfall.Core.Tests/Relations/InterpersonalConflictTests.cs` | 100% pass; grievance tracking and mediation protocols green |
| **QG-I3** | Shelter Reputation & Standing | `bash scripts/run_test.sh Ashfall.Core.Tests/Factions/ShelterReputationTests.cs` | 100% pass; notoriety progression and information horizon verified |
| **QG-I4** | Material Keepsakes & Property | `bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/PersonalBelongingsTests.cs` | 100% pass; keepsake protection and estate inheritance green |
| **QG-J1** | Physical Conditioning | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/PhysicalConditioningTests.cs` | 100% pass; training adaptation and deconditioning curves green |
| **QG-J2** | Shelter Environmental Character | `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/ShelterAtmosphereTests.cs` | 100% pass; sensor normalization and dynamic profiles green |
| **QG-J3** | Psychological Catharsis & Audio | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/TraumaCatharsisTests.cs` | 100% pass; trauma recovery and acoustic director verified |
| **QG-J4** | Micro-Location UX & 120-Day Fuzz | `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/MicroLocationTests.cs` | 100% pass; precomputed odds and 120-day soak verified |
| **QG-INT** | Catalog Schema & Integrity | `dotnet test --filter "FullyQualifiedName~CatalogIntegrity"` | 100% pass; all newly added JSON catalogs pass schema |
| **QG-BLD** | Engine-Free Compilation | `dotnet build Ashfall.csproj` | 0 Errors, 0 Warnings across entire C# solution |

---

# 3. HANDOFF & CLOSURE CHECKLIST

- [ ] All 8 tasks (I1–I4, J1–J4) have documented terminal states with evidence recorded in commit history.
- [ ] No Unity dependencies, shims, or references were added to any files.
- [ ] `Assets/Ashfall.Core/` remains strictly engine-free (`netstandard2.1`).
- [ ] All authored JSON data contains `schema_version: 1` and adheres to snake_case field naming.
- [ ] No `System.Random` or unseeded `Guid.NewGuid()` calls exist in deterministic Core logic.
- [ ] No duplicate managers, shadow registries, or parallel save stores were introduced.
- [ ] File claims in `WORKTREE_OWNERSHIP.md` are audited and cleared upon task completion.
- [ ] Focused verification passes for all modified subsystems using `scripts/run_test.sh`.
- [ ] Build succeeds with 0 Errors and 0 Warnings across the entire repository.
