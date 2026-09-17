
# D1 Flagship Integration Plan [3] — Plan 141: Research → Downstream Unlocks Bridge

> **Source plan:** Plan 141 — Research → Downstream Unlocks Bridge<br>
> **Primary defect:** `ResearchSystem` records completion and logs `breakthroughItem`, but the completion is terminal: no item grant, no recipe unlock, no expedition capability, no shelter upgrade, no combat doctrine, no medical procedure, and no completion event for downstream consumers.<br>
> **Flagship objective:** convert research from an isolated ledger into a deterministic, save-safe, data-authoritative progression spine whose effects are visible in every dependent gameplay domain.<br>
> **Delivery mode:** incremental integration, one downstream adapter at a time, with replay-safe unlock transactions, old-save reconstruction, headless selftests, catalog integrity gates, and UI explanations.<br>
> **Risk class:** MEDIUM-HIGH integration risk; LOW algorithmic risk. The difficult part is not computing unlocks. It is preventing duplicate grants, cross-system drift, stale saves, phantom unlocks, and invented content IDs.
>
> **Required execution order:** forensic inventory → contract/data authority → completion event → exactly-once unlock transaction → crafting → expedition → shelter → combat → medical → reverse-engineering parity → migration → UI → validation → CI → balance/follow-ons.
>
> **Hard rule:** do not introduce a second research truth. `ResearchSystem` remains authoritative for whether a node is complete. The bridge is authoritative only for translating completed research into downstream effects and recording which effects have been materialized.

---

## 0. Executive Integration Directive

The current research implementation has a classic "terminal subsystem" failure: the player invests resources and time, a completion record changes, a string is logged, and the rest of the game remains unchanged. That defect is more damaging than a missing feature because the UI can imply progression while the runtime provides none. The integration must therefore establish a formal boundary between **knowledge state** and **capability state**.

The target runtime pipeline is:

```text
Research completion
    ↓
canonical ResearchCompleted signal
    ↓
ResearchUnlockBridge resolves data-authoritative mappings
    ↓
idempotent unlock transaction
    ├─ physical prototype/item grant (only when explicitly modeled)
    ├─ crafting recipe availability
    ├─ expedition capability / encounter eligibility
    ├─ shelter upgrade availability or modifier
    ├─ combat doctrine / equipment capability
    └─ medical procedure / treatment capability
    ↓
persistent grant ledger + downstream domain state
    ↓
UI/journal notification + audit log
```

The bridge must never infer unlocks from display names, UI text, item-name substrings, or ad-hoc switch statements scattered across downstream systems. Unlock definitions belong in one validated data authority. Downstream systems consume typed capabilities through narrow interfaces. Old saves are reconstructed from completed research nodes so that a save created before this work immediately gains the capabilities it should already have earned.

The six source-plan breakthrough strings are the initial regression corpus. They are not permission to invent the remaining 24 mappings: the 30-mapping milestone is reached only with repository-validated source and target IDs.

---

## 1. Success Criteria

This plan is complete only when all of the following are simultaneously true:

1. Completing a research node produces at least one externally observable gameplay consequence when the catalog says it should.
2. Every unlock is deterministic for a given research-completion set.
3. Replaying the same completion event cannot duplicate items, capabilities, upgrades, journal entries, or notifications.
4. Save/load preserves both research completion and materialized unlock state.
5. An old save with completed research but no unlock ledger is repaired deterministically during migration.
6. Crafting, expedition, shelter, combat, and medical integrations are each independently testable.
7. `WorkshopReverseEngineeringSystem` uses the same unlock path rather than a parallel implementation.
8. Every research mapping resolves both its source research node and its target domain object.
9. Headless execution grants the same unlocks as interactive execution.
10. UI is a projection of runtime state; UI code never performs the grant.
11. No "breakthrough item" is silently treated as a physical inventory object unless the mapping explicitly declares that semantics.
12. A CI verb `--research-unlocks-selftest` fails on missing IDs, duplicate mappings, replay instability, or downstream delivery failures.
13. The initial six known breakthrough strings are covered by regression tests.
14. The 30-mapping milestone contains 30 validated mappings, not 30 speculative IDs.
15. Balance-sensitive extensions such as research failure, exclusive branches, trading, espionage, and synergy bonuses are layered after deterministic core integration, not mixed into the initial repair.

---

## 2. Non-Goals and Guardrails

This flagship plan deliberately separates the dead-end repair from optional systemic expansion.

### 2.1 Non-goals for the first integration slice

- Do not redesign the entire research tree.
- Do not rebalance research durations or costs before unlock delivery works.
- Do not add random research failure to the same commit that establishes deterministic unlock semantics.
- Do not make the UI the source of unlock state.
- Do not add speculative research nodes to satisfy a target count.
- Do not duplicate completed-research state inside the bridge.
- Do not let downstream systems read `research_tree.json` directly.
- Do not make recipe visibility depend on fragile string prefixes.
- Do not convert every breakthrough string into a physical item by default.
- Do not silently mutate saves without schema/version metadata and migration tests.

### 2.2 Mandatory guardrails

- **Exactly once:** an unlock transaction has a stable identity.
- **Source of truth:** research completion comes from `ResearchSystem`.
- **Data authority:** source→unlock mapping is centralized and validated.
- **Typed downstream sinks:** no giant `switch (unlockType)` reaching deep into private internals if a narrow adapter can own the boundary.
- **Migration replay safety:** reconstructing unlocks twice produces no additional effects.
- **No RNG in grants:** research completion determines unlock set purely.
- **Explicit physical grants:** inventory mutation requires a mapping flag and quantity.
- **Observability:** every applied, skipped, rejected, and migrated unlock is diagnosable.
- **Headless parity:** no dependence on panel initialization, scene presence, or input.
- **Catalog resolution:** every target ID must resolve before the build passes data integrity.

---

## 3. Repository Evidence Baseline and Forensic Questions

The source plan establishes the following facts to verify before editing:

- `Assets/Ashfall.Core/Research/ResearchSystem.cs` completes nodes and logs `def.breakthroughItem`.
- The completion path does not grant the item, unlock recipes, or raise a downstream event.
- `WorkshopReverseEngineeringSystem` has the same dead-end pattern.
- `CraftingSystem`, `ExpeditionSystem`, shelter systems, combat systems, and medical systems are intended downstream targets.
- `Assets/StreamingAssets/Data/research_tree.json` is the research data authority or is being moved toward that role by prior plans.
- Existing saves may contain completed research with no materialized downstream effects.

The implementing agent must answer these forensic questions before writing the bridge:

1. What exact type represents a research node definition?
2. What exact collection/state marks a node complete?
3. Is completion synchronous, event-based, or command-based?
4. Can a node complete more than once due to restore/replay?
5. Does the research system already expose an event that should be reused?
6. Is `breakthroughItem` a schema field on every node or optional?
7. Do the six breakthrough item IDs resolve in the item catalog today?
8. Does `CraftingSystem` already have an unlock/availability concept, or are recipes currently globally visible?
9. Does expedition content expose capability tags, destination locks, encounter predicates, or another gate mechanism?
10. How are shelter upgrades represented: build recipes, room definitions, modifiers, flags, or service calls?
11. Does combat have a doctrine catalog, tactics list, equipment permission, or only raw stat systems?
12. Does medical have procedure IDs and an eligibility layer, or must a narrow capability registry be introduced?
13. How are world flags persisted and could they already serve as a capability substrate?
14. What save schema versioning mechanism is canonical in this repository?
15. What composition/bootstrap mechanism should own bridge construction after the composition-root work?
16. How are catalog loaders registered and validated?
17. How are UI panels notified of state changes without owning gameplay state?
18. What selftest verb pattern is canonical in `HostCli.cs`?
19. What determinism/hash helpers already exist and should be reused?
20. What old-save fixtures exist for migration tests?

The output of this audit is a short implementation note, not a new architecture document. Any discovered existing primitive should replace the equivalent proposed primitive below rather than being duplicated.

---

## 4. Target Architecture

### 4.1 Core types

The preferred logical contract is:

```csharp
public enum ResearchUnlockType
{
    InventoryItem,
    Recipe,
    ExpeditionCapability,
    ExpeditionDestination,
    ShelterUpgrade,
    ShelterCapability,
    CombatDoctrine,
    CombatEquipmentCapability,
    MedicalProcedure,
    MedicalCapability,
    WorldCapability
}

public sealed record ResearchUnlockDefinition(
    string Id,
    string ResearchNodeId,
    ResearchUnlockType Type,
    string TargetId,
    string DescriptionKey,
    int Quantity = 0,
    bool GrantOnCompletion = true,
    bool Retroactive = true);

public sealed record GrantedResearchUnlock(
    string UnlockId,
    string ResearchNodeId,
    ResearchUnlockType Type,
    string TargetId,
    long Sequence);

public interface IResearchUnlockSink
{
    ResearchUnlockType Type { get; }
    ResearchUnlockApplyResult Apply(ResearchUnlockContext context);
}
```

Names are proposals; reuse repository naming conventions if equivalents exist. The semantics are the important part: stable unlock ID, source research node, typed target, optional quantity, retroactivity policy, and an idempotent application boundary.

### 4.2 State ownership

`ResearchSystem` owns:

- node definitions;
- prerequisites;
- progress;
- completed node IDs;
- research cost/time semantics.

`ResearchUnlockBridge` owns:

- resolving completed node IDs to unlock definitions;
- orchestration across sinks;
- exactly-once grant ledger;
- pending/retryable unlocks where a downstream subsystem is temporarily unavailable;
- capture/restore of bridge-owned materialization state;
- migration/reconciliation.

Downstream domains own:

- their actual recipe/capability/upgrade/procedure state;
- domain-specific validation;
- domain-specific save representation where already established.

The bridge must not become a second crafting system, expedition system, shelter system, combat system, or medical system.

### 4.3 Transaction identity

Every unlock definition needs a stable `unlockId`, preferably data-authored. If the existing data pipeline strongly discourages a separate ID, derive one deterministically from `(researchNodeId, unlockType, targetId)` and test collision freedom. Never use array index as identity.

### 4.4 Application states

Each definition should resolve to one of:

```text
NotApplicable     mapping disabled by data/version
Pending           eligible but sink unavailable or prerequisite unmet
Applied           downstream effect materialized
AlreadyApplied    replay-safe no-op
Rejected          invalid target/data and must fail validation
```

`Rejected` is not a runtime "keep going" state for shipped data. CI should prevent it from reaching production.

---


## 141-P0 — Forensic Reconnaissance and Authority Freeze

**Goal:** Prove the actual completion, save, catalog, bootstrap, and downstream extension points before any bridge code is introduced.

### Execution substeps

1. Read `ResearchSystem.cs` end-to-end, not only the completion lines. Record the completion mutation, prerequisite check, progress update, callback/event surface, capture state, restore state, and any replay behavior.
2. Read `WorkshopReverseEngineeringSystem` and identify whether it directly mutates research completion or calls `ResearchSystem`. The final architecture must route both completion sources through one canonical publication path.
3. Resolve all six existing `breakthroughItem` strings in the item/catalog loaders. Classify each as a real inventory definition, a placeholder, a capability-shaped item, or an unresolved ID.
4. Inspect `CraftingSystem` for recipe visibility, recipe discovery, requirements, persistent unlocks, and save semantics. Prefer extending an existing recipe-availability primitive over adding another registry.
5. Inspect expedition gating at destination, encounter, route, and option levels. Identify the narrowest stable capability predicate that research can satisfy.
6. Inspect shelter upgrade APIs and distinguish 'upgrade becomes buildable' from 'upgrade is immediately installed'. Research should usually unlock construction, not bypass its material/resource cost.
7. Inspect combat doctrine/tactic/equipment eligibility structures. Do not invent a doctrine framework if combat lacks one; create a capability hook only when needed by validated content.
8. Inspect `MedicalPipelineCoordinator` and medical catalogs for procedure availability and treatment eligibility. Preserve existing medical safety/eligibility checks.
9. Inspect the composition root, save coordinator, host CLI/selftest registry, data-integrity validators, and event infrastructure. Reuse all canonical registration patterns.
10. Write a compact audit table: component, current authority, extension method/event, save owner, catalog owner, headless availability, required adapter. Commit it with the implementation or include it as a code-adjacent doc.

**Integration note:** No production behavior changes in this package. The output is the verified integration map that prevents a speculative parallel architecture.

### Required tests

- Audit test: every proposed target interface maps to an actual class or an explicitly approved new adapter.
- Catalog smoke: six breakthrough IDs are reported as resolved/unresolved with no silent assumptions.
- Bootstrap smoke: identify construction order and prove the bridge can be created without UI scenes.

**Definition of done:** The team can point to one canonical completion path and one concrete integration point per downstream domain.

---

## 141-P1 — Research Unlock Data Authority

**Goal:** Create a single, schema-validated mapping authority from completed research nodes to downstream unlock definitions.

### Execution substeps

1. Choose the data location consistent with the repository's catalog conventions, e.g. `Assets/StreamingAssets/Data/research_unlocks.json` or a nested field in the externalized research tree if that is the established authority.
2. Define schema fields for stable unlock ID, research node ID, unlock type, target ID, localized description key, quantity where applicable, retroactive policy, and optional prerequisite capability list.
3. Seed the authority with the six repository-proven breakthrough relationships first. Preserve the original breakthrough strings so the migration can reconcile historical data.
4. Do not fill the 30-entry target with invented IDs. Add a validation report that shows `validatedMappings`, `unresolvedMappings`, and `remainingAuthoringSlots` until the catalog audit supplies real targets.
5. If one research node grants multiple effects, model multiple unlock definitions sharing the same `researchNodeId`; do not encode compound effects in a magic target string.
6. Treat physical item grants separately from capability unlocks. A recipe unlock must not implicitly grant its crafted item. A shelter upgrade unlock must not silently install the upgrade unless that is explicitly intended.
7. Add a catalog loader that normalizes IDs, rejects blank strings, rejects unknown enum values, rejects duplicate unlock IDs, and preserves deterministic order.
8. Validate that every source research node exists. Validate every target using the target domain's catalog/index rather than a generic string existence check.
9. Expose an immutable lookup `researchNodeId -> ordered unlock definitions` to the bridge. Do not expose mutable data collections.
10. Add a generated or selftest-readable mapping report so CI can show precisely which node unlocks which targets.
11. Document schema evolution and require an explicit schema version if the repository's data pipeline supports it.

**Integration note:** This package creates the authority that replaces `breakthroughItem` as an isolated log-only hint. The legacy field may remain during migration, but it cannot be the only downstream contract.

### Required tests

- Loader rejects duplicate `unlockId`.
- Loader rejects unknown `researchNodeId`.
- Loader rejects unknown target IDs by domain.
- Lookup order is deterministic across loads.
- Six known breakthrough mappings are represented and testable.

**Definition of done:** A single validated catalog can answer 'what changes when node X completes?' without reading UI code or switch statements.

---

## 141-P2 — Canonical Research Completion Publication

**Goal:** Publish research completion exactly once from the authoritative mutation point so every completion source shares one downstream signal.

### Execution substeps

1. Locate the exact line at which a research node transitions from incomplete to complete. Publish only after the state mutation succeeds.
2. Introduce or reuse a typed `ResearchCompleted` event carrying node ID and minimal deterministic context. Avoid passing mutable system objects or UI references.
3. Ensure the event is not raised when loading already-completed state unless migration/reconciliation explicitly requests replay.
4. Ensure repeated completion commands for the same node return an already-complete result without publishing a second completion.
5. Route `WorkshopReverseEngineeringSystem` through the same completion API whenever possible. If it must remain separate, it must invoke the same completion publisher after the authoritative state mutation.
6. Do not let `ResearchUnlockBridge` poll every frame for completions. Event delivery plus explicit reconciliation on restore is simpler and deterministic.
7. Define event ordering relative to research-completion journal entries and UI notifications. Gameplay unlock application should occur before the UI claims the capability is available.
8. Headless hosts must construct the publisher and bridge identically to interactive hosts.
9. Add a small event-sequence test that proves completion mutation precedes publication and unlock application precedes presentation notification.

**Integration note:** The completion event is an integration seam, not a replacement state store.

### Required tests

- Single completion emits one signal.
- Duplicate completion emits zero additional signals.
- Reverse-engineering completion emits the same signal shape.
- Restore does not accidentally emit live completion signals.
- Headless completion path publishes normally.

**Definition of done:** All genuine research completions converge on one event contract.

---

## 141-P3 — Idempotent Unlock Transaction and Grant Ledger

**Goal:** Apply all unlock effects with exactly-once semantics and preserve enough state to reconcile saves safely.

### Execution substeps

1. Create `ResearchUnlockBridge` with injected catalog, sink registry, logging/telemetry abstraction, and any required domain services.
2. For each completion, resolve unlock definitions in deterministic order and process each by stable `unlockId`.
3. Check the bridge grant ledger before applying. If already granted, return `AlreadyApplied` and perform no downstream mutation.
4. For a new unlock, validate the sink exists and target resolves; invoke the typed sink; append to the grant ledger only after a successful application or confirmed domain-level already-present state.
5. If a sink is temporarily unavailable during composition/restore, store a pending unlock rather than marking it granted. Retry pending entries after all services restore.
6. Never retry structurally invalid mappings at runtime. Invalid data is a CI/data-integrity failure.
7. Capture granted unlock IDs and pending unlock IDs using the repository's save-version conventions. Preserve deterministic ordering for snapshot stability.
8. Restore must not reapply an effect just because the bridge ledger was loaded after a downstream system. Reconciliation happens in a deliberate post-restore phase.
9. Expose a pure query for `IsGranted(unlockId)` and `GetUnlocksForResearch(nodeId)` for UI/reporting.
10. Record diagnostic reason codes for Applied, AlreadyApplied, Pending, and Rejected outcomes.
11. Protect item grants against duplication: ledger check first, domain transaction second, ledger commit third, with explicit recovery behavior if save capture occurs between boundaries.
12. If the repository has a general transaction/unit-of-work abstraction, integrate with it; do not create a competing pseudo-transaction framework.

**Integration note:** The grant ledger records materialization, not research completion. It is intentionally derivable from research state plus mapping data for migration and integrity repair.

### Required tests

- Replay same event 100 times produces one grant.
- Save/load/replay produces no duplicate inventory item.
- Downstream already-unlocked state is treated as converged, not duplicated.
- Pending unlock is retried once dependencies are available.
- Capture/restore round trip preserves ledger exactly.

**Definition of done:** Research unlock delivery is replay-safe and save-safe.

---

## 141-P4 — Crafting Adapter

**Goal:** Make research unlock real recipes without coupling `CraftingSystem` to research internals.

### Execution substeps

1. Identify the existing recipe availability mechanism. If recipes are globally available today, introduce the smallest explicit availability layer required for research-gated recipes.
2. Implement a crafting unlock sink/adapter that receives `Recipe` unlock definitions and calls a stable crafting API such as `UnlockRecipe(recipeId)`.
3. Persist recipe availability in the crafting domain if that is already its ownership model; otherwise derive visibility from the bridge/capability registry consistently.
4. Separate 'recipe is known' from 'ingredients are owned'. A researched recipe can be visible but not craftable.
5. Do not require a breakthrough item as an ingredient merely because its ID exists. Ingredient requirements must come from the recipe definition. Research grants knowledge; material recipes define material costs.
6. For `research_water_purification_advanced`, validate that `recipe_water_filter_advanced` actually exists before enabling the mapping.
7. For gas-mask and HEPA examples, resolve exact recipe IDs from repository data and update mappings only after validation.
8. Update crafting UI filtering so locked research recipes are absent or clearly locked according to existing UX policy, and newly unlocked recipes appear without scene reload.
9. Expose the research source in recipe details if useful: 'Unlocked by <research title>' should be data-driven/localizable.
10. Add reverse tests: completing unrelated research must not alter recipe availability.

**Integration note:** Crafting is the first downstream adapter because it provides a simple, visible proof that research changes gameplay without requiring world-state side effects.

### Required tests

- Recipe appears after completion.
- Recipe remains locked before completion.
- Recipe unlock survives save/load.
- Duplicate completion does not duplicate UI entries.
- Missing recipe ID fails data integrity.
- Unrelated node does not unlock recipe.

**Definition of done:** At least one real research node unlocks one real recipe end-to-end in headless and interactive tests.

---

## 141-P5 — Expedition Capability Adapter

**Goal:** Translate research into expedition options without hard-coding research checks inside expedition generation.

### Execution substeps

1. Inventory expedition gates: destination visibility, encounter eligibility, route actions, loot interactions, radio/intelligence actions, and party equipment checks.
2. Prefer a generic capability query such as `HasCapability(capabilityId)` at the expedition boundary over `if researchNode == ...` checks.
3. Implement `ExpeditionCapability` and, only if repository structure supports it, `ExpeditionDestination` unlock types separately.
4. For crypto analysis, use a validated capability such as the repository's actual encrypted-cache/intercept ID. The source plan's 'encrypted cache raids' is an intended example, not proof that a destination ID exists.
5. Require expedition content definitions to declare capability requirements in data where possible. This keeps research knowledge out of encounter code.
6. Ensure capability gating affects both UI discovery and headless encounter selection so the player cannot select an option the simulation will later reject.
7. Decide whether research reveals a destination immediately or only makes it discoverable. Encode that distinction as separate unlock semantics rather than one ambiguous flag.
8. Do not bypass expedition resource, survivor, weather, route, or hazard requirements when a capability is unlocked.
9. Add a deterministic encounter-selection fixture that compares the eligible option set before and after the research capability.
10. Persist any domain-owned discovery state separately from the research capability where necessary.

**Integration note:** Research should expand what an expedition can understand or attempt; it should not magically complete expedition content.

### Required tests

- Encrypted/capability option absent before unlock.
- Option eligible after unlock.
- Other expedition gates still apply.
- Save/load retains capability.
- Headless selection matches UI eligibility.
- Unknown capability target fails validation.

**Definition of done:** A research completion changes at least one genuine expedition decision path through a data-driven capability predicate.

---

## 141-P6 — Shelter Upgrade Adapter

**Goal:** Connect research to buildable shelter improvements while preserving construction cost, installation state, and existing shelter simulation ownership.

### Execution substeps

1. Classify shelter research effects into `upgrade available`, `capability modifier`, and `immediate passive effect`. Default to 'available', not 'installed'.
2. Resolve radiation shielding, solar power, and air filtration targets against actual shelter upgrade/room/system definitions.
3. Implement a shelter unlock adapter that marks validated upgrades as available through the shelter domain's API.
4. Keep resource payment, worker assignment, construction time, maintenance, damage, and power requirements inside shelter systems.
5. If an effect is a persistent efficiency modifier rather than an upgrade object, model it as a named shelter capability and apply it through an existing modifier stack.
6. Prevent double application of multiplicative/additive modifiers on replay by keying them with stable unlock IDs.
7. For solar power, distinguish knowledge of an inverter from possession of an inverter and from installation of a solar upgrade. The mapping may grant a prototype item and unlock an upgrade, but those are separate definitions.
8. For radiation shielding, define exactly what changes: upgrade availability, room mitigation, or construction recipe. Do not silently apply all three.
9. For HEPA/air filtration, ensure the research effect composes with contamination/air-quality simulation rather than overwriting it.
10. Add before/after state snapshots showing that research changes availability but does not bypass material constraints unless the data explicitly says so.

**Integration note:** This adapter must preserve the shelter simulation's causality: research opens a technical option; construction and operation still cost something.

### Required tests

- Upgrade unavailable before research.
- Upgrade available after research.
- Research alone does not install upgrade unless explicitly configured.
- Modifier applies once.
- Power/maintenance constraints still run.
- Target ID validation catches missing shelter definitions.

**Definition of done:** Real shelter capabilities become reachable through research with no replay amplification.

---

## 141-P7 — Combat Doctrine and Equipment Capability Adapter

**Goal:** Expose research-derived combat knowledge through a narrow capability layer without inventing unsupported tactical systems.

### Execution substeps

1. Audit whether `TacticalCombatSystem` already has doctrine, stance, tactic, maneuver, equipment-permission, or modifier abstractions.
2. If a doctrine abstraction exists, implement a `CombatDoctrine` sink that unlocks an existing doctrine ID.
3. If no doctrine abstraction exists, create a minimal combat capability registry used only by validated combat features; do not build a speculative doctrine tree in this plan.
4. Treat `research_tactical_analysis` from the source plan as a proposed example until the node and target IDs are verified in the repository.
5. Keep weapon ownership and ammo requirements separate from capability permission. Unlocking an equipment upgrade does not grant the weapon unless a separate inventory unlock says so.
6. Ensure AI combatants do not accidentally receive player/shelter research capabilities unless faction research intentionally uses the same registry.
7. Expose capability requirements in combat action data where possible rather than checking research IDs in action code.
8. Add deterministic action-set tests comparing available tactics before and after capability grant.
9. Verify modifiers are keyed and non-stacking on replay.
10. Document any combat content gap discovered during the audit as a follow-on task instead of masking it with placeholder IDs.

**Integration note:** The dead-end repair should connect to real combat primitives only. A missing doctrine framework is evidence for a separate plan, not justification to invent one inside the bridge.

### Required tests

- No combat change before unlock.
- Validated tactic/doctrine appears after unlock.
- Duplicate event does not stack modifiers.
- Equipment still requires actual possession.
- Enemy/faction capability scope remains isolated.
- Unknown doctrine target fails integrity.

**Definition of done:** Research can enable at least one repository-backed combat capability, or the plan explicitly records that the combat adapter remains dormant pending a separate validated content/framework task.

---

## 141-P8 — Medical Procedure Adapter

**Goal:** Make medical research unlock treatments/procedures while preserving diagnosis, resource, skill, contraindication, and treatment-pipeline rules.

### Execution substeps

1. Audit `MedicalPipelineCoordinator` for procedure catalogs, treatment eligibility, diagnosis requirements, skill checks, medication/item requirements, and persistence.
2. Implement a `MedicalProcedure` sink that marks an existing procedure/capability as available without performing treatment.
3. Treat `research_radiation_therapy` as an example until source and procedure IDs are verified.
4. Keep diagnosis and survivor-condition gates authoritative. Research availability must not let the player execute a treatment on an ineligible patient.
5. Keep item/medication consumption inside the medical pipeline. Unlocking a procedure does not grant consumables.
6. If research improves effectiveness rather than unlocking a procedure, model a keyed medical capability/modifier so replay cannot stack it.
7. Expose locked-procedure explanations in UI through localized requirement metadata, not hard-coded English.
8. Add headless tests that attempt a procedure before research, after research with missing materials, and after research with all normal requirements satisfied.
9. Verify save/load retains procedure availability and does not apply treatment during restore.
10. Ensure medical procedure unlocks cannot be confused with survivor-specific learned skills unless the game explicitly models research as institutional knowledge.

**Integration note:** Institutional research should open what the shelter knows how to do; patient eligibility and consumables remain downstream responsibilities.

### Required tests

- Procedure unavailable before research.
- Procedure available after research.
- Missing materials still block treatment.
- Wrong diagnosis still blocks treatment.
- Duplicate unlock does not multiply efficacy.
- Unknown medical target fails integrity.

**Definition of done:** At least one validated medical procedure or capability is research-gated end-to-end, or a documented content gap blocks activation without compromising the bridge.

---

## 141-P9 — Physical Breakthrough Item Semantics

**Goal:** Resolve the source plan's six `breakthroughItem` strings without turning research into accidental item duplication.

### Execution substeps

1. For each existing breakthrough string, determine whether the item definition represents a prototype, component, quest token, capability proxy, or ordinary craftable item.
2. Add an explicit mapping field or unlock type for physical grants. Never infer 'grant to inventory' solely because a research definition contains `breakthroughItem`.
3. If a breakthrough is meant to grant one prototype, set explicit quantity=1 and stable unlock ID. The grant ledger then guarantees exactly one prototype per research completion across replay/save/load.
4. If the item is really a capability marker and should not live in inventory, migrate the data toward a capability target while retaining compatibility with the legacy field.
5. If the item should become craftable instead of immediately granted, unlock its recipe and do not inject inventory.
6. Validate capacity/weight behavior when granting a physical prototype. Define whether the grant can overflow inventory, goes to shelter storage, or becomes pending until storage is available.
7. Do not silently drop an earned prototype when storage is full. Use the repository's canonical overflow/mailbox/staging mechanism or keep the grant pending.
8. Add an audit report listing each of the six strings and its chosen semantics, target, quantity, and downstream consumer.
9. Deprecate the log-only use of `breakthroughItem` after all six have a validated interpretation.

**Integration note:** This package converts a vague legacy string into explicit gameplay semantics and closes the most direct bug cited by the source plan.

### Required tests

- Each known breakthrough string has a classified semantics.
- Physical grant occurs once.
- Full inventory does not lose the unlock.
- Capability-only mapping does not create junk inventory.
- Legacy saves reconcile correctly.

**Definition of done:** All six known breakthrough strings have an explicit, tested downstream meaning.

---

## 141-P10 — Workshop Reverse Engineering Parity

**Goal:** Ensure reverse engineering is a second source of research completion, not a second unlock implementation.

### Execution substeps

1. Trace the current reverse-engineering completion flow and determine whether it marks research complete, awards a breakthrough string, or maintains separate progress.
2. Refactor it to call the canonical research completion command when semantically equivalent.
3. If reverse engineering can unlock capabilities without completing a formal node, model that as a distinct `ResearchUnlockSource` only if repository design requires it; still route through the same bridge transaction path.
4. Remove duplicate logging/grant code after parity is established.
5. Ensure reverse engineering cannot grant a node's unlocks and then later research completion grant them again.
6. Preserve any reverse-engineering-specific costs, item destruction, skill checks, and failure outcomes upstream of completion.
7. Add tests for research-first then reverse-engineer, reverse-engineer-first then research, and repeated reverse-engineering attempts.
8. Ensure save migration can recognize historical reverse-engineering completions if they were stored separately.

**Integration note:** Two acquisition pathways may exist, but one materialization mechanism must own downstream effects.

### Required tests

- Research-first/reverse-second = one unlock set.
- Reverse-first/research-second = one unlock set.
- Repeated reverse engineering does not duplicate grants.
- Distinct costs remain intact.

**Definition of done:** Reverse engineering and normal research converge on identical unlock outcomes for the same completed knowledge.

---

## 141-P11 — Research Tiers, Prerequisites, and Synergy Boundaries

**Goal:** Add progression structure only after the core bridge works, and keep eligibility separate from grant materialization.

### Execution substeps

1. Preserve research-node prerequisites inside `ResearchSystem`; do not move research eligibility into the unlock bridge.
2. If unlock definitions require multiple completed nodes, represent that as an explicit condition evaluated from authoritative research state.
3. Define tier metadata for UI/balance if the repository already models tiers or if the 30-mapping authoring pass needs them.
4. Do not make tiers the identity of unlocks. Stable unlock IDs and target IDs remain independent from presentation tiers.
5. For multi-node synergy unlocks, use a stable synthetic unlock definition whose prerequisites list all required node IDs.
6. Reconciliation must evaluate synergies after restore so an old save with all prerequisite nodes gains the synergy once.
7. Exclusive research branches belong in research eligibility, not grant rollback. Once a capability is legitimately granted, removing it requires an explicit design and migration policy.
8. The source example 'all medical research unlocks Chief Medical Officer trait' must be treated as optional design work; verify whether traits are survivor-owned or institutional before implementing.
9. Add exhaustive boundary tests for one-of-N prerequisites, all-of-N prerequisites, and branch exclusivity if those mechanics are activated.

**Integration note:** This package prevents progression rules from leaking into every downstream adapter.

### Required tests

- Prerequisite-incomplete node cannot complete.
- Completed prerequisite set grants synergy once.
- Restore recomputes missing synergy safely.
- Branch rules do not revoke unrelated materialized unlocks.

**Definition of done:** Complex research dependencies can be expressed without changing downstream domains.

---

## 141-P12 — Research Failure Mechanics Isolation

**Goal:** Keep optional RNG/failure mechanics from contaminating deterministic unlock delivery.

### Execution substeps

1. Do not implement random failure until deterministic completion→unlock integration is green.
2. If research failure is later activated, apply RNG before the authoritative completion transition. Failed attempts must not publish `ResearchCompleted`.
3. Use the repository's deterministic RNG/seed service so headless tests can reproduce failure outcomes.
4. A failed attempt may consume research resources only according to explicit data; the unlock bridge never owns those costs.
5. Critical-failure hazards or afflictions must be routed to existing hazard/medical systems through their own contracts, not encoded as 'negative unlocks' inside the bridge unless a formal reversible-capability system exists.
6. Retrying failed research must preserve progress/cost semantics without producing completion events until success.
7. Expose failure outcome data for UI/journal separately from successful breakthrough notifications.
8. Add seeded tests proving the same seed gives the same attempt outcome and that failure produces zero downstream grants.

**Integration note:** The source plan proposes research failure; this is a follow-on layer, not part of the dead-end repair's critical path.

### Required tests

- Failed attempt emits no completion.
- Failed attempt grants nothing.
- Seeded outcomes reproduce.
- Successful retry grants once.

**Definition of done:** Optional failure mechanics can be enabled without changing unlock determinism.

---

## 141-P13 — Research Trading, Faction Sharing, Theft, and Captured Knowledge

**Goal:** Route external knowledge acquisition through explicit research/capability contracts without weakening save or provenance semantics.

### Execution substeps

1. Inventory faction/diplomacy systems for knowledge-sharing primitives before adding any new trade type.
2. Define whether traded research completes a node, adds progress, grants a one-off capability, or provides a research-cost discount. Do not conflate these outcomes.
3. If a faction transfers a complete research node, call the canonical completion command with source metadata such as `FactionTrade` while preserving identical unlock effects.
4. If stolen data is incomplete, store progress in `ResearchSystem`; do not grant downstream capabilities early.
5. Captured enemy research that is reverse-engineered should reuse P10 parity.
6. Record acquisition provenance only for journal/telemetry/audit. Provenance must not change deterministic unlock content unless data explicitly says so.
7. Prevent repeated purchases/thefts of the same completed node from duplicating downstream benefits.
8. Gate this package behind real diplomacy/espionage APIs; otherwise author follow-on tasks rather than creating fake systems.

**Integration note:** This turns the source plan's trading idea into a safe extension point rather than a new parallel progression ledger.

### Required tests

- Trade-completed node unlocks once.
- Partial intel does not unlock early.
- Repeated purchase is idempotent.
- Provenance survives save/load if recorded.

**Definition of done:** External research acquisition, when supported by real systems, converges on the same completion pipeline.

---

## 141-P14 — Save Schema, Restore Ordering, and Old-Save Migration

**Goal:** Guarantee compatibility for saves created before downstream unlock materialization existed.

### Execution substeps

1. Add bridge state to the canonical save graph using the repository's schema/version conventions.
2. Capture stable granted unlock IDs, pending unlock IDs, and bridge schema version. Avoid persisting redundant copies of target descriptions or mutable catalog data.
3. Define restore ordering explicitly: restore research completion → restore downstream domains → restore bridge ledger → run reconciliation → refresh presentation.
4. For old saves with no bridge state, enumerate authoritative completed research nodes and resolve all retroactive mappings.
5. Apply each missing unlock through normal sink idempotency. Do not directly mutate serialized DTOs in migration code when a domain API exists.
6. For old saves where a downstream capability is already present for another historical reason, mark the corresponding unlock converged without duplicating it.
7. If mapping data changes between versions, use stable unlock IDs and schema migration rules. Never rely on list index.
8. Define behavior for removed mappings: retain historical granted capability unless an explicit compatibility migration says removal is safe.
9. Define behavior for renamed target IDs through data migration/alias support rather than silently losing capabilities.
10. Create fixtures for: pre-bridge save, partial bridge save, fully migrated save, corrupted unknown unlock ID, completed research with missing target, and all-research save.
11. Run migration twice and assert byte/semantic stability after the first reconciliation.
12. Log migration summary counts without flooding per-frame logs.

**Integration note:** Old-save repair is a first-class requirement because the source plan explicitly calls for retroactive grant of past research.

### Required tests

- Pre-bridge save gains all valid retroactive unlocks.
- Second migration is a no-op.
- Unknown legacy unlock is reported safely.
- Removed/renamed mapping behavior is deterministic.
- All-research save reaches all validated unlocks.
- Save round trip preserves exact grant set.

**Definition of done:** Every supported historical save converges to the same capability state as a fresh playthrough with the same completed research.

---

## 141-P15 — Composition Root and Lifecycle Wiring

**Goal:** Wire the bridge once, in the correct dependency order, without introducing hidden singletons or UI-owned construction.

### Execution substeps

1. Construct the unlock catalog before the bridge.
2. Construct/inject downstream sink adapters from already-composed gameplay systems.
3. Construct the bridge after its sinks exist but before live research completion can occur.
4. Subscribe the bridge to the canonical completion publisher exactly once; provide disposal/unsubscription if lifecycle patterns require it.
5. Register bridge capture/restore with the save coordinator in the same composition root.
6. Add an explicit post-restore reconciliation hook after all dependent systems finish restoration.
7. Expose only query/read surfaces needed by UI and diagnostics.
8. Do not let `GameBootstrap` manually call domain-specific unlock methods after research completion; all effects go through the bridge.
9. Audit test-mode and headless composition so no dependency is omitted because a scene/UI object is absent.
10. Add a composition selftest asserting one bridge, one subscription, expected sink set, and no duplicate registration.

**Integration note:** The source plan names `SetupResearchUnlockBridge`/`SaveResearchUnlocks`; use current composition-root conventions rather than reintroducing legacy Setup sprawl if those conventions have already evolved.

### Required tests

- One bridge instance in runtime graph.
- One completion subscription.
- All required sinks registered.
- Headless composition succeeds.
- Restore reconciliation executes after dependencies.

**Definition of done:** The research bridge is part of canonical composition and save lifecycle.

---

## 141-P16 — UI, Journal, and First-Breakthrough Explanation

**Goal:** Make downstream consequences legible without letting presentation drive state.

### Execution substeps

1. Expose a bridge query that returns granted/pending unlocks by research node with localized description keys.
2. Update the research panel so a completed node shows the concrete capabilities it unlocked, not only a breakthrough string.
3. Show pending states only when they represent meaningful player-actionable conditions such as storage full; do not expose internal composition retries.
4. Update crafting/expedition/shelter/combat/medical panels through their normal state refresh paths when capabilities change.
5. Create one consolidated breakthrough notification per research completion, listing multiple effects when a node grants more than one.
6. Write the research journal entry after the transaction result is known so it cannot claim an unlock that failed validation.
7. Persist journal entries according to the existing journal authority; do not duplicate them in bridge save state unless the journal system already requires explicit append.
8. Implement the first-research explanation as an onboarding hint using the existing guidance/tutorial framework, not a bespoke modal subsystem.
9. Localize unlock descriptions and requirement messages. IDs never appear directly to players.
10. Ensure keyboard/controller navigation and text scaling continue to work in expanded research panels.
11. Add snapshot/UI tests for locked, newly unlocked, multi-unlock, pending, and migrated states.

**Integration note:** The player-facing requirement is simple: after research completes, the player can immediately understand what became possible and where to use it.

### Required tests

- Completion panel lists actual effects.
- No duplicate notification on reload.
- Migrated unlocks do not spam first-time popups unless policy explicitly wants a migration summary.
- Journal matches grant results.
- UI refreshes without scene reload.

**Definition of done:** Research consequences are visible, localizable, accessible, and accurately reflect runtime state.

---

## 141-P17 — 30-Mapping Authoring Campaign

**Goal:** Reach the source plan's 30-mapping target through repository-backed authoring rather than speculative identifiers.

### Execution substeps

1. Generate an inventory of all research nodes from `research_tree.json` with current completion status fields, breakthrough fields, tier/prerequisite metadata, and display names.
2. Generate inventories of candidate target IDs from recipes, expedition capabilities/destinations, shelter upgrades, combat doctrines/capabilities, and medical procedures.
3. Create a mapping worksheet/report with columns: source node, candidate domain, target ID, target existence, intended effect, grant mode, retroactive, balance note, test fixture.
4. Approve the six known breakthrough-related mappings first.
5. Populate additional mappings only when both source and target exist and the gameplay relationship is intentional.
6. Prefer broad distribution across all downstream domains rather than 30 crafting recipes that technically satisfy the count but fail the cross-system goal.
7. Flag research nodes with no meaningful downstream target as content/design gaps; do not invent dummy capability tokens merely to make every node non-empty.
8. Flag downstream targets that should be research-gated but currently are globally available; migrate their availability in a separate, testable change.
9. Run a reachability check ensuring every mapped target can actually be used after unlock under at least one valid game state.
10. Stop when 30 mappings are validated. If the repository cannot support 30 without fabrication, record the shortfall and create concrete content tasks; correctness outranks the numeric target.

**Integration note:** The mapping campaign is an authoring/validation exercise built on the bridge, not part of the bridge's runtime complexity.

### Required tests

- Exactly N validated mappings reported, targeting 30.
- No unresolved source IDs.
- No unresolved target IDs.
- Every mapping has at least one end-to-end test or reachability probe.
- Domain distribution report generated.

**Definition of done:** Thirty real, validated, testable research unlock mappings exist—or the repository truthfully reports the exact validated count and the specific gaps blocking 30.

---

## 141-P18 — Data Integrity and Cross-Catalog Validation

**Goal:** Make bad research mappings impossible to ship silently.

### Execution substeps

1. Extend `--data-integrity-selftest` with research-unlock validation rather than relying only on the dedicated selftest.
2. Validate unique unlock IDs and deterministic catalog order.
3. Validate every source node exists exactly once.
4. Validate target IDs through typed resolvers per domain.
5. Validate quantity constraints: physical item quantities positive; non-item unlocks must not carry meaningless quantities.
6. Validate description/localization keys if localization integrity infrastructure exists.
7. Validate retroactive mappings are safe for migration and flag mappings whose effects cannot be replayed idempotently.
8. Detect duplicate semantic mappings where two unlock IDs grant the same `(source,type,target)` unintentionally.
9. Detect impossible prerequisites/cycles if unlock definitions add multi-node conditions.
10. Detect mappings whose research node can never be completed due to broken prerequisites if reachability tooling exists.
11. Emit a concise machine-readable report for CI and a human-readable failure message with exact IDs and file locations.

**Integration note:** Validation should fail during development, not leave a runtime warning that players discover after spending research resources.

### Required tests

- Fixture for each validation failure class.
- Clean catalog exits 0.
- Bad target exits non-zero.
- Duplicate semantic grant exits non-zero.
- Invalid quantity exits non-zero.

**Definition of done:** Invalid research-unlock data cannot pass the standard integrity gate.

---

## 141-P19 — Dedicated `--research-unlocks-selftest`

**Goal:** Provide a fast, deterministic host-level proof that the entire bridge works without UI.

### Execution substeps

1. Add the CLI verb using the repository's existing host/selftest registration pattern.
2. Compose a minimal real runtime graph with research, bridge, representative sinks, and catalog loaders.
3. Select one validated mapping from each active domain.
4. Assert no unlocks before completion.
5. Complete each source node through the public research API.
6. Assert each target becomes available/effective through the downstream domain's public query.
7. Replay completion and assert no count/state changes.
8. Capture state, rebuild runtime, restore, reconcile, and assert identical downstream state.
9. Run an old-save migration fixture with completed research and missing bridge ledger.
10. Assert the migration applies once and a second reconciliation is a no-op.
11. Output deterministic summary counts: nodes completed, definitions resolved, applied, already-applied, pending, rejected, domains covered, digest.
12. Exit non-zero on any rejected mapping, unresolved domain, duplicate application, or digest mismatch.

**Integration note:** This verb is the canonical fast integration proof requested by the source plan.

### Required tests

- CLI registration test.
- Representative-domain end-to-end test.
- Replay test.
- Save/restore test.
- Old-save migration test.
- Digest stability test.

**Definition of done:** `godot --headless --path . -- --research-unlocks-selftest` exits 0 with deterministic output on a valid build.

---

## 141-P20 — Comprehensive Automated Test Matrix

**Goal:** Cover the bridge at unit, contract, integration, migration, property, and headless levels so cross-system regressions localize quickly.

### Execution substeps

1. Create unit tests for loader normalization, mapping resolution, transaction identity, grant ledger, pending logic, and capture/restore.
2. Create contract tests for every sink with a shared idempotency harness.
3. Create integration tests for each downstream domain using real catalog entries.
4. Create migration tests from pre-bridge save fixtures and partially materialized states.
5. Create property-style tests: any completion set applied in any event order converges to the same final unlock set when prerequisite semantics permit.
6. Create permutation tests for restore ordering/reconciliation where dependency lifecycle allows it.
7. Create negative tests for missing source, missing target, duplicate unlock ID, invalid type, invalid quantity, and malformed data.
8. Create replay stress: repeatedly publish identical completions and assert stable state/allocations.
9. Create all-research test: complete every valid node and compare resulting grant set with catalog-derived expectation.
10. Create no-research test: bridge remains empty and downstream research-gated capabilities remain locked.
11. Create UI projection tests that consume state but never mutate it.
12. Register the appropriate fast tests in CI and reserve heavier all-catalog/permutation checks for nightly if runtime cost warrants it.

**Integration note:** Test names should identify the boundary under test, e.g. `ResearchUnlockBridge_Replay_DoesNotDuplicatePhysicalPrototype` rather than generic `Works` tests.

### Required tests

- Unit suite green.
- Domain contract suite green.
- Migration suite green.
- All-research/no-research boundaries green.
- Headless selftest green.

**Definition of done:** A regression identifies whether the defect is data, bridge orchestration, sink contract, save migration, or presentation.

---

## 141-P21 — Observability, Audit Trail, and Debugging Surface

**Goal:** Make unlock behavior explainable during development without turning logs into another state authority.

### Execution substeps

1. Emit structured diagnostics containing research node ID, unlock ID, type, target ID, source, and result code.
2. Use debug/info level for normal applied/already-applied outcomes according to repository logging conventions; use errors for structurally invalid mappings that should never ship.
3. Expose a host/debug command to print current completed research and materialized unlock set if an existing diagnostics framework supports it.
4. Generate a deterministic unlock digest useful in bug reports and migration comparison.
5. Add counters for applied, skipped-already, pending, rejected, and migrated unlocks during selftests.
6. Do not log localized descriptions or mutable UI text as identifiers.
7. Add a one-shot migration summary rather than one log line per historical unlock when loading large old saves.
8. Ensure logs contain no personally identifying telemetry; this is local game state.
9. Document the diagnostic sequence for 'research completed but feature still locked'.

**Integration note:** Observability is essential because cross-system defects otherwise appear as unrelated UI/content bugs.

### Required tests

- Diagnostic IDs stable.
- Digest deterministic.
- Migration summary counts correct.
- No debug command mutates state.

**Definition of done:** A developer can determine within one trace whether mapping resolution, transaction application, or a downstream sink failed.

---

## 141-P22 — Performance and Allocation Budget

**Goal:** Keep the integration event-driven and effectively free during normal simulation frames.

### Execution substeps

1. No per-frame scan of all research nodes or unlock mappings.
2. Pre-index mappings by research node during catalog load.
3. Completion processing complexity should scale with unlocks attached to that node, not total catalog size.
4. Reconciliation may scan completed research on restore, but only once per restore/migration phase.
5. Avoid LINQ-heavy transient allocations in hot completion paths if repository performance conventions discourage them.
6. Cache typed sink lookup by enum/type.
7. Benchmark repeated idempotent replay to ensure it remains cheap and produces no inventory/UI churn.
8. Record baseline allocations/time for a representative completion and for all-research migration.
9. If 30 mappings are trivial, do not prematurely optimize; preserve clarity and determinism first.

**Integration note:** This is not a likely bottleneck, but event-driven architecture should make that provable.

### Required tests

- No Update/Tick polling introduced.
- Representative completion under agreed host budget.
- Replay causes no downstream reconstruction churn.
- All-research migration completes comfortably within load-time budget.

**Definition of done:** Research unlock integration has negligible steady-state cost and bounded restore-time cost.

---

## 141-P23 — Balance, Progression, and Anti-Exploit Review

**Goal:** Prevent the repaired research system from accidentally trivializing progression or creating duplication exploits.

### Execution substeps

1. For each mapping, classify power impact: convenience, resource efficiency, survivability, information, traversal, combat, medical, or economy.
2. Do not grant both a powerful final item and its recipe unless that prototype grant is an explicit progression choice.
3. Review early-game node costs against the value of their newly-real unlocks; formerly dead research may become dramatically stronger once connected.
4. Review shelter modifiers for multiplicative stacking and cap interactions.
5. Review expedition capabilities for loot/economy amplification loops.
6. Review medical unlocks for bypass of scarcity or risk.
7. Review combat unlocks for broad stat multipliers that affect every encounter.
8. Test save/reload, cancel/restart research, reverse-engineering, and traded-knowledge sequences for duplicate grants.
9. If research failure mechanics are enabled later, ensure failure cannot be save-scummed through nondeterministic seed reset unless that behavior is intentionally accepted.
10. Record tuning changes separately from integration code so causal regressions remain clear.

**Integration note:** Repair first, rebalance second. Integration correctness should not be obscured by simultaneous broad tuning.

### Required tests

- No duplicate-grant exploit.
- No bypass of normal downstream material/resource costs.
- Modifier caps respected.
- Progression review completed for high-impact mappings.

**Definition of done:** Research is valuable but does not collapse downstream resource and risk systems.

---

## 141-P24 — Rollout, Compatibility Gate, and Close-Out

**Goal:** Land the bridge in small reversible stages and leave a durable proof package for future plans.

### Execution substeps

1. Stage 1: contracts/catalog loader + validation with no gameplay effect.
2. Stage 2: completion publication + bridge ledger with a diagnostic/no-op sink.
3. Stage 3: one real crafting mapping end-to-end.
4. Stage 4: expedition and shelter adapters.
5. Stage 5: medical and combat adapters where repository-backed targets exist.
6. Stage 6: reverse-engineering parity and old-save migration.
7. Stage 7: UI/journal/tutorial projection.
8. Stage 8: expand toward 30 validated mappings.
9. Stage 9: enable integrity gate and dedicated selftest as required CI.
10. Stage 10: balance review and follow-on optional mechanics.
11. At every stage run build, unit tests, data-integrity selftest, bridge selftest, and research-unlocks selftest once introduced.
12. Write a close-out report listing implemented mappings, deferred mappings, dormant adapters, migration schema version, new gates, and any follow-on tasks.

**Integration note:** The rollout is intentionally monotonic: each stage adds proof without requiring a flag-day rewrite of all downstream systems.

### Required tests

- Every stage has a green revert point.
- CI gate enabled before declaring complete.
- Close-out report matches actual mapping catalog.
- No dormant placeholder IDs remain.

**Definition of done:** The research subsystem is no longer terminal, and future research content has a documented, validated path into gameplay.

---

## 5. Initial Mapping Matrix and the 30-Mapping Rule

The six source-plan relationships below are the initial integration corpus. Exact target IDs must still be verified against the repository before implementation. Where the source plan names a recipe or upgrade, it is an intended mapping candidate, not proof that the target exists.

| # | Research source | Legacy breakthrough | Intended downstream effect | Required verification |
|---:|---|---|---|---|
| 1 | `research_water_purification_advanced` | `item_water_filter_advanced` | Unlock advanced water-filter recipe; optional prototype only if explicitly intended | research node exists; item exists; exact recipe exists; recipe was not globally available |
| 2 | `research_radiation_shielding` | `item_radiation_shielding_panel` | Prototype/component semantics + shelter shielding upgrade availability | item exists; shelter target exists; mitigation path identified |
| 3 | `research_gas_mask_improvement` | `item_gas_mask_improved` | Unlock improved gas-mask recipe/capability | exact recipe/equipment target exists |
| 4 | `research_solar_power` | `item_solar_inverter` | Prototype/component semantics + shelter solar/power upgrade availability | item exists; power-grid target exists; installation remains separately costed |
| 5 | `research_crypto_analysis` | `item_radio_cipher_rotor` | Cipher capability + validated expedition/radio option | item exists; capability consumer exists; destination/encounter target verified |
| 6 | `research_air_filtration` | `item_air_filter_hepa` | Unlock HEPA recipe and/or shelter filtration upgrade availability | recipe/upgrade target exists; contamination/air-quality consumer verified |

### 5.1 Slots 7–30

Slots 7–30 are **authoring slots, not pre-approved fake IDs**. Populate them from the repository audit using the following distribution target:

| Domain | Suggested additional validated mappings | Intent |
|---|---:|---|
| Crafting | 4–6 | advanced tools, filters, components, protective equipment |
| Expedition | 4–5 | information/capability gates, specialized route/encounter actions |
| Shelter | 4–5 | upgrade availability, efficiency/capability modifiers |
| Combat | 3–4 | only repository-backed doctrines/tactics/equipment capabilities |
| Medical | 3–4 | procedures, treatment capabilities, institutional medical knowledge |
| Cross-domain synergies | 1–3 | multi-node capabilities only after prerequisite semantics are tested |

The numeric target is subordinate to repository truth. A final count of 26 validated mappings plus four explicit content-gap tickets is superior to 30 mappings containing four invented target IDs.

### 5.2 Mapping review checklist

Every authored mapping must answer:

1. Does the source research node exist?
2. Is the node currently completable?
3. Does the target exist?
4. Which domain owns the target?
5. What exactly changes when granted?
6. Is the effect knowledge, availability, a physical prototype, a modifier, or a one-time state change?
7. Is the effect safe to apply retroactively?
8. Is it idempotent?
9. Does the player still need downstream resources/skills/conditions?
10. Does the UI have a truthful way to explain it?
11. Is there at least one end-to-end test?
12. Is there a plausible gameplay state in which the unlocked target is usable?
13. Can the effect be removed/renamed in future versions without corrupting saves?
14. Does it overlap another mapping semantically?
15. Is the balance impact acceptable?

---

## 6. Exact Unlock Semantics by Type

### 6.1 `InventoryItem`

Use only for an intentionally granted physical prototype/component. `Quantity > 0` is mandatory. The bridge must use the canonical inventory/storage API and handle capacity. Replay must not duplicate the item.

### 6.2 `Recipe`

Makes a validated recipe known/available. It does not grant ingredients, waive costs, or craft the output. Recipe requirements remain authoritative.

### 6.3 `ExpeditionCapability`

Adds a named capability that expedition content may require. It should normally unlock an action, interpretation, interaction, or encounter eligibility rather than teleport the party or auto-resolve risk.

### 6.4 `ExpeditionDestination`

Use only where research truly reveals a location. Keep "destination revealed" separate from "party may travel there now"; route, weather, hazard, equipment, and survivor requirements still apply.

### 6.5 `ShelterUpgrade`

Makes an upgrade buildable/available. It does not install the upgrade unless the design explicitly models a research-completion auto-install.

### 6.6 `ShelterCapability`

Adds a keyed institutional capability or modifier consumed by shelter simulation. All modifiers must be keyed so replay cannot stack them.

### 6.7 `CombatDoctrine`

Unlocks a real doctrine/tactic definition. It must not invent a combat framework merely to satisfy a mapping.

### 6.8 `CombatEquipmentCapability`

Allows use/modification/production of equipment where such a permission model exists. It does not grant the equipment.

### 6.9 `MedicalProcedure`

Makes a procedure available to the medical pipeline. Diagnosis, skill, items, contraindications, and patient state continue to govern execution.

### 6.10 `MedicalCapability`

Adds a keyed institutional treatment/diagnostic modifier where a discrete procedure is not the right abstraction.

### 6.11 `WorldCapability`

Fallback only for genuinely cross-domain institutional knowledge. Do not use this as a dumping ground to avoid defining a proper sink.

---

## 7. Completion and Reconciliation State Machine

```text
                 ┌────────────────────┐
                 │ Research incomplete │
                 └─────────┬──────────┘
                           │ successful canonical completion
                           ▼
                 ┌────────────────────┐
                 │ Completed in       │
                 │ ResearchSystem     │
                 └─────────┬──────────┘
                           │ publish ResearchCompleted
                           ▼
                 ┌────────────────────┐
                 │ Resolve mapping(s) │
                 └─────────┬──────────┘
                           │
          ┌────────────────┼────────────────┐
          ▼                ▼                ▼
     already granted    valid new       invalid data
          │                │                │
          ▼                ▼                ▼
       no-op           apply sink       reject/fail gate
                           │
                      ┌────┴────┐
                      ▼         ▼
                   success    dependency unavailable
                      │         │
                      ▼         ▼
                 ledger commit pending ledger
                      │         │
                      └────┬────┘
                           ▼
                     UI/journal refresh
```

### Restore/migration path

```text
load save
  → restore ResearchSystem completed-node set
  → restore downstream systems
  → restore bridge grant/pending ledger if present
  → determine whether bridge schema exists
      → old save: derive expected retroactive unlocks from completed nodes
      → new save: compare expected unlocks with grant/downstream state
  → apply only missing valid effects
  → mark converged effects
  → retry pending effects
  → emit one reconciliation summary
  → refresh presentation
```

Reconciliation is not a hidden cheat. It is the mechanism that makes capability state consistent with already-earned research after schema upgrades, interrupted restore order, or historical dead-end behavior.

---

## 8. Suggested Save Contract

Use repository-native DTO naming and serialization conventions. The logical content should resemble:

```json
{
  "schemaVersion": 1,
  "grantedUnlockIds": [
    "unlock_research_water_advanced_recipe",
    "unlock_research_crypto_cipher_capability"
  ],
  "pendingUnlockIds": []
}
```

Do not serialize localized descriptions, target display names, enum display strings, or whole research definitions. Save stable IDs only.

### 8.1 Invariants

- Every `grantedUnlockId` resolves to a known definition or a migration alias.
- An ID cannot be both granted and pending.
- Arrays are deterministically ordered for stable diffs.
- Unknown historical IDs are preserved/reported according to repository compatibility policy rather than silently discarded.
- Bridge schema version changes have explicit migration tests.
- Completion state remains in `ResearchSystem`; the bridge save cannot resurrect an incomplete node.

---


## 9. Detailed Test Inventory

Use parameterized fixtures where practical; coverage matters more than method count. The minimum matrix is:

| Area | Required proofs |
|---|---|
| Catalog | duplicate unlock IDs rejected; unknown research source rejected; unknown typed target rejected; invalid quantity/type rejected; deterministic lookup order |
| Completion | first completion publishes once; duplicate completion publishes none; restore publishes no live completion; reverse engineering converges on canonical event |
| Bridge | apply once; replay no-op; multi-unlock deterministic order; pending retry; failed apply not marked granted; capture/restore exact |
| Physical items | explicit quantity grant; no duplicate prototype; full-storage policy; capability-only mapping creates no junk inventory |
| Crafting | recipe locked before research; available after; ingredients still required; survives save/load; unrelated research has no effect |
| Expedition | capability option absent before; present after; route/weather/hazard gates remain; headless eligibility matches UI |
| Shelter | upgrade unavailable before; available after; not auto-installed by default; materials/power/maintenance still apply; modifiers non-stacking |
| Combat | doctrine/capability gating works; equipment still required; modifiers non-stacking; player research does not leak to enemy state |
| Medical | procedure unavailable before; available after; diagnosis/material requirements still apply; efficacy modifiers non-stacking |
| Migration | pre-bridge save repaired; all-research save repaired; second reconciliation no-op; partial ledger converges; renamed/removed mapping policy tested |
| Determinism | completion-order permutations converge; save/load insertion does not change final set; all-research equals catalog expectation; no-research stays empty |
| UI | effects displayed once; no duplicate reload toast; migration summary policy respected; localized requirement text; keyboard/text-scale coverage |
| Headless | dedicated selftest exits 0; bad target exits non-zero; digest stable; every active sink represented |

Mandatory named regression cases should include the six existing breakthrough strings and the historically dead `WorkshopReverseEngineeringSystem` path.

A useful shared sink-contract harness should assert for every sink: `Apply(new)` changes the domain exactly once, `Apply(existing)` is a no-op/converged result, invalid target fails validation, capture/restore preserves domain state, and the bridge never needs UI presence.

Property tests should prove that independent node completions commute: for any permutation of a completed-node set, the final granted-unlock set is identical. Where prerequisite/synergy semantics make order meaningful, the final state must still converge after reconciliation.

---

## 10. Failure Modes and Required Handling

| Failure mode | Correct behavior | Incorrect behavior to prohibit |
|---|---|---|
| Research completes but mapping missing | Completion remains valid; diagnostic/design gap reported | fabricate target or crash player save |
| Mapping target missing | fail data integrity before release | log warning and silently lose reward |
| Duplicate completion event | no-op through grant ledger | duplicate item/modifier/notification |
| Save has completed research but no bridge state | retroactive reconciliation | leave old player permanently disadvantaged |
| Storage full for prototype | canonical overflow/pending handling | silently delete earned item |
| Downstream sink unavailable during restore | pending then post-restore retry | mark granted before effect exists |
| Recipe already unlocked historically | converge ledger without duplicate | add duplicate recipe entry |
| Shelter modifier already present | recognize keyed effect | multiply modifier again |
| UI scene absent | gameplay still unlocks headlessly | postpone grant until panel opens |
| Mapping renamed | explicit migration/alias | treat as brand-new reward and duplicate |
| Research data order changes | same semantic result | identity changes because array index changed |
| Reverse engineering repeats same knowledge | no duplicate effects | farm prototypes/capabilities |
| Optional research failure occurs | no completion event | grant reward despite failed research |
| Faction trade repeats completed node | no duplicate effects | repurchase for infinite item reward |
| One mapping in multi-unlock node invalid | caught in validation; release blocked | partial silent ship with misleading UI |

---

## 11. Cross-System Contract Matrix

| System | Reads research directly? | Receives from bridge | Owns persistent domain state | Must remain authoritative for |
|---|---|---|---|---|
| Research | n/a | none | yes | progress, prerequisites, completion |
| Crafting | no | recipe ID/capability | yes/according to current design | ingredient/cost/crafting result |
| Expedition | no | capability/destination ID | yes where discovery state exists | route, party, hazard, encounter rules |
| Shelter | no | upgrade/capability ID | yes | build/install/operation/resources |
| Combat | no | doctrine/capability ID | yes | action legality, equipment, combat resolution |
| Medical | no | procedure/capability ID | yes | patient eligibility, diagnosis, treatment costs |
| Inventory | no | explicit item grant | yes | capacity, item ownership, stack semantics |
| Journal/UI | query only | presentation event/query | journal owns journal entries | display, localization, navigation |
| Save coordinator | no gameplay decisions | bridge state | yes for serialization order | versioning, restore lifecycle |

The strongest integration outcome is that downstream domains never need to know which research node caused a capability. They only know that a capability or target has become available through a domain-specific interface.

---

## 12. Verification Commands and Gate Order

Use the exact repository command names discovered during implementation. The source plan requires at minimum:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --research-unlocks-selftest
```

Recommended gate sequence:

```text
1. compile core/tests
2. unit + contract tests
3. compile host/game
4. data-integrity selftest
5. bridge/composition selftest if repository has one
6. research-unlocks selftest
7. save-migration corpus tests
8. representative headless campaign/slice smoke if research is reachable there
9. fast CI aggregate
10. nightly all-research/property/permutation checks
```

The dedicated research unlock selftest must not replace normal data integrity. The two gates answer different questions:

- data integrity: "are mappings structurally valid?"
- research unlock selftest: "does a real completion produce real downstream capability safely?"

---

## 13. Implementation File Plan

Exact names should follow repository conventions after P0, but the expected change surface is:

### New or likely-new files

```text
Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs
Assets/Ashfall.Core/Research/ResearchUnlockDefinition.cs
Assets/Ashfall.Core/Research/ResearchUnlockState.cs
Assets/Ashfall.Core/Research/IResearchUnlockSink.cs
Assets/Ashfall.Core/Research/ResearchUnlockCatalogLoader.cs
Assets/StreamingAssets/Data/research_unlocks.json
Ashfall.Core.Tests/Research/ResearchUnlockBridgeTests.cs
Ashfall.Core.Tests/Research/ResearchUnlockCatalogTests.cs
Ashfall.Core.Tests/Research/ResearchUnlockMigrationTests.cs
Ashfall.Core.Tests/Research/ResearchUnlockSinkContractTests.cs
docs/research/RESEARCH_UNLOCK_MAPPING.md
docs/research/RESEARCH_UNLOCK_MIGRATION.md
```

### Existing files/systems to modify only through narrow seams

```text
Assets/Ashfall.Core/Research/ResearchSystem.cs
Assets/Ashfall.Core/.../WorkshopReverseEngineeringSystem.cs
Assets/Ashfall.Core/Crafting/CraftingSystem.cs
Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs
Assets/Ashfall.Core/Shelter/...
Assets/Ashfall.Core/Combat/...
Assets/Ashfall.Core/Medical/...
Assets/StreamingAssets/Data/research_tree.json
src/Host/HostCli.cs
composition/bootstrap files discovered in P0
save coordinator/state DTO files discovered in P0
research/crafting/etc. UI projection files discovered in P0
data-integrity validator registration
CI gate manifest/workflow if repository uses explicit gate registration
```

### Files that must **not** become authorities

- UI panel scripts.
- journal presentation code.
- tutorial/onboarding code.
- ad-hoc `Main` partials with hard-coded node→feature switches.
- individual expedition encounter scripts checking raw research IDs.
- recipe definitions duplicating research completion state.
- item definitions whose presence is used as a proxy for institutional knowledge unless that is an explicit design contract.

---

## 14. Execution Dependency Graph

```text
P0 forensic audit
  │
  ├──► P1 data authority
  │       │
  │       └──► P18 integrity validation
  │
  ├──► P2 completion publication
  │       │
  │       └──► P3 idempotent transaction
  │               │
  │               ├──► P4 crafting
  │               ├──► P5 expedition
  │               ├──► P6 shelter
  │               ├──► P7 combat
  │               ├──► P8 medical
  │               ├──► P9 breakthrough semantics
  │               └──► P10 reverse engineering parity
  │
  ├──► P14 save/migration ◄──────── all active sinks
  │
  ├──► P15 composition/lifecycle
  │
  ├──► P16 UI/journal after gameplay truth
  │
  ├──► P17 mapping campaign after validators + sinks
  │
  ├──► P19 selftest
  ├──► P20 full tests
  ├──► P21 diagnostics
  ├──► P22 performance
  ├──► P23 balance
  └──► P24 rollout/close-out

Optional after core:
P11 tiers/synergies
P12 failure mechanics
P13 trading/faction acquisition
```

### Critical path

`P0 → P1 → P2 → P3 → P4 → P14 → P15 → P18 → P19`

That critical path is sufficient to prove the dead end is fixed with one real downstream domain before expanding breadth.

---


## 15. Incremental Commit Strategy

Land the work in reversible slices:

1. audit + mapping authority/validator;
2. canonical completion event;
3. idempotent bridge/ledger;
4. first real crafting mapping;
5. breakthrough-item classification;
6. expedition + shelter adapters;
7. medical/combat adapters where repository-backed targets exist;
8. reverse-engineering parity;
9. save schema + retroactive migration;
10. composition/post-restore reconciliation;
11. UI/journal projection;
12. mapping expansion toward 30;
13. integrity gate + headless selftest;
14. balance review + close-out.

Every slice must compile, keep supported saves loadable, and add its focused tests. Avoid one giant cross-domain commit because failures then become difficult to localize.

---


## 16. Agent Handoff Checklist

Start each session by recording repository revision, baseline test result, current package, and any file-level drift since the prior session. Work on one domain at a time; do not invent unresolved IDs; add tests with behavior; run data integrity after mapping edits and the research-unlocks selftest after bridge/sink edits.

End each session with:

```text
implemented packages:
files changed:
mappings changed:
tests added:
focused/full test results:
data-integrity result:
research-unlocks-selftest result:
migration fixtures touched:
known gaps:
next package:
```

Optional features must not enter the critical path until deterministic completion→unlock delivery, migration, and CI are green.

---

## 17. Flagship Definition of Done

The integration is not done because `ResearchUnlockBridge.cs` exists. It is done when repository behavior proves the entire chain.

### Runtime

- [ ] A canonical research completion event exists.
- [ ] `ResearchUnlockBridge` is constructed in the canonical composition root.
- [ ] Unlock definitions come from validated data authority.
- [ ] Bridge transaction is idempotent.
- [ ] Physical item grants are explicit and one-time.
- [ ] Crafting receives real recipe unlocks.
- [ ] Expedition receives real capability/destination unlocks where validated.
- [ ] Shelter receives real upgrade/capability unlocks where validated.
- [ ] Combat receives real doctrine/capability unlocks where validated.
- [ ] Medical receives real procedure/capability unlocks where validated.
- [ ] Reverse engineering converges on the same path.
- [ ] UI/journal project state and never grant it.

### Persistence

- [ ] Bridge state has schema version.
- [ ] Save/load round trip preserves grant state.
- [ ] Pre-bridge old saves are retroactively reconciled.
- [ ] Migration is idempotent.
- [ ] Renamed/removed mappings have explicit compatibility policy.
- [ ] Restore ordering is tested.

### Data

- [ ] Six known breakthrough strings have explicit semantics.
- [ ] Every source node resolves.
- [ ] Every target resolves.
- [ ] No duplicate semantic mappings.
- [ ] Thirty mappings are validated, or exact repository-backed shortfall is documented with content-gap tasks.
- [ ] Mapping report is generated or easily auditable.

### Testing/CI

- [ ] Core test build passes.
- [ ] Full core test suite passes.
- [ ] Game/host build passes.
- [ ] `--data-integrity-selftest` passes.
- [ ] `--research-unlocks-selftest` passes.
- [ ] Replay/idempotency tests pass.
- [ ] Save migration fixtures pass.
- [ ] All-research/no-research boundaries pass.
- [ ] Headless parity passes.
- [ ] CI includes required gate registration.

### Player experience

- [ ] Research completion visibly explains real consequences.
- [ ] Newly unlocked recipes/options/upgrades/procedures appear through normal UIs.
- [ ] No duplicate breakthrough spam on reload.
- [ ] Old-save migration does not misleadingly replay dozens of first-time popups.
- [ ] Unlock descriptions are localizable.
- [ ] Research is now a meaningful progression choice rather than a terminal ledger.

---


## 18. Follow-On Task Backlog

Keep these outside the core dead-end repair and schedule them only after the bridge, migration, and CI gates are stable.

1. **141-F1 Research Specialization** — survivor/team specialization affecting speed, cost, or branch eligibility; no change to unlock materialization semantics.
2. **141-F2 Research Collaboration** — multiple survivors contribute through one canonical completion event.
3. **141-F3 Faction Research Exchange** — diplomacy-backed full/partial knowledge transfer with provenance and duplicate-purchase protection.
4. **141-F4 Research Espionage/Theft** — stolen partial data, intel, counterintelligence, and reverse-engineering integration.
5. **141-F5 Research Sabotage** — damage facilities/progress without casually revoking already-learned institutional knowledge.
6. **141-F6 Research Failure/Hazards** — seeded failure before completion; hazards routed through existing hazard/medical systems.
7. **141-F7 Branch Exclusivity** — irreversible specialization rules owned by research eligibility, not by downstream sinks.
8. **141-F8 Synergy Catalog** — multi-node prerequisite unlocks with stable IDs and reconciliation.
9. **141-F9 Faction Research Competition** — independent NPC-faction knowledge progression and world consequences.
10. **141-F10 Research Legacy/New Game+** — explicit carry-over policy instead of blindly copying grant ledgers.
11. **141-F11 Balance Sweep** — compare no-research, generalist, and specialized policies across survival, resource, expedition, shelter, combat, and medical outcomes.
12. **141-F12 Research UX Visualization** — causal tree showing prerequisites and concrete downstream effects as a projection of validated runtime data.

---

## 19. Final Verification Runbook

Before marking Plan 141 closed:

```bash
# 1. compile test project
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj

# 2. full core tests
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj

# 3. compile game/host
dotnet build Ashfall.csproj

# 4. all catalog integrity
godot --headless --path . -- --data-integrity-selftest

# 5. research downstream bridge proof
godot --headless --path . -- --research-unlocks-selftest
```

Then execute the repository's current bridge/composition/save/release gates if they exist. Record:

```text
build SHA:
research unlock schema version:
validated mapping count:
physical breakthrough classifications:
domains active:
domains intentionally dormant:
old-save fixtures:
applied unlocks in selftest:
already-applied replay count:
pending count:
rejected count:
unlock digest:
test count:
all tests pass:
data integrity pass:
research unlock selftest pass:
```

Any non-zero `rejected` count blocks close-out. Any unexpected `pending` count in a fully composed runtime blocks close-out. A mapping count below 30 does not block architectural close-out if the shortfall consists only of explicitly documented absent content, but it does block claiming the source plan's 30-mapping content milestone as finished.

---

## 20. Closing Integration Principle

Research must become a causal gameplay system:

```text
knowledge earned
    → capability becomes available
        → player can make a new decision
            → downstream systems still enforce their own costs and risks
```

The bridge succeeds when research is neither a cosmetic ledger nor a god-mode bypass. It should unlock **possibility**, while crafting still requires materials, expeditions still require preparation, shelter upgrades still require construction and power, combat still requires equipment and tactical conditions, and medicine still requires diagnosis, skill, and supplies.

That separation is the long-term architecture: research decides what the shelter knows; each downstream domain decides what the shelter can actually do with that knowledge.

**End state:** no breakthrough ends at a log line, no unlock is granted twice, no old save loses earned progress, no downstream system needs to understand research internals, and every mapping can be proved by data integrity plus an end-to-end selftest.
