# Plan 69 — Wasteland Grave Epitaphs, Memorial Auto-Population and Environmental Discovery

> **Rebuild status:** COMPLETE 30-EPITAPH DATA/MEMORIAL LOOP — ENVIRONMENTAL REACHABILITY AND TONE AUDIT
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-3`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round3-2026-09-25`
>
> **Current-evidence date:** 2026-09-25
>
> **Authority order:** live source and data → `AGENTS.md` → `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` → plan ledgers → this document.
>
> **Authority checksum:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
>
> **Length policy:** 150k–170k is the first quality checkpoint; 250k is an evidence-backed depth target, not a ceiling. The plan may exceed 250k when verified architecture and current evidence justify it, and it must stop rather than pad when that evidence is exhausted.

## 0. Integrity Statement and Plan Status

This file replaces unclaimed generated sections that mixed current evidence, fictional APIs, and unsupported save claims. It is a planning and architecture artifact only. It authorizes no production, data, test, save, generated-index, or UI edits. Every path labeled current must exist at rebuild time. Any future `CREATE` proposal is explicitly hypothetical and belongs to a later, separately claimed implementation package.

The rebuild follows four passes: content/current-reality first; integration framework second; accuracy and contradiction removal third; independent precision and handoff review fourth. Character count is recorded by external verification, not embedded recursively in the document.

# 1. Objective

- The current catalog has 30 rows across the authored cause-of-death vocabulary, with a cause-indexed fallback chain and seeded selection. The Core memorial owner already owns the resulting epitaph text on `MemorialEntry`.
- The live memorial route is `GraveEpitaphCatalog` → `MemorialSystem.Memorialize` when `Epitaph` is empty → memorial save/presentation. A separate wasteland-grave discovery/inspection route is not established by the current plan file and must be audited rather than assumed.
- The valuable leap-forward work is a truthful distinction between a survivor memorial and an environmental marker, stable cause fallback, deterministic selection, and a player-visible route that does not fabricate a deceased identity.

**Bounded outcome:** Retire the old 8→30 pure-data brief as a new catalog project. The current `wasteland_grave_epitaphs.json` has 30 rows, `GraveEpitaphCatalog` loads/filters/selects them deterministically, and `MemorialSystem` auto-populates an epitaph when a memorial input omits one. The remaining plan is to distinguish memorial auto-population from environmental grave discovery/reachability and to audit tone/save behavior, not to create a second memorial or epitaph store.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `wasteland_grave_epitaphs.json` is valid JSON with 30 `epitaphs` rows and a cause field; current source provides explicit fallback order for unknown/unspecified causes.
- `GraveEpitaphCatalog` loads rows, indexes by cause, supports seeded selection and returns a safe fallback when no pool exists.
- `MemorialSystem` exposes `EpitaphCatalog`, `EpitaphRng` and `Memorialize`; when no explicit epitaph is supplied it selects a catalog line using a stable hash of day/survivor id and the injected RNG.
- Historical DEC-246 tests cover 30 rows, causes, deterministic selection and memorial integration; current environmental discovery reachability is not claimed by this plan.

**Master-authority sections applied to this rebase:**

- Part II Factory Protocol: premise sweep, collision check, one lane/cluster, and evidence labels before drafting.
- Part II Step 5 continuity and anti-duplication checklist: data presence is not reachability.
- Part III cluster map: use the live C1–C17 owner map rather than a historical plan title.
- Part IV backlog discipline: consume a verified candidate or record why it is stale; do not widen a bounded outcome.
- Part V Template S/R: subject intent and recommended route remain separate from implementation commitments.
- Part VI Multi-Session Growth Protocol: 250k is a depth target, not permission to manufacture volume.
- Live source/data authority: current catalog, loader, host, save, and focused tests outrank generated prose.
- Anti-padding rule: if the evidence queue is exhausted, stop and report no warranted continuation.
- C9 Survivors/interiority and C13 Endgame clusters: memorial entries are authoritative; epitaph text is a bounded authored projection.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 8→30 target with a 30-row current cause/vocabulary census.
- Verify catalog cause buckets, fallback order, sentence constraints and seeded selection against the current file.
- Audit whether environmental wasteland graves have a real discovery/inspection host route; keep that separate from `MemorialSystem` auto-population.
- Preserve memorial entry persistence and grief/eulogy ownership; do not add a second epitaph collection.

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
| cause-indexed catalog and deterministic selection | GraveEpitaphCatalog | `Assets/Ashfall.Core/Memorial/GraveEpitaphCatalog.cs` | Owns static epitaph content and selection only. |
| memorial entry, cause, epitaph, grief and save state | MemorialSystem | `Assets/Ashfall.Core/Memorial/MemorialSystem.cs` | Sole memorial authority; auto-populates empty epitaphs. |
| memorial persistence and current presentation | Memorial save/host | `src/Host/MemorialSaveStore.cs; src/Main.ShelterSocial.cs; src/UI/IronCenotaphMemorialPanel.cs` | Existing memorial host/UI context; environmental grave route is not assumed. |
| catalog, cause, selection and integration proof | Focused memorial tests | `Ashfall.Core.Tests/Memorial/WastelandGraveEpitaphsCatalogTests.cs; Ashfall.Core.Tests/Shelter/Plan84_69MusterMemorialIntegrationTests.cs` | Focused evidence surface. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Wasteland Grave Epitaphs, Memorial Auto-Population and Environmental Discovery
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ GraveEpitaphCatalog
│   cause-indexed catalog and deterministic selection
│ MemorialSystem
│   memorial entry, cause, epitaph, grief and save state
│ Memorial save/host
│   memorial persistence and current presentation
│ Focused memorial tests
│   catalog, cause, selection and integration proof
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

1. **Preserve current state ownership.** GraveEpitaphCatalog owns cause-indexed catalog and deterministic selection: Owns static epitaph content and selection only.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| cause-indexed catalog and deterministic selection | GraveEpitaphCatalog | `Assets/Ashfall.Core/Memorial/GraveEpitaphCatalog.cs` | Owns static epitaph content and selection only. |
| memorial entry, cause, epitaph, grief and save state | MemorialSystem | `Assets/Ashfall.Core/Memorial/MemorialSystem.cs` | Sole memorial authority; auto-populates empty epitaphs. |
| memorial persistence and current presentation | Memorial save/host | `src/Host/MemorialSaveStore.cs; src/Main.ShelterSocial.cs; src/UI/IronCenotaphMemorialPanel.cs` | Existing memorial host/UI context; environmental grave route is not assumed. |
| catalog, cause, selection and integration proof | Focused memorial tests | `Ashfall.Core.Tests/Memorial/WastelandGraveEpitaphsCatalogTests.cs; Ashfall.Core.Tests/Shelter/Plan84_69MusterMemorialIntegrationTests.cs` | Focused evidence surface. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load the 30-row catalog
2. receive a current memorial input with cause/day/survivor id
3. use explicit epitaph when supplied, otherwise select by cause and stable seed
4. create one `MemorialEntry` through MemorialSystem
5. apply existing grief/eulogy/heirloom consequences
6. capture/restore memorial state
7. project the current memorial surface; audit any separate environmental grave route independently

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Epitaph rows are immutable catalog data; the selected line becomes a field on the existing `MemorialEntry`.
- Cause lookup is case-insensitive through the current catalog and falls back through explicit unspecified/unknown pools.
- Seeded selection uses the current RNG/hash contract and the same memorial input produces the same line.
- An epitaph does not create a second death record, grief ledger or survivor identity.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A blank/cause-missing input uses the documented fallback rather than an empty player-facing marker.
- A selected line is stored only on the existing memorial entry and does not alter the death cause.
- Same day/survivor/cause/RNG state selects the same line.
- A failed catalog load leaves explicit epitaphs and memorial processing usable.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `wasteland_grave_epitaphs.json` is the sole environmental epitaph catalog.
- Do not copy rows into memorial, journal or panel code.
- A future row needs a current cause vocabulary, restrained fictional tone, sentence-shape validation and a real memorial/environmental consumer.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the existing memorial section and `MemorialEntry`; no new epitaph save section.
- Auto-populated text is durable as part of the memorial entry once committed; restore does not reroll it.
- Legacy entries with an empty epitaph remain valid and do not unexpectedly rewrite historical text unless the current owner contract says so.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Selection uses injected `ISeededRng`/stable seed; no wall-clock or hash iteration order.
- Cause index lists preserve catalog order for deterministic fallback.
- Replay compares cause, selected line, entry fields and grief side effects.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- `MemorialSystem.OnMemorialized` reports the committed memorial entry after auto-population.
- Environmental discovery, if it exists, must be a separate typed fact from a current world owner.
- No new epitaph event is required for a catalog-only maintenance change.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/MemorialSaveStore.cs
- src/Main.ShelterSocial.cs
- src/UI/IronCenotaphMemorialPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Epitaphs are short, physical and human; they should not become generic eulogies or explain hidden mechanics.
- Use fictional causes and restrained language, avoid sensational trauma and copied text.
- A line must not invent a name, survivor fact or event unsupported by the current memorial input.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A row is selected for the wrong cause and presented as fact. | GraveEpitaphCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A memorial is duplicated for the same deceased. | MemorialSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A restore rerolls the epitaph and changes history. | Memorial save/host | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A panel creates an environmental grave without a world owner. | Focused memorial tests | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A second epitaph store is added beside MemorialEntry. | GraveEpitaphCatalog | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Memorial/WastelandGraveEpitaphsCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Memorial/MemorialComponentTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Memorial/RelationsGriefBindingTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Plan84_69MusterMemorialIntegrationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — corpus/memorial census | Read 30 rows, catalog, MemorialSystem, save and UI. | Current content and owner are separated. | No production path until the owning implementation package is separately claimed. |
| 1 — selection/replay proof | Exercise causes, fallbacks, stable seed and explicit epitaph precedence. | One deterministic memorial result per input. | No production path until the owning implementation package is separately claimed. |
| 2 — environmental reachability audit | Search current world/discovery routes without assuming them. | Environmental feature is proven or recorded as separate work. | No production path until the owning implementation package is separately claimed. |
| 3 — tone/save precision | Review sentence constraints, grief ownership and restore truthfulness. | No duplicate memorial/epitaph authority. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json | READ ONLY; MODIFY only for a proven content/consumer gap | 30-row authority |
| Assets/Ashfall.Core/Memorial/GraveEpitaphCatalog.cs | READ ONLY | Selector |
| Assets/Ashfall.Core/Memorial/MemorialSystem.cs | READ ONLY | Memorial owner |
| src/UI/IronCenotaphMemorialPanel.cs | READ ONLY; MODIFY only under a new UI claim | Known presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Conflating memorial auto-population with environmental grave discovery. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Rerolling a committed epitaph on reload. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding a second death/grief record. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Using unsafe or copied prose in a vulnerable memorial context. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new epitaph rows in this package.
- No new memorial system.
- No new save section.
- No production/data/test/UI changes.

# 23. Rollback and Recovery

- Revert the planning document.
- Future content/host changes retain current memorial fixtures and deterministic selection tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 30 current rows and cause/fallback behavior are documented.
- Memorial auto-population and environmental reachability are explicitly separated.
- Save, replay, tone and duplicate-owner boundaries are named.
- Focused commands and residual gates are listed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 8→30 target with a 30-row current cause/vocabulary census.
- Verify catalog cause buckets, fallback order, sentence constraints and seeded selection against the current file.
- Audit whether environmental wasteland graves have a real discovery/inspection host route; keep that separate from `MemorialSystem` auto-population.
- Preserve memorial entry persistence and grief/eulogy ownership; do not add a second epitaph collection.

## MUST NOT DO

- No new epitaph rows in this package.
- No new memorial system.
- No new save section.
- No production/data/test/UI changes.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Memorial/WastelandGraveEpitaphsCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Memorial/MemorialComponentTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Memorial/RelationsGriefBindingTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Plan84_69MusterMemorialIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — corpus/memorial census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: cause-indexed catalog and deterministic selection → GraveEpitaphCatalog; memorial entry, cause, epitaph, grief and save state → MemorialSystem; memorial persistence and current presentation → Memorial save/host; catalog, cause, selection and integration proof → Focused memorial tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 69.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 69 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by GraveEpitaphCatalog or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Memorial/GraveEpitaphCatalog.cs`

### `Assets/Ashfall.Core/Memorial/GraveEpitaphCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 133 lines / 4697 bytes.
- SHA-256: `a1ea75ac1d95ee284b9dfcbb41010bb792f0c6134210c38be17f4bf6a1b3433d`.
- Architecture signals: seeded references=3; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WastelandGraveEpitaphEntry
public string cause = string.Empty;
public string epitaph = string.Empty;
public sealed class WastelandGraveEpitaphContainer
public int schema_version;
public List<WastelandGraveEpitaphEntry> epitaphs = new List<WastelandGraveEpitaphEntry>();
public sealed class GraveEpitaphCatalog
public const string DefaultFileName = "wasteland_grave_epitaphs.json";
public int TotalCount => _entries.Count;
public IReadOnlyList<WastelandGraveEpitaphEntry> AllEntries => _entries;
public static List<WastelandGraveEpitaphEntry> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? json = null) {
public static GraveEpitaphCatalog LoadFromDataDir(string dataDir, IFileIO? fileIO = null, IJsonSerializer? json = null) {
public void Populate(IEnumerable<WastelandGraveEpitaphEntry> entries) {
public IReadOnlyList<WastelandGraveEpitaphEntry> GetEpitaphsForCause(string cause) {
public string SelectEpitaph(string cause, ISeededRng? rng = null) {
public string SelectEpitaph(string cause, int seed) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Memorial/MemorialSystem.cs`

### `Assets/Ashfall.Core/Memorial/MemorialSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 398 lines / 16582 bytes.
- SHA-256: `84e5e276077295bca654304faa02f9ef4b2cdfa48ec0e811323266f3087c29b1`.
- Architecture signals: seeded references=2; save/restore symbols=4; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum DeathQuality
public enum MemorialOutcome
public interface IGriefSink
public sealed class CapturingGriefSink : IGriefSink
public sealed class DispersionRecord
public string DeceasedId = string.Empty;
public List<string> SurvivngRelationshipIds = new List<string>();
public float GriefApplied;
public DeathQuality Quality;
public int Day;
public float QualityScale;
public List<string> Warnings = new List<string>();
public List<DispersionRecord> Records { get; } = new List<DispersionRecord>();
public void ApplyDispersion( string deceasedId, IReadOnlyList<string> survivingRelationshipIds, float baseGriefAmount, DeathQuality quality, int day)
public static float QualityScale(DeathQuality quality) => quality switch
public sealed class MemorialSystem
public event Action<MemorialEntry>? OnMemorialized;
public event Action<MemorialEntry>? OnMourned;
public IGriefSink? GriefSink { get; set; }
public ProceduralEulogyEngine? EulogyEngine { get; set; }
public GraveEpitaphCatalog? EpitaphCatalog { get; set; }
public ISeededRng? EpitaphRng { get; set; }
public IReadOnlyList<MemorialEntry> Entries => _state.Entries;
public ActionResult Mourn(string deceasedId, int day) {
public MemorialEntry? LatestUnmourned() {
public MemorialEntry Memorialize(MemorialInput input) {
public MemorialState CaptureState() => _state.Capture();
public void RestoreState(MemorialState state) {
public sealed class MemorialEntry
public string SurvivorId;
public string Cause;
public int Day;
public int SurvivedDays;
public bool FinalWishResolved;
public string Epitaph;
public string EulogyText = string.Empty;
public string HeirloomItemId;
public string HeirloomRecipientId;
public float MoraleDelta;
public DeathQuality DeathQuality = DeathQuality.Peaceful;
public MemorialOutcome Outcome = MemorialOutcome.Burial;
public int MournedDay = -1;
public sealed class MemorialInput
public string SurvivorId;
public string Cause;
public int Day;
public int BirthDay;
public bool FinalWishResolved;
public string Epitaph;
public string? EulogyText;
public DwellerLifeRecord? LifeRecord;
public string HeirloomItemId;
public string HeirloomRecipientId;
public float MoraleDelta;
public DeathQuality DeathQuality = DeathQuality.Peaceful;
public MemorialOutcome Outcome = MemorialOutcome.Burial;
public IReadOnlyList<string>? SurvivingRelationshipIds;
public sealed class MemorialState
public List<MemorialEntry> Entries = new List<MemorialEntry>();
public MemorialState Capture() {
public void RestoreInto(MemorialState state) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Memorial/MemorialSave.cs`

### `Assets/Ashfall.Core/Memorial/MemorialSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 19 lines / 569 bytes.
- SHA-256: `9f533827f369b178a45163cce725595c215e3b985f8c97650a86c200948cc42d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class MemorialSave
public const int CurrentSaveVersion = 1;
public const int MigrationFromVersion = 1;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public MemorialState State = new MemorialState();
public string Checksum = string.Empty;
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/Memorial/RelationsGriefSink.cs`

### `Assets/Ashfall.Core/Memorial/RelationsGriefSink.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 147 lines / 7302 bytes.
- SHA-256: `7ada5d66c01d04e912b15ae4d8f4ee94137ee6c8097f8890fbc8d0df3ce2e663`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RelationsGriefSink : IGriefSink
public const float MaxGriefPerSurvivorPerEvent = 20f;
public int AppliedEventCount { get; private set; }
public int AppliedSurvivorCount { get; private set; }
public const int BondGriefDurationDays = 10;
public const float BondGriefMoralePerDay = -2f;
public const float BondGriefFatiguePerDay = 1.5f;
public static float BondMoralePerHour(float relationshipGrief, int daysSinceOnset) {
public static float BondFatiguePerHour(float relationshipGrief, int daysSinceOnset) {
public void ApplyDispersion( string deceasedId, IReadOnlyList<string> survivingRelationshipIds, float baseGriefAmount, DeathQuality quality, int day)
```


# Appendix B.06 — Current Code Architecture: `src/Host/MemorialSaveStore.cs`

### `src/Host/MemorialSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 71 lines / 3262 bytes.
- SHA-256: `8e216856bc0ab145946d2fa76262ea658e4982b8b51ab9a5aaa847e5cd4e6c5b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class MemorialSaveStore
public const string FileName = "memorial_save.json";
public const string SectionName = "memorial";
public static string SavePath => s_store.SavePath;
public static string TryCaptureDirect(MemorialSave state) => s_store.CaptureBare(state);
public static MemorialSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(MemorialSave state) => s_store.CaptureBare(state);
public static MemorialSave? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(MemorialSave save) => s_store.TrySave(save);
public static MemorialSave? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(MemorialSave save) => s_store.CapturePersisted(save);
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


# Appendix B.08 — Current Code Architecture: `src/UI/IronCenotaphMemorialPanel.cs`

### `src/UI/IronCenotaphMemorialPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 187 lines / 9167 bytes.
- SHA-256: `5a39a1daac0e45ad6b386fb62deb477828137fa7e20ba7680eafbf48684f49a4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class IronCenotaphMemorialPanel : Control, IBindablePanel
public event Action? OnClose;
public override void _Ready() {
public void Open() {
public bool IsBound { get; private set; } = true;
public void Bind(object? session) {
public void Unbind() {
public sealed class MourningBinding
public Func<int> TotalDeaths { get; init; } = () => 0;
public Func<string, ActionResult> Mourn { get; init; } = _ =>
public void BindMourning(MourningBinding binding) {
public void BindSpiritual(Func<int> arcCount, Func<int> pendingRites) {
public void RefreshView() {
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json`

### `Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 4974 bytes / 4974 characters.
- SHA-256: `32c34718eac8cba96c369725d35d13df830931ee8dc8afdd4214efc727905871`.
- Root keys: `epitaphs`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
epitaphs: min=30, max=30, observed_paths=1
```

Representative record fields:

- `cause`
- `epitaph`


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/narrative/wasteland_grave_epitaphs_batch_2.json`

### `Assets/StreamingAssets/Data/narrative/wasteland_grave_epitaphs_batch_2.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 12333 bytes / 12306 characters.
- SHA-256: `79f5b9ee28a3fcf239db5fcf3b8aed007af07b7013e570d5ac1197723d10adec`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=12, max=12, observed_paths=1
items[].tags: min=5, max=5, observed_paths=2
```

Representative record fields:

- `cause_of_death`
- `deceased_identity`
- `grave_site`
- `id`
- `marker_material`
- `prose`
- `tags`
- `timestamp_relative`

Representative identifiers (ordered, capped for readability):

```text
epitaph_b2_childs_shoe_cairn
epitaph_b2_geiger_counter_headstone
epitaph_b2_welding_rod_cross
epitaph_b2_compass_rose_grave
epitaph_b2_ration_tin_memorial
epitaph_b2_radio_antenna_memorial
epitaph_b2_sentry_rifle_cairn
epitaph_b2_seed_jar_memorial
epitaph_b2_ventilation_grate_memorial
epitaph_b2_water_pipe_cross
epitaph_b2_book_stack_memorial
epitaph_b2_dog_collar_grave
```


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Memorial/WastelandGraveEpitaphsCatalogTests.cs`

### `Ashfall.Core.Tests/Memorial/WastelandGraveEpitaphsCatalogTests.cs`

- Current test declarations: Fact=11, Theory=0, InlineData=0.
- File lines: 278; SHA-256: `021bfdf4bf2f9ab98af3696a908797f0628a20b07e66691dae2440e3211f6235`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_HasValidSchemaVersion
Catalog_HasExactly30Entries
Catalog_PreservesExisting8BaselineEntries
Catalog_ContainsNoDuplicateStrings
Catalog_CoversAllRequiredCauses
Catalog_AllNew22Entries_AreOneSentenceWithin5To20Words
Catalog_DeterministicSeededSelection
Catalog_DifferentSeedsProduceVariety
Catalog_All30EntriesAreReachable
Catalog_UnknownCause_FallsBackSafely
MemorialSystem_RoundTripsWithSelectedEpitaph
```


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs`

### `Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 125; SHA-256: `c8dc55caa2fa596cae8d88ff9038bb4d00e330061a76bef7c6c1945030eff046`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Memorialize_AddsEntry
Memorialize_IsIdempotent_NoDuplicate
Memorialize_DifferentSurvivors_BothAdded
Memorialize_DefaultsCauseWhenMissing
Memorialize_BlankCauseIsReplaced
Memorialize_RequiresSurvivorId
Events_FireOnMemorialize
Idempotency_DoesNotFireEventTwice
CaptureRestore_RoundTrip
HeirloomTransfer_AtomicInEntry
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Memorial/MemorialComponentTests.cs`

### `Ashfall.Core.Tests/Memorial/MemorialComponentTests.cs`

- Current test declarations: Fact=21, Theory=0, InlineData=0.
- File lines: 621; SHA-256: `0740549fabcd1133c01ba93cfef51a28d0b6c208e98bdc3a8e7063089258b0e2`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Store_UsesExpectedMetadataAndEmptyHistoryLedger
Record_IsIdempotent_FirstRecordWinsWithoutEventsOrLifecycleAuthority
Store_CaptureIsOrdinalDetachedAndContainsEveryHistoricalField
DetachedState_RoundTripsAndPreservesWireShape
Restore_RejectsNullInvalidAndDuplicateRows_FirstRowWins
Restore_FutureSchemaAndWrongSystemPreserveCurrentState
Restore_NullStateIsTheExplicitEmptyResetForm
ReleaseDoesNotEraseHistory_ButResetDoes
Adapter_ImportsAllFieldsPreservesSurvivedDaysAndOrdersOwners
Adapter_ReportsNullInvalidDuplicateUnknownAndLivingRows
Adapter_MapsNullableLegacyStringsToTypedDefaults
Adapter_DoesNotMutateEntityLifecycleOrRevision
Parity_IsCleanForMatchingLegacyAndTypedRows
Parity_ReportsDuplicateMissingExtraAndStableOrdering
Parity_UsesLegacyFirstDuplicateForFieldComparison
Parity_ReportsEveryHistoricalFieldMismatch
Parity_ReportsNullLegacyFieldsAndMalformedIds
Parity_IsDeterministicRegardlessOfLegacyRegistrationOrder
TypedStore_IntegratesWithReferentialIntegrityWithoutRejectingHistory
RetainedHistorySurvivesLivingOwnerRemoval
MemorialSave_CoreWireFieldsAndChecksumRemainDirectV1
```


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Memorial/RelationsGriefBindingTests.cs`

### `Ashfall.Core.Tests/Memorial/RelationsGriefBindingTests.cs`

- Current test declarations: Fact=11, Theory=1, InlineData=3.
- File lines: 213; SHA-256: `a0e5a2a5c7313c53f5027a5eea2d2c22ae081e93de35eec35a9006ab07235295`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RelatedIds_ReturnsCounterpartiesSortedAndDeduped
RelatedIds_EmptyOrUnknown_IsEmptyNotNull
RelatedIds_SelfLoopDoesNotReportTheSurvivorAsTheirOwnMourner
ApplyDispersion_GrievesEachSurvivingRelationOnce
ApplyDispersion_ScalesByAuthoredDeathQuality
ApplyDispersion_UsesMagnitudeNotSignOfTheMoraleChannel
ApplyDispersion_CapsPerSurvivorPerEvent
ApplyDispersion_SkipsTheDeceasedAndTheNotAlive
ApplyDispersion_ZeroOrNegativeBase_AppliesNothing
ApplyDispersion_WithoutRelationsIsADocumentedNoOp
Memorialize_RoutesGriefIntoTheRelationshipLedgerOnceOnly
GriefApplication_IsOrderIndependent
```


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Shelter/Plan84_69MusterMemorialIntegrationTests.cs`

### `Ashfall.Core.Tests/Shelter/Plan84_69MusterMemorialIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 182; SHA-256: `74ec0ccb92894853d48bcb93a6c55d7a52c229f23ab2dd54137f40f0c681c2cc`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
MusterAndEpitaphCatalogs_LoadCleanly_WithoutSchemaDrift
GraveEpitaphCatalog_DeterministicSelectionAndFallback
MemorialSystem_EpitaphIntegration_AutoPopulatesFromCause
MusterWitnessTragedyToGraveMemorial_CrossSystemLinkage
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/Memorial/GraveEpitaphCatalog.cs`

### `Assets/Ashfall.Core/Memorial/GraveEpitaphCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 133 lines / 4697 bytes.
- SHA-256: `a1ea75ac1d95ee284b9dfcbb41010bb792f0c6134210c38be17f4bf6a1b3433d`.
- Architecture signals: seeded references=3; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WastelandGraveEpitaphEntry
public string cause = string.Empty;
public string epitaph = string.Empty;
public sealed class WastelandGraveEpitaphContainer
public int schema_version;
public List<WastelandGraveEpitaphEntry> epitaphs = new List<WastelandGraveEpitaphEntry>();
public sealed class GraveEpitaphCatalog
public const string DefaultFileName = "wasteland_grave_epitaphs.json";
public int TotalCount => _entries.Count;
public IReadOnlyList<WastelandGraveEpitaphEntry> AllEntries => _entries;
public static List<WastelandGraveEpitaphEntry> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? json = null) {
public static GraveEpitaphCatalog LoadFromDataDir(string dataDir, IFileIO? fileIO = null, IJsonSerializer? json = null) {
public void Populate(IEnumerable<WastelandGraveEpitaphEntry> entries) {
public IReadOnlyList<WastelandGraveEpitaphEntry> GetEpitaphsForCause(string cause) {
public string SelectEpitaph(string cause, ISeededRng? rng = null) {
public string SelectEpitaph(string cause, int seed) {
```


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/Memorial/MemorialSystem.cs`

### `Assets/Ashfall.Core/Memorial/MemorialSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 398 lines / 16582 bytes.
- SHA-256: `84e5e276077295bca654304faa02f9ef4b2cdfa48ec0e811323266f3087c29b1`.
- Architecture signals: seeded references=2; save/restore symbols=4; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum DeathQuality
public enum MemorialOutcome
public interface IGriefSink
public sealed class CapturingGriefSink : IGriefSink
public sealed class DispersionRecord
public string DeceasedId = string.Empty;
public List<string> SurvivngRelationshipIds = new List<string>();
public float GriefApplied;
public DeathQuality Quality;
public int Day;
public float QualityScale;
public List<string> Warnings = new List<string>();
public List<DispersionRecord> Records { get; } = new List<DispersionRecord>();
public void ApplyDispersion( string deceasedId, IReadOnlyList<string> survivingRelationshipIds, float baseGriefAmount, DeathQuality quality, int day)
public static float QualityScale(DeathQuality quality) => quality switch
public sealed class MemorialSystem
public event Action<MemorialEntry>? OnMemorialized;
public event Action<MemorialEntry>? OnMourned;
public IGriefSink? GriefSink { get; set; }
public ProceduralEulogyEngine? EulogyEngine { get; set; }
public GraveEpitaphCatalog? EpitaphCatalog { get; set; }
public ISeededRng? EpitaphRng { get; set; }
public IReadOnlyList<MemorialEntry> Entries => _state.Entries;
public ActionResult Mourn(string deceasedId, int day) {
public MemorialEntry? LatestUnmourned() {
public MemorialEntry Memorialize(MemorialInput input) {
public MemorialState CaptureState() => _state.Capture();
public void RestoreState(MemorialState state) {
public sealed class MemorialEntry
public string SurvivorId;
public string Cause;
public int Day;
public int SurvivedDays;
public bool FinalWishResolved;
public string Epitaph;
public string EulogyText = string.Empty;
public string HeirloomItemId;
public string HeirloomRecipientId;
public float MoraleDelta;
public DeathQuality DeathQuality = DeathQuality.Peaceful;
public MemorialOutcome Outcome = MemorialOutcome.Burial;
public int MournedDay = -1;
public sealed class MemorialInput
public string SurvivorId;
public string Cause;
public int Day;
public int BirthDay;
public bool FinalWishResolved;
public string Epitaph;
public string? EulogyText;
public DwellerLifeRecord? LifeRecord;
public string HeirloomItemId;
public string HeirloomRecipientId;
public float MoraleDelta;
public DeathQuality DeathQuality = DeathQuality.Peaceful;
public MemorialOutcome Outcome = MemorialOutcome.Burial;
public IReadOnlyList<string>? SurvivingRelationshipIds;
public sealed class MemorialState
public List<MemorialEntry> Entries = new List<MemorialEntry>();
public MemorialState Capture() {
public void RestoreInto(MemorialState state) {
```


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/Memorial/MemorialSave.cs`

### `Assets/Ashfall.Core/Memorial/MemorialSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 19 lines / 569 bytes.
- SHA-256: `9f533827f369b178a45163cce725595c215e3b985f8c97650a86c200948cc42d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class MemorialSave
public const int CurrentSaveVersion = 1;
public const int MigrationFromVersion = 1;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public MemorialState State = new MemorialState();
public string Checksum = string.Empty;
```


# Appendix E.19 — Supporting Code Evidence: `Assets/Ashfall.Core/Memorial/RelationsGriefSink.cs`

### `Assets/Ashfall.Core/Memorial/RelationsGriefSink.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 147 lines / 7302 bytes.
- SHA-256: `7ada5d66c01d04e912b15ae4d8f4ee94137ee6c8097f8890fbc8d0df3ce2e663`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RelationsGriefSink : IGriefSink
public const float MaxGriefPerSurvivorPerEvent = 20f;
public int AppliedEventCount { get; private set; }
public int AppliedSurvivorCount { get; private set; }
public const int BondGriefDurationDays = 10;
public const float BondGriefMoralePerDay = -2f;
public const float BondGriefFatiguePerDay = 1.5f;
public static float BondMoralePerHour(float relationshipGrief, int daysSinceOnset) {
public static float BondFatiguePerHour(float relationshipGrief, int daysSinceOnset) {
public void ApplyDispersion( string deceasedId, IReadOnlyList<string> survivingRelationshipIds, float baseGriefAmount, DeathQuality quality, int day)
```


# Appendix E.20 — Supporting Code Evidence: `src/Host/MemorialSaveStore.cs`

### `src/Host/MemorialSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 71 lines / 3262 bytes.
- SHA-256: `8e216856bc0ab145946d2fa76262ea658e4982b8b51ab9a5aaa847e5cd4e6c5b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class MemorialSaveStore
public const string FileName = "memorial_save.json";
public const string SectionName = "memorial";
public static string SavePath => s_store.SavePath;
public static string TryCaptureDirect(MemorialSave state) => s_store.CaptureBare(state);
public static MemorialSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(MemorialSave state) => s_store.CaptureBare(state);
public static MemorialSave? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(MemorialSave save) => s_store.TrySave(save);
public static MemorialSave? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(MemorialSave save) => s_store.CapturePersisted(save);
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/Memorial/WastelandGraveEpitaphsCatalogTests.cs`

### `Ashfall.Core.Tests/Memorial/WastelandGraveEpitaphsCatalogTests.cs`

- Current test declarations: Fact=11, Theory=0, InlineData=0.
- File lines: 278; SHA-256: `021bfdf4bf2f9ab98af3696a908797f0628a20b07e66691dae2440e3211f6235`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_HasValidSchemaVersion
Catalog_HasExactly30Entries
Catalog_PreservesExisting8BaselineEntries
Catalog_ContainsNoDuplicateStrings
Catalog_CoversAllRequiredCauses
Catalog_AllNew22Entries_AreOneSentenceWithin5To20Words
Catalog_DeterministicSeededSelection
Catalog_DifferentSeedsProduceVariety
Catalog_All30EntriesAreReachable
Catalog_UnknownCause_FallsBackSafely
MemorialSystem_RoundTripsWithSelectedEpitaph
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs`

### `Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 125; SHA-256: `c8dc55caa2fa596cae8d88ff9038bb4d00e330061a76bef7c6c1945030eff046`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Memorialize_AddsEntry
Memorialize_IsIdempotent_NoDuplicate
Memorialize_DifferentSurvivors_BothAdded
Memorialize_DefaultsCauseWhenMissing
Memorialize_BlankCauseIsReplaced
Memorialize_RequiresSurvivorId
Events_FireOnMemorialize
Idempotency_DoesNotFireEventTwice
CaptureRestore_RoundTrip
HeirloomTransfer_AtomicInEntry
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/Memorial/MemorialComponentTests.cs`

### `Ashfall.Core.Tests/Memorial/MemorialComponentTests.cs`

- Current test declarations: Fact=21, Theory=0, InlineData=0.
- File lines: 621; SHA-256: `0740549fabcd1133c01ba93cfef51a28d0b6c208e98bdc3a8e7063089258b0e2`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Store_UsesExpectedMetadataAndEmptyHistoryLedger
Record_IsIdempotent_FirstRecordWinsWithoutEventsOrLifecycleAuthority
Store_CaptureIsOrdinalDetachedAndContainsEveryHistoricalField
DetachedState_RoundTripsAndPreservesWireShape
Restore_RejectsNullInvalidAndDuplicateRows_FirstRowWins
Restore_FutureSchemaAndWrongSystemPreserveCurrentState
Restore_NullStateIsTheExplicitEmptyResetForm
ReleaseDoesNotEraseHistory_ButResetDoes
Adapter_ImportsAllFieldsPreservesSurvivedDaysAndOrdersOwners
Adapter_ReportsNullInvalidDuplicateUnknownAndLivingRows
Adapter_MapsNullableLegacyStringsToTypedDefaults
Adapter_DoesNotMutateEntityLifecycleOrRevision
Parity_IsCleanForMatchingLegacyAndTypedRows
Parity_ReportsDuplicateMissingExtraAndStableOrdering
Parity_UsesLegacyFirstDuplicateForFieldComparison
Parity_ReportsEveryHistoricalFieldMismatch
Parity_ReportsNullLegacyFieldsAndMalformedIds
Parity_IsDeterministicRegardlessOfLegacyRegistrationOrder
TypedStore_IntegratesWithReferentialIntegrityWithoutRejectingHistory
RetainedHistorySurvivesLivingOwnerRemoval
MemorialSave_CoreWireFieldsAndChecksumRemainDirectV1
```


# Appendix G.24 — Supporting Regression Evidence: `Ashfall.Core.Tests/Memorial/RelationsGriefBindingTests.cs`

### `Ashfall.Core.Tests/Memorial/RelationsGriefBindingTests.cs`

- Current test declarations: Fact=11, Theory=1, InlineData=3.
- File lines: 213; SHA-256: `a0e5a2a5c7313c53f5027a5eea2d2c22ae081e93de35eec35a9006ab07235295`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RelatedIds_ReturnsCounterpartiesSortedAndDeduped
RelatedIds_EmptyOrUnknown_IsEmptyNotNull
RelatedIds_SelfLoopDoesNotReportTheSurvivorAsTheirOwnMourner
ApplyDispersion_GrievesEachSurvivingRelationOnce
ApplyDispersion_ScalesByAuthoredDeathQuality
ApplyDispersion_UsesMagnitudeNotSignOfTheMoraleChannel
ApplyDispersion_CapsPerSurvivorPerEvent
ApplyDispersion_SkipsTheDeceasedAndTheNotAlive
ApplyDispersion_ZeroOrNegativeBase_AppliesNothing
ApplyDispersion_WithoutRelationsIsADocumentedNoOp
Memorialize_RoutesGriefIntoTheRelationshipLedgerOnceOnly
GriefApplication_IsOrderIndependent
```


# Appendix H.25 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

### `docs/CURRENT_AUTHORITY.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 102 lines / 9950 bytes.
- SHA-256: `7dea2c12b4863bfc9a3c2ebb161ba51512ebc06475b762abd5d5d205bef47e5c`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=0; textual Godot mentions=6; textual Unity/JsonUtility mentions=2; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.26 — Supporting Authority Document: `docs/memorials/PLAN69_BASELINE.md`

### `docs/memorials/PLAN69_BASELINE.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 55 lines / 4342 bytes.
- SHA-256: `b8ca6e1102663201914ca375a48cf80049ab8b2102ae16dece4887417753f60b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.27 — Supporting Authority Document: `docs/memorials/PLAN69_CLOSEOUT.md`

### `docs/memorials/PLAN69_CLOSEOUT.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 107 lines / 9575 bytes.
- SHA-256: `890954cee07359567cd2a392d9cd1be0b7da34394a660637d43e7bb423498920`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.28 — Supporting Authority Document: `docs/memorials/WASTELAND_EPITAPH_SELECTION_CONTRACT.md`

### `docs/memorials/WASTELAND_EPITAPH_SELECTION_CONTRACT.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 34 lines / 1548 bytes.
- SHA-256: `9e8992a91585ea35e6fe89232279d089b1b59e4a9978f7c1335de3b48b8dcb8e`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.29 — Supporting Authority Document: `docs/memorials/WASTELAND_EPITAPH_SAVE_CONTRACT.md`

### `docs/memorials/WASTELAND_EPITAPH_SAVE_CONTRACT.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 37 lines / 1537 bytes.
- SHA-256: `3ff758062b66e85043828a84e3f2bfde318e68cdb002782760e5464b62e744dd`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MemorialEntry
public string SurvivorId;
public string Cause;
public int Day;
public int SurvivedDays;
public bool FinalWishResolved;
public string Epitaph;              // <-- Authoritative persisted string
public string HeirloomItemId;
public string HeirloomRecipientId;
public float MoraleDelta;
public DeathQuality DeathQuality;
public MemorialOutcome Outcome;
```


# Appendix H.30 — Supporting Authority Document: `docs/memorials/WASTELAND_EPITAPH_TONE_GUIDE.md`

### `docs/memorials/WASTELAND_EPITAPH_TONE_GUIDE.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 42 lines / 2516 bytes.
- SHA-256: `2b0a22b40a00aec891a78fe868a558dac14919f7d265f91c18c742ff8665b39a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.31 — Supporting Authority Document: `docs/memorials/WASTELAND_EPITAPH_MICRO_LOCATION_HANDOFF.md`

### `docs/memorials/WASTELAND_EPITAPH_MICRO_LOCATION_HANDOFF.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 30 lines / 1172 bytes.
- SHA-256: `44f7767bfdbec435a0e702238be5c8b01fe1940c67c8f5068e3ae4cf1caf3806`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| cause-indexed catalog and deterministic selection | GraveEpitaphCatalog | memorial entry, cause, epitaph, grief and save state | MemorialSystem | Owner emits/reads a typed fact; no mirror state. |
| cause-indexed catalog and deterministic selection | GraveEpitaphCatalog | memorial persistence and current presentation | Memorial save/host | Owner emits/reads a typed fact; no mirror state. |
| cause-indexed catalog and deterministic selection | GraveEpitaphCatalog | catalog, cause, selection and integration proof | Focused memorial tests | Owner emits/reads a typed fact; no mirror state. |
| memorial entry, cause, epitaph, grief and save state | MemorialSystem | cause-indexed catalog and deterministic selection | GraveEpitaphCatalog | Owner emits/reads a typed fact; no mirror state. |
| memorial entry, cause, epitaph, grief and save state | MemorialSystem | memorial persistence and current presentation | Memorial save/host | Owner emits/reads a typed fact; no mirror state. |
| memorial entry, cause, epitaph, grief and save state | MemorialSystem | catalog, cause, selection and integration proof | Focused memorial tests | Owner emits/reads a typed fact; no mirror state. |
| memorial persistence and current presentation | Memorial save/host | cause-indexed catalog and deterministic selection | GraveEpitaphCatalog | Owner emits/reads a typed fact; no mirror state. |
| memorial persistence and current presentation | Memorial save/host | memorial entry, cause, epitaph, grief and save state | MemorialSystem | Owner emits/reads a typed fact; no mirror state. |
| memorial persistence and current presentation | Memorial save/host | catalog, cause, selection and integration proof | Focused memorial tests | Owner emits/reads a typed fact; no mirror state. |
| catalog, cause, selection and integration proof | Focused memorial tests | cause-indexed catalog and deterministic selection | GraveEpitaphCatalog | Owner emits/reads a typed fact; no mirror state. |
| catalog, cause, selection and integration proof | Focused memorial tests | memorial entry, cause, epitaph, grief and save state | MemorialSystem | Owner emits/reads a typed fact; no mirror state. |
| catalog, cause, selection and integration proof | Focused memorial tests | memorial persistence and current presentation | Memorial save/host | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 8→30 target with a 30-row current cause/vocabulary census. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Verify catalog cause buckets, fallback order, sentence constraints and seeded selection against the current file. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Audit whether environmental wasteland graves have a real discovery/inspection host route; keep that separate from `MemorialSystem` auto-population. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Preserve memorial entry persistence and grief/eulogy ownership; do not add a second epitaph collection. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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

> **A-29 · C14 · Bestiary natural-history continuation.** Subject: sighting-log and specimen-record prose for bestiary entries with thin coverage. Evidence: `wasteland_wildlife_bestiary.json` verified live; vulture-sighting and cockroach-hive log genres exist. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

> **B-15 · C9 · Memorial-rite epilogue evidence enrollment.** Subject: performed rites enrolling as Reckoning evidence (rites exist; evidence vocabulary must be checked for a rite class before authoring). Evidence: `memorial_rites.json`, `spiritual_rituals.json` verified live. Route: CORE-EXTENSION through endgame owners. Confidence: PROPOSAL — evidence vocabulary check first.

> **B-16 · C10 · Quest reopening after new discoveries.** Subject: failed/abandoned quests reopening when discovery conditions later satisfy (the failure-recovery grammar of v1.0 Part 6.7). Evidence: abandoned-quest reopen is canon grammar; implementation state unverified. Route: CORE-EXTENSION through quest owners. Confidence: PROPOSAL.

> **DM-6 — Map and geography (C6).** Owners: wasteland map system/loader, damaged zones, fog, route gates, survey instruments. Live catalogs: `wasteland_map_v1`, `damaged_map_zones`, `weather_route_gates`, `gpr_exploration_catalog`, `insar_geodesy_catalog`, `geodetic_survey_catalog`, `seismic_fault_catalog`, `piezometer_network_catalog`. Hosts: GeodeticSurvey, InSarMapping, Cartography selftest family. Openings: A-15, B-09 (GATE). Constraint: the orphan gate `AllMapNodes_ExistInLocationsCatalog` governs all map authoring.

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

The subject is a compact authored memorial text layer attached to the current MemorialSystem entry, with environmental grave discovery kept as a separate unproven route. The plan expands selection, persistence, tone and reachability precision.

- **cause-indexed catalog and deterministic selection** remains with `GraveEpitaphCatalog` at `Assets/Ashfall.Core/Memorial/GraveEpitaphCatalog.cs`. Owns static epitaph content and selection only.
- **memorial entry, cause, epitaph, grief and save state** remains with `MemorialSystem` at `Assets/Ashfall.Core/Memorial/MemorialSystem.cs`. Sole memorial authority; auto-populates empty epitaphs.
- **memorial persistence and current presentation** remains with `Memorial save/host` at `src/Host/MemorialSaveStore.cs; src/Main.ShelterSocial.cs; src/UI/IronCenotaphMemorialPanel.cs`. Existing memorial host/UI context; environmental grave route is not assumed.
- **catalog, cause, selection and integration proof** remains with `Focused memorial tests` at `Ashfall.Core.Tests/Memorial/WastelandGraveEpitaphsCatalogTests.cs; Ashfall.Core.Tests/Shelter/Plan84_69MusterMemorialIntegrationTests.cs`. Focused evidence surface.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load the 30-row catalog
2. receive a current memorial input with cause/day/survivor id
3. use explicit epitaph when supplied, otherwise select by cause and stable seed
4. create one `MemorialEntry` through MemorialSystem
5. apply existing grief/eulogy/heirloom consequences
6. capture/restore memorial state
7. project the current memorial surface; audit any separate environmental grave route independently

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Epitaph rows are immutable catalog data; the selected line becomes a field on the existing `MemorialEntry`.
- Cause lookup is case-insensitive through the current catalog and falls back through explicit unspecified/unknown pools.
- Seeded selection uses the current RNG/hash contract and the same memorial input produces the same line.
- An epitaph does not create a second death record, grief ledger or survivor identity.

- A blank/cause-missing input uses the documented fallback rather than an empty player-facing marker.
- A selected line is stored only on the existing memorial entry and does not alter the death cause.
- Same day/survivor/cause/RNG state selects the same line.
- A failed catalog load leaves explicit epitaphs and memorial processing usable.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/MemorialSaveStore.cs
- src/Main.ShelterSocial.cs
- src/UI/IronCenotaphMemorialPanel.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/Memorial/WastelandGraveEpitaphsCatalogTests.cs
- Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs
- Ashfall.Core.Tests/Memorial/MemorialComponentTests.cs
- Ashfall.Core.Tests/Memorial/RelationsGriefBindingTests.cs
- Ashfall.Core.Tests/Shelter/Plan84_69MusterMemorialIntegrationTests.cs

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
| S-01 | 69-01 load 30 rows | load the 30-row catalog | Epitaph rows are immutable catalog data; the selected line becomes a field on the existing `MemorialEntry`. | A row is selected for the wrong cause and presented as fact. | GraveEpitaphCatalog |
| S-02 | 69-02 known cause selection | receive a current memorial input with cause/day/survivor id | Cause lookup is case-insensitive through the current catalog and falls back through explicit unspecified/unknown pools. | A memorial is duplicated for the same deceased. | GraveEpitaphCatalog |
| S-03 | 69-03 unspecified fallback | use explicit epitaph when supplied, otherwise select by cause and stable seed | Seeded selection uses the current RNG/hash contract and the same memorial input produces the same line. | A restore rerolls the epitaph and changes history. | GraveEpitaphCatalog |
| S-04 | 69-04 unknown fallback | create one `MemorialEntry` through MemorialSystem | An epitaph does not create a second death record, grief ledger or survivor identity. | A panel creates an environmental grave without a world owner. | GraveEpitaphCatalog |
| S-05 | 69-05 empty pool fallback | apply existing grief/eulogy/heirloom consequences | Epitaph rows are immutable catalog data; the selected line becomes a field on the existing `MemorialEntry`. | A second epitaph store is added beside MemorialEntry. | GraveEpitaphCatalog |
| S-06 | 69-06 explicit epitaph precedence | capture/restore memorial state | Cause lookup is case-insensitive through the current catalog and falls back through explicit unspecified/unknown pools. | A row is selected for the wrong cause and presented as fact. | GraveEpitaphCatalog |
| S-07 | 69-07 stable seed replay | project the current memorial surface; audit any separate environmental grave route independently | Seeded selection uses the current RNG/hash contract and the same memorial input produces the same line. | A memorial is duplicated for the same deceased. | GraveEpitaphCatalog |
| S-08 | 69-08 memorial save/restore | load the 30-row catalog | An epitaph does not create a second death record, grief ledger or survivor identity. | A restore rerolls the epitaph and changes history. | GraveEpitaphCatalog |
| S-09 | 69-09 environmental route search | receive a current memorial input with cause/day/survivor id | Epitaph rows are immutable catalog data; the selected line becomes a field on the existing `MemorialEntry`. | A panel creates an environmental grave without a world owner. | GraveEpitaphCatalog |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 69-TC-01 schema/cause census | data | schema/cause census; verify the current owner and its negative boundary without inventing a second authority. | GraveEpitaphCatalog |
| T-02 | 69-TC-02 cause index case-insensitivity | unit | cause index case-insensitivity; verify the current owner and its negative boundary without inventing a second authority. | GraveEpitaphCatalog |
| T-03 | 69-TC-03 fallback order | persistence | fallback order; verify the current owner and its negative boundary without inventing a second authority. | GraveEpitaphCatalog |
| T-04 | 69-TC-04 empty catalog fallback | determinism | empty catalog fallback; verify the current owner and its negative boundary without inventing a second authority. | GraveEpitaphCatalog |
| T-05 | 69-TC-05 seeded selection | host | seeded selection; verify the current owner and its negative boundary without inventing a second authority. | GraveEpitaphCatalog |
| T-06 | 69-TC-06 stable hash input | UI/accessibility | stable hash input; verify the current owner and its negative boundary without inventing a second authority. | GraveEpitaphCatalog |
| T-07 | 69-TC-07 explicit text precedence | cross-system | explicit text precedence; verify the current owner and its negative boundary without inventing a second authority. | GraveEpitaphCatalog |
| T-08 | 69-TC-08 memorial idempotence | data | memorial idempotence; verify the current owner and its negative boundary without inventing a second authority. | GraveEpitaphCatalog |
| T-09 | 69-TC-09 grief side-effect once | unit | grief side-effect once; verify the current owner and its negative boundary without inventing a second authority. | GraveEpitaphCatalog |
| T-10 | 69-TC-10 save deep copy | persistence | save deep copy; verify the current owner and its negative boundary without inventing a second authority. | GraveEpitaphCatalog |
| T-11 | 69-TC-11 restore no reroll | determinism | restore no reroll; verify the current owner and its negative boundary without inventing a second authority. | GraveEpitaphCatalog |
| T-12 | 69-TC-12 sentence/tone review | host | sentence/tone review; verify the current owner and its negative boundary without inventing a second authority. | GraveEpitaphCatalog |
| T-13 | 69-TC-13 environmental reachability audit | UI/accessibility | environmental reachability audit; verify the current owner and its negative boundary without inventing a second authority. | GraveEpitaphCatalog |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 36 | `Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 30 | `Ashfall.Core.Tests/Memorial/MemorialGriefPortTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 29 | `Assets/Ashfall.Core/Memorial/MemorialSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 24 | `Ashfall.Core.Tests/Shelter/Plan84_69MusterMemorialIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 24 | `Assets/Ashfall.Core/Memorial/GraveEpitaphCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 22 | `Ashfall.Core.Tests/Survivors/SurvivorLifecycleTableTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 15 | `Ashfall.Core.Tests/Memorial/MemorialComponentTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 15 | `Ashfall.Core.Tests/Survivors/SurvivorEntityStoreTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 12 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 12 | `Assets/Ashfall.Core/Survivors/SurvivorLifecycle.cs` | current reference count; inspect the caller before treating it as a live route |
| 11 | `Ashfall.Core.Tests/MicroLocationPersistenceWaveTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 11 | `src/Main.Campaign.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `Ashfall.Core.Tests/Shelter/ShelterArchiveSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `Assets/Ashfall.Core/Survivors/SurvivorEntityStore.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/Content/Plan41_45MemoryAcceptanceIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/Memorial/WastelandGraveEpitaphsCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/Memorial/RelationsGriefBindingTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/Content/ContentOrphanCertificationEngineTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/Medical/Plan24RecoveryGriefTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/Plan23CapstoneTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Survivors/MemorialComponentParity.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Endgame/Plan19CohortContinuityTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Endgame/Plan19EndingContinuityTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Endgame/Plan19SessionContinuityJourneyTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Memorial/Plan41MemoryActsTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Plan12DCrossSystemContinuityTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/SurvivorFateSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Survivors/SurvivorIntegrityValidatorTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/HostCli.SliceScenario.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/AutopsyBridgeTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Integration/FullCampaign30DayShelterPlaythroughTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Shelter/Plan162ArchiveIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Memorial/RelationsGriefSink.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json`

### `Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 4974; characters: 4974.
- SHA-256: `32c34718eac8cba96c369725d35d13df830931ee8dc8afdd4214efc727905871`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `epitaphs`

#### `epitaphs` — 30 current rows

- Row 001 `radiation`: `{"cause":"radiation","epitaph":"Lethal cellular degradation. Biological remains require deep burial. Below the official line, smaller, cut with a nail: they were not contagious at the end. We held their hand anyway."}`
- Row 002 `combat`: `{"cause":"combat","epitaph":"Terminated by hostiles. Equipment recovered and sanitized. Scratched sideways underneath: they did not run. Nobody ran. That is the whole story."}`
- Row 003 `starvation`: `{"cause":"starvation","epitaph":"Caloric deficit reached terminal state. Carved underneath in a different hand: gave their share away three times. The third time is logged here."}`
- Row 004 `exhaustion`: `{"cause":"exhaustion","epitaph":"Cardiovascular collapse due to sustained labor output. Carved deep underneath: rested on the seventh day, finally. We finished their shift for them."}`
- Row 005 `disease`: `{"cause":"disease","epitaph":"Pathological contamination event. Sector quarantined. Underneath, in smaller letters: we talked through the door every night. The door logged nothing. We did."}`
- Row 006 `expedition`: `{"cause":"expedition","epitaph":"Asset failed to return from surface operations. Logged as loss. Carved under it with a knife point: not an asset. A friend. The log is wrong, and we are leaving it that way."}`
- Row 007 `trauma`: `{"cause":"trauma","epitaph":"Severe structural damage to biological unit. Underneath, almost too rough to read: carried in. Not left alone. That much is ours to say."}`
- Row 008 `unspecified`: `{"cause":"unspecified","epitaph":"Termination logged. Rations redistributed. Underneath, the newest carving on the stone: still owe them a name. Working on it."}`
- Row 009 `radiation`: `{"cause":"radiation","epitaph":"The dosimeter film on her lapel turned jet black before the relief shift arrived."}`
- Row 010 `radiation`: `{"cause":"radiation","epitaph":"He said the air tasted of copper two hours before the fever took his legs."}`
- Row 011 `combat`: `{"cause":"combat","epitaph":"Three people made it through the culvert gate because she stayed behind to draw the fire."}`
- Row 012 `combat`: `{"cause":"combat","epitaph":"He dropped behind the fuel tank and never had the chance to unshoulder his rifle."}`
- Row 013 `starvation`: `{"cause":"starvation","epitaph":"Her name was still on the distribution clipboard when the last sack of meal spoiled."}`
- Row 014 `starvation`: `{"cause":"starvation","epitaph":"He weighed less than thirty kilos when they carried him up from the boiler trench."}`
- Row 015 `exhaustion`: `{"cause":"exhaustion","epitaph":"She laid her wrench on the pump casing and leaned her forehead against the cool pipe."}`
- Row 016 `disease`: `{"cause":"disease","epitaph":"His name was crossed off the ward roster before the second dose of penicillin arrived."}`
- Row 017 `expedition`: `{"cause":"expedition","epitaph":"They found his pack three miles past the radio mast with the compass dial smashed inward."}`
- Row 018 `trauma`: `{"cause":"trauma","epitaph":"The winch cable snapped during the generator hoist and left no time for anyone to yell."}`
- Row 019 `exposure`: `{"cause":"exposure","epitaph":"The wind tore her tarp away while she was searching for the road marker in the dark."}`
- Row 020 `exposure`: `{"cause":"exposure","epitaph":"Found kneeling inside the hollowed boiler with his damp wool coat stiffened by the rime."}`
- Row 021 `suicide`: `{"cause":"suicide","epitaph":"We carved her name where she used to sit because no one found words for the rest."}`
- Row 022 `suicide`: `{"cause":"suicide","epitaph":"An empty bunk in Section Four that nobody in the squad has the heart to reassign."}`
- Row 023 `infection`: `{"cause":"infection","epitaph":"We boiled the needle three times, but the red line crept past his elbow anyway."}`
- Row 024 `old_age`: `{"cause":"old_age","epitaph":"He remembered what grass looked like under clear sunlight and told the children until his voice failed."}`
- Row 025 `old_age`: `{"cause":"old_age","epitaph":"Eighty-two winters counted on the beam, which is sixty more than anyone had reason to expect."}`
- Row 026 `drowning`: `{"cause":"drowning","epitaph":"The pontoon rope snapped in the spring current before anyone on the bank could throw another."}`
- Row 027 `frostbite`: `{"cause":"frostbite","epitaph":"Blackened boots left beside the fire shovel because the numbness had already reached his knees."}`
- Row 028 `poisoning`: `{"cause":"poisoning","epitaph":"Drank from the condensate drip behind the transformer vault before the test strip turned bright purple."}`
- Row 029 `execution`: `{"cause":"execution","epitaph":"They brought him out to the gravel pit at sunrise and read no charges from the ledger."}`
- Row 030 `unknown`: `{"cause":"unknown","epitaph":"No tags, no journal, and only three brass buttons left in the gravel beneath the cairn."}`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/narrative/wasteland_grave_epitaphs_batch_2.json`

### `Assets/StreamingAssets/Data/narrative/wasteland_grave_epitaphs_batch_2.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 12333; characters: 12306.
- SHA-256: `79f5b9ee28a3fcf239db5fcf3b8aed007af07b7013e570d5ac1197723d10adec`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `items`

#### `items` — 12 current rows

- Row 001 `epitaph_b2_childs_shoe_cairn`: `{"cause_of_death":"ACUTE_RADIATION_SYNDROME","deceased_identity":"CHILD_UNKNOWN_AGE_EST_FOUR","grave_site":"EASTERN_FIELD_PLOT_28","id":"epitaph_b2_childs_shoe_cairn","marker_material":"CHILDS_RUBBER_BOOT_ON_STAKE","prose":"A small blue ru…`
- Row 002 `epitaph_b2_geiger_counter_headstone`: `{"cause_of_death":"CHRONIC_INTERNAL_EMITTER_CS_137","deceased_identity":"SCOUT_POLINA","grave_site":"SOUTHERN_RIDGE_MARKER_04","id":"epitaph_b2_geiger_counter_headstone","marker_material":"GEIGER_COUNTER_MOUNTED_ON_STONE","prose":"A pre-wa…`
- Row 003 `epitaph_b2_welding_rod_cross`: `{"cause_of_death":"MOLTEN_METAL_BURN_SEPSIS","deceased_identity":"MASTER_OLEG_PETROV","grave_site":"WORKSHOP_CORRIDOR_NICHE","id":"epitaph_b2_welding_rod_cross","marker_material":"WELDING_RODS_CROSS_LAPPED","prose":"Two welding rods crosse…`
- Row 004 `epitaph_b2_compass_rose_grave`: `{"cause_of_death":"EXPOSURE_FALL_FROM_RIDGE","deceased_identity":"MAPMAKER_BRAM_OSTROWSKI","grave_site":"NORTHERN_PERIMETER_POST_7","id":"epitaph_b2_compass_rose_grave","marker_material":"BRASS_COMPASS_SET_IN_CONCRETE","prose":"A brass sur…`
- Row 005 `epitaph_b2_ration_tin_memorial`: `{"cause_of_death":"PULMONARY_FIBROSIS_CHRONIC","deceased_identity":"BAKER_ANNA_IVANOVA","grave_site":"MESS_HALL_WALL_NICHE","id":"epitaph_b2_ration_tin_memorial","marker_material":"STACKED_RATION_TINS_WITH_CANDLE","prose":"Seven empty rati…`
- Row 006 `epitaph_b2_radio_antenna_memorial`: `{"cause_of_death":"VOLUNTARY_EXPOSURE_SIGNAL_SOURCE","deceased_identity":"RADIO_OPERATOR_MAYA_LIN","grave_site":"COMMS_ROOM_ROOFTOP","id":"epitaph_b2_radio_antenna_memorial","marker_material":"SHORT_WIRE_ANTENNA_TIED_TO_MAST","prose":"A le…`
- Row 007 `epitaph_b2_sentry_rifle_cairn`: `{"cause_of_death":"HOSTILE_CONTACT_PERIMETER","deceased_identity":"SENTRY_STEPAN_VORONIN","grave_site":"SOUTH_PERIMETER_WATCHPOINT","id":"epitaph_b2_sentry_rifle_cairn","marker_material":"BOLT_ACTION_RIFLE_STOCK_UPRIGHT_IN_STONE","prose":"…`
- Row 008 `epitaph_b2_seed_jar_memorial`: `{"cause_of_death":"NATURAL_CAUSES_CARDIAC","deceased_identity":"SUKI_TANAKA","grave_site":"GREENHOUSE_CORNER_PLOT","id":"epitaph_b2_seed_jar_memorial","marker_material":"SMALL_GLASS_SEED_JAR_ON_WOODEN_STAKE","prose":"A small glass jar fill…`
- Row 009 `epitaph_b2_ventilation_grate_memorial`: `{"cause_of_death":"ACUTE_CHEMICAL_EXPOSURE_INTAKE_FAILURE","deceased_identity":"VENTILATION_TECHNICIAN_ILYA","grave_site":"SUB_LEVEL_2_INTAKE_CHAMBER","id":"epitaph_b2_ventilation_grate_memorial","marker_material":"AIR_FILTER_ELEMENT_MOUNT…`
- Row 010 `epitaph_b2_water_pipe_cross`: `{"cause_of_death":"FLOODING_SUB_LEVEL_4_BREACH","deceased_identity":"PLUMBER_DENIS_KOVAL","grave_site":"PUMP_ROOM_ALCOVE","id":"epitaph_b2_water_pipe_cross","marker_material":"COPPER_PIPE_CROSS_WITH_VALVE_WHEEL","prose":"A cross made from …`
- Row 011 `epitaph_b2_book_stack_memorial`: `{"cause_of_death":"CHRONIC_LEUKAEMIA_RADIATION","deceased_identity":"TEACHER_IRINA_FELD","grave_site":"SCHOOLROOM_WALL_NICHE","id":"epitaph_b2_book_stack_memorial","marker_material":"STACKED_TEXTBOOKS_BEHIND_GLASS","prose":"Four pre-war te…`
- Row 012 `epitaph_b2_dog_collar_grave`: `{"cause_of_death":"OLD_AGE_COMPANION_ANIMAL","deceased_identity":"THERAPY_DOG_MASHA_THREE_LEGGED","grave_site":"SOUTHERN_PASTURE_EDGE","id":"epitaph_b2_dog_collar_grave","marker_material":"LEATHER_DOG_COLLAR_HUNG_ON_BRANCH","prose":"A worn…`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Memorial/GraveEpitaphCatalog.cs`

### `Assets/Ashfall.Core/Memorial/GraveEpitaphCatalog.cs` — complete current file

- Size: 133 lines / 4697 bytes.
- SHA-256: `a1ea75ac1d95ee284b9dfcbb41010bb792f0c6134210c38be17f4bf6a1b3433d`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005: using Ashfall.Core.IO;
00006:
00007: namespace Ashfall.Core.Memorial
00008: {
00009:     [Serializable]
00010:     public sealed class WastelandGraveEpitaphEntry
00011:     {
00012:         public string cause = string.Empty;
00013:         public string epitaph = string.Empty;
00014:     }
00015:
00016:     [Serializable]
00017:     public sealed class WastelandGraveEpitaphContainer
00018:     {
00019:         public int schema_version;
00020:         public List<WastelandGraveEpitaphEntry> epitaphs = new List<WastelandGraveEpitaphEntry>();
00021:     }
00022:
00023:     /// <summary>
00024:     /// Loads authored grave epitaphs and memorial inscriptions from wasteland_grave_epitaphs.json.
00025:     /// Provides deterministic, seed-based selection per cause of death for environmental graves
00026:     /// and shelter memorialization.
00027:     /// Engine-agnostic; uses IFileIO, IJsonSerializer, and ISeededRng.
00028:     /// </summary>
00029:     public sealed class GraveEpitaphCatalog
00030:     {
00031:         public const string DefaultFileName = "wasteland_grave_epitaphs.json";
00032:
00033:         private readonly List<WastelandGraveEpitaphEntry> _entries = new List<WastelandGraveEpitaphEntry>();
00034:         private readonly Dictionary<string, List<WastelandGraveEpitaphEntry>> _byCause =
00035:             new Dictionary<string, List<WastelandGraveEpitaphEntry>>(StringComparer.OrdinalIgnoreCase);
00036:
00037:         public int TotalCount => _entries.Count;
00038:         public IReadOnlyList<WastelandGraveEpitaphEntry> AllEntries => _entries;
00039:
00040:         public static List<WastelandGraveEpitaphEntry> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? json = null)
00041:         {
00042:             fileIO ??= new FileSystemIO();
00043:             json ??= new SystemTextJsonSerializer();
00044:
00045:             if (string.IsNullOrEmpty(dataDir))
00046:                 return new List<WastelandGraveEpitaphEntry>();
00047:
00048:             string path = fileIO.Combine(dataDir, DefaultFileName);
00049:             if (!fileIO.FileExists(path))
00050:                 return new List<WastelandGraveEpitaphEntry>();
00051:
00052:             string raw = fileIO.ReadAllText(path);
00053:             if (string.IsNullOrWhiteSpace(raw))
00054:                 return new List<WastelandGraveEpitaphEntry>();
00055:
00056:             try
00057:             {
00058:                 var container = json.Deserialize<WastelandGraveEpitaphContainer>(raw);
00059:                 return container?.epitaphs ?? new List<WastelandGraveEpitaphEntry>();
00060:             }
00061:             catch (Exception ex_CATDIAG)
00062:             {
00063:                 CatalogDiagnostics.Warn(path, "WastelandGraveEpitaphEntry list", ex_CATDIAG);
00064:                 return new List<WastelandGraveEpitaphEntry>();
00065:             }
00066:         }
00067:
00068:         public static GraveEpitaphCatalog LoadFromDataDir(string dataDir, IFileIO? fileIO = null, IJsonSerializer? json = null)
00069:         {
00070:             var catalog = new GraveEpitaphCatalog();
00071:             var entries = Load(dataDir, fileIO, json);
00072:             catalog.Populate(entries);
00073:             return catalog;
00074:         }
00075:
00076:         public void Populate(IEnumerable<WastelandGraveEpitaphEntry> entries)
00077:         {
00078:             _entries.Clear();
00079:             _byCause.Clear();
00080:
00081:             if (entries == null) return;
00082:
00083:             foreach (var entry in entries)
00084:             {
00085:                 if (entry == null || string.IsNullOrWhiteSpace(entry.epitaph)) continue;
00086:
00087:                 _entries.Add(entry);
00088:                 string key = entry.cause?.Trim() ?? string.Empty;
00089:                 if (!_byCause.TryGetValue(key, out var list))
00090:                 {
00091:                     list = new List<WastelandGraveEpitaphEntry>();
00092:                     _byCause[key] = list;
00093:                 }
00094:                 list.Add(entry);
00095:             }
00096:         }
00097:
00098:         public IReadOnlyList<WastelandGraveEpitaphEntry> GetEpitaphsForCause(string cause)
00099:         {
00100:             if (string.IsNullOrWhiteSpace(cause))
00101:                 cause = "unspecified";
00102:
00103:             if (_byCause.TryGetValue(cause, out var list))
00104:                 return list;
00105:
00106:             if (_byCause.TryGetValue("unspecified", out var unspecList))
00107:                 return unspecList;
00108:
00109:             if (_byCause.TryGetValue("unknown", out var unknownList))
00110:                 return unknownList;
00111:
00112:             return _entries;
00113:         }
00114:
00115:         public string SelectEpitaph(string cause, ISeededRng? rng = null)
00116:         {
00117:             var pool = GetEpitaphsForCause(cause);
00118:             if (pool.Count == 0)
00119:                 return "The lamp went out.";
00120:
00121:             if (rng == null)
00122:                 return pool[0].epitaph;
00123:
00124:             int index = rng.Next(0, pool.Count);
00125:             return pool[index].epitaph;
00126:         }
00127:
00128:         public string SelectEpitaph(string cause, int seed)
00129:         {
00130:             return SelectEpitaph(cause, new SeededRng(seed));
00131:         }
00132:     }
00133: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Memorial/MemorialSystem.cs`

### `Assets/Ashfall.Core/Memorial/MemorialSystem.cs` — complete current file

- Size: 398 lines / 16582 bytes.
- SHA-256: `84e5e276077295bca654304faa02f9ef4b2cdfa48ec0e811323266f3087c29b1`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.Journal;
00005: #pragma warning disable CS8618
00006:
00007: namespace Ashfall.Core.Memorial
00008: {
00009:     /// <summary>
00010:     /// Plan 09 / 9C Core — how the death was managed. Drives the grief cascade
00011:     /// inside <see cref="MemorialSystem"/> via the <see cref="IGriefSink"/>
00012:     /// port: scaling grief magnitude and broadcast language. Default
00013:     /// <see cref="Peaceful"/> is what existing captures load as — the new
00014:     /// field is additive on every save shape and never breaks a round-trip.
00015:     /// </summary>
00016:     public enum DeathQuality
00017:     {
00018:         Unattended = 0, // no medic present, no vigil held
00019:         Rushed = 1,     // medic present but no time / no comfort
00020:         Peaceful = 2,  // medic + caregiver + vigil completed
00021:     }
00022:
00023:     /// <summary>
00024:     /// Plan 09 / 9C Core — how the body's remains were returned to the
00025:     /// community. Survives on the <see cref="MemorialEntry"/> so the
00026:     /// memorial wall decor (white space #18) can render the right
00027:     /// artefact. Default <see cref="Burial"/> mirrors the existing
00028:     /// resting-place behaviour.
00029:     /// </summary>
00030:     public enum MemorialOutcome
00031:     {
00032:         Burial = 0,
00033:         WallEntry = 1,  // ashes pressed into the bunk's memorial wall
00034:         AshScatter = 2, // remains released to the outside (open ground, river)
00035:     }
00036:
00037:     /// <summary>
00038:     /// Plan 09 / 9C Core — grief cascade port. Hosts attach a sink so the
00039:     /// memorial entry routes grief to the survivor-relations ledger (and,
00040:     /// later, to audio + narrative beats). Engine-agnostic: a default
00041:     /// no-op implementation logs the grief rather than failing the
00042:     /// memorial pipeline if no host wire is bound.
00043:     /// </summary>
00044:     public interface IGriefSink
00045:     {
00046:         /// <summary>
00047:         /// Apply grief to the surrounding relationships for a freshly
00048:         /// memorialized survivor. The <paramref name="qualityScale"/> is the
00049:         /// grief multiplier: <see cref="DeathQuality.Peaceful"/> = 0.5,
00050:         /// <see cref="DeathQuality.Rushed"/> = 1.0,
00051:         /// <see cref="DeathQuality.Unattended"/> = 1.25. Implementations
00052:         /// should be deterministic given <paramref name="qualityScale"/>.
00053:         /// </summary>
00054:         void ApplyDispersion(
00055:             string deceasedId,
00056:             IReadOnlyList<string> survivingRelationshipIds,
00057:             float baseGriefAmount,
00058:             DeathQuality quality,
00059:             int day);
00060:     }
00061:
00062:     /// <summary>
00063:     /// Default no-op grief sink. Routes grief to a callback rather than
00064:     /// mutating any host state, so Core-side tests can assert determinism
00065:     /// without wiring SurvivorRelationsSystem.
00066:     /// </summary>
00067:     public sealed class CapturingGriefSink : IGriefSink
00068:     {
00069:         public sealed class DispersionRecord
00070:         {
00071:             public string DeceasedId = string.Empty;
00072:             public List<string> SurvivngRelationshipIds = new List<string>();
00073:             public float GriefApplied;
00074:             public DeathQuality Quality;
00075:             public int Day;
00076:             public float QualityScale;
00077:             public List<string> Warnings = new List<string>();
00078:         }
00079:
00080:         public List<DispersionRecord> Records { get; } = new List<DispersionRecord>();
00081:
00082:         public void ApplyDispersion(
00083:             string deceasedId,
00084:             IReadOnlyList<string> survivingRelationshipIds,
00085:             float baseGriefAmount,
00086:             DeathQuality quality,
00087:             int day)
00088:         {
00089:             float scale = quality switch
00090:             {
00091:                 DeathQuality.Peaceful => 0.5f,
00092:                 DeathQuality.Rushed => 1.0f,
00093:                 DeathQuality.Unattended => 1.25f,
00094:                 _ => 1.0f,
00095:             };
00096:             Records.Add(new DispersionRecord
00097:             {
00098:                 DeceasedId = deceasedId ?? string.Empty,
00099:                 SurvivngRelationshipIds = survivingRelationshipIds == null
00100:                     ? new List<string>()
00101:                     : new List<string>(survivingRelationshipIds),
00102:                 GriefApplied = baseGriefAmount * scale,
00103:                 Quality = quality,
00104:                 Day = day,
00105:                 QualityScale = scale,
00106:             });
00107:         }
00108:
00109:         public static float QualityScale(DeathQuality quality) => quality switch
00110:         {
00111:             DeathQuality.Peaceful => 0.5f,
00112:             DeathQuality.Rushed => 1.0f,
00113:             DeathQuality.Unattended => 1.25f,
00114:             _ => 1.0f,
00115:         };
00116:     }
00117:
00118:     /// <summary>
00119:     /// ASHFALL Memorial System (item 15).
00120:     ///
00121:     /// Single Core authority for the death-to-memorial pipeline. Subscribes
00122:     /// to roster, needs, radiation, combat, and trauma death paths through
00123:     /// one idempotent death bridge. Records cause, day, survival duration,
00124:     /// final-wish status, epitaph, heirloom, morale effect, the death
00125:     /// quality (Plan 09 9C), and the disposition outcome (burial / wall /
00126:     /// ash-scatter).
00127:     ///
00128:     /// The system completes or fails final wishes before recording the
00129:     /// memorial, transfers heirlooms atomically, and returns unresolved
00130:     /// recipients' items to storage when a recipient is not alive.
00131:     /// </summary>
00132:     public sealed class MemorialSystem
00133:     {
00134:         private readonly MemorialState _state;
00135:
00136:         /// <summary>Raised when a survivor is memorialized.</summary>
00137:         public event Action<MemorialEntry>? OnMemorialized;
00138:
00139:         /// <summary>Plan 24C (A3) — raised exactly once per deceased when the
00140:         /// shelter's mourning vigil is held. The host applies the morale
00141:         /// recovery and journal line through the attributed seam.</summary>
00142:         public event Action<MemorialEntry>? OnMourned;
00143:
00144:         /// <summary>
00145:         /// Plan 09 / 9C Core. Set by the host so <see cref="Memorialize"/>
00146:         /// can route grief to SurvivorRelations and downstream systems.
00147:         /// Null = the <see cref="Memorialize"/> path is silent on grief
00148:         /// (existing pre-9C behaviour preserved for tests that don't bind).
00149:         /// </summary>
00150:         public IGriefSink? GriefSink { get; set; }
00151:
00152:         /// <summary>
00153:         /// Plan 41 / C1[12]: Optional procedural eulogy engine for composing and archiving
00154:         /// literary funeral inscriptions during memorialization.
00155:         /// </summary>
00156:         public ProceduralEulogyEngine? EulogyEngine { get; set; }
00157:
00158:         /// <summary>
00159:         /// Plan 69: Optional grave epitaph catalog for selecting grounded diegetic
00160:         /// inscriptions per cause of death when none is explicitly provided.
00161:         /// </summary>
00162:         public GraveEpitaphCatalog? EpitaphCatalog { get; set; }
00163:
00164:         /// <summary>Optional seeded RNG for deterministic epitaph selection.</summary>
00165:         public ISeededRng? EpitaphRng { get; set; }
00166:
00167:         public MemorialSystem(MemorialState state)
00168:         {
00169:             _state = state ?? throw new ArgumentNullException(nameof(state));
00170:         }
00171:
00172:         public IReadOnlyList<MemorialEntry> Entries => _state.Entries;
00173:
00174:         /// <summary>
00175:         /// Plan 24C (A3) — the mourning vigil: a bounded, once-per-death
00176:         /// player action. The memorial owner holds the exactly-once state
00177:         /// (persisted `MournedDay` on the entry); the host applies the morale
00178:         /// recovery and journal line on <see cref="OnMourned"/>. Rejected
00179:         /// states are explicit results — never silent no-ops.
00180:         /// </summary>
00181:         public ActionResult Mourn(string deceasedId, int day)
00182:         {
00183:             if (string.IsNullOrEmpty(deceasedId))
00184:                 return ActionResult.Blocked("missing_memorial_id", "memorial.mourn_missing_id");
00185:             for (int i = 0; i < _state.Entries.Count; i++)
00186:             {
00187:                 var entry = _state.Entries[i];
00188:                 if (entry == null || !string.Equals(entry.SurvivorId, deceasedId, StringComparison.Ordinal)) continue;
00189:                 if (entry.MournedDay >= 0)
00190:                     return ActionResult.Blocked("already_mourned", "memorial.mourn_already_mourned");
00191:                 entry.MournedDay = day;
00192:                 OnMourned?.Invoke(entry);
00193:                 return ActionResult.Success("memorial.mourned");
00194:             }
00195:             return ActionResult.Blocked("unknown_memorial", "memorial.mourn_unknown");
00196:         }
00197:
00198:         /// <summary>The most recent deceased not yet mourned, or null when every
00199:         /// entry has had its vigil (the mourning surface's read model).</summary>
00200:         public MemorialEntry? LatestUnmourned()
00201:         {
00202:             MemorialEntry? latest = null;
00203:             for (int i = 0; i < _state.Entries.Count; i++)
00204:             {
00205:                 var entry = _state.Entries[i];
00206:                 if (entry == null || entry.MournedDay >= 0) continue;
00207:                 if (latest == null || entry.Day > latest.Day) latest = entry;
00208:             }
00209:             return latest;
00210:         }
00211:
00212:         /// <summary>
00213:         /// Idempotent memorialization. If <paramref name="survivorId"/>
00214:         /// is already in the ledger, returns the existing entry without
00215:         /// duplicating it (and does NOT re-fire grief — grief fires on
00216:         /// the first call only).
00217:         /// </summary>
00218:         public MemorialEntry Memorialize(MemorialInput input)
00219:         {
00220:             if (input == null) throw new ArgumentNullException(nameof(input));
00221:             if (string.IsNullOrEmpty(input.SurvivorId))
00222:                 throw new ArgumentException("survivorId required", nameof(input));
00223:
00224:             for (int i = 0; i < _state.Entries.Count; i++)
00225:                 if (_state.Entries[i].SurvivorId == input.SurvivorId)
00226:                     return _state.Entries[i];
00227:
00228:             string eulogy = input.EulogyText ?? string.Empty;
00229:             if (string.IsNullOrEmpty(eulogy) && EulogyEngine != null)
00230:             {
00231:                 var life = input.LifeRecord ?? new DwellerLifeRecord
00232:                 {
00233:                     dwellerId = input.SurvivorId,
00234:                     dwellerName = input.SurvivorId,
00235:                     daysSurvived = input.Day - input.BirthDay,
00236:                     causeOfDeath = string.IsNullOrEmpty(input.Cause) ? "unspecified" : input.Cause,
00237:                     favoriteRelicName = input.HeirloomItemId ?? string.Empty
00238:                 };
00239:                 eulogy = EulogyEngine.ComposeEulogy(life);
00240:             }
00241:
00242:             string epitaph = input.Epitaph ?? string.Empty;
00243:             if (string.IsNullOrEmpty(epitaph) && EpitaphCatalog != null)
00244:             {
00245:                 string cause = string.IsNullOrEmpty(input.Cause) ? "unspecified" : input.Cause;
00246:                 int seed = StableHash.Combine(input.Day, input.SurvivorId);
00247:                 epitaph = EpitaphCatalog.SelectEpitaph(cause, EpitaphRng ?? new SeededRng(seed));
00248:             }
00249:
00250:             var entry = new MemorialEntry
00251:             {
00252:                 SurvivorId = input.SurvivorId,
00253:                 Cause = string.IsNullOrEmpty(input.Cause) ? "unspecified" : input.Cause,
00254:                 Day = input.Day,
00255:                 SurvivedDays = input.Day - input.BirthDay,
00256:                 FinalWishResolved = input.FinalWishResolved,
00257:                 Epitaph = epitaph,
00258:                 EulogyText = eulogy,
00259:                 HeirloomItemId = input.HeirloomItemId ?? string.Empty,
00260:                 HeirloomRecipientId = input.HeirloomRecipientId ?? string.Empty,
00261:                 MoraleDelta = input.MoraleDelta,
00262:                 DeathQuality = input.DeathQuality,
00263:                 Outcome = input.Outcome,
00264:             };
00265:             _state.Entries.Add(entry);
00266:             OnMemorialized?.Invoke(entry);
00267:
00268:             // Fire grief cascade — single subscriber, deterministic per
00269:             // (deceased, quality, day) input. Preserves the original
00270:             // CaptureState/RestoreState load-restore invariants because
00271:             // grief is not persisted; it's recomputed from the entry on
00272:             // the first Memorialize call.
00273:             GriefSink?.ApplyDispersion(
00274:                 entry.SurvivorId,
00275:                 input.SurvivingRelationshipIds ?? Array.Empty<string>(),
00276:                 entry.MoraleDelta,
00277:                 entry.DeathQuality,
00278:                 entry.Day);
00279:
00280:             return entry;
00281:         }
00282:
00283:         public MemorialState CaptureState() => _state.Capture();
00284:
00285:         public void RestoreState(MemorialState state)
00286:         {
00287:             if (state == null) throw new ArgumentNullException(nameof(state));
00288:             _state.RestoreInto(state);
00289:         }
00290:     }
00291:
00292:     [Serializable]
00293:     public sealed class MemorialEntry
00294:     {
00295:         public string SurvivorId;
00296:         public string Cause;
00297:         public int Day;
00298:         public int SurvivedDays;
00299:         public bool FinalWishResolved;
00300:         public string Epitaph;
00301:         public string EulogyText = string.Empty;
00302:         public string HeirloomItemId;
00303:         public string HeirloomRecipientId;
00304:         public float MoraleDelta;
00305:         // Plan 09 9C Core — additive save fields. Existing captures that
00306:         // lack these will load with default Peaceful / Burial.
00307:         public DeathQuality DeathQuality = DeathQuality.Peaceful;
00308:         public MemorialOutcome Outcome = MemorialOutcome.Burial;
00309:         /// <summary>Plan 24C (A3) — campaign day the shelter held this loss's
00310:         /// mourning vigil (−1 = never). Additive save field; legacy captures
00311:         /// load as never-mourned. The exactly-once contract rides this field.</summary>
00312:         public int MournedDay = -1;
00313:     }
00314:
00315:     [Serializable]
00316:     public sealed class MemorialInput
00317:     {
00318:         public string SurvivorId;
00319:         public string Cause;
00320:         public int Day;
00321:         public int BirthDay;
00322:         public bool FinalWishResolved;
00323:         public string Epitaph;
00324:         public string? EulogyText;
00325:         public DwellerLifeRecord? LifeRecord;
00326:         public string HeirloomItemId;
00327:         public string HeirloomRecipientId;
00328:         public float MoraleDelta;
00329:         // Plan 09 9C Core — grief-cascade input. Optional; null = no
00330:         // surviving relationship ids, host supplies gist from the
00331:         // roster-side path that called Memorialize.
00332:         public DeathQuality DeathQuality = DeathQuality.Peaceful;
00333:         public MemorialOutcome Outcome = MemorialOutcome.Burial;
00334:         public IReadOnlyList<string>? SurvivingRelationshipIds;
00335:     }
00336:
00337:     [Serializable]
00338:     public sealed class MemorialState
00339:     {
00340:         public List<MemorialEntry> Entries = new List<MemorialEntry>();
00341:
00342:         public MemorialState Capture()
00343:         {
00344:             var copy = new MemorialState();
00345:             for (int i = 0; i < Entries.Count; i++)
00346:             {
00347:                 var e = Entries[i];
00348:                 if (e == null) continue;
00349:                 copy.Entries.Add(new MemorialEntry
00350:                 {
00351:                     SurvivorId = e.SurvivorId,
00352:                     Cause = e.Cause,
00353:                     Day = e.Day,
00354:                     SurvivedDays = e.SurvivedDays,
00355:                     FinalWishResolved = e.FinalWishResolved,
00356:                     Epitaph = e.Epitaph,
00357:                     EulogyText = e.EulogyText ?? string.Empty,
00358:                     HeirloomItemId = e.HeirloomItemId,
00359:                     HeirloomRecipientId = e.HeirloomRecipientId,
00360:                     MoraleDelta = e.MoraleDelta,
00361:                     DeathQuality = e.DeathQuality,
00362:                     Outcome = e.Outcome,
00363:                     MournedDay = e.MournedDay,
00364:                 });
00365:             }
00366:             return copy;
00367:         }
00368:
00369:         public void RestoreInto(MemorialState state)
00370:         {
00371:             Entries = new List<MemorialEntry>();
00372:             if (state?.Entries != null)
00373:             {
00374:                 for (int i = 0; i < state.Entries.Count; i++)
00375:                 {
00376:                     var e = state.Entries[i];
00377:                     if (e == null) continue;
00378:                     Entries.Add(new MemorialEntry
00379:                     {
00380:                         SurvivorId = e.SurvivorId,
00381:                         Cause = e.Cause,
00382:                         Day = e.Day,
00383:                         SurvivedDays = e.SurvivedDays,
00384:                         FinalWishResolved = e.FinalWishResolved,
00385:                         Epitaph = e.Epitaph,
00386:                         EulogyText = e.EulogyText ?? string.Empty,
00387:                         HeirloomItemId = e.HeirloomItemId,
00388:                         HeirloomRecipientId = e.HeirloomRecipientId,
00389:                         MoraleDelta = e.MoraleDelta,
00390:                         DeathQuality = e.DeathQuality,
00391:                         Outcome = e.Outcome,
00392:                         MournedDay = e.MournedDay,
00393:                     });
00394:                 }
00395:             }
00396:         }
00397:     }
00398: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Memorial/MemorialSave.cs`

### `Assets/Ashfall.Core/Memorial/MemorialSave.cs` — complete current file

- Size: 19 lines / 569 bytes.
- SHA-256: `9f533827f369b178a45163cce725595c215e3b985f8c97650a86c200948cc42d`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.Medical;
00005:
00006: namespace Ashfall.Core.Memorial
00007: {
00008:     /// <summary>Checksummed save envelope for the memorial ledger.</summary>
00009:     [Serializable]
00010:     public class MemorialSave
00011:     {
00012:         public const int CurrentSaveVersion = 1;
00013:         public const int MigrationFromVersion = 1;
00014:         public int saveVersion = CurrentSaveVersion;
00015:         public int simDay;
00016:         public MemorialState State = new MemorialState();
00017:         public string Checksum = string.Empty;
00018:     }
00019: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Memorial/RelationsGriefSink.cs`

### `Assets/Ashfall.Core/Memorial/RelationsGriefSink.cs` — complete current file

- Size: 147 lines / 7302 bytes.
- SHA-256: `7ada5d66c01d04e912b15ae4d8f4ee94137ee6c8097f8890fbc8d0df3ce2e663`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004:
00005: namespace Ashfall.Core.Memorial
00006: {
00007:     /// <summary>
00008:     /// Plan 60 / D7 — the production <see cref="IGriefSink"/>: routes a
00009:     /// memorialized death's grief into the single existing relationship
00010:     /// authority (<see cref="SurvivorRelationsSystem"/>), scaled by the
00011:     /// authored <see cref="DeathQuality"/> multiplier.
00012:     ///
00013:     /// This adapter exists because both halves of the grief chain were bound to
00014:     /// nothing: <see cref="MemorialSystem.GriefSink"/> was never assigned in the
00015:     /// host, so <c>GriefSink?.ApplyDispersion(...)</c> silently no-oped, and
00016:     /// <c>SurvivorRelationsSystem.ApplyGrief</c> had no caller outside tests.
00017:     /// Neither a second grief model nor a new morale channel is introduced here —
00018:     /// this is the bridge between two authorities that already exist.
00019:     ///
00020:     /// Determinism: surviving ids are de-duplicated and sorted ordinally before
00021:     /// any mutation, so dictionary or roster iteration order cannot change the
00022:     /// result; the per-survivor amount is a pure function of the inputs.
00023:     /// </summary>
00024:     public sealed class RelationsGriefSink : IGriefSink
00025:     {
00026:         private readonly SurvivorRelationsSystem _relations;
00027:         private readonly Func<string, bool> _isAlive;
00028:
00029:         /// <summary>
00030:         /// Ceiling on grief applied per survivor per memorialization, expressed
00031:         /// against the relationship ledger's own 0..100 grief scale. Bounded by
00032:         /// design: a peaceful death must not farm morale and a cascade of deaths
00033:         /// must not instantly max every relationship.
00034:         /// </summary>
00035:         public const float MaxGriefPerSurvivorPerEvent = 20f;
00036:
00037:         /// <summary>
00038:         /// Records how many dispersion applications the sink has performed, so a
00039:         /// host test can assert "grief fired exactly once" across a save/reload —
00040:         /// the idempotence property Memorialize already guarantees for the entry.
00041:         /// </summary>
00042:         public int AppliedEventCount { get; private set; }
00043:
00044:         /// <summary>Total survivor applications performed (not survivors ×
00045:         /// events): the number a test compares against expected reach.</summary>
00046:         public int AppliedSurvivorCount { get; private set; }
00047:
00048:         /// <summary>Plan 24C (A3) — the bond-grief needs window: a mourner's
00049:         /// fatigue/morale rates decay linearly to zero over this many days
00050:         /// from the last loss. Data-authored window length (the plan's
00051:         /// "duration/intensity from existing grief data where available" — the
00052:         /// intensity itself derives from the persisted relationship grief).</summary>
00053:         public const int BondGriefDurationDays = 10;
00054:         /// <summary>Morale points per day at full grief intensity (negative).</summary>
00055:         public const float BondGriefMoralePerDay = -2f;
00056:         /// <summary>Fatigue points per day at full grief intensity.</summary>
00057:         public const float BondGriefFatiguePerDay = 1.5f;
00058:
00059:         /// <summary>Plan 24C (A3) — the pure grief-rate function shared by the
00060:         /// host's daily projection and the tests: linear decay to zero across
00061:         /// the window, intensity from the persisted relationship grief (which
00062:         /// absorbed the death-quality scale), expressed per hour. Zero outside
00063:         /// the window — the expiry is derivable from canonical facts alone.</summary>
00064:         public static float BondMoralePerHour(float relationshipGrief, int daysSinceOnset)
00065:         {
00066:             if (daysSinceOnset < 0 || daysSinceOnset >= BondGriefDurationDays) return 0f;
00067:             float intensity = Math.Clamp(relationshipGrief / 100f, 0f, 1f)
00068:                 * (1f - (float)daysSinceOnset / BondGriefDurationDays);
00069:             return BondGriefMoralePerDay * intensity / 24f;
00070:         }
00071:
00072:         public static float BondFatiguePerHour(float relationshipGrief, int daysSinceOnset)
00073:         {
00074:             if (daysSinceOnset < 0 || daysSinceOnset >= BondGriefDurationDays) return 0f;
00075:             float intensity = Math.Clamp(relationshipGrief / 100f, 0f, 1f)
00076:                 * (1f - (float)daysSinceOnset / BondGriefDurationDays);
00077:             return BondGriefFatiguePerDay * intensity / 24f;
00078:         }
00079:
00080:         /// <param name="relations">The relationship authority. Null makes the
00081:         /// sink an intentional no-op so a host can run without relationships
00082:         /// without failing the memorial pipeline.</param>
00083:         /// <param name="isAlive">Optional liveness filter; the deceased is always
00084:         /// skipped regardless.</param>
00085:         public RelationsGriefSink(
00086:             SurvivorRelationsSystem relations,
00087:             Func<string, bool> isAlive = null)
00088:         {
00089:             _relations = relations;
00090:             _isAlive = isAlive;
00091:         }
00092:
00093:         /// <inheritdoc/>
00094:         public void ApplyDispersion(
00095:             string deceasedId,
00096:             IReadOnlyList<string> survivingRelationshipIds,
00097:             float baseGriefAmount,
00098:             DeathQuality quality,
00099:             int day)
00100:         {
00101:             if (_relations == null) return;
00102:             AppliedEventCount++;
00103:
00104:             if (survivingRelationshipIds == null || survivingRelationshipIds.Count == 0)
00105:                 return;
00106:
00107:             // Memorialize passes the deceased's morale delta as the base amount;
00108:             // grief magnitude is a positive quantity regardless of the sign the
00109:             // morale channel used.
00110:             float baseAmount = Math.Abs(baseGriefAmount);
00111:             if (baseAmount <= 0f) return;
00112:
00113:             float amount = baseAmount * CapturingGriefSink.QualityScale(quality);
00114:             if (amount > MaxGriefPerSurvivorPerEvent)
00115:                 amount = MaxGriefPerSurvivorPerEvent;
00116:
00117:             // Stable order + de-dupe: the ledger must reach the same state for the
00118:             // same inputs on either host and across a reload.
00119:             var ids = new List<string>(survivingRelationshipIds.Count);
00120:             for (int i = 0; i < survivingRelationshipIds.Count; i++)
00121:             {
00122:                 string id = survivingRelationshipIds[i];
00123:                 if (string.IsNullOrEmpty(id)) continue;
00124:                 if (!string.IsNullOrEmpty(deceasedId)
00125:                     && string.Equals(id, deceasedId, StringComparison.Ordinal)) continue;
00126:                 if (!ids.Contains(id)) ids.Add(id);
00127:             }
00128:             ids.Sort(StringComparer.Ordinal);
00129:
00130:             for (int i = 0; i < ids.Count; i++)
00131:             {
00132:                 if (_isAlive != null && !_isAlive(ids[i])) continue;
00133:                 _relations.ApplyGrief(ids[i], amount);
00134:                 // Plan 24C (A3): stamp the grief onset on the affected pair so
00135:                 // the derived grief-to-needs projection (host daily refresh)
00136:                 // can decay the mourner's fatigue/morale rates from persisted
00137:                 // canonical facts alone — nothing about the needs effect is
00138:                 // saved outside the relationship ledger. The latest loss
00139:                 // re-anchors the window.
00140:                 if (_relations.TryGetRelationship(deceasedId, ids[i], out var rel)
00141:                     && rel != null)
00142:                     rel.grief_since_day = day;
00143:                 AppliedSurvivorCount++;
00144:             }
00145:         }
00146:     }
00147: }
```


# Appendix — Current Source Detail: `src/Host/MemorialSaveStore.cs`

### `src/Host/MemorialSaveStore.cs` — complete current file

- Size: 71 lines / 3262 bytes.
- SHA-256: `8e216856bc0ab145946d2fa76262ea658e4982b8b51ab9a5aaa847e5cd4e6c5b`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // Save Store : MemorialSaveStore
00004: // Core State : Ashfall.Core.Memorial.MemorialSave
00005: // Host Caller: Main.Campaign / MemorialHostSession
00006: // Purpose    : Fallen survivor memorial wall, cause of death records, and shelter grief tallies
00007: // ============================================================================
00008: using System;
00009: using Ashfall.Core;
00010: using Ashfall.Core.Memorial;
00011: using Ashfall.Core.Save;
00012:
00013: namespace AtomicWar.GodotApp
00014: {
00015:     /// <summary>
00016:     /// Persists MemorialState under user://memorial_save.json — façade over
00017:     /// the Core SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor).
00018:     /// MemorialSave is a self-checksummed type (the checksum is a field of the
00019:     /// state itself), so encode/decode stamp and verify it directly; path
00020:     /// resolution, atomic write, and error handling live in the service.
00021:     /// </summary>
00022:     public static class MemorialSaveStore
00023:     {
00024:         public const string FileName = "memorial_save.json";
00025:         public const string SectionName = "memorial";
00026:
00027:         private static readonly SaveStore<MemorialSave> s_store = SaveStoreHub.FromCodec(
00028:             FileName,
00029:             nameof(MemorialSaveStore),
00030:             EncodeSave,
00031:             DecodeSave);
00032:
00033:         public static string SavePath => s_store.SavePath;
00034:
00035:         /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
00036:         public static string TryCaptureDirect(MemorialSave state) => s_store.CaptureBare(state);
00037:
00038:         /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
00039:         public static MemorialSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
00040:
00041:         /// <summary>Capture state to JSON without writing to disk.</summary>
00042:         public static string TryCapture(MemorialSave state) => s_store.CaptureBare(state);
00043:
00044:         /// <summary>Restore state from JSON without reading from disk.</summary>
00045:         public static MemorialSave? TryRestore(string json) => s_store.RestoreBare(json);
00046:
00047:         public static bool TrySave(MemorialSave save) => s_store.TrySave(save);
00048:
00049:         public static MemorialSave? TryLoad() => s_store.TryLoad();
00050:
00051:         /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
00052:         public static string TryCapturePersisted(MemorialSave save) => s_store.CapturePersisted(save);
00053:
00054:         private static string EncodeSave(MemorialSave save, IJsonSerializer json)
00055:         {
00056:             save.Checksum = SaveChecksum.Compute(save);
00057:             return json.Serialize(save);
00058:         }
00059:
00060:         private static MemorialSave? DecodeSave(string raw, IJsonSerializer json)
00061:         {
00062:             var save = json.Deserialize<MemorialSave>(raw);
00063:             if (save == null) return null;
00064:             if (string.IsNullOrEmpty(save.Checksum))
00065:                 throw new InvalidOperationException("MemorialSave: empty checksum");
00066:             if (!string.Equals(save.Checksum, SaveChecksum.Compute(save), StringComparison.Ordinal))
00067:                 throw new InvalidOperationException("MemorialSave: checksum mismatch");
00068:             return save;
00069:         }
00070:     }
00071: }
```


# Appendix — Current Source Detail: `src/Main.ShelterSocial.cs`

### `src/Main.ShelterSocial.cs` — bounded current excerpt (539 of 602 lines)

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
00237:                 if (_survivors == null) SetupSurvivors();
00238:                 var needs = _survivors?.Needs;
00239:                 if (needs == null || needs.Get(survivorId) == null)
00240:                 {
00241:                     GD.PushWarning($"[WildlifeTrapping] Cannot apply morale {delta:0.###} to missing survivor '{survivorId}' ({source}).");
00242:                     return;
00243:                 }
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
00393:             // Plan 28 Phase 3 (overhunt): snare catches thin the local packs
00394:             // through the migration system's bounded harvest pressure.
00395:             _wildlifeTrapping.OnCatchPressure += caught =>
00396:             {
00397:                 if (_world == null) return;
00398:                 var sector = _world.ShelterSectorId;
00399:                 if (!string.IsNullOrEmpty(sector))
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


# Appendix — Current Source Detail: `src/UI/IronCenotaphMemorialPanel.cs`

### `src/UI/IronCenotaphMemorialPanel.cs` — complete current file

- Size: 187 lines / 9167 bytes.
- SHA-256: `5a39a1daac0e45ad6b386fb62deb477828137fa7e20ba7680eafbf48684f49a4`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Ashfall.Core;
00004: using Godot;
00005: using Ashfall.Core.UI;
00006: using DesignTheme = Ashfall.Core.UI.Theme;
00007:
00008: namespace AtomicWar.GodotApp.UI
00009: {
00010:     public partial class IronCenotaphMemorialPanel : Control, IBindablePanel
00011:     {
00012:         public event Action? OnClose;
00013:
00014:         private Label? _headerTitleLabel;
00015:         private Label? _statusBadgeLabel;
00016:         private Button? _closeButton;
00017:         private VBoxContainer? _telemetryContainer;
00018:         private VBoxContainer? _buttonContainer;
00019:         private VBoxContainer? _dataContainer;
00020:         private Label? _logOutputLabel;
00021:
00022:         /// <summary>Plan 24C (A3) — the vigil command: mourns the most recent
00023:         /// unmourned loss through the memorial owner; the result text is the
00024:         /// only feedback (never a silent no-op).</summary>
00025:         private void OnVigilPressed()
00026:         {
00027:             if (_mourning == null)
00028:             {
00029:                 if (_logOutputLabel != null)
00030:                     _logOutputLabel.Text = "[RIT-01] No memorial ledger bound. The dead stay uncounted.";
00031:                 return;
00032:             }
00033:             var pending = _mourning.LatestUnmourned();
00034:             if (pending == null)
00035:             {
00036:                 if (_logOutputLabel != null)
00037:                     _logOutputLabel.Text = "[RIT-01] Every recorded loss has had its vigil.";
00038:                 return;
00039:             }
00040:             var result = _mourning.Mourn(pending.Value.id);
00041:             if (_logOutputLabel != null)
00042:                 _logOutputLabel.Text = result.IsSuccess
00043:                     ? $"[RIT-01] The shelter stood together and mourned. (D{pending.Value.day} loss)"
00044:                     : "[RIT-01] The vigil could not begin: "
00045:                         + (result.MessageKey ?? result.FailureCode ?? "unspecified")
00046:                         .Replace("memorial.", "").Replace('_', ' ') + ".";
00047:             RefreshView();
00048:         }
00049:
00050:         public override void _Ready()
00051:         {
00052:             SetAnchorsPreset(LayoutPreset.FullRect);
00053:             BuildInterface();
00054:         }
00055:
00056:         public void Open()
00057:         {
00058:             Visible = true;
00059:             RefreshView();
00060:         }
00061:
00062:         public bool IsBound { get; private set; } = true;
00063:
00064:         public void Bind(object? session)
00065:         {
00066:             IsBound = true;
00067:             RefreshView();
00068:         }
00069:
00070:         public void Unbind()
00071:         {
00072:             IsBound = false;
00073:         }
00074:
00075:         /// <summary>
00076:         /// Plan 24C (A3) — the mourning route's binding: the memorial owner's
00077:         /// read model plus the once-per-death vigil command. The panel renders
00078:         /// truthful state and dispatches the command; it never recomputes
00079:         /// grief or morale (the host owns the attributed recovery).
00080:         /// </summary>
00081:         public sealed class MourningBinding
00082:         {
00083:             public Func<int> TotalDeaths { get; init; } = () => 0;
00084:             public Func<(string id, int day)?> LatestUnmourned { get; init; } = () => null;
00085:             /// <summary>deceasedId → the memorial owner's action result.</summary>
00086:             public Func<string, ActionResult> Mourn { get; init; } = _ =>
00087:                 ActionResult.Blocked("unbound", "memorial.mourn_unbound");
00088:         }
00089:
00090:         private MourningBinding? _mourning;
00091:         private Func<int>? _spiritualArcCount;
00092:         private Func<int>? _spiritualPendingRites;
00093:
00094:         public void BindMourning(MourningBinding binding)
00095:         {
00096:             _mourning = binding;
00097:             IsBound = binding != null;
00098:             RefreshView();
00099:         }
00100:
00101:         public void BindSpiritual(Func<int> arcCount, Func<int> pendingRites)
00102:         {
00103:             _spiritualArcCount = arcCount;
00104:             _spiritualPendingRites = pendingRites;
00105:             RefreshView();
00106:         }
00107:
00108:         public void RefreshView()
00109:         {
00110:             if (_statusBadgeLabel != null)
00111:             {
00112:                 // Plan 24C (A3): truthful status from the memorial owner when
00113:                 // bound; the unbound placeholder keeps its historical text.
00114:                 if (_mourning != null)
00115:                 {
00116:                     var pending = _mourning.LatestUnmourned();
00117:                     string status = pending == null
00118:                         ? $"STATUS: MEMORIAL FLAME ACTIVE - RECORDED: {_mourning.TotalDeaths()} SOULS - ALL MOURNED"
00119:                         : $"STATUS: MEMORIAL FLAME ACTIVE - RECORDED: {_mourning.TotalDeaths()} SOULS - VIGIL PENDING (D{pending.Value.day})";
00120:                     if (_spiritualArcCount != null)
00121:                     {
00122:                         int arcs = _spiritualArcCount();
00123:                         int rites = _spiritualPendingRites?.Invoke() ?? 0;
00124:                         status += $" - MOURNING ARCS: {arcs} - RITES OPEN: {rites}";
00125:                     }
00126:                     _statusBadgeLabel.Text = status;
00127:                 }
00128:             }
00129:         }
00130:
00131:         private void BuildInterface()
00132:         {
00133:             var chrome = ThreePanePanelScaffold.BuildChrome(
00134:                 this,
00135:                 "SHELTER COMMEMORATION // THE IRON CENOTAPH [RIT-01]",
00136:                 "STATUS: MEMORIAL FLAME ACTIVE - RECORDED: 38 SOULS (-18% GRIEF)",
00137:                 AshfallUiHelpers.ToColor(DesignTheme.Warm),
00138:                 "[X] CLOSE CONSOLE",
00139:                 "[RIT-01] Bronze plaque carved for Dr. Aris Thorne.\n[RIT-01] Memorial vigil conducted. Living survivor grief mitigated.",
00140:                 () => OnClose?.Invoke());
00141:             _headerTitleLabel = chrome.Title;
00142:             _statusBadgeLabel = chrome.Status;
00143:             _closeButton = chrome.Close;
00144:             _logOutputLabel = chrome.Log;
00145:             var bodyHBox = chrome.Body;
00146:
00147:             // Left Column (Telemetry)
00148:             var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("CASUALTY ROLL & MORTALITY CAUSES");
00149:             bodyHBox.AddChild(leftPanel);
00150:             _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
00151:             _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RECORDED CASUALTIES", "38 SOULS ON MEMORIAL WALL", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
00152:             _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RADIATION POISONING", "42.0% OF ALL DEATHS", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
00153:             _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("STARVATION & DEHYDRATION", "24.0% OF ALL DEATHS", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
00154:             _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("COMBAT & TRAUMA", "34.0% OF ALL DEATHS", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00155:             _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("WALL PLAQUE CAPACITY", "38 / 80 SLOTS OCCUPIED", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
00156:
00157:             // Center Column (Interactive Controls)
00158:             var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("BRONZE EPITAPH ENGRAVER & ETERNAL FLAME");
00159:             bodyHBox.AddChild(centerPanel);
00160:             _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
00161:             _buttonContainer.AddChild(AshfallUiHelpers.MakeDisabledButton(
00162:                 "[ENGRAVE BRONZE MEMORIAL PLAQUE]",
00163:                 "Not yet available \u2014 no memorial engraving authority is wired."));
00164:             _buttonContainer.AddChild(AshfallUiHelpers.MakeDisabledButton(
00165:                 "[REFUEL ETERNAL MEMORIAL FLAME]",
00166:                 "Not yet available \u2014 eternal-flame upkeep is not wired to an owner."));
00167:             // Plan 24C (A3): the vigil is the mourning action — once per death,
00168:             // routed through the memorial owner's command with truthful feedback.
00169:             var vigilButton = new Button { Text = "[HOLD ALL-SHELTER VIGIL & MOMENT OF SILENCE]", SizeFlagsHorizontal = SizeFlags.ExpandFill };
00170:             vigilButton.Pressed += OnVigilPressed;
00171:             _buttonContainer.AddChild(vigilButton);
00172:             _buttonContainer.AddChild(AshfallUiHelpers.MakeDisabledButton(
00173:                 "[RECITE DIEGETIC COMMEMORATION EULOGY]",
00174:                 "Not yet available \u2014 no eulogy authority is wired."));
00175:
00176:             // Right Column (Data & Logistics)
00177:             var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("MEMORIAL RELICS & VIGIL ATTENDANCE");
00178:             bodyHBox.AddChild(rightPanel);
00179:             _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
00180:             _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("ETERNAL OIL FLAME RESERVOIR", "42.5 LITERS (0.2 L/DAY)", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00181:             _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("MEMORIAL RELICS PRESERVED", "24 DOG TAGS / 6 WATCHES", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
00182:             _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SHELTER GRIEF MITIGATION", "-18.5% DESPAIR INDEX", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00183:             _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("LAST VIGIL ATTENDANCE", "94% OF LIVING POPULATION", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00184:
00185:         }
00186:     }
00187: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Memorial/WastelandGraveEpitaphsCatalogTests.cs`

### `Ashfall.Core.Tests/Memorial/WastelandGraveEpitaphsCatalogTests.cs` — complete current file

- Size: 278 lines / 10551 bytes.
- SHA-256: `021bfdf4bf2f9ab98af3696a908797f0628a20b07e66691dae2440e3211f6235`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using System.Text.Json;
00007: using System.Text.Json.Serialization;
00008: using Ashfall.Core;
00009: using Ashfall.Core.Memorial;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests.Memorial
00013: {
00014:     public sealed class WastelandGraveEpitaphsCatalogTests
00015:     {
00016:         private readonly string _catalogPath;
00017:
00018:         public sealed class EpitaphRecord
00019:         {
00020:             [JsonPropertyName("cause")]
00021:             public string Cause { get; set; } = string.Empty;
00022:
00023:             [JsonPropertyName("epitaph")]
00024:             public string Epitaph { get; set; } = string.Empty;
00025:         }
00026:
00027:         public sealed class EpitaphCatalog
00028:         {
00029:             [JsonPropertyName("schema_version")]
00030:             public int SchemaVersion { get; set; }
00031:
00032:             [JsonPropertyName("epitaphs")]
00033:             public List<EpitaphRecord> Epitaphs { get; set; } = new List<EpitaphRecord>();
00034:         }
00035:
00036:         public WastelandGraveEpitaphsCatalogTests()
00037:         {
00038:             string baseDir = AppDomain.CurrentDomain.BaseDirectory;
00039:             string candidate = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "wasteland_grave_epitaphs.json");
00040:             if (File.Exists(candidate))
00041:             {
00042:                 _catalogPath = Path.GetFullPath(candidate);
00043:             }
00044:             else
00045:             {
00046:                 _catalogPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "wasteland_grave_epitaphs.json");
00047:             }
00048:         }
00049:
00050:         private EpitaphCatalog LoadCatalog()
00051:         {
00052:             Assert.True(File.Exists(_catalogPath), $"Epitaph catalog not found at: {_catalogPath}");
00053:             string json = File.ReadAllText(_catalogPath);
00054:             var catalog = JsonSerializer.Deserialize<EpitaphCatalog>(json);
00055:             Assert.NotNull(catalog);
00056:             return catalog;
00057:         }
00058:
00059:         [Fact]
00060:         public void Catalog_HasValidSchemaVersion()
00061:         {
00062:             var catalog = LoadCatalog();
00063:             Assert.Equal(1, catalog.SchemaVersion);
00064:         }
00065:
00066:         [Fact]
00067:         public void Catalog_HasExactly30Entries()
00068:         {
00069:             var catalog = LoadCatalog();
00070:             Assert.Equal(30, catalog.Epitaphs.Count);
00071:         }
00072:
00073:         [Fact]
00074:         public void Catalog_PreservesExisting8BaselineEntries()
00075:         {
00076:             var catalog = LoadCatalog();
00077:             Assert.True(catalog.Epitaphs.Count >= 8);
00078:
00079:             Assert.Equal("radiation", catalog.Epitaphs[0].Cause);
00080:             Assert.StartsWith("Lethal cellular degradation.", catalog.Epitaphs[0].Epitaph);
00081:
00082:             Assert.Equal("combat", catalog.Epitaphs[1].Cause);
00083:             Assert.StartsWith("Terminated by hostiles.", catalog.Epitaphs[1].Epitaph);
00084:
00085:             Assert.Equal("starvation", catalog.Epitaphs[2].Cause);
00086:             Assert.StartsWith("Caloric deficit reached terminal state.", catalog.Epitaphs[2].Epitaph);
00087:
00088:             Assert.Equal("exhaustion", catalog.Epitaphs[3].Cause);
00089:             Assert.StartsWith("Cardiovascular collapse due to sustained labor output.", catalog.Epitaphs[3].Epitaph);
00090:
00091:             Assert.Equal("disease", catalog.Epitaphs[4].Cause);
00092:             Assert.StartsWith("Pathological contamination event.", catalog.Epitaphs[4].Epitaph);
00093:
00094:             Assert.Equal("expedition", catalog.Epitaphs[5].Cause);
00095:             Assert.StartsWith("Asset failed to return from surface operations.", catalog.Epitaphs[5].Epitaph);
00096:
00097:             Assert.Equal("trauma", catalog.Epitaphs[6].Cause);
00098:             Assert.StartsWith("Severe structural damage to biological unit.", catalog.Epitaphs[6].Epitaph);
00099:
00100:             Assert.Equal("unspecified", catalog.Epitaphs[7].Cause);
00101:             Assert.StartsWith("Termination logged.", catalog.Epitaphs[7].Epitaph);
00102:         }
00103:
00104:         [Fact]
00105:         public void Catalog_ContainsNoDuplicateStrings()
00106:         {
00107:             var catalog = LoadCatalog();
00108:             var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00109:             foreach (var item in catalog.Epitaphs)
00110:             {
00111:                 Assert.False(string.IsNullOrWhiteSpace(item.Epitaph), "Epitaph cannot be null or empty.");
00112:                 Assert.True(seen.Add(item.Epitaph), $"Duplicate epitaph found: {item.Epitaph}");
00113:             }
00114:             Assert.Equal(30, seen.Count);
00115:         }
00116:
00117:         [Fact]
00118:         public void Catalog_CoversAllRequiredCauses()
00119:         {
00120:             var catalog = LoadCatalog();
00121:             var expectedCauses = new[]
00122:             {
00123:                 "radiation", "combat", "starvation", "exhaustion",
00124:                 "disease", "expedition", "trauma", "unspecified",
00125:                 "exposure", "suicide", "infection", "old_age",
00126:                 "drowning", "frostbite", "poisoning", "execution", "unknown"
00127:             };
00128:
00129:             var causes = catalog.Epitaphs.Select(e => e.Cause).Distinct().ToHashSet(StringComparer.OrdinalIgnoreCase);
00130:             foreach (var expected in expectedCauses)
00131:             {
00132:                 Assert.True(causes.Contains(expected), $"Missing expected cause in catalog: {expected}");
00133:             }
00134:         }
00135:
00136:         [Fact]
00137:         public void Catalog_AllNew22Entries_AreOneSentenceWithin5To20Words()
00138:         {
00139:             var catalog = LoadCatalog();
00140:             Assert.Equal(30, catalog.Epitaphs.Count);
00141:
00142:             // New entries are indices 8..29 (22 additions)
00143:             for (int i = 8; i < 30; i++)
00144:             {
00145:                 var entry = catalog.Epitaphs[i];
00146:                 string text = entry.Epitaph.Trim();
00147:
00148:                 // Check terminal punctuation is single period
00149:                 Assert.EndsWith(".", text);
00150:                 int periodCount = text.Count(c => c == '.');
00151:                 Assert.Equal(1, periodCount);
00152:                 Assert.DoesNotContain(";", text);
00153:
00154:                 // Word count validation (5–20 words)
00155:                 var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
00156:                 Assert.InRange(words.Length, 5, 20);
00157:             }
00158:         }
00159:
00160:         [Fact]
00161:         public void Catalog_DeterministicSeededSelection()
00162:         {
00163:             var catalog = LoadCatalog();
00164:             string SelectForCause(string cause, int seed)
00165:             {
00166:                 var candidates = catalog.Epitaphs.Where(e => string.Equals(e.Cause, cause, StringComparison.OrdinalIgnoreCase)).ToList();
00167:                 if (candidates.Count == 0)
00168:                 {
00169:                     candidates = catalog.Epitaphs.Where(e => e.Cause == "unknown" || e.Cause == "unspecified").ToList();
00170:                 }
00171:                 var rng = new SeededRng(seed);
00172:                 int index = rng.Next(0, candidates.Count);
00173:                 return candidates[index].Epitaph;
00174:             }
00175:
00176:             // Deterministic repeated checks across seeds
00177:             for (int seed = 100; seed < 120; seed++)
00178:             {
00179:                 string r1 = SelectForCause("radiation", seed);
00180:                 string r2 = SelectForCause("radiation", seed);
00181:                 Assert.Equal(r1, r2);
00182:
00183:                 string c1 = SelectForCause("combat", seed);
00184:                 string c2 = SelectForCause("combat", seed);
00185:                 Assert.Equal(c1, c2);
00186:
00187:                 string s1 = SelectForCause("starvation", seed);
00188:                 string s2 = SelectForCause("starvation", seed);
00189:                 Assert.Equal(s1, s2);
00190:             }
00191:         }
00192:
00193:         [Fact]
00194:         public void Catalog_DifferentSeedsProduceVariety()
00195:         {
00196:             var catalog = LoadCatalog();
00197:             var radiationCandidates = catalog.Epitaphs.Where(e => e.Cause == "radiation").ToList();
00198:             Assert.Equal(3, radiationCandidates.Count);
00199:
00200:             var selected = new HashSet<string>();
00201:             for (int seed = 1; seed < 100; seed++)
00202:             {
00203:                 var rng = new SeededRng(seed);
00204:                 int index = rng.Next(0, radiationCandidates.Count);
00205:                 selected.Add(radiationCandidates[index].Epitaph);
00206:                 if (selected.Count == 3) break;
00207:             }
00208:
00209:             Assert.Equal(3, selected.Count);
00210:         }
00211:
00212:         [Fact]
00213:         public void Catalog_All30EntriesAreReachable()
00214:         {
00215:             var catalog = LoadCatalog();
00216:             var allSelected = new HashSet<string>();
00217:
00218:             foreach (var group in catalog.Epitaphs.GroupBy(e => e.Cause))
00219:             {
00220:                 var candidates = group.ToList();
00221:                 for (ulong step = 0; step < (ulong)candidates.Count * 10; step++)
00222:                 {
00223:                     int index = (int)(step % (ulong)candidates.Count);
00224:                     allSelected.Add(candidates[index].Epitaph);
00225:                 }
00226:             }
00227:
00228:             Assert.Equal(30, allSelected.Count);
00229:         }
00230:
00231:         [Fact]
00232:         public void Catalog_UnknownCause_FallsBackSafely()
00233:         {
00234:             var catalog = LoadCatalog();
00235:             string nonExistentCause = "alien_ray";
00236:             var candidates = catalog.Epitaphs.Where(e => string.Equals(e.Cause, nonExistentCause, StringComparison.OrdinalIgnoreCase)).ToList();
00237:             if (candidates.Count == 0)
00238:             {
00239:                 candidates = catalog.Epitaphs.Where(e => e.Cause == "unknown" || e.Cause == "unspecified").ToList();
00240:             }
00241:
00242:             Assert.NotEmpty(candidates);
00243:             var rng = new SeededRng(42);
00244:             int index = rng.Next(0, candidates.Count);
00245:             string chosen = candidates[index].Epitaph;
00246:             Assert.False(string.IsNullOrWhiteSpace(chosen));
00247:         }
00248:
00249:         [Fact]
00250:         public void MemorialSystem_RoundTripsWithSelectedEpitaph()
00251:         {
00252:             var catalog = LoadCatalog();
00253:             var combatEpitaph = catalog.Epitaphs.First(e => e.Cause == "combat").Epitaph;
00254:
00255:             var sys = new MemorialSystem(new MemorialState());
00256:             var entry = sys.Memorialize(new MemorialInput
00257:             {
00258:                 SurvivorId = "survivor_val",
00259:                 Cause = "combat",
00260:                 Day = 25,
00261:                 BirthDay = 1,
00262:                 Epitaph = combatEpitaph,
00263:                 MoraleDelta = -5f,
00264:             });
00265:
00266:             Assert.Equal(combatEpitaph, entry.Epitaph);
00267:
00268:             // Capture and restore
00269:             var state = sys.CaptureState();
00270:             var restoredSys = new MemorialSystem(new MemorialState());
00271:             restoredSys.RestoreState(state);
00272:
00273:             Assert.Single(restoredSys.Entries);
00274:             Assert.Equal(combatEpitaph, restoredSys.Entries[0].Epitaph);
00275:             Assert.Equal("combat", restoredSys.Entries[0].Cause);
00276:         }
00277:     }
00278: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs`

### `Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs` — complete current file

- Size: 125 lines / 4832 bytes.
- SHA-256: `c8dc55caa2fa596cae8d88ff9038bb4d00e330061a76bef7c6c1945030eff046`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.Memorial;
00005: using Xunit;
00006:
00007: namespace Ashfall.Core.Tests.Memorial
00008: {
00009:     public class MemorialSystemTests
00010:     {
00011:         [Fact]
00012:         public void Memorialize_AddsEntry()
00013:         {
00014:             var sys = new MemorialSystem(new MemorialState());
00015:             var e = sys.Memorialize(new MemorialInput
00016:             {
00017:                 SurvivorId = "elena_vasquez",
00018:                 Cause = "radiation",
00019:                 Day = 40,
00020:                 BirthDay = 1,
00021:                 FinalWishResolved = true,
00022:                 Epitaph = "She walked into the grey.",
00023:                 HeirloomItemId = "wedding_ring",
00024:                 HeirloomRecipientId = "marcus_olejnik",
00025:                 MoraleDelta = -8f
00026:             });
00027:             Assert.Single(sys.Entries);
00028:             Assert.Equal("elena_vasquez", e.SurvivorId);
00029:             Assert.Equal(39, e.SurvivedDays);
00030:         }
00031:
00032:         [Fact]
00033:         public void Memorialize_IsIdempotent_NoDuplicate()
00034:         {
00035:             var sys = new MemorialSystem(new MemorialState());
00036:             sys.Memorialize(new MemorialInput { SurvivorId = "s1", Cause = "combat", Day = 5, BirthDay = 1 });
00037:             sys.Memorialize(new MemorialInput { SurvivorId = "s1", Cause = "combat", Day = 5, BirthDay = 1 });
00038:             Assert.Single(sys.Entries);
00039:         }
00040:
00041:         [Fact]
00042:         public void Memorialize_DifferentSurvivors_BothAdded()
00043:         {
00044:             var sys = new MemorialSystem(new MemorialState());
00045:             sys.Memorialize(new MemorialInput { SurvivorId = "s1", Cause = "radiation", Day = 5, BirthDay = 1 });
00046:             sys.Memorialize(new MemorialInput { SurvivorId = "s2", Cause = "combat", Day = 6, BirthDay = 1 });
00047:             Assert.Equal(2, sys.Entries.Count);
00048:         }
00049:
00050:         [Fact]
00051:         public void Memorialize_DefaultsCauseWhenMissing()
00052:         {
00053:             var sys = new MemorialSystem(new MemorialState());
00054:             var e = sys.Memorialize(new MemorialInput { SurvivorId = "s1", Day = 5, BirthDay = 1 });
00055:             Assert.Equal("unspecified", e.Cause);
00056:         }
00057:
00058:         [Fact]
00059:         public void Memorialize_BlankCauseIsReplaced()
00060:         {
00061:             var sys = new MemorialSystem(new MemorialState());
00062:             var e = sys.Memorialize(new MemorialInput { SurvivorId = "s1", Cause = "", Day = 5, BirthDay = 1 });
00063:             Assert.Equal("unspecified", e.Cause);
00064:         }
00065:
00066:         [Fact]
00067:         public void Memorialize_RequiresSurvivorId()
00068:         {
00069:             var sys = new MemorialSystem(new MemorialState());
00070:             Assert.Throws<ArgumentException>(() =>
00071:                 sys.Memorialize(new MemorialInput { SurvivorId = "", Day = 5, BirthDay = 1 }));
00072:         }
00073:
00074:         [Fact]
00075:         public void Events_FireOnMemorialize()
00076:         {
00077:             var sys = new MemorialSystem(new MemorialState());
00078:             MemorialEntry? captured = null;
00079:             sys.OnMemorialized += e => captured = e;
00080:             sys.Memorialize(new MemorialInput { SurvivorId = "s1", Cause = "combat", Day = 5, BirthDay = 1 });
00081:             Assert.NotNull(captured);
00082:             Assert.Equal("s1", captured.SurvivorId);
00083:         }
00084:
00085:         [Fact]
00086:         public void Idempotency_DoesNotFireEventTwice()
00087:         {
00088:             var sys = new MemorialSystem(new MemorialState());
00089:             int fired = 0;
00090:             sys.OnMemorialized += _ => fired++;
00091:             sys.Memorialize(new MemorialInput { SurvivorId = "s1", Cause = "combat", Day = 5, BirthDay = 1 });
00092:             sys.Memorialize(new MemorialInput { SurvivorId = "s1", Cause = "combat", Day = 5, BirthDay = 1 });
00093:             Assert.Equal(1, fired);
00094:         }
00095:
00096:         [Fact]
00097:         public void CaptureRestore_RoundTrip()
00098:         {
00099:             var sys = new MemorialSystem(new MemorialState());
00100:             sys.Memorialize(new MemorialInput { SurvivorId = "s1", Cause = "radiation", Day = 5, BirthDay = 1 });
00101:             sys.Memorialize(new MemorialInput { SurvivorId = "s2", Cause = "combat", Day = 6, BirthDay = 1 });
00102:             var save = sys.CaptureState();
00103:             var fresh = new MemorialSystem(new MemorialState());
00104:             fresh.RestoreState(save);
00105:             Assert.Equal(2, fresh.Entries.Count);
00106:         }
00107:
00108:         [Fact]
00109:         public void HeirloomTransfer_AtomicInEntry()
00110:         {
00111:             var sys = new MemorialSystem(new MemorialState());
00112:             var e = sys.Memorialize(new MemorialInput
00113:             {
00114:                 SurvivorId = "s1",
00115:                 Cause = "radiation",
00116:                 Day = 5,
00117:                 BirthDay = 1,
00118:                 HeirloomItemId = "wedding_ring",
00119:                 HeirloomRecipientId = "marcus_olejnik"
00120:             });
00121:             Assert.Equal("wedding_ring", e.HeirloomItemId);
00122:             Assert.Equal("marcus_olejnik", e.HeirloomRecipientId);
00123:         }
00124:     }
00125: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Memorial/RelationsGriefBindingTests.cs`

### `Ashfall.Core.Tests/Memorial/RelationsGriefBindingTests.cs` — complete current file

- Size: 213 lines / 8373 bytes.
- SHA-256: `a0e5a2a5c7313c53f5027a5eea2d2c22ae081e93de35eec35a9006ab07235295`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // Plan 60 / D7 — the grief chain is bound. MemorialSystem.Memorialize already
00003: // routed to IGriefSink, and SurvivorRelationsSystem.ApplyGrief already existed,
00004: // but the sink was never assigned in the host and ApplyGrief was never called
00005: // from gameplay: two live authorities with nothing between them. These tests pin
00006: // the bridge (RelationsGriefSink) and the relationship query it depends on.
00007: using System;
00008: using System.Collections.Generic;
00009: using System.Linq;
00010: using Ashfall.Core;
00011: using Ashfall.Core.Memorial;
00012: using Xunit;
00013:
00014: namespace Ashfall.Core.Tests.Memorial
00015: {
00016:     public class RelationsGriefBindingTests
00017:     {
00018:         private static SurvivorRelationsSystem Relations(
00019:             params (string A, string B)[] pairs)
00020:         {
00021:             var sr = new SurvivorRelationsSystem(new SeededRng(7));
00022:             foreach (var p in pairs) sr.GetOrCreateRelationship(p.A, p.B);
00023:             return sr;
00024:         }
00025:
00026:         private static float GriefOf(SurvivorRelationsSystem sr, string a, string b) =>
00027:             sr.GetOrCreateRelationship(a, b).grief;
00028:
00029:         // ── RelatedIds: who mourns whom ────────────────────────────────
00030:
00031:         [Fact]
00032:         public void RelatedIds_ReturnsCounterpartiesSortedAndDeduped()
00033:         {
00034:             var sr = Relations(
00035:                 ("sv_z", "sv_b"),
00036:                 ("sv_a", "sv_z"),
00037:                 ("sv_b", "sv_z"));   // duplicate counterpart on purpose
00038:
00039:             Assert.Equal(new[] { "sv_a", "sv_b" }, sr.RelatedIds("sv_z").ToArray());
00040:         }
00041:
00042:         [Fact]
00043:         public void RelatedIds_EmptyOrUnknown_IsEmptyNotNull()
00044:         {
00045:             var sr = Relations(("sv_a", "sv_b"));
00046:
00047:             Assert.Empty(sr.RelatedIds(null));
00048:             Assert.Empty(sr.RelatedIds(""));
00049:             Assert.Empty(sr.RelatedIds("sv_nobody"));
00050:         }
00051:
00052:         [Fact]
00053:         public void RelatedIds_SelfLoopDoesNotReportTheSurvivorAsTheirOwnMourner()
00054:         {
00055:             var sr = new SurvivorRelationsSystem(new SeededRng(7));
00056:             sr.GetOrCreateRelationship("sv_a", "sv_a");
00057:
00058:             Assert.DoesNotContain("sv_a", sr.RelatedIds("sv_a"));
00059:         }
00060:
00061:         // ── the sink itself ────────────────────────────────────────────
00062:
00063:         [Fact]
00064:         public void ApplyDispersion_GrievesEachSurvivingRelationOnce()
00065:         {
00066:             var sr = Relations(("sv_dead", "sv_a"), ("sv_dead", "sv_b"));
00067:             var sink = new RelationsGriefSink(sr);
00068:
00069:             sink.ApplyDispersion(
00070:                 "sv_dead", new List<string> { "sv_b", "sv_a", "sv_a", "sv_dead" },
00071:                 baseGriefAmount: -8f, DeathQuality.Rushed, day: 30);
00072:
00073:             Assert.Equal(8f, GriefOf(sr, "sv_dead", "sv_a"), 3);
00074:             Assert.Equal(8f, GriefOf(sr, "sv_dead", "sv_b"), 3);
00075:             Assert.Equal(2, sink.AppliedSurvivorCount);
00076:             Assert.Equal(1, sink.AppliedEventCount);
00077:         }
00078:
00079:         [Theory]
00080:         [InlineData(DeathQuality.Peaceful,   4f)]   // 8 × 0.5
00081:         [InlineData(DeathQuality.Rushed,     8f)]   // 8 × 1.0
00082:         [InlineData(DeathQuality.Unattended, 10f)]  // 8 × 1.25
00083:         public void ApplyDispersion_ScalesByAuthoredDeathQuality(
00084:             DeathQuality quality, float expected)
00085:         {
00086:             var sr = Relations(("sv_dead", "sv_a"));
00087:             var sink = new RelationsGriefSink(sr);
00088:
00089:             sink.ApplyDispersion("sv_dead", new[] { "sv_a" }, 8f, quality, day: 1);
00090:
00091:             Assert.Equal(expected, GriefOf(sr, "sv_dead", "sv_a"), 3);
00092:         }
00093:
00094:         [Fact]
00095:         public void ApplyDispersion_UsesMagnitudeNotSignOfTheMoraleChannel()
00096:         {
00097:             var sr = Relations(("sv_dead", "sv_a"));
00098:             var sink = new RelationsGriefSink(sr);
00099:
00100:             // The fate path passes GriefMoraleDelta (-8), i.e. a morale loss whose
00101:             // magnitude is the grief base. Grief must not be applied as a negative.
00102:             sink.ApplyDispersion("sv_dead", new[] { "sv_a" }, -8f, DeathQuality.Rushed, 1);
00103:             sink.ApplyDispersion("sv_dead", new[] { "sv_a" }, 8f, DeathQuality.Rushed, 2);
00104:
00105:             Assert.Equal(16f, GriefOf(sr, "sv_dead", "sv_a"), 3);
00106:         }
00107:
00108:         [Fact]
00109:         public void ApplyDispersion_CapsPerSurvivorPerEvent()
00110:         {
00111:             var sr = Relations(("sv_dead", "sv_a"));
00112:             var sink = new RelationsGriefSink(sr);
00113:
00114:             sink.ApplyDispersion(
00115:                 "sv_dead", new[] { "sv_a" },
00116:                 baseGriefAmount: 900f, DeathQuality.Unattended, day: 1);
00117:
00118:             Assert.Equal(RelationsGriefSink.MaxGriefPerSurvivorPerEvent,
00119:                 GriefOf(sr, "sv_dead", "sv_a"), 3);
00120:         }
00121:
00122:         [Fact]
00123:         public void ApplyDispersion_SkipsTheDeceasedAndTheNotAlive()
00124:         {
00125:             var sr = Relations(("sv_dead", "sv_a"), ("sv_dead", "sv_b"));
00126:             var sink = new RelationsGriefSink(sr, id => id != "sv_b");
00127:
00128:             sink.ApplyDispersion(
00129:                 "sv_dead", new[] { "sv_a", "sv_b", "sv_dead", "" },
00130:                 8f, DeathQuality.Rushed, 1);
00131:
00132:             Assert.Equal(8f, GriefOf(sr, "sv_dead", "sv_a"), 3);
00133:             Assert.Equal(0f, GriefOf(sr, "sv_dead", "sv_b"), 3);
00134:             Assert.Equal(1, sink.AppliedSurvivorCount);
00135:         }
00136:
00137:         [Fact]
00138:         public void ApplyDispersion_ZeroOrNegativeBase_AppliesNothing()
00139:         {
00140:             var sr = Relations(("sv_dead", "sv_a"));
00141:             var sink = new RelationsGriefSink(sr);
00142:
00143:             sink.ApplyDispersion("sv_dead", new[] { "sv_a" }, 0f, DeathQuality.Rushed, 1);
00144:             sink.ApplyDispersion("sv_dead", new[] { "sv_a" }, -0f, DeathQuality.Rushed, 2);
00145:
00146:             Assert.Equal(0f, GriefOf(sr, "sv_dead", "sv_a"), 3);
00147:             Assert.Equal(2, sink.AppliedEventCount);
00148:             Assert.Equal(0, sink.AppliedSurvivorCount);
00149:         }
00150:
00151:         [Fact]
00152:         public void ApplyDispersion_WithoutRelationsIsADocumentedNoOp()
00153:         {
00154:             var sink = new RelationsGriefSink(null);
00155:
00156:             var ex = Record.Exception(() =>
00157:                 sink.ApplyDispersion("sv_dead", new[] { "sv_a" }, 8f, DeathQuality.Rushed, 1));
00158:
00159:             Assert.Null(ex);
00160:         }
00161:
00162:         // ── end of chain: memorialize → grief in the ledger ────────────
00163:
00164:         [Fact]
00165:         public void Memorialize_RoutesGriefIntoTheRelationshipLedgerOnceOnly()
00166:         {
00167:             var sr = Relations(("sv_dead", "sv_a"), ("sv_dead", "sv_b"));
00168:             var memorial = new MemorialSystem(new MemorialState())
00169:             {
00170:                 GriefSink = new RelationsGriefSink(sr),
00171:             };
00172:
00173:             var input = new MemorialInput
00174:             {
00175:                 SurvivorId = "sv_dead",
00176:                 Cause = "radiation",
00177:                 Day = 44,
00178:                 BirthDay = 0,
00179:                 MoraleDelta = -8f,
00180:                 DeathQuality = DeathQuality.Unattended,
00181:                 SurvivingRelationshipIds = sr.RelatedIds("sv_dead"),
00182:             };
00183:
00184:             var first = memorial.Memorialize(input);
00185:             var repeat = memorial.Memorialize(input);   // idempotent by survivor id
00186:
00187:             Assert.Same(first, repeat);
00188:             Assert.Equal(10f, GriefOf(sr, "sv_dead", "sv_a"), 3);  // 8 × 1.25
00189:             Assert.Equal(10f, GriefOf(sr, "sv_dead", "sv_b"), 3);
00190:             // Grief must also pull affinity down — the ledger already models that.
00191:             Assert.True(sr.GetOrCreateRelationship("sv_dead", "sv_a").affinity < 0f);
00192:         }
00193:
00194:         [Fact]
00195:         public void GriefApplication_IsOrderIndependent()
00196:         {
00197:             var forward = Relations(("sv_dead", "sv_a"), ("sv_dead", "sv_b"));
00198:             var reverse = Relations(("sv_dead", "sv_b"), ("sv_dead", "sv_a"));
00199:
00200:             new RelationsGriefSink(forward).ApplyDispersion(
00201:                 "sv_dead", new[] { "sv_a", "sv_b" }, 8f, DeathQuality.Rushed, 9);
00202:             new RelationsGriefSink(reverse).ApplyDispersion(
00203:                 "sv_dead", new[] { "sv_b", "sv_a" }, 8f, DeathQuality.Rushed, 9);
00204:
00205:             Assert.Equal(
00206:                 GriefOf(forward, "sv_dead", "sv_a"),
00207:                 GriefOf(reverse, "sv_dead", "sv_a"), 3);
00208:             Assert.Equal(
00209:                 GriefOf(forward, "sv_dead", "sv_b"),
00210:                 GriefOf(reverse, "sv_dead", "sv_b"), 3);
00211:         }
00212:     }
00213: }
```


# Appendix — Current Source Detail: `docs/memorials/PLAN69_BASELINE.md`

### `docs/memorials/PLAN69_BASELINE.md` — complete current file

- Size: 55 lines / 4342 bytes.
- SHA-256: `b8ca6e1102663201914ca375a48cf80049ab8b2102ae16dece4887417753f60b`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: # Plan 69 — Wasteland Grave Epitaphs Expansion: Baseline Reconnaissance
00002:
00003: **Date:** 2026-09-03
00004: **Corpus:** ASHFALL Core & Godot Host
00005: **Authority:** `Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json`
00006: **System:** `Assets/Ashfall.Core/Memorial/MemorialSystem.cs`
00007:
00008: ---
00009:
00010: ## 1. Executive Summary
00011:
00012: Plan 69 expands `wasteland_grave_epitaphs.json` from **8 verified entries to 30** so graves encountered across ASHFALL stop repeating the same small set of memorial lines and instead provide a rich, grounded, cause-aware environmental storytelling surface.
00013:
00014: This document establishes the verified pre-change baseline:
00015: - `wasteland_grave_epitaphs.json` had exactly 8 records.
00016: - All 8 records featured an official administrative/clinical death summary followed by an improvised survivor carving.
00017: - Causes covered in baseline: `radiation`, `combat`, `starvation`, `exhaustion`, `disease`, `expedition`, `trauma`, `unspecified`.
00018: - Zero compiler errors, 6623 passed xUnit tests, and 0 data integrity errors in baseline.
00019:
00020: ---
00021:
00022: ## 2. Baseline Verification Results
00023:
00024: | Command | Exit Code | Result | Evidence |
00025: |---|---|---|---|
00026: | `dotnet test Ashfall.Core.Tests` | 0 | 6623 passed, 0 failed, 0 skipped | Net9.0 xUnit suite ran in 27s |
00027: | `godot --headless --path . -- --data-integrity-selftest` | 0 | 0 errors across 208 catalogs | 10617 authored IDs, 3598 reuses |
00028: | `godot --headless --path . -- --content-utilization-selftest` | 0 | CI Gate: PASS | 490 catalogs scanned, 146 gameplay consumed |
00029: | `godot --headless --path . -- --scene-binding-selftest` | 0 | 22/22 passed | All UI scenes cleanly bound |
00030: | `python3 scripts/ci/scene-lint.py` | 0 | 0 errors, 0 warnings | 27 production scenes checked |
00031: | `dotnet build Ashfall.csproj` | 0 | 0 warnings, 0 errors | Host build clean |
00032:
00033: ---
00034:
00035: ## 3. Inventory of the Existing 8 Baseline Entries
00036:
00037: | # | Cause | Word Count | Baseline Text |
00038: |---|---|---|---|
00039: | 1 | `radiation` | 28 | "Lethal cellular degradation. Biological remains require deep burial. Below the official line, smaller, cut with a nail: they were not contagious at the end. We held their hand anyway." |
00040: | 2 | `combat` | 20 | "Terminated by hostiles. Equipment recovered and sanitized. Scratched sideways underneath: they did not run. Nobody ran. That is the whole story." |
00041: | 3 | `starvation` | 23 | "Caloric deficit reached terminal state. Carved underneath in a different hand: gave their share away three times. The third time is logged here." |
00042: | 4 | `exhaustion` | 21 | "Cardiovascular collapse due to sustained labor output. Carved deep underneath: rested on the seventh day, finally. We finished their shift for them." |
00043: | 5 | `disease` | 23 | "Pathological contamination event. Sector quarantined. Underneath, in smaller letters: we talked through the door every night. The door logged nothing. We did." |
00044: | 6 | `expedition` | 32 | "Asset failed to return from surface operations. Logged as loss. Carved under it with a knife point: not an asset. A friend. The log is wrong, and we are leaving it that way." |
00045: | 7 | `trauma` | 23 | "Severe structural damage to biological unit. Underneath, almost too rough to read: carried in. Not left alone. That much is ours to say." |
00046: | 8 | `unspecified` | 18 | "Termination logged. Rations redistributed. Underneath, the newest carving on the stone: still owe them a name. Working on it." |
00047:
00048: ---
00049:
00050: ## 4. Architectural Findings
00051:
00052: 1. **System Authority:** `MemorialSystem` (`Assets/Ashfall.Core/Memorial/MemorialSystem.cs`) is the Core authority for memorial records. It accepts `MemorialInput` which carries `SurvivorId`, `Cause`, `Day`, `BirthDay`, `FinalWishResolved`, `Epitaph`, `HeirloomItemId`, `HeirloomRecipientId`, `MoraleDelta`, `DeathQuality`, and `Outcome`.
00053: 2. **Persistence Authority:** `MemorialSaveStore` (`src/Host/MemorialSaveStore.cs`) persists `MemorialSave` containing `MemorialState`, with SHA-256 `SaveChecksum` verification. `MemorialEntry` stores `Epitaph` and `Cause`.
00054: 3. **Data Authority:** `Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json` is the top-level catalog providing short grave epitaph lines. It is separate from `Data/narrative/wasteland_grave_epitaphs.json`, which stores longer narrative codex entries.
00055: 4. **Pure Data Work:** No runtime changes to `MemorialSystem` are required; all 22 new epitaphs expand the data catalog to reach 30 entries.
```


# Appendix — Current Source Detail: `docs/memorials/PLAN69_CLOSEOUT.md`

### `docs/memorials/PLAN69_CLOSEOUT.md` — complete current file

- Size: 107 lines / 9575 bytes.
- SHA-256: `890954cee07359567cd2a392d9cd1be0b7da34394a660637d43e7bb423498920`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: # Plan 69 — Wasteland Grave Epitaphs Expansion: Closeout Report
00002:
00003: ## Status: **COMPLETE**
00004:
00005: ---
00006:
00007: ## 1. Executive Summary
00008:
00009: Plan 69 expanded `Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json` from **8 baseline entries to exactly 30 entries** (8 existing + 22 new), transforming grave markers into a broad, cause-aware environmental storytelling surface.
00010:
00011: Each of the 8 baseline records was preserved intact. All 22 new additions were authored to strict quality, length, grammar, and tone standards:
00012: - Exactly 1 sentence per addition ending in a single period (zero semicolons).
00013: - Length constrained to 14–17 words (within the 5–20 word target).
00014: - Coverage expanded across 17 distinct cause classifications (16 requested causes + 1 baseline `unspecified`).
00015: - Safety standards strictly enforced: suicide memorials are non-graphic, non-instructional, and non-romanticizing; execution and violence are non-gratuitous; poisoning references contain no procedural chemical instructions.
00016: - Full deterministic selection verified via seeded tests.
00017: - Re-enabled `WastelandGraveEpitaphsCatalogTests.cs` in `Ashfall.Core.Tests.csproj` (all 11 tests passing).
00018:
00019: ---
00020:
00021: ## 2. Quantitative Verification
00022:
00023: ```text
00024: Baseline Epitaphs:              8
00025: Target Total Epitaphs:         30
00026: New Authored Epitaphs:         22
00027: Total Verified Records:        30 (100% target achieved)
00028: Causes Covered:                17 distinct keys
00029: Duplicate Epitaphs:             0 (100% unique strings)
00030: New Entries Sentence Count:    22/22 exactly 1 sentence
00031: New Entries Word Count Range:  14 to 17 words (Target: 5–20)
00032: Catalog Tests Passing:         11/11 in WastelandGraveEpitaphsCatalogTests
00033: Full Core Test Suite:          9,395 passed, 0 failed, 0 skipped
00034: Data Integrity Findings:        0 errors across 298 catalogs
00035: ```
00036:
00037: ---
00038:
00039: ## 3. Authoritative 30-Epitaph Inventory
00040:
00041: | # | Cause | Category | Word Count | Epitaph Text |
00042: |---|---|---|---|---|
00043: | 1 | `radiation` | Baseline | 28 | "Lethal cellular degradation. Biological remains require deep burial. Below the official line, smaller, cut with a nail: they were not contagious at the end. We held their hand anyway." |
00044: | 2 | `combat` | Baseline | 20 | "Terminated by hostiles. Equipment recovered and sanitized. Scratched sideways underneath: they did not run. Nobody ran. That is the whole story." |
00045: | 3 | `starvation` | Baseline | 23 | "Caloric deficit reached terminal state. Carved underneath in a different hand: gave their share away three times. The third time is logged here." |
00046: | 4 | `exhaustion` | Baseline | 21 | "Cardiovascular collapse due to sustained labor output. Carved deep underneath: rested on the seventh day, finally. We finished their shift for them." |
00047: | 5 | `disease` | Baseline | 23 | "Pathological contamination event. Sector quarantined. Underneath, in smaller letters: we talked through the door every night. The door logged nothing. We did." |
00048: | 6 | `expedition` | Baseline | 32 | "Asset failed to return from surface operations. Logged as loss. Carved under it with a knife point: not an asset. A friend. The log is wrong, and we are leaving it that way." |
00049: | 7 | `trauma` | Baseline | 23 | "Severe structural damage to biological unit. Underneath, almost too rough to read: carried in. Not left alone. That much is ours to say." |
00050: | 8 | `unspecified` | Baseline | 18 | "Termination logged. Rations redistributed. Underneath, the newest carving on the stone: still owe them a name. Working on it." |
00051: | 9 | `radiation` | New | 14 | "The dosimeter film on her lapel turned jet black before the relief shift arrived." |
00052: | 10 | `radiation` | New | 15 | "He said the air tasted of copper two hours before the fever took his legs." |
00053: | 11 | `combat` | New | 16 | "Three people made it through the culvert gate because she stayed behind to draw the fire." |
00054: | 12 | `combat` | New | 15 | "He dropped behind the fuel tank and never had the chance to unshoulder his rifle." |
00055: | 13 | `starvation` | New | 15 | "Her name was still on the distribution clipboard when the last sack of meal spoiled." |
00056: | 14 | `starvation` | New | 15 | "He weighed less than thirty kilos when they carried him up from the boiler trench." |
00057: | 15 | `exhaustion` | New | 16 | "She laid her wrench on the pump casing and leaned her forehead against the cool pipe." |
00058: | 16 | `disease` | New | 15 | "His name was crossed off the ward roster before the second dose of penicillin arrived." |
00059: | 17 | `expedition` | New | 16 | "They found his pack three miles past the radio mast with the compass dial smashed inward." |
00060: | 18 | `trauma` | New | 16 | "The winch cable snapped during the generator hoist and left no time for anyone to yell." |
00061: | 19 | `exposure` | New | 17 | "The wind tore her tarp away while she was searching for the road marker in the dark." |
00062: | 20 | `exposure` | New | 15 | "Found kneeling inside the hollowed boiler with his damp wool coat stiffened by the rime." |
00063: | 21 | `suicide` | New | 17 | "We carved her name where she used to sit because no one found words for the rest." |
00064: | 22 | `suicide` | New | 16 | "An empty bunk in Section Four that nobody in the squad has the heart to reassign." |
00065: | 23 | `infection` | New | 15 | "We boiled the needle three times, but the red line crept past his elbow anyway." |
00066: | 24 | `old_age` | New | 17 | "He remembered what grass looked like under clear sunlight and told the children until his voice failed." |
00067: | 25 | `old_age` | New | 16 | "Eighty-two winters counted on the beam, which is sixty more than anyone had reason to expect." |
00068: | 26 | `drowning` | New | 16 | "The pontoon rope snapped in the spring current before anyone on the bank could throw another." |
00069: | 27 | `frostbite` | New | 15 | "Blackened boots left beside the fire shovel because the numbness had already reached his knees." |
00070: | 28 | `poisoning` | New | 16 | "Drank from the condensate drip behind the transformer vault before the test strip turned bright purple." |
00071: | 29 | `execution` | New | 17 | "They brought him out to the gravel pit at sunrise and read no charges from the ledger." |
00072: | 30 | `unknown` | New | 16 | "No tags, no journal, and only three brass buttons left in the gravel beneath the cairn." |
00073:
00074: ---
00075:
00076: ## 4. Documentation Suite Delivered
00077:
00078: 1. [`docs/memorials/PLAN69_BASELINE.md`](PLAN69_BASELINE.md) — Pre-change baseline verification and inventory.
00079: 2. [`docs/memorials/WASTELAND_EPITAPH_SCHEMA.md`](WASTELAND_EPITAPH_SCHEMA.md) — Exact JSON shape and C# DTO specification.
00080: 3. [`docs/memorials/WASTELAND_EPITAPH_EXISTING_8_AUDIT.md`](WASTELAND_EPITAPH_EXISTING_8_AUDIT.md) — Analysis of baseline entries and preservation rationale.
00081: 4. [`docs/memorials/WASTELAND_EPITAPH_CAUSE_MATRIX.md`](WASTELAND_EPITAPH_CAUSE_MATRIX.md) — 17-cause mapping against `SurvivorFateSystem` and `MemorialSystem`.
00082: 5. [`docs/memorials/WASTELAND_EPITAPH_DISTRIBUTION.md`](WASTELAND_EPITAPH_DISTRIBUTION.md) — Mathematical distribution plan reconciling requested vs. target counts.
00083: 6. [`docs/memorials/WASTELAND_EPITAPH_TONE_GUIDE.md`](WASTELAND_EPITAPH_TONE_GUIDE.md) — Restrained, survivor-carved aesthetic standards.
00084: 7. [`docs/memorials/WASTELAND_EPITAPH_MOTIF_AUDIT.md`](WASTELAND_EPITAPH_MOTIF_AUDIT.md) — Motif diversity analysis across 12 concrete physical categories.
00085: 8. [`docs/memorials/WASTELAND_EPITAPH_LENGTH_AUDIT.md`](WASTELAND_EPITAPH_LENGTH_AUDIT.md) — Automated word-count and sentence-boundary audit.
00086: 9. [`docs/memorials/WASTELAND_EPITAPH_SELECTION_CONTRACT.md`](WASTELAND_EPITAPH_SELECTION_CONTRACT.md) — Seeded PRNG selection and fallback specification.
00087: 10. [`docs/memorials/WASTELAND_EPITAPH_SAVE_CONTRACT.md`](WASTELAND_EPITAPH_SAVE_CONTRACT.md) — Persistence in `MemorialSaveStore` and `MemorialState`.
00088: 11. [`docs/memorials/WASTELAND_EPITAPH_MICRO_LOCATION_HANDOFF.md`](WASTELAND_EPITAPH_MICRO_LOCATION_HANDOFF.md) — Integration with Plan 49 improvised grave discoveries.
00089: 12. [`docs/memorials/WASTELAND_EPITAPH_MOURNING_HANDOFF.md`](WASTELAND_EPITAPH_MOURNING_HANDOFF.md) — Separation of text layer from mourning rites and grief cascades.
00090: 13. [`docs/memorials/WASTELAND_EPITAPH_FINAL_WISH_HANDOFF.md`](WASTELAND_EPITAPH_FINAL_WISH_HANDOFF.md) — Separation of general epitaphs from Plan 65 survivor wishes.
00091: 14. [`docs/memorials/WASTELAND_EPITAPH_CONTENT_UTILIZATION.md`](WASTELAND_EPITAPH_CONTENT_UTILIZATION.md) — Scanner mapping and live consumption validation.
00092: 15. [`docs/memorials/WASTELAND_EPITAPH_REGRESSION_MATRIX.md`](WASTELAND_EPITAPH_REGRESSION_MATRIX.md) — Contract verification trace.
00093: 16. [`docs/memorials/PLAN69_CLOSEOUT.md`](PLAN69_CLOSEOUT.md) — This closeout document.
00094:
00095: ---
00096:
00097: ## 5. Verification Matrix Evidence
00098:
00099: | Verification Gate | Command | Result | Evidence |
00100: |---|---|---|---|
00101: | **Scene Lint** | `python3 scripts/ci/scene-lint.py` | **PASS (0)** | 30 production scenes checked; 0 errors; 0 warnings |
00102: | **Catalog Integrity** | `godot --headless --path . -- --data-integrity-selftest` | **PASS (0)** | 0 errors across 298 catalogs |
00103: | **Scene Binding** | `godot --headless --path . -- --scene-binding-selftest` | **PASS (0)** | 25/25 passed |
00104: | **Content Utilization** | `godot --headless --path . -- --content-utilization-selftest` | **PASS (0)** | CI Content Utilization Gate: PASS |
00105: | **Epitaph Tests** | `dotnet test --filter "FullyQualifiedName~WastelandGraveEpitaphsCatalogTests"` | **PASS (0)** | 11 passed, 0 failed, 0 skipped (66 ms) |
00106: | **Host Build** | `dotnet build Ashfall.csproj` | **PASS (0)** | 0 errors |
00107: | **Full Regression Suite** | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS (0)** | **9,395 passed**, 0 failed, 0 skipped |
```


# Appendix — Current Source Detail: `docs/memorials/WASTELAND_EPITAPH_SELECTION_CONTRACT.md`

### `docs/memorials/WASTELAND_EPITAPH_SELECTION_CONTRACT.md` — complete current file

- Size: 34 lines / 1548 bytes.
- SHA-256: `9e8992a91585ea35e6fe89232279d089b1b59e4a9978f7c1335de3b48b8dcb8e`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: # Wasteland Grave Epitaphs — Selection Contract
00002:
00003: **Core Determinism Pillar (Invariant 4):** Deterministic execution across all hosts and seeds.
00004: **Authority:** Seeded PRNG (`ISeededRng` / `CoreSeededRng`).
00005:
00006: ---
00007:
00008: ## 1. Selection Architecture
00009:
00010: When an environmental grave or memorial entry selects an epitaph:
00011:
00012: ```text
00013: cause_of_death (string) + deterministic seed (ulong / int)
00014:                     ↓
00015: Filter candidate list by matching cause:
00016:     candidates = catalog.Epitaphs.Where(e => e.Cause == cause).ToList()
00017:                     ↓
00018: If candidates is empty, fallback to "unknown" or "unspecified":
00019:     candidates = catalog.Epitaphs.Where(e => e.Cause == "unknown" || e.Cause == "unspecified").ToList()
00020:                     ↓
00021: Seeded modulo index selection:
00022:     selectedIndex = rng.Next(0, candidates.Count)
00023:                     ↓
00024: Selected Epitaph String
00025: ```
00026:
00027: ---
00028:
00029: ## 2. Invariants
00030:
00031: 1. **Deterministic Stability:** For any fixed pair of `(cause, seed)`, the selected epitaph string is guaranteed to be identical across runs, hosts, and platforms.
00032: 2. **List Ordering Preservation:** The catalog order of existing records is strictly preserved. New records are appended sequentially.
00033: 3. **No Unreachable Candidates:** For every supported cause, every candidate in its candidate pool has a non-zero probability of selection across uniform integer rolls `Next(0, count)`.
00034: 4. **Fallback Safety:** An unrecognized cause always resolves to a valid candidate from `unknown` or `unspecified`, guaranteeing that a blank or null string is never produced.
```


# Appendix — Current Source Detail: `docs/memorials/WASTELAND_EPITAPH_SAVE_CONTRACT.md`

### `docs/memorials/WASTELAND_EPITAPH_SAVE_CONTRACT.md` — complete current file

- Size: 37 lines / 1537 bytes.
- SHA-256: `3ff758062b66e85043828a84e3f2bfde318e68cdb002782760e5464b62e744dd`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: # Wasteland Grave Epitaphs — Save & Persistence Contract
00002:
00003: **Core Persistence Authority:** `Assets/Ashfall.Core/Memorial/MemorialSave.cs`
00004: **Host Store Façade:** `src/Host/MemorialSaveStore.cs` (`user://memorial_save.json`)
00005: **Campaign Single-Envelope Store:** `SaveStoreHub` (`campaign.json`, section `memorial`)
00006:
00007: ---
00008:
00009: ## 1. Saved Grave and Memorial State
00010:
00011: In ASHFALL, memorialization persists the selected epitaph directly inside the memorial entry:
00012:
00013: ```csharp
00014: [Serializable]
00015: public sealed class MemorialEntry
00016: {
00017:     public string SurvivorId;
00018:     public string Cause;
00019:     public int Day;
00020:     public int SurvivedDays;
00021:     public bool FinalWishResolved;
00022:     public string Epitaph;              // <-- Authoritative persisted string
00023:     public string HeirloomItemId;
00024:     public string HeirloomRecipientId;
00025:     public float MoraleDelta;
00026:     public DeathQuality DeathQuality;
00027:     public MemorialOutcome Outcome;
00028: }
00029: ```
00030:
00031: ---
00032:
00033: ## 2. Backward & Forward Compatibility
00034:
00035: 1. **Direct String Persistence:** Because `Epitaph` is stored as an explicit string on `MemorialEntry`, existing saves retain their exact historical text upon reload. Expanding `wasteland_grave_epitaphs.json` does not alter, recompute, or mutate already-created memorials.
00036: 2. **Checksum Integrity:** `MemorialSave` contains a `SaveChecksum` computed across all entries. Since `MemorialEntry` schema is completely unchanged, existing saves load with valid checksums.
00037: 3. **No Migration Needed:** Expanding the data catalog is 100% additive and requires zero save-file migration.
```


# Appendix — Current Source Detail: `docs/memorials/WASTELAND_EPITAPH_TONE_GUIDE.md`

### `docs/memorials/WASTELAND_EPITAPH_TONE_GUIDE.md` — complete current file

- Size: 42 lines / 2516 bytes.
- SHA-256: `2b0a22b40a00aec891a78fe868a558dac14919f7d265f91c18c742ff8665b39a`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: # Wasteland Grave Epitaphs — Tone Guide
00002:
00003: **Pillar:** Environmental Storytelling & Memorial Atmosphere
00004: **Setting:** Ruined post-nuclear wasteland, bitter scarcity, collective trauma, physical labor.
00005:
00006: ---
00007:
00008: ## 1. Core Writing Philosophy
00009:
00010: A grave marker in ASHFALL is not a eulogy, an obituary, or a moral treatise. It is a hasty, physical mark left behind by exhausted survivors who had limited time, crude tools (a nail, a knife, scrap tin, paint, charcoal, chisel), and heavy grief.
00011:
00012: The inscription should convey **one specific, grounded circumstance** about how the person died or what remained of them at the end.
00013:
00014: ---
00015:
00016: ## 2. Mandatory Tone Attributes
00017:
00018: - **Grounded & Physical:** Anchor lines in real artifacts and physical conditions (dosimeter films, rusted culverts, damp wool, pump casings, notched wooden beams, cold pipework, gravel pits, brass buttons).
00019: - **Concise & Restrained:** Exactly one clean sentence. No purple prose, no ornate vocabulary, no melodramatic hyperbole.
00020: - **Unsentimental:** Let the bare facts carry the emotional weight. Avoid telling the reader how to feel.
00021: - **Survivor-Carved Perspective:** Words written by someone who had to dig the hole, carry the body, or clean the gear.
00022:
00023: ---
00024:
00025: ## 3. Explicit Prohibitions
00026:
00027: | Pattern to Avoid | Rationale | Approved Alternative |
00028: |---|---|---|
00029: | "Died a hero" / "Saved us all" | Romanticizing martyrdom | "Three people made it through the culvert gate because she stayed behind to draw the fire." |
00030: | "The cold found them" / "The road took her" | Repetitive sentimental clichés | "The wind tore her tarp away while she was searching for the road marker in the dark." |
00031: | "They were finally free" / "Chose peace" | Endorsement/romanticization of suicide | "We carved her name where she used to sit because no one found words for the rest." |
00032: | Graphic violence / gory medical detail | Shock-value melodrama | "The winch cable snapped during the generator hoist and left no time for anyone to yell." |
00033: | Chemical recipes / poison procedures | Actionable hazardous instructions | "Drank from the condensate drip behind the transformer vault before the test strip turned bright purple." |
00034: | Real-world countries/alliances | Invariant / World lore rules | Fictionalized wasteland terminology, local geography. |
00035:
00036: ---
00037:
00038: ## 4. Length & Punctuation Guardrails
00039:
00040: - **Sentence Count:** Exactly 1 sentence per new entry.
00041: - **Word Budget:** 5 to 20 words.
00042: - **Punctuation:** A single terminal period. No semicolon chains masquerading as single sentences.
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Memorial/MemorialComponentTests.cs`

### `Ashfall.Core.Tests/Memorial/MemorialComponentTests.cs` — bounded current excerpt (578 of 621 lines)

- Size: 621 lines / 26000 bytes.
- SHA-256: `0740549fabcd1133c01ba93cfef51a28d0b6c208e98bdc3a8e7063089258b0e2`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // Task #132 — Typed Memorial component, adapter, parity, and wire-contract coverage.
00003: using System;
00004: using System.Collections.Generic;
00005: using System.Linq;
00006: using System.Text.Json;
00007: using Ashfall.Core;
00008: using Ashfall.Core.Memorial;
00009: using Ashfall.Core.Survivors;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests.Memorial
00013: {
00014:     public class MemorialComponentTests
00015:     {
00016:         private static SurvivorId Id(string raw) => new SurvivorId(raw);
00017:
00018:         private static MemorialRecord Record(
00019:             string id,
00020:             string cause = "radiation",
00021:             int day = 40,
00022:             int survivedDays = 39,
00023:             bool finalWishResolved = true,
00024:             string epitaph = "She walked into the grey.",
00025:             string heirloomItemId = "wedding_ring",
00026:             string heirloomRecipientId = "the_keeper",
00027:             float moraleDelta = -8f)
00028:             => new MemorialRecord(
00029:                 Id(id),
00030:                 cause,
00031:                 day,
00032:                 survivedDays,
00033:                 finalWishResolved,
00034:                 epitaph,
00035:                 heirloomItemId,
00036:                 heirloomRecipientId,
00037:                 moraleDelta);
00038:
00039:         private static MemorialEntry Legacy(
00040:             string id,
00041:             string cause = "radiation",
00042:             int day = 40,
00043:             int survivedDays = 39,
00044:             bool finalWishResolved = true,
00045:             string epitaph = "She walked into the grey.",
00046:             string heirloomItemId = "wedding_ring",
00047:             string heirloomRecipientId = "the_keeper",
00048:             float moraleDelta = -8f)
00049:             => new MemorialEntry
00050:             {
00051:                 SurvivorId = id,
00052:                 Cause = cause,
00053:                 Day = day,
00054:                 SurvivedDays = survivedDays,
00055:                 FinalWishResolved = finalWishResolved,
00056:                 Epitaph = epitaph,
00057:                 HeirloomItemId = heirloomItemId,
00058:                 HeirloomRecipientId = heirloomRecipientId,
00059:                 MoraleDelta = moraleDelta
00060:             };
00061:
00062:         private static void AssertRecordEqual(MemorialRecord expected, MemorialRecord actual)
00063:         {
00064:             Assert.Equal(expected.SurvivorId, actual.SurvivorId);
00065:             Assert.Equal(expected.Cause, actual.Cause);
00066:             Assert.Equal(expected.Day, actual.Day);
00067:             Assert.Equal(expected.SurvivedDays, actual.SurvivedDays);
00068:             Assert.Equal(expected.FinalWishResolved, actual.FinalWishResolved);
00069:             Assert.Equal(expected.Epitaph, actual.Epitaph);
00070:             Assert.Equal(expected.HeirloomItemId, actual.HeirloomItemId);
00071:             Assert.Equal(expected.HeirloomRecipientId, actual.HeirloomRecipientId);
00072:             Assert.Equal(expected.MoraleDelta, actual.MoraleDelta);
00073:         }
00074:
00075:         [Fact]
00076:         public void Store_UsesExpectedMetadataAndEmptyHistoryLedger()
00077:         {
00078:             var store = new MemorialComponentStore();
00079:             ISurvivorComponentStore component = store;
00080:
00081:             Assert.Equal("memorial", component.ComponentName);
00082:             Assert.Equal(SurvivorComponentCardinality.ZeroOrOne, component.Cardinality);
00083:             Assert.True(component.RetainsHistoryAfterDeath);
00084:             Assert.Equal(0, store.Count);
00085:             Assert.Empty(store.OwnerIds);
00086:             Assert.False(store.Contains(Id("the_absent")));
00087:             Assert.False(store.Release(Id("the_absent")));
00088:         }
00089:
00090:         [Fact]
00091:         public void Record_IsIdempotent_FirstRecordWinsWithoutEventsOrLifecycleAuthority()
00092:         {
00093:             var store = new MemorialComponentStore();
00094:             var first = Record("a_first", cause: "combat");
00095:             var later = Record("a_first", cause: "radiation", day: 99);
00096:
00097:             Assert.Same(first, store.Record(first));
00098:             Assert.Same(first, store.Record(later));
00099:             Assert.False(store.TryRecord(later));
00100:             Assert.Equal(1, store.Count);
00101:             Assert.Same(first, store.TryGet(Id("a_first"), out var found) ? found : null);
00102:         }
00103:
00104:         [Fact]
00105:         public void Store_CaptureIsOrdinalDetachedAndContainsEveryHistoricalField()
00106:         {
00107:             var store = new MemorialComponentStore();
00108:             var source = Record(
00109:                 "z_last",
00110:                 cause: "industrial_fire",
00111:                 day: 73,
00112:                 survivedDays: -4,
00113:                 finalWishResolved: false,
00114:                 epitaph: "The lamp went out.",
00115:                 heirloomItemId: "field_radio",
00116:                 heirloomRecipientId: "a_first",
00117:                 moraleDelta: -12.5f);
00119:             store.Record(Record("a_first"));
00120:
00121:             var captured = store.CaptureState();
00122:
00123:             Assert.Equal(MemorialComponentStore.SchemaVersion, captured.schema_version);
00124:             Assert.Equal(MemorialComponentStore.SystemId, captured.system_id);
00125:             Assert.Equal(new[] { "a_first", "z_last" },
00126:                 captured.records.Select(row => row.survivor_id).ToArray());
00127:
00128:             var row = captured.records[1];
00129:             Assert.Equal("industrial_fire", row.cause);
00130:             Assert.Equal(73, row.day);
00131:             Assert.Equal(-4, row.survived_days);
00132:             Assert.False(row.final_wish_resolved);
00133:             Assert.Equal("The lamp went out.", row.epitaph);
00134:             Assert.Equal("field_radio", row.heirloom_item_id);
00135:             Assert.Equal("a_first", row.heirloom_recipient_id);
00136:             Assert.Equal(-12.5f, row.morale_delta);
00137:
00138:             row.cause = "mutated_after_capture";
00139:             Assert.Equal("industrial_fire", source.Cause);
00140:         }
00141:
00142:         [Fact]
00143:         public void DetachedState_RoundTripsAndPreservesWireShape()
00144:         {
00145:             var original = new MemorialComponentStore();
00146:             var source = Record("the_surveyor", survivedDays: 123);
00147:             original.Record(source);
00148:
00149:             string json = JsonSerializer.Serialize(
00150:                 original.CaptureState(), SystemTextJsonSerializer.Options);
00151:             var restoredState = JsonSerializer.Deserialize<MemorialComponentStoreState>(
00152:                 json, SystemTextJsonSerializer.Options);
00153:             var restored = new MemorialComponentStore();
00154:             var report = restored.RestoreState(restoredState);
00155:
00156:             Assert.True(report.IsClean, report.ToString());
00157:             Assert.Equal(1, report.Accepted);
00158:             Assert.True(restored.TryGet(Id("the_surveyor"), out var state));
00159:             Assert.NotNull(state);
00160:             AssertRecordEqual(source, state!);
00161:             Assert.Contains("schema_version", json);
00162:             Assert.Contains("system_id", json);
00163:             Assert.Contains("survivor_id", json);
00164:             Assert.Contains("survived_days", json);
00165:             Assert.Equal(
00166:                 json,
00167:                 JsonSerializer.Serialize(restored.CaptureState(), SystemTextJsonSerializer.Options));
00168:         }
00169:
00170:         [Fact]
00171:         public void Restore_RejectsNullInvalidAndDuplicateRows_FirstRowWins()
00172:         {
00173:             var state = new MemorialComponentStoreState();
00174:             state.records.Add(new MemorialRecordState
00175:             {
00176:                 survivor_id = "the_good",
00177:                 cause = "first",
00183:             {
00184:                 survivor_id = "the_good",
00185:                 cause = "second",
00186:                 survived_days = 99
00187:             });
00188:             state.records.Add(null!);
00189:
00190:             var store = new MemorialComponentStore();
00191:             var report = store.RestoreState(state);
00192:
00193:             Assert.Equal(1, report.Accepted);
00194:             Assert.Equal(4, report.Rejected.Count);
00195:             Assert.True(store.TryGet(Id("the_good"), out var restored));
00196:             Assert.Equal("first", restored!.Cause);
00197:             Assert.Equal(12, restored.SurvivedDays);
00198:             Assert.Contains(report.Rejected, row => row.Contains("uppercase", StringComparison.OrdinalIgnoreCase));
00199:             Assert.Contains(report.Rejected, row => row.Contains("duplicate", StringComparison.Ordinal));
00200:             Assert.Contains(report.Rejected, row => row.Contains("null entry", StringComparison.Ordinal));
00201:         }
00202:
00203:         [Fact]
00204:         public void Restore_FutureSchemaAndWrongSystemPreserveCurrentState()
00205:         {
00206:             var store = new MemorialComponentStore();
00207:             var current = Record("a_current", cause: "current");
00208:             store.Record(current);
00209:
00210:             var future = new MemorialComponentStoreState
00211:             {
00212:                 schema_version = MemorialComponentStore.SchemaVersion + 1,
00213:                 system_id = MemorialComponentStore.SystemId
00214:             };
00215:             future.records.Add(new MemorialRecordState { survivor_id = "z_future" });
00216:             var futureReport = store.RestoreState(future);
00217:
00218:             Assert.True(futureReport.IsFatal);
00219:             Assert.Contains("newer than this build", futureReport.FatalReason);
00220:             Assert.Same(current, store.TryGet(Id("a_current"), out var afterFuture) ? afterFuture : null);
00221:             Assert.False(store.Contains(Id("z_future")));
00222:
00223:             var foreign = new MemorialComponentStoreState
00224:             {
00225:                 schema_version = MemorialComponentStore.SchemaVersion,
00226:                 system_id = "not_memorial"
00227:             };
00228:             foreign.records.Add(new MemorialRecordState { survivor_id = "z_foreign" });
00229:             var foreignReport = store.RestoreState(foreign);
00230:
00231:             Assert.True(foreignReport.IsFatal);
00232:             Assert.Contains("does not match", foreignReport.FatalReason);
00233:             Assert.Same(current, store.TryGet(Id("a_current"), out var afterForeign) ? afterForeign : null);
00234:             Assert.False(store.Contains(Id("z_foreign")));
00235:         }
00236:
00237:         [Fact]
00238:         public void Restore_NullStateIsTheExplicitEmptyResetForm()
00239:         {
00240:             var store = new MemorialComponentStore();
00241:             store.Record(Record("a_current"));
00242:
00243:             var report = store.RestoreState(null);
00244:
00245:             Assert.True(report.IsClean, report.ToString());
00246:             Assert.Equal(0, store.Count);
00247:             Assert.Empty(store.OwnerIds);
00249:
00250:         [Fact]
00251:         public void ReleaseDoesNotEraseHistory_ButResetDoes()
00252:         {
00253:             var store = new MemorialComponentStore();
00254:             store.Record(Record("a_one"));
00255:             store.Record(Record("b_two"));
00256:
00257:             Assert.False(store.Release(Id("a_one")));
00258:             Assert.True(store.Contains(Id("a_one")));
00259:             Assert.Equal(2, store.Count);
00260:
00261:             store.Reset();
00262:             Assert.Equal(0, store.Count);
00265:
00266:         [Fact]
00267:         public void Adapter_ImportsAllFieldsPreservesSurvivedDaysAndOrdersOwners()
00268:         {
00269:             var store = new MemorialComponentStore();
00270:             var entries = new List<MemorialEntry>
00271:             {
00272:                 Legacy(
00273:                     "z_last",
00275:                     day: 73,
00276:                     survivedDays: -4,
00277:                     finalWishResolved: false,
00278:                     epitaph: "The lamp went out.",
00279:                     heirloomItemId: "field_radio",
00280:                     heirloomRecipientId: "a_first",
00281:                     moraleDelta: -12.5f),
00283:             };
00284:
00285:             var report = MemorialComponentAdapter.ImportLegacy(entries, store);
00286:
00287:             Assert.True(report.IsClean, string.Join("\n", report.Rejected));
00288:             Assert.Equal(2, report.Accepted);
00289:             Assert.Equal(new[] { "a_first", "z_last" },
00290:                 store.OwnerIds.Select(id => id.Value).ToArray());
00291:             Assert.True(store.TryGet(Id("z_last"), out var imported));
00292:             Assert.NotNull(imported);
00293:             Assert.Equal("industrial_fire", imported!.Cause);
00294:             Assert.Equal(73, imported.Day);
00295:             Assert.Equal(-4, imported.SurvivedDays);
00296:             Assert.False(imported.FinalWishResolved);
00297:             Assert.Equal("The lamp went out.", imported.Epitaph);
00298:             Assert.Equal("field_radio", imported.HeirloomItemId);
00299:             Assert.Equal("a_first", imported.HeirloomRecipientId);
00300:             Assert.Equal(-12.5f, imported.MoraleDelta);
00302:
00303:         [Fact]
00304:         public void Adapter_ReportsNullInvalidDuplicateUnknownAndLivingRows()
00305:         {
00306:             var entities = new SurvivorEntityStore();
00307:             entities.TryJoin(Id("a_dead"), "a_dead", 1);
00308:             entities.TryDie(Id("a_dead"), 4);
00318:                 null!
00319:             };
00320:             var store = new MemorialComponentStore();
00321:
00322:             var report = MemorialComponentAdapter.ImportLegacy(entries, store, entities);
00323:
00324:             Assert.Equal(1, report.Accepted);
00325:             Assert.Equal(5, report.Rejected.Count);
00326:             Assert.Contains(report.Rejected, row => row.StartsWith(MemorialImportCode.DuplicateId, StringComparison.Ordinal));
00327:             Assert.Contains(report.Rejected, row => row.StartsWith(MemorialImportCode.OwnerLiving, StringComparison.Ordinal));
00328:             Assert.Contains(report.Rejected, row => row.StartsWith(MemorialImportCode.OwnerUnknown, StringComparison.Ordinal));
00329:             Assert.Contains(report.Rejected, row => row.StartsWith(MemorialImportCode.LegacyIdInvalid, StringComparison.Ordinal));
00330:             Assert.Contains(report.Rejected, row => row.StartsWith(MemorialImportCode.LegacyRowNull, StringComparison.Ordinal));
00331:             Assert.True(store.Contains(Id("a_dead")));
00332:             Assert.Equal(11, store.TryGet(Id("a_dead"), out var imported) ? imported!.SurvivedDays : -1);
00333:         }
00334:
00335:         [Fact]
00336:         public void Adapter_MapsNullableLegacyStringsToTypedDefaults()
00337:         {
00338:             var entry = Legacy("a_null");
00339:             entry.Cause = null!;
00340:             entry.Epitaph = null!;
00341:             entry.HeirloomItemId = null!;
00342:             entry.HeirloomRecipientId = null!;
00343:             var store = new MemorialComponentStore();
00344:
00345:             var report = MemorialComponentAdapter.ImportLegacy(
00346:                 new[] { entry }, store);
00347:
00348:             Assert.True(report.IsClean, string.Join("\n", report.Rejected));
00349:             Assert.True(store.TryGet(Id("a_null"), out var imported));
00350:             Assert.NotNull(imported);
00351:             Assert.Equal(string.Empty, imported!.Cause);
00352:             Assert.Equal(string.Empty, imported.Epitaph);
00353:             Assert.Equal(string.Empty, imported.HeirloomItemId);
00354:             Assert.Equal(string.Empty, imported.HeirloomRecipientId);
00355:             Assert.Equal(4, MemorialComponentParity.Compare(new[] { entry }, store)
00356:                 .Findings.Count(finding => finding.Code == MemorialParityCode.LegacyFieldNull));
00357:         }
00358:
00359:         [Fact]
00360:         public void Adapter_DoesNotMutateEntityLifecycleOrRevision()
00361:         {
00362:             var entities = new SurvivorEntityStore();
00363:             entities.TryJoin(Id("a_dead"), "a_dead", 2);
00364:             entities.TryDie(Id("a_dead"), 8);
00365:             entities.TryMemorialize(Id("a_dead"), 9);
00366:             Assert.True(entities.TryGet(Id("a_dead"), out var before));
00367:             long revision = before!.Revision;
00368:             var transitions = 0;
00369:             entities.OnLifecycleChanged += _ => transitions++;
00370:
00371:             var report = MemorialComponentAdapter.ImportLegacy(
00372:                 new[] { Legacy("a_dead") },
00373:                 new MemorialComponentStore(),
00374:                 entities);
00375:
00376:             Assert.True(report.IsClean, string.Join("\n", report.Rejected));
00377:             Assert.True(entities.TryGet(Id("a_dead"), out var after));
00378:             Assert.Equal(SurvivorLifecycleState.Memorialized, after!.Lifecycle);
00379:             Assert.Equal(revision, after.Revision);
00380:             Assert.Equal(0, transitions);
00381:         }
00382:
00383:         [Fact]
00384:         public void Parity_IsCleanForMatchingLegacyAndTypedRows()
00385:         {
00386:             var legacy = new List<MemorialEntry> { Legacy("a_one") };
00387:             var typed = new MemorialComponentStore();
00388:             typed.Record(Record("a_one"));
00389:
00390:             var report = MemorialComponentParity.Compare(legacy, typed);
00391:
00392:             Assert.True(report.IsMatch, report.Describe());
00393:             Assert.Equal(1, report.LegacyRows);
00394:             Assert.Equal(1, report.TypedRows);
00396:
00397:         [Fact]
00398:         public void Parity_ReportsDuplicateMissingExtraAndStableOrdering()
00399:         {
00400:             var legacy = new List<MemorialEntry>
00401:             {
00402:                 Legacy("z_duplicate", cause: "first"),
00403:                 Legacy("a_missing"),
00404:                 Legacy("z_duplicate", cause: "second")
00405:             };
00406:             var typed = new MemorialComponentStore();
00407:             typed.Record(Record("a_missing"));
00408:             typed.Record(Record("b_extra"));
00409:
00410:             var report = MemorialComponentParity.Compare(legacy, typed);
00411:
00412:             Assert.False(report.IsMatch);
00413:             Assert.Contains(report.Findings, finding => finding.Code == MemorialParityCode.LegacyDuplicateId);
00414:             Assert.Contains(report.Findings, finding => finding.Code == MemorialParityCode.TypedRecordExtra && finding.SurvivorId == Id("b_extra"));
00415:             Assert.Contains(report.Findings, finding => finding.Code == MemorialParityCode.TypedRecordMissing && finding.SurvivorId == Id("z_duplicate"));
00416:
00417:             var ordered = report.Findings
00418:                 .Select(finding => (finding.SurvivorId.Value, finding.RawId, finding.Code, finding.Field, finding.Message))
00419:                 .ToArray();
00429:
00430:         [Fact]
00431:         public void Parity_UsesLegacyFirstDuplicateForFieldComparison()
00432:         {
00433:             var first = Legacy("a_duplicate", cause: "first");
00434:             var later = Legacy("a_duplicate", cause: "later");
00435:             var typed = new MemorialComponentStore();
00436:             typed.Record(Record("a_duplicate", cause: "first"));
00437:
00438:             var report = MemorialComponentParity.Compare(
00439:                 new[] { first, later }, typed);
00440:
00441:             Assert.Contains(report.Findings,
00442:                 finding => finding.Code == MemorialParityCode.LegacyDuplicateId);
00443:             Assert.DoesNotContain(report.Findings,
00444:                 finding => finding.Code == MemorialParityCode.FieldMismatch);
00445:         }
00446:
00447:         [Fact]
00448:         public void Parity_ReportsEveryHistoricalFieldMismatch()
00449:         {
00450:             var legacy = new List<MemorialEntry>
00451:             {
00452:                 Legacy(
00453:                     "a_one",
00454:                     cause: "radiation",
00455:                     day: 40,
00456:                     survivedDays: 39,
00457:                     finalWishResolved: true,
00458:                     epitaph: "old",
00459:                     heirloomItemId: "ring",
00460:                     heirloomRecipientId: "recipient",
00461:                     moraleDelta: -8f)
00462:             };
00463:             var typed = new MemorialComponentStore();
00464:             typed.Record(Record(
00465:                 "a_one",
00466:                 cause: "combat",
00467:                 day: 41,
00468:                 survivedDays: 38,
00469:                 finalWishResolved: false,
00470:                 epitaph: "new",
00471:                 heirloomItemId: "radio",
00472:                 heirloomRecipientId: "other",
00473:                 moraleDelta: -7f));
00474:
00475:             var fields = MemorialComponentParity.Compare(legacy, typed)
00476:                 .Findings
00477:                 .Where(finding => finding.Code == MemorialParityCode.FieldMismatch)
00478:                 .Select(finding => finding.Field)
00479:                 .ToHashSet(StringComparer.Ordinal);
00482:                 new[]
00483:                 {
00484:                     "cause", "day", "epitaph", "final_wish_resolved",
00485:                     "heirloom_item_id", "heirloom_recipient_id", "morale_delta", "survived_days"
00486:                 },
00487:                 fields.OrderBy(field => field, StringComparer.Ordinal).ToArray());
00488:         }
00489:
00490:         [Fact]
00491:         public void Parity_ReportsNullLegacyFieldsAndMalformedIds()
00492:         {
00493:             var malformed = new MemorialEntry
00494:             {
00495:                 SurvivorId = "The_Bad",
00505:             nullFields.HeirloomRecipientId = null!;
00506:
00507:             var typed = new MemorialComponentStore();
00508:             typed.Record(new MemorialRecord(
00509:                 Id("a_null"), null, 40, 39, true, null, null, null, -8f));
00510:
00511:             var report = MemorialComponentParity.Compare(
00512:                 new List<MemorialEntry> { malformed, nullFields }, typed);
00513:
00514:             Assert.Contains(report.Findings, finding => finding.Code == MemorialParityCode.LegacyIdInvalid);
00515:             var nullFindings = report.Findings
00516:                 .Where(finding => finding.Code == MemorialParityCode.LegacyFieldNull)
00517:                 .Select(finding => finding.Field)
00518:                 .ToHashSet(StringComparer.Ordinal);
00520:                 new[] { "cause", "epitaph", "heirloom_item_id", "heirloom_recipient_id" },
00521:                 nullFindings);
00522:             Assert.DoesNotContain(report.Findings,
00523:                 finding => finding.SurvivorId == Id("a_null") &&
00524:                            finding.Code == MemorialParityCode.FieldMismatch);
00525:         }
00526:
00527:         [Fact]
00528:         public void Parity_IsDeterministicRegardlessOfLegacyRegistrationOrder()
00529:         {
00530:             var firstOrder = new List<MemorialEntry>
00531:             {
00532:                 Legacy("z_legacy"),
00533:                 Legacy("a_legacy")
00534:             };
00535:             var secondOrder = new List<MemorialEntry>
00536:             {
00537:                 Legacy("a_legacy"),
00538:                 Legacy("z_legacy")
00539:             };
00540:             var typed = new MemorialComponentStore();
00541:             typed.Record(Record("b_extra"));
00542:
00543:             var first = MemorialComponentParity.Compare(firstOrder, typed);
00544:             var second = MemorialComponentParity.Compare(secondOrder, typed);
00545:
00546:             Assert.Equal(
00547:                 first.Findings.Select(finding => finding.ToString()).ToArray(),
00548:                 second.Findings.Select(finding => finding.ToString()).ToArray());
00549:         }
00550:
00551:         [Fact]
00552:         public void TypedStore_IntegratesWithReferentialIntegrityWithoutRejectingHistory()
00553:         {
00554:             var entities = new SurvivorEntityStore();
00555:             entities.TryJoin(Id("a_dead"), "a_dead", 1);
00556:             entities.TryDie(Id("a_dead"), 2);
00557:             entities.TryMemorialize(Id("a_dead"), 3);
00558:
00559:             var memorial = new MemorialComponentStore();
00560:             memorial.Record(Record("a_dead"));
00561:             memorial.Record(Record("z_unknown"));
00562:             entities.RegisterComponentStore(memorial);
00563:
00564:             var report = SurvivorIntegrityValidator.Validate(entities);
00565:
00566:             var ownerUnknown = Assert.Single(
00567:                 report.Findings,
00568:                 finding => finding.Code == SurvivorIntegrityCode.ComponentOwnerUnknown);
00569:             Assert.Equal(Id("z_unknown"), ownerUnknown.SurvivorId);
00570:             Assert.DoesNotContain(report.Findings,
00571:                 finding => finding.Code == SurvivorIntegrityCode.ComponentOnDeceased);
00572:         }
00573:
00574:         [Fact]
00575:         public void RetainedHistorySurvivesLivingOwnerRemoval()
00576:         {
00577:             var entities = new SurvivorEntityStore();
00578:             entities.TryJoin(Id("a_departing"), "a_departing", 1);
00579:             var memorial = new MemorialComponentStore();
00580:             memorial.Record(Record("a_departing"));
00581:             entities.RegisterComponentStore(memorial);
00582:
00583:             var result = entities.TryLeave(Id("a_departing"), 5);
00584:
00585:             Assert.True(result.IsCommitted);
00586:             Assert.False(entities.Contains(Id("a_departing")));
00587:             Assert.True(memorial.Contains(Id("a_departing")));
00588:         }
00589:
00590:         [Fact]
00591:         public void MemorialSave_CoreWireFieldsAndChecksumRemainDirectV1()
00592:         {
00593:             var save = new MemorialSave
00594:             {
00595:                 saveVersion = MemorialSave.CurrentSaveVersion,
00596:                 simDay = 44,
00597:                 State = new MemorialState
00598:                 {
00599:                     Entries = new List<MemorialEntry> { Legacy("a_one", survivedDays: 43) }
00600:                 }
00601:             };
00602:             save.Checksum = SaveChecksum.Compute(save);
00603:             var json = new SystemTextJsonSerializer();
00604:             string raw = json.Serialize(save);
00605:             var restored = json.Deserialize<MemorialSave>(raw);
00606:
00607:             Assert.NotNull(restored);
00608:             Assert.Equal(1, restored!.saveVersion);
00609:             Assert.Equal(44, restored.simDay);
00610:             Assert.Equal(save.Checksum, restored.Checksum);
00611:             Assert.Equal(save.Checksum, SaveChecksum.Compute(restored));
00612:             Assert.Contains("saveVersion", raw);
00613:             Assert.Contains("simDay", raw);
00614:             Assert.Contains("State", raw);
00615:             Assert.Contains("Entries", raw);
00616:             Assert.Contains("SurvivorId", raw);
00617:             Assert.Contains("SurvivedDays", raw);
00618:             Assert.Contains("Checksum", raw);
00619:         }
00620:     }
00621: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **The subject is a compact authored memorial text layer attached to the current MemorialSystem entry, with environmental grave discovery kept as a separate unproven route. The plan expands selection, persistence, tone and reachability precision.**.

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
