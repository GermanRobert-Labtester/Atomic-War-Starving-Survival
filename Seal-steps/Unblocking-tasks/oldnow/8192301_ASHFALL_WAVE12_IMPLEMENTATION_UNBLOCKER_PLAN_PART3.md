# ASHFALL — GENERATION WAVE 12 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 3
// SPDX-License-Identifier: MIT

**Document role:** execution-grade Part 3 master plan for Generation Wave 12, continuing directly from the verified Part 2 frontier (`7391824_ASHFALL_WAVE12_IMPLEMENTATION_UNBLOCKER_PLAN_PART2.md`).

**Purpose:** convert the next eight unsealed flagship plans into an execution-grade unblocking package. Each task provides 20 procedural instructions and structured mini-tasks containing 4 mini-substeps each, maintaining rigorous authority boundaries, single ownership, determinism, and zero-warning build discipline.

**Wave 12 Part 3 Premise:** Part 1 targeted depth-content rails, spatial holdfast view, asset hygiene, longitudinal medicine, focus navigation, session soak, authored identity, and voice delivery. Part 2 resolved cross-system bridges (discovery consequences, needs cascade, combat diplomacy, capability gating, relationship effects, balance metrics, release tagging, and asset truth). Part 3 advances into **long-term systemic character and community continuity: per-NPC memory depth, working animal roles, underground contraband economy, shelter governance policy, bunker origin identity, stateful soundscapes, 400-year multi-generational retention, and standing gate compression**.

**Part 3 Execution Set (Exactly 8 Tasks):**
1. **Task E1 — C1[24] Per-NPC Personal Memory & Relationship Depth (Plan 147A/147B/147C/147D)**
2. **Task E2 — C1[25] Working Animals & Companion Operating Roles (Plan 151A/151B/151C/151D)**
3. **Task E3 — C1[26] Black Market Contraband, Risk & Enforcement Waves (Plan 155A/155B/155C/155D)**
4. **Task E4 — C1[27] Shelter Governance, Legitimacy & Policy Decrees (Plan 159A/159B/159C/159D)**
5. **Task F1 — C1[28] Shelter Identity, Naming, Origin & Community Projection (Plan 166A/166B/166C/166D)**
6. **Task F2 — C2[23] Stateful Ambience, Mix Discipline & Sparse Musical Arc (Plan 52A/52B/52C)**
7. **Task F3 — C2[24] Retention Policies, Save Corpus Archaeology & 400-Year Scale (Plan 55A/55B/55C)**
8. **Task F4 — C2[25] Retrospective Standing Gates & Plan-Layer Compression (Plan 59A/59B/59C)**

---

# 0. OPERATING CONTRACT

## 0.1 Allowed Terminal States

Every task in Wave 12 Part 3 must terminate in one of these recognized states:

- **IMPLEMENTED** — Missing mechanism, bridge, read model, or consumer is landed through its canonical owner, and all required verification gates pass.
- **DECIDED-DEFERRED** — Product or architecture decision is signed by the foreman, explicitly defining deferred scope and preserving non-fabricated boundaries.
- **RETIRED** — A legacy shim, speculative system, or duplicate authority is deleted and unregistered with full reference validation.
- **VERIFIED-RESOLVED** — Investigation at repository `HEAD` proves the required contract is already sealed by prior work, skipping redundant implementation.
- **ROUTED-REPAIR** — A genuine production defect is discovered outside the task scope, isolated into a dedicated repair ticket with reproducible test.

## 0.2 Mandatory Non-Negotiable Rules

1. **Godot is authoritative; Unity is retired.** No Unity packages, namespaces, or references may be introduced.
2. **Core remains engine-free.** `Assets/Ashfall.Core/` (`netstandard2.1`) must never reference Godot or engine assemblies.
3. **JSON data is authoritative.** Authoritative data resides in `Assets/StreamingAssets/Data/` declaring `schema_version: 1`.
4. **One authority per concern.** Never duplicate faction standing, world state, affliction status, or inventory ledgers.
5. **Deterministic replay.** No unseeded `System.Random`, wall-clock time, or hash-iteration ordering in domain logic.
6. **Substeps are instructions, not tasks.** The 20 substeps per task represent ordered procedural instructions.
7. **Mini-tasks require 4 mini-substeps.** Any mini-task (e.g. `.1`, `.2`) must contain exactly 4 subsequent execution instructions.
8. **Claims before edits.** File claims must be registered in `WORKTREE_OWNERSHIP.md` before editing.
9. **Zero-warning build bar.** All compilation passes must complete with 0 errors and 0 warnings.
10. **Targeted verification first.** Use `bash scripts/run_test.sh <file>` to verify changed areas without running broad suites.

---

# 1. DETAILED TASK SPECIFICATIONS

## TASK E1 — C1[24] Per-NPC Personal Memory & Relationship Depth (Plan 147A/147B/147C/147D)

- **Source Plan:** `C-integration-plans/C1_planintegration[24].md` (Plan 147)
- **Blocker Class:** PERSONAL MEMORY GAP / LACK OF NPC CONTINUITY
- **Canonical Owner:** `Assets/Ashfall.Core/Narrative/`, `Assets/Ashfall.Core/Social/`
- **Target Subsystem:** Named NPC memory facts, personal gratitude/grudge records, trade stance modulation

### 20 Procedural Substeps:
1. Audit `HoldfastNpcCatalog`, `DoorEncounterSystem`, and `VerdictNpcSystem` to identify existing named NPC registries.
2. Confirm the architectural boundary: per-NPC memory is a personal history layer; do not create a second faction reputation system.
3. Claim `Assets/Ashfall.Core/Narrative/NpcMemoryLedger.cs` and `Assets/Ashfall.Core/Social/NpcPersonalStanceEngine.cs`.
4. Author `NpcMemoryLedger.cs` recording discrete, immutable memory events keyed by unique NPC ID and event category.
5. Define memory event types: `DebtOwed`, `Betrayal`, `LifeSaved`, `ResourceGift`, `BrokenPromise`, `WitnessedCrime`.
6. Separate information states: memories may be `PrivateToNpc`, `ShelterKnown`, or `PublicRumor` via `DayEventVocabulary`.
7. Implement `NpcPersonalStanceEngine.cs` projecting personal trust and willingness to bargain from active memories.
8. Connect personal memory deltas to dialogue selection: NPCs reference specific past encounters in their greeting barks.
9. Connect personal memory to trade price multipliers: grateful merchants offer discounts; swindled traders impose surcharges.
10. Wire door encounter decisions: NPCs who remember being sheltered during past fallout storms return with gifts or intelligence.
11. Implement memory retention policy: emotional intensity decays over campaign days, but profound acts (saving a child) never expire.
12. Ensure memory recording is strictly deterministic: identical player dialogue choices record identical memory records.
13. Update `HoldfastNpcDetailPanel.cs` to present an explainable relationship summary ("Trusts you: You shared medicine on Day 14").
14. Ensure memory events do not double-count with faction standing: personal memory modifies individual interaction only.
15. Add unit tests in `Ashfall.Core.Tests/Narrative/Plan147NpcMemoryTests.cs`.
16. Validate that NPC memory ledgers serialize cleanly inside `NpcSaveStore` without breaking existing campaign save envelopes.
17. Verify that loading an earlier save restores the exact memory state present at the time of the save.
18. Run `--data-integrity-selftest` to ensure all NPC IDs referenced in memory data match valid catalog definitions.
19. Run narrative and social focused test suites to confirm zero regressions in door encounters or trade sessions.
20. Document NPC memory architecture in `docs/narrative/NPC_MEMORY_SYSTEM.md` and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### E1.1 Memory Event Schema & Ledger Architecture
- (a) Define immutable struct `NpcMemoryEvent` (EventId, NpcId, MemoryKind, CampaignDay, Magnitude, ReasonKey).
- (b) Author `NpcMemoryLedger` with indexed queries: `GetMemoriesForNpc(string npcId)` and `GetNetAffinity(string npcId)`.
- (c) Enforce a per-NPC capacity limit (max 10 active memories) evicting low-significance entries first.
- (d) Author unit tests verifying that recording memories updates net personal affinity deterministically.

#### E1.2 Dialogue & Greeting Variation Adapter
- (a) Author `NpcDialogueMemoryAdapter` selecting dialogue barks based on the NPC's most intense active memory.
- (b) Add memory-specific dialogue variations in `npc_dialogues.json` for key settlement traders and wanderers.
- (c) Ensure dialogue tokens safely substitute relevant player details without string formatting exceptions.
- (d) Author unit tests asserting that high-gratitude memories unlock dedicated gratitude dialogue branches.

#### E1.3 Personal Trade Stance Modulation
- (a) Wire `HoldfastTradeSession` to query `NpcPersonalStanceEngine` for individual barter markup deltas.
- (b) Clamp personal price modifiers within safe economic boundaries (maximum 0.75x discount; maximum 1.50x markup).
- (c) Prevent double-counting: personal stance modulates the base quote after faction regional pricing has resolved.
- (d) Author unit tests proving that personal gratitude lowers trading costs independently of faction standing.

#### E1.4 Memory Persistence & Decay Dynamics
- (a) Implement linear memory decay logic in `NpcMemoryLedger.AdvanceDay` reducing magnitude of minor memories.
- (b) Flag indelible memories (e.g. companion rescue, lethal betrayal) with `IsPermanent = true` to bypass decay.
- (c) Serialize memory records into versioned DTO `NpcMemorySaveState` within the existing save section.
- (d) Test save/load round-trips to verify that memory expiration timers resume accurately on reload.

---

## TASK E2 — C1[25] Working Animals & Companion Operating Roles (Plan 151A/151B/151C/151D)

- **Source Plan:** `C-integration-plans/C1_planintegration[25].md` (Plan 151)
- **Blocker Class:** LIMITED COMPANION ROLES / LACK OF OPERATIONAL VALUE
- **Canonical Owner:** `Assets/Ashfall.Core/Ecology/`, `Assets/Ashfall.Core/Expeditions/`
- **Target Subsystem:** Companion operating roles, pack carrying capacity, pest control, defense duty

### 20 Procedural Substeps:
1. Review current companion animal infrastructure: `CompanionAnimalSystem`, `companion_animals.json`, kennel slots.
2. Confirm the core architectural rule: once tamed, animals are persistent campaign entities, not inventory items.
3. Claim `Assets/Ashfall.Core/Ecology/CompanionOperatingRoles.cs` and `src/Host/CompanionSaveStore.cs`.
4. Define three primary operational roles for companions: `PackMule` (logistics), `GuardDog` (defense), `Mouser` (sanitation).
5. Wire `PackMule` companions assigned to wasteland expeditions to expand party cargo capacity by +25kg in `ExpeditionSystem`.
6. Wire `GuardDog` companions assigned to shelter security to increase intrusion detection and repel small mutant incursions.
7. Wire `Mouser` companions (felines) assigned to shelter food stores to suppress rodent infestations and spoilage ticks.
8. Connect companion animal presence to survivor psychological wellness: petting or living near companions reduces stress.
9. Implement daily feed consumption: working animals require daily rations of raw meat, grain, or scraps from inventory.
10. Implement starvation consequences: unfed working animals suffer reduced efficiency, illness, or flee the shelter.
11. Implement companion mortality and grief: animal loss in combat or to disease emits grief events to bonded caretakers.
12. Ensure animal operational assignments integrate cleanly into `DutyRosterPanel.cs` without duplicating roster models.
13. Update `KennelPanel.cs` to allow assigning tamed animals to active operational shelter roles.
14. Ensure animal roles do not create separate combat AI engines; guard animals provide combat modifiers to handlers.
15. Add unit tests in `Ashfall.Core.Tests/Ecology/Plan151CompanionOperatingRolesTests.cs`.
16. Validate that assigned animal roles and health states serialize cleanly within `CompanionSaveStore`.
17. Verify deterministic behavior: identical seeds produce identical animal recovery and hunting outputs.
18. Run `--data-integrity-selftest` to ensure all companion role definitions in `companion_animals.json` validate.
19. Run shelter and expedition focused test suites to confirm zero regressions in food consumption or travel speed.
20. Document companion operating roles in `docs/systems/COMPANION_WORKING_ROLES.md` and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### E2.1 Operational Role Assignment Framework
- (a) Define enum `CompanionWorkRole` (Unassigned, PackHauler, FacilityGuard, PestControl, Therapy).
- (b) Author `CompanionOperatingRoles` manager tracking assigned roles per living companion instance.
- (c) Validate compatibility: assert that only specific species archetypes can fill heavy pack or guard roles.
- (d) Author unit tests verifying that assigning, unassigning, and switching companion roles updates modifiers instantly.

#### E2.2 Logistics & Expedition Pack Carry Integration
- (a) Update `ExpeditionHostSession.PrepareVehicleForDispatch` to include assigned pack animal carrying capacity.
- (b) Apply terrain movement speed adjustments based on pack animal endurance in rough wasteland sectors.
- (c) Wire expedition casualty checks so severe combat encounters can injure or endanger assigned pack animals.
- (d) Author unit tests verifying that expeditions carry extra salvage weight when accompanied by pack companions.

#### E2.3 Food Storage Pest Control & Sanitation
- (a) Connect felines assigned to `PestControl` to the daily food spoilage evaluation tick in `KitchenNutritionSystem`.
- (b) Reduce grain and dried meat spoilage chances by 75% when an active mouser is stationed in the pantry.
- (c) Emit occasional flavor journal entries ("Barnaby caught two contaminated vault rats near the grain bin").
- (d) Author unit tests proving that food spoilage rates decrease measurably when a pest controller is active.

#### E2.4 Companion Feed Drain & Bonded Morale
- (a) Extend daily shelter consumption in `InventoryHostSession` to deduct required animal feed units.
- (b) Implement bonded caretaker links: pair each companion with a primary survivor handler during training.
- (c) Apply passive morale regeneration (+2/day) to the primary handler while their companion is healthy and fed.
- (d) Author unit tests validating that companion starvation triggers handler distress and morale drop.

---

## TASK E3 — C1[26] Black Market Contraband, Risk & Enforcement Waves (Plan 155A/155B/155C/155D)

- **Source Plan:** `C-integration-plans/C1_planintegration[26].md` (Plan 155)
- **Blocker Class:** SHALLOW UNDERGROUND TRADE / LACK OF CONTRABAND RISK
- **Canonical Owner:** `Assets/Ashfall.Core/Economy/`, `Assets/Ashfall.Core/Factions/`
- **Target Subsystem:** Contraband legality tags, search checkpoints, syndicate heat, law enforcement raids

### 20 Procedural Substeps:
1. Review current black market baseline: `BlackMarketSystem`, `BlackMarketSettlementService`, Buy/Sell/Loan/Repay actions.
2. Confirm the core economic principle: black market profit must come from risk, scarcity, and heat, not free markup.
3. Claim `Assets/Ashfall.Core/Economy/ContrabandRiskEngine.cs` and `Assets/Ashfall.Core/Factions/EnforcementRaidCoordinator.cs`.
4. Author `ContrabandRiskEngine.cs` evaluating carried inventory items possessing the `contraband` tag.
5. Define contraband categories: `MilitaryOrdnance`, `BannedNarcotics`, `StolenRelics`, `ToxicChemicals`.
6. Wire wasteland travel checkpoints: expeditions carrying contraband risk inspection when passing near faction outposts.
7. Implement concealment mechanics: specialized shielded cargo compartments in vehicles reduce inspection detection chance.
8. Connect failed inspections to confiscation, immediate faction standing penalties, and potential combat arrests.
9. Implement underground syndicate heat accumulation: high-volume black market trading generates local authority attention.
10. Wire enforcement raids: when syndicate heat exceeds critical thresholds, hostile faction marshals raid the shelter perimeter.
11. Implement heat dissipation: heat decays naturally when black market transactions cease for consecutive campaign days.
12. Ensure contraband trading records cleanly in `DailyBriefingReportBuilder` under security warnings.
13. Update `BlackMarketPanel.cs` to render active syndicate heat and inspection risk percentages on stock items.
14. Ensure confiscation removes only illegal goods, leaving legitimate food, scrap, and common weapons intact.
15. Add unit tests in `Ashfall.Core.Tests/Economy/Plan155ContrabandRiskTests.cs`.
16. Validate that syndicate heat and dealer trust persist cleanly in `BlackMarketSaveStore` across game reloads.
17. Verify deterministic checkpoint detection: identical seeds and stealth profiles yield identical inspection rolls.
18. Run `--data-integrity-selftest` to ensure all contraband items declare proper classification tags in `items.json`.
19. Run economy and faction focused test suites to confirm zero regressions in legal trade or caravan movement.
20. Document underground economy mechanics in `docs/economy/CONTRABAND_AND_HEAT_SYSTEM.md` and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### E3.1 Contraband Item Classification & Tagging
- (a) Audit `items.json` and tag high-tier illicit goods (military stims, untraced weapons) with `contraband` tags.
- (b) Assign specific faction jurisdiction restrictions (e.g. Vanguard bans tech relics; Covenant bans narcotics).
- (c) Expose helper `ItemTagCatalog.IsContrabandInRegion(string itemId, string regionId)` for rapid queries.
- (d) Author unit tests verifying that legal survival goods (clean water, canned beans) are never flagged contraband.

#### E3.2 Travel Checkpoint Search & Concealment Engine
- (a) Intercept expedition route segment execution in `ExpeditionHostSession` when crossing faction territory.
- (b) Roll detection checks comparing party stealth and vehicle shielding against faction patrol vigilance.
- (c) On detection, provide player tactical choices: "Bribe Patrol", "Surrender Contraband", or "Fight Way Through".
- (d) Author unit tests proving that shielded cargo containers reduce inspection detection probability by 60%.

#### E3.3 Syndicate Heat Accumulation & Decay
- (a) Increment syndicate heat by +5 to +20 points for each high-value contraband transaction completed.
- (b) Implement daily heat decay (-2 points/day) during periods of legitimate trade inactivity.
- (c) Trigger regional market closures and dealer temporary disappearances when heat reaches maximum alert.
- (d) Author unit tests validating that heat accumulates proportionally to traded volume and decays stably over time.

#### E3.4 Law Enforcement Raid Dispatch
- (a) Connect critical heat levels (>= 80) to dispatch hostile enforcer squads targeting shelter entrance defenses.
- (b) Route incoming raids through `SkyDefenseBatterySystem` and perimeter combat resolution handlers.
- (c) Successfully repelling or bribing an enforcement raid resets active syndicate heat to moderate levels.
- (d) Author integration tests verifying that high heat levels reliably escalate external combat threats.

---

## TASK E4 — C1[27] Shelter Governance, Legitimacy & Policy Decrees (Plan 159A/159B/159C/159D)

- **Source Plan:** `C-integration-plans/C1_planintegration[27].md` (Plan 159)
- **Blocker Class:** POLICY GAP / UNSTRUCTURED SHELTER RULES
- **Canonical Owner:** `Assets/Ashfall.Core/Shelter/`, `Assets/Ashfall.Core/Social/`
- **Target Subsystem:** Shelter governance policies, council legitimacy, decree enactment, unrest resolution

### 20 Procedural Substeps:
1. Review current shelter management controls: ration allocations, duty assignments, medical priorities.
2. Confirm the core architectural rule: governance owns policy intent and legitimacy; it does not duplicate operations.
3. Claim `Assets/Ashfall.Core/Shelter/ShelterGovernanceSystem.cs` and `src/UI/ShelterGovernancePanel.cs`.
4. Author `ShelterGovernanceSystem.cs` managing active shelter policies, decree status, and community legitimacy.
5. Define policy domains: `RationingProtocol`, `LaborMandates`, `MedicalTriagePriority`, `SecurityCurfew`.
6. Implement community legitimacy score (0 to 100): high legitimacy ensures compliance; low legitimacy breeds unrest.
7. Wire decree enactment: enacting harsh emergency decrees (e.g. "Strict Water Rationing") consumes legitimacy.
8. Connect policy effects to operational systems: "Extended Duty Hours" decree increases labor output but raises overwork risk.
9. Connect policy effects to medical systems: "Triage Priority" decree directs scarce antibiotics to working adults over terminal patients.
10. Implement community vote/consultation procedure: consulting the survivor council increases policy acceptance and legitimacy.
11. Implement civil unrest triggers: legitimacy dropping below 25 triggers work strikes, equipment sabotage, or theft events.
12. Ensure decrees emit canonical day events (`OnPolicyEnacted`, `OnUnrestEscalated`) through `DayEventVocabulary`.
13. Update `DailyBriefingReportBuilder` to summarize current active shelter decrees and community morale response.
14. Create `ShelterGovernancePanel.cs` presenting current laws, council approval, legitimacy bars, and decree toggle controls.
15. Add unit tests in `Ashfall.Core.Tests/Shelter/Plan159ShelterGovernanceTests.cs`.
16. Validate that active decrees and legitimacy scores serialize cleanly inside `GovernanceSaveStore`.
17. Verify deterministic policy outcomes: identical survivor council compositions produce identical voting verdicts.
18. Run `--panel-bind-lifecycle-selftest` to ensure `ShelterGovernancePanel.cs` disposes subscriptions cleanly.
19. Run social, duty, and nutrition focused test suites to confirm zero regressions in basic operations.
20. Document shelter political systems in `docs/shelter/SHELTER_GOVERNANCE_AND_LAWS.md` and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### E4.1 Decree Registry & Effect Adapters
- (a) Define data schema `shelter_decrees.json` specifying decree ID, domain, legitimacy cost, and operational modifiers.
- (b) Implement policy adapters applying active decree multipliers into `WorkerProductivityContract` and `NeedsSystem`.
- (c) Prevent conflicting laws: assert that mutually exclusive decrees (e.g. "Generous Portions" vs "Starvation Rations") cannot co-exist.
- (d) Author unit tests verifying that enacting and repealing decrees instantly adjusts operational multipliers.

#### E4.2 Community Legitimacy & Council Consensus
- (a) Model community legitimacy as a bounded scalar derived from past promises, crisis handling, and food security.
- (b) Implement council vote calculation based on survivor ideological alignments (Pragmatist, Egalitarian, Authoritarian).
- (c) High consensus decrees yield minimal unrest; unilateral decrees enacted without council backing deplete legitimacy.
- (d) Author unit tests proving that survivor happiness and food stability restore community legitimacy over time.

#### E4.3 Civil Unrest & Sabotage Event Cascade
- (a) Monitor daily legitimacy in `ShelterSocialCoordinator` and trigger unrest checks when legitimacy is low (< 30).
- (b) Wire unrest events: survivors refuse shifts in `DutyRosterSystem` or steal medical supplies in `InventorySystem`.
- (c) Provide player resolution commands: "Concede to Council Demands", "Enforce Martial Discipline", or "Negotiate".
- (d) Author unit tests validating that resolving worker grievances restores normal duty shift attendance.

#### E4.4 Governance UI & Decree Enactment Surface
- (a) Implement `ShelterGovernancePanel.cs` with categorized decree lists and detailed trade-off tooltips.
- (b) Render visual community approval gauges indicating predicted legitimacy impacts before enacting laws.
- (c) Ensure keyboard navigation traversal across decree toggles and confirmation modal dialogs.
- (d) Run `--ui-a11y-selftest` to verify that political status dashboards satisfy contrast and layout requirements.

---

## TASK F1 — C1[28] Shelter Identity, Naming, Origin & Community Projection (Plan 166A/166B/166C/166D)

- **Source Plan:** `C-integration-plans/C1_planintegration[28].md` (Plan 166)
- **Blocker Class:** NARRATIVE IDENTITY GAP / GENERIC BUNKER STATE
- **Canonical Owner:** `Assets/Ashfall.Core/Shelter/`, `src/Host/`
- **Target Subsystem:** Shelter custom naming, founding origin selection, community reputation projection

### 20 Procedural Substeps:
1. Review current shelter initialization in `GameBootstrap.cs` and `ShelterHostSession.cs`.
2. Confirm the core defect: the shelter is currently referred to generically as "the shelter" or "Holdfast" in all prose.
3. Claim `Assets/Ashfall.Core/Shelter/ShelterIdentityModel.cs` and `src/UI/ShelterIdentityPanel.cs`.
4. Author `ShelterIdentityModel.cs` storing custom shelter name, founding origin archetype, motto, and community reputation.
5. Define canonical founding origins in `shelter_origins.json`: `MiningOutpost`, `MedicalVault`, `MilitaryBunker`, `AgronomyStation`.
6. Apply founding origin starting bonuses: `MedicalVault` begins with advanced diagnostic beds; `MiningOutpost` with extra scrap.
7. Ensure origin bonuses execute via canonical inventory and facility bootstrap transactions without hardcoded mutations.
8. Implement dynamic token replacement: replace `{shelter_name}` in journal entries, daily briefings, and radio scripts.
9. Derive community reputation projection ("Known For"): compute descriptive tags based on player actions (e.g. "Sanctuary for Sick").
10. Ensure community reputation is a pure read model derived from genuine facts; do not create duplicate reputation meters.
11. Bind `Main.GameFlow.cs` onboarding flow to prompt the player for custom shelter naming upon starting a new campaign.
12. Ensure shelter naming enforces character limits (3 to 24 characters) and filters invalid filesystem characters.
13. Update HUD header and `ShelterHudPanel.cs` to proudly display the custom shelter name and community origin emblem.
14. Ensure shelter identity fields persist cleanly within `ShelterSaveStore` across all save/load operations.
15. Add unit tests in `Ashfall.Core.Tests/Shelter/Plan166ShelterIdentityTests.cs`.
16. Validate that legacy save files lacking custom shelter identity default gracefully to "Holdfast" without errors.
17. Verify deterministic origin generation: identical origin selections yield identical starting inventory allocations.
18. Run `--data-integrity-selftest` to ensure `shelter_origins.json` validates against catalog schema policy.
19. Run UI lifecycle and accessibility selftests on newly authored identity setup dialogs.
20. Document shelter identity architecture in `docs/shelter/SHELTER_IDENTITY_AND_ORIGINS.md` and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### F1.1 Shelter Identity Model & Tokenizer
- (a) Define record `ShelterIdentityState` (ShelterName, OriginId, Motto, FoundedDay, CommunityTags).
- (b) Implement string tokenization utility `TextTokenFormatter.Format(string template, ShelterIdentityState identity)`.
- (c) Substitute `{shelter_name}` across dialogue scripts, daily briefing headers, and milestone achievements.
- (d) Author unit tests asserting that token replacement accurately injects custom names across sample narrative strings.

#### F1.2 Founding Origin Catalog & Bootstrap Integration
- (a) Structure `shelter_origins.json` specifying starting item bundles, room bonuses, and starting survivor traits.
- (b) Connect selected origin to `GameBootstrap.InitializeNewCampaign` to distribute starting supplies atomically.
- (c) Ensure starting bonuses strictly respect inventory capacity limits and facility structural slots.
- (d) Author unit tests proving that each origin archetype initializes with its documented resource profile.

#### F1.3 Derived Community Reputation Projection
- (a) Author `CommunityReputationEvaluator` computing community reputation tags from campaign history.
- (b) Derive tags based on facts: "Merciful Haven" if >= 10 refugees admitted; "Fortress" if >= 5 raids repelled.
- (c) Expose derived tags to visiting caravans and radio contacts to flavor their initial greetings.
- (d) Author unit tests verifying that community reputation tags update dynamically as historical milestones are achieved.

#### F1.4 Onboarding Setup Surface & HUD Display
- (a) Implement initial shelter naming modal dialog displayed during campaign prologues.
- (b) Connect text input fields with keyboard focus validation and length bounds checking.
- (c) Update top-level HUD banner in `ShelterHudPanel.cs` to render the custom shelter title and origin crest.
- (d) Run `--ui-a11y-selftest` to ensure the shelter naming modal is fully keyboard-operable and screen-reader compliant.

---

## TASK F2 — C2[23] Stateful Ambience, Mix Discipline & Sparse Musical Arc (Plan 52A/52B/52C)

- **Source Plan:** `C-integration-plans/C2_planintegration[23].md` (Plan 52)
- **Blocker Class:** AUDIO FLATNESS / LACK OF DYNAMIC MIX DISCIPLINE
- **Canonical Owner:** `src/Audio/`, `Assets/Ashfall.Core/Audio/`
- **Target Subsystem:** Dynamic ambient soundscapes, audio bus ducking, alert concurrency caps, sparse music arc

### 20 Procedural Substeps:
1. Review current audio setup: 12 active audio buses, 196+ audio cues, `AudioManager.cs`, and `AudioEventBridge.cs`.
2. Confirm the core rule: do not add new buses or mutate simulation state; audio purely reflects domain events.
3. Claim `src/Audio/SurfaceAmbienceController.cs`, `src/Audio/AudioStateCoordinator.cs`, and `audio_cues.json`.
4. Author `SurfaceAmbienceController.cs` dynamically managing ambient sound loops based on current location and weather.
5. Map weather states to distinct ambient loops: Blizzards, Fallout Storms, Rain, and eerie silence during Still Wind.
6. Connect shelter room power states to interior loops: running generators, hum of ventilation, dripping water filters.
7. Implement audio bus ducking in `AudioStateCoordinator.cs`: incoming radio calls and emergency sirens duck background music by -12dB.
8. Implement alert concurrency caps: prevent ear-fatigue by limiting simultaneous alarm cues to a maximum of 2 voices.
9. Enforce priority rules: radiation Geiger clicks and hull breach alarms supersede casual UI clicks and ambient barks.
10. Author sparse musical cues in `audio_cues.json` triggered only on milestone transitions (Season Turn, Survivor Death).
11. Ensure silence is treated as an intentional aesthetic state; never fill quiet post-apocalyptic pauses with generic filler music.
12. Wire Geiger audio loop to stop cleanly upon radiation dose reaching zero; eliminate lingering audio loops.
13. Implement audio volume scaling in user settings (Master, Ambience, SFX, Voice, Music) with logarithmic curves.
14. Ensure all audio playback respects the headless execution mode: audio nodes must be silent no-ops under `--headless`.
15. Add unit tests in `Ashfall.Core.Tests/Audio/Plan52AudioAmbienceTests.cs`.
16. Validate that ambient sound loops transition smoothly with cross-fading (2.0-second fade curve) without popping.
17. Verify that audio playback introduces zero garbage collection allocations during continuous background streaming.
18. Run `--audio-selftest` to ensure all 196+ audio cues resolve and bus ducking snapshots execute cleanly.
19. Verify that audio settings save and restore properly across application restarts.
20. Document audio landscape and mix hierarchy in `docs/audio/AUDIO_MIX_AND_AMBIENCE_GUIDE.md` and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### F2.1 Environmental Ambience Controller
- (a) Author `SurfaceAmbienceController` subscribing to `OnWeatherChanged` and `OnLocationChanged` events.
- (b) Select ambient background streams matching wasteland sector ecology (Wasteland, Ruined City, Toxic Marsh).
- (c) Implement cross-fading logic transitioning volume smoothly between previous and incoming ambient loops.
- (d) Author unit tests verifying that changing weather initiates proper cross-fade transitions.

#### F2.2 Bus Ducking & Snapshot Coordination
- (a) Configure AudioServer bus layout ducking sends on `Radio`, `Alerts`, and `Voice` buses.
- (b) Trigger ducking snapshots when radio broadcasts begin, attenuating `Ambience` and `Music` buses by -12dB.
- (c) Release ducking snapshots cleanly when voice transmission completes, smoothly restoring baseline volumes.
- (d) Author verification tests proving that ducking state transitions are idempotent and bounded.

#### F2.3 Emergency Alert Concurrency Caps
- (a) Implement voice reservation in `AudioManager` restricting concurrent high-priority alerts to 2 instances.
- (b) Drop or queue incoming lower-priority alerts when maximum emergency voice channels are occupied.
- (c) Track dropped alerts via `OneShotDroppedCount` telemetry for audio diagnostic profiling.
- (d) Author unit tests asserting that simultaneous emergency alarms do not cause acoustic distortion or clipping.

#### F2.4 Milestone Musical Arc Triggering
- (a) Map musical stingers to high-significance milestones: Campaign Start, Day 100, First Death, Major Ending.
- (b) Ensure musical stingers play once and gracefully yield back to atmospheric natural silence.
- (c) Restrict background music during normal gameplay to maintain the stark, lonely survival atmosphere.
- (d) Run `--audio-selftest` to confirm that all milestone musical cue assets exist and resolve cleanly.

---

## TASK F3 — C2[24] Retention Policies, Save Corpus Archaeology & 400-Year Scale (Plan 55A/55B/55C)

- **Source Plan:** `C-integration-plans/C2_planintegration[24].md` (Plan 55)
- **Blocker Class:** UNBOUNDED DATA GROWTH / LONG-CAMPAIGN HAZARDS
- **Canonical Owner:** `Assets/Ashfall.Core/Save/`, `docs/saves/`
- **Target Subsystem:** History vector compaction, retention policies, 400-year multi-generational stability

### 20 Procedural Substeps:
1. Review growing data collections: `JournalSystem`, `MemorialSystem`, `SurvivorRelationsSystem`, `DayEventVocabulary`.
2. Confirm the core risk: unconstrained history vectors cause save file bloat, slow load times, and memory leaks on long campaigns.
3. Claim `Assets/Ashfall.Core/Save/SaveRetentionPolicy.cs` and `scripts/ci/verify_save_corpus.py`.
4. Define explicit retention and compaction rules for every collection that grows over campaign days.
5. Implement event log summarization: after 60 campaign days, compress detailed daily event logs into monthly summary records.
6. Implement deceased survivor memorial archiving: retain names, death days, and causes, but prune ephemeral combat logs.
7. Bound relationship pair history: maintain a strict maximum of 5 recent interactions per survivor pair in `PairHistoryLedger`.
8. Salvage and version the historical save corpus (`holdfast_archive_*`) in `artifacts/save_corpus/` under Git LFS tracking.
9. Implement multi-generational campaign simulation test: execute a 400-year headless simulation across survivor generations.
10. Assert that save file size scales logarithmically and never exceeds 5MB even after 400 simulated campaign years.
11. Assert that save deserialization and load times remain under 500ms regardless of campaign age.
12. Ensure history compaction preserves player-critical facts: promises made, oaths taken, and major faction decisions are never purged.
13. Implement save file defragmentation: clean up tombstoned records during atomic save serialization.
14. Ensure memory compaction emits zero warnings and strictly maintains culture-invariant serialization formatting.
15. Add unit tests in `Ashfall.Core.Tests/Save/Plan55SaveRetentionTests.cs` validating compaction algorithms.
16. Validate that restoring a compacted save reproduces identical simulation state to an uncompacted continuous run.
17. Verify that save schema versioning remains backwards-compatible with early uncompacted campaign saves.
18. Run `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` across all historical corpus saves.
19. Run `--data-integrity-selftest` to verify that all archived save data contracts remain compliant.
20. Document retention rules in `docs/saves/SAVE_RETENTION_AND_SCALABILITY.md` and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### F3.1 History Compaction Engine Implementation
- (a) Author `SaveRetentionPolicy` defining retention time-windows for each growing save section.
- (b) Implement `JournalSystem.CompactHistory` consolidating daily entries older than 60 days into historical chronicles.
- (c) Preserve critical narrative anchors: milestone achievements and major endings are marked permanent.
- (d) Author unit tests asserting that daily log entries older than the retention threshold are cleanly summarized.

#### F3.2 Memorial & Graveyard Record Archiving
- (a) Audit `MemorialSystem` records to separate essential memorial epitaphs from temporary status counters.
- (b) Prune obsolete temporary modifier references from deceased survivor records during campaign rollover.
- (c) Guarantee that the total count of fallen survivors and their cenotaph markers remain 100% accurate.
- (d) Author unit tests proving that memorial save payload sizes stabilize after survivor casualty waves.

#### F3.3 Historical Save Corpus Archaeology
- (a) Collate all historical save files from `holdfast_archive_*` and normalize their filenames with metadata tags.
- (b) Verify that every corpus file is tracked via Git LFS in `.gitattributes`.
- (c) Create automated test `SaveCorpusCompatibilityTests.cs` loading every archived save file into current engine code.
- (d) Verify that 100% of historical save corpus files load with zero schema migration exceptions.

#### F3.4 400-Year Generational Scale Proof
- (a) Author headless soak test `Soak400YearCampaignTests.cs` advancing campaign state through generational turnover.
- (b) Measure memory footprint at Year 10, Year 50, Year 100, and Year 400, asserting zero progressive memory leakage.
- (c) Compare save payload byte sizes across the 400-year timeline to prove bounded storage overhead.
- (d) Verify that multi-generational lineage and ancestry records remain fully intact upon reaching Year 400.

---

## TASK F4 — C2[25] Retrospective Standing Gates & Plan-Layer Compression (Plan 59A/59B/59C)

- **Source Plan:** `C-integration-plans/C2_planintegration[25].md` (Plan 59)
- **Blocker Class:** PROCESS FATIGUE / RECURRING DEFECT PATTERNS
- **Canonical Owner:** `scripts/ci/`, `docs/`
- **Target Subsystem:** Standing CI gate compression, retrospective rules synthesis, permanent intake cycle

### 20 Procedural Substeps:
1. Review the history of all nine audit and implementation waves to synthesize recurring failure modes.
2. Identify the recurring defect archetypes: unwired seams, invented mechanics, false presence, unproven claims.
3. Claim `scripts/ci/verify_standing_gates.sh` and `docs/retrospective/NINE_WAVES_RETROSPECTIVE.md`.
4. Synthesize all individual wave-specific check scripts into unified, self-documenting standing CI gates.
5. Gate 1: Enforce zero Unity references across the entire repository.
6. Gate 2: Enforce zero Godot engine references inside `Assets/Ashfall.Core/`.
7. Gate 3: Enforce JSON data schema policy and snake_case naming across all 333+ data files.
8. Gate 4: Enforce that every `.cs` source file has a corresponding `.cs.uid` sidecar file.
9. Gate 5: Enforce that every declared input action in `AshfallInputActions` exists in Godot input mappings.
10. Gate 6: Enforce that every audio cue in `audio_cues.json` resolves to a real audio stream file.
11. Gate 7: Enforce that all registered player panels satisfy accessibility, focus, and disposal lifecycle tests.
12. Gate 8: Enforce that seeded replay simulations remain byte-identical across save/load boundaries.
13. Author comprehensive retrospective document `docs/retrospective/NINE_WAVES_RETROSPECTIVE.md`.
14. Document the audit series' own historical mistakes: false assumptions, premature claims, and duplicate tracking.
15. Archive completed historical plan files from root directories into `docs/archive/plans/` with clear index receipts.
16. Establish the permanent post-audit development operating cycle: Intake → Plan → Implement → Verify → Release.
17. Ensure `scripts/ci/verify-fast.sh` executes the consolidated standing gate suite in under 30 seconds.
18. Validate that all CI scripts return explicit non-zero exit codes upon failure and produce structured error summaries.
19. Re-run `scripts/ci/verify_standing_gates.sh` to confirm that all standing gates pass 100% on current repository `HEAD`.
20. Update `docs/INDEX.md` and repository README with the permanent development lifecycle and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### F4.1 Standing CI Gate Consolidation
- (a) Author `scripts/ci/verify_standing_gates.sh` combining static barriers, schema checks, and LFS audits.
- (b) Provide clean terminal output summarizing pass/fail status for each consolidated gate tier.
- (c) Ensure the script accepts `--quick` flag for rapid pre-commit validation during local development.
- (d) Author automated tests verifying that the standing gate runner correctly catches deliberate defect injections.

#### F4.2 Nine Waves Retrospective Synthesis
- (a) Collate root causes for the 59 historical GAP rows and document lessons learned across all waves.
- (b) Articulate the core engineering doctrine: "Evidence first; one owner per concern; presentation is not simulation".
- (c) Document historical false premises (e.g. WornGear duplication, redundant condition systems) as cautionary studies.
- (d) Format retrospective document according to ASHFALL technical publication and Markdown standards.

#### F4.3 Plan Layer Compression & Archiving
- (a) Move completed historical plan markdown files into `docs/archive/plans/completed_waves/`.
- (b) Generate an authoritative archive index `docs/archive/plans/ARCHIVE_INDEX.md` cross-referencing plan numbers.
- (c) Update active plan indexes in `docs/INDEX.md` to point to current living roadmaps.
- (d) Verify that no broken links or orphaned documentation references remain across the documentation tree.

#### F4.4 Permanent Development Operating Cycle
- (a) Define the 5-stage operating lifecycle: Intake Ticket → Focused Plan → Implementation → Gate Pass → Release Tag.
- (b) Create standard issue templates for feature intake, bug repair, and balance re-tuning.
- (c) Formalize rules prohibiting the opening of ad-hoc audit backlogs without active implementation commitments.
- (d) Publish `docs/governance/DEVELOPMENT_OPERATING_CYCLE.md` as the permanent team coordination standard.

---

# 2. CROSS-TASK VERIFICATION MATRIX

| Task | Domain | Focused Test Suite | Build Gate | Data Integrity | Replay / Determinism | UI / Audio Lifecycle | Handoff Target |
|---|---|---|---|---|---|---|---|
| **E1** | NPC Memory | `Plan147NpcMemoryTests.cs` | Required (0/0) | Required (npcs.json) | Deterministic affinity | Dialogue refresh | `NPC_MEMORY.md` |
| **E2** | Animal Roles | `Plan151CompanionRolesTests.cs`| Required (0/0) | Required (animals.json) | Seeded feed drain | `--panel-bind-lifecycle` | `COMPANION_ROLES.md` |
| **E3** | Contraband & Risk | `Plan155ContrabandRiskTests.cs`| Required (0/0) | Required (items.json) | Seeded checkpoint roll | `--ui-a11y-selftest` | `CONTRABAND_RISK.md` |
| **E4** | Shelter Governance| `Plan159ShelterGovTests.cs` | Required (0/0) | Required (decrees.json) | Council vote identity | `--panel-bind-lifecycle` | `SHELTER_GOV.md` |
| **F1** | Shelter Identity | `Plan166ShelterIdTests.cs` | Required (0/0) | Required (origins.json) | Tokenizer parity | `--ui-a11y-selftest` | `SHELTER_IDENTITY.md` |
| **F2** | Dynamic Mix & Audio| `Plan52AudioAmbienceTests.cs` | Required (0/0) | Required (cues.json) | Bounded ducking state | `--audio-selftest` | `AUDIO_MIX_GUIDE.md` |
| **F3** | Save Retention | `Plan55SaveRetentionTests.cs` | Required (0/0) | Required (corpus) | 400-year soak equality | Memory leak check | `SAVE_RETENTION.md` |
| **F4** | Standing Gates | `verify_standing_gates.sh` | Required (0/0) | Required (full catalog) | CI gate determinism | Clean shell execution | `RETROSPECTIVE.md` |

---

# 3. TASK HANDOFF TEMPLATE

Every completed task must publish a handoff document adhering to this structure:

```markdown
## TASK <ID> HANDOFF: <TITLE>

### 1. Workspace & Authority
- Commit / HEAD tested:
- Claimed paths in `WORKTREE_OWNERSHIP.md`:
- Concurrent claims checked:

### 2. Premise Re-Verification
- Historical blocker statement:
- Verified state at execution start:
- Stale assumptions corrected:

### 3. Decisions & Signatures
- Signed decisions consumed:
- Deferred sub-scopes recorded:

### 4. Implementation Details
- Canonical authority modified:
- Old path / New path:
- Files created / modified / deleted:
- Generated assets regenerated:

### 5. Verification & Testing
- Focused test command & exact results:
- Headless selftest results (`--data-integrity`, `--audio`, `--ui-a11y`):
- Determinism / Replay fingerprint results:
- Build output (Errors: 0, Warnings: 0):

### 6. Ledger & Documentation Updates
- `INTEGRATION_PLANS.md` status:
- Architecture / Catalog maps updated:
- Next unblocked dependency:
```

---

# 4. WAVE-LEVEL FAILURE & STOP POLICY

Stop execution and escalate to the foreman/user immediately if any of the following occur:
1. **Missing Domain Owner:** A task requires behavior that lacks an existing canonical authority, prompting the creation of an unauthorized manager or UI-side calculation.
2. **Signature Boundary:** A balance multiplier, permanent deferral, or breaking contract requires approval and lacks a signed record.
3. **Claim Overlap:** A path required for implementation is actively claimed in `WORKTREE_OWNERSHIP.md`.
4. **Premise Invalidation:** Code inspection proves that the problem was already resolved or that current code contradicts the plan's architectural premise.
5. **Determinism Break:** Seeded simulations diverge across runs or save/load cycles.
6. **Engine Infiltration:** Godot or engine assemblies leak into `Assets/Ashfall.Core/`.

---

# 5. DEFINITION OF DONE FOR WAVE 12 PART 3

Wave 12 Part 3 is complete only when all eight tasks have achieved an allowed terminal state (`IMPLEMENTED` or `DECIDED-DEFERRED`), verified by:
- All 8 tasks have executed all 20 procedural substeps and structured mini-tasks without omissions.
- `dotnet build Ashfall.csproj` completes with **0 Errors and 0 Warnings**.
- `godot --headless --path . -- --data-integrity-selftest` passes with **0 Errors across 333+ catalogs**.
- `godot --headless --path . -- --audio-selftest` passes with **0 Failures**.
- `godot --headless --path . -- --panel-bind-lifecycle-selftest` and `--ui-a11y-selftest` pass cleanly.
- `scripts/ci/verify_standing_gates.sh` passes 100% of all consolidated barrier and integrity gates.
- All modified markdown files and documentation indices are synchronized and free of trailing whitespace.
