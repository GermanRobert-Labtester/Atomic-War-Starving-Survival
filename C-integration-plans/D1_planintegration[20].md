# D1 Flagship Integration Plan [20]
## Plan 207 — Maritime & Underwater Exploration Expansion

> **Canonical filename:** `D1_planintegration[20].md`
>
> **Previous:** `D1_planintegration[19].md`
>
> **Next:** `D1_planintegration[21].md`
>
> **Purpose:** Expand ASHFALL's existing self-contained diving mini-game into one coherent maritime exploration
> domain that supports discoverable coastal/ocean zones, multiple dive sites, expedition planning, underwater
> routes, environmental hazards, equipment progression, finite salvage, sunken structures, flooded bunkers,
> and cross-system consequences—without duplicating the existing expedition, combat trauma, radiation,
> inventory, equipment-condition, safe-cracking, weather, or dive-runtime authorities.
>
> **Primary source:** Plan 207 — Maritime & Underwater Exploration Expansion.
>
> **Core repository problem:** `MaritimeDiveSystem.cs` already provides a contained four-room dive gameplay
> loop with air/noise/loot management, `SafeCrackingSystem.cs` provides safe-opening gameplay, and
> `dive_sites.json` already exists. What is missing is the orchestration layer around those mechanics:
> discoverable maritime zones, multiple persistent dive sites, route/access planning, finite salvage,
> environmental conditions, diver/equipment eligibility, maritime expedition state, hazard handoff,
> and a durable world-facing UI.
>
> **Implementation posture:** deterministic, catalog-driven, finite-resource aware, expedition-integrated,
> save-safe, and layered. `MaritimeExplorationSystem` owns world-level maritime discovery/progression and
> orchestration; `MaritimeDiveSystem` remains the authoritative dive-session mechanic; `ExpeditionSystem`
> remains the expedition/travel authority; `RadiationSystem`, `CombatTraumaSystem`, `EquipmentConditionSystem`,
> `Inventory`, and `SafeCrackingSystem` remain authoritative over their respective consequences.
>
> **Critical guardrail:** Plan 207 must not create a second expedition engine or replace the existing dive
> mini-game with an incompatible parallel runtime. The flagship architecture treats the existing dive system as
> a reusable encounter/session primitive embedded inside a broader maritime exploration loop.
---

## 1. Source Problem Statement

The source identifies a clear progression gap:

- `MaritimeDiveSystem.cs` exists, but it is a single self-contained mini-game;
- `SafeCrackingSystem.cs` exists and can already support secured underwater caches/flooded structures;
- `dive_sites.json` exists as data;
- no broader maritime exploration authority exists;
- no zone catalog/management exists;
- no persistent dive-site world progression exists;
- no maritime expedition planning exists;
- no underwater route/access layer exists;
- no generalized current/pressure/contamination hazard orchestration exists;
- no world-facing equipment progression exists;
- no coastal-zone management exists.

The target architecture is:

```text
World / Intelligence / Expedition discovery
                  ↓
       MaritimeExplorationSystem
        ┌─────────┼─────────────┐
        ↓         ↓             ↓
   maritime    dive sites    access state
    zones       + salvage    + equipment gates
        └─────────┼─────────────┘
                  ↓
        Maritime Expedition Plan
                  ↓
          ExpeditionSystem
                  ↓
       MaritimeDiveSystem session
       ├─ air/noise/navigation
       ├─ room exploration
       └─ local dive actions
                  ↓
       authoritative consequence sinks
       ├─ Inventory / loot
       ├─ EquipmentCondition
       ├─ Radiation
       ├─ CombatTrauma
       ├─ SafeCracking
       └─ survivor fate/lifecycle
                  ↓
      site/zone progression update
```

The new system is therefore a world-domain orchestration layer, not a replacement for the existing dive-session
mechanics.
---

## 2. Flagship Success Criteria

The implementation is complete only when all of the following are true:

1. `MaritimeExplorationSystem.cs` exists with schema-versioned capture/restore.
2. Existing `MaritimeDiveSystem` remains the authoritative active dive-session mechanic.
3. Existing `ExpeditionSystem` remains the expedition scheduling/travel authority.
4. Dive sites have stable IDs and persistent discovery/exploration/salvage state.
5. Maritime zones have stable IDs and persistent world-discovery state where needed.
6. Existing `dive_sites.json` is audited and either promoted into the canonical site catalog or migrated cleanly.
7. `maritime_zones.json` becomes the zone/environment authority without duplicating individual site state.
8. Site type, depth, access requirements, hazard profiles, loot/deposit references, and structural topology are data-backed.
9. Maritime expedition planning uses canonical survivor IDs and equipment instance IDs.
10. Divers are validated against real survivor availability, condition, skill/capability, and assignment state.
11. Diving equipment uses the canonical inventory/equipment-condition system.
12. Depth rating is validated before dive launch.
13. Hazard protection is capability/profile based rather than one generic `protectionLevel` for every hazard.
14. Radiation hazard routes to `RadiationSystem`.
15. Injuries route to `CombatTraumaSystem`/medical authorities.
16. Equipment degradation routes to `EquipmentConditionSystem`.
17. Equipment loss removes or relocates the real item instance transactionally.
18. Loot acquisition routes through canonical inventory transactions.
19. Safe cracking in flooded bunkers reuses `SafeCrackingSystem`.
20. Site salvage is finite and cannot reroll infinite unique loot.
21. Unique wreck/bunker loot is exactly-once.
22. Repeatable common salvage, if allowed, uses explicit replenishment/depletion rules.
23. Discovery cannot be farmed by repeatedly reopening the same site.
24. Site progression survives save/load.
25. Expedition/dive outcomes cannot reroll through save/load.
26. Hazard rolls use stable seeds tied to expedition/site/hazard opportunity.
27. Zone environmental changes are derived from real seasonal/weather/radiation inputs where possible.
28. The UI never owns expedition eligibility, hazard, or loot rules.
29. Old saves preserve current dive behavior and begin with no fabricated discoveries.
30. `--maritime-exploration-selftest` proves catalogs, site discovery, planning, equipment gating, dive handoff,
    hazards, finite salvage, safe cracking, save/load, migration, and headless operation.
---

## 3. Repository Reconnaissance Before Editing

Create:

`docs/maritime/MARITIME_EXPLORATION_INTEGRATION_AUDIT.md`

Inspect at minimum:

- `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs`
- `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs`
- `Assets/StreamingAssets/Data/dive_sites.json`
- `Assets/Ashfall.Core/ExpeditionSystem.cs`
- expedition destination/location catalogs
- survivor assignment/availability
- survivor skills/proficiencies
- `EquipmentConditionSystem`
- Inventory/item instances
- diving equipment item definitions
- loot/scavenging tables
- `RadiationSystem`
- `CombatTraumaSystem`
- disease/contamination systems
- weather/season system
- world topology/coastal locations
- map/discovery systems
- quest/journal/archive systems
- save schema/migrations
- deterministic RNG
- UI expedition/map panels
- safe/crate/locked-container content APIs

Build an authority matrix:

| Concern | Existing authority | New maritime role |
|---|---|---|
| active dive air/noise | MaritimeDiveSystem | configure/launch/consume result |
| expedition schedule/travel | ExpeditionSystem | maritime destination adapter |
| loot transaction | Inventory/Loot | select/deplete site pool, commit result |
| radiation dose | RadiationSystem | emit exposure |
| injury | CombatTrauma/Medical | emit injury request |
| equipment condition | EquipmentCondition | consume wear/damage |
| safe opening | SafeCrackingSystem | invoke at secured underwater object |
| survivor death | SurvivorLifecycle | consume canonical result |
| world weather | WeatherSystem | derive maritime conditions |
| site discovery | MaritimeExplorationSystem | own run-local state |

Do not code a second expedition scheduler until the audit proves the existing one cannot represent maritime
missions.
---

## 4. Scope Boundary

### In scope

- maritime zones;
- persistent dive-site discovery/progression;
- site access requirements;
- maritime expedition plan/adapters;
- diver eligibility;
- equipment loadout validation;
- environmental condition profiles;
- currents/pressure/contamination/collapse/entrapment/equipment-failure hazard opportunities;
- finite underwater salvage;
- wreck/flooded-bunker exploration;
- safe-cracking bridge;
- UI/map;
- save/load/migration;
- CI/selftests.

### Out of scope for first pass

- replacing the existing four-room dive gameplay;
- full naval combat;
- ship piloting simulation;
- global ocean-current simulation;
- realistic decompression medicine;
- oxygen partial-pressure modeling;
- real dive-table simulation;
- procedural bathymetry;
- underwater base construction;
- autonomous marine ecosystem simulation;
- separate maritime survivor roster;
- cross-campaign wreck persistence.

The domain should feel broad without becoming a separate game engine.

---
## 5. Existing Dive Mini-Game as Session Primitive
- `MaritimeDiveSystem` remains the low-level dive-session authority for air/noise/navigation/room interactions.
- MaritimeExplorationSystem supplies site/session configuration and consumes a normalized result.
- Do not copy the four-room logic into a new site runtime.
- Where the current mini-game is too hardcoded, extract configuration seams before building orchestration.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 6. Dive Session Adapter
- Define an adapter such as `IMaritimeDiveSessionRunner` that accepts site/session configuration and returns a deterministic result DTO.
- Inputs may include room layout/profile, depth, visibility, current modifier, diver IDs, equipment capabilities, and seeded session ID.
- Result contains explored nodes, recovered loot refs, hazard outcomes, abort reason, remaining air/resource state, and safe-cracking opportunities.
- The adapter lets existing code evolve without making world exploration depend on concrete UI classes.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 7. Site Definition vs Site State
- Separate immutable catalog data from run-local progression.
- `DiveSiteDefinition` stores site type, depth, zone, hazard profile, salvage profile, access requirements, layout/session profile, unique content, and coordinates/location reference.
- `DiveSiteState` stores discovery, exploration, depletion, first/last visit, exploration count, one-time event IDs, and remaining finite resource state.
- Do not persist static site names/loot tables into the save.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 8. Dive Site State Machine
- Recommended progression: undiscovered → discovered → surveyed/explored → partially_salvaged → fully_salvaged.
- Do not use one ambiguous `explored` boolean.
- Some sites may remain revisit-able after full salvage for quests/hazards but yield no finite loot.
- State transitions are monotonic unless a regeneration mechanic explicitly exists.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 9. Site Discovery
- Discovery can originate from expeditions, intelligence/rumors, map events, radio/faction information, or adjacent zone survey.
- Each discovery has a stable source event ID.
- Repeated discovery events are idempotent.
- Discovery does not automatically reveal all hazards/loot details.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 10. Knowledge vs World State
- Distinguish what exists from what the player knows.
- A discovered site may reveal type/location but not exact hazard profile or loot.
- Surveying can reveal depth/current/contamination estimates.
- Do not expose hidden rare loot through UI simply because the catalog contains it.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 11. Maritime Zone Definition
- Zone definition contains zoneId, localized name, zone type, world/coastal location references, environmental profile refs, contained site IDs, and access profile.
- Do not persist live water temperature/radiation/current values if they are derived.
- Zones can have discovery state if the world map does not already own it.
- Site membership is catalog data.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 12. Zone Types
- Retain coastal, estuary, open_ocean, deep_trench, and underwater_ridge as source targets.
- Only author zones that map to actual ASHFALL geography/world locations.
- Do not create empty zone types solely to satisfy a count.
- Zone type is taxonomy, not behavior-hardcoded switch logic.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 13. Site Types
- Retain six source families: coastal_shallows, deep_ocean, sunken_vessel, underwater_cave, contaminated_zone, flooded_bunker.
- Each type should map to distinct session/hazard/salvage patterns.
- Do not treat type as a giant behavior switch; use profile references.
- Site-specific overrides remain data.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 14. Coastal Shallows
- Low depth, broad accessibility, common salvage, low/moderate environmental hazard.
- Good tutorial/early maritime content.
- Should integrate seamlessly with standard expedition travel.
- Do not make it hazard-free by definition.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 15. Deep Ocean
- High depth and equipment gating.
- Strong currents/low visibility/pressure profiles can combine.
- Rare salvage can justify risk.
- Do not model real decompression tables unless the medical/game systems intentionally support them.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 16. Sunken Vessel
- Structured exploration, compartment/session layout, unique finite salvage, potential entrapment/collapse.
- Can expose locked safes/containers through SafeCrackingSystem.
- Ship identity/unique finds may feed journal/archive.
- Do not respawn unique cargo.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 17. Underwater Cave
- Navigation/entrapment/visibility hazard emphasis.
- Session profile may use branching rooms/nodes if MaritimeDiveSystem can support it.
- If current mini-game only supports fixed rooms, first create data-driven room graph support rather than writing a separate cave engine.
- Hidden salvage remains finite.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 18. Contaminated Zone
- Radiation/contamination profile is authored but actual dose/exposure routes to canonical systems.
- Hazmat diving capability gates access/mitigates exposure.
- Do not maintain a maritime-specific radiation meter.
- Valuable materials can make risk strategic.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 19. Flooded Bunker
- Combines maritime access with pre-war structure exploration.
- Locked safes/doors invoke SafeCrackingSystem where compatible.
- Interior salvage is finite.
- Once water-access hurdle is crossed, structure logic can reuse existing encounter/content primitives.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 20. Maritime Environment Snapshot
- At expedition/dive start, resolve zone/site environmental conditions from canonical inputs.
- Snapshot can include water-temperature band, current strength, visibility, radiation/contamination context, and storm/surface conditions.
- Persist the snapshot or the deterministic inputs if the mission spans save/load.
- Do not let weather reroll mid-session unless the expedition simulation explicitly advances it.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 21. Water Temperature
- Treat temperature as gameplay band unless a real thermal/exposure system consumes exact Celsius.
- Cold water can affect endurance/equipment suitability if existing needs/health systems support it.
- Do not create hypothermia as a duplicate status.
- If no consumer exists, use temperature primarily for access/hazard flavor until integrated.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 22. Current Strength
- Current affects travel/air/time/hazard opportunity through the dive-session adapter.
- Use deterministic profile values plus zone/weather modifiers.
- Strong current can increase air consumption or navigation difficulty only if MaritimeDiveSystem exposes those seams.
- Do not directly push survivors around outside the dive runtime.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 23. Visibility
- Visibility can affect route discovery, hazard avoidance, loot spotting, or dive-session information.
- Do not hide UI randomly; use deterministic session modifiers.
- Underwater light/sonar can mitigate if equipment capabilities exist.
- Keep player-facing explanation clear.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 24. Pressure/Depth
- Depth is a site definition fact.
- Equipment depth rating validates access and hazard susceptibility.
- Pressure injury is modeled as a hazard/result routed to medical/trauma authority.
- Do not implement real-world decompression calculations as medical guidance.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 25. Radiation/Contamination
- Zone/site profile supplies exposure context.
- RadiationSystem/Disease/Contamination authority handles actual survivor consequences.
- Hazmat dive suits provide protection through equipment capability adapters.
- Do not duplicate contamination state in MaritimeExplorationState.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 26. Seasonal Conditions
- Zone conditions may vary by season only through Weather/season adapters.
- Do not persist independent random seasonal values when they can be derived.
- Storm periods may change current/visibility/surface travel risk.
- Same campaign state produces same snapshot.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 27. Maritime Expedition Plan
- Represent maritime-specific planning data as an extension/wrapper around ExpeditionSystem, not a second mission scheduler.
- Plan fields: targetSiteId, diver IDs, equipment instance IDs, optional objectives, estimated duration, access validation result.
- ExpeditionSystem owns planned/in_progress/completed/failed/aborted lifecycle where possible.
- Maritime state stores its domain-specific mission record keyed to canonical expedition ID.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 28. Canonical Expedition ID
- Every maritime mission should have one canonical expedition instance ID.
- Use that ID in hazard seeds, loot transactions, dive-session ID, save state, and quest events.
- Do not create unrelated maritime and expedition IDs without a stable mapping.
- This is central to idempotency.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 29. Travel + Dive + Surface Phases
- Model maritime mission as phases layered on the expedition framework: travel_to_site → dive_session → surface/return.
- Travel uses ExpeditionSystem.
- Dive uses MaritimeDiveSystem.
- Return/aborted state returns through ExpeditionSystem.
- Do not independently decrement world travel time in MaritimeExplorationSystem.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 30. Diver Eligibility
- Validate survivor alive/present/not incapacitated/not assigned elsewhere.
- Require diving proficiency/capability only if canonical skill system defines it.
- Medical conditions may affect eligibility through functional capability adapters, not hardcoded condition names.
- UI only displays validation result.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 31. Diving Skill
- Audit whether a diving skill already exists.
- If not, prefer a relevant survival/athletics/technical capability before inventing a new skill solely for this plan.
- If a dedicated diving proficiency is justified, add it through the canonical SkillProgressionSystem.
- MaritimeExplorationSystem must not own XP.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 32. Team Size
- Site/session profile defines min/max divers if needed.
- More divers can increase carrying capacity/redundancy but also consume equipment/air.
- Do not hardcode one universal party size.
- Existing expedition party constraints still apply.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 33. Equipment Authority
- Diving gear is represented by real item/equipment instances.
- Condition, ownership, repair, loss, and destruction remain in Inventory/EquipmentConditionSystem.
- MaritimeExplorationSystem stores references/reservations only.
- Do not duplicate equipment condition inside a persistent `DivingEquipment` DTO if a canonical item already has it.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 34. Diving Equipment Capability Model
- Replace one generic `protectionLevel` with capability tags/ratings.
- Examples: max_depth, radiation_protection, contamination_seal, air_duration/rebreather, illumination, sonar, cutting, salvage_capacity.
- Specific hazards query relevant capabilities.
- One suit should not automatically protect against every hazard.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 35. Basic Dive Suit
- Supports shallow/low-risk dives.
- Has explicit depth/seal capability.
- Condition affects reliability if EquipmentConditionSystem supports it.
- No magical universal protection.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 36. Deep Dive Suit
- Raises depth capability and may reduce pressure hazard.
- Can be heavier/costlier if equipment systems support tradeoffs.
- Does not automatically provide radiation protection.
- Distinct from hazmat dive suit.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 37. Hazmat Dive Suit
- Provides radiation/contamination protection profile.
- Depth rating still matters separately.
- Protection degrades only through real equipment condition rules.
- RadiationSystem consumes effective protection.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 38. Rebreather/Air Equipment
- MaritimeDiveSystem already tracks air; integrate equipment-derived capacity/efficiency through a clear adapter.
- Do not create a second persistent air resource in world state.
- Session result can include consumable usage/remaining equipment state.
- Refill/repair belongs to equipment/inventory systems.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 39. Underwater Light
- Provides visibility/navigation benefit in dark sites.
- Condition/battery/power only if item systems support it.
- Do not create a bespoke battery meter solely for maritime if generic equipment power exists.
- Session adapter consumes capability.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 40. Sonar
- Can improve site survey/navigation/loot or hazard detection where authored.
- Does not reveal exact hidden unique loot automatically.
- Use deterministic discovery improvements.
- Research/crafting owns acquisition.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 41. Cutting Torch
- Enables access to designated blocked compartments/containers.
- Use real item condition/fuel if systems exist.
- Does not replace SafeCrackingSystem for locks/safes.
- Site data marks cutting-required interactions.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 42. Salvage Bag/Capacity
- Can increase underwater carry/recovery capacity.
- Inventory/expedition capacity remains authority.
- Maritime system can expose a temporary recovery-capacity modifier.
- Do not duplicate carried inventory.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 43. Equipment Reservation
- Planning reserves selected item instances for the expedition.
- Cannot simultaneously assign same suit/tool to two missions.
- Cancellation releases reservation.
- Save/load preserves reservation exactly.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 44. Equipment Preflight Validation
- Before launch, validate depth rating, required protection, air system, lights/tools, item condition, and ownership.
- Return structured warnings/errors.
- Do not wait until dive start to discover impossible loadout if known.
- Emergency player override only where design allows.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 45. Equipment Wear
- Wear is computed from real dive duration/hazard events but applied by EquipmentConditionSystem.
- Different hazards can request different damage types/severity.
- Normal use degradation should be bounded.
- Do not write condition directly in MaritimeExplorationSystem.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 46. Equipment Failure
- Equipment failure hazard should be based on actual equipment condition/profile.
- Stable hazard opportunity ID prevents reload rerolls.
- Failure consequence routes to equipment system and dive session.
- Do not destroy items by setting a maritime-only flag.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 47. Equipment Loss
- Loss event removes/transfers the real item instance from expedition inventory transactionally.
- If recoverable, associate it with site salvage/recovery state via stable item ID.
- Do not clone lost equipment.
- Recovery follow-on can reuse item provenance from Plan 190.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 48. Hazard Definition
- Use a data-backed hazard catalog/profile, not one runtime DTO as sole definition.
- Source target hazards: strong_current, underwater_collapse, radiation_hotspot, entrapment, pressure_depth, contaminated_water, marine_creature, equipment_failure.
- Each hazard defines trigger conditions, severity curve, avoidance inputs, and consequence intent.
- Only hazards with real consequence sinks should ship.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 49. Hazard Opportunity Model
- Hazards arise from site/zone/session state at defined checkpoints, not arbitrary per-frame rolls.
- Examples: entering deep compartment, crossing current, opening unstable door, lingering in hotspot.
- Each opportunity has stable ID.
- Same expedition/session seed yields same opportunity outcome.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 50. Hazard Severity
- Compute from authored base severity plus environmental/site state and mitigating equipment/skill.
- Use bounded deterministic arithmetic.
- If avoidance outcome needs RNG, use stable seed.
- Do not let UI order affect results.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 51. Strong Current Hazard
- May increase air/time cost, force abort, separate path access, or cause equipment loss if dive runtime supports it.
- Do not directly injure unless a validated outcome says so.
- Mitigation: diver capability/equipment/route choice.
- Current value comes from environment snapshot.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 52. Underwater Collapse
- Triggered at authored unstable structures/compartments.
- May block route, cause injury, entrapment, or terminate salvage.
- CombatTrauma/Medical owns injury.
- Collapsed structural state can persist if site design supports it.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 53. Entrapment
- Represent as dive-session navigation/emergency event.
- Cutting tool/team support can mitigate where authored.
- Failure can force air/time costs or trauma.
- Do not create a new survivor captivity system.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 54. Pressure/Depth Hazard
- Triggered by exceeding safe equipment/capability thresholds or authored deep site profile.
- Prefer preflight denial for clearly impossible depth.
- Residual risk can remain within allowed range.
- Medical consequence is abstract and routed canonically.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 55. Contaminated Water Hazard
- Uses contamination/radiation exposure APIs.
- Suit seal/protection mitigates.
- Do not duplicate disease or dose state.
- Persist only hazard event/result reference.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 56. Marine Creature Hazard
- Only implement where Bestiary/ecology/combat systems provide actual creatures.
- Do not create a maritime-only random creature damage table if CombatSystem can host the encounter.
- Maritime system can surface encounter opportunity/context.
- Species behavior remains ecology/combat authority.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 57. Hazard Outcome Contract
- Normalized outcomes may include avoided, inconvenience, minor injury request, major injury request, abort, equipment damage, equipment loss, fatal outcome request.
- Do not let MaritimeExplorationSystem directly set survivor health or death if canonical lifecycle exists.
- Fatal means the medical/fate authority confirms death.
- Persist confirmed outcome IDs.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 58. Fatal Hazard Guardrail
- Do not assign `fatal` from one roll and delete the survivor immediately.
- Route a lethal consequence to authoritative health/fate handling.
- If death is deterministic in the current expedition architecture, use that existing API.
- History/journal records confirmed fate.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 59. Hazard Logging
- Persist only meaningful encountered hazard events.
- Do not store every avoided low-level opportunity forever.
- Aggregate statistics can support quests.
- Stable event IDs prevent duplicate journal entries.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 60. Underwater Loot Authority
- Site definition references canonical loot/scavenging tables.
- Inventory owns granted item instances.
- MaritimeExplorationSystem owns finite site salvage availability/selection ledger.
- Do not store raw duplicated item definitions in site state.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 61. Finite Salvage Ledger
- For each site, persist depletion state or consumed unique salvage IDs.
- Use deterministic site/expedition seed to select from remaining eligible resources.
- Once unique item is taken, it cannot reroll.
- Common salvage may have finite quantities or explicit replenishment profile.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 62. Unique Loot
- Sunken vessels/flooded bunkers can contain authored unique item IDs/reward refs.
- Grant transaction ID is persisted before/with item creation.
- Reload cannot duplicate.
- UI should not reveal exact unique loot before discovery unless intelligence provides it.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 63. Common Loot
- Use weighted tables against a finite resource budget/stock.
- Repeated exploration should yield diminishing/empty results as site depletes.
- Do not roll infinite common loot simply because the table is weighted.
- Site fully_salvaged threshold derives from remaining finite stock/content.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 64. Resource Deposits
- If underwater deposits exist, model as explicit finite deposit records or references.
- Extraction requires tools/time.
- Inventory receives resulting resources.
- Do not create infinite ore nodes unless a regeneration mechanic is designed.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 65. Salvage Capacity
- Recovered loot is limited by expedition carry/salvage capacity.
- Selection can prioritize player-marked items if UI supports it.
- Unrecovered discovered items may remain in site state.
- Do not silently delete overflow unique loot.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 66. Partial Salvage
- A dive can discover more loot than can be recovered.
- Site state records known/unrecovered salvage where needed.
- Return expedition carries only committed recovered items.
- This supports meaningful revisits.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 67. Site Depletion
- Fully salvaged means all finite salvage objectives/resources are exhausted or inaccessible.
- Exploration count alone should not automatically deplete a site.
- Source's 'after enough exploration' should become content-based depletion.
- Some environmental/quest interactions may remain after depletion.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 68. Safe-Cracking Bridge
- Flooded bunker/wreck site data can expose a locked-safe interaction with stable safe ID and loot reference.
- Invoke `SafeCrackingSystem` with appropriate context.
- SafeCrackingSystem owns puzzle/noise/tool outcome.
- Maritime system marks the safe opened only after canonical success.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 69. Safe-Cracking Idempotency
- Opened safe state must persist.
- Reload cannot reset combination/puzzle outcome if current safe system persists solved state.
- One safe cannot grant loot twice.
- Use root site interaction ID.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 70. Underwater Safe Noise
- SafeCrackingSystem's local noise remains its own mechanic.
- If relevant to diving hazard, expose a normalized session noise contribution through adapter rather than merging fields.
- Do not conflate shelter acoustics from Plan 205.
- Domain boundaries remain explicit.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 71. Route/Access Model
- Each site has world access requirements and possibly internal dive-session access gates.
- World route uses ExpeditionSystem/world topology.
- Internal route uses MaritimeDiveSystem/site layout.
- Do not create a separate global underwater route graph unless world data requires it.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 72. Coastal Zone Access
- Shallow coastal sites may be reachable from land expedition destinations.
- Open-ocean sites may require boat/vehicle capability only if a maritime transport system exists.
- Do not invent boats as invisible free transport.
- Unimplemented transport requirements should gate content or be deferred.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 73. Boat/Surface Transport Boundary
- Plan 207 source does not define boats as a full system.
- Audit whether Expedition vehicles can represent maritime transport.
- If not, create a minimal surface-transfer capability only if required for target sites, or defer deep-ocean content.
- Do not smuggle a naval vehicle system into this plan.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 74. Travel Duration
- Travel duration belongs to ExpeditionSystem.
- Dive duration belongs to MaritimeDiveSystem/session configuration.
- Surface preparation/decompression time can be a simple maritime phase only if game-scale useful.
- Total mission ETA is projection from these components.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 75. Abort Semantics
- Abort can occur during preflight, travel, or dive.
- Preflight abort consumes nothing except explicitly spent setup resources.
- Dive abort preserves already-incurred wear/exposure/loot according to session result.
- Canonical Expedition status reflects final state.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 76. Failure Semantics
- Mission failure is not automatically diver death.
- Failure may mean no access, forced abort, lost equipment, injury, or stranded/other canonical consequence.
- Use typed reason codes.
- UI explains result.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 77. Exploration Progress
- Progress derives from visited/cleared site nodes, surveyed compartments, resolved interactions, and finite salvage—not arbitrary visit count alone.
- Store normalized progress as derived presentation where possible.
- Persist meaningful flags/interaction states.
- Do not let repeated entry into the same room farm progress.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 78. Site Session Layout
- If `MaritimeDiveSystem` is fixed to four rooms, refactor it toward data-driven nodes/rooms as prerequisite.
- Keep a compatibility profile reproducing the original four-room dive exactly.
- New wreck/cave/bunker sites can then supply different layouts.
- Regression-test original mini-game.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 79. Original Dive Compatibility
- Existing campaigns/content that invoke the current dive mini-game must continue to work.
- Create an adapter/site definition representing the legacy dive.
- Do not require broad maritime discovery state just to access legacy scripted content unless intended.
- Shadow-mode or compatibility tests should compare outcomes.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 80. Dive Site Catalog Migration
- Audit existing `dive_sites.json` rather than creating a second incompatible site catalog.
- Add schemaVersion and new fields while preserving stable IDs.
- Write loader migration/defaults for legacy entries.
- Data-integrity test all site references.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 81. Maritime Zone Catalog
- Create `maritime_zones.json` only for zone-level environment/access/grouping.
- Do not duplicate site loot/depth into zone file.
- Zones reference site IDs.
- Validator ensures each site belongs to valid zone or explicit standalone category.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 82. Catalog Schema Versioning
- Both site and zone data need schema versions.
- Stable IDs are save contracts.
- Display names use localization keys.
- Do not key saves to array position.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 83. Site Reachability Validation
- Every completion-eligible site must have a real discovery/access path.
- Validate world location, required equipment/transport, and expedition destination.
- Do not ship 15 sites that cannot be reached.
- Generate a reachability report.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 84. Zone Reachability Validation
- Every zone must contain at least one reachable site or be marked future/reserved.
- Deep sites requiring unimplemented transport must not block completion.
- UI should not advertise impossible access.
- CI fixture per active zone.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 85. Equipment Reachability Validation
- Every required equipment capability must be obtainable through inventory/crafting/research/content.
- Do not gate a site behind a nonexistent hazmat dive suit.
- Coverage report links item/recipe/research ID.
- Fail base content if impossible.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 86. Loot Reachability Validation
- Every unique loot reference must exist.
- Rare tables must use real item IDs.
- Quest-critical items require exactly-once safeguards.
- No orphan reward IDs.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 87. Maritime Discovery Sources
- Discovery adapters may include expedition proximity, intelligence/rumor system, faction information, map events, and authored quests.
- Do not implement all at once if those plans are absent.
- At least one deterministic discovery path per active site is mandatory.
- Record discovery provenance.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 88. Information Integration
- If Plan 131 information/rumor network exists, maritime site rumors can reveal suspected sites.
- Maritime system owns actual discovered/surveyed state.
- Information system owns claim certainty/source.
- Do not duplicate rumor propagation.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 89. Map Integration
- Maritime map is a projection over world/coastal map + zones/sites.
- Unknown sites stay hidden or suspected according to discovery state.
- Coordinates should be stable world/location references, not arbitrary UI pixels.
- UI may cluster nearby sites by zone.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 90. Zone UI
- Show known zone conditions, access requirements, discovered site count, and known hazards.
- Do not reveal exact hidden conditions unless surveyed/intelligence allows.
- Environmental values can display bands/forecasts if available.
- Use localization.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 91. Dive Site Detail UI
- Show site type, known depth, exploration status, remaining known salvage, hazard intelligence, access/equipment requirements, last visit, and available interactions.
- Unknown details remain unknown.
- Provide estimated risk from real validation, not hidden exact RNG probability.
- UI reads a projection.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 92. Expedition Planning UI
- Select target, divers, equipment, objectives, and departure time through the canonical expedition flow.
- Display validation errors and estimated duration.
- Reserve equipment transactionally.
- Do not implement a separate maritime calendar.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 93. Equipment UI
- Show relevant diving gear, condition, depth rating, hazard capabilities, reservation status, and repair links.
- Inventory remains source of truth.
- Do not list duplicate persistent `DivingEquipment` objects.
- Allow loadout templates only as UI convenience.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 94. Hazard Log UI
- Show encountered significant hazards, affected divers, confirmed outcome, site, and day.
- Do not show untriggered hidden hazard table probabilities unless discovered.
- Link injury/radiation record where possible.
- History is read-only.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 95. Loot Log UI
- Show recovered items, site, expedition, and whether unique if appropriate.
- Use inventory/item lore integration for provenance where Plan 190 exists.
- Do not duplicate inventory ownership.
- Site remaining loot display is knowledge-limited.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 96. Maritime Tutorial
- First maritime expedition explains site discovery, depth, equipment gates, hazards, finite salvage, and abort behavior.
- Use a shallow/tutorial site.
- Do not overload with every hazard type.
- Explain that the existing dive-session controls are reused.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 97. Tooltip Design
- Zone/site tooltips show known depth/risk/access and exploration state.
- Equipment tooltips show relevant capability effects.
- Do not hide critical access failure reasons behind hover-only UI.
- Support keyboard/controller focus.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 98. Accessibility
- Use text labels in addition to risk colors/icons.
- Support text scaling and keyboard/controller navigation.
- Do not rely on underwater animation to convey low air/hazard state.
- Site map should provide list alternative.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 99. Journal Integration
- Record notable discovery, unique wreck, flooded bunker, rare salvage, major hazard, and expedition loss/return.
- Do not journal every common dive.
- Use canonical event IDs.
- Maritime system exports facts; Journal owns entries.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 100. Quest Hooks
- Source hooks: Diver, Explorer, Salvager, Deep Diver, Wreck Hunter, Survivor, Treasure Hunter.
- Count unique expedition IDs, discovered site IDs, recovered item transactions, and confirmed hazard outcomes.
- Do not reward repeated entry without real progress.
- QuestSystem owns rewards.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 101. Achievement Integration
- Plan 149 may observe first deep dive, first wreck, zone completion, all active sites surveyed, rare salvage, or hazard survival.
- Use stable facts.
- Do not require death/unsafe play for completion.
- Maritime system does not own achievement state.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 102. Epilogue/Archive Integration
- Plan 145/archive can consume famous wreck discoveries, unique recoveries, major losses, or maritime exploration milestones.
- Export stable facts only.
- Do not serialize prose as canonical history.
- Site IDs remain stable.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 103. Item Lore Integration
- Plan 190 can record `recovered_from_siteId` provenance for unique/durable items.
- Maritime system supplies expedition/site context.
- ItemLore owns provenance history.
- Commodity salvage usually remains lore-free.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 104. Bestiary Integration
- Plan 187 can observe marine-creature encounters if actual creature systems are wired.
- Maritime does not create independent bestiary counts.
- Creature encounter uses canonical species ID.
- Site hazard result can link to encounter event.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 105. Health History Integration
- Plan 198 can record diving injury, radiation exposure, decompression-like medical outcome, or treatment episode via canonical medical events.
- Maritime itself does not write medical chart facts directly unless consuming confirmed event IDs.
- Health systems remain authoritative.
- Useful for long-term diver stories.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 106. Chronic Condition Integration
- Severe maritime injury can eventually cause chronic conditions through MedicalPipeline/Plan 193.
- Maritime system only emits acute consequence context.
- Do not create a maritime-specific disability flag.
- Condition onset remains medical authority.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 107. Equipment Condition Integration
- Apply wear/damage through `EquipmentConditionSystem` with source event ID and damage profile.
- Repairs use existing repair/crafting path.
- No shadow condition field.
- Save/load idempotency is mandatory.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 108. Radiation Integration
- Emit dose/exposure events with site/environment/protection context.
- RadiationSystem computes dose/consequence.
- Maritime state may retain hazard-event reference only.
- No duplicated radiation level on survivor.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 109. Combat Trauma Integration
- Emit injury intents/encounter results rather than direct HP changes.
- CombatTrauma/Medical owns injury records.
- Marine-creature combat can invoke CombatSystem if available.
- Site hazard log records confirmed outcome.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 110. Death/Fate Integration
- Fatal dive outcome is confirmed by canonical survivor lifecycle/fate authority.
- Expedition status then reflects loss.
- Equipment/body recovery can become future site interaction if supported.
- Do not remove survivor directly from MaritimeExplorationSystem.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 111. SafeCracking Integration
- Reuse safe-cracking for submerged/flooded locked containers.
- Provide environmental context only if SafeCracking accepts modifiers.
- Do not fork the safe-cracking minigame.
- Persist solved/opened interaction ID.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 112. Weather Integration
- Surface weather can affect departure/return and zone condition snapshots.
- Expedition/Weather systems own travel gating.
- Do not create a separate maritime weather forecast.
- Storm/current relationship is data-driven.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 113. Season Integration
- Season affects maritime conditions through canonical weather/environment sources.
- Do not randomize seasons inside maritime state.
- Frozen/iced access can be a zone access modifier if world supports it.
- UI forecast only if WeatherStation/forecast systems expose it.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 114. Location Evolution Integration
- If locations evolve, wreck accessibility or contamination can respond through typed world-state inputs.
- Maritime system should not independently mutate land location evolution merely because a site was explored.
- Site depletion is maritime-owned.
- Environmental transformations require explicit adapter.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 115. Finite Content Anti-Farming
- Unique content is exactly-once.
- Common salvage uses finite stock/budget.
- Discovery/exploration progress requires new interactions/areas.
- Hazard counts use unique encounter IDs.
- Save/load cannot reroll loot or hazards.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 116. Loot Seed Discipline
- Seed loot opportunities from campaign seed + site ID + salvage node ID + site revision, not current visit count alone.
- Persist consumed node IDs.
- Repeat visits cannot regenerate a different unique item.
- Common finite bundles can be deterministic per node.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 117. Hazard Seed Discipline
- Seed hazard opportunity from expedition ID + site ID + opportunity ID + hazard definition ID.
- Persist/derive result.
- Changing UI loadout after launch cannot reroll resolved opportunities.
- Preflight loadout legitimately affects unresolved future hazards.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 118. Discovery Seed Discipline
- Discovery from random survey/intelligence uses stable world/event seed.
- Opening map panel does not roll discovery.
- Repeat survey without new time/opportunity cannot reroll.
- Discovery result persists.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 119. Exploration Count Semantics
- `explorationCount` counts completed distinct dive sessions, not UI openings or aborted preflight.
- It can be useful for statistics but should not be the sole progression/depletion driver.
- Stable expedition/dive IDs prevent duplicates.
- Quest counts decide whether aborted dives qualify.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 120. Maritime Event Retention
- Persist/emit meaningful events: site discovered, expedition launched/completed/aborted, major hazard, unique salvage, wreck/bunker milestone, diver loss.
- Do not log every ordinary item pickup forever.
- Aggregate counts separately.
- Journal consumers choose narrative importance.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 121. State DTO Design
- Persist discovered site state, finite salvage ledger, opened interactions, maritime expedition domain metadata, unresolved transient dive/session link if needed, and migration/schema data.
- Do not persist static zone/site catalog copies.
- Do not persist duplicate equipment lists.
- Derive zone environmental values when possible.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 122. Active Expedition Persistence
- If save occurs mid-maritime expedition, canonical ExpeditionSystem state is primary.
- Maritime state stores link to target site and session-specific deterministic inputs/results needed to resume.
- Do not launch a second mission on restore.
- Equipment reservations remain consistent.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 123. Mid-Dive Save Policy
- Audit whether MaritimeDiveSystem supports mid-session save.
- If yes, extend its own state and link session ID.
- If no, explicitly define save checkpoint restrictions or serialize enough dive state safely.
- Do not pretend world-level persistence solves mini-game session persistence.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 124. Save/Load After Loot Recovery
- Grant transaction commits exactly once.
- Site salvage ledger marks consumed node/content atomically with inventory grant.
- Reload cannot duplicate recovered item.
- Use two-phase/idempotent transaction pattern if necessary.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 125. Save/Load After Hazard
- Hazard outcome/result ID persists before/with consequence handoff.
- Reload cannot reroll equipment loss/injury.
- If downstream medical transaction is pending, resume idempotently.
- Journal notification does not create state.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 126. Old-Save Compatibility
- Existing saves begin with no newly discovered maritime sites unless old data proves otherwise.
- Legacy scripted/current dive remains accessible through compatibility adapter.
- No fabricated exploration counts or salvage depletion.
- Migration is silent.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 127. Existing Dive Site Data Migration
- Preserve stable site IDs from current `dive_sites.json`.
- Add missing fields with deterministic defaults/configured migration.
- Do not rename IDs casually.
- Write migration tests for current file schema.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 128. No-Sites Edge Case
- System loads with zero active sites and behaves like current game.
- Legacy dive content still functions if authored separately.
- UI shows empty/locked maritime layer gracefully.
- No crashes or divide-by-zero.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 129. Many-Sites Edge Case
- 50–100 sites across many zones must remain performant.
- Map filtering/search works.
- Site state remains sparse.
- No per-frame site hazard calculations.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 130. All-Sites-Depleted Edge
- Maritime layer remains usable for travel/quests/known history even when finite salvage exhausted.
- Do not respawn loot automatically.
- UI clearly marks fully salvaged sites.
- Follow-on content can add regenerated ecological resources explicitly.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 131. Insufficient Equipment Edge
- Preflight returns clear unmet requirements.
- No expedition launch.
- No resource/time consumption.
- UI suggests exact missing capability, not hidden item ID only.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 132. Diver Incapacitated Edge
- Eligibility updates before launch.
- If diver becomes incapacitated after planning but before departure, canonical expedition validation blocks/updates mission.
- Equipment reservation remains correct.
- No ghost diver.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 133. Equipment Breaks Mid-Dive Edge
- Apply damage, update capability if session supports dynamic equipment state, and resolve hazard/abort accordingly.
- Do not continue using destroyed gear invisibly.
- Persist result.
- Repair later through canonical system.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 134. Unique Loot Overflow Edge
- If carry capacity cannot take unique item, leave it discovered/unrecovered at site rather than deleting it.
- Site state records the known object.
- Future mission can recover it.
- No duplication.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 135. Aborted Dive Edge
- Explored nodes/hazards/wear already incurred remain.
- Unrecovered loot remains.
- Exploration progress can advance partially.
- Expedition result is aborted rather than magically rolled back.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 136. Zone Condition Change Edge
- A future revisit resolves new weather/season/environment snapshot.
- Site structural/salvage state persists.
- Do not retroactively rewrite previous hazard logs.
- Forecast UI may show anticipated conditions if available.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 137. Maritime Map Projection
- Use zone/site discovery state and canonical world coordinates.
- Projection includes known status, access, risk band, last exploration, and expedition state.
- Do not store UI coordinates in core state unless map system requires them.
- No hidden catalog leakage.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 138. Site Risk Projection
- Risk is derived from known site hazards + current/forecast environmental snapshot + selected loadout.
- Display band rather than exact hidden roll probability.
- Unknown hazards remain uncertain.
- Recalculate when loadout/conditions change.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 139. Loadout Validation Projection
- Return structured `required`, `satisfied`, `warning`, `blocked` entries.
- UI does not contain rules like `if depth > 20 then deep suit`.
- Core/data define capabilities.
- This makes mods/data changes safe.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 140. Maritime Completion Metrics
- Derive discovered sites / eligible sites, surveyed zones, fully salvaged sites, unique wrecks explored.
- Do not hardcode `/15` or `/5` in UI.
- Reserved/unreachable content excluded explicitly.
- Quest completion uses active catalog.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 141. Site Eligibility for Completion
- Add `eligibleForMaritimeCompletion` if needed.
- Tutorial/scripted/future/mod sites do not silently block completion.
- Validator reports active denominator.
- Stable across save catalog fingerprint rules.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 142. Reachability Report
- Generate `docs/maritime/MARITIME_REACHABILITY.md`.
- For each site: zone, world access, discovery source, required equipment, required transport, unique content, test fixture.
- Flag impossible base-game sites.
- Do not claim 15+ complete sites without this report.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 143. Equipment Coverage Report
- Generate `docs/maritime/DIVING_EQUIPMENT_COVERAGE.md`.
- Map each required capability to item IDs, acquisition/recipe/research, condition system integration, and tests.
- Flag dead equipment definitions.
- Ensure no site needs impossible gear.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 144. Hazard Coverage Report
- Generate `docs/maritime/MARITIME_HAZARD_COVERAGE.md`.
- For each hazard: trigger, source conditions, mitigation, downstream consequence owner, event ID, fixture.
- Remove hazards with no real sink.
- Marine creatures require actual combat/ecology integration.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 145. Loot Coverage Report
- Generate `docs/maritime/MARITIME_SALVAGE_COVERAGE.md`.
- Map site salvage tables, unique rewards, finite budgets, inventory transaction IDs, and depletion behavior.
- Flag missing items or infinite unique rolls.
- Audit quest-critical loot separately.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 146. Site Content Quality
- 15+ sites should be mechanically distinct, not 15 copies with changed loot percentages.
- Vary layout, access, hazard combination, narrative, salvage structure, and unique interactions.
- Start with fewer strong sites if necessary, then expand.
- Catalog quota is secondary to reachability and differentiation.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 147. Zone Content Quality
- 5+ zones should change planning through environment/access, not just names.
- Coastal shallows, estuary, open ocean, deep trench, ridge should only exist if geography/content support them.
- Each active zone needs a gameplay reason.
- Document environmental profile.

Implementation consequence: treat this section as a concrete integration contract. The source system, stable IDs, save semantics, deterministic event/result path, downstream owner, and UI projection must all agree before the feature is considered wired.

---
## 148. Maritime Narrative Hooks
- Use discovery provenance, wreck identity, survivor losses, unique salvage, and repeated diver history for stories.
- Do not create a separate maritime narrative state when Journal/Archive/ItemLore already exist.
- Export structured facts.
- Keep routine dives concise.
