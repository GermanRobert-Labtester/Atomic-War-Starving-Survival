# Plan 06 — Narrative Depth: Letters, Echo/Cassettes, and the Year-of-Ash War Arc

> **Rebuild status:** SUBSTANTIALLY INTEGRATED — REMAINING PLAYER-SURFACE AND CONTINUITY AUDIT
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-2026-09-25`
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

- The original proposal predates substantial implementation. Current source includes `SurvivorLetterDeliverySystem`, `PersonalLetterCatalog`, `EchoSystem`, `CassettePlaybackSystem`, `FactionWarChainRunner`, faction content, radio projections and final-wish ownership.
- The plan therefore becomes a composition plan: letters use relationship and morale callbacks; cassettes use inventory/audio/morale; echoes use their delayed-consequence system; the war arc uses the existing chain runner, radio, standing and epilogue consumers.
- The most valuable remaining work is proving that authored letter/cassette/echo/war content is reachable, save-safe, deterministic where required, and free of real-world or duplicate-final-wish claims.

**Bounded outcome:** Keep the existing letter-delivery, echo, cassette, final-wish, faction-war and radio owners. Do not create `LetterDeliveryLedger`, `EchoAudioPlaybackSystem`, or `FactionWarProjectionEngine` as new authorities. The remaining package is a truthful reachability, UI, save and cross-canon audit.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- The lost-kin letter catalog has 25 rows and a tested delivery lifecycle including found, addressed, delivered, withheld, unanswered and save/restore.
- The echo catalog has 23 rows and a host-persisted EchoSystem surface.
- The cassette catalog has 12 sets, while `CassettePlaybackSystem` tracks collection, play, completion and hidden caches.
- `FactionWarChainRunner` maps authored day 480 to playable day 180 and persists chain progress, visited locations, produced flags and stage resolutions.
- Final wishes remain a separate owner; letters may support `deliver_letter` but cannot create final-wish definitions.

**Master-authority sections applied to this rebase:**

- Volumes 4–5 prose contracts
- Volume 16 world-state notifications
- Volume 36 choice contracts
- Volume 55 encounter authority

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Audit current letter/echo/cassette/war player surfaces and close only proven reachability gaps.
- Unify exposition around owner-specific read models without copying mutable state.
- Add one cross-canon trace matrix linking item, survivor, location, flag, radio, journal and memorial consumers.
- Preserve all sealed radio and final-wish boundaries.

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
| letter discovery, addressing and delivery state | Survivor letter owner | `Assets/Ashfall.Core/Narrative/SurvivorLetterDeliverySystem.cs; src/Main.Narrative.cs` | Owns physical/personal letter delivery; not final wishes. |
| echo choices and delayed consequences | EchoSystem | `Assets/Ashfall.Core/Narrative; src/Main.Echoes.cs` | Owns echo lifecycle and persistence. |
| parts, playback, completion and cache disclosure | CassettePlaybackSystem | `Assets/Ashfall.Core/Audio/CassettePlaybackSystem.cs` | Owns cassette progress; audio host owns playback. |
| authored war chain progression | FactionWarChainRunner | `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs` | Owns stage progression; host applies standing/presentation. |
| end-of-life wishes | FinalWishSystem | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` | Sole final-wish authority. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Narrative Depth: Letters, Echo/Cassettes, and the Year-of-Ash War Arc
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ Survivor letter owner
│   letter discovery, addressing and delivery state
│ EchoSystem
│   echo choices and delayed consequences
│ CassettePlaybackSystem
│   parts, playback, completion and cache disclosure
│ FactionWarChainRunner
│   authored war chain progression
│ FinalWishSystem
│   end-of-life wishes
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

1. **Preserve current state ownership.** Survivor letter owner owns letter discovery, addressing and delivery state: Owns physical/personal letter delivery; not final wishes.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| letter discovery, addressing and delivery state | Survivor letter owner | `Assets/Ashfall.Core/Narrative/SurvivorLetterDeliverySystem.cs; src/Main.Narrative.cs` | Owns physical/personal letter delivery; not final wishes. |
| echo choices and delayed consequences | EchoSystem | `Assets/Ashfall.Core/Narrative; src/Main.Echoes.cs` | Owns echo lifecycle and persistence. |
| parts, playback, completion and cache disclosure | CassettePlaybackSystem | `Assets/Ashfall.Core/Audio/CassettePlaybackSystem.cs` | Owns cassette progress; audio host owns playback. |
| authored war chain progression | FactionWarChainRunner | `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs` | Owns stage progression; host applies standing/presentation. |
| end-of-life wishes | FinalWishSystem | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` | Sole final-wish authority. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. author narrative content in the correct catalog
2. validate references and reachability
3. load through the owning catalog/system
4. discover or intercept through an existing host command
5. mutate only the owning state
6. project relationship, morale, radio, journal or memorial consequences
7. capture and restore through the existing section

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Letter records persist delivery state and matched recipient without copying the survivor relationship ledger.
- Echo state persists surfaced/resolved/delayed-consequence facts through its existing save section.
- Cassette state persists collected, played and completed sets.
- War-chain state persists stage resolution days, visited locations and produced flags.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- No letter outcome creates or mutates a final-wish definition.
- No cassette or echo duplicates an existing radio distress scenario.
- War choices apply standing only through the existing FactionWarSystem port.
- Narrative facts never bypass information-flow and discovery gates.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- Use `survivor_letters_lost_kin.json`, `echoes.json`, `cassette_sets.json`, and faction-war catalogs.
- Do not author another parallel letter/echo/war catalog.
- Any new row requires a real consumer and valid references.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use existing letter/echo/cassette/faction-war save owners.
- No new save section is justified by this audit.
- Mid-choice and post-resolution restores must not replay consequences.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Letter and cassette flows are deterministic by state transition.
- Echo consequence scheduling uses its existing seeded contract.
- Faction war chain progression is integer-day deterministic and has no random roll.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Letter delivery applies modest morale/grief/relationship effects through callbacks.
- Cassette playback emits typed events and may disclose authored cache locations.
- War stages resolve through typed events and host-owned standing/radio/journal projections.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Main.Narrative.cs
- src/Main.Echoes.cs
- src/Audio/AudioEventBridge.cs
- src/UI/FactionCommuniqueBoardPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- All authored prose must obey genre contracts and knowledge horizons.
- Real places and peoples in the historical letter corpus require fictionalization or explicit canon handling before player exposure.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A letter is delivered twice after restore. | Survivor letter owner | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A hidden cache is granted before set completion. | EchoSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A war choice mutates standing outside FactionWarSystem. | CassettePlaybackSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A letter becomes a replacement final wish. | FactionWarChainRunner | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | Audio playback ignores cooldown or accessibility state. | FinalWishSystem | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/NarrativeAndFactionWarIntegrationTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Audio/CassettePlaybackSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/FinalWishSystemTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/FactionWarChainRunnerTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/DistressFollowUpTests.cs`
6. `godot --headless --path . -- --data-integrity-selftest` only if a touched narrative catalog changes.

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — owner census | Map every current letter/echo/cassette/war owner and save section. | No duplicate owner exists. | No production path until the owning implementation package is separately claimed. |
| 1 — reachability audit | Trace every authored row to a live command and consumer. | Orphan rows are documented, not assumed live. | No production path until the owning implementation package is separately claimed. |
| 2 — player-surface audit | Inspect current panels, radio strip and audio bridge. | Only truthful missing surfaces are proposed. | No production path until the owning implementation package is separately claimed. |
| 3 — cross-canon matrix | Validate letters against relationships, final wishes, echoes and memorial facts. | No contradictory or duplicate consequence. | No production path until the owning implementation package is separately claimed. |
| 4 — focused polish | Improve labels, empty states, accessibility and continuity copy. | No new mechanics. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/narrative/survivor_letters_lost_kin.json | READ; MODIFY only for proven canon/content defect | Letter authority |
| Assets/Ashfall.Core/Narrative/SurvivorLetterDeliverySystem.cs | READ; MODIFY only for proven state gap | Letter owner |
| Assets/Ashfall.Core/Audio/CassettePlaybackSystem.cs | READ | Cassette owner |
| Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs | READ; MODIFY only for proven trigger/wiring defect | War chain owner |
| src/Main.Narrative.cs | READ; MODIFY only for bounded reachability gap | Host composition |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Recreating existing systems. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Leaking hidden narrative facts. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Double-applying morale or standing. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Breaking sealed distress or final-wish boundaries. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new war model.
- No replacement cassette system.
- No new distress scenarios.
- No broad UI redesign.

# 23. Rollback and Recovery

- Surface changes are independently reversible.
- State changes require focused round-trip tests and default-tolerant migration.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- Every pillar has one current owner.
- All current row counts are recorded.
- Reachability and save paths are proven.
- No duplicate authority is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Audit current letter/echo/cassette/war player surfaces and close only proven reachability gaps.
- Unify exposition around owner-specific read models without copying mutable state.
- Add one cross-canon trace matrix linking item, survivor, location, flag, radio, journal and memorial consumers.
- Preserve all sealed radio and final-wish boundaries.

## MUST NOT DO

- No new war model.
- No replacement cassette system.
- No new distress scenarios.
- No broad UI redesign.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/NarrativeAndFactionWarIntegrationTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Audio/CassettePlaybackSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/FinalWishSystemTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/FactionWarChainRunnerTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/DistressFollowUpTests.cs`
6. `godot --headless --path . -- --data-integrity-selftest` only if a touched narrative catalog changes.

## FIRST SAFE IMPLEMENTATION STEP

0 — owner census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: letter discovery, addressing and delivery state → Survivor letter owner; echo choices and delayed consequences → EchoSystem; parts, playback, completion and cache disclosure → CassettePlaybackSystem; authored war chain progression → FactionWarChainRunner; end-of-life wishes → FinalWishSystem. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 06.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 06 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by Survivor letter owner or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Narrative/SurvivorLetterDeliverySystem.cs`

### `Assets/Ashfall.Core/Narrative/SurvivorLetterDeliverySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 284 lines / 11488 bytes.
- SHA-256: `1431f806795facd3ab066486f025032e38484d297023c5b80205a073b77b427f`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=17; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class LetterDeliveryStates
public const string NotFound = "not_found";
public const string Found = "found";
public const string Addressed = "addressed";
public const string Delivered = "delivered";
public const string Withheld = "withheld";
public const string Unanswered = "unanswered";
public sealed class SurvivorLetterRecordState
public string letter_id = string.Empty;
public string delivery_state = LetterDeliveryStates.NotFound;
public string? matched_survivor_id;
public int found_day = -1;
public int resolved_day = -1;
public float morale_delta_applied;
public sealed class SurvivorLetterDeliverySaveState
public int schema_version = 1;
public List<SurvivorLetterRecordState> letters = new List<SurvivorLetterRecordState>();
public sealed class DwellerAddressCandidate
public string SurvivorId { get; set; } = string.Empty;
public string Name { get; set; } = string.Empty;
public string Role { get; set; } = string.Empty;
public bool IsAlive { get; set; } = true;
public sealed class SurvivorLetterDeliverySystem
public const string SystemId = "survivor_letter_delivery";
public const float DefaultDeliveryMoraleBonus = 8f;
public const float DefaultWithholdMoralePenalty = -3f;
public event Action<string, int>? OnLetterFound;
public event Action<string, string>? OnLetterAddressed;
public event Action<string, string, float>? OnLetterDelivered;
public event Action<string, string?>? OnLetterWithheld;
public event Action? OnStateChanged;
public SurvivorLetterRecordState GetOrCreateRecord(string letterId) {
public string GetState(string letterId) {
public IReadOnlyList<SurvivorLetterRecordState> GetAllRecords() {
public IReadOnlyList<SurvivorLetterRecordState> GetRecordsByState(string deliveryState) {
public bool MarkFound(string letterId, int day) {
public bool TryAddressToSurvivor( string letterId, IEnumerable<DwellerAddressCandidate> livingDwellers) {
public bool AssignRecipientExplicit(string letterId, string survivorId) {
public bool Deliver(string letterId, int day, Action<string, float>? applyMorale = null, float moraleBonus = DefaultDeliveryMoraleBonus) {
public bool Withhold(string letterId, int day, Action<string, float>? applyMorale = null, float moralePenalty = DefaultWithholdMoralePenalty) {
public bool MarkUnanswered(string letterId, int day) {
public SurvivorLetterDeliverySaveState CaptureState() {
public void RestoreState(SurvivorLetterDeliverySaveState? save) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Audio/CassettePlaybackSystem.cs`

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


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs`

### `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 603 lines / 31953 bytes.
- SHA-256: `5294c96557d10b243115c35fd87fff34a5e8bfaf8ebc546b29723b04ebd2e7b6`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public abstract class FactionWarTrigger
public abstract bool IsSatisfied(FactionWarTriggerContext ctx);
public sealed class FactionWarTriggerContext
public int CurrentDay;
public Func<string, bool> IsChainResolved = _ => false;
public Func<string, bool> HasVisitedLocation = _ => false;
public Func<string, int> StageResolvedDay = _ => -1; // -1 = not yet resolved
public Func<string, bool> IsFlagSet = _ => false;
public sealed class FlagTrigger : FactionWarTrigger
public override bool IsSatisfied(FactionWarTriggerContext ctx) => ctx.IsFlagSet(_flagId);
public sealed class PlayerVisitedTrigger : FactionWarTrigger
public override bool IsSatisfied(FactionWarTriggerContext ctx) => ctx.HasVisitedLocation(_locationId);
public sealed class ChainResolvedTrigger : FactionWarTrigger
public override bool IsSatisfied(FactionWarTriggerContext ctx) => ctx.IsChainResolved(_chainId);
public sealed class DayOffsetTrigger : FactionWarTrigger
public override bool IsSatisfied(FactionWarTriggerContext ctx) {
public sealed class AndTrigger : FactionWarTrigger
public override bool IsSatisfied(FactionWarTriggerContext ctx) {
public sealed class AlwaysTrigger : FactionWarTrigger
public static readonly AlwaysTrigger Instance = new AlwaysTrigger();
public override bool IsSatisfied(FactionWarTriggerContext ctx) => true;
public static class FactionWarTriggerTable
public static readonly Dictionary<string, FactionWarTrigger> ByStageId = Build();
public static FactionWarTrigger For(string stageId) =>
public sealed class FactionWarChainProgress
public string chainId = string.Empty;
public string currentStageId = string.Empty;
public bool resolved;
public List<StageResolution> stageResolutions = new List<StageResolution>();
public sealed class StageResolution
public string stageId = string.Empty;
public int day;
public sealed class FactionWarChainRunnerState
public string systemId = FactionWarChainRunner.SystemId;
public int schemaVersion = 1;
public List<FactionWarChainProgress> chains = new List<FactionWarChainProgress>();
public List<string> visitedLocations = new List<string>();
public int cumulativeMoraleDelta;
public List<string> producedFlags = new List<string>();
public sealed class FactionWarChainRunner
public const string SystemId = "faction_war_chain_runner";
public const int AuthoredEpochStart = 480;
public const int PlayableEpochStart = 180;
public static int ToAuthoredDay(int playableDay) {
public event Action<FactionWarEventChain, FactionWarEventStage>? OnStageSurfaced;
public event Action<FactionWarEventChain, FactionWarEventStage, FactionWarEventChoice>? OnStageResolved;
public event Action<FactionWarEventChain>? OnChainResolved;
public FactionWarChainRunnerState State => _state;
public FactionWarContentCatalog Catalog => _catalog;
public int CumulativeMoraleDelta => _state.cumulativeMoraleDelta;
public Func<string, bool>? ExternalFlagProbe;
public Action<string, int>? StandingDeltaApplier;
public void RecordLocationVisited(string locationId) {
public bool HasVisited(string locationId) => _state.visitedLocations.Contains(locationId);
public bool IsChainResolved(string chainId) {
public FactionWarEventStage? GetSurfacedStage(string chainId, int currentDay) {
public bool IsChoiceAvailable(FactionWarEventStage stage, FactionWarEventChoice choice, int currentDay) {
public bool IsFlagSet(string flagId) {
public void TickDay(int currentDay) {
public void ResolveChoice(string chainId, string stageId, string choiceId, int currentDay) {
public FactionWarChainRunnerState CaptureState() => Clone(_state);
public void RestoreState(FactionWarChainRunnerState state) {
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs`

### `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 348 lines / 13947 bytes.
- SHA-256: `79467fd76488c2f4747cd21ba11833fdcad6d795a8e86f5a62db166941aa6e75`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class FactionStandingRecord
public string factionId = string.Empty;
public int standing = 0; // -100 (Blood Feud) to +100 (Allied)
public int territorialControlPercent = 20; // 0 to 100
public bool isHostile = false;
public bool isAllied = false;
public class FactionDefenseReadinessPressure
public string sourceId = string.Empty;
public string factionId = string.Empty;
public float magnitude;
public int startDay;
public int endDay;
public class FactionWarSystemState
public List<FactionStandingRecord> factions = new List<FactionStandingRecord>();
public int activeWarTension = 50; // 0 to 100
public string dominantFactionId = "faction_central_garrison";
public List<string> enactedDecrees = new List<string>();
public int totalArtilleryStrikesLogged = 0;
public bool isWarActive = false;
public List<FactionDefenseReadinessPressure> defenseReadinessPressures = new List<FactionDefenseReadinessPressure>();
public class FactionWarSystem
public const string SystemId = "faction_war_system";
public const int HostileStandingThreshold = -50;
public FactionWarSystemState State => _state;
public int WarTension => _state.activeWarTension;
public string DominantFactionId => _state.dominantFactionId;
public bool IsAtWar => _state.isWarActive;
public void SetWarActive(bool active) {
public event Action<string, int> OnFactionStandingChanged;
public event Action<string> OnDecreeEnacted;
public event Action<string, string> OnTerritorialClashOccurred;
public int GetStanding(string factionId) {
public void ModifyStanding(string factionId, int delta) {
public void EnactDecree(string decreeId) {
public bool TryApplyDefenseReadinessPressure( string factionId, float magnitude, int startDay, int endDay, string sourceId)
public float GetDefenseReadiness01(string factionId, int day) {
public void SimulateDailyFriction(int day) {
public FactionWarSystemState CaptureState() {
public void RestoreState(FactionWarSystemState state) {
public int ApplyRetention(Records.RetentionPolicyCatalog? catalog) {
```


# Appendix B.06 — Current Code Architecture: `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs`

### `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 398 lines / 16866 bytes.
- SHA-256: `da7c4cb4ed8119ba69a3dccb86246a724b5bea389b2fe7f96b4ef812255ddb21`.
- Architecture signals: seeded references=1; save/restore symbols=4; typed event declarations=18; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FinalWishSurvivorState
public string survivorId = string.Empty;
public string wishType = string.Empty;
public string wishId = string.Empty;
public float daysRemaining;
public int stepsCompleted;
public bool isActive;
public bool hasTerminalPrognosis;
public bool wishCompleted;
public sealed class FinalWishSaveState
public List<FinalWishSurvivorState> survivors = new List<FinalWishSurvivorState>();
public Dictionary<string, string> archetypeWishes = new Dictionary<string, string>();
public class FinalWishSystem
public const float WishCompletedMoraleBuff = 15f;
public const float WishFailedMoralePenalty = -10f;
public const string BuffId = "their_memory_lives_on";
public const float DefaultPrognosisDaysMin = 3f;
public const float DefaultPrognosisDaysMax = 7f;
public const string WishRetrieveHeirloom = "retrieve_heirloom";
public const string WishDeliverLetter = "deliver_letter";
public const string WishBuildMemorial = "build_memorial";
public const string WishTeachLesson = "teach_lesson";
public const string WishReconcile = "reconcile";
public const string WishSeeTheSky = "see_the_sky";
public event Action<string, string, float> OnTerminalPrognosisDeclared;
public event Action<string, string> OnFinalWishStepCompleted;
public event Action<string> OnFinalWishCompleted;
public event Action<string> OnFinalWishFailed;
public event Action<float> OnPermanentMoraleBuffApplied;
public event Action OnStateChanged;
public Action<float> ApplyPermanentShelterMoraleBuff;
public Func<string, string> GetWishNarrativeText;
public ISeededRng Rng;
public IFinalWishCatalog? Catalog;
public void RegisterWish(string archetypeId, string wishType) {
public void DeclareTerminalPrognosis(string survivorId, string archetypeId, bool isAlive) {
public bool AdvanceWishStep(string survivorId, string stepId) {
public void OnPrognosisExpired(string survivorId) {
public void Tick(string survivorId, float gameHours, bool isAlive) {
public bool HasActiveWish(string survivorId) {
public string GetWishType(string survivorId) {
public string GetWishId(string survivorId) {
public float GetDaysRemaining(string survivorId) {
public int GetStepsCompleted(string survivorId) {
public bool HasTerminalPrognosis(string survivorId) {
public bool HasCompletedWish(string survivorId) {
public FinalWishSaveState CaptureState() {
public void RestoreState(FinalWishSaveState save) {
```


# Appendix B.07 — Current Code Architecture: `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs`

### `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 367 lines / 14913 bytes.
- SHA-256: `b44e502a94833538a4ff3301c9782446cd49c51cf0e37db732a247c09ab972a2`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum LoreTriggerType
public enum SignificanceLevel
public sealed class ItemLoreEntry
public string LoreId { get; set; } = string.Empty;
public string ItemInstanceId { get; set; } = string.Empty;
public LoreTriggerType TriggerType { get; set; } = LoreTriggerType.Crafting;
public string Text { get; set; } = string.Empty;
public int Day { get; set; } = 1;
public string AssociatedSurvivorId { get; set; } = string.Empty;
public string AssociatedLocationId { get; set; } = string.Empty;
public sealed class ItemProvenanceChain
public string ItemInstanceId { get; set; } = string.Empty;
public string CrafterSurvivorId { get; set; } = string.Empty;
public int CraftingDay { get; set; } = 0;
public string DiscoveryLocationId { get; set; } = string.Empty;
public int DiscoveryDay { get; set; } = 0;
public string DiscoveryContext { get; set; } = string.Empty;
public List<string> OwnershipChain { get; set; } = new List<string>();
public List<string> LoreEntryIds { get; set; } = new List<string>();
public SignificanceLevel Significance { get; set; } = SignificanceLevel.Mundane;
public sealed class ItemLoreState
public int SchemaVersion { get; set; } = 1;
public int NextSequence { get; set; } = 1;
public List<ItemLoreEntry> LoreEntries { get; set; } = new List<ItemLoreEntry>();
public List<ItemProvenanceChain> Provenances { get; set; } = new List<ItemProvenanceChain>();
public sealed class ItemLoreSystem
public event Action<ItemLoreEntry>? OnLoreAdded;
public event Action<ItemProvenanceChain, SignificanceLevel>? OnSignificanceChanged;
public event Action<string, string>? OnOwnershipTransferred;
public int TrackedItemCount => _state.Provenances.Count;
public int TotalLoreEntriesCount => _state.LoreEntries.Count;
public ItemProvenanceChain RegisterItem( string itemInstanceId, string? crafterId = null, int craftingDay = 0, string? discoveryLocationId = null, int discoveryDay = 0,
public bool TransferOwnership(string itemInstanceId, string newOwnerId, int day = 1) {
public ItemLoreEntry AddLore( string itemInstanceId, LoreTriggerType trigger, string text, int day, string? survivorId = null,
public ItemProvenanceChain? GetProvenance(string itemInstanceId) {
public IReadOnlyList<ItemLoreEntry> GetLoreEntries(string itemInstanceId) {
public ItemLoreState CaptureState() => CloneState(_state);
public void RestoreState(ItemLoreState state) {
```


# Appendix B.08 — Current Code Architecture: `src/Main.Narrative.cs`

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


# Appendix B.09 — Current Code Architecture: `src/Main.Echoes.cs`

### `src/Main.Echoes.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 207 lines / 7918 bytes.
- SHA-256: `dde04f1959f6117ccc4f4824cca9a3f8aadecbf34fc82e21ab6b5cfffa51e68f`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=4; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
```


# Appendix B.10 — Current Code Architecture: `src/Audio/AudioEventBridge.cs`

### `src/Audio/AudioEventBridge.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 633 lines / 26686 bytes.
- SHA-256: `840935ff49fb41437d3afaf2892e049e4a0f298cc25ae16650b66df0c07139e7`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public interface IAudioDomainProvider
public sealed class AudioEventBridge : IDisposable
public void SubscribeAll( RadiationSystem? radiation = null, WeatherSystem? weather = null, TacticalCombatSystem? combat = null, CraftingSystem? crafting = null, ExpeditionSystem? expeditions = null,
public void BindRadiation(RadiationSystem? radiation) {
public void BindWeather(WeatherSystem? weather) {
public void BindCombat(TacticalCombatSystem? combat) {
public void BindCrafting(CraftingSystem? crafting) {
public void BindExpeditions(ExpeditionSystem? expeditions) {
public void BindDisease(DiseaseSystem? disease) {
public void BindSurvivorFate(SurvivorFateSystem? survivorFate) {
public void BindFlashbacks(SomaticFlashbackSystem? flashbacks) {
public void BindEchoes(EchoSystem? echoes) {
public void NotifyGameFlow(string cueId) {
public void Dispose() {
internal bool HasRadiationBinding => _radiation != null;
internal bool HasWeatherBinding => _weather != null;
internal bool HasCombatBinding => _combat != null;
internal bool HasCraftingBinding => _crafting != null;
internal bool HasExpeditionsBinding => _expeditions != null;
internal bool HasDiseaseBinding => _disease != null;
internal bool HasSurvivorFateBinding => _survivorFate != null;
internal bool HasFlashbacksBinding => _flashbacks != null;
internal bool HasEchoesBinding => _echoes != null;
```


# Appendix B.11 — Current Code Architecture: `src/UI/FactionCommuniqueBoardPanel.cs`

### `src/UI/FactionCommuniqueBoardPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 230 lines / 8821 bytes.
- SHA-256: `3020c0745e442d04f7caa5e3980d65bb12fe048e9461b0a4e2dfe2e96fb9e6ea`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class FactionCommuniqueBoardPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _isBound && _session != null;
public override void _Ready() {
public void Bind(YearOfAshHostSession? session, int campaignDay) {
public void Unbind() {
public void Open() {
public void RefreshView() {
```


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/narrative/survivor_letters_lost_kin.json`

### `Assets/StreamingAssets/Data/narrative/survivor_letters_lost_kin.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 30702 bytes / 30684 characters.
- SHA-256: `b62086786cdcb8e52b7586577ba43706560402839fa309d36fac31665268bd42`.
- Root keys: `collection_id`, `letters`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
letters: min=25, max=25, observed_paths=1
letters[].tags: min=6, max=7, observed_paths=2
```

Representative record fields:

- `author_dweller`
- `destination_address`
- `dispatch_attempt_date`
- `envelope_condition`
- `galina_dead_letter_note`
- `intended_recipient`
- `letter_id`
- `letter_text`
- `pigeonhole_number`
- `tags`

Representative identifiers (ordered, capped for readability):

```text
letter_01_dmitri_to_mother_in_leningrad
letter_02_baker_anna_to_sister_in_odessa
letter_03_little_sonya_to_father_in_kiev
letter_04_master_oleg_to_brother_at_ural_foundry
letter_05_sister_mara_to_mother_superior_in_zagorsk
letter_06_sentry_stepan_to_fiancee_in_smolensk
letter_07_dr_vel_to_colleague_at_moscow_university
letter_08_cook_oxana_to_daughter_in_rostov
letter_09_radio_taras_to_fellow_amateur_operator
letter_10_stoker_nadia_to_husband_in_donbas_mines
letter_11_scout_harlan_to_old_hunting_partner
letter_12_botanist_elena_to_botany_professor_in_yalta
letter_13_ilya_to_grandpa_clockmaker_in_tula
letter_14_librarian_galina_to_director_of_lenin_library
letter_15_valery_to_childhood_sweetheart_in_kursk
letter_16_pavel_the_scout_to_brother_in_vilnius
letter_17_boris_foundryman_to_father_in_chelyabinsk
letter_18_nurse_sonya_to_medical_school_classmate
letter_19_grigory_welder_to_cousin_at_gorky_auto_plant
letter_20_yuri_cobbler_to_wife_in_ryazan
letter_21_sentry_semyon_to_grandmother_in_vologda
letter_22_taras_to_father_shipyard_welder_in_murmansk
letter_23_mikhail_mechanic_to_brother_at_baikonur_cosmodrome
letter_24_little_sonya_to_future_children_of_tessarat
letter_25_the_final_postmasters_general_manifesto
```


# Appendix C.13 — Catalog Census: `Assets/StreamingAssets/Data/final_wishes.json`

### `Assets/StreamingAssets/Data/final_wishes.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 62358 bytes / 62358 characters.
- SHA-256: `5815eedc2697910f8f651f715275fe7af37186904554c5550704eb4731fe32db`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=52, max=52, observed_paths=1
items[].steps: min=2, max=3, observed_paths=2
items[].steps[].required_items: min=1, max=3, observed_paths=3
```

Representative record fields:

- `archetype_id`
- `buff_id`
- `completion_text`
- `id`
- `morale_bonus`
- `steps`
- `wish_description`
- `wish_title`
- `wish_type`

Representative identifiers (ordered, capped for readability):

```text
wish_surgeon_final_surgery
wish_soldier_fallens_wall
wish_nurse_caregivers_legacy
wish_mother_last_letter
wish_mechanic_lost_wrench
wish_teacher_last_period
wish_refugee_one_last_sunrise
wish_electrician_old_voltmeter
wish_pharmacist_compounding_table
wish_plumber_where_the_weight_sits
wish_hunter_reading_the_runs
wish_courier_last_waybill
wish_reporter_attribution_sealed
wish_blind_preacher_words_for_the_stone
wish_misanthrope_grey_water
wish_botanist_overgrown_sill
wish_prisoner_discharged_grudge
wish_defector_broken_signal
wish_undertaker_apart_from_the_wards
wish_pacifist_last_office
wish_watchmaker_final_balance
wish_chef_simmered_root
wish_exhausted_father_porridge_for_the_dawn
wish_hoarder_floorboard_cache
wish_general_sealed_bulkhead
wish_fierce_mother_who_watches_lina
wish_martyr_steady_watch
wish_burglar_put_it_back
wish_historian_iron_cenotaph
wish_quartermaster_keys_and_ledger
wish_pharmacist_unsent_apology
wish_priest_lapsed_parishioner
wish_teacher_former_student
wish_vet_neighbors_pet
wish_undertaker_estranged_daughter
wish_chef_old_rival
wish_surgeon_colleague_reported
wish_mechanic_brother
wish_reporter_old_contact
wish_veteran_old_comrade
wish_pharmacist_ledger
wish_vet_pet_collar
wish_undertaker_register
wish_electrician_soldering_iron
wish_reporter_observatory
wish_teacher_school_gym
wish_veteran_bridge_seven
wish_undertaker_salt_mine
wish_pharmacist_burn_ledger
wish_undertaker_secret_dies
wish_priest_withhold_forgiveness
wish_mechanic_destroy_evidence
```


# Appendix C.14 — Catalog Census: `Assets/StreamingAssets/Data/cassette_sets.json`

### `Assets/StreamingAssets/Data/cassette_sets.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 30666 bytes / 30598 characters.
- SHA-256: `b5073463b9fd938d6f59ebf87ce14aa7cad571e2fc1771aefdfcc8ff9e00c0b7`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=12, max=12, observed_paths=1
items[].hidden_cache_items: min=3, max=4, observed_paths=2
items[].parts: min=3, max=4, observed_paths=2
```

Representative record fields:

- `completion_narrative`
- `hidden_cache_items`
- `hidden_cache_location`
- `parts`
- `set_id`
- `set_title`
- `total_parts`


# Appendix C.15 — Catalog Census: `Assets/StreamingAssets/Data/echoes.json`

### `Assets/StreamingAssets/Data/echoes.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 55507 bytes / 55507 characters.
- SHA-256: `2b8dba790d47b536d2f13cd93446f708ea6029259b5b1153883dc2b49ec989c4`.
- Root keys: `echoes`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
echoes: min=23, max=23, observed_paths=1
echoes[].choices: min=2, max=2, observed_paths=2
echoes[].choices[].effects: min=2, max=3, observed_paths=3
```

Representative record fields:

- `bodyText`
- `choices`
- `conditions`
- `id`
- `minDay`
- `title`
- `weight`

Representative identifiers (ordered, capped for readability):

```text
echo_answering_machine
echo_childs_coat
echo_wedding_ring
echo_frozen_postman
echo_music_box
echo_crayon_drawing
echo_family_dog_collar
echo_unsent_letter
echo_anniversary_calendar
echo_newborn_bracelet
echo_birthday_cake
echo_smoking_pipe
echo_reading_glasses
echo_library_card
echo_post_it_fridge
echo_the_nameplates
echo_unopened_boots
echo_the_frying_pan
echo_school_register
echo_dosimeter_pilgrim
echo_the_receipt
echo_answering_service
echo_the_second_chalk_count
```


# Appendix C.16 — Catalog Census: `Assets/StreamingAssets/Data/faction_war_events.json`

### `Assets/StreamingAssets/Data/faction_war_events.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 93855 bytes / 93786 characters.
- SHA-256: `3ec09e02415a45ee4015120041651756e2cf7ada84fd70701abd5e289ef6e455`.
- Root keys: `chains`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
chains: min=38, max=38, observed_paths=1
chains[].factionsInvolved: min=2, max=2, observed_paths=2
chains[].stages: min=2, max=2, observed_paths=2
chains[].stages[].choices: min=2, max=3, observed_paths=4
```

Representative record fields:

- `band`
- `chainId`
- `factionsInvolved`
- `locationId`
- `stages`
- `title`


# Appendix D.17 — Existing Focused Test Inventory: `Ashfall.Core.Tests/NarrativeAndFactionWarIntegrationTests.cs`

### `Ashfall.Core.Tests/NarrativeAndFactionWarIntegrationTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 184; SHA-256: `351f660e064afb2c329c08465225f7e226b107645ef8e4d860d5c4f9a834ef8e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SurvivorLetterCatalog_LoadsAll25AuthoredLetters
SurvivorLetterDeliverySystem_FollowsFullDeliveryLifecycle
SurvivorLetterDeliverySystem_WithholdAndUnansweredFlows
SurvivorLetterDeliverySystem_SaveRestoreRoundTrip
FactionWarRunner_AppliesStandingDeltasToFactionWarSystem
```


# Appendix D.18 — Existing Focused Test Inventory: `Ashfall.Core.Tests/FinalWishSystemTests.cs`

### `Ashfall.Core.Tests/FinalWishSystemTests.cs`

- Current test declarations: Fact=28, Theory=0, InlineData=0.
- File lines: 408; SHA-256: `cd9d240cb7a27ac0a73038c589efcab859e8395076a9f325c1326eaadf72940e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
DeclareTerminalPrognosis_ActivatesWish
DeclareTerminalPrognosis_RejectsDeadSurvivor
DeclareTerminalPrognosis_RejectsDuplicate
DeclareTerminalPrognosis_FiresEvent
AdvanceWishStep_CompletesDeliverLetter_InTwoSteps
AdvanceWishStep_BuildMemorial_RequiresThreeSteps
AdvanceWishStep_SeeTheSky_CompletesInOneStep
CompleteWish_AppliesMoraleBuff_AndFiresEvent
OnPrognosisExpired_AppliesPenalty_AndFiresEvent
OnPrognosisExpired_DoesNothingIfWishAlreadyCompleted
Tick_DecrementsDaysRemaining
Tick_ExpiresPrognosis_WhenDaysReachZero
Tick_DoesNothingForDeadSurvivor
RegisterWish_OverridesArchetypeMapping
ArchetypePrefix_Surgeon_MapsToTeachLesson
ArchetypePrefix_Parent_MapsToReconcile
SaveLoad_RoundTrips
SaveLoad_DeepCopy_NoSharedReferences
RestoreState_Null_ClearsAll
AdvanceWishStep_RejectsUnknownSurvivor
OnStateChanged_FiresOnDeclare
Catalog_PoolSelection_IsDeterministicForSameSeed
Catalog_DifferentSeeds_CanSelectDifferentWishes
Catalog_NullCatalog_LeavesWishIdEmpty
Catalog_EmptyPool_LeavesWishIdEmpty
Catalog_StepCountHonorsEntry
Catalog_WishId_RoundTripsThroughSave
Catalog_LegacySave_MissingWishId_LoadsGracefully
```


# Appendix D.19 — Existing Focused Test Inventory: `Ashfall.Core.Tests/FactionWarChainRunnerTests.cs`

### `Ashfall.Core.Tests/FactionWarChainRunnerTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 285; SHA-256: `ff244056f1f6fba4691a954d3dd8c732d0ef20546b9e4ff8db8fd44531040c69`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
TriggerTable_HasAnExplicitEntryForEveryStageInTheCatalog
TriggerTable_HasNoEntriesForStageIdsThatDontExist
ChainOpeningStage_DoesNotSurface_UntilLocationVisited
ChainOpeningStage_DoesNotSurface_BeforeMinDay_EvenIfVisited
ResolveChoice_AdvancesToNextStage_AndAppliesMoraleDelta
ResolveChoice_TerminalStage_MarksChainResolved
ResolveChoice_WrongStage_Throws
TickDay_AutoAdvancesZeroChoiceTerminalStages
CrossChainDependency_DoesNotSurface_UntilPriorChainResolved
AndTrigger_RequiresBothConditions_ForwardRosterFirstAction
FanOutChain_AnyVariantPath_TriggersTheSameFollowUpChain
SaveRoundTrip_PreservesProgressVisitsAndMorale
RestoreState_WrongSystemId_Throws
```


# Appendix D.20 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Radio/DistressFollowUpTests.cs`

### `Ashfall.Core.Tests/Radio/DistressFollowUpTests.cs`

- Current test declarations: Fact=21, Theory=1, InlineData=0.
- File lines: 608; SHA-256: `a633b42fb21d127173f362cbb6523dad7363fe76af21922551de0c27d5a5eb6c`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SignalChainsIntoFollowUpAfterInitialContact
RescueSuccessSchedulesFollowUp
FollowUpContentDiffersFromInitial
FollowUpTimingIsDeterministic
FollowUpSurvivesSaveLoad
FollowUpDoesNotFireWhenInitialSignalIgnored
FollowUpDoesNotFireWhenInitialSignalWasTrap
TrapAftermathFollowUpFiresOnAmbushEncountered
NoDuplicateSchedulingOrFiring
FiredFollowUpDoesNotRefireAfterReload
SameDayOrderingIsDeterministic
ResolvedParentCannotRescheduleOldFollowUp
Validator_RejectsUnsupportedTriggerAndNegativeDelay
Validator_RejectsDuplicateFollowUpIdsAcrossCorpus
Validator_AcceptsWellFormedFollowUpsAndRealCatalogsStayClean
Validator_EnforcesDistressFollowUpSemanticRules
FollowUpSignalsBindFromJson
RadioSaveV6RoundTripsFollowUpState
RadioSaveV5MigratesToEmptyFollowUpState
PendingViewProjectionIsDeterministicAndReadOnly
LastFiredProjectionRecordsMostRecentTransmission
StageAndFollowUpSemanticsAreDistinct
```


# Appendix E.21 — Supporting Code Evidence: `src/Host/Phase0HostSession.cs`

### `src/Host/Phase0HostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1095 lines / 56129 bytes.
- SHA-256: `56eab2884bd11ab7fedfa64542e059d3126ad3d3b977bdbca6ceb41f8559a199`.
- Architecture signals: seeded references=6; save/restore symbols=22; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class Phase0SurvivorEffects
public string survivorId = string.Empty;
public float workEfficiencyMultiplier = 1f;
public float workRefusalHours = 0f;
public float staminaMultiplier = 1f;
public float guiltInsomniaSeverity = 0f;
public float hypervigilance = 0f;
public string moralBranch = "Neutral";
public string radiationPhase = "Healthy";
public float dependencyCraftingPenalty = 0f;
public float dependencyCombatPenalty = 0f;
public string finalWishState = string.Empty;
public string finalWishTitle = string.Empty;
public string finalWishDescription = string.Empty;
public float finalWishDaysRemaining;
public int finalWishStepsDone;
public int finalWishStepsTotal;
public string finalWishCompletionText = string.Empty;
public class Phase0EffectsSaveState
public PhaseProgressionSaveState radiationPhase = new PhaseProgressionSaveState();
public PhantomMemoryEngineState phantom = new PhantomMemoryEngineState();
public GuiltInsomniaSaveState guilt = new GuiltInsomniaSaveState();
public CombatTraumaSaveState combatTrauma = new CombatTraumaSaveState();
public SomaticFlashbackSaveState flashbacks = new SomaticFlashbackSaveState();
public MoralBranchingSaveState moral = new MoralBranchingSaveState();
public TradeSpecialtySaveState tradeSpecialty = new TradeSpecialtySaveState();
public FinalWishSaveState finalWishes = new FinalWishSaveState();
public RespiratoryDegenerationState respiratory = new RespiratoryDegenerationState();
public List<Phase0SurvivorEffects> effects = new List<Phase0SurvivorEffects>();
public float permanentShelterMoraleBuff = 0f;
public sealed class Phase0EffectConsumers
public Action<string, float> ApplyMoraleDelta { get; }
public Action<string, float> ApplyHealthDelta { get; }
public Action<string, float> ApplyFatigueDelta { get; }
public Action<float> ApplyShelterMoraleDelta { get; }
public Action<string, float> ApplyWorkEfficiencyMultiplier { get; }
public Action<string, float> ApplyCraftingPenaltyFactor { get; }
public Action<string, float> ApplyCombatPenaltyFactor { get; }
public Action<string, float> ApplyStaminaDrainMultiplier { get; }
public Action<string, string> FireNarrativeEvent { get; }
public Action<string, string> GrantChronicIllness { get; }
public Action<string> ResetRadiationDose { get; }
public Action<string, float> ApplyWorkRefusalHours { get; }
public IReadOnlyList<string> UnboundRequiredEffects => _unboundRequired;
public static Phase0EffectConsumers NoOp( Action<string, float>? applyMoraleDelta = null, Action<string, float>? applyHealthDelta = null, Action<string, float>? applyFatigueDelta = null, Action<float>? applyShelterMoraleDelta = null, Action<string, float>? applyWorkEfficiencyMultiplier = null,
public static readonly Action<string, float> NoOpMoraleDelta = (_, __) => { };
public static readonly Action<string, float> NoOpHealthDelta = (_, __) => { };
public static readonly Action<string, float> NoOpFatigueDelta = (_, __) => { };
public static readonly Action<float> NoOpShelterMoraleDelta = _ => { };
public sealed class Phase0HostSession
public const int DefaultSeed = 808;
public RadiationPhaseProgression RadiationPhase { get; }
public PhantomMemoryEngine Phantom { get; }
public GuiltInsomniaSystem Guilt { get; }
public CombatTraumaSystem CombatTrauma { get; }
public SomaticFlashbackSystem Flashbacks { get; }
public MoralBranchingSystem Moral { get; }
public ChemicalDependencySystem Dependency { get; }
public TradeSpecialtySystem TradeSpecialty { get; }
public FinalWishSystem FinalWish { get; }
public RespiratoryDegenerationSystem Respiratory { get; }
public Phase0EffectConsumers Consumers { get; set; } = Phase0EffectConsumers.NoOp();
public float PermanentShelterMoraleBuff { get; private set; }
public bool IsInAshZone { get; set; }
public bool IsInFalloutStorm { get; set; }
public bool IsNightTime { get; set; }
public int CurrentDay { get; set; } = 1;
public Func<float> GetFilterHealth;
public IReadOnlyList<Phase0SurvivorEffects> Effects => _effects;
public Phase0SurvivorEffects GetEffects(string survivorId) => GetOrCreateEffects(survivorId);
public string LastEvent { get; private set; } = string.Empty;
public void ValidateConsumers() {
public void LoadTradeSpecialties(string dataDir) {
public void LoadPhantomRules(string dataDir) {
public void LoadFinalWishCatalog(string dataDir) {
public void RegisterDefaultRules() {
public void RegisterSurvivors(IEnumerable<string> ids) {
public void SeedDemoRoster() {
public string ScavengeItem(string survivorId, string itemId) {
public string RaiseNoise(string survivorId) {
public string CraftItem(string survivorId, string professionId, string itemId) {
public string RecordMoralChoice(string survivorId, bool isEmpathyChoice) {
public string RecordGuilt(string survivorId, string sourceId, float severity) {
public string RegisterCombatSurvived(string survivorId) {
public string ConsumeSubstance(string survivorId, string itemId, ChemicalDependencyKind kind) {
public string DeclareTerminalPrognosis(string survivorId, string archetypeId) {
public string AdvanceFinalWish(string survivorId, string stepId) {
public string ApplyInhaler(string survivorId) {
public string TickHour(float gameHours = 1f) {
public string TickDay(int day) {
public string StatusLine() {
public Phase0EffectsSaveState CaptureSave() {
public void RestoreSave(Phase0EffectsSaveState save) {
public int Seed { get; }
public int Next(int min, int max) => _rng.Next(min, max);
public float NextFloat() => _rng.NextFloat();
public double NextDouble() => _rng.NextDouble();
public void BindShelterAssignment(ShelterAssignmentSystem shelterAssignment) {
protected override void UnsubscribeSystemEvents() {
```


# Appendix E.22 — Supporting Code Evidence: `Assets/Ashfall.Core/Narrative/PersonalLetterCatalog.cs`

### `Assets/Ashfall.Core/Narrative/PersonalLetterCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 266 lines / 9185 bytes.
- SHA-256: `e738bfc33612ec3b1adafd8e0b04819fd3f97492235f1535c072ff25c147a800`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PersonalLetterEntry
public string letter_id;
public int day;
public string sender;
public string recipient;
public string location;
public string letter_type;
public string tone;
public string[] key_themes;
public string content;
public string[] cross_refs;
public string[] tags;
public sealed class PersonalLetterFile
public int schema_version;
public string collection_id;
public string description;
public List<PersonalLetterEntry> letters = new List<PersonalLetterEntry>();
public sealed class PersonalLetterCatalog
public IReadOnlyList<PersonalLetterEntry> AllLetters => _allLetters;
public void Load(string json, IJsonSerializer serializer) {
public void Clear() {
public static PersonalLetterCatalog LoadFromDirectory(string dataDir, IFileIO fileIo, IJsonSerializer serializer) {
public PersonalLetterEntry? GetById(string letterId) {
public List<PersonalLetterEntry> GetByType(string letterType) {
public List<PersonalLetterEntry> GetBySender(string senderSnippet) {
public List<PersonalLetterEntry> GetByRecipient(string recipientSnippet) {
public List<PersonalLetterEntry> GetByTag(string tag) {
public List<PersonalLetterEntry> GetByTheme(string theme) {
public List<PersonalLetterEntry> GetBySearch(string query) {
public List<PersonalLetterEntry> GetSortedByDay(bool ascending = true) {
```


# Appendix E.23 — Supporting Code Evidence: `Assets/Ashfall.Core/Survivors/FinalWishCatalog.cs`

### `Assets/Ashfall.Core/Survivors/FinalWishCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 114 lines / 4699 bytes.
- SHA-256: `114e58bdbc9426d83970b9b5374cae5fde9c549d176674eefed28053f55bde2d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FinalWishEntry
public string id = string.Empty;
public string archetype_id = string.Empty;
public string wish_type = string.Empty;
public string wish_title = string.Empty;
public string wish_description = string.Empty;
public List<FinalWishStep> steps = new List<FinalWishStep>();
public string completion_text = string.Empty;
public float morale_bonus;
public string buff_id = string.Empty;
public sealed class FinalWishStep
public string step_id = string.Empty;
public string description = string.Empty;
public List<string> required_items = new List<string>();
public string requires_location = string.Empty;
public bool requires_patient;
public sealed class FinalWishContainer
public int schema_version = 1;
public List<FinalWishEntry> items = new List<FinalWishEntry>();
public interface IFinalWishCatalog
public sealed class FinalWishCatalog : IFinalWishCatalog
public int Count => _byId.Count;
public void Add(FinalWishEntry entry) {
public IReadOnlyList<string> GetWishIdsForArchetype(string archetypeId) {
public FinalWishEntry? GetEntry(string wishId) {
```


# Appendix E.24 — Supporting Code Evidence: `Assets/Ashfall.Core/Narrative/SurvivorLetterCatalog.cs`

### `Assets/Ashfall.Core/Narrative/SurvivorLetterCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 119 lines / 4059 bytes.
- SHA-256: `fe62c907c2912d1fa26170ef89dd5c3a05aed31f53057fc2bc131caa427ccf41`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SurvivorLetterEntry
public string letter_id;
public string pigeonhole_number;
public string author_dweller;
public string intended_recipient;
public string destination_address;
public string dispatch_attempt_date;
public string envelope_condition;
public string letter_text;
public string galina_dead_letter_note;
public string[] tags;
public sealed class SurvivorLetterFile
public int schema_version;
public string collection_id;
public List<SurvivorLetterEntry> letters = new List<SurvivorLetterEntry>();
public sealed class SurvivorLetterCatalog
public IReadOnlyList<SurvivorLetterEntry> AllLetters => _allLetters;
public void Load(string json, IJsonSerializer serializer) {
public SurvivorLetterEntry? GetById(string letterId) {
public List<SurvivorLetterEntry> GetByAuthor(string authorSnippet) {
public List<SurvivorLetterEntry> GetByDestination(string destinationSnippet) {
public List<SurvivorLetterEntry> GetByTag(string tag) {
```


# Appendix E.25 — Supporting Code Evidence: `Assets/Ashfall.Core/Narrative/PersonalLetterProjection.cs`

### `Assets/Ashfall.Core/Narrative/PersonalLetterProjection.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 153 lines / 7519 bytes.
- SHA-256: `fea47b5e61450bdfa2e5c32dd84292569fa3c1c6bd5ac372b6b39fa6bb9cd368`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum LetterTruthClass
public static class PersonalLetterProjection
public static string ResolveRoomForLetter(string letterId) {
public static LetterTruthClass ResolveTruthClass(string letterId) {
public static List<PersonalLetterEntry> GetLettersForRoom(PersonalLetterCatalog catalog, string roomId) {
public static List<PersonalLetterEntry> GetLettersForTruthClass(PersonalLetterCatalog catalog, LetterTruthClass truthClass) {
public static List<PersonalLetterEntry> GetLettersForType(PersonalLetterCatalog catalog, string letterType) {
```


# Appendix G.26 — Supporting Regression Evidence: `Ashfall.Core.Tests/PersonalLetterCatalogTests.cs`

### `Ashfall.Core.Tests/PersonalLetterCatalogTests.cs`

- Current test declarations: Fact=12, Theory=0, InlineData=0.
- File lines: 337; SHA-256: `dd30a3e629e7ec64ce5d70d6fbe8e9d35b6a81e31a2db77a05756a6ea36041a3`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
TEST_LTR_01_PersonalLetters_LoadsAll25AuthoredLetters
TEST_LTR_02_PersonalLetters_AllEntriesHaveValidFieldsAndUniqueIds
TEST_LTR_03_PersonalLetters_Load_IsIdempotent
TEST_LTR_04_PersonalLetters_LoadFromDirectory_ResolvesCanonicalFile
TEST_LTR_05_PersonalLetters_GetByType_FiltersDeliveredAndUnsent
TEST_LTR_06_PersonalLetters_GetBySenderAndRecipient_CaseInsensitive
TEST_LTR_07_PersonalLetters_GetBySearch_FindsKeywords
TEST_LTR_08_PersonalLetters_Projection_MapsCanonicalRooms
TEST_LTR_09_PersonalLetters_Projection_ClassifiesTruthAndProvenance
TEST_LTR_10_PersonalLetters_Projection_ZeroMutationContract
TEST_LTR_11_PersonalLetters_DeterministicOrdering
TEST_LTR_12_PersonalLetters_Clear_ResetsCatalog
```


# Appendix G.27 — Supporting Regression Evidence: `Ashfall.Core.Tests/FinalWishPlan65CatalogTests.cs`

### `Ashfall.Core.Tests/FinalWishPlan65CatalogTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 400; SHA-256: `835a6253d59311e4741c6a84c2f196c5a3866a16429d1c159282740c8d71b8eb`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsAndHasExactly52Wishes
Catalog_WishTypeDistribution_MatchesPlan65Specification
Catalog_AllIdsTitlesAndFields_AreUniqueAndValid
Catalog_AllCrossReferences_ResolveInCanonicalCatalogs
FinalWishSystem_All10WishTypes_ProgressAndCompleteDeterministically
FinalWishSystem_DeterministicSelection_HoldsUnderSeed
FinalWishSystem_SaveLoad_FullRoundTrip_PreservesAllState
```


# Appendix G.28 — Supporting Regression Evidence: `Ashfall.Core.Tests/Narrative/EchoSystemTests.cs`

### `Ashfall.Core.Tests/Narrative/EchoSystemTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 111; SHA-256: `d29e1c82e89a76bcc1c609967854dac4f7d010d120576755c3c8ac418ca2dd74`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
DayAndFlagBoundariesControlAvailability
SameSeedProducesTheSameSelection
ResolutionIsExactlyOnceAndRestoreDoesNotReplay
DelayedConsequenceSurvivesRestoreAndTicksExactlyOnce
```


# Appendix G.29 — Supporting Regression Evidence: `Ashfall.Core.Tests/PersonalLetterRuntimeActivationTests.cs`

### `Ashfall.Core.Tests/PersonalLetterRuntimeActivationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 98; SHA-256: `a621e6e9939cd0a031ca1bc5ae148a3f6b0eed5f06e3dc4268be1c690a7d7edf`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SourceAllowlist_MatchesLetterCatalogsOnly
DiscoveryProjection_LoadsTwentySixLetterRecords
ProducerDiscovery_UnlocksMatchingRoomLetters
LetterDiscovery_IsIdempotentAcrossSaveRestore
```


# Appendix H.30 — Supporting Authority Document: `docs/architecture/LETTER_TRUTH_PROVENANCE_MATRIX.md`

### `docs/architecture/LETTER_TRUTH_PROVENANCE_MATRIX.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 45 lines / 5809 bytes.
- SHA-256: `37b8da320836592c5052c88f06f3723e1dc9b0a46c47b3840526ea5c3dc0ffe7`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| letter discovery, addressing and delivery state | Survivor letter owner | echo choices and delayed consequences | EchoSystem | Owner emits/reads a typed fact; no mirror state. |
| letter discovery, addressing and delivery state | Survivor letter owner | parts, playback, completion and cache disclosure | CassettePlaybackSystem | Owner emits/reads a typed fact; no mirror state. |
| letter discovery, addressing and delivery state | Survivor letter owner | authored war chain progression | FactionWarChainRunner | Owner emits/reads a typed fact; no mirror state. |
| letter discovery, addressing and delivery state | Survivor letter owner | end-of-life wishes | FinalWishSystem | Owner emits/reads a typed fact; no mirror state. |
| echo choices and delayed consequences | EchoSystem | letter discovery, addressing and delivery state | Survivor letter owner | Owner emits/reads a typed fact; no mirror state. |
| echo choices and delayed consequences | EchoSystem | parts, playback, completion and cache disclosure | CassettePlaybackSystem | Owner emits/reads a typed fact; no mirror state. |
| echo choices and delayed consequences | EchoSystem | authored war chain progression | FactionWarChainRunner | Owner emits/reads a typed fact; no mirror state. |
| echo choices and delayed consequences | EchoSystem | end-of-life wishes | FinalWishSystem | Owner emits/reads a typed fact; no mirror state. |
| parts, playback, completion and cache disclosure | CassettePlaybackSystem | letter discovery, addressing and delivery state | Survivor letter owner | Owner emits/reads a typed fact; no mirror state. |
| parts, playback, completion and cache disclosure | CassettePlaybackSystem | echo choices and delayed consequences | EchoSystem | Owner emits/reads a typed fact; no mirror state. |
| parts, playback, completion and cache disclosure | CassettePlaybackSystem | authored war chain progression | FactionWarChainRunner | Owner emits/reads a typed fact; no mirror state. |
| parts, playback, completion and cache disclosure | CassettePlaybackSystem | end-of-life wishes | FinalWishSystem | Owner emits/reads a typed fact; no mirror state. |
| authored war chain progression | FactionWarChainRunner | letter discovery, addressing and delivery state | Survivor letter owner | Owner emits/reads a typed fact; no mirror state. |
| authored war chain progression | FactionWarChainRunner | echo choices and delayed consequences | EchoSystem | Owner emits/reads a typed fact; no mirror state. |
| authored war chain progression | FactionWarChainRunner | parts, playback, completion and cache disclosure | CassettePlaybackSystem | Owner emits/reads a typed fact; no mirror state. |
| authored war chain progression | FactionWarChainRunner | end-of-life wishes | FinalWishSystem | Owner emits/reads a typed fact; no mirror state. |
| end-of-life wishes | FinalWishSystem | letter discovery, addressing and delivery state | Survivor letter owner | Owner emits/reads a typed fact; no mirror state. |
| end-of-life wishes | FinalWishSystem | echo choices and delayed consequences | EchoSystem | Owner emits/reads a typed fact; no mirror state. |
| end-of-life wishes | FinalWishSystem | parts, playback, completion and cache disclosure | CassettePlaybackSystem | Owner emits/reads a typed fact; no mirror state. |
| end-of-life wishes | FinalWishSystem | authored war chain progression | FactionWarChainRunner | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Audit current letter/echo/cassette/war player surfaces and close only proven reachability gaps. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Unify exposition around owner-specific read models without copying mutable state. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Add one cross-canon trace matrix linking item, survivor, location, flag, radio, journal and memorial consumers. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Preserve all sealed radio and final-wish boundaries. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/FinalWishCatalogLoader.cs`

### `Assets/Ashfall.Core/Survivors/FinalWishCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 66 lines / 2476 bytes.
- SHA-256: `e84f8d79b22a5ca1f2e2283a6d0672f70d0f36b7396a12d8616539d1777e577e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class FinalWishCatalogLoader
public const string FileName = "final_wishes.json";
public static FinalWishCatalog LoadCatalog(string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs`

### `Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 371 lines / 17304 bytes.
- SHA-256: `bcfc35c7049581fe1513883dda1e95adfc06458f1b8a10eb520ff4ca1e54fc5d`.
- Architecture signals: seeded references=0; save/restore symbols=20; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class YearOfAshSave
public const int CurrentSaveVersion = 5;
public int saveVersion = CurrentSaveVersion;
public int simDay = 180;
public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
public FactionWarSystemState factionWar = new FactionWarSystemState();
public WarlordDoctrineState warlord = new WarlordDoctrineState();
public YearOfAshDeepFreezeState deepFreeze = new YearOfAshDeepFreezeState();
public YearOfAshRadonState radon = new YearOfAshRadonState();
public QuestlineSystemState quests = new QuestlineSystemState();
public FactionWarChainRunnerState factionWarChainRunner = new FactionWarChainRunnerState();
public IceRoadState iceRoad = new IceRoadState();
public string Checksum = string.Empty;
public class YearOfAshSaveV4
public int saveVersion = 4;
public int simDay = 180;
public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
public FactionWarSystemState factionWar = new FactionWarSystemState();
public WarlordDoctrineState warlord = new WarlordDoctrineState();
public YearOfAshDeepFreezeState deepFreeze = new YearOfAshDeepFreezeState();
public YearOfAshRadonState radon = new YearOfAshRadonState();
public QuestlineSystemState quests = new QuestlineSystemState();
public FactionWarChainRunnerState factionWarChainRunner = new FactionWarChainRunnerState();
public string Checksum = string.Empty;
public class YearOfAshSaveV1
public int saveVersion = 1;
public int simDay = 180;
public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
public FactionWarSystemState factionWar = new FactionWarSystemState();
public string Checksum = string.Empty;
public class YearOfAshSaveV2
public int saveVersion = 2;
public int simDay = 180;
public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
public FactionWarSystemState factionWar = new FactionWarSystemState();
public YearOfAshDeepFreezeState deepFreeze = new YearOfAshDeepFreezeState();
public YearOfAshRadonState radon = new YearOfAshRadonState();
public QuestlineSystemState quests = new QuestlineSystemState();
public string Checksum = string.Empty;
public class YearOfAshSaveV3
public int saveVersion = 3;
public int simDay = 180;
public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
public FactionWarSystemState factionWar = new FactionWarSystemState();
public WarlordDoctrineState warlord = new WarlordDoctrineState();
public YearOfAshDeepFreezeState deepFreeze = new YearOfAshDeepFreezeState();
public YearOfAshRadonState radon = new YearOfAshRadonState();
public QuestlineSystemState quests = new QuestlineSystemState();
public string Checksum = string.Empty;
public static class YearOfAshSaveCodec
public static YearOfAshSave Capture( YearOfAshTimelineSystem timeline, DoorEncounterSystem encounters, FactionWarSystem factionWar, IClock clock, YearOfAshDeepFreezeSystem? deepFreeze = null,
public static void Restore( YearOfAshSave save, YearOfAshTimelineSystem timeline, DoorEncounterSystem encounters, FactionWarSystem factionWar, YearOfAshDeepFreezeSystem? deepFreeze = null,
public static string Encode(YearOfAshSave save, IJsonSerializer json) {
public static YearOfAshSave Decode(string jsonText, IJsonSerializer json) {
```


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/EchoSystem.cs`

### `Assets/Ashfall.Core/Narrative/EchoSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 397 lines / 16479 bytes.
- SHA-256: `8319b50f274bc92f68fe58e62d43dca5c0bc4639392b76098b7907885975ec3c`.
- Architecture signals: seeded references=1; save/restore symbols=3; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class EchoState
public string PendingEchoId = string.Empty;
public int PendingDay;
public List<string> ResolvedEchoIds = new List<string>();
public List<string> ResolvedChoiceIds = new List<string>();
public List<EchoPendingConsequenceState> PendingConsequences = new List<EchoPendingConsequenceState>();
public sealed class EchoPendingConsequenceState
public string EchoId = string.Empty;
public string ChoiceId = string.Empty;
public int DueDay;
public enum EchoResolutionStatus
public sealed class EchoResolutionResult
public EchoResolutionStatus Status { get; internal set; }
public string EchoId { get; internal set; } = string.Empty;
public string ChoiceId { get; internal set; } = string.Empty;
public string Reason { get; internal set; } = string.Empty;
public EchoDefinition? Echo { get; internal set; }
public EchoChoiceDefinition? Choice { get; internal set; }
public double MoraleDelta { get; internal set; }
public bool Succeeded =>
public sealed class EchoDelayedConsequenceResult
public string EchoId { get; internal set; } = string.Empty;
public string ChoiceId { get; internal set; } = string.Empty;
public EchoDelayedConsequence Consequence { get; internal set; } = new EchoDelayedConsequence();
public int DueDay { get; internal set; }
public sealed class EchoSystem
public const string SystemId = "echo_system";
public const string CatalogFileName = EchoCatalogLoader.FileName;
public event Action<EchoDefinition>? OnEchoSurfaced;
public event Action<EchoResolutionResult>? OnEchoResolved;
public event Action<EchoDelayedConsequenceResult>? OnDelayedConsequenceDue;
public event Action<EchoState>? OnStateChanged;
public ContentUtilizationInstrumentation? Instrumentation { get; set; }
public Func<string, bool> HasWorldFlag { get; set; } = _ => false;
public EchoState State => _state;
public IReadOnlyList<EchoDefinition> Catalog => _catalog;
public EchoDefinition? PendingEcho => Find(_state.PendingEchoId);
public bool HasPendingEcho => PendingEcho != null;
public bool HasPersistedState =>
public void RegisterRange(IEnumerable<EchoDefinition>? definitions) {
public EchoDefinition? Find(string echoId) {
public IReadOnlyList<EchoDefinition> GetEligibleCandidates(int day) {
public bool IsAvailable(EchoDefinition definition, int day) {
public EchoDefinition? SelectForDay(int day, ISeededRng rng) {
public EchoResolutionResult CanResolve(string echoId, string choiceId, int day) {
public EchoResolutionResult Resolve(string echoId, string choiceId, int day) {
public IReadOnlyList<EchoDelayedConsequenceResult> TickDay(int day) {
public bool IsResolved(string echoId) {
public EchoState CaptureState() => CloneState(_state);
public void RestoreState(EchoState? saved) {
```


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix Q.567 — Additional Current Architecture Evidence: `src/UI/Phase0Panel.cs`

### `src/UI/Phase0Panel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 338 lines / 15668 bytes.
- SHA-256: `0bef3106cde142175b083fd32dcdc96059aa3bfffb4c3f27d75318f646ffdb8d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Phase0Panel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _phase0 != null;
public int RenderedConditionCount => _conditionList?.GetChildCount() ?? 0;
public override void _Ready() {
public void Bind(Phase0HostSession phase0, SurvivorsHostSession? survivors = null, MedicalPipelineCoordinator? pipeline = null) {
public void RefreshView() {
public void Open() {
public void Unbind() {
public override void _ExitTree() {
internal static class Phase0PanelLabelExtensions
public static Label? WithColor(this Label label, Color color) {
```


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/PalliativeCareDignityEngine.cs`

### `Assets/Ashfall.Core/Medical/PalliativeCareDignityEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 255 lines / 10383 bytes.
- SHA-256: `d4091631c458e75e83d296a7f05a3840004870baec946886e1d16e6852244db1`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum GriefStage
public enum PalliativeCareProtocol
public sealed class PalliativePatientRecord
public string SurvivorId { get; set; } = string.Empty;
public int DaysRemainingPrognosis { get; set; } = 7;
public int PainLevelPermille { get; set; } = 600; // 0..1000
public int LucidityPermille { get; set; } = 800;  // 0..1000
public int DignityIndexPermille { get; set; } = 750; // 0..1000
public PalliativeCareProtocol ActiveProtocol { get; set; } = PalliativeCareProtocol.BalancedAnalgesia;
public string FinalWishQuestId { get; set; } = string.Empty;
public bool FinalWishFulfilled { get; set; }
public GriefStage CurrentGriefStage { get; set; } = GriefStage.Denial;
public int DaysInCurrentGriefStage { get; set; }
public PalliativePatientRecord Clone() => new PalliativePatientRecord
public readonly struct DailyPalliativeOutcome
public int PainDeltaPermille { get; }
public int LucidityDeltaPermille { get; }
public int DignityDeltaPermille { get; }
public bool PrognosisExpired { get; }
public readonly struct MemorialLegacyEcho
public string SurvivorId { get; }
public int MoraleDelta { get; }
public string MemorialJournalKey { get; }
public bool DiedInDignity { get; }
public static class PalliativeCareDignityEngine
public const int HighDignityThresholdPermille = 700;
public static DailyPalliativeOutcome AdvanceDailyCare( PalliativePatientRecord patient, int medicineAvailabilityPermille, int caregiverSkillPermille) {
public static void EvaluateGriefStageProgression( PalliativePatientRecord patient, long simTick, int worldSeed) {
public static MemorialLegacyEcho CalculateMemorialEcho(PalliativePatientRecord deceased) {
```


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Memorial/MemorialSystem.cs`

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


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/MemorialComponentStore.cs`

### `Assets/Ashfall.Core/Survivors/MemorialComponentStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 300 lines / 11785 bytes.
- SHA-256: `27865d26b30d1f5fa997917f702d0c83be04ded4237783b6a30e32655b59e83b`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MemorialRecordState
public string survivor_id = string.Empty;
public string cause = string.Empty;
public int day;
public int survived_days;
public bool final_wish_resolved;
public string epitaph = string.Empty;
public string heirloom_item_id = string.Empty;
public string heirloom_recipient_id = string.Empty;
public float morale_delta;
internal MemorialRecord ToRecord(SurvivorId owner) => new MemorialRecord(
internal static MemorialRecordState Capture(MemorialRecord source) => new MemorialRecordState
public sealed class MemorialComponentStoreState
public const string CurrentSystemId = MemorialComponentStore.SystemId;
public const int CurrentSchemaVersion = MemorialComponentStore.SchemaVersion;
public int schema_version = CurrentSchemaVersion;
public string system_id = CurrentSystemId;
public List<MemorialRecordState> records = new List<MemorialRecordState>();
public sealed class MemorialComponentRestoreReport
public int Accepted { get; internal set; }
public List<string> Rejected { get; } = new List<string>();
public bool IsFatal { get; internal set; }
public string FatalReason { get; internal set; } = string.Empty;
public bool IsClean => !IsFatal && Rejected.Count == 0;
public override string ToString() => IsFatal
public sealed class MemorialRecord
public SurvivorId SurvivorId { get; }
public SurvivorId OwnerId => SurvivorId;
public string Cause { get; }
public int Day { get; }
public int SurvivedDays { get; }
public bool FinalWishResolved { get; }
public string Epitaph { get; }
public string HeirloomItemId { get; }
public string HeirloomRecipientId { get; }
public float MoraleDelta { get; }
public sealed class MemorialComponentStore : ISurvivorComponentStore
public const string SystemId = "memorial_component";
public const int SchemaVersion = 1;
public string ComponentName => "memorial";
public SurvivorComponentCardinality Cardinality => SurvivorComponentCardinality.ZeroOrOne;
public bool RetainsHistoryAfterDeath => true;
public IEnumerable<SurvivorId> OwnerIds => OrderedOwnerIds();
public int Count => _byOwner.Count;
public bool Contains(SurvivorId owner) => !owner.IsEmpty && _byOwner.ContainsKey(owner);
public bool TryGet(SurvivorId owner, out MemorialRecord? record) {
public MemorialRecord Record(MemorialRecord record) {
public bool TryRecord(MemorialRecord record) {
public bool Release(SurvivorId owner) => false;
public void Reset() => _byOwner.Clear();
public MemorialComponentStoreState CaptureState() {
public MemorialComponentRestoreReport RestoreState(MemorialComponentStoreState? saved) {
```


# Appendix Q.571 — Additional Current Architecture Evidence: `src/Main.ShelterInfrastructure.cs`

### `src/Main.ShelterInfrastructure.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 645 lines / 31304 bytes.
- SHA-256: `5ec59e6a0a93e8e5c67bc7a39c91ff4cca74a518400fd09468938167c23630ea`.
- Architecture signals: seeded references=6; save/restore symbols=16; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public Ashfall.Core.Shelter.ShelterFireHazardSystem ShelterFireHazard => GetShelterFireHazardSystem();
public ShelterFireHostSession? ShelterFireSession => _shelterFireSession;
public Ashfall.Core.Narrative.BunkerGraffitiCatalog GetBunkerGraffitiCatalog() {
public Ashfall.Core.Narrative.BunkerCourtCatalog GetBunkerCourtCatalog() {
public Ashfall.Core.Narrative.BunkerMaintenanceCatalog GetBunkerMaintenanceCatalog() {
public Ashfall.Core.Narrative.PersonalLetterCatalog GetPersonalLetterCatalog() {
public Ashfall.Core.Narrative.AbyssalAnomaliesCatalog GetAbyssalAnomaliesCatalog() {
public string BuildMachineTellText(ISeededRng? rng = null) {
public Ashfall.Core.Shelter.ShelterFireHazardSystem GetShelterFireHazardSystem() {
```


# Appendix Q.572 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/NarrativeDiscoveryCatalog.cs`

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


# Appendix Q.573 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Muster/FactionEcologyHeadlessDemo.cs`

### `Assets/Ashfall.Core/Muster/FactionEcologyHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 198 lines / 11351 bytes.
- SHA-256: `bbb53742f4a2a55e6817a118db2b3dbdfff9ae1571a900006694e4f7a1588d15`.
- Architecture signals: seeded references=0; save/restore symbols=9; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FactionEcologyHeadlessReport : HeadlessReport
public FactionActionBoardState Board;
public string MusterPath;
public static class FactionEcologyHeadlessDemo
public static FactionEcologyHeadlessReport Run(string? dataDirectory = null, ILog? log = null) {
public bool IsFlagSet(string flagId) => _isFlagSet(flagId);
public bool IsSubjectAlive(string subjectId) => true;
public bool IsFactionPresent(string factionId) => true;
```


# Appendix Q.574 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/MemorialComponentParity.cs`

### `Assets/Ashfall.Core/Survivors/MemorialComponentParity.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 387 lines / 15460 bytes.
- SHA-256: `f03db0515f61d1e3c861a54bff66d264f3075bd12fe586dc0d8397c5e42ccb1f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class MemorialParityCode
public const string LegacyRowNull = "legacy_row_null";
public const string LegacyIdInvalid = "legacy_id_invalid";
public const string LegacyDuplicateId = "legacy_duplicate_id";
public const string LegacyFieldNull = "legacy_field_null";
public const string TypedRecordMissing = "typed_record_missing";
public const string TypedRecordExtra = "typed_record_extra";
public const string FieldMismatch = "field_mismatch";
public sealed class MemorialParityFinding
public string Code { get; }
public SurvivorId SurvivorId { get; }
public string RawId { get; }
public string Field { get; }
public string Expected { get; }
public string Actual { get; }
public string Message { get; }
public override string ToString() {
public sealed class MemorialParityReport
public int LegacyRows { get; internal set; }
public int TypedRows { get; internal set; }
public List<MemorialParityFinding> Findings { get; } = new List<MemorialParityFinding>();
public bool IsMatch => Findings.Count == 0;
public int FindingCount => Findings.Count;
public string Describe() {
public override string ToString() => $"[MemorialParity] legacy={LegacyRows} typed={TypedRows} findings={Findings.Count}";
public static class MemorialComponentParity
public static MemorialParityReport Compare( IReadOnlyList<MemorialEntry> legacyEntries, MemorialComponentStore typed) {
```


# Appendix Q.575 — Additional Current Architecture Evidence: `src/Host/HostCli.PanelTests.cs`

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


# Appendix Q.576 — Additional Current Architecture Evidence: `src/YearOfAsh/YearOfAshHostSession.cs`

### `src/YearOfAsh/YearOfAshHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 345 lines / 16205 bytes.
- SHA-256: `ccefd687193231434f67fa00f2a465d6a8558f7d6c3c7748f94a35fb28e4b6d2`.
- Architecture signals: seeded references=2; save/restore symbols=3; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class YearOfAshHostSession
public YearOfAshTimelineSystem Timeline => _timeline;
public DoorEncounterSystem Encounters => _encounters;
public FactionWarSystem FactionWar => _factionWar;
public QuestlineSystem Quests => _quests;
public YearOfAshDeepFreezeSystem DeepFreeze => _deepFreeze;
public YearOfAshRadonSystem Radon => _radon;
public WarlordDoctrineSystem Warlord => _warlord;
public FactionWarChainRunner WarRunner => _warRunner;
public IReadOnlyList<SurvivorOccupantSnapshot> DemoRoster => _demoRoster;
public void BindWarlord(WarlordDoctrineSystem warlord) {
public static YearOfAshHostSession Create(string dataDir = "", bool loadExistingSave = true) {
public void TickDay(int day) {
public void RecordWarLocationVisited(string locationId) {
public void ResolveWarChoice(string chainId, string stageId, string choiceId, int currentDay) {
public string GetStatusSummary() {
public int CurrentTributeAsk =>
public bool SettleWarlordTribute(int amountPaid, int day, out int nextAsk) {
public string CollectorLine(string state, int day) => _warlord.Catalog.CollectorLine(state, day);
public string WarlordLine() {
public YearOfAshSave CaptureSave() {
public void RestoreSave(YearOfAshSave save) {
```


# Appendix Q.577 — Additional Current Architecture Evidence: `src/Host/EchoHostSession.cs`

### `src/Host/EchoHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 85 lines / 3056 bytes.
- SHA-256: `b63c17d1242b3e34ca61646c4828bf6e82fb82050e924c5f21d69e13ea471669`.
- Architecture signals: seeded references=1; save/restore symbols=5; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class EchoHostSession : HostSessionBase
public EchoSystem Engine { get; }
public EchoDefinition? PendingEcho => Engine.PendingEcho;
public string LastEvent { get; private set; } = string.Empty;
public static EchoHostSession Create(string dataDir) {
public void ConfigureFlags(IFlagLedger? flags) {
public EchoDefinition? SelectForDay(int day, ISeededRng rng) => Engine.SelectForDay(day, rng);
public EchoResolutionResult Resolve(string echoId, string choiceId, int day) => Engine.Resolve(echoId, choiceId, day);
public System.Collections.Generic.IReadOnlyList<EchoDelayedConsequenceResult> TickDay(int day) => Engine.TickDay(day);
public EchoState CaptureSave() => Engine.CaptureState();
public void RestoreSave(EchoState state) => Engine.RestoreState(state);
```


# Appendix Q.578 — Additional Current Architecture Evidence: `src/Main.ContentCertification.cs`

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


# Appendix Q.579 — Additional Current Architecture Evidence: `src/Main.SurvivorFate.cs`

### `src/Main.SurvivorFate.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 161 lines / 7381 bytes.
- SHA-256: `c803f32ac907ee552c276ee1c31fbd53066c269a8047bafd15d68832b8280b3c`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=4; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public void ReportScriptedDeath(string survivorId, string narrativeReason) {
```


# Appendix Q.580 — Additional Current Architecture Evidence: `src/Host/ContentCertificationHostSession.cs`

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


# Appendix R.581 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Survivors/FinalWishCatalogLoaderTests.cs`

### `Ashfall.Core.Tests/Survivors/FinalWishCatalogLoaderTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 182; SHA-256: `4645bf8ed152909c0a17852f1c2b96e0eb92d93d21e589ad8cc9424f7af95a93`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
LoadsAtLeastThirtyEntries
AllWishIdsAreUniqueAndPrefixed
EveryArchetypeHasAtLeastOneWish
AllRequiresLocation_ResolveAgainstLocationsJson
AllPrefixedRequiredItems_ResolveAgainstItemsJson
LoadCatalog_MissingFile_ReturnsEmptyCatalog
GetEntry_UnknownId_ReturnsNull
```


# Appendix R.582 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/SurvivorLetterCatalogTests.cs`

### `Ashfall.Core.Tests/SurvivorLetterCatalogTests.cs`

- Current test declarations: Fact=2, Theory=0, InlineData=0.
- File lines: 94; SHA-256: `10bfd924abb48e51a22c4e09114fc5a5af5e0baa3f2f7ad0ee26f745a038dca5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SurvivorLetters_LoadsAll25CanonicalLetters
SurvivorLetters_AllEntriesHaveValidFieldsAndUniquePigeonholes
```


# Appendix R.583 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/SurvivorFateSystemTests.cs`

### `Ashfall.Core.Tests/SurvivorFateSystemTests.cs`

- Current test declarations: Fact=17, Theory=1, InlineData=8.
- File lines: 421; SHA-256: `756580478b0fa34685fd19c8903af28e3db98f0957463d870e1c1e16b6514836`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
EveryCause_ProducesOneCompleteCascade
DuplicateReport_IsIdempotent_NoDuplicateSideEffects
MultiCauseSameSurvivor_OnlyFirstCascadeRuns
Death_ClearsDutyCaregivingMedicalAndExpeditionAssignments
Death_OfCaregiver_ClearsTheirCaregivingLane
Death_RecallsActiveExpedition
Death_OfLeader_UpdatesLeadershipStressAndClearsLeader
Death_WithCompletedWish_MemorializedAsResolved
Death_WithActiveWish_FailsItAndMemorializedUnresolved
LastSurvivorDeath_RaisesOnLastSurvivorDied
NoRosterLane_LastSurvivorNeverFires
CaptureRestore_BetweenDeaths_PreservesIdempotency
CaptureState_IsDeterministicallyOrdered
ReconcileFromRoster_SynthesizesFatesForLegacyDead
PlayerAvatarDeath_IsFlaggedOnRecord
DrainDayEvents_EmptiesBuffer_NoDuplicatesOnSecondDrain
ReportDeath_NullOrBlankId_Throws
LanelessSystem_StillRecordsFate
```


# Appendix R.584 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Campaign/Plan47_65ModAgencyIntegrationTests.cs`

### `Ashfall.Core.Tests/Campaign/Plan47_65ModAgencyIntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 264; SHA-256: `116dd9a35b47d064829fec6f3737da3eafb51f4fdc26f078463b9220c2c84e71`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan47_ModCompatibilityAndDependencySort_EndToEnd
Plan65_FinalWishAndEpilogueChronicle_EndToEnd
Plan47_Plan65_CombinedEcosystem_ModdedWishesEpilogueJourney
```


# Appendix R.585 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Memorial/MemorialComponentTests.cs`

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


# Appendix R.586 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs`

### `Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 130; SHA-256: `ce1fab1d5f8ae306d6a960403b523ec53733f42d05b45511e5a686a4a6ae84eb`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
PhantomMemory_Motivation_BoostsWorkSpeedAndDecays
TradeSpecialty_CraftingItems_AdvancesTierAndMasters
FinalWish_CompletedWish_GrantsPermanentShelterMoraleBuff
GuiltInsomnia_RecordedGuilt_RaisesInsomniaSeverity
RespiratoryDegeneration_AshZoneExposure_ReducesStamina
```


# Appendix R.587 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/FactionWarFlagExtensionTests.cs`

### `Ashfall.Core.Tests/FactionWarFlagExtensionTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 265; SHA-256: `4c641fefb2e42e1e763e03080616f5a460618cd239c78014ff6a9a445c42137b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
FlagTrigger_FiresOnlyWhileTheFlagIsKnown
ExternalFlagProbe_IsConsulted
StageRequiresFlag_HoldsSurfacingUntilProduced
StageProducesFlag_LandsInStateAndSurvivesRoundTrip
ZeroChoiceStage_AutoAdvancesAndProduces
ChoiceRequiresFlag_HidesChoiceUntilSetAndRefusesSpeculativeResolve
ChoiceProducesFlagAndStandingDelta_AreApplied
ZeroStandingDeltaOrEmptyFaction_NeverTouchesTheApplier
OldSaveWithoutProducedFlags_RestoresClean
AuthoredPrePlan25Stage_StillSurfacesUnchanged
```


# Appendix R.588 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Verdict/Plan82_67VerdictCassetteIntegrationTests.cs`

### `Ashfall.Core.Tests/Verdict/Plan82_67VerdictCassetteIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 171; SHA-256: `f408e9b34a96d7826acfcd7cd1a88d0a81f07102c1ab76a1bdc41bd8a1eb4e10`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
VerdictLocationsAndCassetteCatalog_LoadAccurately_WithoutCollisions
CassettePlaybackSystem_AcquireAndPlaySequence_GrantsMoraleAndCompletesSet
VerdictCartographyToCassetteScavenging_CrossSystemLinkage
CassettePlaybackSystem_SaveRestoreRoundTrip_PreservesState
```


# Appendix R.589 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/YearOfAshTests.cs`

### `Ashfall.Core.Tests/YearOfAshTests.cs`

- Current test declarations: Fact=26, Theory=0, InlineData=0.
- File lines: 803; SHA-256: `47f8258fe2617608a3078688d92411d22003c93f181e2c8f6bf6115d540b1adb`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Timeline_AdvancesThroughAllThreePhases
Timeline_IgnoresRepeatedAndOutOfOrderDays
Timeline_RestoreDerivesPhaseFromAuthoritativeDay
DoorEncounters_EvaluatesHumanistVsRuthlessReactionsDeterministically
DoorEncounters_TraumaBondDampensNegativeMoraleImpact
FactionWar_ModifiesStandingAndEnactsDecrees
YearOfAshSave_CapturesAndEncodesDeterministicChecksum
YearOfAshSave_Restore_RebuildsTimelineEncountersAndFactionWar
DefaultFactionRoster_IncludesForwardRoster
YearOfAshSave_V4_CapturesAndRestoresChainRunnerProgress
YearOfAshSave_V3Envelope_MigratesWithFreshChainRunner
RestoreState_WithNullSections_IsANoOp
DoorEncounterCatalogLoader_LoadsJsonEntriesCorrectly
YearOfAshCatalogLoader_LoadsItemsEventsAndQuestsCorrectly
FactionWar_SimulatesDailyFrictionCorrectly
YearOfAshCatalogLoader_LoadsLocationsRadioAndSurvivorsCorrectly
YearOfAshRadonSystem_SimulatesThawInfiltrationAndScrubberReplacement
YearOfAshDeepFreezeSystem_SimulatesSubZeroThermalBalanceAndIntakeIcing
YearOfAshSave_Roundtrip_PreservesDeepFreezeAndRadon
YearOfAshSave_Roundtrip_PreservesQuestlineProgress
YearOfAshSave_V1File_MigratesToCurrentWithFreshSections
GetPlayableQuestlines_NeverOffersAnUnadvanceableQuestline
WithheldQuestlineCount_ReportsTheUnauthoredContentGap
LegacyQuestConverter_ProducesPlayableQuestlines
LegacyQuestConverter_LinearTraversal_ReachesTerminal
VerdictLocations_LoadAndAreQueryable
```


# Appendix R.590 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/FactionWarClockTests.cs`

### `Ashfall.Core.Tests/FactionWarClockTests.cs`

- Current test declarations: Fact=1, Theory=0, InlineData=0.
- File lines: 17; SHA-256: `cc0121d7595b73b0436ffc80680671f335261d50bf319d0b1e284cb6285fbef9`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ToAuthoredDay_MapsPlayableYearOfAshOntoWarChainEpoch
```


# Appendix R.591 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/FactionWarCommuniqueExpansionTests.cs`

### `Ashfall.Core.Tests/FactionWarCommuniqueExpansionTests.cs`

- Current test declarations: Fact=31, Theory=0, InlineData=0.
- File lines: 562; SHA-256: `c64fcb63749a4c2011a9e75663f8b4761ec0ab84045a87e38fc72bce5c13b103`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsAtLeastTheBaselineCorpus
Existing18Ids_PreservedVerbatim
Existing18Ids_RemainInChronologicalFileOrder
AllIds_UniqueAndNonEmpty
AllEventChainIds_ResolveToRealChains
AllFactionIds_ResolveToKnownFactions
TitlesAndBodies_NonEmpty_DayPositive
Chronology_NoCommuniquePrecedesTheEarliestStageOfItsChain
Communiques_NeverReferenceFlagGatedChains
ForwardRoster_IssuesNoStatementBeforeItsPublicIdentity
CeasefireStatements_DoNotPrecedeTheCeasefire
GetCommuniquesForFaction_DayBoundary_IsExact
MultipleCommuniquesPerFactionPerDay_AreLegal
AuthorNote_NeverAppearsInPlayerFacingText
AuthorNote_NoPlayerFacingConsumers_Gate
Loader_DuplicateId_BothLoad_DocumentedPolicy
Loader_UnknownEventChainAndFaction_StillLoad_DocumentedPolicy
Loader_NegativeDayAndEmptyBody_StillLoad_DocumentedPolicy
Loader_AuthorNote_AbsentDeserializesToEmpty
Loader_ParseFailure_IsToleratedPerFile
Catalog_LoadsExactly40Communiques
Allocation_MatchesPlan133_Distribution
Coverage_AtLeast10Chains_HaveTwoCompetingPerspectives
Coverage_AtLeast5Chains_HaveThreePerspectives
Coverage_ColdWarAndOpenConflictBands_AreNoLongerUncovered
Repetition_NoDuplicateTitles_AndOnTheSkeletonDoesNotGrow
Repetition_ComeThroughClean_IsBoundedSignatureUse
Temporal_Distribution_IsNotClusteredInOneBand
NoCommunique_AssertsPossessionOfTheBranchSensitiveIntercept
FactionWarChainRunner_ExposesCatalogProperty
YearOfAshTimeline_ClampsPastDay360_PhaseA
```


# Appendix R.592 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Medical/PalliativeCareDignityEngineTests.cs`

### `Ashfall.Core.Tests/Medical/PalliativeCareDignityEngineTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 95; SHA-256: `73f88be9ba46b7c925a579d0e508b38af7c9fc6215d83ae67a30fa20de4c5e75`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AdvanceDailyCare_ReducesPainAndCalculatesDignity
EvaluateGriefStageProgression_AdvancesTowardAcceptance_WhenDignityHigh
CalculateMemorialEcho_HighDignityAndWishFulfilled_GrantsMoraleBuff
CalculateMemorialEcho_AgonizingNeglect_ImposesMoralePenalty
```


# Appendix R.593 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Memorial/MemorialGriefPortTests.cs`

### `Ashfall.Core.Tests/Memorial/MemorialGriefPortTests.cs`

- Current test declarations: Fact=7, Theory=2, InlineData=6.
- File lines: 234; SHA-256: `2edcb5891c1f0fa106613d599d5c7487a140034e9426d23215c1741c4ea08112`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CapturingGriefSink_QualityScaleMatchesSpec
Memorialize_FiresGriefSink_OnceOnFirstCall
Memorialize_DifferentQualities_ProduceDifferentGrief
Memorialize_WithNullGriefSink_DoesNotThrow
MemorialEntry_RoundTrips_DeathQuality_And_Outcome
MemorialEntry_DefaultsTo_PeacefulAndBurial_WhenInputOmitted
CaptureAndRestore_Preserves_DeathQuality_And_Outcome
LegacyEntry_WithoutDeathQuality_LoadsAsPeaceful_Default
SurvivngRelationshipIds_NullSafe
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.
