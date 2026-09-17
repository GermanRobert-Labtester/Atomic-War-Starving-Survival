# ASHFALL: Batch 3 — Quality Implementation Plan

**Roadmap scope:** Steps 33–48<br>
**Generated:** 2026-08-19<br>
**Implementation posture:** Dependency-first, vertical-slice delivery<br>
**Active target:** Godot 4.7+ with C#/.NET 8<br>
**Simulation authority:** Engine-agnostic Core on .NET Standard 2.1<br>
**Data authority:** Versioned JSON under Assets/StreamingAssets/Data/

## Purpose

Batch 3 should turn sixteen attractive feature descriptions into durable, playable systems. It should not be implemented as sixteen independent UI panels. The high-value work is the set of authoritative contracts underneath them: food and nutrition, equipment condition, shelter environment, study and skill progression, medical/psychological care, expedition operations, map intelligence, contracts, and schedule-driven presentation.

The repository already contains substantial partial authorities that this batch must extend rather than duplicate:

- NeedsSystem owns hunger, thirst, fatigue, warmth, morale, health, and hygiene, but it does not currently own recipes or vitamin accounting.
- CulinaryRationCatalog, RefrigerationFermentationCatalog, CraftingSystem, JournalSystem, KnowledgeBase, SkillProgressionSystem, DutyRosterSystem, ResearchSystem, LocationLayoutSystem, ProceduralScavengeSystem, RadiationSystem, YearOfAshDeepFreezeSystem, WarlordDoctrineSystem, ExpeditionSystem, TacticalCombatSystem, WeaponConditionSystem, and WastelandCartographyCatalog already exist in some form.
- Godot has existing panels/host seams for journal, research, crafting, survivors, duty roster, radio, expeditions, maps, weather, geothermal heating, radon ventilation, and medical operations.
- Narrative/data files already cover culinary rations, root-cellar and smoked-meat reports, ink reports, lost-tech manuals, autopsy records, wildlife, radio distress signals, sump drainage, and weather.

The plan therefore favors:

1. Extend an existing authority where it owns the correct concept.
2. Add a new Core system only when no authority exists.
3. Keep Godot Nodes responsible for input, presentation, and wiring.
4. Make each feature reachable through a real campaign action, not a fixture or preview state.
5. Verify state, resource accounting, determinism, save/load, and data references before polishing presentation.

This document refines the user-supplied Batch 3 roadmap. It is an implementation plan, not a claim that every Batch 1 or Batch 2 dependency has already shipped.

## Non-negotiable architecture rules

### Authority and boundaries

Use this flow for every feature:

JSON/catalog → Core rule/state/events → Godot host session/adapter → Godot UI/presentation

- Do not put gameplay rules, resource consumption, RNG, thresholds, or save state in a Godot panel.
- Do not add new gameplay logic to Assets/_Game/.
- Do not launch, build, or test Unity. Unity is a read-only migration reference.
- Keep Assets/Ashfall.Core free of Godot, Unity, GodotSharp, and JsonUtility references.
- Use IJsonSerializer, IFileIO, ISeededRng, and existing Core ports.
- Never introduce System.Random, Guid.NewGuid(), wall-clock-dependent simulation, or unstable collection ordering.

### Data

- Discover the nearest existing catalog schema before adding a file.
- Extend recipes.json, narrative catalogs, existing map/location catalogs, or established system catalogs when they are authoritative.
- Create a new versioned catalog only when the existing schema cannot represent the feature without corrupting a different domain.
- Use snake_case IDs and register references with CatalogIntegrityValidator.
- Do not hard-code example values such as 50 scrap, 2 clean water, +20% rest, or 100 XP into UI or Core defaults. Put tuning in data and tests.
- Preserve all pre-existing working-tree JSON edits. Merge additively and inspect the diff before touching any modified data file.

### State and persistence

Every new stateful system must provide:

- a serializable state DTO;
- CaptureState and RestoreState;
- defensive handling of null, malformed, past, and future versions;
- deterministic ordering for lists and checksum calculation;
- an aggregate save/load connection in the active Godot host;
- dirty-save/event integration;
- a round-trip test;
- an old-save default/migration test where the aggregate save format changes.

New-format envelopes with a missing checksum must fail as corrupt. They must not silently fall through to the legacy bare-state path.

### Commands and transactions

Player actions should return a typed result containing, as appropriate:

- success/failure;
- stable failure code;
- message/localization key;
- consumed and produced item deltas;
- affected survivor/location/system IDs;
- event ID;
- whether the operation was already resolved.

Validate first, then mutate once. Reservations, inventory consumption, rewards, contract payments, medical procedures, and equipment repairs must be atomic and idempotent. A button pressed twice, a modal reopened, or a save loaded after a command must never duplicate a reward or charge.

### Campaign time and event order

Use one campaign progression owner. If the Batch 1/2 campaign-day coordinator exists, extend it. If it does not, establish the smallest compatible coordinator before adding feature-specific ticks.

The target order is:

1. Weather, forecast windows, thaw/freeze state, orbital warnings, power, heat, air, water, and flooding.
2. Production and maintenance jobs: kitchen, pharmacy, workshop, greenhouse, research, study, excavation, traps, and treatment.
3. Survivor needs, nutrition, disease, radiation, social tension, mental-health crises, and duty effects.
4. Expeditions, SOS windows, convoy operations, contracts, treaties, map intelligence, and radio state.
5. Death/memorial processing, journal/codex notifications, audio context, lighting context, and the daily briefing.

Subsystems must not directly call one another’s daily tick. They exchange typed inputs/events and are registered once in the coordinator.

## Delivery strategy

### Phase 0 — Batch 1/2 integration audit and foundation gate

Before writing Batch 3 feature code:

1. Inspect AGENTS.md, current worktree status, current Core systems, current JSON schemas, host sessions, save stores, and tests.
2. Confirm which Batch 1/2 contracts actually exist:
   - campaign-day coordinator and tick order;
   - typed action results;
   - atomic inventory reservation/transaction;
   - shared event/notification bridge;
   - modal/interruption gate;
   - PowerGrid and ShelterOperations ownership;
   - airlock/security host seam;
   - map fog-of-war authority;
   - combat start request/result contract.
3. Treat planned-but-missing dependencies as explicit work. Do not create a second implementation just because a previous plan named a class.
4. Record pre-existing build/test/data failures separately. Never “fix” unrelated dirty JSON or reset concurrent edits.
5. Run the canonical baseline before feature work and retain its output:
   - dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
   - dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
   - dotnet build Ashfall.csproj
   - godot --headless --path . -- --data-integrity-selftest
   - godot --headless --path . -- --bridge-selftest

Foundation work should be limited to shared seams required by at least two Batch 3 items:

- typed Core action result and event identity;
- atomic inventory/resource transaction and reservation;
- campaign-time/job progression contract;
- shared condition/progress/treatment result conventions;
- aggregate save registration and checksum coverage;
- notification/modal interruption contract.

Do not build a general framework for hypothetical future systems. Prove the foundation with one small vertical slice, then reuse it.

### Phase 1 — Shelter environmental loop

Deliver Steps 38, 45, 37, and 48 in this order:

1. thermal network and warmth effects;
2. sump/flooding and power coupling;
3. decontamination airlock flow;
4. schedule/curfew and presentation lighting.

This phase creates a coherent loop: weather and thaw change shelter conditions; power and fuel determine what can run; water is consumed by life-support and decontamination; survivor warmth, fatigue, radiation exposure, and room availability reflect the result.

### Phase 2 — Production, knowledge, and capability

Deliver Steps 33, 44, 40, 35, and 34:

1. food production/nutrition and preservation;
2. shared equipment condition and maintenance;
3. library study and knowledge unlocks;
4. mentorship as scheduled skill progression;
5. chronicler/archive work as persistent evidence and codex progression.

This phase creates a meaningful off-shift economy. Food, tools, books, mentors, ink, paper, and survivor time are all scarce inputs.

### Phase 3 — Personnel, medical, and psychological stability

Deliver Steps 39, 46, and 42:

1. contract-based mercenary recruitment;
2. mental-health crisis care;
3. pathology/autopsy research.

These features must use the existing roster, medical, disease, trauma, chemical-dependency, morale, and memorial authorities. They must not create a second survivor list, second morale meter, or independent death state.

### Phase 4 — Field operations and map intelligence

Deliver Steps 36, 43, 41, and 47:

1. room-by-room indoor scavenging;
2. time-limited radio SOS dispatch;
3. warlord convoy interception;
4. map beacons and persistent fog-of-war progression.

All four should produce real ExpeditionSystem-compatible operations and typed outcomes. The map must show state that the Core already owns; it must not simulate expeditions or combat locally.

### Phase 5 — Campaign integration and quality pass

After all slices:

- connect daily briefing/journal/radio notifications;
- verify memorial, epilogue, faction-standing, economy, and map consequences;
- run economy/resource mass-balance checks;
- run same-seed and save-split comparisons;
- audit every new UI for missing-data, disabled-action, failure, accessibility, and reload states;
- remove fixture-only paths and stale preview controls;
- run the full canonical verification checklist again.

## Feature implementation slices

## [33] Kitchen, culinary nutrition, and food preservation

### Correct authority

Reuse CulinaryRationCatalog, RefrigerationFermentationCatalog, RecipeCatalog/CraftingSystem, Inventory, NeedsSystem, and the existing food/item definitions. NeedsSystem remains the owner of hunger and thirst; it must not become a recipe or vitamin database.

### Core work

Add a focused KitchenNutritionSystem or FoodProductionSystem that owns:

- active preparation jobs and assigned cook;
- ingredient reservations and batch progress;
- meal portions and their data-defined nutrition profile;
- pantry/cellar storage conditions;
- spoilage timers and preservation state;
- meal consumption/dining effects;
- food safety and contamination outcomes.

Nutrition should be an additive effect layer applied when a survivor eats a meal, not an invisible benefit granted at cook completion. Define how calories, micronutrients, deficiency thresholds, and morale effects stack. A meal should not grant repeated daily morale merely because it remains in inventory.

Reuse existing recipes where possible. Add only the fields needed for calories, nutrition tags, preservation requirements, spoilage, and serving effects. Use the root-cellar, fermentation, and smoked-meat narrative records as authored context, not as an orphaned second simulation.

### Godot work

Implement KitchenPanel as a state-driven view of ingredients, jobs, storage, temperature, spoilage, nutrition, and failure reasons. Route cook, preserve, discard, and serve commands through a host session and typed Core results.

### Acceptance

- A valid recipe reserves and consumes exactly the required ingredients.
- A completed batch creates exactly the catalog-defined portions.
- Eating a portion updates hunger/nutrition/morale through Core and cannot be double-applied.
- Cellar temperature and preservation method change spoilage deterministically.
- A save during preparation or spoilage restores the same result.
- Missing ingredients, invalid cook assignment, and unsafe food produce visible stable failures.

### Main attack surface

Do not add a second food inventory, silently create clean water, apply morale on both cooking and eating, or place recipe math in KitchenPanel.

## [34] Chronicler’s desk, codex transcription, and ink longevity

### Correct authority

Extend JournalSystem and KnowledgeBase. Do not create a parallel codex collection or a UI-only list of unlocked lore. Use iron_gall_ink_acidity_reports.json as supporting authored data; add a typed ink/material catalog only if the existing narrative schema cannot express actionable ink condition.

### Core work

Add an ArchiveDeskSystem or CodexTranscriptionSystem that owns:

- a transcription queue and archivist assignment;
- source evidence IDs and provenance;
- paper/ink/material reservations;
- transcription progress and interruption;
- ink formulation, legibility, and archival longevity;
- permanent JournalSystem/KnowledgeBase unlocks;
- optional knowledge/codex completion bonuses defined in data.

A radio signal, treaty, encounter, or historical event should be transcribable only after the player has actually acquired or witnessed the relevant evidence. Archive entries must retain source IDs and not be freely generated from a UI button.

### Godot work

Extend JournalPanel/JournalDetailPanel patterns or add CodexArchivePanel without replacing existing journal navigation. Show source, transcription status, ink condition, provenance, and failure states. Keep layout compatible with the current typography/theme.

### Acceptance

- A discovered transmission can be queued only once unless the catalog explicitly supports revision.
- Inputs are reserved and consumed exactly once.
- Completion creates a real JournalSystem/KnowledgeBase entry and unread notification.
- Ink quality changes durability/legibility according to catalog data.
- Save/load preserves in-progress jobs and unlocked evidence.
- The same evidence cannot generate duplicate completion bonuses.

### Main attack surface

Reject lore that exists only in JSON but is unreachable, duplicated codex state, or a “completion bonus” that bypasses JournalSystem.

## [35] Survivor apprenticeship and skill specialization

### Correct authority

Extend SkillProgressionSystem and DutyRosterSystem. Reconcile with the existing GenerationalSuccessionEngine mentorship concept instead of creating a second mentorship relationship model. Generational inheritance and daily apprenticeship are related but distinct:

- lineage records inherit traits or history;
- an active apprenticeship grants scheduled training progress and role-specific capability.

### Core work

Add active training assignments with:

- mentor/apprentice IDs;
- skill or knowledge target;
- eligibility and compatibility checks;
- daily training hours and opportunity cost;
- deterministic XP gain;
- milestone/perk IDs loaded from data;
- interruption, mentor absence, fatigue, and completion state;
- relationship or morale consequences where authored.

Use a single skill progression write path. Duty roster assignment must determine whether the mentor and apprentice actually have time to train. Do not award XP from both the roster tick and a mentorship tick.

### Godot work

Extend DutyRosterPanel with mentor/apprentice slots, target skill, schedule conflict, progress, milestone preview, and cancel/reassign actions. The panel must show the source of each XP change.

### Acceptance

- An eligible pair can be assigned to a valid target skill.
- Ten days of valid training produce the catalog-defined XP, not a hard-coded UI increment.
- A milestone unlocks one canonical trait/perk exactly once.
- Mentor absence, fatigue, reassignment, and save/load behave predictably.
- Skill progression remains deterministic and existing skill tests remain green.

### Main attack surface

Do not duplicate the generational mentorship API, grant training while the apprentice is assigned elsewhere, or hard-code trait_trained_paramedic without validating that it is a canonical data ID.

## [36] Tactical indoor scavenging/search mode

### Correct authority

Compose LocationLayoutSystem, the existing location/site encounter data, ProceduralScavengeSystem, ExpeditionSystem, inventory, and Batch 1 encounter/combat contracts. If ProceduralScavengeSystem is still maritime-specific, extract a shared search-resolution contract rather than copying its loot logic.

### Core work

Add IndoorScavengeSystem or LocationSearchInstance with saved state for:

- expedition and location ID;
- room/sector graph and current position;
- searched/locked/collapsed room state;
- search progress and action time;
- flashlight/battery or other equipment reserves;
- noise and structural-risk values;
- tool requirements and durability;
- hazard cards and deterministic resolutions;
- loot claims and encounter/combat requests;
- exit/abort/failure state.

Every room must have an idempotent search result. A safe or locked-room choice should use the existing typed encounter-choice pattern and feed combat or narrative resolution through the shared bridge.

### Godot work

Implement ScavengeSearchPanel with a floor/room graph, action costs, battery/noise/risk meters, tool requirements, room results, and a clear return-to-expedition action. Pause the campaign at player-choice boundaries using the shared modal gate.

### Acceptance

- A real expedition can enter an authored indoor location.
- Searching a room advances time and consumes the correct resource once.
- A locked safe can resolve through a tool/skill choice or authored failure path.
- Loot, injury, noise, and collapse outcomes are deterministic and saved.
- Returning to the map preserves the expedition and prevents duplicate room rewards.

### Main attack surface

Do not build a mini-game with local fake inventory, frame-rate-dependent timers, or generic loot disconnected from location identity.

## [37] Decontamination airlock showers and fallout scrub-down

### Correct authority

Use RadiationSystem and ExposureContext for exposure semantics, the expedition return state, water-treatment inventory, airlock/security state, and SurvivorsHostSession for wiring. Lifetime dose must remain lifetime dose; decontamination removes surface contamination/dust and prevents secondary shelter spread, not accumulated historical dose.

### Core work

Add DecontaminationSystem or ExposureDeconSystem with:

- returning survivor/gear contamination snapshots;
- queue and airlock occupancy;
- scrub cycle progress;
- clean-water/filter/soap/energy costs from data;
- surface-contamination reduction;
- missed/bypassed wash consequences;
- shelter contamination event and cleanup path;
- gear condition impact;
- safe/unsafe release state.

Use one atomic inventory transaction for water and consumables. Connect bypass to shelter air/contamination hazards; do not implement a warning that has no downstream effect.

### Godot work

Extend the Batch 2 airlock/security panel seam with a decon queue, dosimeter versus surface-dust distinction, water cost, cycle progress, bypass confirmation, and contamination warning. Do not let a panel directly edit radiation state.

### Acceptance

- A returning scout enters decon with a real contamination snapshot.
- The authored treatment consumes the correct resources once and reduces surface contamination.
- Bypass leaves a traceable shelter contamination consequence.
- Decon state survives a save during the queue.
- Lifetime dose and dose-ledger history remain unchanged by surface scrubbing.

### Main attack surface

Do not silently heal radiation, consume water twice, or create an airlock alarm that never reaches air/disease/shelter state.

## [38] Bunker heating, steam radiators, and winter freeze

### Correct authority

Use YearOfAshDeepFreezeSystem for seasonal deep-freeze conditions, StartingLevelSystem for existing shelter air/heat-related state, NeedsSystem for survivor warmth, and Batch 2 PowerGrid for power/fuel availability. Add a thermal network only for room distribution, boiler, radiators, and pipes.

### Core work

Add ShelterThermalSystem or HeatingNetworkSystem with:

- room thermal nodes and adjacency;
- boiler/fuel/heat output;
- radiator valves and room priority;
- power requirements where applicable;
- ambient weather/deep-freeze input;
- room temperature and freeze state;
- pipe condition, burst risk, and repair state;
- survivor warmth modifiers through an explicit port;
- equipment/room consequences.

The system order must be weather/deep-freeze → boiler and power → room temperatures → pipe incidents → survivor warmth. Avoid writing temperature directly into NeedsSystem from multiple callers.

### Godot work

Extend ShelterOperationsPanel or the existing geothermal heating widget with room heat overlays, boiler fuel, valves, pipe warnings, and power dependencies. Present hazards with text/icons as well as color.

### Acceptance

- Fuel and power change heat output and room temperature.
- A room below the authored threshold affects warmth and can produce a pipe incident.
- Restoring heat prevents further freeze progression without erasing damage already caused.
- Heat, boiler, valves, and pipe state round-trip through saves.
- Same seed and same inputs produce the same thermal incident sequence.

### Main attack surface

Do not create a second warmth meter, make heat free, or let visual room tint be the only representation of a thermal hazard.

## [39] Nomad mercenary recruitment and drifter contracts

### Correct authority

Create ContractorRosterSystem or MercenaryContractSystem because no existing system should be expanded into an ad hoc contractor list. Integrate with the survivor roster, inventory/economy, DutyRosterSystem, ExpeditionSystem, TacticalCombatSystem, airlock visits, and memorial/death flow.

### Core work

Track:

- candidate and contract IDs;
- role/skills/traits and equipment;
- recruitment requirements and initial fee;
- term, daily hazard pay, and payment currency;
- loyalty/trust and contract status;
- assignment eligibility;
- injury/death/absence;
- renewal, dismissal, breach, and expiry;
- consequences for missed payment.

A contractor should be roster-compatible but contract-distinct. Payment must be an atomic daily transaction. Unpaid contracts need an authored grace/loyalty/exit path; they must not create free permanent survivors.

### Godot work

Implement RecruitmentPanel with candidate dossier, contract terms, total cost, role availability, payment schedule, loyalty, and confirmation. Reuse existing survivor dossier components where possible.

### Acceptance

- A valid candidate can be hired only after requirements and initial payment succeed.
- The contractor appears in the correct roster path with contract metadata.
- Daily payment is consumed once and affects contract state if unavailable.
- The contractor can join a real expedition/defense assignment.
- Expiry, dismissal, death, and save/load remove or transition the contract correctly.

### Main attack surface

Do not copy a survivor into a second list, use an unbounded random candidate generator, or make combat bonuses bypass equipment/skill/needs rules.

## [40] Bunker library and technical manual studies

### Correct authority

Compose ResearchSystem, KnowledgeBase, JournalSystem where knowledge evidence is involved, SkillProgressionSystem, DutyRosterSystem, and the existing lost-tech manual catalog. Research remains the authority for recipe/technology unlocks; library study is a way to progress or reveal that authority.

### Core work

Add LibraryStudySystem with:

- manual/knowledge source ID;
- reader assignment and eligibility;
- study hours and off-shift schedule;
- fatigue/morale/opportunity cost;
- skill XP and research progress;
- prerequisite and completion state;
- read/knowledge evidence;
- interruption and re-assignment;
- saved progress.

Prevent duplicate benefits when the same manual is studied through research and library paths. Decide in data whether a manual grants skill XP, unlocks knowledge, advances research, or some combination.

### Godot work

Implement LibraryPanel with shelves/catalog, reader slots, progress, schedule conflict, expected benefit, and completed knowledge. Use existing research/journal visuals rather than making a disconnected screen.

### Acceptance

- An idle/eligible survivor can begin a valid study job.
- Five days of valid study produce the authored skill/research result.
- Fatigue and schedule costs are visible and applied once.
- A completed manual cannot repeatedly grant a permanent unlock.
- Study progress survives save/load and missing manuals fail clearly.

### Main attack surface

Do not let the panel edit skill values, create a second knowledge database, or grant research completion merely by opening a book card.

## [41] Supply-convoy ambush and preemptive raids

### Correct authority

Extend WarlordDoctrineSystem with authored convoy operation state, and compose ExpeditionSystem, MapAtlas/route state, TacticalCombatSystem, inventory, and faction standing. Do not create a second territory or warlord reputation model.

### Core work

Add ConvoyOperationSystem or an explicit convoy module owned by WarlordDoctrineSystem:

- authored convoy identity, route, cargo, escort, and departure window;
- map marker and intercept ETA;
- scout-team eligibility and loadout;
- stealth/detection calculation;
- abort/recall state;
- combat request/result;
- captured cargo and casualty resolution;
- faction standing and retaliation;
- persistence and idempotent completion.

Use a CombatStartRequest/CombatResult contract from the shared expedition/combat integration. Values such as 50 scrap are tuning entries, not universal code.

### Godot work

Extend MapAtlasPanel with convoy markers, intercept path, ETA, stealth breakdown, dispatch/recall, and result history. The map must show Core state and must not resolve the ambush locally.

### Acceptance

- A live convoy can be discovered, targeted, and dispatched against.
- Detection, combat, retreat, loot, casualties, and retaliation are deterministic.
- Captured goods are delivered once through inventory.
- Warlord/faction state updates through the existing doctrine/standing path.
- An active operation survives save/load without duplicate resolution.

### Main attack surface

Do not turn the map into an instant loot button, duplicate warlord territory state, or make ambush risk-free.

## [42] Pathological autopsy and anatomical research

### Correct authority

Create AutopsySystem or PathologyResearchSystem as a clinical procedure layer over DiseaseSystem, MedicalSystem, DoseLedger, CombatTraumaSystem, memorial/death state, inventory, ventilation, and ResearchSystem. Use authored rad-pathology/autopsy and medical casebook data. Do not add a generic “mutant parts” economy without catalog support.

### Core work

Track:

- specimen/deceased ID and legal/medical eligibility;
- procedure, staff, station, tool, and containment requirements;
- sample/waste consumption;
- time and progress;
- airborne/pathogen risk;
- findings and confidence;
- research/antibody/vaccine unlock;
- specimen terminal state and chain of custody.

An autopsy must not resurrect, double-consume, or detach a survivor’s memorial/death record. Airborne risk should feed ventilation and disease systems; discoveries should use the normal research/recipe unlock path.

### Godot work

Implement AutopsyBenchPanel with specimen selection, containment state, tools, staff, risk, progress, findings, and invalid-procedure explanations. Keep medical presentation restrained and non-gore-first.

### Acceptance

- A valid specimen can be processed once through an authored procedure.
- The procedure consumes the correct materials and creates a typed finding.
- A vaccine or antidote unlock is catalog-defined and reaches Research/Crafting/Medical authority.
- Containment failure produces a real ventilation/disease consequence.
- The specimen and research result survive save/load.

### Main attack surface

Do not award a vaccine from a UI click, use deceased IDs as reusable loot, or make pathology bypass medical and contamination risk.

## [43] Radio SOS calls and rescue dispatches

### Correct authority

Use the existing radio host/interception path and radio_distress_signals.json as the starting content authority. Add DistressSignalSystem and RescueDispatchSystem only for active signal lifecycle and expedition linkage. Reuse NarrativeEncounterSystem, ExpeditionSystem, roster/recruitment flow, combat, inventory, and journal.

### Core work

Track:

- signal ID, frequency/source, location, and discovery time;
- active window and expiry;
- caller/cargo manifest;
- confidence/decoding state;
- response/dispatch ID;
- expedition link;
- success, late, failure, and abandonment outcomes;
- journal evidence and faction consequences.

The active signal and response must be saved. Resolution must be idempotent. A successful rescue should return a typed result that flows through the normal visitor/recruitment/roster path; it must not inject a survivor directly into a UI list.

### Godot work

Extend RadioPanel with a red SOS state, transcript, coordinate, expiry, risk, and rapid-dispatch action. Show whether dispatch is unavailable because of weather, roster, fuel, or an active expedition.

### Acceptance

- An authored signal can surface through radio rather than appearing only in a test panel.
- Dispatch before expiry starts a real expedition with a real destination and loadout.
- Late, failed, and successful outcomes differ and are recorded in the journal.
- A rescued specialist is added through the canonical roster flow exactly once.
- Save/reload preserves an active signal and its linked expedition.

### Main attack surface

Do not guarantee success, create a rescue-specific survivor database, or let expiry disappear without a consequence.

## [44] Workshop tool wear, maintenance, and sharpening

### Correct authority

Do not overload WeaponConditionSystem with unrelated tool rules. Extract or add a shared EquipmentConditionSystem with a compatibility adapter for weapons, then integrate Inventory, DeviceState, ProceduralItemInstance, CraftingSystem, medical procedures, and MetallurgyToolingCatalog.

### Core work

Track per-instance:

- condition and maximum condition;
- use/wear profile;
- material and tool family;
- maintenance recipe and station;
- spare-part requirement;
- sharpening/repair progress;
- failure/slip/jam risk;
- last-maintained state.

All uses must route through one condition mutation path. The surgical slip risk in MedicalSystem must read the same item condition that the maintenance bench changes. Repair and sharpening must reserve inputs and complete once.

### Godot work

Add maintenance actions to CraftingPanel or a WorkshopPanel. Show condition, predicted wear, required parts, station availability, repair result, and risk reduction. Do not expose hidden condition writes from a button handler.

### Acceptance

- A tool loses condition through a real authored use.
- A valid sharpening/repair operation restores the catalog-defined amount, including 100% where specified.
- Surgery/production/combat reads the repaired state.
- Invalid tools and missing parts fail without consuming inputs.
- Condition state and pending maintenance survive save/load; deterministic wear tests pass.

### Main attack surface

Do not maintain only a display copy, reset every item to 100%, or create separate weapon/tool condition formulas that disagree.

## [45] Subterranean sump pumps and flooding crises

### Correct authority

Create SumpFloodingSystem or ShelterHydrologySystem because material shielding and deep-freeze do not own water inflow. Integrate WeatherSystem/thaw state, PowerGrid, room availability, generator/storage/smelter operation, and ShelterOperations state.

### Core work

Track:

- sublevel nodes and drainage connections;
- baseline groundwater/inflow;
- weather/thaw event input;
- water level and contamination;
- sump pump installation, condition, and power;
- float-valve/sandbag mitigation;
- drainage rate;
- equipment exposure and disablement;
- repair/drain completion and incident history.

Use the order weather/thaw → inflow → power/breakers → pumps → water level → room/equipment availability. A flooded room must affect the authority that uses it; it cannot only show a red banner.

### Godot work

Add water gauges, pump controls, drainage estimates, breaker dependency, and equipment alerts to ShelterOperationsPanel. Show pump failure and flooded-room recovery states.

### Acceptance

- A thaw or storm increases inflow according to data.
- With power, an operational pump lowers water; without power, water rises.
- Flooded sub-level equipment is actually unavailable until restored.
- Equipment is not damaged repeatedly every UI refresh.
- Flood state, pump condition, and incident history round-trip through saves.

### Main attack surface

Do not make flooding a cosmetic timer, duplicate power consumption, or apply permanent damage on every frame/day without an incident guard.

## [46] Survivor mental-health crises and isolation wards

### Correct authority

Add MentalHealthCrisisSystem or PsychiatricCareSystem as an event/state layer over SomaticFlashbackSystem, CaregivingSystem, GuiltInsomniaSystem, CombatTraumaSystem, NeedsSystem, MedicalSystem, ChemicalDependencySystem, and duty/roster state. It must not introduce a second morale meter.

### Core work

Track:

- stress/trauma inputs and threshold crossing;
- crisis profile and acuity;
- current survivor risk/status;
- ward/room assignment;
- caregiver/counseling plan;
- sedation or medical procedure;
- duration, recovery, relapse, and side effects;
- safety restrictions and work eligibility;
- resolution event and journal/medical note.

Use authored crisis profiles and deterministic threshold logic. A crisis should represent a specific care need, not a generic “hysteria” label. Sedatives must use canonical inventory and chemical-dependency paths. Isolation changes assignments and safety state, not merely a portrait icon.

### Godot work

Implement PsychWardPanel or extend MedicalPanel with active cases, room capacity, interventions, medication risk, counseling assignment, and recovery milestones. Include non-color status indicators and clear confirmation text for irreversible actions.

### Acceptance

- Existing trauma/stress inputs can trigger one authored crisis instance at the threshold crossing.
- A valid counseling or medical intervention changes crisis state and records costs/side effects once.
- Ward capacity and survivor work eligibility change while isolated.
- Recovery persists through save/load and does not retrigger every tick.
- The system remains compatible with morale, medical, and memorial outcomes.

### Main attack surface

Do not treat mental illness as a random punishment, use stigma-only choices, or duplicate stress/morale calculations in the UI.

## [47] Wasteland cartography, map beacons, and fog-of-war

### Correct authority

Extend the Batch 1 map/fog-of-war authority and LocationLayoutSystem using WastelandCartographyCatalog and existing location coordinates. Add MapBeaconSystem only for beacon ownership, installation, coverage, signal, degradation, and map-intelligence effects.

### Core work

Track:

- beacon ID and location;
- eligibility and placement requirements;
- installation progress;
- power/fuel/condition;
- signal radius and terrain obstruction;
- discovered locations/routes;
- travel modifier scope;
- destruction/disablement and repair;
- evidence of discovery.

Coverage must be computed from authored coordinates and route topology, not from UI distance guesses. A beacon reveals only valid nodes in its radius and applies a travel modifier only to the data-defined zone/routes. Discovery must be idempotent and saved.

### Godot work

Extend MapAtlasPanel with eligible placement, preview rings, signal strength, revealed nodes, disabled state, and route modifier explanation. Preserve current map interactions and accessibility.

### Acceptance

- A valid beacon consumes the required item/time/power and installs once.
- Only locations within the authored coverage rules are revealed.
- The travel modifier affects a real expedition route once, without stacking on refresh.
- Beacon damage/power loss changes coverage according to data.
- Beacon state and discovered nodes survive save/load.

### Main attack surface

Do not reveal the whole map, duplicate fog-of-war state, or make the ring a visual promise that the route planner ignores.

## [48] Dynamic bunker lighting, oil lanterns, and sleep curfew

### Correct authority

Create a Core ShelterScheduleSystem or CurfewSystem for time phases, sleep/quiet hours, bed assignments, lighting demand, and rest modifiers. Create a Godot LightingEnvironmentController or ShelterLightingOverlay for CanvasModulate/CanvasItem presentation. Core must not contain colors, shaders, or Godot types.

### Core work

Track:

- schedule/curfew definition from data;
- current campaign phase;
- survivor sleep assignment and compliance;
- lighting circuit demand;
- emergency override;
- fatigue recovery modifier;
- power/brownout interaction;
- schedule transition events.

Curfew must affect fatigue only during a valid sleep window and through NeedsSystem’s established recovery path. The 20% example is tuning, not a hard-coded universal bonus. Emergency lighting must override normal curfew without falsely granting normal rest.

### Godot work

Map Core lighting context to the existing diorama/HUD:

- day: cool industrial light;
- night: restrained amber/lantern state;
- emergency: readable pulsed red state.

Use CanvasModulate or existing CanvasItem layers, preserve UI contrast, and make state readable without color alone. Audio context should consume the same schedule state rather than infer time independently.

### Acceptance

- Activating curfew changes Core schedule state and power load.
- Valid sleeping survivors receive the data-defined fatigue recovery modifier.
- Brownout/emergency state changes presentation and does not grant the normal rest bonus.
- Reopening the UI does not reapply modifiers.
- Schedule state, assignments, and emergency override survive save/load.

### Main attack surface

Do not implement day/night as a visual-only color filter, put schedule rules in Main.cs, or let each panel infer its own current time.

## Cross-system integration contracts

### Inventory and resource mass balance

Every feature must name its resource owner and transaction boundary:

| Resource or state | Owner | Batch 3 consumers |
|---|---|---|
| Food ingredients and meals | Inventory + KitchenNutritionSystem | 33, 46 where medication/food affects care |
| Clean/raw water | Water treatment/inventory authority | 33, 37, 38 if boiler uses water, 45 |
| Fuel and power | PowerGrid/fuel authority | 38, 45, 48, 36/41/43 expedition launch |
| Equipment condition | Shared EquipmentConditionSystem | 36, 42, 44, combat/medical |
| Survivor skill XP | SkillProgressionSystem | 35, 40, 42, 36, 41 |
| Knowledge/unlocks | ResearchSystem + JournalSystem/KnowledgeBase | 34, 40, 42 |
| Radiation exposure | RadiationSystem/DoseLedger | 36, 37, 42, 46 |
| Faction standing | Existing standing/doctrine/treaty authority | 39, 41, 43, 47 |

Add invariant tests for conservation: no operation may create items, fuel, water, XP, or reputation without a catalog-defined source.

### Survivor roster identity

There must remain one living-survivor identity path. Apprentices, contractors, rescued specialists, isolated patients, expedition members, and memorialized dead must use the same stable survivor IDs and save references. Specialized metadata belongs on the canonical record or a typed extension, not in parallel UI dictionaries.

### Map and expedition identity

Indoor searches, SOS rescues, convoy interceptions, beacons, and ordinary travel must share location IDs, route costs, expedition IDs, and completion semantics. Every active operation needs a stable ID, a saved status, and one terminal resolution.

### Medical and mental-health identity

Autopsy, decontamination, trauma care, disease, medication, and psychological crisis must publish typed state/events to the existing medical/needs paths. They may add domain-specific state, but must not overwrite lifetime dose, health, morale, or death records without using their owners.

## Data and file-boundary plan

The exact paths must be confirmed during Phase 0. Candidate implementation boundaries are:

### Core

- Extend Assets/Ashfall.Core/Narrative/CulinaryRationCatalog.cs and Recipe/Crafting paths for 33.
- Extend Assets/Ashfall.Core/Journal/JournalSystem.cs and KnowledgeBase.cs through a new archive system for 34.
- Extend Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs and DutyRoster/DutyRosterSystem.cs for 35 and 40.
- Add an indoor search module beside LocationLayoutSystem/ProceduralScavengeSystem for 36.
- Add decontamination beside RadiationSystem and shelter/airlock domain code for 37.
- Add shelter thermal and hydrology modules for 38 and 45.
- Add ContractorRosterSystem for 39.
- Extend WarlordDoctrineSystem for 41.
- Add PathologyResearchSystem for 42.
- Add DistressSignalSystem/RescueDispatchSystem for 43.
- Add a shared EquipmentConditionSystem and adapt WeaponConditionSystem for 44.
- Add MentalHealthCrisisSystem for 46.
- Extend the cartography/map authority with MapBeaconSystem for 47.
- Add ShelterScheduleSystem/CurfewSystem for 48.

### Godot host and UI

Use thin host sessions and state-driven panels:

- KitchenPanel;
- CodexArchivePanel;
- DutyRosterPanel extension;
- ScavengeSearchPanel;
- AirlockSecurityPanel extension;
- ShelterOperationsPanel and thermal/hydrology overlays;
- RecruitmentPanel;
- LibraryPanel;
- MapAtlasPanel extensions for convoy and beacons;
- AutopsyBenchPanel;
- RadioPanel SOS extension;
- CraftingPanel/Workshop maintenance extension;
- PsychWardPanel or MedicalPanel extension;
- LightingEnvironmentController/ShelterLightingOverlay.

Do not force all wiring into the already-large Main.cs. Follow the repository’s domain partial/session pattern and add one setup/save/flush unit per new domain when that pattern is active.

### Data

Prefer extending existing authority. Candidate new catalogs, only if required after inspection:

- kitchen nutrition/preservation fields;
- archive ink/material and transcription rules;
- mentorship milestones;
- indoor search layouts/hazards;
- decontamination profiles;
- shelter thermal/hydrology/schedule configuration;
- contractor offers and terms;
- convoy operation tuning;
- autopsy procedures/findings;
- equipment maintenance profiles;
- mental-health crisis profiles;
- map-beacon rules.

Existing files such as culinary, iron-gall ink, lost-tech manual, autopsy, wildlife, radio distress, sump, weather, locations, and warlord catalogs should be extended rather than shadowed. All new JSON needs schema_version, valid IDs, loader tests, and CatalogIntegrityValidator coverage.

## Test and verification matrix

### Per-system Core tests

Each item needs:

- happy path;
- invalid input/resource path;
- idempotency/double-command path;
- deterministic same-seed path where RNG exists;
- CaptureState/RestoreState round trip;
- interruption or cancellation path;
- canonical ID/reference validation.

### Cross-system tests

Add focused integration coverage for:

1. Kitchen → inventory → nutrition/needs/morale → spoilage.
2. Heating → power/fuel → room temperature → warmth → freeze/pipe incidents.
3. Flooding → power breaker → room availability → smelter/storage operation.
4. Decontamination → water inventory → surface contamination → shelter air/disease.
5. Mentorship/library → duty schedule → fatigue → skill/research unlock.
6. Equipment maintenance → tool condition → medical/scavenge/combat outcome.
7. Autopsy → specimen/death record → containment → disease/ventilation → research.
8. Mental crisis → ward/caregiver/medication → work eligibility → morale/recovery.
9. Mercenary contract → payment → roster → expedition/combat → expiry/memorial.
10. SOS/convoy/indoor search → map/expedition/combat → loot/rescue/journal.
11. Beacon → fog-of-war → route planner → expedition travel modifier.
12. Curfew → power load → lighting context → valid rest recovery.

### Required command set

Run the project’s canonical checklist at baseline, after each high-risk phase, and at final handoff:

    dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
    dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
    dotnet build Ashfall.csproj
    godot --headless --path . -- --data-integrity-selftest
    godot --headless --path . -- --bridge-selftest

Add targeted headless probes only when the existing HostCli/self-test pattern supports them. No Unity command is permitted.

### Determinism and split-run checks

For each operation with time or randomness:

- run uninterrupted and save/reload-split campaigns with the same seed;
- compare final state checksums and event IDs;
- verify stable ordering of candidates, rooms, routes, loot, and notifications;
- verify that a UI refresh or repeated signal does not advance simulation.

### Performance and accessibility

The map, shelter overlays, search graphs, and audio/lighting contexts must not allocate unbounded objects per tick. Headless mode must tolerate missing visual/audio resources. Critical status must not rely on color alone, and all interactive actions need disabled states/tooltips explaining why they cannot run.

## Definition of done

Batch 3 is ready for acceptance only when:

- all sixteen roadmap items have a reachable, campaign-connected action path;
- no item exists only as a static panel, fixture, hard-coded demo, or disconnected JSON catalog;
- Core owns simulation/state/events and Godot owns presentation/input;
- no new Unity dependency, JsonUtility call, uncontrolled RNG, or duplicate authority was introduced;
- resources, survivor IDs, operation IDs, and terminal outcomes are conserved and idempotent;
- all stateful systems save/load with checksums and old-save defaults/migrations;
- data-integrity and canonical-ID checks pass;
- all relevant Core tests, Godot build, and headless probes pass;
- failures introduced by the batch are repaired before claiming completion;
- unrelated working-tree changes remain untouched and are listed separately;
- remaining architectural or canon uncertainty is documented instead of silently decided.

## Targeted adversarial review packet

The plan is intended to be reviewed by GLM 5.2 and Qwen 3.7 Plus before implementation. Give each reviewer the plan plus the current repository snapshot and ask for an evidence-based attack, not a rewrite.

Require each reviewer to answer:

1. Which proposed system duplicates an existing authority, and what exact file/API proves it?
2. Which dependency is likely to be planned but not actually implemented?
3. Where can a resource, reward, XP grant, radiation change, contract payment, or map reveal be applied twice?
4. Which tick-order dependency can create a one-day exploit or false death/illness result?
5. Which save section, version migration, checksum, or old-save default is missing?
6. Which catalog or ID assumption is unsupported by the current data authority?
7. Which UI panel is at risk of containing domain logic?
8. Which feature is not yet reachable from the real Godot campaign loop?
9. Which feature has weak failure states, weak accessibility, or no recovery path?
10. Which balance loop creates free resources, infinite morale/XP, risk-free scouting, or unbounded contract value?
11. Which content proposal violates ASHFALL’s restrained, fictional, non-glorified tone?
12. What is the smallest corrected milestone order?

The reviewers should classify findings as:

- Blocker: violates Core/Godot/data/save/determinism authority or makes the feature unsafe to implement.
- High: causes duplicate state, broken campaign reachability, resource exploit, or save incompatibility.
- Medium: missing test, failure state, accessibility detail, or balance tuning.
- Low: naming, polish, or later optimization.

Resolve Blocker and High findings before implementation. Do not allow reviewers to expand scope into a rewrite; the goal is to expose unsafe assumptions and tighten the vertical slices.

## Recommended next implementation prompt

Begin with Phase 0 only:

“Audit the current ASHFALL repository and worktree for Batch 3 readiness. Inspect AGENTS.md, the active Core systems, current JSON authority, Godot host sessions/panels, save aggregation, and tests for Steps 33–48. Verify which Batch 1/2 contracts actually exist; do not assume planned classes are present. Preserve all unrelated working-tree changes. Produce a dependency map, identify duplicate-authority and tick-order risks, run the five canonical baseline commands, and recommend the smallest verified foundation slice. Do not implement Batch 3 features until the audit identifies the authoritative owner for each dependency.”
