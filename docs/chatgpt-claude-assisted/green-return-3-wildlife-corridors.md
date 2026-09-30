# GR-3 — Wildlife Corridors and Return Pressure

STATUS: DRAFT — proposal for review; depends on GR-1 and GR-2 premise audits; no approval or claim.

## 1. Objective

Report whether existing wildlife packs can traverse the existing sector graph in relation to known land-condition evidence. Keep population, migration, survival, food-web pressure, and species biology entirely in the current wildlife owners. This is a corridor **read model**. It does not make wildlife return to a site or change population numbers.

The player-facing outcome, if the audited map route supports it, is a legible corridor status: known migration link, known blockage, unknown suitability, or recent movement evidenced by an existing pack. It must not imply a pack occupies a specific location when the source only knows its sector.

## 2. Current Reality

`WildlifeMigrationSystem` in `Assets/Ashfall.Core/WildlifeMigrationSystem.cs` owns persisted pack records: pack/species IDs, current sector, population, seeded population, aggression/starvation, rabies, and last threat day. The live partial owns a sector-neighbor graph and water-sector set, accepts adjacency through `SetSectorAdjacency` / `MergeSectorAdjacency`, and migrates packs through `TickDay(int, ISeededRng?)`. Migration reads season profile, hunger, neighbors, water constraints, war-blocked sectors, and a seeded per-day RNG. Population recovery/loss and movement already have rules. `GetSectorPackPopulation`, `GetGlobalPopulationRatio`, `TryGetNeighbors`, `IsWaterSector`, and block accessors are existing read seams.

`EvolvingWorldSeeder.Seed` loads the canonical `world_evolution_seeds.json` sector graph, waterways, packs, and location seeds. The seeder is idempotent for world records. `EvolvingWorldDayOwner` calls wildlife migration with the campaign world-evolution fork and then calls the wildlife ecosystem tick. The order is materially important: the ecology owner mutates populations only through the migration authority.

`WildlifeEcosystemSystem` owns the fauna catalog, ecological pressure, extinction state, wildlife observations, and derived queries against `WildlifeMigrationSystem`. `SectorSpeciesPopulation` reads the migration owner's packs; `RecordObservation` writes fauna observations. The system already models interactions such as predation, radiation tolerance, seasonal movement, extinction/recolonization, and apex pressure. Those mechanics are outside GR-3.

`LocationEvolutionSystem` tracks per-location contamination, threat/ruin and other location mutations; wildlife uses **sector** IDs while location records use **location** IDs. P0 must inspect the canonical mapping between those vocabularies. No current `WildlifeMigrationSystem` API accepts site contamination, recovery band, habitat suitability, or location IDs. Existing adjacency and blocked-sector inputs do not constitute an environmental suitability provider.

## 3. Required Delta

The missing capability is a truthful view that joins two existing vocabularies without creating a second population or habitat authority. The smallest delta is:

1. verify or identify the existing sector-to-location/region mapping;
2. report current adjacency, water constraint, host-projected blockage, and known live pack movements using existing facts;
3. optionally relate a corridor to GR-1 condition only if a signed mapping and an existing consumer seam exist;
4. label unsupported suitability as unknown.

There is no approved delta to migration behavior. Any request that asks for wildlife populations to rebound in newly recovered land belongs in a separately scoped Second Nature / wildlife-owner plan with foreman approval, because the existing plan boundary assigns wildlife populations and food webs there.

## 4. Evidence

Snapshot checked 2026-09-29; all exact callers and current ownership must be confirmed again before implementation.

| Evidence | Source | Consequence |
|---|---|---|
| Pack records and capture/restore belong to migration owner | `WildlifeMigrationSystem.cs` | Never create GR pack state or duplicate pack count. |
| Sector graph is authored and seeded | `EvolvingWorldSeeder` in `Assets/Ashfall.Core/EvolvingWorldCatalog.cs`; `world_evolution_seeds.json` | Reuse sector graph; no second corridor graph. |
| Blockages are projected from live faction conditions, not saved | `SetSectorBlocked`, `ClearSectorBlockages` in migration live partial; host callers to audit | Treat as a current input/readout; do not persist another blockage ledger. |
| Migration is seeded and has season/water filters | `WildlifeMigrationSystem.Live.cs` | No extra random movement; preserve stream/order. |
| Wildlife counts/population changes are centralized | `WildlifeEcosystemSystem` read queries plus mutation calls on migration | Only existing owners can change counts. |
| Existing fauna observations are species × sector facts | `WildlifeObservation` and `RecordObservation` | Never reinterpret a sighting as proof of land recovery. |
| Location state uses different IDs and has environmental facts | `LocationEvolutionSystem` / `world_evolution_seeds.json` | Require an audited mapping; no guessed string prefix/crosswalk. |

Duplicate check must include Second Nature, all wildlife/ecology plans, seed catalog sector topology, map regions, route blockages, `WildlifeSeasonalCalendar`, and prior world-expansion plans. The file name “corridor” alone is not proof a new graph is needed.

## 5. Existing Extension Seams

For a read-only corridor display, use the migration owner's `TryGetNeighbors`, `IsWaterSector`, `IsSectorBlocked`, and current pack records. Any suitability estimate has no known host provider at the time of this audit. A new optional provider could be proposed only if the sector/location mapping is canonical and the wildlife owner agrees that it is a *read-only report* with no effect on migration.

For a gameplay effect, an extension would belong at the migration/ecosystem owning boundary and must specify deterministic input, behavior when no provider is bound, save/restore, test coverage, and implications for population conservation. This plan expressly does not approve that extension. If the readout cannot be implemented from extant APIs without exposing mutable private state or adding an interface used once, stop and request an integrator decision.

## 6. Proposed Architecture

### 6.1 Corridor projection

A pure query may combine an authored sector edge with current blockage and species water constraints into a report. It must use the existing graph and never create or mutate adjacency. If location condition is cross-referenced, the projection can describe the land record as context only; it cannot claim “habitat suitable” absent a domain-approved biological rule.

### 6.2 Movement is still canonical

Actual movement continues through `WildlifeMigrationSystem.TickDay` and existing host orchestration. No map entry, cleanup action, condition label, or survey can register/spawn a pack, change `currentSectorId`, change hunger, or reset a movement timer.

### 6.3 Observation remains wildlife-owned

Current sightings are already species/sector/day observations. GR-3 may link to a current observation if a reader exists, but must not record sightings because the user opened a map. The known observation confidence field and retention policy remain in wildlife owner and are not generalized as a new Green Return evidence grade.

## 7. Ownership Matrix

| Concern | Owner | GR-3 role |
|---|---|---|
| Pack identity, location by sector, population, hunger, movement | `WildlifeMigrationSystem` | Read-only; all mutations remain there |
| Food-web interaction, species ecology, extinction/recolonization | `WildlifeEcosystemSystem` | Excluded |
| Sector adjacency and water routes | `EvolvingWorldSeeder` + migration owner | Read existing graph |
| Faction/war route blocks | Existing host projection into migration owner | Read only; do not save locally |
| Site contamination/ruin and location ID | `LocationEvolutionSystem` | Optional environmental context only |
| Sector/location/region mapping | Existing authored map/world data, if present | Verify; never infer |
| Rendered corridor status | Existing map/location presenter | Read-only binding |
| Wildlife observation quality | `WildlifeEcosystemSystem` | Read only |

## 8. Data Flow

Canonical seed catalog → existing `EvolvingWorldSeeder` → migration graph/water set → current block projection → pure corridor report → map/detail view.

Optional land context: canonical sector↔location mapping → `LocationEvolutionSystem.TryGetRecord` → GR-1 projection → corridor report notes “land condition record present/unknown.” There is no arrow from corridor report back to migration/ecosystem state. Pack movement/sightings travel through current wildlife migration event and wildlife observation owner; GR-3 may consume their already-committed report but must not replay it.

## 9. State Model

No new persistent state is proposed. A corridor result is derived from graph topology and the current blockage/suitability context. Cached projections must be invalidated through existing world/map refresh lifecycle and may not become save authority.

Proposed output invariants:

- source and target are canonical sector IDs;
- each displayed edge exists in authored adjacency or is explicitly labeled unknown;
- blocked and water-constrained edges are distinct causes;
- a location condition is not converted to a species suitability score;
- no pack population or exact location is inferred from a sector-level record;
- ties and output order use ordinal ID ordering;
- no record is created by inspection.

If P0 finds that current sector-block state cannot be queried safely outside the migration owner, add a read-only accessor only if the owner approves it. Do not expose mutable sets or neighbor lists to Godot.

## 10. API / Contracts

No API additions are authorized. Existing migration accessors return/accept some mutable collection shapes, so P0 must check whether callers can mutate owner state through returned lists. If safe immutable access is lacking, prefer an owner-provided projection/snapshot over passing `_sectorNeighbors` or `_blockedSectors` to UI.

A candidate report contract, if justified, can contain `fromSectorId`, `toSectorId`, authored-edge status, blockage reason, water restriction, current observed movement day if available, and `suitability = unknown`. Do not use `Suitable` / `Unsuitable` until a wildlife owner supplies explicit definition and valid factors. No generic “return pressure” scalar is proposed without canonical semantics.

## 11. Data Changes

No authored content changes are proposed at the outset. Verify `world_evolution_seeds.json`, `map_regions.json`, sector IDs in `wildlife_ecosystem.json`, and all route/war block catalogs. If a canonical crosswalk does not exist, report that as a schema decision; do not add a GR-owned duplicate map of sectors to locations.

An optional environmental suitability catalog would create a second wildlife/land compatibility authority and is therefore out of scope unless Second Nature/wildlife owner revises its plan and the foreman authorizes the joint decision. Any change to an existing data file requires schema/version validation, reference checks, and a real loader consumer.

## 12. Save / Load

The current migration save already captures pack state; migration adjacency, water sectors, and blockage projection are initialized/recomputed via existing catalog/host setup. GR-3 adds no save section and serializes no corridor result. The projection runs after world setup and saved owners restore.

P0 must verify post-restore `EvolvingWorldSeeder` behavior and the daily blockade recomputation path, including whether empty or missing catalogs preserve current movement. A new map-only readout must not modify a save. If later approved gameplay changes influence movement, the owner must define whether inputs are re-derived after restore and how replay tests detect divergence; that is a new integration plan.

## 13. Determinism

The map report has no randomness. Sort output by canonical IDs and evaluate block/water facts in stable order. Existing wildlife movement already uses per-day campaign RNG and candidate neighbor filtering; GR-3 cannot consume the same stream, reorder candidates, sort the migration graph differently, or add a random “sighting chance.”

With identical graph, block state, and wildlife records, report output must be identical across replay and restore. Missing optional provider means “unknown/no extra report context,” not hidden default suitability that changes gameplay. The same seed with GR-3 display enabled must produce byte-identical wildlife owner state compared with display disabled.

## 14. System / Event Wiring

No new tick or event is required for a static graph report. The view refreshes after existing map selection and the existing phase-4 world update. Existing `OnPackMigrated` / wildlife observation events should be consumed only if a currently owning adapter supports it and the event is committed; otherwise query the resulting owner snapshot on refresh.

No subscription may feed back into `TickDay`. Do not hook location mutation into migration as an implicit cross-owner dependency. Any proposal to gate migration using land recovery must be referred to Second Nature and reviewed as a gameplay change, with no implementation under this draft.

## 15. Godot Integration

P0 audits `WastelandMapView`, map marker and location details, `MapPanel` if applicable, selection route, and disposal. The UI should distinguish “known route edge,” “blocked today,” “water route,” “wildlife pack reported in sector,” and “land suitability unknown.” It should not paint every site green because its local contamination decreased. Present species sighting at sector granularity only.

The view must preserve map filtering/navigation, keyboard/controller focus, contrast, marker status semantics, and 1920×1080 layout. Color is supplementary to text/icon. Refresh is event-driven or route-bound; no per-frame traversal of all packs and adjacency. If a bounded snapshot is required, it remains transient and is rebuilt from owner state.

## 16. Narrative / Content Integration

Existing radio intercepts and wildlife knowledge hooks may already report migration. Do not create competing lines that say the same pack moved. An authored line is valid only when a current pack or observation supports the named sector/species and the line's certainty matches the observation.

Do not label a corridor “restored” based on a land readout. Do not generate ecological history or bestiary knowledge from the player's map view. Second Nature owns changed wildlife, crops, and food webs; GR-3's prose stays at the level of route access and reported presence.

## 17. Failure Modes

| Case | Behavior |
|---|---|
| Sector not in canonical graph | Unknown edge; do not create it. |
| Sector has no neighbors | Report no known outgoing link, not “blocked.” |
| Missing sector/location crosswalk | Omit land overlay; no guessed ID mapping. |
| Blockage projection unavailable | Mark status unknown; preserve current migration behavior. |
| Pack is at sector-level only | Do not pin it to a location marker. |
| No pack of a species currently reported | Do not conclude local extinction unless ecosystem owner says so. |
| Water-bound species and dry edge | Explain existing water restriction; do not edit graph. |
| Duplicate or asymmetric adjacency | Validate/diagnose; do not silently repair during display. |
| Missing catalog | Existing owner’s neutral behavior applies; no GR fallback graph. |
| Save restore before graph bind | Defer readout until setup completes. |
| Map opened during day transition | Show last committed snapshot or defer refresh. |
| UI panel closes | Release event subscriptions and transient snapshots. |

## 18. Test Strategy

No testing is authorized by this draft. If approved, inspect current migration, wildlife, map, and save targets for equivalent coverage and run focused targets with `bin/run-scoped-tests` only.

- pure corridor projection for existing edge, missing edge, blocked edge, water constraint, unknown context, stable ordering;
- ensure calling the projection does not change `CaptureState()` for either location or wildlife;
- no-provider parity test compares wildlife state before/after a seeded multi-day simulation;
- optional mapping validator: every mapped sector/location resolves once; no map output invented for unknown IDs;
- observation display uses sector/day/confidence as persisted and does not write a new observation;
- host test verifies the projection sees committed blockage state only after setup/tick boundary;
- UI route test only if scene/presenter changed, covering disposal/focus and status distinction.

No broad ecology or balance simulation should be run unless a new gameplay behavior is separately authorized with a bounded hypothesis.

## 19. Dependency-Ordered Phases

### Phase 0 — graph and owner census

Read migration APIs and all host wiring for adjacency, water, blocked sectors, population writes, observations, and map region mapping. Recheck Second Nature, Living Region, GR-1/2, and wildlife plan corpus for overlap.

**Gate:** identify exact canonical IDs and prove the read-only consumer path. If there is no stable sector/location mapping, keep the display sector-only.

### Phase 1 — projection contract

Agree on what “corridor” means in the existing graph: authored adjacency plus current existing constraints. Keep habitat suitability unknown unless an existing wildlife rule already exposes it. Specify sector-level granularity and evidence day.

**Gate:** contract has no mutation, persistence, RNG, or biological rule.

### Phase 2 — Core read projection (conditional)

If two or more current consumers need the same report, add one minimal immutable owner projection. Otherwise bind existing getters in the host adapter; do not create an abstraction for one call.

**Gate:** Core tests prove projection purity, missing-provider parity, and stable ordering.

### Phase 3 — optional mapping validation

Only if a canonical map crosswalk is found or independently approved, validate references using the existing catalog path. Do not add parallel topology or suitability data.

**Gate:** no unknown ID is silently converted to a sector/location; missing optional mapping leaves existing map unchanged.

### Phase 4 — UI binding

Add readout to existing map/location surface after route audit. Restrict detail to the real granularity and source facts.

**Gate:** map navigation, controller/keyboard focus, accessibility, disposal, and unchanged danger semantics are verified.

### Phase 5 — scoped verification and handoff

Run only changed module tests through the scoped runner; if runtime UI changed, include the narrow Godot headless check under project limits.

**Gate:** handoff explains why no population/migration effect was introduced and lists all cross-owner requests deferred.

## 20. File Impact Map

Proposed only; exact paths must be rechecked and claimed.

| File / area | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/WildlifeMigrationSystem.cs` / `.Live.cs` | READ ONLY; possible minimal read projection | Canonical graph, pack, movement owner | High; migration behavior/determinism |
| `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` | READ ONLY | Existing ecology and observation authority | High if population rules change |
| `Assets/Ashfall.Core/EvolvingWorldCatalog.cs` | READ ONLY | Idempotent seed/graph source | Medium; world setup |
| `Assets/StreamingAssets/Data/world_evolution_seeds.json` | READ ONLY; conditional mapping change only by separate approval | Canonical sector graph and location seeds | Medium; content references |
| `Assets/StreamingAssets/Data/map_regions.json`, `wildlife_ecosystem.json` | READ ONLY | Audit ID vocabularies | Medium |
| `src/Main.CampaignOwners.cs` | READ ONLY | Day order and world RNG | Very high shared seam |
| Map/location UI route | READ ONLY; conditional MODIFY | Render read-only report | Medium; navigation/accessibility |
| Wildlife save owner / registry | NO CHANGE | No new state | High if modified |
| Second Nature plan | READ ONLY | Boundary and dependency | Governance risk if edited |

## 21. Risks

- **Biology duplication:** a “corridor suitability” score could quietly become new habitat simulation. Keep suitability unknown or reuse an existing wildlife-owned output.
- **Vocabularies do not align:** a wildlife pack is in a sector; a location mutation is per named site. Require explicit mapping.
- **Movement parity regression:** reading or sorting the graph in a mutation path could alter seeded target selection. Keep report code out of tick.
- **False abundance/extinction:** a missing pack observation is not a zero population. Use the ecosystem owner's extinction state/query.
- **Duplicate sightings:** existing radio intercepts and bestiary knowledge already react to movement. Use one source and one report path.
- **Unpersisted block state:** faction block projections may be recomputed daily. Display freshness must match when they were built.
- **Scope overlap:** Second Nature is the authority for wildlife changes. Any population or ecology effect exits this plan.

## 22. Out of Scope

No wildlife population/spawn, breeding, extinction, recolonization, predation, mutation, crop, pollination, food-web, hunting/trapping, migration chance, route graph, habitat suitability, species behavior, new observation history, map fog, or settlement pressure. No modification of `WildlifeMigrationSystem.TickDay`, `WildlifeEcosystemSystem.TickDay`, seeding, campaign RNG, or world-day phase order.

## 23. Rollback Strategy

The report is derived and has no saved state; rollback removes only its query/presentation. Existing movement and wildlife saves remain unchanged. If a minimal owner accessor is added, revert it with the report consumer and keep current graph/migration behavior byte-identical. Do not disable or rewrite existing wildlife migration to roll back the map display.

Any proposed gameplay integration requires its own approved plan, migration strategy, deterministic baseline, and rollback test. Do not roll it into this one during implementation.

## 24. Definition of Done

- P0 verifies sector graph, block projection, water rules, population mutation owner, observations, and mapping.
- Corridor output represents only known topology and currently exposed restrictions.
- No new population, habitat, save, RNG, clock, or wildlife observation state is added.
- Missing mappings/provider yield unknown and preserve migration results.
- Display remains sector-level and separates blockage, water, absence of data, and biological unknown.
- Any focused changed-file tests pass through the scoped runner; no full suite without exact authorization phrase.

## 25. Implementation Handoff

### MUST PRESERVE

`WildlifeMigrationSystem` as the only pack/movement authority; `WildlifeEcosystemSystem` as the ecological pressure/observation owner; authored sector adjacency and water constraints; phase-4 deterministic order and stream forks.

### MUST ADD

At most a pure corridor read projection using existing facts and a read-only binding to an audited map surface. Keep biological suitability unknown unless an existing wildlife owner supplies it.

### MUST NOT DO

Change pack counts or movement, add wildlife spawns, add ecological or crop rules, record sightings from map interaction, infer local extinction, create a corridor graph/crosswalk without an approved owner, or connect location condition to wildlife gameplay.

### VERIFY WITH

Focused projection/provider tests and a no-provider deterministic parity target via `bin/run-scoped-tests`; narrow Godot headless check only if the map runtime path changes.

### FIRST SAFE IMPLEMENTATION STEP

Create a read-only matrix of sector graph, block, water, observation, population and location-ID mapping APIs, then show a corridor mock report that contains no invented suitability or location precision.

## Appendix A — Canonical graph examples

The authored graph makes a useful corridor report possible without inventing new topology. `world_evolution_seeds.json` describes 24 sectors, 24 starting packs across 12 species, 30 landmark baselines, and 40 location seeds. The catalog is also the source for `shelter_sector_id` and scarcity goods. Those counts are evidence for the existing subsystem's scale, not acceptance criteria for new content.

| Authored route example | Catalog facts | Safe report | Unsafe inference |
|---|---|---|---|
| Hinterlands → Hills | `sector_4_hinterlands` lists `sector_4_hills` as neighbor; Hills lists Hinterlands | Known adjacent graph edge | Land condition is healthy or a pack will choose it |
| Hinterlands → Floodplain → River | Hinterlands links Floodplain; Floodplain links River; River is marked `water: true` | Two-step known route; water classification applies to River | Every water-bound species can traverse every edge or has actually returned |
| Canyon → Chemical Corridor → Industrial Works | These locations/sectors are linked in authored graph | A route exists in topology subject to current blockers and species filters | The corridor is environmentally suitable because it is connected |
| Bluffs → Docklands | Both sectors occur in graph; exact adjacency must be checked from both rows before display | Show only verified edge direction/edge policy | Assume undirected graph if data is asymmetric |
| Shelter sector | Catalog names `sector_4_hinterlands` as shelter sector | Use this existing value where a current view needs shelter origin | Create an alternate shelter-region anchor |

Graph examples should be validated against the current JSON at implementation time. Do not paste copied topology into a second Green Return catalog. If the map route needs a richer geospatial line than adjacent-sector list, that requirement belongs to the existing map/route authority.

## Appendix B — Worked corridor situations

### Situation 1: authored edge with no pack

The player opens the route display for `sector_4_hinterlands` and sees a known edge to `sector_4_hills`. No current pack record is in either sector. The report says the edge exists and no wildlife movement was reported. It must not say “empty,” “extinct,” or “closed.” Local extinction is a separate persisted wildlife ecosystem fact, and a lack of an observation is not evidence that no animals are present.

### Situation 2: pack movement along a graph edge

The seed catalog places `pack_hill_wolves` (`species_wolf`, population 4) in `sector_4_hills` at initial seed. If the current migration owner later moves it to an authored neighbor, the canonical `OnPackMigrated` event and resulting `currentSectorId` may be reported at sector precision. The report uses the current post-tick record. It cannot claim that land condition caused the move unless the migration owner already records that causal input; current known inputs include hunger, seasonal behavior, neighbor graph, waterways, and blocked sectors.

### Situation 3: blocked target while pack is in blocked sector

The live migration contract states that a blocked sector is not entered, while a pack already inside may flee to an unblocked neighbor. A corridor readout should report the same asymmetry if it exposes that rule: outbound escape may be possible even if incoming travel is blocked. A symmetrical red edge on both sides would misrepresent the owner. Fully enclosed packs stay in place under the current tick rule; GR-3 must not offer an override button.

### Situation 4: water-bound runner

The seed marks sectors such as `sector_4_river` and `sector_8_estuary` as water sectors; wildlife code filters neighbors by species archetype. A species tag such as `water` exists in `wildlife_ecosystem.json`, while actual migration filtering is based on `WildlifeSeasonalCalendar.ArchetypeOf` and the water-sector collection. A UI must not infer engine eligibility from an arbitrary tag. It can either consume an owner projection that already resolved the restriction or display the authored water facts without predicting movement.

### Situation 5: habitat interpretation request

Suppose the player asks why a species has not returned after a nearby site's contamination falls. Current graph adjacency and population authority do not expose a land-site suitability rule. The honest answer is that condition and movement are separately modeled. The report may show current contamination, current pack sector, and known corridor but cannot promise recolonization. If this limitation is considered unacceptable, the owner boundary changes and requires a Second Nature / wildlife-owner plan and explicit design approval.

### Situation 6: duplicate radio and map messages

The campaign host already observes pack sector changes and creates radio intercepts, limited to a bounded number per day, and may unlock field-guide observation knowledge. The map view must not add another durable radio event or journal entry for the same transition. It can show the current pack result in a selected-site panel without creating a second announcement or unlocking a second knowledge fact.

## Appendix C — Corridor projection truth table

The exact result names depend on an approved contract; the table specifies semantic expectations only.

| Graph edge | Incoming target blocked | Outgoing from current blocked sector | Water-compatible | Readout consequence | Migration consequence |
|---|---:|---:|---:|---|---|
| absent | no | no | unknown | No known edge | Existing movement finds no neighbor |
| present | no | no | yes | Known traversable candidate under current inputs | Existing migration may consider it, subject to hunger/season/RNG |
| present | yes | no | yes | Incoming blocked for now | Existing filter excludes target |
| present | no | yes | yes | Show possible exit from blocked ground | Existing owner may select unblocked neighbor |
| present | no | no | no | Water/archetype restriction is visible only if the owner resolves it | Existing seasonal calendar determines candidates |
| present | unknown | unknown | unknown | Graph known, live passability unknown | No change to movement |
| asymmetric | varies | varies | varies | Report authored directed edge or flag catalog issue | Do not normalize graph during read |

This truth table is intentionally not a pathfinding algorithm. Multi-hop route availability belongs to the existing map/expedition route owner, which may account for road closures and non-wildlife constraints not present in the migration graph.

## Appendix D — Information and privacy boundary

| Data | Existing precision | Allowed map report | Disallowed amplification |
|---|---|---|---|
| Pack current sector | Sector ID | Sector-level presence/movement after commit | Exact named location or coordinates |
| Pack population | Integer in saved owner | Only if current bestiary policy permits | Expose exact counts to a surface that intentionally hides them |
| Wildlife observation | Species, sector, day, confidence | Existing observation level/age | Convert one observation into census or extinction claim |
| Location contamination | Location record value | Site-level separate condition field | Use as sector-wide wildlife pressure without approved projection |
| Blocked sector | Current host projection | Current blocked status and source day | Persist a second stale blockage cache |
| Authored adjacency | Sector neighbor list | Known graph edge | Claim operational route open to people/vehicles |

The UI must respect existing bestiary knowledge gating. A species may have a current pack record but an intentionally limited exact-count display; GR-3 must not reveal counts through a corridor tooltip as a side channel.

## Appendix E — Data review checklist

Before adding or changing any data, record:

1. whether the source belongs to `world_evolution_seeds.json`, `wildlife_ecosystem.json`, or an existing map catalog;
2. whether the ID is a sector, location, region, species, or route node;
3. whether adjacency is directed or assumed undirected by the current consumer;
4. whether the water flag is a sector property or a route property;
5. whether blockage comes from authored data or a host's live daily projection;
6. whether the species restriction is defined in the data or inferred by `WildlifeSeasonalCalendar`;
7. what the loader does on missing/invalid data;
8. which integrity validator checks duplicate IDs and references;
9. whether the added fact changes migration or is purely visual;
10. which existing test already covers that invariant.

If any item is unknown, P0 remains open. A map screenshot or successful catalog parse does not prove movement is integrated; a corridor line must stay labeled according to its actual owner.

## Appendix F — Candidate query contract

The following contract is a **PROPOSED / VERIFY** value shape for discussion, not a requested new class or approved public API. The current owners may already expose a better read surface.

### F.1 Inputs

| Input | Candidate value | Verification question |
|---|---|---|
| Origin sector | canonical string ID | Does current map route use this exact sector vocabulary? |
| Destination sector | canonical string ID | Is it a neighbor in the same graph version? |
| Adjacency | present / absent / invalid | Is graph directed and how are asymmetric rows handled? |
| Destination blocked | true / false / unknown | When was live faction blockage last projected? |
| Origin blocked | true / false / unknown | Does current rule allow fleeing from blocked ground? |
| Water classification | source-owned classification | Is water a property of sector or edge for this rule? |
| Movement archetype | owner-resolved enum | Which `WildlifeSeasonalCalendar` helper defines it? |
| Current season window | canonical profile result | Is profile bound in this host; what does missing mean? |
| Pack/sighting evidence | sector-level records | Does the reader respect bestiary knowledge and observation confidence? |
| Location condition overlay | optional GR-1 value | Is there a verified sector-to-location mapping? |

Do not pass a raw arbitrary species tag from JSON into presentation and assume it is migration logic. The current ecosystem species definition has `diet_type`, tags, radiation tolerance, and other facts, while migration's seasonal calendar resolves movement archetype. Those data can be related only through the existing owner’s explicit rule.

### F.2 Outputs

Candidate transient report fields:

- normalized `from_sector_id` and `to_sector_id`;
- topology status (`known_edge`, `no_known_edge`, `invalid_or_unknown`);
- current inbound block status (separate from outbound escape);
- water-rule status as resolved by migration owner (`allowed`, `filtered`, `unknown`);
- source/version/day for any live blocker or pack report;
- optional GR-1 context, explicitly named `land_record_context`, not `habitat_suitability`;
- pack display detail constrained by existing bestiary/knowledge policy;
- an unknown reason rather than default-green fallback.

No output should include a speculative numeric carrying capacity, recovery multiplier, probability of return, or predicted arrival day. The migration engine's daily probability is not a corridor property and is influenced by hunger, season, candidate selection and RNG.

### F.3 Snapshot and mutability

The live migration partial exposes `TryGetNeighbors` with a `List<string>` out value in the inspected source. P0 must verify whether this returns the owner’s mutable list and whether any current caller edits it. A Godot UI must not receive and modify a canonical neighbor list. If current public API leaks mutability, the smallest fix would be an owner-provided immutable copy/read projection, but only under a path claim. Do not create a copied graph that becomes stale or save it separately.

## Appendix G — Exact simulation boundary examples

### G.1 One day, one migration tick

The campaign day owner supplies a deterministic fork for the wildlife migration call, then invokes ecosystem processing after migration. A corridor report after that point can compare pack sector before/after if the host captured the pre-state; otherwise it reports only current sector. GR-3 must not call `TickDay` to “refresh” the map. Doing so could grow starvation, move a pack, alter population, trigger events and consume seeded draws.

P0 should verify whether `WildlifeMigrationSystem.TickDay` has a same-day early return. In the inspected live method, `lastMigrationDay` is written and the pack loop continues; no same-day guard is visible there. The day owner is the effective exactly-once boundary. A duplicate call in one day can apply another starvation increment, another migration roll, another population loss/birth, and another event. Readout code must not introduce such a call, and any day-owner acceptance must test actual orchestration.

### G.2 Ecosystem tick and migration tick differ

`WildlifeEcosystemSystem.TickDay` includes an early return when `last_tick_day == day`; migration's live tick as inspected does not. This distinction is important for a save/replay test. Do not assume that because the ecological sub-owner is guarded, the migration owner is also idempotent. Test and fix only under the owning migration integration plan; GR-3 should remain read-only.

### G.3 Blocked-sector projection

The migration owner documents blocked sectors as a host-projected state recomputed each day, not persisted. A newly opened map after load may see no block state until host setup/tick recomputes it. P0 must identify the setup binding and display boundary. The UI may show “block status not yet refreshed” while no projection exists; it cannot persist yesterday's blockage in a map cache.

### G.4 Restore and graph rebind

`EvolvingWorldSeeder.Seed` sets sector adjacency and water sectors, then registers packs only if pack state is empty. It can be called after restoring a save without replacing restored pack populations, while topology is rebound from authored catalog. The corridor view should not call the seeder; the world host/session owns setup. An absent seed catalog may leave the graph empty and the view unknown. Do not inject a hand-authored fallback network.

If catalog topology changes between game versions, saved `currentSectorId` may refer to a removed sector or new graph. GR-3 does not migrate wildlife records. The owner must define compatibility before any data migration; a read-only report can display “sector no longer in current graph” while existing save recovery policy handles source state.

### G.5 Deterministic movement candidate order

The migration path chooses from candidate neighbors using a seeded `Next(0, candidates.Count)`. Changing adjacency order can change selected destination for the same seed even if the graph has the same edges. A projection may sort its own output copy for stable UI, but must not sort or replace the live owner list. Any new map crosswalk must have a separate deterministic ordering and cannot be fed back into migration candidate order.

### G.6 Observation and radio commit

The host reacts to committed pack movement with bounded radio intercepts and may unlock an existing field-guide entry. A corridor panel that listens to the movement event must not fire a second radio line or observation event. On save restore, already-recorded species observations remain in their owner; reconstructing a movement transition by diffing current state against a stale panel snapshot can create a false new sighting. Use source-owned event identity or display current state only.

## Appendix H — Wildlife/map edge-case matrix

| Edge case | Map report | Wildlife owner behavior | Required guard |
|---|---|---|---|
| No graph loaded | Unknown topology | Existing tick sees no adjacency and pack may stay put | Do not invent default neighbors |
| Origin ID unknown | No route result | Existing owner state untouched | Reject query without adding IDs |
| Destination in graph but not neighbor | No known direct edge | Movement does not consider it by direct adjacency | Do not pathfind through unspecified intermediates |
| Duplicate neighbor ID | Catalog validation issue; one visible edge at most | Current candidate count could be biased by duplicate entries | Flag at data validation; do not silently mutate in read layer |
| Asymmetric edge | Show authored direction or mark invalid based on route contract | Owner may only traverse from the row it loaded | Do not assume reciprocal connection |
| Pack record sector missing from graph | Report pack source at saved sector but graph relation unknown | Existing pack may not move from missing node | Never relocate record during map read |
| Population zero | Consult canonical owner and extinction policy | Pack may remain zero or be handled by ecosystem logic | No spawn from viewing a corridor |
| Remnant population threshold | Use existing ecosystem extinction fact | Extinction is not equivalent to no sighting | Do not expose or reinterpret hidden count |
| Water-bound species, water destination | Owner applies water candidate filter | Candidate can remain if calendar permits | UI uses owner result, not tag guess |
| Water-bound species, dry destination | Report restriction only if resolved by owner | Candidate filtered by existing code | No new migration rule |
| Destination blocked | Mark incoming blocked for current projection | Owner excludes target | Do not disable possible outbound flight from blocked origin |
| Every neighbor blocked | Explain no current open target if owner provides result | Pack stays, hunger can increase | No bypass action |
| Season profile absent | Mark seasonal movement context unknown/neutral as owner defines | Existing documented neutral fallback | Do not substitute a Green Return seasonal table |
| Data changed after save | Show topology mismatch | Saved pack persists until owner migration | No automatic pack reassignment |
| Display order changes | Rows remain stable sorted copy | Simulation source order stays untouched | Separate presentation sort |

## Appendix I — Acceptance examples by layer

### Core read query

Given the same immutable graph/block/water inputs in two different collection insertion orders, the report is identical. Calling it once or fifty times leaves migration and location captures unchanged. An unknown sector returns an unknown result and does not create a pack or adjacency row.

### Data loading

Given a valid seed catalog, existing loader/validator identifies duplicate or unresolved IDs before any new display layer trusts the mapping. Missing optional crosswalk data leaves migration and map route behavior exactly as today. The GR feature does not add a second JSON file containing the same neighbors.

### Host integration

Given a committed day update, the panel can render the current result. Given a pre-day snapshot restore, the panel refresh discards any transient prior-day result. A successful load and reseed restores pack state while rebinding authored topology as the existing owner intends. No extra tick occurs.

### Knowledge UI

Given a species with no bestiary observation, a corridor marker cannot reveal species or counts that current knowledge policy withholds. Given one low-confidence sighting, its confidence and date are not upgraded by route adjacency. Given a known movement event, the map does not duplicate its journal/radio or knowledge effects.

### No-provider parity

Run the same bounded seeded scenario with corridor provider absent and present (presentation-only). Compare `WildlifeSaveState` captures after each day, including pack ordering and values. They must match exactly. This is a focused parity check, not permission to run the broad ecosystem simulation suite.

## Appendix J — Stop/decision record template

If P0 identifies a requested capability that cannot be met by the read-only seam, report it using this short record:

| Field | Required content |
|---|---|
| Requested behavior | One concrete player-visible outcome |
| Existing owner | Current Core/host owner and exact API |
| Missing input | Exact absent field/provider/event |
| Proposed delta | Smallest owner change that would supply it |
| Second Nature overlap | Whether population, crops, food web or biology changes |
| Save/RNG impact | Whether canonical save or stream behavior changes |
| Alternatives | Honest map-only report or defer |
| Required approver | Foreman/owner/integrator signature |
| Stop boundary | Files and behavior untouched until approval |

The template exists to turn a blocker into a reviewable seam decision. It is not an invitation to start implementation while the signature is pending.

## Appendix K — Live blockade data-flow and day applicability

The continued source audit found an existing, concrete host projection in `EvolvingWorldDayOwner` (`src/Main.CampaignOwners.cs`). After the location tick, wildlife migration tick, and wildlife ecosystem tick, the owner clears wildlife sector blockages and projects blocks from the current dominant faction: it iterates authored `location_seeds`, selects seeds owned by that faction, resolves a sector via `SectorOfLocation`, then calls `SetSectorBlocked(sector, true)`. The accompanying comment states the projection is stateless and the migration runtime does not persist blockage. The seed DTO has an optional `sector_id` used by that lookup.

This is stronger evidence than an abstract proposal that blocked-sector data “may” exist. It also creates an important timing constraint:

| Step during campaign day N | Owner operation | Effect on migration |
|---|---|---|
| Before day N world movement | In-memory blocker set holds prior completed projection if setup has already populated it | Determines which destination sectors the migration tick can enter |
| World movement | `WildlifeMigrationSystem.TickDay(N, world fork)` filters candidate neighbors against current `_blockedSectors` | Changes packs/population through canonical migration logic |
| Ecology follow-up | `TickWildlifeEcosystemDay(N)` runs using its own population/migration/apex forks | Ecosystem can also mutate packs through migration APIs |
| Later in `EvolvingWorldDayOwner` | Host calls `ClearSectorBlockages`, then derives and writes the current dominant-faction projection | Replaces in-memory blockers for the next migration tick |
| Save/load boundary | Wildlife save captures packs; block set is not part of `WildlifeSaveState` | Block projection must be rebuilt by its owning host before movement relies on it |

P0 should verify how a fresh boot obtains its first projection before the first day tick. The code comment says the runtime does not persist blockage, but the map must not assume the block set is immediately reconstructed on load merely because packs and sectors are restored. It should either expose an explicit host projection freshness flag, wait until the first projection completes, or show block status as unknown until that point. GR-3 does not add a persisted copy.

### K.1 Day N sample walkthrough

Suppose dominant faction `F` owns two authored location seeds. One has `sector_id = sector_4_canyon`; another lacks a sector ID. On the post-ecology projection step for day N, the first sector becomes blocked and the second location contributes no block. The report can state that `sector_4_canyon` is blocked in the host's current projection if a read accessor exposes that committed result. It must not claim that day N migration considered this new projection: migration already occurred earlier in the sequence. The state is used by the next migration call, subject to fresh recomputation. The location lacking sector mapping remains unmapped; no ID derivation is allowed.

If dominance changes between N and N+1, day N+1's migration begins from the most recently projected block set, and the later N+1 projection replaces it. Exact behavior during restored/fresh initialization must be established by the actual coordinator. Any UI that labels blockers “effective now” without exposing this timing is potentially misleading.

### K.2 Faction ownership versus land condition

The host projection uses seed owner and faction dominance to block movement. It does not use contamination, ruin, vegetation, land condition, or population. Therefore a sector can be blocked while its land report reads lower contamination; a species corridor can be environmentally unknown while a faction owns the ground; and a dominant faction transition can change route permission without any land-condition change. Keep these axes separate in UI and tests.

### K.3 Scope consequence

The existing projector is a host responsibility and a shared day seam. GR-3 may add a *read-only* surface only after P0 finds a safe way to query the same set. It must not reimplement `SectorOfLocation`, run faction dominance itself, use `currentOwner` strings as a substitute for this projection, or write blockers on map selection. Any proposed change to when blockage becomes effective must go through the current world owner and its integrator, outside the scope of this draft.

## Appendix L — Replay, snapshot, and failure matrix for live corridors

| Event | Canonical data before event | Expected corridor projection | Expected pack/save effect | Recovery requirement |
|---|---|---|---|---|
| Normal day after prior block projection | Prior in-memory blocked set + restored packs | Shows the prior committed block state with source day if available | Migration uses world RNG stream once through campaign owner | After successful day, report updated only after new projection |
| Dominant faction changes this day | Old block set at movement step; new dominance known later | During day transition defer or show last committed projection; post-day use newly projected block set | Packs may have moved under prior set for this tick | Do not retroactively reinterpret movement |
| Seed has no `sector_id` | Location/faction ownership exists, sector lookup absent | No mapped block; mapping unknown | No arbitrary sector is blocked | Keep location fact; report mapping gap |
| Two owned locations map to same sector | Multiple seeds resolve one sector | One blocked sector in set; provenance may list both only if useful | Migration filter still treats set membership once | Projection should dedupe naturally and display no duplicate edge |
| Save restored before owner recomputes blockade | Pack save restored; blocker set absent | Block status unknown until host projection | Do not let view trigger migration or persist block | Owner initialization must rebuild state before authoritative movement |
| Mid-day rollback | Pre-day location, wildlife, landmark snapshots restored; block projection may be in-memory | Discard any transient post-tick report and await retry/owner reproject | Retry uses deterministic campaign streams; block owner must participate in restore semantics | Confirm snapshot includes/rebuilds derived blockers or recomputes them |
| Missing seed catalog | No authored sector projection | Unknown, no GR default | Existing owner fallback remains | No hand-authored GR block map |
| Corrupt location save | World save recovery policy determines valid location state | Block projection must derive only from recovered canonical owner | Do not keep a stale map cache of old owner fields | Rebuild after successful restore, otherwise report unavailable |
| Duplicate same-day migration invocation | Current migration state has `lastMigrationDay` | Report can show state but cannot repair engine | Current code audit did not find a same-day guard in migration tick | Day coordinator test/owner review; no GR-side dedupe |

Focused test planning should target only the changed integration seam. A map-only report with an existing accessor needs projection tests; a new host read accessor may need one host test proving block status is unknown before setup and correct after the phase-4 owner projects it. Do not add tests that retest the full migration algorithm unless its behavior changes.

## Appendix M — Species movement interpretation table

An authored fauna row contains identity, display name, radiation tolerance, diet, apex threshold, taming, chance, and tags. The movement owner also consults seasonal archetype and sector graph rules. These fields are not interchangeable.

| Source fact | Current semantic owner | Corridor UI may say | Corridor UI may not infer |
|---|---|---|---|
| `diet_type = herbivore` | Fauna catalog/ecology | Species dietary category if current bestiary displays it | Presence of forage at a sector |
| `radiation_tolerance` | Ecology survival pressure | Existing tolerance descriptor only where authorized | Land safe to inhabit or population will recover |
| tag `water` | Authored species definition | Water-associated tag if bestiary allows | Migration passability; actual filter is owner calendar logic |
| `seasonal_moves` row | Wildlife seasonal catalog | Seasonal behavior is authored | Specific destination edge unless graph/calendar resolve it |
| `currentSectorId` | Migration pack | Current sector-level pack location | Named map site's occupancy |
| `starvationLevel` | Migration save owner | Only expose if existing UI already permits | Route quality or land condition |
| `extinct_species_sectors` | Ecosystem owner | Owner-confirmed local extinction state | Global extinction or empty sector from no sighting |
| `observations[].confidence` | Wildlife observation owner | Observation confidence with source day | Land survey confidence |

If the current bestiary intentionally hides exact population until “documented,” a route tooltip cannot bypass that gate. A route can be known while species occupancy remains undisclosed.

## Appendix N — Duplicate audit: adjacent plans and measurement owners

### N.1 Plan 1 “Wildlife Corridor Journals” is not an implemented duplicate

The duplicate sweep inspected `docs/plans/expansion_wave1/PLAN_01_WILDLAND_FIRE_AND_BURN_RECOVERY.md`. Its header says `DRAFT — premise and authority audit required`; its scope is a report-to-recovery story and optional outdoor incident mechanics, subject to owner review. Its closing receipt calls itself documentation-only and `READY FOR PREMISE AUDIT`, and says it does not claim implementation. There is no `FULLY INTEGRATED` status in the inspected document.

The phrase “Wildlife Corridor Journals” occurs under Pass 167 as a generic planning template. The ten field prompts request an authority seam, player loop, data contract, state ladder, interface, accessibility, performance, content, verification, and rollout. The wording does not specify a concrete corridor record schema, sector-edge rules, live wildlife API, save owner, producer, consumer, event route, or acceptance fixture. Repeated stock sentences and similar template rows occur in neighboring passes; this is a topicized placeholder, not a second executable or review-ready Green Return design. The same plan explicitly states that `WildlifeEcosystemSystem` and `WildlifeMigrationSystem` remain the fauna/population authorities and that outdoor incident content must not write their save sections.

Therefore this file is relevant prior design evidence and an adjacent duplicate-check target, but it does not supersede this narrow readout. The actual duplicate risk remains a second graph, pack store, migration simulation, blockage ledger, or suitability model. P0 must still search any newer wildlife/corridor plan and current source before implementation. Do not “complete” Pass 167 by adding population or movement changes here.

### N.2 InSAR and Cartography are separate evidence domains

`InSarDeformationEngine` (`Assets/Ashfall.Core/World/InSarDeformationEngine.cs`) accepts survey passes keyed to sector and reference geometry, requires at least two compatible passes over positive day span, and derives coherence, relative displacement in millimeters, velocity, confidence, and a deformation classification. Its bounded pass list and `insar_deformation` save registration show an established geophysical time-series owner. `InSarMappingHostSession` and `Main.InSarMapping.cs` provide the host route. This is not an ecological or wildlife corridor authority: displacement does not determine habitat suitability, prey availability, species occupancy, or movement. GR-3 may display an InSAR warning as a separately attributed map context only if the current consumer already supports it; it cannot use it as a new wildlife blocker or migration preference.

`CartographySystem.ProjectCanonicalMap` returns map-knowledge rows: node identity, fog state, survey quality/tier, last-confirmed day, and provenance. That surface can help resolve whether a place is known to the player, but it does not attest land quality or habitat. A `Visited`/`Surveyed` node is not a wildlife observation and cannot stand in for `WildlifeObservation`'s species, sector, day, and confidence fields.

| Data source | Valid corridor contribution | Invalid conversion |
|---|---|---|
| Wildlife migration adjacency/block set | Existing route edge and current host-projected movement constraint | New edge inferred from adjacent named locations |
| Wildlife pack state | Existing sector-level movement/population facts, subject to visibility policy | Exact site occupancy, causal habitat preference, or map-created pack |
| Wildlife observation | Existing species/sector/day/confidence report | Contamination sample or guarantee of current pack presence |
| InSAR deformation summary | Separately labeled sector geophysical warning/context | Wildlife suitability, ecological recovery, or a migration mutation |
| Canonical map survey | Map-node identity and knowledge/provenance state | Physical land state or biological corridor proof |

### N.3 Worked non-join example

Assume a cartographer confirms node `site_A` on day 31, an InSAR sector summary for `sector_4_hinterlands` is processed on day 32, and a wildlife pack is recorded in that sector on day 33. Unless an authored canonical mapping binds `site_A` to that sector, the corridor panel cannot join these records. Even with a mapping, the three rows retain different meanings and times: map knowledge, sector deformation, sector pack. The panel may list all three with separate provenance if the existing route can do so; it may not infer that the animal traversed the site, that deformation blocked passage, or that the land recovered.

The no-mapping state is a successful, truthful result: display the known sector edge and wildlife owner facts, and say local land linkage is unknown. No string prefix, coordinate proximity, or matching display label may silently create the missing relation.

## Appendix O — Corridor report state table and reviewer fixtures

The corridor readout should be derived from current canonical topology and host projection. The following cases specify how the consumer reports uncertainty without changing migration.

| Graph/source state | Report state | Detail text may include | Must not imply |
|---|---|---|---|
| Authored adjacent sectors; neither is blocked | Known edge | Sector IDs/display labels and current season/water context only if existing source provides it | Pack will choose this edge or land is suitable |
| Authored edge intersects a current blocker | Blocked under current projection | Blocking source/day if the host exposes provenance | Permanent closure or ecological damage |
| Source/target exist but no authored edge | No known edge | “Route not present in current sector graph” | Impassable geography or extinction |
| Sector ID missing from wildlife owner | Unknown | The unavailable sector fact, if useful | Empty sector or zero population |
| Sector-to-location mapping absent | Land link unknown | Wildlife sector facts without site condition join | Nearby locations belong to the sector |
| Pack in sector, no observation record | Sector-level pack known | Current pack detail under existing visibility policy | Recent sighting or exact map-site presence |
| Observation exists, pack no longer present | Historical observation | Species, observation sector/day/confidence | Current pack presence |
| Observation confidence low | Qualified report | Owner confidence and date | Certain movement or permanent corridor use |
| Water-only movement rule active | Water constraint, when owner exposes it | Existing water-sector fact | A new GR-3 water route or species ability |
| Projection not yet rebuilt after restore | Block status unknown/stale | “Awaiting world projection” if route supports it | Unblocked state from an empty in-memory set |

### O.1 Minimum fixtures for a read-only projection

1. **Known edge, empty packs:** edge is displayed; no wildlife occupancy is invented.
2. **Pack in source sector, destination edge blocked:** show the pack's existing sector and blocked edge separately; do not move it or reinterpret its presence.
3. **Historical observation, current pack elsewhere:** show the observation as dated; current pack query remains authoritative for current position.
4. **Two locations map to one sector:** one graph sector remains one node; the view may list multiple mapped locations only if authored provenance exists.
5. **One location has no sector ID:** it is omitted from the crosswalk and contributes no block or wildlife association.
6. **Restored save with blocker projection absent:** state is unknown until the current host owner rebuilds it; opening the map does not rebuild or save it.
7. **New dominant faction takes effect after movement step:** the readout reflects the new committed host projection after the day transaction; it does not claim that this day’s migration used a block derived later in the same sequence.
8. **Same seed, same saved state, same ordered events:** projection output ordering is stable and querying consumes no RNG.

### O.2 Owner-specific acceptance matrix

| Owner | Read contract to prove | Acceptance | Failure boundary |
|---|---|---|---|
| `EvolvingWorldSeeder` | Authored sector IDs, adjacency, water sectors, and optional location `sector_id` | Every displayed sector/edge resolves to current catalog | Missing/malformed topology yields unavailable/unknown, not a generated edge |
| `WildlifeMigrationSystem` | Pack state, neighbor query, water and current blocked-sector queries | Same owner result is used for display; no mutation occurs | No parallel pack list, block set, or movement operation |
| `WildlifeEcosystemSystem` | Observation schema and current ecology reads | Species/sector/day/confidence displayed as existing wildlife evidence | No generalized sample APIs or new population calculations |
| `EvolvingWorldDayOwner` | Phase ordering and rebuild timing for faction blockers | UI labels the last completed projection; save/replay behavior matches host lifecycle | GR-3 must stop if correct “current” semantics cannot be exposed safely |
| Map presenter | Existing selected-node/corridor route and refresh lifecycle | Selection/reload updates from owners with stable ordering | No UI-owned occupancy, stale blocker cache, or side effect on open |

If any acceptance row cannot be verified using a public read seam, the correct next step is an owner/integrator decision. Reflection, direct private-field reads, and a duplicate host projection are not substitute seams.

## Appendix P — Worked hinterlands corridor lifecycle

This sequence uses authored records in `world_evolution_seeds.json` to show why corridor reporting needs both source provenance and day applicability. `pack_hinterland_dogs` starts in `sector_4_hinterlands` with population 6. `loc_ration_queue_plaza` is seeded in that sector with owner `faction_the_compact`, contamination `0.1`, and no threats. `loc_grange_hall` is also in the sector, owner `none`, contamination `0.1`. The example does not assert any current dominant faction or guarantee a movement result; it holds hypothetical inputs constant so a reviewer can reason through ownership.

| Campaign step | Existing owner operation | Example result | Allowed readout |
|---|---|---|---|
| Fresh world seed | Seeder loads authored sector graph, water sectors, wildlife packs, and location seeds | Dog pack and two location seeds resolve to same sector ID | Map can show the sector node; location linkage only if the authored `sector_id` is surfaced safely |
| Before first completed day projection | Block set may not yet have been rebuilt in a fresh runtime | State must be considered unknown until host projection is ready | Do not interpret empty in-memory blocker set as proof of openness |
| Day N migration begins | `WildlifeMigrationSystem.TickDay` reads its then-current block set; blocked sectors are excluded as destinations, while packs already inside may flee to unblocked ground | Pack moves only if existing season/hunger/RNG rules select a legal neighbor | Projection reports existing owner facts; it never calls movement itself |
| Ecology tick follows movement | `TickWildlifeEcosystemDay` can apply existing ecology mutations through migration APIs | Species pressure/observation remains wildlife-owned | No Green Return population change |
| Host recomputes dominance blockers later in day | Clears prior set; iterates location seeds owned by current dominant faction; resolves sector through `SectorOfLocation`; sets those sector blocks | If dominance is `faction_the_compact`, the plaza seed maps `sector_4_hinterlands` to blocked | New blocker is the committed current projection for next movement tick, not proof of this day's earlier migration input |
| Map refresh after day owner commits | Read-only query reads current pack and blocker owner state | Block may show alongside the pack still inside the sector | “Blocked as a destination under current faction projection” is accurate; “wildlife displaced by today's takeover” requires movement evidence |
| Save/reload | Wildlife pack state restores; blockage projection is not part of `WildlifeSaveState` | Host must rebuild derived block set before describing it as current | Until rebuilt, report block as unknown or not refreshed |
| Next day movement | Prior committed faction projection is present when the tick begins, if initialization/replay contract succeeded | Migration owner applies its normal 25% daily migration chance to starving packs above threshold and other existing rules | No extra corridor chance or recovery bonus |

This sequence is intentionally precise about ordering. The host's phase-4 coordinator moves wildlife before its later daily block projection. A read model must not rewrite the historical interpretation of a completed day after the new block set is installed. If the player opens the map mid-transaction, the UI should continue showing the last committed projection or a loading/unavailable state according to existing lifecycle; it must not inspect a partially updated set.

### P.1 Proposed corridor DTO, all values read-only

The following contract is a candidate projection. It should be implemented only if an existing map provider can expose every source safely. It is not a new persisted entity or pathfinding model.

| Field | Source | Candidate type/meaning | Null/unknown behavior |
|---|---|---|---|
| `from_sector_id`, `to_sector_id` | Canonical authored topology / `TryGetNeighbors` | Ordered IDs for one existing directed or undirected edge, according to catalog semantics | Missing endpoint or absent adjacency returns no known edge |
| `topology_source` | Seed catalog/source row | Provenance of the adjacency fact | Omit if provider cannot name source |
| `blocked` | `IsSectorBlocked(to)` after host projection | Current destination restriction | Unknown until host projection has completed |
| `block_projection_day` | Host day coordinator, if exposed | Day of latest completed reproject | Unknown if not persisted/exposed; never use view refresh day |
| `block_source_location_ids` | Existing host input only if safe immutable provenance can be derived without recalculating rules | Optional explanation of which authored owner seeds caused the block | Omit rather than recompute faction ownership in the panel |
| `water_sector` | `IsWaterSector` | Existing water constraint bit | Unknown if graph/setup is unavailable |
| `pack_ids` | Wildlife owner’s sector-filtered pack query, only if visibility allows | Current sector-level pack identities | Empty list is valid only when source owner successfully answered |
| `population` | Existing owner query | Aggregate under existing wildlife disclosure policy | Do not infer zero on lookup failure |
| `observation_refs` | Existing `WildlifeObservation` entries, if query exists | Stable IDs or dated source details for historical reports | No observation means “none recorded,” not “no wildlife” |
| `land_context` | Explicit sector/location relation plus GR-1 output | Separate context label; never suitability score | Unknown mapping stays unknown |
| `in_sar_context` | Existing InSAR summary for same canonical sector | Separate deformation warning and units | No summary is not “stable” |
| `report_day` | Last committed host/campaign day | Freshness only | Unknown before commit, no local clock |

### P.2 Full acceptance matrix: same sector, different owners

| Fixture | Required result | Proves | Fails if |
|---|---|---|---|
| Hinterlands pack present, no block, topology loaded | Pack and edge facts appear from canonical owners | Existing state can be read without mutation | Opening/refresh changes pack or RNG state |
| Hinterlands target becomes faction-blocked after movement phase | Block appears after day commit; prior movement remains historical | Phase order preserved | UI retroactively claims same-day block affected earlier movement |
| Pack remains inside newly blocked sector | Sector-level pack presence can coexist with blocked destination status | Block means “cannot move into,” not “empty” or “cannot exit” | Readout hides pack or teleports it |
| Seed owner changes but dominant faction does not | Block set changes only when the host's projection inputs dictate | Owner and dominance semantics remain canonical | GR independently decides ownership from map label |
| Two dominant-owned locations share sector | One sector block, stable single edge state | Set semantics deduplicate sector membership | Duplicate rows produce duplicate routes |
| Location seed lacks `sector_id` | No corresponding wildlife block/link | Missing mapping is preserved | Guessed adjacent sector is blocked |
| Low-confidence historical observation | Show dated qualified observation separately | Confidence and day remain with wildlife owner | Observation is asserted as current pack location |
| InSAR low-coherence summary | Show only separate owner warning if available | Geophysical uncertainty does not become wildlife rule | Corridor is automatically blocked or marked restored |
| Wildlife save restored, host projection unavailable | Pack may be known; blocker unknown | Persistence gap is represented honestly | Empty transient set is displayed as open |
| Ordered same-seed replay | Same query output order and owner snapshot | Projection itself is deterministic and RNG-free | Map query changes seeded stream or save hash |

If an API does not provide a stable current blocker query after restore, do not add a GR save flag to “fix” display freshness. The host owner must expose or rebuild its existing projection, or the UI must keep the blocker unknown.

## Appendix Q — Existing migration rule effects on corridor wording

`WildlifeMigrationSystem.Live.cs` exposes current constants and comments that constrain any truthful corridor explanation: `MigrationChancePerDay = 0.25`, `HungerDriveThreshold = 0.5`, `MigrationStarvationRelief = 0.35`, `StarvationLossThreshold = 0.7`, `RabiesChancePerDay = 0.03`, and `BreathingRoomDaysForBirth = 3`. The migration owner performs those rules; GR-3 must not duplicate them in a map tooltip or run them to predict a result. The table distinguishes a visible current fact from a misleading promise.

| Existing rule/input | Existing owner meaning | Safe explanatory wording if source data is exposed | Unsafe Green Return addition |
|---|---|---|---|
| Daily migration chance 0.25 | Starving pack above hunger threshold may move under existing tick/RNG | “Movement is possible under existing wildlife rules” | “25% chance this corridor will be used,” unless source-owner logic confirms probability applies to this exact edge and context |
| Hunger drive threshold 0.5 | Hunger pressure gates migration behavior | Existing UI may describe starvation state | Corridor score weighted by hunger in a second calculation |
| Starvation relief 0.35 on new ground | Migration owner updates pack condition after movement | Report actual current pack state only where visible | Claim food exists in destination or land recovery caused relief |
| Starvation loss threshold 0.7 | Existing daily survival rule can reduce population | Current owner population/pressure fact if UI allows | Predict deaths using a GR simulation or imply corridor blocks caused loss without a replayed owner result |
| Rabies chance 0.03 above loss threshold | Existing migration owner seeded disease roll | Existing authored health/risk state if exposed through owner | Add another disease chance for crossing a Green Return edge |
| Three days between births | Existing reproduction cadence | Population trend belongs to wildlife owner | Promise population increase after a steward action |
| Water-only sector filter | Water-associated packs are constrained to owner-marked sectors | Distinct water constraint if exposed | Infer marine/river path from map art or location label |
| Blocked destination | Wildlife owner filters target sectors; already-present packs can flee to unblocked ground | “Cannot enter as a destination under the current block projection” | “All movement is impossible” or “sector is empty” |
| Season profile | Owner binds season profile; null can preserve season-neutral behavior | Current season state only if owner exposes it | Add season preference from Green Return land bands |
| RNG fork | Host passes campaign world-evolution stream with stable discriminator | No player-facing probability promise from a read-only panel | Consume RNG to preview a possible path |

### Q.1 Proposed explanatory text tests

These are content acceptance examples; they should be localized and aligned with existing terminology if a UI change is approved.

| Source facts | Acceptable concise report | Reject |
|---|---|---|
| Pack in sector; one destination is blocked; another legal neighbor exists | “The pack is in this sector. One destination is blocked by current faction control.” | “The pack is trapped” |
| Pack in sector, but no current observation | “Pack state is known at sector scale; no recent sighting is recorded.” | “No animals seen, so the route is empty.” |
| Historical observation day 30; current day 45; pack state unavailable | “Last recorded observation: day 30. Current presence is unknown.” | “Wildlife uses this corridor.” |
| Sector topology loaded; no location mapping | “Known sector route. Local site linkage unknown.” | “Animals cross the nearby site.” |
| InSAR low-coherence result and wildlife edge exists | “Deformation reading is inconclusive; wildlife edge is separately known.” | “The animals avoid unstable ground.” |
| Low contamination location value and no habitat model | “Current contamination record available; habitat suitability unknown.” | “Recovered habitat attracts wildlife.” |

### Q.2 Acceptance tests must observe rather than simulate

For a corridor-only change, the highest-value checks are read-path purity and owner-consistent output, not a duplicate run of all migration behavior. A narrow fixture should snapshot `WildlifeSaveState`, location state, and any accessible seeded-RNG test state; query the corridor multiple times in different map selections; then assert those values remain unchanged. A separate owner test may already cover migration's blocked-target semantics. If coverage is absent, coordinate with the wildlife owner; do not make the Green Return plan a second migration test suite.

| Test tier | Candidate test | Scope control |
|---|---|---|
| Pure Core projector | Given canonical graph/block/pack DTO inputs, output stable edge order and unknown semantics | No engine tick or RNG call |
| Host provider | After owner setup, read current blocker and pack facts | Verify no writes to migration/location state |
| Restore lifecycle | Restored pack with projection not rebuilt reports blocker unknown | One relevant host lifecycle target; no unrelated save stores |
| Rollback | Projection refresh follows restored owner snapshot | Only if provider subscribes to transaction events or host route changes |
| Narrative/data | Catalog IDs and sector references validate | Use current focused data validator; do not author new species rows |
| UI route | Keyboard/controller can inspect status and source labels | Run only changed-route UI probe/snapshot, not all map panels |

Acceptance must compare against the existing wildlife owner result and source DTO, not against an expected corridor narrative. If “read path consumes no RNG” cannot be observed through a public test seam, use a narrow deterministic replay fixture owned by the migration test package rather than exposing RNG internals to the panel.

## Appendix R — Topology integrity audit and migration-selection weighting

The current `world_evolution_seeds.json` declares schema version 1, collection ID `world_evolution_seeds`, 24 sectors, 24 packs, 12 canonical species, and 40 location seeds in its description. For this audit I read the authored sector graph and checked each `neighbors` array for duplicate strings and each edge for a reciprocal reverse edge. The targeted `jq` checks returned no duplicate neighbor within any authored array and no non-reciprocal edge. The earlier apparent duplicate in one console excerpt came from overlapping line ranges at the boundary; it was not present in the file. No content repair is warranted from that inspection.

| Integrity check | Method/result on current source snapshot | Why GR-3 cares |
|---|---|---|
| Sector count and IDs | Parsed `world_evolution_seeds.json`; 24 unique authored sector records observed | Display keys must use current IDs and not invent intermediate nodes |
| Duplicate neighbors within one sector | Grouped each `neighbors` array by exact ID; no duplicates observed | A duplicate would remain in current adjacency list and could weight a random destination |
| Reverse-edge existence | For each edge `A -> B`, checked that `B.neighbors` contains `A`; none missing in current catalog | Graph is reciprocal in this snapshot, but runtime must still follow owner direction semantics |
| Unknown endpoint | Each listed neighbor checked against authored sector IDs; no unknown endpoints observed in this scan | Missing target must not render as an invisible/phantom edge |
| Runtime installation | `EvolvingWorldSeeder.Seed` calls `SetSectorAdjacency` with a copy of each authored neighbor list | Seeder does not itself deduplicate or canonicalize the list |
| Runtime seasonal filtering | `WildlifeSeasonalCalendar.FilterNeighbors` enumerates neighbor entries into a candidate list; water filtering may retain each occurrence | Any future duplicate data could affect candidate multiplicity |
| Destination choice | Migration selects using `rng.Next(0, candidates.Count)` | A duplicate candidate could increase destination probability without changing stable replay determinism |

The implication is conditional: current catalog scan found no duplicate and no asymmetry, but the source path is not defensive against every malformed topology input. If a future seed introduces the same destination twice, normal and water-filtered migration can preserve repeated entries; the final indexed RNG selection would weight that destination more heavily. Same-seed replay would still reproduce the bias, so determinism tests alone would not catch it. The GR-3 consumer should not normalize a private copy because that would make display topology differ from migration topology. A future data-authority task may add catalog validation or a wildlife-owner normalization rule if the integrator approves it.

### R.1 Narrow regression acceptance cases

| Candidate fixture | Expected owner/read-model behavior | Test owner and scope |
|---|---|---|
| Current authored graph snapshot | 24 IDs resolve; every edge has a known endpoint; no duplicate per adjacency row | Focused data/catalog validation; do not duplicate in UI test |
| Hypothetical duplicate `A -> B` input | Validator reports duplicate before runtime, or the wildlife owner explicitly documents normalization | Catalog/owner contract; GR-3 projector does not hide it |
| Hypothetical dangling `A -> missing` input | Load/validation reports missing endpoint; no map phantom edge | Data integrity target |
| Hypothetical one-way edge | Preserve direction only if schema/runtime contract allows directed movement; otherwise validation reports asymmetry | Owner decision, not assumption in presenter |
| Merged runtime edge duplicates an existing edge | `MergeSectorAdjacency` currently skips additions already present in the existing list | Existing Core owner behavior; test only if change modifies it |
| Query current valid graph repeatedly | Same ordered edge report; no mutation or RNG | GR-3 projection/provider target only |

This appendix records input integrity for a map consumer. It does not recommend changing the JSON graph, migration weights, seasonal filters, or RNG calls in the Green Return package.

## Appendix S — Wildlife observation lifecycle and visibility contract

The current ecology source provides a narrowly typed observation record in `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs`: `WildlifeObservation` contains `species_id`, `sector_id`, `day`, and `confidence`. `RecordObservation(speciesId, sectorId, day, confidence)` clamps confidence to `[0,1]`, appends a row, trims the oldest rows when the list exceeds `ObservationLogCapacity = 200`, and emits `OnWildlifeObserved(speciesId, sectorId)`. `CaptureState` copies observation rows; restore rebuilds the list. `Observations` exposes a read-only interface over the current rows, and `ObservationCount(speciesId)` supports knowledge progression.

These are facts about a wildlife knowledge log, not a complete occupancy stream. The writer API does not contain location ID, exact pack ID, sensor method, duplicate source key, correction lineage, or a maximum freshness window. Confidence is clamped, not used here as a Green Return generic quality scale. It is suitable as attributed wildlife evidence under this owner's existing use; it is insufficient to infer current corridor suitability.

| Lifecycle case | Existing wildlife owner behavior | GR-3 read treatment |
|---|---|---|
| Observation added for species/sector/day | Appends row, clamps confidence, trims to 200, raises event | Read as dated wildlife observation; do not add a second log |
| Same species/sector/day reported twice | No idempotency key is evident in this method | Do not deduplicate by matching fields; distinct sightings may share day/sector |
| Observation older than current day | Retained until bounded log truncation removes it | Historical; no freshness threshold invented by GR-3 |
| More than 200 observations | Oldest rows are removed from the retained list | Missing row cannot mean “no sighting ever” |
| Confidence outside `[0,1]` at write | Clamped to range | Display stored value; do not reinterpret it as sensor accuracy |
| Confidence is zero | Row still exists with zero confidence | Present only as low-confidence report if existing UI permits |
| Species or sector ID unresolved | Writer does not establish a Green Return mapping | Preserve owner row; do not synthesize site ID |
| Save/load | Observation fields are cloned/restored under wildlife ecology state | Compare restored owner output; no GR copy/save section |
| Observation row evicted | Owner no longer exposes it in retained collection | Show retained evidence only; no archive implied |

### S.1 Reproducible observation scenarios

**Single current report.** On day 24 a scout observation records `species_rad_dog` in `sector_4_hinterlands` at confidence `0.8`. The readout may say that a qualified dog observation is recorded for that sector on day 24 if existing presentation permits confidence. It may not say the pack currently remains there; migration may have changed `currentSectorId` after the observation.

**Current pack without observation.** Migration save reports a pack at the hinterlands sector, but no observation row exists. A corridor report can state current sector-level pack state under the wildlife visibility policy; it cannot create a “sighting” record because the map drew a marker.

**Observation but no current pack.** A day-20 observation remains in the retained log; current pack query returns no matching pack in that sector on day 24. Display “last observation, day 20” separately from current pack status. Do not show an animal icon as if present.

**Retention eviction.** After 201 additional observations, the oldest record falls outside the 200-row log. A map retaining a cached old sighting would contradict current owner state. On mutation/restore, refresh and retire the stale row. Do not create a second archive to avoid the owner’s retention policy.

### S.2 Acceptance matrix by observation consumer

| Concern | Required test/inspection | Pass condition | Out of scope |
|---|---|---|---|
| Schema | Read `WildlifeObservation` and save DTO | Only species, sector, day, confidence assumed | Add location, habitat, or method fields in GR-3 |
| Bounds | Verify `RecordObservation` confidence clamp | Projector consumes owner value unchanged | Confidence-to-habitat conversion |
| Capacity | Verify `ObservationLogCapacity` and owner tests | UI drops evicted observations on refresh | Extending/duplicating archive |
| Restore | Existing capture/restore test or focused owner test | Same retained rows return after load | Map-side save section |
| Events | Trace `OnWildlifeObserved` subscribers | Refresh only after committed owner event if transaction supports it | Second event bus/log |
| Visibility | Existing bestiary/map disclosure rules | No population/pack identity leak beyond current policy | New visibility progression |
| Current versus historical | Pair observation log with current migration read | Dated evidence and current state stay separate | Inferring absence from no observations |

`WildlifeEcosystemSystemTests.BestiaryKnowledge_IsObservationGated` demonstrates that observation count affects bestiary knowledge levels. This is another reason not to record observations when the map opens: that would change knowledge progression. Any provider displaying wildlife evidence must be proven not to call `RecordObservation` or trigger that progression.
