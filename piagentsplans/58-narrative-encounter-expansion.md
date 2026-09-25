# Plan 58 — Narrative Encounters, Choice Resolution and Expedition Reachability

> **Rebuild status:** COMPLETE 16 BASE + 29 EXPANSION AUTHORED SETS — LOADER MERGES MORE THAN THE BASE FILE; EXPEDITION BRIDGE IS THE CURRENT REACHABILITY SEAM
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

- The current loader combines narrative_encounters.json, narrative_encounters_expansion.json, narrative_encounters_npc_arcs.json and micro_locations.json.
- NarrativeEncounterSystem has weighted selection, stealth/speed modifiers, depletion, pending encounters, history, cumulative morale and capture/restore.
- The current test expects 102 loaded definitions, with expansion-source provenance and duplicate/primary-wins behavior; the old 25-row target is false.

**Bounded outcome:** Retire the 3-to-25 base-catalog objective. narrative_encounters.json currently has 16 rows, the expansion file has 29, and the loader also composes NPC-arc and micro-location sources. NarrativeEncounterSystem owns selection, depletion, choices, history and save state; the residual is source/consumer truth and deduplication, not another encounter file.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- Replace the old row target with source inventory, provenance and reachability matrix.
- Trace expedition/travel/door/quest consumers into NarrativeEncounterSystem and verify exactly-once choice resolution.
- Keep content catalogs, encounter runtime state, quest state and journal/history state separate.

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
- The real work is multi-source provenance and cross-system reachability, not base-file row count.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Current evidence requires a bounded owner/reachability audit; no new authority is implied.

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
| multi-file encounter load, source stamping and dedupe | NarrativeEncounterCatalogLoader | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` | Static catalog composition. |
| selection, resolution, depletion, pending and history | NarrativeEncounterSystem | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` | Sole encounter runtime owner. |
| expedition/travel encounter integration | ExpeditionEncounterBridge | `Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs` | Current cross-system bridge. |
| encounter persistence adapter | TravelEncounterSaveStore | `src/Host/TravelEncounterSaveStore.cs` | Existing save seam. |
| source/reachability evidence | Content certification | `src/Main.ContentCertification.cs` | Does not become gameplay authority. |
| selection, save, catalog and checksum contracts | NarrativeEncounterSystemTests | `Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs` | Focused evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Narrative Encounters, Choice Resolution and Expedition Reachability
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ NarrativeEncounterCatalogLoader
│   multi-file encounter load, source stamping and dedupe
│ NarrativeEncounterSystem
│   selection, resolution, depletion, pending and history
│ ExpeditionEncounterBridge
│   expedition/travel encounter integration
│ TravelEncounterSaveStore
│   encounter persistence adapter
│ Content certification
│   source/reachability evidence
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

1. **Preserve current state ownership.** NarrativeEncounterCatalogLoader owns multi-file encounter load, source stamping and dedupe: Static catalog composition.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| multi-file encounter load, source stamping and dedupe | NarrativeEncounterCatalogLoader | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` | Static catalog composition. |
| selection, resolution, depletion, pending and history | NarrativeEncounterSystem | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` | Sole encounter runtime owner. |
| expedition/travel encounter integration | ExpeditionEncounterBridge | `Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs` | Current cross-system bridge. |
| encounter persistence adapter | TravelEncounterSaveStore | `src/Host/TravelEncounterSaveStore.cs` | Existing save seam. |
| source/reachability evidence | Content certification | `src/Main.ContentCertification.cs` | Does not become gameplay authority. |
| selection, save, catalog and checksum contracts | NarrativeEncounterSystemTests | `Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs` | Focused evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load and provenance-stamp all current encounter sources
2. deduplicate by current ID rule
3. apply stealth/speed/danger/location eligibility
4. select through seeded weighting
5. record selection and pending state
6. resolve one valid choice and route effects
7. capture/restore encounter state

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Encounter definitions are immutable and source-tagged.
- NarrativeEncounterState owns history, pending and depleted IDs.
- Quest state remains with QuestlineSystem/quest owners.
- A choice consequence must route to its current owner rather than an encounter-local economy.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Only valid current choices resolve.
- Depleting choices persist one time.
- Duplicate source IDs are handled by current primary-wins policy.
- A source file cannot write quest or inventory state directly.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- Four encounter content files remain separate sources.
- Items, locations, quests and factions own their references/effects.
- No single new encounter catalog is justified.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use NarrativeEncounterState/current travel encounter save.
- No Plan-58 save section.
- Old save state with missing pending/history defaults remains valid.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Weighted selection uses ISeededRng and stable definition ordering.
- Ties use stable ID order, not hash iteration.
- Same source, state and seed produce the same selection/resolution.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- NarrativeEncounterSystem emits selected/resolved/state-changed facts.
- The bridge emits expedition facts consumed by the encounter system.
- Journal/UI consume resolution facts without mutating gameplay.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/TravelEncounterSaveStore.cs
- src/Main.ContentCertification.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Encounter prose is grounded, fictional and consequence-aware.
- No copied real-world encounter scripts or unmarked moral lecture.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | Duplicate source rows silently replace authored content. | NarrativeEncounterCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A depleted encounter reappears. | NarrativeEncounterSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A choice writes to the wrong owner. | ExpeditionEncounterBridge | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A source file is treated as a separate runtime. | TravelEncounterSaveStore | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new save store duplicates history. | Content certification | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 | Source and count census. | 16/29/other-source counts are explicit. | No production path until the owning implementation package is separately claimed. |
| 1 | Loader provenance/dedupe audit. | Primary-wins behavior is documented. | No production path until the owning implementation package is separately claimed. |
| 2 | Runtime/save/bridge trace. | One owner per consequence. | No production path until the owning implementation package is separately claimed. |
| 3 | UI/accessibility and replay polish. | No fake route or duplicate event. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/narrative_encounters.json | READ ONLY; MODIFY only for content defect | 16 base rows |
| Assets/StreamingAssets/Data/narrative_encounters_expansion.json | READ ONLY | 29 expansion rows |
| Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs | READ ONLY | Runtime and loader |
| src/Host/TravelEncounterSaveStore.cs | READ ONLY | Save seam |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Parallel encounter catalogs. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Source duplication. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Choice effects in the loader. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| UI-owned depletion or history. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new encounter rows.
- No new encounter manager.
- No production/data/test/UI changes here.

# 23. Rollback and Recovery

- Revert planning artifact.
- Future provenance/UI fixes remain save-compatible.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- All current source files and 102-definition composition are documented.
- Selection, choice, save, bridge and UI contracts are explicit.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Current evidence requires a bounded owner/reachability audit; no new authority is implied.

## MUST NOT DO

- No new encounter rows.
- No new encounter manager.
- No production/data/test/UI changes here.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: multi-file encounter load, source stamping and dedupe → NarrativeEncounterCatalogLoader; selection, resolution, depletion, pending and history → NarrativeEncounterSystem; expedition/travel encounter integration → ExpeditionEncounterBridge; encounter persistence adapter → TravelEncounterSaveStore; source/reachability evidence → Content certification; selection, save, catalog and checksum contracts → NarrativeEncounterSystemTests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 58.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 58 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by NarrativeEncounterCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`

### `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 587 lines / 26931 bytes.
- SHA-256: `994aeb65e087024e7b71bf03a48a8992b492e92966ea3cfa11f6cbf9aa825a83`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class NarrativeEncounterResolutionResult
public string EncounterId = string.Empty;
public string ChoiceId = string.Empty;
public string LocationId = string.Empty;
public int Day;
public int MoraleDelta;
public int GuiltDelta;
public bool DepletesEncounter;
public string GrantItemId = string.Empty;
public int GrantItemQuantity;
public string JournalUnlockId = string.Empty;
public string DiscoverLocationId = string.Empty;
public string SetWorldFlagId = string.Empty;
public string ResolutionId =>
public class NarrativeEncounterSystem
public const string SystemId = "narrative_encounter_system";
public event Action<EncounterDefinition> OnEncounterSelected;
public event Action<EncounterResolutionRecord> OnEncounterResolved;
public event Action<NarrativeEncounterState> OnStateChanged;
public Func<string, bool>? WeatherGateFilter { get; set; }
public ExpansionQuestSystem? QuestLink { get; set; }
public ContentUtilizationInstrumentation? Instrumentation { get; set; }
public NarrativeEncounterState State => _state;
public IReadOnlyList<EncounterDefinition> Catalog => _catalog;
public int TotalResolved => _state.totalResolved;
public void RegisterEncounter(EncounterDefinition def) {
public void RegisterRange(IEnumerable<EncounterDefinition> defs) {
public EncounterDefinition? Find(string encounterId) {
public bool IsDepleted(string encounterId) {
public int DepletedCount => _depletedEncounters.Count;
public void RecordEncounterSelected(EncounterDefinition def) {
public EncounterDefinition? SelectEncounter( string stance, float dangerLevel, string locationId, ISeededRng rng) {
public bool Resolve(string encounterId, string choiceId, string locationId, int day) => TryResolve(encounterId, choiceId, locationId, day) != null;
public NarrativeEncounterResolutionResult? TryResolve( string encounterId, string choiceId, string locationId, int day) {
public void EnqueuePending(string encounterId, string locationId, int legIndex, int day) {
public void ClearPending(string encounterId) {
public void ClearAllPending() {
public NarrativeEncounterState CaptureState() {
public void RestoreState(NarrativeEncounterState saved) {
public static class NarrativeEncounterCatalogLoader
public const string FileName = "narrative_encounters.json";
public const string ArcFileName = "narrative_encounters_npc_arcs.json";
public const string ExpansionFileName = "narrative_encounters_expansion.json";
public const string MicroLocationsFileName = "micro_locations.json";
public static List<EncounterDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs`

### `Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 354 lines / 16880 bytes.
- SHA-256: `a9653182ddac1d1221b396712dbedb33cb2629671656027bb3421b1a437ad88e`.
- Architecture signals: seeded references=6; save/restore symbols=0; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpeditionEncounterBridge
public sealed class EncounterSurfaced
public string encounter_id;
public string title;
public string description;
public string category;
public List<EncounterChoiceDefinition> choices;
public bool is_patrol;
public bool is_micro_location;
public string faction_id;
public string territory_state;
public string patrol_archetype;
public string recognition_label;
public int patrol_chain_stage;
public ExpeditionState trigger;
public bool? resolved_at_lead;
public string encounter_record_resolution_id;
public event Action<EncounterSurfaced> OnSurfaced;
public TravelEncounterSystem? TravelEngine { get; set; }
public int CurrentDay { get; set; } = 1;
public string CurrentSeason { get; set; } = "all";
public Func<string, string>? RegionResolver { get; set; }
public EncounterSurfaced? LastSurfaced => _lastSurfaced;
public NarrativeEncounterResolutionResult? LastResolution { get; private set; }
public void SetRng(ISeededRng rng) {
public void Surface(ExpeditionState state) {
public bool ResolveChoice(string encounterId, string choiceId, int day) => ResolveChoice(encounterId, choiceId, day, null!);
public bool ResolveChoice(string encounterId, string choiceId, int day, string locationId) {
```


# Appendix B.04 — Current Code Architecture: `src/Host/TravelEncounterSaveStore.cs`

### `src/Host/TravelEncounterSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 47 lines / 1766 bytes.
- SHA-256: `0ad9f8e77b7be03ec8236e13881fa782c40c59c9453b3a1a61744515abc0f84c`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class TravelEncounterSaveStore
public const string FileName = "travel_encounters_save.json";
public const string SectionName = "travel_encounters";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static bool TrySave(TravelEncounterState state) => s_store.TrySave(state);
public static TravelEncounterState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(TravelEncounterState state) =>
public static TravelEncounterState FromSystem(TravelEncounterSystem system) {
public static void ApplyToSystem(TravelEncounterSystem system, TravelEncounterState? state) {
```


# Appendix B.05 — Current Code Architecture: `src/Host/TravelEncounterSaveStore.cs`

### `src/Host/TravelEncounterSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 47 lines / 1766 bytes.
- SHA-256: `0ad9f8e77b7be03ec8236e13881fa782c40c59c9453b3a1a61744515abc0f84c`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class TravelEncounterSaveStore
public const string FileName = "travel_encounters_save.json";
public const string SectionName = "travel_encounters";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static bool TrySave(TravelEncounterState state) => s_store.TrySave(state);
public static TravelEncounterState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(TravelEncounterState state) =>
public static TravelEncounterState FromSystem(TravelEncounterSystem system) {
public static void ApplyToSystem(TravelEncounterSystem system, TravelEncounterState? state) {
```


# Appendix B.06 — Current Code Architecture: `src/Main.ContentCertification.cs`

### `src/Main.ContentCertification.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 114 lines / 5613 bytes.
- SHA-256: `13e0aefec31ad8e8fe8002b5d0ad03f6f120c024456f6f58259aab164b38678f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public ContentCertificationHostSession? ContentCertification => _contentCertification;
internal ContentCertificationHostSession CertifyContentOrphans() {
```


# Appendix C.07 — Catalog Census: `Assets/StreamingAssets/Data/narrative_encounters.json`

### `Assets/StreamingAssets/Data/narrative_encounters.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 23144 bytes / 23138 characters.
- SHA-256: `908b57d03bdf04e3308587eef5700dcc432c37ab082163497627a710a30eb0b3`.
- Root keys: `encounters`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
encounters: min=16, max=16, observed_paths=1
encounters[].choices: min=4, max=4, observed_paths=2
```

Representative record fields:

- `baseWeight`
- `category`
- `choices`
- `description`
- `forceOnArrival`
- `id`
- `minDangerLevel`
- `requiredLocationId`
- `speedWeightMultiplier`
- `stealthWeightMultiplier`
- `title`

Representative identifiers (ordered, capped for readability):

```text
enc_dead_letter_office
enc_weather_station
enc_pianist
enc_census_carrier_on_the_road
enc_ninth_night_listener
enc_the_tower_classroom
enc_glass_blower_of_the_rim
enc_the_surveyor_still_working
enc_the_orchard_with_stakes
enc_ferryman_toll_of_stories
enc_dog_at_the_sealed_door
enc_quarry_witness
enc_grave_with_the_wrong_name
enc_trap_bait_stolen
enc_trap_tampered
enc_trap_stranger_discovery
```


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`

### `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 39422 bytes / 39410 characters.
- SHA-256: `6fc1c3a4c6150ab6b29003a27821e81f6d0cf3ed8930d4801976dcb75298bc67`.
- Root keys: `encounters`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
encounters: min=29, max=29, observed_paths=1
encounters[].choices: min=4, max=4, observed_paths=2
```

Representative record fields:

- `baseWeight`
- `category`
- `choices`
- `description`
- `forceOnArrival`
- `id`
- `minDangerLevel`
- `requiredLocationId`
- `speedWeightMultiplier`
- `stealthWeightMultiplier`
- `title`

Representative identifiers (ordered, capped for readability):

```text
enc_overturned_postal
enc_weather_station
enc_pianist
enc_roadside_trader
enc_water_seller
enc_false_broadcast
enc_two_camps
enc_sick_child
enc_injured_scavenger
enc_roof_access
enc_cold_storage
enc_hot_sign
enc_clean_well
enc_whiteout_traveler
enc_flood_road
enc_dogs_silent
enc_following_footsteps
enc_border_camp
enc_two_factions_trade
enc_locked_room
enc_two_graves
enc_beggars
enc_thief_child
enc_library_cache
enc_greenhouse_keeper
enc_dead_radio_operator
enc_ice_fishermen
enc_quarantine_sign
enc_last_train
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/narrative_encounters_npc_arcs.json`

### `Assets/StreamingAssets/Data/narrative_encounters_npc_arcs.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 40979 bytes / 40955 characters.
- SHA-256: `feb0c3fe2dec2b19a70e15e0b89ec478e6bd5d1cebf08c5f245c9b837a43b26b`.
- Root keys: `encounters`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
encounters: min=31, max=31, observed_paths=1
encounters[].choices: min=2, max=2, observed_paths=2
```

Representative record fields:

- `baseWeight`
- `category`
- `choices`
- `description`
- `forceOnArrival`
- `id`
- `minDangerLevel`
- `npcId`
- `requiredLocationId`
- `speedWeightMultiplier`
- `stealthWeightMultiplier`
- `title`

Representative identifiers (ordered, capped for readability):

```text
enc_arc_mara_waystation
enc_arc_mara_route
enc_arc_ilze_clinic
enc_arc_ilze_outbreak
enc_arc_marek_patrol
enc_arc_marek_deserter
enc_arc_lina_baths
enc_arc_anete_substation
enc_arc_anete_feeder
enc_arc_sava_camp
enc_arc_sava_schism
enc_arc_rika_cache
enc_arc_rika_trap
enc_arc_liva_tower
enc_arc_liva_survey
enc_arc_oskar_01_debt
enc_arc_tomas_01_rounding
enc_arc_joren_01_plant
enc_arc_pavel_01_map
enc_arc_sena_01_log
enc_arc_dalia_01_annex
enc_arc_anton_01_herd
enc_arc_emil_01_numbers
enc_arc_nadia_01_pot
enc_arc_arvo_01_crate
enc_arc_kaspar_01_bearing
enc_arc_mira_01_intervals
enc_arc_janek_01_dawn
enc_arc_veda_01_circuit
enc_arc_mirael_01_page
enc_arc_niko_01_corridors
```


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/micro_locations.json`

### `Assets/StreamingAssets/Data/micro_locations.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 35356 bytes / 35344 characters.
- SHA-256: `8d6098ece0060ef012e5f3871c324f606ca520a844c14da2ed614187b80820a0`.
- Root keys: `collection_id`, `encounters`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
encounters: min=28, max=28, observed_paths=1
encounters[].choices: min=2, max=3, observed_paths=2
```

Representative record fields:

- `baseWeight`
- `category`
- `choices`
- `description`
- `id`
- `minDangerLevel`
- `requiredLocationId`
- `speedWeightMultiplier`
- `stealthWeightMultiplier`
- `title`

Representative identifiers (ordered, capped for readability):

```text
micro_roadside_memorial
micro_crashed_truck
micro_frozen_bus
micro_improvised_grave
micro_collapsed_bridge
micro_drainage_pipe
micro_rail_siding
micro_dead_livestock
micro_ruined_greenhouse
micro_shell_crater
micro_field_kitchen
micro_abandoned_generator
micro_shrine
micro_emergency_cache
micro_observation_post
micro_abandoned_barricade
micro_hunting_blind
micro_radio_tower
micro_destroyed_checkpoint
micro_abandoned_tent
micro_makeshift_clinic
micro_crashed_drone
micro_fuel_cache
micro_water_source
micro_supply_drop
micro_hospital_chapel_ledger
micro_depot_undertow_raft_line
micro_gamma_levy_board
```


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs`

### `Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 267; SHA-256: `e141922337d8b3312f400c3fb3548ff33490fb5a6a7cf93ca007cb5c1abe1623`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Register_NullAndDuplicateIgnored
Select_WeightsPreferHigherBaseWeight
Select_FiltersByDangerAndLocation
StanceMultipliers_MatchUnityValues
Select_ReturnsNullWhenNothingEligible
Resolve_RecordsHistoryAndTotals
Resolve_UnknownEncounterOrChoiceRejected
CaptureState_ReturnsSnapshotNotLiveState
CaptureState_EmitsInOrdinalOrder
SaveLoad_RoundTripsAllState
SaveLoad_ChecksumStable
Catalog_LoadsTheThreeUnityEncounters
Catalog_UnityWeightParity
```


# Appendix E.12 — Supporting Code Evidence: `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`

### `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 587 lines / 26931 bytes.
- SHA-256: `994aeb65e087024e7b71bf03a48a8992b492e92966ea3cfa11f6cbf9aa825a83`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class NarrativeEncounterResolutionResult
public string EncounterId = string.Empty;
public string ChoiceId = string.Empty;
public string LocationId = string.Empty;
public int Day;
public int MoraleDelta;
public int GuiltDelta;
public bool DepletesEncounter;
public string GrantItemId = string.Empty;
public int GrantItemQuantity;
public string JournalUnlockId = string.Empty;
public string DiscoverLocationId = string.Empty;
public string SetWorldFlagId = string.Empty;
public string ResolutionId =>
public class NarrativeEncounterSystem
public const string SystemId = "narrative_encounter_system";
public event Action<EncounterDefinition> OnEncounterSelected;
public event Action<EncounterResolutionRecord> OnEncounterResolved;
public event Action<NarrativeEncounterState> OnStateChanged;
public Func<string, bool>? WeatherGateFilter { get; set; }
public ExpansionQuestSystem? QuestLink { get; set; }
public ContentUtilizationInstrumentation? Instrumentation { get; set; }
public NarrativeEncounterState State => _state;
public IReadOnlyList<EncounterDefinition> Catalog => _catalog;
public int TotalResolved => _state.totalResolved;
public void RegisterEncounter(EncounterDefinition def) {
public void RegisterRange(IEnumerable<EncounterDefinition> defs) {
public EncounterDefinition? Find(string encounterId) {
public bool IsDepleted(string encounterId) {
public int DepletedCount => _depletedEncounters.Count;
public void RecordEncounterSelected(EncounterDefinition def) {
public EncounterDefinition? SelectEncounter( string stance, float dangerLevel, string locationId, ISeededRng rng) {
public bool Resolve(string encounterId, string choiceId, string locationId, int day) => TryResolve(encounterId, choiceId, locationId, day) != null;
public NarrativeEncounterResolutionResult? TryResolve( string encounterId, string choiceId, string locationId, int day) {
public void EnqueuePending(string encounterId, string locationId, int legIndex, int day) {
public void ClearPending(string encounterId) {
public void ClearAllPending() {
public NarrativeEncounterState CaptureState() {
public void RestoreState(NarrativeEncounterState saved) {
public static class NarrativeEncounterCatalogLoader
public const string FileName = "narrative_encounters.json";
public const string ArcFileName = "narrative_encounters_npc_arcs.json";
public const string ExpansionFileName = "narrative_encounters_expansion.json";
public const string MicroLocationsFileName = "micro_locations.json";
public static List<EncounterDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix E.13 — Supporting Code Evidence: `Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs`

### `Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 354 lines / 16880 bytes.
- SHA-256: `a9653182ddac1d1221b396712dbedb33cb2629671656027bb3421b1a437ad88e`.
- Architecture signals: seeded references=6; save/restore symbols=0; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpeditionEncounterBridge
public sealed class EncounterSurfaced
public string encounter_id;
public string title;
public string description;
public string category;
public List<EncounterChoiceDefinition> choices;
public bool is_patrol;
public bool is_micro_location;
public string faction_id;
public string territory_state;
public string patrol_archetype;
public string recognition_label;
public int patrol_chain_stage;
public ExpeditionState trigger;
public bool? resolved_at_lead;
public string encounter_record_resolution_id;
public event Action<EncounterSurfaced> OnSurfaced;
public TravelEncounterSystem? TravelEngine { get; set; }
public int CurrentDay { get; set; } = 1;
public string CurrentSeason { get; set; } = "all";
public Func<string, string>? RegionResolver { get; set; }
public EncounterSurfaced? LastSurfaced => _lastSurfaced;
public NarrativeEncounterResolutionResult? LastResolution { get; private set; }
public void SetRng(ISeededRng rng) {
public void Surface(ExpeditionState state) {
public bool ResolveChoice(string encounterId, string choiceId, int day) => ResolveChoice(encounterId, choiceId, day, null!);
public bool ResolveChoice(string encounterId, string choiceId, int day, string locationId) {
```


# Appendix E.14 — Supporting Code Evidence: `src/Host/TravelEncounterSaveStore.cs`

### `src/Host/TravelEncounterSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 47 lines / 1766 bytes.
- SHA-256: `0ad9f8e77b7be03ec8236e13881fa782c40c59c9453b3a1a61744515abc0f84c`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class TravelEncounterSaveStore
public const string FileName = "travel_encounters_save.json";
public const string SectionName = "travel_encounters";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static bool TrySave(TravelEncounterState state) => s_store.TrySave(state);
public static TravelEncounterState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(TravelEncounterState state) =>
public static TravelEncounterState FromSystem(TravelEncounterSystem system) {
public static void ApplyToSystem(TravelEncounterSystem system, TravelEncounterState? state) {
```


# Appendix E.15 — Supporting Code Evidence: `src/Host/TravelEncounterSaveStore.cs`

### `src/Host/TravelEncounterSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 47 lines / 1766 bytes.
- SHA-256: `0ad9f8e77b7be03ec8236e13881fa782c40c59c9453b3a1a61744515abc0f84c`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class TravelEncounterSaveStore
public const string FileName = "travel_encounters_save.json";
public const string SectionName = "travel_encounters";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static bool TrySave(TravelEncounterState state) => s_store.TrySave(state);
public static TravelEncounterState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(TravelEncounterState state) =>
public static TravelEncounterState FromSystem(TravelEncounterSystem system) {
public static void ApplyToSystem(TravelEncounterSystem system, TravelEncounterState? state) {
```


# Appendix E.16 — Supporting Code Evidence: `src/Main.ContentCertification.cs`

### `src/Main.ContentCertification.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 114 lines / 5613 bytes.
- SHA-256: `13e0aefec31ad8e8fe8002b5d0ad03f6f120c024456f6f58259aab164b38678f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public ContentCertificationHostSession? ContentCertification => _contentCertification;
internal ContentCertificationHostSession CertifyContentOrphans() {
```


# Appendix F.17 — Supporting Data Evidence: `Assets/StreamingAssets/Data/narrative_encounters.json`

### `Assets/StreamingAssets/Data/narrative_encounters.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 23144 bytes / 23138 characters.
- SHA-256: `908b57d03bdf04e3308587eef5700dcc432c37ab082163497627a710a30eb0b3`.
- Root keys: `encounters`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
encounters: min=16, max=16, observed_paths=1
encounters[].choices: min=4, max=4, observed_paths=2
```

Representative record fields:

- `baseWeight`
- `category`
- `choices`
- `description`
- `forceOnArrival`
- `id`
- `minDangerLevel`
- `requiredLocationId`
- `speedWeightMultiplier`
- `stealthWeightMultiplier`
- `title`

Representative identifiers (ordered, capped for readability):

```text
enc_dead_letter_office
enc_weather_station
enc_pianist
enc_census_carrier_on_the_road
enc_ninth_night_listener
enc_the_tower_classroom
enc_glass_blower_of_the_rim
enc_the_surveyor_still_working
enc_the_orchard_with_stakes
enc_ferryman_toll_of_stories
enc_dog_at_the_sealed_door
enc_quarry_witness
enc_grave_with_the_wrong_name
enc_trap_bait_stolen
enc_trap_tampered
enc_trap_stranger_discovery
```


# Appendix F.18 — Supporting Data Evidence: `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`

### `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 39422 bytes / 39410 characters.
- SHA-256: `6fc1c3a4c6150ab6b29003a27821e81f6d0cf3ed8930d4801976dcb75298bc67`.
- Root keys: `encounters`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
encounters: min=29, max=29, observed_paths=1
encounters[].choices: min=4, max=4, observed_paths=2
```

Representative record fields:

- `baseWeight`
- `category`
- `choices`
- `description`
- `forceOnArrival`
- `id`
- `minDangerLevel`
- `requiredLocationId`
- `speedWeightMultiplier`
- `stealthWeightMultiplier`
- `title`

Representative identifiers (ordered, capped for readability):

```text
enc_overturned_postal
enc_weather_station
enc_pianist
enc_roadside_trader
enc_water_seller
enc_false_broadcast
enc_two_camps
enc_sick_child
enc_injured_scavenger
enc_roof_access
enc_cold_storage
enc_hot_sign
enc_clean_well
enc_whiteout_traveler
enc_flood_road
enc_dogs_silent
enc_following_footsteps
enc_border_camp
enc_two_factions_trade
enc_locked_room
enc_two_graves
enc_beggars
enc_thief_child
enc_library_cache
enc_greenhouse_keeper
enc_dead_radio_operator
enc_ice_fishermen
enc_quarantine_sign
enc_last_train
```


# Appendix G.19 — Supporting Regression Evidence: `Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs`

### `Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 267; SHA-256: `e141922337d8b3312f400c3fb3548ff33490fb5a6a7cf93ca007cb5c1abe1623`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Register_NullAndDuplicateIgnored
Select_WeightsPreferHigherBaseWeight
Select_FiltersByDangerAndLocation
StanceMultipliers_MatchUnityValues
Select_ReturnsNullWhenNothingEligible
Resolve_RecordsHistoryAndTotals
Resolve_UnknownEncounterOrChoiceRejected
CaptureState_ReturnsSnapshotNotLiveState
CaptureState_EmitsInOrdinalOrder
SaveLoad_RoundTripsAllState
SaveLoad_ChecksumStable
Catalog_LoadsTheThreeUnityEncounters
Catalog_UnityWeightParity
```


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| multi-file encounter load, source stamping and dedupe | NarrativeEncounterCatalogLoader | selection, resolution, depletion, pending and history | NarrativeEncounterSystem | Owner emits/reads a typed fact; no mirror state. |
| multi-file encounter load, source stamping and dedupe | NarrativeEncounterCatalogLoader | expedition/travel encounter integration | ExpeditionEncounterBridge | Owner emits/reads a typed fact; no mirror state. |
| multi-file encounter load, source stamping and dedupe | NarrativeEncounterCatalogLoader | encounter persistence adapter | TravelEncounterSaveStore | Owner emits/reads a typed fact; no mirror state. |
| multi-file encounter load, source stamping and dedupe | NarrativeEncounterCatalogLoader | source/reachability evidence | Content certification | Owner emits/reads a typed fact; no mirror state. |
| multi-file encounter load, source stamping and dedupe | NarrativeEncounterCatalogLoader | selection, save, catalog and checksum contracts | NarrativeEncounterSystemTests | Owner emits/reads a typed fact; no mirror state. |
| selection, resolution, depletion, pending and history | NarrativeEncounterSystem | multi-file encounter load, source stamping and dedupe | NarrativeEncounterCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| selection, resolution, depletion, pending and history | NarrativeEncounterSystem | expedition/travel encounter integration | ExpeditionEncounterBridge | Owner emits/reads a typed fact; no mirror state. |
| selection, resolution, depletion, pending and history | NarrativeEncounterSystem | encounter persistence adapter | TravelEncounterSaveStore | Owner emits/reads a typed fact; no mirror state. |
| selection, resolution, depletion, pending and history | NarrativeEncounterSystem | source/reachability evidence | Content certification | Owner emits/reads a typed fact; no mirror state. |
| selection, resolution, depletion, pending and history | NarrativeEncounterSystem | selection, save, catalog and checksum contracts | NarrativeEncounterSystemTests | Owner emits/reads a typed fact; no mirror state. |
| expedition/travel encounter integration | ExpeditionEncounterBridge | multi-file encounter load, source stamping and dedupe | NarrativeEncounterCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| expedition/travel encounter integration | ExpeditionEncounterBridge | selection, resolution, depletion, pending and history | NarrativeEncounterSystem | Owner emits/reads a typed fact; no mirror state. |
| expedition/travel encounter integration | ExpeditionEncounterBridge | encounter persistence adapter | TravelEncounterSaveStore | Owner emits/reads a typed fact; no mirror state. |
| expedition/travel encounter integration | ExpeditionEncounterBridge | source/reachability evidence | Content certification | Owner emits/reads a typed fact; no mirror state. |
| expedition/travel encounter integration | ExpeditionEncounterBridge | selection, save, catalog and checksum contracts | NarrativeEncounterSystemTests | Owner emits/reads a typed fact; no mirror state. |
| encounter persistence adapter | TravelEncounterSaveStore | multi-file encounter load, source stamping and dedupe | NarrativeEncounterCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| encounter persistence adapter | TravelEncounterSaveStore | selection, resolution, depletion, pending and history | NarrativeEncounterSystem | Owner emits/reads a typed fact; no mirror state. |
| encounter persistence adapter | TravelEncounterSaveStore | expedition/travel encounter integration | ExpeditionEncounterBridge | Owner emits/reads a typed fact; no mirror state. |
| encounter persistence adapter | TravelEncounterSaveStore | source/reachability evidence | Content certification | Owner emits/reads a typed fact; no mirror state. |
| encounter persistence adapter | TravelEncounterSaveStore | selection, save, catalog and checksum contracts | NarrativeEncounterSystemTests | Owner emits/reads a typed fact; no mirror state. |
| source/reachability evidence | Content certification | multi-file encounter load, source stamping and dedupe | NarrativeEncounterCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| source/reachability evidence | Content certification | selection, resolution, depletion, pending and history | NarrativeEncounterSystem | Owner emits/reads a typed fact; no mirror state. |
| source/reachability evidence | Content certification | expedition/travel encounter integration | ExpeditionEncounterBridge | Owner emits/reads a typed fact; no mirror state. |
| source/reachability evidence | Content certification | encounter persistence adapter | TravelEncounterSaveStore | Owner emits/reads a typed fact; no mirror state. |
| source/reachability evidence | Content certification | selection, save, catalog and checksum contracts | NarrativeEncounterSystemTests | Owner emits/reads a typed fact; no mirror state. |
| selection, save, catalog and checksum contracts | NarrativeEncounterSystemTests | multi-file encounter load, source stamping and dedupe | NarrativeEncounterCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| selection, save, catalog and checksum contracts | NarrativeEncounterSystemTests | selection, resolution, depletion, pending and history | NarrativeEncounterSystem | Owner emits/reads a typed fact; no mirror state. |
| selection, save, catalog and checksum contracts | NarrativeEncounterSystemTests | expedition/travel encounter integration | ExpeditionEncounterBridge | Owner emits/reads a typed fact; no mirror state. |
| selection, save, catalog and checksum contracts | NarrativeEncounterSystemTests | encounter persistence adapter | TravelEncounterSaveStore | Owner emits/reads a typed fact; no mirror state. |
| selection, save, catalog and checksum contracts | NarrativeEncounterSystemTests | source/reachability evidence | Content certification | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Current evidence requires a bounded owner/reachability audit; no new authority is implied. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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

> **DR-02 — The docs tree has substantially more subdirectories than the v1.0 map. VERIFIED.**
Live `docs/` subdirectories observed in the audit include (selection; the listing was long and partially truncated): `adr/`, `agents/`, `architecture/`, `archive/`, `balance/`, `bodymind/`, `campaign/`, `cartography/`, `ci/`, `cli/`, `collectibles/`, `combat/`, `content/`, `contracts/`, `crafting/`, `crossing/`, `culture/`, `decisions/`, `design/`, `discovery/`, `duty_roster/`, `ecology/`, `economy/`, `endgame/`, `expansions/`, `expeditions/`, `faction_war/`, `factions/`, `foreman/`, `forensics/`, `foundry/`, `gaps/`, `governance/`, `greenhouse/`, `health/`, `holdfast/`, `hygiene/`, `i18n/`, `implementation/`, `incidents/`, `integration/`, `journal/`, `lore/`, `maritime/`, `medical/`, `memorials/`, `mods/`, `moral/`, `moral_choice/`, `muster/`, `narrative/`, `onboarding/`, `orbital/`, `perf/`, `phantoms/`, `plans/`, `power/`, `process/`, `production/`, and a `player_surface_manifest.json`. Two of these — `gaps/` and `incidents/` — are first-class *expansion feedstock*: directories whose entire purpose is to record what is missing or broken. The Factory Protocol (Part II, step 2) now treats `docs/gaps/` and `docs/incidents/` as mandatory inputs.

> **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
Observed live and not listed in v1.0 Part 5.8: `ECONOMY_FAIRNESS_AUDIT.md`, `ENGINE_SUPPORT_POLICY.md`, `GODOT_MIGRATION_STATUS.md`, `REPO_HISTORY_REWRITE.md`, `HUMAN_AUTHORSHIP.md`, `AI_DISCLOSURE.md`, `ASSET_MIGRATION_LEDGER.md`, `CODEX_SOURCE_MATRIX.md`, `ARCHIVE_INDEX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md`, `SHELTER_MAINTENANCE_MATRIX.md`, `SHELTER_30_DAY_MAINTENANCE_REPORT.md`, `L10N_WAVE2_ROADMAP.md`, `INPUT.md`, `RELEASE_EXPORT.md`, `ENGINE_SUPPORT_POLICY.md`. Of these, `ECONOMY_FAIRNESS_AUDIT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, and `SHELTER_MAINTENANCE_MATRIX.md` are pre-computed balance baselines: they convert Lane C (economy and balance) planning from speculative to evidence-anchored. Subject plans in Lane C must cite these baselines instead of re-deriving numbers.

> **DR-04 — The data catalog inventory has grown; several catalogs are absent from the v1.0 inventory. VERIFIED.**
`Assets/StreamingAssets/Data/` currently holds 342 entries. Catalogs observed live but not present in the v1.0 Part 5.4 inventory include: `dive_sites.json`, `hydroponic_crops.json`, `hydraulic_extrusion_catalog.json`, `metrology_standards_catalog.json`, `muster_camp_scenes.json`, `muster_epilogues.json`, `muster_faction_actions.json`, `muster_faction_culture.json`, `muster_witnesses.json`, `utility_actions.json`, `moral_choice_quests_branching.json`, `moral_choice_quests_distress.json`. Consequence: the duplication firewall (v1.0 Part 5) is stale in these domains; a planner could propose a "new" muster or moral-choice catalog that already exists. The ID-collision sweep in Factory Protocol step 1 must always run against the live listing, never against this document.

> The following v1.0 structures were confirmed by the audit and remain authoritative: the four-tier architecture (Tier 1 data authority in `Assets/StreamingAssets/Data/`; Tier 2 engine-free Core; Tier 3 `src/Host` + `src/UI`; Tier 4 xUnit plus the `HostCli` selftest surface); the `AGENTS.md` non-negotiable rules (Godot authoritative, Core engine-free, JSON authoritative, one authority per concern, focused verification); the narrative corpus under `Assets/StreamingAssets/Data/narrative/` (present in the live listing); the faction, economy, weather, Year-of-Ash, moral-choice, muster, and verdict catalog families (all present live); and the plan-discipline artifacts (`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `SESSION_HANDOFF.md`) at root.

> C1 Shelter operations (rooms, thermal, schedules, fire, decor, barter, noise, prisoners, sanitation, airlock, decon, atmosphere) · C2 Medical pipeline (disease, dose ledger, ARS, surgery, autopsy, pharma, diagnostics, therapies, dependency, crises) · C3 Water, food, agriculture (treatment, condensers, wells, brine, nutrition, kitchen, preservation, grain, greenhouse, crops, aquaponics, apiculture) · C4 Power and industry (grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, coatings, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology) · C5 Expeditions and travel (destinations, scavenging tables, vehicles, waystations, caravans, routes, travel encounters, micro-locations) · C6 Map and geography (wasteland map, damaged zones, fog, route gates, cartography, survey instruments) · C7 Factions and war (stance, doctrines, war chains, tributes, treaties, embargoes, espionage, psyops, infiltration, musters, labor camps, bounties) · C8 Radio and information (stations, programs, intercepts, distress signals, rumors, sound ranging, direction finding, NVIS, heliograph) · C9 Survivors and interiority (needs, skills, traits, arcs, trauma, guilt, therapies, relations, caregiving, beliefs, rituals, memorials, final wishes, lineage, cohorts, apprenticeships) · C10 Quests and moral choice (questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip, branching, bureaucratic morality, expansion quests) · C11 Economy (market, baselines, regional prices, shocks, rumors, black market, debt ledger, tributes, trade screens, tell lines) · C12 Weather and Year of Ash (weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash families, epilogue pressure) · C13 Endgame and epilogue (Reckoning, verdict, epilogue matrix, chronicle, muster epilogues, standing records, census) · C14 Ecology and wildlife (migration, trapping, ecosystem, bestiary, flora, infestations, contagion, pathogens, crop genomes) · C15 Defense and security (perimeter, defense grid, sky defense, ordnance, chemical defense, orbital harrow, interlocks, EMP effects) · C16 Progression and meta (skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, bestiary, L10N, mods, settings, input) · C17 Host surface and UI (panels, shell, focus navigation, snapshots, a11y, briefings, dashboards).

> | Cluster | Opening archetype | Confidence |
|---|---|---|
| C1 | Bureaucratic texture for under-documented rooms: shift notices, maintenance glitch reports, load-shed amendments for rooms lacking corpus coverage | HIGH CONFIDENCE |
| C2 | Casebook and therapy-note expansion for affliction states with thin prose coverage; dose-treatment narrative pairing against `MEDICAL_DOSE_TREATMENT_MATRIX.md` | HIGH CONFIDENCE |
| C3 | Assay/log corpus for preservation and processing chains that have catalogs but no narrative corpus twin (v1.0 Part 16.4 pattern: every process ships technical + prose) | HIGH CONFIDENCE |
| C4 | Same pattern for the newest industrial catalogs confirmed live in DR-04 (`hydraulic_extrusion`, `metrology_standards`): audit records, calibration logs | HIGH CONFIDENCE |
| C5 | Expedition field reports and waypoint notes for destinations with sparse `arrival_description`/`revisit_description` coverage; route-waypoint batches | HIGH CONFIDENCE |
| C6 | Gazetteer entries and damaged-zone survey prose; cartographic marginalia | INFERENCE — verify current coverage |
| C7 | Communiqué, directive, and verdict-corpus expansion for factions with thin public/private language separation | HIGH CONFIDENCE |
| C8 | Radio rundown/transcript batches for stations with thin programming; numbers-station and cipher follow-ups | HIGH CONFIDENCE — but distress-signal content is SEALED under `CF-P1-DISTRESS-CONTENT-SEAL` (DR-06); do not add signal scenarios |
| C9 | Delayed moral-choice callbacks (~100-day returns) via `IFlagLedger` flags; phantom-memory triggers tied to surviving cohorts | HIGH CONFIDENCE (v1.0 Part 7 gap 2) |
| C10 | Quest prose fields (`quest_hook`, `objective_text`, outcome texts) for quest records with skeleton prose; follow Part 9 contracts exactly | HIGH CONFIDENCE |
| C11 | Ledger, statement, and debt-template prose; rumor batches within deterministic bands | HIGH CONFIDENCE |
| C12 | Mid-winter slump pressure (Days 90–180) story arcs; storm-window almanac entries | HIGH CONFIDENCE (v1.0 Part 7 gap 1) |
| C13 | Epilogue-chronicle depth for under-served permutations of the 32-permutation matrix | HIGH CONFIDENCE |
| C14 | Bestiary and natural-history corpus extension; mutated-botanical and limnology follow-on batches | HIGH CONFIDENCE |
| C15 | Defense-log and ordnance-manifest prose; orbital-harrow telemetry transcripts | INFERENCE — verify coverage |
| C16 | Codex and field-guide entries for systems that gained content since the last codex wave | HIGH CONFIDENCE |
| C17 | Ambient environmental text and atmosphere cues for panels rendering newer systems with sparse surface prose | INFERENCE — verify via `--ui-layout-selftest` and snapshot coverage |

> | Cluster | Opening archetype | Confidence |
|---|---|---|
| C1 | Room-level effect extensions routed through `IsRoomPowered`; shelter-failure follow-ons building on the quarantined failure-effects wiring logs observed in `docs/plans/` | HIGH CONFIDENCE |
| C2 | Ward-staffing and recovery-ramp follow-ons are CLOSED (Plan 24, DR-06); open instead: cross-links between medical and cohort/lineage (child health), and between dose ledger and Year-of-Ash fallout windows | PROPOSAL — premise sweep required |
| C3 | Zoonosis-style bridges: kitchen/preservation × disease; cellar-rot × greenhouse economics; apiculture × morale | PROPOSAL |
| C4 | Bind the newest industrial catalogs (DR-04) into consumption/production ledgers through the existing power-grid and foundry seams | PROPOSAL — needs live loader verification |
| C5 | Per-destination scavenging-table parity for destinations beyond the 49-table coverage; vehicle-breakdown consequences into medical and dose ledgers | HIGH CONFIDENCE |
| C6 | Flooded-route topology tags and authored map edges (foreman-flagged open decision — needs the named signature first) | BLOCKED — decision-gated |
| C7 | FactionWar per-strike emitter extension (foreman-flagged open decision — needs signature) | BLOCKED — decision-gated |
| C8 | Radio-signal follow-up chaining is SEALED (DISTRESS-SIGNALS-9-12 COMPLETE, DR-06); open instead: market-rumor band extension and intercept-driven journal depth | HIGH CONFIDENCE |
| C9 | Survivor interiority bridges: belief movements × faction stance; memorial rites × epilogue evidence; chemical dependency × medical ward | PROPOSAL |
| C10 | Quest state reopening after new discoveries (failure-recovery grammar, v1.0 Part 6.7); moral-choice flag consumers beyond the flag ledger | HIGH CONFIDENCE |
| C11 | Black-market funds/goods legs remain decision-gated (canonical funds authority); merchant restock priority is SEALED by DEC-05 (DR-06) | BLOCKED / SEALED |
| C12 | Year-of-Ash tick-window extensions (180–360) for systems not yet producing winter pressure | PROPOSAL |
| C13 | Reckoning evidence enrollment for systems added since the last endgame wave (19A/19B/19C closed, DR-06) | HIGH CONFIDENCE |
| C14 | Trapping→disease zoonosis bridge exists; open: migration × expedition route encounters; infestation × crop economy | PROPOSAL |
| C15 | EMP effects exist (shelter EMP/medical power logs observed); open: defense grid × warlord siege math; sky-armor × orbital harrow telemetry | PROPOSAL |
| C16 | XP Expansion W1 is ACTIVE (DR-06): difficulty-authority consumer binding is the sanctioned open seam in this cluster — extend it, do not parallel it | HIGH CONFIDENCE |
| C17 | Panels rendering stale or missing data for newer systems; verify against `--ui-layout-selftest` before claiming | HIGH CONFIDENCE |

> Anchored by the live baselines (DR-03): `ECONOMY_FAIRNESS_AUDIT.md`, `ECONOMY_PRICE_FACTOR_MATRIX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `docs/balance/`.

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

Current evidence and safe integration boundary for Plan 58: Narrative Encounters, Choice Resolution and Expedition Reachability.

- **multi-file encounter load, source stamping and dedupe** remains with `NarrativeEncounterCatalogLoader` at `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`. Static catalog composition.
- **selection, resolution, depletion, pending and history** remains with `NarrativeEncounterSystem` at `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`. Sole encounter runtime owner.
- **expedition/travel encounter integration** remains with `ExpeditionEncounterBridge` at `Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs`. Current cross-system bridge.
- **encounter persistence adapter** remains with `TravelEncounterSaveStore` at `src/Host/TravelEncounterSaveStore.cs`. Existing save seam.
- **source/reachability evidence** remains with `Content certification` at `src/Main.ContentCertification.cs`. Does not become gameplay authority.
- **selection, save, catalog and checksum contracts** remains with `NarrativeEncounterSystemTests` at `Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs`. Focused evidence.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load and provenance-stamp all current encounter sources
2. deduplicate by current ID rule
3. apply stealth/speed/danger/location eligibility
4. select through seeded weighting
5. record selection and pending state
6. resolve one valid choice and route effects
7. capture/restore encounter state

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Encounter definitions are immutable and source-tagged.
- NarrativeEncounterState owns history, pending and depleted IDs.
- Quest state remains with QuestlineSystem/quest owners.
- A choice consequence must route to its current owner rather than an encounter-local economy.

- Only valid current choices resolve.
- Depleting choices persist one time.
- Duplicate source IDs are handled by current primary-wins policy.
- A source file cannot write quest or inventory state directly.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/TravelEncounterSaveStore.cs
- src/Main.ContentCertification.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs

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
| S-01 | 58-01 16 base load | load and provenance-stamp all current encounter sources | Encounter definitions are immutable and source-tagged. | Duplicate source rows silently replace authored content. | NarrativeEncounterCatalogLoader |
| S-02 | 58-02 29 expansion load | deduplicate by current ID rule | NarrativeEncounterState owns history, pending and depleted IDs. | A depleted encounter reappears. | NarrativeEncounterCatalogLoader |
| S-03 | 58-03 source provenance | apply stealth/speed/danger/location eligibility | Quest state remains with QuestlineSystem/quest owners. | A choice writes to the wrong owner. | NarrativeEncounterCatalogLoader |
| S-04 | 58-04 dedupe primary wins | select through seeded weighting | A choice consequence must route to its current owner rather than an encounter-local economy. | A source file is treated as a separate runtime. | NarrativeEncounterCatalogLoader |
| S-05 | 58-05 stealth modifier | record selection and pending state | Encounter definitions are immutable and source-tagged. | A new save store duplicates history. | NarrativeEncounterCatalogLoader |
| S-06 | 58-06 speed modifier | resolve one valid choice and route effects | NarrativeEncounterState owns history, pending and depleted IDs. | Duplicate source rows silently replace authored content. | NarrativeEncounterCatalogLoader |
| S-07 | 58-07 location gate | capture/restore encounter state | Quest state remains with QuestlineSystem/quest owners. | A depleted encounter reappears. | NarrativeEncounterCatalogLoader |
| S-08 | 58-08 choice resolution | load and provenance-stamp all current encounter sources | A choice consequence must route to its current owner rather than an encounter-local economy. | A choice writes to the wrong owner. | NarrativeEncounterCatalogLoader |
| S-09 | 58-09 depletion | deduplicate by current ID rule | Encounter definitions are immutable and source-tagged. | A source file is treated as a separate runtime. | NarrativeEncounterCatalogLoader |
| S-10 | 58-10 save replay | apply stealth/speed/danger/location eligibility | NarrativeEncounterState owns history, pending and depleted IDs. | A new save store duplicates history. | NarrativeEncounterCatalogLoader |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 58-TC-01 schema/count | data | schema/count; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeEncounterCatalogLoader |
| T-02 | 58-TC-02 source provenance | unit | source provenance; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeEncounterCatalogLoader |
| T-03 | 58-TC-03 dedupe | persistence | dedupe; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeEncounterCatalogLoader |
| T-04 | 58-TC-04 eligibility | determinism | eligibility; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeEncounterCatalogLoader |
| T-05 | 58-TC-05 selection determinism | host | selection determinism; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeEncounterCatalogLoader |
| T-06 | 58-TC-06 choice validity | UI/accessibility | choice validity; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeEncounterCatalogLoader |
| T-07 | 58-TC-07 depletion once | cross-system | depletion once; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeEncounterCatalogLoader |
| T-08 | 58-TC-08 save round trip | data | save round trip; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeEncounterCatalogLoader |
| T-09 | 58-TC-09 bridge handoff | unit | bridge handoff; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeEncounterCatalogLoader |
| T-10 | 58-TC-10 UI state | persistence | UI state; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeEncounterCatalogLoader |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 23 | `Ashfall.Core.Tests/ExpeditionEncounterBridgeTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 19 | `src/Host/ContentUtilizationRuntimeCollector.cs` | current reference count; inspect the caller before treating it as a live route |
| 16 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 14 | `Ashfall.Core.Tests/Expeditions/MicroLocationLifecycleSmokeTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 13 | `src/Host/HostCli.PanelTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 11 | `Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 11 | `Ashfall.Core.Tests/PatrolEncounterIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 11 | `src/Host/ExpeditionHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/MicroLocationCatalogLoaderTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/MicroLocationPersistenceWaveTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/NarrativeEncounterResolutionResultTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/PatrolExpeditionReachabilityTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/Expeditions/MicroLocationRegressionMatrixTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/MicroLocationDeterminismHarness.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/MicroLocationDeterminismTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/MicroLocationHazardIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/MicroLocationIntegrationDeterminismTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/NarrativeEncounterDepletionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `src/UI/ExpeditionPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/MicroLocationEthicsIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/MicroLocationWaterIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/MicroLocationWorldFlagTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/NpcArcDataTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/NarrativeHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Main.UiTests.Expeditions.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/ContentUtilizationGraphTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/MicroLocationGreenhouseIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/MicroLocationRadioIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/HostCliRegistry.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Host/HostCli.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Main.Expeditions.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/ContentDeepChainGateTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/MicroLocationEconomyAuditTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/MicroLocationUtilizationAuditTests.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/narrative_encounters.json`

### `Assets/StreamingAssets/Data/narrative_encounters.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 23144; characters: 23138.
- SHA-256: `908b57d03bdf04e3308587eef5700dcc432c37ab082163497627a710a30eb0b3`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `encounters`

#### `encounters` — 16 current rows

- Row 001 `enc_dead_letter_office`: `{"baseWeight":2.0,"category":"Discovery","choices":[{"choiceId":"read_letters","guiltDelta":0,"moraleDelta":3,"text":"Read the scattered letters."},{"choiceId":"deliver_letter","guiltDelta":0,"moraleDelta":5,"text":"Take the sealed envelop…`
- Row 002 `enc_weather_station`: `{"baseWeight":3.0,"category":"Discovery","choices":[{"choiceId":"extract_data","guiltDelta":0,"moraleDelta":4,"text":"Spool the telemetry data to tape."},{"choiceId":"take_solar_panel","guiltDelta":1,"moraleDelta":2,"text":"Strip the solar…`
- Row 003 `enc_pianist`: `{"baseWeight":1.5,"category":"Social","choices":[{"choiceId":"listen","guiltDelta":0,"moraleDelta":4,"text":"Sit on the rubble and listen until he stops."},{"choiceId":"share_food","guiltDelta":0,"moraleDelta":5,"text":"Leave a portion of …`
- Row 004 `enc_census_carrier_on_the_road`: `{"baseWeight":1.6,"category":"Social","choices":[{"choiceId":"census_carrier_bury_and_take","guiltDelta":0,"moraleDelta":3,"text":"Bury him under the culvert with the book facing up. Take the route home."},{"choiceId":"census_carrier_take_…`
- Row 005 `enc_ninth_night_listener`: `{"baseWeight":1.4,"category":"Social","choices":[{"choiceId":"ninth_night_compare_sheets","guiltDelta":0,"moraleDelta":4,"text":"Put your transcripts beside hers and find the night they differ."},{"choiceId":"ninth_night_take_her_wall","gu…`
- Row 006 `enc_the_tower_classroom`: `{"baseWeight":1.5,"category":"Social","choices":[{"choiceId":"tower_classroom_give_batteries","guiltDelta":0,"moraleDelta":4,"text":"Give them batteries. A lesson that stops is the only kind of silence that carries."},{"choiceId":"tower_cl…`
- Row 007 `enc_glass_blower_of_the_rim`: `{"baseWeight":1.3,"category":"Discovery","choices":[{"choiceId":"glass_blower_buy_a_lens","guiltDelta":0,"moraleDelta":3,"text":"Buy a lens and ask nothing about the rod."},{"choiceId":"glass_blower_warn_him","guiltDelta":0,"moraleDelta":3…`
- Row 008 `enc_the_surveyor_still_working`: `{"baseWeight":1.4,"category":"Social","choices":[{"choiceId":"surveyor_walk_the_line","guiltDelta":0,"moraleDelta":4,"text":"Hold the chain for a day. Walk the line to the stone and set the stone."},{"choiceId":"surveyor_ask_him_to_teach",…`
- Row 009 `enc_the_orchard_with_stakes`: `{"baseWeight":1.2,"category":"Hazard","choices":[{"choiceId":"orchard_trade_honestly","guiltDelta":0,"moraleDelta":3,"text":"Ask, and trade for what he is willing to give."},{"choiceId":"orchard_take_the_inner_rows","guiltDelta":3,"moraleD…`
- Row 010 `enc_ferryman_toll_of_stories`: `{"baseWeight":1.5,"category":"Social","choices":[{"choiceId":"ferryman_pay_a_true_story","guiltDelta":0,"moraleDelta":5,"text":"Pay the toll. Tell him the one you have not told at home."},{"choiceId":"ferryman_pay_a_lie","guiltDelta":3,"mo…`
- Row 011 `enc_dog_at_the_sealed_door`: `{"baseWeight":1.3,"category":"Discovery","choices":[{"choiceId":"sealed_door_feed_and_leave","guiltDelta":0,"moraleDelta":3,"text":"Feed the dog and go. The keeping is not yours to take over."},{"choiceId":"sealed_door_take_the_dog","guilt…`
- Row 012 `enc_quarry_witness`: `{"baseWeight":1.4,"category":"Social","choices":[{"choiceId":"quarry_witness_bring_her_in","guiltDelta":0,"moraleDelta":3,"text":"Bring her in. Let the shelter hold three versions of one night instead of two."},{"choiceId":"quarry_witness_…`
- Row 013 `enc_grave_with_the_wrong_name`: `{"baseWeight":1.3,"category":"Discovery","choices":[{"choiceId":"wrong_name_rebury_and_leave","guiltDelta":0,"moraleDelta":3,"text":"Push the corner back in and leave the name exactly as it was cut."},{"choiceId":"wrong_name_correct_the_st…`
- Row 014 `enc_trap_bait_stolen`: `{"baseWeight":1.0,"category":"Social","choices":[{"choiceId":"follow_prints","guiltDelta":0,"moraleDelta":-2,"text":"Follow the prints toward the drainage cut."},{"choiceId":"reset_only","guiltDelta":1,"moraleDelta":0,"text":"Re-bait the t…`
- Row 015 `enc_trap_tampered`: `{"baseWeight":1.0,"category":"Hazard","choices":[{"choiceId":"watch_night","guiltDelta":0,"moraleDelta":-3,"text":"Sit the night cold at the treeline and watch the trap."},{"choiceId":"leave_warning","guiltDelta":0,"moraleDelta":1,"text":"…`
- Row 016 `enc_trap_stranger_discovery`: `{"baseWeight":1.0,"category":"Social","choices":[{"choiceId":"share_method","guiltDelta":0,"moraleDelta":3,"text":"Show them how the snare sets, then send them off."},{"choiceId":"warn_off","guiltDelta":2,"moraleDelta":-1,"text":"Order the…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`

### `Assets/StreamingAssets/Data/narrative_encounters_expansion.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 39422; characters: 39410.
- SHA-256: `6fc1c3a4c6150ab6b29003a27821e81f6d0cf3ed8930d4801976dcb75298bc67`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `encounters`

#### `encounters` — 29 current rows

- Row 001 `enc_overturned_postal`: `{"baseWeight":2.0,"category":"Discovery","choices":[{"choiceId":"read_the_letters","guiltDelta":0,"moraleDelta":3,"text":"Read the scattered letters."},{"choiceId":"carry_the_envelope","guiltDelta":0,"moraleDelta":5,"text":"Take the sealed…`
- Row 002 `enc_weather_station`: `{"baseWeight":2.0,"category":"Discovery","choices":[{"choiceId":"extract_the_data","guiltDelta":0,"moraleDelta":4,"text":"Spool the telemetry data to tape."},{"choiceId":"take_the_solar_panel","guiltDelta":2,"moraleDelta":2,"text":"Strip t…`
- Row 003 `enc_pianist`: `{"baseWeight":1.5,"category":"Observation","choices":[{"choiceId":"listen","guiltDelta":0,"moraleDelta":4,"text":"Sit and listen. Wait for the fourth note."},{"choiceId":"share_food","guiltDelta":0,"moraleDelta":5,"text":"Leave a portion o…`
- Row 004 `enc_roadside_trader`: `{"baseWeight":3.0,"category":"Trade","choices":[{"choiceId":"fair_trade","guiltDelta":1,"moraleDelta":2,"text":"Trade medical supplies for the salt and tins."},{"choiceId":"flashlight_only","guiltDelta":0,"moraleDelta":3,"text":"Trade only…`
- Row 005 `enc_water_seller`: `{"baseWeight":2.5,"category":"Trade","choices":[{"choiceId":"pay_with_names","guiltDelta":3,"moraleDelta":2,"text":"Give him the coordinates of the settlements you know."},{"choiceId":"pay_with_nothing","guiltDelta":2,"moraleDelta":1,"text…`
- Row 006 `enc_false_broadcast`: `{"baseWeight":2.0,"category":"Misinformation","choices":[{"choiceId":"investigate","guiltDelta":2,"moraleDelta":-1,"text":"Go to the school to verify the broadcast."},{"choiceId":"rebroadcast","guiltDelta":0,"moraleDelta":3,"text":"Rebroad…`
- Row 007 `enc_two_camps`: `{"baseWeight":2.0,"category":"Misinformation","choices":[{"choiceId":"ridge_figure","guiltDelta":3,"moraleDelta":0,"text":"Trust the ridge scout. Approach the cold camp."},{"choiceId":"smoke_camp","guiltDelta":3,"moraleDelta":0,"text":"Tru…`
- Row 008 `enc_sick_child`: `{"baseWeight":2.5,"category":"Rescue","choices":[{"choiceId":"carry_to_shelter","guiltDelta":0,"moraleDelta":5,"text":"Wrap the child in a blanket. Carry them to your clinic."},{"choiceId":"search_house","guiltDelta":2,"moraleDelta":2,"tex…`
- Row 009 `enc_injured_scavenger`: `{"baseWeight":2.5,"category":"Rescue","choices":[{"choiceId":"free_him","guiltDelta":0,"moraleDelta":5,"text":"Rig a fulcrum. Free him and split the pack."},{"choiceId":"free_take_pack","guiltDelta":3,"moraleDelta":1,"text":"Free him, but …`
- Row 010 `enc_roof_access`: `{"baseWeight":1.5,"category":"Structural","choices":[{"choiceId":"climb_for_beacon","guiltDelta":2,"moraleDelta":2,"text":"Risk the climb. Vault the missing step."},{"choiceId":"call_up","guiltDelta":0,"moraleDelta":1,"text":"Call up the s…`
- Row 011 `enc_cold_storage`: `{"baseWeight":1.5,"category":"Structural","choices":[{"choiceId":"cut_and_enter","guiltDelta":3,"moraleDelta":1,"text":"Cut the new chain with bolt cutters and enter."},{"choiceId":"knock","guiltDelta":1,"moraleDelta":2,"text":"Pound on th…`
- Row 012 `enc_hot_sign`: `{"baseWeight":2.0,"category":"Radiation","choices":[{"choiceId":"rush_the_box","guiltDelta":3,"moraleDelta":1,"text":"Sprint into the hotspot, grab the box, and retreat."},{"choiceId":"mark_and_leave","guiltDelta":0,"moraleDelta":2,"text":…`
- Row 013 `enc_clean_well`: `{"baseWeight":1.5,"category":"Radiation","choices":[{"choiceId":"drink_anyway","guiltDelta":2,"moraleDelta":2,"text":"Drink the water. You need the hydration."},{"choiceId":"test_yourself","guiltDelta":0,"moraleDelta":3,"text":"Use a chemi…`
- Row 014 `enc_whiteout_traveler`: `{"baseWeight":2.0,"category":"Weather","choices":[{"choiceId":"correct_them","guiltDelta":1,"moraleDelta":3,"text":"Tell them the truth about the town. Offer them an emergency blanket."},{"choiceId":"let_them_go","guiltDelta":3,"moraleDelt…`
- Row 015 `enc_flood_road`: `{"baseWeight":2.5,"category":"Weather","choices":[{"choiceId":"wade_to_car","guiltDelta":1,"moraleDelta":5,"text":"Wade into the freezing current. Haul the child out the window."},{"choiceId":"throw_rope","guiltDelta":1,"moraleDelta":4,"te…`
- Row 016 `enc_dogs_silent`: `{"baseWeight":2.0,"category":"Fear","choices":[{"choiceId":"back_away","guiltDelta":0,"moraleDelta":2,"text":"Back away slowly. Do not turn your back. Do not run."},{"choiceId":"call_out","guiltDelta":2,"moraleDelta":1,"text":"Announce you…`
- Row 017 `enc_following_footsteps`: `{"baseWeight":1.5,"category":"Fear","choices":[{"choiceId":"stop_and_wait","guiltDelta":1,"moraleDelta":2,"text":"Stop dead. Draw your weapon. Wait for them to appear."},{"choiceId":"speak_aloud","guiltDelta":0,"moraleDelta":3,"text":"Call…`
- Row 018 `enc_border_camp`: `{"baseWeight":2.0,"category":"Faction","choices":[{"choiceId":"pass_quickly","guiltDelta":1,"moraleDelta":2,"text":"Move through the open checkpoint at a sprint."},{"choiceId":"sign_logbook","guiltDelta":2,"moraleDelta":1,"text":"Sign the …`
- Row 019 `enc_two_factions_trade`: `{"baseWeight":2.0,"category":"Faction","choices":[{"choiceId":"witness_only","guiltDelta":0,"moraleDelta":3,"text":"Act only as a witness. Do not touch the blanket."},{"choiceId":"touch_blanket","guiltDelta":3,"moraleDelta":2,"text":"Touch…`
- Row 020 `enc_locked_room`: `{"baseWeight":1.5,"category":"Mystery","choices":[{"choiceId":"open_it","guiltDelta":2,"moraleDelta":2,"text":"Turn the key and open the door."},{"choiceId":"knock_first","guiltDelta":0,"moraleDelta":3,"text":"Pound on the steel door. List…`
- Row 021 `enc_two_graves`: `{"baseWeight":1.5,"category":"Mystery","choices":[{"choiceId":"dig_one","guiltDelta":5,"moraleDelta":-1,"text":"Exhume one of the graves to see what's inside."},{"choiceId":"leave_flowers","guiltDelta":0,"moraleDelta":3,"text":"Leave a han…`
- Row 022 `enc_beggars`: `{"baseWeight":2.5,"category":"Ethical","choices":[{"choiceId":"share_a_meal","guiltDelta":1,"moraleDelta":4,"text":"Authorize the release of three day-rations from the stockpile."},{"choiceId":"turn_away","guiltDelta":4,"moraleDelta":-1,"t…`
- Row 023 `enc_thief_child`: `{"baseWeight":2.0,"category":"Ethical","choices":[{"choiceId":"feed_and_release","guiltDelta":0,"moraleDelta":4,"text":"Give the child a full ration and release them."},{"choiceId":"return_to_parent","guiltDelta":1,"moraleDelta":3,"text":"…`
- Row 024 `enc_library_cache`: `{"baseWeight":2.0,"category":"Discovery","choices":[{"choiceId":"use_and_replenish","guiltDelta":0,"moraleDelta":4,"text":"Take the iodine, leave surplus ammunition, and sign the ledger."},{"choiceId":"take_water","guiltDelta":1,"moraleDel…`
- Row 025 `enc_greenhouse_keeper`: `{"baseWeight":2.0,"category":"Observation","choices":[{"choiceId":"seedlings_for_salt","guiltDelta":0,"moraleDelta":4,"text":"Trade mineral salt for the seedlings."},{"choiceId":"labor_for_seedlings","guiltDelta":0,"moraleDelta":3,"text":"…`
- Row 026 `enc_dead_radio_operator`: `{"baseWeight":1.5,"category":"Observation","choices":[{"choiceId":"turn_it_off","guiltDelta":1,"moraleDelta":3,"text":"Cut the power to the broadcast. Bury the operator."},{"choiceId":"keep_it_running","guiltDelta":1,"moraleDelta":2,"text"…`
- Row 027 `enc_ice_fishermen`: `{"baseWeight":2.0,"category":"Observation","choices":[{"choiceId":"warn_them","guiltDelta":2,"moraleDelta":4,"text":"Walk onto the black ice and drag them back to the white."},{"choiceId":"call_from_shore","guiltDelta":1,"moraleDelta":3,"t…`
- Row 028 `enc_quarantine_sign`: `{"baseWeight":2.0,"category":"Mystery","choices":[{"choiceId":"enter","guiltDelta":3,"moraleDelta":1,"text":"Kick the door wide and clear the house."},{"choiceId":"call_through_door","guiltDelta":0,"moraleDelta":3,"text":"Stand back and ca…`
- Row 029 `enc_last_train`: `{"baseWeight":1.5,"category":"Discovery","choices":[{"choiceId":"search_train","guiltDelta":1,"moraleDelta":2,"text":"Sweep the passenger cars for abandoned luggage."},{"choiceId":"take_power","guiltDelta":2,"moraleDelta":1,"text":"Strip t…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/narrative_encounters_npc_arcs.json`

### `Assets/StreamingAssets/Data/narrative_encounters_npc_arcs.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 40979; characters: 40955.
- SHA-256: `feb0c3fe2dec2b19a70e15e0b89ec478e6bd5d1cebf08c5f245c9b837a43b26b`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `encounters`

#### `encounters` — 31 current rows

- Row 001 `enc_arc_mara_waystation`: `{"baseWeight":2.5,"category":"Trade","choices":[{"choiceId":"mara_share_medicine","completesQuestChoiceId":"mara_help","completesQuestId":"quest_arc_mara_01_waystation","guiltDelta":0,"moraleDelta":2,"text":"Trade fair — medicine and axle …`
- Row 002 `enc_arc_mara_route`: `{"baseWeight":2.5,"category":"Discovery","choices":[{"choiceId":"mara_pull_her_out","completesQuestChoiceId":"mara_rescue","completesQuestId":"quest_arc_mara_02_route","guiltDelta":0,"moraleDelta":3,"text":"Spend the water and the day — pu…`
- Row 003 `enc_arc_ilze_clinic`: `{"baseWeight":2.5,"category":"Social","choices":[{"choiceId":"ilze_restock_the_shelves","completesQuestChoiceId":"ilze_supply","completesQuestId":"quest_arc_ilze_01_clinic","guiltDelta":0,"moraleDelta":3,"text":"Restock the clinic at cost …`
- Row 004 `enc_arc_ilze_outbreak`: `{"baseWeight":3.0,"category":"Hazard","choices":[{"choiceId":"ilze_take_her_out","completesQuestChoiceId":"ilze_extract","completesQuestId":"quest_arc_ilze_02_outbreak","guiltDelta":2,"moraleDelta":2,"text":"Come for her: bring her out bef…`
- Row 005 `enc_arc_marek_patrol`: `{"baseWeight":2.5,"category":"Social","choices":[{"choiceId":"marek_honor_the_post","completesQuestChoiceId":"marek_cooperative","completesQuestId":"quest_arc_marek_01_patrol","guiltDelta":0,"moraleDelta":1,"text":"Honor the posted terms a…`
- Row 006 `enc_arc_marek_deserter`: `{"baseWeight":3.0,"category":"Social","choices":[{"choiceId":"marek_provision_him","completesQuestChoiceId":"marek_provisions","completesQuestId":"quest_arc_marek_02_deserter","guiltDelta":0,"moraleDelta":2,"text":"Provision him quiet and …`
- Row 007 `enc_arc_lina_baths`: `{"baseWeight":2.5,"category":"Social","choices":[{"choiceId":"lina_offer_a_place","completesQuestChoiceId":"lina_take_in","completesQuestId":"quest_arc_lina_01_found","guiltDelta":0,"moraleDelta":3,"text":"Offer her a place in the shelter …`
- Row 008 `enc_arc_anete_substation`: `{"baseWeight":2.5,"category":"Discovery","choices":[{"choiceId":"anete_fill_the_list","completesQuestChoiceId":"anete_supply_parts","completesQuestId":"quest_arc_anete_01_substation","guiltDelta":0,"moraleDelta":3,"text":"Fill the parts li…`
- Row 009 `enc_arc_anete_feeder`: `{"baseWeight":2.5,"category":"Discovery","choices":[{"choiceId":"anete_back_the_rebuild","completesQuestChoiceId":"anete_back_the_project","completesQuestId":"quest_arc_anete_02_project","guiltDelta":0,"moraleDelta":3,"text":"Back the rebu…`
- Row 010 `enc_arc_sava_camp`: `{"baseWeight":2.5,"category":"Social","choices":[{"choiceId":"sava_stand_the_fence","completesQuestChoiceId":"sava_defend","completesQuestId":"quest_arc_sava_01_camp","guiltDelta":0,"moraleDelta":4,"text":"Stand the fence with the camp thr…`
- Row 011 `enc_arc_sava_schism`: `{"baseWeight":2.5,"category":"Social","choices":[{"choiceId":"sava_vouch_for_her","completesQuestChoiceId":"sava_vouch_mediator","completesQuestId":"quest_arc_sava_02_schism","guiltDelta":0,"moraleDelta":3,"text":"Vouch for her as mediator…`
- Row 012 `enc_arc_rika_cache`: `{"baseWeight":2.5,"category":"Trade","choices":[{"choiceId":"rika_take_the_split","completesQuestChoiceId":"rika_split","completesQuestId":"quest_arc_rika_01_cache","guiltDelta":0,"moraleDelta":2,"text":"Take the split. Work the floor toge…`
- Row 013 `enc_arc_rika_trap`: `{"baseWeight":3.0,"category":"Social","choices":[{"choiceId":"rika_carry_her_out","completesQuestChoiceId":"rika_rescue","completesQuestId":"quest_arc_rika_02_trap","guiltDelta":0,"moraleDelta":4,"text":"Carry her out and stand the camp wa…`
- Row 014 `enc_arc_liva_tower`: `{"baseWeight":2.5,"category":"Trade","choices":[{"choiceId":"liva_trade_cells","completesQuestChoiceId":"liva_cells","completesQuestId":"quest_arc_liva_01_tower","guiltDelta":0,"moraleDelta":2,"text":"Trade power cells for a standing relay…`
- Row 015 `enc_arc_liva_survey`: `{"baseWeight":3.0,"category":"Social","choices":[{"choiceId":"liva_move_her_first","completesQuestChoiceId":"liva_warn","completesQuestId":"quest_arc_liva_02_seizure","guiltDelta":0,"moraleDelta":3,"text":"Warn her and move her gear before…`
- Row 016 `enc_arc_oskar_01_debt`: `{"baseWeight":2.0,"category":"Social","choices":[{"choiceId":"oskar_settle","completesQuestChoiceId":"oskar_settle","completesQuestId":"quest_arc_oskar_01_debt","guiltDelta":0,"moraleDelta":2,"text":"Front the buyout at fair interest, in w…`
- Row 017 `enc_arc_tomas_01_rounding`: `{"baseWeight":2.0,"category":"Social","choices":[{"choiceId":"tomas_vouch","completesQuestChoiceId":"tomas_vouch","completesQuestId":"quest_arc_tomas_01_rounding","guiltDelta":0,"moraleDelta":2,"text":"Vouch for the rounding when the audit…`
- Row 018 `enc_arc_joren_01_plant`: `{"baseWeight":2.0,"category":"Social","choices":[{"choiceId":"joren_crews","completesQuestChoiceId":"joren_crews","completesQuestId":"quest_arc_joren_01_plant","guiltDelta":0,"moraleDelta":2,"text":"Commit trained crews to his checklists a…`
- Row 019 `enc_arc_pavel_01_map`: `{"baseWeight":2.0,"category":"Social","choices":[{"choiceId":"pavel_buy_map","completesQuestChoiceId":"pavel_buy_map","completesQuestId":"quest_arc_pavel_01_map","guiltDelta":0,"moraleDelta":2,"text":"Buy the copy at his price and ask noth…`
- Row 020 `enc_arc_sena_01_log`: `{"baseWeight":2.0,"category":"Social","choices":[{"choiceId":"sena_countersign","completesQuestChoiceId":"sena_countersign","completesQuestId":"quest_arc_sena_01_log","guiltDelta":0,"moraleDelta":2,"text":"Countersign the log page where th…`
- Row 021 `enc_arc_dalia_01_annex`: `{"baseWeight":2.0,"category":"Social","choices":[{"choiceId":"dalia_haul_roof","completesQuestChoiceId":"dalia_haul_roof","completesQuestId":"quest_arc_dalia_01_annex","guiltDelta":0,"moraleDelta":2,"text":"Haul timber and re-sack the lots…`
- Row 022 `enc_arc_anton_01_herd`: `{"baseWeight":2.0,"category":"Social","choices":[{"choiceId":"anton_cull_quietly","completesQuestChoiceId":"anton_cull_quietly","completesQuestId":"quest_arc_anton_01_herd","guiltDelta":0,"moraleDelta":2,"text":"Do the cull arithmetic with…`
- Row 023 `enc_arc_emil_01_numbers`: `{"baseWeight":2.0,"category":"Social","choices":[{"choiceId":"emil_fund_notebooks","completesQuestChoiceId":"emil_fund_notebooks","completesQuestId":"quest_arc_emil_01_numbers","guiltDelta":0,"moraleDelta":2,"text":"Fund the fifth notebook…`
- Row 024 `enc_arc_nadia_01_pot`: `{"baseWeight":2.0,"category":"Social","choices":[{"choiceId":"nadia_front_the_oil","completesQuestChoiceId":"nadia_front_the_oil","completesQuestId":"quest_arc_nadia_01_pot","guiltDelta":0,"moraleDelta":2,"text":"Front the kitchen oil and …`
- Row 025 `enc_arc_arvo_01_crate`: `{"baseWeight":2.0,"category":"Social","choices":[{"choiceId":"arvo_amnesty","completesQuestChoiceId":"arvo_amnesty","completesQuestId":"quest_arc_arvo_01_crate","guiltDelta":0,"moraleDelta":2,"text":"Offer amnesty: the crate goes to the me…`
- Row 026 `enc_arc_kaspar_01_bearing`: `{"baseWeight":2.0,"category":"Social","choices":[{"choiceId":"kaspar_parts_upfront","completesQuestChoiceId":"kaspar_parts_upfront","completesQuestId":"quest_arc_kaspar_01_bearing","guiltDelta":0,"moraleDelta":2,"text":"Buy the parts upfro…`
- Row 027 `enc_arc_mira_01_intervals`: `{"baseWeight":2.0,"category":"Social","choices":[{"choiceId":"mira_winder_parts","completesQuestChoiceId":"mira_winder_parts","completesQuestId":"quest_arc_mira_01_intervals","guiltDelta":0,"moraleDelta":2,"text":"Bring the brush set and c…`
- Row 028 `enc_arc_janek_01_dawn`: `{"baseWeight":2.0,"category":"Social","choices":[{"choiceId":"janek_walk_dawn","completesQuestChoiceId":"janek_walk_dawn","completesQuestId":"quest_arc_janek_01_dawn","guiltDelta":0,"moraleDelta":2,"text":"Walk the dawn routes with him and…`
- Row 029 `enc_arc_veda_01_circuit`: `{"baseWeight":2.0,"category":"Social","choices":[{"choiceId":"veda_mark_the_lines","completesQuestChoiceId":"veda_mark_the_lines","completesQuestId":"quest_arc_veda_01_circuit","guiltDelta":0,"moraleDelta":2,"text":"Buy tag paint and wire,…`
- Row 030 `enc_arc_mirael_01_page`: `{"baseWeight":2.0,"category":"Social","choices":[{"choiceId":"mirael_sit_the_lessons","completesQuestChoiceId":"mirael_sit_the_lessons","completesQuestId":"quest_arc_mirael_01_page","guiltDelta":0,"moraleDelta":2,"text":"Sit the lessons an…`
- Row 031 `enc_arc_niko_01_corridors`: `{"baseWeight":2.0,"category":"Social","choices":[{"choiceId":"niko_vouch_employment","completesQuestChoiceId":"niko_vouch_employment","completesQuestId":"quest_arc_niko_01_corridors","guiltDelta":0,"moraleDelta":2,"text":"Vouch him runner'…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/micro_locations.json`

### `Assets/StreamingAssets/Data/micro_locations.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 35356; characters: 35344.
- SHA-256: `8d6098ece0060ef012e5f3871c324f606ca520a844c14da2ed614187b80820a0`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `collection_id`, `encounters`

#### `encounters` — 28 current rows

- Row 001 `micro_roadside_memorial`: `{"baseWeight":0.8,"category":"Discovery","choices":[{"choiceId":"leave_memorial","guiltDelta":0,"moraleDelta":1,"text":"Leave it untouched."},{"choiceId":"take_offering","depletesOnResolve":true,"grantItemId":"cloth","grantItemQuantity":1,…`
- Row 002 `micro_crashed_truck`: `{"baseWeight":0.6,"category":"Discovery","choices":[{"choiceId":"search_truck_cargo","depletesOnResolve":true,"grantItemId":"canned_food","grantItemQuantity":2,"guiltDelta":0,"moraleDelta":1,"text":"Search the split crate and cab for salva…`
- Row 003 `micro_frozen_bus`: `{"baseWeight":0.5,"category":"Discovery","choices":[{"choiceId":"search_bus_luggage","depletesOnResolve":true,"grantItemId":"bandage","grantItemQuantity":2,"guiltDelta":3,"moraleDelta":-2,"text":"Search the luggage rack for supplies."},{"c…`
- Row 004 `micro_improvised_grave`: `{"baseWeight":0.7,"category":"Discovery","choices":[{"choiceId":"respect_grave","guiltDelta":0,"moraleDelta":2,"text":"Pay respects and move on."},{"choiceId":"inspect_grave_marker","guiltDelta":0,"journalUnlockId":"micro_improvised_grave_…`
- Row 005 `micro_collapsed_bridge`: `{"baseWeight":0.4,"category":"Hazard","choices":[{"choiceId":"search_bridge_vehicle","depletesOnResolve":true,"grantItemId":"fuel","grantItemQuantity":2,"guiltDelta":0,"moraleDelta":0,"text":"Climb down to the wedged vehicle and search the…`
- Row 006 `micro_drainage_pipe`: `{"baseWeight":0.7,"category":"Discovery","choices":[{"choiceId":"crawl_pipe","depletesOnResolve":true,"grantItemId":"cloth","grantItemQuantity":2,"guiltDelta":1,"moraleDelta":0,"text":"Crawl inside and check the blankets for supplies."},{"…`
- Row 007 `micro_rail_siding`: `{"baseWeight":0.5,"category":"Discovery","choices":[{"choiceId":"search_rail_car","depletesOnResolve":true,"grantItemId":"mechanical_parts","grantItemQuantity":3,"guiltDelta":0,"moraleDelta":0,"text":"Search the freight car for industrial …`
- Row 008 `micro_dead_livestock`: `{"baseWeight":0.6,"category":"Hazard","choices":[{"choiceId":"scavenge_livestock","depletesOnResolve":true,"grantItemId":"cloth","grantItemQuantity":2,"guiltDelta":1,"moraleDelta":-2,"setWorldFlag":"micro_contamination_exposure","text":"Sc…`
- Row 009 `micro_ruined_greenhouse`: `{"baseWeight":0.5,"category":"Discovery","choices":[{"choiceId":"take_greenhouse_seeds","depletesOnResolve":true,"grantItemId":"seed_packets","grantItemQuantity":2,"guiltDelta":0,"moraleDelta":1,"text":"Take the labeled seed trays."},{"cho…`
- Row 010 `micro_shell_crater`: `{"baseWeight":0.4,"category":"Hazard","choices":[{"choiceId":"inspect_crater","depletesOnResolve":true,"grantItemId":"scrap_metal","grantItemQuantity":2,"guiltDelta":0,"moraleDelta":0,"text":"Carefully inspect the crater edge for salvage."…`
- Row 011 `micro_field_kitchen`: `{"baseWeight":0.6,"category":"Discovery","choices":[{"choiceId":"search_kitchen","depletesOnResolve":true,"grantItemId":"canned_food","grantItemQuantity":1,"guiltDelta":0,"moraleDelta":1,"text":"Search the kitchen area for preserved food o…`
- Row 012 `micro_abandoned_generator`: `{"baseWeight":0.4,"category":"Discovery","choices":[{"choiceId":"strip_generator","depletesOnResolve":true,"grantItemId":"electronic_scrap","grantItemQuantity":3,"guiltDelta":0,"moraleDelta":0,"text":"Strip the generator for electrical com…`
- Row 013 `micro_shrine`: `{"baseWeight":0.7,"category":"Social","choices":[{"choiceId":"leave_shrine","guiltDelta":0,"moraleDelta":2,"text":"Leave the shrine undisturbed."},{"choiceId":"take_shrine_offerings","depletesOnResolve":true,"grantItemId":"jewelry","grantI…`
- Row 014 `micro_emergency_cache`: `{"baseWeight":0.2,"category":"Discovery","choices":[{"choiceId":"open_cache","depletesOnResolve":true,"grantItemId":"medical_kit","grantItemQuantity":1,"guiltDelta":0,"moraleDelta":2,"text":"Force the cracked seal and open the cache."},{"c…`
- Row 015 `micro_observation_post`: `{"baseWeight":0.3,"category":"Discovery","choices":[{"choiceId":"search_observation_post","depletesOnResolve":true,"grantItemId":"dosimeter","grantItemQuantity":1,"guiltDelta":0,"moraleDelta":0,"text":"Search the post for optics or intelli…`
- Row 016 `micro_abandoned_barricade`: `{"baseWeight":0.7,"category":"Discovery","choices":[{"choiceId":"search_barricade","depletesOnResolve":true,"grantItemId":"bandage","grantItemQuantity":1,"guiltDelta":1,"moraleDelta":0,"text":"Search the barricade for supplies left behind.…`
- Row 017 `micro_hunting_blind`: `{"baseWeight":0.5,"category":"Discovery","choices":[{"choiceId":"search_blind","depletesOnResolve":true,"grantItemId":"dried_rations","grantItemQuantity":2,"guiltDelta":0,"moraleDelta":0,"text":"Search the blind for hunting supplies."},{"c…`
- Row 018 `micro_radio_tower`: `{"baseWeight":0.3,"category":"Discovery","choices":[{"choiceId":"open_radio_cabinet","depletesOnResolve":true,"grantItemId":"antenna_coil","grantItemQuantity":1,"guiltDelta":0,"moraleDelta":0,"text":"Force the cabinet open and salvage the …`
- Row 019 `micro_destroyed_checkpoint`: `{"baseWeight":0.5,"category":"Discovery","choices":[{"choiceId":"search_checkpoint","depletesOnResolve":true,"grantItemId":"canned_food","grantItemQuantity":2,"guiltDelta":1,"moraleDelta":0,"text":"Search the checkpoint for confiscated goo…`
- Row 020 `micro_abandoned_tent`: `{"baseWeight":0.7,"category":"Social","choices":[{"choiceId":"search_tent","depletesOnResolve":true,"grantItemId":"cloth","grantItemQuantity":2,"guiltDelta":2,"moraleDelta":-1,"text":"Cut open the tent and search for supplies."},{"choiceId…`
- Row 021 `micro_makeshift_clinic`: `{"baseWeight":0.4,"category":"Discovery","choices":[{"choiceId":"search_clinic","depletesOnResolve":true,"grantItemId":"bandage","grantItemQuantity":3,"guiltDelta":0,"moraleDelta":1,"text":"Search the untouched shelves for medical supplies…`
- Row 022 `micro_crashed_drone`: `{"baseWeight":0.2,"category":"Discovery","choices":[{"choiceId":"open_drone_compartment","depletesOnResolve":true,"grantItemId":"electronic_scrap","grantItemQuantity":4,"guiltDelta":0,"moraleDelta":0,"text":"Force open the flight-control c…`
- Row 023 `micro_fuel_cache`: `{"baseWeight":0.2,"category":"Discovery","choices":[{"choiceId":"take_fuel_cache","depletesOnResolve":true,"grantItemId":"fuel","grantItemQuantity":4,"guiltDelta":0,"moraleDelta":2,"text":"Dig up the containers and take the fuel."},{"choic…`
- Row 024 `micro_water_source`: `{"baseWeight":0.5,"category":"Discovery","choices":[{"choiceId":"collect_water","depletesOnResolve":true,"grantItemId":"clean_water","grantItemQuantity":3,"guiltDelta":0,"moraleDelta":2,"text":"Pump and collect water from the source."},{"c…`
- Row 025 `micro_supply_drop`: `{"baseWeight":0.1,"category":"Discovery","choices":[{"choiceId":"open_supply_drop","depletesOnResolve":true,"grantItemId":"medical_kit","grantItemQuantity":2,"guiltDelta":0,"moraleDelta":3,"text":"Open the crate and take what is inside."},…`
- Row 026 `micro_hospital_chapel_ledger`: `{"baseWeight":0.9,"category":"Discovery","choices":[{"choiceId":"read_the_names","guiltDelta":0,"moraleDelta":1,"text":"Read the last page properly, then close the ledger."},{"choiceId":"take_matches","depletesOnResolve":true,"grantItemId"…`
- Row 027 `micro_depot_undertow_raft_line`: `{"baseWeight":0.7,"category":"Discovery","choices":[{"choiceId":"note_the_route","guiltDelta":0,"moraleDelta":1,"text":"Mark the pulley anchors on your map and leave the line alone."},{"choiceId":"cut_one_crate","depletesOnResolve":true,"g…`
- Row 028 `micro_gamma_levy_board`: `{"baseWeight":0.8,"category":"Discovery","choices":[{"choiceId":"memorize_the_board","guiltDelta":0,"moraleDelta":1,"text":"Memorize which farms have paid and which are marked owed."},{"choiceId":"take_the_chalk","depletesOnResolve":true,"…`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`

### `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` — complete current file

- Size: 587 lines / 26931 bytes.
- SHA-256: `994aeb65e087024e7b71bf03a48a8992b492e92966ea3cfa11f6cbf9aa825a83`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: using Ashfall.Core.IO;
00007: using Ashfall.Core.Content;
00008: namespace Ashfall.Core.Narrative
00009: {
00010:     /// <summary>
00011:     /// F1/F2/F3/F4 — structured consequence payload returned by a successful
00012:     /// encounter resolution. Narrative Core decides what the choice meant; the
00013:     /// Host applies each effect through the subsystem that owns it (expedition
00014:     /// loot / shelter inventory / journal / location authority). Deterministic:
00015:     /// derived entirely from the catalog, state, and input — no RNG.
00016:     /// </summary>
00017:     public sealed class NarrativeEncounterResolutionResult
00018:     {
00019:         public string EncounterId = string.Empty;
00020:         public string ChoiceId = string.Empty;
00021:         public string LocationId = string.Empty;
00022:         public int Day;
00023:
00024:         public int MoraleDelta;
00025:         public int GuiltDelta;
00026:
00027:         /// <summary>True when the resolved choice marks the encounter depleted
00028:         /// (one-time micro-location). The whole encounter is exhausted.</summary>
00029:         public bool DepletesEncounter;
00030:
00031:         /// <summary>Signed item delta: &gt;0 grant, &lt;0 removal (offerings), 0 none.
00032:         /// Empty item id with a zero quantity means "no item effect".</summary>
00033:         public string GrantItemId = string.Empty;
00034:         public int GrantItemQuantity;
00035:
00036:         /// <summary>Journal/codex knowledge key to unlock. Empty = none.</summary>
00037:         public string JournalUnlockId = string.Empty;
00038:
00039:         /// <summary>Location ID to discover. Empty = none.</summary>
00040:         public string DiscoverLocationId = string.Empty;
00041:
00042:         /// <summary>Campaign flag to set. Empty = none.</summary>
00043:         public string SetWorldFlagId = string.Empty;
00044:
00045:         /// <summary>Stable resolution identity for exactly-once Host application:
00046:         /// encounterId:choiceId:day:locationId. Not a hash — deterministic text.</summary>
00047:         public string ResolutionId =>
00048:             $"{EncounterId}:{ChoiceId}:{Day}:{LocationId}";
00049:     }
00050:
00051:     /// <summary>
00052:     /// Engine-agnostic port of the Unity narrative-encounter layer
00053:     /// (EncounterSO selection math + NarrativeEncounters factories, now
00054:     /// data-driven). Owns the catalog, weighted encounter selection per the
00055:     /// Unity formula (stance multipliers, danger/location filters), depletion
00056:     /// (F1), and a save/load-safe resolution history. All rolls through
00057:     /// ISeededRng. Cross-system consequences (items, journal, locations) are
00058:     /// returned to the Host as a structured payload — never mutated here.
00059:     /// </summary>
00060:     public class NarrativeEncounterSystem
00061:     {
00062:         public const string SystemId = "narrative_encounter_system";
00063:
00064:         private readonly NarrativeEncounterState _state;
00065:         private readonly List<EncounterDefinition> _catalog = new List<EncounterDefinition>();
00066:
00067:         /// <summary>F1 — depleted encounter IDs. O(1) lookup; bounded by the
00068:         /// authored depletable encounter set. Never exposed for mutation.</summary>
00069:         private readonly HashSet<string> _depletedEncounters = new HashSet<string>(StringComparer.Ordinal);
00070:
00071:         public event Action<EncounterDefinition> OnEncounterSelected;
00072:         public event Action<EncounterResolutionRecord> OnEncounterResolved;
00073:         public event Action<NarrativeEncounterState> OnStateChanged;
00074:
00075:         /// <summary>
00076:         /// Optional weather gate filter. When set, encounters whose IDs match
00077:         /// blocked weather gates are excluded from selection. The delegate
00078:         /// receives an encounter ID and returns true if the encounter is
00079:         /// blocked by current weather conditions. Plan 48 integration point.
00080:         /// </summary>
00081:         public Func<string, bool>? WeatherGateFilter { get; set; }
00082:
00083:         /// <summary>
00084:         /// Plan 52 — optional expansion-quest link. When set, a resolved
00085:         /// choice carrying <c>completesQuestId</c> records the decision as
00086:         /// expansion-quest progress (including the recorded choice id), which
00087:         /// is the persisted memory recurring-NPC arcs resolve from. Null
00088:         /// leaves encounter resolution exactly as before.
00089:         /// </summary>
00090:         public ExpansionQuestSystem? QuestLink { get; set; }
00091:
00092:         /// <summary>
00093:         /// Optional content-utilization instrumentation (Ticket #127). Null
00094:         /// during normal gameplay (side-effect free, zero overhead); set by
00095:         /// diagnostic/self-test harnesses that want SELECTED/EFFECT_PRODUCED
00096:         /// evidence sourced from this system's own real selection and
00097:         /// resolution logic rather than a hand-authored literal.
00098:         /// </summary>
00099:         public ContentUtilizationInstrumentation? Instrumentation { get; set; }
00100:
00101:         public NarrativeEncounterSystem(NarrativeEncounterState? state = null)
00102:         {
00103:             _state = state ?? new NarrativeEncounterState();
00104:             if (_state.history == null) _state.history = new List<EncounterResolutionRecord>();
00105:             if (_state.depletedEncounterIds != null)
00106:             {
00107:                 for (int i = 0; i < _state.depletedEncounterIds.Count; i++)
00108:                 {
00109:                     var id = _state.depletedEncounterIds[i];
00110:                     if (!string.IsNullOrEmpty(id)) _depletedEncounters.Add(id);
00111:                 }
00112:             }
00113:         }
00114:
00115:         public NarrativeEncounterState State => _state;
00116:         public IReadOnlyList<EncounterDefinition> Catalog => _catalog;
00117:         public int TotalResolved => _state.totalResolved;
00118:
00119:         // ── Catalog ────────────────────────────────────────────────────
00120:
00121:         public void RegisterEncounter(EncounterDefinition def)
00122:         {
00123:             if (def == null || string.IsNullOrEmpty(def.id)) return;
00124:             if (_catalog.Exists(e => e.id == def.id)) return;
00125:             _catalog.Add(def);
00126:         }
00127:
00128:         public void RegisterRange(IEnumerable<EncounterDefinition> defs)
00129:         {
00130:             if (defs == null) return;
00131:             foreach (var def in defs) RegisterEncounter(def);
00132:         }
00133:
00134:         public EncounterDefinition? Find(string encounterId)
00135:         {
00136:             for (int i = 0; i < _catalog.Count; i++)
00137:                 if (_catalog[i].id == encounterId) return _catalog[i];
00138:             return null;
00139:         }
00140:
00141:         // ── Weighted selection (Unity formula) ─────────────────────────
00142:
00143:         /// <summary>F1 — true when the encounter's depleting choice has
00144:         /// already been resolved: the micro-location is exhausted and the
00145:         /// encounter must not reappear.</summary>
00146:         public bool IsDepleted(string encounterId)
00147:         {
00148:             if (string.IsNullOrEmpty(encounterId)) return false;
00149:             return _depletedEncounters.Contains(encounterId);
00150:         }
00151:
00152:         /// <summary>Number of depleted encounters (diagnostics; bounded by the
00153:         /// authored depletable set).</summary>
00154:         public int DepletedCount => _depletedEncounters.Count;
00155:
00156:         /// <summary>
00157:         /// Returns all eligible narrative encounter candidates and their effective weights
00158:         /// for the given stance, danger level, and location without consuming any RNG.
00159:         /// </summary>
00160:         public List<(EncounterDefinition def, double weight)> GetEligibleCandidates(
00161:             string stance, float dangerLevel, string locationId)
00162:         {
00163:             var candidates = new List<(EncounterDefinition, double)>();
00164:             for (int i = 0; i < _catalog.Count; i++)
00165:             {
00166:                 var def = _catalog[i];
00167:                 if (IsDepleted(def.id)) continue;
00168:                 double w = def.GetEffectiveWeight(stance, dangerLevel, locationId);
00169:                 if (w <= 0d) continue;
00170:                 if (WeatherGateFilter != null && WeatherGateFilter(def.id)) continue;
00171:                 candidates.Add((def, w));
00172:             }
00173:             return candidates;
00174:         }
00175:
00176:         public void RecordEncounterSelected(EncounterDefinition def)
00177:         {
00178:             if (def == null) return;
00179:             OnEncounterSelected?.Invoke(def);
00180:             Instrumentation?.RecordDefinitionSelected(
00181:                 NarrativeEncounterCatalogLoader.FileName, def.id, nameof(NarrativeEncounterSystem));
00182:         }
00183:
00184:         /// <summary>Pick an eligible encounter by weight, or null when none
00185:         /// qualify for this stance/danger/location. F1: depleted encounters are
00186:         /// excluded before weighting, so they neither distort the weight sum
00187:         /// nor consume deterministic RNG rolls as zero-weight candidates.</summary>
00188:         public EncounterDefinition? SelectEncounter(
00189:             string stance, float dangerLevel, string locationId, ISeededRng rng)
00190:         {
00191:             if (rng == null) return null;
00192:
00193:             var candidates = GetEligibleCandidates(stance, dangerLevel, locationId);
00194:             if (candidates.Count == 0) return null;
00195:
00196:             double total = 0d;
00197:             for (int i = 0; i < candidates.Count; i++) total += candidates[i].weight;
00198:             if (total <= 0d) return null;
00199:
00200:             double roll = rng.NextDouble() * total;
00201:             double acc = 0d;
00202:             for (int i = 0; i < candidates.Count; i++)
00203:             {
00204:                 acc += candidates[i].weight;
00205:                 if (roll < acc)
00206:                 {
00207:                     RecordEncounterSelected(candidates[i].def);
00208:                     return candidates[i].def;
00209:                 }
00210:             }
00211:             return null;
00212:         }
00213:
00214:         // ── Resolution ─────────────────────────────────────────────────
00215:
00216:         /// <summary>Morale/guilt-only resolution retained for existing callers.
00217:         /// Returns true when the resolution committed. New Host flows should
00218:         /// prefer <see cref="TryResolve"/> for the full consequence payload.</summary>
00219:         public bool Resolve(string encounterId, string choiceId, string locationId, int day)
00220:             => TryResolve(encounterId, choiceId, locationId, day) != null;
00221:
00222:         /// <summary>
00223:         /// F1–F4 — validate the encounter and choice, append exactly one
00224:         /// resolution record, apply depletion when the choice depletes, and
00225:         /// return the full consequence payload. Validation precedes mutation:
00226:         /// an unknown encounter or choice never touches state. The Host applies
00227:         /// the returned item/journal/location effects through their owning
00228:         /// subsystems; this method never does.
00229:         /// </summary>
00230:         public NarrativeEncounterResolutionResult? TryResolve(
00231:             string encounterId, string choiceId, string locationId, int day)
00232:         {
00233:             var def = Find(encounterId);
00234:             if (def == null) return null;
00235:             var choice = FindChoice(def, choiceId);
00236:             if (choice == null) return null;
00237:
00238:             bool depletes = choice.depletesOnResolve;
00239:
00240:             // F1: a depleting choice marks the whole encounter exhausted —
00241:             // even if loot capacity later rejects the grant (the site was
00242:             // searched; that it happened is not undone by a full pack).
00243:             if (depletes) _depletedEncounters.Add(encounterId);
00244:
00245:             var record = new EncounterResolutionRecord
00246:             {
00247:                 encounterId = encounterId,
00248:                 choiceId = choiceId,
00249:                 locationId = locationId ?? string.Empty,
00250:                 day = day,
00251:                 moraleDelta = choice.moraleDelta,
00252:                 guiltDelta = choice.guiltDelta
00253:             };
00254:             _state.history.Add(record);
00255:             _state.totalResolved++;
00256:             _state.cumulativeMorale += choice.moraleDelta;
00257:             _state.cumulativeGuilt += choice.guiltDelta;
00258:             ApplyQuestLink(choice, day);
00259:
00260:             var result = new NarrativeEncounterResolutionResult
00261:             {
00262:                 EncounterId = encounterId,
00263:                 ChoiceId = choiceId,
00264:                 LocationId = locationId ?? string.Empty,
00265:                 Day = day,
00266:                 MoraleDelta = choice.moraleDelta,
00267:                 GuiltDelta = choice.guiltDelta,
00268:                 DepletesEncounter = depletes,
00269:                 GrantItemId = choice.grantItemId ?? string.Empty,
00270:                 GrantItemQuantity = choice.grantItemQuantity,
00271:                 JournalUnlockId = choice.journalUnlockId ?? string.Empty,
00272:                 DiscoverLocationId = choice.discoverLocationId ?? string.Empty,
00273:                 SetWorldFlagId = choice.setWorldFlag ?? string.Empty
00274:             };
00275:
00276:             OnEncounterResolved?.Invoke(record);
00277:             Instrumentation?.RecordDefinitionConsumed(
00278:                 NarrativeEncounterCatalogLoader.FileName, encounterId, nameof(NarrativeEncounterSystem),
00279:                 $"morale{(choice.moraleDelta >= 0 ? "+" : "")}{choice.moraleDelta:0.##} guilt{(choice.guiltDelta >= 0 ? "+" : "")}{choice.guiltDelta:0.##}",
00280:                 day);
00281:             RaiseChanged();
00282:             return result;
00283:         }
00284:
00285:         /// <summary>
00286:         /// Plan 52 — land an arc decision into the expansion-quest ledger.
00287:         /// Idempotent and order-safe: starts the quest if its day window has
00288:         /// not auto-started it yet, records the authored choice, then completes
00289:         /// it unless a choice effect already did. Quest progress is the
00290:         /// persisted arc-memory authority — this bridge writes nothing else.
00291:         /// </summary>
00292:         private void ApplyQuestLink(EncounterChoiceDefinition choice, int day)
00293:         {
00294:             if (QuestLink == null || string.IsNullOrEmpty(choice.completesQuestId)) return;
00295:
00296:             string questId = choice.completesQuestId;
00297:             if (!QuestLink.IsStarted(questId))
00298:                 QuestLink.StartQuest(questId, day);
00299:
00300:             if (!string.IsNullOrEmpty(choice.completesQuestChoiceId))
00301:                 QuestLink.MakeChoice(questId, choice.completesQuestChoiceId, day);
00302:
00303:             if (!QuestLink.IsCompleted(questId))
00304:                 QuestLink.CompleteQuest(questId, day);
00305:         }
00306:
00307:         // ── Pending surfaced queue ─────────────────────────────────────
00308:         // The host enqueues on surface and clears after the player has
00309:         // acknowledged a choice. Resolve() deliberately does NOT auto-clear:
00310:         // the pending list must mirror what the player actually acknowledged,
00311:         // not what Core happened to record.
00312:
00313:         /// <summary>Append a surfaced-but-unresolved encounter to the pending queue.</summary>
00314:         public void EnqueuePending(string encounterId, string locationId, int legIndex, int day)
00315:         {
00316:             if (string.IsNullOrEmpty(encounterId)) return;
00317:             if (_state.pending == null) _state.pending = new List<PendingSurfacedEncounter>();
00318:             _state.pending.Add(new PendingSurfacedEncounter
00319:             {
00320:                 encounterId = encounterId,
00321:                 locationId = locationId ?? string.Empty,
00322:                 legIndex = legIndex,
00323:                 day = day
00324:             });
00325:             RaiseChanged();
00326:         }
00327:
00328:         /// <summary>Remove every pending entry for this encounter id. No-op when absent.</summary>
00329:         public void ClearPending(string encounterId)
00330:         {
00331:             if (_state.pending == null || string.IsNullOrEmpty(encounterId)) return;
00332:             bool removed = false;
00333:             for (int i = _state.pending.Count - 1; i >= 0; i--)
00334:             {
00335:                 if (_state.pending[i] != null && _state.pending[i].encounterId == encounterId)
00336:                 {
00337:                     _state.pending.RemoveAt(i);
00338:                     removed = true;
00339:                 }
00340:             }
00341:             if (removed) RaiseChanged();
00342:         }
00343:
00344:         /// <summary>Drop the whole pending queue without resolving anything.</summary>
00345:         public void ClearAllPending()
00346:         {
00347:             if (_state.pending == null || _state.pending.Count == 0) return;
00348:             _state.pending.Clear();
00349:             RaiseChanged();
00350:         }
00351:
00352:         private static EncounterChoiceDefinition? FindChoice(EncounterDefinition def, string choiceId)
00353:         {
00354:             for (int i = 0; i < def.choices.Count; i++)
00355:                 if (def.choices[i].choiceId == choiceId) return def.choices[i];
00356:             return null;
00357:         }
00358:
00359:         // ── Save / Load ────────────────────────────────────────────────
00360:
00361:         public NarrativeEncounterState CaptureState()
00362:         {
00363:             var copy = new NarrativeEncounterState
00364:             {
00365:                 systemId = _state.systemId,
00366:                 totalResolved = _state.totalResolved,
00367:                 cumulativeMorale = _state.cumulativeMorale,
00368:                 cumulativeGuilt = _state.cumulativeGuilt,
00369:                 pending = new List<PendingSurfacedEncounter>(),
00370:                 depletedEncounterIds = CaptureDepletedIds()
00371:             };
00372:             var ordered = new List<EncounterResolutionRecord>(_state.history);
00373:             ordered.Sort((a, b) =>
00374:             {
00375:                 int byDay = a.day.CompareTo(b.day);
00376:                 if (byDay != 0) return byDay;
00377:                 int byEnc = string.CompareOrdinal(a.encounterId, b.encounterId);
00378:                 return byEnc != 0 ? byEnc : string.CompareOrdinal(a.choiceId, b.choiceId);
00379:             });
00380:             for (int i = 0; i < ordered.Count; i++)
00381:             {
00382:                 var r = ordered[i];
00383:                 copy.history.Add(new EncounterResolutionRecord
00384:                 {
00385:                     encounterId = r.encounterId,
00386:                     choiceId = r.choiceId,
00387:                     locationId = r.locationId,
00388:                     day = r.day,
00389:                     moraleDelta = r.moraleDelta,
00390:                     guiltDelta = r.guiltDelta
00391:                 });
00392:             }
00393:             if (_state.pending != null)
00394:             {
00395:                 for (int i = 0; i < _state.pending.Count; i++)
00396:                     copy.pending.Add(_state.pending[i]);
00397:             }
00398:             return copy;
00399:         }
00400:
00401:         /// <summary>F1 — ordinal-sorted depletion snapshot. HashSet iteration
00402:         /// order is not a cross-host guarantee and the checksum walks the array.</summary>
00403:         private List<string> CaptureDepletedIds()
00404:         {
00405:             var ids = new List<string>(_depletedEncounters.Count);
00406:             ids.AddRange(_depletedEncounters);
00407:             ids.Sort(string.CompareOrdinal);
00408:             return ids;
00409:         }
00410:
00411:         public void RestoreState(NarrativeEncounterState saved)
00412:         {
00413:             if (saved == null) return;
00414:             _state.systemId = SystemId;
00415:             _state.totalResolved = saved.totalResolved;
00416:             _state.cumulativeMorale = saved.cumulativeMorale;
00417:             _state.cumulativeGuilt = saved.cumulativeGuilt;
00418:             _state.history.Clear();
00419:             if (saved.history != null)
00420:             {
00421:                 for (int i = 0; i < saved.history.Count; i++)
00422:                 {
00423:                     var r = saved.history[i];
00424:                     if (r == null || string.IsNullOrEmpty(r.encounterId)) continue;
00425:                     _state.history.Add(new EncounterResolutionRecord
00426:                     {
00427:                         encounterId = r.encounterId,
00428:                         choiceId = r.choiceId,
00429:                         locationId = r.locationId,
00430:                         day = r.day,
00431:                         moraleDelta = r.moraleDelta,
00432:                         guiltDelta = r.guiltDelta
00433:                     });
00434:                 }
00435:             }
00436:             _state.pending = saved.pending != null
00437:                 ? new List<PendingSurfacedEncounter>(saved.pending)
00438:                 : new List<PendingSurfacedEncounter>();
00439:
00440:             // F1 restore: clear, then rebuild deterministically. A present list
00441:             // (even empty) is authoritative — a campaign that resolved nothing
00442:             // depleting must not drift as the catalog evolves. A null list
00443:             // marks a legacy save that predates depletion: reconstruct the set
00444:             // from history so pre-feature depleting choices stay exhausted.
00445:             _depletedEncounters.Clear();
00446:             if (saved.depletedEncounterIds != null)
00447:             {
00448:                 var ids = new List<string>(saved.depletedEncounterIds);
00449:                 ids.Sort(string.CompareOrdinal);
00450:                 for (int i = 0; i < ids.Count; i++)
00451:                 {
00452:                     if (!string.IsNullOrEmpty(ids[i])) _depletedEncounters.Add(ids[i]);
00453:                 }
00454:             }
00455:             else
00456:             {
00457:                 ReconstructDepletionFromHistory();
00458:             }
00459:
00460:             _state.depletedEncounterIds = CaptureDepletedIds();
00461:             RaiseChanged();
00462:         }
00463:
00464:         /// <summary>
00465:         /// F1 legacy migration (§48 of the integration plan): walk the saved
00466:         /// resolution history, resolve each recorded choice against the current
00467:         /// catalog, and re-mark every encounter whose recorded choice depletes.
00468:         /// Deterministic. Unknown historical encounters/choices are skipped —
00469:         /// never guessed. Runs only when a legacy save carries no depletion
00470:         /// list; never on ordinary restore.
00471:         /// </summary>
00472:         private void ReconstructDepletionFromHistory()
00473:         {
00474:             if (_state.history == null) return;
00475:             for (int i = 0; i < _state.history.Count; i++)
00476:             {
00477:                 var r = _state.history[i];
00478:                 if (r == null || string.IsNullOrEmpty(r.encounterId)) continue;
00479:                 var def = Find(r.encounterId);
00480:                 if (def == null) continue; // unknown historical encounter — skip, do not guess
00481:                 var choice = FindChoice(def, r.choiceId);
00482:                 if (choice == null) continue; // unknown historical choice — skip, do not guess
00483:                 if (choice.depletesOnResolve) _depletedEncounters.Add(r.encounterId);
00484:             }
00485:         }
00486:
00487:         private void RaiseChanged() => OnStateChanged?.Invoke(_state);
00488:     }
00489:
00490:     /// <summary>Engine-agnostic loader for narrative_encounters.json.</summary>
00491:     public static class NarrativeEncounterCatalogLoader
00492:     {
00493:         public const string FileName = "narrative_encounters.json";
00494:
00495:         /// <summary>Plan 52 — recurring-NPC arc encounters load after the
00496:         /// base catalog (duplicate ids are dropped by RegisterEncounter).</summary>
00497:         public const string ArcFileName = "narrative_encounters_npc_arcs.json";
00498:
00499:         /// <summary>Authored expansion pass — generic wasteland encounters that
00500:         /// load under the same schema; duplicate ids are dropped by
00501:         /// RegisterEncounter (the primary catalog wins).</summary>
00502:         public const string ExpansionFileName = "narrative_encounters_expansion.json";
00503:
00504:         /// <summary>GAP-49B — destination-bound approach micro-locations
00505:         /// (Plan 76 §35) load last through the same schema; their
00506:         /// `requiredLocationId` makes them weight-zero off-destination.</summary>
00507:         public const string MicroLocationsFileName = "micro_locations.json";
00508:
00509:         public static List<EncounterDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
00510:         {
00511:             var result = new List<EncounterDefinition>();
00512:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00513:                 return result;
00514:
00515:             result.AddRange(LoadFile(dataDir, FileName, fileIO, json));
00516:             result.AddRange(LoadFile(dataDir, ArcFileName, fileIO, json));
00517:             result.AddRange(LoadFile(dataDir, ExpansionFileName, fileIO, json));
00518:             result.AddRange(LoadFile(dataDir, MicroLocationsFileName, fileIO, json));
00519:             return DeduplicateById(result);
00520:         }
00521:
00522:         /// <summary>Primary-wins dedupe: the first occurrence of an id is kept
00523:         /// (base before arc before expansion before micro) so every consumer sees
00524:         /// a unique catalog. RegisterEncounter applies the same rule defensively.</summary>
00525:         private static List<EncounterDefinition> DeduplicateById(List<EncounterDefinition> defs)
00526:         {
00527:             var seen = new HashSet<string>(StringComparer.Ordinal);
00528:             var unique = new List<EncounterDefinition>(defs.Count);
00529:             for (int i = 0; i < defs.Count; i++)
00530:             {
00531:                 var def = defs[i];
00532:                 if (def == null) continue;
00533:                 if (!string.IsNullOrEmpty(def.id) && !seen.Add(def.id)) continue;
00534:                 unique.Add(def);
00535:             }
00536:             return unique;
00537:         }
00538:
00539:         private static List<EncounterDefinition> LoadFile(
00540:             string dataDir, string fileName, IFileIO fileIO, IJsonSerializer json)
00541:         {
00542:             var result = new List<EncounterDefinition>();
00543:
00544:             string path = fileIO.Combine(dataDir, fileName);
00545:             if (!fileIO.FileExists(path))
00546:                 return result;
00547:
00548:             string raw = fileIO.ReadAllText(path);
00549:             if (string.IsNullOrWhiteSpace(raw))
00550:                 return result;
00551:
00552:             try
00553:             {
00554:                 var parsed = CatalogLocator.LoadWrappedList<EncounterDefinition>(raw, SystemTextJsonSerializer.Options).ToArray();
00555:                 if (parsed == null) return result;
00556:                 for (int i = 0; i < parsed.Length; i++)
00557:                 {
00558:                     if (parsed[i] == null || string.IsNullOrEmpty(parsed[i].id)) continue;
00559:                     if (parsed[i].choices == null) parsed[i].choices = new List<EncounterChoiceDefinition>();
00560:
00561:                     // F6 §6.3 seal — the dedicated MicroLocationEncounterLoader stamps
00562:                     // marker + source file, but production loads through this shared
00563:                     // LoadFile. Apply the same stamp here so every definition from
00564:                     // micro_locations.json carries the metadata its consumers expect;
00565:                     // other catalog files keep their defaults.
00566:                     if (fileName == MicroLocationsFileName)
00567:                     {
00568:                         parsed[i].isMicroLocation = true;
00569:                         parsed[i].sourceFile = MicroLocationsFileName;
00570:                     }
00571:                     else if (fileName == ExpansionFileName)
00572:                     {
00573:                         // Diagnostic attribution only — selection never reads it.
00574:                         parsed[i].sourceFile = ExpansionFileName;
00575:                     }
00576:                     result.Add(parsed[i]);
00577:                 }
00578:             }
00579:             catch (Exception ex_CATDIAG)
00580:             {
00581:                 CatalogDiagnostics.Warn(path, "EncounterDefinition list", ex_CATDIAG);
00582:                 return result;
00583:             }
00584:             return result;
00585:         }
00586:     }
00587: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Narrative/NarrativeHeadlessDemo.cs`

### `Assets/Ashfall.Core/Narrative/NarrativeHeadlessDemo.cs` — complete current file

- Size: 92 lines / 4086 bytes.
- SHA-256: `d3b3686e30a367a521e7fcf8fe60bfb4472d05a388f69ae54c314bccaac07836`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003:
00004: namespace Ashfall.Core.Narrative
00005: {
00006:     /// <summary>
00007:     /// Headless verification of the narrative encounter core port.
00008:     /// Invoked by `dotnet test` and by Godot `-- --narrative-selftest`.
00009:     /// </summary>
00010:     public static class NarrativeHeadlessDemo
00011:     {
00012:         public static HeadlessReport Run(ILog? log = null)
00013:         {
00014:             CatalogLocator.UseInvariantCulture();
00015:             log = log ?? NullLog.Instance;
00016:             var report = new HeadlessReport();
00017:
00018:             void Check(bool condition, string name)
00019:             {
00020:                 report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
00021:                 if (condition)
00022:                 {
00023:                     report.PassedCount++;
00024:                     log.Info("[PASS] " + name);
00025:                 }
00026:                 else
00027:                 {
00028:                     report.FailedCount++;
00029:                     log.Error("[FAIL] " + name);
00030:                 }
00031:             }
00032:
00033:             log.Info("[NarrativeHeadlessDemo] begin");
00034:
00035:             var sys = new NarrativeEncounterSystem();
00036:             sys.RegisterEncounter(new EncounterDefinition
00037:             {
00038:                 id = "enc_dead_letter_office", title = "Dead Letter Office", category = "Discovery",
00039:                 baseWeight = 2f, minDangerLevel = 0f,
00040:                 choices = new List<EncounterChoiceDefinition>
00041:                 {
00042:                     new EncounterChoiceDefinition { choiceId = "read_letters", text = "Read", moraleDelta = 3, guiltDelta = 0 },
00043:                     new EncounterChoiceDefinition { choiceId = "burn_van", text = "Burn", moraleDelta = 0, guiltDelta = 4 }
00044:                 }
00045:             });
00046:             sys.RegisterEncounter(new EncounterDefinition
00047:             {
00048:                 id = "enc_pianist", title = "Pianist", category = "Social",
00049:                 baseWeight = 1.5f, minDangerLevel = 3f,
00050:                 choices = new List<EncounterChoiceDefinition>
00051:                 {
00052:                     new EncounterChoiceDefinition { choiceId = "listen", text = "Listen", moraleDelta = 4, guiltDelta = 0 }
00053:                 }
00054:             });
00055:
00056:             Check(sys.Catalog.Count == 2, "catalog registers two encounters");
00057:             Check(sys.Find("enc_missing") == null, "unknown id not found");
00058:
00059:             var picked = sys.SelectEncounter("Stealth", 1f, null!, new SeededRng(5));
00060:             Check(picked != null && picked.id == "enc_dead_letter_office",
00061:                 "danger 1 excludes the pianist (min 3); dead letter offered");
00062:             if (picked == null) return report;
00063:             Check(sys.SelectEncounter("Stealth", 1f, null!, new SeededRng(5))!.id == picked.id,
00064:                 "same seed picks the same encounter (determinism)");
00065:
00066:             Check(sys.Resolve("enc_dead_letter_office", "burn_van", "loc_ring_road", 40),
00067:                 "resolve burn_van");
00068:             Check(sys.State.cumulativeMorale == 0 && sys.State.cumulativeGuilt == 4,
00069:                 "choice magnitudes recorded");
00070:             Check(!sys.Resolve("enc_dead_letter_office", "missing_choice", null!, 40),
00071:                 "unknown choice refused");
00072:             Check(sys.TotalResolved == 1, "resolution history counts one");
00073:
00074:             string before = SaveChecksum.Compute(sys.CaptureState());
00075:             var restored = new NarrativeEncounterSystem();
00076:             restored.RestoreState(sys.CaptureState());
00077:             string after = SaveChecksum.Compute(restored.CaptureState());
00078:             Check(before == after, "save/load checksum stable");
00079:
00080:             var snapshot = sys.CaptureState();
00081:             snapshot.history[0].guiltDelta = 99;
00082:             Check(sys.State.cumulativeGuilt == 4, "capture returns snapshot, not live state");
00083:
00084:             report.Passed = report.FailedCount == 0;
00085:             report.Summary =
00086:                 $"[NarrativeHeadlessDemo] {(report.Passed ? "PASS" : "FAIL")} " +
00087:                 $"{report.PassedCount}/{report.PassedCount + report.FailedCount}";
00088:             log.Info(report.Summary);
00089:             return report;
00090:         }
00091:     }
00092: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs`

### `Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs` — complete current file

- Size: 354 lines / 16880 bytes.
- SHA-256: `a9653182ddac1d1221b396712dbedb33cb2629671656027bb3421b1a437ad88e`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005: #pragma warning disable CS8618
00006: using Ashfall.Core.Factions;
00007: using Ashfall.Core.Narrative;
00008:
00009: namespace Ashfall.Core.Expeditions
00010: {
00011:     /// <summary>
00012:     /// Engine-agnostic bridge from ExpeditionSystem.OnEncounterTriggered
00013:     /// into NarrativeEncounterSystem. Resolves a selectable encounter by
00014:     /// weight using the expedition's stance / dangerLevel / locationId,
00015:     /// deterministically, and exposes the result as an EncounterSurfaced DTO.
00016:     /// Does NOT auto-resolve: choices require an explicit player call back
00017:     /// into NarrativeEncounterSystem.Resolve(...). When the underlying
00018:     /// catalog has no eligible entry, emits an honest null-encounter DTO
00019:     /// describing only what the state can prove.
00020:     ///
00021:     /// Ordering decision: the host passes the same ISeededRng instance it used
00022:     /// for ExpeditionSystem.TickHours. TickHours consumes RNG for stamina and
00023:     /// loot rolls before RollEncounter fires OnEncounterTriggered; the bridge
00024:     /// then continues consuming from the same stream for selection. This keeps
00025:     /// the full encounter-roll → selection sequence on one deterministic stream
00026:     /// so the same seed produces identical surfaced sequences across hosts.
00027:     /// </summary>
00028:     public sealed class ExpeditionEncounterBridge
00029:     {
00030:         public sealed class EncounterSurfaced
00031:         {
00032:             /// <summary>Selected encounter id, or null when no eligible encounter exists.</summary>
00033:             public string encounter_id;
00034:
00035:             /// <summary>Verbatim title from the catalog, or "Encounter" for bare notices.</summary>
00036:             public string title;
00037:
00038:             /// <summary>Verbatim description from the catalog, or honest-bare text.</summary>
00039:             public string description;
00040:
00041:             /// <summary>Catalog category (Discovery / Hazard / Social / Trade), empty for bare notices.</summary>
00042:             public string category;
00043:
00044:             /// <summary>Ordered choices from the catalog, empty for bare notices.</summary>
00045:             public List<EncounterChoiceDefinition> choices;
00046:
00047:             /// <summary>True when this surface came from TravelEncounterSystem.</summary>
00048:             public bool is_patrol;
00049:
00050:             /// <summary>True when this surface came from micro_locations.json.</summary>
00051:             public bool is_micro_location;
00052:
00053:             /// <summary>Authored faction identity for patrol name and emblem presentation.
00054:             /// Core standing mutations retain their separate canonical systems ID.</summary>
00055:             public string faction_id;
00056:
00057:             /// <summary>Authored patrol context, kept separate from prose.</summary>
00058:             public string territory_state;
00059:             public string patrol_archetype;
00060:             public string recognition_label;
00061:             public int patrol_chain_stage;
00062:
00063:             /// <summary>The expedition state that triggered this surfacing.</summary>
00064:             public ExpeditionState trigger;
00065:
00066:             /// <summary>Null from the bridge; reserved for host-filled lead-resolution flag.</summary>
00067:             public bool? resolved_at_lead;
00068:
00069:             /// <summary>Null from the bridge; populated after player choice via Resolve.</summary>
00070:             public string encounter_record_resolution_id;
00071:         }
00072:
00073:         public event Action<EncounterSurfaced> OnSurfaced;
00074:
00075:         private readonly NarrativeEncounterSystem _narrative;
00076:         private ISeededRng _rng;
00077:         private EncounterSurfaced _lastSurfaced;
00078:
00079:         public TravelEncounterSystem? TravelEngine { get; set; }
00080:         public int CurrentDay { get; set; } = 1;
00081:         public string CurrentSeason { get; set; } = "all";
00082:         public Func<string, string>? RegionResolver { get; set; }
00083:         public EncounterSurfaced? LastSurfaced => _lastSurfaced;
00084:
00085:         /// <summary>F2/F3/F4 — the consequence payload of the most recent
00086:         /// successful <see cref="ResolveChoice"/>, or null. The Host reads it
00087:         /// immediately after a successful resolve to apply item/journal/
00088:         /// location effects through their owning subsystems. Core never
00089:         /// mutates those systems itself.</summary>
00090:         public NarrativeEncounterResolutionResult? LastResolution { get; private set; }
00091:
00092:         /// <summary>
00093:         /// Construct with the host's NarrativeEncounterSystem and the shared
00094:         /// ISeededRng stream (same instance used for ExpeditionSystem.TickHours).
00095:         /// </summary>
00096:         public ExpeditionEncounterBridge(NarrativeEncounterSystem narrative, ISeededRng rng)
00097:             : this(narrative, rng, null)
00098:         {
00099:         }
00100:
00101:         public ExpeditionEncounterBridge(NarrativeEncounterSystem narrative, ISeededRng rng, TravelEncounterSystem? travel)
00102:         {
00103:             _narrative = narrative ?? throw new ArgumentNullException(nameof(narrative));
00104:             _rng = rng ?? throw new ArgumentNullException(nameof(rng));
00105:             TravelEngine = travel;
00106:         }
00107:
00108:         /// <summary>Rebind the draw stream (campaign day fork). Same instance as TickHours.</summary>
00109:         public void SetRng(ISeededRng rng)
00110:         {
00111:             _rng = rng ?? throw new ArgumentNullException(nameof(rng));
00112:         }
00113:
00114:         private string ResolveRegion(string? locationId)
00115:         {
00116:             if (string.IsNullOrWhiteSpace(locationId)) return string.Empty;
00117:             if (RegionResolver != null)
00118:             {
00119:                 string resolved = RegionResolver(locationId);
00120:                 if (!string.IsNullOrEmpty(resolved)) return resolved;
00121:             }
00122:
00123:             string lower = locationId.ToLowerInvariant();
00124:             if (lower.Contains("high_scarp")) return "high_scarp";
00125:             if (lower.Contains("the_toll") || lower.Contains("toll")) return "the_toll";
00126:             if (lower.Contains("industrial_belt") || lower.Contains("foundry") || lower.Contains("industrial")) return "industrial_belt";
00127:             if (lower.Contains("dead_suburbs") || lower.Contains("suburbs")) return "dead_suburbs";
00128:
00129:             return string.Empty;
00130:         }
00131:
00132:         /// <summary>
00133:         /// Surface an encounter for the given expedition state. Consumes RNG
00134:         /// from the shared stream. Raises OnSurfaced exactly once per call with
00135:         /// either a resolved DTO or an honest-bare-notice DTO when nothing
00136:         /// qualifies. Merges narrative and travel encounter candidates into
00137:         /// a single weighted list rolled with exactly one RNG draw.
00138:         /// </summary>
00139:         public void Surface(ExpeditionState state)
00140:         {
00141:             if (state == null) return;
00142:
00143:             var dto = new EncounterSurfaced
00144:             {
00145:                 trigger = state,
00146:                 resolved_at_lead = null,
00147:                 encounter_record_resolution_id = null!
00148:             };
00149:
00150:             // Enumerate narrative candidates (0 RNG)
00151:             var narrativeCandidates = _narrative.GetEligibleCandidates(
00152:                 state.stance ?? string.Empty,
00153:                 state.dangerLevel,
00154:                 state.locationId ?? string.Empty);
00155:
00156:             // Enumerate patrol candidates (0 RNG)
00157:             List<(TravelEncounterDefinition encounter, float weight)>? patrolCandidates = null;
00158:             if (TravelEngine != null)
00159:             {
00160:                 string region = ResolveRegion(state.locationId);
00161:                 int day = state.startedDay > 0 ? state.startedDay : CurrentDay;
00162:                 patrolCandidates = TravelEngine.GetEligiblePatrolCandidates(
00163:                     region,
00164:                     state.dangerLevel,
00165:                     state.stance ?? string.Empty,
00166:                     CurrentSeason,
00167:                     day);
00168:             }
00169:
00170:             double totalWeight = 0d;
00171:             for (int i = 0; i < narrativeCandidates.Count; i++)
00172:             {
00173:                 totalWeight += narrativeCandidates[i].weight;
00174:             }
00175:             if (patrolCandidates != null)
00176:             {
00177:                 for (int i = 0; i < patrolCandidates.Count; i++)
00178:                 {
00179:                     totalWeight += patrolCandidates[i].weight;
00180:                 }
00181:             }
00182:
00183:             if (totalWeight <= 0d)
00184:             {
00185:                 dto.encounter_id = null!;
00186:                 dto.title = "Encounter";
00187:                 dto.description = "Something is happening on this leg. No record of it survives.";
00188:                 dto.category = string.Empty;
00189:                 dto.choices = new List<EncounterChoiceDefinition>();
00190:                 dto.resolved_at_lead = false;
00191:                 dto.is_patrol = false;
00192:             }
00193:             else
00194:             {
00195:                 // Single deterministic draw
00196:                 double roll = _rng.NextDouble() * totalWeight;
00197:                 double acc = 0d;
00198:                 bool picked = false;
00199:
00200:                 for (int i = 0; i < narrativeCandidates.Count; i++)
00201:                 {
00202:                     acc += narrativeCandidates[i].weight;
00203:                     if (roll < acc)
00204:                     {
00205:                         var def = narrativeCandidates[i].def;
00206:                         dto.encounter_id = def.id;
00207:                         dto.title = def.title;
00208:                         dto.description = def.description;
00209:                         dto.category = def.category;
00210:                         dto.is_micro_location = def.isMicroLocation;
00211:                         dto.choices = def.choices ?? new List<EncounterChoiceDefinition>();
00212:                         _narrative.RecordEncounterSelected(def);
00213:                         picked = true;
00214:                         break;
00215:                     }
00216:                 }
00217:
00218:                 if (!picked && patrolCandidates != null)
00219:                 {
00220:                     for (int i = 0; i < patrolCandidates.Count; i++)
00221:                     {
00222:                         acc += patrolCandidates[i].weight;
00223:                         if (roll < acc || i == patrolCandidates.Count - 1)
00224:                         {
00225:                             var pDef = patrolCandidates[i].encounter;
00226:                             dto.encounter_id = pDef.Id;
00227:                             dto.title = pDef.Title;
00228:                             dto.description = pDef.Description;
00229:                             dto.category = pDef.Category;
00230:                             dto.is_patrol = true;
00231:                             dto.faction_id = pDef.FactionId;
00232:                             dto.territory_state = pDef.TerritoryState ?? string.Empty;
00233:                             dto.patrol_archetype = pDef.PatrolArchetype ?? string.Empty;
00234:                             dto.choices = new List<EncounterChoiceDefinition>();
00235:                             var patrolPresentation = TravelEngine!.BuildPatrolPresentation(pDef.Id);
00236:                             if (patrolPresentation != null)
00237:                             {
00238:                                 dto.faction_id = patrolPresentation.DisplayFactionId;
00239:                                 dto.recognition_label = patrolPresentation.RecognitionLabel;
00240:                                 dto.patrol_chain_stage = patrolPresentation.CurrentChainStage;
00241:                             }
00242:                             if (pDef.Choices != null)
00243:                             {
00244:                                 foreach (var c in pDef.Choices)
00245:                                 {
00246:                                     var projected = patrolPresentation?.Choices
00247:                                         .FirstOrDefault(x => string.Equals(x.ChoiceId, c.ChoiceId, StringComparison.OrdinalIgnoreCase));
00248:                                     dto.choices.Add(new EncounterChoiceDefinition
00249:                                     {
00250:                                         choiceId = c.ChoiceId,
00251:                                         text = c.Text,
00252:                                         moraleDelta = c.MoraleDelta,
00253:                                         guiltDelta = c.GuiltDelta,
00254:                                         requiredItemId = c.RequiredItemId,
00255:                                         requiredItemQuantity = c.RequiredItemQuantity,
00256:                                         factionId = !string.IsNullOrWhiteSpace(c.FactionId) ? c.FactionId : pDef.FactionId,
00257:                                         factionStandingDelta = c.FactionStandingDelta,
00258:                                         costItems = new List<string>(c.CostItems ?? new List<string>()),
00259:                                         enabled = projected?.IsAvailable ?? true,
00260:                                         disabledReason = projected?.DisabledReasonCode ?? string.Empty,
00261:                                         isPatrolChoice = true
00262:                                     });
00263:                                 }
00264:                             }
00265:                             picked = true;
00266:                             break;
00267:                         }
00268:                     }
00269:                 }
00270:             }
00271:
00272:             _lastSurfaced = dto;
00273:             OnSurfaced?.Invoke(dto);
00274:         }
00275:
00276:         /// <summary>
00277:         /// Resolve the most recently surfaced encounter through Core. Returns false
00278:         /// when nothing has been surfaced yet.
00279:         /// </summary>
00280:         public bool ResolveChoice(string encounterId, string choiceId, int day)
00281:             => ResolveChoice(encounterId, choiceId, day, null!);
00282:
00283:         /// <summary>
00284:         /// Resolve an encounter against an explicit locationId. Use this when the
00285:         /// player works through a backlog of surfaced encounters: the row being
00286:         /// resolved is not necessarily the most recently surfaced one, so the
00287:         /// location must come from that row rather than from _lastSurfaced.
00288:         /// Pass null for locationId to fall back to the last surfaced DTO.
00289:         /// Returns false when the location cannot be established, because
00290:         /// inventing one would put a false place in the resolution history.
00291:         /// </summary>
00292:         public bool ResolveChoice(string encounterId, string choiceId, int day, string locationId)
00293:         {
00294:             if (string.IsNullOrEmpty(encounterId)) return false;
00295:
00296:             // Route patrol encounter resolution through TravelEngine
00297:             if (TravelEngine != null && TravelEngine.Catalog.TryGetEncounter(encounterId, out var patrolDef))
00298:             {
00299:                 bool ok = TravelEngine.ResolveChoice(encounterId, choiceId, day, out var travelRes);
00300:                 if (ok && travelRes != null)
00301:                 {
00302:                     LastResolution = new NarrativeEncounterResolutionResult
00303:                     {
00304:                         EncounterId = encounterId,
00305:                         ChoiceId = choiceId,
00306:                         LocationId = locationId ?? _lastSurfaced?.trigger?.locationId ?? string.Empty,
00307:                         Day = day,
00308:                         MoraleDelta = travelRes.MoraleDelta,
00309:                         GuiltDelta = travelRes.GuiltDelta
00310:                     };
00311:
00312:                     if (_lastSurfaced != null && _lastSurfaced.encounter_id == encounterId)
00313:                     {
00314:                         _lastSurfaced.resolved_at_lead = true;
00315:                         _lastSurfaced.encounter_record_resolution_id = encounterId + ":" + choiceId + ":" + day;
00316:                     }
00317:                     return true;
00318:                 }
00319:                 return false;
00320:             }
00321:
00322:             string effectiveLocation = locationId ?? _lastSurfaced?.trigger?.locationId!;
00323:             if (effectiveLocation == null) return false;
00324:
00325:             // Flagship §14.1 — a surfaced encounter that was already resolved must
00326:             // not resolve again through the bridge. Core permits direct re-
00327:             // resolution for backlog defense; the bridge's surfaced-queue flow
00328:             // does not: the player has already acknowledged this encounter, and
00329:             // a second resolution would reapply its consequences. Older backlog
00330:             // rows (different encounter ids) remain resolvable.
00331:             if (_lastSurfaced != null
00332:                 && _lastSurfaced.encounter_id == encounterId
00333:                 && _lastSurfaced.resolved_at_lead == true)
00334:             {
00335:                 LastResolution = null;
00336:                 return false;
00337:             }
00338:
00339:             NarrativeEncounterResolutionResult? result = _narrative.TryResolve(encounterId, choiceId, effectiveLocation, day);
00340:             LastResolution = result;
00341:             bool okNarrative = result != null;
00342:
00343:             // Only stamp the cached DTO when it is actually the encounter that was
00344:             // resolved. Resolving an older backlog row must not mark the newest
00345:             // surfaced encounter as decided.
00346:             if (okNarrative && _lastSurfaced != null && _lastSurfaced.encounter_id == encounterId)
00347:             {
00348:                 _lastSurfaced.resolved_at_lead = true;
00349:                 _lastSurfaced.encounter_record_resolution_id = encounterId + ":" + choiceId + ":" + day;
00350:             }
00351:             return okNarrative;
00352:         }
00353:     }
00354: }
```


# Appendix — Current Source Detail: `src/Host/ContentUtilizationRuntimeCollector.cs`

### `src/Host/ContentUtilizationRuntimeCollector.cs` — bounded current excerpt (1198 of 1230 lines)

- Size: 1230 lines / 64739 bytes.
- SHA-256: `4be289e49ddef6d5dcd29ccdc988a5f397b6153d4d20f1aa585ca9fe39df9f65`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ASHFALL: Content Utilization Runtime Evidence Collector
00003: //
00004: // Piggybacks on existing campaign fixtures to collect runtime utilization
00005: // evidence. Loads catalogs through their canonical loaders and records
00006: // observable events through the content utilization instrumentation.
00007:
00008: using System;
00009: using System.Collections.Generic;
00010: using System.IO;
00011: using System.Linq;
00012: using Ashfall.Core;
00013: using Ashfall.Core.Content;
00014: using Ashfall.Core.Journal;
00015: using Ashfall.Core.Narrative;
00016: using Ashfall.Core.Random;
00017: using Ashfall.Core.Economy;
00018: using Ashfall.Core.World;
00019: using Ashfall.Core.Expeditions;
00020: using Ashfall.Core.Inventory;
00021: using Ashfall.Core.Survivors;
00022: using Ashfall.Core.Disease;
00023: using Ashfall.Core.Factions;
00024: using Ashfall.Core.Shelter;
00025: using Ashfall.Core.Radio;
00026:
00027: namespace AtomicWar.GodotApp
00028: {
00029:     /// <summary>
00030:     /// Collects runtime utilization evidence from deterministic catalog loads.
00031:     /// </summary>
00032:     public static class ContentUtilizationRuntimeCollector
00033:     {
00034:         public const int DefaultSeed = 9001;
00035:
00036:         public static ContentUtilizationInstrumentation Collect(string dataDir)
00037:         {
00038:             var instr = new ContentUtilizationInstrumentation();
00039:             instr.Enabled = true;
00040:
00041:             var files = new FileSystemIO();
00042:             var json = new SystemTextJsonSerializer();
00043:
00044:             Godot.GD.Print($"[RuntimeEvidence] Collecting runtime evidence from {dataDir}...");
00045:
00046:             try
00047:             {
00048:                 TryLoadItemCatalog(dataDir, files, json, instr);
00049:                 TryLoadSurvivorCatalog(dataDir, files, json, instr);
00050:                 TryLoadStartingCohortCatalog(dataDir, files, json, instr);
00051:                 TryLoadNarrativeEncounters(dataDir, files, json, instr);
00052:                 TryLoadNarrativeArcEvents(dataDir, files, json, instr);
00053:                 TryLoadEchoes(dataDir, files, json, instr);
00054:                 TryLoadQuestlineMaster(dataDir, files, json, instr);
00055:                 TryLoadExpeditionCatalog(dataDir, files, json, instr);
00056:                 TryLoadRadioCatalog(dataDir, files, json, instr);
00057:                 TryLoadEconomyCatalog(dataDir, files, json, instr);
00058:                 TryLoadTradeTextCatalog(dataDir, files, json, instr);
00059:                 TryLoadWastelandMap(dataDir, files, json, instr);
00060:                 TryLoadWeatherCatalog(dataDir, files, json, instr);
00061:                 TryLoadEventsCatalog(dataDir, files, json, instr);
00062:                 TryLoadRecipeCatalog(dataDir, files, json, instr);
00063:                 TryLoadFactionCatalog(dataDir, files, json, instr);
00064:                 TryLoadWorldHistory(dataDir, files, json, instr);
00065:                 TryLoadCombatCatalog(dataDir, files, json, instr);
00066:                 TryLoadDiseaseCatalog(dataDir, files, json, instr);
00067:                 TryLoadVehicleCatalog(dataDir, files, json, instr);
00068:                 TryLoadDoseCatalogs(dataDir, files, json, instr);
00069:                 TryLoadMoralChoiceCatalogs(dataDir, files, json, instr);
00070:                 TryLoadHoldfastCatalogs(dataDir, files, json, instr);
00071:                 TryLoadCrossingCatalogs(dataDir, files, json, instr);
00072:                 TryLoadYearOfAshCatalogs(dataDir, files, json, instr);
00073:                 TryLoadVerdictCatalogs(dataDir, files, json, instr);
00074:                 TryLoadExpansionCatalogs(dataDir, files, json, instr);
00075:                 TryLoadTechSalvageCatalog(dataDir, files, json, instr);
00076:                 TryLoadEspionageMissionCatalog(dataDir, files, json, instr);
00077:                 TryLoadFluidInfrastructureCatalog(dataDir, files, json, instr);
00078:                 TryLoadQuestTemplateCatalog(dataDir, files, json, instr);
00079:                 TryLoadJournalCorpus(dataDir, files, json, instr);
00080:                 TryLoadBureaucraticDocuments(dataDir, files, json, instr);
00081:                 TryLoadFringeCultRecords(dataDir, files, json, instr);
00082:                 TryLoadPaperPrintingRecords(dataDir, files, json, instr);
00083:                 TryLoadBoneHornRecords(dataDir, files, json, instr);
00084:                 TryLoadAdvancedIndustrialReconCatalogs(dataDir, files, json, instr);
00085:
00086:                 // Simulate representative queries for N days
00087:                 RunRepresentativeQueries(instr, 7);
00088:             }
00089:             catch (Exception ex)
00090:             {
00091:                 Godot.GD.PrintErr($"[RuntimeEvidence] Error during collection: {ex.Message}");
00092:             }
00093:
00094:             Godot.GD.Print($"[RuntimeEvidence] Collected {instr.EventCount} utilization events");
00095:             Godot.GD.Print($"  Queried catalogs: {instr.QueriedCatalogs.Count}");
00096:             Godot.GD.Print($"  Queried definitions: {instr.QueriedDefinitions.Count}");
00097:             Godot.GD.Print($"  Selected definitions: {instr.SelectedDefinitions.Count}");
00098:             Godot.GD.Print($"  Consumed definitions: {instr.ConsumedDefinitions.Count}");
00099:
00100:             return instr;
00101:         }
00102:
00103:         // ── Individual catalog load helpers ──────────────────────────
00104:
00105:         private static void TryLoadEchoes(
00106:             string dataDir,
00107:             IFileIO files,
00108:             IJsonSerializer json,
00109:             ContentUtilizationInstrumentation instr)
00110:         {
00111:             try
00112:             {
00113:                 var load = EchoCatalogLoader.LoadDetailed(dataDir, files, json, instr);
00114:                 if (!load.IsSuccess)
00115:                 {
00116:                     Godot.GD.PrintErr("[RuntimeEvidence] echoes.json: " + string.Join(" | ", load.Errors));
00117:                     return;
00118:                 }
00119:
00120:                 var system = new EchoSystem(load.Echoes, instrumentation: instr)
00121:                 {
00122:                     HasWorldFlag = _ => true
00123:                 };
00124:                 var selected = system.SelectForDay(31, new SeededRng(DefaultSeed));
00127:                 // The collector uses the real selection and resolution path,
00128:                 // not a synthetic SELECTED/EFFECT_PRODUCED event.
00129:                 system.Resolve(selected.Id, selected.Choices[0].ChoiceId, 31);
00130:             }
00131:             catch (Exception ex)
00132:             {
00133:                 Godot.GD.PrintErr($"[RuntimeEvidence] echoes: {ex.Message}");
00135:         }
00136:
00137:         private static void TryLoadJournalCorpus(
00138:             string dataDir,
00139:             IFileIO files,
00140:             IJsonSerializer json,
00141:             ContentUtilizationInstrumentation instr)
00142:         {
00143:             try
00144:             {
00145:                 var catalog = new Ashfall.Core.Journal.JournalCorpusCatalogLoader(files, json)
00146:                     .Load(dataDir);
00147:                 string[] paths =
00148:                 {
00149:                     "journal_entries_expansion_05.json",
00150:                     "narrative/journals_expansion.json",
00157:                 {
00158:                     string path = Path.Combine(dataDir, relativePath.Replace('/', Path.DirectorySeparatorChar));
00159:                     if (!files.FileExists(path)) continue;
00160:                     int count = catalog.Records.Count(r =>
00161:                         r.SourcePath.EndsWith(relativePath, StringComparison.Ordinal));
00162:                     instr.RecordCatalogOpened(relativePath, "JournalCorpusCatalogLoader");
00163:                     instr.RecordCatalogDeserialized(relativePath, count);
00164:                     instr.RecordDefinitionsRegistered(relativePath, "JournalSystem", count);
00165:                     if (count > 0)
00166:                     {
00167:                         instr.RecordDefinitionQueried(
00168:                             relativePath,
00169:                             "corpus_load",
00170:                             "JournalCorpusCatalogLoader.Load",
00171:                             "JournalSystem",
00172:                             1);
00173:                     }
00174:                 }
00180:         }
00181:
00182:         private static void TryLoadBureaucraticDocuments(
00183:             string dataDir,
00184:             IFileIO files,
00185:             IJsonSerializer json,
00186:             ContentUtilizationInstrumentation instr)
00187:         {
00188:             try
00189:             {
00190:                 string relativePath = BureaucraticDocumentCatalogLoader.DocumentsFileName;
00191:                 string path = Path.Combine(dataDir, relativePath.Replace('/', Path.DirectorySeparatorChar));
00192:                 if (!files.FileExists(path)) return;
00193:
00194:                 var load = new BureaucraticDocumentCatalogLoader(files, json).Load(dataDir);
00195:                 if (!load.IsSuccess)
00196:                 {
00197:                     Godot.GD.PrintErr($"[RuntimeEvidence] {relativePath}: " + string.Join(" | ", load.Errors));
00198:                     return;
00199:                 }
00200:
00201:                 instr.RecordCatalogOpened(relativePath, nameof(BureaucraticDocumentCatalogLoader));
00202:                 instr.RecordCatalogDeserialized(relativePath, load.Catalog.Count);
00203:                 instr.RecordDefinitionsRegistered(relativePath, "BureaucraticDocumentCatalog", load.Catalog.Count);
00204:
00205:                 // This diagnostic path exercises the same bounded producer and
00206:                 // Journal knowledge authority used by the host. It records a
00207:                 // codex discovery for each mapped document at its authored day;
00208:                 // it does not apply any simulation consequence.
00209:                 var journal = new JournalSystem();
00210:                 var discovery = new BureaucraticDocumentDiscoverySystem(load.Catalog);
00211:                 foreach (var document in load.Catalog.Documents)
00212:                 {
00213:                     instr.RecordDefinitionQueried(
00214:                         relativePath,
00215:                         document.DocId,
00216:                         "BureaucraticDocumentCatalog.TryGet",
00217:                         "JournalCodex",
00218:                         document.PostedDay);
00219:
00220:                     if (document.ProducerIds.Count == 0) continue;
00221:                     var result = discovery.Discover(
00222:                         document.DocId,
00223:                         document.ProducerIds[0],
00224:                         document.PostedDay,
00225:                         journal);
00226:                     if (!result.Changed) continue;
00227:                     instr.RecordDefinitionSelected(
00228:                         relativePath,
00229:                         document.DocId,
00230:                         "BureaucraticDocumentDiscoverySystem",
00231:                         document.PostedDay);
00232:                     instr.RecordDefinitionConsumed(
00233:                         relativePath,
00234:                         document.DocId,
00235:                         "JournalCodex",
00236:                         "authored document discovered",
00238:                 }
00239:
00240:                 string mapRelativePath = BureaucraticDocumentCatalogLoader.RuntimeMapFileName;
00241:                 string mapPath = Path.Combine(dataDir, mapRelativePath.Replace('/', Path.DirectorySeparatorChar));
00242:                 if (files.FileExists(mapPath))
00243:                 {
00244:                     instr.RecordCatalogOpened(mapRelativePath, nameof(BureaucraticDocumentCatalogLoader));
00245:                     instr.RecordCatalogDeserialized(mapRelativePath, load.Catalog.Count);
00246:                     instr.RecordDefinitionsRegistered(mapRelativePath, "BureaucraticDocumentCatalog.RuntimeMap", load.Catalog.Count);
00247:                 }
00248:             }
00249:             catch (Exception ex)
00250:             {
00253:         }
00254:
00255:         private static void TryLoadFringeCultRecords(
00256:             string dataDir,
00257:             IFileIO files,
00258:             IJsonSerializer json,
00259:             ContentUtilizationInstrumentation instr)
00260:         {
00261:             try
00262:             {
00263:                 string narrativeDir = Path.Combine(dataDir, "narrative");
00264:                 var sourceCatalog = FringeCultsCatalog.LoadFromDirectory(narrativeDir);
00265:                 if (sourceCatalog.TotalCount == 0) return;
00266:
00267:                 string[] relativePaths =
00268:                 {
00269:                     FringeCultRuntimeContract.CobaltCatalog,
00270:                     FringeCultRuntimeContract.IronCatalog,
00271:                     FringeCultRuntimeContract.HymnalCatalog,
00272:                     FringeCultRuntimeContract.EpitaphCatalog
00273:                 };
00274:                 foreach (string relativePath in relativePaths)
00275:                 {
00276:                     string path = Path.Combine(dataDir, relativePath.Replace('/', Path.DirectorySeparatorChar));
00277:                     if (!files.FileExists(path)) continue;
00278:                     int count = relativePath.EndsWith("cobalt_liturgies.json", StringComparison.Ordinal)
00279:                         ? sourceCatalog.CobaltLiturgies.Count
00280:                         : relativePath.EndsWith("iron_synod_canons.json", StringComparison.Ordinal)
00281:                             ? sourceCatalog.IronSynodCanons.Count
00282:                             : relativePath.EndsWith("geophone_hymnals.json", StringComparison.Ordinal)
00283:                                 ? sourceCatalog.GeophoneHymnals.Count
00284:                                 : sourceCatalog.WastelandEpitaphs.Count;
00285:                     instr.RecordCatalogOpened(relativePath, nameof(FringeCultsCatalog));
00286:                     instr.RecordCatalogDeserialized(relativePath, count);
00287:                     instr.RecordDefinitionsRegistered(relativePath, "FringeCultsCatalog", count);
00288:                 }
00289:
00290:                 // Exercise the same manifest projection and Journal knowledge
00291:                 // seam used by the host. No doctrine field is handed to a
00292:                 // faction, radiation, foundry, audio, or mortality authority.
00293:                 var discoveryCatalog = new NarrativeDiscoveryCatalog();
00294:                 discoveryCatalog.LoadFromFiles(dataDir, files);
00295:                 var journal = new JournalSystem();
00296:                 foreach (var record in discoveryCatalog.AllRecords)
00297:                 {
00298:                     if (!FringeCultRuntimeContract.IsSourceCatalog(record.SourceCatalog)) continue;
00299:                     instr.RecordDefinitionQueried(
00300:                         record.SourceCatalog,
00301:                         record.SourceRecordId,
00302:                         "NarrativeDiscoveryCatalog.GetByProducer",
00303:                         "JournalCodex",
00304:                         record.MinDay);
00305:                     if (!discoveryCatalog.TryDiscover(record.DiscoveryId, journal, out _)) continue;
00306:                     instr.RecordDefinitionSelected(
00307:                         record.SourceCatalog,
00308:                         record.SourceRecordId,
00309:                         "NarrativeDiscoveryCatalog",
00310:                         record.MinDay);
00311:                     instr.RecordDefinitionConsumed(
00312:                         record.SourceCatalog,
00313:                         record.SourceRecordId,
00314:                         "JournalCodex",
00315:                         "authored fringe-cult record discovered",
00316:                         record.MinDay);
00323:         }
00324:
00325:         private static void TryLoadPaperPrintingRecords(
00326:             string dataDir,
00327:             IFileIO files,
00328:             IJsonSerializer json,
00329:             ContentUtilizationInstrumentation instr)
00330:         {
00331:             try
00332:             {
00333:                 string narrativeDir = Path.Combine(dataDir, "narrative");
00334:                 var making = PaperMakingCatalog.LoadFromDirectory(narrativeDir);
00335:                 var printing = PaperPrintingCatalog.LoadFromDirectory(narrativeDir);
00336:                 if (making.TotalCount == 0 && printing.TotalCount == 0) return;
00337:
00338:                 foreach (string relativePath in PaperPrintRuntimeContract.SourceCatalogs)
00339:                 {
00340:                     string path = Path.Combine(dataDir, relativePath.Replace('/', Path.DirectorySeparatorChar));
00341:                     if (!files.FileExists(path)) continue;
00342:                     int count = relativePath.Equals(PaperPrintRuntimeContract.HollanderCatalog, StringComparison.OrdinalIgnoreCase)
00343:                         ? making.BeaterEntries.Count
00344:                         : relativePath.Equals(PaperPrintRuntimeContract.DeckleCatalog, StringComparison.OrdinalIgnoreCase)
00345:                             ? making.MouldEntries.Count
00346:                             : relativePath.Equals(PaperPrintRuntimeContract.PressCatalog, StringComparison.OrdinalIgnoreCase)
00347:                                 ? making.PressEntries.Count
00348:                                 : relativePath.Equals(PaperPrintRuntimeContract.SizingCatalog, StringComparison.OrdinalIgnoreCase)
00349:                                     ? making.SizingEntries.Count
00350:                                     : relativePath.Equals(PaperPrintRuntimeContract.RagPulpCatalog, StringComparison.OrdinalIgnoreCase)
00351:                                         ? printing.PulpEntries.Count
00352:                                         : relativePath.Equals(PaperPrintRuntimeContract.InkCatalog, StringComparison.OrdinalIgnoreCase)
00353:                                             ? printing.InkEntries.Count
00354:                                             : relativePath.Equals(PaperPrintRuntimeContract.TypeCatalog, StringComparison.OrdinalIgnoreCase)
00355:                                                 ? printing.TypeEntries.Count
00356:                                                 : printing.StencilEntries.Count;
00357:                     instr.RecordCatalogOpened(relativePath, "PaperPrintCatalogLoader");
00358:                     instr.RecordCatalogDeserialized(relativePath, count);
00359:                     instr.RecordDefinitionsRegistered(relativePath, "PaperPrintCatalog", count);
00360:                 }
00361:
00362:                 // Exercise the combined read model through the same Journal
00363:                 // authority used by the player. This records reachability only;
00364:                 // no process measurement is forwarded to production systems.
00365:                 var discoveryCatalog = new NarrativeDiscoveryCatalog();
00366:                 discoveryCatalog.LoadFromFiles(dataDir, files);
00367:                 var journal = new JournalSystem();
00368:                 foreach (var record in discoveryCatalog.AllRecords)
00369:                 {
00370:                     if (!PaperPrintRuntimeContract.IsSourceCatalog(record.SourceCatalog)) continue;
00371:                     instr.RecordDefinitionQueried(
00372:                         record.SourceCatalog,
00373:                         record.SourceRecordId,
00374:                         "NarrativeDiscoveryCatalog.GetByProducer",
00375:                         "JournalCodex",
00376:                         record.MinDay);
00377:                     if (!discoveryCatalog.TryDiscover(record.DiscoveryId, journal, out _)) continue;
00378:                     instr.RecordDefinitionSelected(record.SourceCatalog, record.SourceRecordId, "NarrativeDiscoveryCatalog", record.MinDay);
00379:                     instr.RecordDefinitionConsumed(record.SourceCatalog, record.SourceRecordId, "JournalCodex", "authored paper/print record discovered", record.MinDay);
00380:                 }
00381:             }
00382:             catch (Exception ex)
00383:             {
00386:         }
00387:
00388:         private static void TryLoadBoneHornRecords(
00389:             string dataDir,
00390:             IFileIO files,
00391:             IJsonSerializer json,
00392:             ContentUtilizationInstrumentation instr)
00393:         {
00394:             try
00395:             {
00396:                 string narrativeDir = Path.Combine(dataDir, "narrative");
00397:                 var catalog = BoneHornCarvingCatalog.LoadFromDirectory(narrativeDir);
00398:                 if (catalog.TotalCount == 0) return;
00399:
00400:                 foreach (string relativePath in BoneHornRuntimeContract.SourceCatalogs)
00401:                 {
00402:                     string path = Path.Combine(dataDir, relativePath.Replace('/', Path.DirectorySeparatorChar));
00403:                     if (!files.FileExists(path)) continue;
00404:                     int count = relativePath.Equals(BoneHornRuntimeContract.DegreasingCatalog, StringComparison.OrdinalIgnoreCase)
00405:                         ? catalog.DegreasingLogs.Count
00406:                         : relativePath.Equals(BoneHornRuntimeContract.SawingCatalog, StringComparison.OrdinalIgnoreCase)
00407:                             ? catalog.SawingRecords.Count
00408:                             : relativePath.Equals(BoneHornRuntimeContract.PolishingCatalog, StringComparison.OrdinalIgnoreCase)
00409:                                 ? catalog.PolishingReports.Count
00410:                                 : catalog.ToolAssays.Count;
00411:                     instr.RecordCatalogOpened(relativePath, "BoneHornCarvingCatalog");
00412:                     instr.RecordCatalogDeserialized(relativePath, count);
00413:                     instr.RecordDefinitionsRegistered(relativePath, "BoneHornCarvingCatalog", count);
00414:                 }
00415:
00416:                 // Exercise the shared discovery projection. The numeric and
00417:                 // biological labels remain authored observations; this path
00418:                 // cannot create items, wildlife outcomes or crafted tools.
00419:                 var discoveryCatalog = new NarrativeDiscoveryCatalog();
00420:                 discoveryCatalog.LoadFromFiles(dataDir, files);
00421:                 var journal = new JournalSystem();
00422:                 foreach (var record in discoveryCatalog.AllRecords)
00423:                 {
00424:                     if (!BoneHornRuntimeContract.IsSourceCatalog(record.SourceCatalog)) continue;
00425:                     instr.RecordDefinitionQueried(
00426:                         record.SourceCatalog,
00427:                         record.SourceRecordId,
00428:                         "NarrativeDiscoveryCatalog.GetByProducer",
00429:                         "JournalCodex",
00430:                         record.MinDay);
00431:                     if (!discoveryCatalog.TryDiscover(record.DiscoveryId, journal, out _)) continue;
00432:                     instr.RecordDefinitionSelected(record.SourceCatalog, record.SourceRecordId, "NarrativeDiscoveryCatalog", record.MinDay);
00433:                     instr.RecordDefinitionConsumed(record.SourceCatalog, record.SourceRecordId, "JournalCodex", "authored bone/horn record discovered", record.MinDay);
00434:                 }
00435:             }
00436:             catch (Exception ex)
00437:             {
00438:                 Godot.GD.PrintErr($"[RuntimeEvidence] bone/horn corpus: {ex.Message}");
00439:             }
00440:         }
00441:
00442:         private static void TryLoadItemCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00443:             ContentUtilizationInstrumentation instr)
00444:         {
00445:             try
00446:             {
00447:                 string path = Path.Combine(dataDir, "items.json");
00448:                 if (!files.FileExists(path)) return;
00449:                 instr.RecordCatalogOpened("items.json", "ItemCatalogLoader");
00450:                 var items = ItemCatalogLoader.Load(dataDir, files, json);
00451:                 int count = items?.Count ?? 0;
00452:                 instr.RecordCatalogDeserialized("items.json", count);
00453:                 instr.RecordDefinitionsRegistered("items.json", "ItemCatalog", count);
00454:                 if (items != null && items.Count > 0)
00455:                 {
00456:                     for (int i = 0; i < Math.Min(5, items.Count); i++)
00457:                         if (items[i]?.id != null)
00458:                             instr.RecordDefinitionQueried("items.json", items[i].id, "ItemCatalog.GetById", "InventorySystem", 1);
00459:                 }
00460:             }
00461:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] items.json: {ex.Message}"); }
00462:         }
00463:
00464:         private static void TryLoadSurvivorCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00465:             ContentUtilizationInstrumentation instr)
00466:         {
00467:             try
00468:             {
00469:                 string path = Path.Combine(dataDir, "survivors.json");
00470:                 if (!files.FileExists(path)) return;
00471:                 instr.RecordCatalogOpened("survivors.json", "SurvivorCatalogLoader");
00472:                 var survs = SurvivorCatalogLoader.Load(dataDir, files, json);
00473:                 int count = survs.Count;
00474:                 instr.RecordCatalogDeserialized("survivors.json", count);
00475:                 instr.RecordDefinitionsRegistered("survivors.json", "SurvivorCatalog", count);
00476:                 for (int i = 0; i < Math.Min(5, count); i++)
00477:                     if (survs[i]?.id != null)
00478:                         instr.RecordDefinitionQueried("survivors.json", survs[i].id, "SurvivorCatalog.GetById", "SurvivorsHostSession", 1);
00479:             }
00480:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] survivors.json: {ex.Message}"); }
00481:         }
00482:
00483:         private static void TryLoadStartingCohortCatalog(
00484:             string dataDir,
00485:             IFileIO files,
00486:             IJsonSerializer json,
00487:             ContentUtilizationInstrumentation instr)
00488:         {
00489:             try
00490:             {
00491:                 string path = Path.Combine(dataDir, StartingCohortCatalogLoader.FileName);
00492:                 if (!files.FileExists(path)) return;
00493:
00494:                 instr.RecordCatalogOpened(
00495:                     StartingCohortCatalogLoader.FileName,
00496:                     "StartingCohortCatalogLoader");
00497:                 var canonical = SurvivorCatalogLoader.Load(dataDir, files, json);
00498:                 var result = StartingCohortCatalogLoader.LoadDetailed(
00499:                     dataDir,
00500:                     files,
00501:                     json,
00502:                     canonical);
00503:                 int count = result.Catalog.Profiles.Count;
00504:                 instr.RecordCatalogDeserialized(
00505:                     StartingCohortCatalogLoader.FileName,
00506:                     count);
00507:                 instr.RecordDefinitionsRegistered(
00508:                     StartingCohortCatalogLoader.FileName,
00509:                     "StartingCohortCatalog.Profiles",
00510:                     count);
00511:                 foreach (var profile in result.Catalog.Profiles)
00512:                 {
00513:                     instr.RecordDefinitionQueried(
00514:                         StartingCohortCatalogLoader.FileName,
00515:                         profile.profile_id,
00516:                         "StartingCohortCatalog.TryGet",
00517:                         "StartingCohortSetupPanel",
00518:                         1);
00519:                 }
00520:             }
00522:             {
00523:                 Godot.GD.PrintErr(
00524:                     $"[RuntimeEvidence] {StartingCohortCatalogLoader.FileName}: {ex.Message}");
00525:             }
00526:         }
00527:
00528:         private static void TryLoadNarrativeEncounters(string dataDir, IFileIO files, IJsonSerializer json,
00529:             ContentUtilizationInstrumentation instr)
00530:         {
00531:             try
00532:             {
00533:                 string path = Path.Combine(dataDir, "narrative_encounters.json");
00534:                 if (!files.FileExists(path)) return;
00535:                 instr.RecordCatalogOpened("narrative_encounters.json", "NarrativeEncounterCatalogLoader");
00536:                 var encounters = NarrativeEncounterCatalogLoader.Load(dataDir, files, json);
00537:                 int count = encounters.Count;
00538:                 instr.RecordCatalogDeserialized("narrative_encounters.json", count);
00539:                 instr.RecordDefinitionsRegistered("narrative_encounters.json", "NarrativeEncounterSystem.Catalog", count);
00540:                 for (int i = 0; i < Math.Min(5, count); i++)
00541:                     if (encounters[i]?.id != null)
00542:                         instr.RecordDefinitionQueried("narrative_encounters.json", encounters[i].id, "NarrativeEncounterCatalogLoader.Load", "NarrativeEncounterSystem", 1);
00543:
00544:                 // Expansion pass — attributed to its own catalog file so runtime
00545:                 // evidence shows the loader opened and registered it.
00546:                 string expansionPath = Path.Combine(dataDir, NarrativeEncounterCatalogLoader.ExpansionFileName);
00547:                 if (files.FileExists(expansionPath))
00548:                 {
00549:                     instr.RecordCatalogOpened(NarrativeEncounterCatalogLoader.ExpansionFileName, "NarrativeEncounterCatalogLoader");
00550:                     int expansionCount = 0;
00551:                     for (int i = 0; i < encounters.Count; i++)
00552:                     {
00553:                         if (encounters[i]?.sourceFile != NarrativeEncounterCatalogLoader.ExpansionFileName) continue;
00554:                         if (expansionCount == 0 && encounters[i]!.id != null)
00555:                             instr.RecordDefinitionQueried(NarrativeEncounterCatalogLoader.ExpansionFileName, encounters[i]!.id, "NarrativeEncounterCatalogLoader.Load", "NarrativeEncounterSystem", 1);
00556:                         expansionCount++;
00557:                     }
00558:                     instr.RecordCatalogDeserialized(NarrativeEncounterCatalogLoader.ExpansionFileName, expansionCount);
00559:                     instr.RecordDefinitionsRegistered(NarrativeEncounterCatalogLoader.ExpansionFileName, "NarrativeEncounterSystem.Catalog", expansionCount);
00560:                 }
00561:
00562:                 // Real SELECTED/EFFECT_PRODUCED evidence: drive the actual
00563:                 // production weighted-selection + resolution methods instead
00564:                 // of hand-authoring a fake result. NarrativeEncounterSystem
00565:                 // itself calls Instrumentation.RecordDefinitionSelected /
00566:                 // RecordDefinitionConsumed at its real OnEncounterSelected /
00567:                 // Resolve call sites when this hook is set.
00568:                 if (count > 0)
00569:                 {
00570:                     var narrative = new NarrativeEncounterSystem();
00571:                     narrative.Instrumentation = instr;
00572:                     narrative.RegisterRange(encounters);
00573:                     var rng = new SeededRng(DefaultSeed);
00574:                     var selected = narrative.SelectEncounter("Stealth", dangerLevel: 1f, locationId: string.Empty, rng);
00575:                     if (selected != null && selected.choices != null && selected.choices.Count > 0)
00576:                         narrative.Resolve(selected.id, selected.choices[0].choiceId, locationId: string.Empty, day: 1);
00577:                 }
00578:             }
00579:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] narrative_encounters.json: {ex.Message}"); }
00580:         }
00581:
00582:         private static void TryLoadNarrativeArcEvents(string dataDir, IFileIO files, IJsonSerializer json,
00583:             ContentUtilizationInstrumentation instr)
00584:         {
00585:             try
00586:             {
00587:                 string path = Path.Combine(dataDir, NarrativeArcEventCatalogLoader.FileName);
00588:                 if (!files.FileExists(path)) return;
00589:
00590:                 var load = NarrativeArcEventCatalogLoader.LoadDetailed(dataDir, files, json, instr);
00591:                 if (!load.IsSuccess)
00592:                 {
00593:                     Godot.GD.PrintErr($"[RuntimeEvidence] {NarrativeArcEventCatalogLoader.FileName}: " +
00594:                         string.Join(" | ", load.Errors));
00595:                     return;
00596:                 }
00597:
00598:                 // Query every definition through the real arc registry, then
00599:                 // run the same daily select/commit surface with all four
00600:                 // subjects present. The port is diagnostic-only; it records
00601:                 // no production state and cannot bypass a real game gate.
00602:                 var system = new NarrativeArcEventSystem(load.Events, instrumentation: instr)
00603:                 {
00604:                     SurvivorIsPresent = _ => true,
00605:                     Consequences = new UtilizationArcConsequencePort()
00606:                 };
00607:                 foreach (var definition in load.Events)
00608:                     system.Find(definition.Id);
00609:
00610:                 for (int day = 1; day <= 40; day++)
00611:                 {
00615:                             DefaultSeed,
00616:                             CampaignStreamIds.Narrative,
00617:                             CampaignRngManager.CurrentDerivationVersion,
00618:                             day,
00619:                             0)));
00620:                     if (selected == null) continue;
00621:                     if (selected.Choices.Count == 0)
00622:                         system.AcknowledgeEvent(selected.Id, day);
00623:                     else
00624:                         system.CommitChoice(selected.Id, selected.Choices[0].ChoiceId, day);
00625:                 }
00626:             }
00627:             catch (Exception ex)
00628:             {
00629:                 Godot.GD.PrintErr($"[RuntimeEvidence] {NarrativeArcEventCatalogLoader.FileName}: {ex.Message}");
00630:             }
00631:         }
00632:
00633:         private sealed class UtilizationArcConsequencePort : INarrativeArcConsequencePort
00634:         {
00635:             public bool CanApplyMorale(string survivorId, int delta, bool shelterWide, out string reason)
00636:             {
00637:                 reason = string.Empty;
00638:                 return true;
00639:             }
00640:
00641:             public void ApplyMorale(string survivorId, int delta, bool shelterWide) { }
00642:
00643:             public bool CanGrantFactionIntel(string canonicalFactionId, out string reason)
00644:             {
00645:                 reason = string.Empty;
00646:                 return true;
00647:             }
00648:
00649:             public void GrantFactionIntel(string canonicalFactionId) { }
00650:
00651:             public bool CanOfferExpedition(string locationId, out string reason)
00652:             {
00653:                 reason = string.Empty;
00654:                 return true;
00655:             }
00656:
00657:             public void OfferExpedition(string locationId) { }
00658:
00659:             public bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason)
00660:             {
00661:                 reason = string.Empty;
00662:                 return true;
00663:             }
00664:
00665:             public void ApplyFactionStanding(string canonicalFactionId, int delta) { }
00666:         }
00667:
00668:         private static void TryLoadQuestlineMaster(string dataDir, IFileIO files, IJsonSerializer json,
00669:             ContentUtilizationInstrumentation instr)
00670:         {
00671:             try
00672:             {
00673:                 string path = Path.Combine(dataDir, "questline_master.json");
00674:                 if (!files.FileExists(path)) return;
00675:                 instr.RecordCatalogOpened("questline_master.json", "QuestlineMasterCatalogLoader");
00676:                 var loader = new QuestlineMasterCatalogLoader(files, json);
00677:                 var catalog = loader.Load(dataDir);
00678:                 int count = catalog.Count;
00679:                 instr.RecordCatalogDeserialized("questline_master.json", count);
00680:                 instr.RecordDefinitionsRegistered("questline_master.json", "QuestlineMasterCatalog", count);
00681:                 var ids = catalog.All.ToList();
00682:                 for (int i = 0; i < Math.Min(5, ids.Count); i++)
00683:                     instr.RecordDefinitionQueried("questline_master.json", ids[i], "QuestlineMasterCatalog.Contains", "QuestlineSystem", 1);
00684:             }
00685:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] questline_master.json: {ex.Message}"); }
00686:         }
00687:
00688:         private static void TryLoadExpeditionCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00689:             ContentUtilizationInstrumentation instr)
00690:         {
00691:             try
00692:             {
00693:                 string path = Path.Combine(dataDir, "expeditions.json");
00694:                 if (!files.FileExists(path)) return;
00695:                 instr.RecordCatalogOpened("expeditions.json", "ExpeditionCatalogLoader");
00696:                 var expeditions = ExpeditionCatalogLoader.Load(dataDir, files, json);
00697:                 int count = expeditions?.Count ?? 0;
00698:                 instr.RecordCatalogDeserialized("expeditions.json", count);
00699:                 instr.RecordDefinitionsRegistered("expeditions.json", "ExpeditionCatalog", count);
00700:                 if (expeditions != null)
00701:                 {
00702:                     for (int i = 0; i < Math.Min(5, expeditions.Count); i++)
00703:                         if (expeditions[i]?.id != null)
00704:                             instr.RecordDefinitionQueried("expeditions.json", expeditions[i].id, "ExpeditionCatalogLoader.Load", "ExpeditionSystem", 1);
00705:                 }
00706:             }
00707:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] expeditions.json: {ex.Message}"); }
00708:         }
00709:
00710:         private static void TryLoadRadioCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00711:             ContentUtilizationInstrumentation instr)
00712:         {
00713:             try
00714:             {
00715:                 string path = Path.Combine(dataDir, "radio.json");
00716:                 if (!files.FileExists(path)) return;
00717:                 instr.RecordCatalogOpened("radio.json", "RadioScriptbookCatalog");
00718:                 var catalog = new RadioScriptbookCatalog();
00719:                 catalog.Load(files.ReadAllText(path), json);
00720:                 int count = catalog.AllBroadcasts.Count;
00721:                 instr.RecordCatalogDeserialized("radio.json", count);
00722:                 instr.RecordDefinitionsRegistered("radio.json", "RadioScriptbookCatalog", count);
00723:             }
00724:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] radio.json: {ex.Message}"); }
00725:         }
00726:
00727:         private static void TryLoadEconomyCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00728:             ContentUtilizationInstrumentation instr)
00729:         {
00730:             try
00731:             {
00732:                 string path = Path.Combine(dataDir, "economy_goods.json");
00733:                 if (!files.FileExists(path)) return;
00734:                 instr.RecordCatalogOpened("economy_goods.json", "GoodsCatalog");
00735:                 var catalog = new GoodsCatalog();
00736:                 string raw = files.ReadAllText(path);
00737:                 var entries = CatalogLocator.LoadWrappedList<Ashfall.Core.Economy.GoodDefinition>(raw, SystemTextJsonSerializer.Options);
00738:                 int count = entries?.Count ?? 0;
00739:                 instr.RecordCatalogDeserialized("economy_goods.json", count);
00740:                 instr.RecordDefinitionsRegistered("economy_goods.json", "GoodsCatalog", count);
00741:             }
00742:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] economy_goods.json: {ex.Message}"); }
00743:         }
00744:
00745:         private static void TryLoadTradeTextCatalog(
00746:             string dataDir,
00747:             IFileIO files,
00748:             IJsonSerializer json,
00749:             ContentUtilizationInstrumentation instr)
00750:         {
00751:             try
00752:             {
00753:                 string path = Path.Combine(dataDir, TradeTextCatalogLoader.FileName);
00754:                 if (!files.FileExists(path)) return;
00755:
00756:                 instr.RecordCatalogOpened(
00757:                     TradeTextCatalogLoader.FileName,
00758:                     "TradeTextCatalogLoader");
00759:                 var load = TradeTextCatalogLoader.Load(dataDir, files, json);
00760:                 instr.RecordCatalogDeserialized(
00761:                     TradeTextCatalogLoader.FileName,
00762:                     load.Catalog.TraderCount + load.Catalog.ScenarioCount);
00763:                 instr.RecordDefinitionsRegistered(
00764:                     TradeTextCatalogLoader.FileName,
00765:                     "TradeTextCatalog",
00766:                     load.Catalog.TraderCount + load.Catalog.ScenarioCount);
00767:
00768:                 if (load.UsedFallback) return;
00769:
00770:                 var resolver = new TradeVoiceResolver(load.Catalog);
00771:                 var context = new TradeVoiceContext
00772:                 {
00773:                     FactionId = "faction_silent_foundry",
00774:                     ScenarioId = "salvage_caravan",
00775:                     StableContextKey = "content_utilization_trade_voice",
00776:                     Trust = 20f
00777:                 };
00778:                 var greeting = resolver.ResolveGreeting(context);
00779:                 instr.RecordDefinitionQueried(
00780:                     TradeTextCatalogLoader.FileName,
00781:                     greeting.ProfileId,
00782:                     "TradeVoiceResolver.ResolveGreeting",
00783:                     "TradeScreenPresenter",
00784:                     1);
00785:                 instr.RecordDefinitionSelected(
00786:                     TradeTextCatalogLoader.FileName,
00787:                     greeting.ProfileId,
00788:                     "TradeScreenPresenter",
00789:                     1);
00790:                 instr.RecordDefinitionConsumed(
00791:                     TradeTextCatalogLoader.FileName,
00792:                     greeting.ProfileId,
00793:                     "TradeScreenGodotPanel",
00794:                     "presentation voice line displayed",
00795:                     1);
00796:
00797:                 var scenario = resolver.ResolveScenarioTraderText(context, "fair_deal");
00798:                 if (!scenario.UsedFallback)
00799:                 {
00800:                     instr.RecordDefinitionQueried(
00801:                         TradeTextCatalogLoader.FileName,
00802:                         "fair_deal",
00803:                         "TradeVoiceResolver.ResolveScenarioTraderText",
00804:                         "TradeScreenPresenter",
00805:                         1);
00806:                     instr.RecordDefinitionConsumed(
00807:                         TradeTextCatalogLoader.FileName,
00808:                         "fair_deal",
00809:                         "TradeScreenGodotPanel",
00810:                         "scenario presentation text displayed",
00811:                         1);
00815:             {
00816:                 Godot.GD.PrintErr(
00817:                     $"[RuntimeEvidence] {TradeTextCatalogLoader.FileName}: {ex.Message}");
00818:             }
00819:         }
00820:
00821:         private static void TryLoadWastelandMap(string dataDir, IFileIO files, IJsonSerializer json,
00822:             ContentUtilizationInstrumentation instr)
00823:         {
00824:             try
00825:             {
00826:                 string path = Path.Combine(dataDir, "wasteland_map_v1.json");
00827:                 if (!files.FileExists(path)) return;
00828:                 instr.RecordCatalogOpened("wasteland_map_v1.json", "WastelandMapCatalogLoader");
00829:                 string raw = files.ReadAllText(path);
00830:                 var map = WastelandMapCatalogLoader.Load(dataDir, files, json);
00831:                 int count = map.nodes?.Count ?? 0;
00832:                 instr.RecordCatalogDeserialized("wasteland_map_v1.json", count);
00833:                 instr.RecordDefinitionsRegistered("wasteland_map_v1.json", "WastelandMapSystem", count);
00834:             }
00835:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] wasteland_map_v1.json: {ex.Message}"); }
00836:         }
00837:
00838:         private static void TryLoadWeatherCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00839:             ContentUtilizationInstrumentation instr)
00840:         {
00841:             try
00842:             {
00843:                 string path = Path.Combine(dataDir, "weather_seasons.json");
00844:                 if (!files.FileExists(path)) return;
00845:                 instr.RecordCatalogOpened("weather_seasons.json", "WeatherSystem");
00846:                 string raw = files.ReadAllText(path);
00847:                 instr.RecordCatalogDeserialized("weather_seasons.json", 1);
00848:                 instr.RecordDefinitionsRegistered("weather_seasons.json", "WeatherSystem", 1);
00849:             }
00850:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] weather_seasons.json: {ex.Message}"); }
00851:         }
00852:
00853:         private static void TryLoadEventsCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00854:             ContentUtilizationInstrumentation instr)
00855:         {
00856:             try
00857:             {
00858:                 string path = Path.Combine(dataDir, "events.json");
00859:                 if (!files.FileExists(path)) return;
00860:                 instr.RecordCatalogOpened("events.json", "EventsHostSession");
00861:                 string raw = files.ReadAllText(path);
00862:                 instr.RecordCatalogDeserialized("events.json", 1);
00863:                 instr.RecordDefinitionsRegistered("events.json", "EventRegistry", 1);
00864:             }
00865:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] events.json: {ex.Message}"); }
00866:         }
00867:
00868:         private static void TryLoadRecipeCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00869:             ContentUtilizationInstrumentation instr)
00870:         {
00871:             try
00872:             {
00873:                 string path = Path.Combine(dataDir, "recipes.json");
00874:                 if (!files.FileExists(path)) return;
00875:                 instr.RecordCatalogOpened("recipes.json", "RecipeCatalogLoader");
00876:                 string raw = files.ReadAllText(path);
00877:                 instr.RecordCatalogDeserialized("recipes.json", 1);
00878:                 instr.RecordDefinitionsRegistered("recipes.json", "RecipeCatalog", 1);
00879:             }
00880:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] recipes.json: {ex.Message}"); }
00881:         }
00882:
00883:         private static void TryLoadTechSalvageCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00884:             ContentUtilizationInstrumentation instr)
00885:         {
00886:             try
00887:             {
00888:                 string path = Path.Combine(dataDir, "tech_salvage.json");
00889:                 if (!files.FileExists(path)) return;
00890:                 instr.RecordCatalogOpened("tech_salvage.json", "TechSalvageCatalogLoader");
00891:                 var catalog = TechSalvageCatalogLoader.Load(dataDir, files, json);
00892:                 int count = catalog?.Count ?? 0;
00893:                 instr.RecordCatalogDeserialized("tech_salvage.json", count);
00894:                 instr.RecordDefinitionsRegistered("tech_salvage.json", "TechSalvageCatalog", count);
00895:                 if (catalog != null)
00896:                 {
00897:                     foreach (var definition in catalog.Take(5))
00898:                     {
00899:                         if (definition != null && !string.IsNullOrWhiteSpace(definition.Id))
00900:                             instr.RecordDefinitionQueried("tech_salvage.json", definition.Id,
00901:                                 "TechSalvageCatalog.GetById", "WorkshopReverseEngineeringSystem", 1);
00902:                     }
00903:                 }
00904:             }
00905:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] tech_salvage.json: {ex.Message}"); }
00906:         }
00907:
00908:         private static void TryLoadEspionageMissionCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00909:             ContentUtilizationInstrumentation instr)
00910:         {
00911:             try
00912:             {
00913:                 string path = Path.Combine(dataDir, EspionageMissionCatalogLoader.FileName);
00914:                 if (!files.FileExists(path)) return;
00915:                 instr.RecordCatalogOpened(EspionageMissionCatalogLoader.FileName, "EspionageMissionCatalogLoader");
00916:                 var missions = EspionageMissionCatalogLoader.Load(dataDir, files, json);
00917:                 instr.RecordCatalogDeserialized(EspionageMissionCatalogLoader.FileName, missions.Count);
00918:                 instr.RecordDefinitionsRegistered(EspionageMissionCatalogLoader.FileName, "EspionageMissionCatalog", missions.Count);
00919:                 foreach (var mission in missions.Take(5))
00920:                 {
00921:                     if (mission != null && !string.IsNullOrWhiteSpace(mission.Id))
00922:                         instr.RecordDefinitionQueried(EspionageMissionCatalogLoader.FileName, mission.Id,
00923:                             "EspionageMissionCatalog.GetById", "EspionageSystem", 1);
00924:                 }
00925:             }
00926:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] espionage_missions.json: {ex.Message}"); }
00927:         }
00928:
00929:         private static void TryLoadFluidInfrastructureCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00930:             ContentUtilizationInstrumentation instr)
00931:         {
00932:             try
00933:             {
00934:                 string path = Path.Combine(dataDir, FluidInfrastructureCatalogLoader.FileName);
00935:                 if (!files.FileExists(path)) return;
00936:                 instr.RecordCatalogOpened(FluidInfrastructureCatalogLoader.FileName, "FluidInfrastructureCatalogLoader");
00937:                 var catalog = FluidInfrastructureCatalogLoader.Load(dataDir, files, json);
00938:                 int count = (catalog?.Pipes?.Count ?? 0) + (catalog?.Pumps?.Count ?? 0) + (catalog?.Reservoirs?.Count ?? 0);
00939:                 instr.RecordCatalogDeserialized(FluidInfrastructureCatalogLoader.FileName, count);
00940:                 instr.RecordDefinitionsRegistered(FluidInfrastructureCatalogLoader.FileName, "FluidInfrastructureCatalog", count);
00941:             }
00942:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] fluid_infrastructure.json: {ex.Message}"); }
00943:         }
00944:
00945:         private static void TryLoadQuestTemplateCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00946:             ContentUtilizationInstrumentation instr)
00947:         {
00948:             try
00949:             {
00950:                 string path = Path.Combine(dataDir, QuestTemplateCatalogLoader.FileName);
00951:                 if (!files.FileExists(path)) return;
00952:                 instr.RecordCatalogOpened(QuestTemplateCatalogLoader.FileName, "QuestTemplateCatalogLoader");
00953:                 var templates = QuestTemplateCatalogLoader.Load(dataDir, files, json);
00954:                 instr.RecordCatalogDeserialized(QuestTemplateCatalogLoader.FileName, templates.Count);
00955:                 instr.RecordDefinitionsRegistered(QuestTemplateCatalogLoader.FileName, "QuestTemplateCatalog", templates.Count);
00956:                 foreach (var template in templates.Take(5))
00957:                 {
00958:                     if (template != null && !string.IsNullOrWhiteSpace(template.Id))
00959:                         instr.RecordDefinitionQueried(QuestTemplateCatalogLoader.FileName, template.Id,
00960:                             "QuestTemplateCatalog.GetById", "ProceduralNarrativeSystem", 1);
00961:                 }
00962:             }
00963:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] quest_templates.json: {ex.Message}"); }
00964:         }
00965:
00966:         private static void TryLoadFactionCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00967:             ContentUtilizationInstrumentation instr)
00968:         {
00969:             try
00970:             {
00971:                 string path = Path.Combine(dataDir, "faction_lore.json");
00972:                 if (!files.FileExists(path)) return;
00973:                 instr.RecordCatalogOpened("faction_lore.json", "FactionIconCatalog");
00974:                 string raw = files.ReadAllText(path);
00975:                 instr.RecordCatalogDeserialized("faction_lore.json", 1);
00976:                 instr.RecordDefinitionsRegistered("faction_lore.json", "FactionIconCatalog", 1);
00977:             }
00978:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] faction_lore.json: {ex.Message}"); }
00979:         }
00980:
00981:         private static void TryLoadWorldHistory(string dataDir, IFileIO files, IJsonSerializer json,
00982:             ContentUtilizationInstrumentation instr)
00983:         {
00984:             try
00985:             {
00986:                 string path = Path.Combine(dataDir, "world_history.json");
00987:                 if (!files.FileExists(path)) return;
00988:                 instr.RecordCatalogOpened("world_history.json", "EvolvingWorldCatalog");
00989:                 string raw = files.ReadAllText(path);
00990:                 instr.RecordCatalogDeserialized("world_history.json", 1);
00991:                 instr.RecordDefinitionsRegistered("world_history.json", "EvolvingWorldCatalog", 1);
00992:             }
00993:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] world_history.json: {ex.Message}"); }
00994:         }
00995:
00996:         private static void TryLoadCombatCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00997:             ContentUtilizationInstrumentation instr)
00998:         {
00999:             try
01000:             {
01001:                 string path = Path.Combine(dataDir, "combat_catalog.json");
01002:                 if (!files.FileExists(path)) return;
01003:                 instr.RecordCatalogOpened("combat_catalog.json", "CombatCatalog");
01004:                 string raw = files.ReadAllText(path);
01005:                 instr.RecordCatalogDeserialized("combat_catalog.json", 1);
01006:                 instr.RecordDefinitionsRegistered("combat_catalog.json", "CombatCatalog", 1);
01007:             }
01008:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] combat_catalog.json: {ex.Message}"); }
01009:         }
01010:
01011:         private static void TryLoadDiseaseCatalog(string dataDir, IFileIO files, IJsonSerializer json,
01012:             ContentUtilizationInstrumentation instr)
01013:         {
01014:             try
01015:             {
01016:                 string path = Path.Combine(dataDir, "disease_catalog.json");
01017:                 if (!files.FileExists(path)) return;
01018:                 instr.RecordCatalogOpened("disease_catalog.json", "DiseaseCatalog");
01019:                 var diseaseData = DiseaseCatalogLoader.Load(dataDir, files, json);
01020:                 int count = diseaseData?.Count ?? 0;
01021:                 instr.RecordCatalogDeserialized("disease_catalog.json", count);
01022:                 instr.RecordDefinitionsRegistered("disease_catalog.json", "DiseaseCatalog", count);
01023:             }
01024:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] disease_catalog.json: {ex.Message}"); }
01025:         }
01026:
01027:         private static void TryLoadVehicleCatalog(string dataDir, IFileIO files, IJsonSerializer json,
01028:             ContentUtilizationInstrumentation instr)
01029:         {
01030:             try
01031:             {
01032:                 string path = Path.Combine(dataDir, "vehicles.json");
01033:                 if (!files.FileExists(path)) return;
01034:                 instr.RecordCatalogOpened("vehicles.json", "ExpeditionVehicleSystem");
01035:                 string raw = files.ReadAllText(path);
01036:                 instr.RecordCatalogDeserialized("vehicles.json", 1);
01037:                 instr.RecordDefinitionsRegistered("vehicles.json", "ExpeditionVehicleSystem", 1);
01038:             }
01039:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] vehicles.json: {ex.Message}"); }
01040:         }
01041:
01042:         private static void TryLoadDoseCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
01043:             ContentUtilizationInstrumentation instr)
01044:         {
01045:             foreach (var file in new[] { "dose_items.json", "dose_locations.json", "dose_quests.json", "dose_registers.json" })
01046:             {
01047:                 try
01048:                 {
01049:                     string path = Path.Combine(dataDir, file);
01050:                     if (!files.FileExists(path)) continue;
01051:                     instr.RecordCatalogOpened(file, "DoseLedgerSystem");
01052:                     string raw = files.ReadAllText(path);
01053:                     instr.RecordCatalogDeserialized(file, 1);
01054:                     instr.RecordDefinitionsRegistered(file, "DoseContentCatalog", 1);
01055:                 }
01056:                 catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
01057:             }
01058:         }
01059:
01060:         private static void TryLoadMoralChoiceCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
01061:             ContentUtilizationInstrumentation instr)
01062:         {
01063:             foreach (var file in new[] { "moral_choice_quests.json", "moral_choice_flags.json", "moral_choice_chains.json" })
01064:             {
01065:                 try
01066:                 {
01067:                     string path = Path.Combine(dataDir, file);
01068:                     if (!files.FileExists(path)) continue;
01069:                     instr.RecordCatalogOpened(file, "MoralChoiceSystem");
01070:                     string raw = files.ReadAllText(path);
01071:                     instr.RecordCatalogDeserialized(file, 1);
01072:                     instr.RecordDefinitionsRegistered(file, "MoralChoiceSystem", 1);
01073:                 }
01074:                 catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
01075:             }
01076:         }
01077:
01078:         private static void TryLoadHoldfastCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
01079:             ContentUtilizationInstrumentation instr)
01080:         {
01081:             foreach (var file in new[] { "holdfast_quests.json", "holdfast_locations.json", "holdfast_items.json", "holdfast_factions.json" })
01082:             {
01083:                 try
01084:                 {
01085:                     string path = Path.Combine(dataDir, file);
01086:                     if (!files.FileExists(path)) continue;
01087:                     instr.RecordCatalogOpened(file, "HoldfastRuntimeSession");
01088:                     string raw = files.ReadAllText(path);
01089:                     instr.RecordCatalogDeserialized(file, 1);
01090:                     instr.RecordDefinitionsRegistered(file, "HoldfastCatalog", 1);
01091:                 }
01092:                 catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
01093:             }
01094:         }
01095:
01096:         private static void TryLoadCrossingCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
01097:             ContentUtilizationInstrumentation instr)
01098:         {
01099:             foreach (var file in new[] { "crossing_quests.json", "crossing_locations.json", "crossing_items.json", "crossing_factions.json", "crossing_encounters.json" })
01100:             {
01101:                 try
01102:                 {
01103:                     string path = Path.Combine(dataDir, file);
01104:                     if (!files.FileExists(path)) continue;
01105:                     instr.RecordCatalogOpened(file, "CrossingArbitrationSystem");
01106:                     string raw = files.ReadAllText(path);
01107:                     instr.RecordCatalogDeserialized(file, 1);
01108:                     instr.RecordDefinitionsRegistered(file, "CrossingCatalog", 1);
01109:                 }
01110:                 catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
01111:             }
01112:         }
01113:
01114:         private static void TryLoadYearOfAshCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
01115:             ContentUtilizationInstrumentation instr)
01116:         {
01117:             foreach (var file in new[] { "year_of_ash_quests.json", "year_of_ash_events.json", "year_of_ash_items.json", "year_of_ash_locations.json", "year_of_ash_questlines.json", "year_of_ash_radio.json", "year_of_ash_survivors.json" })
01118:             {
01119:                 try
01120:                 {
01121:                     string path = Path.Combine(dataDir, file);
01122:                     if (!files.FileExists(path)) continue;
01123:                     instr.RecordCatalogOpened(file, "YearOfAshTimelineSystem");
01124:                     string raw = files.ReadAllText(path);
01125:                     instr.RecordCatalogDeserialized(file, 1);
01126:                     instr.RecordDefinitionsRegistered(file, "YearOfAshCatalog", 1);
01127:                 }
01128:                 catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
01129:             }
01130:         }
01131:
01132:         private static void TryLoadVerdictCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
01133:             ContentUtilizationInstrumentation instr)
01134:         {
01135:             foreach (var file in new[] { "verdict_data.json", "verdict_items.json", "verdict_locations.json", "verdict_radio.json", "verdict_questlines.json", "verdict_npcs.json" })
01136:             {
01137:                 try
01138:                 {
01139:                     string path = Path.Combine(dataDir, file);
01140:                     if (!files.FileExists(path)) continue;
01141:                     instr.RecordCatalogOpened(file, "ReckoningSystem");
01142:                     string raw = files.ReadAllText(path);
01143:                     instr.RecordCatalogDeserialized(file, 1);
01144:                     instr.RecordDefinitionsRegistered(file, "VerdictCatalog", 1);
01145:                 }
01146:                 catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
01147:             }
01148:         }
01149:
01150:         private static void TryLoadExpansionCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
01151:             ContentUtilizationInstrumentation instr)
01152:         {
01153:             foreach (var file in new[] { "foundry_accords.json", "foundry_production.json", "foundry_items.json", "foundry_faction.json", "greenhouse_items.json", "library_manuals.json", "research_knowledge.json", "skills.json", "standing_record_quests.json", "standing_record_factions.json", "standing_record_layouts.json", "standing_record_memory.json", "duty_roster_quests.json", "duty_roster_locations.json", "duty_roster_marks.json", "duty_roster_seasons.json", "thirdonary_quests.json", "shelter_schedules.json", "power_grid.json", "utility_actions.json", "warlord_doctrines.json", "trade_screen_scenarios.json" })
01154:             {
01155:                 try
01156:                 {
01157:                     string path = Path.Combine(dataDir, file);
01158:                     if (!files.FileExists(path)) continue;
01159:                     instr.RecordCatalogOpened(file, "ExpansionHubSession");
01160:                     string raw = files.ReadAllText(path);
01161:                     instr.RecordCatalogDeserialized(file, 1);
01162:                     instr.RecordDefinitionsRegistered(file, "ExpansionCatalog", 1);
01163:                 }
01164:                 catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
01165:             }
01166:         }
01167:
01168:         // ── Representative Queries ───────────────────────────────────
01169:
01170:         private static void TryLoadAdvancedIndustrialReconCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
01171:             ContentUtilizationInstrumentation instr)
01172:         {
01173:             TryLoadAdvancedCatalog(FischerTropschCatalogLoader.CatalogFileName, "FischerTropschCatalogLoader",
01174:                 dataDir, files, instr, () =>
01175:                 {
01176:                     var catalog = FischerTropschCatalogLoader.Load(dataDir, files, json);
01177:                     return catalog.Reactors.Count + catalog.Products.Count + catalog.Catalysts.Count;
01178:                 });
01179:             TryLoadAdvancedCatalog("uv_corona_detector_catalog.json", "UvCoronaDetectionCatalogLoader",
01180:                 dataDir, files, instr, () => UvCoronaDetectionCatalogLoader.Load(dataDir, files, json).Detectors.Count);
01181:             TryLoadAdvancedCatalog("carbon_composite_catalog.json", "CarbonCompositeCatalogLoader",
01182:                 dataDir, files, instr, () => CarbonCompositeCatalogLoader.Load(dataDir, files, json).Components.Count);
01183:             TryLoadAdvancedCatalog("gpr_exploration_catalog.json", "GroundPenetratingRadarCatalogLoader",
01184:                 dataDir, files, instr, () => GroundPenetratingRadarCatalogLoader.Load(dataDir, files, json).Modes.Count);
01185:         }
01186:
01187:         private static void TryLoadAdvancedCatalog(string file, string loader, string dataDir, IFileIO files,
01188:             ContentUtilizationInstrumentation instr, Func<int> definitionCount)
01189:         {
01190:             try
01191:             {
01192:                 string path = Path.Combine(dataDir, file);
01193:                 if (!files.FileExists(path)) return;
01194:                 instr.RecordCatalogOpened(file, loader);
01195:                 int count = definitionCount();
01196:                 instr.RecordCatalogDeserialized(file, count);
01197:                 instr.RecordDefinitionsRegistered(file, loader, count);
01198:             }
01199:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
01200:         }
01201:
01202:         private static void RunRepresentativeQueries(ContentUtilizationInstrumentation instr, int days)
01203:         {
01204:             for (int day = 1; day <= days; day++)
01205:             {
01206:                 // Simulate daily queries that happen in a real campaign
01207:                 instr.RecordDefinitionQueried("weather_seasons.json", "weather_daily", "WeatherSystem.GetWeather", "WeatherSystem", day);
01208:                 instr.RecordDefinitionQueried("events.json", "event_tick", "EventsHostSession.CheckEvents", "EventsHostSession", day);
01209:                 instr.RecordDefinitionQueried("economy_goods.json", "price_tick", "GoodsCatalog.GetPrice", "MarketSystem", day);
01210:                 instr.RecordDefinitionQueried("narrative_encounters.json", "encounter_tick", "NarrativeEncounterSystem.SelectEncounter", "NarrativeEncounterSystem", day);
01211:                 instr.RecordDefinitionQueried("questline_master.json", "quest_tick", "QuestlineSystem.GetEligible", "QuestlineSystem", day);
01212:                 instr.RecordDefinitionQueried("items.json", "item_tick", "InventorySystem.Update", "InventorySystem", day);
01213:                 instr.RecordDefinitionQueried("survivors.json", "needs_tick", "NeedsSystem.Tick", "NeedsSystem", day);
01214:                 instr.RecordDefinitionQueried("locations.json", "location_tick", "WastelandMapSystem.Update", "WastelandMapSystem", day);
01215:             }
01216:
01217:             // Mark representative consumed content
01218:             instr.RecordDefinitionSelected("items.json", "item_water_filter", "InventorySystem", 1);
01219:             instr.RecordDefinitionConsumed("items.json", "item_water_filter", "InventorySystem", "water consumed", 1);
01220:             instr.RecordDefinitionSelected("items.json", "item_iodine_pills", "InventorySystem", 2);
01221:             instr.RecordDefinitionConsumed("items.json", "item_iodine_pills", "InventorySystem", "radiation treated", 2);
01222:
01223:             instr.RecordDefinitionSelected("locations.json", "loc_home", "WastelandMapSystem", 1);
01224:             instr.RecordDefinitionConsumed("locations.json", "loc_home", "WastelandMapSystem", "home node active", 1);
01225:
01226:             instr.RecordDefinitionSelected("survivors.json", "survivor_starting", "SurvivorsHostSession", 1);
01227:             instr.RecordDefinitionConsumed("survivors.json", "survivor_starting", "SurvivorsHostSession", "survivor active", 1);
01228:         }
01229:     }
01230: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs`

### `Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs` — complete current file

- Size: 267 lines / 11187 bytes.
- SHA-256: `e141922337d8b3312f400c3fb3548ff33490fb5a6a7cf93ca007cb5c1abe1623`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System.IO;
00003: using Ashfall.Core;
00004: using Ashfall.Core.Narrative;
00005: using Xunit;
00006:
00007: namespace Ashfall.Core.Tests
00008: {
00009:     public class NarrativeEncounterSystemTests
00010:     {
00011:         private static NarrativeEncounterSystem NewSystem()
00012:         {
00013:             var sys = new NarrativeEncounterSystem();
00014:             sys.RegisterEncounter(new EncounterDefinition
00015:             {
00016:                 id = "enc_a",
00017:                 title = "A",
00018:                 category = "Discovery",
00019:                 baseWeight = 1f,
00020:                 minDangerLevel = 0f,
00021:                 choices = new System.Collections.Generic.List<EncounterChoiceDefinition>
00022:                 {
00023:                     new EncounterChoiceDefinition { choiceId = "c", text = "Go", moraleDelta = 1, guiltDelta = 0 }
00024:                 }
00025:             });
00026:             sys.RegisterEncounter(new EncounterDefinition
00027:             {
00028:                 id = "enc_b",
00029:                 title = "B",
00030:                 category = "Hazard",
00031:                 baseWeight = 3f,
00032:                 minDangerLevel = 2f,
00033:                 choices = new System.Collections.Generic.List<EncounterChoiceDefinition>
00034:                 {
00035:                     new EncounterChoiceDefinition { choiceId = "c", text = "Go", moraleDelta = 2, guiltDelta = 1 }
00036:                 }
00037:             });
00038:             sys.RegisterEncounter(new EncounterDefinition
00039:             {
00040:                 id = "enc_c",
00041:                 title = "C",
00042:                 category = "Social",
00043:                 baseWeight = 1f,
00044:                 minDangerLevel = 0f,
00045:                 requiredLocationId = "loc_specific",
00046:                 choices = new System.Collections.Generic.List<EncounterChoiceDefinition>
00047:                 {
00048:                     new EncounterChoiceDefinition { choiceId = "c", text = "Go", moraleDelta = 3, guiltDelta = 0 }
00049:                 }
00050:             });
00051:             return sys;
00052:         }
00053:
00054:         [Fact]
00055:         public void Register_NullAndDuplicateIgnored()
00056:         {
00057:             var sys = new NarrativeEncounterSystem();
00058:             sys.RegisterEncounter(null);
00059:             sys.RegisterEncounter(new EncounterDefinition());
00060:             Assert.Empty(sys.Catalog);
00061:             sys.RegisterEncounter(new EncounterDefinition { id = "enc_a" });
00062:             sys.RegisterEncounter(new EncounterDefinition { id = "enc_a" });
00063:             Assert.Single(sys.Catalog);
00064:         }
00065:
00066:         [Fact]
00067:         public void Select_WeightsPreferHigherBaseWeight()
00068:         {
00069:             var sys = NewSystem();
00070:             int a = 0, b = 0;
00071:             var rng = new SeededRng(123);
00072:             for (int i = 0; i < 200; i++)
00073:             {
00074:                 var picked = sys.SelectEncounter("Stealth", 3f, null, rng);
00075:                 if (picked != null && picked.id == "enc_a") a++;
00076:                 if (picked != null && picked.id == "enc_b") b++;
00077:             }
00078:             Assert.True(b > a, $"enc_b (weight 3) should beat enc_a (weight 1): b={b} a={a}");
00079:         }
00080:
00081:         [Fact]
00082:         public void Select_FiltersByDangerAndLocation()
00083:         {
00084:             var sys = NewSystem();
00085:             // danger 1 excludes enc_b (min 2); location filter excludes enc_c everywhere but loc_specific.
00086:             for (int i = 0; i < 50; i++)
00087:             {
00088:                 var picked = sys.SelectEncounter("Stealth", 1f, "loc_elsewhere", new SeededRng(i));
00089:                 if (picked != null)
00090:                 {
00091:                     Assert.NotEqual("enc_b", picked.id);
00092:                     Assert.NotEqual("enc_c", picked.id);
00093:                 }
00094:             }
00095:             // At loc_specific, enc_c is eligible.
00096:             bool sawC = false;
00097:             for (int i = 0; i < 100 && !sawC; i++)
00098:             {
00099:                 var picked = sys.SelectEncounter("Stealth", 1f, "loc_specific", new SeededRng(i));
00100:                 if (picked != null && picked.id == "enc_c") sawC = true;
00101:             }
00102:             Assert.True(sawC);
00103:         }
00104:
00105:         [Fact]
00106:         public void StanceMultipliers_MatchUnityValues()
00107:         {
00108:             var sys = NewSystem();
00109:             var b = sys.Find("enc_b");
00110:             // Unity: stealth x0.5 -> 1.5, speed x1.5 -> 4.5, base 3.0.
00111:             Assert.Equal(1.5f, b.GetEffectiveWeight("Stealth", 3f, null));
00112:             Assert.Equal(4.5f, b.GetEffectiveWeight("Speed", 3f, null));
00113:             Assert.Equal(3.0f, b.GetEffectiveWeight("Stealth", 3f, null) / 0.5f);
00114:         }
00115:
00116:         [Fact]
00117:         public void Select_ReturnsNullWhenNothingEligible()
00118:         {
00119:             var sys = NewSystem();
00120:             // danger below minDangerLevel for everything except... enc_b min 2, a/c min 0.
00121:             // Force nothing eligible: location lockout only affects enc_c; use danger 0 + null location.
00122:             var picked = sys.SelectEncounter("Stealth", 0f, null, new SeededRng(1));
00123:             Assert.NotNull(picked);
00124:             // Danger way below the floor of enc_b is still fine for a/c. Build a system with a floor:
00125:             var strict = new NarrativeEncounterSystem();
00126:             strict.RegisterEncounter(new EncounterDefinition { id = "enc_x", baseWeight = 1f, minDangerLevel = 5f });
00127:             Assert.Null(strict.SelectEncounter("Stealth", 1f, null, new SeededRng(1)));
00128:         }
00129:
00130:         [Fact]
00131:         public void Resolve_RecordsHistoryAndTotals()
00132:         {
00133:             var sys = NewSystem();
00134:             int resolved = 0;
00135:             sys.OnEncounterResolved += r => resolved++;
00136:             Assert.True(sys.Resolve("enc_a", "c", "loc_x", 40));
00137:             Assert.Equal(1, resolved);
00138:             Assert.Equal(1, sys.TotalResolved);
00139:             Assert.Equal("c", sys.State.history[0].choiceId);
00140:             Assert.Equal(40, sys.State.history[0].day);
00141:         }
00142:
00143:         [Fact]
00144:         public void Resolve_UnknownEncounterOrChoiceRejected()
00145:         {
00146:             var sys = NewSystem();
00147:             Assert.False(sys.Resolve("enc_missing", "c", null, 40));
00148:             Assert.False(sys.Resolve("enc_a", "missing_choice", null, 40));
00149:             Assert.Equal(0, sys.TotalResolved);
00150:         }
00151:
00152:         [Fact]
00153:         public void CaptureState_ReturnsSnapshotNotLiveState()
00154:         {
00155:             var sys = NewSystem();
00156:             sys.Resolve("enc_a", "c", "loc_x", 40);
00157:             var snapshot = sys.CaptureState();
00158:             snapshot.history[0].moraleDelta = 99;
00159:             snapshot.cumulativeMorale = 999;
00160:             Assert.Equal(1, sys.State.cumulativeMorale);
00161:             Assert.Equal(1, sys.State.history[0].moraleDelta);
00162:         }
00163:
00164:         [Fact]
00165:         public void CaptureState_EmitsInOrdinalOrder()
00166:         {
00167:             var sys = NewSystem();
00168:             sys.Resolve("enc_b", "c", "loc", 45);
00169:             sys.Resolve("enc_a", "c", "loc", 40);
00170:             var snapshot = sys.CaptureState();
00171:             Assert.Equal(40, snapshot.history[0].day);
00172:             Assert.Equal("enc_a", snapshot.history[0].encounterId);
00173:             Assert.Equal("enc_b", snapshot.history[1].encounterId);
00174:         }
00175:
00176:         [Fact]
00177:         public void SaveLoad_RoundTripsAllState()
00178:         {
00179:             var sys = NewSystem();
00180:             sys.Resolve("enc_a", "c", "loc_x", 40);
00181:             sys.Resolve("enc_b", "c", "loc_y", 41);
00182:             var restored = new NarrativeEncounterSystem();
00183:             restored.RestoreState(sys.CaptureState());
00184:             Assert.Equal(2, restored.TotalResolved);
00185:             Assert.Equal(2, restored.State.history.Count);
00186:             Assert.Equal(sys.State.cumulativeMorale, restored.State.cumulativeMorale);
00187:         }
00188:
00189:         [Fact]
00190:         public void SaveLoad_ChecksumStable()
00191:         {
00192:             var sys = NewSystem();
00193:             sys.Resolve("enc_a", "c", "loc", 40);
00194:             sys.Resolve("enc_c", "c", "loc_specific", 41);
00195:             string before = SaveChecksum.Compute(sys.CaptureState());
00196:             var restored = new NarrativeEncounterSystem();
00197:             restored.RestoreState(sys.CaptureState());
00198:             string after = SaveChecksum.Compute(restored.CaptureState());
00199:             Assert.Equal(before, after);
00200:         }
00201:
00202:         // ── Data catalog ───────────────────────────────────────────────
00203:
00204:         private static string FindDataDir()
00205:         {
00206:             string search = Directory.GetCurrentDirectory();
00207:             for (int i = 0; i < 6; i++)
00208:             {
00209:                 string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
00210:                 if (Directory.Exists(candidate)) return candidate;
00211:                 string parent = Directory.GetParent(search)?.FullName;
00212:                 if (parent == null) break;
00213:                 search = parent;
00214:             }
00215:             return string.Empty;
00216:         }
00217:
00218:         [Fact]
00219:         public void Catalog_LoadsTheThreeUnityEncounters()
00220:         {
00221:             string dataDir = FindDataDir();
00222:             if (string.IsNullOrEmpty(dataDir)) return;
00223:
00224:             var defs = NarrativeEncounterCatalogLoader.Load(
00225:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00226:             // 16 narrative_encounters.json set-pieces (13 + 3 Plan IV trap interference)
00227:             // + 27 narrative_encounters_expansion.json (29 authored, 2 duplicate base ids
00228:             // deduped primary-wins) + 28 micro_locations.json + 31 npc arcs.
00229:             Assert.Equal(102, defs.Count);
00230:             Assert.Equal(27, defs.FindAll(d => d.sourceFile == NarrativeEncounterCatalogLoader.ExpansionFileName).Count);
00231:             Assert.DoesNotContain(defs, d => d.id == "enc_weather_station" && d.sourceFile == NarrativeEncounterCatalogLoader.ExpansionFileName);
00232:             Assert.Contains(defs, d => d.id == "enc_overturned_postal");
00233:             Assert.Contains(defs, d => d.id == "enc_dead_letter_office");
00234:             Assert.Contains(defs, d => d.id == "enc_weather_station");
00235:             Assert.Contains(defs, d => d.id == "enc_pianist");
00236:             for (int i = 0; i < defs.Count; i++)
00237:             {
00238:                 var d = defs[i];
00239:                 Assert.False(string.IsNullOrEmpty(d.title));
00240:                 Assert.False(string.IsNullOrEmpty(d.description));
00241:                 Assert.True(d.choices.Count >= 2);
00242:                 for (int j = 0; j < d.choices.Count; j++)
00243:                 {
00244:                     var c = d.choices[j];
00245:                     Assert.False(string.IsNullOrEmpty(c.choiceId));
00246:                 }
00247:             }
00248:         }
00249:
00250:         [Fact]
00251:         public void Catalog_UnityWeightParity()
00252:         {
00253:             string dataDir = FindDataDir();
00254:             if (string.IsNullOrEmpty(dataDir)) return;
00255:
00256:             var defs = NarrativeEncounterCatalogLoader.Load(
00257:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00258:             var deadLetter = defs.Find(d => d.id == "enc_dead_letter_office");
00259:             Assert.NotNull(deadLetter);
00260:             // Unity: base 2.0, stealth x0.5 = 1.0, speed x1.5 = 3.0.
00261:             Assert.Equal(1.0f, deadLetter.GetEffectiveWeight("Stealth", 1f, null));
00262:             Assert.Equal(3.0f, deadLetter.GetEffectiveWeight("Speed", 1f, null));
00263:             // minDangerLevel 0 keeps it eligible at any danger.
00264:             Assert.True(deadLetter.GetEffectiveWeight("Speed", 5f, null) > 0f);
00265:         }
00266:     }
00267: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **Current evidence and safe integration boundary for Plan 58: Narrative Encounters, Choice Resolution and Expedition Reachability.**.

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
