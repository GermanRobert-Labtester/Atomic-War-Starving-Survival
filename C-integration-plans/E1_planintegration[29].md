---
PLAN_ID: E1-29
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 29
STATUS: READY_FOR_EXECUTION_AFTER_PLANS50_53_COMBAT_ESPIONAGE_PSYCHOLOGY_AUDIO_RECON
SOURCE_PLAN: "Plans 50–53 Follow-up — Vehicle Convoy Combat, Deep-Cover Espionage, Post-Traumatic Growth, Spatial Shelter Acoustics"
SEQUENCE_FILENAME: "E1_planintegration[29].md"
PREVIOUS_FILENAME: "E1_planintegration[28].md"
NEXT_FILENAMES:
  - "E1_planintegration[30].md"
  - "E1_planintegration[31].md"
CATEGORY: VEHICLES+COMBAT+ESPIONAGE+PSYCHOLOGY+AUDIO+GODOT
PRIMARY_INTENT: "Integrate armored vehicle combat, external mole networks, post-traumatic growth/resilience, and spatial room soundscaping while preserving singular ownership for combat resolution, vehicle persistent condition, faction/world intelligence, survivor psychology, research unlocks, shelter topology, structural state, water state, and audio presentation."
PREMISE_VERIFICATION_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SECOND_COMBAT_RESOLVER_FORBIDDEN: true
SECOND_VEHICLE_DAMAGE_LEDGER_FORBIDDEN: true
SECOND_FUEL_LEDGER_FORBIDDEN: true
SECOND_AMMO_LEDGER_FORBIDDEN: true
SECOND_ROUTE_AMBUSH_GENERATOR_FORBIDDEN: true
SECOND_FACTION_RELATIONSHIP_SYSTEM_FORBIDDEN: true
SECOND_RESEARCH_UNLOCK_SYSTEM_FORBIDDEN: true
SECOND_TRAUMA_ENGINE_FORBIDDEN: true
SECOND_STRESS_LEDGER_FORBIDDEN: true
SECOND_ROLE_SCHEDULER_FORBIDDEN: true
AUDIO_MUST_REMAIN_PRESENTATION_ONLY: true
RNG_MUST_BE_KEYED_AND_PARTITIONED: true
SAVE_REPLAY_MUST_BE_IDEMPOTENT: true
RUNTIME_RISK: VERY_HIGH
SAVE_RISK: VERY_HIGH
DETERMINISM_RISK: VERY_HIGH
COMBAT_BALANCE_RISK: VERY_HIGH
PSYCHOLOGY_DESIGN_RISK: HIGH
AUDIO_PERFORMANCE_RISK: VERY_HIGH
---

# E1 Plan Integration [29] — Armored Convoy Combat, Deep-Cover Espionage, Post-Traumatic Growth, and Spatial Shelter Acoustics

> **Sequence rule:** this file is `E1_planintegration[29].md`. The next file is `E1_planintegration[30].md`.

## 0. Mission

This plan integrates four follow-up bundles:

1. **Plan 50 — Overland Vehicle Convoy Armored Combat & Ambush Tactics**
2. **Plan 51 — Deep-Cover Spy Networks & Wasteland Diplomatic Infiltration**
3. **Plan 52 — Post-Traumatic Growth & Specialized Survivor Resilience Traits**
4. **Plan 53 — Interactive Acoustic Proximity & Spatial Room Soundscaping**

All four are high-value additions, but each can easily create a duplicate authority:

- VehicleGarage could accidentally become a second CombatSystem.
- Combat could accidentally become a second persistent vehicle-maintenance ledger.
- PatrolTerritoryAuthority could become a second encounter generator.
- ShelterEspionage could become a second faction-relationship or research-unlock system.
- Deep-cover agents could become detached duplicate survivor identities.
- Psychological growth traits could become a second trauma/stress engine.
- “Passive comfort” could become a hidden aura that bypasses Autonomy, Schedule, Housing, and Needs.
- ShelterAcousticBridge could start making gameplay decisions instead of presenting canonical shelter state.
- Spatial audio could create per-frame allocations, non-headless-safe AudioServer calls, and duplicated room/bulkhead truth.

The implementation standard is:

**Combat resolves tactical events; VehicleGarage owns persistent vehicle equipment and vehicle-condition state;
world/route authorities create encounter opportunities; ShelterEspionage owns covert-operation state while
Faction/Research/Expedition own their consequences; E1-15/Psychology owns trauma and growth evidence; audio
projects canonical shelter state and never becomes simulation authority.**

## 1. Cross-Plan Corrections

### 1.1 Directional armor needs a geometric combat contract

Do not store “front HP” and “rear HP” as arbitrary separate vehicle health bars unless the current vehicle model
actually supports zoned armor. Prefer:

- one canonical vehicle/component durability owner;
- directional armor coverage/deflection profiles;
- Combat-provided impact vector / hit sector;
- VehicleGarage-provided armor response;
- one committed persistent damage result.

### 1.2 Mounted weapons remain combat weapons

Vehicle modules can expose weapon capability, firing arc, mount stability, ammo type, and operator requirements.
CombatSystem still resolves shot timing, hit/miss, damage, suppression, and ammo use.

### 1.3 Fuel-line puncture is a causal hazard chain

A ballistic hit may damage a fuel-system component. Fuel owner then records leak; Fire/Hazard owner handles
ignition; Expedition/Vehicle owner handles immobilization. Do not directly set `on_fire=true` in Combat unless
Combat already owns vehicle fire incidents.

### 1.4 Ambush generation must use one encounter authority

PatrolTerritoryAuthority may contribute route threat and ambush eligibility if it is the canonical territorial
threat owner. The actual encounter-generation pipeline should remain singular.

### 1.5 Deep-cover moles remain the same survivor identity

Assignment to a rival stronghold changes mission/location/status. It must not create a duplicate “spy person”
record detached from SurvivorCatalog.

### 1.6 Cover integrity may be espionage-owned, but consequences are external

Cover integrity is a plausible covert-operation state owned by ShelterEspionage. Capture, death, diplomatic
fallout, mission travel, faction hostility, and research unlock effects still belong elsewhere.

### 1.7 Stolen blueprints should not blindly skip research prerequisites

A stolen blueprint can become:

- a canonical research artifact;
- a blueprint key;
- a prerequisite waiver token;
- a research-cost/time modifier;
- an archive-decryption unlock;

but the owning Research/Archive authority must decide exactly what it bypasses. Espionage does not directly
unlock technology.

### 1.8 Framing factions is an information operation

A mole may inject false evidence/claims into the canonical faction-information/world-event system. Whether
another faction believes it, retaliates, or changes standing belongs to those systems.

### 1.9 Post-traumatic growth is not guaranteed and is not a universal “benefit of trauma”

Growth traits should require recovery evidence and may or may not emerge. The system must not imply that trauma
is desirable, necessary for strength, or clinically predictable.

### 1.10 Growth traits must use canonical modifier interfaces

`trait_stoic_resolve` should contribute a typed stress-gain modifier to specific stressors.
`trait_grief_healer` should create a bounded comfort opportunity/action through Schedule/Housing/Autonomy or a
well-defined social-support hook—not a hidden global aura.

### 1.11 Relapse is context-sensitive reactivation, not trait deletion

A survivor may experience a trigger response after recovery without erasing earned growth or resetting the
entire trauma history. E1-15/Psychology owns the trigger/evidence response.

### 1.12 Spatial audio remains presentation-only

Audio may read:

- player camera;
- room topology;
- level/floor;
- sealed bulkhead state;
- generator operating state;
- structural stress;
- water/sump state;
- radio-console location;

but it may not change any of those values.

## 2. Canonical Ownership Matrix

| Concern | Canonical owner | E1-29 role |
|---|---|---|
| Combat hit/miss/damage | CombatSystem | consume vehicle tactical modifiers |
| Mounted weapon module identity | VehicleGarageSystem/catalog | expose capability |
| Persistent vehicle wear/damage | VehicleGarageSystem | own |
| Ammo | Inventory/Combat | consume |
| Fuel | Expedition/fuel authority | consume/leak handoff |
| Route threat | PatrolTerritory/world threat authority | input |
| Encounter spawn | canonical expedition encounter generator | create ambush |
| Hostile vehicle capture | Combat/World/Inventory + VehicleGarage | post-combat handoff |
| Mole survivor identity | SurvivorCatalog | reference |
| Mole covert state/cover | ShelterEspionageSystem | own |
| Mole mission/location | Expedition/world/person-location authority | handoff |
| Faction relations | faction authority | consequence |
| Research prerequisite/unlock | Research/PrewarArchive authority | consume stolen blueprint |
| Trauma/recovery/growth evidence | E1-15/Psychology | own |
| Persistent growth trait identity | canonical survivor trait/component owner | own once granted |
| Stress/morale | Needs/Psychology | modifiers/consequences |
| Counselor role | E1-20/Duty | own appointment/work |
| Memorial ritual | Memorial/E1-23 + Schedule | source event |
| Portrait expression | UI presentation | derived |
| Spatial audio attenuation | ShelterAcousticBridge | presentation |
| Room topology/bulkheads | E1-9/shelter topology | query |
| Structural stress | structure/E1-17 | query |
| Water/sump state | water/sanitation/fluid authority | query |
| Radio-console location | shelter scene/topology | query |
| Audio buses/limiter | AudioManager/ShelterAcousticDirector | presentation |

## 3. Release Slices

### Slice A — Vehicle combat capability
Mounted weapon + directional armor response + ammo conservation + one deterministic roadside ambush.

### Slice B — Vehicle tactical hazards
Smoke retreat, fuel-line puncture, component damage persistence, hostile vehicle recovery.

### Slice C — External mole vertical slice
Assign one survivor, degrade cover, send one report, trigger exfiltration, restore survivor safely.

### Slice D — Offensive espionage consequence
Steal one blueprint through canonical Research/Archive handoff and run one information-operation framing event.

### Slice E — Psychological growth
Resolve one trauma, evaluate one keyed growth milestone, persist one trait, prove typed stress modifier.

### Slice F — Social resilience
Memorial ritual, grief-healer comfort action, counselor assignment, relapse trigger, portrait/audio presentation.

### Slice G — Spatial audio
Distance attenuation, level crossfade, bulkhead occlusion, room-specific sources, headless-safe adapter.

### Slice H — Hardening
Deterministic replay, save migration, allocation tests, catalog integrity, content utilization, long-campaign soak.

---

## E1-29A — Repository authority recon

1. Audit CombatSystem, VehicleGarageSystem/state, vehicle modifications, weapon/ammo definitions, Expedition encounter pipeline, PatrolTerritoryAuthority, fuel system, hazards/fire, recovery/towing, and hostile vehicle entities.
2. Audit ShelterEspionageSystem/state/save, RadioIntelligencePanel, survivor mission/location state, faction info/relations, Expedition missions, PrewarArchiveDecryptionSystem, research prerequisites, Journal, acoustic cues, and localization.
3. Audit E1-15 psychology/trauma implementation, survivor trait/component store, Needs stress modifiers, Housing/Schedule, E1-20 roles, Memorial/E1-23, portrait UI, and mental-health save store.
4. Audit ShelterAcousticDirector, ShelterAcousticBridge, AudioManager, shelter room scenes, topology/bulkhead state, structural stress, water/sump state, radio-console node, audio buses, headless test adapters, and shelter_audio_cues.json.
5. Create `docs/forensics/E1_29_PLANS50_53_AUTHORITY_RECON.md`.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29B — Vehicle combat ownership ADR

1. Define which layer owns tactical hit resolution versus persistent vehicle condition.
2. Combat owns attack sequencing, hit/miss, impact vector, damage packet, suppression, retreat state, and combat outcome.
3. VehicleGarage owns installed modules and persistent damage/wear state.
4. Define typed adapter between them.
5. Add negative tests preventing Combat from maintaining a second saved vehicle-health ledger.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29C — Vehicle tactical modifier contract

1. Define read-only modifier DTO returned from VehicleGarage for active combat vehicle.
2. Include mount capabilities, firing arcs, stabilization, armor sectors, deflection policy, smoke charges, mobility state, and protection reason codes.
3. Do not expose mutable VehicleGarage internals.
4. Version the contract for deterministic replay.
5. Add serialization/unit tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29D — Mounted heavy MG catalog entry

1. Add `vmod_mounted_heavy_mg` to vehicle_modifications.json.
2. Define slot, compatible chassis, weapon capability ID, firing arc, operator requirement, stabilization penalty, ammo class, mass, and maintenance/wear effects.
3. Do not embed CombatSystem damage formulas directly in modification catalog if weapon definitions already own them.
4. Validate localization and item refs.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29E — Smoke canister rack catalog entry

1. Add `vmod_smoke_canister_rack` with charges/capability, compatible slots, mass, resource/reload requirements, and tactical action ID.
2. Smoke effect should reference Combat line-of-sight/visibility policy.
3. Do not create a second smoke-visibility implementation in VehicleGarage.
4. Add integrity tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29F — Vehicle weapon capability adapter

1. Map mounted weapon module to canonical Combat weapon profile.
2. Define operator seat/role and firing arc.
3. Use Combat's shot resolution and damage model.
4. VehicleGarage supplies mount/stability modifiers only.
5. Add mounted vs handheld weapon ownership tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29G — Vehicle ammo consumption

1. Route mounted weapon ammo through canonical Inventory/Combat transaction.
2. Use `ammo_standard` or `ammo_heavy` only if they are real catalog IDs; otherwise map to actual ammo definitions.
3. Do not deduct ammo in both VehicleGarage and Combat.
4. Use stable fire/burst operation IDs.
5. Add no-ammo and double-fire reconciliation tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29H — Directional armor sector model

1. Define vehicle-local hit sectors such as Front, FrontQuarter, Side, Rear, Roof/Underbody only to the fidelity current combat supports.
2. Combat converts world impact vector to vehicle-local sector.
3. VehicleGarage returns armor coverage/deflection for that sector.
4. Do not create arbitrary separate HP bars unless current component damage system supports them.
5. Add angle boundary tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29I — Armor deflection calculation

1. Use impact angle, armor profile, penetration/energy class, and damage type according to existing Combat abstractions.
2. Return deflected, absorbed, penetrated, and residual damage components.
3. Use deterministic fixed-point arithmetic where practical.
4. Do not claim real-world ballistic fidelity beyond data model.
5. Add exact-angle and threshold tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29J — Bullbar frontal protection

1. Map reinforced bullbar to qualifying frontal collision/ballistic protection only.
2. Do not cover exposed rear flatbed.
3. Allow component damage to bullbar itself if modification condition exists.
4. Preserve E1-27 rubble collision behavior without double stacking.
5. Add front-vs-rear tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29K — Rear flatbed vulnerability

1. Represent reduced armor coverage for exposed rear/cargo area.
2. Combat impact on rear sector may threaten cargo/passengers/components only through explicit combat/component rules.
3. Do not directly delete cargo from armor calculator.
4. Add rear-hit integration tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29L — Vehicle persistent combat damage

1. Update VehicleGarageState with canonical combat damage/component condition fields if not already present.
2. Persist damage once after Combat result commits.
3. Do not mirror per-shot temporary Combat HP in save unless it is the canonical persistent condition.
4. Add save round-trip after damaged sortie.
5. Define repair/service integration.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29M — Component damage routing

1. Map penetrations/hazards to specific vehicle components using canonical damage table or keyed component selection.
2. Persist only resulting component damage state.
3. Do not randomly damage components using global RNG.
4. Key stochastic component hit selection by combat event/impact ID.
5. Add deterministic component-damage tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29N — Mobile firing positions

1. Allow eligible vehicle occupants to fire while moving.
2. Combat applies movement/stabilization accuracy modifier based on canonical vehicle speed and mount/weapon type.
3. Do not let VehicleGarage resolve hit chance.
4. Define passenger safety/seat constraints.
5. Add stationary/slow/fast tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29O — Speed-based accuracy penalty

1. Use one documented curve or table.
2. Clamp modifier to sane bounds.
3. Mounted stabilized weapons may have different curve from handheld fire.
4. Use actual tactical speed, not map ETA speed.
5. Add boundary and deterministic replay tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29P — Smoke deployment command

1. Add Combat tactical action to deploy vehicle smoke from equipped rack.
2. Consume one real smoke charge/canister through canonical module/resource state.
3. Create one smoke obscuration entity/zone owned by Combat/visibility system.
4. Use stable action ID.
5. Add no-charge/double-submit tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29Q — Smoke line-of-sight effect

1. Combat/visibility authority applies concealment/LOS blocking.
2. VehicleGarage does not change hit chance directly after deployment.
3. Define duration, wind interaction only if current weather/combat model supports it.
4. Add attacker/defender LOS tests.
5. Persist only if combat save model requires mid-combat restore.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29R — Emergency tactical retreat

1. Smoke may satisfy retreat-enablement/penalty conditions but Combat owns retreat resolution.
2. Check mobility, route escape, immobilization, suppression, and enemy pressure.
3. Do not guarantee retreat because smoke exists.
4. Add successful/failed retreat tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29S — Fuel-line puncture damage event

1. Define fuel-system component as eligible damage target where vehicle architecture supports it.
2. A penetrating ballistic impact can emit `FuelSystemDamaged` with severity/provenance.
3. VehicleGarage/fuel authority applies leak rate/state.
4. Do not directly consume arbitrary fuel in UI/combat code.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29T — Fuel leak progression

1. Fuel authority decrements real fuel according to leak severity and simulation time.
2. Expose leak to Expedition/Combat as mobility/fire-risk context.
3. Use deterministic arithmetic.
4. Stop leak on repair/isolation if canonical mechanics exist.
5. Add conservation tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29U — Fuel-fire hazard handoff

1. Fuel leak creates a fire-hazard eligibility input to canonical Fire/Hazard system.
2. Ignition uses existing combat/fire event rules or keyed RNG.
3. Do not make every puncture ignite automatically unless design says so.
4. Persist incident decision to prevent reload reroll.
5. Add ignited/nonignited tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29V — Fuel-leak immobilization

1. Expedition/Combat vehicle becomes immobilized when canonical fuel/engine conditions no longer support movement.
2. Do not set separate `immobilized` flags in multiple systems.
3. Capture one reason code.
4. Integrate recovery mission if combat ends with stranded vehicle.
5. Add zero-fuel/leak tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29W — Vehicle combat audio events

1. Emit semantic audio cues for heavy MG fire, ricochet, and engine rev from committed presentation events.
2. Use ShelterAcousticDirector/AudioManager routing appropriate to field combat scenes.
3. Do not make audio state influence Combat.
4. Rate-limit bursts/ricochets to avoid audio spam.
5. Add muted/headless tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29X — Ambush authority audit

1. Identify the canonical expedition encounter-generation system.
2. Treat PatrolTerritoryAuthority as threat/territory input if it already owns patrol pressure.
3. Do not create a parallel ambush scheduler.
4. Document route threat -> encounter eligibility -> encounter spawn chain.
5. Add authority-map docs.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29Y — Dynamic route ambush eligibility

1. Use high-threat route segments, faction control/patrol pressure, time/weather/visibility only if existing systems expose them.
2. Return deterministic eligibility/probability inputs.
3. Do not use UI-visible threat score as hidden truth if it is only a read model.
4. Add threshold tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29Z — Ambush RNG partition

1. Key encounter roll by campaign seed + route segment + traversal occurrence + faction/threat policy version.
2. Persist encounter decision or stable occurrence counter.
3. Prevent reload/re-entry rerolls.
4. Add paired replay tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AA — Roadside ambush combat setup

1. Spawn combat through canonical encounter/CombatSystem.
2. Place convoy/attackers using authored or deterministic tactical setup rules.
3. Pass vehicle tactical modifier adapter.
4. Do not create bespoke vehicle-only combat loop.
5. Add one deterministic fixture.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AB — Hostile vehicle capture eligibility

1. After Combat outcome, detect intact/repairable hostile vehicles through world/loot authority.
2. Require faction/ownership transfer, operability/towability, route/recovery capacity, and garage compatibility.
3. Do not clone enemy vehicle into garage.
4. Create capture/salvage claim ID.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AC — Captured vehicle towing

1. Use existing recovery/towing expedition mechanics from E1-27.
2. Consume towing fuel/time and convoy capacity.
3. On arrival, transfer canonical vehicle identity into VehicleGarage if design permits.
4. Apply captured/damaged provenance.
5. Add duplicate-arrival tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AD — Captured vehicle catalog/compatibility

1. Define which hostile vehicle archetypes can become player vehicles.
2. Reject unsupported chassis rather than forcing every encounter vehicle into garage runtime.
3. Validate modification slot compatibility and initial damage.
4. Add content integrity tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AE — VehicleCombatTacticsTests

1. Create armor absorption/deflection, rear vulnerability, moving fire, smoke LOS, retreat, fuel puncture, mounted ammo, and capture/tow tests.
2. Use diagnostic deterministic fixtures.
3. Assert Inventory/fuel conservation.
4. Assert persistent damage round-trip.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AF — Vehicle combat replay determinism

1. Run identical-seed combat twice.
2. Compare shot hit/miss, impact sector, armor result, component damage, smoke action, fuel puncture, fire hazard decision, combat outcome, and ammo/fuel totals.
3. Partition RNG by shot/impact/action IDs.
4. Add call-order perturbation tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AG — Vehicle garage authority documentation

1. Update `docs/VEHICLE_GARAGE_AUTHORITY_MAP.md` with combat modules, armor response, persistent damage, fuel-system damage, and capture/recovery boundaries.
2. Reference E1-27 terrain/fuel integration.
3. Document Combat vs VehicleGarage ownership explicitly.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AH — Offensive espionage ownership ADR

1. Define ShelterEspionage-owned external mole state: assignment ref, target faction/site, cover integrity, handler channel, operation history, known intelligence, extraction eligibility.
2. Keep survivor identity/location/fate canonical elsewhere.
3. Keep faction relations and research unlocks external.
4. Require architecture review.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AI — Offensive operation catalog

1. Add `fop_plant_mole`, `fop_steal_blueprints`, and any supporting offensive operations to faction_intelligence.json.
2. Define eligibility, target faction/site, required operative traits/skills, duration, risk model version, outputs, localization, and handoff consumers.
3. Do not embed direct standing/research changes.
4. Add integrity tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AJ — External mole candidate eligibility

1. Query SurvivorCatalog skills/traits/health/duty availability.
2. Use high charisma/stealth only if those are real canonical stats.
3. Require consent/assignment policy consistent with E1-6/E1-20 if applicable.
4. Do not copy survivor profile into espionage save.
5. Return explicit blockers.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AK — Mole assignment transaction

1. Create stable external operation/assignment ID.
2. Remove/reassign survivor from shelter duty/location through canonical expedition/person-location authority.
3. Create ShelterEspionage mole state referencing same survivor ID.
4. Do not duplicate person inventory or needs.
5. Add cancel/save-load tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AL — Mole target stronghold

1. Use canonical world/faction site ID.
2. Do not store duplicate map coordinates if site graph already owns them.
3. Validate faction hostility/access and operation eligibility.
4. Reference travel/exfiltration route owner.
5. Add missing/destroyed stronghold tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AM — Cover integrity state

1. Store 0..1000 cover permille as espionage-owned covert-state if no existing owner exists.
2. Define meaning: exposure/credibility of cover, not general survivor morale/health.
3. Clamp deterministically.
4. Persist exact value and policy version.
5. Expose only player-known value/estimate according to communication rules.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AN — Weekly cover degradation scheduler

1. Use CampaignCalendar and due queue.
2. Evaluate once per defined week/interval, not per frame.
3. Inputs may include rival security pressure, recent mole actions, cover quality, handler traffic, faction alertness, and compromised contacts.
4. Do not apply a flat degradation if context says otherwise.
5. Add time-skip equivalence tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AO — Rival security sweep RNG

1. Key sweep/burn checks by campaign seed + mole ID + evaluation period + rival faction/site + policy version.
2. Persist decision receipt to prevent reroll.
3. Do not consume global shared RNG order.
4. Add paired replay tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AP — Cover integrity thresholds

1. Define Healthy/Strained/Compromised/Critical/Burned bands.
2. Source target <200 permille should create exfiltration eligibility/opportunity, not automatic teleport.
3. At zero/burned, canonical faction/security/person-fate outcome decides capture/death/escape.
4. Add exact threshold tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AQ — Handler communication channel

1. Integrate into RadioIntelligencePanel as a classified communication surface.
2. UI displays encoded/Morse-style presentation and decoded message only if canonical communication/intelligence state permits.
3. Do not implement real cryptography as gameplay truth unless required.
4. Use translated cable templates.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AR — Morse transmission presentation

1. Create semantic audio event for faint Morse pulses on send/receive.
2. Use ShelterAcousticDirector/radio audio source.
3. Audio remains presentation-only.
4. Rate-limit loop/pulses.
5. Respect mute/accessibility.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AS — Classified cable record

1. Define cable ID, mole/handler ref, direction, timestamp, operation/source ref, classification, encoded presentation template, decoded content ref, delivery status, and player-known state.
2. Do not store raw freeform secret truth twice.
3. Journal may reference committed cable.
4. Add save/retention tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AT — Espionage cable localization

1. Move all subject/body/flavor strings into translation catalogs.
2. Parameterize operative aliases, faction labels, sites, dates, and result terms.
3. Never show raw `fop_*` IDs.
4. Add visible-string scan tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AU — Exfiltration mission creation

1. When cover enters Critical band or an explicit extraction condition occurs, create one exfiltration mission request.
2. Use stable ID derived from mole + trigger.
3. Expedition/world mission owner handles route/team/travel.
4. Do not create duplicate exfiltration each weekly evaluation.
5. Add idempotency tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AV — Exfiltration resolution

1. On successful recovery, return same survivor identity to shelter/site.
2. Restore Duty/Needs/location registrations through canonical person lifecycle.
3. ShelterEspionage closes/pauses mole assignment.
4. On failure/capture/death, consume canonical outcome.
5. Add success/failure/save tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AW — Blueprint theft operation

1. Define target technology/blueprint source as canonical Research/Archive content.
2. Successful espionage produces a stolen-blueprint artifact/intelligence grant with stable provenance.
3. Do not directly call `UnlockTechnology` unless Research contract explicitly owns/permits that effect.
4. Add operation output tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AX — Research prerequisite bypass ADR

1. Audit PrewarArchiveDecryptionSystem and Research prerequisite authority.
2. Choose one supported mechanic: prerequisite waiver token, research-cost/time reduction, blueprint requirement satisfaction, or archive decryption key.
3. Do not skip unrelated prerequisites globally.
4. Document exact scope and revocability.
5. Add negative over-bypass tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AY — Blueprint grant idempotency

1. Use stable source grant ID tied to espionage operation result.
2. Research/Archive consumes once.
3. Reload cannot duplicate unlock/waiver/cipher reward.
4. Retain provenance in research history.
5. Add duplicate callback tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29AZ — Rival-infighting information operation

1. Represent framing as a false-claim/evidence operation inserted into canonical faction-information/world-event rails.
2. Target claim might implicate one faction in another's border incident.
3. Do not directly subtract faction relation points inside ShelterEspionage.
4. Use source/provenance and discovery risk.
5. Add believed/disbelieved/exposed outcomes.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BA — Faction reaction boundary

1. Faction AI/diplomacy authority evaluates planted evidence.
2. Standing/hostility/raid decisions remain external.
3. ShelterEspionage records operation success/failure/exposure only.
4. Do not guarantee inter-faction war from one operation.
5. Add no-effect/limited/escalated scenarios.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BB — Mole mission risk/action budget

1. Active offensive actions should consume cover, time, opportunities, or support according to explicit policy.
2. Do not allow unlimited blueprint theft each day.
3. Use cooldown/operation slots.
4. Make high-impact actions expose the mole more than passive reporting when design supports it.
5. Add exploit soak.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BC — Mole survivor needs/location boundary

1. Decide whether external moles remain fully simulated for hunger/health/gear.
2. Prefer the existing expedition/off-map survivor abstraction.
3. Do not maintain duplicate Needs values in ShelterEspionage.
4. Return canonical consequence events on extraction.
5. Document abstraction level.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BD — Mole death/capture boundary

1. Capture/death outcome belongs to faction/world/person-fate authority.
2. ShelterEspionage updates asset state from committed event.
3. E1-23/Memorial may consume death if survivor fate is known.
4. Journal only reports what player knows.
5. Add unknown-fate scenario.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BE — Offensive espionage journal cables

1. Log significant reports, cover compromise, stolen blueprint, framing operation, exfiltration, capture, or loss as classified telegraphic communiqués.
2. Use stable cable/operation IDs.
3. Do not log every weekly cover tick.
4. Respect information visibility.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BF — OffensiveEspionageNetworkTests

1. Simulate 90 days of assignment, reports, cover degradation, security sweeps, blueprint operation, information operation, exfiltration trigger, and resolution.
2. Assert same survivor identity throughout.
3. Assert no duplicate research grants.
4. Assert keyed deterministic outcomes and bounded history.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BG — External mole save round-trip

1. Persist mole assignment ID, survivor ref, target site/faction, cover integrity, policy version, operation state, cable refs, last/next evaluation period, extraction ref, and result receipts.
2. Do not persist copied survivor health/skills/inventory/faction standing/research tree.
3. Add active operation migration fixtures.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BH — Offensive espionage catalog integrity

1. Extend CatalogIntegrityRules for offensive operation IDs, target capability refs, research/blueprint outputs, cable templates, sites, and localization.
2. Reject direct faction-standing or research-unlock numeric fields if authority says they belong elsewhere.
3. Fail dead references.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BI — Espionage authority documentation

1. Update `docs/FACTION_ESPIONAGE_AUTHORITY_MAP.md` with external mole, cover, handler communication, exfiltration, blueprint theft, and framing boundaries.
2. Document RNG partition keys and save receipts.
3. Document player-known vs hidden cover/intelligence.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BJ — Psychological growth ownership ADR

1. Anchor all recovery/growth/relapse logic in E1-15/Psychology or the canonical survivor trait component.
2. Define which system owns earned growth-trait identity.
3. Needs owns stress values; Psychology owns trauma/recovery evidence.
4. Roles/Housing/Schedule own social-support context.
5. Require architecture review.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BK — Growth trait catalog entries

1. Add `trait_stoic_resolve` and `trait_grief_healer` to psychological_trauma.json only if that catalog is the correct trait registry; otherwise reference the canonical trait catalog.
2. Define localization, eligibility, typed modifiers/actions, provenance requirements, stacking rules, and save behavior.
3. Do not hard-code effects only in UI.
4. Add vocabulary validation.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BL — Trauma resolution milestone event

1. Use canonical therapy/recovery completion evidence from E1-15.
2. Require sustained recovery criteria rather than one treatment completion.
3. Emit one stable `TraumaRecoveryMilestone` per resolved trauma arc/version.
4. Do not erase trauma history.
5. Add duplicate milestone tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BM — Growth milestone eligibility

1. Check survivor alive/active, resolved trauma evidence, no existing mutually exclusive growth result, trait cap, and policy criteria.
2. Do not guarantee a trait for every resolved trauma.
3. Do not require trauma as the only path to general resilience if game design later adds other development routes.
4. Return explainable reason.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BN — Growth RNG key

1. Key growth decision by campaign seed + survivor ID + resolved trauma arc ID + growth policy version.
2. Persist decision/result receipt.
3. Do not reroll through reload or repeated therapy.
4. Allow explicit no-trait outcome.
5. Add deterministic replay tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BO — Stoic Resolve typed modifier

1. Implement as a stress-gain modifier consumed by Needs/Psychology for specific tagged stressors: cold, darkness, injury.
2. Source target is 25% reduction; encode as 750 permille multiplier if compatible with modifier framework.
3. Do not reduce all stress globally.
4. Do not affect hunger, grief, guilt, or unrelated stressors unless separately designed.
5. Add stacking/cap tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BP — Stoic Resolve stacking rules

1. Define combination with shelter comfort, leadership, medicine, other resilience traits, and temporary ritual buffs.
2. Prefer multiplicative or capped modifier pipeline already used by Needs.
3. Prevent reaching zero/negative stress gain through stacking.
4. Add modifier-order determinism tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BQ — Grief Healer opportunity contract

1. Do not implement invisible passive AoE comfort on every sleep tick.
2. Create social-support opportunity when eligible bunkmates share sleeping/housing context and one has grief/stress need.
3. Autonomy/Schedule/Housing validates time/presence.
4. Psychology/Needs applies bounded comfort effect after committed interaction.
5. Add no-target/no-time tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BR — Bunkmate relationship boundary

1. Housing owns bunk/room co-occupancy.
2. Relations may influence willingness/effect but E1-29 does not mutate affinity directly.
3. Do not comfort absent/hostile/sleeping-at-another-site survivors through a room-wide aura.
4. Use stable interaction ID.
5. Add location tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BS — Memorial annual eulogy event

1. CampaignCalendar determines anniversary.
2. E1-23/Memorial owns memorial/death reference.
3. Schedule/Duty/Autonomy creates actual ritual event/attendance.
4. Psychology/Needs consumes ritual exposure.
5. Do not auto-apply resilience to the whole shelter merely because date passed.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BT — Temporary memorial resilience modifier

1. Represent any resilience benefit as a time-bounded canonical modifier with source event ID.
2. Define who receives it: attendees, close relations, or other explicit audience.
3. Do not persist as permanent trait.
4. Do not stack indefinitely from repeated reloads/rituals.
5. Add expiration/idempotency tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BU — Trauma relapse trigger contract

1. E1-15 owns original trigger tags/evidence.
2. Context systems emit matching exposure such as collapsed mine, confinement, fire, combat, loss, etc.
3. Psychology determines whether a trigger response occurs and severity.
4. Do not erase recovery/growth trait.
5. Add resolved-trauma trigger tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BV — Relapse response semantics

1. Use panic/stress/avoidance/autonomy consequences through Psychology/Needs.
2. Do not automatically reset therapy progress to zero.
3. Allow coping/growth traits to modify response if canonical policy supports it.
4. Persist event provenance.
5. Add repeated-exposure cooldown.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BW — Camp Counselor role

1. Add or map Camp Counselor through E1-20 role system.
2. Require actual room/work location in `room_main` only if that room is canonical and suitable; otherwise use a capability-based location.
3. Duty/Schedule owns work periods.
4. Psychology/Needs consumes support events.
5. Do not apply passive global anxiety reduction merely from assignment.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BX — Counselor group-support action

1. During scheduled work, counselor may create bounded group-support sessions or ambient support opportunity.
2. Participants require presence/eligibility.
3. Use one event rather than per-frame stress reduction.
4. Do not duplicate therapy system.
5. Add no-attendance and overlapping-role tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BY — Portrait expression projection

1. Map canonical psychological presentation state into distressed/neutral/resolved portrait expressions.
2. UI owns sprite/expression choice only.
3. Do not expose hidden diagnoses or exact trauma score through facial state.
4. Provide graceful fallback for missing expression assets.
5. Add UI binding tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29BZ — Resolved expression semantics

1. `resolved` means current presentation/recovery state, not permanent immunity.
2. Relapse can temporarily change expression without deleting earned growth.
3. Do not use portrait state as simulation input.
4. Add state transition tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CA — Soothing acoustic variation boundary

1. ShelterAcousticDirector may select presentation variation when canonical camp morale/support context is high.
2. Do not infer morale from presence of growth traits alone.
3. Audio choice must not modify Needs.
4. Use slow hysteresis/cooldown to avoid constant switching.
5. Add muted/headless tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CB — Growth milestone journal

1. Log major recovery breakthrough using survivor ID, recovery arc/provenance, and localized text.
2. Use restrained celebratory wording without implying trauma was beneficial or necessary.
3. Do not log every therapy step.
4. Deduplicate by milestone ID.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CC — Mental-health save persistence

1. Ensure earned growth trait IDs persist through SurvivorMentalHealthSaveStore or canonical trait/component store.
2. Persist growth decision receipts and source trauma arc refs where needed.
3. Do not duplicate Needs stress values.
4. Add old-save migration with no fabricated growth traits.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CD — Growth catalog integrity

1. Run --data-integrity-selftest for growth trait IDs, stressor tags, modifier IDs, role/action refs, localization, and mutual-exclusion rules.
2. Reject unknown stressor vocabulary.
3. Reject traits with direct unsupported stat writes.
4. Fail actionable diagnostics.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CE — SurvivorPostTraumaticGrowthTests

1. Test sustained recovery prerequisite, no-result outcome, Stoic Resolve targeted modifier, Grief Healer interaction, eulogy temporary resilience, relapse after matching trigger, Counselor action, portrait projection, save round-trip, and no-reroll.
2. Include survivor with unresolved trauma as negative control.
3. Verify no direct relation/morale mutations.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CF — Mental-health authority documentation

1. Update `docs/SURVIVOR_MENTAL_HEALTH_AUTHORITY_MAP.md` with recovery milestone, growth trait, relapse, social-support, ritual, role, and UI/audio boundaries.
2. Document keyed RNG and stacking caps.
3. Document that growth is optional and not guaranteed.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CG — Spatial audio ownership ADR

1. Define ShelterAcousticBridge as presentation adapter over canonical camera/topology/room/system state.
2. Audio owns gain/pan/filter/crossfade state only.
3. Do not persist simulation state in audio components.
4. Define headless null-object/no-op backend.
5. Add architecture tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CH — SpatialAcousticNode2D

1. Implement a Godot Node2D/helper responsible for deterministic distance/zoom attenuation projection.
2. Use camera transform and emitter/room anchor positions.
3. Keep calculation pure and testable outside AudioServer.
4. Do not allocate per tick.
5. Separate math from engine side effects.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CI — Distance attenuation curve

1. Define min/max distance and curve family (linear/log/custom piecewise) appropriate to shelter camera scale.
2. Clamp dB output.
3. Use zoom-aware effective distance only if the intended UX calls for it.
4. Document +6 dB close-focus cap separately from physical distance law.
5. Add numeric unit tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CJ — Stereo panning

1. Map emitter screen/world offset relative to camera to stereo pan.
2. Clamp to supported bus/player range.
3. Smooth transitions to avoid zippering.
4. Do not hard-pan room-wide ambience excessively.
5. Add left/center/right tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CK — Generator-room zoom boost

1. Amplify diesel thuds/turbine whine toward source target up to +6 dB when camera zoom/focus is close.
2. Apply as presentation gain after base attenuation.
3. Respect limiter/headroom.
4. Use smooth interpolation.
5. Add close/far/zoom tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CL — Bulkhead occlusion topology query

1. Use canonical shelter room graph and sealed blast-bulkhead state.
2. Determine acoustic path from camera room to emitter room.
3. Do not duplicate door/seal state in audio scene.
4. Cache/invalidate path result when topology/door state changes.
5. Add open/sealed tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CM — 400 Hz low-pass occlusion

1. Apply low-pass filter target around 400 Hz for strongly occluded path according to design.
2. Smooth filter transitions.
3. Use partial attenuation/filtering for intermediate barriers if current model supports it.
4. Do not manipulate shared global bus in a way that affects unrelated sources.
5. Add filter-target tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CN — Level crossfade model

1. Define Surface Gate, Upper Quarters, Deep Strata or canonical level IDs from topology.
2. Crossfade source groups when camera changes level.
3. Keep overlapping transition smooth and bounded.
4. Do not stop/restart all loops abruptly.
5. Add rapid-switch test.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CO — Generator source binding

1. Bind generator-room emitters to canonical generator operating/load state.
2. Diesel/turbine layers reflect presentation categories from Power system.
3. Audio does not determine power output.
4. Use start/stop hysteresis.
5. Add generator-off/on/load tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CP — Excavation rockfall ambience

1. Read canonical structural stress permille for excavation sectors.
2. Scale rumble intensity/frequency presentation according to stress bands.
3. Do not spawn structural damage from audio.
4. Do not expose hidden exact risk if gameplay intends uncertainty; use presentation bands.
5. Add low/high stress tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CQ — Water/sump acoustic states

1. Query canonical water/fluid/sanitation state.
2. Dry pipes may click/hiss; flowing systems use normal fluid layer; flooded sumps gurgle.
3. Audio never changes flow/pressure/flooding.
4. Use state-transition crossfades.
5. Add dry/normal/flooded tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CR — Spatial radio source

1. Anchor radio broadcast audio at canonical communications-console node/room.
2. Apply distance/pan/occlusion like other localized emitters.
3. Preserve E1-12 radio intelligibility/ducking rules where speech is critical.
4. Do not hide essential text information because player camera is far away.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CS — Critical alert spatial-audio boundary

1. Critical safety alerts must remain accessible per E1-12 even if spatial source is distant/occluded.
2. Use dual-path semantic notification/alert mix where necessary.
3. Do not make life-critical alarm unintelligible solely for realism.
4. Add muted/far/occluded alert tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CT — Headless audio backend

1. Detect headless/test mode before AudioServer/player operations.
2. Use null/no-op backend implementing same interface.
3. Pure attenuation/occlusion math remains testable.
4. Do not scatter null checks across every call site.
5. Add headless scene-load test.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CU — Zero-allocation audio tick

1. Preallocate/cached source state, bus references, filter objects/indices, room path data, and scratch buffers.
2. Avoid LINQ, closures, string formatting, temporary arrays, and dictionary churn in hot tick.
3. Measure allocated bytes per update.
4. Fail performance test above agreed budget (target zero steady-state heap allocations).

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CV — Audio update cadence

1. Separate camera-sensitive updates from slower topology/state invalidations.
2. Use dirty flags/event-driven source-state changes.
3. Do not recompute all acoustic paths every frame when nothing changed.
4. Define max source count/budget.
5. Measure p95 update time.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CW — Master limiter configuration

1. Configure limiter on Master bus using project audio bus asset/config.
2. Set threshold/release/headroom based on actual mix testing.
3. Do not rely on limiter as excuse for uncontrolled +dB stacking.
4. Verify alarm cascade avoids digital clipping.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CX — Cue spatial metadata schema

1. Validate all 11 entries in shelter_audio_cues.json expose required attenuation/occlusion/spatial-mode metadata.
2. Define defaults explicitly.
3. Reject impossible ranges or missing source-group mapping.
4. Keep critical non-spatial/semantic alerts distinguishable.
5. Add data-integrity test.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CY — Shelter room emitter scene lint

1. Run scene-lint.py across shelter room scenes.
2. Validate emitter node naming, parent/container rules, room/source IDs, and optional spatial metadata.
3. Do not require emitters in rooms with no audio content.
4. Fail duplicate emitter IDs.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29CZ — SpatialAcousticBridgeTests

1. Test attenuation at multiple distances/zoom levels.
2. Test pan positions.
3. Test sealed/open bulkhead filter target.
4. Test level crossfade.
5. Test headless no-op backend.
6. Test zero-allocation steady-state update harness.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29DA — Audio performance telemetry

1. Add debug/dev telemetry for active spatial sources, update time, allocated bytes, occlusion recomputes, bus writes, and clipped/limited peaks if available.
2. Do not write telemetry to campaign save.
3. Keep disabled/cheap in release builds.
4. Add threshold diagnostics.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29DB — Audio save boundary

1. Do not persist camera pan, current crossfade interpolation, bus filter envelope, or transient emitter gain.
2. Persist only user audio preferences in their proper settings owner.
3. On load, rebuild from canonical shelter state.
4. Add save-inspection negative test.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29DC — Cross-system event ID policy

1. Standardize vehicle combat impact IDs, breakdown/capture IDs, mole operation/cable IDs, growth milestone IDs, and audio presentation event correlation IDs.
2. Ensure downstream Journal/Audio/UI effects can dedupe.
3. Do not derive IDs from wall-clock time.
4. Document ID format.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29DD — Cross-system RNG partitioning

1. Create explicit RNG namespaces for vehicle shot/impact if Combat owns them, route ambush occurrence, mole security sweep, offensive operation resolution, growth milestone, and foundry-independent audio variation if any.
2. Audio presentation variation must never consume gameplay RNG.
3. Document key inputs and policy version.
4. Add stream-isolation tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29DE — Cross-system save/replay audit

1. Save at vehicle ambush midpoint/post-damage, mole weekly evaluation/exfiltration, trauma recovery/growth decision, and spatial-audio scene transition.
2. Ensure no duplicated ammo/fuel/damage, mole roll, growth trait, Journal entry, or audio event.
3. Validate old saves initialize new fields prospectively.
4. Add corruption/missing-ref fallback tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29DF — Cross-system catalog integrity

1. Validate new vehicle mods, mounted weapon/ammo refs, offensive espionage operation IDs, blueprint outputs, growth traits/stressor tags, audio spatial metadata, translation keys, and scene emitter IDs.
2. Run data-integrity selftest.
3. Reject dead/orphan references.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29DG — Content utilization audit

1. Run content-utilization selftest for new vehicle modules, espionage operations, growth traits, and audio cue metadata where registered.
2. Require real runtime consumers.
3. Do not count docs/tests as gameplay consumption unless repository rules explicitly allow.
4. Remove or rewire dead content.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29DH — Localization audit

1. Ensure vehicle tactical actions, ambush text, mole cables, exfiltration notices, growth milestones, counselor/ritual text, and audio-related UI labels use translation catalogs.
2. Reject raw IDs.
3. Validate format parameters.
4. Add visible-string scanner tests.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29DI — Accessibility audit

1. Mounted weapon/smoke/vehicle damage have non-audio UI feedback.
2. Mole communications provide textual equivalents to Morse audio.
3. Growth/relapse states do not rely solely on portrait expression.
4. Spatial/occluded critical alerts retain E1-12 accessible semantics.
5. Support reduced stimulation.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29DJ — Balance instrumentation

1. Track vehicle combat ammo/fuel/repair burden, ambush frequency, captured vehicle frequency/value, mole survival/cover duration, blueprint theft impact, growth trait frequency, relapse frequency, and audio source load.
2. Use metrics for tuning, not gameplay authority.
3. Do not store unnecessary per-event analytics in campaign save.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29DK — Vehicle combat balance soak

1. Simulate representative convoy encounters across light/medium/heavy threat routes.
2. Measure armor survival, rear vulnerability, mounted MG ammo use, smoke retreat success, fuel-line incidents, recovery burden, and captured vehicle value.
3. Ensure mounted weapons are powerful but resource/position constrained.
4. Check no invulnerable frontal build.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29DL — Espionage 90-day soak

1. Run multiple mole archetypes against different faction security levels.
2. Measure cover half-life, operation throughput, exfiltration frequency, capture/loss, research impact, faction-framing success, and cable volume.
3. Ensure high-impact operations carry real exposure/temporal cost.
4. Prevent reload farming.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29DM — Psychology long-run soak

1. Simulate trauma resolution, no-growth outcomes, growth unlocks, stressor exposure, relapse, eulogies, bunkmate support, counselor sessions, and save/load across long campaigns.
2. Measure trait prevalence and stress reduction.
3. Prevent universal growth saturation or zero-stress stacking.
4. Review narrative tone.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29DN — Spatial audio stress soak

1. Run maximum shelter room/source count with camera pan/zoom, level switching, bulkhead toggles, power changes, structural stress, water state changes, radio, and alarm cascades.
2. Measure p95 update time, allocations, active buses/filters, and limiter behavior.
3. Verify no headless crashes.
4. Verify no source leaks after scene unload.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29DO — Documentation pack

1. Update `docs/VEHICLE_GARAGE_AUTHORITY_MAP.md`.
2. Update `docs/FACTION_ESPIONAGE_AUTHORITY_MAP.md`.
3. Update `docs/SURVIVOR_MENTAL_HEALTH_AUTHORITY_MAP.md`.
4. Update `docs/SHELTER_ACOUSTIC_AUTHORITY_MAP.md` with attenuation/occlusion curves and topology ownership.
5. Add E1-29 RNG/save boundary notes to architecture docs.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

## E1-29DP — Release gate

1. Run dotnet build/tests and game build.
2. Run data-integrity and content-utilization selftests.
3. Run VehicleCombatTacticsTests, offensive espionage 90-day tests, SurvivorPostTraumaticGrowthTests, SpatialAcousticBridgeTests, scene lint, save migration, deterministic replay, conservation, and performance tests.
4. Verify no duplicate Combat/VehicleGarage damage ledger, no direct Espionage faction/research mutation, no second trauma/stress engine, and no simulation dependency on audio.
5. Mark DONE only when all four vertical slices pass together.

**Gate:** preserve canonical ownership, stable IDs, deterministic replay, save/load idempotency, and relevant conservation.

---

# 4. Vehicle Combat Tactical Contracts

## 4.1 Impact flow

```text
Combat shot
-> hit/miss
-> impact vector + damage packet
-> vehicle local hit sector
-> VehicleGarage armor response
-> residual component damage
-> VehicleGarage persistent condition
```

Combat owns the tactical hit.
VehicleGarage owns persistent vehicle condition.

## 4.2 Mounted weapon flow

```text
operator action
-> Combat validates mount/arc/ammo
-> Inventory reserves ammo
-> Combat resolves burst
-> ammo consumed once
-> damage/suppression committed
-> audio presentation event
```

## 4.3 Directional armor example

```yaml
vehicle_id: "veh_convoy_01"
impact_id: "impact_000193"
sector: "FRONT"
impact_angle_permille: 210
armor_profile:
  coverage_permille: 900
  deflection_permille: 320
result:
  deflected: false
  absorbed_damage: 14
  residual_damage: 9
```

Exact fields must match existing combat units.

## 4.4 Smoke retreat

```text
deploy smoke
-> consume rack charge
-> Combat creates smoke visibility volume
-> LOS modifiers apply
-> retreat action re-evaluates
```

Smoke helps; it does not guarantee retreat.

## 4.5 Fuel-line hazard

```text
penetrating component hit
-> fuel-system damage event
-> canonical leak rate
-> real fuel decreases
-> optional Fire/Hazard ignition
-> mobility/immobilization follows fuel/engine state
```

No duplicate fuel balance.

# 5. Ambush Authority

Recommended chain:

```text
PatrolTerritory/world threat
-> route traversal context
-> canonical encounter generator
-> keyed ambush eligibility/roll
-> Combat encounter
```

PatrolTerritory may supply risk and faction pressure; it should not become a second independent encounter loop.

# 6. Hostile Vehicle Capture

```text
combat ends
-> hostile vehicle survives
-> world/loot authority marks capturable
-> player claims/tows
-> canonical travel/recovery
-> same vehicle identity enters garage
```

Do not instantiate a fresh duplicate based only on vehicle type.

# 7. Deep-Cover Mole Record

```yaml
mole_assignment_id: "mole_017"
survivor_id: "survivor_0042"
target_faction_id: "faction_iron_raiders"
target_site_id: "site_raider_stronghold"
cover_integrity_permille: 640
cover_policy_version: 2
last_eval_period: 11
next_eval_period: 12
active_operation_id: "fop_steal_blueprints_018"
exfiltration_mission_id: null
```

No copied survivor health, inventory, charisma, stealth, or faction standing.

# 8. Cover Integrity Rules

Cover integrity means **covert cover viability**, not health, morale, loyalty, or reputation.

Possible bands:

- 800–1000: Secure
- 500–799: Strained
- 200–499: Compromised
- 1–199: Critical
- 0: Burned

Tune after simulation.

# 9. Cover Burn RNG

Recommended key:

```text
campaign_seed
+ survivor_id
+ target_site_id
+ evaluation_period
+ cover_policy_version
```

The result is reproducible and cannot be rerolled by loading.

# 10. Exfiltration Flow

```text
cover becomes critical
-> one exfiltration eligibility event
-> expedition mission created
-> route/travel resolves
-> same survivor recovered / captured / lost
-> mole assignment resolves
```

ShelterEspionage does not teleport the survivor home.

# 11. Blueprint Theft Boundary

Correct:

```text
espionage success
-> stolen blueprint / research evidence artifact
-> Research or PrewarArchive validates scope
-> prerequisite waiver / cost reduction / unlock according to canonical rule
```

Incorrect:

```text
ShelterEspionageSystem.UnlockEverythingBefore(tech_id)
```

# 12. Framing Factions

```text
mole operation
-> planted false claim/evidence
-> faction information system
-> target faction evaluates belief
-> diplomacy/AI decides response
```

No direct relation subtraction inside espionage.

# 13. Post-Traumatic Growth Contract

A growth milestone requires:

```text
canonical trauma arc
+ sustained recovery evidence
+ milestone ID
+ eligibility policy
+ keyed growth decision
```

Allowed outcomes include:

- no new trait;
- Stoic Resolve;
- Grief Healer;
- other future authored growth adaptation.

Recovery remains valuable even when no trait unlocks.

# 14. Stoic Resolve Modifier

Target source requirement:

```text
cold stress gain      * 0.75
darkness stress gain  * 0.75
injury stress gain    * 0.75
```

Use the canonical stress modifier pipeline and stacking caps.

It does not reduce all stress sources.

# 15. Grief Healer Action

Preferred:

```text
shared housing / sleep context
-> distressed/grieving bunkmate exists
-> social-support opportunity
-> Autonomy/Schedule validates
-> comfort interaction commits
-> Psychology/Needs applies bounded effect
```

Avoid a passive room-wide `stress -= N` every sleep tick.

# 16. Memorial Ritual

```text
CampaignCalendar anniversary
-> Memorial/E1-23 event opportunity
-> Schedule/attendance
-> ritual completes
-> temporary resilience source event
-> Psychology/Needs applies time-bounded modifier
```

No attendance means no automatic individual benefit.

# 17. Relapse Boundary

A matching trigger may create a panic/stress response even after recovery.

It does not:

- delete the growth trait;
- erase recovery history;
- reset all therapy;
- guarantee chronic deterioration.

# 18. Camp Counselor Boundary

E1-20/Duty owns the appointment/work period.

Psychology owns support outcomes.

`room_main` should be treated as a candidate location only after confirming that the canonical room exists and
has the intended group-support capability.

# 19. Spatial Acoustic Math

A pure function should map:

```text
camera_position
camera_zoom
emitter_position
base_gain
room_path
occlusion_state
level_relationship
```

to:

```text
gain_db
pan
low_pass_cutoff_hz
crossfade_weight
```

Engine-side AudioServer writes happen afterward.

# 20. Bulkhead Occlusion

For a sealed heavy blast bulkhead, source target requests dramatic filtering around 400 Hz.

Implementation should:

- query canonical bulkhead state;
- compute whether the acoustic path crosses it;
- smooth filter transition;
- apply per-source/per-group filtering rather than corrupting unrelated global buses;
- preserve E1-12 critical-alert intelligibility.

# 21. Generator Zoom Boost

Up to +6 dB is a presentation cap, not a guarantee that final output reaches +6 dB relative to Master.

Actual gain must respect:

- source headroom;
- limiter;
- other room layers;
- accessibility settings;
- crossfade state.

# 22. Rockfall and Water Sound Boundaries

Rockfall:

```text
canonical structural stress
-> audio presentation intensity
```

Water:

```text
canonical dry/flowing/flooded sump state
-> audio layer selection
```

Audio never drives stress, flow, or flooding.

# 23. Zero-Allocation Audio Checklist

- cache bus indices;
- cache filter/effect handles;
- cache emitter state;
- no LINQ;
- no closures;
- no string formatting;
- no temporary arrays;
- no per-tick dictionaries;
- dirty/invalidation events for topology;
- fixed scratch buffers if needed;
- test warm steady-state allocations.

# 24. Cross-Plan Exploit Matrix

| Failure | Guard |
|---|---|
| Vehicle armor applied twice | Combat/VehicleGarage adapter contract |
| Mounted MG consumes ammo twice | Combat/Inventory transaction ID |
| Smoke action rerolls retreat | Combat action ID |
| Fuel puncture creates fuel out of sync | canonical fuel ledger |
| Ambush rerolls on reload | route traversal RNG key |
| Captured vehicle cloned | same vehicle identity |
| Mole survivor duplicated | SurvivorCatalog reference |
| Weekly cover roll rerolls | mole-period key |
| Blueprint theft bypasses all research | scoped Research/Archive contract |
| Framing directly changes faction standing | info-operation handoff |
| Growth trait rerolls after therapy | trauma-arc milestone key |
| Stoic Resolve suppresses all stress | tagged stressors only |
| Grief Healer becomes hidden aura | committed support action |
| Annual eulogy stacks infinitely | source ID + expiry |
| Relapse deletes growth trait | immutable trait/history |
| Audio changes structural state | presentation-only boundary |
| Camera zoom exceeds headroom | gain cap + limiter |
| Headless tests call AudioServer | no-op backend |
| Audio tick allocates | performance test |

# 25. Save/Migration Rules

## Vehicle
Persist new combat-damage fields only in VehicleGarage canonical save.
Old vehicles initialize missing damage fields to baseline compatible values without replaying past combat.

## Espionage
Persist mole assignments/cover/evaluation receipts.
Old saves create no retroactive moles, security sweeps, or exfiltration missions.

## Psychology
Old saves receive no fabricated growth traits.
Existing resolved trauma should not automatically back-roll growth unless product explicitly adopts a migration
policy; prospective evaluation is safer.

## Audio
Persist no transient spatial mix state.
Rebuild gain/pan/filter/crossfade from canonical shelter state and user settings.

# 26. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj

godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
```

Also run:

- `VehicleCombatTacticsTests.cs`
- `OffensiveEspionageNetworkTests.cs`
- `SurvivorPostTraumaticGrowthTests.cs`
- `SpatialAcousticBridgeTests.cs`
- shelter room `scene-lint.py`
- paired deterministic replays
- save/migration fixtures
- long-campaign/performance soaks

# 27. Completion Checklist

## Vehicle combat
- [ ] Combat accepts typed vehicle tactical modifiers.
- [ ] Mounted heavy MG catalog entry validates.
- [ ] Smoke canister rack validates.
- [ ] Directional armor uses impact geometry.
- [ ] Persistent damage remains VehicleGarage-owned.
- [ ] Moving fire uses Combat accuracy modifiers.
- [ ] Smoke affects LOS through Combat visibility.
- [ ] Retreat remains Combat-owned.
- [ ] Fuel-line damage uses canonical fuel/fire chain.
- [ ] Mounted ammo conserves Inventory.
- [ ] Ambush generation uses one encounter authority.
- [ ] Hostile vehicle capture preserves identity.
- [ ] Combat replay is deterministic.
- [ ] VehicleGarageState round-trips combat damage.

## Deep-cover espionage
- [ ] Offensive operations validate.
- [ ] Mole uses same SurvivorCatalog identity.
- [ ] Cover integrity has one owner.
- [ ] Weekly cover checks are due-queued/keyed.
- [ ] RadioIntelligencePanel uses classified presentation.
- [ ] All cable strings are localized.
- [ ] Critical cover creates one exfiltration mission.
- [ ] Exfiltration restores same survivor identity.
- [ ] Blueprint theft routes through Research/Archive.
- [ ] Research bypass is scoped and explicit.
- [ ] Framing uses faction information authority.
- [ ] 90-day replay is deterministic.
- [ ] Active moles round-trip through save.

## Post-traumatic growth
- [ ] Growth traits conform to vocabulary.
- [ ] Sustained recovery milestone is canonical.
- [ ] Growth roll is keyed/no-reroll.
- [ ] No-trait outcome is valid.
- [ ] Stoic Resolve affects only tagged stressors.
- [ ] Stacking cannot reduce stress below safe floor.
- [ ] Grief Healer uses a real support interaction.
- [ ] Memorial ritual uses actual attendance.
- [ ] Temporary resilience expires/idempotently applies.
- [ ] Relapse does not erase growth/recovery history.
- [ ] Camp Counselor uses E1-20/Duty.
- [ ] Portrait expressions remain presentation-only.
- [ ] Growth traits persist across save.
- [ ] Old saves gain no fabricated traits.

## Spatial audio
- [ ] SpatialAcousticNode2D math is pure/testable.
- [ ] Attenuation and pan curves are documented.
- [ ] Generator focus boost is capped/headroom-safe.
- [ ] Bulkhead occlusion queries canonical topology.
- [ ] 400 Hz filtering is smoothly applied.
- [ ] Structural stress only drives audio presentation.
- [ ] Water/sump state only drives audio presentation.
- [ ] Radio source is spatialized at canonical console.
- [ ] Level switching crossfades smoothly.
- [ ] Headless backend bypasses AudioServer safely.
- [ ] Steady-state audio tick meets allocation budget.
- [ ] Master limiter is configured and tested.
- [ ] All 11 cues validate spatial metadata.
- [ ] Shelter room scenes pass audio emitter lint.

- [ ] `E1_planintegration[30].md` is the next sequence filename.

# 28. Final Directive

This integration should make convoys more tactical, espionage more ambitious, survivors more psychologically
persistent, and the shelter more acoustically alive—without creating four parallel simulations.

A mounted heavy machine gun is still a Combat weapon.
A bullbar is still a VehicleGarage modification whose persistent damage belongs to the vehicle.
A mole is still the same survivor.
A stolen blueprint is still subject to the Research/Archive authority.
A growth trait is still one adaptation inside the canonical psychological/trait model.
A rockfall rumble is still only a sound generated from structural state that another system owns.

The final standard is:

**Combat resolves combat; VehicleGarage owns persistent vehicle state; world/route systems create encounter
opportunities; ShelterEspionage owns covert operation state while Faction/Research/Expedition own consequences;
E1-15 owns psychological recovery and relapse; ShelterAcousticBridge projects canonical shelter state and never
becomes gameplay authority.**

If any implementation duplicates those authorities, consumes shared RNG order, replays after save/load, exposes
hidden truth through presentation, or turns audio into simulation state, stop and repair the boundary before
proceeding.
