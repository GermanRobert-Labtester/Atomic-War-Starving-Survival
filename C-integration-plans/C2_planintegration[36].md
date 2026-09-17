# C2 — Flagship Integration Plan [36]: Underground Tunnel Network, Subterranean Exploration, Maintenance, Hazards, and Alternative Travel

> **Deliverable:** `C2_planintegration[36].md`
> **Source scope:** Plan 167 — *Underground Tunnel Network System*
> **Primary objective:** create a deterministic underground network of tunnel segments, junctions, buried infrastructure, resource deposits, hidden bunkers, and alternative travel routes that can be discovered, explored, mapped, maintained, repaired, and used for expeditions—while routing travel, survivor skills, radiation, flooding, equipment, mining, thermal influence, location discovery, and injuries through the systems that already own those facts.
> **Required execution order:** **167A Foundation/System Contract → 167B Exploration, Maintenance, Travel, Hazards & Discoveries → 167C Cross-System Integration, Save/CI, Exploit Control, Balance, and Long-Run Closure**
> **Hard dependencies:** `ExpeditionSystem`, Plan 32 world topology/knowledge semantics, Plan 163 cartography where underground mapping consumes shared map-knowledge concepts, `LocationEvolutionSystem`, `SkillProgressionSystem`, `SaltMineExtractionSystem`, canonical equipment/condition systems, Weather/WeatherCascade, `ShelterThermalSystem`, radiation/decontamination, flooding/pumping, survivor health/medical/fate, Plan 31 semantic events, Plan 36 port-contract discipline, Plan 39 save durability, Plan 55 retention.
> **Scope discipline:** no second expedition scheduler, no duplicate survivor skill ledger, no duplicate equipment durability, no duplicate radiation/flooding/thermal simulation, no tunnel-local survivor injury truth, no tunnel “discovered bool” that conflicts with canonical knowledge-state semantics, no fully procedural network that mutates topology after save/load, no surface-location teleportation without a real connected path, no tunnel-collapse reroll exploit, and no underground route that bypasses hazard/capacity/equipment requirements merely because it is shorter than the surface path.

---

# 0. Executive Intent

ASHFALL already hints at a world below the surface:

- salt-mine extraction,
- subterranean seed-vault content,
- access-tunnel flavor text,
- tunnel-oriented survivor traits,
- underground fungi data,
- buried infrastructure references.

But those elements currently behave as isolated content fragments rather than one navigable spatial layer.

The intended architecture is:

```text
canonical world locations
        │
        ▼
underground topology definitions
        │
        ├─ segments
        ├─ junctions
        ├─ entrances/exits
        ├─ hidden endpoints
        └─ underground features
        │
        ▼
TunnelNetworkSystem
        │
        ├─ network knowledge
        ├─ exploration state
        ├─ structural integrity
        ├─ accessibility
        ├─ inspection/repair state
        └─ traversal assessment
        │
        ▼
existing authorities
        │
   ┌────┼────────┬─────────┬──────────┬──────────┐
   ▼    ▼        ▼         ▼          ▼          ▼
Expedition  Location  Skills  Equipment  Radiation  Flood/Thermal
```

The product-level outcome is:

> **The shelter can push downward into a partially known subterranean network, discover new connections and hidden places, maintain aging routes, choose underground travel to avoid surface hazards, and accept a different risk profile—collapse, darkness, gas, flooding, radiation, restricted access, and structural decay—without the tunnels becoming a parallel game detached from the rest of the campaign.**

---

# 1. Source Diagnosis

The source establishes:

- `SaltMineExtractionSystem.cs` is the only substantive subterranean gameplay code,
- tunnel/subterranean references otherwise exist largely in narrative/data,
- `subterranean_seed_vault` exists as a location,
- `tunnel_digger` and `underground_navigator` exist as survivor traits,
- there is no physical tunnel network,
- no inter-bunker underground travel,
- no tunnel exploration,
- no tunnel mapping,
- no tunnel maintenance,
- no underground-only resource discovery,
- no hidden tunnel destinations,
- five hazards are proposed:
  - collapse,
  - flooding,
  - radiation,
  - darkness,
  - gas,
- 30 tunnel segments and 10 junctions are required,
- topology should be seeded/deterministic but progressively discovered,
- tunnel travel should be an alternative to surface travel,
- old saves, headless CI, UI, events, quests, and data integrity are required.

The critical architectural interpretation is:

```text
TunnelNetworkSystem owns the underground graph and its segment condition/accessibility
```

but:

```text
ExpeditionSystem owns expedition lifecycle
SkillProgressionSystem owns skills
EquipmentConditionSystem owns equipment condition
RadiationSystem owns radiation
Flooding/Pump systems own water state
ShelterThermalSystem owns shelter temperature
LocationEvolutionSystem owns location state
Survivor health/fate systems own injuries/death
```

---

# 2. Program-Level Success Criteria

C2[36] closes only when all of the following are true.

1. A canonical underground graph exists with 30 valid segments and 10 valid junctions.
2. The shelter connects to one or more valid tunnel entrances.
3. Underground topology is deterministic across save/load and same-seed replay.
4. Discovery is progressive; hidden segments are not exposed simply because data is loaded.
5. Tunnel knowledge uses the project’s canonical knowledge semantics rather than ad hoc booleans.
6. Tunnel expeditions use `ExpeditionSystem`.
7. Tunnel traversal uses actual connected segments.
8. Tunnel travel never teleports between unconnected surface locations.
9. Structural integrity is owned once and degrades deterministically.
10. Collapse blocks travel until a real repair/reopening path succeeds.
11. Flooding uses canonical weather/water/pump inputs.
12. Radiation uses canonical radiation state and protective-equipment checks.
13. Darkness uses real lighting/equipment requirements.
14. Gas uses one explicit hazard/air-quality/toxin contract.
15. Tunnel tools are actual inventory/equipment items.
16. Equipment wear routes through canonical condition authority.
17. `tunnel_digger` and `underground_navigator` become real, consumed traits.
18. Cartography/mapping integrates with Plan 163 or shared knowledge interfaces if available.
19. Hidden underground locations become canonical locations/discoveries, not tunnel-only ghosts.
20. Salt-mine deposits integrate with `SaltMineExtractionSystem` rather than creating a second mining model.
21. Tunnel-induced thermal effects feed `ShelterThermalSystem`.
22. Tunnel expeditions can fail without creating duplicate injury/casualty state.
23. Old saves initialize safely with only valid known entrances/connections.
24. Tunnel collapse and discovery outcomes cannot be rerolled through save/load.
25. No-tunnel and fully mapped network edge cases are valid.
26. Headless CI proves topology, exploration, maintenance, traversal, hazards, repair, save/load, and discovery.
27. 30 segments + 10 junctions all pass reference integrity.
28. Tunnel event frequency and maintenance burden remain manageable.
29. Underground travel offers meaningful tradeoffs versus surface travel.
30. UI shows only player-known segment/junction/hazard facts.

---

# 3. Architectural Invariants

## 3.1 One underground topology authority

`TunnelNetworkSystem` owns:

- segment existence,
- junction existence,
- underground connectivity,
- segment condition,
- segment accessibility,
- exploration/mapping state,
- inspection/maintenance state.

It does not own surface topology.

## 3.2 ExpeditionSystem remains the travel lifecycle authority

Tunnel expeditions are a mode/profile of expedition, not a second scheduler.

## 3.3 Discovery and visitation are distinct

A tunnel can be:

- rumoured,
- located,
- explored,
- traversed,
- mapped.

Exact vocabulary should align with Plan 32/163.

## 3.4 Hazards delegate to canonical systems

Tunnel hazards provide:

```text
hazard exposure/context
```

not duplicate downstream state.

## 3.5 Structural integrity is one persistent underground fact

Unlike derived map completeness, segment integrity is legitimate dynamic state.

## 3.6 Collapse is a state transition

```text
Passable
→ Degraded
→ Restricted
→ Collapsed
→ UnderRepair
→ Reopened
```

No random boolean with no repair semantics.

## 3.7 Mapping is knowledge, not topology mutation

Exploration reveals pre-existing deterministic topology.

## 3.8 Equipment is real

Lanterns, rope, pumps, masks, inspection kits, repair kits exist as items/categories.

## 3.9 Network completeness is derived

Do not persist one global percentage if it can be calculated.

## 3.10 Every hidden endpoint becomes canonical world content when discovered

No orphan underground endpoint understood only by tunnel UI.

---

# 4. Dependency Graph

```text
tunnel_network.json
       │
       ▼
TunnelNetworkSystem
       │
       ├──────────────► Plan 32/163 knowledge
       ├──────────────► ExpeditionSystem
       ├──────────────► LocationEvolutionSystem
       ├──────────────► SkillProgressionSystem
       ├──────────────► Equipment/Condition
       ├──────────────► SaltMineExtractionSystem
       ├──────────────► Weather/Flooding/Pumps
       ├──────────────► Radiation/Protection
       └──────────────► ShelterThermalSystem
```

Survivor consequence path:

```text
Tunnel hazard
   │
   ▼
Expedition encounter/effect
   │
   ▼
Health / Medical / Fate
```

---

# 5. Baseline Capture

Before implementation, record:

- current expedition state machine,
- route/travel APIs,
- Plan 32 topology/knowledge interfaces,
- Plan 163 cartography APIs if implemented,
- `LocationEvolutionSystem` location-version/state API,
- `SaltMineExtractionSystem` deposit and extraction contracts,
- `SkillProgressionSystem` trait/skill hooks,
- actual definitions of `tunnel_digger`,
- actual definitions of `underground_navigator`,
- equipment query/condition APIs,
- light-source item definitions,
- pumping/flood systems,
- radiation/protection APIs,
- ventilation/toxic-air APIs if present,
- shelter thermal inputs,
- current subterranean location IDs.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/verify-fast.sh
```

Record one baseline save with:

- shelter,
- salt mine,
- seed vault location if accessible,
- no tunnel state.

---

# 6. Workstream 167A — Foundation / System Contract

## Goal

Create one deterministic underground graph, one segment-condition model, one knowledge/accessibility model, and one expedition adapter without duplicating the surface-world or expedition architecture.

---

# 7. 167A Phase A — Create `TunnelNetworkSystem`

Path:

```text
Assets/Ashfall.Core/Underground/TunnelNetworkSystem.cs
```

Responsibilities:

- load topology definitions,
- expose known/accessible underground graph,
- track segment condition and inspection,
- coordinate discovery,
- calculate traversal assessments,
- emit tunnel exploration tasks,
- coordinate repairs,
- capture/restore underground-specific state.

---

# 8. 167A Phase B — Static vs Dynamic Split

Static data:

```text
TunnelSegmentDefinition
TunnelJunctionDefinition
TunnelFeatureDefinition
TunnelHazardDefinition
```

Dynamic state:

```text
TunnelSegmentState
TunnelJunctionKnowledge
TunnelSurveyRecord
TunnelRepairTask
TunnelInspectionRecord
TunnelDiscoveryRecord
```

Do not copy static catalog definitions into save.

---

# 9. 167A Phase C — `TunnelSegmentDefinition`

Fields:

```text
segment_id
name_key
from_node_id
to_node_id
length_ticks
difficulty
terrain/subtype
hazard_ids
required_equipment_tags
hidden_feature_ids
base_integrity
degradation_profile
repair_profile
travel_profile
```

---

# 10. 167A Phase D — `TunnelJunctionDefinition`

Fields:

```text
junction_id
name_key
connected_segment_ids
feature_ids
navigation_tags
camp_capability
surface_exit_location_id optional
```

No dynamic discovered boolean.

---

# 11. 167A Phase E — Dynamic Segment State

Persist:

```text
segment_id
knowledge_state
exploration_progress
structural_integrity
access_state
last_inspected_day
last_repaired_day
flooding_ref/status
known_hazard_ids
known_feature_ids
revision
```

---

# 12. 167A Phase F — Knowledge State Alignment

Do not use only:

```text
discovered bool
```

Recommended underground knowledge states:

```text
Unknown
Rumoured
Located
PartiallyExplored
Explored
Mapped
Traversed
```

If Plan 32/163 supports generic place/route knowledge:

- reuse it directly.

If not:

- add a compatible underground route-knowledge adapter.

---

# 13. 167A Phase G — Traversed vs Mapped

A survivor may physically traverse a poorly mapped segment.

Therefore:

```text
Traversed
```

and:

```text
Mapped
```

are independent dimensions if necessary.

Do not force perfect mapping after one traversal.

---

# 14. 167A Phase H — Network Completeness

Derived:

```text
known weighted segments
+ mapped weighted junctions
+ known endpoint features
--------------------------------
total authored network survey weight
```

No persistent global percentage.

---

# 15. 167A Phase I — `tunnel_network.json`

Create:

```text
Assets/StreamingAssets/Data/tunnel_network.json
```

Top-level sections:

```text
nodes/junctions
segments
features
hazard_profiles
initial_entrances
repair_profiles
```

---

# 16. 167A Phase J — 30 Segment / 10 Junction Contract

Exactly:

```text
30 segments
10 junctions
```

after topology audit.

Every segment must connect valid nodes.

Every junction must be reachable or intentionally isolated/hidden with a discovery path.

---

# 17. 167A Phase K — Surface Entrances

Represent surface/shelter access through canonical location IDs.

Examples:

```text
shelter_tunnel_entrance
salt_mine_access
seed_vault_exit
abandoned_bunker_access
```

All IDs must resolve.

---

# 18. 167A Phase L — Hidden Endpoints

Hidden locations may include:

- bunker,
- seed vault,
- resource deposit,
- pre-war machinery,
- water source,
- geothermal vent,
- biological feature.

Each needs an owner:

```text
LocationSystem
Mining
Water
Power
Biology/content event
```

---

# 19. 167A Phase M — Deterministic Topology Rule

The source says topology is seeded.

Choose one of two models:

## Preferred

Authored 30/10 graph with seeded hidden-state/feature availability.

## Alternate

Deterministically generated graph from immutable seed.

If generated:

- graph must be persisted or reproducible byte-for-byte,
- save migration must preserve it,
- all generated IDs must be stable.

Given content references, the authored graph is safer.

---

# 20. 167A Phase N — Progressive Discovery

Network may exist from day 1 but remains unknown.

Exploration reveals:

- next segment,
- junction,
- endpoint,
- hazard,
- feature.

No runtime graph growth that invents new topology from nothing unless the generator contract explicitly owns it.

---

# 21. 167A Phase O — Initial Network Bootstrap

Source:

```text
shelter + 1–3 segments + 1 junction
```

Interpret as:

- 1–3 entrance-adjacent segments known/partly known,
- one nearby junction discoverable/known according to campaign start.

Do not expose entire graph.

---

# 22. 167A Phase P — Segment Access State

Typed:

```text
Unknown
Passable
Restricted
Flooded
Contaminated
Collapsed
UnderRepair
Sealed
```

Knowledge and physical access must not be conflated.

---

# 23. 167A Phase Q — Structural Integrity

Dynamic 0–100.

This is valid persistent state.

Document threshold bands:

```text
Stable
Worn
Unstable
Critical
Collapsed
```

---

# 24. 167A Phase R — Integrity Degradation

Daily degradation derives from:

- base profile,
- age/condition,
- water ingress,
- seismic/disaster damage,
- excavation stress,
- inspection/maintenance,
- tunnel_digger-related mitigation if justified.

No global random subtraction.

---

# 25. 167A Phase S — Inspection

Inspection requires:

- survivor assignment,
- time,
- access,
- inspection equipment if required.

Inspection primarily improves:

- condition knowledge,
- hazard detection,
- preventive maintenance effectiveness.

It should not magically repair the tunnel.

---

# 26. 167A Phase T — Inspection Effects

Source says inspection slows degradation.

Make mechanism explicit:

- preventive support/maintenance task created,
- reduced degradation window,
- early warning of failure.

Do not reduce degradation merely because a UI check happened.

---

# 27. 167A Phase U — Repair

Repair requires:

- actual materials,
- labor,
- access,
- repair tools,
- engineering/repair skill.

Uses inventory transactions.

---

# 28. 167A Phase V — Repair Materials

Source examples:

```text
concrete
steel
timber
```

Validate actual item IDs.

Use categories/tags if exact items differ.

---

# 29. 167A Phase W — Collapse

Collapse can result from:

- integrity crossing threshold,
- disaster effect,
- seeded acute failure.

Effect:

```text
segment accessibility → Collapsed
```

not automatic deletion.

---

# 30. 167A Phase X — Collapse Persistence

Collapsed segment remains blocked until:

- repair/re-excavation,
- alternate-route discovery,
- abandonment/sealing.

No automatic timer reset.

---

# 31. 167A Phase Y — Collapse Idempotency

Stable collapse ID:

```text
segment_id + failure_sequence/version
```

Reload cannot reroll.

---

# 32. 167A Phase Z — Tunnel Expedition Adapter

Define:

```text
TunnelExpeditionIntent
TunnelTraversalContext
TunnelExpeditionOutcome
```

Use existing expedition lifecycle.

Do not persist a second active-expedition list if ExpeditionSystem already owns active expedition state.

---

# 33. 167A Phase AA — Remove Duplicate Casualty State

Source proposes `TunnelExpedition.casualties`.

Instead:

- Expedition/Health/Fate own survivor outcomes.
- Tunnel system stores only outcome references.

---

# 34. 167A Phase AB — Equipment Requirements

Use item tags:

```text
light_source
vertical_traversal
pump
respirator
radiation_protection
inspection_tool
repair_tool
```

Avoid hardcoded item IDs in traversal code.

---

# 35. 167A Phase AC — Trait Consumption

`tunnel_digger` can influence:

- repair time,
- excavation safety,
- integrity-assessment accuracy,

but not passive global degradation unless fiction supports why.

Prefer:

```text
faster repair / better preventive maintenance
```

over mystical unattended slow degradation.

---

# 36. 167A Phase AD — `underground_navigator`

May influence:

- traversal time,
- wrong-turn risk if modeled,
- hazard detection,
- mapping quality.

Must be consumed in real calculation.

---

# 37. 167A Phase AE — Skill Authority

If tunnel-specific exploration/repair skills exist:

- register/use SkillProgressionSystem.

Do not create a tunnel-local XP ledger.

---

# 38. 167A Phase AF — Tunnel Mapping

If Plan 163 exists:

- use cartography skill/knowledge concepts,
- add underground specialization if appropriate.

Do not create a fully separate mapping engine.

---

# 39. 167A Phase AG — Underground Map Data

Tunnel map rendering reads:

- known segments,
- known junctions,
- access,
- integrity,
- known hazards,
- known endpoints.

No hidden labels leak.

---

# 40. 167A Phase AH — Save State

Persist only underground-specific dynamic state:

```text
segment states
junction knowledge
survey/discovery records
repair/inspection state
stable topology seed/version if generated
```

Do not persist:

- expedition duplicates,
- skill XP duplicates,
- equipment copies,
- surface location state copies.

---

# 41. 167A Phase AI — Old Save Compatibility

Missing tunnel section:

```text
valid
```

Initialize:

- shelter entrance,
- authored bootstrap knowledge,
- no fabricated past tunnel expeditions.

---

# 42. 167A Phase AJ — Old Save Grace

No immediate random collapse solely because migrated segments start old/unknown.

Bootstrap integrity from authored initial condition.

---

# 43. 167A Phase AK — Port Contract

Required:

- ExpeditionSystem,
- topology/knowledge service,
- LocationEvolutionSystem,
- SkillProgressionSystem,
- inventory/equipment/condition,
- health/medical/fate,
- weather/flood,
- radiation,
- thermal,
- mining.

Optional:

- cartography,
- colony/outpost,
- tunnel warfare follow-ons.

---

# 44. 167A Phase AL — Semantic Events

Candidate kinds:

```text
tunnel_segment_discovered
tunnel_junction_discovered
tunnel_segment_mapped
tunnel_collapse
tunnel_flooded
tunnel_repaired
tunnel_endpoint_discovered
tunnel_route_opened
tunnel_expedition_completed
```

Use Plan 31 governance.

---

# 45. 167A Phase AM — Diagnostics

Expose:

```text
TUNNEL_SEGMENTS_TOTAL
TUNNEL_JUNCTIONS_TOTAL
TUNNEL_SEGMENTS_KNOWN
TUNNEL_SEGMENTS_PASSABLE
TUNNEL_SEGMENTS_COLLAPSED
TUNNEL_SEGMENTS_FLOODED
TUNNEL_REPAIRS_ACTIVE
TUNNEL_DISCOVERIES
TUNNEL_REQUIRED_PORTS_MISSING
```

---

# 46. 167A Tests

- 30/10 topology,
- reference resolution,
- initial bootstrap,
- knowledge transitions,
- integrity decay,
- inspection effect,
- repair,
- collapse persistence,
- deterministic topology,
- trait consumption,
- save/load,
- old-save defaults,
- no duplicate expedition ownership.

---

# 47. 167A Definition of Done

- [ ] TunnelNetworkSystem,
- [ ] static/dynamic split,
- [ ] 30 segments,
- [ ] 10 junctions,
- [ ] valid surface entrances,
- [ ] hidden endpoints,
- [ ] knowledge-state model,
- [ ] derived network completeness,
- [ ] integrity/access states,
- [ ] degradation,
- [ ] inspection,
- [ ] repair,
- [ ] collapse persistence,
- [ ] ExpeditionSystem adapter,
- [ ] equipment-tag requirements,
- [ ] trait integration,
- [ ] SkillProgression integration,
- [ ] optional Plan 163 mapping integration,
- [ ] save/old-save,
- [ ] ports,
- [ ] semantic events,
- [ ] diagnostics.

---

# 48. Workstream 167B — Exploration, Maintenance, Travel, Hazards & Discoveries

## Goal

Implement the actual underground gameplay loop: entering the network, exploring segment by segment, confronting distinct subterranean hazards, maintaining routes, discovering hidden places/resources, and choosing tunnel travel as a strategic alternative to the surface.

---

# 49. 167B Phase A — Exploration Lifecycle

Tunnel expedition phases:

```text
Plan
Prepare
Enter
Traverse
Survey
Encounter
Return/Continue
Resolve
```

These map onto existing ExpeditionSystem phases where possible.

---

# 50. 167B Phase B — Expedition Planning

Player selects:

- target known segment/frontier,
- survivors,
- equipment,
- supplies,
- goal.

Goals:

```text
Explore
Map
Inspect
Repair
Traverse
Recover
Extract
Rescue
```

---

# 51. 167B Phase C — Frontier Targeting

Player may target:

- known junction with unknown outgoing segment,
- rumoured tunnel endpoint,
- blocked segment repair,
- known resource feature.

Do not allow exact selection of an undiscovered hidden bunker by ID.

---

# 52. 167B Phase D — Traversal Time

Source says segment `length = ticks`.

Use one canonical travel-time calculation.

Inputs:

- segment length,
- difficulty,
- access state,
- flood depth,
- darkness/light,
- survivor condition,
- navigator skill/trait,
- load/encumbrance.

---

# 53. 167B Phase E — Route Traversal

A multi-segment tunnel route time is:

```text
sum(segment traversal)
+ junction overhead
+ hazard/repair delays
```

No arbitrary shortcut multiplier.

---

# 54. 167B Phase F — Alternative Surface/Tunnel Travel

Travel planner compares:

```text
surface route
tunnel route
```

on:

- estimated time,
- weather exposure,
- faction/raider exposure,
- underground hazards,
- equipment needs,
- route freshness/condition.

---

# 55. 167B Phase G — No Automatic Tunnel Superiority

Some tunnel routes should be:

- shorter,
- safer from weather,
- safer from raids,

but:

- collapse,
- gas,
- flooding,
- darkness,
- radiation

can make them worse.

Balance region by region.

---

# 56. 167B Phase H — Exploration Progress

Segment exploration progress may be 0–100.

This is valid if it represents actual survey coverage of that segment.

Do not equate:

```text
100% exploration = 100% structural certainty forever
```

Condition may change later.

---

# 57. 167B Phase I — Partial Exploration

Failure or retreat can leave:

- partial route knowledge,
- one discovered hazard,
- approximate junction location.

Good for continuity.

---

# 58. 167B Phase J — Segment Mapping

Successful mapping improves:

- junction certainty,
- endpoint knowledge,
- hazard confidence,
- route planning.

Use Plan 163 quality semantics if available.

---

# 59. 167B Phase K — Hidden Segment Discovery

Seeded from:

- frontier junction,
- survey skill,
- mapping tools,
- prior clue/event.

Never UI refresh.

---

# 60. 167B Phase L — Hidden Junction Discovery

Same idempotent pattern.

---

# 61. 167B Phase M — Collapse Hazard

Probability based on:

- current integrity,
- traversal stress,
- disaster/seismic context,
- repair quality,
- inspection freshness.

Avoid pure flat random chance.

---

# 62. 167B Phase N — Collapse Warning

Possible clues:

- cracking,
- debris,
- bad supports,
- recent inspection.

Higher skill/inspection increases warning.

---

# 63. 167B Phase O — Collapse During Expedition

Potential outcomes:

- route blocked behind/ahead,
- injury,
- retreat,
- trapped party if supported,
- rescue task.

Do not kill directly in tunnel system.

---

# 64. 167B Phase P — Trapped Expedition

If ExpeditionSystem supports paused/stranded state:

- use it.

If not:

- implement minimal expedition interruption state there,
- not tunnel-only ghost survivors.

---

# 65. 167B Phase Q — Flooding Hazard

Water ingress derives from:

- surface heavy rain/weather,
- seasonal groundwater,
- damaged barriers,
- tunnel depth.

---

# 66. 167B Phase R — Flood State Ownership

Tunnel segment may persist a local accessibility/flood-pressure state.

Actual pumping/flow effect should use canonical flooding/pump abstractions where possible.

---

# 67. 167B Phase S — Pump Requirement

If flood exceeds threshold:

```text
pump required
```

Pump is a real item/device.

Power/fuel requirement enforced if applicable.

---

# 68. 167B Phase T — Flood Travel

Shallow water:

- slowdown,
- equipment/wetness effects if modeled.

Severe flooding:

- blocked.

---

# 69. 167B Phase U — Radiation Hazard

Fixed/seeded radioactive veins or contaminated infrastructure.

Exposure uses RadiationSystem.

Tunnel provides:

- dose environment,
- duration,
- shielding context.

---

# 70. 167B Phase V — Radiation Equipment

Source says dosimeter + protection.

Require actual equipment/tags:

```text
radiation_monitor
respiratory_protection if needed
protective_clothing
```

Do not require gas mask for radiation unless specific contamination mode justifies it.

---

# 71. 167B Phase W — Darkness Hazard

Darkness is not simply a damage check.

It affects:

- traversal speed,
- hazard detection,
- mapping quality,
- wrong-route risk if modeled.

---

# 72. 167B Phase X — Light Sources

Lantern/flashlight:

- real item,
- battery/fuel state if supported,
- condition.

No infinite free light if item system supports consumption.

---

# 73. 167B Phase Y — Gas Hazard

Use one canonical toxic-air contract.

If ventilation/toxin exposure exists:

- route into it.

If not:

- implement generic environmental toxin exposure in health/affliction authority,
- not tunnel-local HP damage.

---

# 74. 167B Phase Z — Gas Detection

Detection may use:

- gas detector,
- smell clue,
- trained survivor,
- canary-like item only if fiction/content supports it.

Do not make gas mask both detector and protection by default.

---

# 75. 167B Phase AA — Rope Requirement

Vertical/shaft segments require:

```text
vertical_traversal equipment
```

Rope may wear/be consumed according to item rules.

---

# 76. 167B Phase AB — Inspection Kit

Improves structural assessment.

Does not repair.

---

# 77. 167B Phase AC — Repair Kit

Required for certain repair classes.

Materials still consumed separately.

---

# 78. 167B Phase AD — Equipment Loss

Failure can damage/lose tools via equipment/inventory transaction.

No tunnel-owned equipment state.

---

# 79. 167B Phase AE — Maintenance Scheduling

Maintenance panel can create tasks:

```text
Inspect segment
Reinforce support
Pump water
Clear debris
Repair collapse
Seal gas pocket
Install lighting
```

Each maps to real work/resource contracts.

---

# 80. 167B Phase AF — Daily Integrity Tick

Only passable/discovered/owned-maintained segments need daily processing where necessary.

Avoid iterating every hidden segment if state static.

---

# 81. 167B Phase AG — Degradation Budget

Typical stable segments should not demand constant repairs.

Tune:

- stable routes: slow degradation,
- damaged/flood-prone routes: faster,
- abandoned branches: optional.

---

# 82. 167B Phase AH — Preventive Maintenance

Inspection + small maintenance should be cheaper than collapse repair.

This creates meaningful maintenance economics.

---

# 83. 167B Phase AI — Tunnel Digger Trait

Concrete effects:

- repair labor efficiency,
- reinforcement effectiveness,
- excavation safety,
- collapse-clearance time.

No arbitrary global passive aura.

---

# 84. 167B Phase AJ — Underground Navigator Trait

Concrete effects:

- traversal efficiency,
- hazard detection,
- map quality,
- route-confidence improvement.

---

# 85. 167B Phase AK — Hidden Bunkers

Discovery creates/reveals canonical location.

Potential content:

- loot,
- survivors,
- hostile occupants,
- disease,
- radiation,
- lore.

Use existing encounter/location systems.

---

# 86. 167B Phase AL — Resource Deposits

Types:

- salt/mineral,
- water,
- geothermal,
- metal ore if supported.

Each deposit needs an owning extraction system.

---

# 87. 167B Phase AM — Salt Mine Integration

Salt deposit discovered:

```text
TunnelNetworkSystem
→ SaltMineExtractionSystem
```

to register/enable deposit access.

No second salt-production ledger.

---

# 88. 167B Phase AN — Underground Water

If water extraction/purification exists:

- create canonical source.

If not:

- content discovery can remain a location/quest hook until owner exists.

---

# 89. 167B Phase AO — Geothermal Vent

Potential thermal/power opportunity.

Do not directly grant heat/power from tunnel system.

Unlock project/connection through canonical systems.

---

# 90. 167B Phase AP — Ancient Infrastructure

Examples:

- generator,
- pump station,
- pre-war machinery,
- rail spur,
- communication cable.

Discovery enables canonical repair/project content.

---

# 91. 167B Phase AQ — Biological Features

Source:

- fungi,
- mutated creatures,
- root systems.

`UndergroundFungiCatalog` can provide content if it has real consumers.

Do not convert all narrative fungi into loot without content audit.

---

# 92. 167B Phase AR — Seed Vault

`subterranean_seed_vault` must resolve to canonical location.

Tunnel discovery:

```text
reveals/accesses endpoint
```

rather than creates duplicate vault.

---

# 93. 167B Phase AS — Connection to Surface Location

When a segment reaches a surface location:

- register valid alternate route in travel/topology layer,
- keep underground segment chain as route path.

No direct “accessible via tunnel” shadow list if topology can represent it.

---

# 94. 167B Phase AT — Inter-Bunker Travel

Other bunkers become canonical destinations.

Tunnel network supplies route option.

Expedition/travel authority remains unchanged.

---

# 95. 167B Phase AU — Route Opening

Newly discovered tunnel connection can trigger:

- map update,
- expedition targeting,
- faction/trade consequences if later plans use it.

---

# 96. 167B Phase AV — Shelter Thermal Effect

Tunnel entrances can affect:

- drafts,
- ground temperature,
- heat leakage,
- stable subterranean temperature.

Use `ShelterThermalSystem` input seam.

---

# 97. 167B Phase AW — Thermal Direction

Avoid simplistic:

```text
tunnels always warm shelter
```

Effect depends on:

- season,
- seal state,
- airflow,
- tunnel depth.

---

# 98. 167B Phase AX — Weather Immunity Contract

Tunnel travel avoids some surface weather exposure.

It does not ignore:

- groundwater flooding,
- freeze/thaw damage,
- external entrance blockage.

---

# 99. 167B Phase AY — Raider Avoidance

Tunnel route may reduce surface encounter eligibility.

If tunnel warfare/intruder systems do not exist:

- do not replace surface raiders with generic tunnel raiders automatically.

---

# 100. 167B Phase AZ — Expedition Supply Costs

Tunnel travel may require:

- light,
- PPE,
- rope,
- pumps,
- food/water,
- tools.

Surface travel may require different supplies.

---

# 101. 167B Phase BA — Tunnel Events

Source:

```text
The Discovery
The Collapse
The Flood
The Bunker
The Deposit
The Repair
The Expedition
The Connection
```

Use canonical event framework.

---

# 102. 167B Phase BB — Event Significance

Do not create modal event for every routine segment movement.

Use:

- first discovery,
- severe hazard,
- major connection,
- major repair,
- hidden bunker/deposit.

---

# 103. 167B Phase BC — Quest Hooks

Source:

```text
The Explorer
The Mapper
The Miner
The Bunker
The Engineer
The Network
The Vault
```

Use canonical quest runtime.

---

# 104. 167B Phase BD — Explorer Quest

Discover 10 tunnel segments.

Use canonical knowledge state count.

---

# 105. 167B Phase BE — Mapper Quest

Map complete network.

Completion from derived completeness, not persisted percent.

---

# 106. 167B Phase BF — Miner Quest

Discover/resource-enable one valid deposit.

---

# 107. 167B Phase BG — Bunker Quest

Discover hidden bunker.

---

# 108. 167B Phase BH — Engineer Quest

Repair one collapsed segment.

---

# 109. 167B Phase BI — Network Quest

Connect 3 surface locations via valid underground paths.

Compute graph connectivity.

---

# 110. 167B Phase BJ — Vault Quest

Reach canonical `subterranean_seed_vault`.

Physical traversal required.

---

# 111. 167B Phase BK — Tunnel Map UI

Display:

- known junctions,
- known segments,
- access state,
- integrity,
- known hazards,
- endpoints,
- route selection.

---

# 112. 167B Phase BL — Hidden Information

Unknown segment names/hidden endpoint labels must not appear in:

- tooltips,
- accessibility tree,
- filter lists,
- debug-disabled UI.

---

# 113. 167B Phase BM — Expedition Planner

Assign:

- survivors,
- equipment,
- target,
- objective.

Show:

- known hazard confidence,
- estimated time,
- equipment gaps,
- route integrity.

---

# 114. 167B Phase BN — Maintenance Panel

Show:

- integrity band,
- last inspection,
- degradation trend,
- repair materials,
- access state,
- active maintenance tasks.

---

# 115. 167B Phase BO — Discovery Log

Only significant tunnel discoveries.

Avoid every minor exploration increment.

---

# 116. 167B Phase BP — Travel Planner

Compare surface vs tunnel:

| Factor | Surface | Tunnel |
|---|---|---|
| Travel time | estimate | estimate |
| Weather | yes | reduced/indirect |
| Faction encounters | yes | reduced |
| Collapse | no/other | yes |
| Flooding | route-specific | yes |
| Radiation | location-specific | tunnel-specific |
| Required gear | expedition gear | tunnel gear |

---

# 117. 167B Phase BQ — Tooltips

Segment tooltip:

- knowledge state,
- integrity,
- known hazards,
- length,
- last inspection,
- known equipment needs.

No unknown hazard spoilers.

---

# 118. 167B Phase BR — Tutorial

First tunnel discovery explains:

- underground graph,
- exploration,
- hazard equipment,
- maintenance,
- surface-vs-tunnel choice.

Keep staged.

---

# 119. 167B Phase BS — Journal / Archive

Journal logs personal/operational notes.

Plan 162 archive may record:

- first tunnel,
- major collapse,
- seed vault,
- definitive underground network.

Archive remains historical consumer.

---

# 120. 167B Phase BT — Localization

All segment/junction/event/quest names use localization keys.

---

# 121. 167B Phase BU — Content Matrix: 30 Segments

Generate:

| Segment | From | To | Length | Difficulty | Hazard profile | Hidden? | Endpoint | Runtime observed |
|---|---|---|---:|---:|---|---:|---|---:|

---

# 122. 167B Phase BV — Content Matrix: 10 Junctions

Generate:

| Junction | Segments | Surface exit | Features | Initial knowledge | Runtime observed |
|---|---|---|---|---|---:|

---

# 123. 167B Phase BW — Hazard Coverage Matrix

Ensure all five source hazards appear in runtime-valid content:

```text
collapse
flood
radiation
darkness
gas
```

---

# 124. 167B Phase BX — Equipment Coverage Matrix

Ensure each requirement has a real item/tag and at least one acquisition path.

---

# 125. 167B Phase BY — Content Utilization

Run 200-day tunnel scenario.

Report:

```text
segments known
segments traversed
segments mapped
junctions known
collapses
flood events
radiation exposures
gas encounters
repairs
hidden endpoints
resource deposits
```

---

# 126. 167B Phase BZ — Dead Content Policy

Never-observed segment/junction/hazard/equipment requirement:

- fix reachability,
- mark deliberately late,
- remove,
- or exempt with reason.

---

# 127. 167B Definition of Done

- [ ] tunnel exploration lifecycle,
- [ ] targetable frontier,
- [ ] traversal time,
- [ ] surface/tunnel comparison,
- [ ] partial exploration,
- [ ] mapping,
- [ ] collapse,
- [ ] flooding,
- [ ] radiation,
- [ ] darkness,
- [ ] gas,
- [ ] equipment requirements,
- [ ] tool wear/loss,
- [ ] maintenance scheduling,
- [ ] preventive maintenance,
- [ ] trait effects,
- [ ] hidden bunkers,
- [ ] resource deposits,
- [ ] ancient infrastructure,
- [ ] biological features,
- [ ] seed vault,
- [ ] surface connections,
- [ ] inter-bunker travel,
- [ ] thermal/weather effects,
- [ ] 8 events,
- [ ] 7 quests,
- [ ] tunnel map,
- [ ] expedition planner,
- [ ] maintenance panel,
- [ ] travel planner,
- [ ] tutorial/tooltips,
- [ ] localization,
- [ ] content-utilization report.

---

# 128. Workstream 167C — Cross-System Integration, Save/CI, Exploit Control, Balance, and Long-Run Closure

## Goal

Prove the underground network works as a genuine connected world layer over existing systems, remains deterministic and save-safe, produces distinct route tradeoffs, and does not overwhelm the campaign with maintenance or hazard noise.

---

# 129. 167C Phase A — ExpeditionSystem Integration

Tunnel expedition lifecycle must use:

- expedition identity,
- survivor assignment,
- start/active/return/resolution semantics,
- casualty/health handoff.

No parallel active-expedition collection.

---

# 130. 167C Phase B — Route Mode

Add/extend route context:

```text
Surface
Tunnel
Mixed
```

if ExpeditionSystem needs it.

Avoid new travel engine.

---

# 131. 167C Phase C — Mixed Route Support

Potential route:

```text
shelter
→ tunnel segment
→ bunker exit
→ short surface route
```

Only if topology supports cross-layer edges.

---

# 132. 167C Phase D — LocationEvolution Integration

Hidden location discovery updates canonical location knowledge/access.

Tunnel system does not own location depletion/evolution.

---

# 133. 167C Phase E — Plan 32 Knowledge Integration

Tunnel endpoints and surface exits must use canonical knowledge state.

---

# 134. 167C Phase F — Plan 163 Cartography Integration

If Plan 163 is live:

- underground mapping can reuse cartography skill/read models,
- underground map quality remains distinct region/segment context,
- no duplicate map-trading logic unless later plan explicitly adds tunnel maps to trade.

---

# 135. 167C Phase G — SkillProgression Integration

Tunnel exploration/repair XP goes through canonical skill system.

---

# 136. 167C Phase H — Trait Tests

Verify:

`tunnel_digger`
- improves repair/inspection/excavation as designed.

`underground_navigator`
- improves traversal/hazard detection/mapping as designed.

No dead traits.

---

# 137. 167C Phase I — Equipment Integration

Required tools resolve from actual inventory.

---

# 138. 167C Phase J — Condition Integration

Damaged tool gives reduced/no benefit.

Repair uses normal equipment repair.

---

# 139. 167C Phase K — SaltMineExtraction Integration

Discovering valid salt/mineral route can:

- unlock access,
- modify hauling route,
- enable extraction node.

Salt system owns extraction quantities.

---

# 140. 167C Phase L — Weather Integration

Heavy rain/weather can increase flooding risk.

No direct tunnel-owned weather.

---

# 141. 167C Phase M — Flooding/Pump Integration

Pumping uses actual pump/system capacity.

---

# 142. 167C Phase N — Radiation Integration

Exposure uses canonical dose/protection.

---

# 143. 167C Phase O — Gas/Air Integration

Use canonical toxic-air/ventilation/health sink.

---

# 144. 167C Phase P — Thermal Integration

Tunnel entrance influence contributes to thermal model.

---

# 145. 167C Phase Q — Medical Integration

Injuries from collapse/gas/etc. enter canonical affliction/treatment pipeline.

---

# 146. 167C Phase R — Survivor Fate Integration

Deaths only finalize through survivor fate/death authority.

---

# 147. 167C Phase S — Save/Load Matrix

Test:

```text
initial entrance
mid-exploration
partial segment
mapped segment
mid-traversal
flooded segment
collapsed segment
under repair
reopened segment
hidden endpoint discovered
```

---

# 148. 167C Phase T — Topology Save Integrity

If topology authored:

- save stores no duplicate graph.

If generated:

- seed/version reproduces exact graph,
- migration fixture hashes topology.

---

# 149. 167C Phase U — Discovery Anti-Reroll

Reload cannot reroll:

- next segment,
- hidden endpoint,
- hazard discovery,
- feature discovery.

---

# 150. 167C Phase V — Collapse Anti-Reroll

Once collapse outcome committed:

- reload preserves it.

---

# 151. 167C Phase W — Repair Exploit

Cannot:

- start repair,
- refund materials,
- reload,
- duplicate progress.

Use transactional state.

---

# 152. 167C Phase X — Equipment Requirement Exploit

Equipment is checked at:

- departure,
- traversal step,
- hazard step,

as appropriate.

No unequip-after-start loophole if party inventory changes.

---

# 153. 167C Phase Y — Route Bypass Exploit

Cannot choose endpoint unless all required segments are:

- known enough,
- connected,
- accessible.

---

# 154. 167C Phase Z — Surface Threat Bypass Balance

Tunnel travel may avoid surface raiders/weather, but route cost/hazard profile must compensate.

Measure.

---

# 155. 167C Phase AA — No-Tunnel Edge Case

Shelter only.

Expected:

- no crash,
- tunnel UI empty/entrance-only,
- surface game unaffected.

---

# 156. 167C Phase AB — Full-Network Edge Case

All 30/10 known.

Expected:

- map complete,
- routes selectable by access,
- maintenance/staleness still meaningful,
- no repeated completion reward.

---

# 157. 167C Phase AC — All-Collapsed Edge Case

If all routes collapse:

- network temporarily unusable,
- repair tasks remain,
- surface travel unaffected.

No global campaign soft lock unless shelter itself depends on tunnels.

---

# 158. 167C Phase AD — No-Equipment Edge Case

Basic exploration may be possible only on segments without explicit equipment requirements.

Clearly explain blocked segments.

---

# 159. 167C Phase AE — No-Skilled-Survivor Edge Case

Low-skill party can still attempt low-difficulty routes.

Higher risk/slower progress.

---

# 160. 167C Phase AF — Flooded-Network Edge Case

Heavy rain fixture.

Ensure:

- only vulnerable segments flood,
- pumping/closure works,
- no global network wipe.

---

# 161. 167C Phase AG — Radiation-Hot Network Edge Case

High-radiation branches remain optional/blocked without protection.

No unavoidable dose just because network exists.

---

# 162. 167C Phase AH — Maintenance Burden Budget

Measure:

```text
repair tasks/month
inspection tasks/month
materials/month
labor-days/month
```

Stable network should not consume absurd upkeep.

---

# 163. 167C Phase AI — Hazard Frequency Budget

Measure per 100 segment traversals.

Distinguish:

- known hazard exposure,
- acute random incident,
- structural failure.

Avoid hazard every traversal.

---

# 164. 167C Phase AJ — Tunnel-vs-Surface Balance Profiles

Compare:

```text
surface_only
tunnel_heavy
balanced
```

Metrics:

- travel days,
- expedition injuries,
- weather exposure,
- faction encounters,
- repair materials,
- equipment wear.

---

# 165. 167C Phase AK — Route Choice Dominance Test

No one route mode should dominate every dimension.

Tunnel may be best for:

- weather avoidance,
- covert movement.

Surface may be best for:

- capacity,
- simplicity,
- low maintenance.

---

# 166. 167C Phase AL — Thermal Balance

Tunnel entrance should not become free infinite heat/cooling.

Bound contribution.

---

# 167. 167C Phase AM — Resource-Deposit Balance

Hidden deposits must not create infinite resources.

Extraction authority governs depletion/throughput.

---

# 168. 167C Phase AN — Hidden Bunker Loot Balance

Use normal loot/depletion/event systems.

No repeated free loot from rediscovery.

---

# 169. 167C Phase AO — Secret/Discovery Idempotency

Each endpoint/feature discovered once.

---

# 170. 167C Phase AP — Graph Integrity

Validate:

- no dangling segment endpoints,
- no invalid junction refs,
- no duplicate segment IDs,
- no impossible one-way assumptions unless explicitly supported,
- connected components intentional.

---

# 171. 167C Phase AQ — Reachability Audit

For each authored hidden endpoint:

- prove at least one valid discovery/reach path.

---

# 172. 167C Phase AR — Surface Exit Integrity

Every surface exit:

- resolves to canonical location,
- has intended access policy,
- does not bypass locked quest/location without permission.

---

# 173. 167C Phase AS — Seed Vault Gate Integrity

If seed vault has quest/research prerequisite:

- tunnel discovery does not bypass it silently.

Can:

```text
discover entrance
```

without:

```text
fully access contents
```

---

# 174. 167C Phase AT — `--tunnel-network-selftest`

Required scenarios:

1. load 30/10 graph,
2. shelter bootstrap,
3. discover first segment,
4. partial exploration,
5. map segment,
6. traverse,
7. collapse,
8. repair,
9. flood,
10. pump,
11. radiation exposure/protection,
12. darkness/light,
13. gas/protection,
14. hidden bunker,
15. resource deposit,
16. seed vault endpoint,
17. tunnel-vs-surface travel,
18. old save,
19. no-tunnel state,
20. full network.

---

# 175. 167C Phase AU — Data Integrity

Validate:

- segment IDs,
- junction IDs,
- location refs,
- hazard IDs,
- feature IDs,
- equipment tags/items,
- trait IDs,
- quest IDs,
- localization,
- repair materials.

---

# 176. 167C Phase AV — Deliberate Failure Proof

Break:

- invalid junction,
- invalid endpoint,
- missing equipment tag,
- duplicate segment ID,
- collapse idempotency key,
- missing expedition port.

Assert gate fails.

---

# 177. 167C Phase AW — Same-Seed Replay

Same:

```text
campaign seed
exploration choices
repair choices
travel choices
```

→ same:

```text
discoveries
hazard outcomes
collapse events
hidden endpoints
network knowledge digest
```

---

# 178. 167C Phase AX — 200-Day Tunnel Soak

Record:

```text
segments discovered
junctions discovered
routes traversed
collapses
repairs
flooded days
radiation encounters
gas encounters
equipment wear
resource discoveries
hidden bunkers
```

---

# 179. 167C Phase AY — Long-Run Condition Soak

Run 1–5 campaign years.

Measure:

- average integrity,
- repair frequency,
- permanently abandoned segments,
- maintenance cost,
- path availability.

---

# 180. 167C Phase AZ — Performance Budget

Avoid scanning every segment every frame.

Use:

- daily maintenance tick,
- event-driven hazard updates,
- route-local traversal calculations,
- cached graph queries.

---

# 181. 167C Phase BA — Pathfinding Budget

If underground route selection uses graph search:

- 30 segments is small,
- use deterministic traversal cost,
- stable tie-break.

Document complexity.

---

# 182. 167C Phase BB — UI Performance

Tunnel map should not re-layout graph every frame.

Cache node positions.

---

# 183. 167C Phase BC — Accessibility

Tunnel map:

- keyboard/controller navigation,
- non-color hazard/access state,
- hidden labels not leaked,
- clear route comparison.

---

# 184. 167C Phase BD — Headless Behavior

Exploration, hazard progression, repair, and route access work without UI.

---

# 185. 167C Phase BE — Retention

Plan 55:

Keep:

- current segment state,
- current inspections/repairs,
- landmark discoveries,
- major collapses,
- famous connections.

Roll up:

- old routine inspection logs,
- superseded low-value survey details.

---

# 186. 167C Phase BF — Archive / Legacy

Plan 162 may record:

- first underground connection,
- seed vault,
- catastrophic collapse,
- fully mapped network,
- major hidden bunker.

No archive ownership of tunnel state.

---

# 187. 167C Phase BG — Human Playtest

Evaluate:

```text
Do tunnels feel distinct from surface expeditions?
Does maintenance create strategy rather than chores?
Are hazard requirements legible?
Is alternative travel worth considering?
Does discovery feel spatial rather than menu-driven?
```

---

# 188. 167C Phase BH — Documentation

Create:

```text
docs/systems/UNDERGROUND_TUNNEL_NETWORK.md
```

Include:

- topology authority,
- data schema,
- knowledge/discovery,
- expedition adapter,
- integrity/degradation,
- hazards,
- repair,
- route planning,
- save behavior,
- adding segments/junctions.

---

# 189. 167C Definition of Done

- [ ] ExpeditionSystem integration,
- [ ] world-knowledge integration,
- [ ] LocationEvolution integration,
- [ ] SkillProgression integration,
- [ ] trait consumption,
- [ ] equipment/condition integration,
- [ ] SaltMine integration,
- [ ] weather/flooding integration,
- [ ] radiation integration,
- [ ] gas/air integration,
- [ ] thermal integration,
- [ ] medical/fate integration,
- [ ] save/load lifecycle matrix,
- [ ] deterministic topology,
- [ ] discovery anti-reroll,
- [ ] collapse anti-reroll,
- [ ] repair transaction integrity,
- [ ] route-bypass protection,
- [ ] no/full/all-collapsed edge cases,
- [ ] no-equipment/no-skill cases,
- [ ] flood/radiation stress cases,
- [ ] maintenance budget,
- [ ] hazard-frequency budget,
- [ ] tunnel-vs-surface profiles,
- [ ] route dominance test,
- [ ] graph/reachability integrity,
- [ ] seed-vault gating,
- [ ] selftest,
- [ ] deliberate failure proof,
- [ ] same-seed replay,
- [ ] 200-day soak,
- [ ] long-run condition soak,
- [ ] performance/pathfinding/UI budgets,
- [ ] accessibility,
- [ ] headless,
- [ ] retention,
- [ ] archive/legacy,
- [ ] playtest,
- [ ] docs.

---

# 190. Integrated Underground Pipeline

```text
tunnel_network.json
       │
       ▼
authoritative underground topology
       │
       ▼
player knowledge / segment state
       │
       ├─ explore
       ├─ inspect
       ├─ repair
       ├─ map
       └─ traverse
       │
       ▼
TunnelNetworkSystem
       │
       ├─ traversal assessment
       ├─ hazard context
       ├─ accessibility
       └─ discovery intent
       │
       ▼
ExpeditionSystem + canonical downstream systems
```

---

# 191. Topology Contract

The underground graph is:

```text
segments + junctions + canonical surface/location endpoints
```

It is not a second surface world.

---

# 192. Expedition Contract

Tunnel expeditions use the real expedition lifecycle.

No tunnel-only party scheduler.

---

# 193. Knowledge Contract

Tunnel discovery uses canonical knowledge semantics.

No UI-only discovered flag.

---

# 194. Mapping Contract

Underground mapping improves route/hazard knowledge.

If Plan 163 exists, reuse cartography where sensible.

---

# 195. Structural Contract

Segment integrity is the only canonical tunnel-condition number.

---

# 196. Collapse Contract

Collapse is persistent until repaired/reopened.

No automatic daily reset.

---

# 197. Flood Contract

Water hazard is driven by real weather/groundwater/pumping inputs.

---

# 198. Radiation Contract

Tunnel radiation contributes canonical dose.

No tunnel radiation meter.

---

# 199. Darkness Contract

Darkness alters traversal/mapping through real light equipment.

---

# 200. Gas Contract

Gas routes through a canonical toxin/air/health exposure model.

---

# 201. Equipment Contract

All required tunnel tools are real items/tags.

---

# 202. Condition Contract

Tool degradation belongs to EquipmentConditionSystem.

---

# 203. Skill Contract

Exploration/repair/cartography XP belongs to SkillProgressionSystem.

---

# 204. Trait Contract

`tunnel_digger` and `underground_navigator` must be consumed by real calculations.

---

# 205. Mining Contract

Resource deposits unlock canonical extraction systems.

Tunnel system does not own production quantities.

---

# 206. Thermal Contract

Tunnel entrance modifies thermal inputs only.

ShelterThermalSystem owns temperature.

---

# 207. Location Contract

Hidden bunker/vault discoveries resolve to canonical locations.

---

# 208. Travel Contract

A tunnel route is valid only if every required segment is connected and accessible.

---

# 209. Save Contract

Persist:

- segment dynamic state,
- junction knowledge,
- inspections/repairs,
- discovery records,
- generated topology seed/version only if needed.

Do not persist:

- expedition duplicates,
- skill copies,
- item copies,
- surface-location copies.

---

# 210. Old-Save Contract

Old saves get:

- valid entrance/bootstrap state,
- no fabricated history,
- no migration-triggered collapse storm.

---

# 211. Determinism Contract

Same:

```text
seed
network version
player choices
```

→ same topology/discovery/hazard sequence.

---

# 212. Balance Contract

Tunnel travel should be a strategic option, not a universally superior shortcut.

---

# 213. Maintenance Contract

Preventive maintenance should be cheaper than repeated catastrophic repair.

---

# 214. Accessibility Contract

UI reveals only known segments/hazards/endpoints.

---

# 215. Content Acceptance Contract

Tunnel content progresses through:

```text
AUTHORED
→ LOADS
→ CONNECTED
→ DISCOVERABLE
→ EXPLORED
→ TRAVERSABLE
→ CONSEQUENCE/UTILITY PRODUCED
→ PLAYER_VISIBLE
```

---

# 216. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| tunnels become a second expedition system | High | Critical | ExpeditionSystem adapter only |
| underground graph duplicates world topology | Medium | High | cross-layer endpoints + one underground authority |
| random generation changes after reload | Medium | Critical | authored graph or stable seed/version |
| maintenance becomes chores | High | Medium | slow degradation + preventive efficiency |
| tunnel routes dominate surface | Medium | High | explicit route tradeoff balance |
| collapse rerolls on save/load | Medium | High | stable failure IDs |
| hidden endpoints bypass quest gates | Medium | High | discovery vs access separation |
| hazard state duplicates canonical systems | Medium | High | hazard-context ports |
| tools become fake flags | Medium | Medium | actual inventory/tag checks |
| gas lacks canonical owner | Medium | High | prerequisite/health exposure contract |
| salt/resource discovery duplicates extraction | Medium | High | register with extraction owner |
| UI leaks hidden tunnel labels | Medium | High | semantic-tree leakage test |

---

# 217. Commit Strategy

## 167A — Foundation

### C2[36].1 — baseline + underground-topology ADR

### C2[36].2 — segment/junction definitions and dynamic state

### C2[36].3 — tunnel_network.json 30/10 content authority

### C2[36].4 — knowledge/access-state integration

### C2[36].5 — integrity/degradation/inspection

### C2[36].6 — repair/collapse/idempotency

### C2[36].7 — expedition adapter + equipment requirements

### C2[36].8 — trait/skill/cartography integration

### C2[36].9 — save/old-save

### C2[36].10 — ports/events/diagnostics

### Gate: 167A complete

---

## 167B — Exploration / Hazards / Travel

### C2[36].11 — tunnel expedition planning/traversal

### C2[36].12 — partial exploration/mapping

### C2[36].13 — collapse hazard

### C2[36].14 — flooding/pumping

### C2[36].15 — radiation/darkness/gas

### C2[36].16 — tunnel equipment/condition

### C2[36].17 — maintenance scheduling

### C2[36].18 — hidden bunkers/deposits/infrastructure

### C2[36].19 — salt mine/seed vault/surface connections

### C2[36].20 — alternative route planner

### C2[36].21 — events/quests/journal/tutorial

### C2[36].22 — tunnel UI/content-utilization

### Gate: 167B complete

---

## 167C — Closure

### C2[36].23 — Expedition/Location/Knowledge integration

### C2[36].24 — Skills/Traits/Equipment/Mining integration

### C2[36].25 — Weather/Flood/Radiation/Thermal/Medical integration

### C2[36].26 — save-load/anti-reroll matrix

### C2[36].27 — edge-case suite

### C2[36].28 — graph/reachability/gate integrity

### C2[36].29 — selftest + deliberate failure proof

### C2[36].30 — 200-day tunnel soak

### C2[36].31 — long-run maintenance soak

### C2[36].32 — route-choice balance profiles

### C2[36].33 — performance/accessibility/headless

### C2[36].34 — playtest/docs/release closure

### Gate: 167C complete

---

# 218. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --tunnel-network-selftest
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical equivalents of:

```text
port-contract validation
30-segment/10-junction topology integrity
old-save fixture load
same-seed tunnel topology/discovery replay
tunnel collapse idempotency test
tunnel repair transaction test
200-day underground soak
long-run maintenance-budget test
surface-vs-tunnel route balance comparison
tunnel-map hidden-label/accessibility leakage test
```

---

# 219. Flagship Definition of Done

## 167A — Foundation

- [ ] TunnelNetworkSystem,
- [ ] static/dynamic topology split,
- [ ] 30 segment definitions,
- [ ] 10 junction definitions,
- [ ] canonical surface endpoints,
- [ ] progressive knowledge state,
- [ ] derived completeness,
- [ ] integrity/accessibility state,
- [ ] daily degradation,
- [ ] inspection,
- [ ] repair,
- [ ] persistent collapse,
- [ ] ExpeditionSystem adapter,
- [ ] equipment-tag requirements,
- [ ] trait consumption,
- [ ] skill integration,
- [ ] optional cartography integration,
- [ ] save/old-save,
- [ ] semantic events,
- [ ] ports,
- [ ] diagnostics.

## 167B — Exploration / Maintenance / Travel

- [ ] tunnel expedition lifecycle,
- [ ] route traversal,
- [ ] partial exploration,
- [ ] mapping,
- [ ] collapse,
- [ ] flood,
- [ ] radiation,
- [ ] darkness,
- [ ] gas,
- [ ] lantern/rope/pump/mask/inspection/repair equipment,
- [ ] maintenance scheduling,
- [ ] preventive maintenance,
- [ ] tunnel_digger effect,
- [ ] underground_navigator effect,
- [ ] hidden bunkers,
- [ ] deposits,
- [ ] ancient infrastructure,
- [ ] biological features,
- [ ] seed vault,
- [ ] surface connections,
- [ ] alternative travel,
- [ ] thermal/weather influence,
- [ ] 8 events,
- [ ] 7 quests,
- [ ] tunnel map,
- [ ] expedition planner,
- [ ] maintenance panel,
- [ ] travel planner,
- [ ] tutorial/tooltips,
- [ ] content utilization.

## 167C — Integration / Validation

- [ ] ExpeditionSystem,
- [ ] LocationEvolutionSystem,
- [ ] Plan 32 knowledge,
- [ ] optional Plan 163 cartography,
- [ ] SkillProgression,
- [ ] traits,
- [ ] inventory/equipment,
- [ ] equipment condition,
- [ ] SaltMineExtraction,
- [ ] Weather/Flooding,
- [ ] Radiation,
- [ ] gas/air,
- [ ] ShelterThermal,
- [ ] medical/fate,
- [ ] save/load matrix,
- [ ] deterministic topology,
- [ ] discovery/collapse anti-reroll,
- [ ] repair exploit prevention,
- [ ] route bypass prevention,
- [ ] no/full/all-collapsed edge cases,
- [ ] no-equipment/no-skill cases,
- [ ] graph integrity,
- [ ] reachability,
- [ ] seed-vault gating,
- [ ] selftest,
- [ ] failure proof,
- [ ] same-seed replay,
- [ ] 200-day soak,
- [ ] long-run maintenance soak,
- [ ] tunnel-vs-surface balance,
- [ ] maintenance/hazard budgets,
- [ ] performance/pathfinding budgets,
- [ ] accessibility,
- [ ] headless,
- [ ] retention,
- [ ] archive/legacy,
- [ ] playtest,
- [ ] docs.

## Global

- [ ] no second expedition system,
- [ ] no duplicate survivor skill state,
- [ ] no duplicate equipment durability,
- [ ] no duplicate radiation/flood/thermal truth,
- [ ] no duplicate injury/death truth,
- [ ] no hidden endpoint bypass,
- [ ] no collapse reroll,
- [ ] no maintenance spam,
- [ ] no tunnel route universal dominance,
- [ ] full verification green.

---

# 220. Closure Report Template

```markdown
## C2[36] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Expedition authority:
- Surface topology authority:
- Knowledge model:
- Cartography availability:
- Salt mine:
- Tunnel traits:
- Equipment condition:
- Flood/radiation/thermal owners:

### 167A — Foundation
- Segments:
- Junctions:
- Surface entrances:
- Hidden endpoints:
- Knowledge states:
- Integrity model:
- Degradation:
- Inspection:
- Repair:
- Collapse:
- Expedition adapter:
- Skill/trait integration:
- Save schema:
- Old-save bootstrap:
- Missing ports:
- Result:

### 167B — Exploration / Hazards
- Tunnel expeditions:
- Segments traversed:
- Partial explorations:
- Mapped segments:
- Collapses:
- Flood events:
- Radiation events:
- Darkness checks:
- Gas events:
- Equipment requirements:
- Repairs:
- Bunkers:
- Deposits:
- Infrastructure:
- Seed vault:
- Surface connections:
- Travel planner:
- Events:
- Quests:
- UI:
- Unused content:
- Result:

### 167C — Integration / Validation
- Expedition:
- LocationEvolution:
- Knowledge:
- Cartography:
- Skills:
- Traits:
- Equipment:
- Salt mine:
- Weather/flood:
- Radiation:
- Gas/air:
- Thermal:
- Medical/fate:
- Save/load:
- Discovery rerolls:
- Collapse rerolls:
- Repair exploit:
- Route bypass:
- No-tunnel:
- Full network:
- All-collapsed:
- Graph integrity:
- Reachability:
- 200-day soak:
- Maintenance soak:
- Route balance:
- Playtest:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Tunnel network selftest:
- Port contract:
- Topology integrity:
- Old-save fixtures:
- Same-seed replay:
- Tunnel soak:
- Maintenance budget:
- UI/accessibility leakage:
- Verify fast:

### Final Metrics
- TUNNEL_SEGMENTS:
- TUNNEL_JUNCTIONS:
- SEGMENTS_KNOWN:
- SEGMENTS_MAPPED:
- SEGMENTS_PASSABLE:
- SEGMENTS_COLLAPSED:
- SEGMENTS_FLOODED:
- REPAIRS_COMPLETED:
- HIDDEN_BUNKERS_DISCOVERED:
- RESOURCE_DEPOSITS_DISCOVERED:
- TUNNEL_EXPEDITIONS:
- TUNNEL_TRAVEL_DAYS:
- SURFACE_TRAVEL_DAYS:
- COLLAPSE_REROLL_VIOLATIONS:
- GRAPH_REFERENCE_FAILURES:
- REQUIRED_PORTS_MISSING:

### Remaining Debt
- Tunnel content:
- Full-shelter tunnel entrance integration:
- Gas exposure:
- Mixed routes:
- Underground outposts:
- Tunnel trade/warfare:
- UI:
```

---

# 221. Final Execution Directive

Execute Plan 167 as a **subterranean topology and route-condition layer integrated into the existing world/expedition simulation**, not as a second copy of surface exploration.

The critical sequence is:

```text
author a stable 30-segment / 10-junction underground graph
→ connect it to canonical shelter/surface locations
→ reveal it progressively through real expedition travel
→ track legitimate dynamic segment integrity/accessibility
→ route exploration through ExpeditionSystem
→ route skill gains through SkillProgressionSystem
→ route equipment through inventory/condition systems
→ route hazards through radiation/flood/air/medical authorities
→ route discoveries into canonical locations/mining/resources
→ make repair/collapse persistent and idempotent
→ compare underground vs surface travel as a real strategic choice
→ prove long-run maintenance and hazard budgets are manageable
```

Do not create a second expedition scheduler.

Do not duplicate survivor injuries or deaths.

Do not give the tunnel layer its own radiation/flooding/temperature simulation.

Do not reveal hidden endpoints through UI metadata.

Do not let a collapsed segment reopen because the player reloaded.

Do not let tunnel travel become the universally optimal route.

The strongest authority rule is:

> **TunnelNetworkSystem owns underground connectivity, knowledge, integrity, accessibility, and maintenance; every survivor, item, hazard, location, extraction, and expedition consequence remains owned by the system that normally governs that fact.**

The strongest exploration rule is:

> **The network exists before the player knows it; exploration reveals stable topology rather than inventing new paths opportunistically.**

The strongest travel rule is:

> **Underground routes exchange surface danger for underground danger—they are alternatives, not free shortcuts.**

The flagship acceptance scenario is:

> **Start from an old-save-compatible shelter with one known tunnel entrance. Launch a real expedition with an `underground_navigator`, worn light source, rope, and inspection kit into the first unknown branch. Discover a junction, partially map one segment, identify a low-integrity branch, then encounter a flood-prone alternate route. Save/load before a seeded collapse check and prove the same segment collapses with the same outcome. Repair it using real materials and labor, traverse through to a hidden bunker and then to a canonical surface location, compare the resulting tunnel route against the surface route, and verify all injuries, tool wear, flooding, radiation, location discovery, mining unlocks, and thermal effects are handled by their canonical systems. Re-run with the same seed/actions: topology, hidden discoveries, hazard outcomes, repairs, and knowledge digest must reproduce exactly.**
