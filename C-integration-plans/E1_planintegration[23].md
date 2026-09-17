---
PLAN_ID: E1-23
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 23
STATUS: READY_FOR_EXECUTION_WHEN_DEATH_INVENTORY_AND_MEMORIAL_AUTHORITIES_PASS
SOURCE_PLAN: "Plan 206 — Survivor Death, Legacy & Inheritance System"
SEQUENCE_FILENAME: "E1_planintegration[23].md"
PREVIOUS_FILENAME: "E1_planintegration[22].md"
NEXT_FILENAMES:
  - "E1_planintegration[24].md"
  - "E1_planintegration[25].md"
CATEGORY: LINK+SURVIVORS+DEATH+ESTATE+MEMORIAL+LEGACY
PRIMARY_INTENT: "Create immutable death provenance, estate/will instructions, transactional inheritance settlement, memorial integration, and bounded legacy consequences while preserving SurvivorFate/Health/Combat/Disease/Aging as death-cause authorities, Inventory as item owner, Relations/Psychology/Needs as emotional-social authorities, Quest as quest lifecycle owner, Leadership/Governance as succession owner, and E1-5 as cross-campaign legacy owner."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SECOND_DEATH_RESOLVER_FORBIDDEN: true
SECOND_INVENTORY_LEDGER_FORBIDDEN: true
SECOND_RELATIONSHIP_ENGINE_FORBIDDEN: true
SECOND_GRIEF_PSYCHOLOGY_ENGINE_FORBIDDEN: true
SECOND_QUEST_LIFECYCLE_FORBIDDEN: true
SECOND_LEADERSHIP_SUCCESSION_ENGINE_FORBIDDEN: true
CROSS_CAMPAIGN_MECHANICAL_INHERITANCE_FORBIDDEN_BY_DEFAULT: true
RNG_FOR_DEATH_RECORD_OR_ESTATE_SETTLEMENT_FORBIDDEN: true
RUNTIME_RISK: HIGH
SAVE_RISK: VERY_HIGH
INVENTORY_CONSERVATION_RISK: VERY_HIGH
NARRATIVE_SAFETY_RISK: HIGH
EXPLOIT_RISK: HIGH
---

# E1 Plan Integration [23] — Survivor Death Records, Wills, Estates, Inheritance, Memorials, Grief Handoffs, and In-Campaign Legacy

> **Sequence rule:** this file is `E1_planintegration[23].md`.
> The next files are `E1_planintegration[24].md`, `E1_planintegration[25].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan converts Plan 206 into an implementation-grade survivor death, estate, inheritance, and memorial
programme.

The source plan identifies an important continuity gap. ASHFALL already knows whether a survivor is alive or
dead and already has a `MemorialSystem`, yet the death transition appears to discard much of the survivor's
context: how they died, what they owned, what should happen to personal possessions, what unfinished promises
remain, and how the shelter remembers them.

The feature should make death materially and narratively consequential without creating a second mortality
engine.

The architectural rule is:

**the system records and settles the consequences of a death; it does not decide that the survivor dies.**

Canonical death-producing systems—Health, Disease, Radiation, Combat, Aging, accidents, execution/governance,
or other explicit causes—must emit one normalized death event/provenance record. `SurvivorFateSystem` remains
the authoritative alive/dead lifecycle source. E1-23 then:

- freezes immutable death provenance;
- snapshots only the estate references necessary for settlement;
- resolves wills/intestate policy;
- transfers real items through Inventory exactly once;
- feeds MemorialSystem;
- emits grief/trauma/social consequence events to their canonical owners;
- reconciles quests/goals/roles/leadership through their owners;
- preserves in-campaign history;
- optionally exports narrative facts to E1-5 cross-campaign legacy without granting mechanical inheritance by default.

## 1. Source Intent Preserved

Plan 206 asks for:

- cause-of-death records;
- last wills and beneficiaries;
- special bequests;
- executors and witnesses;
- testate/intestate inheritance;
- disputed inheritance;
- sentimental possessions;
- grief/trauma/motivation consequences;
- memorial integration;
- leadership succession;
- quest/event hooks;
- permanent records;
- deterministic behavior;
- old-save compatibility;
- headless validation.

E1-23 preserves those goals while correcting ownership, legal abstraction, item conservation, emotional-effect
duplication, and cross-campaign boundary risks.

## 2. Core Architecture Thesis

```text
Canonical lethal outcome
      |
      +--> Health / Disease / Radiation / Combat / Aging / Accident / Governance
      |
      v
SurvivorFate authoritative death transition
      |
      v
Normalized SurvivorDeathOccurred
      |
      +--> survivor_id
      +--> death event id
      +--> day/time
      +--> location
      +--> cause provenance
      +--> killer/incident refs when appropriate
      +--> witnesses/participants when authoritative
      |
      v
E1-23 death record + estate opening
      |
      +--> immutable death record
      +--> active will snapshot/reference
      +--> personal-estate inventory query
      +--> unresolved obligations/quest refs
      |
      v
Estate settlement transaction
      |
      +--> specific bequests
      +--> category/percentage rules
      +--> commons/residue
      +--> disputed escrow
      |
      v
Canonical owner handoffs
      |
      +--> Inventory transfers
      +--> MemorialSystem
      +--> Relations / Psychology / Needs
      +--> Quest / Personal Goals
      +--> Leadership / Governance
      +--> E1-5 narrative legacy
```

## 3. Architectural Corrections to the Source Plan

### 3.1 Death cause must come from the lethal authority

Do not infer death cause after the fact from current stats. A survivor who dies in combat while also starving
must not be ambiguously classified by whichever system E1-23 checks first.

Every lethal authority should provide typed cause provenance when it creates the death transition.

### 3.2 Death recording uses no RNG

The death happened. Its record is deterministic. Witnesses should be based on canonical presence/participants,
not random selection. Any authored last words or narrative line may be selected deterministically through the
existing narrative system, but it is presentation—not death truth.

### 3.3 Inventory settlement must transfer real objects

`InheritedItem` is an accounting/history DTO, not an item owner. Actual item stacks/instances move through
Inventory or the canonical personal-equipment container.

### 3.4 “Sentimental value” is not a universal hidden item stat by default

Prefer explicit personal-item provenance, gift/relationship history, authored tags, or memorial significance.
Do not add a universal 0–100 sentimental stat to every item unless an existing item-affinity system already
supports it.

### 3.5 Grief, trauma, morale, and motivation remain external

A death can emit psychological/social events. E1-15/Psychology, Needs/MentalHealth, Relations, and
MemorialSystem resolve those consequences. Do not create `LegacyEffect.magnitude` as another parallel mental
state.

### 3.6 Intestate distribution should not be “highest affinity gets everything”

Relationship closeness can inform an authored fallback policy, but the shelter commons, family/partner state,
personal ownership rules, and gameplay fairness matter. A deterministic fallback hierarchy should be
explicit and conservative.

### 3.7 Disputes need institutional context

A survivor being “excluded” should not automatically contest a will. A dispute requires an actual claimant,
grievance, applicable relationship/claim, and governance/interpersonal context. Mediation belongs to
InterpersonalConflict/Governance/Leadership when those systems exist.

### 3.8 Suicide/murder/execution causes require careful event provenance

These categories should only be used when the canonical system explicitly produces that cause. E1-23 must not
infer self-harm intent or criminal responsibility from generic health/mental-health state.

### 3.9 Cross-campaign inheritance is E1-5 territory

Plan 206 is in-campaign estate settlement. It may export narrative history to E1-5 but must not transfer items,
stats, bonuses, or power into future campaigns unless E1-5's separately gated continuation policy explicitly
allows it.

### 3.10 Leadership succession is not inheritance

If a leader dies, Leadership/Governance owns succession. A will cannot assign political office unless that
governance system explicitly supports hereditary/designated succession.

## 4. Non-Negotiable Rules

- `SurvivorFateSystem` remains authoritative for alive/dead state.
- Health/Disease/Radiation/Combat/Aging/Accident/Governance authorities provide death-cause provenance.
- E1-23 never kills a survivor.
- One immutable death record per canonical death event.
- Death records are append-only after commit except schema migration/correction tooling with provenance.
- Inventory owns all item instances/stacks and personal-equipment containers.
- Estate settlement moves real items exactly once.
- No item can exist simultaneously in deceased inventory, heir inventory, escrow, and shelter commons.
- No will can be executed twice after reload.
- Will updates are versioned; the active version at death is frozen for estate settlement.
- Wills cannot be edited after the death event.
- Executor is an administrative role/reference; executor does not own estate items.
- Beneficiaries must be valid canonical survivors/entities according to policy.
- Dead/missing/departed beneficiaries have explicit fallback semantics.
- Percentages/categories are validated before a will becomes valid.
- Specific bequests take precedence only according to one documented rule.
- Category percentages cannot create fractional duplicate items.
- Unique items require deterministic assignment.
- Disputed items enter a real escrow/estate container or remain locked in estate ownership.
- Commons is a canonical shelter inventory/container, not a number in E1-23.
- Relations owns relationship changes.
- E1-15/Psychology and Needs/MentalHealth own grief, trauma, stress, morale, and recovery.
- MemorialSystem owns burial/remembrance presentation/outcomes.
- QuestSystem owns quest lifecycle and rewards.
- PersonalQuest/Goals owns unfinished personal goals.
- Leadership/Governance owns political succession.
- E1-20 Roles owns formal specialist appointments.
- E1-5 owns cross-campaign legacy/continuation.
- Death record generation uses no RNG.
- Estate settlement uses no RNG.
- Dispute eligibility/resolution is deterministic unless canonical mediation/conflict policy uses keyed RNG.
- Old saves do not fabricate historical death records for survivors who died before the feature unless
  authoritative historical data already exists.
- Old saves preserve all living survivors and inventory exactly.
- First post-migration death must settle correctly without retroactive estate reconstruction.
- Death during expedition/outpost/caravan/combat must reconcile item location/ownership correctly.
- Mass-casualty events must be order-stable.
- The player's save/reload cannot reroll heirs, item assignment, cause of death, or dispute outcome.
- The first release should prove death record + one will + deterministic estate settlement + memorial handoff
  before broad dispute/legacy complexity.

## 5. Acceptance Slices

### Slice A — Immutable death provenance
Normalize lethal events into one `DeathRecord` and feed MemorialSystem.

### Slice B — Personal estate settlement
Move equipped/personal items into estate ownership and then commons/heir through real Inventory transactions.

### Slice C — Wills
Create/update/freeze a will and execute one specific bequest plus one residuary rule.

### Slice D — Emotional and quest handoffs
Feed grief/trauma/quest closure to canonical systems.

### Slice E — Disputes and advanced legacy
Only after core conservation and save/load are proven.

Do not begin with inheritance disputes and 11 cause categories before death provenance and item conservation
are correct.

---

## E1-23A — Premise verification and death-authority audit

**Goal:** Verify death transitions, cause provenance, inventory ownership, memorials, relationships, psychology, quests, leadership, roles, and save rails before adding estate state.

### Required substeps

1. Inspect `SurvivorFateSystem`, all Health/Disease/Radiation/Combat/Aging/Accident/Execution death paths, `MemorialSystem`, Inventory/personal equipment, SurvivorRelations, E1-15 psychology, Needs/MentalHealth, personal quests/goals, QuestSystem, Leadership/Governance, E1-20 roles, E1-5 legacy, and save registry.
2. Search for death events, survivor removal, corpse/remains, equipment cleanup, inventory transfer, memorial creation, grief, successor, executor, heir, estate, personal stash, ownership, and quest invalidation.
3. Determine whether dead survivor objects remain queryable after death.
4. Determine whether item ownership is per-survivor, equipped-only, container-based, or shelter-global.
5. Determine whether cause-of-death fields already exist in health/combat records.
6. Determine whether witness/participant/location data exists at death time.
7. Determine whether death currently deletes inventory or merely detaches survivor references.
8. Create `docs/systems/DEATH_ESTATE_AUTHORITY_MAP.md`.
9. Create intake duplicate-search evidence linking E1-5, aging, health history, personal quests, interpersonal conflict, leadership/governance, memorial, and survivor-role plans.
10. Set `PREMISE_VERIFIED_AT` to current HEAD.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23B — Death/estate ownership ADR

**Goal:** Define E1-23 as immutable death provenance plus estate orchestration, not a mortality or psychology system.

### Required substeps

1. Compare one `SurvivorDeathLegacySystem`, a `DeathRecordRegistry` + `EstateSettlementService`, and extension of SurvivorFate/Memorial.
2. Prefer separating immutable death records from mutable estate settlement state.
3. Define death-record owner, will owner, estate/distribution owner, dispute orchestration owner, and memorial handoff.
4. Explicitly exclude alive/dead state, health, grief, morale, relationships, item ownership, quest lifecycle, and leadership office.
5. Define operation IDs and restore ordering.
6. Define feature flags for wills/disputes.
7. Require second-tool architecture review.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23C — Normalized death event contract

**Goal:** Create one typed death event consumed by records, memorials, psychology, quests, and succession.

### Required substeps

1. Define stable death event ID, survivor ID, campaign day/time, location/site, cause code, source authority, source incident ID, killer/actor ID when canonical, witnesses/participants when canonical, and optional contextual tags.
2. Do not copy full survivor DTO.
3. Do not infer intent/criminal responsibility.
4. Use source-specific cause detail payloads only through typed fields.
5. Make event immutable.
6. Add serialization tests.
7. Require every lethal owner to emit exactly once.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23D — Death-cause taxonomy reconciliation

**Goal:** Map the source plan's cause categories to actual lethal authorities without creating ambiguous post-hoc inference.

### Required substeps

1. Audit supported lethal causes.
2. Define stable top-level cause family plus source-specific detail code.
3. Support starvation/dehydration only if Needs produces explicit lethal events.
4. Support radiation only from Radiation/Health lethal event.
5. Support combat only from combat result.
6. Support disease only from Disease/Health.
7. Support old age only from aging authority.
8. Support accident only from explicit incident authority.
9. Support murder/execution/self-inflicted only when source authority explicitly says so.
10. Use Unknown only when provenance is genuinely missing.
11. Add cause-owner contract tests.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23E — Death-record schema

**Goal:** Persist a compact immutable historical record with stable references and safe presentation fields.

### Required substeps

1. Define record ID, death event ID, survivor ID, frozen display-name snapshot if needed for historical resilience, death day/time, cause family/detail, source authority/incident refs, location/site, witness refs, burial/memorial refs, and optional narrative-context ID.
2. Do not store editable free-form truth descriptions as authority.
3. Keep last words as narrative/presentation record with provenance if used.
4. Store schema version.
5. Do not store living-survivor stats.
6. Add round-trip and immutability tests.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23F — Death-record creation idempotency

**Goal:** Guarantee one record per death even when multiple subscribers, saves, or scene reloads observe the transition.

### Required substeps

1. Use death event ID as idempotency key.
2. Check registry before append.
3. Commit record before downstream optional events.
4. Allow replay-safe subscriber delivery.
5. Do not create second record on Memorial restore.
6. Add duplicate-event tests.
7. Add save-at-death-boundary tests.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23G — Witness/location provenance

**Goal:** Capture witnesses and location only from canonical presence/incident data.

### Required substeps

1. Combat witnesses come from encounter participants/observers if available.
2. Shelter accident witnesses come from room/site presence.
3. Expedition death witnesses come from expedition party.
4. Do not randomly choose witnesses.
5. Do not infer witnesses from relationship strength.
6. Use empty/unknown when not available.
7. Add location/witness tests across shelter, expedition, combat, outpost, and remote death.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23H — Last-words/narrative boundary

**Goal:** Treat last words as authored presentation rather than mandatory simulation truth.

### Required substeps

1. Only record last words if an event/dialogue system explicitly produced them.
2. Do not invent quoted speech automatically as factual record.
3. If procedural lines are used, route through narrative/localization system and store line ID.
4. Keep optional.
5. Do not use last words to alter inheritance unless will/narrative event explicitly says so.
6. Add missing/available-line tests.
7. Keep sensitive death narratives restrained.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23I — Will identity/versioning

**Goal:** Make wills editable while alive but freeze the correct version at death.

### Required substeps

1. Define will family ID, version ID, survivor ID, created/updated day, status, witness refs if required, executor ref, beneficiary rules, special bequests, residuary rule, and validity diagnostics.
2. Every edit creates a new immutable version or auditable revision.
3. Exactly one active version while survivor is alive.
4. Death freezes active version ID into estate.
5. Do not mutate frozen will after death.
6. Add edit/version/save tests.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23J — Will validity policy

**Goal:** Validate wills deterministically and avoid unsupported legal simulation.

### Required substeps

1. Define whether witnesses are required by game rules; do not assume real-world law.
2. Validate beneficiary IDs, percentages, category conflicts, item refs, duplicate bequests, executor reference, and residuary policy.
3. Do not infer mental incompetence unless a canonical incapacity/governance system provides an explicit state.
4. Allow invalid/unfinished draft wills without treating them as executable.
5. Show actionable errors.
6. Add exact-boundary tests.
7. Keep rules in `inheritance_rules.json` or equivalent.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23K — Beneficiary rule schema

**Goal:** Represent beneficiaries as deterministic estate-allocation policies.

### Required substeps

1. Define beneficiary entity ID, category/tag filter, percentage/share, priority, fallback beneficiary, and optional notes/localization.
2. Use percentages only for divisible/stackable estate value or item-count allocation where policy defines deterministic rounding.
3. Unique items cannot be split.
4. Do not copy item names/condition into rule.
5. Validate sums/overlaps.
6. Add category overlap tests.
7. Prefer simple first-release rules.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23L — Special bequest contract

**Goal:** Assign a specific item instance through stable inventory identity.

### Required substeps

1. Require stable item instance/stack reference.
2. Validate item is personally owned by testator at will edit time but revalidate at death.
3. Define fallback if item was consumed/lost/transferred before death.
4. Specific bequest precedence is documented.
5. Do not reserve/lock item forever solely because it appears in a will unless design explicitly chooses that cost.
6. Add item-missing/replaced/consumed tests.
7. Keep unique item semantics deterministic.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23M — Executor boundary

**Goal:** Treat executor as administrative responsibility, not ownership or magical distribution power.

### Required substeps

1. Validate executor is living/available at death when manual execution matters.
2. Allow automatic shelter administration if executor absent according to policy.
3. Executor may influence timing/mediation only through canonical Duty/Governance.
4. Do not transfer estate to executor first.
5. Do not let executor redirect bequests unless governance/dispute resolution changes the settlement order.
6. Add absent/dead/executor-is-beneficiary tests.
7. Keep executor optional in first slice.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23N — Estate opening transaction

**Goal:** Freeze the estate at death without deleting or duplicating personal possessions.

### Required substeps

1. On death event, query canonical personal equipment/inventory/owned containers.
2. Create estate ID linked to death record.
3. Move or lock eligible personal items into canonical estate/escrow container exactly once.
4. Exclude communal shelter inventory not personally owned.
5. Handle equipped gear, carried expedition cargo, vehicle cargo, quest items, borrowed tools, and faction-owned items through ownership rules.
6. Record source inventory transaction IDs.
7. Add conservation tests.
8. Do not settle until estate snapshot transaction commits.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23O — Personal versus communal ownership ADR

**Goal:** Resolve what can legally/game-mechanically be inherited before implementing wills.

### Required substeps

1. Audit whether inventory distinguishes personal/communal property.
2. If most shelter items are communal, inheritance should cover only explicit personal possessions/equipment/sentimental items.
3. Do not allow a survivor's will to give away shelter food stock or mission-critical communal tools.
4. Define borrowed/issued equipment handling.
5. Define purchased/gifted personal items.
6. Create ownership policy IDs.
7. Add ownership tests.
8. Document first-release estate scope.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23P — Estate inventory container

**Goal:** Use canonical Inventory containers/state for unsettled estate property.

### Required substeps

1. Define estate container ID/reference.
2. Inventory owns item instances and quantities.
3. E1-23 stores container/reference and settlement status only.
4. Disputed property may remain in estate/escrow container.
5. Commons transfer uses canonical shelter inventory transaction.
6. Add container restore tests.
7. Assert no item copy in `InheritedItem` history becomes authoritative.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23Q — Testate settlement ordering

**Goal:** Define deterministic execution order for a valid will.

### Required substeps

1. Freeze estate.
2. Resolve invalid/nonexistent item refs.
3. Apply valid specific bequests.
4. Apply category/share rules to remaining eligible items.
5. Apply residuary rule.
6. Send remainder to shelter commons.
7. Create settlement receipts.
8. Emit distribution-completed event after inventory commit.
9. Add deterministic ordering tests.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23R — Intestate policy

**Goal:** Create a conservative fallback that does not simply award everything to the highest-affinity survivor.

### Required substeps

1. Define priority using explicit partner/family/household relationships if canonical.
2. Allow personal tagged heir/designated dependent only if existing state supports it.
3. Use shelter commons as safe fallback.
4. Use affinity only as a tie-break or optional authored policy, not sole entitlement by default.
5. Do not infer kinship from affinity.
6. Document deterministic tie-breaking.
7. Add no-family/partner/family/ambiguous tests.
8. Keep policy data-driven.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23S — Beneficiary eligibility

**Goal:** Resolve what happens when beneficiary is dead, missing, departed, hostile, imprisoned, or at another site.

### Required substeps

1. Query SurvivorFate/lifecycle and site ownership.
2. Define living remote beneficiary transfer policy.
3. Dead beneficiary falls to fallback/residue unless chained inheritance is explicitly supported.
4. Missing beneficiary may cause escrow/timeout.
5. Departed hostile beneficiary may be invalid according to governance/property policy.
6. Do not silently delete property.
7. Add lifecycle matrix tests.
8. Keep cross-site transfer real.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23T — Category allocation and deterministic rounding

**Goal:** Allocate stackable/divisible property without fractional duplication.

### Required substeps

1. Define item-category taxonomy from canonical item data.
2. Sort candidate items/stack units deterministically.
3. Use integer share allocation with documented remainder rule.
4. Do not divide unique item condition/value numerically.
5. Do not use market value as hidden allocator unless policy explicitly chooses value-based division.
6. Add 33/33/34, 50/50 odd-count, unique-item, and mixed-stack tests.
7. Assert conservation.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23U — Sentimental-item policy

**Goal:** Make emotionally significant possessions emerge from real provenance instead of a universal magic number.

### Required substeps

1. Audit gift history, personal ownership duration, unique named items, journal/memory links, crafted-by/given-by provenance, and memorial tags.
2. Define `sentimental` as explicit item tag/reference or derived read-model significance.
3. Do not add generic 0–100 sentimental value to every item without owner ADR.
4. Psychology/Relations consumes inheritance event + provenance.
5. Add tests for ordinary item, gifted item, unique personal item, and unknown provenance.
6. Keep mechanical effects bounded.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23V — Distribution receipt ledger

**Goal:** Record what happened without becoming a duplicate inventory ledger.

### Required substeps

1. Define distribution ID, estate ID, will version ID, day, item transaction refs, beneficiary refs, unresolved items, dispute refs, and status.
2. Do not store authoritative duplicate item condition/quantity.
3. Human-readable history can snapshot item label for archival display.
4. Use canonical inventory transaction IDs.
5. Add reconciliation tests.
6. Bound detail after long campaigns through archival compaction.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23W — Escrow for disputed property

**Goal:** Hold disputed items safely while ownership is unresolved.

### Required substeps

1. Use canonical estate/escrow inventory container.
2. Mark disputed item/stack quantity locked by dispute ID.
3. Prevent beneficiary/commons transfer until resolution.
4. Prevent use/equip/crafting while locked unless governance policy allows emergency override.
5. Resolve lock exactly once.
6. Add save/reload/cancel/resolution tests.
7. Assert no duplicate transfer.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23X — Dispute eligibility policy

**Goal:** Require a concrete claim and context before opening inheritance conflict.

### Required substeps

1. Possible claims: conflicting valid will versions, specific-bequest ambiguity, family/partner policy conflict, invalid beneficiary, executor misconduct event, governance challenge, contested ownership.
2. Do not auto-create disputes because a survivor had high affinity.
3. Do not infer mental incompetence from sadness/trauma.
4. Use canonical relationship/family/governance facts.
5. Return deterministic eligibility/reason.
6. Add eligible/ineligible tests.
7. Keep disputes optional in first release.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23Y — Dispute orchestration boundary

**Goal:** Route conflict/mediation through InterpersonalConflict/Governance/Leadership rather than resolving relationships locally.

### Required substeps

1. Create dispute case with estate/item/claim refs.
2. InterpersonalConflict/Governance owns mediation/hearing process.
3. E1-20 Leader/Diplomat/Mediator capabilities may qualify participants.
4. Relations owns relationship consequences.
5. Estate system only applies the final binding resolution.
6. Do not directly change affinity/resentment.
7. Add mediation/upheld/overturned/dropped tests.
8. Use stable case IDs.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23Z — Dispute resolution transaction

**Goal:** Apply a final dispute decision to estate property exactly once.

### Required substeps

1. Validate decision source authority.
2. Validate disputed property still in escrow.
3. Create resolution operation ID.
4. Transfer item to ordered recipient/commons.
5. Release lock.
6. Update distribution status/history.
7. Emit resolution event.
8. Add duplicate-decision/save tests.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AA — MemorialSystem integration

**Goal:** Feed immutable death facts into the existing burial/remembrance authority.

### Required substeps

1. MemorialSystem consumes death record ID, survivor ID, cause presentation, location, witnesses if useful, and estate/memorial context.
2. Do not duplicate burial outcome in E1-23.
3. Memorial outcome may add memorial refs back to death record through append-only linkage or separate index.
4. Do not alter cause of death from memorial choice.
5. Add burial/cremation/missing-body/no-memorial tests according to actual system.
6. Use one event chain.
7. Preserve historical access.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AB — Body/remains boundary

**Goal:** Do not invent corpse inventory if the repository does not model remains.

### Required substeps

1. Audit whether death creates a body/remains object.
2. If body exists, MemorialSystem/Inventory/World owns it.
3. If not, death record remains valid without body state.
4. Do not block estate settlement solely on burial unless design explicitly wants it.
5. Remote/missing body has explicit memorial behavior.
6. Add body/no-body tests.
7. Keep first release simple.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AC — Relations grief-context handoff

**Goal:** Expose deceased-survivor relationship facts without mutating relations in E1-23.

### Required substeps

1. Capture/resolve canonical relation values at death time if downstream grief needs them.
2. Prefer passing survivor IDs and letting Relations/Psychology query historical relation state.
3. If relation records are removed on death, preserve a read-only historical relation snapshot in the proper relationship/history owner—not E1-23.
4. Do not add `relationshipBond` legacy effect locally.
5. Add close-friend/partner/family/rival tests.
6. Document relationship-record lifetime.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AD — Psychology and grief handoff

**Goal:** Route bereavement, witnessed death, traumatic death, and inherited sentimental items to E1-15/Psychology.

### Required substeps

1. Emit `BereavementEvent` or canonical equivalent with deceased ID, relationship context, witness context, cause/event tags, and memorial/inheritance refs.
2. E1-15 decides trauma, grief, coping, recovery.
3. Needs/MentalHealth owns morale/stress.
4. Do not create fixed `grief motivation` bonuses locally.
5. Do not create trauma merely because witness list is non-empty.
6. Add expected/violent/remote/witnessed death tests.
7. Use provenance dedupe.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AE — Motivation/legacy-effect boundary

**Goal:** Reject generic post-death skill bonuses unless a real canonical narrative/psychology mechanic owns them.

### Required substeps

1. Do not implement `grief_motivation +skill` in E1-23 by default.
2. Allow authored inspiration/commitment events through Psychology/Autonomy/Skill policy if explicitly designed.
3. Do not permanently buff a survivor for receiving an inherited item without canonical consumer.
4. Record narrative legacy facts separately.
5. Add negative tests.
6. Document deferral.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AF — Witnessed-death boundary

**Goal:** Use canonical exposure context for trauma rather than a generic death-witness counter.

### Required substeps

1. Combat/incident authority determines witnesses/participants.
2. Psychology determines whether exposure is traumatic.
3. Do not make witnessing ten deaths a positive achievement by default without content review.
4. Journal may record witness history if useful.
5. Add distance/presence/participant tests.
6. Keep UI wording neutral.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AG — Quest lifecycle integration

**Goal:** Reconcile quests owned by or targeted at the deceased without duplicating QuestSystem.

### Required substeps

1. QuestSystem receives survivor-death event.
2. Define quest policies: fail, redirect, continue, memorialize, transfer responsibility, or remain unresolved.
3. Personal quests/goals owned by deceased close/resolve through their owner.
4. Estate may reference quest-owned items but must respect quest-item transfer policy.
5. Do not directly mark quests complete from inheritance.
6. Add active/personal/rescue/faction/item-quest tests.
7. Use stable death event provenance.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AH — Unfinished personal-goal legacy

**Goal:** Preserve narrative traces of unfinished goals without automatically transferring them as quests.

### Required substeps

1. PersonalGoal/Quest owner identifies goals at death.
2. Memorial/journal may record unfinished goal summary.
3. Another survivor may voluntarily adopt/continue only through a new quest/goal event with agency.
4. Do not auto-assign the deceased's quest to heir.
5. Do not make inherited item imply inherited quest.
6. Add continue/decline/no-eligible-survivor tests.
7. Keep narrative handoff optional.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AI — Leadership succession boundary

**Goal:** Trigger canonical leadership/governance succession when an office-holder dies.

### Required substeps

1. Death event identifies deceased's active E1-20 roles/governance office.
2. Leadership/Governance decides acting leader/election/designated successor.
3. A will cannot transfer office unless governance rules explicitly support designation.
4. Estate settlement is independent.
5. Do not add leadership state to death record beyond historical role snapshot/reference.
6. Add leader-death/succession tests.
7. Prevent double succession from role + death listeners.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AJ — Formal role closure

**Goal:** Close/suspend E1-20 active appointments through role authority.

### Required substeps

1. E1-20 consumes death event.
2. Role appointment becomes former/ended.
3. Coverage dashboard updates.
4. Successor/acting post is decided by E1-20/Governance policy.
5. Do not transfer mastery to heir.
6. Role history may be linked to memorial.
7. Add role closure tests.
8. Use death event ID for idempotency.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AK — Inventory location at remote death

**Goal:** Handle estate possessions correctly when death occurs away from shelter.

### Required substeps

1. Expedition/caravan/outpost/vehicle inventory authority determines current item location.
2. Do not teleport all possessions instantly to shelter.
3. Define party recovery, corpse cache, lost cargo, hostile capture, vehicle container, or remote estate container according to existing travel rules.
4. Only recovered items enter estate settlement.
5. Unrecovered property remains world/cargo state.
6. Add expedition/combat/caravan/outpost tests.
7. Keep conservation exact.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AL — Combat loot and estate precedence

**Goal:** Resolve whether enemies, allies, or estate claim items after combat through one ownership policy.

### Required substeps

1. Combat/loot authority resolves battlefield possession/recovery.
2. E1-23 cannot reclaim items already looted/destroyed canonically.
3. Allied recovered personal gear can enter estate.
4. Enemy-captured gear remains lost/captured until recovered.
5. Unique quest items follow Quest policy.
6. Add partial-loot/destroyed/recovered tests.
7. Document ordering.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AM — Mass-casualty ordering

**Goal:** Make multiple deaths in one incident deterministic and conservation-safe.

### Required substeps

1. Use stable incident/death event ordering.
2. Open separate estate per survivor.
3. Handle mutual beneficiaries who die in the same incident using explicit survivorship/cutoff policy.
4. Do not recursively execute inheritance based on arbitrary event order.
5. Define simultaneous-death fallback to commons/family/alternate beneficiary.
6. Add two-way beneficiary and whole-party-loss tests.
7. Persist incident ID.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AN — Beneficiary death after testator but before settlement

**Goal:** Resolve delayed estate administration without recursive duplication.

### Required substeps

1. Define whether beneficiary entitlement vests at testator death or distribution time.
2. Choose one game rule and document it.
3. If vested, beneficiary estate may receive entitlement through explicit transfer chain.
4. If not, fallback/residue applies.
5. Do not depend on processing order.
6. Add delayed-settlement tests.
7. Keep first release conservative.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AO — Personal ownership transfer semantics

**Goal:** Preserve item metadata and provenance when inheritance occurs.

### Required substeps

1. Inventory transaction changes owner/container only.
2. Preserve condition, modifications, ammo state, durability, custom name, crafted-by, gifted-by, and other canonical metadata.
3. Append inheritance provenance if item history supports it.
4. Do not recreate item from definition ID.
5. Add unique weapon/clothing/tool stack tests.
6. Assert instance identity preservation.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AP — Inheritance and shelter commons

**Goal:** Use the shelter's canonical communal inventory as deterministic fallback.

### Required substeps

1. Define commons container ID.
2. Residual estate enters commons through inventory transaction.
3. Commons is not an heir survivor.
4. Quest/faction-owned items may have different fallback.
5. Do not create currency/item value instead of transferring items.
6. Add no-will/no-heir/invalid-will tests.
7. Use commons as first-release safe default.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AQ — Will authoring UI

**Goal:** Create an understandable will editor without turning death into spreadsheet administration.

### Required substeps

1. Show only personally owned eligible property/categories.
2. Allow specific bequests and simple beneficiary rules.
3. Show validation errors/conflicts.
4. Show executor/witness requirements only if game rules use them.
5. Do not expose inaccessible hidden relation stats.
6. Provide sane default residue-to-commons.
7. Allow update/revoke while alive.
8. Add UI tests for empty/simple/complex/invalid wills.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AR — Death record and memorial UI

**Goal:** Present cause, context, memorial, and life history with restrained factual language.

### Required substeps

1. Show survivor name, portrait/reference, death day, location, canonical cause, memorial state, notable role/relationship/history links.
2. Do not invent blame or intent.
3. Do not sensationalize self-inflicted/murder/execution causes.
4. Show Unknown when evidence is insufficient.
5. Link to memorial/journal.
6. Add snapshot tests across cause families.
7. Keep cause-detail localization reviewed.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AS — Estate/inheritance UI

**Goal:** Make settlement status and item ownership auditable.

### Required substeps

1. Show estate status.
2. Show will version used.
3. Show specific bequests/residue.
4. Show items transferred by canonical transaction history.
5. Show unresolved/disputed items.
6. Show absent beneficiaries/fallback reasons.
7. Do not let UI mutate inventory directly.
8. Add pending/distributed/disputed/escrowed snapshots.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AT — Dispute UI

**Goal:** Present claimant, item/claim, mediation status, and final decision without implementing conflict logic locally.

### Required substeps

1. Show dispute reason/evidence.
2. Show involved survivors.
3. Link to InterpersonalConflict/Governance action.
4. Show escrowed property.
5. Show final resolution.
6. Do not show arbitrary success chance if mediation is canonical.
7. Add UI tests.
8. Feature-gate if disputes deferred.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AU — Journal and chronicle integration

**Goal:** Record meaningful death/estate milestones without duplicating immutable records.

### Required substeps

1. Use Chronicle/Journal.
2. Record death, memorial, will reading, major inheritance, dispute resolution, heroic/tragic context only if source event supports it.
3. Do not create multiple entries for the same death across every subsystem.
4. Use death/estate IDs for correlation.
5. Keep death record authoritative.
6. Bound journal history.
7. Add dedupe tests.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AV — E1-5 cross-campaign legacy boundary

**Goal:** Export narrative history only unless continuation mechanics are separately approved.

### Required substeps

1. Provide immutable death summary ID, survivor identity, famous role/achievement/memorial facts, and campaign record references to E1-5.
2. Do not transfer inherited items into next campaign.
3. Do not grant heirs cross-campaign stat bonuses.
4. Do not create new lineage mechanics here.
5. E1-5 chooses what narrative legacy persists.
6. Add clean-start parity test.
7. Keep mechanical inheritance disabled by default.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AW — Death event content hooks

**Goal:** Use committed death records as narrative triggers without generating the death itself.

### Required substeps

1. Examples: first death, leader death, expedition casualty, long-lived elder death, heroic sacrifice only when incident tags say so, unresolved murder only if canonical crime system exists.
2. Do not author `The Death` as a second lethal event.
3. Events subscribe to death record/death event.
4. Use stable IDs.
5. Keep sensitive content reviewed.
6. Add reload/dedupe tests.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AX — Quest hooks for wills and estates

**Goal:** Add estate-focused quests only after settlement is stable and avoid grindy death-count incentives.

### Required substeps

1. Prioritize create first will, resolve one meaningful estate, recover lost personal effects, mediate one dispute, complete memorial request.
2. Do not reward witnessing or causing many deaths as routine progression.
3. Do not encourage farming inheritance through survivor death.
4. QuestSystem owns lifecycle/rewards.
5. Use will/estate/dispute IDs as provenance.
6. Add invalidation tests.
7. Keep optional.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AY — Exploit incentives and anti-death-farming review

**Goal:** Prevent inheritance from making survivor death economically profitable.

### Required substeps

1. Ensure estate transfer preserves existing value rather than creating bonuses/currency.
2. Do not grant large universal morale/skill rewards for deaths.
3. Do not duplicate insured/replacement items unless another system explicitly owns insurance.
4. Quest rewards must not exceed plausible cost in ways that incentivize deaths.
5. Test deliberate equip-transfer/death loops.
6. Test will-edit immediately before lethal mission.
7. Test repeated heir chains.
8. Document acceptable player agency without moralizing.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23AZ — Old-save migration

**Goal:** Introduce death records/wills prospectively without fabricating history or changing living inventory.

### Required substeps

1. Initialize death-record registry empty unless authoritative historical deceased-survivor records already exist.
2. Initialize wills empty.
3. Initialize estates/disputes empty.
4. Do not reconstruct previously dead survivors from memorial text alone unless a migration explicitly maps stable IDs.
5. Preserve MemorialSystem existing records.
6. Preserve living survivors/inventory/relationships exactly.
7. First new death uses normal pipeline.
8. Version migration.
9. Add early/late/memorial-heavy save fixtures.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23BA — Existing memorial migration

**Goal:** Link pre-existing memorial records conservatively without inventing cause-of-death truth.

### Required substeps

1. Audit MemorialSystem persistence.
2. If memorial has stable deceased survivor ID and date, optionally create a historical death stub only if product requires unified UI.
3. Use cause Unknown unless authoritative cause exists.
4. Do not create estate/will/distribution for historical deaths.
5. Mark migrated historical record provenance.
6. Alternatively keep memorial-only history separate.
7. Require ADR decision.
8. Add migration tests.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23BB — Save contract and restoration order

**Goal:** Persist death/estate state without copying survivor, inventory, relationship, psychology, quest, or leadership state.

### Required substeps

1. Persist death records, will versions, estate/distribution metadata, dispute refs/state, receipts, and schema version.
2. Inventory persists estate/escrow item containers.
3. Restore survivor/fate/history registries before validating death refs.
4. Restore Inventory before estate settlement refs.
5. Restore Memorial/Relations/Psychology/Quest/Leadership after death registry as appropriate.
6. Do not replay death/settlement/psychology events on restore.
7. Handle missing item/beneficiary definitions safely.
8. Add corruption/round-trip tests.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23BC — No-RNG baseline

**Goal:** Make death recording, will execution, intestate fallback, and estate distribution fully deterministic.

### Required substeps

1. Death record uses canonical event.
2. Will selection uses frozen version.
3. Specific bequests use stable item IDs.
4. Category allocation uses deterministic ordering/rounding.
5. Intestate policy uses deterministic hierarchy.
6. Dispute creation uses explicit policy.
7. Only canonical mediation/narrative systems may use keyed RNG if already designed.
8. Add call-order/no-reroll tests.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23BD — Time-skip semantics

**Goal:** Handle delayed memorials, executors, disputes, and estate settlement without replaying death transitions.

### Required substeps

1. Death event occurs at canonical simulated time.
2. Immediate settlement can commit at death or a scheduled reading depending on policy.
3. Delayed obligations use campaign time.
4. Do not create new deaths from time skip in E1-23.
5. Handle beneficiary death between opening and settlement according to explicit policy.
6. Add 1/7/30-day skip tests.
7. Ensure skipped settlement produces same result as stepped execution.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23BE — Item and currency conservation audit

**Goal:** Prove estate settlement is pure ownership transfer unless explicit disposal/loss already occurred elsewhere.

### Required substeps

1. Count item instances/stack quantities before death.
2. Account for combat loss/destruction/recovery.
3. Account for estate container.
4. Account for heir/commons/escrow transfers.
5. Assert no unreferenced disappearance.
6. Assert no duplicate item creation.
7. Currency, if personally owned, follows canonical currency/container rules.
8. Add property tests.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23BF — Cause-of-death correctness audit

**Goal:** Prove death records reflect the lethal authority rather than inferred state.

### Required substeps

1. Test starvation, dehydration, radiation, combat, disease, aging, accident, execution if those paths exist.
2. Test survivor with multiple severe conditions.
3. Test combat death with high radiation.
4. Test disease death during expedition.
5. Test unknown legacy/migration cause.
6. Assert source authority/incident ID.
7. Do not classify unsupported self-inflicted/murder causes without explicit events.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23BG — Sensitive-content review

**Goal:** Ensure death-cause presentation is factual, non-sensational, and does not invent intent.

### Required substeps

1. Review cause labels and descriptions.
2. Self-inflicted death wording requires explicit canonical event and neutral presentation.
3. Murder wording requires explicit homicide/crime determination.
4. Execution wording requires governance/judicial event.
5. Unknown/undetermined remains available.
6. Do not generate graphic descriptions by default.
7. Review memorial/last-words content separately.
8. Add localization/content review gate.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23BH — Mass-death and disaster stress test

**Goal:** Prove large casualty events do not duplicate records or estates.

### Required substeps

1. Simulate multiple survivors dying in one combat/disaster incident.
2. Create distinct death event IDs under one incident ID.
3. Resolve estate openings deterministically.
4. Test cross-beneficiaries.
5. Test shared inventory/borrowed gear.
6. Test memorial queue.
7. Measure event volume.
8. Add save/reload mid-incident test.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23BI — Performance and retention strategy

**Goal:** Keep permanent history bounded enough for long campaigns while preserving meaningful records.

### Required substeps

1. Death records remain compact and permanent.
2. Will edit history may archive/compact superseded versions after death according to policy while preserving frozen version.
3. Distribution detail may retain transaction refs and aggregate archived display text.
4. Dispute history bounded by case count.
5. Do not retain full survivor snapshots.
6. Index records by survivor/death day.
7. Measure save-size growth.
8. Add long-campaign performance thresholds.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23BJ — Headless death-legacy selftest

**Goal:** Build a deterministic CI scenario proving death provenance, will freeze, estate settlement, memorial handoff, and save/load.

### Required substeps

1. Create a survivor with personal canonical items and relationships.
2. Create/update a valid will.
3. Trigger a canonical lethal event from a fixture owner.
4. Verify SurvivorFate marks dead before E1-23 settles consequences.
5. Create one death record exactly once.
6. Open estate and move items into canonical estate container.
7. Execute one specific bequest and residue-to-commons.
8. Verify Inventory conservation.
9. Verify Memorial consumes record.
10. Verify Psychology/Relations receive events but E1-23 does not mutate them.
11. Save/reload at death/estate/distribution boundaries.
12. Create `--death-legacy-selftest`.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23BK — Data-integrity selftest

**Goal:** Validate death causes, inheritance policies, will categories, item refs, beneficiary refs, and cross-system handoffs.

### Required substeps

1. Validate cause-code owner mappings.
2. Validate will/inheritance rule definitions.
3. Validate item category/tag refs.
4. Validate commons/estate container policy.
5. Validate dispute resolution consumer refs.
6. Validate memorial/psychology/quest/leadership adapters.
7. Validate no legacy-effect definition directly mutates grief/morale/skill/relations.
8. Validate no cross-campaign item transfer policy is enabled by E1-23.
9. Fail with actionable diagnostics.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23BL — Documentation and observability

**Goal:** Document death provenance, estate ownership, will semantics, inventory transactions, disputes, and legacy boundaries.

### Required substeps

1. Create `docs/systems/SURVIVOR_DEATH_RECORDS.md`.
2. Create `docs/systems/ESTATE_AND_INHERITANCE.md`.
3. Create `docs/systems/WILL_CONTRACT.md`.
4. Create `docs/systems/DEATH_CAUSE_PROVENANCE.md`.
5. Create `docs/systems/DEATH_CONSEQUENCE_HANDOFFS.md`.
6. Document E1-5 cross-campaign boundary.
7. Add debug readout for death event/record IDs, cause source, estate container, frozen will, distribution transactions, disputes, and downstream event refs.
8. Keep debug mutations dev-only.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

## E1-23BM — Release gate and closure

**Goal:** Ship only when one death can be recorded and one estate can settle without duplicating mortality, inventory, psychology, quest, or leadership state.

### Required substeps

1. Run .NET build/test and game build.
2. Run data-integrity selftest.
3. Run death-legacy selftest.
4. Run death-cause provenance tests.
5. Run Inventory/estate conservation tests.
6. Run will version/validity/execution tests.
7. Run intestate/fallback tests.
8. Run remote-death/combat-loot tests.
9. Run Memorial/Psychology/Quest/Leadership boundary tests.
10. Run old-save/memorial migration tests.
11. Run mass-casualty/idempotency tests.
12. Run exploit/sensitive-content review.
13. Run long-campaign save-size/performance tests.
14. Verify record + estate + will + memorial vertical slice before disputes/advanced legacy.
15. Update ADR, authority map, docs, plan register, and handoff.

**Gate:** Preserve SurvivorFate/death-cause ownership, Inventory conservation, immutable provenance, deterministic/idempotent settlement, and external ownership of grief, relations, quests, leadership, and cross-campaign legacy. Add at least one negative duplicate-authority test and one save/load/conservation assertion.

---

# 6. Canonical Death and Estate Authority Matrix

| Fact | Canonical owner | E1-23 role |
|---|---|---|
| Alive/dead state | SurvivorFate | Consume death transition |
| Lethal cause | Health/Disease/Radiation/Combat/Aging/etc. | Record provenance |
| Death record | E1-23 | Own immutable record |
| Personal item ownership | Inventory | Query/transfer |
| Estate/escrow container contents | Inventory | Own item state |
| Will instructions | E1-23 | Own/version/freeze |
| Estate settlement metadata | E1-23 | Orchestrate/reference transactions |
| Relationship state | SurvivorRelations | No direct mutation |
| Grief/trauma | E1-15/Psychology | Event handoff |
| Morale/stress | Needs/MentalHealth | Event handoff |
| Memorial/burial | MemorialSystem | Death-record consumer |
| Quest lifecycle | QuestSystem/Personal Goals | Death-event consumer |
| Leadership succession | Leadership/Governance | Death-event consumer |
| Role closure | E1-20 | Death-event consumer |
| Cross-campaign legacy | E1-5 | Narrative record consumer |
| Combat loot/recovery | Combat/Inventory | Pre-estate ownership resolution |

---

# 7. Suggested Death Event

```yaml
death_event_id: "death_evt_survivor_0042"
survivor_id: "survivor_0042"
day: 91
location_id: "expedition_rail_yard"

cause:
  family: "COMBAT"
  detail: "ballistic_fatality"
  source_authority: "TacticalCombat"
  source_incident_id: "combat_0187"

witness_ids:
  - "survivor_0011"
  - "survivor_0092"
```

No inferred cause from current health values.

---

# 8. Suggested Death Record

```yaml
schema_version: 1
record_id: "death_record_0042"
death_event_id: "death_evt_survivor_0042"
survivor_id: "survivor_0042"

display_name_snapshot: "Mara"
death_day: 91
location_id: "expedition_rail_yard"

cause_family: "COMBAT"
cause_detail: "ballistic_fatality"
cause_source_ref: "combat_0187"

witness_ids:
  - "survivor_0011"
  - "survivor_0092"

memorial_ref: null
```

Historical display-name snapshots are acceptable presentation resilience; living-state snapshots are not.

---

# 9. Will Version Example

```yaml
will_family_id: "will_survivor_0042"
will_version_id: "will_survivor_0042_v3"
survivor_id: "survivor_0042"
created_day: 80
status: "ACTIVE"

special_bequests:
  - item_instance_id: "item_rifle_022"
    beneficiary_id: "survivor_0011"

residuary_policy:
  type: "BENEFICIARY"
  beneficiary_id: "survivor_0092"
```

At death, `v3` is frozen into the estate.

---

# 10. Estate Opening Pipeline

```text
canonical death event
   |
   v
record death exactly once
   |
   v
query personal ownership
   |
   v
resolve combat/travel loss/recovery first
   |
   v
move eligible items to estate container
   |
   v
freeze active will version
   |
   v
settlement becomes eligible
```

The estate is not opened by copying item DTOs.

---

# 11. Testate Settlement Pipeline

```text
estate opened
  |
  v
validate frozen will
  |
  +--> invalid -> intestate/fallback policy
  |
  v
specific bequests
  |
  v
category/share rules
  |
  v
residuary rule
  |
  v
commons fallback
  |
  v
inventory transactions commit
  |
  v
distribution receipt
```

No RNG is required.

---

# 12. Intestate Baseline

Recommended conservative hierarchy:

1. explicit canonical partner/family/dependent rule where supported;
2. explicit designated dependent/household policy where supported;
3. shelter commons.

Affinity may be used only as a documented tie-break when no stronger canonical relationship category exists.

Avoid:

```text
highest affinity survivor gets everything
```

---

# 13. Item Conservation Equation

For every eligible estate item:

```text
pre-death ownership
=
combat/world loss
+ estate/escrow
+ heir transfer
+ commons transfer
+ explicit destruction/disposal
```

There must be exactly one final canonical owner/state.

---

# 14. Unique Item Rules

Unique items:

- preserve instance ID;
- preserve condition/modifications;
- cannot be split;
- follow specific bequest before category allocation when valid;
- use deterministic fallback;
- remain in escrow if disputed.

Do not recreate them from item definitions.

---

# 15. Stackable Item Rules

For stackable property:

- split through Inventory transaction;
- use integer quantities;
- deterministic rounding;
- explicit remainder rule;
- preserve metadata where stack semantics require it.

Never create fractional units.

---

# 16. Remote Death

Death away from shelter must not teleport property.

Possible canonical outcomes:

```text
party recovers gear
-> recovered items enter estate

enemy loots gear
-> items remain lost/captured

vehicle retains cargo
-> vehicle/container owns items

body/cache remains in world
-> recovery mission may later recover property
```

Estate settlement only distributes recovered/owned property.

---

# 17. Combat Ordering

Recommended order:

```text
combat resolves death
-> combat/loot resolves immediate battlefield ownership
-> SurvivorFate commits death
-> E1-23 opens estate from remaining/recovered personal property
```

Exact repository event order must be audited.

---

# 18. Psychological Handoff

Correct:

```text
death record
+ relationship context
+ witness context
+ memorial/inheritance context
-> E1-15/Psychology
-> Needs/MentalHealth
```

Incorrect:

```text
LegacyEffect:
  trauma = 70
  morale = -25
  skill_bonus = +10
```

E1-23 does not own emotional state.

---

# 19. Memorial Handoff

`MemorialSystem` consumes death history and handles remembrance/burial.

E1-23 may display memorial linkage but must not create a second memorial outcome system.

---

# 20. Succession Handoff

If the deceased held:

- leadership office -> Governance/Leadership handles succession;
- formal E1-20 role -> Role system closes appointment/coverage;
- apprenticeship -> Apprenticeship handles mentor loss;
- quest ownership -> Quest/Personal Goal system resolves.

A will cannot automatically assign these responsibilities.

---

# 21. Dispute Model

A dispute requires:

- estate ID;
- claimant;
- concrete claim;
- affected item/rule;
- canonical relationship/governance context;
- resolution authority.

The estate system owns escrow and final transfer only.

---

# 22. Sensitive Cause Semantics

Use `murder`, `execution`, or `self-inflicted` only when an authoritative source explicitly determines that
cause.

Do not infer intent from:

- low mental health;
- weapon proximity;
- being alone;
- relationship conflict;
- generic injury.

Use `Unknown`/`Undetermined` when provenance does not support a stronger label.

---

# 23. Old-Save Migration

Default:

```text
existing living survivors -> unchanged
existing inventory -> unchanged
existing memorials -> preserved
historical death records -> not fabricated
wills -> empty
estates -> empty
disputes -> empty
```

Optionally migrate memorial-only historical stubs only if stable survivor IDs and dates exist.

No inheritance settlement is reconstructed for old deaths.

---

# 24. Cross-Campaign Boundary

E1-23 may export:

- survivor identity;
- death record ID;
- famous role;
- memorial status;
- major historical context.

E1-23 must not export:

- inherited gear into next campaign;
- heir stat bonuses;
- permanent skill inheritance;
- faction standing inheritance;
- shelter upgrades.

Those remain gated by E1-5.

---

# 25. UI Information Architecture

## Survivor will panel

- active will version;
- beneficiaries;
- eligible personal possessions;
- specific bequests;
- residue;
- validity.

## Death/memorial panel

- factual cause;
- date/location;
- witnesses where known;
- memorial;
- notable life facts.

## Estate panel

- pending/distributed/disputed;
- estate container contents;
- bequests;
- transfer receipts;
- unresolved claims.

## Dispute panel

- claimant;
- claim;
- property;
- mediation/governance status;
- final decision.

---

# 26. Exploit Matrix

| Exploit/failure | Guard |
|---|---|
| Reload duplicates death record | Death event ID |
| Reload executes will twice | Estate settlement operation ID |
| Item copied into heir + commons | Inventory transaction/estate container |
| Edit will after death | Frozen version |
| Bequeath communal stock | Ownership policy |
| Equip communal gear then die to privatize it | Borrowed/issued ownership metadata |
| Save-scum heir allocation | Deterministic allocation |
| Death quest farming | No high-value death-count rewards |
| Cross-beneficiary mass death duplicates estates | Simultaneous-death policy |
| Grief effect applied twice | Death event provenance |
| Leadership succession triggered twice | Death event ID |
| Historical old deaths create free estates | Prospective migration |

---

# 27. First Release Scope

Recommended first release:

1. normalized death event;
2. immutable death record;
3. memorial integration;
4. personal estate container;
5. one simple valid will;
6. one specific bequest;
7. residue-to-commons;
8. one intestate fallback;
9. psychology/quest/role/leadership event handoffs;
10. save/load and conservation selftest.

Defer disputes until this passes.

---

# 28. Headless Selftest

1. create survivor;
2. give personal canonical items;
3. create and update will;
4. cause death through canonical fixture authority;
5. verify SurvivorFate dead;
6. create one death record;
7. freeze active will version;
8. move property to estate container;
9. execute specific bequest;
10. send residue to commons;
11. verify Inventory conservation;
12. feed Memorial/Psychology/Quest/Role/Leadership adapters;
13. save/reload at each boundary;
14. assert no duplicate state.

---

# 29. Performance and Retention

Prefer:

- append-only compact death records;
- versioned wills with archival compaction;
- transaction references instead of duplicate item snapshots;
- indexed estate/dispute status;
- event-driven settlement;
- no per-frame ticking.

Avoid:

- full survivor snapshots per death;
- full inventory snapshots in death records;
- per-day legacy-effect ticking;
- unbounded duplicate journal/event history.

---

# 30. Rollback Strategy

### Wills
Disable new will creation; death records and commons fallback remain.

### Disputes
Disable independently; settle by deterministic fallback.

### Sentimental-item presentation
Disable independently.

### Historical memorial migration
Disable independently.

### Cross-campaign export
Disable independently.

Never roll back by copying Inventory, Psychology, Relations, Quest, Leadership, or SurvivorFate state into
E1-23.

---

# 31. Verification Matrix

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --death-legacy-selftest
```

Suggested targeted suites:

- `DeathAuthorityBoundaryTests`
- `DeathEventContractTests`
- `DeathCauseProvenanceTests`
- `DeathRecordTests`
- `DeathRecordIdempotencyTests`
- `WillVersioningTests`
- `WillValidityTests`
- `SpecialBequestTests`
- `EstateOpeningTests`
- `EstateOwnershipTests`
- `EstateInventoryConservationTests`
- `IntestatePolicyTests`
- `BeneficiaryEligibilityTests`
- `EstateRoundingTests`
- `SentimentalItemBoundaryTests`
- `InheritanceDisputeTests`
- `MemorialDeathIntegrationTests`
- `DeathPsychologyBoundaryTests`
- `DeathQuestBoundaryTests`
- `DeathLeadershipBoundaryTests`
- `DeathRoleClosureTests`
- `RemoteDeathEstateTests`
- `CombatLootEstateTests`
- `MassCasualtyEstateTests`
- `DeathMigrationTests`
- `DeathSaveRoundTripTests`
- `DeathExploitTests`
- `DeathPerformanceTests`

---

# 32. Completion Checklist

- [ ] Premise audit completed at current HEAD.
- [ ] Death/estate ownership ADR accepted.
- [ ] One normalized death event exists.
- [ ] Every supported cause maps to a canonical lethal owner.
- [ ] Death record is immutable/idempotent.
- [ ] Witness/location provenance is canonical.
- [ ] Last words remain optional narrative data.
- [ ] Wills are versioned and freeze at death.
- [ ] Will validity is deterministic.
- [ ] Personal versus communal ownership is explicit.
- [ ] Estate uses canonical Inventory container.
- [ ] Specific bequests preserve item instance identity.
- [ ] Intestate fallback is deterministic and conservative.
- [ ] Beneficiary lifecycle cases are defined.
- [ ] Category allocation cannot duplicate items.
- [ ] Sentimental significance is provenance-based or deferred.
- [ ] Distribution history references real Inventory transactions.
- [ ] Disputed items remain in escrow.
- [ ] Dispute resolution routes through conflict/governance.
- [ ] MemorialSystem remains memorial owner.
- [ ] Psychology/Needs remain grief/morale owners.
- [ ] Relations remain relationship owner.
- [ ] Quest/Personal Goal systems remain quest owners.
- [ ] Leadership/Governance remains succession owner.
- [ ] E1-20 closes roles.
- [ ] Remote/combat death property does not teleport.
- [ ] Mass-casualty ordering is stable.
- [ ] Old saves receive no fabricated estates.
- [ ] Existing memorial migration is conservative.
- [ ] No-RNG settlement passes.
- [ ] Inventory conservation passes.
- [ ] Sensitive cause wording review passes.
- [ ] Cross-campaign mechanical inheritance remains disabled.
- [ ] `E1_planintegration[24].md` is the next sequence filename.

---

# 33. Final Directive

Plan 206 should make death meaningful by preserving **truth, property, memory, and unfinished human context**.

When a survivor dies, the game should know exactly which canonical system caused the death, record that fact
once, preserve the survivor's historical identity, resolve what happened to real personal possessions, and
give Memorial, Psychology, Relations, Quests, Roles, and Leadership one consistent event to react to.

The player should never wonder whether a rifle vanished because the survivor died, whether the same heir got
two copies after reload, whether an old save invented a death history, or whether a death record guessed a
cause that the game never actually knew.

The architectural standard is:

**death authorities decide death; E1-23 records and settles its consequences; Inventory moves property;
Memorial and survivor systems carry the human aftermath.**

If `SurvivorDeathLegacySystem` starts deciding who dies, copying inventories, owning grief/trauma/relations,
completing quests, appointing leaders, or granting cross-campaign mechanical inheritance, stop and restore the
boundary.

---

# 35. Scenario Review Bank

For each scenario, document cause provenance, SurvivorFate ordering, estate ownership, will version, inventory
transactions, beneficiary policy, memorial/psychology/quest/leadership handoffs, save/load behavior, and a
negative duplicate-authority assertion.

1. A survivor dies in combat while also critically irradiated.
2. A survivor starves during an expedition with personal gear in a vehicle container.
3. A survivor updates a will one day before death.
4. A specifically bequeathed item was consumed before death.
5. A survivor attempts to bequeath communal shelter food.
6. A beneficiary dies in the same incident as the testator.
7. Two survivors designate each other as heirs and die simultaneously.
8. A beneficiary is missing at the time of settlement.
9. A unique weapon is disputed by two claimants.
10. A leader dies while holding a critical formal role.
11. A quest item is in the deceased survivor's personal container.
12. Combat enemies loot half the deceased survivor's gear.
13. A remote corpse/cache is not recovered until weeks later.
14. MemorialSystem already has a record from an old save but no canonical cause.
15. A death event is delivered twice around autosave.
16. A mass-casualty disaster kills several related survivors.
