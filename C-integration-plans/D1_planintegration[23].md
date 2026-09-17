# D1 Flagship Integration Plan [23]
## Plan 217 — Survivor Genealogy & Family Tree System

> **Canonical filename:** `D1_planintegration[23].md`
>
> **Previous:** `D1_planintegration[22].md`
>
> **Next:** `D1_planintegration[24].md`
>
> **Purpose:** Wire the existing `GenerationalLineageExtension` into the live host/save lifecycle and evolve it
> into one coherent genealogy domain that can answer biological/adoptive parentage, siblinghood, ancestry,
> descendants, spouses/partners, family units, family events, inherited traits, and generational depth while
> remaining compatible with the already-wired `GenerationalSuccessionEngine` and Century Seed UI.
>
> **Primary source:** Plan 217 — Survivor Genealogy & Family Tree System.
>
> **Core repository problem:** ASHFALL already has two partial generational systems. `GenerationalLineageExtension`
> owns basic parent-child `LineageRecord` links and state capture but is not wired into `src/`.
> `GenerationalSuccessionEngine` is already wired through `ExpansionHostSession` and surfaced in
> `CenturySeedPanel`, but it focuses on generation indices, age progression, retirement, mentor/apprentice
> succession, and inherited traits rather than actual genealogical structure. The missing work is not to invent
> a third lineage engine; it is to connect, reconcile, and extend the existing pair into one visible genealogy
> layer.
>
> **Implementation posture:** graph-based, event-driven, derived-query heavy, sparse, deterministic, save-safe,
> backward compatible, host-wired, and explicit about relationship semantics. Biological parentage, adoption,
> spouses, family-unit membership, and succession are distinct relationship/event concepts rather than fields
> collapsed into one mutable survivor record.
>
> **Critical guardrail:** do not store data that can be derived cheaply and deterministically from canonical
> relationships. Sibling IDs, ancestor chains, descendant chains, lineage depth, generation labels, and many
> family-unit summaries should be query results/caches rather than duplicated save authority.
---

## 1. Source Problem Statement

The source establishes the current baseline:

- `GenerationalLineageExtension.cs` exists in Core and already supports `EstablishLineage`, `PerformSuccession`,
  `GetLineage`, `GetParent`, a `LineageRecord` with parent/child/relationship/day/active/inherited-trait fields,
  and `CaptureState/RestoreState`;
- `GenerationalSuccessionEngine.cs` exists in `Ashfall.Core.Legacy`;
- `ExpansionHostSession.cs` already owns/constructs the succession engine;
- `CenturySeedPanel.cs` already binds succession-engine data;
- grep in `src/` finds no host wiring for `GenerationalLineageExtension`;
- sibling detection, spouse/partner tracking, family units, naming, family events, ancestry/descendant traversal,
  full lineage depth, family history, genealogy UI, and kinship-affinity integration are absent.

The target architecture is:

```text
Cohort / birth events
Romance / marriage events
Adoption events
SurvivorFate / death events
Succession engine events
Trait inheritance facts
        ↓
     GenealogyBridge
        ↓
GenerationalLineageExtension
  ├─ parent/adoption graph
  ├─ spouse/partner links
  ├─ family-unit membership
  ├─ family event history
  └─ inherited-trait references
        ↓
 derived genealogy queries
  ├─ siblings
  ├─ ancestors / descendants
  ├─ lineage depth
  ├─ generation groups
  └─ kinship classification
        ↓
 read-only adapters
  ├─ SurvivorRelations
  ├─ CenturySeedPanel
  ├─ GenealogyPanel
  ├─ quest / achievement
  ├─ inheritance
  └─ archive / epilogue
```

`GenerationalSuccessionEngine` remains the Century Seed progression/age/succession authority. The lineage
extension becomes the genealogical relationship authority.
---

## 2. Flagship Success Criteria

Implementation is complete only when all of the following are true:

1. `GenerationalLineageExtension` is constructed exactly once in host composition.
2. It is restored/saved through the same lifecycle as the existing succession engine.
3. Host wiring does not create a second independent extension instance for UI or bridge code.
4. `GenealogyBridge.cs` subscribes to canonical survivor lifecycle/relationship events.
5. Birth creates parent-child edges exactly once.
6. Adoption creates adoptive parent-child edges without erasing biological lineage.
7. Marriage/partnership creates a relationship link without pretending spouses are parent-child lineage.
8. Divorce/separation closes the spouse link historically rather than deleting all evidence.
9. Sibling relationships are derived from shared parentage.
10. Half-siblings and adoptive siblings have explicit query semantics.
11. Ancestor traversal supports multiple parents and detects cycles defensively.
12. Descendant traversal supports large multi-generation trees.
13. Lineage depth is derived, not blindly persisted.
14. Generation labels reconcile with the existing succession-engine generation index where both exist.
15. No family relationship is fabricated merely from current household proximity.
16. Family units are social/grouping constructs and do not replace lineage edges.
17. Family units have stable IDs.
18. Family-unit membership changes are evented and save-safe.
19. Family naming is deterministic when generated.
20. Player-facing family names use localized/data-backed templates.
21. Family-name inheritance policy is explicit and not hardcoded to one parent's surname.
22. Birth/death/marriage/divorce/adoption/reunion/schism/milestone/succession events have stable IDs.
23. Family events are not duplicated by multiple upstream event consumers.
24. Death retains the survivor in the genealogy graph.
25. Extinct family units remain historical rather than disappearing.
26. SurvivorFate remains death authority.
27. CohortSystem remains birth/maturation authority.
28. Romance/Relations system remains marriage/divorce/relationship authority.
29. TraitSystem remains trait authority.
30. Kinship-affinity effects are supplied through a typed relation modifier adapter, not by directly rewriting
    relationship scores on every tick.
31. Parent-child/sibling/spouse/grandparent modifiers are data-backed and bounded.
32. Kinship bonuses do not stack multiple times through multiple graph paths unless explicitly designed.
33. Divorce/schism penalties are event/context effects rather than a permanent universal kinship truth.
34. Old saves restore the original lineage graph unchanged.
35. New family-unit/event collections default cleanly on old saves.
36. Old saves do not fabricate marriages, names, or historical birth events that were never stored.
37. UI can visualize a large family without loading the entire genealogy into one unbounded scene tree.
38. Tree layout handles deceased, adopted, divorced, unknown-parent, and disconnected-family cases.
39. Headless simulation produces the same graph and event ordering as UI play.
40. `--genealogy-selftest` validates host wiring, birth/death/marriage/adoption, siblings, ancestry, descendants,
    depth, family units, naming, inherited traits, save/load, migration, cycle defenses, and determinism.
---

## 3. Repository Reconnaissance Before Editing

Create:

`docs/genealogy/GENEALOGY_INTEGRATION_AUDIT.md`

Inspect at minimum:

- `Assets/Ashfall.Core/GenerationalLineageExtension.cs`
- `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs`
- tests for both systems
- `src/Host/ExpansionHostSession.cs`
- expansion tick/save lifecycle
- `src/UI/CenturySeedPanel.cs`
- `Assets/Ashfall.Core/CohortSystem.cs`
- child birth/maturation events
- `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs`
- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs`
- romance/family system from Plan 150 if implemented
- adoption/family formation state if any
- TraitSystem / inherited trait data
- survivor identity registry
- survivor save schema
- archive/epilogue systems
- inheritance system from Plan 206 if implemented
- event bus / stable event IDs
- `ISeededRng`
- localization/data loader
- Godot tree/graph UI capabilities

Build an authority matrix:

| Fact | Canonical owner | Genealogy role |
|---|---|---|
| survivor exists/alive | survivor/fate | reference + historical display |
| birth | Cohort | create lineage edge/event |
| biological parentage | lineage extension | own edge |
| adoption | lineage extension via canonical adoption event | own adoptive edge |
| marriage/divorce | romance/relations | mirror relationship link/history |
| inherited trait existence | TraitSystem/succession | reference/display |
| generation progression | succession engine | reconcile/display |
| family unit grouping | lineage extension | own social group |
| relationship score | SurvivorRelations | consume kinship modifier |
| inheritance transfer | Plan 206 | query genealogy |
| UI layout | Godot UI | projection only |

The audit must explicitly identify whether current `LineageRecord` is one-record-per-child, one-record-per-parent
edge, or another shape. That determines how multi-parent support can be added safely.
---

## 4. Scope Boundary

### In scope

- host wiring for existing lineage extension;
- relationship graph model;
- biological/adoptive parentage;
- spouse/partner relationship history;
- sibling/ancestor/descendant queries;
- lineage depth;
- family units;
- family naming;
- family events;
- inherited trait display/reference;
- kinship-affinity adapter;
- Century Seed UI integration;
- standalone genealogy UI;
- persistence/migration;
- CI/selftests.

### Explicitly out of scope

- replacing RomanceSystem;
- replacing CohortSystem;
- replacing SurvivorFateSystem;
- replacing TraitSystem;
- replacing GenerationalSuccessionEngine;
- realistic genetics simulation;
- paternity uncertainty;
- legal custody;
- inheritance execution;
- eugenics/trait optimization mechanics;
- inter-settlement arranged-marriage simulation in v1;
- arbitrary player rewriting of canonical family history;
- cross-campaign family persistence.

The system records and queries family structure; it does not become every family-related mechanic.

---
## 5. Host Wiring Strategy
- Add one `GenerationalLineageExtension` property to `ExpansionHostSession` or the canonical composition root that already owns the succession engine.
- Construct it once, inject the existing `GenerationalSuccessionEngine` only if a real dependency is required, and pass the same instance to bridge/UI adapters.
- Do not create one extension inside `CenturySeedPanel` and another in survivor setup.
- Architecture test should verify a single composition instance.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 6. Tick Requirement Audit
- The source requests `TickLineage(day)`, but genealogy is fundamentally event-driven.
- Do not add a meaningless daily tick if the extension only responds to births/deaths/relationships.
- Use a low-frequency maintenance tick only for expiry/reconciliation tasks that genuinely need time.
- Host wiring should prefer events over polling.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 7. Save Flow Integration
- Persist lineage state alongside the existing `GenerationalSuccessionSaveState`.
- Do not merge their schemas unless there is a compelling migration reason.
- One save container can carry both independently versioned sub-states.
- Restore succession and lineage before UI and derived relation caches.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 8. State Versioning
- Give lineage state its own schema version if it does not already have one.
- New collections default safely.
- Stable survivor IDs and relationship/event IDs are save contracts.
- Do not key family relationships by display name.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 9. Lineage Record Shape
- Audit whether `LineageRecord` should remain a relationship edge rather than a survivor aggregate.
- Recommended: each record represents one parent-child relationship.
- This naturally supports two biological parents, adoptive parents, and future guardianship without overwriting one `parentId`.
- If current code assumes one parent per child, migrate carefully.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 10. Do Not Add Derived Sibling IDs to Saved Record
- The source explicitly notes `siblingIds` should be computed on query; preserve that.
- Do not serialize sibling lists.
- Sibling queries derive from shared active parent-child edges.
- This prevents divergence when new siblings/adoptions are added.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 11. Do Not Persist Lineage Depth Blindly
- The source proposes `generationDepth` on `LineageRecord`; treat it as a computed projection/cache.
- Depth depends on the whole graph and can change after migration/adoption/link correction.
- Persisting it risks stale data.
- Cache with graph revision only if profiling requires.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 12. Spouse Relationship Model
- Do not put a single `spouseId` on a parent-child edge.
- Spouse/partner relationship is a separate relationship record or family-event/relationship-state collection.
- This supports divorce, remarriage, widowhood, and multiple historical spouses.
- Current spouse query derives from active spouse links.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 13. Family Name Model
- Do not put one `familyName` on each lineage edge as sole authority.
- Family names belong to survivor identity/family-unit membership or a dedicated naming record.
- A survivor can retain birth family name while joining a new family unit depending policy.
- Keep name history if renamed.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 14. Relationship Edge Types
- Support at minimum biological_parent and adoptive_parent for lineage.
- Spouse/partner lives in a separate partner-link type.
- Mentor/apprentice from succession is not genealogical parentage.
- Do not treat succession mentorship as ancestry.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 15. Adoption Semantics
- Adoption adds an adoptive-parent edge.
- Biological ancestry remains queryable if known.
- Queries can request biological_only, adoptive_only, or inclusive family ancestry.
- UI uses distinct line style.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 16. Multiple Parents
- Support zero, one, two, or more parent edges where adoption/guardianship history warrants.
- Do not hardcode exactly two.
- Generation depth uses a documented rule when multiple parent paths exist.
- Cycle detection mandatory.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 17. Sibling Semantics
- Define full sibling as sharing at least the configured biological parent set; half sibling as sharing one biological parent; adoptive sibling as sharing an adoptive family/parent relationship depending query mode.
- Do not collapse every family-unit member into sibling.
- Expose typed `KinshipKind`.
- UI can simplify labels.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 18. Ancestor Query
- Traverse parent edges upward with visited set.
- Support maximum depth parameter.
- Return stable ordered results with relationship path metadata.
- Detect cycles and surface diagnostics rather than infinite recursion.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 19. Descendant Query
- Traverse child edges downward.
- Support depth limit and relationship-mode filter.
- Return path/degree information.
- Use iterative traversal for large families.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 20. Lineage Depth Definition
- Define whether depth is shortest path from any root, longest path, or generation index.
- Recommended genealogy depth: longest acyclic parent path from known root.
- Succession engine generationIndex may be displayed separately unless exact semantics match.
- Do not silently equate the two.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 21. Generation Reconciliation
- Create a `GenerationReconciliationReport` comparing genealogy-derived depth and `DwellerGenerationRecord.generationIndex` for survivors known to both systems.
- Document expected differences for founders/adoptions/imported survivors.
- Do not mutate one system just to force equality without semantics.
- Use reconciliation in CI.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 22. Cycle Defense
- Parent assignment must reject self-parentage and ancestor-as-child cycles.
- Adoption must also respect cycle rules.
- Restore/migration validates legacy graph and reports corruption.
- Queries never trust graph acyclicity blindly.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 23. Duplicate Edge Prevention
- Stable edge ID derived from parent/child/relationship/source event where possible.
- Same birth/adoption event cannot create duplicate edge.
- Biological and adoptive edges between same pair may coexist only if policy explicitly allows.
- Idempotent event handling.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 24. Edge Activity/History
- Biological/adoptive parentage usually remains historically true and should not become inactive on death.
- `isActive` should mean relationship applicability only if that concept is truly needed.
- Do not deactivate parentage because a parent dies.
- Partner links can become inactive on divorce/death.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 25. Genealogy Bridge
- Create `Assets/Ashfall.Core/Survivors/GenealogyBridge.cs` as a thin event adapter.
- It translates canonical birth/death/marriage/divorce/adoption/succession facts into lineage operations.
- It contains no UI and no independent family state.
- Every handler accepts stable source event ID.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 26. Birth Event Wiring
- Subscribe to CohortSystem's canonical child-birth event.
- Create parent-child edges for every known biological/adoptive parent supplied by that event.
- Assign/inherit family naming according to policy.
- Record one birth family event.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 27. Maturation Event Wiring
- Maturation should not create new lineage relationships.
- It can update family-unit generation summaries/projections if needed.
- Prefer derived current generation instead of mutable counters.
- No duplicate milestone unless an authored adulthood event is meaningful.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 28. Death Event Wiring
- SurvivorFateSystem emits death.
- Genealogy records one death event and leaves node/edges intact.
- Family-unit active/extinct status recalculates from surviving members.
- No genealogy-driven death state mutation.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 29. Marriage Event Wiring
- Marriage must originate from Romance/Relations/family system, not inferred from high affinity.
- Create active partner link and family event.
- Family-unit policy decides whether units merge/form/retain separate identities.
- Do not directly set relationship score.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 30. Divorce/Separation Wiring
- Canonical relationship system emits separation/divorce.
- Close partner link with end day/event.
- Record divorce/separation family event.
- Do not automatically split descendants into one parent's family unless policy says so.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 31. Adoption Event Wiring
- Adoption should come from a dedicated canonical adoption/family event.
- Create adoptive-parent edge and adoption event.
- Family-unit membership can change/expand.
- Do not erase biological links.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 32. Succession Wiring
- GenerationalSuccessionEngine succession/retirement event can create a family milestone/succession event if relevant.
- Do not treat mentorDwellerId as parent.
- Link shared inheritedTraitIds for display only.
- Century Seed progression remains owned by the engine.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 33. Retirement Event
- Retirement is a generational/social milestone, not a family relationship mutation.
- Record only if narratively useful.
- Do not dissolve family unit when patriarch/matriarch retires.
- Leadership/family-head fields update separately if used.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 34. Family Event DTO
- Use structured fields: eventId, eventTypeId, simulation time/day, participant IDs, familyUnitIds, sourceEventId, significanceId, fact refs, localization template ID.
- Do not persist free-text `description` as sole authority.
- Render localized text from structured facts.
- Stable IDs.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 35. Family Event Types
- Retain birth, death, marriage, divorce, adoption, reunion, schism, milestone, succession from source intent.
- Only emit reunion/schism when a real upstream system/event exists.
- Do not fabricate family drama randomly inside genealogy.
- Additional types can be registered later.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 36. Event Ordering
- Sort by simulation timestamp/day then stable event ID.
- Determinism must not depend on dictionary iteration.
- Multiple events same day remain consistently ordered.
- Save/load preserves ordering.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 37. Event Retention
- Family events are meaningful and relatively sparse; keep them long-term.
- If long campaigns create many minor milestones, allow significance-based compaction.
- Never drop births, deaths, marriages, divorces, adoptions, succession, major schisms.
- UI can filter.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 38. Family Unit Definition
- `FamilyUnit` is a social grouping, not the lineage graph itself.
- Fields can include unitId, currentName record/ref, founding day, member IDs or derived membership rules, status, origin event, optional head/elder roles.
- Do not make family-unit membership the only proof of kinship.
- One survivor may have historical membership in multiple units.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 39. Family Unit Membership History
- Prefer membership records with joined/left day and reason over a mutable member list if remarriage/schism matters historically.
- Current member list is derived.
- Source's `memberIds` can remain a convenience projection.
- This preserves history.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 40. Family Unit Formation
- Form only from explicit family/relationship event or founder setup.
- Do not automatically create a family unit for every parent-child edge unless desired.
- A parent/child may belong to a preexisting family.
- Stable unit ID.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 41. Family Unit Merge
- Marriage does not always require destructive merge.
- Policy options: retain both units, create household unit, merge one into another, or create new named unit.
- Choose a default compatible with ASHFALL's social design.
- Historical units remain.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 42. Family Unit Split
- Divorce/schism can create new unit or change membership.
- Do not arbitrarily assign all children.
- Use current household/custody/family policy only if such systems exist.
- Otherwise genealogy records relationship change while units remain mostly genealogical groupings.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 43. Extinction
- Family unit becomes extinct when no living/current members remain according to unit policy.
- Keep historical unit and event history.
- Do not delete.
- Archive/epilogue may consume extinct-family facts.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 44. Dissolution
- Dissolved means social unit ended even if descendants live.
- Distinct from extinct.
- Reason/event required.
- Can later be reactivated only via explicit reunion/reformation.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 45. Reunion
- Reunion should be a real event from separated family members/family system.
- May reactivate a dissolved family unit or create a new unit depending policy.
- Genealogy records it.
- Do not create relationship bonuses automatically beyond downstream relation rules.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 46. Schism
- Schism is a social family event, not a genealogical severing.
- Parentage remains true.
- Family-unit membership/relationship context may change.
- Penalty belongs to RelationsSystem through a typed event.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 47. Family Head Fields
- The source proposes patriarchId/matriarchId; these terms can be overly rigid and may not fit all families.
- Prefer optional `familyHeadIds` or `recognizedElderIds` if leadership within family is needed.
- If source-compatible fields are retained for save compatibility, treat them as optional presentation metadata.
- Do not hardcode gendered authority.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 48. Current Generation
- `currentGeneration` on FamilyUnit should be derived from living members/genealogy depth unless there is a specific authored unit generation concept.
- Do not manually increment and risk drift.
- Cache with graph revision if necessary.
- UI can show oldest/youngest/deepest living generation.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 49. Family Naming Authority
- Use a dedicated naming policy/service inside genealogy or shared identity system.
- Generated family names are data-backed and deterministic.
- Do not mutate survivor display names unless survivor naming rules explicitly support surnames.
- Family-unit name and survivor surname can be separate.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 50. Name Template Catalog
- Create `Assets/StreamingAssets/Data/family_name_templates.json` with schema version.
- Source asks for 30+ fictional/post-nuclear names; ensure they follow ASHFALL naming rules and are localization-friendly.
- Do not use real-world national/ethnic assumptions.
- Validate uniqueness/collision policy.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 51. Deterministic Name Generation
- Seed from campaign seed + family unit ID + naming event ID.
- Do not pass a mutable random stream whose call ordering could change names.
- Persist selected template/name after assignment.
- Reload cannot reroll.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 52. Family Name Inheritance
- Do not hardcode 'inherit parent's family name' to one parent.
- Define policy: keep current unit name, choose one partner's unit, hyphen/compound if supported, generated new name, or authored scenario.
- Player choice may be allowed if broader naming UI supports it; source says not player-chosen, but that is a design choice rather than exploit requirement.
- Determinism only applies when auto-generated.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 53. Name Collision
- Two unrelated family units may share a name.
- Use unit ID for identity.
- UI may append founding generation/location if disambiguation needed.
- Do not force globally unique surnames unless desired.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 54. Rename History
- If family unit can change name, record name-change event and prior name history.
- Do not rewrite historical event text.
- Current name projection uses latest active name record.
- Follow-on if renaming is not v1.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 55. Kinship Classification
- Create a `KinshipResolver` that returns parent, child, sibling_full, sibling_half, sibling_adoptive, spouse, former_spouse, grandparent, grandchild, aunt_uncle, niece_nephew, cousin, etc. only as far as needed.
- V1 need not expose every cousin degree.
- Use graph traversal/path rules.
- Relations consumes typed degree.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 56. Kinship Affinity Adapter
- Do not write +20/+15/+25 directly into base relationship score on every tick.
- Expose an `IRelationshipContextModifierProvider` or existing equivalent.
- SurvivorRelations computes effective affinity from base relationship + kinship context.
- Kinship modifier is data-backed.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 57. Source Kinship Values
- Source proposes parent-child +20, sibling +15, spouse +25, grandparent +10, divorced ex-spouse -10, schism -15.
- Treat these as initial tuning candidates, not immutable truths.
- Spouse affinity may already be represented by Romance/Relations and must not double-count.
- Audit before applying.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 58. No Double-Counting Romance
- If marriage already results from high bond/romance and relations already maintain spouse effects, do not add another +25 blindly.
- Kinship adapter should only fill missing contextual modifier.
- Create baseline regression.
- Document exact relationship formula.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 59. Parent-Child Modifier
- Can supply a stable kinship context bonus if SurvivorRelations needs it.
- Do not force every parent-child relationship to be positive if actual relationship history is hostile.
- Kinship bonus should be modest enough that conflict can overcome it.
- Use context not permanent minimum score.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 60. Sibling Modifier
- Differentiate full/half/adoptive only if design benefits.
- Do not assume siblinghood guarantees friendship.
- Use bounded context bonus.
- Shared family unit alone is insufficient.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 61. Grandparent Modifier
- Derived through two parent edges.
- Apply once even if pedigree contains multiple paths.
- Do not stack +10 twice because of multiple equivalent ancestor paths.
- Use canonical kinship degree.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 62. Former Spouse Context
- Divorce is not inherently a permanent -10 in every case.
- Prefer a divorce-event relation effect owned by Relations/Conflict systems.
- Genealogy can expose former_spouse relation and divorce event.
- Relations decides current effect.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 63. Schism Context
- Likewise, schism event can emit a temporary/persistent relation effect through Plan 202/Relations.
- Do not bake -15 into genealogical truth forever.
- Family graph remains unchanged.
- Separate history from current social state.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 64. Trait Inheritance Integration
- `inheritedTraitIds` already exist in both lineage/succession data.
- Audit which system is canonical for inheritance selection.
- Genealogy displays/link provenance; TraitSystem owns actual trait state.
- Do not duplicate mutation/inheritance calculations.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 65. Trait Provenance
- For inherited traits, record source parent(s)/succession event only if upstream system provides provenance.
- Do not guess which parent supplied a trait from ID overlap.
- UI can show inherited trait without source if unknown.
- Future genetics plan can extend.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 66. Generational Succession Engine Boundary
- Succession engine handles chapter progression, age, retirement, mentor/apprentice transfer, and its own generation record.
- Genealogy does not reimplement chapter time progression.
- Lineage extension can query generation records for UI reconciliation.
- One engine remains wired exactly as before.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 67. Century Seed Compatibility
- Existing `CenturySeedPanel` behavior remains intact.
- Add a lineage tab/section rather than replacing engine display.
- Regression snapshots/build tests protect chapter/generation UI.
- New genealogy data is optional if extension state absent.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 68. Genealogy Panel Projection
- Create `GenealogyPanelModel` or equivalent read-only DTO.
- Projection contains selected family/unit, visible nodes, relationships, generations, filters, events, and summary stats.
- Core graph queries produce data; Godot owns layout.
- No genealogical mutation in UI.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 69. Tree Layout Strategy
- Use layered/generation-based graph layout.
- Parent-child edges define vertical generations.
- Partner links align within generation where possible.
- Adoption/unknown-parent links use distinct styling.
- Do not rely on recursive nested Controls for unbounded trees.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 70. Large Tree Virtualization
- For large shelters, render only visible generation window/subtree.
- Support focus-on-survivor and expand ancestors/descendants.
- Recycle node widgets where UI framework permits.
- Do not instantiate thousands of controls on panel open.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 71. Disconnected Trees
- Some survivors may have no known family.
- Some families may be disconnected due to founders/imported survivors.
- UI supports multiple roots/family units.
- Do not fabricate common ancestor.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 72. Unknown Parents
- Allow parentless/root survivors.
- Do not create 'Unknown Mother/Father' fake survivor nodes unless UI specifically uses placeholders.
- Depth begins from known roots.
- Queries remain valid.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 73. Deceased Nodes
- Keep deceased survivors visible with historical styling.
- Do not remove edges.
- Source suggests gray; use design-system colors and text labels, not color alone.
- Click still opens historical detail.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 74. Infant/Child Nodes
- Age/life-stage comes from Cohort/Aging authority.
- UI can style accordingly.
- Do not persist `isInfant` in genealogy.
- Text label accessible.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 75. Relationship Line Styling
- Parent-child solid, marriage/partner dashed, adoption dotted are acceptable initial conventions.
- Use legend and accessibility-friendly patterns.
- Former partner can use ended/gray style.
- Do not rely on color only.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 76. Zoom/Pan
- Use Godot control/container/canvas mechanism suitable for graph UI.
- Persist UI zoom only as local UI preference if desired, not game save authority.
- Provide reset/focus button.
- Keyboard/controller navigation required.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 77. Family Filters
- By family unit, generation/depth range, alive/deceased, relationship type, selected survivor ancestors/descendants.
- Filters affect projection/rendering only.
- Do not mutate graph.
- UI state can be session-local.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 78. Family Detail Panel
- Show family name/current unit, founding day, status, living/deceased counts, notable events, deepest lineage, selected heads/elders if used, and members.
- Do not duplicate tree itself unnecessarily.
- Use paged event history.
- Historical units remain browsable.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 79. Survivor Lineage Detail
- Show parents by relationship type, current/former partners, siblings, children, grandparents, family unit memberships, inherited traits, and notable events.
- Queries are derived on selection.
- Do not persist redundant lists.
- Unknown facts remain unknown.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 80. Lineage Trace
- Provide ancestor chain/tree and descendant tree modes.
- Depth limit defaults to a reasonable number with expand-more.
- Prevent cycles visually even if corrupt data exists.
- Show diagnostic placeholder in debug builds.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 81. Genealogy Summary
- Derived stats: active families, extinct families, living generations, deepest lineage, known founders, birth/death/marriage counts.
- Do not hardcode 'current generation' from one system without reconciliation.
- Stats are projection-only.
- Quest hooks use stable underlying facts.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 82. Family Event Log UI
- Chronological significant events with filters.
- Use localized templates.
- Click event focuses participants/tree.
- Do not render raw source-event IDs to player.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 83. Quest Hook — Patriarch/Matriarch Refactor
- Source quest title can remain content, but mechanical condition should be gender-neutral family-founder/elder criteria unless narrative intentionally specifies.
- Count 10+ current/historical members and three genealogy depths/generations using stable family unit.
- Do not rely on a patriarchId field.
- QuestSystem owns progress.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 84. Quest Hook — Genealogist
- `document 5 complete family trees` needs a definition of complete.
- Recommended: five active/historical family units with no unresolved missing links among known in-shelter descendants up to configured generation.
- Do not require unknown pre-campaign ancestry.
- Use catalog/runtime facts.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 85. Quest Hook — Matchmaker
- Be careful: arranging marriages may conflict with survivor autonomy.
- Count marriages the player materially facilitated only if RomanceSystem tracks player involvement.
- Otherwise use `witness/see five marriages` or another non-coercive metric.
- Genealogy only exposes marriage events.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 86. Quest Hook — Elder
- Use derived lineage depth/third descendant generation from one founder.
- Do not confuse 'third generation' with great-grandchildren unless exact depth semantics are documented.
- Source text says third generation/great-grandchildren inconsistently; fix before implementation.
- Quest condition should use stable degree.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 87. Quest Hook — Historian
- Count significant unique family events.
- Do not allow event-spam farming.
- Birth/death/marriage/etc. stable IDs.
- QuestSystem owns count.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 88. Quest Hook — Dynasty
- Active family unit for 200 days can use founding day + continuous active status.
- Dissolution pauses/fails per quest rules.
- Do not count extinct history as active.
- Stable unit ID.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 89. Quest Hook — Reunion
- Requires real schism/separation/reunion events from social system.
- Do not synthesize them to satisfy quest.
- Count three distinct returned members or one defined reunion event.
- Document.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 90. Quest Hook — Legacy
- Count three canonical succession events from GenerationalSuccessionEngine.
- Do not create genealogy-side duplicate succession progression.
- Stable event IDs.
- QuestSystem owns reward.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 91. Achievement Integration
- Plan 149 may observe multi-generation depth, long-lived family unit, major reunion, or succession history.
- Use derived facts and stable events.
- No UI-open/documentation farming.
- Genealogy does not own achievement state.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 92. Inheritance Integration
- Plan 206 can query descendants/kinship when selecting heirs.
- Genealogy provides relationship graph only.
- Inheritance policy decides eligible heirs, priority, legal/social rules.
- Do not transfer items here.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 93. Education Integration
- Plan 154 parent-child teaching bonus can query typed kinship relation.
- Education system owns teaching effect.
- Do not duplicate XP or schedule.
- Adoptive parent can qualify according to explicit policy.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 94. Child Development Integration
- Plan 183 can query parents/family unit for caregiving/support context.
- ChildDevelopment owns developmental mechanics.
- Genealogy supplies stable relationship references.
- No daily child-state mutation.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 95. Romance/Family Integration
- Plan 150 remains partner/marriage/family-formation authority.
- Genealogy mirrors canonical relationship events.
- Do not infer marriage from cohabitation or affinity.
- Remarriage supported.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 96. Death/Inheritance Integration
- SurvivorFate records death first.
- Genealogy records historical event.
- Inheritance then queries genealogy if needed.
- Ordering must avoid querying a graph that lost the deceased node.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 97. Shelter Archive Integration
- Archive may remember notable families, extinct lineages, famous founders, multigenerational service, and family schisms/reunions.
- Genealogy exports structured candidate facts.
- Archive owns curation.
- Routine births need not all become archive entries.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 98. Epilogue Integration
- Plan 145 can consume stable family facts: spouse/partner, children, descendants, family-unit status, inherited legacy.
- Do not generate epilogue prose inside genealogy.
- Unknown links remain unknown.
- Historical deceased relations can still matter.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 99. Internal Communication Integration
- Plan 211 may announce births, marriages, deaths, reunions, and major family milestones.
- Genealogy emits events; communication owns distribution/read state.
- Do not duplicate notice-board records.
- Not every family event must be broadcast.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 100. Seasonal Event Integration
- Plan 170 family anniversaries/reunions can query founding/marriage/birth facts.
- Seasonal system owns event scheduling.
- Genealogy exposes dates.
- No duplicate calendar.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 101. Genealogy Event Provenance
- Every event stores `sourceEventId` from Cohort/Fate/Romance/etc.
- One source event cannot create duplicate family events.
- Manual/derived milestones use deterministic synthetic IDs based on stable facts.
- Migration-created anchors are explicitly marked.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 102. Milestone Events
- Examples: first grandchild, third living generation, 10th member, centennial family anniversary if campaign supports it.
- Generate only on monotonic threshold crossing.
- Persist emitted milestone ID set or derive idempotently.
- Do not check every frame.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 103. No Random Family Drama
- Genealogy itself does not roll divorce, schism, reunion, adoption, or marriage.
- Those originate from the relevant social/family systems.
- `ISeededRng` is primarily for name generation unless another explicitly probabilistic derived milestone needs it.
- Keep family history truthful.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 104. Graph Revision
- Maintain a monotonic in-memory graph revision counter incremented on relationship/membership changes.
- Derived caches key to revision.
- Revision need not be persisted if rebuilt.
- Useful for UI query caching.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 105. Query Cache
- Cache expensive ancestor/descendant/sibling queries per survivor + mode + graph revision.
- Invalidate globally or selectively on graph mutation.
- Do not persist cache.
- Measure before over-optimizing.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 106. Ancestor Performance
- Use iterative DFS/BFS with visited sets.
- Limit optional query depth.
- For UI, fetch focused subtree rather than full graph.
- Large multigenerational campaigns remain responsive.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 107. Descendant Performance
- Maintain parent→children adjacency index.
- Do not scan all lineage records for every recursive step.
- Build index on restore.
- Index is derived.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 108. Sibling Performance
- Maintain child→parents and parent→children indexes.
- Sibling query unions children of each parent then classifies overlap.
- Do not persist sibling lists.
- O(family size) not O(all survivors) where practical.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 109. Family Unit Lookup Performance
- Maintain current survivor→unit membership index if unit rules permit one current unit.
- If multiple units, map to small set.
- Persist membership records, derive lookup.
- UI fast.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 110. Data Integrity — Relationship Graph
- Validate all survivor IDs, no self-parent, no cycles, no duplicate edges, valid relationship types, valid event IDs, and consistent partner-link endpoints.
- Missing deceased archived survivor references follow survivor archive compatibility policy.
- Fail base-game corrupted references.
- Mod removal can render historical placeholders.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 111. Data Integrity — Family Units
- Unique unit IDs, valid names/template refs, valid membership intervals, no impossible date ordering, valid status transitions, founder day not after dissolution.
- Current membership derives consistently.
- Extinct status matches living members if auto-derived.
- CI report.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 112. Data Integrity — Family Names
- Template schemaVersion valid.
- 30+ source-target names only after quality review.
- No duplicate IDs.
- No prohibited real-person references under ASHFALL naming rules.
- Localization keys valid if names are localized tokens.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 113. Data Integrity — Event Types
- Registered event types and significance values only.
- Participants exist/archived.
- Source event IDs unique under intended scope.
- No duplicate marriage/birth/death event for same root source.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 114. Old Save Compatibility
- Existing `LineageState` restores original records exactly.
- New familyUnits/familyEvents/partnerLinks/name history default empty.
- Succession save state unchanged.
- Do not fabricate past marriages/family units from relations unless exact historical facts exist.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 115. Selective Migration
- If existing lineage records already encode parent-child links, derive siblings/ancestors/depth immediately without writing migration records.
- If succession engine has inheritedTraitIds, expose for display but do not synthesize parentage.
- If current relationship system stores current spouse but no history, optionally import a current-at-migration partner link marked provenance `legacy_current_relation`; do not invent marriage day.
- Only do this if source facts are trustworthy.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 116. Migration Family Units
- Do not automatically group every current survivor into generated family units from shared surnames/display names.
- Where parent-child clusters are known, family units may be generated only if design requires and with migration provenance.
- Safer default is no units until future family formation/naming events.
- Document.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 117. Migration Events
- Do not generate historical birth/death events for every preexisting lineage edge.
- Those dates may be unknown or unreliable.
- Current graph is enough.
- Future events are recorded normally.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 118. Migration Notification Suppression
- No genealogy notifications/tutorial due solely to restore/migration.
- Century Seed existing UI remains usable.
- First future lineage/family event can trigger tutorial.
- Diagnostics only.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 119. Save Ordering
- Capture succession state and lineage state from the same simulation boundary.
- Ensure birth/death event processing completes before save snapshot.
- Use stable event queues if save occurs mid-dispatch.
- No one state one day ahead of the other.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 120. Restore Ordering
- Restore survivor registry/cohort/fate/traits, succession engine, lineage state, relations, then bridge subscriptions/UI.
- Do not replay historical birth events during restore.
- Rebuild adjacency/query caches after restore.
- Run reconciliation diagnostic.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 121. Idempotent Bridge Handlers
- `OnChildBorn`, `OnSurvivorDied`, `OnMarriage`, `OnDivorce`, `OnAdoption`, `OnSuccession` all accept source event IDs.
- Processed-event ledger or edge/event stable IDs make replay safe.
- Notification consumers do not create graph state.
- Tests replay events.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 122. Survivor ID Lifecycle
- Never reuse survivor IDs across generations.
- Genealogy references remain permanent.
- Departed/deceased survivors remain resolvable through archive/minimal identity registry.
- UI degrades gracefully if full survivor object unavailable.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 123. Family Event Description Localization
- Store template ID + structured participant/name refs.
- Render at display time.
- Do not save English prose.
- This preserves localization and name changes/history handling.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 124. Family Name Localization
- Generated proper names may be literal strings selected from a catalog, while titles/descriptions are localized.
- Do not translate a chosen proper name inconsistently across save/load.
- Store selected stable name token/string.
- Document.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 125. UI Color Guardrail
- Source suggests alive green/deceased gray/infant light green.
- Use design-system palette and semantic icons/text in addition to color.
- Do not encode age/death meaning only by hue.
- Accessibility snapshot required.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 126. Tree Edge Accessibility
- Solid/dashed/dotted line styles need a legend and text alternative.
- Keyboard focus on node should announce parents/partners/children.
- Screen-reader support where Godot UI permits.
- List/tree outline fallback.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 127. Tree Layout Determinism
- Given same graph and sort rules, node layout ordering should be stable.
- Sort siblings/partners by birth/order/stable ID as defined.
- Do not use dictionary iteration.
- Snapshot tests remain stable.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 128. Large Family Stress
- Generate 1,000 survivors across 8–10 generations with marriages/adoptions/deaths.
- Ancestor/descendant queries, family-unit lookup, event filtering, and UI projection must remain bounded.
- Do not render entire graph by default.
- Measure allocations.

Implementation consequence: this section must resolve through a stable relationship/event source, canonical survivor IDs, explicit graph semantics, idempotent mutation, save-safe state, deterministic query ordering, and projection-only UI. Where the fact is derivable from the graph, keep it derived rather than creating a second persistent truth.

---
## 129. Cycle Fuzz
- Generate invalid attempted parent/adoption edges including self-cycle, 2-node, long cycle.
- Mutation APIs reject them.
- Restore validator detects corrupted legacy data.
