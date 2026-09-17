# ASHFALL: 2D Atomic-War Survival
## Quality Next Steps Roadmap — Batch 6 Implementation Plan

Generated: 2026-08-19

Scope: Steps 81–96

Host engine: Godot 4.7+ with .NET 8 and C#

Core target: .NET Standard 2.1

Status: Architecture-aligned implementation plan, prepared for adversarial review

This document is the implementation plan for Batch 6. It is intentionally more
strict than the feature brief: every numbered feature must attach to an existing
simulation authority or introduce one explicitly in Ashfall.Core. A panel,
animation, catalog, or headless demo is not considered an implementation by
itself.

The plan is written so a coding agent can execute one vertical slice at a time
and so GLM 5.2, Qwen 3.7 Plus, or another reviewer can attack the assumptions
without having to infer the repository architecture from the feature prose.

## Executive outcome

Batch 6 contains a strong infrastructure theme, but the current repository is
not yet ready for sixteen independent UI tickets. The audit found:

- CrossingSession and VouchAccessSystem already govern social access to
  crossing nodes. They do not model bridge construction, structural stages, or
  permanent route speed.
- FactionRadioEngine and RadioHostSession are the active radio seam. A
  RadioInterceptionSystem type named by the brief was not found in the active
  Core or Godot source.
- YearOfAshDeepFreezeSystem already models geothermal flow as a thermal input
  and has a Godot GeothermalHeatingWidget. It is not an electrical power-grid
  authority.
- MaterialShieldingSystem already stores room ceiling materials and radiation
  attenuation, but its save arrays are dictionary-order dependent and it does
  not model construction staging or material consumption.
- DiseaseSystem already owns disease patient state and quarantine behavior.
  DiseaseHeadlessDemo verifies that quarantine prevents new transmission in its
  tested path. A biohazard airlock must extend that authority rather than
  create a second infection model.
- Inventory, CraftingSystem, JournalSystem, TravelingCaravanSystem, the
  epilogue matrix, and GenerationalSuccessionEngine are real Core seams.
  Several Batch 6 concepts are only narrative catalogs or data records today.
- No active Core authority was found for a power distribution grid,
  prosthetic limbs, animal companions, paper-mill production, pneumatic
  dispatch, drone reconnaissance, chronometer schedules, or a Hall of Fame
  profile archive.

Therefore the quality target is not "sixteen polished screens." It is a
dependency-safe set of authoritative state transitions with thin Godot
adapters, deterministic outcomes, checksummed saves, canonical IDs, and
playable feedback.

## Quality corrections to the feature brief

The original Done-when statements are useful player promises, but several need
guardrails before implementation:

1. Bridge repair must unlock the specific repaired route, not every eastern
   sector. Its travel modifier must be attached to canonical route data.
2. Morse input is presentation; the transmitted message, signal quality, and
   contact consequence belong in Core. Correct tapping cannot invent a
   non-canonical bunker or faction.
3. Geothermal flow is currently thermal. Electrical generation requires a
   shared power contract before it can affect fuel, breakers, batteries, or
   room functions.
4. A prosthetic restores a capability under fitting, pain, maintenance, and
   rehabilitation constraints. It must not erase an injury or promise an
   unconditional 90 percent recovery.
5. Lightning capture is a bounded hazard and storage interaction. A storm
   cannot create free energy without a deterministic strike, functional
   grounding, capacity, and overflow handling.
6. An ice house changes storage conditions; it does not guarantee six months
   for every food or medicine item. Spoilage rules and temperature tolerances
   must be explicit.
7. A hound supplies probabilistic tracking, warning, and morale benefits. It is
   not an omniscient ambush detector.
8. An artesian well supplies a tested water source with draw, casing, pump, and
   contamination risk. "100 percent pure forever" is not a safe default.
9. Paper production creates stock and supports authored documents. Printing a
   page must not silently unlock a research recipe.
10. An escort contract must reserve real guards, advance through a real route,
    resolve risk, and pay a catalogued reward. It must not award arbitrary
    ammunition or refer to an unverified location such as New Meridian.
11. A biohazard airlock improves exposure control. UV-C, fumigation, and
    negative pressure do not cure a disease unless DiseaseSystem explicitly
    models that effect.
12. Diaries are survivor records with privacy and access rules. Reading one
    can create a caregiving opportunity; it should not apply a hidden
    unconditional morale bonus.
13. Pneumatic capsules reduce message latency. They do not teleport workers or
    mutate the DutyRoster immediately when the player presses a button.
14. Drone recon supplies uncertain intelligence and can fail under weather or
    EMP. It does not guarantee zero casualties or reveal every turret.
15. The master chronometer must coordinate the existing IClock and ISimClock
    contracts. It must not introduce a third authoritative clock.
16. Hall of Fame data is metagame profile state, separate from a live campaign
    save. Completing or deleting a campaign must not corrupt or rewrite the
    campaign's checksum envelope.

## Non-negotiable project constraints

These constraints apply to every slice in this document:

- Godot is the active host. Unity is legacy and read-only for this task.
- Core simulation code belongs under Assets/Ashfall.Core/ and must remain free
  of Godot, Unity, GodotSharp, UnityEngine, and JsonUtility references.
- Godot presentation, input, audio, scene wiring, and view-model assembly
  belong under src/. Hosts must not become the gameplay authority.
- Assets/StreamingAssets/Data/ is the data authority. New IDs must be
  snake_case, schema-versioned, and validated by CatalogIntegrityValidator.
- Extend existing systems before adding parallel authorities. A catalog class
  is not proof that its mechanic exists.
- Stateful systems require serializable CaptureState and RestoreState DTOs.
  New host stores require versioned checksummed envelopes.
- Use ISeededRng for simulation decisions. Do not use System.Random,
  Guid.NewGuid, unordered dictionary iteration, runtime HashCode, or
  presentation-only randomness for authoritative outcomes.
- Use stable ordinal sorting for all saved collections, candidate lists,
  route choices, and tie breakers.
- Resource operations must be atomic: validate all costs and capacity before
  consuming anything, and roll back or route to a defined overflow on failure.
- Use real canonical item, location, faction, route, survivor, disease, and
  expansion IDs. Do not invent IDs in host code.
- Store units explicitly. Distinguish game hours from days, kW from kWh,
  liters or gallons from inventory units, Roentgens from dose ledger units,
  degrees Celsius from Fahrenheit, and route distance from travel time.
- Keep the tone cold, exhausted, human, and restrained. Use records,
  manifests, work orders, test sheets, and personal notes to make systems
  legible without turning them into spectacle.
- Preserve unrelated user changes. The current worktree contains modified
  catalog loaders and many JSON files plus an untracked legacy serializer.
  No Batch 6 work may reset, reformat, or overwrite those changes.

## Dependency-first delivery strategy

The sixteen features are delivered as vertical slices. A slice includes Core
state, data references, host wiring, UI feedback, save coverage, and tests.
Do not start by building all sixteen panels.

### Phase 0 — authority and contract audit

Before writing new mechanics:

1. Map the existing route/location graph and identify every crossing node.
2. Inventory the active power, battery, fuel, generator, breaker, and room-load
   code. If no authoritative grid exists, approve one minimal Core contract.
3. Identify the canonical water, food, medical, material, capsule, paper,
   animal, tool, and radio IDs already present in JSON.
4. Confirm the active injury and disability representation in MedicalSystem,
   CombatTraumaSystem, and survivor state before adding prosthetics.
5. Confirm the canonical day/hour tick source and decide where schedule events
   are raised between IClock and ISimClock.
6. Confirm how completed campaigns are identified and where profile metadata
   can be stored without entering a campaign checksum.
7. Run the existing data-integrity and bridge self-tests before touching data.

Phase 0 produces an authority map and an ID/unit ledger. If a proposed feature
has no valid owner, the feature is split into a prerequisite contract rather
than hidden in a Godot panel.

### Phase 1 — shared infrastructure contracts

Implement the smallest shared contracts needed by multiple features:

- Crossing infrastructure and route modifiers for 81.
- Power production, loads, storage, and outage semantics for 83 and 85.
- Disease airlock adapter around DiseaseSystem for 91.
- Clock schedule events on the existing time contracts for 95.
- Optional common event payload conventions for all new state changes.

### Phase 2 — communication and logistics

Implement 82, 93, 90, and 94 after the time, power, and route contracts are
stable. These features exercise deterministic messages, delayed tasks, escort
contracts, and uncertain map intelligence.

### Phase 3 — resources and capability recovery

Implement 84, 86, 87, 88, and 89. These features bind inventory, craft
capacity, medical state, water, storage, and expedition capability.

### Phase 4 — records and metagame continuity

Implement 92 and 96 after the underlying survivor, journal, epilogue, and
campaign-completion seams are stable.

### Recommended first implementation slice

The first coding slice should be the shared Phase 0/Phase 1 contract work,
followed by Step 81 only after the route authority is confirmed:

1. Add no UI.
2. Write the authority and ID/unit ledger.
3. Add route infrastructure state and save DTOs.
4. Apply one bounded route travel modifier to one canonical crossing.
5. Add Core tests for closed, partial, open, failed, and restored states.
6. Wire a minimal Godot read-only status view only after the Core behavior is
   deterministic and persisted.

If the power audit confirms that no grid authority exists, power contract work
must precede Step 83 and Step 85 even though those numbers are later in the
roadmap.

## Existing repository seams to preserve

| Concern | Existing authority | Batch 6 use | Constraint |
|---|---|---|---|
| Crossing access | CrossingSession, VouchAccessSystem, CrossingQuestSystem | Step 81 | Separate social vouch from structural repair |
| Route and locations | LocationLayoutSystem, expedition/world route data | Steps 81, 88, 90, 94 | Resolve canonical IDs before host wiring |
| Seasonal travel | IceRoadSystem | Steps 81, 90 | Preserve seasonal gates and route-specific effects |
| Thermal geothermal | YearOfAshDeepFreezeSystem, GeothermalHeatingWidget | Step 83 | Extend thermal seam; do not mislabel it as power |
| Radiation structure | MaterialShieldingSystem | Steps 81, 85, 88 | Add staged construction without replacing attenuation |
| Inventory | Inventory, ItemDefinition, DeviceState | Steps 82, 83, 84, 85, 86, 88, 89, 90, 93, 94 | Resolve canonical item definitions and use atomic transactions |
| Crafting | CraftingSystem | Steps 84, 89 | Reuse queues, stations, wear, overflow, and recipes |
| Medical trauma | MedicalSystem, CombatTraumaSystem, respiratory systems | Steps 84, 91, 92 | Confirm injury representation before adding prosthetics |
| Disease | DiseaseSystem, DiseaseHeadlessDemo | Step 91 | One disease authority; airlock is an exposure adapter |
| Radio | FactionRadioEngine, RadioSave, RadioHostSession | Step 82 | Replace missing RadioInterceptionSystem reference with actual seam |
| Journal | JournalSystem, JournalSaveStore, JournalPanel | Steps 82, 89, 92, 94 | Journal evidence is not a second simulation ledger |
| Caravan | TravelingCaravanSystem, TravelingCaravanHostSession, CaravanSaveStore | Step 90 | Extend route/guard contract; sort saved entries |
| Generations | GenerationalSuccessionEngine | Step 96 | Use records for dynasty summary, not live campaign mutation |
| Endgame | EpilogueMatrixRuntime, EpilogueContextFactory, EpiloguePanel | Step 96 | Archive only after authoritative completion |
| Audio | AudioManager, AudioCueCatalog, AudioEventBridge | Steps 82, 93, 95 | Audio remains host presentation |

## Shared implementation contract

Every new Core system must document these questions before implementation:

1. What state does it own?
2. Which existing system owns each input it reads?
3. Which command changes state?
4. What validation makes the command fail without partial cost?
5. What deterministic inputs decide an uncertain result?
6. Which event describes the state change?
7. What does CaptureState contain?
8. How does RestoreState reject or normalize malformed data?
9. Which JSON definitions and IDs are required?
10. What does the Godot host display when the operation is blocked, delayed,
    failed, completed, or restored?

Preferred command shape:

- Validate IDs, actor authority, resources, capacity, and phase.
- Compute the result using explicit inputs and ISeededRng.
- Mutate one state transaction.
- Emit one domain event with the result and resource deltas.
- Mark the owning save section dirty.
- Let the host update presentation and audio from the event.

Do not let a panel directly alter a survivor's needs, a route's speed, a
caravan's inventory, a disease stage, or a campaign archive.

## Batch-wide save and determinism requirements

Every new save DTO should include:

- schema version;
- stable IDs for entities and definitions;
- current simulation day and hour where time-sensitive;
- last processed tick or event sequence where duplicate processing is possible;
- explicit phase/status values rather than inferred UI state;
- resource quantities and units;
- deterministic ordering for lists;
- migration behavior for missing fields;
- validation behavior for unknown future versions.

Save tests must cover:

- clean capture/restore;
- mutated state changes the checksum;
- null or empty checksum is rejected for new envelopes;
- pre-checksum legacy fallback, if the store supports it;
- unknown IDs are rejected or quarantined without deleting valid state;
- duplicate event delivery does not double-spend resources;
- a save taken during a queued operation resumes exactly once.

Cross-system deterministic tests must compare two fresh sessions with the same
seed, the same ordered commands, and the same canonical data. Compare state
DTOs and emitted event payloads, not UI strings.

Known repository hazards to address when a feature touches them:

- FactionRadioEngine currently has insertion-order and runtime-hash risks in
  tie and fallback selection.
- MaterialShieldingSystem captures dictionary entries without explicit sort.
- GenerationalSuccessionEngine captures dictionary records without explicit
  sort.
- TravelingCaravanSystem captures active caravans and inventory in runtime
  order.
- CombatTraumaSystem captures survivor dictionary entries without sorting.

## Data authority and catalog plan

Use existing narrative catalogs as authored reference material where possible.
The following files were found and should be treated as evidence, not as
proof of an active mechanic:

- geothermal_steam_well_logs.json and geothermal_steam_vent_diagnostics.json;
- geothermal_borehole_logs.json;
- artesian_well_contamination_logs.json;
- drone_carrier_blackboxes.json;
- wasteland_wildlife_bestiary.json;
- wasteland_trade_caravan_routes.json;
- dweller_psychological_journals.json;
- pneumatic_carrier_capsule_logs.json;
- pneumatic_tube_diverter_audits.json;
- pneumatic_cylinder_leather_assays.json;
- narrative paper, hydrogeology, steam, medical, and horology catalogs.

For each mechanic, first determine whether the data is:

- a narrative reference only;
- a definition catalog consumed by Core;
- a canonical item or location source;
- or an orphan that needs a loader and integrity registration.

New data should be added only after that classification. Every new data file
needs schema_version, snake_case property names, stable IDs, required-field
validation, and CatalogIntegrityValidator registration when its values are
cross-references.

The data plan should minimize new files. Prefer extending an existing
canonical catalog when its schema and ownership are correct. Do not create one
file per UI panel merely to make the feature appear complete.

## Feature implementation plans

## [81] Wasteland Bridge Repair and River Crossing Engineering

### Intended player outcome

The player can survey a named collapsed bridge, commit materials and labor in
stages, handle a structural incident, pass an inspection, and unlock the
specific route for future expeditions, caravans, and treaty traffic.

### Current seam and architectural correction

CrossingSession currently answers whether a crossing node is socially
accessible through VouchAccessSystem. CrossingQuestSystem adds narrative
stages. Neither system owns structural construction. Add a separate
CrossingInfrastructureSystem, or a clearly named extension under
Ashfall.Core.Crossing, keyed by canonical route and bridge IDs.

Do not overload VouchAccessSystem with girders, concrete, bridge integrity, or
travel speed. A player may have a vouch and still face a collapsed bridge; a
repaired bridge may still require a vouch.

### Core work

Define a bridge record with:

- bridge_id and route_id;
- start_location_id and end_location_id;
- construction phase: surveyed, foundation, structure, deck, inspected,
  open, failed, or abandoned;
- required and delivered material quantities by canonical item ID;
- assigned work hours or crew IDs;
- structural integrity and design load;
- current environmental hazard and last incident day;
- inspection status and failure reason;
- route travel modifier when open;
- access flags that remain delegated to CrossingSession and VouchAccessSystem.

The system should expose commands resembling:

- SurveyBridge;
- DeliverMaterial;
- AssignCrew;
- AdvanceConstruction;
- ResolveInspection;
- RepairDamage;
- OpenRoute.

Each command validates the phase and consumes materials atomically. Delivery
must not remove inventory if the bridge cannot accept the complete quantity.
Construction progress should consume actual worker time or a Core work-order
result, not a button press.

Use a deterministic incident roll keyed by campaign seed, bridge ID, phase,
simulation day, and a stable incident counter. Weather, river state,
radiation, material quality, and worker skill should be explicit inputs.
Avoid a hidden chance that changes after loading.

Travel integration must apply the modifier only when:

- the bridge is open;
- the route is not blocked by IceRoadSystem or another hazard;
- the expedition or caravan actually uses that route;
- access and treaty checks succeed.

No global eastern-sector multiplier is allowed. If several routes share a
bridge, the route graph must name those edges explicitly.

Capture and restore must sort bridge records by bridge_id and material entries
by item ID. A restored partially built bridge must resume the same phase and
not re-consume a delivery.

### Data and IDs

Phase 0 must identify canonical location and route IDs. If bridge definitions
are missing, add one schema-versioned infrastructure catalog with:

- bridge_id;
- route_id;
- construction phases;
- material requirements;
- crew and tool requirements;
- environmental modifiers;
- inspection thresholds;
- bounded travel modifier;
- repair rules.

Every referenced material must resolve through the canonical inventory data.
Do not use display names such as "scrap steel" as identifiers.

### Godot work

Create BridgeEngineeringPanel.cs as a presentation adapter, preferably
adjacent to the existing CrossingQuestPanel. The panel should show:

- the current bridge phase and route endpoints;
- delivered versus required materials;
- crew allocation and estimated remaining work;
- structural integrity and inspection warnings;
- the social access state separately from structural state;
- the exact route effect after opening.

Buttons issue commands and render failure reasons. They do not remove
inventory, mutate route data, or calculate travel time locally.

Add a small read-only route-map marker after the Core slice passes tests. The
marker should distinguish "closed", "under repair", "open but gated", and
"open".

### Acceptance tests

- A closed bridge blocks the named crossing route.
- Delivering an insufficient or unknown material changes nothing.
- A full delivery is consumed exactly once.
- A phase transition persists through capture and restore.
- Opening one bridge changes only its canonical route edge.
- IceRoadSystem can still block a repaired route in its own season.
- VouchAccessSystem can still deny a route after structural completion.
- A failed inspection creates a repair task without losing prior progress.
- Same seed and commands produce identical phase, incident, and route state.
- Save lists are stable regardless of dictionary insertion order.

### Adversarial review questions

- Does the implementation accidentally treat a social vouch as a construction
  permit?
- Can the UI create a bridge for a route absent from canonical data?
- Is the advertised speed improvement applied to caravans as well as scouts?
- Can a failed bridge be reopened by toggling a Godot boolean?
- Does a restore replay the last construction tick or duplicate material cost?

### Done when

One canonical bridge can be repaired end-to-end in a headless Core scenario,
its route modifier is applied by the real travel authority, access gates remain
separate, state survives a checksummed save, and the Godot panel displays the
same state without owning any rules.

## [82] Interactive Morse Code Telegraph Keyer and Audio Decoder

### Intended player outcome

The player keys a message, receives a noisy transmission, decodes it with
usable timing assistance, and earns a canonical radio, journal, rescue, or
location consequence when the message is actually resolved.

### Current seam and architectural correction

The active radio path is FactionRadioEngine, RadioSave, and RadioHostSession.
The named RadioInterceptionSystem was not found. Do not add a phantom
dependency to a panel. Extend the real radio seam or introduce a focused
TelegraphSystem under Ashfall.Core.Radio only if Morse transmissions need
state that FactionRadioEngine should not own.

Morse symbols, decoding, channel state, and contact consequences are Core
concerns. Key-down timing, sidetone audio, animation, accessibility, and
transcript layout are Godot concerns.

### Core work

Define a deterministic telegraph record with:

- station_id and channel_id or frequency;
- current calibration and key condition;
- power or battery source;
- outgoing message ID and canonical payload type;
- encoded symbols and transmission progress;
- signal quality, noise, and retry count;
- incoming signal ID, received symbols, decoded state, and confidence;
- contact state and cooldown;
- journal or world-evidence key emitted on successful resolution;
- last processed day/hour and event sequence.

Use a canonical International Morse table, either in a validated static
contract or a schema-versioned data catalog. Do not let locale, keyboard
layout, frame rate, or audio latency alter the authoritative symbol sequence.

The host may submit a normalized symbol sequence, an accessibility-assisted
letter selection, or a complete text payload after validating the same
encoding. Timing can affect signal quality and noise, but it must be bounded
and testable.

Fix radio determinism risks when touching the engine:

- sort or explicitly order frequency candidates before tie resolution;
- replace runtime-dependent HashCode fallback with a stable Core hash;
- ensure channel selection is independent of dictionary insertion order.

A decoded signal can unlock an existing journal entry, radio broadcast,
encounter, or location lead. A new hidden bunker must be a canonical data
definition or an explicitly generated deterministic lead with a save ID.

### Godot work

Create TelegraphKeyerModal.cs and reuse AudioCueCatalog.RadioMorse through the
host AudioManager. The modal should provide:

- a spring-key control and keyboard alternative;
- adjustable timing tolerance and visual dit/dah feedback;
- a Morse reference chart;
- current frequency, signal strength, and battery status;
- partial transcript and confidence;
- clear error states for noise, power loss, and invalid code;
- a non-audio path for hearing accessibility.

Audio must be event-driven. The panel must not decide that a transmission
worked just because an animation completed.

### Data and persistence

Extend radio data only with canonical stations, channels, message payloads,
and consequences. Link journal keys and location leads through integrity
validation. Do not put arbitrary prose or a hidden rescue survivor in the
scene script.

### Acceptance tests

- The same symbol sequence decodes the same message across hosts.
- Invalid symbols fail without spending the full resource cost.
- A partial or noisy transmission stores its progress and resumes once.
- Duplicate delivery of a decoded message does not duplicate its consequence.
- Radio candidate tie resolution is stable across registration order.
- Runtime restarts do not change an authoritative contact result.
- A failed transmission leaves a journal diagnostic only when specified.
- Capture and restore preserve symbol progress, retry count, and cooldown.

### Adversarial review questions

- Is the keyer a skill toy whose outcome is secretly decided by a UI random
  number?
- Does the feature reference a class that does not exist?
- Can a player type an arbitrary payload that grants arbitrary resources?
- Does audio timing make the game inaccessible or change a saved outcome?
- Is the journal entry being used as a second radio state store?

### Done when

The player can send and decode one canonical Morse exchange, the consequence
is resolved by Core and recorded through the existing radio and journal paths,
and the Godot control remains replaceable without changing the result.

## [83] Subterranean Geothermal Vent Tap and Continuous Base Power

### Intended player outcome

The player converts a surveyed geothermal vent into a maintained electrical
source, sees its production compete with room loads, and handles pressure,
scaling, corrosion, and outage conditions.

### Current seam and architectural correction

YearOfAshDeepFreezeSystem already has geothermalFlowRatePercent, intake icing,
indoor temperature, and freeze-pipeline behavior. GeothermalHeatingWidget is a
thin thermal view. Neither is a power distribution authority. Build the
smallest PowerDistributionSystem needed to represent production, loads,
storage, and allocation, then let the geothermal source feed it.

Do not hide power generation inside GeothermalHeatingWidget or retrofit
electrical side effects into the thermal temperature calculation.

### Core work

Introduce or extend a Core power contract with:

- named power sources and source phase;
- generated power in kW;
- requested and allocated room loads in kW;
- battery or capacitor storage in kWh;
- fuel consumption for fuel sources;
- breaker/load priority state;
- brownout and outage thresholds;
- source efficiency and maintenance condition;
- deterministic event sequence for trips and recovery.

Define GeothermalSourceState with:

- vent_id and survey, drilling, or commissioning phase;
- pressure, temperature, flow, and safe operating range;
- turbine RPM and output cap;
- sulfur scaling and corrosion;
- descaling and maintenance progress;
- valve state, emergency trip, and last incident;
- thermal contribution to Deep Freeze, if applicable;
- electrical contribution to PowerDistributionSystem.

The source output is bounded by catalog data and current condition. A default
30 kW figure is a design target, not a free guaranteed result. A source must
not supply more than the grid can accept or store. Overflow must be curtailed,
diverted to a defined heat sink, or cause a controlled trip.

Make the power contract explicit about tick granularity. If the grid ticks
hourly, kW becomes kWh per hour; if it ticks in smaller intervals, use the
actual interval. Do not mix a daily thermal tick with an hourly electrical
quantity.

Integrate fuel generators and batteries only through the shared contract.
Existing thermal geothermal flow should continue to affect freeze state
through its existing path, with no duplicate temperature calculation.

### Godot work

Extend GeothermalHeatingWidget or create GeothermalPanel.cs for:

- vent commissioning stage;
- pressure and temperature;
- turbine output and scaling;
- allocated versus unused power;
- descaling and maintenance actions;
- thermal and electrical outputs shown as separate rows;
- warning state for pressure, corrosive scaling, and trip.

Create PowerGridPanel.cs only after the shared contract exists. Room widgets
must render the Core load state and request breaker changes through commands.

### Data and persistence

The existing geothermal steam and borehole narrative files may supply
inspection language and authored logs. Add canonical source definitions only
where they are missing, with safe output caps, maintenance costs, and units.
Every source needs a stable vent ID and a route or location reference if it is
physically located in the bunker.

Save source, grid, battery, and thermal linkage state in separate DTOs so an
older thermal save can migrate without inventing power state. New envelopes
must be checksummed.

### Acceptance tests

- A non-commissioned vent produces no electrical power.
- Commissioning consumes the declared materials and labor exactly once.
- Output is capped and allocated according to priority.
- A scaling threshold reduces output or trips the source deterministically.
- A restored source resumes pressure, maintenance, and output exactly.
- Deep Freeze thermal behavior remains unchanged when the electrical load is
  zero.
- A grid outage affects only systems connected to the grid.
- Fuel consumption reflects allocated load and tick duration.
- Power cannot be duplicated by re-running the same tick.
- Same seed and input state produce the same trip and maintenance outcome.

### Adversarial review questions

- Did the feature create a second battery or generator ledger?
- Is the 30 kW number hard-coded in the panel?
- Can excess power silently disappear without a state or event?
- Is a thermal geothermal percentage being presented as electrical output?
- Does the source provide power before drilling and commissioning are complete?

### Done when

One vent can be commissioned, produce bounded power through a shared grid,
affect a real connected load, remain distinct from thermal geothermal state,
and survive a versioned checked save.

## [84] Survivor Bio-Mechanical Prosthetics and Mobility Workshop

### Intended player outcome

A survivor with a canonical disabling injury can receive a fitted prosthesis,
undergo rehabilitation, return to selected duties, and maintain the device as
its condition changes.

### Current seam and architectural correction

CombatTraumaSystem currently models combat encounters, hypervigilance, false
alarms, and companion grounding. It does not model limbs or amputation.
MedicalSystem and inventory equipment must be inspected for the active injury
authority before a prosthetic implementation begins. Do not attach a limb
field to CombatTraumaSystem merely because its name contains trauma.

If no limb injury representation exists, first add a minimal medical injury
contract or extend the existing canonical injury state. A ProstheticsSystem
must consume that state; it must not create a parallel health system.

### Core work

Define prosthetic and fitting state with:

- survivor_id and injury_id;
- affected capability or limb slot using a canonical enum or ID;
- medical eligibility and wound-healed phase;
- socket measurement and fit quality;
- prosthetic item ID and material;
- alignment, spring or pneumatic tuning, and condition;
- pain, skin irritation, infection risk, and rehabilitation progress;
- movement, work, expedition, and combat capability modifiers;
- maintenance interval and failure state;
- last treatment day and event sequence.

Separate capability restoration from health restoration. A prosthesis may
improve movement or tool use while leaving pain, fatigue, infection risk, and
reduced maximum capability visible in survivor state.

Use Inventory and CraftingSystem for canonical prosthetic items, materials,
station wear, and overflow. Equipment must be compatible with the survivor's
injury and cannot be equipped on an uninjured survivor without a defined
accessibility or assistive-equipment rule.

Medical callbacks should apply actual rehabilitation and infection outcomes.
Do not directly set health or morale from the UI.

### Godot work

Create ProstheticsBenchPanel.cs with:

- patient and injury summary;
- fitting measurements and materials;
- socket and alignment controls;
- rehabilitation schedule;
- condition and maintenance status;
- accessible text for pain, risk, and expected capability;
- confirmation before any invasive or irreversible treatment.

The panel should allow a player to understand that a lower outcome is not a
failure of the person. Avoid framing disability only as a combat penalty.
Show work, mobility, rest, and comfort options.

### Data and persistence

Canonical definitions need prosthetic item IDs, compatible injury or capability
IDs, recipes, fit ranges, maintenance costs, and medical prerequisites.
Narrative heirloom or medical casebook entries can enrich presentation but
must not be used as hidden rules.

### Acceptance tests

- An ineligible survivor cannot begin fitting.
- A fitting consumes materials only after all validation passes.
- A fitted device changes the actual capability query used by work or travel.
- Rehabilitation progresses with elapsed time, not a UI refresh.
- Condition loss and maintenance are saved and restored.
- Infection or skin-risk rules resolve through MedicalSystem.
- Removing a prosthesis does not erase the underlying injury.
- Same seed gives the same failure or complication result.
- Two independent survivors do not share mutable fitting state.

### Adversarial review questions

- Is "90 percent mobility" a hard-coded promise unrelated to the injury?
- Does the panel overwrite the survivor's health or trauma directly?
- Is an amputation invented by the prosthetics UI instead of MedicalSystem?
- Can a device be duplicated by opening two panels?
- Does the design treat a survivor as repaired equipment rather than a person?

### Done when

One canonical injury flows through medical eligibility, craft and fitting,
rehabilitation, capability queries, maintenance, and save or load without
duplicating trauma or inventory authority.

## [85] Wasteland Lightning Rods and EMP Grounding Arrays

### Intended player outcome

The player can install and maintain a grounded surface array, decide how much
storm energy to route into safe storage, and protect sensitive bunker
equipment from an EMP or lightning surge.

### Current seam and architectural correction

WeatherKind already includes EMPStorm and AshLightning, but the audit did not
find an active electrical storm or power-grid effect authority. Step 85 must
consume the authoritative weather event once and pass its result into the
shared PowerDistributionSystem from Step 83.

A lightning rod is both a hazard-control device and an energy path. It is not
a universal storm battery and must not bypass surge, storage, or grounding
rules.

### Core work

Define a grounding-array record with:

- array_id and surface location;
- rod condition and grounding quality;
- breaker and diversion state;
- capacitor or battery target;
- surge rating and current stored energy;
- connected equipment protection groups;
- maintenance, corrosion, and last strike day;
- EMP exposure and recovery status.

WeatherSystem should emit a deterministic hazard result containing the storm
identity, strike opportunity, intensity, and event sequence. The array resolves
capture using explicit condition, grounding, capacity, and protection inputs.
Do not roll a second hidden weather event in the array.

Model overflow explicitly:

- safe capture into available kWh;
- controlled discharge;
- breaker trip;
- equipment damage or EMP lockout;
- missed capture when the array is offline.

If batteries and capacitors share the Step 83 power contract, preserve their
storage units and charge limits. A design target such as 50 kWh must be
bounded by the actual strike and storage capacity.

### Godot work

Extend PowerGridPanel.cs with:

- storm status and strike warning;
- rod and grounding condition;
- breaker and diversion state;
- capacitor charge and safe capacity;
- protected versus exposed equipment;
- repair and reset commands.

The HUD may flash an alarm or play an electrical cue, but event resolution
comes from Core.

### Data and persistence

Add canonical array definitions only if the current shelter data lacks them.
They should specify location, materials, surge rating, maintenance, and
protected equipment groups. Link EMP consequences to real device or room
systems; do not create a visual-only outage.

### Acceptance tests

- No capture occurs without an active storm strike.
- A grounded, maintained array captures only within capacity.
- A disabled or ungrounded array cannot silently charge storage.
- Overflow follows the declared safe-discharge or damage path.
- EMP affects only connected or exposed systems.
- Replaying the same weather event does not produce a second strike.
- Save and restore preserve charge, condition, and event sequence.
- Same seed gives the same strike and capture result.
- A lightning event cannot create negative fuel or over-capacity storage.

### Adversarial review questions

- Is the result a free +50 kWh button tied to a weather label?
- Does the panel resolve a strike before WeatherSystem does?
- Is EMP damage merely an alarm texture with no simulation effect?
- Can the player harvest unlimited energy by reopening the panel?
- Does the array share the same units and storage authority as geothermal power?

### Done when

One canonical array handles a deterministic storm event, protects a real
connected load, stores bounded energy through the shared power contract, and
persists safely.

## [86] Subterranean Ice House and Natural Permafrost Cold Storage

### Intended player outcome

The player harvests and stores winter ice, assigns insulation, and uses a
limited cold room to slow spoilage of compatible food and medical goods during
power shortages.

### Current seam and architectural correction

The audit found narrative refrigeration and fermentation material, but no
confirmed active universal cold-storage authority. Phase 0 must locate an
existing spoilage model. If none exists, implement a focused
PerishableStorageSystem before building the panel. Do not let IceHousePanel
invent item spoilage.

### Core work

Define a storage state with:

- ice-house ID and capacity;
- ice blocks and insulation material;
- cellar temperature and target range;
- melt rate based on ambient temperature, insulation, access, and day;
- stored item stacks and their arrival times;
- per-item temperature tolerance and spoilage class;
- medical cold-chain requirements where relevant;
- contamination or seal status;
- last tick and maintenance condition.

Connect the storage rule to the canonical inventory or food/medical
repository. Items should remain identifiable by canonical item ID. A transfer
must validate capacity, temperature compatibility, and contamination before
removing goods from the source.

Use actual elapsed time for melt and spoilage. Power-free operation means the
ice house does not draw electricity for refrigeration; it still consumes ice,
insulation, maintenance, and physical capacity.

Vaccines, blood, and other medicines must have explicit cold-chain data. Do
not silently treat all medical items as safe at one temperature.

### Godot work

Create IceHousePanel.cs with:

- available ice, insulation, and capacity;
- current and projected temperature;
- item slots and cold-chain warnings;
- melt and spoilage timers;
- loading, unloading, sealing, and maintenance commands;
- a clear distinction between power use and resource use.

### Data and persistence

Extend item definitions or a storage-condition catalog with:

- spoilage class;
- safe temperature range;
- maximum warm exposure;
- contamination behavior;
- medicine or food category.

Reuse existing refrigeration narrative entries for text and discovery, not
for hidden calculations. Save stored stacks in stable order with timestamps
and schema version.

### Acceptance tests

- A sealed, insulated ice house lowers temperature without power draw.
- Melt rate increases with poor insulation or warm weather.
- Ice capacity cannot exceed the physical store.
- An incompatible or over-capacity transfer is atomic and fails cleanly.
- Compatible food spoilage is slowed, not removed.
- A cold-chain medicine records and enforces its temperature exposure.
- A restored store resumes from the same tick and temperatures.
- Removing the last ice changes the next spoilage outcome deterministically.

### Adversarial review questions

- Does "six months" appear as a universal hard-coded guarantee?
- Is spoilage calculated in both Inventory and IceHouseSystem?
- Can a player hide unlimited goods by moving them into a UI slot?
- Do medical items have the same tolerance as food without a data reason?
- Does the feature incorrectly promise zero power and zero maintenance cost?

### Done when

An ice house protects a defined set of perishable items through an actual
power outage, with visible resource trade-offs, bounded spoilage, and safe
capture and restore.

## [87] Wasteland Tracking Hounds and Scout Canine Kennel

### Intended player outcome

The player rescues or acquires a named hound, trains it, assigns a compatible
handler, and receives useful but uncertain tracking and morale benefits on
expeditions.

### Current seam and architectural correction

No active canine or animal-companion Core authority was found. Wildlife
bestiary content and narrative references are not enough to wire a kennel.
Create an AnimalCompanionSystem only after canonical animal IDs, food, housing,
and treatment definitions are available. Do not place dog state in
CombatTraumaSystem merely because companions can reduce false alarms.

### Core work

Define companion state with:

- animal_id, species or breed definition, and name;
- health, hunger, fatigue, injury, and condition;
- temperament and handler relationship;
- scent or tracking training progress;
- command reliability and noise profile;
- expedition assignment and current location;
- veterinary or kennel requirements;
- companion morale and grounding relationship;
- breeding eligibility only if a full population model is approved.

Integrate with ExpeditionSystem through bounded modifiers:

- tracking evidence quality;
- early-warning probability;
- route or wounded-game detection;
- noise or food costs;
- fatigue and weather constraints.

An early warning should produce evidence and a chance to detect a threat, not
guarantee that every ambush is revealed. If TacticalCombatSystem consumes
initiative modifiers, it must read the companion result through an explicit
combat input.

Use CaregivingSystem or SurvivorNeedsState for the handler relationship and
CombatTraumaSystem.SetGroundedByCompanion only as a narrow existing effect.
Do not duplicate morale or trauma state inside the kennel.

### Godot work

Create KennelPanel.cs with:

- animal dossier and condition;
- feeding, rest, treatment, and training;
- handler assignment;
- expedition companion slot;
- likely benefit and risk range;
- warning when the animal is too fatigued, injured, or hungry.

The expedition panel should show the hound's actual current evidence bonus
after the Core command resolves.

### Data and persistence

Add canonical animal and companion definitions only where none exist. The
definition must identify food, housing, training actions, condition limits,
and modifiers. Do not use display names or narrative animal labels as IDs.

Breeding is out of the first slice unless the project has a population and
genetics authority. Rescue, train, and assign are sufficient for Batch 6.

### Acceptance tests

- A hungry or injured hound cannot be assigned as fit for expedition duty.
- Feeding and treatment consume canonical resources atomically.
- Training progresses with time and valid handler assignment.
- Same seed and expedition inputs produce the same warning result.
- A warning modifies a real expedition or combat input, not only a label.
- A hound does not reveal every hidden location or attack.
- Companion grounding affects false-alarm behavior only through the existing
  narrow hook.
- Animal and handler state survives save and restore without shared references.

### Adversarial review questions

- Is the dog a guaranteed radar?
- Does the UI invent a dog because the narrative catalog mentions one?
- Can a hound be assigned simultaneously to two expeditions?
- Does feeding bypass inventory capacity or daily needs accounting?
- Is breeding being added without a stable animal population model?

### Done when

One named hound can be maintained, trained, assigned, and consumed by a real
expedition risk calculation, with humane status feedback and deterministic
save behavior.

## [88] Deep Artesian Well Borehole and Uncontaminated Aquifer

### Intended player outcome

The player surveys a borehole, drills through staged strata, manages bit wear
and pressure, tests the recovered water, and connects a validated source to
clean-water storage.

### Current seam and architectural correction

HydroGeologyCatalog and artesian contamination logs exist as narrative or
catalog material. BrineWaterSystem is an active water-related seam, but no
confirmed deep-well extraction authority was found. Add a focused
AquiferExtractionSystem or extend the canonical water-source system; do not
make the panel write directly into NeedsSystem.

An aquifer may be shielded from fallout and still have salinity, mineral,
pressure, casing, pump, or contamination problems. Purity is a tested state,
not a permanent string in the UI.

### Core work

Define borehole state with:

- well_id and bunker or location reference;
- drilling phase and current depth;
- geological strata and target aquifer ID;
- drill-bit condition and replacement cost;
- casing and seal integrity;
- pressure, flow rate, pump state, and power draw;
- sample status and contamination confidence;
- salinity, mineral, or heavy-metal flags;
- recharge or sustainable draw limit;
- last maintenance day and incident sequence.

Commands should include survey, drill, replace bit, case, sample, approve
source, pump, and shut down. Pumped water must enter the canonical water
ledger with an explicit source and quality. Approval may route water directly
to clean storage only after the configured tests pass; otherwise it goes to
raw or quarantined storage.

Connect clean water to existing NeedsSystem, DiseaseSystem, greenhouse,
medical, and economy consumers through their current APIs. Do not bypass
waterborne disease checks by changing a UI counter.

Use a sustainable draw rule. If the design does not simulate recharge, state
the source as a bounded reserve rather than claiming infinite water.

### Godot work

Create WellDrillingPanel.cs with:

- depth and stratum record;
- bit wear, casing, and pump condition;
- pressure and flow in explicit units;
- sample and contamination results;
- raw, quarantined, and approved water destinations;
- power and maintenance costs;
- stop and emergency-shutoff controls.

The panel must show uncertainty when a sample is incomplete. It should not
show "100 percent pure" until the Core approval state says so.

### Data and persistence

Extend hydrogeology definitions with strata, depth, expected pressure, flow
range, contaminants, test requirements, pump load, and draw limits. Link
aquifer IDs to canonical locations or shelter nodes.

Use stable source IDs in water history. Save partially drilled boreholes,
samples, and quarantined batches so loading cannot turn unsafe water into
clean water.

### Acceptance tests

- Drilling consumes time, bit condition, and materials only after validation.
- A sample can fail for salinity or contamination.
- Unapproved water cannot enter clean-water consumption.
- Approved flow is bounded by pump and aquifer limits.
- NeedsSystem consumes the correct water quality.
- DiseaseSystem can still react to contaminated water when its rules require.
- A restored borehole resumes depth, sample, and pump state exactly.
- Re-running approval does not duplicate water or clear an unresolved flag.
- Same seed and geologic inputs produce the same incident outcomes.

### Adversarial review questions

- Is the aquifer a no-cost infinite clean-water button?
- Does radiation shielding get confused with chemical purity?
- Is water being counted in gallons in one system and abstract units in
  another without conversion?
- Can a failed sample be approved by reopening the panel?
- Does the well silently replace BrineWaterSystem rather than integrate with it?

### Done when

A canonical borehole can progress from survey to tested source and deliver
bounded water through the existing needs and disease authorities, with
contamination and draw state persisted.

## [89] Scrap Rag Paper Press and Archival Blueprint Publishing

### Intended player outcome

The player converts approved feedstock into paper stock, runs a press, and
uses the output for a defined record, treaty, currency, or blueprint workflow.

### Current seam and architectural correction

CraftingSystem is active and already owns recipes, ingredient consumption,
timed completion, station wear, and overflow. JournalSystem is active.
PaperMakingCatalog and PaperPrintingCatalog are available as narrative or
catalog seams. Start by extending CraftingSystem and ResearchSystem rather
than creating a second crafting queue.

Paper is a physical capability and a record medium. It is not itself
knowledge. Printing a blueprint should create a document or work item; the
ResearchSystem decides whether the underlying knowledge is unlocked.

### Core work

Define a paper production state only for process-specific state that
CraftingSystem does not already own:

- mill or press station ID;
- pulp feedstock and water;
- vat, screen, press, and drying phases;
- paper quality and moisture;
- station wear;
- sheet stock by quality;
- document job type and canonical source record;
- last tick and queued job ID.

Reuse CraftingSystem for ingredients, duration, crafter modifiers, result
capacity, and station wear wherever possible. If a paper job needs quality,
have a deterministic Core result feed the recipe result or document state.

Document jobs should be validated against the source:

- a blueprint needs an existing researched or observed design;
- a treaty needs a valid party, clause, and signing event;
- a currency note needs an approved currency definition;
- a journal or codex copy needs an existing entry.

No host text field may create a new recipe, treaty, or legal currency.

### Godot work

Create PaperMillPanel.cs with:

- feedstock and water;
- process phase and remaining time;
- station condition;
- sheet quantity and quality;
- document job selector;
- source evidence and validation errors;
- overflow handling.

Use the existing JournalPanel or document viewer for the finished record. Do
not build an unrelated archive with a second journal index.

### Data and persistence

Add or extend recipe definitions for canonical rag, cellulose, water, and
press outputs. Use schema-versioned document job definitions if the
repository lacks them. Every document must retain source IDs and authoring
provenance.

### Acceptance tests

- A paper job uses the real CraftingSystem transaction path.
- Missing feedstock or output capacity prevents consumption.
- Station wear and quality survive save and restore.
- A printed blueprint does not unlock unresearched technology.
- A valid journal or treaty document creates one canonical record.
- Duplicate document jobs are rejected or explicitly versioned.
- Same seed gives stable quality and completion outcomes.
- Output lists are stable and checksum-visible.

### Adversarial review questions

- Is the paper mill a reskinned CraftingSystem or an accidental duplicate?
- Can a player print an arbitrary blueprint from a text box?
- Does paper quality have a downstream consumer or exist only as decoration?
- Is a currency note accepted before Currency or Economy authority approves it?
- Can output overflow destroy ingredients without the CraftingSystem contract?

### Done when

One paper batch completes through the real craft queue, produces a canonical
document or stock item, and demonstrates an actual downstream use without
silently unlocking knowledge.

## [90] Nomad Caravan Armed Escort Contracts and Road Protection

### Intended player outcome

The player accepts a named escort contract, reserves a real guard group,
travels a canonical route, responds to a threat, and receives a defined payout
and standing change on arrival.

### Current seam and architectural correction

TravelingCaravanSystem is active and already stores caravan routes, nodes,
guardCount, robbery state, inventory, daily movement, and trade. Its current
trade model is intentionally small. DutyRoster and Expedition systems are the
likely labor and travel authorities. Extend the caravan path with an explicit
CaravanEscortSystem or contract state instead of putting contract rules in
TravelingCaravanHostSession.

The route, merchant, guard, combat, reward, and faction IDs must come from
canonical data. Do not assume a destination named New Meridian exists.

### Core work

Define escort contract state with:

- contract_id and caravan_id;
- merchant or faction ID;
- route ID and route index;
- departure and delivery deadlines;
- assigned survivor or guard IDs;
- equipment and ammunition reservation;
- route hazards, weather exposure, and threat evidence;
- escort phase: offered, accepted, assembling, traveling, ambushed,
  delivered, failed, or cancelled;
- deterministic encounter counter;
- payout, standing delta, loss, and insurance terms.

Acceptance must reserve guards without duplicating them in DutyRoster.
Departure must fail if the required guards, gear, or route access are not
available. On completion, return survivors to the roster and settle the
contract once.

If TacticalCombatSystem can resolve the ambush, pass a Core combat request
through the existing combat host. If it cannot, use an explicitly named
deterministic abstraction and do not create a second ballistic simulation in
the escort class.

Extend TravelingCaravanSystem save capture with stable sorting for caravans,
route inventory, and contract lists when touching it.

### Godot work

Create CaravanEscortPanel.cs with:

- available contract and merchant;
- route map and hazard evidence;
- required guard count and loadout;
- roster reservation state;
- deadline and expected travel time;
- payout and failure terms;
- active escort event and return status.

The panel should make a contract's risk legible. It must not present a
guaranteed reward when the contract has a probabilistic route outcome.

### Data and persistence

Extend wasteland caravan route data or add an escort contract catalog with
canonical route edges, contract windows, guard requirements, threats, rewards,
and standing effects. Reward item IDs must resolve through GoodsCatalog or the
canonical inventory catalog.

### Acceptance tests

- Accepting a contract reserves the exact guards once.
- A missing guard, item, route, or access gate prevents acceptance.
- The caravan advances through actual route nodes and time.
- An ambush resolves once and changes the contract state.
- Successful delivery pays the catalogued reward exactly once.
- Failed or cancelled contracts release or settle reservations explicitly.
- Survivors cannot be assigned to a second conflicting duty.
- Save and restore resume the escort at the same route index and deadline.
- Same seed gives the same encounter result and payout state.

### Adversarial review questions

- Is the reward hard-coded as 200 ammunition?
- Is guardCount only a display number while no survivor is reserved?
- Does the host create a second caravan route or inventory?
- Can the player accept the same contract twice?
- Does a combat failure return guards to the roster without injury state?

### Done when

One canonical caravan escort consumes real roster capacity, travels a real
route, resolves one deterministic risk, and settles through the existing
inventory, standing, combat, and save paths.

## [91] High-Containment Biohazard Airlock and Epidemic Isolation

### Intended player outcome

The player can move an infected or contaminated survivor into a controlled
airlock, maintain isolation conditions, provide treatment, and prevent
unintended spread while the patient remains under DiseaseSystem care.

### Current seam and architectural correction

DiseaseSystem already owns disease state, patient stages, transmission, and
quarantine events. DiseaseHeadlessDemo verifies a quarantine path. The new
airlock is a shelter engineering and exposure-control adapter, not a second
disease engine.

### Core work

Define BiohazardAirlockState with:

- ward or airlock ID and capacity;
- patient queue and bed assignment;
- pressure, airflow, filtration, and UV cycle state;
- fumigation or autoclave process state;
- suit and consumable inventory;
- power, water, chemical, and maintenance requirements;
- contamination level and last exposure event;
- entry, treatment, transfer, and exit sequence;
- linked DiseaseSystem quarantine ID;
- protocol failure and emergency release state.

Commands must call DiseaseSystem quarantine and release APIs. Entry should
fail atomically if no bed, suit, power, or valid patient record exists.
Airflow and pressure can change transmission probability only through an
explicit exposure contract accepted by DiseaseSystem. Do not duplicate
incubation, disease stage, or cure logic.

UV-C and fumigation can sterilize a surface, suit, or room if the data says
so. They should not cure a living patient's disease unless MedicalSystem
already exposes that treatment.

### Godot work

Create BiohazardAirlockPanel.cs with:

- patient identity and disease stage from DiseaseSystem;
- quarantine status and ward capacity;
- pressure and airflow;
- suit, filter, chemical, water, and power stock;
- current procedure and remaining time;
- contamination warning and safe-transfer requirements;
- explicit emergency and failure states.

Use restrained visual language. The screen should communicate risk and duty,
not make illness into a spectacle.

### Data and persistence

Add protocol definitions for air changes, filter classes, UV cycles,
fumigation costs, suit requirements, and power priority. Disease IDs and
treatment IDs must resolve through existing catalogs.

Save the airlock state with a link to DiseaseSystem state. A new-format save
with a missing checksum is corrupt, not legacy. If the airlock gains its own
store, use the established checked envelope pattern.

### Acceptance tests

- A quarantined patient cannot seed a new infection in the tested isolated
  path.
- A patient cannot be assigned to two beds or wards.
- Entry consumes supplies only after capacity and disease validation.
- Pressure failure changes exposure risk through the shared contract.
- UV or fumigation changes only the surfaces or conditions it owns.
- Disease treatment remains in MedicalSystem and DiseaseSystem.
- Exit releases quarantine only when the protocol allows it.
- Save and restore preserve patient queue, cycle, and linked disease state.
- Duplicate events do not quarantine, charge, or release twice.

### Adversarial review questions

- Does BiohazardAirlockSystem contain its own disease progression?
- Does "negative pressure" automatically mean zero transmission?
- Can the player press a UV button to cure any disease?
- Does an airlock move a patient without updating the survivor roster?
- Are supplies charged twice by the host and the Core command?

### Done when

One infected survivor can be isolated, treated by the actual medical and
disease systems, and safely released through a persisted, resource-aware
airlock protocol.

## [92] Survivor Personal Diaries and Intimate Morale Reflections

### Intended player outcome

The player can open a survivor's personal record, understand a concern that
the simulation has actually generated, and respond through an existing
caregiving, duty, rest, or social action.

### Current seam and architectural correction

JournalSystem already owns a capped, deduplicated, saveable discovery log with
authors, timestamps, tabs, and codex knowledge. The diary feature needs a
separate privacy-aware survivor record or a carefully extended journal
category. It must not overload the global journal with private entries that
every survivor can read.

Dweller psychological journal data exists, but it is not proof of a runtime
diary system. Generated prose must be deterministic and derived from state
and authored templates. Do not call an LLM during simulation or make freeform
text a hidden authority.

### Core work

Define diary state with:

- survivor_id;
- entry ID, day, and hour;
- entry category and authored template key;
- relevant needs, event, relationship, or duty evidence IDs;
- emotional signal and confidence;
- visibility and access rule;
- read or acknowledged state;
- linked caregiving opportunity or action;
- generation sequence and deduplication key.

Diary generation should happen on a defined simulation event, such as a
meaningful loss, sustained need, successful treatment, or relationship change.
Do not emit a diary entry every tick. The source event must remain available
for audit and save replay.

Reading a diary should expose information and potentially create a valid
caregiving command. Any morale or relationship change must use
CaregivingSystem, SurvivorNeedsState, or the relevant social authority.

Treat diaries as sensitive records. Access must be explicit in the player
experience and in tests. A player may have administrator access while
survivors or factions do not.

### Godot work

Create SurvivorDiaryModal.cs with:

- survivor identity and entry date;
- handwritten style as presentation metadata;
- the state evidence behind the reflection;
- access state and privacy notice;
- linked actions such as assign rest, common-room time, or speak;
- a non-scripted text fallback for missing or invalid templates.

Do not put generated text in a scene file. Do not expose private content in a
global journal feed unless the survivor or event explicitly makes it public.

### Data and persistence

Extend the psychological journal catalog with deterministic templates, valid
conditions, tone, and permitted actions. Template variables must come from
validated state fields. Save entry IDs and source event IDs.

### Acceptance tests

- One source event creates at most one diary entry for its deduplication key.
- The text and entry metadata are stable for the same state and seed.
- A private entry is not exposed through the public JournalSystem feed.
- Reading creates no direct hidden morale mutation.
- A linked caregiving action changes the actual relevant survivor state.
- Removing or restoring a survivor preserves or intentionally archives diaries
  according to a declared policy.
- Save and restore preserve entry order, privacy, and read state.
- Invalid template keys fail with a legible fallback and no state loss.

### Adversarial review questions

- Is the diary just random flavor text with no state evidence?
- Does opening a modal secretly change morale?
- Can a dead or removed survivor's private diary leak into every tab?
- Is the same entry generated repeatedly because ticks are not deduplicated?
- Are personal facts being persisted outside the campaign save contract?

### Done when

A meaningful survivor event produces one deterministic private reflection, the
player can respond through an existing caregiving path, and the record
survives save/load without becoming an unbounded second journal.

## [93] Bunker Pneumatic Message Capsule Network

### Intended player outcome

The player dispatches a physical capsule between named bunker stations, sees
its delay and condition, receives an acknowledgement or failure, and uses the
result to inform a real work or medical order.

### Current seam and architectural correction

PneumaticTubeDispatchCatalog contains carrier, diverter, and cylinder data.
DutyRosterSystem is active. No active pneumatic transport authority was
confirmed. Add PneumaticDispatchSystem under Core only for network, queue,
latency, and delivery state. DutyRoster remains authoritative for assignments.

The feature must represent faster communication, not instantaneous physical
movement or an out-of-band roster mutation.

### Core work

Define network state with:

- node and directed edge IDs;
- pressure and condition;
- capsule inventory and capacity;
- queue with dispatch ID, sender, recipient, priority, payload type, and
  source command;
- travel duration and arrival tick;
- jam, leak, loss, and maintenance state;
- acknowledgement and duplicate-delivery guard;
- power or labor requirements where applicable.

Supported payloads should be typed commands or requests, for example:

- work order;
- medical requisition;
- emergency alert;
- roster change request;
- journal or document packet.

At arrival, the recipient authority validates and applies the request. A
medical requisition can be denied for missing supplies; a duty order can be
denied for unavailable staff. The tube system records the delivery result but
does not perform the domain mutation.

Queue ordering must be explicit: priority, dispatch day, sequence, and stable
dispatch ID. Save and restore must not deliver a capsule twice.

### Godot work

Create PneumaticDispatchWidget.cs or integrate the widget into the existing
operations panel with:

- source and destination;
- payload type and priority;
- queue and estimated arrival;
- pressure and jam state;
- capsule stock and maintenance;
- acknowledgement or failure reason.

Use the existing audio manager for whoosh and clatter cues after Core events.

### Data and persistence

Extend pneumatic catalog definitions with canonical nodes, routes, capacity,
latency, wear, payload permissions, and maintenance. Verify IDs against
the bunker room or station catalog.

### Acceptance tests

- A capsule is accepted only on a valid route with capacity and stock.
- Queue order is deterministic under equal priority.
- A jam delays or fails delivery without dropping the record silently.
- The recipient system applies an accepted command once.
- A rejected request leaves a reason and does not mutate the target system.
- Save and restore resume the same queue and arrival tick.
- The same capsule cannot be acknowledged twice.
- Audio events are emitted only after state events, not as authority.

### Adversarial review questions

- Does sending a capsule instantly change DutyRoster state?
- Can the player create unlimited capsules by reopening the widget?
- Is a message payload arbitrary text that bypasses command validation?
- Does queue order depend on dictionary iteration or UI order?
- Does a broken tube still deliver an event because the animation completed?

### Done when

One capsule travels through a canonical network, applies a validated work or
medical request after its delay, and survives a checked save without duplicate
delivery.

## [94] Salvaged Reconnaissance Drone Launch and Aerial Photography

### Intended player outcome

The player repairs a drone, chooses a constrained flight plan, obtains
uncertain imagery, and uses the resulting evidence to improve a later
expedition or tactical decision.

### Current seam and architectural correction

ResearchSystem, LocationLayoutSystem, ExpeditionSystem, and drone blackbox
narrative data are available. No active DroneReconSystem was found. Introduce
one only if the project approves an actual drone state authority. Aerial
photographs are evidence with age, quality, and uncertainty, not a complete
map replacement.

### Core work

Define drone mission state with:

- drone_id and airframe condition;
- battery, camera, radio, and payload condition;
- launch and recovery location;
- planned route or survey area;
- flight phase and elapsed time;
- weather, ash, visibility, and EMP exposure;
- signal quality and recovery probability;
- image evidence IDs and confidence;
- map intel coverage and expiration;
- last processed tick and deterministic mission counter.

Research and CraftingSystem should own repair and battery requirements. The
mission system owns flight state and evidence creation. LocationLayoutSystem
owns valid locations and map coordinates.

A recovered image may reveal a canonical route hazard, fortification class,
or evidence marker. TacticalCombatSystem may consume that evidence through an
explicit modifier if supported. Do not reveal exact enemy positions unless a
catalogued sensor and confidence rule support it.

WeatherSystem must provide the active condition; drone logic must not roll a
second weather state. An EMP event may degrade radio or camera state according
to data.

### Godot work

Create DroneReconPanel.cs with:

- airframe and battery condition;
- launch/recovery risk;
- route plotting against valid map nodes;
- live mission phase;
- image preview with confidence and timestamp;
- map evidence overlay;
- recall, abort, and recovery actions.

The image view should communicate uncertainty and stale intelligence rather
than present a perfect tactical overlay.

### Data and persistence

Extend drone blackbox or research catalogs with canonical airframes, sensors,
repair recipes, mission ranges, weather tolerance, evidence types, and intel
lifetimes. Keep authored logs separate from mission state.

### Acceptance tests

- A damaged or uncharged drone cannot launch.
- A mission references only valid locations or survey regions.
- Weather and EMP can affect mission outcome once per event.
- Recovered imagery creates one evidence record with confidence and expiry.
- A lost drone does not return inventory or crew benefits.
- A recalled drone follows an explicit recovery state.
- Save and restore resume mission phase and battery without replaying flight.
- Same seed, route, and weather produce the same mission result.
- Intel modifies only systems that explicitly consume it.

### Adversarial review questions

- Does the drone reveal every turret and hidden node for free?
- Is "zero casualty" being used to bypass expedition risk without a mission
  failure model?
- Does the panel invent coordinates outside LocationLayoutSystem?
- Does an EMP alter a visual sprite but not the camera or radio result?
- Can a completed mission be replayed for unlimited intel?

### Done when

A repaired drone can complete one canonical survey, produce bounded evidence
that a real expedition or combat path can consume, and persist its mission
state and intel lifetime.

## [95] Master Chronometer and Shelter Tower Bell Rotunda

### Intended player outcome

The restored clock provides a reliable, legible rhythm for shift, meal, and
curfew events while requiring calibration and maintenance when its drift
becomes meaningful.

### Current seam and architectural correction

The repository has IClock in HostDefaults.cs and ISimClock under
Ashfall.Core.Clock. They represent day-based and tick-based concerns and are
both in use. Do not create a third authoritative clock. Add a Chronometer
adapter or schedule system that consumes the existing source and emits
idempotent time-bound events.

The bell must not advance simulation time or directly assign every worker.
DutyRoster, NeedsSystem, and other consumers decide how to react to schedule
events.

### Core work

Define chronometer state with:

- clock_id and current calibration drift;
- pendulum and escapement condition;
- maintenance progress and required tools;
- chime schedule IDs;
- last processed day, hour, tick, and schedule sequence;
- bell condition, muted state, and emergency override;
- room or shelter level.

Schedule events should be keyed by day, hour window, and schedule ID. Crossing
a boundary emits once even if the host ticks multiple frames or restores
mid-window. If drift changes the displayed time, the canonical simulation
time remains unchanged; the event policy must state whether drift affects only
display or also arrival tolerance.

Use TimekeepingHorologyCatalog for maintenance and authored records if its
schema is valid. Do not store the actual clock in a UI AnimationPlayer.

### Godot work

Create ClockTowerPanel.cs with:

- displayed clock and authoritative simulation time;
- drift and escapement condition;
- maintenance action;
- chime schedule;
- mute and emergency settings;
- recent schedule events and acknowledgement.

Audio cues are triggered from Core schedule events through SoundManager. The
panel may animate a pendulum, but the animation is not the clock source.

### Data and persistence

Schedule definitions must resolve to existing duty, meal, sleep, or alert
events. If a new event is needed, define its consumer before adding the
schedule. Use explicit local/simulation time semantics; do not infer a
timezone from the computer clock.

### Acceptance tests

- A chime fires once at the configured boundary.
- Multiple frame ticks do not duplicate the event.
- Save and restore at the boundary does not fire twice or skip incorrectly.
- Drift changes the displayed state according to the declared policy.
- Maintenance consumes tools and time through Core.
- A muted bell suppresses audio only; it does not suppress essential state
  events unless the policy says so.
- DutyRoster and NeedsSystem react through their own event handlers.
- Same tick sequence produces the same event order.

### Adversarial review questions

- Is the clock creating a third time source?
- Can changing a UI clock value advance the campaign?
- Are chimes visual/audio only with no consumer, or do they secretly mutate
  every worker?
- Does a save taken at 07:59:59 cause duplicate 08:00 events?
- Is machine wall-clock time leaking into deterministic simulation?

### Done when

The existing simulation clock drives one restored chronometer, schedule events
are idempotent and consumed by real systems, and the Godot rotunda supplies
presentation and audio only.

## [96] Survival Hall of Fame and Eternal Colony Monument

### Intended player outcome

After an official campaign ends, the player can view a durable local archive
of the colony's ending, survival statistics, notable survivors, and dynasty
summary from the main menu or bunker plaza.

### Current seam and architectural correction

EpilogueMatrixRuntime and EpilogueContextFactory already evaluate campaign
state, and EpiloguePanel presents it. GenerationalSuccessionEngine provides
dynasty records, although its capture ordering should be stabilized. No active
Hall of Fame profile store was found.

Hall of Fame data must be metagame profile state, not a live campaign state
section. It must not be written into the campaign's core checksum envelope,
and loading or deleting a campaign must not mutate the archive accidentally.

### Core and host work

Define a CampaignCompletionRecord with:

- archive record ID;
- campaign or profile ID;
- colony display name validated for safe text;
- completion day and end condition;
- epilogue regional, demographic, and moral outcomes;
- living and deceased counts;
- selected survivor fate summaries;
- generational or dynasty summary;
- lifetime statistics selected by an explicit allowlist;
- milestone and achievement IDs;
- completion schema version and record checksum.

The campaign host creates the record only after the same authoritative
completion path that opens EpiloguePanel. It should pass the completed
EpilogueEvaluationContext or a validated campaign snapshot into the archive
builder. The archive must not recompute missing facts from UI labels.

Implement a versioned HallOfFameProfileStore through the existing IFileIO and
IJsonSerializer ports or a thin Godot adapter. Use an atomic write strategy,
checksum verification, deterministic ordering, and clear handling for
duplicate campaign IDs. Keep the archive separate from campaign save slots.

If the feature promises permanence, define it accurately as persistence in
the local user profile until explicit deletion or platform data loss. Do not
claim cloud or cross-device permanence without an external service contract.

Use GenerationalSuccessionEngine records for summary only. Do not keep live
references in the archive.

### Godot work

Create HallOfFamePanel.cs for the main menu or plaza with:

- sorted campaign cards;
- colony name, ending, day, and population summary;
- survivor and dynasty records;
- lifetime statistics and achievement badges;
- checksum or archive validity status;
- empty, corrupt, duplicate, and deleted-record states;
- accessibility-friendly text and scroll behavior.

The monument presentation may use granite, engraving, or CRT styling, but the
record remains a data-driven view.

### Data and persistence

Define the archive schema separately from campaign save schemas. Allow
versioned migration and reject future versions. Do not copy the entire
campaign save into the archive; store only an allowlisted summary.

If campaign names or survivor names can contain arbitrary user text, sanitize
or length-limit them for display and export. Avoid storing sensitive
information not required for the monument.

### Acceptance tests

- Completing a campaign creates exactly one archive record.
- Reopening the epilogue does not create another record.
- A failed or abandoned campaign is not marked complete unless policy says so.
- Loading an old campaign does not alter the archive.
- Deleting a campaign leaves its completed archive record intact unless the
  player explicitly deletes the archive record.
- A corrupted archive record is reported and does not crash the main menu.
- A mismatched checksum is rejected.
- Duplicate record IDs are handled deterministically.
- Archive sorting is stable across platforms and insertion orders.
- The record contains only allowlisted summary data.

### Adversarial review questions

- Is Hall of Fame state being written into every campaign save?
- Can a player create a monument entry by changing a UI label?
- Is the record actually persistent across save-slot deletion?
- Does the archive trust a campaign snapshot without validating completion?
- Can one corrupted record prevent all other campaigns from appearing?

### Done when

One officially completed campaign creates a checked, versioned, local profile
record that the Hall of Fame panel can render independently of the campaign
save and epilogue scene.

## Cross-system integration order

The following table is the intended dependency graph. A feature may be
developed in a branch, but it is not accepted into the playable build until
the upstream contract exists.

| Order | Slice | Upstream contracts | Downstream consumers |
|---:|---|---|---|
| 0 | Authority and ID ledger | Existing JSON, route graph, current saves | All Batch 6 work |
| 1 | Crossing infrastructure | LocationLayout, CrossingSession, VouchAccess, travel route API | 81, 90, caravan routes |
| 2 | Power distribution | Inventory, fuel, batteries, room loads, tick contract | 83, 85, 86, 91, 94 |
| 3 | Schedule event contract | IClock, ISimClock, DutyRoster, Needs | 93, 95, 90 |
| 4 | Disease airlock adapter | DiseaseSystem, MedicalSystem, shelter capacity | 91, 84 |
| 5 | Telegraph state | FactionRadioEngine, RadioSave, Journal | 82, 94, 96 evidence |
| 6 | Caravan contract | TravelingCaravan, DutyRoster, Expedition, economy | 90, 81 route effects |
| 7 | Perishable storage | Inventory, food/medical definitions, weather | 86, 88, 91 |
| 8 | Capability recovery | Injury authority, Medical, Crafting, Inventory | 84, expedition and duty |
| 9 | Companion state | Animal definitions, Needs/Caregiving, Expedition | 87, CombatTrauma grounding |
| 10 | Water source | BrineWater, Needs, Disease, pump/power | 88, greenhouse, medical |
| 11 | Paper documents | Crafting, Research, Journal, treaty/currency contracts | 89, 92, 96 |
| 12 | Recon intelligence | Research, LocationLayout, Weather, Expedition | 94, combat and map views |
| 13 | Private diary | Survivor state, Journal, Caregiving | 92 |
| 14 | Pneumatic queue | DutyRoster, Medical, schedule, AudioManager | 93 |
| 15 | Campaign archive | Epilogue, GenerationalSuccession, save/profile IO | 96 |

### State ownership rules

Use the following ownership map during implementation:

- CrossingInfrastructureSystem owns construction and bridge state.
  CrossingSession owns social crossing eligibility.
- PowerDistributionSystem owns production, allocation, storage, breakers, and
  outage results. Sources such as geothermal and lightning only contribute
  through it.
- DiseaseSystem owns disease stage, transmission, quarantine identity, and
  release. BiohazardAirlockSystem owns physical containment conditions.
- FactionRadioEngine or a focused TelegraphSystem owns radio and telegraph
  truth. RadioHostSession owns Godot input and audio.
- CraftingSystem owns generic timed craft transactions. Feature systems own
  only additional domain state that cannot be represented as a recipe.
- Inventory owns item quantity and equipment. Feature systems request atomic
  transactions and never maintain shadow counts.
- JournalSystem owns public discovery/codex records. SurvivorDiarySystem
  owns private entries and links to source events.
- TravelingCaravanSystem owns caravan movement and trade. Escort state owns
  contract reservations and outcomes, with one source of truth for the
  caravan's location and inventory.
- EpilogueMatrixRuntime owns ending evaluation. HallOfFameProfileStore owns
  completed campaign summaries.

### Event rules

Every new public state mutation should raise one event that includes:

- stable entity ID;
- event type;
- simulation day/hour or tick;
- state result;
- resource deltas;
- failure reason if applicable;
- event sequence or idempotency key.

Host sessions subscribe, update read models, play audio, mark the relevant
save section dirty, and request UI refresh. They must not re-run the command.

## Candidate file manifest

The paths below are candidates, not permission to create every file. The
implementer must confirm the existing location and namespace before adding
anything.

### Core candidates

- Assets/Ashfall.Core/Crossing/CrossingInfrastructureSystem.cs
- Assets/Ashfall.Core/Crossing/CrossingInfrastructureSaveState.cs, if the
  project keeps DTOs separate
- Assets/Ashfall.Core/Power/PowerDistributionSystem.cs
- Assets/Ashfall.Core/Power/GeothermalSourceSystem.cs
- Assets/Ashfall.Core/Power/GroundingArraySystem.cs
- Assets/Ashfall.Core/Radio/TelegraphSystem.cs, only if FactionRadioEngine
  cannot own the new state
- Assets/Ashfall.Core/Medical/ProstheticsSystem.cs
- Assets/Ashfall.Core/Storage/PerishableStorageSystem.cs
- Assets/Ashfall.Core/Companions/AnimalCompanionSystem.cs
- Assets/Ashfall.Core/Water/AquiferExtractionSystem.cs
- Assets/Ashfall.Core/Production/PaperMillSystem.cs, only for process state
  not already covered by CraftingSystem
- Assets/Ashfall.Core/Economy/CaravanEscortSystem.cs
- Assets/Ashfall.Core/Disease/BiohazardAirlockSystem.cs
- Assets/Ashfall.Core/Survivors/SurvivorDiarySystem.cs
- Assets/Ashfall.Core/Operations/PneumaticDispatchSystem.cs
- Assets/Ashfall.Core/Expedition/DroneReconSystem.cs
- Assets/Ashfall.Core/Clock/ChronometerSystem.cs
- Assets/Ashfall.Core/Endgame/CampaignCompletionRecord.cs
- Assets/Ashfall.Core/Endgame/HallOfFameProfileStore.cs, if profile IO is kept
  in Core ports; otherwise keep the store host-side and the DTO in Core

Prefer extending existing namespaces and systems when the ownership table
allows it. A new file is not a quality metric.

### Godot candidates

- src/UI/BridgeEngineeringPanel.cs
- src/UI/TelegraphKeyerModal.cs
- src/UI/PowerGridPanel.cs
- src/YearOfAsh/GeothermalHeatingWidget.cs, extension only if appropriate
- src/UI/ProstheticsBenchPanel.cs
- src/UI/IceHousePanel.cs
- src/UI/KennelPanel.cs
- src/UI/WellDrillingPanel.cs
- src/UI/PaperMillPanel.cs
- src/UI/CaravanEscortPanel.cs
- src/UI/BiohazardAirlockPanel.cs
- src/UI/SurvivorDiaryModal.cs
- src/UI/PneumaticDispatchWidget.cs
- src/UI/DroneReconPanel.cs
- src/UI/ClockTowerPanel.cs
- src/UI/HallOfFamePanel.cs
- src/Host/HallOfFameProfileStore.cs, if profile persistence is Godot-user
  storage

Host changes should remain thin: construct Core systems, bind callbacks,
translate Godot input to commands, subscribe to events, and present state.

### Data candidates

The names below are examples of bounded catalog groupings, not a requirement
to create sixteen new JSON files:

- infrastructure_routes.json or an extension to the canonical route catalog;
- power_sources.json and power_loads.json, if no existing power authority is
  found;
- telegraph_channels.json and telegraph_messages.json;
- prosthetics_definitions.json;
- storage_conditions.json;
- animal_companions.json;
- aquifer_sources.json;
- paper_processes.json;
- caravan_escort_contracts.json;
- biohazard_protocols.json;
- survivor_diary_templates.json;
- pneumatic_routes.json;
- drone_missions.json;
- chronometer_schedules.json;
- campaign_archive_policy.json, only if policy is data-driven.

Every candidate requires a current catalog loader, schema validation, ID
registration, and a test fixture. If an existing catalog already owns the
same definitions, extend that catalog instead.

## Test plan

### Core unit tests

Create or extend focused test classes rather than one large Batch6Tests file:

- CrossingInfrastructureSystemTests
- PowerDistributionSystemTests
- GeothermalSourceSystemTests
- GroundingArraySystemTests
- TelegraphSystemTests
- ProstheticsSystemTests
- PerishableStorageSystemTests
- AnimalCompanionSystemTests
- AquiferExtractionSystemTests
- PaperMillSystemTests
- CaravanEscortSystemTests
- BiohazardAirlockSystemTests
- SurvivorDiarySystemTests
- PneumaticDispatchSystemTests
- DroneReconSystemTests
- ChronometerSystemTests
- HallOfFameProfileStoreTests

Each class should cover:

- valid command;
- invalid ID;
- insufficient resource;
- capacity or exclusivity failure;
- event emission;
- idempotency;
- deterministic result;
- capture/restore;
- malformed restore data;
- stable ordering.

### Cross-system tests

At minimum, add integration coverage for:

1. A repaired bridge changes one real route but remains subject to VouchAccess
   and IceRoad gates.
2. Geothermal and lightning sources feed one power storage ledger and cannot
   overcharge it.
3. A power outage affects a biohazard airlock procedure without changing
   DiseaseSystem stage directly.
4. A telegraph contact creates one journal or location evidence record.
5. A caravan escort reserves a DutyRoster group and returns it on completion.
6. A prosthetic changes the same capability query used by an expedition or
   work assignment.
7. An aquifer source delivers water through the same quality path consumed by
   needs and disease.
8. A pneumatic message delivers a validated DutyRoster or medical request
   after its simulated delay.
9. A chronometer schedule event is consumed once by a real duty or meal
   handler.
10. A completed epilogue creates a Hall of Fame record without changing the
    campaign save checksum.

### Property and mutation tests

Where practical, use randomized generated inputs with a deterministic seed to
assert:

- quantities never become negative;
- capacity never becomes negative;
- output never exceeds source or storage limits;
- duplicate commands are idempotent;
- restoring a captured state is observationally equivalent to continuing;
- invalid catalog references never become valid through UI operations;
- event sequence numbers are monotonic per owning system.

Mutation review should deliberately:

- reorder dictionary insertion;
- omit optional save fields;
- send a duplicate event;
- retry a completed command;
- change a route or item ID to an unknown value;
- interrupt a process at every phase boundary;
- load a save immediately before and after a scheduled tick.

### Headless scenarios

Add small CLI scenarios only where they express real active behavior:

- crossing infrastructure self-test;
- power and geothermal self-test;
- telegraph deterministic exchange self-test;
- caravan escort self-test;
- biohazard airlock self-test;
- aquifer and storage self-test;
- chronometer boundary self-test;
- Hall of Fame archive self-test.

Do not add a self-test that constructs a system and asserts only that a panel
can be instantiated. A headless scenario must exercise commands, state, and
outcomes.

## Godot acceptance and accessibility

For each panel or modal, manually verify:

- the displayed numbers come from Core state after a command;
- blocked actions explain the first relevant reason;
- success, failure, delay, and restoration are visually distinct;
- focus order works with keyboard/controller navigation;
- every audio interaction has a text or visual equivalent;
- color is not the only signal for danger, disease, power, or purity;
- long survivor, route, and document names wrap safely;
- the panel remains usable at the project's 1920 by 1080 baseline and
  narrower window sizes;
- reopening a panel does not duplicate events or queue entries;
- closing a modal leaves simulation pause state correct.

UI smoke testing must use Godot headless where possible and a normal Godot
interactive run for input, focus, audio, and animation details. No Unity
editor, scene, or test path is part of this plan.

## Verification checklist

Run the canonical repository checks after each accepted slice and report the
result explicitly:

1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj
4. godot --headless --path . -- --data-integrity-selftest
5. godot --headless --path . -- --bridge-selftest

Run relevant domain scenarios as well:

- crossing self-test;
- caravan or traveling-caravan self-test;
- brine or salt-steam self-test when water or treaty data is touched;
- disease self-test when the airlock is touched;
- any new power, radio, clock, or archive headless scenario.

If a command is unavailable in the current host, record the exact failure and
do not replace it with a Unity command or an unverified local simulation.

## Targeted adversarial attack packet

Give the plan and implementation diff to an independent reviewer that sees
the specification and diff but not the implementer's reasoning. Ask the
reviewer to answer these questions with file and test evidence:

### Architecture attacks

1. Did any Core file import Godot, Unity, GodotSharp, UnityEngine, or
   JsonUtility?
2. Did any host panel become the owner of a quantity, timer, route modifier,
   disease stage, or campaign result?
3. Did a new system duplicate an existing authority instead of extending it?
4. Does the code reference a named class that is absent from the repository?
5. Were Unity legacy files or assets extended despite the Godot directive?

### Determinism attacks

6. Is there any System.Random, Guid.NewGuid, runtime HashCode, current wall
   clock, or unordered dictionary iteration in an authoritative path?
7. Are candidate lists, save lists, and tie breakers explicitly sorted?
8. Does loading at every process boundary produce the same outcome as an
   uninterrupted run?
9. Does duplicate input or event delivery double-spend resources?
10. Is audio or animation completion being mistaken for simulation completion?

### Resource and unit attacks

11. Can a failed transaction consume one cost before rejecting another?
12. Can any source create negative inventory, over-capacity storage, or
    energy without a source event?
13. Are kW, kWh, game hours, days, water volume, dose, temperature, and route
    distance represented with explicit conversions?
14. Are canonical item IDs used instead of display names?
15. Does a claimed permanent benefit have a bounded maintenance, capacity, or
    failure model?

### Feature-specific attacks

16. Can an unbuilt bridge alter a route?
17. Can a social vouch be mistaken for structural completion?
18. Can arbitrary Morse text create a faction, survivor, or location?
19. Can geothermal thermal flow be presented as electrical output?
20. Can lightning be harvested repeatedly from one weather event?
21. Can a prosthesis erase an injury or be equipped on two survivors?
22. Can an ice house preserve all item categories indefinitely?
23. Does a hound reveal every ambush or hidden node?
24. Can a failed aquifer sample feed clean water?
25. Can printing unlock unresearched technology?
26. Can an escort grant rewards without reserving guards or reaching a route
    endpoint?
27. Does the airlock cure or progress disease outside DiseaseSystem?
28. Can a diary be generated every tick or expose private content globally?
29. Can a pneumatic capsule mutate a roster instantly or twice?
30. Does drone imagery reveal evidence outside its sensor and confidence rules?
31. Did the chronometer create a third time source or duplicate a boundary
    event?
32. Can Hall of Fame records be created from an unfinished campaign or be
    lost when a campaign slot is deleted?

### Data attacks

33. Does every new cross-reference resolve through CatalogIntegrityValidator?
34. Are all new IDs snake_case and schema-versioned?
35. Does a fresh clone contain every new JSON file and fixture?
36. Can an orphan narrative catalog be mistaken for a runtime system?

The reviewer should reject the slice if any question is answered "yes" without
an explicit, tested design decision.

## Definition of done for Batch 6

Batch 6 is complete only when:

- all sixteen steps have an accepted scope decision;
- prerequisite contracts are implemented or formally deferred with a reason;
- every implemented feature has one Core authority and one Godot adapter;
- all stateful features capture and restore deterministically;
- new host stores use checksummed envelopes;
- no new Unity or JsonUtility dependency exists;
- canonical data is validated on a clean checkout;
- unit, integration, save, deterministic, and relevant headless tests pass;
- UI actions show real Core outcomes and remain accessible;
- the targeted independent review finds no unresolved architecture or
  duplicate-authority issue;
- user-owned dirty worktree changes remain intact;
- the handoff names the next one-system task rather than opening all remaining
  panels at once.

## Recommended execution prompts

### Prompt 1 — Phase 0 authority audit

Audit Batch 6 Steps 81–96 against the current repository without implementing
features. Produce:

- an owner map for route, power, water, food storage, injuries, animals,
  radio, clock, pneumatic dispatch, drones, and campaign archives;
- canonical IDs and units for every proposed resource;
- a list of active versus narrative-only catalogs;
- the minimum save sections and event contracts;
- a list of conflicts with current dirty files;
- the exact prerequisite slices needed before Steps 81–96.

Run the canonical read-only integrity checks where possible and report
PASS/FAIL. Do not create UI or add speculative JSON.

### Prompt 2 — first implementation slice

Implement only the smallest verified CrossingInfrastructureSystem slice for
one canonical bridge:

- Core state, commands, stable save DTO, and events;
- atomic material delivery;
- one route modifier consumed by the real travel authority;
- separate CrossingSession and VouchAccess checks;
- focused Core tests, save round-trip, and deterministic ordering;
- a thin Godot status adapter after Core tests pass.

Do not implement the full map, all bridges, power, or a new broad event bus.

### Prompt 3 — shared power prerequisite

If Phase 0 confirms there is no active power-grid authority, implement only a
minimal PowerDistributionSystem:

- source, load, storage, breaker, and outage contracts;
- explicit kW and kWh units;
- fuel and battery transaction paths;
- stable capture/restore and checksum integration;
- one geothermal source test fixture;
- no lightning UI, no full breaker screen, and no duplicate thermal model.

### Prompt 4 — independent targeted review

Review the implementation diff for the selected slice against this plan and
the AGENTS.md rules. Use the 36-question attack packet. Do not rewrite the
feature or expand scope. Report each finding with severity, file, line, test
evidence, and the smallest safe correction.

## Handoff summary

Batch 6 should produce a more credible ASHFALL because it makes infrastructure,
communication, illness control, capability recovery, and memory persistent in
the same cold material world. Its quality depends on resisting attractive
shortcuts: a vouch is not a bridge, a widget is not a system, a catalog is not
runtime behavior, and a completed animation is not a saved simulation result.
