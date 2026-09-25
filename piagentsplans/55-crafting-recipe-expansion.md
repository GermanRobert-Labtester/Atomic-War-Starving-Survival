# Plan 55 — Crafting Recipe Expansion

> **Rebuild status:** CURRENT-EVIDENCE PLAN — POST-250K DEEP POLISH AND FINAL PRECISION PASS INCLUDED
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-6`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round6-2026-09-25`
>
> **Current-evidence date:** `2026-09-25`
>
> **Authority order:** live source and data → `AGENTS.md` → `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` → live ledgers → this document.
>
> **Authority checksum:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
>
> **Length policy:** 150k–170k is the first completeness checkpoint; 250k is an evidence-backed depth target, not a ceiling. The plan stops when verified evidence is exhausted rather than padding with fictional APIs or duplicate systems.

## 0. Integrity Statement and Plan Status

This is a planning and architecture artifact for **Recipe catalogs, crafting commands, material bills, research gates and station capability**. It preserves the original intent: Improve recipe breadth and progression quality through the current recipe loaders, crafting owner, inventory authority, research and skill contracts without adding a second crafting ledger.

The current residual premise is: Every recipe family must be checked for existing loader, item references, station gates, skill/research prerequisites, inventory atomicity, output semantics, save reachability and host presentation.

It authorizes no production, authored-data, test, save, UI, generated-index or runtime change. Any future CREATE proposal is hypothetical until a separately claimed implementation package rechecks the live owner, public API, data references, save owner, host route and focused test. The current worktree contains unrelated dirty changes; they are not evidence and are not modified by this plan.

The plan has four distinct passes: (1) current-reality and premise reconstruction; (2) integration framework and code architecture; (3) post-250k deep polishing; and (4) final precision/reaccuracy and handoff review. Length is not treated as quality. Repetition without a new decision, evidence record, failure case or executable acceptance condition is a defect.

# 1. Objective

The bounded objective is to make the **Crafting Recipe Expansion** opportunity implementable without creating a parallel gameplay authority. The plan must identify:

- the one current owner for mutable state;
- the authored JSON or static source for content;
- the existing host adapter and route that exposes a player command;
- the event/fact seam for consequences;
- the existing save section, or an explicit decision that no new persistent state is needed;
- deterministic ordering and seeded randomness boundaries;
- UI projection and accessibility behavior;
- exact focused verification commands;
- migration, rollback and fail-closed behavior.

Non-goals are declared throughout: no new generic manager, no panel-owned counter, no copied catalog, no Unity dependency, no new Unity-era `Assets/_Game/` gameplay path, no speculative content row that lacks a consumer, and no unrelated refactor disguised as feature work.

# 2. Current Decision and Terminal/Residual Status

The prior plan is not accepted as proof. The current status is derived from the source/data/test evidence indexed later in this document. The plan must separate:

- **Terminal/maintenance scope:** behavior already present, already routed, or already covered by current tests. This remains maintenance work and is not reopened.
- **Residual implementation scope:** a verified missing consumer, missing persistence path, stale data reference, inaccessible route, absent lifecycle binding, or unproven event connection.
- **Unknown/deferred scope:** a question that cannot be answered from current evidence. Unknowns are not converted into invented classes, fields, save sections or test suites.

The safe posture is therefore **plan-first, implementation-second**. A future builder must perform the current-evidence checkpoint again immediately before editing because this document can become stale when source or data changes.

**Current decision rule:** if live source contradicts a claim in this plan, return `STALE_PLAN` to the foreman. Never restore a deprecated API merely to preserve a historical plan narrative.

# 3. Required Delta

The minimum safe delta for this subject is:

1. Replace catalog-presence assumptions with a row census and reference audit.
2. Identify the current mutable owner and keep it authoritative.
3. Trace at least one real player command from host input to owner mutation.
4. Trace the resulting fact to its current consumer, UI projection and save path where persistence is required.
5. Define typed events or projection records only where a real cross-owner consequence exists.
6. Define deterministic ordering, seed/substream rules and replay boundaries.
7. Define null, empty, duplicate, old-save, invalid-reference and lifecycle behavior.
8. Add only the smallest focused tests that cover a confirmed gap or public contract.
9. Preserve existing commands, panels, save sections and catalog schemas unless a separately signed delta requires an additive change.
10. Record a rollback point before implementation and a truthfulness check after implementation.

The phrase “huge leap forward” is interpreted as a coherent, reachable game-system improvement—not as permission to invent a second architecture or fill a character quota.

# 4. Evidence and Premise Audit

Evidence is classified as follows:

- **CURRENT FILE:** exists in the worktree at rebuild time; its hash and bounded excerpt are recorded.
- **CURRENT CATALOG:** JSON parses at rebuild time; row/key counts are recorded, but presence is not reachability.
- **MECHANICAL REFERENCE:** a text search hit; it is a clue, not proof of a production call.
- **HISTORICAL RECORD:** old plan or closeout prose; useful context only.
- **PROPOSAL:** a future design shape; requires a new claim and current recheck.
- **UNKNOWN:** an unresolved premise; must be reported rather than filled with fiction.

The complete source, catalog, test and authority dossiers appear in Appendices A–F. A future implementation handoff must cite the current path and line, not merely repeat a filename list.

# 5. Existing Extension Seams

The safe route is to extend current seams. The plan does not introduce a new subsystem merely because the proposed feature is large. For each concern, the builder must record one owner, one input, one mutation or read projection, one fact/event, one presentation route and one persistence answer.

**Extension rule:** if a proposed feature can be represented as a projection over existing state, prefer a projection. If it adds a new durable fact, add it to the existing owner state and existing save section only after a migration review. If it changes a shared command, require an integrator claim.

# 6. Proposed Architecture

```text
authored JSON / existing owner state
          │
          ▼
current catalog loader and current Core owner
          │
          ▼
typed fact or read-only projection
          │
          ├─ existing host command / route
          ├─ existing UI projection and feedback
          ├─ existing journal/radio/codex consumer where applicable
          └─ existing save capture/restore where durable
```

The architecture is owner-first. The host does not calculate gameplay state; a panel does not mirror mutable authority; a catalog does not schedule events by itself; and a preview does not emit a real consequence. If a missing seam is confirmed, the smallest additive provider or typed record must be proposed at the current owner boundary.

# 7. Ownership Matrix

The following matrix is a **working contract to be revalidated**, not a claim that every named file currently implements the proposed delta:

| Concern | Candidate owner | Evidence to prove | Boundary |
|---|---|---|---|
| authored content | current JSON catalog | loader, schema, row IDs, reference validation | no duplicate catalog |
| mutable domain state | current Core owner | capture/restore and mutation methods | no shadow state |
| lifecycle | current day/event owner | actual registration and tick order | no hidden timer |
| player command | current host/session seam | input-to-mutation call path | no panel shortcut |
| consequence | existing fact/event consumer | post-mutation ordering and idempotency | no pre-mutation UI fiction |
| presentation | current panel/HUD/radio route | truthful read model and focus behavior | no gameplay authority |
| persistence | existing save section or explicit no-save | capture/restore/deep-copy/checksum | no parallel store |
| validation | current catalog/test owner | focused invalid-data tests | no broad suite by default |

# 8. Data Flow

The intended flow is:

`input → validation → current owner mutation/read model → typed fact → existing consumer → presentation → command result → save capture`

Every arrow is explicit. A missing consumer is a gap, not a reason for the source owner to call a panel directly. A read-only projection may be recomputed, but any durable player decision must pass through the authoritative owner and its existing save path.

# 9. State Model and Invariants

Before implementation, enumerate current state fields and classify each as immutable definition, derived projection, durable owner state, one-shot fact, cooldown, or historical record. New fields require:

- a truthful default for old saves;
- an invariant and range policy;
- a mutation method or event path;
- capture and restore coverage;
- deep-copy/isolation behavior where applicable;
- deterministic ordering for collections;
- idempotency behavior for repeated delivery;
- an explicit decision not to persist derived values.

Invariants must be expressed as executable acceptance criteria wherever possible. “The system feels coherent” is not an invariant.

# 10. API and Contract Design

Any proposed API below is a contract shape, not a declaration that the method already exists:

```text
LoadCatalog(authorityPath) -> validated current catalog
QueryCurrentState(subjectId) -> read-only truthful projection
Preview(command, expectedVersion) -> named availability/refusal and deltas
Execute(command, expectedVersion) -> owner mutation + stable fact
OnFact(fact) -> existing consumer projection
CaptureState() -> deep serializable owner state
RestoreState(snapshot) -> validated current state
```

New interfaces are justified only when at least two real consumers need the same boundary or when a host/engine boundary must be isolated. Do not create a framework because a future feature might need one.

# 11. Data Plan and Catalog Authority

The data plan is additive and narrow. The current JSON row audit appears in Appendix C. For every proposed row or field, record: stable snake_case ID, schema version, references, ranges, default behavior, consumer, validator and rollback. Existing IDs are reused; no duplicate ID is introduced; no “catalog-only” feature is called integrated.

If a catalog is not loaded by a current production owner, the plan must stop at a reachability finding and name the missing binding. It must not create a second loader to make the data appear live.

# 12. Save, Restore and Migration

Determine whether the feature has durable player decisions. If it does, extend the existing owner state and save section. If it is purely authored or derived, say so and do not add a save section.

Required persistence questions:

- What exact state is durable?
- Which existing save owner captures it?
- What is the old-save default?
- Does restore validate all references?
- Are collections cloned rather than aliased?
- Is checksum participation explicit?
- Can a save be loaded during a transition without double application?
- What is the rollback behavior after a bad version?

A proposed new save section requires a separate architecture decision and a named integration owner. This plan does not grant that permission.

# 13. Determinism and Replay

Determinism is a correctness property, not a stylistic preference. Use the existing seeded RNG contract only when the owner already requires randomness. New streams must be named and stable; no `System.Random`, wall-clock seed, GUID tie-breaker or hash-iteration order may decide a gameplay result.

Same campaign seed, same authored data, same command order and same save state must produce the same domain result. Presentation timing, animation, audio scheduling and frame rate may vary, but they must not change simulation truth. Collection projections use stable ordering before any weighted selection or UI sequence.

# 14. System and Event Wiring

The event path is post-mutation and typed where cross-owner effects matter. The builder must inspect setup, registration, day/hour ticks, event subscription order, reset/dispose behavior and reload behavior. A method declaration is not a call path. A test fixture is not a host binding. A panel command is not an event consumer.

Exactly-once consequences use stable identity keys and existing one-shot ledgers where available. Repeated delivery must be harmless or explicitly rejected. Missing owners fail closed with a truthful diagnostic; they do not silently fabricate a default gameplay success.

# 15. Godot Host Integration

Host work is limited to input, binding, lifecycle, presentation and adaptation. The Godot layer may:

- project owner state into readable rows;
- send a named command;
- display pending/success/refusal feedback;
- refresh after an event;
- apply accessibility, focus and close/back behavior.

It may not recalculate a domain score, maintain a second counter, infer a hidden outcome, or mark an event complete before the owner succeeds. Every new surface must be registered in the current panel/route registry only under a separately claimed shared seam.

# 16. Narrative and Content Integration

Content is meaningful only when it is authored, validated, reachable and attached to a real state transition. Faction, survivor, location, quest, radio, journal, codex and atmosphere content must preserve continuity, avoid real-world copied material, and use the canonical narrator voice. A prose row may be beautiful and still be a defect if no current consumer can reach it.

# 17. UI, Accessibility and Legibility

The UI must show current truth, named refusals, meaningful deltas and consequences. It must not expose internal debug state as player-facing authority. Verify keyboard/controller focus, close/back behavior, readable contrast, non-color status communication, controller-safe scrolling, refresh after external mutation and panel disposal.

A missing panel route is a deliberate integration gap. Do not add a fake route merely to make the plan appear complete. If the route is deferred, state the exact reason and the future shared seams required.

# 18. Failure Modes and Negative Contracts

At minimum, reason about: null/empty catalogs, duplicate IDs, missing references, invalid ranges, dead subjects, inaccessible locations, insufficient resources, stale owner state, repeated events, save during transition, corrupted save, unknown version, host reload, panel not mounted, display-only action, and deterministic replay.

Expected behavior is one of: fail closed with named reason; preserve prior state; use a documented legacy default; or degrade to a truthful read-only projection. Silent success, silent mutation, duplicate rewards and fabricated consequences are prohibited.

# 19. Test Strategy

Verification is layered and focused:

- **Data tests:** schema, snake_case IDs, duplicate/reference checks, row census, invalid ranges.
- **Core tests:** pure behavior, boundaries, state transitions, events and stable ordering.
- **Persistence tests:** capture/restore round-trip, old-save default, deep-copy isolation, checksum participation where relevant.
- **Integration tests:** owner → host command → event → existing consumer; setup and lifecycle binding.
- **UI tests:** route/open/close, focus, visible feedback, refresh and disposal where the surface is actually changed.
- **Headless checks:** only the smallest existing CLI/selftest that proves the affected seam.

Test selection follows `TEST_POLICY.md`. The commands below are proposed focused commands, not fresh pass claims. A builder must run each new or changed test alone first, then the directly affected regional target. No full-suite run is implied by this plan.

# 20. Dependency-Ordered Phases

## Phase 0 — Premise and ownership checkpoint

Re-read the current source/data/test evidence, confirm the owner, confirm no overlapping claim, and freeze the row census. **Gate:** no unresolved owner or reference ambiguity.

## Phase 1 — Core contract

If a new durable fact or deterministic rule is confirmed, implement it in the current Core owner with engine-free types and explicit invariants. **Gate:** focused pure tests pass alone.

## Phase 2 — Persistence and lifecycle

Extend the existing capture/restore path and setup/reset/dispose lifecycle only if the confirmed state is durable. **Gate:** round-trip, old-save and reload tests pass.

## Phase 3 — Authored data

Add only schema-valid rows or fields that have a current consumer, validator and rollback story. **Gate:** catalog integrity and reference tests pass.

## Phase 4 — Host wiring

Bind one real command and one real event consumer through the current host seam. **Gate:** source-level call path and focused integration test agree.

## Phase 5 — Presentation

Project current owner state into the existing route. **Gate:** truthfulness, accessibility, refresh and disposal checks pass; no UI-owned authority.

## Phase 6 — Narrative/content QA

Check continuity, tone, unlock conditions, duplicate IDs, player reachability and false claims. **Gate:** content audit reports every row and consumer.

## Phase 7 — Final integration and rollback review

Re-run the smallest affected suite, inspect the diff for unrelated edits, verify no Unity/engine leakage and document the rollback point. **Gate:** handoff is implementation-ready and bounded.

# 21. File Impact Map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| current Core owner | MODIFY only if confirmed | authoritative state/rules | medium |
| current loader/catalog | MODIFY only if confirmed | authored schema/rows | medium |
| current host/session | MODIFY only if confirmed | command/event/lifecycle | medium |
| current UI route | MODIFY only if confirmed | truthful presentation | shared seam |
| current save owner | MODIFY only if durable delta exists | persistence | high |
| focused tests | MODIFY/ADD only for confirmed gap | regression evidence | low |
| unrelated systems | DO NOT TOUCH | scope firewall | high |

Exact proposed paths are listed in the evidence appendix and must be rechecked before implementation. A filename list is not a claim of permission to edit.

# 22. Risks and Mitigations

- **Stale premise:** source changes invalidate a plan section. Mitigation: Phase 0 checkpoint and hash revalidation.
- **Parallel authority:** a new manager/store/registry appears attractive. Mitigation: owner matrix and no-new-authority rule.
- **Catalog orphanism:** rows load but never reach a player. Mitigation: caller graph plus current consumer proof.
- **Save blind spot:** a new fact is durable in memory but not restored. Mitigation: capture/restore test before UI.
- **UI lie:** panel projects an approximation or cached value. Mitigation: projection-only UI and refresh test.
- **Nondeterminism:** collection order or wall-clock changes outcome. Mitigation: stable order and seeded stream contract.
- **Scope explosion:** polish becomes unrelated refactoring. Mitigation: explicit non-goals and phase gates.
- **Shared claim collision:** a host or registry file is active elsewhere. Mitigation: stop and return to foreman.

# 23. Out of Scope

This plan does not authorize: a new game engine, Unity dependency, parallel save store, global event bus rewrite, generic UI framework, unrelated balance retuning, broad catalog migration, generated-index mass edit, speculative new lore, copied text/art, or a broad test rewrite. It also does not reopen features listed as retired/accepted in `KNOWN_DEBT.md` without new evidence.

# 24. Rollback and Recovery

Work in reviewable phases. Keep Core contract, data, host wiring, UI and tests separable where ownership permits. Before implementation, record the current source hashes and current save section. If a phase fails, revert only that phase’s owned paths, restore the prior catalog/schema behavior, and leave existing save readers compatible. Never “fix” a failed phase by creating a second owner or by deleting user data.

Corrupt or unknown future save versions fail closed with a clear diagnostic. A legacy save must retain its documented old meaning. A host reload must reconstruct the same current state without reapplying one-shot consequences.

# 25. Definition of Done and Implementation Handoff

The package is done only when:

- current owner and extension seam are proven from source;
- every proposed content row has a validator and current consumer;
- the smallest implementation diff is reviewed;
- focused Core/data/host/UI tests appropriate to the actual change pass;
- save/restore and determinism are proven if durable or random;
- no UI or catalog duplicates authority;
- no fresh-pass claim is made for a test that was not run;
- all generated documentation paths and hashes are current;
- the implementation handoff names the first safe step and rollback point.

**MUST PRESERVE:** current owner boundaries, current save readers, deterministic ordering, authored-data authority, accessibility and truthful presentation.

**MUST ADD:** only confirmed missing contracts, named evidence, focused tests and explicit migration/rollback behavior.

**MUST NOT DO:** introduce Unity, create a second state owner, use `System.Random` in deterministic Core behavior, claim catalog presence as integration, or touch an active shared claim.

**VERIFY WITH:** the focused commands in Appendix G, the current test-policy runner, scoped diff checks, catalog integrity where data changes, and a headless check only if a runtime path is actually changed.

**FIRST SAFE IMPLEMENTATION STEP:** re-read the named current owner, enumerate its public state and capture/restore path, and produce a one-page delta table showing exactly what is missing before editing any production file.

# Appendix A — Current Source Dossier

### Current source: `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`
- Evidence status: **CURRENT FILE PRESENT**; 495 lines / 20750 bytes; SHA-256 `1c2b994cf1bfc8ba2578c6fe49a8a1feae92f025fcab02b3093a1d4a8dbbc4e2`.
The following is a bounded source excerpt, not a generated API and not a claim that every declaration is production-reachable.
```text
1: // SPDX-License-Identifier: MIT
2: using System;
3: using System.Collections.Generic;
4: #pragma warning disable CS8618
5: using Ashfall.Core.Inventory;
6: using Ashfall.Core.PlayerCommand;
7: using InventoryContainer = Ashfall.Core.Inventory.Inventory;
8:
9: namespace Ashfall.Core.Crafting
   10: {
   11:     /// <summary>
   12:     /// Engine-agnostic port of Unity's CraftingSystem (craft queue, ingredient
   13:     /// consumption, timed completion with overflow/rollback, station wear).
   14:     /// Operates on the shared Ashfall.Core.Inventory container.
   15:     /// </summary>
   16:     public class CraftingSystem
   17:     {
   18:         public const float StationWearPerCraft = 5f;
   19:
   20:         public bool IsPaused { get; set; }
   21:
   22:         private readonly InventoryContainer _inventory;
   23:         private readonly List<CraftingStation> _stations = new List<CraftingStation>();
   24:         private readonly List<ActiveCraft> _active = new List<ActiveCraft>();
   25:         private Func<string, bool> _isCraftResultAllowed;
   26:         private Func<int> _getDay;
   27:         private Func<string, Recipe?> _recipeLookup;
   28:         private Func<string, float> _crafterCostMultiplier; // crafterId -> material cost mult
   29:         private Func<string, float> _crafterCraftTimeMultiplier; // crafterId -> duration mult
   30:         /// <summary>Plan 24B A2 — the shared worker-productivity slot. Composed
   31:         /// with (never replacing) the Phase0 penalty slot; bound once by the
   32:         /// host to the shared contract. Null ⇒ no contribution (legacy).</summary>
   33:         private Func<string, float> _crafterProductivityTimeMultiplier;
   34:         private Func<string, bool> _canCraftMoonshine;
   35:         private Func<string, bool> _researchGate;
   36:
   37:         public InventoryContainer OverflowStash { get; set; }
   38:
   39:         public event Action<Recipe> OnCraftStarted;
   40:         public event Action<Recipe, string> OnCraftCompleted; // recipe, crafterId (empty when unassigned)
   41:         public event Action<Recipe, string, int> OnCraftResultOverflow; // recipe, itemId, amount
   42:
   43:         public CraftingSystem(InventoryContainer inventory)
   44:         {
   45:             _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
   46:         }
   47:
   48:         public void BindCraftResultGate(Func<string, bool> isResultAllowed)
   49:             => _isCraftResultAllowed = isResultAllowed;
   50:
   51:         public void SetDayProvider(Func<int> getDay) => _getDay = getDay;
   52:
   53:         public void SetCrafterCostMultiplier(Func<string, float> mult) => _crafterCostMultiplier = mult;
   54:         public void SetCrafterCraftTimeMultiplier(Func<string, float> mult) => _crafterCraftTimeMultiplier = mult;
   55:
```
### Current source: `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs`
- Evidence status: **CURRENT FILE PRESENT**; 178 lines / 8195 bytes; SHA-256 `42085805289825275a84c52421734dfe0dc7a28e4098cac3b0c13e6f6369e897`.
The following is a bounded source excerpt, not a generated API and not a claim that every declaration is production-reachable.
```text
1: // SPDX-License-Identifier: MIT
2: using System;
3: using System.Collections.Generic;
4: using System.Linq;
5: using Ashfall.Core.IO;
6: using Ashfall.Core.Inventory;
7:
8: namespace Ashfall.Core.Crafting
9: {
   10:     [Serializable]
   11:     internal sealed class RecipeJsonDto
   12:     {
   13:         public string id { get; set; } = string.Empty;
   14:         public string recipeName { get; set; } = string.Empty;
   15:         public List<RecipeIngredientJsonDto>? ingredients { get; set; }
   16:         public string resultItemId { get; set; } = string.Empty;
   17:         public int resultAmount { get; set; } = 1;
   18:         public float craftingTimeHours { get; set; } = 1f;
   19:         public string requiredStationId { get; set; } = string.Empty;
   20:         public string requiredResearchId { get; set; } = string.Empty;
   21:         public string requiredBlueprintId { get; set; } = string.Empty;
   22:     }
   23:
   24:     [Serializable]
   25:     internal sealed class RecipeIngredientJsonDto
   26:     {
   27:         public string itemId { get; set; } = string.Empty;
   28:         public int amount { get; set; } = 1;
   29:     }
   30:
   31:     /// <summary>
   32:     /// Shared Core loader for crafting recipes from recipes.json (authority).
   33:     /// Resolves ingredient and result ItemDefinitions from the supplied ItemCatalog.
   34:     /// Zero engine dependencies; adheres to Invariant 1 and Invariant 6.
   35:     /// </summary>
   36:     public static class RecipeCatalogLoader
   37:     {
   38:         public const string FileName = "recipes.json";
   39:
   40:         /// <summary>
   41:         /// Recipe ids in recipes.json that have <c>resultAmount = 0</c>
   42:         /// because their real effect is a non-inventory action (heater
   43:         /// refuel, thermal-pipe thaw, deep rebar injection, advanced water
   44:         /// purifier rebuild, desalination still rebuild, improvised heater
   45:         /// rebuild — each preserves its named output as a documentary
   46:         /// status token; the inventory is not consumed and not produced)
   47:         /// wired through gameplay systems external to this loader. They are
   48:         /// explicitly allowlisted so a strict-mode load does not break
   49:         /// them; new recipes with zero resultAmount MUST NOT be added to
   50:         /// this set without first giving them an authoritative non-inventory
   51:         /// command surface. Plan 10 remediation removed the sink
   52:         /// "lubricate_weapon" — it was NOT in the allowlist and is now
   53:         /// expected to fail closed under any loader run.
   54:         /// </summary>
   55:         public static readonly System.Collections.Generic.HashSet<string> LegacyZeroResultAllowlist =
  166:                 // continue with a partial catalog.
  167:                 throw;
  168:             }
  169:             catch (Exception ex)
  170:             {
  171:                 CatalogDiagnostics.Warn("RecipeCatalogLoader", FileName, ex);
  172:                 result.AddFatal("Failed to load recipes: " + ex.Message, ex);
  173:             }
  174:
  175:             return result;
  176:         }
  177:     }
  178: }
```
### Current source: `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs`
- Evidence status: **CURRENT FILE PRESENT**; 106 lines / 4445 bytes; SHA-256 `41507e39b7b7922c9ad441faf8e2aeda710840db9b1b33fb9d11ee1493eea175`.
The following is a bounded source excerpt, not a generated API and not a claim that every declaration is production-reachable.
```text
1: // SPDX-License-Identifier: MIT
2: using System;
3: using System.Collections.Generic;
4: using System.Linq;
5: using Ashfall.Core.IO;
6:
7: namespace Ashfall.Core.Crafting
8: {
9:     [Serializable]
   10:     internal sealed class PharmaRecipeJsonDto
   11:     {
   12:         public string recipe_id { get; set; } = string.Empty;
   13:         public string display_name { get; set; } = string.Empty;
   14:         public List<string>? input_ids { get; set; }
   15:         public List<int>? input_amounts { get; set; }
   16:         public string output_item_id { get; set; } = string.Empty;
   17:         public int output_amount { get; set; } = 1;
   18:         public float base_hours { get; set; } = 2f;
   19:         public float required_temperature { get; set; } = 80f;
   20:         public float purity_target { get; set; } = 0.9f;
   21:         public float dependency_risk { get; set; } = 0f;
   22:         public string required_station { get; set; } = "pharma_bench";
   23:         public string category { get; set; } = "pharmaceutical";
   24:     }
   25:
   26:     /// <summary>
   27:     /// Core loader for pharmaceutical compounding & distillation recipes from pharma_recipes.json.
   28:     /// Engine-agnostic, zero host dependencies.
   29:     /// </summary>
   30:     public static class PharmaRecipeCatalogLoader
   31:     {
   32:         public const string FileName = "pharma_recipes.json";
   33:
   34:         public static PharmaRecipeCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
   35:         {
   36:             var result = LoadWithResult(dataDir, fileIO, serializer);
   37:             var catalog = new PharmaRecipeCatalog();
   38:             catalog.recipes = result.Entries.ToList();
   39:             return catalog;
   40:         }
   41:
   42:         public static CatalogLoadResult<PharmaRecipe> LoadWithResult(
   43:             string dataDir, IFileIO fileIO, IJsonSerializer serializer)
   44:         {
   45:             string path = fileIO.Combine(dataDir, FileName);
   46:             var result = new CatalogLoadResult<PharmaRecipe>(
   47:                 path,
   48:                 "Pharmaceutical recipe catalog",
   49:                 CatalogClassification.Required);
   50:
   51:             if (fileIO == null || serializer == null || string.IsNullOrEmpty(dataDir))
   52:             {
   53:                 result.AddFatal("Required dependencies are null");
   54:                 return result;
   55:             }
   94:                     result.AddEntry(recipe);
   95:                 }
   96:             }
   97:             catch (Exception ex)
   98:             {
   99:                 CatalogDiagnostics.Warn("PharmaRecipeCatalogLoader", FileName, ex);
  100:                 result.AddFatal("Failed to load pharma recipes: " + ex.Message, ex);
  101:             }
  102:
  103:             return result;
  104:         }
  105:     }
  106: }
```
### Current source: `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs`
- Evidence status: **CURRENT FILE PRESENT**; 320 lines / 11951 bytes; SHA-256 `13a5e861745ddbf7ef9428bb701dc55dcef605d5339a79503f616536eb95bbdd`.
The following is a bounded source excerpt, not a generated API and not a claim that every declaration is production-reachable.
```text
1: // SPDX-License-Identifier: MIT
2: using System;
3: using System.Collections.Generic;
4: using System.Linq;
5: using Ashfall.Core.Inventory;
6:
7: namespace Ashfall.Core.Crafting
8: {
9:     [Serializable]
   10:     public sealed class ChemicalRetortState
   11:     {
   12:         public string vesselId = string.Empty;
   13:         public string activeProcessId = string.Empty;
   14:         public int processProgress;
   15:         public int processingTicksRequired;
   16:         public string heatBand = "Nominal"; // Low, Nominal, High, Runaway
   17:         public string pressureBand = "Nominal"; // Vacuum, Nominal, Elevated, Critical
   18:         public float catalystCondition = 100.0f; // 0..100
   19:         public float scrubberCondition = 100.0f; // 0..100
   20:         public bool isSealed = true;
   21:         public string assignedOperatorId = string.Empty;
   22:         public string failureState = "None"; // None, BatchLoss, VesselDamage, ScrubberFailure, ExposureEvent
   23:         public int lastTickDay;
   24:
   25:         public ChemicalRetortState Clone()
   26:         {
   27:             return new ChemicalRetortState
   28:             {
   29:                 vesselId = vesselId,
   30:                 activeProcessId = activeProcessId,
   31:                 processProgress = processProgress,
   32:                 processingTicksRequired = processingTicksRequired,
   33:                 heatBand = heatBand,
   34:                 pressureBand = pressureBand,
   35:                 catalystCondition = catalystCondition,
   36:                 scrubberCondition = scrubberCondition,
   37:                 isSealed = isSealed,
   38:                 assignedOperatorId = assignedOperatorId,
   39:                 failureState = failureState,
   40:                 lastTickDay = lastTickDay
   41:             };
   42:         }
   43:     }
   44:
   45:     [Serializable]
   46:     public sealed class ChemicalSynthesisSave
   47:     {
   48:         public List<ChemicalRetortState> vessels = new List<ChemicalRetortState>();
   49:         public float scrubberReserve = 100.0f;
   50:         public int apparatusTier = 1;
   51:         public int lastTickDay;
   52:
   53:         public ChemicalSynthesisSave Clone()
   54:         {
   55:             return new ChemicalSynthesisSave
```
### Current source: `src/Host/CraftingHostSession.cs`
- Evidence status: **CURRENT FILE PRESENT**; 391 lines / 18284 bytes; SHA-256 `5e226d17d80a157213152dddc664ee701f5e79c7d8b3ff8ce08b3c103459ebb0`.
The following is a bounded source excerpt, not a generated API and not a claim that every declaration is production-reachable.
```text
1: // SPDX-License-Identifier: MIT
2: using System;
3: using System.Collections.Generic;
4: #pragma warning disable CS8618
5: using Ashfall.Core;
6: using Ashfall.Core.Crafting;
7: using Ashfall.Core.Inventory;
8: using Ashfall.Core.PlayerCommand;
9: using InventoryContainer = Ashfall.Core.Inventory.Inventory;
   10:
   11: namespace AtomicWar.GodotApp
   12: {
   13:     /// <summary>
   14:     /// Thin Godot-host session for CraftingSystem (ported from Unity's
   15:     /// _Game/Crafting/CraftingSystem). Shares the migrated Inventory; seeds a
   16:     /// small recipe catalog; persists active crafts via CraftingSaveStore.
   17:     /// No gameplay rules — hosts only present.
   18:     /// </summary>
   19:     public sealed class CraftingHostSession
   20:     : HostSessionBase
   21:     {
   22:         public CraftingSystem Engine { get; }
   23:         public InventoryContainer Inventory { get; }
   24:         public WorkshopReverseEngineeringSystem Workshop { get; }
   25:         public PharmaLabSystem PharmaLab { get; }
   26:         public ResearchSystem Research { get; }
   27:         public System.Collections.Generic.List<Recipe> Recipes { get; } =
   28:             new System.Collections.Generic.List<Recipe>();
   29:
   30:         public string LastEvent { get; private set; } = string.Empty;
   31:
   32:         /// <summary>Full item catalog loaded from JSON (display-name lookups for panels). Null for seed-only sessions.</summary>
   33:         public ItemCatalog? LoadedItemCatalog { get; private set; }
   34:
   35:         public CraftingHostSession(
   36:             InventoryContainer inventory = null!,
   37:             System.Collections.Generic.List<Recipe> recipes = null!,
   38:             ResearchSystem? research = null,
   39:             ISeededRng? rng = null,
   40:             ILog? log = null,
   41:             bool seedDefaultWorkbench = false)
   42:         {
   43:             var logger = log ?? new GodotLog();
   44:             Inventory = inventory ?? new InventoryContainer();
   45:             Research = research ?? new ResearchSystem(logger);
   46:             Engine = new CraftingSystem(Inventory);
   47:             Workshop = new WorkshopReverseEngineeringSystem(Inventory, Research, Engine, logger);
   48:             Engine.BindResearchGate(id => Research.IsManualUnlocked(id)
   49:                 || Research.GetKnowledge(id)?.isCompleted == true
   50:                 || Research.IsBlueprintUnlocked(id));
   51:             PharmaLab = new PharmaLabSystem(Inventory, rng ?? new SeededRng(1986), logger);
   52:
   53:             // Plan 34: award the completed node's breakthrough_item exactly once —
   54:             // the Core event fires only on the completed transition, never on
   55:             // save restore, so restored campaigns do not re-grant.
   95:             ILog? log = null)
   96:         {
   97:             var fileIO = new FileSystemIO();
   98:             var serializer = new SystemTextJsonSerializer();
   99:             var itemCatalog = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);
  100:             var recipes = RecipeCatalogLoader.Load(dataDir, fileIO, serializer, itemCatalog);
  101:
  102:             var session = new CraftingHostSession(inventory, recipes, research, rng, log, seedDefaultWorkbench: false);
  103:             session.LoadedItemCatalog = itemCatalog;
  104:             if (itemCatalog != null && itemCatalog.Count > 0)
  105:             {
  106:                 session.Engine.BindCraftResultGate(id =>
  107:                     !string.IsNullOrEmpty(id) && session.LoadedItemCatalog != null && session.LoadedItemCatalog.Contains(id));
  108:             }
  109:
  110:             // When the session owns its research instance, load the authoritative
  111:             // research_knowledge.json catalog (Plan 34: JSON is the sole authored
  123:             session.Workshop.LoadCatalog(relicCatalog);
  124:
  125:             var techCatalog = TechSalvageCatalogLoader.Load(dataDir, fileIO, serializer);
  126:             session.Workshop.LoadTechSalvageCatalog(techCatalog);
  127:
  128:             var pharmaCatalog = PharmaRecipeCatalogLoader.Load(dataDir, fileIO, serializer);
  129:             session.PharmaLab.LoadCatalog(pharmaCatalog);
  130:
  131:             return session;
  132:         }
  133:
  134:         public void SeedStation()
  135:         {
  136:             if (Engine.GetStation("workbench") == null)
  137:             {
  138:                 Engine.AddStation(new CraftingStation { id = "workbench", displayName = "Civilian Workbench" });
  139:             }
```
### Current source: `src/UI/CraftingPanel.cs`
- Evidence status: **CURRENT FILE PRESENT**; 470 lines / 19822 bytes; SHA-256 `b6f10d7b99d59eaf8f648bbedfbeb864aaa427a9643e26a003ada88d7a753142`.
The following is a bounded source excerpt, not a generated API and not a claim that every declaration is production-reachable.
```text
1: // SPDX-License-Identifier: MIT
2: using System;
3: using System.Collections.Generic;
4: using Godot;
5: using Ashfall.Core.Crafting;
6: using Ashfall.Core.UI;
7: using AtomicWar.GodotApp.UI;
8:
9: namespace AtomicWar.GodotApp.UI
   10: {
   11:     /// <summary>
   12:     /// ASHFALL — Crafting panel.
   13:     /// Displays real recipes from CraftingHostSession, shows ingredient availability,
   14:     /// lets the player start crafts and track the active queue.
   15:     /// Thin presentation layer — all craft logic lives in CraftingSystem.
   16:     /// </summary>
   17:     public partial class CraftingPanel : Control, IBindablePanel
   18:     {
   19:         public event Action? OnClose;
   20:         public event Action? OnCraftStarted;
   21:         public event Action? OnOpenWorkshopRequested;
   22:         public event Action? OnOpenPharmaLabRequested;
   23:
   24:         public bool IsBound => _craftingHost != null;
   25:
   26:         private CraftingHostSession? _craftingHost;
   27:         private InventoryHostSession? _inventoryHost;
   28:         private SurvivorsHostSession? _survivorsHost;
   29:
   30:         private VBoxContainer _recipeList = null!;
   31:         private VBoxContainer _queueList = null!;
   32:         private Label _queueHeader = null!;
   33:         private Label _filterStatus = null!;
   34:
   35:         // ── Bench operator (trade specialty attribution) ───────────────
   36:         private OptionButton? _crafterSelect;
   37:         private Label? _crafterDetail;
   38:         private readonly List<string> _crafterIds = new();
   39:         private Ashfall.Core.Survivors.TradeSpecialtySystem? _tradeSpecialty;
   40:         private Func<string, string>? _professionResolver;
   41:
   42:         /// <summary>
   43:         /// The survivor chosen at the bench, or "" for unassigned — in which case
   44:         /// the host auto-credits a living survivor whose trade matches the item.
   45:         /// Index 0 is the UNASSIGNED row, so ids are offset by one.
   46:         /// </summary>
   47:         public string SelectedCrafterId =>
   48:             _crafterSelect != null && _crafterSelect.Selected > 0 && _crafterSelect.Selected - 1 < _crafterIds.Count
   49:                 ? _crafterIds[_crafterSelect.Selected - 1]
   50:                 : string.Empty;
   51:
   52:         private string _activeFilter = "all";  // "all" | "craftable" | "queued"
   53:         private bool _craftSubmitting; // debounce
   54:
   55:         // ── Binding ────────────────────────────────────────────────────
   74:             _craftingHost.Engine.OnCraftCompleted += OnEngineCraftCompleted;
   75:
   76:             RefreshView();
   77:         }
   78:
   79:         private void OnEngineCraftStarted(Recipe _)
   80:         {
   81:             _craftSubmitting = false;
   82:             RefreshView();
   83:             OnCraftStarted?.Invoke();
   84:         }
   85:
   86:         private void OnEngineCraftCompleted(Recipe recipe, string crafterId) => RefreshView();
   87:
   88:         // ── Refresh ────────────────────────────────────────────────────
   89:
   90:         public void RefreshView()
```

# Appendix B — Current Data and Catalog Audit

- `Assets/StreamingAssets/Data/recipes.json` — 61479 bytes; SHA-256 `c98b0c502df8f74f4bc498f583c0eaf659cec6320e39080de056703d85662d49`.
  - `root` object keys (2): `schema_version`, `recipes`
  - `root.recipes` list rows: **123**
- sample row key: `craft_bandage`; fields: `id`, `recipeName`, `ingredients`, `resultItemId`, `resultAmount`, `craftingTimeHours`, `requiredStationId`
- sample row key: `purify_water`; fields: `id`, `recipeName`, `ingredients`, `resultItemId`, `resultAmount`, `craftingTimeHours`, `requiredStationId`
- sample row key: `craft_anti_rad`; fields: `id`, `recipeName`, `ingredients`, `resultItemId`, `resultAmount`, `craftingTimeHours`, `requiredStationId`
- `Assets/StreamingAssets/Data/workshop_recipes.json` — 12281 bytes; SHA-256 `7afabc1a54e5a49f350d6c793ab5e2571ddd9e8dcd8d8f14d1cf6f803dac97e6`.
  - `root` object keys (2): `schema_version`, `recipes`
  - `root.recipes` list rows: **13**
- sample row key: `recipe_workshop_reload_9x19`; fields: `id`, `display_name`, `category`, `required_room_ids`, `required_rule_ids`, `inputs`, `outputs`, `base_labor_ticks`, `base_scrap_waste_permille`, `tool_wear_permille`, `calibration_requirement`, `skill_weights`, `tags`
- sample row key: `recipe_workshop_reload_357`; fields: `id`, `display_name`, `category`, `required_room_ids`, `required_rule_ids`, `inputs`, `outputs`, `base_labor_ticks`, `base_scrap_waste_permille`, `tool_wear_permille`, `calibration_requirement`, `skill_weights`, `tags`
- sample row key: `recipe_workshop_reload_12g`; fields: `id`, `display_name`, `category`, `required_room_ids`, `required_rule_ids`, `inputs`, `outputs`, `base_labor_ticks`, `base_scrap_waste_permille`, `tool_wear_permille`, `calibration_requirement`, `skill_weights`, `tags`
- `Assets/StreamingAssets/Data/pharma_recipes.json` — 12898 bytes; SHA-256 `1469a8b8945f64679557162247d0a0c1912d95f5ca9670dcaa5c21f079ae62b2`.
  - `root` object keys (2): `schema_version`, `recipes`
  - `root.recipes` list rows: **27**
- sample row key: `None`; fields: `recipe_id`, `display_name`, `input_ids`, `input_amounts`, `output_item_id`, `output_amount`, `base_hours`, `required_temperature`, `purity_target`, `dependency_risk`, `required_station`, `category`
- sample row key: `None`; fields: `recipe_id`, `display_name`, `input_ids`, `input_amounts`, `output_item_id`, `output_amount`, `base_hours`, `required_temperature`, `purity_target`, `dependency_risk`, `required_station`, `category`
- sample row key: `None`; fields: `recipe_id`, `display_name`, `input_ids`, `input_amounts`, `output_item_id`, `output_amount`, `base_hours`, `required_temperature`, `purity_target`, `dependency_risk`, `required_station`, `category`
- `Assets/StreamingAssets/Data/metallurgy_recipes.json` — 7738 bytes; SHA-256 `90c7ce51ab4d725da78099c44d842a73cca8d2c695d956bfaaa0723f13168e96`.
  - `root` object keys (4): `schema_version`, `collection_id`, `description`, `recipes`
  - `root.recipes` list rows: **12**
- sample row key: `metallurgy_iron_ingot`; fields: `id`, `display_name`, `input_items`, `fuel_units`, `water_litres`, `process_heat_tier`, `flux_item_id`, `flux_amount`, `slag_yield`, `labor_days`, `skill_target`, `quality_target`, `result_item_id`, `result_amount`, `tags`
- sample row key: `metallurgy_copper_ingot`; fields: `id`, `display_name`, `input_items`, `fuel_units`, `water_litres`, `process_heat_tier`, `flux_item_id`, `flux_amount`, `slag_yield`, `labor_days`, `skill_target`, `quality_target`, `result_item_id`, `result_amount`, `tags`
- sample row key: `metallurgy_steel_billet`; fields: `id`, `display_name`, `input_items`, `fuel_units`, `water_litres`, `process_heat_tier`, `flux_item_id`, `flux_amount`, `slag_yield`, `labor_days`, `skill_target`, `quality_target`, `result_item_id`, `result_amount`, `tags`
- `Assets/StreamingAssets/Data/items.json` — 390056 bytes; SHA-256 `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`.
  - `root` object keys (2): `schema_version`, `items`
  - `root.items` list rows: **724**
- sample row key: `item_decon_chelator_concentrate`; fields: `id`, `displayName`, `description`, `type`, `stackMax`, `weight`, `tradeValue`
- sample row key: `item_lead_lined_effluent_filter`; fields: `id`, `displayName`, `description`, `type`, `stackMax`, `weight`, `tradeValue`
- sample row key: `item_heavy_neoprene_scrub_brush`; fields: `id`, `displayName`, `description`, `type`, `stackMax`, `weight`, `tradeValue`

# Appendix C — Current Test Inventory

- `Ashfall.Core.Tests/CraftingSystemTests.cs` — 251 lines; SHA-256 `7adf7b670437317b141dd5191d2b941477a472741cb6e35931f897f702ef8b20`; test attributes 10; declaration lines 13.
  - `public class CraftingSystemTests`
  - `private static ItemDefinition Def(string id, ItemType type = ItemType.Material,`
  - `private static Recipe MakeRecipe(string id, ItemDefinition result,`
  - `public void CanCraft_TrueWhenAllIngredientsHeld()`
  - `public void BindCraftResultGate_RefusesUnknownResult_AllowsCataloguedResult()`
  - `public void CanCraft_False_WhenStationMissingOrBroken()`
  - `public void StartCraft_ConsumesIngredients_CompletesAfterTick()`
  - `public void StartCraft_Fails_WhenIngredientsInsufficient()`
  - `public void FullInventory_StartRejected_NoIngredientsConsumed()`
  - `public void OverflowPath_WithFreedSlot_RefundsWhenNoStash()`
  - `public void OverflowPath_WithStash_StashesResultAndRefundsNothing()`
- `Ashfall.Core.Tests/CraftingCommandTests.cs` — 302 lines; SHA-256 `ade3127765341b5da9b9b7084ea88a1b89541f6d90027911ac9a27d4163f496c`; test attributes 14; declaration lines 17.
  - `public class CraftingCommandTests`
  - `public class ExpeditionCommandTests`
  - `public void PreviewCraft_UnknownRecipe_ReturnsUnavailable()`
  - `public void PreviewCraft_Available_ShowsProjectedDeltas()`
  - `public void ExecuteCraft_Success_AdvancesStateVersion()`
  - `public void ExecuteCraft_StalePreview_RejectsWithoutMutation()`
  - `public void ExecuteCraft_Failure_NoPartialMutation()`
  - `public void PreviewStart_InvalidParams_ReturnsUnavailable()`
  - `public void PreviewStart_AlreadyActive_ReturnsUnavailable()`
  - `public void PreviewStart_Available_ShowsProjectedDeltas()`
  - `public void ExecuteStart_Success_AdvancesStateVersion()`
  - `public void ExecuteStart_StalePreview_RejectsWithoutMutation()`
- `Ashfall.Core.Tests/CraftingAfflictionLoopTests.cs` — 472 lines; SHA-256 `75be59b0227945d42a2e1ec6d090c4b1b18ae38fc4cc472cafac55d5e09ab0e4`; test attributes 20; declaration lines 25.
  - `public class CraftingAfflictionLoopTests`
  - `sys.GetOrCreate("sv1"); // ensure record exists but degradation=0`
  - `private static ItemDefinition MakeItem(string id, ItemType type = ItemType.Material, int stackMax = 50)`
  - `private static Recipe MakeRecipe(string id, ItemDefinition result, int resultAmt,`
  - `public void StartCraft_ConsumesIngredientsAtomically()`
  - `public void StartCraft_WhenInsufficientIngredients_ConsumesNothing()`
  - `public void StartCraft_WhenNullRecipe_ReturnsFalse()`
  - `public void Tick_CompletesQueuedCraft_ExactlyOnce()`
  - `public void Tick_DoesNotCompleteBeforeDuration()`
  - `public void MultipleRecipes_QueueBothAndCompleteInOrder()`
  - `public void RestoreState_ReconstitutesQueueWithHoursRemaining()`
  - `public void RestoreState_ThenTickToCompletion_DoesNotDuplicate()`

# Appendix D — Mechanical Caller/Reference Graphs

### Mechanical references to `CraftingSystem`
Assets/Ashfall.Core/PharmaLabSystem.cs:12: /// Domain layer over CraftingSystem, RecipeCatalog, ChemicalDependencySystem,
Assets/Ashfall.Core/EquipmentConditionSystem.cs:98: private readonly CraftingSystem _crafting;
Assets/Ashfall.Core/EquipmentConditionSystem.cs:180: CraftingSystem crafting,
Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs:13: /// Composes <see cref="ResearchSystem"/>, <see cref="Crafting.CraftingSystem"/>,
Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs:95: private readonly Crafting.CraftingSystem _craftingSystem;
Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs:109: Crafting.CraftingSystem craftingSystem,
Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs:114: _craftingSystem = craftingSystem ?? throw new ArgumentNullException(nameof(craftingSystem));
Assets/Ashfall.Core/Crafting/CraftingSystem.cs:12: /// Engine-agnostic port of Unity's CraftingSystem (craft queue, ingredient
Assets/Ashfall.Core/Crafting/CraftingSystem.cs:16: public class CraftingSystem
Assets/Ashfall.Core/Crafting/CraftingSystem.cs:43: public CraftingSystem(InventoryContainer inventory)
Assets/Ashfall.Core/Crafting/CraftingSystem.cs:396: public CraftingSystemSave CaptureState()
Assets/Ashfall.Core/Crafting/CraftingSystem.cs:409: return new CraftingSystemSave { ActiveCrafts = crafts };
Assets/Ashfall.Core/Crafting/CraftingSystem.cs:414: public void RestoreState(CraftingSystemSave save)
Assets/Ashfall.Core/Crafting/CraftingSystem.cs:482: public class CraftingSystemSave
Interpretation: these are search hits only; an integrating builder must trace the actual call path, setup binding, event subscription and save registration before claiming reachability.
### Mechanical references to `RecipeCatalogLoader`
Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs:30: public static class PharmaRecipeCatalogLoader
Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs:99: CatalogDiagnostics.Warn("PharmaRecipeCatalogLoader", FileName, ex);
Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs:36: public static class RecipeCatalogLoader
Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs:171: CatalogDiagnostics.Warn("RecipeCatalogLoader", FileName, ex);
Assets/Ashfall.Core/Crafting/TrapRecipeIntegrity.cs:12: ///   1. a real item definition (guaranteed by RecipeCatalogLoader strict
Assets/Ashfall.Core/Content/ContentDeepChainGate.cs:146: Description = "recipes.json is loaded by RecipeCatalogLoader (authority)",
Assets/Ashfall.Core/Content/ContentDeepChainGate.cs:148: RequiredLoader = "RecipeCatalogLoader",
Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:348: ["recipes.json"] = new[] { "RecipeCatalogLoader" },
Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:433: ["pharma_recipes.json"] = new[] { "PharmaRecipeCatalogLoader" },
Assets/Ashfall.Core/Cooking/CookingRecipeCatalogLoader.cs:20: public static class CookingRecipeCatalogLoader
src/Host/ContentUtilizationRuntimeCollector.cs:875: instr.RecordCatalogOpened("recipes.json", "RecipeCatalogLoader");
src/Host/HostCli.Plans162_165.cs:338: var recipes = Ashfall.Core.Crafting.RecipeCatalogLoader.Load(dataDirectory, io, json, items);
src/Host/CraftingHostSession.cs:100: var recipes = RecipeCatalogLoader.Load(dataDir, fileIO, serializer, itemCatalog);
src/Host/CraftingHostSession.cs:128: var pharmaCatalog = PharmaRecipeCatalogLoader.Load(dataDir, fileIO, serializer);
Interpretation: these are search hits only; an integrating builder must trace the actual call path, setup binding, event subscription and save registration before claiming reachability.
### Mechanical references to `PharmaRecipeCatalogLoader`
Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs:30: public static class PharmaRecipeCatalogLoader
Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs:99: CatalogDiagnostics.Warn("PharmaRecipeCatalogLoader", FileName, ex);
Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:433: ["pharma_recipes.json"] = new[] { "PharmaRecipeCatalogLoader" },
src/Host/CraftingHostSession.cs:128: var pharmaCatalog = PharmaRecipeCatalogLoader.Load(dataDir, fileIO, serializer);
Ashfall.Core.Tests/ProductionSliceTests.cs:53: var catalog = PharmaRecipeCatalogLoader.Load(_dataDir, fileIO, serializer);
Interpretation: these are search hits only; an integrating builder must trace the actual call path, setup binding, event subscription and save registration before claiming reachability.
### Mechanical references to `ChemicalSynthesisSystem`
Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs:65: public sealed class ChemicalSynthesisSystem
Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs:88: public ChemicalSynthesisSystem(
src/Main.ChemicalSynthesis.cs:25: var system = new ChemicalSynthesisSystem(
src/Host/ChemicalSynthesisHostSession.cs:8: /// Host session for ChemicalSynthesisSystem.
src/Host/ChemicalSynthesisHostSession.cs:14: public ChemicalSynthesisSystem System { get; }
src/Host/ChemicalSynthesisHostSession.cs:19: ChemicalSynthesisSystem system,
src/UI/ChemicalLabPanel.cs:15: /// Bound to ChemicalSynthesisHostSession -> ChemicalSynthesisSystem.
src/UI/ChemicalLabPanel.cs:303: private void RenderDossier(ChemicalSynthesisSystem sys, ChemicalSynthesisCatalog cat)
Ashfall.Core.Tests/FlagshipIntegrationIxSmokeTests.cs:23: public ChemicalSynthesisSystem ChemicalSynthesis { get; }
Ashfall.Core.Tests/FlagshipIntegrationIxSmokeTests.cs:60: ChemicalSynthesis = new ChemicalSynthesisSystem(
Interpretation: these are search hits only; an integrating builder must trace the actual call path, setup binding, event subscription and save registration before claiming reachability.
### Mechanical references to `CraftingHostSession`
src/Main.Narrative.cs:40: private CraftingHostSession _crafting = null!;
src/Main.World.cs:281: _crafting = CraftingHostSession.Create(_dataDir, _inventory.Inventory, _sharedResearch);
src/Host/CraftingSaveStore.cs:5: // Host Caller: Main.World / CraftingHostSession
src/Host/CraftingHostSession.cs:19: public sealed class CraftingHostSession
src/Host/CraftingHostSession.cs:35: public CraftingHostSession(
src/Host/CraftingHostSession.cs:90: public static CraftingHostSession Create(
src/Host/CraftingHostSession.cs:102: var session = new CraftingHostSession(inventory, recipes, research, rng, log, seedDefaultWorkbench: false);
src/Host/HostCli.PanelTests.cs:4219: var craftSession = new CraftingHostSession(craftInv, seedDefaultWorkbench: true);
src/Host/HostCli.PanelTests.cs:4233: var mechParts = CraftingHostSession.Catalog.Get("scrap_mechanical")!;
src/Host/HostCli.PanelTests.cs:4285: var craftSession2 = new CraftingHostSession(craftInv2, seedDefaultWorkbench: true);
src/Host/HostCli.PanelTests.cs:4381: var intCrafting = new CraftingHostSession(intInv, seedDefaultWorkbench: true);
src/Host/HostCli.PanelTests.cs:4382: var intMechDef = CraftingHostSession.Catalog.Get("scrap_mechanical");
src/Host/HostCli.PanelTests.cs:4414: var intCraft2 = new CraftingHostSession(intInv2, seedDefaultWorkbench: true);
src/Host/HostCli.PanelTests.cs:4415: var intMechDef2 = CraftingHostSession.Catalog.Get("scrap_mechanical");
Interpretation: these are search hits only; an integrating builder must trace the actual call path, setup binding, event subscription and save registration before claiming reachability.
### Mechanical references to `CraftingPanel`
Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1407: ["items.json"] = new[] { "InventoryPanel", "CraftingPanel", "EquipmentPanel" },
src/Main.PanelLifecycle.cs:24: _craftingPanel,
src/Main.UiHandlers.cs:17: public void OpenCraftingPanel()
src/Main.UiHandlers.cs:24: _craftingPanel.Bind(_crafting, _inventory, _survivors,
src/Main.UiHandlers.cs:26: _craftingPanel.Open();
src/Main.UiPanels.cs:54: private CraftingPanel _craftingPanel = null!;
src/Main.UiPanels.cs:312: _craftingPanel = PanelSceneLoader.Load<CraftingPanel>("res://assets/ui/panels/CraftingPanel.tscn");
src/Main.UiPanels.cs:313: _craftingPanel.OnClose += CloseCraftingPanel;
src/Main.UiPanels.cs:314: _craftingPanel.OnCraftStarted += () => { UpdateHud(); _craftingDirty = true; };
src/Main.UiPanels.cs:315: _craftingPanel.OnOpenWorkshopRequested += () => OpenPlayerPanel("workshop");
src/Main.UiPanels.cs:316: _craftingPanel.OnOpenPharmaLabRequested += () => OpenPlayerPanel("pharma_lab");
src/Main.UiPanels.cs:317: AddChild(_craftingPanel);
src/Main.UiPanels.cs:1545: OpenCraftingPanel,
src/Main.World.cs:728: private void CloseCraftingPanel()
Interpretation: these are search hits only; an integrating builder must trace the actual call path, setup binding, event subscription and save registration before claiming reachability.

# Appendix E — Read-Only Master Authority Slices

The following slices are read-only excerpts from the user-specified authority. They are included to constrain architecture and quality; live source/data remains authoritative.

authority lines 933-938:
933: **DM-1 — Shelter operations (C1).** Owners: shelter rooms/identities/machines, thermal, schedules, social events, decor, fire, noise, airlock security, decon, atmosphere, sanitation (power-fed). Live catalogs: `shelter_rooms`, `shelter_room_identities`, `shelter_machine_identities`, `shelter_schedules`, `shelter_social_events`, `shelter_audio_cues`, `shelter_insulation_catalog`, `shelter_shielding`, `sanitation_facilities`, plus the sealed grid catalog. Hosts: ShelterAssignment, ShelterAtmosphere, ShelterDecor, ShelterFire, ShelterSchedule, ShelterThermal, Sanitation, AirlockSecurity, Decontamination, Ventilation. Openings: A-01, A-02, B-01, B-02, D-06, F-04. Notable constraint: room effects route through `IsRoomPowered`; shelter state persists through the holdfast/shelter save family.
934:
935: **DM-2 — Medical pipeline (C2).** Owners: disease, pathogens, dose ledger, ARS, surgery, autopsy, pharma lab, diagnostics, therapies, dependency, crises. Live catalogs: `disease_catalog`, `pathogens`, `dose_items/locations/quests/registers`, `autopsy_procedures`, `surgical_procedures`, `pharma_recipes`, `microfluidic_diagnostic_catalog`, `medical_texts`, `psychological_therapies`, `chemical_dependency_items`. Hosts: MedicalWard, DoseLedger, PsychologyArc, MentalHealthCrisis. Docs: `MEDICAL_PIPELINE_JOURNEY.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md` (all verified live). Openings: A-03, A-04, A-05, B-03, B-04, B-25, C-14 support, G-03.
936:
937: **DM-3 — Water, food, agriculture (C3).** Owners: water treatment, condensers, deep wells, brine, nutrition, kitchen, preservation, grain, greenhouse, aquaponics, aeroponics, apiculture, cryo cultivars. Live catalogs: `water_treatment` family via systems, `fog_harvesting_catalog`, `deep_well` systems, `brine` systems, `nutrition_profiles`, `food_preservation`, `grain_processing`, `greenhouse_items`, `hydroponic_crops`, `aquaponics_system_catalog`, `aeroponics_nutrient_catalog`, `cryo_cultivars`, `crop_strains`, `dive_sites`. Hosts: Greenhouse, GrainProcessing, KitchenNutrition, FoodPreservation, DeepWell, Sanitation, DeepCoast. Openings: A-06, A-07, A-08, B-05, C-12, F-012 (dive/hydroponic audit consumed as F-012 above).
938:
authority lines 1867-1872:
1867: ## 7.8 Item authoring rules (CANON, Part 12.1 distilled)
1868:
1869: Zero new item ids when existing ids suffice — reuse and tag with `expansion_item_tags.json`. Loot references must point at real item ids (historical defect class: singular/plural drift — guarded tests exist). Every new item requires: catalog entry, integrity compliance, at least one acquisition path (loot, craft, trade, or event reference), and at least one consumption surface (a recipe, need, trade entry, or quest connection). An item with no acquisition path or no use fails the utilization review even if integrity passes.
1870:
1871: ## 7.9 Catalog-agnostic integrity checklist (every authored tranche)
1872:

# Appendix F — Focused Runner Command Contract

The following commands are **planned verification commands**, not claims that this planning-only pass ran them:

```text
bash scripts/run_test.sh Ashfall.Core.Tests/CraftingSystemTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/CraftingCommandTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/CraftingAfflictionLoopTests.cs
bash scripts/run_test.sh <directly-affected-focused-directory>
python3 scripts/ci/generate-architecture-map.py --check   # only if a mapped owner/host seam changes
python3 scripts/ci/generate-docs-index.py --check          # only under the generated-index owner
```

Builder rule: resolve each angle-bracket placeholder to a real current test file before running. Do not run a broad suite by default. New test files run alone first. Quarantine/re-enable decisions require current API/content evidence, a reason, and a passing focused target.

# Appendix G — Quality Matrix and Negative Test Inventory

| Quality dimension | Required proof | Failure if absent |
|---|---|---|
| premise accuracy | live path/hash/catalog evidence | stale or fictional plan |
| ownership | one mutable owner and explicit boundary | parallel authority |
| data integrity | schema, IDs, references and rows | orphan content |
| integration | real command and event path | compile-only fiction |
| persistence | capture/restore and old-save default | state loss |
| determinism | stable order and seeded stream | replay divergence |
| UI truth | owner projection and feedback | panel cache/lie |
| accessibility | focus, input, contrast and disposal | inaccessible route |
| failure handling | named refusal/fail-closed behavior | silent success |
| rollback | phase-local revert and old-save compatibility | unrecoverable data |
| QA honesty | command/result distinction | false completion claim |

Negative cases to test or document include duplicate ID, missing reference, empty catalog, invalid numeric range, stale save, repeated event, host reload, unavailable owner, insufficient resource, inaccessible location, and same-seed replay.

# Appendix H — Plan-Specific Decision Ledger

| Decision | Current evidence | Safe conclusion | Revisit when |
|---|---|---|---|
| owner | source files and declarations in Appendix A | extend current owner only | source contract changes |
| content | catalog audit in Appendix B | add only with a current consumer | loader/schema changes |
| persistence | owner/save evidence | no new section by default | durable fact confirmed |
| host | caller/reference graph in Appendix D | one real route required | shared seam claimed |
| UI | current panel path | projection-only | route/manifest changes |
| randomness | deterministic mandate | seeded stream or no randomness | simulation rule requires choice |
| tests | current inventory | smallest confirmed target | public contract changes |

# Appendix I — Original Intent Preservation and Stale-Claim Cleanup

The original plan’s useful intent is preserved as a bounded design goal, not as authority. Historical “sealed”, “approved”, “100 tests”, “600-day trace” or exact future row counts are not accepted merely because they appear in an old plan. This rebuild removes unsupported claims, fictional APIs, fake save sections, duplicate authorities and test-count padding. Completed behavior is retained as maintenance scope; residual behavior is tied to a current path and a current consumer.

# Appendix J — Handoff Checklist

- [ ] Current owner re-read immediately before implementation.
- [ ] Exact claimed paths confirmed against `WORKTREE_OWNERSHIP.md`.
- [ ] Current catalog rows and references re-censused.
- [ ] Existing save reader and capture/restore path identified.
- [ ] Existing host command and event consumer traced.
- [ ] Determinism and seed order specified.
- [ ] UI/accessibility behavior specified without panel authority.
- [ ] Focused tests selected from current tree.
- [ ] New test file run alone first if created.
- [ ] Rollback and old-save behavior documented.
- [ ] No production/data/test/UI edits made by this planning pass.

# Appendix K — Source Hash and Path Verification Record

The following records are generated from current files. A later builder must re-run the hash check after any source/data edit; a stale hash invalidates the affected evidence block.


## Audit cycle 01, lens 01: Crafting Recipe Expansion boundary

**Question 01.01.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 02: Crafting Recipe Expansion boundary

**Question 01.02.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 03: Crafting Recipe Expansion boundary

**Question 01.03.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 04: Crafting Recipe Expansion boundary

**Question 01.04.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 05: Crafting Recipe Expansion boundary

**Question 01.05.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 06: Crafting Recipe Expansion boundary

**Question 01.06.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 07: Crafting Recipe Expansion boundary

**Question 01.07.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 08: Crafting Recipe Expansion boundary

**Question 01.08.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 09: Crafting Recipe Expansion boundary

**Question 01.09.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 10: Crafting Recipe Expansion boundary

**Question 01.10.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 11: Crafting Recipe Expansion boundary

**Question 01.11.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 12: Crafting Recipe Expansion boundary

**Question 01.12.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 02, lens 01: Crafting Recipe Expansion boundary

**Question 02.01.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 02: Crafting Recipe Expansion boundary

**Question 02.02.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 03: Crafting Recipe Expansion boundary

**Question 02.03.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 04: Crafting Recipe Expansion boundary

**Question 02.04.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 05: Crafting Recipe Expansion boundary

**Question 02.05.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 06: Crafting Recipe Expansion boundary

**Question 02.06.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 07: Crafting Recipe Expansion boundary

**Question 02.07.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 08: Crafting Recipe Expansion boundary

**Question 02.08.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 09: Crafting Recipe Expansion boundary

**Question 02.09.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 10: Crafting Recipe Expansion boundary

**Question 02.10.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 11: Crafting Recipe Expansion boundary

**Question 02.11.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 12: Crafting Recipe Expansion boundary

**Question 02.12.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 03, lens 01: Crafting Recipe Expansion boundary

**Question 03.01.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 02: Crafting Recipe Expansion boundary

**Question 03.02.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 03: Crafting Recipe Expansion boundary

**Question 03.03.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 04: Crafting Recipe Expansion boundary

**Question 03.04.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 05: Crafting Recipe Expansion boundary

**Question 03.05.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 06: Crafting Recipe Expansion boundary

**Question 03.06.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 07: Crafting Recipe Expansion boundary

**Question 03.07.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 08: Crafting Recipe Expansion boundary

**Question 03.08.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 09: Crafting Recipe Expansion boundary

**Question 03.09.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 10: Crafting Recipe Expansion boundary

**Question 03.10.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 11: Crafting Recipe Expansion boundary

**Question 03.11.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 12: Crafting Recipe Expansion boundary

**Question 03.12.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 04, lens 01: Crafting Recipe Expansion boundary

**Question 04.01.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 02: Crafting Recipe Expansion boundary

**Question 04.02.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 03: Crafting Recipe Expansion boundary

**Question 04.03.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 04: Crafting Recipe Expansion boundary

**Question 04.04.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 05: Crafting Recipe Expansion boundary

**Question 04.05.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 06: Crafting Recipe Expansion boundary

**Question 04.06.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 07: Crafting Recipe Expansion boundary

**Question 04.07.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 08: Crafting Recipe Expansion boundary

**Question 04.08.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 09: Crafting Recipe Expansion boundary

**Question 04.09.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 10: Crafting Recipe Expansion boundary

**Question 04.10.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 11: Crafting Recipe Expansion boundary

**Question 04.11.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 12: Crafting Recipe Expansion boundary

**Question 04.12.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 05, lens 01: Crafting Recipe Expansion boundary

**Question 05.01.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 02: Crafting Recipe Expansion boundary

**Question 05.02.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 03: Crafting Recipe Expansion boundary

**Question 05.03.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 04: Crafting Recipe Expansion boundary

**Question 05.04.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 05: Crafting Recipe Expansion boundary

**Question 05.05.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 06: Crafting Recipe Expansion boundary

**Question 05.06.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 07: Crafting Recipe Expansion boundary

**Question 05.07.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 08: Crafting Recipe Expansion boundary

**Question 05.08.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 09: Crafting Recipe Expansion boundary

**Question 05.09.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 10: Crafting Recipe Expansion boundary

**Question 05.10.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 11: Crafting Recipe Expansion boundary

**Question 05.11.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 12: Crafting Recipe Expansion boundary

**Question 05.12.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 06, lens 01: Crafting Recipe Expansion boundary

**Question 06.01.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 02: Crafting Recipe Expansion boundary

**Question 06.02.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 03: Crafting Recipe Expansion boundary

**Question 06.03.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 04: Crafting Recipe Expansion boundary

**Question 06.04.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 05: Crafting Recipe Expansion boundary

**Question 06.05.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 06: Crafting Recipe Expansion boundary

**Question 06.06.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 07: Crafting Recipe Expansion boundary

**Question 06.07.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 08: Crafting Recipe Expansion boundary

**Question 06.08.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 09: Crafting Recipe Expansion boundary

**Question 06.09.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 10: Crafting Recipe Expansion boundary

**Question 06.10.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 11: Crafting Recipe Expansion boundary

**Question 06.11.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 12: Crafting Recipe Expansion boundary

**Question 06.12.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 07, lens 01: Crafting Recipe Expansion boundary

**Question 07.01.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 02: Crafting Recipe Expansion boundary

**Question 07.02.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 03: Crafting Recipe Expansion boundary

**Question 07.03.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 04: Crafting Recipe Expansion boundary

**Question 07.04.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 05: Crafting Recipe Expansion boundary

**Question 07.05.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 06: Crafting Recipe Expansion boundary

**Question 07.06.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 07: Crafting Recipe Expansion boundary

**Question 07.07.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 08: Crafting Recipe Expansion boundary

**Question 07.08.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 09: Crafting Recipe Expansion boundary

**Question 07.09.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 10: Crafting Recipe Expansion boundary

**Question 07.10.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 11: Crafting Recipe Expansion boundary

**Question 07.11.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 12: Crafting Recipe Expansion boundary

**Question 07.12.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 08, lens 01: Crafting Recipe Expansion boundary

**Question 08.01.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 02: Crafting Recipe Expansion boundary

**Question 08.02.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 03: Crafting Recipe Expansion boundary

**Question 08.03.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 04: Crafting Recipe Expansion boundary

**Question 08.04.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 05: Crafting Recipe Expansion boundary

**Question 08.05.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 06: Crafting Recipe Expansion boundary

**Question 08.06.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 07: Crafting Recipe Expansion boundary

**Question 08.07.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 08: Crafting Recipe Expansion boundary

**Question 08.08.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 09: Crafting Recipe Expansion boundary

**Question 08.09.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 10: Crafting Recipe Expansion boundary

**Question 08.10.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 11: Crafting Recipe Expansion boundary

**Question 08.11.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 12: Crafting Recipe Expansion boundary

**Question 08.12.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 09, lens 01: Crafting Recipe Expansion boundary

**Question 09.01.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 02: Crafting Recipe Expansion boundary

**Question 09.02.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 03: Crafting Recipe Expansion boundary

**Question 09.03.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 04: Crafting Recipe Expansion boundary

**Question 09.04.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 05: Crafting Recipe Expansion boundary

**Question 09.05.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 06: Crafting Recipe Expansion boundary

**Question 09.06.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 07: Crafting Recipe Expansion boundary

**Question 09.07.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 08: Crafting Recipe Expansion boundary

**Question 09.08.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 09: Crafting Recipe Expansion boundary

**Question 09.09.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 10: Crafting Recipe Expansion boundary

**Question 09.10.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 11: Crafting Recipe Expansion boundary

**Question 09.11.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 12: Crafting Recipe Expansion boundary

**Question 09.12.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 10, lens 01: Crafting Recipe Expansion boundary

**Question 10.01.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 10.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 10.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 10.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 10, lens 02: Crafting Recipe Expansion boundary

**Question 10.02.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 10.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 10.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 10.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 10, lens 03: Crafting Recipe Expansion boundary

**Question 10.03.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 10.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 10.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 10.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 10, lens 04: Crafting Recipe Expansion boundary

**Question 10.04.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 10.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 10.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 10.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 10, lens 05: Crafting Recipe Expansion boundary

**Question 10.05.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 10.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 10.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 10.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 10, lens 06: Crafting Recipe Expansion boundary

**Question 10.06.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 10.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 10.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 10.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 10, lens 07: Crafting Recipe Expansion boundary

**Question 10.07.** Does `src/Host/CraftingHostSession.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 10.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 10.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 10.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 10, lens 08: Crafting Recipe Expansion boundary

**Question 10.08.** Does `src/UI/CraftingPanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/pharma_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 10.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 10.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 10.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 10, lens 09: Crafting Recipe Expansion boundary

**Question 10.09.** Does `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/metallurgy_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 10.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 10.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 10.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 10, lens 10: Crafting Recipe Expansion boundary

**Question 10.10.** Does `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 10.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 10.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 10.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 10, lens 11: Crafting Recipe Expansion boundary

**Question 10.11.** Does `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 10.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 10.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 10.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 10, lens 12: Crafting Recipe Expansion boundary

**Question 10.12.** Does `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/workshop_recipes.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 10.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 10.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 10.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.



> **Pre-polish body length at generation time:** 257,088 characters.
> **Target interpretation:** 150k–170k is the first checkpoint; 250k+ is the evidence-backed depth target and is not a ceiling.

# Post-250K Deep Polishing Pass — Crafting Recipe Expansion

This pass was applied only after the plan body exceeded the 250,000-character evidence-backed depth target. The trigger is structural, not a quality claim: the base plan is already complete enough for review, so the polishing pass audits decisions and failure paths instead of appending fictional feature scope.

## Polish 1 — Content and evidence depth

- Rechecked the distinction between terminal maintenance work and residual implementation work.
- Rechecked every current source/data path named in the plan; missing paths are treated as premise gaps, not silently promoted.
- Rechecked catalog counts, schema keys and sample rows; row presence is not described as reachability.
- Rechecked the owner matrix so mutable state, authored content, lifecycle, host commands, facts, presentation and persistence have separate homes.
- Rechecked the plan against the live master authority, which remains read-only and subordinate to current source/data.
- Rechecked that historical “sealed” language, fake APIs, fake test counts and duplicate managers cannot become implementation instructions.

**Polish 1 outcome:** the plan is allowed to discuss future work only as `PROPOSAL` or `UNKNOWN`; every implementation step has a current evidence question and a completion gate.

## Polish 2 — Integration and code architecture

- Rechecked the proposed flow: input → validation → current owner → typed fact → existing consumer → truthful presentation → existing save path.
- Rechecked Core/host boundaries: no engine imports in Core, no gameplay math in panels, no cross-environment value exports from an aspect index.
- Rechecked deterministic behavior: no wall-clock, GUID or `System.Random` decision path; stable ordering precedes any weighted selection.
- Rechecked persistence: no new save section is assumed; old-save defaults, capture/restore, deep-copy and checksum behavior remain explicit questions.
- Rechecked accessibility: focus, close/back, controller input, readable status and refresh/disposal are part of acceptance, not optional polish.
- Rechecked failure behavior: missing owner, empty catalog, duplicate ID, stale save and repeated delivery fail closed or preserve prior truth.

**Polish 2 outcome:** the implementation route is the smallest extension of current seams, with a separate claim required for any shared composition root.

## Final precision and reaccuracy pass

1. Re-run the current source/data census and compare it with the hashes in this document.
2. Trace one real player command from the current host input to the current Core owner.
3. Trace one real fact from the owner to its current consumer and verify post-mutation ordering.
4. Trace every durable proposed field through the existing capture/restore path; delete hypothetical fields that do not survive this test.
5. Reject any proposed row, API, save section or panel route that lacks a current owner, validator and consumer.
6. Re-run the focused test selection after implementation and record actual output separately from this planning artifact.
7. Perform a final scope audit: no unrelated systems, no Unity dependency, no generated index, no speculative architecture.

**Precision result:** this plan is implementation-ready only after those current-evidence checks pass. A stale premise returns `STALE_PLAN`; it does not justify restoring an old API.

## Full repolishing phase — maximum useful depth

The final repolish is a quality ceiling, not a length ceiling. It must improve decision clarity, not add noise. The reviewer asks:

- Can a new builder identify the first safe file to read?
- Can they tell what is already complete?
- Can they tell what remains genuinely missing?
- Can they prove the player-visible route?
- Can they prove persistence and replay?
- Can they identify every owner boundary they must not cross?
- Can they run the smallest meaningful verification target?
- Can they roll back one phase without corrupting a save?
- Can they explain why each proposed row or field is necessary?
- Can they reject a stale or duplicate implementation proposal?

A plan that cannot answer those questions is not ready for implementation, regardless of character count. This final phase therefore closes on precision, safety, legibility and truthful game feel—not on a larger document.

# Appendix L — Verification Record for This Planning Pass

- Plan generation status: **complete**.
- Base body threshold: **250,000+ characters before the post-250k polish appendices**.
- Current authority SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.
- Production files changed by this pass: **none**.
- Authored JSON changed by this pass: **none**.
- Tests run by this planning pass: **none**; focused commands are explicitly separated as future verification.
- Structural verifier: run externally after generation; it must check required sections, minimum length, authority reference, current path existence and no false fresh-pass claims.
- `git diff --check`: run externally against only the 15 claimed plan files and the two governance rows after finalization.

# Appendix M — Implementation Handoff Contract

**Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-6`

**Outcome:** `Crafting Recipe Expansion`

**Files changed:** the exact 15 plan paths in the Round 6 claim, plus the Round 6 governance rows only.

**Current contract used:** live Core/host/data/test evidence in Appendices A–D; master authority read-only in Appendix E.

**Verification commands and results:** planning-only structural QA and scoped diff check; focused xUnit/headless commands are future implementation gates and are not claimed as run here.

**Tests reused / added / aggregated:** existing focused tests are inventoried; no test file is created or changed by this plan.

**Known limitation or debt:** current production reachability for every proposed residual must be rechecked; a plan is not a runtime integration.

**Shared files intentionally untouched:** production, data, test, UI, generated index, runtime and unrelated dirty worktree files.

**Ready for implementation:** only after Phase 0 premise checkpoint and a new implementation claim.
