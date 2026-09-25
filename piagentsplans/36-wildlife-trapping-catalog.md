# Plan 36 — Wildlife Trapping Catalog, Catch Resolution and Durable Harvest Loop

> **Rebuild status:** COMPLETE TRAP/PREY/BAIT LOOP — PERSISTENCE AND REACHABILITY MAINTENANCE
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-2`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round2-2026-09-25`
>
> **Current-evidence date:** 2026-09-25
>
> **Authority order:** live source and data → `AGENTS.md` → `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` → plan ledgers → this document.
>
> **Authority checksum:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
>
> **Length policy:** the 150k–170k band is a completeness checkpoint, never a reason to add filler. This plan is allowed to trim below the band if the verified architecture is exhausted.

## 0. Integrity Statement and Plan Status

This file replaces unclaimed generated sections that mixed current evidence, fictional APIs, and unsupported save claims. It is a planning and architecture artifact only. It authorizes no production, data, test, save, generated-index, or UI edits. Every path labeled current must exist at rebuild time. Any future `CREATE` proposal is explicitly hypothetical and belongs to a later, separately claimed implementation package.

The rebuild follows four passes: content/current-reality first; integration framework second; accuracy and contradiction removal third; independent precision and handoff review fourth. Character count is recorded by external verification, not embedded recursively in the document.

# 1. Objective

- Wildlife trapping is already a full loop: select a canonical trap, consume current inventory through the host bill, set a site, wait for weather/migration/skill context, check the trap, resolve primary/bycatch outcomes, butcher/transfer inventory, repair and persist.
- The catalog is the authority for trap/prey/bait definitions; `WildlifeTrappingSystem` owns site/catch/durability state; `WildlifeTrappingHostSession` translates inventory and disease/conamination effects; the canonical disease and dose owners receive effects.
- The quality target is deterministic fairness and truthful risk presentation: show why a catch happened, whether contamination/disease was applied, and what a repair costs without duplicating state in the panel.

**Bounded outcome:** Retire the old “no data/zero traps” premise. The current catalog has 10 traps, 15 prey and 6 baits, with a registered runtime, weather/migration modifiers, disease/contamination routes, host repair flow, panel and extensive save/replay tests. The correct plan is a conservation and integration audit, not a new trap catalog.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `wildlife_trapping_catalog.json` is present with 10 traps, 15 prey and 6 baits; the loader registers definitions into the current Core system.
- The host session implements atomic setup/repair bills, disease/contamination delegates, journal/codex first-catch facts and current weather/density context.
- Focused tests cover catalog integrity, migration/season eligibility, weather, skill, bycatch, disease mapping, durability, persistence, legacy fixtures and deterministic replay.
- `WildlifeTrappingPanel` is the current UI surface and `src/Main.ShelterSocial.cs` captures the existing `wildlife_trapping` save section.

**Master-authority sections applied to this rebase:**

- Master authority Volume 28 verification cookbook: focused evidence before broad gates.
- Lane D save/state/compatibility guidance: owner DTOs, migration and restore proof.
- Lane E UI/UX/accessibility guidance: truthful projections and keyboard/controller lifecycle.
- Lane G testing guidance: smallest affected target, negative cases and deterministic replay.
- Anti-padding protocol: content exhaustion may end the plan before the character checkpoint.
- Volume 32 ecology and authored-data guidance.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 10/15 “create” brief with a current 10/15/6 census and consumer matrix.
- Document the exact owner boundary among catalog, trap system, inventory transaction, disease, dose, journal and codex.
- Preserve deterministic replay and save/legacy normalization as release gates.
- Audit UI disclosure of trap durability, catch risk, contamination and repair affordability.

Anything beyond this list is a different package. In particular, this plan does not convert a documentation gap into permission to create a second domain owner.

# 4. Current Evidence and Premise Audit

The current evidence set for this plan is enumerated in the appendices with file hashes, declaration digests, catalog schema/counts and focused test inventories. A source declaration proves an API surface exists; it does not prove a fresh test run or live player reachability. Those claims require the verification steps in this document.

**Evidence classes used here:**

- **VERIFIED CURRENT:** the named path exists and its contents were read during this rebuild.
- **HISTORICAL RECORD:** an archived closeout or old plan says a package once landed; it is useful context but is not current pass evidence.
- **PROPOSAL:** a future seam or file shape that requires a new claim and premise recheck.
- **UNKNOWN:** deliberately unresolved because the present plan does not need to invent an answer.

# 5. Existing Extension Seams

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| trap/prey/bait definitions | WildlifeTrappingCatalog | `Assets/Ashfall.Core/WildlifeTrappingCatalog.cs` | Sole catalog loader and registration. |
| sites, catches, bycatch and durability | WildlifeTrappingSystem | `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` | Owns trap simulation state. |
| inventory bills and effect routing | WildlifeTrappingHostSession | `src/Host/WildlifeTrappingHostSession.cs` | Adapts canonical inventory/disease/dose owners. |
| health and contamination consequences | Wildlife/disease/dose owners | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs; src/Host/DoseLedgerHostSession.cs` | Receive routed effects; do not duplicate risk state. |
| catalog, replay, persistence and disease proof | Wildlife trapping tests | `Ashfall.Core.Tests/WildlifeTrappingCatalogTests.cs; Ashfall.Core.Tests/WildlifeTrappingIntegrationTests.cs; Ashfall.Core.Tests/WildlifeTrappingReplayTests.cs` | Executable current evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Wildlife Trapping Catalog, Catch Resolution and Durable Harvest Loop
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ WildlifeTrappingCatalog
│   trap/prey/bait definitions
│ WildlifeTrappingSystem
│   sites, catches, bycatch and durability
│ WildlifeTrappingHostSession
│   inventory bills and effect routing
│ Wildlife/disease/dose owners
│   health and contamination consequences
│ Wildlife trapping tests
│   catalog, replay, persistence and disease proof
                │
                ▼
Host projection → existing command → owner mutation → typed fact
                │
                ├─ UI / briefing / journal / audio presentation
                ├─ existing save envelope and checksum
                └─ focused Core / host / headless verification
```

The architecture is deliberately projection-first where a read model is sufficient, owner-extension-first where new mutable facts are required, and data-first only when an existing catalog can express the content. It does not permit a new subsystem merely to make the plan look larger.

## 6.1 Architectural decisions

1. **Preserve current state ownership.** WildlifeTrappingCatalog owns trap/prey/bait definitions: Sole catalog loader and registration.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| trap/prey/bait definitions | WildlifeTrappingCatalog | `Assets/Ashfall.Core/WildlifeTrappingCatalog.cs` | Sole catalog loader and registration. |
| sites, catches, bycatch and durability | WildlifeTrappingSystem | `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` | Owns trap simulation state. |
| inventory bills and effect routing | WildlifeTrappingHostSession | `src/Host/WildlifeTrappingHostSession.cs` | Adapts canonical inventory/disease/dose owners. |
| health and contamination consequences | Wildlife/disease/dose owners | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs; src/Host/DoseLedgerHostSession.cs` | Receive routed effects; do not duplicate risk state. |
| catalog, replay, persistence and disease proof | Wildlife trapping tests | `Ashfall.Core.Tests/WildlifeTrappingCatalogTests.cs; Ashfall.Core.Tests/WildlifeTrappingIntegrationTests.cs; Ashfall.Core.Tests/WildlifeTrappingReplayTests.cs` | Executable current evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load/validate catalog
2. preview site eligibility and inventory bill
3. atomically consume setup materials
4. advance trap by day with weather/migration/skill context
5. resolve primary and bycatch outcomes through seeded RNG
6. route disease/contamination through canonical owners
7. transfer/butcher yield through inventory transaction
8. capture/restore and continue deterministic checks

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Trap sites, catch carcasses, durability and first-catch journal facts are system state.
- Disease/contamination outcomes are routed to their canonical owners and are not duplicated in the trap catalog.
- A broken trap remains visibly broken until repaired; repair consumes a current inventory bill atomically.
- Legacy trap state normalizes to functional defaults without silently changing current outcomes.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Every trap/prey/bait reference resolves to the current item, migration and season contracts.
- A failed setup/repair changes neither inventory nor trap state.
- Same seed, day, weather, migration and site state produce the same catch trace.
- A contamination/disease effect is applied once and is visible to the player-facing log/UI.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `wildlife_trapping_catalog.json` is the sole trap/prey/bait authority.
- No new catch table or parallel migration catalog is introduced.
- New rows require item IDs, migration/season semantics, bounded risk and a current consumer.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the current `wildlife_trapping` section and `WildlifeTrappingSaveStore`.
- No new save section is justified.
- Legacy fields normalize to functional defaults and current capture must round-trip every site/carcass/durability field.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Catch, bycatch, disease and contamination rolls use the injected seeded RNG.
- Site iteration order is stable.
- Paired uninterrupted and save/restore campaigns produce identical event/hash traces.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Trap set/check/repair and catch events are emitted by the current system.
- Disease/contamination delegates are host-routed effects, not trap-local gameplay authority.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/WildlifeTrappingHostSession.cs
- src/Host/WildlifeTrappingSaveStore.cs
- src/Main.ShelterSocial.cs
- src/UI/WildlifeTrappingPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Prey, weather and disease descriptions remain fictional and restrained.
- The loop should communicate ecological uncertainty and risk without romanticizing contamination.
- Journal/codex first-catch facts are the existing narrative seam.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A trap consumes materials after a failed eligibility check. | WildlifeTrappingCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | Disease or contamination is applied twice. | WildlifeTrappingSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A migration/season mismatch silently makes prey unavailable. | WildlifeTrappingHostSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A save restore changes trap durability or first-catch dedupe. | Wildlife/disease/dose owners | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | UI shows a catch without the canonical consequence log. | Wildlife trapping tests | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingReplayTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingPersistenceTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingDiseaseMappingTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — census | Read catalog, loader, host, panel and focused tests. | 10/15/6 current authority is proven. | No production path until the owning implementation package is separately claimed. |
| 1 — owner/risk matrix | Trace setup, catch, bycatch, disease, dose, inventory and journal. | One owner per concern. | No production path until the owning implementation package is separately claimed. |
| 2 — replay/persistence proof | Run continuous, interrupted and legacy fixtures. | Traces and state hashes match. | No production path until the owning implementation package is separately claimed. |
| 3 — UI/content seal | Audit risk disclosure and tone. | Player can explain outcome and cost. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/wildlife_trapping_catalog.json | READ ONLY; MODIFY only for proven row gap | 10/15/6 authority |
| Assets/Ashfall.Core/WildlifeTrappingSystem.cs | READ ONLY | Runtime owner |
| src/Host/WildlifeTrappingHostSession.cs | READ ONLY | Effect/inventory adapter |
| src/UI/WildlifeTrappingPanel.cs | READ ONLY | Current UI |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Adding a second trap state store. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Applying disease/contamination outside canonical owners. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Using unseeded or wall-clock randomness. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Copying risk numbers into the panel. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new trap/prey/bait count for this rebase.
- No new disease or dose model.
- No save-section change.
- No production edits in this rebase.

# 23. Rollback and Recovery

- Revert the plan file.
- Future catalog/host changes retain prior fixtures and focused replay/persistence tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 10 traps, 15 prey and 6 baits are current.
- Risk and consequence owner boundaries are explicit.
- Determinism, legacy and UI disclosure contracts are named.
- No parallel authority is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 10/15 “create” brief with a current 10/15/6 census and consumer matrix.
- Document the exact owner boundary among catalog, trap system, inventory transaction, disease, dose, journal and codex.
- Preserve deterministic replay and save/legacy normalization as release gates.
- Audit UI disclosure of trap durability, catch risk, contamination and repair affordability.

## MUST NOT DO

- No new trap/prey/bait count for this rebase.
- No new disease or dose model.
- No save-section change.
- No production edits in this rebase.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingReplayTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingPersistenceTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingDiseaseMappingTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: trap/prey/bait definitions → WildlifeTrappingCatalog; sites, catches, bycatch and durability → WildlifeTrappingSystem; inventory bills and effect routing → WildlifeTrappingHostSession; health and contamination consequences → Wildlife/disease/dose owners; catalog, replay, persistence and disease proof → Wildlife trapping tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 36.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 36 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by WildlifeTrappingCatalog or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/WildlifeTrappingCatalog.cs`

### `Assets/Ashfall.Core/WildlifeTrappingCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 388 lines / 16243 bytes.
- SHA-256: `a0934ce15a436f9b891633cf235112c96f0a716601ae22d4174e0096ee0dc63c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TrapDefinition
public string trap_id = string.Empty;
public string displayName = string.Empty;
public string description = string.Empty;
public string trapType = "snare";
public List<TrapSetupCost> setupCosts = new List<TrapSetupCost>();
public int checkIntervalDays = 2;
public int durabilityChecks = 8;
public float baseCatchModifier = 1.0f;
public List<string> compatiblePrey = new List<string>();
public bool requiresWater = false;
public float weatherSensitivity = 0.0f;
public float networkPenaltyPerTrap = 0f;
public float bycatchChance = 0f; // Plan 36 III: probability of bycatch on successful catch
public List<BycatchCandidate> bycatchSpecies = new List<BycatchCandidate>(); // Plan 36 III: weighted bycatch pool
public float narrativeIncidentChance = 0f;
public List<string> narrativeIncidentIds = new List<string>();
public float trapEncounterChance = 0f;
public InventoryBill CalculateRepairBill() {
public InventoryBill CalculateSetupBill() {
public bool Validate(out string error) {
public sealed class TrapSetupCost
public string itemId = string.Empty;
public int amount = 1;
public sealed class BycatchCandidate
public string speciesId = string.Empty;
public float weight = 1.0f;
public sealed class PreyDefinition
public string speciesId = string.Empty;
public string displayName = string.Empty;
public string description = string.Empty;
public float baseYieldKg = 1.0f;
public float toxicChance = 0.2f;
public float hideYield = 0.0f;
public string hideItemId = string.Empty;
public string preferredTrapType = "snare";
public List<string> attractedByBaitIds = new List<string>();
public float minSkillLevel = 0.0f;
public string migrationSpeciesId = string.Empty;
public List<string> activeSeasons = new List<string>();
public float diseaseRisk = 0.1f;
public float contaminationRisk = 0.05f;
public string diseaseId = string.Empty; // Plan 36 Closure II: per-species disease mapping
public float contaminationDose = 0f; // Plan 36 Closure II: explicit contamination dose in rads
public float moraleEffect = 0f;
public float moralWeight = 0f;
public bool isRareSpecies = false;
public const string FallbackDiseaseId = "disease_zoonotic_flu";
public const float FallbackContaminationDose = 2.0f;
public bool Validate(out string error) {
public string ResolveDiseaseId() => ResolveDiseaseId(this);
public static string ResolveDiseaseId(PreyDefinition? prey) {
public static string ResolveDiseaseId(float diseaseRisk, string? explicitDiseaseId = null) {
internal sealed class WildlifeTrappingCatalogFileRaw
public int schema_version = 1;
public List<TrapDefinition> traps = new List<TrapDefinition>();
public List<PreyDefinition> prey = new List<PreyDefinition>();
public List<BaitProfile> baits = new List<BaitProfile>();
public static class WildlifeTrappingCatalogLoader
public const string FileName = "wildlife_trapping_catalog.json";
public static WildlifeTrappingCatalog? Load( string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null) {
public sealed class WildlifeTrappingCatalog
public IReadOnlyDictionary<string, TrapDefinition> Traps => _traps;
public IReadOnlyDictionary<string, PreyDefinition> Prey => _prey;
public IReadOnlyDictionary<string, BaitProfile> Baits => _baits;
public void RegisterWith(WildlifeTrappingSystem system) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/WildlifeTrappingSystem.cs`

### `Assets/Ashfall.Core/WildlifeTrappingSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1620 lines / 78902 bytes.
- SHA-256: `bc94947453b31bc97de2d069c1a56487a684e7679a736acec13c7f34e30d1c59`.
- Architecture signals: seeded references=13; save/restore symbols=2; typed event declarations=33; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WildlifeTrappingState
public string systemId = WildlifeTrappingSystem.SystemId;
public List<TrapSite> trapSites = new List<TrapSite>();
public int totalCatch;
public int totalToxicRemoved;
public List<string> firstCatchLoggedSpeciesIds = new List<string>();
public List<WildlifeTrappingPendingEvent> pendingEvents = new List<WildlifeTrappingPendingEvent>();
public int eventSequence;
public int nextDeploymentSequence;
public int rngSeed;
public ulong primaryRngState;
public int encounterRngSeed;
public ulong encounterRngState;
public int incidentRngSeed;
public ulong incidentRngState;
public sealed class TrapSite
public string siteId = string.Empty;
public string assignedHunterId = string.Empty;
public string baitType = string.Empty;
public string trapType = "snare"; // snare, deadfall, cage, pit
public string trapId = string.Empty; // Plan 36: catalog link
public int setDay = -1;
public int checkDay = -1;
public int checkIntervalDays = 2;
public int remainingDurability = -1; // -1 = legacy/untracked, >0 = operational, 0 = broken
public bool isBroken; // Plan 36: trap cannot produce catches when true
public bool hasCatch;
public string catchSpecies = string.Empty;
public string bycatchSpecies = string.Empty; // Plan 36 III: bycatch species if occurred
public float bycatchYield; // Plan VI: independently resolved secondary carcass yield
public bool bycatchToxic; // Plan VI: independently resolved secondary toxicity
public float carcassYield;
public bool isToxic;
public bool toxinRemoved;
public bool isMeatProcessed;
public bool hidePreserved;
public string diseaseId = string.Empty; // Tasks 5-8: resolved disease ID from catch
public float contaminationDose; // Tasks 5-8: resolved contamination dose in rads
public string bycatchDiseaseId = string.Empty; // Plan VI: resolved secondary disease ID
public float bycatchContaminationDose; // Plan VI: resolved secondary contamination dose
public string pendingNarrativeEvent = string.Empty;
public int deploymentSequence;
public sealed class BaitProfile
public string baitId = string.Empty;
public string displayName = string.Empty;
public float catchBonusMultiplier = 1.0f; // multiplies base catch chance
public float toxicReduction = 0.0f; // reduces toxic chance by this fraction
public List<string> preferredSpecies = new List<string>();
public int craftCostScrapMeat = 0;
public int craftCostRoots = 0;
public int craftCostChemicals = 0;
public sealed class QuarrySpecies
public string speciesId = string.Empty;
public string displayName = string.Empty;
public float baseYieldKg = 1.0f;
public float toxicChance = 0.2f;
public float hideYield = 0.0f;
public string hideItemId = string.Empty;
public string preferredTrapType = "snare";
public List<string> attractedByBaitIds = new List<string>();
public float minSkillLevel = 0.0f;
public sealed class WildlifeSelectionContext
public static readonly WildlifeSelectionContext Default = new WildlifeSelectionContext();
public string SeasonWindowId { get; set; } = string.Empty;
public WeatherKind CurrentWeather { get; set; } = WeatherKind.Clear;
public HashSet<string> PresentMigrationSpecies { get; set; } = new HashSet<string>(StringComparer.Ordinal);
public Dictionary<string, float> AbundanceFactors { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);
public Dictionary<string, float> HunterSkillLevels { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);
public sealed class WildlifeTrappingSystem
public const string SystemId = "wildlife_trapping";
public WildlifeTrappingState State => _state;
public event Action OnTrappingChanged;
public event Action<string, string, string, bool> OnButcheryCompleted; // siteId, butcherId, species, isToxic
public event Action<ButcheryCompletedEvent>? OnButcheryCompletedDetailed;
public event Action<string, string> OnHidePreserved; // siteId, hideItemId
public event Action<string, string>? OnTrophyReady;
public event Action<string, string, string>? OnNewSpeciesDiscovered;
public event Action<string, string, string, string, int, string>? OnBycatchOccurred;
public event Action<BycatchOccurredEvent>? OnBycatchResolved;
public event Action<TrapLifecycleEvent>? OnTrapDeployed;
public event Action<TrapLifecycleEvent>? OnTrapBroken;
public event Action<TrapLifecycleEvent>? OnTrapRepaired;
public event Action<TrapLifecycleEvent>? OnTrapRemoved;
public event Action<WildlifeTrappingPendingEvent>? OnPendingEventCreated;
public static int DeriveEncounterStreamSeed(int parentSeed) {
public static int DeriveIncidentStreamSeed(int parentSeed) {
public void RegisterBait(BaitProfile bait) {
public void RegisterQuarry(QuarrySpecies species) {
public void RegisterPreyDefinition(PreyDefinition prey) {
public void RegisterTrapDefinition(TrapDefinition trap) {
public void SetHunterSkill(float skillLevel) {
public void SetSelectionContext(WildlifeSelectionContext context) {
public static float SkillMultiplierFor(float skillLevel) {
public static float WeatherPenaltyFor(WeatherKind kind) {
public static float CalculateWeatherMultiplier(float weatherSensitivity, WeatherKind weather) {
public Func<WeatherKind, float>? WeatherPenaltyProvider { get; set; }
public float EffectiveWeatherPenalty(WeatherKind weather) => WeatherPenaltyProvider != null
public static float CalculatePrimaryCatchChance( float densityMultiplier, float hunterSkillLevel, float baitMultiplier, float weatherSensitivity, WeatherKind weather,
public List<WildlifeTrappingPendingEvent> GetPendingEvents() {
public bool MarkEventDelivered(string eventId) {
public int CountPendingEvents(string kind) {
public bool CanSetTrapAtSite(string siteId, out string failureCode) {
public ActionResult SetTrap(string siteId, string baitType, string hunterId, string trapType = "snare", string trapId = "", int checkIntervalDays = -1, int durabilityChecks = -1) {
public const float BaseCatchChance = 0.5f;
public List<string> GetEligibleQuarryIds(string baitType, string trapType, float hunterSkillLevel, string trapId = "") {
public ActionResult CheckTraps(float densityMultiplier = 1f) {
public ActionResult Butcher(string siteId, string butcherId = "") {
public ActionResult PreserveHide(string siteId, out string hideItemId, out float hideQuantity) {
public ActionResult TransferCatchToInventory(string siteId, Inventory.Inventory inventory, string fallbackRawMeatId = "raw_meat") {
public string GetTrophyRecipeForSpecies(string speciesId) {
public IReadOnlyDictionary<string, BaitProfile> GetBaitCatalog() => _baitCatalog;
public IReadOnlyDictionary<string, QuarrySpecies> GetQuarryCatalog() => _quarryCatalog;
public IReadOnlyDictionary<string, TrapDefinition> GetTrapDefinitionCatalog() => _trapDefinitionCatalog;
public IReadOnlyDictionary<string, PreyDefinition> GetPreyDefinitionCatalog() => _preyDefinitionCatalog;
public bool RollDiseaseRisk(float diseaseRisk) {
public bool RollContaminationRisk(float contaminationRisk) {
public ActionResult RepairTrap(string siteId, int restoreDurability) {
public ActionResult RemoveTrap(string siteId) {
public ActionResult RemoveToxin(string siteId) {
public void TickDay(int day, float densityMultiplier = 1f) {
public WildlifeTrappingState CaptureState() {
public void RestoreState(WildlifeTrappingState saved) {
public static string BuildNarrativeIncidentSourceId(string siteId, string eventId) => $"wildlife-trap:{siteId ?? string.Empty}:incident:{eventId ?? string.Empty}";
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Disease/DiseaseSystem.cs`

### `Assets/Ashfall.Core/Disease/DiseaseSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1706 lines / 79732 bytes.
- SHA-256: `63336084c564afa0084a69c1bee665d9978f012387d6c26bdb58d0d7901bc8e6`.
- Architecture signals: seeded references=8; save/restore symbols=3; typed event declarations=14; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=1; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DiseaseIds
public const string ExpansionId = "expansion_disease_expansion";
public const string CatalogCollectionId = DiseaseCatalog.CollectionId;
public const string Cholera = "disease_cholera";
public const string ZoonoticFlu = "disease_zoonotic_flu";
public const string BloodFever = "disease_blood_fever";
public const string SporeBlight = "disease_spore_blight";
public const string TyphoidWaterborne = "disease_typhoid_waterborne";
public const string Dysentery = "disease_dysentery";
public const string EventInfection = "disease_infection";
public const string EventQuarantineStarted = "disease_quarantine_started";
public const string EventQuarantineEnded = "disease_quarantine_ended";
public const string EventOutbreakDeclared = "disease_outbreak_declared";
public const string EventOutbreakContained = "disease_outbreak_contained";
public const string EventRecovered = "disease_recovered";
public const string EventDied = "disease_death";
public const string EventProtocolApplied = "disease_protocol_applied";
public const string EventProtocolReset = "disease_protocol_reset";
public const string EventTreatmentApplied = "disease_treatment_applied";
public sealed class DiseaseInfectionState
public string survivor_id = string.Empty;
public int infected_day = 0;
public int days_sick = 0;
public bool quarantined = false;
public string current_stage = DiseaseStageNames.Incubating;
public int stage_entered_day = 0;
public int treatments_applied = 0;
public float lethality_reduction = 0f;
public int last_treatment_day = -1;
public bool is_diagnosed = false;
public sealed class DiseaseImmunityRecord
public string survivor_id = string.Empty;
public string disease_id = string.Empty;
public int immunity_until_day = 0;
public float strength = 1.0f;
public sealed class DiseaseExposureContext
public string SurvivorId { get; set; } = string.Empty;
public string DiseaseId { get; set; } = string.Empty;
public string SourceId { get; set; } = string.Empty;
public float ProbabilityModifier { get; set; } = 1.0f;
public bool BypassImmunity { get; set; } = false;
public int Day { get; set; } = 0;
public sealed class DiseaseExposureResult
public bool Infected { get; set; }
public string Reason { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string DiseaseId { get; set; } = string.Empty;
public float EffectiveProbability { get; set; }
public static DiseaseExposureResult CreateInfected(string survivorId, string diseaseId, float prob) =>
public static DiseaseExposureResult CreateBlocked(string reason, string survivorId, string diseaseId, float prob = 0f) =>
public sealed class DiseaseTreatmentResult
public bool Accepted { get; set; }
public string Reason { get; set; } = string.Empty;
public string Role { get; set; } = string.Empty;
public string ItemId { get; set; } = string.Empty;
public string DiseaseId { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public float LethalityReduction { get; set; }
public bool Cured { get; set; }
public static DiseaseTreatmentResult Refuse(string reason, string itemId, string diseaseId, string survivorId) =>
public static class DiseaseTreatmentRefusals
public const string NotPatient = "not_patient";
public const string UnknownDisease = "unknown_disease";
public const string NoTreatmentAuthorised = "no_treatment_authorised";
public const string ItemNotAuthorised = "item_not_authorised";
public const string OutsideWindow = "outside_window";
public const string AlreadyTreatedToday = "already_treated_today";
public const string NoSupplyChannel = "no_supply_channel";
public const string SupplyUnavailable = "supply_unavailable";
public sealed class DiseaseEntryState
public string disease_id = string.Empty;
public string vector_type = DiseaseVectorNames.Water;
public float spread_timer = 0f;
public bool outbreak_active = false;
public int deaths_during_outbreak = 0;
public int outbreaks_total = 0;
public int outbreaks_prevented = 0;
public int recovered_total = 0;
public int deaths_total = 0;
public int infections_total = 0;
public List<DiseaseInfectionState> infected = new List<DiseaseInfectionState>();
public sealed class DiseaseSystemState
public const int CurrentVersion = 2;
public int stateVersion = CurrentVersion;
public string system_id = DiseaseIds.ExpansionId;
public bool water_purified = false;
public bool vents_sealed = false;
public bool tools_sterilized = false;
public bool air_filtration = false;
public int water_purified_until_day = 0;
public int vents_sealed_until_day = 0;
public int tools_sterilized_until_day = 0;
public int air_filtration_until_day = 0;
public int rngSeed = 0;
public long rngPosition = 0;
public List<DiseaseEntryState> diseases = new List<DiseaseEntryState>();
public List<DiseaseImmunityRecord> immunities = new List<DiseaseImmunityRecord>();
public sealed class DiseasePatientSnapshot
public string survivor_id = string.Empty;
public string disease_id = string.Empty;
public string disease_name = string.Empty;
public int days_sick = 0;
public bool quarantined = false;
public bool contagious = false;          // past incubation, not isolated
public int contagion_risk_percent = 0;   // infectivity * 100
public int treatments_applied = 0;
public float effective_lethality = 0f;
public string current_stage = DiseaseStageNames.Incubating;
public string stage_token = "incubating";
public sealed class DiseaseSnapshot
public int total_infected = 0;
public int total_quarantined = 0;
public int total_contagious = 0;
public int total_outbreaks = 0;
public int total_outbreaks_prevented = 0;
public int total_recovered = 0;
public int total_deaths = 0;
public List<DiseasePatientSnapshot> patients = new List<DiseasePatientSnapshot>();
public sealed class DiseaseSystem
public const int DefaultSeed = 1013;
public const int OutbreakThreshold = 3;
public const float MaxLethalityReduction = 0.9f;
public event Action<string, string> OnInfection;                    // survivorId, diseaseId
public event Action<string, string> OnQuarantineStarted;            // survivorId, diseaseId
public event Action<string, string> OnQuarantineEnded;              // survivorId, diseaseId
public event Action<string> OnOutbreakDeclared;                     // diseaseId
public event Action<string, bool> OnOutbreakContained;              // diseaseId, prevented
public event Action<string, string, bool> OnOutcomeResolved;        // survivorId, diseaseId, recovered
public event Action<string, string, string, string, int>? OnTreatmentApplied;
public event Action<DiseaseSystemState> OnStateChanged;
public event Action<string, string> OnEventRaised;                  // eventId, detail
public Func<string, string, float>? EffectiveLethalityModifier;
public event Action<string, string>? OnStrainMutated;
public Func<string, float>? GetIsolationQuality;
public Func<float>? OnsetProbabilityMultiplier { get; set; }
public ContainmentCapability Containment { get; set; } = ContainmentCapability.None;
public DiseaseSystemState State => _state;
public DiseaseCatalog Catalog => _catalog;
public string SystemId => _state.system_id;
public bool HasImmunity(string survivorId, string diseaseId, int currentDay) {
public DiseaseImmunityRecord? GetImmunity(string survivorId, string diseaseId) {
public void SetImmunity(string survivorId, string diseaseId, int untilDay, float strength = 1.0f) {
public DiseaseExposureResult TryExpose(DiseaseExposureContext context) {
public DiseaseExposureResult TryInfect(string survivorId, string diseaseId, int day, string? sourceId = null) {
public void BindCatalog(DiseaseCatalog catalog) {
public DiseaseDefinition? GetDefinition(string diseaseId) {
public bool RegisterStrain(DiseaseDefinition strainDefinition) {
public void Infect(string survivorId, string diseaseId, int day) {
public event Action<string, string, string, int>? OnOutbreakTriggered;
public DiseaseOutbreakResult TriggerOutbreak( IDiseaseOutbreakSource source, string diseaseId, int day, IReadOnlyList<string>? candidates = null) {
public void TickDaily(int day, IReadOnlyList<string>? candidates = null) {
public void Quarantine(string survivorId, string diseaseId) {
public void EndQuarantine(string survivorId, string diseaseId) {
public bool MutateInfection(string survivorId, string fromDiseaseId, string toDiseaseId) {
public Func<string, int, bool>? TryConsumeItem;
public DiseaseTreatmentResult TryTreat( string survivorId, string diseaseId, string itemId, int day) {
public float GetEffectiveLethality(string survivorId, string diseaseId) {
public bool IsContagious(string survivorId, string diseaseId) {
public bool IsInfected(string survivorId, string diseaseId) {
public bool IsQuarantined(string survivorId, string diseaseId) {
public bool TryGetInfection(string survivorId, string diseaseId, out int daysSick, out bool quarantined) {
public bool Diagnose(string survivorId, string diseaseId) {
public bool IsDiagnosed(string survivorId, string diseaseId) {
public DiseaseClinicalPicture GetClinicalPicture(string survivorId, string diseaseId) {
public string GetTransmissionVector(string diseaseId) {
public bool IsVectorBlocked(string vectorType) {
public void PurifyWater(int day = 0) {
public void ResetWaterPurification() {
public void SealVents(int day = 0) {
public void ResetVentSeal() {
public void SterilizeTools(int day = 0) {
public void ResetToolSterilization() {
public void SetAirFiltration(bool active, int day = 0) {
public void TickProtocolExpiry(int day) {
public int ProtocolDaysRemaining(string vectorType, int today) {
public DiseaseSnapshot GetSnapshot() {
public DiseaseEntryState? GetDiseaseState(string diseaseId) {
public DiseaseSystemState CaptureState() {
public void RestoreState(DiseaseSystemState saved) {
```


# Appendix B.05 — Current Code Architecture: `src/Host/WildlifeTrappingHostSession.cs`

### `src/Host/WildlifeTrappingHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 643 lines / 28260 bytes.
- SHA-256: `9318ab54790453b20a0f62683207ac150c70862300b1fbbdba9ac6fcd06cf8e6`.
- Architecture signals: seeded references=1; save/restore symbols=1; typed event declarations=6; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WildlifeTrappingHostSession
public WildlifeTrappingSystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public WildlifeTrappingCatalog? Catalog { get; set; }
public InventoryHostSession? Inventory { get; set; }
public Action<string, string, int>? ApplyDisease { get; set; }
public Action<string, float>? ApplyContamination { get; set; }
public Func<string, string, string, bool>? DeliverMoralConsequence { get; set; }
public Func<string, string, int, bool>? DeliverTrapEncounter { get; set; }
public Func<string, bool>? DeliverTrappingBroadcast { get; set; }
public Func<string, string, int, string, bool>? DeliverNarrativeIncident { get; set; }
public event Action<BycatchOccurredEvent>? OnBycatchOccurred;
public Func<int, bool>? DeliverButcheryFood { get; set; }
public Action<string, float, string>? ApplyMorale { get; set; }
public Func<PreyDefinition, string>? DiseaseResolver { get; set; }
public event Action<string>? OnTrapCrafted;
public ActionResult SetTrap(string siteId, string baitType, string hunterId) {
public bool CanSetTrapAtSite(string siteId, out string failureCode) => System.CanSetTrapAtSite(siteId, out failureCode);
public void SetSelectionContext(WildlifeSelectionContext context) => System.SetSelectionContext(context);
public ActionResult TrySetTrap(string siteId, string trapId, string baitType, string hunterId) {
public bool TryGetSetupBill(string trapId, out InventoryBill bill, out string reason) {
public bool CanAffordSetup(string trapId, out InventoryBill bill, out string failureReason) {
public float WildlifeDensityMultiplier { get; set; } = 1f;
public ActionResult CheckTraps(float? densityMultiplier = null) {
public void DeliverPendingEvents() {
public string ComposeBroadcastMessage(WildlifeTrappingPendingEvent ev) {
public const float FallbackContaminationDose = PreyDefinition.FallbackContaminationDose;
public const string FallbackDiseaseId = PreyDefinition.FallbackDiseaseId;
public ActionResult Butcher(string siteId, string butcherId = "") {
public static string ResolveDiseaseId(PreyDefinition prey) => PreyDefinition.ResolveDiseaseId(prey);
public ActionResult RemoveToxin(string siteId) {
public ActionResult PreserveHide(string siteId) {
public bool TryGetRepairBill(string siteId, out InventoryBill bill, out string reason) {
public bool CanAffordRepair(string siteId, out InventoryBill bill, out string failureReason) {
public ActionResult TryRepairTrap(string siteId) {
public ActionResult RemoveTrap(string siteId) {
public void ReconcileMapMarkers() {
public void TickDay(int day) {
public event Action<int>? OnCatchPressure;
public override void Save() {
```


# Appendix B.06 — Current Code Architecture: `src/Host/WildlifeTrappingSaveStore.cs`

### `src/Host/WildlifeTrappingSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 56 lines / 2823 bytes.
- SHA-256: `e7466c5984ace8f5eda69dc4f37361fdef786ec1a425251bc51342ca416b6518`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class WildlifeTrappingSaveStore
public const string FileName = "wildlife_trapping_save.json";
public const string SectionName = "wildlife_trapping";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static bool TrySave(WildlifeTrappingState state) => s_store.TrySave(state);
public static WildlifeTrappingState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(WildlifeTrappingState state) => s_store.CapturePersisted(state);
public static string TryCaptureDirect(WildlifeTrappingState state) => s_store.CaptureBare(state);
public static WildlifeTrappingState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(WildlifeTrappingState state) => s_store.CaptureBare(state);
public static WildlifeTrappingState? TryRestore(string json) => s_store.RestoreBare(json);
```


# Appendix B.07 — Current Code Architecture: `src/Main.ShelterSocial.cs`

### `src/Main.ShelterSocial.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 602 lines / 30015 bytes.
- SHA-256: `efdcc3d73d212c2e6349a7a1bd586b42f082827ea76457826c93321a1fea5415`.
- Architecture signals: seeded references=0; save/restore symbols=17; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public string Id { get; }
public string DisplayName { get; }
public Ashfall.Core.Journal.RiskBiasTrait RiskBias => Ashfall.Core.Journal.RiskBiasTrait.Realist;
```


# Appendix B.08 — Current Code Architecture: `src/UI/WildlifeTrappingPanel.cs`

### `src/UI/WildlifeTrappingPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 524 lines / 21950 bytes.
- SHA-256: `9cacd807a62a562475a738f5344fae0374a26cce1df74fe7c35c191e8dabcb60`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class WildlifeTrappingPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _host != null;
public string? SelectedRepairSiteId => _selectedRepairSiteId;
public Button? RepairButton => _repairBtn;
public OptionButton? RepairSiteDropdown => _repairSiteDropdown;
public Button? SetTrapButton => _setTrapBtn;
public AshfallStatusRail? StatusRail => _statusRail;
public void Bind(WildlifeTrappingHostSession session) {
public void Unbind() {
public override void _Ready() {
public void SelectRepairSite(string siteId) {
public void RefreshView() {
public override void _ExitTree() {
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json`

### `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 23204 bytes / 23194 characters.
- SHA-256: `ad76e65163efc3860f4ef7d6b209479f27489c0f5cb34a717fe4e64bd5d64d00`.
- Root keys: `baits`, `prey`, `schema_version`, `traps`.

Array-path census (minimum, maximum, observed rows):

```text
baits: min=6, max=6, observed_paths=1
baits[].preferredSpecies: min=3, max=3, observed_paths=2
prey: min=15, max=15, observed_paths=1
prey[].activeSeasons: min=0, max=3, observed_paths=2
prey[].attractedByBaitIds: min=2, max=2, observed_paths=2
traps: min=10, max=10, observed_paths=1
traps[].compatiblePrey: min=4, max=5, observed_paths=2
traps[].narrativeIncidentIds: min=3, max=3, observed_paths=2
traps[].setupCosts: min=1, max=1, observed_paths=2
```

Representative record fields:

- `baseCatchModifier`
- `bycatchChance`
- `bycatchSpecies`
- `checkIntervalDays`
- `compatiblePrey`
- `description`
- `displayName`
- `durabilityChecks`
- `narrativeIncidentChance`
- `narrativeIncidentIds`
- `requiresWater`
- `setupCosts`
- `trapEncounterChance`
- `trapType`
- `trap_id`
- `weatherSensitivity`

Representative identifiers (ordered, capped for readability):

```text
trap_snare
trap_deadfall
trap_pit
trap_net
trap_fish
trap_cage
trap_bird_snare
trap_body_grip
trap_box
trap_improvised_wire
```


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/items.json`

### `Assets/StreamingAssets/Data/items.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 390056 bytes / 390056 characters.
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=724, max=724, observed_paths=1
```

Representative record fields:

- `category`
- `contamination`
- `degradeRate`
- `description`
- `disassembleYieldFraction`
- `displayName`
- `display_name`
- `durability`
- `empShielded`
- `equipSlot`
- `healthEffect`
- `hungerRestore`
- `id`
- `isEquipable`
- `moraleEffect`
- `radCleanse`
- `radProtection`
- `repairCosts`
- `repairRecipe`
- `scrapValue`
- `stackMax`
- `tags`
- `thirstRestore`
- `tradeValue`
- `type`
- `value`
- `weight`
- `weight_kg`

Representative identifiers (ordered, capped for readability):

```text
item_decon_chelator_concentrate
item_lead_lined_effluent_filter
item_heavy_neoprene_scrub_brush
item_sealed_waste_bin
item_theodolite_brass_precision
item_surveyor_stadia_rod
item_datum_plate_bronze
item_concrete_mix
item_forged_rotor_shaft
item_magnetic_bearing_coil
item_high_vacuum_pump
item_containment_ring_steel
item_reinforced_concrete_vault
item_seismic_damper_pad
item_vacuum_pump_oil
item_bearing_grease
item_rotor_balancing_kit
item_portable_pid_detector
item_detector_sensor_module
item_hermetic_sample_ampoule
item_hot_dust_drum
item_sludge_cake
item_tailings_drum
dosimeter
geiger_counter
iodine_pills
anti_rad
gas_mask
hazmat_suit
water_filter
air_filter
clean_water
irradiated_water
canned_food
fuel
cloth
scrap_metal
bandage
raw_meat
cooked_meat
dirty_water
morphine
chelation_agent
potassium_iodide
medical_kit
battery
calibration_kit
tweezers
splint
antibiotics
jewelry
diamond
currency
mechanical_parts
electronic_scrap
item_radiosonde
solar_cell
chemicals
handheld_radio
engine
roots
berries
vacuum_tube
spring_mechanism
phonograph_needle
projector_bulb
lubricant_oil
film_reel
antenna_coil
soldering_kit
music_box_comb
spring_key
typewriter_ribbon
machine_oil
camera_lens_cleaner
photographic_film
item_acoustic_decoy
item_ammonium_nitrate_sack
item_amnestic_syrup
item_anchor_notes
item_ash_ghillie
item_bio_plastic
item_black_water_vial
item_co2_scrubber_cartridge
item_epoxy_injector
item_faraday_mesh
item_frostbite_salve
item_fungicide_fogger
item_galvanized_rebar
item_glycol_antifreeze_canister
item_hermetic_hatch_silicone_gasket
item_high_tensile_steel_culvert_brace
item_insulated_snowmobile_battery
item_lead_shielded_sample_cask
item_lead_visor
item_lithium_salts
item_mine_prod
item_mycelium_bricks
item_prussian_blue_chelating_pellets
item_radon_detector_electret
item_rebreather_scrubber
item_ro_membrane
item_scopolamine_root
item_sealed_lead_pig
item_snow_goggles_improvised
item_sound_baffling
item_suitcase_locked
item_surgical_bone_chisel
item_teddy_bear
item_thermal_paste
item_welders_glass
aa_batteries
alcohol_wipes_box_10_of_10
ammo_762x54r_jhp_ap
ammo_357
ammo_12g
ammo_308
ammo_556
ammo_762
antiseptic_1l_of_1l
battery_pack
box_of_nails_10
canned_soup
childrens_books
cigarette_lighter
clean_water_jug
cooking_oil
copper_wire_10m_of_10m
diesel_fuel
dried_rations
faraday_pack
field_surgical_kit
fuel_1l
fuel_cell
growing_manual
iodine_tablets
item_cassette_tape
item_pre_war_photo_album
item_vinyl_collection
mechanical_components
medkit
metal_pipe
military_grade_hatchet
military_mre
military_radio
military_rations
military_supply_crate
music_box_fur_elise
night_vision_scope
plastic_material
scrap_plastic
synthetic_fuel_canister
carbon_black_powder
protective_childs_coat
rubber_hose
scrap_wood
sealed_government_document
seed_packets
spirits
steel_rebar
```


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/WildlifeTrappingCatalogTests.cs`

### `Ashfall.Core.Tests/WildlifeTrappingCatalogTests.cs`

- Current test declarations: Fact=53, Theory=0, InlineData=0.
- File lines: 1085; SHA-256: `282c418468777621d6e9b517bc980962915302802818f11c804a7e0c8f837835`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_Loads
Catalog_Has10Traps
Catalog_Has15Prey
Catalog_Has6Baits
TrapIds_AreUnique
PreyIds_AreUnique
TrapSetupCosts_ResolveToItems
PreyHideItemIds_ResolveToItems
PreyMigrationIds_ResolveToKnownSpecies
PreyActiveSeasons_ResolveToKnownWindows
RegisterWith_PopulatesQuarryCatalog
RegisterWith_PopulatesBaitCatalog
TrapDefinitions_HaveDistinctTrapTypes
TrapDefinitions_HaveCompatiblePrey
PreyDefinitions_HaveValidPreferredTrapType
CatchResolution_WorksWithCatalogPrey
SaveRoundTrip_PreservesState
MissingFile_ReturnsNull
TrapIds_FollowConvention
PreyYieldItems_ResolveToRawMeat
SetTrap_WithCatalogParams_PersistsTrapId
SetTrap_LegacyCall_HasDefaultDurability
CheckTraps_DecrementsDurability
CheckTraps_DecrementsOnNoCatch
CheckTraps_BreaksAtZero
BrokenTrap_ProducesNoCatches
LegacyTrap_NeverBreaks
RepairTrap_RestoresDurability
RepairTrap_BlocksWhenNotBroken
SaveRoundTrip_PreservesDurability
SaveRoundTrip_PreservesBrokenState
ImprovisedWireSnare_BreaksBeforeCageTrap
LegacySave_DeserializesWithDefaults
LegacySave_MixedOldNewTraps
LegacySave_RestoreIntoRuntime
LegacyTrap_NeverBreaksAfterManyChecks
LegacySave_RoundTripPreservesDefaults
Replay_UninterruptedVsRestored_IdenticalOutcome
Replay_ThreeRuns_IdenticalHash
Replay_BreakOccursOnSameDay
EdgeCase_UnknownTrapId_BlocksSafely
EdgeCase_ExactCostBalance_DeploySucceeds
EdgeCase_FinalDurabilityCatch_ResolvesBeforeBreak
EdgeCase_BrokenTrap_NoRNGAdvancement
EdgeCase_RepairAfterSaveLoad
EdgeCase_LegacyTrap_RepairBlocked
CheckTraps_SetsHasCatch_AfterSuccessfulCatch
CheckTraps_RespectsCheckInterval
CheckTraps_DecrementsDurability_EveryCheck
CalculateRepairBill_SnareTrap_ComputesCeilHalf
CalculateRepairBill_CageTrap_ComputesCeilHalfPerItem
CalculateRepairBill_AggregatesDuplicateItemsBeforeHalving
CalculateRepairBill_EmptySetupCosts_YieldsEmptyBill
```


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/WildlifeTrappingIntegrationTests.cs`

### `Ashfall.Core.Tests/WildlifeTrappingIntegrationTests.cs`

- Current test declarations: Fact=52, Theory=0, InlineData=0.
- File lines: 1375; SHA-256: `dbd9b3501f9e0e35166d96a69a821ca6e77baf8bdcc983d4a9329210311ef04b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SetTrap_AndCheck_ResolvesCatch
SaveAndRestore_PreservesTrapSites
WT_WX_001_ZeroSensitivityTrap_IgnoresWeatherPenalty
WT_WX_002_WeatherPenalty_FalloutStormWithSensitivity03_Produces15PercentReduction
WT_WX_003_WeatherPenalty_BlizzardWithSensitivity03_Produces24PercentReduction
WT_WX_004_ClearWeather_ProducesZeroPenalty
WT_WX_005_DeterministicReplay_SameSeedAndWeather_ProducesIdenticalCatch
WT_WX_006_BycatchIsolation_WeatherDoesNotAlterBycatchFormula
WT_WX_007_DurabilityDecrementsOnCheck_RegardlessOfWeather
WT_WX_008_ExhaustiveEnumPolicy_EveryWeatherKindHasExplicitMapping
WT_WX_009_PrimaryCatchChance_ClampsBetween005And095
WT_SK_001_SkillMultiplier_CurveEvaluation
WT_SK_002_SkillMultiplier_ClampsOutOfRangeValues
WT_SK_003_PerSiteHunterSkill_UsesAssignedHunterProgression
WT_SK_004_UnassignedSite_FallsBackToGlobalHunterSkill
WT_SK_005_SkillProgression_GetDisciplineProgress01_NormalizesCorrectly
WT_SK_006_TwoTraps_TwoHunters_EvaluatedIndependently
WT_SK_007_QuarryEligibilityPerHunter_MinSkillLevelGating
WT_SK_008_MidCampaignProgressionUpdate_SeenOnNextCheck
WT_SK_009_BycatchIsolation_SkillDoesNotModifyBycatch
WT_SK_010_DurabilityIsolation_SkillDoesNotAlterDurabilityDecrement
WT_SK_011_SharedAuthorityGuard_TrappingResolvesSharedSkillProgression
WT_JC_001_FirstCatch_FiresOnNewSpeciesDiscovered
WT_JC_002_SecondCatchSameSpecies_DoesNotFireEventAgain
WT_JC_003_DifferentSpecies_SequentialDiscovery_FiresOnceEach
WT_JC_004_FirstCatchLoggedSpeciesIds_RoundTripsThroughSaveRestore
WT_JC_005_LegacySaveWithoutFirstCatch_RestoresAsEmptyList
WT_JC_006_JournalEntry_CreatedOnce_WithValidAuthorAndDedup
WT_JC_007_JournalSystem_UnlockWildlifeCaught_UnlocksCodexKey
WT_JC_008_CodexEntries_ContainsAll15AuthoritativePreySpecies
WT_JC_009_Bycatch_NotCountedAsFirstCatch
WT_CS_001_DataContract_ImprovisedWire_RequiresNoStation
WT_CS_002_DataContract_BoxTrap_RequiresWorkbench
WT_CS_003_DataContract_FishTrap_RequiresWorkbench
WT_CS_004_StationlessRecipe_CraftableWithoutWorkbench
WT_CS_005_BoxTrap_BlockedWithoutWorkbench
WT_CS_006_FishTrap_BlockedWithoutWorkbench
WT_CS_007_BrokenWorkbench_BlocksBoxAndFishTraps
WT_CS_008_OperationalWorkbench_AllowsBoxTrap
WT_CS_009_OperationalWorkbench_AllowsFishTrap
WT_CS_010_ShelterNotBuilt_WorkbenchAbsent
WT_CS_011_ShelterBuilt_WorkbenchSynchronizes
WT_CS_012_StationLosesAvailability_BlocksNewCraft
WT_CS_013_NoUnconditionalProductionSeed_SourceGate
WT_XI_001_DailyWorldRefresh_OccursBeforeTrapCheck
WT_XI_002_FullCheck_UsesWeather_Density_Hunter_And_Bait
WT_XI_003_PostLoadContextRebuild_BeforeCheck
WT_XI_004_DiseaseAndContaminationBridge_Unchanged
WT_XI_005_OverhuntCatchPressure_Unchanged
WT_XI_006_PanelBinding_StillWorks_SourceGate
WildlifeTrapping_EndToEnd_CraftDeployMigrateButcherSaveRestoreBreak_IsDeterministic
WildlifeTrapping_EndToEnd_CatalogIntegrity_Deploy_BaitReach_Durability_SaveRoundTrip
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/WildlifeTrappingReplayTests.cs`

### `Ashfall.Core.Tests/WildlifeTrappingReplayTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 506; SHA-256: `557b71ad022e24993159476705eb6fca6c1317df56400c700987c39e21b6fb0c`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ButcheryReplay_SaveLoadBoundary_Seed42_PreservesDiseaseAndContaminationOutcome
ButcheryReplay_SaveLoadBoundary_Seed123_PreservesDiseaseAndContaminationOutcome
ButcheryReplay_LowRiskPrey_RemainsDeterministic
ButcheryReplay_FinalHealthStateHash_MatchesAfterRestore
DeterministicReplay_MultiDayCampaignTrace_UninterruptedVsSavedRestored_MatchesExactEventTraceAndHash
DeterministicReplay_ThreeConsecutiveRuns_ProduceIdenticalStateAndEventHash
DeterministicReplay_BreakageBoundary_MatchesBreakDayAndPreventsSubsequentCatches
```


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/WildlifeTrappingPersistenceTests.cs`

### `Ashfall.Core.Tests/WildlifeTrappingPersistenceTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 552; SHA-256: `e855f5ca0b876976f340316c20d93b96a296ca5c02c9c911a55fbf3843333aef`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CaptureSerializeDeserialize_PartiallyWornTrap_PreservesNewFields
CaptureSerializeDeserialize_BrokenTrap_PreservesBrokenState
Deserialize_LegacyTrapWithoutDurabilityFields_UsesFunctionalDefaults
RestoreLegacyThenCapture_EmitsCurrentTrapFields
WildlifeTrappingSaveStore_RoundTrip_PreservesDiseaseContaminationAndTrapFields
LegacySaveFixture_PrePlan36_LoadsWithFunctionalDefaultsAndPreservesState
MixedSaveFixture_LoadsAndPreservesAllFourTrapCategories
RestoreState_NullStringFields_NormalizedToEmptyString
RestoreState_NegativeOrInconsistentDurability_NormalizedCorrectly
```


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/WildlifeTrappingDiseaseMappingTests.cs`

### `Ashfall.Core.Tests/WildlifeTrappingDiseaseMappingTests.cs`

- Current test declarations: Fact=5, Theory=1, InlineData=0.
- File lines: 136; SHA-256: `b385419bef39f060727cd74da6f3f9287ac2469ef3f1172540e57dda53db692d`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_ContainsExactly15Prey
AuthoritativeCatalogPrey_ResolvesExpectedDiseaseId
ThresholdMicroTest_ExactBoundary_SeparatesNoneFromZoonoticFlu
ExplicitCatalogDiseaseId_AlwaysWinsOverTierFallback
LowRiskPrey_RabbitCottonHareMirrorCarp_NeverResolveFallbackDisease
FallbackPrey_DeerFoxPheasantAshPikeMuskratHedgehogBoar_ResolveZoonoticFlu
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/Inventory/InventoryTransaction.cs`

### `Assets/Ashfall.Core/Inventory/InventoryTransaction.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 387 lines / 14478 bytes.
- SHA-256: `2efcf72bae930c0c8ddc43036f44cb41ea02a76a4445233c86319df02578ecdb`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public readonly struct InventoryBillItem : IEquatable<InventoryBillItem>
public string ItemId { get; }
public int Amount { get; }
public ItemDefinition? Definition { get; }
public bool Equals(InventoryBillItem other) =>
public override bool Equals(object? obj) => obj is InventoryBillItem other && Equals(other);
public override int GetHashCode() => StableHash.Combine(StableHash.Of(ItemId), Amount);
public override string ToString() => $"{Amount}x {ItemId}";
public sealed class InventoryBill
public IReadOnlyList<InventoryBillItem> Costs => _costs;
public IReadOnlyList<InventoryBillItem> Grants => _grants;
public bool IsEmpty => _costs.Count == 0 && _grants.Count == 0;
public InventoryBill AddCost(string itemId, int amount, ItemDefinition? def = null) {
public InventoryBill AddCost(ItemDefinition def, int amount) {
public InventoryBill AddGrant(string itemId, int amount, ItemDefinition? def = null) {
public InventoryBill AddGrant(ItemDefinition def, int amount) {
public Dictionary<string, int> GetAggregatedCosts() {
public Dictionary<string, int> GetAggregatedGrants() {
public static InventoryBill FromCosts(IReadOnlyDictionary<string, int> costs) {
public static InventoryBill FromCosts(IEnumerable<KeyValuePair<string, int>> costs) {
public static InventoryBill FromCosts(IEnumerable<string> itemIds) {
public static InventoryBill FromCostsAndGrants( IReadOnlyDictionary<string, int>? costs, IReadOnlyDictionary<string, int>? grants) {
public enum InventoryTransactionStatus
public sealed class InventoryTransactionValidationResult
public bool IsValid => Status == InventoryTransactionStatus.Success;
public InventoryTransactionStatus Status { get; }
public string FailureReason { get; }
public string FailedItemId { get; }
public int RequiredAmount { get; }
public int AvailableAmount { get; }
public static InventoryTransactionValidationResult Success() =>
public static InventoryTransactionValidationResult Insufficient(string itemId, int required, int available) =>
public static InventoryTransactionValidationResult CapacityExceeded(int requiredSlots, int availableSlots) =>
public static InventoryTransactionValidationResult WeightExceeded(float currentWeight, float addedWeight, float maxWeight) =>
public static InventoryTransactionValidationResult Invalid(string reason) =>
public static InventoryTransactionValidationResult Cancelled() =>
public static InventoryTransactionValidationResult CallbackError(string message) =>
public override string ToString() => IsValid ? "Valid" : $"{Status}: {FailureReason}";
public sealed class InventoryTransactionQuote
public InventoryBill Bill { get; }
public IReadOnlyDictionary<string, int> AggregatedCosts { get; }
public IReadOnlyDictionary<string, int> AggregatedGrants { get; }
public float TotalCostWeight { get; }
public float TotalGrantWeight { get; }
public float NetWeightChange => TotalGrantWeight - TotalCostWeight;
public InventoryTransactionValidationResult Validation { get; }
public bool CanExecute => Validation.IsValid;
internal sealed class InventorySnapshot
public int Capacity { get; }
public float MaxWeight { get; }
public List<InventorySlot> Slots { get; }
public List<EquippedItem> Equipped { get; }
public sealed class InventoryTransaction : IDisposable
public InventoryBill Bill { get; }
public InventoryTransactionValidationResult Validation { get; }
public bool IsCommitted => _isCommitted;
public bool IsCancelled => _isCancelled;
public bool IsActive => !_isCommitted && !_isCancelled;
public bool TryCommit(Action? onCommitted = null) {
public void Cancel() {
public void Dispose() {
```


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/Disease/DiseaseQuarantineCoordinator.cs`

### `Assets/Ashfall.Core/Disease/DiseaseQuarantineCoordinator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 375 lines / 15683 bytes.
- SHA-256: `3a926de1f7f6e5bb05b4f4bd0a57492ceb92dac2142f13ddd6ca4aa3303529fc`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DiseaseQuarantineCoordinator
public event Action<string, string, int>? OnQuarantineAssigned;
public event Action<string, string, int>? OnQuarantineReleased;
public event Action<int, int, float>? OnDailyBurdenProcessed;
public MedicalWardSystem Ward => _medicalWard;
public DiseaseSystem Disease => _diseaseSystem;
public DutyRosterSystem? Roster => _dutyRoster;
public ContainmentCapability Containment =>
public bool IsIsolated(string survivorId) {
public float GetIsolationQuality(string survivorId) {
public MedicalBed? FindAvailableIsolationBed() {
public QuarantineCommandPreview PreviewAssignIsolation(string survivorId) {
public QuarantineCommandResult ExecuteAssignIsolation(string survivorId, int day) {
public QuarantineCommandPreview PreviewReleaseIsolation(string survivorId) {
public QuarantineCommandResult ExecuteReleaseIsolation(string survivorId, int day) {
public void TickDaily(int day) {
public void Rehydrate() {
public sealed class QuarantineCommandPreview
public bool CanExecute { get; set; }
public string Reason { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string TargetBedId { get; set; } = string.Empty;
public string? ConflictingRole { get; set; }
public float ProjectedIsolationQuality { get; set; } = 1.0f;
public Dictionary<string, int> DailySupplyCost { get; set; } = new Dictionary<string, int>();
public static QuarantineCommandPreview Success(string survivorId, string bedId, string? conflictingRole, float projectedQuality, Dictionary<string, int> costs) =>
public static QuarantineCommandPreview Blocked(string reason, string survivorId) =>
public sealed class QuarantineCommandResult
public bool Success { get; set; }
public string Reason { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string BedId { get; set; } = string.Empty;
public int Day { get; set; }
public static QuarantineCommandResult Ok(string survivorId, string bedId, int day) =>
public static QuarantineCommandResult Fail(string reason, string survivorId, string bedId = "", int day = 0) =>
```


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/World/FieldGuideCatalog.cs`

### `Assets/Ashfall.Core/World/FieldGuideCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 198 lines / 6438 bytes.
- SHA-256: `1d2d48b1d3dfd7e199b7d8b5db1b88befe606fa70d14ef47f4f71474451fc865`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FieldGuideEntry
public string Id { get; set; } = string.Empty;
public string Category { get; set; } = string.Empty; // "Fauna" or "Flora"
public string SubjectId { get; set; } = string.Empty;
public string CommonName { get; set; } = string.Empty;
public string ScientificName { get; set; } = string.Empty;
public string Habitat { get; set; } = string.Empty;
public int ThreatLevel { get; set; } = 1;
public string Observation { get; set; } = string.Empty;
public string FieldIntel { get; set; } = string.Empty;
public string TrapPreference { get; set; } = string.Empty;
public string Edibility { get; set; } = string.Empty;
public string UnlockTrigger { get; set; } = string.Empty;
public string ArtId { get; set; } = string.Empty;
public List<string> Tags { get; set; } = new List<string>();
public sealed class FieldGuideCatalogData
public int SchemaVersion { get; set; } = 1;
public string CollectionId { get; set; } = string.Empty;
public List<FieldGuideEntry> Entries { get; set; } = new List<FieldGuideEntry>();
public sealed class FieldGuideState
public List<string> UnlockedEntryIds { get; set; } = new List<string>();
public sealed class FieldGuideCatalog
public int Count => _allEntries.Count;
public IReadOnlyList<FieldGuideEntry> Entries => _allEntries;
public int UnlockedCount => _unlockedIds.Count;
public static FieldGuideCatalog LoadFromJson(string json) {
public static FieldGuideCatalog LoadFromDirectory(string directoryPath, IFileIO fileIO) {
public bool TryGetEntry(string id, out FieldGuideEntry entry) {
public FieldGuideEntry? GetEntry(string id) {
public IReadOnlyList<FieldGuideEntry> GetEntriesByCategory(string category) {
public IReadOnlyList<FieldGuideEntry> GetEntriesByTag(string tag) {
public bool UnlockEntry(string id) {
public bool IsUnlocked(string id) {
public FieldGuideState CaptureState() {
public void RestoreState(FieldGuideState? state) {
```


# Appendix E.19 — Supporting Code Evidence: `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs`

### `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 316 lines / 12926 bytes.
- SHA-256: `0172edb956df8e3ed9e87ce2cb19d01cadddf975cb01758f4b55c41f312c784c`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CreatureDiscoveryRecord
public string CreatureId { get; set; } = string.Empty;
public int DiscoveredDay { get; set; } = 1;
public int EncounterCount { get; set; }
public int KillCount { get; set; }
public int ButcherCount { get; set; }
public string FirstLocationId { get; set; } = string.Empty;
public int LastEncounterDay { get; set; } = 1;
public List<string> UnlockedNoteKeys { get; set; } = new List<string>();
public sealed class CreatureSightingRecord
public string SightingId { get; set; } = string.Empty;
public string CreatureId { get; set; } = string.Empty;
public int Day { get; set; }
public string LocationId { get; set; } = string.Empty;
public string WitnessSurvivorId { get; set; } = string.Empty;
public string SightingType { get; set; } = "spotted"; // spotted, attacked, fled, tracks
public sealed class BestiaryState
public int SchemaVersion { get; set; } = 1;
public int NextSequence { get; set; } = 1;
public List<CreatureDiscoveryRecord> Discoveries { get; set; } = new List<CreatureDiscoveryRecord>();
public List<CreatureSightingRecord> Sightings { get; set; } = new List<CreatureSightingRecord>();
public sealed class BestiarySystem
public const int TotalCanonicalFaunaCount = 24;
public event Action<string>? OnCreatureDiscovered;
public event Action<string, int>? OnCreatureEncountered; // (creatureId, count)
public event Action<string, int>? OnCreatureKilled;      // (creatureId, count)
public event Action<string, string>? OnNoteUnlocked;     // (creatureId, noteKey)
public int DiscoveredCount => _state.Discoveries.Count;
public WastelandBestiaryCatalog Catalog => _catalog;
public void LoadCatalog(string json, IJsonSerializer? serializer = null) {
public CreatureDiscoveryRecord RecordEncounter(string creatureId, int day, string locationId = "", string witnessId = "") {
public void RecordKill(string creatureId, int day, string locationId = "") {
public void RecordButcher(string creatureId, int day) {
public CreatureDiscoveryRecord? GetDiscovery(string creatureId) {
public IReadOnlyList<CreatureDiscoveryRecord> GetAllDiscoveries() => _state.Discoveries;
public IReadOnlyList<CreatureSightingRecord> GetRecentSightings(int limit = 20) {
public float GetCompletionPercentage() {
public bool IsBasicStatsUnlocked(string creatureId) {
public bool IsBehaviorUnlocked(string creatureId) {
public bool IsCombatTacticsUnlocked(string creatureId) {
public BestiaryState CaptureState() {
public void RestoreState(BestiaryState? saved) {
public BestiaryCensus GetCensus() {
public struct BestiaryCensus
public readonly int TotalDiscovered;
public readonly int TotalCanonicalFauna;
public readonly float CompletionPercentage;
public readonly int TotalSightings;
public readonly int TotalKills;
public readonly int TotalButchered;
```


# Appendix E.20 — Supporting Code Evidence: `Assets/Ashfall.Core/ExpansionHubSave.cs`

### `Assets/Ashfall.Core/ExpansionHubSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 507 lines / 28559 bytes.
- SHA-256: `6f9efccbfed1c5101889fb1aeff9b33800d110d217b1f669924bbbfdf2d1e282`.
- Architecture signals: seeded references=0; save/restore symbols=31; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class ExpansionHubSave
public const int CurrentSaveVersion = 6;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public DebtDispatcherState debtDispatcher = new DebtDispatcherState();
public FactionEmbargoLedgerState embargoes = new FactionEmbargoLedgerState();
public DebtConsequenceBridgeState debtBridge = new DebtConsequenceBridgeState();
public SaltMineState saltMine = new SaltMineState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV5
public int saveVersion = 5;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public DebtDispatcherState debtDispatcher = new DebtDispatcherState();
public FactionEmbargoLedgerState embargoes = new FactionEmbargoLedgerState();
public DebtConsequenceBridgeState debtBridge = new DebtConsequenceBridgeState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV1
public int saveVersion = 1;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV2
public int saveVersion = 2;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV3
public int saveVersion = 3;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV4
public int saveVersion = 4;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public string Checksum = string.Empty;
public static class ExpansionHubSaveCodec
public static ExpansionHubSave Capture( int simDay, WaystationSystem waystation, LocationLayoutSystem layouts, LocationMemorySystem memory, SiteEncounterSystem siteEncounters,
public static string Encode(ExpansionHubSave save, IJsonSerializer json) {
public static ExpansionHubSave Decode(string jsonText, IJsonSerializer json) {
public static void Restore( ExpansionHubSave save, WaystationSystem waystation, LocationLayoutSystem layouts, LocationMemorySystem memory, SiteEncounterSystem siteEncounters,
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/WildlifeTrappingCatalogTestFixture.cs`

### `Ashfall.Core.Tests/WildlifeTrappingCatalogTestFixture.cs`

- Current test declarations: Fact=0, Theory=0, InlineData=0.
- File lines: 364; SHA-256: `705e77d015029049610d4b8bd10134706a154476f9582d61e9f55c39af718e6d`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/WildlifeTrapping/WildlifeTrappingRuntimeCompletionTests.cs`

### `Ashfall.Core.Tests/WildlifeTrapping/WildlifeTrappingRuntimeCompletionTests.cs`

- Current test declarations: Fact=28, Theory=0, InlineData=0.
- File lines: 995; SHA-256: `c423548dfec379a10a223738751c33a1f1eca7fdd0fe0eaff87bea1f62eccd1a`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Task1_01_SetupCosts_ExactBalance_AtomicallyDeductedAndTrapDeployed
Task1_02_SetupCosts_ShortByOneItem_FailsAtomically_ZeroItemsConsumed
Task1_03_SetupCosts_DuplicateCostEntries_AggregatedCorrectly
Task1_04_SetupCosts_ZeroCostTrap_DeploysWithoutDeductions
Task1_05_SetupCosts_TrapIdentityPreservedAcrossSaveLoad
Task2_01_Durability_DecrementsOnCheck_Catch
Task2_02_Durability_DecrementsOnCheck_NoCatch
Task2_03_Durability_BreaksAtZero
Task2_04_Durability_BrokenTrapProducesNoCatches_SkipsRng
Task2_05_Durability_LegacySave_DefaultsToMinusOne_NeverBreaks
Task2_06_Durability_SnareBreaksEarlierThanCage
Task2_07_Repair_RestoresDefinitionDurability_ClearsBroken
Task2_08_RepairBill_AffordabilityPreflight_WireAndBox
Task3_01_Crafting_ThreeCoreTrapRecipesExistAndResolveCleanly
Task3_02_Crafting_CraftItemThenDeploy_ConsumesItemWithoutDoubleCharging
Task4_01_SeasonGating_PreyExcludedOutOfSeason
Task4_02_SeasonGating_PreyIncludedInSeason
Task4_03_SeasonGating_YearRoundPrey_AlwaysEligible
Task4_04_MigrationGating_AbsentPackExcludesMigrationPrey
Task4_05_MigrationGating_PresentPackIncludesMigrationPrey
Task4_06_SeasonalAbundance_ZeroAbundanceExcludesPrey
Task4_07_DeterministicReplay_SameContextProducesIdenticalCatch
Task5_01_HighRiskPrey_DiseaseAndContaminationRecordedOnCatch
Task5_02_DeterministicMiss_NoDiseaseOrContaminationApplied
Task5_03_Contamination_DoseAppliedToRadiationSystem
Task5_04_Disease_InfectionRecordedInDiseaseSystem
Task5_05_SaveLoad_PreservesCatchRiskFields
Task8_01_Deterministic100DaySimulation_AndBaselineMetrics
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/WildlifeTrappingCatalogIntegrationTests.cs`

### `Ashfall.Core.Tests/WildlifeTrappingCatalogIntegrationTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 317; SHA-256: `df8ccd3daa417a7572cff3c53a094295ceda76c1058d0c935b7d18e4ff796df5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Task8_01_CatalogLoads_WithExactAuthoredCounts
Task8_02_CatalogIds_AreNonEmptyUniqueAndConformant
Task8_03_CrossReferences_ResolveCleanly
Task8_04_RegisterWith_RegistersAllTrapsPreyAndBaits
Task8_05_EveryPrey_IsFoundAndAccurateInRuntimeRegistry
Task8_06_EveryBait_IsFoundAndAccurateInRuntimeRegistry
Task8_07_EveryTrapDefinition_IsFoundAndAccurateInRuntimeRegistry
Task8_08_DeployEveryTrapType_RetainsCorrectTrapIdAndDurability
Task8_09_RuntimeConsumability_LiveTrappingAndButcheryCycleSucceeds
```


# Appendix G.24 — Supporting Regression Evidence: `Ashfall.Core.Tests/WildlifeDiseaseFallbackTests.cs`

### `Ashfall.Core.Tests/WildlifeDiseaseFallbackTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 221; SHA-256: `7c35bb542c3a960d6615d0ea53c0a8d4ccc748dabfa509119f8d166c39492fcc`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Task5_01_Catalog_ContainsExactly15Prey_AllUniqueAndValid
Task5_02_NamedFallbackPrey_FollowExactTier
Task5_03_ExactBoundary_RiskTable_IsInclusiveAtPointOne
Task5_04_ExplicitOverrides_AlwaysBeatTierFallback
Task5_05_WholeCatalogSweep_All15PreyMatchIndependentOracle
Task5_06_ResolveDiseaseId_RunsExactlyOncePerButchery
Task5_07_DefensiveDiseaseCases
```


# Appendix H.25 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

### `docs/CURRENT_AUTHORITY.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 102 lines / 9950 bytes.
- SHA-256: `7dea2c12b4863bfc9a3c2ebb161ba51512ebc06475b762abd5d5d205bef47e5c`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=0; textual Godot mentions=6; textual Unity/JsonUtility mentions=2; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| trap/prey/bait definitions | WildlifeTrappingCatalog | sites, catches, bycatch and durability | WildlifeTrappingSystem | Owner emits/reads a typed fact; no mirror state. |
| trap/prey/bait definitions | WildlifeTrappingCatalog | inventory bills and effect routing | WildlifeTrappingHostSession | Owner emits/reads a typed fact; no mirror state. |
| trap/prey/bait definitions | WildlifeTrappingCatalog | health and contamination consequences | Wildlife/disease/dose owners | Owner emits/reads a typed fact; no mirror state. |
| trap/prey/bait definitions | WildlifeTrappingCatalog | catalog, replay, persistence and disease proof | Wildlife trapping tests | Owner emits/reads a typed fact; no mirror state. |
| sites, catches, bycatch and durability | WildlifeTrappingSystem | trap/prey/bait definitions | WildlifeTrappingCatalog | Owner emits/reads a typed fact; no mirror state. |
| sites, catches, bycatch and durability | WildlifeTrappingSystem | inventory bills and effect routing | WildlifeTrappingHostSession | Owner emits/reads a typed fact; no mirror state. |
| sites, catches, bycatch and durability | WildlifeTrappingSystem | health and contamination consequences | Wildlife/disease/dose owners | Owner emits/reads a typed fact; no mirror state. |
| sites, catches, bycatch and durability | WildlifeTrappingSystem | catalog, replay, persistence and disease proof | Wildlife trapping tests | Owner emits/reads a typed fact; no mirror state. |
| inventory bills and effect routing | WildlifeTrappingHostSession | trap/prey/bait definitions | WildlifeTrappingCatalog | Owner emits/reads a typed fact; no mirror state. |
| inventory bills and effect routing | WildlifeTrappingHostSession | sites, catches, bycatch and durability | WildlifeTrappingSystem | Owner emits/reads a typed fact; no mirror state. |
| inventory bills and effect routing | WildlifeTrappingHostSession | health and contamination consequences | Wildlife/disease/dose owners | Owner emits/reads a typed fact; no mirror state. |
| inventory bills and effect routing | WildlifeTrappingHostSession | catalog, replay, persistence and disease proof | Wildlife trapping tests | Owner emits/reads a typed fact; no mirror state. |
| health and contamination consequences | Wildlife/disease/dose owners | trap/prey/bait definitions | WildlifeTrappingCatalog | Owner emits/reads a typed fact; no mirror state. |
| health and contamination consequences | Wildlife/disease/dose owners | sites, catches, bycatch and durability | WildlifeTrappingSystem | Owner emits/reads a typed fact; no mirror state. |
| health and contamination consequences | Wildlife/disease/dose owners | inventory bills and effect routing | WildlifeTrappingHostSession | Owner emits/reads a typed fact; no mirror state. |
| health and contamination consequences | Wildlife/disease/dose owners | catalog, replay, persistence and disease proof | Wildlife trapping tests | Owner emits/reads a typed fact; no mirror state. |
| catalog, replay, persistence and disease proof | Wildlife trapping tests | trap/prey/bait definitions | WildlifeTrappingCatalog | Owner emits/reads a typed fact; no mirror state. |
| catalog, replay, persistence and disease proof | Wildlife trapping tests | sites, catches, bycatch and durability | WildlifeTrappingSystem | Owner emits/reads a typed fact; no mirror state. |
| catalog, replay, persistence and disease proof | Wildlife trapping tests | inventory bills and effect routing | WildlifeTrappingHostSession | Owner emits/reads a typed fact; no mirror state. |
| catalog, replay, persistence and disease proof | Wildlife trapping tests | health and contamination consequences | Wildlife/disease/dose owners | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 10/15 “create” brief with a current 10/15/6 census and consumer matrix. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Document the exact owner boundary among catalog, trap system, inventory transaction, disease, dose, journal and codex. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Preserve deterministic replay and save/legacy normalization as release gates. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Audit UI disclosure of trap durability, catch risk, contamination and repair affordability. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

Every requirement in the objective and delta must resolve to at least one current owner, one negative condition and one future focused verification. A requirement with no owner is removed or returned as `STALE_PLAN`.

# Appendix K — Status and Evidence Labels

| Claim type | Label | Meaning |
| --- | --- | --- |
| Current source | VERIFIED PATH INVENTORY | Appendices hash current files; declarations are read-only. |
| Historical completion | HISTORICAL, NOT FRESH TEST PROOF | Focused tests must be rerun by an implementation/verification package. |
| Proposed types/paths | PROPOSAL ONLY | Never shown as current evidence; requires a new claim. |
| Character count | COMPLETENESS CHECK ONLY | External verifier records it; no padding or repeated boilerplate. |

# Appendix L — Plan Maintenance and Re-Audit Triggers

Re-run the premise sweep when any of the following occurs:

1. A listed Core owner is renamed, split, merged or removed.
2. A listed catalog changes `schema_version`, root shape or consumer.
3. A save section, checksum contract or campaign-day order changes.
4. A listed test is removed, renamed or moved to quarantine.
5. A live ledger marks a surface sealed, retired, accepted or blocked.
6. A generated architecture/save/catalog matrix changes the owner relationship.
7. A new active claim touches any current or proposed path.

The re-audit records only changed evidence. Historical prose is not rewritten merely to appear current, and current evidence is not deleted merely because an old plan disagrees with it.

# Appendix M — Definition of a Safe No-Change Result

A safe no-change result is valid when the current implementation already satisfies the requested behavior. It records: current owner paths, focused tests that exist, any rerun performed by a future verification package, and the precise condition that would justify reopening. A no-change result does not create a placeholder subsystem, a synthetic integration framework, or a test solely to increase counts.

# Appendix N — Final Precision Checklist

- [ ] Every current path in this document exists or is explicitly labeled unavailable.
- [ ] Every proposed path/type is labeled `PROPOSAL` and excluded from current claims.
- [ ] No current owner is duplicated.
- [ ] Core architecture remains engine-free.
- [ ] JSON is described as data authority, not automatic reachability.
- [ ] Save impact names the current owner and migration behavior.
- [ ] Determinism names streams or explicitly states no randomness.
- [ ] UI remains presentation over owner commands.
- [ ] Tests are focused and current commands use `scripts/run_test.sh` policy.
- [ ] Sealed/retired/blocked ledger decisions are respected.
- [ ] No full-suite result is claimed without a dedicated execution window.
- [ ] No Unity dependency or historical architecture is proposed.
- [ ] Character count is not used as evidence of quality.
- [ ] The first implementation step is a premise recheck, not code creation.
- [ ] The implementation handoff can be executed without reinterpreting ownership.

# Appendix O — Handoff Record Template

```text
Package:
Current status rechecked:
Outcome implemented:
Files changed:
Current owner contract used:
Save section/version touched:
Determinism streams touched:
Focused verification commands and results:
Tests reused/added:
Known limitation or debt:
Shared files intentionally untouched:
Ready for independent sweep: yes/no
```


# Appendix Q.557 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs`

### `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 651 lines / 32234 bytes.
- SHA-256: `beef926f2f7a20485888a8c6112ee6cbdbe2b5ace9750ac879c41022aabf64c8`.
- Architecture signals: seeded references=4; save/restore symbols=2; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FaunaSpeciesDef
public string id { get; set; } = string.Empty;              // species_*
public string display_name { get; set; } = string.Empty;
public float radiation_tolerance { get; set; } = 0.5f;      // 0..1 vs OutdoorRadModifier/250
public string diet_type { get; set; } = "herbivore";        // herbivore | carnivore | scavenger
public int apex_population_threshold { get; set; }          // 0 = not an apex species
public bool tameable { get; set; }
public float tameness_chance { get; set; } = 0.15f;
public List<string> tags { get; set; } = new List<string>();
public sealed class PredatorPreyEdgeDef
public string predator_species_id { get; set; } = string.Empty;
public string prey_species_id { get; set; } = string.Empty;
public float predation_pressure { get; set; } = 0.05f;      // fraction of prey removed per day
public sealed class SeasonalMoveDef
public string species_id { get; set; } = string.Empty;
public string season_window_id { get; set; } = string.Empty;
public float move_chance { get; set; } = 0.1f;              // per pack per day in window
public sealed class WildlifeEcosystemContainer
public int schema_version { get; set; } = 1;
public List<FaunaSpeciesDef> species { get; set; } = new List<FaunaSpeciesDef>();
public List<PredatorPreyEdgeDef> predator_prey { get; set; } = new List<PredatorPreyEdgeDef>();
public List<SeasonalMoveDef> seasonal_moves { get; set; } = new List<SeasonalMoveDef>();
public sealed class PressureKeyState
public string sector_id { get; set; } = string.Empty;
public string species_id { get; set; } = string.Empty;
public int pressure { get; set; }
public sealed class ApexActivityState
public string species_id { get; set; } = string.Empty;
public string sector_id { get; set; } = string.Empty;
public int since_day { get; set; }
public int until_day { get; set; }
public bool spotted_reported { get; set; }
public sealed class DomesticAnimalState
public string animal_id { get; set; } = string.Empty;
public string species_id { get; set; } = string.Empty;
public int tamed_day { get; set; }
public string caretaker_id { get; set; } = string.Empty;
public sealed class WildlifeObservation
public string species_id { get; set; } = string.Empty;
public string sector_id { get; set; } = string.Empty;
public int day { get; set; }
public float confidence { get; set; } = 1f;
public sealed class WildlifeEcosystemState
public string system_id { get; set; } = "wildlife_ecosystem";
public int schema_version { get; set; } = 1;
public int last_tick_day { get; set; }
public List<PressureKeyState> pressures { get; set; } = new List<PressureKeyState>();
public List<string> extinct_species_sectors { get; set; } = new List<string>(); // "sector|species"
public List<ApexActivityState> apex_activities { get; set; } = new List<ApexActivityState>();
public List<DomesticAnimalState> domestic_animals { get; set; } = new List<DomesticAnimalState>();
public List<WildlifeObservation> observations { get; set; } = new List<WildlifeObservation>();
public int domestic_counter;
public sealed class WildlifeEcosystemSystem
public const string SystemId = "wildlife_ecosystem";
public const int ExtinctionThreshold = 2;      // remnant pair = locally extinct
public const int RecolonizationPopulation = 4;
public const int ApexDurationDays = 5;
public const int ObservationLogCapacity = 200;
public const int PressureDecayPerDay = 1;
public const float HazardAvoidanceMigrationChance = 0.35f;
public event Action<string, string>? OnWildlifeObserved;             // species, sector
public event Action<string, string>? OnLocalExtinction;              // species, sector
public event Action<string, string, string>? OnHazardAvoidanceMigration; // species, fromSector, toSector
public event Action<string, string>? OnApexPredatorSpotted;          // species, sector
public event Action<DomesticAnimalState>? OnWildlifeTamed;
public event Action<string, string, int>? OnWildlifePopulationShifted; // species, sector, delta
public string SaveId => SystemId;
public WildlifeEcosystemState State => _state;
public WildlifeEcosystemContainer Catalog => _catalog;
public IReadOnlyList<DomesticAnimalState> DomesticAnimals => _state.domestic_animals;
public IReadOnlyList<ApexActivityState> ApexActivities => _state.apex_activities;
public IReadOnlyList<WildlifeObservation> Observations => _state.observations;
public void LoadCatalog(WildlifeEcosystemContainer catalog) {
public FaunaSpeciesDef? Species(string speciesId) =>
public int SectorSpeciesPopulation(WildlifeMigrationSystem migration, string sectorId, string speciesId) {
public bool IsLocallyExtinct(string sectorId, string speciesId) =>
public float SectorDensityMultiplier(WildlifeMigrationSystem migration, string sectorId) {
public static string ExtinctionKey(string sectorId, string speciesId) => sectorId + "|" + speciesId;
public void RecordHuntingPressure(string sectorId, string speciesId, int amount) {
public int PressureOn(string sectorId, string speciesId) {
public void RecordObservation(string speciesId, string sectorId, int day, float confidence = 1f) {
public int ObservationCount(string speciesId) {
public string KnowledgeLevel(string speciesId) {
public void TickDay( int day, WildlifeMigrationSystem migration, float outdoorRadModifier, string seasonWindowId, ISeededRng populationRng,
public bool CanTame(string speciesId) =>
public DomesticAnimalState? TryTame( string speciesId, string sectorId, int day, string caretakerId, WildlifeMigrationSystem migration, ISeededRng tamingRng) {
public WildlifeEcosystemState CaptureState() {
public void RestoreState(WildlifeEcosystemState? state) {
public static class WildlifeEcosystemCatalogLoader
public const string DefaultFileName = "wildlife_ecosystem.json";
public static WildlifeEcosystemContainer Load( string dataDir, IFileIO? files = null, IJsonSerializer? json = null) {
public static List<string> Validate(WildlifeEcosystemContainer catalog) {
```


# Appendix Q.558 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/WildlifeHarvestLedger.cs`

### `Assets/Ashfall.Core/World/WildlifeHarvestLedger.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 164 lines / 7661 bytes.
- SHA-256: `86fb21dc40c8b4b0b98fe9ebe8343d75f8ba37b80944fbd9174e9ede447317d0`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SpeciesHarvestRecord
public string SpeciesId { get; set; } = string.Empty;
public int HarvestTakenThisSeason { get; set; }
public int LastMaxSafeQuota { get; set; }
public int LastPopulationPermille { get; set; }
public SpeciesPopulationBand LastPostHarvestBand { get; set; } = SpeciesPopulationBand.Stable;
public sealed class WildlifeHarvestState
public int SchemaVersion { get; set; } = 1;
public int Season { get; set; }
public Dictionary<string, SpeciesHarvestRecord> Species { get; set; } =
public WildlifeHarvestState Clone() => new WildlifeHarvestState
public struct WildlifeHarvestCensus
public int TrackedSpecies { get; }
public int SpeciesAtRisk { get; }
public int SpeciesOverQuota { get; }
public int TotalHarvestTaken { get; }
public int Season { get; }
public sealed class WildlifeHarvestLedger
public IReadOnlyDictionary<string, SpeciesHarvestRecord> Species => _state.Species;
public int Season => _state.Season;
public int TrackedSpecies => _state.Species.Count;
public WildlifeHarvestState CaptureState() => _state.Clone();
public void RestoreState(WildlifeHarvestState? saved) {
public HarvestQuotaResult EvaluateQuota(string speciesId, int populationPermille, int reproductionPermille, int requestedUnits) {
public HarvestQuotaResult ApplyHarvest(string speciesId, int populationPermille, int reproductionPermille, int requestedUnits) {
public void BeginSeason(int season) {
public PredatorConflictPosture EvaluatePredatorConflict(int predatorPopulationPermille, int proximityMetres, int shelterNoisePermille) =>
public TamingReadinessResult EvaluateTamingReadiness(int animalHungerPermille, int trustExposurePermille, int speciesTamabilityPermille, int tamingSeed) =>
public bool TryGetSpecies(string speciesId, out SpeciesHarvestRecord record) =>
public WildlifeHarvestCensus GetCensus() {
public void Clear() => _state.Species.Clear();
```


# Appendix Q.559 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Disease/DiseaseHeadlessDemo.cs`

### `Assets/Ashfall.Core/Disease/DiseaseHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 489 lines / 28179 bytes.
- SHA-256: `d985f4110e9c912b9818316dad3b3cf219b2d9b5703fed11e1e5531a51a47b1b`.
- Architecture signals: seeded references=8; save/restore symbols=11; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DiseaseHeadlessDemo
public const int DemoSeed = 1013;
public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null) {
public string id = string.Empty;
public int schema_version;
public List<DiseaseDemoItemRow> items = new List<DiseaseDemoItemRow>();
```


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Inventory/Inventory.cs`

### `Assets/Ashfall.Core/Inventory/Inventory.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1352 lines / 53625 bytes.
- SHA-256: `38b5c11e1b92bf7d767b47beb3ff290b3dfc8360233858a7dd6e2854c3e264a9`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=29; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public struct EquippedGearData
public float RadProtection;
public float MaxDurability;
public float CurrentDurability;
public float DegradeRate;
public class WornGear
public EquippedItem? SourceEquipped;
public IEquipmentConditionSink? ConditionSink;
public ItemDefinition? SourceItem;
public float RadProtection;
public float MaxDurability;
public float CurrentDurability;
public float DegradeRate;
public Action<float>? OnDegraded;
public float DurabilityFraction() {
public float EffectiveProtection() {
public void Degrade(float gameHours) {
public class Inventory : IPlayerInventoryPort, IEquipmentConditionSink
public const float ContaminationDosePerUnit = 50f;
public int Capacity = 20;
public float MaxWeight = 100f;
public IReadOnlyList<InventorySlot> Slots => _slots;
public IReadOnlyList<EquippedItem> Equipped => _equipped;
public IReadOnlyList<InventorySlot> GetSlots() => _slots;
public event Action<ItemDefinition, int> OnItemAdded;
public event Action<ItemDefinition, int> OnItemRemoved;
public event Action OnInventoryChanged;
public bool HasSufficient(string itemId, int count) {
public bool TryConsume(string itemId, int count, Action? onCommitted = null) {
public bool TryConsumeById(string itemId, int count) => TryConsume(itemId, count);
public bool TryProduce(string itemId, int count, ItemDefinition? def = null) {
public bool Remove(string itemId, int amount) => RemoveById(itemId, amount);
public int Count(ItemDefinition item) {
public int CountById(string itemId) {
public int CountByType(ItemType type) {
public int RemoveByType(ItemType type, int amount) {
public float FoodFillRatio() {
public float WaterFillRatio() {
public float FuelFillRatio() {
public InventorySlot? FindSlot(string itemId) {
public InventorySlot? FindBestWorkingDevice(string itemId) {
public bool HasWorkingGeiger() {
public DeviceState? GetBestGeigerState() {
public void DriftAllDevices(float days = 1f) {
public bool RechargeDevice(string deviceItemId, ItemDefinition batteryItem) {
public bool RecalibrateDevice(string deviceItemId, ItemDefinition kitItem, int currentDay) {
public float GetCurrentWeight() {
public bool CanAdd(ItemDefinition item, int amount) {
public bool Add(ItemDefinition item, int amount) {
public bool AddById(string itemId, int amount) {
public bool CanAddById(string itemId, int amount) {
public bool Remove(ItemDefinition item, int amount) {
public bool RemoveById(string itemId, int amount) {
public void Clear() {
public InventoryTransactionValidationResult ValidateTransaction( InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
public InventoryTransactionQuote Quote( InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
public InventoryTransactionQuote QuoteTransaction( InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
public InventoryTransaction BeginTransaction( InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
public bool TryExecuteTransaction( InventoryBill bill, Action? onCommitted = null, Func<string, ItemDefinition?>? lookup = null) {
public bool TryConsumeBill(IReadOnlyDictionary<string, int> costs, Action? onCommitted = null) {
public bool TryConsumeBill(IEnumerable<KeyValuePair<string, int>> costs, Action? onCommitted = null) {
public bool TryConsumeBill(IEnumerable<string> itemIds, Action? onCommitted = null) {
internal void ApplyTransactionMutations(InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
internal void NotifyTransactionCommitted(InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
internal void RestoreSnapshot(InventorySnapshot snapshot) {
public bool Transfer(ItemDefinition item, int amount, Inventory destination) {
public bool Equip(ItemDefinition item) {
public bool Equip( ItemDefinition item, IEnumerable<Medical.LimbState>? limbs, Func<string, ItemDefinition?>? catalog = null, Func<string, float>? conditionProvider = null) {
public ItemDefinition? Unequip(EquipSlot slot) {
public bool TryUnequipTo(EquipSlot slot, Inventory destination) {
public EquippedItem? GetEquipped(EquipSlot slot) {
public float GetEquippedProtection() {
public List<WornGear> BuildWornGear() {
public void FillWornGear(List<WornGear> buffer) {
public event Action<EquippedItem, string>? OnProtectiveGearFailed;
public bool TryRepairEquippedGear(EquippedItem item) {
public event Action<EquippedItem, float>? OnProtectiveGearRepaired;
public void RecordWear(EquippedItem item, float wearDelta, string cause = "radiation") {
public void DegradeEquippedGear(float gameHours, float multiplier = 1f, string cause = "use") {
public sealed class ProtectiveLifeEstimate
public string ItemId = string.Empty;
public string DisplayName = string.Empty;
public float CurrentDurability;
public float MaxDurability;
public float DegradeRate;
public float ExposureMultiplier = 1f;
public float HoursRemaining;
public bool TryEstimateWeakestProtectiveLife( float exposureMultiplier, out ProtectiveLifeEstimate? life) {
public bool Consume( ItemDefinition item, Func<ItemType, float, bool>? applyNeed = null, Action<float>? applyRadCleanse = null, Action? applyIodine = null, Action<float>? applyContamination = null,
public InventorySaveState CaptureState() {
public void ResortSlotsByType() {
public bool IsSortedByType() {
public void RestoreState(InventorySaveState state, Func<string, ItemDefinition?> lookup) {
public class InventorySlot
public ItemDefinition Item;
public int Amount;
public DeviceState? Device;
public float CurrentDurability = -1f;
public float GetDurability() {
public bool IsBrokenOrDegraded() {
public class EquippedItem
public ItemDefinition Item;
public float CurrentDurability;
public class InventorySaveState
public int capacity;
public float maxWeight;
public List<SlotSave> slots = new List<SlotSave>();
public List<EquippedSave> equipped = new List<EquippedSave>();
public class SlotSave
public string itemId;
public int amount;
public bool hasDevice;
public float battery;
public float calibration;
public bool broken;
public int lastCalibratedDay;
public class EquippedSave
public string itemId;
public float durability;
```


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/DesperationSystem.cs`

### `Assets/Ashfall.Core/Survivors/DesperationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 288 lines / 11666 bytes.
- SHA-256: `ae21338fa8cfaca95506734b57332cfe6f0dc52a0c3403c09d853980b5bf6626`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DesperationEventDef
public string desperation_id { get; set; } = string.Empty;
public string event_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public float required_starvation { get; set; } = 90.0f;
public List<string> required_conditions { get; set; } = new List<string>();
public List<string> forbidden_conditions { get; set; } = new List<string>();
public string resource_yield_item_id { get; set; } = "raw_meat";
public int resource_yield_count { get; set; } = 6;
public string taboo_level { get; set; } = "Broken";
public float morale_shock { get; set; } = 35.0f;
public float prion_risk { get; set; } = 0.15f;
public float mutiny_pressure { get; set; } = 25.0f;
public string trait_granted { get; set; } = "trait_cannibal";
public string trait_awarded { get; set; } = "trait_cannibal";
public string Id => !string.IsNullOrEmpty(desperation_id) ? desperation_id : event_id;
public sealed class DesperationCatalogContainer
public int schema_version { get; set; } = 1;
public List<DesperationEventDef> events { get; set; } = new List<DesperationEventDef>();
public sealed class DesperationActRecord
public string actId { get; set; } = string.Empty;
public string eventId { get; set; } = string.Empty;
public string actorId { get; set; } = string.Empty;
public string corpseId { get; set; } = string.Empty;
public int day { get; set; }
public string tabooLevel { get; set; } = "Broken";
public int meatYield { get; set; }
public bool prionContracted { get; set; }
public sealed class DesperationState
public string systemId = DesperationSystem.SystemId;
public float mutinyPressure;
public List<string> harvestedCorpseIds = new List<string>();
public List<string> cannibalSurvivorIds = new List<string>();
public List<DesperationActRecord> actsHistory = new List<DesperationActRecord>();
public List<string> unburiedCorpseIds = new List<string>();
public List<string> buriedCorpseIds = new List<string>();
public List<string> oneShotShockIds = new List<string>();
public sealed class DesperationSystem
public const string SystemId = "desperation";
public const float CrisisStarvationThreshold = 90.0f;
public DesperationState State => _state;
public float MutinyPressure => _state.mutinyPressure;
public event Action<DesperationActRecord>? OnTabooBroken;
public event Action<float>? OnMutinyPressureChanged;
public void LoadCatalog(string dataPath) {
public void RegisterEvent(DesperationEventDef def) {
public void RegisterCorpse(string corpseId) {
public ActionResult PerformBurial(string corpseId) {
public bool IsActionEligible(string survivorId, string eventId, string corpseId) {
public ActionResult HarvestCorpse(string actorId, string corpseId, string eventId, int currentDay, bool hasCynicalTrait = false) {
public DesperationState CaptureState() {
public void RestoreState(DesperationState state) {
```


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/PharmaLabSystem.cs`

### `Assets/Ashfall.Core/PharmaLabSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 283 lines / 11728 bytes.
- SHA-256: `8fcbaed015f729821875ddad0a7de831ef943a8d02d5b55e8d9e57e826265bbf`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=9; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PharmaLabState
public string systemId = PharmaLabSystem.SystemId;
public bool isProcessing;
public string currentRecipeId = string.Empty;
public string assignedChemistId = string.Empty;
public float progressHours;
public float hoursRequired;
public PharmaPhase currentPhase;
public float temperature;
public float purity;
public float contaminationRisk;
public List<string> reservedInputIds = new List<string>();
public List<int> reservedInputAmounts = new List<int>();
public List<string> completedRecipeIds = new List<string>();
public int totalBatchesProduced;
public int totalDependencyEvents;
public enum PharmaPhase { Idle, Mixing, Heating, Distillation, Cooling, Purification, Complete } [Serializable] public sealed class PharmaRecipe { public string recipe_id = string.Empty; public string display_name = string.Empty; public List<string> input_ids = new List<string>(); public List<int> input_amounts = new List<int>(); public string output_item_id = string.Empty; public int output_amount = 1; public float base_hours = 2f; public float required_temperature = 80f; public float purity_target = 0.9f; public float dependency_risk; // 0-1, chance of addiction event public string required_station = "pharma_bench"; public string category = "pharmaceutical"; }
public sealed class PharmaRecipeCatalog
public string schema_version = "1.0";
public List<PharmaRecipe> recipes = new List<PharmaRecipe>();
public sealed class PharmaLabSystem
public const string SystemId = "pharma_lab";
public PharmaLabState State => _state;
public IReadOnlyDictionary<string, PharmaRecipe> Recipes => _recipes;
public bool IsProcessing => _state.isProcessing;
public event Action<ActionResult> OnBatchCompleted;
public event Action<float> OnDependencyRisk; // parameter = risk level
public event Action OnPharmaStateChanged;
public void BindSkillEvaluator(Func<string, float> evaluator) {
public void LoadCatalog(PharmaRecipeCatalog catalog) {
public void RegisterRecipe(PharmaRecipe recipe) {
public PharmaRecipe? GetRecipe(string id) {
public ActionResult StartBatch(string recipeId, string chemistId) {
public ActionResult TickProgress(float hours) {
public ActionResult CancelBatch() {
public PharmaLabState CaptureState() => CloneState(_state);
public void RestoreState(PharmaLabState saved) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

### `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 2073 lines / 130366 bytes.
- SHA-256: `7588dbb7ed053936964371ce06c49160f772cb9fffd2e7d519884ab430f044c4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ContentUtilizationScanner
public static bool IsNarrativeSubdirectoryFile(string relativePath) {
public static bool IsAuthoritativeCatalog(string fileName) {
public ContentUtilizationGraph Scan() {
```


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Shelter/AeroponicsSystem.cs`

### `Assets/Ashfall.Core/Shelter/AeroponicsSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 527 lines / 22277 bytes.
- SHA-256: `9665008eab5558955c3d7f66cb831b376fcadd120864cb9eb308f7d0e1e67c54`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum AeroponicLightMode
public enum RootDiseaseStage
public sealed class AeroponicNutrientDefinition
public string NutrientProfileId { get; set; } = string.Empty;
public string CropTag { get; set; } = string.Empty;
public float Nitrogen { get; set; }
public float Phosphorus { get; set; }
public float Potassium { get; set; }
public float Iron { get; set; }
public float Calcium { get; set; }
public float Magnesium { get; set; }
public float OptimalEcMin { get; set; } = 1f;
public float OptimalEcMax { get; set; } = 2.5f;
public float OptimalPhMin { get; set; } = 5.5f;
public float OptimalPhMax { get; set; } = 6.5f;
public string YieldItemId { get; set; } = string.Empty;
public int YieldAmount { get; set; } = 1;
public bool Medicinal { get; set; }
public float BaseGrowthPerDay { get; set; } = 10f;
public sealed class AeroponicsNutrientCatalog
public int SchemaVersion { get; set; } = 1;
public List<AeroponicNutrientDefinition> Profiles { get; set; } =
public sealed class AeroponicChamberState
public string ChamberId = string.Empty;
public string RoomId = string.Empty;
public string CropCycleId = string.Empty;
public string NutrientProfileId = string.Empty;
public float MistIntervalSeconds = 30f;
public float ReservoirEcMsCm = 1.5f;
public float ReservoirPh = 6f;
public float NutrientBalance = 1f;
public float RootBiomass;
public float GrowthPct;
public float NozzleConditionPct = 100f;
public float ReservoirTemperatureC = 20f;
public RootDiseaseStage DiseaseStage;
public AeroponicLightMode LightMode = AeroponicLightMode.Balanced;
public float WaterQuality = 1f;
public float ReservoirLitres;
public float PowerAvailability01 = 1f;
public int PlantedDay = -1;
public int LastMaintenanceDay = -1;
public int LastDiseaseRollDay = -1;
public int LastHarvestDay = -1;
public bool HarvestResolved;
public sealed class AeroponicsState
public string SystemId = AeroponicsSystem.SystemId;
public List<AeroponicChamberState> Chambers = new List<AeroponicChamberState>();
public int LastProcessedDay = -1;
public sealed class AeroponicsSnapshot
public string ChamberId = string.Empty;
public string CropCycleId = string.Empty;
public float GrowthPct;
public float RootBiomass;
public float ReservoirEcMsCm;
public float ReservoirPh;
public float NozzleConditionPct;
public RootDiseaseStage DiseaseStage;
public AeroponicLightMode LightMode;
public float PowerAvailability01;
public float ReservoirLitres;
public bool HarvestReady;
public sealed class AeroponicHarvestResult
public bool Success;
public string ItemId = string.Empty;
public int Amount;
public bool Medicinal;
public sealed class AeroponicsSystem
public const string SystemId = "aeroponics";
public const float MaximumGrowthMultiplier = 3f;
public const float BaseWaterPerDayLitres = 4f;
public AeroponicsState State => _state;
public IReadOnlyDictionary<string, AeroponicNutrientDefinition> Profiles => _profiles;
public IReadOnlyList<AeroponicChamberState> Chambers => _state.Chambers;
public event Action<AeroponicChamberState>? OnChamberChanged;
public event Action<string>? OnDiseaseMilestone;
public event Action<AeroponicHarvestResult>? OnHarvested;
public void LoadCatalog(AeroponicsNutrientCatalog? catalog) {
public void RegisterProfile(AeroponicNutrientDefinition profile) {
public ActionResult AddChamber(string chamberId, string roomId) {
public ActionResult Plant(string chamberId, string cropCycleId, string nutrientProfileId, int day) {
public ActionResult SetChemistry(string chamberId, float ecMsCm, float ph) {
public ActionResult SetLightMode(string chamberId, AeroponicLightMode mode) {
public ActionResult AddWater(string chamberId, float litres, float quality01) {
public ActionResult MaintainNozzles(string chamberId, float amount, int day) {
public ActionResult TreatRootDisease(string chamberId, int day) {
public void TickDay(int day, float operatorSkill = 0f) {
public AeroponicHarvestResult Harvest(string chamberId, int day) {
public AeroponicsSnapshot Snapshot(string chamberId) {
public AeroponicsState CaptureState() {
public void RestoreState(AeroponicsState? saved) {
public static class AeroponicsCatalogLoader
public const string FileName = "aeroponics_nutrient_catalog.json";
public static AeroponicsNutrientCatalog? Load( string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
```


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/DiseaseAfflictionHandler.cs`

### `Assets/Ashfall.Core/Medical/DiseaseAfflictionHandler.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 176 lines / 8179 bytes.
- SHA-256: `b839868c1ca10cdb00c489920220545281f04590d762a75b4bd5211cfc3e68ad`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DiseaseAfflictionHandler : IAfflictionHandler
public const string SymptomGastrointestinalDistress = "symptom_gastrointestinal_distress";
public const string SymptomFever = "symptom_fever";
public const string SymptomPersistentCough = "symptom_persistent_cough";
public const string SymptomFatigue = "symptom_fatigue";
public const string SymptomBreathlessness = "symptom_breathlessness";
public AfflictionId DefinitionId => new AfflictionId(_definition.id);
public string DiseaseId => _definition.id;
public string DisplayName => _definition.display_name;
public AfflictionEpisodeSnapshot? GetEpisode(Survivors.SurvivorId survivor) {
public IReadOnlyList<SymptomProjection> ProjectSymptoms(Survivors.SurvivorId survivor) {
public bool CouldHaveCondition(Survivors.SurvivorId survivor) {
public string? ValidateTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null) {
public bool ApplyTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null) {
public bool HasResolved(Survivors.SurvivorId survivor) {
public static int RegisterAll(MedicalPipelineCoordinator pipeline, DiseaseSystem disease, DiseaseCatalog catalog) {
```


# Appendix Q.566 — Additional Current Architecture Evidence: `src/Host/DiseaseSaveStore.cs`

### `src/Host/DiseaseSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 49 lines / 2361 bytes.
- SHA-256: `8002e13be286bd28bbe4f3b93dcf88e9ab097fb20218cf2b856da5b9bfa4c12f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DiseaseSaveStore
public const string FileName = "disease_save.json";
public const string SectionName = "disease";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(DiseaseSystemState state) => s_store.CaptureBare(state);
public static DiseaseSystemState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(DiseaseSystemState state) => s_store.CaptureBare(state);
public static DiseaseSystemState? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(DiseaseSystemState state) => s_store.TrySave(state);
public static DiseaseSystemState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(DiseaseSystemState state) => s_store.CapturePersisted(state);
```


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`

### `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 3459 lines / 190529 bytes.
- SHA-256: `d79f9fa53bb0e6eed315b2dbb58e4c2a2f32991272ddd44200da26afa1458719`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CatalogIntegrityReport
public readonly List<string> Errors = new List<string>();
public readonly List<string> Warnings = new List<string>();
public int ErrorCount => Errors.Count;
public bool Clean => Errors.Count == 0;
public int AuthoredIds;
public int ReuseCount;
public void Error(string message) {
public void Warn(string message) {
public static class CatalogIntegrityValidator
public static readonly string[] IdPrefixes = {
public static readonly string[] DefinitionKeys = {
public static readonly string[] ReferenceKeys = {
public static readonly string[] RangeKeys = { "minDay", "maxDay", "MinDay", "min_day" };
public static readonly string[] VocabularyKeys = {
public static readonly string[] KnownRuntimeIds = {
public static readonly string[] PrefixPatternKeys = {
public readonly Dictionary<string, List<string>> Registry = new Dictionary<string, List<string>>(StringComparer.Ordinal);
public readonly List<Ref> PendingRefs = new List<Ref>();
public readonly Dictionary<string, RangeMemoEntry> RangeMemo = new Dictionary<string, RangeMemoEntry>(StringComparer.Ordinal);
public CatalogIntegrityReport Report;
public string File;
public int Authored;
public int Reuse;
public string Value;
public string Path;
public bool Strict;
public string? EntityContext;
public static CatalogIntegrityReport Validate(string dataDirectory, IFileIO files) => Validate(dataDirectory, files, SearchOption.TopDirectoryOnly);
public static CatalogIntegrityReport Validate(string dataDirectory, IFileIO files, SearchOption searchOption) {
public static void ValidateNightWatchOperationsCatalog( string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateShelterOperationsCatalogs( string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateDifficultyPresetCatalog( string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateVehicleArmorGradeCatalog( string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public int? Min;
public int? Max;
public static void ValidateDistressSignalStages(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateTradeEmbargoRules(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateRegionalPriceAtlas(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateCommitments(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateWildlifeTrappingCatalog(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
```


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Economy/BlackMarketSettlementService.cs`

### `Assets/Ashfall.Core/Economy/BlackMarketSettlementService.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 372 lines / 19874 bytes.
- SHA-256: `e8914870d671c1b6a02b3ff9abdc2140ef8861e044d437ed5a187262749cf2fe`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public readonly struct BlackMarketActionPreview
public readonly bool IsAvailable;
public readonly string ActionId;
public readonly string SyndicateId;
public readonly string EntryId;
public readonly string DebtId;
public readonly string ItemId;
public readonly int Quantity;
public readonly long SettlementUnits;
public readonly int DueDay;
public readonly string ReasonId;
public readonly string Message;
public readonly struct BlackMarketActionResult
public readonly bool Success;
public readonly string ActionId;
public readonly string SyndicateId;
public readonly string EntryId;
public readonly string DebtId;
public readonly string ItemId;
public readonly int Quantity;
public readonly long WalletDelta;
public readonly int InventoryDelta;
public readonly long SettlementUnits;
public readonly int DueDay;
public readonly string ReasonId;
public readonly string Message;
public sealed class BlackMarketSettlementService
public const string BuyAction = "buy";
public const string SellAction = "sell";
public const string LoanAction = "take_loan";
public const string RepayAction = "repay";
public long WalletValue => _wallet.Value;
public int InventoryCount(string itemId) =>
public BlackMarketActionPreview PreviewBuy(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketActionResult Buy(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketActionPreview PreviewSell(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketActionResult Sell(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketActionPreview PreviewLoan(string syndicateId, long units, int day, int durationDays) {
public BlackMarketActionResult TakeLoan(string syndicateId, long units, int day, int durationDays) {
public BlackMarketActionPreview PreviewRepay(string debtId, long units, int day) {
public BlackMarketActionResult Repay(string debtId, long units, int day) {
public static string ReasonText(string reasonId) {
```


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/ExpansionMasterSession.cs`

### `Assets/Ashfall.Core/ExpansionMasterSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 212 lines / 10386 bytes.
- SHA-256: `568760f21b08597c7cd360ea5f147c07feebc84edac5a7ca2f20addfd7545f0b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpansionMasterSession
public HoldfastSession Holdfast { get; }
public DutyRosterSystem DutyRoster { get; }
public DutyRosterCatalog DutyRosterData { get; }
public LocationLayoutSystem StandingRecord { get; }
public CrossingSession Crossing { get; }
public SimClock Clock { get; }
public ILog Log { get; }
public SilentFoundrySystem SilentFoundry { get; }
public SilentFoundryCatalog FoundryData { get; }
public DiseaseSystem Disease { get; }
public DiseaseCatalog DiseaseData { get; }
public bool AllExpansionsActive =>
public static ExpansionMasterSession Load(string dataDirectory, int seed =808, ILog? log = null) {
public void TickDaily(WeatherKind weather, float outdoorTemp, List<DutyRosterOccupant>? homeOccupants = null, IReadOnlyList<string>? diseaseCandidates = null) {
public static HeadlessReport RunAllSelfTests(string? dataDirectory = null, ILog? log = null) {
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/DiseaseProtocolHandler.cs`

### `Assets/Ashfall.Core/Medical/DiseaseProtocolHandler.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 140 lines / 6019 bytes.
- SHA-256: `3da3d99ac11d1ffbb386f6a137d050a9bf4037b4e5fd4a6241e81fe87605fa96`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public interface IMedicalProtocolHandler
public sealed class DiseaseProtocolHandler : IMedicalProtocolHandler
public string ProtocolId => _protocolId;
public string DisplayName => _displayName;
public IReadOnlyDictionary<string, int> ItemCosts => _costs;
public string? Validate() {
public bool Apply() {
public static int RegisterAll(MedicalPipelineCoordinator pipeline, DiseaseSystem disease, Func<int>? dayProvider = null) {
```


# Appendix Q.571 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Defense/DefenseSystem.cs`

### `Assets/Ashfall.Core/Defense/DefenseSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 570 lines / 26116 bytes.
- SHA-256: `7390ef59dc11e96f793658fc2f2fa6ea9f3153ebd4f3642f7adf782f6c6f1f27`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DefenseTrapDefinition
public string id { get; set; } = string.Empty;              // trap_*
public string display_name { get; set; } = string.Empty;
public string defense_type { get; set; } = "snare";          // snare | deadfall | capture_pit | spike_border
public int base_strength { get; set; } = 1;                  // raiders neutralized when sprung
public int max_hp { get; set; } = 60;
public float activation_chance { get; set; } = 0.75f;
public float capture_chance { get; set; }                    // 0..1, only for incapacitating traps
public bool concealed { get; set; }
public string placement_tag { get; set; } = "perimeter";
public Dictionary<string, int> build_costs { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
public Dictionary<string, int> reset_costs { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
public Dictionary<string, int> repair_costs { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
public List<string> tags { get; set; } = new List<string>();
public sealed class TrapInstallationState
public string installation_id { get; set; } = string.Empty;
public string trap_id { get; set; } = string.Empty;
public string placement_id { get; set; } = string.Empty;
public int current_hp { get; set; }
public int max_hp { get; set; }
public bool armed { get; set; } = true;
public bool sprung { get; set; }
public bool broken { get; set; }
public string? manned_by { get; set; }                       // optional survivor id
public int last_activation_day { get; set; } = -1;
public int total_activations { get; set; }
public int total_captures { get; set; }
public sealed class DefenseActivationRecord
public int day { get; set; }
public string installation_id { get; set; } = string.Empty;
public string defense_type { get; set; } = string.Empty;
public string target { get; set; } = "raider";               // game abstraction, never a spawn coordinate
public string outcome { get; set; } = "";                    // sprung_captured | sprung_killed | missed | broken
public int raiders_neutralized { get; set; }
public sealed class DefenseSystemState
public string system_id { get; set; } = "settlement_defenses";
public int schema_version { get; set; } = 1;
public int last_tick_day { get; set; }
public List<TrapInstallationState> installations { get; set; } = new List<TrapInstallationState>();
public List<DefenseActivationRecord> raid_log { get; set; } = new List<DefenseActivationRecord>();
public sealed class PerimeterStrengthBreakdown
public float Walls;
public int Traps;
public int Turrets;
public int Power;             // powered emplacements count (0 if grid query absent)
public int Manning;           // armed traps with an assigned survivor
public float DamagePenalties; // HP-integrity penalties across installations
public int Total => Math.Max(0, (int)Math.Round(Walls + Traps + Turrets + Power + Manning - DamagePenalties));
public sealed class DefenseEngagementResult
public int Day;
public int InitialRaiderStrength;
public int RaidersNeutralizedByTraps;
public int RaidersCaptured;
public AssaultSimulationResult? PerimeterResult;   // null when no emplacements engaged
public int RemainingRaiders;
public bool Repelled;
public bool Breached;
public List<DefenseActivationRecord> Records { get; } = new List<DefenseActivationRecord>();
public sealed class DefenseSystem
public const string SystemId = "settlement_defenses";
public const int RaidLogCapacity = 50;
public const int TrapSelfDamagePerSpring = 10;
public event Action<string, string>? OnTrapSprung;            // installationId, trapDefinitionId
public event Action<TrapInstallationState>? OnTrapBroken;
public event Action<int, int>? OnRaiderCaptured;              // day, count — host hands off to PrisonerSystem
public event Action<DefenseEngagementResult>? OnRaidResolved;
public string SaveId => SystemId;
public DefenseSystemState State => _state;
public IReadOnlyList<DefenseTrapDefinition> TrapDefinitions => _trapDefs;
public IReadOnlyList<TrapInstallationState> Installations => _state.installations;
public IReadOnlyList<DefenseActivationRecord> RaidLog => _state.raid_log;
public DefenseTrapDefinition? FindTrapDefinition(string trapId) =>
public TrapInstallationState? FindInstallation(string installationId) {
public bool CanInstall(string trapId) => FindTrapDefinition(trapId) != null;
public bool InstallTrap( string trapId, string placementId, Func<string, int, bool> consumeItems) {
public bool CanResetTrap(string installationId) {
public bool TryResetTrap(string installationId, Func<string, int, bool> consumeItems) {
public bool CanRepairTrap(string installationId) {
public bool TryRepairTrap(string installationId, Func<string, int, bool> consumeItems) {
public void AssignManning(string installationId, string? survivorId) {
public PerimeterStrengthBreakdown CalculatePerimeterStrength( PerimeterDefenseSystem? perimeter, Func<string, bool>? isEmplacementPowered = null) {
public DefenseEngagementResult ResolvePreCombatRaid( int day, int raiderStrength, bool isNight, PerimeterDefenseSystem? perimeter, Func<string, bool>? isEmplacementPowered,
public void TickDay(int day) {
public DefenseSystemState CaptureState() {
public void RestoreState(DefenseSystemState? state) {
public sealed class TrapCatalogContainer
public int schema_version { get; set; } = 1;
public List<DefenseTrapDefinition> traps { get; set; } = new List<DefenseTrapDefinition>();
public static class TrapCatalogLoader
public const string DefaultFileName = "defenses.json";
public static List<DefenseTrapDefinition> Load( string dataDir, Ashfall.Core.IFileIO? fileIO = null, Ashfall.Core.IJsonSerializer? json = null) {
public static List<string> Validate(IEnumerable<DefenseTrapDefinition>? traps) {
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.


# Appendix S — Quality Assurance Pass Record: Plan 36

This record is part of the planning artifact, not a fresh runtime test result.

## Pass 1 — content and premise accuracy
- Content pass replaced the missing-catalog premise with the current 10/15/6 catalog.
- The historical baseline is separated from the current source/data/test authority.
- Current row counts and owner boundaries are stated without using count as a quality proxy.

## Pass 2 — integration architecture
- Integration pass traced setup/check/butcher/repair and canonical effect routing.
- Core, data, host, UI, save, event and test seams are named with current paths.
- The plan does not authorize a parallel save section, catalog, manager or host cache.

## Final precision and reaccuracy pass
- Precision pass protects seeded replay, legacy normalization and disease/dose ownership.
- Every embedded current-file hash, focused runner command and master-authority reference is rechecked.
- Any proposed future seam is labeled as requiring a separate claim and premise verification.
