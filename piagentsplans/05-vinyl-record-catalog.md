# Plan 05 — Vinyl Record Catalog and Cultural Broadcast Reachability

> **Rebuild status:** COMPLETE 30-RECORD CONTENT LOOP — REACHABILITY AND MAINTENANCE PLAN
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

- The original plan identified a real content seam but its baseline is obsolete: `vinyl_record_archive.json` currently contains 30 unique records and current tests assert 30.
- The live route is record catalog → acquisition route or item acquisition → `VinylMoraleSystem` → daily morale effect → optional rare cultural broadcast → radio/UI projection → save restore.
- A useful rebase preserves the cultural texture while refusing invented buff schemas, real-world copying, and a parallel music or morale authority. Future work should prove every authored record is obtainable, every playback effect is once-per-day, and every broadcast is truthfully surfaced.

**Bounded outcome:** Retire the old one-record-to-twenty proposal. The authoritative archive now contains 30 records, the acquisition map, morale playback, cultural broadcast bridge, host session, save store and panel are present. The remaining work is a bounded maintenance/reachability audit, not another record batch or a second morale owner.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `Assets/StreamingAssets/Data/narrative/vinyl_record_archive.json` is present with schema version 1 and 30 unique `record_id` values.
- `VinylRecordCatalog.cs`, `VinylRecordAcquisitionMap.cs` and `VinylMoraleSystem.cs` are current Core owners; `VinylMoraleHostSession.cs`, `VinylMoraleSaveStore.cs` and `VinylMoralePanel.cs` are current host/UI surfaces.
- Focused tests cover all 30 records, acquisition routes, duplicate-safe collectible registration, once-daily morale, cultural broadcast triggering and save restore.
- The record catalog is content authority; item acquisition and the radio bridge are separate existing seams and must not be merged into a new record manager.

**Master-authority sections applied to this rebase:**

- Master authority Volume 28 verification cookbook: focused evidence before broad gates.
- Lane D save/state/compatibility guidance: owner DTOs, migration and restore proof.
- Lane E UI/UX/accessibility guidance: truthful projections and keyboard/controller lifecycle.
- Lane G testing guidance: smallest affected target, negative cases and deterministic replay.
- Anti-padding protocol: content exhaustion may end the plan before the character checkpoint.
- Volume 32 cultural archive restoration log.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 1→20 content brief with a 30-row current census and a provenance/tone review checklist.
- Add a reachability matrix from every canonical record to at least one acquisition route, collectible, item or host-visible source.
- Preserve once-daily playback and exactly-once rare broadcast semantics in any future catalog row.
- Treat real-person/real-work provenance and copied text as a content audit issue, not as a reason to invent new gameplay fields.

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
| 30-record metadata and lookup | VinylRecordCatalog | `Assets/Ashfall.Core/Narrative/VinylRecordCatalog.cs` | Canonical record definitions and IDs. |
| authored acquisition routes | VinylRecordAcquisitionMap | `Assets/Ashfall.Core/Narrative/VinylRecordAcquisitionMap.cs` | Maps records to current acquisition surfaces without owning inventory. |
| playback and daily morale effect | VinylMoraleSystem | `Assets/Ashfall.Core/VinylMoraleSystem.cs` | Owns current/last playback and once-daily effect state. |
| host commands and event projection | VinylMoraleHostSession | `src/Host/VinylMoraleHostSession.cs; src/Host/VinylMoraleSaveStore.cs` | Composes commands, save bytes and cultural broadcast notification. |
| rare record broadcast fact | Radio culture bridge | `src/Host/RadioHostSession.cs; src/UI/RadioIntelligencePanel.cs` | Consumes the existing cultural broadcast event; it does not own record effects. |
| catalog, acquisition, replay and persistence proof | Focused vinyl tests | `Ashfall.Core.Tests/VinylRecordCatalogTests.cs; Ashfall.Core.Tests/VinylMoraleSystemTests.cs; Ashfall.Core.Tests/VinylAcquisitionIntegrationTests.cs; Ashfall.Core.Tests/VinylRadioBridgeTests.cs` | Executable evidence for the current loop. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Vinyl Record Catalog and Cultural Broadcast Reachability
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ VinylRecordCatalog
│   30-record metadata and lookup
│ VinylRecordAcquisitionMap
│   authored acquisition routes
│ VinylMoraleSystem
│   playback and daily morale effect
│ VinylMoraleHostSession
│   host commands and event projection
│ Radio culture bridge
│   rare record broadcast fact
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

1. **Preserve current state ownership.** VinylRecordCatalog owns 30-record metadata and lookup: Canonical record definitions and IDs.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| 30-record metadata and lookup | VinylRecordCatalog | `Assets/Ashfall.Core/Narrative/VinylRecordCatalog.cs` | Canonical record definitions and IDs. |
| authored acquisition routes | VinylRecordAcquisitionMap | `Assets/Ashfall.Core/Narrative/VinylRecordAcquisitionMap.cs` | Maps records to current acquisition surfaces without owning inventory. |
| playback and daily morale effect | VinylMoraleSystem | `Assets/Ashfall.Core/VinylMoraleSystem.cs` | Owns current/last playback and once-daily effect state. |
| host commands and event projection | VinylMoraleHostSession | `src/Host/VinylMoraleHostSession.cs; src/Host/VinylMoraleSaveStore.cs` | Composes commands, save bytes and cultural broadcast notification. |
| rare record broadcast fact | Radio culture bridge | `src/Host/RadioHostSession.cs; src/UI/RadioIntelligencePanel.cs` | Consumes the existing cultural broadcast event; it does not own record effects. |
| catalog, acquisition, replay and persistence proof | Focused vinyl tests | `Ashfall.Core.Tests/VinylRecordCatalogTests.cs; Ashfall.Core.Tests/VinylMoraleSystemTests.cs; Ashfall.Core.Tests/VinylAcquisitionIntegrationTests.cs; Ashfall.Core.Tests/VinylRadioBridgeTests.cs` | Executable evidence for the current loop. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. Load the immutable vinyl archive
2. resolve an acquisition route or canonical item/collectible
3. commit the record to the existing acquisition/collectible owner
4. play through `VinylMoraleSystem`
5. apply the once-daily morale effect
6. emit rare cultural broadcast fact when classified
7. project through radio/UI and capture the existing vinyl save state

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Playback identity, last-played day and total-play count are owner state; record metadata is immutable catalog data.
- A record may be acquired repeatedly without duplicate registration; the collectible ledger owns exactly-once claims.
- Daily morale applies at most once for the same owner day, and rare broadcast emission is separately exactly-once.
- Restore must preserve owned/discovered records, playback state and broadcast counters without re-emitting a past event.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Canonical record IDs are unique and every runtime lookup uses the catalog ID, not a display title.
- An unknown record fails closed with a visible host diagnostic and does not mutate playback state.
- A repeated acquisition is idempotent at the collectible/record owner and cannot double-apply morale.
- The same seed/day and the same captured state produce the same playback and broadcast projection.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- Keep `vinyl_record_archive.json` as the sole authored record catalog.
- Do not duplicate rows into `items.json`; use the current acquisition map and collectible references.
- Any new record requires a current acquisition route, a consumer test and a tone/provenance review.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the current vinyl save section/store; do not add a second record ownership store.
- Capture must preserve current/last record, day counters, owned/discovered acquisition state and broadcast counters.
- Legacy empty state must restore as an empty collection and remain playable.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Record selection and daily effect must use the injected seeded RNG and stable ordinal ordering.
- Replay tests must compare playback state, morale delta and broadcast count, not just catalog count.
- No wall-clock or hash-iteration ordering may decide a record selection.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- `OnPlaybackChanged` reports current transport state.
- `OnDailyEffectApplied` reports the owner-day morale effect.
- `OnCulturalBroadcast` is a fact consumed by the radio bridge; the vinyl system does not own radio state.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/VinylMoraleHostSession.cs
- src/Host/VinylMoraleSaveStore.cs
- src/UI/VinylMoralePanel.cs
- src/Host/RadioHostSession.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Album titles and descriptions remain fictional and restrained.
- The cultural archive is a record of lived world texture, not a real-world music catalog.
- The rare broadcast should feel like a shelter ritual and must not imply hidden outcome data.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | Catalog and acquisition map disagree on a record ID. | VinylRecordCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A record is obtainable only through a stale test fixture or dead route. | VinylRecordAcquisitionMap | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Daily morale applies twice after a save/restore boundary. | VinylMoraleSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Rare broadcast fires from a panel click rather than the owner event. | VinylMoraleHostSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A record row is added without a current consumer. | Radio culture bridge | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/VinylRecordCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/VinylMoraleSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/VinylAcquisitionIntegrationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/VinylRadioBridgeTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — census and premise check | Read current archive, loader, acquisition map and save owner. | 30 unique records and no duplicate authority. | No production path until the owning implementation package is separately claimed. |
| 1 — reachability matrix | Trace every record to a current acquisition/collectible/host surface. | No orphan record or dead route remains. | No production path until the owning implementation package is separately claimed. |
| 2 — replay and UI proof | Verify once-daily morale, rare broadcast and restore behavior. | Focused tests cover the full player-visible loop. | No production path until the owning implementation package is separately claimed. |
| 3 — content seal | Review tone, provenance and future row admission. | No copied or unexplained row enters the catalog. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/narrative/vinyl_record_archive.json | READ ONLY; MODIFY only for proven content gap | 30-row authority |
| Assets/Ashfall.Core/Narrative/VinylRecordCatalog.cs | READ ONLY | Current loader |
| Assets/Ashfall.Core/Narrative/VinylRecordAcquisitionMap.cs | READ ONLY | Current route authority |
| src/UI/VinylMoralePanel.cs | READ ONLY | Current presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Duplicating records into a second catalog. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Treating a display title as a stable ID. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Firing rare broadcasts from UI callbacks. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding real-world copyrighted/prohibited references. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new music system.
- No 250k character requirement.
- No additional record count without a content need.
- No production edits in this rebase.

# 23. Rollback and Recovery

- Revert the isolated planning document.
- Future data changes require the prior valid JSON fixture and a catalog diff.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- The 30-row current authority is documented.
- Acquisition, morale, broadcast and save contracts are explicit.
- A focused verification path exists for every future content change.
- No parallel owner or new save section is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 1→20 content brief with a 30-row current census and a provenance/tone review checklist.
- Add a reachability matrix from every canonical record to at least one acquisition route, collectible, item or host-visible source.
- Preserve once-daily playback and exactly-once rare broadcast semantics in any future catalog row.
- Treat real-person/real-work provenance and copied text as a content audit issue, not as a reason to invent new gameplay fields.

## MUST NOT DO

- No new music system.
- No 250k character requirement.
- No additional record count without a content need.
- No production edits in this rebase.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/VinylRecordCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/VinylMoraleSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/VinylAcquisitionIntegrationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/VinylRadioBridgeTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — census and premise check — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: 30-record metadata and lookup → VinylRecordCatalog; authored acquisition routes → VinylRecordAcquisitionMap; playback and daily morale effect → VinylMoraleSystem; host commands and event projection → VinylMoraleHostSession; rare record broadcast fact → Radio culture bridge; catalog, acquisition, replay and persistence proof → Focused vinyl tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 05.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 05 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by VinylRecordCatalog or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Narrative/VinylRecordCatalog.cs`

### `Assets/Ashfall.Core/Narrative/VinylRecordCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 103 lines / 3422 bytes.
- SHA-256: `89a5dbfaa5c03c4833af3fce0d3ce96a3a4b05b5f54f2d36486d6a32cc13f6f5`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class VinylRecordEntry
public string record_id;
public string catalog_number;
public string title;
public string performer;
public int recording_year;
public string format_rpm;
public string physical_condition;
public int daily_morale_modifier;
public float broadcast_frequency_mhz;
public string needle_audio_texture;
public string dweller_resonance_notes;
public string[] tags;
public sealed class VinylRecordsFile
public int schema_version;
public string collection_id;
public List<VinylRecordEntry> records = new List<VinylRecordEntry>();
public sealed class VinylRecordCatalog
public IReadOnlyList<VinylRecordEntry> AllRecords => _allRecords;
public void Load(string json, IJsonSerializer serializer) {
public VinylRecordEntry? GetById(string recordId) {
public List<VinylRecordEntry> GetByFormat(string formatSnippet) {
public List<VinylRecordEntry> GetByTag(string tag) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Narrative/VinylRecordAcquisitionMap.cs`

### `Assets/Ashfall.Core/Narrative/VinylRecordAcquisitionMap.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 197 lines / 11105 bytes.
- SHA-256: `09df98875084fd922f61f5bbf0ea356e9b90a98f0d1ae0eaf53385859c826748`.
- Architecture signals: seeded references=2; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class VinylAcquisitionRoute
public string RecordId { get; set; } = string.Empty;
public string AssociatedItemId { get; set; } = string.Empty;
public string AcquisitionChannel { get; set; } = string.Empty; // scavenging, expedition, barter, cultural_vault
public string LocationType { get; set; } = string.Empty;
public string RarityTier { get; set; } = "rare"; // common, uncommon, rare, very_rare
public int DiscoveryWeight { get; set; } = 10;
public static class VinylRecordAcquisitionMap
public const string DefaultVinylCrateItemId = "item_vinyl_collection";
public const string ChamberRecordCollectibleItemId = "item_collectible_vinyl_chamber_record";
public const string CivilBroadcastCollectibleItemId = "item_collectible_vinyl_civil_broadcast";
public const string FolkCompilationCollectibleItemId = "item_collectible_vinyl_folk_compilation";
public static IReadOnlyList<VinylAcquisitionRoute> GetAllRoutes() => AllRoutesList;
public static VinylAcquisitionRoute? GetRoute(string recordId) {
public static bool IsVinylAcquisitionItem(string itemId) {
public static string? TryAcquireFromItem( string itemId, VinylMoraleSystem vinylSystem, ISeededRng? rng = null) {
public static string? ResolveRecordForDiscovery( string locationType, IReadOnlyCollection<string> alreadyOwned, ISeededRng rng) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/VinylMoraleSystem.cs`

### `Assets/Ashfall.Core/VinylMoraleSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 210 lines / 8476 bytes.
- SHA-256: `d79d1947400d5e054b6f3ca75127f1f17ddf6ca7e014fe8f93d140645eea95d5`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=11; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class VinylMoraleState
public string systemId = VinylMoraleSystem.SystemId;
public List<string> ownedRecordIds = new List<string>();
public string currentPlayingId = string.Empty;
public string lastPlayedId = string.Empty;
public int lastPlayedDay = -1;
public int totalPlays;
public float totalMoraleApplied;
public bool isTurntableActive;
public string lastBroadcastRecordId = string.Empty;
public int lastBroadcastDay = -1;
public int broadcastCount;
public float lastBroadcastSignalStrength;
public sealed class VinylRecordDefinition
public string record_id = string.Empty;
public string display_name = string.Empty;
public string genre = string.Empty;
public float morale_daily_bonus = 3f;
public float flashback_suppression; // 0-1, reduces flashback probability
public string audio_cue_id = string.Empty;
public string description = string.Empty;
public sealed class VinylMoraleSystem
public const string SystemId = "vinyl_morale";
public VinylMoraleState State => _state;
public bool IsPlaying => _state.isTurntableActive && !string.IsNullOrEmpty(_state.currentPlayingId);
public event Action<float> OnMoraleApplied;      // morale amount
public event Action<float> OnFlashbackSuppressed; // suppression amount
public event Action OnPlaybackChanged;
public event Action<VinylRecordDefinition, int> OnCulturalBroadcast; // record, day — rare vinyl → radio
public void LoadCatalog(List<VinylRecordDefinition> records) {
public void MergeRecord(VinylRecordDefinition record) {
public void AcquireRecord(string recordId) {
public ActionResult Play(string recordId) {
public ActionResult Play(string recordId, int day) {
public bool IsRareCulturalRecord(VinylRecordDefinition record) {
public ActionResult Stop() {
public void CancelBroadcastBrownout() {
public void ApplyDailyEffect(int day) {
public VinylRecordDefinition? GetRecord(string id) {
public VinylMoraleState CaptureState() => CloneState(_state);
public void RestoreState(VinylMoraleState saved) {
```


# Appendix B.05 — Current Code Architecture: `src/Host/VinylMoraleHostSession.cs`

### `src/Host/VinylMoraleHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 78 lines / 2366 bytes.
- SHA-256: `0038ca6bd56a516ac27f5e70c82dba3f51f86b4003ba94666a81132bd89b5259`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class VinylMoraleHostSession
public VinylMoraleSystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public Func<int> DayProvider { get; set; } = () => -1;
public void AcquireRecord(string recordId) {
public ActionResult PlayRecord(string recordId, int day = -1) {
public ActionResult StopPlayback() {
public void TickDay(int day) {
public override void Save() {
```


# Appendix B.06 — Current Code Architecture: `src/Host/VinylMoraleSaveStore.cs`

### `src/Host/VinylMoraleSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 56 lines / 2689 bytes.
- SHA-256: `7830b5c3c6e299a70e69f3f185f135ccd4bd736e84f5b27c127ad45f642860ee`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class VinylMoraleSaveStore
public const string FileName = "vinyl_morale_save.json";
public const string SectionName = "vinyl_morale";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(VinylMoraleState state) => s_store.CaptureBare(state);
public static VinylMoraleState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(VinylMoraleState state) => s_store.CaptureBare(state);
public static VinylMoraleState? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(VinylMoraleState state) => s_store.TrySave(state);
public static VinylMoraleState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(VinylMoraleState state) => s_store.CapturePersisted(state);
```


# Appendix B.07 — Current Code Architecture: `src/UI/VinylMoralePanel.cs`

### `src/UI/VinylMoralePanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 206 lines / 8940 bytes.
- SHA-256: `ed28d8aae4df2aa44d11f4592a54d5d2f331adc35ac0772d40e9131104d610d7`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class VinylMoralePanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _host != null;
public void Bind(VinylMoraleHostSession session) {
public void Unbind() {
public override void _Ready() {
public void RefreshView() {
public override void _ExitTree() {
```


# Appendix B.08 — Current Code Architecture: `src/Host/RadioHostSession.cs`

### `src/Host/RadioHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 785 lines / 37883 bytes.
- SHA-256: `fa3dcecb7e92d69fd292ca9bd06e75d24e57f730ade13ba7b99e7c21fd6b93d8`.
- Architecture signals: seeded references=4; save/restore symbols=16; typed event declarations=1; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RadioHostSession
public const int DemoSeed = 2026;
public event Action<RadioIntercept, string?>? BroadcastIntercepted;
public FactionRadioEngine Engine { get; }
public SignalTriangulationSystem Triangulation { get; }
public RadioBroadcastCatalog BroadcastCatalog { get; }
public RadioStationCatalog Stations { get; }
public RadioScheduleCoordinator ScheduleCoordinator { get; }
public RadioDistressSystem DistressSystem { get; }
public RadioRecordingSystem RecordingSystem { get; }
public RadioSignalLog SignalLog { get; }
public DistressRescueMissionManager RescueMissions { get; }
public SignalTrustLedger SignalTrust { get; }
public DistressFollowUpScheduler FollowUps { get; }
public ISeededRng Rng { get; }
public IReadOnlyList<RadioIntercept> History => _history;
public int Day { get; private set; }
public float CurrentFrequency { get; private set; }
public RadioIntercept? LastIntercept { get; private set; }
public ScheduledBroadcastResult? LastScheduledBroadcast { get; private set; }
public string LastEvent { get; private set; } = string.Empty;
public Func<string>? WeatherConditionProvider { get; set; }
public Func<WeatherKind>? WeatherKindProvider { get; set; }
public RadioReceiverBand CurrentBand => RadioReceiverPlan.GetBandForFrequencyMhz(CurrentFrequency);
public void SetBand(string bandId) {
public void CycleBand() {
public static RadioHostSession Create(string dataDir, int day = 1, ICampaignRngManager? campaignRng = null) {
public void SetDay(int day) {
public string Listen(float? frequencyMhz = null) {
public string BroadcastBeacon(string customMessage = "Holdfast shelter holding. Awaiting courier contact.") {
public void TuneDelta(float deltaMhz) {
public bool RecordBearingObservation(float bearingDegrees) {
public TriangulationCandidate? TriangulateCurrentSignal() {
public string RecordMarketRumor(string message, int day) {
public string InterceptWarlordWarning(string message, int day) {
public string RecordCulturalBroadcast(string recordId, string genre, string displayName, int day, float signalStrength) {
public bool HasPlayed(RadioIntercept intercept) {
public RadioSaveState CaptureSave() {
public void RestoreSave(RadioSaveState state) {
public string ActiveStationId { get; set; } = "station_alpha";
public void SetActiveStation(string stationId) {
public string RecordObservation(string signalId, float bearing, float signalStrength = 0.7f, float noise = 0.2f, string? stationId = null) {
public string RecordObservationDemo(string signalId, float bearing, float signalStrength = 0.7f, float noise = 0.2f) => RecordObservation(signalId, bearing, signalStrength, noise);
public string TriangulateSignal(string signalId) {
public string TriangulateDemo(string signalId) => TriangulateSignal(signalId);
public string TriangulationStatusLine(string signalId) {
public string StatusLine() {
public RadioProgramSlot? GetCurrentSlot(string stationId, int? hour = null) {
public RadioProgramSlot? GetNextSlot(string stationId, int? hour = null) {
public RadioSignalStrength GetSignalStrength(string stationId, RadioReceptionFactors? factors = null) {
public RadioStationDefinition? GetStationAtCurrentFrequency() {
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/narrative/vinyl_record_archive.json`

### `Assets/StreamingAssets/Data/narrative/vinyl_record_archive.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 25564 bytes / 25562 characters.
- SHA-256: `0c1efb951d1a3f415b4480445c9a0112ae44b5eae3dad3745b07f77163aeb84b`.
- Root keys: `collection_id`, `records`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
records: min=30, max=30, observed_paths=1
records[].tags: min=6, max=6, observed_paths=2
```

Representative record fields:

- `broadcast_frequency_mhz`
- `catalog_number`
- `daily_morale_modifier`
- `dweller_resonance_notes`
- `format_rpm`
- `needle_audio_texture`
- `performer`
- `physical_condition`
- `record_id`
- `recording_year`
- `tags`
- `title`


# Appendix D.10 — Existing Focused Test Inventory: `Ashfall.Core.Tests/VinylRecordCatalogTests.cs`

### `Ashfall.Core.Tests/VinylRecordCatalogTests.cs`

- Current test declarations: Fact=2, Theory=0, InlineData=0.
- File lines: 93; SHA-256: `a98b118abaef7b04f61c293262d6dbde210307af634d7ee39d95bfcdb1699569`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
VinylRecords_LoadsAll30CanonicalRecordings
VinylRecords_AllEntriesHaveValidFieldsAndUniqueCatalogNumbers
```


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/VinylMoraleSystemTests.cs`

### `Ashfall.Core.Tests/VinylMoraleSystemTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 154; SHA-256: `d92848e620d6d1551ee202f7b162576a7c63050a2ddff0444b334ca94bb66891`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
VinylArchive_Loads30Records_WithDistinctMoraleEffects
```


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/VinylAcquisitionIntegrationTests.cs`

### `Ashfall.Core.Tests/VinylAcquisitionIntegrationTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 189; SHA-256: `cac2ef6d5928740c662177279879284eedd027947f3e0983b1047d3db256e58e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
All30AuthoredRecords_HaveAcquisitionRoute
IsVinylAcquisitionItem_IdentifiesVinylItemsCorrectly
TryAcquireFromItem_AddsUnownedRecordToVinylSystem
ResolveRecordForDiscovery_ResolvesByLocation
PlaybackAndDailyMorale_WorksWithAcquiredRecord
RareCulturalRecord_TriggersBroadcastEvent
State_RoundTripsThroughCaptureAndRestore
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/VinylRadioBridgeTests.cs`

### `Ashfall.Core.Tests/VinylRadioBridgeTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 122; SHA-256: `da4ff844aff00335ff1beb921feb1e03ed8dc048928af4d17e6ccbeaf1d8fd1b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
PlayRareVinyl_TriggersCulturalBroadcast
PlayCommonVinyl_DoesNotTriggerBroadcast
PlayJazzRare_TriggersBroadcastViaGenre
Stop_ClearsBroadcastSignal
SaveRoundTrip_PreservesBroadcastState
IsRareCulturalRecord_ChecksBonusAndGenre
SecondRarePlay_IncrementsBroadcastCount
```


# Appendix E.14 — Supporting Code Evidence: `src/Host/AssetRegistry.cs`

### `src/Host/AssetRegistry.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1254 lines / 57965 bytes.
- SHA-256: `19261be4581f85792e82931048a05b0e3f9604d829ec19418792a90af282f3a2`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum AssetLoadResult
public readonly struct AssetResult
public readonly Texture2D? Texture;
public readonly AssetLoadResult Result;
public readonly string ResolvedPath;
public readonly string RequestedId;
public bool IsValid => Texture != null && (Result == AssetLoadResult.Loaded || Result == AssetLoadResult.FallbackUsed);
public bool IsMissing => Result == AssetLoadResult.Missing || Result == AssetLoadResult.FailedToLoad;
public static class AssetRegistry
public const string FallbackSurvivorPath = "res://assets/sprites/Characters/placeholder_survivor.png";
public const string FallbackSurvivorRelativePath = "assets/sprites/Characters/placeholder_survivor.png";
public const string FallbackIconPath = "res://assets/ui/Icons/icon_placeholder.png";
public const string FallbackIconRelativePath = "assets/ui/Icons/icon_placeholder.png";
public readonly struct MissingAssetWarning
public readonly string Category;
public readonly string RequestedId;
public readonly string FallbackUsed;
public readonly string Message;
public override string ToString() => Message;
public static int DuplicateFallbackRequestCount => _duplicateFallbackRequests;
public static int TotalFallbackRequestCount => _totalFallbackRequests;
public static string GetEffectiveFallbackDescription(string category) {
public static void SetFallbackTexture(Texture2D? texture) {
public static AssetResult GetItem(string itemId) {
public static AssetResult GetPortrait(string survivorId) {
public static AssetResult GetLocation(string locationId) {
public static AssetResult GetFaction(string factionId) {
public static AssetResult GetByPath(string path) {
public static string? ResolveItemPath(string itemId) {
public static string? ResolvePortraitPath(string survivorId) {
public static string? ResolveLocationPath(string locationId) {
public static void ClearMissingLog() {
public static int MissingAssetCount => _loggedMissing.Count;
public static IReadOnlyCollection<MissingAssetWarning> LoggedWarnings => _loggedWarnings.Values;
public static bool HasLoggedWarning(string category, string id) => _loggedWarnings.ContainsKey($"{category}:{id}");
public static MissingAssetWarning? GetLoggedWarning(string category, string id) =>
public static class AssetRegistrySelfTest
public struct ResultRow
public string Id;
public string Category;
public string? ResolvedPath;
public bool Exists;
public bool Loaded;
public int ReferenceCount;
public struct Report
public int TotalChecked;
public int Missing;
public int UniqueMissing;
public int DuplicateFallbackRequests;
public int FailedToLoad;
public int Passed;
public int ProbeFailures;
public List<ResultRow> Rows;
public string Summary;
public bool Clean => Missing == 0 && FailedToLoad == 0 && ProbeFailures == 0;
public static Report Run(string dataDir, int topCount = 50) {
public static void RunFullCoverage(string dataDir) {
```


# Appendix E.15 — Supporting Code Evidence: `src/Host/AssetCoverageScanner.cs`

### `src/Host/AssetCoverageScanner.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 595 lines / 25956 bytes.
- SHA-256: `80504e625a7eb892cd75c2ad5631d46d003493cd35dfb3402ca1cd7b3f2d6188`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class AssetCoverageScanner
public static AssetCoverageSummaryReport RunTopProbes(string dataDir, int topCount = 50) {
public static void RunFullCoverageSweep(string dataDir) {
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/CollectibleDiscoveryState.cs`

### `Assets/Ashfall.Core/CollectibleDiscoveryState.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 301 lines / 12645 bytes.
- SHA-256: `6d22c7bd8bb5ded22bb340a5a4fe0433e8d90ba4b4e20c1a8366207f5583bcf7`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum CollectibleDiscoveryStatus
public sealed class CollectibleDiscoveryLocationEntry
public string item_id = string.Empty;
public string location_id = string.Empty;
public sealed class CollectibleDiscoverySave
public int schema_version = 2;
public string[] discovered_ids = Array.Empty<string>();
public string[] unacknowledged_ids = Array.Empty<string>();
public string[] acknowledged_ids = Array.Empty<string>();
public string[] ever_acquired_ids = Array.Empty<string>();
public CollectibleDiscoveryLocationEntry[] discovery_locations = Array.Empty<CollectibleDiscoveryLocationEntry>();
public sealed class CollectibleDiscoveryState
public int Count => _unacknowledgedIds.Count + _acknowledgedIds.Count;
public int UnacknowledgedCount => _unacknowledgedIds.Count;
public int AcknowledgedCount => _acknowledgedIds.Count;
public int EverAcquiredCount => _everAcquiredIds.Count;
public bool WasEverAcquired(string itemId) {
public bool IsDiscovered(string itemId) {
public bool IsAcknowledged(string itemId) {
public bool IsUnacknowledged(string itemId) {
public CollectibleDiscoveryStatus GetDiscoveryStatus(string itemId) {
public IReadOnlyDictionary<string, string> DiscoveryLocations => _discoveryLocations;
public string? GetDiscoveryLocation(string itemId) {
public bool MarkDiscovered(string itemId, string? discoveryLocationId = null) {
public bool AcknowledgeDiscovery(string itemId) {
public CollectibleDiscoverySave CaptureState() {
public void RestoreState(CollectibleDiscoverySave? save) {
```


# Appendix E.17 — Supporting Code Evidence: `src/Main.ShelterSocial.cs`

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


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs`

### `Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 432 lines / 20841 bytes.
- SHA-256: `fad9423c257b5fcc2c8e38b078acf1669cc01e3cd3156e8b2764f3c79703dc43`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CollectibleIntegrityFinding
public string SourceCatalog { get; }
public string SourceId { get; }
public string FieldPath { get; }
public string TargetId { get; }
public string TargetCatalog { get; }
public string ErrorCode { get; }
public string Message { get; }
public override string ToString() =>
public static class CollectibleCatalogIntegrityValidator
public static readonly HashSet<string> ValidCategories = new HashSet<string>(StringComparer.Ordinal) {
public static readonly HashSet<string> ValidRarities = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
public static readonly HashSet<string> ValidEffectTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
public static List<CollectibleIntegrityFinding> Validate( string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null) {
public sealed class ItemFileRootDto
public int schema_version { get; set; } = 1;
public List<ItemHeaderDto> items { get; set; } = new List<ItemHeaderDto>();
public sealed class ItemHeaderDto
public string id { get; set; } = string.Empty;
public sealed class JournalVoiceProseFileRaw
public int schema_version { get; set; } = 1;
public Dictionary<string, Dictionary<string, string>> prose_variants { get; set; } =
```


# Appendix G.19 — Supporting Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleVinylIntegrationTests.cs`

### `Ashfall.Core.Tests/Collectibles/CollectibleVinylIntegrationTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 245; SHA-256: `4be4cee2288fbac062c1b33144a656c46a01818440479449de4efd08ec07430d`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ChamberRecordCollectible_RegistersRecord_NoDuplicateOnReacquire
CivilBroadcastCollectible_RegistersRecord_NoDuplicateOnReacquire
FolkCompilationCollectible_RegistersRecord_NoDuplicateOnReacquire
VinylPickup_AppliesZeroMorale_PlaybackAppliesExactlyOneDailyEffect
VinylDailyEffect_IsOncePerDay_WhilePlaying
VinylAcquisition_SaveRestore_OwnedAndDiscovered_ReacquireDoesNotReRegister
VinylCatalogRecords_ResolveForUi
```


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs`

### `Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 246; SHA-256: `36f39d80a73fb2196269c85037f963788a8362b33cd24e9664c4b20bc7fad6e5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CollectibleDiscoverySave_FiveIds_SerializesInOrdinalOrder
CollectibleDiscoverySave_RoundTrip_PreservesAllIds
CollectibleDiscoverySave_AddSixth_ReserializesCanonically
CollectibleDiscoverySave_PrePlan47_MissingSectionLoadsEmpty
CollectibleDiscoverySave_PrePlan47_DoesNotBackfillFromInventoryOrOtherSystems
CollectibleDiscoverySave_PrePlan47_CanDiscoverAfterLoad
CollectibleDiscoverySave_FullCampaignRoundTrip_PreservesState
CollectibleDiscoveryState_DroppingInventoryItem_DoesNotClearDiscovery
CollectibleDiscoveryState_VinylRegistration_IsIndependent
CollectibleDiscoverySave_ScrambledInsertion_PreservesOrdinalSerialization
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/CollectibleDiscoveryStateTests.cs`

### `Ashfall.Core.Tests/CollectibleDiscoveryStateTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 196; SHA-256: `db727cde8fcf9c01f1bf3bd9abe0a59402e811a90c7ade136ed274828e061a82`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
EmptyState_IsDiscoveredFalse_CountZero
FirstMark_ReturnsTrue_AndChangesState
RepeatMark_IsIdempotent
MarkEmptyOrNull_IsRejected
CaptureState_SortsOrdinal_DeterministicAcrossInsertionOrders
Restore_ClearsThenLoads_ToleratesDuplicates
Restore_NullOrMissingSection_LoadsSafelyEmpty
Restore_EmitsNoEffects_AndDoesNotMutateInventory
UnrelatedCollectible_RemainsUndiscovered
EnvelopeRoundTrip_PreservesExactSet
Envelope_MutatedState_ChangesChecksum
Envelope_MissingChecksumRejected
State_IsCampaignScoped_NotGlobalStatic
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/CollectibleItemPresentationTests.cs`

### `Ashfall.Core.Tests/CollectibleItemPresentationTests.cs`

- Current test declarations: Fact=15, Theory=0, InlineData=0.
- File lines: 333; SHA-256: `5db7acdb95aa5ec79acda68fb521dfea1cab79566fb676faad7e302ec2e50878`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Test1_CollectibleCard_ShowsCategory
Test2_CollectibleCard_ShowsRarity
Test3_FirstDiscovery_ShowsTextualNew
Test4_AcknowledgedDiscovery_ShowsTextualDiscovered
Test5_NonCollectibleCard_UnchangedOrNull
Test6_EffectBearingItem_ShowsCorrectHint
Test7_NoEffectItem_ShowsNoFabricatedBenefit
Test8_Acknowledgement_ChangesStatusExactlyOnce
Test9_SaveLoad_PreservesNewState
Test10_SaveLoad_PreservesDiscoveredState
Test11_UiReopen_DoesNotAutoAcknowledge
Test12_DuplicateNonUnique_DisplaysDiscovered
Test13_LongLocalizedMetadata_DoesNotBreakLayout
Test14_Status_IsMeaningfulWithoutColor
Test15_ItemCardBindings_RemainRegressionSafe
```


# Appendix H.23 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

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
| 30-record metadata and lookup | VinylRecordCatalog | authored acquisition routes | VinylRecordAcquisitionMap | Owner emits/reads a typed fact; no mirror state. |
| 30-record metadata and lookup | VinylRecordCatalog | playback and daily morale effect | VinylMoraleSystem | Owner emits/reads a typed fact; no mirror state. |
| 30-record metadata and lookup | VinylRecordCatalog | host commands and event projection | VinylMoraleHostSession | Owner emits/reads a typed fact; no mirror state. |
| 30-record metadata and lookup | VinylRecordCatalog | rare record broadcast fact | Radio culture bridge | Owner emits/reads a typed fact; no mirror state. |
| 30-record metadata and lookup | VinylRecordCatalog | catalog, acquisition, replay and persistence proof | Focused vinyl tests | Owner emits/reads a typed fact; no mirror state. |
| authored acquisition routes | VinylRecordAcquisitionMap | 30-record metadata and lookup | VinylRecordCatalog | Owner emits/reads a typed fact; no mirror state. |
| authored acquisition routes | VinylRecordAcquisitionMap | playback and daily morale effect | VinylMoraleSystem | Owner emits/reads a typed fact; no mirror state. |
| authored acquisition routes | VinylRecordAcquisitionMap | host commands and event projection | VinylMoraleHostSession | Owner emits/reads a typed fact; no mirror state. |
| authored acquisition routes | VinylRecordAcquisitionMap | rare record broadcast fact | Radio culture bridge | Owner emits/reads a typed fact; no mirror state. |
| authored acquisition routes | VinylRecordAcquisitionMap | catalog, acquisition, replay and persistence proof | Focused vinyl tests | Owner emits/reads a typed fact; no mirror state. |
| playback and daily morale effect | VinylMoraleSystem | 30-record metadata and lookup | VinylRecordCatalog | Owner emits/reads a typed fact; no mirror state. |
| playback and daily morale effect | VinylMoraleSystem | authored acquisition routes | VinylRecordAcquisitionMap | Owner emits/reads a typed fact; no mirror state. |
| playback and daily morale effect | VinylMoraleSystem | host commands and event projection | VinylMoraleHostSession | Owner emits/reads a typed fact; no mirror state. |
| playback and daily morale effect | VinylMoraleSystem | rare record broadcast fact | Radio culture bridge | Owner emits/reads a typed fact; no mirror state. |
| playback and daily morale effect | VinylMoraleSystem | catalog, acquisition, replay and persistence proof | Focused vinyl tests | Owner emits/reads a typed fact; no mirror state. |
| host commands and event projection | VinylMoraleHostSession | 30-record metadata and lookup | VinylRecordCatalog | Owner emits/reads a typed fact; no mirror state. |
| host commands and event projection | VinylMoraleHostSession | authored acquisition routes | VinylRecordAcquisitionMap | Owner emits/reads a typed fact; no mirror state. |
| host commands and event projection | VinylMoraleHostSession | playback and daily morale effect | VinylMoraleSystem | Owner emits/reads a typed fact; no mirror state. |
| host commands and event projection | VinylMoraleHostSession | rare record broadcast fact | Radio culture bridge | Owner emits/reads a typed fact; no mirror state. |
| host commands and event projection | VinylMoraleHostSession | catalog, acquisition, replay and persistence proof | Focused vinyl tests | Owner emits/reads a typed fact; no mirror state. |
| rare record broadcast fact | Radio culture bridge | 30-record metadata and lookup | VinylRecordCatalog | Owner emits/reads a typed fact; no mirror state. |
| rare record broadcast fact | Radio culture bridge | authored acquisition routes | VinylRecordAcquisitionMap | Owner emits/reads a typed fact; no mirror state. |
| rare record broadcast fact | Radio culture bridge | playback and daily morale effect | VinylMoraleSystem | Owner emits/reads a typed fact; no mirror state. |
| rare record broadcast fact | Radio culture bridge | host commands and event projection | VinylMoraleHostSession | Owner emits/reads a typed fact; no mirror state. |
| rare record broadcast fact | Radio culture bridge | catalog, acquisition, replay and persistence proof | Focused vinyl tests | Owner emits/reads a typed fact; no mirror state. |
| catalog, acquisition, replay and persistence proof | Focused vinyl tests | 30-record metadata and lookup | VinylRecordCatalog | Owner emits/reads a typed fact; no mirror state. |
| catalog, acquisition, replay and persistence proof | Focused vinyl tests | authored acquisition routes | VinylRecordAcquisitionMap | Owner emits/reads a typed fact; no mirror state. |
| catalog, acquisition, replay and persistence proof | Focused vinyl tests | playback and daily morale effect | VinylMoraleSystem | Owner emits/reads a typed fact; no mirror state. |
| catalog, acquisition, replay and persistence proof | Focused vinyl tests | host commands and event projection | VinylMoraleHostSession | Owner emits/reads a typed fact; no mirror state. |
| catalog, acquisition, replay and persistence proof | Focused vinyl tests | rare record broadcast fact | Radio culture bridge | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 1→20 content brief with a 30-row current census and a provenance/tone review checklist. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Add a reachability matrix from every canonical record to at least one acquisition route, collectible, item or host-visible source. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Preserve once-daily playback and exactly-once rare broadcast semantics in any future catalog row. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Treat real-person/real-work provenance and copied text as a content audit issue, not as a reason to invent new gameplay fields. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.555 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/CollectibleCatalog.cs`

### `Assets/Ashfall.Core/CollectibleCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 96 lines / 3384 bytes.
- SHA-256: `4600b03742469b0cf906f3c8e20a69a46816d30ffee540386278b84f4e7deb94`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CollectibleDefinition
public string item_id = string.Empty;
public string category = string.Empty;
public string rarity = "common";
public string effect_type = "none";
public string effect_target = string.Empty;
public float effect_value;
public string location_type = string.Empty;
public bool unique;
public sealed class CollectibleCatalogFileRaw
public int schema_version = 1;
public List<CollectibleDefinition> collectibles = new List<CollectibleDefinition>();
public static class CollectibleCatalogLoader
public const string FileName = "collectibles.json";
public static CollectibleCatalog? Load( string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null) {
public sealed class CollectibleCatalog
public IReadOnlyDictionary<string, CollectibleDefinition> ByItemId => _byItemId;
public int Count => _byItemId.Count;
public CollectibleDefinition? GetByItemId(string itemId) {
public bool IsCollectible(string itemId) => _byItemId.ContainsKey(itemId);
```


# Appendix Q.556 — Additional Current Architecture Evidence: `src/Main.Collectibles.cs`

### `src/Main.Collectibles.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 162 lines / 6289 bytes.
- SHA-256: `17c24829e879e3ed175bd2f785ff3c2e8deee3af6221ca3e6377278a5aa86fed`.
- Architecture signals: seeded references=1; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public CollectibleDiscoveryState? CollectibleDiscovery => _collectibleDiscovery;
public UniqueItemClaimRegistry? UniqueClaims => _uniqueClaims;
public CollectibleEffectDispatcher? CollectibleDispatcher => _collectibleDispatcher;
public void MarkCollectiblesDirty() => _collectiblesDirty = true;
```


# Appendix Q.557 — Additional Current Architecture Evidence: `src/Host/CollectibleDiscoverySaveStore.cs`

### `src/Host/CollectibleDiscoverySaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 49 lines / 2437 bytes.
- SHA-256: `117edb3e4cc9ceed58075edb0bf6026b9ee6d8aa9cdee311175bba8309d557d5`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class CollectibleDiscoverySaveStore
public const string FileName = "collectible_discovery_save.json";
public const string SectionName = "collectible_discovery";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(CollectibleDiscoverySave state) => s_store.CaptureBare(state);
public static CollectibleDiscoverySave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static bool TrySave(CollectibleDiscoverySave state) => s_store.TrySave(state);
public static CollectibleDiscoverySave? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(CollectibleDiscoverySave state) => s_store.CapturePersisted(state);
```


# Appendix Q.558 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs`

### `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 782 lines / 31079 bytes.
- SHA-256: `1c3897163c7cd9260c589b5d055f69aeed910b69cbe318aa629523bb5aa5f424`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
internal sealed class ItemJsonDto
public string id { get; set; } = string.Empty;
public string displayName { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public string iconPath { get; set; } = string.Empty;
public string type { get; set; } = string.Empty;
public int stackMax { get; set; } = 1;
public float weight { get; set; }
public float radProtection { get; set; }
public float durability { get; set; }
public float degradeRate { get; set; }
public float degrade_rate { get; set; }
public bool isEquipable { get; set; }
public string equipSlot { get; set; } = string.Empty;
public float contamination { get; set; }
public float hungerRestore { get; set; }
public float thirstRestore { get; set; }
public float healthEffect { get; set; }
public float radCleanse { get; set; }
public float moraleEffect { get; set; }
public float decorLocalizedMoraleDelta { get; set; }
public bool empShielded { get; set; }
public float tradeValue { get; set; }
public int tradeTier { get; set; }
public float disassembleYieldFraction { get; set; } = 0.5f;
public List<string>? tags { get; set; }
public List<ScrapYieldDto>? scrapValue { get; set; }
public RepairRecipeDto? repairRecipe { get; set; }
public LimbRequirementDto? limbRequirements { get; set; }
public LimbRequirementDto? limb_requirements { get; set; }
public LimbProvisionDto? providesLimb { get; set; }
public LimbProvisionDto? provides_limb { get; set; }
internal sealed class LimbRequirementDto
public int hands { get; set; } = 1;
public string? gripClass { get; set; }
public string? grip_class { get; set; }
internal sealed class LimbProvisionDto
public int hands { get; set; }
public int legs { get; set; }
public int qualityPermille { get; set; } = 500;
public int quality_permille { get; set; } = 500;
public string? gripClass { get; set; }
public string? grip_class { get; set; }
internal sealed class ScrapYieldDto
public string materialId { get; set; } = string.Empty;
public int amount { get; set; } = 1;
internal sealed class RepairRecipeDto
public List<ScrapYieldDto>? costs { get; set; }
public float hours { get; set; } = 0.5f;
public bool requiresTools { get; set; } = true;
public float max_repair_condition_fraction { get; set; } = 1.0f;
internal sealed class StartingSupplyJsonDto
public string itemId { get; set; } = string.Empty;
public int amount { get; set; } = 1;
internal sealed class StartingSuppliesProfileJsonDto
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public List<StartingSupplyJsonDto> supplies { get; set; } =
internal sealed class StartingSuppliesRootJsonDto
public int schema_version { get; set; } = 1;
public string default_profile_id { get; set; } = string.Empty;
public List<StartingSupplyJsonDto>? starting_supplies { get; set; }
public List<StartingSuppliesProfileJsonDto>? profiles { get; set; }
public enum StartingSuppliesLoadStatus
public sealed class StartingSuppliesLoadResult
public StartingSuppliesLoadStatus Status { get; set; } = StartingSuppliesLoadStatus.Success;
public string ErrorMessage { get; set; } = string.Empty;
public string SelectedProfileId { get; set; } = StartingSuppliesCatalog.StandardProfileId;
public int AcceptedRowCount => Supplies.Count;
public bool IsSuccess => Status == StartingSuppliesLoadStatus.Success;
public sealed class StartingSuppliesCatalogLoadResult
public StartingSuppliesCatalog Catalog { get; internal set; } =
public List<string> Errors { get; } = new List<string>();
public List<string> Warnings { get; } = new List<string>();
public bool UsedLegacyFallback { get; internal set; }
public bool IsUsable => Catalog.Profiles.Count > 0;
public static class ItemCatalogLoader
public const string PrimaryFileName = "items.json";
public const string StartingSuppliesFileName = "starting_supplies.json";
public const string ItemDescriptionsFileName = ItemDescriptionCatalogLoader.PrimaryFileName;
public static ItemCatalog LoadCatalog(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static List<ItemDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static ItemDescriptionCatalog LoadDescriptionCatalog(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static CatalogLoadResult<ItemDescriptionCatalog> LoadDescriptionCatalogWithResult(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static CatalogLoadResult<ItemCatalog> LoadCatalogWithResult( string dataDir, IFileIO fileIO, IJsonSerializer serializer, ItemCatalog? targetCatalog = null) {
public static void LoadInto(ItemCatalog catalog, string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static StartingSuppliesCatalog LoadStartingSuppliesCatalog( string dataDir, IFileIO fileIO, IJsonSerializer serializer, ItemCatalog? catalog = null) {
public static StartingSuppliesCatalogLoadResult LoadStartingSuppliesCatalogDetailed( string dataDir, IFileIO fileIO, IJsonSerializer serializer, ItemCatalog? catalog = null) {
public static StartingSuppliesLoadResult LoadStartingSuppliesDetailed( string dataDir, IFileIO fileIO, IJsonSerializer serializer, ItemCatalog? catalog = null, string? profileId = null)
internal static ItemDefinition ConvertDto(ItemJsonDto dto) {
```


# Appendix Q.559 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/RadioRecordingSystem.cs`

### `Assets/Ashfall.Core/Radio/RadioRecordingSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 106 lines / 4606 bytes.
- SHA-256: `7517b367865804c98714f607869c81f5a69ef80b2d8916a60b94564e9d5ab16d`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RadioRecordingSystem
public const string BlankTapeItemId = "item_blank_magnetic_tape";
public event Action<RecordedCassetteEntry>? OnBroadcastRecorded;
public IReadOnlyCollection<RecordedCassetteEntry> RecordedTapes => _recordedTapes.Values;
public RecordedCassetteEntry? RecordBroadcast(ScheduledBroadcastResult broadcast, int day) {
public RecordedCassetteEntry? ReplayCassette(string cassetteId) {
public int CalculateTradeValue(string cassetteId) {
public List<RecordedCassetteEntry> CaptureState() {
public void RestoreState(List<RecordedCassetteEntry>? savedEntries) {
```


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Collectibles/CollectibleEffectDispatcher.cs`

### `Assets/Ashfall.Core/Collectibles/CollectibleEffectDispatcher.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 351 lines / 16641 bytes.
- SHA-256: `8b68c52ae37580be2981c55da7f02b4c4b83dc63480d13f19525b5f38c0e5d93`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class CollectibleDispatchResult
public bool IsCollectible;
public bool AlreadyDiscovered;
public string EffectType = string.Empty;
public bool EffectApplied;
public bool DiscoveryRegistered;
public string FailureReason = string.Empty;
public string? DiscoveryLocationId;
public bool HasDiscoveryEffects => !string.IsNullOrEmpty(EffectType) && EffectType != "none" && EffectApplied;
public class CollectibleEffectDispatcher
public const float MaxMoraleEffectValue = 10f;
public CollectibleDiscoveryState Discovery => _discovery;
public event Action<CollectibleDispatchResult>? OnCollectibleDiscovered;
public CollectibleDispatchResult DispatchOnAcquire(string itemId, string? discoveryLocationId = null) {
public CollectibleMigrationReport ReconcileDiscoveredSubsystemState( Func<VinylMoraleSystem?>? vinylProvider = null, ISeededRng? vinylRng = null) {
public sealed class CollectibleMigrationReport
public int KnowledgeReconciled;
public int LocationReconciled;
public int VinylChecked;
```


# Appendix Q.562 — Additional Current Architecture Evidence: `src/Host/HostCli.Collectibles.cs`

### `src/Host/HostCli.Collectibles.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 495 lines / 30626 bytes.
- SHA-256: `3ac21d702fc5aa4eab294bb7503aef4f7638ad980a5e80e49de3d986ac7f942c`.
- Architecture signals: seeded references=5; save/restore symbols=10; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunCollectibleSelfTest(string dataDirectory) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `src/Host/AssetCoverageReport.cs`

### `src/Host/AssetCoverageReport.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 93 lines / 3805 bytes.
- SHA-256: `e46cde7a0d26f5cbd7ed14d27aebaf7d43698000df6f4ef890b227e599643c14`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public struct AssetCoverageResultRow
public string Id;
public string Category;
public string? ResolvedPath;
public bool Exists;
public bool Loaded;
public int ReferenceCount;
public struct AssetCoverageSummaryReport
public int TotalChecked;
public int Missing;
public int UniqueMissing;
public int DuplicateFallbackRequests;
public int FailedToLoad;
public int Passed;
public int ProbeFailures;
public List<AssetCoverageResultRow> Rows;
public string Summary;
public bool Clean => Missing == 0 && FailedToLoad == 0 && ProbeFailures == 0;
public static class AssetCoverageReport
public static void PrintSummary(AssetCoverageSummaryReport report) {
public static void PrintFullCoverageSweep( int totalIds, int totalMissing, Dictionary<string, List<string>> missingByCategory, Dictionary<string, List<string>> idsByCategory) {
```


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/UI/CollectiblePresentationModel.cs`

### `Assets/Ashfall.Core/UI/CollectiblePresentationModel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 177 lines / 7493 bytes.
- SHA-256: `9659242d0a91255e4cc33ec0257ec8a6f1f71fe8647bd197772a704e1900d522`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CollectiblePresentationModel
public string ItemId { get; }
public string DisplayName { get; }
public string Category { get; }
public string Rarity { get; }
public CollectibleDiscoveryStatus DiscoveryStatus { get; }
public string DiscoveryStateText { get; }
public bool IsNewDiscovery { get; }
public string EffectIntentText { get; }
public bool HasEffectBenefit { get; }
public string Description { get; }
public bool IsLocked { get; }
public string LockedReason { get; }
public string AccessibleLabel { get; }
public string TooltipText => AccessibleLabel;
public static string FormatCategory(string? rawCategory) {
public static string FormatRarity(string? rawRarity) {
public static string FormatEffectIntent(string? effectType, string? effectTarget, float effectValue) {
```


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs`

### `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 674 lines / 31034 bytes.
- SHA-256: `44b077c262c9c05de45f09f97809e9067c307d3a0349bdaa8edf5fa008e94c34`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=23; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ArchiveDocumentState
public string document_id = string.Empty;
public int physical_degradation_permille;        // 0..1000
public bool is_chemically_stabilized;
public int transcription_permille;               // 0..1000
public string active_scholar_id = string.Empty;
public int microfiche_copy_count;
public bool knowledge_preserved;                 // the permanent unlock (microfiche)
public string status = "archived";               // archived | transcribing | transcribed | lost
public sealed class ArchiveProjectState
public string document_id = string.Empty;
public string kind = string.Empty;               // restoration | transcription
public string survivor_id = string.Empty;
public int started_day = -1;
public int last_progress_day = -1;
public sealed class ArchiveRecordingState
public string recording_id = string.Empty;
public string category = string.Empty;           // music_performance | oral_history | survivor_testimony | radio_archive | commemorative
public string operator_id = string.Empty;
public int recorded_day = -1;
public sealed class ArchiveSalonState
public bool active;
public string modifier_key = "salon_stress_resistance";
public int start_day = -1;
public int duration_days;
public int cooldown_until_day = -1;
public sealed class ArchiveChronicleEntry
public string chronicle_id = string.Empty;
public int campaign_day;
public string event_type = string.Empty;
public string summary_key = string.Empty;
public List<string> participants = new();
public string author_id = string.Empty;
public string volume_id = string.Empty;
public sealed class CulturalArchiveVaultSave
public int schema_version = 1;
public List<ArchiveDocumentState> documents = new();
public List<ArchiveProjectState> active_projects = new();
public List<ArchiveRecordingState> recordings = new();
public List<ArchiveChronicleEntry> chronicle_entries = new();
public ArchiveSalonState salon = new();
public int next_chronicle_ordinal;
public float degradation_remainder;              // deterministic fractional permille carry
public DocumentationState documentation = new();
public sealed class CulturalArchiveVaultSystem
public const string SystemId = "cultural_archives";
public const string InstitutionId = "institution_cultural_archive";
public const int RestorationReliefPermille = 350;
public const int LegibilityLimitPermille = 900;   // above this, pages cannot be worked
public const int LostThresholdPermille = 1000;
public const float BaseDailyDegradationPermille = 2f;
public const int SalonDefaultDurationDays = 5;
public const int SalonCooldownDays = 10;
public const float SalonMoralePerDay = 2f;
public const string CutDiscCostItemId = "acetate_blank_disc";
public DocumentationSystem Documentation { get; }
public event Action<string>? OnDocumentRestored;               // documentId
public event Action<string>? OnMicroficheCreated;              // documentId
public event Action<string>? OnTomeTranscribed;                // documentId
public event Action<string>? OnDocumentLost;                   // documentId
public event Action<string, VinylRecordDefinition>? OnArchiveRecordingCreated;
public event Action<int>? OnSalonStarted;                      // day
public event Action<int>? OnSalonEnded;                        // day
public event Action<float>? OnSalonMoraleTick;                 // morale delta, once per day while active
public event Action<ArchiveChronicleEntry>? OnChronicleEntryAdded;
public event Action? OnDocumentationChanged;
public void LoadTomeCatalog(List<CulturalArchiveTomeDefinition> tomes) {
public IReadOnlyList<ArchiveDocumentState> Documents => _state.documents.AsReadOnly();
public IReadOnlyList<ArchiveRecordingState> Recordings => _state.recordings.AsReadOnly();
public IReadOnlyList<ArchiveChronicleEntry> Chronicle => _state.chronicle_entries.AsReadOnly();
public ArchiveSalonState Salon => _state.salon;
public ArchiveDocumentState? GetDocument(string documentId) =>
public ActionResult TryRestoreDocument(string documentId) {
public ActionResult TryStartTranscription(string documentId, string scholarId) {
public ActionResult TryCreateMicroficheCopy(string documentId, string operatorId) {
public static readonly string[] LegalRecordingCategories = {
public ActionResult TryCutArchiveDisc(string recordingId, string category, string operatorId, int day) {
public static VinylRecordDefinition BuildRecordDefinition(ArchiveRecordingState recording) {
public ActionResult TryStartSalon(int day) {
public ActionResult TryRecordChronicleEntry( string eventType, int campaignDay, string summaryKey, IReadOnlyList<string>? participants, string authorId = "", string? volumeId = null) {
public void TickDay(int day) {
public ActionResult TryCreatePhotograph( string authorId, string title, string cameraUsed, IEnumerable<string>? subjects, string locationId,
public ActionResult TryCreateSketch( string authorId, string title, string subject, string medium, float artisticQuality,
public ActionResult TryCreateWrittenRecord( string authorId, string title, string recordType, string content, float writingQuality,
public ActionResult TryShareDocumentation(string documentationId, int currentDay, out float moraleBoost) {
public CulturalArchiveVaultSave CaptureState() {
public void RestoreState(CulturalArchiveVaultSave? saved) {
```


# Appendix Q.566 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Audio/CassettePlaybackSystem.cs`

### `Assets/Ashfall.Core/Audio/CassettePlaybackSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 236 lines / 8914 bytes.
- SHA-256: `d468b84d48b5e107676782a0444b396ed104059fcbea63b4d69cbfcdb429c876`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CassettePlaybackState
public List<string> collectedPartItemIds = new List<string>();
public List<string> playedPartItemIds = new List<string>();
public List<string> completedSetIds = new List<string>();
public int totalPlaybacks;
public float totalMoraleAwarded;
public sealed class CassettePlaybackSystem
public const string SystemId = "cassette_playback";
public CassettePlaybackState State => _state;
public int TotalSetsCount => _setsById.Count;
public event Action<CassettePartDefinition, CassetteSetDefinition, float>? OnTapePlayed;
public event Action<CassetteSetDefinition>? OnSetCompleted;
public void LoadCatalog(IEnumerable<CassetteSetDefinition> sets) {
public bool AcquirePart(string itemId) {
public ActionResult PlayPart(string itemId, int simDay = -1) {
public bool TryGetPart(string itemId, out CassettePartDefinition? partDef, out CassetteSetDefinition? setDef) {
public bool IsPartCollected(string itemId) {
public bool IsPartPlayed(string itemId) {
public bool IsSetComplete(string setId) {
public void GetSetProgress(string setId, out int collected, out int total) {
public IReadOnlyList<string> GetDiscoveredCacheLocations() {
public IReadOnlyList<string> GetCacheItems(string cacheLocation) {
public CassettePlaybackState CaptureState() {
public void RestoreState(CassettePlaybackState? saved) {
```


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Collectibles/CollectibleMapProjector.cs`

### `Assets/Ashfall.Core/Collectibles/CollectibleMapProjector.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 152 lines / 6514 bytes.
- SHA-256: `e7ba58e122c2940d4244a89d6f71803099642aa9cf892939f3866b6a4591bada`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CollectibleMapMarker
public const string SemanticRoleName = "Collectible discovery";
public string MarkerId { get; }
public string CollectibleId { get; }
public string DisplayName { get; }
public string Category { get; }
public string LocationId { get; }
public string FoundHereLabel { get; }
public string AccessibleText { get; }
public string SemanticRole => SemanticRoleName;
public static string GenerateMarkerId(string collectibleId, string locationId) =>
public sealed class CollectibleMapCluster
public string LocationId { get; }
public IReadOnlyList<CollectibleMapMarker> Markers { get; }
public int Count => Markers.Count;
public string AccessibleText { get; }
public static class CollectibleMapProjector
public static IReadOnlyList<CollectibleMapMarker> ProjectMarkers( CollectibleDiscoveryState discoveryState, CollectibleCatalog catalog, IReadOnlyDictionary<string, string>? itemDisplayNames = null) {
public static IReadOnlyList<CollectibleMapCluster> ClusterByLocation(IEnumerable<CollectibleMapMarker> markers) {
```


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/HostCliRegistry.cs`

### `Assets/Ashfall.Core/HostCliRegistry.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1912 lines / 102959 bytes.
- SHA-256: `827182dd993f0bff556c0f2e1ba84448391b5b2b728a0d2242780da4594d9940`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum HostCliAction
public sealed class HostCliActionDescriptor
public HostCliAction Action { get; }
public string Category { get; }
public string PrimaryFlag { get; }
public IReadOnlyList<string> Aliases { get; }
public string Description { get; }
public string ValuePlaceholder { get; }
public IReadOnlyList<string> AllFlags { get; }
public bool IsSelfTest { get; }
public bool IsTest { get; }
public bool HeadlessCompatible { get; }
public string TestId { get; }
public string FormatHelpLine() {
public static class HostCliRegistry
public static readonly IReadOnlyList<string> Categories = new ReadOnlyCollection<string>(new[] {
public static IReadOnlyList<HostCliActionDescriptor> AllDescriptors => _descriptors;
public static IReadOnlyDictionary<string, HostCliActionDescriptor> FlagMap => _flagMap;
public static IReadOnlyList<HostCliActionDescriptor> CoreDescriptors => _coreDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> ExpansionDescriptors => _expansionDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> HostDomainDescriptors => _hostDomainDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> UiDescriptors => _uiDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> ConfigDescriptors => _configDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> InfoDescriptors => _infoDescriptors;
public static IReadOnlyDictionary<string, HostCliActionDescriptor> ValidateFlagRegistry() {
public static IReadOnlyDictionary<string, HostCliActionDescriptor> ValidateDescriptors(IEnumerable<HostCliActionDescriptor> descriptors) {
public static HostCliAction Resolve(string[]? args) {
public static void PrintHelp(Action<string> print) {
public static void PrintSelfTests(Action<string> print) {
public static HostSelfTestManifest CreateSelfTestManifest() {
public static string GenerateJsonManifest() {
public static string GenerateMarkdownCatalog(string verifiedDate) {
public sealed class HostSelfTestManifest
public string SchemaVersion { get; set; } = "1.0.0";
public string Description { get; set; } = "";
public int TotalTests { get; set; }
public int HeadlessTestCount { get; set; }
public List<HostSelfTestItem> Tests { get; set; } = new List<HostSelfTestItem>();
public sealed class HostSelfTestItem
public string TestId { get; set; } = "";
public string Action { get; set; } = "";
public string Category { get; set; } = "";
public string PrimaryFlag { get; set; } = "";
public string[] Aliases { get; set; } = Array.Empty<string>();
public string Description { get; set; } = "";
public bool HeadlessCompatible { get; set; }
public string ExpectedSummaryId { get; set; } = "";
public int TimeoutSeconds { get; set; } = 30;
```


# Appendix Q.569 — Additional Current Architecture Evidence: `src/Host/HostCli.PanelTests.cs`

### `src/Host/HostCli.PanelTests.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 4479 lines / 250824 bytes.
- SHA-256: `23b1c0498d6a8cea2b2f69b49d342aed1d444425b6bee4d6c0b144f195e78443`.
- Architecture signals: seeded references=15; save/restore symbols=102; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=3; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=12.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunYearOfAshSaveSelfTest(string dataDirectory) {
public static int RunDutyRosterSaveSelfTest(string dataDirectory) {
public static int RunExpansionHubSaveSelfTest(string dataDirectory) {
public static int RunExpeditionSelfTest() {
public static int RunBridgeSelfTest() {
public static int RunPowerGridCatalogSelfTest() {
public static int RunExpeditionEncounterBridgeSelfTest() {
public static int RunMedicalSelfTest() {
public static int RunNarrativeSelfTest() {
public static int RunOralLoreSelfTest(string dataDirectory) {
public static int RunSurvivorsSelfTest() {
public static int RunWorldSelfTest() {
public static int RunEconomySelfTest(string dataDirectory) {
public static int RunUtilityAiSelfTest(string dataDirectory) {
public static int RunDoseLedgerSelfTest(string dataDirectory) {
public static int RunBlackFlotillaSelfTest(string dataDirectory) {
public static int RunRadioSelfTest() {
public static int RunHoldfastBriefing(string dataDirectory) {
public static int RunIceRoadTickDemo(string dataDirectory) {
public static int RunHoldfastSaveSelfTest(string dataDirectory) {
public static int RunStandaloneSystemsSelfTest() {
public static int RunPhase0SelfTest() {
public static int RunCaravanSelfTest() {
public static int RunAssetRegistrySelfTest(string dataDirectory) {
public static int RunAssetCoverageReport(string dataDirectory) {
public static int RunDay1PlayableSelfTest(string dataDirectory) {
public static int RunDay1ToDay2MilestoneSelfTest(string dataDirectory) {
public static int RunUiLayoutSelfTest(string dataDirectory) {
public static int RunSettingsSelfTest(string dataDirectory) {
public static int RunPlayableShellSelfTest(string dataDirectory) {
public static int RunShelterHazardLoopSelfTest(string dataDirectory) {
public static int RunShelterOperationsSelfTest(string dataDirectory) {
public static string SnapshotGoldenRoot() {
public static string SnapshotCaptureRoot() {
internal sealed class PanelTestFaultyFileIo : Ashfall.Core.IFileIO
public bool DirectoryExists(string path) => true;
public bool FileExists(string path) => true;
public string ReadAllText(string path) => throw new System.IO.IOException("Simulated I/O disk error");
public void WriteAllText(string path, string contents) { }
public string Combine(params string[] parts) => System.IO.Path.Combine(parts);
internal sealed class PanelTestCorruptJsonFileIo : Ashfall.Core.IFileIO
public bool DirectoryExists(string path) => true;
public bool FileExists(string path) => true;
public string ReadAllText(string path) => "{ not valid json syntax !!!";
public void WriteAllText(string path, string contents) { }
public string Combine(params string[] parts) => System.IO.Path.Combine(parts);
```


# Appendix Q.570 — Additional Current Architecture Evidence: `src/Main.ExpandedShelterSystems.cs`

### `src/Main.ExpandedShelterSystems.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 898 lines / 42067 bytes.
- SHA-256: `a6f038a1dc347c2ab4b861767374e75aa2a79d980a2e6a7b71396061978c3da2`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public void OpenExpandedPanel(string panelKey) {
```


# Appendix Q.571 — Additional Current Architecture Evidence: `src/Host/PanelBindLifecycleSelfTest.cs`

### `src/Host/PanelBindLifecycleSelfTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1371 lines / 70877 bytes.
- SHA-256: `a96e666a51d3760bb8e972a3acf6ab4fed7a74328402409cdbe372d614a9fe1c`.
- Architecture signals: seeded references=3; save/restore symbols=0; typed event declarations=0; textual Godot mentions=5; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=1; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class PanelBindLifecycleSelfTest
public static int Run(string dataDirectory = "") {
```


# Appendix Q.572 — Additional Current Architecture Evidence: `src/UI/AshfallUiHelpers.cs`

### `src/UI/AshfallUiHelpers.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 991 lines / 44280 bytes.
- SHA-256: `b8115d3dedf5c786ecb3a1523938a54c016133128cd34e939c68995a1f1fa52d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=6; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class AshfallUiHelpers
public const string FallbackIconPath = "assets/ui/Icons/icon_placeholder.png";
public const string FallbackIconResPath = "res://assets/ui/Icons/icon_placeholder.png";
public const string FallbackSurvivorResPath = "res://assets/sprites/Characters/placeholder_survivor.png";
public const string FallbackSurvivorPath = "assets/sprites/Characters/placeholder_survivor.png";
public static FontFile? LoadFont(string path) {
public static FontFile? FontBarlowRegular =>
public static FontFile? FontBarlowSemiBold =>
public static FontFile? FontBarlowBold =>
public static FontFile? FontShareTechMono =>
public static void ApplyFont(Label label, FontFile? font) {
public static Label MakeTitle(string text, int fontSize = Theme.FontSizeH1) {
public static Label MakeSectionHeader(string text) {
public static Label MakeSubsectionHeader(string text) {
public static Label MakeBody(string text, bool autowrap = true) {
public static Label MakeSmall(string text, bool autowrap = false) {
public static Label MakeMono(string text) {
public static Label MakeLabel(string text) {
public static Label MakeLabel(string text, int fontSize, (float r, float g, float b, float a) colorToken) {
public static Label MakeLabel(string text, int fontSize, Color color) {
public static Label MakeLabel(string text, int fontSize, bool bold) {
public static Label MakeMetadata(string text, bool autowrap = false) {
public static Label MakeWarning(string text) {
public static Label MakeCritical(string text) {
public static Color ColorBackdrop => ToColor(Theme.BackdropOverlay);
public static Color ColorSurface => ToColor(Theme.Surface);
public static Color ColorSurfaceCard => ToColor(Theme.SurfaceCard);
public static Color ColorPrimary => ToColor(Theme.Warm);
public static Color ColorHighlight => ToColor(Theme.Hot);
public static Color ColorText => ToColor(Theme.Pale);
public static Color ColorMuted => ToColor(Theme.Muted);
public static Color ColorDim => ToColor(Theme.Dim);
public static Color ColorSuccess => ToColor(Theme.Success);
public static Color ColorWarning => ToColor(Theme.Warning);
public static Color ColorCritical => ToColor(Theme.Critical);
public static Color ColorRadiation => ToColor(Theme.Radiation);
public static Color ColorRadiationAcute => ToColor(Theme.RadiationAcute);
public static Color ColorInfo => ToColor(Theme.Info);
public static ColorRect MakeBackdropOverlay() {
public static Label MakeSuccess(string text) {
public static Label MakeInfo(string text) {
public static Label MakeRadiation(string text, bool acute = false) {
public static VBoxContainer MakeVBox(int separation = Theme.SpacingSm) {
public static HBoxContainer MakeHBox(int separation = Theme.SpacingSm) {
public static MarginContainer MakeMargins(int all = Theme.HudPanelPadding) {
public static MarginContainer MakeMargins(int left, int top, int right, int bottom) {
public static Control MakeEmptyState( string message, string title = "NO DATA RECORDED", string? actionHint = null, (float r, float g, float b, float a)? accentColor = null) {
public static Label MakeEmptyStateLabel(string message, string? hint = null) {
public static PanelContainer MakePanel(int minWidth = 0, int minHeight = 0) {
public static StyleBox MakePanelFrameStyleBox() {
public static StyleBox MakeHeaderFrameStyleBox() {
public static PanelContainer MakeHeaderBar() {
public static PanelContainer MakeCard(int minW = 0, int minH = 0) => MakePanel(minW, minH);
public static PanelContainer MakeCardFrame(string title, string? subtitle = null, int minW = 0, int minH = 0) {
public static HSeparator MakeSeparator() {
public static Button MakeButton(string text, Action onPressed, bool disabled = false) {
public static Button MakeDisabledButton(string text, string reasonDisabled) {
public static Control MakeSeverityBadge(SeverityLevel level, string text, string? customIcon = null) {
public static Color GetSeverityColor(SeverityLevel level) => level switch
public static string GetSeverityIcon(SeverityLevel level) => level switch
public static HBoxContainer MakeDataRow(string label, string value, Color? valueColor = null, int fontSize = Theme.FontSizeSmall) {
public static Label MakeDimLabel(string text) {
public static Label MakeColoredLabel(string text, (float r, float g, float b, float a) colorToken, int fontSize = Theme.FontSizeBody) {
public static HBoxContainer MakeActionBar(int separation = Theme.SpacingSm) {
public static TextureRect MakeFactionEmblem(string factionId, int size = 40) {
public static TextureRect MakeBadgeIcon(string badgeId, int size = 32) {
public static TextureRect MakeItemIcon(string itemId, int size = 32) {
public static Color ToColor((float r, float g, float b, float a) token) {
public static TextureRect? MakeSurvivorPortrait(string survivorId, int size = 56) {
public static Texture2D? TryLoadTexture(string path) {
public static StyleBoxFlat MakeFlatBg(Color bg, Color? border = null, int borderWidth = 1, int cornerRadius = 0) {
public static void EmptyChildren(Node parent) {
public static void EmptyChildrenExcept(Node parent, Node preservedChild) {
public static string FormatDoseMsv(float msv) {
public static string FormatDosePairMsv(float nominalMsv, float bookedMsv) {
public static string FormatDoseSource(Ashfall.Core.DoseContentCatalog? content, string? sourceId) {
```


# Appendix Q.573 — Additional Current Architecture Evidence: `src/Economy/EconomyMarketPanel.cs`

### `src/Economy/EconomyMarketPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 226 lines / 9684 bytes.
- SHA-256: `537efab7a1ffe7ce2ac210700b0e04f6260f0affc9d80669ffa510d139ab7054`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class EconomyMarketPanel : PanelContainer
public string CurrentRegion { get; set; } = "settlement";
public override void _Ready() {
public void BindSession(EconomyHostSession session) {
public void BindStance(Ashfall.Core.Economy.IFactionStanceProvider provider, string factionId) {
public void UnbindSession() {
public override void _ExitTree() {
public void RefreshView() {
```


# Appendix Q.574 — Additional Current Architecture Evidence: `src/UI/TriangulationPanel.cs`

### `src/UI/TriangulationPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 233 lines / 8962 bytes.
- SHA-256: `09b3141f69c0a707aef71f4f15d53a56331512f0863f3d4c4806d9562dafd232`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=4; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class TriangulationPanel : Control
public event Action? OnClose;
public event Action<string>? OnLocationDiscovered;
public bool IsBound => _radioHost != null;
public int RefreshCount { get; private set; }
public void Bind(RadioHostSession radioHost, string signalId = "") {
public void Open() {
public override void _Ready() {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix Q.575 — Additional Current Architecture Evidence: `src/World/SurvivorActorView.cs`

### `src/World/SurvivorActorView.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 308 lines / 11695 bytes.
- SHA-256: `14c093c9d0fffdcf3ade45ffa6304620a3cf6952f77f51a9c2516f137465b059`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class SurvivorActorView : CharacterBody2D
public const string FallbackTexturePath = AssetRegistry.FallbackSurvivorPath;
public const float Gravity = 980f;
public const float MaxSeekSpeed = 74f;
public const float SeekAccel = 460f;
public const float ArriveRadius = 3f;
public Label Label { get; private set; }
public Sprite2D Sprite { get; private set; }
public Sprite2D Body { get; private set; }
public ColorRect HealthIndicator { get; private set; }
public ColorRect RadiationIndicator { get; private set; }
public ColorRect StatusIndicator { get; private set; }
public SurvivorNeedsState SurvivorState { get; private set; }
public void SetMoveTarget(Vector2 target) {
public Vector2 MoveTarget => _moveTarget;
public bool IsMoving => _moving;
public override void _PhysicsProcess(double delta) {
public void UpdatePortrait() {
public void UpdateFromSurvivor(SurvivorNeedsState state, SurvivorRadState? rad = null) {
```


# Appendix Q.576 — Additional Current Architecture Evidence: `src/Economy/TradeScreenGodotPanel.cs`

### `src/Economy/TradeScreenGodotPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1005 lines / 45453 bytes.
- SHA-256: `e1b1550b9352690b706cf5f7090f6fe4251b6293acbe40a80772533e4ac2452c`.
- Architecture signals: seeded references=3; save/restore symbols=0; typed event declarations=2; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class TradeScreenGodotPanel : PanelContainer
public bool HasFactionEmblem => _textureFactionEmblem?.Texture != null;
public bool HasLeaderLabel => !string.IsNullOrEmpty(_lblLeader?.Text);
public bool HasStanceBadge => !string.IsNullOrEmpty(_badgeStance?.Text);
public bool HasTrustMeter => !string.IsNullOrEmpty(_lblTrust?.Text);
public bool HasAggressionMeter => !string.IsNullOrEmpty(_lblAggression?.Text);
public bool HasRepelCounter => !string.IsNullOrEmpty(_lblRepels?.Text);
public bool HasPriceShockBanner => _shocksContainer != null;
public bool HasBioTradeRows => _bioTradeRows.Count >= 4;
public bool HasFairnessIndicator => !string.IsNullOrEmpty(_lblFairness?.Text);
public bool HasParleyButton => _btnDemandParley != null;
public bool HasRadioTicker => _lblRadioTicker != null;
public bool HasTellPlate => !string.IsNullOrEmpty(_lblTellPlate?.Text);
public bool HasTraderVoice => !string.IsNullOrEmpty(_lblTraderVoice?.Text);
public string TraderProfileId => _viewModel?.TraderProfileId ?? _activeTraderProfileId;
public bool HasNewsStrip => _newsStrip != null && _newsStrip.GetChildCount() > 0;
public bool HasArbitratorScale => _scalePlayerFill != null && _scaleFactionFill != null;
public bool IsGrimDrawerCollapsed => _grimDrawerBody == null || !_grimDrawerBody.Visible;
public bool IsViewModelBound => _viewModel != null;
public int ActiveOfferCount => _playerOfferCounts.Count;
public int ActiveAskCount => _factionAskCounts.Count;
public int ActiveBioCount => _bioOfferCounts.Count;
public event Action? OnClose;
public void Open() {
public void Close() {
public override void _Ready() {
public void BindViewModel(ITradeScreenViewModel viewModel, ITradeIntentSink intentSink) {
public void BindSession( EconomyHostSession session, IFactionStanceProvider stanceProvider = null!, IPriceShockProvider priceShockProvider = null!, IFactionRadioProvider radioProvider = null!, ISeededRng rng = null!,
public void SetActiveFaction(string factionId) {
public void FocusLedgerSection(string sectionId) {
public void SetTraderVoiceContext(TradeVoiceContext context) {
public void AddPlayerOffer(string itemId, int count) {
public void AddFactionAsk(string itemId, int count) {
public void RefreshView() {
```


# Appendix Q.577 — Additional Current Architecture Evidence: `src/Host/CollectibleEffectDispatcher.cs`

### `src/Host/CollectibleEffectDispatcher.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 30 lines / 1115 bytes.
- SHA-256: `7476d05481d7427cb88c9871fed2041a6b0818e0f00afb46438bd9877a9a545b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CollectibleEffectDispatcher : Ashfall.Core.CollectibleEffectDispatcher
```


# Appendix Q.578 — Additional Current Architecture Evidence: `src/Host/ContentUtilizationRuntimeCollector.cs`

### `src/Host/ContentUtilizationRuntimeCollector.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1230 lines / 64739 bytes.
- SHA-256: `4be289e49ddef6d5dcd29ccdc988a5f397b6153d4d20f1aa585ca9fe39df9f65`.
- Architecture signals: seeded references=3; save/restore symbols=0; typed event declarations=0; textual Godot mentions=47; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class ContentUtilizationRuntimeCollector
public const int DefaultSeed = 9001;
public static ContentUtilizationInstrumentation Collect(string dataDir) {
public bool CanApplyMorale(string survivorId, int delta, bool shelterWide, out string reason) {
public void ApplyMorale(string survivorId, int delta, bool shelterWide) { }
public bool CanGrantFactionIntel(string canonicalFactionId, out string reason) {
public void GrantFactionIntel(string canonicalFactionId) { }
public bool CanOfferExpedition(string locationId, out string reason) {
public void OfferExpedition(string locationId) { }
public bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason) {
public void ApplyFactionStanding(string canonicalFactionId, int delta) { }
```


# Appendix Q.579 — Additional Current Architecture Evidence: `src/Host/HostCli.StartingSupplies.cs`

### `src/Host/HostCli.StartingSupplies.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 154 lines / 6155 bytes.
- SHA-256: `741b811cd864dfe7e7775ceb17d795f7054dffa6a12ab0fa273a6af3aac9bd30`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=1.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunStartingSuppliesSelfTest(string dataDirectory) {
```


# Appendix Q.580 — Additional Current Architecture Evidence: `src/Host/HostCli.cs`

### `src/Host/HostCli.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1130 lines / 85981 bytes.
- SHA-256: `f83b99e37e991491c5a219476b27a93aa2fdb6ba530de12ba746cc75bfea81c5`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=6; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum HostCliAction
public static partial class HostCli
public static string? ExtractArgValue(string[]? args, string flag) {
public static void ConfigureHostEnvironment(string[]? args) {
public static HostCliAction Parse(string[] args) {
public static void PrintHelp() {
public static void PrintVersion(string dataDir) {
```


# Appendix Q.581 — Additional Current Architecture Evidence: `src/Host/InventoryHostSession.cs`

### `src/Host/InventoryHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 721 lines / 38324 bytes.
- SHA-256: `89d74d0fe6814c5dc75f9106ac26714153475da73d5f44a46bd2ba6fb12f7e7e`.
- Architecture signals: seeded references=0; save/restore symbols=5; typed event declarations=3; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class InventoryHostSession
public InventoryContainer Inventory { get; }
public ItemCatalog Catalog { get; }
public ItemDescriptionCatalog DescriptionCatalog { get; set; }
public ExpansionEnrichmentCatalog? EnrichmentCatalog { get; set; }
public SurvivorsHostSession? Survivors { get; set; }
public Func<string, ItemType, float, bool>? ApplyNeedOverride { get; set; }
public Func<string, string, int, int, int, ResourceAllocationDecision?>? RationingAuthorizer { get; set; }
public Action<string, float>? ApplyRadCleanseOverride { get; set; }
public Action<string>? ApplyIodineOverride { get; set; }
public Action<string, float>? ApplyContaminationOverride { get; set; }
public Func<string?>? DefaultSurvivorResolver { get; set; }
public string LastEvent { get; private set; } = string.Empty;
public static InventoryHostSession CreateForFixture( InventoryContainer? inventory = null, ItemCatalog? catalog = null) {
public static void SeedCatalogForTest(ItemCatalog catalog) {
public static InventoryHostSession Create( string? dataDir = null, string? startingSuppliesProfileId = null, bool seedWhenNoSave = true) {
public ItemInspectionModel? GetInspection(string itemId) {
public void LoadOrSeedStartingSupplies( string dataDir, IFileIO fileIO = null!, IJsonSerializer serializer = null!, bool failClosed = true, string? profileId = null)
public void SeedStartingSupplies() {
public bool TryAdd(string itemId, int amount) {
public string Add(string itemId, int amount) {
public string Remove(string itemId, int amount) {
public string Equip(string itemId) => EquipResult(itemId).MessageKey;
public ActionResult EquipResult(string itemId) {
public string Unequip(string slotName) {
public string? ResolveTargetSurvivorId(string? requestedSurvivorId = null) {
public string Consume(string itemId, float therapeuticScale = 1f) => ConsumeResult(itemId, null, therapeuticScale).MessageKey;
public string Consume(string itemId, string? survivorId, float therapeuticScale = 1f) => ConsumeResult(itemId, survivorId, therapeuticScale).MessageKey;
public ActionResult ConsumeResult(string itemId, float therapeuticScale = 1f) => ConsumeResult(itemId, null, therapeuticScale);
public ActionResult ConsumeResult(string itemId, string? survivorId, float therapeuticScale = 1f) {
public Action<string, string>? OnConsumed;
public int CurrentDay { get; set; } = 1;
public Action<string, int>? OnAntiRadAdministered { get; set; }
public Action<string, string, Ashfall.Core.Medical.ChemicalDependencyKind>? OnChemicalSubstanceConsumed { get; set; }
public Ashfall.Core.Medical.MedicalRecordLog? MedicalRecordLog { get; set; }
public List<Ashfall.Core.Campaign.DayStateChangeEvent> PendingDayEvents { get; } = new List<Ashfall.Core.Campaign.DayStateChangeEvent>();
public void DrainDayEvents(List<Ashfall.Core.Campaign.DayStateChangeEvent> target) {
public string InventoryLine() {
public string EquipLine() {
public InventorySaveState CaptureSave() => Inventory.CaptureState();
public void RestoreSave(InventorySaveState state) {
```


# Appendix Q.582 — Additional Current Architecture Evidence: `src/Main.Application.cs`

### `src/Main.Application.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1144 lines / 55746 bytes.
- SHA-256: `ea35c17ff8c236beead0d68584cde85f606db646aa0072a8ca203c74cb1893ed`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=7; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public override void _Ready() {
public override void _Process(double delta) {
public override void _UnhandledKeyInput(InputEvent @event) {
public override void _Notification(int what) {
```


# Appendix Q.583 — Additional Current Architecture Evidence: `src/Main.Inventory.cs`

### `src/Main.Inventory.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 269 lines / 11760 bytes.
- SHA-256: `7f22de47729189d6c5ab6223cd95c137e04d977b50300fcb8f608a796f456ddc`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.584 — Additional Current Architecture Evidence: `src/Main.Lifecycle.cs`

### `src/Main.Lifecycle.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 667 lines / 27644 bytes.
- SHA-256: `301a7cd6ee1481cd50b89db465c206737175c21f73a04890737b34c0f54f0b88`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=5; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public void ResetAllSessionsInMemory() {
public void DeleteGlobalSavesOnDisk() {
public void RegisterManifestSetupActions() {
public int ExecuteSubsystemManifestBootstrap(LifecyclePhase? phase = null) {
```


# Appendix Q.585 — Additional Current Architecture Evidence: `src/Main.Narrative.cs`

### `src/Main.Narrative.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 728 lines / 29845 bytes.
- SHA-256: `b588f3d83df8088676183b79152ffa57c62a3b5d30ed8c7cb73ae0a4b7ece863`.
- Architecture signals: seeded references=0; save/restore symbols=6; typed event declarations=0; textual Godot mentions=9; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.586 — Additional Current Architecture Evidence: `src/UI/RadioPanel.cs`

### `src/UI/RadioPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 700 lines / 33756 bytes.
- SHA-256: `38a56568e4a4b51ea77c9054ff60cbd6fab8ea2771bf0803089a6959663b937e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=4; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class RadioPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action? OnRadioBroadcastSent;
public bool IsBound => _radioHost != null;
public bool IsProductionBound => _productionHost != null;
public int RenderedSignalCount => _interceptsGrid?.RowCount ?? 0;
public void Bind(RadioHostSession radio) {
public void BindProduction(RadioProgramProductionHostSession production) {
public void Unbind() {
public void RefreshView() {
public override void _Ready() {
public void Open() {
public void Close() {
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
```


# Appendix R.587 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Radio/RadioRecordingSystemTests.cs`

### `Ashfall.Core.Tests/Radio/RadioRecordingSystemTests.cs`

- Current test declarations: Fact=2, Theory=0, InlineData=0.
- File lines: 67; SHA-256: `acba03d69e2de525afa613d9b97f227bffc308a4b9e8cee28bb7924369cf307f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RecordBroadcast_CreatesCassetteEntry_AndReplayIsNonMutating
CalculateTradeValue_IntelligenceCarriesHigherValueThanRoutine
```


# Appendix R.588 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/CollectibleCatalogTests.cs`

### `Ashfall.Core.Tests/CollectibleCatalogTests.cs`

- Current test declarations: Fact=14, Theory=0, InlineData=0.
- File lines: 231; SHA-256: `2576aaf6254d8d9c6d412cf605b804a448639f31798be51ae1379f56c0f0ff35`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_Loads
Catalog_Has40Entries
Catalog_AllItemIdsResolve
Catalog_AllCategoriesPresent
Catalog_CategoryDistribution
Catalog_ValidRarityValues
Catalog_ValidEffectTypes
Catalog_NonNoneEffectsHaveTarget
Catalog_UniqueCount
Catalog_ItemIdsAreCollectible
Catalog_NoDuplicateItemIds
Catalog_GetByItemId
Catalog_IsCollectible
Catalog_MissingFile_ReturnsNull
```


# Appendix R.589 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleSaveMigrationTests.cs`

### `Ashfall.Core.Tests/Collectibles/CollectibleSaveMigrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 215; SHA-256: `14d582081994ea61924ac8bf5497896ef59eb2cf7148bb8cd5b7dcd6ebda033b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
LegacyCaseA_ManualDiscovered_NodeReconciledOnce_Idempotent
LegacyCaseB_MapDiscovered_LocationReconciledAsSurveyed_RoutesUntouched
LegacyCaseC_VinylDiscovered_OwnershipReconciled_NeverMorale
FullMigration_CaptureReload_NoRepeatEffect
```


# Appendix R.590 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleMapIntegrationTests.cs`

### `Ashfall.Core.Tests/Collectibles/CollectibleMapIntegrationTests.cs`

- Current test declarations: Fact=11, Theory=0, InlineData=0.
- File lines: 267; SHA-256: `89400de14710cc9a1474f622558bef0981c165d21a9863e558686f1cf8396fa0`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
DiscoveringCollectible_RecordsOriginLocation
DiscoveringCollectible_CreatesMapProjection
CollectibleMarker_UsesCorrectDiscoveryLocation
CollectibleMarker_ShowsNameOrCategory
CollectibleMarker_SurvivesSaveLoad
CollectibleMarker_DoesNotExposeHiddenEffectTarget
MultipleCollectiblesAtSameLocation_RemainSeparateLogicalMarkers
RepeatedMapProjection_DoesNotDuplicateMarkers
CollectibleMarker_HasAccessibleText
LegacyDiscoveryWithoutLocation_DoesNotCrashOrFabricateMarker
MapRevealEffectAndDiscoveryMarker_AreIndependentConcepts
```


# Appendix R.591 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/CollectibleCodexUnlockLiveTests.cs`

### `Ashfall.Core.Tests/CollectibleCodexUnlockLiveTests.cs`

- Current test declarations: Fact=3, Theory=2, InlineData=0.
- File lines: 226; SHA-256: `f93ce88d3f5ad9b0efc9e5a7e0b800dcccb7cf68a8cf290deff4ae0c8423dee1`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
EveryLiveCodexCollectible_WritesAuthoredEntry_OnFirstAcquisition
RepeatAcquisition_IsIdempotent
CodexAlreadyKnowsKey_SecondDiscovery_RegistersWithoutDuplicateEntry
SaveRestore_PreservesUnlocks_WithoutReplayingNotifications
FactionInfoAcquisition_DoesNotMutateFactionStanding
```


# Appendix R.592 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Visual/AssetFallbackDiagnosticsTests.cs`

### `Ashfall.Core.Tests/Visual/AssetFallbackDiagnosticsTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 139; SHA-256: `bb711cee506e16cd2a254020b0f3a5ef55005c4bc5be41a2989ec245532491ae`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
FactionIconCatalog_FallbackPath_IsCanonicalAndRelative
FactionIconCatalog_UnknownId_ResolvesToFallback
FactionIconCatalog_AllMappedPaths_AreRelativeAndNeverUseResScheme
HostAssetRegistry_FallbackPaths_UseResSchemeOnlyAtGodotResourceBoundary
CoreUI_AssetCatalogs_UseRelativePathsOnly
CanonicalFallbackAssets_ExistOnDisk
```


# Appendix R.593 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CrossPlanCollectibleIntegrationTests.cs`

### `Ashfall.Core.Tests/Collectibles/CrossPlanCollectibleIntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 301; SHA-256: `9c0a95980f1dda40c3d55a5bacd6f93f2092ffcee697954ecfab3f4a274bfeef`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CrossPlanIntegrationMatrix_HasNoUndocumentedGaps
CrossPlanIntegration_WiredRowsHaveExecutableVerification
CrossPlanIntegration_DeferredRowsDescribeBlockerAndResolution
```


# Appendix R.594 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleCampaignSmokeTests.cs`

### `Ashfall.Core.Tests/Collectibles/CollectibleCampaignSmokeTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 529; SHA-256: `c47c5a1157190d73e0f256e720f645d23ab3d9435d60fc1a6bc81eb28237f53b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CollectibleCampaignSmoke_CatalogValuesWithinBounds
CollectibleCampaignSmoke_Seed42_CompletesFullLifecycle
CollectibleCampaignSmoke_UniqueCollectiblesAppearAtMostOnce
CollectibleCampaignSmoke_ThreeRunsProduceIdenticalTrace
CollectibleCampaignSmoke_ThreeRunsProduceIdenticalFinalHash
```


# Appendix R.595 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleDispatcherHardeningTests.cs`

### `Ashfall.Core.Tests/Collectibles/CollectibleDispatcherHardeningTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 295; SHA-256: `b06445d21c0eda460dd08ee20dafa80650177fbacc1615b97ee19f791f2e3d4f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ManualAcquisition_RevealsKnowledgeNode_NeverCompletesIt
ManualReveal_UnrelatedNodesUnchanged
ManualAcquisition_AllFiveAuthoredTargetsReveal
MapClue_RevealsSurveyed_NeverVisited_WithClueProvenance
MapClue_RouteDiscoveryStateUntouched
MapClue_UnknownNode_TypedFailure_DiscoveryNotRegistered
KnowledgeUnknownTarget_TypedFailure_DiscoveryNotRegistered
Reacquisition_Idempotent_NoDuplicateDiscoveryEvent
VinylCollectible_DispatcherAppliesNoMorale
```


# Appendix R.596 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleMapRevealIntegrationTests.cs`

### `Ashfall.Core.Tests/Collectibles/CollectibleMapRevealIntegrationTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 206; SHA-256: `b4ff07333481a09cb0efcd9e1c1fb841ea237fe3f532ba3fedfaa4558938786a`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RoadMap_RevealsLogisticsReserveCache_LocationOnly
TopoMap_RevealsHiddenRelayBunker_LocationOnly
SurvivorMap_RevealsDeaddropCommandShelter_LocationOnly
SurvivorMap_RevealsLockedDeaddrop_ExistenceNotEntry
MapReacquisition_Idempotent_NoRouteMutation_NoDuplicateEvent
MapReveal_SaveRestore_LocationKnown_RoutesUntouched_ReacquireNoOp
MapReveal_RaisesNodeKnowledgeChanged_PanelObservable
```


# Appendix R.597 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/DoseCollectibleSaveFuzzTests.cs`

### `Ashfall.Core.Tests/DoseCollectibleSaveFuzzTests.cs`

- Current test declarations: Fact=16, Theory=0, InlineData=0.
- File lines: 374; SHA-256: `b91db5d6125a06b65e37301033d23b19feae9375f4f426487f093ab810620ff5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
DoseFuzz_FullFiveRegisterEnvelope_CleanRoundTrip
DoseFuzz_ChecksumMutation_IsRejectedWithExactError
DoseFuzz_NullChecksum_IsRejected_NotSilentlyAccepted
DoseFuzz_V1Legacy_MigratesWithEmptyQuestSection
DoseFuzz_FutureVersion_IsRejected
DoseFuzz_SerializeTwice_IsByteIdentical
DiscoveryFuzz_CleanRoundTrip_PreservesPartitionsAndLocations
DiscoveryFuzz_SerializeTwice_IsByteIdentical
DiscoveryFuzz_V1LegacyRestore_MarksAllDiscoveredAsAcknowledged
DiscoveryFuzz_NullRestore_IsHonestEmptyState
ClaimsFuzz_CleanRoundTrip_PreservesClaimsAndAvailabilityGate
ClaimsFuzz_SerializeTwice_IsByteIdentical_AndOrdinalSorted
ClaimsFuzz_StaleSaveIds_NoLongerUnique_AreDroppedOnRestore
TutorialFuzz_CleanRoundTrip_PreservesSeenAndQueue
TutorialFuzz_NullRestore_IsHonestEmptyState
ScavengingCatalog_IsPinnedAsDataOnly_NoSaveSurface
```


# Appendix R.598 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleResearchIntegrationTests.cs`

### `Ashfall.Core.Tests/Collectibles/CollectibleResearchIntegrationTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 186; SHA-256: `a60923d56b72a1b1a02aeb1347b5581e4a97ef59a5231af4ddf01b0d11314d38`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
DieselServiceManual_RevealsKnowledge_DoesNotComplete
RadioRepairGuide_RevealsKnowledge_DoesNotComplete
WaterTreatmentHandbook_RevealsKnowledge_DoesNotComplete
AirFilterManual_RevealsKnowledge_DoesNotComplete
DosimeterGuide_RevealsKnowledge_DoesNotComplete
ManualReacquisition_Idempotent_NodeStillNotCompleted
ManualAcquisition_SaveRestore_NodeStillRevealed_ReacquireNoRepeat
ManualReveal_RaisesResearchStateEvent_PanelObservable
```


# Appendix R.599 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Data/RuntimeJsonBootstrapParityTests.cs`

### `Ashfall.Core.Tests/Data/RuntimeJsonBootstrapParityTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 358; SHA-256: `867486fc7d22b5633d6af109fe3c59b5a6e1303cc17f1b6622f585862ca2701f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ItemCatalogLoader_LoadsAllAuthoritativeItemsFromJson
ItemCatalogLoader_StartingSupplies_MatchesAuthoritativeJson
RecipeCatalogLoader_LoadsAllAuthoritativeRecipesFromJson
SurvivorStartingStateLoader_LoadsStartingSurvivorsFromJson
ExpeditionCatalogLoader_LoadsExpeditionsFromJson
RuntimeBootstrap_ChangingJsonDirectlyAltersLiveRuntimeWithoutEditingCSharp
SaveCompatibility_PersistedItemAndSurvivorIdsResolveAgainstJsonCatalogs
```


# Appendix R.600 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/StartingSuppliesProfileTests.cs`

### `Ashfall.Core.Tests/StartingSuppliesProfileTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 327; SHA-256: `04287be82223cfaa57edb089a262537b8b3fd566b5bdb1456cd73313ea84c4b7`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ProductionCatalog_HasSixProfiles_AndExactLegacyDefault
EveryProductionProfile_UsesCanonicalPositiveUniqueItems
LegacyFallback_MatchesTheLegacyProfileExactly
ProfileMetrics_AreDistinctAndNoProfileDominatesAnother
LegacyV1Shape_RemainsReadable
InvalidAlternateProfile_IsSkippedWithoutRemovingStandard
ExplicitUnknownProfile_ResolvesToDefault
```


# Appendix R.601 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/CollectibleMerchantSimulationTests.cs`

### `Ashfall.Core.Tests/CollectibleMerchantSimulationTests.cs`

- Current test declarations: Fact=2, Theory=0, InlineData=0.
- File lines: 235; SHA-256: `1f5f2c291378e528abbc332c4ee14c93f6cbf97a32c5c9b9cc5d89f782ef7824`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Simulation50_CollectibleRevenueUnder20Percent_AndNoSingleDominance
Simulation_25_25_SaveReplay_MatchesUninterrupted50
```


# Appendix R.602 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleBalanceCharacterizationTests.cs`

### `Ashfall.Core.Tests/Collectibles/CollectibleBalanceCharacterizationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 241; SHA-256: `cd44e92eb2d1093b0808c3d16e84de32b9ca869898b8a4810136dc2f14f3d2f7`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
HundredRuns_Deterministic_DistributionValid_NoUniqueDuplicates
Corpus_AnalyticalRatio_MatchesMeasuredWithinTolerance
TradeValueLadder_MediansRiseWithRarity
WeightBands_PlausibleByCategory
```


# Appendix R.603 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleContentUtilizationTests.cs`

### `Ashfall.Core.Tests/Collectibles/CollectibleContentUtilizationTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 204; SHA-256: `9ceec4684e14d98c2b824e3cd74e78a615eb415076dac81b4f169b003475e4c0`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AllCollectibles_HaveLiveItemsAndDefinitions
AllCollectibles_HaveAtLeastOneLiveSource
CollectibleAcquisition_SpansAtLeastFiveDistinctLiveTables
NoSingleSourceExceedsThirtyPercentOfCollectibleWeight
UniqueCollectibles_PhysicallyAwardedAtMostOnce_PerCampaign
MixedSet_SaveRestore_DiscoveryLedgerStable_UniqueSuppressionStable
```


# Appendix R.604 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleTutorialIntegrationTests.cs`

### `Ashfall.Core.Tests/Collectibles/CollectibleTutorialIntegrationTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 227; SHA-256: `1647e3fd2a4ad3bcfa75c580f05e353283fef766e78c4e2d8e838ebc1807f332`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CollectibleTutorialEntry_ExistsAndHasContent
FirstCollectibleDiscovery_TriggersCulturalArtifacts
SubsequentCollectibleDiscovery_DoesNotRetriggerCulturalArtifacts
FirstEffectBearingCollectible_TriggersReadingAndDiscovering
SubsequentEffectBearingCollectible_DoesNotRetriggerReadingAndDiscovering
FirstEffectBearingDiscovery_QueuesTutorialsInStableOrder
CollectibleTutorialSeenState_SurvivesSaveLoad
RestoredDiscovery_DoesNotTriggerTutorialFromHistoricalState
```


# Appendix R.605 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/GreenhouseItemCatalogTests.cs`

### `Ashfall.Core.Tests/GreenhouseItemCatalogTests.cs`

- Current test declarations: Fact=21, Theory=0, InlineData=0.
- File lines: 395; SHA-256: `8d0455261b65808c3cd8fc08c876df318761924dd0e99ccb67581bbf7881ee96`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
GreenhouseFile_ContainsExactlyThirtyEntries
GreenhouseFile_PreservesOriginalFourteen
GreenhouseFile_ContainsAllSixteenNewSupplies
GreenhouseFile_HasUniqueIds
GreenhouseFile_AllTypesAreValidItemTypeValues
GreenhouseFile_NamesAndDescriptionsNonEmpty
GreenhouseFile_NumericRangesValid
GreenhouseFile_HandToolsHaveLowStacksAndLowWeight
GreenhouseFile_NewSuppliesAreNotSameValueClones
GlobalCatalog_RegistersAllThirtyGreenhouseEntries
GlobalCatalog_NewSuppliesResolveAcrossCategories
GlobalCatalog_NoIdCollisionsAcrossItemFiles
GreenhouseFile_DeadParityCopiesRemoved
GreenhouseFile_NewSuppliesClaimNoConsumableEffectFields
Crafting_FourGreenhouseRecipesExistAndAreUnique
Crafting_GreenhouseRecipeOutputsResolveInGlobalRegistry
Crafting_GreenhouseRecipeIngredientsResolve
Crafting_GreenhouseOutputsNotPricedBelowInputValue
Scavenging_GreenhouseTableBindsThreePlan91Items
Scavenging_BoundItemIdsResolveInGlobalRegistry
Scavenging_BoundEntriesUseSaneWeightsAndRarity
```


# Appendix R.606 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Tooling/LoaderWiringGateTests.cs`

### `Ashfall.Core.Tests/Tooling/LoaderWiringGateTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 189; SHA-256: `cf130fbfbbf7ff00c3627c76b4cc6050938a9cfd48e63a1ea3feb0e371b56a66`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
EveryLoadAndRegisterLoader_IsCalledFromProduction_OrAllowlisted
FormerlyAllowlistedLoadFeeders_AreProductionWired
RecentSystemCatalogs_AreBoundFromMainHostPartials
AllowlistEntries_StillExist_AsLoaders
```


# Appendix R.607 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Inventory/Plan22CatalogClassificationParityTests.cs`

### `Ashfall.Core.Tests/Inventory/Plan22CatalogClassificationParityTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 157; SHA-256: `a36b30fea109b8025a945bd50d8af9e43b927dbfd159a254675452513b4cb366`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
EveryProtectiveAndMedicalItem_ClassifiesIdenticallyToRetiredOracle
AuthoredRepairBills_Parse_WithAuthoredCaps
MedicalTags_AreAuthored_OnTheTypeMismatchedItems
AuthoredRepairBillCosts_ResolveToRealItems
RepairBillMaterials_AndCanisters_AreTradeGoods
RepairBillMaterials_ResolveToCatalogItems
```


# Appendix R.608 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/WildlifeTrappingCatalogTests.cs`

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


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.


# Appendix S — Quality Assurance Pass Record: Plan 05

This record is part of the planning artifact, not a fresh runtime test result.

## Pass 1 — content and premise accuracy
- Content pass replaced the stale 1→20 target with the current 30-record census.
- The historical baseline is separated from the current source/data/test authority.
- Current row counts and owner boundaries are stated without using count as a quality proxy.

## Pass 2 — integration architecture
- Integration pass traced catalog → acquisition → morale → radio fact → save.
- Core, data, host, UI, save, event and test seams are named with current paths.
- The plan does not authorize a parallel save section, catalog, manager or host cache.

## Final precision and reaccuracy pass
- Precision pass removed any implication that display metadata owns broadcast effects.
- Every embedded current-file hash, focused runner command and master-authority reference is rechecked.
- Any proposed future seam is labeled as requiring a separate claim and premise verification.
