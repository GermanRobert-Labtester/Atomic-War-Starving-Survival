# C2 — Flagship Integration Plan [35]: Wasteland Cartography, Fog of War, Survey Quality, Map Trading, and Canonical World-Knowledge Progression

> **Deliverable:** `C2_planintegration[35].md`
> **Source scope:** Plan 163 — *Wasteland Cartography & Mapping*
> **Primary objective:** turn the wasteland map from a static destination list into a deterministic, progressively revealed knowledge surface where survivors explore routes, survey regions, discover locations/hazards/resources, improve map quality, teach cartography, use real mapping equipment, buy and sell map knowledge, and feed planning-quality benefits back into expedition decisions—without creating a second world topology, duplicate survivor skill ledger, duplicate equipment condition state, or parallel location-discovery authority.
> **Required execution order:** **163A Foundation/System Contract → 163B Fog of War, Surveying, Skill, Equipment, Trading & UI → 163C Cross-System Integration, Save/CI, Exploit Control, Balance, and Knowledge-State Closure**
> **Hard dependencies:** Plan 32 canonical world topology and knowledge ladder; `ExpeditionSystem`; `LocationEvolutionSystem`; `SkillProgressionSystem`; canonical item/equipment/condition systems; faction standing/coordinator; `MarketSystem`; Plan 31 semantic event vocabulary; Plan 36 ports; Plan 39 save durability; Plan 45 content acceptance; Plan 55 retention; ColonySystem (Plan 160) only when available.
> **Scope discipline:** no duplicate location graph, no duplicate route graph, no second “discovered” boolean that conflicts with Plan 32 knowledge state, no duplicate skill proficiency authority if cartography is represented by the canonical skill system, no duplicate equipment durability, no map-quality value stored as an unexplained global scalar, no trade-only world facts without a provenance/knowledge contract, no fog-of-war UI state used as gameplay truth, no map purchase that silently teleports discovery to “visited,” and no discovery reroll exploit via save/load or repeated panel opening.

---

# 0. Executive Intent

ASHFALL already has a world and already has travel.

It can:

- enumerate expedition destinations,
- maintain location state,
- run expeditions,
- reveal some travel content,
- track route and location data,
- evolve world locations,
- manage survivor skills and equipment.

What is missing is a coherent **knowledge layer for geography**.

The current experience trends toward:

```text
world exists
→ destinations are listed
→ player selects one
```

The target experience is:

```text
world exists
→ player knows only part of it
→ routes and rumors reveal possibilities
→ expeditions physically traverse space
→ survivors survey what they encounter
→ cartographic quality improves
→ maps can be traded/shared
→ location knowledge becomes more precise
→ planning becomes more reliable
```

The intended architecture is:

```text
canonical topology / locations
        │
        ▼
Plan 32 knowledge ladder
        │
        ├─ Unknown
        ├─ Rumoured
        ├─ Located
        ├─ Surveyed
        ├─ Visited
        └─ Mapped
        │
        ▼
CartographySystem
        │
        ├─ survey quality
        ├─ discovery provenance
        ├─ region coverage
        ├─ map artifacts/knowledge packets
        ├─ cartographer contribution
        └─ planning-quality projection
        │
        ▼
canonical owners
        │
   ┌────┼────────┬──────────┬───────────┐
   ▼    ▼        ▼          ▼           ▼
travel locations skills   equipment   market/factions
```

The strongest product outcome is:

> **The player begins with partial geographic knowledge, gradually turns rumors into surveyed and mapped places, can deliberately send skilled survivors to chart dangerous territory, can buy or sell maps without confusing “knowledge” with “physical visitation,” and sees mapping quality improve the reliability of expedition planning rather than merely filling a cosmetic percentage bar.**

---

# 1. Source Diagnosis

The source establishes:

- `ExpeditionSystem.cs` handles travel to predefined destinations,
- `LocationEvolutionSystem.cs` tracks location state,
- `SkillProgressionSystem.cs` exists,
- `locations.json` defines locations,
- there is no cartography system,
- the map is effectively known from the start,
- there is no fog of war,
- no mapping skill,
- no map trading,
- no map quality,
- no cartography-focused expedition type,
- 20 map regions are expected,
- mapping equipment should matter,
- map quality should degrade as the world changes,
- map trading should reveal knowledge without exploration,
- colonies may reveal surroundings,
- old saves, deterministic seeding, headless processing, UI, events, quests, and CI are all required.

However, Plan 32 already established the stronger canonical world-knowledge model:

```text
Unknown
→ Rumoured
→ Located
→ Surveyed
→ Visited
→ Mapped
```

Therefore this implementation must **not** regress the architecture to:

```text
MapRegion.discovered bool
```

as the primary world-knowledge truth.

Instead:

```text
Plan 32 knowledge state
= canonical place knowledge

CartographySystem
= survey/mapping quality + provenance + regional coverage + tradeable knowledge projection
```

This is the core design decision for C2[35].

---

# 2. Program-Level Success Criteria

C2[35] closes only when all of the following are true.

1. The world map starts with partial knowledge rather than full authoritative disclosure.
2. Fog of war is a projection of canonical knowledge state.
3. Shelter/home area is revealed according to explicit bootstrap rules.
4. Expeditions reveal knowledge only along actual traversed routes/visited areas.
5. Cartography-focused expeditions can intentionally improve regional mapping.
6. Buying a map can increase knowledge without marking a place physically visited.
7. Selling a map does not erase the player’s own knowledge.
8. Plan 32 knowledge states remain the canonical source for place discovery.
9. Cartography does not create a second route/location graph.
10. Region exploration/completeness is derived from underlying place/route knowledge.
11. Map quality is tracked per region/place/map artifact—not as one unexplained global truth.
12. Cartography skill uses or extends `SkillProgressionSystem`.
13. Mapping equipment uses actual item/equipment/condition systems.
14. Equipment degradation uses canonical condition authority.
15. Discovery and map-creation outcomes are deterministic under `ISeededRng`.
16. Save/load cannot reroll discovery, map quality, secret discovery, or traded knowledge.
17. Map trading uses canonical `MarketSystem` and faction-standing APIs.
18. Map value is based on actual quality, freshness, rarity, and information scope.
19. Stale maps become less accurate when canonical region state changes.
20. “Map quality” affects real expedition planning/risk/readability rather than only UI.
21. Cartography apprenticeships route through canonical skill/apprenticeship systems.
22. Colony reveal effects are optional and only active if Plan 160 exists.
23. 20 map regions validate against canonical locations/topology.
24. UI never exposes hidden location truth simply because the rendering layer knows it.
25. Headless CI proves fog/knowledge progression, mapping, trade, save/load, staleness, and deterministic replay.
26. All-discovered and no-discovery edge cases are valid.
27. Long-run map knowledge remains bounded and retention-aware.
28. No map purchase or survey action bypasses real content prerequisites silently.

---

# 3. Architectural Invariants

## 3.1 Plan 32 owns place knowledge

Canonical ladder:

```text
Unknown
Rumoured
Located
Surveyed
Visited
Mapped
```

Cartography reads/writes through that authority.

## 3.2 CartographySystem owns mapping detail, not world truth

It owns:

- survey quality,
- mapping provenance,
- region coverage,
- map artifacts/knowledge packets,
- freshness,
- cartographer contribution,
- mapping-task state.

It does not own:

- whether a location exists,
- route connectivity,
- actual location evolution,
- survivor skill XP,
- equipment durability,
- faction standing.

## 3.3 Fog of war is a UI/read-model projection

Never use hidden UI tile state as simulation truth.

## 3.4 Region completeness is derived

Example:

```text
mapped weighted POIs
+ surveyed routes
+ known hazards/resources
→ regional completeness
```

Do not increment a free-floating percentage independently.

## 3.5 Physical visitation and information acquisition are distinct

Buying a map may produce:

```text
Located / Surveyed / Mapped
```

depending quality and policy.

It may never produce:

```text
Visited
```

without actual travel.

## 3.6 Skill belongs to SkillProgressionSystem

If cartography becomes a new skill:

- register it there,
- use normal XP/tier semantics,
- do not persist a duplicate `proficiency` in `CartographyState`.

## 3.7 Equipment belongs to inventory/equipment systems

Cartography reads:

- compass,
- sextant,
- survey tools,
- cartography kit,
- condition.

It does not own those items.

## 3.8 Map quality has provenance

Every map knows:

- who produced it,
- when,
- what area it covers,
- source method,
- freshness,
- quality.

## 3.9 Map trade is knowledge trade

The traded good/reference transfers information, not world mutation by fiat.

## 3.10 World change can stale a map

Location evolution does not erase historical knowledge, but may reduce current planning reliability.

---

# 4. Dependency Graph

```text
Plan 32 topology + knowledge ladder
             │
             ▼
      CartographySystem
             │
   ┌─────────┼──────────────┐
   ▼         ▼              ▼
 survey   map artifacts   region coverage
   │         │              │
   ├─────────┼──────────────┤
   ▼         ▼              ▼
Expedition  Market/Faction  UI
   │
   ▼
LocationEvolution
   │
   ▼
staleness / remapping needs
```

Skill/equipment side:

```text
SkillProgressionSystem ───────┐
Inventory/Equipment ──────────┤
EquipmentConditionSystem ─────┤
                              ▼
                     mapping performance
```

---

# 5. Baseline Capture

Before implementation, inspect and record:

- Plan 32 place/route/knowledge APIs,
- existing map graph and route representation,
- `ExpeditionSystem` travel route lifecycle,
- `LocationEvolutionSystem` change/version semantics,
- current world-map UI visibility logic,
- `SkillProgressionSystem` skill registration/XP API,
- apprenticeship/teaching APIs if any,
- inventory/equipment query APIs,
- equipment-condition authority,
- `MarketSystem` item/value composition,
- faction standing/trade hooks,
- colony reveal hooks if present,
- save-section registration,
- current discovery/triangulation/damaged-map features.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/verify-fast.sh
```

Capture:

```text
knowledge state of all initial locations
route visibility
shelter/home region
map UI visible destinations
existing damaged-map data
```

This baseline is required because Plan 163 must integrate with—not overwrite—the Plan 32 knowledge ladder.

---

# 6. Workstream 163A — Foundation / System Contract

## Goal

Create one cartography authority for survey quality, map provenance, regional coverage, and deterministic map creation while delegating place knowledge, skills, equipment, and topology to their existing owners.

---

# 7. 163A Phase A — Create `CartographySystem`

Path:

```text
Assets/Ashfall.Core/Exploration/CartographySystem.cs
```

Responsibilities:

- coordinate mapping activity,
- calculate survey/mapping quality,
- create map records/artifacts,
- track region-level coverage projections,
- apply knowledge upgrades through Plan 32,
- manage map freshness/staleness,
- expose trade/planning assessments,
- capture/restore cartography-specific state.

---

# 8. 163A Phase B — Do Not Adopt Source DTOs Literally

The source proposes:

```text
MapRegion.discovered bool
CartographySkill.proficiency
CartographyState.map completeness
```

These would duplicate existing authorities.

Replace with:

```text
MapRegionDefinition
MapSurveyRecord
MapArtifact
CartographyActivity
RegionCartographySnapshot
MapTradeAssessment
MapPlanningAssessment
```

---

# 9. 163A Phase C — `MapRegionDefinition`

Static authored fields:

```text
region_id
name_key
terrain_type
location_ids
route_ids
hazard_domains
survey_weight
rarity
map_trade_policy
```

No runtime `discovered` bool.

---

# 10. 163A Phase D — `MapSurveyRecord`

Persistent mapping-history record:

```text
survey_id
region_id
location_id optional
route_id optional
survivor_id
day
method
quality
freshness_version
source_event_id
tools_used
```

---

# 11. 163A Phase E — `MapArtifact`

If maps are tradeable goods/knowledge packages, define:

```text
map_id
region_id
coverage_scope
quality
created_day
created_by
source_type
freshness_version
rarity
knowledge_payload
```

Do not store copied location definitions.

---

# 12. 163A Phase F — `RegionCartographySnapshot`

Derived read model:

```text
region_id
knowledge_band
coverage_percent
survey_quality
freshness
known_locations
known_routes
known_hazards
known_resources
known_secrets
planning_confidence
```

Do not persist this snapshot.

---

# 13. 163A Phase G — `map_regions.json`

Create:

```text
Assets/StreamingAssets/Data/map_regions.json
```

Fields:

```text
region_id
name_key
terrain
location_ids
route_ids
default_visibility_policy
survey_weights
rarity
trade_policy
localization keys
```

---

# 14. 163A Phase H — 20 Region Requirement

Source requires:

```text
20 map regions
```

Do not invent 20 arbitrary regions before topology audit.

Procedure:

1. inventory canonical locations,
2. cluster by geography/topology,
3. ensure every region has real locations/routes,
4. avoid overlapping ambiguous ownership unless hierarchy is explicit,
5. create exactly 20 validated rows if world content supports it.

If topology cannot support 20 meaningful regions:

```text
fail content-authoring prerequisite
```

rather than padding with empty data.

---

# 15. 163A Phase I — Region Coverage Formula

Coverage is derived.

Example:

```text
weighted mapped locations
+ weighted surveyed routes
+ hazard/resource knowledge
--------------------------------
total authored survey weight
```

Keep formula deterministic.

---

# 16. 163A Phase J — Map Completeness

Global map completeness is:

```text
weighted average of regional coverage
```

derived for UI/achievements.

Never persist as authority.

---

# 17. 163A Phase K — Knowledge Upgrade API

Cartography requests knowledge transitions through Plan 32.

Example:

```text
Unknown → Rumoured
Rumoured → Located
Located → Surveyed
Surveyed → Mapped
```

Physical travel separately sets/qualifies:

```text
Visited
```

according to Plan 32 semantics.

---

# 18. 163A Phase L — No Illegal State Regression

Cartography may never downgrade:

```text
Visited → Located
Mapped → Rumoured
```

Staleness affects quality/planning confidence, not existential knowledge.

---

# 19. 163A Phase M — Fog of War Projection

Map UI projection:

```text
Unknown
→ opaque fog

Rumoured
→ vague region/marker

Located
→ approximate position

Surveyed
→ confirmed details

Visited
→ visitation marker/history

Mapped
→ precise cartographic presentation
```

Exact visuals belong to UI layer.

---

# 20. 163A Phase N — Shelter Bootstrap Knowledge

Old/new campaign starts with:

- shelter location known,
- immediate region state according to authored bootstrap,
- nearby routes only if fiction/system says known.

Do not reveal all location IDs merely because catalog loader sees them.

---

# 21. 163A Phase O — Cartography Skill Authority

Audit whether a cartography/navigation skill already exists.

If absent:

```text
register cartography with SkillProgressionSystem
```

Fields belong to canonical skill system:

- XP,
- level/tier.

Specialization may be:

- skill specialization data,
- survivor trait/tag,
- cartography-specific specialization reference if canonical skill system lacks one.

Avoid duplicate proficiency 0–100.

---

# 22. 163A Phase P — Skill Contribution

Mapping performance can read:

```text
skill tier / XP-derived proficiency
terrain specialization
fatigue
health
```

No separate hidden skill.

---

# 23. 163A Phase Q — Terrain Specializations

Source:

```text
terrain
urban
industrial
wasteland
```

Reconcile with actual region terrain vocabulary.

Possible final set:

```text
urban
rural
industrial
wasteland
water/coastal
```

Only add supported terrains.

---

# 24. 163A Phase R — Mapping Equipment Authority

Use real items.

Source concepts:

```text
Compass
Sextant
Survey tools
Cartography kit
```

Audit item catalog first.

Reuse existing items before creating new IDs.

---

# 25. 163A Phase S — Equipment Effects

Source seed bonuses:

```text
compass +10% range
sextant +20% accuracy
survey tools +30% quality
cartography kit +50% all bonuses
```

Translate into bounded data-authored modifiers.

Do not allow additive stacking to create absurd >100% quality/range.

---

# 26. 163A Phase T — Equipment Condition

Condition reduces effective tool contribution.

Use `EquipmentConditionSystem`.

No cartography-owned durability.

---

# 27. 163A Phase U — Mapping Activity Types

Typed:

```text
RouteObservation
RegionSurvey
LocationSurvey
HazardSurvey
ResourceSurvey
CartographyExpedition
MapRevision
TradeImport
ColonyRecon
```

---

# 28. 163A Phase V — Discovery Method Provenance

Every knowledge improvement records one method:

```text
travel
survey
trade
rumor
artifact
colony
quest
```

This allows later reliability/explainability.

---

# 29. 163A Phase W — Quality Model

Source bands:

```text
Rough: 0–33
Standard: 33–66
Detailed: 66–100
```

Fix boundary ambiguity explicitly.

Recommended:

```text
Rough: 0–32
Standard: 33–65
Detailed: 66–100
```

or:

```text
[0,33)
[33,66)
[66,101]
```

Use one canonical threshold definition.

---

# 30. 163A Phase X — “Precise” vs “Detailed”

The source text says rough → standard → detailed, but detailed is described as “precise locations.”

Do not add an undocumented fourth tier unless desired.

Keep three tiers and expose numerical quality internally.

---

# 31. 163A Phase Y — Quality Calculation

Inputs:

```text
cartography skill
terrain specialization
tool modifiers
time spent
survey method
weather/visibility if relevant
route safety interruption
existing prior map quality
```

Output bounded 0–100.

---

# 32. 163A Phase Z — Deterministic RNG

Use dedicated stream:

```text
cartography
```

RNG may affect:

- discovery opportunity,
- hidden feature find,
- survey uncertainty.

Core quality calculation should be mostly deterministic.

---

# 33. 163A Phase AA — Stable RNG Keys

Use:

```text
campaign seed
survey activity ID
region/location ID
survivor ID
day
discovery slot
```

Candidate lists sorted.

---

# 34. 163A Phase AB — Discovery Idempotency

Each discovery has stable key:

```text
region
feature/location
discovery method event ID
```

Reload/repeated UI cannot duplicate.

---

# 35. 163A Phase AC — Freshness / Region Version

LocationEvolutionSystem should expose:

```text
region/location revision
```

or a stable change version/event sequence.

Map artifact stores:

```text
observed_revision
```

---

# 36. 163A Phase AD — Staleness

When current region revision exceeds map revision:

```text
map freshness declines
planning confidence declines
```

Do not erase historical discovery.

---

# 37. 163A Phase AE — Map Revision

A new survey can:

- update stale details,
- increase quality,
- preserve prior provenance.

No duplicate “new map” required unless player creates a separate artifact.

---

# 38. 163A Phase AF — Save State

Persist only:

```text
survey records
map artifacts/knowledge packets
mapping activity state
idempotency keys
specialization metadata if not owned elsewhere
schema version
```

Do not persist:

- world graph,
- location definitions,
- skill XP copy,
- equipment state copy,
- region completeness snapshot.

---

# 39. 163A Phase AG — Old Save Compatibility

Missing cartography section:

```text
valid
```

Bootstrap from Plan 32 current knowledge.

Do not regress already visited/mapped places.

---

# 40. 163A Phase AH — Old Save Default

Source says:

```text
shelter region revealed
```

Correct migration rule:

```text
preserve all canonical knowledge already present
+ ensure shelter region meets minimum bootstrap knowledge
```

Never hide a location the old save already legitimately knows.

---

# 41. 163A Phase AI — Port Contract

Required sinks/sources:

- Plan 32 knowledge service,
- ExpeditionSystem,
- LocationEvolutionSystem,
- SkillProgressionSystem,
- equipment/condition,
- MarketSystem,
- faction standing,
- optional ColonySystem,
- quest/journal/event.

Missing mandatory port fails validation.

---

# 42. 163A Phase AJ — Semantic Events

Candidate kinds:

```text
region_discovered
location_mapped
route_surveyed
map_created
map_updated
map_traded
map_purchased
hidden_location_discovered
cartography_mastery_reached
```

Use Plan 31 vocabulary governance.

---

# 43. 163A Phase AK — Diagnostics

Expose:

```text
MAP_REGIONS_TOTAL
REGIONS_KNOWN
REGIONS_SURVEYED
REGIONS_MAPPED
MAP_ARTIFACTS
STALE_MAPS
CARTOGRAPHY_SKILL_USERS
UNRESOLVED_MAP_REFS
REQUIRED_PORTS_MISSING
```

---

# 44. 163A Tests

- knowledge ladder transitions,
- physical visitation distinction,
- region coverage derivation,
- quality thresholds,
- equipment modifiers,
- condition scaling,
- deterministic survey,
- idempotency,
- staleness,
- old-save preservation,
- missing-port failure.

---

# 45. 163A Definition of Done

- [ ] CartographySystem,
- [ ] Plan 32 knowledge authority reused,
- [ ] no `discovered bool` as canonical truth,
- [ ] MapRegionDefinition,
- [ ] MapSurveyRecord,
- [ ] MapArtifact,
- [ ] derived region snapshot,
- [ ] map_regions.json,
- [ ] 20 validated regions,
- [ ] coverage/completeness derivation,
- [ ] cartography skill integration,
- [ ] terrain specializations,
- [ ] real mapping equipment,
- [ ] condition integration,
- [ ] three explicit quality bands,
- [ ] deterministic RNG,
- [ ] discovery idempotency,
- [ ] freshness/staleness model,
- [ ] save/old-save,
- [ ] ports,
- [ ] semantic events,
- [ ] diagnostics.

---

# 46. Workstream 163B — Fog of War, Surveying, Skill, Equipment, Trading & UI

## Goal

Implement the player-facing exploration loop: unknown geography, route reveal, deliberate cartography expeditions, skill/tool progression, tradeable maps, quality/freshness, events/quests, and a map interface that exposes only earned knowledge.

---

# 47. 163B Phase A — Fog-of-War Rules

Initial world map should render by canonical knowledge state.

Minimum new-game reveal:

- shelter/home,
- immediate safe knowledge,
- authored starting rumors/intel.

Everything else remains hidden or approximate.

---

# 48. 163B Phase B — Unknown Region Rendering

Unknown:

```text
fog / silhouette / no exact marker
```

Do not expose hidden location names in tooltip/accessibility tree.

This is important: accessibility labels must not leak hidden truth.

---

# 49. 163B Phase C — Rumoured Rendering

Rumoured regions/locations may show:

- approximate area,
- vague category,
- uncertainty.

No exact coordinates unless knowledge level permits.

---

# 50. 163B Phase D — Located Rendering

Located:

- map marker,
- approximate or exact position according to Plan 32 definition,
- basic route knowledge if known.

---

# 51. 163B Phase E — Surveyed Rendering

Surveyed:

- confirmed terrain,
- known hazards,
- known resource indicators,
- better route confidence.

---

# 52. 163B Phase F — Visited Rendering

Visited:

- physical visit history,
- current known location state,
- visitation marker.

Visited does not necessarily imply a high-quality regional map.

---

# 53. 163B Phase G — Mapped Rendering

Mapped:

- high-confidence geometry,
- detailed known POIs,
- route/hazard information according to map quality.

---

# 54. 163B Phase H — Route Reveal

Expedition route traversal emits mapping opportunities along actual path.

No reveal of neighboring unrelated region without rule.

---

# 55. 163B Phase I — Passive Observation

Normal expeditions may grant:

```text
limited route/location knowledge
```

based on:

- visibility,
- survivor skill,
- route traversal,
- time.

---

# 56. 163B Phase J — Cartography Expedition

Create specialized expedition intent:

```text
MapRegion
SurveyRoute
SurveyLocation
```

using existing ExpeditionSystem.

Do not create a separate travel scheduler.

---

# 57. 163B Phase K — Cartography Expedition Cost

Costs:

- survivor time,
- supplies,
- exposure/risk,
- equipment wear,
- opportunity cost.

Use existing systems.

---

# 58. 163B Phase L — Cartography Expedition Success

Result can include:

- better coverage,
- higher map quality,
- hidden location chance,
- hazard/resource survey.

Success does not mean no expedition risk.

---

# 59. 163B Phase M — Targeting Unknown Regions

Player may target:

- rumoured region,
- edge/frontier,
- known route corridor.

Do not allow exact clicking of a completely hidden precise location unless some intel exists.

---

# 60. 163B Phase N — Skill Progression

Mapping activity awards cartography XP through SkillProgressionSystem.

XP sources:

```text
new region surveyed
route mapped
location mapped
map revised after change
high-quality map completed
```

Avoid farming repeated low-value surveys.

---

# 61. 163B Phase O — XP Anti-Farming

Diminish or zero XP for:

- repeatedly mapping unchanged fully mapped region,
- re-opening UI,
- buying own previously sold map,
- trivial same-route observations.

---

# 62. 163B Phase P — Apprenticeship

Cartography can be taught via canonical apprenticeship if supported.

Mentor/student:

- real survivors,
- real skill XP,
- real time.

No duplicate “cartography teaching” XP ledger.

---

# 63. 163B Phase Q — Terrain Specialization

A specialist gains bounded bonus when survey terrain matches.

This affects:

- quality,
- time,
- hidden-feature detection.

No binary “can/cannot map” gate unless content says so.

---

# 64. 163B Phase R — Mapping Equipment Audit

Audit existing item catalog for:

- compass,
- sextant,
- binoculars,
- survey instruments,
- notebooks/map kit,
- navigation tools.

Reuse where possible.

---

# 65. 163B Phase S — Compass

Role:

- navigation/range support.

Do not literally increase world-space reveal radius without route/travel context.

Translate source `+10% discovery range` into survey opportunity/route observation modifier.

---

# 66. 163B Phase T — Sextant

Role:

- positional accuracy where relevant.

If world setting/geography does not support meaningful sextant usage in inland/local scale, consider:

- survey optics,
- rangefinder,
- compass/chronometer

instead.

Preserve intent, not anachronistic mechanic.

---

# 67. 163B Phase U — Survey Tools

Improve:

- quality,
- route/hazard accuracy,
- time efficiency.

---

# 68. 163B Phase V — Cartography Kit

Composite/high-tier item or loadout.

If it duplicates all individual items, define slot/stacking rules.

Do not grant full bonus plus all components additively without cap.

---

# 69. 163B Phase W — Equipment Crafting/Trade

Items use canonical crafting/market systems.

No cartography-only inventory.

---

# 70. 163B Phase X — Equipment Wear

Mapping use may degrade relevant tools through `EquipmentConditionSystem`.

No duplicate condition field.

---

# 71. 163B Phase Y — Tool Loss/Breakage

Broken tool:

- reduces mapping performance,
- remains governed by equipment/repair systems.

---

# 72. 163B Phase Z — Map Artifact Creation

A survivor can create a map when sufficient survey information exists.

Inputs:

```text
region knowledge
skill
tools
time
medium/material if modeled
```

---

# 73. 163B Phase AA — Map Artifact Scope

Map may cover:

- region,
- route,
- settlement cluster,
- hazard corridor.

Avoid one monolithic “whole world map item” until endgame.

---

# 74. 163B Phase AB — Map Material Cost

Only add paper/ink/material consumption if those items exist or are intentionally added.

Do not create flavor-only hidden costs.

---

# 75. 163B Phase AC — Map Quality

Quality derives from:

- underlying survey knowledge,
- cartographer skill,
- tools,
- freshness.

It cannot exceed the quality of source knowledge.

---

# 76. 163B Phase AD — No Information Creation by Copying

Copying a rough map:

```text
cannot produce a detailed map
```

without additional survey/knowledge.

---

# 77. 163B Phase AE — Map Trading Model

Map trading uses:

```text
MarketSystem
+ MapTradeAssessment
```

A map has value based on:

- coverage,
- quality,
- freshness,
- rarity,
- buyer knowledge gap,
- strategic relevance.

---

# 78. 163B Phase AF — Buyer Knowledge Gap

A faction that already knows the region should value the map less.

This prevents infinite sale loops.

---

# 79. 163B Phase AG — Seller Knowledge Preservation

Selling a map does not make the player forget the region.

Trade transfers a copy/knowledge package.

---

# 80. 163B Phase AH — Buying Maps

Purchased map applies knowledge payload through Plan 32.

Possible transitions:

```text
Unknown → Rumoured
Unknown/Rumoured → Located
Located → Surveyed
```

High-quality maps may achieve `Mapped` only if design permits third-party maps to satisfy that state.

Never set `Visited`.

---

# 81. 163B Phase AI — Trade-Only Region Rule

Source says some regions may only be discoverable through trade.

Use cautiously.

A region can require:

```text
external map/intel to become targetable
```

but once known, physical exploration should remain possible.

Avoid permanent vendor lockout unless fiction demands it.

---

# 82. 163B Phase AJ — Faction Standing

Selling valuable maps may improve standing through canonical faction APIs.

Standing reward depends on:

- strategic value,
- novelty,
- trust.

No flat standing farm per repeated sale.

---

# 83. 163B Phase AK — Map Purchase Pricing

Faction/market sets price.

No cartography-owned currency.

---

# 84. 163B Phase AL — Map Authenticity

If false/inaccurate maps are supported later:

- provenance/quality must model reliability.

Do not add deceptive map system in baseline unless another plan requires it.

---

# 85. 163B Phase AM — Stale Maps

When `LocationEvolutionSystem` changes:

- route closed,
- settlement destroyed,
- hazard moved,
- resource depleted,

map freshness decreases.

---

# 86. 163B Phase AN — Stale Does Not Mean Useless

Old map may still retain:

- terrain,
- old route,
- landmark,
- approximate location.

Planning confidence falls selectively.

---

# 87. 163B Phase AO — Map Update

Revisit/survey/trade new intelligence to update.

Record new revision.

---

# 88. 163B Phase AP — Planning Effects

Higher-quality/fresher maps may improve:

- travel-time estimate accuracy,
- hazard prediction,
- route selection confidence,
- resource-location certainty.

Do not directly grant arbitrary expedition-success bonus unless based on actual planning inputs.

---

# 89. 163B Phase AQ — Route Optimization

If ExpeditionSystem supports path selection:

- better maps expose/score better known routes.

If not:

- map improves estimate/risk warning only.

Do not fabricate invisible shortcut bonuses.

---

# 90. 163B Phase AR — Hazard Knowledge

Mapped hazard data uses canonical hazard IDs/state.

A map can be stale if hazard moved/resolved.

---

# 91. 163B Phase AS — Resource Knowledge

Resource indicators should be:

- categorical,
- approximate,
- freshness-sensitive.

Do not reveal exact loot tables unless intended.

---

# 92. 163B Phase AT — Hidden Features

High-quality mapping can unlock hidden locations/secrets.

Use seeded discovery + authored eligibility.

---

# 93. 163B Phase AU — Secret Discovery Fairness

A secret must have:

- region eligibility,
- minimum survey quality,
- deterministic discovery slot/chance,
- idempotent result.

No UI refresh rerolls.

---

# 94. 163B Phase AV — Colonies Reveal Surroundings

If Plan 160 is live:

```text
colony founded
→ surrounding region knowledge increases
```

through Plan 32 knowledge authority.

Quality depends on colony scouting/cartography capability if modeled.

---

# 95. 163B Phase AW — Map Journal

Log significant discoveries:

- new region,
- major location,
- hidden landmark,
- masterwork map.

Avoid logging every +1% coverage tick.

---

# 96. 163B Phase AX — Cartography Events

Source examples:

```text
The Discovery
The Map
The Trade
The Expedition
The Masterwork
The Secret
The Update
```

Use canonical event framework.

---

# 97. 163B Phase AY — “The Discovery”

Trigger on meaningful region/location knowledge advancement.

---

# 98. 163B Phase AZ — “The Map”

High-quality map completed.

---

# 99. 163B Phase BA — “The Trade”

First/major strategic map sale or purchase.

Avoid every routine transaction generating narrative event.

---

# 100. 163B Phase BB — “The Expedition”

First dedicated mapping expedition.

---

# 101. 163B Phase BC — “The Masterwork”

Master cartographer creates 95–100 quality map under real requirements.

No free “perfect map” from skill alone.

---

# 102. 163B Phase BD — “The Secret”

Hidden location discovered through survey.

---

# 103. 163B Phase BE — “The Update”

Stale strategic map refreshed after major region change.

---

# 104. 163B Phase BF — Quest Hooks

Source:

```text
The Explorer
The Cartographer
The Mapmaker
The Trade
The Secret
The Legacy
The Expedition
```

Use canonical quest runtime.

---

# 105. 163B Phase BG — “Discover All Regions”

Quest completion must read canonical 20-region coverage/knowledge.

No UI percentage shortcut.

---

# 106. 163B Phase BH — “Master Cartography Skill”

Use SkillProgressionSystem mastery.

---

# 107. 163B Phase BI — “Definitive Wasteland Atlas”

Endgame map artifact requires:

- high regional coverage,
- freshness threshold,
- cartography mastery,
- actual creation step.

---

# 108. 163B Phase BJ — Map UI

World map displays:

- fog/knowledge state,
- region boundaries,
- locations,
- routes,
- hazards/resources,
- map quality,
- freshness.

---

# 109. 163B Phase BK — Map Quality Indicator

Show:

```text
Rough / Standard / Detailed
```

plus numeric percentage only if consistent with UI style.

---

# 110. 163B Phase BL — Freshness Indicator

Examples:

```text
Current
Aging
Stale
Outdated
```

derived from world revisions.

---

# 111. 163B Phase BM — Discovery Filters

Filter:

```text
locations
hazards
resources
landmarks
secrets
routes
```

Only known data rendered.

---

# 112. 163B Phase BN — Region Tooltip

Show:

- knowledge state,
- coverage,
- quality,
- freshness,
- last surveyed day,
- known hazards/resources.

No hidden facts.

---

# 113. 163B Phase BO — Route Planning Panel

If route planner exists:

- show confidence ranges based on map quality.

Example:

```text
Travel time: 1.8–2.5 days
Hazard confidence: Moderate
Map freshness: Aging
```

---

# 114. 163B Phase BP — Accessibility / Fog Leakage

Accessibility tree must not expose:

- hidden location labels,
- hidden coordinates,
- secret POIs.

Test explicitly.

---

# 115. 163B Phase BQ — Keyboard/Controller

Map pan/zoom/filter/focus behavior must follow Plan 37 input architecture.

---

# 116. 163B Phase BR — Tutorial

First discovery explains:

- fog of war,
- knowledge levels,
- mapping quality,
- cartography expedition,
- map trade.

Keep progressive, not one giant tutorial.

---

# 117. 163B Phase BS — Localization

Region names, map quality labels, event/quest text, tooltips use localization keys.

---

# 118. 163B Phase BT — 20-Region Coverage Matrix

Generate:

| Region | Locations | Routes | Terrain | Start state | Survey producers | Trade availability | Runtime observed |
|---|---:|---:|---|---|---|---|---:|

---

# 119. 163B Phase BU — Equipment Coverage Matrix

Generate:

| Tool | Item ID | Effect | Condition scaling | Craft source | Trade source | Runtime used |
|---|---|---|---|---|---|---:|

---

# 120. 163B Phase BV — Map Artifact Coverage

Report:

```text
maps created
maps sold
maps bought
maps updated
stale maps
masterwork maps
```

---

# 121. 163B Phase BW — Content Utilization

Run 100/200-day exploration scenario.

Report:

```text
regions known
regions surveyed
regions mapped
routes mapped
locations discovered
hidden locations
skill XP
tools used
map trades
```

---

# 122. 163B Phase BX — Dead Region/Tool Rule

Never-observed region or mapping tool:

- fix content,
- mark intentionally late/rare,
- remove,
- exempt with reason.

---

# 123. 163B Definition of Done

- [ ] fog of war,
- [ ] Plan 32 knowledge rendering,
- [ ] route reveal,
- [ ] passive survey,
- [ ] dedicated cartography expeditions,
- [ ] skill progression,
- [ ] apprenticeship,
- [ ] terrain specializations,
- [ ] mapping equipment,
- [ ] equipment condition/degradation,
- [ ] map artifacts,
- [ ] map quality,
- [ ] trade assessment,
- [ ] buying/selling,
- [ ] faction standing,
- [ ] buyer novelty protection,
- [ ] map freshness/staleness,
- [ ] planning-quality effects,
- [ ] hazards/resources/secrets,
- [ ] optional colony reveal,
- [ ] journal,
- [ ] 7 events,
- [ ] 7 quests,
- [ ] map UI,
- [ ] filters/tooltips,
- [ ] accessibility,
- [ ] tutorial/localization,
- [ ] utilization reports.

---

# 124. Workstream 163C — Cross-System Integration, Save/CI, Exploit Control, Balance, and Knowledge-State Closure

## Goal

Prove cartography advances the canonical world-knowledge model, integrates with travel/location evolution/skills/equipment/market/factions, survives save/load, prevents information/XP/trade exploits, and creates strategic map progression rather than arbitrary fog.

---

# 125. 163C Phase A — ExpeditionSystem Integration

Expedition emits:

```text
route segment traversed
region entered
location visited
observation opportunity
cartography task completed
```

Cartography consumes.

---

# 126. 163C Phase B — Travel Knowledge Separation

Expedition may cause:

```text
Visited
```

Cartography may cause:

```text
Surveyed / Mapped
```

Do not conflate.

---

# 127. 163C Phase C — LocationEvolutionSystem Integration

Region/location change invalidates relevant map freshness.

Do not alter knowledge-state existence.

---

# 128. 163C Phase D — SkillProgression Integration

Every cartography XP award uses canonical skill API.

No duplicate proficiency.

---

# 129. 163C Phase E — Equipment Integration

Tool bonuses query actual equipped/carried items.

No free tool flags.

---

# 130. 163C Phase F — EquipmentCondition Integration

Use real condition.

Broken tool contribution updates immediately.

---

# 131. 163C Phase G — MarketSystem Integration

Map artifacts are valid goods/knowledge commodities.

Final price determined by market.

---

# 132. 163C Phase H — Faction Standing Integration

Map trade standing effects use canonical faction API with reason IDs.

---

# 133. 163C Phase I — Colony Integration

Optional.

Colonies reveal surroundings via canonical knowledge update.

No CartographySystem-owned colony state.

---

# 134. 163C Phase J — Quest Integration

Quest triggers use canonical region/map facts.

No map UI completion hacks.

---

# 135. 163C Phase K — Journal/Archive Integration

Plan 162 archive may record:

- first region mapped,
- definitive atlas,
- hidden landmark,
- strategic map trade.

Archive remains historical consumer.

---

# 136. 163C Phase L — Old Save Migration

Preserve all existing Plan 32 knowledge.

Initialize:

- survey records only where safely reconstructible,
- map artifacts empty unless real prior data exists.

Do not retroactively create maps.

---

# 137. 163C Phase M — Save/Load Matrix

Test at:

```text
unknown region
rumoured
located
mid-survey
survey completed
map artifact created
map sold
map bought
map stale
map updated
```

Exact state persists.

---

# 138. 163C Phase N — Discovery Save-Scum Prevention

Reload cannot reroll:

- hidden location,
- survey result,
- quality,
- route discovery.

Persist activity/discovery IDs.

---

# 139. 163C Phase O — UI Refresh Exploit

Opening/closing map never changes:

- coverage,
- quality,
- knowledge,
- XP,
- discovery.

---

# 140. 163C Phase P — Mapping XP Exploit

Repeatedly surveying unchanged fully known region:

```text
minimal/zero XP
```

according to policy.

---

# 141. 163C Phase Q — Trade Loop Exploit

Prevent:

```text
sell map
buy it back
sell again
→ infinite wealth/standing
```

Use:

- buyer knowledge gap,
- ownership history,
- transaction value drop,
- no repeated standing for identical knowledge.

---

# 142. 163C Phase R — Map Duplication Exploit

If map artifacts can be copied:

- copy cost/time,
- no information quality gain,
- sale value declines for known buyer.

---

# 143. 163C Phase S — Purchase-Reveal Exploit

Buying one map should apply exactly its payload.

No reveal of all POIs in region unless map quality/coverage says so.

---

# 144. 163C Phase T — Trade-Only Secret Guard

A trade-only starting clue may be required.

But after purchase:

- player can physically explore,
- later map it personally.

Avoid permanent NPC monopoly.

---

# 145. 163C Phase U — No Discoveries Edge Case

World map:

- shelter/bootstrap area visible,
- remaining fog valid,
- no errors.

---

# 146. 163C Phase V — All Discovered Edge Case

Full world:

- no fog,
- completeness derives 100 if all weighted content mapped,
- cartography can still update stale regions,
- no repeated completion rewards.

---

# 147. 163C Phase W — No Cartographer Edge Case

Normal expeditions still reveal minimal route/location knowledge.

Formal high-quality mapping is slower/limited.

Do not soft-lock exploration.

---

# 148. 163C Phase X — No Equipment Edge Case

Mapping works at lower quality.

No hard lock unless a particular specialized survey requires tool.

---

# 149. 163C Phase Y — Master Cartographer Edge Case

Mastery improves efficiency/quality but cannot:

- discover nonexistent content,
- bypass access hazards,
- create quality above source knowledge.

---

# 150. 163C Phase Z — Stale World Edge Case

Mass location evolution event.

Assert:

- maps become stale selectively,
- knowledge remains,
- update tasks appear,
- UI accurately explains uncertainty.

---

# 151. 163C Phase AA — Topology Integrity

All 20 regions:

- contain valid locations/routes,
- map to canonical graph,
- no orphan region references.

---

# 152. 163C Phase AB — Knowledge-Ladder Integrity

For every location:

```text
allowed transitions only
```

No CartographySystem local flags bypass Plan 32.

---

# 153. 163C Phase AC — Planning Parity

UI planning estimate uses exact same map-quality/freshness assessment as expedition planning.

No duplicate formula.

---

# 154. 163C Phase AD — Map Quality Ceiling

Quality <= 100.

No equipment/skill stacking overflow.

---

# 155. 163C Phase AE — Freshness Floor

Stale map still retains minimum historical value.

Do not automatically drop to 0.

---

# 156. 163C Phase AF — Hidden Secret Idempotency

Secret location discovered once.

No duplicate quest/location creation.

---

# 157. 163C Phase AG — `--cartography-selftest`

Required scenarios:

1. new-game fog,
2. shelter bootstrap reveal,
3. route traversal reveal,
4. cartography expedition,
5. skill progression,
6. tool-quality bonus,
7. damaged-tool reduction,
8. map creation,
9. map sale,
10. map purchase,
11. bought map does not mark visited,
12. world evolution stales map,
13. map revision,
14. old save,
15. all-discovered,
16. no-cartographer,
17. no-equipment,
18. hidden-secret idempotency.

---

# 158. 163C Phase AH — Data Integrity

Validate:

- 20 region IDs,
- location refs,
- route refs,
- terrain IDs,
- mapping-tool item IDs,
- skill IDs,
- quest IDs,
- localization,
- quality thresholds,
- trade policies.

---

# 159. 163C Phase AI — Deliberate Failure Proof

Break:

- invalid region location ID,
- duplicate region ID,
- illegal knowledge-state transition,
- quality >100,
- missing market/faction port.

Assert gate/selftest fails.

---

# 160. 163C Phase AJ — Same-Seed Replay

Same:

```text
seed
route choices
survey choices
trades
```

→ same:

```text
discoveries
quality
hidden finds
map artifact IDs
knowledge digest
```

---

# 161. 163C Phase AK — 200-Day Exploration Soak

Record:

```text
regions discovered
coverage
mapped regions
locations found
routes mapped
cartography XP
tool wear
map trades
stale maps
map updates
```

---

# 162. 163C Phase AL — Exploration Strategy Profiles

Run:

```text
direct_expedition
cartography_heavy
trade_heavy
balanced
minimal_mapping
```

Compare:

- world knowledge,
- resource cost,
- expedition losses,
- trade value,
- time to strategic regions.

No one strategy should dominate all axes.

---

# 163. 163C Phase AM — Fog Fairness Test

Ask:

```text
Did the player have a discoverable route to useful knowledge?
Was a hidden destination meaningfully telegraphed?
Did fog obscure information the simulation claimed the player knew?
Did the UI leak information the player had not earned?
```

---

# 164. 163C Phase AN — Planning Value Test

Compare expedition estimates under:

```text
Unknown
Rough
Standard
Detailed
Detailed but stale
```

Better maps should improve prediction/choice quality.

Do not necessarily increase raw success chance directly.

---

# 165. 163C Phase AO — Trade Value Test

Compare map value for buyer who:

```text
knows nothing
knows rough region
already owns equivalent map
owns fresher map
```

Value should reflect novelty.

---

# 166. 163C Phase AP — Skill Value Test

Compare novice vs master on same survey.

Master should:

- map faster,
- reach higher quality,
- detect more detail.

Still bounded by actual access/tools/world state.

---

# 167. 163C Phase AQ — Tool Value Test

Compare:

- no tools,
- compass,
- survey tools,
- full kit.

Verify material benefit without mandatory hard lock.

---

# 168. 163C Phase AR — Staleness Value Test

After world evolution:

- detailed old map should outperform no map,
- but underperform current detailed map.

---

# 169. 163C Phase AS — Accessibility

World map:

- hidden items absent from semantic tree,
- fog state has accessible labels,
- pan/zoom keyboard/controller,
- filter focus order,
- non-color quality/freshness indicators.

---

# 170. 163C Phase AT — Headless Behavior

Discovery, survey, trade, staleness, and knowledge upgrades run without UI.

---

# 171. 163C Phase AU — Retention

Plan 55 policy:

Keep:

- current best map per region,
- landmark discoveries,
- masterwork/definitive atlas,
- significant trade/secret events.

Roll up:

- obsolete survey samples,
- superseded low-quality maps if not player-owned artifacts.

---

# 172. 163C Phase AV — Archive / Legacy

Plan 162 can record:

- first region mapped,
- cartography masterwork,
- definitive atlas,
- major secret discovery.

Plan 140/meta legacy may consume only approved summaries.

---

# 173. 163C Phase AW — Performance Budget

Large map should not:

- scan all content every frame,
- recompute all regions every UI tick.

Use:

- event-driven invalidation,
- cached derived snapshot with invalidation,
- on-demand search/render.

---

# 174. 163C Phase AX — Long-Map Save Budget

Measure:

```text
20 regions
all routes
all locations
survey history
multiple map artifacts
```

Ensure bounded size.

---

# 175. 163C Phase AY — Human Playtest

Evaluate:

```text
Does fog create curiosity rather than confusion?
Do maps feel valuable?
Does cartography feel like a real specialization?
Does buying knowledge feel different from exploring it?
Do stale maps create understandable uncertainty?
```

Human review.

---

# 176. 163C Phase AZ — Documentation

Create:

```text
docs/systems/CARTOGRAPHY_AND_WORLD_KNOWLEDGE.md
```

Include:

- Plan 32 authority relationship,
- region schema,
- fog projection,
- survey model,
- skill/equipment,
- map artifacts,
- trade,
- freshness/staleness,
- save/migration,
- adding regions.

---

# 177. 163C Definition of Done

- [ ] ExpeditionSystem integration,
- [ ] Plan 32 knowledge integration,
- [ ] LocationEvolution staleness,
- [ ] SkillProgression integration,
- [ ] equipment/condition integration,
- [ ] MarketSystem integration,
- [ ] faction standing integration,
- [ ] optional colony integration,
- [ ] quest/journal/archive integration,
- [ ] save/load matrix,
- [ ] discovery anti-reroll,
- [ ] UI-refresh exploit blocked,
- [ ] XP farm blocked,
- [ ] trade loop blocked,
- [ ] no-discovery edge case,
- [ ] all-discovered edge case,
- [ ] no-cartographer/no-tools valid,
- [ ] stale-world edge case,
- [ ] topology integrity,
- [ ] knowledge-ladder integrity,
- [ ] UI/runtime planning parity,
- [ ] quality caps,
- [ ] secret idempotency,
- [ ] selftest,
- [ ] deliberate failure proof,
- [ ] same-seed replay,
- [ ] 200-day soak,
- [ ] strategy profiles,
- [ ] fog fairness,
- [ ] planning-value test,
- [ ] trade/skill/tool/staleness value tests,
- [ ] accessibility,
- [ ] headless,
- [ ] retention,
- [ ] performance/save budgets,
- [ ] playtest,
- [ ] docs.

---

# 178. Integrated Cartography Pipeline

```text
canonical topology
      │
      ▼
Plan 32 knowledge state
      │
      ├─ Unknown
      ├─ Rumoured
      ├─ Located
      ├─ Surveyed
      ├─ Visited
      └─ Mapped
      │
      ▼
CartographySystem
      │
      ├─ survey activity
      ├─ quality
      ├─ provenance
      ├─ coverage
      ├─ freshness
      └─ map artifact
      │
      ▼
Expedition / Market / Faction / UI
```

---

# 179. World Topology Contract

Cartography never owns:

- location existence,
- route existence,
- graph connectivity.

Those remain canonical world topology facts.

---

# 180. Knowledge Ladder Contract

Cartography updates knowledge through Plan 32 only.

No independent discovered flag.

---

# 181. Fog Contract

Fog is derived from knowledge.

Removing fog never grants knowledge.

---

# 182. Region Contract

Region definitions group canonical places/routes for surveying and presentation.

They do not become a second travel graph.

---

# 183. Coverage Contract

Coverage is derived.

No manual percentage mutations.

---

# 184. Skill Contract

Cartography XP/level belongs to SkillProgressionSystem.

No duplicate proficiency field.

---

# 185. Equipment Contract

Tools are normal inventory/equipment items.

No cartography-owned tool list as authority.

---

# 186. Condition Contract

Tool wear uses EquipmentConditionSystem.

---

# 187. Map Quality Contract

Quality is bounded 0–100 and cannot exceed underlying source-knowledge quality.

---

# 188. Map Artifact Contract

A map artifact contains:

```text
scope
quality
freshness
provenance
knowledge payload
```

not copied world definitions.

---

# 189. Trade Contract

Map trading transfers information and a commodity.

It does not change physical visitation.

---

# 190. Buyer Novelty Contract

Value declines as buyer already knows equivalent/better information.

---

# 191. Staleness Contract

World change lowers current planning reliability.

Historical discovery remains.

---

# 192. Planning Contract

Higher-quality maps improve:

- estimate precision,
- hazard awareness,
- route confidence.

They do not automatically nullify risk.

---

# 193. Secret Discovery Contract

Secrets require authored eligibility + survey threshold + deterministic discovery state.

---

# 194. Old-Save Contract

Existing knowledge is preserved.

Cartography starts around that truth rather than resetting the world to fog.

---

# 195. Save Contract

Persist:

- survey/map artifacts,
- activity state,
- idempotency,
- freshness references.

Do not persist:

- topology,
- skill XP copy,
- equipment condition copy,
- region completeness copy.

---

# 196. Determinism Contract

Same:

```text
seed
world state
travel
survey choices
```

→ same mapping history.

---

# 197. UI Contract

UI exposes only player-known geographic facts.

Accessibility metadata obeys same knowledge boundary.

---

# 198. Content Acceptance Contract

Map regions/tools/projection rules progress through:

```text
AUTHORED
→ LOADS
→ ELIGIBLE
→ SURVEYED
→ KNOWLEDGE_PRODUCED
→ PLANNING_EFFECT_PRODUCED
→ PLAYER_VISIBLE
```

---

# 199. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| cartography duplicates Plan 32 location knowledge | High | Critical | Plan 32 ladder is sole authority |
| fog leaks hidden labels through UI/accessibility | Medium | High | semantic-tree leakage tests |
| buying maps marks locations visited | Medium | High | strict knowledge/visitation split |
| cartography skill duplicates skill system | Medium | High | SkillProgression owns XP |
| tool bonuses stack beyond 100% | Medium | Medium | caps/diminishing rules |
| stale map becomes useless instantly | Medium | Medium | selective freshness degradation |
| map trading creates infinite money/standing | Medium | High | buyer novelty/history |
| save/load rerolls secret discovery | Medium | High | stable discovery IDs |
| 20 regions do not fit topology | Medium | High | audit before authoring |
| map quality only cosmetic | Medium | High | planning-value integration |
| world graph rescanned every frame | Medium | Medium | event-driven caches |
| trade-only region becomes permanent vendor gate | Medium | Medium | purchase opens exploration, not permanent lock |

---

# 200. Commit Strategy

## 163A — Foundation

### C2[35].1 — baseline + Plan 32/cartography ADR

### C2[35].2 — region/survey/map-artifact DTOs

### C2[35].3 — map_regions.json + 20-region topology audit

### C2[35].4 — knowledge-ladder integration

### C2[35].5 — coverage/completeness projection

### C2[35].6 — cartography skill/specialization integration

### C2[35].7 — mapping equipment/condition integration

### C2[35].8 — quality/freshness/staleness model

### C2[35].9 — save/old-save/idempotency

### C2[35].10 — ports/events/diagnostics

### Gate: 163A complete

---

## 163B — Discovery / Trade / UI

### C2[35].11 — fog-of-war projection

### C2[35].12 — route/passive discovery

### C2[35].13 — cartography expedition

### C2[35].14 — skill XP/apprenticeship

### C2[35].15 — tools/crafting/trade/degradation

### C2[35].16 — map artifact creation

### C2[35].17 — Market/faction map trading

### C2[35].18 — planning-quality effects

### C2[35].19 — secrets/staleness/map updates

### C2[35].20 — events/quests/journal

### C2[35].21 — map UI/accessibility/tutorial/localization

### C2[35].22 — content-utilization reports

### Gate: 163B complete

---

## 163C — Closure

### C2[35].23 — Expedition/LocationEvolution integration

### C2[35].24 — skill/equipment/market/faction integration

### C2[35].25 — save-load/exploit matrix

### C2[35].26 — no/all discovery and no-tool edge cases

### C2[35].27 — topology/knowledge-ladder integrity

### C2[35].28 — selftest + deliberate failure proof

### C2[35].29 — 200-day exploration soak

### C2[35].30 — strategy/value/fairness tests

### C2[35].31 — performance/save budgets

### C2[35].32 — playtest/docs/release closure

### Gate: 163C complete

---

# 201. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --cartography-selftest
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical equivalents of:

```text
port-contract validation
map-region content-utilization report
Plan 32 knowledge-ladder validation
old-save fixture load
same-seed discovery replay
200-day exploration soak
map-trade exploit test
world-map hidden-label/accessibility leakage test
map-planning runtime/UI parity test
```

---

# 202. Flagship Definition of Done

## 163A — Foundation

- [ ] CartographySystem,
- [ ] Plan 32 knowledge ladder remains canonical,
- [ ] no duplicate discovered boolean,
- [ ] region/survey/map-artifact models,
- [ ] 20 real topology-backed regions,
- [ ] region coverage/completeness derived,
- [ ] cartography skill integrated,
- [ ] specializations,
- [ ] real mapping equipment,
- [ ] equipment condition,
- [ ] bounded quality tiers,
- [ ] deterministic discovery,
- [ ] freshness/staleness,
- [ ] save/old-save,
- [ ] ports,
- [ ] events,
- [ ] diagnostics.

## 163B — Discovery / Trade / UI

- [ ] new-game fog,
- [ ] shelter/bootstrap reveal,
- [ ] route reveal,
- [ ] normal expedition observation,
- [ ] cartography expeditions,
- [ ] skill XP,
- [ ] apprenticeship,
- [ ] mapping tools,
- [ ] map artifacts,
- [ ] buying maps,
- [ ] selling maps,
- [ ] faction standing,
- [ ] buyer novelty protection,
- [ ] quality/freshness effects,
- [ ] planning integration,
- [ ] hidden locations,
- [ ] map updates,
- [ ] optional colony reveal,
- [ ] journal,
- [ ] 7 events,
- [ ] 7 quests,
- [ ] map UI,
- [ ] filters/tooltips,
- [ ] accessibility,
- [ ] tutorial/localization,
- [ ] utilization.

## 163C — Integration / Validation

- [ ] ExpeditionSystem,
- [ ] LocationEvolutionSystem,
- [ ] SkillProgressionSystem,
- [ ] equipment/condition,
- [ ] MarketSystem,
- [ ] faction standing,
- [ ] optional ColonySystem,
- [ ] quest/archive integration,
- [ ] save/load matrix,
- [ ] discovery anti-reroll,
- [ ] UI refresh no-op,
- [ ] XP farm protection,
- [ ] trade-loop protection,
- [ ] purchase payload exactness,
- [ ] no-discovery case,
- [ ] all-discovered case,
- [ ] no-cartographer case,
- [ ] no-equipment case,
- [ ] stale-world case,
- [ ] topology integrity,
- [ ] knowledge-ladder integrity,
- [ ] planning parity,
- [ ] quality caps,
- [ ] secret idempotency,
- [ ] selftest,
- [ ] failure proof,
- [ ] same-seed replay,
- [ ] 200-day soak,
- [ ] strategy profiles,
- [ ] fog fairness,
- [ ] planning/trade/skill/tool value tests,
- [ ] performance/save budgets,
- [ ] accessibility,
- [ ] headless,
- [ ] retention,
- [ ] playtest,
- [ ] docs.

## Global

- [ ] no duplicate topology,
- [ ] no duplicate knowledge authority,
- [ ] no duplicate skill authority,
- [ ] no duplicate equipment durability,
- [ ] no UI-only fog truth,
- [ ] no purchase-as-visit bug,
- [ ] no map-trade infinite loop,
- [ ] no hidden-label leakage,
- [ ] no world-state reroll exploit,
- [ ] full verification green.

---

# 203. Closure Report Template

```markdown
## C2[35] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Canonical knowledge ladder:
- Locations:
- Routes:
- Existing known locations:
- Existing map UI:
- Skill system:
- Equipment condition:
- Market:
- Faction trade hooks:

### 163A — Foundation
- Map regions:
- Region topology validity:
- Survey records:
- Map artifacts:
- Knowledge integration:
- Coverage derivation:
- Cartography skill:
- Specializations:
- Mapping tools:
- Quality model:
- Freshness:
- Save schema:
- Old-save migration:
- Missing ports:
- Result:

### 163B — Discovery / Trade
- Fog bootstrap:
- Route reveal:
- Normal expedition survey:
- Cartography expeditions:
- XP awards:
- Apprenticeship:
- Maps created:
- Maps sold:
- Maps bought:
- Faction standing events:
- Hidden discoveries:
- Stale maps:
- Updated maps:
- Quests:
- UI:
- Unused regions/tools:
- Result:

### 163C — Validation
- Expedition integration:
- LocationEvolution integration:
- Skill integration:
- Equipment integration:
- Market integration:
- Faction integration:
- Colony integration:
- Old-save preservation:
- Save-load rerolls:
- XP exploit:
- Trade exploit:
- Hidden-label leakage:
- No-discovery case:
- All-discovered case:
- Stale-world case:
- Planning parity:
- 200-day soak:
- Strategy profiles:
- Playtest:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Cartography selftest:
- Port contract:
- Knowledge-ladder gate:
- Content utilization:
- Old-save fixtures:
- Same-seed replay:
- Exploration soak:
- UI/accessibility leakage:
- Verify fast:

### Final Metrics
- MAP_REGIONS:
- REGIONS_KNOWN:
- REGIONS_SURVEYED:
- REGIONS_MAPPED:
- WORLD_MAP_COMPLETENESS:
- LOCATIONS_DISCOVERED:
- ROUTES_MAPPED:
- CARTOGRAPHY_XP_AWARDED:
- MAP_ARTIFACTS_CREATED:
- MAPS_TRADED:
- STALE_MAPS:
- SECRET_DISCOVERIES:
- KNOWLEDGE_STATE_VIOLATIONS:
- TRADE_LOOP_VIOLATIONS:
- UI_HIDDEN_INFO_LEAKS:
- REQUIRED_PORTS_MISSING:

### Remaining Debt
- Region authoring:
- Mapping tools:
- Trade content:
- Hidden locations:
- Route planning:
- Colony reveal:
- UI:
```

---

# 204. Final Execution Directive

Execute Plan 163 as a **cartographic-quality and survey layer over Plan 32’s canonical world topology and knowledge ladder**.

The critical sequence is:

```text
preserve the canonical place/route graph
→ preserve Plan 32 knowledge states
→ author 20 real regions over that graph
→ derive fog from knowledge
→ let real expeditions reveal route/place knowledge
→ let cartography expeditions improve survey quality
→ use canonical skill/equipment/condition systems
→ create provenance-aware map artifacts
→ trade maps as information without marking places visited
→ stale maps when world state evolves
→ feed map quality into real expedition planning
→ prove determinism, anti-farming, and no hidden-information leakage
```

Do not create a second world map graph.

Do not replace Plan 32 with a boolean `discovered`.

Do not create a separate cartography XP ledger.

Do not copy equipment durability.

Do not let buying a map count as physically visiting a location.

Do not let accessibility labels reveal hidden POIs.

The strongest authority rule is:

> **The world exists in the canonical topology and the player’s geographic knowledge exists in Plan 32; cartography only determines how much detail, confidence, freshness, and tradeable survey information the player has about that world.**

The strongest exploration rule is:

> **Exploration and cartography are related but not identical: travel proves physical visitation, while mapping improves knowledge quality and planning confidence.**

The strongest trade rule is:

> **A map transfers information, not experience—buying knowledge may reveal where something is, but it must never fabricate the fact that the player has actually been there.**

The flagship acceptance scenario is:

> **Start a seeded campaign with only the shelter region and one nearby rumor visible. Travel a real expedition route through an unknown region with a novice cartographer carrying a worn compass, reveal only the traversed geography, then launch a dedicated survey with a better-equipped specialist and raise the region from rough knowledge to a detailed map. Sell a copy to a faction that lacks the information, buy a different high-quality regional map from another faction, and verify the purchased region becomes known without receiving `Visited`. Trigger `LocationEvolutionSystem` to close a mapped route and change a settlement; the old map must become stale while historical knowledge remains. Save/load before a hidden-location survey and prove the same discovery result, map IDs, quality, trade state, and knowledge digest reproduce exactly, with no hidden POIs leaking through the UI or accessibility tree.**
