---
PLAN_ID: E1-2
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 2
STATUS: READY_WHEN_RAILS_PASS
SOURCE_PLAN: "Plan 58 — The Continuation: Outposts, Waystations, and a Second Holdfast"
DRIVE_FILENAME: "E1_planintegration[2].md"
PREVIOUS_FILENAME: "E1_planintegration.md"
NEXT_FILENAME: "E1_planintegration[3].md"
CATEGORY: LINK+CONTINUATION+SAVE_INTEGRATION
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
NO_SECOND_CAMPAIGN_AUTHORITY: true
NO_NG_PLUS_POWER_BONUS: true
---

# E1 Plan Integration [2] — Outposts, Waystations, and a Second Holdfast

> **Filename sequence:** `E1_planintegration.md` → `E1_planintegration[2].md` →
> `E1_planintegration[3].md` → `E1_planintegration[4].md` ...
>
> The bracketed sequence number belongs immediately before `.md`.

## 0. Flagship Mission

This integration plan converts Plan 58 into an execution-grade programme for remote outposts and campaign
continuation while enforcing the source plan's central architectural rule: **one authority per fact**.
Outposts must be projections of existing graph, waystation, expedition, roster, ration, caravan, weather,
territory, commitment, census, production, knowledge, memorial, relations, and legacy systems. They are not
permission to create a second shelter simulation. Continuation likewise carries bounded records and world
memory into a new campaign; it is not New Game Plus and must not grant mechanical inheritance by default.

The source plan already identifies the correct danger. Waystations and expedition camps exist, route and
knowledge rails exist or are planned, population instruments exist, and the save model is campaign/slot
scoped rather than site scoped. The implementation challenge is therefore integration and ownership, not
feature invention. If intake concludes that camps + waystations + graph already satisfy the desired player
experience, the correct outcome is a smaller patch or no new system at all.

## 1. Source Constraints Carried Forward

- Existing `WaystationSystem` is the starting point for capacity/forward-base semantics.
- Existing `waystation_network` routing should be extended before any new management surface is proposed.
- Expedition camp lifecycle already provides multi-day field-life semantics.
- Graph/routes, knowledge, dose, power, fitness/labour, commitments, generations, consent, relations,
  world autonomy, production, legacy records, and retention are prerequisite rails rather than optional extras.
- Population truth must remain in census/register/cohort/survivor authorities.
- Wider play space is justified through **reachability**, not uncontrolled map inflation.
- Active outpost state belongs in the campaign save envelope, never in an independent per-site save tree.
- Per-site logs and records require retention ceilings before scale.
- Plan intake must be passed before implementation.
- Plan 160/content work is a consumer of these rails, not a competing outpost authority.

## 2. Programme Definition of Success

A successful implementation allows the player to establish one remote post, staff it from the canonical
roster, feed it through the canonical ration/inventory pipeline, supply it through real routes and caravan
arrival, gain real intel/reachability value, suffer route/weather/control-driven isolation, evacuate or lose
the site, see the consequences in census/relations/memorial/legacy records, save/load deterministically, and
later begin a new campaign in a world that remembers bounded prior records without inheriting raw power.

The programme is not complete merely because an outpost JSON record exists. It is complete only when the
full loop is connected and tested.

## 3. Non-Negotiable Guardrails

1. One ration ledger.
2. One survivor aggregate and one roster truth.
3. One inventory/storage authority.
4. One graph/route truth.
5. One territory/faction truth.
6. One active campaign save envelope.
7. No outpost-local copies of weather, dose, survivor health, faction ownership, or route openness.
8. No new UI framework.
9. No unbounded per-site history.
10. No unreachable commitments accepted as satisfiable.
11. No previous-save live dependency after continuation seed creation.
12. No inherited stats/resources/equipment/recipes/multipliers without a later explicit pillar-level decision.
13. No implementation before rails/intake permit it.
14. No field in the outpost model without a named authoritative owner.
15. No silent map-node expansion beyond the ADR budget.

## 4. Execution Spine

`Intake → ADR → authority table → save contract → reachability budget → one-site establishment → staffing →
rations → supply lines → intel/control → commitments → population → production/storage → failure/loss → UI
extension → retention soak → continuation ADR → seed derivation → save isolation → remembered-world projection
→ clean-start parity → cross-campaign replay → Plan 160/content handoff → release gate`


---

## 58A.1 — Intake and premise refresh

**Goal:** Prove that the feature is admissible at current HEAD before changing runtime code.

### Required substeps

1. Re-run the plan-register/intake workflow and record the actual status of every hard rail named by Plan 58.
2. Re-verify WaystationSystem host wiring, the existing waystation_network surface, expedition camp events, campaign save ownership, and current graph node count.
3. Run duplicate searches across outpost, colony, waystation, camp, remote base, settlement, caravan, site, forward base, and second holdfast terminology.
4. Record which desired player outcomes are already supported and which are genuine missing seams.
5. If any hard rail is not ready, mark the plan BLOCKED with the exact prerequisite rather than inventing a local substitute.
6. Attach PREMIS E_VERIFIED_AT/current commit evidence to the registered plan.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58A.2 — ADR: what an outpost is

**Goal:** Define an outpost as a composition of existing authorities and cap any genuinely new state.

### Required substeps

1. Write ADR-0001-outposts.md before implementation.
2. Answer whether the feature is new durable authority or persistent waystation/camp state projected through graph + roster + supply + obligations.
3. Choose multi-site within one campaign as the default ownership model unless evidence forces another model.
4. Define the fiction boundary: reachability consequence, not bigger-map genre expansion.
5. Define a hard first-slice node budget and outpost count budget.
6. Define explicit non-goals including no second base-building resource graph, no local unit-control mode, and no procedural territory.
7. Require second-tool review against pillars and source evidence.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58A.3 — Authority ownership matrix

**Goal:** Name the owner of every fact before a schema is allowed to ship.

### Required substeps

1. Create a table for node, route, bunk capacity, staff, fitness, food, storage, power, dose, weather, control, treaty legality, production, knowledge, radio, obligations, census, memorials, and legacy records.
2. Mark values as site-owned, reference-only, or derived/query-only.
3. Forbid duplicate persisted copies of authority-owned facts.
4. Require an explicit blocker entry for any field with no owner.
5. Add an architectural test fixture that rejects prohibited duplicate fields in the outpost DTO/schema.
6. Use this matrix as the review checklist for every later task.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58A.4 — Campaign save contract

**Goal:** Add one campaign-owned outpost section without creating a site-scoped save tree.

### Required substeps

1. Read SaveSlotRoot, SaveStoreHub, SaveSectionRegistry, checksum, migration, and default-section conventions.
2. Register one outposts section with versioning and deterministic ordering.
3. Store only site identity/lifecycle and legitimate site-owned state; foreign domain facts remain references.
4. Define load order and orphan handling for missing node, survivor, route, catalog, or content identifiers.
5. Add empty-state migration from campaigns that predate the section.
6. Add checksum/corruption tests and save round-trips for active, isolated, lost, and abandoned sites.
7. Document campaign/site/legacy/slot boundaries in SAVE_MODEL.md.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58A.5 — Reachability and node-budget gate

**Goal:** Use graph reachability as the expansion mechanism and prevent silent world-scale growth.

### Required substeps

1. Re-verify graph authority and current resolvable nodes.
2. Require candidate sites to resolve to graph nodes and satisfy discovery/knowledge requirements.
3. Require route existence/availability for establishment.
4. Query route closure, weather, dose, and territory from owners instead of persisting local risk flags.
5. Define whether ruined/lost sites remain discoverable landmarks.
6. Add node-budget validation to content integrity checks.
7. Measure reachable strategic options rather than raw node count.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58B.1 — Establishment production triple

**Goal:** Build a post through canonical materials, labour, and duration contracts.

### Required substeps

1. Define one first-slice construction bill.
2. Reserve/consume materials through canonical inventory rules.
3. Assign labour through roster/duty and fitness checks.
4. Use existing production scheduling/duration conventions rather than an outpost-only queue.
5. Define interruption, cancellation, route closure, and material recovery semantics.
6. Add started/progress/blocked/completed/cancelled/failed events.
7. Add deterministic tests including mid-construction save/load.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58B.2 — Staffing and duty assignments

**Goal:** Post survivors using canonical roster, fitness, policy, consent, and relations systems.

### Required substeps

1. Represent a site posting as a normal duty assignment targeting a site ID.
2. Prevent incompatible simultaneous assignments.
3. Gate posting by fitness/needs according to existing rules.
4. Route voluntary/compulsory posting through existing policy/consent authorities.
5. Apply relationship consequences for separation where existing relations rails support it.
6. Keep posted survivors inside the canonical survivor aggregate and census.
7. Cover recall, reassignment, injury evacuation, death, and stranded states with tests.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58B.3 — Ration and food integration

**Goal:** Make remote food pressure a site dimension of the existing consumption authority.

### Required substeps

1. Identify the canonical ration/consumption owner.
2. Add site-aware withdrawal/query semantics at that owner or its canonical facade.
3. Derive demand from posted population and existing needs rules.
4. Transfer goods through canonical inventory/storage contracts.
5. Derive days-of-supply rather than persisting a second balance.
6. Apply short-ration/starvation through existing needs/health systems.
7. Add conservation tests proving transfers never create food and day-tick reloads never double-consume.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58B.4 — Route and caravan supply lines

**Goal:** Credit supplies only when the canonical caravan/transport authority actually delivers them.

### Required substeps

1. Represent supply lines as route/path references.
2. Use caravan arrival as the delivery boundary, not dispatch.
3. Use canonical shipment manifests and inventory transfer.
4. Integrate weather/territorial closure or risk at existing route/travel seams.
5. Define refusal for full/invalid destination storage.
6. Define reroute, hold, return, loss, and partial delivery using existing transport semantics.
7. Add deterministic replay tests for closure, hostility, full storage, loss, and partial delivery.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58B.5 — Intel and knowledge value

**Goal:** Make a functioning outpost extend information capability through existing radio/reveal rails.

### Required substeps

1. Define the exact player-facing intel benefit for the first slice.
2. Use existing radio/triangulation/intel authority for coverage and warnings.
3. Use the knowledge ladder for discovered/verified/current state.
4. Apply power and staffing dependencies through their owners.
5. Degrade benefit when unpowered, isolated, abandoned, or lost.
6. Add tests that prove coverage changes only through authoritative state.
7. Expose the value in the existing waystation surface after runtime queries exist.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58B.6 — Territory and treaty integration

**Goal:** Let autonomous world control affect outpost safety and legality without copying control state.

### Required substeps

1. Query control at site and route from existing faction/world authority.
2. Define friendly, neutral, contested, and hostile establishment semantics.
3. Apply treaty restrictions through existing treaty state.
4. Handle control flips while staffed and while caravans are en route.
5. Emit world events suitable for UI/intel/commitment consequences.
6. Add tests for legal/illegal establishment, hostile flip, and treaty prohibition.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58B.7 — Commitments and satisfiability

**Goal:** Allow outposts to become promise targets only when the commitment can be proven feasible at creation.

### Required substeps

1. Extend commitment target identity to a canonical site reference where needed.
2. Check route reachability, quantity, deadline, capacity, and known closure constraints before acceptance.
3. Define what happens when a previously valid commitment becomes impossible after a world change.
4. Record failure as a gameplay consequence, not a silent cancellation.
5. Use existing grievance/standing consequences for broken promises.
6. Add negative tests for unreachable, impossible-quantity, impossible-deadline, and lost-site targets.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58B.8 — Census and site membership

**Goal:** Track people by site without shadow populations or duplicated survivor records.

### Required substeps

1. Add or reuse canonical survivor-location/site identity.
2. Group census/register queries by current site.
3. Define how travelers in transit are counted.
4. Ensure campaign totals reconcile across holdfast, sites, transit, missing, dead, and other explicit categories.
5. Prevent a survivor from appearing at two sites.
6. Use retention policy for historical census rows.
7. Add save/load and death-in-transit/site tests.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58B.9 — Remote production and storage

**Goal:** Prove one bounded producer role using existing production and storage contracts.

### Required substeps

1. Choose exactly one first-slice production/relay role.
2. Use canonical recipe/input/labour/duration/output semantics.
3. Require staff, supply, power, and route prerequisites through existing authorities.
4. Deliver output to canonical storage or report refusal.
5. Do not create outpost-only item databases or recipe schedulers.
6. Add tests for missing input, no staff, full storage, isolation, and successful delivery.
7. Do not add a second producer archetype until the first is player-accepted.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58B.10 — Loss, evacuation, and reoccupation

**Goal:** Make site loss resolve every owned consequence rather than toggling a local flag.

### Required substeps

1. Define abandonment, evacuation, overrun/control loss, and supply-collapse outcomes.
2. Resolve survivor location, injury, death, missing, and return through canonical systems.
3. Resolve cargo and storage according to canonical transfer/loss rules.
4. Apply grievance/consent/leadership consequences through existing rails.
5. Create memorial/legacy records through existing record systems.
6. Define ruins/reoccupation as graph/knowledge consequences.
7. Add tests ensuring lost sites stop production and retain no phantom staff.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58B.11 — Waystation network UI extension

**Goal:** Extend the existing routed surface with authoritative live state and actions.

### Required substeps

1. Audit current WaystationNetworkPanel/route wiring first.
2. For every displayed field, name the query authority.
3. For every button/action, name the command authority and error semantics.
4. Keep only transient selection/filter state in UI.
5. Add click-through/navigation to survivor, route, obligation, inventory, or intel surfaces where applicable.
6. Keep keyboard navigation and standard accessibility behavior.
7. Add interaction tests so no button succeeds only in panel-local state.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58B.12 — Retention and 200-outpost-day soak

**Goal:** Bound all collections multiplied by site count and time before content expansion.

### Required substeps

1. Inventory logs, deliveries, census rows, warnings, obligations, production events, memorials, and site history.
2. Classify each as current state, bounded history, summary statistic, or legacy record.
3. Use the canonical retention rail for ceilings and pruning.
4. Run a 200-site-day soak with establishment, supply, isolation, loss, reoccupation, and census movement.
5. Capture memory growth and serialized section size.
6. Assert ceilings and deterministic pruning.
7. Block broader content if retention ceilings are exceeded.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58C.1 — Continuation ADR

**Goal:** Separate generational succession, campaign continuation, and New Game Plus in writing.

### Required substeps

1. Define generational succession as same holdfast over years and continuation as a new holdfast in the same remembered world.
2. Define the legacy ledger as records-only source.
3. List permitted seed categories and prohibited mechanical inheritance.
4. Define clean start as first-class and fully supported.
5. Define an explicit exception process for future mechanical inheritance proposals.
6. Version the continuation seed contract.
7. Require independent review of the no-bonus rule.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58C.2 — Deterministic continuation seed

**Goal:** Derive a bounded immutable initialization payload from approved legacy records.

### Required substeps

1. Create a pure seed-derivation path from legacy records plus new campaign seed.
2. Include only approved records such as sites, memorials, obligations, standing/treaty context, and ending facts.
3. Cap inherited records under retention policy.
4. Handle unsupported/missing/corrupt legacy records explicitly.
5. Add deterministic golden tests.
6. Ensure no raw prior runtime object graph crosses the boundary.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58C.3 — Previous-save isolation

**Goal:** Make the new campaign independent after initialization.

### Required substeps

1. Consume the continuation seed during normal new-campaign composition.
2. Drop all handles to the previous campaign envelope after creation.
3. Add guard tests that new-campaign services cannot access a previous-save provider.
4. Test that deleting/moving the old save does not affect the new campaign.
5. Test that mutating the old save after creation does not affect the new campaign.
6. Test that the new campaign can never write back into the old envelope.
7. Document the one-way boundary in SAVE_MODEL.md.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58C.4 — Remembered-world projection

**Goal:** Translate bounded records into world context through existing authorities.

### Required substeps

1. Project approved faction/treaty/standing starting conditions through canonical authorities.
2. Project prior lost sites, ruins, graves, and memorials through graph/knowledge/content rails where approved.
3. Carry obligations as obligations rather than rewards.
4. Decide which memories are immediately known and which require rediscovery.
5. Ensure remembered places do not bypass knowledge/reveal rules unless the ADR explicitly allows prior knowledge.
6. Add tests for memorial, ruin, standing, treaty, and obligation projection.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58C.5 — No-bonus and clean-start parity guards

**Goal:** Prove continuation changes context and responsibility rather than hard power.

### Required substeps

1. Define hard-power parity dimensions: starting resources, stats, equipment, production capacity, recipes/unlocks, and permanent modifiers.
2. Compare clean and continued starts under controlled seeds.
3. Fail if legacy fields influence prohibited balance calculations.
4. Allow continued starts to be harder where remembered obligations/hostility justify it.
5. Ensure clean start remains narratively complete and uses the same composition pipeline.
6. Add CI guards for no-stat, no-resource, no-recipe, and no-multiplier inheritance.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58C.6 — Cross-campaign replay and perception test

**Goal:** Prove the feature is deterministic and visible to returning players.

### Required substeps

1. Run seed replay across the campaign boundary.
2. Create a known prior-world fixture containing a lost site, memorial, treaty condition, and obligation.
3. Playtest the continued campaign and ask what the player noticed from last time.
4. Measure time-to-first-recognized-memory and number of seeded records actually surfaced.
5. If perception is low, improve presentation/discovery rather than widening the seed schema.
6. Confirm clean and continued modes both pass core acceptance sweeps.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58D.1 — Plan 160/content integration handoff

**Goal:** Make expansion content consume canonical outpost rails rather than defining a competing system.

### Required substeps

1. Map every Plan 160 field/action to the authority matrix.
2. Allow narrative/site/quest content but reject duplicate ration, roster, route, territory, survivor, or save truth.
3. Provide validated authoring hooks for candidate sites and quests.
4. Add schema validation for forbidden authority-owned fields.
5. Create at least one content integration fixture establishing/using a post through canonical APIs.
6. Mark Plan 160 as a consumer/extension in register metadata.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

## 58D.2 — Release gate and rollback

**Goal:** Ship only the smallest slice that satisfies architecture, runtime, save, retention, and player-value evidence.

### Required substeps

1. Run full .NET build/test and Godot integrity, bridge, expedition, and expansion selftests.
2. Run plan intake, save/triad drift, and fast verification gates.
3. Run lifecycle, supply, roster, ration, census, retention, continuation, isolation, and no-bonus suites.
4. Capture metrics for establishment funnel, supply reliability, food/dose pressure, obligations, site loss, and continuation memory.
5. Define a feature disable path that stops new establishment/continuation creation while preserving safe save reads.
6. Write completion evidence and residual blockers before DONE.

### Integration contract

- Every durable fact touched by this work package has exactly one authoritative owner.
- References cross subsystem boundaries; copied foreign state does not.
- Derived state is recomputed from canonical owners unless persistence is explicitly justified.
- Save/load cannot create a second source of truth or repeat a daily/event effect.
- UI reads and mutations resolve through the same underlying authority model.
- Failure is explicit: unresolved IDs, blocked routes, missing rails, and rejected commands surface as errors/states rather than being silently repaired.
- The work package must not expand map/system/UI scope beyond the ADR without a new intake decision.

### Required negative probes

- Authority-owned value is duplicated locally and the two copies disagree.
- Referenced route/node/survivor/content ID is missing after load.
- Save/load occurs exactly across a day tick or lifecycle transition.
- UI-local optimistic state disagrees with authoritative command result.
- A route/control/weather change invalidates a previously cached assumption.
- A transfer operation duplicates or destroys inventory unexpectedly.
- A blocked prerequisite tempts implementation to invent a local substitute.

### Evidence before merge

- [ ] Targeted tests cover success and failure paths.
- [ ] Save round-trip coverage exists for persistent changes.
- [ ] Runtime/headless evidence exists for player-facing wiring.
- [ ] Register/intake metadata is current at merge HEAD.
- [ ] Documentation points to canonical ownership rather than duplicating rules.
- [ ] The declared player/system metric has a baseline and post-change result where measurable.

---

# 5. Canonical Authority Matrix

| Fact | Expected authority | Outpost persistence rule |
|---|---|---|
| site identity/lifecycle | campaign outpost section | persist only site-owned identity/lifecycle |
| graph node | graph authority | store node ID only |
| route state | route authority | never copy; query |
| weather | weather authority | never copy; query |
| dose/exposure | exposure/location authority | never copy; query |
| territorial control | faction/world authority | never copy; query |
| treaty legality | treaty authority | never copy; query |
| survivor identity | survivor aggregate | reference IDs only |
| survivor fitness/needs | fitness/needs authority | query |
| duty posting | roster authority | site ID as assignment target |
| bunks/capacity | WaystationSystem/capacity authority | extend/query existing semantics |
| food quantity | inventory/ration authority | no local ledger |
| storage | canonical inventory/storage | no local item database |
| caravan cargo | caravan + inventory authority | credit only on delivery |
| commitments | commitment authority | reference target site |
| census/site counts | census/register + placement | projection, not duplicate population |
| production | production authority | reference jobs/producer identity |
| knowledge/radio | reveal/intel/radio authority | query/derive |
| memorial/death | memorial/legacy authority | location context only |
| historical events | retention-governed log authority | bounded |
| continuation seed | new-campaign initializer | creation-time payload only |

A field with no owner does not ship.

---

# 6. Lifecycle Model

```text
CANDIDATE
  -> ESTABLISHING
       -> ACTIVE
            -> UNDER_SUPPLIED (prefer derived)
            -> ISOLATED (prefer derived)
            -> EVACUATING
            -> ABANDONED
            -> LOST
ABANDONED/LOST
  -> RUINED / REOCCUPATION_CANDIDATE
```

Lifecycle state is not permission to duplicate route, food, staff health, weather, or control state. State
transitions must be caused by authoritative commands/events and must resolve all downstream consequences.

---

# 7. First Playable Slice

The first slice is intentionally one post, not an outpost framework explosion:

1. one eligible graph node;
2. one construction bill;
3. one staffing rule;
4. one ration draw path;
5. one caravan supply path;
6. one route-closure starvation scenario;
7. one intel benefit;
8. one production/relay role;
9. one evacuation/loss path;
10. one existing waystation-network UI route;
11. one save round-trip;
12. one bounded legacy record that can later appear in continuation.

Do not add multiple archetypes before this loop proves player value.

---

# 8. Player Acceptance Scenarios

## Scenario A — Build and staff

The player discovers a valid node through normal knowledge progression, sees route eligibility, commits
canonical materials and labour, passes roster/fitness rules, waits the production duration, and receives an
active post. No resource or survivor state is duplicated.

## Scenario B — Isolation

The site has finite food. A route becomes unavailable due to canonical world state. A planned caravan does
not magically deliver. Staff continue consuming through the same ration/needs authority. The UI derives
remaining supply from real inventory. The player must reroute, evacuate, or accept consequences.

## Scenario C — Promise under pressure

A delivery commitment targets the post. Satisfiability was valid at acceptance. Later world changes isolate
the route. The commitment becomes at-risk and can fail honestly, producing existing grievance/standing
consequences rather than being silently cancelled.

## Scenario D — Loss

The player evacuates or loses the post. Survivor placement, deaths/missing outcomes, inventory transfer,
production shutdown, census totals, memorials, grievance, and legacy records all resolve through canonical
authorities. No phantom staff or producer remains active.

## Scenario E — Continued world

The prior campaign ends with a lost site, memorial, treaty/standing condition, and obligation. The next
campaign is created from a bounded deterministic seed. It starts with ordinary mechanical power but a world
that remembers. Removing the old save after creation does not affect the new campaign.

## Scenario F — Clean start

A clean campaign uses the same new-campaign composition path with an empty/default legacy seed. It is fully
supported, narratively complete, and not mechanically inferior.

---

# 9. Continuation Boundary

```text
Previous campaign envelope
        |
        | one-time read during creation
        v
Bounded LegacyLedger records
        |
        | deterministic derivation
        v
ContinuationSeed vN
        |
        | ordinary new-campaign composition
        v
New campaign envelope
        X  no live dependency backward
Previous campaign envelope
```

Permitted by default: bounded ending facts, site history, memorial/death records, approved faction/treaty/
standing start context, inherited obligations, and provenance needed for remembered-world presentation.

Prohibited by default: inventory, raw resources, survivor stats, weapons/equipment, recipes/unlocks, production
multipliers, generator upgrades, permanent buffs, and direct copies of active previous-campaign runtime state.

---

# 10. Verification Matrix

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/plan-intake-check.sh plan_58
godot --headless --path . -- --expedition-selftest
godot --headless --path . -- --expansions-selftest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Targeted suites should include:

- OutpostAuthorityOwnershipTests
- OutpostLifecycleTests
- OutpostSaveRoundTripTests
- OutpostRosterTests
- OutpostRationTests
- OutpostSupplyLineTests
- OutpostCommitmentSatisfiabilityTests
- OutpostCensusTests
- OutpostRetentionTests
- WaystationNetworkAuthorityTests
- ContinuationSeedTests
- ContinuationIsolationTests
- ContinuationNoBonusTests
- ContinuationCleanStartParityTests
- ContinuationMemoryProjectionTests

---

# 11. Negative Invariant Catalogue

1. No persisted outpost weather copy.
2. No persisted outpost territory-owner copy.
3. No copied survivor health/needs state.
4. No second food ledger.
5. No survivor in mutually exclusive simultaneous postings.
6. No inventory increase during transfer.
7. No destination credit before caravan arrival.
8. No stale local reachability after route closure.
9. No known-unsatisfiable commitment accepted.
10. No dead/missing survivor retained as active site staff.
11. No lost/abandoned site continues producing without an explicit reactivation transition.
12. No gameplay UI mutation without authoritative command.
13. No continued campaign read/write dependency on previous save after creation.
14. No legacy field enters prohibited stat/resource/recipe/multiplier calculations.
15. No retention collection exceeds its ceiling after soak.
16. No content schema duplicates authority-owned world or survivor facts.
17. No node count silently exceeds the ADR budget.

---

# 12. Metrics

Track at minimum:

- first outpost candidate discovery rate;
- first outpost establishment rate;
- time-to-establishment;
- route/dose/time savings attributable to a post;
- daily food pressure at site;
- supply delivery success rate;
- days isolated;
- commitments met/broken involving sites;
- remote-duty injury/death rate;
- outposts abandoned/lost/reoccupied;
- waystation-network action usage;
- authority-verified interactive actions / total interactive actions;
- save section growth per 100 site-days;
- retention ceilings exceeded (target 0);
- continuation seed derivation success;
- clean-vs-continued hard-power parity;
- time-to-first-recognized-memory;
- seeded memories actually surfaced;
- live previous-save dependencies (target 0).

---

# 13. Rollback Plan

## Outpost feature rollback

- Disable new establishment while retaining safe reads of existing section state.
- Preserve evacuation/closure paths where necessary to avoid trapping saves.
- Disable optional intel/production effects independently if they regress.
- Never delete the outpost section as a UI/content rollback.

## Continuation rollback

- Disable creation of new continued campaigns if seed derivation is unsafe.
- Keep clean start available.
- Already-created continued campaigns remain independent normal campaign saves.
- Never restore a live dependency on previous saves as a workaround.

---

# 14. Plan 160 / Content Contract

Content may define site narrative identity, candidate site records that resolve to approved nodes, quest text,
scenario-specific construction requirements within canonical schemas, narrative consequences, encounter hooks,
and presentation metadata.

Content must not own food balances, survivor stats, route openness, territory control, weather, dose, roster
truth, campaign save roots, inventory engines, commitment engines, or radio/knowledge systems.

---

# 15. Completion Checklist

- [ ] Intake passed at current HEAD or the plan is explicitly blocked.
- [ ] ADR defines the feature as projection of existing authorities.
- [ ] Every proposed field has an owner.
- [ ] One campaign envelope remains authoritative.
- [ ] One ration/inventory authority remains authoritative.
- [ ] One roster/survivor authority remains authoritative.
- [ ] Graph/node budget is explicit and enforced.
- [ ] Establishment uses materials + labour + duration rails.
- [ ] Staffing uses fitness/policy/relations rails.
- [ ] Supply credits only on canonical delivery.
- [ ] Route closure can honestly isolate/starve the site.
- [ ] Intel value uses existing radio/knowledge rails.
- [ ] Commitment satisfiability is enforced.
- [ ] Census has no shadow population.
- [ ] One production slice uses canonical producer/storage contracts.
- [ ] Loss resolves staff, inventory, social, memorial, and legacy consequences.
- [ ] Existing waystation-network UI is extended with authoritative actions.
- [ ] 200-site-day retention soak passes.
- [ ] Continuation is distinct from succession and NG+.
- [ ] Legacy ledger remains records-only.
- [ ] No mechanical inheritance exists without explicit exception.
- [ ] Continued campaign is isolated from previous save after initialization.
- [ ] Clean start is first-class.
- [ ] Cross-campaign replay is deterministic.
- [ ] Returning-player memory is perceptible.
- [ ] Plan 160/content consumes rails rather than duplicating them.
- [ ] `E1_planintegration[3].md` is the next sequence filename.

---

# 16. Final Directive

Outposts should make the same campaign stretch farther, not create a second game inside the game. Their
strategic value comes from forcing existing food, labour, route, information, obligation, relationship, and
world-control systems to operate across distance. Continuation should make the next campaign inherit evidence
and responsibility, not power.

The release criterion is therefore architectural as much as experiential: **one authority per fact, one active
campaign envelope, one bounded records-only bridge between campaigns, and no unapproved mechanical inheritance.**


---

# 17. Per-Package Execution Worksheets

## Worksheet — 58A.1: Intake and premise refresh

### Before code

- [ ] Record current HEAD and premise-verification result.
- [ ] Confirm required rails are at the declared readiness level.
- [ ] List authoritative systems touched and explicitly list facts this package must not own.
- [ ] Identify save section(s) and migration impact.
- [ ] Create a failing baseline or reproducible disconnected scenario.
- [ ] Write the rollback/disable behavior.

### Reviewer questions

- Is this change extending an authority or accidentally creating a peer source of truth?
- Could reload cause this package and another system to disagree?
- Is any derived value persisted only because querying feels inconvenient?
- Does every cross-domain reference have missing-ID behavior?
- Does the UI use authority-provided state and rejection reasons?
- Does this package expand the ADR-approved map/system scope?
- What player-facing pressure or option becomes meaningfully different?
- What negative test would fail if ownership drifts later?

### Before merge

- [ ] Targeted tests pass.
- [ ] Save/load tests pass where relevant.
- [ ] Runtime/selftest evidence is captured.
- [ ] No new unowned field or duplicated authority exists.
- [ ] Plan register/intake metadata is updated.
- [ ] Documentation identifies canonical ownership.
- [ ] Metric delta or acceptance evidence is recorded.

## Worksheet — 58A.2: ADR: what an outpost is

### Before code

- [ ] Record current HEAD and premise-verification result.
- [ ] Confirm required rails are at the declared readiness level.
- [ ] List authoritative systems touched and explicitly list facts this package must not own.
- [ ] Identify save section(s) and migration impact.
- [ ] Create a failing baseline or reproducible disconnected scenario.
- [ ] Write the rollback/disable behavior.

### Reviewer questions

- Is this change extending an authority or accidentally creating a peer source of truth?
- Could reload cause this package and another system to disagree?
- Is any derived value persisted only because querying feels inconvenient?
- Does every cross-domain reference have missing-ID behavior?
- Does the UI use authority-provided state and rejection reasons?
- Does this package expand the ADR-approved map/system scope?
- What player-facing pressure or option becomes meaningfully different?
- What negative test would fail if ownership drifts later?

### Before merge

- [ ] Targeted tests pass.
- [ ] Save/load tests pass where relevant.
- [ ] Runtime/selftest evidence is captured.
- [ ] No new unowned field or duplicated authority exists.
- [ ] Plan register/intake metadata is updated.
- [ ] Documentation identifies canonical ownership.
- [ ] Metric delta or acceptance evidence is recorded.

## Worksheet — 58A.3: Authority ownership matrix

### Before code

- [ ] Record current HEAD and premise-verification result.
- [ ] Confirm required rails are at the declared readiness level.
- [ ] List authoritative systems touched and explicitly list facts this package must not own.
- [ ] Identify save section(s) and migration impact.
- [ ] Create a failing baseline or reproducible disconnected scenario.
- [ ] Write the rollback/disable behavior.

### Reviewer questions

- Is this change extending an authority or accidentally creating a peer source of truth?
- Could reload cause this package and another system to disagree?
- Is any derived value persisted only because querying feels inconvenient?
- Does every cross-domain reference have missing-ID behavior?
- Does the UI use authority-provided state and rejection reasons?
- Does this package expand the ADR-approved map/system scope?
- What player-facing pressure or option becomes meaningfully different?
- What negative test would fail if ownership drifts later?

### Before merge

- [ ] Targeted tests pass.
- [ ] Save/load tests pass where relevant.
- [ ] Runtime/selftest evidence is captured.
- [ ] No new unowned field or duplicated authority exists.
- [ ] Plan register/intake metadata is updated.
- [ ] Documentation identifies canonical ownership.
- [ ] Metric delta or acceptance evidence is recorded.

## Worksheet — 58A.4: Campaign save contract

### Before code

- [ ] Record current HEAD and premise-verification result.
- [ ] Confirm required rails are at the declared readiness level.
- [ ] List authoritative systems touched and explicitly list facts this package must not own.
- [ ] Identify save section(s) and migration impact.
- [ ] Create a failing baseline or reproducible disconnected scenario.
- [ ] Write the rollback/disable behavior.

### Reviewer questions

- Is this change extending an authority or accidentally creating a peer source of truth?
- Could reload cause this package and another system to disagree?
- Is any derived value persisted only because querying feels inconvenient?
- Does every cross-domain reference have missing-ID behavior?
- Does the UI use authority-provided state and rejection reasons?
- Does this package expand the ADR-approved map/system scope?
- What player-facing pressure or option becomes meaningfully different?
- What negative test would fail if ownership drifts later?

### Before merge

- [ ] Targeted tests pass.
- [ ] Save/load tests pass where relevant.
- [ ] Runtime/selftest evidence is captured.
- [ ] No new unowned field or duplicated authority exists.
- [ ] Plan register/intake metadata is updated.
- [ ] Documentation identifies canonical ownership.
- [ ] Metric delta or acceptance evidence is recorded.

## Worksheet — 58A.5: Reachability and node-budget gate

### Before code

- [ ] Record current HEAD and premise-verification result.
- [ ] Confirm required rails are at the declared readiness level.
- [ ] List authoritative systems touched and explicitly list facts this package must not own.
- [ ] Identify save section(s) and migration impact.
- [ ] Create a failing baseline or reproducible disconnected scenario.
- [ ] Write the rollback/disable behavior.

### Reviewer questions

- Is this change extending an authority or accidentally creating a peer source of truth?
- Could reload cause this package and another system to disagree?
- Is any derived value persisted only because querying feels inconvenient?
- Does every cross-domain reference have missing-ID behavior?
- Does the UI use authority-provided state and rejection reasons?
- Does this package expand the ADR-approved map/system scope?
- What player-facing pressure or option becomes meaningfully different?
- What negative test would fail if ownership drifts later?

### Before merge

- [ ] Targeted tests pass.
- [ ] Save/load tests pass where relevant.
- [ ] Runtime/selftest evidence is captured.
- [ ] No new unowned field or duplicated authority exists.
- [ ] Plan register/intake metadata is updated.
- [ ] Documentation identifies canonical ownership.
- [ ] Metric delta or acceptance evidence is recorded.

## Worksheet — 58B.1: Establishment production triple

### Before code

- [ ] Record current HEAD and premise-verification result.
- [ ] Confirm required rails are at the declared readiness level.
- [ ] List authoritative systems touched and explicitly list facts this package must not own.
- [ ] Identify save section(s) and migration impact.
- [ ] Create a failing baseline or reproducible disconnected scenario.
- [ ] Write the rollback/disable behavior.

### Reviewer questions

- Is this change extending an authority or accidentally creating a peer source of truth?
- Could reload cause this package and another system to disagree?
- Is any derived value persisted only because querying feels inconvenient?
- Does every cross-domain reference have missing-ID behavior?
- Does the UI use authority-provided state and rejection reasons?
- Does this package expand the ADR-approved map/system scope?
- What player-facing pressure or option becomes meaningfully different?
- What negative test would fail if ownership drifts later?

### Before merge

- [ ] Targeted tests pass.
- [ ] Save/load tests pass where relevant.
- [ ] Runtime/selftest evidence is captured.
- [ ] No new unowned field or duplicated authority exists.
- [ ] Plan register/intake metadata is updated.
- [ ] Documentation identifies canonical ownership.
- [ ] Metric delta or acceptance evidence is recorded.

## Worksheet — 58B.2: Staffing and duty assignments

### Before code

- [ ] Record current HEAD and premise-verification result.
- [ ] Confirm required rails are at the declared readiness level.
- [ ] List authoritative systems touched and explicitly list facts this package must not own.
- [ ] Identify save section(s) and migration impact.
- [ ] Create a failing baseline or reproducible disconnected scenario.
- [ ] Write the rollback/disable behavior.

### Reviewer questions

- Is this change extending an authority or accidentally creating a peer source of truth?
- Could reload cause this package and another system to disagree?
- Is any derived value persisted only because querying feels inconvenient?
- Does every cross-domain reference have missing-ID behavior?
- Does the UI use authority-provided state and rejection reasons?
- Does this package expand the ADR-approved map/system scope?
- What player-facing pressure or option becomes meaningfully different?
- What negative test would fail if ownership drifts later?

### Before merge

- [ ] Targeted tests pass.
- [ ] Save/load tests pass where relevant.
- [ ] Runtime/selftest evidence is captured.
- [ ] No new unowned field or duplicated authority exists.
- [ ] Plan register/intake metadata is updated.
- [ ] Documentation identifies canonical ownership.
- [ ] Metric delta or acceptance evidence is recorded.

## Worksheet — 58B.3: Ration and food integration

### Before code

- [ ] Record current HEAD and premise-verification result.
- [ ] Confirm required rails are at the declared readiness level.
- [ ] List authoritative systems touched and explicitly list facts this package must not own.
- [ ] Identify save section(s) and migration impact.
- [ ] Create a failing baseline or reproducible disconnected scenario.
- [ ] Write the rollback/disable behavior.

### Reviewer questions

- Is this change extending an authority or accidentally creating a peer source of truth?
- Could reload cause this package and another system to disagree?
- Is any derived value persisted only because querying feels inconvenient?
- Does every cross-domain reference have missing-ID behavior?
- Does the UI use authority-provided state and rejection reasons?
- Does this package expand the ADR-approved map/system scope?
- What player-facing pressure or option becomes meaningfully different?
- What negative test would fail if ownership drifts later?

### Before merge

- [ ] Targeted tests pass.
- [ ] Save/load tests pass where relevant.
- [ ] Runtime/selftest evidence is captured.
- [ ] No new unowned field or duplicated authority exists.
- [ ] Plan register/intake metadata is updated.
- [ ] Documentation identifies canonical ownership.
- [ ] Metric delta or acceptance evidence is recorded.


<!-- Worksheet appendix capped to keep the flagship file inside the requested 50–90k character range. -->
