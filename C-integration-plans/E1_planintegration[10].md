---
PLAN_ID: E1-10
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 10
STATUS: READY_FOR_EXECUTION_WHEN_RAILS_PASS
SOURCE_PLAN: "Plan 160 — Expedition Colony & Outpost System"
SEQUENCE_FILENAME: "E1_planintegration[10].md"
PREVIOUS_FILENAME: "E1_planintegration[9].md"
NEXT_FILENAMES:
  - "E1_planintegration[11].md"
  - "E1_planintegration[12].md"
CATEGORY: LINK+EXPEDITIONS+OUTPOSTS+SUPPLY+TERRITORY
PRIMARY_INTENT: "Extend expeditions into one persistent fixed-site presence by composing existing site, route, caravan, roster, inventory, production, defense, faction, market, communications, governance, and save authorities."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SECOND_SHELTER_STACK_FORBIDDEN: true
SECOND_POPULATION_LEDGER_FORBIDDEN: true
SECOND_RESOURCE_LEDGER_FORBIDDEN: true
SECOND_TERRITORY_AUTHORITY_FORBIDDEN: true
RUNTIME_RISK: VERY_HIGH
SAVE_RISK: VERY_HIGH
BALANCE_RISK: VERY_HIGH
SCOPE_RISK: EXTREME
---

# E1 Plan Integration [10] — Expedition Outposts, Fixed Forward Presence, Supply Lines, Territorial Reach, and Colony-Scale Growth

> **Sequence rule:** this file is `E1_planintegration[10].md`.
> The next files are `E1_planintegration[11].md`, `E1_planintegration[12].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan expands Plan 160 into an implementation-grade fixed-outpost and colony-growth programme.

The source plan proposes permanent expedition colonies with population, buildings, resource production,
defense, supply lines, territorial influence, trade, communications, governance, and eventual colony
specialization. That is a compelling late-game direction, but it is also one of the highest-risk architectural
expansions in the roadmap because nearly every proposed colony mechanic already has or should have an
authoritative owner elsewhere.

If implemented naively, `ColonySystem` becomes a second shelter: a second population list, second food ledger,
second morale model, second inventory, second production engine, second defense system, second power/water
stack, second governance model, and second world-control authority. That would directly conflict with the
outpost architecture already established in the E1 sequence.

E1-10 therefore reframes the feature:

**a colony is not a miniature duplicate shelter; it is a persistent site identity whose capabilities are
projected through existing campaign authorities.**

The first accepted target is not five colony types. It is one fixed outpost at one real expedition
destination, staffed by canonical survivors, supplied by canonical caravans, using canonical inventory,
production, defense, faction-control, communications, and commitment systems.

Only after this vertical slice proves strategic value should the system scale toward settlement, fortress,
trading post, farming commune, multi-site networks, or colony federation.

## 1. Source Intent Preserved

Plan 160 asks for:

- permanent presence at expedition destinations;
- colony establishment;
- survivors and buildings;
- supply lines;
- defense;
- production and consumption;
- territorial influence;
- trade/economy;
- communications;
- governance;
- quests/events;
- persistence and old-save compatibility;
- deterministic outcomes;
- colony-map UI;
- fifteen buildings and ten blueprints.

E1-10 preserves the player-value goal—forward presence and territorial reach—but converts each proposed
subsystem into an integration contract with existing rails.

## 2. Core Architecture Thesis

```text
Existing expedition destination / world node
        |
        v
Persistent SiteIdentity
        |
        +--> site type / role
        +--> lifecycle
        +--> graph location
        +--> references to installed capabilities
        |
        v
Canonical campaign authorities
        |
        +--> survivors / roster / census
        +--> inventory / storage
        +--> ration / needs
        +--> production / jobs
        +--> caravans / routes / supply
        +--> defense / combat
        +--> faction control / treaties
        +--> market / trade
        +--> radio / information
        +--> commitments / governance
        |
        v
Site read model / outpost UI
```

The site layer owns identity and lifecycle. It does not own duplicate downstream facts.

## 3. Non-Negotiable Rules

- The first implementation target is one outpost type, not five colony archetypes.
- Outpost population is canonical survivor placement/assignment, not a duplicate survivor list.
- Outpost inventory is canonical inventory/container state.
- Outpost food is canonical ration/needs/inventory state.
- Production is canonical production/job authority.
- Supply is canonical route + caravan + inventory transfer.
- Defense is canonical defense/combat authority.
- Territory belongs to the existing faction/world-control authority.
- Trade belongs to Market/Economy authority.
- Communications belong to radio/information-flow rails.
- Governance belongs to governance/commitment/leadership authority.
- Location evolution remains owned by `LocationEvolutionSystem`.
- The outpost layer may store site identity, lifecycle, installed site capability references, and cross-system
  references necessary to compose the site.
- No generic `resources: map` inside the site DTO unless it is merely a serialized reference to canonical
  inventory storage—and even then prefer container IDs.
- No generic `morale: 0-100` inside the site DTO.
- No generic `defense: 0-100` inside the site DTO.
- No generic `territorialInfluenceMap` inside the site DTO unless that is already the world-control authority.
- No fixed influence-radius ownership in the outpost system.
- No population growth simulation inside the site system.
- Births/refugees/recruitment use canonical population rails.
- Supply cannot credit destination resources before canonical delivery.
- Site loss must reconcile population, inventory, commitments, control, and history.
- Site construction uses real materials/labor/time.
- Save/load is idempotent.
- Old saves get empty site state unless existing waystations/camps can be safely migrated.
- The first slice must be playable without any colony micromanagement screen beyond one outpost management view.

## 4. Relationship to Earlier E1 Architecture

This plan is downstream of the earlier outpost/continuation plan and must not supersede its one-authority
constraints. Where that plan defined:

- outpost identity;
- campaign save section;
- site-aware census;
- site-aware rations;
- route/caravan supply;
- loss/memorial/legacy consequences;
- `waystation_network` UI;
- continuation memory;

E1-10 should consume those rails rather than create `ColonySystem` as a parallel domain.

If those rails do not exist at implementation HEAD, E1-10 may be BLOCKED and should produce prerequisite
tickets rather than filling gaps locally.

## 5. Acceptance Slices

### Slice A — Fixed outpost at existing node
Establish one site, one persistent identity, one staffing model.

### Slice B — Supply and survival
Food, storage, caravan resupply, route disruption, evacuation.

### Slice C — Site capabilities
One storage/housing/utility/production capability at a time.

### Slice D — Defense and world reaction
Site can be threatened, defended, lost, and remembered.

### Slice E — Trade/communications/governance
Only after base site state remains architecture-safe.

### Slice F — Colony scale
Settlement/fortress/trading/farming specialization only if measured demand exists.


---

## E1-10A — Premise verification and outpost/colony authority audit

**Goal:** Verify all outpost, waystation, camp, expedition, route, caravan, roster, inventory, production, defense, faction, market, communication, governance, and save rails before adding new site state.

### Required substeps

1. Inspect `ExpeditionSystem`, `ExpeditionVehicleSystem`, `LocationEvolutionSystem`, route/world graph, waystation/camp systems, survivor roster/census, inventory/storage, ration/needs, production, caravan/trade, shelter defense/combat, faction standing/control/treaty, market, communications/radio, governance/commitments, memorial/legacy, and save registry.
2. Search for any already-implemented `Outpost`, `Waystation`, `ForwardBase`, `Settlement`, `Colony`, `Camp`, `TradingPost`, or `Site` authorities.
3. Verify current expedition destination identity and whether locations can persist changed state.
4. Verify whether site-aware inventory, census, ration, production, and assignment rails already exist.
5. Verify whether previous E1 outpost architecture is implemented or still only planned.
6. Map every proposed Plan 160 field to a canonical owner.
7. Mark `population`, `resources`, `defense`, `morale`, and `territorial influence` as prohibited local copies unless audit proves no owner exists and a separate rail is approved.
8. Create `docs/systems/OUTPOST_COLONY_AUTHORITY_MAP.md`.
9. Create intake duplicate-search evidence linking Plan 58/outpost work, Plan 152 mobile camps, Plan 156 shelter expansion, and relevant supply/control plans.
10. Set `PREMISE_VERIFIED_AT` to current HEAD.
11. Stop if a fixed-outpost authority already exists and extend it instead.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10B — Colony/outpost ADR and scope collapse

**Goal:** Decide whether Plan 160 should implement a new colony authority, extend the existing outpost model, or remain blocked.

### Required substeps

1. Write an ADR comparing `ColonySystem`, existing outpost/waystation extension, and generic persistent-site authority.
2. Default to extending the existing outpost/site model.
3. Define site-owned facts: stable site ID, world location/node reference, site archetype/role, lifecycle, establishment state, installed capability references, and provenance/history IDs.
4. Explicitly exclude population stats, resources, morale, defense score, production queues, faction control, market state, and communications state.
5. Define first-release archetype as `outpost` or `forward_post` only.
6. Define how later settlement/fortress/trading/farming specializations can be represented as capability compositions rather than five hard-coded systems.
7. Define feature flags for trade, defense, local production, governance, and population growth.
8. Require second-tool review before code.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10C — Persistent site identity and lifecycle contract

**Goal:** Create stable site identity that attaches to real expedition destinations and survives save/load.

### Required substeps

1. Define site ID, location/node ID, authored/generated display name, lifecycle status, established day, role/archetype tag, capability references, and optional founder/provenance IDs.
2. Define lifecycle: Candidate, Establishing, Active, UnderSupplied, Isolated, Evacuating, Abandoned, Lost, Ruined, Reoccupied where each state has real semantics.
3. Prefer derived UnderSupplied/Isolated status where possible.
4. Do not store current population count if census can derive it.
5. Do not store raw resources if inventory can derive them.
6. Define stable identity across abandonment/reoccupation.
7. Define save schema and versioning.
8. Add validation for unresolved location, duplicate site ID, illegal lifecycle transition, and missing capability definition.
9. Add migration fixture from no site state.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10D — Location eligibility and fixed-site establishment

**Goal:** Establish sites only at real world nodes/locations that satisfy authored and runtime constraints.

### Required substeps

1. Verify location category, ownership, hazard, knowledge/reveal, route access, and expedition reachability.
2. Define site-eligibility policy as data/queries, not hard-coded location IDs.
3. Require location to be discovered/known at appropriate knowledge rung.
4. Require route/path to exist.
5. Require faction/treaty legality where world-control rails support it.
6. Define whether some locations are permanently ineligible.
7. Define one-site-per-location or multi-site rules explicitly.
8. Add tests for eligible, hidden, unreachable, hostile-controlled, already-occupied, invalid-location, and removed-location cases.
9. Do not create new world nodes merely because colony system needs space.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10E — Establishment project and initial investment

**Goal:** Use canonical construction/production/commitment rails to create the first fixed outpost.

### Required substeps

1. Define establishment blueprint/capability bill.
2. Use inventory for material reservation.
3. Use duty/roster for assigned builders.
4. Use expedition/travel to move builders/materials to site.
5. Use canonical project/job scheduler for duration.
6. Define whether initial shelter/tent/storage capability must be completed before Active state.
7. Handle route closure mid-establishment.
8. Handle cancellation/abandonment.
9. Make completion transaction idempotent.
10. Add tests for insufficient materials, insufficient labor, route closure, save/load, cancellation, and duplicate completion.
11. Do not create a local construction scheduler.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10F — Site staffing and survivor placement

**Goal:** Represent outpost staff through canonical survivor placement, roster, census, and consent systems.

### Required substeps

1. Define site assignment target in the canonical roster.
2. Move survivors via expedition/travel authority rather than teleporting placement state.
3. Validate fitness, consent, policy, role, and route access.
4. Define minimum staffing as capability requirement where needed.
5. Define recall/reassignment.
6. Define survivor status while traveling to/from site.
7. Ensure census groups survivors by site without duplicates.
8. Add tests for assignment, recall, stranded staff, unfit survivor, duplicate assignment, death at site, and save/load.
9. Do not store a parallel `population` list in site state if survivor placement already owns it.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10G — Food, ration, and survival integration

**Goal:** Make outpost residents consume from canonical food/needs authorities.

### Required substeps

1. Create or reuse site-aware inventory access.
2. Use canonical ration/consumption system for daily food draw.
3. Compute days-of-supply as derived read model.
4. Handle short ration/starvation through Needs.
5. Do not store site morale/hunger summaries as canonical state.
6. Define emergency ration policy through governance/ration authority.
7. Add tests for full ration, shortage, no supply, resumed supply, midnight save/reload, and no double-consumption.
8. Measure site food pressure.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10H — Canonical site storage

**Goal:** Give the site inventory capacity through canonical containers, not a resource dictionary.

### Required substeps

1. Create/register one or more inventory containers based on site capabilities.
2. Define accessibility and capacity.
3. Define item transfers from caravan/expedition/shelter inventory.
4. Define overflow behavior.
5. Ensure item metadata survives transfer.
6. Add tests for add/remove/transfer, full storage, container removal, site loss, and save/load.
7. Prohibit generic `resources: map` state.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10I — Supply route contract

**Goal:** Represent supply as canonical routes/paths rather than site-owned route lists.

### Required substeps

1. Define origin and destination site/shelter references.
2. Resolve route/path through world graph.
3. Store route plan/reference only where caravan authority requires it.
4. Query weather, hazards, faction control, and closures live.
5. Do not copy route openness into site state.
6. Add tests for valid route, closed route, hostile segment, no path, route changed, and save/load.
7. Expose route health as derived status.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10J — Caravan delivery and supply-line scheduler

**Goal:** Deliver resources only through real vehicle/caravan movement and canonical inventory transactions.

### Required substeps

1. Use existing caravan/vehicle dispatch.
2. Assign cargo manifest through inventory.
3. Credit destination only on arrival.
4. Handle loss, delay, reroute, return, partial delivery, and full destination storage.
5. Define recurring supply orders only if a canonical scheduling/commitment system exists.
6. Make delivery idempotent.
7. Add tests for arrival, loss, delayed route, partial delivery, duplicate arrival event, and save/reload.
8. Do not simulate abstract daily supply ticks detached from actual transport unless deliberately approved.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10K — Personnel transfer through supply network

**Goal:** Move survivors between shelter and sites through canonical expedition/travel state.

### Required substeps

1. Define transfer request and travel group.
2. Use route/path validation.
3. Apply travel time, hazards, needs, and vehicle capacity.
4. Do not alter site census until canonical placement changes.
5. Handle interruption, death, missing, return, and route closure.
6. Add tests for successful transfer, stranded group, no capacity, invalid route, and save/load.
7. Keep personnel movement separate from item shipment logic where authorities differ.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10L — Site capability model

**Goal:** Represent buildings/specializations as capabilities consumed by existing systems rather than local stat blocks.

### Required substeps

1. Define capability IDs such as sleeping, storage, medical, workshop, farm/production, defense, watch, radio, market, power, water, governance, quarantine.
2. Each capability references a real consumer authority.
3. Installed capabilities may come from construction blueprints/projects.
4. Do not store arbitrary `effects` lists.
5. Define compatibility/dependency between capabilities.
6. Add integrity validation that each capability has a registered consumer.
7. Start with storage + sleeping + radio/watch or one production capability.
8. Defer broad building taxonomy until capability composition works.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10M — Site construction and upgrade integration

**Goal:** Use existing construction/production rails to add fixed-site capabilities.

### Required substeps

1. Reuse E1-9-style project patterns where available.
2. Target a site and capability slot.
3. Use real resources, labor, tools, and duration.
4. Do not create separate `ColonyBuilding` state if capability installation can reference room/building instances from a generic construction authority.
5. Define condition/maintenance ownership.
6. Add tests for build, upgrade, invalid capability, no labor, no resource, save/load, and duplicate completion.
7. Keep first slice to 2-3 site capabilities.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10N — Production at outposts

**Goal:** Allow one local producer through canonical production/inventory/labor systems.

### Required substeps

1. Select one justified producer: scavenging relay, farm, mine, workshop, or water collection only if supporting rails exist.
2. Use canonical recipes/jobs/inputs/outputs.
3. Use site-assigned workers through duty roster.
4. Use site inventory for inputs/outputs.
5. Deliver output to storage only on job completion.
6. Do not create colony-wide abstract production-per-day numbers unless production authority already uses them.
7. Add tests for normal production, no input, no worker, full storage, route isolation, and save/load.
8. Measure whether local production meaningfully offsets supply burden.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10O — Market/trading-post integration

**Goal:** Turn a site into a trade point through canonical market/economy authority rather than a new colony market.

### Required substeps

1. Audit `MarketSystem` location binding.
2. Register eligible active site as a market location or market capability.
3. Use existing prices, stock, standing, and transaction rules.
4. Do not store local price tables in site state.
5. Define trader access via route/world rules.
6. Add tests for market enabled, route isolated, faction restriction, inventory transfer, and save/load.
7. Feature-gate trading-post specialization until baseline outpost is stable.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10P — Communications and early warning

**Goal:** Use radio/information-flow rails to make fixed sites useful as network nodes.

### Required substeps

1. Expose site radio/watch capability.
2. Use existing radio coverage and information-flow authorities.
3. Do not create a site-local communications system.
4. Allow warnings/intel to propagate through canonical channels.
5. Define power/staffing requirements.
6. Add tests for active, unpowered, unstaffed, isolated, destroyed, and save/load.
7. Measure actionable warnings attributable to site coverage.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10Q — Territory and faction-control integration

**Goal:** Make site presence influence world-control policy without owning territorial truth.

### Required substeps

1. Audit world/faction territory authority.
2. Define site presence as an input/event to that authority.
3. Do not store influence radius or ownership map in site state.
4. Use graph/node/region semantics rather than arbitrary kilometers unless world model supports metric distance.
5. Define contested, neutral, allied, and hostile location behavior.
6. Apply treaties/standing rules through faction authority.
7. Add tests for establishing in neutral, allied, contested, hostile territory, control flip, and site loss.
8. Keep offensive conquest out of first slice.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10R — Local defense capability

**Goal:** Defend fixed sites through canonical defense/combat systems.

### Required substeps

1. Audit shelter defense and tactical combat applicability to remote sites.
2. Register site defense capabilities, fortifications, and garrison references.
3. Do not store generic `defense: 0-100`.
4. Use site-assigned survivors as garrison through roster/placement.
5. Use canonical weapons/ammunition/inventory.
6. Resolve attack outcome through defense/combat authority.
7. Add tests for undefended site, staffed defense, no ammo, damaged fortification, evacuation, and save/load.
8. Feature-gate siege depth if remote-defense rails are not ready.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10S — Attack, loss, evacuation, and casualty reconciliation

**Goal:** Handle site failure as a cross-system transaction, not a status toggle.

### Required substeps

1. Define loss causes: attack, abandonment, evacuation, starvation collapse, scripted event, or world-control change where supported.
2. Resolve survivors through travel/health/death authority.
3. Resolve inventory as evacuated, looted, destroyed, or abandoned through canonical item/world rules.
4. Resolve commitments and supply orders.
5. Resolve capability/building condition.
6. Resolve faction/control consequences.
7. Record memorial/history/legacy events.
8. Transition site lifecycle exactly once.
9. Add tests for clean evacuation, failed defense, casualties, lost cargo, abandoned equipment, reoccupation, and save/load.
10. Ensure no survivor remains assigned to a lost site.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10T — Commitments, obligations, and satisfiability

**Goal:** Bind outposts to the campaign's existing commitments rather than hidden automatic obligations.

### Required substeps

1. Allow supply, defense, staffing, trade, or faction promises to target site IDs.
2. Use canonical commitment authority.
3. Run satisfiability checks before accepting impossible deliveries or duties.
4. Handle route closure after commitment acceptance.
5. Handle site loss.
6. Do not create site-specific promise state.
7. Add tests for satisfiable supply promise, impossible route, lost site, late delivery, and save/load.
8. Use this rail to make remote presence strategically costly.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10U — Governance and local administration gate

**Goal:** Do not create colony government until governance rails exist and fixed-site staffing is stable.

### Required substeps

1. Audit governance/leadership/consent systems.
2. Represent site policy needs as requests to canonical governance.
3. Do not store a second site morale/leadership/government simulation.
4. Define only minimal local administrator assignment if roster/role authority supports it.
5. Use consent/coercion rules for drafted staff.
6. Defer elections, revolts, local laws, taxes, and autonomy to separate plans.
7. Add tests for administrator assignment and site policy delegation if enabled.
8. Feature-flag all local-government depth.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10V — Population growth and recruitment gate

**Goal:** Keep births, refugees, and recruitment under canonical population systems.

### Required substeps

1. Audit cohort/birth/refugee/recruitment systems.
2. Do not increase site population by a local growth tick.
3. Assign incoming people through canonical survivor creation/placement.
4. Respect site capacity/housing.
5. Use census to report population.
6. Add tests for refugee arrival, birth placement if relevant, recruitment, full capacity, and transfer.
7. Defer autonomous colony population growth until broader demographic rails prove it.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10W — Morale and social life boundary

**Goal:** Do not create colony-wide morale as a separate scalar.

### Required substeps

1. Use survivor-level morale/mental-health and social systems.
2. Derive site social health summaries for UI only.
3. Allow site events/conditions to emit canonical stress/support events.
4. Use local conditions such as isolation, ration shortage, safety, crowding, leadership through existing systems.
5. Do not store `morale: 0-100`.
6. Add tests for shortage stress, safe-site support, social event, and derived summary consistency.
7. Keep revolt mechanics out of first slice.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10X — Revolt and rebellion follow-on gate

**Goal:** Explicitly defer colony revolt until autonomy, ideological friction, governance, and collective grievance rails are mature.

### Required substeps

1. Define prerequisites: E1-6 autonomy, E1-7 ideological/group friction, governance authority, collective grievance, site staffing, and conflict resolution.
2. Do not implement a random low-morale revolt event.
3. Use real survivor groups/leadership if future plan enables it.
4. Require fail-safe and player agency.
5. Keep Plan 160's `The Revolt` quest/event disabled in foundational release.
6. Document a separate follow-on ticket.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10Y — Colony type reclassification as specialization bundles

**Goal:** Replace five hard-coded colony systems with optional site specializations composed from capabilities.

### Required substeps

1. Define Outpost as minimal persistent site.
2. Define Settlement as outpost + housing/social/local-production capacity once rails exist.
3. Define Fortress as site + defense/garrison capability bundle.
4. Define Trading Post as site + market/storage/communications capability bundle.
5. Define Farming Commune as site + agricultural production/water/storage/housing bundle.
6. Do not create separate class hierarchies unless behavior truly differs.
7. Allow role label to be derived from installed capabilities or explicitly selected with requirements.
8. Add tests for specialization eligibility and role transition.
9. Keep first release role fixed as Outpost.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10Z — Blueprint and site-building content authority

**Goal:** Create data-driven site capability/construction content only after the generic site model is stable.

### Required substeps

1. Define blueprint ID, target site role/capability, material bill, labor, duration, prerequisite research/knowledge, topology/location constraints, installed capability IDs, and localization.
2. Validate all item/research/location/capability references.
3. Prohibit direct resource, morale, defense, faction, or production deltas in blueprint data.
4. Start with 5-6 site blueprints.
5. Expand toward 10 blueprints/15 buildings only after acceptance metrics.
6. Add integrity tests.
7. Document authoring rules.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10AA — LocationEvolution integration

**Goal:** Make fixed presence alter location state through the canonical location-evolution authority.

### Required substeps

1. Define site-established/site-lost/site-abandoned events.
2. Allow location state to reflect occupied/fortified/trading/ruined conditions through existing evolution APIs.
3. Do not let site layer overwrite unrelated location mutations.
4. Define precedence/conflict with scripted location evolution.
5. Add tests for establish, upgrade, loss, reoccupation, and unrelated mutation.
6. Persist location truth only in location authority.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10AB — Expedition-base integration

**Goal:** Use active sites as launch/resupply/return points through ExpeditionSystem.

### Required substeps

1. Register eligible site as expedition origin/waypoint.
2. Validate staff, supplies, route, vehicle, and knowledge.
3. Use canonical expedition dispatch.
4. Allow return to site and resupply from site inventory.
5. Do not create a second expedition engine.
6. Add tests for launch, return, unavailable site, isolated site, lost site, and save/load.
7. Measure range/endurance gains.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10AC — Vehicle/mobile-base interaction

**Goal:** Compose fixed outposts with E1-8 vehicle camps without merging their ownership.

### Required substeps

1. Allow vehicles to travel to and resupply at sites.
2. Allow mobile camp to operate near/away from fixed site.
3. Do not turn parked vehicles into site-owned inventory copies.
4. Use canonical cargo/fuel/repair transfers.
5. Define whether vehicles can be assigned to site defense or supply routes through existing assignment authority.
6. Add tests for vehicle resupply, repair, supply-line assignment, destroyed vehicle, and site loss.
7. Keep mobile and fixed base lifecycles separate.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10AD — Site network and multi-outpost scaling gate

**Goal:** Prove one outpost before enabling many-site management.

### Required substeps

1. Define first-release maximum active sites as one or very small cap.
2. Measure supply, staffing, UI, event, save, and performance burden.
3. Add second-site support only after first-site acceptance.
4. Use global site registry/index.
5. Ensure census/inventory/routes/commitments scale by site ID.
6. Prevent O(sites × all systems × all entities) ticks.
7. Add tests for zero, one, two, max configured sites.
8. Do not market 'vast territory' before multi-site performance passes.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10AE — Territorial influence presentation

**Goal:** Show strategic reach without inventing an unsupported influence-radius simulation.

### Required substeps

1. Use canonical controlled/known/reachable nodes or regions.
2. Render route reach, radio coverage, defended routes, trade access, or faction-control changes as separate layers.
3. Do not draw arbitrary 5-20 km circles unless the world model uses geographic radius.
4. Show contested/unsafe paths from world authority.
5. Add map tests for one site, multiple layers, isolated site, lost site, and control flip.
6. Keep visual influence derived.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10AF — Outpost management UI and read model

**Goal:** Present one site through authoritative summaries rather than a second shelter dashboard.

### Required substeps

1. Reuse `waystation_network`/expedition/site UI if available.
2. Show site identity, lifecycle, staff, days of supply, storage, incoming/outgoing shipments, route health, capabilities, production, defense status, faction control, communications, commitments, and blocking reasons.
3. Each field comes from its canonical query.
4. Each action calls canonical command: assign/recall, send supply, establish capability, evacuate, launch expedition, open market, etc.
5. Do not store UI-local gameplay state.
6. Add snapshot tests for establishing, active, undersupplied, isolated, threatened, evacuated, lost.
7. Add interaction tests for core commands.
8. Keep one-page summary compact.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10AG — Event and quest hooks

**Goal:** Add authored stories after the site loop is stable.

### Required substeps

1. Create event hooks for founding, first supply, isolation, recovery, attack, evacuation, trade opening, local discovery, and site loss.
2. Quest authority owns quests.
3. Prioritize Pioneer, Lifeline, Siege precursor, Expansion, Connection.
4. Defer Rebellion and large warfare.
5. Use site IDs as provenance.
6. Prevent duplicate event/quest triggers after reload.
7. Add tests for eligibility, dedupe, invalidation, site loss, and completion.
8. Keep event effects authority-owned.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10AH — Site journal, memorial, and legacy integration

**Goal:** Record significant fixed-site history without turning the journal into authoritative state.

### Required substeps

1. Use canonical journal/chronicle.
2. Record founding, major upgrade, siege, evacuation, loss, reoccupation, notable deaths, and landmark status.
3. Keep routine deliveries out of permanent chronicle.
4. Use bounded event history.
5. Allow E1-5 legacy to retain famous site records narratively.
6. Do not grant automatic cross-campaign site ownership.
7. Add tests for dedupe, loss, reoccupation, and legacy export.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10AI — Old-save compatibility and migration

**Goal:** Introduce fixed sites safely without inventing colonies for historical expedition destinations.

### Required substeps

1. Default missing site state to empty.
2. Do not retroactively convert every waystation/camp into a colony.
3. Only migrate existing persistent site records if schema/evidence clearly maps them.
4. Preserve existing expedition/location state.
5. Version site section.
6. Add migration fixtures for no site, active expedition camp, waystation, changed location, and unsupported prototype colony record.
7. Make migration idempotent.
8. Ensure old saves remain fully playable with zero sites.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10AJ — Save ownership and restoration ordering

**Goal:** Persist site identity/lifecycle while restoring cross-system references deterministically.

### Required substeps

1. Use one registered outpost/site save section.
2. Persist site identity, lifecycle, capability references, establishment transaction state, bounded site history references, and stable cross-system IDs.
3. Do not persist copied inventory/population/morale/defense/territory values.
4. Restore catalogs/world locations before site validation.
5. Restore inventory/roster/world/faction authorities before re-binding site references where needed.
6. Rebuild derived read models.
7. Add corruption/reference validation.
8. Add round-trip tests for establishing, active, isolated, under attack, evacuating, lost, and reoccupied states.
9. Ensure restore does not replay founding or delivery events.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10AK — Exploit and conservation audit

**Goal:** Prevent sites from duplicating people, items, production, trade, or territory through retries/reloads.

### Required substeps

1. Test establishment double completion.
2. Test duplicate survivor assignment.
3. Test duplicate caravan delivery.
4. Test save/reload around midnight ration tick.
5. Test site inventory transfer conservation.
6. Test production output once.
7. Test market transaction once.
8. Test lost-site cargo reconciliation.
9. Test reoccupation does not duplicate capabilities.
10. Test supply-line recurring order cannot spawn duplicate caravans.
11. Test blueprint unlock cannot be bypassed.
12. Test site founding cannot claim same location twice.
13. Add fuzz/property tests across lifecycle transitions if infrastructure supports it.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent collection has retention/cap rules.

### Negative tests

- Site DTO stores a second resource ledger.
- Site DTO stores a second morale/defense/territory scalar as truth.
- The same survivor is present at shelter and outpost simultaneously.
- A shipment credits resources before arrival or twice after reload.
- A lost site keeps active staff/production/market state.
- Reoccupation duplicates capability instances.
- A second site causes linear duplication of global simulation work in every subsystem.
- A UI action mutates local copies instead of canonical authorities.

### Acceptance evidence

- [ ] Deterministic targeted tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/population conservation passes.
- [ ] Headless one-site scenario passes.
- [ ] Micromanagement and balance metrics are captured.
- [ ] Performance remains bounded.

---

## E1-10AL — Balance and micromanagement audit

**Goal:** Ensure outposts create strategic reach rather than doubling shelter workload.

### Required substeps

1. Track player actions per site/week.
2. Automate routine supply planning only through explicit existing automation/commitment rails.
3. Limit manual local assignment changes.
4. Measure benefit: expedition range, route safety, intel, production, trade, defense.
5. Measure costs: labor, food, transport, vehicles, risk, commitments.
6. Check whether site becomes mandatory.
7. Check whether one specialization dominates.
8. Check whether local production trivializes shelter scarcity.
9. Keep maximum sites low until interaction burden is acceptable.
10. Run playtest with site enabled vs no site.
11. Do not expand to settlement/federation until micromanagement metrics pass.

### Outpost/colony invariants

- Site identity/lifecycle are local; population/resources/defense/territory remain canonical elsewhere.
- Supply credits only on real delivery.
- Survivor placement changes through roster/travel authority.
- All site capabilities have explicit canonical consumers.
- Save/reload cannot duplicate founding, delivery, production, or population.
- One outpost vertical slice precedes colony-scale breadth.
- Mass governance/revolt/warfare remains gated.
- Every persistent col

<!-- Deliverable capped to remain within the requested 50–90k character envelope. -->
