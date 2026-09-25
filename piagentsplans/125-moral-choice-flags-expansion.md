# Plan 125 — Moral Choice Flags, Ethical Memory and Downstream Gate Semantics

> **Rebuild status:** COMPLETE 25-FLAG DATA LOOP — PRODUCER/FLAG-OWNER AND REACHABILITY AUDIT
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

- The current catalog is a display/definition catalog with 25 `id`/`display_name` rows. The live moral owner stores `activeFlags` in `MoralChoiceState`, sets them idempotently through `MoralChoiceSystem.SetFlag`, and mirrors them to the injected flag ledger.
- The route is quest definition `set_flag` → `MoralChoiceSystem.Resolve` → `SetFlag`/flag ledger → branch/quest predicates and later host projections → `MoralChoiceSaveStore` restore.
- The high-value audit is to prove every authored flag has a real producer or is explicitly reserved, every consumer reads the canonical owner, and UI history cannot claim a flag that the save state did not set.

**Bounded outcome:** Retire the old 10→25 pure-data brief as a new catalog project. The current JSON has 25 flags, `MoralChoiceIds` pins the canonical IDs, `MoralChoiceSystem` owns active flag state and `MoralChoiceState` persists it. The remaining work is a producer/gate/UI audit that distinguishes flag definitions from the authoritative active-flag set; do not create a second moral ledger.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `moral_choice_flags.json` is valid schema version 1 with 25 flags; `MoralChoiceFlagCatalogLoader` reads the definitions and returns `MoralChoiceFlagDefinitions`.
- `MoralChoiceIds.AllFlags` includes the authored persistent flag IDs plus branch/message markers; `MoralChoiceState.activeFlags` is the persistent owner field.
- `MoralChoiceSystem.SetFlag` is idempotent, writes the injected `IFlagLedger` with source `moral_choice`, and `HasFlag` checks both the ledger and state during restore.
- `Main.MoralChoice` loads definitions, choice catalogs, gossip and the save; the current host does not by itself prove that every definition is displayed or consumed, so the rebase makes that an explicit audit question.

**Master-authority sections applied to this rebase:**

- Part II Factory Protocol: premise sweep, collision check, one lane/cluster, and evidence labels before drafting.
- Part II Step 5 continuity and anti-duplication checklist: data presence is not reachability.
- Part III cluster map: use the live C1–C17 owner map rather than a historical plan title.
- Part IV backlog discipline: consume a verified candidate or record why it is stale; do not widen a bounded outcome.
- Part V Template S/R: subject intent and recommended route remain separate from implementation commitments.
- Part VI Multi-Session Growth Protocol: 250k is a depth target, not permission to manufacture volume.
- Live source/data authority: current catalog, loader, host, save, and focused tests outrank generated prose.
- Anti-padding rule: if the evidence queue is exhausted, stop and report no warranted continuation.
- C10 Quests/moral-choice cluster: persistent ethical memory is a state owner, not a prose catalog side effect.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 10→25 target with a 25-row definition/state/producer/consumer matrix.
- Separate immutable flag definitions from mutable active flags and identify the sole owner for each downstream gate.
- Trace representative producers from authored choice `set_flag` fields through the current resolution/save route.
- Audit delayed callbacks, branch locks, faction reactions, gossip and UI/history projections for truthful flag availability without creating a parallel ledger.

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
| authored flag definitions | MoralChoiceFlagCatalogLoader | `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagCatalogLoader.cs` | Loads display metadata; it does not own active state. |
| canonical flag ID vocabulary | MoralChoiceIds | `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs` | Pins stable identifiers and existing branch/message flags. |
| flag mutation, quest gates and resolution facts | MoralChoiceSystem | `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` | Sole active moral-choice owner; writes state and injected ledger. |
| persistent active flags and resolutions | MoralChoiceState/MoralChoiceSave | `Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs; src/Host/MoralChoiceSaveStore.cs` | Existing moral-choice save path; no second flag store. |
| host composition and player presentation | Main.MoralChoice/MoralChoicePanel | `src/Main.MoralChoice.cs; src/UI/MoralChoiceModal.cs` | Host/UI projection; it cannot set a flag independently. |
| catalog, branch/gossip and flag-state proof | Moral focused tests | `Ashfall.Core.Tests/MoralChoiceFlagPlan125Tests.cs; Ashfall.Core.Tests/World/Plan124_125WarMoralIntegrationTests.cs; Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs` | Focused evidence surface. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Moral Choice Flags, Ethical Memory and Downstream Gate Semantics
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ MoralChoiceFlagCatalogLoader
│   authored flag definitions
│ MoralChoiceIds
│   canonical flag ID vocabulary
│ MoralChoiceSystem
│   flag mutation, quest gates and resolution facts
│ MoralChoiceState/MoralChoiceSave
│   persistent active flags and resolutions
│ Main.MoralChoice/MoralChoicePanel
│   host composition and player presentation
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

1. **Preserve current state ownership.** MoralChoiceFlagCatalogLoader owns authored flag definitions: Loads display metadata; it does not own active state.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| authored flag definitions | MoralChoiceFlagCatalogLoader | `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagCatalogLoader.cs` | Loads display metadata; it does not own active state. |
| canonical flag ID vocabulary | MoralChoiceIds | `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs` | Pins stable identifiers and existing branch/message flags. |
| flag mutation, quest gates and resolution facts | MoralChoiceSystem | `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` | Sole active moral-choice owner; writes state and injected ledger. |
| persistent active flags and resolutions | MoralChoiceState/MoralChoiceSave | `Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs; src/Host/MoralChoiceSaveStore.cs` | Existing moral-choice save path; no second flag store. |
| host composition and player presentation | Main.MoralChoice/MoralChoicePanel | `src/Main.MoralChoice.cs; src/UI/MoralChoiceModal.cs` | Host/UI projection; it cannot set a flag independently. |
| catalog, branch/gossip and flag-state proof | Moral focused tests | `Ashfall.Core.Tests/MoralChoiceFlagPlan125Tests.cs; Ashfall.Core.Tests/World/Plan124_125WarMoralIntegrationTests.cs; Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs` | Focused evidence surface. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load 25 flag definitions and the current moral-choice catalogs
2. restore MoralChoiceSave and replay active flags
3. present available moral choices using current prerequisites
4. resolve one choice through MoralChoiceSystem
5. set the authored flag idempotently and mirror it to IFlagLedger
6. emit resolution/branch/threshold facts for existing consumers
7. project current moral history and capture the existing save

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Definitions are immutable catalog rows; `activeFlags` is a set-like owner state and `resolutions` records the choice facts that produced flags.
- SetFlag is idempotent and branch/flag ledger writes are exactly-once.
- Restore replays active flags into the runtime ledger and preserves the saved moral state.
- A flag definition without a producer is not a gameplay effect; it must be labeled reserved/orphaned rather than displayed as earned.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A flag ID must be canonical and a definition row cannot create an active flag by itself.
- SetFlag is idempotent; repeated resolution or restore cannot duplicate a ledger effect.
- Quest/branch gates read the canonical owner and return a named missing-flag result.
- A future delayed callback must use an existing persisted fact or a separately claimed save shape, never a panel timer.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `moral_choice_flags.json` is the sole definition catalog.
- Do not add flag effects, morality thresholds or consequence prose to the definition row unless the current schema and owner support them.
- A new flag needs a producer, consumer, persistence path and content/tone review.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the existing `moral_choice` section and `MoralChoiceState`; no new moral-flags section.
- Capture/restore includes active flags, resolutions, branch locks and pending threshold bits.
- Legacy saves with missing optional flag fields restore empty/neutral and do not re-earn historical choices.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Flag sets use ordinal/id membership and no random selection.
- Moral choice RNG is the existing campaign stream; definitions and display labels are deterministic.
- Replay compares active flags, resolutions, branch locks, ledger facts and downstream projections.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Moral choice resolution emits the current fact after the flag mutation and state change.
- Branch lock and threshold events consume current moral state, not panel copies.
- Gossip/faction reactions consume resolved choice/flag facts through their existing owners.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Main.MoralChoice.cs
- src/Host/MoralChoiceSaveStore.cs
- src/UI/MoralChoiceModal.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Ethical memory should be specific and restrained, not a score disguised as a moral verdict.
- A flag records a player action; downstream prose must not claim an outcome the current owner did not resolve.
- Avoid copied real-world political or religious language; keep the established fictional tone.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A catalog definition is shown as active without an owner fact. | MoralChoiceFlagCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A choice sets a flag in a panel but not in MoralChoiceState. | MoralChoiceIds | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Restore replays a flag into the ledger twice. | MoralChoiceSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A branch gate reads a shadow list and diverges. | MoralChoiceState/MoralChoiceSave | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A delayed callback fires from wall-clock time after reload. | Main.MoralChoice/MoralChoicePanel | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceFlagPlan125Tests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan124_125WarMoralIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — definition/state census | Read 25 rows, IDs, state, system, save and host. | Definitions and active state are separated. | No production path until the owning implementation package is separately claimed. |
| 1 — producer/consumer matrix | Trace representative set_flag values and downstream gates. | Every live flag has an owner path or a recorded gap. | No production path until the owning implementation package is separately claimed. |
| 2 — replay/migration proof | Exercise set, resolve, restore, ledger replay and branch/gossip consumers. | Exactly-once and legacy behavior are proven. | No production path until the owning implementation package is separately claimed. |
| 3 — UI/tone precision | Review history projection and delayed-callback boundary. | No fake moral memory or panel authority. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/moral_choice_flags.json | READ ONLY; MODIFY only for a proven definition/producer gap | 25-row definitions |
| Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs | READ ONLY | Active flag owner |
| Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs | READ ONLY | Persistence state |
| src/Main.MoralChoice.cs | READ ONLY; MODIFY only under a new host claim | Composition/commands |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Creating a second active flag collection. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Treating display definitions as gameplay state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding unseeded delayed callbacks. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Changing moral score/branch gates while fixing a label. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new moral flag system.
- No arbitrary flag growth.
- No new save section.
- No production/data/test/UI changes in this planning package.

# 23. Rollback and Recovery

- Revert the planning document.
- Future moral-choice changes retain current save fixtures and flag/gossip tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 25 definitions and the active-flag owner are distinguished.
- Producer, consumer, save, replay and UI contracts are explicit.
- No parallel moral ledger or unowned delayed effect is proposed.
- Focused commands and limitations are named.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 10→25 target with a 25-row definition/state/producer/consumer matrix.
- Separate immutable flag definitions from mutable active flags and identify the sole owner for each downstream gate.
- Trace representative producers from authored choice `set_flag` fields through the current resolution/save route.
- Audit delayed callbacks, branch locks, faction reactions, gossip and UI/history projections for truthful flag availability without creating a parallel ledger.

## MUST NOT DO

- No new moral flag system.
- No arbitrary flag growth.
- No new save section.
- No production/data/test/UI changes in this planning package.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceFlagPlan125Tests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan124_125WarMoralIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — definition/state census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: authored flag definitions → MoralChoiceFlagCatalogLoader; canonical flag ID vocabulary → MoralChoiceIds; flag mutation, quest gates and resolution facts → MoralChoiceSystem; persistent active flags and resolutions → MoralChoiceState/MoralChoiceSave; host composition and player presentation → Main.MoralChoice/MoralChoicePanel; catalog, branch/gossip and flag-state proof → Moral focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 125.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 125 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by MoralChoiceFlagCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagCatalogLoader.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 59 lines / 2010 bytes.
- SHA-256: `db33abbac6c7bb582f3d17a592dc6a05fab119cf1b85be7f417b7c20a927b115`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MoralChoiceFlagCatalogContainer
public int schema_version = 1;
public string description = string.Empty;
public List<MoralFlagRecord> flags = new List<MoralFlagRecord>();
public sealed class MoralFlagRecord
public string id = string.Empty;
public string display_name = string.Empty;
public static class MoralChoiceFlagCatalogLoader
public const string DefaultFileName = "moral_choice_flags.json";
public static MoralChoiceFlagDefinitions Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagDefinitions.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagDefinitions.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 20 lines / 634 bytes.
- SHA-256: `764eda5eb3dce4983316d0a4b40463a539e49aa5ff7cd4a1031e0f733be970de`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MoralChoiceFlagDefinitions
public List<MoralFlagDefinition> Flags { get; set; } = new List<MoralFlagDefinition>();
public sealed class MoralFlagDefinition
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 259 lines / 15870 bytes.
- SHA-256: `92baf733ed69c3c0f1b9881ed7fdfc252ee7567c26f551c1f400a9d5f9261fb1`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class MoralChoiceIds
public const int BaseQuestCount = 68;
public const int ChainQuestCount = 100;
public const int ExpansionQuestCount = 50;
public const int TotalQuestCount = BaseQuestCount + ChainQuestCount + ExpansionQuestCount;
public const string ShareChild = "quest_moral_share_child";
public const string ShareFamily = "quest_moral_share_family";
public const string ShareInjured = "quest_moral_share_injured";
public const string ShareWater = "quest_moral_share_water";
public const string ShareElder = "quest_moral_share_elder";
public const string SharePregnant = "quest_moral_share_pregnant";
public const string ShareRaider = "quest_moral_share_raider";
public const string SharePeacekeeper = "quest_moral_share_peacekeeper";
public const string ShareKeeper = "quest_moral_share_keeper";
public const string ShareBanditLeader = "quest_moral_share_bandit_leader";
public const string ShareScientist = "quest_moral_share_scientist";
public const string ShareFarmer = "quest_moral_share_farmer";
public const string ShareScavengerChild = "quest_moral_env_scavenger_child";
public const string ListenOldMan = "quest_moral_listen_oldman";
public const string ListenMother = "quest_moral_listen_mother";
public const string ListenSoldier = "quest_moral_listen_soldier";
public const string ListenChild = "quest_moral_listen_child";
public const string ListenDoctor = "quest_moral_listen_doctor";
public const string ListenPreacher = "quest_moral_listen_preacher";
public const string ListenEngineer = "quest_moral_listen_engineer";
public const string ListenWarning = "quest_moral_listen_warning";
public const string ListenLover = "quest_moral_listen_lover";
public const string ListenTeacher = "quest_moral_listen_teacher";
public const string ListenThief = "quest_moral_listen_thief";
public const string ListenProphet = "quest_moral_listen_prophet";
public const string ListenBuriedLetters = "quest_moral_env_buried_letters";
public const string ComfortWidow = "quest_moral_comfort_widow";
public const string ComfortChild = "quest_moral_comfort_child";
public const string ComfortInjured = "quest_moral_comfort_injured";
public const string ComfortFear = "quest_moral_comfort_fear";
public const string ComfortAddict = "quest_moral_comfort_addict";
public const string ComfortGuilt = "quest_moral_comfort_guilt";
public const string ComfortElder = "quest_moral_comfort_elder";
public const string ComfortNightmare = "quest_moral_comfort_nightmare";
public const string ComfortLoneliness = "quest_moral_comfort_loneliness";
public const string ComfortAnger = "quest_moral_comfort_anger";
public const string ComfortHope = "quest_moral_comfort_hope";
public const string ComfortDespair = "quest_moral_comfort_despair";
public const string ComfortWoundedScavenger = "quest_moral_env_wounded_scavenger";
public const string DeadUnmarked = "quest_moral_dead_unmarked";
public const string DeadBurned = "quest_moral_dead_burned";
public const string DeadBloated = "quest_moral_dead_bloated";
public const string DeadChild = "quest_moral_dead_child";
public const string DeadMass = "quest_moral_dead_mass";
public const string DeadHanged = "quest_moral_dead_hanged";
public const string DeadCloset = "quest_moral_dead_closet";
public const string DeadWater = "quest_moral_dead_water";
public const string DeadCremated = "quest_moral_dead_cremated";
public const string DeadExecuted = "quest_moral_dead_executed";
public const string DeadSuicide = "quest_moral_dead_suicide";
public const string DeadMassacre = "quest_moral_dead_massacre";
public const string DeadExplorer = "quest_moral_env_dead_explorer";
public const string TrustFire = "quest_moral_trust_fire";
public const string TrustWounded = "quest_moral_trust_wounded";
public const string TrustMerchant = "quest_moral_trust_merchant";
public const string TrustChild = "quest_moral_trust_child";
public const string TrustDeserter = "quest_moral_trust_deserter";
public const string TrustWoman = "quest_moral_trust_woman";
public const string TrustSoldier = "quest_moral_trust_soldier";
public const string TrustRunaway = "quest_moral_trust_runaway";
public const string TrustSilent = "quest_moral_trust_silent";
public const string TrustSignal = "quest_moral_trust_signal";
public const string TrustBorrower = "quest_moral_trust_borrower";
public const string TrustMessenger = "quest_moral_trust_messenger";
public const string TrustShelterRefugee = "quest_moral_env_shelter_refugee";
public static readonly string[] ChainMercy = Enumerable.Range(1, 25) .Select(i => $"quest_moral_chain_mercy_{i:D2}").ToArray();
public static readonly string[] ChainIron = Enumerable.Range(1, 25) .Select(i => $"quest_moral_chain_iron_{i:D2}").ToArray();
public static readonly string[] ChainListen = Enumerable.Range(1, 25) .Select(i => $"quest_moral_chain_listen_{i:D2}").ToArray();
public static readonly string[] ChainBetray = Enumerable.Range(1, 25) .Select(i => $"quest_moral_chain_betray_{i:D2}").ToArray();
public static readonly string[] AllChain = ChainMercy.Concat(ChainIron).Concat(ChainListen).Concat(ChainBetray).ToArray();
public static readonly string[] AllExpansion = {
public static readonly string[] All = {
public const string TrapPreyHigh = "quest_moral_trap_prey_high";
public const string TrapPreyMedium = "quest_moral_trap_prey_medium";
public const string TrapPreyLow = "quest_moral_trap_prey_low";
public const string FlagMercyRoadLocked = "flag_branch_mercy_road_locked";
public const string FlagIronWayLocked = "flag_branch_iron_way_locked";
public const string FlagListenerLocked = "flag_branch_listener_locked";
public const string FlagBrokenCompactLocked = "flag_branch_broken_compact_locked";
public const string FlagBetrayedAlly = "flag_betrayed_ally";
public const string FlagBetrayedFaction = "flag_betrayed_faction";
public const string FlagBetrayedTrust = "flag_betrayed_trust";
public const string FlagBrokenPact = "flag_broken_pact";
public const string FlagBecomeWarlord = "flag_become_warlord";
public const string FlagThroneOfAsh = "flag_throne_of_ash";
public const string FlagSparedRaider = "flag_spared_raider";
public const string FlagExecutedPrisoner = "flag_executed_prisoner";
public const string FlagSharedRations = "flag_shared_rations";
public const string FlagHoardedMedicine = "flag_hoarded_medicine";
public const string FlagShelteredRefugee = "flag_sheltered_refugee";
public const string FlagExpelledSurvivor = "flag_expelled_survivor";
public const string FlagRepairedInfrastructure = "flag_repaired_infrastructure";
public const string FlagSabotagedRival = "flag_sabotaged_rival";
public const string FlagBrokeTreaty = "flag_broke_treaty";
public const string FlagHonoredDebt = "flag_honored_debt";
public const string FlagIgnoredDistress = "flag_ignored_distress";
public const string FlagRespondedDistress = "flag_responded_distress";
public const string FlagForgedRecord = "flag_forged_record";
public const string FlagPreservedArchive = "flag_preserved_archive";
public const string FlagChosenFactionSide = "flag_chosen_faction_side";
public const string FlagMessengerKept = "flag_moral_messenger_kept";
public static readonly string[] AllFlags = {
public const string BranchMercyRoad = "branch_mercy_road";
public const string BranchIronWay = "branch_iron_way";
public const string BranchListenerThread = "branch_listener_thread";
public const string BranchBrokenCompact = "branch_broken_compact";
public static readonly string[] AllBranches = {
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 687 lines / 30717 bytes.
- SHA-256: `4cb9adafbbbdfc153d1c80ac2595f1e6c870a6669e2c9e85844a6a416133c9c1`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum MoralPathBand
public enum MoralEndingKind
public sealed class MoralChoiceQuestDefinition
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string Category { get; set; } = string.Empty;
public string Trigger { get; set; } = string.Empty;
public string Discovery { get; set; } = string.Empty;
public string LocationId { get; set; } = string.Empty;
public int MinDay { get; set; }
public int MaxDay { get; set; }
public List<MoralChoiceOption> Choices { get; set; } = new List<MoralChoiceOption>();
public sealed class MoralChoiceOption
public string Label { get; set; } = string.Empty;
public int MoralDelta { get; set; }
public int EmpathyDelta { get; set; }
public string SetFlag { get; set; } = string.Empty;
public string OutcomeText { get; set; } = string.Empty;
public string Epitaph { get; set; } = string.Empty;
public sealed class MoralChoiceSystem
public const string SystemId = "moral_choice";
public const string QuestIdPrefix = "quest_moral_";
public const int MinScore = -200;
public const int MaxScore = 200;
public const int ListenerEmpathyThreshold = 15;
public const int ConfidantEmpathyThreshold = 30;
public const int StorykeeperEmpathyThreshold = 45;
public const int StorykeeperQuestThreshold = 25;
public const int EndingLockMinQuests = 20;
public const string EventLegendPositive = "moral_event_legend_positive";
public const string EventLegendNegative = "moral_event_legend_negative";
public const string EventBountyIssued = "moral_event_bounty_issued";
public const string EventContractTaken = "moral_event_contract_taken";
public const string EventContractRaised = "moral_event_contract_raised";
public const string EventPatrolDefense = "moral_event_patrol_defense";
public const int LegendPositiveFlag = 1;
public const int LegendNegativeFlag = 2;
public event Action<MoralChoiceResolution>? OnQuestResolved;
public event Action<string>? OnThresholdEventFired;
public event Action<string>? OnBranchLocked;
public MoralChoiceState State => _state;
public int MoralScore => _state.moralScore;
public int EmpathyPoints => _state.empathyPoints;
public int QuestsResolved => _state.resolutions.Count;
public MoralPathBand CurrentBand => BandForScore(_state.moralScore);
public IReadOnlyList<MoralChoiceResolution> Resolutions => _state.resolutions;
public bool IsListener => _state.empathyPoints >= ListenerEmpathyThreshold;
public bool IsConfidant => _state.empathyPoints >= ConfidantEmpathyThreshold;
public MoralChoiceChainData? ChainData => _chainData;
public void InitializeChainData(MoralChoiceChainData chainData) {
public static bool IsCanonicalQuestId(string questId) =>
public static bool IsAvailableOnDay(MoralChoiceQuestDefinition quest, int day) =>
public bool IsResolved(string questId) => TryGetResolution(questId, out _);
public bool TryGetResolution(string questId, out MoralChoiceResolution? resolution) {
public void RegisterQuest(MoralChoiceQuestDefinition def) {
public void RegisterQuests(IEnumerable<MoralChoiceQuestDefinition> defs) {
public IReadOnlyDictionary<string, MoralChoiceQuestDefinition> Catalog => _catalog;
public int CatalogCount => _catalog.Count;
public MoralChoiceQuestDefinition? GetQuest(string id) =>
public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyOffers(int day, int maxOffers = 1) {
public bool TryResolve(string questId, int choiceIndex, string locationId, int day, out MoralResolveResult result) {
public bool IsBranchLocked(string branchId) =>
public int GetBranchProgress(string branchId) =>
public string GetQuestBranch(string questId) =>
public bool IsChainQuestAccessible(string questId, int day) {
public bool EvaluateGate(MoralQuestGate gate) {
public void SetFlag(string flagId) {
public bool HasFlag(string flagId) =>
public List<MoralEchoQuestDefinition> FindAvailableEchoQuests(int currentDay) {
public void MarkEchoQuestFired(string echoQuestId) {
public MoralChoiceResolution Resolve(MoralChoiceQuestDefinition quest, int choiceIndex, string locationId, int day) {
public void Reconcile(int day) {
public MoralEndingKind SelectEnding() =>
public static MoralEndingKind SelectEnding(int moralScore, int empathyPoints, int questsResolved) {
public static MoralPathBand BandForScore(int score) {
public MoralChoiceState CaptureState() => Clone(_state);
public void RestoreState(MoralChoiceState state) {
```


# Appendix B.06 — Current Code Architecture: `Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 76 lines / 3245 bytes.
- SHA-256: `67dec17ef0a93d7516a0ddf681661c46e8cd566d8a0602fabc77426430f027b5`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MoralChoiceState
public string systemId = MoralChoiceSystem.SystemId;
public int schemaVersion = 1;
public int moralScore;
public int empathyPoints;
public List<MoralChoiceResolution> resolutions = new List<MoralChoiceResolution>();
public int lastReconciledDay = -1;
public int bandAtLastReconcile = -1;
public List<string> firedThresholdEvents = new List<string>();
public int pendingLegendFlags;
public Dictionary<string, int> branchProgress = new Dictionary<string, int>();
public List<string> lockedBranches = new List<string>();
public List<string> firedEchoQuests = new List<string>();
public List<string> activeFlags = new List<string>();
public sealed class MoralChoiceResolution
public string questId = string.Empty;
public string locationId = string.Empty;
public int resolvedDay = -1;
public int choiceIndex = -1;
public int moralDelta;
public int empathyDelta;
public string impactMark = "flat";
public int outcomeRoll = -1;
public int propagatesOnDay = -1;
public string epitaph = string.Empty;
```


# Appendix B.07 — Current Code Architecture: `Assets/Ashfall.Core/Flags/IFlagLedger.cs`

### `Assets/Ashfall.Core/Flags/IFlagLedger.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 63 lines / 2518 bytes.
- SHA-256: `39a8e52d852cf5501cba6a4568b29d21d4203b86db239249e9de91470cd72ea6`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public interface IFlagLedger
public sealed class InMemoryFlagLedger : IFlagLedger
public bool IsSet(string flagId) {
public void Set(string flagId, string originSystem = "", string sourceEvent = "", int day = 0, string subjectId = "") {
public void Clear(string flagId) {
public int GetCounter(string counterId) {
public void Increment(string counterId, int amount = 1, string originSystem = "", string sourceEvent = "", int day = 0, string subjectId = "") {
public void SetCounter(string counterId, int value, string originSystem = "", string sourceEvent = "", int day = 0, string subjectId = "") {
```


# Appendix B.08 — Current Code Architecture: `src/Main.MoralChoice.cs`

### `src/Main.MoralChoice.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 344 lines / 16147 bytes.
- SHA-256: `e12534c6d2a8cd86c90d7283de173c51bf07cd68e46bb52f6129bbeb05375b8d`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=4; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public MoralChoiceSystem MoralChoice => _moralChoice;
public IReadOnlyList<MoralChoiceQuestDefinition> MoralChoiceDefs => _moralChoiceDefs;
public MoralChoiceQuestDefinition? GetMoralChoiceDef(string questId) {
public List<MoralChoiceQuestDefinition> GetAvailableMoralChoices() {
public const string TrappingMoralQuestIdPrefix = "quest_moral_trap_prey_";
public List<MoralChoiceQuestDefinition> GetResolvedMoralChoices() {
public MoralChoiceResolution? GetMoralChoiceResolution(string questId) {
public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyMoralOffers(int maxOffers = 1) {
public bool TryResolveMoralChoice(string questId, int choiceIndex) {
```


# Appendix B.09 — Current Code Architecture: `src/Host/MoralChoiceSaveStore.cs`

### `src/Host/MoralChoiceSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 45 lines / 1930 bytes.
- SHA-256: `0c4d6da4e24f76023002cf48e24931f967220096c74954e6f8f33eea949baf3b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class MoralChoiceSaveStore
public const string FileName = "moral_choice_save.json";
public const string SectionName = "moral_choice";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static void Save(MoralChoiceState state, string? pathOverride = null) {
public static MoralChoiceState? TryLoad(string? pathOverride = null) {
public static string TryCapturePersisted(MoralChoiceState state) => s_store.CapturePersisted(state);
```


# Appendix B.10 — Current Code Architecture: `src/UI/MoralChoiceModal.cs`

### `src/UI/MoralChoiceModal.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 317 lines / 14077 bytes.
- SHA-256: `ef9e9b6e34bb952eb6ac37133cccd919dfc0ef4135b4831ff0085ff6662da334`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=6; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class MoralChoiceModal : Control, IModalPanel
public event Action<string, int>? OnChoiceSelected;
public event Action? OnClose;
public event Action? OnModalClosed;
public bool IsModalOpen => Visible;
public Control? InitialFocusControl => _firstInteractiveButton ?? _closeButton;
public override void _Ready() {
public void Bind( MoralChoiceQuestDefinition quest, MoralChoiceSystem? moralChoiceSystem = null, Action<string, int>? onChoiceCallback = null) {
public void RefreshContent() {
public void Open() {
public void SelectChoiceForTest(int choiceIndex) => ExecuteChoice(choiceIndex);
public void CloseModal() {
public override void _UnhandledInput(InputEvent @event) {
```


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/moral_choice_flags.json`

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


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/moral_choice_quests.json`

### `Assets/StreamingAssets/Data/moral_choice_quests.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 141249 bytes / 141245 characters.
- SHA-256: `1c84bf9e37036b9247ba0f48e1a14a8d1b497a0bcc63e0e506f23f8bb399b4e1`.
- Root keys: `quests`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
quests: min=68, max=68, observed_paths=1
quests[].choices: min=4, max=4, observed_paths=2
```

Representative record fields:

- `category`
- `choices`
- `discovery`
- `display_name`
- `id`
- `location_id`
- `max_day`
- `min_day`
- `trigger`

Representative identifiers (ordered, capped for readability):

```text
quest_moral_share_child
quest_moral_share_family
quest_moral_share_injured
quest_moral_share_water
quest_moral_share_elder
quest_moral_share_pregnant
quest_moral_share_raider
quest_moral_share_peacekeeper
quest_moral_share_keeper
quest_moral_share_bandit_leader
quest_moral_share_scientist
quest_moral_share_farmer
quest_moral_listen_oldman
quest_moral_listen_mother
quest_moral_listen_soldier
quest_moral_listen_child
quest_moral_listen_doctor
quest_moral_listen_preacher
quest_moral_listen_engineer
quest_moral_listen_warning
quest_moral_listen_lover
quest_moral_listen_teacher
quest_moral_listen_thief
quest_moral_listen_prophet
quest_moral_comfort_widow
quest_moral_comfort_child
quest_moral_comfort_injured
quest_moral_comfort_fear
quest_moral_comfort_addict
quest_moral_comfort_guilt
quest_moral_comfort_elder
quest_moral_comfort_nightmare
quest_moral_comfort_loneliness
quest_moral_comfort_anger
quest_moral_comfort_hope
quest_moral_comfort_despair
quest_moral_dead_unmarked
quest_moral_dead_burned
quest_moral_dead_bloated
quest_moral_dead_child
quest_moral_dead_mass
quest_moral_dead_hanged
quest_moral_dead_closet
quest_moral_dead_water
quest_moral_dead_cremated
quest_moral_dead_executed
quest_moral_dead_suicide
quest_moral_dead_massacre
quest_moral_trust_fire
quest_moral_trust_wounded
quest_moral_trust_merchant
quest_moral_trust_child
quest_moral_trust_deserter
quest_moral_trust_woman
quest_moral_trust_soldier
quest_moral_trust_runaway
quest_moral_trust_silent
quest_moral_trust_signal
quest_moral_trust_borrower
quest_moral_trust_messenger
quest_moral_env_scavenger_child
quest_moral_env_buried_letters
quest_moral_env_shelter_refugee
quest_moral_env_dead_explorer
quest_moral_env_wounded_scavenger
quest_moral_trap_prey_high
quest_moral_trap_prey_medium
quest_moral_trap_prey_low
```


# Appendix C.13 — Catalog Census: `Assets/StreamingAssets/Data/moral_choice_chains.json`

### `Assets/StreamingAssets/Data/moral_choice_chains.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 31788 bytes / 31784 characters.
- SHA-256: `2a30c42686baef9f1e64cdda74fdb13cbe9204c641790d47212e5aeb48fc2135`.
- Root keys: `branches`, `description`, `echo_quests`, `faction_reactions`, `gossip_propagation`, `lockout_rules`, `merge_rules`, `quest_gates`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
branches: min=4, max=4, observed_paths=1
branches[].entry_quests: min=3, max=3, observed_paths=2
branches[].locks_out: min=2, max=2, observed_paths=2
branches[].merge_allowed: min=1, max=1, observed_paths=2
echo_quests.quests: min=60, max=60, observed_paths=1
quest_gates: min=88, max=88, observed_paths=1
quest_gates[].requires: min=1, max=1, observed_paths=2
```

Representative record fields:

- `description`
- `display_name`
- `entry_quests`
- `id`
- `lock_threshold`
- `locked_flag`
- `locks_out`
- `merge_allowed`

Representative identifiers (ordered, capped for readability):

```text
branch_mercy_road
branch_iron_way
branch_listener_thread
branch_broken_compact
```


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/MoralChoiceFlagPlan125Tests.cs`

### `Ashfall.Core.Tests/MoralChoiceFlagPlan125Tests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 137; SHA-256: `3ab7d00efd1e4ccba071b0bab44ab7b9cac89422dab5b463ec0e7c86b7495307`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CatalogContainsExactlyTwentyFiveUniqueHistoricalFlags
CatalogIdsAreSynchronizedWithStaticIds
EveryPlan125FlagHasARealMoralChoiceProducer
ConfiguredProducerWritesFlagOnlyAfterResolutionAndIsIdempotent
OldStateLeavesNewFlagsUnsetAndRoundTripsExistingHistory
RaiderResolutionKeepsSameIncidentChoicesMutuallyExclusive
```


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/World/Plan124_125WarMoralIntegrationTests.cs`

### `Ashfall.Core.Tests/World/Plan124_125WarMoralIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 215; SHA-256: `646d2c4f7cf0a53de3c0bceaf523f2a477f4d40de80a20d6dd5a9e2ad9741347`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan124_FactionWarLocationOverrides_LoadsExactTwentyOverridesAndVerifiesExpansionTier
Plan125_MoralChoiceFlags_LoadsExactTwentyFiveFlagsAndValidatesAllExpansionFlags
Plan124_125_CrossDomainCoherence_WarDevastationAndMoralReactivityLinkages
Plan124_125_DeterministicDayProgressionAndFlagResolution
```


# Appendix D.16 — Existing Focused Test Inventory: `Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs`

### `Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs`

- Current test declarations: Fact=36, Theory=0, InlineData=0.
- File lines: 681; SHA-256: `52feaf16447c30403ed9a14ec9a3c991dfb6d2c0ec873633e0821e42cf5d1df3`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ChainCatalogLoadsFourBranches
ChainCatalogHasQuestGates
ChainCatalogHasEchoQuests
ChainCatalogLockoutRulesArePermanent
ChainCatalogMissingFileReturnsEmpty
BranchingQuestsLoadAllFourChains
BranchingQuestsAllChainsComplete
BranchingQuestsHaveValidChoices
ExpansionQuestsLoadFiftyQuests
ExpansionQuestIdsMatchStaticList
GossipCatalogLoadsAllBands
GossipCatalogHasNpcGreetings
GossipCatalogHasDecayRules
FactionReactionsLoadAllThresholdEvents
FactionReactionsHaveDialogue
FlagCatalogLoadsTwentyFiveFlags
FlagCatalogIdsMatchStaticList
BranchTracking_LocksOutOpposingBranches
BranchTracking_LockedBranchBlocksAccessibility
BranchTracking_GateRequiresMoralThreshold
BranchTracking_GateRequiresPriorQuestResolved
BranchTracking_BranchLockFlagsAreSet
EchoQuests_AvailableAfterTriggerAndDelay
EchoQuests_NotAvailableForWrongChoice
EchoQuests_MarkFiredPreventsRefire
GossipRuntime_ReturnsCorrectBandChatter
GossipRuntime_PickReturnsNonEmpty
GossipRuntime_DecayToNeutralAfterFullDecay
GossipRuntime_DecayOneLevelAfterInterval
GossipRuntime_StaysNeutralBeforePropagation
SaveRoundTrip_PreservesBranchTracking
StaticIds_AllChainHasOneHundredEntries
StaticIds_AllExpansionHasFiftyEntries
StaticIds_AllFlagsHasTwentySixEntries
StaticIds_AllBranchesHasFourEntries
StaticIds_ChainQuestsFollowNamingPattern
```


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagCatalogLoader.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 59 lines / 2010 bytes.
- SHA-256: `db33abbac6c7bb582f3d17a592dc6a05fab119cf1b85be7f417b7c20a927b115`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MoralChoiceFlagCatalogContainer
public int schema_version = 1;
public string description = string.Empty;
public List<MoralFlagRecord> flags = new List<MoralFlagRecord>();
public sealed class MoralFlagRecord
public string id = string.Empty;
public string display_name = string.Empty;
public static class MoralChoiceFlagCatalogLoader
public const string DefaultFileName = "moral_choice_flags.json";
public static MoralChoiceFlagDefinitions Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagDefinitions.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagDefinitions.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 20 lines / 634 bytes.
- SHA-256: `764eda5eb3dce4983316d0a4b40463a539e49aa5ff7cd4a1031e0f733be970de`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MoralChoiceFlagDefinitions
public List<MoralFlagDefinition> Flags { get; set; } = new List<MoralFlagDefinition>();
public sealed class MoralFlagDefinition
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
```


# Appendix E.19 — Supporting Code Evidence: `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 259 lines / 15870 bytes.
- SHA-256: `92baf733ed69c3c0f1b9881ed7fdfc252ee7567c26f551c1f400a9d5f9261fb1`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class MoralChoiceIds
public const int BaseQuestCount = 68;
public const int ChainQuestCount = 100;
public const int ExpansionQuestCount = 50;
public const int TotalQuestCount = BaseQuestCount + ChainQuestCount + ExpansionQuestCount;
public const string ShareChild = "quest_moral_share_child";
public const string ShareFamily = "quest_moral_share_family";
public const string ShareInjured = "quest_moral_share_injured";
public const string ShareWater = "quest_moral_share_water";
public const string ShareElder = "quest_moral_share_elder";
public const string SharePregnant = "quest_moral_share_pregnant";
public const string ShareRaider = "quest_moral_share_raider";
public const string SharePeacekeeper = "quest_moral_share_peacekeeper";
public const string ShareKeeper = "quest_moral_share_keeper";
public const string ShareBanditLeader = "quest_moral_share_bandit_leader";
public const string ShareScientist = "quest_moral_share_scientist";
public const string ShareFarmer = "quest_moral_share_farmer";
public const string ShareScavengerChild = "quest_moral_env_scavenger_child";
public const string ListenOldMan = "quest_moral_listen_oldman";
public const string ListenMother = "quest_moral_listen_mother";
public const string ListenSoldier = "quest_moral_listen_soldier";
public const string ListenChild = "quest_moral_listen_child";
public const string ListenDoctor = "quest_moral_listen_doctor";
public const string ListenPreacher = "quest_moral_listen_preacher";
public const string ListenEngineer = "quest_moral_listen_engineer";
public const string ListenWarning = "quest_moral_listen_warning";
public const string ListenLover = "quest_moral_listen_lover";
public const string ListenTeacher = "quest_moral_listen_teacher";
public const string ListenThief = "quest_moral_listen_thief";
public const string ListenProphet = "quest_moral_listen_prophet";
public const string ListenBuriedLetters = "quest_moral_env_buried_letters";
public const string ComfortWidow = "quest_moral_comfort_widow";
public const string ComfortChild = "quest_moral_comfort_child";
public const string ComfortInjured = "quest_moral_comfort_injured";
public const string ComfortFear = "quest_moral_comfort_fear";
public const string ComfortAddict = "quest_moral_comfort_addict";
public const string ComfortGuilt = "quest_moral_comfort_guilt";
public const string ComfortElder = "quest_moral_comfort_elder";
public const string ComfortNightmare = "quest_moral_comfort_nightmare";
public const string ComfortLoneliness = "quest_moral_comfort_loneliness";
public const string ComfortAnger = "quest_moral_comfort_anger";
public const string ComfortHope = "quest_moral_comfort_hope";
public const string ComfortDespair = "quest_moral_comfort_despair";
public const string ComfortWoundedScavenger = "quest_moral_env_wounded_scavenger";
public const string DeadUnmarked = "quest_moral_dead_unmarked";
public const string DeadBurned = "quest_moral_dead_burned";
public const string DeadBloated = "quest_moral_dead_bloated";
public const string DeadChild = "quest_moral_dead_child";
public const string DeadMass = "quest_moral_dead_mass";
public const string DeadHanged = "quest_moral_dead_hanged";
public const string DeadCloset = "quest_moral_dead_closet";
public const string DeadWater = "quest_moral_dead_water";
public const string DeadCremated = "quest_moral_dead_cremated";
public const string DeadExecuted = "quest_moral_dead_executed";
public const string DeadSuicide = "quest_moral_dead_suicide";
public const string DeadMassacre = "quest_moral_dead_massacre";
public const string DeadExplorer = "quest_moral_env_dead_explorer";
public const string TrustFire = "quest_moral_trust_fire";
public const string TrustWounded = "quest_moral_trust_wounded";
public const string TrustMerchant = "quest_moral_trust_merchant";
public const string TrustChild = "quest_moral_trust_child";
public const string TrustDeserter = "quest_moral_trust_deserter";
public const string TrustWoman = "quest_moral_trust_woman";
public const string TrustSoldier = "quest_moral_trust_soldier";
public const string TrustRunaway = "quest_moral_trust_runaway";
public const string TrustSilent = "quest_moral_trust_silent";
public const string TrustSignal = "quest_moral_trust_signal";
public const string TrustBorrower = "quest_moral_trust_borrower";
public const string TrustMessenger = "quest_moral_trust_messenger";
public const string TrustShelterRefugee = "quest_moral_env_shelter_refugee";
public static readonly string[] ChainMercy = Enumerable.Range(1, 25) .Select(i => $"quest_moral_chain_mercy_{i:D2}").ToArray();
public static readonly string[] ChainIron = Enumerable.Range(1, 25) .Select(i => $"quest_moral_chain_iron_{i:D2}").ToArray();
public static readonly string[] ChainListen = Enumerable.Range(1, 25) .Select(i => $"quest_moral_chain_listen_{i:D2}").ToArray();
public static readonly string[] ChainBetray = Enumerable.Range(1, 25) .Select(i => $"quest_moral_chain_betray_{i:D2}").ToArray();
public static readonly string[] AllChain = ChainMercy.Concat(ChainIron).Concat(ChainListen).Concat(ChainBetray).ToArray();
public static readonly string[] AllExpansion = {
public static readonly string[] All = {
public const string TrapPreyHigh = "quest_moral_trap_prey_high";
public const string TrapPreyMedium = "quest_moral_trap_prey_medium";
public const string TrapPreyLow = "quest_moral_trap_prey_low";
public const string FlagMercyRoadLocked = "flag_branch_mercy_road_locked";
public const string FlagIronWayLocked = "flag_branch_iron_way_locked";
public const string FlagListenerLocked = "flag_branch_listener_locked";
public const string FlagBrokenCompactLocked = "flag_branch_broken_compact_locked";
public const string FlagBetrayedAlly = "flag_betrayed_ally";
public const string FlagBetrayedFaction = "flag_betrayed_faction";
public const string FlagBetrayedTrust = "flag_betrayed_trust";
public const string FlagBrokenPact = "flag_broken_pact";
public const string FlagBecomeWarlord = "flag_become_warlord";
public const string FlagThroneOfAsh = "flag_throne_of_ash";
public const string FlagSparedRaider = "flag_spared_raider";
public const string FlagExecutedPrisoner = "flag_executed_prisoner";
public const string FlagSharedRations = "flag_shared_rations";
public const string FlagHoardedMedicine = "flag_hoarded_medicine";
public const string FlagShelteredRefugee = "flag_sheltered_refugee";
public const string FlagExpelledSurvivor = "flag_expelled_survivor";
public const string FlagRepairedInfrastructure = "flag_repaired_infrastructure";
public const string FlagSabotagedRival = "flag_sabotaged_rival";
public const string FlagBrokeTreaty = "flag_broke_treaty";
public const string FlagHonoredDebt = "flag_honored_debt";
public const string FlagIgnoredDistress = "flag_ignored_distress";
public const string FlagRespondedDistress = "flag_responded_distress";
public const string FlagForgedRecord = "flag_forged_record";
public const string FlagPreservedArchive = "flag_preserved_archive";
public const string FlagChosenFactionSide = "flag_chosen_faction_side";
public const string FlagMessengerKept = "flag_moral_messenger_kept";
public static readonly string[] AllFlags = {
public const string BranchMercyRoad = "branch_mercy_road";
public const string BranchIronWay = "branch_iron_way";
public const string BranchListenerThread = "branch_listener_thread";
public const string BranchBrokenCompact = "branch_broken_compact";
public static readonly string[] AllBranches = {
```


# Appendix E.20 — Supporting Code Evidence: `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 687 lines / 30717 bytes.
- SHA-256: `4cb9adafbbbdfc153d1c80ac2595f1e6c870a6669e2c9e85844a6a416133c9c1`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum MoralPathBand
public enum MoralEndingKind
public sealed class MoralChoiceQuestDefinition
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string Category { get; set; } = string.Empty;
public string Trigger { get; set; } = string.Empty;
public string Discovery { get; set; } = string.Empty;
public string LocationId { get; set; } = string.Empty;
public int MinDay { get; set; }
public int MaxDay { get; set; }
public List<MoralChoiceOption> Choices { get; set; } = new List<MoralChoiceOption>();
public sealed class MoralChoiceOption
public string Label { get; set; } = string.Empty;
public int MoralDelta { get; set; }
public int EmpathyDelta { get; set; }
public string SetFlag { get; set; } = string.Empty;
public string OutcomeText { get; set; } = string.Empty;
public string Epitaph { get; set; } = string.Empty;
public sealed class MoralChoiceSystem
public const string SystemId = "moral_choice";
public const string QuestIdPrefix = "quest_moral_";
public const int MinScore = -200;
public const int MaxScore = 200;
public const int ListenerEmpathyThreshold = 15;
public const int ConfidantEmpathyThreshold = 30;
public const int StorykeeperEmpathyThreshold = 45;
public const int StorykeeperQuestThreshold = 25;
public const int EndingLockMinQuests = 20;
public const string EventLegendPositive = "moral_event_legend_positive";
public const string EventLegendNegative = "moral_event_legend_negative";
public const string EventBountyIssued = "moral_event_bounty_issued";
public const string EventContractTaken = "moral_event_contract_taken";
public const string EventContractRaised = "moral_event_contract_raised";
public const string EventPatrolDefense = "moral_event_patrol_defense";
public const int LegendPositiveFlag = 1;
public const int LegendNegativeFlag = 2;
public event Action<MoralChoiceResolution>? OnQuestResolved;
public event Action<string>? OnThresholdEventFired;
public event Action<string>? OnBranchLocked;
public MoralChoiceState State => _state;
public int MoralScore => _state.moralScore;
public int EmpathyPoints => _state.empathyPoints;
public int QuestsResolved => _state.resolutions.Count;
public MoralPathBand CurrentBand => BandForScore(_state.moralScore);
public IReadOnlyList<MoralChoiceResolution> Resolutions => _state.resolutions;
public bool IsListener => _state.empathyPoints >= ListenerEmpathyThreshold;
public bool IsConfidant => _state.empathyPoints >= ConfidantEmpathyThreshold;
public MoralChoiceChainData? ChainData => _chainData;
public void InitializeChainData(MoralChoiceChainData chainData) {
public static bool IsCanonicalQuestId(string questId) =>
public static bool IsAvailableOnDay(MoralChoiceQuestDefinition quest, int day) =>
public bool IsResolved(string questId) => TryGetResolution(questId, out _);
public bool TryGetResolution(string questId, out MoralChoiceResolution? resolution) {
public void RegisterQuest(MoralChoiceQuestDefinition def) {
public void RegisterQuests(IEnumerable<MoralChoiceQuestDefinition> defs) {
public IReadOnlyDictionary<string, MoralChoiceQuestDefinition> Catalog => _catalog;
public int CatalogCount => _catalog.Count;
public MoralChoiceQuestDefinition? GetQuest(string id) =>
public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyOffers(int day, int maxOffers = 1) {
public bool TryResolve(string questId, int choiceIndex, string locationId, int day, out MoralResolveResult result) {
public bool IsBranchLocked(string branchId) =>
public int GetBranchProgress(string branchId) =>
public string GetQuestBranch(string questId) =>
public bool IsChainQuestAccessible(string questId, int day) {
public bool EvaluateGate(MoralQuestGate gate) {
public void SetFlag(string flagId) {
public bool HasFlag(string flagId) =>
public List<MoralEchoQuestDefinition> FindAvailableEchoQuests(int currentDay) {
public void MarkEchoQuestFired(string echoQuestId) {
public MoralChoiceResolution Resolve(MoralChoiceQuestDefinition quest, int choiceIndex, string locationId, int day) {
public void Reconcile(int day) {
public MoralEndingKind SelectEnding() =>
public static MoralEndingKind SelectEnding(int moralScore, int empathyPoints, int questsResolved) {
public static MoralPathBand BandForScore(int score) {
public MoralChoiceState CaptureState() => Clone(_state);
public void RestoreState(MoralChoiceState state) {
```


# Appendix E.21 — Supporting Code Evidence: `Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 76 lines / 3245 bytes.
- SHA-256: `67dec17ef0a93d7516a0ddf681661c46e8cd566d8a0602fabc77426430f027b5`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MoralChoiceState
public string systemId = MoralChoiceSystem.SystemId;
public int schemaVersion = 1;
public int moralScore;
public int empathyPoints;
public List<MoralChoiceResolution> resolutions = new List<MoralChoiceResolution>();
public int lastReconciledDay = -1;
public int bandAtLastReconcile = -1;
public List<string> firedThresholdEvents = new List<string>();
public int pendingLegendFlags;
public Dictionary<string, int> branchProgress = new Dictionary<string, int>();
public List<string> lockedBranches = new List<string>();
public List<string> firedEchoQuests = new List<string>();
public List<string> activeFlags = new List<string>();
public sealed class MoralChoiceResolution
public string questId = string.Empty;
public string locationId = string.Empty;
public int resolvedDay = -1;
public int choiceIndex = -1;
public int moralDelta;
public int empathyDelta;
public string impactMark = "flat";
public int outcomeRoll = -1;
public int propagatesOnDay = -1;
public string epitaph = string.Empty;
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/MoralChoiceFlagPlan125Tests.cs`

### `Ashfall.Core.Tests/MoralChoiceFlagPlan125Tests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 137; SHA-256: `3ab7d00efd1e4ccba071b0bab44ab7b9cac89422dab5b463ec0e7c86b7495307`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CatalogContainsExactlyTwentyFiveUniqueHistoricalFlags
CatalogIdsAreSynchronizedWithStaticIds
EveryPlan125FlagHasARealMoralChoiceProducer
ConfiguredProducerWritesFlagOnlyAfterResolutionAndIsIdempotent
OldStateLeavesNewFlagsUnsetAndRoundTripsExistingHistory
RaiderResolutionKeepsSameIncidentChoicesMutuallyExclusive
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/World/Plan124_125WarMoralIntegrationTests.cs`

### `Ashfall.Core.Tests/World/Plan124_125WarMoralIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 215; SHA-256: `646d2c4f7cf0a53de3c0bceaf523f2a477f4d40de80a20d6dd5a9e2ad9741347`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan124_FactionWarLocationOverrides_LoadsExactTwentyOverridesAndVerifiesExpansionTier
Plan125_MoralChoiceFlags_LoadsExactTwentyFiveFlagsAndValidatesAllExpansionFlags
Plan124_125_CrossDomainCoherence_WarDevastationAndMoralReactivityLinkages
Plan124_125_DeterministicDayProgressionAndFlagResolution
```


# Appendix G.24 — Supporting Regression Evidence: `Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs`

### `Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs`

- Current test declarations: Fact=36, Theory=0, InlineData=0.
- File lines: 681; SHA-256: `52feaf16447c30403ed9a14ec9a3c991dfb6d2c0ec873633e0821e42cf5d1df3`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ChainCatalogLoadsFourBranches
ChainCatalogHasQuestGates
ChainCatalogHasEchoQuests
ChainCatalogLockoutRulesArePermanent
ChainCatalogMissingFileReturnsEmpty
BranchingQuestsLoadAllFourChains
BranchingQuestsAllChainsComplete
BranchingQuestsHaveValidChoices
ExpansionQuestsLoadFiftyQuests
ExpansionQuestIdsMatchStaticList
GossipCatalogLoadsAllBands
GossipCatalogHasNpcGreetings
GossipCatalogHasDecayRules
FactionReactionsLoadAllThresholdEvents
FactionReactionsHaveDialogue
FlagCatalogLoadsTwentyFiveFlags
FlagCatalogIdsMatchStaticList
BranchTracking_LocksOutOpposingBranches
BranchTracking_LockedBranchBlocksAccessibility
BranchTracking_GateRequiresMoralThreshold
BranchTracking_GateRequiresPriorQuestResolved
BranchTracking_BranchLockFlagsAreSet
EchoQuests_AvailableAfterTriggerAndDelay
EchoQuests_NotAvailableForWrongChoice
EchoQuests_MarkFiredPreventsRefire
GossipRuntime_ReturnsCorrectBandChatter
GossipRuntime_PickReturnsNonEmpty
GossipRuntime_DecayToNeutralAfterFullDecay
GossipRuntime_DecayOneLevelAfterInterval
GossipRuntime_StaysNeutralBeforePropagation
SaveRoundTrip_PreservesBranchTracking
StaticIds_AllChainHasOneHundredEntries
StaticIds_AllExpansionHasFiftyEntries
StaticIds_AllFlagsHasTwentySixEntries
StaticIds_AllBranchesHasFourEntries
StaticIds_ChainQuestsFollowNamingPattern
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
| authored flag definitions | MoralChoiceFlagCatalogLoader | canonical flag ID vocabulary | MoralChoiceIds | Owner emits/reads a typed fact; no mirror state. |
| authored flag definitions | MoralChoiceFlagCatalogLoader | flag mutation, quest gates and resolution facts | MoralChoiceSystem | Owner emits/reads a typed fact; no mirror state. |
| authored flag definitions | MoralChoiceFlagCatalogLoader | persistent active flags and resolutions | MoralChoiceState/MoralChoiceSave | Owner emits/reads a typed fact; no mirror state. |
| authored flag definitions | MoralChoiceFlagCatalogLoader | host composition and player presentation | Main.MoralChoice/MoralChoicePanel | Owner emits/reads a typed fact; no mirror state. |
| authored flag definitions | MoralChoiceFlagCatalogLoader | catalog, branch/gossip and flag-state proof | Moral focused tests | Owner emits/reads a typed fact; no mirror state. |
| canonical flag ID vocabulary | MoralChoiceIds | authored flag definitions | MoralChoiceFlagCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| canonical flag ID vocabulary | MoralChoiceIds | flag mutation, quest gates and resolution facts | MoralChoiceSystem | Owner emits/reads a typed fact; no mirror state. |
| canonical flag ID vocabulary | MoralChoiceIds | persistent active flags and resolutions | MoralChoiceState/MoralChoiceSave | Owner emits/reads a typed fact; no mirror state. |
| canonical flag ID vocabulary | MoralChoiceIds | host composition and player presentation | Main.MoralChoice/MoralChoicePanel | Owner emits/reads a typed fact; no mirror state. |
| canonical flag ID vocabulary | MoralChoiceIds | catalog, branch/gossip and flag-state proof | Moral focused tests | Owner emits/reads a typed fact; no mirror state. |
| flag mutation, quest gates and resolution facts | MoralChoiceSystem | authored flag definitions | MoralChoiceFlagCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| flag mutation, quest gates and resolution facts | MoralChoiceSystem | canonical flag ID vocabulary | MoralChoiceIds | Owner emits/reads a typed fact; no mirror state. |
| flag mutation, quest gates and resolution facts | MoralChoiceSystem | persistent active flags and resolutions | MoralChoiceState/MoralChoiceSave | Owner emits/reads a typed fact; no mirror state. |
| flag mutation, quest gates and resolution facts | MoralChoiceSystem | host composition and player presentation | Main.MoralChoice/MoralChoicePanel | Owner emits/reads a typed fact; no mirror state. |
| flag mutation, quest gates and resolution facts | MoralChoiceSystem | catalog, branch/gossip and flag-state proof | Moral focused tests | Owner emits/reads a typed fact; no mirror state. |
| persistent active flags and resolutions | MoralChoiceState/MoralChoiceSave | authored flag definitions | MoralChoiceFlagCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| persistent active flags and resolutions | MoralChoiceState/MoralChoiceSave | canonical flag ID vocabulary | MoralChoiceIds | Owner emits/reads a typed fact; no mirror state. |
| persistent active flags and resolutions | MoralChoiceState/MoralChoiceSave | flag mutation, quest gates and resolution facts | MoralChoiceSystem | Owner emits/reads a typed fact; no mirror state. |
| persistent active flags and resolutions | MoralChoiceState/MoralChoiceSave | host composition and player presentation | Main.MoralChoice/MoralChoicePanel | Owner emits/reads a typed fact; no mirror state. |
| persistent active flags and resolutions | MoralChoiceState/MoralChoiceSave | catalog, branch/gossip and flag-state proof | Moral focused tests | Owner emits/reads a typed fact; no mirror state. |
| host composition and player presentation | Main.MoralChoice/MoralChoicePanel | authored flag definitions | MoralChoiceFlagCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| host composition and player presentation | Main.MoralChoice/MoralChoicePanel | canonical flag ID vocabulary | MoralChoiceIds | Owner emits/reads a typed fact; no mirror state. |
| host composition and player presentation | Main.MoralChoice/MoralChoicePanel | flag mutation, quest gates and resolution facts | MoralChoiceSystem | Owner emits/reads a typed fact; no mirror state. |
| host composition and player presentation | Main.MoralChoice/MoralChoicePanel | persistent active flags and resolutions | MoralChoiceState/MoralChoiceSave | Owner emits/reads a typed fact; no mirror state. |
| host composition and player presentation | Main.MoralChoice/MoralChoicePanel | catalog, branch/gossip and flag-state proof | Moral focused tests | Owner emits/reads a typed fact; no mirror state. |
| catalog, branch/gossip and flag-state proof | Moral focused tests | authored flag definitions | MoralChoiceFlagCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| catalog, branch/gossip and flag-state proof | Moral focused tests | canonical flag ID vocabulary | MoralChoiceIds | Owner emits/reads a typed fact; no mirror state. |
| catalog, branch/gossip and flag-state proof | Moral focused tests | flag mutation, quest gates and resolution facts | MoralChoiceSystem | Owner emits/reads a typed fact; no mirror state. |
| catalog, branch/gossip and flag-state proof | Moral focused tests | persistent active flags and resolutions | MoralChoiceState/MoralChoiceSave | Owner emits/reads a typed fact; no mirror state. |
| catalog, branch/gossip and flag-state proof | Moral focused tests | host composition and player presentation | Main.MoralChoice/MoralChoicePanel | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 10→25 target with a 25-row definition/state/producer/consumer matrix. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Separate immutable flag definitions from mutable active flags and identify the sole owner for each downstream gate. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Trace representative producers from authored choice `set_flag` fields through the current resolution/save route. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Audit delayed callbacks, branch locks, faction reactions, gossip and UI/history projections for truthful flag availability without creating a parallel ledger. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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

> **DR-07 — Gate-count and test-total drift. HIGH CONFIDENCE.**
The v1.0 bible states 57 CI gates at v1.1.0 (53 fast + 3 full + 1 performance) and quotes both 11,098 and 11,697 full-suite totals from different handoffs. The live 19-wave closeout evidence in `INTEGRATION_PLANS.md` records `verify-fast.sh` ALL 47 GATES PASSED at that batch's close. These figures cannot all describe the same instant. Factory rule: any subject plan that names a gate count or test total must re-verify the number against the live gate inventory at drafting time and cite the closeout it came from. Never carry counts forward from this or any prior document.

> **DR-08 — Wave directories beyond the v1.0 history. VERIFIED.**
`docs/plans/` (126 entries) contains wave directories `wave8_part2/`, `wave9_part2/`, `wave10_part1/`, `wave10_part2/`, `wave11_part1/`, `wave11_part2/`, `wave12_part1_1/`, `flagship_b5_b8/`, and `xp/`, plus `UNCLAIMED_CORPUS_CENSUS.md`, `UNBLOCKED_PLANS_AUDIT_2026-09-19.md`, and `WAVE10_MICRO_DEFERRAL_SWEEP.md`. Two of these are standing expansion inputs: `UNCLAIMED_CORPUS_CENSUS.md` (authored content no system consumes — a utilization-seam backlog) and the unblocked-plans audit. The Factory Protocol consumes both.

> **DR-10 — v1.0 items the audit could not confirm in this pass. UNVERIFIED.**
Not confirmed in this audit pass (single-session, listing-level access): the 11,697 test total; the D1 seal state; the full 57-gate inventory; codec version pin values; the `ClaimPersonalBelonging` no-caller status; decision-blocked item states beyond those the ledger records as resolved. Each of these remains plausible but must be re-verified in live source before any plan depends on it. Factory rule: UNVERIFIED premises get a verification step inside the plan, never silent trust.

> The following v1.0 structures were confirmed by the audit and remain authoritative: the four-tier architecture (Tier 1 data authority in `Assets/StreamingAssets/Data/`; Tier 2 engine-free Core; Tier 3 `src/Host` + `src/UI`; Tier 4 xUnit plus the `HostCli` selftest surface); the `AGENTS.md` non-negotiable rules (Godot authoritative, Core engine-free, JSON authoritative, one authority per concern, focused verification); the narrative corpus under `Assets/StreamingAssets/Data/narrative/` (present in the live listing); the faction, economy, weather, Year-of-Ash, moral-choice, muster, and verdict catalog families (all present live); and the plan-discipline artifacts (`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `SESSION_HANDOFF.md`) at root.

> C1 Shelter operations (rooms, thermal, schedules, fire, decor, barter, noise, prisoners, sanitation, airlock, decon, atmosphere) · C2 Medical pipeline (disease, dose ledger, ARS, surgery, autopsy, pharma, diagnostics, therapies, dependency, crises) · C3 Water, food, agriculture (treatment, condensers, wells, brine, nutrition, kitchen, preservation, grain, greenhouse, crops, aquaponics, apiculture) · C4 Power and industry (grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, coatings, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology) · C5 Expeditions and travel (destinations, scavenging tables, vehicles, waystations, caravans, routes, travel encounters, micro-locations) · C6 Map and geography (wasteland map, damaged zones, fog, route gates, cartography, survey instruments) · C7 Factions and war (stance, doctrines, war chains, tributes, treaties, embargoes, espionage, psyops, infiltration, musters, labor camps, bounties) · C8 Radio and information (stations, programs, intercepts, distress signals, rumors, sound ranging, direction finding, NVIS, heliograph) · C9 Survivors and interiority (needs, skills, traits, arcs, trauma, guilt, therapies, relations, caregiving, beliefs, rituals, memorials, final wishes, lineage, cohorts, apprenticeships) · C10 Quests and moral choice (questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip, branching, bureaucratic morality, expansion quests) · C11 Economy (market, baselines, regional prices, shocks, rumors, black market, debt ledger, tributes, trade screens, tell lines) · C12 Weather and Year of Ash (weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash families, epilogue pressure) · C13 Endgame and epilogue (Reckoning, verdict, epilogue matrix, chronicle, muster epilogues, standing records, census) · C14 Ecology and wildlife (migration, trapping, ecosystem, bestiary, flora, infestations, contagion, pathogens, crop genomes) · C15 Defense and security (perimeter, defense grid, sky defense, ordnance, chemical defense, orbital harrow, interlocks, EMP effects) · C16 Progression and meta (skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, bestiary, L10N, mods, settings, input) · C17 Host surface and UI (panels, shell, focus navigation, snapshots, a11y, briefings, dashboards).

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

The subject is the boundary between authored ethical definitions and the current active moral-choice state. The plan expands producer/consumer traceability, replay, persistence and truthful player memory without creating a second moral authority.

- **authored flag definitions** remains with `MoralChoiceFlagCatalogLoader` at `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagCatalogLoader.cs`. Loads display metadata; it does not own active state.
- **canonical flag ID vocabulary** remains with `MoralChoiceIds` at `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs`. Pins stable identifiers and existing branch/message flags.
- **flag mutation, quest gates and resolution facts** remains with `MoralChoiceSystem` at `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`. Sole active moral-choice owner; writes state and injected ledger.
- **persistent active flags and resolutions** remains with `MoralChoiceState/MoralChoiceSave` at `Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs; src/Host/MoralChoiceSaveStore.cs`. Existing moral-choice save path; no second flag store.
- **host composition and player presentation** remains with `Main.MoralChoice/MoralChoicePanel` at `src/Main.MoralChoice.cs; src/UI/MoralChoiceModal.cs`. Host/UI projection; it cannot set a flag independently.
- **catalog, branch/gossip and flag-state proof** remains with `Moral focused tests` at `Ashfall.Core.Tests/MoralChoiceFlagPlan125Tests.cs; Ashfall.Core.Tests/World/Plan124_125WarMoralIntegrationTests.cs; Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs`. Focused evidence surface.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load 25 flag definitions and the current moral-choice catalogs
2. restore MoralChoiceSave and replay active flags
3. present available moral choices using current prerequisites
4. resolve one choice through MoralChoiceSystem
5. set the authored flag idempotently and mirror it to IFlagLedger
6. emit resolution/branch/threshold facts for existing consumers
7. project current moral history and capture the existing save

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Definitions are immutable catalog rows; `activeFlags` is a set-like owner state and `resolutions` records the choice facts that produced flags.
- SetFlag is idempotent and branch/flag ledger writes are exactly-once.
- Restore replays active flags into the runtime ledger and preserves the saved moral state.
- A flag definition without a producer is not a gameplay effect; it must be labeled reserved/orphaned rather than displayed as earned.

- A flag ID must be canonical and a definition row cannot create an active flag by itself.
- SetFlag is idempotent; repeated resolution or restore cannot duplicate a ledger effect.
- Quest/branch gates read the canonical owner and return a named missing-flag result.
- A future delayed callback must use an existing persisted fact or a separately claimed save shape, never a panel timer.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Main.MoralChoice.cs
- src/Host/MoralChoiceSaveStore.cs
- src/UI/MoralChoiceModal.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/MoralChoiceFlagPlan125Tests.cs
- Ashfall.Core.Tests/World/Plan124_125WarMoralIntegrationTests.cs
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs

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
| S-01 | 125-01 load 25 definitions | load 25 flag definitions and the current moral-choice catalogs | Definitions are immutable catalog rows; `activeFlags` is a set-like owner state and `resolutions` records the choice facts that produced flags. | A catalog definition is shown as active without an owner fact. | MoralChoiceFlagCatalogLoader |
| S-02 | 125-02 definition without active state | restore MoralChoiceSave and replay active flags | SetFlag is idempotent and branch/flag ledger writes are exactly-once. | A choice sets a flag in a panel but not in MoralChoiceState. | MoralChoiceFlagCatalogLoader |
| S-03 | 125-03 choice sets canonical flag | present available moral choices using current prerequisites | Restore replays active flags into the runtime ledger and preserves the saved moral state. | Restore replays a flag into the ledger twice. | MoralChoiceFlagCatalogLoader |
| S-04 | 125-04 repeated SetFlag idempotence | resolve one choice through MoralChoiceSystem | A flag definition without a producer is not a gameplay effect; it must be labeled reserved/orphaned rather than displayed as earned. | A branch gate reads a shadow list and diverges. | MoralChoiceFlagCatalogLoader |
| S-05 | 125-05 flag ledger mirror | set the authored flag idempotently and mirror it to IFlagLedger | Definitions are immutable catalog rows; `activeFlags` is a set-like owner state and `resolutions` records the choice facts that produced flags. | A delayed callback fires from wall-clock time after reload. | MoralChoiceFlagCatalogLoader |
| S-06 | 125-06 missing gate refusal | emit resolution/branch/threshold facts for existing consumers | SetFlag is idempotent and branch/flag ledger writes are exactly-once. | A catalog definition is shown as active without an owner fact. | MoralChoiceFlagCatalogLoader |
| S-07 | 125-07 branch lock reads flag | project current moral history and capture the existing save | Restore replays active flags into the runtime ledger and preserves the saved moral state. | A choice sets a flag in a panel but not in MoralChoiceState. | MoralChoiceFlagCatalogLoader |
| S-08 | 125-08 restore replays active flag | load 25 flag definitions and the current moral-choice catalogs | A flag definition without a producer is not a gameplay effect; it must be labeled reserved/orphaned rather than displayed as earned. | Restore replays a flag into the ledger twice. | MoralChoiceFlagCatalogLoader |
| S-09 | 125-09 history UI shows earned/unknown | restore MoralChoiceSave and replay active flags | Definitions are immutable catalog rows; `activeFlags` is a set-like owner state and `resolutions` records the choice facts that produced flags. | A branch gate reads a shadow list and diverges. | MoralChoiceFlagCatalogLoader |
| S-10 | 125-10 delayed callback decision | present available moral choices using current prerequisites | SetFlag is idempotent and branch/flag ledger writes are exactly-once. | A delayed callback fires from wall-clock time after reload. | MoralChoiceFlagCatalogLoader |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 125-TC-01 definition schema/uniqueness | data | definition schema/uniqueness; verify the current owner and its negative boundary without inventing a second authority. | MoralChoiceFlagCatalogLoader |
| T-02 | 125-TC-02 canonical ID coverage | unit | canonical ID coverage; verify the current owner and its negative boundary without inventing a second authority. | MoralChoiceFlagCatalogLoader |
| T-03 | 125-TC-03 loader empty/malformed behavior | persistence | loader empty/malformed behavior; verify the current owner and its negative boundary without inventing a second authority. | MoralChoiceFlagCatalogLoader |
| T-04 | 125-TC-04 SetFlag idempotence | determinism | SetFlag idempotence; verify the current owner and its negative boundary without inventing a second authority. | MoralChoiceFlagCatalogLoader |
| T-05 | 125-TC-05 activeFlags deep copy | host | activeFlags deep copy; verify the current owner and its negative boundary without inventing a second authority. | MoralChoiceFlagCatalogLoader |
| T-06 | 125-TC-06 ledger mirror | UI/accessibility | ledger mirror; verify the current owner and its negative boundary without inventing a second authority. | MoralChoiceFlagCatalogLoader |
| T-07 | 125-TC-07 HasFlag precedence | cross-system | HasFlag precedence; verify the current owner and its negative boundary without inventing a second authority. | MoralChoiceFlagCatalogLoader |
| T-08 | 125-TC-08 choice producer binding | data | choice producer binding; verify the current owner and its negative boundary without inventing a second authority. | MoralChoiceFlagCatalogLoader |
| T-09 | 125-TC-09 branch gate missing flag | unit | branch gate missing flag; verify the current owner and its negative boundary without inventing a second authority. | MoralChoiceFlagCatalogLoader |
| T-10 | 125-TC-10 restore replay | persistence | restore replay; verify the current owner and its negative boundary without inventing a second authority. | MoralChoiceFlagCatalogLoader |
| T-11 | 125-TC-11 legacy moral save | determinism | legacy moral save; verify the current owner and its negative boundary without inventing a second authority. | MoralChoiceFlagCatalogLoader |
| T-12 | 125-TC-12 resolution idempotence | host | resolution idempotence; verify the current owner and its negative boundary without inventing a second authority. | MoralChoiceFlagCatalogLoader |
| T-13 | 125-TC-13 gossip consumer determinism | UI/accessibility | gossip consumer determinism; verify the current owner and its negative boundary without inventing a second authority. | MoralChoiceFlagCatalogLoader |
| T-14 | 125-TC-14 UI history truthfulness | cross-system | UI history truthfulness; verify the current owner and its negative boundary without inventing a second authority. | MoralChoiceFlagCatalogLoader |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 56 | `Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 39 | `Ashfall.Core.Tests/MoralChoiceSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 24 | `Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 18 | `src/Host/HostCli.MoralChoice.cs` | current reference count; inspect the caller before treating it as a live route |
| 15 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 13 | `Ashfall.Core.Tests/MoralChoiceCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 12 | `Ashfall.Core.Tests/MoralChoiceFlagPlan125Tests.cs` | current reference count; inspect the caller before treating it as a live route |
| 12 | `Assets/Ashfall.Core/Factions/MilitaryBranchSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 12 | `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Ashfall.Core.Tests/Journeys/MoralChoiceJourneyTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `src/Host/MoralChoiceSaveStore.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `Ashfall.Core.Tests/PrpfStandingSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `Assets/Ashfall.Core/Factions/RebelBranchSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `src/Main.MoralChoice.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/World/Plan100_48MoralReactionsWeatherGatesIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagCatalogLoader.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/IndependentBranchSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/World/Plan124_125WarMoralIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/WorldFlagConsumerIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Factions/PrpfStandingSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/MoralChoice/Plan144MoralChoiceStubClosureTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/MoralChoice/Plan15_18MoralCodexIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Main.UiPanels.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/FactionBranchCoordinatorTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/MicroLocationWorldFlagTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/MilitaryBranchSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/MoralChoice/MoralChoiceDailyOfferTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Radio/DistressSignalMoralChoiceTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/RebelBranchSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Factions/MilitaryBranchState.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Flags/OneShotTriggerLedger.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


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


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/moral_choice_quests.json`

### `Assets/StreamingAssets/Data/moral_choice_quests.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 141249; characters: 141245.
- SHA-256: `1c84bf9e37036b9247ba0f48e1a14a8d1b497a0bcc63e0e506f23f8bb399b4e1`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `quests`

#### `quests` — 68 current rows

- Row 001 `quest_moral_share_child`: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Gave the child everything in the pack. Came away with a drawing of a house.","label":"Give all your food","moral_delta":10,"outcome_text":"You hand over everything you are carryi…`
- Row 002 `quest_moral_share_family`: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Emptied the pack in a stairwell for a family of four. They tried to give a share back.","label":"Give all your supplies","moral_delta":15,"outcome_text":"You empty the pack onto …`
- Row 003 `quest_moral_share_injured`: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Splinted a stranger's leg in an alley. He insists he owes me.","label":"Give medical supplies","moral_delta":8,"outcome_text":"You splint it against a length of pipe and use most…`
- Row 004 `quest_moral_share_water`: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Gave a stranger my whole canteen. Traded it for the way to clean water.","label":"Give all your water","moral_delta":7,"outcome_text":"You hand over your canteen and they drink i…`
- Row 005 `quest_moral_share_elder`: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Spent food and medicine on a dying woman. She left me her family's door.","label":"Give food and medicine","moral_delta":12,"outcome_text":"You spend rations and medicine on some…`
- Row 006 `quest_moral_share_pregnant`: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Emptied the whole medical kit on a delivery in the dark. The child has my name.","label":"Give all your medical supplies","moral_delta":18,"outcome_text":"You put everything ster…`
- Row 007 `quest_moral_share_raider`: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Patched a raider who asked for quarter. Bought their crew's patrol times.","label":"Give medical supplies","moral_delta":5,"outcome_text":"You pack the wound and strap it. They w…`
- Row 008 `quest_moral_share_peacekeeper`: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Kept a Peacekeeper alive at the bend. Came away with their ciphers and a name to use.","label":"Give all your supplies","moral_delta":14,"outcome_text":"You work the leg and leav…`
- Row 009 `quest_moral_share_keeper`: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Fed the archivist and found them lenses. They gave me a survey of the valley.","label":"Give food and spare glasses","moral_delta":16,"outcome_text":"Food first, then the spare l…`
- Row 010 `quest_moral_share_bandit_leader`: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Paid the toll to the warlord by the fire pit. The road stayed open.","label":"Give them supplies","moral_delta":6,"outcome_text":"You put the supplies down by the tins. He looks …`
- Row 011 `quest_moral_share_scientist`: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Gave a dying specialist all my medicine. She wrote out a treatment schedule worth more.","label":"Give all your medicine","moral_delta":15,"outcome_text":"You hand over the whole…`
- Row 012 `quest_moral_share_farmer`: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Gave live seed and feed to the sterile rows. Two-fifths of the harvest is mine on paper.","label":"Give seeds and fertilizer","moral_delta":20,"outcome_text":"You hand over live …`
- Row 013 `quest_moral_listen_oldman`: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Sat through an old man's stories. He gave me the way into a bunker.","label":"Hear the story","moral_delta":8,"outcome_text":"You sit through a story about a water main and the …`
- Row 014 `quest_moral_listen_mother`: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Sat with her until the talking ran out. Came away with a toy meant for smaller hands.","label":"Sit with her","moral_delta":10,"outcome_text":"You sit on the floor near the stov…`
- Row 015 `quest_moral_listen_soldier`: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Sat through the full after-action report. My rifle shoots true again.","label":"Accept the debrief","moral_delta":9,"outcome_text":"You take the whole report, contact by contact…`
- Row 016 `quest_moral_listen_child`: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard a kid's whole theory of the stars. There's a paper sky somewhere with my name in it.","label":"Hear the whole theory","moral_delta":12,"outcome_text":"You crouch down and …`
- Row 017 `quest_moral_listen_doctor`: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard every black-tag name read out loud. Left with the key to a pharmacy lockbox.","label":"Hear the names","moral_delta":14,"outcome_text":"You stay for all of it, every name …`
- Row 018 `quest_moral_listen_preacher`: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Stood through the whole sermon. They blessed my weapon like I was one of theirs.","label":"Hear the sermon","moral_delta":6,"outcome_text":"You stand through the whole sermon, f…`
- Row 019 `quest_moral_listen_engineer`: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Walked the overpass and heard out the math. Now I know the way into the maintenance level.","label":"Follow the math","moral_delta":11,"outcome_text":"You walk the span with the…`
- Row 020 `quest_moral_listen_warning`: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard the whole warning, every mad word. Left with a filter mask that works.","label":"Hear the warning out","moral_delta":9,"outcome_text":"You stand there and let them get it …`
- Row 021 `quest_moral_listen_lover`: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard her whole story. She gave me the last photo to carry forward.","label":"Listen sympathetically","moral_delta":9,"outcome_text":"You hear it through. At the end she gives y…`
- Row 022 `quest_moral_listen_teacher`: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Sat through the teacher's whole lesson. I can read the road signs now.","label":"Listen attentively","moral_delta":15,"outcome_text":"Hours pass like minutes. You leave reading …`
- Row 023 `quest_moral_listen_thief`: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard the thief out. Let him keep the light. He's been useful since.","label":"Hear the explanation","moral_delta":6,"outcome_text":"His sister is coughing somewhere. You listen…`
- Row 024 `quest_moral_listen_prophet`: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard the prophet's vision. The message was madness with a map in it, and the map was real.","label":"Hear the vision","moral_delta":18,"outcome_text":"The vision is nonsense st…`
- Row 025 `quest_moral_comfort_widow`: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Sat with a woman beside fresh earth. Came away wearing her partner's coat.","label":"Sit with her","moral_delta":14,"outcome_text":"You sit down in the dirt next to her and sta…`
- Row 026 `quest_moral_comfort_child`: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Talked a kid up out of a nightmare. He paid me in a salvage map.","label":"Wake him slowly","moral_delta":16,"outcome_text":"You say his name from a step back until he comes up…`
- Row 027 `quest_moral_comfort_injured`: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Talked a gut-shot man through the worst of it. He says he owes me his life.","label":"Keep talking to him","moral_delta":10,"outcome_text":"You crouch and keep your voice on hi…`
- Row 028 `quest_moral_comfort_fear`: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Sat on a cellar floor and talked someone through the dark. They sorted my pack to say thank you.","label":"Talk them through it","moral_delta":12,"outcome_text":"You sit on the…`
- Row 029 `quest_moral_comfort_addict`: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Sat up all night with someone coming off the drug. Morning came anyway.","label":"Sit with them through it","moral_delta":8,"outcome_text":"You stay through the worst hours, wa…`
- Row 030 `quest_moral_comfort_guilt`: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Told a survivor their living meant nothing either way. They gave me a radio frequency for it.","label":"Tell them someone had to be the one","moral_delta":15,"outcome_text":"Yo…`
- Row 031 `quest_moral_comfort_elder`: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Sat through an old man's whole archive. He paid me in a good knife.","label":"Listen to all of it","moral_delta":14,"outcome_text":"You sit until your legs ache, through flood …`
- Row 032 `quest_moral_comfort_nightmare`: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Woke a sleeper out of a bad dream. They split their breakfast with me.","label":"Wake them gently","moral_delta":10,"outcome_text":"You say their name from where their hands ca…`
- Row 033 `quest_moral_comfort_loneliness`: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Talked two hours with someone starving for it. They paid me in food.","label":"Stay and talk","moral_delta":12,"outcome_text":"You trade two hours of nothing much, weather, rat…`
- Row 034 `quest_moral_comfort_anger`: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Stood by while someone screamed themselves empty. Then I heard the whole list of their dead.","label":"Wait it out nearby","moral_delta":9,"outcome_text":"You stand off at a di…`
- Row 035 `quest_moral_comfort_hope`: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Told a dreamer the dream would hold. Now I carry stones for it.","label":"Tell them it can stand","moral_delta":18,"outcome_text":"You go over the drawing and say so, it can st…`
- Row 036 `quest_moral_comfort_despair`: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Talked a dying arithmetic down to one day. They agreed to the day.","label":"Talk about tomorrow only","moral_delta":16,"outcome_text":"You don't argue the winter. You argue fo…`
- Row 037 `quest_moral_dead_unmarked`: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Marked a grave that had no name on it. Left the coords in the log so nobody digs there twice.","label":"Mark the grave","moral_delta":12,"outcome_text":"You write the coordinates …`
- Row 038 `quest_moral_dead_burned`: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Buried a burned body under its own wall. Painted a stripe so nobody tracks it home.","label":"Cover and mark it","moral_delta":10,"outcome_text":"You kick down the half-standing p…`
- Row 039 `quest_moral_dead_bloated`: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Pulled a body out of the intake. Lost my gloves and my appetite. Three camps drink tomorrow and don't know why.","label":"Drag it clear","moral_delta":8,"outcome_text":"You rope t…`
- Row 040 `quest_moral_dead_child`: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Buried a child and a toy truck in frozen ground. Cost me the daylight and chipped the shovel. A stranger nodded at me from the road.","label":"Bury them with the truck","moral_del…`
- Row 041 `quest_moral_dead_mass`: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Paced out a pit of forty or more and put it on the map as ground you don't dig. Left a ration on a stone. Can't say why.","label":"Record it and leave a marker","moral_delta":18,"…`
- Row 042 `quest_moral_dead_hanged`: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Cut a hanged stranger down off the gantry and put them under the roadbed. Left the rope on the grave. Didn't want it.","label":"Cut them down and bury them","moral_delta":14,"outc…`
- Row 043 `quest_moral_dead_closet`: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Buried the one from the locked closet in the garden soil, coat and all. Slept in the house after. It was just a house.","label":"Carry them out and bury them","moral_delta":12,"ou…`
- Row 044 `quest_moral_dead_water`: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Waded in and pulled a floater onto the walk, buried it in the fill. The water's still bad. It's less bad.","label":"Haul it out and bury it","moral_delta":10,"outcome_text":"You w…`
- Row 045 `quest_moral_dead_cremated`: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Finished a stranger's cremation with a shovel and the wind. Kicked the pit in after. Their fire had quit; mine didn't.","label":"Finish it by hand","moral_delta":16,"outcome_text"…`
- Row 046 `quest_moral_dead_executed`: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Cut the wire off a prisoner's wrists and buried them outside the fence. Left the cell open. Nothing there to keep.","label":"Cut the wire and bury them","moral_delta":18,"outcome_…`
- Row 047 `quest_moral_dead_suicide`: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Buried the one from the sealed room with their letter on their chest, unread. It wasn't written to me.","label":"Bury them with the letter unread","moral_delta":14,"outcome_text":…`
- Row 048 `quest_moral_dead_massacre`: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Dug one trench behind the block, two days, broke the shovel, finished with a shelf. Didn't count them. Marked it with a door.","label":"Dig a common grave","moral_delta":22,"outco…`
- Row 049 `quest_moral_trust_fire`: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Authorized thermal proximity. Acquired updated topographical data.","label":"Authorize proximity","moral_delta":10,"outcome_text":"You permit entry. They transfer a portion of th…`
- Row 050 `quest_moral_trust_wounded`: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Provided comprehensive aid to compromised unknown. Unit integrated into operations.","label":"Provide comprehensive aid","moral_delta":14,"outcome_text":"You expend medical suppl…`
- Row 051 `quest_moral_trust_merchant`: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Executed standard exchange. Acquired coordinates for stable logistics hub.","label":"Execute standard exchange","moral_delta":8,"outcome_text":"You meet the stated caloric price …`
- Row 052 `quest_moral_trust_child`: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Reunited juvenile with primary unit. Secured positive faction standing.","label":"Reunite with unit","moral_delta":16,"outcome_text":"You expend significant operational time loca…`
- Row 053 `quest_moral_trust_deserter`: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Maintained concealment for AWOL combatant. Acquired weapon maintenance data.","label":"Maintain concealment","moral_delta":12,"outcome_text":"You omit their presence from your lo…`
- Row 054 `quest_moral_trust_woman`: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Processed actionable intelligence. Integrated broker into logistics network.","label":"Process intelligence","moral_delta":18,"outcome_text":"The data details hostile rotation sc…`
- Row 055 `quest_moral_trust_soldier`: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Integrated pre-war clearing tactics. Acquired veteran combatant asset.","label":"Integrate tactical data","moral_delta":15,"outcome_text":"You drill urban clearing tactics until …`
- Row 056 `quest_moral_trust_runaway`: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Executed concealment for pursued asset. Acquired high-caloric ration.","label":"Execute concealment","moral_delta":15,"outcome_text":"You divert the pursuers with false telemetry…`
- Row 057 `quest_moral_trust_silent`: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Mirrored non-verbal asset signals. Acquired forward scouting capability.","label":"Mirror signals and observe","moral_delta":10,"outcome_text":"You return neutral gestures. They …`
- Row 058 `quest_moral_trust_signal`: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Executed heavy extraction on subterranean asset. Acquired salvage yield.","label":"Execute extraction","moral_delta":15,"outcome_text":"You expend heavy labor to pry the grate. T…`
- Row 059 `quest_moral_trust_borrower`: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Authorized temporary tool transfer. Tool upgraded. Secondary cache acquired.","label":"Authorize transfer","moral_delta":11,"outcome_text":"The tool is returned within 48 hours, …`
- Row 060 `quest_moral_trust_messenger`: `{"category":"trust","choices":[{"empathy_delta":4,"epitaph":"Executed delivery protocol. Seal intact. Granted high-priority node access.","label":"Execute delivery protocol","moral_delta":20,"outcome_text":"You carry the envelope through h…`
- Row 061 `quest_moral_env_scavenger_child`: `{"category":"share","choices":[{"empathy_delta":2,"epitaph":"Executed joint extraction with juvenile forager. Extracted metals divided.","label":"Execute joint extraction","moral_delta":12,"outcome_text":"You provide caloric support and ap…`
- Row 062 `quest_moral_env_buried_letters`: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Read an old person's letters aloud, all of them. They said the record held.","label":"Read them all aloud","moral_delta":10,"outcome_text":"You read the whole bundle: love notes…`
- Row 063 `quest_moral_env_shelter_refugee`: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Authorized structural entry for refugee. Acquired combustion fuel payment.","label":"Authorize structural entry","moral_delta":8,"outcome_text":"You permit entry. The unit powers…`
- Row 064 `quest_moral_env_dead_explorer`: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Buried the surveyor by the path, canteen and all. Took the logbook. If I get near sector nine, their sister gets it.","label":"Bury them, keep the logbook","moral_delta":8,"outcom…`
- Row 065 `quest_moral_env_wounded_scavenger`: `{"category":"comfort","choices":[{"empathy_delta":2,"epitaph":"Spent my medkit on a man who said he was done. He paid in directions.","label":"Patch the wound","moral_delta":10,"outcome_text":"You spend gauze and powder on a stranger. The …`
- Row 066 `quest_moral_trap_prey_high`: `{"category":"share","choices":[{"empathy_delta":0,"epitaph":"Butchered a collared rad-dog for meat and said nothing.","label":"Butcher it. Meat is meat.","moral_delta":-8,"outcome_text":"You do the work quickly and do not look at the colla…`
- Row 067 `quest_moral_trap_prey_medium`: `{"category":"share","choices":[{"empathy_delta":0,"epitaph":"Served questionable fowl to stretch the rations.","label":"Cook it through and stretch the ration","moral_delta":-5,"outcome_text":"Char on the outside, caution on the inside. Mo…`
- Row 068 `quest_moral_trap_prey_low`: `{"category":"share","choices":[{"empathy_delta":0,"epitaph":"Boiled a twitching squirrel rather than waste it.","label":"In the pot. Waste is the real cruelty.","moral_delta":-2,"outcome_text":"It is done before the water boils. The childr…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/moral_choice_chains.json`

### `Assets/StreamingAssets/Data/moral_choice_chains.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 31788; characters: 31784.
- SHA-256: `2a30c42686baef9f1e64cdda74fdb13cbe9204c641790d47212e5aeb48fc2135`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `description`, `branches`, `merge_rules`, `lockout_rules`, `quest_gates`, `echo_quests`, `gossip_propagation`, `faction_reactions`

#### `branches` — 4 current rows

- Row 001 `branch_mercy_road`: `{"description":"Compassion as a permanent stance. Helping becomes habit, then identity, then burden. Unlocks cooperative storylines; permanently closes the Iron Way and Broken Compact after the third mercy quest.","display_name":"The Mercy…`
- Row 002 `branch_iron_way`: `{"description":"Pragmatism hardens into ruthlessness. Survival at the cost of others becomes a doctrine. Permanently closes the Mercy Road and Listener's Thread after the third iron quest.","display_name":"The Iron Way","entry_quests":["qu…`
- Row 003 `branch_listener_thread`: `{"description":"Understanding over action. The player accumulates stories and wisdom instead of taking sides. Permanently closes the Iron Way and Broken Compact after the third listener quest.","display_name":"The Listener's Thread","entry…`
- Row 004 `branch_broken_compact`: `{"description":"Betrayal as a survival strategy. Trust becomes a weapon. Permanently closes the Mercy Road and Listener's Thread after the third betrayal quest.","display_name":"The Broken Compact","entry_quests":["quest_moral_chain_betray…`

#### `merge_rules` — 4 keyed entries

- Entry 001 `description`: `"A player on Branch A can access merge quests from Branch B only if merge_allowed includes B. Merge quests are marked with 'merge_from' in their prerequisites. Merging does NOT unlock the merged branch's exclusive content — only the shared…`
- Entry 002 `merge_quest_prefix`: `"quest_moral_merge_"`
- Entry 003 `merge_quests_require_min_progress`: `5`
- Entry 004 `merge_never_unlocks_exclusive`: `true`

#### `lockout_rules` — 4 keyed entries

- Entry 001 `description`: `"When a branch locks, all quests belonging to the locked-out branches become permanently inaccessible. The player can never return to those storylines. This creates meaningful permanent consequences."`
- Entry 002 `lockout_is_permanent`: `true`
- Entry 003 `lockout_fires_journal_entry`: `true`
- Entry 004 `lockout_journal_template`: `"A door has closed. The path of {locked_branch_name} is no longer open to you."`

#### `quest_gates` — 88 current rows

- Row 001 `quest_moral_chain_mercy_04`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_04","requires":["quest_moral_chain_mercy_03"],"requires_choice_index":null,"requires_min_moral":15}`
- Row 002 `quest_moral_chain_mercy_05`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_05","requires":["quest_moral_chain_mercy_04"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 003 `quest_moral_chain_mercy_06`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_06","requires":["quest_moral_chain_mercy_05"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 004 `quest_moral_chain_mercy_07`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_07","requires":["quest_moral_chain_mercy_06"],"requires_choice_index":null,"requires_min_moral":30}`
- Row 005 `quest_moral_chain_mercy_08`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_08","requires":["quest_moral_chain_mercy_07"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 006 `quest_moral_chain_mercy_09`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_09","requires":["quest_moral_chain_mercy_08"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 007 `quest_moral_chain_mercy_10`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_10","requires":["quest_moral_chain_mercy_09"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 008 `quest_moral_chain_mercy_11`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_11","requires":["quest_moral_chain_mercy_10"],"requires_choice_index":null,"requires_min_moral":50}`
- Row 009 `quest_moral_chain_mercy_12`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_12","requires":["quest_moral_chain_mercy_11"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 010 `quest_moral_chain_mercy_13`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_13","requires":["quest_moral_chain_mercy_12"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 011 `quest_moral_chain_mercy_14`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_14","requires":["quest_moral_chain_mercy_13"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 012 `quest_moral_chain_mercy_15`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_15","requires":["quest_moral_chain_mercy_14"],"requires_choice_index":null,"requires_min_moral":75}`
- Row 013 `quest_moral_chain_mercy_16`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_16","requires":["quest_moral_chain_mercy_15"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 014 `quest_moral_chain_mercy_17`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_17","requires":["quest_moral_chain_mercy_16"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 015 `quest_moral_chain_mercy_18`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_18","requires":["quest_moral_chain_mercy_17"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 016 `quest_moral_chain_mercy_19`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_19","requires":["quest_moral_chain_mercy_18"],"requires_choice_index":null,"requires_min_moral":90}`
- Row 017 `quest_moral_chain_mercy_20`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_20","requires":["quest_moral_chain_mercy_19"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 018 `quest_moral_chain_mercy_21`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_21","requires":["quest_moral_chain_mercy_20"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 019 `quest_moral_chain_mercy_22`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_22","requires":["quest_moral_chain_mercy_21"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 020 `quest_moral_chain_mercy_23`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_23","requires":["quest_moral_chain_mercy_22"],"requires_choice_index":null,"requires_min_moral":100}`
- Row 021 `quest_moral_chain_mercy_24`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_24","requires":["quest_moral_chain_mercy_23"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 022 `quest_moral_chain_mercy_25`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_25","requires":["quest_moral_chain_mercy_24"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 023 `quest_moral_chain_iron_04`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_04","requires":["quest_moral_chain_iron_03"],"requires_choice_index":null,"requires_max_moral":-15}`
- Row 024 `quest_moral_chain_iron_05`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_05","requires":["quest_moral_chain_iron_04"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 025 `quest_moral_chain_iron_06`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_06","requires":["quest_moral_chain_iron_05"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 026 `quest_moral_chain_iron_07`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_07","requires":["quest_moral_chain_iron_06"],"requires_choice_index":null,"requires_max_moral":-30}`
- Row 027 `quest_moral_chain_iron_08`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_08","requires":["quest_moral_chain_iron_07"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 028 `quest_moral_chain_iron_09`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_09","requires":["quest_moral_chain_iron_08"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 029 `quest_moral_chain_iron_10`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_10","requires":["quest_moral_chain_iron_09"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 030 `quest_moral_chain_iron_11`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_11","requires":["quest_moral_chain_iron_10"],"requires_choice_index":null,"requires_max_moral":-50}`
- Row 031 `quest_moral_chain_iron_12`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_12","requires":["quest_moral_chain_iron_11"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 032 `quest_moral_chain_iron_13`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_13","requires":["quest_moral_chain_iron_12"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 033 `quest_moral_chain_iron_14`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_14","requires":["quest_moral_chain_iron_13"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 034 `quest_moral_chain_iron_15`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_15","requires":["quest_moral_chain_iron_14"],"requires_choice_index":null,"requires_max_moral":-75}`
- Row 035 `quest_moral_chain_iron_16`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_16","requires":["quest_moral_chain_iron_15"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 036 `quest_moral_chain_iron_17`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_17","requires":["quest_moral_chain_iron_16"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 037 `quest_moral_chain_iron_18`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_18","requires":["quest_moral_chain_iron_17"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 038 `quest_moral_chain_iron_19`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_19","requires":["quest_moral_chain_iron_18"],"requires_choice_index":null,"requires_max_moral":-90}`
- Row 039 `quest_moral_chain_iron_20`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_20","requires":["quest_moral_chain_iron_19"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 040 `quest_moral_chain_iron_21`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_21","requires":["quest_moral_chain_iron_20"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 041 `quest_moral_chain_iron_22`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_22","requires":["quest_moral_chain_iron_21"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 042 `quest_moral_chain_iron_23`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_23","requires":["quest_moral_chain_iron_22"],"requires_choice_index":null,"requires_max_moral":-100}`
- Row 043 `quest_moral_chain_iron_24`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_24","requires":["quest_moral_chain_iron_23"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 044 `quest_moral_chain_iron_25`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_25","requires":["quest_moral_chain_iron_24"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 045 `quest_moral_chain_listen_04`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_04","requires":["quest_moral_chain_listen_03"],"requires_choice_index":null,"requires_min_empathy":8}`
- Row 046 `quest_moral_chain_listen_05`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_05","requires":["quest_moral_chain_listen_04"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 047 `quest_moral_chain_listen_06`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_06","requires":["quest_moral_chain_listen_05"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 048 `quest_moral_chain_listen_07`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_07","requires":["quest_moral_chain_listen_06"],"requires_choice_index":null,"requires_min_empathy":15}`
- Row 049 `quest_moral_chain_listen_08`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_08","requires":["quest_moral_chain_listen_07"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 050 `quest_moral_chain_listen_09`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_09","requires":["quest_moral_chain_listen_08"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 051 `quest_moral_chain_listen_10`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_10","requires":["quest_moral_chain_listen_09"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 052 `quest_moral_chain_listen_11`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_11","requires":["quest_moral_chain_listen_10"],"requires_choice_index":null,"requires_min_empathy":22}`
- Row 053 `quest_moral_chain_listen_12`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_12","requires":["quest_moral_chain_listen_11"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 054 `quest_moral_chain_listen_13`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_13","requires":["quest_moral_chain_listen_12"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 055 `quest_moral_chain_listen_14`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_14","requires":["quest_moral_chain_listen_13"],"requires_choice_index":null,"requires_min_empathy":30}`
- Row 056 `quest_moral_chain_listen_15`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_15","requires":["quest_moral_chain_listen_14"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 057 `quest_moral_chain_listen_16`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_16","requires":["quest_moral_chain_listen_15"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 058 `quest_moral_chain_listen_17`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_17","requires":["quest_moral_chain_listen_16"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 059 `quest_moral_chain_listen_18`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_18","requires":["quest_moral_chain_listen_17"],"requires_choice_index":null,"requires_min_empathy":38}`
- Row 060 `quest_moral_chain_listen_19`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_19","requires":["quest_moral_chain_listen_18"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 061 `quest_moral_chain_listen_20`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_20","requires":["quest_moral_chain_listen_19"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 062 `quest_moral_chain_listen_21`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_21","requires":["quest_moral_chain_listen_20"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 063 `quest_moral_chain_listen_22`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_22","requires":["quest_moral_chain_listen_21"],"requires_choice_index":null,"requires_min_empathy":45}`
- Row 064 `quest_moral_chain_listen_23`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_23","requires":["quest_moral_chain_listen_22"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 065 `quest_moral_chain_listen_24`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_24","requires":["quest_moral_chain_listen_23"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 066 `quest_moral_chain_listen_25`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_25","requires":["quest_moral_chain_listen_24"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 067 `quest_moral_chain_betray_04`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_04","requires":["quest_moral_chain_betray_03"],"requires_choice_index":null,"requires_flag":"flag_betrayed_trust"}`
- Row 068 `quest_moral_chain_betray_05`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_05","requires":["quest_moral_chain_betray_04"],"requires_choice_index":null,"requires_flag":null}`
- Row 069 `quest_moral_chain_betray_06`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_06","requires":["quest_moral_chain_betray_05"],"requires_choice_index":null,"requires_flag":null}`
- Row 070 `quest_moral_chain_betray_07`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_07","requires":["quest_moral_chain_betray_06"],"requires_choice_index":null,"requires_flag":"flag_betrayed_ally"}`
- Row 071 `quest_moral_chain_betray_08`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_08","requires":["quest_moral_chain_betray_07"],"requires_choice_index":null,"requires_flag":null}`
- Row 072 `quest_moral_chain_betray_09`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_09","requires":["quest_moral_chain_betray_08"],"requires_choice_index":null,"requires_flag":null}`
- Row 073 `quest_moral_chain_betray_10`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_10","requires":["quest_moral_chain_betray_09"],"requires_choice_index":null,"requires_flag":null}`
- Row 074 `quest_moral_chain_betray_11`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_11","requires":["quest_moral_chain_betray_10"],"requires_choice_index":null,"requires_flag":"flag_betrayed_faction"}`
- Row 075 `quest_moral_chain_betray_12`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_12","requires":["quest_moral_chain_betray_11"],"requires_choice_index":null,"requires_flag":null}`
- Row 076 `quest_moral_chain_betray_13`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_13","requires":["quest_moral_chain_betray_12"],"requires_choice_index":null,"requires_flag":null}`
- Row 077 `quest_moral_chain_betray_14`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_14","requires":["quest_moral_chain_betray_13"],"requires_choice_index":null,"requires_flag":null}`
- Row 078 `quest_moral_chain_betray_15`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_15","requires":["quest_moral_chain_betray_14"],"requires_choice_index":null,"requires_flag":"flag_broken_pact"}`
- Row 079 `quest_moral_chain_betray_16`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_16","requires":["quest_moral_chain_betray_15"],"requires_choice_index":null,"requires_flag":null}`
- Row 080 `quest_moral_chain_betray_17`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_17","requires":["quest_moral_chain_betray_16"],"requires_choice_index":null,"requires_flag":null}`
- Row 081 `quest_moral_chain_betray_18`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_18","requires":["quest_moral_chain_betray_17"],"requires_choice_index":null,"requires_flag":null}`
- Row 082 `quest_moral_chain_betray_19`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_19","requires":["quest_moral_chain_betray_18"],"requires_choice_index":null,"requires_flag":"flag_become_warlord"}`
- Row 083 `quest_moral_chain_betray_20`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_20","requires":["quest_moral_chain_betray_19"],"requires_choice_index":null,"requires_flag":null}`
- Row 084 `quest_moral_chain_betray_21`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_21","requires":["quest_moral_chain_betray_20"],"requires_choice_index":null,"requires_flag":null}`
- Row 085 `quest_moral_chain_betray_22`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_22","requires":["quest_moral_chain_betray_21"],"requires_choice_index":null,"requires_flag":null}`
- Row 086 `quest_moral_chain_betray_23`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_23","requires":["quest_moral_chain_betray_22"],"requires_choice_index":null,"requires_flag":"flag_throne_of_ash"}`
- Row 087 `quest_moral_chain_betray_24`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_24","requires":["quest_moral_chain_betray_23"],"requires_choice_index":null,"requires_flag":null}`
- Row 088 `quest_moral_chain_betray_25`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_25","requires":["quest_moral_chain_betray_24"],"requires_choice_index":null,"requires_flag":null}`

#### `echo_quests` — 2 keyed entries

- Entry 001 `description`: `"Echo quests fire when a specific earlier quest was resolved a certain way. They reference the prior choice and present consequences or callbacks. Accessible regardless of branch unless their branch field restricts them."`
- Entry 002 `quests`: `[{"branch":null,"min_days_after":30,"quest_id":"quest_moral_echo_child_returns","triggered_by":"quest_moral_share_child","triggered_by_choice":0},{"branch":null,"min_days_after":20,"quest_id":"quest_moral_echo_child_steals","triggered_by":…`

#### `gossip_propagation` — 2 keyed entries

- Entry 001 `description`: `"After propagatesOnDay fires, gossip text plays through camp chatter, NPC greeting changes, and faction stance shifts. Each band has a tone and set of templates."`
- Entry 002 `gossip_file`: `"moral_choice_gossip.json"`

#### `faction_reactions` — 2 keyed entries

- Entry 001 `description`: `"Threshold event dialogues — what faction NPCs say when band-crossing events fire."`
- Entry 002 `reactions_file`: `"moral_choice_faction_reactions.json"`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` — bounded current excerpt (602 of 687 lines)

- Size: 687 lines / 30717 bytes.
- SHA-256: `4cb9adafbbbdfc153d1c80ac2595f1e6c870a6669e2c9e85844a6a416133c9c1`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005:
00006: namespace Ashfall.Core.MoralChoice
00007: {
00008:     public enum MoralPathBand
00009:     {
00010:         VeryEvil,
00011:         Evil,
00012:         SlightlyEvil,
00013:         Neutral,
00014:         SlightlyPositive,
00015:         Positive,
00016:         VeryPositive
00017:     }
00018:
00019:     public enum MoralEndingKind
00020:     {
00021:         Warlord,
00022:         SurvivorKing,
00023:         NeutralSurvivor,
00024:         BalancedSurvivor,
00025:         CommunityBuilder,
00026:         Savior,
00027:         SaintOfWasteland,
00028:         Storykeeper
00029:     }
00030:
00031:     /// <summary>
00032:     /// In-code quest shape. Phase 2 loads these from
00033:     /// Assets/StreamingAssets/Data/moral_choice_quests.json; ids must use the
00034:     /// canonical quest_moral_ prefix (the design doc drafts them as
00035:     /// qst_moral_* — that spelling is rejected on purpose).
00036:     /// </summary>
00037:     public sealed class MoralChoiceQuestDefinition
00038:     {
00039:         public string Id { get; set; } = string.Empty;
00040:         public string DisplayName { get; set; } = string.Empty;
00041:
00042:         /// <summary>share | listen | comfort | dead | trust</summary>
00043:         public string Category { get; set; } = string.Empty;
00044:
00045:         public string Trigger { get; set; } = string.Empty;
00046:
00047:         /// <summary>Encounter prose shown when the quest is discovered.</summary>
00048:         public string Discovery { get; set; } = string.Empty;
00049:
00050:         public string LocationId { get; set; } = string.Empty;
00051:
00052:         /// <summary>First day the quest may be offered; 0 = always.</summary>
00053:         public int MinDay { get; set; }
00054:
00055:         /// <summary>Last day the quest may be offered; 0 or negative = unbounded.</summary>
00056:         public int MaxDay { get; set; }
00057:
00058:         public List<MoralChoiceOption> Choices { get; set; } = new List<MoralChoiceOption>();
00059:     }
00060:
00061:     public sealed class MoralChoiceOption
00062:     {
00063:         /// <summary>UI text for the choice, e.g. "Give all your food".</summary>
00064:         public string Label { get; set; } = string.Empty;
00065:
00066:         public int MoralDelta { get; set; }
00067:         public int EmpathyDelta { get; set; }
00068:         /// <summary>Optional canonical historical flag written after this choice commits.</summary>
00069:         public string SetFlag { get; set; } = string.Empty;
00070:         public string OutcomeText { get; set; } = string.Empty;
00071:         public string Epitaph { get; set; } = string.Empty;
00072:     }
00073:
00074:     /// <summary>
00075:     /// Engine-agnostic moral choice ledger: invisible moral + empathy
00076:     /// accumulators, band computation, overnight reconciliation with
00077:     /// one-time threshold events, ending selection, branch tracking,
00078:     /// quest gating, and echo quest availability. The score is never
00079:     /// surfaced to the player — the world is the UI (host layers read
00080:     /// CurrentBand / events, never the raw number).
00081:     /// </summary>
00082:     public sealed class MoralChoiceSystem
00083:     {
00084:         public const string SystemId = "moral_choice";
00085:         public const string QuestIdPrefix = "quest_moral_";
00086:
00087:         public const int MinScore = -200;
00088:         public const int MaxScore = 200;
00089:
00090:         public const int ListenerEmpathyThreshold = 15;
00091:         public const int ConfidantEmpathyThreshold = 30;
00092:         public const int StorykeeperEmpathyThreshold = 45;
00093:         public const int StorykeeperQuestThreshold = 25;
00094:
00095:         /// <summary>Below this many resolved quests no ending band locks; mild endings fire instead.</summary>
00096:         public const int EndingLockMinQuests = 20;
00097:
00098:         public const string EventLegendPositive = "moral_event_legend_positive";
00099:         public const string EventLegendNegative = "moral_event_legend_negative";
00100:         public const string EventBountyIssued = "moral_event_bounty_issued";
00101:         public const string EventContractTaken = "moral_event_contract_taken";
00102:         public const string EventContractRaised = "moral_event_contract_raised";
00103:         public const string EventPatrolDefense = "moral_event_patrol_defense";
00104:
00105:         /// <summary>Pending-overflow bits settled at the next Reconcile (bit 1 = positive, bit 2 = negative).</summary>
00106:         public const int LegendPositiveFlag = 1;
00107:         public const int LegendNegativeFlag = 2;
00108:
00109:         private readonly ISeededRng _rng;
00110:         private readonly ILog _log;
00111:         private readonly Flags.IFlagLedger? _flags;
00112:         private MoralChoiceState _state = new MoralChoiceState();
00113:
00114:         /// <summary>Branch architecture from moral_choice_chains.json; null until InitializeChainData.</summary>
00115:         private MoralChoiceChainData? _chainData;
00116:         private readonly Dictionary<string, MoralChoiceQuestDefinition> _catalog = new Dictionary<string, MoralChoiceQuestDefinition>(StringComparer.Ordinal);
00117:         private Dictionary<string, string> _questToBranch = new Dictionary<string, string>();
00118:         private HashSet<string> _entryQuestSet = new HashSet<string>();
00119:
00120:         public event Action<MoralChoiceResolution>? OnQuestResolved;
00121:         public event Action<string>? OnThresholdEventFired;
00122:         public event Action<string>? OnBranchLocked;
00123:
00124:         public MoralChoiceSystem(ISeededRng rng, ILog? log = null, Flags.IFlagLedger? flags = null)
00125:         {
00126:             _rng = rng ?? throw new ArgumentNullException(nameof(rng));
00127:             _log = log ?? NullLog.Instance;
00128:             _flags = flags;
00129:         }
00130:
00131:         public MoralChoiceState State => _state;
00132:         public int MoralScore => _state.moralScore;
00133:         public int EmpathyPoints => _state.empathyPoints;
00134:         public int QuestsResolved => _state.resolutions.Count;
00135:         public MoralPathBand CurrentBand => BandForScore(_state.moralScore);
00136:         public IReadOnlyList<MoralChoiceResolution> Resolutions => _state.resolutions;
00137:
00138:         public bool IsListener => _state.empathyPoints >= ListenerEmpathyThreshold;
00139:         public bool IsConfidant => _state.empathyPoints >= ConfidantEmpathyThreshold;
00140:
00141:         /// <summary>Chain data reference; null if not yet initialized.</summary>
00142:         public MoralChoiceChainData? ChainData => _chainData;
00143:
00144:         /// <summary>
00145:         /// Load the branching architecture (moral_choice_chains.json). Builds
00146:         /// internal lookup maps for branch ownership and entry-quest tracking.
00147:         /// Safe to call once at startup; subsequent calls are no-ops.
00148:         /// </summary>
00149:         public void InitializeChainData(MoralChoiceChainData chainData)
00150:         {
00151:             if (chainData == null || _chainData != null) return;
00152:             _chainData = chainData;
00153:
00169:         }
00170:
00171:         public static bool IsCanonicalQuestId(string questId) =>
00172:             questId.StartsWith(QuestIdPrefix, StringComparison.Ordinal);
00173:
00174:         /// <summary>MaxDay &lt;= 0 means unbounded; a malformed window (max &lt; min) is never available.</summary>
00175:         public static bool IsAvailableOnDay(MoralChoiceQuestDefinition quest, int day) =>
00176:             day >= quest.MinDay && (quest.MaxDay <= 0 || (day <= quest.MaxDay && quest.MaxDay >= quest.MinDay));
00177:
00178:         public bool IsResolved(string questId) => TryGetResolution(questId, out _);
00179:
00180:         public bool TryGetResolution(string questId, out MoralChoiceResolution? resolution)
00181:         {
00182:             resolution = _state.resolutions.FirstOrDefault(r => string.Equals(r.questId, questId, StringComparison.Ordinal));
00183:             return resolution != null;
00184:         }
00185:
00186:         // ── Catalog registration ───────────────────────────────────────
00187:
00188:         public void RegisterQuest(MoralChoiceQuestDefinition def)
00189:         {
00190:             if (def == null || string.IsNullOrEmpty(def.Id)) return;
00191:             _catalog[def.Id] = def;
00192:         }
00193:
00194:         public void RegisterQuests(IEnumerable<MoralChoiceQuestDefinition> defs)
00195:         {
00196:             if (defs == null) return;
00197:             foreach (var def in defs)
00198:                 RegisterQuest(def);
00199:         }
00200:
00201:         public IReadOnlyDictionary<string, MoralChoiceQuestDefinition> Catalog => _catalog;
00202:         public int CatalogCount => _catalog.Count;
00203:
00204:         public MoralChoiceQuestDefinition? GetQuest(string id) =>
00205:             !string.IsNullOrEmpty(id) && _catalog.TryGetValue(id, out var def) ? def : null;
00206:
00207:         // ── Seeded daily offers ────────────────────────────────────────
00208:
00209:         /// <summary>
00210:         /// Returns deterministic daily moral choice offers for the given day.
00211:         /// Uses seed formula: unchecked((ulong)_rng.Seed * 31337UL + (ulong)day * 1009UL + 0x5EEDUL).
00212:         /// Only returns unresolved quests whose day window is active and whose chain prerequisites/gates are met.
00213:         /// </summary>
00214:         public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyOffers(int day, int maxOffers = 1)
00215:         {
00216:             if (maxOffers <= 0 || _catalog.Count == 0) return Array.Empty<MoralChoiceQuestDefinition>();
00217:
00218:             var candidates = new List<MoralChoiceQuestDefinition>();
00219:             foreach (var kv in _catalog)
00220:             {
00221:                 var q = kv.Value;
00222:                 if (!IsResolved(q.Id) && IsAvailableOnDay(q, day) && IsChainQuestAccessible(q.Id, day))
00223:                 {
00224:                     candidates.Add(q);
00225:                 }
00226:             }
00233:             if (candidates.Count <= maxOffers) return candidates;
00234:
00235:             ulong dailySeedRaw = unchecked((ulong)_rng.Seed * 31337UL + (ulong)day * 1009UL + 0x5EEDUL);
00236:             int dailySeed = unchecked((int)(dailySeedRaw ^ (dailySeedRaw >> 32)));
00237:             var dailyRng = new SeededRng(dailySeed);
00238:
00239:             var pool = new List<MoralChoiceQuestDefinition>(candidates);
00252:
00253:         /// <summary>
00254:         /// Attempts to resolve a registered moral choice quest.
00255:         /// Enforces strict single-resolution, catalog validity, day window, and choice index bounds
00256:         /// returning structured status codes without throwing on invalid player/client input.
00257:         /// </summary>
00258:         public bool TryResolve(string questId, int choiceIndex, string locationId, int day, out MoralResolveResult result)
00259:         {
00260:             if (string.IsNullOrEmpty(questId) || !_catalog.TryGetValue(questId, out var def))
00261:             {
00262:                 result = MoralResolveResult.Failed(MoralResolveResultCode.UnknownChoice,
00263:                     $"Quest '{questId}' is not registered in moral choice catalog.");
00264:                 return false;
00265:             }
00266:
00267:             if (TryGetResolution(questId, out var existing))
00268:             {
00269:                 result = MoralResolveResult.Failed(MoralResolveResultCode.AlreadyResolved,
00270:                     $"Quest '{questId}' was already resolved on day {existing!.resolvedDay}.", existing);
00271:                 return false;
00272:             }
00273:
00274:             if (!IsAvailableOnDay(def, day))
00275:             {
00276:                 result = MoralResolveResult.Failed(MoralResolveResultCode.ChoiceNotAvailable,
00277:                     $"Quest '{questId}' is not available on day {day} (window: {def.MinDay}..{def.MaxDay}).");
00278:                 return false;
00279:             }
00280:
00281:             if (!IsChainQuestAccessible(questId, day))
00282:             {
00283:                 result = MoralResolveResult.Failed(MoralResolveResultCode.RequirementMissing,
00284:                     $"Quest '{questId}' chain gate or branch requirements are not met.");
00285:                 return false;
00286:             }
00287:
00288:             if (choiceIndex < 0 || choiceIndex >= def.Choices.Count)
00289:             {
00290:                 result = MoralResolveResult.Failed(MoralResolveResultCode.UnknownOption,
00291:                     $"Choice index {choiceIndex} is out of bounds for quest '{questId}' (0..{def.Choices.Count - 1}).");
00292:                 return false;
00293:             }
00294:
00295:             var resolution = Resolve(def, choiceIndex, locationId, day);
00296:             result = MoralResolveResult.Succeeded(resolution);
00297:             return true;
00298:         }
00299:
00300:         // ── Branch tracking ────────────────────────────────────────────
00301:
00302:         /// <summary>Whether a branch has been permanently locked by the lockout mechanic.</summary>
00303:         public bool IsBranchLocked(string branchId) =>
00304:             _state.lockedBranches.Contains(branchId);
00305:
00306:         /// <summary>How many entry quests the player has resolved for a branch.</summary>
00307:         public int GetBranchProgress(string branchId) =>
00308:             _state.branchProgress.TryGetValue(branchId, out int v) ? v : 0;
00309:
00310:         /// <summary>Which branch owns a quest (by chain data); empty string if not a chain quest.</summary>
00311:         public string GetQuestBranch(string questId) =>
00312:             _questToBranch.TryGetValue(questId, out var b) ? b : string.Empty;
00313:
00314:         /// <summary>
00315:         /// Whether a chain quest is accessible: branch not locked, gate
00316:         /// prerequisites met, day window valid, and not already resolved.
00317:         /// Non-chain quests (base/expansion) only check day + resolved.
00318:         /// </summary>
00319:         public bool IsChainQuestAccessible(string questId, int day)
00320:         {
00321:             if (IsResolved(questId)) return false;
00322:
00323:             if (_questToBranch.TryGetValue(questId, out var branchId))
00324:             {
00325:                 if (IsBranchLocked(branchId)) return false;
00326:             }
00327:
00334:
00335:         /// <summary>
00336:         /// Evaluate a quest gate's prerequisites: prior quests resolved,
00337:         /// moral/empathy thresholds, and flag requirements.
00338:         /// </summary>
00339:         public bool EvaluateGate(MoralQuestGate gate)
00340:         {
00341:             if (gate == null) return true;
00342:
00343:             foreach (var req in gate.Requires)
00344:             {
00345:                 if (!IsResolved(req)) return false;
00346:             }
00347:
00348:             if (gate.RequiresMinMoral.HasValue && _state.moralScore < gate.RequiresMinMoral.Value) return false;
00349:             if (gate.RequiresMaxMoral.HasValue && _state.moralScore > gate.RequiresMaxMoral.Value) return false;
00350:             if (gate.RequiresMinEmpathy.HasValue && _state.empathyPoints < gate.RequiresMinEmpathy.Value) return false;
00351:
00352:             if (!string.IsNullOrEmpty(gate.RequiresFlag) && !_state.activeFlags.Contains(gate.RequiresFlag)) return false;
00353:
00354:             return true;
00355:         }
00356:
00357:         /// <summary>Set a moral flag (idempotent).</summary>
00358:         public void SetFlag(string flagId)
00359:         {
00360:             if (string.IsNullOrEmpty(flagId)) return;
00361:             if (!_state.activeFlags.Contains(flagId))
00362:                 _state.activeFlags.Add(flagId);
00363:             _flags?.Set(flagId, "moral_choice");
00364:         }
00365:
00366:         /// <summary>Whether a moral flag is currently set.</summary>
00367:         public bool HasFlag(string flagId) =>
00368:             !string.IsNullOrEmpty(flagId) && (_flags != null ? (_flags.IsSet(flagId) || _state.activeFlags.Contains(flagId)) : _state.activeFlags.Contains(flagId));
00369:
00370:         // ── Echo quests ────────────────────────────────────────────────
00371:
00372:         /// <summary>
00373:         /// Find echo quests that should fire given the current state and day.
00374:         /// An echo quest fires when: its trigger quest was resolved with the
00375:         /// matching choice, enough days have passed, it hasn't fired yet, and
00376:         /// its branch (if any) is not locked.
00377:         /// </summary>
00378:         public List<MoralEchoQuestDefinition> FindAvailableEchoQuests(int currentDay)
00379:         {
00380:             if (_chainData == null) return new List<MoralEchoQuestDefinition>();
00381:
00382:             var result = new List<MoralEchoQuestDefinition>();
00383:             foreach (var echo in _chainData.EchoQuests)
00384:             {
00385:                 if (_state.firedEchoQuests.Contains(echo.QuestId)) continue;
00386:                 if (!TryGetResolution(echo.TriggeredBy, out var trigger)) continue;
00387:                 if (trigger!.choiceIndex != echo.TriggeredByChoice) continue;
00388:                 if (currentDay < trigger.resolvedDay + echo.MinDaysAfter) continue;
00389:
00390:                 if (!string.IsNullOrEmpty(echo.Branch) && IsBranchLocked(echo.Branch)) continue;
00391:
00392:                 result.Add(echo);
00393:             }
00394:             return result;
00395:         }
00396:
00397:         /// <summary>Mark an echo quest as fired (called by the host when the echo is presented).</summary>
00398:         public void MarkEchoQuestFired(string echoQuestId)
00399:         {
00400:             if (string.IsNullOrEmpty(echoQuestId)) return;
00401:             if (!_state.firedEchoQuests.Contains(echoQuestId))
00402:                 _state.firedEchoQuests.Add(echoQuestId);
00403:         }
00404:
00405:         // ── Quest resolution ───────────────────────────────────────────
00406:
00407:         /// <summary>
00408:         /// Resolve a quest choice. One resolution per quest per save: repeat
00409:         /// calls return the stored resolution without re-applying deltas or
00410:         /// re-rolling. Band-crossing consequences never land here — they
00411:         /// settle overnight in Reconcile.
00412:         /// </summary>
00413:         public MoralChoiceResolution Resolve(MoralChoiceQuestDefinition quest, int choiceIndex, string locationId, int day)
00414:         {
00415:             if (quest == null) throw new ArgumentNullException(nameof(quest));
00416:             if (!_catalog.ContainsKey(quest.Id)) _catalog[quest.Id] = quest;
00417:             if (!IsCanonicalQuestId(quest.Id))
00418:             {
00419:                 throw new ArgumentException(
00420:                     $"Moral quest id '{quest.Id}' must use the canonical '{QuestIdPrefix}' prefix " +
00421:                     "(the design doc drafts ids as 'qst_moral_*'; register them as 'quest_moral_*').",
00422:                     nameof(quest));
00423:             }
00424:             if (choiceIndex < 0 || choiceIndex >= quest.Choices.Count)
00429:             if (day < 0) throw new ArgumentOutOfRangeException(nameof(day));
00430:
00431:             if (TryGetResolution(quest.Id, out var existing))
00432:             {
00433:                 _log.Warn($"Moral quest '{quest.Id}' already resolved on day {existing!.resolvedDay}; replaying stored outcome.");
00434:                 return existing;
00435:             }
00436:
00437:             var choice = quest.Choices[choiceIndex];
00444:             {
00445:                 questId = quest.Id,
00446:                 locationId = locationId ?? string.Empty,
00447:                 resolvedDay = day,
00448:                 choiceIndex = choiceIndex,
00449:                 moralDelta = choice.MoralDelta,
00450:                 empathyDelta = choice.EmpathyDelta,
00451:                 impactMark = MarkFor(choice.MoralDelta),
00452:                 outcomeRoll = _rng.Next(0, 100),
00453:                 propagatesOnDay = day + 1 + _rng.Next(0, 3),
00454:                 epitaph = choice.Epitaph
00455:             };
00456:             _state.resolutions.Add(resolution);
00457:
00458:             // Historical moral memory is part of the committed resolution.
00459:             // SetFlag is idempotent, and the existing save state remains the
00460:             // sole persistence authority for the resulting active flag set.
00461:             if (!string.IsNullOrEmpty(choice.SetFlag))
00462:                 SetFlag(choice.SetFlag);
00463:
00464:             OnQuestResolved?.Invoke(resolution);
00465:
00466:             // Overflow never lands mid-scene: flag it, settle it overnight.
00467:             if (unclamped > MaxScore) _state.pendingLegendFlags |= LegendPositiveFlag;
00468:             else if (unclamped < MinScore) _state.pendingLegendFlags |= LegendNegativeFlag;
00475:
00476:         /// <summary>
00477:         /// If the resolved quest is a branch entry quest, increment that
00478:         /// branch's progress. When the lock threshold is reached, lock out
00479:         /// the opposing branches and set the branch-locked flags.
00480:         /// </summary>
00481:         private void TrackBranchProgress(string questId)
00482:         {
00483:             if (!_entryQuestSet.Contains(questId)) return;
00484:             if (!_questToBranch.TryGetValue(questId, out var branchId)) return;
00485:             if (_chainData == null) return;
00486:
00487:             var branch = _chainData.Branches.FirstOrDefault(
00488:                 b => string.Equals(b.Id, branchId, StringComparison.Ordinal));
00489:             if (branch == null) return;
00490:
00491:             if (!_state.branchProgress.ContainsKey(branchId))
00492:                 _state.branchProgress[branchId] = 0;
00493:             _state.branchProgress[branchId]++;
00494:
00495:             if (_state.branchProgress[branchId] >= branch.LockThreshold)
00497:                 foreach (var lockedId in branch.LocksOut)
00498:                 {
00499:                     if (!_state.lockedBranches.Contains(lockedId))
00500:                     {
00501:                         _state.lockedBranches.Add(lockedId);
00502:
00503:                         var lockedBranch = _chainData.Branches.FirstOrDefault(
00508:                         }
00509:
00510:                         OnBranchLocked?.Invoke(lockedId);
00511:                     }
00512:                 }
00513:             }
00514:         }
00516:         /// <summary>
00517:         /// Overnight settlement: pending legend overflow, then band crossings
00518:         /// and their one-time faction events, so an act's consequences always
00519:         /// land overnight, never mid-scene. Every band crossed between the
00520:         /// last reconcile and now settles its event (dedup keeps each
00521:         /// one-time). Out-of-order days are ignored. A never-reconciled save
00522:         /// starts from the Neutral band.
00523:         /// </summary>
00524:         public void Reconcile(int day)
00525:         {
00526:             if (day < _state.lastReconciledDay) return;
00527:             _state.lastReconciledDay = day;
00528:
00529:             if ((_state.pendingLegendFlags & LegendPositiveFlag) != 0) FireThresholdEvent(EventLegendPositive);
00530:             if ((_state.pendingLegendFlags & LegendNegativeFlag) != 0) FireThresholdEvent(EventLegendNegative);
00531:             _state.pendingLegendFlags = 0;
00532:
00533:             int from = _state.bandAtLastReconcile < 0 ? (int)MoralPathBand.Neutral : _state.bandAtLastReconcile;
00534:             int to = (int)CurrentBand;
00535:             if (to == from) return;
00536:
00537:             int step = to > from ? 1 : -1;
00541:                 if (band == to) break;
00542:             }
00543:             _state.bandAtLastReconcile = to;
00544:         }
00545:
00546:         private void FireBandEvents(MoralPathBand band)
00547:         {
00552:                     break;
00553:                 case MoralPathBand.Positive:
00554:                     FireThresholdEvent(EventContractTaken);
00555:                     break;
00556:                 case MoralPathBand.VeryPositive:
00557:                     FireThresholdEvent(EventContractRaised);
00558:                     FireThresholdEvent(EventPatrolDefense);
00559:                     break;
00560:             }
00561:         }
00562:
00563:         public MoralEndingKind SelectEnding() =>
00564:             SelectEnding(_state.moralScore, _state.empathyPoints, _state.resolutions.Count);
00565:
00566:         /// <summary>
00567:         /// Priority: Storykeeper threshold overrides band; below the quest
00568:         /// lock the mild endings fire (the band has not earned the right to
00569:         /// define the run yet); otherwise band decides.
00570:         /// </summary>
00571:         public static MoralEndingKind SelectEnding(int moralScore, int empathyPoints, int questsResolved)
00572:         {
00573:             if (empathyPoints >= StorykeeperEmpathyThreshold && questsResolved >= StorykeeperQuestThreshold)
00574:             {
00575:                 return MoralEndingKind.Storykeeper;
00576:             }
00577:
00578:             if (questsResolved < EndingLockMinQuests)
00579:             {
00580:                 return moralScore < 0 ? MoralEndingKind.NeutralSurvivor
00581:                     : moralScore == 0 ? MoralEndingKind.BalancedSurvivor
00582:                     : MoralEndingKind.CommunityBuilder;
00595:         }
00596:
00597:         public static MoralPathBand BandForScore(int score)
00598:         {
00599:             score = Math.Clamp(score, MinScore, MaxScore);
00600:             if (score <= -100) return MoralPathBand.VeryEvil;
00601:             if (score <= -50) return MoralPathBand.Evil;
00607:         }
00608:
00609:         public MoralChoiceState CaptureState() => Clone(_state);
00610:
00611:         public void RestoreState(MoralChoiceState state)
00612:         {
00613:             if (state == null) throw new ArgumentNullException(nameof(state));
00614:             if (!string.Equals(state.systemId, SystemId, StringComparison.Ordinal))
00615:             {
00616:                 throw new ArgumentException(
00617:                     $"State belongs to system '{state.systemId}', expected '{SystemId}'.", nameof(state));
00618:             }
00619:             if (state.schemaVersion > 1)
00620:             {
00621:                 throw new NotSupportedException(
00622:                     $"Future moral choice save schema {state.schemaVersion}; supported schema is 1.");
00623:             }
00624:             if (state.schemaVersion < 1)
00625:             {
00626:                 throw new ArgumentException("Moral choice save is missing a valid schemaVersion.", nameof(state));
00627:             }
00628:             _state = Clone(state);
00629:             if (_flags != null && _state.activeFlags != null)
00630:             {
00631:                 foreach (var f in _state.activeFlags)
00632:                     _flags.Set(f, "moral_choice");
00633:             }
00634:         }
00635:
00636:         private void FireThresholdEvent(string eventId)
00637:         {
00638:             if (_state.firedThresholdEvents.Contains(eventId)) return;
00639:             _state.firedThresholdEvents.Add(eventId);
00640:             OnThresholdEventFired?.Invoke(eventId);
00641:         }
00642:
00643:         private static string MarkFor(int moralDelta) =>
00644:             moralDelta > 0 ? "up" : moralDelta < 0 ? "down" : "flat";
00645:
00646:         /// <summary>Deep copy so captured/restored states never alias the live ledger.</summary>
00647:         private static MoralChoiceState Clone(MoralChoiceState source)
00648:         {
00649:             var copy = new MoralChoiceState
00650:             {
00651:                 systemId = source.systemId,
00652:                 schemaVersion = source.schemaVersion,
00653:                 moralScore = source.moralScore,
00654:                 empathyPoints = source.empathyPoints,
00655:                 lastReconciledDay = source.lastReconciledDay,
00656:                 bandAtLastReconcile = source.bandAtLastReconcile,
00657:                 pendingLegendFlags = source.pendingLegendFlags,
00658:                 firedThresholdEvents = new List<string>(source.firedThresholdEvents ?? new List<string>()),
00659:                 resolutions = new List<MoralChoiceResolution>(),
00660:                 branchProgress = new Dictionary<string, int>(source.branchProgress ?? new Dictionary<string, int>()),
00661:                 lockedBranches = new List<string>(source.lockedBranches ?? new List<string>()),
00662:                 firedEchoQuests = new List<string>(source.firedEchoQuests ?? new List<string>()),
00663:                 activeFlags = new List<string>(source.activeFlags ?? new List<string>())
00664:             };
00665:             if (source.resolutions != null)
00666:             {
00667:                 foreach (var r in source.resolutions)
00668:                 {
00669:                     copy.resolutions.Add(new MoralChoiceResolution
00670:                     {
00671:                         questId = r.questId,
00672:                         locationId = r.locationId,
00673:                         resolvedDay = r.resolvedDay,
00674:                         choiceIndex = r.choiceIndex,
00675:                         moralDelta = r.moralDelta,
00676:                         empathyDelta = r.empathyDelta,
00677:                         impactMark = r.impactMark,
00678:                         outcomeRoll = r.outcomeRoll,
00679:                         propagatesOnDay = r.propagatesOnDay,
00680:                         epitaph = r.epitaph
00681:                     });
00682:                 }
00683:             }
00684:             return copy;
00685:         }
00686:     }
00687: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs` — complete current file

- Size: 76 lines / 3245 bytes.
- SHA-256: `67dec17ef0a93d7516a0ddf681661c46e8cd566d8a0602fabc77426430f027b5`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004:
00005: namespace Ashfall.Core.MoralChoice
00006: {
00007:     /// <summary>
00008:     /// Save DTO for the moral choice system ("The Weight of Survival",
00009:     /// docs/MORAL_CHOICE_SYSTEM.md). Journal resolutions, seeded outcome
00010:     /// rolls, gossip propagation schedules, branch progress, locked branches,
00011:     /// echo quest tracking, and moral flags all live here so a save replays
00012:     /// identically — there is no second file format.
00013:     /// </summary>
00014:     [Serializable]
00015:     public sealed class MoralChoiceState
00016:     {
00017:         public string systemId = MoralChoiceSystem.SystemId;
00018:         public int schemaVersion = 1;
00019:         public int moralScore;
00020:         public int empathyPoints;
00021:         public List<MoralChoiceResolution> resolutions = new List<MoralChoiceResolution>();
00022:         public int lastReconciledDay = -1;
00023:
00024:         /// <summary>Ordinal of MoralPathBand at the last reconcile; -1 = never reconciled.</summary>
00025:         public int bandAtLastReconcile = -1;
00026:
00027:         /// <summary>One-time threshold/legend events already fired, by id.</summary>
00028:         public List<string> firedThresholdEvents = new List<string>();
00029:
00030:         /// <summary>Overflow bits (LegendPositiveFlag/LegendNegativeFlag) awaiting overnight settlement.</summary>
00031:         public int pendingLegendFlags;
00032:
00033:         /// <summary>Entry-quest resolutions per branch (branch_id → count). Drives branch locking.</summary>
00034:         public Dictionary<string, int> branchProgress = new Dictionary<string, int>();
00035:
00036:         /// <summary>Branches permanently locked by the lockout mechanic.</summary>
00037:         public List<string> lockedBranches = new List<string>();
00038:
00039:         /// <summary>Echo quests already fired (by quest_id). Each fires at most once per save.</summary>
00040:         public List<string> firedEchoQuests = new List<string>();
00041:
00042:         /// <summary>Moral flags set during this save (flag_id list, treated as a set).</summary>
00043:         public List<string> activeFlags = new List<string>();
00044:     }
00045:
00046:     /// <summary>
00047:     /// One resolved moral quest: the journal line, the ledger entry, and the
00048:     /// seeded rolls drawn at resolution time. Treated as immutable after
00049:     /// creation; fields stay public/mutable to match the save-DTO convention
00050:     /// the JSON pipeline deserializes into.
00051:     /// </summary>
00052:     [Serializable]
00053:     public sealed class MoralChoiceResolution
00054:     {
00055:         public string questId = string.Empty;
00056:         public string locationId = string.Empty;
00057:         public int resolvedDay = -1;
00058:         public int choiceIndex = -1;
00059:
00060:         /// <summary>Raw design delta of the chosen option (pre-clamp); the journal arrow shows its sign.</summary>
00061:         public int moralDelta;
00062:
00063:         public int empathyDelta;
00064:
00065:         /// <summary>up | down | flat — never the number, only the direction.</summary>
00066:         public string impactMark = "flat";
00067:
00068:         /// <summary>0-99, rolled once at resolution and stored for deterministic outcome branches.</summary>
00069:         public int outcomeRoll = -1;
00070:
00071:         /// <summary>Gossip leaves the witnessing circle on this day (resolvedDay + 1..3).</summary>
00072:         public int propagatesOnDay = -1;
00073:
00074:         public string epitaph = string.Empty;
00075:     }
00076: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagCatalogLoader.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagCatalogLoader.cs` — complete current file

- Size: 59 lines / 2010 bytes.
- SHA-256: `db33abbac6c7bb582f3d17a592dc6a05fab119cf1b85be7f417b7c20a927b115`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005:
00006: namespace Ashfall.Core.MoralChoice
00007: {
00008:     // ── JSON wire records ──
00009:
00010:     [Serializable]
00011:     public sealed class MoralChoiceFlagCatalogContainer
00012:     {
00013:         public int schema_version = 1;
00014:         public string description = string.Empty;
00015:         public List<MoralFlagRecord> flags = new List<MoralFlagRecord>();
00016:     }
00017:
00018:     [Serializable]
00019:     public sealed class MoralFlagRecord
00020:     {
00021:         public string id = string.Empty;
00022:         public string display_name = string.Empty;
00023:     }
00024:
00025:     /// <summary>
00026:     /// Loads moral_choice_flags.json — persistent moral-history definitions
00027:     /// used by branch locking, quest gating, and downstream predicates.
00028:     /// Engine-agnostic.
00029:     /// </summary>
00030:     public static class MoralChoiceFlagCatalogLoader
00031:     {
00032:         public const string DefaultFileName = "moral_choice_flags.json";
00033:
00034:         public static MoralChoiceFlagDefinitions Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
00035:         {
00036:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00037:                 return new MoralChoiceFlagDefinitions();
00038:
00039:             string path = fileIO.Combine(dataDir, DefaultFileName);
00040:             if (!fileIO.FileExists(path))
00041:                 return new MoralChoiceFlagDefinitions();
00042:
00043:             string raw = fileIO.ReadAllText(path);
00044:             if (string.IsNullOrWhiteSpace(raw))
00045:                 return new MoralChoiceFlagDefinitions();
00046:
00047:             var container = json.Deserialize<MoralChoiceFlagCatalogContainer>(raw);
00048:             if (container?.flags == null)
00049:                 return new MoralChoiceFlagDefinitions();
00050:
00051:             var data = new MoralChoiceFlagDefinitions();
00052:             data.Flags = container.flags
00053:                 .Where(f => f != null)
00054:                 .Select(f => new MoralFlagDefinition { Id = f.id, DisplayName = f.display_name })
00055:                 .ToList();
00056:             return data;
00057:         }
00058:     }
00059: }
```


# Appendix — Current Source Detail: `src/Main.MoralChoice.cs`

### `src/Main.MoralChoice.cs` — complete current file

- Size: 344 lines / 16147 bytes.
- SHA-256: `e12534c6d2a8cd86c90d7283de173c51bf07cd68e46bb52f6129bbeb05375b8d`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using Godot;
00003: using System;
00004: using System.Collections.Generic;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.MoralChoice;
00008:
00009: namespace AtomicWar.GodotApp
00010: {
00011:     public partial class Main : Control
00012:     {
00013:         // ── Moral choice ("The Weight of Survival") host wiring ──
00014:         // The score is invisible by design: hosts read CurrentBand and the
00015:         // threshold events, never the raw number.
00016:         private MoralChoiceSystem _moralChoice = null!;
00017:         private List<MoralChoiceQuestDefinition> _moralChoiceDefs = new List<MoralChoiceQuestDefinition>();
00018:         private bool _moralChoiceDirty;
00019:
00020:         // ── Branching / gossip / faction reactions (Phase 2 data) ──
00021:         private MoralChoiceChainData _moralChainData = new MoralChoiceChainData();
00022:         private MoralChoiceGossipData _moralGossipData = new MoralChoiceGossipData();
00023:         private MoralChoiceFactionReactionsData _moralFactionReactions = new MoralChoiceFactionReactionsData();
00024:         private MoralChoiceFlagDefinitions _moralFlagDefs = new MoralChoiceFlagDefinitions();
00025:         private MoralChoiceGossipRuntime _moralGossipRuntime = null!;
00026:
00027:         /// <summary>
00028:         /// Fixed world seed so every host agrees on unseeded rolls; per-save
00029:         /// outcome rolls and propagation days are stored in the ledger DTO.
00030:         /// </summary>
00031:         private const int MoralChoiceSeed = 20260825;
00032:
00033:         private void SetupMoralChoice()
00034:         {
00035:             if (_moralChoice != null) return;
00036:             SetupJournal();
00037:             SetupCampaignDay();
00038:             var fileIO = new FileSystemIO();
00039:             var json = new SystemTextJsonSerializer();
00040:
00041:             _moralChoice = new MoralChoiceSystem(_campaignDay.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.MoralChoice).Rng, flags: _consequenceLedger);
00042:             _moralChoiceDefs = MoralChoiceCatalogLoader.Load(_dataDir, fileIO, json);
00043:
00044:             // Load branching chain quests and merge into the catalog
00045:             var chainQuests = MoralChoiceBranchQuestCatalogLoader.Load(_dataDir, fileIO, json);
00046:             _moralChoiceDefs.AddRange(chainQuests);
00047:
00048:             // Load expansion quests and merge into the catalog
00049:             var expansionQuests = MoralChoiceExpansionQuestCatalogLoader.Load(_dataDir, fileIO, json);
00050:             _moralChoiceDefs.AddRange(expansionQuests);
00051:
00052:             // Register all definitions into the Core system's authoritative catalog
00053:             _moralChoice.RegisterQuests(_moralChoiceDefs);
00054:
00055:             // Load chain architecture (branches, gates, echo quests)
00056:             _moralChainData = MoralChoiceChainCatalogLoader.Load(_dataDir, fileIO, json);
00057:             _moralChoice.InitializeChainData(_moralChainData);
00058:
00059:             // Load gossip, faction reactions, and flag definitions
00060:             _moralGossipData = MoralChoiceGossipCatalogLoader.Load(_dataDir, fileIO, json);
00061:             _moralFactionReactions = MoralChoiceFactionReactionsCatalogLoader.Load(_dataDir, fileIO, json);
00062:             _moralFlagDefs = MoralChoiceFlagCatalogLoader.Load(_dataDir, fileIO, json);
00063:             _moralGossipRuntime = new MoralChoiceGossipRuntime(_moralGossipData, _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.MoralChoice, 0, 1));
00064:
00065:             _moralChoice.OnQuestResolved += WriteMoralChoiceJournalEntry;
00066:             _moralChoice.OnQuestResolved += _ => _moralChoiceDirty = true;
00067:             // Plan IV Task 5: a resolved trapping dilemma acks its pending
00068:             // outbox fact so the outbox never re-surfaces it after restore.
00069:             _moralChoice.OnQuestResolved += resolution =>
00070:             {
00071:                 if (resolution.questId != null && resolution.questId.StartsWith(TrappingMoralQuestIdPrefix, StringComparison.Ordinal))
00072:                     MarkTrappingMoralEventDelivered(resolution.questId);
00073:             };
00074:             _moralChoice.OnThresholdEventFired += WriteThresholdEventJournalEntry;
00075:             _moralChoice.OnThresholdEventFired += _ => _moralChoiceDirty = true;
00076:             _moralChoice.OnBranchLocked += WriteBranchLockoutJournalEntry;
00077:             _moralChoice.OnBranchLocked += _ => _moralChoiceDirty = true;
00078:
00079:             var save = MoralChoiceSaveStore.TryLoad();
00080:             if (save != null)
00081:             {
00082:                 try
00083:                 {
00084:                     _moralChoice.RestoreState(save);
00085:                     GD.Print($"[Ashfall Godot] Moral choice ledger restored " +
00086:                              $"(day {save.lastReconciledDay}, {_moralChoice.QuestsResolved} resolved).");
00087:                 }
00088:                 catch (Exception e)
00089:                 {
00090:                     GD.PrintErr($"[Ashfall Godot] Moral choice restore rejected: {e.Message}");
00091:                 }
00092:             }
00093:             GD.Print($"[Ashfall Godot] Moral choice ready. {_moralChoiceDefs.Count} quests " +
00094:                      $"({_moralChainData.Branches.Count} branches, " +
00095:                      $"{_moralGossipData.CampChatter.Neutral.Count} neutral chatter lines).");
00096:         }
00097:
00098:         public MoralChoiceSystem MoralChoice => _moralChoice;
00099:         public IReadOnlyList<MoralChoiceQuestDefinition> MoralChoiceDefs => _moralChoiceDefs;
00100:
00101:         public MoralChoiceQuestDefinition? GetMoralChoiceDef(string questId)
00102:         {
00103:             SetupMoralChoice();
00104:             return _moralChoiceDefs.FirstOrDefault(
00105:                 d => string.Equals(d.Id, questId, StringComparison.Ordinal));
00106:         }
00107:
00108:         public List<MoralChoiceQuestDefinition> GetAvailableMoralChoices()
00109:         {
00110:             SetupMoralChoice();
00111:             var list = new List<MoralChoiceQuestDefinition>();
00112:             foreach (var d in _moralChoiceDefs)
00113:             {
00114:                 // Plan IV Task 5: trapping-sourced dilemmas are excluded from
00115:                 // the standing offer pass — they surface only while their
00116:                 // moral-consequence fact is pending in the trapping outbox.
00117:                 if (d.Id != null && d.Id.StartsWith(TrappingMoralQuestIdPrefix, StringComparison.Ordinal))
00118:                     continue;
00119:                 if (!_moralChoice.IsResolved(d.Id) &&
00120:                     MoralChoiceSystem.IsAvailableOnDay(d, _simDay) &&
00121:                     _moralChoice.IsChainQuestAccessible(d.Id, _simDay))
00122:                 {
00123:                     list.Add(d);
00124:                 }
00125:             }
00126:
00127:             // Plan IV Task 5: surface pending trapping dilemmas in outbox
00128:             // sequence order. The pending fact is the persistence owner until
00129:             // the player resolves the dilemma in the moral ledger.
00130:             if (_wildlifeTrapping != null)
00131:             {
00132:                 var pending = _wildlifeTrapping.System.GetPendingEvents();
00133:                 foreach (var ev in pending)
00134:                 {
00135:                     if (ev == null || !string.Equals(ev.kind, WildlifeTrappingEventKinds.MoralConsequence, StringComparison.Ordinal))
00136:                         continue;
00137:                     var def = GetMoralChoiceDef(ev.payloadId);
00138:                     if (def == null || _moralChoice.IsResolved(def.Id)) continue;
00139:                     if (!list.Any(l => string.Equals(l.Id, def.Id, StringComparison.Ordinal)))
00140:                         list.Add(def);
00141:                 }
00142:             }
00143:             return list;
00144:         }
00145:
00146:         /// <summary>Catalog id prefix shared by all trapping-sourced moral dilemmas.</summary>
00147:         public const string TrappingMoralQuestIdPrefix = "quest_moral_trap_prey_";
00148:
00149:         /// <summary>
00150:         /// Plan IV Task 5: ack every pending moral-consequence fact that maps
00151:         /// to the given quest id. Called from the resolution event so the
00152:         /// trapping outbox marks the fact delivered only after the moral
00153:         /// ledger committed the resolution.
00154:         /// </summary>
00155:         private void MarkTrappingMoralEventDelivered(string questId)
00156:         {
00157:             if (_wildlifeTrapping == null) return;
00158:             var pending = _wildlifeTrapping.System.GetPendingEvents();
00159:             foreach (var ev in pending)
00160:             {
00161:                 if (ev == null || !string.Equals(ev.kind, WildlifeTrappingEventKinds.MoralConsequence, StringComparison.Ordinal))
00162:                     continue;
00163:                 if (!string.Equals(ev.payloadId, questId, StringComparison.Ordinal)) continue;
00164:                 _wildlifeTrapping.System.MarkEventDelivered(ev.eventId);
00165:             }
00166:         }
00167:
00168:         public List<MoralChoiceQuestDefinition> GetResolvedMoralChoices()
00169:         {
00170:             SetupMoralChoice();
00171:             var list = new List<MoralChoiceQuestDefinition>();
00172:             foreach (var d in _moralChoiceDefs)
00173:             {
00174:                 if (_moralChoice.IsResolved(d.Id))
00175:                     list.Add(d);
00176:             }
00177:             return list;
00178:         }
00179:
00180:         public MoralChoiceResolution? GetMoralChoiceResolution(string questId)
00181:         {
00182:             SetupMoralChoice();
00183:             if (_moralChoice.TryGetResolution(questId, out var res))
00184:                 return res;
00185:             return null;
00186:         }
00187:
00188:         public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyMoralOffers(int maxOffers = 1)
00189:         {
00190:             SetupMoralChoice();
00191:             return _moralChoice.GetDailyOffers(_simDay, maxOffers);
00192:         }
00193:
00194:         /// <summary>
00195:         /// Resolve a catalog quest by id. Returns false when the id is unknown
00196:         /// or the quest is already resolved; the journal line is written by
00197:         /// the event hook, and overnight settlement lands in TickSimDay.
00198:         /// </summary>
00199:         public bool TryResolveMoralChoice(string questId, int choiceIndex)
00200:         {
00201:             SetupMoralChoice();
00202:             var def = _moralChoiceDefs.FirstOrDefault(
00203:                 d => string.Equals(d.Id, questId, StringComparison.Ordinal));
00204:             if (def == null) return false;
00205:             if (!_moralChoice.TryResolve(questId, choiceIndex, def.LocationId, _simDay, out var resolveResult))
00206:                 return false;
00207:
00208:             // CORE-MECH W8: a choice with a public footprint seeds the canonical
00209:             // rumor network. RumorSystem stays the only rumor authority; the seed
00210:             // builder is pure; the one-shot trigger guarantees one seed per choice.
00211:             SeedMoralChoiceGossip(resolveResult?.Resolution);
00212:
00213:             _moralChoiceDirty = true;
00214:             AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.UiConfirm);
00215:             return true;
00216:         }
00217:
00218:         /// <summary>
00219:         /// CORE-MECH W8 — turn a resolved choice into a rumor seed and hand it to
00220:         /// the existing RumorSystem. Uses the authored <c>propagatesOnDay</c> hook
00221:         /// (gossip leaves the witnessing circle on resolvedDay + 1..3). Fails closed
00222:         /// when the info owner is not set up: the choice still resolves.
00223:         /// </summary>
00224:         private void SeedMoralChoiceGossip(Ashfall.Core.MoralChoice.MoralChoiceResolution? resolution)
00225:         {
00226:             if (resolution == null || string.IsNullOrEmpty(resolution.questId)) return;
00227:
00228:             float impact = Math.Clamp(Math.Abs(resolution.moralDelta) / 20f, 0f, 1f);
00229:             if (resolution.empathyDelta > 0) impact = Math.Clamp(impact + 0.15f, 0f, 1f);
00230:
00231:             var seed = Ashfall.Core.MoralChoice.MoralChoiceGossipSeed.Build(
00232:                 resolution.questId,
00233:                 resolution.locationId,
00234:                 resolution.propagatesOnDay > 0 ? resolution.propagatesOnDay : resolution.resolvedDay,
00235:                 resolution.epitaph,
00236:                 impact,
00237:                 isPrivate: false);
00238:             if (seed == null) return;
00239:
00240:             SetupRumorNetwork();
00241:             if (_rumorNetwork == null) return;
00242:
00243:             // One seed per choice, ever — the W11 primitive rebuilt inline to clear
00244:             // the W8→W11 ordering dependency.
00245:             var triggers = GossipTriggers();
00246:             if (!triggers.TryFire("moral." + seed.SubjectId, Math.Max(1, seed.OriginDay)))
00247:                 return;
00248:
00249:             var rumor = _rumorNetwork.System.GenerateRumor(
00250:                 seed.OriginLocationId,
00251:                 Ashfall.Core.InformationFlow.RumorSubjectType.Faction,
00252:                 seed.SubjectId,
00253:                 seed.Headline,
00254:                 seed.Description,
00255:                 seed.Truthfulness,
00256:                 seed.OriginDay);
00257:             rumor.DecayRate = seed.DecayRate;
00258:             rumor.PropagationSpeed = seed.PropagationSpeed;
00259:             // The rumor host raises its own StateChanged on generation (Main binds
00260:             // that to the dirty flag), so the seed needs no extra bookkeeping.
00261:             _rumorNetworkDirty = true;
00262:         }
00263:
00264:         /// <summary>W8/W11 shared trigger ledger over the campaign consequence ledger.</summary>
00265:         private Ashfall.Core.Flags.OneShotTriggerLedger GossipTriggers()
00266:             => _gossipTriggers ??= new Ashfall.Core.Flags.OneShotTriggerLedger(_consequenceLedger);
00267:
00268:         private Ashfall.Core.Flags.OneShotTriggerLedger? _gossipTriggers;
00269:
00270:         /// <summary>Journal integration: one entry per resolution, arrow only — never the number.</summary>
00271:         private void WriteMoralChoiceJournalEntry(MoralChoiceResolution resolution)
00272:         {
00273:             SetupJournal();
00274:             string arrow = resolution.impactMark == "up" ? "🔺"
00275:                 : resolution.impactMark == "down" ? "🔻" : "⚪";
00276:             _journal.TryAddRawEntry(resolution.questId, $"{arrow} {resolution.epitaph}", null!, resolution.resolvedDay);
00277:             _journalDirty = true;
00278:         }
00279:
00280:         /// <summary>Branch lockout journal entry: a door has closed.</summary>
00281:         private void WriteBranchLockoutJournalEntry(string lockedBranchId)
00282:         {
00283:             if (_moralChainData?.LockoutRules == null) return;
00284:             var branch = _moralChainData.Branches.FirstOrDefault(
00285:                 b => string.Equals(b.Id, lockedBranchId, StringComparison.Ordinal));
00286:             string branchName = branch?.DisplayName ?? lockedBranchId;
00287:             string template = _moralChainData.LockoutRules.LockoutJournalTemplate;
00288:             string text = template.Replace("{locked_branch_name}", branchName);
00289:
00290:             SetupJournal();
00291:             _journal.TryAddRawEntry($"branch_lockout_{lockedBranchId}", text, null!, _simDay);
00292:             _journalDirty = true;
00293:         }
00294:
00295:         /// <summary>
00296:         /// Get the faction reaction dialogue for a threshold event.
00297:         /// Returns null if no reaction data exists for the event.
00298:         /// </summary>
00299:         private MoralThresholdReaction? GetFactionReaction(string eventId)
00300:         {
00301:             SetupMoralChoice();
00302:             if (_moralFactionReactions.ThresholdReactions.TryGetValue(eventId, out var reaction))
00303:                 return reaction;
00304:             return null;
00305:         }
00306:
00307:         /// <summary>
00308:         /// Journal the authored faction reaction when a moral threshold fires.
00309:         /// Uses the catalog journal line when present; otherwise a restrained fallback.
00310:         /// </summary>
00311:         private void WriteThresholdEventJournalEntry(string eventId)
00312:         {
00313:             var reaction = GetFactionReaction(eventId);
00314:             string text = reaction != null && !string.IsNullOrWhiteSpace(reaction.JournalEntry)
00315:                 ? reaction.JournalEntry
00316:                 : $"Threshold crossed: {eventId}.";
00317:
00318:             SetupJournal();
00319:             _journal.TryAddRawEntry($"moral_threshold_{eventId}", text, null!, _simDay);
00320:             _journalDirty = true;
00321:         }
00322:
00323:         /// <summary>
00324:         /// Get the current gossip band (with decay) for NPC interactions.
00325:         /// </summary>
00326:         private MoralPathBand GetCurrentGossipBand()
00327:         {
00328:             SetupMoralChoice();
00329:             return _moralGossipRuntime.GetEffectiveGossipBand(_moralChoice, _simDay);
00330:         }
00331:
00332:         private void SaveMoralChoice()
00333:         {
00334:             if (_moralChoice == null) return;
00335:             if (CaptureSection("moral_choice", MoralChoiceSaveStore.TryCapturePersisted(_moralChoice.CaptureState())))
00336:                 _moralChoiceDirty = false;
00337:         }
00338:
00339:         private void FlushMoralChoiceIfDirty()
00340:         {
00341:             if (_moralChoiceDirty) SaveMoralChoice();
00342:         }
00343:     }
00344: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/MoralChoiceFlagPlan125Tests.cs`

### `Ashfall.Core.Tests/MoralChoiceFlagPlan125Tests.cs` — complete current file

- Size: 137 lines / 5614 bytes.
- SHA-256: `3ab7d00efd1e4ccba071b0bab44ab7b9cac89422dab5b463ec0e7c86b7495307`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005: using Xunit;
00006: using Ashfall.Core;
00007: using Ashfall.Core.MoralChoice;
00008:
00009: namespace Ashfall.Core.Tests
00010: {
00011:     public sealed class MoralChoiceFlagPlan125Tests : CatalogTestBase
00012:     {
00013:         private static readonly IFileIO s_files = new FileSystemIO();
00014:         private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
00015:
00016:         private static readonly string[] s_newFlags =
00017:         {
00018:             "flag_spared_raider",
00019:             "flag_executed_prisoner",
00020:             "flag_shared_rations",
00021:             "flag_hoarded_medicine",
00022:             "flag_sheltered_refugee",
00023:             "flag_expelled_survivor",
00024:             "flag_repaired_infrastructure",
00025:             "flag_sabotaged_rival",
00026:             "flag_broke_treaty",
00027:             "flag_honored_debt",
00028:             "flag_ignored_distress",
00029:             "flag_responded_distress",
00030:             "flag_forged_record",
00031:             "flag_preserved_archive",
00032:             "flag_chosen_faction_side"
00033:         };
00034:
00035:         [Fact]
00036:         public void CatalogContainsExactlyTwentyFiveUniqueHistoricalFlags()
00037:         {
00038:             var definitions = MoralChoiceFlagCatalogLoader.Load(DataDirectory, s_files, s_json);
00039:             Assert.Equal(25, definitions.Flags.Count);
00040:             Assert.Equal(25, definitions.Flags.Select(f => f.Id).Distinct(StringComparer.Ordinal).Count());
00041:             Assert.All(definitions.Flags, flag =>
00042:             {
00043:                 Assert.StartsWith("flag_", flag.Id, StringComparison.Ordinal);
00044:                 Assert.False(string.IsNullOrWhiteSpace(flag.DisplayName));
00045:             });
00046:             Assert.All(s_newFlags, id => Assert.Contains(definitions.Flags, flag => flag.Id == id));
00047:         }
00048:
00049:         [Fact]
00050:         public void CatalogIdsAreSynchronizedWithStaticIds()
00051:         {
00052:             var catalogIds = MoralChoiceFlagCatalogLoader.Load(DataDirectory, s_files, s_json)
00053:                 .Flags.Select(f => f.Id).ToHashSet(StringComparer.Ordinal);
00054:             var staticIds = MoralChoiceIds.AllFlags.ToHashSet(StringComparer.Ordinal);
00055:
00056:             Assert.Contains(MoralChoiceIds.FlagMessengerKept, staticIds);
00057:             Assert.True(catalogIds.IsSubsetOf(staticIds));
00058:             Assert.Equal(26, MoralChoiceIds.AllFlags.Length);
00059:         }
00060:
00061:         [Fact]
00062:         public void EveryPlan125FlagHasARealMoralChoiceProducer()
00063:         {
00064:             var quests = new List<MoralChoiceQuestDefinition>();
00065:             quests.AddRange(MoralChoiceCatalogLoader.LoadStubs(DataDirectory, s_files, s_json));
00066:             quests.AddRange(MoralChoiceBranchQuestCatalogLoader.Load(DataDirectory, s_files, s_json));
00067:             quests.AddRange(MoralChoiceExpansionQuestCatalogLoader.Load(DataDirectory, s_files, s_json));
00068:
00069:             var producers = quests
00070:                 .SelectMany(q => q.Choices.Select((choice, index) => new { q.Id, index, choice.SetFlag }))
00071:                 .Where(p => !string.IsNullOrWhiteSpace(p.SetFlag))
00072:                 .GroupBy(p => p.SetFlag, StringComparer.Ordinal)
00073:                 .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.Ordinal);
00074:
00075:             foreach (string flag in s_newFlags)
00076:             {
00077:                 Assert.True(producers.TryGetValue(flag, out var matches), $"No producer for {flag}");
00078:                 Assert.NotEmpty(matches!);
00079:             }
00080:         }
00081:
00082:         [Fact]
00083:         public void ConfiguredProducerWritesFlagOnlyAfterResolutionAndIsIdempotent()
00084:         {
00085:             var quest = new MoralChoiceQuestDefinition
00086:             {
00087:                 Id = "quest_moral_plan125_write",
00088:                 Choices = new List<MoralChoiceOption>
00089:                 {
00090:                     new MoralChoiceOption { Label = "Commit", SetFlag = "flag_shared_rations" },
00091:                     new MoralChoiceOption { Label = "Decline" }
00092:                 }
00093:             };
00094:             var system = new MoralChoiceSystem(new SeededRng(125));
00095:             system.RegisterQuest(quest);
00096:             Assert.False(system.HasFlag("flag_shared_rations"));
00097:
00098:             system.Resolve(quest, 0, string.Empty, 12);
00099:
00100:             Assert.True(system.HasFlag("flag_shared_rations"));
00101:             Assert.Single(system.State.activeFlags, id => id == "flag_shared_rations");
00102:
00103:             system.Resolve(quest, 1, string.Empty, 99);
00104:
00105:             Assert.Single(system.State.activeFlags, id => id == "flag_shared_rations");
00106:         }
00107:
00108:         [Fact]
00109:         public void OldStateLeavesNewFlagsUnsetAndRoundTripsExistingHistory()
00110:         {
00111:             var oldState = new MoralChoiceState();
00112:             oldState.activeFlags.Add(MoralChoiceIds.FlagBrokenPact);
00113:
00114:             var restored = new MoralChoiceSystem(new SeededRng(125));
00115:             restored.RestoreState(oldState);
00116:
00117:             Assert.True(restored.HasFlag(MoralChoiceIds.FlagBrokenPact));
00118:             Assert.False(restored.HasFlag("flag_preserved_archive"));
00119:             Assert.False(restored.HasFlag("flag_broke_treaty"));
00120:         }
00121:
00122:         [Fact]
00123:         public void RaiderResolutionKeepsSameIncidentChoicesMutuallyExclusive()
00124:         {
00125:             var raider = MoralChoiceCatalogLoader.Load(DataDirectory, s_files, s_json)
00126:                 .Single(q => q.Id == MoralChoiceIds.ShareRaider);
00127:             var system = new MoralChoiceSystem(new SeededRng(125));
00128:             system.RegisterQuest(raider);
00129:
00130:             system.Resolve(raider, 0, raider.LocationId, 1);
00131:
00132:             Assert.True(system.HasFlag("flag_spared_raider"));
00133:             Assert.False(system.HasFlag("flag_executed_prisoner"));
00134:             Assert.False(system.HasFlag("flag_shared_rations"));
00135:         }
00136:     }
00137: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagDefinitions.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagDefinitions.cs` — complete current file

- Size: 20 lines / 634 bytes.
- SHA-256: `764eda5eb3dce4983316d0a4b40463a539e49aa5ff7cd4a1031e0f733be970de`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003:
00004: namespace Ashfall.Core.MoralChoice
00005: {
00006:     /// <summary>
00007:     /// Wire shape for moral_choice_flags.json — persistent moral-history
00008:     /// definitions used by quest access, branch locking, and later predicates.
00009:     /// </summary>
00010:     public sealed class MoralChoiceFlagDefinitions
00011:     {
00012:         public List<MoralFlagDefinition> Flags { get; set; } = new List<MoralFlagDefinition>();
00013:     }
00014:
00015:     public sealed class MoralFlagDefinition
00016:     {
00017:         public string Id { get; set; } = string.Empty;
00018:         public string DisplayName { get; set; } = string.Empty;
00019:     }
00020: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs` — complete current file

- Size: 259 lines / 15870 bytes.
- SHA-256: `92baf733ed69c3c0f1b9881ed7fdfc252ee7567c26f551c1f400a9d5f9261fb1`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Linq;
00004:
00005: namespace Ashfall.Core.MoralChoice
00006: {
00007:     /// <summary>
00008:     /// Canonical quest ids and flags for the moral choice system
00009:     /// ("The Weight of Survival"), pinned 1:1 against the data files.
00010:     /// Base catalog: moral_choice_quests.json (68 quests).
00011:     /// Chain quests: moral_choice_quests_branching.json (100 quests, 4 × 25).
00012:     /// Expansion quests: moral_choice_quests_expansion.json (50 quests).
00013:     /// Threshold event ids live on MoralChoiceSystem; this class owns quest
00014:     /// and flag ids.
00015:     /// </summary>
00016:     public static class MoralChoiceIds
00017:     {
00018:         public const int BaseQuestCount = 68;
00019:         public const int ChainQuestCount = 100;
00020:         public const int ExpansionQuestCount = 50;
00021:         public const int TotalQuestCount = BaseQuestCount + ChainQuestCount + ExpansionQuestCount;
00022:
00023:         // ── Base: Sharing Supplies (13) ─────────────────────────────────
00024:         public const string ShareChild = "quest_moral_share_child";
00025:         public const string ShareFamily = "quest_moral_share_family";
00026:         public const string ShareInjured = "quest_moral_share_injured";
00027:         public const string ShareWater = "quest_moral_share_water";
00028:         public const string ShareElder = "quest_moral_share_elder";
00029:         public const string SharePregnant = "quest_moral_share_pregnant";
00030:         public const string ShareRaider = "quest_moral_share_raider";
00031:         public const string SharePeacekeeper = "quest_moral_share_peacekeeper";
00032:         public const string ShareKeeper = "quest_moral_share_keeper";
00033:         public const string ShareBanditLeader = "quest_moral_share_bandit_leader";
00034:         public const string ShareScientist = "quest_moral_share_scientist";
00035:         public const string ShareFarmer = "quest_moral_share_farmer";
00036:         public const string ShareScavengerChild = "quest_moral_env_scavenger_child";
00037:
00038:         // ── Base: Listening to Stories (13) ─────────────────────────────
00039:         public const string ListenOldMan = "quest_moral_listen_oldman";
00040:         public const string ListenMother = "quest_moral_listen_mother";
00041:         public const string ListenSoldier = "quest_moral_listen_soldier";
00042:         public const string ListenChild = "quest_moral_listen_child";
00043:         public const string ListenDoctor = "quest_moral_listen_doctor";
00044:         public const string ListenPreacher = "quest_moral_listen_preacher";
00045:         public const string ListenEngineer = "quest_moral_listen_engineer";
00046:         public const string ListenWarning = "quest_moral_listen_warning";
00047:         public const string ListenLover = "quest_moral_listen_lover";
00048:         public const string ListenTeacher = "quest_moral_listen_teacher";
00049:         public const string ListenThief = "quest_moral_listen_thief";
00050:         public const string ListenProphet = "quest_moral_listen_prophet";
00051:         public const string ListenBuriedLetters = "quest_moral_env_buried_letters";
00052:
00053:         // ── Base: Offering Comfort (13) ─────────────────────────────────
00054:         public const string ComfortWidow = "quest_moral_comfort_widow";
00055:         public const string ComfortChild = "quest_moral_comfort_child";
00056:         public const string ComfortInjured = "quest_moral_comfort_injured";
00057:         public const string ComfortFear = "quest_moral_comfort_fear";
00058:         public const string ComfortAddict = "quest_moral_comfort_addict";
00059:         public const string ComfortGuilt = "quest_moral_comfort_guilt";
00060:         public const string ComfortElder = "quest_moral_comfort_elder";
00061:         public const string ComfortNightmare = "quest_moral_comfort_nightmare";
00062:         public const string ComfortLoneliness = "quest_moral_comfort_loneliness";
00063:         public const string ComfortAnger = "quest_moral_comfort_anger";
00064:         public const string ComfortHope = "quest_moral_comfort_hope";
00065:         public const string ComfortDespair = "quest_moral_comfort_despair";
00066:         public const string ComfortWoundedScavenger = "quest_moral_env_wounded_scavenger";
00067:
00068:         // ── Base: Respecting the Dead (13) ──────────────────────────────
00069:         public const string DeadUnmarked = "quest_moral_dead_unmarked";
00070:         public const string DeadBurned = "quest_moral_dead_burned";
00071:         public const string DeadBloated = "quest_moral_dead_bloated";
00072:         public const string DeadChild = "quest_moral_dead_child";
00073:         public const string DeadMass = "quest_moral_dead_mass";
00074:         public const string DeadHanged = "quest_moral_dead_hanged";
00075:         public const string DeadCloset = "quest_moral_dead_closet";
00076:         public const string DeadWater = "quest_moral_dead_water";
00077:         public const string DeadCremated = "quest_moral_dead_cremated";
00078:         public const string DeadExecuted = "quest_moral_dead_executed";
00079:         public const string DeadSuicide = "quest_moral_dead_suicide";
00080:         public const string DeadMassacre = "quest_moral_dead_massacre";
00081:         public const string DeadExplorer = "quest_moral_env_dead_explorer";
00082:
00083:         // ── Base: Trusting Strangers (13) ───────────────────────────────
00084:         public const string TrustFire = "quest_moral_trust_fire";
00085:         public const string TrustWounded = "quest_moral_trust_wounded";
00086:         public const string TrustMerchant = "quest_moral_trust_merchant";
00087:         public const string TrustChild = "quest_moral_trust_child";
00088:         public const string TrustDeserter = "quest_moral_trust_deserter";
00089:         public const string TrustWoman = "quest_moral_trust_woman";
00090:         public const string TrustSoldier = "quest_moral_trust_soldier";
00091:         public const string TrustRunaway = "quest_moral_trust_runaway";
00092:         public const string TrustSilent = "quest_moral_trust_silent";
00093:         public const string TrustSignal = "quest_moral_trust_signal";
00094:         public const string TrustBorrower = "quest_moral_trust_borrower";
00095:         public const string TrustMessenger = "quest_moral_trust_messenger";
00096:         public const string TrustShelterRefugee = "quest_moral_env_shelter_refugee";
00097:
00098:         // ── Chain: Mercy Road (25) ──────────────────────────────────────
00099:         public static readonly string[] ChainMercy = Enumerable.Range(1, 25)
00100:             .Select(i => $"quest_moral_chain_mercy_{i:D2}").ToArray();
00101:
00102:         // ── Chain: Iron Way (25) ────────────────────────────────────────
00103:         public static readonly string[] ChainIron = Enumerable.Range(1, 25)
00104:             .Select(i => $"quest_moral_chain_iron_{i:D2}").ToArray();
00105:
00106:         // ── Chain: Listener's Thread (25) ───────────────────────────────
00107:         public static readonly string[] ChainListen = Enumerable.Range(1, 25)
00108:             .Select(i => $"quest_moral_chain_listen_{i:D2}").ToArray();
00109:
00110:         // ── Chain: Broken Compact (25) ──────────────────────────────────
00111:         public static readonly string[] ChainBetray = Enumerable.Range(1, 25)
00112:             .Select(i => $"quest_moral_chain_betray_{i:D2}").ToArray();
00113:
00114:         /// <summary>All 100 chain quest ids (mercy + iron + listen + betray).</summary>
00115:         public static readonly string[] AllChain =
00116:             ChainMercy.Concat(ChainIron).Concat(ChainListen).Concat(ChainBetray).ToArray();
00117:
00118:         // ── Expansion (50) ──────────────────────────────────────────────
00119:         public static readonly string[] AllExpansion =
00120:         {
00121:             "quest_moral_share_medicine",
00122:             "quest_moral_share_coat",
00123:             "quest_moral_share_fire",
00124:             "quest_moral_trust_scout",
00125:             "quest_moral_trust_stranger_water",
00126:             "quest_moral_comfort_dying_stranger",
00127:             "quest_moral_comfort_grieving",
00128:             "quest_moral_dead_unburied",
00129:             "quest_moral_dead_name",
00130:             "quest_moral_listen_confession",
00131:             "quest_moral_listen_old_story",
00132:             "quest_moral_share_seed",
00133:             "quest_moral_trust_coded_message",
00134:             "quest_moral_comfort_broken",
00135:             "quest_moral_dead_mass_grave",
00136:             "quest_moral_listen_child_question",
00137:             "quest_moral_share_shelter",
00138:             "quest_moral_trust_traitor",
00139:             "quest_moral_comfort_letter",
00140:             "quest_moral_dead_dog",
00141:             "quest_moral_listen_singer",
00142:             "quest_moral_share_skill",
00143:             "quest_moral_trust_child_message",
00144:             "quest_moral_comfort_despairing",
00145:             "quest_moral_dead_last_request",
00146:             "quest_moral_listen_veteran",
00147:             "quest_moral_share_last_match",
00148:             "quest_moral_trust_map",
00149:             "quest_moral_comfort_dying_alone",
00150:             "quest_moral_dead_unknown",
00151:             "quest_moral_listen_prophecy",
00152:             "quest_moral_share_blanket",
00153:             "quest_moral_trust_returned",
00154:             "quest_moral_comfort_reminder",
00155:             "quest_moral_dead_pet",
00156:             "quest_moral_listen_drunkard",
00157:             "quest_moral_share_rope",
00158:             "quest_moral_trust_camp_invite",
00159:             "quest_moral_comfort_old_fear",
00160:             "quest_moral_dead_letter_unsent",
00161:             "quest_moral_listen_silence",
00162:             "quest_moral_share_skill_medical",
00163:             "quest_moral_trust_returned_thief",
00164:             "quest_moral_comfort_burned",
00165:             "quest_moral_dead_mass_burial_help",
00166:             "quest_moral_listen_rumor",
00167:             "quest_moral_share_lamp_oil",
00168:             "quest_moral_trust_orphan",
00169:             "quest_moral_comfort_remorse",
00170:             "quest_moral_dead_ceremony",
00171:         };
00172:
00173:         /// <summary>All 68 base quest ids in catalog order.</summary>
00174:         public static readonly string[] All =
00175:         {
00176:             ShareChild, ShareFamily, ShareInjured, ShareWater, ShareElder, SharePregnant,
00177:             ShareRaider, SharePeacekeeper, ShareKeeper, ShareBanditLeader, ShareScientist, ShareFarmer,
00178:             ShareScavengerChild,
00179:             ListenOldMan, ListenMother, ListenSoldier, ListenChild, ListenDoctor, ListenPreacher,
00180:             ListenEngineer, ListenWarning, ListenLover, ListenTeacher, ListenThief, ListenProphet,
00181:             ListenBuriedLetters,
00182:             ComfortWidow, ComfortChild, ComfortInjured, ComfortFear, ComfortAddict, ComfortGuilt,
00183:             ComfortElder, ComfortNightmare, ComfortLoneliness, ComfortAnger, ComfortHope, ComfortDespair,
00184:             ComfortWoundedScavenger,
00185:             DeadUnmarked, DeadBurned, DeadBloated, DeadChild, DeadMass, DeadHanged,
00186:             DeadCloset, DeadWater, DeadCremated, DeadExecuted, DeadSuicide, DeadMassacre,
00187:             DeadExplorer,
00188:             TrustFire, TrustWounded, TrustMerchant, TrustChild, TrustDeserter, TrustWoman,
00189:             TrustSoldier, TrustRunaway, TrustSilent, TrustSignal, TrustBorrower, TrustMessenger,
00190:             TrustShelterRefugee,
00191:
00192:             // ── Trapping (3) — Flagship Plan IV Task 5 ───────────────────
00193:             TrapPreyHigh, TrapPreyMedium, TrapPreyLow,
00194:         };
00195:
00196:         // ── Trapping (3) — Flagship Plan IV Task 5 ──────────────────
00197:         public const string TrapPreyHigh = "quest_moral_trap_prey_high";
00198:         public const string TrapPreyMedium = "quest_moral_trap_prey_medium";
00199:         public const string TrapPreyLow = "quest_moral_trap_prey_low";
00200:
00201:         // ── Branch flags ────────────────────────────────────────────────
00202:         public const string FlagMercyRoadLocked = "flag_branch_mercy_road_locked";
00203:         public const string FlagIronWayLocked = "flag_branch_iron_way_locked";
00204:         public const string FlagListenerLocked = "flag_branch_listener_locked";
00205:         public const string FlagBrokenCompactLocked = "flag_branch_broken_compact_locked";
00206:
00207:         // ── Narrative flags ─────────────────────────────────────────────
00208:         public const string FlagBetrayedAlly = "flag_betrayed_ally";
00209:         public const string FlagBetrayedFaction = "flag_betrayed_faction";
00210:         public const string FlagBetrayedTrust = "flag_betrayed_trust";
00211:         public const string FlagBrokenPact = "flag_broken_pact";
00212:         public const string FlagBecomeWarlord = "flag_become_warlord";
00213:         public const string FlagThroneOfAsh = "flag_throne_of_ash";
00214:
00215:         // ── Persistent moral-memory flags (Plan 125) ──────────────────
00216:         public const string FlagSparedRaider = "flag_spared_raider";
00217:         public const string FlagExecutedPrisoner = "flag_executed_prisoner";
00218:         public const string FlagSharedRations = "flag_shared_rations";
00219:         public const string FlagHoardedMedicine = "flag_hoarded_medicine";
00220:         public const string FlagShelteredRefugee = "flag_sheltered_refugee";
00221:         public const string FlagExpelledSurvivor = "flag_expelled_survivor";
00222:         public const string FlagRepairedInfrastructure = "flag_repaired_infrastructure";
00223:         public const string FlagSabotagedRival = "flag_sabotaged_rival";
00224:         public const string FlagBrokeTreaty = "flag_broke_treaty";
00225:         public const string FlagHonoredDebt = "flag_honored_debt";
00226:         public const string FlagIgnoredDistress = "flag_ignored_distress";
00227:         public const string FlagRespondedDistress = "flag_responded_distress";
00228:         public const string FlagForgedRecord = "flag_forged_record";
00229:         public const string FlagPreservedArchive = "flag_preserved_archive";
00230:         public const string FlagChosenFactionSide = "flag_chosen_faction_side";
00231:
00232:         /// <summary>Set when the Dying Messenger's packet is delivered unopened — a Storykeeper key.</summary>
00233:         public const string FlagMessengerKept = "flag_moral_messenger_kept";
00234:
00235:         /// <summary>All 26 moral flag ids, including the external messenger marker.</summary>
00236:         public static readonly string[] AllFlags =
00237:         {
00238:             FlagMercyRoadLocked, FlagIronWayLocked, FlagListenerLocked, FlagBrokenCompactLocked,
00239:             FlagBetrayedAlly, FlagBetrayedFaction, FlagBetrayedTrust, FlagBrokenPact,
00240:             FlagBecomeWarlord, FlagThroneOfAsh,
00241:             FlagSparedRaider, FlagExecutedPrisoner, FlagSharedRations, FlagHoardedMedicine,
00242:             FlagShelteredRefugee, FlagExpelledSurvivor, FlagRepairedInfrastructure,
00243:             FlagSabotagedRival, FlagBrokeTreaty, FlagHonoredDebt, FlagIgnoredDistress,
00244:             FlagRespondedDistress, FlagForgedRecord, FlagPreservedArchive, FlagChosenFactionSide,
00245:             FlagMessengerKept
00246:         };
00247:
00248:         // ── Branch ids ──────────────────────────────────────────────────
00249:         public const string BranchMercyRoad = "branch_mercy_road";
00250:         public const string BranchIronWay = "branch_iron_way";
00251:         public const string BranchListenerThread = "branch_listener_thread";
00252:         public const string BranchBrokenCompact = "branch_broken_compact";
00253:
00254:         public static readonly string[] AllBranches =
00255:         {
00256:             BranchMercyRoad, BranchIronWay, BranchListenerThread, BranchBrokenCompact
00257:         };
00258:     }
00259: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Flags/IFlagLedger.cs`

### `Assets/Ashfall.Core/Flags/IFlagLedger.cs` — complete current file

- Size: 63 lines / 2518 bytes.
- SHA-256: `39a8e52d852cf5501cba6a4568b29d21d4203b86db239249e9de91470cd72ea6`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004:
00005: namespace Ashfall.Core.Flags
00006: {
00007:     public interface IFlagLedger
00008:     {
00009:         bool IsSet(string flagId);
00010:         void Set(string flagId, string originSystem = "", string sourceEvent = "", int day = 0, string subjectId = "");
00011:         void Clear(string flagId);
00012:         int GetCounter(string counterId);
00013:         void Increment(string counterId, int amount = 1, string originSystem = "", string sourceEvent = "", int day = 0, string subjectId = "");
00014:         void SetCounter(string counterId, int value, string originSystem = "", string sourceEvent = "", int day = 0, string subjectId = "");
00015:     }
00016:
00017:     public sealed class InMemoryFlagLedger : IFlagLedger
00018:     {
00019:         private static string Normalize(string id) => id == null ? string.Empty : id.Trim().ToLowerInvariant();
00020:
00021:         private readonly HashSet<string> _flags = new HashSet<string>(StringComparer.Ordinal);
00022:         private readonly Dictionary<string, int> _counters = new Dictionary<string, int>(StringComparer.Ordinal);
00023:
00024:         public bool IsSet(string flagId)
00025:         {
00026:             if (string.IsNullOrEmpty(flagId)) return false;
00027:             return _flags.Contains(Normalize(flagId));
00028:         }
00029:
00030:         public void Set(string flagId, string originSystem = "", string sourceEvent = "", int day = 0, string subjectId = "")
00031:         {
00032:             if (string.IsNullOrEmpty(flagId)) return;
00033:             _flags.Add(Normalize(flagId));
00034:         }
00035:
00036:         public void Clear(string flagId)
00037:         {
00038:             if (string.IsNullOrEmpty(flagId)) return;
00039:             _flags.Remove(Normalize(flagId));
00040:         }
00041:
00042:         public int GetCounter(string counterId)
00043:         {
00044:             if (string.IsNullOrEmpty(counterId)) return 0;
00045:             _counters.TryGetValue(Normalize(counterId), out var val);
00046:             return val;
00047:         }
00048:
00049:         public void Increment(string counterId, int amount = 1, string originSystem = "", string sourceEvent = "", int day = 0, string subjectId = "")
00050:         {
00051:             if (string.IsNullOrEmpty(counterId)) return;
00052:             string n = Normalize(counterId);
00053:             _counters.TryGetValue(n, out var cur);
00054:             _counters[n] = cur + amount;
00055:         }
00056:
00057:         public void SetCounter(string counterId, int value, string originSystem = "", string sourceEvent = "", int day = 0, string subjectId = "")
00058:         {
00059:             if (string.IsNullOrEmpty(counterId)) return;
00060:             _counters[Normalize(counterId)] = value;
00061:         }
00062:     }
00063: }
```


# Appendix — Current Source Detail: `src/Host/MoralChoiceSaveStore.cs`

### `src/Host/MoralChoiceSaveStore.cs` — complete current file

- Size: 45 lines / 1930 bytes.
- SHA-256: `0c4d6da4e24f76023002cf48e24931f967220096c74954e6f8f33eea949baf3b`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // Save Store : MoralChoiceSaveStore
00004: // Core State : Ashfall.Core.MoralChoice.MoralChoiceState
00005: // Host Caller: Main.MoralChoice / MoralChoiceHostSession
00006: // Purpose    : Moral choice branches, ethical dilemmas, community trust, and faction reactions
00007: // ============================================================================
00008: using Ashfall.Core.MoralChoice;
00009: using Ashfall.Core.Save;
00010:
00011: namespace AtomicWar.GodotApp
00012: {
00013:     /// <summary>
00014:     /// Persists MoralChoiceState as JSON under user://moral_choice_save.json —
00015:     /// thin façade over the Core SaveStore&lt;T&gt; service (via SaveStoreHub).
00016:     /// Checksummed envelope, atomic write, and legacy bare-state loading live
00017:     /// in the service; this class keeps the void Save call surface with an
00018:     /// optional path override used by the host.
00019:     /// </summary>
00020:     public static class MoralChoiceSaveStore
00021:     {
00022:         public const string FileName = "moral_choice_save.json";
00023:         public const string SectionName = "moral_choice";
00024:
00025:         private static readonly SaveStore<MoralChoiceState> s_store =
00026:             SaveStoreHub.Checksummed<MoralChoiceState>(FileName, nameof(MoralChoiceSaveStore));
00027:
00028:         public static string SavePath => s_store.SavePath;
00029:
00030:         public static bool Exists => s_store.Exists();
00031:
00032:         public static void Save(MoralChoiceState state, string? pathOverride = null)
00033:         {
00034:             s_store.TrySave(state, pathOverride);
00035:         }
00036:
00037:         public static MoralChoiceState? TryLoad(string? pathOverride = null)
00038:         {
00039:             return s_store.TryLoad(pathOverride);
00040:         }
00041:
00042:         /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
00043:         public static string TryCapturePersisted(MoralChoiceState state) => s_store.CapturePersisted(state);
00044:     }
00045: }
```


# Appendix — Current Source Detail: `src/UI/MoralChoiceModal.cs`

### `src/UI/MoralChoiceModal.cs` — complete current file

- Size: 317 lines / 14077 bytes.
- SHA-256: `ef9e9b6e34bb952eb6ac37133cccd919dfc0ef4135b4831ff0085ff6662da334`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006: using Ashfall.Core.MoralChoice;
00007: using Ashfall.Core.UI;
00008: using DesignTheme = Ashfall.Core.UI.Theme;
00009:
00010: namespace AtomicWar.GodotApp.UI
00011: {
00012:     /// <summary>
00013:     /// ASHFALL — Moral Choice Decision Modal ("The Weight of Survival").
00014:     /// Surfaces authored moral dilemmas, narrative encounters, and irrevocable tactical options
00015:     /// to the player without exposing underlying numeric moral/empathy metrics.
00016:     /// Implements <see cref="IModalPanel"/> for focus management and keyboard handling.
00017:     /// </summary>
00018:     public partial class MoralChoiceModal : Control, IModalPanel
00019:     {
00020:         public event Action<string, int>? OnChoiceSelected;
00021:         public event Action? OnClose;
00022:         public event Action? OnModalClosed;
00023:
00024:         public bool IsModalOpen => Visible;
00025:         public Control? InitialFocusControl => _firstInteractiveButton ?? _closeButton;
00026:
00027:         private Label _titleLabel = null!;
00028:         private Label _subtitleLabel = null!;
00029:         private VBoxContainer _encounterContainer = null!;
00030:         private VBoxContainer _choicesContainer = null!;
00031:         private VBoxContainer _feedbackContainer = null!;
00032:         private Button _closeButton = null!;
00033:         private Control? _firstInteractiveButton;
00034:
00035:         private MoralChoiceQuestDefinition? _currentQuest;
00036:         private MoralChoiceSystem? _moralChoiceSystem;
00037:         private Action<string, int>? _onChoiceCallback;
00038:
00039:         public override void _Ready()
00040:         {
00041:             SetAnchorsPreset(LayoutPreset.FullRect);
00042:             BuildLayout();
00043:             Visible = false;
00044:         }
00045:
00046:         private void BuildLayout()
00047:         {
00048:             AshfallUiHelpers.EmptyChildren(this);
00049:
00050:             // Dark semi-transparent scrim backdrop
00051:             var scrim = new ColorRect
00052:             {
00053:                 Color = new Color(0.02f, 0.02f, 0.04f, 0.88f)
00054:             };
00055:             scrim.SetAnchorsPreset(LayoutPreset.FullRect);
00056:             AddChild(scrim);
00057:
00058:             // Center dialog container (max width 1100, centered)
00059:             var center = new CenterContainer();
00060:             center.SetAnchorsPreset(LayoutPreset.FullRect);
00061:             AddChild(center);
00062:
00063:             var panelCard = AshfallUiHelpers.MakeCardFrame("THE WEIGHT OF SURVIVAL", "ETHICAL DIRECTIVE & TACTICAL CHOICE");
00064:             panelCard.CustomMinimumSize = new Vector2(1040, 680);
00065:             center.AddChild(panelCard);
00066:
00067:             var margin = panelCard.GetChild<MarginContainer>(0);
00068:             var mainVBox = margin.GetChild<VBoxContainer>(0);
00069:
00070:             // Title & category header
00071:             _titleLabel = AshfallUiHelpers.MakeTitle("MORAL DILEMMA // UNRESOLVED");
00072:             _titleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
00073:             mainVBox.AddChild(_titleLabel);
00074:
00075:             _subtitleLabel = AshfallUiHelpers.MakeSmall("CATEGORY: UNKNOWN · LOCATION: GENERAL SECTOR");
00076:             _subtitleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
00077:             mainVBox.AddChild(_subtitleLabel);
00078:
00079:             mainVBox.AddChild(AshfallUiHelpers.MakeSeparator());
00080:
00081:             // Scrollable central content
00082:             var scroll = new ScrollContainer
00083:             {
00084:                 CustomMinimumSize = new Vector2(980, 440),
00085:                 SizeFlagsVertical = SizeFlags.ExpandFill,
00086:                 HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
00087:             };
00088:             mainVBox.AddChild(scroll);
00089:
00090:             var scrollContent = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
00091:             scrollContent.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00092:             scroll.AddChild(scrollContent);
00093:
00094:             _encounterContainer = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
00095:             scrollContent.AddChild(_encounterContainer);
00096:
00097:             _choicesContainer = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
00098:             scrollContent.AddChild(_choicesContainer);
00099:
00100:             _feedbackContainer = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
00101:             scrollContent.AddChild(_feedbackContainer);
00102:
00103:             mainVBox.AddChild(AshfallUiHelpers.MakeSeparator());
00104:
00105:             // Bottom bar with close/return
00106:             var bottomBar = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingMd);
00107:             bottomBar.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00108:
00109:             _closeButton = AshfallUiHelpers.MakeButton("RETURN TO OVERVIEW // [ESC]", () => CloseModal());
00110:             _closeButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00111:             bottomBar.AddChild(_closeButton);
00112:
00113:             mainVBox.AddChild(bottomBar);
00114:         }
00115:
00116:         public void Bind(
00117:             MoralChoiceQuestDefinition quest,
00118:             MoralChoiceSystem? moralChoiceSystem = null,
00119:             Action<string, int>? onChoiceCallback = null)
00120:         {
00121:             _currentQuest = quest ?? throw new ArgumentNullException(nameof(quest));
00122:             _moralChoiceSystem = moralChoiceSystem;
00123:             _onChoiceCallback = onChoiceCallback;
00124:             _firstInteractiveButton = null;
00125:
00126:             RefreshContent();
00127:         }
00128:
00129:         public void RefreshContent()
00130:         {
00131:             if (_currentQuest == null) return;
00132:
00133:             // The choice buttons are rebuilt on every refresh. Clear the
00134:             // cached focus target before freeing the old button tree.
00135:             _firstInteractiveButton = null;
00136:
00137:             bool isResolved = _moralChoiceSystem?.IsResolved(_currentQuest.Id) ?? false;
00138:             MoralChoiceResolution? resolution = null;
00139:             _moralChoiceSystem?.TryGetResolution(_currentQuest.Id, out resolution);
00140:
00141:             // Header titles
00142:             string statusTag = isResolved ? "RESOLVED & RECORDED" : "TACTICAL ACTION REQUIRED";
00143:             _titleLabel.Text = $"ETHICAL PROTOCOL // {_currentQuest.DisplayName.ToUpperInvariant()}";
00144:             _subtitleLabel.Text = $"CATEGORY: {_currentQuest.Category.ToUpperInvariant()} · STATUS: {statusTag} · LOCATION: {(string.IsNullOrEmpty(_currentQuest.LocationId) ? "SECTOR PERIMETER" : _currentQuest.LocationId)}";
00145:
00146:             // 1. Encounter / Narrative briefing
00147:             AshfallUiHelpers.EmptyChildren(_encounterContainer);
00148:             var encounterCard = AshfallUiHelpers.MakeCardFrame("FIELD ENCOUNTER DOSSIER", _currentQuest.Category.ToUpperInvariant());
00149:             var encBox = encounterCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00150:
00151:             if (!string.IsNullOrWhiteSpace(_currentQuest.Trigger))
00152:             {
00153:                 var trigLabel = AshfallUiHelpers.MakeBody($"► SITUATION: {_currentQuest.Trigger}");
00154:                 trigLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
00155:                 encBox.AddChild(trigLabel);
00156:                 encBox.AddChild(AshfallUiHelpers.MakeSeparator());
00157:             }
00158:
00159:             string encounterText = !string.IsNullOrWhiteSpace(_currentQuest.Discovery)
00160:                 ? _currentQuest.Discovery
00161:                 : "A critical dilemma confronts the shelter cohort. Survival calculations require immediate leadership action.";
00162:
00163:             var bodyLbl = AshfallUiHelpers.MakeBody(encounterText);
00164:             encBox.AddChild(bodyLbl);
00165:             _encounterContainer.AddChild(encounterCard);
00166:
00167:             // 2. Choices section
00168:             AshfallUiHelpers.EmptyChildren(_choicesContainer);
00169:             var choicesCard = AshfallUiHelpers.MakeCardFrame("AUTHORITATIVE DECISION GATES", isResolved ? "RESOLUTION RECORDED" : "SELECT ACTION");
00170:             var chBox = choicesCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00171:
00172:             if (!isResolved)
00173:             {
00174:                 var warnNotice = AshfallUiHelpers.MakeSmall("ATTENTION: Ethical choices permanently alter survivor morale, camp chatter, and regional branch viability. Once committed, a choice cannot be rescinded.");
00175:                 warnNotice.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warning));
00176:                 chBox.AddChild(warnNotice);
00177:                 chBox.AddChild(AshfallUiHelpers.MakeSeparator());
00178:             }
00179:
00180:             for (int i = 0; i < _currentQuest.Choices.Count; i++)
00181:             {
00182:                 int choiceIndex = i;
00183:                 var opt = _currentQuest.Choices[i];
00184:                 bool wasChosen = isResolved && resolution != null && resolution.choiceIndex == i;
00185:
00186:                 var optBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
00187:
00188:                 if (isResolved)
00189:                 {
00190:                     if (wasChosen)
00191:                     {
00192:                         var row = AshfallUiHelpers.MakeDataRow($"[COMMITTED RESOLUTION] Option {i + 1}", opt.Label, AshfallUiHelpers.ToColor(DesignTheme.Hot));
00193:                         optBox.AddChild(row);
00194:
00195:                         if (!string.IsNullOrEmpty(opt.OutcomeText))
00196:                         {
00197:                             var outLbl = AshfallUiHelpers.MakeSmall($"Consequence: {opt.OutcomeText}");
00198:                             outLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
00199:                             optBox.AddChild(outLbl);
00200:                         }
00201:
00202:                         if (!string.IsNullOrEmpty(opt.Epitaph))
00203:                         {
00204:                             var epiLbl = AshfallUiHelpers.MakeSmall($"Camp Chronicle: \"{opt.Epitaph}\"");
00205:                             epiLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
00206:                             optBox.AddChild(epiLbl);
00207:                         }
00208:                     }
00209:                     else
00210:                     {
00211:                         var row = AshfallUiHelpers.MakeDataRow($"[UNSELECTED] Option {i + 1}", opt.Label, AshfallUiHelpers.ToColor(DesignTheme.Dim));
00212:                         optBox.AddChild(row);
00213:                     }
00214:                 }
00215:                 else
00216:                 {
00217:                     // Active unresolved option: interactive button without exposing numeric scores
00218:                     var btn = AshfallUiHelpers.MakeButton($"[{i + 1}] COMMIT PATH // {opt.Label.ToUpperInvariant()}", () =>
00219:                     {
00220:                         ExecuteChoice(choiceIndex);
00221:                     });
00222:                     btn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00223:                     optBox.AddChild(btn);
00224:
00225:                     if (_firstInteractiveButton == null)
00226:                         _firstInteractiveButton = btn;
00227:                 }
00228:
00229:                 chBox.AddChild(optBox);
00230:                 if (i < _currentQuest.Choices.Count - 1)
00231:                     chBox.AddChild(AshfallUiHelpers.MakeSeparator());
00232:             }
00233:
00234:             _choicesContainer.AddChild(choicesCard);
00235:
00236:             // 3. Feedback / consequence strip
00237:             AshfallUiHelpers.EmptyChildren(_feedbackContainer);
00238:             if (isResolved && resolution != null)
00239:             {
00240:                 var fbCard = AshfallUiHelpers.MakeCardFrame("RESOLUTION ARCHIVE & CONSEQUENCE RECORD", $"RESOLVED DAY {resolution.resolvedDay}");
00241:                 var fbBox = fbCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00242:
00243:                 string arrow = resolution.impactMark == "up" ? "🔺 Positive Social Trajectory"
00244:                     : resolution.impactMark == "down" ? "🔻 Hardened Survival Stance" : "⚪ Neutral Pragmatic Shift";
00245:
00246:                 fbBox.AddChild(AshfallUiHelpers.MakeDataRow("Moral Resonance", arrow, AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00247:                 fbBox.AddChild(AshfallUiHelpers.MakeDataRow("Camp Record", resolution.epitaph, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
00248:                 fbBox.AddChild(AshfallUiHelpers.MakeDataRow("Journal Status", "Archived to permanent Holdfast survival chronicle.", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
00249:
00250:                 _feedbackContainer.AddChild(fbCard);
00251:             }
00252:         }
00253:
00254:         private void ExecuteChoice(int choiceIndex)
00255:         {
00256:             if (_currentQuest == null) return;
00257:             string questId = _currentQuest.Id;
00258:
00259:             // Prefer Bind callback when present so host paths cannot double-resolve
00260:             // via both OnChoiceSelected and the Bind delegate.
00261:             if (_onChoiceCallback != null)
00262:                 _onChoiceCallback.Invoke(questId, choiceIndex);
00263:             else
00264:                 OnChoiceSelected?.Invoke(questId, choiceIndex);
00265:
00266:             // Re-render in place
00267:             RefreshContent();
00268:         }
00269:
00270:         public void Open()
00271:         {
00272:             Visible = true;
00273:             RefreshContent();
00274:             _firstInteractiveButton?.GrabFocus();
00275:         }
00276:
00277:         public void SelectChoiceForTest(int choiceIndex) => ExecuteChoice(choiceIndex);
00278:
00279:         public void CloseModal()
00280:         {
00281:             Visible = false;
00282:             OnModalClosed?.Invoke();
00283:             OnClose?.Invoke();
00284:         }
00285:
00286:         public override void _UnhandledInput(InputEvent @event)
00287:         {
00288:             if (!Visible) return;
00289:
00290:             if (@event is InputEventKey key && key.Pressed)
00291:             {
00292:                 if (key.Keycode == Key.Escape)
00293:                 {
00294:                     CloseModal();
00295:                     GetViewport().SetInputAsHandled();
00296:                     return;
00297:                 }
00298:
00299:                 // Keyboard quick-selection for options 1-9 if unresolved
00300:                 if (_currentQuest != null && (_moralChoiceSystem == null || !_moralChoiceSystem.IsResolved(_currentQuest.Id)))
00301:                 {
00302:                     int number = -1;
00303:                     if (key.Keycode >= Key.Key1 && key.Keycode <= Key.Key9)
00304:                         number = (int)(key.Keycode - Key.Key1);
00305:                     else if (key.Keycode >= Key.Kp1 && key.Keycode <= Key.Kp9)
00306:                         number = (int)(key.Keycode - Key.Kp1);
00307:
00308:                     if (number >= 0 && number < _currentQuest.Choices.Count)
00309:                     {
00310:                         ExecuteChoice(number);
00311:                         GetViewport().SetInputAsHandled();
00312:                     }
00313:                 }
00314:             }
00315:         }
00316:     }
00317: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **The subject is the boundary between authored ethical definitions and the current active moral-choice state. The plan expands producer/consumer traceability, replay, persistence and truthful player memory without creating a second moral authority.**.

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
