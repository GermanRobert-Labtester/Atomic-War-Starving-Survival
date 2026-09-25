# Plan 102 — Foundry Accords, Treaty Consequences and Signing Authority

> **Rebuild status:** COMPLETE 18-TREATY CATALOG — LIVE FOUNDRY CONSUMER AND REACHABILITY REQUIRE PRECISION
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-5`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round5-2026-09-25`
>
> **Current-evidence date:** 2026-09-25
>
> **Authority order:** live source and data → `AGENTS.md` → `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` → plan ledgers → this document.
>
> **Authority checksum:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
>
> **Length policy:** 150k–170k is the first quality checkpoint; 250k is an evidence-backed depth target, not a ceiling. The plan may exceed 250k when verified current architecture and evidence justify it, and it must stop rather than pad when that evidence is exhausted.

## 0. Integrity Statement and Plan Status

This file replaces unclaimed generated sections that mixed current evidence, fictional APIs, and unsupported save claims. It is a planning and architecture artifact only. It authorizes no production, data, test, save, generated-index, or UI edits. Every path labeled current must exist at rebuild time. Any future `CREATE` proposal is explicitly hypothetical and belongs to a later, separately claimed implementation package.

The rebuild follows four passes: content/current-reality first; integration framework second; accuracy and contradiction removal third; independent precision and handoff review fourth. Character count is recorded by external verification, not embedded recursively in the document.

# 1. Objective

- The catalog has 18 treaty rows, including the original resource/logistics charters and later saltworks, membrane, coal, apprentice, crisis and incident-book agreements.
- SilentFoundryCatalogLoader, SilentFoundrySystem, SilentFoundryConsequencePolicyCatalog, ExpansionMasterSession and SilentFoundryHostSession are the current implementation chain.
- Treaty prose is authored content; allocation, compliance, penalties, labor and production consequences belong to their existing Foundry/economy owners.

**Bounded outcome:** Retire the obsolete 4-to-10 data-growth objective. The current foundry_accords.json contains 18 treaties, the current test pins all 18, and the loader, ratification-day reader, treaty compliance state, and consequence-policy bridge already exist. The remaining plan is a current-owner audit: preserve one treaty authority, prove which treaties are actually consumed, and add no parallel diplomacy system.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- foundry_accords.json is schema version 1 and currently defines 18 treaties.
- FoundryAccordExpansionTests pins the 18-row count, the ten Silent Foundry signatory rows, signatory validity, ranges, tags, day bands and consequence-policy references.
- SilentFoundryTypes owns treaty IDs, SilentFoundryCatalog loads accord data, and SilentFoundrySystem owns production/compliance state.
- The host binds accord data and ratification days through ExpansionMasterSession/SilentFoundryHostSession; the plan must verify the exact live path before claiming a new treaty feature.

**Master-authority sections applied to this rebase:**

- Part II factory protocol: establish current reality, reject duplicate authority, and name one safe extension seam before design.
- Part II continuity checklist: data presence, a loader, a host route, a player-visible outcome, and persistence are separate proofs.
- Part III cluster map: preserve the current Core owner and route cross-system effects through typed facts rather than panel copies.
- Part VI Multi-Session Growth Protocol: 250k is an evidence-backed depth target, not a mandate to manufacture prose or row count.
- Live source/data authority outranks this plan; a future audit that contradicts a current declaration returns the package to STALE_PLAN.
- Anti-padding rule: preserve completed work as maintenance scope and spend detail only on proven residual gaps.
- Part II current-reality rule: JSON presence, a loader, a host route, a player-visible outcome, and persistence are separate proofs.
- Part III one-authority rule: extend the existing owner and route typed facts through it; do not create a second ledger, save store, registry, simulation, or panel cache.
- Part VI replay rule: any new randomness must use the existing seeded campaign stream and stable ordinal ordering; no System.Random or wall-clock decision path.
- Part VI save rule: a new mutable field is incomplete until CaptureState, RestoreState, old-save defaults, and checksum migration are specified.
- Part VII UI rule: presentation projects owner state and routes real commands; it never becomes a gameplay authority or a fake operational route.
- Part VIII quality rule: the 150k–170k band is an initial completeness checkpoint; 250k is an evidence-backed depth target, not a ceiling or a reason to pad.
- Live source/data evidence outranks the original plan. If a future audit contradicts a declaration here, the package returns to STALE_PLAN rather than reviving an obsolete API.
- C11 Foundry/economy cluster: treaty prose, production allocations, faction standing and consequence state have separate owners.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the old count target with an 18-row treaty census and owner/reachability matrix.
- Map treaty fields to current consumers: signatory identity, ratification day, allocation, compliance, consequence policy and journal/UI projection.
- Treat authored legal prose as narrative unless an existing contract explicitly executes it.
- Add no treaty rows, no second treaty save and no parallel faction treaty manager.

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
| treaty JSON parsing, lookup and ratification-day loading | SilentFoundryCatalogLoader | `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs` | Static treaty catalog owner. |
| foundry production, treaty compliance, allocations and current state | SilentFoundrySystem | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` | Sole Foundry state authority. |
| treaty breach consequence mapping | SilentFoundryConsequencePolicyCatalog | `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs` | Existing consequence policy boundary. |
| catalog and Foundry composition | ExpansionMasterSession | `Assets/Ashfall.Core/ExpansionMasterSession.cs` | Composes the existing owners. |
| host adapter and player-facing Foundry operations | SilentFoundryHostSession | `src/Foundry/SilentFoundryHostSession.cs` | Presentation and command adapter only. |
| current treaty catalog and reference contract | FoundryAccordExpansionTests | `Ashfall.Core.Tests/FoundryAccordExpansionTests.cs` | Focused executable evidence, not a new runtime owner. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Foundry Accords, Treaty Consequences and Signing Authority
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ SilentFoundryCatalogLoader
│   treaty JSON parsing, lookup and ratification-day loading
│ SilentFoundrySystem
│   foundry production, treaty compliance, allocations and current state
│ SilentFoundryConsequencePolicyCatalog
│   treaty breach consequence mapping
│ ExpansionMasterSession
│   catalog and Foundry composition
│ SilentFoundryHostSession
│   host adapter and player-facing Foundry operations
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

1. **Preserve current state ownership.** SilentFoundryCatalogLoader owns treaty JSON parsing, lookup and ratification-day loading: Static treaty catalog owner.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| treaty JSON parsing, lookup and ratification-day loading | SilentFoundryCatalogLoader | `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs` | Static treaty catalog owner. |
| foundry production, treaty compliance, allocations and current state | SilentFoundrySystem | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` | Sole Foundry state authority. |
| treaty breach consequence mapping | SilentFoundryConsequencePolicyCatalog | `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs` | Existing consequence policy boundary. |
| catalog and Foundry composition | ExpansionMasterSession | `Assets/Ashfall.Core/ExpansionMasterSession.cs` | Composes the existing owners. |
| host adapter and player-facing Foundry operations | SilentFoundryHostSession | `src/Foundry/SilentFoundryHostSession.cs` | Presentation and command adapter only. |
| current treaty catalog and reference contract | FoundryAccordExpansionTests | `Ashfall.Core.Tests/FoundryAccordExpansionTests.cs` | Focused executable evidence, not a new runtime owner. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load foundry_accords.json
2. validate treaty IDs and signatory references
3. load ratification days and consequence policies
4. read current Foundry compliance/allocation state
5. project only the treaties consumed by the current owner
6. present through the existing Foundry route
7. capture existing Foundry state only

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Treaty rows are immutable authored definitions.
- Foundry compliance and allocation state remain in SilentFoundryState and its current save path.
- A treaty is not a faction-wide reputation ledger and cannot mutate FactionStanceEngine directly.
- Ratifiation day and consequence policy are references consumed by current systems, not a second calendar.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Every signatory must resolve through canonical faction identity.
- A missing consequence policy must fail closed and leave compliance unchanged.
- A treaty row cannot grant water, power, tariffs or penalties without the owning Foundry command.
- The same catalog, state and day produce the same treaty projection.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- foundry_accords.json is the sole treaty catalog.
- Foundry recipes, power, water, faction and consequence catalogs remain independent authorities.
- No duplicate treaty, treaty-article, or compliance catalog is justified.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use SilentFoundryState/ExpansionHubSave and current Foundry save migrations.
- No Plan-102 save section is required for catalog-only changes.
- Any future allocation field must be added to the existing state with old-save defaults and checksum tests.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Treaty enumeration uses catalog order or stable treaty ID order.
- Foundry RNG remains the existing seeded stream.
- Ratification-day lookup and compliance projection are pure and host-independent.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Current Foundry events expose production/compliance facts.
- A treaty breach may feed the existing consequence policy only through its named owner.
- UI refresh is read-only and must not award or penalize a faction as a side effect.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Foundry/SilentFoundryHostSession.cs
- src/Host/ExpansionHostSession.cs
- src/UI/SilentFoundryPanel.cs
- src/Main.UiTests.SilentFoundry.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Treaty language is fictional, restrained and grounded in water, power, labor and maintenance.
- Legalistic tone is acceptable as authored record voice, not as a real political system.
- A breach consequence must identify the actual affected owner and remain proportionate.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A treaty signatory references a removed faction. | SilentFoundryCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A static tariff field silently changes market prices. | SilentFoundrySystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A consequence policy applies twice after restore. | SilentFoundryConsequencePolicyCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A panel shows a treaty as active while the Foundry owner has no state. | ExpansionMasterSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new treaty save section duplicates Foundry state. | SilentFoundryHostSession | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/FoundryAccordExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/FoundryTreatyConsequenceExpansionTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/SilentFoundrySystemTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — treaty census | Read all 18 rows and classify every field by static, owner-consumed or presentation-only use. | No count target survives as a false requirement. | No production path until the owning implementation package is separately claimed. |
| 1 — owner and reachability trace | Follow catalog, host, consequence and current save paths. | Every visible treaty field has one source. | No production path until the owning implementation package is separately claimed. |
| 2 — failure and replay audit | Check missing references, breach ordering, restore and same-seed behavior. | No duplicate consequence or hidden reputation write. | No production path until the owning implementation package is separately claimed. |
| 3 — UI and content polish | Audit current Foundry surface, labels, focus and truthful unavailable states. | Only proven gaps become future implementation work. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/foundry_accords.json | READ ONLY; MODIFY only for a proven reference/content defect | 18 treaty definitions |
| Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs | READ ONLY | Catalog loader |
| Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs | READ ONLY | Compliance/production owner |
| src/Foundry/SilentFoundryHostSession.cs | READ ONLY | Host adapter |
| src/UI/SilentFoundryPanel.cs | READ ONLY | Presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Creating a second treaty authority. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Executing legal prose as hidden gameplay. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Changing old save checksums for a catalog-only edit. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Claiming all 18 treaties are reachable without caller evidence. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Double-applying breach consequences. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new treaty rows.
- No new diplomacy manager.
- No direct faction trust writes.
- No production/data/test/UI changes in this planning package.

# 23. Rollback and Recovery

- Revert the planning artifact.
- Future read-only treaty projections can be removed without state migration.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- All 18 current rows and their references are documented.
- Live Foundry, consequence, save and host owners are named.
- No static treaty field is described as an executable effect without evidence.
- Focused commands and replay/failure contracts are explicit.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the old count target with an 18-row treaty census and owner/reachability matrix.
- Map treaty fields to current consumers: signatory identity, ratification day, allocation, compliance, consequence policy and journal/UI projection.
- Treat authored legal prose as narrative unless an existing contract explicitly executes it.
- Add no treaty rows, no second treaty save and no parallel faction treaty manager.

## MUST NOT DO

- No new treaty rows.
- No new diplomacy manager.
- No direct faction trust writes.
- No production/data/test/UI changes in this planning package.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/FoundryAccordExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/FoundryTreatyConsequenceExpansionTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/SilentFoundrySystemTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — treaty census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: treaty JSON parsing, lookup and ratification-day loading → SilentFoundryCatalogLoader; foundry production, treaty compliance, allocations and current state → SilentFoundrySystem; treaty breach consequence mapping → SilentFoundryConsequencePolicyCatalog; catalog and Foundry composition → ExpansionMasterSession; host adapter and player-facing Foundry operations → SilentFoundryHostSession; current treaty catalog and reference contract → FoundryAccordExpansionTests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 102.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 102 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by SilentFoundryCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 303 lines / 12451 bytes.
- SHA-256: `f1630ce8db3a4c544cd1889159df443cbc87fa346a587cf679f5858b3021a33a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FoundryIngredientEntry
public string item_id = string.Empty;
public int amount = 1;
public sealed class FoundryProductEntry
public string product_id = string.Empty;
public string display_name = string.Empty;
public string category = string.Empty;
public string result_item_id = string.Empty;
public int result_amount = 1;
public List<FoundryIngredientEntry> ingredients = new List<FoundryIngredientEntry>();
public float labor_hours = 0f;
public float cast_hours = 0f;
public int fuel_units = 0;
public int water_litres = 0;
public float skill_target = 0.5f;
public float quality_target = 70f;
public string treaty_id = string.Empty;
public int quota_amount = 0;
public string sink = string.Empty;
public string notes = string.Empty;
public string[] tags = Array.Empty<string>();
public sealed class FoundryProductionFile
public int schema_version = 1;
public string collection_id = string.Empty;
public List<FoundryProductEntry> products = new List<FoundryProductEntry>();
public sealed class FoundryFactionRelation
public string faction_id = string.Empty;
public string stance = string.Empty;   // ally | trade_partner | rival | internal
public string notes = string.Empty;
public sealed class FoundryFactionEntry
public string faction_id = string.Empty;
public string display_name = string.Empty;
public string short_name = string.Empty;
public string identity = string.Empty;
public string icon_path = string.Empty;
public string[] internal_divisions = Array.Empty<string>();
public List<FoundryFactionRelation> relationships = new List<FoundryFactionRelation>();
public string[] tags = Array.Empty<string>();
public static class SilentFoundryCatalogLoader
public const string ProductionFileName = "foundry_production.json";
public const string FactionFileName = "foundry_faction.json";
public const string AccordsFileName = "foundry_accords.json";
public static Dictionary<string, int> LoadAccordRatificationDays( string dataDirectory, IFileIO? files = null, IJsonSerializer? serializer = null) {
public static FoundryProductionFile LoadProduction( string dataDirectory, IFileIO? files = null, IJsonSerializer? serializer = null) {
public static FoundryFactionEntry? LoadFaction( string dataDirectory, IFileIO? files = null, IJsonSerializer? serializer = null) {
public sealed class SilentFoundryCatalog
public FoundryFactionEntry Faction { get; private set; }
public IReadOnlyList<FoundryProductEntry> AllProducts => _products;
public int ProductCount => _products.Count;
public void Load(FoundryProductionFile production, FoundryFactionEntry faction) {
public void MergeHeavyRecipes(IEnumerable<FoundryProductEntry> heavyProducts) {
public void MergeGlassworksRecipes(IEnumerable<FoundryProductEntry> glassProducts) {
public FoundryProductEntry? GetProduct(string productId) {
public List<FoundryProductEntry> GetByCategory(string category) {
public List<FoundryProductEntry> GetQuotaProducts() {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 689 lines / 34425 bytes.
- SHA-256: `0f779f80dada6c90e5cb5bfe305b5a9cfe253a6d12036b85044329a3ffff2caa`.
- Architecture signals: seeded references=6; save/restore symbols=0; typed event declarations=20; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=1; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed partial class SilentFoundrySystem
public const int DefaultSeed = 1009;
public const int MaxWorkers = 8;            // room_bp_11 max_dweller_capacity
public const float BlueprintBasePowerKw = 45f;
public const float BlueprintWaterFlowLpm = 40f;
public const string EventUnlocked = "silent_foundry_unlocked";
public const string EventRepairStarted = "silent_foundry_repair_started";
public const string EventRepaired = "silent_foundry_repaired";
public const string EventMaintenanceDue = "silent_foundry_maintenance_due";
public const string EventHeatPrepared = "silent_foundry_heat_prepared";
public const string EventHeatStarted = "silent_foundry_heat_started";
public const string EventHeatCompleted = "silent_foundry_heat_completed";
public const string EventCastCompleted = "silent_foundry_cast_completed";
public const string EventCastFailed = "silent_foundry_cast_failed";
public const string EventSafetyWarning = "silent_foundry_safety_warning";
public const string EventIncident = "silent_foundry_incident";
public const string EventTreatyQuotaMet = "silent_foundry_treaty_quota_met";
public const string EventTreatyQuotaMissed = "silent_foundry_treaty_quota_missed";
public const string EventConsequenceApplied = "silent_foundry_treaty_consequence_applied";
public const string EventLaborDispute = "silent_foundry_labor_dispute";
public const string EventStrikeStarted = "silent_foundry_strike_started";
public const string EventStrikeResolved = "silent_foundry_strike_resolved";
public const string EventBlueprintReferenced = "silent_foundry_blueprint_referenced";
public const string EventJournalTriggered = "silent_foundry_journal_triggered";
public event Action<SilentFoundryState> OnStateChanged;
public event Action<FoundryProductionRecord> OnProductionCompleted;
public event Action<FoundryFailedCastRecord> OnCastFailed;
public event Action<string> OnSafetyWarning;
public event Action<FoundryIncidentRecord> OnIncident;
public event Action<FoundryTreatyCompliance> OnTreatyQuotaMet;
public event Action<FoundryTreatyCompliance> OnTreatyQuotaMissed;
public event Action<FoundryConsequenceRecord> OnConsequenceApplied;
public event Action<FoundryLaborDispute, int> OnLaborDisputeChanged;
public event Action<FoundryStrikeResolution, int> OnStrikeResolved;
public event Action<FoundryJournalTrigger> OnJournalTriggered;
public event Action<string> OnEventRaised;
public const float StandingMin = -100f;
public const float StandingMax = 100f;
public const float StandingNeutral = 0f;
public void BindCatalog(SilentFoundryCatalog catalog, int maintenanceCycleDaysFromBlueprint) {
public void BindTreaties(IReadOnlyDictionary<string, int> ratificationDaysById) {
public void BindConsequencePolicy(SilentFoundryConsequencePolicyCatalog policy) {
public void BindInventory( Func<string, int> getCount, Func<string, int, bool> canAdd, Action<string, int> addItem, Action<string, int> consume) {
public SilentFoundryState State => _state;
public SilentFoundryCatalog Catalog => _catalog;
public bool IsUnlocked => _state.unlocked;
public FoundryHeatStage HeatStage => _state.heatStage;
public FoundryLaborDispute LaborDispute => _state.laborDispute;
public bool IsMaintenanceOverdue => _state.daysSinceMaintenance > _state.maintenanceCycleDays;
public int DaysOverdue => Math.Max(0, _state.daysSinceMaintenance - _state.maintenanceCycleDays);
public int OverdueCycles => _state.maintenanceCycleDays > 0
public float GetComponentCondition(FoundryFacilityComponent component) {
public float AverageFacilityCondition() {
public bool IsJournalTriggered(string templateId) =>
public FoundryTreatyCompliance? GetTreatyCompliance(string treatyId) {
public IReadOnlyList<FoundryProductionRecord> CompletedProduction => _state.completed;
public IReadOnlyList<FoundryFailedCastRecord> FailedCasts => _state.failed;
public IReadOnlyList<FoundryIncidentRecord> Incidents => _state.incidents;
public int TotalProductionCount => _state.completed.Count;
public int TotalFailedCount => _state.failed.Count;
public float CumulativeStress => _state.cumulativeStress;
public float CumulativeHope => _state.cumulativeHope;
public bool IsHeatActive => HeatStage != FoundryHeatStage.Idle && HeatStage != FoundryHeatStage.Complete;
public float CurrentPowerDemandKw => HeatStage switch
public float CurrentWasteHeatKw => HeatStage switch
public void SuspendHeat(string reason, int day) {
public float GuildStanding => _consequenceState.guildStanding;
public IReadOnlyList<FoundryConsequenceRecord> AppliedConsequences => _consequenceState.applied;
public bool IsConsequenceApplied(string treatyId, int cycleMarker) => _consequenceState.IsApplied(treatyId, cycleMarker);
public FoundryTreatyOutcome GetTreatyOutcome(string treatyId, int day) {
public bool Unlock(int day) {
public string StartRepair(FoundryFacilityComponent component, int day) {
public string PerformMaintenance(int day) {
public string PrepareSand(int waterLitres) {
public string CompactMold(float skill) {
public void AssessTreatyCompliance(int day) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Foundry/SilentFoundryTypes.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundryTypes.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 282 lines / 10679 bytes.
- SHA-256: `519c0399c514e2bae74c69de64a1644410c1e0647f825381b5ae487219f10d95`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class SilentFoundryIds
public const string ExpansionId = "exp_10_the_silent_foundry";
public const string FactionId = "faction_silent_foundry";
public const string BlueprintRoomId = "room_bp_11_the_silent_foundry_smelter_bay";
public const string JournalFirstHeat = "jrnl_foundry_first_heat";
public const string JournalStrike = "jrnl_foundry_strike";
public const string TreatyBrinePipe = "treaty_brine_pipe_and_iodine_exchange";
public const string TreatyLabourSchedule = "treaty_cluster_labour_schedule";
public const string TreatyRoadIron = "treaty_road_iron_charter";
public const string TreatyClusterCharter = "treaty_the_cluster_charter";
public const string TreatySaltworksAccess = "treaty_saltworks_access";
public const string TreatyMembraneRepair = "treaty_membrane_repair";
public const string TreatyCoalWindow = "treaty_coal_window";
public const string TreatyApprenticeExchange = "treaty_apprentice_exchange";
public const string TreatyCrisisMutualAid = "treaty_crisis_mutual_aid";
public const string TreatyIncidentBook = "treaty_the_incident_book";
public const string ItemScrapMetal = "scrap_metal";
public const string ItemCoal = "coal";
public const string ItemCharcoal = "charcoal";
public const string ItemCleanWater = "clean_water";
public const string ItemFirebrick = "item_foundry_firebrick";
public const string ItemGreenSand = "item_foundry_green_sand";
public const string ItemFlux = "item_foundry_flux";
public const string ItemAlloyAdditive = "item_foundry_alloy_additive";
public const string ItemScrapMechanical = "scrap_mechanical";
public const string ItemCopperWire = "copper_wire_10m_of_10m";
public const string ItemShoringBracket = "item_foundry_shoring_bracket";
public const string CategoryHeavyMetallurgy = "heavy_metallurgy";
public enum FoundryHeatStage
public enum FoundryLaborDispute
public enum FoundryQualityTier
public enum FoundryIncidentSeverity
public enum FoundryFacilityComponent
public enum FoundryStrikeResolution
public sealed class FoundryProductionRecord
public string productId = string.Empty;
public string displayName = string.Empty;
public int amount = 0;
public FoundryQualityTier tier = FoundryQualityTier.Usable;
public int completedDay = 0;
public int workers = 0;
public string purity = FoundryPurityNames.Standard;
public string materialProfileId = string.Empty;
public int craftQualityPermille = 0;
public sealed class FoundryFailedCastRecord
public string productId = string.Empty;
public string displayName = string.Empty;
public string reason = string.Empty;
public int failedDay = 0;
public int materialsLost = 0;
public sealed class FoundryIncidentRecord
public FoundryIncidentSeverity severity = FoundryIncidentSeverity.None;
public int day = 0;
public string summary = string.Empty;
public int workersInjured = 0;
public int downtimeDays = 0;
public sealed class FoundryRepairRecord
public string component = string.Empty;
public int day = 0;
public float conditionBefore = 0f;
public float conditionAfter = 0f;
public sealed class FoundryTreatyCompliance
public string treatyId = string.Empty;
public string obligation = string.Empty;   // brine_pipe_quota | labor_shifts | road_iron_quota | charter_eligibility
public int quotaTotal = 0;
public int quotaFulfilled = 0;
public int quotaDeadlineDay = 0;           // next assessment day
public int lastAssessmentDay = 0;
public int metCount = 0;
public int missedCount = 0;
public bool currentCycleMet = false;
public float standingPenalty = 0f;
public bool constitutionEligible = false;
public sealed class SilentFoundryState
public const int CurrentVersion = 1;
public int stateVersion = CurrentVersion;
public bool unlocked = false;
public int unlockDay = 0;
public float refractoryLining = 100f;
public float hearthTuyeres = 100f;
public float sandBeds = 100f;
public float structuralSupports = 100f;
public float safetyExhaust = 100f;
public int maintenanceCycleDays = 4;       // authored anchor from room_bp_11
public int maintenanceDueDay = 0;
public int daysSinceMaintenance = 0;
public int maintenancePerformed = 0;
public float sandQuality = 65f;
public float sandMoisture = 65f;           // target band ~55..75
public float binderQuality = 60f;
public float patternQuality = 70f;
public float contamination = 5f;           // grows with low-grade charge
public int moldReuseCount = 0;
public float compaction = 70f;
public FoundryHeatStage heatStage = FoundryHeatStage.Idle;
public int heatStartedDay = 0;
public int stageElapsedDays = 0;
public string activeProductId = string.Empty;
public int assignedWorkers = 0;
public float workerSkill = 0.5f;
public float laborAccumulated = 0f;
public float workerExposure = 0f;          // fatigue/exposure units accrued
public int materialsConsumed = 0;
public bool childLaborUsed = false;
public float pendingQuality = 0f;
public List<FoundryProductionRecord> completed = new List<FoundryProductionRecord>();
public List<FoundryFailedCastRecord> failed = new List<FoundryFailedCastRecord>();
public List<FoundryIncidentRecord> incidents = new List<FoundryIncidentRecord>();
public List<FoundryRepairRecord> repairs = new List<FoundryRepairRecord>();
public FoundryLaborDispute laborDispute = FoundryLaborDispute.None;
public int laborDisputeStartedDay = 0;
public int strikeStartedDay = 0;
public bool overtimeFlag = false;
public bool educationConflictFlag = false;
public List<FoundryTreatyCompliance> treatyCompliance = new List<FoundryTreatyCompliance>();
public List<string> triggeredJournals = new List<string>();
public float cumulativeStress = 0f;
public float cumulativeHope = 0f;
public int firstHeatDay = 0;
public int strikeDay = 0;
public string activeMetallurgyRecipeId = string.Empty;
public float metallurgySlag = 0f;              // 0..100 normalized
public int metallurgyBatchesCompleted = 0;
public FoundryForgingSessionState? activeForging = null;
public int rngSeed = 0;
public sealed class FoundryJournalTrigger
public string TemplateId = string.Empty;
public float StressDelta = 0f;
public float HopeEarned = 0f;
public int Day = 0;
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 226 lines / 9433 bytes.
- SHA-256: `1e740631e69095fbd5d3c16a808fefb1345d299a09474d906ba53367848090e2`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum FoundryTreatyOutcome
public sealed class FoundryGoodModifier
public string good_id = string.Empty;
public float demand_delta = 0f;
public string reason = string.Empty;
public sealed class FoundryTreatyConsequencePolicy
public string treaty_id = string.Empty;
public string faction_id = string.Empty;
public string outcome = string.Empty;   // met | missed | violated
public float standing_delta = 0f;
public string reason = string.Empty;
public List<FoundryGoodModifier> market_modifiers = new List<FoundryGoodModifier>();
public sealed class FoundryTreatyConsequenceFile
public int schema_version = 1;
public string collection_id = string.Empty;
public List<FoundryTreatyConsequencePolicy> policies = new List<FoundryTreatyConsequencePolicy>();
public sealed class FoundryConsequenceRecord
public string treatyId = string.Empty;
public FoundryTreatyOutcome outcome = FoundryTreatyOutcome.Pending;
public int appliedDay = 0;
public int cycleMarker = 0;
public float standingDelta = 0f;
public List<FoundryGoodModifier> modifiers = new List<FoundryGoodModifier>();
public string reason = string.Empty;
public sealed class SilentFoundryConsequenceState
public const int CurrentVersion = 1;
public int stateVersion = CurrentVersion;
public List<FoundryConsequenceRecord> applied = new List<FoundryConsequenceRecord>();
public float guildStanding = 0f;
public bool IsApplied(string treatyId, int cycleMarker) {
public static class SilentFoundryConsequenceCatalogLoader
public const string FileName = "foundry_treaty_consequences.json";
public static FoundryTreatyConsequenceFile Load( string dataDirectory, IFileIO? files = null, IJsonSerializer? serializer = null) {
public sealed class SilentFoundryConsequencePolicyCatalog
public IReadOnlyList<string> Errors => _errors;
public bool HasErrors => _errors.Count > 0;
public int PolicyCount => _byKey.Count;
public IReadOnlyCollection<FoundryTreatyConsequencePolicy> AllPolicies => _byKey.Values;
public void Load(FoundryTreatyConsequenceFile file) {
public FoundryTreatyConsequencePolicy? Find(string treatyId, FoundryTreatyOutcome outcome) {
public static bool IsKnownOutcome(string outcome) {
public static readonly string[] KnownOutcomes = { "met", "missed", "violated" };
public static string OutcomeName(FoundryTreatyOutcome outcome) {
```


# Appendix B.06 — Current Code Architecture: `Assets/Ashfall.Core/ExpansionMasterSession.cs`

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


# Appendix B.07 — Current Code Architecture: `src/Foundry/SilentFoundryHostSession.cs`

### `src/Foundry/SilentFoundryHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 650 lines / 31351 bytes.
- SHA-256: `bbbed72dc2e0ebf48a735ed8f9f561cd61fd27bc7635edfd15ade6c688e89299`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=1; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SilentFoundryHostSession
public const int DefaultSeed = 1009;
public SilentFoundrySystem Engine { get; }
public SaltMineExtractionSystem SaltMine { get; }
public SilentFoundryCatalog Catalog { get; }
public ItemCatalog FoundryItems { get; }
public SilentFoundryConsequencePolicyCatalog ConsequencePolicy { get; }
public FactionStanceEngine GuildStanceEngine { get; }
public Ashfall.Core.Shelter.PowerGridSystem? PowerGrid { get; set; }
public Ashfall.Core.ShelterThermalSystem? ThermalSystem { get; set; }
public string LastEvent { get; set; } = string.Empty;
public event Action? StateChanged;
public void BindPowerAndThermal(Ashfall.Core.Shelter.PowerGridSystem? powerGrid, Ashfall.Core.ShelterThermalSystem? thermal) {
public string BeginForging(string outputItemId, int day) {
public string SubmitForgingCommand(Ashfall.Core.Foundry.FoundryForgingCommand command, int day) {
public string CompleteForging(int day) {
public void TickDaily(int day) {
public static SilentFoundryHostSession Create( string dataDir, ExpansionHostSession expansions, InventoryHostSession inventory, JournalSystem? journal = null, MarketSystem? market = null,
public float GuildTrust => Engine.GuildStanding;
public TradeStance GuildStance => GuildStanceEngine.GetStance(SilentFoundryIds.FactionId);
public void SyncGuildStanding() {
public void BindStanceProviders(Func<int> campaignDayProvider, Func<float> partyRadiationProvider, Func<SurvivorsHostSession?> survivorsProvider) {
public string Id { get; }
public string DisplayName { get; }
public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;
public string Unlock(int day) => Engine.Unlock(day) ? "The Silent Foundry is open." : "Already open.";
public string Repair(FoundryFacilityComponent component, int day) => Engine.StartRepair(component, day);
public string Maintain(int day) => Engine.PerformMaintenance(day);
public string PrepareSand(int water) => Engine.PrepareSand(water);
public string CompactMold() => Engine.CompactMold(0.6f);
public string StartHeat(string productId, int workers, float skill, int day) {
public string Tap(int day) => Engine.TapAndCast(day);
public string SetOvertime(bool on) { Engine.SetOvertime(on); return on ? "Overtime ordered." : "Overtime rescinded."; }
public string SetChildLabor(bool on) { Engine.SetChildLaborUsed(on); return on ? "Children sent to the charging floor." : "Children returned to lessons."; }
public string OpenDispute(int day) => Engine.BeginLaborDispute(day);
public string ResolveStrike(FoundryStrikeResolution resolution, int day) => Engine.ResolveStrike(resolution, day);
public string OpenSaltMine(string veinId = "vein_salt_01", string displayName = "Main Salt Vein", int initialWorkers = 2) {
public string OpenSaltMineDemo() => OpenSaltMine();
public string TickSaltMine(int day) {
public string TickSaltMineDemo(int day) => TickSaltMine(day);
public string DeliverSaltTreaty(int day) {
public string DeliverSaltTreatyDemo(int day) => DeliverSaltTreaty(day);
public string SaltMineStatusLine() {
public string StatusLine() {
public sealed class FoundryItemJson
public string? id;
public string? displayName;
public string? description;
public string? type;
public int stackMax = 1;
public float weight;
public float tradeValue;
public float durability;
```


# Appendix B.08 — Current Code Architecture: `src/Host/ExpansionHostSession.cs`

### `src/Host/ExpansionHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 500 lines / 25698 bytes.
- SHA-256: `9809fca6ed766dfd903f6dcc7df79e68f779d539a85bbd8a66066710d570da45`.
- Architecture signals: seeded references=0; save/restore symbols=5; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpansionHostSession
public const int DefaultSeed = 1117; // greenhouse + vouch demo seed
public WaystationSystem Waystation { get; }
public LocationLayoutSystem Layouts { get; }
public LocationMemorySystem Memory { get; }
public SiteEncounterSystem SiteEncounters { get; }
public StandingRecordCatalog RecordQuests { get; }
public VouchAccessSystem Vouch { get; }
public GreenhouseSystem Greenhouse { get; private set; }
public CrossingArbitrationSystem Arbitration { get; }
public LedgerDebtSystem Ledger { get; }
public CrossingQuestSystem CrossingQuests { get; }
public GenerationalSuccessionEngine Generational { get; }
public EpilogueMatrixRuntime Epilogue { get; }
public DutyRosterSystem DutyRoster { get; private set; }
public Ashfall.Core.Foundry.SilentFoundrySystem SilentFoundry { get; private set; }
public Ashfall.Core.Foundry.SilentFoundryCatalog FoundryData { get; private set; }
public Ashfall.Core.Disease.DiseaseSystem Disease { get; private set; }
public Ashfall.Core.Disease.DiseaseCatalog DiseaseData { get; private set; }
public DebtTemplateCatalog? DebtCatalog { get; private set; }
public DebtConsequenceDispatcher? DebtDispatcher { get; private set; }
public FactionEmbargoLedger Embargoes { get; } = new FactionEmbargoLedger();
public void BindDutyRoster(DutyRosterSystem roster) {
public void BindGreenhouse(GreenhouseSystem shared) {
public event Action<CrossingStageNarrativeEvent>? OnCrossingStageNarrative;
public static ExpansionHostSession Create( string dataDirectory, ILog log = null!, Ashfall.Core.Flags.IFlagLedger? consequenceLedger = null) {
public void ShutdownDebtIntegration() {
public override void Dispose() {
public ExpansionHubSave CaptureSave(int simDay, DebtConsequenceBridgeState? debtBridge = null) =>
public void RestoreSave(ExpansionHubSave save, DebtConsequenceHostBridge? debtBridge = null) =>
public void LoadDefaultBackerPool() {
public string ArbitrationLine() {
public string LedgerLine() {
public void UnlockWaystation() => Waystation.Unlock();
public void SetWaystationWintering(bool wintering) => Waystation.SetWintering(wintering);
public bool AssignWaystationWatch(string[] ids) => Waystation.AssignWatch(ids);
public void ResupplyWaystation() => Waystation.Resupply();
public void TickWaystation(bool iceRoadOpen) => Waystation.TickDaily(iceRoadOpen);
public string WaystationLine() {
public void UnlockRecord() {
public bool ArriveAtSite(string parentId) => Layouts.ArriveAtParent(parentId);
public bool EnterSiteRoom(string roomId) => Layouts.EnterRoom(roomId);
public bool InspectSiteRoom(string roomId) => Layouts.InspectRoom(roomId);
public string RoomLine(string parentId, string roomId) {
public string StandingRecordLine() {
public string RecordQuestLine() {
public bool GrantVouch(string npcId) => Vouch.GrantVouch(npcId, isLastResort: false);
public bool BurnVouch() => Vouch.BurnVouch();
public bool SoftenAccess() => Vouch.SoftenAccess();
public string CrossingLine() {
public bool StartCrossingQuest(string questId, int currentDay) => CrossingQuests.StartQuest(questId, currentDay);
public void TickCrossingQuests(int currentDay) => CrossingQuests.TickDaily(currentDay, hasVouchAccess: Vouch.HasAccess);
public int AdvanceCrossingQuestStage(string questId) => CrossingQuests.AdvanceStage(questId);
public bool MakeCrossingChoice(string questId, string choiceId) => CrossingQuests.MakeChoice(questId, choiceId);
public List<CrossingQuestDef> GetAvailableCrossingQuests(int currentDay) => CrossingQuests.GetAvailableQuests(currentDay);
public bool FailCrossingQuest(string questId) => CrossingQuests.FailQuest(questId);
public bool IsCrossingQuestFailed(string questId) => CrossingQuests.IsQuestFailed(questId);
public bool IsCrossingQuestCompleted(string questId) => CrossingQuests.IsQuestCompleted(questId);
public string CrossingQuestLine() {
public void EnsureGreenhousePlots(int count) => Greenhouse.EnsurePlots(count);
public bool PlantGreenhouse(int plotIndex, string seedItemId, int day) => Greenhouse.Plant(plotIndex, seedItemId, day, out _);
public void WaterGreenhouse(int plotIndex, float units) => Greenhouse.Water(plotIndex, units, tainted: false);
public GreenhouseHarvest HarvestGreenhouse(int plotIndex) => Greenhouse.Harvest(plotIndex);
public void TickGreenhouse(int simDay) =>
public string GreenhouseLine() {
public void RegisterGenerationDweller(string dwellerId, int age, int generation = 0) => Generational.RegisterDweller(dwellerId, age, generation);
public string AdvanceGenerationalTime(int days) {
public string FormMentorshipDemo(string mentorId, string apprenticeId, string traitId) {
public string GenerationalLine() {
public GenerationalSuccessionSaveState CaptureGenerationalSave() => Generational.CaptureState();
public void RestoreGenerationalSave(GenerationalSuccessionSaveState state) => Generational.RestoreState(state);
public string GenerateEpilogueNarrativeDemo(EpilogueEvaluationContext ctx) => Epilogue.GenerateEpilogueNarrative(ctx);
```


# Appendix B.09 — Current Code Architecture: `src/UI/SilentFoundryPanel.cs`

### `src/UI/SilentFoundryPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 589 lines / 27918 bytes.
- SHA-256: `84d462c5bad418e6959814aab73cd019e88dc668a9fffc1b1a574dcf803e78ff`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=4; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class SilentFoundryPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<int>? OnProductSelected;
public bool IsBound => _host != null;
public void Bind(SilentFoundryHostSession session, int currentDay) {
public void SetMachineTellCatalog(Ashfall.Core.Shelter.ShelterMachineTellCatalog? catalog) {
public override void _Ready() {
public void RefreshView() {
internal static List<AshfallDataGrid.Row> BuildFixtureRows() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix B.10 — Current Code Architecture: `src/Main.UiTests.SilentFoundry.cs`

### `src/Main.UiTests.SilentFoundry.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 236 lines / 14168 bytes.
- SHA-256: `6c9a3716fcf30f522741de20e2a0e7db4bb6384cfeccf8ef192664303c59f065`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/foundry_accords.json`

### `Assets/StreamingAssets/Data/foundry_accords.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 20524 bytes / 20522 characters.
- SHA-256: `d82bb6e361e13ef12aa6b73d3ad570bc5e18fe45acb8354010597b1ad00bdf53`.
- Root keys: `collection_id`, `schema_version`, `treaties`.

Array-path census (minimum, maximum, observed rows):

```text
treaties: min=18, max=18, observed_paths=1
treaties[].signatory_factions: min=2, max=3, observed_paths=2
treaties[].tags: min=6, max=6, observed_paths=2
```

Representative record fields:

- `demarcated_territory`
- `penalties`
- `power_quota_kw`
- `ratified_day`
- `signatory_factions`
- `tags`
- `tariff_schedule`
- `treaty_articles`
- `treaty_id`
- `treaty_title`
- `water_allocation_lpm`


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/FoundryAccordExpansionTests.cs`

### `Ashfall.Core.Tests/FoundryAccordExpansionTests.cs`

- Current test declarations: Fact=11, Theory=0, InlineData=0.
- File lines: 362; SHA-256: `73613c87e16e2a46a840492ce7dfedd0a89818997867e174ffb5770a5dfbd3a9`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsAllAccordsWithoutErrors
Parity_BaselineFourDistrict8AccordsPreserved
FoundryRoster_ContainsExactlyTheSixNewDependencyReadyAccords
TreatyId_AllIdsAreUniqueAndFollowSnakeCasePrefix
Signatories_AllFactionsAreValidAndNonEmpty
Resources_WaterAndPowerAllocationsAreNonNegativeAndPlausible
LegalText_ArticlesFollowNumberedClausesAndPenaltiesAreEnforceable
Tags_FollowNormalizedVocabularyWithoutSynonymSplits
Timeline_RatificationDaysAreChronologicallyOrdered
Diversity_CatalogSpansResourceLogisticsTerritorialAndGovernanceRoles
ConsequenceSeam_Plan103PoliciesResolveAgainstTheseAccords
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/FoundryTreatyConsequenceExpansionTests.cs`

### `Ashfall.Core.Tests/FoundryTreatyConsequenceExpansionTests.cs`

- Current test declarations: Fact=14, Theory=0, InlineData=0.
- File lines: 326; SHA-256: `83ec8f1d815bef517019f71291388c35121d792b59cb9b048fe3561e42d619f5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsExactlyFifteenPoliciesWithoutErrors
ReferenceIntegrity_AllTreatyIdsResolveInFoundryAccords
ReferenceIntegrity_AllFactionIdsAreSignatoriesOfReferencedTreaty
OutcomeValidation_AllPoliciesUseCanonicalOutcomeVocabulary
MechanicalEffectValidation_AllMarketGoodModifiersResolveInEconomyGoods
PolicyUniqueness_NoDuplicateTreatyAndOutcomeKeys
CoverageMatrix_EightTreatiesCoveredWithRationalDistribution
RepresentativePolicy_SaltworksMetAndViolated
RepresentativePolicy_CoalWindowMetAndMissed
RepresentativePolicy_MembraneRepairMetAndViolated
RepresentativePolicy_CrisisMutualAidMetAndViolated
RepresentativePolicy_IncidentBookMet
Idempotency_RecordStateTracksAssessmentDayCycleKey
Balance_StandingDeltasAreBoundedAndProportional
```


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/SilentFoundrySystemTests.cs`

### `Ashfall.Core.Tests/SilentFoundrySystemTests.cs`

- Current test declarations: Fact=35, Theory=0, InlineData=0.
- File lines: 916; SHA-256: `4f3c37bec6f3232e20309bddda99525206c3c11d3d34f775792865f5118adfd8`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Identity_ExactIdsResolve
Catalog_LoadsAllAuthoredProductsAndFaction
Catalog_QuotaProductsMapToExactTreaties
JournalTemplates_LoadWithExpansionIsolation
JournalDeltas_MatchAuthoredTemplate
Blueprint_ResolvesAndAnchorsMaintenanceCycle
Treaties_GuildIsExactSignatoryOfTenFoundryTreaties
Unlock_IsIdempotentAndRaisesOnce
Repair_ConsumesFirebrickAndRestoresComponent
Maintenance_FourDayCycleAndOverdueConsequences
SandPrep_ConsumesSandAndWaterAndImprovesBed
SandPrep_BlocksWithoutSandOrWater
Production_StartValidatesChargeAndConsumesResources
Production_MissingChargeGivesVisibleReason
Production_FirstHeatCompletesAndTriggersJournalOnce
Production_SecondHeatDoesNotRetriggerJournal
Production_UntappedHeatBurnsOutAndRecordsFailure
Production_QualityTiersAreDeterministicPerSeed
Safety_WarningsSurfaceBeforeIrreversibleTap
Incident_SameSeedSameOutcome
Incident_IsNeverHiddenAndLeavesARecord
Incident_WellMaintainedFurnaceNeverIncidents
Treaty_RailQuotaMetOnDeadline
Treaty_RailQuotaMissedOnDeadline
Treaty_LaborShiftViolationWhenStrikeOrOvertime
Treaty_RatificationDaysAreNotAssessedBeforeRatification
Strike_FatigueAloneDoesNotTriggerDispute
Strike_ProductionPressurePlusShiftGrievanceTriggersAndEscalates
Strike_ResolutionIsTypedAndOnceOnly
Save_RoundTripPreservesAllFoundryState
Save_ActiveFurnaceSurvivesRoundTrip
Save_MissingFoundryStateDefaultsSafely
Save_ChecksumStableAcrossHostSerializers
Save_ExpansionHubEnvelopeRoundTripsWithMigration
Events_EmitExactlyOncePerOutcome
```


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 303 lines / 12451 bytes.
- SHA-256: `f1630ce8db3a4c544cd1889159df443cbc87fa346a587cf679f5858b3021a33a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FoundryIngredientEntry
public string item_id = string.Empty;
public int amount = 1;
public sealed class FoundryProductEntry
public string product_id = string.Empty;
public string display_name = string.Empty;
public string category = string.Empty;
public string result_item_id = string.Empty;
public int result_amount = 1;
public List<FoundryIngredientEntry> ingredients = new List<FoundryIngredientEntry>();
public float labor_hours = 0f;
public float cast_hours = 0f;
public int fuel_units = 0;
public int water_litres = 0;
public float skill_target = 0.5f;
public float quality_target = 70f;
public string treaty_id = string.Empty;
public int quota_amount = 0;
public string sink = string.Empty;
public string notes = string.Empty;
public string[] tags = Array.Empty<string>();
public sealed class FoundryProductionFile
public int schema_version = 1;
public string collection_id = string.Empty;
public List<FoundryProductEntry> products = new List<FoundryProductEntry>();
public sealed class FoundryFactionRelation
public string faction_id = string.Empty;
public string stance = string.Empty;   // ally | trade_partner | rival | internal
public string notes = string.Empty;
public sealed class FoundryFactionEntry
public string faction_id = string.Empty;
public string display_name = string.Empty;
public string short_name = string.Empty;
public string identity = string.Empty;
public string icon_path = string.Empty;
public string[] internal_divisions = Array.Empty<string>();
public List<FoundryFactionRelation> relationships = new List<FoundryFactionRelation>();
public string[] tags = Array.Empty<string>();
public static class SilentFoundryCatalogLoader
public const string ProductionFileName = "foundry_production.json";
public const string FactionFileName = "foundry_faction.json";
public const string AccordsFileName = "foundry_accords.json";
public static Dictionary<string, int> LoadAccordRatificationDays( string dataDirectory, IFileIO? files = null, IJsonSerializer? serializer = null) {
public static FoundryProductionFile LoadProduction( string dataDirectory, IFileIO? files = null, IJsonSerializer? serializer = null) {
public static FoundryFactionEntry? LoadFaction( string dataDirectory, IFileIO? files = null, IJsonSerializer? serializer = null) {
public sealed class SilentFoundryCatalog
public FoundryFactionEntry Faction { get; private set; }
public IReadOnlyList<FoundryProductEntry> AllProducts => _products;
public int ProductCount => _products.Count;
public void Load(FoundryProductionFile production, FoundryFactionEntry faction) {
public void MergeHeavyRecipes(IEnumerable<FoundryProductEntry> heavyProducts) {
public void MergeGlassworksRecipes(IEnumerable<FoundryProductEntry> glassProducts) {
public FoundryProductEntry? GetProduct(string productId) {
public List<FoundryProductEntry> GetByCategory(string category) {
public List<FoundryProductEntry> GetQuotaProducts() {
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 689 lines / 34425 bytes.
- SHA-256: `0f779f80dada6c90e5cb5bfe305b5a9cfe253a6d12036b85044329a3ffff2caa`.
- Architecture signals: seeded references=6; save/restore symbols=0; typed event declarations=20; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=1; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed partial class SilentFoundrySystem
public const int DefaultSeed = 1009;
public const int MaxWorkers = 8;            // room_bp_11 max_dweller_capacity
public const float BlueprintBasePowerKw = 45f;
public const float BlueprintWaterFlowLpm = 40f;
public const string EventUnlocked = "silent_foundry_unlocked";
public const string EventRepairStarted = "silent_foundry_repair_started";
public const string EventRepaired = "silent_foundry_repaired";
public const string EventMaintenanceDue = "silent_foundry_maintenance_due";
public const string EventHeatPrepared = "silent_foundry_heat_prepared";
public const string EventHeatStarted = "silent_foundry_heat_started";
public const string EventHeatCompleted = "silent_foundry_heat_completed";
public const string EventCastCompleted = "silent_foundry_cast_completed";
public const string EventCastFailed = "silent_foundry_cast_failed";
public const string EventSafetyWarning = "silent_foundry_safety_warning";
public const string EventIncident = "silent_foundry_incident";
public const string EventTreatyQuotaMet = "silent_foundry_treaty_quota_met";
public const string EventTreatyQuotaMissed = "silent_foundry_treaty_quota_missed";
public const string EventConsequenceApplied = "silent_foundry_treaty_consequence_applied";
public const string EventLaborDispute = "silent_foundry_labor_dispute";
public const string EventStrikeStarted = "silent_foundry_strike_started";
public const string EventStrikeResolved = "silent_foundry_strike_resolved";
public const string EventBlueprintReferenced = "silent_foundry_blueprint_referenced";
public const string EventJournalTriggered = "silent_foundry_journal_triggered";
public event Action<SilentFoundryState> OnStateChanged;
public event Action<FoundryProductionRecord> OnProductionCompleted;
public event Action<FoundryFailedCastRecord> OnCastFailed;
public event Action<string> OnSafetyWarning;
public event Action<FoundryIncidentRecord> OnIncident;
public event Action<FoundryTreatyCompliance> OnTreatyQuotaMet;
public event Action<FoundryTreatyCompliance> OnTreatyQuotaMissed;
public event Action<FoundryConsequenceRecord> OnConsequenceApplied;
public event Action<FoundryLaborDispute, int> OnLaborDisputeChanged;
public event Action<FoundryStrikeResolution, int> OnStrikeResolved;
public event Action<FoundryJournalTrigger> OnJournalTriggered;
public event Action<string> OnEventRaised;
public const float StandingMin = -100f;
public const float StandingMax = 100f;
public const float StandingNeutral = 0f;
public void BindCatalog(SilentFoundryCatalog catalog, int maintenanceCycleDaysFromBlueprint) {
public void BindTreaties(IReadOnlyDictionary<string, int> ratificationDaysById) {
public void BindConsequencePolicy(SilentFoundryConsequencePolicyCatalog policy) {
public void BindInventory( Func<string, int> getCount, Func<string, int, bool> canAdd, Action<string, int> addItem, Action<string, int> consume) {
public SilentFoundryState State => _state;
public SilentFoundryCatalog Catalog => _catalog;
public bool IsUnlocked => _state.unlocked;
public FoundryHeatStage HeatStage => _state.heatStage;
public FoundryLaborDispute LaborDispute => _state.laborDispute;
public bool IsMaintenanceOverdue => _state.daysSinceMaintenance > _state.maintenanceCycleDays;
public int DaysOverdue => Math.Max(0, _state.daysSinceMaintenance - _state.maintenanceCycleDays);
public int OverdueCycles => _state.maintenanceCycleDays > 0
public float GetComponentCondition(FoundryFacilityComponent component) {
public float AverageFacilityCondition() {
public bool IsJournalTriggered(string templateId) =>
public FoundryTreatyCompliance? GetTreatyCompliance(string treatyId) {
public IReadOnlyList<FoundryProductionRecord> CompletedProduction => _state.completed;
public IReadOnlyList<FoundryFailedCastRecord> FailedCasts => _state.failed;
public IReadOnlyList<FoundryIncidentRecord> Incidents => _state.incidents;
public int TotalProductionCount => _state.completed.Count;
public int TotalFailedCount => _state.failed.Count;
public float CumulativeStress => _state.cumulativeStress;
public float CumulativeHope => _state.cumulativeHope;
public bool IsHeatActive => HeatStage != FoundryHeatStage.Idle && HeatStage != FoundryHeatStage.Complete;
public float CurrentPowerDemandKw => HeatStage switch
public float CurrentWasteHeatKw => HeatStage switch
public void SuspendHeat(string reason, int day) {
public float GuildStanding => _consequenceState.guildStanding;
public IReadOnlyList<FoundryConsequenceRecord> AppliedConsequences => _consequenceState.applied;
public bool IsConsequenceApplied(string treatyId, int cycleMarker) => _consequenceState.IsApplied(treatyId, cycleMarker);
public FoundryTreatyOutcome GetTreatyOutcome(string treatyId, int day) {
public bool Unlock(int day) {
public string StartRepair(FoundryFacilityComponent component, int day) {
public string PerformMaintenance(int day) {
public string PrepareSand(int waterLitres) {
public string CompactMold(float skill) {
public void AssessTreatyCompliance(int day) {
```


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundryTypes.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundryTypes.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 282 lines / 10679 bytes.
- SHA-256: `519c0399c514e2bae74c69de64a1644410c1e0647f825381b5ae487219f10d95`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class SilentFoundryIds
public const string ExpansionId = "exp_10_the_silent_foundry";
public const string FactionId = "faction_silent_foundry";
public const string BlueprintRoomId = "room_bp_11_the_silent_foundry_smelter_bay";
public const string JournalFirstHeat = "jrnl_foundry_first_heat";
public const string JournalStrike = "jrnl_foundry_strike";
public const string TreatyBrinePipe = "treaty_brine_pipe_and_iodine_exchange";
public const string TreatyLabourSchedule = "treaty_cluster_labour_schedule";
public const string TreatyRoadIron = "treaty_road_iron_charter";
public const string TreatyClusterCharter = "treaty_the_cluster_charter";
public const string TreatySaltworksAccess = "treaty_saltworks_access";
public const string TreatyMembraneRepair = "treaty_membrane_repair";
public const string TreatyCoalWindow = "treaty_coal_window";
public const string TreatyApprenticeExchange = "treaty_apprentice_exchange";
public const string TreatyCrisisMutualAid = "treaty_crisis_mutual_aid";
public const string TreatyIncidentBook = "treaty_the_incident_book";
public const string ItemScrapMetal = "scrap_metal";
public const string ItemCoal = "coal";
public const string ItemCharcoal = "charcoal";
public const string ItemCleanWater = "clean_water";
public const string ItemFirebrick = "item_foundry_firebrick";
public const string ItemGreenSand = "item_foundry_green_sand";
public const string ItemFlux = "item_foundry_flux";
public const string ItemAlloyAdditive = "item_foundry_alloy_additive";
public const string ItemScrapMechanical = "scrap_mechanical";
public const string ItemCopperWire = "copper_wire_10m_of_10m";
public const string ItemShoringBracket = "item_foundry_shoring_bracket";
public const string CategoryHeavyMetallurgy = "heavy_metallurgy";
public enum FoundryHeatStage
public enum FoundryLaborDispute
public enum FoundryQualityTier
public enum FoundryIncidentSeverity
public enum FoundryFacilityComponent
public enum FoundryStrikeResolution
public sealed class FoundryProductionRecord
public string productId = string.Empty;
public string displayName = string.Empty;
public int amount = 0;
public FoundryQualityTier tier = FoundryQualityTier.Usable;
public int completedDay = 0;
public int workers = 0;
public string purity = FoundryPurityNames.Standard;
public string materialProfileId = string.Empty;
public int craftQualityPermille = 0;
public sealed class FoundryFailedCastRecord
public string productId = string.Empty;
public string displayName = string.Empty;
public string reason = string.Empty;
public int failedDay = 0;
public int materialsLost = 0;
public sealed class FoundryIncidentRecord
public FoundryIncidentSeverity severity = FoundryIncidentSeverity.None;
public int day = 0;
public string summary = string.Empty;
public int workersInjured = 0;
public int downtimeDays = 0;
public sealed class FoundryRepairRecord
public string component = string.Empty;
public int day = 0;
public float conditionBefore = 0f;
public float conditionAfter = 0f;
public sealed class FoundryTreatyCompliance
public string treatyId = string.Empty;
public string obligation = string.Empty;   // brine_pipe_quota | labor_shifts | road_iron_quota | charter_eligibility
public int quotaTotal = 0;
public int quotaFulfilled = 0;
public int quotaDeadlineDay = 0;           // next assessment day
public int lastAssessmentDay = 0;
public int metCount = 0;
public int missedCount = 0;
public bool currentCycleMet = false;
public float standingPenalty = 0f;
public bool constitutionEligible = false;
public sealed class SilentFoundryState
public const int CurrentVersion = 1;
public int stateVersion = CurrentVersion;
public bool unlocked = false;
public int unlockDay = 0;
public float refractoryLining = 100f;
public float hearthTuyeres = 100f;
public float sandBeds = 100f;
public float structuralSupports = 100f;
public float safetyExhaust = 100f;
public int maintenanceCycleDays = 4;       // authored anchor from room_bp_11
public int maintenanceDueDay = 0;
public int daysSinceMaintenance = 0;
public int maintenancePerformed = 0;
public float sandQuality = 65f;
public float sandMoisture = 65f;           // target band ~55..75
public float binderQuality = 60f;
public float patternQuality = 70f;
public float contamination = 5f;           // grows with low-grade charge
public int moldReuseCount = 0;
public float compaction = 70f;
public FoundryHeatStage heatStage = FoundryHeatStage.Idle;
public int heatStartedDay = 0;
public int stageElapsedDays = 0;
public string activeProductId = string.Empty;
public int assignedWorkers = 0;
public float workerSkill = 0.5f;
public float laborAccumulated = 0f;
public float workerExposure = 0f;          // fatigue/exposure units accrued
public int materialsConsumed = 0;
public bool childLaborUsed = false;
public float pendingQuality = 0f;
public List<FoundryProductionRecord> completed = new List<FoundryProductionRecord>();
public List<FoundryFailedCastRecord> failed = new List<FoundryFailedCastRecord>();
public List<FoundryIncidentRecord> incidents = new List<FoundryIncidentRecord>();
public List<FoundryRepairRecord> repairs = new List<FoundryRepairRecord>();
public FoundryLaborDispute laborDispute = FoundryLaborDispute.None;
public int laborDisputeStartedDay = 0;
public int strikeStartedDay = 0;
public bool overtimeFlag = false;
public bool educationConflictFlag = false;
public List<FoundryTreatyCompliance> treatyCompliance = new List<FoundryTreatyCompliance>();
public List<string> triggeredJournals = new List<string>();
public float cumulativeStress = 0f;
public float cumulativeHope = 0f;
public int firstHeatDay = 0;
public int strikeDay = 0;
public string activeMetallurgyRecipeId = string.Empty;
public float metallurgySlag = 0f;              // 0..100 normalized
public int metallurgyBatchesCompleted = 0;
public FoundryForgingSessionState? activeForging = null;
public int rngSeed = 0;
public sealed class FoundryJournalTrigger
public string TemplateId = string.Empty;
public float StressDelta = 0f;
public float HopeEarned = 0f;
public int Day = 0;
```


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 226 lines / 9433 bytes.
- SHA-256: `1e740631e69095fbd5d3c16a808fefb1345d299a09474d906ba53367848090e2`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum FoundryTreatyOutcome
public sealed class FoundryGoodModifier
public string good_id = string.Empty;
public float demand_delta = 0f;
public string reason = string.Empty;
public sealed class FoundryTreatyConsequencePolicy
public string treaty_id = string.Empty;
public string faction_id = string.Empty;
public string outcome = string.Empty;   // met | missed | violated
public float standing_delta = 0f;
public string reason = string.Empty;
public List<FoundryGoodModifier> market_modifiers = new List<FoundryGoodModifier>();
public sealed class FoundryTreatyConsequenceFile
public int schema_version = 1;
public string collection_id = string.Empty;
public List<FoundryTreatyConsequencePolicy> policies = new List<FoundryTreatyConsequencePolicy>();
public sealed class FoundryConsequenceRecord
public string treatyId = string.Empty;
public FoundryTreatyOutcome outcome = FoundryTreatyOutcome.Pending;
public int appliedDay = 0;
public int cycleMarker = 0;
public float standingDelta = 0f;
public List<FoundryGoodModifier> modifiers = new List<FoundryGoodModifier>();
public string reason = string.Empty;
public sealed class SilentFoundryConsequenceState
public const int CurrentVersion = 1;
public int stateVersion = CurrentVersion;
public List<FoundryConsequenceRecord> applied = new List<FoundryConsequenceRecord>();
public float guildStanding = 0f;
public bool IsApplied(string treatyId, int cycleMarker) {
public static class SilentFoundryConsequenceCatalogLoader
public const string FileName = "foundry_treaty_consequences.json";
public static FoundryTreatyConsequenceFile Load( string dataDirectory, IFileIO? files = null, IJsonSerializer? serializer = null) {
public sealed class SilentFoundryConsequencePolicyCatalog
public IReadOnlyList<string> Errors => _errors;
public bool HasErrors => _errors.Count > 0;
public int PolicyCount => _byKey.Count;
public IReadOnlyCollection<FoundryTreatyConsequencePolicy> AllPolicies => _byKey.Values;
public void Load(FoundryTreatyConsequenceFile file) {
public FoundryTreatyConsequencePolicy? Find(string treatyId, FoundryTreatyOutcome outcome) {
public static bool IsKnownOutcome(string outcome) {
public static readonly string[] KnownOutcomes = { "met", "missed", "violated" };
public static string OutcomeName(FoundryTreatyOutcome outcome) {
```


# Appendix E.19 — Supporting Code Evidence: `Assets/Ashfall.Core/ExpansionMasterSession.cs`

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


# Appendix F.20 — Supporting Data Evidence: `Assets/StreamingAssets/Data/foundry_accords.json`

### `Assets/StreamingAssets/Data/foundry_accords.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 20524 bytes / 20522 characters.
- SHA-256: `d82bb6e361e13ef12aa6b73d3ad570bc5e18fe45acb8354010597b1ad00bdf53`.
- Root keys: `collection_id`, `schema_version`, `treaties`.

Array-path census (minimum, maximum, observed rows):

```text
treaties: min=18, max=18, observed_paths=1
treaties[].signatory_factions: min=2, max=3, observed_paths=2
treaties[].tags: min=6, max=6, observed_paths=2
```

Representative record fields:

- `demarcated_territory`
- `penalties`
- `power_quota_kw`
- `ratified_day`
- `signatory_factions`
- `tags`
- `tariff_schedule`
- `treaty_articles`
- `treaty_id`
- `treaty_title`
- `water_allocation_lpm`


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/FoundryAccordExpansionTests.cs`

### `Ashfall.Core.Tests/FoundryAccordExpansionTests.cs`

- Current test declarations: Fact=11, Theory=0, InlineData=0.
- File lines: 362; SHA-256: `73613c87e16e2a46a840492ce7dfedd0a89818997867e174ffb5770a5dfbd3a9`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsAllAccordsWithoutErrors
Parity_BaselineFourDistrict8AccordsPreserved
FoundryRoster_ContainsExactlyTheSixNewDependencyReadyAccords
TreatyId_AllIdsAreUniqueAndFollowSnakeCasePrefix
Signatories_AllFactionsAreValidAndNonEmpty
Resources_WaterAndPowerAllocationsAreNonNegativeAndPlausible
LegalText_ArticlesFollowNumberedClausesAndPenaltiesAreEnforceable
Tags_FollowNormalizedVocabularyWithoutSynonymSplits
Timeline_RatificationDaysAreChronologicallyOrdered
Diversity_CatalogSpansResourceLogisticsTerritorialAndGovernanceRoles
ConsequenceSeam_Plan103PoliciesResolveAgainstTheseAccords
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/FoundryTreatyConsequenceExpansionTests.cs`

### `Ashfall.Core.Tests/FoundryTreatyConsequenceExpansionTests.cs`

- Current test declarations: Fact=14, Theory=0, InlineData=0.
- File lines: 326; SHA-256: `83ec8f1d815bef517019f71291388c35121d792b59cb9b048fe3561e42d619f5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsExactlyFifteenPoliciesWithoutErrors
ReferenceIntegrity_AllTreatyIdsResolveInFoundryAccords
ReferenceIntegrity_AllFactionIdsAreSignatoriesOfReferencedTreaty
OutcomeValidation_AllPoliciesUseCanonicalOutcomeVocabulary
MechanicalEffectValidation_AllMarketGoodModifiersResolveInEconomyGoods
PolicyUniqueness_NoDuplicateTreatyAndOutcomeKeys
CoverageMatrix_EightTreatiesCoveredWithRationalDistribution
RepresentativePolicy_SaltworksMetAndViolated
RepresentativePolicy_CoalWindowMetAndMissed
RepresentativePolicy_MembraneRepairMetAndViolated
RepresentativePolicy_CrisisMutualAidMetAndViolated
RepresentativePolicy_IncidentBookMet
Idempotency_RecordStateTracksAssessmentDayCycleKey
Balance_StandingDeltasAreBoundedAndProportional
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/SilentFoundrySystemTests.cs`

### `Ashfall.Core.Tests/SilentFoundrySystemTests.cs`

- Current test declarations: Fact=35, Theory=0, InlineData=0.
- File lines: 916; SHA-256: `4f3c37bec6f3232e20309bddda99525206c3c11d3d34f775792865f5118adfd8`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Identity_ExactIdsResolve
Catalog_LoadsAllAuthoredProductsAndFaction
Catalog_QuotaProductsMapToExactTreaties
JournalTemplates_LoadWithExpansionIsolation
JournalDeltas_MatchAuthoredTemplate
Blueprint_ResolvesAndAnchorsMaintenanceCycle
Treaties_GuildIsExactSignatoryOfTenFoundryTreaties
Unlock_IsIdempotentAndRaisesOnce
Repair_ConsumesFirebrickAndRestoresComponent
Maintenance_FourDayCycleAndOverdueConsequences
SandPrep_ConsumesSandAndWaterAndImprovesBed
SandPrep_BlocksWithoutSandOrWater
Production_StartValidatesChargeAndConsumesResources
Production_MissingChargeGivesVisibleReason
Production_FirstHeatCompletesAndTriggersJournalOnce
Production_SecondHeatDoesNotRetriggerJournal
Production_UntappedHeatBurnsOutAndRecordsFailure
Production_QualityTiersAreDeterministicPerSeed
Safety_WarningsSurfaceBeforeIrreversibleTap
Incident_SameSeedSameOutcome
Incident_IsNeverHiddenAndLeavesARecord
Incident_WellMaintainedFurnaceNeverIncidents
Treaty_RailQuotaMetOnDeadline
Treaty_RailQuotaMissedOnDeadline
Treaty_LaborShiftViolationWhenStrikeOrOvertime
Treaty_RatificationDaysAreNotAssessedBeforeRatification
Strike_FatigueAloneDoesNotTriggerDispute
Strike_ProductionPressurePlusShiftGrievanceTriggersAndEscalates
Strike_ResolutionIsTypedAndOnceOnly
Save_RoundTripPreservesAllFoundryState
Save_ActiveFurnaceSurvivesRoundTrip
Save_MissingFoundryStateDefaultsSafely
Save_ChecksumStableAcrossHostSerializers
Save_ExpansionHubEnvelopeRoundTripsWithMigration
Events_EmitExactlyOncePerOutcome
```


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| treaty JSON parsing, lookup and ratification-day loading | SilentFoundryCatalogLoader | foundry production, treaty compliance, allocations and current state | SilentFoundrySystem | Owner emits/reads a typed fact; no mirror state. |
| treaty JSON parsing, lookup and ratification-day loading | SilentFoundryCatalogLoader | treaty breach consequence mapping | SilentFoundryConsequencePolicyCatalog | Owner emits/reads a typed fact; no mirror state. |
| treaty JSON parsing, lookup and ratification-day loading | SilentFoundryCatalogLoader | catalog and Foundry composition | ExpansionMasterSession | Owner emits/reads a typed fact; no mirror state. |
| treaty JSON parsing, lookup and ratification-day loading | SilentFoundryCatalogLoader | host adapter and player-facing Foundry operations | SilentFoundryHostSession | Owner emits/reads a typed fact; no mirror state. |
| treaty JSON parsing, lookup and ratification-day loading | SilentFoundryCatalogLoader | current treaty catalog and reference contract | FoundryAccordExpansionTests | Owner emits/reads a typed fact; no mirror state. |
| foundry production, treaty compliance, allocations and current state | SilentFoundrySystem | treaty JSON parsing, lookup and ratification-day loading | SilentFoundryCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| foundry production, treaty compliance, allocations and current state | SilentFoundrySystem | treaty breach consequence mapping | SilentFoundryConsequencePolicyCatalog | Owner emits/reads a typed fact; no mirror state. |
| foundry production, treaty compliance, allocations and current state | SilentFoundrySystem | catalog and Foundry composition | ExpansionMasterSession | Owner emits/reads a typed fact; no mirror state. |
| foundry production, treaty compliance, allocations and current state | SilentFoundrySystem | host adapter and player-facing Foundry operations | SilentFoundryHostSession | Owner emits/reads a typed fact; no mirror state. |
| foundry production, treaty compliance, allocations and current state | SilentFoundrySystem | current treaty catalog and reference contract | FoundryAccordExpansionTests | Owner emits/reads a typed fact; no mirror state. |
| treaty breach consequence mapping | SilentFoundryConsequencePolicyCatalog | treaty JSON parsing, lookup and ratification-day loading | SilentFoundryCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| treaty breach consequence mapping | SilentFoundryConsequencePolicyCatalog | foundry production, treaty compliance, allocations and current state | SilentFoundrySystem | Owner emits/reads a typed fact; no mirror state. |
| treaty breach consequence mapping | SilentFoundryConsequencePolicyCatalog | catalog and Foundry composition | ExpansionMasterSession | Owner emits/reads a typed fact; no mirror state. |
| treaty breach consequence mapping | SilentFoundryConsequencePolicyCatalog | host adapter and player-facing Foundry operations | SilentFoundryHostSession | Owner emits/reads a typed fact; no mirror state. |
| treaty breach consequence mapping | SilentFoundryConsequencePolicyCatalog | current treaty catalog and reference contract | FoundryAccordExpansionTests | Owner emits/reads a typed fact; no mirror state. |
| catalog and Foundry composition | ExpansionMasterSession | treaty JSON parsing, lookup and ratification-day loading | SilentFoundryCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| catalog and Foundry composition | ExpansionMasterSession | foundry production, treaty compliance, allocations and current state | SilentFoundrySystem | Owner emits/reads a typed fact; no mirror state. |
| catalog and Foundry composition | ExpansionMasterSession | treaty breach consequence mapping | SilentFoundryConsequencePolicyCatalog | Owner emits/reads a typed fact; no mirror state. |
| catalog and Foundry composition | ExpansionMasterSession | host adapter and player-facing Foundry operations | SilentFoundryHostSession | Owner emits/reads a typed fact; no mirror state. |
| catalog and Foundry composition | ExpansionMasterSession | current treaty catalog and reference contract | FoundryAccordExpansionTests | Owner emits/reads a typed fact; no mirror state. |
| host adapter and player-facing Foundry operations | SilentFoundryHostSession | treaty JSON parsing, lookup and ratification-day loading | SilentFoundryCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| host adapter and player-facing Foundry operations | SilentFoundryHostSession | foundry production, treaty compliance, allocations and current state | SilentFoundrySystem | Owner emits/reads a typed fact; no mirror state. |
| host adapter and player-facing Foundry operations | SilentFoundryHostSession | treaty breach consequence mapping | SilentFoundryConsequencePolicyCatalog | Owner emits/reads a typed fact; no mirror state. |
| host adapter and player-facing Foundry operations | SilentFoundryHostSession | catalog and Foundry composition | ExpansionMasterSession | Owner emits/reads a typed fact; no mirror state. |
| host adapter and player-facing Foundry operations | SilentFoundryHostSession | current treaty catalog and reference contract | FoundryAccordExpansionTests | Owner emits/reads a typed fact; no mirror state. |
| current treaty catalog and reference contract | FoundryAccordExpansionTests | treaty JSON parsing, lookup and ratification-day loading | SilentFoundryCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| current treaty catalog and reference contract | FoundryAccordExpansionTests | foundry production, treaty compliance, allocations and current state | SilentFoundrySystem | Owner emits/reads a typed fact; no mirror state. |
| current treaty catalog and reference contract | FoundryAccordExpansionTests | treaty breach consequence mapping | SilentFoundryConsequencePolicyCatalog | Owner emits/reads a typed fact; no mirror state. |
| current treaty catalog and reference contract | FoundryAccordExpansionTests | catalog and Foundry composition | ExpansionMasterSession | Owner emits/reads a typed fact; no mirror state. |
| current treaty catalog and reference contract | FoundryAccordExpansionTests | host adapter and player-facing Foundry operations | SilentFoundryHostSession | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the old count target with an 18-row treaty census and owner/reachability matrix. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Map treaty fields to current consumers: signatory identity, ratification day, allocation, compliance, consequence policy and journal/UI projection. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Treat authored legal prose as narrative unless an existing contract explicitly executes it. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Add no treaty rows, no second treaty save and no parallel faction treaty manager. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix — Master authority re-read

# Appendix — Master Authority Re-read Record

The following excerpts were selected from the live master authority by this plan's subject terms. They are planning constraints, not claims that the historical backlog is current.

> This document is the complete compiled edition of the ASHFALL Master Expansion Authority v2.0. It combines, in order: (1) the uploaded source document (Volumes 1–24 and its Parts 0 through V, reproduced verbatim), and (2) the Plan Factory's expansion volumes 25 through 57 (the factory batches of 2026-09-24, reproduced verbatim). No content has been altered, merged, or summarized; the two bodies are concatenated at their natural boundary. The source document's own authority order stands: live repository source first; then AGENTS.md; then this document. The factory's constitution (evidence labels, honest bounds, anti-padding) governs Volumes 25 onward.

> **Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival` (ASHFALL: Atomic War – Starving Survival)
**Document version:** 2.0.0 — compiled 2026-09-24, supersedes the v1.0 master world bible (compiled 2026-09-23) as an expansion scaffold.
**Document class:** SUBJECT-PLAN FACTORY. This document is not itself an integration plan. It is a repeatable generator: any future planning session can consume its matrices, backlog, and templates to produce an unbounded series of bounded subject plans, each of which names its own best integration route.
**Audit basis:** Live repository inspection performed 2026-09-24 (repository root listing, `Assets/StreamingAssets/Data/` listing at 342 entries, `docs/` listing, `docs/plans/` listing at 126 entries, `INTEGRATION_PLANS.md`, `SESSION_HANDOFF.md`, `AGENTS.md`, branch list). Every claim in the Drift Register (Part I) is labeled VERIFIED, HIGH CONFIDENCE, or UNVERIFIED.
**Authority order:** unchanged from v1.0 — live repository source and data first; then `AGENTS.md`; then this document; then the docs registry and atlas; then plan ledgers. Where this document and live source disagree, live source wins and this document must be corrected.

> **DR-02 — The docs tree has substantially more subdirectories than the v1.0 map. VERIFIED.**
Live `docs/` subdirectories observed in the audit include (selection; the listing was long and partially truncated): `adr/`, `agents/`, `architecture/`, `archive/`, `balance/`, `bodymind/`, `campaign/`, `cartography/`, `ci/`, `cli/`, `collectibles/`, `combat/`, `content/`, `contracts/`, `crafting/`, `crossing/`, `culture/`, `decisions/`, `design/`, `discovery/`, `duty_roster/`, `ecology/`, `economy/`, `endgame/`, `expansions/`, `expeditions/`, `faction_war/`, `factions/`, `foreman/`, `forensics/`, `foundry/`, `gaps/`, `governance/`, `greenhouse/`, `health/`, `holdfast/`, `hygiene/`, `i18n/`, `implementation/`, `incidents/`, `integration/`, `journal/`, `lore/`, `maritime/`, `medical/`, `memorials/`, `mods/`, `moral/`, `moral_choice/`, `muster/`, `narrative/`, `onboarding/`, `orbital/`, `perf/`, `phantoms/`, `plans/`, `power/`, `process/`, `production/`, and a `player_surface_manifest.json`. Two of these — `gaps/` and `incidents/` — are first-class *expansion feedstock*: directories whose entire purpose is to record what is missing or broken. The Factory Protocol (Part II, step 2) now treats `docs/gaps/` and `docs/incidents/` as mandatory inputs.

> **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
Observed live and not listed in v1.0 Part 5.8: `ECONOMY_FAIRNESS_AUDIT.md`, `ENGINE_SUPPORT_POLICY.md`, `GODOT_MIGRATION_STATUS.md`, `REPO_HISTORY_REWRITE.md`, `HUMAN_AUTHORSHIP.md`, `AI_DISCLOSURE.md`, `ASSET_MIGRATION_LEDGER.md`, `CODEX_SOURCE_MATRIX.md`, `ARCHIVE_INDEX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md`, `SHELTER_MAINTENANCE_MATRIX.md`, `SHELTER_30_DAY_MAINTENANCE_REPORT.md`, `L10N_WAVE2_ROADMAP.md`, `INPUT.md`, `RELEASE_EXPORT.md`, `ENGINE_SUPPORT_POLICY.md`. Of these, `ECONOMY_FAIRNESS_AUDIT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, and `SHELTER_MAINTENANCE_MATRIX.md` are pre-computed balance baselines: they convert Lane C (economy and balance) planning from speculative to evidence-anchored. Subject plans in Lane C must cite these baselines instead of re-deriving numbers.

> **DR-05 — A process script lives inside the data authority. VERIFIED (hygiene finding).**
`Assets/StreamingAssets/Data/` contains `rewrite.py` alongside the JSON catalogs. The data directory is canonically "the sole authored JSON data authority" (`AGENTS.md` rule 3); a Python rewrite script inside it is a process artifact in a content directory. Recommended handling: a Tooling-lane (Lane H) subject plan proposing relocation of the script to `scripts/` or `tools/` with a documented rationale, after verifying what the script rewrites and who calls it. Do not move it without call-site verification; it may be load-bearing for a historical catalog migration.

> **DR-06 — Integration ledger state differs from the v1.0 queue snapshot. VERIFIED.**
Live `INTEGRATION_PLANS.md` (read 2026-09-24) shows, at minimum: the **XP Expansion W1** batch ACTIVE (difficulty authority package `XP-WAVE1-DIFFICULTY-AUTHORITY`, with a premise correction recorded against Plan 122 SOFC fuel); the **DISTRESS-SIGNALS-9-12** flagship COMPLETE and presented for acceptance; **Plan 24 CLOSED** (Wave 8, 2026-09-17, signatures resolved 2026-09-18, ward staffing sealed under option b, `DEBT-PLAN24-MEDICAL-WARD-STAFFING` RETIRED); **19A/19B/19C** waves closed with evidence (Endgame 84/84 PASS, focused suites 47 PASS, `verify-fast.sh` reported ALL 47 GATES PASSED); **C2[2] Plan 17** legibility executed with a documented not-executed list (Plan 31 semantic-kind authority, 17C audio phases, 17B deep test matrix remain open gaps); **PR 3 content seal SEALED 2026-09-19** (`CF-P1-DISTRESS-CONTENT-SEAL`); the **availability consumer RETIRED** (Wave 9 Part 2, Option B approved; `SignalTrustAvailability` retained as a pure-math specification pin). Consequence: subject plans in the radio/distress domain must treat the rescue-signal runtime as *sealed and closed*, not as an open expansion surface, unless they extend it through its recorded seams.

> The following v1.0 structures were confirmed by the audit and remain authoritative: the four-tier architecture (Tier 1 data authority in `Assets/StreamingAssets/Data/`; Tier 2 engine-free Core; Tier 3 `src/Host` + `src/UI`; Tier 4 xUnit plus the `HostCli` selftest surface); the `AGENTS.md` non-negotiable rules (Godot authoritative, Core engine-free, JSON authoritative, one authority per concern, focused verification); the narrative corpus under `Assets/StreamingAssets/Data/narrative/` (present in the live listing); the faction, economy, weather, Year-of-Ash, moral-choice, muster, and verdict catalog families (all present live); and the plan-discipline artifacts (`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `SESSION_HANDOFF.md`) at root.

> **Step 5 — Run the continuity and anti-duplication checklist.**
The v1.0 checklist (Part 13.2) applies in full, plus two factory additions: (a) duplication firewall — prove the candidate does not duplicate any live catalog, system, or `docs/` authority map; (b) unclaimed-content check — if the candidate's content domain appears in `UNCLAIMED_CORPUS_CENSUS.md`, the plan must wire the unclaimed content first or explain why new content outranks it.

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

The safe subject is the 18-row treaty catalog and the existing Foundry owner chain, not another diplomacy feature.

- **treaty JSON parsing, lookup and ratification-day loading** remains with `SilentFoundryCatalogLoader` at `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs`. Static treaty catalog owner.
- **foundry production, treaty compliance, allocations and current state** remains with `SilentFoundrySystem` at `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`. Sole Foundry state authority.
- **treaty breach consequence mapping** remains with `SilentFoundryConsequencePolicyCatalog` at `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs`. Existing consequence policy boundary.
- **catalog and Foundry composition** remains with `ExpansionMasterSession` at `Assets/Ashfall.Core/ExpansionMasterSession.cs`. Composes the existing owners.
- **host adapter and player-facing Foundry operations** remains with `SilentFoundryHostSession` at `src/Foundry/SilentFoundryHostSession.cs`. Presentation and command adapter only.
- **current treaty catalog and reference contract** remains with `FoundryAccordExpansionTests` at `Ashfall.Core.Tests/FoundryAccordExpansionTests.cs`. Focused executable evidence, not a new runtime owner.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load foundry_accords.json
2. validate treaty IDs and signatory references
3. load ratification days and consequence policies
4. read current Foundry compliance/allocation state
5. project only the treaties consumed by the current owner
6. present through the existing Foundry route
7. capture existing Foundry state only

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Treaty rows are immutable authored definitions.
- Foundry compliance and allocation state remain in SilentFoundryState and its current save path.
- A treaty is not a faction-wide reputation ledger and cannot mutate FactionStanceEngine directly.
- Ratifiation day and consequence policy are references consumed by current systems, not a second calendar.

- Every signatory must resolve through canonical faction identity.
- A missing consequence policy must fail closed and leave compliance unchanged.
- A treaty row cannot grant water, power, tariffs or penalties without the owning Foundry command.
- The same catalog, state and day produce the same treaty projection.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Foundry/SilentFoundryHostSession.cs
- src/Host/ExpansionHostSession.cs
- src/UI/SilentFoundryPanel.cs
- src/Main.UiTests.SilentFoundry.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/FoundryAccordExpansionTests.cs
- Ashfall.Core.Tests/FoundryTreatyConsequenceExpansionTests.cs
- Ashfall.Core.Tests/SilentFoundrySystemTests.cs

These commands are intentionally small and named. A planning rebuild does not run them and does not convert historical pass counts into fresh evidence. An implementation package records the actual command, result, fixture and limitation.

## F. Precision questions for the next owner

- Which current method is the single mutation point for each durable fact?
- Which loader and validator prove that every authored row is admitted?
- Which host command makes the feature reachable from the live game?
- Which existing save section carries the fact, and what is its frozen legacy shape?
- Which deterministic stream, ordering rule or no-RNG contract governs repeated execution?
- What visible refusal prevents a player from mistaking a projection for authority?
- What focused test would fail if the owner were bypassed?
- What future file or type is explicitly *not* part of this package?


# Appendix — Scenario matrix

# Appendix — Scenario and Negative-Contract Matrix

| ID | Scenario | Precondition | Expected owner outcome | Negative proof | Owner |
| --- | --- | --- | --- | --- | --- |
| S-01 | 102-01 The safe subject is the 18-row treaty catalog and the existing Foundry owner chain, not another diplomacy feature. | load foundry_accords.json | Treaty rows are immutable authored definitions. | A treaty signatory references a removed faction. | SilentFoundryCatalogLoader |
| S-02 | 102-02 current owner boundary | validate treaty IDs and signatory references | Foundry compliance and allocation state remain in SilentFoundryState and its current save path. | A static tariff field silently changes market prices. | SilentFoundryCatalogLoader |
| S-03 | 102-03 missing reference refusal | load ratification days and consequence policies | A treaty is not a faction-wide reputation ledger and cannot mutate FactionStanceEngine directly. | A consequence policy applies twice after restore. | SilentFoundryCatalogLoader |
| S-04 | 102-04 save continuation | read current Foundry compliance/allocation state | Ratifiation day and consequence policy are references consumed by current systems, not a second calendar. | A panel shows a treaty as active while the Foundry owner has no state. | SilentFoundryCatalogLoader |
| S-05 | 102-05 same-seed replay | project only the treaties consumed by the current owner | Treaty rows are immutable authored definitions. | A new treaty save section duplicates Foundry state. | SilentFoundryCatalogLoader |
| S-06 | 102-06 UI truthfulness | present through the existing Foundry route | Foundry compliance and allocation state remain in SilentFoundryState and its current save path. | A treaty signatory references a removed faction. | SilentFoundryCatalogLoader |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 102-TC-01 schema and count | data | schema and count; verify the named current owner and its negative boundary without inventing a second authority. | SilentFoundryCatalogLoader |
| T-02 | 102-TC-02 reference validation | unit | reference validation; verify the named current owner and its negative boundary without inventing a second authority. | SilentFoundryCatalogLoader |
| T-03 | 102-TC-03 current owner boundary | persistence | current owner boundary; verify the named current owner and its negative boundary without inventing a second authority. | SilentFoundryCatalogLoader |
| T-04 | 102-TC-04 failure refusal | determinism | failure refusal; verify the named current owner and its negative boundary without inventing a second authority. | SilentFoundryCatalogLoader |
| T-05 | 102-TC-05 save continuation | host | save continuation; verify the named current owner and its negative boundary without inventing a second authority. | SilentFoundryCatalogLoader |
| T-06 | 102-TC-06 deterministic replay | UI/accessibility | deterministic replay; verify the named current owner and its negative boundary without inventing a second authority. | SilentFoundryCatalogLoader |
| T-07 | 102-TC-07 host/UI truth | cross-system | host/UI truth; verify the named current owner and its negative boundary without inventing a second authority. | SilentFoundryCatalogLoader |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 28 | `Ashfall.Core.Tests/SilentFoundrySystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 17 | `Assets/Ashfall.Core/ExpansionMasterSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 17 | `src/Foundry/SilentFoundryHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 15 | `Ashfall.Core.Tests/Foundry/Plan213MetallurgyReconciliationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 15 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 13 | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Ashfall.Core.Tests/SilentFoundryConsequenceTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `Ashfall.Core.Tests/Foundry/FoundryPlan129IntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `src/Host/ExpansionHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/Foundry/FoundryActionSurfaceTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/Foundry/GlassworksB100Tests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/Foundry/MetallurgyB66Tests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/ShelterMachineTellTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Assets/Ashfall.Core/Foundry/SilentFoundryHeadlessDemo.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/ExpansionsIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/Foundry/FoundryExpansionProductTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/Integration/PlansB66ToB69CrossSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Content/ContentDeepChainGate.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `src/UI/SilentFoundryPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Foundry/FoundryActionSurface.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Radio/Plan94_102RadioFoundryIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Foundry/SilentFoundryTypes.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/UI/BrineExtractionPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/ApicultureAndTriangulationIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/ContentDeepChainGateTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/DutyRosterIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/FoundryTreatyConsequenceExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Assets/Ashfall.Core/ExpansionHubSave.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.TreatyLabor.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Main.ExpansionHub.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/FactionIconCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/FlagshipEconomyScenarioTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/FoundryAccordExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/Narrative/Plan95_103JournalTreatyIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/foundry_accords.json`

### `Assets/StreamingAssets/Data/foundry_accords.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 20524; characters: 20522.
- SHA-256: `d82bb6e361e13ef12aa6b73d3ad570bc5e18fe45acb8354010597b1ad00bdf53`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `collection_id`, `treaties`

#### `treaties` — 18 current rows

- Row 001 `treaty_brine_pipe_and_iodine_exchange`: `{"demarcated_territory":"The smelter bay casting floor to the saltworks membrane hall","penalties":"A missed cycle suspends the iodine allocation until the pipe debt is cast.","power_quota_kw":12.0,"ratified_day":280,"signatory_factions":[…`
- Row 002 `treaty_cluster_labour_schedule`: `{"demarcated_territory":"The charging floor, the school bell, and the eight hours between them","penalties":"Lockouts or unannounced strikes forfeit the works' coal window on the next ice.","power_quota_kw":8.0,"ratified_day":305,"signator…`
- Row 003 `treaty_road_iron_charter`: `{"demarcated_territory":"The casting floor to the Cut, by whatever lane is marked","penalties":"A missed cycle closes the foundry's lane on the next window and re-opens the accident book.","power_quota_kw":6.0,"ratified_day":330,"signatory…`
- Row 004 `treaty_the_cluster_charter`: `{"demarcated_territory":"The smelter bay, its ledger, and the schedule it answers to","penalties":"None are provided. A charter that needed penalties would not be worth signing.","power_quota_kw":0.0,"ratified_day":365,"signatory_factions"…`
- Row 005 `treaty_garrison_grain_tithe_compact`: `{"demarcated_territory":"The Verge agricultural flats and Checkpoint Gamma perimeter","penalties":"Failure to deliver closes the Eastern Arterial Road to civilian commerce.","power_quota_kw":15.0,"ratified_day":120,"signatory_factions":["f…`
- Row 006 `treaty_flotilla_saline_corridor_concordat`: `{"demarcated_territory":"The dredged channel from Lock Gate Four to the Shallows Market","penalties":"Missed fuel tariffs double the lockage toll for sixty days.","power_quota_kw":10.0,"ratified_day":180,"signatory_factions":["faction_the_…`
- Row 007 `treaty_switchback_fuel_and_passage_accord`: `{"demarcated_territory":"The Switchback Waystation to Snowline Patrol Station","penalties":"Unprovoked hostility revokes snowline transit rights.","power_quota_kw":5.0,"ratified_day":210,"signatory_factions":["faction_ash_sign","faction_fo…`
- Row 008 `treaty_scale_suburban_fair_trade_convention`: `{"demarcated_territory":"The Caravanserai, Grange Hall, and Verity Motel triangle","penalties":"Short-weighing bars the offending trader from regional markets for one season.","power_quota_kw":8.0,"ratified_day":240,"signatory_factions":["…`
- Row 009 `treaty_scrap_salvage_demarcation`: `{"demarcated_territory":"The Recovery Yard and Concrete Batching Plant perimeter","penalties":"Delivery of unsorted scrap incurs a fifteen percent price discount.","power_quota_kw":18.0,"ratified_day":260,"signatory_factions":["faction_the…`
- Row 010 `treaty_roster_border_demilitarization_pact`: `{"demarcated_territory":"The Neutral Ground five-kilometer buffer zone","penalties":"Armor incursions across the buffer authorize defensive counter-battery fire.","power_quota_kw":0.0,"ratified_day":290,"signatory_factions":["faction_forwa…`
- Row 011 `treaty_deep_coast_aquifer_protection_treaty`: `{"demarcated_territory":"Pump Station Nine and coastal marsh intake tributaries","penalties":"Pollution incidents forfeit maritime trade docking rights.","power_quota_kw":14.0,"ratified_day":315,"signatory_factions":["faction_the_fleet","f…`
- Row 012 `treaty_high_scarp_observatory_sanctuary`: `{"demarcated_territory":"The Summit Relay Spire and Low-Background Laboratory enclosure","penalties":"Sabotage of transmission equipment triggers total embargo on mountain fuel deliveries.","power_quota_kw":20.0,"ratified_day":340,"signato…`
- Row 013 `treaty_saltworks_access`: `{"demarcated_territory":"The east pipe walk between the smelter bay casting floor and the saltworks membrane hall","penalties":"Two unentered diversions in one assessment cycle suspend the compact's water allocation for seven days and rest…`
- Row 014 `treaty_membrane_repair`: `{"demarcated_territory":"The membrane hall service corridor from the brine intake valves to the iodine store test bench","penalties":"A repair that fails acceptance twice costs the Foundry a replacement plate from its next heat and permits…`
- Row 015 `treaty_coal_window`: `{"demarcated_territory":"The marked ice-road lane from the Cut weigh-hut to the Foundry coal gate and its charging-floor staging apron","penalties":"A missed declared window loses the Foundry's next priority slot and one anchor credit; an …`
- Row 016 `treaty_apprentice_exchange`: `{"demarcated_territory":"The charging floor, the Cluster school bell, and the registered salvage-claim desk at the works' outer gate","penalties":"A signatory that converts a trainee into quota labor loses the next intake, replaces damaged…`
- Row 017 `treaty_crisis_mutual_aid`: `{"demarcated_territory":"The smelter bay, the marked ice-road staging yards, and the Office's Cluster cistern approach","penalties":"A signatory that refuses a named service without a recorded capacity reason loses the no-tariff privilege …`
- Row 018 `treaty_the_incident_book`: `{"demarcated_territory":"The Foundry incident desk, the Office register hall, and the Archivists' copy shelf at the Cluster school","penalties":"A missing or altered entry permits an unannounced joint inspection and suspends the Foundry's …`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs` — complete current file

- Size: 303 lines / 12451 bytes.
- SHA-256: `f1630ce8db3a4c544cd1889159df443cbc87fa346a587cf679f5858b3021a33a`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using System.IO;
00006: using Ashfall.Core.Narrative;
00007:
00008: using Ashfall.Core.IO;
00009: namespace Ashfall.Core.Foundry
00010: {
00011:     // ---------------------------------------------------------------------
00012:     // Static authored production catalog — foundry_production.json
00013:     // Typed ownership for game rules. Do NOT hide rules in notes/tags.
00014:     // ---------------------------------------------------------------------
00015:
00016:     /// <summary>One ingredient line of a foundry production recipe.</summary>
00017:     [Serializable]
00018:     public sealed class FoundryIngredientEntry
00019:     {
00020:         public string item_id = string.Empty;
00021:         public int amount = 1;
00022:     }
00023:
00024:     /// <summary>
00025:     /// One castable product. All costs and consequences are typed fields so the
00026:     /// runtime can enforce them deterministically. `treaty_id`/`quota_amount`
00027:     /// bind a product to an authored treaty obligation (rail spikes, wheels,
00028:     /// acid pipes) without duplicating the treaty itself.
00029:     /// </summary>
00030:     [Serializable]
00031:     public sealed class FoundryProductEntry
00032:     {
00033:         public string product_id = string.Empty;
00034:         public string display_name = string.Empty;
00035:
00036:         /// <summary>
00037:         /// Sink category the product feeds: agricultural_tool | structural_beam |
00038:         /// ice_anchor | winch_drum | brine_resistant_pipe | repair_plate |
00039:         /// bracket_fastener | water_component | heavy_tool | heavy_alloy_part |
00040:         /// defense_plate.
00041:         /// </summary>
00042:         public string category = string.Empty;
00043:
00044:         public string result_item_id = string.Empty;
00045:         public int result_amount = 1;
00046:         public List<FoundryIngredientEntry> ingredients = new List<FoundryIngredientEntry>();
00047:
00048:         /// <summary>Total human labour-hours across the heat (spread over workers).</summary>
00049:         public float labor_hours = 0f;
00050:
00051:         /// <summary>Furnace time in hours from tap to pour completion.</summary>
00052:         public float cast_hours = 0f;
00053:
00054:         /// <summary>Coal/charcoal units consumed by the charge.</summary>
00055:         public int fuel_units = 0;
00056:
00057:         /// <summary>Water litres demanded by the heat (cooling + sand prep).</summary>
00058:         public int water_litres = 0;
00059:
00060:         /// <summary>Typical skill (0..1) the recipe assumes; deviation shifts quality.</summary>
00061:         public float skill_target = 0.5f;
00062:
00063:         /// <summary>Baseline quality score (0..100) the recipe is balanced around.</summary>
00064:         public float quality_target = 70f;
00065:
00066:         /// <summary>Optional authored treaty obligation (exact treaty id).</summary>
00067:         public string treaty_id = string.Empty;
00068:
00069:         /// <summary>Units of this product owed per treaty assessment cycle (0 = none).</summary>
00070:         public int quota_amount = 0;
00071:
00072:         /// <summary>Human-readable sink label for UI (authored context only).</summary>
00073:         public string sink = string.Empty;
00074:
00075:         public string notes = string.Empty;
00076:         public string[] tags = Array.Empty<string>();
00077:     }
00078:
00079:     [Serializable]
00080:     public sealed class FoundryProductionFile
00081:     {
00082:         public int schema_version = 1;
00083:         public string collection_id = string.Empty;
00084:         public List<FoundryProductEntry> products = new List<FoundryProductEntry>();
00085:     }
00086:
00087:     // ---------------------------------------------------------------------
00088:     // Static faction registry entry — foundry_faction.json
00089:     // Registers the District 8 works faction id (faction_silent_foundry).
00090:     // ---------------------------------------------------------------------
00091:
00092:     /// <summary>A named relationship to another faction; typed, not lore prose.</summary>
00093:     [Serializable]
00094:     public sealed class FoundryFactionRelation
00095:     {
00096:         public string faction_id = string.Empty;
00097:         public string stance = string.Empty;   // ally | trade_partner | rival | internal
00098:         public string notes = string.Empty;
00099:     }
00100:
00101:     [Serializable]
00102:     public sealed class FoundryFactionEntry
00103:     {
00104:         public string faction_id = string.Empty;
00105:         public string display_name = string.Empty;
00106:         public string short_name = string.Empty;
00107:         public string identity = string.Empty;
00108:         public string icon_path = string.Empty;
00109:         public string[] internal_divisions = Array.Empty<string>();
00110:         public List<FoundryFactionRelation> relationships = new List<FoundryFactionRelation>();
00111:         public string[] tags = Array.Empty<string>();
00112:     }
00113:
00114:     // ---------------------------------------------------------------------
00115:     // Loader
00116:     // ---------------------------------------------------------------------
00117:
00118:     /// <summary>
00119:     /// Engine-agnostic loader for the two Foundry static catalogs. Reads the
00120:     /// exact snake_case JSON schema authored in StreamingAssets/Data.
00121:     /// </summary>
00122:     public static class SilentFoundryCatalogLoader
00123:     {
00124:         public const string ProductionFileName = "foundry_production.json";
00125:         public const string FactionFileName = "foundry_faction.json";
00126:         public const string AccordsFileName = "foundry_accords.json";
00127:
00128:         /// <summary>
00129:         /// District 8 accord ratification days (treaty id → ratified day) from
00130:         /// foundry_accords.json. Same schema as the narrative treaty corpus, but
00131:         /// authored for the live Sector 4 / District 8 campaign.
00132:         /// </summary>
00133:         public static Dictionary<string, int> LoadAccordRatificationDays(
00134:             string dataDirectory,
00135: IFileIO? files = null,
00136: IJsonSerializer? serializer = null)
00137:         {
00138:             var ratification = new Dictionary<string, int>(StringComparer.Ordinal);
00139:             files = files ?? new FileSystemIO();
00140:             serializer = serializer ?? new SystemTextJsonSerializer();
00141:             string path = Path.Combine(dataDirectory, AccordsFileName);
00142:             if (!files.FileExists(path)) return ratification;
00143:             string text = files.ReadAllText(path);
00144:             if (string.IsNullOrWhiteSpace(text)) return ratification;
00145:             try
00146:             {
00147:                 var file = serializer.Deserialize<RegionalTreatiesFile>(text);
00148:                 if (file?.treaties == null) return ratification;
00149:                 for (int i = 0; i < file.treaties.Count; i++)
00150:                 {
00151:                     var t = file.treaties[i];
00152:                     if (t != null && !string.IsNullOrEmpty(t.treaty_id) && t.ratified_day > 0)
00153:                         ratification[t.treaty_id] = t.ratified_day;
00154:                 }
00155:             }
00156:             catch (Exception ex_CATDIAG)
00157:             {
00158:                 CatalogDiagnostics.Warn(path, "RegionalTreatiesFile", ex_CATDIAG);
00159:                 return new Dictionary<string, int>(StringComparer.Ordinal);
00160:             }
00161:             return ratification;
00162:         }
00163:
00164:         public static FoundryProductionFile LoadProduction(
00165:             string dataDirectory,
00166: IFileIO? files = null,
00167: IJsonSerializer? serializer = null)
00168:         {
00169:             files = files ?? new FileSystemIO();
00170:             serializer = serializer ?? new SystemTextJsonSerializer();
00171:             string path = Path.Combine(dataDirectory, ProductionFileName);
00172:             if (!files.FileExists(path)) return new FoundryProductionFile();
00173:             string text = files.ReadAllText(path);
00174:             if (string.IsNullOrWhiteSpace(text)) return new FoundryProductionFile();
00175:             try
00176:             {
00177:                 return serializer.Deserialize<FoundryProductionFile>(text) ?? new FoundryProductionFile();
00178:             }
00179:             catch (Exception ex_CATDIAG)
00180:             {
00181:                 CatalogDiagnostics.Warn(path, "FoundryProductionFile", ex_CATDIAG);
00182:                 return new FoundryProductionFile();
00183:             }
00184:         }
00185:
00186:         public static FoundryFactionEntry? LoadFaction(
00187:             string dataDirectory,
00188: IFileIO? files = null,
00189: IJsonSerializer? serializer = null)
00190:         {
00191:             files = files ?? new FileSystemIO();
00192:             serializer = serializer ?? new SystemTextJsonSerializer();
00193:             string path = Path.Combine(dataDirectory, FactionFileName);
00194:             if (!files.FileExists(path)) return null;
00195:             string text = files.ReadAllText(path);
00196:             if (string.IsNullOrWhiteSpace(text)) return null;
00197:             try
00198:             {
00199:                 return serializer.Deserialize<FoundryFactionEntry>(text);
00200:             }
00201:             catch (Exception ex_CATDIAG)
00202:             {
00203:                 CatalogDiagnostics.Warn(path, "FoundryFactionEntry", ex_CATDIAG);
00204:                 return null;
00205:             }
00206:         }
00207:     }
00208:
00209:     /// <summary>
00210:     /// In-memory lookup surface over the static Foundry catalogs. Static data
00211:     /// only — mutable simulation state lives in SilentFoundrySystem.
00212:     /// </summary>
00213:     public sealed class SilentFoundryCatalog
00214:     {
00215:         private readonly Dictionary<string, FoundryProductEntry> _byProductId =
00216:             new Dictionary<string, FoundryProductEntry>(StringComparer.Ordinal);
00217:
00218:         private readonly List<FoundryProductEntry> _products = new List<FoundryProductEntry>();
00219:
00220:         public FoundryFactionEntry Faction { get; private set; }
00221:
00222:         public IReadOnlyList<FoundryProductEntry> AllProducts => _products;
00223:         public int ProductCount => _products.Count;
00224:
00225:         public void Load(FoundryProductionFile production, FoundryFactionEntry faction)
00226:         {
00227:             _byProductId.Clear();
00228:             _products.Clear();
00229:             Faction = faction;
00230:
00231:             if (production?.products == null) return;
00232:             foreach (var p in production.products)
00233:             {
00234:                 if (p == null || string.IsNullOrEmpty(p.product_id)) continue;
00235:                 if (!_byProductId.ContainsKey(p.product_id))
00236:                 {
00237:                     _byProductId[p.product_id] = p;
00238:                     _products.Add(p);
00239:                 }
00240:             }
00241:         }
00242:
00243:         /// <summary>
00244:         /// Merge Plan B66 heavy metallurgy recipes (projected as product
00245:         /// entries) into the bound catalog. Existing product ids are never
00246:         /// overwritten. Deterministic; safe to call again.
00247:         /// </summary>
00248:         public void MergeHeavyRecipes(IEnumerable<FoundryProductEntry> heavyProducts)
00249:         {
00250:             if (heavyProducts == null) return;
00251:             foreach (var p in heavyProducts)
00252:             {
00253:                 if (p == null || string.IsNullOrEmpty(p.product_id)) continue;
00254:                 if (_byProductId.ContainsKey(p.product_id)) continue;
00255:                 _byProductId[p.product_id] = p;
00256:                 _products.Add(p);
00257:             }
00258:         }
00259:
00260:         /// <summary>
00261:         /// Merge Plan B100 glassworks recipes into the same heat-machine
00262:         /// catalog. Existing product IDs are never overwritten and repeated
00263:         /// binding is deterministic.
00264:         /// </summary>
00265:         public void MergeGlassworksRecipes(IEnumerable<FoundryProductEntry> glassProducts)
00266:         {
00267:             if (glassProducts == null) return;
00268:             foreach (var p in glassProducts)
00269:             {
00270:                 if (p == null || string.IsNullOrEmpty(p.product_id)) continue;
00271:                 if (_byProductId.ContainsKey(p.product_id)) continue;
00272:                 _byProductId[p.product_id] = p;
00273:                 _products.Add(p);
00274:             }
00275:         }
00276:
00277:         public FoundryProductEntry? GetProduct(string productId)
00278:         {
00279:             if (string.IsNullOrEmpty(productId)) return null;
00280:             return _byProductId.TryGetValue(productId, out var entry) ? entry : null;
00281:         }
00282:
00283:         public List<FoundryProductEntry> GetByCategory(string category)
00284:         {
00285:             var results = new List<FoundryProductEntry>();
00286:             if (string.IsNullOrEmpty(category)) return results;
00287:             for (int i = 0; i < _products.Count; i++)
00288:                 if (string.Equals(_products[i].category, category, StringComparison.OrdinalIgnoreCase))
00289:                     results.Add(_products[i]);
00290:             return results;
00291:         }
00292:
00293:         /// <summary>Products that carry a treaty obligation (quota_amount > 0).</summary>
00294:         public List<FoundryProductEntry> GetQuotaProducts()
00295:         {
00296:             var results = new List<FoundryProductEntry>();
00297:             for (int i = 0; i < _products.Count; i++)
00298:                 if (_products[i].quota_amount > 0 && !string.IsNullOrEmpty(_products[i].treaty_id))
00299:                     results.Add(_products[i]);
00300:             return results;
00301:         }
00302:     }
00303: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` — complete current file

- Size: 689 lines / 34425 bytes.
- SHA-256: `0f779f80dada6c90e5cb5bfe305b5a9cfe253a6d12036b85044329a3ffff2caa`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: namespace Ashfall.Core.Foundry
00007: {
00008:     // ---------------------------------------------------------------------
00009:
00010:     /// <summary>
00011:     /// THE SILENT FOUNDRY (Expansion 10) — smelter-bay production, repair and
00012:     /// maintenance system. Owns the mutable simulation state; the static
00013:     /// production catalog and the blueprint remain authored catalogs and are
00014:     /// never mutated here.
00015:     ///
00016:     /// Identity is material continuity, not a weapon factory: agricultural
00017:     /// tools, structural beams, railway spikes and wheels, acid-resistant
00018:     /// pipes, repair plates, brackets, water components, heavy tools and
00019:     /// heavy-alloy parts. Every heat has a source (charge), a rule (catalog
00020:     /// product), labour/time/fuel/water costs, a quality outcome, inventory,
00021:     /// trade, treaty and persistence consequences.
00022:     ///
00023:     /// Determinism: all randomness flows through the injected ISeededRng
00024:     /// (default xorshift64*). Same seed ⇒ same outcomes. No System.Random.
00025:     /// </summary>
00026:     public sealed partial class SilentFoundrySystem
00027:     {
00028:         public const int DefaultSeed = 1009;
00029:         public const int MaxWorkers = 8;            // room_bp_11 max_dweller_capacity
00030:         public const float BlueprintBasePowerKw = 45f;
00031:         public const float BlueprintWaterFlowLpm = 40f;
00032:
00033:         // Event-id strings for bus forwarding (typed events are the primary surface).
00034:         public const string EventUnlocked = "silent_foundry_unlocked";
00035:         public const string EventRepairStarted = "silent_foundry_repair_started";
00036:         public const string EventRepaired = "silent_foundry_repaired";
00037:         public const string EventMaintenanceDue = "silent_foundry_maintenance_due";
00038:         public const string EventHeatPrepared = "silent_foundry_heat_prepared";
00039:         public const string EventHeatStarted = "silent_foundry_heat_started";
00040:         public const string EventHeatCompleted = "silent_foundry_heat_completed";
00041:         public const string EventCastCompleted = "silent_foundry_cast_completed";
00042:         public const string EventCastFailed = "silent_foundry_cast_failed";
00043:         public const string EventSafetyWarning = "silent_foundry_safety_warning";
00044:         public const string EventIncident = "silent_foundry_incident";
00045:         public const string EventTreatyQuotaMet = "silent_foundry_treaty_quota_met";
00046:         public const string EventTreatyQuotaMissed = "silent_foundry_treaty_quota_missed";
00047:         public const string EventConsequenceApplied = "silent_foundry_treaty_consequence_applied";
00048:         public const string EventLaborDispute = "silent_foundry_labor_dispute";
00049:         public const string EventStrikeStarted = "silent_foundry_strike_started";
00050:         public const string EventStrikeResolved = "silent_foundry_strike_resolved";
00051:         public const string EventBlueprintReferenced = "silent_foundry_blueprint_referenced";
00052:         public const string EventJournalTriggered = "silent_foundry_journal_triggered";
00053:
00054:         // Typed events (established convention — no third bus).
00055:         public event Action<SilentFoundryState> OnStateChanged;
00056:         public event Action<FoundryProductionRecord> OnProductionCompleted;
00057:         public event Action<FoundryFailedCastRecord> OnCastFailed;
00058:         public event Action<string> OnSafetyWarning;
00059:         public event Action<FoundryIncidentRecord> OnIncident;
00060:         public event Action<FoundryTreatyCompliance> OnTreatyQuotaMet;
00061:         public event Action<FoundryTreatyCompliance> OnTreatyQuotaMissed;
00062:         /// <summary>Fired once per assessment cycle when a policy consequence is applied.</summary>
00063:         public event Action<FoundryConsequenceRecord> OnConsequenceApplied;
00064:         public event Action<FoundryLaborDispute, int> OnLaborDisputeChanged;
00065:         public event Action<FoundryStrikeResolution, int> OnStrikeResolved;
00066:         public event Action<FoundryJournalTrigger> OnJournalTriggered;
00067:         /// <summary>Forwarder for the string event bus (optional).</summary>
00068:         public event Action<string> OnEventRaised;
00069:
00070:         private readonly SilentFoundryState _state;
00071:         private ISeededRng _rng;
00072:         private readonly Func<int, ISeededRng> _rngFactory;
00073:         private readonly ILog _log;
00074:         private SilentFoundryCatalog _catalog = new SilentFoundryCatalog();
00075:         private readonly Dictionary<string, int> _treatyRatificationDays = new Dictionary<string, int>(StringComparer.Ordinal);
00076:         private SilentFoundryConsequencePolicyCatalog _consequencePolicy = new SilentFoundryConsequencePolicyCatalog();
00077:         private readonly SilentFoundryConsequenceState _consequenceState = new SilentFoundryConsequenceState();
00078:
00079:         // Inventory ports (wired by host; deterministic, no host objects here).
00080:         private Func<string, int> _getCount = _ => 0;
00081:         private Func<string, int, bool> _canAdd = (_, _) => false;
00082:         private Action<string, int> _addItem = (_, _) => { };
00083:         private Action<string, int> _consume = (_, _) => { };
00084:
00085:         /// <summary>Standing range mirrors the existing FactionStanceConstants [-100, 100].</summary>
00086:         public const float StandingMin = -100f;
00087:         public const float StandingMax = 100f;
00088:         public const float StandingNeutral = 0f;
00089:
00090:         public SilentFoundrySystem(
00091: SilentFoundryState? state = null,
00092: ISeededRng? rng = null,
00093: Func<int, ISeededRng>? rngFactory = null,
00094: ILog? log = null)
00095:         {
00096:             _rngFactory = rngFactory ?? (seed => new SeededRng(seed));
00097:             _state = state ?? new SilentFoundryState();
00098:             _rng = rng ?? _rngFactory(_state.rngSeed == 0 ? DefaultSeed : _state.rngSeed);
00099:             _state.rngSeed = _rng.Seed;
00100:             _log = log ?? NullLog.Instance;
00101:             NormalizeState();
00102:         }
00103:
00104:         private void NormalizeState()
00105:         {
00106:             if (_state.stateVersion < 1 || _state.stateVersion > SilentFoundryState.CurrentVersion)
00107:                 _state.stateVersion = SilentFoundryState.CurrentVersion;
00108:             if (_state.completed == null) _state.completed = new List<FoundryProductionRecord>();
00109:             if (_state.failed == null) _state.failed = new List<FoundryFailedCastRecord>();
00110:             if (_state.incidents == null) _state.incidents = new List<FoundryIncidentRecord>();
00111:             if (_state.repairs == null) _state.repairs = new List<FoundryRepairRecord>();
00112:             if (_state.treatyCompliance == null) _state.treatyCompliance = new List<FoundryTreatyCompliance>();
00113:             if (_state.triggeredJournals == null) _state.triggeredJournals = new List<string>();
00114:         }
00115:
00116:         // -----------------------------------------------------------------
00117:         // Binding
00118:         // -----------------------------------------------------------------
00119:
00120:         /// <summary>
00121:         /// Bind the static production catalog and blueprint-derived constants.
00122:         /// The blueprint is static authored data — only its typed values are read.
00123:         /// </summary>
00124:         public void BindCatalog(SilentFoundryCatalog catalog, int maintenanceCycleDaysFromBlueprint)
00125:         {
00126:             if (catalog != null) _catalog = catalog;
00127:             _state.maintenanceCycleDays = maintenanceCycleDaysFromBlueprint > 0
00128:                 ? maintenanceCycleDaysFromBlueprint
00129:                 : 4;
00130:             Raise(EventBlueprintReferenced, "blueprint room_bp_11_the_silent_foundry_smelter_bay referenced; maintenance cycle "
00131:                 + _state.maintenanceCycleDays + "d");
00132:         }
00133:
00134:         /// <summary>Bind treaty ratification-day anchors (exact ids from RegionalTreatyCatalog).</summary>
00135:         public void BindTreaties(IReadOnlyDictionary<string, int> ratificationDaysById)
00136:         {
00137:             if (ratificationDaysById == null) return;
00138:             _treatyRatificationDays.Clear();
00139:             foreach (var kvp in ratificationDaysById)
00140:             {
00141:                 if (!string.IsNullOrEmpty(kvp.Key) && kvp.Value > 0)
00142:                     _treatyRatificationDays[kvp.Key] = kvp.Value;
00143:             }
00144:             EnsureTreatyComplianceRows();
00145:         }
00146:
00147:         /// <summary>
00148:         /// Bind the authored consequence policy catalog. Without a bound policy
00149:         /// no consequences are applied (treaty assessment still records met/missed).
00150:         /// </summary>
00151:         public void BindConsequencePolicy(SilentFoundryConsequencePolicyCatalog policy)
00152:         {
00153:             if (policy == null) return;
00154:             _consequencePolicy = policy;
00155:             if (policy.HasErrors)
00156:             {
00157:                 for (int i = 0; i < policy.Errors.Count; i++)
00158:                     _log.Warn("[SilentFoundry] consequence policy: " + policy.Errors[i]);
00159:             }
00160:         }
00161:
00162:         /// <summary>Bind inventory ports. Defaults to a sealed (no-op) inventory.</summary>
00163:         public void BindInventory(
00164:             Func<string, int> getCount,
00165:             Func<string, int, bool> canAdd,
00166:             Action<string, int> addItem,
00167:             Action<string, int> consume)
00168:         {
00169:             if (getCount != null) _getCount = getCount;
00170:             if (canAdd != null) _canAdd = canAdd;
00171:             if (addItem != null) _addItem = addItem;
00172:             if (consume != null) _consume = consume;
00173:         }
00174:
00175:         // -----------------------------------------------------------------
00176:         // State queries
00177:         // -----------------------------------------------------------------
00178:
00179:         public SilentFoundryState State => _state;
00180:         public SilentFoundryCatalog Catalog => _catalog;
00181:         public bool IsUnlocked => _state.unlocked;
00182:         public FoundryHeatStage HeatStage => _state.heatStage;
00183:         public FoundryLaborDispute LaborDispute => _state.laborDispute;
00184:         public bool IsMaintenanceOverdue => _state.daysSinceMaintenance > _state.maintenanceCycleDays;
00185:         public int DaysOverdue => Math.Max(0, _state.daysSinceMaintenance - _state.maintenanceCycleDays);
00186:         public int OverdueCycles => _state.maintenanceCycleDays > 0
00187:             ? DaysOverdue / _state.maintenanceCycleDays : 0;
00188:
00189:         public float GetComponentCondition(FoundryFacilityComponent component)
00190:         {
00191:             switch (component)
00192:             {
00193:                 case FoundryFacilityComponent.RefractoryLining: return _state.refractoryLining;
00194:                 case FoundryFacilityComponent.HearthTuyeres: return _state.hearthTuyeres;
00195:                 case FoundryFacilityComponent.SandBeds: return _state.sandBeds;
00196:                 case FoundryFacilityComponent.StructuralSupports: return _state.structuralSupports;
00197:                 case FoundryFacilityComponent.SafetyExhaust: return _state.safetyExhaust;
00198:                 default: return 0f;
00199:             }
00200:         }
00201:
00202:         public float AverageFacilityCondition()
00203:         {
00204:             return (GetComponentCondition(FoundryFacilityComponent.RefractoryLining)
00205:                 + GetComponentCondition(FoundryFacilityComponent.HearthTuyeres)
00206:                 + GetComponentCondition(FoundryFacilityComponent.SandBeds)
00207:                 + GetComponentCondition(FoundryFacilityComponent.StructuralSupports)
00208:                 + GetComponentCondition(FoundryFacilityComponent.SafetyExhaust)) / 5f;
00209:         }
00210:
00211:         public bool IsJournalTriggered(string templateId) =>
00212:             _state.triggeredJournals != null && _state.triggeredJournals.Contains(templateId);
00213:
00214:         public FoundryTreatyCompliance? GetTreatyCompliance(string treatyId)
00215:         {
00216:             if (_state.treatyCompliance == null) return null;
00217:             for (int i = 0; i < _state.treatyCompliance.Count; i++)
00218:                 if (_state.treatyCompliance[i] != null
00219:                     && string.Equals(_state.treatyCompliance[i].treatyId, treatyId, StringComparison.Ordinal))
00220:                     return _state.treatyCompliance[i];
00221:             return null;
00222:         }
00223:
00224:         public IReadOnlyList<FoundryProductionRecord> CompletedProduction => _state.completed;
00225:         public IReadOnlyList<FoundryFailedCastRecord> FailedCasts => _state.failed;
00226:         public IReadOnlyList<FoundryIncidentRecord> Incidents => _state.incidents;
00227:         public int TotalProductionCount => _state.completed.Count;
00228:         public int TotalFailedCount => _state.failed.Count;
00229:         public float CumulativeStress => _state.cumulativeStress;
00230:         public float CumulativeHope => _state.cumulativeHope;
00231:
00232:         public bool IsHeatActive => HeatStage != FoundryHeatStage.Idle && HeatStage != FoundryHeatStage.Complete;
00233:
00234:         /// <summary>Electrical power demand in kW for the active heat stage.</summary>
00235:         public float CurrentPowerDemandKw => HeatStage switch
00236:         {
00237:             FoundryHeatStage.ChargeLoaded => 3.0f,
00238:             FoundryHeatStage.Preheat => 15.0f,
00239:             FoundryHeatStage.AtHeat => 22.0f,
00240:             FoundryHeatStage.Tapped => 18.0f,
00241:             FoundryHeatStage.Casting => 16.0f,
00242:             FoundryHeatStage.Cooling => 4.0f,
00243:             _ => 0f
00244:         };
00245:
00246:         /// <summary>Useful waste heat in kW emitted into adjacent shelter facilities.</summary>
00247:         public float CurrentWasteHeatKw => HeatStage switch
00248:         {
00249:             FoundryHeatStage.ChargeLoaded => 2.0f,
00250:             FoundryHeatStage.Preheat => 12.0f,
00251:             FoundryHeatStage.AtHeat => 25.0f,
00252:             FoundryHeatStage.Tapped => 22.0f,
00253:             FoundryHeatStage.Casting => 18.0f,
00254:             FoundryHeatStage.Cooling => 6.0f,
00255:             _ => 0f
00256:         };
00257:
00258:         /// <summary>
00259:         /// Suspend or abort active heat due to electrical grid brownout or lack of power.
00260:         /// </summary>
00261:         public void SuspendHeat(string reason, int day)
00262:         {
00263:             if (!IsHeatActive) return;
00264:             var product = _catalog.GetProduct(_state.activeProductId);
00265:             _state.failed.Add(new FoundryFailedCastRecord
00266:             {
00267:                 productId = _state.activeProductId,
00268:                 displayName = product?.display_name ?? _state.activeProductId,
00269:                 reason = "Heat aborted due to grid power failure: " + reason,
00270:                 failedDay = day,
00271:                 materialsLost = _state.materialsConsumed
00272:             });
00273:             _state.heatStage = FoundryHeatStage.Idle;
00274:             _state.activeProductId = string.Empty;
00275:             _state.assignedWorkers = 0;
00276:             _state.laborAccumulated = 0f;
00277:             Raise(EventCastFailed, "heat aborted: power grid failure (day " + day + ")");
00278:             OnSafetyWarning?.Invoke("Foundry heat aborted: electrical power failure.");
00279:             RaiseStateChanged();
00280:         }
00281:
00282:         // ── Treaty consequence queries ─────────────────────────────────
00283:
00284:         /// <summary>Authoritative net standing of the Foundry Guild from its treaties.</summary>
00285:         public float GuildStanding => _consequenceState.guildStanding;
00286:
00287:         /// <summary>Idempotency + audit ledger of applied consequences.</summary>
00288:         public IReadOnlyList<FoundryConsequenceRecord> AppliedConsequences => _consequenceState.applied;
00289:
00290:         public bool IsConsequenceApplied(string treatyId, int cycleMarker)
00291:             => _consequenceState.IsApplied(treatyId, cycleMarker);
00292:
00293:         /// <summary>
00294:         /// Derive the current outcome state for a treaty. NotRatified and Pending
00295:         /// are neutral; the last applied consequence (if any) carries the outcome.
00296:         /// Pure derivation — no mutable state.
00297:         /// </summary>
00298:         public FoundryTreatyOutcome GetTreatyOutcome(string treatyId, int day)
00299:         {
00300:             if (string.IsNullOrEmpty(treatyId)) return FoundryTreatyOutcome.NotRatified;
00301:             int ratificationDay = _treatyRatificationDays.TryGetValue(treatyId, out int rDay) ? rDay : 0;
00302:             if (ratificationDay <= 0 || day < ratificationDay) return FoundryTreatyOutcome.NotRatified;
00303:
00304:             var c = GetTreatyCompliance(treatyId);
00305:             if (c == null || c.lastAssessmentDay == 0) return FoundryTreatyOutcome.Pending;
00306:
00307:             // The most recent applied consequence for this treaty is the outcome.
00308:             FoundryConsequenceRecord? latest = null;
00309:             for (int i = 0; i < _consequenceState.applied.Count; i++)
00310:             {
00311:                 var r = _consequenceState.applied[i];
00312:                 if (r != null && string.Equals(r.treatyId, treatyId, StringComparison.Ordinal)
00313:                     && (latest == null || r.appliedDay > latest.appliedDay))
00314:                     latest = r;
00315:             }
00316:             if (latest != null) return latest.outcome;
00317:
00318:             // Assessed but no policy consequence exists (e.g. treaty_16) — fall
00319:             // back to the compliance row's coarse signal.
00320:             return c.missedCount > c.metCount ? FoundryTreatyOutcome.Missed
00321:                 : c.metCount > 0 ? FoundryTreatyOutcome.Met
00322:                 : FoundryTreatyOutcome.Pending;
00323:         }
00324:
00325:         // -----------------------------------------------------------------
00326:         // Unlock & facilities
00327:         // -----------------------------------------------------------------
00328:
00329:         /// <summary>Unlock the Foundry. Idempotent; raises the unlock event once.</summary>
00330:         public bool Unlock(int day)
00331:         {
00332:             if (_state.unlocked) return false;
00333:             _state.unlocked = true;
00334:             _state.unlockDay = day;
00335:             EnsureTreatyComplianceRows();
00336:             Raise(EventUnlocked, "The Silent Foundry is open (day " + day + ").");
00337:             RaiseStateChanged();
00338:             return true;
00339:         }
00340:
00341:         /// <summary>
00342:         /// Repair one facility component. Consumes firebrick + labour + time
00343:         /// (the repair happens over a full day). Returns a human-readable reason
00344:         /// when it cannot start.
00345:         /// </summary>
00346:         public string StartRepair(FoundryFacilityComponent component, int day)
00347:         {
00348:             if (!_state.unlocked) return "The Foundry is not unlocked.";
00349:             if (HeatStage != FoundryHeatStage.Idle && HeatStage != FoundryHeatStage.Complete)
00350:                 return "A heat is in progress; repairs cannot start on the active furnace floor.";
00351:
00352:             int firebrickCost = ComponentRepairCost(component);
00353:             if (_getCount(SilentFoundryIds.ItemFirebrick) < firebrickCost)
00354:                 return "Not enough firebrick (" + firebrickCost + " required, " + _getCount(SilentFoundryIds.ItemFirebrick) + " held).";
00355:
00356:             _consume(SilentFoundryIds.ItemFirebrick, firebrickCost);
00357:             float before = GetComponentCondition(component);
00358:             float restored = 100f;
00359:             switch (component)
00360:             {
00361:                 case FoundryFacilityComponent.RefractoryLining: _state.refractoryLining = restored; break;
00362:                 case FoundryFacilityComponent.HearthTuyeres: _state.hearthTuyeres = restored; break;
00363:                 case FoundryFacilityComponent.SandBeds: _state.sandBeds = restored; break;
00364:                 case FoundryFacilityComponent.StructuralSupports: _state.structuralSupports = restored; break;
00365:                 case FoundryFacilityComponent.SafetyExhaust: _state.safetyExhaust = restored; break;
00366:             }
00367:             _state.repairs.Add(new FoundryRepairRecord
00368:             {
00369:                 component = component.ToString(),
00370:                 day = day,
00371:                 conditionBefore = before,
00372:                 conditionAfter = restored
00373:             });
00374:             Raise(EventRepairStarted, "repair started on " + component + " (day " + day + ")");
00375:             Raise(EventRepaired, component.ToString() + " restored to " + restored.ToString("F0"));
00376:             RaiseStateChanged();
00377:             return "Repair complete: " + component + " restored to 100.";
00378:         }
00379:
00380:         /// <summary>Perform full maintenance. Resets the 4-day cycle.</summary>
00381:         public string PerformMaintenance(int day)
00382:         {
00383:             if (!_state.unlocked) return "The Foundry is not unlocked.";
00384:             _state.maintenanceDueDay = day + _state.maintenanceCycleDays;
00385:             _state.daysSinceMaintenance = 0;
00386:             _state.maintenancePerformed++;
00387:             // Service restores a little wear without a full rebuild.
00388:             _state.refractoryLining = Math.Min(100f, _state.refractoryLining + 6f);
00389:             _state.hearthTuyeres = Math.Min(100f, _state.hearthTuyeres + 6f);
00390:             _state.safetyExhaust = Math.Min(100f, _state.safetyExhaust + 6f);
00391:             Raise(EventRepaired, "full maintenance performed; next due day " + _state.maintenanceDueDay);
00392:             RaiseStateChanged();
00393:             return "Maintenance performed. Next service due day " + _state.maintenanceDueDay + ".";
00394:         }
00395:
00396:         private static int ComponentRepairCost(FoundryFacilityComponent component)
00397:         {
00398:             switch (component)
00399:             {
00400:                 case FoundryFacilityComponent.RefractoryLining: return 8;
00401:                 case FoundryFacilityComponent.HearthTuyeres: return 10;
00402:                 case FoundryFacilityComponent.SandBeds: return 4;
00403:                 case FoundryFacilityComponent.StructuralSupports: return 12;
00404:                 case FoundryFacilityComponent.SafetyExhaust: return 6;
00405:                 default: return 6;
00406:             }
00407:         }
00408:
00409:         // -----------------------------------------------------------------
00410:         // Green-sand casting bed
00411:         // -----------------------------------------------------------------
00412:
00413:         /// <summary>
00414:         /// Replenish/refresh the sand bed: consumes green sand (and optionally
00415:         /// water) and resets the reuse counter. Player choice: preserve scarce
00416:         /// high-quality sand for a critical cast or refresh now.
00417:         /// </summary>
00418:         public string PrepareSand(int waterLitres)
00419:         {
00420:             if (!_state.unlocked) return "The Foundry is not unlocked.";
00421:             if (HeatStage != FoundryHeatStage.Idle && HeatStage != FoundryHeatStage.Complete)
00422:                 return "Cannot prepare sand while a heat is active.";
00423:
00424:             int sandNeeded = 2;
00425:             if (_getCount(SilentFoundryIds.ItemGreenSand) < sandNeeded)
00426:                 return "Not enough green sand (" + sandNeeded + " required).";
00427:
00428:             int waterAvailable = _getCount(SilentFoundryIds.ItemCleanWater);
00429:             if (waterAvailable < waterLitres)
00430:                 return "Not enough clean water (" + waterLitres + " required, " + waterAvailable + " held).";
00431:
00432:             _consume(SilentFoundryIds.ItemGreenSand, sandNeeded);
00433:             if (waterLitres > 0) _consume(SilentFoundryIds.ItemCleanWater, waterLitres);
00434:
00435:             _state.sandQuality = Math.Min(100f, _state.sandQuality + 12f);
00436:             _state.sandMoisture = MathfCompat.Clamp(60f + waterLitres * 0.1f, 0f, 100f);
00437:             _state.binderQuality = Math.Min(100f, _state.binderQuality + 6f);
00438:             _state.contamination = Math.Max(0f, _state.contamination - 15f);
00439:             _state.moldReuseCount = 0;
00440:             _state.compaction = 70f;
00441:             RaiseStateChanged();
00442:             return "Sand bed refreshed: quality " + _state.sandQuality.ToString("F0")
00443:                 + ", moisture " + _state.sandMoisture.ToString("F0") + ".";
00444:         }
00445:
00446:         /// <summary>Compact the mold with the available skill; raises compaction.</summary>
00447:         public string CompactMold(float skill)
00448:         {
00449:             if (!_state.unlocked) return "The Foundry is not unlocked.";
00450:             if (HeatStage != FoundryHeatStage.Idle && HeatStage != FoundryHeatStage.Complete)
00451:                 return "Cannot work the mold while a heat is active.";
00452:             _state.compaction = MathfCompat.Clamp(_state.compaction + 20f * MathfCompat.Clamp(skill, 0f, 1f), 0f, 100f);
00453:             _state.patternQuality = MathfCompat.Clamp(_state.patternQuality + 4f * skill, 0f, 100f);
00454:             RaiseStateChanged();
00455:             return "Mold compacted. Compaction " + _state.compaction.ToString("F0") + ".";
00456:         }
00457:
00458:         // -----------------------------------------------------------------
00459:
00460:         // Treaty compliance
00461:         // -----------------------------------------------------------------
00462:
00463:         private void EnsureTreatyComplianceRows()
00464:         {
00465:             if (_state.treatyCompliance == null) _state.treatyCompliance = new List<FoundryTreatyCompliance>();
00466:
00467:             void Ensure(string treatyId, string obligation)
00468:             {
00469:                 if (GetTreatyCompliance(treatyId) == null)
00470:                 {
00471:                     _state.treatyCompliance.Add(new FoundryTreatyCompliance
00472:                     {
00473:                         treatyId = treatyId,
00474:                         obligation = obligation,
00475:                         quotaDeadlineDay = _treatyRatificationDays.TryGetValue(treatyId, out int day) ? day : 0
00476:                     });
00477:                 }
00478:             }
00479:
00480:             Ensure(SilentFoundryIds.TreatyBrinePipe, "brine_pipe_quota");
00481:             Ensure(SilentFoundryIds.TreatyLabourSchedule, "labor_shifts");
00482:             Ensure(SilentFoundryIds.TreatyRoadIron, "road_iron_quota");
00483:             Ensure(SilentFoundryIds.TreatyClusterCharter, "charter_eligibility");
00484:         }
00485:
00486:         private void ApplyQuotaFulfilment(FoundryProductEntry product, int amount)
00487:         {
00488:             if (string.IsNullOrEmpty(product.treaty_id) || product.quota_amount <= 0) return;
00489:             var c = GetTreatyCompliance(product.treaty_id);
00490:             if (c == null) return;
00491:             c.quotaFulfilled += amount;
00492:             c.currentCycleMet = c.quotaFulfilled >= c.quotaTotal;
00493:         }
00494:
00495:         /// <summary>
00496:         /// Evaluate every tracked treaty. Assessment days are derived from each
00497:         /// treaty's authored ratification day plus its assessment cycle (30 days).
00498:         /// Day-agnostic — works for synthetic days and long campaigns alike.
00499:         /// </summary>
00500:         public void AssessTreatyCompliance(int day)
00501:         {
00502:             if (_state.treatyCompliance == null) return;
00503:
00504:             // Initialize quota totals from the catalog each assessment.
00505:             for (int i = 0; i < _state.treatyCompliance.Count; i++)
00506:             {
00507:                 var c = _state.treatyCompliance[i];
00508:                 if (c == null) continue;
00509:                 if (c.quotaTotal == 0 && !string.IsNullOrEmpty(c.treatyId))
00510:                 {
00511:                     int total = 0;
00512:                     for (int p = 0; p < _catalog.AllProducts.Count; p++)
00513:                     {
00514:                         var prod = _catalog.AllProducts[p];
00515:                         if (prod != null && !string.IsNullOrEmpty(prod.treaty_id)
00516:                             && string.Equals(prod.treaty_id, c.treatyId, StringComparison.Ordinal)
00517:                             && prod.quota_amount > 0)
00518:                         {
00519:                             total += prod.quota_amount;
00520:                         }
00521:                     }
00522:                     c.quotaTotal = total;
00523:                 }
00524:             }
00525:
00526:             for (int i = 0; i < _state.treatyCompliance.Count; i++)
00527:             {
00528:                 var c = _state.treatyCompliance[i];
00529:                 if (c == null || string.IsNullOrEmpty(c.treatyId)) continue;
00530:
00531:                 int ratificationDay = _treatyRatificationDays.TryGetValue(c.treatyId, out int rDay) ? rDay : 0;
00532:                 if (ratificationDay <= 0) continue;
00533:                 if (day < ratificationDay) continue;
00534:
00535:                 const int assessmentCycle = 30;
00536:                 int assessmentDay = ratificationDay + ((day - ratificationDay) / assessmentCycle) * assessmentCycle;
00537:                 if (assessmentDay == day && day != c.lastAssessmentDay)
00538:                 {
00539:                     AssessOne(c, day);
00540:                     c.lastAssessmentDay = day;
00541:                 }
00542:
00543:                 // Cluster-charter eligibility: late-campaign marker, derived not asserted.
00544:                 // (Serialized field name `constitutionEligible` kept for save compatibility.)
00545:                 if (string.Equals(c.treatyId, SilentFoundryIds.TreatyClusterCharter, StringComparison.Ordinal))
00546:                 {
00547:                     c.constitutionEligible = _state.incidents.Count == 0 || day < ratificationDay;
00548:                 }
00549:             }
00550:         }
00551:
00552:         private void AssessOne(FoundryTreatyCompliance c, int day)
00553:         {
00554:             switch (c.obligation)
00555:             {
00556:                 case "road_iron_quota":
00557:                 case "brine_pipe_quota":
00558:                     if (c.quotaTotal > 0)
00559:                     {
00560:                         c.currentCycleMet = c.quotaFulfilled >= c.quotaTotal;
00561:                         if (c.currentCycleMet)
00562:                         {
00563:                             c.metCount++;
00564:                             Raise(EventTreatyQuotaMet, c.treatyId + " quota met day " + day);
00565:                             OnTreatyQuotaMet?.Invoke(c);
00566:                             ApplyConsequence(c.treatyId, FoundryTreatyOutcome.Met, day);
00567:                         }
00568:                         else
00569:                         {
00570:                             c.missedCount++;
00571:                             Raise(EventTreatyQuotaMissed, c.treatyId + " quota missed day " + day
00572:                                 + " (" + c.quotaFulfilled + "/" + c.quotaTotal + ")");
00573:                             OnTreatyQuotaMissed?.Invoke(c);
00574:                             ApplyConsequence(c.treatyId, FoundryTreatyOutcome.Missed, day);
00575:                         }
00576:                         c.quotaFulfilled = 0; // next cycle starts fresh
00577:                         c.currentCycleMet = false;
00578:                     }
00579:                     break;
00580:
00581:                 case "labor_shifts":
00582:                     // The labour schedule accord: shifts capped at 8h during
00583:                     // liquid pours; workers hold a water ration; lockouts and
00584:                     // strikes close the coal window. A strike or open overtime
00585:                     // is a violation.
00586:                     // Fatigue alone is never a violation — only the policy-level
00587:                     // labor semantics (strike / overtime / child labor) count.
00588:                     bool violation = _state.laborDispute == FoundryLaborDispute.StrikeActive
00589:                         || _state.overtimeFlag || _state.childLaborUsed;
00590:                     if (violation)
00591:                     {
00592:                         c.missedCount++;
00593:                         Raise(EventTreatyQuotaMissed, c.treatyId + " labor-shift violation day " + day);
00594:                         OnTreatyQuotaMissed?.Invoke(c);
00595:                         ApplyConsequence(c.treatyId, FoundryTreatyOutcome.Violated, day);
00596:                     }
00597:                     else
00598:                     {
00599:                         c.metCount++;
00600:                         Raise(EventTreatyQuotaMet, c.treatyId + " labor accord upheld day " + day);
00601:                         OnTreatyQuotaMet?.Invoke(c);
00602:                         ApplyConsequence(c.treatyId, FoundryTreatyOutcome.Met, day);
00603:                     }
00604:                     break;
00605:
00606:                 case "charter_eligibility":
00607:                     // The Cluster Charter is a finale marker, not a quota.
00608:                     // Eligibility is derived; no economy or standing consequence
00609:                     // is defined for it by policy (regression-guarded in tests).
00610:                     break;
00611:             }
00612:         }
00613:
00614:         /// <summary>
00615:         /// Apply the authored policy consequence for a treaty outcome — once per
00616:         /// assessment cycle. Idempotency is keyed by (treatyId, cycleMarker) and
00617:         /// the cycleMarker is the assessment day, so reloading a save or calling
00618:         /// AssessTreatyCompliance again on the same day never re-applies.
00619:         /// Pre-ratification and pending states are neutral by construction: this
00620:         /// is only reached from AssessOne, which the ratification gate already
00621:         /// guards.
00622:         /// </summary>
00623:         private void ApplyConsequence(string treatyId, FoundryTreatyOutcome outcome, int day)
00624:         {
00625:             if (string.IsNullOrEmpty(treatyId)) return;
00626:             if (outcome != FoundryTreatyOutcome.Met
00627:                 && outcome != FoundryTreatyOutcome.Missed
00628:                 && outcome != FoundryTreatyOutcome.Violated) return;
00629:
00630:             int cycleMarker = day;
00631:             if (_consequenceState.IsApplied(treatyId, cycleMarker)) return;
00632:
00633:             var policy = _consequencePolicy.Find(treatyId, outcome);
00634:             if (policy == null)
00635:             {
00636:                 // No authored policy → the outcome is recorded but carries no
00637:                 // consequence (e.g. treaty_16 has no policy by design).
00638:                 return;
00639:             }
00640:
00641:             float nextStanding = MathfCompat.Clamp(
00642:                 _consequenceState.guildStanding + policy.standing_delta, StandingMin, StandingMax);
00643:             _consequenceState.guildStanding = nextStanding;
00644:
00645:             var record = new FoundryConsequenceRecord
00646:             {
00647:                 treatyId = treatyId,
00648:                 outcome = outcome,
00649:                 appliedDay = day,
00650:                 cycleMarker = cycleMarker,
00651:                 standingDelta = policy.standing_delta,
00652:                 reason = policy.reason,
00653:                 modifiers = new List<FoundryGoodModifier>()
00654:             };
00655:             if (policy.market_modifiers != null)
00656:             {
00657:                 for (int i = 0; i < policy.market_modifiers.Count; i++)
00658:                 {
00659:                     if (policy.market_modifiers[i] == null) continue;
00660:                     record.modifiers.Add(new FoundryGoodModifier
00661:                     {
00662:                         good_id = policy.market_modifiers[i].good_id,
00663:                         demand_delta = policy.market_modifiers[i].demand_delta,
00664:                         reason = policy.market_modifiers[i].reason
00665:                     });
00666:                 }
00667:             }
00668:             _consequenceState.applied.Add(record);
00669:
00670:             Raise(EventConsequenceApplied, treatyId + " " + SilentFoundryConsequencePolicyCatalog.OutcomeName(outcome)
00671:                 + " day " + day + " standing " + policy.standing_delta.ToString("F0"));
00672:             OnConsequenceApplied?.Invoke(record);
00673:         }
00674:
00675:         // -----------------------------------------------------------------
00676:
00677:         // Internals
00678:         // -----------------------------------------------------------------
00679:
00680:         private void Raise(string eventId, string message)
00681:         {
00682:             OnEventRaised?.Invoke(eventId);
00683:             _log.Info("[SilentFoundry] " + eventId + " — " + message);
00684:         }
00685:
00686:         private void RaiseStateChanged() => OnStateChanged?.Invoke(_state);
00687:     }
00688: }
00689:
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Foundry/SilentFoundryTypes.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundryTypes.cs` — complete current file

- Size: 282 lines / 10679 bytes.
- SHA-256: `519c0399c514e2bae74c69de64a1644410c1e0647f825381b5ae487219f10d95`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: namespace Ashfall.Core.Foundry
00007: {
00008:     // ---------------------------------------------------------------------
00009:     // Identity (exact ids from the authored data — never aliased)
00010:     // ---------------------------------------------------------------------
00011:
00012:     public static class SilentFoundryIds
00013:     {
00014:         public const string ExpansionId = "exp_10_the_silent_foundry";
00015:         public const string FactionId = "faction_silent_foundry";
00016:         public const string BlueprintRoomId = "room_bp_11_the_silent_foundry_smelter_bay";
00017:
00018:         public const string JournalFirstHeat = "jrnl_foundry_first_heat";
00019:         public const string JournalStrike = "jrnl_foundry_strike";
00020:
00021:         // District 8 accords (data authority: foundry_accords.json).
00022:         public const string TreatyBrinePipe = "treaty_brine_pipe_and_iodine_exchange";
00023:         public const string TreatyLabourSchedule = "treaty_cluster_labour_schedule";
00024:         public const string TreatyRoadIron = "treaty_road_iron_charter";
00025:         public const string TreatyClusterCharter = "treaty_the_cluster_charter";
00026:         public const string TreatySaltworksAccess = "treaty_saltworks_access";
00027:         public const string TreatyMembraneRepair = "treaty_membrane_repair";
00028:         public const string TreatyCoalWindow = "treaty_coal_window";
00029:         public const string TreatyApprenticeExchange = "treaty_apprentice_exchange";
00030:         public const string TreatyCrisisMutualAid = "treaty_crisis_mutual_aid";
00031:         public const string TreatyIncidentBook = "treaty_the_incident_book";
00032:
00033:         // Charge/consumable material ids (items.json / foundry_items.json).
00034:         public const string ItemScrapMetal = "scrap_metal";
00035:         public const string ItemCoal = "coal";
00036:         public const string ItemCharcoal = "charcoal";
00037:         public const string ItemCleanWater = "clean_water";
00038:         public const string ItemFirebrick = "item_foundry_firebrick";
00039:         public const string ItemGreenSand = "item_foundry_green_sand";
00040:         public const string ItemFlux = "item_foundry_flux";
00041:         public const string ItemAlloyAdditive = "item_foundry_alloy_additive";
00042:
00043:         // Plan B66 — heavy metallurgy expansion.
00044:         public const string ItemScrapMechanical = "scrap_mechanical";
00045:         public const string ItemCopperWire = "copper_wire_10m_of_10m";
00046:         public const string ItemShoringBracket = "item_foundry_shoring_bracket";
00047:         public const string CategoryHeavyMetallurgy = "heavy_metallurgy";
00048:     }
00049:
00050:     // ---------------------------------------------------------------------
00051:     // Domain enums
00052:     // ---------------------------------------------------------------------
00053:
00054:     /// <summary>Heat lifecycle stages. Deterministic, prerequisite-gated.</summary>
00055:     public enum FoundryHeatStage
00056:     {
00057:         Idle = 0,
00058:         ChargeLoaded = 1,
00059:         Preheat = 2,
00060:         AtHeat = 3,
00061:         Tapped = 4,
00062:         Casting = 5,
00063:         Cooling = 6,
00064:         Complete = 7
00065:     }
00066:
00067:     /// <summary>Labor dispute ladder. Escalates only from real conflicts.</summary>
00068:     public enum FoundryLaborDispute
00069:     {
00070:         None = 0,
00071:         Tensions = 1,
00072:         StrikeActive = 2,
00073:         Resolved = 3
00074:     }
00075:
00076:     public enum FoundryQualityTier
00077:     {
00078:         Scrap = 0,
00079:         Usable = 1,
00080:         Good = 2,
00081:         Fine = 3
00082:     }
00083:
00084:     public enum FoundryIncidentSeverity
00085:     {
00086:         None = 0,
00087:         Contained = 1,
00088:         Severe = 2
00089:     }
00090:
00091:     public enum FoundryFacilityComponent
00092:     {
00093:         RefractoryLining = 0,
00094:         HearthTuyeres = 1,
00095:         SandBeds = 2,
00096:         StructuralSupports = 3,
00097:         SafetyExhaust = 4
00098:     }
00099:
00100:     public enum FoundryStrikeResolution
00101:     {
00102:         ConcedeShiftLimits = 0,
00103:         UpholdQuota = 1,
00104:         Mediation = 2
00105:     }
00106:
00107:     // ---------------------------------------------------------------------
00108:     // History records (mutable, serialized)
00109:     // ---------------------------------------------------------------------
00110:
00111:     [Serializable]
00112:     public sealed class FoundryProductionRecord
00113:     {
00114:         public string productId = string.Empty;
00115:         public string displayName = string.Empty;
00116:         public int amount = 0;
00117:         public FoundryQualityTier tier = FoundryQualityTier.Usable;
00118:         public int completedDay = 0;
00119:         public int workers = 0;
00120:
00121:         // Plan 213 — additive provenance. Old saves deserialize to the
00122:         // defaults (Standard/unknown, no craft pass) and are never
00123:         // recalculated retroactively.
00124:         public string purity = FoundryPurityNames.Standard;
00125:         public string materialProfileId = string.Empty;
00126:         public int craftQualityPermille = 0;
00127:     }
00128:
00129:     [Serializable]
00130:     public sealed class FoundryFailedCastRecord
00131:     {
00132:         public string productId = string.Empty;
00133:         public string displayName = string.Empty;
00134:         public string reason = string.Empty;
00135:         public int failedDay = 0;
00136:         public int materialsLost = 0;
00137:     }
00138:
00139:     [Serializable]
00140:     public sealed class FoundryIncidentRecord
00141:     {
00142:         public FoundryIncidentSeverity severity = FoundryIncidentSeverity.None;
00143:         public int day = 0;
00144:         public string summary = string.Empty;
00145:         public int workersInjured = 0;
00146:         public int downtimeDays = 0;
00147:     }
00148:
00149:     [Serializable]
00150:     public sealed class FoundryRepairRecord
00151:     {
00152:         public string component = string.Empty;
00153:         public int day = 0;
00154:         public float conditionBefore = 0f;
00155:         public float conditionAfter = 0f;
00156:     }
00157:
00158:     [Serializable]
00159:     public sealed class FoundryTreatyCompliance
00160:     {
00161:         public string treatyId = string.Empty;
00162:         public string obligation = string.Empty;   // brine_pipe_quota | labor_shifts | road_iron_quota | charter_eligibility
00163:         public int quotaTotal = 0;
00164:         public int quotaFulfilled = 0;
00165:         public int quotaDeadlineDay = 0;           // next assessment day
00166:         public int lastAssessmentDay = 0;
00167:         public int metCount = 0;
00168:         public int missedCount = 0;
00169:         public bool currentCycleMet = false;
00170:         /// <summary>Sum of standing consequences from missed obligations.</summary>
00171:         /// <summary>
00172:         /// Legacy per-treaty penalty counter (pre-policy). Kept for old-save
00173:         /// compat; the policy-driven <c>GuildStanding</c> is the standing authority.
00174:         /// </summary>
00175:         public float standingPenalty = 0f;
00176:         public bool constitutionEligible = false;
00177:     }
00178:
00179:     // ---------------------------------------------------------------------
00180:     // Save DTO (versioned, plain public fields, no host objects)
00181:     // ---------------------------------------------------------------------
00182:
00183:     [Serializable]
00184:     public sealed class SilentFoundryState
00185:     {
00186:         public const int CurrentVersion = 1;
00187:
00188:         public int stateVersion = CurrentVersion;
00189:
00190:         public bool unlocked = false;
00191:         public int unlockDay = 0;
00192:
00193:         // Facility condition (0..100, authored baseline 100).
00194:         public float refractoryLining = 100f;
00195:         public float hearthTuyeres = 100f;
00196:         public float sandBeds = 100f;
00197:         public float structuralSupports = 100f;
00198:         public float safetyExhaust = 100f;
00199:
00200:         // Maintenance.
00201:         public int maintenanceCycleDays = 4;       // authored anchor from room_bp_11
00202:         public int maintenanceDueDay = 0;
00203:         public int daysSinceMaintenance = 0;
00204:         public int maintenancePerformed = 0;
00205:
00206:         // Green-sand casting bed.
00207:         public float sandQuality = 65f;
00208:         public float sandMoisture = 65f;           // target band ~55..75
00209:         public float binderQuality = 60f;
00210:         public float patternQuality = 70f;
00211:         public float contamination = 5f;           // grows with low-grade charge
00212:         public int moldReuseCount = 0;
00213:         public float compaction = 70f;
00214:
00215:         // Heat lifecycle.
00216:         public FoundryHeatStage heatStage = FoundryHeatStage.Idle;
00217:         public int heatStartedDay = 0;
00218:         public int stageElapsedDays = 0;
00219:
00220:         // Active production.
00221:         public string activeProductId = string.Empty;
00222:         public int assignedWorkers = 0;
00223:         public float workerSkill = 0.5f;
00224:         public float laborAccumulated = 0f;
00225:         public float workerExposure = 0f;          // fatigue/exposure units accrued
00226:         public int materialsConsumed = 0;
00227:         public bool childLaborUsed = false;
00228:
00229:         // Output quality of the cast currently completing.
00230:         public float pendingQuality = 0f;
00231:
00232:         // History.
00233:         public List<FoundryProductionRecord> completed = new List<FoundryProductionRecord>();
00234:         public List<FoundryFailedCastRecord> failed = new List<FoundryFailedCastRecord>();
00235:         public List<FoundryIncidentRecord> incidents = new List<FoundryIncidentRecord>();
00236:         public List<FoundryRepairRecord> repairs = new List<FoundryRepairRecord>();
00237:
00238:         // Labor.
00239:         public FoundryLaborDispute laborDispute = FoundryLaborDispute.None;
00240:         public int laborDisputeStartedDay = 0;
00241:         public int strikeStartedDay = 0;
00242:         public bool overtimeFlag = false;
00243:         public bool educationConflictFlag = false;
00244:
00245:         // Treaty compliance.
00246:         public List<FoundryTreatyCompliance> treatyCompliance = new List<FoundryTreatyCompliance>();
00247:
00248:         // Journal + morale.
00249:         public List<string> triggeredJournals = new List<string>();
00250:         public float cumulativeStress = 0f;
00251:         public float cumulativeHope = 0f;
00252:         public int firstHeatDay = 0;
00253:         public int strikeDay = 0;
00254:
00255:         // Plan B66 — heavy metallurgy expansion. Legacy defaults keep old
00256:         // saves safe: no slag, no active heavy batch (never a half-full
00257:         // crucible materializing from a pre-B66 save).
00258:         public string activeMetallurgyRecipeId = string.Empty;
00259:         public float metallurgySlag = 0f;              // 0..100 normalized
00260:         public int metallurgyBatchesCompleted = 0;
00261:
00262:         // Plan 213 — forging session (additive; null = no pass, legacy clean).
00263:         public FoundryForgingSessionState? activeForging = null;
00264:
00265:         // Determinism.
00266:         public int rngSeed = 0;
00267:     }
00268:
00269:     // ---------------------------------------------------------------------
00270:     // Journal trigger payload
00271:     // ---------------------------------------------------------------------
00272:
00273:     public sealed class FoundryJournalTrigger
00274:     {
00275:         public string TemplateId = string.Empty;
00276:         public float StressDelta = 0f;
00277:         public float HopeEarned = 0f;
00278:         public int Day = 0;
00279:     }
00280:
00281:     // ---------------------------------------------------------------------
00282: }
```


# Appendix — Current Source Detail: `src/Foundry/SilentFoundryHostSession.cs`

### `src/Foundry/SilentFoundryHostSession.cs` — complete current file

- Size: 650 lines / 31351 bytes.
- SHA-256: `bbbed72dc2e0ebf48a735ed8f9f561cd61fd27bc7635edfd15ade6c688e89299`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Godot;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Economy;
00008: using Ashfall.Core.Foundry;
00009: using Ashfall.Core.Inventory;
00010: using Ashfall.Core.Journal;
00011: using Ashfall.Core.Narrative;
00012: using Ashfall.Core.Survivors;
00013: using InventoryContainer = Ashfall.Core.Inventory.Inventory;
00014:
00015: namespace AtomicWar.GodotApp
00016: {
00017:     /// <summary>
00018:     /// Thin Godot-host session for THE SILENT FOUNDRY (Expansion 10).
00019:     /// Loads the static catalogs (foundry_production.json, foundry_items.json,
00020:     /// foundry_faction.json), wires the Core SilentFoundrySystem to the shared
00021:     /// inventory and the journal bridge, and exposes thin commands for the UI.
00022:     /// No gameplay rules here — hosts only present.
00023:     /// </summary>
00024:     public sealed class SilentFoundryHostSession
00025:     {
00026:         public const int DefaultSeed = 1009;
00027:
00028:         public SilentFoundrySystem Engine { get; }
00029:         public SaltMineExtractionSystem SaltMine { get; }
00030:         public SilentFoundryCatalog Catalog { get; }
00031:         public ItemCatalog FoundryItems { get; }
00032:         public SilentFoundryConsequencePolicyCatalog ConsequencePolicy { get; }
00033:         public FactionStanceEngine GuildStanceEngine { get; }
00034:
00035:         public Ashfall.Core.Shelter.PowerGridSystem? PowerGrid { get; set; }
00036:         public Ashfall.Core.ShelterThermalSystem? ThermalSystem { get; set; }
00037:
00038:         public string LastEvent { get; set; } = string.Empty;
00039:         public event Action? StateChanged;
00040:
00041:         private readonly JournalSystem? _journal;
00042:         private readonly NarrativeBatchCatalog _journalTemplates = new NarrativeBatchCatalog();
00043:         private readonly InventoryContainer _inventory;
00044:         private readonly ItemCatalog _inventoryCatalog;
00045:         private readonly MarketSystem? _market;
00046:
00047:         public void BindPowerAndThermal(Ashfall.Core.Shelter.PowerGridSystem? powerGrid, Ashfall.Core.ShelterThermalSystem? thermal)
00048:         {
00049:             PowerGrid = powerGrid;
00050:             ThermalSystem = thermal;
00051:         }
00052:
00053:         // ── Plan 213 follow-up — interactive forging commands (presentation
00054:         // adapters over the Core-owned deterministic forging pass; no rules here).
00055:
00056:         /// <summary>Begin a forging pass on the latest provenance-bearing batch.</summary>
00057:         public string BeginForging(string outputItemId, int day)
00058:         {
00059:             string result = Engine.BeginForging(outputItemId, day);
00060:             LastEvent = result;
00061:             StateChanged?.Invoke();
00062:             return result;
00063:         }
00064:
00065:         /// <summary>Record one abstract forging command (heat/shape/finish/inspect).</summary>
00066:         public string SubmitForgingCommand(Ashfall.Core.Foundry.FoundryForgingCommand command, int day)
00067:         {
00068:             string result = Engine.SubmitForgingCommand(command, day);
00069:             LastEvent = result;
00070:             StateChanged?.Invoke();
00071:             return result;
00072:         }
00073:
00074:         /// <summary>Complete the pass: deterministic scoring against the authored sequence.</summary>
00075:         public string CompleteForging(int day)
00076:         {
00077:             var result = Engine.CompleteForging(day);
00078:             LastEvent = result.Accepted
00079:                 ? $"Forging pass complete: {result.Purity} purity, quality {result.FinalQualityPermille}/1000."
00080:                 : $"Forging pass not completed ({result.Reason}).";
00081:             StateChanged?.Invoke();
00082:             return LastEvent;
00083:         }
00084:
00085:         public void TickDaily(int day)
00086:         {
00087:             if (Engine.IsHeatActive)
00088:             {
00089:                 // Check if the foundry room / workshop is unpowered or experiencing brownout
00090:                 bool isPowered = PowerGrid == null || PowerGrid.IsRoomPowered("room_foundry") || PowerGrid.IsRoomPowered("room_workshop");
00091:                 if (!isPowered)
00092:                 {
00093:                     Engine.SuspendHeat("Grid brownout / room unpowered", day);
00094:                     LastEvent = $"Foundry heat suspended on day {day}: electrical grid brownout.";
00095:                     StateChanged?.Invoke();
00096:                     return;
00097:                 }
00098:
00099:                 float powerKw = Engine.CurrentPowerDemandKw;
00100:                 float wasteHeatKw = Engine.CurrentWasteHeatKw;
00101:
00102:                 // Inject waste heat into shelter workshop / bunker
00103:                 ThermalSystem?.AddAuxiliaryHeat("room_workshop", wasteHeatKw);
00104:
00105:                 LastEvent = $"Foundry active (Heat: {Engine.HeatStage}): drew {powerKw:F1} kW; emitted +{wasteHeatKw:F1} kW waste heat warming workshop.";
00106:             }
00107:
00108:             Engine.TickDaily(day);
00109:             // C2[6] 23A: the salt mine is a real electrical load. Its private
00110:             // isPowered field is no longer an independent authority — feed it from
00111:             // the canonical allocation (mine shares the foundry/workshop bus).
00112:             SaltMine.SetPower(PowerGrid == null
00113:                 || PowerGrid.IsRoomServed("room_foundry")
00114:                 || PowerGrid.IsRoomServed("room_workshop"));
00115:             SaltMine.TickDaily(day, new CoreSeededRng(day * 31));
00116:             StateChanged?.Invoke();
00117:         }
00118:
00119:         private SilentFoundryHostSession(
00120:             SilentFoundrySystem engine,
00121:             SaltMineExtractionSystem saltMine,
00122:             SilentFoundryCatalog catalog,
00123:             ItemCatalog foundryItems,
00124:             InventoryContainer inventory,
00125:             ItemCatalog inventoryCatalog,
00126:             JournalSystem? journal,
00127:             MarketSystem? market,
00128:             SilentFoundryConsequencePolicyCatalog consequencePolicy,
00129:             ILog log)
00130:         {
00131:             Engine = engine;
00132:             SaltMine = saltMine;
00133:             Catalog = catalog;
00134:             FoundryItems = foundryItems;
00135:             _inventory = inventory;
00136:             _inventoryCatalog = inventoryCatalog;
00137:             _journal = journal;
00138:             _market = market;
00139:             ConsequencePolicy = consequencePolicy;
00140:             Engine.BindConsequencePolicy(consequencePolicy);
00141:
00142:             // The Foundry Guild is registered with the existing stance engine
00143:             // (no alias, no second standing system). Its trust is derived from
00144:             // the Core consequence ledger; the ledger is the save authority.
00145:             GuildStanceEngine = new FactionStanceEngine
00146:             {
00147:                 // GAP-STUB-03 partial wire: inventory-grounded providers that
00148:                 // SilentFoundryHostSession can see. The remaining providers
00149:                 // (Day, radiation, hated military, military-faction check) need
00150:                 // Main-level state injection and remain as defaults until then.
00151:                 PartyHasArsProvider = () => _inventory.CountByType(ItemType.AntiRad) > 0,
00152:                 PartyIntactHazmatProvider = () =>
00153:                 {
00154:                     for (int i = 0; i < _inventory.Equipped.Count; i++)
00155:                     {
00156:                         var e = _inventory.Equipped[i];
00157:                         if (e.Item != null && (e.Item.id == "hazmat_suit" || e.Item.id == "gas_mask"))
00158:                             return true;
00159:                     }
00160:                     return false;
00161:                 }
00162:             };
00163:             GuildStanceEngine.RegisterFaction(new FactionThresholds(
00164:                 SilentFoundryIds.FactionId,
00165:                 raidThreshold: -50f,
00166:                 robThreshold: -20f,
00167:                 minTrustToTrade: -40f,
00168:                 intelShareThreshold: 40f));
00169:             SyncGuildStanding();
00170:
00171:             Engine.OnConsequenceApplied += ApplyConsequenceToSurfaces;
00172:
00173:             Engine.BindInventory(
00174:                 id => _inventory.CountById(id),
00175:                 (id, amount) => _inventoryCatalog.Get(id) != null,
00176:                 (id, amount) =>
00177:                 {
00178:                     var def = _inventoryCatalog.Get(id);
00179:                     if (def != null) _inventory.Add(def, amount);
00180:                 },
00181:                 (id, amount) =>
00182:                 {
00183:                     var def = _inventoryCatalog.Get(id);
00184:                     if (def != null) _inventory.Remove(def, amount);
00185:                 });
00186:
00187:             Engine.OnStateChanged += _ => StateChanged?.Invoke();
00188:             Engine.OnProductionCompleted += r =>
00189:             {
00190:                 // Plan 213 — semantic payload: job/product, output, quality tier.
00191:                 LastEvent = $"Cast complete: {r.displayName} ×{r.amount} ({r.tier}, purity {r.purity}). Output stored.";
00192:                 StateChanged?.Invoke();
00193:             };
00194:             Engine.OnForgingCompleted += f =>
00195:             {
00196:                 LastEvent = $"Forging completed: {f.ProductOutputItemId} quality {f.FinalQualityPermille}/1000 ({f.Purity}).";
00197:                 StateChanged?.Invoke();
00198:             };
00199:             Engine.OnCastFailed += f => { LastEvent = "Cast failed: " + f.reason; StateChanged?.Invoke(); };
00200:             Engine.OnIncident += i => { LastEvent = "INCIDENT: " + i.summary; StateChanged?.Invoke(); };
00201:             Engine.OnTreatyQuotaMet += c => { LastEvent = "Treaty quota met: " + c.treatyId; StateChanged?.Invoke(); };
00202:             Engine.OnTreatyQuotaMissed += c => { LastEvent = "Treaty quota missed: " + c.treatyId; StateChanged?.Invoke(); };
00203:             Engine.OnJournalTriggered += BridgeJournalTrigger;
00204:
00205:             // Salt mine events
00206:             SaltMine.OnStateChanged += _ => StateChanged?.Invoke();
00207:             SaltMine.OnExtractionBatchProduced += (id, kg) => { LastEvent = $"Extraction: {kg:F1} kg from {id}."; StateChanged?.Invoke(); };
00208:             SaltMine.OnTreatyDeliveryAccepted += r => { LastEvent = $"Treaty delivery accepted: {r.quantityDelivered:F1} barrels."; StateChanged?.Invoke(); };
00209:             SaltMine.OnTreatyDeliveryMissed += r => { LastEvent = $"Treaty delivery missed."; StateChanged?.Invoke(); };
00210:             SaltMine.OnDrillFailure += id => { LastEvent = $"DRILL FAILURE at {id}!"; StateChanged?.Invoke(); };
00211:             SaltMine.OnWorkerExposure += (id, contam) => { LastEvent = $"Worker exposure at {id}: {contam:P0}"; StateChanged?.Invoke(); };
00212:
00213:             // The authored journal templates stay the source of the narrative text.
00214:             // Plan 26A — resolve through the one data-path authority.
00215:             string jrnlPath = CatalogPath.ResolveSub("narrative", "jrnl_templates_cycle_d.json");
00216:             if (System.IO.File.Exists(jrnlPath))
00217:             {
00218:                 try
00219:                 {
00220:                     _journalTemplates.LoadJournalBatch(System.IO.File.ReadAllText(jrnlPath), new SystemTextJsonSerializer());
00221:                 }
00222:                 catch (Exception e)
00223:                 {
00224:                     GD.PrintErr("[SilentFoundry] journal template load failed: " + e.Message);
00225:                 }
00226:             }
00227:         }
00228:
00229:         public static SilentFoundryHostSession Create(
00230:             string dataDir,
00231:             ExpansionHostSession expansions,
00232:             InventoryHostSession inventory,
00233:             JournalSystem? journal = null,
00234:             MarketSystem? market = null,
00235:             ILog? log = null,
00236:             bool seedSupplies = true)
00237:         {
00238:             log = log ?? new GodotLog();
00239:             var files = new FileSystemIO();
00240:             var json = new SystemTextJsonSerializer();
00241:
00242:             var engine = expansions.SilentFoundry;
00243:             var catalog = expansions.FoundryData;
00244:             if (engine == null || catalog == null)
00245:             {
00246:                 // Standalone fallback (tests / hosts that skip the hub): build a
00247:                 // fresh engine bound to the static catalogs + blueprint + treaties.
00248:                 var built = BuildStandalone(dataDir, files, json, log);
00249:                 engine = built.Engine;
00250:                 catalog = built.Catalog;
00251:             }
00252:
00253:             // Authored consequence policy (data authority: foundry_treaty_consequences.json).
00254:             var policyCatalog = new SilentFoundryConsequencePolicyCatalog();
00255:             policyCatalog.Load(SilentFoundryConsequenceCatalogLoader.Load(dataDir, files, json));
00256:
00257:             // Foundry item definitions → shared inventory catalog (data authority: foundry_items.json).
00258:             var foundryItems = LoadFoundryItems(dataDir, files, json);
00259:             foreach (string id in foundryItems.Ids)
00260:             {
00261:                 var def = foundryItems.Get(id);
00262:                 if (def != null) inventory.Catalog.Register(def);
00263:             }
00264:             EnsureChargeMaterials(inventory.Catalog);
00265:
00266:             var saltMine = new SaltMineExtractionSystem();
00267:             var session = new SilentFoundryHostSession(engine, saltMine, catalog, foundryItems,
00268:                 inventory.Inventory, inventory.Catalog, journal, market, policyCatalog, log);
00269:             // Starter stock is a fresh-campaign grant only. Seeding on restore
00270:             // would duplicate charge materials onto the restored inventory
00271:             // every Continue/load, so the caller gates it on fresh init.
00272:             if (seedSupplies)
00273:                 SeedFoundrySupplies(inventory);
00274:             return session;
00275:         }
00276:
00277:         /// <summary>Expose the current guild stance/trust for presentation.</summary>
00278:         public float GuildTrust => Engine.GuildStanding;
00279:         public TradeStance GuildStance => GuildStanceEngine.GetStance(SilentFoundryIds.FactionId);
00280:
00281:         /// <summary>
00282:         /// Mirror the authoritative Core standing into the existing stance engine
00283:         /// (SetTrust, never ModifyTrust on restore — no double counting).
00284:         /// </summary>
00285:         public void SyncGuildStanding()
00286:         {
00287:             GuildStanceEngine.SetTrust(SilentFoundryIds.FactionId, Engine.GuildStanding);
00288:         }
00289:
00290:         /// <summary>
00291:         /// Wire the remaining FactionStanceEngine providers from Main state
00292:         /// (day, radiation, hated-military check). Called once after construction
00293:         /// from Main.Economy.SetupEconomy(). Providers are live accessors, not
00294:         /// captured values: campaignDay/radiationProvider/survivors are invoked
00295:         /// fresh every time FactionStanceEngine calls them, so guild trust
00296:         /// reflects the campaign's actual current day/radiation/roster rather
00297:         /// than whatever those values happened to be at bind time.
00298:         /// </summary>
00299:         public void BindStanceProviders(Func<int> campaignDayProvider, Func<float> partyRadiationProvider, Func<SurvivorsHostSession?> survivorsProvider)
00300:         {
00301:             GuildStanceEngine.DayProvider = campaignDayProvider ?? (() => 0);
00302:             GuildStanceEngine.PartyRadiationProvider = partyRadiationProvider ?? (() => -1f);
00303:             GuildStanceEngine.HasHatedMilitarySurvivor = () => HasMilitarySurvivor(survivorsProvider?.Invoke());
00304:             GuildStanceEngine.ClampTrustProvider = v => Math.Clamp(v, -100f, 100f);
00305:             GuildStanceEngine.IsMilitaryFaction = id => IsMilitaryFaction(id);
00306:         }
00307:
00308:         private static bool IsMilitaryFaction(string factionId)
00309:         {
00310:             if (string.IsNullOrEmpty(factionId)) return false;
00311:             // Known military faction IDs from data authority (characters.json,
00312:             // faction_radio_corpus.json). Extend as new military factions are
00313:             // added to the catalogs.
00314:             return factionId == "military_remnants"
00315:                 || factionId.StartsWith("military_", StringComparison.OrdinalIgnoreCase);
00316:         }
00317:
00318:         private static bool HasMilitarySurvivor(SurvivorsHostSession? survivors)
00319:         {
00320:             if (survivors?.Roster == null) return false;
00321:             foreach (var entry in survivors.Roster.Roster)
00322:             {
00323:                 if (!entry.isAlive) continue;
00324:                 var def = survivors.Roster.FindDefinition(entry.definitionId);
00325:                 if (def == null) continue;
00326:                 if (IsMilitaryProfession(def.profession)) return true;
00327:             }
00328:             return false;
00329:         }
00330:
00331:         private static bool IsMilitaryProfession(string profession)
00332:         {
00333:             if (string.IsNullOrEmpty(profession)) return false;
00334:             var lower = profession.ToLowerInvariant();
00335:             return lower.Contains("military")
00336:                 || lower.Contains("garrison")
00337:                 || lower.Contains("soldier")
00338:                 || lower.Contains("enforcer")
00339:                 || lower.Contains("martial")
00340:                 || lower.Contains("army")
00341:                 || lower.Contains("navy")
00342:                 || lower.Contains("commander");
00343:         }
00344:
00345:         /// <summary>
00346:         /// Apply a Core consequence record to the real economy surfaces exactly
00347:         /// once: standing into the existing FactionStanceEngine, market/logistics
00348:         /// modifiers into the existing MarketSystem demand path. The Core ledger
00349:         /// already guarantees once-per-cycle; this only mirrors it outward.
00350:         /// </summary>
00351:         private void ApplyConsequenceToSurfaces(FoundryConsequenceRecord record)
00352:         {
00353:             if (record == null) return;
00354:
00355:             if (Math.Abs(record.standingDelta) > 1e-6f)
00356:             {
00357:                 GuildStanceEngine.ModifyTrust(SilentFoundryIds.FactionId, record.standingDelta);
00358:             }
00359:
00360:             if (_market != null && record.modifiers != null)
00361:             {
00362:                 for (int i = 0; i < record.modifiers.Count; i++)
00363:                 {
00364:                     var m = record.modifiers[i];
00365:                     if (m == null || string.IsNullOrEmpty(m.good_id)) continue;
00366:                     if (_market.FindGood(m.good_id) == null)
00367:                     {
00368:                         GD.PrintErr($"[SilentFoundry] consequence references unknown good '{m.good_id}'; skipped.");
00369:                         continue;
00370:                     }
00371:                     _market.AdjustDemand(m.good_id, m.demand_delta);
00372:                 }
00373:             }
00374:
00375:             var sb = new System.Text.StringBuilder();
00376:             sb.Append("Consequence: ").Append(record.treatyId).Append(" ")
00377:               .Append(SilentFoundryConsequencePolicyCatalog.OutcomeName(record.outcome))
00378:               .Append(" · standing ").Append(record.standingDelta.ToString("+0;-0;0"))
00379:               .Append(" · guild ").Append(Engine.GuildStanding.ToString("F0"));
00380:             if (record.modifiers != null)
00381:             {
00382:                 for (int i = 0; i < record.modifiers.Count; i++)
00383:                 {
00384:                     var m = record.modifiers[i];
00385:                     if (m == null) continue;
00386:                     sb.Append(" · ").Append(m.good_id).Append(" demand ").Append(m.demand_delta.ToString("+0.00;-0.00"));
00387:                 }
00388:             }
00389:             LastEvent = sb.ToString();
00390:             StateChanged?.Invoke();
00391:         }
00392:
00393:         /// <summary>Standalone engine build (used when the expansion hub has none).</summary>
00394:         private static (SilentFoundrySystem Engine, SilentFoundryCatalog Catalog) BuildStandalone(
00395:             string dataDir, IFileIO files, IJsonSerializer json, ILog log)
00396:         {
00397:             var catalog = new SilentFoundryCatalog();
00398:             catalog.Load(
00399:                 SilentFoundryCatalogLoader.LoadProduction(dataDir, files, json)!,
00400:                 SilentFoundryCatalogLoader.LoadFaction(dataDir, files, json)!);
00401:
00402:             int maintenanceCycle = 4;
00403:             var blueprints = new BunkerBlueprintCatalog();
00404:             string bpPath = files.Combine(dataDir, "narrative", "bunker_blueprints_codex.json");
00405:             if (files.FileExists(bpPath))
00406:             {
00407:                 blueprints.Load(files.ReadAllText(bpPath), json);
00408:                 var bp = blueprints.GetById(SilentFoundryIds.BlueprintRoomId);
00409:                 if (bp != null && bp.maintenance_cycle_days > 0) maintenanceCycle = bp.maintenance_cycle_days;
00410:             }
00411:
00412:             var engine = new SilentFoundrySystem(log: log);
00413:             // District 8 accords (foundry_accords.json) drive the treaty clock.
00414:             var ratificationDays = SilentFoundryCatalogLoader.LoadAccordRatificationDays(dataDir, files, json);
00415:             if (ratificationDays.Count > 0)
00416:                 engine.BindTreaties(ratificationDays);
00417:             engine.BindCatalog(catalog, maintenanceCycle);
00418:             // Plan B66: heavy metallurgy roster merges into the production
00419:             // catalog so the standard heat machine resolves heavy recipes.
00420:             engine.BindMetallurgyCatalog(MetallurgyCatalogLoader.Load(dataDir, files, json));
00421:             // Plan B100: scientific glassworks uses the same heat/casting
00422:             // authority and therefore needs no new save section or host loop.
00423:             engine.BindGlassworksCatalog(GlassworksCatalogLoader.Load(dataDir, files, json));
00424:             return (engine, catalog);
00425:         }
00426:
00427:         /// <summary>Register the charge materials the Foundry consumes (canonical items.json ids).</summary>
00428:         private static void EnsureChargeMaterials(ItemCatalog catalog)
00429:         {
00430:             if (catalog.Get(SilentFoundryIds.ItemScrapMetal) == null)
00431:                 catalog.Register(new ItemDefinition
00432:                 {
00433:                     id = SilentFoundryIds.ItemScrapMetal, displayName = "Scrap Metal",
00434:                     type = ItemType.Material, stackMax = 50, weight = 0.4f, tradeValue = 1f
00435:                 });
00436:             if (catalog.Get(SilentFoundryIds.ItemCoal) == null)
00437:                 catalog.Register(new ItemDefinition
00438:                 {
00439:                     id = SilentFoundryIds.ItemCoal, displayName = "Coal",
00440:                     type = ItemType.Fuel, stackMax = 50, weight = 0.9f, tradeValue = 2f
00441:                 });
00442:             if (catalog.Get(SilentFoundryIds.ItemCharcoal) == null)
00443:                 catalog.Register(new ItemDefinition
00444:                 {
00445:                     id = SilentFoundryIds.ItemCharcoal, displayName = "Charcoal",
00446:                     type = ItemType.Fuel, stackMax = 50, weight = 0.5f, tradeValue = 1f
00447:                 });
00448:         }
00449:
00450:         /// <summary>Read foundry_items.json into an ItemCatalog (same schema as items.json).</summary>
00451:         private static ItemCatalog LoadFoundryItems(string dataDir, IFileIO files, IJsonSerializer json)
00452:         {
00453:             var catalog = new ItemCatalog();
00454:             string path = files.Combine(dataDir, "foundry_items.json");
00455:             if (!files.FileExists(path)) return catalog;
00456:             try
00457:             {
00458:                 var defs = CatalogLocator.LoadWrappedList<FoundryItemJson>(files.ReadAllText(path), SystemTextJsonSerializer.Options);
00459:                 if (defs == null) return catalog;
00460:                 for (int i = 0; i < defs.Count; i++)
00461:                 {
00462:                     var d = defs[i];
00463:                     if (d == null || string.IsNullOrEmpty(d.id)) continue;
00464:                     if (!Enum.TryParse(d.type, ignoreCase: true, out ItemType type)) type = ItemType.Material;
00465:                     catalog.Register(new ItemDefinition
00466:                     {
00467:                         id = d.id!,
00468:                         displayName = d.displayName ?? string.Empty,
00469:                         description = d.description ?? string.Empty,
00470:                         type = type,
00471:                         stackMax = d.stackMax > 0 ? d.stackMax : 1,
00472:                         weight = d.weight,
00473:                         tradeValue = d.tradeValue,
00474:                         durability = d.durability > 0 ? d.durability : 100f
00475:                     });
00476:                 }
00477:             }
00478:             catch (Exception e)
00479:             {
00480:                 GD.PrintErr("[SilentFoundry] foundry_items.json load failed: " + e.Message);
00481:             }
00482:             return catalog;
00483:         }
00484:
00485:         /// <summary>Seed a modest starter stock of charge materials into the shared inventory.</summary>
00486:         private static void SeedFoundrySupplies(InventoryHostSession inventory)
00487:         {
00488:             // Capacity-bounded shared container: keep to a few slots; the rest is
00489:             // gathered through expeditions and trade, like every other material.
00490:             inventory.Add(SilentFoundryIds.ItemScrapMetal, 12);
00491:             inventory.Add(SilentFoundryIds.ItemCoal, 12);
00492:             inventory.Add(SilentFoundryIds.ItemCleanWater, 6);
00493:             inventory.Add(SilentFoundryIds.ItemFlux, 3);
00494:         }
00495:
00496:         /// <summary>Bridge a Core journal trigger to the real journal system (once-only via knowledge key).</summary>
00497:         private void BridgeJournalTrigger(FoundryJournalTrigger trigger)
00498:         {
00499:             if (_journal == null) return;
00500:             if (trigger == null) return;
00501:
00502:             string body = string.Empty;
00503:             string authorRole = string.Empty;
00504:             var template = _journalTemplates.JournalTemplates.TryGetValue(trigger.TemplateId, out var t) ? t : null;
00505:             if (template != null)
00506:             {
00507:                 body = template.body_template ?? string.Empty;
00508:                 authorRole = template.author_role ?? string.Empty;
00509:             }
00510:             if (string.IsNullOrEmpty(body))
00511:             {
00512:                 body = trigger.TemplateId == SilentFoundryIds.JournalFirstHeat
00513:                     ? "The first successful heat is poured. New iron, not scrap."
00514:                     : "The charging floor has stopped. The strike is real.";
00515:             }
00516:
00517:             // Preserve the authored author role in the journal entry (the template
00518:             // remains the text authority; the role becomes the entry's author).
00519:             ISurvivorAuthor? author = null;
00520:             if (!string.IsNullOrEmpty(authorRole))
00521:             {
00522:                 author = new FoundryJournalAuthor(trigger.TemplateId, authorRole);
00523:             }
00524:
00525:             // The template id doubles as the knowledge key: KnowledgeBase dedupes,
00526:             // so reloading a save can never inject a duplicate entry.
00527:             _journal.TryAddRawEntry(trigger.TemplateId, body!, author!, trigger.Day);
00528:         }
00529:
00530:         /// <summary>Minimal author surface so the journal preserves the authored role.</summary>
00531:         private sealed class FoundryJournalAuthor : ISurvivorAuthor
00532:         {
00533:             public string Id { get; }
00534:             public string DisplayName { get; }
00535:             public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;
00536:
00537:             public FoundryJournalAuthor(string templateId, string role)
00538:             {
00539:                 Id = templateId;
00540:                 DisplayName = char.ToUpperInvariant(role[0]) + role.Substring(1); // foundryman -> Foundryman
00541:             }
00542:         }
00543:
00544:         // ---- Thin commands for the UI ----
00545:
00546:         public string Unlock(int day) => Engine.Unlock(day) ? "The Silent Foundry is open." : "Already open.";
00547:         public string Repair(FoundryFacilityComponent component, int day) => Engine.StartRepair(component, day);
00548:         public string Maintain(int day) => Engine.PerformMaintenance(day);
00549:         public string PrepareSand(int water) => Engine.PrepareSand(water);
00550:         public string CompactMold() => Engine.CompactMold(0.6f);
00551:         public string StartHeat(string productId, int workers, float skill, int day)
00552:         {
00553:             if (PowerGrid != null && !PowerGrid.IsRoomPowered("room_foundry") && !PowerGrid.IsRoomPowered("room_workshop"))
00554:             {
00555:                 LastEvent = "Cannot start heat: electrical grid is unpowered or in brownout.";
00556:                 StateChanged?.Invoke();
00557:                 return LastEvent;
00558:             }
00559:             string res = Engine.StartProduction(productId, workers, skill, day);
00560:             LastEvent = res;
00561:             StateChanged?.Invoke();
00562:             return res;
00563:         }
00564:         public string Tap(int day) => Engine.TapAndCast(day);
00565:         public string SetOvertime(bool on) { Engine.SetOvertime(on); return on ? "Overtime ordered." : "Overtime rescinded."; }
00566:         public string SetChildLabor(bool on) { Engine.SetChildLaborUsed(on); return on ? "Children sent to the charging floor." : "Children returned to lessons."; }
00567:         public string OpenDispute(int day) => Engine.BeginLaborDispute(day);
00568:         public string ResolveStrike(FoundryStrikeResolution resolution, int day) => Engine.ResolveStrike(resolution, day);
00569:
00570:         // ---- Salt mine production commands ----
00571:
00572:         /// <summary>Register and unlock a salt mine vein.</summary>
00573:         public string OpenSaltMine(string veinId = "vein_salt_01", string displayName = "Main Salt Vein", int initialWorkers = 2)
00574:         {
00575:             var vein = new SaltMineVeinState
00576:             {
00577:                 veinId = veinId,
00578:                 displayName = displayName,
00579:                 isUnlocked = false,
00580:                 remainingOre = 5000f,
00581:                 extractionRate = 10f,
00582:                 maxWorkers = 4,
00583:                 assignedWorkers = 0,
00584:                 drillCondition = 1.0f,
00585:                 pumpPressure = 1.0f
00586:             };
00587:             SaltMine.RegisterVein(vein);
00588:             SaltMine.UnlockVein(veinId);
00589:             SaltMine.AssignWorkers(veinId, initialWorkers);
00590:             return $"Salt mine opened. {initialWorkers} workers assigned to {displayName}.";
00591:         }
00592:
00593:         public string OpenSaltMineDemo() => OpenSaltMine();
00594:
00595:         /// <summary>Run one day of salt extraction.</summary>
00596:         public string TickSaltMine(int day)
00597:         {
00598:             SaltMine.TickDaily(day, new CoreSeededRng(day * 31));
00599:             var s = SaltMine.State;
00600:             return $"Salt mine tick d{day}: salt={s.saltStorage:F1} kg, brine={s.brineStorage:F1} brl, sulfur={s.sulfurStorage:F1} kg.";
00601:         }
00602:
00603:         public string TickSaltMineDemo(int day) => TickSaltMine(day);
00604:
00605:         /// <summary>Deliver to treaty.</summary>
00606:         public string DeliverSaltTreaty(int day)
00607:         {
00608:             var record = SaltMine.DeliverToTreaty(day);
00609:             if (record == null) return "No delivery possible.";
00610:             return record.accepted
00611:                 ? $"Treaty delivery accepted: {record.quantityDelivered:F1} barrels brine."
00612:                 : "Treaty delivery failed: insufficient stock.";
00613:         }
00614:
00615:         public string DeliverSaltTreatyDemo(int day) => DeliverSaltTreaty(day);
00616:
00617:         /// <summary>Salt mine status.</summary>
00618:         public string SaltMineStatusLine()
00619:         {
00620:             var s = SaltMine.State;
00621:             var vein = SaltMine.GetVein("vein_salt_01");
00622:             string veinStatus = vein != null
00623:                 ? $"vein: {vein.assignedWorkers}/{vein.maxWorkers} workers, drill {vein.drillCondition:P0}, pump {vein.pumpPressure:P0}"
00624:                 : "no vein";
00625:             return $"SALT MINE: {veinStatus} · storage: salt={s.saltStorage:F1} kg, brine={s.brineStorage:F1} brl, sulfur={s.sulfurStorage:F1} kg · deliveries: {SaltMine.GetDeliveryCount()}";
00626:         }
00627:
00628:         public string StatusLine()
00629:         {
00630:             var s = Engine.State;
00631:             return $"FOUNDRY: {(s.unlocked ? "OPEN" : "SEALED")} · heat {Engine.HeatStage} · "
00632:                 + $"hearth {s.hearthTuyeres:F0}/100 · maintenance {(Engine.IsMaintenanceOverdue ? "OVERDUE " + Engine.DaysOverdue + "d" : (s.maintenanceDueDay > 0 ? "due d" + s.maintenanceDueDay : "unscheduled"))} · "
00633:                 + $"casts {Engine.TotalProductionCount} · failed {Engine.TotalFailedCount} · "
00634:                 + $"labor {Engine.LaborDispute} · hope {Engine.CumulativeHope:F0}";
00635:         }
00636:     }
00637:
00638:     /// <summary>foundry_items.json row (same camelCase schema as items.json).</summary>
00639:     public sealed class FoundryItemJson
00640:     {
00641:         public string? id;
00642:         public string? displayName;
00643:         public string? description;
00644:         public string? type;
00645:         public int stackMax = 1;
00646:         public float weight;
00647:         public float tradeValue;
00648:         public float durability;
00649:     }
00650: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/FoundryAccordExpansionTests.cs`

### `Ashfall.Core.Tests/FoundryAccordExpansionTests.cs` — complete current file

- Size: 362 lines / 17008 bytes.
- SHA-256: `73613c87e16e2a46a840492ce7dfedd0a89818997867e174ffb5770a5dfbd3a9`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Foundry;
00007: using Ashfall.Core.Narrative;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests
00011: {
00012:     /// <summary>
00013:     /// Plan 102: Comprehensive regression suite for the expanded inter-faction
00014:     /// Foundry treaty accords catalog (foundry_accords.json).
00015:     /// Covers schema parsing, baseline parity, signatory authority, resource
00016:     /// allocations, structured legal articles, penalties, and consequence bindings.
00017:     /// </summary>
00018:     public sealed class FoundryAccordExpansionTests
00019:     {
00020:         private static string FindDataDir()
00021:         {
00022:             string start = Directory.GetCurrentDirectory();
00023:             if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
00024:             if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
00025:             throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
00026:         }
00027:
00028:         private static (RegionalTreatiesFile File, RegionalTreatyCatalog Catalog) LoadAccords()
00029:         {
00030:             string dataDir = FindDataDir();
00031:             var files = new FileSystemIO();
00032:             var json = new SystemTextJsonSerializer();
00033:
00034:             string accordsRaw = files.ReadAllText(Path.Combine(dataDir, SilentFoundryCatalogLoader.AccordsFileName));
00035:             var file = json.Deserialize<RegionalTreatiesFile>(accordsRaw)!;
00036:             var catalog = new RegionalTreatyCatalog();
00037:             catalog.Load(accordsRaw, json);
00038:
00039:             return (file, catalog);
00040:         }
00041:
00042:         // ── 1. Catalog Loading & Count ───────────────────────────────────
00043:
00044:         [Fact]
00045:         public void Catalog_LoadsAllAccordsWithoutErrors()
00046:         {
00047:             var (file, catalog) = LoadAccords();
00048:             Assert.NotNull(file);
00049:             Assert.Equal(1, file.schema_version);
00050:             Assert.Equal("foundry_district8_accords", file.collection_id);
00051:
00052:             // The file retains eight pre-existing regional accords plus the
00053:             // ten-accord Foundry-signatory District 8 network.
00054:             Assert.Equal(18, catalog.AllTreaties.Count);
00055:             Assert.Equal(10, catalog.GetByExactSignatoryFaction(SilentFoundryIds.FactionId).Count);
00056:         }
00057:
00058:         // ── 2. Baseline Parity Preservation ─────────────────────────────
00059:
00060:         [Fact]
00061:         public void Parity_BaselineFourDistrict8AccordsPreserved()
00062:         {
00063:             var (_, catalog) = LoadAccords();
00064:
00065:             // 1. Brine Pipe & Iodine Exchange
00066:             var brine = catalog.GetById(SilentFoundryIds.TreatyBrinePipe);
00067:             Assert.NotNull(brine);
00068:             Assert.Equal(280, brine.ratified_day);
00069:             Assert.Equal("The Brine Pipe & Iodine Exchange", brine.treaty_title);
00070:             Assert.Equal(40.0f, brine.water_allocation_lpm);
00071:             Assert.Equal(12.0f, brine.power_quota_kw);
00072:             Assert.Contains("faction_silent_foundry", brine.signatory_factions);
00073:             Assert.Contains("faction_the_office", brine.signatory_factions);
00074:
00075:             // 2. Cluster Labour Schedule
00076:             var labour = catalog.GetById(SilentFoundryIds.TreatyLabourSchedule);
00077:             Assert.NotNull(labour);
00078:             Assert.Equal(305, labour.ratified_day);
00079:             Assert.Equal("The Cluster Labour Schedule", labour.treaty_title);
00080:             Assert.Equal(25.0f, labour.water_allocation_lpm);
00081:             Assert.Equal(8.0f, labour.power_quota_kw);
00082:             Assert.Contains("faction_silent_foundry", labour.signatory_factions);
00083:             Assert.Contains("faction_the_office", labour.signatory_factions);
00084:             Assert.Contains("faction_the_cutters", labour.signatory_factions);
00085:
00086:             // 3. Road Iron Charter
00087:             var roadIron = catalog.GetById(SilentFoundryIds.TreatyRoadIron);
00088:             Assert.NotNull(roadIron);
00089:             Assert.Equal(330, roadIron.ratified_day);
00090:             Assert.Equal("The Road Iron Charter", roadIron.treaty_title);
00091:             Assert.Equal(15.0f, roadIron.water_allocation_lpm);
00092:             Assert.Equal(6.0f, roadIron.power_quota_kw);
00093:             Assert.Contains("faction_silent_foundry", roadIron.signatory_factions);
00094:             Assert.Contains("faction_the_cutters", roadIron.signatory_factions);
00095:             Assert.Contains("faction_the_fleet", roadIron.signatory_factions);
00096:
00097:             // 4. Cluster Charter
00098:             var charter = catalog.GetById(SilentFoundryIds.TreatyClusterCharter);
00099:             Assert.NotNull(charter);
00100:             Assert.Equal(365, charter.ratified_day);
00101:             Assert.Equal("The Cluster Charter", charter.treaty_title);
00102:             Assert.Equal(0.0f, charter.water_allocation_lpm);
00103:             Assert.Equal(0.0f, charter.power_quota_kw);
00104:             Assert.Contains("faction_silent_foundry", charter.signatory_factions);
00105:             Assert.Contains("faction_the_office", charter.signatory_factions);
00106:             Assert.Contains("faction_the_cutters", charter.signatory_factions);
00107:             Assert.Contains("faction_the_fleet", charter.signatory_factions);
00108:         }
00109:
00110:         [Fact]
00111:         public void FoundryRoster_ContainsExactlyTheSixNewDependencyReadyAccords()
00112:         {
00113:             var (_, catalog) = LoadAccords();
00114:             var expected = new HashSet<string>(StringComparer.Ordinal)
00115:             {
00116:                 SilentFoundryIds.TreatySaltworksAccess,
00117:                 SilentFoundryIds.TreatyMembraneRepair,
00118:                 SilentFoundryIds.TreatyCoalWindow,
00119:                 SilentFoundryIds.TreatyApprenticeExchange,
00120:                 SilentFoundryIds.TreatyCrisisMutualAid,
00121:                 SilentFoundryIds.TreatyIncidentBook
00122:             };
00123:
00124:             var foundryIds = new HashSet<string>(StringComparer.Ordinal);
00125:             foreach (var treaty in catalog.GetByExactSignatoryFaction(SilentFoundryIds.FactionId))
00126:                 foundryIds.Add(treaty.treaty_id);
00127:
00128:             Assert.Equal(10, foundryIds.Count);
00129:             foreach (var treatyId in expected)
00130:                 Assert.Contains(treatyId, foundryIds);
00131:
00132:             // Plan 103 is live: all consequence policies resolve to known
00133:             // treaty IDs in the catalog, including the expanded accord definitions.
00134:             var consequence = SilentFoundryConsequenceCatalogLoader.Load(FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00135:             Assert.NotEmpty(consequence.policies);
00136:             foreach (var policy in consequence.policies)
00137:                 Assert.NotNull(catalog.GetById(policy.treaty_id));
00138:         }
00139:
00140:         // ── 3. Treaty ID Uniqueness & Grammar ───────────────────────────
00141:
00142:         [Fact]
00143:         public void TreatyId_AllIdsAreUniqueAndFollowSnakeCasePrefix()
00144:         {
00145:             var (_, catalog) = LoadAccords();
00146:             var seenIds = new HashSet<string>(StringComparer.Ordinal);
00147:
00148:             foreach (var treaty in catalog.AllTreaties)
00149:             {
00150:                 Assert.False(string.IsNullOrWhiteSpace(treaty.treaty_id), "treaty_id cannot be blank");
00151:                 Assert.StartsWith("treaty_", treaty.treaty_id);
00152:                 Assert.Equal(treaty.treaty_id.ToLowerInvariant(), treaty.treaty_id);
00153:                 Assert.DoesNotContain(" ", treaty.treaty_id);
00154:                 Assert.True(seenIds.Add(treaty.treaty_id), $"Duplicate treaty_id found: {treaty.treaty_id}");
00155:             }
00156:         }
00157:
00158:         // ── 4. Signatory Authority Resolution ───────────────────────────
00159:
00160:         [Fact]
00161:         public void Signatories_AllFactionsAreValidAndNonEmpty()
00162:         {
00163:             var (_, catalog) = LoadAccords();
00164:             var validFactionIds = new HashSet<string>(StringComparer.Ordinal)
00165:             {
00166:                 "faction_silent_foundry",
00167:                 "faction_the_office",
00168:                 "faction_the_cutters",
00169:                 "faction_the_fleet",
00170:                 "faction_central_garrison",
00171:                 "faction_rebuilders",
00172:                 "faction_ash_sign",
00173:                 "faction_forward_roster",
00174:                 "faction_the_scale",
00175:                 "faction_archivists",
00176:                 "faction_grain_exchange",
00177:                 "faction_hydro_barons",
00178:                 "faction_scavenger_guild"
00179:             };
00180:
00181:             foreach (var treaty in catalog.AllTreaties)
00182:             {
00183:                 Assert.NotNull(treaty.signatory_factions);
00184:                 Assert.True(treaty.signatory_factions.Length >= 2,
00185:                     $"Treaty '{treaty.treaty_id}' must have at least 2 signatories");
00186:
00187:                 foreach (var faction in treaty.signatory_factions)
00188:                 {
00189:                     Assert.False(string.IsNullOrWhiteSpace(faction),
00190:                         $"Treaty '{treaty.treaty_id}' has empty faction id in signatories");
00191:                     Assert.True(validFactionIds.Contains(faction),
00192:                         $"Signatory '{faction}' in treaty '{treaty.treaty_id}' is not an authorized faction");
00193:                 }
00194:             }
00195:         }
00196:
00197:         // ── 5. Resource Allocation Validity ─────────────────────────────
00198:
00199:         [Fact]
00200:         public void Resources_WaterAndPowerAllocationsAreNonNegativeAndPlausible()
00201:         {
00202:             var (_, catalog) = LoadAccords();
00203:
00204:             foreach (var treaty in catalog.AllTreaties)
00205:             {
00206:                 // Non-negative allocation invariant
00207:                 Assert.True(treaty.water_allocation_lpm >= 0f,
00208:                     $"Treaty '{treaty.treaty_id}' water allocation must be >= 0");
00209:                 Assert.True(treaty.power_quota_kw >= 0f,
00210:                     $"Treaty '{treaty.treaty_id}' power quota must be >= 0");
00211:
00212:                 // Industrial sanity upper bounds
00213:                 Assert.True(treaty.water_allocation_lpm <= 200f,
00214:                     $"Treaty '{treaty.treaty_id}' water allocation exceeds realistic maximum");
00215:                 Assert.True(treaty.power_quota_kw <= 100f,
00216:                     $"Treaty '{treaty.treaty_id}' power quota exceeds realistic maximum");
00217:             }
00218:         }
00219:
00220:         // ── 6. Legal Prose & Article Formatting ─────────────────────────
00221:
00222:         [Fact]
00223:         public void LegalText_ArticlesFollowNumberedClausesAndPenaltiesAreEnforceable()
00224:         {
00225:             var (_, catalog) = LoadAccords();
00226:
00227:             foreach (var treaty in catalog.AllTreaties)
00228:             {
00229:                 Assert.False(string.IsNullOrWhiteSpace(treaty.treaty_title));
00230:                 Assert.False(string.IsNullOrWhiteSpace(treaty.demarcated_territory));
00231:                 Assert.False(string.IsNullOrWhiteSpace(treaty.tariff_schedule));
00232:                 Assert.False(string.IsNullOrWhiteSpace(treaty.treaty_articles));
00233:                 Assert.False(string.IsNullOrWhiteSpace(treaty.penalties));
00234:
00235:                 // Structured clause convention: ARTICLE 1, ARTICLE 2, etc.
00236:                 Assert.Contains("ARTICLE 1:", treaty.treaty_articles);
00237:                 Assert.Contains("ARTICLE 2:", treaty.treaty_articles);
00238:
00239:                 // Penalty must not be placeholder
00240:                 Assert.True(treaty.penalties.Length >= 15,
00241:                     $"Penalty for '{treaty.treaty_id}' is suspiciously short");
00242:             }
00243:         }
00244:
00245:         // ── 7. Tag Vocabulary Normalization ─────────────────────────────
00246:
00247:         [Fact]
00248:         public void Tags_FollowNormalizedVocabularyWithoutSynonymSplits()
00249:         {
00250:             var (_, catalog) = LoadAccords();
00251:             var recognizedTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
00252:             {
00253:                 "foundry", "saltworks", "brine", "iodine", "exchange", "district8",
00254:                 "labour", "cluster", "school", "schedule", "road", "ice", "anchors",
00255:                 "charter", "garrison", "rebuilders", "grain", "tithe", "verge",
00256:                 "flotilla", "cutters", "maritime", "saline", "coast", "ash_sign",
00257:                 "forward_roster", "switchback", "scarp", "fuel", "the_scale",
00258:                 "trade", "suburbs", "convention", "scrap", "salvage", "industrial",
00259:                 "neutral_ground", "demilitarization", "border", "water", "aquifer",
00260:                 "observatory", "sanctuary", "access", "maintenance", "logistics", "training", "inspection",
00261:                 "emergency", "security", "records", "accountability"
00262:             };
00263:
00264:             foreach (var treaty in catalog.AllTreaties)
00265:             {
00266:                 Assert.NotNull(treaty.tags);
00267:                 Assert.NotEmpty(treaty.tags);
00268:
00269:                 foreach (var tag in treaty.tags)
00270:                 {
00271:                     Assert.False(string.IsNullOrWhiteSpace(tag));
00272:                     Assert.Equal(tag.ToLowerInvariant(), tag);
00273:                     Assert.True(recognizedTags.Contains(tag),
00274:                         $"Treaty '{treaty.treaty_id}' has unrecognized tag '{tag}'");
00275:                 }
00276:             }
00277:         }
00278:
00279:         // ── 8. Timeline Chronology & Ordering ───────────────────────────
00280:
00281:         [Fact]
00282:         public void Timeline_RatificationDaysAreChronologicallyOrdered()
00283:         {
00284:             var (_, catalog) = LoadAccords();
00285:
00286:             foreach (var treaty in catalog.AllTreaties)
00287:             {
00288:                 Assert.True(treaty.ratified_day > 0,
00289:                     $"Treaty '{treaty.treaty_id}' must have positive ratified_day");
00290:                 Assert.True(treaty.ratified_day <= 365,
00291:                     $"Treaty '{treaty.treaty_id}' must occur within campaign year 1 (<= 365)");
00292:             }
00293:
00294:             // Ratification days are the chronology authority; the original
00295:             // regional records remain in their authored order and the six new
00296:             // Foundry records are appended without rewriting legacy entries.
00297:
00298:             // Verify query by ratification day
00299:             var day200Treaties = catalog.GetRatifiedByDay(200);
00300:             Assert.Equal(2, day200Treaties.Count); // grain tithe (120), saline corridor (180)
00301:
00302:             var day300Treaties = catalog.GetRatifiedByDay(300);
00303:             Assert.Equal(9, day300Treaties.Count);
00304:
00305:             var day365Treaties = catalog.GetRatifiedByDay(365);
00306:             Assert.Equal(18, day365Treaties.Count);
00307:         }
00308:
00309:         // ── 9. Functional Diversity Audit ───────────────────────────────
00310:
00311:         [Fact]
00312:         public void Diversity_CatalogSpansResourceLogisticsTerritorialAndGovernanceRoles()
00313:         {
00314:             var (_, catalog) = LoadAccords();
00315:
00316:             // Resource & Infrastructure
00317:             Assert.NotNull(catalog.GetById("treaty_brine_pipe_and_iodine_exchange"));
00318:             Assert.NotNull(catalog.GetById("treaty_deep_coast_aquifer_protection_treaty"));
00319:
00320:             // Logistics & Transport
00321:             Assert.NotNull(catalog.GetById("treaty_road_iron_charter"));
00322:             Assert.NotNull(catalog.GetById("treaty_switchback_fuel_and_passage_accord"));
00323:             Assert.NotNull(catalog.GetById("treaty_flotilla_saline_corridor_concordat"));
00324:
00325:             // Labor & Training
00326:             Assert.NotNull(catalog.GetById("treaty_cluster_labour_schedule"));
00327:
00328:             // Trade & Commerce
00329:             Assert.NotNull(catalog.GetById("treaty_scale_suburban_fair_trade_convention"));
00330:             Assert.NotNull(catalog.GetById("treaty_scrap_salvage_demarcation"));
00331:             Assert.NotNull(catalog.GetById("treaty_garrison_grain_tithe_compact"));
00332:
00333:             // Demilitarization & Security
00334:             Assert.NotNull(catalog.GetById("treaty_roster_border_demilitarization_pact"));
00335:
00336:             // Governance, Accountability & Sanctuary
00337:             Assert.NotNull(catalog.GetById("treaty_the_cluster_charter"));
00338:             Assert.NotNull(catalog.GetById("treaty_high_scarp_observatory_sanctuary"));
00339:         }
00340:
00341:         // ── 10. Consequence Integration Seam ────────────────────────────
00342:
00343:         [Fact]
00344:         public void ConsequenceSeam_Plan103PoliciesResolveAgainstTheseAccords()
00345:         {
00346:             string dataDir = FindDataDir();
00347:             var files = new FileSystemIO();
00348:             var json = new SystemTextJsonSerializer();
00349:
00350:             var consequenceFile = SilentFoundryConsequenceCatalogLoader.Load(dataDir, files, json);
00351:             var (_, catalog) = LoadAccords();
00352:
00353:             Assert.NotEmpty(consequenceFile.policies);
00354:             foreach (var policy in consequenceFile.policies)
00355:             {
00356:                 var accord = catalog.GetById(policy.treaty_id);
00357:                 Assert.NotNull(accord);
00358:                 Assert.Contains(accord.signatory_factions, f => f == policy.faction_id);
00359:             }
00360:         }
00361:     }
00362: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **The safe subject is the 18-row treaty catalog and the existing Foundry owner chain, not another diplomacy feature.**.

The first polish also checks continuity against the master authority: the plan is one bounded outcome, uses an existing owner seam, names a data authority, and does not widen into unrelated economy, UI, save or content work. Any apparently attractive addition that lacks a current owner is recorded as out of scope rather than smuggled into the architecture.

# Appendix — Polishing Pass 2: Integration and Code Architecture

This pass turns the evidence into an executable route. It distinguishes Core domain rules, host composition, Godot presentation, current save envelopes, deterministic streams, typed events and focused tests. The route is deliberately extend-first. A future builder may add a field, catalog row, read model or host adapter only after claiming the exact path and proving that the existing owner can accept it.

The second pass also reviews the handoff from data to player experience. A row that cannot be reached from a command is an orphan; a command that updates a shadow field is a split authority; a panel that recomputes a result is a presentation bug; a save that restores a display but not the owner is a persistence bug. Each failure is given a focused negative obligation.

# Appendix — Final Precision and Reaccuracy Pass

Before handoff, re-read every current path, hash, catalog count, public declaration, test declaration and save owner named above. Correct stale terminology, remove fictional type names, replace old section pins with current owner names, and downgrade any unsupported pass claim to historical evidence. Re-run the structural verifier after this pass. The final artifact should let a builder execute the first safe step without reinterpreting ownership.

**Precision result:** current implementation claims are separated from future proposals; content is not counted as reachability; Core remains engine-free; UI remains a projection; save and determinism are explicit; and every residual gap has a named verification route. If a future source audit contradicts this record, the source wins and the plan returns `STALE_PLAN` for re-audit.

# Appendix — Quality Assurance Pass Record

This record is part of the planning artifact, not a fresh runtime test result.

## Pass A — premise and content
- Current catalog rows, source owners, historical closeout and remaining residual are separated.
- The old baseline count is not presented as the current count.
- No copied, real-world, fabricated or unowned content is proposed.

## Pass B — integration architecture
- Data → loader → Core owner → host command → UI/event → save → replay is named.
- Existing save sections and codecs are identified; no parallel section is invented.
- Deterministic ordering, no-RNG cases, seeded streams and legacy defaults are explicit.

## Pass C — precision and handoff
- Every referenced current path is hash-pinned in the evidence appendices.
- Proposed future seams are labeled as proposals and excluded from current claims.
- Focused test commands, failure responses, rollback and non-goals are included.
