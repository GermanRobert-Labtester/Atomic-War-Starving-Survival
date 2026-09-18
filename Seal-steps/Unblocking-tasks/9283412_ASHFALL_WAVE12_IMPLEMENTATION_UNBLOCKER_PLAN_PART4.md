# ASHFALL — GENERATION WAVE 12 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 4
// SPDX-License-Identifier: MIT

**Document role:** execution-grade Part 4 master plan for Generation Wave 12, derived from the verified post-Wave 11 unclaimed corpus ledger and dependency DAG.

**Purpose:** convert the eight highest-priority unsealed systemic interaction and social/informational depth plans beyond the Wave 12 Part 3 frontier into an execution-grade unblocking package. Each task provides 20 procedural execution instructions and structured mini-tasks with 4 additional mini-substeps each, eliminating guesswork for coding agents and foremen.

**Wave 12 Part 4 Premise:** Parts 1, 2, and 3 resolved spatial holdfast presentation, input/focus reality, session durability, longitudinal medicine, needs cascades, pair history, personal memory, working animals, contraband heat, decrees, and mix ducking. Part 4 establishes the **human agency and systemic discovery layer**: information warfare and propaganda credibility, dynamic shortage-driven quest generation, generational survivor aging and retirement, typed difficulty profiles, cognitive skill decay and recall, individual daily rhythms and routines, item identification forensics, and unified crisis emergency alerts.

**Part 4 Execution Set (Exactly 8 Tasks):**
1. **Task G1 — C1[29] Information Warfare: Propaganda, Credibility & Attribution Network (Plan 168A/168B/168C)**
2. **Task G2 — C1[30] Dynamic Contract Generation: Shortages, Opportunities & Quest Lifecycle (Plan 171A/171B/171C)**
3. **Task G3 — C1[31] Generational Continuity: Aging, Functional Capacity, Retirement & Mentorship (Plan 176A/176B/176C)**
4. **Task G4 — C1[32] Calibrated Challenge: Difficulty Profiles, Assistive Tuning & Presets (Plan 181A/181B/181C)**
5. **Task H1 — C1[33] Cognitive Preservation: Memory & Knowledge Decay, Certification & Recall (Plan 185A/185B/185C)**
6. **Task H2 — C1[34] Personal Rhythms: Daily Survivor Routines, Schedule Coordination & Rest (Plan 188A/188B/188C)**
7. **Task H3 — C1[35] Epistemic Discovery: Item Identification, Specialist Appraisal & Forensics (Plan 191A/191B/191C)**
8. **Task H4 — C1[36] Unified Warning System: Emergency Alerts, Hazard Escalation & Protocols (Plan 194A/194B/194C)**

---

# 0. OPERATING CONTRACT

## 0.1 Allowed Terminal States

Every task in Wave 12 Part 4 must reach one of these terminal states:

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

## TASK G1 — C1[29] Information Warfare: Propaganda, Credibility & Attribution Network (Plan 168A/168B/168C)

- **Source Plan:** `C-integration-plans/C1_planintegration[29].md` (Plan 168)
- **Blocker Class:** INFORMATION WARFARE / PERSUASION SEAM GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Narrative/`, `Assets/Ashfall.Core/Factions/`
- **Target Subsystem:** Authored propaganda message intent, attribution, credibility history, broadcast/print delivery coordination

### 20 Procedural Substeps:
1. Review `PaperPrintingCatalog.cs` and `VerdictRadioSystem.cs` to locate existing printing and broadcast narrative hooks.
2. Confirm the core invariant: propaganda owns message intent and delivery coordination; it does not duplicate faction standing or morale.
3. Claim `Assets/Ashfall.Core/Narrative/PropagandaCoordinator.cs` and `Assets/Ashfall.Core/Narrative/PropagandaCampaign.cs`.
4. Define `PropagandaMessage` value record capturing Medium, Theme, TargetAudience, StatedAuthor, and TruthfulnessRating.
5. Implement message authoring validation ensuring medium matches available infrastructure (Print, Radio, Whispers).
6. Connect printed flyer distribution to expedition courier drops and caravan trade bundles.
7. Wire radio broadcast propaganda to the transmission slot scheduler in `VerdictRadioSystem.cs`.
8. Implement an attribution resolver calculating probability of sender identity discovery based on operational security.
9. Build a credibility tracking model evaluating message plausibility against observed wasteland historical facts.
10. Route successful exposure events into `FactionStanceEngine` as typed narrative evidence records rather than raw modifier numbers.
11. Route domestic propaganda exposure to the shelter morale adapter without creating a parallel survivor mood ledger.
12. Enforce backfire consequences when an attributed message is caught fabricating verifiable wasteland events.
13. Author `propaganda_themes.json` under `Assets/StreamingAssets/Data/` declaring valid rhetorical frameworks.
14. Ensure all authored propaganda definitions declare `schema_version: 1` and pass `CatalogIntegrityValidator.cs`.
15. Add save/restore serialization for active campaigns and attribution discovery records under `CampaignSaveSection`.
16. Construct unit tests in `Ashfall.Core.Tests/Narrative/PropagandaCoordinatorTests.cs` verifying attribution mechanics.
17. Verify deterministic seed isolation ensuring identical campaigns yield identical listener belief updates.
18. Validate that anti-spam rate limiting prevents infinite faction opinion farming through repeated broadcasts.
19. Inspect build output to confirm zero new warnings or namespace collisions with legacy narrative tools.
20. Hand off the task with comprehensive test execution logs and updated documentation in `docs/factions/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### G1.1 Message & Campaign Authoring Surface
- (a) Define `PropagandaMedium` enum (Leaflet, RadioBroadcast, CourierGraffiti, RumorPlanting) in domain models.
- (b) Author schema-validated message templates in `propaganda_templates.json` with localized text keys.
- (c) Validate that message assembly consumes legitimate paper and ink supplies from shelter storage.
- (d) Write unit tests verifying that authoring fails gracefully if designated transmission channels are inactive.

#### G1.2 Attribution & Exposure Mechanics
- (a) Implement exposure coverage calculation based on radio wattage, leaflet volume, or settlement proximity.
- (b) Author attribution evaluation checking survivor tradecraft skills against target faction counter-intelligence.
- (c) Emit `PropagandaAttributedEvent` across the semantic event bus when authorship is decisively unmasked.
- (d) Verify attribution events trigger appropriate hostile or suspicious stance modifications in affected factions.

#### G1.3 Credibility Projection & Backfire Resolution
- (a) Build credibility comparison logic matching declared statements against canonical world events in `JournalSystem`.
- (b) Implement audience skepticism modifiers scaled by faction ideology and historical player relations.
- (c) Calculate backfire severity penalties applying domestic cynicism when shelter survivors detect fabricated claims.
- (d) Author tests covering truthful, exaggerated, and fabricated message trajectories to ensure deterministic outcomes.

#### G1.4 Distribution Pipeline Integration
- (a) Connect leaflet batches to outgoing expedition loadouts via `ExpeditionSupplyCoordinator`.
- (b) Bind radio broadcast campaigns to electrical grid operating schedules to ensure real power draw.
- (c) Connect nomadic merchants to rumor planting actions through `TravelingCaravanSystem`.
- (d) Run end-to-end integration test confirming that broadcast termination immediately ceases regional exposure.

---

## TASK G2 — C1[30] Dynamic Contract Generation: Shortages, Opportunities & Quest Lifecycle (Plan 171A/171B/171C)

- **Source Plan:** `C-integration-plans/C1_planintegration[30].md` (Plan 171)
- **Blocker Class:** PROCEDURAL QUEST SEAM / REACHABILITY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Quests/`, `Assets/Ashfall.Core/Economy/`
- **Target Subsystem:** State-driven quest candidate generation, template parameter binding, canonical quest instantiation

### 20 Procedural Substeps:
1. Audit `Assets/Ashfall.Core/Quests/` and `questline_master.json` to inventory existing canonical quest runtime boundaries.
2. Confirm the core invariant: dynamic quests are instantiated directly into the canonical quest runner, never a shadow runner.
3. Claim `Assets/Ashfall.Core/Quests/DynamicQuestGenerator.cs` and `Assets/Ashfall.Core/Quests/QuestTemplateRegistry.cs`.
4. Define `DynamicQuestTemplate` schema expressing trigger preconditions, parameter slots, and objective templates.
5. Implement trigger fact evaluators detecting acute shelter shortages (water, antibiotics, fuel, mechanical parts).
6. Implement faction opportunity triggers detecting border skirmishes, medical emergencies, and trade embargoes.
7. Build a deterministic parameter resolver that binds valid, alive NPCs and reachable discovered map nodes to template slots.
8. Enforce strict reachability validation so no dynamic quest requires items that cannot be scavenged or crafted.
9. Implement dynamic quest instantiation converting qualified templates into standard `QuestInstance` records.
10. Ensure dynamic quest acceptance, tracking, and objective evaluation route exclusively through `QuestManager.cs`.
11. Bind quest expiration timers to campaign calendar days with automatic withdrawal on opportunity window closure.
12. Wire quest reward fulfillment through canonical inventory and faction ledger dispatchers.
13. Author `dynamic_quest_templates.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Ensure zero procedural text hallucination; all quest summaries must assemble from validated authored fragments.
15. Verify that dynamic quest instance IDs use deterministic hashing derived from campaign seed, day, and template ID.
16. Implement save/restore integration storing active dynamic quest instances inside the standard quest save section.
17. Author unit tests in `Ashfall.Core.Tests/Quests/DynamicQuestGeneratorTests.cs` verifying generation constraints.
18. Run stress tests generating 500 consecutive daily batches to verify zero memory leaks or unbounded state growth.
19. Verify that completing a dynamic quest triggers appropriate downstream journal entries and faction standing updates.
20. Hand off the task with passing test fixtures, zero warnings, and documentation added to `docs/quests/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### G2.1 Shortage & Opportunity Fact Extraction
- (a) Extract daily resource deficits from `Inventory` and `ShelterResourceCoordinator` as typed shortage facts.
- (b) Query `FactionStanceEngine` for factions experiencing military pressure or embargoes to form opportunity facts.
- (c) Scan `WeatherSystem` and `DiseaseQuarantineCoordinator` for active environmental crisis conditions.
- (d) Write unit tests verifying that fact extraction runs cleanly without mutating shelter or faction state.

#### G2.2 Deterministic Parameter Binding
- (a) Query `WorldMapTopology` for valid destination nodes within realistic expedition travel radii.
- (b) Select active, living NPCs from `SurvivorLedger` for escort, medicine delivery, or personal errand roles.
- (c) Filter target item IDs against `CatalogIntegrityValidator` to guarantee physical reachability in loot tables.
- (d) Test parameter binding across varied world states to ensure zero null references or unbound template tags.

#### G2.3 Canonical Quest Runtime Instantiation
- (a) Map bound template parameters into canonical `QuestObjective` instances with explicit progress thresholds.
- (b) Register instantiated quests into `QuestManager` under a designated dynamic quest category.
- (c) Bind quest failure and abandonment penalties to existing faction trust and survivor morale seams.
- (d) Author tests validating that completing a dynamic quest properly retires its lifecycle record in `QuestManager`.

#### G2.4 Opportunity Expiry & Cooldown Management
- (a) Implement per-template cooldown tracking preventing identical shortage quests from flooding the board daily.
- (b) Bind quest expiration to regional state changes (e.g., epidemic end cancels unfulfilled medicine delivery).
- (c) Emit cleanup notifications when expired opportunities are silently pruned from available mission lists.
- (d) Write tests proving that expired quests leave zero dangling references in active UI or save state.

---

## TASK G3 — C1[31] Generational Continuity: Aging, Functional Capacity, Retirement & Mentorship (Plan 176A/176B/176C)

- **Source Plan:** `C-integration-plans/C1_planintegration[31].md` (Plan 176)
- **Blocker Class:** LIFECYCLE CHRONOLOGY / FUNCTIONAL CAPACITY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Survivors/`, `Assets/Ashfall.Core/Needs/`
- **Target Subsystem:** Survivor birth/recruitment chronology, life-stage projection, bounded capability modifiers, retirement/mentorship

### 20 Procedural Substeps:
1. Review `SurvivorLifecycle.cs`, `DutyRosterSystem.cs`, and `SkillProgressionSystem.cs` for existing survivor status hooks.
2. Confirm the core invariant: chronological age is a lifecycle fact; functional capacity is governed by fitness and health.
3. Claim `Assets/Ashfall.Core/Survivors/SurvivorAgingCoordinator.cs` and `Assets/Ashfall.Core/Survivors/LifeStage.cs`.
4. Define `LifeStage` enumeration: Youth, YoungAdult, Adult, MiddleAged, Elder, Venerable with explicit age boundaries.
5. Establish canonical birthdate tracking on survivor records using campaign calendar day reckoning.
6. For legacy saves missing birthdates, implement deterministic age assignment based on recruitment lore tags.
7. Author data-driven functional capacity curves in `aging_parameters.json` defining physical stamina adjustments.
8. Wire functional capacity modifiers into `Plan137NeedsPerformanceCoordinator` without overwriting baseline attributes.
9. Implement a voluntary retirement preference system where elderly survivors seek lighter duties or mentorship roles.
10. Build a structured mentorship mechanic where retired elders pass skill XP to younger survivors during shared leisure.
11. Connect high age to chronic ailment vulnerability in `DiseaseSystem` and `Plan60LongitudinalMedicineCoordinator`.
12. Ensure death from natural senescence routes strictly through the canonical mortality and memorial systems.
13. Wire elder counsel bonuses into `CouncilGovernanceSystem` when elders participate in shelter policy votes.
14. Ensure `SurvivorAgingCoordinator` advances ages strictly on campaign year-turn or seasonal boundary ticks.
15. Implement UI read-model formatters presenting chronological age and life-stage badges cleanly on survivor inspect panels.
16. Author unit tests in `Ashfall.Core.Tests/Survivors/SurvivorAgingTests.cs` validating stage transitions.
17. Run a 400-year headless simulation validating that multi-generational population turnover preserves stability.
18. Validate that survivor deaths from advanced age trigger bereavement in kin through `SurvivorRelationsSystem`.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and full culture-invariant formatting.
20. Hand off the task with passing test fixtures, balance documentation, and updated entries in `docs/survivors/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### G3.1 Chronological Age & Life-Stage Projection
- (a) Add `BirthCampaignDay` field to survivor core data models with non-breaking serialization fallbacks.
- (b) Implement `GetLifeStage(int currentCampaignDay)` extension method evaluating authored threshold brackets.
- (c) Provide culture-invariant age formatting displaying completed years and seasonal days.
- (d) Author unit tests verifying that stage transitions occur precisely on the configured calendar boundaries.

#### G3.2 Functional Capacity & Duty Integration
- (a) Compute physical work efficiency scalars that soften severe cliff debuffs with high skill compensation.
- (b) Connect elder physical capacity limitations to heavy labor duty assignments in `DutyRosterSystem`.
- (c) Enable preferential assignment of elder survivors to teaching, research, and medical caregiving stations.
- (d) Test duty assignment logic to ensure elders are never auto-assigned to high-risk mining or breach defenses.

#### G3.3 Mentorship & Knowledge Transfer Mechanics
- (a) Detect pairing between elder masters (Skill Level 4+) and novice youth in identical shelter rooms.
- (b) Calculate daily passive skill experience transfer rate bounded by relationship affinity and room comfort.
- (c) Emit `SkillMentoredEvent` across the semantic event bus when a novice gains a skill level via mentorship.
- (d) Author tests verifying that mentorship ceases if relationship scores degrade into mutual hostility.

#### G3.4 Natural Senescence & Generational Succession
- (a) Implement age-weighted natural mortality checks integrated with `SurvivorMortalityCoordinator`.
- (b) Route peaceful passing events to `MemorialSystem` with specialized elder respect epitaphs.
- (c) Trigger heirloom and equipment inheritance transfer to designated kin or shelter reserves.
- (d) Verify through test simulation that life-stage distributions remain demographically viable over multi-decade runs.

---

## TASK G4 — C1[32] Calibrated Challenge: Difficulty Profiles, Assistive Tuning & Presets (Plan 181A/181B/181C)

- **Source Plan:** `C-integration-plans/C1_planintegration[32].md` (Plan 181)
- **Blocker Class:** CHALLENGE CONFIGURATION / ACCESSIBILITY INDEPENDENCE GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Campaign/`, `Assets/Ashfall.Core/Configuration/`
- **Target Subsystem:** Typed difficulty profile, preset identity, lock policy, independent accessibility options, NG+ composition

### 20 Procedural Substeps:
1. Review `CampaignCalendar.cs`, `NeedsSystem.cs`, and `RadiationSystem.cs` to map existing difficulty multipliers.
2. Confirm the core invariant: difficulty is configuration, not simulation; accessibility settings must remain independent.
3. Claim `Assets/Ashfall.Core/Campaign/DifficultyProfile.cs` and `Assets/Ashfall.Core/Campaign/DifficultyCoordinator.cs`.
4. Define `DifficultyPreset` enum: Story, Standard, Hardcore, AshfallNightmare, and Custom.
5. Create immutable `DifficultyProfile` value record with typed scalar properties for Scarcity, Combat, Rads, and Fatigue.
6. Author `difficulty_presets.json` under `Assets/StreamingAssets/Data/` declaring default profiles with `schema_version: 1`.
7. Implement preset loading and validation in `CatalogIntegrityValidator.cs` enforcing strict bounds on all scalars.
8. Wire scarcity multipliers into `ProceduralScavengeSystem` and `MarketSystem` via typed read-only getters.
9. Wire metabolic demand scalars into `NeedsSystem` ensuring starvation and dehydration rates scale predictably.
10. Wire radiation dose scalars into `RadiationSystem` ensuring ambient exposure obeys chosen difficulty thresholds.
11. Implement campaign difficulty locking preventing mid-run difficulty downgrades if "Ironman Mode" is toggled on.
12. Ensure accessibility toggles (visual contrast, colorblind palettes, hold-to-confirm) are never gated by difficulty level.
13. Integrate New Game+ challenge modifiers as additive layers on top of baseline difficulty profiles.
14. Save active difficulty profile settings within `CampaignSaveSection` with full backward compatibility.
15. Prevent difficulty changes from retroactively mutating already-generated items, encounters, or world map nodes.
16. Author unit tests in `Ashfall.Core.Tests/Campaign/DifficultyCoordinatorTests.cs` testing preset boundary values.
17. Verify that custom difficulty tuning correctly records customization flags without corrupting preset badges.
18. Validate deterministic replayability: identical seed + identical difficulty preset = identical simulation run.
19. Inspect build output to confirm zero compiler warnings, zero float precision drift, and clean netstandard2.1 compliance.
20. Hand off the task with comprehensive test execution logs and updated documentation in `docs/campaign/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### G4.1 Typed Profile Schema & Presets
- (a) Author `DifficultyProfile` struct declaring typed modifiers: `LootScarcity`, `MetabolicRate`, `CombatDamageTaken`.
- (b) Create standard preset configurations in `difficulty_presets.json` with baseline 1.0 multipliers for Standard.
- (c) Provide profile validation ensuring custom multipliers remain within hard-capped safety envelopes (e.g. 0.2 to 3.0).
- (d) Write unit tests verifying that all authored presets load cleanly without schema errors.

#### G4.2 Subsystem Adapter Wiring
- (a) Wire `LootScarcity` into scavenge roll calculations in `ProceduralScavengeSystem`.
- (b) Wire `MetabolicRate` into hourly calorie and hydration consumption in `NeedsSystem`.
- (c) Wire `EnvironmentalHazardRate` into toxic dust and radiation accumulation in `RadiationSystem`.
- (d) Test each subsystem adapter with 0.5x, 1.0x, and 2.0x profiles to verify linear, predictable scaling.

#### G4.3 Accessibility Independence & UI Binding
- (a) Audit settings UI hierarchy to ensure accessibility options reside in a dedicated, top-level settings menu.
- (b) Author difficulty comparison presenters displaying clear percentage differences between presets.
- (c) Add warning dialogs explaining that changing difficulty mid-campaign may disable specific achievement tags.
- (d) Verify in headless test harness that changing accessibility options leaves difficulty profile hashes unchanged.

#### G4.4 Campaign Locking & New Game+ Stacking
- (a) Implement `IsDifficultyLocked` flag in campaign metadata honoring permadeath and ironman commitments.
- (b) Create composition logic combining New Game+ challenge skulls with active difficulty profile modifiers.
- (c) Guard against save-file tampering by validating difficulty profile checksums during save deserialization.
- (d) Author tests verifying that locked campaigns reject runtime difficulty modification requests.

---

## TASK H1 — C1[33] Cognitive Preservation: Memory & Knowledge Decay, Certification & Recall (Plan 185A/185B/185C)

- **Source Plan:** `C-integration-plans/C1_planintegration[33].md` (Plan 185)
- **Blocker Class:** COGNITIVE DRIFT / SKILL DORMANCY SEAM GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Skills/`, `Assets/Ashfall.Core/Survivors/`
- **Target Subsystem:** Knowledge and skill decay coordinator, practice reinforcement, certification permanence, cognitive recall

### 20 Procedural Substeps:
1. Review `SkillProgressionSystem.cs` and `PhantomMemoryEngine.cs` to understand current skill and memory structures.
2. Confirm the core invariant: memory decay coordinates degradation; it does not replace canonical skill progression.
3. Claim `Assets/Ashfall.Core/Skills/SkillDecayCoordinator.cs` and `Assets/Ashfall.Core/Skills/CognitiveRetention.cs`.
4. Define `SkillDormancyRecord` tracking the last campaign day a survivor actively practiced or exercised a skill.
5. Establish decay thresholds: skills remain stable for 30 days of inactivity before experiencing gradual point decay.
6. Implement certification protection honoring Plan 180 certificates, ensuring certified master ranks never degrade.
7. Build reinforcement adapters rewarding skill practice during daily duty execution, crafting, and medical care.
8. Connect cognitive trauma and severe malnutrition in `NeedsSystem` to accelerated decay rates.
9. Implement a rapid relearning bonus ensuring survivors regain previously decayed skill ranks at triple speed.
10. Ensure physical skills (Melee, Athletics) decay independently from intellectual skills (Medicine, Electronics).
11. Author data-driven retention curves in `skill_decay_parameters.json` with standard `schema_version: 1`.
12. Wire forgotten recipe mechanics to `CraftingSystem` so unpracticed rare formulas become dormant until refreshed.
13. Integrate written shelter technical manuals and libraries as passive retention aids that halve decay rates.
14. Save and restore survivor dormancy timestamps cleanly within the existing `SkillsSaveSection`.
15. Emit `SkillDecayedEvent` across the semantic event bus whenever a skill level threshold drops.
16. Author unit tests in `Ashfall.Core.Tests/Skills/SkillDecayTests.cs` verifying decay timing and relearning rates.
17. Verify deterministic seed isolation ensuring identical inactivity patterns cause identical skill decay.
18. Validate that survivor inspections present dormant and certified skills with legible visual indicators.
19. Inspect build output to confirm zero compiler warnings, zero memory churn, and strict null safety.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/skills/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### H1.1 Skill Dormancy & Inactivity Tracking
- (a) Add `LastPracticedCampaignDay` dictionary to survivor skill data records.
- (b) Update timestamp whenever a survivor executes an action yielding experience in a given skill domain.
- (c) Provide daily evaluation method calculating inactive elapsed days against dormancy thresholds.
- (d) Write unit tests verifying that daily duty execution successfully resets dormancy timers.

#### H1.2 Certification Protection & Minimum Floors
- (a) Integrate `ICertificationProvider` to query active master certificates held by the survivor.
- (b) Establish immutable skill tier floors that block decay from dropping a skill below certified benchmarks.
- (c) Author tests proving certified surgeons never forget basic surgical hygiene regardless of inactivity duration.
- (d) Validate that uncertified skill levels decay gracefully down to the baseline tier floor.

#### H1.3 Cognitive Health & Environmental Modifiers
- (a) Query `RadiationSystem` and `DiseaseSystem` for acute brain fever or neurotoxic poison afflictions.
- (b) Apply decay acceleration factors when severe dehydration or sleep deprivation persists beyond 48 hours.
- (c) Connect library and workstation proximity in `ShelterRoomSystem` to establish retention buff zones.
- (d) Author tests validating that healthy, well-read survivors experience substantially slower knowledge decay.

#### H1.4 Relearning Acceleration & Memory Recovery
- (a) Track `HistoricalMaxSkillLevel` on survivor records to preserve lifetime achievement peaks.
- (b) Apply 3.0x experience multiplier when practicing skills that have decayed below their historical peak.
- (c) Emit `SkillRelearnedEvent` when a survivor successfully recovers a dormant skill level.
- (d) Author characterization tests confirming that recovery curves are significantly faster than initial acquisition.

---

## TASK H2 — C1[34] Personal Rhythms: Daily Survivor Routines, Schedule Coordination & Rest (Plan 188A/188B/188C)

- **Source Plan:** `C-integration-plans/C1_planintegration[34].md` (Plan 188)
- **Blocker Class:** BEHAVIORAL INTENT / ROSTER COORDINATION GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Shelter/`, `Assets/Ashfall.Core/Survivors/`
- **Target Subsystem:** Individual daily rhythm, routine preference templates, schedule intent resolution, emergency interruptions

### 20 Procedural Substeps:
1. Review `ShelterScheduleSystem.cs` and `DutyRosterSystem.cs` to locate shelter-wide phase management boundaries.
2. Confirm the core invariant: personal routines define behavioral preference and intent, never overriding emergency curfews.
3. Claim `Assets/Ashfall.Core/Shelter/SurvivorRoutineCoordinator.cs` and `Assets/Ashfall.Core/Shelter/DailyRoutine.cs`.
4. Define `DailyRoutineTemplate` modeling 24-hour schedules partitioned into Sleep, Labor, Nutrition, Social, and FreeTime.
5. Create authored routine archetypes (NightOwl, EarlyRiser, Workaholic, CommunalSocializer) in `routine_archetypes.json`.
6. Assign individual routine archetypes to survivors based on personality traits, background tags, and duty roles.
7. Implement schedule intent resolution converting routine blocks into prioritized survivor location intents.
8. Wire nutrition blocks to mess hall seating and meal consumption in `FoodRationingCoordinator`.
9. Wire sleep blocks to assigned bunks and fatigue restoration in `NeedsSystem`.
10. Ensure emergency alarm states (Raids, Fires, Breaches) immediately suspend personal routines in favor of combat duty.
11. Implement routine conflict detection highlighting overcrowded workshops or congested meal hours.
12. Allow player overrides permitting custom manual timetable configuration per survivor or work cohort.
13. Wire routine satisfaction into survivor mental health; adhering to preferred rhythms yields minor stress reductions.
14. Ensure routine evaluation runs on hour-tick increments rather than per-frame polling.
15. Author save/restore serialization storing custom routine overrides inside `ShelterSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Shelter/SurvivorRoutineTests.cs` verifying transition mechanics.
17. Verify deterministic schedule resolution ensuring identical shelter clocks produce identical movement intents.
18. Validate that room transitions cleanly update occupancy counts in `ShelterRoomSystem`.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/shelter/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### H2.1 Routine Archetypes & Preference Models
- (a) Author `DailyRoutineTemplate` class representing a 24-element array of hourly activity intents.
- (b) Author schema-validated archetypes in `routine_archetypes.json` with distinct circadian cycles.
- (c) Implement trait-to-archetype matching (e.g., nocturnal trait maps directly to NightOwl archetype).
- (d) Write unit tests verifying that all authored archetypes sum to exactly 24 valid hourly blocks.

#### H2.2 Hourly Intent Resolution & Dispatch
- (a) Implement `ResolveHourlyIntent(SurvivorId survivor, int hour)` returning current target activity.
- (b) Dispatch activity intents to corresponding shelter room target finders in `ShelterRoomSystem`.
- (c) Coordinate meal shifts to prevent entire shelter populations from rushing mess facilities simultaneously.
- (d) Test intent resolution across all 24 hours of a campaign day to verify smooth transition sequences.

#### H2.3 Emergency Override & Curfew Enforcement
- (a) Listen for `EmergencyAlertTriggeredEvent` and immediately transition all survivors to EmergencyResponse state.
- (b) Enforce shelter curfew rules locking exterior airlocks during nighttime radiation storms.
- (c) Restore baseline routine execution automatically once emergency all-clear signals are broadcast.
- (d) Author tests proving that wounded survivors in medical clinic beds ignore routine work intents.

#### H2.4 Player Customization & Schedule Overrides
- (a) Provide player API allowing individual timetable adjustment per survivor or duty squad.
- (b) Validate custom player schedules ensuring mandatory minimum rest periods (e.g. at least 4 hours sleep).
- (c) Calculate schedule friction penalties when players force survivors into conflicting circadian cycles.
- (d) Author characterization tests confirming that custom schedules persist cleanly across save/load cycles.

---

## TASK H3 — C1[35] Epistemic Discovery: Item Identification, Specialist Appraisal & Forensics (Plan 191A/191B/191C)

- **Source Plan:** `C-integration-plans/C1_planintegration[35].md` (Plan 191)
- **Blocker Class:** UNCERTAINTY MODELING / SPECIALIST APPRAISAL GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Inventory/`, `Assets/Ashfall.Core/Research/`
- **Target Subsystem:** Per-instance knowledge state, analysis workbench jobs, confidence/reveal semantics, safe property masking

### 20 Procedural Substeps:
1. Review `Inventory.cs`, `ItemInstance.cs`, and `ProceduralScavengeSystem.cs` to map item property representations.
2. Confirm the core invariant: identification tracks knowledge about an item instance; it never alters underlying item truth.
3. Claim `Assets/Ashfall.Core/Inventory/ItemIdentificationCoordinator.cs` and `Assets/Ashfall.Core/Inventory/ItemKnowledgeState.cs`.
4. Define `ItemKnowledgeLevel` enum: Unknown, VisualAppraisal, TestedSafe, FullyAnalyzed, CounterfeitDetected.
5. Create `ItemKnowledgeRecord` storing known traits, confidence level, and identification provenance per item instance.
6. Establish auto-identification rules for common staples (water, bread, raw scrap) to avoid micromanagement bloat.
7. Flag rare, military, medical, chemical, and ancient pre-collapse tech as requiring specialist identification.
8. Build an appraisal work order pipeline at the research laboratory or workshop bench utilizing science/engineering skills.
9. Implement safe property masking in UI presenters so unidentified toxic medicine displays as "Unlabeled Medication".
10. Allow hazardous blind usage: desperate players may consume unverified items, triggering full physiological effects.
11. Implement counterfeit detection revealing that apparent luxury goods or pristine weapons are defective knockoffs.
12. Wire specialist appraisal skills to market valuation in `MarketSystem`, preventing full-price trading of unknown scrap.
13. Author `unidentified_item_profiles.json` declaring mystery templates with standard `schema_version: 1`.
14. Ensure item identification records link to instance UUIDs and serialize cleanly in `InventorySaveSection`.
15. Emit `ItemIdentifiedEvent` across the semantic event bus when research unlocks true item identity.
16. Author unit tests in `Ashfall.Core.Tests/Inventory/ItemIdentificationTests.cs` verifying reveal mechanics.
17. Verify deterministic seed isolation ensuring identical research efforts yield identical reveal speeds.
18. Validate that inventory stacking logic separates identified from unidentified instances of the same base item.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and strict null safety.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/inventory/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### H3.1 Knowledge State & Uncertainty Modeling
- (a) Define `ItemKnowledgeRecord` struct referencing canonical item instance UUIDs without duplicating base item stats.
- (b) Implement masking logic replacing sensitive names, radiation values, and effects with mystery descriptions.
- (c) Provide auto-identification rule engine automatically classifying mundane consumables as fully identified.
- (d) Author unit tests verifying that pristine items retain underlying stats even while masked from player view.

#### H3.2 Appraisal Work Orders & Bench Execution
- (a) Create `AppraisalWorkOrder` schema defining required workbench station, tool tier, and specialist labor hours.
- (b) Assign appraisal tasks to survivors possessing Medical, Engineering, or Science skill proficiencies.
- (c) Advance appraisal progress during normal daily work shifts in `DutyRosterSystem`.
- (d) Test appraisal execution to ensure progress suspends cleanly when work shifts terminate or supplies fail.

#### H3.3 Hazardous Blind Usage & Forensics
- (a) Allow player interaction to force consumption or equipment of unidentified items despite warnings.
- (b) Route blind consumption effects directly to `NeedsSystem` or `DiseaseSystem` based on actual underlying stats.
- (c) Reveal true item identity immediately following blind usage as physiological consequences manifest.
- (d) Author tests validating that drinking unlabeled toxic liquid correctly triggers poisoning and unmasks the item.

#### H3.4 Counterfeit Detection & Market Discounting
- (a) Model counterfeit items possessing authentic outward appearance but substandard internal durability.
- (b) Connect appraisal skill checks to counterfeit unmasking, updating the instance knowledge record.
- (c) Apply severe market value discounts in `MarketSystem` when attempting to trade unverified or counterfeit goods.
- (d) Author characterization tests confirming that merchants refuse or heavily discount unidentified merchandise.

---

## TASK H4 — C1[36] Unified Warning System: Emergency Alerts, Hazard Escalation & Protocols (Plan 194A/194B/194C)

- **Source Plan:** `C-integration-plans/C1_planintegration[36].md` (Plan 194)
- **Blocker Class:** CRISIS PRESENTATION / PROTOCOL ROUTING GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Events/`, `Assets/Ashfall.Core/Shelter/`
- **Target Subsystem:** Consolidated alert records, severity/urgency projection, deduplication, recommended protocol routing

### 20 Procedural Substeps:
1. Review `WeatherSystem.cs`, `RadiationSystem.cs`, and `ShelterFireHazardSystem.cs` to inventory existing hazard signals.
2. Confirm the core invariant: alerts present facts and route protocols; they do not simulate the underlying hazard.
3. Claim `Assets/Ashfall.Core/Events/EmergencyAlertCoordinator.cs` and `Assets/Ashfall.Core/Events/EmergencyAlert.cs`.
4. Define `AlertSeverity` (Advisory, Warning, Critical, Catastrophic) and `AlertUrgency` (Immediate, Pending, LongRange).
5. Create `EmergencyAlertRecord` containing SourceSubsystem, Timestamp, AffectedZone, LeadTimeHours, and ActionLink.
6. Implement alert deduplication ensuring multiple sensors reporting the same structural breach coalesce into a single alert.
7. Build an escalation engine that increases alert severity if underlying hazard metrics continue deteriorating.
8. Wire active emergency alerts to the shelter siren and lighting systems in `ShelterRoomSystem` and audio bus.
9. Route emergency response protocol activations (Firefighting, Lockdown, Medical Triage) to `DutyRosterSystem`.
10. Ensure player acknowledgment registers read state without halting or pausing real-time hazard progression.
11. Implement alert auto-resolution clearing notifications immediately when the underlying hazard condition normalizes.
12. Author data-driven alert metadata in `emergency_alert_definitions.json` with standard `schema_version: 1`.
13. Integrate historical alert logging with `JournalSystem` preserving an audit trail of shelter catastrophes.
14. Ensure alert presentation obeys accessibility guidelines with distinct non-color visual iconography and text tags.
15. Save and restore active alert collections and acknowledgment states within `EventsSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Events/EmergencyAlertTests.cs` verifying deduplication and escalation.
17. Verify deterministic alert dispatch ensuring identical hazard feeds generate identical alert sequences.
18. Validate that high-frequency sensor updates do not cause UI frame drops or garbage collection spikes.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and strict netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/events/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### H4.1 Alert Registry & Deduplication Logic
- (a) Implement `EmergencyAlertRecord` struct with unique composite keys: `(Subsystem, HazardType, ZoneId)`.
- (b) Build deduplication filter merging repeated warning pulses within configured time windows into single records.
- (c) Author alert registration API accepting incoming hazard observations from environmental subsystems.
- (d) Write unit tests verifying that five simultaneous fire sensor triggers in Room B generate exactly one alert.

#### H4.2 Severity Projection & Escalation Engine
- (a) Map raw subsystem metrics (temperature, sievert dose, water level) to standard `AlertSeverity` tiers.
- (b) Implement lead-time calculation estimating hours remaining until critical infrastructure failure occurs.
- (c) Elevate alert severity automatically when hazard metrics surpass secondary critical thresholds.
- (d) Author tests proving that a slow radiation leak escalates from Advisory to Critical as rem levels rise.

#### H4.3 Response Protocol Activation
- (a) Define typed `EmergencyProtocol` descriptors (ShelterLockdown, RadiationShelterInPlace, EvacuateSublevel).
- (b) Connect protocol activation buttons on the UI alert card to automatic duty reassignments in `DutyRosterSystem`.
- (c) Route lockdown commands to door airlock controls in `ShelterRoomSystem` sealing breached sectors.
- (d) Test protocol execution to verify that non-essential workers immediately seek safety upon protocol trigger.

#### H4.4 Lifecycle Resolution & Audit Logging
- (a) Monitor incoming hazard status feeds and automatically mark alerts resolved when conditions clear.
- (b) Move resolved alerts into an archival ledger for post-crisis debriefing and journal reviews.
- (c) Ensure player manual dismissal hides visual banners without canceling active emergency defense protocols.
- (d) Author characterization tests confirming that resolved alerts leave zero persistent UI artifacts.

---

# 2. QUALITY GATE & VERIFICATION MATRIX

| Gate ID | Target System | Focused Verification Command | Passing Criterion |
| :--- | :--- | :--- | :--- |
| **QG-G1** | Propaganda & Info Warfare | `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/PropagandaCoordinatorTests.cs` | 100% pass; attribution and credibility mechanics verified |
| **QG-G2** | Dynamic Quest Generation | `bash scripts/run_test.sh Ashfall.Core.Tests/Quests/DynamicQuestGeneratorTests.cs` | 100% pass; zero dangling parameters or unreachable items |
| **QG-G3** | Generational Survivor Aging | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/SurvivorAgingTests.cs` | 100% pass; life-stage transitions and capacity scaling green |
| **QG-G4** | Difficulty Profiles | `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/DifficultyCoordinatorTests.cs` | 100% pass; preset boundaries and accessibility separation green |
| **QG-H1** | Skill Decay & Recall | `bash scripts/run_test.sh Ashfall.Core.Tests/Skills/SkillDecayTests.cs` | 100% pass; certification protection and relearning validated |
| **QG-H2** | Daily Survivor Routines | `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/SurvivorRoutineTests.cs` | 100% pass; 24-hour intent resolution and overrides verified |
| **QG-H3** | Item Identification & Forensics | `bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/ItemIdentificationTests.cs` | 100% pass; mystery masking and specialist appraisal green |
| **QG-H4** | Unified Emergency Alerts | `bash scripts/run_test.sh Ashfall.Core.Tests/Events/EmergencyAlertTests.cs` | 100% pass; deduplication, escalation, and protocols validated |
| **QG-INT** | Catalog Schema & Integrity | `dotnet test --filter "FullyQualifiedName~CatalogIntegrity"` | 100% pass; all newly added JSON catalogs pass schema |
| **QG-BLD** | Engine-Free Compilation | `dotnet build Ashfall.csproj` | 0 Errors, 0 Warnings across entire C# solution |

---

# 3. HANDOFF & CLOSURE CHECKLIST

- [ ] All 8 tasks (G1–G4, H1–H4) have documented terminal states with evidence recorded in commit history.
- [ ] No Unity dependencies, shims, or references were added to any files.
- [ ] `Assets/Ashfall.Core/` remains strictly engine-free (`netstandard2.1`).
- [ ] All authored JSON data contains `schema_version: 1` and adheres to snake_case field naming.
- [ ] No `System.Random` or unseeded `Guid.NewGuid()` calls exist in deterministic Core logic.
- [ ] No duplicate managers, shadow registries, or parallel save stores were introduced.
- [ ] File claims in `WORKTREE_OWNERSHIP.md` are audited and cleared upon task completion.
- [ ] Focused verification passes for all modified subsystems using `scripts/run_test.sh`.
- [ ] Build succeeds with 0 Errors and 0 Warnings across the entire repository.
