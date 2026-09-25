# Plan 62 — Trade Tell Lines, Stance/Trust Bands and Deterministic Negotiation Presentation

> **Rebuild status:** COMPLETE 60-LINE CONTENT/ENGINE LOOP — TRADE-SCREEN REACHABILITY AND TONE AUDIT
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

- The current `trade_tell_lines.json` has four trust bands and 60 non-empty lines distributed across hostile_raid, rob, refuse, trade and share_intel pools. The historical 0-line premise is stale.
- The live route is JSON → `TradeTellEngine.LoadFromJson` → `ITradeTellProvider.TrySelectTell` → `TradeScreenPresenter`/`TradeScreenScenarios` view model, with the presenter’s injected seeded RNG selecting a pool line.
- The plan protects the distinction between a behavioral observation and a mechanical trade modifier: tells inform presentation/negotiation context but do not silently alter price, trust or stance unless a current command already does so.

**Bounded outcome:** Retire the old 4-band/60-line pure-data brief as a new content project. The current JSON has 60 lines across five stances and four trust bands, `TradeTellEngine` parses and selects them deterministically, and `TradeScreenPresenter`/`TradeScreenScenarios` consume the provider. The remaining plan is a truthful trade-screen/reachability, replay and tone audit—not a new negotiation model or a second tell catalog.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `trade_tell_lines.json` is valid JSON with four trust bands and 60 current tell lines; pools are keyed by the five current `TradeStance` values.
- `TradeTellEngine` registers ordered bands, resolves the first inclusive trust range, selects a line through `ISeededRng`, and returns false for a missing stance/band pool.
- `TradeScreenPresenter` and `TradeScreenScenarios.CreateBinding` inject `ITradeTellProvider` and project the selected tell into the existing trade view model.
- Historical DEC-242 tests cover the corpus and seeded rotation; this plan does not claim a fresh test run or a new negotiation mechanic.

**Master-authority sections applied to this rebase:**

- Part II Factory Protocol: premise sweep, collision check, one lane/cluster, and evidence labels before drafting.
- Part II Step 5 continuity and anti-duplication checklist: data presence is not reachability.
- Part III cluster map: use the live C1–C17 owner map rather than a historical plan title.
- Part IV backlog discipline: consume a verified candidate or record why it is stale; do not widen a bounded outcome.
- Part V Template S/R: subject intent and recommended route remain separate from implementation commitments.
- Part VI Multi-Session Growth Protocol: 250k is a depth target, not permission to manufacture volume.
- Live source/data authority: current catalog, loader, host, save, and focused tests outrank generated prose.
- Anti-padding rule: if the evidence queue is exhausted, stop and report no warranted continuation.
- C11 Economy cluster: trade tells are presentation/negotiation context; market, settlement and inventory remain mechanical owners.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the empty/60-line premise with a 60-line current census across stance/band pools.
- Verify all five stance keys, four inclusive trust ranges, non-empty lines and stable selection behavior against the current engine.
- Trace the trade presenter/scenario binding to the live host route and audit what the player sees before/after a command.
- Preserve seeded selection and keep tells observational; no hidden price or trust delta is introduced by this plan.

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
| band/pool parsing and deterministic tell selection | TradeTellEngine | `Assets/Ashfall.Core/Economy/TradeTellEngine.cs` | Owns tell content selection; it does not own market price or faction standing. |
| current trade projection and command context | TradeScreenPresenter | `Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs` | Injects tell provider/RNG and projects the view model. |
| scenario/preview binding | TradeScreenScenarios | `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs` | Uses the tell provider for current trade presentation and tests. |
| stance, trust and transaction state | Faction/settlement trade owners | `Assets/Ashfall.Core/Economy/MarketSystem.cs; Assets/Ashfall.Core/World/SettlementCatalog.cs` | Current mechanical context; tells do not duplicate it. |
| player-visible table and caravan actions | Trade UI/host route | `src/UI/CaravanBarterLedgerPanel.cs; src/Main.Economy.cs` | Presentation/commands only; exact route must be verified. |
| corpus, seam and replay proof | Trade focused tests | `Ashfall.Core.Tests/TradeTellCorpusTests.cs; Ashfall.Core.Tests/Radiation/Plan81_62DoseTradeTellIntegrationTests.cs; Ashfall.Core.Tests/TradeScreenSeamTests.cs` | Focused evidence surface. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Trade Tell Lines, Stance/Trust Bands and Deterministic Negotiation Presentation
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ TradeTellEngine
│   band/pool parsing and deterministic tell selection
│ TradeScreenPresenter
│   current trade projection and command context
│ TradeScreenScenarios
│   scenario/preview binding
│ Faction/settlement trade owners
│   stance, trust and transaction state
│ Trade UI/host route
│   player-visible table and caravan actions
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

1. **Preserve current state ownership.** TradeTellEngine owns band/pool parsing and deterministic tell selection: Owns tell content selection; it does not own market price or faction standing.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| band/pool parsing and deterministic tell selection | TradeTellEngine | `Assets/Ashfall.Core/Economy/TradeTellEngine.cs` | Owns tell content selection; it does not own market price or faction standing. |
| current trade projection and command context | TradeScreenPresenter | `Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs` | Injects tell provider/RNG and projects the view model. |
| scenario/preview binding | TradeScreenScenarios | `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs` | Uses the tell provider for current trade presentation and tests. |
| stance, trust and transaction state | Faction/settlement trade owners | `Assets/Ashfall.Core/Economy/MarketSystem.cs; Assets/Ashfall.Core/World/SettlementCatalog.cs` | Current mechanical context; tells do not duplicate it. |
| player-visible table and caravan actions | Trade UI/host route | `src/UI/CaravanBarterLedgerPanel.cs; src/Main.Economy.cs` | Presentation/commands only; exact route must be verified. |
| corpus, seam and replay proof | Trade focused tests | `Ashfall.Core.Tests/TradeTellCorpusTests.cs; Ashfall.Core.Tests/Radiation/Plan81_62DoseTradeTellIntegrationTests.cs; Ashfall.Core.Tests/TradeScreenSeamTests.cs` | Focused evidence surface. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load trade tell catalog through the current engine
2. bind provider and seeded RNG into trade presenter/scenario
3. read current stance/trust from the existing trade owner
4. resolve the first trust band
5. select a line from the matching stance/band pool
6. project the observation beside the current offer/command state
7. leave price, inventory and faction state owned by their existing commands

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Tell lines and trust bands are immutable catalog data; selected tell is a transient deterministic projection.
- Band resolution is first-match inclusive; missing pool returns false rather than silently borrowing another stance’s voice.
- The injected RNG, not wall-clock or panel timing, controls rotation.
- Mechanical trade state remains owned by the existing market/settlement/transaction seams.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Every stance/band pool is non-empty and line text is trimmed before registration.
- A trust value at a boundary belongs to the documented inclusive band; gaps are visible as an empty selection.
- A missing stance/band returns false and does not mutate the trade view or RNG state unexpectedly.
- Same seed, trust, stance and catalog state produce the same tell ID/line.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `trade_tell_lines.json` is the sole tell corpus.
- Do not add price modifiers, trust deltas or dialogue branches to a tell line.
- A new line needs a current stance/band pool, tone review and a consumer projection.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- No new save section: a selected tell is transient and re-derived from current state/seed.
- A future “seen tell” memory would require a current journal/knowledge owner and a new claim.
- Trade transaction saves remain owned by the current market/settlement/inventory systems.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- `ISeededRng` controls pool index; no `System.Random`, time or dictionary order.
- Band order is registration order and stance key mapping is explicit.
- Replay compares tell ID, line, stance, band and view-model state.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Trade presenter updates its view model after a current trade command/refresh.
- A tell selection is presentation data, not a new moral or market event.
- If a future negotiation consequence is added, it must use an existing owner command and typed fact.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/UI/CaravanBarterLedgerPanel.cs
- src/Main.Economy.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Tells are terse physical observations in a restrained survival setting.
- Avoid modern slang, copied dialogue, certainty about hidden intent and claims that a posture proves a mechanical outcome.
- Different stances may color voice, but the catalog must not stereotype or editorialize real groups.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A missing pool falls back to a different stance/band line. | TradeTellEngine | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A panel changes price/trust based on text without a command. | TradeScreenPresenter | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Two hosts use different RNG streams for the same transaction. | TradeScreenScenarios | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A line claims a hidden fact or copied real-world voice. | Faction/settlement trade owners | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new negotiation engine duplicates the market owner. | Trade UI/host route | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/TradeTellCorpusTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Radiation/Plan81_62DoseTradeTellIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/TradeScreenSeamTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — corpus census | Read 60 lines, engine, presenter, scenarios and current trade owner. | Stance/band shape is exact. | No production path until the owning implementation package is separately claimed. |
| 1 — selection proof | Exercise boundaries, missing pools and seeded replay. | One deterministic tell projection per input. | No production path until the owning implementation package is separately claimed. |
| 2 — host/UI truth audit | Trace presenter binding and current caravan route. | Tell text cannot bypass mechanical owner. | No production path until the owning implementation package is separately claimed. |
| 3 — tone/polish pass | Review physical specificity, restraint and accessibility. | No copied or mechanically misleading content. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/trade_tell_lines.json | READ ONLY; MODIFY only for a proven pool/content gap | 60-line authority |
| Assets/Ashfall.Core/Economy/TradeTellEngine.cs | READ ONLY | Selection owner |
| Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs | READ ONLY | Projection seam |
| src/UI/CaravanBarterLedgerPanel.cs | READ ONLY; MODIFY only under a new UI claim | Current presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Adding hidden mechanical effects to flavor text. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Breaking inclusive band boundaries. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Using a different RNG stream in the host. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Creating a second trade negotiation authority. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new tell lines in this package.
- No new negotiation mechanics.
- No new save section.
- No production/data/test/UI changes.

# 23. Rollback and Recovery

- Revert the planning document.
- Future engine/host changes retain the current trade save and seeded presentation fixtures.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 60 current lines and all stance/band pools are documented.
- Selection, replay, presenter and mechanical-owner boundaries are explicit.
- No hidden effect or duplicate trade authority is proposed.
- Focused tests and tone/accessibility obligations are named.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the empty/60-line premise with a 60-line current census across stance/band pools.
- Verify all five stance keys, four inclusive trust ranges, non-empty lines and stable selection behavior against the current engine.
- Trace the trade presenter/scenario binding to the live host route and audit what the player sees before/after a command.
- Preserve seeded selection and keep tells observational; no hidden price or trust delta is introduced by this plan.

## MUST NOT DO

- No new tell lines in this package.
- No new negotiation mechanics.
- No new save section.
- No production/data/test/UI changes.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/TradeTellCorpusTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Radiation/Plan81_62DoseTradeTellIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/TradeScreenSeamTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — corpus census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: band/pool parsing and deterministic tell selection → TradeTellEngine; current trade projection and command context → TradeScreenPresenter; scenario/preview binding → TradeScreenScenarios; stance, trust and transaction state → Faction/settlement trade owners; player-visible table and caravan actions → Trade UI/host route; corpus, seam and replay proof → Trade focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 62.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 62 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by TradeTellEngine or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Economy/TradeTellEngine.cs`

### `Assets/Ashfall.Core/Economy/TradeTellEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 213 lines / 8276 bytes.
- SHA-256: `ca0b50673b96d37a7d4a74f100dc0e7a3403ec2738106837d728eba22da784cb`.
- Architecture signals: seeded references=3; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class TradeTrustBands
public const string Hostile = "hostile";
public const string Wary = "wary";
public const string Neutral = "neutral";
public const string Warm = "warm";
public sealed class TradeTell
public string Id { get; }
public TradeStance Stance { get; }
public string Band { get; }
public string Line { get; }
public interface ITradeTellProvider
public sealed class TradeTellEngine : ITradeTellProvider
public int BandCount => _bands.Count;
public int PoolCount => _pools.Count;
public int LineCount { get; private set; }
public void RegisterBand(string id, float minInclusive, float maxInclusive) {
public void RegisterTellPool(TradeStance stance, string bandId, IEnumerable<string> lines) {
public string BandForTrust(float trust) {
public bool TrySelectTell(TradeStance stance, float trust, ISeededRng rng, out TradeTell tell) {
public bool TryGetPoolLines(TradeStance stance, string bandId, out IReadOnlyList<string> lines) {
public static TradeTellEngine LoadFromJson(string json) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs`

### `Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 467 lines / 18043 bytes.
- SHA-256: `eaba535609c3ce04e7628d9930b6667dbc223dc4c5fa83db4e78bfa2292387eb`.
- Architecture signals: seeded references=2; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TradeSelectionSnapshot
public Dictionary<string, int> PlayerOffers { get; set; } = new(StringComparer.Ordinal);
public Dictionary<string, int> FactionAsks { get; set; } = new(StringComparer.Ordinal);
public Dictionary<BiologicalTradeItem, int> BiologicalOffers { get; set; } = new();
public sealed class TradeScreenPresenter : ITradeIntentSink
public TradeScreenViewModel ViewModel { get; } = new TradeScreenViewModel();
public int ActiveOfferCount => _playerOfferCounts.Count;
public int ActiveAskCount => _factionAskCounts.Count;
public int ActiveBioCount => _bioOfferCounts.Count;
public int GetPlayerOfferCount(string itemId) =>
public int GetFactionAskCount(string itemId) =>
public int GetBiologicalOfferCount(BiologicalTradeItem item) =>
public TradeSelectionSnapshot CaptureSelection() {
public void RestoreSelection(TradeSelectionSnapshot snapshot) {
public void SetVoiceContext(TradeVoiceContext context) {
public void SetWorldContext(string phaseLabel, int day) {
public void SetWatchedItems(IEnumerable<string> itemIds) {
public bool Open(string factionId, string factionName, string leaderName, int successionGeneration) {
public void Close(bool traded = false) {
public void SetPlayerOffer(string itemId, int count) {
public void SetFactionAsk(string itemId, int count) {
public void SetBiologicalOffer(BiologicalTradeItem item, int count) {
public void ClearOffers() {
public void Recalculate() {
public bool TryConfirmTrade() {
public bool TryDemandParley() {
public string BuildQuoteSummary() {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs`

### `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 286 lines / 12304 bytes.
- SHA-256: `d0860320466aeb33812b16aea8a2484937cb8a57b88f4007e3f8f9989356b795`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TradeScreenScenario
public string Id { get; set; } = string.Empty;
public string FactionId { get; set; } = string.Empty;
public string FactionName { get; set; } = string.Empty;
public string LeaderName { get; set; } = string.Empty;
public int SuccessionGeneration { get; set; } = 1;
public TradeStance Stance { get; set; } = TradeStance.Refuse;
public float Trust { get; set; }
public float Aggression { get; set; }
public int ConsecutiveRepels { get; set; }
public bool HasSurrendered { get; set; }
public bool CanDemandParley { get; set; }
public string WorldPhase { get; set; } = string.Empty;
public int WorldDay { get; set; } = 1;
public List<ShockBadgeData> PriceShocks { get; set; } = new();
public List<ScarcityBandData> Scarcity { get; set; } = new();
public List<TradeLineData> PlayerOffers { get; set; } = new();
public List<TradeLineData> FactionDemands { get; set; } = new();
public Dictionary<BiologicalTradeItem, int> BiologicalOffers { get; set; } = new();
public TradeFairness ExpectedFairness { get; set; } = TradeFairness.EmptyTable;
public bool ConfirmSucceeds { get; set; }
public string RadioTicker { get; set; } = string.Empty;
public sealed class MockTradeIntentSink : ITradeIntentSink
public int ConfirmCalls { get; private set; }
public int ParleyCalls { get; private set; }
public int CloseCalls { get; private set; }
public bool? LastCloseWasTraded { get; private set; }
public bool ConfirmResult { get; set; } = true;
public bool TryConfirmTrade() {
public bool TryDemandParley() {
public void Close(bool traded) {
public sealed class MockTradeScreenBinding
public TradeScreenScenario Scenario { get; }
public TradeScreenViewModel ViewModel { get; }
public MockTradeIntentSink Intents { get; }
public static class TradeScreenScenarioLoader
public static IReadOnlyList<TradeScreenScenario> LoadFromJson(string json) {
public static MockTradeScreenBinding CreateBinding(TradeScreenScenario scenario, ITradeTellProvider tells, ISeededRng rng) {
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/World/SettlementCatalog.cs`

### `Assets/Ashfall.Core/World/SettlementCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 463 lines / 17110 bytes.
- SHA-256: `492fea4b385a078e5e94ed407fe2d74a9876a8143a92b87144db32bcb0d9d29e`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SettlementEconomy
public string PrimaryExport { get; set; } = string.Empty;
public string PrimaryImport { get; set; } = string.Empty;
public string TradeSpecialty { get; set; } = string.Empty;
public float PriceModifierExports { get; set; } = 1.0f;
public float PriceModifierImports { get; set; } = 1.0f;
public List<string> StockItemIds { get; set; } = new List<string>();
public sealed class SettlementSociety
public string Governance { get; set; } = string.Empty;
public int Population { get; set; } = 50;
public string CoreValue { get; set; } = string.Empty;
public string InternalTension { get; set; } = string.Empty;
public sealed class SettlementFactionRelation
public string PrimaryFaction { get; set; } = string.Empty;
public string StandingGateFaction { get; set; } = string.Empty;
public int MinStandingToEnter { get; set; } = 0;
public int HostileStandingThreshold { get; set; } = -40;
public sealed class SettlementDefinition
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string Archetype { get; set; } = string.Empty;
public string Region { get; set; } = string.Empty;
public string LocationId { get; set; } = string.Empty;
public string RouteNode { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public string SurvivalAdaptation { get; set; } = string.Empty;
public SettlementEconomy Economy { get; set; } = new SettlementEconomy();
public SettlementSociety Society { get; set; } = new SettlementSociety();
public SettlementFactionRelation FactionRelation { get; set; } = new SettlementFactionRelation();
public string LocationLink { get; set; } = string.Empty;
public int Population { get; set; } = 0;
public string Allegiance { get; set; } = string.Empty;
public int ThreatLevel { get; set; } = 2;
public string Attitude { get; set; } = "neutral";
public List<string> TradeGoods { get; set; } = new List<string>();
public List<string> TradeNeeds { get; set; } = new List<string>();
public string KeeperNpcId { get; set; } = string.Empty;
public string TraderNpcId { get; set; } = string.Empty;
public string FixtureNpcId { get; set; } = string.Empty;
public string SideworkQuestId { get; set; } = string.Empty;
public string GetEffectiveLocationId() => !string.IsNullOrEmpty(LocationLink) ? LocationLink : LocationId;
public int GetEffectivePopulation() => Population > 0 ? Population : (Society?.Population ?? 50);
public string GetEffectiveAllegiance() => !string.IsNullOrEmpty(Allegiance) ? Allegiance : (FactionRelation?.PrimaryFaction ?? "none");
public sealed class SettlementNpcGreeting
public string LowStanding { get; set; } = string.Empty;
public string Neutral { get; set; } = string.Empty;
public string HighStanding { get; set; } = string.Empty;
public sealed class SettlementNpcEntry
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string SettlementId { get; set; } = string.Empty;
public string Role { get; set; } = string.Empty; // "Keeper", "Trader", "Fixture"
public string Profession { get; set; } = string.Empty;
public string Faction { get; set; } = "none";
public string TradeSpecialty { get; set; } = string.Empty;
public string PhysicalAnchor { get; set; } = string.Empty;
public string Value { get; set; } = string.Empty;
public string Fear { get; set; } = string.Empty;
public string Contradiction { get; set; } = string.Empty;
public string PersonalThread { get; set; } = string.Empty;
public SettlementNpcGreeting Greetings { get; set; } = new SettlementNpcGreeting();
public List<string> TradeTells { get; set; } = new List<string>();
public string SideworkQuestId { get; set; } = string.Empty;
public string PortraitId { get; set; } = string.Empty;
public sealed class RepeatableQuestStage
public string Id { get; set; } = string.Empty;
public string Text { get; set; } = string.Empty;
public sealed class RepeatableQuestEntry
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string ProviderNpcId { get; set; } = string.Empty;
public string SettlementId { get; set; } = string.Empty;
public string Type { get; set; } = string.Empty;
public string Briefing { get; set; } = string.Empty;
public string PrereqQuestId { get; set; } = string.Empty;
public string TargetLocationId { get; set; } = string.Empty;
public int CooldownDays { get; set; } = 7;
public string RewardItemId { get; set; } = string.Empty;
public int RewardCount { get; set; } = 1;
public int StandingDelta { get; set; } = 5;
public List<RepeatableQuestStage> Stages { get; set; } = new List<RepeatableQuestStage>();
public sealed class SettlementState
public Dictionary<string, int> QuestAvailableDay { get; set; } = new Dictionary<string, int>();
public Dictionary<string, int> CompletedQuestCounts { get; set; } = new Dictionary<string, int>();
public sealed class SettlementCatalog
public int SettlementCount => _allSettlements.Count;
public int NpcCount => _allNpcs.Count;
public int QuestCount => _allQuests.Count;
public IReadOnlyList<SettlementDefinition> Settlements => _allSettlements;
public IReadOnlyList<SettlementNpcEntry> Npcs => _allNpcs;
public IReadOnlyList<RepeatableQuestEntry> Quests => _allQuests;
public static SettlementCatalog LoadFromDirectory(string directoryPath, IFileIO fileIO) {
public void LoadSettlementsJson(string json) {
public void LoadNpcsJson(string json) {
public void LoadQuestsJson(string json) {
public bool TryGetSettlement(string id, out SettlementDefinition settlement) {
public bool TryGetNpc(string id, out SettlementNpcEntry npc) {
public bool TryGetQuest(string id, out RepeatableQuestEntry quest) {
public string GetNpcGreeting(string npcId, float standing) {
public bool IsQuestAvailable(string questId, int currentDay) {
public void CompleteQuest(string questId, int currentDay) {
public int GetCompletedQuestCount(string questId) {
public SettlementState CaptureState() {
public void RestoreState(SettlementState? state) {
```


# Appendix B.06 — Current Code Architecture: `src/UI/CaravanBarterLedgerPanel.cs`

### `src/UI/CaravanBarterLedgerPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 273 lines / 10835 bytes.
- SHA-256: `d26fed01020da21467d00dbcef0594d808b44162cbeb8e55fc54f0d9acb0ce4f`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=6; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class CaravanBarterLedgerPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<string>? OnSetActiveFaction;
public bool IsBound => _tradeInner != null && _session != null;
public void Bind( EconomyHostSession session, IFactionStanceProvider? stanceProvider = null, IPriceShockProvider? priceShockProvider = null, IFactionRadioProvider? radioProvider = null, ISeededRng? rng = null)
public void BindViewModel(ITradeScreenViewModel viewModel, ITradeIntentSink intentSink) {
public void SetActiveFaction(string factionId) {
public void RefreshView() {
public override void _Ready() {
public void Open() {
public void Close() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix B.07 — Current Code Architecture: `src/Main.Economy.cs`

### `src/Main.Economy.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 366 lines / 15389 bytes.
- SHA-256: `2043c87b9cfaed583952235f93761567e656a645d675090b74cf213fe2e6c1ef`.
- Architecture signals: seeded references=0; save/restore symbols=5; typed event declarations=0; textual Godot mentions=11; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/trade_tell_lines.json`

### `Assets/StreamingAssets/Data/trade_tell_lines.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 8811 bytes / 8811 characters.
- SHA-256: `a86a8e9486d67d14d4ed4df63a47a32ecc25988d2287f822bb57754376b28b08`.
- Root keys: `$schema`, `description`, `schema_version`, `tells`, `trust_bands`, `version`.

Array-path census (minimum, maximum, observed rows):

```text
tells.hostile_raid.hostile: min=3, max=3, observed_paths=1
tells.hostile_raid.neutral: min=3, max=3, observed_paths=1
tells.hostile_raid.warm: min=3, max=3, observed_paths=1
tells.hostile_raid.wary: min=3, max=3, observed_paths=1
tells.refuse.hostile: min=3, max=3, observed_paths=1
tells.refuse.neutral: min=3, max=3, observed_paths=1
tells.refuse.warm: min=3, max=3, observed_paths=1
tells.refuse.wary: min=3, max=3, observed_paths=1
tells.rob.hostile: min=3, max=3, observed_paths=1
tells.rob.neutral: min=3, max=3, observed_paths=1
tells.rob.warm: min=3, max=3, observed_paths=1
tells.rob.wary: min=3, max=3, observed_paths=1
tells.share_intel.hostile: min=3, max=3, observed_paths=1
tells.share_intel.neutral: min=3, max=3, observed_paths=1
tells.share_intel.warm: min=3, max=3, observed_paths=1
tells.share_intel.wary: min=3, max=3, observed_paths=1
tells.trade.hostile: min=15, max=15, observed_paths=1
tells.trade.neutral: min=15, max=15, observed_paths=1
tells.trade.warm: min=15, max=15, observed_paths=1
tells.trade.wary: min=15, max=15, observed_paths=1
trust_bands: min=4, max=4, observed_paths=1
```

Representative record fields:

- `id`
- `max`
- `min`

Representative identifiers (ordered, capped for readability):

```text
hostile
wary
neutral
warm
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/trade_screen_scenarios.json`

### `Assets/StreamingAssets/Data/trade_screen_scenarios.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 23775 bytes / 23775 characters.
- SHA-256: `fc2d88b9a5cbe663c054a25226096a59dc259089b01d4521952314d6c37e507c`.
- Root keys: `$schema`, `description`, `scenarios`, `schema_version`, `version`.

Array-path census (minimum, maximum, observed rows):

```text
scenarios: min=15, max=15, observed_paths=1
scenarios[].faction_demands: min=1, max=1, observed_paths=2
scenarios[].player_offers: min=1, max=2, observed_paths=2
scenarios[].price_shocks: min=1, max=1, observed_paths=2
scenarios[].scarcity: min=1, max=1, observed_paths=2
```

Representative record fields:

- `aggression`
- `biological_offers`
- `can_demand_parley`
- `confirm_succeeds`
- `consecutive_repels`
- `expected_fairness`
- `faction_demands`
- `faction_id`
- `faction_name`
- `has_surrendered`
- `id`
- `leader_name`
- `player_offers`
- `price_shocks`
- `radio_ticker`
- `scarcity`
- `stance`
- `succession_generation`
- `trust`
- `world_day`
- `world_phase`

Representative identifiers (ordered, capped for readability):

```text
fair_deal
offer_short
empty_table
last_vials
winter_cart
depot_window
emergency_requisition
back_room_exchange
ledgerless_broker
long_road_caravan
salvage_caravan
settlement_of_accounts
crate_lot
border_runner
road_knowledge
```


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/faction_territory.json`

### `Assets/StreamingAssets/Data/faction_territory.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 19950 bytes / 19950 characters.
- SHA-256: `ef0940bbe98f3082b75cbbef77df670e7e2c7358b89ddeefc20b8fd119b90915`.
- Root keys: `collection_id`, `contested_zones`, `schema_version`, `territories`.

Array-path census (minimum, maximum, observed rows):

```text
contested_zones: min=5, max=5, observed_paths=1
contested_zones[].claimant_factions: min=3, max=3, observed_paths=2
territories: min=19, max=19, observed_paths=1
territories[].contested_with: min=2, max=2, observed_paths=2
territories[].control_points: min=2, max=2, observed_paths=2
territories[].controlled_nodes: min=1, max=1, observed_paths=2
```

Representative record fields:

- `classification`
- `contested_with`
- `control_points`
- `control_strength`
- `controlled_nodes`
- `description`
- `display_name`
- `faction`
- `id`
- `primary_resource_interest`
- `shift_trigger`
- `territory_scale`
- `trade_tax`
- `travel_safety`

Representative identifiers (ordered, capped for readability):

```text
territory_the_office
territory_the_cutters
territory_black_flotilla
territory_the_fleet
territory_deserter_coalition
territory_cold_count
territory_the_tally
territory_grain_exchange
territory_quiet_house
territory_scavenger_guild
territory_long_walk
territory_undertow
territory_hydro_barons
territory_iron_raiders
territory_the_provisioned
territory_archivists
territory_lamplighters
territory_sun_seekers
territory_osteophages
```


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/TradeTellCorpusTests.cs`

### `Ashfall.Core.Tests/TradeTellCorpusTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 178; SHA-256: `c16c43086c8d61f19ff3d3be209b669262cddd2c359f206e62a91a544a8288e3`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Corpus_LoadsFourBandsAndTwentyPools
Corpus_EveryStanceAndBandSelectsALegibleLine
Corpus_ToneLint_NoModernSlangOrAnachronisms
Corpus_NoDuplicateLinesWithinPool
Engine_BandBoundaries_MapTrustCorrectly
Engine_DeterministicRotation_SameSeedSameLine
Engine_RotationVariesAcrossSeeds
```


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Radiation/Plan81_62DoseTradeTellIntegrationTests.cs`

### `Ashfall.Core.Tests/Radiation/Plan81_62DoseTradeTellIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 165; SHA-256: `1d031e831d793301f400db8fcc6b9122a0a307a6607bfca9d1f052377e32d14c`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan81_62_DoseLocations_SectorMappingAndRiskCalibration
Plan81_62_TradeTell_TrustBands_And_PoolCoverage
Plan81_62_HotspotTrade_TellRotation_SeededDeterminism
Plan81_62_DoseLocation_RadiationStress_ModulatesTraderStance
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/TradeScreenSeamTests.cs`

### `Ashfall.Core.Tests/TradeScreenSeamTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 329; SHA-256: `7940a9f2745852452d4a37666199d3187914ce18d05551e675588fa27169cfc6`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Scenarios_LoadAllThreeFromData
Scenario_FairDeal_ComputedFairnessMatchesDataExpectation
Scenario_OfferShort_BlocksConfirmAndKeepsStanceLegible
Scenario_EmptyTable_IsDeliberateNotBroken
Scenario_IntentSink_CloseRecordsTradedFlag
Presenter_MapsProvidersOntoViewModel
Presenter_ZeroMutation_InvariantHolds
Presenter_ApiParity_TradeScreenUISurface
Presenter_BioOffersPricedByCoreRule
Presenter_RoutesExecutionThroughSink
```


# Appendix E.14 — Supporting Code Evidence: `Assets/Ashfall.Core/Economy/TradeTellEngine.cs`

### `Assets/Ashfall.Core/Economy/TradeTellEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 213 lines / 8276 bytes.
- SHA-256: `ca0b50673b96d37a7d4a74f100dc0e7a3403ec2738106837d728eba22da784cb`.
- Architecture signals: seeded references=3; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class TradeTrustBands
public const string Hostile = "hostile";
public const string Wary = "wary";
public const string Neutral = "neutral";
public const string Warm = "warm";
public sealed class TradeTell
public string Id { get; }
public TradeStance Stance { get; }
public string Band { get; }
public string Line { get; }
public interface ITradeTellProvider
public sealed class TradeTellEngine : ITradeTellProvider
public int BandCount => _bands.Count;
public int PoolCount => _pools.Count;
public int LineCount { get; private set; }
public void RegisterBand(string id, float minInclusive, float maxInclusive) {
public void RegisterTellPool(TradeStance stance, string bandId, IEnumerable<string> lines) {
public string BandForTrust(float trust) {
public bool TrySelectTell(TradeStance stance, float trust, ISeededRng rng, out TradeTell tell) {
public bool TryGetPoolLines(TradeStance stance, string bandId, out IReadOnlyList<string> lines) {
public static TradeTellEngine LoadFromJson(string json) {
```


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs`

### `Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 467 lines / 18043 bytes.
- SHA-256: `eaba535609c3ce04e7628d9930b6667dbc223dc4c5fa83db4e78bfa2292387eb`.
- Architecture signals: seeded references=2; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TradeSelectionSnapshot
public Dictionary<string, int> PlayerOffers { get; set; } = new(StringComparer.Ordinal);
public Dictionary<string, int> FactionAsks { get; set; } = new(StringComparer.Ordinal);
public Dictionary<BiologicalTradeItem, int> BiologicalOffers { get; set; } = new();
public sealed class TradeScreenPresenter : ITradeIntentSink
public TradeScreenViewModel ViewModel { get; } = new TradeScreenViewModel();
public int ActiveOfferCount => _playerOfferCounts.Count;
public int ActiveAskCount => _factionAskCounts.Count;
public int ActiveBioCount => _bioOfferCounts.Count;
public int GetPlayerOfferCount(string itemId) =>
public int GetFactionAskCount(string itemId) =>
public int GetBiologicalOfferCount(BiologicalTradeItem item) =>
public TradeSelectionSnapshot CaptureSelection() {
public void RestoreSelection(TradeSelectionSnapshot snapshot) {
public void SetVoiceContext(TradeVoiceContext context) {
public void SetWorldContext(string phaseLabel, int day) {
public void SetWatchedItems(IEnumerable<string> itemIds) {
public bool Open(string factionId, string factionName, string leaderName, int successionGeneration) {
public void Close(bool traded = false) {
public void SetPlayerOffer(string itemId, int count) {
public void SetFactionAsk(string itemId, int count) {
public void SetBiologicalOffer(BiologicalTradeItem item, int count) {
public void ClearOffers() {
public void Recalculate() {
public bool TryConfirmTrade() {
public bool TryDemandParley() {
public string BuildQuoteSummary() {
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs`

### `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 286 lines / 12304 bytes.
- SHA-256: `d0860320466aeb33812b16aea8a2484937cb8a57b88f4007e3f8f9989356b795`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TradeScreenScenario
public string Id { get; set; } = string.Empty;
public string FactionId { get; set; } = string.Empty;
public string FactionName { get; set; } = string.Empty;
public string LeaderName { get; set; } = string.Empty;
public int SuccessionGeneration { get; set; } = 1;
public TradeStance Stance { get; set; } = TradeStance.Refuse;
public float Trust { get; set; }
public float Aggression { get; set; }
public int ConsecutiveRepels { get; set; }
public bool HasSurrendered { get; set; }
public bool CanDemandParley { get; set; }
public string WorldPhase { get; set; } = string.Empty;
public int WorldDay { get; set; } = 1;
public List<ShockBadgeData> PriceShocks { get; set; } = new();
public List<ScarcityBandData> Scarcity { get; set; } = new();
public List<TradeLineData> PlayerOffers { get; set; } = new();
public List<TradeLineData> FactionDemands { get; set; } = new();
public Dictionary<BiologicalTradeItem, int> BiologicalOffers { get; set; } = new();
public TradeFairness ExpectedFairness { get; set; } = TradeFairness.EmptyTable;
public bool ConfirmSucceeds { get; set; }
public string RadioTicker { get; set; } = string.Empty;
public sealed class MockTradeIntentSink : ITradeIntentSink
public int ConfirmCalls { get; private set; }
public int ParleyCalls { get; private set; }
public int CloseCalls { get; private set; }
public bool? LastCloseWasTraded { get; private set; }
public bool ConfirmResult { get; set; } = true;
public bool TryConfirmTrade() {
public bool TryDemandParley() {
public void Close(bool traded) {
public sealed class MockTradeScreenBinding
public TradeScreenScenario Scenario { get; }
public TradeScreenViewModel ViewModel { get; }
public MockTradeIntentSink Intents { get; }
public static class TradeScreenScenarioLoader
public static IReadOnlyList<TradeScreenScenario> LoadFromJson(string json) {
public static MockTradeScreenBinding CreateBinding(TradeScreenScenario scenario, ITradeTellProvider tells, ISeededRng rng) {
```


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/World/SettlementCatalog.cs`

### `Assets/Ashfall.Core/World/SettlementCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 463 lines / 17110 bytes.
- SHA-256: `492fea4b385a078e5e94ed407fe2d74a9876a8143a92b87144db32bcb0d9d29e`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SettlementEconomy
public string PrimaryExport { get; set; } = string.Empty;
public string PrimaryImport { get; set; } = string.Empty;
public string TradeSpecialty { get; set; } = string.Empty;
public float PriceModifierExports { get; set; } = 1.0f;
public float PriceModifierImports { get; set; } = 1.0f;
public List<string> StockItemIds { get; set; } = new List<string>();
public sealed class SettlementSociety
public string Governance { get; set; } = string.Empty;
public int Population { get; set; } = 50;
public string CoreValue { get; set; } = string.Empty;
public string InternalTension { get; set; } = string.Empty;
public sealed class SettlementFactionRelation
public string PrimaryFaction { get; set; } = string.Empty;
public string StandingGateFaction { get; set; } = string.Empty;
public int MinStandingToEnter { get; set; } = 0;
public int HostileStandingThreshold { get; set; } = -40;
public sealed class SettlementDefinition
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string Archetype { get; set; } = string.Empty;
public string Region { get; set; } = string.Empty;
public string LocationId { get; set; } = string.Empty;
public string RouteNode { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public string SurvivalAdaptation { get; set; } = string.Empty;
public SettlementEconomy Economy { get; set; } = new SettlementEconomy();
public SettlementSociety Society { get; set; } = new SettlementSociety();
public SettlementFactionRelation FactionRelation { get; set; } = new SettlementFactionRelation();
public string LocationLink { get; set; } = string.Empty;
public int Population { get; set; } = 0;
public string Allegiance { get; set; } = string.Empty;
public int ThreatLevel { get; set; } = 2;
public string Attitude { get; set; } = "neutral";
public List<string> TradeGoods { get; set; } = new List<string>();
public List<string> TradeNeeds { get; set; } = new List<string>();
public string KeeperNpcId { get; set; } = string.Empty;
public string TraderNpcId { get; set; } = string.Empty;
public string FixtureNpcId { get; set; } = string.Empty;
public string SideworkQuestId { get; set; } = string.Empty;
public string GetEffectiveLocationId() => !string.IsNullOrEmpty(LocationLink) ? LocationLink : LocationId;
public int GetEffectivePopulation() => Population > 0 ? Population : (Society?.Population ?? 50);
public string GetEffectiveAllegiance() => !string.IsNullOrEmpty(Allegiance) ? Allegiance : (FactionRelation?.PrimaryFaction ?? "none");
public sealed class SettlementNpcGreeting
public string LowStanding { get; set; } = string.Empty;
public string Neutral { get; set; } = string.Empty;
public string HighStanding { get; set; } = string.Empty;
public sealed class SettlementNpcEntry
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string SettlementId { get; set; } = string.Empty;
public string Role { get; set; } = string.Empty; // "Keeper", "Trader", "Fixture"
public string Profession { get; set; } = string.Empty;
public string Faction { get; set; } = "none";
public string TradeSpecialty { get; set; } = string.Empty;
public string PhysicalAnchor { get; set; } = string.Empty;
public string Value { get; set; } = string.Empty;
public string Fear { get; set; } = string.Empty;
public string Contradiction { get; set; } = string.Empty;
public string PersonalThread { get; set; } = string.Empty;
public SettlementNpcGreeting Greetings { get; set; } = new SettlementNpcGreeting();
public List<string> TradeTells { get; set; } = new List<string>();
public string SideworkQuestId { get; set; } = string.Empty;
public string PortraitId { get; set; } = string.Empty;
public sealed class RepeatableQuestStage
public string Id { get; set; } = string.Empty;
public string Text { get; set; } = string.Empty;
public sealed class RepeatableQuestEntry
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string ProviderNpcId { get; set; } = string.Empty;
public string SettlementId { get; set; } = string.Empty;
public string Type { get; set; } = string.Empty;
public string Briefing { get; set; } = string.Empty;
public string PrereqQuestId { get; set; } = string.Empty;
public string TargetLocationId { get; set; } = string.Empty;
public int CooldownDays { get; set; } = 7;
public string RewardItemId { get; set; } = string.Empty;
public int RewardCount { get; set; } = 1;
public int StandingDelta { get; set; } = 5;
public List<RepeatableQuestStage> Stages { get; set; } = new List<RepeatableQuestStage>();
public sealed class SettlementState
public Dictionary<string, int> QuestAvailableDay { get; set; } = new Dictionary<string, int>();
public Dictionary<string, int> CompletedQuestCounts { get; set; } = new Dictionary<string, int>();
public sealed class SettlementCatalog
public int SettlementCount => _allSettlements.Count;
public int NpcCount => _allNpcs.Count;
public int QuestCount => _allQuests.Count;
public IReadOnlyList<SettlementDefinition> Settlements => _allSettlements;
public IReadOnlyList<SettlementNpcEntry> Npcs => _allNpcs;
public IReadOnlyList<RepeatableQuestEntry> Quests => _allQuests;
public static SettlementCatalog LoadFromDirectory(string directoryPath, IFileIO fileIO) {
public void LoadSettlementsJson(string json) {
public void LoadNpcsJson(string json) {
public void LoadQuestsJson(string json) {
public bool TryGetSettlement(string id, out SettlementDefinition settlement) {
public bool TryGetNpc(string id, out SettlementNpcEntry npc) {
public bool TryGetQuest(string id, out RepeatableQuestEntry quest) {
public string GetNpcGreeting(string npcId, float standing) {
public bool IsQuestAvailable(string questId, int currentDay) {
public void CompleteQuest(string questId, int currentDay) {
public int GetCompletedQuestCount(string questId) {
public SettlementState CaptureState() {
public void RestoreState(SettlementState? state) {
```


# Appendix E.18 — Supporting Code Evidence: `src/UI/CaravanBarterLedgerPanel.cs`

### `src/UI/CaravanBarterLedgerPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 273 lines / 10835 bytes.
- SHA-256: `d26fed01020da21467d00dbcef0594d808b44162cbeb8e55fc54f0d9acb0ce4f`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=6; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class CaravanBarterLedgerPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<string>? OnSetActiveFaction;
public bool IsBound => _tradeInner != null && _session != null;
public void Bind( EconomyHostSession session, IFactionStanceProvider? stanceProvider = null, IPriceShockProvider? priceShockProvider = null, IFactionRadioProvider? radioProvider = null, ISeededRng? rng = null)
public void BindViewModel(ITradeScreenViewModel viewModel, ITradeIntentSink intentSink) {
public void SetActiveFaction(string factionId) {
public void RefreshView() {
public override void _Ready() {
public void Open() {
public void Close() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix G.19 — Supporting Regression Evidence: `Ashfall.Core.Tests/TradeTellCorpusTests.cs`

### `Ashfall.Core.Tests/TradeTellCorpusTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 178; SHA-256: `c16c43086c8d61f19ff3d3be209b669262cddd2c359f206e62a91a544a8288e3`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Corpus_LoadsFourBandsAndTwentyPools
Corpus_EveryStanceAndBandSelectsALegibleLine
Corpus_ToneLint_NoModernSlangOrAnachronisms
Corpus_NoDuplicateLinesWithinPool
Engine_BandBoundaries_MapTrustCorrectly
Engine_DeterministicRotation_SameSeedSameLine
Engine_RotationVariesAcrossSeeds
```


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/Radiation/Plan81_62DoseTradeTellIntegrationTests.cs`

### `Ashfall.Core.Tests/Radiation/Plan81_62DoseTradeTellIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 165; SHA-256: `1d031e831d793301f400db8fcc6b9122a0a307a6607bfca9d1f052377e32d14c`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan81_62_DoseLocations_SectorMappingAndRiskCalibration
Plan81_62_TradeTell_TrustBands_And_PoolCoverage
Plan81_62_HotspotTrade_TellRotation_SeededDeterminism
Plan81_62_DoseLocation_RadiationStress_ModulatesTraderStance
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/TradeScreenSeamTests.cs`

### `Ashfall.Core.Tests/TradeScreenSeamTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 329; SHA-256: `7940a9f2745852452d4a37666199d3187914ce18d05551e675588fa27169cfc6`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Scenarios_LoadAllThreeFromData
Scenario_FairDeal_ComputedFairnessMatchesDataExpectation
Scenario_OfferShort_BlocksConfirmAndKeepsStanceLegible
Scenario_EmptyTable_IsDeliberateNotBroken
Scenario_IntentSink_CloseRecordsTradedFlag
Presenter_MapsProvidersOntoViewModel
Presenter_ZeroMutation_InvariantHolds
Presenter_ApiParity_TradeScreenUISurface
Presenter_BioOffersPricedByCoreRule
Presenter_RoutesExecutionThroughSink
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
| band/pool parsing and deterministic tell selection | TradeTellEngine | current trade projection and command context | TradeScreenPresenter | Owner emits/reads a typed fact; no mirror state. |
| band/pool parsing and deterministic tell selection | TradeTellEngine | scenario/preview binding | TradeScreenScenarios | Owner emits/reads a typed fact; no mirror state. |
| band/pool parsing and deterministic tell selection | TradeTellEngine | stance, trust and transaction state | Faction/settlement trade owners | Owner emits/reads a typed fact; no mirror state. |
| band/pool parsing and deterministic tell selection | TradeTellEngine | player-visible table and caravan actions | Trade UI/host route | Owner emits/reads a typed fact; no mirror state. |
| band/pool parsing and deterministic tell selection | TradeTellEngine | corpus, seam and replay proof | Trade focused tests | Owner emits/reads a typed fact; no mirror state. |
| current trade projection and command context | TradeScreenPresenter | band/pool parsing and deterministic tell selection | TradeTellEngine | Owner emits/reads a typed fact; no mirror state. |
| current trade projection and command context | TradeScreenPresenter | scenario/preview binding | TradeScreenScenarios | Owner emits/reads a typed fact; no mirror state. |
| current trade projection and command context | TradeScreenPresenter | stance, trust and transaction state | Faction/settlement trade owners | Owner emits/reads a typed fact; no mirror state. |
| current trade projection and command context | TradeScreenPresenter | player-visible table and caravan actions | Trade UI/host route | Owner emits/reads a typed fact; no mirror state. |
| current trade projection and command context | TradeScreenPresenter | corpus, seam and replay proof | Trade focused tests | Owner emits/reads a typed fact; no mirror state. |
| scenario/preview binding | TradeScreenScenarios | band/pool parsing and deterministic tell selection | TradeTellEngine | Owner emits/reads a typed fact; no mirror state. |
| scenario/preview binding | TradeScreenScenarios | current trade projection and command context | TradeScreenPresenter | Owner emits/reads a typed fact; no mirror state. |
| scenario/preview binding | TradeScreenScenarios | stance, trust and transaction state | Faction/settlement trade owners | Owner emits/reads a typed fact; no mirror state. |
| scenario/preview binding | TradeScreenScenarios | player-visible table and caravan actions | Trade UI/host route | Owner emits/reads a typed fact; no mirror state. |
| scenario/preview binding | TradeScreenScenarios | corpus, seam and replay proof | Trade focused tests | Owner emits/reads a typed fact; no mirror state. |
| stance, trust and transaction state | Faction/settlement trade owners | band/pool parsing and deterministic tell selection | TradeTellEngine | Owner emits/reads a typed fact; no mirror state. |
| stance, trust and transaction state | Faction/settlement trade owners | current trade projection and command context | TradeScreenPresenter | Owner emits/reads a typed fact; no mirror state. |
| stance, trust and transaction state | Faction/settlement trade owners | scenario/preview binding | TradeScreenScenarios | Owner emits/reads a typed fact; no mirror state. |
| stance, trust and transaction state | Faction/settlement trade owners | player-visible table and caravan actions | Trade UI/host route | Owner emits/reads a typed fact; no mirror state. |
| stance, trust and transaction state | Faction/settlement trade owners | corpus, seam and replay proof | Trade focused tests | Owner emits/reads a typed fact; no mirror state. |
| player-visible table and caravan actions | Trade UI/host route | band/pool parsing and deterministic tell selection | TradeTellEngine | Owner emits/reads a typed fact; no mirror state. |
| player-visible table and caravan actions | Trade UI/host route | current trade projection and command context | TradeScreenPresenter | Owner emits/reads a typed fact; no mirror state. |
| player-visible table and caravan actions | Trade UI/host route | scenario/preview binding | TradeScreenScenarios | Owner emits/reads a typed fact; no mirror state. |
| player-visible table and caravan actions | Trade UI/host route | stance, trust and transaction state | Faction/settlement trade owners | Owner emits/reads a typed fact; no mirror state. |
| player-visible table and caravan actions | Trade UI/host route | corpus, seam and replay proof | Trade focused tests | Owner emits/reads a typed fact; no mirror state. |
| corpus, seam and replay proof | Trade focused tests | band/pool parsing and deterministic tell selection | TradeTellEngine | Owner emits/reads a typed fact; no mirror state. |
| corpus, seam and replay proof | Trade focused tests | current trade projection and command context | TradeScreenPresenter | Owner emits/reads a typed fact; no mirror state. |
| corpus, seam and replay proof | Trade focused tests | scenario/preview binding | TradeScreenScenarios | Owner emits/reads a typed fact; no mirror state. |
| corpus, seam and replay proof | Trade focused tests | stance, trust and transaction state | Faction/settlement trade owners | Owner emits/reads a typed fact; no mirror state. |
| corpus, seam and replay proof | Trade focused tests | player-visible table and caravan actions | Trade UI/host route | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the empty/60-line premise with a 60-line current census across stance/band pools. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Verify all five stance keys, four inclusive trust ranges, non-empty lines and stable selection behavior against the current engine. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Trace the trade presenter/scenario binding to the live host route and audit what the player sees before/after a command. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Preserve seeded selection and keep tells observational; no hidden price or trust delta is introduced by this plan. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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

> The v1.0 master bible was a snapshot: a large, well-structured reference that a planner reads before drafting one plan. Its structural weakness, identified during the 2026-09-24 audit, is that it is a *library*, not a *machine*. It tells a planner what exists, but it does not encode the generative move — the repeatable transformation of (repository evidence × lane × subsystem) into a bounded subject plan with a recommended integration route.

> 1. **The Drift Register (Part I):** a live-audit correction layer. The repository has moved since the v1.0 snapshot; every plan drafted against stale premises is wasted work. The register lists what changed, with evidence and confidence labels.
2. **The Factory Protocol (Part II):** the operating loop that converts evidence into subject plans. It is deterministic, like everything else in this project: same inputs, same plan shape, same verification demands.
3. **The Generator Matrices (Part III):** the combinatorial core. Ten expansion lanes × seventeen subsystem clusters, with per-cell opening archetypes. This is the mechanism by which one document yields hundreds of expansion plans without inventing duplicate systems.
4. **The Seeded Backlog (Part IV) and Templates (Part V):** audit-derived candidate expansions, each with a subject, evidence, confidence, and best integration route; plus the wave-charter, subject-plan, and verification templates the repository already uses, extended for factory output.

> **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
Observed live and not listed in v1.0 Part 5.8: `ECONOMY_FAIRNESS_AUDIT.md`, `ENGINE_SUPPORT_POLICY.md`, `GODOT_MIGRATION_STATUS.md`, `REPO_HISTORY_REWRITE.md`, `HUMAN_AUTHORSHIP.md`, `AI_DISCLOSURE.md`, `ASSET_MIGRATION_LEDGER.md`, `CODEX_SOURCE_MATRIX.md`, `ARCHIVE_INDEX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md`, `SHELTER_MAINTENANCE_MATRIX.md`, `SHELTER_30_DAY_MAINTENANCE_REPORT.md`, `L10N_WAVE2_ROADMAP.md`, `INPUT.md`, `RELEASE_EXPORT.md`, `ENGINE_SUPPORT_POLICY.md`. Of these, `ECONOMY_FAIRNESS_AUDIT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, and `SHELTER_MAINTENANCE_MATRIX.md` are pre-computed balance baselines: they convert Lane C (economy and balance) planning from speculative to evidence-anchored. Subject plans in Lane C must cite these baselines instead of re-deriving numbers.

> **DR-06 — Integration ledger state differs from the v1.0 queue snapshot. VERIFIED.**
Live `INTEGRATION_PLANS.md` (read 2026-09-24) shows, at minimum: the **XP Expansion W1** batch ACTIVE (difficulty authority package `XP-WAVE1-DIFFICULTY-AUTHORITY`, with a premise correction recorded against Plan 122 SOFC fuel); the **DISTRESS-SIGNALS-9-12** flagship COMPLETE and presented for acceptance; **Plan 24 CLOSED** (Wave 8, 2026-09-17, signatures resolved 2026-09-18, ward staffing sealed under option b, `DEBT-PLAN24-MEDICAL-WARD-STAFFING` RETIRED); **19A/19B/19C** waves closed with evidence (Endgame 84/84 PASS, focused suites 47 PASS, `verify-fast.sh` reported ALL 47 GATES PASSED); **C2[2] Plan 17** legibility executed with a documented not-executed list (Plan 31 semantic-kind authority, 17C audio phases, 17B deep test matrix remain open gaps); **PR 3 content seal SEALED 2026-09-19** (`CF-P1-DISTRESS-CONTENT-SEAL`); the **availability consumer RETIRED** (Wave 9 Part 2, Option B approved; `SignalTrustAvailability` retained as a pure-math specification pin). Consequence: subject plans in the radio/distress domain must treat the rescue-signal runtime as *sealed and closed*, not as an open expansion surface, unless they extend it through its recorded seams.

> **DR-10 — v1.0 items the audit could not confirm in this pass. UNVERIFIED.**
Not confirmed in this audit pass (single-session, listing-level access): the 11,697 test total; the D1 seal state; the full 57-gate inventory; codec version pin values; the `ClaimPersonalBelonging` no-caller status; decision-blocked item states beyond those the ledger records as resolved. Each of these remains plausible but must be re-verified in live source before any plan depends on it. Factory rule: UNVERIFIED premises get a verification step inside the plan, never silent trust.

> This protocol is the heart of v2.0. It converts repository evidence into subject plans, deterministically, and it is designed to be executed by any future planning session (human or LLM) without re-deriving the method. One execution of the protocol yields one subject plan; the matrices in Part III provide the candidate space; the backlog in Part IV holds pre-audited candidates.

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

The subject is a deterministic behavioral-observation layer inside the existing trade presenter. The plan expands content integrity, replay, mechanical boundaries and player-facing truthfulness without turning flavor into hidden balance.

- **band/pool parsing and deterministic tell selection** remains with `TradeTellEngine` at `Assets/Ashfall.Core/Economy/TradeTellEngine.cs`. Owns tell content selection; it does not own market price or faction standing.
- **current trade projection and command context** remains with `TradeScreenPresenter` at `Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs`. Injects tell provider/RNG and projects the view model.
- **scenario/preview binding** remains with `TradeScreenScenarios` at `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs`. Uses the tell provider for current trade presentation and tests.
- **stance, trust and transaction state** remains with `Faction/settlement trade owners` at `Assets/Ashfall.Core/Economy/MarketSystem.cs; Assets/Ashfall.Core/World/SettlementCatalog.cs`. Current mechanical context; tells do not duplicate it.
- **player-visible table and caravan actions** remains with `Trade UI/host route` at `src/UI/CaravanBarterLedgerPanel.cs; src/Main.Economy.cs`. Presentation/commands only; exact route must be verified.
- **corpus, seam and replay proof** remains with `Trade focused tests` at `Ashfall.Core.Tests/TradeTellCorpusTests.cs; Ashfall.Core.Tests/Radiation/Plan81_62DoseTradeTellIntegrationTests.cs; Ashfall.Core.Tests/TradeScreenSeamTests.cs`. Focused evidence surface.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load trade tell catalog through the current engine
2. bind provider and seeded RNG into trade presenter/scenario
3. read current stance/trust from the existing trade owner
4. resolve the first trust band
5. select a line from the matching stance/band pool
6. project the observation beside the current offer/command state
7. leave price, inventory and faction state owned by their existing commands

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Tell lines and trust bands are immutable catalog data; selected tell is a transient deterministic projection.
- Band resolution is first-match inclusive; missing pool returns false rather than silently borrowing another stance’s voice.
- The injected RNG, not wall-clock or panel timing, controls rotation.
- Mechanical trade state remains owned by the existing market/settlement/transaction seams.

- Every stance/band pool is non-empty and line text is trimmed before registration.
- A trust value at a boundary belongs to the documented inclusive band; gaps are visible as an empty selection.
- A missing stance/band returns false and does not mutate the trade view or RNG state unexpectedly.
- Same seed, trust, stance and catalog state produce the same tell ID/line.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/UI/CaravanBarterLedgerPanel.cs
- src/Main.Economy.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/TradeTellCorpusTests.cs
- Ashfall.Core.Tests/Radiation/Plan81_62DoseTradeTellIntegrationTests.cs
- Ashfall.Core.Tests/TradeScreenSeamTests.cs

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
| S-01 | 62-01 load 60 lines | load trade tell catalog through the current engine | Tell lines and trust bands are immutable catalog data; selected tell is a transient deterministic projection. | A missing pool falls back to a different stance/band line. | TradeTellEngine |
| S-02 | 62-02 first trust band boundary | bind provider and seeded RNG into trade presenter/scenario | Band resolution is first-match inclusive; missing pool returns false rather than silently borrowing another stance’s voice. | A panel changes price/trust based on text without a command. | TradeTellEngine |
| S-03 | 62-03 last trust band boundary | read current stance/trust from the existing trade owner | The injected RNG, not wall-clock or panel timing, controls rotation. | Two hosts use different RNG streams for the same transaction. | TradeTellEngine |
| S-04 | 62-04 missing stance pool | resolve the first trust band | Mechanical trade state remains owned by the existing market/settlement/transaction seams. | A line claims a hidden fact or copied real-world voice. | TradeTellEngine |
| S-05 | 62-05 empty catalog | select a line from the matching stance/band pool | Tell lines and trust bands are immutable catalog data; selected tell is a transient deterministic projection. | A new negotiation engine duplicates the market owner. | TradeTellEngine |
| S-06 | 62-06 seeded selection replay | project the observation beside the current offer/command state | Band resolution is first-match inclusive; missing pool returns false rather than silently borrowing another stance’s voice. | A missing pool falls back to a different stance/band line. | TradeTellEngine |
| S-07 | 62-07 presenter view-model projection | leave price, inventory and faction state owned by their existing commands | The injected RNG, not wall-clock or panel timing, controls rotation. | A panel changes price/trust based on text without a command. | TradeTellEngine |
| S-08 | 62-08 caravan refresh without mutation | load trade tell catalog through the current engine | Mechanical trade state remains owned by the existing market/settlement/transaction seams. | Two hosts use different RNG streams for the same transaction. | TradeTellEngine |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 62-TC-01 JSON shape and pool count | data | JSON shape and pool count; verify the current owner and its negative boundary without inventing a second authority. | TradeTellEngine |
| T-02 | 62-TC-02 five stance key mapping | unit | five stance key mapping; verify the current owner and its negative boundary without inventing a second authority. | TradeTellEngine |
| T-03 | 62-TC-03 four band ranges | persistence | four band ranges; verify the current owner and its negative boundary without inventing a second authority. | TradeTellEngine |
| T-04 | 62-TC-04 inclusive boundary behavior | determinism | inclusive boundary behavior; verify the current owner and its negative boundary without inventing a second authority. | TradeTellEngine |
| T-05 | 62-TC-05 line trim/nonempty validation | host | line trim/nonempty validation; verify the current owner and its negative boundary without inventing a second authority. | TradeTellEngine |
| T-06 | 62-TC-06 missing pool false result | UI/accessibility | missing pool false result; verify the current owner and its negative boundary without inventing a second authority. | TradeTellEngine |
| T-07 | 62-TC-07 ISeededRng selection | cross-system | ISeededRng selection; verify the current owner and its negative boundary without inventing a second authority. | TradeTellEngine |
| T-08 | 62-TC-08 same-seed replay | data | same-seed replay; verify the current owner and its negative boundary without inventing a second authority. | TradeTellEngine |
| T-09 | 62-TC-09 presenter injection | unit | presenter injection; verify the current owner and its negative boundary without inventing a second authority. | TradeTellEngine |
| T-10 | 62-TC-10 scenario binding | persistence | scenario binding; verify the current owner and its negative boundary without inventing a second authority. | TradeTellEngine |
| T-11 | 62-TC-11 no market mutation | determinism | no market mutation; verify the current owner and its negative boundary without inventing a second authority. | TradeTellEngine |
| T-12 | 62-TC-12 UI tone/accessibility review | host | UI tone/accessibility review; verify the current owner and its negative boundary without inventing a second authority. | TradeTellEngine |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 10 | `Ashfall.Core.Tests/Economy/TradeScreenPresenterSnapshotTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Assets/Ashfall.Core/Economy/TradeTellEngine.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `src/Host/OrphanSealWave1HostSessions.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/TradeScreenSeamTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Assets/Ashfall.Core/Random/CampaignRngStream.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/TradeTellCorpusTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Assets/Ashfall.Core/Survivors/PsychologicalArcSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/Radiation/Plan81_62DoseTradeTellIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/MicroLocationDeterminismHarness.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Phantoms/PhantomMemoryHostSessionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/WildlifeTrappingCatalogTestFixture.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Combat/EnemyCompositionSelector.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Damage.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Realtime.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Economy/MarketSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Expeditions/DraisineRerailingSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Legacy/CampaignLegacySystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Radio/FactionRadioEngine.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/PhantomMemoryHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/Plans74To77HostSessions.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/PowerGridHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Main.Plans146_149.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/trade_tell_lines.json`

### `Assets/StreamingAssets/Data/trade_tell_lines.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 8811; characters: 8811.
- SHA-256: `a86a8e9486d67d14d4ed4df63a47a32ecc25988d2287f822bb57754376b28b08`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `$schema`, `version`, `description`, `trust_bands`, `tells`

#### `trust_bands` — 4 current rows

- Row 001 `hostile`: `{"id":"hostile","max":-40,"min":-100}`
- Row 002 `wary`: `{"id":"wary","max":0,"min":-39}`
- Row 003 `neutral`: `{"id":"neutral","max":40,"min":1}`
- Row 004 `warm`: `{"id":"warm","max":100,"min":41}`

#### `tells` — 5 keyed entries

- Entry 001 `hostile_raid`: `{"hostile":["The table is a formality. Their hands never leave the rifle slings.","They have already decided what they are taking. The barter is theater.","Nobody sits on your side of the table anymore."],"neutral":["They talk trade, but t…`
- Entry 002 `rob`: `{"hostile":["Whatever you set down, they intend to keep. All of it.","They weigh your pack, not your offer.","The quartermaster's fingers drum the counter like a countdown."],"neutral":["The stall is open, but the scale is loaded.","They n…`
- Entry 003 `refuse`: `{"hostile":["The stall shutter is down. It does not open for you.","They fold their arms and wait for you to understand.","Nothing you carry is worth the conversation."],"neutral":["The shutter is half-open. Not today, they say. Not to you…`
- Entry 004 `trade`: `{"hostile":["They keep their goods close and leave little space between you.","One hand stays beneath the table while they study the offer.","Their attention keeps drifting from your hands to the doorway.","They count their goods twice and…`
- Entry 005 `share_intel`: `{"hostile":["They trade, they talk, and none of it can be trusted twice.","The intel comes fast and cheap, like they want you gone.","Every warning they share is also a test of what you fear."],"neutral":["Between items, they mention which…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/trade_screen_scenarios.json`

### `Assets/StreamingAssets/Data/trade_screen_scenarios.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 23775; characters: 23775.
- SHA-256: `fc2d88b9a5cbe663c054a25226096a59dc259089b01d4521952314d6c37e507c`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `$schema`, `version`, `description`, `scenarios`

#### `scenarios` — 15 current rows

- Row 001 `fair_deal`: `{"aggression":0.35,"biological_offers":{"PintOfBlood":1},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Clean Water","item_id":"clean_water","quantity…`
- Row 002 `offer_short`: `{"aggression":0.6,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":false,"consecutive_repels":2,"expected_fairness":"short","faction_demands":[{"display_name":"Fuel","item_id":"fuel","quantity":2,"unit_price":40}],"facti…`
- Row 003 `empty_table`: `{"aggression":0.2,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":false,"consecutive_repels":0,"expected_fairness":"empty","faction_demands":[],"faction_id":"rot_farmers","faction_name":"Rot Farmers","has_surrendered":f…`
- Row 004 `last_vials`: `{"aggression":0.15,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Antibiotics","item_id":"antibiotics","quantity":1,"unit_price…`
- Row 005 `winter_cart`: `{"aggression":0.25,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":false,"consecutive_repels":1,"expected_fairness":"short","faction_demands":[{"display_name":"Heavy Wool Coat","item_id":"item_heavy_wool_coat","quantity…`
- Row 006 `depot_window`: `{"aggression":0.3,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Gas Mask","item_id":"gas_mask","quantity":1,"unit_price":55},{…`
- Row 007 `emergency_requisition`: `{"aggression":0.7,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":false,"consecutive_repels":1,"expected_fairness":"short","faction_demands":[{"display_name":"Military Radio","item_id":"military_radio","quantity":1,"uni…`
- Row 008 `back_room_exchange`: `{"aggression":0.4,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Rad-Away","item_id":"rad_away","quantity":1,"unit_price":38},{…`
- Row 009 `ledgerless_broker`: `{"aggression":0.3,"biological_offers":{"BoneMarrow":1},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Comm Codebook Alpha","item_id":"item_comm_codebo…`
- Row 010 `long_road_caravan`: `{"aggression":0.2,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Canned Grain Stew","item_id":"item_canned_grain_stew","quantit…`
- Row 011 `salvage_caravan`: `{"aggression":0.25,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Steel Rail Segment","item_id":"steel_rail_segment","quantity"…`
- Row 012 `settlement_of_accounts`: `{"aggression":0.5,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":false,"consecutive_repels":0,"expected_fairness":"short","faction_demands":[{"display_name":"Canned Food","item_id":"canned_food","quantity":2,"unit_pric…`
- Row 013 `crate_lot`: `{"aggression":0.2,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Canned Grain Stew","item_id":"item_canned_grain_stew","quantit…`
- Row 014 `border_runner`: `{"aggression":0.45,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Military Radio Module","item_id":"item_military_radio_module"…`
- Row 015 `road_knowledge`: `{"aggression":0.1,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Road Map","item_id":"item_collectible_road_map","quantity":1,"…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/faction_territory.json`

### `Assets/StreamingAssets/Data/faction_territory.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 19950; characters: 19950.
- SHA-256: `ef0940bbe98f3082b75cbbef77df670e7e2c7358b89ddeefc20b8fd119b90915`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `collection_id`, `territories`, `contested_zones`

#### `territories` — 19 current rows

- Row 001 `territory_the_office`: `{"classification":"territorial","contested_with":["faction_the_tally","faction_scavenger_guild"],"control_points":["loc_settlement_nine_rails","loc_weighbridge"],"control_strength":85,"controlled_nodes":["loc_cut_arsenal_ruin"],"descriptio…`
- Row 002 `territory_the_cutters`: `{"classification":"territorial","contested_with":["faction_undertow","faction_black_flotilla"],"control_points":["loc_settlement_brine_pans","loc_the_shallows_market"],"control_strength":70,"controlled_nodes":["loc_cut_radiation_zone_alpha…`
- Row 003 `territory_black_flotilla`: `{"classification":"mixed","contested_with":["faction_the_fleet","faction_the_cutters"],"control_points":["loc_settlement_cape_beacon","loc_black_flotilla_outpost"],"control_strength":80,"controlled_nodes":["loc_black_flotilla_outpost"],"de…`
- Row 004 `territory_the_fleet`: `{"classification":"nomadic","contested_with":["faction_black_flotilla","faction_undertow"],"control_points":["loc_black_flotilla_outpost","loc_lock_gate_four"],"control_strength":60,"controlled_nodes":["loc_black_flotilla_outpost"],"descri…`
- Row 005 `territory_deserter_coalition`: `{"classification":"territorial","contested_with":["faction_iron_raiders","faction_the_provisioned"],"control_points":["loc_settlement_iron_siding","loc_settlement_fort_karkov"],"control_strength":90,"controlled_nodes":["loc_cut_abandoned_d…`
- Row 006 `territory_cold_count`: `{"classification":"territorial","contested_with":["faction_the_tally","faction_long_walk"],"control_points":["loc_settlement_slate_hollow","loc_low_background_lab"],"control_strength":65,"controlled_nodes":["loc_cut_arsenal_ruin"],"descrip…`
- Row 007 `territory_the_tally`: `{"classification":"territorial","contested_with":["faction_undertow","faction_the_office"],"control_points":["loc_settlement_lock_seven","loc_lock_gate_four"],"control_strength":75,"controlled_nodes":["loc_cut_radiation_zone_alpha"],"descr…`
- Row 008 `territory_grain_exchange`: `{"classification":"territorial","contested_with":["faction_iron_raiders","faction_the_tally"],"control_points":["loc_settlement_silo_burrow","loc_grain_silo"],"control_strength":70,"controlled_nodes":["loc_cut_merchant_caravanserai"],"desc…`
- Row 009 `territory_quiet_house`: `{"classification":"ideological","contested_with":["faction_osteophages","faction_iron_raiders"],"control_points":["loc_settlement_st_nicholas","loc_shrine_switchback_waystation"],"control_strength":50,"controlled_nodes":["loc_black_flotill…`
- Row 010 `territory_scavenger_guild`: `{"classification":"territorial","contested_with":["faction_iron_raiders","faction_the_office"],"control_points":["loc_settlement_tinkers_notch","loc_cut_merchant_caravanserai"],"control_strength":75,"controlled_nodes":["loc_cut_merchant_ca…`
- Row 011 `territory_long_walk`: `{"classification":"nomadic","contested_with":["faction_sun_seekers","faction_cold_count"],"control_points":["loc_settlement_pilgrim_hearth","loc_shrine_switchback_waystation"],"control_strength":45,"controlled_nodes":["loc_cut_merchant_car…`
- Row 012 `territory_undertow`: `{"classification":"territorial","contested_with":["faction_the_cutters","faction_the_tally"],"control_points":["loc_settlement_ferry_crossing","loc_water_station"],"control_strength":65,"controlled_nodes":["loc_cut_abandoned_depot"],"descr…`
- Row 013 `territory_hydro_barons`: `{"classification":"territorial","contested_with":["faction_undertow","faction_grain_exchange"],"control_points":["loc_water_station","loc_terrace_pumphouse"],"control_strength":80,"controlled_nodes":["loc_holdfast"],"description":"The mass…`
- Row 014 `territory_iron_raiders`: `{"classification":"nomadic","contested_with":["faction_scavenger_guild","faction_deserter_coalition"],"control_points":["loc_cut_abandoned_depot","loc_cut_arsenal_ruin"],"control_strength":60,"controlled_nodes":["loc_cut_abandoned_depot","…`
- Row 015 `territory_the_provisioned`: `{"classification":"territorial","contested_with":["faction_deserter_coalition","faction_scavenger_guild"],"control_points":["loc_excavation_command_vault","loc_logistics_reserve_cache"],"control_strength":90,"controlled_nodes":["loc_hidden…`
- Row 016 `territory_archivists`: `{"classification":"ideological","contested_with":["faction_the_office","faction_the_tally"],"control_points":["loc_excavation_archive_bunker","loc_hidden_relay_bunker"],"control_strength":40,"controlled_nodes":["loc_logistics_reserve_cache…`
- Row 017 `territory_lamplighters`: `{"classification":"nomadic","contested_with":["faction_iron_raiders","faction_undertow"],"control_points":["loc_cut_merchant_caravanserai","loc_cut_abandoned_depot"],"control_strength":50,"controlled_nodes":["loc_cut_merchant_caravanserai"…`
- Row 018 `territory_sun_seekers`: `{"classification":"nomadic","contested_with":["faction_long_walk","faction_osteophages"],"control_points":["loc_cut_radiation_zone_alpha","loc_broadcast_bunker_echo"],"control_strength":40,"controlled_nodes":["loc_cut_radiation_zone_alpha"…`
- Row 019 `territory_osteophages`: `{"classification":"territorial","contested_with":["faction_quiet_house","faction_the_cutters"],"control_points":["loc_cut_radiation_zone_alpha","loc_excavation_mine_shaft"],"control_strength":55,"controlled_nodes":["loc_cut_radiation_zone_…`

#### `contested_zones` — 5 current rows

- Row 001 `zone_contested_water_rights`: `{"claimant_factions":["faction_hydro_barons","faction_undertow","faction_the_cutters"],"conflict_driver":"Hydro-Barons seek metered pipeline monopoly; Undertow controls river barge transit; Cutters require brine flow for salt pans.","dispu…`
- Row 002 `zone_contested_cut_salvage`: `{"claimant_factions":["faction_scavenger_guild","faction_iron_raiders","faction_deserter_coalition"],"conflict_driver":"Scavenger Guild claims unstripped machine tooling; Iron Raiders ambush salvage convoys; Deserters fortify rail sidings …`
- Row 003 `zone_contested_merchant_crossroads`: `{"claimant_factions":["faction_the_office","faction_the_tally","faction_scavenger_guild"],"conflict_driver":"The Office attempts freight tariff enforcement; The Tally demands debt seizure rights; Scavenger Guild defends open non-chartered …`
- Row 004 `zone_contested_scarp_pass`: `{"claimant_factions":["faction_long_walk","faction_cold_count","faction_quiet_house"],"conflict_driver":"Long Walk pilgrims require unimpeded seasonal circuit; Cold Count seals radiation baseline lab; Quiet House maintains non-violent spri…`
- Row 005 `zone_contested_coastal_bluff`: `{"claimant_factions":["faction_black_flotilla","faction_the_fleet","faction_the_cutters"],"conflict_driver":"Black Flotilla claims raised shipwreck salvage; The Fleet asserts pre-war naval jurisdiction; Cutters contest coastal brine access…`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Economy/TradeTellEngine.cs`

### `Assets/Ashfall.Core/Economy/TradeTellEngine.cs` — complete current file

- Size: 213 lines / 8276 bytes.
- SHA-256: `ca0b50673b96d37a7d4a74f100dc0e7a3403ec2738106837d728eba22da784cb`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: namespace Ashfall.Core.Economy
00003: {
00004:     using System;
00005:     using System.Collections.Generic;
00006:     using System.Text.Json;
00007:
00008:     /// <summary>
00009:     /// Trust band the trader falls into at the table. Bands are data-defined
00010:     /// (trade_tell_lines.json); these are the canonical ids.
00011:     /// </summary>
00012:     public static class TradeTrustBands
00013:     {
00014:         public const string Hostile = "hostile";
00015:         public const string Wary = "wary";
00016:         public const string Neutral = "neutral";
00017:         public const string Warm = "warm";
00018:     }
00019:
00020:     /// <summary>One selected tell: the trader's readable posture at the table.</summary>
00021:     public sealed class TradeTell
00022:     {
00023:         public string Id { get; }
00024:         public TradeStance Stance { get; }
00025:         public string Band { get; }
00026:         public string Line { get; }
00027:
00028:         public TradeTell(string id, TradeStance stance, string band, string line)
00029:         {
00030:             Id = id ?? string.Empty;
00031:             Stance = stance;
00032:             Band = band ?? string.Empty;
00033:             Line = line ?? string.Empty;
00034:         }
00035:     }
00036:
00037:     /// <summary>
00038:     /// Tell selection surface: stance × trust band → tell id + line.
00039:     /// Data-defined catalog, deterministic rotation via ISeededRng.
00040:     /// </summary>
00041:     public interface ITradeTellProvider
00042:     {
00043:         bool TrySelectTell(TradeStance stance, float trust, ISeededRng rng, out TradeTell tell);
00044:         string BandForTrust(float trust);
00045:     }
00046:
00047:     /// <summary>
00048:     /// The tell-line corpus engine. Same pattern as FactionRadioEngine: JSON
00049:     /// in StreamingAssets is the authority, selection is seed-deterministic,
00050:     /// and the engine is pure C# with zero host references.
00051:     /// </summary>
00052:     public sealed class TradeTellEngine : ITradeTellProvider
00053:     {
00054:         private readonly List<(string Id, float MinInclusive, float MaxInclusive)> _bands = new();
00055:         private readonly Dictionary<string, List<string>> _pools = new(StringComparer.Ordinal);
00056:         private readonly Dictionary<string, TradeStance> _stances = new(StringComparer.Ordinal);
00057:
00058:         public int BandCount => _bands.Count;
00059:         public int PoolCount => _pools.Count;
00060:         public int LineCount { get; private set; }
00061:
00062:         public IReadOnlyList<string> Bands
00063:         {
00064:             get
00065:             {
00066:                 var ids = new List<string>();
00067:                 foreach (var band in _bands) ids.Add(band.Id);
00068:                 return ids;
00069:             }
00070:         }
00071:
00072:         public void RegisterBand(string id, float minInclusive, float maxInclusive)
00073:         {
00074:             if (string.IsNullOrWhiteSpace(id)) return;
00075:             _bands.Add((id.Trim(), minInclusive, maxInclusive));
00076:         }
00077:
00078:         public void RegisterTellPool(TradeStance stance, string bandId, IEnumerable<string> lines)
00079:         {
00080:             if (string.IsNullOrWhiteSpace(bandId) || lines == null) return;
00081:             string key = PoolKey(stance, bandId.Trim());
00082:             var pool = new List<string>();
00083:             foreach (var line in lines)
00084:             {
00085:                 if (!string.IsNullOrWhiteSpace(line))
00086:                 {
00087:                     pool.Add(line.Trim());
00088:                 }
00089:             }
00090:             _pools[key] = pool;
00091:             _stances[key] = stance;
00092:             LineCount += pool.Count;
00093:         }
00094:
00095:         /// <summary>Ordered scan: first band whose [min, max] range contains the trust value.</summary>
00096:         public string BandForTrust(float trust)
00097:         {
00098:             foreach (var band in _bands)
00099:             {
00100:                 if (trust >= band.MinInclusive && trust <= band.MaxInclusive) return band.Id;
00101:             }
00102:             return _bands.Count > 0 ? _bands[_bands.Count - 1].Id : TradeTrustBands.Neutral;
00103:         }
00104:
00105:         public bool TrySelectTell(TradeStance stance, float trust, ISeededRng rng, out TradeTell tell)
00106:         {
00107:             string band = BandForTrust(trust);
00108:             string key = PoolKey(stance, band);
00109:             if (!_pools.TryGetValue(key, out var pool) || pool.Count == 0)
00110:             {
00111:                 tell = null!;
00112:                 return false;
00113:             }
00114:
00115:             int index = pool.Count <= 1 ? 0 : (rng != null ? rng.Next(0, pool.Count) : 0);
00116:             tell = new TradeTell(
00117:                 id: $"{StanceKey(stance)}_{band}_{index}",
00118:                 stance: stance,
00119:                 band: band,
00120:                 line: pool[index]);
00121:             return true;
00122:         }
00123:
00124:         /// <summary>Raw pool access for corpus lint/tests. Pool is keyed by stance + band id.</summary>
00125:         public bool TryGetPoolLines(TradeStance stance, string bandId, out IReadOnlyList<string> lines)
00126:         {
00127:             if (_pools.TryGetValue(PoolKey(stance, bandId ?? string.Empty), out var pool))
00128:             {
00129:                 lines = pool;
00130:                 return true;
00131:             }
00132:             lines = Array.Empty<string>();
00133:             return false;
00134:         }
00135:
00136:         private static string PoolKey(TradeStance stance, string bandId)
00137:         {
00138:             return StanceKey(stance) + "/" + (bandId ?? string.Empty).Trim().ToLowerInvariant();
00139:         }
00140:
00141:         private static string StanceKey(TradeStance stance)
00142:         {
00143:             switch (stance)
00144:             {
00145:                 case TradeStance.HostileRaid: return "hostile_raid";
00146:                 case TradeStance.Rob: return "rob";
00147:                 case TradeStance.Refuse: return "refuse";
00148:                 case TradeStance.ShareIntel: return "share_intel";
00149:                 case TradeStance.Trade:
00150:                 default: return "trade";
00151:             }
00152:         }
00153:
00154:         /// <summary>
00155:         /// Loads the tell corpus from raw JSON text:
00156:         /// { "trust_bands": [{id,min,max}...], "tells": { "trade": { "warm": [lines...] } } }
00157:         /// </summary>
00158:         public static TradeTellEngine LoadFromJson(string json)
00159:         {
00160:             var engine = new TradeTellEngine();
00161:             if (string.IsNullOrWhiteSpace(json)) return engine;
00162:
00163:             using var doc = JsonDocument.Parse(json);
00164:             var root = doc.RootElement;
00165:
00166:             if (root.TryGetProperty("trust_bands", out var bandsProp) && bandsProp.ValueKind == JsonValueKind.Array)
00167:             {
00168:                 foreach (var b in bandsProp.EnumerateArray())
00169:                 {
00170:                     string? id = b.TryGetProperty("id", out var idEl) ? idEl.GetString() : null;
00171:                     float min = b.TryGetProperty("min", out var minEl) ? (float)minEl.GetDouble()! : -100f;
00172:                     float max = b.TryGetProperty("max", out var maxEl) ? (float)maxEl.GetDouble() : 100f;
00173:                     engine.RegisterBand(id!, min, max);
00174:                 }
00175:             }
00176:
00177:             if (root.TryGetProperty("tells", out var tellsProp) && tellsProp.ValueKind == JsonValueKind.Object)
00178:             {
00179:                 foreach (var stanceProp in tellsProp.EnumerateObject())
00180:                 {
00181:                     if (!TryParseStance(stanceProp.Name, out var stance)) continue;
00182:                     if (stanceProp.Value.ValueKind != JsonValueKind.Object) continue;
00183:
00184:                     foreach (var bandProp in stanceProp.Value.EnumerateObject())
00185:                     {
00186:                         if (bandProp.Value.ValueKind != JsonValueKind.Array) continue;
00187:                         var lines = new List<string>();
00188:                         foreach (var line in bandProp.Value.EnumerateArray())
00189:                         {
00190:                             lines.Add(line.GetString() ?? string.Empty);
00191:                         }
00192:                         engine.RegisterTellPool(stance, bandProp.Name, lines);
00193:                     }
00194:                 }
00195:             }
00196:
00197:             return engine;
00198:         }
00199:
00200:         private static bool TryParseStance(string key, out TradeStance stance)
00201:         {
00202:             switch ((key ?? string.Empty).Trim())
00203:             {
00204:                 case "hostile_raid": stance = TradeStance.HostileRaid; return true;
00205:                 case "rob": stance = TradeStance.Rob; return true;
00206:                 case "refuse": stance = TradeStance.Refuse; return true;
00207:                 case "trade": stance = TradeStance.Trade; return true;
00208:                 case "share_intel": stance = TradeStance.ShareIntel; return true;
00209:                 default: stance = TradeStance.Trade; return false;
00210:             }
00211:         }
00212:     }
00213: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs`

### `Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs` — bounded current excerpt (432 of 467 lines)

- Size: 467 lines / 18043 bytes.
- SHA-256: `eaba535609c3ce04e7628d9930b6667dbc223dc4c5fa83db4e78bfa2292387eb`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: namespace Ashfall.Core.Economy
00003: {
00004:     using System;
00005:     using System.Collections.Generic;
00006:     using System.Text;
00007:     using Ashfall.Core.Radio;
00008:
00009:     public sealed class TradeSelectionSnapshot
00010:     {
00011:         public Dictionary<string, int> PlayerOffers { get; set; } = new(StringComparer.Ordinal);
00012:         public Dictionary<string, int> FactionAsks { get; set; } = new(StringComparer.Ordinal);
00013:         public Dictionary<BiologicalTradeItem, int> BiologicalOffers { get; set; } = new();
00014:     }
00015:
00016:     /// <summary>
00017:     /// Track B (Nerves): maps the frozen core interfaces
00018:     /// (IFactionStanceProvider / IPriceShockProvider) onto the Act 0 seam
00019:     /// (ITradeScreenViewModel) and exposes the existing TradeScreenUI surface
00020:     /// (Open, Close, SetPlayerOffer, SetFactionAsk, Recalculate, TryConfirmTrade,
00021:     /// TryDemandParley, BuildQuoteSummary) for API parity.
00022:     ///
00023:     /// Zero-mutation invariant: the presenter only ever calls read methods on
00024:     /// the stance provider (Get*/WillTrade/TryGet*). Trust, aggression and all
00025:     /// other simulation state are never written through this class; execution
00026:     /// is routed exclusively through the injected ITradeExecutionSink.
00027:     /// </summary>
00028:     public sealed class TradeScreenPresenter : ITradeIntentSink
00029:     {
00030:         private readonly IFactionStanceProvider _stance;
00031:         private readonly IPriceShockProvider _shocks;
00032:         private readonly ITradeTellProvider _tells;
00033:         private readonly IFactionRadioProvider? _radio;
00034:         private readonly ITradeExecutionSink _execution;
00035:         private readonly ISeededRng _rng;
00036:         private readonly Func<string, float> _unitPrice;
00037:         private readonly Func<string, string> _displayName;
00038:         private readonly TradeVoiceResolver? _voiceResolver;
00039:         private TradeVoiceContext _voiceContext = new TradeVoiceContext();
00040:
00041:         private readonly Dictionary<string, int> _playerOfferCounts = new();
00042:         private readonly Dictionary<string, int> _factionAskCounts = new();
00043:         private readonly Dictionary<BiologicalTradeItem, int> _bioOfferCounts = new();
00044:         private readonly List<string> _watchedItems = new();
00045:
00046:         private string _worldPhaseLabel = string.Empty;
00047:         private int _worldDay = 1;
00048:
00049:         public TradeScreenViewModel ViewModel { get; } = new TradeScreenViewModel();
00050:
00051:         public int ActiveOfferCount => _playerOfferCounts.Count;
00052:         public int ActiveAskCount => _factionAskCounts.Count;
00053:         public int ActiveBioCount => _bioOfferCounts.Count;
00054:
00055:         public int GetPlayerOfferCount(string itemId) =>
00056:             _playerOfferCounts.TryGetValue(itemId, out int count) ? count : 0;
00057:
00058:         public int GetFactionAskCount(string itemId) =>
00059:             _factionAskCounts.TryGetValue(itemId, out int count) ? count : 0;
00060:
00061:         public int GetBiologicalOfferCount(BiologicalTradeItem item) =>
00062:             _bioOfferCounts.TryGetValue(item, out int count) ? count : 0;
00063:
00064:         public TradeSelectionSnapshot CaptureSelection()
00065:         {
00066:             return new TradeSelectionSnapshot
00067:             {
00068:                 PlayerOffers = new Dictionary<string, int>(_playerOfferCounts, StringComparer.Ordinal),
00069:                 FactionAsks = new Dictionary<string, int>(_factionAskCounts, StringComparer.Ordinal),
00070:                 BiologicalOffers = new Dictionary<BiologicalTradeItem, int>(_bioOfferCounts)
00071:             };
00072:         }
00073:
00074:         public void RestoreSelection(TradeSelectionSnapshot snapshot)
00075:         {
00076:             if (snapshot == null) return;
00077:             _playerOfferCounts.Clear();
00078:             if (snapshot.PlayerOffers != null)
00079:             {
00080:                 foreach (var pair in snapshot.PlayerOffers)
00081:                 {
00082:                     if (pair.Value > 0) _playerOfferCounts[pair.Key] = pair.Value;
00083:                 }
00084:             }
00085:
00086:             _factionAskCounts.Clear();
00087:             if (snapshot.FactionAsks != null)
00088:             {
00089:                 foreach (var pair in snapshot.FactionAsks)
00090:                 {
00091:                     if (pair.Value > 0) _factionAskCounts[pair.Key] = pair.Value;
00092:                 }
00093:             }
00094:
00095:             _bioOfferCounts.Clear();
00096:             if (snapshot.BiologicalOffers != null)
00097:             {
00098:                 foreach (var pair in snapshot.BiologicalOffers)
00099:                 {
00100:                     if (pair.Value > 0) _bioOfferCounts[pair.Key] = pair.Value;
00105:         }
00106:
00107:         public TradeScreenPresenter(
00108:             IFactionStanceProvider stanceProvider,
00109:             IPriceShockProvider? priceShockProvider = null,
00110:             ITradeTellProvider? tells = null,
00111:             ISeededRng? rng = null,
00112:             Func<string, float>? unitPriceLookup = null,
00113:             Func<string, string>? displayNameLookup = null,
00114:             ITradeExecutionSink? executionSink = null,
00115:             IFactionRadioProvider? radioProvider = null,
00116:             TradeVoiceResolver? voiceResolver = null)
00117:         {
00118:             _stance = stanceProvider;
00119:             _shocks = priceShockProvider;
00120:             _tells = tells;
00121:             _radio = radioProvider;
00122:             _rng = rng;
00123:             _execution = executionSink;
00124:             _unitPrice = unitPriceLookup ?? (_ => 10f);
00125:             _displayName = displayNameLookup ?? (id => (id ?? string.Empty).Replace('_', ' '));
00126:             _voiceResolver = voiceResolver;
00127:         }
00128:
00129:         /// <summary>
00130:         /// Sets presentation context only. The context is copied into the
00131:         /// resolver on refresh; it never becomes trade or faction state.
00132:         /// </summary>
00133:         public void SetVoiceContext(TradeVoiceContext context)
00134:         {
00135:             context ??= new TradeVoiceContext();
00136:             _voiceContext = new TradeVoiceContext
00137:             {
00138:                 TraderProfileId = context.TraderProfileId,
00139:                 ScenarioId = context.ScenarioId,
00140:                 FactionId = context.FactionId,
00141:                 CaravanId = context.CaravanId,
00142:                 CaravanOriginRegion = context.CaravanOriginRegion,
00143:                 SpecialtyId = context.SpecialtyId,
00144:                 Stance = context.Stance,
00145:                 Trust = context.Trust,
00146:                 StableContextKey = context.StableContextKey
00147:             };
00148:             Recalculate();
00149:         }
00150:
00151:         /// <summary>World context for the news strip (phase label + day).</summary>
00152:         public void SetWorldContext(string phaseLabel, int day)
00153:         {
00154:             _worldPhaseLabel = phaseLabel ?? string.Empty;
00155:             _worldDay = Math.Max(1, day);
00156:         }
00157:
00158:         /// <summary>Items whose scarcity multipliers show in the news strip.</summary>
00159:         public void SetWatchedItems(IEnumerable<string> itemIds)
00160:         {
00161:             _watchedItems.Clear();
00162:             if (itemIds != null)
00163:             {
00168:         // ── TradeScreenUI parity surface ─────────────────────────────
00169:
00170:         public bool Open(string factionId, string factionName, string leaderName, int successionGeneration)
00171:         {
00172:             if (_stance != null && !_stance.IsFactionActive(factionId)) return false;
00173:
00174:             ViewModel.SetOpen(true);
00175:             ViewModel.SetFaction(factionId, factionName, leaderName, successionGeneration);
00176:             ClearOffers();
00177:             Recalculate();
00178:             return true;
00179:         }
00180:
00181:         public void Close(bool traded = false)
00182:         {
00183:             ViewModel.SetOpen(false);
00184:         }
00185:
00186:         public void SetPlayerOffer(string itemId, int count)
00187:         {
00188:             if (string.IsNullOrEmpty(itemId)) return;
00189:             if (count <= 0) _playerOfferCounts.Remove(itemId);
00190:             else _playerOfferCounts[itemId] = count;
00192:         }
00193:
00194:         public void SetFactionAsk(string itemId, int count)
00195:         {
00196:             if (string.IsNullOrEmpty(itemId)) return;
00197:             if (count <= 0) _factionAskCounts.Remove(itemId);
00198:             else _factionAskCounts[itemId] = count;
00199:             Recalculate();
00200:         }
00201:
00202:         public void SetBiologicalOffer(BiologicalTradeItem item, int count)
00203:         {
00204:             if (count <= 0) _bioOfferCounts.Remove(item);
00205:             else _bioOfferCounts[item] = count;
00206:             Recalculate();
00207:         }
00208:
00209:         public void ClearOffers()
00210:         {
00211:             _playerOfferCounts.Clear();
00212:             _factionAskCounts.Clear();
00213:             _bioOfferCounts.Clear();
00214:             Recalculate();
00215:         }
00216:
00217:         /// <summary>
00218:         /// Re-maps the providers onto the view model. Read-only against every
00219:         /// provider; raises exactly one Changed event through the VM batching.
00220:         /// </summary>
00221:         public void Recalculate()
00222:         {
00223:             string factionId = ViewModel.FactionId;
00224:
00225:             var stance = TradeStance.Refuse;
00226:             float trust = 0f;
00227:             float aggression = 0f;
00230:             if (_stance != null)
00231:             {
00232:                 stance = _stance.GetStance(factionId);
00233:                 trust = _stance.GetEffectiveTrust(factionId);
00234:                 aggression = _stance.GetRaidAggression(factionId);
00235:                 willTrade = _stance.WillTrade(factionId);
00236:             }
00237:
00238:             ViewModel.SetStance(stance);
00239:             ViewModel.SetMeters(trust, aggression);
00245:             }
00246:
00247:             if (_voiceResolver != null)
00248:             {
00249:                 var context = new TradeVoiceContext
00250:                 {
00251:                     TraderProfileId = _voiceContext.TraderProfileId,
00252:                     ScenarioId = _voiceContext.ScenarioId,
00253:                     FactionId = string.IsNullOrEmpty(_voiceContext.FactionId)
00254:                         ? factionId
00255:                         : _voiceContext.FactionId,
00256:                     CaravanId = _voiceContext.CaravanId,
00257:                     CaravanOriginRegion = _voiceContext.CaravanOriginRegion,
00258:                     SpecialtyId = _voiceContext.SpecialtyId,
00259:                     Stance = stance,
00260:                     Trust = trust,
00261:                     StableContextKey = _voiceContext.StableContextKey
00262:                 };
00263:                 var voice = _voiceResolver.ResolveGreeting(context);
00264:                 ApplyVoice(voice);
00265:             }
00266:
00267:             if (_radio != null)
00268:             {
00269:                 var intercept = _radio.GetFactionEvent(factionId, RadioEventKind.InterceptChatter, _worldDay, _rng);
00270:                 ViewModel.SetRadioTicker(intercept.Message);
00271:             }
00272:
00273:             ViewModel.SetShockBadges(CollectShockBadges());
00274:             ViewModel.SetScarcityBands(CollectScarcityBands());
00275:             ViewModel.SetTable(
00276:                 BuildLines(_playerOfferCounts),
00277:                 BuildLines(_factionAskCounts),
00278:                 _bioOfferCounts,
00279:                 willTrade);
00280:         }
00281:
00282:         public bool TryConfirmTrade()
00283:         {
00284:             if (!ViewModel.CanConfirm) return false;
00285:             if (_execution != null)
00286:             {
00287:                 // Snapshot the table: the sink owns its copy, and clearing our
00288:                 // state afterwards must never empty a dictionary we already gave away.
00289:                 if (!_execution.TryExecuteTrade(
00290:                         ViewModel.FactionId,
00291:                         new Dictionary<string, int>(_playerOfferCounts),
00292:                         new Dictionary<string, int>(_factionAskCounts),
00293:                         new Dictionary<BiologicalTradeItem, int>(_bioOfferCounts)))
00294:                 {
00295:                     return false;
00296:                 }
00297:             }
00298:             ClearOffers();
00299:             if (_voiceResolver != null)
00300:             {
00301:                 var context = new TradeVoiceContext
00302:                 {
00303:                     TraderProfileId = _voiceContext.TraderProfileId,
00304:                     ScenarioId = _voiceContext.ScenarioId,
00305:                     FactionId = string.IsNullOrEmpty(_voiceContext.FactionId)
00306:                         ? ViewModel.FactionId
00307:                         : _voiceContext.FactionId,
00308:                     CaravanId = _voiceContext.CaravanId,
00309:                     CaravanOriginRegion = _voiceContext.CaravanOriginRegion,
00310:                     SpecialtyId = _voiceContext.SpecialtyId,
00311:                     Stance = ViewModel.Stance,
00312:                     Trust = ViewModel.Trust,
00313:                     StableContextKey = _voiceContext.StableContextKey
00314:                 };
00315:                 ApplyVoice(_voiceResolver.ResolveLine(
00316:                     context,
00317:                     TradeVoiceLineFamily.Acceptance,
00318:                     "pleased"));
00319:             }
00320:             if (_radio != null)
00321:             {
00322:                 var reaction = _radio.GetFactionEvent(ViewModel.FactionId, RadioEventKind.TradeReaction, _worldDay, _rng);
00323:                 ViewModel.SetRadioTicker(reaction.Message);
00324:             }
00325:             return true;
00326:         }
00327:
00328:         public bool TryDemandParley()
00329:         {
00330:             bool parleyOk = false;
00331:             if (_execution != null)
00332:             {
00333:                 parleyOk = _execution.TryDemandParley(ViewModel.FactionId);
00334:             }
00335:             else
00336:             {
00337:                 parleyOk = ViewModel.CanDemandParley;
00340:             if (_radio != null)
00341:             {
00342:                 var parleyLine = _radio.GetFactionEvent(ViewModel.FactionId, RadioEventKind.ParleyResolution, _worldDay, _rng);
00343:                 ViewModel.SetRadioTicker(parleyLine.Message);
00344:             }
00345:             return parleyOk;
00346:         }
00347:
00348:         /// <summary>Qualitative, multi-line quote summary (ECON-002: no raw digits).</summary>
00349:         public string BuildQuoteSummary()
00350:         {
00351:             var sb = new StringBuilder();
00352:             sb.AppendLine("THE NEGOTIATION TABLE");
00353:             sb.AppendLine($"{ViewModel.FactionName} · {ViewModel.LeaderName} (gen {ViewModel.SuccessionGeneration})");
00354:
00355:             if (ViewModel.PlayerOffers.Count == 0 && ViewModel.BiologicalOffers.Count == 0)
00356:             {
00357:                 sb.AppendLine("OFFER: your edge of the table is bare.");
00368:             }
00369:
00370:             if (ViewModel.FactionDemands.Count == 0)
00371:             {
00372:                 sb.AppendLine("DEMAND: their edge of the table is bare.");
00373:             }
00374:             else
00375:             {
00376:                 sb.Append("DEMAND: ");
00377:                 sb.AppendLine(FormatLines(ViewModel.FactionDemands));
00378:             }
00379:
00380:             sb.Append("SCALE: ").AppendLine(ViewModel.FairnessLabel);
00381:             return sb.ToString();
00384:         // ── Internals ────────────────────────────────────────────────
00385:
00386:         private static string FormatLines(IReadOnlyList<TradeLineData> lines)
00387:         {
00388:             var parts = new List<string>();
00389:             foreach (var line in lines)
00390:             {
00396:         private void ApplyVoice(TradeVoiceResult voice)
00397:         {
00398:             string displayName = _voiceResolver != null &&
00399:                 _voiceResolver.Catalog.TryGetTrader(voice.ProfileId, out var trader)
00400:                 ? trader.display_name
00401:                 : "The Merchant";
00402:             ViewModel.SetTraderVoice(voice.ProfileId, displayName, voice.Text);
00403:         }
00404:
00405:         private string JoinBioLines()
00406:         {
00407:             var parts = new List<string>();
00408:             foreach (var pair in ViewModel.BiologicalOffers)
00409:             {
00410:                 parts.Add($"{pair.Value}x {pair.Key}");
00411:             }
00412:             return string.Join(", ", parts);
00413:         }
00414:
00415:         private List<TradeLineData> BuildLines(Dictionary<string, int> counts)
00416:         {
00417:             var lines = new List<TradeLineData>();
00418:             foreach (var pair in counts)
00419:             {
00420:                 if (pair.Value > 0)
00421:                 {
00422:                     lines.Add(new TradeLineData(pair.Key, _displayName(pair.Key), pair.Value, _unitPrice(pair.Key)));
00423:                 }
00424:             }
00425:             return lines;
00426:         }
00427:
00428:         private static readonly PriceShockKind[] s_allShockKinds =
00429:         {
00430:             PriceShockKind.PlumePassing,
00431:             PriceShockKind.ConvoyAmbush,
00432:             PriceShockKind.FactionConflict,
00433:             PriceShockKind.SeasonalScarcity,
00434:             PriceShockKind.DiseaseOutbreak,
00435:             PriceShockKind.FuelShortage
00436:         };
00437:
00438:         private List<ShockBadgeData> CollectShockBadges()
00439:         {
00440:             var badges = new List<ShockBadgeData>();
00441:             if (_shocks == null) return badges;
00442:
00443:             for (int i = 0; i < s_allShockKinds.Length; i++)
00444:             {
00445:                 PriceShockKind kind = s_allShockKinds[i];
00446:                 if (_shocks.TryGetPriceShock(kind, _worldDay, out var rule))
00447:                 {
00448:                     badges.Add(new ShockBadgeData(rule.Kind, rule.Multiplier, rule.Trigger));
00449:                 }
00450:             }
00451:             return badges;
00452:         }
00453:
00454:         private List<ScarcityBandData> CollectScarcityBands()
00455:         {
00456:             var bands = new List<ScarcityBandData>();
00457:             if (_shocks == null) return bands;
00458:
00459:             foreach (var itemId in _watchedItems)
00460:             {
00461:                 float multiplier = _shocks.GetScarcityMultiplier(_worldDay, itemId);
00462:                 bands.Add(new ScarcityBandData(itemId, _displayName(itemId), multiplier));
00463:             }
00464:             return bands;
00465:         }
00466:     }
00467: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs`

### `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs` — complete current file

- Size: 286 lines / 12304 bytes.
- SHA-256: `d0860320466aeb33812b16aea8a2484937cb8a57b88f4007e3f8f9989356b795`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: namespace Ashfall.Core.Economy
00003: {
00004:     using System;
00005:     using System.Collections.Generic;
00006:     using System.Text.Json;
00007:
00008:     /// <summary>
00009:     /// One data-defined Negotiation Table scenario (fair deal, offer short,
00010:     /// empty table). JSON in StreamingAssets is the authority; nothing about
00011:     /// a scenario is hardcoded in hosts.
00012:     /// </summary>
00013:     public sealed class TradeScreenScenario
00014:     {
00015:         public string Id { get; set; } = string.Empty;
00016:         public string FactionId { get; set; } = string.Empty;
00017:         public string FactionName { get; set; } = string.Empty;
00018:         public string LeaderName { get; set; } = string.Empty;
00019:         public int SuccessionGeneration { get; set; } = 1;
00020:         public TradeStance Stance { get; set; } = TradeStance.Refuse;
00021:         public float Trust { get; set; }
00022:         public float Aggression { get; set; }
00023:         public int ConsecutiveRepels { get; set; }
00024:         public bool HasSurrendered { get; set; }
00025:         public bool CanDemandParley { get; set; }
00026:         public string WorldPhase { get; set; } = string.Empty;
00027:         public int WorldDay { get; set; } = 1;
00028:         public List<ShockBadgeData> PriceShocks { get; set; } = new();
00029:         public List<ScarcityBandData> Scarcity { get; set; } = new();
00030:         public List<TradeLineData> PlayerOffers { get; set; } = new();
00031:         public List<TradeLineData> FactionDemands { get; set; } = new();
00032:         public Dictionary<BiologicalTradeItem, int> BiologicalOffers { get; set; } = new();
00033:         public TradeFairness ExpectedFairness { get; set; } = TradeFairness.EmptyTable;
00034:         public bool ConfirmSucceeds { get; set; }
00035:         public string RadioTicker { get; set; } = string.Empty;
00036:     }
00037:
00038:     /// <summary>Records intent routed through the mock sink (skin-track assertions).</summary>
00039:     public sealed class MockTradeIntentSink : ITradeIntentSink
00040:     {
00041:         public int ConfirmCalls { get; private set; }
00042:         public int ParleyCalls { get; private set; }
00043:         public int CloseCalls { get; private set; }
00044:         public bool? LastCloseWasTraded { get; private set; }
00045:         public bool ConfirmResult { get; set; } = true;
00046:
00047:         public bool TryConfirmTrade()
00048:         {
00049:             ConfirmCalls++;
00050:             return ConfirmResult;
00051:         }
00052:
00053:         public bool TryDemandParley()
00054:         {
00055:             ParleyCalls++;
00056:             return true;
00057:         }
00058:
00059:         public void Close(bool traded)
00060:         {
00061:             CloseCalls++;
00062:             LastCloseWasTraded = traded;
00063:         }
00064:     }
00065:
00066:     /// <summary>A mock binding: view-model + intent sink built from a scenario.</summary>
00067:     public sealed class MockTradeScreenBinding
00068:     {
00069:         public TradeScreenScenario Scenario { get; }
00070:         public TradeScreenViewModel ViewModel { get; }
00071:         public MockTradeIntentSink Intents { get; }
00072:
00073:         public MockTradeScreenBinding(TradeScreenScenario scenario, TradeScreenViewModel viewModel, MockTradeIntentSink intents)
00074:         {
00075:             Scenario = scenario;
00076:             ViewModel = viewModel;
00077:             Intents = intents;
00078:         }
00079:     }
00080:
00081:     /// <summary>
00082:     /// Loads trade screen scenarios from JSON and builds mock bindings for the
00083:     /// skin track. Both tracks build against the same seam.
00084:     /// </summary>
00085:     public static class TradeScreenScenarioLoader
00086:     {
00087:         /// <summary>
00088:         /// Parses { "scenarios": [ { id, faction_id, stance, trust, player_offers: [{item_id,
00089:         /// display_name, quantity, unit_price}], biological_offers: {PintOfBlood: n},
00090:         /// faction_demands: [...], expected_fairness, confirm_succeeds, ... } ] }.
00091:         /// </summary>
00092:         public static IReadOnlyList<TradeScreenScenario> LoadFromJson(string json)
00093:         {
00094:             var result = new List<TradeScreenScenario>();
00095:             if (string.IsNullOrWhiteSpace(json)) return result;
00096:
00097:             using var doc = JsonDocument.Parse(json);
00098:             var root = doc.RootElement;
00099:             if (!root.TryGetProperty("scenarios", out var scenariosEl) || scenariosEl.ValueKind != JsonValueKind.Array)
00100:             {
00101:                 return result;
00102:             }
00103:
00104:             foreach (var s in scenariosEl.EnumerateArray())
00105:             {
00106:                 var scenario = new TradeScreenScenario
00107:                 {
00108:                     Id = GetString(s, "id"),
00109:                     FactionId = GetString(s, "faction_id"),
00110:                     FactionName = GetString(s, "faction_name"),
00111:                     LeaderName = GetString(s, "leader_name"),
00112:                     SuccessionGeneration = GetInt(s, "succession_generation", 1),
00113:                     Stance = ParseStance(GetString(s, "stance")),
00114:                     Trust = GetFloat(s, "trust", 0f),
00115:                     Aggression = GetFloat(s, "aggression", 0f),
00116:                     ConsecutiveRepels = GetInt(s, "consecutive_repels", 0),
00117:                     HasSurrendered = GetBool(s, "has_surrendered", false),
00118:                     CanDemandParley = GetBool(s, "can_demand_parley", false),
00119:                     WorldPhase = GetString(s, "world_phase"),
00120:                     WorldDay = GetInt(s, "world_day", 1),
00121:                     ExpectedFairness = ParseFairness(GetString(s, "expected_fairness")),
00122:                     ConfirmSucceeds = GetBool(s, "confirm_succeeds", false),
00123:                     RadioTicker = GetString(s, "radio_ticker")
00124:                 };
00125:
00126:                 if (s.TryGetProperty("price_shocks", out var shocksEl) && shocksEl.ValueKind == JsonValueKind.Array)
00127:                 {
00128:                     foreach (var sh in shocksEl.EnumerateArray())
00129:                     {
00130:                         scenario.PriceShocks.Add(new ShockBadgeData(
00131:                             ParseShockKind(GetString(sh, "kind")),
00132:                             GetFloat(sh, "multiplier", 1f),
00133:                             GetString(sh, "note")));
00134:                     }
00135:                 }
00136:
00137:                 if (s.TryGetProperty("scarcity", out var scarEl) && scarEl.ValueKind == JsonValueKind.Array)
00138:                 {
00139:                     foreach (var sc in scarEl.EnumerateArray())
00140:                     {
00141:                         scenario.Scarcity.Add(new ScarcityBandData(
00142:                             GetString(sc, "item_id"),
00143:                             GetString(sc, "display_name"),
00144:                             GetFloat(sc, "multiplier", 1f)));
00145:                     }
00146:                 }
00147:
00148:                 if (s.TryGetProperty("player_offers", out var offersEl) && offersEl.ValueKind == JsonValueKind.Array)
00149:                 {
00150:                     foreach (var o in offersEl.EnumerateArray())
00151:                     {
00152:                         scenario.PlayerOffers.Add(new TradeLineData(
00153:                             GetString(o, "item_id"),
00154:                             GetString(o, "display_name"),
00155:                             GetInt(o, "quantity", 0),
00156:                             GetFloat(o, "unit_price", 0f)));
00157:                     }
00158:                 }
00159:
00160:                 if (s.TryGetProperty("faction_demands", out var demandsEl) && demandsEl.ValueKind == JsonValueKind.Array)
00161:                 {
00162:                     foreach (var d in demandsEl.EnumerateArray())
00163:                     {
00164:                         scenario.FactionDemands.Add(new TradeLineData(
00165:                             GetString(d, "item_id"),
00166:                             GetString(d, "display_name"),
00167:                             GetInt(d, "quantity", 0),
00168:                             GetFloat(d, "unit_price", 0f)));
00169:                     }
00170:                 }
00171:
00172:                 if (s.TryGetProperty("biological_offers", out var bioEl) && bioEl.ValueKind == JsonValueKind.Object)
00173:                 {
00174:                     foreach (var b in bioEl.EnumerateObject())
00175:                     {
00176:                         if (Enum.TryParse<BiologicalTradeItem>(b.Name, ignoreCase: true, out var kind) && b.Value.ValueKind == JsonValueKind.Number)
00177:                         {
00178:                             scenario.BiologicalOffers[kind] = b.Value.GetInt32();
00179:                         }
00180:                     }
00181:                 }
00182:
00183:                 result.Add(scenario);
00184:             }
00185:
00186:             return result;
00187:         }
00188:
00189:         /// <summary>Builds the skin-track binding: a populated view-model + recording mock sink.</summary>
00190:         public static MockTradeScreenBinding CreateBinding(TradeScreenScenario scenario, ITradeTellProvider tells, ISeededRng rng)
00191:         {
00192:             if (scenario == null) throw new ArgumentNullException(nameof(scenario));
00193:
00194:             var vm = new TradeScreenViewModel();
00195:             vm.SetOpen(true);
00196:             vm.SetFaction(scenario.FactionId, scenario.FactionName, scenario.LeaderName, scenario.SuccessionGeneration);
00197:             vm.SetStance(scenario.Stance);
00198:             vm.SetMeters(scenario.Trust, scenario.Aggression);
00199:             vm.SetFactionPresence(scenario.ConsecutiveRepels, scenario.HasSurrendered, scenario.CanDemandParley);
00200:             vm.SetWorld(scenario.WorldPhase, scenario.WorldDay);
00201:             vm.SetShockBadges(scenario.PriceShocks);
00202:             vm.SetScarcityBands(scenario.Scarcity);
00203:
00204:             bool willTrade = scenario.Stance == TradeStance.Trade || scenario.Stance == TradeStance.ShareIntel;
00205:             vm.SetTable(scenario.PlayerOffers, scenario.FactionDemands, scenario.BiologicalOffers, willTrade);
00206:
00207:             if (tells != null && tells.TrySelectTell(scenario.Stance, scenario.Trust, rng, out var tell))
00208:             {
00209:                 vm.SetTell(tell.Id, tell.Line);
00210:             }
00211:
00212:             vm.SetRadioTicker(scenario.RadioTicker);
00213:
00214:             var sink = new MockTradeIntentSink { ConfirmResult = scenario.ConfirmSucceeds };
00215:             return new MockTradeScreenBinding(scenario, vm, sink);
00216:         }
00217:
00218:         // ── JSON helpers (invariant culture only) ────────────────────
00219:
00220:         private static string GetString(JsonElement el, string prop)
00221:         {
00222:             return el.TryGetProperty(prop, out var v) && v.ValueKind == JsonValueKind.String
00223:                 ? v.GetString() ?? string.Empty
00224:                 : string.Empty;
00225:         }
00226:
00227:         private static int GetInt(JsonElement el, string prop, int fallback)
00228:         {
00229:             return el.TryGetProperty(prop, out var v) && v.ValueKind == JsonValueKind.Number
00230:                 ? v.GetInt32()
00231:                 : fallback;
00232:         }
00233:
00234:         private static float GetFloat(JsonElement el, string prop, float fallback)
00235:         {
00236:             return el.TryGetProperty(prop, out var v) && v.ValueKind == JsonValueKind.Number
00237:                 ? (float)v.GetDouble()
00238:                 : fallback;
00239:         }
00240:
00241:         private static bool GetBool(JsonElement el, string prop, bool fallback)
00242:         {
00243:             if (!el.TryGetProperty(prop, out var v)) return fallback;
00244:             if (v.ValueKind == JsonValueKind.True) return true;
00245:             if (v.ValueKind == JsonValueKind.False) return false;
00246:             return fallback;
00247:         }
00248:
00249:         private static TradeStance ParseStance(string key)
00250:         {
00251:             switch ((key ?? string.Empty).Trim())
00252:             {
00253:                 case "hostile_raid":
00254:                 case "HostileRaid": return TradeStance.HostileRaid;
00255:                 case "rob":
00256:                 case "Rob": return TradeStance.Rob;
00257:                 case "trade":
00258:                 case "Trade": return TradeStance.Trade;
00259:                 case "share_intel":
00260:                 case "ShareIntel": return TradeStance.ShareIntel;
00261:                 default: return TradeStance.Refuse;
00262:             }
00263:         }
00264:
00265:         private static TradeFairness ParseFairness(string key)
00266:         {
00267:             switch ((key ?? string.Empty).Trim())
00268:             {
00269:                 case "fair": return TradeFairness.Fair;
00270:                 case "short": return TradeFairness.Short;
00271:                 default: return TradeFairness.EmptyTable;
00272:             }
00273:         }
00274:
00275:         private static PriceShockKind ParseShockKind(string key)
00276:         {
00277:             switch ((key ?? string.Empty).Trim())
00278:             {
00279:                 case "ConvoyAmbush": return PriceShockKind.ConvoyAmbush;
00280:                 case "FactionWar": return PriceShockKind.FactionWar;
00281:                 case "WinterDeepens": return PriceShockKind.WinterDeepens;
00282:                 default: return PriceShockKind.PlumePassing;
00283:             }
00284:         }
00285:     }
00286: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/TradeTellCorpusTests.cs`

### `Ashfall.Core.Tests/TradeTellCorpusTests.cs` — complete current file

- Size: 178 lines / 6987 bytes.
- SHA-256: `c16c43086c8d61f19ff3d3be209b669262cddd2c359f206e62a91a544a8288e3`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Economy;
00007: using Xunit;
00008:
00009: namespace Ashfall.Core.Tests
00010: {
00011:     /// <summary>
00012:     /// Negotiation Table tell corpus: coverage counts, tone lint, band math,
00013:     /// and seed determinism. Mirrors the FactionRadioCorpusTests pattern.
00014:     /// </summary>
00015:     public class TradeTellCorpusTests
00016:     {
00017:         private static TradeTellEngine CreateLoadedEngine()
00018:         {
00019:             string corpusPath = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data/trade_tell_lines.json");
00020:             if (!File.Exists(corpusPath))
00021:             {
00022:                 corpusPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data/trade_tell_lines.json");
00023:             }
00024:
00025:             Assert.True(File.Exists(corpusPath), $"Tell corpus JSON not found at {corpusPath}");
00026:             return TradeTellEngine.LoadFromJson(File.ReadAllText(corpusPath));
00027:         }
00028:
00029:         private static string StanceKey(TradeStance stance)
00030:         {
00031:             switch (stance)
00032:             {
00033:                 case TradeStance.HostileRaid: return "hostile_raid";
00034:                 case TradeStance.Rob: return "rob";
00035:                 case TradeStance.Refuse: return "refuse";
00036:                 case TradeStance.ShareIntel: return "share_intel";
00037:                 default: return "trade";
00038:             }
00039:         }
00040:
00041:         [Fact]
00042:         public void Corpus_LoadsFourBandsAndTwentyPools()
00043:         {
00044:             var engine = CreateLoadedEngine();
00045:
00046:             Assert.Equal(4, engine.BandCount);
00047:             Assert.Equal(TradeTrustBands.Hostile, engine.Bands[0]);
00048:             Assert.Equal(TradeTrustBands.Wary, engine.Bands[1]);
00049:             Assert.Equal(TradeTrustBands.Neutral, engine.Bands[2]);
00050:             Assert.Equal(TradeTrustBands.Warm, engine.Bands[3]);
00051:
00052:             // 5 stances x 4 bands = 20 pools, >= 3 lines each (>= 60 total).
00053:             Assert.Equal(20, engine.PoolCount);
00054:             Assert.True(engine.LineCount >= 60, $"Expected >= 60 tell lines, found {engine.LineCount}");
00055:         }
00056:
00057:         [Fact]
00058:         public void Corpus_EveryStanceAndBandSelectsALegibleLine()
00059:         {
00060:             var engine = CreateLoadedEngine();
00061:             var rng = new SeededRng(2026);
00062:
00063:             foreach (TradeStance stance in Enum.GetValues(typeof(TradeStance)))
00064:             {
00065:                 foreach (float trust in new[] { -100f, -40f, -39f, 0f, 1f, 40f, 41f, 100f })
00066:                 {
00067:                     bool selected = engine.TrySelectTell(stance, trust, rng, out var tell);
00068:                     Assert.True(selected, $"No tell for stance={stance} trust={trust}");
00069:                     Assert.False(string.IsNullOrWhiteSpace(tell.Line));
00070:                     Assert.InRange(tell.Line.Length, 20, 140);
00071:                     Assert.Contains(StanceKey(stance), tell.Id);
00072:                     Assert.Contains(engine.BandForTrust(trust), tell.Id);
00073:                 }
00074:             }
00075:         }
00076:
00077:         [Fact]
00078:         public void Corpus_ToneLint_NoModernSlangOrAnachronisms()
00079:         {
00080:             var engine = CreateLoadedEngine();
00081:             var forbiddenWords = new[]
00082:             {
00083:                 " lol ", " gg ", " bruh ", " meta ", " player ", " respawn ", " nerf ", " buff ", " xp "
00084:             };
00085:
00086:             // Exhaustive: every line in every pool.
00087:             foreach (TradeStance stance in Enum.GetValues(typeof(TradeStance)))
00088:             {
00089:                 foreach (var band in engine.Bands)
00090:                 {
00091:                     Assert.True(engine.TryGetPoolLines(stance, band, out var lines));
00092:                     foreach (var line in lines)
00093:                     {
00094:                         string lower = " " + line.ToLowerInvariant() + " ";
00095:                         foreach (var forbidden in forbiddenWords)
00096:                         {
00097:                             Assert.DoesNotContain(forbidden, lower);
00098:                         }
00099:                     }
00100:                 }
00101:             }
00102:         }
00103:
00104:         [Fact]
00105:         public void Corpus_NoDuplicateLinesWithinPool()
00106:         {
00107:             var engine = CreateLoadedEngine();
00108:
00109:             foreach (TradeStance stance in Enum.GetValues(typeof(TradeStance)))
00110:             {
00111:                 foreach (var band in engine.Bands)
00112:                 {
00113:                     Assert.True(engine.TryGetPoolLines(stance, band, out var lines),
00114:                         $"Missing pool for stance={stance} band={band}");
00115:                     Assert.True(lines.Count >= 3, $"Pool stance={stance} band={band} has {lines.Count} lines, expected >= 3");
00116:
00117:                     var seen = new HashSet<string>(StringComparer.Ordinal);
00118:                     foreach (var line in lines)
00119:                     {
00120:                         Assert.DoesNotContain(line, seen);
00121:                         seen.Add(line);
00122:                     }
00123:                 }
00124:             }
00125:         }
00126:
00127:         [Fact]
00128:         public void Engine_BandBoundaries_MapTrustCorrectly()
00129:         {
00130:             var engine = CreateLoadedEngine();
00131:
00132:             Assert.Equal(TradeTrustBands.Hostile, engine.BandForTrust(-100f));
00133:             Assert.Equal(TradeTrustBands.Hostile, engine.BandForTrust(-40f));
00134:             Assert.Equal(TradeTrustBands.Wary, engine.BandForTrust(-39f));
00135:             Assert.Equal(TradeTrustBands.Wary, engine.BandForTrust(0f));
00136:             Assert.Equal(TradeTrustBands.Neutral, engine.BandForTrust(1f));
00137:             Assert.Equal(TradeTrustBands.Neutral, engine.BandForTrust(40f));
00138:             Assert.Equal(TradeTrustBands.Warm, engine.BandForTrust(41f));
00139:             Assert.Equal(TradeTrustBands.Warm, engine.BandForTrust(100f));
00140:         }
00141:
00142:         [Fact]
00143:         public void Engine_DeterministicRotation_SameSeedSameLine()
00144:         {
00145:             var engine1 = CreateLoadedEngine();
00146:             var engine2 = CreateLoadedEngine();
00147:
00148:             var rng1 = new SeededRng(9999);
00149:             var rng2 = new SeededRng(9999);
00150:
00151:             for (int i = 0; i < 20; i++)
00152:             {
00153:                 foreach (TradeStance stance in new[] { TradeStance.Trade, TradeStance.Refuse })
00154:                 {
00155:                     Assert.True(engine1.TrySelectTell(stance, 22f, rng1, out var t1));
00156:                     Assert.True(engine2.TrySelectTell(stance, 22f, rng2, out var t2));
00157:                     Assert.Equal(t1.Id, t2.Id);
00158:                     Assert.Equal(t1.Line, t2.Line);
00159:                 }
00160:             }
00161:         }
00162:
00163:         [Fact]
00164:         public void Engine_RotationVariesAcrossSeeds()
00165:         {
00166:             var engine = CreateLoadedEngine();
00167:
00168:             var lines = new HashSet<string>(StringComparer.Ordinal);
00169:             for (int seed = 1; seed <= 6; seed++)
00170:             {
00171:                 Assert.True(engine.TrySelectTell(TradeStance.Trade, 22f, new SeededRng(seed), out var tell));
00172:                 lines.Add(tell.Line);
00173:             }
00174:
00175:             Assert.True(lines.Count > 1, "Different seeds should rotate through the pool, not pin one line.");
00176:         }
00177:     }
00178: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/World/SettlementCatalog.cs`

### `Assets/Ashfall.Core/World/SettlementCatalog.cs` — bounded current excerpt (454 of 463 lines)

- Size: 463 lines / 17110 bytes.
- SHA-256: `492fea4b385a078e5e94ed407fe2d74a9876a8143a92b87144db32bcb0d9d29e`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Text.Json;
00006: using System.Text.Json.Serialization;
00007:
00008: namespace Ashfall.Core.World
00009: {
00010:     [Serializable]
00011:     public sealed class SettlementEconomy
00012:     {
00013:         [JsonPropertyName("primary_export")]
00014:         public string PrimaryExport { get; set; } = string.Empty;
00015:
00016:         [JsonPropertyName("primary_import")]
00017:         public string PrimaryImport { get; set; } = string.Empty;
00018:
00019:         [JsonPropertyName("trade_specialty")]
00020:         public string TradeSpecialty { get; set; } = string.Empty;
00021:
00022:         [JsonPropertyName("price_modifier_exports")]
00023:         public float PriceModifierExports { get; set; } = 1.0f;
00024:
00025:         [JsonPropertyName("price_modifier_imports")]
00026:         public float PriceModifierImports { get; set; } = 1.0f;
00027:
00028:         [JsonPropertyName("stock_item_ids")]
00029:         public List<string> StockItemIds { get; set; } = new List<string>();
00030:     }
00031:
00032:     [Serializable]
00033:     public sealed class SettlementSociety
00034:     {
00035:         [JsonPropertyName("governance")]
00036:         public string Governance { get; set; } = string.Empty;
00037:
00038:         [JsonPropertyName("population")]
00039:         public int Population { get; set; } = 50;
00040:
00041:         [JsonPropertyName("core_value")]
00042:         public string CoreValue { get; set; } = string.Empty;
00043:
00044:         [JsonPropertyName("internal_tension")]
00045:         public string InternalTension { get; set; } = string.Empty;
00046:     }
00047:
00048:     [Serializable]
00049:     public sealed class SettlementFactionRelation
00050:     {
00051:         [JsonPropertyName("primary_faction")]
00052:         public string PrimaryFaction { get; set; } = string.Empty;
00053:
00054:         [JsonPropertyName("standing_gate_faction")]
00055:         public string StandingGateFaction { get; set; } = string.Empty;
00056:
00057:         [JsonPropertyName("min_standing_to_enter")]
00058:         public int MinStandingToEnter { get; set; } = 0;
00059:
00060:         [JsonPropertyName("hostile_standing_threshold")]
00061:         public int HostileStandingThreshold { get; set; } = -40;
00062:     }
00063:
00064:     [Serializable]
00065:     public sealed class SettlementDefinition
00066:     {
00067:         [JsonPropertyName("id")]
00068:         public string Id { get; set; } = string.Empty;
00069:
00070:         [JsonPropertyName("display_name")]
00071:         public string DisplayName { get; set; } = string.Empty;
00072:
00073:         [JsonPropertyName("archetype")]
00074:         public string Archetype { get; set; } = string.Empty;
00075:
00076:         [JsonPropertyName("region")]
00077:         public string Region { get; set; } = string.Empty;
00078:
00079:         [JsonPropertyName("location_id")]
00080:         public string LocationId { get; set; } = string.Empty;
00081:
00082:         [JsonPropertyName("route_node")]
00083:         public string RouteNode { get; set; } = string.Empty;
00084:
00085:         [JsonPropertyName("description")]
00086:         public string Description { get; set; } = string.Empty;
00087:
00088:         [JsonPropertyName("survival_adaptation")]
00089:         public string SurvivalAdaptation { get; set; } = string.Empty;
00090:
00091:         [JsonPropertyName("economy")]
00092:         public SettlementEconomy Economy { get; set; } = new SettlementEconomy();
00093:
00094:         [JsonPropertyName("society")]
00095:         public SettlementSociety Society { get; set; } = new SettlementSociety();
00096:
00097:         [JsonPropertyName("faction_relation")]
00098:         public SettlementFactionRelation FactionRelation { get; set; } = new SettlementFactionRelation();
00099:
00100:         [JsonPropertyName("location_link")]
00101:         public string LocationLink { get; set; } = string.Empty;
00102:
00103:         [JsonPropertyName("population")]
00104:         public int Population { get; set; } = 0;
00105:
00106:         [JsonPropertyName("allegiance")]
00107:         public string Allegiance { get; set; } = string.Empty;
00108:
00109:         [JsonPropertyName("threat_level")]
00110:         public int ThreatLevel { get; set; } = 2;
00111:
00112:         [JsonPropertyName("attitude")]
00113:         public string Attitude { get; set; } = "neutral";
00114:
00115:         [JsonPropertyName("trade_goods")]
00116:         public List<string> TradeGoods { get; set; } = new List<string>();
00117:
00118:         [JsonPropertyName("trade_needs")]
00119:         public List<string> TradeNeeds { get; set; } = new List<string>();
00120:
00121:         [JsonPropertyName("keeper_npc_id")]
00122:         public string KeeperNpcId { get; set; } = string.Empty;
00123:
00124:         [JsonPropertyName("trader_npc_id")]
00125:         public string TraderNpcId { get; set; } = string.Empty;
00126:
00127:         [JsonPropertyName("fixture_npc_id")]
00128:         public string FixtureNpcId { get; set; } = string.Empty;
00129:
00130:         [JsonPropertyName("sidework_quest_id")]
00131:         public string SideworkQuestId { get; set; } = string.Empty;
00132:
00133:         public string GetEffectiveLocationId() => !string.IsNullOrEmpty(LocationLink) ? LocationLink : LocationId;
00134:         public int GetEffectivePopulation() => Population > 0 ? Population : (Society?.Population ?? 50);
00135:         public string GetEffectiveAllegiance() => !string.IsNullOrEmpty(Allegiance) ? Allegiance : (FactionRelation?.PrimaryFaction ?? "none");
00136:     }
00137:
00138:     [Serializable]
00139:     public sealed class SettlementNpcGreeting
00140:     {
00141:         [JsonPropertyName("low_standing")]
00142:         public string LowStanding { get; set; } = string.Empty;
00143:
00144:         [JsonPropertyName("neutral")]
00145:         public string Neutral { get; set; } = string.Empty;
00146:
00147:         [JsonPropertyName("high_standing")]
00148:         public string HighStanding { get; set; } = string.Empty;
00149:     }
00150:
00151:     [Serializable]
00152:     public sealed class SettlementNpcEntry
00153:     {
00154:         [JsonPropertyName("id")]
00155:         public string Id { get; set; } = string.Empty;
00156:
00157:         [JsonPropertyName("display_name")]
00158:         public string DisplayName { get; set; } = string.Empty;
00159:
00160:         [JsonPropertyName("settlement_id")]
00161:         public string SettlementId { get; set; } = string.Empty;
00162:
00163:         [JsonPropertyName("role")]
00164:         public string Role { get; set; } = string.Empty; // "Keeper", "Trader", "Fixture"
00165:
00166:         [JsonPropertyName("profession")]
00167:         public string Profession { get; set; } = string.Empty;
00168:
00169:         [JsonPropertyName("faction")]
00170:         public string Faction { get; set; } = "none";
00171:
00172:         [JsonPropertyName("trade_specialty")]
00173:         public string TradeSpecialty { get; set; } = string.Empty;
00174:
00175:         [JsonPropertyName("physical_anchor")]
00176:         public string PhysicalAnchor { get; set; } = string.Empty;
00177:
00178:         [JsonPropertyName("value")]
00179:         public string Value { get; set; } = string.Empty;
00180:
00181:         [JsonPropertyName("fear")]
00182:         public string Fear { get; set; } = string.Empty;
00183:
00184:         [JsonPropertyName("contradiction")]
00185:         public string Contradiction { get; set; } = string.Empty;
00186:
00187:         [JsonPropertyName("personal_thread")]
00188:         public string PersonalThread { get; set; } = string.Empty;
00189:
00190:         [JsonPropertyName("greetings")]
00191:         public SettlementNpcGreeting Greetings { get; set; } = new SettlementNpcGreeting();
00192:
00193:         [JsonPropertyName("trade_tells")]
00194:         public List<string> TradeTells { get; set; } = new List<string>();
00195:
00196:         [JsonPropertyName("sidework_quest_id")]
00197:         public string SideworkQuestId { get; set; } = string.Empty;
00198:
00199:         [JsonPropertyName("portrait_id")]
00200:         public string PortraitId { get; set; } = string.Empty;
00201:     }
00202:
00203:     [Serializable]
00204:     public sealed class RepeatableQuestStage
00205:     {
00206:         [JsonPropertyName("id")]
00207:         public string Id { get; set; } = string.Empty;
00208:
00209:         [JsonPropertyName("text")]
00210:         public string Text { get; set; } = string.Empty;
00211:     }
00212:
00213:     [Serializable]
00214:     public sealed class RepeatableQuestEntry
00215:     {
00216:         [JsonPropertyName("id")]
00217:         public string Id { get; set; } = string.Empty;
00218:
00219:         [JsonPropertyName("display_name")]
00220:         public string DisplayName { get; set; } = string.Empty;
00221:
00222:         [JsonPropertyName("provider_npc_id")]
00223:         public string ProviderNpcId { get; set; } = string.Empty;
00224:
00225:         [JsonPropertyName("settlement_id")]
00226:         public string SettlementId { get; set; } = string.Empty;
00227:
00228:         [JsonPropertyName("type")]
00229:         public string Type { get; set; } = string.Empty;
00230:
00231:         [JsonPropertyName("briefing")]
00232:         public string Briefing { get; set; } = string.Empty;
00233:
00234:         [JsonPropertyName("prereq_quest_id")]
00235:         public string PrereqQuestId { get; set; } = string.Empty;
00236:
00237:         [JsonPropertyName("target_location_id")]
00238:         public string TargetLocationId { get; set; } = string.Empty;
00239:
00240:         [JsonPropertyName("cooldown_days")]
00241:         public int CooldownDays { get; set; } = 7;
00242:
00243:         [JsonPropertyName("reward_item_id")]
00244:         public string RewardItemId { get; set; } = string.Empty;
00245:
00246:         [JsonPropertyName("reward_count")]
00247:         public int RewardCount { get; set; } = 1;
00248:
00249:         [JsonPropertyName("standing_delta")]
00250:         public int StandingDelta { get; set; } = 5;
00251:
00252:         [JsonPropertyName("stages")]
00253:         public List<RepeatableQuestStage> Stages { get; set; } = new List<RepeatableQuestStage>();
00254:     }
00255:
00256:     [Serializable]
00257:     public sealed class SettlementState
00258:     {
00259:         [JsonPropertyName("cooldowns")]
00260:         public Dictionary<string, int> QuestAvailableDay { get; set; } = new Dictionary<string, int>();
00261:
00262:         [JsonPropertyName("completed_quest_counts")]
00263:         public Dictionary<string, int> CompletedQuestCounts { get; set; } = new Dictionary<string, int>();
00264:     }
00265:
00266:     public sealed class SettlementCatalog
00267:     {
00268:         private readonly Dictionary<string, SettlementDefinition> _settlementsById = new(StringComparer.OrdinalIgnoreCase);
00269:         private readonly Dictionary<string, SettlementNpcEntry> _npcsById = new(StringComparer.OrdinalIgnoreCase);
00270:         private readonly Dictionary<string, RepeatableQuestEntry> _questsById = new(StringComparer.OrdinalIgnoreCase);
00271:         private readonly List<SettlementDefinition> _allSettlements = new();
00272:         private readonly List<SettlementNpcEntry> _allNpcs = new();
00273:         private readonly List<RepeatableQuestEntry> _allQuests = new();
00274:
00275:         private readonly Dictionary<string, int> _questAvailableDay = new(StringComparer.OrdinalIgnoreCase);
00276:         private readonly Dictionary<string, int> _completedQuestCounts = new(StringComparer.OrdinalIgnoreCase);
00277:
00278:         public int SettlementCount => _allSettlements.Count;
00279:         public int NpcCount => _allNpcs.Count;
00280:         public int QuestCount => _allQuests.Count;
00281:
00282:         public IReadOnlyList<SettlementDefinition> Settlements => _allSettlements;
00283:         public IReadOnlyList<SettlementNpcEntry> Npcs => _allNpcs;
00284:         public IReadOnlyList<RepeatableQuestEntry> Quests => _allQuests;
00285:
00286:         public static SettlementCatalog LoadFromDirectory(string directoryPath, IFileIO fileIO)
00287:         {
00288:             var catalog = new SettlementCatalog();
00289:             if (string.IsNullOrEmpty(directoryPath) || fileIO == null) return catalog;
00290:
00291:             string settlementsPath = Path.Combine(directoryPath, "settlements.json");
00292:             if (fileIO.FileExists(settlementsPath))
00293:             {
00294:                 catalog.LoadSettlementsJson(fileIO.ReadAllText(settlementsPath));
00295:             }
00296:
00297:             string npcsPath = Path.Combine(directoryPath, "wasteland_settlement_npcs.json");
00298:             if (fileIO.FileExists(npcsPath))
00299:             {
00300:                 catalog.LoadNpcsJson(fileIO.ReadAllText(npcsPath));
00301:             }
00302:
00303:             string questsPath = Path.Combine(directoryPath, "repeatable_quests.json");
00304:             if (fileIO.FileExists(questsPath))
00305:             {
00306:                 catalog.LoadQuestsJson(fileIO.ReadAllText(questsPath));
00307:             }
00308:
00309:             return catalog;
00310:         }
00311:
00312:         public void LoadSettlementsJson(string json)
00313:         {
00314:             if (string.IsNullOrWhiteSpace(json)) return;
00315:             using var doc = JsonDocument.Parse(json);
00316:             if (doc.RootElement.TryGetProperty("settlements", out var settlementsElem) && settlementsElem.ValueKind == JsonValueKind.Array)
00317:             {
00318:                 foreach (var item in settlementsElem.EnumerateArray())
00319:                 {
00320:                     var settlement = JsonSerializer.Deserialize<SettlementDefinition>(item.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
00321:                     if (settlement != null && !string.IsNullOrEmpty(settlement.Id))
00322:                     {
00323:                         _settlementsById[settlement.Id] = settlement;
00324:                         _allSettlements.Add(settlement);
00328:         }
00329:
00330:         public void LoadNpcsJson(string json)
00331:         {
00332:             if (string.IsNullOrWhiteSpace(json)) return;
00333:             using var doc = JsonDocument.Parse(json);
00334:             if (doc.RootElement.TryGetProperty("npcs", out var npcsElem) && npcsElem.ValueKind == JsonValueKind.Array)
00335:             {
00336:                 foreach (var item in npcsElem.EnumerateArray())
00337:                 {
00338:                     var npc = JsonSerializer.Deserialize<SettlementNpcEntry>(item.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
00339:                     if (npc != null && !string.IsNullOrEmpty(npc.Id))
00340:                     {
00341:                         _npcsById[npc.Id] = npc;
00342:                         _allNpcs.Add(npc);
00346:         }
00347:
00348:         public void LoadQuestsJson(string json)
00349:         {
00350:             if (string.IsNullOrWhiteSpace(json)) return;
00351:             using var doc = JsonDocument.Parse(json);
00352:             if (doc.RootElement.TryGetProperty("quests", out var questsElem) && questsElem.ValueKind == JsonValueKind.Array)
00353:             {
00354:                 foreach (var item in questsElem.EnumerateArray())
00355:                 {
00356:                     var quest = JsonSerializer.Deserialize<RepeatableQuestEntry>(item.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
00357:                     if (quest != null && !string.IsNullOrEmpty(quest.Id))
00358:                     {
00359:                         _questsById[quest.Id] = quest;
00360:                         _allQuests.Add(quest);
00364:         }
00365:
00366:         public bool TryGetSettlement(string id, out SettlementDefinition settlement)
00367:         {
00368:             if (string.IsNullOrEmpty(id))
00369:             {
00370:                 settlement = null!;
00371:                 return false;
00372:             }
00373:             return _settlementsById.TryGetValue(id, out settlement!);
00374:         }
00375:
00376:         public bool TryGetNpc(string id, out SettlementNpcEntry npc)
00377:         {
00378:             if (string.IsNullOrEmpty(id))
00379:             {
00380:                 npc = null!;
00381:                 return false;
00382:             }
00383:             return _npcsById.TryGetValue(id, out npc!);
00384:         }
00385:
00386:         public bool TryGetQuest(string id, out RepeatableQuestEntry quest)
00387:         {
00388:             if (string.IsNullOrEmpty(id))
00389:             {
00390:                 quest = null!;
00391:                 return false;
00392:             }
00393:             return _questsById.TryGetValue(id, out quest!);
00394:         }
00395:
00396:         public string GetNpcGreeting(string npcId, float standing)
00397:         {
00398:             if (!TryGetNpc(npcId, out var npc)) return string.Empty;
00399:             if (standing <= -15f) return npc.Greetings.LowStanding;
00400:             if (standing >= 25f) return npc.Greetings.HighStanding;
00401:             return npc.Greetings.Neutral;
00402:         }
00403:
00404:         public bool IsQuestAvailable(string questId, int currentDay)
00405:         {
00406:             if (!_questsById.ContainsKey(questId)) return false;
00407:             if (_questAvailableDay.TryGetValue(questId, out int nextAvailable))
00408:             {
00409:                 return currentDay >= nextAvailable;
00410:             }
00411:             return true;
00412:         }
00413:
00414:         public void CompleteQuest(string questId, int currentDay)
00415:         {
00416:             if (!TryGetQuest(questId, out var quest)) return;
00417:
00418:             _questAvailableDay[questId] = currentDay + Math.Max(1, quest.CooldownDays);
00419:             if (!_completedQuestCounts.ContainsKey(questId))
00420:             {
00421:                 _completedQuestCounts[questId] = 0;
00422:             }
00423:             _completedQuestCounts[questId]++;
00424:         }
00425:
00426:         public int GetCompletedQuestCount(string questId)
00427:         {
00428:             return _completedQuestCounts.TryGetValue(questId, out int count) ? count : 0;
00429:         }
00430:
00431:         public SettlementState CaptureState()
00432:         {
00433:             return new SettlementState
00434:             {
00435:                 QuestAvailableDay = new Dictionary<string, int>(_questAvailableDay, StringComparer.OrdinalIgnoreCase),
00436:                 CompletedQuestCounts = new Dictionary<string, int>(_completedQuestCounts, StringComparer.OrdinalIgnoreCase)
00437:             };
00438:         }
00439:
00440:         public void RestoreState(SettlementState? state)
00441:         {
00442:             _questAvailableDay.Clear();
00443:             _completedQuestCounts.Clear();
00444:             if (state == null) return;
00445:
00446:             if (state.QuestAvailableDay != null)
00447:             {
00448:                 foreach (var kvp in state.QuestAvailableDay)
00449:                 {
00450:                     _questAvailableDay[kvp.Key] = kvp.Value;
00451:                 }
00452:             }
00453:
00454:             if (state.CompletedQuestCounts != null)
00455:             {
00456:                 foreach (var kvp in state.CompletedQuestCounts)
00457:                 {
00458:                     _completedQuestCounts[kvp.Key] = kvp.Value;
00459:                 }
00460:             }
00461:         }
00462:     }
00463: }
```


# Appendix — Current Source Detail: `src/UI/CaravanBarterLedgerPanel.cs`

### `src/UI/CaravanBarterLedgerPanel.cs` — complete current file

- Size: 273 lines / 10835 bytes.
- SHA-256: `d26fed01020da21467d00dbcef0594d808b44162cbeb8e55fc54f0d9acb0ce4f`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Godot;
00004: using Ashfall.Core;
00005: using Ashfall.Core.Economy;
00006: using Ashfall.Core.Radio;
00007: using Ashfall.Core.UI;
00008: using AtomicWar.GodotApp.UI;
00009: using AtomicWar.GodotApp.Economy;
00010: using DesignTheme = Ashfall.Core.UI.Theme;
00011:
00012: using Ashfall.Core.IO;
00013: namespace AtomicWar.GodotApp.UI;
00014:
00015: /// <summary>
00016: /// ASHFALL — Caravan Barter Ledger (#35 Stitch).
00017: ///
00018: /// Dashboard HYBRID wrapper around the existing TradeScreenGodotPanel.
00019: /// The "ledger components" — trader card, two-column offer/ask table,
00020: /// arbitrator, radio ticker — are already implemented inside
00021: /// TradeScreenGodotPanel and would be regressed by any wholesale refactor.
00022: /// This wrapper adds the Stitch dashboard chrome (sidebar nav for trade-flow
00023: /// actions + status rail of faction stance / trust / repels counters) and
00024: /// hosts TradeScreenGodotPanel inside the dashboard shell content slot.
00025: ///
00026: /// The trade engine (EconomyHostSession / IFactionStanceProvider /
00027: /// IPriceShockProvider / IFactionRadioProvider) remains the authoritative
00028: /// source. The wrapper reads stance / trust / aggression / repels strictly
00029: /// from existing session APIs to populate the status rail — no fake
00030: /// metrics, no derived values not already exposed by the engine.
00031: ///
00032: /// Sub-section nav (sidebar) lets the user jump between:
00033: ///   • Caravan context (faction profile, headline stance)
00034: ///   • Player offer column (open the existing player offer list)
00035: ///   • Faction stock column (open the existing faction asks)
00036: ///   • Arbitrator scale (open the existing fair-deal strip)
00037: ///
00038: /// The existing "ACCEPT BARTER" / "DEMAND PARLEY" buttons still live inside
00039: /// TradeScreenGodotPanel. This wrapper does not re-implement the wiring; it
00040: /// only routes the user toward the right sub-section via sidebar selection.
00041: /// </summary>
00042: public partial class CaravanBarterLedgerPanel : Control, IBindablePanel
00043: {
00044:     public event Action? OnClose;
00045:     public event Action<string>? OnSetActiveFaction;
00046:
00047:     private AshfallDashboardShell _shell = null!;
00048:     private AshfallSidebar? _sidebar;
00049:     private AshfallStatusRail? _statusRail;
00050:     private TradeScreenGodotPanel _tradeInner = null!;
00051:
00052:     private EconomyHostSession? _session;
00053:     private IFactionStanceProvider? _stance;
00054:     private string _activeFactionId = "scavenger_camp";
00055:
00056:     public bool IsBound => _tradeInner != null && _session != null;
00057:
00058:     public void Bind(
00059:         EconomyHostSession session,
00060:         IFactionStanceProvider? stanceProvider = null,
00061:         IPriceShockProvider? priceShockProvider = null,
00062:         IFactionRadioProvider? radioProvider = null,
00063:         ISeededRng? rng = null)
00064:     {
00065:         if (_session != null)
00066:             _session.StateChanged -= RefreshView;
00067:         _session = session;
00068:         _stance = stanceProvider;
00069:         if (_session != null)
00070:             _session.StateChanged += RefreshView;
00071:         if (_tradeInner != null)
00072:         {
00073:             _tradeInner.BindSession(session, stanceProvider!, priceShockProvider!, radioProvider!, rng!);
00074:         }
00075:         RefreshView();
00076:     }
00077:
00078:     public void BindViewModel(ITradeScreenViewModel viewModel, ITradeIntentSink intentSink)
00079:     {
00080:         if (_tradeInner != null)
00081:         {
00082:             _tradeInner.BindViewModel(viewModel, intentSink);
00083:         }
00084:     }
00085:
00086:     public void SetActiveFaction(string factionId)
00087:     {
00088:         _activeFactionId = factionId ?? "scavenger_camp";
00089:         if (_tradeInner != null)
00090:         {
00091:             _tradeInner.SetActiveFaction(_activeFactionId);
00092:         }
00093:         RefreshView();
00094:     }
00095:
00096:     public void RefreshView()
00097:     {
00098:         if (_statusRail == null || _tradeInner == null) return;
00099:         if (_session == null || _stance == null)
00100:         {
00101:             _statusRail.Set("faction",  "—",            AshfallMetricCard.Criticality.Normal);
00102:             _statusRail.Set("stance",   "—",            AshfallMetricCard.Criticality.Normal);
00103:             _statusRail.Set("trust",    "0",            AshfallMetricCard.Criticality.Normal);
00104:             _statusRail.Set("aggress",  "0.00",         AshfallMetricCard.Criticality.Normal);
00105:             _statusRail.Set("repels",   "0",            AshfallMetricCard.Criticality.Normal);
00106:             return;
00107:         }
00108:
00109:         var stance = _stance.GetStance(_activeFactionId);
00110:         float trust = _stance.GetEffectiveTrust(_activeFactionId);
00111:         float aggression = _stance.GetRaidAggression(_activeFactionId);
00112:         int consecutiveRepels = SafeGetConsecutiveRepels(_stance, _activeFactionId);
00113:
00114:         string stanceLabel = stance.ToString().ToUpperInvariant();
00115:         var stanceCrit = stance.ToString() switch
00116:         {
00117:             "Trade" => AshfallMetricCard.Criticality.Normal,
00118:             "Rob" => AshfallMetricCard.Criticality.Warn,
00119:             "HostileRaid" => AshfallMetricCard.Criticality.Critical,
00120:             _ => AshfallMetricCard.Criticality.Caution,
00121:         };
00122:
00123:         var trustCrit = trust >= 50 ? AshfallMetricCard.Criticality.Normal
00124:             : trust >= 0 ? AshfallMetricCard.Criticality.Caution
00125:             : trust >= -25 ? AshfallMetricCard.Criticality.Warn
00126:             : AshfallMetricCard.Criticality.Critical;
00127:
00128:         _statusRail.Set("faction",  _activeFactionId.Replace('_', ' ').ToUpperInvariant(), AshfallMetricCard.Criticality.Normal);
00129:         _statusRail.Set("stance",   $"[{stanceLabel}]", stanceCrit);
00130:         _statusRail.Set("trust",    trust > 0 ? $"+{trust:0}" : $"{trust:0}",       trustCrit);
00131:         _statusRail.Set("aggress",  $"{aggression:0.00}",                            AshfallMetricCard.Criticality.Normal);
00132:         _statusRail.Set("repels",   $"{consecutiveRepels}",                          AshfallMetricCard.Criticality.Normal);
00133:     }
00134:
00135:     private static int SafeGetConsecutiveRepels(IFactionStanceProvider stance, string factionId)
00136:     {
00137:         try
00138:         {
00139:             // The stance engine exposes a negotiated-count accessor in HoldfastTradeSession,
00140:             // but the IFactionStanceProvider abstraction does not. We probe via a known
00141:             // property pattern, otherwise fall back to zero — never invent data.
00142:             var t = stance.GetType();
00143:             var prop = t.GetProperty("ConsecutiveRepels");
00144:             if (prop != null)
00145:             {
00146:                 var raw = prop.GetValue(stance);
00147:                 if (raw is int i) return i;
00148:             }
00149:             return 0;
00150:         }
00151:         catch (Exception ex_CATDIAG)
00152:         {
00153:             CatalogDiagnostics.Warn("<reflection>", "ConsecutiveRepels property", ex_CATDIAG);
00154:             return 0;
00155:         }
00156:     }
00157:
00158:     public override void _Ready()
00159:     {
00160:         SetAnchorsPreset(LayoutPreset.FullRect);
00161:         Visible = false;
00162:
00163:         var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.90f) };
00164:         bg.SetAnchorsPreset(LayoutPreset.FullRect);
00165:         AddChild(bg);
00166:
00167:         var center = new CenterContainer();
00168:         center.SetAnchorsPreset(LayoutPreset.FullRect);
00169:         AddChild(center);
00170:
00171:         _shell = new AshfallDashboardShell(
00172:             "CARAVAN BARTER LEDGER — OPEN_TRADE_TABLE",
00173:             1100, 720);
00174:         center.AddChild(_shell);
00175:
00176:         _sidebar = _shell.SetSidebar(new[]
00177:         {
00178:             new AshfallSidebar.Item { Id = "context",      Label = "Context",       Hint = "Faction profile" },
00179:             new AshfallSidebar.Item { Id = "your_offers",  Label = "Your Offers",   Hint = "Player edge" },
00180:             new AshfallSidebar.Item { Id = "their_asks",   Label = "Their Asks",    Hint = "Faction edge" },
00181:             new AshfallSidebar.Item { Id = "fairness",     Label = "Fairness",      Hint = "DEAL IS FAIR indicator" },
00182:             new AshfallSidebar.Item { Id = "biology",      Label = "Biology",       Hint = "Biological drawer" },
00183:         }, "LEDGER OPS", "context");
00184:         _statusRail = _shell.SetStatusRail();
00185:         _statusRail.AddCard("faction", "FACTION",   "—",        AshfallMetricCard.Criticality.Normal, 180);
00186:         _statusRail.AddCard("stance",  "STANCE",    "—",        AshfallMetricCard.Criticality.Normal, 140);
00187:         _statusRail.AddCard("trust",   "TRUST",     "0",        AshfallMetricCard.Criticality.Normal, 110);
00188:         _statusRail.AddCard("aggress", "AGGRESSION","0.00",     AshfallMetricCard.Criticality.Normal, 130);
00189:         _statusRail.AddCard("repels",  "REPELS",    "0",        AshfallMetricCard.Criticality.Normal, 110);
00190:
00191:         _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());
00192:
00193:         // TradeScreenGodotPanel builds its own internal UI when added to the tree.
00194:         // We reparent it into the shell's content slot so the existing chrome
00195:         // plots inside the dashboard frame.
00196:         _tradeInner = new TradeScreenGodotPanel();
00197:         _tradeInner.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00198:         _tradeInner.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
00199:         _shell.SetContent(_tradeInner);
00200:
00201:         if (_session != null)
00202:         {
00203:             _tradeInner.BindSession(_session, _stance!);
00204:         }
00205:
00206:         // Sidebar ids are ledger section ops (context/your_offers/their_asks/
00207:         // fairness/biology), not faction ids. Never route section ids through
00208:         // SetActiveFaction / OnSetActiveFaction — that would corrupt stance rail.
00209:         if (_sidebar != null)
00210:         {
00211:             _sidebar.OnSelected += id =>
00212:             {
00213:                 switch (id)
00214:                 {
00215:                     case "context":
00216:                     case "your_offers":
00217:                     case "their_asks":
00218:                     case "fairness":
00219:                     case "biology":
00220:                         _tradeInner?.FocusLedgerSection(id);
00221:                         break;
00222:                     default:
00223:                         // Real faction ids only.
00224:                         SetActiveFaction(id);
00225:                         OnSetActiveFaction?.Invoke(id);
00226:                         break;
00227:                 }
00228:                 RefreshView();
00229:             };
00230:         }
00231:         RefreshView();
00232:     }
00233:
00234:     public void Open()
00235:     {
00236:         Visible = true;
00237:         _tradeInner.Visible = true;
00238:         _tradeInner.Open();
00239:         RefreshView();
00240:     }
00241:
00242:     public void Close() {
00243:             _tradeInner.Close();
00244:             if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
00245:                 Visible = false;
00246:             OnClose?.Invoke();
00247:         }
00248:
00249:     public override void _UnhandledInput(InputEvent @event)
00250:     {
00251:         if (!Visible) return;
00252:         if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00253:         {
00254:             OnClose?.Invoke();
00255:             GetViewport().SetInputAsHandled();
00256:         }
00257:     }
00258:
00259:
00260:     public void Unbind()
00261:     {
00262:         if (_session != null)
00263:         {
00264:             _session.StateChanged -= RefreshView;
00265:         }
00266:     }
00267:
00268:     public override void _ExitTree()
00269:         {
00270:             Unbind();
00271:             base._ExitTree();
00272:         }
00273: }
```


# Appendix — Current Source Detail: `src/Main.Economy.cs`

### `src/Main.Economy.cs` — complete current file

- Size: 366 lines / 15389 bytes.
- SHA-256: `2043c87b9cfaed583952235f93761567e656a645d675090b74cf213fe2e6c1ef`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using Godot;
00003: using System;
00004: using System.Globalization;
00005: using System.IO;
00006: using System.Linq;
00007: using System.Collections.Generic;
00008: using AtomicWar.Journal;
00009: using Ashfall.Core;
00010: using Ashfall.Core.Campaign;
00011: using Ashfall.Core.Economy;
00012: using Ashfall.Core.Expeditions;
00013: using Ashfall.Core.Foundry;
00014: using Ashfall.Core.Inventory;
00015: using Ashfall.Core.Journal;
00016: using Ashfall.Core.Muster;
00017: using Ashfall.Core.YearOfAsh;
00018: using Ashfall.Core.Radio;
00019: using Ashfall.Core.Survivors;
00020: using AtomicWar.GodotApp.Economy;
00021: using AtomicWar.GodotApp.YearOfAsh;
00022: using AtomicWar.GodotApp.Muster;
00023: using AtomicWar.GodotApp.Dose;
00024: using AtomicWar.GodotApp.UtilityAI;
00025: using AtomicWar.GodotApp.Radio;
00026: using AtomicWar.GodotApp.Audio;
00027: using AtomicWar.GodotApp.UI;
00028:
00029: namespace AtomicWar.GodotApp
00030: {
00031:     public partial class Main : Control
00032:     {
00033:         // ── Economy fields (GAP-ARCH-01 Phase 1) ──
00034:         private EconomyHostSession _economy = null!;
00035:         private bool _economyDirty;
00036:         private TravelingCaravanHostSession _caravans = null!;
00037:         private bool _caravansDirty;
00038:         private AtomicWar.GodotApp.Economy.TradeScreenGodotPanel _tradePanel = null!;
00039:         private Ashfall.Core.Radio.FactionRadioEngine _tradeRadio = null!;
00040:         private TradeVoiceResolver _tradeVoiceResolver = null!;
00041:
00042:         private void FlushCaravanIfDirty()
00043:         {
00044:             if (_caravansDirty) SaveCaravans();
00045:         }
00046:
00047:         private void SetupEconomy()
00048:         {
00049:             if (_economy != null) return;
00050:             _economy = EconomyHostSession.Create(_dataDir);
00051:             _economy.BindRationingResourceValidator(IsCanonicalRationingResource);
00052:             // Plan 42 / Plan 46 — the rationing owner's tier change reaches the
00053:             // journal voice trigger and the session telemetry through this one
00054:             // forwarder; no second ration model is created in the host.
00055:             _economy.RationTierChangedSeam += target =>
00056:             {
00057:                 RecordPlayMetricRationPolicyChanged(target);
00058:                 TriggerSurvivorVoiceRationCut(target);
00059:             };
00060:             BindRationingToInventory();
00061:             _economy.StateChanged += () => _economyDirty = true;
00062:
00063:             // Plan 212 follow-up — trade rumors from real market state: a shock
00064:             // the canonical market applies (or expires) becomes one band item.
00065:             // The text is Core-projected; this adapter only relays it once per
00066:             // event. Restore never re-fires the events, so no replay.
00067:             _economy.Market.OnShockStarted += shock =>
00068:             {
00069:                 if (shock == null) return;
00070:                 SetupRadio();
00071:                 _radio?.RecordMarketRumor(
00072:                     Ashfall.Core.Economy.EconomyMarketRumorRules.ShockStartedLine(shock), shock.startDay);
00073:             };
00074:             _economy.Market.OnShockExpired += shock =>
00075:             {
00076:                 if (shock == null) return;
00077:                 SetupRadio();
00078:                 _radio?.RecordMarketRumor(
00079:                     Ashfall.Core.Economy.EconomyMarketRumorRules.ShockExpiredLine(shock), shock.startDay);
00080:             };
00081:
00082:             var save = EconomySaveStore.TryLoad();
00083:             if (save != null)
00084:             {
00085:                 _economy.Market.RestoreState(save);
00086:                 _economyDirty = false; // restore just raised state-change events
00087:                 GD.Print("[Ashfall Godot] Economy state restored.");
00088:             }
00089:
00090:             if (_economyPanel == null && _rightColumn != null)
00091:             {
00092:                 _economyPanel = new EconomyMarketPanel();
00093:                 _rightColumn.AddChild(_economyPanel);
00094:             }
00095:             if (_economyPanel != null)
00096:             {
00097:                 _economyPanel.BindSession(_economy);
00098:                 _economyPanel.RefreshView();
00099:             }
00100:         }
00101:
00102:         private bool IsCanonicalRationingResource(string resourceId)
00103:         {
00104:             if (string.IsNullOrWhiteSpace(resourceId)) return false;
00105:             string id = resourceId.Trim();
00106:             return (_inventory?.Catalog?.Get(id) != null)
00107:                 || (_economy?.Catalog?.Find(id) != null)
00108:                 || GoodCategories.IsKnown(id);
00109:         }
00110:
00111:         private void BindRationingToInventory()
00112:         {
00113:             if (_inventory == null || _economy == null) return;
00114:             _inventory.RationingAuthorizer = (resourceId, consumerId, demand, day, available) =>
00115:                 _economy.AuthorizeAllocation(resourceId, consumerId, demand, available, day);
00116:         }
00117:
00118:         private void OnEconomyOpenClicked()
00119:         {
00120:             SetupEconomy();
00121:             _statusLabel.Text = _economy.StatusLine();
00122:             _codexViewer.Text = _economy.StatusLine();
00123:         }
00124:
00125:         private void OnEconomySaveClicked()
00126:         {
00127:             SetupEconomy();
00128:             SaveEconomy();
00129:         }
00130:
00131:         private void SaveEconomy()
00132:         {
00133:             if (_economy == null) return;
00134:             if (CaptureSection("economy", EconomySaveStore.TryCapturePersisted(_economy.CaptureSave())))
00135:             {
00136:                 _economyDirty = false;
00137:                 GD.Print("[Ashfall Godot] Economy save written.");
00138:             }
00139:         }
00140:
00141:         private void FlushEconomyIfDirty()
00142:         {
00143:             if (_economyDirty) SaveEconomy();
00144:         }
00145:
00146:         /// <summary>
00147:         /// Plan 212 — weather→market shock bridge. The weather authority owns
00148:         /// weather; the market owns its indices; the band mapping is Core
00149:         /// policy (<see cref="EconomyWeatherShockRules"/>). This adapter only
00150:         /// reads the canonical weather state and applies the bounded,
00151:         /// idempotent shock. Deterministic: pure function of current weather.
00152:         /// Plan 14A — the embargo authority FIRST advances its decay state with
00153:         /// the same authoritative weather (activation + decay are Core state);
00154:         /// the market then applies the decay-aware multiplier as one embargo
00155:         /// factor. No embargo shock ever enters ApplyShock — the two shock
00156:         /// paths stay separate factors in the canonical price equation.
00157:         /// </summary>
00158:         private void TickEconomyWeatherBridge(int day)
00159:         {
00160:             if (_economy == null) return;
00161:             if (_world?.Weather == null) return;
00162:             _economy.EmbargoSystem?.NotifyWeather(day, _world.Weather.Current);
00163:             var band = EconomyWeatherShockRules.TryGetWeatherShock(_world.Weather.Current);
00164:             if (band == null) return;
00165:             _economy.Market.ApplyShock(
00166:                 band.CategoryId, band.IsShortage, band.SeverityBp,
00167:                 startDay: day, durationDays: band.DurationDays, sourceId: band.SourceId);
00168:         }
00169:
00170:         private void SetupCaravans()
00171:         {
00172:             if (_caravans != null) return;
00173:             SetupEconomy();
00174:             _caravans = TravelingCaravanHostSession.Create(_dataDir);
00175:             // Plan 14A — caravans share the campaign's ONE embargo authority
00176:             // (rules from trade_embargoes.json); route blocking evaluates the
00177:             // same rules the market prices from.
00178:             _caravans.Engine.Embargoes = _economy.EmbargoSystem;
00179:             SetupWorld();
00180:             _caravans.Engine.Map = _world?.WastelandMap;
00181:             // C2 / Plan 20C (§41) — weather availability from the ONE effects
00182:             // table, combined with (never mixed into) the embargo multiplier.
00183:             _caravans.Engine.WeatherAvailabilityProvider = weather =>
00184:             {
00185:                 var effects = _world?.WeatherEffects;
00186:                 if (effects != null && effects.TryGetEffects(weather, out var fx) && fx != null)
00187:                     return fx.caravan_availability_multiplier;
00188:                 return 1f;
00189:             };
00190:             _caravans.StateChanged += () => _caravansDirty = true;
00191:             GD.Print("[Ashfall Godot] Caravan host ready.");
00192:         }
00193:
00194:         private void SaveCaravans()
00195:         {
00196:             if (_caravans == null) return;
00197:             if (CaptureSection("caravan", CaravanSaveStore.TryCapturePersisted(_caravans.CaptureSave())))
00198:             {
00199:             _caravansDirty = false;
00200:             _yearOfAshDirty = false;
00201:                 GD.Print("[Ashfall Godot] Caravan save written.");
00202:             }
00203:         }
00204:
00205:         private void SaveSilentFoundry()
00206:         {
00207:             if (_silentFoundry == null) return;
00208:             try
00209:             {
00210:                 CaptureSection("silent_foundry", SilentFoundrySaveStore.TryCapturePersisted(_silentFoundry.Engine.CaptureState()));
00211:             }
00212:             catch (Exception e)
00213:             {
00214:                 GD.PushWarning("[Ashfall Godot] SilentFoundry save failed: " + e.Message);
00215:             }
00216:         }
00217:
00218:         private void SetupSilentFoundry()
00219:         {
00220:             if (_silentFoundry != null) return;
00221:             SetupExpansions();
00222:             SetupInventory();
00223:             SetupJournal();
00224:             SetupEconomy();
00225:             SetupPowerGrid();
00226:             _silentFoundry = AtomicWar.GodotApp.SilentFoundryHostSession.Create(
00227:                 _dataDir, _expansions, _inventory, _journal, market: _economy.Market,
00228:                 seedSupplies: _campaignInitializationMode == CampaignInitializationMode.FreshInitialize);
00229:             _silentFoundry.BindPowerAndThermal(_powerGrid?.System, _shelterThermal?.System);
00230:             // Plan B66: heavy batches emit smoke/CO through the canonical
00231:             // ventilation authority (register/deactivate around each batch).
00232:             _silentFoundry.Engine.BindVentilation(_ventilation);
00233:             // GAP-STUB-03 (resolved): wire the remaining FactionStanceEngine
00234:             // providers as live accessors into Main state, not one-time
00235:             // captured values, so guild trust reflects the campaign's actual
00236:             // current day, radiation, and military-survivor presence on every
00237:             // future read — including after the values change post-bind.
00238:             _silentFoundry.BindStanceProviders(
00239:                 campaignDayProvider: () => _simDay,
00240:                 partyRadiationProvider: () => _holdfastRuntime?.Radiation ?? 0f,
00241:                 survivorsProvider: () => _survivors);
00242:             // v6 SaltMine: hub envelope already restored into expansions/foundry
00243:             // Core systems; SaltMine lives on this host session — restore it
00244:             // from the same hub payload when present.
00245:             var hubSave = ExpansionHubSaveStore.TryLoad();
00246:             if (hubSave?.saltMine != null)
00247:                 _silentFoundry.SaltMine.RestoreState(hubSave.saltMine);
00248:             // Foundry + SaltMine ride the expansion-hub save; state-change
00249:             // events mark the hub save dirty so nothing is lost.
00250:             _silentFoundry.StateChanged += () =>
00251:             {
00252:                 _foundryDirty = true;
00253:                 _silentFoundryPanel?.RefreshView();
00254:                 _factionsPanel?.RefreshView();
00255:                 _economyPanel?.RefreshView();
00256:                 if (_state == GameState.Playing) UpdateHud();
00257:             };
00258:             if (_silentFoundryPanel != null)
00259:             {
00260:                 _silentFoundryPanel.Bind(_silentFoundry, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay);
00261:                 _silentFoundryPanel.SetMachineTellCatalog(GetMachineTellCatalog());
00262:             }
00263:             // Live market strip: show the guild's real trade access at all times.
00264:             if (_economyPanel != null)
00265:                 _economyPanel.BindStance(_silentFoundry.GuildStanceEngine, Ashfall.Core.Foundry.SilentFoundryIds.FactionId);
00266:             GD.Print("[Ashfall Godot] Silent Foundry host ready (exp_10_the_silent_foundry).");
00267:         }
00268:
00269:         private void CloseSilentFoundryPanel()
00270:         {
00271:             _silentFoundryPanel.Visible = false;
00272:         }
00273:
00274:         private void CloseTradePanel()
00275:         {
00276:             if (_silentFoundry != null)
00277:                 _silentFoundry.StateChanged -= _tradePanel.RefreshView;
00278:             _tradePanel.Visible = false;
00279:         }
00280:
00281:         private TradeVoiceResolver GetTradeVoiceResolver()
00282:         {
00283:             if (_tradeVoiceResolver != null) return _tradeVoiceResolver;
00284:
00285:             var load = TradeTextCatalogLoader.Load(
00286:                 _dataDir,
00287:                 new FileSystemIO(),
00288:                 new SystemTextJsonSerializer());
00289:             if (!load.IsValid)
00290:             {
00291:                 GD.PushWarning("[Ashfall Godot] Trade voice catalog using fallback: "
00292:                     + (load.Errors.Count == 0
00293:                         ? "catalog missing"
00294:                         : string.Join("; ", load.Errors)));
00295:             }
00296:
00297:             _tradeVoiceResolver = new TradeVoiceResolver(load.Catalog);
00298:             return _tradeVoiceResolver;
00299:         }
00300:
00301:         private HardcoreEconomyTuning LoadHardcoreEconomyTuning()
00302:         {
00303:             var tuning = new HardcoreEconomyTuning();
00304:             string path = Path.Combine(_dataDir, "hardcore_economy_tuning.json");
00305:             if (!File.Exists(path))
00306:             {
00307:                 GD.PushWarning($"[Ashfall Godot] Hardcore economy tuning missing: {path}");
00308:                 return tuning;
00309:             }
00310:
00311:             var result = HardcoreEconomyTuningLoader.Load(File.ReadAllText(path));
00312:             if (!result.IsValid || result.Bundle == null)
00313:             {
00314:                 GD.PushWarning("[Ashfall Godot] Hardcore economy tuning rejected: "
00315:                     + string.Join("; ", result.Errors));
00316:                 return tuning;
00317:             }
00318:
00319:             tuning.Apply(result.Bundle);
00320:             return tuning;
00321:         }
00322:
00323:         /// <summary>
00324:         /// Open the live trade screen bound to the Foundry Guild's real stance
00325:         /// engine (derived from the durable consequence ledger). The panel's
00326:         /// confirm gate follows TradeStance: below Trade the stall is blocked.
00327:         /// </summary>
00328:         private void OpenTradeScreen()
00329:         {
00330:             if (_tradePanel == null) return;
00331:             if (_tradeRadio == null)
00332:             {
00333:                 string radioPath = Path.Combine(_dataDir, "faction_radio_corpus.json");
00334:                 _tradeRadio = Ashfall.Core.Radio.FactionRadioEngine.LoadFromJson(
00335:                     System.IO.File.Exists(radioPath) ? System.IO.File.ReadAllText(radioPath) : "{}");
00336:             }
00337:             var tuning = LoadHardcoreEconomyTuning();
00338:             SetupCampaignDay();
00339:             _tradePanel.BindSession(
00340:                 _economy,
00341:                 _silentFoundry.GuildStanceEngine,
00342:                 tuning,
00343:                 _tradeRadio,
00344:                 _campaignDay.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.Economy).Rng,
00345:                 GetTradeVoiceResolver());
00346:             _tradePanel.SetActiveFaction(Ashfall.Core.Foundry.SilentFoundryIds.FactionId);
00347:             // Live refresh when a treaty consequence moves the guild's standing
00348:             // (subscribe once per open; CloseTradePanel removes it).
00349:             _silentFoundry.StateChanged -= _tradePanel.RefreshView;
00350:             _silentFoundry.StateChanged += _tradePanel.RefreshView;
00351:             _tradePanel.Open();
00352:             GD.Print($"[Ashfall Godot] Trade screen open — Foundry Guild stance {_silentFoundry.GuildStance} · trust {_silentFoundry.GuildTrust:F0}");
00353:         }
00354:
00355:         private void CloseEconomyPanel()
00356:         {
00357:             _economyPanel.Visible = false;
00358:         }
00359:
00360:         private void CloseEconomyDetailPanel()
00361:         {
00362:             _economyDetailPanel.Visible = false;
00363:         }
00364:
00365:     }
00366: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Radiation/Plan81_62DoseTradeTellIntegrationTests.cs`

### `Ashfall.Core.Tests/Radiation/Plan81_62DoseTradeTellIntegrationTests.cs` — complete current file

- Size: 165 lines / 7150 bytes.
- SHA-256: `1d031e831d793301f400db8fcc6b9122a0a307a6607bfca9d1f052377e32d14c`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Economy;
00008: using Ashfall.Core.IO;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests
00012: {
00013:     /// <summary>
00014:     /// Wave 39 Batch 2 Integration Suite:
00015:     /// - Plan 81 (DEC-241): Radiation Dose Locations Expansion (5 → 14 locations)
00016:     /// - Plan 62 (DEC-242): Trade Tell Lines & Negotiation Tells (4 bands × 5 stances → 60 tell lines)
00017:     ///
00018:     /// Validates cross-system integration between environmental radiation hotspots
00019:     /// and wasteland barter psychology: sector dose mapping, trader trust band
00020:     /// resolution, and seed-deterministic tell rotation under harsh wasteland exposure.
00021:     /// </summary>
00022:     public sealed class Plan81_62DoseTradeTellIntegrationTests
00023:     {
00024:         private static string FindDataDir()
00025:         {
00026:             string dataDir;
00027:             if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
00028:                 CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
00029:             return dataDir ?? string.Empty;
00030:         }
00031:
00032:         private static DoseContentCatalog LoadDoseCatalog()
00033:         {
00034:             string dataDir = FindDataDir();
00035:             Assert.False(string.IsNullOrEmpty(dataDir), "Could not locate StreamingAssets/Data directory");
00036:             return DoseContentCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00037:         }
00038:
00039:         private static TradeTellEngine LoadTellEngine()
00040:         {
00041:             string dataDir = FindDataDir();
00042:             string path = Path.Combine(dataDir, "trade_tell_lines.json");
00043:             Assert.True(File.Exists(path), "trade_tell_lines.json must exist");
00044:             return TradeTellEngine.LoadFromJson(File.ReadAllText(path));
00045:         }
00046:
00047:         [Fact]
00048:         public void Plan81_62_DoseLocations_SectorMappingAndRiskCalibration()
00049:         {
00050:             var catalog = LoadDoseCatalog();
00051:             Assert.NotNull(catalog.locations);
00052:             Assert.Equal(14, catalog.locations.Count);
00053:
00054:             // Verify all 4 canonical sectors are present
00055:             var sectors = new HashSet<string>(catalog.locations.Select(l => l.sector), StringComparer.Ordinal);
00056:             Assert.Contains("bunker", sectors);
00057:             Assert.Contains("surface", sectors);
00058:             Assert.Contains("expedition", sectors);
00059:             Assert.Contains("faction", sectors);
00060:
00061:             // Verify dose calibrations: bunker is shielded, expedition has hot zones
00062:             foreach (var loc in catalog.locations)
00063:             {
00064:                 Assert.False(string.IsNullOrWhiteSpace(loc.id));
00065:                 Assert.StartsWith("loc_", loc.id);
00066:                 Assert.False(string.IsNullOrWhiteSpace(loc.displayName));
00067:                 Assert.InRange(loc.riskLevel, 0, 8);
00068:                 Assert.True(loc.radiationUsv >= 0.01f, $"Location {loc.id} must have positive dose");
00069:
00070:                 if (loc.sector == "bunker")
00071:                 {
00072:                     Assert.InRange(loc.riskLevel, 0, 2);
00073:                     Assert.True(loc.radiationUsv <= 0.5f, $"Bunker location {loc.id} dose exceeds 0.5 uSv/h");
00074:                 }
00075:                 else if (loc.sector == "expedition")
00076:                 {
00077:                     Assert.True(loc.riskLevel >= 3, $"Expedition location {loc.id} risk below 3");
00078:                     Assert.True(loc.radiationUsv >= 5.0f, $"Expedition hot zone {loc.id} dose below 5.0 uSv/h");
00079:                 }
00080:             }
00081:         }
00082:
00083:         [Fact]
00084:         public void Plan81_62_TradeTell_TrustBands_And_PoolCoverage()
00085:         {
00086:             var engine = LoadTellEngine();
00087:
00088:             Assert.Equal(4, engine.BandCount);
00089:             Assert.Equal(20, engine.PoolCount); // 5 stances x 4 bands
00090:             Assert.True(engine.LineCount >= 60, $"Expected >= 60 tell lines, found {engine.LineCount}");
00091:
00092:             // Verify trust band mapping thresholds
00093:             Assert.Equal(TradeTrustBands.Hostile, engine.BandForTrust(-75f));
00094:             Assert.Equal(TradeTrustBands.Hostile, engine.BandForTrust(-40f));
00095:             Assert.Equal(TradeTrustBands.Wary, engine.BandForTrust(-20f));
00096:             Assert.Equal(TradeTrustBands.Wary, engine.BandForTrust(0f));
00097:             Assert.Equal(TradeTrustBands.Neutral, engine.BandForTrust(20f));
00098:             Assert.Equal(TradeTrustBands.Neutral, engine.BandForTrust(40f));
00099:             Assert.Equal(TradeTrustBands.Warm, engine.BandForTrust(60f));
00100:             Assert.Equal(TradeTrustBands.Warm, engine.BandForTrust(100f));
00101:         }
00102:
00103:         [Fact]
00104:         public void Plan81_62_HotspotTrade_TellRotation_SeededDeterminism()
00105:         {
00106:             var doseCatalog = LoadDoseCatalog();
00107:             var engine = LoadTellEngine();
00108:
00109:             // Locate an expedition hot zone
00110:             var hotZone = doseCatalog.locations.Find(l => l.sector == "expedition" && l.radiationUsv >= 30.0f);
00111:             Assert.NotNull(hotZone);
00112:
00113:             const int seed = 428162;
00114:             var rng1 = new SeededRng(seed);
00115:             var rng2 = new SeededRng(seed);
00116:
00117:             var sequence1 = new List<string>();
00118:             var sequence2 = new List<string>();
00119:
00120:             // Simulate 10 trade negotiation rounds in a high-risk zone under wary trust
00121:             for (int i = 0; i < 10; i++)
00122:             {
00123:                 bool ok1 = engine.TrySelectTell(TradeStance.Trade, trust: -15f, rng1, out var tell1);
00124:                 Assert.True(ok1);
00125:                 sequence1.Add(tell1.Line);
00126:
00127:                 bool ok2 = engine.TrySelectTell(TradeStance.Trade, trust: -15f, rng2, out var tell2);
00128:                 Assert.True(ok2);
00129:                 sequence2.Add(tell2.Line);
00130:             }
00131:
00132:             // Verify exact deterministic replay matching
00133:             Assert.Equal(sequence1.Count, sequence2.Count);
00134:             for (int i = 0; i < sequence1.Count; i++)
00135:             {
00136:                 Assert.Equal(sequence1[i], sequence2[i]);
00137:             }
00138:         }
00139:
00140:         [Fact]
00141:         public void Plan81_62_DoseLocation_RadiationStress_ModulatesTraderStance()
00142:         {
00143:             var doseCatalog = LoadDoseCatalog();
00144:             var engine = LoadTellEngine();
00145:
00146:             // Check high radiation locations
00147:             var extremeLoc = doseCatalog.locations.OrderByDescending(l => l.radiationUsv).First();
00148:             Assert.True(extremeLoc.radiationUsv >= 40f);
00149:             Assert.Equal("loc_military_depot_perimeter", extremeLoc.id);
00150:
00151:             var rng = new SeededRng(999);
00152:
00153:             // In an extreme radiation hot zone, a desperate trade under Refuse or Rob stance
00154:             // selects legible, restrained posture lines within bounds
00155:             foreach (TradeStance stance in new[] { TradeStance.Refuse, TradeStance.Rob, TradeStance.HostileRaid })
00156:             {
00157:                 bool selected = engine.TrySelectTell(stance, trust: -50f, rng, out var tell);
00158:                 Assert.True(selected);
00159:                 Assert.Equal(TradeTrustBands.Hostile, tell.Band);
00160:                 Assert.InRange(tell.Line.Length, 20, 140);
00161:                 Assert.False(string.IsNullOrWhiteSpace(tell.Line));
00162:             }
00163:         }
00164:     }
00165: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/TradeScreenSeamTests.cs`

### `Ashfall.Core.Tests/TradeScreenSeamTests.cs` — complete current file

- Size: 329 lines / 14462 bytes.
- SHA-256: `7940a9f2745852452d4a37666199d3187914ce18d05551e675588fa27169cfc6`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Economy;
00007: using Xunit;
00008:
00009: namespace Ashfall.Core.Tests
00010: {
00011:     /// <summary>
00012:     /// Negotiation Table seam: Act 0 scenarios (fair / short / empty),
00013:     /// Track B presenter mapping + zero-mutation invariant + TradeScreenUI
00014:     /// API parity.
00015:     /// </summary>
00016:     public class TradeScreenSeamTests
00017:     {
00018:         private static string ReadDataFile(string fileName)
00019:         {
00020:             string path = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", fileName);
00021:             if (!File.Exists(path))
00022:             {
00023:                 path = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", fileName);
00024:             }
00025:             Assert.True(File.Exists(path), $"Data file not found at {path}");
00026:             return File.ReadAllText(path);
00027:         }
00028:
00029:         private static IReadOnlyList<TradeScreenScenario> LoadScenarios()
00030:         {
00031:             return TradeScreenScenarioLoader.LoadFromJson(ReadDataFile("trade_screen_scenarios.json"));
00032:         }
00033:
00034:         private static TradeTellEngine LoadTells()
00035:         {
00036:             return TradeTellEngine.LoadFromJson(ReadDataFile("trade_tell_lines.json"));
00037:         }
00038:
00039:         private static FactionStanceEngine CreateStanceEngine()
00040:         {
00041:             var engine = new FactionStanceEngine();
00042:             engine.RegisterFaction(new FactionThresholds(
00043:                 "scavenger_camp",
00044:                 raidThreshold: -50f,
00045:                 robThreshold: -20f,
00046:                 minTrustToTrade: -40f,
00047:                 intelShareThreshold: 40f,
00048:                 raidAggression: 0.35f,
00049:                 trustInversion: false,
00050:                 healthyRadiationCeiling: 20f,
00051:                 highRadiationFloor: 60f));
00052:             return engine;
00053:         }
00054:
00055:         // ── Act 0: mock scenarios ────────────────────────────────────
00056:
00057:         [Fact]
00058:         public void Scenarios_LoadAllThreeFromData()
00059:         {
00060:             var scenarios = LoadScenarios();
00061:
00062:             // Plan 61: catalog expanded from 3 to 15 (12 new scenarios across
00063:             // eight trader archetypes). The three originals remain pinned.
00064:             Assert.True(scenarios.Count >= 15, $"Expected at least 15 scenarios, got {scenarios.Count}");
00065:             Assert.Contains(scenarios, s => s.Id == "fair_deal");
00066:             Assert.Contains(scenarios, s => s.Id == "offer_short");
00067:             Assert.Contains(scenarios, s => s.Id == "empty_table");
00068:         }
00069:
00070:         [Fact]
00071:         public void Scenario_FairDeal_ComputedFairnessMatchesDataExpectation()
00072:         {
00073:             var binding = TradeScreenScenarioLoader.CreateBinding(
00074:                 GetScenario("fair_deal"), LoadTells(), new SeededRng(2026));
00075:
00076:             var vm = binding.ViewModel;
00077:             Assert.True(vm.IsOpen);
00078:             Assert.Equal(TradeFairness.Fair, vm.Fairness);
00079:             Assert.Equal("DEAL IS FAIR", vm.FairnessLabel);
00080:             Assert.True(vm.CanConfirm);
00081:             Assert.False(string.IsNullOrWhiteSpace(vm.StanceTellLine));
00082:
00083:             // 3x18 + 1x15 + 1 pint of blood (25) = 94 vs 2x22 = 44.
00084:             Assert.Equal(94f, vm.PlayerOfferValue, 2);
00085:             Assert.Equal(44f, vm.FactionAskValue, 2);
00086:
00087:             // Intent routing through the seam.
00088:             Assert.True(binding.Intents.TryConfirmTrade());
00089:             Assert.Equal(1, binding.Intents.ConfirmCalls);
00090:         }
00091:
00092:         [Fact]
00093:         public void Scenario_OfferShort_BlocksConfirmAndKeepsStanceLegible()
00094:         {
00095:             var binding = TradeScreenScenarioLoader.CreateBinding(
00096:                 GetScenario("offer_short"), LoadTells(), new SeededRng(2026));
00097:
00098:             var vm = binding.ViewModel;
00099:             Assert.Equal(TradeFairness.Short, vm.Fairness);
00100:             Assert.Equal("OFFER SHORT", vm.FairnessLabel);
00101:             Assert.False(vm.CanConfirm);
00102:             Assert.True(vm.ConsecutiveRepels > 0);
00103:
00104:             // The mock sink still routes and records intent; the verdict is data-defined.
00105:             Assert.False(binding.Intents.TryConfirmTrade());
00106:             Assert.Equal(1, binding.Intents.ConfirmCalls);
00107:         }
00108:
00109:         [Fact]
00110:         public void Scenario_EmptyTable_IsDeliberateNotBroken()
00111:         {
00112:             var binding = TradeScreenScenarioLoader.CreateBinding(
00113:                 GetScenario("empty_table"), LoadTells(), new SeededRng(2026));
00114:
00115:             var vm = binding.ViewModel;
00116:             Assert.Equal(TradeFairness.EmptyTable, vm.Fairness);
00117:             Assert.Equal("EMPTY TABLE", vm.FairnessLabel);
00118:             Assert.False(vm.CanConfirm);
00119:             Assert.Empty(vm.PlayerOffers);
00120:             Assert.Empty(vm.FactionDemands);
00121:             Assert.Empty(vm.BiologicalOffers);
00122:
00123:             // A deliberate posture: stance, tell, and radio all speak.
00124:             Assert.Equal(TradeStance.Refuse, vm.Stance);
00125:             Assert.False(string.IsNullOrWhiteSpace(vm.StanceTellLine));
00126:             Assert.False(string.IsNullOrWhiteSpace(vm.RadioTickerLine));
00127:         }
00128:
00129:         [Fact]
00130:         public void Scenario_IntentSink_CloseRecordsTradedFlag()
00131:         {
00132:             var binding = TradeScreenScenarioLoader.CreateBinding(
00133:                 GetScenario("fair_deal"), LoadTells(), new SeededRng(2026));
00134:
00135:             binding.Intents.Close(traded: true);
00136:             Assert.Equal(1, binding.Intents.CloseCalls);
00137:             Assert.True(binding.Intents.LastCloseWasTraded);
00138:         }
00139:
00140:         private static TradeScreenScenario GetScenario(string id)
00141:         {
00142:             var scenarios = LoadScenarios();
00143:             foreach (var s in scenarios)
00144:             {
00145:                 if (s.Id == id) return s;
00146:             }
00147:             Assert.Fail($"Scenario {id} not found");
00148:             return null;
00149:         }
00150:
00151:         // ── Track B: presenter ───────────────────────────────────────
00152:
00153:         /// <summary>Counting decorator proving the presenter never mutates providers.</summary>
00154:         private sealed class MutationCountingStanceProvider : IFactionStanceProvider
00155:         {
00156:             private readonly IFactionStanceProvider _inner;
00157:             public int Mutations { get; private set; }
00158:
00159:             public MutationCountingStanceProvider(IFactionStanceProvider inner) { _inner = inner; }
00160:
00161:             public TradeStance GetStance(string factionId) => _inner.GetStance(factionId);
00162:             public bool WillTrade(string factionId) => _inner.WillTrade(factionId);
00163:             public bool WillShareIntel(string factionId) => _inner.WillShareIntel(factionId);
00164:             public float GetTrust(string factionId) => _inner.GetTrust(factionId);
00165:             public float GetEffectiveTrust(string factionId) => _inner.GetEffectiveTrust(factionId);
00166:             public float ModifyTrust(string factionId, float delta) { Mutations++; return _inner.ModifyTrust(factionId, delta); }
00167:             public void SetTrust(string factionId, float value) { Mutations++; _inner.SetTrust(factionId, value); }
00168:             public float GetRaidAggression(string factionId) => _inner.GetRaidAggression(factionId);
00169:             public void SetRaidAggression(string factionId, float value) { Mutations++; _inner.SetRaidAggression(factionId, value); }
00170:             public bool IsFactionActive(string factionId) => _inner.IsFactionActive(factionId);
00171:         }
00172:
00173:         [Fact]
00174:         public void Presenter_MapsProvidersOntoViewModel()
00175:         {
00176:             var stance = CreateStanceEngine();
00177:             var tuning = new HardcoreEconomyTuning();
00178:             tuning.Apply(new HardcoreEconomyTuningBundle(
00179:                 new[] { new ScarcityEntry(ScarcityTier.Critical, 2.0f, "1-50", new[] { "clean_water" }, "drought") },
00180:                 Array.Empty<FactionTradePreference>(),
00181:                 new[] { new PriceShockRule(PriceShockKind.PlumePassing, 2.5f, 10, new[] { "rad_pills" }, "rad plume") }
00182:             ));
00183:
00184:             var presenter = new TradeScreenPresenter(
00185:                 stance, tuning, LoadTells(), new SeededRng(2026),
00186:                 unitPriceLookup: id => id == "clean_water" ? 22f : 18f);
00187:             presenter.SetWorldContext("CivilWar", 5);
00188:             presenter.SetWatchedItems(new[] { "clean_water" });
00189:
00190:             Assert.True(presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1));
00191:
00192:             var vm = presenter.ViewModel;
00193:             Assert.True(vm.IsOpen);
00194:             Assert.Equal(TradeStance.Trade, vm.Stance);
00195:             Assert.Equal(0f, vm.Trust, 2);
00196:             Assert.Equal(0.35f, vm.Aggression, 2);
00197:             Assert.False(string.IsNullOrWhiteSpace(vm.StanceTellLine));
00198:             Assert.Single(vm.ShockBadges);
00199:             Assert.Equal(PriceShockKind.PlumePassing, vm.ShockBadges[0].Kind);
00200:             Assert.Single(vm.ScarcityMultipliers);
00201:             Assert.Equal(2.0f, vm.ScarcityMultipliers[0].Multiplier, 2);
00202:         }
00203:
00204:         [Fact]
00205:         public void Presenter_ZeroMutation_InvariantHolds()
00206:         {
00207:             var counting = new MutationCountingStanceProvider(CreateStanceEngine());
00208:             var presenter = new TradeScreenPresenter(counting, null, LoadTells(), new SeededRng(2026));
00209:
00210:             presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
00211:             presenter.SetPlayerOffer("canned_food", 3);
00212:             presenter.SetFactionAsk("clean_water", 2);
00213:             presenter.SetBiologicalOffer(BiologicalTradeItem.PintOfBlood, 1);
00214:             presenter.Recalculate();
00215:             presenter.TryConfirmTrade();
00216:             presenter.TryDemandParley();
00217:             presenter.BuildQuoteSummary();
00218:             presenter.Close(traded: false);
00219:
00220:             Assert.Equal(0, counting.Mutations);
00221:         }
00222:
00223:         [Fact]
00224:         public void Presenter_ApiParity_TradeScreenUISurface()
00225:         {
00226:             var presenter = new TradeScreenPresenter(
00227:                 CreateStanceEngine(), null, LoadTells(), new SeededRng(2026),
00228:                 unitPriceLookup: id => id == "clean_water" ? 30f : 18f,
00229:                 displayNameLookup: id => id == "canned_food" ? "Canned Food" : "Clean Water");
00230:
00231:             // Open rejects inactive factions like TradeScreenUI.Open.
00232:             Assert.False(presenter.Open("unknown_nomads", "Nomads", "None", 1));
00233:             Assert.True(presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1));
00234:
00235:             // SetPlayerOffer / SetFactionAsk drive the fairness verdict.
00236:             presenter.SetPlayerOffer("canned_food", 1);   // 18
00237:             presenter.SetFactionAsk("clean_water", 1);    // 30
00238:             Assert.Equal(TradeFairness.Short, presenter.ViewModel.Fairness);
00239:             Assert.False(presenter.TryConfirmTrade());
00240:
00241:             presenter.SetPlayerOffer("canned_food", 3);   // 54 >= 30
00242:             Assert.Equal(TradeFairness.Fair, presenter.ViewModel.Fairness);
00243:             Assert.True(presenter.TryConfirmTrade());
00244:
00245:             // Successful confirm clears the table, like TradeScreenUI.
00246:             Assert.Equal(TradeFairness.EmptyTable, presenter.ViewModel.Fairness);
00247:             Assert.Empty(presenter.ViewModel.PlayerOffers);
00248:
00249:             // BuildQuoteSummary is qualitative — no raw digit totals.
00250:             presenter.SetPlayerOffer("canned_food", 2);
00251:             presenter.SetFactionAsk("clean_water", 1);
00252:             string summary = presenter.BuildQuoteSummary();
00253:             Assert.Contains("DEAL IS FAIR", summary);
00254:             Assert.Contains("Canned Food", summary);
00255:             Assert.Contains("Clean Water", summary);
00256:             Assert.DoesNotContain("36.0", summary);
00257:
00258:             // Close collapses the open state.
00259:             presenter.Close(traded: true);
00260:             Assert.False(presenter.ViewModel.IsOpen);
00261:         }
00262:
00263:         [Fact]
00264:         public void Presenter_BioOffersPricedByCoreRule()
00265:         {
00266:             var presenter = new TradeScreenPresenter(
00267:                 CreateStanceEngine(), null, LoadTells(), new SeededRng(2026),
00268:                 unitPriceLookup: _ => 0f);
00269:
00270:             presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
00271:             presenter.SetBiologicalOffer(BiologicalTradeItem.Organ, 1);   // 4*25 = 100
00272:             presenter.SetFactionAsk("clean_water", 3);                    // 0
00273:
00274:             // Bio-only offer still counts toward the scale; demands worth nothing => fair.
00275:             Assert.Equal(TradeFairness.Fair, presenter.ViewModel.Fairness);
00276:             Assert.Equal(100f, presenter.ViewModel.PlayerOfferValue, 2);
00277:         }
00278:
00279:         [Fact]
00280:         public void Presenter_RoutesExecutionThroughSink()
00281:         {
00282:             var recorder = new RecordingExecutionSink();
00283:             var presenter = new TradeScreenPresenter(
00284:                 CreateStanceEngine(), null, LoadTells(), new SeededRng(2026),
00285:                 unitPriceLookup: _ => 10f,
00286:                 executionSink: recorder);
00287:
00288:             presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
00289:             presenter.SetPlayerOffer("canned_food", 2);
00290:             presenter.SetFactionAsk("clean_water", 1);
00291:             Assert.True(presenter.TryConfirmTrade());
00292:
00293:             Assert.Equal(1, recorder.ExecuteCalls);
00294:             Assert.Equal("scavenger_camp", recorder.LastFactionId);
00295:             Assert.Equal(2, recorder.LastPlayerOffers["canned_food"]);
00296:
00297:             presenter.TryDemandParley();
00298:             Assert.Equal(1, recorder.ParleyCalls);
00299:         }
00300:
00301:         private sealed class RecordingExecutionSink : ITradeExecutionSink
00302:         {
00303:             public int ExecuteCalls { get; private set; }
00304:             public int ParleyCalls { get; private set; }
00305:             public string LastFactionId { get; private set; }
00306:             public IReadOnlyDictionary<string, int> LastPlayerOffers { get; private set; }
00307:
00308:             public bool WillTrade(string factionId) => true;
00309:
00310:             public bool TryExecuteTrade(
00311:                 string factionId,
00312:                 IReadOnlyDictionary<string, int> playerOffers,
00313:                 IReadOnlyDictionary<string, int> factionAsks,
00314:                 IReadOnlyDictionary<BiologicalTradeItem, int> biologicalOffers)
00315:             {
00316:                 ExecuteCalls++;
00317:                 LastFactionId = factionId;
00318:                 LastPlayerOffers = playerOffers;
00319:                 return true;
00320:             }
00321:
00322:             public bool TryDemandParley(string factionId)
00323:             {
00324:                 ParleyCalls++;
00325:                 return true;
00326:             }
00327:         }
00328:     }
00329: }
```


# Appendix — Focused Evidence Detail: `Ashfall.Core.Tests/TradeTellCorpusTests.cs`

### `Ashfall.Core.Tests/TradeTellCorpusTests.cs` — complete current file

- Size: 178 lines / 6987 bytes.
- SHA-256: `c16c43086c8d61f19ff3d3be209b669262cddd2c359f206e62a91a544a8288e3`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Economy;
00007: using Xunit;
00008:
00009: namespace Ashfall.Core.Tests
00010: {
00011:     /// <summary>
00012:     /// Negotiation Table tell corpus: coverage counts, tone lint, band math,
00013:     /// and seed determinism. Mirrors the FactionRadioCorpusTests pattern.
00014:     /// </summary>
00015:     public class TradeTellCorpusTests
00016:     {
00017:         private static TradeTellEngine CreateLoadedEngine()
00018:         {
00019:             string corpusPath = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data/trade_tell_lines.json");
00020:             if (!File.Exists(corpusPath))
00021:             {
00022:                 corpusPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data/trade_tell_lines.json");
00023:             }
00024:
00025:             Assert.True(File.Exists(corpusPath), $"Tell corpus JSON not found at {corpusPath}");
00026:             return TradeTellEngine.LoadFromJson(File.ReadAllText(corpusPath));
00027:         }
00028:
00029:         private static string StanceKey(TradeStance stance)
00030:         {
00031:             switch (stance)
00032:             {
00033:                 case TradeStance.HostileRaid: return "hostile_raid";
00034:                 case TradeStance.Rob: return "rob";
00035:                 case TradeStance.Refuse: return "refuse";
00036:                 case TradeStance.ShareIntel: return "share_intel";
00037:                 default: return "trade";
00038:             }
00039:         }
00040:
00041:         [Fact]
00042:         public void Corpus_LoadsFourBandsAndTwentyPools()
00043:         {
00044:             var engine = CreateLoadedEngine();
00045:
00046:             Assert.Equal(4, engine.BandCount);
00047:             Assert.Equal(TradeTrustBands.Hostile, engine.Bands[0]);
00048:             Assert.Equal(TradeTrustBands.Wary, engine.Bands[1]);
00049:             Assert.Equal(TradeTrustBands.Neutral, engine.Bands[2]);
00050:             Assert.Equal(TradeTrustBands.Warm, engine.Bands[3]);
00051:
00052:             // 5 stances x 4 bands = 20 pools, >= 3 lines each (>= 60 total).
00053:             Assert.Equal(20, engine.PoolCount);
00054:             Assert.True(engine.LineCount >= 60, $"Expected >= 60 tell lines, found {engine.LineCount}");
00055:         }
00056:
00057:         [Fact]
00058:         public void Corpus_EveryStanceAndBandSelectsALegibleLine()
00059:         {
00060:             var engine = CreateLoadedEngine();
00061:             var rng = new SeededRng(2026);
00062:
00063:             foreach (TradeStance stance in Enum.GetValues(typeof(TradeStance)))
00064:             {
00065:                 foreach (float trust in new[] { -100f, -40f, -39f, 0f, 1f, 40f, 41f, 100f })
00066:                 {
00067:                     bool selected = engine.TrySelectTell(stance, trust, rng, out var tell);
00068:                     Assert.True(selected, $"No tell for stance={stance} trust={trust}");
00069:                     Assert.False(string.IsNullOrWhiteSpace(tell.Line));
00070:                     Assert.InRange(tell.Line.Length, 20, 140);
00071:                     Assert.Contains(StanceKey(stance), tell.Id);
00072:                     Assert.Contains(engine.BandForTrust(trust), tell.Id);
00073:                 }
00074:             }
00075:         }
00076:
00077:         [Fact]
00078:         public void Corpus_ToneLint_NoModernSlangOrAnachronisms()
00079:         {
00080:             var engine = CreateLoadedEngine();
00081:             var forbiddenWords = new[]
00082:             {
00083:                 " lol ", " gg ", " bruh ", " meta ", " player ", " respawn ", " nerf ", " buff ", " xp "
00084:             };
00085:
00086:             // Exhaustive: every line in every pool.
00087:             foreach (TradeStance stance in Enum.GetValues(typeof(TradeStance)))
00088:             {
00089:                 foreach (var band in engine.Bands)
00090:                 {
00091:                     Assert.True(engine.TryGetPoolLines(stance, band, out var lines));
00092:                     foreach (var line in lines)
00093:                     {
00094:                         string lower = " " + line.ToLowerInvariant() + " ";
00095:                         foreach (var forbidden in forbiddenWords)
00096:                         {
00097:                             Assert.DoesNotContain(forbidden, lower);
00098:                         }
00099:                     }
00100:                 }
00101:             }
00102:         }
00103:
00104:         [Fact]
00105:         public void Corpus_NoDuplicateLinesWithinPool()
00106:         {
00107:             var engine = CreateLoadedEngine();
00108:
00109:             foreach (TradeStance stance in Enum.GetValues(typeof(TradeStance)))
00110:             {
00111:                 foreach (var band in engine.Bands)
00112:                 {
00113:                     Assert.True(engine.TryGetPoolLines(stance, band, out var lines),
00114:                         $"Missing pool for stance={stance} band={band}");
00115:                     Assert.True(lines.Count >= 3, $"Pool stance={stance} band={band} has {lines.Count} lines, expected >= 3");
00116:
00117:                     var seen = new HashSet<string>(StringComparer.Ordinal);
00118:                     foreach (var line in lines)
00119:                     {
00120:                         Assert.DoesNotContain(line, seen);
00121:                         seen.Add(line);
00122:                     }
00123:                 }
00124:             }
00125:         }
00126:
00127:         [Fact]
00128:         public void Engine_BandBoundaries_MapTrustCorrectly()
00129:         {
00130:             var engine = CreateLoadedEngine();
00131:
00132:             Assert.Equal(TradeTrustBands.Hostile, engine.BandForTrust(-100f));
00133:             Assert.Equal(TradeTrustBands.Hostile, engine.BandForTrust(-40f));
00134:             Assert.Equal(TradeTrustBands.Wary, engine.BandForTrust(-39f));
00135:             Assert.Equal(TradeTrustBands.Wary, engine.BandForTrust(0f));
00136:             Assert.Equal(TradeTrustBands.Neutral, engine.BandForTrust(1f));
00137:             Assert.Equal(TradeTrustBands.Neutral, engine.BandForTrust(40f));
00138:             Assert.Equal(TradeTrustBands.Warm, engine.BandForTrust(41f));
00139:             Assert.Equal(TradeTrustBands.Warm, engine.BandForTrust(100f));
00140:         }
00141:
00142:         [Fact]
00143:         public void Engine_DeterministicRotation_SameSeedSameLine()
00144:         {
00145:             var engine1 = CreateLoadedEngine();
00146:             var engine2 = CreateLoadedEngine();
00147:
00148:             var rng1 = new SeededRng(9999);
00149:             var rng2 = new SeededRng(9999);
00150:
00151:             for (int i = 0; i < 20; i++)
00152:             {
00153:                 foreach (TradeStance stance in new[] { TradeStance.Trade, TradeStance.Refuse })
00154:                 {
00155:                     Assert.True(engine1.TrySelectTell(stance, 22f, rng1, out var t1));
00156:                     Assert.True(engine2.TrySelectTell(stance, 22f, rng2, out var t2));
00157:                     Assert.Equal(t1.Id, t2.Id);
00158:                     Assert.Equal(t1.Line, t2.Line);
00159:                 }
00160:             }
00161:         }
00162:
00163:         [Fact]
00164:         public void Engine_RotationVariesAcrossSeeds()
00165:         {
00166:             var engine = CreateLoadedEngine();
00167:
00168:             var lines = new HashSet<string>(StringComparer.Ordinal);
00169:             for (int seed = 1; seed <= 6; seed++)
00170:             {
00171:                 Assert.True(engine.TrySelectTell(TradeStance.Trade, 22f, new SeededRng(seed), out var tell));
00172:                 lines.Add(tell.Line);
00173:             }
00174:
00175:             Assert.True(lines.Count > 1, "Different seeds should rotate through the pool, not pin one line.");
00176:         }
00177:     }
00178: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **The subject is a deterministic behavioral-observation layer inside the existing trade presenter. The plan expands content integrity, replay, mechanical boundaries and player-facing truthfulness without turning flavor into hidden balance.**.

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
