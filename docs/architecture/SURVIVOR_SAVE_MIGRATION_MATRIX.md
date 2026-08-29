# ASHFALL — Survivor Save Migration Matrix

**Task:** #132 — host-independent survivor migration
**Status:** additive planning authority; no legacy wire format is changed by this document
**Last reviewed:** 2026-08-29
**Scope:** `Assets/Ashfall.Core/**/*.cs`, Godot host save façades under `src/`, and the campaign save registry

## 1. Purpose and non-negotiable boundaries

This matrix separates three things that currently share survivor-shaped names but do not have the same authority:

1. **Authored definition data** — who may be instantiated, display text, traits, and base values from `Assets/StreamingAssets/Data/survivors.json` and `starting_survivors.json`.
2. **Canonical campaign identity/lifecycle** — which survivors exist in this campaign and whether each is `Resident`, `Away`, `Dead`, or `Memorialized`, owned by the Core `SurvivorEntityStore` target.
3. **Domain components and history** — needs, radiation, medical state, duties, relationships, fate, memorials, equipment, and other sections that remain owned by their domains while they migrate to typed `SurvivorId` ownership.

The migration is additive and staged. Existing host save façades remain the compatibility boundary until a domain has a typed component, a parity result with the legacy authority, and a host cutover. No existing field is renamed on disk merely because its in-memory representation becomes typed.

### Current canonical identity contract

| Concern | Target contract |
|---|---|
| Type | `Ashfall.Core.Survivors.SurvivorId` |
| Wire representation | bare JSON string, preserving all existing survivor id fields |
| Validation | lowercase snake_case grammar; reject invalid input; never normalize |
| Equality | ordinal |
| Ordering | `string.CompareOrdinal` through `SurvivorId.CompareTo` |
| Empty value | `SurvivorId.None`; never resolves to an aggregate |
| Entity authority | `SurvivorEntityStore` |
| Definition link | separate `SurvivorAggregate.DefinitionId`; equal to `Id.Value` today, not guaranteed forever |
| Lifecycle authority | `SurvivorAggregate.Lifecycle`; fate owns death cause/details, not existence |

## 2. Current state versus target state

### 2.1 Authored catalog is not campaign state

`Assets/StreamingAssets/Data/survivors.json` is a definition catalog. It is the data authority for authored survivor definitions and currently contains 129 definitions. `starting_survivors.json` selects the initial roster. Neither file says that a survivor has joined a particular campaign, died, gone away, been memorialized, or owns a current needs/radiation record.

`SurvivorCatalogLoader` and `SurvivorRosterSystem` may continue to load definitions and display projections. `SurvivorRosterSystem.Join` is the current legacy campaign membership path, but the migration target is:

```text
survivors.json definition
        ↓ load definition
SurvivorEntityStore.TryJoin(SurvivorId, definition_id, day)
        ↓ canonical campaign aggregate
survivor-owned component stores and historical ledgers
```

A catalog id is therefore an input to joining, not a substitute for a campaign entity row. A future generated or duplicate survivor may retain a separate `DefinitionId` while receiving a distinct `SurvivorId`.

### 2.2 Canonical entity persistence is not registered yet

`SurvivorEntityStore` already has a detached, deterministic `SurvivorEntityStoreState` with `schema_version`, `system_id`, and sorted `survivors` rows. It is **not currently registered** in `SaveSectionRegistry`, has no `SectionFileNames` entry, has no `SurvivorEntityStoreSaveStore`, and is not captured by the Godot `SaveAll` path. This is deliberate P0/P1 staging, not an invitation to silently add a second campaign save.

Until the entity section is introduced and wired by a later host task:

- `survivors_save.json` remains the compatibility file for the existing needs/radiation slice.
- `SurvivorEntityStoreState` is an additive Core contract used by tests, parity, and the future migration boundary.
- A host must not claim that saving `survivors_save.json` persists canonical aggregate membership or lifecycle.
- A future entity section must be added to the registry and campaign envelope as one explicit change, with legacy import rules documented and tested.

### 2.3 Existing `survivors` section

| Item | Current contract |
|---|---|
| Registry key | `survivors` |
| File | `survivors_save.json` |
| Façade | `src/Host/SurvivorsSaveStore.cs` |
| State | `SurvivorsSaveState.survivors` list of `SurvivorSliceState` |
| Contents | raw-string `id`, needs values/flags, health cap-adjacent health value, alive mirror, radiation dose/exposure/status/timers |
| Envelope | generic checksummed `{ State, Checksum }`; legacy bare state is rejected (`allowLegacyBareState: false`) |
| Current owner | `SurvivorsHostSession` maps the slice into `NeedsSystem` and `RadiationSystem` |
| Missing from this section | canonical aggregate membership, definition link, joined/lifecycle days, revision, active expedition id, fate cause, memorial history |
| Migration rule | preserve the existing JSON shape; introduce typed adapters/parity around it before changing ownership |

The current host also maintains duplicate in-memory views (`RosterState`, `_radStates`, and the two Core systems). Those are projections/legacy authorities to reconcile, not additional save authorities to preserve indefinitely.

## 3. Save-section inventory and cutover matrix

The rows below name the current section that can contain survivor-scoped state, the kind of ownership it represents, and the conflict rule required before a typed component replaces or fronts it.

### 3.1 Identity, lifecycle, fate, and social records

| Current section | Current owner / survivor-shaped data | State class | Migration target | Conflict and cutover rule |
|---|---|---|---|---|
| `survivors` | `SurvivorsHostSession`; `SurvivorSliceState.id` | active | `NeedsComponentStore` plus `RadiationComponentStore`; aggregate identity remains separate | Match by `SurvivorId`; no aggregate fields may be inferred from a needs slice; preserve existing field names and checksummed envelope |
| `survivor_fate` | `SurvivorFateSystem`; one fate event per raw survivor id | history / death detail | fate ledger keyed by `SurvivorId` | First death report wins; call `SurvivorEntityStore.TryDie` for lifecycle, then retain cause/day/detail in fate; duplicate reports are idempotent |
| `memorial` | `MemorialSystem`; memorial entries keyed by survivor id | history | historical memorial component | Memorialization requires aggregate lifecycle `Dead`; memorial data survives death and is never a second death authority |
| `survivor_relations` | `SurvivorRelationsSystem` | active + historical relationship state | social component(s), with typed endpoints | Every survivor endpoint must resolve; relation history may outlive death only where the owning relation declares retention; deterministic endpoint ordering is required |
| `survivor_social` | `SurvivorSocialCoordinator` and social sub-systems | active + historical social state | social component(s) | Do not merge leadership, friction, ration conflict, trauma bonds, or social state into the aggregate; each sub-state retains its own lifecycle/history rule |
| `survivor_entity_store` *(target; not registered)* | `SurvivorEntityStore` | canonical campaign state | `SurvivorEntityStoreState` | Add one registry key/file only in a later wiring task; rows are sorted by `SurvivorId`; unknown/future schema is rejected, old rows are reported and deterministically repaired only by Core rules |

### 3.2 Medical and health-adjacent records

| Current section | Current survivor key(s) | State class | Migration target | Conflict and cutover rule |
|---|---|---|---|---|
| `medical` | patient/admission, affliction, treatment and respiratory records | active medical state | `MedicalComponent` keyed by `SurvivorId` | Medical admission status is medical-owned. `Deceased` must project from aggregate lifecycle rather than compete with it; active admissions require an existing aggregate |
| `medical_ward` | `MedicalAdmissionRecord.PatientId`; ward beds/inpatients | active placement | medical ward component | A patient endpoint must resolve; bed occupancy and admission status remain ward-owned; death clears active placement through reconciliation, while historical admission records follow medical retention policy |
| `disease` | disease carriers/hosts and patient ids | active + epidemiological history | disease component keyed by survivor id where the field is proven survivor-scoped | Do not convert polymorphic subjects blindly; disease records may reference non-survivor hosts in future data. Preserve history needed for outbreak reconstruction |
| `chemical_dependency` | patient/survivor dependency and withdrawal records | active medical state | medical component or dedicated dependency component | Dependency records require a live aggregate while active; withdrawal history may survive death if the domain declares it historical; no duplicate alive/dead authority |

### 3.3 Radiation, travel, duty, and shelter assignment

| Current section | Current survivor key(s) | State class | Migration target | Conflict and cutover rule |
|---|---|---|---|---|
| `dose_ledger` | survivor dose ledger, cohorts, exposure history | active + history | radiation component plus dose-history ledger | Current dose/status belongs to the radiation component; lifetime exposure and cohort history retain their own policy; the ledger must not create an unknown survivor |
| `expedition` | active sortie participant keyed by survivor id | active participation + route history | expedition participation linked to aggregate `Away` and `ActiveExpeditionId` | `Away` iff exactly one active expedition agrees with the aggregate; death clears deployment atomically; duplicate active participation is an integrity error |
| `duty_roster` | duty rows and role-to-survivor assignments | active assignment | assignment component | Only aggregate `Resident` is assignment-eligible. A current `assignedAway` row is a legacy ambiguity and must be reported/reconciled, not used to invent a new lifecycle state |
| `shelter_assignment` | room/quarters owner ids | active assignment | shelter assignment component | Owner must resolve to a `Resident`; away/dead occupants are stale assignments and should be released by the assignment domain during cutover |
| `shelter_schedule` | shift/curfew participant ids | active schedule | schedule component | Keep schedule ownership separate from duty ownership; every survivor endpoint resolves and resident eligibility is checked at assignment time |

### 3.4 Social pairings, training, crises, combat, and startup state

| Current section | Current survivor key(s) | State class | Migration target | Conflict and cutover rule |
|---|---|---|---|---|
| `caregiving` | patient-to-caregiver pair and bond strength | active two-endpoint relation | caregiving/social component | Both patient and caregiver must resolve to aggregates; a pair is not valid when either endpoint is unknown; death/away handling is domain policy, not a lifecycle rewrite |
| `apprenticeship` | mentor/apprentice or learner ids | active two-endpoint relation + progress history | skills/social component | Validate both endpoints and preserve progress history; do not treat an apprentice id as a definition id |
| `mental_health_crisis` | survivor/patient crisis records | active medical/psychological state | psychological component | Active crisis requires an existing living aggregate; resolution history can retain a deceased subject if required for narrative continuity |
| `combat` | combat participants, trauma owners, and polymorphic subjects | active encounter + post-combat history | combat/psychological component | Convert only fields proven to be survivor-scoped; `subjectId` is polymorphic in combat/flags and must remain raw until classified; combat history may outlive death |
| `phase0` | startup census, pre-war setup, and initial claims may mention survivor ids | campaign setup / provenance | no direct survivor component; import projection only | Phase0 may seed definitions or initial aggregate joins, but it must not become a second roster or lifecycle authority after import; preserve provenance for deterministic replay |

### 3.5 Holdfast population and equipment ownership

| Current section | Current survivor key(s) | State class | Migration target | Conflict and cutover rule |
|---|---|---|---|---|
| `holdfast` | census, levy, population claims, and shelter-level survivor references | campaign projection / policy state | holdfast projection keyed through aggregate ids | Census/levy is not proof that an aggregate exists. Reconcile claims against `SurvivorEntityStore`; retain non-survivor population tokens if the data model allows them, rather than forcing every census subject into `SurvivorId` |
| `equipment_condition` | equipment owner ids and weapon/tool condition | active equipment ownership | equipment component keyed by optional `SurvivorId` | `ownerId` may be empty to mean unassigned/any owner. Introduce an explicit optional-owner boundary before converting; never turn empty owner into `SurvivorId.None` and then treat it as a real survivor |
| `inventory` | item instances, equipped ownership, and inventory holders | active inventory | inventory component / item store with optional owner | Inventory can contain shelter/world/unassigned items. Convert only fields proven to be survivor owners; equipment condition and inventory must agree on ownership but neither owns survivor lifecycle |

## 4. Indirect and polymorphic identity fields

The identity inventory found 1,377 `survivorId`-shaped tokens and several misleadingly generic names. The migration must classify by evidence, not by spelling.

| Field family | Current classification | Required action |
|---|---|---|
| `survivorId`, `survivorIds`, `survivor_id` | survivor-scoped when the containing type is a survivor domain record | Parse at the boundary and use `SurvivorId` internally; preserve bare string wire shape |
| `patientId`, `PatientId`, `caregiverId`, `CaregiverId` | survivor-scoped in medical/caregiving domains | Convert endpoint-by-endpoint; validate both sides of pair records |
| `subjectId`, `SubjectId` | polymorphic: survivor, faction, location, quest token, or other subject | Do not blanket-convert. Add a domain-specific survivor projection only after the containing contract proves the subject is a survivor |
| `ownerId`, `OwnerId` | optional owner; survivor-scoped in equipment in practice, but empty currently means unassigned/any owner | Define an explicit optional-owner representation before conversion; preserve empty/unassigned semantics |
| `authorId`, `AuthorId` | survivor-scoped through journal author abstractions, not a general identity authority | Convert with the journal domain and `ISurvivorAuthor`; do not make journal text the canonical roster |
| `definitionId` | authored content id, not campaign identity | Keep separate from `SurvivorId`; today it is 1:1 only because the join path uses the catalog id directly |
| `actorId` / `byActor` | survivor-scoped only in systems whose actor is proven to be a campaign survivor | Classify per system; name drift is not permission to convert blindly |

## 5. Conflict matrix and resolution rules

| Conflict | Current symptom | Canonical rule | Resolution during migration |
|---|---|---|---|
| Identity authority | roster, host mirrors, needs, radiation, fate, expedition, and many domain dictionaries each accept raw strings | `SurvivorEntityStore` is the only authority for campaign membership and lifecycle | Register typed stores against the entity store; a component may reference an id only after resolving it |
| Definition versus entity | `survivors.json` ids are used as if they were living campaign rows | definition content and campaign membership are different concepts | Load catalog definitions, then explicitly `TryJoin`; persist `DefinitionId` separately |
| Lifecycle booleans | roster `isAlive`, needs `IsAlive`/`IsDead`, radiation `IsAlive`, ward `Deceased`, and memorial entries can disagree | aggregate lifecycle is the single alive/dead/away answer | Reconcile booleans as projections/inputs; fate owns death detail; medical owns incapacity/admission |
| Duplicate registration | `List.Contains` used reference equality in legacy Needs/Radiation paths; same id can have multiple objects | at most one active component record per `(component, SurvivorId)` | typed stores use `Dictionary<SurvivorId, ...>`; duplicate upsert replaces deterministically or reports a conflict; parity reports duplicate legacy ids |
| Stale restore state | host clears a mirror but old Core state can remain registered | restore replaces one complete component snapshot | unregister/reset old legacy state before rehydration; typed stores restore detached rows and expose deterministic diagnostics |
| Endpoint pairs | caregiving/apprenticeship and similar records validate only one side or neither | every survivor endpoint resolves; pair cardinality is domain-owned | validate both endpoints before commit; reject/report dangling rows without fabricating aggregates |
| History retention | active needs/duties are kept on deceased records, while memorial/fate/trauma history must survive | each component declares `RetainsHistoryAfterDeath` | active stores release on leave/death according to domain lifecycle; history stores retain immutable records and never reactivate them |
| Away versus assignedAway | an expedition makes a survivor `Away`, but duty/assignment records may still say assigned | only `Resident` is assignment-eligible; `Away` is not a new assignment lifecycle state | preserve legacy rows long enough for audit, emit a warning, then release/reassign them at assignment-domain cutover |
| Expedition pair | aggregate says `Away` without an active sortie, or sortie lists a resident/deceased survivor | `Away` iff aggregate and exactly one active expedition agree on member and expedition id | cross-domain integrity sweep is an error for unknown/duplicate/mismatched participation; death clears the expedition link |
| Death duplication | fate, needs, radiation, medical, and combat can all report death | `TryDie` is idempotent; fate records cause/detail once | first accepted death transitions aggregate; later reports become no-ops or append explicitly non-authoritative diagnostics |
| Wire compatibility | old saves use raw string ids, mixed naming conventions, and existing checksummed envelopes | typed identity serializes as a bare string; old section envelopes stay byte-compatible until a versioned cutover | use adapters/parity; do not rename `id`, `patientId`, or other legacy fields in this phase; new Core state may use snake_case only when it is a new section |
| Save-section ownership | one campaign save can mention a survivor in many independent sections | each domain remains responsible for its own component; aggregate section owns only identity/lifecycle | add registry entries and campaign-envelope mappings explicitly; never infer a new section from a catalog or host mirror |
| Deterministic ordering | dictionaries and registration order differ between hosts; duplicate first-match behavior depends on insertion order | all canonical survivor rows and component owner lists sort by ordinal `SurvivorId` | capture sorted rows, expose sorted owner ids, and make parity diagnostics stable by id then field |
| Unknown ids | a domain can create a record for an id absent from the roster | no component invents a survivor | integrity error for unknown owners; restore reports the row and leaves the aggregate set unchanged |
| Future schema | a newer entity save may contain lifecycle values or fields this build cannot interpret | reject future schema rather than guess | `RestoreState` fails closed for newer `schema_version`; older rows use documented deterministic repairs only |
| Empty identity | null/empty strings are accepted by several legacy stores | empty means no identity, never a survivor | reject/skip at typed boundary; optional ownership must use an explicit optional representation, not an empty `SurvivorId` |

## 6. Migration sequence and evidence required for each cutover

1. **Canonical identity:** keep `SurvivorId`, `SurvivorAggregate`, and `SurvivorEntityStore` additive and host-independent.
2. **Needs dual-run:** maintain the legacy `NeedsSystem` as the simulation authority while a typed `NeedsComponentStore` captures/upserts the same records by `SurvivorId`.
3. **Parity:** compare every persisted needs field, including threshold flags, `MaxHealthCap`, `IsAlive`, `IsDead`, and deterministic owner sets. Duplicate legacy ids, missing typed rows, extra typed rows, and field mismatches must be reported in stable ordinal order.
4. **Restore proof:** capture/restore typed rows detached from live objects; prove that a second restore does not leave ghost registrations or alter canonical ordering.
5. **Lifecycle reconciliation:** once host wiring is available, component registration and release follow aggregate lifecycle events. Needs is initially `ZeroOrOne` while composition is staged; it may become `OnePerEligible` only after the host guarantees creation for every eligible survivor.
6. **Save cutover:** introduce canonical entity persistence as an explicit registry section and define import from the legacy roster/survivor slice. Do not overload `survivors_save.json` with aggregate fields without a versioned contract.
7. **Domain-by-domain removal:** remove a legacy authority only after its typed component, save adapter, parity tests, restore tests, and integrity checks are green. History-owning domains must retain their post-death records even after active components release.

### Out of scope for this additive document

- Repairing or formatting any Task #131 `src/Main.*` composition-root work.
- Wiring `SurvivorEntityStore` into the Godot host or `SaveAll`.
- Renaming existing save JSON fields or changing existing section envelopes.
- Converting polymorphic `subjectId`, optional `ownerId`, or journal `authorId` without domain-specific evidence.
- Making `SurvivorDefinition` and `SurvivorId` the same type.

## 7. Source authority

- [`docs/architecture/survivor_identity_inventory.json`](survivor_identity_inventory.json) — identity census, comparer findings, lifecycle backing, and known defects.
- [`Assets/Ashfall.Core/Survivors/SurvivorId.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorId.cs) — canonical value semantics and wire converter.
- [`Assets/Ashfall.Core/Survivors/SurvivorAggregate.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorAggregate.cs) — minimal aggregate boundary.
- [`Assets/Ashfall.Core/Survivors/SurvivorEntityStore.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorEntityStore.cs) — lifecycle transactions, deterministic capture, and restore rules.
- [`Assets/Ashfall.Core/Survivors/ISurvivorComponentStore.cs`](../../Assets/Ashfall.Core/Survivors/ISurvivorComponentStore.cs) — component cardinality and history-retention contract.
- [`Assets/Ashfall.Core/Survivors/SurvivorIntegrityValidator.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorIntegrityValidator.cs) — cross-component and lifecycle checks.
- [`Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`](../../Assets/Ashfall.Core/Save/SaveSectionRegistry.cs) — current campaign section authority; no `survivor_entity_store` entry exists yet.
- [`docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`](../saves/SAVE_STORE_CONTRACT_MATRIX.md) — current save-store envelope, slot-root, and section inventory.
- [`src/Host/SurvivorsSaveStore.cs`](../../src/Host/SurvivorsSaveStore.cs) and [`src/Host/SurvivorsHostSession.cs`](../../src/Host/SurvivorsHostSession.cs) — current needs/radiation compatibility boundary.
