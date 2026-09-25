# Plan 93 — Verdict Investigation NPCs, Dialogue Gates and One-Shot State

> **Rebuild status:** COMPLETE 18-NPC CATALOG/DATA LOOP — SITE REACHABILITY AND DIALOG CONSEQUENCE AUDIT
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

- The historical baseline was six NPCs; DEC-263 expanded the current file to 18 entries spanning tape echoes, paper ghosts, living figures and readings.
- The live route is `verdict_npcs.json` → `VerdictNpcCatalogLoader` → `VerdictNpcSystem.GetAvailable/Speak` → `VerdictHostSession.MaterializedNpcFlags`/`AvailableNpcs` → current Verdict panel/host → `VerdictSave` v4 migration.
- The high-value work is semantic: every NPC has a location, phase floor, gating flag, dialogue and one-shot identity; a row should not appear merely because it parsed. The plan therefore treats the player encounter and exactly-oncte state as the integration contract.

**Bounded outcome:** Retire the old 6→15 pure-data brief as a new authoring project. The current authority contains 18 NPCs across the 15 Verdict locations, the Core loader and one-shot state are live, `VerdictHostSession` materializes current flags, and `VerdictSave` carries the NPC state. The remaining plan is to prove site/phase/flag reachability and truthful dialogue presentation without creating a second NPC or Verdict save owner.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `verdict_npcs.json` is valid schema version 1 with 18 `items` rows; current data includes 15 investigation-site references and phase values across the authored range.
- `VerdictNpcSystem` exposes catalog registration, availability filtering, one-shot `Speak`, capture and restore; it does not spawn a physical faction or own Verdict evidence.
- `VerdictHostSession.Create` loads the catalog, derives real progress flags and exposes `AvailableNpcs`; `VerdictSave` current version 4 carries the NPC state alongside machine log, reckoning, evidence, radio, quests and accusations.
- Historical DEC-263 tests cover 18 rows, 15 sites and one-shot interaction; this planning artifact does not claim a fresh test result.

**Master-authority sections applied to this rebase:**

- Part II Factory Protocol: premise sweep, collision check, one lane/cluster, and evidence labels before drafting.
- Part II Step 5 continuity and anti-duplication checklist: data presence is not reachability.
- Part III cluster map: use the live C1–C17 owner map rather than a historical plan title.
- Part IV backlog discipline: consume a verified candidate or record why it is stale; do not widen a bounded outcome.
- Part V Template S/R: subject intent and recommended route remain separate from implementation commitments.
- Part VI Multi-Session Growth Protocol: 250k is a depth target, not permission to manufacture volume.
- Live source/data authority: current catalog, loader, host, save, and focused tests outrank generated prose.
- Anti-padding rule: if the evidence queue is exhausted, stop and report no warranted continuation.
- C13 Verdict/epilogue cluster: NPC evidence is a projection over current investigation state, not a new investigation owner.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 6→15 count target with an 18-row current census and a site/phase/flag/kind coverage matrix.
- Trace each row from the current Verdict location and progress flag to an available host projection and a visible dialogue action.
- Audit one-shot behavior across repeated clicks, save/restore, wrong location and changed phase; no new NPC state is needed for this residual.
- Preserve VerdictSave v1–v4 migration and the current host flag materialization rules.

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
| NPC catalog parsing and registration | VerdictNpcCatalogLoader | `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs` | Loads authored NPC rows; it does not decide availability from arbitrary UI state. |
| availability, one-shot speech and NPC state | VerdictNpcSystem | `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs` | Owns `spokenNpcIds` and emits `OnSpoken` after a successful one-shot action. |
| live progress flags and host projection | VerdictHostSession | `src/Host/VerdictHostSession.cs` | Maps current machine/evidence/reckoning progress to flags and exposes available NPCs. |
| NPC persistence and version migration | VerdictSaveCodec | `Assets/Ashfall.Core/Verdict/VerdictSave.cs` | Current Verdict envelope owns NPC state; no Plan-93-specific section. |
| player-visible encounter and diagnostic surface | Verdict UI/CLI | `src/VerdictPanel.cs; src/Host/HostCli.ExpansionDepth.cs` | Presentation/host projection only; it cannot mark an NPC spoken directly. |
| row coverage, gates, one-shot and migration proof | Verdict focused tests | `Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs; Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs; Ashfall.Core.Tests/VerdictSystemTests.cs` | Focused evidence surface. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Verdict Investigation NPCs, Dialogue Gates and One-Shot State
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ VerdictNpcCatalogLoader
│   NPC catalog parsing and registration
│ VerdictNpcSystem
│   availability, one-shot speech and NPC state
│ VerdictHostSession
│   live progress flags and host projection
│ VerdictSaveCodec
│   NPC persistence and version migration
│ Verdict UI/CLI
│   player-visible encounter and diagnostic surface
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

1. **Preserve current state ownership.** VerdictNpcCatalogLoader owns NPC catalog parsing and registration: Loads authored NPC rows; it does not decide availability from arbitrary UI state.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| NPC catalog parsing and registration | VerdictNpcCatalogLoader | `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs` | Loads authored NPC rows; it does not decide availability from arbitrary UI state. |
| availability, one-shot speech and NPC state | VerdictNpcSystem | `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs` | Owns `spokenNpcIds` and emits `OnSpoken` after a successful one-shot action. |
| live progress flags and host projection | VerdictHostSession | `src/Host/VerdictHostSession.cs` | Maps current machine/evidence/reckoning progress to flags and exposes available NPCs. |
| NPC persistence and version migration | VerdictSaveCodec | `Assets/Ashfall.Core/Verdict/VerdictSave.cs` | Current Verdict envelope owns NPC state; no Plan-93-specific section. |
| player-visible encounter and diagnostic surface | Verdict UI/CLI | `src/VerdictPanel.cs; src/Host/HostCli.ExpansionDepth.cs` | Presentation/host projection only; it cannot mark an NPC spoken directly. |
| row coverage, gates, one-shot and migration proof | Verdict focused tests | `Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs; Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs; Ashfall.Core.Tests/VerdictSystemTests.cs` | Focused evidence surface. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load `verdict_npcs.json` through the current Verdict host
2. read live machine/evidence/reckoning state
3. materialize the existing progress flags
4. query available NPCs by phase and canonical location
5. present a site-specific dialogue command
6. call `Speak` through the owner
7. project the one-shot result and capture the existing Verdict envelope

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- NPC metadata is catalog data; `spokenNpcIds` is the current one-shot owner state.
- Availability is a pure query over supplied flags, phase and optional location; it does not mutate the catalog or flag ledger.
- A successful `Speak` adds the ID once and emits one fact; a failed location/phase/unknown call does not mutate state.
- Restore deep-copies spoken IDs and must not replay the historical speech event.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A location ID must resolve through the current Verdict location authority; an unresolved row fails closed or is reported as data drift.
- A phase floor and gating flag are both required when authored; an empty gate means unconditional only when the row’s contract says so.
- One NPC can speak once per save, independent of refresh, panel rebuild or repeated host calls.
- The same current flag/phase/location state produces the same ordered availability list.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `verdict_npcs.json` is the sole NPC authority; no duplicate roster belongs in the panel or save file.
- New rows require a current Verdict location, valid phase, explicit gate semantics, dialogue and a host consumer.
- A row count is not proof that every investigation site has a reachable encounter.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use `VerdictSave` current version 4 and its frozen v1/v2/v3 shapes; no new NPC save section.
- NPC state is restored beside the other Verdict owners, and missing legacy NPC state defaults empty.
- Checksum validation must cover the current envelope before any host command is offered.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Availability ordering should be stable by catalog order or ordinal ID; the current source does not require randomness.
- The host uses fixed machine-log seed behavior; no new RNG belongs in NPC selection.
- Two identical host traces produce identical available IDs, first speech results and spoken sets.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- `OnSpoken` is emitted only after a successful one-shot NPC interaction.
- The host records `LastEvent` and raises state changed for presentation refresh.
- No new day-event kind is required for an NPC line.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/VerdictHostSession.cs
- src/VerdictPanel.cs
- src/Host/HostCli.ExpansionDepth.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- NPC voices should be terse, situated and fictional; a line must not claim a fact not represented by the current evidence/phase state.
- Human residue is a narrative layer over the existing investigation authority, not a new faction or combat system.
- Keep uncertainty honest: an unavailable figure is not a hidden puzzle answer.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A parsed NPC is displayed at the wrong location or phase. | VerdictNpcCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A repeated click speaks twice or emits two journal facts. | VerdictNpcSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A panel marks an NPC spoken without calling `VerdictNpcSystem.Speak`. | VerdictHostSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A missing legacy NPC field crashes Verdict restore. | VerdictSaveCodec | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new catalog duplicates the Verdict evidence owner. | Verdict UI/CLI | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/VerdictSystemTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — census and collision check | Read 18 rows, loader, host flags, save and panel. | No old 6→15 target remains as current truth. | No production path until the owning implementation package is separately claimed. |
| 1 — reachability matrix | Trace each row to a location, phase, flag and visible action. | No orphan or misleading row remains. | No production path until the owning implementation package is separately claimed. |
| 2 — one-shot/replay proof | Exercise repeat, wrong-site, restore and migration paths. | Exactly-once state survives reload. | No production path until the owning implementation package is separately claimed. |
| 3 — UI/precision pass | Review availability explanations and accessibility. | Presentation cannot bypass Core. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/verdict_npcs.json | READ ONLY; MODIFY only for a proven coverage/consumer gap | 18-row authority |
| Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs | READ ONLY | Availability/one-shot owner |
| Assets/Ashfall.Core/Verdict/VerdictSave.cs | READ ONLY | Existing save envelope |
| src/VerdictPanel.cs | READ ONLY; MODIFY only under a new UI claim | Current presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Adding a second NPC state collection. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Inferring gates from display order. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Losing one-shot state during Verdict migration. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Treating a dialogue line as a gameplay effect without a typed fact. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new NPC system.
- No new Verdict save section.
- No arbitrary additional NPC count.
- No production/data/test/UI changes in this planning package.

# 23. Rollback and Recovery

- Revert the planning document.
- Future host/UI changes retain current Verdict save fixtures and one-shot tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 18 current rows and their gate fields are documented.
- Host availability, one-shot state and save migration are explicit.
- No parallel NPC/Verdict authority is proposed.
- Focused tests and negative cases are named.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 6→15 count target with an 18-row current census and a site/phase/flag/kind coverage matrix.
- Trace each row from the current Verdict location and progress flag to an available host projection and a visible dialogue action.
- Audit one-shot behavior across repeated clicks, save/restore, wrong location and changed phase; no new NPC state is needed for this residual.
- Preserve VerdictSave v1–v4 migration and the current host flag materialization rules.

## MUST NOT DO

- No new NPC system.
- No new Verdict save section.
- No arbitrary additional NPC count.
- No production/data/test/UI changes in this planning package.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/VerdictSystemTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — census and collision check — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: NPC catalog parsing and registration → VerdictNpcCatalogLoader; availability, one-shot speech and NPC state → VerdictNpcSystem; live progress flags and host projection → VerdictHostSession; NPC persistence and version migration → VerdictSaveCodec; player-visible encounter and diagnostic surface → Verdict UI/CLI; row coverage, gates, one-shot and migration proof → Verdict focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 93.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 93 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by VerdictNpcCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs`

### `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 155 lines / 5915 bytes.
- SHA-256: `09f7ee1e6d99e55b7559acda53ff56389392ff3ae5cf9ca2069d4b3db2a28ca4`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class VerdictNpcEntry
public string id = string.Empty;
public string name = string.Empty;
public string role = string.Empty;
public string kind = "paper_ghost";  // tape_echo | paper_ghost | living | readings
public string gatingFlag = string.Empty;
public string locationId = string.Empty;
public int phaseMin = 1;
public List<string> dialogue = new List<string>();
public class VerdictNpcState
public List<string> spokenNpcIds = new List<string>();
public sealed class VerdictNpcSystem
public VerdictNpcState State => _state;
public IReadOnlyList<VerdictNpcEntry> Catalog => _catalog;
public event Action<VerdictNpcEntry> OnSpoken;
public void Register(VerdictNpcEntry entry) {
public VerdictNpcEntry? Find(string id) {
public List<VerdictNpcEntry> GetAvailable( IReadOnlyCollection<string> setFlags, int phase, string? locationId = null) {
public bool Speak(string npcId, string? locationId = null) {
public VerdictNpcState CaptureState() {
public void RestoreState(VerdictNpcState state) {
public static class VerdictNpcCatalogLoader
public const string FileName = "verdict_npcs.json";
public static int LoadAndRegister(VerdictNpcSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Verdict/VerdictSave.cs`

### `Assets/Ashfall.Core/Verdict/VerdictSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 272 lines / 12122 bytes.
- SHA-256: `35bd29e9933555d17a90114245b9395ee98ec22077e7d46e2467dd185f603806`.
- Architecture signals: seeded references=0; save/restore symbols=14; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class VerdictSave
public const int CurrentSaveVersion = 4;
public const int MigrationFromVersion = 1;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public MachineLogSystemState machineLog = new MachineLogSystemState();
public ReckoningState reckoning = new ReckoningState();
public EvidenceLedgerState evidence = new EvidenceLedgerState();
public VerdictNpcState npcs = new VerdictNpcState();
public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
public QuestlineSystemState quests = new QuestlineSystemState();
public int censusLastWindowDay = -1;
public VerdictAccusationState accusations = new VerdictAccusationState();
public string Checksum = string.Empty;
public class VerdictSaveV3
public int saveVersion = 3;
public int simDay;
public MachineLogSystemState machineLog = new MachineLogSystemState();
public ReckoningState reckoning = new ReckoningState();
public EvidenceLedgerState evidence = new EvidenceLedgerState();
public VerdictNpcState npcs = new VerdictNpcState();
public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
public QuestlineSystemState quests = new QuestlineSystemState();
public int censusLastWindowDay = -1;
public string Checksum = string.Empty;
public class VerdictSaveV1
public int saveVersion = 1;
public int simDay;
public MachineLogSystemState machineLog = new MachineLogSystemState();
public ReckoningState reckoning = new ReckoningState();
public EvidenceLedgerState evidence = new EvidenceLedgerState();
public int censusLastWindowDay = -1;
public string Checksum = string.Empty;
public class VerdictSaveV2
public int saveVersion = 2;
public int simDay;
public MachineLogSystemState machineLog = new MachineLogSystemState();
public ReckoningState reckoning = new ReckoningState();
public EvidenceLedgerState evidence = new EvidenceLedgerState();
public VerdictNpcState npcs = new VerdictNpcState();
public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
public int censusLastWindowDay = -1;
public string Checksum = string.Empty;
public static class VerdictSaveCodec
public static VerdictSave Capture( int simDay, MachineLogSystem machineLog, ReckoningSystem reckoning, EvidenceLedger evidence, int censusLastWindowDay,
public static string Encode(VerdictSave save, IJsonSerializer json) {
public static bool TryDecode(string json, IJsonSerializer serializer, out VerdictSave save) {
public static void Restore( VerdictSave save, MachineLogSystem machineLog, ReckoningSystem reckoning, EvidenceLedger evidence, VerdictNpcSystem? npcs = null,
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs`

### `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 225 lines / 10025 bytes.
- SHA-256: `abee1d19deb019bb058c9435069600549265c68836d2723c607fccd88c23d334`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class VerdictCatalogLoader
public const string DataFile = "verdict_data.json";
public const string LocationsFile = "verdict_locations.json";
public const string ItemsFile = "verdict_items.json";
public const string RadioFile = "verdict_radio.json";
public class VerdictLocationEntry
public string id = string.Empty;
public string displayName = string.Empty;
public string description = string.Empty;
public int dangerLevel = 5;
public float travelHours = 5f;
public float baseRadsPerHour = 30f;
public static List<VerdictLocationEntry> LoadLocations( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public class VerdictItemEffects
public int enrolled_evidence;
public string note = string.Empty;
public class VerdictItemEntry
public string id = string.Empty;
public string displayName = string.Empty;
public float weightKg;
public float tradeValue;
public string category = "story_item";
public string tier = string.Empty;
public string description = string.Empty;
public VerdictItemEffects mechanical_effects = null!;
public string downstream_quest_trigger = string.Empty;
public string faction_affinity = string.Empty;
public string rarity = string.Empty;
public static List<VerdictItemEntry> LoadItems( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public class VerdictRadioEntry
public string id = string.Empty;
public string frequency = string.Empty;
public int dayTrigger = 180;
public string source = string.Empty;
public string message = string.Empty;
public string signalStrength = string.Empty;
public string kind = "telemetry";
public string audio_cue = string.Empty;
public static List<VerdictRadioEntry> LoadRadio( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public class VerdictWorldHistoryLadderEntry
public int layer { get; set; }
public string knowledge_key { get; set; } = string.Empty;
public string title { get; set; } = string.Empty;
public string discovery_location_id { get; set; } = string.Empty;
public string body_summary { get; set; } = string.Empty;
public List<string> corruption_corpus = new List<string>();
public List<VerdictWorldHistoryLadderEntry> world_history_ladder = new List<VerdictWorldHistoryLadderEntry>();
public static List<string> LoadCorruptionCorpus( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<VerdictWorldHistoryLadderEntry> LoadWorldHistoryLadder( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public List<VerdictRadioEntry> broadcasts = new List<VerdictRadioEntry>();
```


# Appendix B.05 — Current Code Architecture: `src/Host/VerdictHostSession.cs`

### `src/Host/VerdictHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 284 lines / 14368 bytes.
- SHA-256: `eb9abc73b3557cdfa1d7b975f426399da4cbf2e299387681232521df4b1f6f8b`.
- Architecture signals: seeded references=4; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class VerdictHostSession
public MachineLogSystem MachineLog { get; }
public ReckoningSystem Reckoning { get; }
public EvidenceLedger Evidence { get; }
public VerdictEvidenceChain EvidenceChain { get; }
public VerdictNpcSystem Npcs { get; }
public VerdictCensusBroadcast Census { get; }
public VerdictRadioSystem Radio { get; internal set; }
public QuestlineSystem Quests { get; }
public IReadOnlyList<VerdictCatalogLoader.VerdictLocationEntry> Locations { get; }
public IReadOnlyList<VerdictCatalogLoader.VerdictItemEntry> Items { get; }
public IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> RadioEntries { get; }
public IReadOnlyList<string> CorruptionCorpus { get; private set; }
public System.Collections.Generic.HashSet<string> MaterializedNpcFlags() {
public System.Collections.Generic.List<Ashfall.Core.Verdict.VerdictNpcEntry> AvailableNpcs(string locationId = null!) {
public string LastEvent { get; private set; } = string.Empty;
public static VerdictHostSession Create( string dataDir, ISimClock clock = null!, IEventBus bus = null!, IFlagLedger flags = null!, ISeededRng radioRng = null!,
public int LoadedSaveVersion { get; private set; }
public bool WasSaveMigrated { get; private set; }
public void AdvanceDay(int day, int livingCount, int logReadCount) {
public void TickCensus() {
public System.Collections.Generic.List<string> TickRadio(int day) {
public int EnrollEvidenceFromItems(int day) {
public void TickCorruption(int day) {
public VerdictSave CaptureSave() {
public void RestoreSave(VerdictSave save) {
public string StatusLine() {
public VerdictCatalogLoader.VerdictLocationEntry? FindLocation(string id) {
```


# Appendix B.06 — Current Code Architecture: `src/VerdictPanel.cs`

### `src/VerdictPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 432 lines / 17793 bytes.
- SHA-256: `880c980f41d127d45df8a9b2d4b42bb34d01603c53f4015a86e39b45345af50e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=1; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class VerdictPanel : PanelContainer
public event System.Action? OnClose;
public void Open() {
public void Close() {
public override void _UnhandledInput(InputEvent @event) {
public override void _Ready() {
public void Bind(VerdictHostSession verdict) {
public void RefreshView() {
public int RenderedRadioRowCount() {
public delegate void NpcSpokenEventHandler(string npcId);
public override void _ExitTree() {
```


# Appendix B.07 — Current Code Architecture: `src/Host/HostCli.ExpansionDepth.cs`

### `src/Host/HostCli.ExpansionDepth.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 144 lines / 7946 bytes.
- SHA-256: `c2e979fba2fe17db74263330c5ba2382dfc878788a22ca81b8df117457f89ff8`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunExpansionDepthSelfTest(string dataDirectory) {
public int schema_version { get; set; }
public List<VerdictQuestlineDef>? quests { get; set; }
public string questlineId { get; set; } = string.Empty;
public string title { get; set; } = string.Empty;
public int schema_version { get; set; }
public List<VerdictNpcDef>? items { get; set; }
public string id { get; set; } = string.Empty;
public string name { get; set; } = string.Empty;
public int schema_version { get; set; }
public List<QuestlineMasterEntryDef>? entries { get; set; }
public string id { get; set; } = string.Empty;
```


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/verdict_npcs.json`

### `Assets/StreamingAssets/Data/verdict_npcs.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 9832 bytes / 9832 characters.
- SHA-256: `ace1edded901844ae68ebbaad7f913990316ab155888ccee2c531766be7cbadc`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=18, max=18, observed_paths=1
items[].dialogue: min=2, max=2, observed_paths=2
```

Representative record fields:

- `dialogue`
- `gating_flag`
- `id`
- `kind`
- `location_id`
- `name`
- `phase_min`
- `role`

Representative identifiers (ordered, capped for readability):

```text
npc_eden_vale
npc_ferris_voss
npc_iran_bell
npc_selya_saltmarsh
npc_maro_veen
npc_whisper_cipher
npc_tomas_reid
npc_elena_vane
npc_kasper_holt
npc_mara_elsen
npc_ilya_venn
npc_garrick_daal
npc_sena_korr
npc_torin_rask
npc_oren_varek
npc_lena_rost
npc_tessa_mirn
npc_karel_norn
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/verdict_locations.json`

### `Assets/StreamingAssets/Data/verdict_locations.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 14430 bytes / 14416 characters.
- SHA-256: `e97faf513dfa2e3f6b9cfb7fd1311cea6534a283e7d81e099784232b9d9380d0`.
- Root keys: `locations`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
locations: min=15, max=15, observed_paths=1
```

Representative record fields:

- `baseRadsPerHour`
- `dangerLevel`
- `description`
- `displayName`
- `id`
- `travelHours`

Representative identifiers (ordered, capped for readability):

```text
loc_geophone_pit_1
loc_twelve_gauge_array
loc_network_fuse_bunker
loc_archive_tape_silo
loc_abandoned_tide_gauge
loc_coastal_meteorological_station
loc_clifftop_observation_bunker
loc_sealed_marine_laboratory
loc_forestry_survey_post
loc_geological_core_vault
loc_river_gauging_station
loc_abandoned_agricultural_station
loc_decommissioned_signal_relay
loc_border_checkpoint_ruins
loc_minefield_observation_tower
```


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/verdict_data.json`

### `Assets/StreamingAssets/Data/verdict_data.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 9814 bytes / 9776 characters.
- SHA-256: `2c7ef4992e16b4d32e0a3a12af42f4d12bf8460e51a0c69eadc8e0d42799dae2`.
- Root keys: `catalog`, `corruption_corpus`, `currencies`, `description`, `endings`, `facets`, `readout_steps`, `schema_version`, `world_history_ladder`.

Array-path census (minimum, maximum, observed rows):

```text
corruption_corpus: min=25, max=25, observed_paths=1
currencies: min=1, max=1, observed_paths=1
endings: min=3, max=3, observed_paths=1
facets: min=3, max=3, observed_paths=1
facets[].bylines: min=5, max=5, observed_paths=2
readout_steps: min=4, max=4, observed_paths=1
world_history_ladder: min=12, max=12, observed_paths=1
```

Representative record fields:

- `id`
- `label`
- `note`

Representative identifiers (ordered, capped for readability):

```text
enrolled_evidence
```


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs`

### `Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 240; SHA-256: `c63225a0ebdbc8bdb493a8de624bf65d6e098095b7a9bfd5c940b2e30957c6e2`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_Loads_All_18_Npc_Entries
All_18_Npc_Ids_Are_Unique_And_Prefixed
Original_6_Baseline_Npcs_Preserved
Plan18_Tribunal_Npcs_Preserved
All_9_Plan93_Investigation_Npcs_Present
All_Npc_Kinds_Are_Supported
All_Plan93_LocationIds_Map_To_Distinct_Verdict_Sites
GetAvailable_Filters_By_Phase_And_Flag_And_Location
Speak_Is_OneShot_And_Persists_In_State
Availability_Is_Deterministic_Across_Invocations
```


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs`

### `Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 217; SHA-256: `4fc5a94aeab9132f1282222f25b67d05edb8cb60f9a05cfee8b41b23b505087e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan93_VerdictNpcCatalog_LoadsAll18Npcs_WithValidGatingAndDialogue
Plan101_DoseQuestCatalog_LoadsAll12Questlines_WithValidTransitions
CrossSystem_VerdictArchivistsAndDosimetryQuests_ExhibitNarrativeCoherence
CrossSystem_DeterministicExecution_UnderSimulationPasses
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/VerdictSystemTests.cs`

### `Ashfall.Core.Tests/VerdictSystemTests.cs`

- Current test declarations: Fact=54, Theory=0, InlineData=0.
- File lines: 723; SHA-256: `f25ace785950670bd5a6ef2436b246b418a95a82c41be19a2745faf76a4bbd06`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Evidence_Enroll_IsIdempotent
Evidence_Enroll_RejectsUnknown_WhenCatalogPopulated
Evidence_Enroll_AllowsAny_WhenCatalogEmpty
Evidence_FiresEvent
Evidence_CaptureRestore_Roundtrip
Evidence_RejectNullEmpty
MachineLog_Post_DuplicateSuppression
MachineLog_Post_DifferentKind_Allowed
MachineLog_ReadEntry_OneWay
MachineLog_ReadEntry_OutOfRange
MachineLog_CorruptionMarker_Deterministic
MachineLog_SpinTape_OnePerDay
MachineLog_CaptureRestore_Roundtrip
MachineLog_CaptureRestore_DoesNotAliasEntries
VerdictEvidenceChain_ReadEnrollsLedgerAndReckoningExactlyOnce
VerdictEvidenceChain_ReconcileIsSafeAfterSaveRestore
VerdictEvidenceChain_ReconcileRepairsDerivedReckoningCount
MachineLog_Post_RejectsEmptyFacility
Reckoning_Dormant_BeforeDay160
Reckoning_Knowing_AtDay160
Reckoning_Culpable_NeedsEvidence
Reckoning_Counted_AtDay240
Reckoning_CallIsOneShot
Reckoning_NeverReverses
Reckoning_SelectEnding_MutuallyExclusive
Reckoning_SelectEnding_RejectsBeforeCounted
Reckoning_SelectEnding_RejectsUnknown
Reckoning_CensusWindow_OpenInCulpable
Reckoning_CaptureRestore_Roundtrip
EndingEvaluator_ResolvedEnding_Priority
EndingEvaluator_NullState_ReturnsNull
EndingEvaluator_DecideEnding_FallsBackByEvidence
EndingEvaluator_DecideEnding_NullBeforeCounted
EndingEvaluator_TempestDecommissioned_OnlyOnRecount
Readout_Dormant_WhenStateNull
Readout_Knowing_InPhase
Readout_NegativeOrOverflowingCounters_StayBounded
Readout_Resolved_WhenCountPresented
Npc_Register_Find
Npc_Speak_OneShot
Npc_GetAvailable_RespectsPhase
Npc_GetAvailable_RespectsGatingFlag
Npc_CaptureRestore_Roundtrip
Save_CaptureEncode_DecodeRestore_Roundtrip
Save_TamperRejection
Save_RejectsEmptyChecksum
Save_RejectsNewerVersion
Census_WindowOpen_Every7DaysAt03
Census_BroadcastOnce_PerWindow
Census_SilentAfterSigning
Census_CanonConstants
CatalogLoader_Locations_ReturnsEmpty_WhenFileMissing
CatalogLoader_Locations_ReturnsEmpty_WhenNullArgs
VerdictItemsJson_MatchesRuntimeSchema
```


# Appendix E.14 — Supporting Code Evidence: `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs`

### `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 155 lines / 5915 bytes.
- SHA-256: `09f7ee1e6d99e55b7559acda53ff56389392ff3ae5cf9ca2069d4b3db2a28ca4`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class VerdictNpcEntry
public string id = string.Empty;
public string name = string.Empty;
public string role = string.Empty;
public string kind = "paper_ghost";  // tape_echo | paper_ghost | living | readings
public string gatingFlag = string.Empty;
public string locationId = string.Empty;
public int phaseMin = 1;
public List<string> dialogue = new List<string>();
public class VerdictNpcState
public List<string> spokenNpcIds = new List<string>();
public sealed class VerdictNpcSystem
public VerdictNpcState State => _state;
public IReadOnlyList<VerdictNpcEntry> Catalog => _catalog;
public event Action<VerdictNpcEntry> OnSpoken;
public void Register(VerdictNpcEntry entry) {
public VerdictNpcEntry? Find(string id) {
public List<VerdictNpcEntry> GetAvailable( IReadOnlyCollection<string> setFlags, int phase, string? locationId = null) {
public bool Speak(string npcId, string? locationId = null) {
public VerdictNpcState CaptureState() {
public void RestoreState(VerdictNpcState state) {
public static class VerdictNpcCatalogLoader
public const string FileName = "verdict_npcs.json";
public static int LoadAndRegister(VerdictNpcSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Verdict/VerdictSave.cs`

### `Assets/Ashfall.Core/Verdict/VerdictSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 272 lines / 12122 bytes.
- SHA-256: `35bd29e9933555d17a90114245b9395ee98ec22077e7d46e2467dd185f603806`.
- Architecture signals: seeded references=0; save/restore symbols=14; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class VerdictSave
public const int CurrentSaveVersion = 4;
public const int MigrationFromVersion = 1;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public MachineLogSystemState machineLog = new MachineLogSystemState();
public ReckoningState reckoning = new ReckoningState();
public EvidenceLedgerState evidence = new EvidenceLedgerState();
public VerdictNpcState npcs = new VerdictNpcState();
public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
public QuestlineSystemState quests = new QuestlineSystemState();
public int censusLastWindowDay = -1;
public VerdictAccusationState accusations = new VerdictAccusationState();
public string Checksum = string.Empty;
public class VerdictSaveV3
public int saveVersion = 3;
public int simDay;
public MachineLogSystemState machineLog = new MachineLogSystemState();
public ReckoningState reckoning = new ReckoningState();
public EvidenceLedgerState evidence = new EvidenceLedgerState();
public VerdictNpcState npcs = new VerdictNpcState();
public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
public QuestlineSystemState quests = new QuestlineSystemState();
public int censusLastWindowDay = -1;
public string Checksum = string.Empty;
public class VerdictSaveV1
public int saveVersion = 1;
public int simDay;
public MachineLogSystemState machineLog = new MachineLogSystemState();
public ReckoningState reckoning = new ReckoningState();
public EvidenceLedgerState evidence = new EvidenceLedgerState();
public int censusLastWindowDay = -1;
public string Checksum = string.Empty;
public class VerdictSaveV2
public int saveVersion = 2;
public int simDay;
public MachineLogSystemState machineLog = new MachineLogSystemState();
public ReckoningState reckoning = new ReckoningState();
public EvidenceLedgerState evidence = new EvidenceLedgerState();
public VerdictNpcState npcs = new VerdictNpcState();
public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
public int censusLastWindowDay = -1;
public string Checksum = string.Empty;
public static class VerdictSaveCodec
public static VerdictSave Capture( int simDay, MachineLogSystem machineLog, ReckoningSystem reckoning, EvidenceLedger evidence, int censusLastWindowDay,
public static string Encode(VerdictSave save, IJsonSerializer json) {
public static bool TryDecode(string json, IJsonSerializer serializer, out VerdictSave save) {
public static void Restore( VerdictSave save, MachineLogSystem machineLog, ReckoningSystem reckoning, EvidenceLedger evidence, VerdictNpcSystem? npcs = null,
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs`

### `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 225 lines / 10025 bytes.
- SHA-256: `abee1d19deb019bb058c9435069600549265c68836d2723c607fccd88c23d334`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class VerdictCatalogLoader
public const string DataFile = "verdict_data.json";
public const string LocationsFile = "verdict_locations.json";
public const string ItemsFile = "verdict_items.json";
public const string RadioFile = "verdict_radio.json";
public class VerdictLocationEntry
public string id = string.Empty;
public string displayName = string.Empty;
public string description = string.Empty;
public int dangerLevel = 5;
public float travelHours = 5f;
public float baseRadsPerHour = 30f;
public static List<VerdictLocationEntry> LoadLocations( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public class VerdictItemEffects
public int enrolled_evidence;
public string note = string.Empty;
public class VerdictItemEntry
public string id = string.Empty;
public string displayName = string.Empty;
public float weightKg;
public float tradeValue;
public string category = "story_item";
public string tier = string.Empty;
public string description = string.Empty;
public VerdictItemEffects mechanical_effects = null!;
public string downstream_quest_trigger = string.Empty;
public string faction_affinity = string.Empty;
public string rarity = string.Empty;
public static List<VerdictItemEntry> LoadItems( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public class VerdictRadioEntry
public string id = string.Empty;
public string frequency = string.Empty;
public int dayTrigger = 180;
public string source = string.Empty;
public string message = string.Empty;
public string signalStrength = string.Empty;
public string kind = "telemetry";
public string audio_cue = string.Empty;
public static List<VerdictRadioEntry> LoadRadio( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public class VerdictWorldHistoryLadderEntry
public int layer { get; set; }
public string knowledge_key { get; set; } = string.Empty;
public string title { get; set; } = string.Empty;
public string discovery_location_id { get; set; } = string.Empty;
public string body_summary { get; set; } = string.Empty;
public List<string> corruption_corpus = new List<string>();
public List<VerdictWorldHistoryLadderEntry> world_history_ladder = new List<VerdictWorldHistoryLadderEntry>();
public static List<string> LoadCorruptionCorpus( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<VerdictWorldHistoryLadderEntry> LoadWorldHistoryLadder( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public List<VerdictRadioEntry> broadcasts = new List<VerdictRadioEntry>();
```


# Appendix E.17 — Supporting Code Evidence: `src/Host/VerdictHostSession.cs`

### `src/Host/VerdictHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 284 lines / 14368 bytes.
- SHA-256: `eb9abc73b3557cdfa1d7b975f426399da4cbf2e299387681232521df4b1f6f8b`.
- Architecture signals: seeded references=4; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class VerdictHostSession
public MachineLogSystem MachineLog { get; }
public ReckoningSystem Reckoning { get; }
public EvidenceLedger Evidence { get; }
public VerdictEvidenceChain EvidenceChain { get; }
public VerdictNpcSystem Npcs { get; }
public VerdictCensusBroadcast Census { get; }
public VerdictRadioSystem Radio { get; internal set; }
public QuestlineSystem Quests { get; }
public IReadOnlyList<VerdictCatalogLoader.VerdictLocationEntry> Locations { get; }
public IReadOnlyList<VerdictCatalogLoader.VerdictItemEntry> Items { get; }
public IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> RadioEntries { get; }
public IReadOnlyList<string> CorruptionCorpus { get; private set; }
public System.Collections.Generic.HashSet<string> MaterializedNpcFlags() {
public System.Collections.Generic.List<Ashfall.Core.Verdict.VerdictNpcEntry> AvailableNpcs(string locationId = null!) {
public string LastEvent { get; private set; } = string.Empty;
public static VerdictHostSession Create( string dataDir, ISimClock clock = null!, IEventBus bus = null!, IFlagLedger flags = null!, ISeededRng radioRng = null!,
public int LoadedSaveVersion { get; private set; }
public bool WasSaveMigrated { get; private set; }
public void AdvanceDay(int day, int livingCount, int logReadCount) {
public void TickCensus() {
public System.Collections.Generic.List<string> TickRadio(int day) {
public int EnrollEvidenceFromItems(int day) {
public void TickCorruption(int day) {
public VerdictSave CaptureSave() {
public void RestoreSave(VerdictSave save) {
public string StatusLine() {
public VerdictCatalogLoader.VerdictLocationEntry? FindLocation(string id) {
```


# Appendix E.18 — Supporting Code Evidence: `src/VerdictPanel.cs`

### `src/VerdictPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 432 lines / 17793 bytes.
- SHA-256: `880c980f41d127d45df8a9b2d4b42bb34d01603c53f4015a86e39b45345af50e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=1; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class VerdictPanel : PanelContainer
public event System.Action? OnClose;
public void Open() {
public void Close() {
public override void _UnhandledInput(InputEvent @event) {
public override void _Ready() {
public void Bind(VerdictHostSession verdict) {
public void RefreshView() {
public int RenderedRadioRowCount() {
public delegate void NpcSpokenEventHandler(string npcId);
public override void _ExitTree() {
```


# Appendix G.19 — Supporting Regression Evidence: `Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs`

### `Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 240; SHA-256: `c63225a0ebdbc8bdb493a8de624bf65d6e098095b7a9bfd5c940b2e30957c6e2`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_Loads_All_18_Npc_Entries
All_18_Npc_Ids_Are_Unique_And_Prefixed
Original_6_Baseline_Npcs_Preserved
Plan18_Tribunal_Npcs_Preserved
All_9_Plan93_Investigation_Npcs_Present
All_Npc_Kinds_Are_Supported
All_Plan93_LocationIds_Map_To_Distinct_Verdict_Sites
GetAvailable_Filters_By_Phase_And_Flag_And_Location
Speak_Is_OneShot_And_Persists_In_State
Availability_Is_Deterministic_Across_Invocations
```


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs`

### `Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 217; SHA-256: `4fc5a94aeab9132f1282222f25b67d05edb8cb60f9a05cfee8b41b23b505087e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan93_VerdictNpcCatalog_LoadsAll18Npcs_WithValidGatingAndDialogue
Plan101_DoseQuestCatalog_LoadsAll12Questlines_WithValidTransitions
CrossSystem_VerdictArchivistsAndDosimetryQuests_ExhibitNarrativeCoherence
CrossSystem_DeterministicExecution_UnderSimulationPasses
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/VerdictSystemTests.cs`

### `Ashfall.Core.Tests/VerdictSystemTests.cs`

- Current test declarations: Fact=54, Theory=0, InlineData=0.
- File lines: 723; SHA-256: `f25ace785950670bd5a6ef2436b246b418a95a82c41be19a2745faf76a4bbd06`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Evidence_Enroll_IsIdempotent
Evidence_Enroll_RejectsUnknown_WhenCatalogPopulated
Evidence_Enroll_AllowsAny_WhenCatalogEmpty
Evidence_FiresEvent
Evidence_CaptureRestore_Roundtrip
Evidence_RejectNullEmpty
MachineLog_Post_DuplicateSuppression
MachineLog_Post_DifferentKind_Allowed
MachineLog_ReadEntry_OneWay
MachineLog_ReadEntry_OutOfRange
MachineLog_CorruptionMarker_Deterministic
MachineLog_SpinTape_OnePerDay
MachineLog_CaptureRestore_Roundtrip
MachineLog_CaptureRestore_DoesNotAliasEntries
VerdictEvidenceChain_ReadEnrollsLedgerAndReckoningExactlyOnce
VerdictEvidenceChain_ReconcileIsSafeAfterSaveRestore
VerdictEvidenceChain_ReconcileRepairsDerivedReckoningCount
MachineLog_Post_RejectsEmptyFacility
Reckoning_Dormant_BeforeDay160
Reckoning_Knowing_AtDay160
Reckoning_Culpable_NeedsEvidence
Reckoning_Counted_AtDay240
Reckoning_CallIsOneShot
Reckoning_NeverReverses
Reckoning_SelectEnding_MutuallyExclusive
Reckoning_SelectEnding_RejectsBeforeCounted
Reckoning_SelectEnding_RejectsUnknown
Reckoning_CensusWindow_OpenInCulpable
Reckoning_CaptureRestore_Roundtrip
EndingEvaluator_ResolvedEnding_Priority
EndingEvaluator_NullState_ReturnsNull
EndingEvaluator_DecideEnding_FallsBackByEvidence
EndingEvaluator_DecideEnding_NullBeforeCounted
EndingEvaluator_TempestDecommissioned_OnlyOnRecount
Readout_Dormant_WhenStateNull
Readout_Knowing_InPhase
Readout_NegativeOrOverflowingCounters_StayBounded
Readout_Resolved_WhenCountPresented
Npc_Register_Find
Npc_Speak_OneShot
Npc_GetAvailable_RespectsPhase
Npc_GetAvailable_RespectsGatingFlag
Npc_CaptureRestore_Roundtrip
Save_CaptureEncode_DecodeRestore_Roundtrip
Save_TamperRejection
Save_RejectsEmptyChecksum
Save_RejectsNewerVersion
Census_WindowOpen_Every7DaysAt03
Census_BroadcastOnce_PerWindow
Census_SilentAfterSigning
Census_CanonConstants
CatalogLoader_Locations_ReturnsEmpty_WhenFileMissing
CatalogLoader_Locations_ReturnsEmpty_WhenNullArgs
VerdictItemsJson_MatchesRuntimeSchema
```


# Appendix H.22 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

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
| NPC catalog parsing and registration | VerdictNpcCatalogLoader | availability, one-shot speech and NPC state | VerdictNpcSystem | Owner emits/reads a typed fact; no mirror state. |
| NPC catalog parsing and registration | VerdictNpcCatalogLoader | live progress flags and host projection | VerdictHostSession | Owner emits/reads a typed fact; no mirror state. |
| NPC catalog parsing and registration | VerdictNpcCatalogLoader | NPC persistence and version migration | VerdictSaveCodec | Owner emits/reads a typed fact; no mirror state. |
| NPC catalog parsing and registration | VerdictNpcCatalogLoader | player-visible encounter and diagnostic surface | Verdict UI/CLI | Owner emits/reads a typed fact; no mirror state. |
| NPC catalog parsing and registration | VerdictNpcCatalogLoader | row coverage, gates, one-shot and migration proof | Verdict focused tests | Owner emits/reads a typed fact; no mirror state. |
| availability, one-shot speech and NPC state | VerdictNpcSystem | NPC catalog parsing and registration | VerdictNpcCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| availability, one-shot speech and NPC state | VerdictNpcSystem | live progress flags and host projection | VerdictHostSession | Owner emits/reads a typed fact; no mirror state. |
| availability, one-shot speech and NPC state | VerdictNpcSystem | NPC persistence and version migration | VerdictSaveCodec | Owner emits/reads a typed fact; no mirror state. |
| availability, one-shot speech and NPC state | VerdictNpcSystem | player-visible encounter and diagnostic surface | Verdict UI/CLI | Owner emits/reads a typed fact; no mirror state. |
| availability, one-shot speech and NPC state | VerdictNpcSystem | row coverage, gates, one-shot and migration proof | Verdict focused tests | Owner emits/reads a typed fact; no mirror state. |
| live progress flags and host projection | VerdictHostSession | NPC catalog parsing and registration | VerdictNpcCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| live progress flags and host projection | VerdictHostSession | availability, one-shot speech and NPC state | VerdictNpcSystem | Owner emits/reads a typed fact; no mirror state. |
| live progress flags and host projection | VerdictHostSession | NPC persistence and version migration | VerdictSaveCodec | Owner emits/reads a typed fact; no mirror state. |
| live progress flags and host projection | VerdictHostSession | player-visible encounter and diagnostic surface | Verdict UI/CLI | Owner emits/reads a typed fact; no mirror state. |
| live progress flags and host projection | VerdictHostSession | row coverage, gates, one-shot and migration proof | Verdict focused tests | Owner emits/reads a typed fact; no mirror state. |
| NPC persistence and version migration | VerdictSaveCodec | NPC catalog parsing and registration | VerdictNpcCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| NPC persistence and version migration | VerdictSaveCodec | availability, one-shot speech and NPC state | VerdictNpcSystem | Owner emits/reads a typed fact; no mirror state. |
| NPC persistence and version migration | VerdictSaveCodec | live progress flags and host projection | VerdictHostSession | Owner emits/reads a typed fact; no mirror state. |
| NPC persistence and version migration | VerdictSaveCodec | player-visible encounter and diagnostic surface | Verdict UI/CLI | Owner emits/reads a typed fact; no mirror state. |
| NPC persistence and version migration | VerdictSaveCodec | row coverage, gates, one-shot and migration proof | Verdict focused tests | Owner emits/reads a typed fact; no mirror state. |
| player-visible encounter and diagnostic surface | Verdict UI/CLI | NPC catalog parsing and registration | VerdictNpcCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| player-visible encounter and diagnostic surface | Verdict UI/CLI | availability, one-shot speech and NPC state | VerdictNpcSystem | Owner emits/reads a typed fact; no mirror state. |
| player-visible encounter and diagnostic surface | Verdict UI/CLI | live progress flags and host projection | VerdictHostSession | Owner emits/reads a typed fact; no mirror state. |
| player-visible encounter and diagnostic surface | Verdict UI/CLI | NPC persistence and version migration | VerdictSaveCodec | Owner emits/reads a typed fact; no mirror state. |
| player-visible encounter and diagnostic surface | Verdict UI/CLI | row coverage, gates, one-shot and migration proof | Verdict focused tests | Owner emits/reads a typed fact; no mirror state. |
| row coverage, gates, one-shot and migration proof | Verdict focused tests | NPC catalog parsing and registration | VerdictNpcCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| row coverage, gates, one-shot and migration proof | Verdict focused tests | availability, one-shot speech and NPC state | VerdictNpcSystem | Owner emits/reads a typed fact; no mirror state. |
| row coverage, gates, one-shot and migration proof | Verdict focused tests | live progress flags and host projection | VerdictHostSession | Owner emits/reads a typed fact; no mirror state. |
| row coverage, gates, one-shot and migration proof | Verdict focused tests | NPC persistence and version migration | VerdictSaveCodec | Owner emits/reads a typed fact; no mirror state. |
| row coverage, gates, one-shot and migration proof | Verdict focused tests | player-visible encounter and diagnostic surface | Verdict UI/CLI | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 6→15 count target with an 18-row current census and a site/phase/flag/kind coverage matrix. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Trace each row from the current Verdict location and progress flag to an available host projection and a visible dialogue action. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Audit one-shot behavior across repeated clicks, save/restore, wrong location and changed phase; no new NPC state is needed for this residual. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Preserve VerdictSave v1–v4 migration and the current host flag materialization rules. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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

> **DR-06 — Integration ledger state differs from the v1.0 queue snapshot. VERIFIED.**
Live `INTEGRATION_PLANS.md` (read 2026-09-24) shows, at minimum: the **XP Expansion W1** batch ACTIVE (difficulty authority package `XP-WAVE1-DIFFICULTY-AUTHORITY`, with a premise correction recorded against Plan 122 SOFC fuel); the **DISTRESS-SIGNALS-9-12** flagship COMPLETE and presented for acceptance; **Plan 24 CLOSED** (Wave 8, 2026-09-17, signatures resolved 2026-09-18, ward staffing sealed under option b, `DEBT-PLAN24-MEDICAL-WARD-STAFFING` RETIRED); **19A/19B/19C** waves closed with evidence (Endgame 84/84 PASS, focused suites 47 PASS, `verify-fast.sh` reported ALL 47 GATES PASSED); **C2[2] Plan 17** legibility executed with a documented not-executed list (Plan 31 semantic-kind authority, 17C audio phases, 17B deep test matrix remain open gaps); **PR 3 content seal SEALED 2026-09-19** (`CF-P1-DISTRESS-CONTENT-SEAL`); the **availability consumer RETIRED** (Wave 9 Part 2, Option B approved; `SignalTrustAvailability` retained as a pure-math specification pin). Consequence: subject plans in the radio/distress domain must treat the rescue-signal runtime as *sealed and closed*, not as an open expansion surface, unless they extend it through its recorded seams.

> **DR-07 — Gate-count and test-total drift. HIGH CONFIDENCE.**
The v1.0 bible states 57 CI gates at v1.1.0 (53 fast + 3 full + 1 performance) and quotes both 11,098 and 11,697 full-suite totals from different handoffs. The live 19-wave closeout evidence in `INTEGRATION_PLANS.md` records `verify-fast.sh` ALL 47 GATES PASSED at that batch's close. These figures cannot all describe the same instant. Factory rule: any subject plan that names a gate count or test total must re-verify the number against the live gate inventory at drafting time and cite the closeout it came from. Never carry counts forward from this or any prior document.

> **DR-10 — v1.0 items the audit could not confirm in this pass. UNVERIFIED.**
Not confirmed in this audit pass (single-session, listing-level access): the 11,697 test total; the D1 seal state; the full 57-gate inventory; codec version pin values; the `ClaimPersonalBelonging` no-caller status; decision-blocked item states beyond those the ledger records as resolved. Each of these remains plausible but must be re-verified in live source before any plan depends on it. Factory rule: UNVERIFIED premises get a verification step inside the plan, never silent trust.

> The following v1.0 structures were confirmed by the audit and remain authoritative: the four-tier architecture (Tier 1 data authority in `Assets/StreamingAssets/Data/`; Tier 2 engine-free Core; Tier 3 `src/Host` + `src/UI`; Tier 4 xUnit plus the `HostCli` selftest surface); the `AGENTS.md` non-negotiable rules (Godot authoritative, Core engine-free, JSON authoritative, one authority per concern, focused verification); the narrative corpus under `Assets/StreamingAssets/Data/narrative/` (present in the live listing); the faction, economy, weather, Year-of-Ash, moral-choice, muster, and verdict catalog families (all present live); and the plan-discipline artifacts (`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `SESSION_HANDOFF.md`) at root.

> **Step 2 — Select exactly one lane and one subsystem cluster.**
From Part III. The selection rule is lane rotation discipline (v1.0 Part 11): at most one plan per lane per wave; data-first lanes (A, C, J) precede wiring lanes (B, D, E) within the same domain. The session states the lane and cluster in the plan header.

> **Step 4 — Draft the subject plan in the v2.0 subject-plan format (Part V, Template S).**
A subject plan is not an integration plan. It states WHAT should expand, WHY (with evidence), WHAT MUST NOT CHANGE, and WHICH INTEGRATION ROUTE the repository should prefer — but it does not prescribe line-level implementation. The integration route recommendation (Part V, Template R) names the tier (data-only / host wiring / Core extension), the seams, the save impact class, and the verification class. This preserves the repo's own separation: subject plans propose; integration plans (drafted later, against the live tree, in an owning session) commit.

> - One plan = one bounded outcome riding existing seams (v1.0 Part 10). The factory never widens a plan to reach a size target.
- No plan may create a parallel authority. Every state change names its owning system.
- Data-first preference: if an expansion can be authored as JSON through an existing loader, it must be, and the plan must say so.
- The factory never drafts against decision-blocked items (the current list must be re-read from `INTEGRATION_PLANS.md` each session — DR-06 shows signatures resolve over time).
- Subject plans do not edit files. Implementation happens only in an owning session after plan selection (v1.0 approval-based workflow).
- Every generated plan must state its position relative to each epilogue permutation it touches (v1.0 Part 6.6).

> Ten lanes (A–J, from v1.0 Part 11) against seventeen subsystem clusters distilled from the live Core inventory (v1.0 Parts 5.1–5.2 and 16, confirmed live). Each cell names an opening archetype. Confidence labels reflect the audit state as of 2026-09-24 and must be re-checked at drafting time. This matrix is the combinatorial engine: 170 cells, each capable of yielding multiple subject plans over time as content lands and seams mature. Not every cell is currently open; cells marked SEALED are closed by evidence (e.g., the distress-signal content seal, DR-06) and may not be opened without new evidence and foreman signature.

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

The subject is human residue inside the existing Verdict investigation authority: authored NPC rows, live progress flags, site/phase availability, one-shot speech and versioned persistence. The plan does not add a new investigation, faction or narrative authority.

- **NPC catalog parsing and registration** remains with `VerdictNpcCatalogLoader` at `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs`. Loads authored NPC rows; it does not decide availability from arbitrary UI state.
- **availability, one-shot speech and NPC state** remains with `VerdictNpcSystem` at `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs`. Owns `spokenNpcIds` and emits `OnSpoken` after a successful one-shot action.
- **live progress flags and host projection** remains with `VerdictHostSession` at `src/Host/VerdictHostSession.cs`. Maps current machine/evidence/reckoning progress to flags and exposes available NPCs.
- **NPC persistence and version migration** remains with `VerdictSaveCodec` at `Assets/Ashfall.Core/Verdict/VerdictSave.cs`. Current Verdict envelope owns NPC state; no Plan-93-specific section.
- **player-visible encounter and diagnostic surface** remains with `Verdict UI/CLI` at `src/VerdictPanel.cs; src/Host/HostCli.ExpansionDepth.cs`. Presentation/host projection only; it cannot mark an NPC spoken directly.
- **row coverage, gates, one-shot and migration proof** remains with `Verdict focused tests` at `Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs; Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs; Ashfall.Core.Tests/VerdictSystemTests.cs`. Focused evidence surface.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load `verdict_npcs.json` through the current Verdict host
2. read live machine/evidence/reckoning state
3. materialize the existing progress flags
4. query available NPCs by phase and canonical location
5. present a site-specific dialogue command
6. call `Speak` through the owner
7. project the one-shot result and capture the existing Verdict envelope

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- NPC metadata is catalog data; `spokenNpcIds` is the current one-shot owner state.
- Availability is a pure query over supplied flags, phase and optional location; it does not mutate the catalog or flag ledger.
- A successful `Speak` adds the ID once and emits one fact; a failed location/phase/unknown call does not mutate state.
- Restore deep-copies spoken IDs and must not replay the historical speech event.

- A location ID must resolve through the current Verdict location authority; an unresolved row fails closed or is reported as data drift.
- A phase floor and gating flag are both required when authored; an empty gate means unconditional only when the row’s contract says so.
- One NPC can speak once per save, independent of refresh, panel rebuild or repeated host calls.
- The same current flag/phase/location state produces the same ordered availability list.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/VerdictHostSession.cs
- src/VerdictPanel.cs
- src/Host/HostCli.ExpansionDepth.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs
- Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs
- Ashfall.Core.Tests/VerdictSystemTests.cs

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
| S-01 | 93-01 all 18 rows load and deduplicate | load `verdict_npcs.json` through the current Verdict host | NPC metadata is catalog data; `spokenNpcIds` is the current one-shot owner state. | A parsed NPC is displayed at the wrong location or phase. | VerdictNpcCatalogLoader |
| S-02 | 93-02 phase floor hides an early NPC | read live machine/evidence/reckoning state | Availability is a pure query over supplied flags, phase and optional location; it does not mutate the catalog or flag ledger. | A repeated click speaks twice or emits two journal facts. | VerdictNpcCatalogLoader |
| S-03 | 93-03 missing gate flag hides a gated NPC | materialize the existing progress flags | A successful `Speak` adds the ID once and emits one fact; a failed location/phase/unknown call does not mutate state. | A panel marks an NPC spoken without calling `VerdictNpcSystem.Speak`. | VerdictNpcCatalogLoader |
| S-04 | 93-04 correct flag and location reveal an NPC | query available NPCs by phase and canonical location | Restore deep-copies spoken IDs and must not replay the historical speech event. | A missing legacy NPC field crashes Verdict restore. | VerdictNpcCatalogLoader |
| S-05 | 93-05 wrong location rejects speech | present a site-specific dialogue command | NPC metadata is catalog data; `spokenNpcIds` is the current one-shot owner state. | A new catalog duplicates the Verdict evidence owner. | VerdictNpcCatalogLoader |
| S-06 | 93-06 first speech emits once | call `Speak` through the owner | Availability is a pure query over supplied flags, phase and optional location; it does not mutate the catalog or flag ledger. | A parsed NPC is displayed at the wrong location or phase. | VerdictNpcCatalogLoader |
| S-07 | 93-07 second speech is refused | project the one-shot result and capture the existing Verdict envelope | A successful `Speak` adds the ID once and emits one fact; a failed location/phase/unknown call does not mutate state. | A repeated click speaks twice or emits two journal facts. | VerdictNpcCatalogLoader |
| S-08 | 93-08 save/restore preserves spoken set | load `verdict_npcs.json` through the current Verdict host | Restore deep-copies spoken IDs and must not replay the historical speech event. | A panel marks an NPC spoken without calling `VerdictNpcSystem.Speak`. | VerdictNpcCatalogLoader |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 93-TC-01 catalog schema and ID uniqueness | data | catalog schema and ID uniqueness; verify the current owner and its negative boundary without inventing a second authority. | VerdictNpcCatalogLoader |
| T-02 | 93-TC-02 location reference resolution | unit | location reference resolution; verify the current owner and its negative boundary without inventing a second authority. | VerdictNpcCatalogLoader |
| T-03 | 93-TC-03 phase-floor boundary | persistence | phase-floor boundary; verify the current owner and its negative boundary without inventing a second authority. | VerdictNpcCatalogLoader |
| T-04 | 93-TC-04 case-insensitive flag gate | determinism | case-insensitive flag gate; verify the current owner and its negative boundary without inventing a second authority. | VerdictNpcCatalogLoader |
| T-05 | 93-TC-05 empty flag semantics | host | empty flag semantics; verify the current owner and its negative boundary without inventing a second authority. | VerdictNpcCatalogLoader |
| T-06 | 93-TC-06 dialogue nonempty validation | UI/accessibility | dialogue nonempty validation; verify the current owner and its negative boundary without inventing a second authority. | VerdictNpcCatalogLoader |
| T-07 | 93-TC-07 availability query purity | cross-system | availability query purity; verify the current owner and its negative boundary without inventing a second authority. | VerdictNpcCatalogLoader |
| T-08 | 93-TC-08 wrong-location refusal | data | wrong-location refusal; verify the current owner and its negative boundary without inventing a second authority. | VerdictNpcCatalogLoader |
| T-09 | 93-TC-09 one-shot Speak idempotence | unit | one-shot Speak idempotence; verify the current owner and its negative boundary without inventing a second authority. | VerdictNpcCatalogLoader |
| T-10 | 93-TC-10 OnSpoken event fact | persistence | OnSpoken event fact; verify the current owner and its negative boundary without inventing a second authority. | VerdictNpcCatalogLoader |
| T-11 | 93-TC-11 VerdictSave v1-v4 migration | determinism | VerdictSave v1-v4 migration; verify the current owner and its negative boundary without inventing a second authority. | VerdictNpcCatalogLoader |
| T-12 | 93-TC-12 deep-copy restore isolation | host | deep-copy restore isolation; verify the current owner and its negative boundary without inventing a second authority. | VerdictNpcCatalogLoader |
| T-13 | 93-TC-13 host panel availability projection | UI/accessibility | host panel availability projection; verify the current owner and its negative boundary without inventing a second authority. | VerdictNpcCatalogLoader |
| T-14 | 93-TC-14 no new save section | cross-system | no new save section; verify the current owner and its negative boundary without inventing a second authority. | VerdictNpcCatalogLoader |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 30 | `Assets/Ashfall.Core/Verdict/VerdictSave.cs` | current reference count; inspect the caller before treating it as a live route |
| 22 | `Ashfall.Core.Tests/VerdictSaveMigrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 19 | `Ashfall.Core.Tests/VerdictSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 19 | `src/Host/VerdictHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 17 | `src/Host/VerdictSaveStore.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `Ashfall.Core.Tests/VerdictQuestOwnershipTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `src/UI/VerdictDashboardPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/VerdictAccusationSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `src/Main.Verdict.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `src/Host/HostCli.SelfTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/VerdictPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Main.UiPanels.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/UI/ExpansionsHubPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/YearOfAsh/YearOfAshHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/CatalogIntegrityRules.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Radio/RadioSave.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Verdict/VerdictQuestMigration.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/HostCli.PanelTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Main.Application.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Main.PlayerSurfaces.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/UI/SnapshotHarness.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/verdict_npcs.json`

### `Assets/StreamingAssets/Data/verdict_npcs.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 9832; characters: 9832.
- SHA-256: `ace1edded901844ae68ebbaad7f913990316ab155888ccee2c531766be7cbadc`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `items`

#### `items` — 18 current rows

- Row 001 `npc_eden_vale`: `{"dialogue":["Still here. Static's thinning. That's not good news, that's a storm on the way.","The array's drawing again. I don't know what it's drawing for. I don't think it draws for us."],"gating_flag":"flag_verdict_eden_log_recovered"…`
- Row 002 `npc_ferris_voss`: `{"dialogue":["The archive keeps its own count. It does not require a reader.","An order can be paused, and a pause is not a cancellation. That is the whole Standard."],"gating_flag":"flag_verdict_fuse_world_read","id":"npc_ferris_voss","ki…`
- Row 003 `npc_iran_bell`: `{"dialogue":["Shift 36 closed out per procedure. The valve reads per 36; the record is complete.","Nobody swept that floor. It sweeps itself, the way the schedule requires."],"gating_flag":"flag_verdict_shift_charter_restored","id":"npc_ir…`
- Row 004 `npc_selya_saltmarsh`: `{"dialogue":["Persons shall not be counted twice. Persons shall not be counted once. The footnotes do not say which clause won.","The count column is blank, and it has been since Year One, when I ran out of households to sight."],"gating_f…`
- Row 005 `npc_maro_veen`: `{"dialogue":["This is the Office of Censuses. The count is open. All persons having custody of persons must present them.","Off-count is a penalty assessed against the holder. This message will repeat."],"gating_flag":"flag_verdict_call_re…`
- Row 006 `npc_whisper_cipher`: `{"dialogue":["Readings follow. No meaning is assigned by this facility.","The meter is the meter. Access is granted by maintenance, withdrawn by nobody."],"gating_flag":"flag_verdict_relay_read","id":"npc_whisper_cipher","kind":"readings",…`
- Row 007 `npc_tomas_reid`: `{"dialogue":["Procedure is not an obstacle to justice; it is the only wall between justice and execution.","If the chain of custody is broken, the machine is not judging a person—it is judging an echo."],"gating_flag":"flag_verdict_reid_en…`
- Row 008 `npc_elena_vane`: `{"dialogue":["The tape does not lie, for it has no blood to race and no belly to hunger.","When the Standard speaks, human repentance is merely noise in the carrier frequency."],"gating_flag":"flag_verdict_vane_enrolled","id":"npc_elena_va…`
- Row 009 `npc_kasper_holt`: `{"dialogue":["Every document has an origin, a transit, and a rest. If any of the three are missing, the page is illegitimate.","I do not read what is written on the parchment; I inspect the rag fibres and the iron-gall ink."],"gating_flag"…`
- Row 010 `npc_mara_elsen`: `{"dialogue":["The old high-water mark was wrong by six centimetres. I checked it three times.","No storm came through that night. The harbour climbed anyway.","They told me to stop writing the corrections in red. So I wrote them smaller."]…`
- Row 011 `npc_ilya_venn`: `{"dialogue":["The pressure trace dropped after the wind had already turned. Instruments are not supposed to remember weather late.","Central sent a correction packet for numbers I took myself. I kept the paper chart.","After that, the fore…`
- Row 012 `npc_garrick_daal`: `{"dialogue":["Channel four was civil traffic until someone changed the routing table.","After midnight, every third packet came back with a different origin.","I stopped acknowledging them. The system acknowledged for me."],"gating_flag":"…`
- Row 013 `npc_sena_korr`: `{"dialogue":["The mussels changed before the water report did.","Sample twelve was discarded twice. I kept both labels.","Someone wanted the contamination to begin on a date."],"gating_flag":"flag_verdict_eden_log_recovered","id":"npc_sena…`
- Row 014 `npc_torin_rask`: `{"dialogue":["The dead stand starts seventy metres before the fire line.","I marked it as wind damage. The map came back without the mark.","Trees do not know where the administrative boundary runs."],"gating_flag":"flag_verdict_relay_read…`
- Row 015 `npc_oren_varek`: `{"dialogue":["Core seven has two labels and one depth.","The ash layer sits below material they dated earlier.","I was told to file the duplicate under equipment error.","The downriver gauging station holds the baseline. Compare it before …`
- Row 016 `npc_lena_rost`: `{"dialogue":["The river rose without rain upstream.","Telemetry called it sensor drift. The concrete stairs were wet.","When the reading came back down, the mud line stayed."],"gating_flag":"flag_verdict_fuse_world_read","id":"npc_lena_ros…`
- Row 017 `npc_tessa_mirn`: `{"dialogue":["The control trays failed first.","That should have ended the trial. Instead they changed which trays were called control.","The seeds were honest. The labels were not."],"gating_flag":"flag_verdict_shift_charter_restored","id…`
- Row 018 `npc_karel_norn`: `{"dialogue":["The warning crossed the border before the order authorizing it.","I logged the time twice because I thought the clock had slipped.","Then headquarters asked me which copy of the log I intended to keep."],"gating_flag":"flag_v…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/verdict_locations.json`

### `Assets/StreamingAssets/Data/verdict_locations.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 14430; characters: 14416.
- SHA-256: `e97faf513dfa2e3f6b9cfb7fd1311cea6534a283e7d81e099784232b9d9380d0`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `locations`

#### `locations` — 15 current rows

- Row 001 `loc_geophone_pit_1`: `{"baseRadsPerHour":34,"dangerLevel":6,"description":"A concrete collar sunk like a wellhead, the lid propped on a brick. Below: a seismometer array the size of a dinner plate, bolted to bedrock, humming at a pitch almost too low to hear. T…`
- Row 002 `loc_twelve_gauge_array`: `{"baseRadsPerHour":38,"dangerLevel":7,"description":"Twelve shot-firing sounding stations on the ridge, each a one-metre steel post with a grease-stained plate reading TEMPEST SITE 07 and a firing order stencilled in flaking yellow. The or…`
- Row 003 `loc_network_fuse_bunker`: `{"baseRadsPerHour":42,"dangerLevel":8,"description":"A dry shielded service way between two facilities, entered through a door the size of a bank vault with a handle that turns freely. Three years of readouts line the walls in glass-fronte…`
- Row 004 `loc_archive_tape_silo`: `{"baseRadsPerHour":48,"dangerLevel":9,"description":"A vault the size of a chapel, wall-to-wall with steel racks of tape reels, each rack tagged by year. Twenty-one racks, four years per rack, the tags to the front, the labels in the same …`
- Row 005 `loc_abandoned_tide_gauge`: `{"baseRadsPerHour":28.0,"dangerLevel":5,"description":"A salt-stained concrete turret bolted over a tidal rock shelf, accessible only when the tide withdraws across the black gravel bar. Inside the spray-crusted wellhead, a perforated copp…`
- Row 006 `loc_coastal_meteorological_station`: `{"baseRadsPerHour":32.0,"dangerLevel":6,"description":"A wind-scoured timber shelter perched on the highest promontory of Cape Wrath, its guy wires whistling in the perpetual ocean draft. The roof-mounted anemometer cups have seized with c…`
- Row 007 `loc_clifftop_observation_bunker`: `{"baseRadsPerHour":36.0,"dangerLevel":7,"description":"A low-profile reinforced concrete casemate recessed into the basalt cliff face, overlooking the grey swell of the northern shelf. Monocular periscopes with thick quartz lenses still pe…`
- Row 008 `loc_sealed_marine_laboratory`: `{"baseRadsPerHour":44.0,"dangerLevel":8,"description":"A cluster of zinc-roofed laboratories nestled in a tidal cove, surrounded by rust-bleached brine intake conduits and dry holding tanks. Within the darkened basement storage vault, rows…`
- Row 009 `loc_forestry_survey_post`: `{"baseRadsPerHour":26.0,"dangerLevel":5,"description":"A soot-blackened timber ranger post set beside an overgrown logging haul road in the deep pines of the Blackwood. The front dispatch office has been ransacked for firewood, but the pad…`
- Row 010 `loc_geological_core_vault`: `{"baseRadsPerHour":38.0,"dangerLevel":7,"description":"A subterranean adit driven straight into a dry shale bluff, protected by an unhung steel mesh security gate. Inside, endless rows of industrial steel shelving hold thousands of cylindr…`
- Row 011 `loc_river_gauging_station`: `{"baseRadsPerHour":30.0,"dangerLevel":6,"description":"A hexagonal concrete gauging tower rising directly from the rocky gorge wall of the Karsk River, its lower catwalk twisted by spring ice dams. The upper chamber houses a brass float me…`
- Row 012 `loc_abandoned_agricultural_station`: `{"baseRadsPerHour":35.0,"dangerLevel":7,"description":"A sprawling complex of shattered greenhouse bays and brick administration offices sprawled across the fertile river terrace. Beneath the broken glass panes, hundreds of galvanized soil…`
- Row 013 `loc_decommissioned_signal_relay`: `{"baseRadsPerHour":36.0,"dangerLevel":6,"description":"A four-legged steel lattice mast anchored to a freezing alpine col, its parabolic microwave feedhorns aimed dead south along the high border fence line. The corrugated transmitter shac…`
- Row 014 `loc_border_checkpoint_ruins`: `{"baseRadsPerHour":40.0,"dangerLevel":8,"description":"A fortified mountain pass bottleneck choked with crumbling concrete chicane barriers, rusted razor wire, and overturned transport hulls. The above-ground guardhouses show only bullet-p…`
- Row 015 `loc_minefield_observation_tower`: `{"baseRadsPerHour":46.0,"dangerLevel":8,"description":"A hexagonal reinforced concrete watchtower looming over an expanse of marked border minefields and dead pine stumps on the southern crest. The upper observation platform features a hea…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/verdict_data.json`

### `Assets/StreamingAssets/Data/verdict_data.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 9814; characters: 9776.
- SHA-256: `2c7ef4992e16b4d32e0a3a12af42f4d12bf8460e51a0c69eadc8e0d42799dae2`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `catalog`, `schema_version`, `description`, `currencies`, `readout_steps`, `facets`, `endings`, `world_history_ladder`, `corruption_corpus`

#### `currencies` — 1 current rows

- Row 001 `enrolled_evidence`: `{"id":"enrolled_evidence","label":"Enrolled Evidence","note":"A reading the machine keeps only because a human read it. Fragile: it is a record, not a resource."}`

#### `readout_steps` — 4 current rows

- Row 001 `step_fuse_advance`: `{"id":"step_fuse_advance","label":"Fuse world schedule advances 12 minutes","readout":"A clock being serviced, not an attack.","trigger_phase":"counted"}`
- Row 002 `step_drone_sleep`: `{"id":"step_drone_sleep","label":"Drone-hive draw -0.5 degrees","readout":"The wing standing down, the way a wing stands down.","trigger_phase":"counted"}`
- Row 003 `step_summit_light`: `{"id":"step_summit_light","label":"Summit light, cold, idle","readout":"The sector seeing itself counted.","trigger_phase":"culpable"}`
- Row 004 `step_census_carrier`: `{"id":"step_census_carrier","label":"The carrier tone on a dead band","readout":"A schedule, not a threat.","trigger_phase":"culpable"}`

#### `facets` — 3 current rows

- Row 001 `facet_archive`: `{"bylines":["rustic","brutal","practical","positive","neutral"],"id":"facet_archive","label":"The Archive","need":"memory"}`
- Row 002 `facet_fire_computing`: `{"bylines":["rustic","brutal","practical","positive","neutral"],"id":"facet_fire_computing","label":"The Fire-Computing Room","need":"history"}`
- Row 003 `facet_vent_shaft`: `{"bylines":["rustic","brutal","practical","positive","neutral"],"id":"facet_vent_shaft","label":"The Vent Shaft","need":"warmth"}`

#### `endings` — 3 current rows

- Row 001 `ending_verdict_the_sector_recounts`: `{"id":"ending_verdict_the_sector_recounts","label":"The Sector Recounts","trigger":"enrolled_evidence >= threshold AND presented count honored","vignette":"The count is read aloud at the Grain Exchange weighbridge by a trader who does not …`
- Row 002 `ending_verdict_the_count_is_held`: `{"id":"ending_verdict_the_count_is_held","label":"The Count Is Held","trigger":"enrolled_evidence < threshold AND count not presented","vignette":"Nobody presents the count. The carrier tone continues on the dead band. The radio screen rea…`
- Row 003 `ending_verdict_the_offer_is_a_lease`: `{"id":"ending_verdict_the_offer_is_a_lease","label":"The Offer Is a Lease","trigger":"presented count declined (no honor)","vignette":"The count converts into a lease: quarterly maintenance, a reading per season, a census every 1,827 days,…`

#### `world_history_ladder` — 12 current rows

- Row 001 `loc_geophone_pit_1`: `{"body_summary":"The array reads the ground the way the papers said it never did: even, patient, and uninterested in everything above it but the count.","discovery_location_id":"loc_geophone_pit_1","knowledge_key":"lore_verdict_geophone_on…`
- Row 002 `loc_network_fuse_bunker`: `{"body_summary":"Twenty-nine shift charters frame the readout cabinets. The machine kept the codes after the staff stopped keeping the shifts.","discovery_location_id":"loc_network_fuse_bunker","knowledge_key":"lore_verdict_shift_charters"…`
- Row 003 `loc_network_fuse_bunker`: `{"body_summary":"The charter's full text. The sentence that does the work: nothing in this Standard shall be construed to require the presentation to be read.","discovery_location_id":"loc_network_fuse_bunker","knowledge_key":"lore_verdict…`
- Row 004 `location_the_dead_hand_core`: `{"body_summary":"The UXO field register reads held, not live. The fields were never awake. They were held Pending Count since a war the count never ended.","discovery_location_id":"location_the_dead_hand_core","knowledge_key":"lore_verdict…`
- Row 005 `loc_comm_array`: `{"body_summary":"The census carrier is a pure data tone on a derelict band. The words are thirty years old, the voice is dead, and the schedule is still walking.","discovery_location_id":"loc_comm_array","knowledge_key":"lore_verdict_the_c…`
- Row 006 `loc_archive_tape_silo`: `{"body_summary":"The count is presented. It names the shelter's persons, by name. A machine does not reason. It counts. This is the count.","discovery_location_id":"loc_archive_tape_silo","knowledge_key":"lore_verdict_the_count","layer":6,…`
- Row 007 `loc_twelve_gauge_array`: `{"body_summary":"The second array is not a copy of the first. Its stations fire into the ridge and listen for the answer, a deeper register built to compare one machine's patience with another's.","discovery_location_id":"loc_twelve_gauge_…`
- Row 008 `loc_decommissioned_signal_relay`: `{"body_summary":"The cable does not terminate at a weapon. It runs from the sounding stations to a relay mast, where the line is converted into a schedule the counting house can carry. The machines were built to serve the interval between …`
- Row 009 `location_radar_site`: `{"body_summary":"The radar annex predates the census language. Its target grid is a list of what might cross the horizon, and the first count began as a promise that no movement would go unrecorded. The later register only changed the obje…`
- Row 010 `location_weather_station`: `{"body_summary":"The barograph shows a clean break in the transmission record: eleven minutes with no pressure, no wind, and no machine status. Then the carrier returns under a new station prefix, and the log marks the restart as routine. …`
- Row 011 `loc_clifftop_observation_bunker`: `{"body_summary":"The final operator left a grease mark on the bus valve and a correction to the horizon grid. The mark is human, the correction is careful, and the duty roster says the station was already empty. The machine kept the hand a…`
- Row 012 `loc_border_checkpoint_ruins`: `{"body_summary":"The checkpoint ledger closes every convoy except the one that carried the count onward. Forty-two entries remain without a destination, so the register keeps the window open rather than call them absent. A count that canno…`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs`

### `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs` — complete current file

- Size: 155 lines / 5915 bytes.
- SHA-256: `09f7ee1e6d99e55b7559acda53ff56389392ff3ae5cf9ca2069d4b3db2a28ca4`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: using Ashfall.Core.IO;
00007: namespace Ashfall.Core.Verdict
00008: {
00009:     /// <summary>One Verdict NPC — a figure with flag-gated availability and phase reactions.</summary>
00010:     [Serializable]
00011:     public class VerdictNpcEntry
00012:     {
00013:         public string id = string.Empty;
00014:         public string name = string.Empty;
00015:         public string role = string.Empty;
00016:         public string kind = "paper_ghost";  // tape_echo | paper_ghost | living | readings
00017:
00018:         // Plan 93: verdict_npcs.json is snake_case data authority while the
00019:         // shared serializer options are case-insensitive only (no naming
00020:         // policy) — these fields need explicit mappings or they silently
00021:         // deserialize to defaults (empty gate/location, phaseMin 1).
00022:         [System.Text.Json.Serialization.JsonPropertyName("gating_flag")]
00023:         public string gatingFlag = string.Empty;
00024:         [System.Text.Json.Serialization.JsonPropertyName("location_id")]
00025:         public string locationId = string.Empty;
00026:         [System.Text.Json.Serialization.JsonPropertyName("phase_min")]
00027:         public int phaseMin = 1;
00028:         public List<string> dialogue = new List<string>();
00029:     }
00030:
00031:     [Serializable]
00032:     public class VerdictNpcState
00033:     {
00034:         public List<string> spokenNpcIds = new List<string>();
00035:     }
00036:
00037:     /// <summary>
00038:     /// ASHFALL: THE VERDICT (Expansion 08) — the six figures of the machine's
00039:     /// human record. Each is a flag-gated encounter card: available only when
00040:     /// its gate flag is set, reactive to the Reckoning phase, one-shot spoken.
00041:     /// No human faction is spawned — the Tempest stays a utility.
00042:     /// </summary>
00043:     public sealed class VerdictNpcSystem
00044:     {
00045:         private readonly VerdictNpcState _state;
00046:         private readonly List<VerdictNpcEntry> _catalog = new List<VerdictNpcEntry>();
00047:
00048:         public VerdictNpcState State => _state;
00049:         public IReadOnlyList<VerdictNpcEntry> Catalog => _catalog;
00050:
00051:         public event Action<VerdictNpcEntry> OnSpoken;
00052:
00053:         public VerdictNpcSystem(VerdictNpcState? state = null)
00054:         {
00055:             _state = state ?? new VerdictNpcState();
00056:         }
00057:
00058:         public void Register(VerdictNpcEntry entry)
00059:         {
00060:             if (entry == null || string.IsNullOrEmpty(entry.id)) return;
00061:             if (!_catalog.Exists(e => e.id == entry.id)) _catalog.Add(entry);
00062:         }
00063:
00064:         public VerdictNpcEntry? Find(string id)
00065:         {
00066:             foreach (var e in _catalog)
00067:                 if (e.id == id) return e;
00068:             return null;
00069:         }
00070:
00071:         /// <summary>NPCs whose gate flag is set and whose phase requirement is met.</summary>
00072:         public List<VerdictNpcEntry> GetAvailable(
00073: IReadOnlyCollection<string> setFlags, int phase, string? locationId = null)
00074:         {
00075:             var result = new List<VerdictNpcEntry>();
00076:             foreach (var e in _catalog)
00077:             {
00078:                 if (e.phaseMin > 1 && phase < e.phaseMin) continue;
00079:                 if (!string.IsNullOrEmpty(e.gatingFlag) &&
00080:                     (setFlags == null || !ContainsFlag(setFlags, e.gatingFlag))) continue;
00081:                 if (!string.IsNullOrEmpty(locationId) && e.locationId != locationId) continue;
00082:                 result.Add(e);
00083:             }
00084:             return result;
00085:         }
00086:
00087:         /// <summary>Spend the NPC's only interjection. Idempotent per NPC.</summary>
00088:         public bool Speak(string npcId, string? locationId = null)
00089:         {
00090:             var npc = Find(npcId);
00091:             if (npc == null) return false;
00092:             if (_state.spokenNpcIds.Contains(npcId)) return false; // one-shot
00093:             if (!string.IsNullOrEmpty(locationId) && npc.locationId != locationId) return false;
00094:
00095:             _state.spokenNpcIds.Add(npcId);
00096:             OnSpoken?.Invoke(npc);
00097:             return true;
00098:         }
00099:
00100:         private static bool ContainsFlag(IReadOnlyCollection<string> flags, string id)
00101:         {
00102:             foreach (var f in flags)
00103:                 if (string.Equals(f, id, StringComparison.OrdinalIgnoreCase)) return true;
00104:             return false;
00105:         }
00106:
00107:         public VerdictNpcState CaptureState()
00108:         {
00109:             var copy = new VerdictNpcState();
00110:             copy.spokenNpcIds.AddRange(_state.spokenNpcIds);
00111:             return copy;
00112:         }
00113:
00114:         public void RestoreState(VerdictNpcState state)
00115:         {
00116:             if (state == null) return;
00117:             _state.spokenNpcIds.Clear();
00118:             _state.spokenNpcIds.AddRange(state.spokenNpcIds);
00119:         }
00120:     }
00121:
00122:     /// <summary>Loader for verdict_npcs.json.</summary>
00123:     public static class VerdictNpcCatalogLoader
00124:     {
00125:         public const string FileName = "verdict_npcs.json";
00126:
00127:         public static int LoadAndRegister(VerdictNpcSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json)
00128:         {
00129:             if (system == null || fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00130:                 return 0;
00131:             string path = fileIO.Combine(dataDir, FileName);
00132:             if (!fileIO.FileExists(path)) return 0;
00133:             string raw = fileIO.ReadAllText(path);
00134:             if (string.IsNullOrWhiteSpace(raw)) return 0;
00135:             try
00136:             {
00137:                 var list = CatalogLocator.LoadWrappedList<VerdictNpcEntry>(raw, SystemTextJsonSerializer.Options);
00138:                 if (list == null) return 0;
00139:                 int count = 0;
00140:                 foreach (var e in list)
00141:                 {
00142:                     if (e == null || string.IsNullOrEmpty(e.id)) continue;
00143:                     system.Register(e);
00144:                     count++;
00145:                 }
00146:                 return count;
00147:             }
00148:             catch (Exception ex_CATDIAG)
00149:             {
00150:                 CatalogDiagnostics.Warn(path, "VerdictNpcEntry list", ex_CATDIAG);
00151:                 return 0;
00152:             }
00153:         }
00154:     }
00155: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Verdict/VerdictSave.cs`

### `Assets/Ashfall.Core/Verdict/VerdictSave.cs` — complete current file

- Size: 272 lines / 12122 bytes.
- SHA-256: `35bd29e9933555d17a90114245b9395ee98ec22077e7d46e2467dd185f603806`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Ashfall.Core.YearOfAsh;
00004:
00005: using Ashfall.Core.IO;
00006: namespace Ashfall.Core.Verdict
00007: {
00008:     /// <summary>
00009:     /// ASHFALL: THE VERDICT (Expansion 08) — cross-host save envelope.
00010:     /// Mirrors DoseLedgerSave / HoldfastSave: checksum recomputed on encode,
00011:     /// hard-reject on decode for tamper / checksumless / newer version.
00012:     ///
00013:     /// v3 adds the Verdict questline section. Verdict quest progress is owned
00014:     /// here — the Year of Ash envelope is no longer a second owner (its
00015:     /// registration was removed). v1/v2 saves migrate with an empty quest
00016:     /// section; a one-time adoption helper folds any quest_verdict_* progress
00017:     /// that a pre-v3 save carried inside the Year of Ash envelope into the
00018:     /// Verdict envelope (see <see cref="VerdictQuestMigration"/>).
00019:     ///
00020:     /// Migration validates the checksum over each version's FROZEN shape (see
00021:     /// <see cref="VerdictSaveV1"/> / <see cref="VerdictSaveV2"/>) because
00022:     /// <see cref="SaveChecksum"/> walks public fields — validating a legacy
00023:     /// payload against the current shape would always mismatch.
00024:     /// </summary>
00025:     [Serializable]
00026:     public class VerdictSave
00027:     {
00028:         public const int CurrentSaveVersion = 4;
00029:         public const int MigrationFromVersion = 1;
00030:
00031:         public int saveVersion = CurrentSaveVersion;
00032:         public int simDay;
00033:         public MachineLogSystemState machineLog = new MachineLogSystemState();
00034:         public ReckoningState reckoning = new ReckoningState();
00035:         public EvidenceLedgerState evidence = new EvidenceLedgerState();
00036:         public VerdictNpcState npcs = new VerdictNpcState();
00037:         public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
00038:         public QuestlineSystemState quests = new QuestlineSystemState();
00039:         public int censusLastWindowDay = -1;
00040:
00041:         // v4 section.
00042:         public VerdictAccusationState accusations = new VerdictAccusationState();
00043:
00044:         public string Checksum = string.Empty;
00045:     }
00046:
00047:     /// <summary>
00048:     /// Frozen v3 envelope shape (npcs + radio + quests, no accusations section).
00049:     /// Do not add fields here — it must match what v3 wrote byte-for-byte in field set.
00050:     /// </summary>
00051:     [Serializable]
00052:     public class VerdictSaveV3
00053:     {
00054:         public int saveVersion = 3;
00055:         public int simDay;
00056:         public MachineLogSystemState machineLog = new MachineLogSystemState();
00057:         public ReckoningState reckoning = new ReckoningState();
00058:         public EvidenceLedgerState evidence = new EvidenceLedgerState();
00059:         public VerdictNpcState npcs = new VerdictNpcState();
00060:         public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
00061:         public QuestlineSystemState quests = new QuestlineSystemState();
00062:         public int censusLastWindowDay = -1;
00063:         public string Checksum = string.Empty;
00064:     }
00065:
00066:     /// <summary>
00067:     /// Frozen v1 envelope shape (no npcs, no radio, no quests). Kept so a v1
00068:     /// file on disk validates against the field set it was actually hashed with.
00069:     /// Do not add fields here.
00070:     /// </summary>
00071:     [Serializable]
00072:     public class VerdictSaveV1
00073:     {
00074:         public int saveVersion = 1;
00075:         public int simDay;
00076:         public MachineLogSystemState machineLog = new MachineLogSystemState();
00077:         public ReckoningState reckoning = new ReckoningState();
00078:         public EvidenceLedgerState evidence = new EvidenceLedgerState();
00079:         public int censusLastWindowDay = -1;
00080:         public string Checksum = string.Empty;
00081:     }
00082:
00083:     /// <summary>
00084:     /// Frozen v2 envelope shape (npcs + radio, no quests). Kept so a v2 file on
00085:     /// disk validates against the field set it was actually hashed with.
00086:     /// Do not add fields here.
00087:     /// </summary>
00088:     [Serializable]
00089:     public class VerdictSaveV2
00090:     {
00091:         public int saveVersion = 2;
00092:         public int simDay;
00093:         public MachineLogSystemState machineLog = new MachineLogSystemState();
00094:         public ReckoningState reckoning = new ReckoningState();
00095:         public EvidenceLedgerState evidence = new EvidenceLedgerState();
00096:         public VerdictNpcState npcs = new VerdictNpcState();
00097:         public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
00098:         public int censusLastWindowDay = -1;
00099:         public string Checksum = string.Empty;
00100:     }
00101:
00102:     public static class VerdictSaveCodec
00103:     {
00104:         public static VerdictSave Capture(
00105:             int simDay,
00106:             MachineLogSystem machineLog,
00107:             ReckoningSystem reckoning,
00108:             EvidenceLedger evidence,
00109:             int censusLastWindowDay,
00110: VerdictNpcSystem? npcs = null,
00111: VerdictRadioSystem? radio = null,
00112: QuestlineSystem? quests = null,
00113: VerdictAccusationSystem? accusations = null)
00114:         {
00115:             var save = new VerdictSave
00116:             {
00117:                 simDay = simDay,
00118:                 machineLog = machineLog.CaptureState(),
00119:                 reckoning = reckoning.CaptureState(),
00120:                 evidence = evidence.CaptureState(),
00121:                 npcs = npcs != null ? npcs.CaptureState() : new VerdictNpcState(),
00122:                 radio = radio != null ? radio.CaptureState() : new VerdictRadioSystem.VerdictRadioState(),
00123:                 quests = quests != null ? quests.CaptureState() : new QuestlineSystemState(),
00124:                 accusations = accusations != null ? accusations.CaptureState() : new VerdictAccusationState(),
00125:                 censusLastWindowDay = censusLastWindowDay
00126:             };
00127:             save.Checksum = SaveChecksum.Compute(save);
00128:             return save;
00129:         }
00130:
00131:         public static string Encode(VerdictSave save, IJsonSerializer json)
00132:         {
00133:             if (save == null) throw new ArgumentNullException(nameof(save));
00134:             save.Checksum = SaveChecksum.Compute(save);
00135:             return json.Serialize(save);
00136:         }
00137:
00138:         /// <summary>
00139:         /// Decodes and migrates a Verdict save. Legacy versions are parsed as
00140:         /// their FROZEN shapes so the checksum is verified over exactly the
00141:         /// fields that version wrote. Rejects: newer versions, too-old versions,
00142:         /// checksumless payloads, and tampered payloads.
00143:         /// </summary>
00144:         public static bool TryDecode(string json, IJsonSerializer serializer, out VerdictSave save)
00145:         {
00146:             save = null!;
00147:             if (string.IsNullOrEmpty(json) || serializer == null) return false;
00148:             try
00149:             {
00150:                 var decoded = serializer.Deserialize<VerdictSave>(json);
00151:                 if (decoded == null) return false;
00152:                 if (decoded.saveVersion > VerdictSave.CurrentSaveVersion) return false; // newer — reject
00153:                 if (decoded.saveVersion < VerdictSave.MigrationFromVersion) return false; // too old — reject
00154:
00155:                 if (decoded.saveVersion == 1) return MigrateV1(json, serializer, out save);
00156:                 if (decoded.saveVersion == 2) return MigrateV2(json, serializer, out save);
00157:                 if (decoded.saveVersion == 3) return MigrateV3(json, serializer, out save);
00158:
00159:                 // Current version: validate over the current shape.
00160:                 if (string.IsNullOrEmpty(decoded.Checksum)) return false;   // tamper/legacy
00161:                 string recomputed = SaveChecksum.Compute(decoded);
00162:                 if (!string.Equals(recomputed, decoded.Checksum, StringComparison.Ordinal))
00163:                     return false; // tampered
00164:                 save = decoded;
00165:                 return true;
00166:             }
00167:             catch (Exception ex_CATDIAG)
00168:             {
00169:                 CatalogDiagnostics.Warn("<decode>", "VerdictSave", ex_CATDIAG);
00170:                 return false;
00171:             }
00172:         }
00173:
00174:         private static bool MigrateV1(string json, IJsonSerializer serializer, out VerdictSave save)
00175:         {
00176:             save = null!;
00177:             var v1 = serializer.Deserialize<VerdictSaveV1>(json);
00178:             if (v1 == null) return false;
00179:             if (string.IsNullOrEmpty(v1.Checksum)) return false;
00180:             if (!string.Equals(SaveChecksum.Compute(v1), v1.Checksum, StringComparison.Ordinal)) return false;
00181:
00182:             var migrated = new VerdictSave
00183:             {
00184:                 saveVersion = VerdictSave.CurrentSaveVersion,
00185:                 simDay = v1.simDay,
00186:                 machineLog = v1.machineLog,
00187:                 reckoning = v1.reckoning,
00188:                 evidence = v1.evidence,
00189:                 // npcs / radio / quests / accusations stay at their field initialisers (fresh defaults).
00190:                 censusLastWindowDay = v1.censusLastWindowDay
00191:             };
00192:             migrated.Checksum = SaveChecksum.Compute(migrated);
00193:             save = migrated;
00194:             return true;
00195:         }
00196:
00197:         private static bool MigrateV2(string json, IJsonSerializer serializer, out VerdictSave save)
00198:         {
00199:             save = null!;
00200:             var v2 = serializer.Deserialize<VerdictSaveV2>(json);
00201:             if (v2 == null) return false;
00202:             if (string.IsNullOrEmpty(v2.Checksum)) return false;
00203:             if (!string.Equals(SaveChecksum.Compute(v2), v2.Checksum, StringComparison.Ordinal)) return false;
00204:
00205:             var migrated = new VerdictSave
00206:             {
00207:                 saveVersion = VerdictSave.CurrentSaveVersion,
00208:                 simDay = v2.simDay,
00209:                 machineLog = v2.machineLog,
00210:                 reckoning = v2.reckoning,
00211:                 evidence = v2.evidence,
00212:                 npcs = v2.npcs ?? new VerdictNpcState(),
00213:                 radio = v2.radio ?? new VerdictRadioSystem.VerdictRadioState(),
00214:                 // quests / accusations stay at their field initialisers (fresh defaults).
00215:                 censusLastWindowDay = v2.censusLastWindowDay
00216:             };
00217:             migrated.Checksum = SaveChecksum.Compute(migrated);
00218:             save = migrated;
00219:             return true;
00220:         }
00221:
00222:         private static bool MigrateV3(string json, IJsonSerializer serializer, out VerdictSave save)
00223:         {
00224:             save = null!;
00225:             var v3 = serializer.Deserialize<VerdictSaveV3>(json);
00226:             if (v3 == null) return false;
00227:             if (string.IsNullOrEmpty(v3.Checksum)) return false;
00228:             if (!string.Equals(SaveChecksum.Compute(v3), v3.Checksum, StringComparison.Ordinal)) return false;
00229:
00230:             var migrated = new VerdictSave
00231:             {
00232:                 saveVersion = VerdictSave.CurrentSaveVersion,
00233:                 simDay = v3.simDay,
00234:                 machineLog = v3.machineLog,
00235:                 reckoning = v3.reckoning,
00236:                 evidence = v3.evidence,
00237:                 npcs = v3.npcs ?? new VerdictNpcState(),
00238:                 radio = v3.radio ?? new VerdictRadioSystem.VerdictRadioState(),
00239:                 quests = v3.quests ?? new QuestlineSystemState(),
00240:                 // accusations stays at its field initialiser (fresh empty state).
00241:                 censusLastWindowDay = v3.censusLastWindowDay
00242:             };
00243:             migrated.Checksum = SaveChecksum.Compute(migrated);
00244:             save = migrated;
00245:             return true;
00246:         }
00247:
00248:         public static void Restore(
00249:             VerdictSave save,
00250:             MachineLogSystem machineLog,
00251:             ReckoningSystem reckoning,
00252:             EvidenceLedger evidence,
00253: VerdictNpcSystem? npcs = null,
00254: VerdictRadioSystem? radio = null,
00255: QuestlineSystem? quests = null,
00256: VerdictAccusationSystem? accusations = null)
00257:         {
00258:             if (save == null) return;
00259:             machineLog.RestoreState(save.machineLog);
00260:             reckoning.RestoreState(save.reckoning);
00261:             evidence.RestoreState(save.evidence);
00262:             if (npcs != null)
00263:                 npcs.RestoreState(save.npcs ?? new VerdictNpcState());
00264:             if (radio != null)
00265:                 radio.RestoreState(save.radio ?? new VerdictRadioSystem.VerdictRadioState());
00266:             if (quests != null)
00267:                 quests.RestoreState(save.quests ?? new QuestlineSystemState());
00268:             if (accusations != null)
00269:                 accusations.RestoreState(save.accusations ?? new VerdictAccusationState());
00270:         }
00271:     }
00272: }
```


# Appendix — Current Source Detail: `src/Host/VerdictHostSession.cs`

### `src/Host/VerdictHostSession.cs` — complete current file

- Size: 284 lines / 14368 bytes.
- SHA-256: `eb9abc73b3557cdfa1d7b975f426399da4cbf2e299387681232521df4b1f6f8b`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using Ashfall.Core;
00006: using Ashfall.Core.Clock;
00007: using Ashfall.Core.Events;
00008: using Ashfall.Core.Flags;
00009: using Ashfall.Core.Verdict;
00010: using Ashfall.Core.YearOfAsh;
00011: using AtomicWar.GodotApp.YearOfAsh;
00012: using AtomicWar.GodotApp.Audio;
00013: using ClockSimClock = Ashfall.Core.Clock.SimClock;
00014:
00015: namespace AtomicWar.GodotApp
00016: {
00017:     /// <summary>
00018:     /// ASHFALL: THE VERDICT (Expansion 08) — thin Godot host session.
00019:     /// Wraps MachineLogSystem + ReckoningSystem + EvidenceLedger + the 99.0 MHz
00020:     /// census broadcast, wires the sim clock / event bus / flag ledger / census
00021:     /// port, and persists to user:// via VerdictSaveStore. No gameplay rules
00022:     /// here — hosts only present.
00023:     /// </summary>
00024:     public sealed class VerdictHostSession
00025:     : HostSessionBase{
00026:         private static readonly FileSystemIO s_files = new FileSystemIO();
00027:         private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();
00028:
00029:         public MachineLogSystem MachineLog { get; }
00030:         public ReckoningSystem Reckoning { get; }
00031:         public EvidenceLedger Evidence { get; }
00032:         public VerdictEvidenceChain EvidenceChain { get; }
00033:         public VerdictNpcSystem Npcs { get; }
00034:         public VerdictCensusBroadcast Census { get; }
00035:         public VerdictRadioSystem Radio { get; internal set; }
00036:         public QuestlineSystem Quests { get; }
00037:         public IReadOnlyList<VerdictCatalogLoader.VerdictLocationEntry> Locations { get; }
00038:         public IReadOnlyList<VerdictCatalogLoader.VerdictItemEntry> Items { get; }
00039:         public IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> RadioEntries { get; }
00040:         public IReadOnlyList<string> CorruptionCorpus { get; private set; }
00041:         private readonly ISeededRng _machineRng;
00042:         private int _currentDay = -1;
00043:
00044:         /// <summary>Flag-gate materialization: the six Verdict figures unlock from
00045:         /// real progress (evidence, phase, call). Hosts present this; no NPC gate is
00046:         /// a debug backdoor — each flag traces to an actual machine milestone.</summary>
00047:         public System.Collections.Generic.HashSet<string> MaterializedNpcFlags()
00048:         {
00049:             var flags = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
00050:             if (MachineLog.ReadCount() >= 1) flags.Add("flag_verdict_fuse_world_read");
00051:             if (MachineLog.ReadCount() >= 1) flags.Add("flag_verdict_relay_read");
00052:             if (Evidence.IsEnrolled("evidence_eden_log")) flags.Add("flag_verdict_eden_log_recovered");
00053:             if (Evidence.IsEnrolled("evidence_fuse_linen")) flags.Add("flag_verdict_fuse_world_read");
00054:             if (Evidence.IsEnrolled("evidence_fuse_linen")) flags.Add("flag_verdict_shift_charter_restored");
00055:             if (Evidence.IsEnrolled("evidence_geophone_hymn")) flags.Add("flag_verdict_clerk_met");
00056:             if (Reckoning.State.callResolved) flags.Add("flag_verdict_call_resolved");
00057:             // Plan 93 — investigation-site residue NPCs. The cliff-bunker signal
00058:             // decodes from sustained study of the machine's own log (a second
00059:             // read entry), the same progress surface that materializes the
00060:             // original gates above.
00061:             if (MachineLog.ReadCount() >= 2) flags.Add("flag_verdict_cliff_signal_decoded");
00062:             return flags;
00063:         }
00064:
00065:         /// <summary>NPCs currently available given live progress (flag + phase + optional site).</summary>
00066:         public System.Collections.Generic.List<Ashfall.Core.Verdict.VerdictNpcEntry> AvailableNpcs(string locationId = null!)
00067:         {
00068:             int phase = (int)Reckoning.Phase;
00069:             return Npcs.GetAvailable(MaterializedNpcFlags(), phase, locationId);
00070:         }
00071:
00072:         public string LastEvent { get; private set; } = string.Empty;
00073:         public VerdictHostSession(
00074:             MachineLogSystem machineLog = null!,
00075:             ReckoningSystem reckoning = null!,
00076:             EvidenceLedger evidence = null!,
00077:             VerdictNpcSystem npcs = null!,
00078:             VerdictCensusBroadcast census = null!,
00079:             IReadOnlyList<VerdictCatalogLoader.VerdictLocationEntry> locations = null!,
00080:             IReadOnlyList<VerdictCatalogLoader.VerdictItemEntry> items = null!,
00081:             IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> radio = null!,
00082:             QuestlineSystem quests = null!)
00083:         {
00084:             MachineLog = machineLog ?? new MachineLogSystem();
00085:             Reckoning = reckoning ?? new ReckoningSystem();
00086:             Evidence = evidence ?? new EvidenceLedger();
00087:             EvidenceChain = new VerdictEvidenceChain(MachineLog, Evidence, Reckoning);
00088:             Npcs = npcs ?? new VerdictNpcSystem();
00089:             Quests = quests ?? new QuestlineSystem();
00090:             Census = census;
00091:             Locations = locations ?? new List<VerdictCatalogLoader.VerdictLocationEntry>();
00092:             Items = items ?? new List<VerdictCatalogLoader.VerdictItemEntry>();
00093:             RadioEntries = radio ?? new List<VerdictCatalogLoader.VerdictRadioEntry>();
00094:             CorruptionCorpus = new List<string>();
00095:             _machineRng = new SeededRng(8841209 + 17);
00096:
00097:             MachineLog.OnLogPosted += e => { LastEvent = $"log:{e.facilityId}@{e.day}:{e.kind}"; RaiseStateChanged(); };
00098:             MachineLog.OnEntryRead += e => { LastEvent = $"read:{e.evidenceTag}"; RaiseStateChanged(); };
00099:             Reckoning.OnPhaseChanged += p => { LastEvent = $"phase:{p}"; RaiseStateChanged(); };
00100:             Reckoning.OnReckoningCall += n => { LastEvent = $"reckoning_call:{n}"; RaiseStateChanged(); };
00101:             Reckoning.OnVerdictResolved += key => { LastEvent = $"resolved:{key}"; RaiseStateChanged(); };
00102:             Evidence.OnEnrolled += id => { LastEvent = $"evidence:{id}"; RaiseStateChanged(); };
00103:             Npcs.OnSpoken += n => { LastEvent = $"npc:{n.id}"; RaiseStateChanged(); };
00104:         }
00105:
00106:         public static VerdictHostSession Create(
00107:             string dataDir,
00108:             ISimClock clock = null!,
00109:             IEventBus bus = null!,
00110:             IFlagLedger flags = null!,
00111:             ISeededRng radioRng = null!,
00112:             IWorldCensus census = null!)
00113:         {
00114:             clock = clock ?? new ClockSimClock();
00115:             bus = bus ?? new SimpleEventBus();
00116:             flags = flags ?? new Ashfall.Core.Flags.CampaignConsequenceLedger();
00117:             radioRng = radioRng ?? new SeededRng(8841209);
00118:
00119:             var locations = VerdictCatalogLoader.LoadLocations(dataDir, s_files, s_json);
00120:             var items = VerdictCatalogLoader.LoadItems(dataDir, s_files, s_json);
00121:             var radioEntries = VerdictCatalogLoader.LoadRadio(dataDir, s_files, s_json);
00122:             var censusBroadcast = new VerdictCensusBroadcast(clock, bus, flags, radioRng, census);
00123:             var quests = new QuestlineSystem();
00124:             VerdictQuestCatalogLoader.LoadAndRegister(quests, dataDir, s_files, s_json);
00125:             var session = new VerdictHostSession(census: censusBroadcast, locations: locations, items: items, radio: radioEntries, quests: quests);
00126:             session.Radio = new VerdictRadioSystem(bus, clock, radioEntries);
00127:             VerdictNpcCatalogLoader.LoadAndRegister(session.Npcs, dataDir, s_files, s_json);
00128:             session.CorruptionCorpus = VerdictCatalogLoader.LoadCorruptionCorpus(dataDir, s_files, s_json);
00129:
00130:             var save = VerdictSaveStore.TryLoad();
00131:             if (save != null)
00132:             {
00133:                 VerdictSaveCodec.Restore(save, session.MachineLog, session.Reckoning, session.Evidence, session.Npcs, session.Radio, session.Quests);
00134:                 // Observability: remember which save version loaded and whether it migrated (C).
00135:                 session.LoadedSaveVersion = save.saveVersion;
00136:                 session.WasSaveMigrated = save.saveVersion != VerdictSave.CurrentSaveVersion;
00137:                 session.LastEvent = "Verdict state restored from save.";
00138:             }
00139:
00140:             // One-time migration: pre-v3 Verdict saves persisted Verdict quest
00141:             // progress inside the Year of Ash envelope. Fold any quest_verdict_*
00142:             // records found there into the Verdict envelope (Verdict wins on
00143:             // conflict) so no progress is lost when the old save upgrades. This
00144:             // runs whether or not a Verdict save file exists yet.
00145:             var yearOfAshSave = YearOfAshSaveStore.TryLoad();
00146:             if (yearOfAshSave != null && yearOfAshSave.quests != null)
00147:             {
00148:                 int adopted = VerdictQuestMigration.AdoptFromYearOfAsh(session.Quests.State, yearOfAshSave.quests);
00149:                 if (adopted > 0)
00150:                     session.LastEvent = "Migrated " + adopted + " Verdict quest record(s) from the Year of Ash save.";
00151:             }
00152:             return session;
00153:         }
00154:
00155:         /// <summary>Save version loaded at startup (observability; 0 = none).</summary>
00156:         public int LoadedSaveVersion { get; private set; }
00157:         /// <summary>True when the loaded save was migrated to the current version (v1→v2).</summary>
00158:         public bool WasSaveMigrated { get; private set; }
00159:
00160:         /// <summary>Coarse game-time step — call once per sim-day, not per-frame.</summary>
00161:         public void AdvanceDay(int day, int livingCount, int logReadCount)
00162:         {
00163:             _currentDay = day;
00164:             var fired = Reckoning.Poll(day, livingCount, logReadCount, Evidence.Count);
00165:             if (fired.Count > 0) LastEvent = string.Join(";", fired);
00166:         }
00167:
00168:         /// <summary>Feed the census broadcast with the current clock (window check).</summary>
00169:         public void TickCensus()
00170:         {
00171:             Census.BroadcastIfDue();
00172:         }
00173:
00174:         /// <summary>Evaluate the diegetic radio corpus against the current day and phase.
00175:         /// Broadcasts fire once each, gated on the Culpable+ census carrier window.
00176:         /// Returns the ids fired this call (observability).</summary>
00177:         public System.Collections.Generic.List<string> TickRadio(int day)
00178:         {
00179:             if (Radio == null) return new System.Collections.Generic.List<string>();
00180:             var fired = Radio.Poll(day, Reckoning.Phase);
00181:             for (int i = 0; i < fired.Count; i++)
00182:             {
00183:                 var entry = FindRadioEntry(fired[i]);
00184:                 if (entry != null && !string.IsNullOrWhiteSpace(entry.audio_cue))
00185:                 {
00186:                     AudioManager.Instance?.PlayCue(AudioCueCatalog.RadioSignalLock);
00187:                     AudioManager.Instance?.PlayVoiceOverCue(entry.audio_cue);
00188:                     break;
00189:                 }
00190:             }
00191:             if (fired.Count > 0) LastEvent = "radio:" + string.Join(";", fired);
00192:             if (fired.Count > 0) RaiseStateChanged();
00193:             return fired;
00194:         }
00195:
00196:         private VerdictCatalogLoader.VerdictRadioEntry? FindRadioEntry(string id)
00197:         {
00198:             for (int i = 0; i < RadioEntries.Count; i++)
00199:                 if (string.Equals(RadioEntries[i].id, id, StringComparison.Ordinal))
00200:                     return RadioEntries[i];
00201:             return null;
00202:         }
00203:
00204:         /// <summary>
00205:         /// Enroll evidence from the reachable verdict items where the authored
00206:         /// mechanical_effects.enrolled_evidence is non-zero. Idempotent via the
00207:         /// EvidenceLedger; never double-enrolls. Returns count enrolled this call.
00208:         /// </summary>
00209:         public int EnrollEvidenceFromItems(int day)
00210:         {
00211:             if (Items == null) return 0;
00212:             int enrolled = 0;
00213:             for (int i = 0; i < Items.Count; i++)
00214:             {
00215:                 var it = Items[i];
00216:                 if (it == null || string.IsNullOrEmpty(it.id)) continue;
00217:                 int amount = it.mechanical_effects != null ? it.mechanical_effects.enrolled_evidence : 0;
00218:                 if (amount <= 0) continue;
00219:                 if (Evidence.Enroll(it.id, day)) enrolled++;
00220:             }
00221:             if (enrolled > 0)
00222:             {
00223:                 LastEvent = "evidence_from_items:" + enrolled;
00224:                 RaiseStateChanged();
00225:             }
00226:             return enrolled;
00227:         }
00228:
00229:         /// <summary>Corruption tick: in Culpable+ phases, post a data-corrupted log entry
00230:         /// at a deterministic schedule (every 11th day of the countdown). Data-driven corpus.</summary>
00231:         public void TickCorruption(int day)
00232:         {
00233:             if (day > _currentDay) _currentDay = day;
00234:             if (Reckoning.Phase < ReckoningPhase.Culpable) return;
00235:             if (day <= 0 || day % 11 != 0) return; // deterministic cadence, not per-frame
00236:             MachineLog.InsertCorruptionMarker(day, _machineRng, (IReadOnlyList<string>)CorruptionCorpus);
00237:         }
00238:
00239:         public VerdictSave CaptureSave()
00240:         {
00241:             return VerdictSaveCodec.Capture(
00242:                 CurrentDaySafe(), MachineLog, Reckoning, Evidence,
00243:                 Census != null ? Census.LastWindowDay : -1,
00244:                 Npcs, Radio, Quests);
00245:         }
00246:
00247:         public void RestoreSave(VerdictSave save)
00248:         {
00249:             VerdictSaveCodec.Restore(save, MachineLog, Reckoning, Evidence, Npcs, Radio, Quests);
00250:             EvidenceChain.ReconcileReadEntries();
00251:             LastEvent = "Verdict state restored.";
00252:         }
00253:
00254:         public string StatusLine()
00255:         {
00256:             return $"Verdict phase: {Reckoning.Phase}; evidence: {Evidence.Count}; " +
00257:                    $"logs read: {MachineLog.ReadCount()}/{MachineLog.Entries.Count}; " +
00258:                    $"locations: {Locations.Count}; " +
00259:                    $"call: {(Reckoning.State.callResolved ? "RESOLVED" : "OPEN")}";
00260:         }
00261:
00262:         public VerdictCatalogLoader.VerdictLocationEntry? FindLocation(string id)
00263:         {
00264:             if (string.IsNullOrEmpty(id)) return null;
00265:             foreach (var loc in Locations)
00266:                 if (loc.id == id) return loc;
00267:             return null;
00268:         }
00269:
00270:         private int CurrentDaySafe()
00271:         {
00272:             if (_currentDay >= 0) return _currentDay;
00273:
00274:             int latestLogDay = -1;
00275:             for (int i = 0; i < MachineLog.Entries.Count; i++)
00276:             {
00277:                 var entry = MachineLog.Entries[i];
00278:                 if (entry != null && entry.day > latestLogDay)
00279:                     latestLogDay = entry.day;
00280:             }
00281:             return latestLogDay >= 0 ? latestLogDay : 0;
00282:         }
00283:     }
00284: }
```


# Appendix — Current Source Detail: `src/VerdictPanel.cs`

### `src/VerdictPanel.cs` — bounded current excerpt (364 of 432 lines)

- Size: 432 lines / 17793 bytes.
- SHA-256: `880c980f41d127d45df8a9b2d4b42bb34d01603c53f4015a86e39b45345af50e`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003: #pragma warning disable CS8618
00004: using Godot;
00005: using Ashfall.Core.Verdict;
00006: using Ashfall.Core.UI;
00007: using AtomicWar.GodotApp.UI;
00008: using CoreTheme = Ashfall.Core.UI.Theme;
00009:
00010: namespace AtomicWar.GodotApp
00011: {
00012:     /// <summary>
00013:     /// ASHFALL: THE VERDICT (Expansion 08) — shelter machine surface.
00014:     /// Diegetic panel presenting the machine log, the Reckoning phase strip
00015:     /// (phase-colored), the shelter readout, evidence counter, and the
00016:     /// available Verdict figures (flag-gated, one-shot spoken). Thin
00017:     /// presentation only; zero simulation logic.
00018:     /// </summary>
00019:     public partial class VerdictPanel : PanelContainer
00020:     {
00021:         public event System.Action? OnClose;
00022:
00023:         private VerdictHostSession _verdict;
00024:         private Label _lblPhase;
00025:         private Label _lblReadout;
00026:         private VBoxContainer _logList;
00027:         private VBoxContainer _npcList;
00028:         private VBoxContainer _placeList;
00029:         private VBoxContainer _radioList;
00030:
00031:         public void Open()
00032:         {
00033:             Visible = true;
00034:             RefreshView();
00035:         }
00036:
00037:         public void Close()
00038:         {
00039:             Visible = false;
00040:             OnClose?.Invoke();
00041:         }
00042:
00043:         public override void _UnhandledInput(InputEvent @event)
00044:         {
00045:             if (!Visible) return;
00046:             if (@event is InputEventKey key && key.Pressed && !key.Echo && key.Keycode == Key.Escape)
00047:             {
00048:                 Close();
00049:                 GetViewport().SetInputAsHandled();
00050:             }
00051:         }
00052:
00053:         public override void _Ready()
00054:         {
00055:             SetAnchorsPreset(LayoutPreset.FullRect);
00056:             CustomMinimumSize = new Vector2(CoreTheme.PanelMaxWidth, 400);
00057:
00058:             // Apply standard panel 9-slice via shared helper (frame_9slice first)
00059:             AddThemeStyleboxOverride("panel", AshfallUiHelpers.MakePanelFrameStyleBox());
00060:
00061:             var rootVbox = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
00062:             AddChild(rootVbox);
00063:
00064:             // ── Title ──
00065:             rootVbox.AddChild(AshfallUiHelpers.MakeTitle("THE MACHINE'S REGISTER", CoreTheme.FontSizeH3));
00066:
00067:             // ── Phase strip ──
00068:             _lblPhase = new Label { Text = "phase: dormant" };
00069:             _lblPhase.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeBody);
00070:             _lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Muted));
00071:             rootVbox.AddChild(_lblPhase);
00072:
00073:             // ── Readout ──
00074:             _lblReadout = new Label
00075:             {
00076:                 Text = "[shelter instruments] — standby cycle.",
00077:                 AutowrapMode = TextServer.AutowrapMode.WordSmart
00078:             };
00079:             _lblReadout.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeSmall);
00080:             _lblReadout.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
00081:             rootVbox.AddChild(_lblReadout);
00082:
00083:             rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());
00084:
00085:             // ── Log scroll ──
00086:             var scroll = new ScrollContainer
00087:             {
00088:                 HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
00089:                 CustomMinimumSize = new Vector2(0, 190)
00090:             };
00091:             rootVbox.AddChild(scroll);
00092:
00093:             _logList = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingXs);
00094:             scroll.AddChild(_logList);
00095:
00096:             rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());
00097:
00098:             // ── Figures ──
00099:             rootVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("FIGURES OF THE RECORD"));
00100:
00101:             _npcList = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
00102:             rootVbox.AddChild(_npcList);
00103:
00104:             rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());
00105:             rootVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("PLACES & EVIDENCE"));
00106:
00107:             _placeList = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingXs);
00108:             rootVbox.AddChild(_placeList);
00109:
00110:             rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());
00111:             rootVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("TRANSMISSIONS"));
00112:
00113:             var radioScroll = new ScrollContainer
00114:             {
00115:                 HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
00116:                 CustomMinimumSize = new Vector2(0, 170)
00117:             };
00118:             rootVbox.AddChild(radioScroll);
00119:             _radioList = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingXs);
00126:         }
00127:
00128:         public void Bind(VerdictHostSession verdict)
00129:         {
00130:             _verdict = verdict;
00131:             if (verdict != null) verdict.StateChanged += RefreshView;
00132:         }
00133:
00134:         public void RefreshView()
00135:         {
00136:             if (_verdict == null || _logList == null || _radioList == null) return;
00137:
00138:             RefreshPhaseStrip();
00143:         }
00144:
00145:         /// <summary>Thin read-only accessor for tests: the number of broadcast rows
00146:         /// currently rendered in the TRANSMISSIONS section (broadcast labels carry a
00147:         /// "[D{day}]" marker; the summary header label is excluded).</summary>
00148:         public int RenderedRadioRowCount()
00149:         {
00150:             if (_radioList == null) return 0;
00151:             int n = 0;
00152:             foreach (Node child in _radioList.GetChildren())
00153:                 if (child is Label lbl && lbl.Text != null && lbl.Text.Contains("[D")) n++;
00154:             return n;
00155:         }
00156:
00157:         private void RefreshPhaseStrip()
00158:         {
00159:             var state = _verdict.Reckoning.State;
00160:             string phaseName = _verdict.Reckoning.Phase.ToString().ToLowerInvariant();
00161:             string callState = state.callResolved ? " · call RESOLVED" : "";
00162:             _lblPhase.Text = $"phase: {phaseName} · evidence {_verdict.Evidence.Count} · " +
00163:                              $"logs read {_verdict.MachineLog.ReadCount()}/{_verdict.MachineLog.Entries.Count}{callState}";
00164:
00165:             // Phase-colored strip — uses Theme tokens for each phase.
00166:             switch (_verdict.Reckoning.Phase)
00167:             {
00168:                 case ReckoningPhase.Dormant:
00169:                     _lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Muted));
00170:                     break;
00171:                 case ReckoningPhase.Knowing:
00172:                     _lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
00173:                     break;
00174:                 case ReckoningPhase.Culpable:
00175:                     _lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Warm));
00176:                     break;
00177:                 case ReckoningPhase.Counted:
00178:                     _lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Critical));
00179:                     break;
00180:             }
00181:
00182:             _lblReadout.Text = VerdictReadout.LineFor(
00206:                     AutowrapMode = TextServer.AutowrapMode.WordSmart
00207:                 };
00208:                 row.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
00209:                 if (!e.read)
00210:                     row.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
00211:                 else
00212:                     row.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Muted));
00213:                 _logList.AddChild(row);
00214:             }
00215:
00216:             if (shown == 0)
00221:                     AutowrapMode = TextServer.AutowrapMode.WordSmart
00222:                 };
00223:                 empty.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
00224:                 empty.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
00225:                 _logList.AddChild(empty);
00226:             }
00227:         }
00228:
00234:             if (available.Count == 0)
00235:             {
00236:                 var none = new Label
00237:                 {
00238:                     Text = "No figures have stepped forward yet. The record waits.",
00239:                     AutowrapMode = TextServer.AutowrapMode.WordSmart
00240:                 };
00241:                 none.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
00242:                 none.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
00243:                 _npcList.AddChild(none);
00244:                 return;
00245:             }
00246:
00247:             for (int i = 0; i < available.Count; i++)
00251:                 {
00252:                     "tape_echo" => "▤",
00253:                     "paper_ghost" => "✉",
00254:                     "living" => "◉",
00255:                     "readings" => "▥",
00256:                     _ => "·"
00257:                 };
00258:                 string shown = _verdict.Npcs.State.spokenNpcIds.Contains(npc.id) ? "(spoken)" : "";
00259:                 var row = new Label
00260:                 {
00261:                     Text = $"{kindIcon} {npc.name} — {npc.role} {shown}",
00262:                     AutowrapMode = TextServer.AutowrapMode.WordSmart
00263:                 };
00264:                 row.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeSmall);
00265:                 row.AddThemeColorOverride("font_color",
00266:                     _verdict.Npcs.State.spokenNpcIds.Contains(npc.id)
00267:                         ? AshfallUiHelpers.ToColor(CoreTheme.Muted)
00268:                         : AshfallUiHelpers.ToColor(CoreTheme.Pale));
00269:
00270:                 if (!_verdict.Npcs.State.spokenNpcIds.Contains(npc.id))
00271:                 {
00272:                     var btn = AshfallUiHelpers.MakeButton("hear", () =>
00273:                     {
00274:                         if (_verdict.Npcs.Speak(npc.id))
00275:                         {
00276:                             RefreshView();
00277:                             EmitSignal("NpcSpoken", npc.id);
00278:                         }
00292:         /// <summary>Render the reachable Verdict places and evidence/story items.
00293:         /// Thin presentation: lists what the machine's records point to (the four
00294:         /// standing sites and the fifteen evidence/quest objects) as read-only rows.</summary>
00295:         private void RefreshPlaces()
00296:         {
00297:             AshfallUiHelpers.EmptyChildren(_placeList);
00298:
00299:             bool any = false;
00300:             if (_verdict.Locations != null)
00301:             {
00302:                 for (int i = 0; i < _verdict.Locations.Count; i++)
00303:                 {
00304:                     var loc = _verdict.Locations[i];
00305:                     if (loc == null || string.IsNullOrEmpty(loc.displayName)) continue;
00306:                     any = true;
00307:                     var row = new Label
00308:                     {
00309:                         Text = $"▨ {loc.displayName} ({loc.id}) · danger {loc.dangerLevel}\n   {Truncate(loc.description, 180)}",
00310:                         AutowrapMode = TextServer.AutowrapMode.WordSmart
00311:                     };
00312:                     row.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
00313:                     row.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
00314:                     _placeList.AddChild(row);
00315:                 }
00316:             }
00317:             if (_verdict.Items != null)
00320:                 {
00321:                     var it = _verdict.Items[i];
00322:                     if (it == null || string.IsNullOrEmpty(it.id)) continue;
00323:                     any = true;
00324:                     string kind = string.IsNullOrEmpty(it.category) ? "story_item" : it.category;
00325:                     string icon = kind switch
00326:                     {
00327:                         "consumable" => "⊕",
00328:                         "quest_item" => "◆",
00329:                         _ => "◈"
00330:                     };
00331:                     var row = new Label
00334:                         AutowrapMode = TextServer.AutowrapMode.WordSmart
00335:                     };
00336:                     row.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
00337:                     row.AddThemeColorOverride("font_color",
00338:                         it.id.StartsWith("evidence_") ? AshfallUiHelpers.ToColor(CoreTheme.Warm)
00339:                                                        : AshfallUiHelpers.ToColor(CoreTheme.Muted));
00340:                     _placeList.AddChild(row);
00341:                 }
00348:                     AutowrapMode = TextServer.AutowrapMode.WordSmart
00349:                 };
00350:                 empty.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
00351:                 empty.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
00352:                 _placeList.AddChild(empty);
00353:             }
00354:         }
00355:
00356:         /// <summary>Render the diegetic radio corpus (verdict_radio.json). Lists each
00357:         /// broadcast with its dayTrigger and kind, marking fired vs pending. Drivers
00358:         /// from the session's VerdictRadioSystem state. Thin presentation only.</summary>
00359:         private void RefreshRadio()
00360:         {
00361:             AshfallUiHelpers.EmptyChildren(_radioList);
00362:             if (_verdict.Radio == null)
00363:             {
00364:                 _radioList.AddChild(AshfallUiHelpers.MakeSmall("The radio is silent.", true));
00365:                 return;
00366:             }
00367:
00368:             var radio = _verdict.Radio;
00369:             var header = new Label
00370:             {
00371:                 Text = $"{radio.FiredCount}/{radio.Corpus.Count} broadcasts received",
00372:                 AutowrapMode = TextServer.AutowrapMode.WordSmart
00373:             };
00374:             header.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeSmall);
00375:             header.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
00376:             _radioList.AddChild(header);
00377:
00378:             bool any = false;
00379:             if (radio.Corpus != null)
00380:             {
00381:                 for (int i = 0; i < radio.Corpus.Count; i++)
00382:                 {
00383:                     var r = radio.Corpus[i];
00384:                     if (r == null || string.IsNullOrEmpty(r.id)) continue;
00385:                     any = true;
00386:                     bool isFired = radio.HasFired(r.id);
00387:                     string kindIcon = r.kind switch
00388:                     {
00389:                         "carrier" => "◌",
00390:                         "call" => "▸",
00391:                         "maintenance" => "⚙",
00392:                         "witness" => "✉",
00393:                         "readings" => "▥",
00394:                         _ => "·"
00395:                     };
00396:                     var row = new Label
00397:                     {
00398:                         Text = $"{kindIcon} {Truncate(r.message, 90)} \n   [D{r.dayTrigger}] {r.id} · {(isFired ? "RECEIVED" : "pending")}",
00399:                         AutowrapMode = TextServer.AutowrapMode.WordSmart
00400:                     };
00401:                     row.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
00402:                     row.AddThemeColorOverride("font_color",
00403:                         isFired ? AshfallUiHelpers.ToColor(CoreTheme.Pale)
00404:                                 : AshfallUiHelpers.ToColor(CoreTheme.Muted));
00405:                     _radioList.AddChild(row);
00406:                 }
00407:             }
00408:             if (!any)
00409:             {
00410:                 _radioList.AddChild(AshfallUiHelpers.MakeSmall("No broadcasts logged yet.", true));
00411:             }
00412:         }
00413:
00414:         private static string Truncate(string s, int max)
00415:         {
00416:             if (string.IsNullOrEmpty(s) || s.Length <= max) return s ?? string.Empty;
00417:             return s.Substring(0, max) + "…";
00418:         }
00419:
00420:         [Signal]
00421:         public delegate void NpcSpokenEventHandler(string npcId);
00422:
00423:         public override void _ExitTree()
00424:         {
00425:             if (_verdict != null)
00426:             {
00427:                 _verdict.StateChanged -= RefreshView;
00428:             }
00429:             base._ExitTree();
00430:         }
00431:     }
00432: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs`

### `Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs` — complete current file

- Size: 240 lines / 8495 bytes.
- SHA-256: `c63225a0ebdbc8bdb493a8de624bf65d6e098095b7a9bfd5c940b2e30957c6e2`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Verdict;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests.Verdict
00011: {
00012:     public class VerdictNpcExpansionTests : CatalogTestBase
00013:     {
00014:         private static VerdictNpcSystem LoadSystem()
00015:         {
00016:             var system = new VerdictNpcSystem();
00017:             var files = new FileSystemIO();
00018:             var json = new SystemTextJsonSerializer();
00019:             int count = VerdictNpcCatalogLoader.LoadAndRegister(system, DataDirectory, files, json);
00020:             Assert.True(count >= 15, $"Expected at least 15 NPCs registered, got {count}");
00021:             return system;
00022:         }
00023:
00024:         [Fact]
00025:         public void Catalog_Loads_All_18_Npc_Entries()
00026:         {
00027:             var system = LoadSystem();
00028:             Assert.Equal(18, system.Catalog.Count);
00029:             Assert.True(system.Catalog.Count >= 15);
00030:         }
00031:
00032:         [Fact]
00033:         public void All_18_Npc_Ids_Are_Unique_And_Prefixed()
00034:         {
00035:             var system = LoadSystem();
00036:             var ids = system.Catalog.Select(e => e.id).ToList();
00037:             var distinct = ids.Distinct(StringComparer.Ordinal).ToList();
00038:             Assert.Equal(ids.Count, distinct.Count);
00039:             foreach (var id in ids)
00040:             {
00041:                 Assert.True(id.StartsWith("npc_"), $"NPC id '{id}' must start with npc_ prefix");
00042:             }
00043:         }
00044:
00045:         [Fact]
00046:         public void Original_6_Baseline_Npcs_Preserved()
00047:         {
00048:             var system = LoadSystem();
00049:             var baselineIds = new[]
00050:             {
00051:                 "npc_eden_vale",
00052:                 "npc_ferris_voss",
00053:                 "npc_iran_bell",
00054:                 "npc_selya_saltmarsh",
00055:                 "npc_maro_veen",
00056:                 "npc_whisper_cipher"
00057:             };
00058:
00059:             foreach (var bId in baselineIds)
00060:             {
00061:                 var npc = system.Find(bId);
00062:                 Assert.NotNull(npc);
00063:                 Assert.False(string.IsNullOrWhiteSpace(npc.name));
00064:                 Assert.False(string.IsNullOrWhiteSpace(npc.role));
00065:                 Assert.NotEmpty(npc.dialogue);
00066:             }
00067:         }
00068:
00069:         [Fact]
00070:         public void Plan18_Tribunal_Npcs_Preserved()
00071:         {
00072:             var system = LoadSystem();
00073:             var plan18Ids = new[]
00074:             {
00075:                 "npc_tomas_reid",
00076:                 "npc_elena_vane",
00077:                 "npc_kasper_holt"
00078:             };
00079:
00080:             foreach (var pId in plan18Ids)
00081:             {
00082:                 var npc = system.Find(pId);
00083:                 Assert.NotNull(npc);
00084:                 Assert.False(string.IsNullOrWhiteSpace(npc.name));
00085:                 Assert.False(string.IsNullOrWhiteSpace(npc.role));
00086:                 Assert.NotEmpty(npc.dialogue);
00087:             }
00088:         }
00089:
00090:         [Fact]
00091:         public void All_9_Plan93_Investigation_Npcs_Present()
00092:         {
00093:             var system = LoadSystem();
00094:             var plan93Ids = new[]
00095:             {
00096:                 "npc_mara_elsen",
00097:                 "npc_ilya_venn",
00098:                 "npc_garrick_daal",
00099:                 "npc_sena_korr",
00100:                 "npc_torin_rask",
00101:                 "npc_oren_varek",
00102:                 "npc_lena_rost",
00103:                 "npc_tessa_mirn",
00104:                 "npc_karel_norn"
00105:             };
00106:
00107:             foreach (var id in plan93Ids)
00108:             {
00109:                 var npc = system.Find(id);
00110:                 Assert.NotNull(npc);
00111:                 Assert.False(string.IsNullOrWhiteSpace(npc.name));
00112:                 Assert.False(string.IsNullOrWhiteSpace(npc.role));
00113:                 Assert.False(string.IsNullOrWhiteSpace(npc.gatingFlag));
00114:                 Assert.False(string.IsNullOrWhiteSpace(npc.locationId));
00115:                 Assert.InRange(npc.phaseMin, 1, 3);
00116:                 Assert.InRange(npc.dialogue.Count, 2, 4);
00117:                 foreach (var line in npc.dialogue)
00118:                 {
00119:                     Assert.False(string.IsNullOrWhiteSpace(line));
00120:                 }
00121:             }
00122:         }
00123:
00124:         [Fact]
00125:         public void All_Npc_Kinds_Are_Supported()
00126:         {
00127:             var system = LoadSystem();
00128:             var validKinds = new HashSet<string>(StringComparer.Ordinal)
00129:             {
00130:                 "paper_ghost",
00131:                 "tape_echo",
00132:                 "living",
00133:                 "readings"
00134:             };
00135:
00136:             foreach (var npc in system.Catalog)
00137:             {
00138:                 Assert.Contains(npc.kind, validKinds);
00139:             }
00140:         }
00141:
00142:         [Fact]
00143:         public void All_Plan93_LocationIds_Map_To_Distinct_Verdict_Sites()
00144:         {
00145:             var system = LoadSystem();
00146:             var expectedSiteMappings = new Dictionary<string, string>(StringComparer.Ordinal)
00147:             {
00148:                 ["npc_mara_elsen"] = "loc_abandoned_tide_gauge",
00149:                 ["npc_ilya_venn"] = "loc_coastal_meteorological_station",
00150:                 ["npc_garrick_daal"] = "loc_clifftop_observation_bunker",
00151:                 ["npc_sena_korr"] = "loc_sealed_marine_laboratory",
00152:                 ["npc_torin_rask"] = "loc_forestry_survey_post",
00153:                 ["npc_oren_varek"] = "loc_geological_core_vault",
00154:                 ["npc_lena_rost"] = "loc_river_gauging_station",
00155:                 ["npc_tessa_mirn"] = "loc_abandoned_agricultural_station",
00156:                 ["npc_karel_norn"] = "loc_decommissioned_signal_relay"
00157:             };
00158:
00159:             foreach (var kvp in expectedSiteMappings)
00160:             {
00161:                 var npc = system.Find(kvp.Key);
00162:                 Assert.NotNull(npc);
00163:                 Assert.Equal(kvp.Value, npc.locationId);
00164:             }
00165:         }
00166:
00167:         [Fact]
00168:         public void GetAvailable_Filters_By_Phase_And_Flag_And_Location()
00169:         {
00170:             var system = LoadSystem();
00171:             const string npcId = "npc_garrick_daal";
00172:             var npc = system.Find(npcId);
00173:             Assert.NotNull(npc);
00174:             Assert.Equal(2, npc.phaseMin);
00175:             Assert.Equal("flag_verdict_cliff_signal_decoded", npc.gatingFlag);
00176:             Assert.Equal("loc_clifftop_observation_bunker", npc.locationId);
00177:
00178:             var flags = new[] { "flag_verdict_cliff_signal_decoded" };
00179:
00180:             // Phase 1 -> hidden (requires phase 2)
00181:             var p1 = system.GetAvailable(flags, 1, npc.locationId);
00182:             Assert.DoesNotContain(p1, e => e.id == npcId);
00183:
00184:             // Phase 2, flag missing -> hidden
00185:             var noFlag = system.GetAvailable(Array.Empty<string>(), 2, npc.locationId);
00186:             Assert.DoesNotContain(noFlag, e => e.id == npcId);
00187:
00188:             // Phase 2, flag present, wrong location -> hidden
00189:             var wrongLoc = system.GetAvailable(flags, 2, "loc_abandoned_tide_gauge");
00190:             Assert.DoesNotContain(wrongLoc, e => e.id == npcId);
00191:
00192:             // Phase 2, flag present, right location -> visible
00193:             var valid = system.GetAvailable(flags, 2, npc.locationId);
00194:             Assert.Contains(valid, e => e.id == npcId);
00195:
00196:             // Phase 3, flag present, right location -> visible
00197:             var p3 = system.GetAvailable(flags, 3, npc.locationId);
00198:             Assert.Contains(p3, e => e.id == npcId);
00199:         }
00200:
00201:         [Fact]
00202:         public void Speak_Is_OneShot_And_Persists_In_State()
00203:         {
00204:             var system = LoadSystem();
00205:             const string npcId = "npc_mara_elsen";
00206:
00207:             // Speak at correct location
00208:             bool first = system.Speak(npcId, "loc_abandoned_tide_gauge");
00209:             Assert.True(first);
00210:
00211:             // Speak second time -> false (one-shot)
00212:             bool second = system.Speak(npcId, "loc_abandoned_tide_gauge");
00213:             Assert.False(second);
00214:
00215:             // Round-trip state
00216:             var state = system.CaptureState();
00217:             Assert.Contains(npcId, state.spokenNpcIds);
00218:
00219:             var newSystem = LoadSystem();
00220:             newSystem.RestoreState(state);
00221:             Assert.False(newSystem.Speak(npcId, "loc_abandoned_tide_gauge"));
00222:         }
00223:
00224:         [Fact]
00225:         public void Availability_Is_Deterministic_Across_Invocations()
00226:         {
00227:             var system = LoadSystem();
00228:             var allFlags = system.Catalog.Select(e => e.gatingFlag).Where(f => !string.IsNullOrEmpty(f)).ToList();
00229:
00230:             var run1 = system.GetAvailable(allFlags, 3);
00231:             var run2 = system.GetAvailable(allFlags, 3);
00232:
00233:             Assert.Equal(run1.Count, run2.Count);
00234:             for (int i = 0; i < run1.Count; i++)
00235:             {
00236:                 Assert.Equal(run1[i].id, run2[i].id);
00237:             }
00238:         }
00239:     }
00240: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs`

### `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs` — complete current file

- Size: 225 lines / 10025 bytes.
- SHA-256: `abee1d19deb019bb058c9435069600549265c68836d2723c607fccd88c23d334`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004:
00005: using Ashfall.Core.IO;
00006: namespace Ashfall.Core.Verdict
00007: {
00008:     /// <summary>
00009:     /// ASHFALL: THE VERDICT (Expansion 08) — catalog loader for the three
00010:     /// Verdict data files (verdict_data.json, verdict_locations.json,
00011:     /// verdict_radio.json). Authoring authority is the design bible; the loader
00012:     /// mirrors the WitnessCatalogLoader pattern (missing file => empty list,
00013:     /// malformed => empty, engine-agnostic via IFileIO/IJsonSerializer).
00014:     /// </summary>
00015:     public static class VerdictCatalogLoader
00016:     {
00017:         public const string DataFile = "verdict_data.json";
00018:         public const string LocationsFile = "verdict_locations.json";
00019:         public const string ItemsFile = "verdict_items.json";
00020:         public const string RadioFile = "verdict_radio.json";
00021:
00022:         // ── Locations ───────────────────────────────────────────────────────────
00023:
00024:         public class VerdictLocationEntry
00025:         {
00026:             public string id = string.Empty;
00027:             public string displayName = string.Empty;
00028:             public string description = string.Empty;
00029:             public int dangerLevel = 5;
00030:             public float travelHours = 5f;
00031:             public float baseRadsPerHour = 30f;
00032:         }
00033:
00034:         public static List<VerdictLocationEntry> LoadLocations(
00035:             string dataDir, IFileIO fileIO, IJsonSerializer json)
00036:         {
00037:             var result = new List<VerdictLocationEntry>();
00038:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return result;
00039:             string path = fileIO.Combine(dataDir, LocationsFile);
00040:             if (!fileIO.FileExists(path)) return result;
00041:             string raw = fileIO.ReadAllText(path);
00042:             if (string.IsNullOrWhiteSpace(raw)) return result;
00043:             try
00044:             {
00045:                 var list = CatalogLocator.LoadWrappedList<VerdictLocationEntry>(raw, SystemTextJsonSerializer.Options);
00046:                 for (int i = 0; i < list.Count; i++)
00047:                 {
00048:                     var e = list[i];
00049:                     if (e == null || string.IsNullOrEmpty(e.id)) continue;
00050:                     result.Add(e);
00051:                 }
00052:             }
00053:             catch (Exception ex_CATDIAG)
00054:             {
00055:                 CatalogDiagnostics.Warn(path, "VerdictLocationEntry list", ex_CATDIAG);
00056:                 return result;
00057:             }
00058:             return result;
00059:         }
00060:
00061:         // ── Items ───────────────────────────────────────────────────────────────
00062:
00063:         /// <summary>One Verdict story/evidence item row. Runtime-schema compatible
00064:         /// (id/displayName/weightKg/tradeValue/category/description) with optional
00065:         /// enrichments the bible authors (tier, mechanical_effects, etc.). Loaded
00066:         /// only so the story content is reachable; never treated as loot.</summary>
00067:         /// <summary>Optional effect payload of an evidence/story item (recorded for
00068:         /// reachability; the game enrolls evidence through the EvidenceLedger, not
00069:         /// this mirror). Mirrors the authored JSON shape.</summary>
00070:         public class VerdictItemEffects
00071:         {
00072:             public int enrolled_evidence;
00073:             public string note = string.Empty;
00074:         }
00075:
00076:         public class VerdictItemEntry
00077:         {
00078:             public string id = string.Empty;
00079:             public string displayName = string.Empty;
00080:             public float weightKg;
00081:             public float tradeValue;
00082:             public string category = "story_item";
00083:             public string tier = string.Empty;
00084:             public string description = string.Empty;
00085:             public VerdictItemEffects mechanical_effects = null!;
00086:             public string downstream_quest_trigger = string.Empty;
00087:             public string faction_affinity = string.Empty;
00088:             public string rarity = string.Empty;
00089:         }
00090:
00091:         public static List<VerdictItemEntry> LoadItems(
00092:             string dataDir, IFileIO fileIO, IJsonSerializer json)
00093:         {
00094:             var result = new List<VerdictItemEntry>();
00095:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return result;
00096:             string path = fileIO.Combine(dataDir, ItemsFile);
00097:             if (!fileIO.FileExists(path)) return result;
00098:             string raw = fileIO.ReadAllText(path);
00099:             if (string.IsNullOrWhiteSpace(raw)) return result;
00100:             try
00101:             {
00102:                 var entries = CatalogLocator.LoadWrappedList<VerdictItemEntry>(raw, SystemTextJsonSerializer.Options);
00103:                 for (int i = 0; i < entries.Count; i++)
00104:                 {
00105:                     var e = entries[i];
00106:                     if (e == null || string.IsNullOrEmpty(e.id)) continue;
00107:                     result.Add(e);
00108:                 }
00109:             }
00110:             catch (Exception ex_CATDIAG)
00111:             {
00112:                 CatalogDiagnostics.Warn(path, "VerdictItemEntry list", ex_CATDIAG);
00113:                 return result;
00114:             }
00115:             return result;
00116:         }
00117:
00118:         // ── Radio ───────────────────────────────────────────────────────────────
00119:
00120:         public class VerdictRadioEntry
00121:         {
00122:             public string id = string.Empty;
00123:             public string frequency = string.Empty;
00124:             public int dayTrigger = 180;
00125:             public string source = string.Empty;
00126:             public string message = string.Empty;
00127:             public string signalStrength = string.Empty;
00128:             public string kind = "telemetry";
00129:             public string audio_cue = string.Empty;
00130:         }
00131:
00132:         public static List<VerdictRadioEntry> LoadRadio(
00133:             string dataDir, IFileIO fileIO, IJsonSerializer json)
00134:         {
00135:             var result = new List<VerdictRadioEntry>();
00136:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return result;
00137:             string path = fileIO.Combine(dataDir, RadioFile);
00138:             if (!fileIO.FileExists(path)) return result;
00139:             string raw = fileIO.ReadAllText(path);
00140:             if (string.IsNullOrWhiteSpace(raw)) return result;
00141:             try
00142:             {
00143:                 var parsed = json.Deserialize<VerdictRadioContainer>(raw);
00144:                 if (parsed?.broadcasts == null) return result;
00145:                 foreach (var e in parsed.broadcasts)
00146:                 {
00147:                     if (e == null || string.IsNullOrEmpty(e.id)) continue;
00148:                     result.Add(e);
00149:                 }
00150:             }
00151:             catch (Exception ex_CATDIAG)
00152:             {
00153:                 CatalogDiagnostics.Warn(path, "VerdictRadioContainer", ex_CATDIAG);
00154:                 return result;
00155:             }
00156:             return result;
00157:         }
00158:
00159:         public class VerdictWorldHistoryLadderEntry
00160:         {
00161:             public int layer { get; set; }
00162:             public string knowledge_key { get; set; } = string.Empty;
00163:             public string title { get; set; } = string.Empty;
00164:             public string discovery_location_id { get; set; } = string.Empty;
00165:             public string body_summary { get; set; } = string.Empty;
00166:         }
00167:
00168:         private class VerdictDataContainer
00169:         {
00170:             public List<string> corruption_corpus = new List<string>();
00171:             public List<VerdictWorldHistoryLadderEntry> world_history_ladder = new List<VerdictWorldHistoryLadderEntry>();
00172:         }
00173:
00174:         /// <summary>Load the corruption corpus from verdict_data.json (empty if missing).</summary>
00175:         public static List<string> LoadCorruptionCorpus(
00176:             string dataDir, IFileIO fileIO, IJsonSerializer json)
00177:         {
00178:             var result = new List<string>();
00179:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return result;
00180:             string path = fileIO.Combine(dataDir, DataFile);
00181:             if (!fileIO.FileExists(path)) return result;
00182:             string raw = fileIO.ReadAllText(path);
00183:             if (string.IsNullOrWhiteSpace(raw)) return result;
00184:             try
00185:             {
00186:                 var parsed = json.Deserialize<VerdictDataContainer>(raw);
00187:                 if (parsed?.corruption_corpus != null)
00188:                     result.AddRange(parsed.corruption_corpus);
00189:             }
00190:             catch (Exception ex_CATDIAG)
00191:             {
00192:                 CatalogDiagnostics.Warn(path, "VerdictDataContainer", ex_CATDIAG);
00193:             }
00194:             return result;
00195:         }
00196:
00197:         /// <summary>Load the world history ladder from verdict_data.json (empty if missing).</summary>
00198:         public static List<VerdictWorldHistoryLadderEntry> LoadWorldHistoryLadder(
00199:             string dataDir, IFileIO fileIO, IJsonSerializer json)
00200:         {
00201:             var result = new List<VerdictWorldHistoryLadderEntry>();
00202:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return result;
00203:             string path = fileIO.Combine(dataDir, DataFile);
00204:             if (!fileIO.FileExists(path)) return result;
00205:             string raw = fileIO.ReadAllText(path);
00206:             if (string.IsNullOrWhiteSpace(raw)) return result;
00207:             try
00208:             {
00209:                 var parsed = json.Deserialize<VerdictDataContainer>(raw);
00210:                 if (parsed?.world_history_ladder != null)
00211:                     result.AddRange(parsed.world_history_ladder);
00212:             }
00213:             catch (Exception ex_CATDIAG)
00214:             {
00215:                 CatalogDiagnostics.Warn(path, "VerdictDataContainer.world_history_ladder", ex_CATDIAG);
00216:             }
00217:             return result;
00218:         }
00219:
00220:         private class VerdictRadioContainer
00221:         {
00222:             public List<VerdictRadioEntry> broadcasts = new List<VerdictRadioEntry>();
00223:         }
00224:     }
00225: }
```


# Appendix — Current Source Detail: `src/Host/HostCli.ExpansionDepth.cs`

### `src/Host/HostCli.ExpansionDepth.cs` — complete current file

- Size: 144 lines / 7946 bytes.
- SHA-256: `c2e979fba2fe17db74263330c5ba2382dfc878788a22ca81b8df117457f89ff8`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using Godot;
00003: using System;
00004: using System.Collections.Generic;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Narrative;
00008:
00009: namespace AtomicWar.GodotApp
00010: {
00011:     public static partial class HostCli
00012:     {
00013:         /// <summary>
00014:         /// --expansion-depth-selftest / --plan18-selftest:
00015:         /// Verifies Plan 18 Expansion Deepening:
00016:         /// Holdfast (24 quests), Standing Record (52 memories, 22 quests),
00017:         /// Crossing (20 quests, 14 encounters), Verdict (16 questlines, 9 NPCs),
00018:         /// cross-expansion evidence hooks, and save stability.
00019:         /// </summary>
00020:         public static int RunExpansionDepthSelfTest(string dataDirectory)
00021:         {
00022:             CatalogLocator.UseInvariantCulture();
00023:             int failures = 0;
00024:             int totalAssertions = 0;
00025:
00026:             void Check(bool ok, string label)
00027:             {
00028:                 totalAssertions++;
00029:                 GD.Print($"[{(ok ? "PASS" : "FAIL")}] {label}");
00030:                 if (!ok) failures++;
00031:             }
00032:
00033:             GD.Print("[ExpansionDepthHeadlessDemo] begin Plan 18 verification...");
00034:
00035:             var json = new SystemTextJsonSerializer();
00036:             var files = new FileSystemIO();
00037:
00038:             // 1. Holdfast (24 quests, 38 locations)
00039:             var holdfastCatalog = new HoldfastCatalogLoader(files, json, NullLog.Instance).Load(dataDirectory);
00040:             Check(holdfastCatalog != null, "Holdfast catalog loaded");
00041:             Check(holdfastCatalog != null && holdfastCatalog.Quests.Count >= 22, $"Holdfast quests count (expected >= 22, got {holdfastCatalog?.Quests.Count ?? 0})");
00042:             Check(holdfastCatalog != null && holdfastCatalog.Locations.Count >= 30, $"Holdfast locations count (expected >= 30, got {holdfastCatalog?.Locations.Count ?? 0})");
00043:             Check(holdfastCatalog != null && holdfastCatalog.GetQuest("quest_holdfast_salt_convoy_haul") != null, "Holdfast salt convoy haul quest present");
00044:             Check(holdfastCatalog != null && holdfastCatalog.GetQuest("quest_holdfast_census_claimant_audit") != null, "Holdfast census claimant audit quest present");
00045:             Check(holdfastCatalog != null && holdfastCatalog.GetQuest("quest_holdfast_brine_boiler_scum") != null, "Holdfast brine boiler scum quest present");
00046:
00047:             // 2. Standing Record (52 memories, 22 quests, 14 layouts)
00048:             var standingRecordCat = new StandingRecordCatalogLoader(files, json, NullLog.Instance).Load(dataDirectory);
00049:             Check(standingRecordCat != null && standingRecordCat.Quests.Count >= 22, $"Standing Record quests count (expected >= 22, got {standingRecordCat?.Quests.Count ?? 0})");
00050:             Check(standingRecordCat != null && standingRecordCat.GetQuest("quest_record_vault_breach_forensics") != null, "Standing Record vault breach quest present");
00051:             Check(standingRecordCat != null && standingRecordCat.GetQuest("quest_record_the_unmarked_plaque") != null, "Standing Record memorial plaque quest present");
00052:
00053:             var memSys = new LocationMemorySystem(files, json, NullLog.Instance);
00054:             memSys.Load(dataDirectory);
00055:             Check(memSys.StratumCount >= 50, $"Standing Record memories count (expected >= 50, got {memSys.StratumCount})");
00056:
00057:             var layoutSys = new LocationLayoutSystem(files, json, NullLog.Instance);
00058:             layoutSys.Load(dataDirectory);
00059:             Check(layoutSys.LayoutCount >= 14, $"Standing Record layouts count (expected >= 14, got {layoutSys.LayoutCount})");
00060:
00061:             // 3. Crossing (20 quests, 14 encounters)
00062:             var crossingSession = CrossingSession.Load(dataDirectory, NullLog.Instance);
00063:             var crossingCatalog = crossingSession?.Catalog;
00064:             Check(crossingCatalog != null, "Crossing catalog loaded");
00065:             Check(crossingCatalog != null && crossingCatalog.Quests.Count >= 20, $"Crossing quests count (expected >= 20, got {crossingCatalog?.Quests.Count ?? 0})");
00066:             Check(crossingCatalog != null && crossingCatalog.Encounters.Count >= 14, $"Crossing encounters count (expected >= 14, got {crossingCatalog?.Encounters.Count ?? 0})");
00067:             Check(crossingCatalog != null && crossingCatalog.GetQuest("quest_crossing_asylum_in_the_truss") != null, "Crossing asylum quest present");
00068:             Check(crossingCatalog != null && crossingCatalog.GetEncounter("enc_nc_mass_crossing_surge") != null, "Crossing mass surge crisis present");
00069:
00070:             // 4. Verdict (16 questlines, 9 NPCs)
00071:             string qlPath = files.Combine(dataDirectory, "verdict_questlines.json");
00072:             if (files.FileExists(qlPath))
00073:             {
00074:                 var root = json.Deserialize<VerdictQuestlinesRoot>(files.ReadAllText(qlPath));
00075:                 Check(root != null && root.quests != null && root.quests.Count >= 16, $"Verdict questlines count (expected >= 16, got {root?.quests?.Count ?? 0})");
00076:                 Check(root != null && root.quests != null && root.quests.Any(q => q.questlineId == "quest_verdict_alibi_verification"), "Verdict alibi verification questline present");
00077:                 Check(root != null && root.quests != null && root.quests.Any(q => q.questlineId == "quest_verdict_prior_verdict_appeal"), "Verdict prior verdict appeal questline present");
00078:             }
00079:             else
00080:             {
00081:                 Check(false, "verdict_questlines.json exists");
00082:             }
00083:
00084:             string npcPath = files.Combine(dataDirectory, "verdict_npcs.json");
00085:             if (files.FileExists(npcPath))
00086:             {
00087:                 var root = json.Deserialize<VerdictNpcsRoot>(files.ReadAllText(npcPath));
00088:                 Check(root != null && root.items != null && root.items.Count >= 9, $"Verdict NPCs count (expected >= 9, got {root?.items?.Count ?? 0})");
00089:                 Check(root != null && root.items != null && root.items.Any(n => n.id == "npc_tomas_reid"), "Verdict defense clerk Tomas Reid present");
00090:                 Check(root != null && root.items != null && root.items.Any(n => n.id == "npc_elena_vane"), "Verdict cult deaconess Elena Vane present");
00091:             }
00092:             else
00093:             {
00094:                 Check(false, "verdict_npcs.json exists");
00095:             }
00096:
00097:             // 5. Questline Master sync check (437 entries)
00098:             string masterPath = files.Combine(dataDirectory, "questline_master.json");
00099:             if (files.FileExists(masterPath))
00100:             {
00101:                 var master = json.Deserialize<QuestlineMasterRoot>(files.ReadAllText(masterPath));
00102:                 Check(master != null && master.entries != null && master.entries.Count >= 400, $"Questline master entries count (expected >= 400, got {master?.entries?.Count ?? 0})");
00103:             }
00104:
00105:             GD.Print($"[ExpansionDepthHeadlessDemo] completed with {failures} failures across {totalAssertions} assertions.");
00106:             return failures == 0 ? 0 : 1;
00107:         }
00108:
00109:         private sealed class VerdictQuestlinesRoot
00110:         {
00111:             public int schema_version { get; set; }
00112:             public List<VerdictQuestlineDef>? quests { get; set; }
00113:         }
00114:
00115:         private sealed class VerdictQuestlineDef
00116:         {
00117:             public string questlineId { get; set; } = string.Empty;
00118:             public string title { get; set; } = string.Empty;
00119:         }
00120:
00121:         private sealed class VerdictNpcsRoot
00122:         {
00123:             public int schema_version { get; set; }
00124:             public List<VerdictNpcDef>? items { get; set; }
00125:         }
00126:
00127:         private sealed class VerdictNpcDef
00128:         {
00129:             public string id { get; set; } = string.Empty;
00130:             public string name { get; set; } = string.Empty;
00131:         }
00132:
00133:         private sealed class QuestlineMasterRoot
00134:         {
00135:             public int schema_version { get; set; }
00136:             public List<QuestlineMasterEntryDef>? entries { get; set; }
00137:         }
00138:
00139:         private sealed class QuestlineMasterEntryDef
00140:         {
00141:             public string id { get; set; } = string.Empty;
00142:         }
00143:     }
00144: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs`

### `Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs` — complete current file

- Size: 217 lines / 9783 bytes.
- SHA-256: `4fc5a94aeab9132f1282222f25b67d05edb8cb60f9a05cfee8b41b23b505087e`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Verdict;
00008: using Ashfall.Core.YearOfAsh;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests.Verdict
00012: {
00013:     /// <summary>
00014:     /// Cross-system integration tests for Wave 40 Batch 5:
00015:     /// - Plan 93 (DEC-263): Verdict NPCs Expansion (6 -> 18 investigation-site NPCs)
00016:     /// - Plan 101 (DEC-264): Radiation Dose Quests Expansion (4 -> 12 dose-ledger questlines)
00017:     ///
00018:     /// Validates referential integrity, investigation site coverage, phase gating,
00019:     /// dose quest stage transitions, and thematic alignment between archival
00020:     /// dosimetry records and clinical shelter radiation dilemmas.
00021:     /// </summary>
00022:     public sealed class Plan93_101VerdictDoseQuestIntegrationTests
00023:     {
00024:         private static string ResolveDataDir()
00025:         {
00026:             string candidate = Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data");
00027:             if (Directory.Exists(candidate)) return candidate;
00028:
00029:             var dir = new DirectoryInfo(AppContext.BaseDirectory);
00030:             while (dir != null)
00031:             {
00032:                 string check = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
00033:                 if (Directory.Exists(check)) return check;
00034:                 dir = dir.Parent;
00035:             }
00036:
00037:             string current = Directory.GetCurrentDirectory();
00038:             if (CatalogLocator.TryFindDataDirectory(current, out string found))
00039:                 return found;
00040:
00041:             throw new DirectoryNotFoundException("Could not locate Assets/StreamingAssets/Data directory.");
00042:         }
00043:
00044:         [Fact]
00045:         public void Plan93_VerdictNpcCatalog_LoadsAll18Npcs_WithValidGatingAndDialogue()
00046:         {
00047:             string dataDir = ResolveDataDir();
00048:             var io = new FileSystemIO();
00049:             var json = new SystemTextJsonSerializer();
00050:             var system = new VerdictNpcSystem();
00051:
00052:             int loadedCount = VerdictNpcCatalogLoader.LoadAndRegister(system, dataDir, io, json);
00053:
00054:             Assert.Equal(18, loadedCount);
00055:             Assert.Equal(18, system.Catalog.Count);
00056:
00057:             var validKinds = new HashSet<string>(StringComparer.Ordinal)
00058:             {
00059:                 "tape_echo",
00060:                 "paper_ghost",
00061:                 "living",
00062:                 "readings"
00063:             };
00064:
00065:             var seenIds = new HashSet<string>(StringComparer.Ordinal);
00066:             foreach (var npc in system.Catalog)
00067:             {
00068:                 Assert.True(npc.id.StartsWith("npc_"), $"NPC id must start with npc_: {npc.id}");
00069:                 Assert.False(string.IsNullOrWhiteSpace(npc.name), $"Name must not be empty for {npc.id}");
00070:                 Assert.False(string.IsNullOrWhiteSpace(npc.role), $"Role must not be empty for {npc.id}");
00071:                 Assert.Contains(npc.kind, validKinds);
00072:                 Assert.True(npc.phaseMin >= 1 && npc.phaseMin <= 3, $"phaseMin must be 1..3 for {npc.id}");
00073:                 Assert.True(npc.gatingFlag.StartsWith("flag_verdict_"), $"gatingFlag must start with flag_verdict_: {npc.id}");
00074:                 Assert.False(string.IsNullOrWhiteSpace(npc.locationId), $"locationId must not be empty for {npc.id}");
00075:                 Assert.NotNull(npc.dialogue);
00076:                 Assert.NotEmpty(npc.dialogue);
00077:                 Assert.True(seenIds.Add(npc.id), $"Duplicate NPC id: {npc.id}");
00078:             }
00079:
00080:             // Verify original baseline NPCs
00081:             var eden = system.Find("npc_eden_vale");
00082:             Assert.NotNull(eden);
00083:             Assert.Equal("Eden Vale", eden.name);
00084:             Assert.Equal("tape_echo", eden.kind);
00085:
00086:             var voss = system.Find("npc_ferris_voss");
00087:             Assert.NotNull(voss);
00088:             Assert.Equal("Ferris Voss", voss.name);
00089:             Assert.Equal("paper_ghost", voss.kind);
00090:         }
00091:
00092:         [Fact]
00093:         public void Plan101_DoseQuestCatalog_LoadsAll12Questlines_WithValidTransitions()
00094:         {
00095:             string dataDir = ResolveDataDir();
00096:             var io = new FileSystemIO();
00097:             var json = new SystemTextJsonSerializer();
00098:
00099:             var catalog = DoseContentCatalogLoader.Load(dataDir, io, json);
00100:
00101:             Assert.NotNull(catalog.quests);
00102:             Assert.Equal(12, catalog.quests.Count);
00103:
00104:             var questMap = catalog.quests.ToDictionary(q => q.questlineId, StringComparer.Ordinal);
00105:
00106:             // Verify all canonical questline IDs match DoseQuestMigration
00107:             Assert.Equal(12, DoseQuestMigration.CanonicalQuestlineIds.Length);
00108:             foreach (var canonicalId in DoseQuestMigration.CanonicalQuestlineIds)
00109:             {
00110:                 Assert.True(questMap.ContainsKey(canonicalId), $"Missing canonical dose questline: {canonicalId}");
00111:                 Assert.True(DoseQuestMigration.IsDoseQuestline(canonicalId));
00112:             }
00113:
00114:             // Verify stage transitions and terminal integrity
00115:             foreach (var quest in catalog.quests)
00116:             {
00117:                 Assert.False(string.IsNullOrWhiteSpace(quest.title), $"Title empty for {quest.questlineId}");
00118:                 Assert.False(string.IsNullOrWhiteSpace(quest.synopsis), $"Synopsis empty for {quest.questlineId}");
00119:                 Assert.NotNull(quest.stages);
00120:                 Assert.NotEmpty(quest.stages);
00121:
00122:                 var stageIds = quest.stages.Select(s => s.stageId).ToHashSet(StringComparer.Ordinal);
00123:                 bool hasTerminal = false;
00124:
00125:                 foreach (var stage in quest.stages)
00126:                 {
00127:                     Assert.False(string.IsNullOrWhiteSpace(stage.stageId));
00128:                     Assert.False(string.IsNullOrWhiteSpace(stage.narrativePrompt));
00129:
00130:                     if (stage.isTerminal)
00131:                     {
00132:                         hasTerminal = true;
00133:                     }
00134:                     else
00135:                     {
00136:                         Assert.NotNull(stage.choices);
00137:                         Assert.NotEmpty(stage.choices);
00138:                         foreach (var choice in stage.choices)
00139:                         {
00140:                             Assert.False(string.IsNullOrWhiteSpace(choice.choiceId));
00141:                             Assert.False(string.IsNullOrWhiteSpace(choice.text));
00142:                             Assert.True(stageIds.Contains(choice.nextStageId),
00143:                                 $"nextStageId '{choice.nextStageId}' in {quest.questlineId} does not resolve to an existing stage.");
00144:                         }
00145:                     }
00146:                 }
00147:
00148:                 Assert.True(hasTerminal, $"Questline {quest.questlineId} must have at least one terminal stage.");
00149:             }
00150:         }
00151:
00152:         [Fact]
00153:         public void CrossSystem_VerdictArchivistsAndDosimetryQuests_ExhibitNarrativeCoherence()
00154:         {
00155:             string dataDir = ResolveDataDir();
00156:             var io = new FileSystemIO();
00157:             var json = new SystemTextJsonSerializer();
00158:
00159:             var system = new VerdictNpcSystem();
00160:             VerdictNpcCatalogLoader.LoadAndRegister(system, dataDir, io, json);
00161:             var doseCatalog = DoseContentCatalogLoader.Load(dataDir, io, json);
00162:             var questMap = doseCatalog.quests.ToDictionary(q => q.questlineId, StringComparer.Ordinal);
00163:
00164:             // 1. Archival dosimetrist at archive tape silo aligns with calibration dispute quests
00165:             var kasper = system.Find("npc_kasper_holt");
00166:             Assert.NotNull(kasper);
00167:             Assert.Equal("loc_archive_tape_silo", kasper.locationId);
00168:             Assert.Contains("custodian", kasper.role.ToLowerInvariant());
00169:
00170:             var calibrationQuest = questMap["quest_the_broken_calibration_chain"];
00171:             Assert.NotNull(calibrationQuest);
00172:             Assert.Contains("calibration", calibrationQuest.title.ToLowerInvariant());
00173:
00174:             // 2. Radiobiology researcher at marine lab aligns with acute exposure hospice dilemmas
00175:             var sena = system.Find("npc_sena_korr");
00176:             Assert.NotNull(sena);
00177:             Assert.Equal("loc_sealed_marine_laboratory", sena.locationId);
00178:             Assert.Contains("researcher", sena.role.ToLowerInvariant());
00179:
00180:             var sickRoomQuest = questMap["quest_the_sick_of_room_seven"];
00181:             Assert.NotNull(sickRoomQuest);
00182:             Assert.Contains("sick", sickRoomQuest.title.ToLowerInvariant());
00183:
00184:             // 3. Census clerk tracking population count aligns with register audits
00185:             var selya = system.Find("npc_selya_saltmarsh");
00186:             Assert.NotNull(selya);
00187:             Assert.Equal("loc_twelve_gauge_array", selya.locationId);
00188:             Assert.Contains("census", selya.role.ToLowerInvariant());
00189:
00190:             var auditQuest = questMap["quest_the_register_audit"];
00191:             Assert.NotNull(auditQuest);
00192:             Assert.Contains("audit", auditQuest.title.ToLowerInvariant());
00193:         }
00194:
00195:         [Fact]
00196:         public void CrossSystem_DeterministicExecution_UnderSimulationPasses()
00197:         {
00198:             string dataDir = ResolveDataDir();
00199:             var io = new FileSystemIO();
00200:             var json = new SystemTextJsonSerializer();
00201:
00202:             // Run 50 reloads/queries to confirm deterministic stability
00203:             for (int i = 0; i < 50; i++)
00204:             {
00205:                 var system = new VerdictNpcSystem();
00206:                 int npcCount = VerdictNpcCatalogLoader.LoadAndRegister(system, dataDir, io, json);
00207:                 Assert.Equal(18, npcCount);
00208:
00209:                 var tapeSiloNpcs = system.Catalog.Where(n => n.locationId == "loc_archive_tape_silo").ToList();
00210:                 Assert.Equal(3, tapeSiloNpcs.Count); // Maro Veen, Elena Vane, Kasper Holt
00211:
00212:                 var doseCatalog = DoseContentCatalogLoader.Load(dataDir, io, json);
00213:                 Assert.Equal(12, doseCatalog.quests.Count);
00214:             }
00215:         }
00216:     }
00217: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/VerdictSystemTests.cs`

### `Ashfall.Core.Tests/VerdictSystemTests.cs` — bounded current excerpt (677 of 723 lines)

- Size: 723 lines / 28015 bytes.
- SHA-256: `f25ace785950670bd5a6ef2436b246b418a95a82c41be19a2745faf76a4bbd06`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Text.Json;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Clock;
00008: using Ashfall.Core.Events;
00009: using Ashfall.Core.Flags;
00010: using Ashfall.Core.Verdict;
00011: using Xunit;
00012:
00013: namespace Ashfall.Core.Tests
00014: {
00015:     /// <summary>
00016:     /// ASHFALL: THE VERDICT (Expansion 08) — unit tests for the nine Verdict
00017:     /// systems that previously had only integration-level coverage.
00018:     /// </summary>
00019:     public class VerdictUnitTests
00020:     {
00021:         // ── EvidenceLedger ──────────────────────────────────────────────────────
00022:
00023:         [Fact]
00024:         public void Evidence_Enroll_IsIdempotent()
00025:         {
00026:             var ledger = new EvidenceLedger();
00027:             ledger.Register(new EvidenceDefinition { id = "ev_a" });
00028:             Assert.True(ledger.Enroll("ev_a", 160));
00029:             Assert.False(ledger.Enroll("ev_a", 161));
00030:             Assert.Equal(1, ledger.Count);
00031:         }
00032:
00033:         [Fact]
00034:         public void Evidence_Enroll_RejectsUnknown_WhenCatalogPopulated()
00035:         {
00036:             var ledger = new EvidenceLedger();
00037:             ledger.Register(new EvidenceDefinition { id = "ev_a" });
00038:             Assert.False(ledger.Enroll("ev_unknown", 160));
00039:             Assert.Equal(0, ledger.Count);
00040:         }
00041:
00042:         [Fact]
00043:         public void Evidence_Enroll_AllowsAny_WhenCatalogEmpty()
00044:         {
00045:             var ledger = new EvidenceLedger();
00046:             Assert.True(ledger.Enroll("ev_any", 160));
00047:             Assert.Equal(1, ledger.Count);
00048:         }
00049:
00050:         [Fact]
00051:         public void Evidence_FiresEvent()
00052:         {
00053:             var ledger = new EvidenceLedger();
00054:             string fired = null;
00055:             ledger.OnEnrolled += id => fired = id;
00056:             ledger.Enroll("ev_x", 200);
00057:             Assert.Equal("ev_x", fired);
00058:         }
00059:
00060:         [Fact]
00061:         public void Evidence_CaptureRestore_Roundtrip()
00062:         {
00063:             var ledger = new EvidenceLedger();
00064:             ledger.Enroll("ev_1", 160);
00065:             ledger.Enroll("ev_2", 180);
00066:             var snap = ledger.CaptureState();
00067:
00068:             var restored = new EvidenceLedger();
00069:             restored.RestoreState(snap);
00070:             Assert.Equal(2, restored.Count);
00071:             Assert.True(restored.IsEnrolled("ev_1"));
00072:             Assert.True(restored.IsEnrolled("ev_2"));
00073:         }
00074:
00075:         [Fact]
00076:         public void Evidence_RejectNullEmpty()
00077:         {
00078:             var ledger = new EvidenceLedger();
00079:             Assert.False(ledger.Enroll("", 0));
00080:             Assert.False(ledger.Enroll(null, 0));
00081:             Assert.False(ledger.IsEnrolled(""));
00082:             Assert.False(ledger.IsEnrolled(null));
00083:         }
00084:
00085:         // ── MachineLogSystem ────────────────────────────────────────────────────
00086:
00087:         [Fact]
00088:         public void MachineLog_Post_DuplicateSuppression()
00089:         {
00090:             var log = new MachineLogSystem();
00091:             Assert.True(log.Post("fac_a", 160, "operating", "body", "ev_a"));
00092:             Assert.False(log.Post("fac_a", 160, "operating", "body2", "ev_b"));
00093:             Assert.Single(log.Entries);
00094:         }
00095:
00096:         [Fact]
00097:         public void MachineLog_Post_DifferentKind_Allowed()
00098:         {
00099:             var log = new MachineLogSystem();
00100:             Assert.True(log.Post("fac_a", 160, "operating", "body", "ev_a"));
00101:             Assert.True(log.Post("fac_a", 160, "maintenance", "body2", "ev_b"));
00104:
00105:         [Fact]
00106:         public void MachineLog_ReadEntry_OneWay()
00107:         {
00108:             var log = new MachineLogSystem();
00109:             log.Post("fac_a", 160, "operating", "body", "ev_a");
00110:             Assert.Equal("ev_a", log.ReadEntry(0));
00115:
00116:         [Fact]
00117:         public void MachineLog_ReadEntry_OutOfRange()
00118:         {
00119:             var log = new MachineLogSystem();
00120:             Assert.Equal(string.Empty, log.ReadEntry(-1));
00121:             Assert.Equal(string.Empty, log.ReadEntry(0));
00124:
00125:         [Fact]
00126:         public void MachineLog_CorruptionMarker_Deterministic()
00127:         {
00128:             var log1 = new MachineLogSystem();
00129:             var log2 = new MachineLogSystem();
00130:             var rng1 = new SeededRng(42);
00131:             var rng2 = new SeededRng(42);
00132:             log1.InsertCorruptionMarker(170, rng1);
00133:             log2.InsertCorruptionMarker(170, rng2);
00134:             Assert.Equal(log1.Entries[0].bodyShort, log2.Entries[0].bodyShort);
00135:         }
00136:
00137:         [Fact]
00138:         public void MachineLog_SpinTape_OnePerDay()
00139:         {
00140:             var log = new MachineLogSystem();
00141:             int spins = 0;
00142:             log.OnTapeSpin += () => spins++;
00143:             log.SpinTape(160);
00144:             log.SpinTape(160);
00145:             log.SpinTape(161);
00146:             Assert.Equal(2, spins);
00148:
00149:         [Fact]
00150:         public void MachineLog_CaptureRestore_Roundtrip()
00151:         {
00152:             var log = new MachineLogSystem();
00153:             log.Post("fac_a", 160, "operating", "body", "ev_a");
00154:             log.ReadEntry(0);
00155:             log.SpinTape(160);
00156:             var snap = log.CaptureState();
00157:
00158:             var restored = new MachineLogSystem();
00159:             restored.RestoreState(snap);
00160:             Assert.Single(restored.Entries);
00161:             Assert.True(restored.Entries[0].read);
00162:             Assert.Equal(160, restored.State.lastTapeSpinDay);
00163:         }
00164:
00165:         [Fact]
00166:         public void MachineLog_CaptureRestore_DoesNotAliasEntries()
00167:         {
00168:             var log = new MachineLogSystem();
00169:             log.Post("fac_a", 160, "operating", "body", "ev_a");
00170:
00171:             var captured = log.CaptureState();
00172:             captured.entries[0].bodyShort = "mutated save";
00173:             Assert.Equal("body", log.Entries[0].bodyShort);
00174:
00175:             var restored = new MachineLogSystem();
00176:             restored.RestoreState(captured);
00177:             captured.entries[0].bodyShort = "mutated after restore";
00178:             Assert.Equal("mutated save", restored.Entries[0].bodyShort);
00179:         }
00180:
00181:         [Fact]
00182:         public void VerdictEvidenceChain_ReadEnrollsLedgerAndReckoningExactlyOnce()
00183:         {
00184:             var log = new MachineLogSystem();
00185:             var ledger = new EvidenceLedger();
00186:             var reckoning = new ReckoningSystem();
00187:             var chain = new VerdictEvidenceChain(log, ledger, reckoning);
00188:
00189:             log.Post("fac_a", 162, "maintenance", "read this", "evidence_geophone_hymn");
00190:             Assert.Equal("evidence_geophone_hymn", log.ReadEntry(0));
00191:             Assert.Equal(1, ledger.Count);
00192:             Assert.Equal(1, reckoning.State.enrolledEvidence);
00193:
00194:             Assert.Equal(string.Empty, log.ReadEntry(0));
00195:             Assert.Equal(0, chain.ReconcileReadEntries());
00196:             Assert.Equal(1, ledger.Count);
00197:             Assert.Equal(1, reckoning.State.enrolledEvidence);
00198:         }
00199:
00200:         [Fact]
00201:         public void VerdictEvidenceChain_ReconcileIsSafeAfterSaveRestore()
00202:         {
00203:             var log = new MachineLogSystem();
00204:             var ledger = new EvidenceLedger();
00205:             var reckoning = new ReckoningSystem();
00206:             var chain = new VerdictEvidenceChain(log, ledger, reckoning);
00207:             log.Post("fac_a", 170, "operating", "read this", "evidence_fuse_linen");
00208:             log.ReadEntry(0);
00209:
00210:             var logB = new MachineLogSystem();
00211:             logB.RestoreState(log.CaptureState());
00212:             var ledgerB = new EvidenceLedger();
00213:             ledgerB.RestoreState(ledger.CaptureState());
00214:             var reckoningB = new ReckoningSystem();
00215:             reckoningB.RestoreState(reckoning.CaptureState());
00216:             var chainB = new VerdictEvidenceChain(logB, ledgerB, reckoningB);
00217:
00218:             Assert.Equal(0, chainB.ReconcileReadEntries());
00219:             Assert.Equal(1, ledgerB.Count);
00220:             Assert.Equal(1, reckoningB.State.enrolledEvidence);
00221:         }
00222:
00223:         [Fact]
00224:         public void VerdictEvidenceChain_ReconcileRepairsDerivedReckoningCount()
00225:         {
00226:             var log = new MachineLogSystem();
00227:             log.Post("fac_a", 170, "operating", "read this", "evidence_fuse_linen");
00228:             log.ReadEntry(0);
00230:             var ledger = new EvidenceLedger();
00231:             ledger.Enroll("evidence_fuse_linen", 170);
00232:             var reckoning = new ReckoningSystem();
00233:             reckoning.EnrollEvidence(3);
00234:             var chain = new VerdictEvidenceChain(log, ledger, reckoning);
00235:
00236:             chain.ReconcileReadEntries();
00237:             Assert.Equal(1, reckoning.State.enrolledEvidence);
00238:         }
00239:
00240:         [Fact]
00241:         public void MachineLog_Post_RejectsEmptyFacility()
00242:         {
00243:             var log = new MachineLogSystem();
00244:             Assert.False(log.Post("", 160, "operating", "body", "ev"));
00245:             Assert.False(log.Post(null, 160, "operating", "body", "ev"));
00246:         }
00247:
00248:         // ── ReckoningSystem ─────────────────────────────────────────────────────
00249:
00250:         [Fact]
00251:         public void Reckoning_Dormant_BeforeDay160()
00252:         {
00253:             var r = new ReckoningSystem();
00254:             var fired = r.Poll(159, 14, 0, 0);
00255:             Assert.Equal(ReckoningPhase.Dormant, r.Phase);
00256:             Assert.Empty(fired);
00257:         }
00258:
00259:         [Fact]
00260:         public void Reckoning_Knowing_AtDay160()
00261:         {
00262:             var r = new ReckoningSystem();
00263:             var fired = r.Poll(160, 14, 1, 0);
00264:             Assert.Equal(ReckoningPhase.Knowing, r.Phase);
00265:             Assert.Contains("phase_knowing", fired);
00266:         }
00267:
00268:         [Fact]
00269:         public void Reckoning_Culpable_NeedsEvidence()
00270:         {
00271:             var r = new ReckoningSystem();
00272:             r.Poll(160, 14, 1, 0);
00273:             var fired = r.Poll(210, 14, 2, 0);
00274:             Assert.Equal(ReckoningPhase.Knowing, r.Phase);
00275:             Assert.Empty(fired);
00276:
00277:             r.EnrollEvidence(1);
00278:             fired = r.Poll(210, 14, 2, 0);
00279:             Assert.Equal(ReckoningPhase.Culpable, r.Phase);
00280:             Assert.Contains("carrier_heard", fired);
00281:         }
00282:
00283:         [Fact]
00284:         public void Reckoning_Counted_AtDay240()
00285:         {
00286:             var r = new ReckoningSystem();
00287:             r.Poll(160, 14, 1, 0);
00288:             r.EnrollEvidence(1);
00289:             r.Poll(210, 14, 2, 0);
00290:             var fired = r.Poll(240, 14, 3, 1);
00291:             Assert.Equal(ReckoningPhase.Counted, r.Phase);
00292:             Assert.Contains("reckoning_call", fired);
00293:             Assert.True(r.State.callResolved);
00294:         }
00295:
00296:         [Fact]
00297:         public void Reckoning_CallIsOneShot()
00298:         {
00299:             var r = new ReckoningSystem();
00300:             r.Poll(160, 14, 1, 0);
00301:             r.EnrollEvidence(1);
00302:             r.Poll(210, 14, 2, 0);
00303:             r.Poll(240, 14, 3, 1);
00304:             var fired = r.Poll(250, 14, 3, 1);
00305:             Assert.DoesNotContain("reckoning_call", fired);
00306:         }
00307:
00308:         [Fact]
00309:         public void Reckoning_NeverReverses()
00310:         {
00311:             var r = new ReckoningSystem();
00312:             r.Poll(160, 14, 1, 0);
00313:             r.EnrollEvidence(1);
00314:             r.Poll(210, 14, 2, 0);
00315:             r.Poll(240, 14, 3, 1);
00316:             Assert.Equal(ReckoningPhase.Counted, r.Phase);
00317:             r.Poll(300, 14, 3, 1);
00318:             Assert.Equal(ReckoningPhase.Counted, r.Phase);
00319:         }
00320:
00321:         [Fact]
00322:         public void Reckoning_SelectEnding_MutuallyExclusive()
00323:         {
00324:             var r = new ReckoningSystem();
00325:             r.Poll(160, 14, 1, 0);
00326:             r.EnrollEvidence(1);
00327:             r.Poll(210, 14, 2, 0);
00328:             r.Poll(240, 14, 3, 1);
00335:
00336:         [Fact]
00337:         public void Reckoning_SelectEnding_RejectsBeforeCounted()
00338:         {
00339:             var r = new ReckoningSystem();
00340:             r.Poll(160, 14, 1, 0);
00341:             Assert.False(r.SelectEnding("ending_verdict_the_sector_recounts", 170));
00342:         }
00343:
00344:         [Fact]
00345:         public void Reckoning_SelectEnding_RejectsUnknown()
00346:         {
00347:             var r = new ReckoningSystem();
00348:             r.Poll(160, 14, 1, 0);
00349:             r.EnrollEvidence(1);
00350:             r.Poll(210, 14, 2, 0);
00351:             r.Poll(240, 14, 3, 1);
00354:
00355:         [Fact]
00356:         public void Reckoning_CensusWindow_OpenInCulpable()
00357:         {
00358:             var r = new ReckoningSystem();
00359:             Assert.False(r.IsCensusWindowOpen(210));
00360:             r.Poll(160, 14, 1, 0);
00361:             r.EnrollEvidence(1);
00362:             r.Poll(210, 14, 2, 0);
00365:
00366:         [Fact]
00367:         public void Reckoning_CaptureRestore_Roundtrip()
00368:         {
00369:             var r = new ReckoningSystem();
00370:             r.Poll(160, 14, 1, 0);
00371:             r.EnrollEvidence(2);
00372:             r.Poll(210, 14, 2, 2);
00373:             var snap = r.CaptureState();
00374:
00375:             var restored = new ReckoningSystem();
00376:             restored.RestoreState(snap);
00377:             Assert.Equal(ReckoningPhase.Culpable, restored.Phase);
00378:             Assert.True(restored.State.carrierHeard);
00379:             Assert.Equal(2, restored.State.enrolledEvidence);
00380:         }
00381:
00382:         // ── VerdictEndingEvaluator ──────────────────────────────────────────────
00383:
00384:         [Fact]
00385:         public void EndingEvaluator_ResolvedEnding_Priority()
00386:         {
00387:             var s = new ReckoningState { countPresented = true };
00388:             Assert.Equal(VerdictEndingEvaluator.EndingKeyCounted, VerdictEndingEvaluator.ResolvedEnding(s));
00389:
00390:             var s2 = new ReckoningState { countHeld = true };
00391:             Assert.Equal(VerdictEndingEvaluator.EndingKeyHeld, VerdictEndingEvaluator.ResolvedEnding(s2));
00392:
00393:             var s3 = new ReckoningState { offerIsLease = true };
00394:             Assert.Equal(VerdictEndingEvaluator.EndingKeyLease, VerdictEndingEvaluator.ResolvedEnding(s3));
00395:         }
00396:
00397:         [Fact]
00398:         public void EndingEvaluator_NullState_ReturnsNull()
00399:         {
00400:             Assert.Null(VerdictEndingEvaluator.ResolvedEnding(null));
00401:             Assert.Null(VerdictEndingEvaluator.DecideEnding(null, 0, 240));
00402:         }
00403:
00404:         [Fact]
00405:         public void EndingEvaluator_DecideEnding_FallsBackByEvidence()
00406:         {
00407:             var s = new ReckoningState { phase = ReckoningPhase.Counted };
00408:             Assert.Equal(VerdictEndingEvaluator.EndingKeyCounted,
00409:                 VerdictEndingEvaluator.DecideEnding(s, 5, 240));
00410:             Assert.Equal(VerdictEndingEvaluator.EndingKeyHeld,
00411:                 VerdictEndingEvaluator.DecideEnding(s, 2, 240));
00413:
00414:         [Fact]
00415:         public void EndingEvaluator_DecideEnding_NullBeforeCounted()
00416:         {
00417:             var s = new ReckoningState { phase = ReckoningPhase.Knowing };
00418:             Assert.Null(VerdictEndingEvaluator.DecideEnding(s, 10, 200));
00419:         }
00420:
00421:         [Fact]
00422:         public void EndingEvaluator_TempestDecommissioned_OnlyOnRecount()
00423:         {
00424:             Assert.True(VerdictEndingEvaluator.IsTempestDecommissioned(
00425:                 new ReckoningState { countPresented = true }));
00426:             Assert.False(VerdictEndingEvaluator.IsTempestDecommissioned(
00427:                 new ReckoningState { countHeld = true }));
00428:             Assert.False(VerdictEndingEvaluator.IsTempestDecommissioned(
00429:                 new ReckoningState { offerIsLease = true }));
00430:         }
00431:
00432:         // ── VerdictReadout ──────────────────────────────────────────────────────
00433:
00434:         [Fact]
00435:         public void Readout_Dormant_WhenStateNull()
00436:         {
00437:             var line = VerdictReadout.LineFor(null, 0, 0);
00438:             Assert.Contains("shelter instruments", line);
00439:         }
00440:
00441:         [Fact]
00442:         public void Readout_Knowing_InPhase()
00443:         {
00444:             var s = new ReckoningState { phase = ReckoningPhase.Knowing };
00445:             var line = VerdictReadout.LineFor(s, 1, 1);
00446:             Assert.Contains("shelter instruments", line);
00447:         }
00448:
00449:         [Fact]
00450:         public void Readout_NegativeOrOverflowingCounters_StayBounded()
00451:         {
00452:             var knowing = new ReckoningState { phase = ReckoningPhase.Knowing };
00453:             var culpable = new ReckoningState { phase = ReckoningPhase.Culpable };
00454:
00455:             var negative = VerdictReadout.LineFor(knowing, enrolledEvidence: -1, readCount: 0);
00456:             var overflow = VerdictReadout.LineFor(
00457:                 culpable,
00459:                 readCount: 1);
00460:
00461:             Assert.Contains("shelter instruments", negative);
00462:             Assert.Contains("shelter instruments", overflow);
00463:         }
00464:
00465:         [Fact]
00466:         public void Readout_Resolved_WhenCountPresented()
00467:         {
00468:             var s = new ReckoningState { countPresented = true };
00469:             var line = VerdictReadout.LineFor(s, 5, 5);
00470:             Assert.Contains("signature received", line.ToLower());
00471:         }
00472:
00473:         // ── VerdictNpcSystem ────────────────────────────────────────────────────
00474:
00475:         [Fact]
00476:         public void Npc_Register_Find()
00477:         {
00478:             var npcs = new VerdictNpcSystem();
00479:             npcs.Register(new VerdictNpcEntry { id = "npc_a", name = "A" });
00480:             Assert.NotNull(npcs.Find("npc_a"));
00483:
00484:         [Fact]
00485:         public void Npc_Speak_OneShot()
00486:         {
00487:             var npcs = new VerdictNpcSystem();
00488:             npcs.Register(new VerdictNpcEntry { id = "npc_a" });
00489:             Assert.True(npcs.Speak("npc_a"));
00490:             Assert.False(npcs.Speak("npc_a"));
00491:             Assert.Single(npcs.State.spokenNpcIds);
00492:         }
00493:
00494:         [Fact]
00495:         public void Npc_GetAvailable_RespectsPhase()
00496:         {
00497:             var npcs = new VerdictNpcSystem();
00498:             npcs.Register(new VerdictNpcEntry { id = "npc_a", phaseMin = 2 });
00499:             var avail1 = npcs.GetAvailable(new List<string>(), 1);
00500:             Assert.Empty(avail1);
00501:             var avail2 = npcs.GetAvailable(new List<string>(), 2);
00502:             Assert.Single(avail2);
00503:         }
00504:
00505:         [Fact]
00506:         public void Npc_GetAvailable_RespectsGatingFlag()
00507:         {
00508:             var npcs = new VerdictNpcSystem();
00509:             npcs.Register(new VerdictNpcEntry { id = "npc_a", gatingFlag = "flag_x" });
00510:             Assert.Empty(npcs.GetAvailable(new List<string>(), 3));
00511:             Assert.Single(npcs.GetAvailable(new List<string> { "flag_x" }, 3));
00512:         }
00513:
00514:         [Fact]
00515:         public void Npc_CaptureRestore_Roundtrip()
00516:         {
00517:             var npcs = new VerdictNpcSystem();
00518:             npcs.Register(new VerdictNpcEntry { id = "npc_a" });
00519:             npcs.Speak("npc_a");
00520:             var snap = npcs.CaptureState();
00521:
00522:             var restored = new VerdictNpcSystem();
00523:             restored.RestoreState(snap);
00524:             Assert.Contains("npc_a", restored.State.spokenNpcIds);
00525:         }
00526:
00527:         // ── VerdictSave (codec) ─────────────────────────────────────────────────
00528:
00529:         [Fact]
00530:         public void Save_CaptureEncode_DecodeRestore_Roundtrip()
00531:         {
00532:             var log = new MachineLogSystem();
00533:             log.Post("fac", 160, "operating", "body", "ev");
00534:             var reck = new ReckoningSystem();
00535:             reck.Poll(160, 14, 1, 0);
00536:             var evidence = new EvidenceLedger();
00537:             evidence.Enroll("ev", 160);
00538:             var json = new SystemTextJsonSerializer();
00539:
00540:             var save = VerdictSaveCodec.Capture(160, log, reck, evidence, -1);
00541:             string encoded = VerdictSaveCodec.Encode(save, json);
00542:             Assert.True(VerdictSaveCodec.TryDecode(encoded, json, out var decoded));
00543:             Assert.Equal(160, decoded.simDay);
00544:
00545:             var log2 = new MachineLogSystem();
00546:             var reck2 = new ReckoningSystem();
00547:             var evidence2 = new EvidenceLedger();
00548:             VerdictSaveCodec.Restore(decoded, log2, reck2, evidence2);
00549:             Assert.Single(log2.Entries);
00550:             Assert.Equal(ReckoningPhase.Knowing, reck2.Phase);
00551:             Assert.Equal(1, evidence2.Count);
00552:         }
00553:
00554:         [Fact]
00555:         public void Save_TamperRejection()
00556:         {
00557:             var log = new MachineLogSystem();
00558:             var reck = new ReckoningSystem();
00559:             var evidence = new EvidenceLedger();
00560:             var json = new SystemTextJsonSerializer();
00561:
00562:             var save = VerdictSaveCodec.Capture(160, log, reck, evidence, -1);
00563:             string encoded = VerdictSaveCodec.Encode(save, json);
00564:             string tampered = encoded.Replace("\"simDay\":160", "\"simDay\":999");
00565:             Assert.False(VerdictSaveCodec.TryDecode(tampered, json, out _));
00566:         }
00567:
00568:         [Fact]
00569:         public void Save_RejectsEmptyChecksum()
00570:         {
00571:             var json = new SystemTextJsonSerializer();
00572:             var save = new VerdictSave { Checksum = "" };
00573:             string encoded = json.Serialize(save);
00574:             Assert.False(VerdictSaveCodec.TryDecode(encoded, json, out _));
00575:         }
00576:
00577:         [Fact]
00578:         public void Save_RejectsNewerVersion()
00579:         {
00580:             var json = new SystemTextJsonSerializer();
00581:             var save = new VerdictSave { saveVersion = 999, Checksum = "x" };
00582:             string encoded = json.Serialize(save);
00583:             Assert.False(VerdictSaveCodec.TryDecode(encoded, json, out _));
00584:         }
00585:
00586:         // ── VerdictCensusBroadcast ──────────────────────────────────────────────
00587:
00588:         private class StubClock : ISimClock
00589:         {
00590:             private int _dayIndex;
00591:             private int _hourOfDay;
00592:             public int DayIndex { get => _dayIndex; set { _dayIndex = value; } }
00593:             public int HourOfDay { get => _hourOfDay; set { _hourOfDay = value; } }
00594:             public long CurrentTick => (long)_dayIndex * 1440 + (long)_hourOfDay * 60;
00595:             public void AdvanceTicks(long ticks) { _hourOfDay += (int)(ticks / 60); if (_hourOfDay >= 24) { _dayIndex += _hourOfDay / 24; _hourOfDay %= 24; } }
00596:             public void AdvanceHours(int hours) { AdvanceTicks(hours * 60L); }
00597:             public void AdvanceDays(int days) { _dayIndex += days; }
00598:         }
00599:
00600:         private class StubCensus : IWorldCensus
00601:         {
00602:             public long Count { get; set; }
00603:             public long LivingRegisteredSouls() => Count;
00604:         }
00605:
00606:         [Fact]
00607:         public void Census_WindowOpen_Every7DaysAt03()
00608:         {
00609:             var clock = new StubClock { DayIndex = 210, HourOfDay = 3 };
00610:             var census = new VerdictCensusBroadcast(
00611:                 clock, new SimpleEventBus(), new InMemoryFlagLedger(),
00621:
00622:         [Fact]
00623:         public void Census_BroadcastOnce_PerWindow()
00624:         {
00625:             var clock = new StubClock { DayIndex = 210, HourOfDay = 3 };
00626:             var bus = new SimpleEventBus();
00627:             var flags = new InMemoryFlagLedger();
00637:
00638:         [Fact]
00639:         public void Census_SilentAfterSigning()
00640:         {
00641:             var clock = new StubClock { DayIndex = 210, HourOfDay = 3 };
00642:             var bus = new SimpleEventBus();
00643:             var flags = new InMemoryFlagLedger();
00644:             flags.Set("flag_exp08_signed_reckoning");
00645:             var census = new VerdictCensusBroadcast(
00646:                 clock, bus, flags, new SeededRng(1), new StubCensus { Count = 14 });
00647:
00648:             int headers = 0;
00653:
00654:         [Fact]
00655:         public void Census_CanonConstants()
00656:         {
00657:             Assert.Equal(4.0, VerdictCensusBroadcast.CarrierSeconds);
00658:             Assert.Equal(1.7, VerdictCensusBroadcast.HeldBreathPauseSeconds);
00659:             Assert.Equal(211004, VerdictCensusBroadcast.ExpectedProvincialCount);
00660:         }
00661:
00662:         // ── VerdictCatalogLoader (locations) ────────────────────────────────────
00663:
00664:         [Fact]
00665:         public void CatalogLoader_Locations_ReturnsEmpty_WhenFileMissing()
00666:         {
00667:             var io = new FileSystemIO();
00668:             var json = new SystemTextJsonSerializer();
00669:             var result = VerdictCatalogLoader.LoadLocations("/nonexistent", io, json);
00670:             Assert.Empty(result);
00671:         }
00672:
00673:         [Fact]
00674:         public void CatalogLoader_Locations_ReturnsEmpty_WhenNullArgs()
00675:         {
00676:             Assert.Empty(VerdictCatalogLoader.LoadLocations(null, null, null));
00677:             Assert.Empty(VerdictCatalogLoader.LoadLocations("", new FileSystemIO(), new SystemTextJsonSerializer()));
00678:         }
00679:
00680:         // ── Verdict items JSON schema validation ────────────────────────────────
00681:
00682:         [Fact]
00683:         public void VerdictItemsJson_MatchesRuntimeSchema()
00684:         {
00685:             string dataDir;
00686:             if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
00687:                 CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);
00688:             if (string.IsNullOrEmpty(dataDir)) return;
00689:
00690:             var io = new FileSystemIO();
00691:             var json = new SystemTextJsonSerializer();
00692:             string path = io.Combine(dataDir, "verdict_items.json");
00693:             if (!io.FileExists(path)) return;
00694:
00695:             string raw = io.ReadAllText(path);
00696:             using var doc = JsonDocument.Parse(raw);
00697:             JsonElement array = doc.RootElement;
00698:             if (array.ValueKind == JsonValueKind.Object)
00699:             {
00700:                 foreach (var prop in array.EnumerateObject())
00701:                 {
00702:                     if (prop.Name.Equals("schema_version", StringComparison.OrdinalIgnoreCase))
00703:                         continue;
00704:                     if (prop.Value.ValueKind == JsonValueKind.Array)
00705:                     {
00706:                         array = prop.Value;
00707:                         break;
00708:                     }
00709:                 }
00710:             }
00711:             var items = CatalogLocator.LoadWrappedList<Ashfall.Core.Inventory.ItemDefinition>(array.GetRawText(), SystemTextJsonSerializer.Options);
00712:             Assert.NotNull(items);
00713:             Assert.True(items.Count >= 15, $"expected >=15 verdict items, got {items?.Count ?? 0}");
00714:
00715:             foreach (var item in items!)
00716:             {
00717:                 Assert.NotNull(item);
00718:                 Assert.False(string.IsNullOrEmpty(item!.id), $"item has empty id");
00719:                 Assert.False(string.IsNullOrEmpty(item.displayName), $"item {item.id} has empty displayName");
00720:             }
00721:         }
00722:     }
00723: }
```


# Appendix — Focused Evidence Detail: `Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs`

### `Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs` — complete current file

- Size: 240 lines / 8495 bytes.
- SHA-256: `c63225a0ebdbc8bdb493a8de624bf65d6e098095b7a9bfd5c940b2e30957c6e2`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Verdict;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests.Verdict
00011: {
00012:     public class VerdictNpcExpansionTests : CatalogTestBase
00013:     {
00014:         private static VerdictNpcSystem LoadSystem()
00015:         {
00016:             var system = new VerdictNpcSystem();
00017:             var files = new FileSystemIO();
00018:             var json = new SystemTextJsonSerializer();
00019:             int count = VerdictNpcCatalogLoader.LoadAndRegister(system, DataDirectory, files, json);
00020:             Assert.True(count >= 15, $"Expected at least 15 NPCs registered, got {count}");
00021:             return system;
00022:         }
00023:
00024:         [Fact]
00025:         public void Catalog_Loads_All_18_Npc_Entries()
00026:         {
00027:             var system = LoadSystem();
00028:             Assert.Equal(18, system.Catalog.Count);
00029:             Assert.True(system.Catalog.Count >= 15);
00030:         }
00031:
00032:         [Fact]
00033:         public void All_18_Npc_Ids_Are_Unique_And_Prefixed()
00034:         {
00035:             var system = LoadSystem();
00036:             var ids = system.Catalog.Select(e => e.id).ToList();
00037:             var distinct = ids.Distinct(StringComparer.Ordinal).ToList();
00038:             Assert.Equal(ids.Count, distinct.Count);
00039:             foreach (var id in ids)
00040:             {
00041:                 Assert.True(id.StartsWith("npc_"), $"NPC id '{id}' must start with npc_ prefix");
00042:             }
00043:         }
00044:
00045:         [Fact]
00046:         public void Original_6_Baseline_Npcs_Preserved()
00047:         {
00048:             var system = LoadSystem();
00049:             var baselineIds = new[]
00050:             {
00051:                 "npc_eden_vale",
00052:                 "npc_ferris_voss",
00053:                 "npc_iran_bell",
00054:                 "npc_selya_saltmarsh",
00055:                 "npc_maro_veen",
00056:                 "npc_whisper_cipher"
00057:             };
00058:
00059:             foreach (var bId in baselineIds)
00060:             {
00061:                 var npc = system.Find(bId);
00062:                 Assert.NotNull(npc);
00063:                 Assert.False(string.IsNullOrWhiteSpace(npc.name));
00064:                 Assert.False(string.IsNullOrWhiteSpace(npc.role));
00065:                 Assert.NotEmpty(npc.dialogue);
00066:             }
00067:         }
00068:
00069:         [Fact]
00070:         public void Plan18_Tribunal_Npcs_Preserved()
00071:         {
00072:             var system = LoadSystem();
00073:             var plan18Ids = new[]
00074:             {
00075:                 "npc_tomas_reid",
00076:                 "npc_elena_vane",
00077:                 "npc_kasper_holt"
00078:             };
00079:
00080:             foreach (var pId in plan18Ids)
00081:             {
00082:                 var npc = system.Find(pId);
00083:                 Assert.NotNull(npc);
00084:                 Assert.False(string.IsNullOrWhiteSpace(npc.name));
00085:                 Assert.False(string.IsNullOrWhiteSpace(npc.role));
00086:                 Assert.NotEmpty(npc.dialogue);
00087:             }
00088:         }
00089:
00090:         [Fact]
00091:         public void All_9_Plan93_Investigation_Npcs_Present()
00092:         {
00093:             var system = LoadSystem();
00094:             var plan93Ids = new[]
00095:             {
00096:                 "npc_mara_elsen",
00097:                 "npc_ilya_venn",
00098:                 "npc_garrick_daal",
00099:                 "npc_sena_korr",
00100:                 "npc_torin_rask",
00101:                 "npc_oren_varek",
00102:                 "npc_lena_rost",
00103:                 "npc_tessa_mirn",
00104:                 "npc_karel_norn"
00105:             };
00106:
00107:             foreach (var id in plan93Ids)
00108:             {
00109:                 var npc = system.Find(id);
00110:                 Assert.NotNull(npc);
00111:                 Assert.False(string.IsNullOrWhiteSpace(npc.name));
00112:                 Assert.False(string.IsNullOrWhiteSpace(npc.role));
00113:                 Assert.False(string.IsNullOrWhiteSpace(npc.gatingFlag));
00114:                 Assert.False(string.IsNullOrWhiteSpace(npc.locationId));
00115:                 Assert.InRange(npc.phaseMin, 1, 3);
00116:                 Assert.InRange(npc.dialogue.Count, 2, 4);
00117:                 foreach (var line in npc.dialogue)
00118:                 {
00119:                     Assert.False(string.IsNullOrWhiteSpace(line));
00120:                 }
00121:             }
00122:         }
00123:
00124:         [Fact]
00125:         public void All_Npc_Kinds_Are_Supported()
00126:         {
00127:             var system = LoadSystem();
00128:             var validKinds = new HashSet<string>(StringComparer.Ordinal)
00129:             {
00130:                 "paper_ghost",
00131:                 "tape_echo",
00132:                 "living",
00133:                 "readings"
00134:             };
00135:
00136:             foreach (var npc in system.Catalog)
00137:             {
00138:                 Assert.Contains(npc.kind, validKinds);
00139:             }
00140:         }
00141:
00142:         [Fact]
00143:         public void All_Plan93_LocationIds_Map_To_Distinct_Verdict_Sites()
00144:         {
00145:             var system = LoadSystem();
00146:             var expectedSiteMappings = new Dictionary<string, string>(StringComparer.Ordinal)
00147:             {
00148:                 ["npc_mara_elsen"] = "loc_abandoned_tide_gauge",
00149:                 ["npc_ilya_venn"] = "loc_coastal_meteorological_station",
00150:                 ["npc_garrick_daal"] = "loc_clifftop_observation_bunker",
00151:                 ["npc_sena_korr"] = "loc_sealed_marine_laboratory",
00152:                 ["npc_torin_rask"] = "loc_forestry_survey_post",
00153:                 ["npc_oren_varek"] = "loc_geological_core_vault",
00154:                 ["npc_lena_rost"] = "loc_river_gauging_station",
00155:                 ["npc_tessa_mirn"] = "loc_abandoned_agricultural_station",
00156:                 ["npc_karel_norn"] = "loc_decommissioned_signal_relay"
00157:             };
00158:
00159:             foreach (var kvp in expectedSiteMappings)
00160:             {
00161:                 var npc = system.Find(kvp.Key);
00162:                 Assert.NotNull(npc);
00163:                 Assert.Equal(kvp.Value, npc.locationId);
00164:             }
00165:         }
00166:
00167:         [Fact]
00168:         public void GetAvailable_Filters_By_Phase_And_Flag_And_Location()
00169:         {
00170:             var system = LoadSystem();
00171:             const string npcId = "npc_garrick_daal";
00172:             var npc = system.Find(npcId);
00173:             Assert.NotNull(npc);
00174:             Assert.Equal(2, npc.phaseMin);
00175:             Assert.Equal("flag_verdict_cliff_signal_decoded", npc.gatingFlag);
00176:             Assert.Equal("loc_clifftop_observation_bunker", npc.locationId);
00177:
00178:             var flags = new[] { "flag_verdict_cliff_signal_decoded" };
00179:
00180:             // Phase 1 -> hidden (requires phase 2)
00181:             var p1 = system.GetAvailable(flags, 1, npc.locationId);
00182:             Assert.DoesNotContain(p1, e => e.id == npcId);
00183:
00184:             // Phase 2, flag missing -> hidden
00185:             var noFlag = system.GetAvailable(Array.Empty<string>(), 2, npc.locationId);
00186:             Assert.DoesNotContain(noFlag, e => e.id == npcId);
00187:
00188:             // Phase 2, flag present, wrong location -> hidden
00189:             var wrongLoc = system.GetAvailable(flags, 2, "loc_abandoned_tide_gauge");
00190:             Assert.DoesNotContain(wrongLoc, e => e.id == npcId);
00191:
00192:             // Phase 2, flag present, right location -> visible
00193:             var valid = system.GetAvailable(flags, 2, npc.locationId);
00194:             Assert.Contains(valid, e => e.id == npcId);
00195:
00196:             // Phase 3, flag present, right location -> visible
00197:             var p3 = system.GetAvailable(flags, 3, npc.locationId);
00198:             Assert.Contains(p3, e => e.id == npcId);
00199:         }
00200:
00201:         [Fact]
00202:         public void Speak_Is_OneShot_And_Persists_In_State()
00203:         {
00204:             var system = LoadSystem();
00205:             const string npcId = "npc_mara_elsen";
00206:
00207:             // Speak at correct location
00208:             bool first = system.Speak(npcId, "loc_abandoned_tide_gauge");
00209:             Assert.True(first);
00210:
00211:             // Speak second time -> false (one-shot)
00212:             bool second = system.Speak(npcId, "loc_abandoned_tide_gauge");
00213:             Assert.False(second);
00214:
00215:             // Round-trip state
00216:             var state = system.CaptureState();
00217:             Assert.Contains(npcId, state.spokenNpcIds);
00218:
00219:             var newSystem = LoadSystem();
00220:             newSystem.RestoreState(state);
00221:             Assert.False(newSystem.Speak(npcId, "loc_abandoned_tide_gauge"));
00222:         }
00223:
00224:         [Fact]
00225:         public void Availability_Is_Deterministic_Across_Invocations()
00226:         {
00227:             var system = LoadSystem();
00228:             var allFlags = system.Catalog.Select(e => e.gatingFlag).Where(f => !string.IsNullOrEmpty(f)).ToList();
00229:
00230:             var run1 = system.GetAvailable(allFlags, 3);
00231:             var run2 = system.GetAvailable(allFlags, 3);
00232:
00233:             Assert.Equal(run1.Count, run2.Count);
00234:             for (int i = 0; i < run1.Count; i++)
00235:             {
00236:                 Assert.Equal(run1[i].id, run2[i].id);
00237:             }
00238:         }
00239:     }
00240: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **The subject is human residue inside the existing Verdict investigation authority: authored NPC rows, live progress flags, site/phase availability, one-shot speech and versioned persistence. The plan does not add a new investigation, faction or narrative authority.**.

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
