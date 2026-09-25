# Plan 37 — Excavation Sites, Archaeology and Deep-Strata Reachability

> **Rebuild status:** COMPLETE 8-SITE CATALOG — CORE LOADER AND EXCAVATION UI EXIST; NORMAL-PLAY CRAWLABILITY IS THE RESIDUAL
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

- The current catalog has 8 authored sites with depth bands, hazards, shoring costs and history. ExcavationCatalogLoader parses it; ExcavationSystem owns mutable dig state; ExcavationHazardSystem owns hazard exposure; the host and UI already provide an excavation route.

**Bounded outcome:** Retire the no-data premise. excavation_sites.json now has 8 rows, ExcavationCatalogLoader and ExcavationSystem are live, and the ArchaeologyExcavationPanel is routed through the current player surface. The residual is to audit how catalog sites become selectable excavation work without creating a second archaeology or hazard system.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- Map each authored site to the current excavation/archaeology route and identify whether all sites are actually selectable. Keep catalog definitions, mutable dig state, hazard state and archaeology discovery separate.

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
- The valuable future change is reachability and content-reference proof, not more rows.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- The required delta is a reachability and contract audit, not another site catalog. Any new site must use existing loader, hazard, inventory and panel paths.

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
| site definitions and depth-band parsing | ExcavationCatalogLoader | `Assets/Ashfall.Core/Excavation/ExcavationCatalogLoader.cs` | Static site catalog owner. |
| mutable dig progress, workers, shoring and collapse state | ExcavationSystem | `Assets/Ashfall.Core/ExcavationSystem.cs` | Sole excavation state owner. |
| hazard exposure and mitigation state | ExcavationHazardSystem | `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs` | Sole hazard state owner. |
| artifact discovery/decryption route | ArchaeologySystem | `Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs` | Separate archaeology concern. |
| host adapter and save boundary | ExcavationHostSession | `src/Host/ExcavationHostSession.cs` | Thin adapter. |
| current player surface | ArchaeologyExcavationPanel | `src/UI/ArchaeologyExcavationPanel.cs` | Presentation and commands only. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Excavation Sites, Archaeology and Deep-Strata Reachability
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ ExcavationCatalogLoader
│   site definitions and depth-band parsing
│ ExcavationSystem
│   mutable dig progress, workers, shoring and collapse state
│ ExcavationHazardSystem
│   hazard exposure and mitigation state
│ ArchaeologySystem
│   artifact discovery/decryption route
│ ExcavationHostSession
│   host adapter and save boundary
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

1. **Preserve current state ownership.** ExcavationCatalogLoader owns site definitions and depth-band parsing: Static site catalog owner.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| site definitions and depth-band parsing | ExcavationCatalogLoader | `Assets/Ashfall.Core/Excavation/ExcavationCatalogLoader.cs` | Static site catalog owner. |
| mutable dig progress, workers, shoring and collapse state | ExcavationSystem | `Assets/Ashfall.Core/ExcavationSystem.cs` | Sole excavation state owner. |
| hazard exposure and mitigation state | ExcavationHazardSystem | `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs` | Sole hazard state owner. |
| artifact discovery/decryption route | ArchaeologySystem | `Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs` | Separate archaeology concern. |
| host adapter and save boundary | ExcavationHostSession | `src/Host/ExcavationHostSession.cs` | Thin adapter. |
| current player surface | ArchaeologyExcavationPanel | `src/UI/ArchaeologyExcavationPanel.cs` | Presentation and commands only. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load eight authored sites
2. validate depth/risk/hazard references
3. show current dig records
4. route worker/shoring actions through ExcavationSystem
5. apply hazard mitigation through hazard owner
6. record archaeology discovery through archaeology owner
7. capture existing excavation/hazard/archaeology state

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- The eight catalog rows are immutable definitions.
- ExcavationState owns active dig records.
- Hazard state is not copied into the site catalog or panel.
- Archaeology discovery consumes current excavation outcomes rather than replacing them.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A site must be a current catalog ID before it can enter a dig record.
- Unknown hazard or mitigation IDs fail closed without state mutation.
- The panel cannot calculate collapse or reward outcomes.
- A discovery is emitted once under the archaeology owner contract.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- excavation_sites.json and excavation_hazard_mitigation.json are the site/hazard authorities.
- Items, archaeology, disease and expedition catalogs own their references.
- No duplicate site or dig-progress catalog is justified.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use ExcavationSaveStore, hazard save and archaeology existing saves.
- No Plan-37 save section.
- Old empty excavation state must remain a valid fresh state.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Risk, depth and collapse use the existing seeded stream.
- Catalog ordering is stable; no dictionary/hash order decides dig selection.
- Same site, state and seed produce the same outcome.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Catalog load emits definitions, not gameplay progress.
- ExcavationSystem emits changed facts after progress/worker/shoring commands.
- Hazard mitigation is applied through its current owner.
- Panel refresh emits no domain event.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/ExcavationHostSession.cs
- src/UI/ArchaeologyExcavationPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Site history is restrained fictional environmental storytelling.
- Hazards remain plausible and do not provide real-world excavation instructions.
- Artifacts feed existing archaeology/lore systems rather than a parallel codex.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A catalog site has no selectable route. | ExcavationCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | Panel invents a site or hazard outcome. | ExcavationSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Shoring cost is charged twice. | ExcavationHazardSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Hazard mitigation mutates the wrong owner. | ArchaeologySystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new save section duplicates dig state. | ExcavationHostSession | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/World/ExcavationSitesCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/ExcavationIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/ExcavationSystemTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/ExcavationSystemTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 | Census all eight sites and references. | Every site has a current owner classification. | No production path until the owning implementation package is separately claimed. |
| 1 | Trace host/UI selection and current commands. | No decorative site is called playable. | No production path until the owning implementation package is separately claimed. |
| 2 | Audit hazard, archaeology and save boundaries. | Cross-system effects remain single-owner. | No production path until the owning implementation package is separately claimed. |
| 3 | Polish route, focus and feedback. | Only proven gaps are handed off. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/excavation_sites.json | READ ONLY; MODIFY only for content/reference defect | 8 sites |
| Assets/Ashfall.Core/Excavation/ExcavationCatalogLoader.cs | READ ONLY | Loader |
| Assets/Ashfall.Core/ExcavationSystem.cs | READ ONLY | Mutable dig state |
| src/UI/ArchaeologyExcavationPanel.cs | READ ONLY | Current UI route |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Creating a second excavation system. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Treating a catalog row as a reachable quest. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Duplicating hazard or archaeology state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding a new save section. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new site rows.
- No new hazard engine.
- No production/data/test/UI changes here.

# 23. Rollback and Recovery

- Revert the planning artifact.
- Future route fixes are isolated and save-compatible.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 8 current sites and loader/UI evidence are documented.
- All site/hazard/dig/archaeology owners are explicit.
- Focused tests and UI/accessibility contracts are named.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- The required delta is a reachability and contract audit, not another site catalog. Any new site must use existing loader, hazard, inventory and panel paths.

## MUST NOT DO

- No new site rows.
- No new hazard engine.
- No production/data/test/UI changes here.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/World/ExcavationSitesCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/ExcavationIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/ExcavationSystemTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/ExcavationSystemTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: site definitions and depth-band parsing → ExcavationCatalogLoader; mutable dig progress, workers, shoring and collapse state → ExcavationSystem; hazard exposure and mitigation state → ExcavationHazardSystem; artifact discovery/decryption route → ArchaeologySystem; host adapter and save boundary → ExcavationHostSession; current player surface → ArchaeologyExcavationPanel. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 37.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 37 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by ExcavationCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Excavation/ExcavationCatalogLoader.cs`

### `Assets/Ashfall.Core/Excavation/ExcavationCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 272 lines / 15853 bytes.
- SHA-256: `9ee0c361e472225c4c3f5b950f4bed2a264719375c68d1916598a83665f76409`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExcavationCatalogContainer
public int schema_version { get; set; } = 1;
public List<ExcavationSiteDef> sites { get; set; } = new List<ExcavationSiteDef>();
public sealed class ExcavationSiteDef
public string site_id { get; set; } = string.Empty;
public string location_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public float max_depth_meters { get; set; } = 100f;
public float required_progress { get; set; } = 100f;
public float structural_risk { get; set; } = 0.3f;
public List<string> required_tools { get; set; } = new List<string>();
public List<string> shoring_materials { get; set; } = new List<string>();
public string hazard_type { get; set; } = string.Empty;
public string relic_reward_id { get; set; } = string.Empty;
public string loot_table { get; set; } = "salvage_common";
public List<ExcavationDepthBandDef> depth_bands { get; set; } = new List<ExcavationDepthBandDef>();
public string journal_entry_id { get; set; } = string.Empty;
public sealed class ExcavationDepthBandDef
public float depth_meters { get; set; }
public string label { get; set; } = string.Empty;
public float risk { get; set; }
public static class ExcavationCatalogLoader
public const string CatalogFileName = "excavation_sites.json";
public static List<ExcavationSiteDef> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null) {
public static List<ExcavationSiteDef> GetDefaultSites() {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/ExcavationSystem.cs`

### `Assets/Ashfall.Core/ExcavationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 164 lines / 7185 bytes.
- SHA-256: `ee423dfdbe7885f439587d5dff23697bad53ef3f7df05bf6a756d2032f5b029f`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExcavationState
public string systemId = ExcavationSystem.SystemId;
public List<ExcavationSite> sites = new List<ExcavationSite>();
public sealed class ExcavationSite
public string siteId = string.Empty;
public string roomBlueprintId = string.Empty;
public float progress;
public float requiredProgress = 100f;
public int assignedWorkerCount;
public float structuralRisk; // 0-1, risk of cave-in
public bool hasCavedIn;
public bool isComplete;
public List<string> requiredTools = new List<string>();
public bool shoringApplied;
public int reinforcedBeams; // Plans 90-93: cast beams set through the foundry loop
public List<string> discoveredCaches = new List<string>();
public sealed class ExcavationSystem
public const string SystemId = "excavation";
public const string StructuralBeamItemId = "item_foundry_t_beam";
public const int StructuralBeamCost = 2;
public ExcavationState State => _state;
public event Action OnExcavationChanged;
public ActionResult AddSite(string siteId, string roomBlueprintId, float requiredProgress, float risk) {
public ActionResult AssignWorkers(string siteId, int count) {
public ActionResult ApplyShoring(string siteId) {
public ActionResult TryApplyStructuralReinforcement(string siteId) {
public void TickDay() {
public ExcavationState CaptureState() => CloneState(_state);
public void RestoreState(ExcavationState saved) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs`

### `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 482 lines / 19643 bytes.
- SHA-256: `2251ec3acbcde282740467faea55ab88e306def6f56f2df73be2d5484765a42f`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=21; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MitigationItemCost
public string ItemId { get; set; } = string.Empty;
public int Amount { get; set; } = 1;
public sealed class ExcavationMitigationDefinition
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public List<string> HazardTags { get; set; } = new List<string>();
public List<MitigationItemCost> RequiredItems { get; set; } = new List<MitigationItemCost>();
public int LaborTicks { get; set; } = 60;
public Dictionary<string, int> Effect { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
public bool RequiresRespiratoryProtection { get; set; }
public List<string> Tags { get; set; } = new List<string>();
public sealed class ExcavationHazardCatalogData
public int SchemaVersion { get; set; } = 1;
public List<ExcavationMitigationDefinition> Mitigations { get; set; } = new List<ExcavationMitigationDefinition>();
public sealed class ExcavationSectorHazardState
public string SectorId { get; set; } = string.Empty;
public int MethanePpm { get; set; } = 500; // 0 - 10000+
public int FloodLevelPermille { get; set; } = 0; // 0 - 1000
public int SporeConcentrationPermille { get; set; } = 0; // 0 - 1000
public int ShoringHealthPermille { get; set; } = 1000; // 0 - 1000
public bool IsBulkheadSealed { get; set; }
public List<string> InstalledMitigationIds { get; set; } = new List<string>();
public List<string> ActiveTrappedMiners { get; set; } = new List<string>();
public int? RescueDeadlineDay { get; set; }
public int RescueLaborRemaining { get; set; }
public bool RescueCompleted { get; set; }
public bool RescueFailed { get; set; }
public sealed class ExcavationHazardSave
public string systemId = ExcavationHazardSystem.SystemId;
public int schemaVersion = 1;
public Dictionary<string, ExcavationSectorHazardState> sectors = new(StringComparer.Ordinal);
public int currentDay;
public sealed class ExcavationHazardSystem
public const string SystemId = "excavation_hazards";
public ExcavationHazardSave State => _state;
public IReadOnlyDictionary<string, ExcavationMitigationDefinition> Catalog => _catalog;
public const int MethaneIgnitionThresholdPpm = 4000;
public const int FloodCriticalThresholdPermille = 500;
public event Action<string, string>? OnMitigationInstalled; // sectorId, mitigationId
public event Action<string>? OnMethaneIgnition; // sectorId
public event Action<string>? OnSectorFlooded;
public event Action<string, int>? OnRescueStarted; // sectorId, trappedCount
public event Action<string>? OnRescueSucceeded;
public event Action<string>? OnRescueFailed;
public event Action? OnHazardStateChanged;
public void LoadCatalog(ExcavationHazardCatalogData? data) {
public void LoadCatalog(string json) {
public ExcavationSectorHazardState GetOrCreateSector(string sectorId) {
public bool CanApplyMitigation(string sectorId, string mitigationId, out string reason) {
public ActionResult TryApplyMitigation(string sectorId, string mitigationId, IReadOnlyList<string>? workerIds = null) {
public ActionResult TryToggleBulkhead(string sectorId, bool seal, out string reason) {
public void TriggerCaveInRescue( string sectorId, IReadOnlyList<string> trappedSurvivorIds, int deadlineDays = 3, int requiredLabor = 240) {
public void ProgressRescueLabor(string sectorId, int laborAmount) {
public void AddMethane(string sectorId, int deltaPpm, int cap = int.MaxValue) {
public bool AddFloodWater(string sectorId, int deltaPermille) {
public void TickDay(int day) {
public ExcavationHazardSave CaptureState() {
public void RestoreState(ExcavationHazardSave? saved) {
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs`

### `Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 570 lines / 24961 bytes.
- SHA-256: `4e0292bdf601c43ef6d76ebba89b0ed7f6761bcc14b6b8ac81ff2b5857fc3a9b`.
- Architecture signals: seeded references=2; save/restore symbols=5; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class LoreArchiveDef
public string archive_id { get; set; } = string.Empty;
public string title_key { get; set; } = string.Empty;
public string summary_key { get; set; } = string.Empty;
public string era { get; set; } = "PreExchange";
public List<string> topic_tags { get; set; } = new List<string>();
public int encryption_tier { get; set; } = 1;
public float required_engineering { get; set; } = 2.0f;
public float base_work_hours { get; set; } = 12.0f;
public float power_kw { get; set; } = 2.0f;
public float corruption_risk { get; set; } = 0.08f;
public string required_key_item_id { get; set; } = "item_decryption_keycard_prewar";
public int research_reward { get; set; } = 20;
public float broker_value { get; set; } = 100.0f;
public bool unique { get; set; } = true;
public sealed class ArchaeologyCatalogContainer
public int schema_version { get; set; } = 1;
public List<LoreArchiveDef> archives { get; set; } = new List<LoreArchiveDef>();
public sealed class ExcavationSite
public string siteId { get; set; } = string.Empty;
public string zoneId { get; set; } = string.Empty;
public string displayName { get; set; } = string.Empty;
public bool discovered { get; set; }
public float excavationProgress { get; set; } // 0..100
public bool exhausted { get; set; }
public string archiveId { get; set; } = string.Empty;
public sealed class PreWarArchiveInstance
public string archiveId { get; set; } = string.Empty;
public string titleKey { get; set; } = string.Empty;
public string summaryKey { get; set; } = string.Empty;
public int encryptionTier { get; set; } = 1;
public float decryptionProgress { get; set; } // 0..100
public bool encrypted { get; set; } = true;
public bool corrupted { get; set; }
public bool unlocked { get; set; }
public bool sold { get; set; }
public bool researchClaimed { get; set; }
public int researchPoints { get; set; } = 20;
public float brokerValue { get; set; } = 100.0f;
public sealed class ArchaeologyState
public string systemId = ArchaeologySystem.SystemId;
public List<ExcavationSite> sites = new List<ExcavationSite>();
public List<PreWarArchiveInstance> archives = new List<PreWarArchiveInstance>();
public List<string> unlockedLoreIds = new List<string>();
public List<string> soldArchiveIds = new List<string>();
public sealed class ArchaeologySystem
public const string SystemId = "archaeology";
public ArchaeologyState State => CaptureState();
public IReadOnlyList<ExcavationSite> Sites => CaptureState().sites;
public IReadOnlyList<PreWarArchiveInstance> Archives => CaptureState().archives;
public event Action<ExcavationSite>? OnExcavationSiteDiscovered;
public event Action<PreWarArchiveInstance>? OnArchiveRecovered;
public event Action<PreWarArchiveInstance>? OnDecryptionStarted;
public event Action<PreWarArchiveInstance>? OnArchiveCorrupted;
public event Action<PreWarArchiveInstance, int>? OnLoreUnlocked;
public event Action<PreWarArchiveInstance, float>? OnArchiveSold;
public void LoadCatalog(string dataPath) {
public void RegisterArchive(LoreArchiveDef def) {
public ExcavationSite? SurveyRuins(string zoneId, float scoutSkill) {
public PreWarArchiveInstance? ProgressExcavation(string siteId, float laborHours) {
public ActionResult ProgressDecryption( string archiveId, float hours, float engineerSkill, bool hasPower, bool hasKeycard = false)
public ActionResult SellArchiveToBroker(string archiveId) {
public ArchaeologyState CaptureState() => NormalizeState(_state);
public void RestoreState(ArchaeologyState state) {
```


# Appendix B.06 — Current Code Architecture: `src/Host/ExcavationHostSession.cs`

### `src/Host/ExcavationHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 72 lines / 2200 bytes.
- SHA-256: `82d484a67d1feeb0bf32a6f1f04c3131ea7d31d947253eaa8c25784dcbb83109`.
- Architecture signals: seeded references=1; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExcavationHostSession
public ExcavationSystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public ActionResult AddSite(string siteId, string blueprintId, float requiredProgress = 100f, float risk = 0.2f) {
public ActionResult AssignWorkers(string siteId, int workerCount) {
public ActionResult ApplyShoring(string siteId) {
public void TickDay() {
public override void Save() {
```


# Appendix B.07 — Current Code Architecture: `src/Host/ExcavationHostSession.cs`

### `src/Host/ExcavationHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 72 lines / 2200 bytes.
- SHA-256: `82d484a67d1feeb0bf32a6f1f04c3131ea7d31d947253eaa8c25784dcbb83109`.
- Architecture signals: seeded references=1; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExcavationHostSession
public ExcavationSystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public ActionResult AddSite(string siteId, string blueprintId, float requiredProgress = 100f, float risk = 0.2f) {
public ActionResult AssignWorkers(string siteId, int workerCount) {
public ActionResult ApplyShoring(string siteId) {
public void TickDay() {
public override void Save() {
```


# Appendix B.08 — Current Code Architecture: `src/UI/ArchaeologyExcavationPanel.cs`

### `src/UI/ArchaeologyExcavationPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 226 lines / 10735 bytes.
- SHA-256: `a57113819ff5b232473cdccda842264cbee2f4fa37564a93a9f850a45364ff3b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=7; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class ArchaeologyExcavationPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<string, string>? OnActionRequested;
public bool IsBound => _system != null;
public void Bind(ArchaeologySystem system) { _system = system; _feedbackText = string.Empty; RefreshView(); }
public void Unbind() { _system = null; }
public override void _Ready() {
public void Open() { Visible = true; RefreshView(); }
public void Close() {
public string LastFeedback { get; private set; } = string.Empty;
public void ShowFeedback(string message, bool isFailure) {
public void RefreshView() {
public override void _UnhandledInput(InputEvent @event) {
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/excavation_sites.json`

### `Assets/StreamingAssets/Data/excavation_sites.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 10152 bytes / 10152 characters.
- SHA-256: `0318a4379b0fb9a640d090766f8079663f598e1c0de524633ec7add986fd42c8`.
- Root keys: `schema_version`, `sites`.

Array-path census (minimum, maximum, observed rows):

```text
sites: min=8, max=8, observed_paths=1
sites[].depth_bands: min=3, max=4, observed_paths=2
sites[].required_tools: min=2, max=2, observed_paths=2
sites[].shoring_materials: min=2, max=2, observed_paths=2
```

Representative record fields:

- `depth_bands`
- `description`
- `display_name`
- `hazard_type`
- `journal_entry_id`
- `location_id`
- `loot_table`
- `max_depth_meters`
- `relic_reward_id`
- `required_progress`
- `required_tools`
- `shoring_materials`
- `site_id`
- `structural_risk`


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json`

### `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 4945 bytes / 4945 characters.
- SHA-256: `0910d261385a532cbebbc8483b3705b6b4edae19c60f20da7c67b63a10655fca`.
- Root keys: `mitigations`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
mitigations: min=8, max=8, observed_paths=1
mitigations[].hazard_tags: min=1, max=1, observed_paths=2
mitigations[].required_items: min=1, max=2, observed_paths=2
mitigations[].tags: min=3, max=3, observed_paths=2
```

Representative record fields:

- `display_name`
- `effect`
- `hazard_tags`
- `id`
- `labor_ticks`
- `required_items`
- `requires_respiratory_protection`
- `tags`

Representative identifiers (ordered, capped for readability):

```text
mitigation_ventilation_blower_install
mitigation_methane_flare_burnoff
mitigation_sump_drainage_pump
mitigation_chemical_spore_scrub
mitigation_timber_shoring_reinforcement
mitigation_emergency_bulkhead_seal
mitigation_sky_armor_blast_matting
mitigation_trapped_miner_clearance
```


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/World/ExcavationSitesCatalogTests.cs`

### `Ashfall.Core.Tests/World/ExcavationSitesCatalogTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 287; SHA-256: `27c7bd3e6cee1a9314331e730467200567fde2991faaec44428d34df55ae9057`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ExcavationCatalog_LoadsAllEightAuthoredSites
ExcavationCatalog_AllSiteIdsAreUnique
ExcavationCatalog_AllLocationReferencesResolveInLocationsCatalog
ExcavationCatalog_DepthBandsAreOrderedAndWithinRange
ExcavationCatalog_DepthBandsSpanShallowMediumDeepTiers
ExcavationCatalog_HazardCoverageMatchesDesignContract
ExcavationCatalog_RelicRewardLinksAreValid
ExcavationCatalog_DefaultSitesFallbackMatchesEightSites
ExcavationSystem_MultiSiteParallelExcavation_AndDeterministicProgress
ExcavationSystem_SaveAndRestore_MaintainsAllEightSitesState
```


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/ExcavationIntegrationTests.cs`

### `Ashfall.Core.Tests/ExcavationIntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 52; SHA-256: `9453aa7a85726d194258b32e437ff25c4d6bf5d9a7c13c50db20a9a86b9ddfe0`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AddSite_AssignWorkers_AndTick_AdvancesProgress
ApplyShoring_ReducesRisk
SaveAndRestore_PreservesSitesAndProgress
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/ExcavationSystemTests.cs`

### `Ashfall.Core.Tests/ExcavationSystemTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 68; SHA-256: `4b061ec9bf026c10cfbddbf1b405368f18811d016c0eeef3ba2afb2c66603a00`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/ExcavationSystemTests.cs`

### `Ashfall.Core.Tests/ExcavationSystemTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 68; SHA-256: `4b061ec9bf026c10cfbddbf1b405368f18811d016c0eeef3ba2afb2c66603a00`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Excavation/ExcavationCatalogLoader.cs`

### `Assets/Ashfall.Core/Excavation/ExcavationCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 272 lines / 15853 bytes.
- SHA-256: `9ee0c361e472225c4c3f5b950f4bed2a264719375c68d1916598a83665f76409`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExcavationCatalogContainer
public int schema_version { get; set; } = 1;
public List<ExcavationSiteDef> sites { get; set; } = new List<ExcavationSiteDef>();
public sealed class ExcavationSiteDef
public string site_id { get; set; } = string.Empty;
public string location_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public float max_depth_meters { get; set; } = 100f;
public float required_progress { get; set; } = 100f;
public float structural_risk { get; set; } = 0.3f;
public List<string> required_tools { get; set; } = new List<string>();
public List<string> shoring_materials { get; set; } = new List<string>();
public string hazard_type { get; set; } = string.Empty;
public string relic_reward_id { get; set; } = string.Empty;
public string loot_table { get; set; } = "salvage_common";
public List<ExcavationDepthBandDef> depth_bands { get; set; } = new List<ExcavationDepthBandDef>();
public string journal_entry_id { get; set; } = string.Empty;
public sealed class ExcavationDepthBandDef
public float depth_meters { get; set; }
public string label { get; set; } = string.Empty;
public float risk { get; set; }
public static class ExcavationCatalogLoader
public const string CatalogFileName = "excavation_sites.json";
public static List<ExcavationSiteDef> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null) {
public static List<ExcavationSiteDef> GetDefaultSites() {
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/ExcavationSystem.cs`

### `Assets/Ashfall.Core/ExcavationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 164 lines / 7185 bytes.
- SHA-256: `ee423dfdbe7885f439587d5dff23697bad53ef3f7df05bf6a756d2032f5b029f`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExcavationState
public string systemId = ExcavationSystem.SystemId;
public List<ExcavationSite> sites = new List<ExcavationSite>();
public sealed class ExcavationSite
public string siteId = string.Empty;
public string roomBlueprintId = string.Empty;
public float progress;
public float requiredProgress = 100f;
public int assignedWorkerCount;
public float structuralRisk; // 0-1, risk of cave-in
public bool hasCavedIn;
public bool isComplete;
public List<string> requiredTools = new List<string>();
public bool shoringApplied;
public int reinforcedBeams; // Plans 90-93: cast beams set through the foundry loop
public List<string> discoveredCaches = new List<string>();
public sealed class ExcavationSystem
public const string SystemId = "excavation";
public const string StructuralBeamItemId = "item_foundry_t_beam";
public const int StructuralBeamCost = 2;
public ExcavationState State => _state;
public event Action OnExcavationChanged;
public ActionResult AddSite(string siteId, string roomBlueprintId, float requiredProgress, float risk) {
public ActionResult AssignWorkers(string siteId, int count) {
public ActionResult ApplyShoring(string siteId) {
public ActionResult TryApplyStructuralReinforcement(string siteId) {
public void TickDay() {
public ExcavationState CaptureState() => CloneState(_state);
public void RestoreState(ExcavationState saved) {
```


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs`

### `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 482 lines / 19643 bytes.
- SHA-256: `2251ec3acbcde282740467faea55ab88e306def6f56f2df73be2d5484765a42f`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=21; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MitigationItemCost
public string ItemId { get; set; } = string.Empty;
public int Amount { get; set; } = 1;
public sealed class ExcavationMitigationDefinition
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public List<string> HazardTags { get; set; } = new List<string>();
public List<MitigationItemCost> RequiredItems { get; set; } = new List<MitigationItemCost>();
public int LaborTicks { get; set; } = 60;
public Dictionary<string, int> Effect { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
public bool RequiresRespiratoryProtection { get; set; }
public List<string> Tags { get; set; } = new List<string>();
public sealed class ExcavationHazardCatalogData
public int SchemaVersion { get; set; } = 1;
public List<ExcavationMitigationDefinition> Mitigations { get; set; } = new List<ExcavationMitigationDefinition>();
public sealed class ExcavationSectorHazardState
public string SectorId { get; set; } = string.Empty;
public int MethanePpm { get; set; } = 500; // 0 - 10000+
public int FloodLevelPermille { get; set; } = 0; // 0 - 1000
public int SporeConcentrationPermille { get; set; } = 0; // 0 - 1000
public int ShoringHealthPermille { get; set; } = 1000; // 0 - 1000
public bool IsBulkheadSealed { get; set; }
public List<string> InstalledMitigationIds { get; set; } = new List<string>();
public List<string> ActiveTrappedMiners { get; set; } = new List<string>();
public int? RescueDeadlineDay { get; set; }
public int RescueLaborRemaining { get; set; }
public bool RescueCompleted { get; set; }
public bool RescueFailed { get; set; }
public sealed class ExcavationHazardSave
public string systemId = ExcavationHazardSystem.SystemId;
public int schemaVersion = 1;
public Dictionary<string, ExcavationSectorHazardState> sectors = new(StringComparer.Ordinal);
public int currentDay;
public sealed class ExcavationHazardSystem
public const string SystemId = "excavation_hazards";
public ExcavationHazardSave State => _state;
public IReadOnlyDictionary<string, ExcavationMitigationDefinition> Catalog => _catalog;
public const int MethaneIgnitionThresholdPpm = 4000;
public const int FloodCriticalThresholdPermille = 500;
public event Action<string, string>? OnMitigationInstalled; // sectorId, mitigationId
public event Action<string>? OnMethaneIgnition; // sectorId
public event Action<string>? OnSectorFlooded;
public event Action<string, int>? OnRescueStarted; // sectorId, trappedCount
public event Action<string>? OnRescueSucceeded;
public event Action<string>? OnRescueFailed;
public event Action? OnHazardStateChanged;
public void LoadCatalog(ExcavationHazardCatalogData? data) {
public void LoadCatalog(string json) {
public ExcavationSectorHazardState GetOrCreateSector(string sectorId) {
public bool CanApplyMitigation(string sectorId, string mitigationId, out string reason) {
public ActionResult TryApplyMitigation(string sectorId, string mitigationId, IReadOnlyList<string>? workerIds = null) {
public ActionResult TryToggleBulkhead(string sectorId, bool seal, out string reason) {
public void TriggerCaveInRescue( string sectorId, IReadOnlyList<string> trappedSurvivorIds, int deadlineDays = 3, int requiredLabor = 240) {
public void ProgressRescueLabor(string sectorId, int laborAmount) {
public void AddMethane(string sectorId, int deltaPpm, int cap = int.MaxValue) {
public bool AddFloodWater(string sectorId, int deltaPermille) {
public void TickDay(int day) {
public ExcavationHazardSave CaptureState() {
public void RestoreState(ExcavationHazardSave? saved) {
```


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs`

### `Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 570 lines / 24961 bytes.
- SHA-256: `4e0292bdf601c43ef6d76ebba89b0ed7f6761bcc14b6b8ac81ff2b5857fc3a9b`.
- Architecture signals: seeded references=2; save/restore symbols=5; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class LoreArchiveDef
public string archive_id { get; set; } = string.Empty;
public string title_key { get; set; } = string.Empty;
public string summary_key { get; set; } = string.Empty;
public string era { get; set; } = "PreExchange";
public List<string> topic_tags { get; set; } = new List<string>();
public int encryption_tier { get; set; } = 1;
public float required_engineering { get; set; } = 2.0f;
public float base_work_hours { get; set; } = 12.0f;
public float power_kw { get; set; } = 2.0f;
public float corruption_risk { get; set; } = 0.08f;
public string required_key_item_id { get; set; } = "item_decryption_keycard_prewar";
public int research_reward { get; set; } = 20;
public float broker_value { get; set; } = 100.0f;
public bool unique { get; set; } = true;
public sealed class ArchaeologyCatalogContainer
public int schema_version { get; set; } = 1;
public List<LoreArchiveDef> archives { get; set; } = new List<LoreArchiveDef>();
public sealed class ExcavationSite
public string siteId { get; set; } = string.Empty;
public string zoneId { get; set; } = string.Empty;
public string displayName { get; set; } = string.Empty;
public bool discovered { get; set; }
public float excavationProgress { get; set; } // 0..100
public bool exhausted { get; set; }
public string archiveId { get; set; } = string.Empty;
public sealed class PreWarArchiveInstance
public string archiveId { get; set; } = string.Empty;
public string titleKey { get; set; } = string.Empty;
public string summaryKey { get; set; } = string.Empty;
public int encryptionTier { get; set; } = 1;
public float decryptionProgress { get; set; } // 0..100
public bool encrypted { get; set; } = true;
public bool corrupted { get; set; }
public bool unlocked { get; set; }
public bool sold { get; set; }
public bool researchClaimed { get; set; }
public int researchPoints { get; set; } = 20;
public float brokerValue { get; set; } = 100.0f;
public sealed class ArchaeologyState
public string systemId = ArchaeologySystem.SystemId;
public List<ExcavationSite> sites = new List<ExcavationSite>();
public List<PreWarArchiveInstance> archives = new List<PreWarArchiveInstance>();
public List<string> unlockedLoreIds = new List<string>();
public List<string> soldArchiveIds = new List<string>();
public sealed class ArchaeologySystem
public const string SystemId = "archaeology";
public ArchaeologyState State => CaptureState();
public IReadOnlyList<ExcavationSite> Sites => CaptureState().sites;
public IReadOnlyList<PreWarArchiveInstance> Archives => CaptureState().archives;
public event Action<ExcavationSite>? OnExcavationSiteDiscovered;
public event Action<PreWarArchiveInstance>? OnArchiveRecovered;
public event Action<PreWarArchiveInstance>? OnDecryptionStarted;
public event Action<PreWarArchiveInstance>? OnArchiveCorrupted;
public event Action<PreWarArchiveInstance, int>? OnLoreUnlocked;
public event Action<PreWarArchiveInstance, float>? OnArchiveSold;
public void LoadCatalog(string dataPath) {
public void RegisterArchive(LoreArchiveDef def) {
public ExcavationSite? SurveyRuins(string zoneId, float scoutSkill) {
public PreWarArchiveInstance? ProgressExcavation(string siteId, float laborHours) {
public ActionResult ProgressDecryption( string archiveId, float hours, float engineerSkill, bool hasPower, bool hasKeycard = false)
public ActionResult SellArchiveToBroker(string archiveId) {
public ArchaeologyState CaptureState() => NormalizeState(_state);
public void RestoreState(ArchaeologyState state) {
```


# Appendix E.19 — Supporting Code Evidence: `src/Host/ExcavationHostSession.cs`

### `src/Host/ExcavationHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 72 lines / 2200 bytes.
- SHA-256: `82d484a67d1feeb0bf32a6f1f04c3131ea7d31d947253eaa8c25784dcbb83109`.
- Architecture signals: seeded references=1; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExcavationHostSession
public ExcavationSystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public ActionResult AddSite(string siteId, string blueprintId, float requiredProgress = 100f, float risk = 0.2f) {
public ActionResult AssignWorkers(string siteId, int workerCount) {
public ActionResult ApplyShoring(string siteId) {
public void TickDay() {
public override void Save() {
```


# Appendix F.20 — Supporting Data Evidence: `Assets/StreamingAssets/Data/excavation_sites.json`

### `Assets/StreamingAssets/Data/excavation_sites.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 10152 bytes / 10152 characters.
- SHA-256: `0318a4379b0fb9a640d090766f8079663f598e1c0de524633ec7add986fd42c8`.
- Root keys: `schema_version`, `sites`.

Array-path census (minimum, maximum, observed rows):

```text
sites: min=8, max=8, observed_paths=1
sites[].depth_bands: min=3, max=4, observed_paths=2
sites[].required_tools: min=2, max=2, observed_paths=2
sites[].shoring_materials: min=2, max=2, observed_paths=2
```

Representative record fields:

- `depth_bands`
- `description`
- `display_name`
- `hazard_type`
- `journal_entry_id`
- `location_id`
- `loot_table`
- `max_depth_meters`
- `relic_reward_id`
- `required_progress`
- `required_tools`
- `shoring_materials`
- `site_id`
- `structural_risk`


# Appendix F.21 — Supporting Data Evidence: `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json`

### `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 4945 bytes / 4945 characters.
- SHA-256: `0910d261385a532cbebbc8483b3705b6b4edae19c60f20da7c67b63a10655fca`.
- Root keys: `mitigations`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
mitigations: min=8, max=8, observed_paths=1
mitigations[].hazard_tags: min=1, max=1, observed_paths=2
mitigations[].required_items: min=1, max=2, observed_paths=2
mitigations[].tags: min=3, max=3, observed_paths=2
```

Representative record fields:

- `display_name`
- `effect`
- `hazard_tags`
- `id`
- `labor_ticks`
- `required_items`
- `requires_respiratory_protection`
- `tags`

Representative identifiers (ordered, capped for readability):

```text
mitigation_ventilation_blower_install
mitigation_methane_flare_burnoff
mitigation_sump_drainage_pump
mitigation_chemical_spore_scrub
mitigation_timber_shoring_reinforcement
mitigation_emergency_bulkhead_seal
mitigation_sky_armor_blast_matting
mitigation_trapped_miner_clearance
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/World/ExcavationSitesCatalogTests.cs`

### `Ashfall.Core.Tests/World/ExcavationSitesCatalogTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 287; SHA-256: `27c7bd3e6cee1a9314331e730467200567fde2991faaec44428d34df55ae9057`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ExcavationCatalog_LoadsAllEightAuthoredSites
ExcavationCatalog_AllSiteIdsAreUnique
ExcavationCatalog_AllLocationReferencesResolveInLocationsCatalog
ExcavationCatalog_DepthBandsAreOrderedAndWithinRange
ExcavationCatalog_DepthBandsSpanShallowMediumDeepTiers
ExcavationCatalog_HazardCoverageMatchesDesignContract
ExcavationCatalog_RelicRewardLinksAreValid
ExcavationCatalog_DefaultSitesFallbackMatchesEightSites
ExcavationSystem_MultiSiteParallelExcavation_AndDeterministicProgress
ExcavationSystem_SaveAndRestore_MaintainsAllEightSitesState
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/ExcavationIntegrationTests.cs`

### `Ashfall.Core.Tests/ExcavationIntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 52; SHA-256: `9453aa7a85726d194258b32e437ff25c4d6bf5d9a7c13c50db20a9a86b9ddfe0`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AddSite_AssignWorkers_AndTick_AdvancesProgress
ApplyShoring_ReducesRisk
SaveAndRestore_PreservesSitesAndProgress
```


# Appendix G.24 — Supporting Regression Evidence: `Ashfall.Core.Tests/ExcavationSystemTests.cs`

### `Ashfall.Core.Tests/ExcavationSystemTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 68; SHA-256: `4b061ec9bf026c10cfbddbf1b405368f18811d016c0eeef3ba2afb2c66603a00`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.


# Appendix G.25 — Supporting Regression Evidence: `Ashfall.Core.Tests/ExcavationSystemTests.cs`

### `Ashfall.Core.Tests/ExcavationSystemTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 68; SHA-256: `4b061ec9bf026c10cfbddbf1b405368f18811d016c0eeef3ba2afb2c66603a00`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| site definitions and depth-band parsing | ExcavationCatalogLoader | mutable dig progress, workers, shoring and collapse state | ExcavationSystem | Owner emits/reads a typed fact; no mirror state. |
| site definitions and depth-band parsing | ExcavationCatalogLoader | hazard exposure and mitigation state | ExcavationHazardSystem | Owner emits/reads a typed fact; no mirror state. |
| site definitions and depth-band parsing | ExcavationCatalogLoader | artifact discovery/decryption route | ArchaeologySystem | Owner emits/reads a typed fact; no mirror state. |
| site definitions and depth-band parsing | ExcavationCatalogLoader | host adapter and save boundary | ExcavationHostSession | Owner emits/reads a typed fact; no mirror state. |
| site definitions and depth-band parsing | ExcavationCatalogLoader | current player surface | ArchaeologyExcavationPanel | Owner emits/reads a typed fact; no mirror state. |
| mutable dig progress, workers, shoring and collapse state | ExcavationSystem | site definitions and depth-band parsing | ExcavationCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| mutable dig progress, workers, shoring and collapse state | ExcavationSystem | hazard exposure and mitigation state | ExcavationHazardSystem | Owner emits/reads a typed fact; no mirror state. |
| mutable dig progress, workers, shoring and collapse state | ExcavationSystem | artifact discovery/decryption route | ArchaeologySystem | Owner emits/reads a typed fact; no mirror state. |
| mutable dig progress, workers, shoring and collapse state | ExcavationSystem | host adapter and save boundary | ExcavationHostSession | Owner emits/reads a typed fact; no mirror state. |
| mutable dig progress, workers, shoring and collapse state | ExcavationSystem | current player surface | ArchaeologyExcavationPanel | Owner emits/reads a typed fact; no mirror state. |
| hazard exposure and mitigation state | ExcavationHazardSystem | site definitions and depth-band parsing | ExcavationCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| hazard exposure and mitigation state | ExcavationHazardSystem | mutable dig progress, workers, shoring and collapse state | ExcavationSystem | Owner emits/reads a typed fact; no mirror state. |
| hazard exposure and mitigation state | ExcavationHazardSystem | artifact discovery/decryption route | ArchaeologySystem | Owner emits/reads a typed fact; no mirror state. |
| hazard exposure and mitigation state | ExcavationHazardSystem | host adapter and save boundary | ExcavationHostSession | Owner emits/reads a typed fact; no mirror state. |
| hazard exposure and mitigation state | ExcavationHazardSystem | current player surface | ArchaeologyExcavationPanel | Owner emits/reads a typed fact; no mirror state. |
| artifact discovery/decryption route | ArchaeologySystem | site definitions and depth-band parsing | ExcavationCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| artifact discovery/decryption route | ArchaeologySystem | mutable dig progress, workers, shoring and collapse state | ExcavationSystem | Owner emits/reads a typed fact; no mirror state. |
| artifact discovery/decryption route | ArchaeologySystem | hazard exposure and mitigation state | ExcavationHazardSystem | Owner emits/reads a typed fact; no mirror state. |
| artifact discovery/decryption route | ArchaeologySystem | host adapter and save boundary | ExcavationHostSession | Owner emits/reads a typed fact; no mirror state. |
| artifact discovery/decryption route | ArchaeologySystem | current player surface | ArchaeologyExcavationPanel | Owner emits/reads a typed fact; no mirror state. |
| host adapter and save boundary | ExcavationHostSession | site definitions and depth-band parsing | ExcavationCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| host adapter and save boundary | ExcavationHostSession | mutable dig progress, workers, shoring and collapse state | ExcavationSystem | Owner emits/reads a typed fact; no mirror state. |
| host adapter and save boundary | ExcavationHostSession | hazard exposure and mitigation state | ExcavationHazardSystem | Owner emits/reads a typed fact; no mirror state. |
| host adapter and save boundary | ExcavationHostSession | artifact discovery/decryption route | ArchaeologySystem | Owner emits/reads a typed fact; no mirror state. |
| host adapter and save boundary | ExcavationHostSession | current player surface | ArchaeologyExcavationPanel | Owner emits/reads a typed fact; no mirror state. |
| current player surface | ArchaeologyExcavationPanel | site definitions and depth-band parsing | ExcavationCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| current player surface | ArchaeologyExcavationPanel | mutable dig progress, workers, shoring and collapse state | ExcavationSystem | Owner emits/reads a typed fact; no mirror state. |
| current player surface | ArchaeologyExcavationPanel | hazard exposure and mitigation state | ExcavationHazardSystem | Owner emits/reads a typed fact; no mirror state. |
| current player surface | ArchaeologyExcavationPanel | artifact discovery/decryption route | ArchaeologySystem | Owner emits/reads a typed fact; no mirror state. |
| current player surface | ArchaeologyExcavationPanel | host adapter and save boundary | ExcavationHostSession | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | The required delta is a reachability and contract audit, not another site catalog. Any new site must use existing loader, hazard, inventory and panel paths. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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

> **DR-04 — The data catalog inventory has grown; several catalogs are absent from the v1.0 inventory. VERIFIED.**
`Assets/StreamingAssets/Data/` currently holds 342 entries. Catalogs observed live but not present in the v1.0 Part 5.4 inventory include: `dive_sites.json`, `hydroponic_crops.json`, `hydraulic_extrusion_catalog.json`, `metrology_standards_catalog.json`, `muster_camp_scenes.json`, `muster_epilogues.json`, `muster_faction_actions.json`, `muster_faction_culture.json`, `muster_witnesses.json`, `utility_actions.json`, `moral_choice_quests_branching.json`, `moral_choice_quests_distress.json`. Consequence: the duplication firewall (v1.0 Part 5) is stale in these domains; a planner could propose a "new" muster or moral-choice catalog that already exists. The ID-collision sweep in Factory Protocol step 1 must always run against the live listing, never against this document.

> | Cluster | Opening archetype | Confidence |
|---|---|---|
| C2 | Dose-treatment matrix paired tests against `MEDICAL_DOSE_TREATMENT_MATRIX.md` | HIGH CONFIDENCE |
| C11 | Debt-ledger consequence dispatcher coverage; rumor-band determinism pins | HIGH CONFIDENCE |
| C10 | Moral-choice flag consumer coverage for newly added consumers | HIGH CONFIDENCE |
| C13 | Epilogue permutation reachability tests for under-served permutations | HIGH CONFIDENCE |
| Cross | Determinism two-pass proofs for every new simulation; TEST-AGGREGATION metadata for catalog checks | CANON process |

> **SB-02 — Mid-winter slump pressure campaign (Lane A/C12).** Evidence: v1.0 Part 7 gap 1 (Days 90–180). Subject: a bounded story-pressure wave (blight, cave-in, levy arc) authored through existing catalogs. Integration route: data-first; each pressure rides its owning system (ecology for blight, subterranean/excavation for cave-ins, warlord doctrines for levies). Confidence: HIGH CONFIDENCE.

> **SB-12 — Dive-site and hydroponic domain expansion (Lanes A and B/C3, C5).** Evidence: DR-04 — `dive_sites.json`, `hydroponic_crops.json` live but absent from v1.0's inventory. Subject: premise-sweep these domains for unexploited seams (dive oxygen drain is a canon hourly system; hydroponics may lack narrative corpus and economy legs). Integration route: data-first + existing host sessions. Confidence: INFERENCE pending sweep.

> ### Subject
A bounded campaign-window content wave that inserts authored pressure into Days 90–180: a crop blight epidemic arc (ecology), a deep-strata cave-in arc (subterranean/excavation), and a warlord conscription levy arc (doctrines/tribute), each delivered through existing catalogs and event systems, so the stabilized mid-game stays legible as triage rather than routine.

> ### Recommended integration route
Tier: DATA-ONLY (three parallel authored tranches, one per owning system). Seams: ecology infestation catalog + crop strain catalog → existing infestation event dispatch; subterranean zones + excavation hazard mitigation catalogs → existing cave-in event path; warlord doctrines + tribute ledger → existing levy/tribute seams with `FactionStanceEngine` for reactions. Save impact class: NONE (events derive from catalogs and campaign state). Determinism: events must use existing seeded event streams — no new simulation. Verification: integrity + utilization selftests, one focused event-dispatch test per arc, balance harness re-run for the levy's economic pressure.

> ### Recommended integration route
Tier: audit (DOCS-ONLY) then DATA-ONLY corpus authoring, with HOST-WIRING only for confirmed unconsumed catalogs. Seams: corpus twins into `Assets/StreamingAssets/Data/narrative/` following the assay-report genre contracts; consumption wiring through the named host sessions (`HydraulicExtrusion` session confirmed live in the v1.0 host inventory). Verification: data-integrity selftest, content-utilization selftest (the decisive gate — presence is not reachability), focused loader tests.

> ### Recommended integration route
Tier: DATA-ONLY with HOST-WIRING verification. Seams: muster catalog family → existing muster loaders (verify integrity rules cover the newer catalogs — if the loader family predates them, extend it) → `MusterSystem` action selection → witness evidence into the Reckoning enrollment path → muster epilogue evaluation. Save impact class: NONE expected (catalog-driven), unless witness state persists — then EXISTING-SECTION with verification. Verification: integrity + utilization selftests, focused muster tests, one epilogue-reachability test for the new witness chains.

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

Current evidence and safe integration boundary for Plan 37: Excavation Sites, Archaeology and Deep-Strata Reachability.

- **site definitions and depth-band parsing** remains with `ExcavationCatalogLoader` at `Assets/Ashfall.Core/Excavation/ExcavationCatalogLoader.cs`. Static site catalog owner.
- **mutable dig progress, workers, shoring and collapse state** remains with `ExcavationSystem` at `Assets/Ashfall.Core/ExcavationSystem.cs`. Sole excavation state owner.
- **hazard exposure and mitigation state** remains with `ExcavationHazardSystem` at `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs`. Sole hazard state owner.
- **artifact discovery/decryption route** remains with `ArchaeologySystem` at `Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs`. Separate archaeology concern.
- **host adapter and save boundary** remains with `ExcavationHostSession` at `src/Host/ExcavationHostSession.cs`. Thin adapter.
- **current player surface** remains with `ArchaeologyExcavationPanel` at `src/UI/ArchaeologyExcavationPanel.cs`. Presentation and commands only.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load eight authored sites
2. validate depth/risk/hazard references
3. show current dig records
4. route worker/shoring actions through ExcavationSystem
5. apply hazard mitigation through hazard owner
6. record archaeology discovery through archaeology owner
7. capture existing excavation/hazard/archaeology state

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- The eight catalog rows are immutable definitions.
- ExcavationState owns active dig records.
- Hazard state is not copied into the site catalog or panel.
- Archaeology discovery consumes current excavation outcomes rather than replacing them.

- A site must be a current catalog ID before it can enter a dig record.
- Unknown hazard or mitigation IDs fail closed without state mutation.
- The panel cannot calculate collapse or reward outcomes.
- A discovery is emitted once under the archaeology owner contract.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/ExcavationHostSession.cs
- src/UI/ArchaeologyExcavationPanel.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/World/ExcavationSitesCatalogTests.cs
- Ashfall.Core.Tests/ExcavationIntegrationTests.cs
- Ashfall.Core.Tests/ExcavationSystemTests.cs
- Ashfall.Core.Tests/ExcavationSystemTests.cs

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
| S-01 | 37-01 eight site rows load | load eight authored sites | The eight catalog rows are immutable definitions. | A catalog site has no selectable route. | ExcavationCatalogLoader |
| S-02 | 37-02 site selection route | validate depth/risk/hazard references | ExcavationState owns active dig records. | Panel invents a site or hazard outcome. | ExcavationCatalogLoader |
| S-03 | 37-03 unknown site refusal | show current dig records | Hazard state is not copied into the site catalog or panel. | Shoring cost is charged twice. | ExcavationCatalogLoader |
| S-04 | 37-04 worker assignment | route worker/shoring actions through ExcavationSystem | Archaeology discovery consumes current excavation outcomes rather than replacing them. | Hazard mitigation mutates the wrong owner. | ExcavationCatalogLoader |
| S-05 | 37-05 shoring cost once | apply hazard mitigation through hazard owner | The eight catalog rows are immutable definitions. | A new save section duplicates dig state. | ExcavationCatalogLoader |
| S-06 | 37-06 hazard mitigation | record archaeology discovery through archaeology owner | ExcavationState owns active dig records. | A catalog site has no selectable route. | ExcavationCatalogLoader |
| S-07 | 37-07 collapse replay | capture existing excavation/hazard/archaeology state | Hazard state is not copied into the site catalog or panel. | Panel invents a site or hazard outcome. | ExcavationCatalogLoader |
| S-08 | 37-08 archaeology discovery | load eight authored sites | Archaeology discovery consumes current excavation outcomes rather than replacing them. | Shoring cost is charged twice. | ExcavationCatalogLoader |
| S-09 | 37-09 save continuation | validate depth/risk/hazard references | The eight catalog rows are immutable definitions. | Hazard mitigation mutates the wrong owner. | ExcavationCatalogLoader |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 37-TC-01 schema/count | data | schema/count; verify the named current owner and its negative boundary without inventing a second authority. | ExcavationCatalogLoader |
| T-02 | 37-TC-02 site ID/ranges | unit | site ID/ranges; verify the named current owner and its negative boundary without inventing a second authority. | ExcavationCatalogLoader |
| T-03 | 37-TC-03 loader parity | persistence | loader parity; verify the named current owner and its negative boundary without inventing a second authority. | ExcavationCatalogLoader |
| T-04 | 37-TC-04 host selection | determinism | host selection; verify the named current owner and its negative boundary without inventing a second authority. | ExcavationCatalogLoader |
| T-05 | 37-TC-05 worker/shoring guard | host | worker/shoring guard; verify the named current owner and its negative boundary without inventing a second authority. | ExcavationCatalogLoader |
| T-06 | 37-TC-06 hazard owner | UI/accessibility | hazard owner; verify the named current owner and its negative boundary without inventing a second authority. | ExcavationCatalogLoader |
| T-07 | 37-TC-07 seeded collapse | cross-system | seeded collapse; verify the named current owner and its negative boundary without inventing a second authority. | ExcavationCatalogLoader |
| T-08 | 37-TC-08 archaeology handoff | data | archaeology handoff; verify the named current owner and its negative boundary without inventing a second authority. | ExcavationCatalogLoader |
| T-09 | 37-TC-09 save round trip | unit | save round trip; verify the named current owner and its negative boundary without inventing a second authority. | ExcavationCatalogLoader |
| T-10 | 37-TC-10 UI state | persistence | UI state; verify the named current owner and its negative boundary without inventing a second authority. | ExcavationCatalogLoader |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 16 | `Ashfall.Core.Tests/World/ExcavationSitesCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 12 | `Ashfall.Core.Tests/Archaeology/ArchaeologySystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `src/Host/DiseaseOutbreakHostAdapter.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/World/ExcavationHazardSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `src/Audio/AudioSelfTest.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `src/Host/ExcavationHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/ExcavationIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Integration/Plans46_49_CrossSystemIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Shelter/CupolaFoundryEngineTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/World/Plan11ExplorationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/World/Plan36_37TrappingExcavationIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Audio/ShelterOperationsAudioBridge.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Combat/Plan10_11CombatExplorationIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/ExcavationSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Integration/FullCampaign30DayShelterPlaythroughTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/ExcavationSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Shelter/SeismicDynamicsSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Main.Plans186_189.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Main.Plans46_49.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Main.ShelterSocial.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/UI/ArchaeologyExcavationPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/UI/SubterraneanOperationsPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Integration/Plans186_189_CampaignContinuityTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Integration/PlansB66ToB69CrossSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/SaveSnapshotAlias26Tests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Shelter/SeismicMonitoringB68Tests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Shelter/ShelterSeismicDynamicsPlan56Tests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Host/HostCli.WorldExploration.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Host/PanelBindLifecycleSelfTest.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/UI/ExcavationPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/Balance/ShelterOperationsBalanceSim.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/excavation_sites.json`

### `Assets/StreamingAssets/Data/excavation_sites.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 10152; characters: 10152.
- SHA-256: `0318a4379b0fb9a640d090766f8079663f598e1c0de524633ec7add986fd42c8`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `sites`

#### `sites` — 8 current rows

- Row 001 `excavation_command_vault`: `{"depth_bands":[{"depth_meters":20.0,"label":"Cratered Rubble Approach","risk":0.25},{"depth_meters":55.0,"label":"Collapsed Service Conduit","risk":0.38},{"depth_meters":90.0,"label":"Reinforced Command Shell","risk":0.28},{"depth_meters"…`
- Row 002 `excavation_utility_tunnels`: `{"depth_bands":[{"depth_meters":15.0,"label":"Service Access Hatch","risk":0.15},{"depth_meters":38.0,"label":"Flooded Conduit Junction","risk":0.26},{"depth_meters":65.0,"label":"Main Infrastructure Sump","risk":0.35}],"description":"Subt…`
- Row 003 `excavation_metro_interchange`: `{"depth_bands":[{"depth_meters":25.0,"label":"Collapsed Ticket Concourse","risk":0.3},{"depth_meters":55.0,"label":"Sub-Platform Mezzanine","risk":0.42},{"depth_meters":80.0,"label":"Mold-Choked Track Tunnel","risk":0.5},{"depth_meters":10…`
- Row 004 `excavation_mine_shaft`: `{"depth_bands":[{"depth_meters":30.0,"label":"Weathered Mine Collar","risk":0.32},{"depth_meters":70.0,"label":"Fractured Gallery Level 1","risk":0.55},{"depth_meters":115.0,"label":"Timbered Drift Level 2","risk":0.65},{"depth_meters":150…`
- Row 005 `excavation_archive_bunker`: `{"depth_bands":[{"depth_meters":15.0,"label":"Debris Trench Access","risk":0.18},{"depth_meters":45.0,"label":"Microfilm Catalog Ante-Chamber","risk":0.32},{"depth_meters":85.0,"label":"Master Climate Vault","risk":0.46}],"description":"A …`
- Row 006 `excavation_drainage_network`: `{"depth_bands":[{"depth_meters":12.0,"label":"Culvert Silt Layer","risk":0.12},{"depth_meters":28.0,"label":"Cracked Service Channel","risk":0.22},{"depth_meters":45.0,"label":"Submerged Siphon Chamber","risk":0.32}],"description":"Stormwa…`
- Row 007 `excavation_storage_chamber`: `{"depth_bands":[{"depth_meters":20.0,"label":"Loading Bay Collapse","risk":0.25},{"depth_meters":48.0,"label":"Intermediate Staging Void","risk":0.35},{"depth_meters":75.0,"label":"Sealed Logistics Vault","risk":0.48}],"description":"An au…`
- Row 008 `excavation_civilian_shelter`: `{"depth_bands":[{"depth_meters":10.0,"label":"Basement Masonry Fill","risk":0.1},{"depth_meters":24.0,"label":"Stairwell Air-Lock Rubble","risk":0.2},{"depth_meters":40.0,"label":"Civilian Bunking Quarters","risk":0.28}],"description":"A p…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json`

### `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 4945; characters: 4945.
- SHA-256: `0910d261385a532cbebbc8483b3705b6b4edae19c60f20da7c67b63a10655fca`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `mitigations`

#### `mitigations` — 8 current rows

- Row 001 `mitigation_ventilation_blower_install`: `{"display_name":"Forced-Air Ventilation Blower","effect":{"methane_vent_rate_permille":300,"passive_decay_bonus_permille":150},"hazard_tags":["methane"],"id":"mitigation_ventilation_blower_install","labor_ticks":100,"required_items":[{"amo…`
- Row 002 `mitigation_methane_flare_burnoff`: `{"display_name":"Controlled Gas Flare Burn-Off","effect":{"methane_vent_rate_permille":600,"passive_decay_bonus_permille":0},"hazard_tags":["methane"],"id":"mitigation_methane_flare_burnoff","labor_ticks":40,"required_items":[{"amount":1,"…`
- Row 003 `mitigation_sump_drainage_pump`: `{"display_name":"Subterranean Sump Drainage Pump","effect":{"flood_drain_rate_permille":350,"passive_decay_bonus_permille":200},"hazard_tags":["flood"],"id":"mitigation_sump_drainage_pump","labor_ticks":160,"required_items":[{"amount":4,"i…`
- Row 004 `mitigation_chemical_spore_scrub`: `{"display_name":"Biocide Spore Decontamination","effect":{"passive_decay_bonus_permille":100,"spore_reduction_permille":500},"hazard_tags":["spores"],"id":"mitigation_chemical_spore_scrub","labor_ticks":90,"required_items":[{"amount":2,"it…`
- Row 005 `mitigation_timber_shoring_reinforcement`: `{"display_name":"Heavy Timber Strut Shoring","effect":{"collapse_risk_reduction_permille":300,"shoring_health_restore_permille":400},"hazard_tags":["shoring"],"id":"mitigation_timber_shoring_reinforcement","labor_ticks":120,"required_items…`
- Row 006 `mitigation_emergency_bulkhead_seal`: `{"display_name":"Emergency Steel Bulkhead Seal","effect":{"propagation_block_permille":1000,"sector_isolation_permille":1000},"hazard_tags":["methane","flood","spores","cave_in"],"id":"mitigation_emergency_bulkhead_seal","labor_ticks":30,"…`
- Row 007 `mitigation_sky_armor_blast_matting`: `{"display_name":"Blast Matting & Sandbag Curtain","effect":{"blast_damage_absorption_permille":600,"collapse_risk_reduction_permille":450},"hazard_tags":["cave_in","shoring"],"id":"mitigation_sky_armor_blast_matting","labor_ticks":110,"req…`
- Row 008 `mitigation_trapped_miner_clearance`: `{"display_name":"Emergency Debris Clearance & Rescue","effect":{"rubble_clearance_progress_permille":340},"hazard_tags":["cave_in"],"id":"mitigation_trapped_miner_clearance","labor_ticks":80,"required_items":[{"amount":2,"item_id":"scrap_m…`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Excavation/ExcavationCatalogLoader.cs`

### `Assets/Ashfall.Core/Excavation/ExcavationCatalogLoader.cs` — complete current file

- Size: 272 lines / 15853 bytes.
- SHA-256: `9ee0c361e472225c4c3f5b950f4bed2a264719375c68d1916598a83665f76409`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005:
00006: namespace Ashfall.Core.Excavation
00007: {
00008:     /// <summary>
00009:     /// Schema container for excavation_sites.json catalog.
00010:     /// </summary>
00011:     [Serializable]
00012:     public sealed class ExcavationCatalogContainer
00013:     {
00014:         public int schema_version { get; set; } = 1;
00015:         public List<ExcavationSiteDef> sites { get; set; } = new List<ExcavationSiteDef>();
00016:     }
00017:
00018:     /// <summary>
00019:     /// DTO defining an authored deep-strata excavation site.
00020:     /// </summary>
00021:     [Serializable]
00022:     public sealed class ExcavationSiteDef
00023:     {
00024:         public string site_id { get; set; } = string.Empty;
00025:         public string location_id { get; set; } = string.Empty;
00026:         public string display_name { get; set; } = string.Empty;
00027:         public string description { get; set; } = string.Empty;
00028:         public float max_depth_meters { get; set; } = 100f;
00029:         public float required_progress { get; set; } = 100f;
00030:         public float structural_risk { get; set; } = 0.3f;
00031:         public List<string> required_tools { get; set; } = new List<string>();
00032:         public List<string> shoring_materials { get; set; } = new List<string>();
00033:         public string hazard_type { get; set; } = string.Empty;
00034:         public string relic_reward_id { get; set; } = string.Empty;
00035:         public string loot_table { get; set; } = "salvage_common";
00036:         public List<ExcavationDepthBandDef> depth_bands { get; set; } = new List<ExcavationDepthBandDef>();
00037:         public string journal_entry_id { get; set; } = string.Empty;
00038:     }
00039:
00040:     /// <summary>
00041:     /// DTO defining an individual depth stratum / band.
00042:     /// </summary>
00043:     [Serializable]
00044:     public sealed class ExcavationDepthBandDef
00045:     {
00046:         public float depth_meters { get; set; }
00047:         public string label { get; set; } = string.Empty;
00048:         public float risk { get; set; }
00049:     }
00050:
00051:     /// <summary>
00052:     /// Loader and query authority for authored deep-strata excavation sites.
00053:     /// </summary>
00054:     public static class ExcavationCatalogLoader
00055:     {
00056:         public const string CatalogFileName = "excavation_sites.json";
00057:
00058:         public static List<ExcavationSiteDef> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
00059:         {
00060:             fileIO ??= new FileSystemIO();
00061:             serializer ??= new SystemTextJsonSerializer();
00062:
00063:             string fullPath = Path.Combine(dataDir, CatalogFileName);
00064:             if (!fileIO.FileExists(fullPath))
00065:             {
00066:                 return GetDefaultSites();
00067:             }
00068:
00069:             try
00070:             {
00071:                 string json = fileIO.ReadAllText(fullPath);
00072:                 var container = serializer.Deserialize<ExcavationCatalogContainer>(json);
00073:                 if (container != null && container.sites != null && container.sites.Count > 0)
00074:                 {
00075:                     return container.sites;
00076:                 }
00077:             }
00078:             catch
00079:             {
00080:                 // Fallback to built-in defaults if JSON parse fails
00081:             }
00082:
00083:             return GetDefaultSites();
00084:         }
00085:
00086:         public static List<ExcavationSiteDef> GetDefaultSites()
00087:         {
00088:             return new List<ExcavationSiteDef>
00089:             {
00090:                 new ExcavationSiteDef
00091:                 {
00092:                     site_id = "excavation_command_vault",
00093:                     location_id = "loc_excavation_command_vault",
00094:                     display_name = "Collapsed Civil Defense Command Vault",
00095:                     description = "A buried regional continuity command bunker constructed to survive strategic nuclear strikes. Blast damage collapsed the surface approach, leaving deeper operational rooms partially pressurized.",
00096:                     max_depth_meters = 120f,
00097:                     required_progress = 160f,
00098:                     structural_risk = 0.38f,
00099:                     required_tools = new List<string> { "tools_precision", "shovel" },
00100:                     shoring_materials = new List<string> { "scrap_metal", "mechanical_parts" },
00101:                     hazard_type = "hazard_radiation_hotspot",
00102:                     relic_reward_id = "item_comm_codebook_alpha",
00103:                     loot_table = "salvage_rare",
00104:                     journal_entry_id = "journal_excavation_command_vault",
00105:                     depth_bands = new List<ExcavationDepthBandDef>
00106:                     {
00107:                         new ExcavationDepthBandDef { depth_meters = 20f, label = "Cratered Rubble Approach", risk = 0.25f },
00108:                         new ExcavationDepthBandDef { depth_meters = 55f, label = "Collapsed Service Conduit", risk = 0.38f },
00109:                         new ExcavationDepthBandDef { depth_meters = 90f, label = "Reinforced Command Shell", risk = 0.28f },
00110:                         new ExcavationDepthBandDef { depth_meters = 120f, label = "Sealed Operations Center", risk = 0.52f }
00111:                     }
00112:                 },
00113:                 new ExcavationSiteDef
00114:                 {
00115:                     site_id = "excavation_utility_tunnels",
00116:                     location_id = "loc_excavation_utility_tunnels",
00117:                     display_name = "Utility Tunnel Network",
00118:                     description = "Subterranean municipal service corridors and conduit trunks running beneath the old district grid. Flooded sections and silted pipes conceal pre-war electrical salvage and emergency maintenance tools.",
00119:                     max_depth_meters = 65f,
00120:                     required_progress = 95f,
00121:                     structural_risk = 0.22f,
00122:                     required_tools = new List<string> { "tools_precision", "wrench" },
00123:                     shoring_materials = new List<string> { "scrap_wood", "metal_sheet" },
00124:                     hazard_type = "hazard_flood",
00125:                     relic_reward_id = "tools_precision",
00126:                     loot_table = "salvage_common",
00127:                     journal_entry_id = "journal_excavation_utility_tunnels",
00128:                     depth_bands = new List<ExcavationDepthBandDef>
00129:                     {
00130:                         new ExcavationDepthBandDef { depth_meters = 15f, label = "Service Access Hatch", risk = 0.15f },
00131:                         new ExcavationDepthBandDef { depth_meters = 38f, label = "Flooded Conduit Junction", risk = 0.26f },
00132:                         new ExcavationDepthBandDef { depth_meters = 65f, label = "Main Infrastructure Sump", risk = 0.35f }
00133:                     }
00134:                 },
00135:                 new ExcavationSiteDef
00136:                 {
00137:                     site_id = "excavation_metro_interchange",
00138:                     location_id = "loc_excavation_metro_interchange",
00139:                     display_name = "Buried Metro Interchange",
00140:                     description = "A multi-tier transit nexus crushed under collapsed concrete avenues. Damp platform recesses foster spore mold colonies among abandoned commuter baggage and maintenance gear.",
00141:                     max_depth_meters = 105f,
00142:                     required_progress = 145f,
00143:                     structural_risk = 0.42f,
00144:                     required_tools = new List<string> { "shovel", "crowbar" },
00145:                     shoring_materials = new List<string> { "steel_columns", "hydraulic_jack" },
00146:                     hazard_type = "hazard_spore_mold",
00147:                     relic_reward_id = "item_logistics_cipher_sheet",
00148:                     loot_table = "salvage_rare",
00149:                     journal_entry_id = "journal_excavation_metro_interchange",
00150:                     depth_bands = new List<ExcavationDepthBandDef>
00151:                     {
00152:                         new ExcavationDepthBandDef { depth_meters = 25f, label = "Collapsed Ticket Concourse", risk = 0.30f },
00153:                         new ExcavationDepthBandDef { depth_meters = 55f, label = "Sub-Platform Mezzanine", risk = 0.42f },
00154:                         new ExcavationDepthBandDef { depth_meters = 80f, label = "Mold-Choked Track Tunnel", risk = 0.50f },
00155:                         new ExcavationDepthBandDef { depth_meters = 105f, label = "Sealed Express Dispatch Platform", risk = 0.58f }
00156:                     }
00157:                 },
00158:                 new ExcavationSiteDef
00159:                 {
00160:                     site_id = "excavation_mine_shaft",
00161:                     location_id = "loc_excavation_mine_shaft",
00162:                     display_name = "Industrial Mine Shaft Adit 4",
00163:                     description = "A deep extraction adit bored into fractured granitic bedrock. Rotting timber supports and pockets of trapped methane create extreme cave-in hazards protecting rich industrial machinery.",
00164:                     max_depth_meters = 150f,
00165:                     required_progress = 190f,
00166:                     structural_risk = 0.55f,
00167:                     required_tools = new List<string> { "pickaxe", "hydraulic_jack" },
00168:                     shoring_materials = new List<string> { "timber_beams", "hydraulic_jack" },
00169:                     hazard_type = "hazard_methane_pocket",
00170:                     relic_reward_id = "heavy_industrial_motor",
00171:                     loot_table = "salvage_rare",
00172:                     journal_entry_id = "journal_excavation_mine_shaft",
00173:                     depth_bands = new List<ExcavationDepthBandDef>
00174:                     {
00175:                         new ExcavationDepthBandDef { depth_meters = 30f, label = "Weathered Mine Collar", risk = 0.32f },
00176:                         new ExcavationDepthBandDef { depth_meters = 70f, label = "Fractured Gallery Level 1", risk = 0.55f },
00177:                         new ExcavationDepthBandDef { depth_meters = 115f, label = "Timbered Drift Level 2", risk = 0.65f },
00178:                         new ExcavationDepthBandDef { depth_meters = 150f, label = "Flooded Ore Sump & Machine Pocket", risk = 0.75f }
00179:                     }
00180:                 },
00181:                 new ExcavationSiteDef
00182:                 {
00183:                     site_id = "excavation_archive_bunker",
00184:                     location_id = "loc_excavation_archive_bunker",
00185:                     display_name = "Sealed Archive Bunker",
00186:                     description = "A climate-isolated subterranean depository housing pre-war technological patents and civil registries. Damp air locks hold aggressive spore mold around hardened vault vaults.",
00187:                     max_depth_meters = 85f,
00188:                     required_progress = 135f,
00189:                     structural_risk = 0.32f,
00190:                     required_tools = new List<string> { "tools_precision", "crowbar" },
00191:                     shoring_materials = new List<string> { "reinforced_arches", "scrap_metal" },
00192:                     hazard_type = "hazard_spore_mold",
00193:                     relic_reward_id = "item_archive_index_cylinder",
00194:                     loot_table = "salvage_rare",
00195:                     journal_entry_id = "journal_excavation_archive_bunker",
00196:                     depth_bands = new List<ExcavationDepthBandDef>
00197:                     {
00198:                         new ExcavationDepthBandDef { depth_meters = 15f, label = "Debris Trench Access", risk = 0.18f },
00199:                         new ExcavationDepthBandDef { depth_meters = 45f, label = "Microfilm Catalog Ante-Chamber", risk = 0.32f },
00200:                         new ExcavationDepthBandDef { depth_meters = 85f, label = "Master Climate Vault", risk = 0.46f }
00201:                     }
00202:                 },
00203:                 new ExcavationSiteDef
00204:                 {
00205:                     site_id = "excavation_drainage_network",
00206:                     location_id = "loc_excavation_drainage_network",
00207:                     display_name = "Drainage Network Sluice 09",
00208:                     description = "Stormwater culverts and overflow sluices converted into illicit smuggling routes before the bombardment. Silt and contaminated backwash hide sealed waterproof caches.",
00209:                     max_depth_meters = 45f,
00210:                     required_progress = 75f,
00211:                     structural_risk = 0.20f,
00212:                     required_tools = new List<string> { "shovel", "wrench" },
00213:                     shoring_materials = new List<string> { "scrap_wood", "scrap_metal" },
00214:                     hazard_type = "hazard_flood",
00215:                     relic_reward_id = "water_filter",
00216:                     loot_table = "salvage_common",
00217:                     journal_entry_id = "journal_excavation_drainage_network",
00218:                     depth_bands = new List<ExcavationDepthBandDef>
00219:                     {
00220:                         new ExcavationDepthBandDef { depth_meters = 12f, label = "Culvert Silt Layer", risk = 0.12f },
00221:                         new ExcavationDepthBandDef { depth_meters = 28f, label = "Cracked Service Channel", risk = 0.22f },
00222:                         new ExcavationDepthBandDef { depth_meters = 45f, label = "Submerged Siphon Chamber", risk = 0.32f }
00223:                     }
00224:                 },
00225:                 new ExcavationSiteDef
00226:                 {
00227:                     site_id = "excavation_storage_chamber",
00228:                     location_id = "loc_excavation_storage_chamber",
00229:                     display_name = "Forgotten Storage Chamber 14",
00230:                     description = "An auxiliary military logistics cache sealed in haste during civil evacuation. Unstable masonry slabs overhang intact pallets of rations and industrial spares.",
00231:                     max_depth_meters = 75f,
00232:                     required_progress = 110f,
00233:                     structural_risk = 0.34f,
00234:                     required_tools = new List<string> { "crowbar", "shovel" },
00235:                     shoring_materials = new List<string> { "scrap_metal", "mechanical_parts" },
00236:                     hazard_type = "hazard_toxic_air",
00237:                     relic_reward_id = "spring_mechanism",
00238:                     loot_table = "salvage_rare",
00239:                     journal_entry_id = "journal_excavation_storage_chamber",
00240:                     depth_bands = new List<ExcavationDepthBandDef>
00241:                     {
00242:                         new ExcavationDepthBandDef { depth_meters = 20f, label = "Loading Bay Collapse", risk = 0.25f },
00243:                         new ExcavationDepthBandDef { depth_meters = 48f, label = "Intermediate Staging Void", risk = 0.35f },
00244:                         new ExcavationDepthBandDef { depth_meters = 75f, label = "Sealed Logistics Vault", risk = 0.48f }
00245:                     }
00246:                 },
00247:                 new ExcavationSiteDef
00248:                 {
00249:                     site_id = "excavation_civilian_shelter",
00250:                     location_id = "loc_excavation_civilian_shelter",
00251:                     display_name = "Pre-War Civilian Shelter B-12",
00252:                     description = "A privately funded neighborhood shelter built beneath a residential complex. Shorter excavation depths yield domestic survival gear, medical supplies, and handwritten diaries.",
00253:                     max_depth_meters = 40f,
00254:                     required_progress = 65f,
00255:                     structural_risk = 0.18f,
00256:                     required_tools = new List<string> { "shovel", "crowbar" },
00257:                     shoring_materials = new List<string> { "scrap_wood", "scrap_metal" },
00258:                     hazard_type = "hazard_toxic_air",
00259:                     relic_reward_id = "music_box_comb",
00260:                     loot_table = "salvage_common",
00261:                     journal_entry_id = "journal_excavation_civilian_shelter",
00262:                     depth_bands = new List<ExcavationDepthBandDef>
00263:                     {
00264:                         new ExcavationDepthBandDef { depth_meters = 10f, label = "Basement Masonry Fill", risk = 0.10f },
00265:                         new ExcavationDepthBandDef { depth_meters = 24f, label = "Stairwell Air-Lock Rubble", risk = 0.20f },
00266:                         new ExcavationDepthBandDef { depth_meters = 40f, label = "Civilian Bunking Quarters", risk = 0.28f }
00267:                     }
00268:                 }
00269:             };
00270:         }
00271:     }
00272: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/ExcavationSystem.cs`

### `Assets/Ashfall.Core/ExcavationSystem.cs` — complete current file

- Size: 164 lines / 7185 bytes.
- SHA-256: `ee423dfdbe7885f439587d5dff23697bad53ef3f7df05bf6a756d2032f5b029f`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.Inventory;
00005: #pragma warning disable CS8618
00006:
00007: namespace Ashfall.Core
00008: {
00009:     [Serializable]
00010:     public sealed class ExcavationState
00011:     {
00012:         public string systemId = ExcavationSystem.SystemId;
00013:         public List<ExcavationSite> sites = new List<ExcavationSite>();
00014:     }
00015:
00016:     [Serializable]
00017:     public sealed class ExcavationSite
00018:     {
00019:         public string siteId = string.Empty;
00020:         public string roomBlueprintId = string.Empty;
00021:         public float progress;
00022:         public float requiredProgress = 100f;
00023:         public int assignedWorkerCount;
00024:         public float structuralRisk; // 0-1, risk of cave-in
00025:         public bool hasCavedIn;
00026:         public bool isComplete;
00027:         public List<string> requiredTools = new List<string>();
00028:         public bool shoringApplied;
00029:         public int reinforcedBeams; // Plans 90-93: cast beams set through the foundry loop
00030:         public List<string> discoveredCaches = new List<string>();
00031:     }
00032:
00033:     public sealed class ExcavationSystem
00034:     {
00035:         public const string SystemId = "excavation";
00036:         // Plans 90-93: the canonical structural beam is the foundry's cast
00037:         // T-beam; reinforcement is an item-flow consumer of the foundry loop.
00038:         public const string StructuralBeamItemId = "item_foundry_t_beam";
00039:         public const int StructuralBeamCost = 2;
00040:         private ExcavationState _state = new ExcavationState();
00041:         private readonly ISeededRng _rng;
00042:         private readonly ILog _log;
00043:         private readonly Inventory.Inventory? _inventory;
00044:
00045:         public ExcavationState State => _state;
00046:         public event Action OnExcavationChanged;
00047:
00048:         public ExcavationSystem(ISeededRng rng, ILog? log = null, Inventory.Inventory? inventory = null)
00049:         {
00050:             _rng = rng ?? throw new ArgumentNullException(nameof(rng));
00051:             _log = log ?? NullLog.Instance;
00052:             _inventory = inventory;
00053:         }
00054:
00055:         public ActionResult AddSite(string siteId, string roomBlueprintId, float requiredProgress, float risk)
00056:         {
00057:             if (_state.sites.Exists(s => s.siteId == siteId))
00058:                 return ActionResult.Blocked("site_exists", "excavation.site_exists");
00059:             _state.sites.Add(new ExcavationSite
00060:             {
00061:                 siteId = siteId, roomBlueprintId = roomBlueprintId,
00062:                 requiredProgress = requiredProgress, structuralRisk = risk
00063:             });
00064:             OnExcavationChanged?.Invoke();
00065:             return ActionResult.Success("excavation.site_added");
00066:         }
00067:
00068:         public ActionResult AssignWorkers(string siteId, int count)
00069:         {
00070:             var site = _state.sites.Find(s => s.siteId == siteId);
00071:             if (site == null) return ActionResult.Failed("unknown_site", "excavation.unknown_site");
00072:             if (site.isComplete) return ActionResult.Blocked("already_complete", "excavation.already_complete");
00073:             if (site.hasCavedIn) return ActionResult.Blocked("caved_in", "excavation.caved_in");
00074:             site.assignedWorkerCount = Math.Max(0, count);
00075:             OnExcavationChanged?.Invoke();
00076:             return ActionResult.Success("excavation.workers_assigned",
00077:                 new Dictionary<string, double> { { "workers", count } });
00078:         }
00079:
00080:         public ActionResult ApplyShoring(string siteId)
00081:         {
00082:             var site = _state.sites.Find(s => s.siteId == siteId);
00083:             if (site == null) return ActionResult.Failed("unknown_site", "excavation.unknown_site");
00084:             if (site.shoringApplied) return ActionResult.Blocked("already_shored", "excavation.already_shored");
00085:             site.shoringApplied = true;
00086:             site.structuralRisk *= 0.5f;
00087:             OnExcavationChanged?.Invoke();
00088:             return ActionResult.Success("excavation.shoring_applied",
00089:                 new Dictionary<string, double> { { "risk", site.structuralRisk } });
00090:         }
00091:
00092:         /// <summary>
00093:         /// Plans 90-93 — set cast structural beams (foundry output) into the
00094:         /// working face. Atomic inventory billing: a failed reinforcement
00095:         /// consumes nothing and mutates nothing. Each set halves structural
00096:         /// risk again, with diminishing returns at low risk.
00097:         /// </summary>
00098:         public ActionResult TryApplyStructuralReinforcement(string siteId)
00099:         {
00100:             var site = _state.sites.Find(s => s.siteId == siteId);
00101:             if (site == null) return ActionResult.Failed("unknown_site", "excavation.unknown_site");
00102:             if (site.isComplete) return ActionResult.Blocked("already_complete", "excavation.already_complete");
00103:             if (site.hasCavedIn) return ActionResult.Blocked("caved_in", "excavation.caved_in");
00104:             if (site.structuralRisk <= 0.05f) return ActionResult.Blocked("risk_already_low", "excavation.risk_already_low");
00105:             if (_inventory == null) return ActionResult.Failed("no_inventory", "excavation.no_inventory");
00106:
00107:             var bill = new InventoryBill();
00108:             bill.AddCost(StructuralBeamItemId, StructuralBeamCost);
00109:
00110:             bool committed = _inventory.TryExecuteTransaction(bill, () =>
00111:             {
00112:                 site.reinforcedBeams++;
00113:                 site.structuralRisk = Math.Max(0.05f, site.structuralRisk * 0.5f);
00114:                 OnExcavationChanged?.Invoke();
00115:             });
00116:             if (!committed) return ActionResult.Failed("missing_beams", "excavation.missing_beams");
00117:
00118:             return ActionResult.Success("excavation.reinforced",
00119:                 new Dictionary<string, double> { { "risk", site.structuralRisk } });
00120:         }
00121:
00122:         public void TickDay()
00123:         {
00124:             foreach (var site in _state.sites)
00125:             {
00126:                 if (site.isComplete || site.hasCavedIn || site.assignedWorkerCount <= 0) continue;
00127:
00128:                 float dailyProgress = site.assignedWorkerCount * 5f;
00129:                 if (site.shoringApplied) dailyProgress *= 1.2f;
00130:                 site.progress += dailyProgress;
00131:
00132:                 if (_rng.NextDouble() < site.structuralRisk * 0.1f)
00133:                 {
00134:                     site.hasCavedIn = true;
00135:                     site.progress = Math.Max(0, site.progress - 20f);
00136:                     _log.Warn($"[Excavation] cave-in at {site.siteId}!");
00137:                 }
00138:
00139:                 if (site.progress >= site.requiredProgress)
00140:                 {
00141:                     site.isComplete = true;
00142:                     _log.Info($"[Excavation] completed {site.siteId} -> {site.roomBlueprintId}");
00143:                 }
00144:             }
00145:             OnExcavationChanged?.Invoke();
00146:         }
00147:
00148:         public ExcavationState CaptureState() => CloneState(_state);
00149:
00150:         public void RestoreState(ExcavationState saved)
00151:         {
00152:             if (saved == null) return;
00153:             _state = CloneState(saved);
00154:         }
00155:
00156:         private static ExcavationState CloneState(ExcavationState src)
00157:         {
00158:             if (src == null) return new ExcavationState();
00159:             var s = new SystemTextJsonSerializer();
00160:             var json = s.Serialize(src);
00161:             return s.Deserialize<ExcavationState>(json) ?? new ExcavationState();
00162:         }
00163:     }
00164: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs`

### `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs` — complete current file

- Size: 482 lines / 19643 bytes.
- SHA-256: `2251ec3acbcde282740467faea55ab88e306def6f56f2df73be2d5484765a42f`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Text.Json.Serialization;
00005: using Ashfall.Core.Inventory;
00006: using Ashfall.Core.IO;
00007: using Ashfall.Core.Shelter;
00008: using InventoryContainer = Ashfall.Core.Inventory.Inventory;
00009:
00010: namespace Ashfall.Core.Excavation
00011: {
00012:     [Serializable]
00013:     public sealed class MitigationItemCost
00014:     {
00015:         [JsonPropertyName("item_id")]
00016:         public string ItemId { get; set; } = string.Empty;
00017:
00018:         [JsonPropertyName("amount")]
00019:         public int Amount { get; set; } = 1;
00020:     }
00021:
00022:     [Serializable]
00023:     public sealed class ExcavationMitigationDefinition
00024:     {
00025:         [JsonPropertyName("id")]
00026:         public string Id { get; set; } = string.Empty;
00027:
00028:         [JsonPropertyName("display_name")]
00029:         public string DisplayName { get; set; } = string.Empty;
00030:
00031:         [JsonPropertyName("hazard_tags")]
00032:         public List<string> HazardTags { get; set; } = new List<string>();
00033:
00034:         [JsonPropertyName("required_items")]
00035:         public List<MitigationItemCost> RequiredItems { get; set; } = new List<MitigationItemCost>();
00036:
00037:         [JsonPropertyName("labor_ticks")]
00038:         public int LaborTicks { get; set; } = 60;
00039:
00040:         [JsonPropertyName("effect")]
00041:         public Dictionary<string, int> Effect { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
00042:
00043:         [JsonPropertyName("requires_respiratory_protection")]
00044:         public bool RequiresRespiratoryProtection { get; set; }
00045:
00046:         [JsonPropertyName("tags")]
00047:         public List<string> Tags { get; set; } = new List<string>();
00048:     }
00049:
00050:     [Serializable]
00051:     public sealed class ExcavationHazardCatalogData
00052:     {
00053:         [JsonPropertyName("schema_version")]
00054:         public int SchemaVersion { get; set; } = 1;
00055:
00056:         [JsonPropertyName("mitigations")]
00057:         public List<ExcavationMitigationDefinition> Mitigations { get; set; } = new List<ExcavationMitigationDefinition>();
00058:     }
00059:
00060:     [Serializable]
00061:     public sealed class ExcavationSectorHazardState
00062:     {
00063:         public string SectorId { get; set; } = string.Empty;
00064:         public int MethanePpm { get; set; } = 500; // 0 - 10000+
00065:         public int FloodLevelPermille { get; set; } = 0; // 0 - 1000
00066:         public int SporeConcentrationPermille { get; set; } = 0; // 0 - 1000
00067:         public int ShoringHealthPermille { get; set; } = 1000; // 0 - 1000
00068:         public bool IsBulkheadSealed { get; set; }
00069:         public List<string> InstalledMitigationIds { get; set; } = new List<string>();
00070:         public List<string> ActiveTrappedMiners { get; set; } = new List<string>();
00071:         public int? RescueDeadlineDay { get; set; }
00072:         public int RescueLaborRemaining { get; set; }
00073:         public bool RescueCompleted { get; set; }
00074:         public bool RescueFailed { get; set; }
00075:     }
00076:
00077:     [Serializable]
00078:     public sealed class ExcavationHazardSave
00079:     {
00080:         public string systemId = ExcavationHazardSystem.SystemId;
00081:         public int schemaVersion = 1;
00082:         public Dictionary<string, ExcavationSectorHazardState> sectors = new(StringComparer.Ordinal);
00083:         public int currentDay;
00084:     }
00085:
00086:     public sealed class ExcavationHazardSystem
00087:     {
00088:         public const string SystemId = "excavation_hazards";
00089:
00090:         private ExcavationHazardSave _state = new ExcavationHazardSave();
00091:         private readonly Dictionary<string, ExcavationMitigationDefinition> _catalog = new(StringComparer.Ordinal);
00092:         private readonly InventoryContainer _inventory;
00093:         private readonly ExcavationSystem? _excavation;
00094:         private readonly SkyLayerArmorSystem? _skyArmor;
00095:         private readonly ISeededRng _rng;
00096:         private readonly ILog _log;
00097:
00098:         public ExcavationHazardSave State => _state;
00099:         public IReadOnlyDictionary<string, ExcavationMitigationDefinition> Catalog => _catalog;
00100:
00101:         /// <summary>Methane concentration (PPM) above which ignition risk begins; shared by the risk evaluator and the exactly-once ignition event.</summary>
00102:         public const int MethaneIgnitionThresholdPpm = 4000;
00103:
00104:         /// <summary>Flood level (permille) above which a sector counts as flooded; shared by the risk evaluator and the exactly-once flood event.</summary>
00105:         public const int FloodCriticalThresholdPermille = 500;
00106:
00107:         public event Action<string, string>? OnMitigationInstalled; // sectorId, mitigationId
00108:         public event Action<string>? OnMethaneIgnition; // sectorId
00109:         public event Action<string>? OnSectorFlooded;
00110:         public event Action<string, int>? OnRescueStarted; // sectorId, trappedCount
00111:         public event Action<string>? OnRescueSucceeded;
00112:         public event Action<string>? OnRescueFailed;
00113:         public event Action? OnHazardStateChanged;
00114:
00115:         public ExcavationHazardSystem(
00116:             InventoryContainer inventory,
00117:             ISeededRng rng,
00118:             ExcavationSystem? excavation = null,
00119:             SkyLayerArmorSystem? skyArmor = null,
00120:             ILog? log = null)
00121:         {
00122:             _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
00123:             _rng = rng ?? throw new ArgumentNullException(nameof(rng));
00124:             _excavation = excavation;
00125:             _skyArmor = skyArmor;
00126:             _log = log ?? NullLog.Instance;
00127:         }
00128:
00129:         public void LoadCatalog(ExcavationHazardCatalogData? data)
00130:         {
00131:             if (data?.Mitigations == null) return;
00132:             _catalog.Clear();
00133:             foreach (var m in data.Mitigations)
00134:             {
00135:                 if (!string.IsNullOrEmpty(m.Id))
00136:                     _catalog[m.Id] = m;
00137:             }
00138:         }
00139:
00140:         public void LoadCatalog(string json)
00141:         {
00142:             if (string.IsNullOrWhiteSpace(json)) return;
00143:             var serializer = new SystemTextJsonSerializer();
00144:             var data = serializer.Deserialize<ExcavationHazardCatalogData>(json);
00145:             LoadCatalog(data);
00146:         }
00147:
00148:         public ExcavationSectorHazardState GetOrCreateSector(string sectorId)
00149:         {
00150:             if (string.IsNullOrEmpty(sectorId)) sectorId = "sector_excavation_alpha";
00151:             if (_state.sectors.TryGetValue(sectorId, out var sector))
00152:                 return sector;
00153:
00154:             var created = new ExcavationSectorHazardState
00155:             {
00156:                 SectorId = sectorId,
00157:                 MethanePpm = 300,
00158:                 FloodLevelPermille = 0,
00159:                 SporeConcentrationPermille = 0,
00160:                 ShoringHealthPermille = 1000
00161:             };
00162:             _state.sectors[sectorId] = created;
00163:             return created;
00164:         }
00165:
00166:         public bool CanApplyMitigation(string sectorId, string mitigationId, out string reason)
00167:         {
00168:             reason = string.Empty;
00169:             if (!_catalog.TryGetValue(mitigationId, out var def))
00170:             {
00171:                 reason = "unknown_mitigation";
00172:                 return false;
00173:             }
00174:
00175:             var sector = GetOrCreateSector(sectorId);
00176:             if (sector.IsBulkheadSealed && def.Id != "mitigation_emergency_bulkhead_seal")
00177:             {
00178:                 reason = "sector_bulkhead_sealed";
00179:                 return false;
00180:             }
00181:
00182:             // Respiratory requirement check
00183:             if (def.RequiresRespiratoryProtection && _inventory.CountById("gas_mask") < 1)
00184:             {
00185:                 reason = "missing_gas_mask";
00186:                 return false;
00187:             }
00188:
00189:             // Item validation
00190:             var bill = BuildBill(def);
00191:             var validation = _inventory.ValidateTransaction(bill);
00192:             if (!validation.IsValid)
00193:             {
00194:                 reason = validation.FailureReason;
00195:                 return false;
00196:             }
00197:
00198:             return true;
00199:         }
00200:
00201:         private InventoryBill BuildBill(ExcavationMitigationDefinition def)
00202:         {
00203:             var bill = new InventoryBill();
00204:             foreach (var item in def.RequiredItems)
00205:             {
00206:                 if (!string.IsNullOrEmpty(item.ItemId) && item.Amount > 0)
00207:                     bill.AddCost(item.ItemId, item.Amount);
00208:             }
00209:             return bill;
00210:         }
00211:
00212:         public ActionResult TryApplyMitigation(string sectorId, string mitigationId, IReadOnlyList<string>? workerIds = null)
00213:         {
00214:             if (!CanApplyMitigation(sectorId, mitigationId, out var reason))
00215:                 return ActionResult.Blocked("cannot_apply", reason);
00216:
00217:             var def = _catalog[mitigationId];
00218:             var bill = BuildBill(def);
00219:
00220:             if (!_inventory.TryExecuteTransaction(bill))
00221:                 return ActionResult.Blocked("transaction_failed", "insufficient_materials");
00222:
00223:             var sector = GetOrCreateSector(sectorId);
00224:
00225:             // Apply mitigation effects (mutation flows through the canonical
00226:             // AddMethane/AddFloodWater seams so hazard notifications cannot drift).
00227:             if (def.Effect.TryGetValue("methane_vent_rate_permille", out int methaneVent))
00228:             {
00229:                 int reduction = (int)(sector.MethanePpm * (methaneVent / 1000f));
00230:                 ApplyMethaneDelta(sector, -reduction, int.MaxValue);
00231:             }
00232:
00233:             if (def.Effect.TryGetValue("flood_drain_rate_permille", out int floodDrain))
00234:             {
00235:                 ApplyFloodDelta(sector, -floodDrain);
00236:             }
00237:
00238:             if (def.Effect.TryGetValue("spore_reduction_permille", out int sporeRed))
00239:             {
00240:                 sector.SporeConcentrationPermille = Math.Max(0, sector.SporeConcentrationPermille - sporeRed);
00241:             }
00242:
00243:             if (def.Effect.TryGetValue("shoring_health_restore_permille", out int shoringGain))
00244:             {
00245:                 sector.ShoringHealthPermille = Math.Min(1000, sector.ShoringHealthPermille + shoringGain);
00246:             }
00247:
00248:             if (def.Tags.Contains("installed") && !sector.InstalledMitigationIds.Contains(def.Id))
00249:             {
00250:                 sector.InstalledMitigationIds.Add(def.Id);
00251:             }
00252:
00253:             if (def.Id == "mitigation_trapped_miner_clearance" && sector.ActiveTrappedMiners.Count > 0)
00254:             {
00255:                 int clearance = def.Effect.GetValueOrDefault("rubble_clearance_progress_permille", 340);
00256:                 ProgressRescueLabor(sectorId, clearance);
00257:             }
00258:
00259:             OnMitigationInstalled?.Invoke(sectorId, mitigationId);
00260:             OnHazardStateChanged?.Invoke();
00261:             return ActionResult.Success("excavation.mitigation_applied");
00262:         }
00263:
00264:         public ActionResult TryToggleBulkhead(string sectorId, bool seal, out string reason)
00265:         {
00266:             reason = string.Empty;
00267:             var sector = GetOrCreateSector(sectorId);
00268:             if (seal && sector.ActiveTrappedMiners.Count > 0)
00269:             {
00270:                 reason = "cannot_seal_trapped_miners";
00271:                 return ActionResult.Blocked("trapped_miners", reason);
00272:             }
00273:
00274:             sector.IsBulkheadSealed = seal;
00275:             OnHazardStateChanged?.Invoke();
00276:             return ActionResult.Success(seal ? "bulkhead.sealed" : "bulkhead.opened");
00277:         }
00278:
00279:         public void TriggerCaveInRescue(
00280:             string sectorId,
00281:             IReadOnlyList<string> trappedSurvivorIds,
00282:             int deadlineDays = 3,
00283:             int requiredLabor = 240)
00284:         {
00285:             var sector = GetOrCreateSector(sectorId);
00286:             sector.ActiveTrappedMiners = new List<string>(trappedSurvivorIds);
00287:             sector.RescueDeadlineDay = _state.currentDay + deadlineDays;
00288:             sector.RescueLaborRemaining = requiredLabor;
00289:             sector.RescueCompleted = false;
00290:             sector.RescueFailed = false;
00291:
00292:             OnRescueStarted?.Invoke(sectorId, trappedSurvivorIds.Count);
00293:             OnHazardStateChanged?.Invoke();
00294:         }
00295:
00296:         public void ProgressRescueLabor(string sectorId, int laborAmount)
00297:         {
00298:             var sector = GetOrCreateSector(sectorId);
00299:             if (sector.ActiveTrappedMiners.Count == 0 || sector.RescueCompleted || sector.RescueFailed)
00300:                 return;
00301:
00302:             sector.RescueLaborRemaining = Math.Max(0, sector.RescueLaborRemaining - laborAmount);
00303:             if (sector.RescueLaborRemaining <= 0)
00304:             {
00305:                 sector.RescueCompleted = true;
00306:                 sector.ActiveTrappedMiners.Clear();
00307:                 sector.RescueDeadlineDay = null;
00308:                 OnRescueSucceeded?.Invoke(sectorId);
00309:             }
00310:             OnHazardStateChanged?.Invoke();
00311:         }
00312:
00313:         public (float collapseRisk, float ignitionRisk, bool respiratoryHazard) EvaluateOperationRisk(string sectorId)
00314:         {
00315:             var sector = GetOrCreateSector(sectorId);
00316:             float baseCollapse = (1000 - sector.ShoringHealthPermille) / 1000f * 0.40f;
00317:
00318:             if (sector.FloodLevelPermille > FloodCriticalThresholdPermille) baseCollapse += 0.20f;
00319:             if (sector.InstalledMitigationIds.Contains("mitigation_sky_armor_blast_matting"))
00320:                 baseCollapse *= 0.55f;
00321:
00322:             float ignitionRisk = 0f;
00323:             if (sector.MethanePpm > MethaneIgnitionThresholdPpm)
00324:             {
00325:                 ignitionRisk = Math.Clamp((sector.MethanePpm - MethaneIgnitionThresholdPpm) / 6000f, 0f, 0.90f);
00326:             }
00327:
00328:             bool respHazard = sector.SporeConcentrationPermille > 200 || sector.MethanePpm > 5000;
00329:             return (Math.Clamp(baseCollapse, 0f, 1f), ignitionRisk, respHazard);
00330:         }
00331:
00332:         /// <summary>
00333:         /// Canonical methane mutation. Every producer (daily accumulation,
00334:         /// ventilation, seismic outgassing) routes through here so crossing
00335:         /// <see cref="MethaneIgnitionThresholdPpm"/> raises
00336:         /// <see cref="OnMethaneIgnition"/> exactly once per crossing.
00337:         /// </summary>
00338:         public void AddMethane(string sectorId, int deltaPpm, int cap = int.MaxValue)
00339:         {
00340:             var sector = GetOrCreateSector(sectorId);
00341:             if (ApplyMethaneDelta(sector, deltaPpm, cap))
00342:                 OnHazardStateChanged?.Invoke();
00343:         }
00344:
00345:         /// <summary>
00346:         /// Canonical flood-level mutation. Crossing
00347:         /// <see cref="FloodCriticalThresholdPermille"/> raises
00348:         /// <see cref="OnSectorFlooded"/> exactly once per crossing. Returns true
00349:         /// when the level actually changed.
00350:         /// </summary>
00351:         public bool AddFloodWater(string sectorId, int deltaPermille)
00352:         {
00353:             var sector = GetOrCreateSector(sectorId);
00354:             bool changed = ApplyFloodDelta(sector, deltaPermille);
00355:             if (changed)
00356:                 OnHazardStateChanged?.Invoke();
00357:             return changed;
00358:         }
00359:
00360:         private bool ApplyMethaneDelta(ExcavationSectorHazardState sector, int deltaPpm, int cap)
00361:         {
00362:             if (sector == null || deltaPpm == 0) return false;
00363:
00364:             long next = (long)sector.MethanePpm + deltaPpm;
00365:             if (next < 0) next = 0;
00366:             if (next > cap) next = cap;
00367:             int after = (int)next;
00368:             if (after == sector.MethanePpm) return false;
00369:
00370:             int before = sector.MethanePpm;
00371:             sector.MethanePpm = after;
00372:
00373:             if (before <= MethaneIgnitionThresholdPpm && after > MethaneIgnitionThresholdPpm)
00374:             {
00375:                 _log.Warn($"[ExcavationHazard] methane ignition threshold crossed in {sector.SectorId} ({after}ppm)");
00376:                 OnMethaneIgnition?.Invoke(sector.SectorId);
00377:             }
00378:
00379:             return true;
00380:         }
00381:
00382:         private bool ApplyFloodDelta(ExcavationSectorHazardState sector, int deltaPermille)
00383:         {
00384:             if (sector == null || deltaPermille == 0) return false;
00385:
00386:             int before = sector.FloodLevelPermille;
00387:             int after = Math.Clamp(before + deltaPermille, 0, 1000);
00388:             if (after == before) return false;
00389:
00390:             sector.FloodLevelPermille = after;
00391:
00392:             if (before <= FloodCriticalThresholdPermille && after > FloodCriticalThresholdPermille)
00393:             {
00394:                 _log.Warn($"[ExcavationHazard] sector flooding in {sector.SectorId} ({after} permille)");
00395:                 OnSectorFlooded?.Invoke(sector.SectorId);
00396:             }
00397:
00398:             return true;
00399:         }
00400:
00401:         /// <summary>
00402:         /// Authored daily mitigation upkeep: an installed mitigation keeps
00403:         /// working between applications. The JSON
00404:         /// <c>passive_decay_bonus_permille</c> value applies to the same hazard
00405:         /// its primary effect targets (methane / flood / spores / shoring).
00406:         /// </summary>
00407:         private void ApplyPassiveMitigationDecay(ExcavationSectorHazardState sector)
00408:         {
00409:             if (sector?.InstalledMitigationIds == null) return;
00410:             for (int i = 0; i < sector.InstalledMitigationIds.Count; i++)
00411:             {
00412:                 string mitigationId = sector.InstalledMitigationIds[i];
00413:                 if (string.IsNullOrEmpty(mitigationId)) continue;
00414:                 if (!_catalog.TryGetValue(mitigationId, out var def) || def?.Effect == null) continue;
00415:                 if (!def.Effect.TryGetValue("passive_decay_bonus_permille", out int bonus) || bonus <= 0) continue;
00416:
00417:                 if (def.Effect.ContainsKey("methane_vent_rate_permille"))
00418:                     ApplyMethaneDelta(sector, -bonus, int.MaxValue);
00419:                 if (def.Effect.ContainsKey("flood_drain_rate_permille"))
00420:                     ApplyFloodDelta(sector, -bonus);
00421:                 if (def.Effect.ContainsKey("spore_reduction_permille"))
00422:                     sector.SporeConcentrationPermille = Math.Max(0, sector.SporeConcentrationPermille - bonus);
00423:                 if (def.Effect.ContainsKey("shoring_health_restore_permille"))
00424:                     sector.ShoringHealthPermille = Math.Min(1000, sector.ShoringHealthPermille + bonus);
00425:             }
00426:         }
00427:
00428:         public void TickDay(int day)
00429:         {
00430:             _state.currentDay = day;
00431:
00432:             foreach (var sector in _state.sectors.Values)
00433:             {
00434:                 if (sector.IsBulkheadSealed) continue;
00435:
00436:                 // Passive hazard accumulation (RNG draw order is part of the
00437:                 // deterministic replay contract and must not change).
00438:                 ApplyMethaneDelta(sector, _rng.Next(50, 150), int.MaxValue);
00439:
00440:                 // Authored passive mitigation upkeep (replaces the former
00441:                 // hardcoded blower-only drain; JSON is the authority).
00442:                 ApplyPassiveMitigationDecay(sector);
00443:
00444:                 // Passive shoring decay
00445:                 sector.ShoringHealthPermille = Math.Max(0, sector.ShoringHealthPermille - _rng.Next(20, 50));
00446:
00447:                 // Check rescue deadline expiry
00448:                 if (sector.ActiveTrappedMiners.Count > 0 && !sector.RescueCompleted && !sector.RescueFailed)
00449:                 {
00450:                     if (sector.RescueDeadlineDay.HasValue && sector.RescueDeadlineDay.Value <= day)
00451:                     {
00452:                         sector.RescueFailed = true;
00453:                         OnRescueFailed?.Invoke(sector.SectorId);
00454:                     }
00455:                 }
00456:             }
00457:
00458:             OnHazardStateChanged?.Invoke();
00459:         }
00460:
00461:         public ExcavationHazardSave CaptureState()
00462:         {
00463:             var s = new SystemTextJsonSerializer();
00464:             var json = s.Serialize(_state);
00465:             return s.Deserialize<ExcavationHazardSave>(json) ?? new ExcavationHazardSave();
00466:         }
00467:
00468:         public void RestoreState(ExcavationHazardSave? saved)
00469:         {
00470:             if (saved == null)
00471:             {
00472:                 _state = new ExcavationHazardSave();
00473:                 return;
00474:             }
00475:
00476:             var s = new SystemTextJsonSerializer();
00477:             var json = s.Serialize(saved);
00478:             _state = s.Deserialize<ExcavationHazardSave>(json) ?? new ExcavationHazardSave();
00479:             OnHazardStateChanged?.Invoke();
00480:         }
00481:     }
00482: }
```


# Appendix — Current Source Detail: `src/UI/ArchaeologyExcavationPanel.cs`

### `src/UI/ArchaeologyExcavationPanel.cs` — complete current file

- Size: 226 lines / 10735 bytes.
- SHA-256: `a57113819ff5b232473cdccda842264cbee2f4fa37564a93a9f850a45364ff3b`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005: using Godot;
00006: using Ashfall.Core.Archaeology;
00007: using Ashfall.Core.UI;
00008: using DesignTheme = Ashfall.Core.UI.Theme;
00009:
00010: namespace AtomicWar.GodotApp.UI
00011: {
00012:     /// <summary>
00013:     /// Plans 190–193: pre-war archaeology & decryption workflow.
00014:     /// Presentation-only — decryption and broker-sale commands are emitted via
00015:     /// <see cref="OnActionRequested"/> and resolved by the host through the
00016:     /// bound <see cref="ArchaeologySystem"/>.
00017:     /// </summary>
00018:     public partial class ArchaeologyExcavationPanel : Control, IBindablePanel
00019:     {
00020:         public event Action? OnClose;
00021:         public event Action<string, string>? OnActionRequested;
00022:
00023:         private AshfallDashboardShell _shell = null!;
00024:         private AshfallStatusRail? _statusRail;
00025:         private ArchaeologySystem? _system;
00026:         private ItemList _archiveList = null!;
00027:         private VBoxContainer _detail = null!;
00028:         private int _selectedArchiveIndex = -1;
00029:         private string _feedbackText = string.Empty;
00030:         private bool _feedbackIsFailure;
00031:
00032:         public bool IsBound => _system != null;
00033:
00034:         public void Bind(ArchaeologySystem system) { _system = system; _feedbackText = string.Empty; RefreshView(); }
00035:         public void Unbind() { _system = null; }
00036:
00037:         public override void _Ready()
00038:         {
00039:             SetAnchorsPreset(LayoutPreset.FullRect);
00040:
00041:             _shell = new AshfallDashboardShell("BEFORE // PRE-WAR ARCHIVES & DIG SITES", minWidth: 1000, minHeight: 650);
00042:
00043:             _statusRail = _shell.SetStatusRail();
00044:             _statusRail.AddCard("sites", "Dig Sites", "—", AshfallMetricCard.Criticality.Normal, minWidth: 90);
00045:             _statusRail.AddCard("archives", "Archives Held", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
00046:             _statusRail.AddCard("decrypted", "Decrypted", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
00047:             _statusRail.AddCard("research", "Research Value", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
00048:
00049:             _archiveList = new ItemList
00050:             {
00051:                 CustomMinimumSize = new Vector2(300, 0),
00052:                 SizeFlagsVertical = SizeFlags.ExpandFill,
00053:                 SizeFlagsHorizontal = SizeFlags.Fill
00054:             };
00055:             _archiveList.ItemSelected += index => { _selectedArchiveIndex = (int)index; RefreshView(); };
00056:
00057:             _detail = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
00058:
00059:             var bodyRow = new HBoxContainer();
00060:             bodyRow.AddChild(_archiveList);
00061:
00062:             var detailScroll = new ScrollContainer
00063:             {
00064:                 SizeFlagsVertical = SizeFlags.ExpandFill,
00065:                 SizeFlagsHorizontal = SizeFlags.ExpandFill
00066:             };
00067:             detailScroll.AddChild(_detail);
00068:             bodyRow.AddChild(detailScroll);
00069:
00070:             _shell.SetContent(bodyRow);
00071:             _shell.AttachHeaderCloseButton("CLOSE", () => OnClose?.Invoke());
00072:             AddChild(_shell);
00073:             Visible = false;
00074:         }
00075:
00076:         public void Open() { Visible = true; RefreshView(); }
00077:         public void Close()
00078:         {
00079:             if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
00080:                 Visible = false;
00081:             OnClose?.Invoke();
00082:         }
00083:
00084:         /// <summary>Last feedback line rendered by the panel (test/diagnostic surface).</summary>
00085:         public string LastFeedback { get; private set; } = string.Empty;
00086:
00087:         public void ShowFeedback(string message, bool isFailure)
00088:         {
00089:             _feedbackText = message;
00090:             _feedbackIsFailure = isFailure;
00091:             LastFeedback = message;
00092:             RefreshView();
00093:         }
00094:
00095:         private List<PreWarArchiveInstance> SortedArchives()
00096:         {
00097:             if (_system == null) return new List<PreWarArchiveInstance>();
00098:             return _system.Archives
00099:                 .OrderBy(a => a.archiveId, StringComparer.Ordinal)
00100:                 .ToList();
00101:         }
00102:
00103:         public void RefreshView()
00104:         {
00105:             if (_system == null || _detail == null || _archiveList == null) return;
00106:             AshfallUiHelpers.EmptyChildren(_detail);
00107:
00108:             var archives = SortedArchives();
00109:             _selectedArchiveIndex = Math.Clamp(_selectedArchiveIndex, -1, Math.Max(0, archives.Count - 1));
00110:
00111:             int decrypted = archives.Count(a => a.unlocked);
00112:             int research = archives.Where(a => a.unlocked && !a.researchClaimed).Sum(a => a.researchPoints);
00113:
00114:             if (_statusRail != null)
00115:             {
00116:                 _statusRail.Set("sites", $"{_system.Sites.Count(s => s.discovered)}/{_system.Sites.Count}", AshfallMetricCard.Criticality.Normal);
00117:                 _statusRail.Set("archives", archives.Count.ToString(), AshfallMetricCard.Criticality.Normal);
00118:                 _statusRail.Set("decrypted", decrypted.ToString(),
00119:                     decrypted > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);
00120:                 _statusRail.Set("research", research > 0 ? $"{research} pts" : "—",
00121:                     research > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Normal);
00122:             }
00123:
00124:             _archiveList.Clear();
00125:             for (int i = 0; i < archives.Count; i++)
00126:             {
00127:                 var a = archives[i];
00128:                 string state = a.corrupted ? "CORRUPTED" : a.unlocked ? "DECRYPTED" : a.sold ? "SOLD" : $"encrypted T{a.encryptionTier} — {a.decryptionProgress:0}%";
00129:                 _archiveList.AddItem($"{ArchiveTitle(a)} — {state}", null, false);
00130:                 if (i == _selectedArchiveIndex)
00131:                     _archiveList.Select(i);
00132:             }
00133:             if (archives.Count == 0)
00134:                 _archiveList.AddItem("No archives recovered", null, false);
00135:
00136:             var selected = _selectedArchiveIndex >= 0 && _selectedArchiveIndex < archives.Count
00137:                 ? archives[_selectedArchiveIndex] : null;
00138:
00139:             // ── Dig sites ──
00140:             if (_system.Sites.Count > 0)
00141:             {
00142:                 _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("DIG SITES"));
00143:                 foreach (var site in _system.Sites)
00144:                 {
00145:                     if (site == null) continue;
00146:                     string state = site.exhausted ? "exhausted" : site.discovered ? $"{site.excavationProgress:0}% excavated" : "undiscovered";
00147:                     _detail.AddChild(AshfallUiHelpers.MakeDataRow(
00148:                         string.IsNullOrEmpty(site.displayName) ? ItemDisplay.Prettify(site.siteId) : site.displayName,
00149:                         state,
00150:                         site.exhausted ? AshfallUiHelpers.ColorDim :
00151:                         site.discovered ? AshfallUiHelpers.ColorText : AshfallUiHelpers.ColorMuted));
00152:                 }
00153:             }
00154:
00155:             // ── Archive detail ──
00156:             if (selected != null)
00157:             {
00158:                 _detail.AddChild(AshfallUiHelpers.MakeSeparator());
00159:                 _detail.AddChild(AshfallUiHelpers.MakeSectionHeader(ArchiveTitle(selected).ToUpperInvariant()));
00160:                 _detail.AddChild(AshfallUiHelpers.MakeDataRow("Encryption tier", $"T{selected.encryptionTier}", AshfallUiHelpers.ColorText));
00161:                 _detail.AddChild(AshfallUiHelpers.MakeDataRow("Decryption", $"{selected.decryptionProgress:0}%",
00162:                     selected.unlocked ? AshfallUiHelpers.ColorSuccess :
00163:                     selected.corrupted ? AshfallUiHelpers.ColorCritical :
00164:                     selected.decryptionProgress > 0 ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorDim));
00165:                 _detail.AddChild(AshfallUiHelpers.MakeDataRow("Research value", selected.unlocked ? $"{selected.researchPoints} pts" : "locked",
00166:                     selected.unlocked ? AshfallUiHelpers.ColorInfo : AshfallUiHelpers.ColorDim));
00167:
00168:                 if (selected.corrupted)
00169:                     _detail.AddChild(AshfallUiHelpers.MakeWarning("The archive degraded past recovery — the data is gone."));
00170:
00171:                 if (!string.IsNullOrEmpty(_feedbackText))
00172:                 {
00173:                     _detail.AddChild(AshfallUiHelpers.MakeSeparator());
00174:                     _detail.AddChild(_feedbackIsFailure
00175:                         ? AshfallUiHelpers.MakeWarning(_feedbackText)
00176:                         : AshfallUiHelpers.MakeSuccess(_feedbackText));
00177:                 }
00178:
00179:                 // ── Actions ──
00180:                 _detail.AddChild(AshfallUiHelpers.MakeSeparator());
00181:                 _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("ACTIONS"));
00182:                 var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
00183:
00184:                 if (!selected.unlocked && !selected.corrupted && !selected.sold)
00185:                 {
00186:                     var decryptBtn = AshfallUiHelpers.MakeButton("RUN DECRYPTION SHIFT", () =>
00187:                         OnActionRequested?.Invoke("decrypt", selected.archiveId));
00188:                     decryptBtn.TooltipText = "Puts an engineer on the archive for a work shift. Higher tiers need power and keycards.";
00189:                     row.AddChild(decryptBtn);
00190:                 }
00191:                 if (selected.unlocked && !selected.researchClaimed)
00192:                 {
00193:                     var sellBtn = AshfallUiHelpers.MakeButton("SELL TO BROKER", () =>
00194:                         OnActionRequested?.Invoke("sell", selected.archiveId));
00195:                     sellBtn.TooltipText = "Hands the archive to a broker for cap-and-lead. The research value is lost.";
00196:                     row.AddChild(sellBtn);
00197:                 }
00198:
00199:                 if (row.GetChildCount() == 0)
00200:                     row.AddChild(AshfallUiHelpers.MakeMetadata(selected.unlocked
00201:                         ? "Decrypted and claimed — the knowledge is already ours."
00202:                         : "Nothing to do for this archive right now."));
00203:                 _detail.AddChild(row);
00204:             }
00205:             else
00206:             {
00207:                 _detail.AddChild(AshfallUiHelpers.MakeEmptyState(
00208:                     "No pre-war archives in hand. Excavation parties bring data storages back from the digs.",
00209:                     title: "NO ARCHIVES"));
00210:             }
00211:         }
00212:
00213:         private static string ArchiveTitle(PreWarArchiveInstance a) =>
00214:             string.IsNullOrEmpty(a.titleKey) ? ItemDisplay.Prettify(a.archiveId) : ItemDisplay.Prettify(a.titleKey);
00215:
00216:         public override void _UnhandledInput(InputEvent @event)
00217:         {
00218:             if (!Visible) return;
00219:             if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00220:             {
00221:                 OnClose?.Invoke();
00222:                 GetViewport().SetInputAsHandled();
00223:             }
00224:         }
00225:     }
00226: }
```


# Appendix — Current Source Detail: `src/UI/ExcavationPanel.cs`

### `src/UI/ExcavationPanel.cs` — complete current file

- Size: 106 lines / 3526 bytes.
- SHA-256: `af45bc1a0dc912767c8cbd38b370911df71852bfae0f5a0f9b5a4bc294b23a75`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Godot;
00004: using Ashfall.Core;
00005: using Ashfall.Core.UI;
00006: using AtomicWar.GodotApp;
00007: using DesignTheme = Ashfall.Core.UI.Theme;
00008:
00009: namespace AtomicWar.GodotApp.UI
00010: {
00011:     public partial class ExcavationPanel : Control, IBindablePanel
00012:     {
00013:         public event Action? OnClose;
00014:
00015:         private AshfallDashboardShell _shell = null!;
00016:         private AshfallStatusRail? _statusRail;
00017:         private VBoxContainer _contentStack = null!;
00018:         private Label _detailText = null!;
00019:
00020:         private ExcavationHostSession? _host;
00021:
00022:         public bool IsBound => _host != null;
00023:
00024:         public void Bind(ExcavationHostSession session)
00025:         {
00026:             _host = session;
00027:             if (_host != null)
00028:             {
00029:                 _host.StateChanged += RefreshView;
00030:             }
00031:             RefreshView();
00032:         }
00033:
00034:         public void Unbind()
00035:         {
00036:             if (_host != null)
00037:             {
00038:                 _host.StateChanged -= RefreshView;
00039:                 _host = null;
00040:             }
00041:         }
00042:
00043:
00044:
00045:         public override void _Ready()
00046:         {
00047:             SetAnchorsPreset(LayoutPreset.FullRect);
00048:
00049:             _shell = new AshfallDashboardShell("Subterranean Excavation // Deep Strata", minWidth: 1000, minHeight: 650);
00050:             AddChild(_shell);
00051:
00052:             _statusRail = _shell.SetStatusRail();
00053:             _statusRail.AddCard("active_sites", "Excavation Sites", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
00054:
00055:             _contentStack = new VBoxContainer();
00056:             _contentStack.AddThemeConstantOverride("separation", 12);
00057:             _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00058:             _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;
00059:
00060:             _detailText = AshfallUiHelpers.MakeBody("", autowrap: true);
00061:             _contentStack.AddChild(_detailText);
00062:
00063:             _shell.SetContent(_contentStack);
00064:
00065:             _shell.AttachHeaderCloseButton("CLOSE", () =>
00066:             {
00067:                 Visible = false;
00068:                 OnClose?.Invoke();
00069:             });
00070:
00071:             RefreshView();
00072:         }
00073:
00074:         public void RefreshView()
00075:         {
00076:             if (_host == null || _statusRail == null) return;
00077:
00078:             var s = _host.System.State;
00079:             _statusRail.Set("active_sites", s.sites.Count.ToString(), AshfallMetricCard.Criticality.Normal);
00080:
00081:             if (_detailText != null)
00082:             {
00083:                 if (s.sites.Count == 0)
00084:                 {
00085:                     _detailText.Text = "No subterranean excavation sites currently active.\nAssign surveying teams or discover buried deep-strata vaults to begin excavation operations.\n\nLast Event: " + (string.IsNullOrEmpty(_host.LastEvent) ? "None recorded" : _host.LastEvent);
00086:                 }
00087:                 else
00088:                 {
00089:                     string text = $"Subterranean Excavation Sites ({s.sites.Count} total):\n";
00090:                     foreach (var site in s.sites)
00091:                     {
00092:                         text += $"  • [{site.siteId}] Progress: {site.progress:F0}/{site.requiredProgress:F0} | Workers: {site.assignedWorkerCount} | Shoring: {(site.shoringApplied ? "REINFORCED" : "UNSHORED")} | Cave-in Risk: {site.structuralRisk:P0}\n";
00093:                     }
00094:                     text += $"\nLast Event: {_host.LastEvent}";
00095:                     _detailText.Text = text;
00096:                 }
00097:             }
00098:         }
00099:
00100:         public override void _ExitTree()
00101:         {
00102:             Unbind();
00103:             base._ExitTree();
00104:         }
00105:     }
00106: }
```


# Appendix — Current Source Detail: `src/Host/ExcavationHostSession.cs`

### `src/Host/ExcavationHostSession.cs` — complete current file

- Size: 72 lines / 2200 bytes.
- SHA-256: `82d484a67d1feeb0bf32a6f1f04c3131ea7d31d947253eaa8c25784dcbb83109`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Godot;
00004: using Ashfall.Core;
00005:
00006: namespace AtomicWar.GodotApp
00007: {
00008:     /// <summary>
00009:     /// Host session for ExcavationSystem.
00010:     /// Manages underground rubble clearing, worker assignments, structural shoring, cave-in risk, and room discovery.
00011:     /// </summary>
00012:     public sealed class ExcavationHostSession
00013:     : HostSessionBase{
00014:         public ExcavationSystem System { get; }
00015:         public string LastEvent { get; private set; } = string.Empty;
00016:         public ExcavationHostSession(ExcavationSystem system)
00017:         {
00018:             System = system ?? new ExcavationSystem(new SeededRng(1986), new GodotLog());
00019:
00020:             System.OnExcavationChanged += () =>
00021:             {
00022:                 RaiseStateChanged();
00023:             };
00024:         }
00025:
00026:         public ActionResult AddSite(string siteId, string blueprintId, float requiredProgress = 100f, float risk = 0.2f)
00027:         {
00028:             var res = System.AddSite(siteId, blueprintId, requiredProgress, risk);
00029:             if (res.IsSuccess)
00030:             {
00031:                 LastEvent = $"Surveyed new excavation site: {siteId}";
00032:                 RaiseStateChanged();
00033:             }
00034:             return res;
00035:         }
00036:
00037:         public ActionResult AssignWorkers(string siteId, int workerCount)
00038:         {
00039:             var res = System.AssignWorkers(siteId, workerCount);
00040:             if (res.IsSuccess)
00041:             {
00042:                 LastEvent = $"Assigned {workerCount} workers to excavation site {siteId}";
00043:                 RaiseStateChanged();
00044:             }
00045:             return res;
00046:         }
00047:
00048:         public ActionResult ApplyShoring(string siteId)
00049:         {
00050:             var res = System.ApplyShoring(siteId);
00051:             if (res.IsSuccess)
00052:             {
00053:                 LastEvent = $"Reinforced shoring on excavation site {siteId}";
00054:                 RaiseStateChanged();
00055:             }
00056:             return res;
00057:         }
00058:
00059:         public void TickDay()
00060:         {
00061:             System.TickDay();
00062:             RaiseStateChanged();
00063:         }
00064:
00065:         public override void Save()
00066:         {
00067:             if (!IsDirty) return;
00068:             ExcavationSaveStore.TrySave(System.CaptureState());
00069:             base.Save();
00070:         }
00071:     }
00072: }
```


# Appendix — Current Source Detail: `src/Main.ShelterSocial.cs`

### `src/Main.ShelterSocial.cs` — complete current file

- Size: 602 lines / 30015 bytes.
- SHA-256: `efdcc3d73d212c2e6349a7a1bd586b42f082827ea76457826c93321a1fea5415`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Inventory;
00007: using Ashfall.Core.Medical;
00008: using Ashfall.Core.Radiation;
00009: using Ashfall.Core.Shelter;
00010: using Ashfall.Core.StartingLevel;
00011: using Ashfall.Core.Survivors;
00012: using Ashfall.Core.YearOfAsh;
00013: using Ashfall.Core.World;
00014: using Ashfall.Core.Crafting;
00015: using Ashfall.Core.Journal;
00016: using Ashfall.Core.Expeditions;
00017: using Ashfall.Core.Narrative;
00018: using AtomicWar.GodotApp.UI;
00019:
00020: namespace AtomicWar.GodotApp
00021: {
00022:     public partial class Main : Control
00023:     {
00024:         private SurvivorRelationsHostSession _survivorRelations = null!;
00025:         private SurvivorRelationsPanel _survivorRelationsPanel = null!;
00026:         private bool _survivorRelationsDirty;
00027:         private RegionalTreatyHostSession _regionalTreaty = null!;
00028:         private RegionalTreatyPanel _regionalTreatyPanel = null!;
00029:         private bool _regionalTreatyDirty;
00030:         private VinylMoraleHostSession _vinylMorale = null!;
00031:         private VinylMoralePanel _vinylMoralePanel = null!;
00032:         private bool _vinylMoraleDirty;
00033:         private WildlifeTrappingHostSession _wildlifeTrapping = null!;
00034:         private WildlifeTrappingPanel _wildlifeTrappingPanel = null!;
00035:         private bool _wildlifeTrappingDirty;
00036:         private ExcavationHostSession _excavation = null!;
00037:         private ExcavationPanel _excavationPanel = null!;
00038:         private bool _excavationDirty;
00039:         private ApprenticeshipHostSession _apprenticeship = null!;
00040:         private ApprenticeshipPanel _apprenticeshipPanel = null!;
00041:         private bool _apprenticeshipDirty;
00042:         private CaregivingHostSession _caregiving = null!;
00043:         private CaregivingPanel _caregivingPanel = null!;
00044:         private bool _caregivingDirty;
00045:
00046:         private void SetupSurvivorRelations()
00047:         {
00048:             if (_survivorRelations != null) return;
00049:             SetupCampaignDay();
00050:             var srState = SurvivorRelationsSaveStore.TryLoad() ?? new SurvivorRelationsState();
00051:             var srSys = new SurvivorRelationsSystem(_campaignDay.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.Social).Rng, new GodotLog());
00052:             _survivorRelationsCore = srSys;
00053:             srSys.RestoreState(srState);
00054:             // Plan 202: every canonical mediation (including the existing
00055:             // panel route) emits one typed morale outcome to the sole Needs
00056:             // owner. The relation ledger remains the authority for affinity
00057:             // and mediation history.
00058:             srSys.OnConflictResolved += ApplyInterpersonalConflictMorale;
00059:             _survivorRelations = new SurvivorRelationsHostSession(srSys);
00060:             if (_survivorRelationsPanel != null && _survivorRelationsPanel.IsInsideTree())
00061:                 RemoveChild(_survivorRelationsPanel);
00062:             _survivorRelationsPanel = new SurvivorRelationsPanel();
00063:             _survivorRelationsPanel.Bind(_survivorRelations);
00064:             _survivorRelationsPanel.Visible = false;
00065:             AddChild(_survivorRelationsPanel);
00066:         }
00067:
00068:         private void SaveSurvivorRelations()
00069:         {
00070:             if (_survivorRelations != null)
00071:                 CaptureSection("survivor_relations", SurvivorRelationsSaveStore.TryCapturePersisted(_survivorRelations.System.CaptureState()));
00072:         }
00073:
00074:         private void SetupRegionalTreaty()
00075:         {
00076:             if (_regionalTreaty != null) return;
00077:             var rtState = RegionalTreatySaveStore.TryLoad() ?? new RegionalTreatyState();
00078:             var rtSys = new RegionalTreatySystem(new GodotLog());
00079:             rtSys.RestoreState(rtState);
00080:             // Mechanical treaty catalog only. Narrative protocols and foundry
00081:             // accords are different schemas and must not be fed into this system.
00082:             if (!string.IsNullOrEmpty(_dataDir))
00083:             {
00084:                 var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
00085:                 var json = new SystemTextJsonSerializer();
00086:                 rtSys.LoadCatalog(RegionalTreatyCatalogLoader.Load(_dataDir, fileIO, json));
00087:             }
00088:             _regionalTreaty = new RegionalTreatyHostSession(rtSys);
00089:
00090:             // Plan VIII · Task 21 — typed treaty transitions become world
00091:             // consequences through the canonical consumers: faction-war standing
00092:             // (escalation spine, 21.10) and the radio broadcast wire (21.6).
00093:             // RestoreState never emits transitions, so neither consumer can
00094:             // double-apply across a save/load.
00095:             rtSys.OnTreatyTransition += transition =>
00096:                 OnTreatyTransitionWorldConsequences(rtSys, transition);
00097:
00098:             if (_regionalTreatyPanel != null && _regionalTreatyPanel.IsInsideTree())
00099:                 RemoveChild(_regionalTreatyPanel);
00100:             _regionalTreatyPanel = new RegionalTreatyPanel();
00101:             _regionalTreatyPanel.Bind(_regionalTreaty);
00102:             _regionalTreatyPanel.Visible = false;
00103:             AddChild(_regionalTreatyPanel);
00104:         }
00105:
00106:         private void OnTreatyTransitionWorldConsequences(RegionalTreatySystem treatySystem, TreatyTransition transition)
00107:         {
00108:             if (transition.IsBreach && !string.IsNullOrEmpty(transition.FactionId))
00109:             {
00110:                 // Task 21.10 — through the canonical escalation API only
00111:                 // (FactionWarSystem.ModifyStanding clamps and raises its own event).
00112:                 var def = treatySystem.GetDefinition(transition.TreatyId);
00113:                 int penalty = def != null ? (int)def.violation_penalty_affinity : -20;
00114:                 _yearOfAsh?.FactionWar.ModifyStanding(transition.FactionId, penalty);
00115:             }
00116:
00117:             if (_radio != null)
00118:             {
00119:                 var def = treatySystem.GetDefinition(transition.TreatyId);
00120:                 _radio.ScheduleCoordinator.InjectTreatyAlert(TreatyBulletins.Compose(transition, def));
00121:             }
00122:         }
00123:
00124:         private void SaveRegionalTreaty()
00125:         {
00126:             if (_regionalTreaty != null)
00127:                 CaptureSection("regional_treaty", RegionalTreatySaveStore.TryCapturePersisted(_regionalTreaty.System.CaptureState()));
00128:         }
00129:
00130:         private void SetupVinylMorale()
00131:         {
00132:             if (_vinylMorale != null) return;
00133:             var vmState = VinylMoraleSaveStore.TryLoad() ?? new VinylMoraleState();
00134:             var vmSys = new VinylMoraleSystem(new GodotLog());
00135:             vmSys.RestoreState(vmState);
00136:             LoadVinylRecordCatalog(vmSys);
00137:             _vinylMorale = new VinylMoraleHostSession(vmSys);
00138:             _vinylMorale.DayProvider = () => _simDay;
00139:             _vinylMorale.System.OnMoraleApplied += amount =>
00140:             {
00141:                 if (amount > 0 && _survivors?.Needs != null)
00142:                 {
00143:                     foreach (var sv in _survivors.Needs.Registered)
00144:                     {
00145:                         _survivors.Needs.Modify(sv, NeedKind.Morale, amount);
00146:                     }
00147:                 }
00148:             };
00149:             if (_vinylMoralePanel != null && _vinylMoralePanel.IsInsideTree())
00150:                 RemoveChild(_vinylMoralePanel);
00151:             _vinylMoralePanel = new VinylMoralePanel();
00152:             _vinylMoralePanel.Bind(_vinylMorale);
00153:             _vinylMoralePanel.Visible = false;
00154:             AddChild(_vinylMoralePanel);
00155:         }
00156:
00157:         /// <summary>
00158:         /// Load the pre-war vinyl record archive (narrative/vinyl_record_archive.json)
00159:         /// into the VinylMoraleSystem. The archive uses the Narrative VinylRecordEntry
00160:         /// shape (rich archival metadata); the morale system uses VinylRecordDefinition
00161:         /// (playback-focused). This bridges the two without a second catalog file.
00162:         /// Missing file is non-fatal — the system runs with an empty catalog (headless tests).
00163:         /// </summary>
00164:         private void LoadVinylRecordCatalog(VinylMoraleSystem system)
00165:         {
00166:             string path = System.IO.Path.Combine(_dataDir, "narrative", "vinyl_record_archive.json");
00167:             if (!System.IO.File.Exists(path)) return;
00168:             string json = System.IO.File.ReadAllText(path);
00169:             if (string.IsNullOrWhiteSpace(json)) return;
00170:
00171:             var file = new SystemTextJsonSerializer().Deserialize<VinylRecordsFile>(json);
00172:             if (file?.records == null) return;
00173:
00174:             var defs = new List<VinylRecordDefinition>(file.records.Count);
00175:             foreach (var r in file.records)
00176:             {
00177:                 if (r == null || string.IsNullOrEmpty(r.record_id)) continue;
00178:                 // Genre: prefer the first tag (e.g. "classical", "jazz", "folk");
00179:                 // IsRareCulturalRecord checks genre for classical/jazz/symphony/hymnal.
00180:                 string genre = (r.tags != null && r.tags.Length > 0) ? r.tags[0] : string.Empty;
00181:                 defs.Add(new VinylRecordDefinition
00182:                 {
00183:                     record_id = r.record_id,
00184:                     display_name = !string.IsNullOrEmpty(r.title) ? r.title : r.record_id,
00185:                     genre = genre,
00186:                     morale_daily_bonus = r.daily_morale_modifier,
00187:                     flashback_suppression = 0f,
00188:                     audio_cue_id = string.Empty,
00189:                     description = !string.IsNullOrEmpty(r.dweller_resonance_notes)
00190:                         ? r.dweller_resonance_notes
00191:                         : (r.needle_audio_texture ?? string.Empty)
00192:                 });
00193:             }
00194:             system.LoadCatalog(defs);
00195:         }
00196:
00197:         private void SaveVinylMorale()
00198:         {
00199:             if (_vinylMorale != null)
00200:                 CaptureSection("vinyl_morale", VinylMoraleSaveStore.TryCapturePersisted(_vinylMorale.System.CaptureState()));
00201:         }
00202:
00203:         private void SetupWildlifeTrapping()
00204:         {
00205:             if (_wildlifeTrapping != null) return;
00206:             SetupCampaignDay();
00207:             var wtrapState = WildlifeTrappingSaveStore.TryLoad() ?? new WildlifeTrappingState();
00208:             var wtrapSys = new WildlifeTrappingSystem(_campaignDay.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.Shelter).Rng, new GodotLog());
00209:             // Plan 36: load trapping catalog and register prey/bait definitions
00210:             WildlifeTrappingCatalog? trapCatalog = null;
00211:             if (!string.IsNullOrEmpty(_dataDir))
00212:             {
00213:                 var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
00214:                 var json = new SystemTextJsonSerializer();
00215:                 trapCatalog = WildlifeTrappingCatalogLoader.Load(_dataDir, fileIO, json, new GodotLog());
00216:                 if (trapCatalog != null) trapCatalog.RegisterWith(wtrapSys);
00217:             }
00218:             wtrapSys.RestoreState(wtrapState);
00219:             // C2 / Plan 20C (§40) — trap penalties from the ONE weather-effects
00220:             // table (penalty = 1 − trap_yield_multiplier); unbound → the legacy
00221:             // hardcoded curve byte-identical.
00222:             wtrapSys.WeatherPenaltyProvider = kind =>
00223:             {
00224:                 if (_world?.WeatherEffects != null
00225:                     && _world.WeatherEffects.TryGetEffects(kind, out var fx)
00226:                     && fx != null)
00227:                     return 1f - fx.trap_yield_multiplier;
00228:                 return WildlifeTrappingSystem.WeatherPenaltyFor(kind);
00229:             };
00230:             _wildlifeTrapping = new WildlifeTrappingHostSession(wtrapSys);
00231:             _wildlifeTrapping.Catalog = trapCatalog;
00232:             _wildlifeTrapping.Inventory = _inventory;
00233:             _wildlifeTrapping.DeliverButcheryFood = foodUnits =>
00234:                 _inventory != null && _inventory.TryAdd("raw_meat", foodUnits);
00235:             _wildlifeTrapping.ApplyMorale = (survivorId, delta, source) =>
00236:             {
00237:                 if (_survivors == null) SetupSurvivors();
00238:                 var needs = _survivors?.Needs;
00239:                 if (needs == null || needs.Get(survivorId) == null)
00240:                 {
00241:                     GD.PushWarning($"[WildlifeTrapping] Cannot apply morale {delta:0.###} to missing survivor '{survivorId}' ({source}).");
00242:                     return;
00243:                 }
00244:                 needs.Modify(survivorId, NeedKind.Morale, delta);
00245:             };
00246:
00247:             // Plan VI: bycatch is a domain fact; the authored narrative
00248:             // authority owns its prose and presentation. The source key is
00249:             // stable across restore so a repeated host subscription cannot
00250:             // duplicate the notification.
00251:             _wildlifeTrapping.OnBycatchOccurred += occurrence =>
00252:             {
00253:                 if (occurrence == null) return;
00254:                 SetupEventsHost();
00255:                 if (!_eventsHost.TryGetEvent("event_trapping_bycatch_entanglement", out var authored))
00256:                 {
00257:                     GD.PushWarning("[WildlifeTrapping] Bycatch narrative event is not authored; domain fact remains in the journal.");
00258:                     return;
00259:                 }
00260:                 SetupEventAdapter();
00261:                 string sourceId = $"wildlife-trap:{occurrence.siteId}:bycatch:{occurrence.day}:{occurrence.bycatchSpeciesId}";
00262:                 _hostEventAdapter?.DispatchCatalogEvent(
00263:                     authored.Id,
00264:                     authored.BodyText,
00265:                     occurrence.day,
00266:                     sourceId);
00267:             };
00268:             SetupWorld();
00269:             _wildlifeTrapping.Map = _world?.WastelandMap;
00270:
00271:             // Contextual trapping lessons are routed through the persisted
00272:             // onboarding authority, never opened by the Core domain itself.
00273:             SetupOnboarding();
00274:             _wildlifeTrapping.OnTrapCrafted += _ =>
00275:                 _onboardingJourney?.RequestContextualTutorial(
00276:                     Ashfall.Core.Localization.WildlifeTrappingLocalization.FirstSnareTutorialId);
00277:             wtrapSys.OnTrapDeployed += _ =>
00278:                 _onboardingJourney?.RequestContextualTutorial(
00279:                     Ashfall.Core.Localization.WildlifeTrappingLocalization.FirstSnareTutorialId);
00280:             wtrapSys.OnTrapBroken += _ =>
00281:                 _onboardingJourney?.RequestContextualTutorial(
00282:                     Ashfall.Core.Localization.WildlifeTrappingLocalization.WearOutTutorialId);
00283:             wtrapSys.OnBycatchOccurred += (_, _, _, _, _, _) =>
00284:                 _onboardingJourney?.RequestContextualTutorial(
00285:                     Ashfall.Core.Localization.WildlifeTrappingLocalization.BycatchTutorialId);
00286:             // Plan 36 Closure II: wire disease/contamination delegates to live authorities
00287:             _wildlifeTrapping.ApplyDisease = (survivorId, diseaseId, day) =>
00288:             {
00289:                 if (string.IsNullOrEmpty(survivorId)) return;
00290:                 var def = _survivors?.Roster?.FindDefinition(survivorId);
00291:                 if (def != null && def.traitIds != null && def.traitIds.Contains("skill_sanitization_expert"))
00292:                     return;
00293:                 if (_disease == null) SetupDisease();
00294:                 if (_disease?.Engine != null)
00295:                 {
00296:                     _disease.Engine.Infect(survivorId, diseaseId, day);
00297:                 }
00298:                 else
00299:                 {
00300:                     GD.PrintErr($"[Ashfall Godot] Trapping: cannot apply disease '{diseaseId}' to '{survivorId}' - disease authority offline.");
00301:                 }
00302:             };
00303:             _wildlifeTrapping.ApplyContamination = (survivorId, dose) =>
00304:             {
00305:                 if (string.IsNullOrEmpty(survivorId) || dose <= 0f) return;
00306:                 if (_survivors == null) SetupSurvivors();
00307:                 if (_survivors != null)
00308:                 {
00309:                     _survivors.ExposeToZone(survivorId, dose);
00310:                 }
00311:                 else
00312:                 {
00313:                     GD.PrintErr($"[Ashfall Godot] Trapping: cannot apply contamination dose {dose} to '{survivorId}' - survivors authority offline.");
00314:                 }
00315:             };
00316:
00317:             // ── Plan IV: destination-authority adapters ──
00318:             // Trapping emits pending domain facts; these adapters hand each
00319:             // one to the owning authority. A rejected/absent destination
00320:             // leaves the fact pending for retry — nothing is dropped.
00321:
00322:             // Task 5 — moral authority. The dilemma itself is an authored
00323:             // quest in the moral catalog; surfacing is derived from the
00324:             // pending outbox (GetAvailableMoralChoices), and the ack happens
00325:             // at RESOLUTION so a save before the player decides replays the
00326:             // exact same dilemma. Unknown quests stay pending with a warning.
00327:             _wildlifeTrapping.DeliverMoralConsequence = (questId, speciesId, survivorId) =>
00328:             {
00329:                 SetupMoralChoice();
00330:                 if (_moralChoice.GetQuest(questId) == null)
00331:                 {
00332:                     GD.PushWarning($"[WildlifeTrapping] Moral quest '{questId}' not registered; consequence stays pending.");
00333:                     return false;
00334:                 }
00335:                 // The moral ledger owns persistence after acceptance; a
00336:                 // resolved quest is acked immediately so restore never
00337:                 // re-dispatches an already-resolved dilemma.
00338:                 return _moralChoice.IsResolved(questId);
00339:             };
00340:
00341:             // Task 6 — encounter authority. Pending interference encounters
00342:             // persist in the narrative encounter state and surface through
00343:             // the existing pending-encounter projection.
00344:             _wildlifeTrapping.DeliverTrapEncounter = (encounterId, siteId, day) =>
00345:             {
00346:                 if (_narrative == null) return false;
00347:                 var engine = _narrative.Engine;
00348:                 if (engine == null || engine.Find(encounterId) == null)
00349:                 {
00350:                     GD.PushWarning($"[WildlifeTrapping] Encounter '{encounterId}' not registered; fact stays pending.");
00351:                     return false;
00352:                 }
00353:                 var pendingList = engine.State.pending;
00354:                 for (int i = 0; i < pendingList.Count; i++)
00355:                 {
00356:                     var p = pendingList[i];
00357:                     if (p != null && string.Equals(p.encounterId, encounterId, StringComparison.Ordinal)
00358:                         && string.Equals(p.locationId, siteId, StringComparison.Ordinal))
00359:                         return true; // already queued — idempotent re-ack
00360:                 }
00361:                 engine.EnqueuePending(encounterId, siteId, 0, day);
00362:                 return true;
00363:             };
00364:
00365:             // Plan VI: miss-only atmospheric incidents are delivered through
00366:             // the same catalog/event adapter. A failed dispatch leaves the
00367:             // Core outbox pending for a later retry.
00368:             _wildlifeTrapping.DeliverNarrativeIncident = (eventId, siteId, day, sourceId) =>
00369:             {
00370:                 SetupEventsHost();
00371:                 if (!_eventsHost.TryGetEvent(eventId, out var authored))
00372:                 {
00373:                     GD.PushWarning($"[WildlifeTrapping] Narrative incident '{eventId}' is not registered; fact stays pending.");
00374:                     return false;
00375:                 }
00376:                 SetupEventAdapter();
00377:                 return _hostEventAdapter != null
00378:                     && _hostEventAdapter.DispatchCatalogEvent(eventId, authored.BodyText, day, sourceId);
00379:             };
00380:
00381:             // Task 7 — radio authority. One dynamic wildlife-net slot: while
00382:             // an unsurfaced report occupies it, later facts stay pending and
00383:             // deliver in sequence order as the slot frees.
00384:             _wildlifeTrapping.DeliverTrappingBroadcast = message =>
00385:             {
00386:                 if (_radio == null) return false;
00387:                 var coordinator = _radio.ScheduleCoordinator;
00388:                 if (coordinator == null || coordinator.HasTrappingAlert) return false;
00389:                 coordinator.InjectTrappingAlert(message);
00390:                 return true;
00391:             };
00392:
00393:             // Plan 28 Phase 3 (overhunt): snare catches thin the local packs
00394:             // through the migration system's bounded harvest pressure.
00395:             _wildlifeTrapping.OnCatchPressure += caught =>
00396:             {
00397:                 if (_world == null) return;
00398:                 var sector = _world.ShelterSectorId;
00399:                 if (!string.IsNullOrEmpty(sector))
00400:                     _world.Wildlife.ApplyHarvestPressure(sector, caught);
00401:             };
00402:
00403:             // WT-INT-01: wire first-catch species discovery to Journal and Codex
00404:             wtrapSys.OnNewSpeciesDiscovered += (speciesId, siteId, hunterId) =>
00405:             {
00406:                 if (_journal == null) SetupJournal();
00407:                 if (_journal == null) return;
00408:
00409:                 string knowledgeKey = Ashfall.Core.Journal.KnowledgeKeys.WildlifeSpeciesCaught(speciesId);
00410:
00411:                 // Resolve display name for species from catalog if available
00412:                 string speciesName = speciesId;
00413:                 if (trapCatalog != null && trapCatalog.Prey.TryGetValue(speciesId, out var preyDef) && !string.IsNullOrEmpty(preyDef.displayName))
00414:                 {
00415:                     speciesName = preyDef.displayName;
00416:                 }
00417:
00418:                 // Resolve author: assigned hunter -> shelter fallback
00419:                 Ashfall.Core.Journal.ISurvivorAuthor? author = null;
00420:                 if (_survivors?.Roster != null && !string.IsNullOrEmpty(hunterId))
00421:                 {
00422:                     var survivorDef = _survivors.Roster.FindDefinition(hunterId);
00423:                     if (survivorDef != null)
00424:                     {
00425:                         author = new TrappingJournalAuthor(survivorDef.id, survivorDef.displayName);
00426:                     }
00427:                 }
00428:                 author ??= new TrappingJournalAuthor(
00429:                     string.IsNullOrEmpty(hunterId) ? "shelter_crew" : hunterId,
00430:                     string.IsNullOrEmpty(hunterId) ? "Shelter Trapper" : hunterId);
00431:
00432:                 string text = $"Captured first specimen of {speciesName} at trap site {siteId} (hunter: {author.DisplayName}).";
00433:                 _journal.TryDiscoverRawKnowledge(knowledgeKey, text, author, _simDay);
00434:             };
00435:
00436:             // Plan 36 III: wire bycatch occurrences to Journal
00437:             wtrapSys.OnBycatchOccurred += (siteId, trapId, primarySpecies, bycatchSpecies, day, hunterId) =>
00438:             {
00439:                 if (_journal == null) SetupJournal();
00440:                 if (_journal == null) return;
00441:
00442:                 string knowledgeKey = $"wildlife.bycatch.{bycatchSpecies}";
00443:                 string text = $"Secondary quarry entangled at trap site {siteId}: {bycatchSpecies} (primary catch: {primarySpecies}, trap: {trapId}).";
00444:                 _journal.TryDiscoverRawKnowledge(knowledgeKey, text, null, _simDay);
00445:             };
00446:
00447:             if (_wildlifeTrappingPanel != null && _wildlifeTrappingPanel.IsInsideTree())
00448:                 RemoveChild(_wildlifeTrappingPanel);
00449:             _wildlifeTrappingPanel = new WildlifeTrappingPanel();
00450:             _wildlifeTrappingPanel.Bind(_wildlifeTrapping);
00451:             _wildlifeTrappingPanel.Visible = false;
00452:             AddChild(_wildlifeTrappingPanel);
00453:         }
00454:
00455:         private void SaveWildlifeTrapping()
00456:         {
00457:             if (_wildlifeTrapping != null)
00458:                 CaptureSection("wildlife_trapping", WildlifeTrappingSaveStore.TryCapturePersisted(_wildlifeTrapping.System.CaptureState()));
00459:         }
00460:
00461:         private void SetupExcavation()
00462:         {
00463:             if (_excavation != null) return;
00464:             SetupCampaignDay();
00465:             var exState = ExcavationSaveStore.TryLoad() ?? new ExcavationState();
00466:             var exSys = new ExcavationSystem(_campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 2), new GodotLog());
00467:             exSys.RestoreState(exState);
00468:             _excavation = new ExcavationHostSession(exSys);
00469:             if (_excavationPanel != null && _excavationPanel.IsInsideTree())
00470:                 RemoveChild(_excavationPanel);
00471:             _excavationPanel = new ExcavationPanel();
00472:             _excavationPanel.Bind(_excavation);
00473:             _excavationPanel.Visible = false;
00474:             AddChild(_excavationPanel);
00475:         }
00476:
00477:         private void SaveExcavation()
00478:         {
00479:             if (_excavation != null)
00480:                 CaptureSection("excavation", ExcavationSaveStore.TryCapturePersisted(_excavation.System.CaptureState()));
00481:         }
00482:
00483:         private void SetupApprenticeship()
00484:         {
00485:             if (_apprenticeship != null) return;
00486:             SetupDutyRoster();
00487:             _expandedShelterRoster = _dutyRoster.Roster;
00488:             SetupCampaignDay();
00489:             var appState = ApprenticeshipSaveStore.TryLoad() ?? new ApprenticeshipState();
00490:             var appSkills = EnsureSharedSkillProgression();
00491:             if (appState.skillProgression != null)
00492:                 appSkills.RestoreState(appState.skillProgression);
00493:             var appSys = new ApprenticeshipSystem(_campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Social, 0, 3), appSkills, _expandedShelterRoster, _survivorRelationsCore, new GodotLog());
00494:             appSys.RestoreState(appState);
00495:             appSys.IsApprenticeEligible = id =>
00496:             {
00497:                 SetupDoseLedger();
00498:                 if (_doseLedger?.Cohort == null) return true;
00499:                 if (_doseLedger.Cohort.GetChild(id) == null) return true;
00500:                 int currentDay = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
00501:                 return _doseLedger.Cohort.IsSchoolEligible(id, currentDay);
00502:             };
00503:             _apprenticeship = new ApprenticeshipHostSession(appSys);
00504:             if (_apprenticeshipPanel != null && _apprenticeshipPanel.IsInsideTree())
00505:                 RemoveChild(_apprenticeshipPanel);
00506:             _apprenticeshipPanel = new ApprenticeshipPanel();
00507:             _apprenticeshipPanel.Bind(_apprenticeship);
00508:             _apprenticeshipPanel.Visible = false;
00509:             AddChild(_apprenticeshipPanel);
00510:         }
00511:
00512:         private void SaveApprenticeship()
00513:         {
00514:             if (_apprenticeship != null)
00515:             {
00516:                 var state = _apprenticeship.System.CaptureState();
00517:                 state.skillProgression = EnsureSharedSkillProgression().CaptureState();
00518:                 CaptureSection("apprenticeship", ApprenticeshipSaveStore.TryCapturePersisted(state));
00519:             }
00520:         }
00521:
00522:         private void SetupCaregiving()
00523:         {
00524:             if (_caregiving != null) return;
00525:             SetupDutyRoster();
00526:             _expandedShelterRoster = _dutyRoster.Roster;
00527:             var cgState = CaregivingSaveStore.TryLoad() ?? new CaregivingSaveState();
00528:             var cgSys = new CaregivingSystem();
00529:             cgSys.RestoreState(cgState);
00530:             cgSys.IsAlive = id => _survivors?.Needs.Get(id)?.IsAliveState == true;
00531:             cgSys.CanProvideCare = id =>
00532:             {
00533:                 if (_survivors?.Needs.Get(id)?.IsAliveState != true) return false;
00534:                 return EvaluateSurvivorFitness(id).Level != FitnessLevel.Incapacitated;
00535:             };
00536:             cgSys.NeedsCare = id =>
00537:             {
00538:                 var needs = _survivors?.Needs.Get(id);
00539:                 if (needs == null || !needs.IsAliveState) return false;
00540:                 if (needs.Health < 75f) return true;
00541:                 if (_medicalWard?.GetActiveAdmission(id) != null) return true;
00542:                 if (_disease?.Engine != null && _disease.Catalog?.All != null)
00543:                 {
00544:                     var diseases = _disease.Catalog.All;
00545:                     for (int i = 0; i < diseases.Count; i++)
00546:                     {
00547:                         var disease = diseases[i];
00548:                         if (disease != null && !string.IsNullOrEmpty(disease.id)
00549:                             && _disease.Engine.IsInfected(id, disease.id)) return true;
00550:                     }
00551:                 }
00552:                 return _survivors?.RadStateFor(id)?.HasAcuteRadiationSickness == true;
00553:             };
00554:             cgSys.AdjustAffinity = (caregiverId, patientId, delta) =>
00555:                 _survivorRelationsCore?.ModifyAffinity(caregiverId, patientId, delta);
00556:             cgSys.ApplyFatigueDelta = (id, delta) =>
00557:                 _survivors?.Needs.ApplyAttributedDelta(
00558:                     id, NeedKind.Fatigue, delta, "caregiving.fatigue");
00559:             cgSys.ApplyHealthRecoveryBonus = (id, amount) =>
00560:                 _survivors?.Needs.ApplyAttributedDelta(
00561:                     id, NeedKind.Health, amount, "caregiving.recovery");
00562:             cgSys.OnCaregivingStarted += (caregiverId, _) =>
00563:             {
00564:                 // Care is labor. Vacate the caregiver's existing duty before
00565:                 // the next shift so one survivor cannot silently cover two
00566:                 // incompatible assignments.
00567:                 if (_dutyRoster == null)
00568:                     SetupDutyRoster();
00569:                 var rosterHost = _dutyRoster;
00570:                 string role = rosterHost?.Roster?.GetRoleOf(caregiverId);
00571:                 if (!string.IsNullOrEmpty(role))
00572:                     rosterHost!.Roster.Assign(role, string.Empty);
00573:             };
00574:             _caregiving = new CaregivingHostSession(cgSys);
00575:             if (_caregivingPanel != null && _caregivingPanel.IsInsideTree())
00576:                 RemoveChild(_caregivingPanel);
00577:             _caregivingPanel = new CaregivingPanel();
00578:             _caregivingPanel.Bind(_caregiving);
00579:             _caregivingPanel.Visible = false;
00580:             AddChild(_caregivingPanel);
00581:         }
00582:
00583:         private void SaveCaregiving()
00584:         {
00585:             if (_caregiving != null)
00586:                 CaptureSection("caregiving", CaregivingSaveStore.TryCapturePersisted(_caregiving.System.CaptureState()));
00587:         }
00588:
00589:         private sealed class TrappingJournalAuthor : Ashfall.Core.Journal.ISurvivorAuthor
00590:         {
00591:             public string Id { get; }
00592:             public string DisplayName { get; }
00593:             public Ashfall.Core.Journal.RiskBiasTrait RiskBias => Ashfall.Core.Journal.RiskBiasTrait.Realist;
00594:
00595:             public TrappingJournalAuthor(string id, string displayName)
00596:             {
00597:                 Id = id;
00598:                 DisplayName = displayName;
00599:             }
00600:         }
00601:     }
00602: }
```


# Appendix — Current Source Detail: `src/Main.Plans190_193.cs`

### `src/Main.Plans190_193.cs` — complete current file

- Size: 590 lines / 25776 bytes.
- SHA-256: `5dbed7ad680ddca7eeea7e542c17531d13f456a4e1d583e47cab527002a2963a`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // Main Partial : Plans 190-193 Host Wire & Orchestration
00004: // Subsystems   : Infection & Amputation, Railways & Armored Trains,
00005: //                Subterranean Fungi Cultivation, Wasteland Justice & Tribal Law
00006: // ============================================================================
00007: using System;
00008: using System.Linq;
00009: using System.Collections.Generic;
00010: using Godot;
00011: using Ashfall.Core;
00012: using Ashfall.Core.Medical;
00013: using Ashfall.Core.Expeditions;
00014: using Ashfall.Core.Archaeology;
00015: using Ashfall.Core.Farming;
00016: using Ashfall.Core.Narrative;
00017:
00018: namespace AtomicWar.GodotApp
00019: {
00020:     public partial class Main
00021:     {
00022:         private AmputationSystem? _amputation;
00023:         private RailwaySystem? _railway;
00024:         private FungiCultivationSystem? _fungi;
00025:         private JusticeSystem? _justice;
00026:
00027:         // ── Plan 190: Infection & Amputation Mechanics ───────────────────
00028:
00029:         public AmputationSystem EnsureAmputation()
00030:         {
00031:             if (_amputation != null) return _amputation;
00032:
00033:             var rng = _campaignDay != null ? _campaignDay.Rng.Fork("amputation") : new SeededRng(190);
00034:             var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();
00035:             var needs = _survivors?.Needs;
00036:
00037:             _amputation = new AmputationSystem(rng, inv, needs, new GodotLog());
00038:
00039:             string catalogPath = CatalogPath.ResolveCatalog("surgical_procedures.json");
00040:             var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
00041:             if (_catalogIo.FileExists(catalogPath))
00042:             {
00043:                 string json = _catalogIo.ReadAllText(catalogPath);
00044:                 {
00045:                     try
00046:                     {
00047:                         var catalog = System.Text.Json.JsonSerializer.Deserialize<SurgicalProcedureCatalog>(json);
00048:                         if (catalog?.procedures != null)
00049:                         {
00050:                             foreach (var p in catalog.procedures)
00051:                                 _amputation.RegisterProcedure(p);
00052:                         }
00053:                     }
00054:                     catch (Exception ex)
00055:                     {
00056:                         GD.PrintErr($"[Main.Amputation] Failed to parse {catalogPath}: {ex.Message}");
00057:                     }
00058:                 }
00059:             }
00060:
00061:             var saved = AmputationSaveStore.TryLoad();
00062:             if (saved != null)
00063:             {
00064:                 _amputation.RestoreState(saved);
00065:             }
00066:
00067:             _amputation.OnAmputationComplete += (survivorId, limb, condition) =>
00068:             {
00069:                 _journal?.TryAddRawEntry("amputation_performed", $"Emergency amputation performed on {survivorId}'s {limb} (State: {condition}).", null!, _simDay);
00070:             };
00071:
00072:             _amputation.OnGangreneDeclared += (survivorId, limb) =>
00073:             {
00074:                 _journal?.TryAddRawEntry("gangrene_warning", $"Critical medical emergency: {survivorId}'s {limb} wound has turned gangrenous!", null!, _simDay);
00075:             };
00076:
00077:             _amputation.OnProstheticFitted += (survivorId, prostheticId) =>
00078:             {
00079:                 _journal?.TryAddRawEntry("prosthetic_fitted", $"Prosthetic '{prostheticId}' fitted to {survivorId}.", null!, _simDay);
00080:             };
00081:
00082:             _amputation.OnPhantomPainEpisode += (survivorId, stressAmount) =>
00083:             {
00084:                 _journal?.TryAddRawEntry("phantom_pain_episode", $"{survivorId} experienced a severe phantom-pain episode.", null!, _simDay);
00085:             };
00086:
00087:             return _amputation;
00088:         }
00089:
00090:         private void SetupAmputation()
00091:         {
00092:             EnsureAmputation();
00093:         }
00094:
00095:         private void SaveAmputation()
00096:         {
00097:             if (_amputation != null)
00098:             {
00099:                 CaptureSection("amputation", AmputationSaveStore.TryCapturePersisted(_amputation.CaptureState()));
00100:             }
00101:         }
00102:
00103:         // ── Plan 191: Railways & Armored Trains ──────────────────────────
00104:
00105:         public RailwaySystem EnsureRailway()
00106:         {
00107:             if (_railway != null) return _railway;
00108:
00109:             var rng = _campaignDay != null ? _campaignDay.Rng.Fork("railway") : new SeededRng(191);
00110:             var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();
00111:
00112:             _railway = new RailwaySystem(rng, inv, new GodotLog());
00113:
00114:             string catalogPath = CatalogPath.ResolveCatalog("rail_network.json");
00115:             var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
00116:             if (_catalogIo.FileExists(catalogPath))
00117:             {
00118:                 string json = _catalogIo.ReadAllText(catalogPath);
00119:                 {
00120:                     try
00121:                     {
00122:                         var catalog = System.Text.Json.JsonSerializer.Deserialize<RailwayNetworkCatalog>(json);
00123:                         if (catalog != null)
00124:                         {
00125:                             _railway.RegisterCatalog(catalog);
00126:                         }
00127:                     }
00128:                     catch (Exception ex)
00129:                     {
00130:                         GD.PrintErr($"[Main.Railway] Failed to parse {catalogPath}: {ex.Message}");
00131:                     }
00132:                 }
00133:             }
00134:
00135:             string logisticsPath = CatalogPath.ResolveCatalog("rail_logistics_catalog.json");
00136:             if (_catalogIo.FileExists(logisticsPath))
00137:             {
00138:                 string json = _catalogIo.ReadAllText(logisticsPath);
00139:                 {
00140:                     try
00141:                     {
00142:                         var container = System.Text.Json.JsonSerializer.Deserialize<RailLogisticsCatalogContainer>(json);
00143:                         if (container != null && container.edges != null)
00144:                         {
00145:                             _railway.RegisterLogisticsCatalog(container.edges);
00146:                         }
00147:                     }
00148:                     catch (Exception ex)
00149:                     {
00150:                         GD.PrintErr($"[Main.Railway] Failed to parse {logisticsPath}: {ex.Message}");
00151:                     }
00152:                 }
00153:             }
00154:
00155:             var saved = RailwaySaveStore.TryLoad();
00156:             if (saved != null)
00157:             {
00158:                 _railway.RestoreState(saved);
00159:             }
00160:
00161:             _railway.OnTrainDispatched += (trainId, segmentId) =>
00162:             {
00163:                 _journal?.TryAddRawEntry("train_dispatched", $"Armored train {trainId} departed onto rail segment {segmentId}.", null!, _simDay);
00164:                 RecordRailRunFromTrain(_railway, trainId, segmentId);
00165:             };
00166:
00167:             _railway.OnDerailment += (trainId, segmentId) =>
00168:             {
00169:                 _journal?.TryAddRawEntry("train_derailment", $"Disaster! Train {trainId} derailed on degraded rail segment {segmentId}!", null!, _simDay);
00170:             };
00171:
00172:             _railway.OnTrainAmbushed += (trainId, segmentId) =>
00173:             {
00174:                 _journal?.TryAddRawEntry("train_ambush", $"Train {trainId} came under heavy raider fire on segment {segmentId}!", null!, _simDay);
00175:             };
00176:
00177:             return _railway;
00178:         }
00179:
00180:         private void SetupRailway()
00181:         {
00182:             EnsureRailway();
00183:         }
00184:
00185:         private void SaveRailway()
00186:         {
00187:             if (_railway != null)
00188:             {
00189:                 CaptureSection("railway", RailwaySaveStore.TryCapturePersisted(_railway.CaptureState()));
00190:             }
00191:         }
00192:
00193:         // ── Plan 192: Subterranean Fungi Cultivation ─────────────────────
00194:
00195:         public FungiCultivationSystem EnsureFungi()
00196:         {
00197:             if (_fungi != null) return _fungi;
00198:
00199:             var rng = _campaignDay != null ? _campaignDay.Rng.Fork("fungi") : new SeededRng(192);
00200:             var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();
00201:
00202:             _fungi = new FungiCultivationSystem(rng, inv, new GodotLog());
00203:
00204:             string catalogPath = CatalogPath.ResolveCatalog("underground_flora.json");
00205:             var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
00206:             if (_catalogIo.FileExists(catalogPath))
00207:             {
00208:                 string json = _catalogIo.ReadAllText(catalogPath);
00209:                 {
00210:                     try
00211:                     {
00212:                         var catalog = System.Text.Json.JsonSerializer.Deserialize<UndergroundFloraCatalog>(json);
00213:                         if (catalog != null)
00214:                         {
00215:                             _fungi.RegisterCatalog(catalog);
00216:                         }
00217:                     }
00218:                     catch (Exception ex)
00219:                     {
00220:                         GD.PrintErr($"[Main.Fungi] Failed to parse {catalogPath}: {ex.Message}");
00221:                     }
00222:                 }
00223:             }
00224:
00225:             var saved = FungiSaveStore.TryLoad();
00226:             if (saved != null)
00227:             {
00228:                 _fungi.RestoreState(saved);
00229:             }
00230:
00231:             _fungi.OnToxicBloom += (plotId, roomId) =>
00232:             {
00233:                 _journal?.TryAddRawEntry("fungi_toxic_bloom", $"Toxic mold outbreak detected at plot {plotId} in {roomId}!", null!, _simDay);
00234:             };
00235:
00236:             _fungi.OnFungiHarvested += (plotId, strain, count) =>
00237:             {
00238:                 _journal?.TryAddRawEntry("fungi_harvest", $"Harvested {count} units of {strain} from subterranean bed {plotId}.", null!, _simDay);
00239:             };
00240:
00241:             _fungi.OnSubstratePrepared += (plotId, preparation, usedHeat) =>
00242:             {
00243:                 _journal?.TryAddRawEntry("fungi_substrate_prepared", $"Bed {plotId} substrate prepared ({preparation}{(usedHeat ? ", heated" : "")}).", null!, _simDay);
00244:             };
00245:
00246:             _fungi.OnSubstrateDisposed += (plotId, method) =>
00247:             {
00248:                 _journal?.TryAddRawEntry("fungi_substrate_disposed", $"Contaminated substrate from bed {plotId} disposed ({method}).", null!, _simDay);
00249:             };
00250:
00251:             _fungi.OnContaminationSpread += (sourcePlotId, roomId) =>
00252:             {
00253:                 _journal?.TryAddRawEntry("fungi_contamination_spread", $"Mold contamination is spreading from bed {sourcePlotId} to neighbouring beds in {roomId}.", null!, _simDay);
00254:             };
00255:
00256:             return _fungi;
00257:         }
00258:
00259:         private void SetupFungi()
00260:         {
00261:             EnsureFungi();
00262:         }
00263:
00264:         private void SaveFungi()
00265:         {
00266:             if (_fungi != null)
00267:             {
00268:                 CaptureSection("fungi_cultivation", FungiSaveStore.TryCapturePersisted(_fungi.CaptureState()));
00269:             }
00270:         }
00271:
00272:         // ── Plan 193: Wasteland Justice & Tribal Law ─────────────────────
00273:
00274:         public JusticeSystem EnsureJustice()
00275:         {
00276:             if (_justice != null) return _justice;
00277:
00278:             var rng = _campaignDay != null ? _campaignDay.Rng.Fork("justice") : new SeededRng(193);
00279:             var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();
00280:             var needs = _survivors?.Needs;
00281:
00282:             _justice = new JusticeSystem(rng, inv, needs, _politics, new GodotLog());
00283:
00284:             string catalogPath = CatalogPath.ResolveCatalog("wasteland_laws.json");
00285:             var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
00286:             if (_catalogIo.FileExists(catalogPath))
00287:             {
00288:                 string json = _catalogIo.ReadAllText(catalogPath);
00289:                 {
00290:                     try
00291:                     {
00292:                         var catalog = System.Text.Json.JsonSerializer.Deserialize<WastelandLawsCatalog>(json);
00293:                         if (catalog?.laws != null)
00294:                         {
00295:                             foreach (var l in catalog.laws)
00296:                                 _justice.RegisterLaw(l);
00297:                         }
00298:                     }
00299:                     catch (Exception ex)
00300:                     {
00301:                         GD.PrintErr($"[Main.Justice] Failed to parse {catalogPath}: {ex.Message}");
00302:                     }
00303:                 }
00304:             }
00305:
00306:             var saved = JusticeSaveStore.TryLoad();
00307:             if (saved != null)
00308:             {
00309:                 _justice.RestoreState(saved);
00310:             }
00311:
00312:             _justice.OnTrialConcluded += (incidentId, verdict, punishment) =>
00313:             {
00314:                 _journal?.TryAddRawEntry("trial_concluded", $"Tribal tribunal reached verdict for {incidentId}: {verdict} (Sentence: {punishment}).", null!, _simDay);
00315:             };
00316:
00317:             _justice.OnBanishment += (survivorId, incidentId) =>
00318:             {
00319:                 _journal?.TryAddRawEntry("survivor_banished", $"{survivorId} was formally banished from the shelter following tribunal proceedings.", null!, _simDay);
00320:             };
00321:
00322:             _justice.OnExecution += (survivorId, incidentId) =>
00323:             {
00324:                 _journal?.TryAddRawEntry("execution_carried_out", $"Capital punishment carried out on {survivorId}.", null!, _simDay);
00325:             };
00326:
00327:             _justice.OnVigilanteOutbreak += (incidentId, accusedId) =>
00328:             {
00329:                 _journal?.TryAddRawEntry("vigilante_mob", $"Shelter unrest boiled over! Vigilante mob enacted street justice on {accusedId}.", null!, _simDay);
00330:             };
00331:
00332:             return _justice;
00333:         }
00334:
00335:         private void SetupJustice()
00336:         {
00337:             EnsureJustice();
00338:         }
00339:
00340:         private void SaveJustice()
00341:         {
00342:             if (_justice != null)
00343:             {
00344:                 CaptureSection("wasteland_justice", JusticeSaveStore.TryCapturePersisted(_justice.CaptureState()));
00345:             }
00346:         }
00347:
00348:         // ── Daily Tick Orchestration for Plans 190-193 ───────────────────
00349:
00350:         private void TickPlans190_193(int currentDay)
00351:         {
00352:             _amputation?.TickDay(currentDay);
00353:             _railway?.TickDay(currentDay);
00354:
00355:             // Plan 204: project real per-room thermal state into the fungi beds when the
00356:             // shelter thermal authority is live; otherwise the Core default band applies.
00357:             if (_fungi != null)
00358:             {
00359:                 var thermalRooms = _shelterThermal?.System.State.rooms;
00360:                 if (thermalRooms != null && thermalRooms.Count > 0)
00361:                 {
00362:                     var temps = new Dictionary<string, float>(StringComparer.Ordinal);
00363:                     for (int i = 0; i < thermalRooms.Count; i++)
00364:                         temps[thermalRooms[i].roomId] = thermalRooms[i].currentTempC;
00365:                     _fungi.TickDay(currentDay, roomTemperatureOverride: roomId =>
00366:                         temps.TryGetValue(roomId, out var t) ? t : 15f);
00367:                 }
00368:                 else
00369:                 {
00370:                     _fungi.TickDay(currentDay);
00371:                 }
00372:             }
00373:
00374:             _justice?.TickDay(currentDay);
00375:         }
00376:
00377:         // ── UI-07 closeout: amputation / tribunal / railway / archaeology consoles ──
00378:
00379:         private void HandleAmputationAction(string action, string param = "")
00380:         {
00381:             if (action == "CLOSE") { CloseAmputationTriagePanel(); return; }
00382:             if (_amputationTriagePanel == null || _amputation == null) return;
00383:
00384:             var parts = param.Split(':');
00385:             switch (action)
00386:             {
00387:                 case "amputate":
00388:                 {
00389:                     if (parts.Length != 2 || !System.Enum.TryParse<LimbId>(parts[1], out var limb)) break;
00390:                     string procedureId = (limb == LimbId.LeftArm || limb == LimbId.RightArm)
00391:                         ? "procedure_amputation_arm_field" : "procedure_amputation_leg_field";
00392:                     var result = _amputation.PerformAmputation(parts[0], limb, procedureId);
00393:                     _amputationTriagePanel.ShowFeedback(
00394:                         result.Success
00395:                             ? (result.SurvivorDied
00396:                                 ? "The procedure was done, but the patient did not survive the shock."
00397:                                 : "Amputation complete. Recovery will be long; phantom pain is possible.")
00398:                             : "The surgery could not proceed — check tools and supplies.",
00399:                         !result.Success || result.SurvivorDied);
00400:                     break;
00401:                 }
00402:                 case "treat":
00403:                 {
00404:                     if (parts.Length != 2 || !System.Enum.TryParse<LimbId>(parts[1], out var limb2)) break;
00405:                     _amputation.TreatWound(parts[0], limb2, cleaningEfficacy: 0.5f);
00406:                     _amputationTriagePanel.ShowFeedback("Wound cleaned and dressed. Infection risk is reduced.", false);
00407:                     break;
00408:                 }
00409:                 case "prosthetic":
00410:                 {
00411:                     if (parts.Length != 2 || !System.Enum.TryParse<LimbId>(parts[1], out var limb3)) break;
00412:                     string prostheticItem = (limb3 == LimbId.LeftArm || limb3 == LimbId.RightArm)
00413:                         ? "prosthetic_wooden_arm" : "prosthetic_wooden_leg";
00414:                     var res = _amputation.FitProsthetic(parts[0], limb3, prostheticItem);
00415:                     _survivorDowntimePanel?.RefreshView();
00416:                     _amputationTriagePanel.ShowFeedback(
00417:                         res.IsSuccess ? "Prosthetic fitted. Some function returns — never all of it."
00418:                                       : "Fitting failed — the limb or the workshop isn't ready.",
00419:                         !res.IsSuccess);
00420:                     break;
00421:                 }
00422:             }
00423:             _amputationTriagePanel.RefreshView();
00424:         }
00425:
00426:         private void HandleJusticeAction(string action, string param = "")
00427:         {
00428:             if (action == "CLOSE") { CloseJusticeTribunalPanel(); return; }
00429:             if (_justiceTribunalPanel == null || _justice == null) return;
00430:
00431:             switch (action)
00432:             {
00433:                 case "report":
00434:                 {
00435:                     var parts = param.Split(':');
00436:                     if (parts.Length != 2 || string.IsNullOrEmpty(parts[0])) break;
00437:                     if (!System.Enum.TryParse<CrimeType>(parts[1], out var crime)) break;
00438:                     string incidentId = $"inc_{parts[0]}_{parts[1]}_{_simDay}";
00439:                     var inc = _justice.ReportCrime(incidentId, crime, parts[0], victimId: null, _simDay);
00440:                     _justiceTribunalPanel.ShowFeedback(
00441:                         inc != null ? $"Report filed. The case joins the docket for the tribunal's day."
00442:                                     : "The report could not be filed.",
00443:                         inc == null);
00444:                     break;
00445:                 }
00446:             }
00447:             _justiceTribunalPanel.RefreshView();
00448:         }
00449:
00450:         private void HandleRailwayAction(string action, string param = "")
00451:         {
00452:             if (action == "CLOSE") { CloseRailwayTerminalPanel(); return; }
00453:             if (_railwayTerminalPanel == null || _railway == null) return;
00454:
00455:             Ashfall.Core.ActionResult? res = action switch
00456:             {
00457:                 "repair_track" => _railway.RepairTrack(param, integrityRestored: 0.25f),
00458:                 "repair_bridge" => _railway.RepairBridge(param),
00459:                 "clear_obstacle" => _railway.ClearTrackObstacle(param),
00460:                 "clear_derailment" => _railway.ClearDerailment(param),
00461:                 "service" => _railway.ServiceTransmission(param),
00462:                 _ => null
00463:             };
00464:
00465:             if (res != null)
00466:                 _railwayTerminalPanel.ShowFeedback(
00467:                     res.Value.IsSuccess ? "Done. The line is one step closer to running."
00468:                                   : "The crew couldn't do it — check what the terminal says is missing.",
00469:                     !res.Value.IsSuccess);
00470:             _railwayTerminalPanel.RefreshView();
00471:         }
00472:
00473:         private void HandleArchaeologyAction(string action, string param = "")
00474:         {
00475:             if (action == "CLOSE") { CloseArchaeologyExcavationPanel(); return; }
00476:             if (_archaeologyExcavationPanel == null || _archaeology == null) return;
00477:
00478:             switch (action)
00479:             {
00480:                 case "decrypt":
00481:                 {
00482:                     var res = _archaeology.ProgressDecryption(param, hours: 8f, engineerSkill: 0.5f, hasPower: true);
00483:                     _archaeologyExcavationPanel.ShowFeedback(
00484:                         res.IsSuccess ? "The decryption shift worked through the cipher layer."
00485:                                       : "The shift made no headway — higher tiers need power or a keycard.",
00486:                         !res.IsSuccess);
00487:                     break;
00488:                 }
00489:                 case "sell":
00490:                 {
00491:                     var res = _archaeology.SellArchiveToBroker(param);
00492:                     _archaeologyExcavationPanel.ShowFeedback(
00493:                         res.IsSuccess ? "The broker took the archive and paid in kind."
00494:                                       : "The broker refused the archive.",
00495:                         !res.IsSuccess);
00496:                     break;
00497:                 }
00498:             }
00499:             _archaeologyExcavationPanel.RefreshView();
00500:         }
00501:
00502:         private void CloseAmputationTriagePanel() { _amputationTriagePanel?.Visible = false; }
00503:         private void CloseJusticeTribunalPanel() { _justiceTribunalPanel?.Visible = false; }
00504:         private void CloseRailwayTerminalPanel() { _railwayTerminalPanel?.Visible = false; }
00505:         private void CloseArchaeologyExcavationPanel() { _archaeologyExcavationPanel?.Visible = false; }
00506:         private void CloseDesperationCrisisPanel() { _desperationCrisisPanel?.Visible = false; }
00507:         private void CloseMercenaryBountyBoardPanel() { _mercenaryBountyBoardPanel?.Visible = false; }
00508:
00509:         // ── Plans 186/187: desperation + fallout console commands ──────────
00510:
00511:         private void HandleDesperationAction(string action, string param = "")
00512:         {
00513:             if (action == "CLOSE") { CloseDesperationCrisisPanel(); return; }
00514:             if (_desperationCrisisPanel == null || _desperation == null) return;
00515:
00516:             switch (action)
00517:             {
00518:                 case "harvest_corpse":
00519:                 {
00520:                     // Actor resolves from the canonical roster authority.
00521:                     string actorId = _survivors?.RosterState.FirstOrDefault(s => !string.IsNullOrEmpty(s.Id))?.Id ?? string.Empty;
00522:                     var res = _desperation.HarvestCorpse(actorId, param, "desperation_consume_corpse", _simDay);
00523:                     _desperationCrisisPanel.ShowFeedback(
00524:                         res.IsSuccess ? "The unthinkable is done. The shelter eats; nothing is the same."
00525:                                       : "The crisis threshold has not been reached — the act is not yet on the table.",
00526:                         !res.IsSuccess);
00527:                     break;
00528:                 }
00529:                 case "bury_corpse":
00530:                 {
00531:                     var res = _desperation.PerformBurial(param);
00532:                     _desperationCrisisPanel.ShowFeedback(
00533:                         res.IsSuccess ? "The dead are laid to rest with what dignity remains."
00534:                                       : "Nothing to bury.",
00535:                         !res.IsSuccess);
00536:                     break;
00537:                 }
00538:             }
00539:             _desperationCrisisPanel.RefreshView();
00540:         }
00541:
00542:         private void HandleFalloutAction(string action, string param = "")
00543:         {
00544:             if (action == "CLOSE") { _falloutPlumePanel.Visible = false; return; }
00545:             if (_falloutPlumePanel == null || _fallout == null) return;
00546:
00547:             switch (action)
00548:             {
00549:                 case "seal_shelter":
00550:                 {
00551:                     float hours = float.TryParse(param, out var h) ? h : 48f;
00552:                     bool sealedOk = _fallout.SealShelter(hours);
00553:                     _falloutPlumePanel.SetFeedback(
00554:                         sealedOk ? "Airlock seal engaged. Outside air stays outside for as long as the seal holds."
00555:                                  : "The seal could not be engaged.",
00556:                         !sealedOk);
00557:                     break;
00558:                 }
00559:             }
00560:             _falloutPlumePanel.RefreshView();
00561:         }
00562:
00563:         // ── Plan 188: mercenary bounty board commands ──────────────────────
00564:
00565:         private void HandleMercenaryAction(string action, string param = "")
00566:         {
00567:             if (action == "CLOSE") { CloseMercenaryBountyBoardPanel(); return; }
00568:             if (_mercenaryBountyBoardPanel == null || _mercenary == null) return;
00569:
00570:             Ashfall.Core.ActionResult? res = action switch
00571:             {
00572:                 // Claim requires the authored proof item — the canonical
00573:                 // inventory authority holds it; Core verifies and pays once.
00574:                 "claim" => (Ashfall.Core.ActionResult?)_mercenary.ClaimReward(param),
00575:                 "accept" => (Ashfall.Core.ActionResult?)_mercenary.AcceptContract(param, _simDay),
00576:                 _ => null
00577:             };
00578:
00579:             if (res != null)
00580:                 _mercenaryBountyBoardPanel.ShowFeedback(
00581:                     res.Value.IsSuccess
00582:                         ? (action == "claim"
00583:                             ? "Payout collected at the board. The ledger closes."
00584:                             : "Contract accepted. Proof of the deed is what gets paid — nothing else.")
00585:                         : "The board refused it — check proof, expiry and standing.",
00586:                     !res.Value.IsSuccess);
00587:             _mercenaryBountyBoardPanel.RefreshView();
00588:         }
00589:     }
00590: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/World/ExcavationSitesCatalogTests.cs`

### `Ashfall.Core.Tests/World/ExcavationSitesCatalogTests.cs` — complete current file

- Size: 287 lines / 12219 bytes.
- SHA-256: `27c7bd3e6cee1a9314331e730467200567fde2991faaec44428d34df55ae9057`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using System.Text.Json;
00007: using Ashfall.Core;
00008: using Ashfall.Core.Excavation;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests.World
00012: {
00013:     public class ExcavationSitesCatalogTests
00014:     {
00015:         private static string ResolveDataDir()
00016:         {
00017:             string baseDir = AppContext.BaseDirectory;
00018:             string probe = Path.Combine(baseDir, "StreamingAssets", "Data");
00019:             if (Directory.Exists(probe)) return probe;
00020:
00021:             probe = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
00022:             if (Directory.Exists(probe)) return Path.GetFullPath(probe);
00023:
00024:             probe = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
00025:             if (Directory.Exists(probe)) return Path.GetFullPath(probe);
00026:
00027:             return string.Empty;
00028:         }
00029:
00030:         [Fact]
00031:         public void ExcavationCatalog_LoadsAllEightAuthoredSites()
00032:         {
00033:             string dataDir = ResolveDataDir();
00034:             var sites = ExcavationCatalogLoader.Load(dataDir);
00035:
00036:             Assert.NotNull(sites);
00037:             Assert.Equal(8, sites.Count);
00038:
00039:             var expectedIds = new[]
00040:             {
00041:                 "excavation_command_vault",
00042:                 "excavation_utility_tunnels",
00043:                 "excavation_metro_interchange",
00044:                 "excavation_mine_shaft",
00045:                 "excavation_archive_bunker",
00046:                 "excavation_drainage_network",
00047:                 "excavation_storage_chamber",
00048:                 "excavation_civilian_shelter"
00049:             };
00050:
00051:             foreach (var id in expectedIds)
00052:             {
00053:                 var site = sites.FirstOrDefault(s => s.site_id == id);
00054:                 Assert.NotNull(site);
00055:                 Assert.False(string.IsNullOrWhiteSpace(site.display_name));
00056:                 Assert.False(string.IsNullOrWhiteSpace(site.description));
00057:                 Assert.True(site.max_depth_meters > 0f);
00058:                 Assert.True(site.required_progress > 0f);
00059:                 Assert.True(site.structural_risk > 0f && site.structural_risk <= 1.0f);
00060:             }
00061:         }
00062:
00063:         [Fact]
00064:         public void ExcavationCatalog_AllSiteIdsAreUnique()
00065:         {
00066:             string dataDir = ResolveDataDir();
00067:             var sites = ExcavationCatalogLoader.Load(dataDir);
00068:             var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00069:
00070:             foreach (var site in sites)
00071:             {
00072:                 Assert.True(set.Add(site.site_id), $"Duplicate site_id detected: {site.site_id}");
00073:             }
00074:         }
00075:
00076:         [Fact]
00077:         public void ExcavationCatalog_AllLocationReferencesResolveInLocationsCatalog()
00078:         {
00079:             string dataDir = ResolveDataDir();
00080:             var sites = ExcavationCatalogLoader.Load(dataDir);
00081:             string locationsPath = Path.Combine(dataDir, "locations.json");
00082:             Assert.True(File.Exists(locationsPath), "locations.json must exist");
00083:
00084:             string locationsJson = File.ReadAllText(locationsPath);
00085:             using var doc = JsonDocument.Parse(locationsJson);
00086:             var locationIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00087:
00088:             if (doc.RootElement.TryGetProperty("locations", out var locArray))
00089:             {
00090:                 foreach (var loc in locArray.EnumerateArray())
00091:                 {
00092:                     if (loc.TryGetProperty("id", out var idProp))
00093:                         locationIds.Add(idProp.GetString() ?? string.Empty);
00094:                 }
00095:             }
00096:
00097:             foreach (var site in sites)
00098:             {
00099:                 Assert.False(string.IsNullOrWhiteSpace(site.location_id));
00100:                 Assert.True(locationIds.Contains(site.location_id),
00101:                     $"Excavation site '{site.site_id}' references unknown location_id '{site.location_id}'");
00102:             }
00103:         }
00104:
00105:         [Fact]
00106:         public void ExcavationCatalog_DepthBandsAreOrderedAndWithinRange()
00107:         {
00108:             string dataDir = ResolveDataDir();
00109:             var sites = ExcavationCatalogLoader.Load(dataDir);
00110:
00111:             foreach (var site in sites)
00112:             {
00113:                 Assert.NotNull(site.depth_bands);
00114:                 Assert.InRange(site.depth_bands.Count, 3, 6);
00115:
00116:                 float prevDepth = 0f;
00117:                 foreach (var band in site.depth_bands)
00118:                 {
00119:                     Assert.False(string.IsNullOrWhiteSpace(band.label));
00120:                     Assert.True(band.depth_meters > prevDepth,
00121:                         $"Depth band '{band.label}' on site '{site.site_id}' is not monotonically increasing.");
00122:                     Assert.True(band.depth_meters <= site.max_depth_meters + 0.1f,
00123:                         $"Depth band '{band.label}' exceeds max_depth_meters on site '{site.site_id}'");
00124:                     Assert.InRange(band.risk, 0.05f, 1.0f);
00125:                     prevDepth = band.depth_meters;
00126:                 }
00127:             }
00128:         }
00129:
00130:         [Fact]
00131:         public void ExcavationCatalog_DepthBandsSpanShallowMediumDeepTiers()
00132:         {
00133:             string dataDir = ResolveDataDir();
00134:             var sites = ExcavationCatalogLoader.Load(dataDir);
00135:
00136:             var shallowSites = sites.Where(s => s.max_depth_meters <= 70f).ToList();
00137:             var mediumSites = sites.Where(s => s.max_depth_meters > 70f && s.max_depth_meters <= 110f).ToList();
00138:             var deepSites = sites.Where(s => s.max_depth_meters > 110f).ToList();
00139:
00140:             Assert.True(shallowSites.Count >= 3, $"Expected at least 3 shallow sites, got {shallowSites.Count}");
00141:             Assert.True(mediumSites.Count >= 2, $"Expected at least 2 medium sites, got {mediumSites.Count}");
00142:             Assert.True(deepSites.Count >= 2, $"Expected at least 2 deep sites, got {deepSites.Count}");
00143:         }
00144:
00145:         [Fact]
00146:         public void ExcavationCatalog_HazardCoverageMatchesDesignContract()
00147:         {
00148:             string dataDir = ResolveDataDir();
00149:             var sites = ExcavationCatalogLoader.Load(dataDir);
00150:
00151:             // Spore mold required on at least 2 sites (Metro Interchange, Archive Bunker)
00152:             var moldSites = sites.Where(s => s.hazard_type == "hazard_spore_mold").ToList();
00153:             Assert.True(moldSites.Count >= 2, $"Expected at least 2 spore mold sites, got {moldSites.Count}");
00154:             Assert.Contains(moldSites, s => s.site_id == "excavation_metro_interchange");
00155:             Assert.Contains(moldSites, s => s.site_id == "excavation_archive_bunker");
00156:
00157:             // Flood hazard on Utility Tunnels and Drainage Network
00158:             var floodSites = sites.Where(s => s.hazard_type == "hazard_flood").ToList();
00159:             Assert.True(floodSites.Count >= 2, $"Expected at least 2 flood sites, got {floodSites.Count}");
00160:             Assert.Contains(floodSites, s => s.site_id == "excavation_utility_tunnels");
00161:             Assert.Contains(floodSites, s => s.site_id == "excavation_drainage_network");
00162:
00163:             // Gas/Methane on Mine Shaft
00164:             Assert.Contains(sites, s => s.site_id == "excavation_mine_shaft" && s.hazard_type == "hazard_methane_pocket");
00165:
00166:             // Radiation hotspot on Command Vault
00167:             Assert.Contains(sites, s => s.site_id == "excavation_command_vault" && s.hazard_type == "hazard_radiation_hotspot");
00168:         }
00169:
00170:         [Fact]
00171:         public void ExcavationCatalog_RelicRewardLinksAreValid()
00172:         {
00173:             string dataDir = ResolveDataDir();
00174:             var sites = ExcavationCatalogLoader.Load(dataDir);
00175:
00176:             // 3 primary relic sources: Command Vault, Archive Bunker, Storage Chamber
00177:             var relicSites = new[] { "excavation_command_vault", "excavation_archive_bunker", "excavation_storage_chamber" };
00178:             foreach (var id in relicSites)
00179:             {
00180:                 var site = sites.FirstOrDefault(s => s.site_id == id);
00181:                 Assert.NotNull(site);
00182:                 Assert.False(string.IsNullOrWhiteSpace(site.relic_reward_id));
00183:             }
00184:         }
00185:
00186:         [Fact]
00187:         public void ExcavationCatalog_DefaultSitesFallbackMatchesEightSites()
00188:         {
00189:             var defaultSites = ExcavationCatalogLoader.GetDefaultSites();
00190:             Assert.Equal(8, defaultSites.Count);
00191:
00192:             var siteIds = defaultSites.Select(s => s.site_id).ToHashSet();
00193:             Assert.Contains("excavation_command_vault", siteIds);
00194:             Assert.Contains("excavation_utility_tunnels", siteIds);
00195:             Assert.Contains("excavation_metro_interchange", siteIds);
00196:             Assert.Contains("excavation_mine_shaft", siteIds);
00197:             Assert.Contains("excavation_archive_bunker", siteIds);
00198:             Assert.Contains("excavation_drainage_network", siteIds);
00199:             Assert.Contains("excavation_storage_chamber", siteIds);
00200:             Assert.Contains("excavation_civilian_shelter", siteIds);
00201:         }
00202:
00203:         [Fact]
00204:         public void ExcavationSystem_MultiSiteParallelExcavation_AndDeterministicProgress()
00205:         {
00206:             string dataDir = ResolveDataDir();
00207:             var defs = ExcavationCatalogLoader.Load(dataDir);
00208:
00209:             var rng1 = new SeededRng(2026);
00210:             var sys1 = new ExcavationSystem(rng1);
00211:
00212:             var rng2 = new SeededRng(2026);
00213:             var sys2 = new ExcavationSystem(rng2);
00214:
00215:             foreach (var def in defs)
00216:             {
00217:                 sys1.AddSite(def.site_id, "room_" + def.site_id, def.required_progress, def.structuralRisk());
00218:                 sys2.AddSite(def.site_id, "room_" + def.site_id, def.required_progress, def.structuralRisk());
00219:             }
00220:
00221:             Assert.Equal(8, sys1.State.sites.Count);
00222:             Assert.Equal(8, sys2.State.sites.Count);
00223:
00224:             // Assign workers to multiple sites
00225:             sys1.AssignWorkers("excavation_utility_tunnels", 3);
00226:             sys1.AssignWorkers("excavation_drainage_network", 2);
00227:             sys1.ApplyShoring("excavation_utility_tunnels");
00228:
00229:             sys2.AssignWorkers("excavation_utility_tunnels", 3);
00230:             sys2.AssignWorkers("excavation_drainage_network", 2);
00231:             sys2.ApplyShoring("excavation_utility_tunnels");
00232:
00233:             for (int day = 0; day < 5; day++)
00234:             {
00235:                 sys1.TickDay();
00236:                 sys2.TickDay();
00237:             }
00238:
00239:             for (int i = 0; i < 8; i++)
00240:             {
00241:                 Assert.Equal(sys1.State.sites[i].progress, sys2.State.sites[i].progress);
00242:                 Assert.Equal(sys1.State.sites[i].hasCavedIn, sys2.State.sites[i].hasCavedIn);
00243:                 Assert.Equal(sys1.State.sites[i].isComplete, sys2.State.sites[i].isComplete);
00244:             }
00245:         }
00246:
00247:         [Fact]
00248:         public void ExcavationSystem_SaveAndRestore_MaintainsAllEightSitesState()
00249:         {
00250:             string dataDir = ResolveDataDir();
00251:             var defs = ExcavationCatalogLoader.Load(dataDir);
00252:
00253:             var sys1 = new ExcavationSystem(new SeededRng(100));
00254:             foreach (var def in defs)
00255:             {
00256:                 sys1.AddSite(def.site_id, "room_" + def.site_id, def.required_progress, def.structural_risk);
00257:             }
00258:
00259:             sys1.AssignWorkers("excavation_command_vault", 4);
00260:             sys1.AssignWorkers("excavation_civilian_shelter", 2);
00261:             sys1.ApplyShoring("excavation_command_vault");
00262:             sys1.TickDay();
00263:
00264:             var saved = sys1.CaptureState();
00265:             Assert.Equal(8, saved.sites.Count);
00266:
00267:             var sys2 = new ExcavationSystem(new SeededRng(200));
00268:             sys2.RestoreState(saved);
00269:
00270:             Assert.Equal(8, sys2.State.sites.Count);
00271:             var vault2 = sys2.State.sites.First(s => s.siteId == "excavation_command_vault");
00272:             Assert.True(vault2.shoringApplied);
00273:             Assert.Equal(4, vault2.assignedWorkerCount);
00274:             Assert.True(vault2.progress > 0f);
00275:
00276:             var shelter2 = sys2.State.sites.First(s => s.siteId == "excavation_civilian_shelter");
00277:             Assert.False(shelter2.shoringApplied);
00278:             Assert.Equal(2, shelter2.assignedWorkerCount);
00279:             Assert.True(shelter2.progress > 0f);
00280:         }
00281:     }
00282:
00283:     internal static class ExcavationSiteDefExtensions
00284:     {
00285:         public static float structuralRisk(this ExcavationSiteDef def) => def.structural_risk;
00286:     }
00287: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/World/ExcavationHazardSystemTests.cs`

### `Ashfall.Core.Tests/World/ExcavationHazardSystemTests.cs` — complete current file

- Size: 365 lines / 14520 bytes.
- SHA-256: `2fe1430cc1dd0d50f66a935cc2473b3bd3622c78a8adf037ac3e4534d12a340e`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Excavation;
00007: using Ashfall.Core.Inventory;
00008: using Ashfall.Core.IO;
00009: using Ashfall.Core.Shelter;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests.World
00013: {
00014:     public class ExcavationHazardSystemTests
00015:     {
00016:         private static string GetExcavationCatalogJson()
00017:         {
00018:             string path = Path.Combine(AppContext.BaseDirectory, "Assets/StreamingAssets/Data/excavation_hazard_mitigation.json");
00019:             if (!File.Exists(path))
00020:             {
00021:                 path = Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data/excavation_hazard_mitigation.json");
00022:             }
00023:             if (File.Exists(path))
00024:             {
00025:                 return File.ReadAllText(path);
00026:             }
00027:
00028:             return @"{
00029:   ""schema_version"": 1,
00030:   ""mitigations"": [
00031:     {
00032:       ""id"": ""mitigation_ventilation_blower_install"",
00033:       ""display_name"": ""Forced-Air Ventilation Blower"",
00034:       ""hazard_tags"": [""methane""],
00035:       ""required_items"": [
00036:         { ""item_id"": ""iron_pipe"", ""amount"": 2 },
00037:         { ""item_id"": ""mechanical_parts"", ""amount"": 2 }
00038:       ],
00039:       ""labor_ticks"": 100,
00040:       ""effect"": { ""methane_vent_rate_permille"": 300, ""passive_decay_bonus_permille"": 150 },
00041:       ""requires_respiratory_protection"": false,
00042:       ""tags"": [""ventilation"", ""installed""]
00043:     },
00044:     {
00045:       ""id"": ""mitigation_sump_drainage_pump"",
00046:       ""display_name"": ""Sump Drainage Pump"",
00047:       ""hazard_tags"": [""flood""],
00048:       ""required_items"": [
00049:         { ""item_id"": ""iron_pipe"", ""amount"": 4 },
00050:         { ""item_id"": ""mechanical_parts"", ""amount"": 3 }
00051:       ],
00052:       ""labor_ticks"": 110,
00053:       ""effect"": { ""flood_drain_rate_permille"": 350, ""passive_decay_bonus_permille"": 200 },
00054:       ""requires_respiratory_protection"": false,
00055:       ""tags"": [""drainage"", ""flood_control"", ""installed""]
00056:     },
00057:     {
00058:       ""id"": ""mitigation_chemical_spore_scrub"",
00059:       ""display_name"": ""Biocide Spore Decontamination"",
00060:       ""hazard_tags"": [""spores""],
00061:       ""required_items"": [
00062:         { ""item_id"": ""chemicals"", ""amount"": 2 },
00063:         { ""item_id"": ""clean_water"", ""amount"": 1 }
00064:       ],
00065:       ""labor_ticks"": 90,
00066:       ""effect"": { ""spore_reduction_permille"": 500, ""passive_decay_bonus_permille"": 100 },
00067:       ""requires_respiratory_protection"": true,
00068:       ""tags"": [""biocide""]
00069:     },
00070:     {
00071:       ""id"": ""mitigation_timber_shoring_reinforcement"",
00072:       ""display_name"": ""Heavy Timber Strut Shoring"",
00073:       ""hazard_tags"": [""shoring""],
00074:       ""required_items"": [
00075:         { ""item_id"": ""scrap_wood"", ""amount"": 6 },
00076:         { ""item_id"": ""scrap_metal"", ""amount"": 2 }
00077:       ],
00078:       ""labor_ticks"": 120,
00079:       ""effect"": { ""shoring_health_restore_permille"": 400 },
00080:       ""requires_respiratory_protection"": false,
00081:       ""tags"": [""shoring""]
00082:     },
00083:     {
00084:       ""id"": ""mitigation_sky_armor_blast_matting"",
00085:       ""display_name"": ""Blast Matting & Sandbag Curtain"",
00086:       ""hazard_tags"": [""cave_in""],
00087:       ""required_items"": [
00088:         { ""item_id"": ""cloth"", ""amount"": 4 },
00089:         { ""item_id"": ""scrap_metal"", ""amount"": 3 }
00090:       ],
00091:       ""labor_ticks"": 110,
00092:       ""effect"": { ""collapse_risk_reduction_permille"": 450, ""passive_decay_bonus_permille"": 600 },
00093:       ""requires_respiratory_protection"": false,
00094:       ""tags"": [""blast_matting"", ""installed""]
00095:     }
00096:   ]
00097: }";
00098:         }
00099:
00100:         private static ExcavationHazardSystem CreateSystem(
00101:             out Inventory.Inventory inv,
00102:             int seed = 42)
00103:         {
00104:             var rng = new SeededRng(seed);
00105:             inv = new Inventory.Inventory { Capacity = 100, MaxWeight = 500f };
00106:             var excavation = new ExcavationSystem(rng);
00107:             var skyArmor = new SkyLayerArmorSystem();
00108:
00109:             var system = new ExcavationHazardSystem(inv, rng, excavation, skyArmor);
00110:             system.LoadCatalog(GetExcavationCatalogJson());
00111:             return system;
00112:         }
00113:
00114:         [Fact]
00115:         public void VentilationBlower_InstallsAndReducesMethane()
00116:         {
00117:             var system = CreateSystem(out var inv);
00118:             var sector = system.GetOrCreateSector("sector_1");
00119:             sector.MethanePpm = 2000;
00120:
00121:             inv.Add(new ItemDefinition { id = "iron_pipe" }, 2);
00122:             inv.Add(new ItemDefinition { id = "mechanical_parts" }, 2);
00123:
00124:             var res = system.TryApplyMitigation("sector_1", "mitigation_ventilation_blower_install");
00125:             Assert.Equal(ActionResult.StatusKind.Success, res.Status);
00126:
00127:             Assert.True(sector.MethanePpm < 2000);
00128:             Assert.Contains("mitigation_ventilation_blower_install", sector.InstalledMitigationIds);
00129:         }
00130:
00131:         [Fact]
00132:         public void SporeScrub_RequiresGasMaskInInventory()
00133:         {
00134:             var system = CreateSystem(out var inv);
00135:             var sector = system.GetOrCreateSector("sector_spores");
00136:             sector.SporeConcentrationPermille = 600;
00137:
00138:             inv.Add(new ItemDefinition { id = "chemicals" }, 5);
00139:             inv.Add(new ItemDefinition { id = "clean_water" }, 5);
00140:             // Missing gas_mask
00141:
00142:             var failRes = system.TryApplyMitigation("sector_spores", "mitigation_chemical_spore_scrub");
00143:             Assert.NotEqual(ActionResult.StatusKind.Success, failRes.Status);
00144:
00145:             // Add gas mask
00146:             inv.Add(new ItemDefinition { id = "gas_mask" }, 1);
00147:             var okRes = system.TryApplyMitigation("sector_spores", "mitigation_chemical_spore_scrub");
00148:             Assert.Equal(ActionResult.StatusKind.Success, okRes.Status);
00149:             Assert.Equal(100, sector.SporeConcentrationPermille); // 600 - 500
00150:         }
00151:
00152:         [Fact]
00153:         public void ShoringReinforcement_RestoresShoringHealth()
00154:         {
00155:             var system = CreateSystem(out var inv);
00156:             var sector = system.GetOrCreateSector("sector_deep");
00157:             sector.ShoringHealthPermille = 300;
00158:
00159:             inv.Add(new ItemDefinition { id = "scrap_wood" }, 6);
00160:             inv.Add(new ItemDefinition { id = "scrap_metal" }, 2);
00161:
00162:             var res = system.TryApplyMitigation("sector_deep", "mitigation_timber_shoring_reinforcement");
00163:             Assert.Equal(ActionResult.StatusKind.Success, res.Status);
00164:             Assert.Equal(700, sector.ShoringHealthPermille); // 300 + 400
00165:         }
00166:
00167:         [Fact]
00168:         public void BulkheadToggle_BlocksSealingWhenMinersTrapped()
00169:         {
00170:             var system = CreateSystem(out _);
00171:             system.TriggerCaveInRescue("sector_cave", new[] { "survivor_1" }, 3, 200);
00172:
00173:             var sealRes = system.TryToggleBulkhead("sector_cave", true, out var reason);
00174:             Assert.NotEqual(ActionResult.StatusKind.Success, sealRes.Status);
00175:             Assert.Equal("trapped_miners", sealRes.FailureCode);
00176:         }
00177:
00178:         [Fact]
00179:         public void TrappedMinersRescue_CompletesSuccessfullyWithLabor()
00180:         {
00181:             var system = CreateSystem(out _);
00182:             system.TriggerCaveInRescue("sector_mine", new[] { "survivor_1", "survivor_2" }, 3, 200);
00183:
00184:             var sector = system.GetOrCreateSector("sector_mine");
00185:             Assert.Equal(2, sector.ActiveTrappedMiners.Count);
00186:             Assert.False(sector.RescueCompleted);
00187:
00188:             system.ProgressRescueLabor("sector_mine", 200);
00189:
00190:             Assert.True(sector.RescueCompleted);
00191:             Assert.Empty(sector.ActiveTrappedMiners);
00192:         }
00193:
00194:         [Fact]
00195:         public void TrappedMinersRescue_FailsWhenDeadlineExceeded()
00196:         {
00197:             var system = CreateSystem(out _);
00198:             system.TriggerCaveInRescue("sector_mine", new[] { "survivor_1" }, 2, 200); // 2 days deadline (day 0 + 2 = 2)
00199:
00200:             var sector = system.GetOrCreateSector("sector_mine");
00201:             system.TickDay(1);
00202:             Assert.False(sector.RescueFailed);
00203:
00204:             system.TickDay(3); // Day 3 > Day 2
00205:             Assert.True(sector.RescueFailed);
00206:         }
00207:
00208:         [Fact]
00209:         public void BlastMatting_ReducesCollapseRisk()
00210:         {
00211:             var system = CreateSystem(out var inv);
00212:             var sector = system.GetOrCreateSector("sector_mat");
00213:             sector.ShoringHealthPermille = 200; // Low shoring
00214:
00215:             var (riskBefore, _, _) = system.EvaluateOperationRisk("sector_mat");
00216:
00217:             inv.Add(new ItemDefinition { id = "cloth" }, 4);
00218:             inv.Add(new ItemDefinition { id = "scrap_metal" }, 3);
00219:             system.TryApplyMitigation("sector_mat", "mitigation_sky_armor_blast_matting");
00220:
00221:             var (riskAfter, _, _) = system.EvaluateOperationRisk("sector_mat");
00222:             Assert.True(riskAfter < riskBefore);
00223:         }
00224:
00225:         [Fact]
00226:         public void SaveRestore_PreservesSectorHazardsAndInstalledMitigations()
00227:         {
00228:             var system = CreateSystem(out var inv);
00229:             var sector = system.GetOrCreateSector("sector_save");
00230:             sector.MethanePpm = 4500;
00231:             sector.ShoringHealthPermille = 800;
00232:
00233:             inv.Add(new ItemDefinition { id = "iron_pipe" }, 2);
00234:             inv.Add(new ItemDefinition { id = "mechanical_parts" }, 2);
00235:             system.TryApplyMitigation("sector_save", "mitigation_ventilation_blower_install");
00236:
00237:             var save = system.CaptureState();
00238:             var system2 = CreateSystem(out _);
00239:             system2.RestoreState(save);
00240:
00241:             var restored = system2.GetOrCreateSector("sector_save");
00242:             Assert.Equal(sector.MethanePpm, restored.MethanePpm);
00243:             Assert.Contains("mitigation_ventilation_blower_install", restored.InstalledMitigationIds);
00244:         }
00245:
00246:         [Fact]
00247:         public void DeterministicReplay_YieldsIdenticalHazardEvolution()
00248:         {
00249:             var sysA = CreateSystem(out _, seed: 777);
00250:             var sysB = CreateSystem(out _, seed: 777);
00251:
00252:             sysA.GetOrCreateSector("sec_A").MethanePpm = 1000;
00253:             sysB.GetOrCreateSector("sec_A").MethanePpm = 1000;
00254:
00255:             sysA.TickDay(1);
00256:             sysB.TickDay(1);
00257:
00258:             Assert.Equal(sysA.GetOrCreateSector("sec_A").MethanePpm,
00259:                          sysB.GetOrCreateSector("sec_A").MethanePpm);
00260:         }
00261:
00262:         [Fact]
00263:         public void AddMethane_CrossingIgnitionThreshold_RaisesExactlyOncePerCrossing()
00264:         {
00265:             var system = CreateSystem(out _);
00266:             var sector = system.GetOrCreateSector("sector_ignition");
00267:             int ignitions = 0;
00268:             system.OnMethaneIgnition += _ => ignitions++;
00269:
00270:             // Below threshold: no ignition.
00271:             system.AddMethane("sector_ignition", 3000);
00272:             Assert.Equal(0, ignitions);
00273:             Assert.Equal(3300, sector.MethanePpm);
00274:
00275:             // Crossing raises once.
00276:             system.AddMethane("sector_ignition", 1000);
00277:             Assert.Equal(1, ignitions);
00278:             Assert.Equal(4300, sector.MethanePpm);
00279:
00280:             // Staying above the threshold must not re-fire.
00281:             system.AddMethane("sector_ignition", 500);
00282:             Assert.Equal(1, ignitions);
00283:
00284:             // Ventilating below and crossing again is a fresh ignition.
00285:             system.AddMethane("sector_ignition", -1000);
00286:             Assert.Equal(1, ignitions);
00287:             system.AddMethane("sector_ignition", 1000);
00288:             Assert.Equal(2, ignitions);
00289:         }
00290:
00291:         [Fact]
00292:         public void TickDay_MethaneAccumulation_RaisesIgnitionOnCrossing()
00293:         {
00294:             var system = CreateSystem(out _, seed: 4242);
00295:             var sector = system.GetOrCreateSector("sector_tick");
00296:             sector.MethanePpm = ExcavationHazardSystem.MethaneIgnitionThresholdPpm - 10;
00297:             int ignitions = 0;
00298:             system.OnMethaneIgnition += _ => ignitions++;
00299:
00300:             system.TickDay(1);
00301:
00302:             Assert.Equal(1, ignitions);
00303:             Assert.True(sector.MethanePpm > ExcavationHazardSystem.MethaneIgnitionThresholdPpm);
00304:         }
00305:
00306:         [Fact]
00307:         public void AddFloodWater_CrossingCriticalThreshold_RaisesExactlyOncePerCrossing()
00308:         {
00309:             var system = CreateSystem(out _);
00310:             int floods = 0;
00311:             system.OnSectorFlooded += _ => floods++;
00312:
00313:             Assert.True(system.AddFloodWater("sector_flood", 300));
00314:             Assert.Equal(0, floods);
00315:
00316:             Assert.True(system.AddFloodWater("sector_flood", 300));
00317:             Assert.Equal(1, floods);
00318:             Assert.Equal(600, system.GetOrCreateSector("sector_flood").FloodLevelPermille);
00319:
00320:             // Already flooded: no repeat notification.
00321:             system.AddFloodWater("sector_flood", 100);
00322:             Assert.Equal(1, floods);
00323:         }
00324:
00325:         [Fact]
00326:         public void InstalledMitigation_PassiveDecay_UsesAuthoredBonus()
00327:         {
00328:             var system = CreateSystem(out var inv, seed: 5150);
00329:             var sector = system.GetOrCreateSector("sector_passive");
00330:             sector.MethanePpm = 3000;
00331:
00332:             inv.Add(new ItemDefinition { id = "iron_pipe" }, 2);
00333:             inv.Add(new ItemDefinition { id = "mechanical_parts" }, 2);
00334:             var install = system.TryApplyMitigation("sector_passive", "mitigation_ventilation_blower_install");
00335:             Assert.Equal(ActionResult.StatusKind.Success, install.Status);
00336:
00337:             int before = sector.MethanePpm;
00338:             system.TickDay(1);
00339:
00340:             // Authored passive decay is 150/day; accumulation draws 50–149, so the
00341:             // net change must be strictly negative (the former hardcoded -200 is retired).
00342:             Assert.True(sector.MethanePpm < before,
00343:                 $"expected authored passive decay to outpace accumulation (before={before}, after={sector.MethanePpm})");
00344:         }
00345:
00346:         [Fact]
00347:         public void InstalledDrainagePump_PassiveDecay_LowersFloodLevel()
00348:         {
00349:             var system = CreateSystem(out var inv);
00350:             var sector = system.GetOrCreateSector("sector_pump");
00351:             sector.FloodLevelPermille = 700;
00352:
00353:             inv.Add(new ItemDefinition { id = "iron_pipe" }, 4);
00354:             inv.Add(new ItemDefinition { id = "mechanical_parts" }, 3);
00355:             var install = system.TryApplyMitigation("sector_pump", "mitigation_sump_drainage_pump");
00356:             Assert.Equal(ActionResult.StatusKind.Success, install.Status);
00357:
00358:             // Install drains 350 immediately; the authored daily upkeep drains 200 more.
00359:             int afterInstall = sector.FloodLevelPermille;
00360:             system.TickDay(1);
00361:
00362:             Assert.Equal(Math.Max(0, afterInstall - 200), sector.FloodLevelPermille);
00363:         }
00364:     }
00365: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **Current evidence and safe integration boundary for Plan 37: Excavation Sites, Archaeology and Deep-Strata Reachability.**.

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
