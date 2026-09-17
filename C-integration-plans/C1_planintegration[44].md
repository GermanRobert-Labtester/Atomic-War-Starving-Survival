# C1 — Flagship Integration Plan [44]: Micro-Location Encounter UX, Turntable Recovery Broadcast, War-Economy Caravan Loop & 120-Day Campaign Fuzz Certification

> **Output:** `C1_planintegration[44].md`
>
> **Scope:** Tasks 13–16 — Plan 49 micro-location follow-up, Plans 02–09 turntable/vinyl lifecycle follow-up, unified war-economy recovery caravan integration, and automated multi-system long-campaign fuzzing/hardening.
>
> **Primary mission:** connect four late-stage systems into one deterministic, testable campaign slice without duplicating ownership. Micro-location encounters remain a presentation surface over canonical encounter resolution and cargo/injury systems; turntable playback becomes a bounded acoustic/psychological recovery input over real vinyl items and item wear; caravan missions compose vehicle, parole, espionage, preserved-food, expedition, combat, archive, foundry, mental-health, and save authorities; the fuzz harness certifies those integrations under 120-day save/load pressure with deterministic telemetry and bounded state growth.
>
> **Primary architecture rule:** every new bridge must translate between existing authorities rather than own a second truth. UI submits intents. Encounter resolution owns odds and outcomes. Inventory/cargo owns loot quantities and capacity. Mental health owns trauma/stress. Vehicle systems own vehicle condition/armor/cargo capability. Audio director owns acoustic intent; Godot audio runtime renders it. Turntable owns playback selection/state, not global morale. Item condition owns vinyl wear. Expedition/caravan orchestration owns mission lifecycle, not combat/food/espionage internals. The fuzz harness observes and perturbs systems but does not introduce production-only behavior.
>
> **Mandatory execution order:** P0 authority/repository audit → Task 13 micro-location UI/read-model + consequence bridges → Task 14 turntable/vinyl lifecycle + recovery effect boundary → Task 15 caravan mission composition + persistence → Task 16 deterministic fuzz harness + telemetry/performance gates → cross-system save/load → accessibility/localization → 45/120-day simulation → CI → SHIP/NO-SHIP.
>
> **High-risk corrections to raw tasks:**
> 1. Micro-location UI must **display canonical precomputed odds**, not locally calculate injury/loot odds from duplicated formulas.
> 2. Failed encounters should emit a **traumatic-exposure semantic event** to `SurvivorMentalHealthSystem`; do not invent “stress tokens” if the mental-health model uses trauma tokens, stress events, crisis inputs, or exposure records under different semantics.
> 3. A flatbed trailer increases **cargo/haul capacity**, not the micro-location’s generated loot. The encounter resolver creates the loot bundle; vehicle/cargo systems determine what can be transported.
> 4. Turntable playback should not blindly apply `-15% all daily stress` shelter-wide if stress already has multiple canonical modifiers. Use one bounded recovery/acoustic context through the mental-health/stress authority, with configuration and anti-stacking.
> 5. Album-specific therapy bonuses belong in recovery-action/turntable content data and must not hardcode `+25%` in UI/host code.
> 6. Vinyl wear belongs to canonical item condition/durability state. `TurntableSystem` may emit playback wear, but must not create a parallel `vinylWear` truth.
> 7. `AudioManager` and `ShelterAcousticDirector` must not become two competing audio authorities. The director owns intent/mix context; Godot audio/runtime renders.
> 8. Caravan missions should be an **orchestrator/state machine over existing vehicle, expedition, cargo, parole, espionage, food, combat, archive, foundry, mental-health, faction/trade systems**, not a second expedition engine.
> 9. The source 30% guide ambush reduction and mortar armor absorption values are **tuning candidates** until validated by balance tests.
> 10. “Bitwise Day-80 equality” should mean equality of **canonical deterministic state serialization/hash**, not process-memory bytes or nondeterministic dictionary ordering.
> 11. The 5-second 120-day benchmark should be a calibrated Release/nightly performance gate, not a brittle universal assertion on every developer/CI machine.
> 12. Economic-stability fuzzing should not assert “no starvation deaths or breakdowns.” Those may be valid outcomes. Assert bounded rates, deterministic causality, no impossible state, recoverability where intended, and no runaway systemic collapse caused by integration defects.

---

# 0. Global Authority Invariants

## Encounter / Micro-Location
- `MicroLocationModal` is presentation-only.
- canonical micro-location/encounter resolver owns choice availability, odds, RNG, injury result, loot generation, consequence IDs, and unresolved encounter state.
- inventory/cargo authority owns what loot physically enters expedition cargo.
- medical/injury authority owns actual injuries.
- mental-health authority owns traumatic exposure, trauma tokens, stress, crisis, and recovery.
- `VehicleGarageSystem` owns vehicle/trailer configuration and payload/cargo contribution.
- audio director owns encounter acoustic cue intents.

## Turntable / Vinyl
- canonical item catalog/inventory owns vinyl item identity/existence.
- item condition/durability system owns record wear/condition.
- `TurntableSystem` owns selected record, playback cycle/state, and diegetic turntable semantics.
- mental-health/stress authority owns any stress/recovery consequence.
- `ShelterAcousticDirector` owns shelter audio mix/cue intent.
- Godot `AudioManager`/runtime owns playback nodes/effects, not gameplay.
- save store persists only authoritative turntable state required for deterministic continuity.

## Caravan
- caravan mission orchestrator owns mission lifecycle and cross-system references.
- `VehicleGarageSystem` owns serviceability, modifications, armor, trailers, fuel/condition.
- crew/schedule authority owns survivor assignment.
- `ShelterPrisonerSystem` owns parole/captive status.
- `ShelterEspionageSystem` owns infiltrator/sleeper-agent truth and leak resolution.
- `FoodPreservationSystem` owns cured ration identity/shelf life.
- Inventory/cargo owns supplies and returned goods.
- Expedition/world route authority owns route/location.
- `ExpeditionCombatHandoff`/combat owns combat resolution.
- archive catalog/system owns archive items.
- foundry/item authority owns ingots/materials.
- mental health owns combat-shock consequences.
- faction/trade/economy systems own trade value/standing/economic consequences.

## Fuzzing
- fuzz harness uses public/test seams and canonical commands.
- fuzz RNG is explicitly seeded.
- production simulation behavior is identical with/without fuzz harness.
- normalized state serialization/hash is deterministic.
- telemetry is observational.
- no test-only “repair” mutates production state to make assertions pass.

---

# 1. Definition of Done

Plan [44] closes only when all four task families are proven together.

## Task 13
- all 25 micro-location IDs resolve;
- every choice has localized labels, requirement badges, and canonical risk/reward presentation;
- UI cannot roll its own injury/loot result;
- mental-health consequence uses semantic exposure;
- vehicle payload affects transport capacity rather than generated loot;
- optional audio failure is non-fatal;
- unresolved encounters save/load exactly;
- German/French coverage passes;
- all choice paths are deterministic under seed.

## Task 14
- all 8 vinyl item IDs exist and map to acquisition/lore definitions;
- turntable is wired to shelter-room/acoustic context;
- stress/recovery effect is bounded, data-driven, and applied exactly once through the canonical mental-health/stress authority;
- targeted album therapy modifiers are content-driven and recovery-action-aware;
- vinyl wear flows through item condition;
- cleaning consumes canonical purified alcohol;
- vinyl drops are seeded and reachable in intended micro-locations;
- playback persists deterministically without duplicating audio runtime state;
- `ShelterSocialPanel` mini-player is thin/presentation-only;
- save, scene binding, data integrity, and audio routing tests pass.

## Task 15
- caravan missions are data-driven and use existing route/vehicle/crew/cargo/combat authorities;
- serviced vehicle requirement is authoritative;
- paroled guide eligibility comes from prisoner/parole state;
- guide ambush modifier is bounded and data-driven;
- cured ration requirement uses real preserved-food items/shelf-life;
- infiltrator leakage is resolved by espionage authority;
- combat handoff uses vehicle armor through the vehicle-damage/armor contract;
- returned archives and ingots are canonical cargo items;
- combat shock enters mental health through semantic event;
- save section round-trips mission state with checksum/versioning;
- 45-day integration test covers dispatch → travel → leak/ambush → combat → return → cargo/trauma.

## Task 16
- deterministic 120-day fuzzer exists;
- seven-system randomized inputs are reproducible;
- Day 30 blackout and Day 60 raid+deep-freeze are explicit injected scenarios;
- save/reload every 10 days succeeds into fresh composition roots;
- normalized state hash on Day 80 matches uninterrupted run;
- no invalid collections, null refs, duplicate events, orphan state, or unbounded growth;
- final epilogue is valid;
- performance benchmark is Release/nightly and calibrated;
- telemetry artifact schema is versioned;
- all catalog/utilization gates remain clean after fuzz.

---

# 2. P0 — Repository & Authority Audit

Before implementation, inspect the actual current APIs.

## 2.1 Micro-location
- `src/UI/MicroLocationModal.cs`
- micro-location catalog/data files
- choice schema
- encounter resolver
- seeded RNG service
- injury/medical handoff
- loot/cargo/inventory handoff
- `VehicleGarageSystem`
- trailer/payload model
- `SurvivorMentalHealthSystem`
- audio cue catalog/director
- localization file and supported locale validation
- unresolved encounter persistence
- `CatalogIntegrityValidator`
- `--content-utilization-selftest`

## 2.2 Turntable
- all 8 vinyl item records in `Assets/StreamingAssets/Data/items.json`
- `VinylRecordAcquisitionMap`
- `TurntableSystem`
- `Main.UiPanels.cs`
- `ShelterSocialPanel`
- current stress/mental-health modifier contract
- psychological trauma recovery-action catalog
- item condition/wear system
- cleaning/consumable APIs
- purified alcohol item ID
- audio manager/runtime
- `ShelterAcousticDirector`
- turntable save store / save registry
- micro-location loot placement system

## 2.3 Caravan
- world/route/expedition system
- vehicle garage/serviceability/modification APIs
- passenger/crew assignment
- captive/parole state
- espionage/sleeper-agent state
- preserved-food ration catalogs
- expedition cargo/supply model
- combat handoff and vehicle damage contract
- faction/trade/economy reward contract
- prewar archive items/catalog
- foundry ingot items/catalog
- mental-health combat trauma input
- save hub/section envelope/checksum format

## 2.4 Fuzzer
- composition root for headless campaign
- existing deterministic clocks/RNGs
- save capture/restore APIs
- canonical state hash/serialization if present
- existing fuzz/property tests
- telemetry infrastructure
- performance benchmark conventions
- `CampaignEpilogueEngine`
- catalog/utilization test APIs
- CI scripts.

## 2.5 Publish authority matrix

Create:
`docs/architecture/C1_44_CROSS_SYSTEM_AUTHORITY_MATRIX.md`

Columns:
```text
fact
canonical owner
read API
command/write API
bridge role
persisted?
deterministic ID
status
```

Mandatory rows:
- micro-location choice;
- choice odds;
- injury;
- loot;
- cargo;
- traumatic exposure;
- audio cue;
- vinyl item;
- vinyl condition;
- cleaning;
- selected track;
- playback cycle;
- stress recovery modifier;
- therapy modifier;
- caravan mission;
- route;
- ambush risk;
- guide;
- sleeper-agent leak;
- cured ration stock;
- vehicle armor;
- returned archive;
- foundry ingot;
- combat shock;
- fuzz seed;
- save checkpoint;
- state hash;
- telemetry.

## 2.6 ADRs

Create/update:
- `ADR_MICRO_LOCATION_UI_VS_ENCOUNTER_RESOLUTION.md`
- `ADR_MICRO_LOCATION_LOOT_VS_VEHICLE_CARGO.md`
- `ADR_TURNTABLE_RECOVERY_EFFECT_BOUNDARY.md`
- `ADR_TURNTABLE_AUDIO_DIRECTOR_VS_AUDIO_RUNTIME.md`
- `ADR_CARAVAN_AS_COMPOSED_EXPEDITION_ORCHESTRATOR.md`
- `ADR_CARAVAN_GUIDE_AND_ESPIONAGE_RISK_STACK.md`
- `ADR_DETERMINISTIC_CAMPAIGN_STATE_HASHING.md`
- `ADR_LONG_CAMPAIGN_FUZZ_PERFORMANCE_GATE.md`

---

# 3. Task 13 — Micro-Location Encounter Visual Flow & Dialog Branching

## 3.1 Thin modal architecture

`MicroLocationModal.cs` should receive an immutable presentation model:

```text
MicroLocationEncounterPresentation
  encounter_id
  micro_location_id
  title_key
  description_key
  hazard_visual_key
  hazard_tags[]
  choices[]
  unresolved
  revision
```

Choice:

```text
MicroLocationChoicePresentation
  choice_id
  title_key
  description_key
  requirement_badges[]
  enabled
  disabled_reason_key optional
  injury_odds_permille
  loot_preview
  consequence_severity_band
  audio_cue_ids[]
```

The modal:
- renders;
- submits choice ID;
- waits for canonical result;
- never rolls RNG;
- never edits loot/injury/stress itself.

## 3.2 Environmental hazard illustrations

Use:
- stable illustration key;
- hazard tag;
- catalog/resource mapping.

Examples:
- radiation;
- unstable masonry;
- flooded basement;
- fire;
- chemical contamination;
- darkness;
- collapsed access.

No hardcoded file paths in modal callbacks.

Fallback:
- generic hazard illustration if optional art missing;
- missing art cannot block encounter.

## 3.3 Requirement badges

Badge types:
```text
tool
skill
trait
vehicle
protective_equipment
crew_count
time
```

Tool example:
- prybar;
- Geiger counter.

Skill example:
- scavenging >= N;
- mechanics >= N.

Rules:
- resolver owns whether requirement is met;
- UI gets reason from presentation;
- no duplicated inventory/skill lookup unless adapter is canonical resolver projection.

## 3.4 Risk/reward odds

Critical rule:
**UI displays odds computed by canonical encounter resolver.**

Do not duplicate:
```text
injuryChance = base - skill * ...
yield = base + ...
```
inside Godot.

Presentation should show:
- injury odds permille/percent;
- expected/possible scrap range;
- rare reward indicator if design supports;
- consequence severity.

If loot is not statistically disclosed by design:
- show qualitative bands instead.
- do not fake exact odds.

## 3.5 Mental-health consequence

On a failed/traumatic encounter:
```text
encounter resolver
→ semantic `traumatic_exposure`
→ SurvivorMentalHealthSystem
```

Payload:
```text
event_id
survivor_id
source_encounter_id
trauma_tags[]
severity
witnessed_injury_or_death refs
day
```

Mental health decides:
- stress;
- trauma token;
- floor change;
- crisis contribution.

UI does not “add stress token.”

## 3.6 Vehicle flatbed integration

Correct composition:

```text
micro-location resolver → generated loot bundle
vehicle/expedition cargo authority → transport capacity
flatbed trailer → payload/bulk capacity modifier
cargo allocator → what returns
```

The trailer should not increase RNG-generated scrap quantity unless a separate “access equipment enables extraction” rule explicitly exists.

If flatbed enables large-object recovery:
- model as requirement/transport eligibility;
- keep generated object identity canonical.

## 3.7 Consequence audio

Choice outcome can emit semantic cues:
- scavenge rustle;
- metal tear;
- collapsing masonry.

Use `ShelterAcousticDirector` or shared acoustic cue authority even if encounter is away from shelter only if director is designed as global acoustic intent. Otherwise use the canonical encounter audio director/host bridge and share cue catalog semantics.

Do not force shelter-only naming into world audio if architecture says otherwise.

Optional cue missing:
- warn/log;
- proceed narrative;
- no exception;
- no failed choice.

Production catalog integrity should still flag missing required cues.

## 3.8 German/French localization

Verify all 25 micro-location choice strings:
- title;
- choice labels;
- requirement text;
- consequence text;
- odds labels;
- disabled reasons.

Require:
- `en`;
- `de`;
- `fr`
coverage for all shipped keys.

Do not treat machine-translated placeholder as “verified” unless project policy permits.

Add localization matrix:
`docs/discovery/MICRO_LOCATION_L10N_MATRIX.md`.

## 3.9 Determinism

Seed identity:
```text
campaign_seed
+ expedition_id
+ micro_location_id
+ encounter_sequence
+ choice_id
```

Same input must yield same:
- loot bundle;
- injury roll;
- traumatic-exposure event;
- consequence branch.

Presentation/audio variation may differ only if explicitly presentation-seeded and non-gameplay.

## 3.10 Save/load unresolved encounter

Persist canonical:
```text
encounter_id
micro_location_id
resolved choice availability snapshot or source revision
selected-but-uncommitted intent if transaction model requires
RNG substream position / deterministic sequence key
resolved prerequisites if snapshot semantics require
```

Do not persist:
- Godot button hover;
- animation state;
- tooltip open state.

After restore:
- prompt content identical;
- enabled/disabled choice semantics identical unless canonical world state intentionally revalidates;
- no duplicate resolution.

## 3.11 Tests

Create:
`MicroLocationChoiceBranchTests.cs`

Coverage:
- all 25 micro-locations;
- every choice;
- requirement met/unmet;
- all deterministic result branches;
- loot;
- injury;
- traumatic exposure;
- flatbed cargo;
- optional audio missing;
- save/load unresolved;
- localization keys.

Use generated test cases from catalog, not 25 copy-pasted tests where possible.

## 3.12 Gates

Run:
```bash
python3 scripts/ci/scene-lint.py
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --data-integrity-selftest
```

Require:
- zero orphaned micro-location entries;
- all 25 IDs resolve;
- all choices reachable or explicitly gated/deferred.

Update:
`docs/discovery/MICRO_LOCATION_UI.md`
with wireframes, odds semantics, requirements, accessibility, fallback behavior.


---

# 4. Task 14 — Turntable Morale Broadcast & Vinyl Scavenging Lifecycle

## 4.1 Mission

Complete the diegetic vinyl/turntable loop so recovered records have material history, acoustic presence, wear, and bounded mental-recovery value without creating a second morale or item-condition system.

## 4.2 Inventory and acquisition audit

Inventory all eight vinyl item records in:
`Assets/StreamingAssets/Data/items.json`.

Verify for each:
- stable item ID;
- display/localization key;
- album/track lore reference;
- stackability;
- uniqueness;
- item condition support;
- acquisition sources;
- `VinylRecordAcquisitionMap` entry;
- audio asset/cue mapping;
- therapy tags if applicable.

Produce:
`docs/audio/VINYL_RECORD_AUTHORITY_MATRIX.md`

Columns:
```text
vinyl_item_id
display_key
lore_ref
audio_asset_ref
acquisition_locations[]
condition_supported
cleaning_recipe_or_item
therapy_tags[]
turntable_compatible
status
```

No duplicate “vinyl catalog” that can diverge from items.json.

## 4.3 Turntable ownership boundary

`TurntableSystem` owns:
- inserted/selected canonical vinyl item-instance ID;
- playback state;
- playback cycle count if needed for wear events;
- start/stop/pause semantics;
- room/broadcast scope;
- canonical current program/album ID;
- deterministic gameplay position if track position matters to effects.

It does not own:
- item existence;
- item condition;
- survivor stress;
- trauma recovery;
- actual Godot stream player;
- shelter machinery ducking;
- audio bus levels.

## 4.4 `Main.UiPanels.cs` integration

Register/bind turntable UI through the same panel lifecycle pattern as other shelter panels.

Room binding:
- verify physical turntable object/room authority;
- determine which rooms can hear playback;
- avoid hardcoding “all bunks” if acoustic reach/room graph exists.

If shelter audio currently has no spatial propagation:
- use a documented shelter-wide broadcast scope;
- do not invent fake per-room attenuation.

## 4.5 Morale/stress effect boundary

Raw requirement:
> playing a vinyl record reduces base daily stress accumulation by 15% across all bunks.

Flagship implementation:
- treat `15%` as a **content/balance candidate**, not UI hardcode;
- apply through one canonical mental-health/stress modifier interface;
- define source key such as `turntable_broadcast`;
- ensure it modifies **only the intended stress accumulation component**, not trauma Stress Floor, crisis duration, injury stress, or every stress source indiscriminately;
- cap stacking with other shelter-wide recovery broadcasts/comfort effects;
- stop effect immediately/at canonical boundary when playback ends or becomes inaudible;
- verify no double application from both `TurntableSystem` and `ShelterAcousticDirector`.

Suggested contract:
```text
ShelterRecoveryContext
  source_id
  eligible_survivor_ids or audience_scope
  stress_accumulation_multiplier
  trauma_recovery_modifiers[]
  start_tick
  end_condition
```

Canonical mental-health/stress authority consumes it.

## 4.6 Targeted album therapy bonuses

Raw example:
- classical symphony accelerates quiet-room recovery by 25%.

Implement as data:
```text
vinyl_therapy_effects[]
  vinyl_item_id
  compatible_recovery_action_tags[]
  recovery_rate_modifier_permille
  audience_scope
  room_or_audibility_requirements[]
  stacking_group
```

Rules:
- `25%` is tuning data;
- mental-health recovery engine owns resulting therapy rate;
- turntable merely exposes active album context;
- quiet-room survivor must actually be in audibility/broadcast scope;
- no album can reduce base Stress Floor directly;
- no therapy effect if playback unavailable/broken;
- stacking group prevents multiple turntables/albums from multiplying uncontrollably.

## 4.7 Vinyl wear

Use canonical item-condition system.

Flow:
```text
completed playback wear interval
→ TurntableSystem emits item-use observation
→ ItemConditionSystem applies wear
→ canonical vinyl instance condition changes
```

Do not persist separate:
`vinyl_wear`.

Define wear:
- per complete play cycle or normalized playback duration;
- deterministic;
- data-driven by material/condition profile.

Interrupted playback:
- proportional wear if intended;
or
- wear on committed intervals.

No reload exploit:
- playback position + wear checkpoint prevents replaying same clean interval.

## 4.8 Cleaning with purified alcohol

Verify canonical purified alcohol item ID and whether it is already consumed in another crafting/cleaning system.

Flow:
```text
clean vinyl intent
→ validate vinyl instance
→ validate condition/cleanable state
→ reserve purified alcohol
→ consume exactly once
→ ItemConditionSystem applies cleaning/restoration effect
→ emit semantic event
```

Guardrails:
- no cleaning above max;
- no duplicate consumption;
- no “purified alcohol” hardcoded string;
- cleaning cannot repair physical scratches unless item-condition model distinguishes contamination vs damage.

If item condition has only one scalar:
- document exactly what cleaning can restore;
- do not magically reverse all wear.

## 4.9 Vinyl scavenging placement

Add seeded placement to high-tier micro-locations:
- `loc_luxury_penthouse`;
- `loc_collapsed_music_conservatory`.

Verify IDs exist before editing.

Prefer loot table/content entries rather than special-case code.

Rules:
- seeded;
- rarity data-driven;
- unique record duplication policy explicit;
- acquisition map updated;
- content utilization proves reachability.

Flatbed/caravan cargo integration is unrelated to drop generation unless record counts as transport cargo normally.

## 4.10 Analog audio presentation

`ShelterAcousticDirector` owns intent:
- selected vinyl content;
- turntable bus target;
- machinery duck request;
- analog noise profile.

Godot runtime/AudioManager renders:
- actual track;
- surface noise;
- pop/crackle filter;
- bus effects.

Do not make `AudioManager` a second state owner.

Analog artifacts:
- presentation only;
- never alter therapy efficacy;
- deterministic enough for tests if needed;
- no per-frame allocations.

## 4.11 Machinery ducking

When turntable audible in living quarters:
- request a bounded duck on `Machinery`;
- define magnitude in audio tuning data;
- avoid fighting Task 7 high-priority event duck controller;
- turntable duck and event duck should compose through one mixer.

Priority:
1. critical alerts/events;
2. speech/dialogue if applicable;
3. turntable;
4. machinery/ambient bed.

## 4.12 ShelterSocialPanel mini-player

Display:
- spinning record art;
- localized track/album title;
- play/pause/stop;
- record condition;
- active recovery effect summary;
- audience scope;
- cleaning action if appropriate.

Animation:
- visual only;
- no gameplay timing from rotation.

The mini-player submits commands through a `TurntableUiAdapter` or equivalent.

## 4.13 Playback persistence

`VinylMoraleSaveStore` should persist only if it is the canonical/current turntable save owner and not duplicate an existing Turntable save section.

Suggested persisted state:
```text
schema_version
turntable_id
vinyl_item_instance_id
playback_state
gameplay_position_ticks
play_cycle_wear_checkpoint
active_broadcast_start_tick
processed_effect_interval_id
```

Do not persist:
- Godot audio stream handle;
- bus dB;
- random vinyl pop position;
- UI animation angle.

If needle position is purely cosmetic/audio:
- restoring approximate playback position is enough.
If stress/therapy effect depends on active listening duration:
- canonical gameplay position/tick must be exact.

## 4.14 Tests

`VinylTurntableMoraleTests.cs`:
- no record;
- record inserted;
- playback start;
- 15%-candidate stress modifier application exactly once;
- scope/audibility;
- targeted therapy effect;
- playback stop removes modifier;
- broken/worn record behavior;
- wear per cycle;
- partial play;
- cleaning transaction;
- save/load playback;
- no duplicate stress effect after restore;
- seeded record drop;
- machine duck request;
- no audio runtime dependency in Core.

Run:
```text
--data-integrity-selftest
--scene-binding-selftest
```

Update:
`docs/audio/TURNTABLE_BROADCAST_SYSTEM.md`
with effect stacking, record catalog, condition/wear, cleaning, audio routing, save semantics.

---

# 5. Task 15 — Cross-Pillar Master Integration: War Economy & Recovery Caravan

## 5.1 Mission

Build a mid/endgame caravan trade loop by composing existing vehicle, crew, prisoner/parole, espionage, food preservation, expedition, combat, archive, foundry, mental-health, economy, faction, and audio systems.

Do **not** create:
- second route engine;
- second cargo inventory;
- second vehicle damage model;
- second ambush RNG;
- second combat system;
- second captive status;
- second archive item registry.

## 5.2 Data file

Create:
`Assets/StreamingAssets/Data/caravan_trade_missions.json`

Suggested mission schema:

```text
mission_id
display_key
origin_id
destination_id
route_id
minimum_vehicle_tags[]
minimum_vehicle_condition
required_crew_min
required_roles[]
guide_policy
ration_requirements
cargo_capacity_requirement
trade_manifest_policy
ambush_risk_profile
espionage_exposure_profile
combat_handoff_profile
reward_table_id
archive_reward_tags[]
foundry_material_reward_tags[]
duration_model
faction_requirements[]
unlock_conditions[]
localization_keys[]
```

Mission data should reference canonical IDs.

## 5.3 Mission state machine

Recommended:

```text
DRAFT
→ VALIDATING
→ READY
→ DISPATCHING
→ OUTBOUND
→ AT_DESTINATION
→ RETURNING
→ AMBUSH/ENCOUNTER optional
→ ARRIVING
→ SETTLING_CARGO
→ COMPLETE
```

Exceptional:
```text
CANCELLED
FAILED
STRANDED
DESTROYED
```

State changes exactly once with stable mission event IDs.

## 5.4 Vehicle requirement

Dispatch requires:
- real vehicle instance;
- `VehicleGarageSystem` says serviceable;
- required modifications/armor/cargo capability;
- fuel/energy if modeled;
- not assigned elsewhere.

No `isServiced` duplicate in caravan save.

Store:
- vehicle instance ref;
- dispatch snapshot refs if needed.

Revalidate immediately before departure.

## 5.5 Crew assignment

Use canonical schedule/expedition assignment.

Crew eligibility:
- alive;
- available;
- not in conflicting duty/therapy;
- medically eligible;
- vehicle seat/capacity;
- required roles.

No caravan-local “crew busy” truth.

## 5.6 Paroled captive guide

Raw rule:
> paroled captives reduce ambush risk by 30%.

Correct flow:
- `ShelterPrisonerSystem` owns `paroled` status;
- caravan queries eligible guide candidates;
- guide must be free, willing/assigned under canonical rules, and know relevant route/region if such knowledge exists;
- mission records guide survivor/person ref;
- route-risk calculator consumes guide context.

Treat `30%` as default tuning data:
```text
guide_ambush_risk_multiplier = 0.70
```
not code constant.

Anti-exploit:
- multiple guides do not stack to zero risk unless explicitly designed;
- unknown/untrusted guide may introduce other risks only if supported;
- parole status cannot be granted by caravan system.

## 5.7 Preserved ration requirement

`FoodPreservationSystem` owns cured-ration identity and shelf life.

Dispatch validator computes:
```text
required_food_days
required_calories/rations
shelf_life_remaining_at_departure
expected_trip_duration
safety_margin
```

Only items that remain valid through intended consumption window qualify.

Reserve/transfer rations into canonical expedition/caravan cargo.

No caravan-local food count.

Consumption:
- expedition/supply system;
- spoilage remains FoodPreservation authority.

## 5.8 Espionage leak risk

Sleeper-agent truth belongs to `ShelterEspionageSystem`.

If a crew member is an infiltrator/sleeper agent:
- espionage system receives mission exposure opportunity;
- seeded espionage resolver decides whether route details leak;
- emits semantic `caravan_route_leaked` event;
- world/faction/encounter risk system consumes leak.

Caravan state stores:
- leak event ref;
not:
- duplicate spy identity truth.

No UI certainty unless espionage information is actually known to player.

## 5.9 Ambush risk composition

Create/reuse canonical risk projection:

```text
CaravanAmbushRiskContext
  route_base_risk
  faction_hostility
  route_intelligence
  guide_modifier
  leaked_route_modifier
  vehicle_signature_modifier
  escort_modifier
  weather_modifier
  time_modifier
```

Do not multiply arbitrary percentages from each subsystem without clamping/normalization.

Publish formula and order.

Example:
- paroled guide multiplier 0.70;
- leak may add/multiply risk;
- final risk clamped.

Use seeded encounter selection.

## 5.10 Ambush handoff

Use:
`ExpeditionCombatHandoff`

Caravan orchestrator passes:
- crew;
- vehicle;
- cargo;
- route location;
- enemy force;
- mission context;
- ambush surprise state.

Combat owns:
- hit resolution;
- injuries;
- deaths;
- enemy outcomes.

## 5.11 Vehicle armor vs mortar

Raw requirement:
> vehicle armor modifications absorb incoming mortar fire.

Correct architecture:
- combat produces vehicle-hit/damage event with damage type `mortar` or equivalent;
- vehicle armor/modification authority computes mitigation/absorption;
- vehicle damage/condition authority commits resulting damage;
- caravan reads outcome.

Do not subtract damage directly inside caravan mission state.

If mortar damage type/modifier does not exist:
- add it to canonical vehicle damage/armor data/model, not mission-specific branch.

## 5.12 Returned cargo

Potential rewards:
- high-value prewar archives;
- raw foundry ingots;
- trade goods.

Reward generation belongs to canonical mission/trade/loot table.

Return sequence:
```text
reward bundle generated
→ cargo capacity validates carried items
→ arrival
→ inventory transfer transaction
→ archive items registered/recognized
→ foundry ingots enter canonical inventory/material authority
```

No direct TechTree unlock from caravan return.
Archives must still go through archive/decryption system.

## 5.13 Crew combat shock

After intense firefight:
```text
combat result
→ traumatic_exposure event
→ SurvivorMentalHealthSystem
```

Payload can include:
- ambush intensity;
- casualties witnessed;
- vehicle destruction;
- close-call tags;
- survivor injury.

Mental health decides:
- stress;
- combat-shock trauma;
- crisis/floor consequences.

Do not hardcode `add trauma_combat_shock` in caravan code.

Quiet-room therapy remains downstream mental-health/schedule behavior.

## 5.14 Departure/arrival audio

Semantic events:
- `caravan_departed`;
- `caravan_arrived`;
- gate cycle if applicable.

Audio director maps:
- engine roar;
- gate klaxon.

Optional cue failure:
- non-fatal;
- integrity warning.

Audio never controls mission timing.

## 5.15 Save section

Use `SaveStoreHub` section key:
`caravan_trade_network`
only if no existing canonical section already owns caravan/expedition mission state.

Schema:
```text
schema_version
missions[]
next_mission_sequence
processed_event_ids[]
checksum_envelope/version
```

Persist mission refs:
- mission ID;
- state;
- vehicle ref;
- crew refs;
- guide ref;
- cargo ref/manifest IDs;
- route/progress;
- leak event ref;
- encounter/combat handoff ref;
- deterministic sequence state.

Do not duplicate:
- vehicle condition;
- captive parole state;
- spy identity;
- item condition;
- survivor trauma;
- inventory contents;
- faction trust.

## 5.16 Save/load boundaries

Save at:
- DRAFT;
- OUTBOUND;
- immediately before ambush;
- mid-combat only if combat save supports;
- RETURNING;
- ARRIVING;
- cargo settlement.

Exactly-once:
- ration reservation;
- departure;
- leak event;
- ambush selection;
- reward generation;
- cargo transfer;
- trauma event;
- arrival audio.

## 5.17 45-day integration test

Create:
`CaravanTradeMasterIntegrationTests.cs`

Scenario:
- vehicle initially unserviceable → repair;
- valid flatbed/armor vehicle;
- crew assignment;
- paroled guide;
- cured rations loaded;
- sleeper agent seeded on crew;
- dispatch;
- route progression;
- seeded leak;
- seeded ambush;
- combat handoff;
- mortar hit;
- vehicle armor mitigation;
- survivor traumatic exposure;
- return;
- archive + ingot cargo;
- inventory settlement;
- mental-health follow-up;
- save/load checkpoints.

Assertions:
- no duplicate cargo;
- no duplicate mission transition;
- deterministic ambush location;
- deterministic trade yield;
- deterministic trauma event;
- guide modifier applied once;
- espionage leak applied once;
- armor authority owns mitigation;
- archives remain locked/unprocessed until archive system;
- ingots usable by foundry authority.

## 5.18 Core engine-reference invariant

Source scan Core convoy implementation for:
- `UnityEngine`;
- `Godot`;
- Node/scene/audio types.

Expected:
`0`.

Host bridges handle:
- UI;
- audio;
- scene transitions.

## 5.19 Documentation

Create/update:
`docs/architecture/WAR_ECONOMY_CARAVAN_INTEGRATION.md`

Include:
- authority graph;
- mission state machine;
- risk formula;
- guide/spy effects;
- cargo ownership;
- combat handoff;
- mental-health handoff;
- save envelope;
- deterministic IDs;
- 45-day test trace.

---

# 6. Task 16 — Automated Multi-System Stress Fuzzing & Long-Campaign Telemetry Gate

## 6.1 Mission

Build a deterministic 120-day headless campaign fuzz harness that stresses the integrated campaign architecture without becoming a substitute for targeted unit/integration tests.

Systems:
1. Vehicle Garage
2. Espionage
3. Mental Health
4. Food
5. Captives
6. Archives
7. Foundry

Also observe:
- power;
- weather;
- raids/combat;
- economy;
- campaign epilogue;
- audio-event queues where Core emits them.

## 6.2 Harness location

Create:
`Ashfall.Core.Tests/Campaign/LongCampaignFuzzingTests.cs`

Use current test composition root.

Do not instantiate UI/Godot.

## 6.3 Seed model

Every fuzz case has:
```text
master_seed
scenario_id
day
subsystem_stream_id
action_sequence
```

Derive subsystem streams deterministically.

No shared mutable `Random` whose call order changes when unrelated code adds a roll.

Prefer:
- named RNG streams;
- deterministic split/fork API.

## 6.4 Daily input generation

Each day, generate bounded actions across seven systems.

Examples:

### Vehicle
- schedule repair;
- service vehicle;
- install/remove valid modification;
- dispatch/return;
- damage event.

### Espionage
- create/resolve intel;
- sleeper opportunity;
- leak opportunity;
- counterintelligence action.

### Mental Health
- trauma exposure;
- therapy;
- crisis;
- recovery action;
- quiet-room reservation.

### Food
- preserve;
- consume;
- spoil;
- ration convoy.

### Captives
- capture;
- release/parole;
- labor/guide assignment where legal system supports;
- escape/transfer if existing.

### Archives
- discover;
- solvent treatment;
- assign cryptographer;
- decrypt;
- unlock reward.

### Foundry
- ingest ingots;
- queue recipe;
- consume fuel/material;
- complete output.

Every generated action:
- must go through canonical command;
- expected validation failures are allowed and recorded;
- test must distinguish valid rejection from exception/corruption.

## 6.5 Injected crisis Day 30

Total power blackout.

Use canonical power command/event.

Verify:
- systems survive unavailable power;
- active jobs pause/fail according to contracts;
- no null refs;
- save/load still valid;
- audio Core can emit blackout context but headless ignores runtime.

## 6.6 Injected crisis Day 60

Simultaneous:
- exterior raid;
- deep-freeze storm.

Use canonical raid/weather injection seams.

Verify interactions:
- shelter power/thermal;
- vehicle availability;
- food;
- mental health;
- captive security;
- foundry work;
- archive work;
- schedule.

Do not directly mutate private fields.

## 6.7 Assertions every day

Check:
- no exceptions;
- all IDs unique;
- no negative inventory;
- no impossible item count;
- no duplicate survivor assignment;
- no missing survivor refs;
- no negative duration;
- no stress outside bounds;
- Stress Floor <= valid max;
- no vehicle condition outside range;
- no mission with impossible state transition;
- no unresolved combat handoff orphan;
- no duplicate archive reward;
- no foundry negative stock;
- no stale dead-drop/incident queue beyond retention policy;
- deterministic serialization succeeds.

## 6.8 Save/load every 10 days

On days:
10, 20, 30, ..., 120.

Procedure:
1. capture canonical save;
2. serialize;
3. create fresh composition root;
4. restore;
5. run reconciliation;
6. compare normalized state hash;
7. continue from restored root.

Never reuse in-memory system objects.

## 6.9 Day-80 uninterrupted comparison

Run paired:
- A: uninterrupted 1→80;
- B: save/reload every 10 days →80.

Compare:
- canonical normalized deterministic serialization/hash.

Do not compare:
- object reference addresses;
- dictionary enumeration order;
- transient cache;
- presentation state;
- wall-clock timestamp.

Normalize:
- stable collection ordering;
- culture-invariant numeric formatting;
- omit ephemeral caches;
- version envelope consistent.

## 6.10 Hash algorithm

Reuse existing canonical state hash if present.

Otherwise:
- deterministic UTF-8 canonical serialization;
- stable property order;
- sorted keyed collections;
- integer/fixed-point preferred;
- G9/G17 culture-invariant formatting only where unavoidable floating point exists;
- cryptographic or stable noncryptographic hash documented.

Never rely on default `GetHashCode()`.

## 6.11 Collection-growth telemetry

Track daily counts:
```text
dead_drops
audio_cue_events
incidents
processed_event_ids
quest_events
trauma_history
archive_events
caravan_events
vehicle_jobs
foundry_jobs
save_size_bytes
managed_memory_snapshot
```

Define:
- expected bounded/unbounded-by-design categories;
- retention/compaction policy;
- slope thresholds.

“No unbounded growth” means:
- collections with retention policy reach bounded plateau/linear-with-real-history expectation;
- not “all collections must stop growing.”

## 6.12 Economic stability

Do not assert zero starvation, zero breakdown, zero repair.

Instead define scenario metrics:
```text
food_days_remaining
starvation_deaths
mental_crises
recovery_rate
vehicle_operational_fraction
repair_backlog
archive_progress
foundry_throughput
caravan_profit/loss
```

Assertions:
- values remain finite/in-range;
- no runaway exploit (infinite profit/material);
- no deterministic deadlock with all recovery paths permanently impossible unless scenario truly exhausts resources;
- high-risk random seeds may fail strategically but state remains coherent.

Balance thresholds should be statistical/nightly, not brittle per-seed unless fixture-specific.

## 6.13 Day-120 epilogue

Run:
`CampaignEpilogueEngine`

Validate:
- non-null;
- stable deterministic output IDs/structure;
- no missing survivor/faction/location refs;
- no contradictory impossible outcomes from duplicated events;
- no duplicate major chronicle entries;
- all localization/content keys resolve.

Text exact-match is optional if prose generation changes; structural facts must match canonical campaign state.

## 6.14 Performance benchmark

Raw target:
`120-day simulation < 5.0 s Release`.

Treat as:
- Release-mode benchmark;
- on designated CI runner;
- warm-up;
- median/p95 across repeated runs;
- hardware/runtime metadata logged.

Fast PR CI:
- correctness only or looser budget.

Nightly:
- strict calibrated budget.

Suggested:
```text
median <= 5.0 s on reference runner
p95 <= 6.0 s
```
if baseline proves realistic.

Do not fail developers on unknown laptops because wall time differs.

## 6.15 Floating-point divergence

Preferred:
- integer arithmetic for permille, wear, stress, rates where existing architecture already supports it.

If float required:
- canonical serialization uses invariant G9 for float / G17 for double as appropriate;
- do not round internal simulation solely to satisfy hash;
- audit order-of-operations;
- avoid culture-sensitive parse/format.

## 6.16 CI script

Create:
`scripts/ci/run-campaign-fuzz.sh`

Modes:
```text
--fast
--nightly
--seed <n>
--seeds <n>
--days <n>
--telemetry <path>
```

Exit nonzero on:
- invariant failure;
- deterministic mismatch;
- integrity failure;
- reference-runner performance breach.

## 6.17 Telemetry artifact

Write:
`artifacts/long_campaign_telemetry.json`

Schema:
```text
schema_version
build_commit
seed
scenario
days
save_checkpoints[]
state_hashes[]
daily_metrics[]
collection_growth[]
economy_metrics[]
mental_health_metrics[]
vehicle_metrics[]
archive_metrics[]
foundry_metrics[]
caravan_metrics[]
crisis_events[]
performance
integrity_results
final_epilogue_summary
failure optional
```

Keep telemetry deterministic except:
- explicitly separate environment metadata.

Do not include process-specific timestamps inside canonical comparison payload.

## 6.18 Catalog/utilization after fuzz

Run after simulation:
- catalog integrity;
- content utilization;
- save contract validation;
- state hash.

Require zero integrity errors.

## 6.19 Architecture test map

Update:
`docs/architecture/ARCHITECTURE_TEST_MAP.md`

Add:
- 120-day fuzz certification;
- systems covered;
- seeds;
- checkpoints;
- hash method;
- performance runner;
- telemetry artifact;
- known exclusions.


---

# 7. Cross-System Mission Flow

The intended mid/endgame flow should be visible as one chain while ownership remains distributed:

```text
MICRO-LOCATION DISCOVERY
    │
    ├── encounter choice
    ├── requirement/skill check
    ├── injury/trauma exposure
    ├── vinyl/archive/material discovery
    └── vehicle cargo-capacity decision
    │
    ▼
SHELTER RECOVERY / PREPARATION
    │
    ├── treat injuries
    ├── quiet-room therapy
    ├── turntable recovery context
    ├── repair/service vehicle
    ├── cure long-life rations
    ├── parole/assign guide
    └── inspect intelligence risk
    │
    ▼
CARAVAN DISPATCH
    │
    ├── route
    ├── vehicle
    ├── crew
    ├── guide
    ├── preserved food
    ├── cargo
    └── espionage exposure
    │
    ▼
AMBUSH / TRADE / RETURN
    │
    ├── combat handoff
    ├── armor mitigation
    ├── trauma exposure
    ├── archive cargo
    ├── foundry ingots
    └── faction/economy result
    │
    ▼
ARCHIVE / FOUNDRY / RECOVERY
    │
    ├── archive decryption
    ├── research unlock
    ├── foundry production
    ├── mental-health recovery
    └── next mission
```

The fuzz harness certifies the chain under repeated save/load, crises, and randomized operations.

---

# 8. Micro-Location → Turntable Integration

Vinyl records added to micro-location loot must follow the same content route as every other item:

```text
micro-location reward table
→ seeded loot resolver
→ canonical item instance
→ expedition cargo
→ shelter inventory
→ VinylRecordAcquisitionMap/lore visibility
→ TurntableSystem eligibility
```

No:
```text
choice button → TurntableSystem.AddAlbum()
```

For unique vinyl:
- define duplication policy;
- duplicate discovery can become trade item, spare copy, or be suppressed by loot rules;
- do not silently delete canonical generated item.

For damaged vinyl:
- condition comes from canonical item instance;
- loot generation can seed initial condition using canonical item-generation rules.

---

# 9. Micro-Location → Caravan Cargo Integration

The flatbed trailer distinction is critical.

Correct:

```text
loot bundle = encounter truth
cargo capacity = vehicle truth
returned subset = cargo allocation truth
```

A flatbed can:
- increase bulk mass/volume limit;
- enable oversized-item transport;
- reduce forced abandonment.

It should not:
- generate extra scrap simply because it exists.

If designers want a “recover machinery” branch:
- make the flatbed a requirement badge;
- reward is a specific large cargo item generated by encounter resolver;
- cargo authority verifies transport.

---

# 10. Turntable → Mental Health Integration

The turntable is a shelter recovery context, not a stress owner.

Recommended canonical event/state:

```text
TurntableBroadcastContext
  turntable_id
  vinyl_item_instance_id
  album_id
  audience_scope
  start_tick
  active
  recovery_tags[]
  stress_accumulation_modifier_permille
  therapy_modifiers[]
  stacking_group
```

Mental-health/stress authority decides:
- which survivors hear it;
- which stress accumulation source it affects;
- whether crisis state blocks benefit;
- whether active therapy qualifies;
- how stacking is capped.

Never:
- reduce trauma Stress Floor by music alone;
- clear crisis because a track starts;
- apply a buff twice through both room and shelter scopes.

---

# 11. Vinyl Wear State Machine

```text
CLEAN / GOOD / WORN / DAMAGED
        │
        ├── playback use
        ├── contamination/dust if modeled
        └── physical damage if modeled
        │
        ▼
ItemConditionSystem
        │
        ├── cleaning can restore removable contamination
        └── irreversible wear remains if model distinguishes it
```

If item condition has only one scalar:
- define a conservative cleaning cap;
- avoid “purified alcohol restores a scratched record to new.”

Wear checkpoint identity:
```text
vinyl-wear:<item-instance>:<playback-cycle-or-interval>
```

This prevents:
- save/reload before track end;
- replay same interval;
- duplicate wear or zero-wear exploit.

---

# 12. Caravan Mission State Contract

Suggested runtime state:

```text
CaravanTradeMissionState
  mission_instance_id
  mission_definition_id
  phase
  route_id
  route_progress
  vehicle_instance_id
  crew_survivor_ids[]
  guide_id optional
  cargo_manifest_ref
  ration_manifest_ref
  departure_tick
  expected_return_tick optional
  espionage_exposure_ref optional
  route_leak_event_ref optional
  active_encounter_ref optional
  combat_handoff_ref optional
  reward_roll_ref optional
  arrival_settlement_ref optional
  processed_event_ids[]
```

Do not persist copies of:
- vehicle condition;
- vehicle armor values;
- survivor health;
- prisoner parole status;
- spy status;
- item stacks;
- trauma tokens;
- faction standing.

Re-query canonical state at appropriate boundaries.

---

# 13. Caravan Dispatch Validation Pipeline

Execute validation in deterministic order:

1. mission definition exists;
2. route exists/unlocked;
3. origin/destination valid;
4. vehicle exists;
5. vehicle serviceable;
6. vehicle not assigned elsewhere;
7. vehicle satisfies tags/modifications;
8. crew count valid;
9. crew eligibility valid;
10. guide eligibility valid if assigned;
11. required preserved ration quantity valid;
12. ration shelf life sufficient;
13. cargo reservation succeeds;
14. schedule reservations succeed;
15. mission/faction/trade prerequisites valid;
16. deterministic mission ID allocated;
17. departure state committed;
18. departure semantic event emitted.

If any precommit step fails:
- no partial mission;
- no ration loss;
- no crew stuck busy;
- no vehicle stuck assigned.

Use a transaction/orchestration result object.

---

# 14. Guide Risk Contract

A paroled captive guide is not a generic 30% magic shield.

Potential factors:
- route familiarity;
- faction knowledge;
- guide relationship/loyalty if modeled;
- guide health;
- destination;
- current intelligence.

MVP can retain:
```text
base guide risk multiplier = 0.70
```
but:
- data-driven;
- one guide contribution;
- clamped;
- only applies on routes for which guide qualifies.

Debug trace should show:
```text
route base risk
guide modifier
espionage leak modifier
faction modifier
weather modifier
final risk
```

---

# 15. Espionage Leak Contract

Sleeper agent logic remains invisible unless discovered.

Caravan orchestrator sends:
```text
MissionExposureOpportunity
  mission_id
  route_id
  destination_faction
  crew_ids
  departure_window
  security_context
```

Espionage authority returns:
```text
NoLeak
or
LeakEventRef
```

The mission then consumes only the consequence:
- risk modifier;
- ambush candidate;
- faction intelligence effect.

Never store:
```text
crewMember.IsSleeperAgent = true
```
inside caravan state.

Player UI must not reveal hidden sleeper identity because a mission risk calculation saw it internally.

---

# 16. Caravan Combat Handoff Contract

Input:
```text
CaravanCombatContext
  mission_id
  route_location
  crew_refs
  vehicle_ref
  cargo_ref
  enemy_force_ref
  surprise_state
  weather_context
```

Output:
```text
CaravanCombatResult
  combat_event_id
  survivor_outcomes[]
  vehicle_damage_event_refs[]
  cargo_loss_refs[]
  enemy_outcome
  traumatic_exposure_refs[]
  mission_continuation_state
```

Combat remains canonical.

Vehicle armor receives damage events.
Inventory/cargo receives cargo losses.
Mental health receives exposure events.
Mission decides continue/return/fail from resolved outcomes.

---

# 17. Archive Reward Boundary

Returned prewar archives are **items/content objects**, not immediate research.

Correct:

```text
caravan reward
→ archive item enters cargo
→ cargo settles to inventory/archive intake
→ archive system recognizes item
→ decryption/reconstruction
→ research/TechTree unlock later
```

This protects the entire Plan 62 loop.

---

# 18. Foundry Reward Boundary

Returned ingots:

```text
caravan reward
→ canonical material item
→ inventory
→ FoundrySystem recipe/input
```

Caravan does not:
- increment foundry stock counter separately;
- create “virtual ingots” outside inventory.

---

# 19. Trauma Boundary Across Tasks

Three potential trauma sources in this plan:
1. micro-location failure;
2. caravan ambush/firefight;
3. catastrophic fuzz scenarios.

All must use the same semantic event contract.

Suggested:

```text
TraumaticExposureEvent
  event_id
  survivor_id
  source_type
  source_ref
  severity_permille
  trauma_tags[]
  witnessed_casualty_refs[]
  injury_refs[]
  day
```

`SurvivorMentalHealthSystem` owns resolution.

No source system directly creates:
- combat-shock token;
- survivor-guilt token;
- Stress Floor delta;
- crisis.

---

# 20. Audio Boundary Across Tasks

Cue sources:
- micro-location rustle/metal/collapse;
- turntable playback;
- caravan engine/gate;
- archive radio/tape from previous plan;
- crisis ambience from previous plan.

All use shared semantic cue routing.

Suggested:
```text
AcousticCueRequest
  event_id
  cue_id
  source_context
  priority
  spatial_context optional
  optional
```

Rules:
- missing optional cue = log + no-op;
- missing required production cue = integrity failure;
- cue cannot block gameplay event;
- no gameplay waits for clip duration;
- headless no-op.

---

# 21. Save/Load Certification Matrix

Mandatory save checkpoints:

## Micro-location
- before modal opens;
- unresolved prompt;
- after requirement state changes;
- immediately before choice commit;
- after resolved choice.

## Turntable
- record inserted/stopped;
- playing mid-track;
- paused;
- immediately before wear checkpoint;
- active therapy broadcast.

## Caravan
- draft;
- dispatch validation;
- outbound;
- post-leak/pre-ambush;
- post-combat;
- return;
- cargo settlement.

## Fuzz
- every 10 campaign days.

For each:
- capture;
- restore into fresh composition root;
- compare canonical normalized hash;
- ensure no side-effect replay.

---

# 22. Deterministic Hash Contract

Canonical hash input must:
- exclude UI state;
- exclude audio runtime state;
- exclude cached derived collections unless they are canonical;
- exclude wall-clock timestamps;
- sort keyed collections;
- serialize IDs consistently;
- serialize integer permille exactly;
- serialize floats invariantly if unavoidable;
- normalize line endings if textual;
- use stable schema version.

Recommended layers:
```text
StateCanonicalizer
→ deterministic bytes
→ hash
```

Hash should be used for:
- Day-80 uninterrupted vs reloaded comparison;
- save roundtrip;
- paired simulation determinism.

Do not use runtime object `GetHashCode()`.

---

# 23. Fuzzer Action Model

Each randomized action returns:

```text
FuzzActionResult
  action_id
  subsystem
  command_type
  expected_validity
  accepted/rejected
  rejection_reason
  semantic_events[]
  state_hash_before
  state_hash_after
```

Expected validation rejections are not failures.

Failures:
- exception;
- invariant corruption;
- non-deterministic result;
- unexpected mutation on rejection;
- orphan reference;
- impossible state.

This distinction prevents fuzzing from becoming “all random commands must succeed.”

---

# 24. Fuzzer Crisis Injection Contract

## Day 30 — Blackout

Inject through canonical power system.

Capture before/after:
- power state;
- foundry jobs;
- archive jobs;
- vehicle charging/repair if powered;
- food storage if applicable;
- mental-health stress events;
- schedule changes;
- acoustic cue queue size.

## Day 60 — Raid + Deep Freeze

Inject through:
- raid authority;
- weather/thermal authority.

Observe:
- combat;
- thermal;
- power;
- food;
- captives/security;
- mental health;
- vehicle condition;
- foundry;
- archive;
- caravan missions;
- epilogue history.

No direct private-field mutation.

---

# 25. Telemetry Growth Gate

Classify collections:

### Must be bounded
- active audio cue requests;
- active dead-drop jobs;
- active incidents;
- pending schedule commands;
- active caravan encounters;
- active UI-independent presentation requests.

### May grow with real history but must compact
- processed event IDs;
- journal/chronicle;
- trauma history;
- archive discoveries;
- mission history.

For each:
- define retention;
- define compaction;
- report Day 30/60/90/120 count;
- report slope.

Flag:
- monotonic growth unrelated to real events;
- duplicate history;
- never-expiring pending state.

---

# 26. Economic Stability Gate

Measure but do not overconstrain.

Metrics:
- food reserve days;
- preserved-food stock;
- caravan trade margin;
- vehicle repair cost;
- vehicle operational percentage;
- foundry net material flow;
- archive reward cadence;
- mental-health crisis frequency;
- recovery completion rate;
- survivor losses.

Assertions:
- no NaN/infinite values;
- no negative impossible stock;
- no infinite profit loop;
- no free repair;
- no free preserved food;
- no duplicate archive reward;
- no permanent all-system deadlock caused by one transient event;
- loss states remain valid and explainable.

Nightly statistical thresholds can identify balance regressions across many seeds.

---

# 27. Epilogue Certification

At Day 120, validate the chronicle against campaign facts.

Examples:
- survivor recorded dead must not appear alive in final active roster;
- destroyed vehicle cannot be described as operational unless restored/rebuilt by real event;
- caravan route success must correspond to completed mission;
- archive breakthrough must correspond to actual unlock;
- major raid must appear at most once per source event;
- trauma/recovery statements must not contradict final mental-health state;
- faction/economy claims must derive from canonical state.

Prefer structural fact assertions over brittle exact prose strings.

---

# 28. Failure Injection Matrix

## F44.1 UI locally rolls injury odds
Expected: encounter authority gate fails.

## F44.2 flatbed multiplies generated scrap by 1.5
Expected: cargo-vs-loot authority gate fails unless explicit extraction rule is authored.

## F44.3 traumatic encounter directly creates a trauma token
Expected: mental-health authority gate fails.

## F44.4 missing optional rustle cue aborts encounter
Expected: optional-audio fallback fails.

## F44.5 unresolved encounter rerolls after save/load
Expected: determinism gate fails.

## F44.6 German choice key missing
Expected: localization coverage fails.

## F44.7 turntable applies stress reduction both in TurntableSystem and mental health
Expected: double-effect gate fails.

## F44.8 album directly lowers Stress Floor
Expected: mental-health authority gate fails.

## F44.9 vinyl wear stored in both item condition and VinylMoraleSaveStore
Expected: duplicate condition gate fails.

## F44.10 save/reload resets playback wear checkpoint
Expected: wear exploit gate fails.

## F44.11 purified alcohol consumed twice on cleaning retry
Expected: transaction idempotence fails.

## F44.12 two turntable buffs stack to 50%+ unintentionally
Expected: stacking-group gate fails.

## F44.13 caravan dispatch copies vehicle condition into mission and never refreshes
Expected: vehicle authority gate fails.

## F44.14 non-paroled prisoner assigned as guide
Expected: prisoner eligibility gate fails.

## F44.15 two guides stack 30% reductions multiplicatively without policy
Expected: risk-stack gate fails.

## F44.16 caravan reads sleeper-agent identity and exposes it in UI
Expected: information-boundary gate fails.

## F44.17 caravan code subtracts mortar damage directly from vehicle HP
Expected: vehicle armor authority gate fails.

## F44.18 archive reward instantly unlocks technology
Expected: archive/research boundary fails.

## F44.19 caravan gives ingots to a parallel foundry counter
Expected: inventory/material authority fails.

## F44.20 crew shock directly adds `trauma_combat_shock`
Expected: mental-health boundary fails.

## F44.21 save load replays cargo settlement
Expected: exactly-once gate fails.

## F44.22 Core caravan references `Godot.Node`
Expected: engine-reference invariant fails.

## F44.23 fuzzer uses unseeded `Random`
Expected: deterministic fuzzer gate fails.

## F44.24 save/reloaded run compares dictionary order and falsely diverges
Expected: canonicalization test exposes hash defect.

## F44.25 fuzz action rejection mutates inventory anyway
Expected: rejection atomicity fails.

## F44.26 Day 30 blackout injected by private-field mutation
Expected: public-command harness gate fails.

## F44.27 telemetry collection grows one item every tick forever
Expected: collection-growth gate fails.

## F44.28 test asserts zero starvation deaths and fails a legitimate harsh seed
Expected: balance-test semantics review fails.

## F44.29 performance test fails on slow laptop despite correct reference-runner baseline
Expected: benchmark policy gate fails.

## F44.30 culture changes float serialization hash
Expected: culture-invariant determinism test fails.

---

# 29. Performance Budgets

## Micro-location
- presentation construction <1 ms after warmup for one encounter;
- no catalog parse on modal refresh;
- no per-frame odds recalculation;
- hazard art/icons cached;
- no loot simulation in UI.

## Turntable
- playback effect calculation event/day-boundary driven;
- no per-frame stress iteration;
- vinyl wear checkpoint bounded;
- audio rendering handled by shared bridge;
- mini-player animation may run per-frame but must not allocate.

## Caravan
- dispatch validation O(crew + manifest), not all survivors × all routes;
- route risk evaluation bounded;
- mission update O(active missions);
- no repeated full cargo catalog scans;
- index mission definitions/routes.

## Fuzzer
- 120-day run benchmarked in Release;
- telemetry writing buffered;
- canonical hash efficient enough for 12 checkpoint restores;
- nightly performance budget separated from functional PR gate.

---

# 30. Accessibility & Localization

## Micro-location modal
- requirement badge includes text/icon;
- tool missing state not color-only;
- odds available as text;
- injury vs reward labels unambiguous;
- choice buttons keyboard/controller accessible;
- hazard illustration has alt/accessible description if framework supports;
- font scale verified by scene lint;
- German/French strings verified.

## Turntable
- spinning art has reduced-motion option;
- track title accessible;
- play/pause labels not icon-only;
- active recovery effect described textually;
- vinyl condition text available;
- audio-only radiation/turntable benefits have visual equivalents where gameplay relevant.

## Caravan
- mission readiness explains blockers;
- guide modifier shown only if player is allowed to know it;
- hidden espionage state never leaked;
- ration-duration warning textual;
- vehicle readiness accessible;
- combat-shock aftermath links to mental-health UI.

## Fuzz
- no player UI; telemetry keys stable and machine-readable.

---

# 31. Documentation Deliverables

Create/update:
- `docs/discovery/MICRO_LOCATION_UI.md`
- `docs/discovery/MICRO_LOCATION_L10N_MATRIX.md`
- `docs/audio/TURNTABLE_BROADCAST_SYSTEM.md`
- `docs/audio/VINYL_RECORD_AUTHORITY_MATRIX.md`
- `docs/architecture/WAR_ECONOMY_CARAVAN_INTEGRATION.md`
- `docs/architecture/ARCHITECTURE_TEST_MAP.md`
- `docs/architecture/C1_44_CROSS_SYSTEM_AUTHORITY_MATRIX.md`
- ADRs from P0.
- `artifacts/long_campaign_telemetry.json` generated by fuzz runs, not committed if artifacts policy excludes runtime outputs.

---

# 32. Recommended Commit Breakdown

## Task 13 — Micro-location
```text
44A-01 micro-location authority audit
44A-02 encounter presentation DTO/adapter
44A-03 hazard illustration mapping
44A-04 requirement badge model
44A-05 canonical odds projection
44A-06 traumatic-exposure mental-health bridge
44A-07 flatbed cargo-capacity bridge
44A-08 consequence audio requests
44A-09 optional cue fallback
44A-10 DE/FR localization completion
44A-11 unresolved encounter persistence
44A-12 all-choice generated branch tests
44A-13 catalog/utilization gates
44A-14 scene lint/accessibility
44A-15 MICRO_LOCATION_UI docs
```

## Task 14 — Turntable
```text
44B-01 eight-vinyl inventory audit
44B-02 acquisition/lore matrix
44B-03 TurntableSystem authority cleanup
44B-04 room/broadcast binding
44B-05 recovery-context adapter
44B-06 15%-candidate tuning data
44B-07 album-specific therapy modifiers
44B-08 item-condition wear handoff
44B-09 wear checkpoint/idempotence
44B-10 purified-alcohol cleaning transaction
44B-11 high-tier micro-location vinyl placement
44B-12 acoustic-director playback context
44B-13 machinery duck composition
44B-14 ShelterSocialPanel mini-player
44B-15 save-store persistence
44B-16 morale/recovery/wear tests
44B-17 data-integrity/scene-binding
44B-18 docs
```

## Task 15 — Caravan
```text
44C-01 authority audit/ADR
44C-02 mission catalog schema
44C-03 loader/integrity
44C-04 mission state machine
44C-05 vehicle serviceability bridge
44C-06 crew assignment bridge
44C-07 paroled-guide eligibility
44C-08 guide risk tuning
44C-09 preserved ration validator
44C-10 cargo reservation
44C-11 espionage exposure opportunity
44C-12 route leak consequence
44C-13 ambush risk projection
44C-14 ExpeditionCombatHandoff
44C-15 vehicle mortar/armor canonical damage path
44C-16 archive/ingot reward tables
44C-17 arrival cargo settlement
44C-18 traumatic-exposure bridge
44C-19 departure/arrival acoustic cues
44C-20 SaveStoreHub section
44C-21 save/reload idempotence
44C-22 45-day integration test
44C-23 zero-engine-reference gate
44C-24 docs
```

## Task 16 — Fuzzing
```text
44D-01 fuzz composition root
44D-02 named deterministic RNG streams
44D-03 seven-system action generators
44D-04 rejection atomicity
44D-05 daily invariant validator
44D-06 Day-30 blackout injector
44D-07 Day-60 raid+deep-freeze injector
44D-08 save/reload every 10 days
44D-09 canonical state serializer
44D-10 deterministic hash
44D-11 Day-80 paired equality
44D-12 collection-growth telemetry
44D-13 economy/recovery metrics
44D-14 Day-120 epilogue validation
44D-15 Release benchmark
44D-16 invariant numeric formatting
44D-17 run-campaign-fuzz.sh
44D-18 telemetry JSON writer
44D-19 post-fuzz integrity/utilization
44D-20 ARCHITECTURE_TEST_MAP sign-off
```

## Final cross-system hardening
```text
44E-01 shared traumatic-exposure contract
44E-02 shared acoustic-cue contract
44E-03 shared schedule/assignment audit
44E-04 cross-system save-order audit
44E-05 exactly-once event matrix
44E-06 content reachability
44E-07 deterministic fingerprints
44E-08 45/120-day combined runs
44E-09 performance/allocation report
44E-10 final SHIP/NO-SHIP report
```

---

# 33. Exhaustive Acceptance Checklist

## Task 13 — Micro-location UI

- [ ] `MicroLocationModal.cs` audited
- [ ] micro-location resolver identified
- [ ] choice schema identified
- [ ] canonical RNG identified
- [ ] canonical injury owner identified
- [ ] canonical loot owner identified
- [ ] cargo authority identified
- [ ] mental-health trauma input identified
- [ ] hazard illustration key added
- [ ] generic hazard-art fallback
- [ ] art failure non-fatal
- [ ] choice presentation DTO
- [ ] modal remains presentation-only
- [ ] tool requirement badge
- [ ] skill requirement badge
- [ ] trait requirement badge if real
- [ ] protective-equipment badge if real
- [ ] vehicle requirement badge if real
- [ ] disabled reason canonical
- [ ] odds read from resolver
- [ ] no UI-local injury formula
- [ ] no UI-local loot formula
- [ ] injury odds clear
- [ ] scrap/reward preview clear
- [ ] qualitative fallback if exact odds hidden
- [ ] traumatic failure emits semantic exposure
- [ ] mental health decides trauma/stress
- [ ] no direct stress token creation
- [ ] flatbed reads canonical vehicle configuration
- [ ] flatbed affects cargo capacity
- [ ] flatbed does not silently multiply generated loot
- [ ] oversized extraction uses explicit requirement if added
- [ ] scavenge rustle cue
- [ ] metal tear cue
- [ ] collapsing masonry cue
- [ ] optional missing audio non-fatal
- [ ] production required cue integrity
- [ ] German strings complete
- [ ] French strings complete
- [ ] all 25 micro-location IDs validate
- [ ] every choice tested
- [ ] every requirement path tested
- [ ] deterministic loot
- [ ] deterministic injury
- [ ] deterministic trauma event
- [ ] unresolved encounter saves
- [ ] unresolved encounter restores
- [ ] restore does not reroll
- [ ] scene lint
- [ ] content utilization zero orphaned entries
- [ ] UI wireframes updated
- [ ] accessibility verified

## Task 14 — Turntable

- [ ] all 8 vinyl item IDs inventoried
- [ ] all 8 map to acquisition lore
- [ ] all 8 item IDs canonical
- [ ] audio refs resolved
- [ ] item condition support verified
- [ ] TurntableSystem authority documented
- [ ] playback room/broadcast scope documented
- [ ] `Main.UiPanels.cs` binding
- [ ] no duplicate audio state owner
- [ ] stress/recovery modifier goes through canonical authority
- [ ] source 15% is data/tuning
- [ ] stress modifier applies once
- [ ] modifier affects intended base accumulation only
- [ ] no Stress Floor reduction
- [ ] no crisis clear from playback
- [ ] audience scope correct
- [ ] stacking group defined
- [ ] album therapy effects data-driven
- [ ] classical/targeted effect requires compatible recovery action
- [ ] source 25% is tuning data
- [ ] item condition owns wear
- [ ] Turntable emits use/wear observation
- [ ] wear deterministic
- [ ] partial playback wear policy defined
- [ ] wear checkpoint survives save
- [ ] no reload wear exploit
- [ ] purified alcohol canonical ID verified
- [ ] cleaning transaction atomic
- [ ] cleaning does not repair unsupported scratch damage
- [ ] penthouse location ID verified
- [ ] conservatory location ID verified
- [ ] seeded vinyl placement
- [ ] uniqueness/duplicates policy
- [ ] acquisition map updated
- [ ] AudioManager is rendering-only
- [ ] acoustic director owns intent
- [ ] analog surface noise presentation-only
- [ ] vinyl pop filter presentation-only
- [ ] turntable machinery duck uses shared mixer
- [ ] critical Events priority preserved
- [ ] ShelterSocialPanel mini-player
- [ ] spinning art reduced-motion safe
- [ ] track title localized
- [ ] condition visible
- [ ] active recovery effect visible
- [ ] playback commands adapter-driven
- [ ] save active canonical track
- [ ] gameplay playback position persisted if required
- [ ] no audio stream handle persisted
- [ ] no bus dB persisted
- [ ] no random pop state persisted
- [ ] `VinylTurntableMoraleTests.cs`
- [ ] deterministic vinyl drop
- [ ] data-integrity selftest
- [ ] scene-binding selftest
- [ ] docs updated

## Task 15 — Caravan

- [ ] caravan mission catalog created
- [ ] schema versioned
- [ ] mission IDs stable
- [ ] route refs valid
- [ ] vehicle tag refs valid
- [ ] crew role refs valid
- [ ] ration refs valid
- [ ] reward tables valid
- [ ] faction refs valid
- [ ] localization keys valid
- [ ] mission state machine implemented
- [ ] invalid transition blocked
- [ ] serviced vehicle required
- [ ] vehicle condition not duplicated
- [ ] vehicle assignment canonical
- [ ] crew assignment canonical
- [ ] crew health eligibility
- [ ] crew schedule exclusivity
- [ ] paroled guide status canonical
- [ ] non-paroled captive rejected
- [ ] guide route qualification if supported
- [ ] 30% risk reduction data-driven
- [ ] guide effect applies once
- [ ] multi-guide stacking prevented unless designed
- [ ] cured ration identity canonical
- [ ] shelf-life check
- [ ] trip duration check
- [ ] ration safety margin
- [ ] cargo reservation
- [ ] ration consumption canonical
- [ ] spoilage remains FoodPreservation-owned
- [ ] sleeper-agent truth stays Espionage-owned
- [ ] mission exposure opportunity
- [ ] leak deterministic
- [ ] leak event stable
- [ ] hidden spy identity not leaked to UI
- [ ] ambush risk composition documented
- [ ] risk clamped
- [ ] ambush location seeded
- [ ] combat uses `ExpeditionCombatHandoff`
- [ ] combat owns casualties/injuries
- [ ] vehicle armor owns mortar mitigation
- [ ] caravan does not subtract vehicle HP
- [ ] returned archive canonical
- [ ] returned ingot canonical
- [ ] archive not auto-unlocked
- [ ] foundry ingot not virtual duplicate
- [ ] combat shock semantic exposure
- [ ] mental health decides token/stress
- [ ] engine-roar cue
- [ ] gate-klaxon cue
- [ ] audio does not control timing
- [ ] `caravan_trade_network` key checked for collision
- [ ] checksum/version envelope
- [ ] processed IDs
- [ ] no duplicate cargo settlement
- [ ] no duplicate departure
- [ ] no duplicate leak
- [ ] no duplicate ambush
- [ ] no duplicate reward roll
- [ ] no duplicate trauma event
- [ ] 45-day integration test
- [ ] paired deterministic run
- [ ] trade yield identical
- [ ] ambush location identical
- [ ] trauma event identical
- [ ] zero `UnityEngine` references in Core convoy
- [ ] zero `Godot` references in Core convoy
- [ ] full Core tests pass
- [ ] architecture doc updated

## Task 16 — Fuzzing

- [ ] `LongCampaignFuzzingTests.cs`
- [ ] headless composition root
- [ ] master seed
- [ ] named subsystem RNG streams
- [ ] deterministic action ordering
- [ ] Vehicle Garage action generator
- [ ] Espionage action generator
- [ ] Mental Health action generator
- [ ] Food action generator
- [ ] Captives action generator
- [ ] Archives action generator
- [ ] Foundry action generator
- [ ] validation rejection recorded
- [ ] rejected action cannot mutate state
- [ ] Day 30 blackout via canonical API
- [ ] Day 60 raid via canonical API
- [ ] Day 60 deep-freeze via canonical API
- [ ] zero null refs
- [ ] zero illegal collection mutation
- [ ] zero negative inventory
- [ ] zero duplicate assignment
- [ ] zero orphan survivor ref
- [ ] zero invalid duration
- [ ] stress bounds valid
- [ ] Stress Floor bounds valid
- [ ] vehicle condition bounds valid
- [ ] mission state transitions valid
- [ ] combat handoff refs resolved
- [ ] archive rewards exactly once
- [ ] foundry stock valid
- [ ] save every 10 days
- [ ] restore into fresh systems
- [ ] post-restore reconciliation
- [ ] normalized hash after each restore
- [ ] uninterrupted Day 80 run
- [ ] reload-cycling Day 80 run
- [ ] Day 80 hash equality
- [ ] canonical serializer stable order
- [ ] no default `GetHashCode`
- [ ] culture-invariant numeric serialization
- [ ] integer/fixed-point retained where possible
- [ ] active cue growth bounded
- [ ] dead-drop growth bounded
- [ ] incident retention bounded
- [ ] processed IDs compacted/justified
- [ ] save-size growth monitored
- [ ] economic metrics finite
- [ ] no infinite profit
- [ ] no free repair
- [ ] no free food
- [ ] starvation outcomes treated statistically
- [ ] breakdown outcomes treated statistically
- [ ] vehicle repair outcomes treated statistically
- [ ] Day 120 epilogue generated
- [ ] epilogue refs valid
- [ ] no impossible contradictions
- [ ] Release benchmark
- [ ] reference CI runner metadata
- [ ] warmup
- [ ] median recorded
- [ ] p95 recorded
- [ ] 5s target calibrated
- [ ] PR gate not hardware-brittle
- [ ] G9/G17 use audited where floats remain
- [ ] `run-campaign-fuzz.sh`
- [ ] fast mode
- [ ] nightly mode
- [ ] seed override
- [ ] days override
- [ ] telemetry path override
- [ ] telemetry schema version
- [ ] telemetry state hashes
- [ ] telemetry daily metrics
- [ ] telemetry collection growth
- [ ] telemetry economy metrics
- [ ] telemetry performance
- [ ] post-fuzz catalog integrity
- [ ] post-fuzz utilization gate
- [ ] `ARCHITECTURE_TEST_MAP.md` sign-off

## Cross-system

- [ ] one traumatic-exposure contract
- [ ] one acoustic cue request contract
- [ ] one schedule/assignment authority
- [ ] one item/inventory authority
- [ ] one mental-health authority
- [ ] one vehicle authority
- [ ] one expedition/combat authority
- [ ] one archive/research path
- [ ] one foundry material path
- [ ] restore side-effect free
- [ ] stable IDs
- [ ] deterministic hash
- [ ] no engine refs in Core integration
- [ ] accessibility
- [ ] localization
- [ ] scene lint
- [ ] data integrity
- [ ] content utilization
- [ ] verify-fast

---

# 34. CI / Verification Commands

Run current equivalents of:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj

dotnet test Ashfall.Core.Tests --filter MicroLocationChoiceBranchTests
dotnet test Ashfall.Core.Tests --filter VinylTurntableMoraleTests
dotnet test Ashfall.Core.Tests --filter CaravanTradeMasterIntegrationTests
dotnet test Ashfall.Core.Tests --filter LongCampaignFuzzingTests

godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --scene-binding-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --real-campaign-journey-selftest

python3 scripts/ci/scene-lint.py
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/run-campaign-fuzz.sh --fast
bash scripts/ci/verify-fast.sh
```

Nightly/reference:
```bash
bash scripts/ci/run-campaign-fuzz.sh --nightly
```

Capture:
- command;
- commit;
- runtime;
- seed set;
- telemetry artifact;
- pass/fail.

---

# 35. SHIP / NO-SHIP Gate

**SHIP** only if:

```text
micro_location_choice_authorities == 1
AND micro_location_rng_authorities == 1
AND injury_authorities == 1
AND loot_authorities == 1
AND cargo_authorities == 1
AND mental_health_authorities == 1
AND vehicle_authorities == 1
AND vinyl_item_authorities == 1
AND item_condition_authorities == 1
AND turntable_authorities == 1
AND acoustic_intent_authorities == 1
AND caravan_mission_authorities == 1
AND prisoner_parole_authorities == 1
AND espionage_authorities == 1
AND food_preservation_authorities == 1
AND expedition_combat_authorities == 1
AND archive_authorities == 1
AND foundry_material_authorities == 1
AND research_tech_unlock_authorities == 1

AND micro_location_ui_rng_rolls == 0
AND micro_location_ui_direct_injury_mutations == 0
AND micro_location_ui_direct_loot_mutations == 0
AND micro_location_direct_trauma_token_mutations == 0
AND flatbed_direct_loot_generation_multipliers == 0
AND unresolved_encounter_reload_rerolls == 0
AND orphaned_micro_location_entries == 0
AND missing_required_micro_location_localizations == 0

AND turntable_direct_stress_state_mutations == 0
AND turntable_direct_stress_floor_mutations == 0
AND duplicate_turntable_recovery_effects == 0
AND duplicate_vinyl_condition_state == 0
AND vinyl_wear_reload_exploits == 0
AND cleaning_double_consumption_paths == 0
AND unseeded_vinyl_drop_paths == 0
AND duplicate_audio_state_authorities == 0

AND caravan_duplicate_vehicle_state == 0
AND caravan_duplicate_prisoner_state == 0
AND caravan_duplicate_spy_state == 0
AND caravan_duplicate_inventory_state == 0
AND caravan_duplicate_trauma_state == 0
AND non_paroled_guides_dispatched == 0
AND unintended_multi_guide_risk_stacking == 0
AND hidden_sleeper_identity_leaks_to_player == 0
AND caravan_direct_vehicle_damage_mutations == 0
AND duplicate_cargo_settlements == 0
AND duplicate_reward_rolls == 0
AND duplicate_traumatic_exposure_events == 0
AND core_convoy_unityengine_references == 0
AND core_convoy_godot_references == 0

AND fuzz_unseeded_rng == 0
AND rejected_fuzz_actions_with_side_effects == 0
AND fuzz_null_reference_exceptions == 0
AND fuzz_illegal_collection_mutations == 0
AND fuzz_state_corruption_events == 0
AND fuzz_negative_impossible_inventory_states == 0
AND fuzz_orphan_reference_count == 0
AND save_restore_side_effect_replays == 0
AND day80_canonical_hash_mismatches == 0
AND culture_dependent_hash_paths == 0
AND unbounded_active_collection_leaks == 0
AND infinite_economy_loops == 0
AND invalid_day120_epilogue_refs == 0

AND all_25_micro_location_ids_valid == true
AND micro_location_choice_branch_tests == pass
AND micro_location_determinism == pass
AND micro_location_save_roundtrip == pass
AND micro_location_localization_de_fr == pass
AND turntable_vinyl_integrity == pass
AND vinyl_turntable_morale_tests == pass
AND turntable_save_roundtrip == pass
AND caravan_45_day_integration == pass
AND caravan_paired_determinism == pass
AND caravan_save_roundtrip == pass
AND long_campaign_120_day_fuzz == pass
AND day80_hash_stability == pass
AND campaign_epilogue_validation == pass
AND nightly_performance_reference_gate == pass
AND catalog_integrity == pass
AND content_utilization == pass
AND scene_binding_selftest == pass
AND scene_lint == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 36. Implementer Handoff

1. Audit the current micro-location resolver before touching `MicroLocationModal`.
2. Build one encounter presentation DTO containing requirement badges and canonical risk/reward information.
3. Never roll choice resolution in UI.
4. Route failed traumatic outcomes through one semantic exposure event to mental health.
5. Keep vehicle flatbeds in the cargo-capacity domain; do not use them as arbitrary loot multipliers.
6. Make optional encounter audio failure non-fatal while keeping production asset-integrity warnings.
7. Generate branch tests from the 25-location catalog so every shipped choice path is exercised.
8. Preserve unresolved encounter deterministic identity across save/load.
9. Verify German and French catalog coverage for all shipped choice strings and disabled reasons.
10. Audit all eight vinyl records and use canonical item instances throughout the turntable loop.
11. Treat the source 15% stress reduction and 25% therapy acceleration as tuning data, not code constants.
12. Apply turntable recovery through the canonical stress/mental-health modifier seam exactly once.
13. Do not let music alter base trauma Stress Floor.
14. Route vinyl wear through item condition and use playback interval/cycle IDs to prevent reload exploits.
15. Make purified-alcohol cleaning transactional and honest about what physical damage it can restore.
16. Add vinyl placement through micro-location loot data with seeded RNG.
17. Keep `AudioManager` as runtime/rendering and `ShelterAcousticDirector` as acoustic intent/mix authority.
18. Compose turntable machinery ducking through the shared duck controller from the acoustic plan.
19. Persist only gameplay-relevant turntable state; never persist bus/effect/runtime objects.
20. Implement caravan missions as orchestration over existing systems, not as a new expedition/combat/inventory stack.
21. Revalidate vehicle, crew, guide, ration, cargo, and route state atomically before departure.
22. Read parole status only from `ShelterPrisonerSystem`.
23. Keep the 30% guide modifier data-driven and one-stack by default.
24. Ask `ShelterEspionageSystem` to resolve route-leak opportunities; never copy sleeper-agent truth into caravan state.
25. Build one documented ambush-risk projection with traceable inputs.
26. Hand ambushes to `ExpeditionCombatHandoff`.
27. Route mortar hits through canonical vehicle armor/damage.
28. Return archives and ingots as canonical cargo items. Do not unlock technology or increment a parallel foundry resource.
29. Route intense-firefight effects through the same mental-health traumatic-exposure contract used by micro-locations.
30. Make departure/arrival audio semantic and non-blocking.
31. Use `caravan_trade_network` only after confirming no existing save section already owns the same state.
32. Make mission transitions and cargo settlement exactly once.
33. Build the 45-day integration test with at least one guide, one sleeper leak, one ambush, one mortar hit, one trauma event, and one archive/ingot return.
34. Enforce zero `UnityEngine`/`Godot` references in Core convoy code.
35. Build the fuzzer on the real headless composition root and only public/test seams.
36. Fork deterministic RNG streams by subsystem so unrelated new random calls do not perturb all results.
37. Treat rejected random actions as valid outcomes and prove they do not mutate state.
38. Inject Day-30 and Day-60 crises through canonical systems.
39. Save/reload into fresh systems every 10 days.
40. Canonicalize state before hashing; do not compare object memory or unordered serialization.
41. Compare uninterrupted and reload-cycled Day-80 hashes.
42. Classify collections by retention expectations before declaring “unbounded.”
43. Record economic/mental/vehicle metrics without asserting that harsh campaigns can never contain death or breakdown.
44. Validate the Day-120 epilogue against final canonical facts.
45. Run the strict 5-second-style benchmark only on the designated calibrated Release runner; log median/p95 and hardware/runtime.
46. Keep integer/fixed-point state where already supported and invariant G9/G17 formatting where floats remain.
47. Emit versioned `artifacts/long_campaign_telemetry.json`.
48. Re-run catalog integrity and content utilization after fuzz.
49. Add the fuzz certification to `ARCHITECTURE_TEST_MAP.md`.
50. Mark SHIP only after all four task families pass together under save/load and deterministic long-campaign pressure.

---

# 37. Final Outcome

When this plan is complete, ASHFALL gains a coherent bridge from moment-to-moment scavenging decisions to shelter recovery and then into a true mid/endgame war economy.

A micro-location will no longer be a static card with an opaque button.

The player will see what a choice requires, which tool or skill matters, what the injury risk looks like, and what sort of material reward is at stake. The numbers will be the numbers the encounter resolver actually uses—not a second approximation invented by the UI.

When the player chooses badly, the consequences remain real.

A collapse can injure a survivor through the medical system.
A horrific discovery can become a traumatic-exposure event for mental health.
A flatbed can let the expedition carry home a bulky haul because the vehicle actually has the capacity.

The trailer does not create more scrap out of nowhere.

That same scavenging layer can seed vinyl records into high-value locations.

A recovered record is a real item with lore, condition, provenance, and deterministic acquisition. Playing it on the shelter turntable can become a meaningful recovery tool, but not a magic global morale button.

Music can reduce a bounded class of daily stress accumulation.
A particular album can support a compatible therapy routine.
A record can wear out.
It can be cleaned.
It can be damaged beyond what cleaning can repair.

All of those facts stay where they belong:
inventory owns the record,
item condition owns its wear,
mental health owns recovery,
the acoustic director owns how it sounds.

The result is diegetic rather than abstract.

That preparation then feeds the caravan loop.

A long-distance armored trade mission requires a vehicle that is actually serviced, a crew who are actually available, preserved food that will actually survive the journey, and cargo capacity that genuinely exists.

A paroled captive can serve as a guide because their parole status is real.
Their local knowledge can reduce route risk through a documented modifier.

But that does not make the route safe.

A sleeper agent hidden among the crew can create an espionage opportunity. If the espionage authority resolves a route leak, the ambush system receives that consequence without the caravan code suddenly becoming omniscient about who the spy is.

When an ambush happens, the mission hands control to the existing combat system.

Vehicle armor absorbs mortar damage because the vehicle damage model says it does.
Crew injuries are medical outcomes.
Combat shock is a mental-health outcome.
Cargo loss is an inventory/cargo outcome.

If the crew survives and returns with a prewar archive, that archive is not an instant technology unlock. It becomes a real archive object that still needs reconstruction and decryption.

If they return with raw ingots, those ingots enter the foundry through the same material pipeline as every other input.

The systems remain separate, but the gameplay loop becomes continuous.

The final layer is proof.

The 120-day campaign fuzzer repeatedly stresses the exact integration seams most likely to fail:
vehicle repair,
sleeper-agent leaks,
mental crises,
food preservation,
captives,
archives,
foundry jobs,
blackouts,
raids,
deep freeze,
save/load,
caravan missions,
and endgame chronicle generation.

Every ten days the entire campaign is serialized and restored into a fresh composition root.

On Day 80, a reload-cycled run must match an uninterrupted run through canonical deterministic state hashing.

At Day 120, the epilogue must describe a world that is actually consistent with what happened.

Telemetry must show that queues, incident lists, dead drops, event IDs, and other state do not leak forever.

Performance must remain bounded on a calibrated Release runner.

And the fuzzer is not allowed to hide genuine harsh outcomes. A bad campaign may contain starvation, breakdown, vehicle loss, or mission failure. The certification criterion is not “nothing bad happens.”

The criterion is:

**nothing impossible happens, nothing happens twice, nothing silently diverges after save/load, and every consequence belongs to exactly one authority.**

That is the integration standard for Tasks 13–16.
