# ASHFALL — GENERATION WAVE 12 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 8
// SPDX-License-Identifier: MIT

**Document role:** execution-grade Part 8 master plan for Generation Wave 12, completing the unified C1/C2 flagship integration series.

**Purpose:** convert the final eight unsealed inner-life, universal-access, character-arc, covert-intelligence, governance-succession, time-capsule, famine-rationing, and photographic-documentation plans into an execution-grade unblocking package. Each task provides 20 procedural execution instructions and structured mini-tasks with 4 additional mini-substeps each, eliminating guesswork for coding agents and foremen.

**Wave 12 Part 8 Premise:** Parts 1 through 7 resolved presentation, mechanics, decrees, informational warfare, social life, late progression, and civil infrastructure. Part 8 delivers the **capstone of human meaning, political continuity, and archival legacy**: survivor dream processing and nightmares, universal accessibility contracts, identity-driven personal quests, covert intelligence networks and informants, leadership succession and council challenges, time capsules across generations, emergency famine rationing governance, and survivor photography with darkroom chemical processing.

**Part 8 Execution Set (Exactly 8 Tasks):**
1. **Task O1 — C2[38] Subconscious Processing: Dreams, Nightmares, Sleep Memory & Trauma Consolidation (Plan 177A/177B/177C)**
2. **Task O2 — C2[39] Inclusive Presentation: Accessibility Preferences, Input Adaptation & Universal Contract (Plan 184A/184B/184C)**
3. **Task O3 — C2[40] Personal Narrative: Survivor Quests, Life Milestones & Character Arcs (Plan 200A/200B/200C)**
4. **Task O4 — C2[41] Covert Operations: Intelligence Networks, Wasteland Informants & Counter-Espionage (Plan 203A/203B/203C)**
5. **Task P1 — C2[42] Political Legitimacy: Leadership Succession, Deputy Authority & Council Challenges (Plan 208A/208B/208C)**
6. **Task P2 — C2[43] Inter-Generational Messages: Time Capsules, Sealed Vault Vaults & Delayed Discovery (Plan 212A/212B/212C)**
7. **Task P3 — C2[44] Crisis Allocation: Shelter Resource Rationing, Priority Queues & Famine Governance (Plan 215A/215B/215C)**
8. **Task P4 — C2[45] Visual Documentation: Survivor Photography, Darkroom Processing & Family Albums (Plan 219A/219B/219C)**

---

# 0. OPERATING CONTRACT

## 0.1 Allowed Terminal States

Every task in Wave 12 Part 8 must reach one of these terminal states:

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

## TASK O1 — C2[38] Subconscious Processing: Dreams, Nightmares, Sleep Memory & Trauma Consolidation (Plan 177A/177B/177C)

- **Source Plan:** `C-integration-plans/C2_planintegration[38].md` (Plan 177)
- **Blocker Class:** SLEEP PROCESSING / NIGHTMARE RECOVERY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Survivors/`, `Assets/Ashfall.Core/Needs/`
- **Target Subsystem:** Sleep dream state transitions, trauma-linked nightmares, sleep debt consolidation, morning journal reflections

### 20 Procedural Substeps:
1. Review `NeedsSystem.cs`, `SurvivorMentalHealthSystem.cs`, and `DailyRoutineCoordinator.cs` to map sleep cycle hooks.
2. Enforce core invariant: dreams project psychological state during sleep; they do not duplicate fatigue or sleep need models.
3. Claim `Assets/Ashfall.Core/Survivors/SurvivorDreamCoordinator.cs` and `Assets/Ashfall.Core/Survivors/DreamEventRecord.cs`.
4. Define `DreamKind` enum: RestfulNostalgia, PropheticInsight, TraumaNightmare, FeverHallucination, LucidClarity.
5. Trigger dream state evaluation during the REM phase of overnight sleep in assigned shelter bunks.
6. Connect acute trauma tokens from `SurvivorMentalHealthSystem` to increased nightmare probability (up to 70%).
7. Implement nightmare disruptions: severe nightmares wake the survivor prematurely, halting fatigue recovery and waking roommates.
8. Model prophetic insights: skilled, calm survivors occasionally dream of hidden caches, adding map clues in `JournalSystem`.
9. Wire peaceful nostalgic dreams to morning morale boosts and minor stress floor reductions.
10. Connect fever hallucinations to acute radiation poisoning or high infection in `DiseaseSystem`.
11. Implement morning journal reflections: survivors record impactful dreams, creating short narrative entries in character logs.
12. Allow dream-catcher crafts or herbal sedatives to suppress nightmare frequency for traumatized survivors.
13. Author `dream_event_definitions.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce catalog integrity validation confirming all dream narrative keys link to valid localized prose entries.
15. Save and restore overnight dream outcomes and sleep quality scores cleanly within `SurvivorsSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Survivors/SurvivorDreamTests.cs` verifying dream trigger odds.
17. Verify deterministic seed isolation ensuring identical psychological states produce identical dream sequences.
18. Validate that morning inspect UI presents dream icons and brief diegetic flavor text without popup modal spam.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/survivors/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### O1.1 Dream State Classification & REM Trigger
- (a) Author `DreamStateEvaluator` running during hours 3 to 6 of consecutive survivor sleep cycles.
- (b) Weight dream selection by current mental stress, physical pain, and room environmental comfort.
- (c) Provide fallback ensuring exhausted, healthy survivors experience deep, dreamless restorative rest.
- (d) Write unit tests verifying that survivors sleeping under 2 hours never enter REM dream evaluation.

#### O1.2 Trauma Nightmares & Roommate Disturbance
- (a) Intercept trauma token levels; if trauma > 50, elevate `TraumaNightmare` selection probability.
- (b) Trigger thrashing and vocal distress, reducing sleep recovery efficiency by 50% for the night.
- (c) Emit noise distress events waking adjacent roommates sleeping in the same uninsulated dormitory room.
- (d) Author tests validating that private bedrooms prevent nightmare disturbances from spreading to roommates.

#### O1.3 Nostalgic Consolation & Creative Inspiration
- (a) Select peaceful pre-war memory themes when survivors sleep in decorated rooms holding personal keepsakes.
- (b) Apply passive stress floor relief (+5% mental resilience) upon waking from nostalgic dreams.
- (c) Grant temporary crafting speed bonuses when an artisan dreams of pre-war architectural or mechanical designs.
- (d) Author tests proving that positive dreams accelerate recovery from mild depression.

#### O1.4 Sleep Pharmacology & Protective Talismans
- (a) Author herbal tea and sedative consumption actions in `NeedsSystem` suppressing nightmare occurrence.
- (b) Support crafted decorative dream-catchers and comfort charms hung over bunks to reduce nightmare odds by 25%.
- (c) Record morning dream summaries in survivor personal diaries for player inspection.
- (d) Author characterization tests confirming that pharmacological sleep aids provide consistent, deterministic relief.

---

## TASK O2 — C2[39] Inclusive Presentation: Accessibility Preferences, Input Adaptation & Universal Contract (Plan 184A/184B/184C)

- **Source Plan:** `C-integration-plans/C2_planintegration[39].md` (Plan 184)
- **Blocker Class:** ACCESSIBILITY PREFERENCES / UNIVERSAL PRESENTATION GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Configuration/`, `src/UI/`
- **Target Subsystem:** High-contrast themes, dyslexia-friendly typography, hold-to-confirm timings, screen-reader semantic telemetry

### 20 Procedural Substeps:
1. Review `src/UI/ThemeManager.cs`, `DifficultyCoordinator.cs`, and `InputRouter.cs` to locate presentation settings.
2. Enforce core invariant: accessibility options are universal presentation adapters; they never alter Core simulation rules.
3. Claim `Assets/Ashfall.Core/Configuration/AccessibilityContract.cs` and `src/UI/AccessibilityPresenter.cs`.
4. Define `ColorVisionMode` enum: StandardTritanopia, Deuteranopia, Protanopia, HighContrastMonochrome.
5. Create immutable `AccessibilityPreferences` record storing visual, auditory, cognitive, and motor configuration flags.
6. Implement dynamic color palette re-mapping in Godot UI themes ensuring critical warning states remain distinguishable.
7. Add support for dyslexia-friendly font overrides and adjustable global UI font scaling (100% to 150%).
8. Implement motor accessibility features: customizable hold-to-confirm durations and toggleable button-mashing alternatives.
9. Build a screen-shake and flashing-lights disable toggle in camera and visual effect systems to protect photosensitive players.
10. Provide semantic audio captioning: display text badges (e.g. `[Geiger Clicking Increases]`, `[Airlock Hiss]`) during play.
11. Build a screen-reader telemetry stream emitting structured accessibility text events across UI navigation actions.
12. Ensure all accessibility options are exposed in a dedicated, top-level settings menu accessible before campaign start.
13. Author `accessibility_presets.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce that accessibility preference changes take effect immediately across all active panels without requiring a restart.
15. Save and restore accessibility preferences in user local config files independent of campaign save slots.
16. Author unit tests in `Ashfall.Core.Tests/Configuration/AccessibilityContractTests.cs` verifying preference serialization.
17. Verify that toggling accessibility options has zero impact on campaign simulation checksums or achievement eligibility.
18. Validate that all UI buttons, sliders, and list items maintain clear visible focus rectangles for keyboard/controller navigation.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/ui/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### O2.1 Visual Palette & Contrast Adaptations
- (a) Author color vision filter shaders re-mapping red-green danger cues to blue-yellow high-contrast pairs.
- (b) Implement high-contrast UI mode rendering bold solid backgrounds behind all text readouts.
- (c) Provide non-color iconography: pair all color-coded status badges with distinct geometric shapes.
- (d) Write unit tests verifying that all status badge mappings provide valid shape and text alternatives.

#### O2.2 Typography & Cognitive Readability
- (a) Provide OpenDyslexic or heavy-weighted font replacement option across all UI label controls.
- (b) Implement global font scaling multiplier (1.0x, 1.25x, 1.5x) with dynamic text-wrap bounds checking.
- (c) Add simplified text summary toggles converting dense mechanical logs into concise plain-language bullets.
- (d) Author tests validating that large font scaling does not truncate text or cause UI layout overflows.

#### O2.3 Motor Controls & Timing Flexibility
- (a) Implement adjustable hold-to-confirm timing threshold (0.0s instant click up to 2.0s sustained hold).
- (b) Provide single-press toggle alternatives for actions requiring continuous key depression or rapid mashing.
- (c) Enable pause-on-emergency option automatically halting gameplay upon critical alarm triggers.
- (d) Author tests proving that motor timing adjustments function seamlessly across controller and keyboard inputs.

#### O2.4 Auditory Closed Captions & Telemetry
- (a) Author `AudioCaptionManager` subscribing to sound playback events across the Godot audio bus.
- (b) Render directional caption banners on screen indicating sound category, volume, and off-screen provenance.
- (c) Emit UI semantic accessibility events for external screen-reader assistive software interfaces.
- (d) Author characterization tests confirming that muted audio with captions conveys 100% of vital hazard data.

---

## TASK O3 — C2[40] Personal Narrative: Survivor Quests, Life Milestones & Character Arcs (Plan 200A/200B/200C)

- **Source Plan:** `C-integration-plans/C2_planintegration[40].md` (Plan 200)
- **Blocker Class:** PERSONAL QUEST / CHARACTER ARC GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Quests/`, `Assets/Ashfall.Core/Survivors/`
- **Target Subsystem:** Identity-driven questlines (recovering pre-war homes, finding lost siblings, crafting signature inventions)

### 20 Procedural Substeps:
1. Review `QuestManager.cs`, `SurvivorLifecycle.cs`, and `SurvivorTraits.cs` to map character narrative seams.
2. Enforce core invariant: personal quests instantiate into canonical quest systems; they never run on a disconnected shadow engine.
3. Claim `Assets/Ashfall.Core/Quests/SurvivorPersonalQuestCoordinator.cs` and `Assets/Ashfall.Core/Quests/CharacterArcRecord.cs`.
4. Define `ArcType` enum: LostFamilySearch, VengeancePursuit, MasterpieceInvention, PrewarRestoration, FaithPilgrimage.
5. Generate unique personal quests triggered by survivor background tags, high relationship affinity, or trauma catharsis.
6. Bind personal quest objectives to canonical wasteland locations, specific artifact items, or faction diplomacy actions.
7. Implement milestone progression tracking: completing personal quest stages grants character traits and resolves chronic traumas.
8. Model failure and abandonment: failing a personal quest due to target location destruction inflicts existential depression.
9. Connect quest completion to permanent survivor loyalty: survivors who achieve their life's purpose never defect or rebel.
10. Author unique signature rewards: unlocking custom crafting recipes, heirloom weapons, or exclusive faction contacts.
11. Allow mutual survivor support: assigning friends or partners to assist in personal quests deepens relationship bonds.
12. Ensure personal quests respect campaign pacing: each survivor initiates at most one personal arc per campaign lifetime.
13. Author `personal_quest_templates.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce catalog integrity validation confirming all quest target nodes exist in the canonical wasteland world map.
15. Save and restore active personal quest stages and milestone completion flags inside `QuestsSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Quests/SurvivorPersonalQuestTests.cs` verifying arc triggering rules.
17. Verify deterministic seed isolation ensuring identical recruits generate identical life goal questlines.
18. Validate that survivor inspect cards display active personal quest objectives, progress bars, and reward previews.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/quests/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### O3.1 Arc Template Schema & Trigger Logic
- (a) Define `SurvivorArcTemplate` schema declaring origin requirements, prerequisite days in shelter, and stage objectives.
- (b) Evaluate survivor emotional readiness: initiate quest only when relationship affinity with leadership exceeds +40.
- (c) Provide distinct arc variants tailored to survivor background classes (Doctor, Mechanic, Soldier, Scavenger).
- (d) Write unit tests verifying that personal quests trigger cleanly without duplicate assignments to the same survivor.

#### O3.2 Multi-Stage Objective Instantiation
- (a) Instantiate multi-part objective chains directly into `QuestManager` under a designated `PersonalArc` category.
- (b) Connect intermediate milestones to expedition scouting trips, item retrieval, or dialogue encounters.
- (c) Support emergent branching: allow survivors to choose between forgiveness and vengeance at arc climaxes.
- (d) Author tests validating that quest stages advance deterministically upon objective fulfillment.

#### O3.3 Psychological Catharsis & Trait Evolution
- (a) Resolve underlying mental health afflictions upon successful completion of life milestone quests.
- (b) Transform negative starting traits into positive veteran traits (e.g., `Haunted` evolves into `SteadfastResilience`).
- (c) Apply permanent shelter-wide inspiration buffs celebrating the survivor's personal triumph.
- (d) Author tests proving that fulfilling personal quests locks survivor morale above high baseline minimums.

#### O3.4 Signature Item & Recipe Unlocks
- (a) Award bespoke signature items upon arc completion (e.g., grandfather's tuned hunting rifle, custom medical kit).
- (b) Unlock exclusive high-efficiency recipes in `CraftingSystem` reflecting the survivor's life realization.
- (c) Preserve signature item provenance in `PersonalBelongingsCoordinator` as protected sentimental heirlooms.
- (d) Author characterization tests confirming that signature items serialize cleanly across save/load cycles.

---

## TASK O4 — C2[41] Covert Operations: Intelligence Networks, Wasteland Informants & Counter-Espionage (Plan 203A/203B/203C)

- **Source Plan:** `C-integration-plans/C2_planintegration[41].md` (Plan 203)
- **Blocker Class:** COVERT INTELLIGENCE / INFORMANT NETWORK GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Factions/`, `Assets/Ashfall.Core/World/`
- **Target Subsystem:** Informant recruitment in outposts, rumor verification pipelines, espionage counter-measures, intelligence dossiers

### 20 Procedural Substeps:
1. Review `FactionStanceEngine.cs`, `VerdictRadioSystem.cs`, and `WorldMapTopology.cs` to map intelligence collection seams.
2. Enforce core invariant: intelligence generates facts and observations; it does not directly mutate faction territory or army sizes.
3. Claim `Assets/Ashfall.Core/Factions/CovertIntelligenceCoordinator.cs` and `Assets/Ashfall.Core/Factions/InformantDossier.cs`.
4. Define `IntelligenceAssetTier` enum: CasualRumormonger, PaidInformant, EmbeddedAgent, DoubleAgent.
5. Recruit local informants in wasteland trading outposts and neutral settlements using bribe currencies or scarce medicines.
6. Model informant reporting reliability: reports range from accurate tactical intelligence to exaggerated rumors or enemy plant leaks.
7. Receive early raid warnings: embedded informants report enemy war-party mobilizations 3 to 5 days before raid arrival.
8. Build a counter-intelligence screen in the shelter: assign security sentries to detect foreign spies among incoming refugee waves.
9. Verify unconfirmed wasteland rumors through scouting expeditions, turning speculative leads into confirmed map discoveries.
10. Conduct covert disinformation campaigns feeding false operational plans to enemy spies to mislead opposing warlords.
11. Model informant turnover: informants demand periodic retainer fees and risk discovery/execution by hostile authorities.
12. Intercept foreign diplomatic ciphers: decode intercepted letters to anticipate shifting wasteland alliances and trade wars.
13. Author `intelligence_operation_definitions.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce catalog integrity validation confirming all informant network nodes link to valid world map location IDs.
15. Save and restore active informant rosters, intelligence dossiers, and unconfirmed rumor queues inside `FactionsSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Factions/CovertIntelligenceTests.cs` verifying raid warning timings.
17. Verify deterministic seed isolation ensuring identical intelligence operations yield identical espionage discoveries.
18. Validate that intelligence map overlay renders network coverage radii, informant status pins, and threat heatmaps cleanly.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/factions/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### O4.1 Informant Recruitment & Outpost Placement
- (a) Author recruitment interaction during expedition visits to major wasteland settlements and trade hubs.
- (b) Deduct recurring monthly retainer costs in barter goods or ammunition to maintain active informant networks.
- (c) Track informant loyalty and risk exposure based on host settlement police brutality and counter-spy sweeps.
- (d) Write unit tests verifying that unpaid informants terminate their contracts and cease reporting.

#### O4.2 Early Warning & Raid Intelligence
- (a) Intercept hostile faction raid preparation events in `ShelterDefenseCoordinator`.
- (b) Emit `RaidImminentWarningEvent` 72 hours in advance, revealing attacking faction identity, squad size, and vector.
- (c) Display tactical reconnaissance cards showing enemy troop equipment tiers and special combat capabilities.
- (d) Author tests validating that high-level embedded informants provide 100% accurate raid strength estimates.

#### O4.3 Rumor Verification & Map Clues
- (a) Ingest ambiguous wasteland rumors regarding hidden pre-war bunkers, bountiful water springs, or faction caches.
- (b) Mark unconfirmed rumor nodes on the world map with question-mark indicators and approximate search zones.
- (c) Dispatch scouting expeditions to verify rumors, converting speculative leads into permanently discovered map nodes.
- (d) Author tests proving that dispatched reconnaissance successfully resolves ambiguous rumors into tangible locations.

#### O4.4 Counter-Espionage & Double Agents
- (a) Screen incoming visitors and recruits in `AirlockSecuritySystem` for covert foreign faction spy allegiances.
- (b) Intercept and interrogate discovered infiltrators, offering them clemency in exchange for turning double agent.
- (c) Feed false supply inventory reports to enemy factions via double agents, discouraging anticipated raids.
- (d) Author characterization tests confirming that active counter-espionage eliminates internal shelter sabotage events.

---

## TASK P1 — C2[42] Political Legitimacy: Leadership Succession, Deputy Authority & Council Challenges (Plan 208A/208B/208C)

- **Source Plan:** `C-integration-plans/C2_planintegration[42].md` (Plan 208)
- **Blocker Class:** GOVERNANCE CONTINUITY / LEADERSHIP SUCCESSION GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Governance/`, `Assets/Ashfall.Core/Survivors/`
- **Target Subsystem:** Deputy designation, incapacitation succession triggers, democratic elections vs strongman coups, legitimacy ratings

### 20 Procedural Substeps:
1. Review `CouncilGovernanceSystem.cs`, `SurvivorLifecycle.cs`, and `SurvivorRelationsSystem.cs` to map governance authority.
2. Enforce core invariant: succession coordinates political transitions; it does not replace canonical survivor state or council rules.
3. Claim `Assets/Ashfall.Core/Governance/LeadershipSuccessionCoordinator.cs` and `Assets/Ashfall.Core/Governance/PoliticalLegitimacy.cs`.
4. Define `SuccessionMethod` enum: DesignatedDeputyLineage, DemocraticElection, MartialStrengthTrial, ElderCouncilAppointment.
5. Model `PoliticalLegitimacy` (0 to 100) reflecting survivor confidence in the ruling authority based on food security and fair laws.
6. Allow the player to designate an official Deputy Leader to immediately assume command if the primary leader is incapacitated or dies.
7. Handle emergency succession triggers: when the leader is killed, coma-stricken, or captured on expedition, succession fires atomically.
8. Model political challenges: if legitimacy falls below 30, ambitious rival survivors mount council votes of no confidence.
9. Implement democratic elections: holding periodic open ballots boosts shelter legitimacy and quells civil unrest.
10. Model authoritarian power grabs: ruthless survivors may attempt a coup d'état during periods of severe famine or military defeat.
11. Connect leadership traits to shelter-wide governance bonuses (e.g., Charismatic Leader boosts morale; Engineer Leader boosts build speed).
12. Route failed leadership challenges to exile or demotion, resolving political tension without permanent game-over screens.
13. Author `governance_succession_laws.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce catalog integrity validation confirming all succession laws reference valid governance decree IDs.
15. Save and restore active succession laws, designated deputies, and legitimacy metrics inside `GovernanceSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Governance/LeadershipSuccessionTests.cs` verifying atomic leader handoffs.
17. Verify deterministic seed isolation ensuring identical political climates produce identical election and challenge outcomes.
18. Validate that council chamber UI displays the current leader portrait, designated deputy, legitimacy meters, and challenge logs.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/governance/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### P1.1 Legitimacy Modeling & Popular Confidence
- (a) Author `PoliticalLegitimacyCalculator` evaluating food rationing, shelter warmth, casualty rates, and rule enforcement.
- (b) Apply legitimacy bonuses when leadership successfully defends the shelter from raids or resolves epidemics.
- (c) Apply severe legitimacy penalties when survivors starve, freeze, or suffer arbitrary executions.
- (d) Write unit tests verifying that stable prosperity maintains political legitimacy above 80%.

#### P1.2 Designated Deputy & Atomic Handover
- (a) Provide player interface in council chamber to designate a trusted survivor as official First Deputy.
- (b) Intercept leader death, coma, or capture events emitted by `SurvivorLifecycle`.
- (c) Execute atomic handover: promote Deputy to primary Leader role without interrupting active shelter systems.
- (d) Author tests validating that atomic succession prevents leadership power vacuums and panic breakdowns.

#### P1.3 Democratic Elections & Popular Votes
- (a) Implement constitutional election laws triggering shelter-wide voting every 360 campaign days.
- (b) Tally survivor ballots weighted by candidate charisma, personal popularity, and alignment with council factions.
- (c) Transition leadership peacefully to the winning candidate, granting an immediate +20 legitimacy celebration boost.
- (d) Author tests proving that holding fair elections completely suppresses underground coup conspiracies.

#### P1.4 Council Challenges & Coup d'État Resolution
- (a) Detect conditions for political rebellion: legitimacy < 30 and presence of aggressive, ambitious survivor rivals.
- (b) Initiate formal vote of no confidence or armed coup standoff in the common assembly room.
- (c) Support multiple resolution paths: stepping down peacefully, negotiating concessions, or executing coup leaders.
- (d) Author characterization tests confirming that successful dispute resolution stabilizes shelter authority.

---

## TASK P2 — C2[43] Inter-Generational Messages: Time Capsules, Sealed Vault Vaults & Delayed Discovery (Plan 212A/212B/212C)

- **Source Plan:** `C-integration-plans/C2_planintegration[43].md` (Plan 212)
- **Blocker Class:** GENERATIONAL COMMUNICATION / TIME CAPSULE GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Shelter/`, `Assets/Ashfall.Core/Campaign/`
- **Target Subsystem:** Time capsule fabrication and sealing, milestone opening triggers, ancestral inheritance gifts, archival legacy

### 20 Procedural Substeps:
1. Review `CampaignCalendar.cs`, `ShelterArchiveCoordinator.cs`, and `Inventory.cs` to map multi-generational seams.
2. Enforce core invariant: time capsules preserve real inventory items and authored letters; they do not duplicate item containers.
3. Claim `Assets/Ashfall.Core/Shelter/TimeCapsuleCoordinator.cs` and `Assets/Ashfall.Core/Shelter/TimeCapsuleVault.cs`.
4. Define `CapsuleTriggerKind` enum: ElapsedYears, PopulationMilestone, NuclearWinterEnd, DescendantAdulthood, CrisisRecovery.
5. Author crafting recipe for heavy lead-lined `TimeCapsuleContainer` items fabricated at the machine workbench.
6. Allow survivors to deposit personal letters, photographs, rare seeds, and heirloom tools into the capsule.
7. Implement permanent capsule hermetic sealing: once sealed, contents become completely inaccessible until trigger conditions are met.
8. Bury or embed the capsule into the shelter cornerstone foundation or deep strata bedrock in `ShelterRoomSystem`.
9. Track elapsed campaign time across decades: monitor opening preconditions annually on campaign year turns.
10. Implement dramatic capsule opening ceremonies: when triggers unlock, descendants unseal the capsule in a public ceremony.
11. Grant massive ancestral morale bonuses and cultural inspiration to descendants upon reading letters from their deceased ancestors.
12. Recover preserved pre-collapse crop seeds from opened capsules, unlocking extinct botanical strains in hydroponic bays.
13. Wire time capsule unsealing to `JournalSystem` logging historical reflections comparing past hopes to present reality.
14. Author `time_capsule_templates.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
15. Enforce catalog integrity validation confirming all capsule deposit items map to valid item instances.
16. Save and restore sealed capsule inventories, burial timestamps, and opening trigger predicates inside `ShelterSaveSection`.
17. Author unit tests in `Ashfall.Core.Tests/Shelter/TimeCapsuleTests.cs` verifying multi-decade trigger evaluations.
18. Verify deterministic seed isolation ensuring identical ancestral gifts yield identical emotional inspiration values.
19. Validate that shelter foundation inspect screens display the sealed time capsule monument and remaining countdown years.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/shelter/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### P2.1 Capsule Fabrication & Item Curation
- (a) Author crafting recipe requiring lead ingots, rubber gaskets, and welding supplies to construct a hermetic capsule.
- (b) Provide curation UI allowing player and survivors to deposit up to 6 unique items, keepsakes, and written letters.
- (c) Lock deposited item instances, removing them from active inventory circulation while preserving unique UUIDs.
- (d) Write unit tests verifying that perishable food cannot be deposited without vacuum canning or desiccation.

#### P2.2 Sealing Protocol & Cornerstone Burial
- (a) Author `SealTimeCapsuleAction` executing irreversible sealing protocol with player confirmation.
- (b) Designate burial location in the shelter foundation or memorial plaza, establishing a visible commemorative monument.
- (c) Record founding date, contributing survivors, and opening trigger criteria in the shelter archive.
- (d) Author tests validating that buried capsules survive room damage, fires, and sublevel floods without item loss.

#### P2.3 Multi-Decade Trigger Evaluation
- (a) Implement trigger evaluation listening to `YearTurnTransitionEvent` from `CampaignCalendar`.
- (b) Support descendant triggers: unlock capsule when the founding leader's grandchild reaches adult life stage.
- (c) Support environmental triggers: unlock capsule when ambient surface radiation drops below safe atmospheric thresholds.
- (d) Author tests proving that capsule triggers unlock precisely when configured conditions are fulfilled.

#### P2.4 Ceremonial Unsealing & Cultural Harvest
- (a) Trigger shelter-wide celebration event when capsule opening preconditions are achieved.
- (b) Transfer sealed items back into communal inventory with prestigious `AncestralRelic` tags.
- (c) Unlock heirloom crop seeds and pre-war blueprint schematics deposited decades prior.
- (d) Author characterization tests confirming that unsealing capsules delivers immense cultural resilience buffs.

---

## TASK P3 — C2[44] Crisis Allocation: Shelter Resource Rationing, Priority Queues & Famine Governance (Plan 215A/215B/215C)

- **Source Plan:** `C-integration-plans/C2_planintegration[44].md` (Plan 215)
- **Blocker Class:** RESOURCE RATIONING / CRISIS ALLOCATION GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Economy/`, `Assets/Ashfall.Core/Needs/`
- **Target Subsystem:** Strict caloric rationing tiers, water priority for critical medical wings, fairness perception, famine unrest

### 20 Procedural Substeps:
1. Review `FoodRationingCoordinator.cs`, `NeedsSystem.cs`, and `PowerGridSystem.cs` to map resource allocation seams.
2. Enforce core invariant: rationing controls distribution policy and consumption quotas; it does not invent food or water.
3. Claim `Assets/Ashfall.Core/Economy/CrisisRationingCoordinator.cs` and `Assets/Ashfall.Core/Economy/RationingPolicy.cs`.
4. Define `RationingTier` enum: AbundantFeast, StandardNourishment, HalfRations, StarvationBuffer, EmergencyStrictFamine.
5. Define `ResourceCategory` for priority allocation: DrinkingWater, CaloricFood, ElectricalPower, MedicalAntibiotics, FuelOil.
6. Implement demographic allocation queues prioritizing pregnant mothers, sick patients in clinics, and heavy manual laborers.
7. Model fairness perception: survivors evaluate whether leadership and elite guards receive identical ration cuts as common laborers.
8. Calculate morale and unrest penalties when severe famine rationing persists without transparent communication.
9. Wire water rationing to `NeedsSystem` and `DiseaseSystem`: cutting water rations accelerates dehydration and hygiene breakdown.
10. Wire power load shedding: prioritize life support (ventilation, oxygen scrubbers) while cutting power to workshops and recreational lighting.
11. Implement authoritarian breadlines: assigning armed sentries to mess halls prevents panic hoarding and food rioting during famines.
12. Connect black market diversion: corrupt ration distributors may skim supplies, triggering investigation actions in `HiddenAgendaCoordinator`.
13. Author `crisis_rationing_policies.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce catalog integrity validation confirming all rationing policies link to valid caloric and hydration modifier values.
15. Save and restore active rationing tiers, priority queue rankings, and civil unrest metrics inside `EconomySaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Economy/CrisisRationingTests.cs` verifying caloric deduction formulas.
17. Verify deterministic seed isolation ensuring identical rationing regimes produce identical survivor health trajectories.
18. Validate that rationing UI displays food reserve countdown meters, calorie intake sliders, and unrest risk gauges clearly.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/economy/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### P3.1 Caloric Tiers & Daily Quota Dispatch
- (a) Author `RationingTierDefinition` schema mapping policy tiers to daily caloric and hydration intakes (e.g. 1000 kcal for HalfRations).
- (b) Dispatch daily ration quotas to `FoodRationingCoordinator` during scheduled morning meal shifts.
- (c) Apply physical work stamina debuffs when survivors are maintained on StarvationBuffer rations for >7 days.
- (d) Write unit tests verifying that transitioning to HalfRations exactly halves daily pantry inventory depletion.

#### P3.2 Priority Queues & Demographic Protection
- (a) Build priority queue assigning highest distribution priority to ICU hospital patients and nursing mothers.
- (b) Assign secondary priority to active sentry guards and hazardous mining crews to maintain vital defenses.
- (c) Assign lowest priority to off-duty leisure cohorts during acute emergency shortages.
- (d) Author tests validating that vulnerable clinic patients receive full hydration rations even during water crises.

#### P3.3 Fairness Perception & Famine Unrest
- (a) Calculate `FairnessPerceptionIndex` comparing leadership meal allotments against common survivor rations.
- (b) Trigger civil unrest and strikes in `InterpersonalConflictCoordinator` if leaders feast while survivors starve.
- (c) Apply shared hardship morale bonuses when leaders voluntarily subject themselves to strict starvation rations.
- (d) Author tests proving that egalitarian rationing significantly reduces riot probabilities during famines.

#### P3.4 Power Load Shedding & Brownout Governance
- (a) Author electrical priority switchboard categorizing room circuits into Essential Life Support, Industrial, and Luxury.
- (b) Automatically disconnect luxury lighting and entertainment consoles when total power demand exceeds generator output.
- (c) Preserve power to airlock seals, hydroponic pumps, and medical incubators during severe fuel brownouts.
- (d) Author characterization tests confirming that disciplined load shedding prevents catastrophic life support failures.

---

## TASK P4 — C2[45] Visual Documentation: Survivor Photography, Darkroom Processing & Family Albums (Plan 219A/219B/219C)

- **Source Plan:** `C-integration-plans/C2_planintegration[45].md` (Plan 219)
- **Blocker Class:** PHOTOGRAPHIC MEDIA / CREATIVE RECORD GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Inventory/`, `Assets/Ashfall.Core/Narrative/`
- **Target Subsystem:** Camera equipment, chemical darkroom development, framed photographs, portrait albums, nostalgic morale buffers

### 20 Procedural Substeps:
1. Review `Inventory.cs`, `PersonalBelongingsCoordinator.cs`, and `ShelterRoomSystem.cs` to map photographic media seams.
2. Enforce core invariant: photographs represent narrative artifact items and visual records; they do not require external GPUs.
3. Claim `Assets/Ashfall.Core/Narrative/SurvivorPhotographyCoordinator.cs` and `Assets/Ashfall.Core/Narrative/PhotographRecord.cs`.
4. Define `PhotoSubjectKind` enum: IndividualPortrait, FamilyGathering, CompletedConstruction, VictoriousDefense, ExpeditionDeparture.
5. Author pre-war analog cameras and rolls of unexposed film distributed across technical wasteland salvage locations.
6. Designate darkroom facilities in `ShelterRoomSystem` with red safelights, chemical developer trays, and drying lines.
7. Implement photographic chemical processing consuming chemical solvents and unexposed film to produce developed prints.
8. Capture survivor visual composition: developed photographs record participating survivor IDs, clothing, and timestamp.
9. Allow survivors to frame photographs in bedrooms, granting persistent nostalgic comfort and stress relief during rest.
10. Assemble shelter photo albums in `ShelterArchiveCoordinator`, chronicling the aging faces of survivors across decades.
11. Connect deceased survivor photographs to memorial shrines, comforting bereaved family members through visual remembrance.
12. Build an expedition photojournalism mechanic: scouts photograph distant wasteland ruins, creating valuable intelligence artifacts.
13. Author `photographic_equipment_definitions.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce catalog integrity validation confirming all photographic items map to valid item instances in `items.json`.
15. Save and restore developed photograph records, album indexes, and darkroom chemical stores inside `InventorySaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Narrative/SurvivorPhotographyTests.cs` verifying darkroom development flows.
17. Verify deterministic seed isolation ensuring identical darkroom processing produces identical photographic quality grades.
18. Validate that photo album UI displays authentic monochrome sepia prints, survivor annotations, and date stamps.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/narrative/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### P4.1 Camera Equipment & Field Photography
- (a) Define `CameraEquipmentItem` schema specifying film type, shutter reliability, and optical clarity.
- (b) Author photo-taking action allowing survivors to capture portraits of roommates, family groups, or wasteland vistas.
- (c) Consume one exposure from equipped `UnexposedFilmRoll` item upon capturing a photograph.
- (d) Write unit tests verifying that photography actions fail gracefully if cameras lack loaded film rolls.

#### P4.2 Darkroom Chemical Processing
- (a) Author chemical development work order requiring water, fixing salts, and darkroom workbench labor.
- (b) Advance development progress during artisan work shifts in `DutyRosterSystem`.
- (c) Convert exposed film into physical `DevelopedPhotograph` items with unique instance UUIDs and metadata.
- (d) Author tests validating that lack of water or chemical reagents cleanly pauses darkroom development batches.

#### P4.3 Framed Photographs & Room Comfort
- (a) Author crafting recipe converting wood scrap and developed photos into `FramedPhotograph` decorative furnishings.
- (b) Place framed portraits in residential bunk slots, linking the image to specific resident occupants.
- (c) Apply passive stress alleviation buffs when survivors gaze upon photographs of their spouses or children.
- (d) Author tests proving that framed photos of deceased comrades provide solace and accelerate grief recovery.

#### P4.4 Shelter Photo Albums & Legacy Chronicles
- (a) Create `ShelterPhotoAlbum` entity binding chronological collections of photographs across campaign years.
- (b) Display historical photo galleries in UI allowing players to visually observe survivor aging and shelter expansion.
- (c) Trade rare photographic prints of undiscovered wasteland anomalies to visiting collectors for high barter rewards.
- (d) Author characterization tests confirming that photo album records persist accurately across multi-generational saves.

---

# 2. QUALITY GATE & VERIFICATION MATRIX

| Gate ID | Target System | Focused Verification Command | Passing Criterion |
| :--- | :--- | :--- | :--- |
| **QG-O1** | Subconscious Dreams & Sleep | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/SurvivorDreamTests.cs` | 100% pass; dream triggers, nightmares, and sleep debt verified |
| **QG-O2** | Universal Accessibility | `bash scripts/run_test.sh Ashfall.Core.Tests/Configuration/AccessibilityContractTests.cs` | 100% pass; colorblind modes, typography, and timing verified |
| **QG-O3** | Personal Quests & Arcs | `bash scripts/run_test.sh Ashfall.Core.Tests/Quests/SurvivorPersonalQuestTests.cs` | 100% pass; arc triggers, catharsis, and rewards verified |
| **QG-O4** | Covert Intelligence & Spies | `bash scripts/run_test.sh Ashfall.Core.Tests/Factions/CovertIntelligenceTests.cs` | 100% pass; informant networks, raid alerts, and counter-spy green |
| **QG-P1** | Leadership Succession | `bash scripts/run_test.sh Ashfall.Core.Tests/Governance/LeadershipSuccessionTests.cs` | 100% pass; atomic succession, legitimacy, and elections green |
| **QG-P2** | Multi-Decade Time Capsules | `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/TimeCapsuleTests.cs` | 100% pass; hermetic sealing, trigger checks, and opening verified |
| **QG-P3** | Crisis Famine Rationing | `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/CrisisRationingTests.cs` | 100% pass; caloric tiers, priority queues, and fairness verified |
| **QG-P4** | Survivor Photography | `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/SurvivorPhotographyTests.cs` | 100% pass; darkroom development, framing, and albums green |
| **QG-INT** | Catalog Schema & Integrity | `dotnet test --filter "FullyQualifiedName~CatalogIntegrity"` | 100% pass; all newly added JSON catalogs pass schema |
| **QG-BLD** | Engine-Free Compilation | `dotnet build Ashfall.csproj` | 0 Errors, 0 Warnings across entire C# solution |

---

# 3. HANDOFF & CLOSURE CHECKLIST

- [ ] All 8 tasks (O1–O4, P1–P4) have documented terminal states with evidence recorded in commit history.
- [ ] No Unity dependencies, shims, or references were added to any files.
- [ ] `Assets/Ashfall.Core/` remains strictly engine-free (`netstandard2.1`).
- [ ] All authored JSON data contains `schema_version: 1` and adheres to snake_case field naming.
- [ ] No `System.Random` or unseeded `Guid.NewGuid()` calls exist in deterministic Core logic.
- [ ] No duplicate managers, shadow registries, or parallel save stores were introduced.
- [ ] File claims in `WORKTREE_OWNERSHIP.md` are audited and cleared upon task completion.
- [ ] Focused verification passes for all modified subsystems using `scripts/run_test.sh`.
- [ ] Build succeeds with 0 Errors and 0 Warnings across the entire repository.
