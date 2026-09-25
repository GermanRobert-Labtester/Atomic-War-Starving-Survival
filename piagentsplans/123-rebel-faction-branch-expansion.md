# Plan 123 — Rebel Faction Branches, PoNR Flags and Mutual-Exclusion Outcomes

> **Rebuild status:** COMPLETE 15-BRANCH CATALOG/STATE LOOP — FACTION-BRANCH REACHABILITY AND MIGRATION AUDIT
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

- The current `rebel_faction_branch.json` contains 15 branches with entry morality bands, point-of-no-return flags/triggers and two authored endings per branch.
- The live route is catalog → `RebelBranchCatalog` → `FactionBranchCoordinator`/`RebelBranchSystem` → `WeightOfChoicesSave` v2 → `FactionBranchHostSession`/Factions and Quests panels.
- The valuable expansion is proving that a rebel choice is reachable, mutually exclusive with Military/Independent commitments, idempotent at PoNR/ending resolution, and truthfully projected after a save restore. The plan preserves the current moral-band gate and faction-alignment semantics.

**Bounded outcome:** Retire the old 8→15 pure-data brief as an authoring project. The current Rebel catalog has 15 branches, the Core state/save codecs and the combined FactionBranchCoordinator/host session are live, and historical DEC-289 integrated the branch set. The remaining plan is a precise host/UI, exclusivity, migration and player-reachability audit—not a second rebel authority or more branches.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `rebel_faction_branch.json` is valid schema version 1 with 15 `branches`; current tests and the historical closeout identify 15 PoNR flags and 30 ending rows.
- `RebelBranchSystem` owns the Rebel timeline, committed branch, PoNR lock, faction alignment and ending resolution; it reads `MoralChoiceSystem.CurrentBand` rather than owning morality.
- `FactionBranchCoordinator` composes Military/Rebel/Independent/PRPF, enforces one active faction commitment and replays durable flags after restore.
- `FactionBranchHostSession` loads the current catalogs and saves through `WeightOfChoicesSaveStore`; current host/Main/Factions/Quests references establish a real route, while UI truthfulness still requires focused audit.

**Master-authority sections applied to this rebase:**

- Part II Factory Protocol: premise sweep, collision check, one lane/cluster, and evidence labels before drafting.
- Part II Step 5 continuity and anti-duplication checklist: data presence is not reachability.
- Part III cluster map: use the live C1–C17 owner map rather than a historical plan title.
- Part IV backlog discipline: consume a verified candidate or record why it is stale; do not widen a bounded outcome.
- Part V Template S/R: subject intent and recommended route remain separate from implementation commitments.
- Part VI Multi-Session Growth Protocol: 250k is a depth target, not permission to manufacture volume.
- Live source/data authority: current catalog, loader, host, save, and focused tests outrank generated prose.
- Anti-padding rule: if the evidence queue is exhausted, stop and report no warranted continuation.
- C7 Factions/war cluster: branch commitment, faction standing and moral choice are distinct authorities joined by the coordinator.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 8→15 target with a 15-row branch/flag/ending census and a current branch-coverage matrix.
- Trace the host command from the Factions/Quests surface through coordinator exclusivity, Rebel commit, PoNR lock and ending resolution.
- Audit `WeightOfChoicesSave` v1→v2 migration, durable flag replay and conflict rejection.
- Preserve no-new-state/no-new-save-section boundaries and label any panel claim as shared/integrator-owned.

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
| rebel branch definitions and ordered lookup | RebelBranchCatalog | `Assets/Ashfall.Core/Factions/RebelBranchCatalog.cs` | Immutable catalog after load; no mutable campaign state. |
| Rebel branch state, alignment, PoNR and ending | RebelBranchSystem | `Assets/Ashfall.Core/Factions/RebelBranchSystem.cs` | Sole Rebel branch owner; reads moral band and IFlagLedger ports. |
| cross-faction exclusivity and shared branch lifecycle | FactionBranchCoordinator | `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` | Owns the combined coordinator and active-kind invariant. |
| combined branch persistence and v1→v2 migration | WeightOfChoicesSaveCodec | `Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs; Assets/Ashfall.Core/Factions/RebelBranchSave.cs` | Existing envelope; all branch sections are intentionally present whether committed or not. |
| host commands, save flush and panel route | FactionBranchHostSession/Main.FactionBranch | `src/Host/FactionBranchHostSession.cs; src/Main.FactionBranch.cs; src/UI/FactionsPanel.cs` | Host/UI composition only; no second branch state. |
| catalog, system, exclusivity and integration proof | Rebel focused tests | `Ashfall.Core.Tests/RebelBranchExpansionTests.cs; Ashfall.Core.Tests/RebelBranchSystemTests.cs; Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs` | Focused executable evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Rebel Faction Branches, PoNR Flags and Mutual-Exclusion Outcomes
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ RebelBranchCatalog
│   rebel branch definitions and ordered lookup
│ RebelBranchSystem
│   Rebel branch state, alignment, PoNR and ending
│ FactionBranchCoordinator
│   cross-faction exclusivity and shared branch lifecycle
│ WeightOfChoicesSaveCodec
│   combined branch persistence and v1→v2 migration
│ FactionBranchHostSession/Main.FactionBranch
│   host commands, save flush and panel route
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

1. **Preserve current state ownership.** RebelBranchCatalog owns rebel branch definitions and ordered lookup: Immutable catalog after load; no mutable campaign state.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| rebel branch definitions and ordered lookup | RebelBranchCatalog | `Assets/Ashfall.Core/Factions/RebelBranchCatalog.cs` | Immutable catalog after load; no mutable campaign state. |
| Rebel branch state, alignment, PoNR and ending | RebelBranchSystem | `Assets/Ashfall.Core/Factions/RebelBranchSystem.cs` | Sole Rebel branch owner; reads moral band and IFlagLedger ports. |
| cross-faction exclusivity and shared branch lifecycle | FactionBranchCoordinator | `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` | Owns the combined coordinator and active-kind invariant. |
| combined branch persistence and v1→v2 migration | WeightOfChoicesSaveCodec | `Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs; Assets/Ashfall.Core/Factions/RebelBranchSave.cs` | Existing envelope; all branch sections are intentionally present whether committed or not. |
| host commands, save flush and panel route | FactionBranchHostSession/Main.FactionBranch | `src/Host/FactionBranchHostSession.cs; src/Main.FactionBranch.cs; src/UI/FactionsPanel.cs` | Host/UI composition only; no second branch state. |
| catalog, system, exclusivity and integration proof | Rebel focused tests | `Ashfall.Core.Tests/RebelBranchExpansionTests.cs; Ashfall.Core.Tests/RebelBranchSystemTests.cs; Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs` | Focused executable evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load the 15 Rebel rows and the other branch catalogs
2. restore WeightOfChoicesSave and replay durable flags
3. query coordinator availability using current moral band/standing
4. commit one mutually exclusive branch through the coordinator
5. lock the authored PoNR exactly once
6. apply bounded Rebel alignment changes through the current faction seam
7. resolve the first matching ending row and project the result
8. save the combined envelope and refresh Factions/Quests

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Catalog rows are immutable; `RebelBranchSystemState` owns timeline, branch record, alignment and durable flag set.
- A branch commitment is first-commit-wins; PoNR lock is irreversible and idempotent; ending resolution is first-resolution-wins after lock.
- The combined save always writes Military/Rebel/Independent/PRPF sections, with uncommitted sections at truthful defaults.
- A save with more than one committed faction is corrupt and must be rejected or quarantined by the existing codec/host contract.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A second cross-faction commitment is refused even if the other branch row is available.
- An out-of-band entry range returns a named reason and does not mutate the branch.
- Unknown branch/ending IDs fail closed; no middle-row fallback is used to conceal a malformed catalog during normal validation.
- Restored flags are visible to same-session gates immediately and cannot be double-counted by a replay.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `rebel_faction_branch.json` is the sole Rebel branch authority.
- No branch prose/flag copy belongs in a Factions panel, save file or second coordinator catalog.
- A future row needs a current producer, moral-band range, PoNR flag, ending ranges and a host command.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use `WeightOfChoicesSave` current v2 and frozen v1 migration; no new Rebel-only section.
- The individual `RebelBranchSaveCodec` is a structural component, not a second live store when the combined envelope is active.
- Unknown future save versions fail closed; missing v1 Independent state defaults fresh and uncommitted.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Branch selection is catalog-order/ID based and does not use random selection.
- The moral band parser and ending range scan are deterministic.
- Replay compares active faction kind, branch ID, PoNR day, alignment, ending and durable flags.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Coordinator forwards branch committed, PoNR locked, ending resolved and state changed facts.
- Durable flags are written to the runtime flag ledger and replayed after restore.
- Factions/Quests panels project availability and consequences; they do not set commitment state.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/FactionBranchHostSession.cs
- src/Host/WeightOfChoicesSaveStore.cs
- src/Main.FactionBranch.cs
- src/UI/FactionsPanel.cs
- src/UI/QuestsPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Rebel branches should express incompatible responsibilities and consequences without glamorizing real-world violence.
- A PoNR is irreversible in state, but the interface must explain the cost before the player commits.
- Endings must be selected by current moral/faction facts, not by a panel-authored label.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A player commits to both Rebel and Military through different UI paths. | RebelBranchCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A PoNR flag is set in a panel but not in durable state. | RebelBranchSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A restored v1 save loses the active Rebel branch or duplicates its ending. | FactionBranchCoordinator | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A branch ending range gap is silently mapped to a different branch. | WeightOfChoicesSaveCodec | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new rebel coordinator duplicates `FactionBranchCoordinator`. | FactionBranchHostSession/Main.FactionBranch | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/RebelBranchExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/RebelBranchSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — census/collision | Read 15 rows, catalogs, coordinator, host and combined save. | Current branch set and owners are proven. | No production path until the owning implementation package is separately claimed. |
| 1 — command trace | Trace availability, commit, exclusivity, PoNR, alignment and ending. | One live route is documented. | No production path until the owning implementation package is separately claimed. |
| 2 — migration/replay | Exercise v1/v2 restore, flags, conflict rejection and repeated resolution. | No state loss or double effect. | No production path until the owning implementation package is separately claimed. |
| 3 — UI/precision pass | Review Factions/Quests projection and lockout language. | Residual work is bounded and truthful. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/rebel_faction_branch.json | READ ONLY; MODIFY only for a proven branch/consumer gap | 15-row authority |
| Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs | READ ONLY | Combined owner |
| Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs | READ ONLY | Existing v2 envelope |
| src/UI/FactionsPanel.cs | READ ONLY; MODIFY only under a new UI claim | Current presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Creating a Rebel-only coordinator or save section. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Treating moral band as faction standing. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Changing exclusivity while fixing panel behavior. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Calling an old individual codec a second live persistence path. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new branches in this package.
- No new faction authority.
- No new save section.
- No production/data/test/UI changes.

# 23. Rollback and Recovery

- Revert the planning document.
- Future coordinator/save changes retain frozen v1/v2 fixtures and focused branch tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 15 branches, PoNR flags and ending ranges are current.
- Combined exclusivity, durable flags, migration and host route are explicit.
- No parallel rebel state/save owner is proposed.
- Focused tests and negative contracts are named.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 8→15 target with a 15-row branch/flag/ending census and a current branch-coverage matrix.
- Trace the host command from the Factions/Quests surface through coordinator exclusivity, Rebel commit, PoNR lock and ending resolution.
- Audit `WeightOfChoicesSave` v1→v2 migration, durable flag replay and conflict rejection.
- Preserve no-new-state/no-new-save-section boundaries and label any panel claim as shared/integrator-owned.

## MUST NOT DO

- No new branches in this package.
- No new faction authority.
- No new save section.
- No production/data/test/UI changes.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/RebelBranchExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/RebelBranchSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — census/collision — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: rebel branch definitions and ordered lookup → RebelBranchCatalog; Rebel branch state, alignment, PoNR and ending → RebelBranchSystem; cross-faction exclusivity and shared branch lifecycle → FactionBranchCoordinator; combined branch persistence and v1→v2 migration → WeightOfChoicesSaveCodec; host commands, save flush and panel route → FactionBranchHostSession/Main.FactionBranch; catalog, system, exclusivity and integration proof → Rebel focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 123.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 123 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by RebelBranchCatalog or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Factions/RebelBranchCatalog.cs`

### `Assets/Ashfall.Core/Factions/RebelBranchCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 83 lines / 3522 bytes.
- SHA-256: `3be27e0a1f977c20b995ee7eeded13dc466c21cdf683f4191e9ce3e2753d7f29`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RebelBranchEndingEntry
public string ending_id { get; set; } = string.Empty;
public string band_min { get; set; } = string.Empty;
public string band_max { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public sealed class RebelBranchEntry
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string ponr_flag { get; set; } = string.Empty;
public string ponr_trigger { get; set; } = string.Empty;
public string entry_band_min { get; set; } = string.Empty;
public string entry_band_max { get; set; } = string.Empty;
public List<RebelBranchEndingEntry> endings { get; set; } = new List<RebelBranchEndingEntry>();
public sealed class RebelBranchDataFile
public int schema_version { get; set; } = 1;
public string faction_id { get; set; } = string.Empty;
public List<RebelBranchEntry> branches { get; set; } = new List<RebelBranchEntry>();
public sealed class RebelBranchCatalog : IEnumerable<RebelBranchEntry>
public int Count => _order.Count;
public static RebelBranchCatalog Empty() => new RebelBranchCatalog();
public void Register(RebelBranchEntry entry) {
public RebelBranchEntry? GetById(string id) =>
public bool Contains(string id) => GetById(id) != null;
public IEnumerator<RebelBranchEntry> GetEnumerator() => _order.GetEnumerator();
public static RebelBranchCatalog LoadAndRegister(string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Factions/RebelBranchSystem.cs`

### `Assets/Ashfall.Core/Factions/RebelBranchSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 254 lines / 12146 bytes.
- SHA-256: `ffc81ec9b30340fcd1fcd1a8641a190567f1ccb8f4241726011fe3996e8291c4`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=9; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RebelBranchSystem
public const string SystemId = "rebel_branch_system";
public const int MinAlignment = -200;
public const int MaxAlignment = 200;
public event Action<string>? OnBranchCommitted;
public event Action<string, int>? OnPonrLocked;
public event Action<string>? OnEndingResolved;
public event Action<int>? OnAlignmentChanged;
public RebelBranchSystemState State => _state;
public int CurrentDay => _state.timeline.currentDay;
public string? CommittedBranchId => string.IsNullOrEmpty(_state.branch.branchId) ? null : _state.branch.branchId;
public bool IsPonrLocked => _state.branch.ponrLocked;
public int RebelAlignment => _state.rebelAlignment.alignment;
public string? ResolvedEndingId => string.IsNullOrEmpty(_state.branch.resolvedEndingId) ? null : _state.branch.resolvedEndingId;
public void AdvanceDay(int day) {
public string CommitBranch(string branchId, MoralChoiceSystem moralChoice) {
public void LockPointOfNoReturn() {
public void ShiftFactionAlignment(int delta) {
public string ResolveEnding(MoralChoiceSystem moralChoice) {
public static bool IsGameOver(int livingSurvivorCount) => livingSurvivorCount <= 0;
public RebelBranchSystemState CaptureState() => Clone(_state);
public void RestoreState(RebelBranchSystemState state) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Factions/RebelBranchState.cs`

### `Assets/Ashfall.Core/Factions/RebelBranchState.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 62 lines / 2402 bytes.
- SHA-256: `9d936759dd90803d61fa300cd1830b17b2582aeee13b3604dfa769a68d848b8e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class RebelBranchTimelineState
public int currentDay = 0;
public class RebelBranchRecord
public string branchId = string.Empty;
public bool committed = false;
public bool ponrLocked = false;
public int ponrLockedDay = -1;
public string resolvedEndingId = string.Empty;
public class RebelBranchSystemState
public string systemId = RebelBranchSystem.SystemId;
public int schemaVersion = 1;
public RebelBranchTimelineState timeline = new RebelBranchTimelineState();
public RebelBranchRecord branch = new RebelBranchRecord();
public FactionAlignmentRecord rebelAlignment = new FactionAlignmentRecord {
public List<string> setFlags = new List<string>();
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`

### `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 667 lines / 27660 bytes.
- SHA-256: `e5d365c6263dfb636692ae8dc1994210b13b97fd3bdf4c84f849d05a96dcde4f`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=36; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum FactionBranchKind
public sealed class FactionBranchOption
public string BranchId { get; set; } = string.Empty;
public FactionBranchKind FactionKind { get; set; }
public string FactionId { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string PonrFlag { get; set; } = string.Empty;
public string PonrTrigger { get; set; } = string.Empty;
public string EntryBandMin { get; set; } = string.Empty;
public string EntryBandMax { get; set; } = string.Empty;
public bool IsCommitted { get; set; }
public bool IsPonrLocked { get; set; }
public bool IsAvailable { get; set; }
public string? LockoutReason { get; set; }
public List<string> PossibleEndings { get; set; } = new List<string>();
public string ConsequencesSummary { get; set; } = string.Empty;
public sealed class FactionStandingSummary
public string FactionId { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public int Standing { get; set; }
public int Alignment { get; set; }
public bool IsHostile { get; set; }
public bool IsAllied { get; set; }
public bool IsJoined { get; set; }
public bool IsOpposed { get; set; }
public sealed class FactionBranchCoordinator
public const string SystemId = "faction_branch_coordinator";
public MilitaryBranchSystem Military { get; }
public RebelBranchSystem Rebel { get; }
public IndependentBranchSystem Independent { get; }
public PrpfStandingSystem Prpf { get; }
public event Action<FactionBranchKind, string>? OnBranchCommitted;
public event Action<string, int>? OnPonrLocked;
public event Action<string>? OnEndingResolved;
public event Action? OnStateChanged;
public bool IsCommitted => ActiveFactionKind != FactionBranchKind.None;
public int CurrentDay =>
public void AdvanceDay(int day) {
public bool CanCommit(string branchId, MoralChoiceSystem moralChoice, out string? reason) {
public ActionResult CommitBranch(string branchId, MoralChoiceSystem moralChoice) {
public ActionResult LockPonr(int day) {
public ActionResult ResolveEnding(MoralChoiceSystem moralChoice) {
public void ModifyStanding(string factionId, int delta) {
public void ShiftFactionAlignment(string factionId, int delta) {
public bool TryJoinPrpf(MoralChoiceSystem moralChoice) {
public void OpposePrpf() {
public void TickDay(int day) {
public FactionBranchKind DetectBranchKind(string branchId) {
public IReadOnlyList<FactionBranchOption> GetBranchOptions(MoralChoiceSystem? moralChoice) {
public IReadOnlyList<FactionStandingSummary> GetFactionStandingSummaries() {
public WeightOfChoicesSave CaptureState() {
public void RestoreState(WeightOfChoicesSave save) {
public static FactionBranchCoordinator LoadFromData( string dataDir, IFileIO fileIO, IJsonSerializer json, IFlagLedger flags, ILog? log = null)
```


# Appendix B.06 — Current Code Architecture: `Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs`

### `Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 188 lines / 9036 bytes.
- SHA-256: `c6a10fc9ec25bcc1b02b9ec098a6cd614d6a4cf60a6fc50fd53809fa574058a4`.
- Architecture signals: seeded references=0; save/restore symbols=8; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class WeightOfChoicesSave
public const int CurrentSaveVersion = 2;
public int saveVersion = CurrentSaveVersion;
public MilitaryBranchSystemState militaryBranch = new MilitaryBranchSystemState();
public RebelBranchSystemState rebelBranch = new RebelBranchSystemState();
public IndependentBranchSystemState independentBranch = new IndependentBranchSystemState();
public PrpfSystemState prpf = new PrpfSystemState();
public string Checksum = string.Empty;
public class WeightOfChoicesSaveV1
public int saveVersion = 1;
public MilitaryBranchSystemState militaryBranch = new MilitaryBranchSystemState();
public RebelBranchSystemState rebelBranch = new RebelBranchSystemState();
public PrpfSystemState prpf = new PrpfSystemState();
public string Checksum = string.Empty;
public static class WeightOfChoicesSaveCodec
public static WeightOfChoicesSave Capture( MilitaryBranchSystem militaryBranch, RebelBranchSystem rebelBranch, IndependentBranchSystem independentBranch, PrpfStandingSystem prpf) {
public static void Restore( WeightOfChoicesSave save, MilitaryBranchSystem militaryBranch, RebelBranchSystem rebelBranch, IndependentBranchSystem independentBranch, PrpfStandingSystem prpf)
public static string Encode(WeightOfChoicesSave save, IJsonSerializer json) {
public static WeightOfChoicesSave Decode(string jsonText, IJsonSerializer json) {
public static bool HasConflictingFactionCommitment(WeightOfChoicesSave save) {
```


# Appendix B.07 — Current Code Architecture: `src/Host/FactionBranchHostSession.cs`

### `src/Host/FactionBranchHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 55 lines / 1731 bytes.
- SHA-256: `9a8e5b20ec4116cb7037ca4d02dbf3fa07eb2877472ca4ba60a2e4af05f54109`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FactionBranchHostSession : HostSessionBase
public FactionBranchCoordinator Coordinator { get; }
public static FactionBranchHostSession CreateDefault(string dataDir, IFlagLedger? flags = null) {
public bool TrySave() {
public bool TryLoad() {
public override void Save() {
```


# Appendix B.08 — Current Code Architecture: `src/Host/WeightOfChoicesSaveStore.cs`

### `src/Host/WeightOfChoicesSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 53 lines / 2589 bytes.
- SHA-256: `6cad240dee44677d33e929c004068fbe24e426a9b088ab5554f9183f29b5e108`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class WeightOfChoicesSaveStore
public const string FileName = "weight_of_choices_save.json";
public const string SectionName = "weight_of_choices";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(WeightOfChoicesSave state) => s_store.CaptureBare(state);
public static WeightOfChoicesSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(WeightOfChoicesSave state) => s_store.CaptureBare(state);
public static WeightOfChoicesSave? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(WeightOfChoicesSave state) => s_store.TrySave(state);
public static WeightOfChoicesSave? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(WeightOfChoicesSave state) => s_store.CapturePersisted(state);
```


# Appendix B.09 — Current Code Architecture: `src/Main.FactionBranch.cs`

### `src/Main.FactionBranch.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 89 lines / 3167 bytes.
- SHA-256: `e575193d6eeab9f4cc0e594625b8fc59c9d731c06f5e50a7a92c75147cdc0da1`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public bool CommitFactionBranch(string branchId) {
```


# Appendix B.10 — Current Code Architecture: `src/UI/FactionsPanel.cs`

### `src/UI/FactionsPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 536 lines / 30554 bytes.
- SHA-256: `a129850bff990e300e3f80308d74c8ecbd029463eda3417438045b1a6892986d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=17; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class FactionsPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<string>? OnFactionDetailRequested;
public event Action? OnMusterPanelRequested;
public event Action? OnFoundryPanelRequested;
public event Action? OnCultureCodexRequested;
public event Action<int>? OnWarlordTributePay;
public event Action? OnWarlordTributeRefuse;
public event Action<string>? OnCommitBranchRequested;
public bool IsBound => _factions != null || _muster != null || _expansions != null || _branchCoordinator != null;
public bool HasGuildCard { get; private set; }
public void Bind( HoldfastFactionsCatalog? factions, HoldfastTradeSession? trade = null, MusterHostSession? muster = null, ExpansionHostSession? expansions = null, YearOfAshHostSession? yearOfAsh = null,
public void RefreshView() {
public override void _Ready() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix B.11 — Current Code Architecture: `src/UI/QuestsPanel.cs`

### `src/UI/QuestsPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 774 lines / 37776 bytes.
- SHA-256: `c1a69c26ccc3ef00cfd1078aee697c71d5550eb0b0ae1c461e0ec060f9bbf11b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=16; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class QuestsPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<string>? OnQuestDetailRequested;
public event Action? OnCrossingPanelRequested;
public event Action? OnProceduralQuestRequested;
public event Action<string>? OnBeginSurvivorArcRequested;
public event Action<string, string>? OnDeliverArcObjectiveRequested;
public event Action<string, string>? OnChooseArcBranchRequested;
public bool IsBound => _holdfastQuests != null || _crossingQuests != null || _branchCoordinator != null || _moralDefs != null || _survivorArcs != null || _proceduralNarrative != null;
public void Bind( HoldfastQuestSystem? holdfastQuests, CrossingQuestSystem? crossingQuests = null, DutyRosterHostSession? dutyRoster = null, int currentDay = 1, Ashfall.Core.Factions.FactionBranchCoordinator? branchCoordinator = null,
public void Unbind() {
public void RefreshView() {
public override void _Ready() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
```


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/rebel_faction_branch.json`

### `Assets/StreamingAssets/Data/rebel_faction_branch.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 12323 bytes / 12323 characters.
- SHA-256: `eeeefcb394ec31bfaf018e664859bd08dcfabd0e5ae0d4b263391b823c6b3495`.
- Root keys: `branches`, `faction_id`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
branches: min=15, max=15, observed_paths=1
branches[].endings: min=3, max=3, observed_paths=2
```

Representative record fields:

- `display_name`
- `endings`
- `entry_band_max`
- `entry_band_min`
- `id`
- `ponr_flag`
- `ponr_trigger`

Representative identifiers (ordered, capped for readability):

```text
branch_rebel_1_true_rebel
branch_rebel_2_defector
branch_rebel_3_opportunist
branch_rebel_4_martyr
branch_rebel_5_warlord
branch_rebel_6_reformer
branch_rebel_7_lone_wolf
branch_rebel_8_revolution
branch_rebel_9_bombmaker
branch_rebel_10_courier
branch_rebel_11_propagandist
branch_rebel_12_dissident
branch_rebel_13_protector
branch_rebel_14_saboteur
branch_rebel_15_negotiator
```


# Appendix C.13 — Catalog Census: `Assets/StreamingAssets/Data/independent_faction_branch.json`

### `Assets/StreamingAssets/Data/independent_faction_branch.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 12209 bytes / 12207 characters.
- SHA-256: `678a4bd46d2091ff54c62bd3617c4c1507a2582e520d0be46c8261e78cf5036e`.
- Root keys: `branches`, `faction_id`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
branches: min=15, max=15, observed_paths=1
branches[].endings: min=3, max=3, observed_paths=2
```

Representative record fields:

- `display_name`
- `endings`
- `entry_band_max`
- `entry_band_min`
- `id`
- `ponr_flag`
- `ponr_trigger`
- `requires_hostile_to_military`
- `requires_hostile_to_rebel`
- `requires_prpf_standing_min`

Representative identifiers (ordered, capped for readability):

```text
branch_ind_1_survivor
branch_ind_2_mercenary
branch_ind_3_peacekeeper_diplomat
branch_ind_4_exile
branch_ind_5_kingmaker
branch_ind_6_legend
branch_ind_7_ghost
branch_ind_8_wasteland_myth
branch_ind_9_hermit
branch_ind_10_mediator
branch_ind_11_scavenger_king
branch_ind_12_caretaker
branch_ind_13_witness
branch_ind_14_engineer
branch_ind_15_prophet
```


# Appendix C.14 — Catalog Census: `Assets/StreamingAssets/Data/moral_choice_flags.json`

### `Assets/StreamingAssets/Data/moral_choice_flags.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 2090 bytes / 2090 characters.
- SHA-256: `e5a95235ce28f9d2a1c9c8bcaeb72789122d9d77e1d38b0658cc86ae4d6a4db4`.
- Root keys: `description`, `flags`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
flags: min=25, max=25, observed_paths=1
```

Representative record fields:

- `display_name`
- `id`

Representative identifiers (ordered, capped for readability):

```text
flag_betrayed_ally
flag_betrayed_faction
flag_betrayed_trust
flag_broken_pact
flag_become_warlord
flag_throne_of_ash
flag_branch_mercy_road_locked
flag_branch_iron_way_locked
flag_branch_listener_locked
flag_branch_broken_compact_locked
flag_spared_raider
flag_executed_prisoner
flag_shared_rations
flag_hoarded_medicine
flag_sheltered_refugee
flag_expelled_survivor
flag_repaired_infrastructure
flag_sabotaged_rival
flag_broke_treaty
flag_honored_debt
flag_ignored_distress
flag_responded_distress
flag_forged_record
flag_preserved_archive
flag_chosen_faction_side
```


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/RebelBranchExpansionTests.cs`

### `Ashfall.Core.Tests/RebelBranchExpansionTests.cs`

- Current test declarations: Fact=12, Theory=0, InlineData=0.
- File lines: 347; SHA-256: `c9d8335f251cd2efe56f35912f2f2a2807aefe5c233e6c1eccb5dcaa31d9f0b6`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_ContainsExactlyFifteenBranches
OriginalEightBranches_PreserveOrderAndShape
SevenNewBranches_AreAppendedWithCanonicalIds
BranchesFlagsAndEndings_AreUniqueAndUseCanonicalPrefixes
AllBandsAndEndingRanges_AreValid
SevenNewBranches_HaveExactlyOneEndingForEveryMoralBand
SevenNewBranches_CanCommitAtNeutralAndLockTheirOwnPonrFlags
Coordinator_ExposesAndResolvesANewRebelBranch
SevenNewBranches_ResolveAnAuthoredEndingAtEveryMoralBand
NewBranch_SaveRoundTripPreservesPonrAndResolvedEnding
LegacyStateWithoutNewFieldsStillRestores
HighRiskArcsRemainNonOperationalInTheirNarrativeText
```


# Appendix D.16 — Existing Focused Test Inventory: `Ashfall.Core.Tests/RebelBranchSystemTests.cs`

### `Ashfall.Core.Tests/RebelBranchSystemTests.cs`

- Current test declarations: Fact=16, Theory=0, InlineData=0.
- File lines: 274; SHA-256: `350f20fdea0204230dfd9b998b66bdf520e58197b04c88da4b0fe7b8710c4910`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsAllFifteenBranchesWithThreeEndingsEach
CommitBranch_WithinEntryBand_Succeeds
CommitBranch_OutsideEntryBand_Throws
CommitBranch_CalledTwice_KeepsFirstCommitment
LockPointOfNoReturn_BeforeCommit_Throws
LockPointOfNoReturn_SetsDurableAndRuntimeFlag
LockPointOfNoReturn_CalledTwice_IsANoOp
ResolveEnding_BeforePonrLocked_Throws
ResolveEnding_NeutralBand_ResolvesSurvivorEnding
ResolveEnding_IsIdempotent_EvenIfMoralityDriftsAfterward
ShiftFactionAlignment_ClampsToRange
IsGameOver_ZeroOrNegativeSurvivors_IsTrue
SaveRoundTrip_PreservesBranchTimelineAlignmentAndFlags
Decode_TamperedChecksum_Throws
Decode_NewerSaveVersion_Throws
RestoreState_WrongSystemId_Throws
```


# Appendix D.17 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs`

### `Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 208; SHA-256: `678c8099a9851b93478ddb7d760c23c9bf07de0506ef7e67f8fc0857610c4e09`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RebelBranchCatalog_LoadsAll15BranchesFromAuthJson
ThreeWayFactionBranchIds_AreCompletelyDisjoint
ThreeWayPonrFlags_AreCompletelyDisjoint
RebelSystem_And_VerdictCorpusLadder_ExecuteConcurrentlyWithoutInterference
Coordinator_CoordinatesRebelBranchesWithExclusivity
```


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/Factions/RebelBranchCatalog.cs`

### `Assets/Ashfall.Core/Factions/RebelBranchCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 83 lines / 3522 bytes.
- SHA-256: `3be27e0a1f977c20b995ee7eeded13dc466c21cdf683f4191e9ce3e2753d7f29`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RebelBranchEndingEntry
public string ending_id { get; set; } = string.Empty;
public string band_min { get; set; } = string.Empty;
public string band_max { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public sealed class RebelBranchEntry
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string ponr_flag { get; set; } = string.Empty;
public string ponr_trigger { get; set; } = string.Empty;
public string entry_band_min { get; set; } = string.Empty;
public string entry_band_max { get; set; } = string.Empty;
public List<RebelBranchEndingEntry> endings { get; set; } = new List<RebelBranchEndingEntry>();
public sealed class RebelBranchDataFile
public int schema_version { get; set; } = 1;
public string faction_id { get; set; } = string.Empty;
public List<RebelBranchEntry> branches { get; set; } = new List<RebelBranchEntry>();
public sealed class RebelBranchCatalog : IEnumerable<RebelBranchEntry>
public int Count => _order.Count;
public static RebelBranchCatalog Empty() => new RebelBranchCatalog();
public void Register(RebelBranchEntry entry) {
public RebelBranchEntry? GetById(string id) =>
public bool Contains(string id) => GetById(id) != null;
public IEnumerator<RebelBranchEntry> GetEnumerator() => _order.GetEnumerator();
public static RebelBranchCatalog LoadAndRegister(string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix E.19 — Supporting Code Evidence: `Assets/Ashfall.Core/Factions/RebelBranchSystem.cs`

### `Assets/Ashfall.Core/Factions/RebelBranchSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 254 lines / 12146 bytes.
- SHA-256: `ffc81ec9b30340fcd1fcd1a8641a190567f1ccb8f4241726011fe3996e8291c4`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=9; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RebelBranchSystem
public const string SystemId = "rebel_branch_system";
public const int MinAlignment = -200;
public const int MaxAlignment = 200;
public event Action<string>? OnBranchCommitted;
public event Action<string, int>? OnPonrLocked;
public event Action<string>? OnEndingResolved;
public event Action<int>? OnAlignmentChanged;
public RebelBranchSystemState State => _state;
public int CurrentDay => _state.timeline.currentDay;
public string? CommittedBranchId => string.IsNullOrEmpty(_state.branch.branchId) ? null : _state.branch.branchId;
public bool IsPonrLocked => _state.branch.ponrLocked;
public int RebelAlignment => _state.rebelAlignment.alignment;
public string? ResolvedEndingId => string.IsNullOrEmpty(_state.branch.resolvedEndingId) ? null : _state.branch.resolvedEndingId;
public void AdvanceDay(int day) {
public string CommitBranch(string branchId, MoralChoiceSystem moralChoice) {
public void LockPointOfNoReturn() {
public void ShiftFactionAlignment(int delta) {
public string ResolveEnding(MoralChoiceSystem moralChoice) {
public static bool IsGameOver(int livingSurvivorCount) => livingSurvivorCount <= 0;
public RebelBranchSystemState CaptureState() => Clone(_state);
public void RestoreState(RebelBranchSystemState state) {
```


# Appendix E.20 — Supporting Code Evidence: `Assets/Ashfall.Core/Factions/RebelBranchState.cs`

### `Assets/Ashfall.Core/Factions/RebelBranchState.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 62 lines / 2402 bytes.
- SHA-256: `9d936759dd90803d61fa300cd1830b17b2582aeee13b3604dfa769a68d848b8e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class RebelBranchTimelineState
public int currentDay = 0;
public class RebelBranchRecord
public string branchId = string.Empty;
public bool committed = false;
public bool ponrLocked = false;
public int ponrLockedDay = -1;
public string resolvedEndingId = string.Empty;
public class RebelBranchSystemState
public string systemId = RebelBranchSystem.SystemId;
public int schemaVersion = 1;
public RebelBranchTimelineState timeline = new RebelBranchTimelineState();
public RebelBranchRecord branch = new RebelBranchRecord();
public FactionAlignmentRecord rebelAlignment = new FactionAlignmentRecord {
public List<string> setFlags = new List<string>();
```


# Appendix E.21 — Supporting Code Evidence: `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`

### `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 667 lines / 27660 bytes.
- SHA-256: `e5d365c6263dfb636692ae8dc1994210b13b97fd3bdf4c84f849d05a96dcde4f`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=36; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum FactionBranchKind
public sealed class FactionBranchOption
public string BranchId { get; set; } = string.Empty;
public FactionBranchKind FactionKind { get; set; }
public string FactionId { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string PonrFlag { get; set; } = string.Empty;
public string PonrTrigger { get; set; } = string.Empty;
public string EntryBandMin { get; set; } = string.Empty;
public string EntryBandMax { get; set; } = string.Empty;
public bool IsCommitted { get; set; }
public bool IsPonrLocked { get; set; }
public bool IsAvailable { get; set; }
public string? LockoutReason { get; set; }
public List<string> PossibleEndings { get; set; } = new List<string>();
public string ConsequencesSummary { get; set; } = string.Empty;
public sealed class FactionStandingSummary
public string FactionId { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public int Standing { get; set; }
public int Alignment { get; set; }
public bool IsHostile { get; set; }
public bool IsAllied { get; set; }
public bool IsJoined { get; set; }
public bool IsOpposed { get; set; }
public sealed class FactionBranchCoordinator
public const string SystemId = "faction_branch_coordinator";
public MilitaryBranchSystem Military { get; }
public RebelBranchSystem Rebel { get; }
public IndependentBranchSystem Independent { get; }
public PrpfStandingSystem Prpf { get; }
public event Action<FactionBranchKind, string>? OnBranchCommitted;
public event Action<string, int>? OnPonrLocked;
public event Action<string>? OnEndingResolved;
public event Action? OnStateChanged;
public bool IsCommitted => ActiveFactionKind != FactionBranchKind.None;
public int CurrentDay =>
public void AdvanceDay(int day) {
public bool CanCommit(string branchId, MoralChoiceSystem moralChoice, out string? reason) {
public ActionResult CommitBranch(string branchId, MoralChoiceSystem moralChoice) {
public ActionResult LockPonr(int day) {
public ActionResult ResolveEnding(MoralChoiceSystem moralChoice) {
public void ModifyStanding(string factionId, int delta) {
public void ShiftFactionAlignment(string factionId, int delta) {
public bool TryJoinPrpf(MoralChoiceSystem moralChoice) {
public void OpposePrpf() {
public void TickDay(int day) {
public FactionBranchKind DetectBranchKind(string branchId) {
public IReadOnlyList<FactionBranchOption> GetBranchOptions(MoralChoiceSystem? moralChoice) {
public IReadOnlyList<FactionStandingSummary> GetFactionStandingSummaries() {
public WeightOfChoicesSave CaptureState() {
public void RestoreState(WeightOfChoicesSave save) {
public static FactionBranchCoordinator LoadFromData( string dataDir, IFileIO fileIO, IJsonSerializer json, IFlagLedger flags, ILog? log = null)
```


# Appendix E.22 — Supporting Code Evidence: `Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs`

### `Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 188 lines / 9036 bytes.
- SHA-256: `c6a10fc9ec25bcc1b02b9ec098a6cd614d6a4cf60a6fc50fd53809fa574058a4`.
- Architecture signals: seeded references=0; save/restore symbols=8; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class WeightOfChoicesSave
public const int CurrentSaveVersion = 2;
public int saveVersion = CurrentSaveVersion;
public MilitaryBranchSystemState militaryBranch = new MilitaryBranchSystemState();
public RebelBranchSystemState rebelBranch = new RebelBranchSystemState();
public IndependentBranchSystemState independentBranch = new IndependentBranchSystemState();
public PrpfSystemState prpf = new PrpfSystemState();
public string Checksum = string.Empty;
public class WeightOfChoicesSaveV1
public int saveVersion = 1;
public MilitaryBranchSystemState militaryBranch = new MilitaryBranchSystemState();
public RebelBranchSystemState rebelBranch = new RebelBranchSystemState();
public PrpfSystemState prpf = new PrpfSystemState();
public string Checksum = string.Empty;
public static class WeightOfChoicesSaveCodec
public static WeightOfChoicesSave Capture( MilitaryBranchSystem militaryBranch, RebelBranchSystem rebelBranch, IndependentBranchSystem independentBranch, PrpfStandingSystem prpf) {
public static void Restore( WeightOfChoicesSave save, MilitaryBranchSystem militaryBranch, RebelBranchSystem rebelBranch, IndependentBranchSystem independentBranch, PrpfStandingSystem prpf)
public static string Encode(WeightOfChoicesSave save, IJsonSerializer json) {
public static WeightOfChoicesSave Decode(string jsonText, IJsonSerializer json) {
public static bool HasConflictingFactionCommitment(WeightOfChoicesSave save) {
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/RebelBranchExpansionTests.cs`

### `Ashfall.Core.Tests/RebelBranchExpansionTests.cs`

- Current test declarations: Fact=12, Theory=0, InlineData=0.
- File lines: 347; SHA-256: `c9d8335f251cd2efe56f35912f2f2a2807aefe5c233e6c1eccb5dcaa31d9f0b6`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_ContainsExactlyFifteenBranches
OriginalEightBranches_PreserveOrderAndShape
SevenNewBranches_AreAppendedWithCanonicalIds
BranchesFlagsAndEndings_AreUniqueAndUseCanonicalPrefixes
AllBandsAndEndingRanges_AreValid
SevenNewBranches_HaveExactlyOneEndingForEveryMoralBand
SevenNewBranches_CanCommitAtNeutralAndLockTheirOwnPonrFlags
Coordinator_ExposesAndResolvesANewRebelBranch
SevenNewBranches_ResolveAnAuthoredEndingAtEveryMoralBand
NewBranch_SaveRoundTripPreservesPonrAndResolvedEnding
LegacyStateWithoutNewFieldsStillRestores
HighRiskArcsRemainNonOperationalInTheirNarrativeText
```


# Appendix G.24 — Supporting Regression Evidence: `Ashfall.Core.Tests/RebelBranchSystemTests.cs`

### `Ashfall.Core.Tests/RebelBranchSystemTests.cs`

- Current test declarations: Fact=16, Theory=0, InlineData=0.
- File lines: 274; SHA-256: `350f20fdea0204230dfd9b998b66bdf520e58197b04c88da4b0fe7b8710c4910`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsAllFifteenBranchesWithThreeEndingsEach
CommitBranch_WithinEntryBand_Succeeds
CommitBranch_OutsideEntryBand_Throws
CommitBranch_CalledTwice_KeepsFirstCommitment
LockPointOfNoReturn_BeforeCommit_Throws
LockPointOfNoReturn_SetsDurableAndRuntimeFlag
LockPointOfNoReturn_CalledTwice_IsANoOp
ResolveEnding_BeforePonrLocked_Throws
ResolveEnding_NeutralBand_ResolvesSurvivorEnding
ResolveEnding_IsIdempotent_EvenIfMoralityDriftsAfterward
ShiftFactionAlignment_ClampsToRange
IsGameOver_ZeroOrNegativeSurvivors_IsTrue
SaveRoundTrip_PreservesBranchTimelineAlignmentAndFlags
Decode_TamperedChecksum_Throws
Decode_NewerSaveVersion_Throws
RestoreState_WrongSystemId_Throws
```


# Appendix G.25 — Supporting Regression Evidence: `Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs`

### `Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 208; SHA-256: `678c8099a9851b93478ddb7d760c23c9bf07de0506ef7e67f8fc0857610c4e09`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RebelBranchCatalog_LoadsAll15BranchesFromAuthJson
ThreeWayFactionBranchIds_AreCompletelyDisjoint
ThreeWayPonrFlags_AreCompletelyDisjoint
RebelSystem_And_VerdictCorpusLadder_ExecuteConcurrentlyWithoutInterference
Coordinator_CoordinatesRebelBranchesWithExclusivity
```


# Appendix H.26 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

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
| rebel branch definitions and ordered lookup | RebelBranchCatalog | Rebel branch state, alignment, PoNR and ending | RebelBranchSystem | Owner emits/reads a typed fact; no mirror state. |
| rebel branch definitions and ordered lookup | RebelBranchCatalog | cross-faction exclusivity and shared branch lifecycle | FactionBranchCoordinator | Owner emits/reads a typed fact; no mirror state. |
| rebel branch definitions and ordered lookup | RebelBranchCatalog | combined branch persistence and v1→v2 migration | WeightOfChoicesSaveCodec | Owner emits/reads a typed fact; no mirror state. |
| rebel branch definitions and ordered lookup | RebelBranchCatalog | host commands, save flush and panel route | FactionBranchHostSession/Main.FactionBranch | Owner emits/reads a typed fact; no mirror state. |
| rebel branch definitions and ordered lookup | RebelBranchCatalog | catalog, system, exclusivity and integration proof | Rebel focused tests | Owner emits/reads a typed fact; no mirror state. |
| Rebel branch state, alignment, PoNR and ending | RebelBranchSystem | rebel branch definitions and ordered lookup | RebelBranchCatalog | Owner emits/reads a typed fact; no mirror state. |
| Rebel branch state, alignment, PoNR and ending | RebelBranchSystem | cross-faction exclusivity and shared branch lifecycle | FactionBranchCoordinator | Owner emits/reads a typed fact; no mirror state. |
| Rebel branch state, alignment, PoNR and ending | RebelBranchSystem | combined branch persistence and v1→v2 migration | WeightOfChoicesSaveCodec | Owner emits/reads a typed fact; no mirror state. |
| Rebel branch state, alignment, PoNR and ending | RebelBranchSystem | host commands, save flush and panel route | FactionBranchHostSession/Main.FactionBranch | Owner emits/reads a typed fact; no mirror state. |
| Rebel branch state, alignment, PoNR and ending | RebelBranchSystem | catalog, system, exclusivity and integration proof | Rebel focused tests | Owner emits/reads a typed fact; no mirror state. |
| cross-faction exclusivity and shared branch lifecycle | FactionBranchCoordinator | rebel branch definitions and ordered lookup | RebelBranchCatalog | Owner emits/reads a typed fact; no mirror state. |
| cross-faction exclusivity and shared branch lifecycle | FactionBranchCoordinator | Rebel branch state, alignment, PoNR and ending | RebelBranchSystem | Owner emits/reads a typed fact; no mirror state. |
| cross-faction exclusivity and shared branch lifecycle | FactionBranchCoordinator | combined branch persistence and v1→v2 migration | WeightOfChoicesSaveCodec | Owner emits/reads a typed fact; no mirror state. |
| cross-faction exclusivity and shared branch lifecycle | FactionBranchCoordinator | host commands, save flush and panel route | FactionBranchHostSession/Main.FactionBranch | Owner emits/reads a typed fact; no mirror state. |
| cross-faction exclusivity and shared branch lifecycle | FactionBranchCoordinator | catalog, system, exclusivity and integration proof | Rebel focused tests | Owner emits/reads a typed fact; no mirror state. |
| combined branch persistence and v1→v2 migration | WeightOfChoicesSaveCodec | rebel branch definitions and ordered lookup | RebelBranchCatalog | Owner emits/reads a typed fact; no mirror state. |
| combined branch persistence and v1→v2 migration | WeightOfChoicesSaveCodec | Rebel branch state, alignment, PoNR and ending | RebelBranchSystem | Owner emits/reads a typed fact; no mirror state. |
| combined branch persistence and v1→v2 migration | WeightOfChoicesSaveCodec | cross-faction exclusivity and shared branch lifecycle | FactionBranchCoordinator | Owner emits/reads a typed fact; no mirror state. |
| combined branch persistence and v1→v2 migration | WeightOfChoicesSaveCodec | host commands, save flush and panel route | FactionBranchHostSession/Main.FactionBranch | Owner emits/reads a typed fact; no mirror state. |
| combined branch persistence and v1→v2 migration | WeightOfChoicesSaveCodec | catalog, system, exclusivity and integration proof | Rebel focused tests | Owner emits/reads a typed fact; no mirror state. |
| host commands, save flush and panel route | FactionBranchHostSession/Main.FactionBranch | rebel branch definitions and ordered lookup | RebelBranchCatalog | Owner emits/reads a typed fact; no mirror state. |
| host commands, save flush and panel route | FactionBranchHostSession/Main.FactionBranch | Rebel branch state, alignment, PoNR and ending | RebelBranchSystem | Owner emits/reads a typed fact; no mirror state. |
| host commands, save flush and panel route | FactionBranchHostSession/Main.FactionBranch | cross-faction exclusivity and shared branch lifecycle | FactionBranchCoordinator | Owner emits/reads a typed fact; no mirror state. |
| host commands, save flush and panel route | FactionBranchHostSession/Main.FactionBranch | combined branch persistence and v1→v2 migration | WeightOfChoicesSaveCodec | Owner emits/reads a typed fact; no mirror state. |
| host commands, save flush and panel route | FactionBranchHostSession/Main.FactionBranch | catalog, system, exclusivity and integration proof | Rebel focused tests | Owner emits/reads a typed fact; no mirror state. |
| catalog, system, exclusivity and integration proof | Rebel focused tests | rebel branch definitions and ordered lookup | RebelBranchCatalog | Owner emits/reads a typed fact; no mirror state. |
| catalog, system, exclusivity and integration proof | Rebel focused tests | Rebel branch state, alignment, PoNR and ending | RebelBranchSystem | Owner emits/reads a typed fact; no mirror state. |
| catalog, system, exclusivity and integration proof | Rebel focused tests | cross-faction exclusivity and shared branch lifecycle | FactionBranchCoordinator | Owner emits/reads a typed fact; no mirror state. |
| catalog, system, exclusivity and integration proof | Rebel focused tests | combined branch persistence and v1→v2 migration | WeightOfChoicesSaveCodec | Owner emits/reads a typed fact; no mirror state. |
| catalog, system, exclusivity and integration proof | Rebel focused tests | host commands, save flush and panel route | FactionBranchHostSession/Main.FactionBranch | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 8→15 target with a 15-row branch/flag/ending census and a current branch-coverage matrix. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Trace the host command from the Factions/Quests surface through coordinator exclusivity, Rebel commit, PoNR lock and ending resolution. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Audit `WeightOfChoicesSave` v1→v2 migration, durable flag replay and conflict rejection. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Preserve no-new-state/no-new-save-section boundaries and label any panel claim as shared/integrator-owned. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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

> **DR-04 — The data catalog inventory has grown; several catalogs are absent from the v1.0 inventory. VERIFIED.**
`Assets/StreamingAssets/Data/` currently holds 342 entries. Catalogs observed live but not present in the v1.0 Part 5.4 inventory include: `dive_sites.json`, `hydroponic_crops.json`, `hydraulic_extrusion_catalog.json`, `metrology_standards_catalog.json`, `muster_camp_scenes.json`, `muster_epilogues.json`, `muster_faction_actions.json`, `muster_faction_culture.json`, `muster_witnesses.json`, `utility_actions.json`, `moral_choice_quests_branching.json`, `moral_choice_quests_distress.json`. Consequence: the duplication firewall (v1.0 Part 5) is stale in these domains; a planner could propose a "new" muster or moral-choice catalog that already exists. The ID-collision sweep in Factory Protocol step 1 must always run against the live listing, never against this document.

> **DR-06 — Integration ledger state differs from the v1.0 queue snapshot. VERIFIED.**
Live `INTEGRATION_PLANS.md` (read 2026-09-24) shows, at minimum: the **XP Expansion W1** batch ACTIVE (difficulty authority package `XP-WAVE1-DIFFICULTY-AUTHORITY`, with a premise correction recorded against Plan 122 SOFC fuel); the **DISTRESS-SIGNALS-9-12** flagship COMPLETE and presented for acceptance; **Plan 24 CLOSED** (Wave 8, 2026-09-17, signatures resolved 2026-09-18, ward staffing sealed under option b, `DEBT-PLAN24-MEDICAL-WARD-STAFFING` RETIRED); **19A/19B/19C** waves closed with evidence (Endgame 84/84 PASS, focused suites 47 PASS, `verify-fast.sh` reported ALL 47 GATES PASSED); **C2[2] Plan 17** legibility executed with a documented not-executed list (Plan 31 semantic-kind authority, 17C audio phases, 17B deep test matrix remain open gaps); **PR 3 content seal SEALED 2026-09-19** (`CF-P1-DISTRESS-CONTENT-SEAL`); the **availability consumer RETIRED** (Wave 9 Part 2, Option B approved; `SignalTrustAvailability` retained as a pure-math specification pin). Consequence: subject plans in the radio/distress domain must treat the rescue-signal runtime as *sealed and closed*, not as an open expansion surface, unless they extend it through its recorded seams.

> **DR-08 — Wave directories beyond the v1.0 history. VERIFIED.**
`docs/plans/` (126 entries) contains wave directories `wave8_part2/`, `wave9_part2/`, `wave10_part1/`, `wave10_part2/`, `wave11_part1/`, `wave11_part2/`, `wave12_part1_1/`, `flagship_b5_b8/`, and `xp/`, plus `UNCLAIMED_CORPUS_CENSUS.md`, `UNBLOCKED_PLANS_AUDIT_2026-09-19.md`, and `WAVE10_MICRO_DEFERRAL_SWEEP.md`. Two of these are standing expansion inputs: `UNCLAIMED_CORPUS_CENSUS.md` (authored content no system consumes — a utilization-seam backlog) and the unblocked-plans audit. The Factory Protocol consumes both.

> **DR-09 — Branch and agent sprawl. VERIFIED.**
The repository carries numerous agent- and CI-generated branches (`Zcode_Branch`, `bug_fixing_main`, multiple `chore/*` and `ci-autogen-*` branches) and a wide set of per-tool agent rulebooks at root (`CLAUDE.md`, `CODEX.md`, `CRUSH.md`, `GEMINI.md`, `GOOSE.md`, `MIMOCODE.md`, `QWEN.md`, `VIBE.md`, `.clinerules`, `.cursorrules`, `.windsurfrules`, `.zcode/`). Consequence: multi-agent discipline (worktree ownership, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`) is not optional; every factory-generated plan must carry an ownership-claim step. No expansion plan may assume it is the only writer.

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

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

The subject is an irreversible but legible faction branch path inside the combined Weight of Choices coordinator. The plan expands branch state, migration, host and UI truthfulness while preserving one cross-faction commitment authority.

- **rebel branch definitions and ordered lookup** remains with `RebelBranchCatalog` at `Assets/Ashfall.Core/Factions/RebelBranchCatalog.cs`. Immutable catalog after load; no mutable campaign state.
- **Rebel branch state, alignment, PoNR and ending** remains with `RebelBranchSystem` at `Assets/Ashfall.Core/Factions/RebelBranchSystem.cs`. Sole Rebel branch owner; reads moral band and IFlagLedger ports.
- **cross-faction exclusivity and shared branch lifecycle** remains with `FactionBranchCoordinator` at `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`. Owns the combined coordinator and active-kind invariant.
- **combined branch persistence and v1→v2 migration** remains with `WeightOfChoicesSaveCodec` at `Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs; Assets/Ashfall.Core/Factions/RebelBranchSave.cs`. Existing envelope; all branch sections are intentionally present whether committed or not.
- **host commands, save flush and panel route** remains with `FactionBranchHostSession/Main.FactionBranch` at `src/Host/FactionBranchHostSession.cs; src/Main.FactionBranch.cs; src/UI/FactionsPanel.cs`. Host/UI composition only; no second branch state.
- **catalog, system, exclusivity and integration proof** remains with `Rebel focused tests` at `Ashfall.Core.Tests/RebelBranchExpansionTests.cs; Ashfall.Core.Tests/RebelBranchSystemTests.cs; Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs`. Focused executable evidence.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load the 15 Rebel rows and the other branch catalogs
2. restore WeightOfChoicesSave and replay durable flags
3. query coordinator availability using current moral band/standing
4. commit one mutually exclusive branch through the coordinator
5. lock the authored PoNR exactly once
6. apply bounded Rebel alignment changes through the current faction seam
7. resolve the first matching ending row and project the result
8. save the combined envelope and refresh Factions/Quests

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Catalog rows are immutable; `RebelBranchSystemState` owns timeline, branch record, alignment and durable flag set.
- A branch commitment is first-commit-wins; PoNR lock is irreversible and idempotent; ending resolution is first-resolution-wins after lock.
- The combined save always writes Military/Rebel/Independent/PRPF sections, with uncommitted sections at truthful defaults.
- A save with more than one committed faction is corrupt and must be rejected or quarantined by the existing codec/host contract.

- A second cross-faction commitment is refused even if the other branch row is available.
- An out-of-band entry range returns a named reason and does not mutate the branch.
- Unknown branch/ending IDs fail closed; no middle-row fallback is used to conceal a malformed catalog during normal validation.
- Restored flags are visible to same-session gates immediately and cannot be double-counted by a replay.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/FactionBranchHostSession.cs
- src/Host/WeightOfChoicesSaveStore.cs
- src/Main.FactionBranch.cs
- src/UI/FactionsPanel.cs
- src/UI/QuestsPanel.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/RebelBranchExpansionTests.cs
- Ashfall.Core.Tests/RebelBranchSystemTests.cs
- Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs

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
| S-01 | 123-01 load 15 Rebel branches | load the 15 Rebel rows and the other branch catalogs | Catalog rows are immutable; `RebelBranchSystemState` owns timeline, branch record, alignment and durable flag set. | A player commits to both Rebel and Military through different UI paths. | RebelBranchCatalog |
| S-02 | 123-02 moral-band availability | restore WeightOfChoicesSave and replay durable flags | A branch commitment is first-commit-wins; PoNR lock is irreversible and idempotent; ending resolution is first-resolution-wins after lock. | A PoNR flag is set in a panel but not in durable state. | RebelBranchCatalog |
| S-03 | 123-03 out-of-band rejection | query coordinator availability using current moral band/standing | The combined save always writes Military/Rebel/Independent/PRPF sections, with uncommitted sections at truthful defaults. | A restored v1 save loses the active Rebel branch or duplicates its ending. | RebelBranchCatalog |
| S-04 | 123-04 first Rebel commitment | commit one mutually exclusive branch through the coordinator | A save with more than one committed faction is corrupt and must be rejected or quarantined by the existing codec/host contract. | A branch ending range gap is silently mapped to a different branch. | RebelBranchCatalog |
| S-05 | 123-05 Military exclusivity | lock the authored PoNR exactly once | Catalog rows are immutable; `RebelBranchSystemState` owns timeline, branch record, alignment and durable flag set. | A new rebel coordinator duplicates `FactionBranchCoordinator`. | RebelBranchCatalog |
| S-06 | 123-06 Independent exclusivity | apply bounded Rebel alignment changes through the current faction seam | A branch commitment is first-commit-wins; PoNR lock is irreversible and idempotent; ending resolution is first-resolution-wins after lock. | A player commits to both Rebel and Military through different UI paths. | RebelBranchCatalog |
| S-07 | 123-07 PoNR lock once | resolve the first matching ending row and project the result | The combined save always writes Military/Rebel/Independent/PRPF sections, with uncommitted sections at truthful defaults. | A PoNR flag is set in a panel but not in durable state. | RebelBranchCatalog |
| S-08 | 123-08 ending resolution once | save the combined envelope and refresh Factions/Quests | A save with more than one committed faction is corrupt and must be rejected or quarantined by the existing codec/host contract. | A restored v1 save loses the active Rebel branch or duplicates its ending. | RebelBranchCatalog |
| S-09 | 123-09 v1 to v2 restore | load the 15 Rebel rows and the other branch catalogs | Catalog rows are immutable; `RebelBranchSystemState` owns timeline, branch record, alignment and durable flag set. | A branch ending range gap is silently mapped to a different branch. | RebelBranchCatalog |
| S-10 | 123-10 conflicting multi-faction save | restore WeightOfChoicesSave and replay durable flags | A branch commitment is first-commit-wins; PoNR lock is irreversible and idempotent; ending resolution is first-resolution-wins after lock. | A new rebel coordinator duplicates `FactionBranchCoordinator`. | RebelBranchCatalog |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 123-TC-01 catalog schema/duplicate IDs | data | catalog schema/duplicate IDs; verify the current owner and its negative boundary without inventing a second authority. | RebelBranchCatalog |
| T-02 | 123-TC-02 15 branch census | unit | 15 branch census; verify the current owner and its negative boundary without inventing a second authority. | RebelBranchCatalog |
| T-03 | 123-TC-03 flag ID uniqueness | persistence | flag ID uniqueness; verify the current owner and its negative boundary without inventing a second authority. | RebelBranchCatalog |
| T-04 | 123-TC-04 ending range coverage | determinism | ending range coverage; verify the current owner and its negative boundary without inventing a second authority. | RebelBranchCatalog |
| T-05 | 123-TC-05 entry-band parse/compare | host | entry-band parse/compare; verify the current owner and its negative boundary without inventing a second authority. | RebelBranchCatalog |
| T-06 | 123-TC-06 first-commit-wins | UI/accessibility | first-commit-wins; verify the current owner and its negative boundary without inventing a second authority. | RebelBranchCatalog |
| T-07 | 123-TC-07 cross-faction exclusivity | cross-system | cross-faction exclusivity; verify the current owner and its negative boundary without inventing a second authority. | RebelBranchCatalog |
| T-08 | 123-TC-08 PoNR idempotence | data | PoNR idempotence; verify the current owner and its negative boundary without inventing a second authority. | RebelBranchCatalog |
| T-09 | 123-TC-09 alignment clamp | unit | alignment clamp; verify the current owner and its negative boundary without inventing a second authority. | RebelBranchCatalog |
| T-10 | 123-TC-10 ending idempotence | persistence | ending idempotence; verify the current owner and its negative boundary without inventing a second authority. | RebelBranchCatalog |
| T-11 | 123-TC-11 durable flag replay | determinism | durable flag replay; verify the current owner and its negative boundary without inventing a second authority. | RebelBranchCatalog |
| T-12 | 123-TC-12 WeightOfChoices v1 migration | host | WeightOfChoices v1 migration; verify the current owner and its negative boundary without inventing a second authority. | RebelBranchCatalog |
| T-13 | 123-TC-13 future version rejection | UI/accessibility | future version rejection; verify the current owner and its negative boundary without inventing a second authority. | RebelBranchCatalog |
| T-14 | 123-TC-14 host/UI projection | cross-system | host/UI projection; verify the current owner and its negative boundary without inventing a second authority. | RebelBranchCatalog |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 30 | `Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs` | current reference count; inspect the caller before treating it as a live route |
| 29 | `Ashfall.Core.Tests/WeightOfChoicesSaveTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 23 | `Ashfall.Core.Tests/RebelBranchSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 18 | `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` | current reference count; inspect the caller before treating it as a live route |
| 15 | `src/Host/WeightOfChoicesSaveStore.cs` | current reference count; inspect the caller before treating it as a live route |
| 13 | `Assets/Ashfall.Core/Factions/RebelBranchSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Ashfall.Core.Tests/RebelBranchExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `src/Host/FactionBranchHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/FactionBranchCoordinatorTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Factions/RebelBranchCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Factions/RebelBranchSave.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/HostCli.SelfTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Main.UiPanels.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/RebelBranchCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Muster/MusterWarfareEngine.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Main.FactionBranch.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/UI/FactionsPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/MusterWarfareTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Assets/Ashfall.Core/Factions/RebelBranchState.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/UI/QuestsPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/Campaign/CampaignConsequenceLedgerTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/Factions/Plan121_122FactionBranchIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/IndependentBranchCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/VersionReportContractTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Factions/IndependentBranchState.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Factions/PrpfIds.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Factions/PrpfState.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Factions/RebelBranchIds.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/HostCliRegistry.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/VersionReport.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/HostCli.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Main.PanelLifecycle.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Main.PlayerSurfaces.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/rebel_faction_branch.json`

### `Assets/StreamingAssets/Data/rebel_faction_branch.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 12323; characters: 12323.
- SHA-256: `eeeefcb394ec31bfaf018e664859bd08dcfabd0e5ae0d4b263391b823c6b3495`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `faction_id`, `branches`

#### `branches` — 15 current rows

- Row 001 `branch_rebel_1_true_rebel`: `{"display_name":"The True Rebel","endings":[{"band_max":"very_positive","band_min":"positive","display_name":"The Liberator","ending_id":"ending_rebel_1a_liberator"},{"band_max":"evil","band_min":"very_evil","display_name":"The Zealot","en…`
- Row 002 `branch_rebel_2_defector`: `{"display_name":"The Defector","endings":[{"band_max":"very_positive","band_min":"slightly_positive","display_name":"The Reformer","ending_id":"ending_rebel_2a_reformer_of_military"},{"band_max":"evil","band_min":"very_evil","display_name"…`
- Row 003 `branch_rebel_3_opportunist`: `{"display_name":"The Opportunist","endings":[{"band_max":"very_evil","band_min":"very_evil","display_name":"The Warlord","ending_id":"ending_rebel_3a_warlord"},{"band_max":"very_positive","band_min":"positive","display_name":"The Benevolen…`
- Row 004 `branch_rebel_4_martyr`: `{"display_name":"The Martyr","endings":[{"band_max":"very_positive","band_min":"very_positive","display_name":"The Martyr of the Cause","ending_id":"ending_rebel_4a_martyr_of_the_cause"},{"band_max":"positive","band_min":"positive","displa…`
- Row 005 `branch_rebel_5_warlord`: `{"display_name":"The Warlord","endings":[{"band_max":"very_evil","band_min":"very_evil","display_name":"The Warlord King","ending_id":"ending_rebel_5a_warlord_king"},{"band_max":"evil","band_min":"slightly_evil","display_name":"The Benevol…`
- Row 006 `branch_rebel_6_reformer`: `{"display_name":"The Reformer","endings":[{"band_max":"very_positive","band_min":"very_positive","display_name":"The Visionary","ending_id":"ending_rebel_6a_visionary"},{"band_max":"positive","band_min":"positive","display_name":"The Refor…`
- Row 007 `branch_rebel_7_lone_wolf`: `{"display_name":"The Lone Wolf","endings":[{"band_max":"slightly_positive","band_min":"neutral","display_name":"The Lone Survivor","ending_id":"ending_rebel_7a_lone_survivor"},{"band_max":"evil","band_min":"very_evil","display_name":"The T…`
- Row 008 `branch_rebel_8_revolution`: `{"display_name":"The Revolution","endings":[{"band_max":"very_positive","band_min":"slightly_positive","display_name":"The New Republic","ending_id":"ending_rebel_8a_new_republic"},{"band_max":"neutral","band_min":"slightly_evil","display_…`
- Row 009 `branch_rebel_9_bombmaker`: `{"display_name":"The Bombmaker","endings":[{"band_max":"very_positive","band_min":"positive","display_name":"The Repentant Defector","ending_id":"ending_rebel_9a_repentant_defector"},{"band_max":"slightly_positive","band_min":"slightly_evi…`
- Row 010 `branch_rebel_10_courier`: `{"display_name":"The Courier","endings":[{"band_max":"very_positive","band_min":"positive","display_name":"The Trusted Network","ending_id":"ending_rebel_10a_trusted_network"},{"band_max":"slightly_positive","band_min":"slightly_evil","dis…`
- Row 011 `branch_rebel_11_propagandist`: `{"display_name":"The Propagandist","endings":[{"band_max":"very_positive","band_min":"positive","display_name":"The Voice of the People","ending_id":"ending_rebel_11a_voice_of_the_people"},{"band_max":"slightly_positive","band_min":"slight…`
- Row 012 `branch_rebel_12_dissident`: `{"display_name":"The Dissident","endings":[{"band_max":"very_positive","band_min":"positive","display_name":"The Reconciled Civilian","ending_id":"ending_rebel_12a_reconciled_civilian"},{"band_max":"slightly_positive","band_min":"slightly_…`
- Row 013 `branch_rebel_13_protector`: `{"display_name":"The Protector","endings":[{"band_max":"very_positive","band_min":"positive","display_name":"The Community Shield","ending_id":"ending_rebel_13a_community_shield"},{"band_max":"slightly_positive","band_min":"slightly_evil",…`
- Row 014 `branch_rebel_14_saboteur`: `{"display_name":"The Saboteur","endings":[{"band_max":"very_positive","band_min":"positive","display_name":"The Resistance Hero","ending_id":"ending_rebel_14a_resistance_hero"},{"band_max":"slightly_positive","band_min":"slightly_evil","di…`
- Row 015 `branch_rebel_15_negotiator`: `{"display_name":"The Negotiator","endings":[{"band_max":"very_positive","band_min":"positive","display_name":"The Peacemaker","ending_id":"ending_rebel_15a_peacemaker"},{"band_max":"slightly_positive","band_min":"slightly_evil","display_na…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/independent_faction_branch.json`

### `Assets/StreamingAssets/Data/independent_faction_branch.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 12209; characters: 12207.
- SHA-256: `678a4bd46d2091ff54c62bd3617c4c1507a2582e520d0be46c8261e78cf5036e`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `faction_id`, `branches`

#### `branches` — 15 current rows

- Row 001 `branch_ind_1_survivor`: `{"display_name":"The Survivor","endings":[{"band_max":"slightly_positive","band_min":"neutral","display_name":"The Lone Survivor","ending_id":"ending_ind_1a_lone_survivor"},{"band_max":"evil","band_min":"very_evil","display_name":"The Trai…`
- Row 002 `branch_ind_2_mercenary`: `{"display_name":"The Mercenary","endings":[{"band_max":"evil","band_min":"very_evil","display_name":"The Warlord","ending_id":"ending_ind_2a_warlord"},{"band_max":"slightly_positive","band_min":"slightly_evil","display_name":"The Survivor"…`
- Row 003 `branch_ind_3_peacekeeper_diplomat`: `{"display_name":"The Peacekeeper / Diplomat","endings":[{"band_max":"very_positive","band_min":"very_positive","display_name":"The Peacekeeper Unifier","ending_id":"ending_ind_3a_peacekeeper_unifier"},{"band_max":"positive","band_min":"pos…`
- Row 004 `branch_ind_4_exile`: `{"display_name":"The Exile","endings":[{"band_max":"very_evil","band_min":"very_evil","display_name":"The Tyrant","ending_id":"ending_ind_4a_tyrant"},{"band_max":"evil","band_min":"slightly_evil","display_name":"The Ghost","ending_id":"end…`
- Row 005 `branch_ind_5_kingmaker`: `{"display_name":"The Kingmaker","endings":[{"band_max":"evil","band_min":"very_evil","display_name":"The Puppet Master","ending_id":"ending_ind_5a_puppet_master"},{"band_max":"slightly_positive","band_min":"slightly_evil","display_name":"T…`
- Row 006 `branch_ind_6_legend`: `{"display_name":"The Legend","endings":[{"band_max":"very_positive","band_min":"positive","display_name":"The Savior","ending_id":"ending_ind_6a_savior"},{"band_max":"evil","band_min":"very_evil","display_name":"The Monster","ending_id":"e…`
- Row 007 `branch_ind_7_ghost`: `{"display_name":"The Ghost","endings":[{"band_max":"slightly_positive","band_min":"neutral","display_name":"The Unseen","ending_id":"ending_ind_7a_unseen"},{"band_max":"slightly_evil","band_min":"very_evil","display_name":"The Forgotten","…`
- Row 008 `branch_ind_8_wasteland_myth`: `{"display_name":"The Wasteland Myth","endings":[{"band_max":"evil","band_min":"very_evil","display_name":"The Feared Legend","ending_id":"ending_ind_8a_feared_legend"},{"band_max":"very_positive","band_min":"positive","display_name":"The R…`
- Row 009 `branch_ind_9_hermit`: `{"display_name":"The Hermit","endings":[{"band_max":"slightly_positive","band_min":"neutral","display_name":"The Quiet Holding","ending_id":"ending_ind_9a_quiet_holding"},{"band_max":"very_positive","band_min":"positive","display_name":"Th…`
- Row 010 `branch_ind_10_mediator`: `{"display_name":"The Mediator","endings":[{"band_max":"very_positive","band_min":"positive","display_name":"The Open Table","ending_id":"ending_ind_10a_open_table"},{"band_max":"slightly_positive","band_min":"neutral","display_name":"The N…`
- Row 011 `branch_ind_11_scavenger_king`: `{"display_name":"The Scavenger King","endings":[{"band_max":"very_positive","band_min":"positive","display_name":"Quartermaster Without a Flag","ending_id":"ending_ind_11a_quartermaster"},{"band_max":"slightly_positive","band_min":"neutral…`
- Row 012 `branch_ind_12_caretaker`: `{"display_name":"The Caretaker","endings":[{"band_max":"very_positive","band_min":"positive","display_name":"House With Extra Chairs","ending_id":"ending_ind_12a_extra_chairs"},{"band_max":"slightly_positive","band_min":"neutral","display_…`
- Row 013 `branch_ind_13_witness`: `{"display_name":"The Witness","endings":[{"band_max":"very_positive","band_min":"positive","display_name":"The Record Stands","ending_id":"ending_ind_13a_record_stands"},{"band_max":"slightly_positive","band_min":"neutral","display_name":"…`
- Row 014 `branch_ind_14_engineer`: `{"display_name":"The Engineer","endings":[{"band_max":"very_positive","band_min":"positive","display_name":"Load-Bearing","ending_id":"ending_ind_14a_load_bearing"},{"band_max":"slightly_positive","band_min":"neutral","display_name":"Neces…`
- Row 015 `branch_ind_15_prophet`: `{"display_name":"The Prophet","endings":[{"band_max":"very_positive","band_min":"positive","display_name":"Keeper of Vigils","ending_id":"ending_ind_15a_keeper_of_vigils"},{"band_max":"slightly_positive","band_min":"neutral","display_name"…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/moral_choice_flags.json`

### `Assets/StreamingAssets/Data/moral_choice_flags.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 2090; characters: 2090.
- SHA-256: `e5a95235ce28f9d2a1c9c8bcaeb72789122d9d77e1d38b0658cc86ae4d6a4db4`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `description`, `flags`

#### `flags` — 25 current rows

- Row 001 `flag_betrayed_ally`: `{"display_name":"Betrayed an Ally","id":"flag_betrayed_ally"}`
- Row 002 `flag_betrayed_faction`: `{"display_name":"Betrayed a Faction","id":"flag_betrayed_faction"}`
- Row 003 `flag_betrayed_trust`: `{"display_name":"Broke Trust","id":"flag_betrayed_trust"}`
- Row 004 `flag_broken_pact`: `{"display_name":"Broken Pact","id":"flag_broken_pact"}`
- Row 005 `flag_become_warlord`: `{"display_name":"Became Warlord","id":"flag_become_warlord"}`
- Row 006 `flag_throne_of_ash`: `{"display_name":"Throne of Ash","id":"flag_throne_of_ash"}`
- Row 007 `flag_branch_mercy_road_locked`: `{"display_name":"Mercy Road Locked","id":"flag_branch_mercy_road_locked"}`
- Row 008 `flag_branch_iron_way_locked`: `{"display_name":"Iron Way Locked","id":"flag_branch_iron_way_locked"}`
- Row 009 `flag_branch_listener_locked`: `{"display_name":"Listener's Thread Locked","id":"flag_branch_listener_locked"}`
- Row 010 `flag_branch_broken_compact_locked`: `{"display_name":"Broken Compact Locked","id":"flag_branch_broken_compact_locked"}`
- Row 011 `flag_spared_raider`: `{"display_name":"Spared a Raider","id":"flag_spared_raider"}`
- Row 012 `flag_executed_prisoner`: `{"display_name":"Executed a Prisoner","id":"flag_executed_prisoner"}`
- Row 013 `flag_shared_rations`: `{"display_name":"Shared Rations","id":"flag_shared_rations"}`
- Row 014 `flag_hoarded_medicine`: `{"display_name":"Hoarded Medicine","id":"flag_hoarded_medicine"}`
- Row 015 `flag_sheltered_refugee`: `{"display_name":"Sheltered a Refugee","id":"flag_sheltered_refugee"}`
- Row 016 `flag_expelled_survivor`: `{"display_name":"Expelled a Survivor","id":"flag_expelled_survivor"}`
- Row 017 `flag_repaired_infrastructure`: `{"display_name":"Repaired Shared Infrastructure","id":"flag_repaired_infrastructure"}`
- Row 018 `flag_sabotaged_rival`: `{"display_name":"Sabotaged a Rival","id":"flag_sabotaged_rival"}`
- Row 019 `flag_broke_treaty`: `{"display_name":"Broke a Treaty","id":"flag_broke_treaty"}`
- Row 020 `flag_honored_debt`: `{"display_name":"Honored a Debt","id":"flag_honored_debt"}`
- Row 021 `flag_ignored_distress`: `{"display_name":"Ignored a Distress Call","id":"flag_ignored_distress"}`
- Row 022 `flag_responded_distress`: `{"display_name":"Responded to Distress","id":"flag_responded_distress"}`
- Row 023 `flag_forged_record`: `{"display_name":"Forged a Record","id":"flag_forged_record"}`
- Row 024 `flag_preserved_archive`: `{"display_name":"Preserved an Archive","id":"flag_preserved_archive"}`
- Row 025 `flag_chosen_faction_side`: `{"display_name":"Chose a Faction Side","id":"flag_chosen_faction_side"}`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Factions/RebelBranchSystem.cs`

### `Assets/Ashfall.Core/Factions/RebelBranchSystem.cs` — complete current file

- Size: 254 lines / 12146 bytes.
- SHA-256: `ffc81ec9b30340fcd1fcd1a8641a190567f1ccb8f4241726011fe3996e8291c4`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Ashfall.Core.Flags;
00004: using Ashfall.Core.MoralChoice;
00005:
00006: namespace Ashfall.Core.Factions
00007: {
00008:     /// <summary>
00009:     /// Resolution engine for the Rebel faction slice of "The Weight of
00010:     /// Choices" branching system. Structural mirror of MilitaryBranchSystem —
00011:     /// same method set, same idempotency and clamping rules — so the two
00012:     /// faction branch systems stay behaviorally identical even though their
00013:     /// branch/ending data differs. See MilitaryBranchSystem's remarks for the
00014:     /// full design rationale (morality as a gate, not a judgment; faction
00015:     /// alignment as a value distinct from the player's own morality score).
00016:     ///
00017:     /// Zero engine dependencies; deterministic; runs on its own local day
00018:     /// counter (RebelBranchTimelineState), independent of the global IClock,
00019:     /// of Year of Ash's day 180-360 window, and of MilitaryBranchSystem's own
00020:     /// timeline (a player is only ever on one of the two in a playthrough,
00021:     /// but the systems do not share a clock instance).
00022:     /// </summary>
00023:     public sealed class RebelBranchSystem
00024:     {
00025:         public const string SystemId = "rebel_branch_system";
00026:
00027:         /// <summary>Faction alignment is clamped to the same -200..+200 range as
00028:         /// MoralChoiceSystem purely for player-facing scale consistency.</summary>
00029:         public const int MinAlignment = -200;
00030:         public const int MaxAlignment = 200;
00031:
00032:         private readonly RebelBranchCatalog _catalog;
00033:         private readonly IFlagLedger _flags;
00034:         private readonly ILog _log;
00035:         private RebelBranchSystemState _state;
00036:
00037:         public event Action<string>? OnBranchCommitted;
00038:         public event Action<string, int>? OnPonrLocked;
00039:         public event Action<string>? OnEndingResolved;
00040:         public event Action<int>? OnAlignmentChanged;
00041:
00042:         public RebelBranchSystem(RebelBranchCatalog catalog, IFlagLedger flags, RebelBranchSystemState? state = null, ILog? log = null)
00043:         {
00044:             _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
00045:             _flags = flags ?? throw new ArgumentNullException(nameof(flags));
00046:             _log = log ?? NullLog.Instance;
00047:             _state = state ?? new RebelBranchSystemState();
00048:             if (_state.timeline == null) _state.timeline = new RebelBranchTimelineState();
00049:             if (_state.branch == null) _state.branch = new RebelBranchRecord();
00050:             if (_state.rebelAlignment == null)
00051:                 _state.rebelAlignment = new FactionAlignmentRecord { factionId = RebelBranchIds.FactionId, alignment = -80 };
00052:             if (_state.setFlags == null) _state.setFlags = new System.Collections.Generic.List<string>();
00053:         }
00054:
00055:         public RebelBranchSystemState State => _state;
00056:         public int CurrentDay => _state.timeline.currentDay;
00057:         public string? CommittedBranchId => string.IsNullOrEmpty(_state.branch.branchId) ? null : _state.branch.branchId;
00058:         public bool IsPonrLocked => _state.branch.ponrLocked;
00059:         public int RebelAlignment => _state.rebelAlignment.alignment;
00060:         public string? ResolvedEndingId => string.IsNullOrEmpty(_state.branch.resolvedEndingId) ? null : _state.branch.resolvedEndingId;
00061:
00062:         public void AdvanceDay(int day)
00063:         {
00064:             if (day < 0) throw new ArgumentOutOfRangeException(nameof(day));
00065:             if (day < _state.timeline.currentDay) return; // never rewind
00066:             _state.timeline.currentDay = day;
00067:         }
00068:
00069:         /// <summary>
00070:         /// Commits the player to a base Rebel branch. Soft-gated by the
00071:         /// branch's entry band range read from the catalog. Committing twice
00072:         /// is a no-op that returns the already-committed branch (first
00073:         /// commit wins), matching MilitaryBranchSystem.CommitBranch.
00074:         /// </summary>
00075:         public string CommitBranch(string branchId, MoralChoiceSystem moralChoice)
00076:         {
00077:             if (string.IsNullOrEmpty(branchId)) throw new ArgumentNullException(nameof(branchId));
00078:             if (moralChoice == null) throw new ArgumentNullException(nameof(moralChoice));
00079:
00080:             if (_state.branch.committed)
00081:             {
00082:                 _log.Warn($"Rebel branch already committed to '{_state.branch.branchId}'; ignoring commit to '{branchId}'.");
00083:                 return _state.branch.branchId;
00084:             }
00085:
00086:             var def = _catalog.GetById(branchId);
00087:             if (def == null)
00088:                 throw new ArgumentException($"Unknown Rebel branch id '{branchId}'.", nameof(branchId));
00089:
00090:             var band = moralChoice.CurrentBand;
00091:             var min = ParseBand(def.entry_band_min);
00092:             var max = ParseBand(def.entry_band_max);
00093:             if (band < min || band > max)
00094:             {
00095:                 throw new InvalidOperationException(
00096:                     $"Branch '{branchId}' requires a morality band between {min} and {max}; " +
00097:                     $"current band is {band}. Morality is a gate, not a judgment — a different " +
00098:                     "branch is accessible at this band.");
00099:             }
00100:
00101:             _state.branch.branchId = branchId;
00102:             _state.branch.committed = true;
00103:             OnBranchCommitted?.Invoke(branchId);
00104:             return branchId;
00105:         }
00106:
00107:         /// <summary>
00108:         /// Fires the committed branch's point-of-no-return. Irreversible: once
00109:         /// locked, the flag is set in both the runtime IFlagLedger and the
00110:         /// save-durable setFlags list. Locking twice is a no-op.
00111:         /// </summary>
00112:         public void LockPointOfNoReturn()
00113:         {
00114:             if (!_state.branch.committed)
00115:                 throw new InvalidOperationException("Cannot lock a point-of-no-return before a branch is committed.");
00116:             if (_state.branch.ponrLocked) return;
00117:
00118:             string flagId = RebelBranchIds.PonrFlagFor(_state.branch.branchId);
00119:             _state.branch.ponrLocked = true;
00120:             _state.branch.ponrLockedDay = _state.timeline.currentDay;
00121:             SetDurableFlag(flagId);
00122:             OnPonrLocked?.Invoke(flagId, _state.timeline.currentDay);
00123:         }
00124:
00125:         /// <summary>
00126:         /// Shifts the Rebel faction's OWN internal alignment (not the
00127:         /// player's MoralChoiceSystem score) toward good or evil as a
00128:         /// consequence of the player's in-faction choices. Clamped to
00129:         /// -200..+200, mirroring FactionStandingRecord's clamp-on-write shape.
00130:         /// </summary>
00131:         public void ShiftFactionAlignment(int delta)
00132:         {
00133:             int next = Math.Clamp(_state.rebelAlignment.alignment + delta, MinAlignment, MaxAlignment);
00134:             _state.rebelAlignment.alignment = next;
00135:             OnAlignmentChanged?.Invoke(next);
00136:         }
00137:
00138:         /// <summary>
00139:         /// Resolves the ending for the committed, PoNR-locked branch using the
00140:         /// player's current morality band. Idempotent: resolving twice
00141:         /// returns the first-resolved ending.
00142:         /// </summary>
00143:         public string ResolveEnding(MoralChoiceSystem moralChoice)
00144:         {
00145:             if (moralChoice == null) throw new ArgumentNullException(nameof(moralChoice));
00146:             if (!_state.branch.ponrLocked)
00147:                 throw new InvalidOperationException("Cannot resolve an ending before the point-of-no-return has locked the branch.");
00148:
00149:             if (!string.IsNullOrEmpty(_state.branch.resolvedEndingId))
00150:                 return _state.branch.resolvedEndingId;
00151:
00152:             var def = _catalog.GetById(_state.branch.branchId)
00153:                 ?? throw new InvalidOperationException($"Committed branch '{_state.branch.branchId}' is missing from the catalog.");
00154:
00155:             var band = moralChoice.CurrentBand;
00156:             foreach (var ending in def.endings)
00157:             {
00158:                 var min = ParseBand(ending.band_min);
00159:                 var max = ParseBand(ending.band_max);
00160:                 if (band >= min && band <= max)
00161:                 {
00162:                     _state.branch.resolvedEndingId = ending.ending_id;
00163:                     OnEndingResolved?.Invoke(ending.ending_id);
00164:                     return ending.ending_id;
00165:                 }
00166:             }
00167:
00168:             // No row matched (a gap in authored ranges) — fall back to the
00169:             // middle-listed ending rather than throwing, so a data gap never
00170:             // hard-crashes a playthrough's final resolution.
00171:             var fallback = def.endings.Count > 0 ? def.endings[def.endings.Count / 2].ending_id : string.Empty;
00172:             _state.branch.resolvedEndingId = fallback;
00173:             if (!string.IsNullOrEmpty(fallback)) OnEndingResolved?.Invoke(fallback);
00174:             return fallback;
00175:         }
00176:
00177:         /// <summary>
00178:         /// Full-party-wipe loss check. Deliberately a pure function over a
00179:         /// caller-supplied living-survivor count, identical to
00180:         /// MilitaryBranchSystem.IsGameOver — no SurvivorRosterSystem exists
00181:         /// in Core yet, and the game-over check is faction-agnostic, so it is
00182:         /// not duplicated logic drift risk, just the same pure function
00183:         /// mirrored for symmetry with the Military slice.
00184:         /// </summary>
00185:         public static bool IsGameOver(int livingSurvivorCount) => livingSurvivorCount <= 0;
00186:
00187:         private void SetDurableFlag(string flagId)
00188:         {
00189:             _flags.Set(flagId);
00190:             if (!_state.setFlags.Contains(flagId))
00191:                 _state.setFlags.Add(flagId);
00192:         }
00193:
00194:         private static MoralPathBand ParseBand(string band) => band switch
00195:         {
00196:             "very_evil" => MoralPathBand.VeryEvil,
00197:             "evil" => MoralPathBand.Evil,
00198:             "slightly_evil" => MoralPathBand.SlightlyEvil,
00199:             "neutral" => MoralPathBand.Neutral,
00200:             "slightly_positive" => MoralPathBand.SlightlyPositive,
00201:             "positive" => MoralPathBand.Positive,
00202:             "very_positive" => MoralPathBand.VeryPositive,
00203:             _ => throw new ArgumentException($"Unknown morality band token '{band}'.", nameof(band))
00204:         };
00205:
00206:         public RebelBranchSystemState CaptureState() => Clone(_state);
00207:
00208:         public void RestoreState(RebelBranchSystemState state)
00209:         {
00210:             if (state == null) return;
00211:             if (!string.Equals(state.systemId, SystemId, StringComparison.Ordinal))
00212:             {
00213:                 throw new ArgumentException(
00214:                     $"State belongs to system '{state.systemId}', expected '{SystemId}'.", nameof(state));
00215:             }
00216:             if (state.schemaVersion > 1)
00217:             {
00218:                 throw new NotSupportedException(
00219:                     $"Future Rebel branch save schema {state.schemaVersion}; supported schema is 1.");
00220:             }
00221:             _state = Clone(state);
00222:
00223:             // Replay durable flags into the runtime ledger so same-session
00224:             // gating checks (IFlagLedger.IsSet) agree with the restored save
00225:             // immediately — IFlagLedger itself is not persisted.
00226:             foreach (var flagId in _state.setFlags)
00227:                 _flags.Set(flagId);
00228:         }
00229:
00230:         private static RebelBranchSystemState Clone(RebelBranchSystemState source)
00231:         {
00232:             return new RebelBranchSystemState
00233:             {
00234:                 systemId = source.systemId,
00235:                 schemaVersion = source.schemaVersion,
00236:                 timeline = new RebelBranchTimelineState { currentDay = source.timeline.currentDay },
00237:                 branch = new RebelBranchRecord
00238:                 {
00239:                     branchId = source.branch.branchId,
00240:                     committed = source.branch.committed,
00241:                     ponrLocked = source.branch.ponrLocked,
00242:                     ponrLockedDay = source.branch.ponrLockedDay,
00243:                     resolvedEndingId = source.branch.resolvedEndingId
00244:                 },
00245:                 rebelAlignment = new FactionAlignmentRecord
00246:                 {
00247:                     factionId = source.rebelAlignment.factionId,
00248:                     alignment = source.rebelAlignment.alignment
00249:                 },
00250:                 setFlags = new System.Collections.Generic.List<string>(source.setFlags ?? new System.Collections.Generic.List<string>())
00251:             };
00252:         }
00253:     }
00254: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Factions/RebelBranchCatalog.cs`

### `Assets/Ashfall.Core/Factions/RebelBranchCatalog.cs` — complete current file

- Size: 83 lines / 3522 bytes.
- SHA-256: `3be27e0a1f977c20b995ee7eeded13dc466c21cdf683f4191e9ce3e2753d7f29`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections;
00004: using System.Collections.Generic;
00005:
00006: namespace Ashfall.Core.Factions
00007: {
00008:     /// <summary>One morality-gated ending row nested under a Rebel branch definition.</summary>
00009:     public sealed class RebelBranchEndingEntry
00010:     {
00011:         public string ending_id { get; set; } = string.Empty;
00012:         public string band_min { get; set; } = string.Empty;
00013:         public string band_max { get; set; } = string.Empty;
00014:         public string display_name { get; set; } = string.Empty;
00015:     }
00016:
00017:     /// <summary>One base Rebel branch row, matching rebel_faction_branch.json shape.</summary>
00018:     public sealed class RebelBranchEntry
00019:     {
00020:         public string id { get; set; } = string.Empty;
00021:         public string display_name { get; set; } = string.Empty;
00022:         public string ponr_flag { get; set; } = string.Empty;
00023:         public string ponr_trigger { get; set; } = string.Empty;
00024:         public string entry_band_min { get; set; } = string.Empty;
00025:         public string entry_band_max { get; set; } = string.Empty;
00026:         public List<RebelBranchEndingEntry> endings { get; set; } = new List<RebelBranchEndingEntry>();
00027:     }
00028:
00029:     /// <summary>Root shape of rebel_faction_branch.json.</summary>
00030:     public sealed class RebelBranchDataFile
00031:     {
00032:         public int schema_version { get; set; } = 1;
00033:         public string faction_id { get; set; } = string.Empty;
00034:         public List<RebelBranchEntry> branches { get; set; } = new List<RebelBranchEntry>();
00035:     }
00036:
00037:     /// <summary>Immutable-after-load catalog of Rebel branch/ending definitions.</summary>
00038:     public sealed class RebelBranchCatalog : IEnumerable<RebelBranchEntry>
00039:     {
00040:         private readonly Dictionary<string, RebelBranchEntry> _byId =
00041:             new Dictionary<string, RebelBranchEntry>(StringComparer.Ordinal);
00042:         private readonly List<RebelBranchEntry> _order = new List<RebelBranchEntry>();
00043:
00044:         public int Count => _order.Count;
00045:         public RebelBranchEntry this[int index] => _order[index];
00046:
00047:         public static RebelBranchCatalog Empty() => new RebelBranchCatalog();
00048:
00049:         public void Register(RebelBranchEntry entry)
00050:         {
00051:             if (entry == null || string.IsNullOrEmpty(entry.id) || _byId.ContainsKey(entry.id)) return;
00052:             _byId[entry.id] = entry;
00053:             _order.Add(entry);
00054:         }
00055:
00056:         public RebelBranchEntry? GetById(string id) =>
00057:             string.IsNullOrEmpty(id) ? null : (_byId.TryGetValue(id, out var e) ? e : null);
00058:
00059:         public bool Contains(string id) => GetById(id) != null;
00060:
00061:         public IEnumerator<RebelBranchEntry> GetEnumerator() => _order.GetEnumerator();
00062:         IEnumerator IEnumerable.GetEnumerator() => _order.GetEnumerator();
00063:
00064:         public static RebelBranchCatalog LoadAndRegister(string dataDir, IFileIO fileIO, IJsonSerializer json)
00065:         {
00066:             if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
00067:             if (json == null) throw new ArgumentNullException(nameof(json));
00068:
00069:             var catalog = new RebelBranchCatalog();
00070:             string path = fileIO.Combine(dataDir, "rebel_faction_branch.json");
00071:             if (!fileIO.FileExists(path)) return catalog;
00072:
00073:             string text = fileIO.ReadAllText(path);
00074:             var file = json.Deserialize<RebelBranchDataFile>(text);
00075:             if (file?.branches == null) return catalog;
00076:
00077:             foreach (var entry in file.branches)
00078:                 catalog.Register(entry);
00079:
00080:             return catalog;
00081:         }
00082:     }
00083: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`

### `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` — bounded current excerpt (638 of 667 lines)

- Size: 667 lines / 27660 bytes.
- SHA-256: `e5d365c6263dfb636692ae8dc1994210b13b97fd3bdf4c84f849d05a96dcde4f`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.Flags;
00005: using Ashfall.Core.MoralChoice;
00006:
00007: namespace Ashfall.Core.Factions
00008: {
00009:     public enum FactionBranchKind
00010:     {
00011:         None = 0,
00012:         Military = 1,
00013:         Rebel = 2,
00014:         Independent = 3
00015:     }
00016:
00017:     /// <summary>
00018:     /// UI-ready descriptor of a faction branch option with availability and consequences.
00019:     /// </summary>
00020:     public sealed class FactionBranchOption
00021:     {
00022:         public string BranchId { get; set; } = string.Empty;
00023:         public FactionBranchKind FactionKind { get; set; }
00024:         public string FactionId { get; set; } = string.Empty;
00025:         public string DisplayName { get; set; } = string.Empty;
00026:         public string PonrFlag { get; set; } = string.Empty;
00027:         public string PonrTrigger { get; set; } = string.Empty;
00028:         public string EntryBandMin { get; set; } = string.Empty;
00029:         public string EntryBandMax { get; set; } = string.Empty;
00030:         public bool IsCommitted { get; set; }
00031:         public bool IsPonrLocked { get; set; }
00032:         public bool IsAvailable { get; set; }
00033:         public string? LockoutReason { get; set; }
00034:         public List<string> PossibleEndings { get; set; } = new List<string>();
00035:         public string ConsequencesSummary { get; set; } = string.Empty;
00036:     }
00037:
00038:     /// <summary>
00039:     /// UI-ready summary of a faction's standing and alignment metrics.
00040:     /// </summary>
00041:     public sealed class FactionStandingSummary
00042:     {
00043:         public string FactionId { get; set; } = string.Empty;
00044:         public string DisplayName { get; set; } = string.Empty;
00045:         public int Standing { get; set; }
00046:         public int Alignment { get; set; }
00047:         public bool IsHostile { get; set; }
00048:         public bool IsAllied { get; set; }
00049:         public bool IsJoined { get; set; }
00050:         public bool IsOpposed { get; set; }
00051:     }
00052:
00053:     /// <summary>
00054:     /// Unified coordinator for "The Weight of Choices" faction progression layer.
00055:     ///
00056:     /// Manages the four constituent systems:
00057:     /// - MilitaryBranchSystem (15 Military branches, faction alignment, PoNR)
00058:     /// - RebelBranchSystem (15 Rebel branches, faction alignment, PoNR)
00059:     /// - IndependentBranchSystem (15 Independent branches, cross-faction relations, PoNR)
00060:     /// - PrpfStandingSystem (PRPF third-power standing, alignment, join/oppose)
00061:     ///
00062:     /// Invariants:
00063:     /// 1. Base faction commitment is strictly mutually exclusive: committing to one faction
00064:     ///    locks out the others.
00065:     /// 2. PRPF standing/alignment is durable and shared across all playthrough paths.
00066:     /// 3. Zero engine coupling, fully deterministic, save/load safe via WeightOfChoicesSaveCodec.
00067:     /// </summary>
00068:     public sealed class FactionBranchCoordinator
00069:     {
00070:         public const string SystemId = "faction_branch_coordinator";
00071:
00072:         private readonly MilitaryBranchCatalog _militaryCatalog;
00073:         private readonly RebelBranchCatalog _rebelCatalog;
00074:         private readonly IndependentBranchCatalog _independentCatalog;
00075:         private readonly IFlagLedger _flags;
00076:         private readonly ILog _log;
00077:
00078:         public MilitaryBranchSystem Military { get; }
00079:         public RebelBranchSystem Rebel { get; }
00080:         public IndependentBranchSystem Independent { get; }
00081:         public PrpfStandingSystem Prpf { get; }
00082:
00083:         public event Action<FactionBranchKind, string>? OnBranchCommitted;
00084:         public event Action<string, int>? OnPonrLocked;
00085:         public event Action<string>? OnEndingResolved;
00086:         public event Action? OnStateChanged;
00087:
00088:         public FactionBranchCoordinator(
00089:             MilitaryBranchCatalog? militaryCatalog = null,
00090:             RebelBranchCatalog? rebelCatalog = null,
00091:             IndependentBranchCatalog? independentCatalog = null,
00092:             IFlagLedger? flags = null,
00093:             ILog? log = null,
00094:             MilitaryBranchSystemState? militaryState = null,
00095:             RebelBranchSystemState? rebelState = null,
00096:             IndependentBranchSystemState? independentState = null,
00097:             PrpfSystemState? prpfState = null)
00098:         {
00099:             _flags = flags ?? new InMemoryFlagLedger();
00100:             _log = log ?? NullLog.Instance;
00101:
00102:             _militaryCatalog = militaryCatalog ?? MilitaryBranchCatalog.Empty();
00103:             _rebelCatalog = rebelCatalog ?? RebelBranchCatalog.Empty();
00104:             _independentCatalog = independentCatalog ?? IndependentBranchCatalog.Empty();
00105:
00106:             Military = new MilitaryBranchSystem(_militaryCatalog, _flags, militaryState, _log);
00107:             Rebel = new RebelBranchSystem(_rebelCatalog, _flags, rebelState, _log);
00108:             Independent = new IndependentBranchSystem(_independentCatalog, _flags, independentState, _log);
00109:             Prpf = new PrpfStandingSystem(_flags, prpfState, _log);
00110:
00111:             WireEvents();
00112:         }
00114:         private void WireEvents()
00115:         {
00116:             Military.OnBranchCommitted += branchId =>
00117:             {
00118:                 OnBranchCommitted?.Invoke(FactionBranchKind.Military, branchId);
00119:                 OnStateChanged?.Invoke();
00120:             };
00121:             Military.OnPonrLocked += (branchId, day) =>
00122:             {
00123:                 OnPonrLocked?.Invoke(branchId, day);
00124:                 OnStateChanged?.Invoke();
00125:             };
00126:             Military.OnEndingResolved += endingId =>
00127:             {
00128:                 OnEndingResolved?.Invoke(endingId);
00129:                 OnStateChanged?.Invoke();
00130:             };
00131:             Military.OnAlignmentChanged += _ => OnStateChanged?.Invoke();
00132:
00133:             Rebel.OnBranchCommitted += branchId =>
00134:             {
00135:                 OnBranchCommitted?.Invoke(FactionBranchKind.Rebel, branchId);
00136:                 OnStateChanged?.Invoke();
00137:             };
00138:             Rebel.OnPonrLocked += (branchId, day) =>
00139:             {
00140:                 OnPonrLocked?.Invoke(branchId, day);
00141:                 OnStateChanged?.Invoke();
00142:             };
00143:             Rebel.OnEndingResolved += endingId =>
00144:             {
00145:                 OnEndingResolved?.Invoke(endingId);
00146:                 OnStateChanged?.Invoke();
00147:             };
00148:             Rebel.OnAlignmentChanged += _ => OnStateChanged?.Invoke();
00149:
00150:             Independent.OnBranchCommitted += branchId =>
00151:             {
00152:                 OnBranchCommitted?.Invoke(FactionBranchKind.Independent, branchId);
00153:                 OnStateChanged?.Invoke();
00154:             };
00155:             Independent.OnPonrLocked += (branchId, day) =>
00156:             {
00157:                 OnPonrLocked?.Invoke(branchId, day);
00158:                 OnStateChanged?.Invoke();
00159:             };
00160:             Independent.OnEndingResolved += endingId =>
00161:             {
00162:                 OnEndingResolved?.Invoke(endingId);
00163:                 OnStateChanged?.Invoke();
00164:             };
00165:             Independent.OnMilitaryStandingChanged += _ => OnStateChanged?.Invoke();
00166:             Independent.OnRebelStandingChanged += _ => OnStateChanged?.Invoke();
00167:
00168:             Prpf.OnStandingChanged += _ => OnStateChanged?.Invoke();
00169:             Prpf.OnAlignmentChanged += _ => OnStateChanged?.Invoke();
00170:             Prpf.OnJoined += () => OnStateChanged?.Invoke();
00171:             Prpf.OnOpposed += () => OnStateChanged?.Invoke();
00172:         }
00173:
00174:         public FactionBranchKind ActiveFactionKind
00175:         {
00176:             get
00177:             {
00178:                 if (Military.State.branch.committed) return FactionBranchKind.Military;
00179:                 if (Rebel.State.branch.committed) return FactionBranchKind.Rebel;
00180:                 if (Independent.State.branch.committed) return FactionBranchKind.Independent;
00181:                 return FactionBranchKind.None;
00182:             }
00183:         }
00184:
00185:         public string? ActiveBranchId
00186:         {
00187:             get
00188:             {
00189:                 return ActiveFactionKind switch
00190:                 {
00191:                     FactionBranchKind.Military => Military.CommittedBranchId,
00192:                     FactionBranchKind.Rebel => Rebel.CommittedBranchId,
00193:                     FactionBranchKind.Independent => Independent.CommittedBranchId,
00194:                     _ => null
00195:                 };
00196:             }
00197:         }
00198:
00199:         public bool IsCommitted => ActiveFactionKind != FactionBranchKind.None;
00200:
00201:         public bool IsPonrLocked
00202:         {
00203:             get
00204:             {
00205:                 return ActiveFactionKind switch
00206:                 {
00207:                     FactionBranchKind.Military => Military.IsPonrLocked,
00208:                     FactionBranchKind.Rebel => Rebel.IsPonrLocked,
00209:                     FactionBranchKind.Independent => Independent.IsPonrLocked,
00210:                     _ => false
00211:                 };
00212:             }
00213:         }
00214:
00215:         public string? ResolvedEndingId
00216:         {
00217:             get
00218:             {
00219:                 return ActiveFactionKind switch
00220:                 {
00221:                     FactionBranchKind.Military => Military.ResolvedEndingId,
00222:                     FactionBranchKind.Rebel => Rebel.ResolvedEndingId,
00223:                     FactionBranchKind.Independent => Independent.ResolvedEndingId,
00224:                     _ => null
00225:                 };
00226:             }
00227:         }
00228:
00229:         public int CurrentDay =>
00230:             Math.Max(Military.CurrentDay, Math.Max(Rebel.CurrentDay, Independent.CurrentDay));
00231:
00232:         public void AdvanceDay(int day)
00233:         {
00234:             if (day < 0) throw new ArgumentOutOfRangeException(nameof(day));
00235:             Military.AdvanceDay(day);
00236:             Rebel.AdvanceDay(day);
00237:             Independent.AdvanceDay(day);
00238:             OnStateChanged?.Invoke();
00239:         }
00240:
00241:         public bool CanCommit(string branchId, MoralChoiceSystem moralChoice, out string? reason)
00242:         {
00243:             if (string.IsNullOrEmpty(branchId))
00244:             {
00245:                 reason = "missing_branch_id";
00253:
00254:             var kind = DetectBranchKind(branchId);
00255:             if (kind == FactionBranchKind.None)
00256:             {
00257:                 reason = $"unknown_branch_id '{branchId}'";
00258:                 return false;
00259:             }
00260:
00261:             // Exclusivity check
00262:             if (IsCommitted)
00263:             {
00264:                 if (ActiveFactionKind != kind)
00265:                 {
00266:                     reason = $"Already committed to {ActiveFactionKind} branch '{ActiveBranchId}'. Faction branches are mutually exclusive.";
00267:                     return false;
00268:                 }
00269:                 if (string.Equals(ActiveBranchId, branchId, StringComparison.Ordinal))
00270:                 {
00272:                     return true; // Already on this branch
00273:                 }
00274:                 reason = $"Already committed to branch '{ActiveBranchId}' in {ActiveFactionKind}.";
00275:                 return false;
00276:             }
00277:
00278:             var band = moralChoice.CurrentBand;
00279:
00280:             if (kind == FactionBranchKind.Military)
00281:             {
00282:                 var def = _militaryCatalog.GetById(branchId);
00283:                 if (def == null) { reason = "unknown_military_branch"; return false; }
00284:                 var min = ParseBand(def.entry_band_min);
00285:                 var max = ParseBand(def.entry_band_max);
00286:                 if (band < min || band > max)
00290:                 }
00291:             }
00292:             else if (kind == FactionBranchKind.Rebel)
00293:             {
00294:                 var def = _rebelCatalog.GetById(branchId);
00295:                 if (def == null) { reason = "unknown_rebel_branch"; return false; }
00296:                 var min = ParseBand(def.entry_band_min);
00297:                 var max = ParseBand(def.entry_band_max);
00298:                 if (band < min || band > max)
00302:                 }
00303:             }
00304:             else if (kind == FactionBranchKind.Independent)
00305:             {
00306:                 var def = _independentCatalog.GetById(branchId);
00307:                 if (def == null) { reason = "unknown_independent_branch"; return false; }
00308:                 var min = ParseBand(def.entry_band_min);
00309:                 var max = ParseBand(def.entry_band_max);
00310:                 if (band < min || band > max)
00318:                     return false;
00319:                 }
00320:                 if (def.requires_hostile_to_military == true && !Independent.IsHostileToMilitary)
00321:                 {
00322:                     reason = "Requires hostile standing with Military (standing <= -50).";
00323:                     return false;
00324:                 }
00325:                 if (def.requires_hostile_to_rebel == true && !Independent.IsHostileToRebel)
00326:                 {
00327:                     reason = "Requires hostile standing with Rebels (standing <= -50).";
00328:                     return false;
00329:                 }
00330:             }
00331:
00334:         }
00335:
00336:         public ActionResult CommitBranch(string branchId, MoralChoiceSystem moralChoice)
00337:         {
00338:             if (string.IsNullOrEmpty(branchId))
00339:                 return ActionResult.Failed("missing_branch_id", "branch.missing_id");
00340:             if (moralChoice == null)
00341:                 return ActionResult.Failed("missing_moral_choice", "branch.missing_moral_choice");
00342:
00343:             if (!CanCommit(branchId, moralChoice, out string? reason))
00344:                 return ActionResult.Blocked("commitment_blocked", reason ?? "branch.cannot_commit");
00345:
00346:             var kind = DetectBranchKind(branchId);
00347:             try
00348:             {
00349:                 string committedId = kind switch
00350:                 {
00351:                     FactionBranchKind.Military => Military.CommitBranch(branchId, moralChoice),
00352:                     FactionBranchKind.Rebel => Rebel.CommitBranch(branchId, moralChoice),
00353:                     FactionBranchKind.Independent => Independent.CommitBranch(branchId, moralChoice, Prpf),
00354:                     _ => throw new InvalidOperationException($"Cannot commit to unknown faction kind for branch '{branchId}'.")
00355:                 };
00356:                 return ActionResult.Success($"branch.committed:{committedId}");
00357:             }
00358:             catch (Exception ex)
00359:             {
00360:                 _log.Error($"FactionBranchCoordinator commit error: {ex.Message}");
00361:                 return ActionResult.Failed("commit_exception", ex.Message);
00362:             }
00363:         }
00364:
00365:         public ActionResult LockPonr(int day)
00366:         {
00367:             if (!IsCommitted)
00368:                 return ActionResult.Blocked("not_committed", "branch.ponr_requires_commitment");
00369:             if (IsPonrLocked)
00370:                 return ActionResult.Success("branch.ponr_already_locked");
00371:
00372:             try
00373:             {
00374:                 if (day > CurrentDay)
00375:                     AdvanceDay(day);
00376:
00377:                 switch (ActiveFactionKind)
00378:                 {
00379:                     case FactionBranchKind.Military:
00380:                         Military.LockPointOfNoReturn();
00381:                         break;
00382:                     case FactionBranchKind.Rebel:
00383:                         Rebel.LockPointOfNoReturn();
00384:                         break;
00385:                     case FactionBranchKind.Independent:
00386:                         Independent.LockPointOfNoReturn();
00387:                         break;
00388:                 }
00389:                 return ActionResult.Success("branch.ponr_locked");
00390:             }
00391:             catch (Exception ex)
00392:             {
00393:                 return ActionResult.Failed("ponr_error", ex.Message);
00394:             }
00395:         }
00396:
00397:         public ActionResult ResolveEnding(MoralChoiceSystem moralChoice)
00398:         {
00399:             if (!IsCommitted)
00400:                 return ActionResult.Blocked("not_committed", "branch.ending_requires_commitment");
00401:             if (!IsPonrLocked)
00402:                 return ActionResult.Blocked("ponr_not_locked", "branch.ending_requires_ponr");
00403:
00404:             try
00405:             {
00406:                 string endingId = ActiveFactionKind switch
00407:                 {
00408:                     FactionBranchKind.Military => Military.ResolveEnding(moralChoice),
00409:                     FactionBranchKind.Rebel => Rebel.ResolveEnding(moralChoice),
00410:                     FactionBranchKind.Independent => Independent.ResolveEnding(moralChoice),
00411:                     _ => throw new InvalidOperationException("No active faction to resolve ending.")
00412:                 };
00413:                 return ActionResult.Success($"branch.ending_resolved:{endingId}");
00414:             }
00415:             catch (Exception ex)
00416:             {
00417:                 return ActionResult.Failed("ending_error", ex.Message);
00418:             }
00419:         }
00420:
00421:         public void ModifyStanding(string factionId, int delta)
00422:         {
00423:             if (string.Equals(factionId, PrpfIds.FactionId, StringComparison.Ordinal))
00424:             {
00425:                 Prpf.ModifyStanding(delta);
00426:             }
00427:             else if (string.Equals(factionId, MilitaryBranchIds.FactionId, StringComparison.Ordinal))
00428:             {
00429:                 Independent.ModifyMilitaryStanding(delta);
00430:             }
00431:             else if (string.Equals(factionId, RebelBranchIds.FactionId, StringComparison.Ordinal))
00432:             {
00433:                 Independent.ModifyRebelStanding(delta);
00434:             }
00435:             OnStateChanged?.Invoke();
00436:         }
00437:
00438:         public void ShiftFactionAlignment(string factionId, int delta)
00439:         {
00440:             if (string.Equals(factionId, MilitaryBranchIds.FactionId, StringComparison.Ordinal))
00441:             {
00442:                 Military.ShiftFactionAlignment(delta);
00443:             }
00444:             else if (string.Equals(factionId, RebelBranchIds.FactionId, StringComparison.Ordinal))
00445:             {
00446:                 Rebel.ShiftFactionAlignment(delta);
00447:             }
00448:             else if (string.Equals(factionId, PrpfIds.FactionId, StringComparison.Ordinal))
00449:             {
00450:                 Prpf.ShiftFactionAlignment(delta);
00451:             }
00452:             OnStateChanged?.Invoke();
00453:         }
00454:
00455:         public bool TryJoinPrpf(MoralChoiceSystem moralChoice)
00456:         {
00457:             bool ok = Prpf.TryJoin(moralChoice);
00458:             if (ok) OnStateChanged?.Invoke();
00459:             return ok;
00460:         }
00461:
00462:         public void OpposePrpf()
00463:         {
00464:             Prpf.Oppose();
00465:             OnStateChanged?.Invoke();
00466:         }
00467:
00468:         /// <summary>Advance PRPF daily influence once join/oppose is committed.</summary>
00469:         public void TickDay(int day)
00470:         {
00471:             Prpf.TickDay(day);
00472:         }
00473:
00474:         public FactionBranchKind DetectBranchKind(string branchId)
00475:         {
00476:             if (string.IsNullOrEmpty(branchId)) return FactionBranchKind.None;
00477:             if (_militaryCatalog.Contains(branchId) || branchId.StartsWith("branch_mil_", StringComparison.Ordinal))
00478:                 return FactionBranchKind.Military;
00479:             if (_rebelCatalog.Contains(branchId) || branchId.StartsWith("branch_rebel_", StringComparison.Ordinal))
00480:                 return FactionBranchKind.Rebel;
00481:             if (_independentCatalog.Contains(branchId) || branchId.StartsWith("branch_ind_", StringComparison.Ordinal))
00482:                 return FactionBranchKind.Independent;
00483:             return FactionBranchKind.None;
00484:         }
00485:
00486:         public IReadOnlyList<FactionBranchOption> GetBranchOptions(MoralChoiceSystem? moralChoice)
00487:         {
00488:             var list = new List<FactionBranchOption>();
00489:
00490:             // 1. Military branches
00491:             foreach (var b in _militaryCatalog)
00492:             {
00493:                 bool isComm = string.Equals(ActiveBranchId, b.id, StringComparison.Ordinal);
00494:                 string? reason = null;
00495:                 bool avail = moralChoice != null && CanCommit(b.id, moralChoice, out reason);
00496:                 if (moralChoice == null) reason = "no_moral_choice_data";
00497:
00498:                 var opt = new FactionBranchOption
00499:                 {
00500:                     BranchId = b.id,
00501:                     FactionKind = FactionBranchKind.Military,
00502:                     FactionId = MilitaryBranchIds.FactionId,
00503:                     DisplayName = b.display_name,
00504:                     PonrFlag = b.ponr_flag,
00505:                     PonrTrigger = b.ponr_trigger,
00506:                     EntryBandMin = b.entry_band_min,
00507:                     EntryBandMax = b.entry_band_max,
00508:                     IsCommitted = isComm,
00509:                     IsPonrLocked = isComm && IsPonrLocked,
00510:                     IsAvailable = avail,
00511:                     LockoutReason = avail ? null : reason,
00512:                     ConsequencesSummary = "Aligns with Military Command. Locks out Rebel and Independent paths."
00513:                 };
00514:                 foreach (var e in b.endings)
00515:                     opt.PossibleEndings.Add($"{e.display_name} ({e.band_min}..{e.band_max})");
00516:                 list.Add(opt);
00518:
00519:             // 2. Rebel branches
00520:             foreach (var b in _rebelCatalog)
00521:             {
00522:                 bool isComm = string.Equals(ActiveBranchId, b.id, StringComparison.Ordinal);
00523:                 string? reason = null;
00524:                 bool avail = moralChoice != null && CanCommit(b.id, moralChoice, out reason);
00525:                 if (moralChoice == null) reason = "no_moral_choice_data";
00526:
00527:                 var opt = new FactionBranchOption
00528:                 {
00529:                     BranchId = b.id,
00530:                     FactionKind = FactionBranchKind.Rebel,
00531:                     FactionId = RebelBranchIds.FactionId,
00532:                     DisplayName = b.display_name,
00533:                     PonrFlag = b.ponr_flag,
00534:                     PonrTrigger = b.ponr_trigger,
00535:                     EntryBandMin = b.entry_band_min,
00536:                     EntryBandMax = b.entry_band_max,
00537:                     IsCommitted = isComm,
00538:                     IsPonrLocked = isComm && IsPonrLocked,
00539:                     IsAvailable = avail,
00540:                     LockoutReason = avail ? null : reason,
00541:                     ConsequencesSummary = "Aligns with the Wasteland Insurgency. Locks out Military and Independent paths."
00542:                 };
00543:                 foreach (var e in b.endings)
00544:                     opt.PossibleEndings.Add($"{e.display_name} ({e.band_min}..{e.band_max})");
00545:                 list.Add(opt);
00547:
00548:             // 3. Independent branches
00549:             foreach (var b in _independentCatalog)
00550:             {
00551:                 bool isComm = string.Equals(ActiveBranchId, b.id, StringComparison.Ordinal);
00552:                 string? reason = null;
00553:                 bool avail = moralChoice != null && CanCommit(b.id, moralChoice, out reason);
00554:                 if (moralChoice == null) reason = "no_moral_choice_data";
00555:
00556:                 var opt = new FactionBranchOption
00557:                 {
00558:                     BranchId = b.id,
00559:                     FactionKind = FactionBranchKind.Independent,
00560:                     FactionId = IndependentBranchIds.FactionId,
00561:                     DisplayName = b.display_name,
00562:                     PonrFlag = b.ponr_flag,
00563:                     PonrTrigger = b.ponr_trigger,
00564:                     EntryBandMin = b.entry_band_min,
00565:                     EntryBandMax = b.entry_band_max,
00566:                     IsCommitted = isComm,
00567:                     IsPonrLocked = isComm && IsPonrLocked,
00568:                     IsAvailable = avail,
00569:                     LockoutReason = avail ? null : reason,
00570:                     ConsequencesSummary = "Walks an unaligned wasteland path. Locks out Military and Rebel allegiance."
00571:                 };
00572:                 foreach (var e in b.endings)
00573:                     opt.PossibleEndings.Add($"{e.display_name} ({e.band_min}..{e.band_max})");
00574:                 list.Add(opt);
00578:         }
00579:
00580:         public IReadOnlyList<FactionStandingSummary> GetFactionStandingSummaries()
00581:         {
00582:             return new List<FactionStandingSummary>
00583:             {
00584:                 new FactionStandingSummary
00585:                 {
00586:                     FactionId = MilitaryBranchIds.FactionId,
00587:                     DisplayName = "Military Outposts",
00588:                     Standing = Independent.MilitaryStanding,
00589:                     Alignment = Military.MilitaryAlignment,
00590:                     IsHostile = Independent.IsHostileToMilitary,
00591:                     IsAllied = ActiveFactionKind == FactionBranchKind.Military,
00592:                     IsJoined = ActiveFactionKind == FactionBranchKind.Military,
00593:                     IsOpposed = ActiveFactionKind == FactionBranchKind.Rebel || Independent.IsHostileToMilitary
00594:                 },
00595:                 new FactionStandingSummary
00596:                 {
00597:                     FactionId = RebelBranchIds.FactionId,
00598:                     DisplayName = "Rebel Insurgency",
00599:                     Standing = Independent.RebelStanding,
00600:                     Alignment = Rebel.RebelAlignment,
00601:                     IsHostile = Independent.IsHostileToRebel,
00602:                     IsAllied = ActiveFactionKind == FactionBranchKind.Rebel,
00603:                     IsJoined = ActiveFactionKind == FactionBranchKind.Rebel,
00604:                     IsOpposed = ActiveFactionKind == FactionBranchKind.Military || Independent.IsHostileToRebel
00605:                 },
00606:                 new FactionStandingSummary
00607:                 {
00608:                     FactionId = PrpfIds.FactionId,
00609:                     DisplayName = "Peace Protection Forces (PRPF)",
00610:                     Standing = Prpf.Standing,
00611:                     Alignment = Prpf.Alignment,
00612:                     IsHostile = Prpf.IsHostile,
00613:                     IsAllied = Prpf.IsAllied,
00614:                     IsJoined = Prpf.IsJoined,
00615:                     IsOpposed = Prpf.IsOpposed
00616:                 }
00617:             };
00618:         }
00619:
00620:         public WeightOfChoicesSave CaptureState()
00621:         {
00622:             return WeightOfChoicesSaveCodec.Capture(Military, Rebel, Independent, Prpf);
00623:         }
00624:
00625:         public void RestoreState(WeightOfChoicesSave save)
00626:         {
00627:             if (save == null) return;
00628:             WeightOfChoicesSaveCodec.Restore(save, Military, Rebel, Independent, Prpf);
00629:             OnStateChanged?.Invoke();
00630:         }
00631:
00632:         public static FactionBranchCoordinator LoadFromData(
00633:             string dataDir,
00634:             IFileIO fileIO,
00635:             IJsonSerializer json,
00636:             IFlagLedger flags,
00637:             ILog? log = null)
00638:         {
00639:             var milCatalog = MilitaryBranchCatalog.LoadAndRegister(dataDir, fileIO, json);
00640:             var rebCatalog = RebelBranchCatalog.LoadAndRegister(dataDir, fileIO, json);
00641:             var indCatalog = IndependentBranchCatalog.LoadAndRegister(dataDir, fileIO, json);
00642:
00643:             return new FactionBranchCoordinator(
00644:                 milCatalog,
00645:                 rebCatalog,
00646:                 indCatalog,
00647:                 flags,
00648:                 log);
00649:         }
00650:
00651:         private static MoralPathBand ParseBand(string s)
00652:         {
00653:             if (string.IsNullOrEmpty(s)) return MoralPathBand.Neutral;
00654:             return s.ToLowerInvariant() switch
00655:             {
00656:                 "very_evil" => MoralPathBand.VeryEvil,
00657:                 "evil" => MoralPathBand.Evil,
00658:                 "slightly_evil" => MoralPathBand.SlightlyEvil,
00659:                 "neutral" => MoralPathBand.Neutral,
00660:                 "slightly_positive" => MoralPathBand.SlightlyPositive,
00661:                 "positive" => MoralPathBand.Positive,
00662:                 "very_positive" => MoralPathBand.VeryPositive,
00663:                 _ => MoralPathBand.Neutral
00664:             };
00665:         }
00666:     }
00667: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs`

### `Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs` — complete current file

- Size: 188 lines / 9036 bytes.
- SHA-256: `c6a10fc9ec25bcc1b02b9ec098a6cd614d6a4cf60a6fc50fd53809fa574058a4`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003:
00004: namespace Ashfall.Core.Factions
00005: {
00006:     /// <summary>
00007:     /// Combined save envelope for "The Weight of Choices" branching system.
00008:     /// A playthrough commits to at most ONE of Military, Rebel, or
00009:     /// Independent (never more than one — the three branch systems are
00010:     /// mutually exclusive per design), but PRPF standing/alignment is always
00011:     /// present regardless of which base faction the player picked, or even
00012:     /// if they picked none yet. All three per-faction sections are always
00013:     /// written so the envelope shape stays constant; whichever ones the
00014:     /// player never committed to simply stay at their fresh-system defaults
00015:     /// (uncommitted, unlocked) and are harmless dead weight in the file.
00016:     ///
00017:     /// v2 adds the Independent branch section. v1 files (Military + Rebel +
00018:     /// PRPF only) migrate with a fresh, uncommitted Independent section.
00019:     /// </summary>
00020:     [Serializable]
00021:     public class WeightOfChoicesSave
00022:     {
00023:         public const int CurrentSaveVersion = 2;
00024:
00025:         public int saveVersion = CurrentSaveVersion;
00026:         public MilitaryBranchSystemState militaryBranch = new MilitaryBranchSystemState();
00027:         public RebelBranchSystemState rebelBranch = new RebelBranchSystemState();
00028:         public IndependentBranchSystemState independentBranch = new IndependentBranchSystemState();
00029:         public PrpfSystemState prpf = new PrpfSystemState();
00030:
00031:         /// <summary>Integrity hash computed over all payload fields.</summary>
00032:         public string Checksum = string.Empty;
00033:     }
00034:
00035:     /// <summary>
00036:     /// Frozen v1 envelope shape (Military + Rebel + PRPF only, no Independent
00037:     /// section). Kept so a v1 file on disk validates against the field set
00038:     /// it was actually hashed with — SaveChecksum walks public fields, so
00039:     /// validating a v1 payload against the v2 shape would always mismatch.
00040:     /// Do not add fields here.
00041:     /// </summary>
00042:     [Serializable]
00043:     public class WeightOfChoicesSaveV1
00044:     {
00045:         public int saveVersion = 1;
00046:         public MilitaryBranchSystemState militaryBranch = new MilitaryBranchSystemState();
00047:         public RebelBranchSystemState rebelBranch = new RebelBranchSystemState();
00048:         public PrpfSystemState prpf = new PrpfSystemState();
00049:         public string Checksum = string.Empty;
00050:     }
00051:
00052:     /// <summary>
00053:     /// Serialization codec for the combined Military + Rebel + Independent +
00054:     /// PRPF envelope. Follows the same Capture/Restore/Encode/Decode shape as
00055:     /// YearOfAshSaveCodec and the individual per-faction codecs, but composes
00056:     /// all four live systems in one call so a host session only needs to
00057:     /// hold one save section for the whole branching system.
00058:     /// </summary>
00059:     public static class WeightOfChoicesSaveCodec
00060:     {
00061:         public static WeightOfChoicesSave Capture(
00062:             MilitaryBranchSystem militaryBranch,
00063:             RebelBranchSystem rebelBranch,
00064:             IndependentBranchSystem independentBranch,
00065:             PrpfStandingSystem prpf)
00066:         {
00067:             if (militaryBranch == null) throw new ArgumentNullException(nameof(militaryBranch));
00068:             if (rebelBranch == null) throw new ArgumentNullException(nameof(rebelBranch));
00069:             if (independentBranch == null) throw new ArgumentNullException(nameof(independentBranch));
00070:             if (prpf == null) throw new ArgumentNullException(nameof(prpf));
00071:
00072:             var save = new WeightOfChoicesSave
00073:             {
00074:                 militaryBranch = militaryBranch.CaptureState(),
00075:                 rebelBranch = rebelBranch.CaptureState(),
00076:                 independentBranch = independentBranch.CaptureState(),
00077:                 prpf = prpf.CaptureState()
00078:             };
00079:             save.Checksum = SaveChecksum.Compute(save);
00080:             return save;
00081:         }
00082:
00083:         public static void Restore(
00084:             WeightOfChoicesSave save,
00085:             MilitaryBranchSystem militaryBranch,
00086:             RebelBranchSystem rebelBranch,
00087:             IndependentBranchSystem independentBranch,
00088:             PrpfStandingSystem prpf)
00089:         {
00090:             if (save == null) throw new ArgumentNullException(nameof(save));
00091:             if (militaryBranch == null) throw new ArgumentNullException(nameof(militaryBranch));
00092:             if (rebelBranch == null) throw new ArgumentNullException(nameof(rebelBranch));
00093:             if (independentBranch == null) throw new ArgumentNullException(nameof(independentBranch));
00094:             if (prpf == null) throw new ArgumentNullException(nameof(prpf));
00095:
00096:             militaryBranch.RestoreState(save.militaryBranch);
00097:             rebelBranch.RestoreState(save.rebelBranch);
00098:             independentBranch.RestoreState(save.independentBranch);
00099:             prpf.RestoreState(save.prpf);
00100:         }
00101:
00102:         public static string Encode(WeightOfChoicesSave save, IJsonSerializer json)
00103:         {
00104:             if (save == null) throw new ArgumentNullException(nameof(save));
00105:             if (json == null) throw new ArgumentNullException(nameof(json));
00106:             save.Checksum = SaveChecksum.Compute(save);
00107:             return json.Serialize(save);
00108:         }
00109:
00110:         public static WeightOfChoicesSave Decode(string jsonText, IJsonSerializer json)
00111:         {
00112:             if (string.IsNullOrEmpty(jsonText))
00113:                 throw new InvalidOperationException("WeightOfChoicesSave: empty save payload.");
00114:             if (json == null) throw new ArgumentNullException(nameof(json));
00115:
00116:             var save = json.Deserialize<WeightOfChoicesSave>(jsonText);
00117:             if (save == null)
00118:                 throw new InvalidOperationException("WeightOfChoicesSave: deserialization returned null.");
00119:
00120:             if (save.saveVersion > WeightOfChoicesSave.CurrentSaveVersion)
00121:                 throw new InvalidOperationException(
00122:                     $"WeightOfChoicesSave: saveVersion {save.saveVersion} is newer than supported ({WeightOfChoicesSave.CurrentSaveVersion}).");
00123:
00124:             // A v1 file was hashed over the v1 field set. Validate it against the frozen
00125:             // v1 shape and upgrade in place; the Independent section starts fresh.
00126:             if (save.saveVersion < WeightOfChoicesSave.CurrentSaveVersion)
00127:                 return MigrateToCurrent(jsonText, json, save.saveVersion);
00128:
00129:             if (!string.IsNullOrEmpty(save.Checksum))
00130:             {
00131:                 string actual = SaveChecksum.Compute(save);
00132:                 if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
00133:                     throw new InvalidOperationException("WeightOfChoicesSave: checksum mismatch (corrupted or tampered save).");
00134:             }
00135:
00136:             return save;
00137:         }
00138:
00139:         private static WeightOfChoicesSave MigrateToCurrent(string jsonText, IJsonSerializer json, int version)
00140:         {
00141:             if (version == 1)
00142:             {
00143:                 var v1 = json.Deserialize<WeightOfChoicesSaveV1>(jsonText);
00144:                 if (v1 == null)
00145:                     throw new InvalidOperationException("WeightOfChoicesSave: v1 deserialization returned null.");
00146:
00147:                 if (!string.IsNullOrEmpty(v1.Checksum))
00148:                 {
00149:                     string actual = SaveChecksum.Compute(v1);
00150:                     if (!string.Equals(v1.Checksum, actual, StringComparison.Ordinal))
00151:                         throw new InvalidOperationException("WeightOfChoicesSave: checksum mismatch (corrupted or tampered save).");
00152:                 }
00153:
00154:                 var upgraded = new WeightOfChoicesSave
00155:                 {
00156:                     saveVersion = WeightOfChoicesSave.CurrentSaveVersion,
00157:                     militaryBranch = v1.militaryBranch,
00158:                     rebelBranch = v1.rebelBranch,
00159:                     prpf = v1.prpf
00160:                     // independentBranch stays at its field initialiser (fresh, uncommitted).
00161:                 };
00162:                 upgraded.Checksum = SaveChecksum.Compute(upgraded);
00163:                 return upgraded;
00164:             }
00165:
00166:             throw new InvalidOperationException(
00167:                 $"WeightOfChoicesSave: no migration path from saveVersion {version}.");
00168:         }
00169:
00170:         /// <summary>
00171:         /// True if the player has committed to more than one of
00172:         /// Military/Rebel/Independent simultaneously — a data-integrity
00173:         /// invariant violation, since the three branch systems are mutually
00174:         /// exclusive by design. Hosts should check this after Restore and
00175:         /// treat a true result as a corrupt save, not a valid multi-faction
00176:         /// state.
00177:         /// </summary>
00178:         public static bool HasConflictingFactionCommitment(WeightOfChoicesSave save)
00179:         {
00180:             if (save == null) throw new ArgumentNullException(nameof(save));
00181:             int committedCount = 0;
00182:             if (save.militaryBranch?.branch?.committed == true) committedCount++;
00183:             if (save.rebelBranch?.branch?.committed == true) committedCount++;
00184:             if (save.independentBranch?.branch?.committed == true) committedCount++;
00185:             return committedCount > 1;
00186:         }
00187:     }
00188: }
```


# Appendix — Current Source Detail: `src/Host/FactionBranchHostSession.cs`

### `src/Host/FactionBranchHostSession.cs` — complete current file

- Size: 55 lines / 1731 bytes.
- SHA-256: `9a8e5b20ec4116cb7037ca4d02dbf3fa07eb2877472ca4ba60a2e4af05f54109`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Ashfall.Core;
00004: using Ashfall.Core.Factions;
00005: using Ashfall.Core.Flags;
00006: using Godot;
00007:
00008: namespace AtomicWar.GodotApp
00009: {
00010:     /// <summary>
00011:     /// Host session for FactionBranchCoordinator ("The Weight of Choices").
00012:     /// Thin Godot host layer exposing coordinator actions to Factions and Quests panels.
00013:     /// </summary>
00014:     public sealed class FactionBranchHostSession : HostSessionBase
00015:     {
00016:         public FactionBranchCoordinator Coordinator { get; }
00017:
00018:         public FactionBranchHostSession(FactionBranchCoordinator coordinator)
00019:         {
00020:             Coordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
00021:             Coordinator.OnStateChanged += () => RaiseStateChanged();
00022:         }
00023:
00024:         public static FactionBranchHostSession CreateDefault(string dataDir, IFlagLedger? flags = null)
00025:         {
00026:             var coordinator = FactionBranchCoordinator.LoadFromData(
00027:                 dataDir,
00028:                 new FileSystemIO(),
00029:                 new SystemTextJsonSerializer(),
00030:                 flags ?? new CampaignConsequenceLedger(),
00031:                 new GodotLog());
00032:             return new FactionBranchHostSession(coordinator);
00033:         }
00034:
00035:         public bool TrySave()
00036:         {
00037:             return WeightOfChoicesSaveStore.TrySave(Coordinator.CaptureState());
00038:         }
00039:
00040:         public bool TryLoad()
00041:         {
00042:             var loaded = WeightOfChoicesSaveStore.TryLoad();
00043:             if (loaded == null) return false;
00044:             Coordinator.RestoreState(loaded);
00045:             return true;
00046:         }
00047:
00048:         public override void Save()
00049:         {
00050:             if (!IsDirty) return;
00051:             TrySave();
00052:             base.Save();
00053:         }
00054:     }
00055: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/RebelBranchSystemTests.cs`

### `Ashfall.Core.Tests/RebelBranchSystemTests.cs` — complete current file

- Size: 274 lines / 10719 bytes.
- SHA-256: `350f20fdea0204230dfd9b998b66bdf520e58197b04c88da4b0fe7b8710c4910`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Ashfall.Core;
00004: using Ashfall.Core.Factions;
00005: using Ashfall.Core.Flags;
00006: using Ashfall.Core.MoralChoice;
00007: using Xunit;
00008:
00009: namespace Ashfall.Core.Tests
00010: {
00011:     public class RebelBranchSystemTests
00012:     {
00013:         private static RebelBranchCatalog LoadCatalog()
00014:         {
00015:             string start = System.IO.Directory.GetCurrentDirectory();
00016:             string dir;
00017:             if (!CatalogLocator.TryFindDataDirectory(start, out dir))
00018:                 CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir);
00019:             Assert.False(string.IsNullOrEmpty(dir), "StreamingAssets/Data must be findable from the test run");
00020:             return RebelBranchCatalog.LoadAndRegister(dir, new FileSystemIO(), new SystemTextJsonSerializer());
00021:         }
00022:
00023:         private static MoralChoiceSystem MakeMoralChoice(int seed = 1) =>
00024:             new MoralChoiceSystem(new StubRng(seed));
00025:
00026:         [Fact]
00027:         public void Catalog_LoadsAllFifteenBranchesWithThreeEndingsEach()
00028:         {
00029:             var catalog = LoadCatalog();
00030:             Assert.Equal(RebelBranchIds.BranchCount, catalog.Count);
00031:             foreach (var branchId in RebelBranchIds.AllBranches)
00032:             {
00033:                 var entry = catalog.GetById(branchId);
00034:                 Assert.NotNull(entry);
00035:                 Assert.Equal(3, entry!.endings.Count);
00036:             }
00037:         }
00038:
00039:         [Fact]
00040:         public void CommitBranch_WithinEntryBand_Succeeds()
00041:         {
00042:             var catalog = LoadCatalog();
00043:             var flags = new InMemoryFlagLedger();
00044:             var system = new RebelBranchSystem(catalog, flags);
00045:             var moral = MakeMoralChoice(); // starts at Neutral band
00046:
00047:             string committed = system.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
00048:
00049:             Assert.Equal(RebelBranchIds.BranchTrueRebel, committed);
00050:             Assert.Equal(RebelBranchIds.BranchTrueRebel, system.CommittedBranchId);
00051:         }
00052:
00053:         [Fact]
00054:         public void CommitBranch_OutsideEntryBand_Throws()
00055:         {
00056:             var catalog = LoadCatalog();
00057:             var flags = new InMemoryFlagLedger();
00058:             var system = new RebelBranchSystem(catalog, flags);
00059:             var moral = MakeMoralChoice();
00060:
00061:             // REB-4 Martyr requires a positive-or-better starting band; a fresh
00062:             // MoralChoiceSystem starts at Neutral, which is outside that range.
00063:             Assert.Throws<InvalidOperationException>(() =>
00064:                 system.CommitBranch(RebelBranchIds.BranchMartyr, moral));
00065:         }
00066:
00067:         [Fact]
00068:         public void CommitBranch_CalledTwice_KeepsFirstCommitment()
00069:         {
00070:             var catalog = LoadCatalog();
00071:             var flags = new InMemoryFlagLedger();
00072:             var system = new RebelBranchSystem(catalog, flags);
00073:             var moral = MakeMoralChoice();
00074:
00075:             system.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
00076:             string second = system.CommitBranch(RebelBranchIds.BranchLoneWolf, moral);
00077:
00078:             Assert.Equal(RebelBranchIds.BranchTrueRebel, second);
00079:             Assert.Equal(RebelBranchIds.BranchTrueRebel, system.CommittedBranchId);
00080:         }
00081:
00082:         [Fact]
00083:         public void LockPointOfNoReturn_BeforeCommit_Throws()
00084:         {
00085:             var catalog = LoadCatalog();
00086:             var flags = new InMemoryFlagLedger();
00087:             var system = new RebelBranchSystem(catalog, flags);
00088:
00089:             Assert.Throws<InvalidOperationException>(() => system.LockPointOfNoReturn());
00090:         }
00091:
00092:         [Fact]
00093:         public void LockPointOfNoReturn_SetsDurableAndRuntimeFlag()
00094:         {
00095:             var catalog = LoadCatalog();
00096:             var flags = new InMemoryFlagLedger();
00097:             var system = new RebelBranchSystem(catalog, flags);
00098:             var moral = MakeMoralChoice();
00099:
00100:             system.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
00101:             system.AdvanceDay(55);
00102:             system.LockPointOfNoReturn();
00103:
00104:             Assert.True(system.IsPonrLocked);
00105:             Assert.True(flags.IsSet(RebelBranchIds.FlagPonrTrueRebel));
00106:             Assert.Contains(RebelBranchIds.FlagPonrTrueRebel, system.State.setFlags);
00107:             Assert.Equal(55, system.State.branch.ponrLockedDay);
00108:         }
00109:
00110:         [Fact]
00111:         public void LockPointOfNoReturn_CalledTwice_IsANoOp()
00112:         {
00113:             var catalog = LoadCatalog();
00114:             var flags = new InMemoryFlagLedger();
00115:             var system = new RebelBranchSystem(catalog, flags);
00116:             var moral = MakeMoralChoice();
00117:
00118:             system.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
00119:             system.AdvanceDay(10);
00120:             system.LockPointOfNoReturn();
00121:             system.AdvanceDay(20);
00122:             system.LockPointOfNoReturn();
00123:
00124:             Assert.Equal(10, system.State.branch.ponrLockedDay);
00125:         }
00126:
00127:         [Fact]
00128:         public void ResolveEnding_BeforePonrLocked_Throws()
00129:         {
00130:             var catalog = LoadCatalog();
00131:             var flags = new InMemoryFlagLedger();
00132:             var system = new RebelBranchSystem(catalog, flags);
00133:             var moral = MakeMoralChoice();
00134:
00135:             system.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
00136:
00137:             Assert.Throws<InvalidOperationException>(() => system.ResolveEnding(moral));
00138:         }
00139:
00140:         [Fact]
00141:         public void ResolveEnding_NeutralBand_ResolvesSurvivorEnding()
00142:         {
00143:             var catalog = LoadCatalog();
00144:             var flags = new InMemoryFlagLedger();
00145:             var system = new RebelBranchSystem(catalog, flags);
00146:             var moral = MakeMoralChoice(); // Neutral band
00147:
00148:             system.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
00149:             system.LockPointOfNoReturn();
00150:             string ending = system.ResolveEnding(moral);
00151:
00152:             Assert.Equal(RebelBranchIds.EndingTrueRebelC, ending);
00153:             Assert.Equal(ending, system.ResolvedEndingId);
00154:         }
00155:
00156:         [Fact]
00157:         public void ResolveEnding_IsIdempotent_EvenIfMoralityDriftsAfterward()
00158:         {
00159:             var catalog = LoadCatalog();
00160:             var flags = new InMemoryFlagLedger();
00161:             var system = new RebelBranchSystem(catalog, flags);
00162:             var moral = MakeMoralChoice();
00163:
00164:             system.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
00165:             system.LockPointOfNoReturn();
00166:             string first = system.ResolveEnding(moral);
00167:
00168:             var quest = new MoralChoiceQuestDefinition
00169:             {
00170:                 Id = "quest_moral_share_child",
00171:                 Choices = { new MoralChoiceOption { MoralDelta = 150, EmpathyDelta = 1 } }
00172:             };
00173:             moral.Resolve(quest, 0, "loc_test", 1);
00174:
00175:             string second = system.ResolveEnding(moral);
00176:             Assert.Equal(first, second);
00177:         }
00178:
00179:         [Fact]
00180:         public void ShiftFactionAlignment_ClampsToRange()
00181:         {
00182:             var catalog = LoadCatalog();
00183:             var flags = new InMemoryFlagLedger();
00184:             var system = new RebelBranchSystem(catalog, flags);
00185:
00186:             system.ShiftFactionAlignment(-500);
00187:             Assert.Equal(RebelBranchSystem.MinAlignment, system.RebelAlignment);
00188:
00189:             system.ShiftFactionAlignment(1000);
00190:             Assert.Equal(RebelBranchSystem.MaxAlignment, system.RebelAlignment);
00191:         }
00192:
00193:         [Fact]
00194:         public void IsGameOver_ZeroOrNegativeSurvivors_IsTrue()
00195:         {
00196:             Assert.True(RebelBranchSystem.IsGameOver(0));
00197:             Assert.True(RebelBranchSystem.IsGameOver(-1));
00198:             Assert.False(RebelBranchSystem.IsGameOver(1));
00199:         }
00200:
00201:         [Fact]
00202:         public void SaveRoundTrip_PreservesBranchTimelineAlignmentAndFlags()
00203:         {
00204:             var catalog = LoadCatalog();
00205:             var flagsA = new InMemoryFlagLedger();
00206:             var systemA = new RebelBranchSystem(catalog, flagsA);
00207:             var moral = MakeMoralChoice();
00208:
00209:             systemA.AdvanceDay(25);
00210:             systemA.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
00211:             systemA.AdvanceDay(55);
00212:             systemA.LockPointOfNoReturn();
00213:             systemA.ShiftFactionAlignment(30);
00214:             systemA.ResolveEnding(moral);
00215:
00216:             var save = RebelBranchSaveCodec.Capture(systemA);
00217:             var jsonSerializer = new SystemTextJsonSerializer();
00218:             string jsonText = RebelBranchSaveCodec.Encode(save, jsonSerializer);
00219:             Assert.Contains(RebelBranchIds.BranchTrueRebel, jsonText);
00220:
00221:             var loaded = RebelBranchSaveCodec.Decode(jsonText, jsonSerializer);
00222:
00223:             var flagsB = new InMemoryFlagLedger();
00224:             var systemB = new RebelBranchSystem(catalog, flagsB);
00225:             RebelBranchSaveCodec.Restore(loaded, systemB);
00226:
00227:             Assert.Equal(55, systemB.CurrentDay);
00228:             Assert.Equal(RebelBranchIds.BranchTrueRebel, systemB.CommittedBranchId);
00229:             Assert.True(systemB.IsPonrLocked);
00230:             Assert.Equal(-50, systemB.RebelAlignment); // -80 default + 30 shift
00231:             Assert.Equal(systemA.ResolvedEndingId, systemB.ResolvedEndingId);
00232:             Assert.True(flagsB.IsSet(RebelBranchIds.FlagPonrTrueRebel));
00233:         }
00234:
00235:         [Fact]
00236:         public void Decode_TamperedChecksum_Throws()
00237:         {
00238:             var catalog = LoadCatalog();
00239:             var flags = new InMemoryFlagLedger();
00240:             var system = new RebelBranchSystem(catalog, flags);
00241:             var moral = MakeMoralChoice();
00242:             system.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
00243:
00244:             var save = RebelBranchSaveCodec.Capture(system);
00245:             var jsonSerializer = new SystemTextJsonSerializer();
00246:             string jsonText = RebelBranchSaveCodec.Encode(save, jsonSerializer);
00247:
00248:             string tampered = jsonText.Replace(RebelBranchIds.BranchTrueRebel, RebelBranchIds.BranchLoneWolf);
00249:
00250:             Assert.Throws<InvalidOperationException>(() => RebelBranchSaveCodec.Decode(tampered, jsonSerializer));
00251:         }
00252:
00253:         [Fact]
00254:         public void Decode_NewerSaveVersion_Throws()
00255:         {
00256:             var save = new RebelBranchSave { saveVersion = 99 };
00257:             var jsonSerializer = new SystemTextJsonSerializer();
00258:             string jsonText = RebelBranchSaveCodec.Encode(save, jsonSerializer);
00259:
00260:             Assert.Throws<InvalidOperationException>(() => RebelBranchSaveCodec.Decode(jsonText, jsonSerializer));
00261:         }
00262:
00263:         [Fact]
00264:         public void RestoreState_WrongSystemId_Throws()
00265:         {
00266:             var catalog = LoadCatalog();
00267:             var flags = new InMemoryFlagLedger();
00268:             var system = new RebelBranchSystem(catalog, flags);
00269:             var badState = new RebelBranchSystemState { systemId = "not_the_right_system" };
00270:
00271:             Assert.Throws<ArgumentException>(() => system.RestoreState(badState));
00272:         }
00273:     }
00274: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Factions/RebelBranchState.cs`

### `Assets/Ashfall.Core/Factions/RebelBranchState.cs` — complete current file

- Size: 62 lines / 2402 bytes.
- SHA-256: `9d936759dd90803d61fa300cd1830b17b2582aeee13b3604dfa769a68d848b8e`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: namespace Ashfall.Core.Factions
00007: {
00008:     /// <summary>
00009:     /// Self-contained day counter for the Rebel branch system's own clock.
00010:     /// Mirrors MilitaryBranchTimelineState: independent of the global IClock
00011:     /// and of Year of Ash's 180-360 day timeline. Note this is a SEPARATE
00012:     /// counter from MilitaryBranchTimelineState — a player can only ever be
00013:     /// on one of Military/Rebel in a given playthrough, but the two systems
00014:     /// do not share a clock instance, so each stays self-contained and
00015:     /// trivially composable if a future host session wires both.
00016:     /// </summary>
00017:     [Serializable]
00018:     public class RebelBranchTimelineState
00019:     {
00020:         public int currentDay = 0;
00021:     }
00022:
00023:     /// <summary>
00024:     /// Which base Rebel branch (if any) the player has committed to, and
00025:     /// whether its point-of-no-return has fired. Mirrors
00026:     /// MilitaryBranchRecord exactly.
00027:     /// </summary>
00028:     [Serializable]
00029:     public class RebelBranchRecord
00030:     {
00031:         public string branchId = string.Empty;
00032:         public bool committed = false;
00033:         public bool ponrLocked = false;
00034:         public int ponrLockedDay = -1;
00035:         public string resolvedEndingId = string.Empty;
00036:     }
00037:
00038:     [Serializable]
00039:     public class RebelBranchSystemState
00040:     {
00041:         public string systemId = RebelBranchSystem.SystemId;
00042:         public int schemaVersion = 1;
00043:
00044:         public RebelBranchTimelineState timeline = new RebelBranchTimelineState();
00045:         public RebelBranchRecord branch = new RebelBranchRecord();
00046:
00047:         /// <summary>Rebel's own internal alignment, distinct from the player's
00048:         /// MoralChoiceSystem score and from FactionWarSystem.standing (player
00049:         /// relationship, not faction-internal morality). Rebels start
00050:         /// evil-leaning per design, same as Military, and are equally
00051:         /// swayable by the player.</summary>
00052:         public FactionAlignmentRecord rebelAlignment = new FactionAlignmentRecord
00053:         {
00054:             factionId = RebelBranchIds.FactionId,
00055:             alignment = -80
00056:         };
00057:
00058:         /// <summary>Flags set by branch/PoNR events. Distinct from the runtime IFlagLedger
00059:         /// (which is not persisted) — this list is the save-durable record.</summary>
00060:         public List<string> setFlags = new List<string>();
00061:     }
00062: }
```


# Appendix — Current Source Detail: `src/Host/WeightOfChoicesSaveStore.cs`

### `src/Host/WeightOfChoicesSaveStore.cs` — complete current file

- Size: 53 lines / 2589 bytes.
- SHA-256: `6cad240dee44677d33e929c004068fbe24e426a9b088ab5554f9183f29b5e108`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // Save Store : WeightOfChoicesSaveStore
00004: // Core State : Ashfall.Core.Factions.WeightOfChoicesSave
00005: // Host Caller: Main.FactionBranch / FactionBranchHostSession
00006: // Purpose    : Unified faction branching progression, alignment & PRPF standing
00007: // ============================================================================
00008: using Ashfall.Core.Factions;
00009: using Ashfall.Core.Save;
00010:
00011: namespace AtomicWar.GodotApp
00012: {
00013:     /// <summary>
00014:     /// Weight of Choices faction branch save persistence — thin façade over the Core
00015:     /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Composes all four
00016:     /// constituent faction branch subsystems (Military, Rebel, Independent, PRPF)
00017:     /// into a single atomic versioned envelope.
00018:     /// </summary>
00019:     public static class WeightOfChoicesSaveStore
00020:     {
00021:         public const string FileName = "weight_of_choices_save.json";
00022:         public const string SectionName = "weight_of_choices";
00023:
00024:         private static readonly SaveStore<WeightOfChoicesSave> s_store = SaveStoreHub.FromCodec(
00025:             FileName,
00026:             nameof(WeightOfChoicesSaveStore),
00027:             WeightOfChoicesSaveCodec.Encode,
00028:             WeightOfChoicesSaveCodec.Decode);
00029:
00030:         public static string SavePath => s_store.SavePath;
00031:
00032:         public static bool Exists => s_store.Exists();
00033:
00034:         /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
00035:         public static string TryCaptureDirect(WeightOfChoicesSave state) => s_store.CaptureBare(state);
00036:
00037:         /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
00038:         public static WeightOfChoicesSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
00039:
00040:         /// <summary>Capture state to JSON without writing to disk.</summary>
00041:         public static string TryCapture(WeightOfChoicesSave state) => s_store.CaptureBare(state);
00042:
00043:         /// <summary>Restore state from JSON without reading from disk.</summary>
00044:         public static WeightOfChoicesSave? TryRestore(string json) => s_store.RestoreBare(json);
00045:
00046:         public static bool TrySave(WeightOfChoicesSave state) => s_store.TrySave(state);
00047:
00048:         public static WeightOfChoicesSave? TryLoad() => s_store.TryLoad();
00049:
00050:         /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
00051:         public static string TryCapturePersisted(WeightOfChoicesSave state) => s_store.CapturePersisted(state);
00052:     }
00053: }
```


# Appendix — Current Source Detail: `src/Main.FactionBranch.cs`

### `src/Main.FactionBranch.cs` — complete current file

- Size: 89 lines / 3167 bytes.
- SHA-256: `e575193d6eeab9f4cc0e594625b8fc59c9d731c06f5e50a7a92c75147cdc0da1`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Ashfall.Core;
00004: using Ashfall.Core.Factions;
00005: using Godot;
00006:
00007: namespace AtomicWar.GodotApp
00008: {
00009:     public partial class Main : Control
00010:     {
00011:         private FactionBranchHostSession _factionBranch = null!;
00012:         private bool _factionBranchDirty;
00013:         private CounterIntelligenceHostSession _counterIntelligence = null!;
00014:         private bool _counterIntelligenceDirty;
00015:
00016:         private void SetupFactionBranch()
00017:         {
00018:             if (_factionBranch != null) return;
00019:
00020:             // Plan 26A — resolve through the one data-path authority.
00021:             string dataDir = CatalogPath.ResolveDataDir();
00022:
00023:             _factionBranch = FactionBranchHostSession.CreateDefault(dataDir, flags: _consequenceLedger);
00024:             _factionBranch.StateChanged += () => _factionBranchDirty = true;
00025:
00026:             if (_factionBranch.TryLoad())
00027:             {
00028:                 _factionBranchDirty = false;
00029:             }
00030:         }
00031:
00032:         private void SaveFactionBranch()
00033:         {
00034:             if (_factionBranch != null)
00035:             {
00036:                 if (CaptureSection("weight_of_choices", WeightOfChoicesSaveStore.TryCapturePersisted(_factionBranch.Coordinator.CaptureState())))
00037:                     _factionBranchDirty = false;
00038:             }
00039:         }
00040:
00041:         private void SetupCounterIntelligence()
00042:         {
00043:             if (_counterIntelligence != null) return;
00044:             var state = CounterIntelligenceSaveStore.TryLoad() ?? new CounterIntelligenceState();
00045:             var system = new CounterIntelligenceSystem(state, new SeededRng(2001), new GodotLog());
00046:             _counterIntelligence = new CounterIntelligenceHostSession(system);
00047:             _counterIntelligence.LoadCatalog(_dataDir);
00048:             _counterIntelligence.StateChanged += () => _counterIntelligenceDirty = true;
00049:         }
00050:
00051:         private void SaveCounterIntelligence()
00052:         {
00053:             if (_counterIntelligence != null)
00054:             {
00055:                 if (CaptureSection("counter_intelligence", CounterIntelligenceSaveStore.TryCapturePersisted(_counterIntelligence.System.CaptureState())))
00056:                     _counterIntelligenceDirty = false;
00057:             }
00058:         }
00059:
00060:         private void FlushFactionBranch()
00061:         {
00062:             if (_factionBranchDirty)
00063:                 SaveFactionBranch();
00064:         }
00065:
00066:         public bool CommitFactionBranch(string branchId)
00067:         {
00068:             SetupFactionBranch();
00069:             SetupMoralChoice();
00070:             if (_factionBranch == null || _moralChoice == null) return false;
00071:             var result = _factionBranch.Coordinator.CommitBranch(branchId, _moralChoice);
00072:             if (result.IsSuccess)
00073:             {
00074:                 _factionBranchDirty = true;
00075:                 _moralChoiceDirty = true;
00076:                 AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.UiConfirm);
00077:                 return true;
00078:             }
00079:             return false;
00080:         }
00081:
00082:         private void TickFactionBranchDay(int day)
00083:         {
00084:             SetupFactionBranch();
00085:             _factionBranch?.Coordinator.TickDay(day);
00086:             _factionBranchDirty = true;
00087:         }
00088:     }
00089: }
```


# Appendix — Current Source Detail: `src/UI/FactionsPanel.cs`

### `src/UI/FactionsPanel.cs` — bounded current excerpt (521 of 536 lines)

- Size: 536 lines / 30554 bytes.
- SHA-256: `a129850bff990e300e3f80308d74c8ecbd029463eda3417438045b1a6892986d`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005: using Godot;
00006: using Ashfall.Core;
00007: using Ashfall.Core.UI;
00008: using AtomicWar.GodotApp.YearOfAsh;
00009:
00010: namespace AtomicWar.GodotApp.UI
00011: {
00012:     /// <summary>
00013:     /// ASHFALL — Factions & Diplomacy panel.
00014:     /// Manages wasteland faction relations, trust metrics, trade privileges,
00015:     /// Scavenger Guild claims, Crossing arbitration, and diplomatic communiques.
00016:     /// </summary>
00017:     public partial class FactionsPanel : Control, IBindablePanel
00018:     {
00019:         public event Action? OnClose;
00020:         public event Action<string>? OnFactionDetailRequested;
00021:         public event Action? OnMusterPanelRequested;
00022:         public event Action? OnFoundryPanelRequested;
00023:         public event Action? OnCultureCodexRequested;
00024:         /// <summary>Player chose to pay the warlord tribute in full (amount = current ask).</summary>
00025:         public event Action<int>? OnWarlordTributePay;
00026:         /// <summary>Player refused the warlord tribute this week.</summary>
00027:         public event Action? OnWarlordTributeRefuse;
00028:         /// <summary>Player committed allegiance to a specific faction branch.</summary>
00029:         public event Action<string>? OnCommitBranchRequested;
00030:
00031:         private VBoxContainer _overviewContainer = null!;
00032:         private VBoxContainer _factionsContainer = null!;
00033:         private VBoxContainer _relationsContainer = null!;
00034:         private VBoxContainer _eventsContainer = null!;
00035:         private Label _statusSummary = null!;
00036:
00037:         private HoldfastFactionsCatalog? _factions;
00038:         private HoldfastTradeSession? _trade;
00039:         private MusterHostSession? _muster;
00040:         private ExpansionHostSession? _expansions;
00041:         private YearOfAshHostSession? _yearOfAsh;
00042:         private Ashfall.Core.Factions.FactionBranchCoordinator? _branchCoordinator;
00043:         private Ashfall.Core.MoralChoice.MoralChoiceSystem? _moralChoice;
00044:
00045:         public bool IsBound => _factions != null || _muster != null || _expansions != null || _branchCoordinator != null;
00046:
00047:         /// <summary>True after RefreshView when the Silent Foundry Guild card rendered.</summary>
00048:         public bool HasGuildCard { get; private set; }
00049:
00050:         /// <summary>Last authored collector line shown in the warlord card (presentation-local).</summary>
00051:         private string _collectorNote = string.Empty;
00052:
00053:         public void Bind(
00054:             HoldfastFactionsCatalog? factions,
00055:             HoldfastTradeSession? trade = null,
00056:             MusterHostSession? muster = null,
00057:             ExpansionHostSession? expansions = null,
00058:             YearOfAshHostSession? yearOfAsh = null,
00059:             Ashfall.Core.Factions.FactionBranchCoordinator? branchCoordinator = null,
00060:             Ashfall.Core.MoralChoice.MoralChoiceSystem? moralChoice = null)
00061:         {
00062:             _factions = factions;
00063:             _trade = trade;
00064:             _muster = muster;
00065:             _expansions = expansions;
00066:             _yearOfAsh = yearOfAsh;
00067:             _branchCoordinator = branchCoordinator;
00068:             _moralChoice = moralChoice;
00069:
00070:             if (_muster != null)
00071:                 _muster.StateChanged += RefreshView;
00072:             if (_expansions != null)
00073:                 _expansions.StateChanged += RefreshView;
00074:             if (_branchCoordinator != null)
00075:                 _branchCoordinator.OnStateChanged += RefreshView;
00076:             if (_yearOfAsh?.Warlord != null)
00077:             {
00078:                 _yearOfAsh.Warlord.OnStateChanged += RefreshView;
00079:                 _yearOfAsh.Warlord.OnTributeSettled += (paidFull, day) =>
00080:                     _collectorNote = _yearOfAsh.CollectorLine(paidFull ? "paid" : "short", day);
00081:                 _yearOfAsh.Warlord.OnTributeDemanded += (_, _, day) =>
00082:                     _collectorNote = _yearOfAsh.CollectorLine("demand", day);
00083:             }
00084:
00085:             RefreshView();
00086:         }
00087:
00088:         public void RefreshView()
00089:         {
00090:             if (_overviewContainer == null || _factionsContainer == null ||
00091:                 _relationsContainer == null || _eventsContainer == null)
00092:                 return;
00093:
00094:             AshfallUiHelpers.EmptyChildren(_overviewContainer);
00095:             AshfallUiHelpers.EmptyChildren(_factionsContainer);
00096:             AshfallUiHelpers.EmptyChildren(_relationsContainer);
00097:             AshfallUiHelpers.EmptyChildren(_eventsContainer);
00098:
00099:             // ── 1. Diplomatic Summary ──
00100:             int totalFactions = _factions?.Count ?? 5;
00101:             float guildTrust = _muster?.ScavengerGuild?.Trust ?? 50.0f;
00102:             int guildClaims = _muster?.ScavengerGuild?.State?.claimedSiteIds?.Count ?? 0;
00103:             int blacklistedCount = _muster?.ScavengerGuild?.State?.blacklistedShelterIds?.Count ?? 0;
00104:
00105:             var ovCard = AshfallUiHelpers.MakeCardFrame("WASTELAND DIPLOMATIC & TRADE NETWORK", "REGISTRY STATUS");
00106:             var ovBox = ovCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00107:
00108:             ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Known Major Factions", $"{totalFactions} Sovereign Organizations", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
00109:             ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Scavenger Guild Trust", $"{guildTrust:F1} / 100", AshfallUiHelpers.ToColor(guildTrust >= 50 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Critical)));
00110:             ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Guild Claimed Sites", $"{guildClaims} Active Mining / Scrap Claims", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
00111:             ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Sanctions & Blacklists", blacklistedCount > 0 ? $"{blacklistedCount} Active Hostile Enforcements" : "Zero Sanctions Imposed", AshfallUiHelpers.ToColor(blacklistedCount > 0 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Pale)));
00112:
00113:             if (_muster != null)
00114:             {
00115:                 var btnMuster = AshfallUiHelpers.MakeButton("OPEN SECTOR MUSTER // CURRENTS & ESCALATION", () =>
00116:                 {
00117:                     OnMusterPanelRequested?.Invoke();
00118:                 });
00119:                 ovBox.AddChild(btnMuster);
00120:             }
00121:
00122:             var btnCulture = AshfallUiHelpers.MakeButton("OPEN FACTION CULTURE CODEX // EVERYDAY CUSTOMS", () =>
00123:             {
00124:                 OnCultureCodexRequested?.Invoke();
00125:             });
00126:             ovBox.AddChild(btnCulture);
00127:
00128:             _overviewContainer.AddChild(ovCard);
00129:
00130:             // ── 2. Known Factions List ──
00131:             var factionEntries = new List<HoldfastFactionEntry>();
00132:             if (_factions != null && _factions.Count > 0)
00133:             {
00134:                 foreach (var f in _factions)
00135:                 {
00136:                     if (f != null && !string.IsNullOrEmpty(f.Id))
00137:                         factionEntries.Add(f);
00138:                 }
00139:             }
00140:
00141:             // If empty, supply canonical core factions
00142:             if (factionEntries.Count == 0)
00143:             {
00144:                 factionEntries.Add(new HoldfastFactionEntry(
00145:                     "faction_black_flotilla", "The Black Flotilla", "Maritime Traders / Neutral", "The Flooded Coast",
00146:                     true, 45f, new[] { "clean_water", "medicine", "electronics" }, new[] { "fuel", "fish_rations", "filter_spares" },
00147:                     "\"The sea did not burn. It only poisoned. We sail what remains.\"", "Open Water Barter Agreement"));
00148:
00149:                 factionEntries.Add(new HoldfastFactionEntry(
00150:                     "faction_scavenger_guild", "The Scavenger Guild", "Industrial Scrappers / Pragmatic", "loc_scavenger_guildhall",
00151:                     true, guildTrust, new[] { "dosimeters", "scrap_metal", "tools" }, new[] { "mechanical_parts", "lead_sheeting" },
00152:                     "\"Every ruin has an owner. Violate the two-color ledger at your peril.\"", "Brannick Sten's Claim Accord"));
00153:
00154:                 factionEntries.Add(new HoldfastFactionEntry(
00155:                     "faction_ledger_keepers", "The Ledger Keepers", "Archivists & Chroniclers / Neutral", "The High Vaults",
00156:                     true, 60f, new[] { "cassette_tapes", "books", "schematics" }, new[] { "purified_water", "anti_rad_pills" },
00157:                     "\"The war took the cities. We will not let it take the memory.\"", "Mutual Archival Exchange"));
00158:
00159:                 factionEntries.Add(new HoldfastFactionEntry(
00160:                     "faction_iron_covenant", "The Iron Covenant", "Militant Enclave / Wary", "Sector 01 Outpost",
00161:                     true, 30f, new[] { "ammunition", "armor_plates", "fuel" }, new[] { "weapons", "reinforced_concrete" },
00162:                     "\"Order is forged under pressure. Civilians stay outside the gate.\"", "Armistice Checkpoint"));
00163:
00164:                 factionEntries.Add(new HoldfastFactionEntry(
00165:                     "faction_green_thread", "The Green Thread", "Agrarian Collectivists / Cautious Allies", "The Allotments",
00166:                     true, 55f, new[] { "seeds", "potassium_iodide", "fertilizer" }, new[] { "fresh_produce", "herbal_poultices" },
00167:                     "\"The soil will breathe again if we shield the roots from fallout.\"", "Seed Sharing Protocol"));
00168:             }
00169:
00170:             foreach (var f in factionEntries)
00171:             {
00172:                 var card = AshfallUiHelpers.MakeCardFrame(f.DisplayName, f.Alignment.ToUpperInvariant());
00173:                 var cardBox = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00174:
00175:                 var headerRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
00176:                 var emblem = AshfallUiHelpers.MakeFactionEmblem(f.Id, 44);
00177:                 headerRow.AddChild(emblem);
00178:
00179:                 var quoteBox = AshfallUiHelpers.MakeVBox(2);
00180:                 quoteBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00181:                 var quoteLbl = AshfallUiHelpers.MakeSmall(f.SignatureQuote);
00182:                 quoteLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
00183:                 quoteBox.AddChild(quoteLbl);
00184:
00185:                 var regionLbl = AshfallUiHelpers.MakeLabel($"Base: {f.HomeRegion} · Stance: {f.AccessRule}", Ashfall.Core.UI.Theme.FontSizeLabel, Ashfall.Core.UI.Theme.Muted);
00186:                 quoteBox.AddChild(regionLbl);
00187:                 headerRow.AddChild(quoteBox);
00188:                 cardBox.AddChild(headerRow);
00189:
00190:                 cardBox.AddChild(AshfallUiHelpers.MakeSeparator());
00191:
00192:                 // Trade profile
00193:                 string wantsText = f.Wants != null && f.Wants.Length > 0 ? string.Join(", ", f.Wants) : "None registered";
00194:                 string offersText = f.Offers != null && f.Offers.Length > 0 ? string.Join(", ", f.Offers) : "None registered";
00195:
00196:                 cardBox.AddChild(AshfallUiHelpers.MakeDataRow("Demand (Wants)", wantsText.Replace('_', ' '), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
00197:                 cardBox.AddChild(AshfallUiHelpers.MakeDataRow("Supply (Offers)", offersText.Replace('_', ' '), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
00198:                 cardBox.AddChild(AshfallUiHelpers.MakeDataRow("Standing / Trust", $"{f.Trust:F1} / 100", AshfallUiHelpers.ToColor(f.Trust >= 50 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim)));
00199:
00200:                 var btnRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
00201:                 string factionId = f.Id;
00202:                 var btnInspect = AshfallUiHelpers.MakeButton($"DIPLOMATIC DOSSIER // [{f.DisplayName}]", () =>
00203:                 {
00204:                     OnFactionDetailRequested?.Invoke(factionId);
00205:                 });
00206:                 btnInspect.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00207:                 btnRow.AddChild(btnInspect);
00208:                 cardBox.AddChild(btnRow);
00209:
00210:                 _factionsContainer.AddChild(card);
00211:             }
00212:
00213:             // ── 2b. Treaty Systems — The Silent Foundry Guild (Exp 10) ──
00214:             var foundrySys = _expansions?.SilentFoundry;
00215:             var foundryFaction = _expansions?.FoundryData?.Faction;
00216:             HasGuildCard = foundrySys != null && foundryFaction != null;
00217:             if (foundrySys != null && foundryFaction != null)
00218:             {
00219:                 var guildCard = AshfallUiHelpers.MakeCardFrame(
00220:                     foundryFaction.display_name, "ACCORD SYSTEMS // THE WORKS");
00221:                 var guildBox = guildCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00222:
00223:                 var guildHeader = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
00224:                 var guildEmblem = AshfallUiHelpers.MakeFactionEmblem(foundryFaction.faction_id, 44);
00225:                 guildHeader.AddChild(guildEmblem);
00226:                 var guildIdentity = AshfallUiHelpers.MakeSmall(foundryFaction.identity, autowrap: true);
00227:                 guildIdentity.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00228:                 guildIdentity.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
00229:                 guildHeader.AddChild(guildIdentity);
00230:                 guildBox.AddChild(guildHeader);
00231:                 guildBox.AddChild(AshfallUiHelpers.MakeSeparator());
00232:
00233:                 float standing = foundrySys.GuildStanding;
00234:                 guildBox.AddChild(AshfallUiHelpers.MakeDataRow("Foundry Standing", $"{standing:F0} / 100", AshfallUiHelpers.ToColor(standing >= 0 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Critical)));
00235:                 guildBox.AddChild(AshfallUiHelpers.MakeDataRow("Foundry", foundrySys.IsUnlocked ? $"OPEN · heat {foundrySys.HeatStage} · casts {foundrySys.TotalProductionCount}" : "SEALED — blueprint catalogued", AshfallUiHelpers.ToColor(foundrySys.IsUnlocked ? Ashfall.Core.UI.Theme.Pale : Ashfall.Core.UI.Theme.Dim)));
00236:
00237:                 if (foundryFaction.internal_divisions != null && foundryFaction.internal_divisions.Length > 0)
00238:                     guildBox.AddChild(AshfallUiHelpers.MakeDataRow("Internal Divisions", string.Join(", ", foundryFaction.internal_divisions).Replace('_', ' '), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
00239:
00240:                 foreach (var rel in foundryFaction.relationships)
00241:                 {
00242:                     if (rel == null || string.IsNullOrEmpty(rel.faction_id)) continue;
00243:                     guildBox.AddChild(AshfallUiHelpers.MakeDataRow(
00244:                         "↔ " + rel.faction_id.Replace('_', ' '),
00245:                         rel.stance.Replace('_', ' ') + " — " + rel.notes, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
00246:                 }
00248:                 var btnFoundry = AshfallUiHelpers.MakeButton("OPEN THE FOUNDRY FLOOR", () =>
00249:                 {
00250:                     // The host routes this through the standard panel-open path.
00251:                     OnFoundryPanelRequested?.Invoke();
00252:                 });
00253:                 guildBox.AddChild(btnFoundry);
00254:
00255:                 _factionsContainer.AddChild(guildCard);
00256:             }
00257:
00258:             // ── 3. Strategic Standing & Legal Accords ──
00259:             var relCard = AshfallUiHelpers.MakeCardFrame("STRATEGIC TREATIES & LEDGER DEBT", "TREATY STATUS");
00260:             var relBox = relCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00261:
00262:             relBox.AddChild(AshfallUiHelpers.MakeDataRow("Scavenger Guild Claim Ledger", "Two-color boundary system active. Stripping marked sites causes immediate blacklist.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
00263:             relBox.AddChild(AshfallUiHelpers.MakeDataRow("Nobody's Crossing Accord", "Vouch access required for passage across the northern ice road gate.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
00264:             relBox.AddChild(AshfallUiHelpers.MakeDataRow("Ledger Keepers Archive", "Knowledge reciprocity active. Relic blueprints grant credit value.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
00265:             _relationsContainer.AddChild(relCard);
00266:
00267:             // ── 3b. Adaptive Warlord Doctrine (Year of Ash, proposed model) ──
00268:             if (_yearOfAsh?.Warlord != null)
00269:             {
00270:                 var w = _yearOfAsh.Warlord;
00271:                 var wl = w.Catalog.Warlord;
00272:                 var wCard = AshfallUiHelpers.MakeCardFrame("WARLORD DOCTRINE — SECTOR 4", "ADAPTIVE STRATEGY (identity: " + wl.faction_id + ")");
00273:                 var wBox = wCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00274:
00275:                 string doctrine = w.Doctrine != null ? w.Doctrine.display_name : w.DoctrineId;
00276:                 wBox.AddChild(AshfallUiHelpers.MakeDataRow("Current Doctrine", doctrine, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
00277:                 wBox.AddChild(AshfallUiHelpers.MakeDataRow("Supply", w.Supply + " / " + w.SupplyNeed, AshfallUiHelpers.ToColor(w.Supply < w.SupplyNeed ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Pale)));
00279:                 // Tribute ledger (player-visible, from Core state).
00280:                 int ask = Math.Max(1, (int)(wl.tribute_base_amount * w.TributeMultiplier));
00281:                 string tributeState = w.State.consecutiveShortWeeks > 0
00282:                     ? $"ask {ask}× {wl.tribute_currency_item} — {w.State.consecutiveShortWeeks} short week(s), collector is keeping notes"
00283:                     : $"ask {ask}× {wl.tribute_currency_item} — ledger current";
00284:                 wBox.AddChild(AshfallUiHelpers.MakeDataRow("Tribute", tributeState,
00285:                     AshfallUiHelpers.ToColor(w.State.consecutiveShortWeeks > 0 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Warm)));
00286:                 wBox.AddChild(AshfallUiHelpers.MakeDataRow("Paid to Date", w.State.totalWeeksPaid + " of " + w.State.totalWeeksAsked + " asks", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
00287:                 wBox.AddChild(AshfallUiHelpers.MakeDataRow("Operations", w.TotalOperations + " · " + w.State.casualties + " casualties", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
00288:
00289:                 // Collector note (authored prose, deterministic by day).
00290:                 if (!string.IsNullOrEmpty(_collectorNote))
00291:                     wBox.AddChild(AshfallUiHelpers.MakeSmall(_collectorNote, autowrap: true));
00295:                 {
00296:                     var payRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
00297:                     payRow.AddThemeConstantOverride("h_separation", (int)Ashfall.Core.UI.Theme.SpacingSm);
00298:                     var btnPay = AshfallUiHelpers.MakeButton($"PAY TRIBUTE ({ask}× {wl.tribute_currency_item})", () => OnWarlordTributePay?.Invoke(ask));
00299:                     btnPay.CustomMinimumSize = new Vector2(300, 34);
00300:                     payRow.AddChild(btnPay);
00301:                     var btnRefuse = AshfallUiHelpers.MakeButton("REFUSE THIS WEEK", () => OnWarlordTributeRefuse?.Invoke());
00302:                     btnRefuse.CustomMinimumSize = new Vector2(180, 34);
00303:                     payRow.AddChild(btnRefuse);
00304:                     wBox.AddChild(payRow);
00305:                 }
00310:                     {
00311:                         var rec = w.State.territory[i];
00312:                         if (rec == null) continue;
00313:                         string stateName = ((Ashfall.Core.Warlords.WarlordTerritoryState)rec.state).ToString();
00314:                         float danger = w.TravelDangerModifier(rec.locationId);
00315:                         wBox.AddChild(AshfallUiHelpers.MakeDataRow(
00316:                             rec.locationId,
00317:                             stateName + (danger > 0f ? " · travel danger +" + (danger * 100f).ToString("F0") + "%" : ""),
00318:                             AshfallUiHelpers.ToColor(rec.state == (int)Ashfall.Core.Warlords.WarlordTerritoryState.Controlled
00319:                                 ? Ashfall.Core.UI.Theme.Hot
00320:                                 : (rec.state == (int)Ashfall.Core.Warlords.WarlordTerritoryState.Contested ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim))));
00321:                     }
00322:                 }
00323:                 _relationsContainer.AddChild(wCard);
00324:             }
00325:
00326:             // ── 3b. The Weight of Choices: Faction Progression & Branch Storylines ──
00327:             if (_branchCoordinator != null)
00328:             {
00329:                 var branchCard = AshfallUiHelpers.MakeCardFrame("THE WEIGHT OF CHOICES // FACTION PROGRESSION", "STRATEGIC ALLEGIANCE");
00330:                 var branchBox = branchCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00331:
00332:                 string activeFaction = _branchCoordinator.ActiveFactionKind.ToString();
00333:                 string activeBranch = _branchCoordinator.ActiveBranchId ?? "Unaligned (Prospective Paths Open)";
00334:                 string ponrText = _branchCoordinator.IsPonrLocked ? "LOCKED (Point of No Return Reached)" : "Open (Pre-PoNR)";
00335:                 string endingText = _branchCoordinator.ResolvedEndingId ?? "Unresolved";
00336:
00337:                 branchBox.AddChild(AshfallUiHelpers.MakeDataRow("Active Allegiance", activeFaction, AshfallUiHelpers.ToColor(_branchCoordinator.IsCommitted ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Pale)));
00338:                 branchBox.AddChild(AshfallUiHelpers.MakeDataRow("Current Branch", activeBranch.Replace('_', ' '), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot)));
00339:                 branchBox.AddChild(AshfallUiHelpers.MakeDataRow("PoNR Status", ponrText, AshfallUiHelpers.ToColor(_branchCoordinator.IsPonrLocked ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Pale)));
00340:                 if (_branchCoordinator.ResolvedEndingId != null)
00341:                 {
00342:                     branchBox.AddChild(AshfallUiHelpers.MakeDataRow("Resolved Ending", endingText.Replace('_', ' '), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
00343:                 }
00344:
00345:                 branchBox.AddChild(AshfallUiHelpers.MakeSeparator());
00346:
00347:                 // Standing summaries
00348:                 var standings = _branchCoordinator.GetFactionStandingSummaries();
00349:                 foreach (var s in standings)
00350:                 {
00351:                     string statusDesc = s.IsJoined ? "Joined / Allied" : (s.IsOpposed ? "Opposed" : (s.IsHostile ? "Hostile" : (s.IsAllied ? "Allied" : "Neutral")));
00352:                     branchBox.AddChild(AshfallUiHelpers.MakeDataRow(
00353:                         s.DisplayName,
00354:                         $"Standing: {s.Standing:+0;-0;0} | Alignment: {s.Alignment:+0;-0;0} ({statusDesc})",
00355:                         AshfallUiHelpers.ToColor(s.IsHostile ? Ashfall.Core.UI.Theme.Critical : (s.IsJoined || s.IsAllied ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Pale))));
00356:                 }
00357:
00358:                 branchBox.AddChild(AshfallUiHelpers.MakeSeparator());
00359:                 branchBox.AddChild(AshfallUiHelpers.MakeSmall("BRANCH PATH AVAILABILITY & CONSEQUENCES:"));
00360:
00361:                 var options = _branchCoordinator.GetBranchOptions(_moralChoice);
00362:                 int rendered = 0;
00363:                 foreach (var opt in options)
00364:                 {
00365:                     if (rendered >= 6 && !_branchCoordinator.IsCommitted) break;
00366:                     if (_branchCoordinator.IsCommitted && !opt.IsCommitted) continue;
00367:
00368:                     string statusTag = opt.IsCommitted ? "[COMMITTED]" : (opt.IsAvailable ? "[AVAILABLE]" : $"[LOCKED: {opt.LockoutReason}]");
00369:                     var optRow = AshfallUiHelpers.MakeDataRow(
00370:                         $"{opt.DisplayName} ({opt.FactionKind})",
00371:                         statusTag,
00372:                         AshfallUiHelpers.ToColor(opt.IsCommitted ? Ashfall.Core.UI.Theme.Hot : (opt.IsAvailable ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim)));
00373:                     branchBox.AddChild(optRow);
00374:
00375:                     if (opt.IsCommitted || opt.IsAvailable)
00376:                     {
00377:                         var desc = AshfallUiHelpers.MakeSmall($"Consequence: {opt.ConsequencesSummary} · Trigger: {opt.PonrTrigger}");
00378:                         branchBox.AddChild(desc);
00379:                     }
00380:
00381:                     if (opt.IsAvailable && !_branchCoordinator.IsCommitted)
00382:                     {
00383:                         string branchId = opt.BranchId;
00384:                         var commitBtn = AshfallUiHelpers.MakeButton($"COMMIT ALLEGIANCE // [{opt.DisplayName.ToUpperInvariant()}]", () =>
00385:                         {
00386:                             OnCommitBranchRequested?.Invoke(branchId);
00387:                         });
00388:                         commitBtn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00389:                         branchBox.AddChild(commitBtn);
00390:
00391:                         var warn = AshfallUiHelpers.MakeSmall("WARNING: Committing allegiance permanently locks out competing factions. The door will close.");
00392:                         warn.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warning));
00393:                         branchBox.AddChild(warn);
00394:                     }
00395:                     rendered++;
00396:                 }
00397:
00398:                 _relationsContainer.AddChild(branchCard);
00399:             }
00400:
00401:             // ── 4. Diplomatic Events & Radio Intercepts ──
00402:             var evCard = AshfallUiHelpers.MakeCardFrame("RECENT DIPLOMATIC COMMUNIQUES", "RADIO INTERCEPTS");
00403:             var evBox = evCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00404:
00405:             var catalog = _yearOfAsh?.WarRunner?.Catalog;
00406:             int currentDay = _yearOfAsh?.Timeline?.CurrentDay ?? 0;
00407:             var visible = catalog?.Communiques?
00408:                 .Where(c => c.day <= currentDay)
00409:                 .OrderByDescending(c => c.day)
00410:                 .Take(4)
00411:                 .ToList();
00415:                 foreach (var c in visible)
00416:                 {
00417:                     evBox.AddChild(AshfallUiHelpers.MakeDataRow($"[Day {c.day:D2}] {c.factionId}", c.title, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
00418:                 }
00419:             }
00420:             else
00421:             {
00422:                 evBox.AddChild(AshfallUiHelpers.MakeDataRow("STATUS", "No diplomatic communiqués intercepted on local frequencies.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
00423:             }
00424:             _eventsContainer.AddChild(evCard);
00425:         }
00426:
00427:         public override void _Ready()
00428:         {
00429:             SetAnchorsPreset(LayoutPreset.FullRect);
00430:             Visible = false;
00431:
00434:             AddChild(bg);
00435:
00436:             var scroll = new ScrollContainer();
00437:             scroll.SetAnchorsPreset(LayoutPreset.FullRect);
00438:             scroll.HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled;
00439:             AddChild(scroll);
00440:
00441:             var center = new CenterContainer();
00442:             center.SetAnchorsPreset(LayoutPreset.FullRect);
00443:             center.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00444:             center.SizeFlagsVertical = SizeFlags.ExpandFill;
00445:             scroll.AddChild(center);
00446:
00447:             var rootBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
00449:             center.AddChild(rootBox);
00450:
00451:             var title = AshfallUiHelpers.MakeTitle("FACTIONS & WASTELAND DIPLOMACY", Ashfall.Core.UI.Theme.FontSizeH1);
00452:             title.HorizontalAlignment = HorizontalAlignment.Center;
00453:             rootBox.AddChild(title);
00454:
00455:             _statusSummary = AshfallUiHelpers.MakeMetadata("Monitor geopolitical standings, faction trust, trade specialization, claim boundaries, and diplomatic treaties.");
00456:             _statusSummary.HorizontalAlignment = HorizontalAlignment.Center;
00457:             _statusSummary.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
00458:             rootBox.AddChild(_statusSummary);
00459:
00460:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00461:
00462:             _overviewContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
00463:             rootBox.AddChild(_overviewContainer);
00464:
00465:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00466:
00467:             var factionsTitle = AshfallUiHelpers.MakeSectionHeader("KNOWN FACTION PROTOCOLS & ALLIANCES");
00468:             rootBox.AddChild(factionsTitle);
00469:
00470:             _factionsContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
00471:             rootBox.AddChild(_factionsContainer);
00472:
00473:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00474:
00475:             var relTitle = AshfallUiHelpers.MakeSectionHeader("TREATIES, STANDING & DEBT OBLIGATIONS");
00476:             rootBox.AddChild(relTitle);
00477:
00478:             _relationsContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
00479:             rootBox.AddChild(_relationsContainer);
00480:
00481:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00482:
00483:             var evTitle = AshfallUiHelpers.MakeSectionHeader("RECENT FACTION COMMUNIQUES & DISPATCHES");
00484:             rootBox.AddChild(evTitle);
00485:
00486:             _eventsContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
00487:             rootBox.AddChild(_eventsContainer);
00488:
00489:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00490:
00491:             var btnClose = AshfallUiHelpers.MakeButton("CLOSE DIPLOMACY [Esc]", () => OnClose?.Invoke());
00492:             btnClose.CustomMinimumSize = new Vector2(220, 42);
00493:             rootBox.AddChild(btnClose);
00494:
00495:             var hint = AshfallUiHelpers.MakeSmall("[Esc] to close factions panel");
00496:             hint.HorizontalAlignment = HorizontalAlignment.Center;
00497:             hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
00498:             rootBox.AddChild(hint);
00499:         }
00500:
00501:         public void Open()
00502:         {
00503:             Visible = true;
00504:             RefreshView();
00505:             QueueRedraw();
00506:         }
00507:
00508:         public override void _UnhandledInput(InputEvent @event)
00509:         {
00510:             if (!Visible) return;
00511:
00512:             if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00513:             {
00514:                 OnClose?.Invoke();
00515:                 GetViewport().SetInputAsHandled();
00516:             }
00517:         }
00518:
00519:
00520:     public void Unbind()
00521:     {
00522:         if (_muster != null)
00523:                 _muster.StateChanged -= RefreshView;
00524:             if (_expansions != null)
00525:                 _expansions.StateChanged -= RefreshView;
00526:             if (_yearOfAsh?.Warlord != null)
00527:                 _yearOfAsh.Warlord.OnStateChanged -= RefreshView;
00528:     }
00529:
00530:     public override void _ExitTree()
00531:         {
00532:             Unbind();
00533:             base._ExitTree();
00534:         }
00535:     }
00536: }
```


# Appendix — Current Source Detail: `src/UI/QuestsPanel.cs`

### `src/UI/QuestsPanel.cs` — bounded current excerpt (687 of 774 lines)

- Size: 774 lines / 37776 bytes.
- SHA-256: `c1a69c26ccc3ef00cfd1078aee697c71d5550eb0b0ae1c461e0ec060f9bbf11b`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Crossing;
00007: using Ashfall.Core.Narrative;
00008: using Ashfall.Core.UI;
00009:
00010: namespace AtomicWar.GodotApp.UI
00011: {
00012:     /// <summary>
00013:     /// ASHFALL — Quests & Story progression panel.
00014:     /// Manages active wasteland operations, narrative objectives, Holdfast protocol stages,
00015:     /// Nobody's Charter missions, and historical quest completions using real Core systems.
00016:     /// </summary>
00017:     public partial class QuestsPanel : Control, IBindablePanel
00018:     {
00019:         public event Action? OnClose;
00020:         public event Action<string>? OnQuestDetailRequested;
00021:         public event Action? OnCrossingPanelRequested;
00022:         public event Action? OnProceduralQuestRequested;
00023:
00024:         /// <summary>Open the authored personal arc belonging to a survivor.</summary>
00025:         public event Action<string>? OnBeginSurvivorArcRequested;
00026:
00027:         /// <summary>Hand one objective item from stores to a survivor's arc.</summary>
00028:         public event Action<string, string>? OnDeliverArcObjectiveRequested;
00029:
00030:         /// <summary>Resolve a survivor arc's crisis fork with one of its two branches.</summary>
00031:         public event Action<string, string>? OnChooseArcBranchRequested;
00032:
00033:         private VBoxContainer _overviewContainer = null!;
00034:         private VBoxContainer _activeContainer = null!;
00035:         private VBoxContainer _availableContainer = null!;
00036:         private VBoxContainer _completedContainer = null!;
00037:         private Label _statusSummary = null!;
00038:
00039:         private HoldfastQuestSystem? _holdfastQuests;
00040:         private CrossingQuestSystem? _crossingQuests;
00041:         private DutyRosterHostSession? _dutyRoster;
00042:         private Ashfall.Core.Factions.FactionBranchCoordinator? _branchCoordinator;
00043:         private Ashfall.Core.MoralChoice.MoralChoiceSystem? _moralChoice;
00044:         private IReadOnlyList<Ashfall.Core.MoralChoice.MoralChoiceQuestDefinition>? _moralDefs;
00045:         private NarrativeQuestlineHostSession? _survivorArcs;
00046:         private ProceduralNarrativeHostSession? _proceduralNarrative;
00047:         private Func<string, string>? _survivorDisplayName;
00048:         private Func<string, string>? _itemLabel;
00049:         private int _currentDay = 1;
00050:
00051:         public bool IsBound => _holdfastQuests != null || _crossingQuests != null || _branchCoordinator != null || _moralDefs != null || _survivorArcs != null || _proceduralNarrative != null;
00052:
00053:         public void Bind(
00054:             HoldfastQuestSystem? holdfastQuests,
00055:             CrossingQuestSystem? crossingQuests = null,
00056:             DutyRosterHostSession? dutyRoster = null,
00057:             int currentDay = 1,
00058:             Ashfall.Core.Factions.FactionBranchCoordinator? branchCoordinator = null,
00059:             Ashfall.Core.MoralChoice.MoralChoiceSystem? moralChoice = null,
00060:             IReadOnlyList<Ashfall.Core.MoralChoice.MoralChoiceQuestDefinition>? moralDefs = null,
00061:             NarrativeQuestlineHostSession? survivorArcs = null,
00062:             Func<string, string>? survivorDisplayName = null,
00063:             Func<string, string>? itemLabel = null,
00064:             ProceduralNarrativeHostSession? proceduralNarrative = null)
00065:         {
00066:             Unbind();
00067:
00068:             _holdfastQuests = holdfastQuests;
00069:             _crossingQuests = crossingQuests;
00070:             _dutyRoster = dutyRoster;
00071:             _currentDay = currentDay;
00072:             _branchCoordinator = branchCoordinator;
00073:             _moralChoice = moralChoice;
00074:             _moralDefs = moralDefs;
00075:             _survivorArcs = survivorArcs;
00076:             _proceduralNarrative = proceduralNarrative;
00077:             _survivorDisplayName = survivorDisplayName;
00078:             _itemLabel = itemLabel;
00079:
00080:             if (_holdfastQuests != null)
00081:                 _holdfastQuests.OnStateChanged += HandleHoldfastStateChanged;
00082:             if (_crossingQuests != null)
00083:                 _crossingQuests.OnStateChanged += HandleCrossingStateChanged;
00084:             if (_branchCoordinator != null)
00085:                 _branchCoordinator.OnStateChanged += RefreshView;
00086:             if (_survivorArcs != null)
00087:                 _survivorArcs.StateChanged += RefreshView;
00088:             if (_proceduralNarrative != null)
00089:                 _proceduralNarrative.StateChanged += RefreshView;
00090:
00091:             RefreshView();
00092:         }
00093:
00094:         public void Unbind()
00095:         {
00096:             if (_holdfastQuests != null)
00097:             {
00098:                 _holdfastQuests.OnStateChanged -= HandleHoldfastStateChanged;
00099:                 _holdfastQuests = null;
00100:             }
00101:             if (_crossingQuests != null)
00102:             {
00103:                 _crossingQuests.OnStateChanged -= HandleCrossingStateChanged;
00104:                 _crossingQuests = null;
00105:             }
00106:             if (_branchCoordinator != null)
00107:             {
00108:                 _branchCoordinator.OnStateChanged -= RefreshView;
00109:                 _branchCoordinator = null;
00110:             }
00111:             if (_survivorArcs != null)
00112:             {
00131:         private void HandleCrossingStateChanged(CrossingQuestSystemState _) => RefreshView();
00132:
00133:         public void RefreshView()
00134:         {
00135:             if (_overviewContainer == null || _activeContainer == null ||
00136:                 _availableContainer == null || _completedContainer == null)
00137:                 return;
00138:
00139:             AshfallUiHelpers.EmptyChildren(_overviewContainer);
00140:             AshfallUiHelpers.EmptyChildren(_activeContainer);
00141:             AshfallUiHelpers.EmptyChildren(_availableContainer);
00142:             AshfallUiHelpers.EmptyChildren(_completedContainer);
00143:
00144:             int activeCount = 0;
00145:             int completedCount = 0;
00146:
00147:             // ── 1. Active & Completed Quests Extraction ──
00148:             var activeList = new List<(string id, string name, string type, string stageText, int stageNum, int totalStages, string briefing)>();
00149:             var completedList = new List<(string id, string name, string type, string resolution)>();
00150:             var availableList = new List<(string id, string name, string type, string reqs, string briefing)>();
00151:
00152:             // Check Holdfast Main Questline
00155:                 foreach (string qId in HoldfastQuestSystem.MainQuestIds)
00156:                 {
00157:                     var def = _holdfastQuests.GetDef(qId);
00158:                     var progress = _holdfastQuests.GetProgress(qId);
00159:                     string displayName = def?.display_name ?? _holdfastQuests.GetDisplayName(qId);
00160:                     int stageCount = def?.StageCount ?? 4;
00161:
00162:                     if (progress != null && progress.completed)
00163:                     {
00168:                     {
00169:                         activeCount++;
00170:                         string stageText = _holdfastQuests.GetStageText(qId);
00171:                         if (string.IsNullOrEmpty(stageText) && def?.stages != null && def.stages.Length > progress.stage)
00172:                             stageText = def.stages[progress.stage].text;
00173:                         activeList.Add((qId, displayName, "Main Protocol // The Holdfast", stageText, progress.stage + 1, stageCount, def?.briefing ?? ""));
00174:                     }
00175:                     else
00176:                     {
00177:                         // Available or upcoming
00179:                         if (!string.IsNullOrEmpty(def?.prereq_quest_id))
00180:                             reqs += $" · Requires: {def.prereq_quest_id}";
00181:                         availableList.Add((qId, displayName, "Holdfast Directive", reqs, def?.briefing ?? "Awaiting protocol conditions."));
00182:                     }
00183:                 }
00184:             }
00185:
00187:             if (_crossingQuests != null)
00188:             {
00189:                 var availCrossing = _crossingQuests.GetAvailableQuests(_currentDay);
00190:                 if (availCrossing != null)
00191:                 {
00192:                     foreach (var cDef in availCrossing)
00193:                     {
00194:                         if (cDef == null) continue;
00195:                         var p = _crossingQuests.GetProgress(cDef.id);
00196:                         if (p != null && p.completed)
00197:                         {
00198:                             completedCount++;
00199:                             completedList.Add((cDef.id, cDef.display_name, "Nobody's Charter // Crossing", "Arbitration objective resolved."));
00200:                         }
00201:                         else if (p != null && p.started)
00202:                         {
00203:                             activeCount++;
00204:                             string stageText = (cDef.stages != null && cDef.stages.Count > p.currentStage && p.currentStage >= 0)
00205:                                 ? cDef.stages[p.currentStage].text
00206:                                 : (cDef.stages != null && cDef.stages.Count > 0 ? cDef.stages[0].text : "Crossing objective");
00207:                             activeList.Add((cDef.id, cDef.display_name, "Nobody's Charter // Crossing", stageText, p.currentStage + 1, cDef.stages?.Count ?? 1, cDef.briefing));
00208:                         }
00209:                         else
00210:                         {
00211:                             availableList.Add((cDef.id, cDef.display_name, "Crossing Charter", $"Day >= {cDef.min_day}", cDef.briefing));
00218:             if (_branchCoordinator != null)
00219:             {
00220:                 if (_branchCoordinator.IsCommitted)
00221:                 {
00222:                     string branchName = _branchCoordinator.ActiveBranchId?.Replace('_', ' ') ?? "Faction Branch";
00223:                     string factionName = _branchCoordinator.ActiveFactionKind.ToString();
00224:
00225:                     if (_branchCoordinator.ResolvedEndingId != null)
00226:                     {
00227:                         completedCount++;
00228:                         completedList.Add((
00229:                             _branchCoordinator.ActiveBranchId!,
00230:                             $"Faction Finale: {branchName}",
00231:                             $"The Weight of Choices // {factionName}",
00232:                             $"Resolved Ending: {_branchCoordinator.ResolvedEndingId.Replace('_', ' ')}"));
00233:                     }
00234:                     else
00235:                     {
00236:                         activeCount++;
00237:                         string stageText = _branchCoordinator.IsPonrLocked
00238:                             ? "Point of No Return Reached. Faction fate sealed — proceeding to final resolution."
00239:                             : "Pre-PoNR Stage: Executing faction directives and shaping ideological alignment.";
00240:                         int stageNum = _branchCoordinator.IsPonrLocked ? 2 : 1;
00241:                         activeList.Add((
00242:                             _branchCoordinator.ActiveBranchId!,
00243:                             $"Faction Allegiance: {branchName}",
00244:                             $"The Weight of Choices // {factionName}",
00245:                             stageText,
00246:                             stageNum, 2,
00247:                             $"Allegiance to {factionName} active. Mutually exclusive branch path locked in."));
00248:                     }
00249:                 }
00250:                 else
00251:                 {
00252:                     // Prospective branches available to commit
00253:                     var options = _branchCoordinator.GetBranchOptions(_moralChoice);
00254:                     foreach (var opt in options)
00255:                     {
00256:                         if (opt.IsAvailable)
00257:                         {
00258:                             availableList.Add((
00259:                                 opt.BranchId,
00260:                                 $"Prospective Allegiance: {opt.DisplayName}",
00261:                                 $"Faction Branch ({opt.FactionKind})",
00262:                                 $"Morality: {opt.EntryBandMin}..{opt.EntryBandMax}",
00263:                                 $"{opt.ConsequencesSummary} Trigger: {opt.PonrTrigger}"));
00264:                         }
00265:                     }
00266:                 }
00267:             }
00272:                 foreach (var mDef in _moralDefs)
00273:                 {
00274:                     if (_moralChoice.IsResolved(mDef.Id))
00275:                     {
00276:                         if (_moralChoice.TryGetResolution(mDef.Id, out var res) && res != null)
00277:                         {
00278:                             completedCount++;
00279:                             completedList.Add((mDef.Id, mDef.DisplayName, $"The Weight of Survival // {mDef.Category.ToUpperInvariant()}", res.epitaph));
00280:                         }
00281:                     }
00282:                     else if (Ashfall.Core.MoralChoice.MoralChoiceSystem.IsAvailableOnDay(mDef, _currentDay) &&
00283:                              _moralChoice.IsChainQuestAccessible(mDef.Id, _currentDay))
00284:                     {
00285:                         availableList.Add((
00286:                             mDef.Id,
00293:             }
00294:
00295:             // If no active quests found in live session, provide the initial starting protocol
00296:             if (activeList.Count == 0)
00297:             {
00298:                 activeList.Add((
00299:                     HoldfastQuestSystem.Sheet,
00306:
00307:             // ── Overview Card ──
00308:             var ovCard = AshfallUiHelpers.MakeCardFrame("NARRATIVE OPERATIONS & CAMPAIGN DIRECTIVES", "MISSION STATUS");
00309:             var ovBox = ovCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00310:
00311:             ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Campaign Timeline", $"Day {_currentDay:00} After Nuclear Exchange", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
00312:             ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Active Mission Operations", $"{Math.Max(activeCount, activeList.Count)} Operation(s) In Progress", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot)));
00313:             ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Completed Protocols", $"{completedCount} Milestones Recorded", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
00314:
00315:             if (_crossingQuests != null)
00316:             {
00317:                 var btnCrossing = AshfallUiHelpers.MakeButton("OPEN NOBODY'S CHARTER // CROSSING PROTOCOLS", () =>
00318:                 {
00319:                     OnCrossingPanelRequested?.Invoke();
00320:                 });
00321:                 ovBox.AddChild(btnCrossing);
00322:             }
00323:
00324:             _overviewContainer.AddChild(ovCard);
00325:
00326:             // ── Canonical procedural narrative runtime (Plan 171) ──
00327:             // This card is a read/command surface over the existing
00328:             // ProceduralNarrativeSystem + QuestRuntimeCoordinator pair. The
00329:             // panel never selects templates or mutates quest state itself.
00330:             if (_proceduralNarrative != null)
00332:                 var runtime = _proceduralNarrative.QuestRuntime;
00333:                 var proceduralCard = AshfallUiHelpers.MakeCardFrame(
00334:                     "PROCEDURAL OPERATIONAL OPPORTUNITIES",
00335:                     "JSON templates · canonical quest runtime");
00336:                 var proceduralBox = proceduralCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00337:                 var proceduralRows = runtime.BuildReadModel();
00338:                 int activeProcedural = 0;
00339:                 int offeredProcedural = 0;
00340:                 for (int i = 0; i < proceduralRows.Count; i++)
00341:                 {
00342:                     if (!proceduralRows[i].isProcedural) continue;
00343:                     if (proceduralRows[i].status == Ashfall.Core.Quests.QuestLifecycleState.Active) activeProcedural++;
00344:                     if (proceduralRows[i].status == Ashfall.Core.Quests.QuestLifecycleState.Offered) offeredProcedural++;
00345:                 }
00346:                 proceduralBox.AddChild(AshfallUiHelpers.MakeDataRow(
00347:                     "Canonical runtime",
00348:                     $"{activeProcedural} active · {offeredProcedural} offered · {proceduralRows.Count} total",
00349:                     AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
00350:                 var generateButton = AshfallUiHelpers.MakeButton(
00351:                     "REQUEST NEW OPERATIONAL OPPORTUNITY",
00352:                     () => OnProceduralQuestRequested?.Invoke());
00353:                 proceduralBox.AddChild(generateButton);
00354:                 _availableContainer.AddChild(proceduralCard);
00355:             }
00356:
00357:             // ── Active Quests ──
00358:             foreach (var q in activeList)
00359:             {
00360:                 var card = AshfallUiHelpers.MakeCardFrame(q.name, $"{q.type.ToUpperInvariant()} · STAGE {q.stageNum}/{q.totalStages}");
00361:                 var cardBox = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00362:
00363:                 var stageHeader = AshfallUiHelpers.MakeSubsectionHeader("CURRENT OPERATIONAL OBJECTIVE");
00364:                 cardBox.AddChild(stageHeader);
00365:
00366:                 var stageLbl = AshfallUiHelpers.MakeBody($"► {q.stageText}");
00367:                 stageLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
00368:                 cardBox.AddChild(stageLbl);
00369:
00370:                 if (!string.IsNullOrEmpty(q.briefing))
00371:                 {
00372:                     cardBox.AddChild(AshfallUiHelpers.MakeSeparator());
00373:                     var briefLbl = AshfallUiHelpers.MakeSmall(q.briefing);
00374:                     briefLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
00375:                     cardBox.AddChild(briefLbl);
00376:                 }
00377:
00378:                 var btnRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
00380:                 var inspectBtn = AshfallUiHelpers.MakeButton($"INSPECT QUEST DOSSIER // [{q.name}]", () =>
00381:                 {
00382:                     OnQuestDetailRequested?.Invoke(questId);
00383:                 });
00384:                 inspectBtn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00385:                 btnRow.AddChild(inspectBtn);
00386:                 cardBox.AddChild(btnRow);
00387:
00388:                 _activeContainer.AddChild(card);
00389:             }
00390:
00391:             // ── Available / Upcoming Missions ──
00392:             if (availableList.Count > 0)
00393:             {
00394:                 int showCount = Math.Min(8, availableList.Count);
00395:                 for (int i = 0; i < showCount; i++)
00397:                     var avail = availableList[i];
00398:                     var card = AshfallUiHelpers.MakeCardFrame(avail.name, avail.reqs);
00399:                     var cardBox = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00400:
00401:                     var briefLbl = AshfallUiHelpers.MakeSmall(avail.briefing);
00402:                     briefLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
00403:                     cardBox.AddChild(briefLbl);
00404:
00405:                     string qId = avail.id;
00406:                     var btn = AshfallUiHelpers.MakeButton($"VIEW BRIEFING // [{avail.name}]", () =>
00407:                     {
00408:                         OnQuestDetailRequested?.Invoke(qId);
00409:                     });
00410:                     cardBox.AddChild(btn);
00411:
00412:                     _availableContainer.AddChild(card);
00413:                 }
00414:             }
00415:
00416:             // ── Completed Quests ──
00417:             var compCard = AshfallUiHelpers.MakeCardFrame("HISTORICAL OPERATION COMPLETIONS", "LOG ARCHIVE");
00418:             var compBox = compCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00419:
00420:             if (completedList.Count > 0)
00421:             {
00422:                 foreach (var comp in completedList)
00427:             else
00428:             {
00429:                 compBox.AddChild(AshfallUiHelpers.MakeDataRow("Day 01 Protocol", "Bunker seal integrity established. Air filtration online.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
00430:                 compBox.AddChild(AshfallUiHelpers.MakeDataRow("Opening Census", "Initial 12-survivor roster logged into Holdfast ledger.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
00431:             }
00432:             _completedContainer.AddChild(compCard);
00433:
00434:             // ── Duty Roster quests (Exp 02) — real runtime read model ──
00435:             if (_dutyRoster != null)
00436:             {
00438:                 var rosterCard = AshfallUiHelpers.MakeCardFrame(
00439:                     "DUTY ROSTER // ALLOCATION 12 CHART", qRuntime.StartedCount + " started · " + qRuntime.CompletedCount + " complete");
00440:                 var rosterBox = rosterCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00441:
00442:                 var active = qRuntime.GetActiveQuests();
00443:                 if (active.Count > 0)
00444:                 {
00445:                     for (int i = 0; i < active.Count; i++)
00446:                     {
00447:                         var q = active[i];
00448:                         if (q == null) continue;
00449:                         var p = qRuntime.GetProgress(q.id);
00450:                         string stage = p != null ? $"stage {p.currentStage + 1}/{q.StageCount}" : "";
00451:                         rosterBox.AddChild(AshfallUiHelpers.MakeDataRow(
00452:                             $"▶ {q.display_name}", stage, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
00453:                         // Authored stage prose rendered to the player (house voice).
00456:                         {
00457:                             var proseLbl = AshfallUiHelpers.MakeSmall(prose, autowrap: true);
00458:                             proseLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
00459:                             rosterBox.AddChild(proseLbl);
00460:                         }
00461:                     }
00462:                 }
00467:                 }
00468:
00469:                 var available = qRuntime.GetAvailableQuests(_dutyRoster.Clock.Day);
00470:                 if (available.Count > 0)
00471:                 {
00472:                     for (int i = 0; i < available.Count && i < 6; i++)
00473:                     {
00474:                         var q = available[i];
00475:                         if (q == null) continue;
00476:                         string prereq = string.IsNullOrEmpty(q.prereq_quest_id) ? "" : " · after " + q.prereq_quest_id;
00477:                         rosterBox.AddChild(AshfallUiHelpers.MakeDataRow(
00478:                             $"◦ {q.display_name}", $"day {q.min_day}+{prereq}", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
00479:                     }
00482:                 }
00483:
00484:                 var started = qRuntime.GetProgress(DutyRosterIds.QuestTheChart);
00485:                 var btnStart = AshfallUiHelpers.MakeButton(
00486:                     started != null && started.started && !started.completed ? "ADVANCE CHART QUEST" : "START THE CHART",
00487:                     () =>
00488:                     {
00495:                 rosterBox.AddChild(btnStart);
00496:
00497:                 _availableContainer.AddChild(rosterCard);
00498:             }
00499:
00500:             RenderSurvivorArcs();
00501:         }
00502:
00503:         /// <summary>
00504:         /// Renders the authored survivor personal arcs (narrative_questlines.json)
00505:         /// as a real command surface: open an arc, hand over the objective the
00506:         /// current stage owes, or resolve the crisis fork. The commands are raised
00507:         /// as events; Main owns inventory spend, morale and trait recording.
00508:         /// Survivor, item and trait identifiers are shown through labels, never raw.
00509:         /// </summary>
00510:         private void RenderSurvivorArcs()
00511:         {
00512:             if (_survivorArcs == null) return;
00513:             if (_activeContainer == null || _availableContainer == null || _completedContainer == null) return;
00514:
00515:             var defs = _survivorArcs.Definitions;
00516:             for (int i = 0; i < defs.Count; i++)
00517:             {
00518:                 var def = defs[i];
00519:                 if (def == null || string.IsNullOrEmpty(def.survivorId) || def.stages.Count == 0) continue;
00520:
00521:                 string survivorId = def.survivorId;
00522:                 string who = SurvivorLabel(survivorId);
00523:                 var arc = _survivorArcs.GetArc(survivorId);
00524:
00525:                 // ── Not yet opened: the arc is available ──
00526:                 if (arc == null)
00527:                 {
00528:                     var opening = def.stages[0];
00529:                     var card = AshfallUiHelpers.MakeCardFrame(def.title, $"Personal arc // {who}");
00530:                     var box = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00531:
00532:                     var openLbl = AshfallUiHelpers.MakeSmall($"{opening.name}: {opening.description}");
00533:                     openLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
00534:                     box.AddChild(openLbl);
00535:
00536:                     var openBtn = AshfallUiHelpers.MakeButton($"OPEN PERSONAL ARC // [{who}]", () =>
00537:                     {
00538:                         OnBeginSurvivorArcRequested?.Invoke(survivorId);
00539:                         RefreshView();
00540:                     });
00541:                     box.AddChild(openBtn);
00542:
00543:                     _availableContainer.AddChild(card);
00544:                     continue;
00545:                 }
00546:
00547:                 // ── Resolved: show what was chosen and what it recorded ──
00548:                 if (arc.status == Ashfall.Core.Quests.NarrativeArcStatus.Resolved)
00549:                 {
00550:                     var chosen = def.FindBranchStage()?.FindBranch(arc.chosenBranchId);
00551:                     var doneCard = AshfallUiHelpers.MakeCardFrame(def.title, $"Personal arc // {who}");
00552:                     var doneBox = doneCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00553:
00554:                     doneBox.AddChild(AshfallUiHelpers.MakeDataRow(
00555:                         chosen != null ? $"Resolved — {chosen.label}" : "Resolved",
00556:                         string.IsNullOrEmpty(arc.grantedTraitId)
00557:                             ? "no trait recorded"
00558:                             : $"recorded: {TraitLabel(arc.grantedTraitId)}",
00559:                         AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
00561:                     var epilogue = def.FindStage(def.FinalStageIndex);
00562:                     if (epilogue != null && !string.IsNullOrEmpty(epilogue.description))
00563:                         doneBox.AddChild(AshfallUiHelpers.MakeSmall(epilogue.description));
00564:
00565:                     _completedContainer.AddChild(doneCard);
00566:                     continue;
00567:                 }
00568:
00569:                 // ── In progress: either owes supplies or awaits the fork ──
00570:                 var stage = def.FindStage(arc.currentStage);
00571:                 var card2 = AshfallUiHelpers.MakeCardFrame(def.title, $"Personal arc // {who}");
00572:                 var box2 = card2.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00573:
00574:                 box2.AddChild(AshfallUiHelpers.MakeSubsectionHeader(
00575:                     stage != null ? $"CURRENT STAGE // {stage.name.ToUpperInvariant()}" : "CURRENT STAGE"));
00576:
00577:                 if (stage != null && !string.IsNullOrEmpty(stage.description))
00578:                 {
00579:                     var stageLbl = AshfallUiHelpers.MakeBody($"► {stage.description}");
00580:                     stageLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
00581:                     box2.AddChild(stageLbl);
00582:                 }
00583:
00584:                 if (stage != null && stage.HasBranch &&
00587:                     box2.AddChild(AshfallUiHelpers.MakeSeparator());
00588:                     box2.AddChild(AshfallUiHelpers.MakeSmall(
00589:                         "Two ways through this, and only one can be taken. Choosing ends the arc."));
00590:                     AddArcBranchButton(box2, survivorId, stage.branchA!);
00591:                     AddArcBranchButton(box2, survivorId, stage.branchB!);
00592:                 }
00593:                 else
00594:                 {
00595:                     var owed = _survivorArcs.GetOutstandingObjectives(survivorId);
00596:                     if (owed.Count > 0)
00597:                     {
00598:                         box2.AddChild(AshfallUiHelpers.MakeSeparator());
00599:                         box2.AddChild(AshfallUiHelpers.MakeSubsectionHeader("OWED FROM STORES"));
00600:                         for (int o = 0; o < owed.Count; o++)
00601:                         {
00602:                             string itemId = owed[o];
00603:                             var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
00604:
00605:                             var itemLbl = AshfallUiHelpers.MakeBody(ItemNameLabel(itemId));
00606:                             itemLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00607:                             row.AddChild(itemLbl);
00608:
00609:                             var handBtn = AshfallUiHelpers.MakeButton("HAND OVER", () =>
00610:                             {
00611:                                 OnDeliverArcObjectiveRequested?.Invoke(survivorId, itemId);
00612:                                 RefreshView();
00613:                             });
00614:                             row.AddChild(handBtn);
00615:
00624:                 }
00625:
00626:                 _activeContainer.AddChild(card2);
00627:             }
00628:         }
00629:
00630:         private void AddArcBranchButton(
00631:             VBoxContainer box, string survivorId, Ashfall.Core.Quests.NarrativeQuestlineBranchDef branch)
00632:         {
00633:             if (branch == null) return;
00634:             string branchId = branch.id;
00635:
00636:             var lbl = AshfallUiHelpers.MakeSmall(
00637:                 $"{branch.label} — {branch.description} (morale {branch.moraleDelta:+0;-0;0})");
00638:             lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
00639:             box.AddChild(lbl);
00640:
00641:             var btn = AshfallUiHelpers.MakeButton($"CHOOSE // [{branch.label}]", () =>
00642:             {
00643:                 OnChooseArcBranchRequested?.Invoke(survivorId, branchId);
00644:                 RefreshView();
00645:             });
00646:             box.AddChild(btn);
00647:         }
00649:         private string SurvivorLabel(string survivorId)
00650:         {
00651:             var resolved = _survivorDisplayName?.Invoke(survivorId);
00652:             return string.IsNullOrWhiteSpace(resolved) ? HumanizeArcToken(survivorId) : resolved!;
00653:         }
00654:
00655:         private string ItemNameLabel(string itemId)
00656:         {
00657:             var resolved = _itemLabel?.Invoke(itemId);
00658:             return string.IsNullOrWhiteSpace(resolved) ? HumanizeArcToken(itemId) : resolved!;
00659:         }
00660:
00661:         private static string TraitLabel(string traitId) => HumanizeArcToken(traitId);
00662:
00665:         {
00666:             if (string.IsNullOrWhiteSpace(id)) return string.Empty;
00667:             string[] parts = id.Split('_', StringSplitOptions.RemoveEmptyEntries);
00668:             for (int i = 0; i < parts.Length; i++)
00669:             {
00670:                 if (parts[i].Length == 0) continue;
00671:                 parts[i] = char.ToUpperInvariant(parts[i][0]) + (parts[i].Length > 1 ? parts[i][1..] : string.Empty);
00672:             }
00673:             return string.Join(" ", parts);
00674:         }
00675:
00676:         public override void _Ready()
00677:         {
00678:             SetAnchorsPreset(LayoutPreset.FullRect);
00679:             Visible = false;
00680:
00683:             AddChild(bg);
00684:
00685:             var scroll = new ScrollContainer();
00686:             scroll.SetAnchorsPreset(LayoutPreset.FullRect);
00687:             scroll.HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled;
00688:             AddChild(scroll);
00689:
00690:             var center = new CenterContainer();
00691:             center.SetAnchorsPreset(LayoutPreset.FullRect);
00692:             center.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00693:             center.SizeFlagsVertical = SizeFlags.ExpandFill;
00694:             scroll.AddChild(center);
00695:
00696:             var rootBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
00698:             center.AddChild(rootBox);
00699:
00700:             var title = AshfallUiHelpers.MakeTitle("OPERATIONS & STORY PROGRESSION", Ashfall.Core.UI.Theme.FontSizeH1);
00701:             title.HorizontalAlignment = HorizontalAlignment.Center;
00702:             rootBox.AddChild(title);
00703:
00704:             _statusSummary = AshfallUiHelpers.MakeMetadata("Active survival objectives, Holdfast protocol directives, and narrative campaign storylines.");
00705:             _statusSummary.HorizontalAlignment = HorizontalAlignment.Center;
00706:             _statusSummary.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
00707:             rootBox.AddChild(_statusSummary);
00708:
00709:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00710:
00711:             _overviewContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
00712:             rootBox.AddChild(_overviewContainer);
00713:
00714:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00715:
00716:             var activeTitle = AshfallUiHelpers.MakeSectionHeader("ACTIVE OPERATIONS & CURRENT STAGES");
00717:             rootBox.AddChild(activeTitle);
00718:
00719:             _activeContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
00720:             rootBox.AddChild(_activeContainer);
00721:
00722:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00723:
00724:             var availTitle = AshfallUiHelpers.MakeSectionHeader("UPCOMING PROTOCOLS & AVAILABLE DIRECTIVES");
00725:             rootBox.AddChild(availTitle);
00726:
00727:             _availableContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
00728:             rootBox.AddChild(_availableContainer);
00729:
00730:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00731:
00732:             var compTitle = AshfallUiHelpers.MakeSectionHeader("COMPLETED PROTOCOL LOGS");
00733:             rootBox.AddChild(compTitle);
00734:
00735:             _completedContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
00736:             rootBox.AddChild(_completedContainer);
00737:
00738:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00739:
00740:             var btnClose = AshfallUiHelpers.MakeButton("CLOSE QUESTS [Esc]", () => OnClose?.Invoke());
00741:             btnClose.CustomMinimumSize = new Vector2(220, 42);
00742:             rootBox.AddChild(btnClose);
00743:
00744:             var hint = AshfallUiHelpers.MakeSmall("[Esc] to close quest journal");
00745:             hint.HorizontalAlignment = HorizontalAlignment.Center;
00746:             hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
00747:             rootBox.AddChild(hint);
00748:         }
00749:
00750:         public void Open()
00751:         {
00752:             Visible = true;
00753:             RefreshView();
00754:             QueueRedraw();
00755:         }
00756:
00757:         public override void _UnhandledInput(InputEvent @event)
00758:         {
00759:             if (!Visible) return;
00760:
00761:             if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00762:             {
00763:                 OnClose?.Invoke();
00764:                 GetViewport().SetInputAsHandled();
00765:             }
00766:         }
00767:
00768:         public override void _ExitTree()
00769:         {
00770:             Unbind();
00771:             base._ExitTree();
00772:         }
00773:     }
00774: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **The subject is an irreversible but legible faction branch path inside the combined Weight of Choices coordinator. The plan expands branch state, migration, host and UI truthfulness while preserving one cross-faction commitment authority.**.

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
