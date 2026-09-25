# Plan 110 — Moral-Choice Gossip Lines, Decay and Rumor Propagation

> **Rebuild status:** COMPLETE 420-LINE GOSSIP CONTENT RUNTIME — REACHABILITY AND TONE MAINTENANCE
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

- The current content is complete at 420 strings across camp chatter, NPC greetings and whispers. `MoralChoiceGossipRuntime` reads the pools by moral band, applies authored decay, and `MoralChoiceGossipSeed` translates resolved choices into the canonical rumor system.
- The safe route is moral-choice owner → effective band/decay → deterministic runtime selection → current presentation/rumor host. Gossip strings are content, not a second moral-choice ledger.
- The meaningful residual is reachability: verify the current host actually projects the chosen line or rumor at the appropriate surface, keeps bands distinct, and never leaks internal IDs or hidden outcome state.

**Bounded outcome:** Retire the old “expand every pool” implementation premise. The current catalog has 21 pools × 20 lines, seeded selection/decay, rumor seeding and focused tests. The rebase protects player-visible reachability, tone separation and exactly-once propagation without coupling lines to parallel quest/faction authorities.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `moral_choice_gossip.json` is present with 21 pools and exactly 20 lines per pool; `MoralChoiceGossipExpansionTests` asserts the full shape and seeded selection.
- `MoralChoiceGossipCatalogLoader`, `MoralChoiceGossipRuntime` and `MoralChoiceGossipSeed` are current Core owners; `RumorNetworkHostSession`, `RumorBoardPanel` and `Main.RumorNetwork` own propagation/presentation.
- Focused tests cover band selection, decay, public/private/suppressed choice propagation, authored delay, determinism and internal-ID leakage.
- The data does not need per-line quest/faction reference fields; the existing seed route is the correct integration seam.

**Master-authority sections applied to this rebase:**

- Master authority Volume 28 verification cookbook: focused evidence before broad gates.
- Lane D save/state/compatibility guidance: owner DTOs, migration and restore proof.
- Lane E UI/UX/accessibility guidance: truthful projections and keyboard/controller lifecycle.
- Lane G testing guidance: smallest affected target, negative cases and deterministic replay.
- Anti-padding protocol: content exhaustion may end the plan before the character checkpoint.
- Volume 32 narrative fact and projection guidance.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 0–10 to 20 content target with a 21-pool/420-line current census.
- Define the route from moral choice to effective band, line selection and rumor seed without duplicating choice state.
- Add a UI reachability and tone audit for camp, greeting and whisper surfaces.
- Preserve exactly-once rumor seeding, decay and save/restore behavior.

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
| gossip content and decay data | MoralChoiceGossipCatalogLoader | `Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipCatalogLoader.cs` | Sole line-pool catalog loader. |
| band selection and decay projection | MoralChoiceGossipRuntime | `Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipRuntime.cs` | Owns no player choice state; reads current moral owner. |
| choice-to-rumor translation | MoralChoiceGossipSeed | `Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipSeed.cs` | Produces a seed for the canonical rumor owner. |
| rumor propagation and board projection | RumorSystem/host | `Assets/Ashfall.Core/InformationFlow/RumorSystem.cs; src/Host/RumorNetworkHostSession.cs; src/UI/RumorBoardPanel.cs` | Owns rumor state and presentation. |
| content, tone, determinism and propagation proof | Gossip focused tests | `Ashfall.Core.Tests/MoralChoiceGossipExpansionTests.cs; Ashfall.Core.Tests/MoralChoice/GossipPropagationTests.cs` | Executable current evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Moral-Choice Gossip Lines, Decay and Rumor Propagation
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ MoralChoiceGossipCatalogLoader
│   gossip content and decay data
│ MoralChoiceGossipRuntime
│   band selection and decay projection
│ MoralChoiceGossipSeed
│   choice-to-rumor translation
│ RumorSystem/host
│   rumor propagation and board projection
│ Gossip focused tests
│   content, tone, determinism and propagation proof
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

1. **Preserve current state ownership.** MoralChoiceGossipCatalogLoader owns gossip content and decay data: Sole line-pool catalog loader.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| gossip content and decay data | MoralChoiceGossipCatalogLoader | `Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipCatalogLoader.cs` | Sole line-pool catalog loader. |
| band selection and decay projection | MoralChoiceGossipRuntime | `Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipRuntime.cs` | Owns no player choice state; reads current moral owner. |
| choice-to-rumor translation | MoralChoiceGossipSeed | `Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipSeed.cs` | Produces a seed for the canonical rumor owner. |
| rumor propagation and board projection | RumorSystem/host | `Assets/Ashfall.Core/InformationFlow/RumorSystem.cs; src/Host/RumorNetworkHostSession.cs; src/UI/RumorBoardPanel.cs` | Owns rumor state and presentation. |
| content, tone, determinism and propagation proof | Gossip focused tests | `Ashfall.Core.Tests/MoralChoiceGossipExpansionTests.cs; Ashfall.Core.Tests/MoralChoice/GossipPropagationTests.cs` | Executable current evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load gossip pools/decay
2. read current moral-choice band and propagation day
3. derive effective band after decay
4. select a stable seeded line for the current surface
5. present camp/greeting/whisper projection
6. translate eligible choice into canonical rumor seed
7. propagate/decay/intercept through RumorSystem
8. capture/restore current choice and rumor owners

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- The line catalog is immutable; effective moral band and last-propagation day belong to the moral-choice owner/runtime input.
- Rumor state belongs to `RumorSystem`; a moral choice may seed it once under the current propagation contract.
- Decay is deterministic and bounded; suppressed/private choices do not become public rumors.
- Restore preserves rumor state and choice-derived seed dedupe without replaying old public events.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Every supported moral band maps to a non-empty pool for each surface.
- The same seed, day and state produce the same selected line.
- Internal quest/faction IDs never appear in player-facing strings.
- A line is not treated as a hidden mechanical effect unless a current owner consumes it.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `moral_choice_gossip.json` is the sole line pool authority.
- Do not add per-line mutable state or duplicate the catalog in narrative files.
- New lines require band-specific tone, concrete fictional context and no internal identifiers.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the existing moral-choice and rumor save paths.
- No new gossip save section is justified.
- Restore must not regenerate a rumor or re-fire a line event for an already-processed choice.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Selection and rumor seed generation use the injected seeded RNG.
- Catalog order is stable and line IDs are not generated from hash order.
- Paired runs produce identical line, seed and propagation traces.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Moral choice resolution is the upstream fact.
- Gossip runtime selection is a projection, not a new event authority.
- Rumor generation/propagation/interception events come from the canonical rumor owner.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/RumorNetworkHostSession.cs
- src/Main.RumorNetwork.cs
- src/UI/RumorBoardPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Lines should be specific, restrained and fictional, not real-world or copied text.
- Band tone must be distinguishable without caricaturing survivors.
- Rumor propagation should communicate social consequence without exposing hidden outcome data.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A pool is unreachable for a valid band. | MoralChoiceGossipCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | The same line repeats due to unseeded/random iteration behavior. | MoralChoiceGossipRuntime | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A private choice seeds a public rumor. | MoralChoiceGossipSeed | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A stale line pool is cached after a save restore. | RumorSystem/host | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | Internal IDs leak into a player-facing line. | Gossip focused tests | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceGossipExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoice/GossipPropagationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoice/Plan110_111GossipPhantomIntegrationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — pool census | Read catalog, runtime, seed, rumor owner and tests. | 21 pools × 20 lines is proven. | No production path until the owning implementation package is separately claimed. |
| 1 — reachability matrix | Trace each surface from moral owner to presentation. | No valid band/surface is orphaned. | No production path until the owning implementation package is separately claimed. |
| 2 — tone/replay review | Review band separation, IDs and seeded selection. | No leakage or repetition defect remains. | No production path until the owning implementation package is separately claimed. |
| 3 — propagation/save proof | Verify one-time rumor seed, decay and restore. | Current rumor owner remains sole authority. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/moral_choice_gossip.json | READ ONLY; MODIFY only for proven tone/reachability gap | 420-line authority |
| Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipRuntime.cs | READ ONLY | Selection/decay |
| Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipSeed.cs | READ ONLY | Seed translation |
| src/UI/RumorBoardPanel.cs | READ ONLY | Current presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Creating a second rumor or moral state store. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding hidden mechanics to decorative strings. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Using wall-clock/random selection. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Overwriting current catalog files with a new parallel pool. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new line count for this rebase.
- No new rumor network.
- No save-section change.
- No production edits in this rebase.

# 23. Rollback and Recovery

- Revert the plan file.
- Future line/host changes retain prior catalog, rumor save fixture and focused tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 420 current lines and 21 pools are documented.
- Band/decay/rumor boundaries are explicit.
- UI and save contracts are named.
- No parallel content or state authority is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 0–10 to 20 content target with a 21-pool/420-line current census.
- Define the route from moral choice to effective band, line selection and rumor seed without duplicating choice state.
- Add a UI reachability and tone audit for camp, greeting and whisper surfaces.
- Preserve exactly-once rumor seeding, decay and save/restore behavior.

## MUST NOT DO

- No new line count for this rebase.
- No new rumor network.
- No save-section change.
- No production edits in this rebase.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceGossipExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoice/GossipPropagationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoice/Plan110_111GossipPhantomIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — pool census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: gossip content and decay data → MoralChoiceGossipCatalogLoader; band selection and decay projection → MoralChoiceGossipRuntime; choice-to-rumor translation → MoralChoiceGossipSeed; rumor propagation and board projection → RumorSystem/host; content, tone, determinism and propagation proof → Gossip focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 110.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 110 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by MoralChoiceGossipCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipCatalogLoader.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 144 lines / 6358 bytes.
- SHA-256: `00c6f3d80e2dab5b4c2b299b084798886fb7366c82c1572a52ba03260bdccbbe`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MoralChoiceGossipContainer
public int schema_version = 1;
public string description = string.Empty;
public MoralCampChatterRecord camp_chatter = new MoralCampChatterRecord();
public MoralNpcGreetingShiftsRecord npc_greeting_shifts = new MoralNpcGreetingShiftsRecord();
public MoralWhisperLinesRecord whisper_lines = new MoralWhisperLinesRecord();
public MoralGossipDecayRecord gossip_decay = new MoralGossipDecayRecord();
public sealed class MoralCampChatterRecord
public string description = string.Empty;
public List<string> very_positive = new List<string>();
public List<string> positive = new List<string>();
public List<string> slightly_positive = new List<string>();
public List<string> neutral = new List<string>();
public List<string> slightly_evil = new List<string>();
public List<string> evil = new List<string>();
public List<string> very_evil = new List<string>();
public sealed class MoralNpcGreetingShiftsRecord
public string description = string.Empty;
public List<string> very_positive = new List<string>();
public List<string> positive = new List<string>();
public List<string> slightly_positive = new List<string>();
public List<string> neutral = new List<string>();
public List<string> slightly_evil = new List<string>();
public List<string> evil = new List<string>();
public List<string> very_evil = new List<string>();
public sealed class MoralWhisperLinesRecord
public string description = string.Empty;
public List<string> very_positive = new List<string>();
public List<string> positive = new List<string>();
public List<string> slightly_positive = new List<string>();
public List<string> neutral = new List<string>();
public List<string> slightly_evil = new List<string>();
public List<string> evil = new List<string>();
public List<string> very_evil = new List<string>();
public sealed class MoralGossipDecayRecord
public string description = string.Empty;
public int decay_interval_days = 30;
public int full_decay_days = 60;
public int dramatic_reset_threshold = 10;
public static class MoralChoiceGossipCatalogLoader
public const string DefaultFileName = "moral_choice_gossip.json";
public static MoralChoiceGossipData Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipRuntime.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipRuntime.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 184 lines / 8452 bytes.
- SHA-256: `743377f248a886fbb40e7b54ad724a4c485181723542283a7b6f3bf173e18342`.
- Architecture signals: seeded references=2; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MoralChoiceGossipRuntime
public MoralChoiceGossipData Data => _data;
public List<string> GetCampChatter(MoralPathBand band) {
public string PickCampChatter(MoralPathBand band) {
public List<string> GetNpcGreetings(MoralPathBand band) {
public string PickNpcGreeting(MoralPathBand band) {
public List<string> GetWhisperLines(MoralPathBand band) {
public string PickWhisper(MoralPathBand band) {
public MoralPathBand GetEffectiveGossipBand(MoralChoiceSystem system, int currentDay) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipSeed.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipSeed.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 108 lines / 4797 bytes.
- SHA-256: `475b38799cfac138d98e9d815eee92158bc39c57f92dc6e2a9feb825be400eb6`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MoralGossipSeed
public string SubjectId { get; }
public string OriginLocationId { get; }
public int OriginDay { get; }
public string Headline { get; }
public string Description { get; }
public float Truthfulness { get; }
public float DecayRate { get; }
public int PropagationSpeed { get; }
public static class MoralChoiceGossipSeed
public const float PublicImpactThreshold = 0.5f;
public const float MinimumImpactToTravel = 0.25f;
public static MoralGossipSeed? Build( string questId, string originLocationId, int resolvedDay, string epitaph, float publicImpact01,
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/InformationFlow/RumorSystem.cs`

### `Assets/Ashfall.Core/InformationFlow/RumorSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 410 lines / 15528 bytes.
- SHA-256: `fa134f0a4a6174821bbf55f975697d87efec5660e0ccca6eba3bb78328ed553f`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum RumorSubjectType
public sealed class WastelandRumor
public string RumorId { get; set; } = string.Empty;
public string OriginLocationId { get; set; } = string.Empty;
public int OriginDay { get; set; } = 1;
public RumorSubjectType SubjectType { get; set; } = RumorSubjectType.Faction;
public string SubjectId { get; set; } = string.Empty;
public string Headline { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public float Truthfulness { get; set; } = 0.85f; // 0.0 (false) to 1.0 (verified)
public float DecayRate { get; set; } = 0.05f;
public int PropagationSpeed { get; set; } = 1; // hubs per day
public bool IsIntercepted { get; set; } = false;
public List<string> ReachedHubIds { get; set; } = new List<string>();
public sealed class InformationHub
public string HubId { get; set; } = string.Empty;
public string HubName { get; set; } = string.Empty;
public string LocationId { get; set; } = string.Empty;
public float Credibility { get; set; } = 0.8f;
public string Bias { get; set; } = "neutral";
public sealed class InformationHubDef
public string hub_id { get; set; } = string.Empty;
public string hub_name { get; set; } = string.Empty;
public string location_id { get; set; } = string.Empty;
public float credibility { get; set; } = 0.8f;
public string bias { get; set; } = "neutral";
public int daily_rumor_capacity { get; set; } = 5;
public sealed class RumorCatalogData
public int schema_version { get; set; } = 1;
public List<InformationHubDef> hubs { get; set; } = new List<InformationHubDef>();
public sealed class BriefingItem
public string RumorId { get; set; } = string.Empty;
public string Headline { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public RumorSubjectType SubjectType { get; set; }
public float Truthfulness { get; set; }
public bool IsVerified { get; set; }
public bool IsThreat { get; set; }
public sealed class IntelligenceBriefingReport
public string LocationId { get; set; } = string.Empty;
public string HubName { get; set; } = string.Empty;
public float HubCredibility { get; set; }
public int TotalItems { get; set; }
public int ThreatCount { get; set; }
public int OpportunityCount { get; set; }
public float AverageTruthfulness { get; set; }
public List<BriefingItem> Items { get; set; } = new List<BriefingItem>();
public sealed class RumorNetworkState
public int SchemaVersion { get; set; } = 1;
public int NextSequence { get; set; } = 1;
public List<WastelandRumor> Rumors { get; set; } = new List<WastelandRumor>();
public List<InformationHub> Hubs { get; set; } = new List<InformationHub>();
public sealed class RumorSystem
public event Action<WastelandRumor>? OnRumorGenerated;
public event Action<WastelandRumor, string>? OnRumorPropagated;
public event Action<WastelandRumor>? OnRumorIntercepted;
public event Action? OnStateChanged;
public int TotalRumorCount => _state.Rumors.Count;
public int InterceptedRumorCount => _state.Rumors.Count(r => r.IsIntercepted);
public int HubCount => _state.Hubs.Count;
public IReadOnlyList<WastelandRumor> Rumors => _state.Rumors;
public IReadOnlyList<InformationHub> Hubs => _state.Hubs;
public RumorNetworkState State => _state;
public InformationHub RegisterHub( string hubId, string name, string locationId, float credibility = 0.8f, string bias = "neutral")
public WastelandRumor GenerateRumor( string originLocationId, RumorSubjectType subjectType, string subjectId, string headline, string description,
public bool PropagateRumorToHub(string rumorId, string hubId) {
public bool InterceptRumor(string rumorId) {
public void TickDay(int currentDay) {
public IReadOnlyList<WastelandRumor> GetRumorsAtLocation(string locationId) {
public IReadOnlyList<WastelandRumor> GetInterceptedRumors() {
public void LoadCatalog(RumorCatalogData? catalog) {
public void LoadCatalog(string json) {
public IntelligenceBriefingReport CreateBriefingReport(string locationId) {
public RumorNetworkState CaptureState() {
public void RestoreState(RumorNetworkState state) {
```


# Appendix B.06 — Current Code Architecture: `src/Host/RumorNetworkHostSession.cs`

### `src/Host/RumorNetworkHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 110 lines / 4108 bytes.
- SHA-256: `1ec816ab6466fa45f095de4054dccf3739b85989fc8302a781ddd9e9ba723f3f`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=1; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RumorNetworkHostSession
public RumorSystem System => _system;
public event Action? StateChanged;
public int TotalRumorCount => _system.TotalRumorCount;
public int InterceptedRumorCount => _system.InterceptedRumorCount;
public int HubCount => _system.HubCount;
public IReadOnlyList<WastelandRumor> Rumors => _system.Rumors;
public IReadOnlyList<InformationHub> Hubs => _system.Hubs;
public InformationHub RegisterHub(string hubId, string name, string locationId, float credibility = 0.8f, string bias = "neutral") {
public WastelandRumor GenerateRumor( string originLocationId, RumorSubjectType subjectType, string subjectId, string headline, string description,
public bool PropagateRumorToHub(string rumorId, string hubId) {
public bool InterceptRumor(string rumorId) {
public void TickDay(int currentDay) {
public IReadOnlyList<WastelandRumor> GetRumorsAtLocation(string locationId) {
public IReadOnlyList<WastelandRumor> GetInterceptedRumors() {
public RumorNetworkState CaptureState() {
public void RestoreState(RumorNetworkState state) {
```


# Appendix B.07 — Current Code Architecture: `src/Main.RumorNetwork.cs`

### `src/Main.RumorNetwork.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 85 lines / 2802 bytes.
- SHA-256: `4fd5ee6bee378af6c3fcf21321445ac35c660c325cff145f2b0583f1f68fbb96`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public RumorNetworkHostSession RumorNetwork => EnsureRumorNetwork();
public RumorNetworkHostSession EnsureRumorNetwork() {
public void TickRumorNetwork(int day) {
public void ShowRumorNetworkPanel() {
```


# Appendix B.08 — Current Code Architecture: `src/UI/RumorBoardPanel.cs`

### `src/UI/RumorBoardPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 240 lines / 9668 bytes.
- SHA-256: `5a5860a36d39f3a7861282077ebcefca93de459eea1bb2a069942b5771ab3103`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class RumorBoardPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _host != null;
public void Bind(RumorNetworkHostSession session) {
public void Unbind() {
public override void _Ready() {
public void RefreshView() {
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/moral_choice_gossip.json`

### `Assets/StreamingAssets/Data/moral_choice_gossip.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 27959 bytes / 27945 characters.
- SHA-256: `fabce75419bbfe57e5637338851bd0418a1e3056f49cfe39b61858df4361c0f8`.
- Root keys: `camp_chatter`, `description`, `gossip_decay`, `npc_greeting_shifts`, `schema_version`, `whisper_lines`.

Array-path census (minimum, maximum, observed rows):

```text
camp_chatter.evil: min=20, max=20, observed_paths=1
camp_chatter.neutral: min=20, max=20, observed_paths=1
camp_chatter.positive: min=20, max=20, observed_paths=1
camp_chatter.slightly_evil: min=20, max=20, observed_paths=1
camp_chatter.slightly_positive: min=20, max=20, observed_paths=1
camp_chatter.very_evil: min=20, max=20, observed_paths=1
camp_chatter.very_positive: min=20, max=20, observed_paths=1
npc_greeting_shifts.evil: min=20, max=20, observed_paths=1
npc_greeting_shifts.neutral: min=20, max=20, observed_paths=1
npc_greeting_shifts.positive: min=20, max=20, observed_paths=1
npc_greeting_shifts.slightly_evil: min=20, max=20, observed_paths=1
npc_greeting_shifts.slightly_positive: min=20, max=20, observed_paths=1
npc_greeting_shifts.very_evil: min=20, max=20, observed_paths=1
npc_greeting_shifts.very_positive: min=20, max=20, observed_paths=1
whisper_lines.evil: min=20, max=20, observed_paths=1
whisper_lines.neutral: min=20, max=20, observed_paths=1
whisper_lines.positive: min=20, max=20, observed_paths=1
whisper_lines.slightly_evil: min=20, max=20, observed_paths=1
whisper_lines.slightly_positive: min=20, max=20, observed_paths=1
whisper_lines.very_evil: min=20, max=20, observed_paths=1
whisper_lines.very_positive: min=20, max=20, observed_paths=1
```


# Appendix D.10 — Existing Focused Test Inventory: `Ashfall.Core.Tests/MoralChoiceGossipExpansionTests.cs`

### `Ashfall.Core.Tests/MoralChoiceGossipExpansionTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 131; SHA-256: `00a0310bba9766e2fd96020dc0995945ad4c7f7463a64f00449cd27dbfbba791`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_HasExactlyTwentyOnePoolsAndFourHundredTwentyLines
SlightlyPositiveWhispers_AreLoadedAndRuntimeReachable
GossipSelection_RemainsSeedDeterministicAcrossAllSectionsAndBands
GossipLines_DoNotContainInternalContextIdentifiers
```


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs`

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


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/MoralChoice/GossipPropagationTests.cs`

### `Ashfall.Core.Tests/MoralChoice/GossipPropagationTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 108; SHA-256: `dd62bb991fc425bf3107351729342de8d93354bdb599f54d0d04273862798614`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
PublicChoice_SeedsARumor
PrivateChoice_NeverTravels
QuietChoice_StaysHome
Suppression_StopsTheSeed
DecisiveChoice_TravelsMoreAccuratelyThanAMarginalOne
UntruthFadesFaster
AuthoredPropagationDay_IsHonored
Seed_IsDeterministic
InvalidInput_FailsClosed
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/MoralChoice/Plan110_111GossipPhantomIntegrationTests.cs`

### `Ashfall.Core.Tests/MoralChoice/Plan110_111GossipPhantomIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 249; SHA-256: `31d22ce9210615526ecddc6373fab01e83160a392be2f256391bf6b30f61692f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan110_MoralChoiceGossip_LoadsAllTwentyOneArrays_AtExactlyTwentyLinesEach
Plan111_PhantomMemoryTriggers_LoadsAllTwentyBackgrounds_WithValidTriggerRules
Plan111_PhantomMemoryEngine_ResolvesOutcomesAndLifecycle_Deterministically
Plan110_111_CrossSystem_GossipToneAndPhantomMemories_CoherenceContract
```


# Appendix E.14 — Supporting Code Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Journal/JournalSystem.cs`

### `Assets/Ashfall.Core/Journal/JournalSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 490 lines / 21046 bytes.
- SHA-256: `c0d8316adaed06ce3b5415dcfd5fe79f63ad31fa6675181ad85859191601d220`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=12; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class JournalSystem
public const int MaxEntries = 64;
public const int TabCount = 5;
public event Action<JournalEntry> OnEntryAdded;
public event Action<JournalEntry> OnNotificationPing;
public event Action<int> OnTabChanged;
public event Action<string> OnCodexUnlocked;
public int ActiveTab { get; private set; }
public int CodexUnlockCount { get; private set; }
public int GetLastSeenIndex(int tab) {
public int GetLastSeenCodexIndex(int tab) {
public bool HasUnreadForTab(int tab) {
public void SwitchTab(int tab) {
public void MarkTabViewed(int tab) {
public bool UnlockItemSeen(string itemId) => UnlockCodex(KnowledgeKeys.ItemSeen(itemId));
public bool UnlockLocationVisited(string locationId) => UnlockCodex(KnowledgeKeys.LocationVisited(locationId));
public bool UnlockSurvivorMet(string survivorId) => UnlockCodex(KnowledgeKeys.SurvivorMet(survivorId));
public bool UnlockEventFired(string eventId) => UnlockCodex(KnowledgeKeys.EventFired(eventId));
public bool UnlockRoomHistorySeen(string vignetteId) => UnlockCodex(KnowledgeKeys.RoomHistorySeen(vignetteId));
public bool UnlockGlitchNoted(string glitchId) => UnlockCodex(KnowledgeKeys.GlitchNoted(glitchId));
public bool UnlockWildlifeCaught(string speciesId) => UnlockCodex(KnowledgeKeys.WildlifeSpeciesCaught(speciesId));
public bool UnlockNarrativeDiscovered(string discoveryId) => UnlockCodex(KnowledgeKeys.NarrativeDiscovered(discoveryId));
public bool UnlockBureaucraticDocument(string docId) => UnlockCodex(KnowledgeKeys.BureaucraticDocument(docId));
public bool AddKnowledgeEvidence(string survivorId, string knowledgeKey) => UnlockCodex(knowledgeKey);
public bool IsItemSeen(string itemId) => _knowledge.Has(KnowledgeKeys.ItemSeen(itemId));
public bool IsLocationVisited(string locationId) => _knowledge.Has(KnowledgeKeys.LocationVisited(locationId));
public bool IsSurvivorMet(string survivorId) => _knowledge.Has(KnowledgeKeys.SurvivorMet(survivorId));
public bool IsEventFired(string eventId) => _knowledge.Has(KnowledgeKeys.EventFired(eventId));
public bool IsRoomHistorySeen(string vignetteId) => _knowledge.Has(KnowledgeKeys.RoomHistorySeen(vignetteId));
public bool IsGlitchNoted(string glitchId) => _knowledge.Has(KnowledgeKeys.GlitchNoted(glitchId));
public bool IsWildlifeCaught(string speciesId) => _knowledge.Has(KnowledgeKeys.WildlifeSpeciesCaught(speciesId));
public bool IsNarrativeDiscovered(string discoveryId) => _knowledge.Has(KnowledgeKeys.NarrativeDiscovered(discoveryId));
public bool IsBureaucraticDocumentDiscovered(string docId) => _knowledge.Has(KnowledgeKeys.BureaucraticDocument(docId));
public void SetEntryFactory(Func<JournalEntry> factory, Action<JournalEntry> recycler) {
public KnowledgeBase Knowledge => _knowledge;
public void BindAuthoredCorpus(JournalCorpusAdapter? adapter) {
public bool HasAuthoredCorpus => _authoredCorpus != null;
public JournalEntry? TryAddAuthoredEntry( string knowledgeKey, ISurvivorAuthor? fallbackAuthor = null) {
public IReadOnlyList<JournalEntry> Entries => _entries;
public int EntryCount => _entries.Count;
public string LatestText =>
public bool HasUnread { get; set; }
public bool NotificationPing { get; private set; }
public int NotificationPingCount { get; private set; }
public bool HudIsOpen { get; set; }
public JournalEntry? TryDiscover( string knowledgeKey, ISurvivorAuthor author, int day, float hour = -1f) {
public JournalEntry? TryDiscoverKnowledge( string knowledgeKey, ISurvivorAuthor? author, int day, float hour = -1f) {
public JournalEntry? TryDiscoverRawKnowledge( string knowledgeKey, string text, ISurvivorAuthor? author, int day, float hour = -1f)
public JournalEntry? TryAddRawEntry( string knowledgeKey, string text, ISurvivorAuthor author, int day, float hour = -1f)
public void AcknowledgePing() {
public void MarkRead() {
public void Clear() {
public JournalSave CaptureState() {
public void RestoreState(JournalSave save) {
public class JournalSave
public JournalEntry[] Entries;
public KnowledgeBaseSave Knowledge;
public int NextSeq;
public bool HasUnread;
public bool NotificationPing;
public int NotificationPingCount;
public bool HudIsOpen;
public int ActiveTab;
public int[] LastSeenIndexPerTab;
public int[] LastSeenCodexPerTab;
public int CodexUnlockCount;
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`

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


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/Codex/CodexProjectionBuilder.cs`

### `Assets/Ashfall.Core/Codex/CodexProjectionBuilder.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 366 lines / 18515 bytes.
- SHA-256: `e13379e433814fe417ac6090a719688a7f5bc6a6ac2d2c3ea10421fcd36875f6`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class CodexProjectionBuilder
public static IReadOnlyList<CodexEntryProjection> Build( FieldGuideCatalog? fieldGuide, ResearchState? researchState, IReadOnlyDictionary<string, ResearchKnowledgeDef>? researchCatalog, JournalSystem? journalSystem, int currentDay = 1,
```


# Appendix E.18 — Supporting Code Evidence: `src/Journal/JournalBookUI.cs`

### `src/Journal/JournalBookUI.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 627 lines / 26158 bytes.
- SHA-256: `6529212c56a3b9c7b7f2d9204fd07c0746e29d5dd85d2aaf1065e3eadc6ce497`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=8; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class JournalBookUI : Control
public const int MaxVisibleCollapsed = 4;
public const int MaxVisibleOpen = JournalSystem.MaxEntries;
public bool IsOpen { get; private set; }
public bool HasUnread { get; private set; }
public bool NotificationPing { get; private set; }
public int NotificationPingCount { get; private set; }
public int EntryCount { get; private set; }
public string LatestText { get; private set; } = string.Empty;
public string LatestAuthor { get; private set; } = string.Empty;
public string LatestTimestamp { get; private set; } = string.Empty;
public string StatusLine { get; private set; } = "JOURNAL: —";
public string DetailSummary { get; private set; } = "No entries yet.";
public int ActiveTab { get; private set; }
public IReadOnlyList<JournalEntry> Entries => _entries;
public event Action? OnOpened;
public event Action? OnClosed;
public event Action<JournalEntry>? OnEntryPushed;
public event Action<int>? OnTabChanged;
public void Bind( JournalSystem journal, Func<JournalTab, IReadOnlyList<JournalCodexRow>> codexProvider, Func<int, bool>? unreadProvider = null, Func<int>? dayProvider = null) {
public override void _Ready() {
public override void _ExitTree() {
public void SwitchTab(int tab) {
public void Push(JournalEntry entry) {
public void SetEntries(IReadOnlyList<JournalEntry> entries) {
public void ApplyUiState(bool isOpen, bool hasUnread, bool notificationPing = false, int activeTab = 0) {
public void Clear() {
public void Open() {
public void Close() {
public void Toggle() {
public void MarkRead() {
public void AcknowledgePing() {
public string ActiveTabContent => _content != null ? _content.Text : string.Empty;
public void Refresh() {
```


# Appendix G.19 — Supporting Regression Evidence: `Ashfall.Core.Tests/MoralChoiceSystemTests.cs`

### `Ashfall.Core.Tests/MoralChoiceSystemTests.cs`

- Current test declarations: Fact=22, Theory=0, InlineData=0.
- File lines: 428; SHA-256: `43c39778ac33913ad3ea4cb255b57fb54511909b6560e169bb599e20dc6ed4ec`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
InitialStateNeutralAndEmpty
BandEdgesPinned
ResolveAppliesDeltasAndRaisesEvent
ResolveIsIdempotentPerQuest
ScoreClampsAndLegendSettlesAtReconcileOncePerDirection
ResolveRejectsNonCanonicalQuestId
ResolveRejectsOutOfRangeChoice
ImpactMarksFollowDeltaSign
SameSeedSameRolls
ReconcileFiresExtremeBandEventsOnce
ReconcileFiresAllCrossedBandsOnBigJump
ReconcileFiresContractAtPositiveBand
ReconcileIgnoresOutOfOrderDays
EndingStorykeeperOverridesBand
EndingSelectionRules
StorykeeperNeedsBothThresholds
ListenerAndConfidantThresholdsPinnedAtBoundary
RestoreRejectsMismatchedSystemAndBadSchema
PendingLegendFlagsSurviveRoundTrip
SaveRoundTripPreservesLedger
CapturedStateIsDetached
AvailabilityWindow
```


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/JournalSystemTests.cs`

### `Ashfall.Core.Tests/JournalSystemTests.cs`

- Current test declarations: Fact=17, Theory=0, InlineData=0.
- File lines: 376; SHA-256: `1b17cc5ad807b917c4745d5552d7755ed96a30b61aca4290ad49ff55b83ae8ce`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
TryDiscover_DeduplicatesPerKnowledgeKey
TryDiscover_RejectsEmptyKey
TryAddRawEntry_RecordsFreeformText_OncePerKey
MaxEntries_EvictsOldest
CodexUnlock_RecordsAndFlags
MarkReadAndAcknowledgePing_ClearFlags
CaptureRestore_RoundTrips_EntriesKnowledgeAndFlags
Clear_ResetsEverything
RestoreState_HandlesNull
EntryLifecycle_NewestFirstOrdering_AndSequenceId
TryDiscoverKnowledge_DualContract_EntryAndCodexUnlock
AddKnowledgeEvidence_Vs_TryDiscoverKnowledge_Interaction
TryAddRawEntry_EdgeCases_NullAuthor_ClampedDay_FormattedHour
MaxEntries_WithRecyclerAndFactory_RecyclesEvictedAndClears
Tabs_Switching_Clamping_AndLastSeenTracking
RestoreState_SuppressesAllEvents
DeterministicOrdering_WithoutWallClock
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs`

### `Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 248; SHA-256: `71802e0882c69e830cd7c2bd3d0530f55223a56ba01afa2dfb6c8b6c9d51be5f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsExactSixCanonicalReactions
Catalog_ContainsAllSixCanonicalEventIds
Catalog_EveryReactionHasNonEmptyEventDescription
Catalog_EveryReactionHasAllThreeFactionDialogues
Catalog_AllDialogueBlocksHaveValidSpeakerLocationAndLines
Catalog_EveryReactionHasNonEmptyJournalEntry
Catalog_PreservesBountyIssuedBaselineParity
Runtime_BandCrossings_TriggerAllCanonicalReactionsOnce
Runtime_OverflowLegends_TriggerOnceOvernight
Runtime_StateCaptureAndRestore_PreservesFiredEvents
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/InformationFlow/RumorSystemTests.cs`

### `Ashfall.Core.Tests/InformationFlow/RumorSystemTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 116; SHA-256: `ecce7473aacb503f68f948812ceab598ea0faa1f4df687bfe9fbb8660ba2819e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RegisterHub_StoresHubMetadata
GenerateRumor_CreatesRumorAndEmitsEvent
PropagateRumorToHub_TransfersRumorAndMutatesTruthfulness
InterceptRumor_MarksIntercepted
TickDay_DecaysTruthfulnessAndRemovesExpiredRumors
CaptureState_And_RestoreState_RoundTripsAccurately
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
| gossip content and decay data | MoralChoiceGossipCatalogLoader | band selection and decay projection | MoralChoiceGossipRuntime | Owner emits/reads a typed fact; no mirror state. |
| gossip content and decay data | MoralChoiceGossipCatalogLoader | choice-to-rumor translation | MoralChoiceGossipSeed | Owner emits/reads a typed fact; no mirror state. |
| gossip content and decay data | MoralChoiceGossipCatalogLoader | rumor propagation and board projection | RumorSystem/host | Owner emits/reads a typed fact; no mirror state. |
| gossip content and decay data | MoralChoiceGossipCatalogLoader | content, tone, determinism and propagation proof | Gossip focused tests | Owner emits/reads a typed fact; no mirror state. |
| band selection and decay projection | MoralChoiceGossipRuntime | gossip content and decay data | MoralChoiceGossipCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| band selection and decay projection | MoralChoiceGossipRuntime | choice-to-rumor translation | MoralChoiceGossipSeed | Owner emits/reads a typed fact; no mirror state. |
| band selection and decay projection | MoralChoiceGossipRuntime | rumor propagation and board projection | RumorSystem/host | Owner emits/reads a typed fact; no mirror state. |
| band selection and decay projection | MoralChoiceGossipRuntime | content, tone, determinism and propagation proof | Gossip focused tests | Owner emits/reads a typed fact; no mirror state. |
| choice-to-rumor translation | MoralChoiceGossipSeed | gossip content and decay data | MoralChoiceGossipCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| choice-to-rumor translation | MoralChoiceGossipSeed | band selection and decay projection | MoralChoiceGossipRuntime | Owner emits/reads a typed fact; no mirror state. |
| choice-to-rumor translation | MoralChoiceGossipSeed | rumor propagation and board projection | RumorSystem/host | Owner emits/reads a typed fact; no mirror state. |
| choice-to-rumor translation | MoralChoiceGossipSeed | content, tone, determinism and propagation proof | Gossip focused tests | Owner emits/reads a typed fact; no mirror state. |
| rumor propagation and board projection | RumorSystem/host | gossip content and decay data | MoralChoiceGossipCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| rumor propagation and board projection | RumorSystem/host | band selection and decay projection | MoralChoiceGossipRuntime | Owner emits/reads a typed fact; no mirror state. |
| rumor propagation and board projection | RumorSystem/host | choice-to-rumor translation | MoralChoiceGossipSeed | Owner emits/reads a typed fact; no mirror state. |
| rumor propagation and board projection | RumorSystem/host | content, tone, determinism and propagation proof | Gossip focused tests | Owner emits/reads a typed fact; no mirror state. |
| content, tone, determinism and propagation proof | Gossip focused tests | gossip content and decay data | MoralChoiceGossipCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| content, tone, determinism and propagation proof | Gossip focused tests | band selection and decay projection | MoralChoiceGossipRuntime | Owner emits/reads a typed fact; no mirror state. |
| content, tone, determinism and propagation proof | Gossip focused tests | choice-to-rumor translation | MoralChoiceGossipSeed | Owner emits/reads a typed fact; no mirror state. |
| content, tone, determinism and propagation proof | Gossip focused tests | rumor propagation and board projection | RumorSystem/host | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 0–10 to 20 content target with a 21-pool/420-line current census. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Define the route from moral choice to effective band, line selection and rumor seed without duplicating choice state. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Add a UI reachability and tone audit for camp, greeting and whisper surfaces. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Preserve exactly-once rumor seeding, decay and save/restore behavior. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.555 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Factions/MilitaryBranchSystem.cs`

### `Assets/Ashfall.Core/Factions/MilitaryBranchSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 270 lines / 13334 bytes.
- SHA-256: `cd72f6d11de1b8962c5c1dfbcb68d457bc5afffbcb7f74934bad68f1b2acc703`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=9; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MilitaryBranchSystem
public const string SystemId = "military_branch_system";
public const int MinAlignment = -200;
public const int MaxAlignment = 200;
public event Action<string>? OnBranchCommitted;
public event Action<string, int>? OnPonrLocked;
public event Action<string>? OnEndingResolved;
public event Action<int>? OnAlignmentChanged;
public MilitaryBranchSystemState State => _state;
public int CurrentDay => _state.timeline.currentDay;
public string? CommittedBranchId => string.IsNullOrEmpty(_state.branch.branchId) ? null : _state.branch.branchId;
public bool IsPonrLocked => _state.branch.ponrLocked;
public int MilitaryAlignment => _state.militaryAlignment.alignment;
public string? ResolvedEndingId => string.IsNullOrEmpty(_state.branch.resolvedEndingId) ? null : _state.branch.resolvedEndingId;
public void AdvanceDay(int day) {
public string CommitBranch(string branchId, MoralChoiceSystem moralChoice) {
public void LockPointOfNoReturn() {
public void ShiftFactionAlignment(int delta) {
public string ResolveEnding(MoralChoiceSystem moralChoice) {
public static bool IsGameOver(int livingSurvivorCount) => livingSurvivorCount <= 0;
public MilitaryBranchSystemState CaptureState() => Clone(_state);
public void RestoreState(MilitaryBranchSystemState state) {
```


# Appendix Q.556 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Collectibles/CollectibleEffectDispatcher.cs`

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


# Appendix Q.557 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`

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


# Appendix Q.558 — Additional Current Architecture Evidence: `src/Main.MoralChoice.cs`

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


# Appendix Q.559 — Additional Current Architecture Evidence: `src/UI/MoralChoiceModal.cs`

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


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Factions/RebelBranchSystem.cs`

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


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/BureaucraticDocumentCatalog.cs`

### `Assets/Ashfall.Core/Narrative/BureaucraticDocumentCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 530 lines / 21274 bytes.
- SHA-256: `edfd58f1440ff67778b8396e389bbc8038143dd0c9e4eb833a7e269dcd6b8bb2`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum BureaucraticDocumentTruthClass
public sealed class BureaucraticDocumentDefinition
public string DocId { get; }
public string DocType { get; }
public string Title { get; }
public int PostedDay { get; }
public string PostedBy { get; }
public string Location { get; }
public string Material { get; }
public string Transcript { get; }
public IReadOnlyList<string> Tags { get; }
public BureaucraticDocumentTruthClass TruthClass { get; }
public string LocationId { get; }
public IReadOnlyList<string> ProducerIds { get; }
public IReadOnlyList<string> RelatedDocumentIds { get; }
public string KnowledgeKey => BureaucraticDocumentDiscoverySystem.KnowledgeKey(DocId);
public sealed class BureaucraticDocumentCatalog
public IReadOnlyList<BureaucraticDocumentDefinition> Documents => _readOnlyDocuments;
public int Count => _documents.Count;
public bool TryGet(string docId, out BureaucraticDocumentDefinition document) {
public sealed class BureaucraticDocumentCatalogLoadResult
public int SchemaVersion { get; internal set; }
public BureaucraticDocumentCatalog Catalog { get; internal set; } =
public List<string> Warnings { get; } = new List<string>();
public List<string> Errors { get; } = new List<string>();
public bool IsSuccess => Errors.Count == 0;
public sealed class BureaucraticDocumentCatalogLoader
public const string DocumentsFileName = "narrative/bureaucratic_documents_expansion.json";
public const string RuntimeMapFileName = "narrative/bureaucratic_document_runtime_map.json";
public BureaucraticDocumentCatalogLoadResult Load(string dataDirectory) {
public int schema_version;
public List<RawDocument?>? documents;
public string? doc_id;
public string? doc_type;
public string? title;
public int posted_day;
public string? posted_by;
public string? location;
public string? material;
public string? transcript;
public string[]? tags;
public int schema_version;
public List<RawMapping?>? documents;
public string? doc_id;
public string? truth_class;
public string? location_id;
public string[]? producer_ids;
public string[]? related_doc_ids;
public static RawMapping Unresolved(string id) => new RawMapping
public enum BureaucraticDocumentDiscoveryStatus
public sealed class BureaucraticDocumentDiscoveryResult
public string DocId { get; }
public string ProducerId { get; }
public BureaucraticDocumentDiscoveryStatus Status { get; }
public bool Changed => Status == BureaucraticDocumentDiscoveryStatus.Discovered;
public sealed class BureaucraticDocumentDiscoverySystem
public const string KnowledgePrefix = "bureaucratic_document_";
public BureaucraticDocumentCatalog Catalog => _catalog;
public static string KnowledgeKey(string docId) => KnowledgePrefix + (docId ?? string.Empty);
public bool IsDiscovered(JournalSystem journal, string docId) {
public BureaucraticDocumentDiscoveryResult Discover( string docId, string producerId, int currentDay, JournalSystem journal) {
public IReadOnlyList<BureaucraticDocumentDiscoveryResult> DiscoverByProducer( string producerId, int currentDay, JournalSystem journal) {
```


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs`

### `Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 310 lines / 15289 bytes.
- SHA-256: `1de3b3d2d06a1d1caadc79c401928064930aa039c048b0dc91332e8bd303444c`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=11; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class IndependentBranchSystem
public const string SystemId = "independent_branch_system";
public const int MinStanding = -100;
public const int MaxStanding = 100;
public const int HostileThreshold = -50;
public const int AlliedThreshold = 50;
public event Action<string>? OnBranchCommitted;
public event Action<string, int>? OnPonrLocked;
public event Action<string>? OnEndingResolved;
public event Action<int>? OnMilitaryStandingChanged;
public event Action<int>? OnRebelStandingChanged;
public IndependentBranchSystemState State => _state;
public int CurrentDay => _state.timeline.currentDay;
public string? CommittedBranchId => string.IsNullOrEmpty(_state.branch.branchId) ? null : _state.branch.branchId;
public bool IsPonrLocked => _state.branch.ponrLocked;
public int MilitaryStanding => _state.militaryStanding.standing;
public int RebelStanding => _state.rebelStanding.standing;
public bool IsHostileToMilitary => _state.militaryStanding.isHostile;
public bool IsHostileToRebel => _state.rebelStanding.isHostile;
public string? ResolvedEndingId => string.IsNullOrEmpty(_state.branch.resolvedEndingId) ? null : _state.branch.resolvedEndingId;
public void AdvanceDay(int day) {
public void ModifyMilitaryStanding(int delta) {
public void ModifyRebelStanding(int delta) {
public string CommitBranch(string branchId, MoralChoiceSystem moralChoice, PrpfStandingSystem? prpf = null) {
public void LockPointOfNoReturn() {
public string ResolveEnding(MoralChoiceSystem moralChoice) {
public static bool IsGameOver(int livingSurvivorCount) => livingSurvivorCount <= 0;
public IndependentBranchSystemState CaptureState() => Clone(_state);
public void RestoreState(IndependentBranchSystemState state) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Factions/PrpfStandingSystem.cs`

### `Assets/Ashfall.Core/Factions/PrpfStandingSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 203 lines / 8152 bytes.
- SHA-256: `472cea4b833b33f2236360dfe03662b034a31c48f697d951a553fc285f60a120`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PrpfStandingSystem
public const string SystemId = "prpf_standing_system";
public const int MinStanding = -100;
public const int MaxStanding = 100;
public const int HostileThreshold = -50;
public const int AlliedThreshold = 50;
public const int MinAlignment = -200;
public const int MaxAlignment = 200;
public const MoralPathBand JoinMinPlayerMoralBand = MoralPathBand.SlightlyPositive;
public event Action<int>? OnStandingChanged;
public event Action<int>? OnAlignmentChanged;
public event Action? OnJoined;
public event Action? OnOpposed;
public PrpfSystemState State => _state;
public int Standing => _state.standing.standing;
public bool IsHostile => _state.standing.isHostile;
public bool IsAllied => _state.standing.isAllied;
public int Alignment => _state.alignment.alignment;
public bool IsJoined => _state.joined;
public bool IsOpposed => _state.opposed;
public void ModifyStanding(int delta) {
public void ShiftFactionAlignment(int delta) {
public bool TryJoin(MoralChoiceSystem moralChoice) {
public void Oppose() {
public void TickDay(int day) {
public PrpfSystemState CaptureState() => Clone(_state);
public void RestoreState(PrpfSystemState state) {
```


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/LibraryStudySystem.cs`

### `Assets/Ashfall.Core/LibraryStudySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 353 lines / 15082 bytes.
- SHA-256: `d8ed15df8fd325172f8ad2b9e2ae8629172d95cbf94708b833150fda32039f5f`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class LibraryStudyState
public string systemId = LibraryStudySystem.SystemId;
public List<StudyJob> activeJobs = new List<StudyJob>();
public List<string> completedManualIds = new List<string>();
public int totalStudyHours;
public sealed class ManualDefinition
public string manual_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string category { get; set; } = string.Empty;       // "technical", "medical", "military", etc.
public int studyHoursRequired { get; set; } = 10;
public float fatiguePerHour { get; set; } = 0.3f;
public float moraleEffect { get; set; } = -0.5f;           // studying is draining
public List<string> skillXpGrants { get; set; } = new List<string>(); // skill_id, xp_amount pairs
public List<string> researchUnlocks { get; set; } = new List<string>();
public List<string> knowledgeUnlocks { get; set; } = new List<string>();
public List<string> prerequisites { get; set; } = new List<string>();
public bool requiresPower { get; set; } = true;
public List<string> lootTableIds { get; set; } = new List<string>();
public List<string> expeditionRewardIds { get; set; } = new List<string>();
public List<string> traderPoolIds { get; set; } = new List<string>();
public string archiveScribingRecipeId { get; set; } = string.Empty;
public List<string> startingOriginIds { get; set; } = new List<string>();
public string originFacility { get; set; } = string.Empty;
public int technicalComplexityTier { get; set; } = 1;
public string schematicSummary { get; set; } = string.Empty;
public sealed class StudyJob
public string jobId = string.Empty;
public string manualId = string.Empty;
public string readerId = string.Empty;
public int dayStarted = -1;
public float progressHours;
public bool isComplete;
public bool isCancelled;
public sealed class LibraryStudySystem
public const string SystemId = "library_study";
public Func<bool>? PowerAvailable { get; set; }
public bool IsManualPowered(string manualId) {
public LibraryStudyState State => _state;
public IReadOnlyDictionary<string, ManualDefinition> Catalog => _catalog;
public event Action<StudyJob> OnJobCompleted;
public event Action OnLibraryChanged;
public bool IsReaderStudying(string survivorId) {
public static string NormalizeDiscipline(string category) {
public float GetComprehensionRate(string readerId, string manualId) {
public float GetEffectiveStudyHours(string readerId, string manualId) {
public float GetEstimatedDays(string readerId, string manualId) {
public void LoadCatalog(List<ManualDefinition> manuals) {
public ActionResult StartStudy(string manualId, string readerId) {
public ActionResult CancelStudy(string jobId) {
public void TickDay(int day) {
public List<StudyJob> GetActiveJobs() => _state.activeJobs.FindAll(j => !j.isComplete && !j.isCancelled);
public bool IsManualCompleted(string manualId) => _state.completedManualIds.Contains(manualId);
public LibraryStudyState CaptureState() => CloneState(_state);
public void RestoreState(LibraryStudyState saved) {
```


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/NarrativeDiscoveryCatalog.cs`

### `Assets/Ashfall.Core/Narrative/NarrativeDiscoveryCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1551 lines / 83735 bytes.
- SHA-256: `b745f1e9265ba36bb0169d2fddadc1274d5f551f0572b5992e5d94c371e35240`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class NarrativeDiscoveryManifestEntry
public string discovery_id = string.Empty;
public string source_catalog = string.Empty;
public string source_record_id = string.Empty;
public string channel = string.Empty;
public string producer_id = string.Empty;
public string[] producer_ids = Array.Empty<string>();
public int min_day = 1;
public int weight = 1;
public bool one_time = true;
public string truth_class = string.Empty;
public string provenance_label = string.Empty;
public string identity_status = string.Empty;
public string numeric_claim_label = string.Empty;
public string[] related_discovery_ids = Array.Empty<string>();
public string DiscoveryId => discovery_id;
public string SourceCatalog => source_catalog;
public string SourceRecordId => source_record_id;
public string Channel => channel;
public string ProducerId => producer_id;
public IReadOnlyList<string> ProducerIds => producer_ids;
public int MinDay => min_day;
public int Weight => weight;
public bool OneTime => one_time;
public string TruthClass => truth_class;
public string ProvenanceLabel => provenance_label;
public string IdentityStatus => identity_status;
public string NumericClaimLabel => numeric_claim_label;
public IReadOnlyList<string> RelatedDiscoveryIds => related_discovery_ids;
public sealed class NarrativeDiscoveryManifestFile
public int schema_version = 1;
public List<NarrativeDiscoveryManifestEntry> entries = new List<NarrativeDiscoveryManifestEntry>();
public sealed class NarrativeDiscoveredRecord
public string DiscoveryId { get; set; } = string.Empty;
public string KnowledgeKey { get; set; } = string.Empty;
public string SourceCatalog { get; set; } = string.Empty;
public string SourceRecordId { get; set; } = string.Empty;
public string Channel { get; set; } = string.Empty;
public string ProducerId { get; set; } = string.Empty;
public string[] ProducerIds { get; set; } = Array.Empty<string>();
public int MinDay { get; set; } = 1;
public string Title { get; set; } = string.Empty;
public string Subtitle { get; set; } = string.Empty;
public string BodyText { get; set; } = string.Empty;
public string Category { get; set; } = string.Empty;
public string[] Tags { get; set; } = Array.Empty<string>();
public string TruthClass { get; set; } = string.Empty;
public string ProvenanceLabel { get; set; } = string.Empty;
public string IdentityStatus { get; set; } = string.Empty;
public string NumericClaimLabel { get; set; } = string.Empty;
public string RecordFamily { get; set; } = string.Empty;
public string FacilityOrStationLabel { get; set; } = string.Empty;
public string TechnicalSummary { get; set; } = string.Empty;
public string[] RelatedDiscoveryIds { get; set; } = Array.Empty<string>();
public interface INarrativeSourceAdapter
public static class NarrativeJsonHelpers
public static string GetStringProp(JsonElement elem, string propName, string fallback = "") {
public static string[] GetStringArrayProp(JsonElement elem, string propName) {
public static int GetIntProp(JsonElement elem, string propName, int fallback = 0) {
public static float GetFloatProp(JsonElement elem, string propName, float fallback = 0f) {
public sealed class ProcessLogSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) => HandledCatalogs.Contains(sourceCatalog);
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class BunkerGlitchSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class BunkerBlueprintSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class BunkerCourtSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class WireConfessionSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class TradeLedgerSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class RegionalTreatySourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class SurgeonsCasebookSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class DeadHandDirectiveSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class CourierDispatchSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public static class PersonalLetterRuntimeContract
public const string LettersExpansionCatalog = "narrative/letters_expansion.json";
public const string UnsentLettersBatch2Catalog = "narrative/unsent_letters_batch_2.json";
public static readonly string[] SourceCatalogs = {
public static bool IsSourceCatalog(string sourceCatalog) {
public sealed class PersonalLetterSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public static class AbyssalAnomaliesRuntimeContract
public const string HydrophoneCatalog = "narrative/hydrophone_acoustic_logs.json";
public const string GeothermalCatalog = "narrative/geothermal_borehole_logs.json";
public const string CryopodCatalog = "narrative/cryopod_failure_logs.json";
public const string SaltMineCatalog = "narrative/salt_mine_inscriptions.json";
public static readonly string[] SourceCatalogs = {
public static bool IsSourceCatalog(string sourceCatalog) {
public static bool IsActivatedSourceRecord(string sourceRecordId) => AbyssalAnomaliesProjection.IsActivated(sourceRecordId);
public sealed class AbyssalAnomaliesSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public static class FringeCultRuntimeContract
public const string CobaltCatalog = "narrative/cobalt_liturgies.json";
public const string IronCatalog = "narrative/iron_synod_canons.json";
public const string HymnalCatalog = "narrative/geophone_hymnals.json";
public const string EpitaphCatalog = "narrative/wasteland_grave_epitaphs.json";
public static readonly string[] SourceCatalogs = {
public static bool IsSourceCatalog(string sourceCatalog) {
public static string DefaultTruthClass(string sourceCatalog) {
public static bool IsValidTruthClass(string truthClass) {
public sealed class FringeCultSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) => FringeCultRuntimeContract.IsSourceCatalog(sourceCatalog);
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public static class PaperPrintRuntimeContract
public const string HollanderCatalog = "narrative/hollander_beater_pulping_logs.json";
public const string DeckleCatalog = "narrative/deckle_mould_watermark_audits.json";
public const string PressCatalog = "narrative/screw_press_felt_reports.json";
public const string SizingCatalog = "narrative/tub_sizing_gelatin_assays.json";
public const string RagPulpCatalog = "narrative/rag_pulp_beater_records.json";
public const string InkCatalog = "narrative/iron_gall_ink_acidity_reports.json";
public const string TypeCatalog = "narrative/typographic_lead_wear_logs.json";
public const string StencilCatalog = "narrative/stencil_propaganda_smear_logs.json";
public static readonly string[] SourceCatalogs = {
public static bool IsSourceCatalog(string sourceCatalog) {
public static bool IsPaperMakingCatalog(string sourceCatalog) =>
public static string GetFamily(string sourceCatalog) {
public sealed class PaperPrintSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) => PaperPrintRuntimeContract.IsSourceCatalog(sourceCatalog);
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public static class BoneHornRuntimeContract
public const string DegreasingCatalog = "narrative/bone_degreasing_prep_logs.json";
public const string SawingCatalog = "narrative/antler_horn_sawing_records.json";
public const string PolishingCatalog = "narrative/scraping_polishing_reports.json";
public const string ToolAssayCatalog = "narrative/needle_awl_hook_assays.json";
public static readonly string[] SourceCatalogs = {
public static bool IsSourceCatalog(string sourceCatalog) {
public static string GetFamily(string sourceCatalog) {
public sealed class BoneHornSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) => BoneHornRuntimeContract.IsSourceCatalog(sourceCatalog);
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class NarrativeDiscoveryCatalog
public IReadOnlyList<NarrativeDiscoveredRecord> AllRecords => _records;
public int Count => _records.Count;
public void RegisterAdapter(INarrativeSourceAdapter adapter) {
public void Clear() {
public void LoadFromFiles(string dataDirectory, IFileIO files) {
public void Load(string manifestJson, string dataDirectory, IFileIO files) {
public bool TryGetRecord(string discoveryId, out NarrativeDiscoveredRecord? record) {
public IReadOnlyList<NarrativeDiscoveredRecord> GetByProducer(string producerId) {
public IReadOnlyList<NarrativeDiscoveredRecord> GetByChannel(string channel) {
public bool TryDiscover(string discoveryId, JournalSystem journal, out NarrativeDiscoveredRecord? record) {
```


# Appendix Q.566 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs`

### `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 475 lines / 21068 bytes.
- SHA-256: `a973da78df420959de81f7a109374059b3fbc4f84752836dc06f34b0aa8103ce`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum SurvivorDeathCause
public sealed class SurvivorFateEvent
public string survivorId = string.Empty;
public SurvivorDeathCause cause = SurvivorDeathCause.Unknown;
public string causeDetail = string.Empty;  // e.g. disease id, encounter id
public int day;
public string source = string.Empty;       // reporting system id, for audit
public bool isPlayerAvatar;                // distinguishes avatar death from roster death
public SurvivorFateEvent Clone() => new SurvivorFateEvent
public sealed class SurvivorFateSaveState
public string systemId = SurvivorFateSystem.SystemId;
public List<SurvivorFateEvent> fates = new List<SurvivorFateEvent>();
public sealed class SurvivorFateSave
public const int CurrentSaveVersion = 1;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public SurvivorFateSaveState State = new SurvivorFateSaveState();
public string Checksum = string.Empty;
public sealed class SurvivorFateSystem
public const string SystemId = "survivor_fate_system";
public const string CounterDeathsTotal = "deaths_total";
public const string FlagSurvivorDiedPrefix = "flag_survivor_died_";
public const string JournalKeyPrefix = "survivor_death_";
public const float GriefMoraleDelta = -8f;
public const string GriefModifierSource = "grief.shelter_loss";
public event Action<SurvivorFateEvent> OnSurvivorFate;
public event Action<SurvivorFateEvent> OnLastSurvivorDied;
public IReadOnlyList<SurvivorFateEvent> Fates => _state.fates;
public bool HasFate(string survivorId) =>
public SurvivorFateEvent FindFate(string survivorId) =>
public int DeathCount => _state.fates.Count;
public SurvivorFateEvent ReportDeath(SurvivorFateEvent fate) {
public SurvivorFateEvent ReportDeath( string survivorId, SurvivorDeathCause cause, string causeDetail = "", string source = "", bool isPlayerAvatar = false,
public int ReconcileFromRoster() {
public void DrainDayEvents(List<DayStateChangeEvent> target) {
public int PendingDayEventCount => _pendingDayEvents.Count;
public SurvivorFateSaveState CaptureState() {
public void RestoreState(SurvivorFateSaveState saved) {
public static string DescribeCause(SurvivorFateEvent fate) {
```


# Appendix Q.567 — Additional Current Architecture Evidence: `src/Host/ContentUtilizationRuntimeCollector.cs`

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


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/ArchiveDeskSystem.cs`

### `Assets/Ashfall.Core/ArchiveDeskSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 204 lines / 8404 bytes.
- SHA-256: `9bde74b2c74012b2c4e4c47cef31fc7e91b2145a1e6c6f1a9b2ff787d8bd91f2`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ArchiveDeskState
public string systemId = ArchiveDeskSystem.SystemId;
public List<TranscriptionJob> queue = new List<TranscriptionJob>();
public List<string> unlockedEvidenceIds = new List<string>();
public int totalTranscriptions;
public sealed class InkMaterialDefinition
public string ink_id = string.Empty;
public string display_name = string.Empty;
public float legibilityScore = 1f;      // 0-1
public float archivalLongevityDays = 365f;
public float fadeRatePerDay = 0.001f;
public string requiredItemId = string.Empty;
public int requiredAmount = 1;
public sealed class TranscriptionJob
public string jobId = string.Empty;
public string evidenceId = string.Empty;
public string archivistId = string.Empty;
public string inkId = string.Empty;
public int dayStarted = -1;
public float progressHours;
public float totalHoursRequired = 4f;
public bool isComplete;
public bool isCancelled;
public float legibilityScore = 1f;
public string journalEntryId = string.Empty;
public sealed class ArchiveDeskSystem
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public RiskBiasTrait RiskBias { get; set; } = RiskBiasTrait.Realist;
public const string SystemId = "archive_desk";
public ArchiveDeskState State => _state;
public IReadOnlyDictionary<string, InkMaterialDefinition> Catalog => _inkCatalog;
public event Action<TranscriptionJob> OnJobCompleted;
public event Action OnArchiveChanged;
public void LoadInkCatalog(List<InkMaterialDefinition> inks) {
public ActionResult QueueTranscription(string evidenceId, string archivistId, string inkId) {
public ActionResult CancelJob(string jobId) {
public void TickDay(int day) {
public List<TranscriptionJob> GetActiveJobs() => _state.queue.FindAll(j => !j.isComplete && !j.isCancelled);
public bool IsEvidenceUnlocked(string evidenceId) => _state.unlockedEvidenceIds.Contains(evidenceId);
public ArchiveDeskState CaptureState() => CloneState(_state);
public void RestoreState(ArchiveDeskState saved) {
```


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs`

### `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 489 lines / 20000 bytes.
- SHA-256: `ae757ef1db3c06fc7c23741ada561df5e2b07ca1585de1c2e6578811bae2e1de`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=16; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class CrossingQuestStage
public class CrossingQuestChoice
public class CrossingQuestDef
public class CrossingStageNarrativeEvent
public string questId { get; set; } = "";
public string questDisplayName { get; set; } = "";
public int stageIndex { get; set; }
public string stageId { get; set; } = "";
public string stageText { get; set; } = "";
public string briefing { get; set; } = "";
public bool isCompletion { get; set; }
public class CrossingQuestProgress
public string questId = "";
public int currentStage;
public bool started;
public bool completed;
public bool failed;
public string chosenChoiceId = "";
public class CrossingQuestSystemState
public string systemId = CrossingQuestSystem.SystemId;
public int lastTickedDay;
public List<CrossingQuestProgress> quests = new();
public HashSet<string> setFlags = new();
public HashSet<string> dispatchedStageEvents = new();
public class CrossingQuestSystem
public const string SystemId = "crossing_quest_system";
public const string OpeningQuest = "quest_crossing_the_vouch";
public event Action<string, int> OnQuestStageChanged;
public event Action<string> OnQuestStarted;
public event Action<string> OnQuestCompleted;
public event Action<string> OnQuestFailed;
public event Action<string, string> OnFlagSet;
public event Action<CrossingStageNarrativeEvent> OnStageNarrativeEmitted;
public event Action<CrossingQuestSystemState> OnStateChanged;
public CrossingQuestSystemState State => _state;
public void BindCatalog(IReadOnlyList<CrossingQuestDef> catalog) {
public void BindMoralSystem(Ashfall.Core.MoralChoice.MoralChoiceSystem? moralSystem) {
public CrossingQuestDef? GetDef(string questId) {
public IReadOnlyList<CrossingQuestDef> Catalog => _catalog;
public void BindConsequenceLedger(IFlagLedger? ledger) {
public List<CrossingQuestDef> GetVisibleQuests(int currentDay) {
public List<CrossingQuestDef> GetEligibleQuests(int currentDay, bool hasVouchAccess = false) {
public List<CrossingQuestDef> GetAvailableQuests(int currentDay) => GetEligibleQuests(currentDay, false);
public bool IsQuestCompleted(string questId) {
public bool IsQuestFailed(string questId) {
public bool IsQuestStarted(string questId) {
public CrossingQuestProgress? GetProgress(string questId) {
public void TickDaily(int currentDay, bool hasVouchAccess = false) {
public bool StartQuest(string questId, int currentDay) {
public bool FailQuest(string questId) {
public int AdvanceStage(string questId) {
public bool MakeChoice(string questId, string choiceId) {
public bool HasFlag(string flag) => _state.setFlags.Contains(flag);
public event Action? OnOpeningQuestCompleted;
public CrossingQuestSystemState CaptureState() {
public void RestoreState(CrossingQuestSystemState? saved) {
public static class CrossingQuestCatalogLoader
public const string FileName = "crossing_quests.json";
public static List<CrossingQuestDef> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null) {
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DeepCoastHeadlessDemo.cs`

### `Assets/Ashfall.Core/DeepCoastHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 379 lines / 21629 bytes.
- SHA-256: `aa6e8be3cbea443b4290465ced3815d6aee2faa57780c9c951e878a4aed50d6a`.
- Architecture signals: seeded references=7; save/restore symbols=6; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DeepCoastHeadlessDemo
public const int DefaultSeed = 4048;
public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null) {
```


# Appendix Q.571 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Factions/MilitaryBranchState.cs`

### `Assets/Ashfall.Core/Factions/MilitaryBranchState.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 72 lines / 2881 bytes.
- SHA-256: `2d2fe40cba5e0e744daa4bd29f9486fd701a5e4d48d28f56d95494b7f1fb9d09`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class MilitaryBranchTimelineState
public int currentDay = 0;
public class FactionAlignmentRecord
public string factionId = string.Empty;
public int alignment = -80; // Military starts evil-leaning per design, swayable by the player.
public class MilitaryBranchRecord
public string branchId = string.Empty;
public bool committed = false;
public bool ponrLocked = false;
public int ponrLockedDay = -1;
public string resolvedEndingId = string.Empty;
public class MilitaryBranchSystemState
public string systemId = MilitaryBranchSystem.SystemId;
public int schemaVersion = 1;
public MilitaryBranchTimelineState timeline = new MilitaryBranchTimelineState();
public MilitaryBranchRecord branch = new MilitaryBranchRecord();
public FactionAlignmentRecord militaryAlignment = new FactionAlignmentRecord {
public List<string> setFlags = new List<string>();
```


# Appendix Q.572 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs`

### `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 403 lines / 14637 bytes.
- SHA-256: `364d311490e3a73f9e9d2d31861f78f70ecb80a4615f9b48ade364cb2330e7a8`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PerformanceCampaignHarness : IDisposable
public CampaignDayCoordinator Coordinator { get; }
public SurvivorRosterSystem Survivors { get; }
public Inventory.Inventory Inventory { get; }
public JournalSystem Journal { get; }
public WeatherSystem Weather { get; }
public ExpeditionSystem Expeditions { get; }
public LocationEvolutionSystem LocationEvolution { get; }
public WildlifeMigrationSystem Wildlife { get; }
public LandmarkDegradationSystem Landmark { get; }
public int CurrentDay => Coordinator.Calendar is Ashfall.Core.Clock.ISimClock simClock ? simClock.DayIndex : Coordinator.LastAdvancedDay;
public ISeededRng Rng { get; }
public double AdvanceDays(int days) {
public string CaptureSavePayload() {
public double MeasureSaveLatency() {
public double MeasureLoadLatency(string payload) {
public static double MeasureChecksumLatency(string payload) {
public long MeasureRetainedMemoryAfterNewGame() {
public void Dispose() {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public string Id => "perf_author";
public string DisplayName => "Perf";
public Ashfall.Core.Journal.RiskBiasTrait RiskBias => Ashfall.Core.Journal.RiskBiasTrait.Realist;
```


# Appendix Q.573 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Print/BroadsheetPressLedger.cs`

### `Assets/Ashfall.Core/Print/BroadsheetPressLedger.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 260 lines / 12706 bytes.
- SHA-256: `98a790707c68e2a1838a68e28c6d132cc203e21c639804cf76ad58743d51eae9`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PressedPublication
public string PublicationId { get; set; } = string.Empty;
public int Kind { get; set; }
public string Headline { get; set; } = string.Empty;
public int CopiesPrinted { get; set; }
public int AudienceReachPermille { get; set; }
public int MoraleStabilizationPermille { get; set; }
public int Day { get; set; } = 1;
public PublicationKind KindValue => (PublicationKind)Kind;
public PressedPublication Clone() => new PressedPublication
public sealed class BroadsheetPressState
public int SchemaVersion { get; set; } = 1;
public TypeTrayState TypeTray { get; set; } = new TypeTrayState();
public List<PressedPublication> Publications { get; set; } = new List<PressedPublication>();
public int CumulativeCopiesPrinted { get; set; }
public int CumulativeReachPermille { get; set; }
public int CumulativeMoraleStabilizedPermille { get; set; }
public int DebunkPamphletsPrinted { get; set; }
public BroadsheetPressState Clone() => new BroadsheetPressState
public struct BroadsheetPressCensus
public int PublicationCount { get; }
public int CumulativeCopiesPrinted { get; }
public int AverageReachPermille { get; }
public int TypePiecesAvailable { get; }
public int TypeWearPermille { get; }
public int InkReservoirPermille { get; }
public int PaperStockPermille { get; }
public int DebunkPamphletsPrinted { get; }
public bool IsTypeDegraded { get; }
public sealed class BroadsheetPressLedger
public const int MaxArchivedPublications = 200;
public TypeTrayState Tray => _state.TypeTray;
public IReadOnlyList<PressedPublication> Publications => _state.Publications;
public int PublicationCount => _state.Publications.Count;
public BroadsheetPressState CaptureState() => _state.Clone();
public void RestoreState(BroadsheetPressState? saved) {
public PrintRunResult ExecuteRun( string? publicationId, PublicationKind kind, int targetCopies, int compositorSkillPermille, int shelterPopulation,
public PressedPublication? FindPublication(string publicationId) =>
public void RestoreTypeTray(int freshTypePiecesAdded) =>
public void RestockConsumables(int inkPermille, int paperPermille, int freshTypePieces) {
public int CalculateDebunkCorrection(int rumorStrengthPermille, int pamphletAudienceReachPermille, int evidenceQualityPermille) =>
public BroadsheetPressCensus GetCensus() {
public void Clear() {
```


# Appendix Q.574 — Additional Current Architecture Evidence: `src/Host/RumorNetworkSelfTest.cs`

### `src/Host/RumorNetworkSelfTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 146 lines / 7073 bytes.
- SHA-256: `845ea7d60bf9cac197d68bc2d191b8062d63af92c48fed0b62d76291b208e914`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
internal static class RumorNetworkSelfTest
public static int Run(string dataDirectory) {
```


# Appendix Q.575 — Additional Current Architecture Evidence: `src/Journal/JournalSelfTest.cs`

### `src/Journal/JournalSelfTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 228 lines / 12076 bytes.
- SHA-256: `ce4c5c32c80278be6bd03f50dcd52d1d87b49478fa2f8d4463c45c1f7dbc3c70`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class JournalSelfTest
public static int Run(JournalCatalogs catalogs) {
```


# Appendix Q.576 — Additional Current Architecture Evidence: `src/Host/DeepCoastHostSession.cs`

### `src/Host/DeepCoastHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 429 lines / 22369 bytes.
- SHA-256: `9bc0543abcb1ccb531d92489e906c66c950c69da29668beb6244f6b5ba601499`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DeepCoastHostSession
public const int DemoSeed = 4048;
public District8DeepCoastSystem DeepCoast { get; }
public JournalSystem Journal { get; }
public FactionStanceEngine Stances { get; }
public HoldfastTradeInventory Inventory { get; }
public MaritimeHostSession Maritime { get; }
public string LastEvent { get; private set; } = string.Empty;
public static DeepCoastHostSession Create( District8DeepCoastSystem deepCoast = null!, JournalSystem journal = null!, FactionStanceEngine stances = null!, HoldfastTradeInventory inventory = null!, MaritimeHostSession maritime = null!,
public string Survey(int day) {
public string Decide(string decisionId, int day) {
public string ClearPerimeter(int day) {
public string ClearChannel(int day) {
public CommandResult RepairBerth(int day) {
public string StartDockDive(string diverId, string operatorId, int day) {
public string TickDockDive(float seconds) {
public string CrankDockDive() {
public string AdvanceDockDive(int noise) {
public string CompleteDockDive(bool success, List<SalvageEntry> rewards = null!, int day = 1) {
public bool IsRouteNodeBlocked(string nodeId) {
public bool DockExpeditionAvailable => DeepCoast.IsNodeAccessible(District8DeepCoastSystem.DockId);
public bool IsFleetActive => DeepCoast.IsFleetStoodUp;
public void TickDaily(int day, WeatherKind weather = WeatherKind.Clear) {
public string StatusLine() {
public District8DeepCoastState CaptureDeepCoast() => DeepCoast.CaptureState();
public void RestoreDeepCoast(District8DeepCoastState state) => DeepCoast.RestoreState(state);
public void SetCurrentDay(int day) => _lastDay = day > 0 ? day : 1;
```


# Appendix Q.577 — Additional Current Architecture Evidence: `src/Host/CodexHostSession.cs`

### `src/Host/CodexHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 132 lines / 5551 bytes.
- SHA-256: `79dc646ce6df36cfcea262b37953ab42519cda19a46b0bafc29580bc614000cb`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CodexHostSession
public static CodexHostSession Create(string dataDir, ILog? log = null) {
public int AuthoredEntryCount => _authored.Count;
public IReadOnlyList<AuthoredCodexEntry> AuthoredEntries => _authored;
public IReadOnlyList<CodexEntryProjection> Build( FieldGuideCatalog? fieldGuide, ResearchState? researchState, IReadOnlyDictionary<string, ResearchKnowledgeDef>? researchCatalog, JournalSystem? journal, int currentDay,
public IReadOnlyList<CodexEntryProjection> BuildKnown( FieldGuideCatalog? fieldGuide, ResearchState? researchState, IReadOnlyDictionary<string, ResearchKnowledgeDef>? researchCatalog, JournalSystem? journal, int currentDay,
public IReadOnlyList<CodexEntryProjection> BuildForLocation( string locationId, JournalSystem? journal, int currentDay, Func<string, bool>? factionContact = null, int maxSpoilerTier = int.MaxValue)
public IReadOnlyDictionary<CodexCategory, int> KnownCountByCategory( FieldGuideCatalog? fieldGuide, ResearchState? researchState, IReadOnlyDictionary<string, ResearchKnowledgeDef>? researchCatalog, JournalSystem? journal, int currentDay,
```


# Appendix Q.578 — Additional Current Architecture Evidence: `src/Host/HostCli.MoralChoice.cs`

### `src/Host/HostCli.MoralChoice.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 199 lines / 9799 bytes.
- SHA-256: `6893cbda7adde1de1c75a6437425ceb9bc7b3e691fcbd9a3fe703598939d3dd0`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunMoralChoiceSelfTest(string dataDirectory) {
```


# Appendix Q.579 — Additional Current Architecture Evidence: `src/UI/JournalDetailPanel.cs`

### `src/UI/JournalDetailPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 134 lines / 5031 bytes.
- SHA-256: `707f3dfef12868787d8b04ab19d0fc2fe85ac37d79068e908a39d2439c7ba395`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=3; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class JournalDetailPanel : Control
public event Action? OnClose;
public bool IsBound => _journal != null;
public int RenderedRowCount { get; private set; }
public void Bind(JournalSystem? journal) {
public void RefreshView() {
public override void _Ready() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
```


# Appendix Q.580 — Additional Current Architecture Evidence: `src/UI/JournalPanel.cs`

### `src/UI/JournalPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 486 lines / 18930 bytes.
- SHA-256: `412d1eb12aed0d996c58bb0b653d3b90517d40855ec738f9b5cfb4c139f5bf26`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=3; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class JournalPanel : Control
public event Action? OnClose;
public void Bind(JournalHostSession session) => Bind(session?.System!);
public void Bind(JournalSystem journal) {
public void RefreshView() {
public override void _Ready() {
public void Open() {
public void Close() {
public void Unbind() {
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
```


# Appendix Q.581 — Additional Current Architecture Evidence: `src/Foundry/SilentFoundryHostSession.cs`

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


# Appendix Q.582 — Additional Current Architecture Evidence: `src/Host/DutyRosterHostSession.cs`

### `src/Host/DutyRosterHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 440 lines / 19490 bytes.
- SHA-256: `3beb81558d909f0dc287c797f1b8f5c188a99ecfdc4c833f470aabc19056f658`.
- Architecture signals: seeded references=0; save/restore symbols=3; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DutyRosterHostSession
public const int DefaultSeed = 908; // roster seed offset: _worldSeed + 1208 style
public DutyRosterSystem Roster { get; }
public MoraleMarkSystem Marks { get; }
public ShelterEncounterSystem Encounters { get; }
public DutyRosterQuestRuntime Quests { get; }
public DutyRosterCatalog Catalog { get; }
public SimClock Clock { get; }
public string LastEvent { get; private set; } = string.Empty;
public int LocationCount => Catalog.Locations.Count;
public int QuestCount => Catalog.Quests.Count;
public int MarkCount => Catalog.Marks.Count;
public int SeasonCount => Catalog.Seasons.Count;
public RoleFitnessVerdict? PreviewRoleFitness(string survivorId, string roleId) => Roster.PreviewRoleFitness(survivorId, roleId);
public DutyHourSnapshot? PreviewDutyHours(string survivorId) => Roster.PreviewDutyHours(survivorId);
public static DutyRosterHostSession Create(string dataDirectory, ILog? log = null, Ashfall.Core.Journal.JournalSystem journal = null!) {
public void Unlock(int day) {
public DutyRosterSave CaptureSave() =>
public void RestoreSave(DutyRosterSave save) =>
public bool SaveState() {
public string StartRosterQuest(string questId) {
public string AdvanceRosterQuest(string questId) {
public string ResolveRosterChoice(string questId, string choiceId) {
public string ActiveQuestProse(string questId) {
public string QuestsLine() {
public string TickDay() {
public void SyncDay(int day) {
public void DrainDayEvents(List<DayStateChangeEvent> target) {
public string TickDay(IReadOnlyList<DutyRosterOccupant> occupants) {
public void SyncHoldfastToDuty( CensusClaimSystem census, IceRoadSystem iceRoad, WaystationSystem waystation, BrineWaterSystem brine, int day)
public DutyRosterHoldfastSnapshot SnapshotForHoldfast() {
public string InspectWall() {
public string ResolveChart(string choiceId) {
public string ResolveInk() {
public string BurnChart() {
public string QueueVisitor(string visitorId) {
public string StartEncounter(string kind) {
public string ActivateSecondWinter() {
public string GrantOverflowAccess() {
public string RegisterOverflowVisit(string nodeId) {
public string BridgeHatchReturn(string survivorId = null!, bool crisis = false) {
public string GrantBlankRowsAccess() {
public string WallLine() {
public string EncountersLine() {
public string MarksLine() {
public string CatalogLine() {
public CommandResult AssignDuty(string role, string survivorId, bool confirmFitnessWarning = false) {
```


# Appendix Q.583 — Additional Current Architecture Evidence: `src/Host/JournalHostSession.cs`

### `src/Host/JournalHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 63 lines / 1760 bytes.
- SHA-256: `5df4457cf7b981872e2a48f8ca6698cdb0f43202d3a69e8047340d70d151263a`.
- Architecture signals: seeded references=0; save/restore symbols=3; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class JournalHostSession : HostSessionBase
public JournalSystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public override void Save() {
public void RestoreSave(JournalSave state) {
```


# Appendix Q.584 — Additional Current Architecture Evidence: `src/UI/QuestsPanel.cs`

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


# Appendix Q.585 — Additional Current Architecture Evidence: `src/Host/ContentCertificationHostSession.cs`

### `src/Host/ContentCertificationHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 138 lines / 7225 bytes.
- SHA-256: `ca32f8fac108e6a72c64fd4b0d4e373934a3371f8a9c1139f1e5957c6195ec86`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ContentCertificationFamily
public string FamilyId { get; }
public string Cluster { get; }
public string CatalogFileName { get; }
public string CanonicalConsumer { get; }
public sealed class ContentCertificationHostSession : HostSessionBase
public static readonly IReadOnlyList<ContentCertificationFamily> Families = new[] {
public ContentCertificationReport LastReport { get; private set; }
public bool HasReport => LastReport.TotalCandidatesEvaluated > 0;
public void MarkCatalogLoaded(string catalogFileName, bool loaded) {
public void MarkConsumerActive(string canonicalConsumer, bool active) {
public bool IsCatalogLoaded(string catalogFileName) => !string.IsNullOrWhiteSpace(catalogFileName)
public bool IsConsumerActive(string canonicalConsumer) => !string.IsNullOrWhiteSpace(canonicalConsumer) && _activeConsumers.Contains(canonicalConsumer);
public ContentCertificationReport Certify() {
public IEnumerable<ContentCertificationFamily> FlaggedFamilies() {
public IEnumerable<ContentCertificationFamily> DormantFamilies() => Families.Where(f => !IsCatalogLoaded(f.CatalogFileName));
public string Describe() {
```


# Appendix Q.586 — Additional Current Architecture Evidence: `src/Host/HostCli.Collectibles.cs`

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


# Appendix Q.587 — Additional Current Architecture Evidence: `src/Host/HostEventAdapter.cs`

### `src/Host/HostEventAdapter.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 257 lines / 11622 bytes.
- SHA-256: `996e3e8410684edcbe5863d95ee66ac01ef48defe66758674318045cca36205f`.
- Architecture signals: seeded references=0; save/restore symbols=3; typed event declarations=7; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class HostEventState
public List<string> triggeredEventIds = new List<string>();
public Dictionary<string, int> eventTriggerDays = new Dictionary<string, int>();
public List<string> dispatchedSourceIds = new List<string>();
public string lastDispatchedEvent = string.Empty;
public class HostEventAdapter
public const string EventThinMarginDisclosure = "event_the_thin_margin_disclosure";
public const string EventThirstySeason = "event_the_thirsty_season";
public const string EventOsteophageExplanation = "event_osteophage_explanation";
public const string EventMeasurementBroadcast = "event_measurement_broadcast";
public event Action<string, string>? OnEventDispatched;
public event Action? StateChanged;
public HostEventState State => _state;
public IReadOnlyList<string> TriggeredEventIds => _state.triggeredEventIds;
public string LastDispatchedEvent => _state.lastDispatchedEvent;
public IReadOnlyList<string> DispatchedSourceIds => _state.dispatchedSourceIds;
public bool HasTriggered(string eventId) => _state.triggeredEventIds.Contains(eventId);
public int GetTriggerDay(string eventId) {
public void TriggerEvent(string eventId, int currentDay) {
public bool DispatchCatalogEvent(string eventId, string bodyText, int currentDay, string sourceId) {
public void EvaluateTriggers( int day, bool hydroAuditDone, bool hydroSeized, bool osteophageInquiry, bool coldCountBroadcast)
public void Dispose() {
public HostEventState CaptureState() {
public void RestoreState(HostEventState? state) {
```


# Appendix Q.588 — Additional Current Architecture Evidence: `src/Journal/JournalCodex.cs`

### `src/Journal/JournalCodex.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 464 lines / 19139 bytes.
- SHA-256: `1881856b99bb59df45dc78c3db6f2d4944d2bcf971d7b8ea0ef9955300a4f798`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum JournalTab
public struct JournalCodexRow
public string? DisplayName;
public string? Meta;
public string? Body;
public bool IsLocked;
public IReadOnlyList<JournalCodexLink>? Links;
public string? NavigationId;
public static JournalCodexRow Locked(string? displayName) {
public sealed class JournalCodexLink
public string Id { get; }
public string Label { get; }
public string RoutePrefix { get; }
public class JournalCodex
public JournalCatalogs Catalogs => _catalogs;
public IReadOnlyList<JournalCodexRow> BuildRows(JournalTab tab) {
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.


# Appendix S — Quality Assurance Pass Record: Plan 110

This record is part of the planning artifact, not a fresh runtime test result.

## Pass 1 — content and premise accuracy
- Content pass replaced the partial-pool premise with the current 21-pool/420-line catalog.
- The historical baseline is separated from the current source/data/test authority.
- Current row counts and owner boundaries are stated without using count as a quality proxy.

## Pass 2 — integration architecture
- Integration pass traced moral band → runtime line → rumor seed → canonical rumor owner.
- Core, data, host, UI, save, event and test seams are named with current paths.
- The plan does not authorize a parallel save section, catalog, manager or host cache.

## Final precision and reaccuracy pass
- Precision pass records the live line-projection residual without inventing per-line mechanics.
- Every embedded current-file hash, focused runner command and master-authority reference is rechecked.
- Any proposed future seam is labeled as requiring a separate claim and premise verification.
