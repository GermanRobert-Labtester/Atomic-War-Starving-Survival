# Plan 99 — Hardcore Economy Tuning, Scarcity and Price-Shock Contracts

> **Rebuild status:** COMPLETE 8/8/6 TUNING CATALOG — LOADER AND PROVIDER ARE CURRENT; BALANCE TUNING IS NOT ROW GROWTH
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

- The current catalog has 8 scarcity tiers across early/mid/late campaign bands, 8 faction preferences and 6 price shocks.
- HardcoreEconomyTuningLoader validates ranges/references; HardcoreEconomyTuning applies deterministic scarcity/faction/shock multipliers and caps stacked values.
- Main.Economy loads the bundle and the focused tests pin counts, boundaries, references and stacked multiplier behavior.

**Bounded outcome:** Retire the 2/1/1 target. hardcore_economy_tuning.json currently has 8 scarcity tiers, 8 faction preferences and 6 price-shock rules, and HardcoreEconomyTuningLoader/HardcoreEconomyTuning expose validated current behavior. The plan becomes a bounded balance, consumer and save-independence audit.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- Replace the old count target with a current 8/8/6 census and consumer matrix.
- Audit how scarcity tiers, faction preferences and shocks are applied to market/trade providers and whether the current day offset is the authoritative clock.
- No new price ledger, market state or tuning save section.

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
- The correct next step is balance evidence, not filling a stale row target.

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
| catalog validation and typed bundle | HardcoreEconomyTuningLoader | `Assets/Ashfall.Core/Economy/HardcoreEconomyTuningLoader.cs` | Static tuning owner. |
| scarcity/faction/shock lookup and multiplier math | HardcoreEconomyTuning | `Assets/Ashfall.Core/Economy/HardcoreEconomyTuning.cs` | Sole tuning provider. |
| actual item valuation and settlement | Market/trade providers | `Assets/Ashfall.Core/Economy/MarketSystem.cs; Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs` | Current economic owners. |
| host load and provider composition | Main.Economy | `src/Main.Economy.cs` | Thin host seam. |
| current count/range/consumer contract | HardcoreEconomyTuningExpansionTests | `Ashfall.Core.Tests/HardcoreEconomyTuningExpansionTests.cs` | Focused evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Hardcore Economy Tuning, Scarcity and Price-Shock Contracts
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ HardcoreEconomyTuningLoader
│   catalog validation and typed bundle
│ HardcoreEconomyTuning
│   scarcity/faction/shock lookup and multiplier math
│ Market/trade providers
│   actual item valuation and settlement
│ Main.Economy
│   host load and provider composition
│ HardcoreEconomyTuningExpansionTests
│   current count/range/consumer contract
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

1. **Preserve current state ownership.** HardcoreEconomyTuningLoader owns catalog validation and typed bundle: Static tuning owner.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| catalog validation and typed bundle | HardcoreEconomyTuningLoader | `Assets/Ashfall.Core/Economy/HardcoreEconomyTuningLoader.cs` | Static tuning owner. |
| scarcity/faction/shock lookup and multiplier math | HardcoreEconomyTuning | `Assets/Ashfall.Core/Economy/HardcoreEconomyTuning.cs` | Sole tuning provider. |
| actual item valuation and settlement | Market/trade providers | `Assets/Ashfall.Core/Economy/MarketSystem.cs; Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs` | Current economic owners. |
| host load and provider composition | Main.Economy | `src/Main.Economy.cs` | Thin host seam. |
| current count/range/consumer contract | HardcoreEconomyTuningExpansionTests | `Ashfall.Core.Tests/HardcoreEconomyTuningExpansionTests.cs` | Focused evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load and validate 8/8/6 bundle
2. resolve current scarcity tier by day/item
3. resolve faction preference
4. resolve active shock by kind/day offset
5. combine through current provider with cap
6. project market/trade values
7. leave inventory/settlement with current owners

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Tuning rows are immutable authored configuration.
- Actual prices, inventory and transactions remain with market/trade owners.
- A shock is a deterministic provider projection, not a persistent event ledger unless current state already records it.
- No Plan-99 save section.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A missing item/faction/shock produces a documented neutral or refusal, never a guessed wildcard.
- Stacked multiplier is bounded by current code.
- Tuning does not mutate market state.
- Day offset uses the existing campaign day provider.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- hardcore_economy_tuning.json is the tuning authority.
- Items, factions, market and trade catalogs own their data.
- No duplicate price or economy catalog.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- No tuning save section; tuning is configuration.
- Any future active-shock persistence must use the current market/event owner and migration proof.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Day and item selection are pure and deterministic.
- Provider composition is stable.
- No wall-clock or unseeded price randomness.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Current tuning provider returns facts to market/trade consumers.
- Market/trade emits transaction facts; tuning does not settle them.
- UI refresh reads current provider output.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Main.Economy.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Faction preference text is fictional and economic, not real-world political advice.
- Price shocks remain grounded in current world events.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A wildcard silently captures every item. | HardcoreEconomyTuningLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | Tuning mutates market price without settlement owner. | HardcoreEconomyTuning | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Stacking exceeds the cap. | Market/trade providers | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A day offset uses local time. | Main.Economy | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new price ledger duplicates market state. | HardcoreEconomyTuningExpansionTests | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/HardcoreEconomyTuningExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 | 8/8/6 census. | Current target is retired. | No production path until the owning implementation package is separately claimed. |
| 1 | Provider and clock trace. | Every multiplier has a real consumer and day source. | No production path until the owning implementation package is separately claimed. |
| 2 | Boundary/save/replay audit. | No price authority duplication. | No production path until the owning implementation package is separately claimed. |
| 3 | Balance/UI polish. | Current refusals and caps are visible. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/hardcore_economy_tuning.json | READ ONLY; MODIFY only for approved balance delta | 8/8/6 rows |
| Assets/Ashfall.Core/Economy/HardcoreEconomyTuning.cs | READ ONLY | Provider owner |
| src/Main.Economy.cs | READ ONLY | Host load seam |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Parallel price authority. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Wildcard guessing. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Tuning settlement side effects. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Wall-clock economy. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new tuning rows.
- No new price ledger.
- No production/data/test/UI changes here.

# 23. Rollback and Recovery

- Revert planning artifact.
- Future balance changes are config/provider deltas with current market tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 8/8/6 current data and provider boundaries are explicit.
- Balance work is separated from catalog growth.

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

- No new tuning rows.
- No new price ledger.
- No production/data/test/UI changes here.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/HardcoreEconomyTuningExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: catalog validation and typed bundle → HardcoreEconomyTuningLoader; scarcity/faction/shock lookup and multiplier math → HardcoreEconomyTuning; actual item valuation and settlement → Market/trade providers; host load and provider composition → Main.Economy; current count/range/consumer contract → HardcoreEconomyTuningExpansionTests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 99.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 99 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by HardcoreEconomyTuningLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Economy/HardcoreEconomyTuningLoader.cs`

### `Assets/Ashfall.Core/Economy/HardcoreEconomyTuningLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 147 lines / 7386 bytes.
- SHA-256: `5cd3d438f435dc960685c70a5d8def4e870054a68925e4cb26180cf0ab37840a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class HardcoreEconomyTuningLoader
public static HardcoreEconomyTuningLoadResult Load(string json) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Economy/HardcoreEconomyTuning.cs`

### `Assets/Ashfall.Core/Economy/HardcoreEconomyTuning.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 257 lines / 10884 bytes.
- SHA-256: `617b0e9e9ce1f5dcfddb6852a4e464181af204039b2aca9c523142f7c3ee7c8f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public readonly struct ScarcityEntry
public ScarcityTier Tier { get; }
public float Multiplier { get; }
public string DayRangeLabel { get; }
public IReadOnlyList<string> AffectedItemIds { get; }
public string Rationale { get; }
public readonly struct FactionTradePreference
public string FactionId { get; }
public IReadOnlyList<string> BuysAtPremium { get; }
public IReadOnlyList<string> Refuses { get; }
public string TradeCurrency { get; }
public readonly struct PriceShockRule
public PriceShockKind Kind { get; }
public float Multiplier { get; }
public int DurationDays { get; }
public IReadOnlyList<string> AffectedItemIds { get; }
public string Trigger { get; }
public sealed class HardcoreEconomyTuningBundle
public IReadOnlyList<ScarcityEntry> ScarcityTiers { get; private set; }
public IReadOnlyList<FactionTradePreference> FactionPreferences { get; private set; }
public IReadOnlyList<PriceShockRule> PriceShockRules { get; private set; }
public sealed class HardcoreEconomyTuning : IPriceShockProvider
public bool IsActive => _bundle.ScarcityTiers.Count > 0
public void Apply(HardcoreEconomyTuningBundle bundle) {
public float GetScarcityMultiplier(int currentDay, string itemId) {
public bool TryGetFactionPreference(string factionId, out FactionTradePreference preference) {
public bool TryGetPriceShock(PriceShockKind kind, int dayOffsetFromShockStart, out PriceShockRule rule) {
public sealed class HardcoreEconomyTuningLoadResult
public bool IsValid { get; private set; }
public List<string> Errors { get; private set; } = new();
public HardcoreEconomyTuningBundle? Bundle { get; private set; }
public static HardcoreEconomyTuningLoadResult Success(HardcoreEconomyTuningBundle bundle) => new() { IsValid = true, Bundle = bundle };
public static HardcoreEconomyTuningLoadResult Failure(IEnumerable<string> errors) => new() { IsValid = false, Errors = new List<string>(errors) };
```


# Appendix B.04 — Current Code Architecture: `src/Main.Economy.cs`

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


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/Economy/MarketSystem.cs`

### `Assets/Ashfall.Core/Economy/MarketSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1197 lines / 54572 bytes.
- SHA-256: `42b5c0022b34894ea0daacab232dddaf1ebb6cc17a017c5ef63c4b713cefd491`.
- Architecture signals: seeded references=5; save/restore symbols=5; typed event declarations=14; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class DemandEntry
public string itemId = string.Empty;
public float multiplier = 1f;
public class LedgerEntry
public int day = 0;
public string itemId = string.Empty;
public int quantity = 0;
public float unitPrice = 0f;
public float totalValue = 0f;
public string counterparty = string.Empty; // faction/person or "market"
public sealed class CategoryIndexEntry
public string categoryId = string.Empty;
public float multiplier = 1f;
public sealed class TradePressureEntry
public string categoryId = string.Empty;
public float buyUnits = 0f;
public float sellUnits = 0f;
public sealed class MarketShockState
public string shockId = string.Empty;
public string categoryId = string.Empty;
public bool isShortage = true;
public float severityBp = 2000f;              // 500..3000 bp (5%..30% effect) clamped on apply
public int startDay = 0;
public int expiryDay = 1;                // startDay + durationDays >= 1
public string sourceId = string.Empty;
public class MarketState
public const int Version = 3;
public string systemId = MarketSystem.SystemId;
public int version = Version;
public int day = 0;
public long tickCount = 0;
public List<DemandEntry> demand = new List<DemandEntry>();
public List<LedgerEntry> ledger = new List<LedgerEntry>();
public List<CategoryIndexEntry> categoryIndices = new List<CategoryIndexEntry>();
public List<TradePressureEntry> tradePressure = new List<TradePressureEntry>();
public List<MarketShockState> activeShocks = new List<MarketShockState>();
public TradeEmbargoState? tradeEmbargo;
public ResourceRationingState? rationing;
public struct TransactionResult
public bool Accepted;
public string ItemId;
public int Quantity;
public float UnitPrice;
public float TotalValue;
public float RemainderValue; // barter only: offered value not exchanged (whole-unit rule)
public string RejectReason;
public enum MarketTransactionSide
public enum PriceFactorKind
public sealed class PriceFactorRecord
public PriceFactorKind kind;
public string sourceId = string.Empty;
public float beforePrice;
public float afterPrice;
public float delta;
public float multiplier = 1f;
public bool isConstraint;
public sealed class PriceExplanation
public string itemId = string.Empty;
public MarketTransactionSide side;
public float basePrice;
public float demandMultiplier = 1f;
public float unclampedPrice;
public float finalPrice;
public List<PriceFactorRecord> factors = new List<PriceFactorRecord>();
public class MarketSystem
public const string SystemId = "economy_market_system";
public const int MarketStateVersion = MarketState.Version;
public const float MinDemandMult = 0.25f;
public const float MaxDemandMult = 4f;
public const float ShortageThreshold = 1.35f;
public const float PriceFloorFraction = 0.25f; // price >= base * floor
public const float PriceCeilingFraction = 4f;  // price <= base * ceiling
public const float PressureRetainedPerDay = 0.75f;
public const float PressureMaxUnits = 100000f;
public const float PressureComponentCapPermille = 350f;
public const float IndexSmoothingAlpha = 0.25f;
public const float FallbackIndexFloor = 0.2f;
public const float FallbackIndexCeiling = 5f;
public const float ShockProductFloor = 0.4f;
public const float ShockProductCeiling = 2.5f;
public const float ShockSeverityMinBp = 500f;
public const float ShockSeverityMaxBp = 3000f;
public event Action<string, float> OnDemandAdjusted;     // itemId, delta
public event Action OnEconomyChanged;                     // any price-relevant change
public event Action<MarketState> OnStateChanged;
public event Action<MarketShockState> OnShockStarted;
public event Action<MarketShockState> OnShockExpired;
public Func<float>? PriceMultiplierProvider { get; set; }
public MarketState State => _state;
public int Day => _state.day;
public long TickCount => _state.tickCount;
public void BindCatalog(GoodsCatalog catalog) {
public void LoadCatalog(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null) {
public GoodDefinition? FindGood(string itemId) =>
public void BindCommodityCatalog(CommodityBaselineCatalog catalog) {
public CommodityBaselineDefinition? FindCommodityBaseline(string categoryId) =>
public void BindRegionalPriceAtlas(RegionalPriceAtlas atlas, string marketRegion = "settlement") {
public void BindEmbargoSystem(TradeEmbargoSystem embargoSystem) {
public void TickDay(int day, ISeededRng rng) {
public void TickDays(int days, ISeededRng? rng = null) {
public float GetCategoryMultiplier(string categoryId) {
public float GetEffectiveCategoryMultiplierForItem(string itemId) {
public MarketShockState? ApplyShock( string categoryId, bool isShortage, float severityBp, int startDay, int durationDays, string sourceId) {
public IReadOnlyList<MarketShockState> ActiveShocks => _state.activeShocks;
public float GetDemandMultiplier(string itemId) {
public void AdjustDemand(string itemId, float delta) {
public bool IsSuppliesShort() {
public float GetPrice(string itemId) {
public float GetPrice(string itemId, string? region) {
public PriceExplanation ExplainPrice( string itemId, MarketTransactionSide side = MarketTransactionSide.Buy, string? region = null) {
public TransactionResult Buy(string itemId, int quantity, int day, string counterparty = "market", string? region = null) {
public TransactionResult Sell(string itemId, int quantity, int day, string counterparty = "market", string? region = null) {
public TransactionResult Barter(string giveItemId, int giveQuantity, string takeItemId, int day, string? region = null) {
public MarketState CaptureState() {
public void RestoreState(MarketState saved) {
public IReadOnlyList<TradePressureEntry> TradePressure => _state.tradePressure;
```


# Appendix B.06 — Current Code Architecture: `src/Main.Economy.cs`

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


# Appendix C.07 — Catalog Census: `Assets/StreamingAssets/Data/hardcore_economy_tuning.json`

### `Assets/StreamingAssets/Data/hardcore_economy_tuning.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 7588 bytes / 7588 characters.
- SHA-256: `c63a081ee6cc5d87fbf574275c8a45eaa7318715ff9024f6a8102743eeea0a74`.
- Root keys: `faction_preferences`, `price_shock_rules`, `scarcity_tiers`, `schema_version`, `version`.

Array-path census (minimum, maximum, observed rows):

```text
faction_preferences: min=8, max=8, observed_paths=1
faction_preferences[].buys_at_premium: min=4, max=10, observed_paths=2
faction_preferences[].refuses: min=3, max=4, observed_paths=2
price_shock_rules: min=6, max=6, observed_paths=1
price_shock_rules[].affected_item_ids: min=1, max=3, observed_paths=2
scarcity_tiers: min=8, max=8, observed_paths=1
scarcity_tiers[].affected_item_ids: min=4, max=4, observed_paths=2
```

Representative record fields:

- `affected_item_ids`
- `day_range_label`
- `multiplier`
- `rationale`
- `tier`


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/items.json`

### `Assets/StreamingAssets/Data/items.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 390056 bytes / 390056 characters.
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=724, max=724, observed_paths=1
```

Representative record fields:

- `category`
- `contamination`
- `degradeRate`
- `description`
- `disassembleYieldFraction`
- `displayName`
- `display_name`
- `durability`
- `empShielded`
- `equipSlot`
- `healthEffect`
- `hungerRestore`
- `id`
- `isEquipable`
- `moraleEffect`
- `radCleanse`
- `radProtection`
- `repairCosts`
- `repairRecipe`
- `scrapValue`
- `stackMax`
- `tags`
- `thirstRestore`
- `tradeValue`
- `type`
- `value`
- `weight`
- `weight_kg`

Representative identifiers (ordered, capped for readability):

```text
item_decon_chelator_concentrate
item_lead_lined_effluent_filter
item_heavy_neoprene_scrub_brush
item_sealed_waste_bin
item_theodolite_brass_precision
item_surveyor_stadia_rod
item_datum_plate_bronze
item_concrete_mix
item_forged_rotor_shaft
item_magnetic_bearing_coil
item_high_vacuum_pump
item_containment_ring_steel
item_reinforced_concrete_vault
item_seismic_damper_pad
item_vacuum_pump_oil
item_bearing_grease
item_rotor_balancing_kit
item_portable_pid_detector
item_detector_sensor_module
item_hermetic_sample_ampoule
item_hot_dust_drum
item_sludge_cake
item_tailings_drum
dosimeter
geiger_counter
iodine_pills
anti_rad
gas_mask
hazmat_suit
water_filter
air_filter
clean_water
irradiated_water
canned_food
fuel
cloth
scrap_metal
bandage
raw_meat
cooked_meat
dirty_water
morphine
chelation_agent
potassium_iodide
medical_kit
battery
calibration_kit
tweezers
splint
antibiotics
jewelry
diamond
currency
mechanical_parts
electronic_scrap
item_radiosonde
solar_cell
chemicals
handheld_radio
engine
roots
berries
vacuum_tube
spring_mechanism
phonograph_needle
projector_bulb
lubricant_oil
film_reel
antenna_coil
soldering_kit
music_box_comb
spring_key
typewriter_ribbon
machine_oil
camera_lens_cleaner
photographic_film
item_acoustic_decoy
item_ammonium_nitrate_sack
item_amnestic_syrup
item_anchor_notes
item_ash_ghillie
item_bio_plastic
item_black_water_vial
item_co2_scrubber_cartridge
item_epoxy_injector
item_faraday_mesh
item_frostbite_salve
item_fungicide_fogger
item_galvanized_rebar
item_glycol_antifreeze_canister
item_hermetic_hatch_silicone_gasket
item_high_tensile_steel_culvert_brace
item_insulated_snowmobile_battery
item_lead_shielded_sample_cask
item_lead_visor
item_lithium_salts
item_mine_prod
item_mycelium_bricks
item_prussian_blue_chelating_pellets
item_radon_detector_electret
item_rebreather_scrubber
item_ro_membrane
item_scopolamine_root
item_sealed_lead_pig
item_snow_goggles_improvised
item_sound_baffling
item_suitcase_locked
item_surgical_bone_chisel
item_teddy_bear
item_thermal_paste
item_welders_glass
aa_batteries
alcohol_wipes_box_10_of_10
ammo_762x54r_jhp_ap
ammo_357
ammo_12g
ammo_308
ammo_556
ammo_762
antiseptic_1l_of_1l
battery_pack
box_of_nails_10
canned_soup
childrens_books
cigarette_lighter
clean_water_jug
cooking_oil
copper_wire_10m_of_10m
diesel_fuel
dried_rations
faraday_pack
field_surgical_kit
fuel_1l
fuel_cell
growing_manual
iodine_tablets
item_cassette_tape
item_pre_war_photo_album
item_vinyl_collection
mechanical_components
medkit
metal_pipe
military_grade_hatchet
military_mre
military_radio
military_rations
military_supply_crate
music_box_fur_elise
night_vision_scope
plastic_material
scrap_plastic
synthetic_fuel_canister
carbon_black_powder
protective_childs_coat
rubber_hose
scrap_wood
sealed_government_document
seed_packets
spirits
steel_rebar
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/holdfast_factions.json`

### `Assets/StreamingAssets/Data/holdfast_factions.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 6557 bytes / 6557 characters.
- SHA-256: `e2f13c2291cba31c05b7b0dbca06dc37bd9bf5d692e51d9c2250c8738bdcbd44`.
- Root keys: `actions`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
actions: min=9, max=9, observed_paths=1
actions[].offers: min=3, max=3, observed_paths=2
actions[].wants: min=3, max=3, observed_paths=2
```

Representative record fields:

- `access_rule`
- `alignment`
- `badge_asset_id`
- `display_name`
- `home_region`
- `id`
- `is_active`
- `offers`
- `signature_quote`
- `trust`
- `wants`

Representative identifiers (ordered, capped for readability):

```text
faction_the_office
faction_the_cutters
faction_the_fleet
faction_black_flotilla
faction_supply_corps
faction_railway_guild
faction_hydro_barons
faction_ordnance_foundry
faction_scavengers
```


# Appendix D.10 — Existing Focused Test Inventory: `Ashfall.Core.Tests/HardcoreEconomyTuningExpansionTests.cs`

### `Ashfall.Core.Tests/HardcoreEconomyTuningExpansionTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 340; SHA-256: `f20357c27bdb9464a1b55618bce2d5689aa84dcc55e015c7ccca07cb8ff140f2`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AuthoritativeCatalog_LoadsSuccessfully
ScarcityTiers_ExactEightTiers_AndBaselinesPreserved
ScarcityTiers_FullCampaignDayCoverage_AcrossAllTiers
MatchesItem_WildcardPrefix_MatchesCorrectly
FactionPreferences_ExactEightUniqueFactions
FactionPreferences_NoCollisionBetweenPremiumAndRefuses
PriceShocks_ExactSixShocks_AndBaselinesPreserved
PriceShocks_QueryWithinAndBeyondDuration
Stacking_CombinedMultiplierRemainsBounded
NegativeFixture_InvalidTier_ReturnsFailure
NegativeFixture_DuplicateFaction_ReturnsFailure
Persistence_OldSaveSimulation_OperatesSafely
FactionPreferences_GarrisonLegacyKeyAndSystemsAliasResolveTogether
```


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs`

### `Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 219; SHA-256: `d51416b81e386bc4e81b3f415a5e968f25250c2d1904b92beddb5d9b6afd6829`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan92_FactionWarDialogue_LoadsAll40Snippets_WithValidStructureAndGating
Plan99_HardcoreEconomyTuning_LoadsAllTiersFactionPrefsAndPriceShocks
CrossSystem_WartimeDialogue_ReflectsEconomicScarcityAndPriceShocks
CrossSystem_DynamicEconomyAndDialogueGating_ArePureCoreAndDeterministic
```


# Appendix E.12 — Supporting Code Evidence: `Assets/Ashfall.Core/Economy/HardcoreEconomyTuningLoader.cs`

### `Assets/Ashfall.Core/Economy/HardcoreEconomyTuningLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 147 lines / 7386 bytes.
- SHA-256: `5cd3d438f435dc960685c70a5d8def4e870054a68925e4cb26180cf0ab37840a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class HardcoreEconomyTuningLoader
public static HardcoreEconomyTuningLoadResult Load(string json) {
```


# Appendix E.13 — Supporting Code Evidence: `Assets/Ashfall.Core/Economy/HardcoreEconomyTuning.cs`

### `Assets/Ashfall.Core/Economy/HardcoreEconomyTuning.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 257 lines / 10884 bytes.
- SHA-256: `617b0e9e9ce1f5dcfddb6852a4e464181af204039b2aca9c523142f7c3ee7c8f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public readonly struct ScarcityEntry
public ScarcityTier Tier { get; }
public float Multiplier { get; }
public string DayRangeLabel { get; }
public IReadOnlyList<string> AffectedItemIds { get; }
public string Rationale { get; }
public readonly struct FactionTradePreference
public string FactionId { get; }
public IReadOnlyList<string> BuysAtPremium { get; }
public IReadOnlyList<string> Refuses { get; }
public string TradeCurrency { get; }
public readonly struct PriceShockRule
public PriceShockKind Kind { get; }
public float Multiplier { get; }
public int DurationDays { get; }
public IReadOnlyList<string> AffectedItemIds { get; }
public string Trigger { get; }
public sealed class HardcoreEconomyTuningBundle
public IReadOnlyList<ScarcityEntry> ScarcityTiers { get; private set; }
public IReadOnlyList<FactionTradePreference> FactionPreferences { get; private set; }
public IReadOnlyList<PriceShockRule> PriceShockRules { get; private set; }
public sealed class HardcoreEconomyTuning : IPriceShockProvider
public bool IsActive => _bundle.ScarcityTiers.Count > 0
public void Apply(HardcoreEconomyTuningBundle bundle) {
public float GetScarcityMultiplier(int currentDay, string itemId) {
public bool TryGetFactionPreference(string factionId, out FactionTradePreference preference) {
public bool TryGetPriceShock(PriceShockKind kind, int dayOffsetFromShockStart, out PriceShockRule rule) {
public sealed class HardcoreEconomyTuningLoadResult
public bool IsValid { get; private set; }
public List<string> Errors { get; private set; } = new();
public HardcoreEconomyTuningBundle? Bundle { get; private set; }
public static HardcoreEconomyTuningLoadResult Success(HardcoreEconomyTuningBundle bundle) => new() { IsValid = true, Bundle = bundle };
public static HardcoreEconomyTuningLoadResult Failure(IEnumerable<string> errors) => new() { IsValid = false, Errors = new List<string>(errors) };
```


# Appendix E.14 — Supporting Code Evidence: `src/Main.Economy.cs`

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


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Economy/MarketSystem.cs`

### `Assets/Ashfall.Core/Economy/MarketSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1197 lines / 54572 bytes.
- SHA-256: `42b5c0022b34894ea0daacab232dddaf1ebb6cc17a017c5ef63c4b713cefd491`.
- Architecture signals: seeded references=5; save/restore symbols=5; typed event declarations=14; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class DemandEntry
public string itemId = string.Empty;
public float multiplier = 1f;
public class LedgerEntry
public int day = 0;
public string itemId = string.Empty;
public int quantity = 0;
public float unitPrice = 0f;
public float totalValue = 0f;
public string counterparty = string.Empty; // faction/person or "market"
public sealed class CategoryIndexEntry
public string categoryId = string.Empty;
public float multiplier = 1f;
public sealed class TradePressureEntry
public string categoryId = string.Empty;
public float buyUnits = 0f;
public float sellUnits = 0f;
public sealed class MarketShockState
public string shockId = string.Empty;
public string categoryId = string.Empty;
public bool isShortage = true;
public float severityBp = 2000f;              // 500..3000 bp (5%..30% effect) clamped on apply
public int startDay = 0;
public int expiryDay = 1;                // startDay + durationDays >= 1
public string sourceId = string.Empty;
public class MarketState
public const int Version = 3;
public string systemId = MarketSystem.SystemId;
public int version = Version;
public int day = 0;
public long tickCount = 0;
public List<DemandEntry> demand = new List<DemandEntry>();
public List<LedgerEntry> ledger = new List<LedgerEntry>();
public List<CategoryIndexEntry> categoryIndices = new List<CategoryIndexEntry>();
public List<TradePressureEntry> tradePressure = new List<TradePressureEntry>();
public List<MarketShockState> activeShocks = new List<MarketShockState>();
public TradeEmbargoState? tradeEmbargo;
public ResourceRationingState? rationing;
public struct TransactionResult
public bool Accepted;
public string ItemId;
public int Quantity;
public float UnitPrice;
public float TotalValue;
public float RemainderValue; // barter only: offered value not exchanged (whole-unit rule)
public string RejectReason;
public enum MarketTransactionSide
public enum PriceFactorKind
public sealed class PriceFactorRecord
public PriceFactorKind kind;
public string sourceId = string.Empty;
public float beforePrice;
public float afterPrice;
public float delta;
public float multiplier = 1f;
public bool isConstraint;
public sealed class PriceExplanation
public string itemId = string.Empty;
public MarketTransactionSide side;
public float basePrice;
public float demandMultiplier = 1f;
public float unclampedPrice;
public float finalPrice;
public List<PriceFactorRecord> factors = new List<PriceFactorRecord>();
public class MarketSystem
public const string SystemId = "economy_market_system";
public const int MarketStateVersion = MarketState.Version;
public const float MinDemandMult = 0.25f;
public const float MaxDemandMult = 4f;
public const float ShortageThreshold = 1.35f;
public const float PriceFloorFraction = 0.25f; // price >= base * floor
public const float PriceCeilingFraction = 4f;  // price <= base * ceiling
public const float PressureRetainedPerDay = 0.75f;
public const float PressureMaxUnits = 100000f;
public const float PressureComponentCapPermille = 350f;
public const float IndexSmoothingAlpha = 0.25f;
public const float FallbackIndexFloor = 0.2f;
public const float FallbackIndexCeiling = 5f;
public const float ShockProductFloor = 0.4f;
public const float ShockProductCeiling = 2.5f;
public const float ShockSeverityMinBp = 500f;
public const float ShockSeverityMaxBp = 3000f;
public event Action<string, float> OnDemandAdjusted;     // itemId, delta
public event Action OnEconomyChanged;                     // any price-relevant change
public event Action<MarketState> OnStateChanged;
public event Action<MarketShockState> OnShockStarted;
public event Action<MarketShockState> OnShockExpired;
public Func<float>? PriceMultiplierProvider { get; set; }
public MarketState State => _state;
public int Day => _state.day;
public long TickCount => _state.tickCount;
public void BindCatalog(GoodsCatalog catalog) {
public void LoadCatalog(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null) {
public GoodDefinition? FindGood(string itemId) =>
public void BindCommodityCatalog(CommodityBaselineCatalog catalog) {
public CommodityBaselineDefinition? FindCommodityBaseline(string categoryId) =>
public void BindRegionalPriceAtlas(RegionalPriceAtlas atlas, string marketRegion = "settlement") {
public void BindEmbargoSystem(TradeEmbargoSystem embargoSystem) {
public void TickDay(int day, ISeededRng rng) {
public void TickDays(int days, ISeededRng? rng = null) {
public float GetCategoryMultiplier(string categoryId) {
public float GetEffectiveCategoryMultiplierForItem(string itemId) {
public MarketShockState? ApplyShock( string categoryId, bool isShortage, float severityBp, int startDay, int durationDays, string sourceId) {
public IReadOnlyList<MarketShockState> ActiveShocks => _state.activeShocks;
public float GetDemandMultiplier(string itemId) {
public void AdjustDemand(string itemId, float delta) {
public bool IsSuppliesShort() {
public float GetPrice(string itemId) {
public float GetPrice(string itemId, string? region) {
public PriceExplanation ExplainPrice( string itemId, MarketTransactionSide side = MarketTransactionSide.Buy, string? region = null) {
public TransactionResult Buy(string itemId, int quantity, int day, string counterparty = "market", string? region = null) {
public TransactionResult Sell(string itemId, int quantity, int day, string counterparty = "market", string? region = null) {
public TransactionResult Barter(string giveItemId, int giveQuantity, string takeItemId, int day, string? region = null) {
public MarketState CaptureState() {
public void RestoreState(MarketState saved) {
public IReadOnlyList<TradePressureEntry> TradePressure => _state.tradePressure;
```


# Appendix E.16 — Supporting Code Evidence: `src/Main.Economy.cs`

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


# Appendix F.17 — Supporting Data Evidence: `Assets/StreamingAssets/Data/hardcore_economy_tuning.json`

### `Assets/StreamingAssets/Data/hardcore_economy_tuning.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 7588 bytes / 7588 characters.
- SHA-256: `c63a081ee6cc5d87fbf574275c8a45eaa7318715ff9024f6a8102743eeea0a74`.
- Root keys: `faction_preferences`, `price_shock_rules`, `scarcity_tiers`, `schema_version`, `version`.

Array-path census (minimum, maximum, observed rows):

```text
faction_preferences: min=8, max=8, observed_paths=1
faction_preferences[].buys_at_premium: min=4, max=10, observed_paths=2
faction_preferences[].refuses: min=3, max=4, observed_paths=2
price_shock_rules: min=6, max=6, observed_paths=1
price_shock_rules[].affected_item_ids: min=1, max=3, observed_paths=2
scarcity_tiers: min=8, max=8, observed_paths=1
scarcity_tiers[].affected_item_ids: min=4, max=4, observed_paths=2
```

Representative record fields:

- `affected_item_ids`
- `day_range_label`
- `multiplier`
- `rationale`
- `tier`


# Appendix F.18 — Supporting Data Evidence: `Assets/StreamingAssets/Data/items.json`

### `Assets/StreamingAssets/Data/items.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 390056 bytes / 390056 characters.
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=724, max=724, observed_paths=1
```

Representative record fields:

- `category`
- `contamination`
- `degradeRate`
- `description`
- `disassembleYieldFraction`
- `displayName`
- `display_name`
- `durability`
- `empShielded`
- `equipSlot`
- `healthEffect`
- `hungerRestore`
- `id`
- `isEquipable`
- `moraleEffect`
- `radCleanse`
- `radProtection`
- `repairCosts`
- `repairRecipe`
- `scrapValue`
- `stackMax`
- `tags`
- `thirstRestore`
- `tradeValue`
- `type`
- `value`
- `weight`
- `weight_kg`

Representative identifiers (ordered, capped for readability):

```text
item_decon_chelator_concentrate
item_lead_lined_effluent_filter
item_heavy_neoprene_scrub_brush
item_sealed_waste_bin
item_theodolite_brass_precision
item_surveyor_stadia_rod
item_datum_plate_bronze
item_concrete_mix
item_forged_rotor_shaft
item_magnetic_bearing_coil
item_high_vacuum_pump
item_containment_ring_steel
item_reinforced_concrete_vault
item_seismic_damper_pad
item_vacuum_pump_oil
item_bearing_grease
item_rotor_balancing_kit
item_portable_pid_detector
item_detector_sensor_module
item_hermetic_sample_ampoule
item_hot_dust_drum
item_sludge_cake
item_tailings_drum
dosimeter
geiger_counter
iodine_pills
anti_rad
gas_mask
hazmat_suit
water_filter
air_filter
clean_water
irradiated_water
canned_food
fuel
cloth
scrap_metal
bandage
raw_meat
cooked_meat
dirty_water
morphine
chelation_agent
potassium_iodide
medical_kit
battery
calibration_kit
tweezers
splint
antibiotics
jewelry
diamond
currency
mechanical_parts
electronic_scrap
item_radiosonde
solar_cell
chemicals
handheld_radio
engine
roots
berries
vacuum_tube
spring_mechanism
phonograph_needle
projector_bulb
lubricant_oil
film_reel
antenna_coil
soldering_kit
music_box_comb
spring_key
typewriter_ribbon
machine_oil
camera_lens_cleaner
photographic_film
item_acoustic_decoy
item_ammonium_nitrate_sack
item_amnestic_syrup
item_anchor_notes
item_ash_ghillie
item_bio_plastic
item_black_water_vial
item_co2_scrubber_cartridge
item_epoxy_injector
item_faraday_mesh
item_frostbite_salve
item_fungicide_fogger
item_galvanized_rebar
item_glycol_antifreeze_canister
item_hermetic_hatch_silicone_gasket
item_high_tensile_steel_culvert_brace
item_insulated_snowmobile_battery
item_lead_shielded_sample_cask
item_lead_visor
item_lithium_salts
item_mine_prod
item_mycelium_bricks
item_prussian_blue_chelating_pellets
item_radon_detector_electret
item_rebreather_scrubber
item_ro_membrane
item_scopolamine_root
item_sealed_lead_pig
item_snow_goggles_improvised
item_sound_baffling
item_suitcase_locked
item_surgical_bone_chisel
item_teddy_bear
item_thermal_paste
item_welders_glass
aa_batteries
alcohol_wipes_box_10_of_10
ammo_762x54r_jhp_ap
ammo_357
ammo_12g
ammo_308
ammo_556
ammo_762
antiseptic_1l_of_1l
battery_pack
box_of_nails_10
canned_soup
childrens_books
cigarette_lighter
clean_water_jug
cooking_oil
copper_wire_10m_of_10m
diesel_fuel
dried_rations
faraday_pack
field_surgical_kit
fuel_1l
fuel_cell
growing_manual
iodine_tablets
item_cassette_tape
item_pre_war_photo_album
item_vinyl_collection
mechanical_components
medkit
metal_pipe
military_grade_hatchet
military_mre
military_radio
military_rations
military_supply_crate
music_box_fur_elise
night_vision_scope
plastic_material
scrap_plastic
synthetic_fuel_canister
carbon_black_powder
protective_childs_coat
rubber_hose
scrap_wood
sealed_government_document
seed_packets
spirits
steel_rebar
```


# Appendix G.19 — Supporting Regression Evidence: `Ashfall.Core.Tests/HardcoreEconomyTuningExpansionTests.cs`

### `Ashfall.Core.Tests/HardcoreEconomyTuningExpansionTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 340; SHA-256: `f20357c27bdb9464a1b55618bce2d5689aa84dcc55e015c7ccca07cb8ff140f2`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AuthoritativeCatalog_LoadsSuccessfully
ScarcityTiers_ExactEightTiers_AndBaselinesPreserved
ScarcityTiers_FullCampaignDayCoverage_AcrossAllTiers
MatchesItem_WildcardPrefix_MatchesCorrectly
FactionPreferences_ExactEightUniqueFactions
FactionPreferences_NoCollisionBetweenPremiumAndRefuses
PriceShocks_ExactSixShocks_AndBaselinesPreserved
PriceShocks_QueryWithinAndBeyondDuration
Stacking_CombinedMultiplierRemainsBounded
NegativeFixture_InvalidTier_ReturnsFailure
NegativeFixture_DuplicateFaction_ReturnsFailure
Persistence_OldSaveSimulation_OperatesSafely
FactionPreferences_GarrisonLegacyKeyAndSystemsAliasResolveTogether
```


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs`

### `Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 219; SHA-256: `d51416b81e386bc4e81b3f415a5e968f25250c2d1904b92beddb5d9b6afd6829`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan92_FactionWarDialogue_LoadsAll40Snippets_WithValidStructureAndGating
Plan99_HardcoreEconomyTuning_LoadsAllTiersFactionPrefsAndPriceShocks
CrossSystem_WartimeDialogue_ReflectsEconomicScarcityAndPriceShocks
CrossSystem_DynamicEconomyAndDialogueGating_ArePureCoreAndDeterministic
```


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| catalog validation and typed bundle | HardcoreEconomyTuningLoader | scarcity/faction/shock lookup and multiplier math | HardcoreEconomyTuning | Owner emits/reads a typed fact; no mirror state. |
| catalog validation and typed bundle | HardcoreEconomyTuningLoader | actual item valuation and settlement | Market/trade providers | Owner emits/reads a typed fact; no mirror state. |
| catalog validation and typed bundle | HardcoreEconomyTuningLoader | host load and provider composition | Main.Economy | Owner emits/reads a typed fact; no mirror state. |
| catalog validation and typed bundle | HardcoreEconomyTuningLoader | current count/range/consumer contract | HardcoreEconomyTuningExpansionTests | Owner emits/reads a typed fact; no mirror state. |
| scarcity/faction/shock lookup and multiplier math | HardcoreEconomyTuning | catalog validation and typed bundle | HardcoreEconomyTuningLoader | Owner emits/reads a typed fact; no mirror state. |
| scarcity/faction/shock lookup and multiplier math | HardcoreEconomyTuning | actual item valuation and settlement | Market/trade providers | Owner emits/reads a typed fact; no mirror state. |
| scarcity/faction/shock lookup and multiplier math | HardcoreEconomyTuning | host load and provider composition | Main.Economy | Owner emits/reads a typed fact; no mirror state. |
| scarcity/faction/shock lookup and multiplier math | HardcoreEconomyTuning | current count/range/consumer contract | HardcoreEconomyTuningExpansionTests | Owner emits/reads a typed fact; no mirror state. |
| actual item valuation and settlement | Market/trade providers | catalog validation and typed bundle | HardcoreEconomyTuningLoader | Owner emits/reads a typed fact; no mirror state. |
| actual item valuation and settlement | Market/trade providers | scarcity/faction/shock lookup and multiplier math | HardcoreEconomyTuning | Owner emits/reads a typed fact; no mirror state. |
| actual item valuation and settlement | Market/trade providers | host load and provider composition | Main.Economy | Owner emits/reads a typed fact; no mirror state. |
| actual item valuation and settlement | Market/trade providers | current count/range/consumer contract | HardcoreEconomyTuningExpansionTests | Owner emits/reads a typed fact; no mirror state. |
| host load and provider composition | Main.Economy | catalog validation and typed bundle | HardcoreEconomyTuningLoader | Owner emits/reads a typed fact; no mirror state. |
| host load and provider composition | Main.Economy | scarcity/faction/shock lookup and multiplier math | HardcoreEconomyTuning | Owner emits/reads a typed fact; no mirror state. |
| host load and provider composition | Main.Economy | actual item valuation and settlement | Market/trade providers | Owner emits/reads a typed fact; no mirror state. |
| host load and provider composition | Main.Economy | current count/range/consumer contract | HardcoreEconomyTuningExpansionTests | Owner emits/reads a typed fact; no mirror state. |
| current count/range/consumer contract | HardcoreEconomyTuningExpansionTests | catalog validation and typed bundle | HardcoreEconomyTuningLoader | Owner emits/reads a typed fact; no mirror state. |
| current count/range/consumer contract | HardcoreEconomyTuningExpansionTests | scarcity/faction/shock lookup and multiplier math | HardcoreEconomyTuning | Owner emits/reads a typed fact; no mirror state. |
| current count/range/consumer contract | HardcoreEconomyTuningExpansionTests | actual item valuation and settlement | Market/trade providers | Owner emits/reads a typed fact; no mirror state. |
| current count/range/consumer contract | HardcoreEconomyTuningExpansionTests | host load and provider composition | Main.Economy | Owner emits/reads a typed fact; no mirror state. |

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

> | Cluster | Opening archetype | Confidence |
|---|---|---|
| C4 | Fuel and feedstock income-versus-expenditure audits for each industrial chain; dominated-process analysis (do any catalogs produce strictly dominated outputs?) | HIGH CONFIDENCE |
| C5 | Scavenging E[value] re-runs after any loot authoring (Plan 76.2 harness pattern); vehicle dominance follow-ups against the live dominance table | HIGH CONFIDENCE |
| C7 | Tribute-cycle sustainability (7-day cadence) versus mid-game income; embargo economic pressure | HIGH CONFIDENCE |
| C11 | Price-shock and rumor-band systemic outcomes; debt-interest runaway analysis; black-market pricing tiers | HIGH CONFIDENCE |
| C12 | Winter resource compression (Days 90–180): calories, fuel, filters, morale — sustainability-day math per difficulty preset | HIGH CONFIDENCE |
| C14 | Trapping yield versus equipment degradation cost; zoonosis risk premium on uncooked yield | HIGH CONFIDENCE |
| All others | Balance audits only where numbers exist; never invent tuning targets without an intended design statement | — |

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

Current evidence and safe integration boundary for Plan 99: Hardcore Economy Tuning, Scarcity and Price-Shock Contracts.

- **catalog validation and typed bundle** remains with `HardcoreEconomyTuningLoader` at `Assets/Ashfall.Core/Economy/HardcoreEconomyTuningLoader.cs`. Static tuning owner.
- **scarcity/faction/shock lookup and multiplier math** remains with `HardcoreEconomyTuning` at `Assets/Ashfall.Core/Economy/HardcoreEconomyTuning.cs`. Sole tuning provider.
- **actual item valuation and settlement** remains with `Market/trade providers` at `Assets/Ashfall.Core/Economy/MarketSystem.cs; Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs`. Current economic owners.
- **host load and provider composition** remains with `Main.Economy` at `src/Main.Economy.cs`. Thin host seam.
- **current count/range/consumer contract** remains with `HardcoreEconomyTuningExpansionTests` at `Ashfall.Core.Tests/HardcoreEconomyTuningExpansionTests.cs`. Focused evidence.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load and validate 8/8/6 bundle
2. resolve current scarcity tier by day/item
3. resolve faction preference
4. resolve active shock by kind/day offset
5. combine through current provider with cap
6. project market/trade values
7. leave inventory/settlement with current owners

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Tuning rows are immutable authored configuration.
- Actual prices, inventory and transactions remain with market/trade owners.
- A shock is a deterministic provider projection, not a persistent event ledger unless current state already records it.
- No Plan-99 save section.

- A missing item/faction/shock produces a documented neutral or refusal, never a guessed wildcard.
- Stacked multiplier is bounded by current code.
- Tuning does not mutate market state.
- Day offset uses the existing campaign day provider.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Main.Economy.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/HardcoreEconomyTuningExpansionTests.cs
- Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs

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
| S-01 | 99-01 8 tiers load | load and validate 8/8/6 bundle | Tuning rows are immutable authored configuration. | A wildcard silently captures every item. | HardcoreEconomyTuningLoader |
| S-02 | 99-02 8 factions load | resolve current scarcity tier by day/item | Actual prices, inventory and transactions remain with market/trade owners. | Tuning mutates market price without settlement owner. | HardcoreEconomyTuningLoader |
| S-03 | 99-03 6 shocks load | resolve faction preference | A shock is a deterministic provider projection, not a persistent event ledger unless current state already records it. | Stacking exceeds the cap. | HardcoreEconomyTuningLoader |
| S-04 | 99-04 day boundary | resolve active shock by kind/day offset | No Plan-99 save section. | A day offset uses local time. | HardcoreEconomyTuningLoader |
| S-05 | 99-05 faction preference | combine through current provider with cap | Tuning rows are immutable authored configuration. | A new price ledger duplicates market state. | HardcoreEconomyTuningLoader |
| S-06 | 99-06 shock window | project market/trade values | Actual prices, inventory and transactions remain with market/trade owners. | A wildcard silently captures every item. | HardcoreEconomyTuningLoader |
| S-07 | 99-07 stacked cap | leave inventory/settlement with current owners | A shock is a deterministic provider projection, not a persistent event ledger unless current state already records it. | Tuning mutates market price without settlement owner. | HardcoreEconomyTuningLoader |
| S-08 | 99-08 consumer projection | load and validate 8/8/6 bundle | No Plan-99 save section. | Stacking exceeds the cap. | HardcoreEconomyTuningLoader |
| S-09 | 99-09 replay | resolve current scarcity tier by day/item | Tuning rows are immutable authored configuration. | A day offset uses local time. | HardcoreEconomyTuningLoader |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 99-TC-01 schema/count | data | schema/count; verify the named current owner and its negative boundary without inventing a second authority. | HardcoreEconomyTuningLoader |
| T-02 | 99-TC-02 range/reference validation | unit | range/reference validation; verify the named current owner and its negative boundary without inventing a second authority. | HardcoreEconomyTuningLoader |
| T-03 | 99-TC-03 day boundary | persistence | day boundary; verify the named current owner and its negative boundary without inventing a second authority. | HardcoreEconomyTuningLoader |
| T-04 | 99-TC-04 faction exactness | determinism | faction exactness; verify the named current owner and its negative boundary without inventing a second authority. | HardcoreEconomyTuningLoader |
| T-05 | 99-TC-05 shock lifetime | host | shock lifetime; verify the named current owner and its negative boundary without inventing a second authority. | HardcoreEconomyTuningLoader |
| T-06 | 99-TC-06 stack cap | UI/accessibility | stack cap; verify the named current owner and its negative boundary without inventing a second authority. | HardcoreEconomyTuningLoader |
| T-07 | 99-TC-07 market handoff | cross-system | market handoff; verify the named current owner and its negative boundary without inventing a second authority. | HardcoreEconomyTuningLoader |
| T-08 | 99-TC-08 no tuning save | data | no tuning save; verify the named current owner and its negative boundary without inventing a second authority. | HardcoreEconomyTuningLoader |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 26 | `Ashfall.Core.Tests/EconomySystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 20 | `Ashfall.Core.Tests/MarketAdapterProbeTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 19 | `Ashfall.Core.Tests/EconomyProbeTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 16 | `Ashfall.Core.Tests/Economy/EconomyHostSessionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 15 | `src/Host/HostCli.PanelTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 14 | `Ashfall.Core.Tests/HardcoreEconomyTuningExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 13 | `Assets/Ashfall.Core/Economy/HardcoreEconomyTuningLoader.cs` | current reference count; inspect the caller before treating it as a live route |
| 12 | `Ashfall.Core.Tests/Plan56Phase5Tests.cs` | current reference count; inspect the caller before treating it as a live route |
| 12 | `Assets/Ashfall.Core/Economy/HardcoreEconomyTuning.cs` | current reference count; inspect the caller before treating it as a live route |
| 11 | `Ashfall.Core.Tests/Plan56EconomyGoodsTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Ashfall.Core.Tests/FlagshipEconomyScenarioTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Ashfall.Core.Tests/MarketPriceDeterminismTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `Ashfall.Core.Tests/Economy/Plan211BlackMarketTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/DynamicEconomyCharacterizationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/Economy/Plan212DynamicEconomyTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `src/Host/HostCli.WorldPlaytest.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `src/Host/WeatherCascadeHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/Economy/Plan212EconomyHostWiringTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `src/Main.Economy.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/Economy/Plan155BlackMarketIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/SilentFoundryConsequenceTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Economy/BlackMarketSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `src/Foundry/SilentFoundryHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `src/Host/BlackMarketHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Plan56CloseOutTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Plan56FollowUpTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Shelter/Plan210_212CrossPlanIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/BlackMarketSelfTest.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Economy/Plan14AEconomyIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Economy/Plan211BlackMarketSettlementTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Plan23FlotillaFactionDepthTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/TradeThemeAndEconomyTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Economy/BlackMarketSettlementService.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/hardcore_economy_tuning.json`

### `Assets/StreamingAssets/Data/hardcore_economy_tuning.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 7588; characters: 7588.
- SHA-256: `c63a081ee6cc5d87fbf574275c8a45eaa7318715ff9024f6a8102743eeea0a74`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `version`, `scarcity_tiers`, `faction_preferences`, `price_shock_rules`

#### `scarcity_tiers` — 8 current rows

- Row 001 `row-1`: `{"affected_item_ids":["clean_water","iodine_pills","anti_rad","air_filter"],"day_range_label":"Days 1-15","multiplier":2.5,"rationale":"Immediate survival. Everyone needs them. Nobody has enough.","tier":"Critical"}`
- Row 002 `row-2`: `{"affected_item_ids":["antibiotics","medical_kit","fuel","water_filter"],"day_range_label":"Days 15-40","multiplier":2.0,"rationale":"Infections set in. Filters clog. Fuel runs low.","tier":"High"}`
- Row 003 `row-3`: `{"affected_item_ids":["scrap_mechanical","calibration_kit","antibiotics","water_filter"],"day_range_label":"Days 41-100","multiplier":1.6,"rationale":"The first medical panic passes; repairs and calibration become the daily tax.","tier":"M…`
- Row 004 `row-4`: `{"affected_item_ids":["seed_packets","item_seed_ash_grain","scrap_mechanical","engine"],"day_range_label":"Days 101-160","multiplier":1.3,"rationale":"Calories settle while seed stock and working prime movers carry the next season.","tier"…`
- Row 005 `row-5`: `{"affected_item_ids":["engine","item_foundry_roof_armor_plate","scrap_mechanical","seed_packets"],"day_range_label":"Days 161-220","multiplier":1.5,"rationale":"Shelters rebuild their roofs and workshops, pulling heavy parts back into scar…`
- Row 006 `row-6`: `{"affected_item_ids":["fuel","ammo_*","medical_kit","canned_food"],"day_range_label":"Days 221-280","multiplier":1.8,"rationale":"The old caches are gone and every route guards fuel, ammunition, and preserved food.","tier":"LateScarcity"}`
- Row 007 `row-7`: `{"affected_item_ids":["fuel","canned_food","clean_water","medical_kit"],"day_range_label":"Days 281-340","multiplier":2.2,"rationale":"Frozen routes and wells make heat, food, water, and treatment hard to replace.","tier":"DeepWinter"}`
- Row 008 `row-8`: `{"affected_item_ids":["item_seed_ash_grain","engine","medical_kit","dosimeter"],"day_range_label":"Days 341+","multiplier":2.4,"rationale":"Long survival makes irreplaceable germplasm, machinery, treatment, and measurement priceless.","tie…`

#### `faction_preferences` — 8 current rows

- Row 001 `central_garrison_remnants`: `{"buys_at_premium":["ammo_*","body_armour_military","fuel","mre_military"],"faction_id":"central_garrison_remnants","refuses":["jewelry","book","cigarette"],"trade_currency":"Fuel, ammunition, obedience"}`
- Row 002 `faction_black_flotilla`: `{"buys_at_premium":["item_marine_sealant_kit","item_descent_line","item_sealed_dive_lamp","item_rebreather_canister","brass_fittings","scrap_mechanical","item_process_barrel","item_ro_resin","chart_*","paper_scrap"],"faction_id":"faction_b…`
- Row 003 `faction_the_scale`: `{"buys_at_premium":["water_filter","clean_water","item_foundry_brine_pipe","brass_fittings","scrap_mechanical"],"faction_id":"faction_the_scale","refuses":["jewelry","cigarette_pack_sealed","family_photograph"],"trade_currency":"Volumetric…`
- Row 004 `faction_the_compact`: `{"buys_at_premium":["paper_scrap","book","dosimeter","geiger_counter"],"faction_id":"faction_the_compact","refuses":["ammo_*","item_hot_dust_drum","item_tailings_drum"],"trade_currency":"Archival deeds, survey boundary chits, and legal arb…`
- Row 005 `faction_the_underwrite`: `{"buys_at_premium":["fuel","ammo_*","medical_kit","calibration_kit"],"faction_id":"faction_the_underwrite","refuses":["item_sludge_cake","item_hot_dust_drum","item_tailings_drum"],"trade_currency":"Fuel vouchers, convoy insurance underwrit…`
- Row 006 `faction_the_cutters`: `{"buys_at_premium":["fuel","engine","scrap_mechanical","canned_food"],"faction_id":"faction_the_cutters","refuses":["jewelry","paper_scrap","book"],"trade_currency":"Black coal, haulage sledges, and cleared pass transit chits"}`
- Row 007 `faction_the_rebuilders`: `{"buys_at_premium":["seed_packets","item_seed_ash_grain","clean_water","antibiotics"],"faction_id":"faction_the_rebuilders","refuses":["item_hot_dust_drum","item_tailings_drum","item_sludge_cake"],"trade_currency":"Grain bushels, heirloom …`
- Row 008 `faction_the_overlay`: `{"buys_at_premium":["brass_fittings","dosimeter","geiger_counter","paper_scrap"],"faction_id":"faction_the_overlay","refuses":["jewelry","cigarette_pack_sealed","item_sludge_cake"],"trade_currency":"Cadastral keys, triangulation data, and …`

#### `price_shock_rules` — 6 current rows

- Row 001 `row-1`: `{"affected_item_ids":["*"],"duration_days":3,"kind":"PlumePassing","multiplier":1.8,"trigger":"fallout storm crosses a trade route"}`
- Row 002 `row-2`: `{"affected_item_ids":["fuel","canned_food","medical_kit"],"duration_days":3,"kind":"ConvoyAmbush","multiplier":1.6,"trigger":"a major supply convoy is intercepted and destroyed on the route"}`
- Row 003 `row-3`: `{"affected_item_ids":["ammo_*","medical_kit","fuel"],"duration_days":5,"kind":"FactionConflict","multiplier":1.7,"trigger":"an armed border conflict closes the safest faction crossings"}`
- Row 004 `row-4`: `{"affected_item_ids":["canned_food","clean_water","seed_packets"],"duration_days":7,"kind":"SeasonalScarcity","multiplier":1.5,"trigger":"a sudden blizzard disrupts foraging and greenhouse transport"}`
- Row 005 `row-5`: `{"affected_item_ids":["antibiotics","medical_kit","clean_water"],"duration_days":4,"kind":"DiseaseOutbreak","multiplier":2.0,"trigger":"a waterborne outbreak spreads through crowded shelter housing"}`
- Row 006 `row-6`: `{"affected_item_ids":["fuel","engine","scrap_mechanical"],"duration_days":3,"kind":"FuelShortage","multiplier":1.9,"trigger":"a regional pumping failure stalls fuel distribution"}`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/items.json`

### `Assets/StreamingAssets/Data/items.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 390056; characters: 390056.
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `items`

#### `items` — 724 current rows

- Row 001 `item_decon_chelator_concentrate`: `{"description":"A sealed glass ampoule of chelating agent concentrate. The label is half dissolved but the formula is standard: binds heavy radionuclides into a water-soluble complex for rinse removal. One ampoule per decon cycle. The pre-…`
- Row 002 `item_lead_lined_effluent_filter`: `{"description":"A cylindrical filtration cartridge with a lead-foil inner liner and activated charcoal matrix. Installed in the decon airlock effluent tank to capture radionuclide-laden particulates before they can be sluiced into the gene…`
- Row 003 `item_heavy_neoprene_scrub_brush`: `{"description":"A stiff-bristled scrub brush with a neoprene grip and an integrated scraper edge. Designed for the coarse physical removal of radioactive particulates from canvas, leather, and skin before the chemical wash stage. The brist…`
- Row 004 `item_sealed_waste_bin`: `{"description":"A galvanized steel bin with a rubber gasket seal and a clamp-down lid. Once sealed, the contents are considered permanently isolated: the bin is stacked in the waste gallery and added to the long-term burial manifest. No on…`
- Row 005 `item_theodolite_brass_precision`: `{"description":"A pre-war surveying theodolite with a brass body, etched vernier scales, and a spirit level that still holds true. The optics are fogged at the edges but the crosshairs are sharp. Measures horizontal and vertical angles to …`
- Row 006 `item_surveyor_stadia_rod`: `{"description":"A collapsible aluminum stadia rod with metric graduations and a reflective target panel. Extends to four meters and collapses to a meter and a half. The red-and-white markings are faded but still legible. Used in conjunctio…`
- Row 007 `item_datum_plate_bronze`: `{"description":"A small bronze plaque stamped with the survey datum designation and a crosshair center mark. Installed at established benchmarks to serve as a permanent reference point. Bronze because it does not rust, does not spark, and …`
- Row 008 `item_concrete_mix`: `{"description":"A twenty-kilogram sack of pre-mixed concrete with a cold-weather additive. Sets in four hours even below freezing. Used for monument foundations, vault reinforcement, and emergency structural repairs. The aggregate is crush…`
- Row 009 `item_forged_rotor_shaft`: `{"description":"A precision-forged steel shaft machined to sub-millimeter tolerance. The bearing journals are polished to a mirror finish and the keyway is broached for a shear pin. This is the heart of the flywheel assembly: everything el…`
- Row 010 `item_magnetic_bearing_coil`: `{"description":"A set of electromagnetic bearing coils wound with lacquered copper wire and potted in epoxy. When energized, they levitate the rotor shaft on a magnetic field, eliminating mechanical contact at operating speed. The control …`
- Row 011 `item_high_vacuum_pump`: `{"description":"A compact turbomolecular pump capable of pulling a vacuum of one ten-thousandth of a torr. The rotor spins at forty thousand RPM on its own magnetic bearings. Without this, the flywheel's aerodynamic drag would turn stored …`
- Row 012 `item_containment_ring_steel`: `{"description":"A forged steel ring, two centimeters thick and one meter in diameter, designed to encircle the flywheel rotor. In the event of a catastrophic rotor failure, the ring absorbs the initial fragment impact and redirects the ene…`
- Row 013 `item_reinforced_concrete_vault`: `{"description":"A pre-cast reinforced concrete vault section with embedded steel rebar and anchor bolt channels. Weighs over a ton. Designed to be lowered into the flywheel pit and bolted together to form a containment vault that can redir…`
- Row 014 `item_seismic_damper_pad`: `{"description":"A layered elastomer-and-steel isolation pad that sits between the flywheel foundation and the bedrock. Absorbs micro-tremors, rotor imbalance vibrations, and the occasional seismic event. Without it, a four-ton rotor at six…`
- Row 015 `item_vacuum_pump_oil`: `{"description":"A liter of synthetic vacuum pump oil with low vapor pressure. Used to lubricate the roughing pump and maintain the turbomolecular pump's backing vacuum. The oil darkens with use as it absorbs water vapor and trace contamina…`
- Row 016 `item_bearing_grease`: `{"description":"A tube of synthetic grease rated for continuous operation at one hundred twenty degrees Celsius. Used on the flywheel's touchdown bearings — the mechanical backup that engages when the magnetic suspension loses power. Witho…`
- Row 017 `item_rotor_balancing_kit`: `{"description":"A kit containing precision weights, a stroboscopic balancer, and a set of balance-adjustment shims. Used to correct rotor imbalance that develops over time from thermal cycling and material fatigue. An unbalanced rotor at s…`
- Row 018 `item_portable_pid_detector`: `{"description":"A hand-held photoionization detector with a UV lamp module and interchangeable sensor heads. Responds to a broad range of volatile organic compounds and many inorganic gases. The display shows a normalized concentration bar…`
- Row 019 `item_detector_sensor_module`: `{"description":"An interchangeable sensor head for the portable PID detector. Different modules are optimized for different bands: low-band for nerve agents and blister agents, medium-band for corrosive vapors, wide-band for broad-spectrum…`
- Row 020 `item_hermetic_sample_ampoule`: `{"description":"A borosilicate glass ampoule with a PTFE-lined screw cap and a vacuum-seal indicator dot. Designed to hold atmospheric or soil samples for transport back to the shelter laboratory. The interior is purged and sterile. Once t…`
- Row 021 `item_hot_dust_drum`: `{"description":"A drum of concentrate from the electrostatic air scrubber: the fallout the plates caught so the rest of us would not. Lid welded, sides taped, paint marked with the trefoil. It does not decay on any schedule that matters to…`
- Row 022 `item_sludge_cake`: `{"description":"A grey-green pressed block of silt and settled muck from the deep sump centrifuge. Heavy, reeking, and faintly warm to the palm. The assay says there is metal in it - a little. The foundry pays in scrap what the ground give…`
- Row 023 `item_tailings_drum`: `{"description":"A rust-painted drum of centrifuge tailings, lid clamped and the seam sealed with tar. The concentrate even the sump would not keep. Handle with gloves, bury it deep, and do not camp downstream of the hole.","displayName":"S…`
- Row 024 `dosimeter`: `{"contamination":0,"description":"A pen-sized instrument on a worn lanyard, its window scratched but the needle still free. It measures accumulated dose in rads, no batteries needed, just a charge you cannot renew. One per person is the ru…`
- Row 025 `geiger_counter`: `{"contamination":0,"description":"A boxy field unit with a speaker grille and a dial marked in rads per hour. It reads the fallout around you, where the dosimeter only counts what you already absorbed. The click rate climbs with the contam…`
- Row 026 `iodine_pills`: `{"contamination":0,"description":"A small blister strip of potassium iodide tablets in foil that still crinkles. Taken before or just after exposure, they saturate the thyroid so it passes on the radioactive kind. Five pills to a pack, lig…`
- Row 027 `anti_rad`: `{"contamination":0,"description":"Decorporation medication in a foil pack of capsules. Taken after exposure, it pulls accumulated dose out of the body, clearing fifty rads per dose. Not a cure, just a deduction, and the body pays for it la…`
- Row 028 `gas_mask`: `{"contamination":0,"degradeRate":1.0,"description":"A full-face respirator with twin filter canisters and a rubber seal that still holds. It cuts airborne contamination by thirty percent, enough for minutes in heavy ash and hours in light …`
- Row 029 `hazmat_suit`: `{"contamination":0,"degradeRate":0.5,"description":"A one-piece sealed suit of layered PVC with boot covers and a hood. It stops eighty percent of radiation on the body, the best protection you can wear into a hot zone, and every percent s…`
- Row 030 `water_filter`: `{"contamination":0,"description":"A hand pump filter the size of a thermos, a ceramic element inside and a rubber bulb outside. It turns standing water into something you can drink without trading rads for thirst, and no power is needed. O…`
- Row 031 `air_filter`: `{"contamination":0,"description":"A rectangular panel filter for shelter air systems, sealed in heavy paper. It pulls fallout dust out of the air drawn into a bunker, and the sealing edge has to sit perfectly or it is just a cardboard rect…`
- Row 032 `clean_water`: `{"contamination":0,"description":"A sealed bottle of clean water, clear to the bottom. It restores forty points of thirst without adding anything to your dose, and even that small certainty, drinking without checking the color first, lifts…`
- Row 033 `irradiated_water`: `{"contamination":0.5,"description":"Murky water in a plastic bottle, silt settled in the bottom. It restores twenty-five points of thirst, but drinking it adds half a point of contamination to your dose. Traded at two, because someone alwa…`
- Row 034 `canned_food`: `{"contamination":0,"description":"A tin can with a paper label gone grey at the edges. It restores forty points of hunger, and the slow ceremony of heating it, or eating it cold from the tin, is worth two points of morale on its own. Half …`
- Row 035 `fuel`: `{"contamination":0,"description":"A plastic jerrycan of fuel, sloshing heavy. Two kilos of diesel or kerosene, worth fourteen, and every liter has a story about who siphoned it and from what. Twenty cans stack in a corner of the bunker, an…`
- Row 036 `cloth`: `{"contamination":0,"description":"Folded cloth, cotton or wool or whatever was left in a factory flat. A fifth of a kilo a bolt, worth a little over one, and it stacks twenty deep. It mends clothes, lines boots, wraps wounds, muffles sound…`
- Row 037 `scrap_metal`: `{"contamination":0,"description":"Pieces of metal: brackets, panels, a car door someone already stripped. Half a kilo each, worth a little over one, and twenty stack to a load a person can carry. It becomes braces, patch plates, traps and …`
- Row 038 `bandage`: `{"contamination":0,"description":"A rolled bandage with a strip of tape and a sealed gauze pad. It restores thirty points of health, stopping the bleed and covering a wound until it can heal on its own. Not a cure, a delay that gives the b…`
- Row 039 `raw_meat`: `{"contamination":0.3,"description":"A slab of raw meat wrapped in paper, still cold at the edges. Eaten uncooked it adds three tenths of a point of contamination to your dose, so it is always cooked first, over a fire or a stove. Half a ki…`
- Row 040 `cooked_meat`: `{"contamination":0,"description":"Cooked meat, dark on the outside, still warm in the middle of the cut. It restores fifty points of hunger and three points of morale, because meat is the difference between surviving and having had dinner.…`
- Row 041 `dirty_water`: `{"contamination":0.6,"description":"Water scooped from a puddle or a barrel, brown at the bottom. Drinking it adds six tenths of a point of contamination to your dose, and it offers nothing back in return. Worth two, and that price is only…`
- Row 042 `morphine`: `{"contamination":0,"description":"An amber glass ampoule of pharmaceutical morphine, batch seal intact. It restores thirty-five points of health where stronger medicine is wasted on a body that only needs the pain to stop. Two hundred gram…`
- Row 043 `chelation_agent`: `{"contamination":0,"description":"A sealed pharmaceutical vial of DMSA compound, pre-war stock. It binds heavy radioisotopes in the bloodstream and escorts them out through the kidneys. One course costs a week and the patient stays close t…`
- Row 044 `potassium_iodide`: `{"contamination":0,"description":"A small white tablet in a foil blister, stamped with a half-life and a dosage. It saturates the thyroid with stable iodine so the radioactive kind finds no purchase. Timing matters more than amount; taken …`
- Row 045 `medical_kit`: `{"contamination":0,"description":"A canvas kit with rolled bandages, tape, scissors and a small bottle of antiseptic. It restores sixty points of health, a proper field kit that can close a cut, pack a wound and stabilize someone for the w…`
- Row 046 `battery`: `{"contamination":0,"description":"A sealed battery, the kind that powered car doors and sirens in another year. Two tenths of a kilo, worth five, and ten stack in a crate that keeps radios, clocks and meters alive a little longer. Every ba…`
- Row 047 `calibration_kit`: `{"contamination":0,"description":"A small case of weights, shims and reference cards for zeroing instruments. It keeps dosimeters and geiger counters honest, because a meter that lies gets people killed by the numbers. Four tenths of a kil…`
- Row 048 `tweezers`: `{"contamination":0,"description":"Stainless tweezers with a fine point, kept in a leather sleeve. Worth eighteen, which sounds like a lot for a pair of tweezers, until you need a shard of glass out of a hand or a splinter out of a boot sol…`
- Row 049 `splint`: `{"contamination":0,"description":"Two flat boards with padding and a roll of webbing, sized for an arm or a leg. It holds a broken bone straight so the break can set, worth nine, four tenths of a kilo. The webbing gets reused until it fray…`
- Row 050 `antibiotics`: `{"contamination":0,"description":"A blister pack of antibiotics, sealed and dry. Ten packs stack to a weight you can forget, each pack worth ten, which makes it the best value per gram in the wasteland. They treat the infections that turn …`
- Row 051 `jewelry`: `{"contamination":0,"description":"A small piece of jewelry: a ring, a chain, a pin that catches the light. It restores two points of morale when worn, because the person who wears it is not completely reduced yet. Nearly weightless, fifty …`
- Row 052 `diamond`: `{"contamination":0,"description":"A loose cut stone kept in a dented steel specimen box. It has no practical use at the Holdfast, but the Cold Ledger still recognizes its old scarcity.","displayName":"Cut Diamond","durability":0,"empShield…`
- Row 053 `currency`: `{"contamination":0,"description":"Paper currency from before, bundled with a band that still says a bank name nobody visits. Worth twenty at trade tables, which is what a collector pays and what a fire starter would not. A hundred bills st…`
- Row 054 `mechanical_parts`: `{"contamination":0,"description":"Gears, shafts, bearings and fasteners in a greasy bag, the innards of machines that no longer run whole. Three trade units for a fifteenth of a kilo, fifty to a stack. They rebuild pumps, generators, latch…`
- Row 055 `electronic_scrap`: `{"contamination":0,"description":"Circuit boards, wiring and chips pulled from dead electronics, the EMP and the years having done the killing. A tenth of a kilo, six trade units, fifty to a stack. Some of it is worth nothing, and some of …`
- Row 056 `item_radiosonde`: `{"category":"equipment","description":"A salvaged radiosonde payload, recovered after atmospheric flight. Contains intact sensors and telemetry logs.","display_name":"Recovered Radiosonde Package","durability":0.85,"id":"item_radiosonde","…`
- Row 057 `solar_cell`: `{"contamination":0,"description":"A single solar cell, glass intact or cracked, frame bent but the wafer still blue. One point two kilos, twenty-two trade units, ten to a stack. Charged, it feeds a battery; broken, it is still the best gla…`
- Row 058 `chemicals`: `{"contamination":0.05,"description":"Containers of industrial chemicals: acids, solvents, powders in unlabeled jars. Two hundred fifty grams, five trade units, thirty to a stack, and handling them carries a contamination risk of one twenti…`
- Row 059 `handheld_radio`: `{"contamination":0,"description":"A handheld radio with a rubber antenna and a cracked dial face. It receives the bands that still carry voices, static, and the occasional transmission from a settlement you cannot reach. Eight tenths of a …`
- Row 060 `engine`: `{"contamination":0,"description":"An engine block, complete enough to turn: pistons, head, and the wiring that used to be the harness. Twenty-five kilos, worth eighty, full durability when it is whole, and one is all a person carries. It p…`
- Row 061 `roots`: `{"contamination":0,"description":"Washed roots, pale and knobby, tied in a bundle. Eight points of hunger per serving, one trade unit, two tenths of a kilo, twenty to a stack. They boil soft in an hour and taste like nothing, which is fine…`
- Row 062 `berries`: `{"contamination":0,"description":"A handful of dark berries in a folded leaf, soft at the press of a thumb. Six points of hunger, one trade unit, twenty bundles to a stack. Foragers argue about which bushes are safe, and the argument has n…`
- Row 063 `vacuum_tube`: `{"contamination":0,"description":"A glass vacuum tube with filigreed pins, still intact after decades on a shelf. It carries signals the way the old world carried conversations: through heated wire and careful vacuum. Worth eight, a tenth …`
- Row 064 `spring_mechanism`: `{"contamination":0,"description":"A coiled spring mechanism, still tensioned inside its housing. It stores energy the way a lung stores breath, and releases it the way a memory releases itself: all at once. Worth six, a fifth of a kilo, fi…`
- Row 065 `phonograph_needle`: `{"contamination":0,"description":"A tiny sapphire needle, still mounted in its cartridge. It reads the grooves of a record the way a finger reads braille: by feeling the shape of something that was made to be heard. Worth four, nearly weig…`
- Row 066 `projector_bulb`: `{"contamination":0,"description":"A high-wattage projector bulb, filament intact or merely resting. It throws light through a lens the way memory throws light through time: bright enough to see, not bright enough to stay. Worth twelve, a q…`
- Row 067 `lubricant_oil`: `{"contamination":0,"description":"A small can of precision lubricant oil, the kind used in clockwork and camera shutters. It reduces friction the way patience reduces panic: slowly, and only when applied correctly. Worth three, a tenth of …`
- Row 068 `film_reel`: `{"contamination":0,"description":"A metal film reel with a few meters of 8mm celluloid still wound tight. The images on it are someone's birthday, someone's parade, someone's last clear day. Worth fifteen, three tenths of a kilo, five to a…`
- Row 069 `antenna_coil`: `{"contamination":0,"description":"A wound copper antenna coil, tinned and still conductive after years in a damp bunker. It catches signals the way a shoreline catches driftwood: whatever comes close enough to touch. Worth ten, two tenths …`
- Row 070 `soldering_kit`: `{"contamination":0,"description":"A small soldering kit with a coil of rosin-core solder, a tip cleaner, and a pencil iron that still heats when given a battery. It joins wire to wire and trace to trace, which is how the old world fixed an…`
- Row 071 `music_box_comb`: `{"contamination":0,"description":"A brass music box comb with teeth still filed to pitch. It plucks the cylinder the way a fingernail plucks a thread: each tooth a note, each note a ghost. Worth nine, three tenths of a kilo, five to a stac…`
- Row 072 `spring_key`: `{"contamination":0,"description":"A small winding key for a music box or clock mechanism, still fitted to its shaft. It stores torque the way a promise stores obligation: tight, and released all at once. Worth four, a tenth of a kilo, ten …`
- Row 073 `typewriter_ribbon`: `{"contamination":0,"description":"A dried typewriter ribbon, ink still dark in the fabric but the strike surface gone to dust. It leaves no mark, which is the tragedy of all good tools worn past their last honest use. Worth three, nearly w…`
- Row 074 `machine_oil`: `{"contamination":0,"description":"A small can of machine oil, the thin kind that runs into gears and bearings and makes them forget they ever seized. It stops rust the way a good day stops despair: temporarily, and only where it reaches. W…`
- Row 075 `camera_lens_cleaner`: `{"contamination":0,"description":"A small lens cleaning kit with a blower brush and a strip of microfiber cloth. It clears fog and dust from glass the way a clear thought clears confusion: slowly, and only when you are patient enough to us…`
- Row 076 `photographic_film`: `{"contamination":0,"description":"A sealed canister of undeveloped 120 film, expiration date long past but the emulsion still potentially viable. It captures light the way a promise captures trust: briefly, and only if you act before it fa…`
- Row 077 `item_acoustic_decoy`: `{"contamination":0,"description":"A salvaged acoustic decoy module, still responsive to sound triggers. It emits a localized auditory signature that draws hostile attention away from the source. Fragile, improvised, and worth more in the r…`
- Row 078 `item_ammonium_nitrate_sack`: `{"contamination":0,"description":"A fifty-kilo sack of fertilizer-grade ammonium nitrate, the kind that feeds fields and, under the wrong conditions, changes them. Sealed in a worn canvas sack with a printed lot number that predates the ex…`
- Row 079 `item_amnestic_syrup`: `{"contamination":0,"description":"A chilled bottle of amnestic syrup, labeled in faded pharmacy script. It induces temporary memory suppression — a mercy in some cases, a liability in others. Worth twenty-two, three tenths of a kilo, five …`
- Row 080 `item_anchor_notes`: `{"contamination":0,"description":"A sheaf of handwritten notes tied with twine, detailing fixed coordinates, shelter layouts, and cached supply points. The handwriting is steady, the ink faded, the information older than the writer. Worth …`
- Row 081 `item_ash_ghillie`: `{"contamination":0,"degradeRate":0.5,"description":"A salvaged ghillie wrap woven from ash-colored fabric strips and frayed cord. It breaks up a silhouette the way a lie breaks up a confrontation: only if you are patient enough to apply it…`
- Row 082 `item_bio_plastic`: `{"contamination":0,"description":"A flexible sheet of mycelium-based bioplastic, grown in a darkroom and cured under pressure. It seals tanks, patches suits, and lines containers the way patience seals wounds: imperfectly, but well enough …`
- Row 083 `item_black_water_vial`: `{"contamination":0,"description":"A sealed glass vial of ultra-filtered black water, drawn from a deep aquifer and run through three stages of charcoal and pressure. It restores thirst without the usual contamination tax, which makes it wo…`
- Row 084 `item_co2_scrubber_cartridge`: `{"contamination":0,"description":"A cylindrical CO2 scrubber cartridge filled with activated charcoal and soda lime. It strips carbon dioxide from recirculated air the way a deadline strips hesitation: efficiently, and with an expiration d…`
- Row 085 `item_epoxy_injector`: `{"contamination":0,"description":"A dual-cartridge epoxy injector with a static mixer tip. It bonds metal to metal, ceramic to ceramic, and hope to desperation in under five minutes. Worth eleven, three tenths of a kilo, eight to a stack. …`
- Row 086 `item_faraday_mesh`: `{"contamination":0,"description":"A roll of woven copper Faraday mesh, fine enough to wrap a circuit and thick enough to stop a pulse. It shields electronics the way a locked door shields a room: only if the seal is complete. Worth sixteen…`
- Row 087 `item_frostbite_salve`: `{"contamination":0,"description":"A tin of medicated frostbite salve with a sharp camphor smell. It restores circulation and reduces tissue damage when applied early, which is the only time it works. Worth seven, a tenth of a kilo, ten to …`
- Row 088 `item_fungicide_fogger`: `{"contamination":0,"description":"A pressurized fungicide fogger with a replaceable cartridge. It clears mold from sealed rooms and fungal growth from ventilation shafts the way a whistle clears a room: loudly, and with mixed results. Wort…`
- Row 089 `item_galvanized_rebar`: `{"contamination":0,"description":"A length of hot-dip galvanized rebar, still coated and still straight. It reinforces concrete the way principles reinforce decisions: visibly, and only if you pour before it sets. Worth eight, three kilos,…`
- Row 090 `item_glycol_antifreeze_canister`: `{"contamination":0,"description":"A sealed one-litre canister of ethylene glycol antifreeze. It prevents freezing in engines and heat exchangers the way morale prevents collapse in a long winter: chemically, and not for everyone. Worth fiv…`
- Row 091 `item_hermetic_hatch_silicone_gasket`: `{"contamination":0,"description":"A custom-cut silicone gasket for a bunker hermetic hatch. It seals against pressure, fallout dust, and the slow creep of air that should not be moving. Worth twenty-one, a quarter kilo, five to a stack. Bu…`
- Row 092 `item_high_tensile_steel_culvert_brace`: `{"contamination":0,"description":"A curved high-tensile steel brace salvaged from a collapsed culvert section. It spans gaps the way a decision spans consequences: with structural integrity, and only if the load is calculated. Worth ninete…`
- Row 093 `item_insulated_snowmobile_battery`: `{"contamination":0,"description":"A heavy insulated battery from a snowmobile engine block, still holding a charge through the cold that killed the machine it came from. It powers heaters, radios, and the small comforts people refuse to gi…`
- Row 094 `item_lead_shielded_sample_cask`: `{"contamination":0,"degradeRate":0.5,"description":"A small lead-shielded cask for transporting radioactive samples. It protects the handler the way a secret protects the guilty: completely, and with moral weight nobody discusses. Worth se…`
- Row 095 `item_lead_visor`: `{"contamination":0,"degradeRate":1.0,"description":"A heavy lead-glass visor mounted in a leather head harness. It protects the eyes and face from radiant heat and flash, the way sunglasses protect the eyes from ordinary light: only this k…`
- Row 096 `item_lithium_salts`: `{"contamination":0,"description":"A sealed pouch of lithium carbonate salts, the psychiatric staple that became a wasteland trade good. It stabilizes mood the way a fixed schedule stabilizes a day: imperfectly, but enough to function. Wort…`
- Row 097 `item_mine_prod`: `{"contamination":0,"description":"A bundle of insulated copper mine prods, the kind used to test electrical continuity in dangerous circuits. They save lives the way a second opinion saves a diagnosis: by confirming what should not be assu…`
- Row 098 `item_mycelium_bricks`: `{"contamination":0,"description":"Compressed bricks of cultivated mycelium bound with agricultural waste. They insulate, they dampen sound, and they grow if you leave them in the dark too long. Worth thirteen, three kilos, ten to a stack. …`
- Row 099 `item_prussian_blue_chelating_pellets`: `{"contamination":0,"description":"A bottle of Prussian blue chelating pellets, the cesium and thallium binder that turns internal contamination into something the body can pass. Worth twenty-six, a tenth of a kilo, five to a stack. It does…`
- Row 100 `item_radon_detector_electret`: `{"contamination":0,"description":"A passive radon detector electret chamber, small enough to carry and slow enough to trust. It accumulates charge the way a bunker accumulates secrets: over time, and only if left undisturbed. Worth fifteen…`
- Row 101 `item_rebreather_scrubber`: `{"contamination":0,"description":"A compact rebreather scrubber pack with replaceable CO2 and moisture cartridges. It recycles exhaled air the way a library recycles stories: by filtering out the parts that are dangerous to repeat. Worth e…`
- Row 102 `item_ro_membrane`: `{"contamination":0,"description":"A thin-film reverse osmosis membrane sheet, rated for brackish and lightly contaminated water. It turns undrinkable water into drinkable water the way discipline turns chaos into routine: slowly, with wast…`
- Row 103 `item_scopolamine_root`: `{"contamination":0,"description":"A dried bundle of scopolamine-bearing root, harvested from a plant that thrives in disturbed soil. It suppresses memory and nausea, which makes it useful for trauma and for travel. Worth twenty, nearly wei…`
- Row 104 `item_sealed_lead_pig`: `{"contamination":0,"degradeRate":0.5,"description":"A sealed lead container for transporting radioactive sources. It is heavy, warm to the touch, and marked with a trefoil that nobody alive today remembers being taught to fear. Worth fifte…`
- Row 105 `item_snow_goggles_improvised`: `{"contamination":0,"description":"Goggles carved from scrap leather and fitted with slotted wood or bone. They prevent snow blindness the way a shelter prevents hypothermia: imperfectly, but with enough discipline to make the difference. W…`
- Row 106 `item_sound_baffling`: `{"contamination":0,"description":"Folded panels of acoustic foam and compressed fibreglass, salvaged from recording studios and server rooms. They deaden sound the way a closed mouth deadens conflict: partially, and only if the seal is hon…`
- Row 107 `item_suitcase_locked`: `{"contamination":0,"description":"A hard-shell suitcase with a combination dial still set to factory default. It rattles when shaken, which means something solid is inside, and it smells faintly of old tobacco and camphor. Worth seventeen,…`
- Row 108 `item_surgical_bone_chisel`: `{"contamination":0,"description":"A stainless steel bone chisel with a sterilized handle and a blade that still holds an edge. It removes bone the way a decision removes doubt: precisely, and with finality. Worth twenty-four, two tenths of…`
- Row 109 `item_teddy_bear`: `{"contamination":0,"description":"A worn teddy bear with one button eye and a fur matted by ash and time. It restores three points of morale simply by being present, which is more than most things in the bunker manage. Worth eight, two ten…`
- Row 110 `item_thermal_paste`: `{"contamination":0,"description":"A syringe of ceramic thermal paste, the kind used between heat spreaders and processors. It bridges microscopic gaps the way diplomacy bridges ideological ones: thinly, evenly, and with the understanding t…`
- Row 111 `item_welders_glass`: `{"contamination":0,"description":"A set of darkened welding glass plates in a steel frame. They filter the arc the way a bunker filters fallout: by blocking the part that does permanent damage. Worth thirteen, a quarter kilo, five to a sta…`
- Row 112 `aa_batteries`: `{"contamination":0,"description":"A pair of alkaline AA batteries, still holding a faint charge. They power flashlights, radios, and the small devices people refuse to let go of. Worth three, a tenth of a kilo, twenty to a stack. Every bat…`
- Row 113 `alcohol_wipes_box_10_of_10`: `{"contamination":0,"description":"A sealed box of ten isopropyl alcohol wipes. They sterilize surfaces and skin the way silence sterilizes a room: quickly, and only where applied. Worth four, two tenths of a kilo, ten to a stack. Medics, m…`
- Row 114 `ammo_762x54r_jhp_ap`: `{"contamination":0,"description":"A handful of 7.62x54R jacketed hollow-point armour-piercing rounds. They punch through cover and expand in tissue the way a bad decision punches through a truce: with consequences nobody wanted. Worth elev…`
- Row 115 `ammo_357`: `{"contamination":0,"description":"A box of .357 revolver rounds, brass casings, lead bullets. Feeds jury-rigged pipe rifles and revolvers. The box is dented. The rounds are clean. The ammunition works.","displayName":".357 Rounds","durabil…`
- Row 116 `ammo_12g`: `{"contamination":0,"description":"A box of 12-gauge shells, plastic hulls, lead shot. Feeds scrap shotguns. The box is dented. The shells are clean. The ammunition works.","displayName":"12-Gauge Shells","durability":0,"empShielded":false,…`
- Row 117 `ammo_308`: `{"contamination":0,"description":"A box of .308 Winchester ammunition, brass casings, copper bullets. Feeds held-bolt rifles. The box is dented. The rounds are clean. The ammunition works.","displayName":".308 Rounds","durability":0,"empSh…`
- Row 118 `ammo_556`: `{"contamination":0,"description":"A box of 5.56mm ammunition, brass casings, copper bullets. Feeds assault rifles. The box is dented. The rounds are clean. The ammunition works.","displayName":"5.56mm Rounds","durability":0,"empShielded":f…`
- Row 119 `ammo_762`: `{"contamination":0,"description":"A box of 7.62mm ammunition, brass casings, copper bullets. Feeds light machine guns. The box is dented. The rounds are clean. The ammunition works.","displayName":"7.62mm Rounds","durability":0,"empShielde…`
- Row 120 `antiseptic_1l_of_1l`: `{"contamination":0,"description":"A one-litre bottle of surgical-grade antiseptic solution. It cleans wounds and surfaces the way a verdict cleans a court: decisively, and not always gently. Worth seven, a kilo, five to a stack. The label …`
- Row 121 `battery_pack`: `{"contamination":0,"description":"A sealed battery pack, the kind that powered tools and emergency lighting before the exchange. It stores energy the way a promise stores obligation: visibly, and with an expiration date nobody reads. Worth…`
- Row 122 `box_of_nails_10`: `{"contamination":0,"description":"A small carton of ten steel nails, galvanized and straight. They hold wood the way a contract holds people: only if both sides are honest and the surface is prepared. Worth two, a tenth of a kilo, twenty t…`
- Row 123 `canned_soup`: `{"contamination":0,"description":"A can of condensed soup, label faded but seal intact. It restores thirty points of hunger and one point of morale, because warmth is not just temperature. Worth eight, three tenths of a kilo, ten to a stac…`
- Row 124 `childrens_books`: `{"contamination":0,"description":"A bundle of children's picture books, pages intact but covers softened by damp. They teach reading the way a bunker teaches patience: one letter, one day, one survival at a time. Worth five, three tenths o…`
- Row 125 `cigarette_lighter`: `{"contamination":0,"description":"A brass pocket lighter, still filled and still sparking. It creates fire the way a speech creates momentum: out of nothing, and only if the conditions are right. Worth six, two tenths of a kilo, ten to a s…`
- Row 126 `clean_water_jug`: `{"contamination":0,"description":"A one-litre jug of filtered clean water, sealed with a screw cap. It restores thirst without the contamination tax, which is the only tax people refuse to pay voluntarily. Worth twelve, a kilo, five to a s…`
- Row 127 `cooking_oil`: `{"contamination":0,"description":"A bottle of vegetable cooking oil, yellow and clear. It calms hunger the way diplomacy calms borders: by making everything more slippery and less direct. Worth four, three tenths of a kilo, ten to a stack.…`
- Row 128 `copper_wire_10m_of_10m`: `{"contamination":0,"description":"A coil of ten metres of solid-core copper wire, insulated and still bright. It carries current the way a road carries traffic: only if the path is clear and the connection is honest. Worth eight, three ten…`
- Row 129 `diesel_fuel`: `{"contamination":0,"description":"A can of diesel fuel, the smell unchanged since the last time a truck engine turned over. It powers generators, heaters, and the slow hope that something might still move. Worth ten, two kilos, five to a s…`
- Row 130 `dried_rations`: `{"contamination":0,"description":"A pack of dried meat and grain biscuits, vacuum-sealed and still edible. It restores twenty points of hunger and nothing else, which is exactly what a ration is supposed to do. Worth five, two tenths of a …`
- Row 131 `faraday_pack`: `{"contamination":0,"description":"A roll-up Faraday pack with conductive mesh lining and a magnetic seal. It shields electronics from EMP the way a basement shields people from blast: imperfectly, but better than nothing. Worth fourteen, f…`
- Row 132 `field_surgical_kit`: `{"contamination":0,"description":"A compact field surgical kit with scalpels, sutures, and a tourniquet. It closes wounds the way a treaty closes conflict: under pressure, with limited resources, and with the understanding that scarring is…`
- Row 133 `fuel_1l`: `{"contamination":0,"description":"A sealed one-litre can of fuel, diesel or kerosene, the kind that runs engines and stoves and keeps the dark at bay. Worth six, a kilo, five to a stack. Fuel is measured in litres but traded in survival. A…`
- Row 134 `fuel_cell`: `{"contamination":0,"description":"A compact hydrogen fuel cell, still pressurised and still delivering current. It powers sensors, radios, and life support the way a savings account powers a retirement: slowly, and only if you did not touc…`
- Row 135 `growing_manual`: `{"contamination":0,"description":"A water-damaged growing manual with soil charts and planting calendars. It turns dirt into food the way a teacher turns ignorance into skill: with patience, repetition, and the willingness to fail publicly…`
- Row 136 `iodine_tablets`: `{"contamination":0,"description":"A small bottle of potassium iodide tablets, the thyroid-blocking staple of fallout preparedness. They are bitter, cheap, and worth more than gold when the siren sounds. Worth five, two tenths of a kilo, te…`
- Row 137 `item_cassette_tape`: `{"contamination":0,"description":"A cassette tape with a handwritten label. The recording on it is someone's voice, telling a story that may or may not be true. Worth nine, a tenth of a kilo, ten to a stack. Recordings outlive the people w…`
- Row 138 `item_pre_war_photo_album`: `{"contamination":0,"description":"A leather-bound photo album filled with pre-war family photographs. The faces are strangers, the places are gone, and the captions are in handwriting nobody reads anymore. Worth thirteen, three tenths of a…`
- Row 139 `item_vinyl_collection`: `{"contamination":0,"description":"A crate of vinyl records, sleeves worn but discs intact. They spin at 33 rpm and carry music that predates the exchange by decades. Worth sixteen, two kilos, five to a stack. Gramophones are rare. Records …`
- Row 140 `mechanical_components`: `{"contamination":0,"description":"An assortment of gears, cams, and bearings scavenged from dead machinery. They are the vocabulary of repair, and without them nothing mechanical says anything intelligible. Worth seven, a quarter kilo, twe…`
- Row 141 `medkit`: `{"contamination":0,"description":"A canvas medical kit with bandages, antiseptic, and basic surgical tools. It stabilises the injured the way a truce stabilises a war: temporarily, and only if both sides respect the terms. Worth eleven, fo…`
- Row 142 `metal_pipe`: `{"contamination":0,"description":"A length of steel pipe, threaded at one end and rusted at the other. It moves water, gas, and ideas through confined spaces the way a messenger moves through hostile territory: quickly, and with risk. Wort…`
- Row 143 `military_grade_hatchet`: `{"contamination":0,"description":"A steel-frame hatchet with a polymer handle and a blade balanced for throwing or chopping. It splits wood the way a verdict splits a room: with finality, and with attention to who is holding it. Worth twel…`
- Row 144 `military_mre`: `{"contamination":0,"description":"A pre-war military Meal, Ready-to-Eat. The pouch is swollen at one corner, which means the contents are still safe, and the heater works if you have a match. Worth seven, five tenths of a kilo, ten to a st…`
- Row 145 `military_radio`: `{"contamination":0,"description":"A military-specification radio set with encryption modules and a frequency range that still includes the bands that matter. It receives orders, weather, and the occasional voice that sounds like authority.…`
- Row 146 `military_rations`: `{"contamination":0,"description":"A pack of military-issue ration bars, dense and tasteless and reliable. They restore forty points of hunger and zero points of joy, which is exactly what a survival ration is designed to do. Worth six, thr…`
- Row 147 `military_supply_crate`: `{"contamination":0,"description":"A wooden military supply crate with stencilled markings and a lid that still seals. Inside is the kind of inventory that makes a bunker feel like a fortress: ammo, rations, medical supplies, and the quiet …`
- Row 148 `music_box_fur_elise`: `{"contamination":0,"description":"A replacement cylinder for a music box, programmed with the opening bars of Fur Elise. It plays the same melody every time, which is either comfort or curse depending on the day. Worth seven, a tenth of a …`
- Row 149 `night_vision_scope`: `{"contamination":0,"description":"A generation-one night vision scope with a damaged IR illuminator and a lens that still gains in the dark. It turns night into grey the way optimism turns despair into strategy: imperfectly, but enough to …`
- Row 150 `plastic_material`: `{"contamination":0,"description":"A sheet of industrial-grade plastic sheeting, the kind used for vapour barriers and temporary shelters. It keeps moisture out the way a lie keeps the truth out: completely, and only if the edges are sealed…`
- Row 151 `scrap_plastic`: `{"contamination":0,"description":"Torn sheeting, cracked containers, bottle shards — the shelter sheds plastic the way it sheds heat. Sorted and baled, it is feedstock for the retort. Worth one, a fifth of a kilo, fifty to a stack.","displ…`
- Row 152 `synthetic_fuel_canister`: `{"contamination":0,"description":"A dented can of reclamation fuel rendered from waste plastic in the back-draft retort. It burns dirty and runs engines rough — expect more wear, fewer kilometres to the can. Worth eight, four kilos, ten to…`
- Row 153 `carbon_black_powder`: `{"contamination":0,"description":"Fine soot pressed from the retort's draft chamber. Seals gaskets, cuts rubber compound, recharge respirator inserts. Breathing it is its own small emergency. Worth three, half a kilo, twenty to a stack.","…`
- Row 154 `protective_childs_coat`: `{"contamination":0,"degradeRate":0.5,"description":"A small padded coat with a detachable hood, sized for a child. It provides insulation and a modicum of rad protection the way a promise provides safety: only if the adult keeps it. Worth …`
- Row 155 `rubber_hose`: `{"contamination":0,"description":"A length of reinforced rubber hose, still flexible and still capable of carrying water or air. It connects systems the way a mediator connects people: by finding a path through resistance. Worth three, two…`
- Row 156 `scrap_wood`: `{"contamination":0,"description":"A bundle of salvaged wood planks and boards, warped but still structural. It builds shelves, beds, and the small walls that make a bunker feel like a home. Worth two, a kilo, twenty to a stack. Carpenters …`
- Row 157 `sealed_government_document`: `{"contamination":0,"description":"A manila envelope marked RESTRICTED in faded ink, sealed with wax that cracked long ago. The contents are bureaucratic and obsolete, but bureaucracy once ran the world, and its remnants still carry weight.…`
- Row 158 `seed_packets`: `{"contamination":0,"description":"A envelope of assorted vegetable seeds, some dated, some anonymous. They grow food the way a decision grows consequences: slowly, and only if the soil is honest. Worth six, a tenth of a kilo, twenty to a s…`
- Row 159 `spirits`: `{"contamination":0,"description":"A bottle of industrial-grade spirits, the kind used for cleaning, sterilising, and forgetting. It burns the throat and clears the mind the way a confrontation clears the air: painfully, and only for a mome…`
- Row 160 `steel_rebar`: `{"contamination":0,"description":"A length of steel reinforcing bar, rusted at the cut end and still straight. It reinforces concrete the way a conviction reinforces a person: visibly, and only if poured while the moment is still hot. Wort…`
- Row 161 `sugar`: `{"contamination":0,"description":"A sealed packet of white sugar, crystals still dry. It sweetens the bitter and preserves the fruit the way a compliment preserves a relationship: temporarily, and only if the timing is right. Worth two, a …`
- Row 162 `sulphur`: `{"contamination":0,"description":"A small jar of yellow sulphur powder, the kind that smells like rotten eggs and makes everything it touches flammable. It is the backbone of gunpowder, fertiliser, and the old-world chemical industry. Wort…`
- Row 163 `thermal_blanket`: `{"contamination":0,"description":"A mylar thermal blanket, crinkly and reflective. It retains body heat the way a good story retains attention: only if you wrap yourself in it completely. Worth three, a tenth of a kilo, twenty to a stack. …`
- Row 164 `tobacco_pouch`: `{"contamination":0,"description":"A leather tobacco pouch with a drawstring, still faintly scented with dried leaf. It restores two points of morale and costs three trade units, which is the exchange rate for a small comfort in a large dis…`
- Row 165 `water_bottle_0_5l_of_1l`: `{"contamination":0,"description":"A half-litre plastic water bottle, crushed slightly at the shoulder. It holds enough water to matter and not enough to be careless with. Worth four, a tenth of a kilo, twenty to a stack. Water is measured …`
- Row 166 `water_bottle_1l_full`: `{"contamination":0,"description":"A full one-litre water bottle, cap sealed and condensation on the outside. It is the difference between a short walk and a long one, between a clear head and a headache that starts behind the eyes. Worth s…`
- Row 167 `water_purification_tablets`: `{"contamination":0,"description":"A small bottle of water purification tablets, the chlorine-dioxide kind that kills bacteria and protozoa without adding taste. They make questionable water drinkable the way a judge makes a conflict resolv…`
- Row 168 `water_purification_tablets_40_of_40`: `{"contamination":0,"description":"A large bottle of forty water purification tablets, the chlorine-dioxide kind. It is enough to treat a bunker's worth of questionable water, which makes it worth more than the sum of its parts. Worth eight…`
- Row 169 `wood_block`: `{"contamination":0,"description":"A rough-cut block of hardwood, still smelling of resin and sawdust. It becomes furniture, tool handles, and the small structures that make a bunker feel made rather than dug. Worth two, two kilos, twenty t…`
- Row 170 `wool_blanket`: `{"contamination":0,"description":"A thick wool blanket, scratchy but warm. It prevents hypothermia the way a friend prevents despair: imperfectly, but with enough insistence to matter. Worth four, a kilo, ten to a stack. Cold is the oldest…`
- Row 171 `pistol_cz75_9x19`: `{"contamination":0,"description":"A CZ 75 semi-automatic pistol in 9x19mm, blued steel and Bakelite grips. It is accurate, reliable, and heavy enough to make a point without raising your voice. Worth thirty-five, a kilo, one to a stack. Gu…`
- Row 172 `ars_diagnosis_report`: `{"contamination":0,"description":"A medical report documenting acute radiation syndrome staging and treatment recommendations. It tells you how far along the damage has progressed, which is information most people are not brave enough to s…`
- Row 173 `concrete_mix`: `{"contamination":0,"description":"A sack of pre-mixed concrete powder, still usable if kept dry. It fills holes, strengthens walls, and makes the bunker feel permanent the way a promise feels binding: only if the mix is right and the pour …`
- Row 174 `family_photo_pendant`: `{"contamination":0,"description":"A small pendant containing a faded family photograph. It is worn close to the body, which is where grief and love both live. Worth eleven, two tenths of a kilo, ten to a stack. People who have lost everyth…`
- Row 175 `intake_valve_report`: `{"contamination":0,"description":"A technical report on intake valve corrosion and replacement schedule. It is boring, precise, and exactly what an engineer needs to keep an air system alive. Worth seven, a tenth of a kilo, five to a stack…`
- Row 176 `medical_inventory_report`: `{"contamination":0,"description":"A typed medical inventory report with columns for stock, expiry, and allocation. It is the difference between having medicine and knowing where it is. Worth six, a tenth of a kilo, five to a stack. Hospita…`
- Row 177 `refugee_screening_report`: `{"contamination":0,"description":"A triage and screening report for incoming refugees, with health assessments and contamination readings. It is the bureaucratic face of compassion, and it is as necessary as the compassion itself. Worth ei…`
- Row 178 `signal_source_report`: `{"contamination":0,"description":"A field report triangulating a radio signal source, with coordinates, signal strength, and a note on the voice that was heard. It is the closest thing to a map that the wasteland offers: direction instead …`
- Row 179 `supply_inventory_report`: `{"contamination":0,"description":"A logistics report listing bunker supplies by category, quantity, and reorder threshold. It is the difference between running out and running late. Worth five, a tenth of a kilo, five to a stack. Quarterma…`
- Row 180 `structural_report`: `{"contamination":0,"description":"A structural engineering report assessing load-bearing walls and ceiling spans. It is written in the language of safety and read in the language of fear. Worth seven, a tenth of a kilo, five to a stack. En…`
- Row 181 `treatment_protocol`: `{"contamination":0,"description":"A medical treatment protocol for a specific condition, written in shorthand that only another doctor would recognise. It is the difference between guessing and treating, which is the difference between dyi…`
- Row 182 `water_sample_contaminated`: `{"contamination":5,"degradeRate":0.5,"description":"A labelled water sample in a sealed glass vial, drawn from a known contaminated source. It is evidence, not drinking water, and the difference matters. Worth six, a tenth of a kilo, five …`
- Row 183 `radio_transcript_142_5`: `{"contamination":0,"description":"A typed transcript of a radio broadcast on frequency 142.5 kHz. The voice is calm, the content is unsettling, and the signal source has never been located. Worth twelve, a tenth of a kilo, five to a stack.…`
- Row 184 `rad_away`: `{"contamination":0.0,"description":"A pre-war IV chelating agent that binds heavy isotopes in the bloodstream and routes them through the kidneys. The label says 'EDTA-Isotope Flush' and lists a contraindication for kidney disease. Someone…`
- Row 185 `fuel_canister`: `{"contamination":0.0,"description":"A sealed metal canister of refined hydrocarbon fuel, the kind that runs generators and portable heaters without fouling the burner jets. The canister is heavy, the valve is stiff from cold, and the smell…`
- Row 186 `item_geiger_m3`: `{"contamination":0.0,"description":"A military-surplus M3 ionization counter in a rubberized case, calibrated for gamma and beta radiation. The display is a needle against a glass arc, marked in roentgens and millisieverts in two colors. T…`
- Row 187 `item_dosimeter_pen`: `{"contamination":0.0,"description":"A quartz-fiber dosimeter the size of a ballpoint pen. You clip it to your collar and it measures everything your body absorbs from the moment you take it out until the moment you read it against a viewer…`
- Row 188 `item_air_filter_hepa`: `{"contamination":0.0,"description":"A pleated glass-fiber HEPA filter cartridge for bunker intake fans. It traps particles down to 0.3 microns — fine enough to catch ash and fallout dust, not fine enough to catch vapors. The gray pleating …`
- Row 189 `item_desal_membrane`: `{"contamination":0.0,"description":"A spiral-wound polyamide membrane for reverse-osmosis water stacks. Clean side and dirty side are marked with a blue dot and a red dot, and if you install it backward you will pressurize the wrong chambe…`
- Row 190 `scrap_mechanical`: `{"contamination":0.0,"description":"A mixed lot of salvaged hardware: gears, bearings, a handful of bolts, a bracket that looks like it came off a vehicle door, a few springs. Nothing is labeled. Nothing is clean. Half of it is the right s…`
- Row 191 `scrap_electronic`: `{"contamination":0.0,"description":"A bag of stripped circuit boards, capacitors, loose resistors, and reeled copper trace wire pulled from dead appliances. The boards are scorched along one edge from the EMP. The capacitors may or may not…`
- Row 192 `scrap_chemical`: `{"contamination":0.0,"description":"Industrial chemicals in unlabeled containers: white powder, amber liquid, a sealed drum of something that smells faintly of hospitals. Some of it is solvents, some is stabilizers, some is fertilizer prec…`
- Row 193 `filter_pack`: `{"contamination":0.0,"description":"A flat activated-charcoal insert for M40-series respirators and civilian equivalents. The filter captures particulates and some volatile organics, but not all vapors and nothing nuclear. The outside of t…`
- Row 194 `inhaler`: `{"contamination":0.0,"description":"A modified salbutamol aerosol adapted by someone who knew what they were doing — the nozzle is not quite the right diameter and there is cloth tape around the canister join. It opens the airways in a sev…`
- Row 195 `herbal_tea`: `{"contamination":0.0,"description":"A cloth sachet of dried pine needles and wild mint, brewed in hot water. The pine needles are bitter. The mint covers it badly. Together they produce a tea that clears the sinuses, soothes a raw throat, …`
- Row 196 `item_taper_kit_opioid`: `{"contamination":0,"description":"An ampoule of buprenorphine in a measured oral dispenser, a small bottle of clonidine for the sweats, and a printed half-sheet carved from a sawed-off cereal box that lays out a seven-day dose-down plan. U…`
- Row 197 `item_stabilization_tea`: `{"contamination":0,"description":"A brew of dried valerian root, passionflower tops and a half-handful of chamomile, dried on a bunk-room window. A sedative withdrawal aid: it does not cure the sleeplessness, there is no cure for that, jus…`
- Row 198 `item_thiamine_dose`: `{"contamination":0,"description":"A single sealed vial of thiamine for IV push. Pre-war pharmacies kept them beside the alcohol prep trays; the bunker pharmacy salvaged the bundle. Restores what the drinker's body stops absorbing when the …`
- Row 199 `item_substitution_dose`: `{"contamination":0,"description":"A measured nikethamide tablet. A milder substitution therapy than the opioid taper kit; used to anchor the descending dose-down without flattening the worker to the floor. Take once a day, with water, neve…`
- Row 200 `smokeless_powder`: `{"description":"Fine granular powder in a sealed tin, the kind the pre-war benchrest shooters hoarded against the next shortage. It smells the same as it did in the cabinet; the cabinet is gone. A kilo feeds fourteen standard rifle loads w…`
- Row 201 `empty_brass_shell`: `{"description":"Spent brass, de-primed and tumbled, the kind that takes an afternoon per hundred if you do it by hand and ten minutes per hundred if you still have a tumbler. Each one is a future round waiting on a charge, a primer, and a …`
- Row 202 `reloading_primer`: `{"description":"A flat tin of small circular primers, the kind that fits inside a brass shell at the bench and is the difference between a load that goes off and a load that pretends to. Sold in tens, and a tin is ten. Worth three. They we…`
- Row 203 `cardboard_wad`: `{"description":"Cut cardboard discs in a cigarette tin, the kind that keeps the shot column together in a 12-gauge shell when the wadding machine is gone. Worth nothing by itself. A handful of them and a primer is the start of a load that …`
- Row 204 `duct_tape`: `{"description":"A roll of grey tape that still unsticks from itself, the kind of indefinite repair material that has held together more ladders, doors, and pipe joints in this sector than any one of them deserved. A small roll, three hundr…`
- Row 205 `wooden_plank`: `{"description":"A straight-grained plank, rough sawn, the kind reused as a bench top in the rebuilds and as a stock blank in the improvised ranges. Two kilos, worth two. The grain is the grain. The peace is not.","displayName":"Wooden Plan…`
- Row 206 `alcohol`: `{"description":"A 250-millilitre tin of ethanol with methanol mixed in to make it undrinkable, the kind the pre-war chemistry sets kept under the sink. Burns clean enough to use as a solvent and dirty enough to use as a molotov charge. Hal…`
- Row 207 `empty_tin_can`: `{"description":"An emptied tin can, the kind scavenged from the sealed cabinets at the back of every ruined kitchen. A molotov charges in these. A signal fire starts in these. A child's drum keeps time in these. Approximately one hundred g…`
- Row 208 `rope`: `{"description":"Ten metres of three-strand hemp rope on a coil, the kind still found in truck beds and barn corners and the holds of the shipwrecks. Holds a man. Holds a raft. Holds a barricade together under fire. Eight hundred grams, wor…`
- Row 209 `leather_strap`: `{"description":"A thirty-centimetre strap cut from a satchel handle or a bridle, the kind of nothing-and-everything component that turns a piece of pipe into a sling-usable weapon or a barrel into a back-pack. Two hundred grams, worth thre…`
- Row 210 `ammunition_brass`: `{"description":"A small bag of cut brass strip ready for draw on the reloading press at the Ordnance Foundry bench, the kind the loaders trade for half-rounds in the morning shift. Five hundred grams, worth fourteen. The Foundry buys it ba…`
- Row 211 `ammo_9x19`: `{"description":"A strip of nine-millimetre rounds in a foil pack, the standard civilian pistol caliber that survived the exchange in police station evidence lockers and basement gun safes. Smokeless load, ten to a strip, four hundred grams…`
- Row 212 `ammo_22lr`: `{"description":"A tin of .22 long rifle cartridges, rimfire, the smallest useful caliber that still arrives at the trade table. A brick of fifty rounds, half a kilo, worth three. The plinking ammunition of a vanished world; the rabbit ammu…`
- Row 213 `ammo_762x54r`: `{"description":"A can of rimmed 7.62x54R cartridges, the long-range hunting caliber in eastern service for a hundred and twenty years. A tin of twenty rounds, a kilo, worth twelve. They feed the Mosin-Nagant in the caches and the cosaques'…`
- Row 214 `ammo_357_jhp`: `{"description":"Box of hand-loaded .357 jacketed hollow-point rounds, the kind the reloading bench turns out one at a time with a single-stage press when the supply holds. Each round is a careful thing: a brass case, a hand-set primer, six…`
- Row 215 `ammo_12g_buck`: `{"description":"Hand-loaded 12-gauge shells with nine pellets of buckshot in a rolled crimp, the kind the range veterans spend a Sunday afternoon building when the wadding is right and the crimp is closed. Twenty rounds to a box, almost a …`
- Row 216 `ammo_308_incendiary`: `{"description":"Hand-loaded .308 with a magnesium-base incendiary tip, the kind the bench loaders built when the burst-incendiary contract inventory opened up. Eight rounds to a box, six hundred grams, worth twenty. A round is a round. A r…`
- Row 217 `ammo_556_subsonic`: `{"description":"A mismatched lot of 5.56 subsonic loads, the kind the bullet lube tank churned out before the tank was sold. Quieter than standard 5.56 — about as quiet as 5.56 ever gets, which is to say not quiet at all, but quieter. Thre…`
- Row 218 `ammo_improvised_rod`: `{"description":"A bundle of rebar rods cut to launcher length, sharpened on one end and taped on the other, the kind of projectile that fits the pipe launchers in the salvage ranges. Six rods to a grip-wrap, three hundred grams, worth two.…`
- Row 219 `ammo_improvised_burn`: `{"description":"A wadded rag soaked in alcohol inside an empty tin can, the kind of burn charge that lights the way to the door and lights the door behind it. Two cans per grip-wrap, four hundred grams, worth three. They are not ammunition…`
- Row 220 `weapon_pipe_shotgun`: `{"description":"A twelve-gauge made of welded pipe fittings and a salvaged receiver, the second build for the gun makers who couldn't get the first one to feed. Five kilos, worth twenty. Loud. It will fire twice without jamming and that is…`
- Row 221 `weapon_nail_driver`: `{"description":"A nine-millimetre pistol-action drilled to fire nails through a fitted barrel sleeve, the kind of kit the silent-ranges built for barn-clearance work where you don't want a rifle and a knife and you don't have either. Four …`
- Row 222 `weapon_rebar_spear`: `{"description":"Welded rebar spear with a one-shot pipe launcher slung under the haft, the kind of thing the salvage ranges build when they have a stockpile of pipe fittings and a Wednesday afternoon. Three kilos, worth twelve. The spear i…`
- Row 223 `weapon_molotov_thrower`: `{"description":"A short rail-and-rubber-band launcher chambered for a tin-can burn charge, the kind of rig the cellar-defence lines agreed on after the third time someone lost the match. One kilo, worth six. It is the safest molotov rig an…`
- Row 224 `weapon_service_rifle`: `{"description":"An M-pattern service rifle pulled off a sealed armoury rack, with the issued sling still on it and the maintenance schedule still in the stock well. Four kilos, worth seventy-five. The rifle that an army kept. Most of the a…`
- Row 225 `weapon_marksman_rifle`: `{"description":"A heavy barrel, a post-war bedding job, and a 4x scope with a salt-corroded ring. Six kilos, worth ninety. The scope is half its trade value. The bedding is the other half. The bolt-action is the rest.","displayName":"Snipe…`
- Row 226 `weapon_smg`: `{"description":"A civilian-pattern submachine gun in nine-millimetre, three-round burst, the kind of pistol-caliber carbine the regional shops sold before the exchange and that the regional shops do not have anymore. Three kilos, worth fif…`
- Row 227 `weapon_sidearm`: `{"description":"A nine-millimetre duty pistol off a department-issue rack, the kind of sidearm that satisfies a small argument and a long argument and a bar-stool argument. One kilo, worth thirty-five. The holster is the holster. The pisto…`
- Row 228 `weapon_rust_mosin`: `{"description":"A 7.62x54R bolt-action rifle taken out of a barn and cleaned down to the salt-pitted metal. Four kilos, worth twenty-eight. It is ugly. It is loud. It will fire five times without jamming if the bore is clean. The bore is r…`
- Row 229 `weapon_farm_carbine`: `{"description":"A short-barrelled .22 carbine cut down from a walnut-stocked plinker to clear woodlots and chicken coops. Two kilos, worth eighteen. Quiet. The .22 is the quiet the night-watch likes.","displayName":"Farm-Clearing Carbine",…`
- Row 230 `weapon_revolver`: `{"description":"A .357 revolver with the cylinder worn smooth and the gate still tight, the kind of gun that lives in a farmhouse drawer for forty years and works the first time it is pulled out. No magazine to lose, no feed ramp to foul, …`
- Row 231 `weapon_coach_shotgun`: `{"description":"A break-open double gun cut down to knuckle length for doorway work, chambered for the buckshot handloads the reloading benches turn out one shell at a time. Two barrels, two chances, and nothing after that worth standing a…`
- Row 232 `weapon_trail_carbine`: `{"description":"A full-stock carbine in the old rimmed military round, carried the length of the trade roads by people who shoot for a living and reload by lamplight when the tins run low. The recoil talks to the shoulder the whole day it …`
- Row 233 `weapon_battle_rifle`: `{"description":"A heavy service rifle in seven-point-six-two off some garrison rack nobody is coming back to claim, the working parts polished slate-grey by drills nobody counted anymore. Two-round pairs, deliberate, the shot that arrives …`
- Row 234 `weapon_quiet_carbine`: `{"description":"A carbine built around the subsonic handload — the round does its work below the crack of supersonic brass, and the carbine was re-barrelled and tuned until it agreed with it. What it saves the ears it spends in energy: lig…`
- Row 235 `iron_pipe`: `{"description":"A one-metre length of two-inch iron pipe, the kind pulled out of every disassembled railing and rerouted sink the rebuilds tore down. Three quarters of a kilo, worth three. The pipe is the pipe. The pipe does not know what …`
- Row 236 `aluminum_shavings`: `{"description":"A paper bag of fine aluminium shavings sifted off the milling machine at the school shop, the kind used in reloading as a permissive igniter above a standard primer. Fifty grams, worth two. The shavings are the shavings. Th…`
- Row 237 `item_decor_poster_ration`: `{"decorLocalizedMoraleDelta":1.5,"description":"A poster the Civic Council printed before the exchange, kept by a literate survivor who folded it flat in a binder. The numerals are still legible. The chart still encourages the right number…`
- Row 238 `item_decor_poster_warning`: `{"decorLocalizedMoraleDelta":0.8,"description":"A poster about radiation written in a font the bunker can read. It says what the corridor already knew. It says it again. The repetition is the point. The poster will say what the corridor al…`
- Row 239 `item_decor_locomotive_nameplate`: `{"decorLocalizedMoraleDelta":2.0,"description":"A steel rectangle pulled off a derelict locomotive and sanded smooth on the workbench. The locomotive number is still legible. The locomotive itself is somewhere on the southern line, rusted …`
- Row 240 `item_decor_carved_memorial`: `{"decorLocalizedMoraleDelta":2.2,"description":"A wooden rectangle, sanded smooth, with the name of a dead survivor and the year they did not live to see. The carver was the same person who keeps the forge pencil, and the carver remembers …`
- Row 241 `item_decor_chalk_drawing`: `{"decorLocalizedMoraleDelta":1.0,"description":"Four chalked shapes on glued paper. Sun. House. Dog with four legs. Adult with dust on the knees. The drawing is two years old. The chalk did not smudge. The house has the original window cou…`
- Row 242 `item_decor_pressed_flower`: `{"decorLocalizedMoraleDelta":1.2,"description":"A frame the size of a palm, made from scrap palette wood and a pane of plastic pulled off a melted cooler. Inside: a pressed flower whose stalk is shorter than the frame's thickness. The flow…`
- Row 243 `item_decor_medal_civic`: `{"decorLocalizedMoraleDelta":0.6,"description":"A medal pulled from a coat the bunker did not wear before the exchange. The ribbon has frayed. The ribbon colour is one the bureacracy used for years of service. The wearer did not retire, th…`
- Row 244 `item_decor_classroom_chart`: `{"decorLocalizedMoraleDelta":1.4,"description":"The chart the children's teacher drew on the first morning they were asked what the children should learn. The teacher called it The Reading Hour. The chalk does not smudge. The teacher's han…`
- Row 245 `item_decor_signal_log`: `{"decorLocalizedMoraleDelta":1.0,"description":"The cover sheet the radio operator writes dates on. The dates make a year. The year makes a calendar. The calendar on the cover sheet is what the bunker means by 'the second autumn' or 'the t…`
- Row 246 `item_decor_memorial_plaque_generic`: `{"decorLocalizedMoraleDelta":1.6,"description":"A small rectangle of joined pine, sanded and varnished and undecorated. The plaque is set in the memorial wall when the survivor the corridor has lost had no specific heirloom, no specific ki…`
- Row 247 `item_decor_memorial_plaque_carving`: `{"decorLocalizedMoraleDelta":1.8,"description":"A plaque with a shallow carving on the front, made by the forge teacher for a survivor whose last trade was pewter. The carving is of the trade's tool. The trade's tool was carried out by the…`
- Row 248 `item_decor_memorial_plaque_drawing`: `{"decorLocalizedMoraleDelta":2.0,"description":"A plaque with a small drawing framed on the front, made by the children for a survivor whose work the children had watched one winter. The drawing is a chimney. The chimney the survivor rebui…`
- Row 249 `item_decor_trophy_wolf_head`: `{"decorLocalizedMoraleDelta":3.0,"description":"Taxidermied twin heads of an apex steppe predator mounted on charred timbers. A grim testament to survivor vigilance and trapping skill.","displayName":"Two-Headed Steppe Wolf Trophy","empShi…`
- Row 250 `item_decor_trophy_deer_antlers`: `{"decorLocalizedMoraleDelta":2.0,"description":"Sweeping calcified antlers polished and secured to a dark hardwood shield. Evokes memories of open country before the fallout.","displayName":"Wasteland Mule Deer Antlers","empShielded":false…`
- Row 251 `item_decor_trophy_boar_tusks`: `{"decorLocalizedMoraleDelta":3.0,"description":"Curved yellowed tusks banded in brass and bolted to salvage cedar. Displays the brute strength required to bring down irradiated quarry.","displayName":"Razorback Boar Tusks","empShielded":fa…`
- Row 252 `item_decor_trophy_fox_pelt`: `{"decorLocalizedMoraleDelta":2.0,"description":"A thick russet-gray winter coat cured with borax and hung from copper rings. Brings warmth and a touch of comfort to cold bunker walls.","displayName":"Barren Fox Pelt","empShielded":false,"i…`
- Row 253 `item_decor_trophy_beetle_carapace`: `{"decorLocalizedMoraleDelta":2.0,"description":"Iridescent chitin plates cleaned with acid and lacquered against dust. Serves as a trophy and a study in wasteland armor biology.","displayName":"Titan Slag-Back Beetle Carapace","empShielded…`
- Row 254 `item_decor_trophy_molerat_skull`: `{"decorLocalizedMoraleDelta":1.0,"description":"The heavy, reinforced cranium and chisel incisors of a subterranean pest. Warns all who see it of what burrows beneath the concrete.","displayName":"Tessarat Blind Mole-Rat Skull","empShielde…`
- Row 255 `item_decor_trophy_crow_feathers`: `{"decorLocalizedMoraleDelta":1.0,"description":"A dark fan of glossy black-and-silver plumage bound with wire. Reminds observers of the sentinels watching from scorched power poles.","displayName":"Three-Eyed Sentry Crow Feathers","empShie…`
- Row 256 `item_decor_trophy_pheasant_plume`: `{"decorLocalizedMoraleDelta":1.0,"description":"Vibrant ember-tinted tail feathers preserved behind a salvage glass bezel. A rare splash of vivid color amidst the gray ruins.","displayName":"Ash Pheasant Plume","empShielded":false,"id":"it…`
- Row 257 `item_decor_trophy_ash_hound_pelt`: `{"decorLocalizedMoraleDelta":2.0,"description":"A dense cinder-colored hide taken from an ash hound pack hunter. Softened with fat and mounted for warmth.","displayName":"Ash Hound Pelt","empShielded":false,"id":"item_decor_trophy_ash_houn…`
- Row 258 `item_decor_trophy_gulden_wolf`: `{"decorLocalizedMoraleDelta":3.0,"description":"The formidable head and fangs of a dust lynx apex ambush hunter. A sign of rare survival against predator ambush.","displayName":"Apex Dust Lynx Trophy","empShielded":false,"id":"item_decor_t…`
- Row 259 `item_decor_trophy_kestrel_wings`: `{"decorLocalizedMoraleDelta":1.0,"description":"Broad slate-colored wingspan from an iron crow scav-scout mounted on salvage copper plate.","displayName":"Iron Crow Wings Trophy","empShielded":false,"id":"item_decor_trophy_kestrel_wings","…`
- Row 260 `item_comm_codebook_alpha`: `{"description":"A lead-sheathed military cipher ledger recovered from a deep command vault. Contains frequency transposition tables, numeric group keys, and authentication offsets for emergency broadcast networks.","displayName":"Communica…`
- Row 261 `item_logistics_cipher_sheet`: `{"description":"A laminated pre-war inventory code sheet detailing encrypted sub-basement warehousing locations and supply reserve authorization codes.","displayName":"Logistics Cipher Sheet","empShielded":true,"id":"item_logistics_cipher_…`
- Row 262 `item_archive_index_cylinder`: `{"description":"A sealed brass cylinder containing high-density microfilm indexing classified emergency dead-drop bunkers, coordinates, and automated survival protocols.","displayName":"Archive Microfilm Index Cylinder","empShielded":true,…`
- Row 263 `charcoal`: `{"description":"A cloth bag of wood charcoal, the pieces still black and dry enough to burn clean. It is what is left when the fire has taken everything else out of the wood. The archive uses it ground fine for ink. The forge uses it for h…`
- Row 264 `book`: `{"description":"A bound volume with pages that still turn. The cover is gone or the title is no longer legible. It does not matter what it was about. It matters that someone carried it this far and did not burn it for warmth. The words ins…`
- Row 265 `blueprint_roll`: `{"description":"A tube of drafting paper, the kind an engineer carried before the exchange. The blueprint inside is faded but legible — floor plans, elevation marks, a structure that may or may not still stand. The person who carried this …`
- Row 266 `radio_headset`: `{"description":"A padded headset with a wired microphone on a flexible boom. The ear cups are cracked leather over foam that has gone to dust. The cable ends in a connector that fits the bunker's intercom panel. It still transmits. The per…`
- Row 267 `service_pistol`: `{"description":"A nine-millimetre sidearm in a worn holster. The bluing is gone from the slide and the grip tape is peeling. Seven rounds in the magazine. The safety still clicks. The person who carried this one kept it oiled, kept it load…`
- Row 268 `surgical_mask`: `{"description":"A folded surgical mask in a torn paper wrapper. The elastic ear loops are still intact. It will not stop radiation. It will stop the things that come out of a patient's lungs when the lungs are still working. The person who…`
- Row 269 `scalpel`: `{"description":"A surgical scalpel in a sealed blister pack, the blade still bright under the foil. Number ten blade, the kind used for initial incisions. The handle is lightweight alloy, the kind that does not corrode. Someone sterilized …`
- Row 270 `forceps`: `{"description":"A pair of surgical forceps, stainless steel, the serrated tips still aligned. The hinge moves without grit. They are the kind used to clamp and to hold, to pull what should not remain inside a body. They have been cleaned a…`
- Row 271 `surgical_suture`: `{"description":"A sealed packet of absorbable suture material, the kind that dissolves in tissue over three weeks. The needle is swaged, the thread still coated. Five packets to a pack. The expiry date is illegible but the seal has not bee…`
- Row 272 `dog_tags`: `{"description":"Two aluminium tags on a beaded chain, the stamped letters still legible. A name, a number, a blood type, a faith that is no longer practised. The chain was cut, not broken. Someone removed these from the person they belonge…`
- Row 273 `concrete_rubble`: `{"description":"A sack of broken concrete, the rebar stubs bent and rusted at the edges. Pulled from a collapsed wall or a shattered overpass, it is the kind of material that was a building yesterday and is fill today. Heavy. Abrasive. The…`
- Row 274 `chemical_solvent`: `{"description":"A sealed glass bottle of industrial solvent, the label long gone but the contents still sharp enough to sting the eyes through the cap. Pulled from a laboratory shelf or a cleaning supply cabinet that someone forgot to loot…`
- Row 275 `empty_toner_cartridge`: `{"description":"A plastic toner cartridge from a laser printer that no longer works, the drum scratched and the powder compartment empty. It is light, the size of a shoe, and it still smells faintly of carbon. The residual toner dust insid…`
- Row 276 `mineral_chunk`: `{"description":"A fist-sized piece of mineral broken from a exposed stratum or a collapsed mine shaft. The colour runs from rust-red to chalk-white depending on what it is. Heavy for its size, it can be ground into a coarse pigment or diss…`
- Row 277 `blood_sample`: `{"description":"A sealed glass vial of blood, the type written on a strip of tape that is starting to peel. Taken from a medical supply cabinet or a field kit that someone left behind. It is not useful as medicine in this form. It is usefu…`
- Row 278 `organic_residue`: `{"description":"A jar of dark organic residue scraped from a compost heap, a drainage channel, or the inside of a water filter that has been running too long. It smells like earth and something older. The archive boils it down for a sepia-…`
- Row 279 `wedding_ring`: `{"description":"A simple gold band, scratched by years of manual labor. It has no gem, just a faint inscription inside worn down by friction against skin: 'Always, M.'","displayName":"Wedding Ring","empShielded":true,"id":"wedding_ring","s…`
- Row 280 `family_photograph`: `{"description":"A color print from before the war, corners softened by grease and handling. A family sits together on a porch in summer sunlight that feels impossible now.","displayName":"Family Photograph","empShielded":true,"id":"family_…`
- Row 281 `worn_photograph`: `{"description":"A black-and-white portrait creased down the middle. One figure has been touched so many times that the emulsion has worn away to white paper.","displayName":"Worn Photograph","empShielded":true,"id":"worn_photograph","stack…`
- Row 282 `photo_album`: `{"description":"A velvet-covered album with half its pages torn out. The remaining prints show birthday parties, snowy streets, and houses with unboarded windows.","displayName":"Photo Album","empShielded":true,"id":"photo_album","stackMax…`
- Row 283 `childs_drawing`: `{"description":"A piece of heavy construction paper showing a yellow sun, a blue house with curly smoke, and three smiling stick figures holding hands.","displayName":"Child's Crayon Drawing","empShielded":true,"id":"childs_drawing","stack…`
- Row 284 `teddy_bear`: `{"description":"A stuffed bear with one button eye and a patched ear. The fur smells of attic dust and old laundry detergent.","displayName":"Teddy Bear","empShielded":true,"id":"teddy_bear","stackMax":1,"tradeValue":8,"type":"Component","…`
- Row 285 `recipe_card`: `{"description":"An index card stained with butter and vanilla, bearing handwritten notes for rye bread. In the margin, in shaky pencil: 'When flour is short, add boiled potato.'","displayName":"Recipe Card","empShielded":true,"id":"recipe_…`
- Row 286 `recipe_tin`: `{"description":"A tin box with painted blue cornflowers, packed with handwritten cards spanning forty years of meals, family birthdays, and wartime ration substitutions.","displayName":"Mother's Recipe Tin","empShielded":true,"id":"recipe_…`
- Row 287 `childs_mitten`: `{"description":"A single red wool mitten with a safety pin still attached to the cuff where it used to clip to a coat sleeve. The wool is coarse but hand-knit.","displayName":"Child's Mitten","empShielded":true,"id":"childs_mitten","stackM…`
- Row 288 `childs_red_scarf`: `{"description":"A long red scarf knitted with uneven stitches, frayed at the tassels. Bright enough to spot across a crowded pre-war train platform.","displayName":"Child's Red Scarf","empShielded":true,"id":"childs_red_scarf","stackMax":1…`
- Row 289 `engraved_lighter`: `{"description":"A heavy petrol lighter with an etched regimental crest on the lid. The flint still sparks and the wick is charred black from lighting wood stoves.","displayName":"Engraved Lighter","empShielded":true,"id":"engraved_lighter"…`
- Row 290 `tarnished_medal`: `{"description":"A bronze military medal in a satin-lined presentation box. The ribbon is faded olive drab, and the clasp has never been pinned to a dress uniform.","displayName":"Tarnished Service Medal","empShielded":true,"id":"tarnished_…`
- Row 291 `pocket_notebook`: `{"description":"A black oilcloth notebook filled with pencil entries: grocery tallies, tire pressure notes, and promises to call relatives that abruptly end on Day Zero.","displayName":"Pocket Notebook","empShielded":true,"id":"pocket_note…`
- Row 292 `family_apartment_key`: `{"description":"A brass mortise key stamped '4B' on a worn leather fob. The apartment building it opened collapsed into rubble decades ago, but the key remains polished.","displayName":"Family Apartment Key","empShielded":true,"id":"family…`
- Row 293 `foreman_whistle`: `{"description":"A heavy brass whistle with a dried pea inside that still rattles. Used to signal shift changes and blast evacuations across the municipal foundry floor.","displayName":"Foreman's Whistle","empShielded":true,"id":"foreman_wh…`
- Row 294 `nurse_fob_watch`: `{"description":"An upside-down pinned watch with a red seconds hand for taking pulse readings. The crystal is cracked, but the spring mechanism still ticks cleanly.","displayName":"Nurse's Fob Watch","empShielded":true,"id":"nurse_fob_watc…`
- Row 295 `tarnished_pocket_watch`: `{"description":"A silver pocket watch that stopped at four minutes past eight. The back cover is engraved with initials and an anniversary date from before the Exchange.","displayName":"Tarnished Pocket Watch","empShielded":true,"id":"tarn…`
- Row 296 `machinist_caliper`: `{"description":"A stainless steel vernier caliper in a wooden case. The thumb roller moves smoothly, measuring thousandths of an inch with uncompromising pre-war precision.","displayName":"Machinist's Caliper","empShielded":true,"id":"mach…`
- Row 297 `item_gauge_block_set`: `{"description":"A wooden case of hardened steel gauge blocks, wrung together to build reference lengths. The contact faces are still flat enough for shop certification work if the bench stays quiet.","displayName":"Gauge Block Set","empShi…`
- Row 298 `item_optical_flat`: `{"description":"A fused-quartz disk ground and polished for interference fringes. Used to check surface flatness on precision parts. Drop it once and the certificate is gone.","displayName":"Optical Flat","empShielded":true,"id":"item_opti…`
- Row 299 `item_surface_plate`: `{"description":"A thick granite plate with a scraped reference face. The datum everything else is measured against. Too heavy to move casually; too valuable to leave undamped during excavation.","displayName":"Granite Surface Plate","empSh…`
- Row 300 `item_micrometer_set`: `{"description":"Outside micrometers in a felt-lined tin, zeroed against a reference bar. Good enough for field and shop grades when the caliper alone is not.","displayName":"Micrometer Set","empShielded":true,"id":"item_micrometer_set","st…`
- Row 301 `engineers_slide_rule`: `{"description":"A bamboo and celluloid calculating rule in a leather sheath. Used to calculate structural loads, radiation half-lives, and generator outputs without power.","displayName":"Engineer's Slide Rule","empShielded":true,"id":"eng…`
- Row 302 `miners_tag`: `{"description":"A numbered brass token stamped 'Shift 3 / #142'. Miners hung them on the lamp-house board on their way underground so the surface knew who was below.","displayName":"Miner's Brass Tag","empShielded":true,"id":"miners_tag","…`
- Row 303 `farm_ledger`: `{"description":"A cloth-bound ledger tracking bushels of winter wheat, rainfall averages, and livestock weights across twenty seasons of peacetime soil.","displayName":"Farm Harvest Ledger","empShielded":true,"id":"farm_ledger","stackMax":…`
- Row 304 `tram_punch`: `{"description":"A nickel-plated ticket punch that cuts a distinctive star-shaped hole. The lever still snaps with crisp, practiced rhythm.","displayName":"Tram Conductor's Punch","empShielded":true,"id":"tram_punch","stackMax":1,"tradeValu…`
- Row 305 `mechanic_gloves`: `{"description":"Heavy split-cowhide gloves stained black with motor oil and diesel. The right index finger has been mended three times with copper wire.","displayName":"Mechanic's Work Gloves","empShielded":true,"id":"mechanic_gloves","sta…`
- Row 306 `teachers_stamp`: `{"description":"A rubber-faced wooden stamp reading 'EXCELLENT WORK' in block capitals, with dried red ink encrusted in the grain of the handle.","displayName":"Teacher's Wooden Stamp","empShielded":true,"id":"teachers_stamp","stackMax":1,…`
- Row 307 `bus_ticket`: `{"description":"A thin slip of printed paper for the 7:15 AM crosstown route, punched twice and folded into a tiny square. The date is forty-eight hours before Day Zero.","displayName":"Worn Bus Ticket","empShielded":true,"id":"bus_ticket"…`
- Row 308 `shopping_list`: `{"description":"The back of a gas receipt listing: 'Milk, oats, 1/2 lb butter, coffee, birthday candles.' Four of the five items are crossed off with firm pencil strokes.","displayName":"Pencil Shopping List","empShielded":true,"id":"shopp…`
- Row 309 `enamel_mug`: `{"description":"A speckled blue enamel tin mug, chipped down to black iron along the rim where someone's teeth rested while drinking tea in the morning.","displayName":"Enamel Camp Mug","empShielded":true,"id":"enamel_mug","stackMax":4,"tr…`
- Row 310 `cheap_comb`: `{"description":"A brown pocket comb with three missing teeth. Pulled from a bathroom medicine cabinet that survived the blast wave intact.","displayName":"Cheap Plastic Comb","empShielded":true,"id":"cheap_comb","stackMax":10,"tradeValue":…`
- Row 311 `matchbook`: `{"description":"A cardboard matchbook advertising 'Silver Star Diner — Open 24 Hours'. Twelve cardboard matches remain, unspent.","displayName":"Diner Matchbook","empShielded":true,"id":"matchbook","stackMax":10,"tradeValue":3,"type":"Comp…`
- Row 312 `creased_receipt`: `{"description":"A purple-ink grocery receipt for three cans of peaches and a loaf of bread, dated an ordinary Tuesday afternoon before the sirens.","displayName":"Creased Cash Receipt","empShielded":true,"id":"creased_receipt","stackMax":2…`
- Row 313 `keyring_charm`: `{"description":"A small cast-brass clover souvenir charm attached to a split ring. Smooth to the touch from years in a jacket pocket.","displayName":"Brass Keyring Charm","empShielded":true,"id":"keyring_charm","stackMax":5,"tradeValue":4,…`
- Row 314 `midwife_satchel`: `{"description":"A stiff leather doctor's satchel containing clean clamps, swaddling cloths, and an umbilical scissor oiled and wrapped in linen.","displayName":"Midwife's Medical Satchel","empShielded":true,"id":"midwife_satchel","stackMax…`
- Row 315 `lighthouse_logbook`: `{"description":"A heavy canvas-bound ledger recording coastal weather, tides, and the final entries tracking navigational lights going dark one by one across the gulf.","displayName":"Lighthouse Keeper's Logbook","empShielded":true,"id":"l…`
- Row 316 `train_ticket_book`: `{"description":"A pad of numbered emergency transit coupons marked 'Sector Priority Evacuation'. Most were never torn from the stub.","displayName":"Evacuation Train Ticket Book","empShielded":true,"id":"train_ticket_book","stackMax":1,"tr…`
- Row 317 `civil_defense_radio`: `{"description":"A transistor radio encased in bright yellow impact plastic, tuned to the emergency broadcast frequency that fell silent thirty years ago.","displayName":"Civil Defense Pocket Radio","empShielded":true,"id":"civil_defense_ra…`
- Row 318 `dog_tags_military`: `{"description":"A pair of notched stainless steel tags on a beaded chain, stamped with an army serial number and blood type.","displayName":"Military Dog Tags","empShielded":true,"id":"dog_tags_military","stackMax":5,"tradeValue":10,"type"…`
- Row 319 `dog_tags_personal`: `{"description":"A set of military tags with a small gold religious medal taped securely to the back of the primary tag.","displayName":"Personal Dog Tags","empShielded":true,"id":"dog_tags_personal","stackMax":1,"tradeValue":15,"type":"Com…`
- Row 320 `undelivered_mail`: `{"description":"A bundle of postmarked letters tied with hemp twine, never sorted into mailboxes on the morning the missiles launched.","displayName":"Undelivered Mail","empShielded":true,"id":"undelivered_mail","stackMax":5,"tradeValue":8…`
- Row 321 `silver_scalpel`: `{"description":"A solid steel surgical scalpel with a balanced handle and interchangeable blade slot, sterilized and preserved in oiled chamois.","displayName":"Silver Surgical Scalpel","empShielded":true,"id":"silver_scalpel","stackMax":2…`
- Row 322 `worn_stethoscope`: `{"description":"A medical stethoscope with flexible rubber tubing and a cold chrome chestpiece that has listened to hundreds of human chests.","displayName":"Worn Stethoscope","empShielded":true,"id":"worn_stethoscope","stackMax":1,"tradeV…`
- Row 323 `stethoscope`: `{"description":"A clinical acoustic stethoscope in good working condition, essential for triage and pulse diagnostics in the infirmary.","displayName":"Stethoscope","empShielded":true,"id":"stethoscope","stackMax":1,"tradeValue":24,"type":…`
- Row 324 `family_heirloom_seeds`: `{"description":"A wax-sealed paper packet containing twenty-five unhybridized tomato seeds kept dry and shielded from fallout radiation.","displayName":"Heirloom Seed Packet","empShielded":true,"id":"family_heirloom_seeds","stackMax":5,"tr…`
- Row 325 `field_dressing_kit`: `{"description":"A sealed military trauma pouch with sterile pressure bandages, tourniquet, and antiseptic wipes packed for combat triage.","displayName":"Field Dressing Kit","empShielded":false,"id":"field_dressing_kit","stackMax":5,"trade…`
- Row 326 `item_foundry_roof_armor_plate`: `{"description":"Thickened sky-layer armor plate designed to withstand falling structural debris and acid rain fallout.","displayName":"Heavy Roof-Armor Plate","durability":100,"empShielded":true,"id":"item_foundry_roof_armor_plate","stackM…`
- Row 327 `item_foundry_shoring_bracket`: `{"description":"A heavy cast iron collar machined to brace tunnel sections and shaft walls against ground pressure. The shoulders are drilled for anchor bolts and the surface is rough from the sand mold. The Silent Foundry delivers these i…`
- Row 328 `item_foundry_blast_fitting`: `{"description":"A precision-cast alloy bushing and hinge bracket for pressure-rated doors and airlock bulkheads. The tolerances are tight enough that the threads are smooth to the touch. The Foundry makes these to specification from a tech…`
- Row 329 `item_foundry_reinforcement_shoe`: `{"description":"An anchor shoe cast to stabilize subsiding foundation columns in lower shelter blocks. The base plate is wide and the collar angle is adjustable with a set screw. The Foundry engraves a lot number into every one so the stru…`
- Row 330 `item_foundry_structural_coupling`: `{"description":"A cast iron sleeve coupling for joining heavy power conduits and steam lines between bunker sectors. The flanges are drilled for six bolts and the bore is machined to accept the standard conduit diameter used in pre-war civ…`
- Row 331 `item_foundry_replacement_die`: `{"description":"A set of high-carbon alloy stamping dies for reshaping and reworking workshop metal stock. The die faces are hardened and ground to mirror finish. When a press or a forming machine loses its die to a broken edge, the machin…`
- Row 332 `item_foundry_drill_blanks`: `{"description":"Hexagonal tool-steel rods for regrinding rock drills and salt-mine augers. The blanks arrive in the Foundry's wrapped paper and the steel is hard enough to scratch a thumbnail. A blunt drill in a salt mine or a borehole cos…`
- Row 333 `item_foundry_crucible_spare`: `{"description":"A refractory-lined shell for replacing worn cupola and smelter crucibles. The lining is compressed and fired ceramic that can tolerate the Foundry's maximum pour temperature for sixty heats before it needs replacing. A crac…`
- Row 334 `item_foundry_press_fitting`: `{"description":"A precision hydraulic collar for pharmaceutical compactors and workshop hydraulic presses. The bore is lapped to a fine finish and the seal groove is cut for an O-ring that must be replaced each time the collar is disassemb…`
- Row 335 `item_foundry_bearing_housing`: `{"description":"A split cast-iron bearing housing for heavy machine tool spindles and conveyor shafting. The bore is bored and line-reamed to accept a standard roller bearing race. The halves are matched, drilled, and pinned so they cannot…`
- Row 336 `item_foundry_furnace_grate`: `{"description":"A sectional cast iron grate for solid-fuel furnaces and boiler fireboxes. The bar spacing is set to let ash fall through without dropping unburned fuel. The Foundry replaces grates on a burn-through schedule: one bad sectio…`
- Row 337 `item_foundry_weather_canister`: `{"description":"A sealed cast cylinder for outdoor instrument housings — barometers, anemometers, and recording thermometers. The Foundry makes these for the weather stations that survived the Exchange, most of which are still transmitting…`
- Row 338 `item_foundry_cast_shot`: `{"description":"Chilled lead-iron defensive shot canister used in automated bunker deadfall traps and perimeter barriers.","displayName":"Heavy Cast Shot (canister)","durability":100,"empShielded":true,"id":"item_foundry_cast_shot","stackM…`
- Row 339 `item_foundry_casing_blanks`: `{"description":"Rough-cast cylindrical billets for machining into custom housing shells and protective casings. The castings come out of the sand mold slightly oversized on all surfaces, waiting for the lathe. The Electrician uses them to …`
- Row 340 `item_seed_hardy_tuber`: `{"description":"Black rutabaga root cuttings with deep dormant eyes. Survives frost and damp soil that would rot ordinary roots.","displayName":"Frost Tuber Eyes","empShielded":true,"id":"item_seed_hardy_tuber","stackMax":15,"tradeValue":7…`
- Row 341 `crop_hardy_tuber`: `{"description":"A knobbled, ash-grey tuber the size of a fist, dense with starch and slightly bitter when eaten raw. Cooked, it softens to a mealy texture that fills the stomach in a way that flour cannot. The Botanist selected this strain…`
- Row 342 `item_seed_ash_grain`: `{"description":"A small cloth packet of ash-grain seed, labeled in the Botanist's handwriting with a lot number and germination date. The seed is a winter-adapted cereal bred from several ancestors and selected over four generations of pos…`
- Row 343 `crop_ash_grain`: `{"description":"A sheaf of coarse-stalked grain with small dense heads, the hull still on. The grain is bitter and needs double boiling to leach out the tannins before it can be eaten comfortably. Made into flour it bakes into a dense, dar…`
- Row 344 `item_seed_biolum_mushroom`: `{"description":"A glass vial of mycelium culture rather than true seed — the Botanist calls it a seed because the concept is easier to communicate. The culture produces the glowing bracket mushroom that grows in the tunnel sections, provid…`
- Row 345 `crop_biolum_mushroom`: `{"description":"Luminescent fungal caps with a mild sedative effect. Harvested for calm-inducing teas and pharma extraction.","displayName":"Phosphor Cap Fungi","empShielded":true,"hungerRestore":5,"id":"crop_biolum_mushroom","moraleEffect…`
- Row 346 `item_seed_nutrient_algae`: `{"description":"A sealed ampoule of algae starter culture in nutrient solution. The Botanist grows it in the UV-lit tank behind the pump room in thin green sheets that can be dried, ground, and added to grain flour to increase the protein …`
- Row 347 `crop_nutrient_algae`: `{"description":"Thick protein-rich green paste skimmed from greenhouse cultivation basins. Bitter but life-sustaining.","displayName":"Nutrient Algae Slurry","empShielded":true,"healthEffect":1,"hungerRestore":15,"id":"crop_nutrient_algae"…`
- Row 348 `item_seed_medicinal_herb`: `{"description":"Rooted herbal cuttings wrapped in damp sphagnum moss. Supplies the shelter dispensary with natural antipyretics.","displayName":"Yarrow & Fever-Bark Cutting","empShielded":true,"id":"item_seed_medicinal_herb","stackMax":15,…`
- Row 349 `crop_medicinal_herb`: `{"description":"Aromatic bundle of harvested fever-bark and dried yarrow leaves. Brewed to reduce fever and cleanse wounds.","displayName":"Medicinal Herb Bundle","empShielded":true,"healthEffect":3,"hungerRestore":2,"id":"crop_medicinal_h…`
- Row 350 `item_seed_leafy_green`: `{"description":"A small paper envelope of mixed leafy vegetable seeds — varieties selected for dense nutrition per square centimeter and fast harvest cycle. The Botanist grows these in the shallowest greenhouse beds and harvests them conti…`
- Row 351 `crop_leafy_green`: `{"description":"Crisp, peppery green leaves packed with essential vitamin C. The primary shelter defense against scurvy.","displayName":"Fresh Winter Cress","empShielded":true,"healthEffect":1,"hungerRestore":8,"id":"crop_leafy_green","mor…`
- Row 352 `item_seed_oilseed`: `{"description":"A packet of cold-adapted oilseed, selected for high oil content per gram of seed. The Botanist presses the harvest in a small screw press the mechanic built from salvaged parts. The oil is pale green and slightly bitter and…`
- Row 353 `crop_oilseed`: `{"description":"A handful of small oily seeds, green-grey and dense. The press yields about thirty percent by weight as oil. What is left after pressing is a high-protein cake that the Botanist mixes into the grain bread to increase its ca…`
- Row 354 `item_seed_cold_legume`: `{"description":"A paper envelope of legume seeds bred for nitrogen fixation in contaminated soil and for production in low-light conditions. The Botanist uses them as a cover crop between grain cycles to restore the greenhouse beds without…`
- Row 355 `item_fermentation_sugar_feedstock`: `{"description":"Crated drums of dense syrup left over from pre-war food processing. Far too concentrated to drink and too valuable to waste: the fermenter converts it into preservation and industrial resources. Gooey, heavy, and sealed aga…`
- Row 356 `item_fermentation_starch_feedstock`: `{"description":"Compressed bricks of starchy meal milled from surplus tubers and grain sweepings. Nobody wants to eat them straight, but the fermentation reactor turns them into something the shelter can actually use.","displayName":"Press…`
- Row 357 `item_fermentation_culture_starter`: `{"description":"A sealed vial of starter medium. The label says only that it is inert until introduced into the reactor's prepared environment, and that each vial is good for exactly one batch. The rest of the label dissolved long ago.","d…`
- Row 358 `item_fermentation_filter_module`: `{"description":"A replaceable breather and scrubber cartridge for the fermentation reactor's gas and effluent lines. Pressure-packed filtration medium inside a scored steel canister. One module keeps the reactor's air path clean until it c…`
- Row 359 `item_fermentation_service_kit`: `{"description":"Aboard-shelf kit of abstract maintenance supplies for the fermentation reactor: replacement seals, scrub pads, calibration shims, and a bound checklist. The work itself is wearisome, not dangerous — the reactor never holds …`
- Row 360 `item_fermentation_preservation_concentrate`: `{"description":"A shelf-stable draw of concentrated preservation medium from the fermentation reactor. A little of it in the curing brine stretches meat and garden surplus far past their natural span. The kitchen keeps its own ledger of wh…`
- Row 361 `item_fermentation_cleaning_reagent`: `{"description":"A mild reactive draw from the fermenter, bottled for the workshop. Eats through rust and corrosion without touching sound metal underneath. The machinists use it to reclaim seized scrap for the foundry's feed stock.","displ…`
- Row 362 `item_fermented_organic_acid_carboy`: `{"description":"A heavy glass carboy of cloudy, sharp-smelling acid drawn off the fermentation reactor. Traders pay well for it; the chemists talk about what it might wash out of contaminated earth, someday, if anyone ever builds the rig f…`
- Row 363 `item_fermentation_waste_pomace`: `{"description":"The pressed-out remainder of a fermentation batch: spent solids, dead medium, and filter leavings sealed into a drum. Mostly useless — it can be buried, traded for a fraction, or held against some future composting use.","d…`
- Row 364 `item_crop_waste`: `{"description":"Cut stalks, husks, and leaf rot stripped from the greenhouse beds after a harvest. Heavy, bulky, and nearly worthless as food. It burns poorly and composts slowly, but there is more of it than anything else the shelter thro…`
- Row 365 `item_biofuel_low_grade`: `{"description":"A canister of cloudy, watery fuel drawn off a half-finished cellulose run. It burns, after a fashion. Generators cough on it and engines wear fast, but in a cold week nobody asks questions about the color.","displayName":"L…`
- Row 366 `item_biofuel_generator_grade`: `{"description":"Steady-burning fuel settled and strained in the still bay. Not clean enough for a long drive, but the grid takes it without complaint, and the price of a quiet generator is measured in more than cans.","displayName":"Genera…`
- Row 367 `item_biofuel_high_grade`: `{"description":"Clear, dense fuel from a patient, well-tended run. It costs power, labor, and cartridges that are hard to replace. Engines run long on it, and traders who know the difference pay for it.","displayName":"High-Grade Biofuel C…`
- Row 368 `item_separation_media_cartridge`: `{"description":"A sealed cartridge of packing and filter media for the still bay's settling columns. Each one swells, clogs, and dies a little more with every batch. When the last cartridge goes, the runs go cloudy.","displayName":"Separat…`
- Row 369 `item_ecm_jammer_module`: `{"description":"A rack of decoy transmitters and noise boxes pulled from a dead broadcast van. Bolted to a vehicle it can make a hostile scanner argue with itself about where the truck actually is. It runs hot, eats power, and every minute…`
- Row 370 `item_machined_blank_small`: `{"description":"A measured steel blank, faced and trued on the lathe, waiting for its final shape. Half the work is already in it. The other half wants a machine the shelter almost has.","displayName":"Machined Blank, Small","id":"item_mac…`
- Row 371 `item_machined_blank_medium`: `{"description":"A heavier steel blank, bored and trued, staged for internal work. Good steel is scarce; a true blank is scarcer. The broaching station turns these into parts nothing else in the wasteland can make.","displayName":"Machined …`
- Row 372 `item_internal_spline_hub`: `{"description":"A steel hub with teeth cut into its bore, true to a tolerance measured in wisps of nothing. Pumps, gearboxes, and couplings that run on parts like this simply keep running. Everything else clatters, heats, and dies.","displ…`
- Row 373 `item_keyed_actuator_collar`: `{"description":"A small collar with a cut keyway, made to lock a shaft and a wheel into one will. Cheap-looking and exact. Machinery built with them stops eating itself.","displayName":"Keyed Actuator Collar","id":"item_keyed_actuator_coll…`
- Row 374 `item_cutting_fluid_canister`: `{"description":"A canister of foul-smelling cutting oil, rendered and strained in the workshop. It keeps tools cool and edges alive through long cuts. Without it, steel burns and broaches snap.","displayName":"Cutting Fluid Canister","id":…`
- Row 375 `item_hydraulic_ram_assembly`: `{"description":"A heavy ram assembly pulled intact from a dead grain press. It is the muscle a broaching station needs to push a tool through steel without shuddering. Finding one intact is luck. Keeping it sealed is discipline.","displayN…`
- Row 376 `item_fog_mesh_roll`: `{"description":"A roll of fine, dark mesh woven to catch what the air carries. Strung on a ridge in the right weather, it weeps water all night. Strung wrong, or in the wrong place, it catches wind and nothing else.","displayName":"Fog Mes…`
- Row 377 `item_powered_mist_assist_module`: `{"description":"A sealed collector unit that pulls moisture out of heavy air faster than the wind alone manages. It hums all night, eats power, and fouls quickly in ash-laden fog. Worth it only where the fog is honest and frequent.","displ…`
- Row 378 `item_reinforced_support_cable`: `{"description":"Guy-wire stock, double-stranded and crimped at the ends. It holds fog nets against gusts that turn loose mesh into rags. A storm finds every lazy tension job eventually.","displayName":"Reinforced Support Cable","id":"item_…`
- Row 379 `item_runflat_insert_utility`: `{"description":"A dense salvaged insert pressed into a light truck tyre. It keeps the wheel rolling after a puncture but adds unsprung mass and drag. Fitting it takes a workshop and patience.","displayName":"Utility Run-Flat Insert","id":"…`
- Row 380 `item_runflat_insert_reinforced`: `{"description":"A heavy laminated insert built for long hauls. Tolerates wire and scrap better than the utility insert, at a real fuel and heat cost.","displayName":"Reinforced Run-Flat Insert","id":"item_runflat_insert_reinforced","stackM…`
- Row 381 `item_rim_bead_kit`: `{"description":"Seals, beadlock wedges, and a seating tool. The parts that keep a run-flat seated on the rim when the sidewall takes a hit.","displayName":"Rim & Bead Kit","id":"item_rim_bead_kit","stackMax":8,"tradeValue":18,"type":"Mater…`
- Row 382 `item_armored_beadlock_set`: `{"description":"Matched beadlock rims with armored sidewall plates. Nearly shrugs off road debris; runs hot and is hard to field-repair.","displayName":"Armored Beadlock Set","id":"item_armored_beadlock_set","stackMax":4,"tradeValue":42,"t…`
- Row 383 `item_runflat_balancing_kit`: `{"description":"Weights and a static balancer for a run-flat wheel set. Without balancing, a heavy insert shakes the whole vehicle apart.","displayName":"Run-Flat Balancing Kit","id":"item_runflat_balancing_kit","stackMax":10,"tradeValue":…`
- Row 384 `crop_cold_legume`: `{"description":"Dense, starchy legumes packed with vegetable protein. Essential for hearty survival potages and brines.","displayName":"Harvested Iron Peas","empShielded":true,"hungerRestore":16,"id":"crop_cold_legume","moraleEffect":1,"st…`
- Row 385 `item_honey_pot`: `{"description":"A sealed clay pot of raw golden comb honey from the greenhouse apiary. High calorie density and natural morale booster.","displayName":"Raw Comb Honey (pot)","empShielded":true,"healthEffect":1,"hungerRestore":18,"id":"item…`
- Row 386 `item_beeswax_block`: `{"description":"A dense yellow block of rendered beeswax. Used for waterproofing textiles, candle casting, and sealing preservation jars.","displayName":"Purified Beeswax Block","empShielded":true,"id":"item_beeswax_block","stackMax":15,"t…`
- Row 387 `item_raw_propolis`: `{"description":"Sticky antimicrobial resin harvested from beehive frames. Formulated into salves to treat burns and infected wounds.","displayName":"Raw Propolis Resin","empShielded":true,"healthEffect":4,"id":"item_raw_propolis","moraleEf…`
- Row 388 `item_mead_must_base`: `{"description":"Strained washings of honey combs and clean water prepared for crock fermentation into celebratory mead.","displayName":"Honey Must Fermentation Base","empShielded":true,"id":"item_mead_must_base","stackMax":8,"tradeValue":2…`
- Row 389 `item_preservation_salt`: `{"description":"Crushed and graded rock salt from subterranean veins. Essential for curing meats, brining vegetables, and curing hides.","displayName":"Coarse Preservation Salt","empShielded":true,"id":"item_preservation_salt","stackMax":3…`
- Row 390 `item_trade_salt_sack`: `{"description":"A heavy 25kg burlap sack of weighed halite crystals stamped by the weigh-hut. Universal inland barter currency.","displayName":"Standard Trade Salt Sack","empShielded":true,"id":"item_trade_salt_sack","stackMax":4,"tradeVal…`
- Row 391 `item_medical_saline_salt`: `{"description":"Recrystallized medical-grade sodium chloride used in sterile IV solutions, wound washes, and burn irrigation.","displayName":"High-Purity Saline Salt","empShielded":true,"id":"item_medical_saline_salt","stackMax":20,"tradeV…`
- Row 392 `item_pickled_tubers`: `{"description":"Crisp sliced tubers cured in saline brine and wild mustard seed. Stable for months in pantry storage.","displayName":"Pickled Greenhouse Tubers","empShielded":true,"hungerRestore":14,"id":"item_pickled_tubers","moraleEffect…`
- Row 393 `item_dried_mushrooms`: `{"description":"Bracket mushrooms from the tunnel beds, sliced thin and dried over the boiler exhaust. They taste of umami and a faint earthiness that is either the substrate or the air in the lower corridors — nobody is entirely sure. Reh…`
- Row 394 `item_smoked_meat`: `{"description":"Strips of surface-trapped animal protein, smoked over chips of compressed peat and birch bark in the smokehouse behind the generator room. The smoking is long and low, enough to dry the meat to a dark leather that keeps for…`
- Row 395 `item_canned_grain_stew`: `{"description":"Boiled ash-grain and tuber stew sealed in sanitized tin containers. Maximum shelf life during deep winter.","displayName":"Sealed Ash-Grain Potage","empShielded":true,"hungerRestore":28,"id":"item_canned_grain_stew","morale…`
- Row 396 `item_salted_meat`: `{"description":"Dense strips of meat packed in coarse halite salt. Tough to chew unless soaked, but virtually impervious to decay.","displayName":"Dry-Salted Flesh Strips","empShielded":true,"hungerRestore":20,"id":"item_salted_meat","mora…`
- Row 397 `item_fat_confit`: `{"description":"Animal fat, rendered clean and used to slow-cook and preserve protein strips submerged in the fat itself. The result stores at cellar temperature for three weeks and provides both the protein and the fat in one dense servin…`
- Row 398 `item_fermented_sauerkraut`: `{"description":"Shredded greenhouse cabbage fermented in salt brine in a sealed ceramic crock. The fermentation produces lactic acid that preserves the cabbage and creates a dense vitamin C source at almost no fuel cost. The smell permeate…`
- Row 399 `item_honey_preserved_pulp`: `{"description":"Boiled fruit mash preserved in raw honey syrup. A rare sweet treat that lifts morale across the shelter.","displayName":"Honey-Glazed Fruit Mash","empShielded":true,"hungerRestore":16,"id":"item_honey_preserved_pulp","moral…`
- Row 400 `item_dried_herb_packets`: `{"description":"Sanitized packets of dried yarrow and fever-bark. Infused in hot water for fever relief and soothing warmth.","displayName":"Dried Herbal Infusion Packets","empShielded":true,"healthEffect":3,"id":"item_dried_herb_packets",…`
- Row 401 `item_brined_legume_mash`: `{"description":"Salt-brined pea paste in sealed stoneware crocks. High vegetable protein source when meat is scarce.","displayName":"Brined Iron-Pea Paste","empShielded":true,"hungerRestore":22,"id":"item_brined_legume_mash","moraleEffect"…`
- Row 402 `item_water_filter_advanced`: `{"description":"Multi-stage sintered ceramic filter cartridge with silver impregnation. Strips 99.9% of suspended radioactive particulate from tainted water.","displayName":"Advanced Ceramic Water Filter","empShielded":true,"id":"item_wate…`
- Row 403 `item_radiation_shielding_panel`: `{"description":"Composite panel of borated polymer and lead foil laminate for lining bunker bulkheads and medical ward walls against gamma wash.","displayName":"Borated Radiation Shielding Panel","empShielded":true,"id":"item_radiation_shi…`
- Row 404 `item_gas_mask_improved`: `{"description":"A military respirator with an upgraded seal gasket and an additional over-pressure valve that prevents fogging. The original mask was designed for chemical protection and was not rated for the combination of particulate and…`
- Row 405 `item_solar_inverter`: `{"description":"Heavy-duty 24V inverter module converting erratic DC photovoltaic output into clean AC power for sensitive bunker electronics.","displayName":"Pure Sine Solar Inverter","empShielded":true,"id":"item_solar_inverter","stackMa…`
- Row 406 `item_radio_cipher_rotor`: `{"description":"Precision brass rotor from a pre-war encryption transceiver. Allows automated frequency-hopping and cryptographically secure communications.","displayName":"Electromechanical Cipher Rotor","empShielded":true,"id":"item_radi…`
- Row 407 `item_air_filter_hepa_hospital`: `{"description":"A pleated glass-fiber HEPA filter cartridge for bunker intake fans. It traps particles down to 0.3 microns — fine enough to catch ash and fallout dust, not fine enough to catch vapors. The gray pleating turns black in a mon…`
- Row 408 `item_surgical_kit`: `{"description":"Autoclaved surgical toolkit containing precision scalpels, titanium hemostats, vascular clamps, and sterile needle drivers.","displayName":"Sterile Field Surgical Kit","empShielded":true,"id":"item_surgical_kit","stackMax":…`
- Row 409 `item_reagent_clean`: `{"description":"Distilled lab-grade organic solvent and chemical precursor for synthesizing medical antibiotics and chelation compounds.","displayName":"Ultra-Pure Chemical Reagent","empShielded":true,"id":"item_reagent_clean","stackMax":1…`
- Row 410 `item_diving_suit_vulcanized`: `{"description":"Heavy pressure-sealed rubberized diving suit with brass helmet collar for deep submerged maritime salvage runs.","displayName":"Vulcanized Heavy Diving Suit","empShielded":true,"id":"item_diving_suit_vulcanized","stackMax":…`
- Row 411 `item_cloud_seeding_canister`: `{"description":"Pressurized pyrotechnic canister containing silver iodide flare compound to induce atmospheric precipitation over fallout plumes.","displayName":"Silver-Iodide Cloud Seeding Canister","empShielded":true,"id":"item_cloud_see…`
- Row 412 `item_seismic_detector`: `{"description":"High-sensitivity ground vibration probe for detecting subterranean structural shifts, mining cave-ins, and surface raider convoys.","displayName":"Piezoelectric Seismic Geophone","empShielded":true,"id":"item_seismic_detect…`
- Row 413 `item_field_guide_annotated`: `{"description":"Thick waterproof field notebook documenting mutation phenotypes, edible flora, and hazard evasion protocols.","displayName":"Annotated Wasteland Field Guide","empShielded":true,"id":"item_field_guide_annotated","stackMax":1…`
- Row 414 `item_thermal_lance`: `{"description":"High-temperature cutting lance rig utilizing pressurized oxygen and burning magnesium rods to melt through blast doors.","displayName":"Magnesium Thermal Breaching Lance","empShielded":true,"id":"item_thermal_lance","stackM…`
- Row 415 `item_sentry_targeting_chip`: `{"description":"A replacement logic board for the automated sentry turret's targeting system. The original chip failed to distinguish between threat and non-threat at ranges under fifteen meters, which became a problem. The replacement chi…`
- Row 416 `item_dosimeter_calibrated`: `{"description":"Miniaturized semiconductor gamma detector with real-time digital dose rate readouts and cumulative ledger logging.","displayName":"Calibrated Solid-State Dosimeter","empShielded":true,"id":"item_dosimeter_calibrated","stack…`
- Row 417 `item_radio_vacuum_tube`: `{"description":"Militarized glass triode tube engineered for ultra-low noise amplification in harsh electromagnetic environments.","displayName":"Low-Noise Radio Vacuum Tube","empShielded":true,"id":"item_radio_vacuum_tube","stackMax":10,"…`
- Row 418 `item_battery_reconditioned`: `{"description":"A lead-acid battery that has been drained, flushed, and recharged with fresh electrolyte to restore partial capacity. The original cells had sulfated plates that were ground back with abrasive compound. The reconditioned ba…`
- Row 419 `item_hydroponic_nutrients`: `{"description":"Chelated mineral salts and micro-nutrients formulated to accelerate root growth in closed-loop bunker trays.","displayName":"Concentrated Hydroponic Nutrient Pack","empShielded":true,"id":"item_hydroponic_nutrients","stackM…`
- Row 420 `item_military_radio_module`: `{"description":"A sealed transceiver module from a military communication system, still in its rubberized shock housing. The module covers the tactical frequency bands and includes a built-in encryption chip that no longer has anyone to en…`
- Row 421 `item_radar_display_tube`: `{"description":"Phosphor CRT display tube from an airport Doppler radar console, capable of plotting distant atmospheric storm tracks.","displayName":"Cathode Radar Display Tube","empShielded":true,"id":"item_radar_display_tube","stackMax"…`
- Row 422 `item_hydraulic_actuator`: `{"description":"High-pressure fluid actuator rated for 5-ton force output in heavy machinery, deep-well pumps, and exo-frames.","displayName":"Heavy Hydraulic Linear Actuator","empShielded":true,"id":"item_hydraulic_actuator","stackMax":4,…`
- Row 423 `item_iff_beacon`: `{"description":"Encrypted friend-or-foe transponder emitting authenticated beacon responses to neutralize automated sentry guns.","displayName":"IFF Transponder Beacon","empShielded":true,"id":"item_iff_beacon","stackMax":3,"tradeValue":85…`
- Row 424 `item_cbrn_cartridge`: `{"description":"Military respirator cartridge offering Level-A protection against persistent biological aerosols and nerve agents.","displayName":"Carbon-Nanotube CBRN Cartridge","empShielded":true,"id":"item_cbrn_cartridge","stackMax":8,"…`
- Row 425 `item_surgical_arm_servo`: `{"description":"A motorized joint actuator salvaged from a robotic surgical assistant. The servo provides precise, tremor-free movement and is rated for continuous operation over a six-hour procedure. The surgical assistant it came from wa…`
- Row 426 `item_vacuum_seal_canner`: `{"description":"Manual double-seam can sealer and pressure vessel for hermetically sealing food tins without electricity.","displayName":"Mechanical Vacuum-Seal Canner","empShielded":true,"id":"item_vacuum_seal_canner","stackMax":1,"tradeV…`
- Row 427 `trap_improvised_wire`: `{"description":"A twist of copper wire shaped into a loose loop and pegged to the ground. The poorest survivor's option, better than doing nothing. It breaks often, catches little, and costs almost nothing to replace.","displayName":"Impro…`
- Row 428 `trap_box`: `{"description":"A wooden box with a gravity door, triggered by a treadle inside. Bulky, dependable, and shelter-craftable. The workhorse once the bunker can afford one.","displayName":"Box Trap","empShielded":false,"id":"trap_box","stackMa…`
- Row 429 `trap_fish`: `{"description":"A woven funnel of wood and cord set in moving water. Fish enter but cannot find the exit. Strong where geography supports it, worthless where it does not.","displayName":"Fish Trap","empShielded":false,"id":"trap_fish","sta…`
- Row 430 `trap_body_grip`: `{"description":"A heavy spring-loaded mechanism that closes steel jaws on anything that passes through the frame. Efficient, durable, and severe. Manufactured before the exchange, or found in trapper caches.","displayName":"Body-Grip Trap"…`
- Row 431 `trap_snare`: `{"description":"A loop of cord or wire set across a game trail, tightened by a springy branch. Cheap enough to deploy more than one, unreliable enough that materials still matter.","displayName":"Wire Snare","empShielded":false,"id":"trap_…`
- Row 432 `trap_deadfall`: `{"description":"A flat stone or salvaged plate balanced on a trigger stick. Low-tech, material-light, and as old as hunger. The trigger is the hard part.","displayName":"Deadfall Trap","empShielded":false,"id":"trap_deadfall","stackMax":3,…`
- Row 433 `trap_pit`: `{"description":"A concealed pit dug into a game trail, lined with sharpened stakes. Labor-heavy, location-dependent, and capable of stopping something large.","displayName":"Pit Trap","empShielded":false,"id":"trap_pit","stackMax":1,"trade…`
- Row 434 `trap_net`: `{"description":"A mesh of cord or salvaged netting strung between posts or branches. Broader coverage than a snare, higher upfront cost, and more things can go wrong.","displayName":"Net Trap","empShielded":false,"id":"trap_net","stackMax"…`
- Row 435 `trap_cage`: `{"description":"A sprung cage of salvaged metal and wire, triggered by weight on a plate. Expensive to build, durable once built. The animal lives inside it.","displayName":"Cage Trap","empShielded":false,"id":"trap_cage","stackMax":2,"tra…`
- Row 436 `trap_bird_snare`: `{"description":"A fine noose of thread or thin wire set on a perch stick. Lightweight, fast to deploy, and fragile. Frequent small calories with a health-risk shadow.","displayName":"Bird Snare","empShielded":false,"id":"trap_bird_snare","…`
- Row 437 `item_collectible_vinyl_chamber_record`: `{"description":"A carefully sleeved classical record with a split paper jacket. A hand-written listening date is inked on the inner sleeve in small, deliberate handwriting.","displayName":"Scratched Chamber Record","empShielded":false,"id"…`
- Row 438 `item_collectible_vinyl_civil_broadcast`: `{"description":"A government instructional recording pressed on heavy vinyl. The label bears a municipal crest and a serial number stamped in red ink.","displayName":"Civil-Information Broadcast Pressing","empShielded":false,"id":"item_col…`
- Row 439 `item_collectible_vinyl_folk_compilation`: `{"description":"A worn compilation from a small regional label. The sleeve art shows a river bend and a painted sunset. Someone traced the track listing in pencil.","displayName":"Regional Folk Compilation","empShielded":false,"id":"item_c…`
- Row 440 `item_collectible_family_portrait`: `{"description":"A faded photograph of three people in ordinary clothing. One face is circled in blue ink on the reverse. The edge is creased from a wallet fold.","displayName":"Family Portrait","empShielded":false,"id":"item_collectible_fa…`
- Row 441 `item_collectible_unit_photograph`: `{"description":"A group portrait in front of a concrete wall. Unit insignia is visible on shoulder patches. Someone wrote names on the back in three different hands.","displayName":"Military Unit Photograph","empShielded":false,"id":"item_…`
- Row 442 `item_collectible_civil_defense_poster`: `{"description":"A printed poster showing shelter procedures with numbered diagrams. The corners are torn where it was ripped from a wall. The ink is still bright.","displayName":"Civil-Defense Poster","empShielded":false,"id":"item_collect…`
- Row 443 `item_collectible_propaganda_poster`: `{"description":"A large-format poster with bold typography and a factory silhouette. The slogan is half-obscured by a water stain that runs diagonally across the lower third.","displayName":"State Propaganda Poster","empShielded":false,"id…`
- Row 444 `item_collectible_concert_poster`: `{"description":"A hand-lettered poster for a community hall concert. The date and venue are legible. Someone added a phone number in marker at the bottom.","displayName":"Concert Poster","empShielded":false,"id":"item_collectible_concert_p…`
- Row 445 `item_collectible_field_medicine_handbook`: `{"description":"A soft-cover manual with dog-eared pages and a coffee ring on the back cover. The spine is reinforced with medical tape. Diagrams show wound closure and splinting.","displayName":"Field Medicine Handbook","empShielded":fals…`
- Row 446 `item_collectible_pre_war_novel`: `{"description":"A paperback with a cracked spine and a bookmark left at page 84. The cover shows a painted landscape that no longer exists. The pages smell of dust and old paper.","displayName":"Pre-War Novel","empShielded":false,"id":"ite…`
- Row 447 `item_collectible_science_magazine`: `{"description":"A damaged issue with a marked technical article on water purification. Someone underlined a paragraph about filtration rates in red pencil.","displayName":"Science Periodical","empShielded":false,"id":"item_collectible_scie…`
- Row 448 `item_collectible_hunting_magazine`: `{"description":"An outdoors magazine with advertisements for gear that no longer exists. The cover shows a mountain lake. The pages are foxed at the edges.","displayName":"Hunting Magazine","empShielded":false,"id":"item_collectible_huntin…`
- Row 449 `item_collectible_diesel_service_manual`: `{"description":"A grease-stained manual with torque specifications and wiring diagrams. The cover is reinforced with duct tape. A mechanic's notes are penciled in the margins.","displayName":"Diesel Engine Service Manual","empShielded":fal…`
- Row 450 `item_collectible_radio_repair_guide`: `{"description":"A spiral-bound guide with component diagrams and frequency charts. The binding is cracked and three pages are loose. Someone marked the emergency frequencies in ink.","displayName":"Shortwave Radio Repair Guide","empShielde…`
- Row 451 `item_collectible_water_treatment_handbook`: `{"description":"A technical handbook with chemical dosing tables and filter maintenance procedures. The cover is water-damaged but the text is legible. Tabs mark the purification chapter.","displayName":"Water-Treatment Handbook","empShiel…`
- Row 452 `item_collectible_air_filter_manual`: `{"description":"A manufacturer's manual for HEPA filtration units. Diagrams show seal replacement and pressure-drop testing. The warranty card is still tucked inside the back cover.","displayName":"Air-Filtration Maintenance Manual","empSh…`
- Row 453 `item_collectible_dosimeter_guide`: `{"description":"A pocket-sized guide for calibrating pen dosimeters and Geiger counters. The calibration tables are printed on waterproof stock. A sticker shows a lab serial number.","displayName":"Dosimeter Calibration Guide","empShielded…`
- Row 454 `item_collectible_unit_log_fragment`: `{"description":"A torn page from a military unit log. Dates, route codes, and supply counts are typed in columns. Three names are crossed out in blue pencil.","displayName":"Unit Log Fragment","empShielded":false,"id":"item_collectible_uni…`
- Row 455 `item_collectible_deployment_order`: `{"description":"A carbon copy of a deployment order with unit designations and movement times. The paper is creased where it was folded into quarters. A rubber stamp mark is partially visible.","displayName":"Deployment Order","empShielded…`
- Row 456 `item_collectible_casualty_list`: `{"description":"A typed list of names, service numbers, and evacuation codes. The paper is brittle and the ink has faded to brown. Someone annotated the margins in pencil.","displayName":"Casualty Evacuation List","empShielded":false,"id":…`
- Row 457 `item_collectible_mothers_letter`: `{"description":"A handwritten letter on lined paper, the script careful and slightly uneven. It asks about food, sleep, and whether the heating is working. The envelope is addressed but unsealed.","displayName":"Mother's Letter","empShield…`
- Row 458 `item_collectible_soldiers_letter`: `{"description":"A letter folded into a small square, never posted, the handwriting hurried. It describes a cold night and a missing friend. The paper is creased from being carried in a pocket.","displayName":"Soldier's Unsent Letter","empS…`
- Row 459 `item_collectible_rejection_letter`: `{"description":"A form letter denying a housing application. The signature is a rubber stamp. Someone wrote 'appeal' in the margin and underlined it twice.","displayName":"Administrative Rejection Letter","empShielded":false,"id":"item_col…`
- Row 460 `item_collectible_civil_defense_badge`: `{"description":"A metal badge with a stamped serial number and a pin-back clasp. The enamel is chipped at one edge. It identifies a civil-defense volunteer role.","displayName":"Civil-Defense Badge","empShielded":false,"id":"item_collectib…`
- Row 461 `item_collectible_transit_badge`: `{"description":"A brass badge with a transit authority crest. The pin is bent but the engraving is clear. It marks a municipal worker or inspector.","displayName":"Transit Authority Badge","empShielded":false,"id":"item_collectible_transit…`
- Row 462 `item_collectible_military_patch`: `{"description":"An embroidered shoulder patch with a unit insignia. The thread is frayed at the edges where it was pulled from a uniform. The colors are still distinct.","displayName":"Military Unit Patch","empShielded":false,"id":"item_co…`
- Row 463 `item_collectible_trade_guild_patch`: `{"description":"A woven patch showing a gear and hammer crossed. The backing is stiff with old adhesive. It marks a maintenance or trade guild member.","displayName":"Trade Guild Patch","empShielded":false,"id":"item_collectible_trade_guil…`
- Row 464 `item_collectible_childs_doll`: `{"description":"A cloth doll with button eyes and yarn hair. One arm is reattached with rough stitching. The dress is made from a scrap of patterned fabric.","displayName":"Child's Doll","empShielded":false,"id":"item_collectible_childs_do…`
- Row 465 `item_collectible_music_box`: `{"description":"A small tin music box with a painted lid. The mechanism still turns when wound, though the melody is simple and slightly out of tune. The key is bent but functional.","displayName":"Wind-Up Music Box","empShielded":false,"i…`
- Row 466 `item_collectible_prayer_book`: `{"description":"A small devotional book with a worn leather cover. The pages are thin and the text is printed in two columns. A ribbon marker is sewn into the spine.","displayName":"Pocket Prayer Book","empShielded":false,"id":"item_collec…`
- Row 467 `item_collectible_prayer_beads`: `{"description":"A string of wooden beads with a metal cross or medallion. The wood is smooth from handling. The cord is knotted at intervals and shows wear at the clasp.","displayName":"Prayer Beads","empShielded":false,"id":"item_collecti…`
- Row 468 `item_collectible_team_pennant`: `{"description":"A triangular pennant in washed-out team colors. The felt is soft and the stitching is loose at one corner. A date and score are printed below the team name.","displayName":"Local Team Pennant","empShielded":false,"id":"item…`
- Row 469 `item_collectible_match_program`: `{"description":"A folded program for a local sports match. Player names and positions are listed inside. The cover shows a team logo and a sponsor's advertisement.","displayName":"Match Program","empShielded":false,"id":"item_collectible_m…`
- Row 470 `item_collectible_civic_token`: `{"description":"A brass token stamped with a civic crest and a festival date. The edge is milled and the surface shows light corrosion. It was a souvenir or admission token.","displayName":"Municipal Commemorative Token","empShielded":fals…`
- Row 471 `item_collectible_folk_craft`: `{"description":"A small ceramic tile with a painted landscape. The glaze is crackled and one corner is chipped. The painting shows a village scene with a church spire.","displayName":"Hand-Painted Ceramic Tile","empShielded":false,"id":"it…`
- Row 472 `item_collectible_exchange_day_newspaper`: `{"description":"A newspaper from the day of the exchange. The headline is partially visible through a water stain. The paper is brittle and the edges crumble when handled.","displayName":"Exchange-Day Newspaper","empShielded":false,"id":"i…`
- Row 473 `item_collectible_local_newspaper`: `{"description":"An ordinary local paper from before the crisis. The front page shows sports scores, a weather forecast, and a classified ad for a used car. Nothing apocalyptic.","displayName":"Local Newspaper","empShielded":false,"id":"ite…`
- Row 474 `item_collectible_road_map`: `{"description":"A folded road map with routes highlighted in yellow marker. The folds are worn through at the creases. Someone circled a junction and wrote 'check this' in pencil.","displayName":"Pre-War Road Map","empShielded":false,"id":…`
- Row 475 `item_collectible_topo_map`: `{"description":"A military topographic map with contour lines and grid references. The paper is waterproof stock. Annotations show patrol routes and observation posts in red ink.","displayName":"Military Topographic Map","empShielded":fals…`
- Row 476 `item_collectible_survivor_map`: `{"description":"A hand-drawn map on the back of a utility bill. Landmarks are sketched in pencil with notes about danger zones and water sources. The handwriting is cramped and urgent.","displayName":"Hand-Drawn Survivor Map","empShielded"…`
- Row 477 `item_document_evacuation_list`: `{"description":"A carbon-copy roster with names crossed out in different inks. Some names were added after the original typing. The last page is blank except for a date stamp.","displayName":"Carbon-Copy Evacuation Roster","empShielded":fa…`
- Row 478 `item_document_ration_record`: `{"description":"A ruled ledger with increasingly uneven handwriting. Serving weights are recorded in columns that grow narrower toward the back cover.","displayName":"Shelter Ration Ledger","empShielded":false,"id":"item_document_ration_re…`
- Row 479 `item_document_blood_trail_note`: `{"description":"A folded clinical note found along a corridor. The paper is creased from being carried in a pocket. One corner is stained dark.","displayName":"Folded Clinical Note","empShielded":false,"id":"item_document_blood_trail_note"…`
- Row 480 `item_document_barricade_placement`: `{"description":"A rough sketch of numbered barricade locations drawn on the back of a municipal form. One route is circled twice in red.","displayName":"Barricade Placement Sketch","empShielded":false,"id":"item_document_barricade_placemen…`
- Row 481 `item_document_sealed_door_warning`: `{"description":"A printed warning notice taped over a door frame. Someone has written additional instructions in marker beneath the official text.","displayName":"Sealed-Room Warning Notice","empShielded":false,"id":"item_document_sealed_d…`
- Row 482 `item_document_family_photograph`: `{"description":"A photograph with writing on the reverse. The image shows three people standing in front of a building that no longer exists. The paper is soft from handling.","displayName":"Family Photograph","empShielded":false,"id":"ite…`
- Row 483 `item_document_military_map`: `{"description":"A folded topographic map with grease-pencil deployment marks. The annotations contradict the printed legend in several places.","displayName":"Annotated Military Map","empShielded":false,"id":"item_document_military_map","s…`
- Row 484 `item_document_broadcast_transcript`: `{"description":"A typed broadcast script with corrections in two different hands. Several lines are struck through before the final version.","displayName":"Emergency Broadcast Script","empShielded":false,"id":"item_document_broadcast_tran…`
- Row 485 `item_document_vandalized_propaganda`: `{"description":"A faction poster written over by multiple hands in different inks. The original message is barely visible beneath the responses.","displayName":"Defaced Propaganda Poster","empShielded":false,"id":"item_document_vandalized_…`
- Row 486 `item_document_handwritten_warning`: `{"description":"A piece of cardboard with a warning written in charcoal. The letters are large and deliberate. Wire is threaded through a hole punched in one corner.","displayName":"Improvised Warning Sign","empShielded":false,"id":"item_d…`
- Row 487 `item_document_maintenance_record`: `{"description":"A clipboard with a maintenance checklist. The same temporary repair is signed off six times in different handwriting. The seventh slot is empty.","displayName":"Maintenance Clipboard","empShielded":false,"id":"item_document…`
- Row 488 `item_document_shelter_rejection_list`: `{"description":"A typed admission list with reason codes beside each name. The codes are administrative: CAPACITY, MEDICAL, SECURITY, DEFERRED. Several names have no code at all.","displayName":"Shelter Admission Rejection List","empShield…`
- Row 489 `item_document_ration_theft_ledger`: `{"description":"A small notebook comparing expected and actual stock levels. The discrepancies are recorded without comment. The last entry is dated three days before the final supply drop.","displayName":"Unofficial Stock Discrepancy Ledg…`
- Row 490 `item_document_water_notice`: `{"description":"An official notice posted beside a water source. A handwritten correction has been added below the printed text, contradicting the all-clear date.","displayName":"Contaminated Water Notice","empShielded":false,"id":"item_do…`
- Row 491 `item_document_repair_note`: `{"description":"A repair checklist that ends mid-procedure. The last completed step is circled twice. The pen mark trails off the edge of the paper.","displayName":"Half-Finished Repair Checklist","empShielded":false,"id":"item_document_re…`
- Row 492 `item_document_casualty_list`: `{"description":"A typed list of names with status columns. The categories shift partway through — from MEDICAL to MISSING to simply a dash. The typing becomes uneven.","displayName":"Typed Casualty Report","empShielded":false,"id":"item_do…`
- Row 493 `item_document_triage_record`: `{"description":"A treatment priority sheet with numbered strips of cardboard attached. The numbers correspond to patients who are no longer present. The saline bags above are empty.","displayName":"Triage Priority Sheet","empShielded":fals…`
- Row 494 `item_document_last_letter`: `{"description":"A letter folded into an envelope that was never sealed. The handwriting is careful at the beginning and rushed at the end. No address is written on the front.","displayName":"Unsent Letter","empShielded":false,"id":"item_do…`
- Row 495 `item_document_supply_requisition`: `{"description":"A requisition form with SUBSTITUTED stamped across the original request. The approved items are less than half of what was requested. The signature is illegible.","displayName":"Denied Supply Requisition Form","empShielded"…`
- Row 496 `item_document_evacuation_route_map`: `{"description":"A public evacuation map with handwritten blocked-route markings. Three of the four printed routes are crossed out. A fourth route has been added in pencil.","displayName":"Public Evacuation Route Map","empShielded":false,"i…`
- Row 497 `item_document_quarantine_notice`: `{"description":"A printed quarantine notice with a date and time. A second notice has been stapled over it with tighter restrictions. The staples are rusted.","displayName":"Quarantine Restriction Notice","empShielded":false,"id":"item_doc…`
- Row 498 `item_document_civil_defense_poster`: `{"description":"A pre-crisis civil defense poster listing emergency procedures. The advice is practical and calm. Someone has written 'GOOD LUCK' in the margin.","displayName":"Civil Defense Preparedness Poster","empShielded":false,"id":"i…`
- Row 499 `item_document_field_report`: `{"description":"A concise operational report typed on a manual typewriter. The margins contain handwritten annotations in a different hand. One paragraph is underlined twice.","displayName":"Faction Field Report","empShielded":false,"id":"…`
- Row 500 `item_document_journal_fragment`: `{"description":"A few torn pages from a personal journal. The entries describe small routines — meals, weather, a broken window. The last entry stops mid-sentence.","displayName":"Torn Journal Pages","empShielded":false,"id":"item_document…`
- Row 501 `item_document_death_certificate`: `{"description":"An official death certificate with several blank fields. The cause-of-death line contains a question mark. The signature block is empty.","displayName":"Incomplete Death Certificate","empShielded":false,"id":"item_document_…`
- Row 502 `item_document_supply_inventory`: `{"description":"An inventory sheet showing expected and actual stock. One crate is listed as MISSING with a note: 'See Requisition 4471.' The requisition number does not appear elsewhere.","displayName":"Warehouse Inventory Sheet","empShie…`
- Row 503 `item_document_radio_log`: `{"description":"A handwritten log of radio frequencies and times. The entries stop on a specific date. The last entry reads: '142.5 — no response. 88.4 — static. 104.7 — silence.'","displayName":"Handwritten Radio Frequency Log","empShield…`
- Row 504 `item_document_child_drawing`: `{"description":"A child's drawing on lined paper. The figures are tall and the house is small. A sun occupies the entire upper corner. Labels are written in an adult's hand beneath the child's marks.","displayName":"Child's Crayon Drawing"…`
- Row 505 `item_document_confession`: `{"description":"A confession written on the back of a utility form. The handwriting is steady. The author names a specific action, a specific place, and a specific consequence. Nothing else.","displayName":"Handwritten Confession","empShie…`
- Row 506 `item_document_will`: `{"description":"A will written on lined paper and witnessed by two signatures. The author assigns specific items to specific people. One name has been crossed out and replaced.","displayName":"Handwritten Will","empShielded":false,"id":"it…`
- Row 507 `item_document_debt_default_notice`: `{"description":"A printed notice from a creditor faction declaring the shelter in default. The notice lists the debt amount, the missed deadline, and the consequences. A stamp reads FINAL NOTICE in red ink.","displayName":"Creditor Default…`
- Row 508 `item_document_weather_gate_warning`: `{"description":"A weather-beaten notice posted at a route entrance. The lamination is cracked and water has seeped in. The warning advises against travel.","displayName":"Route Closure Warning","empShielded":false,"id":"item_document_weath…`
- Row 509 `item_document_patrol_order`: `{"description":"A typed patrol order from a faction commander. The order specifies the patrol route, rules of engagement, and what to look for. Margins contain handwritten annotations.","displayName":"Faction Patrol Order","empShielded":fa…`
- Row 510 `item_heavy_wool_coat`: `{"description":"A thick, dense wool overcoat tailored to withstand biting subzero blizzards.","displayName":"Heavy Boiled-Wool Greatcoat","empShielded":false,"id":"item_heavy_wool_coat","stackMax":1,"tradeValue":45,"type":"Clothing","weigh…`
- Row 511 `item_thermal_parka`: `{"description":"Pre-war arctic exploration parka filled with thermal synthetic down.","displayName":"Down-Lined Arctic Parka","display_name":"Down-Lined Arctic Parka","empShielded":false,"id":"item_thermal_parka","stackMax":1,"tradeValue":…`
- Row 512 `item_fur_mittens`: `{"description":"Heavy fur gauntlets that protect fingers from cold-numbness and frostbite.","displayName":"Trapped-Pelts Gauntlet Mittens","empShielded":false,"id":"item_fur_mittens","stackMax":1,"tradeValue":18,"type":"Clothing","weight":…`
- Row 513 `item_insulated_boots`: `{"description":"Double-felt insulated snow boots with deep rubber treads.","displayName":"Felt-Lined Mukluk Snow Boots","empShielded":false,"id":"item_insulated_boots","stackMax":1,"tradeValue":35,"type":"Clothing","weight":2.2}`
- Row 514 `item_improvised_burn_barrel`: `{"description":"A perforated 55-gallon steel drum rigged for emergency scrap wood burning.","displayName":"Slotted Steel Scrap Burn Barrel","empShielded":true,"id":"item_improvised_burn_barrel","stackMax":1,"tradeValue":25,"type":"Device",…`
- Row 515 `item_portable_kerosene_heater`: `{"description":"A portable convection kerosene space heater capable of warming bunker rooms.","displayName":"Pressurized Wick Kerosene Heater","empShielded":true,"id":"item_portable_kerosene_heater","stackMax":1,"tradeValue":80,"type":"Dev…`
- Row 516 `item_acoustic_guitar`: `{"description":"A weathered acoustic guitar strung with scavenged wire strings.","displayName":"Restrung Acoustic Guitar","empShielded":false,"id":"item_acoustic_guitar","stackMax":1,"tradeValue":40,"type":"Tool","weight":2.0}`
- Row 517 `item_harmonica`: `{"description":"A pocket brass diatonic harmonica in the key of C.","displayName":"Brass Reed Harmonica","empShielded":false,"id":"item_harmonica","stackMax":1,"tradeValue":15,"type":"Tool","weight":0.2}`
- Row 518 `item_playing_cards`: `{"description":"A complete deck of 52 cards used for poker, blackjack, and solitary patience.","displayName":"Plastic-Coated Playing Cards","empShielded":false,"id":"item_playing_cards","stackMax":5,"tradeValue":12,"type":"Trade","weight":…`
- Row 519 `item_carved_figurine`: `{"description":"A small animal or figure hand-carved from scrap wood during downtime hours.","displayName":"Whittled Pine Figurine","empShielded":false,"id":"item_carved_figurine","stackMax":10,"tradeValue":8,"type":"Trade","weight":0.1}`
- Row 520 `item_wasteland_sketch`: `{"description":"A sketch of the surface or shelter life drawn on reclaimed cardstock.","displayName":"Charcoal Wasteland Sketch","empShielded":false,"id":"item_wasteland_sketch","stackMax":10,"tradeValue":6,"type":"Trade","weight":0.05}`
- Row 521 `item_dog_tags_scavenged`: `{"description":"Notched military identification tags worn by high-value targets or officers.","displayName":"Scavenged Dog Tags","empShielded":false,"id":"item_dog_tags_scavenged","stackMax":20,"tradeValue":25,"type":"Trade","weight":0.05}`
- Row 522 `item_warlord_trophy`: `{"description":"A notched branding iron or insignia carried by wasteland gang leaders as proof of authority.","displayName":"Raider Warlord Sigil","empShielded":false,"id":"item_warlord_trophy","stackMax":5,"tradeValue":50,"type":"Trade","…`
- Row 523 `item_decryption_keycard_prewar`: `{"description":"A magnetic keycard containing pre-war military decryption tokens and algorithmic hashes.","displayName":"Cryptographic Master Keycard","empShielded":true,"id":"item_decryption_keycard_prewar","stackMax":5,"tradeValue":65,"t…`
- Row 524 `surgical_saw`: `{"description":"Tempered surgical steel saw with fine serrations used for rapid traumatic amputations.","displayName":"Amputation Bone Saw","empShielded":false,"id":"surgical_saw","stackMax":1,"tradeValue":45,"type":"Tool","weight":1.2}`
- Row 525 `painkillers`: `{"description":"Concentrated analgesic tablets that mitigate traumatic surgical shock and severe nerve pain.","displayName":"Synthetic Painkillers","empShielded":false,"id":"painkillers","stackMax":20,"tradeValue":15,"type":"Medical","weig…`
- Row 526 `prosthetic_wooden_arm`: `{"description":"A carved pine prosthetic arm with articulated copper joints and leather straps.","displayName":"Articulated Wooden Arm","empShielded":false,"id":"prosthetic_wooden_arm","stackMax":1,"tradeValue":35,"type":"Equipment","weigh…`
- Row 527 `prosthetic_wooden_leg`: `{"description":"A reinforced oak peg leg with rubber foot tread, restoring mobility after lower limb loss.","displayName":"Carved Peg Leg","empShielded":false,"id":"prosthetic_wooden_leg","stackMax":1,"tradeValue":30,"type":"Equipment","we…`
- Row 528 `bionic_arm_prototype`: `{"description":"Advanced pre-war prosthetic arm with direct nerve-interface sensors and titanium servo motors.","displayName":"Myoelectric Bionic Arm","empShielded":true,"id":"bionic_arm_prototype","stackMax":1,"tradeValue":180,"type":"Equ…`
- Row 529 `bionic_leg_prototype`: `{"description":"Pre-war military bionic limb featuring pneumatic suspension and hydraulic load balancers.","displayName":"Hydraulic Bionic Leg","empShielded":true,"id":"bionic_leg_prototype","stackMax":1,"tradeValue":200,"type":"Equipment"…`
- Row 530 `train_coal`: `{"description":"High-density anthracite coal mined from subterranean seams, essential for locomotive steam boilers.","displayName":"Locomotive Boiler Coal","empShielded":false,"id":"train_coal","stackMax":50,"tradeValue":12,"type":"Fuel","…`
- Row 531 `steel_rail_segment`: `{"description":"Heavy rolled steel standard gauge track section used for permanent way repairs.","displayName":"Steel Railing Section","empShielded":false,"id":"steel_rail_segment","stackMax":10,"tradeValue":40,"type":"Material","weight":1…`
- Row 532 `railroad_ties`: `{"description":"Pressure-treated timber sleepers for anchoring heavy iron tracks across gravel roadbeds.","displayName":"Creosote Railway Ties","empShielded":false,"id":"railroad_ties","stackMax":10,"tradeValue":20,"type":"Material","weigh…`
- Row 533 `fungus_spores_common`: `{"description":"Spore inoculum for growing high-yield edible grey mycelium beds in darkness.","displayName":"Cultivated Grey Spores","empShielded":false,"id":"fungus_spores_common","stackMax":20,"tradeValue":8,"type":"Material","weight":0.…`
- Row 534 `fungus_spores_bioluminescent`: `{"description":"Rare bioluminescent fungal spores providing ambient cold illumination without electricity.","displayName":"Phosphor Bracket Spores","empShielded":false,"id":"fungus_spores_bioluminescent","stackMax":10,"tradeValue":25,"type…`
- Row 535 `fungus_spores_medicinal`: `{"description":"Rare mycelial strain containing antimicrobial peptides for medical drug synthesis.","displayName":"Cordyceps Spores","empShielded":false,"id":"fungus_spores_medicinal","stackMax":10,"tradeValue":50,"type":"Material","weight…`
- Row 536 `harvested_mushrooms_subterranean`: `{"description":"Nutrient-dense cultivated caps harvested from dark underground beds. Safe to cook and consume.","displayName":"Subterranean Cap Mushrooms","empShielded":false,"hungerRestore":6,"id":"harvested_mushrooms_subterranean","moral…`
- Row 537 `stolen_ration_cache`: `{"description":"Hoarded shelter emergency foodstuffs recovered as physical evidence in theft investigations.","displayName":"Concealed Ration Cache","empShielded":false,"id":"stolen_ration_cache","stackMax":5,"tradeValue":15,"type":"Trade"…`
- Row 538 `forensic_clue_bloodstained`: `{"description":"Physical forensic evidence linking a suspect to a violent assault or sabotage site.","displayName":"Bloodstained Fabric Scrap","empShielded":false,"id":"forensic_clue_bloodstained","stackMax":10,"tradeValue":10,"type":"Trad…`
- Row 539 `slate_and_chalk`: `{"description":"Reusable classroom slate used by children and apprentices for spelling and numeracy drills.","displayName":"Writing Slate & Chalk","empShielded":false,"id":"slate_and_chalk","stackMax":5,"tradeValue":8,"type":"Tool","weight…`
- Row 540 `school_primer`: `{"description":"Hand-copied instructional book covering basic first aid, botany, and foraging for young dwellers.","displayName":"Wasteland Survival Primer","empShielded":false,"id":"school_primer","stackMax":5,"tradeValue":25,"type":"Tool…`
- Row 541 `iron_shackles`: `{"description":"Heavy riveted iron cuffs used to secure dangerous captives and unruly raiders in detention.","displayName":"Forged Iron Restraints","empShielded":false,"id":"iron_shackles","stackMax":5,"tradeValue":20,"type":"Tool","weight…`
- Row 542 `sedative_draught`: `{"description":"Distilled poppy and valerian extract used to pacify frantic prisoners or calm traumatized children.","displayName":"Herbal Calming Tincture","empShielded":false,"id":"sedative_draught","stackMax":10,"tradeValue":18,"type":"…`
- Row 543 `gene_therapy_retroviral_vial`: `{"description":"Pre-war cryo-preserved viral vector used in clinical gene therapy to excise unstable DNA mutations.","displayName":"Retroviral Gene Splicer","empShielded":true,"id":"gene_therapy_retroviral_vial","stackMax":5,"tradeValue":1…`
- Row 544 `camo_ash_cloak`: `{"description":"Loose hooded cowl treated with fine volcanic ash to blend seamlessly into scorched wastelands.","displayName":"Ash-Washed Cowl","empShielded":false,"id":"camo_ash_cloak","stackMax":1,"tradeValue":45,"type":"Armor","weight":…`
- Row 545 `camo_ghillie_shroud`: `{"description":"Heavy burlap netting woven with dried reeds and marsh moss for deep concealment in wet biomes.","displayName":"Swamp Foliage Ghillie","empShielded":false,"id":"camo_ghillie_shroud","stackMax":1,"tradeValue":65,"type":"Armor…`
- Row 546 `camo_night_stalker_suit`: `{"description":"Matte-black rubberized jumpsuit that muffles footsteps and eliminates glare during night sorties.","displayName":"Rubberized Night Suit","empShielded":false,"id":"camo_night_stalker_suit","stackMax":1,"tradeValue":80,"type"…`
- Row 547 `weapon_suppressor_improvised`: `{"description":"Machined aluminum canister packed with steel wool baffles to suppress rifle muzzle reports.","displayName":"Improvised Weapon Baffle","empShielded":false,"id":"weapon_suppressor_improvised","stackMax":5,"tradeValue":50,"typ…`
- Row 548 `night_optics_goggles`: `{"description":"Light-amplifying electro-optical goggles providing acute situational awareness in subterranean darkness.","displayName":"Low-Light Night Optics","empShielded":true,"id":"night_optics_goggles","stackMax":2,"tradeValue":110,"…`
- Row 549 `item_aviation_fuel_canister`: `{"description":"Refined pressurized fuel canister suitable for ultralight combustion engines and blowtorches.","displayName":"Aviation Fuel Canister","empShielded":true,"id":"item_aviation_fuel_canister","stackMax":10,"tradeValue":45,"type…`
- Row 550 `item_aircraft_airframe_spares`: `{"description":"Duralumin ribs, tension cables, and treated fabric for maintaining improvised airframes.","displayName":"Aircraft Airframe Spares","empShielded":true,"id":"item_aircraft_airframe_spares","stackMax":5,"tradeValue":60,"type":…`
- Row 551 `item_chain_gang_shackles`: `{"description":"Heavy wrought iron leg fetters and link chain used to secure captives on arduous labor details.","displayName":"Chain Gang Shackles","empShielded":true,"id":"item_chain_gang_shackles","stackMax":4,"tradeValue":25,"type":"To…`
- Row 552 `item_slave_collar`: `{"description":"Heavy riveted steel collar with padlock lug, scavenged from wasteland slavers to coerce compliance.","displayName":"Locking Slaver Collar","empShielded":true,"id":"item_slave_collar","stackMax":5,"tradeValue":35,"type":"Too…`
- Row 553 `item_medical_precursor_base`: `{"description":"Distilled pharmaceutical reagent base used in laboratory synthesis of advanced medicines.","displayName":"Chemical Precursor Base","empShielded":true,"id":"item_medical_precursor_base","stackMax":20,"tradeValue":20,"type":"…`
- Row 554 `item_sterile_solvent_pack`: `{"description":"Pure anhydrous solvent solution used for chemical compounding and crystalline precipitation.","displayName":"Sterile Solvent Pack","empShielded":true,"id":"item_sterile_solvent_pack","stackMax":15,"tradeValue":25,"type":"Me…`
- Row 555 `item_chem_hyper_stim`: `{"description":"Concentrated neural stimulant ampoule that accelerates reflex response at severe cardiac risk.","displayName":"Hyper-Stim Combat Ampoule","empShielded":true,"id":"item_chem_hyper_stim","stackMax":10,"tradeValue":55,"type":"…`
- Row 556 `item_chem_dulcimer_tincture`: `{"description":"Potent botanical analgesic tincture that suppresses acute physical pain but induces sedation.","displayName":"Dulcimer Pain Tincture","empShielded":true,"id":"item_chem_dulcimer_tincture","stackMax":10,"tradeValue":50,"type…`
- Row 557 `item_chem_clarity_salts`: `{"description":"Refined crystalline stimulant salts that temporarily eliminate mental exhaustion.","displayName":"Clarity Focus Salts","empShielded":true,"id":"item_chem_clarity_salts","stackMax":10,"tradeValue":45,"type":"Medical","weight…`
- Row 558 `item_chem_haze_resin`: `{"description":"Pungent sedative resin block vaporized to calm psychological panic and acute trauma.","displayName":"Haze Calming Resin","empShielded":true,"id":"item_chem_haze_resin","stackMax":10,"tradeValue":40,"type":"Medical","weight"…`
- Row 559 `item_chem_fungal_antibiotic`: `{"description":"A cloudy ampoule distilled from medicinal cordyceps spores. The lab treats it as a last-line infection breaker, not a comfort drug.","displayName":"Cordyceps Antibiotic Ampoule","empShielded":true,"id":"item_chem_fungal_ant…`
- Row 560 `item_chem_spore_sedative`: `{"description":"A dark tincture pressed from cultivated caps. It drops a body into heavy sleep and leaves the mouth tasting of cellar dust.","displayName":"Mycelial Cortex Depressant","empShielded":true,"id":"item_chem_spore_sedative","sta…`
- Row 561 `item_chem_choke_spore_toxin`: `{"description":"A sealed vial of concentrated black-mold spores in solvent. Opening it is a decision. The lab keeps it on the poison shelf, not the dispensary.","displayName":"Choke-Spore Contact Toxin","empShielded":true,"id":"item_chem_c…`
- Row 562 `item_official_ballot_box`: `{"description":"Heavy welded steel ballot repository with tamper-evident seal for settlement democratic votes.","displayName":"Padlocked Ballot Box","empShielded":true,"id":"item_official_ballot_box","stackMax":1,"tradeValue":30,"type":"To…`
- Row 563 `item_flatbread`: `{"description":"Dense unleavened bread baked from greenhouse ash-grain. Keeps for days wrapped in cloth and tastes faintly of the filter-bed soil it grew in.","displayName":"Ash-Grain Flatbread","empShielded":true,"hungerRestore":22,"id":"…`
- Row 564 `item_boiled_roots`: `{"description":"Surface-foraged roots boiled clean in shelter water. Soft, bland, and safe — most days that is enough.","displayName":"Boiled Foraged Roots","empShielded":true,"hungerRestore":14,"id":"item_boiled_roots","moraleEffect":1,"s…`
- Row 565 `item_vegetable_soup`: `{"description":"A hot pot of greenhouse greens and tubers simmered in clean water. Steam on the window, a bowl in every hand. The shelter feels almost whole when it is on the stove.","displayName":"Greenhouse Vegetable Soup","empShielded":…`
- Row 566 `item_pemmican`: `{"description":"Smoked meat and rendered grain pressed into wax-paper blocks. A day's ration that fits in a coat pocket and outlasts the expedition that carries it.","displayName":"Pemmican Travel Block","empShielded":true,"hungerRestore":…`
- Row 567 `item_travel_ration`: `{"description":"Smoked meat, pickled tubers, and flatbread wrapped together in waxed cloth. Everything a walker needs for a long day outside the wire, packed to carry light.","displayName":"Bundled Travel Ration","empShielded":true,"hunger…`
- Row 568 `item_geophone_probe`: `{"description":"A weighted coil in a spiked can. Driven into soil or bedrock, it turns the ground's shudder into a whisper of current.","displayName":"Geophone Probe","id":"item_geophone_probe","stackMax":6,"tradeValue":30,"type":"Material…`
- Row 569 `item_low_noise_sensor_amplifier`: `{"description":"A shielded preamp box wound for quiet. The hard part was never hearing; it was hearing only the ground.","displayName":"Low-Noise Sensor Amplifier","id":"item_low_noise_sensor_amplifier","stackMax":4,"tradeValue":55,"type":…`
- Row 570 `item_vibration_dampening_mount`: `{"description":"Sand-filled pads and rubber gaskets that convince machinery noise to stay home instead of walking into the sensor lines.","displayName":"Vibration Dampening Mount","id":"item_vibration_dampening_mount","stackMax":4,"tradeVa…`
- Row 571 `item_bedrock_sensor_rig`: `{"description":"A cased borehole string with grouting kit. Lowered, sealed, and forgotten — until the deep rock has news.","displayName":"Bedrock Sensor Rig","id":"item_bedrock_sensor_rig","stackMax":3,"tradeValue":90,"type":"Material","we…`
- Row 572 `item_iron_pyrite_ore`: `{"description":"Fool's gold, so called. The acid line knows better: roasted and worked, it gives up its sulfur honestly.","displayName":"Iron Pyrite Ore","id":"item_iron_pyrite_ore","stackMax":12,"tradeValue":14,"type":"Material","weight":…`
- Row 573 `item_industrial_acid_carboy`: `{"description":"A waxed-glass carboy of bench acid, packed in straw. Everyone in the shelter knows exactly which shelf it lives on.","displayName":"Industrial Acid Carboy","id":"item_industrial_acid_carboy","stackMax":4,"tradeValue":60,"ty…`
- Row 574 `item_industrial_oxidizer_reagent`: `{"description":"A general-purpose oxidizing reagent for the bench train. It makes slow chemistry out of stubborn slag and old metal.","displayName":"Industrial Oxidizer Reagent","id":"item_industrial_oxidizer_reagent","stackMax":6,"tradeVa…`
- Row 575 `item_oxidizer_reagent_flask`: `{"description":"A sealed bench flask of the third fraction. Small, expensive, and never stored where a lamp could fall on it.","displayName":"Precision Oxidizer Flask","id":"item_oxidizer_reagent_flask","stackMax":4,"tradeValue":85,"type":…`
- Row 576 `item_battery_electrolyte_concentrate`: `{"description":"Fractionated acid cut to cell strength in waxed flasks. The difference between a battery and a planter box.","displayName":"Battery Electrolyte Concentrate","id":"item_battery_electrolyte_concentrate","stackMax":6,"tradeVal…`
- Row 577 `item_foundry_pickling_reagent`: `{"description":"A dilute bath that eats scale and slag film off fresh castings. Rinsed in time, it stops where told.","displayName":"Foundry Pickling Reagent","id":"item_foundry_pickling_reagent","stackMax":6,"tradeValue":42,"type":"Materi…`
- Row 578 `item_neutralizer_lime_bag`: `{"description":"Burnt lime slaked and bagged. The quiet answer to everything the acid line spills, stored within reach of the retort room.","displayName":"Neutralizer Lime Bag","id":"item_neutralizer_lime_bag","stackMax":8,"tradeValue":18,…`
- Row 579 `item_battery_maintenance_fluid`: `{"description":"Topped-up electrolyte blended for aging cells. The generator crew takes it without being asked twice.","displayName":"Battery Maintenance Fluid","id":"item_battery_maintenance_fluid","stackMax":6,"tradeValue":32,"type":"Mat…`
- Row 580 `paper_stock`: `{"category":"material","description":"Reams of buffered paper pulled from a printer's warehouse before the roof fell in. Cultural work runs on this: transcription, chronicles, testimony kept past the life of the people in it.","display_nam…`
- Row 581 `microfiche_film`: `{"category":"material","description":"Sealed high-contrast film for the archive camera. Each sheet holds a shelf of books at stamp size. Keep it cold, keep it dark, keep it away from the flood line.","display_name":"Microfiche Film Sheets"…`
- Row 582 `acetate_blank_disc`: `{"category":"material","description":"Aluminum blanks coated in black lacquer. The cutting stylus carves sound straight into the surface — one pass, no second takes. The archive calls them fragile; the musicians call them proof.","display_…`
- Row 583 `ammo_76mm_he_flak`: `{"category":"ordnance","description":"Battery round with a clockwork fuse set by hand. Fills the sky with fragments on a schedule. Heavy, loud, and the closest thing to an umbrella the shelter owns.","display_name":"76mm Time-Fuzed Flak Sh…`
- Row 584 `ammo_76mm_proximity_fuse`: `{"category":"ordnance","description":"The fuse listens for the target and decides on its own. Rarer than the flak rounds and worth the trouble: it does not need the gunner's guess to be perfect.","display_name":"76mm Proximity-Fuzed Shell"…`
- Row 585 `ammo_76mm_tungsten_penetrator`: `{"category":"ordnance","description":"A dense dart behind a powder charge, for things that fall too fast and too straight. Three to a crate, each one accounted for in the ledger.","display_name":"76mm Tungsten-Core Penetrator","durability"…`
- Row 586 `ammo_chaff_burst`: `{"category":"ordnance","description":"A cloud of foil strips fired ahead of the track to confuse the incoming seeker. Stops nothing by itself; buys the gun crews the seconds that do.","display_name":"Chaff Burst Cartridge","durability":1.0…`
- Row 587 `ammo_76mm_beacon_smokey`: `{"category":"ordnance","description":"Burns bright and orange where it bursts, marking the track for guns that lost it. Nobody's favorite round. Everybody's when the fog closes in.","display_name":"76mm Beacon Smoke Round","durability":1.0…`
- Row 588 `ammo_76mm_shaped_charge`: `{"category":"ordnance","description":"Copper liner, cavity forward, all the promise of a cutting torch at terminal velocity. For the heavy tracks that shrug at fragments.","display_name":"76mm Shaped-Charge Shell","durability":1.0,"id":"am…`
- Row 589 `item_grain_flour`: `{"description":"Dark flour milled from ash-barley grain. It is coarse, shelf-stable, and suitable for the shelter kitchen's bread and porridge recipes.","displayName":"Milled Ash-Barley Flour","empShielded":true,"hungerRestore":18,"id":"it…`
- Row 590 `item_silo_pest_treatment`: `{"description":"A sealed maintenance packet used by the grain authority to lower normalized silo pest pressure.","displayName":"Silo Pest Treatment","empShielded":true,"id":"item_silo_pest_treatment","stackMax":10,"tradeValue":6,"type":"Ma…`
- Row 591 `item_oxygen_supply`: `{"description":"A sealed medical oxygen supply unit produced by the shelter's abstract air-separation plant.","displayName":"Oxygen Supply","empShielded":true,"id":"item_oxygen_supply","stackMax":20,"tradeValue":12,"type":"Medical","weight…`
- Row 592 `item_nitrogen_supply`: `{"description":"A sealed nitrogen supply unit used as a general foundry and workshop process input.","displayName":"Nitrogen Supply","empShielded":true,"id":"item_nitrogen_supply","stackMax":20,"tradeValue":10,"type":"Material","weight":1.…`
- Row 593 `item_rock_salt_sack`: `{"description":"Crude mined rock salt crystals used as electrochemical feedstock for chlor-alkali sanitation chemistry.","displayName":"Rock Salt Sack","empShielded":true,"id":"item_rock_salt_sack","stackMax":20,"tradeValue":6,"type":"Mate…`
- Row 594 `item_liquid_bleach_carboy`: `{"description":"Concentrated sodium hypochlorite solution produced by the chlor-alkali plant for water disinfection and medical hygiene.","displayName":"Liquid Bleach Carboy","empShielded":true,"id":"item_liquid_bleach_carboy","stackMax":1…`
- Row 595 `item_caustic_soda_flakes`: `{"description":"Solid sodium hydroxide byproduct used in industrial soap synthesis, scrubbing, and chemical neutralization.","displayName":"Caustic Soda Flakes","empShielded":true,"id":"item_caustic_soda_flakes","stackMax":20,"tradeValue":…`
- Row 596 `item_industrial_cell_anode`: `{"description":"Dimensionally stable dimensioned anode plate used in high-purity chlor-alkali synthesis cells.","displayName":"Industrial Cell Anode","empShielded":true,"id":"item_industrial_cell_anode","stackMax":5,"tradeValue":25,"type":…`
- Row 597 `item_parabolic_aluminum_dish_segment`: `{"description":"Precision-stamped polished aluminum curved segment for assembling high-temperature solar concentrators.","displayName":"Parabolic Aluminum Dish Segment","empShielded":true,"id":"item_parabolic_aluminum_dish_segment","stackM…`
- Row 598 `item_dual_axis_tracking_gimbal`: `{"description":"Motorized or counterweighted mechanical mount that keeps optical concentrators aligned with the sun.","displayName":"Dual-Axis Tracking Gimbal","empShielded":true,"id":"item_dual_axis_tracking_gimbal","stackMax":4,"tradeVal…`
- Row 599 `item_focal_stirling_engine_generator`: `{"description":"Closed-cycle heat engine mounted at a solar dish focal point to generate electricity from concentrated sunlight.","displayName":"Focal Stirling Engine Generator","empShielded":true,"id":"item_focal_stirling_engine_generator…`
- Row 600 `item_cast_borosilicate_glass_blank`: `{"description":"Annealed thick glass disc with low thermal expansion, ready for rough grinding, polishing, and parabolization.","displayName":"Cast Borosilicate Glass Blank","empShielded":true,"id":"item_cast_borosilicate_glass_blank","sta…`
- Row 601 `item_precision_rangefinder_achromat`: `{"description":"Hand-finished optical doublet for analog range observation and steadier weapon calibration.","displayName":"Precision Rangefinder Achromat","empShielded":true,"id":"item_precision_rangefinder_achromat","stackMax":2,"tradeVa…`
- Row 602 `item_cerium_oxide_polishing_rouge`: `{"description":"Ultra-fine optical polishing compound used to achieve scratch-free optical surfaces and figure correction.","displayName":"Cerium Oxide Polishing Rouge","empShielded":true,"id":"item_cerium_oxide_polishing_rouge","stackMax"…`
- Row 603 `item_optical_pitch_lap`: `{"description":"Channelled optical pitch tool cast on a backing plate, used for precision parabolization and figuring.","displayName":"Optical Pitch Lap","empShielded":true,"id":"item_optical_pitch_lap","stackMax":8,"tradeValue":15,"type":…`
- Row 604 `item_foucault_tester_rig`: `{"description":"Optical bench knife-edge testing apparatus for quantitative shadowgram surface figure measurement.","displayName":"Foucault Knife-Edge Tester Rig","empShielded":true,"id":"item_foucault_tester_rig","stackMax":2,"tradeValue"…`
- Row 605 `item_titanium_breaching_shield`: `{"description":"Heavy reinforced tactical breaching shield offering high frontal kinetic protection and anchorable phalanx defense.","displayName":"Titanium Breaching Shield","empShielded":true,"id":"item_titanium_breaching_shield","stackM…`
- Row 606 `item_laminated_ballistic_viewport_glass`: `{"description":"Multi-layer polycarbonate-glass composite viewport insert providing optical clarity under tactical fire.","displayName":"Laminated Ballistic Viewport Glass","empShielded":true,"id":"item_laminated_ballistic_viewport_glass",…`
- Row 607 `item_hardened_ground_anchor_spikes`: `{"description":"Tungsten-tipped deployable anchor spikes allowing tactical shields to lock into concrete or frozen earth.","displayName":"Hardened Ground Anchor Spikes","empShielded":true,"id":"item_hardened_ground_anchor_spikes","stackMax…`
- Row 608 `item_ebpvd_ceramic_target_ingot`: `{"description":"High-purity sintered zirconia-yttria target ingot for electron-beam evaporation in vacuum chambers.","displayName":"EB-PVD Ceramic Target Ingot","id":"item_ebpvd_ceramic_target_ingot","stackMax":5,"tradeValue":45,"type":"Ma…`
- Row 609 `item_electron_gun_tungsten_filament`: `{"description":"Precision wound refractory tungsten cathode hairpin for high-voltage electron beam generation.","displayName":"Electron Gun Tungsten Filament","id":"item_electron_gun_tungsten_filament","stackMax":10,"tradeValue":35,"type":…`
- Row 610 `item_mcraly_bond_coat_powder`: `{"description":"Metallic superalloy powder matrix applied before ceramic barrier deposition to provide oxidation resistance.","displayName":"MCrAlY Bond Coat Powder","id":"item_mcraly_bond_coat_powder","stackMax":10,"tradeValue":30,"type":…`
- Row 611 `item_superalloy_turbine_blade_blank`: `{"description":"Precision-cast nickel-chromium superalloy blade blank awaiting protective thermal barrier coating.","displayName":"Superalloy Turbine Blade Blank","id":"item_superalloy_turbine_blade_blank","stackMax":5,"tradeValue":55,"typ…`
- Row 612 `item_coated_turbine_blade`: `{"description":"Finished high-temperature turbine blade with columnar ceramic thermal barrier coating.","displayName":"EB-PVD Coated Turbine Blade","id":"item_coated_turbine_blade","stackMax":5,"tradeValue":110,"type":"Equipment","weight":…`
- Row 613 `item_coated_combustor_tile`: `{"description":"Ceramic-insulated combustor liner tile for generator and thermal plant upgrade.","displayName":"Coated Combustor Tile","id":"item_coated_combustor_tile","stackMax":5,"tradeValue":95,"type":"Equipment","weight":4.0}`
- Row 614 `item_coated_diesel_injector`: `{"description":"Hardened and coated fuel injector nozzle with elevated heat and corrosion resistance.","displayName":"Coated Diesel Injector Nozzle","id":"item_coated_diesel_injector","stackMax":10,"tradeValue":60,"type":"Equipment","weigh…`
- Row 615 `item_ebpvd_vacuum_pump_seal`: `{"description":"Fluoropolymer hermetic gasket seal engineered for high-vacuum coater chambers.","displayName":"EB-PVD Viton Vacuum Seal","id":"item_ebpvd_vacuum_pump_seal","stackMax":10,"tradeValue":18,"type":"Material","weight":0.3}`
- Row 616 `item_mine_flail_module`: `{"description":"Reinforced vehicle-mounted rotating flail drum assembly for mechanical mine detonation and breach clearance.","displayName":"Heavy Mine-Clearing Flail Module","id":"item_mine_flail_module","stackMax":1,"tradeValue":220,"typ…`
- Row 617 `item_hardened_flail_chain`: `{"description":"Heavy forged alloy strike chain links designed to withstand ground impact and blast detonations.","displayName":"Hardened Flail Chain Link","id":"item_hardened_flail_chain","stackMax":20,"tradeValue":25,"type":"Material","w…`
- Row 618 `item_hydraulic_drive_motor`: `{"description":"High-torque industrial hydraulic motor configured for rotating heavy clearance drums and flails.","displayName":"Heavy Hydraulic Drive Motor","id":"item_hydraulic_drive_motor","stackMax":2,"tradeValue":90,"type":"Equipment"…`
- Row 619 `item_armored_blast_shield`: `{"description":"Curved sloped armor plate mounted behind the flail drum to shield chassis and driver from blast fragmentation.","displayName":"Armored Flail Blast Deflector","id":"item_armored_blast_shield","stackMax":2,"tradeValue":85,"ty…`
- Row 620 `item_pdms_silicone_kit`: `{"description":"Two-part prepolymer and curing agent kit for casting microchannel diagnostic cartridges.","displayName":"Microfluidic PDMS Elastomer Kit","id":"item_pdms_silicone_kit","stackMax":10,"tradeValue":35,"type":"Material","weight…`
- Row 621 `item_microfluidic_reader`: `{"description":"Compact multichannel photometer for reading fluorescent assay cartridges with laser excitation.","displayName":"Optical Fluorescence Micro-Reader","id":"item_microfluidic_reader","stackMax":2,"tradeValue":140,"type":"Equipm…`
- Row 622 `item_assay_reagent_pack`: `{"description":"Freeze-dried antibody/fluorophore reagent pellets for loading into microfluidic diagnostic chips.","displayName":"Lyophilized Assay Reagent Pack","id":"item_assay_reagent_pack","stackMax":25,"tradeValue":28,"type":"Consumab…`
- Row 623 `item_microfluidic_cartridge_general`: `{"description":"Sealed lab-on-a-chip diagnostic cartridge loaded with reagents for rapid patient pathogen detection.","displayName":"Microfluidic Diagnostic Cartridge","id":"item_microfluidic_cartridge_general","stackMax":20,"tradeValue":4…`
- Row 624 `item_rail_grinding_head`: `{"description":"High-precision abrasive stone grinding head for reprofiling damaged steel rails on draisines.","displayName":"Rail Grinding Modular Head","id":"item_rail_grinding_head","stackMax":1,"tradeValue":190,"type":"Equipment","weig…`
- Row 625 `item_abrasive_grinding_stone`: `{"description":"Vitrified corundum abrasive cup wheel rated for heavy rail corrugation and defect removal.","displayName":"Abrasive Corundum Grinding Stone","id":"item_abrasive_grinding_stone","stackMax":10,"tradeValue":32,"type":"Material…`
- Row 626 `item_rail_profiling_cylinder`: `{"description":"Double-acting hydraulic cylinder providing constant pressure on rail grinding stones.","displayName":"Hydraulic Downforce Cylinder","id":"item_rail_profiling_cylinder","stackMax":4,"tradeValue":50,"type":"Equipment","weight…`
- Row 627 `item_spark_suppression_manifold`: `{"description":"High-pressure misting manifold that suppresses grinding sparks and prevents wayside brushfires.","displayName":"Water Spark Suppression Manifold","id":"item_spark_suppression_manifold","stackMax":2,"tradeValue":40,"type":"E…`
- Row 628 `item_blowtorch`: `{"description":"Portable pressurized cutting and thawing torch for unfreezing conduits and metal fabrication.","displayName":"Oxy-Propane Blowtorch","id":"item_blowtorch","stackMax":1,"tradeValue":45,"type":"Tool","weight":2.2}`
- Row 629 `item_manual_generator_maintenance`: `{"description":"Exhaustive handwritten and illustrated maintenance manual detailing diesel engine tolerances, injector calibration, and radiator loop repair.","displayName":"Technical Manual: Generator Maintenance","id":"item_manual_genera…`
- Row 630 `item_manual_field_medicine`: `{"description":"Annotated trauma care manual with step-by-step sterile debridement, hypothermia rewarming, and frostbite salvage procedures.","displayName":"Technical Manual: Field Medicine","id":"item_manual_field_medicine","stackMax":5,"…`
- Row 631 `item_manual_rough_repairs`: `{"description":"Practical engineering guide covering load-bearing timber shoring, hydraulic jacking, and seismic conduit reinforcement.","displayName":"Technical Manual: Structural Shoring & Repairs","id":"item_manual_rough_repairs","stack…`
- Row 632 `item_manual_seismology`: `{"description":"Geological field manual recording tectonic shear patterns, fault stress telemetry, and tremor epicenter triangulation.","displayName":"Technical Manual: Deep-Earth Seismology","id":"item_manual_seismology","stackMax":5,"tra…`
- Row 633 `item_metallurgy_iron_ingot`: `{"description":"A rough-squared ingot of reclaimed iron from the smelter bay. Pour marks still visible on the flank. The starting point of every serious repair job in the shelter.","displayName":"Reclaimed Iron Ingot","id":"item_metallurgy…`
- Row 634 `item_metallurgy_copper_ingot`: `{"description":"Stripped wire and salvage melted down into a dense copper ingot. Conductivity varies with the scrap it came from, but it beats scavenging for another spool that may not exist.","displayName":"Refined Copper Ingot","id":"ite…`
- Row 635 `item_metallurgy_steel_billet`: `{"description":"A heavy billet of machine steel, rendered down from mechanical scrap. Consistent enough to machine, dull gray, still warm at the core when it leaves the cooling beds.","displayName":"Mixed Machine-Steel Billet","id":"item_m…`
- Row 636 `item_metallurgy_solder_stock`: `{"description":"Hand-cast bars of soft alloy for electrical and plumbing work. Not pre-war specification, but it flows and it holds. The electronics bench goes through it slowly and always needs more.","displayName":"Solder Stock","id":"it…`
- Row 637 `item_metallurgy_heavy_i_beam`: `{"description":"A long structural beam drawn from the smelter's heavy mold. The flanges are thick and slightly uneven, but it carries load the way the old factory steel did. Deep-strata work stalls without them.","displayName":"Heavy I-Bea…`
- Row 638 `item_metallurgy_shoring_plate`: `{"description":"A flat rolled plate for lining excavated tunnels and reinforcing weak overhead rock. Bolt holes are cast in. Cold to the touch even hours after pouring.","displayName":"Shoring Plate","id":"item_metallurgy_shoring_plate","s…`
- Row 639 `item_metallurgy_spring_steel_billet`: `{"description":"A billet alloyed for flex and fatigue resistance. The foundry crew argues about the quench, but the results keep winches and valve returns working long past when rigid stock would have cracked.","displayName":"Spring Steel …`
- Row 640 `item_metallurgy_gear_blank`: `{"description":"A cast gear wheel before the teeth are cut. Casting them in-house saves weeks of waiting on trade caravans for salvage that may not match anyway.","displayName":"Cast Gear Blank","id":"item_metallurgy_gear_blank","stackMax"…`
- Row 641 `item_metallurgy_shaft_stock`: `{"description":"Straight, turned shafting in common diameters. Everything that spins in the shelter eventually needs a piece of this: pumps, blowers, drill heads.","displayName":"Machined Shaft Stock","id":"item_metallurgy_shaft_stock","st…`
- Row 642 `item_metallurgy_shielding_plate`: `{"description":"A heavy plate cast around lead salvage for radiation shielding. Awkward to hang, impossible to fake. The cryo vault specification calls for these by name.","displayName":"Dense Shielding Plate","id":"item_metallurgy_shieldi…`
- Row 643 `item_metallurgy_tool_blank`: `{"description":"An alloy tool blank ready for the grinder. With patient finishing it becomes a chisel, a cutter, or a die that outlasts three of anything salvaged.","displayName":"High-Grade Tool Blank","id":"item_metallurgy_tool_blank","s…`
- Row 644 `item_aeroponic_medicinal_root`: `{"description":"A clean medicinal root grown in a sealed mist chamber. It is small, pale, and more valuable for what the reservoir kept out than for its size.","displayName":"Aeroponic Medicinal Root","id":"item_aeroponic_medicinal_root","…`
- Row 645 `item_aeroponic_food_leaf`: `{"description":"A dense leaf crop raised under measured light and nutrient mist. The harvest is modest, but it arrives without a surface run or a contaminated root bed.","displayName":"Aeroponic Food Leaf","id":"item_aeroponic_food_leaf","…`
- Row 646 `item_insect_larvae_meal`: `{"description":"Dried black soldier larvae milled into a coarse fish feed. Protein-dense, shelf-stable, and one of the few rationed inputs a closed aquaponics loop cannot invent from scrap.","displayName":"Insect Larvae Meal","id":"item_in…`
- Row 647 `item_biofilter_media`: `{"description":"Inert ceramic beads and plastic lattice for bacterial colonization. Backwashing a fouled bed without fresh media only redistributes the sludge.","displayName":"Biofilter Media","id":"item_biofilter_media","stackMax":20,"tra…`
- Row 648 `item_aquaponic_fish`: `{"description":"A farmed fish pulled from a raft tank before the dissolved oxygen fell. The flesh is pale and clean relative to surface water, but it still needs kitchen work before it is a meal.","displayName":"Aquaponic Fish","id":"item_…`
- Row 649 `item_ballistics_cleaning_kit`: `{"description":"A bench tin containing bore solvent, lint-free patches, gauges, and a small torque key. It is enough to service one worn weapon if the hands using it are patient.","displayName":"Ballistics Cleaning Kit","id":"item_ballisti…`
- Row 650 `item_pneumatic_capsule_50mm`: `{"description":"A hard polymer capsule with a felt cargo sleeve and a numbered brass collar. It is built for small sealed transfers between shelter rooms.","displayName":"50mm Pneumatic Capsule","id":"item_pneumatic_capsule_50mm","stackMax…`
- Row 651 `item_pneumatic_capsule_100mm`: `{"description":"A wide-bore service capsule for heavier parts and sealed food allotments. Its locking collar is overbuilt because a jammed tube stops being a convenience quickly.","displayName":"100mm Pneumatic Capsule","id":"item_pneumati…`
- Row 652 `item_geothermal_descaling_kit`: `{"description":"A measured drum of descaling compound, gaskets, and a sacrificial brush head for the ORC heat exchanger. It is unpleasant work, but less unpleasant than opening a leaking loop.","displayName":"Geothermal Descaling Kit","id"…`
- Row 653 `item_manual_bolt_shears`: `{"description":"Heavy compound shears for cutting fence wire and light rebar by hand. Slow, quiet, and hard on the wrists.","displayName":"Manual Bolt Shears","id":"item_manual_bolt_shears","stackMax":2,"tradeValue":22,"type":"Tool","weigh…`
- Row 654 `item_hydraulic_wire_cutter`: `{"description":"A compact hydraulic cutter head for concertina and reinforced fence. Quieter than a ram, hungrier for maintenance.","displayName":"Hydraulic Wire Cutter","id":"item_hydraulic_wire_cutter","stackMax":1,"tradeValue":48,"type"…`
- Row 655 `item_mechanical_breach_ram`: `{"description":"A braced mechanical ram for doors, sandbags, and light masonry. Loud enough to announce itself two lanes over.","displayName":"Mechanical Breach Ram","id":"item_mechanical_breach_ram","stackMax":1,"tradeValue":55,"type":"To…`
- Row 656 `item_expedition_winch_kit`: `{"description":"Cable, snatch block, and mount hardware for pulling wreckage or hedgehog obstacles when a vehicle can take the load.","displayName":"Expedition Winch Kit","id":"item_expedition_winch_kit","stackMax":1,"tradeValue":60,"type"…`
- Row 657 `item_linear_breach_section`: `{"description":"A sealed, single-use clearance charge section from prewar stocks. Gameplay consumable only — no construction or initiation details survive in the kit notes.","displayName":"Linear Breach Section","id":"item_linear_breach_se…`
- Row 658 `item_groundwater_sensor`: `{"description":"A sealed down-hole probe salvaged from a monitoring station. Reads water level and conductivity through a cracked glass gauge. Still more honest than dipping a rag down the casing and tasting it.","displayName":"Groundwater…`
- Row 659 `item_aquifer_isolation_module`: `{"description":"A heavy shut-off collar meant for capping a compromised well line. Sealing one source means the rest of the grid works harder. Everyone understands the arithmetic even if they hate it.","displayName":"Aquifer Isolation Modu…`
- Row 660 `item_well_maintenance_kit`: `{"description":"Brushes, calibration weights, and a hand pump for flushing silt out of a monitoring line. Dirty work, but a fouled sensor lies to you, and a lying sensor drowns you later.","displayName":"Well Service Kit","id":"item_well_m…`
- Row 661 `item_switch_stand_module`: `{"description":"A hand-throw stand with a calibrated latch for holding a switch point true. Without one, the points drift and every crew on the corridor trusts the rails a little less.","displayName":"Switch Stand Module","id":"item_switch…`
- Row 662 `item_signal_lamp_module`: `{"description":"A shielded lamp and lens assembly for a corridor signal post. A dark signal reads as a broken promise; a lit one, even amber, tells crews somebody is still watching the line.","displayName":"Signal Lamp Module","id":"item_s…`
- Row 663 `item_rail_control_component`: `{"description":"Sealed relay logic for interlocking a junction's levers so conflicting routes cannot be set at once. Old semaphore logic in a dust-proof case — the quiet kind of safety.","displayName":"Route-Control Component","id":"item_r…`
- Row 664 `item_track_maintenance_kit`: `{"description":"Sledge, spikes, broom, and a scoop for drifting ash. Junction work is mostly housekeeping: keep the points moving and the lamps burning, or the whole corridor stops.","displayName":"Track Maintenance Kit","id":"item_track_m…`
- Row 665 `item_tablet_binder`: `{"description":"Neutral compressed starch base that holds a pressed dose together. No effect on its own; without it, a batch is just powder in a drawer.","displayName":"Inert Binder Stock","id":"item_tablet_binder","stackMax":20,"tradeValu…`
- Row 666 `item_sealed_packaging_foil`: `{"description":"Crimp-sealed foil strips that keep moisture and dust off finished doses. Sealed stock keeps for seasons; a paper twist does not.","displayName":"Sealed Packaging Foil","id":"item_sealed_packaging_foil","stackMax":15,"tradeV…`
- Row 667 `item_press_tooling_set`: `{"description":"Matched die and punch set for the tablet press. Worn tooling makes cracked, uneven doses — the kind a sick person can't afford to guess about.","displayName":"Press Tooling Set","id":"item_press_tooling_set","stackMax":3,"t…`
- Row 668 `item_tablet_coating_base`: `{"description":"Neutral film-forming wash for finished doses. A coated dose keeps longer and goes down easier — shelter chemistry, nothing more arcane than that.","displayName":"Protective Coating Base","id":"item_tablet_coating_base","sta…`
- Row 669 `cassette_greenhouse_tapes_1`: `{"contamination":0,"description":"A cassette in a seed envelope instead of a case. The label reads Growth Trial in survey pencil. A woman's voice talks yield curves over the hum of grow-lights, pleased with a row of plants.","displayName":…`
- Row 670 `cassette_greenhouse_tapes_2`: `{"contamination":0,"description":"A cassette labelled Filtered Light. The same technician, flatter now, measuring a sun that reads forty percent under baseline. Wind behind the glass the whole way through.","displayName":"Cassette: Greenho…`
- Row 671 `cassette_greenhouse_tapes_3`: `{"contamination":0,"description":"A cassette labelled Save the Seed. Envelopes are counted out loud, one hundred and twelve, names checked against a list. It ends with a promise about the cold box at the seed library annex.","displayName":…`
- Row 672 `cassette_field_hospital_7_1`: `{"contamination":0,"description":"A cassette labelled Intake in a nurse's precise hand. A tired voice counts forty-one cots twice and reports that the kettle still works.","displayName":"Cassette: Field Hospital 7 I","durability":0,"empShi…`
- Row 673 `cassette_field_hospital_7_2`: `{"contamination":0,"description":"A cassette labelled Substitutions. Halved tablets, a sterilizer run on hope, and a surgeon who tells stories while he stitches.","displayName":"Cassette: Field Hospital 7 II","durability":0,"empShielded":f…`
- Row 674 `cassette_field_hospital_7_3`: `{"contamination":0,"description":"A cassette labelled The Transfer List. Fourteen names read twice, an X argued into a star, and a road described as re-prioritised.","displayName":"Cassette: Field Hospital 7 III","durability":0,"empShielde…`
- Row 675 `cassette_field_hospital_7_4`: `{"contamination":0,"description":"A cassette labelled Night Shift. Recorded at four in the morning, when the arithmetic of one generator and three machines had already been done.","displayName":"Cassette: Field Hospital 7 IV","durability":…`
- Row 676 `cassette_field_hospital_7_5`: `{"contamination":0,"description":"A cassette labelled Leave the Tags in a shakier hand. Forty-one names, treatment given, treatment owed, and an instruction about where help should go.","displayName":"Cassette: Field Hospital 7 V","durabil…`
- Row 677 `cassette_evacuation_train_1`: `{"contamination":0,"description":"A cassette labelled Departure Board. Platform noise, a whistle, and a conductor who notes that there are more people than tickets.","displayName":"Cassette: Evacuation Train I","durability":0,"empShielded"…`
- Row 678 `cassette_evacuation_train_2`: `{"contamination":0,"description":"A cassette labelled Red Signal. Two hours at a light nobody is watching, eleven unticketed passengers, and a lamp kept lit on purpose.","displayName":"Cassette: Evacuation Train II","durability":0,"empShie…`
- Row 679 `cassette_evacuation_train_3`: `{"contamination":0,"description":"A cassette labelled No Further East. Control says pending. Three hundred people and a railway that is pending.","displayName":"Cassette: Evacuation Train III","durability":0,"empShielded":false,"equipSlot"…`
- Row 680 `cassette_evacuation_train_4`: `{"contamination":0,"description":"A cassette labelled End of the Line. A conductor promises to lock the carriages and hang the lamps where they'll be seen, and asks someone to explain the difference between stopping and running out of some…`
- Row 681 `cassette_station_14_1`: `{"contamination":0,"description":"A cassette labelled Scheduled Programming. Announcements at six, agriculture at seven, and one strange minute cut into the weather.","displayName":"Cassette: Station 14 I","durability":0,"empShielded":fals…`
- Row 682 `cassette_station_14_2`: `{"contamination":0,"description":"A cassette labelled Frequency Change. A broadcaster who has filed four temporary notices in eleven years and kept count of which outlived the stations.","displayName":"Cassette: Station 14 II","durability"…`
- Row 683 `cassette_station_14_3`: `{"contamination":0,"description":"A cassette labelled Caller List. Names on one side and what people asked on the other, read between records.","displayName":"Cassette: Station 14 III","durability":0,"empShielded":false,"equipSlot":"","hea…`
- Row 684 `cassette_station_14_4`: `{"contamination":0,"description":"A cassette labelled No Network Feed. The feed stopped at 4:51, mid-sentence, and the station decided that a quiet station is still a station.","displayName":"Cassette: Station 14 IV","durability":0,"empShi…`
- Row 685 `cassette_station_14_5`: `{"contamination":0,"description":"A cassette labelled Open Microphone. Road conditions, a water notice, the Brandts' good news, and the times tables read like a bulletin.","displayName":"Cassette: Station 14 V","durability":0,"empShielded"…`
- Row 686 `cassette_station_14_6`: `{"contamination":0,"description":"A cassette labelled Carrier. Technical instructions for keeping a transmitter alive: the mast first, the fuse second, the operator last.","displayName":"Cassette: Station 14 VI","durability":0,"empShielded…`
- Row 687 `cassette_fathers_tapes_1`: `{"contamination":0,"description":"A cassette labelled For Saturday. A dripping tap, a promised washer, school shoes dried by the stove, and an order to eat something green.","displayName":"Cassette: Father's Tapes I","durability":0,"empShi…`
- Row 688 `cassette_fathers_tapes_2`: `{"contamination":0,"description":"A cassette labelled If the Trains Stop. Instructions about a blue biscuit tin, a ration card, and an aunt who raised three children on less.","displayName":"Cassette: Father's Tapes II","durability":0,"emp…`
- Row 689 `cassette_fathers_tapes_3`: `{"contamination":0,"description":"A cassette labelled The Blue Cup. A story about a cracked cup buried in a plant pot and watered for a month, told without a moral.","displayName":"Cassette: Father's Tapes III","durability":0,"empShielded"…`
- Row 690 `cassette_fathers_tapes_4`: `{"contamination":0,"description":"A cassette labelled Keep This One. The next size of shoes, wool on top, an apology that isn't finished, and a plant by the window that should be watered anyway.","displayName":"Cassette: Father's Tapes IV"…`
- Row 691 `cassette_dam_keeper_log_1`: `{"contamination":0,"description":"A cassette labelled Load Shedding. Four megawatts between two and six, a hand's width of water under the mark, and a good week defined as nothing on the board being red.","displayName":"Cassette: Dam Keepe…`
- Row 692 `cassette_dam_keeper_log_2`: `{"contamination":0,"description":"A cassette labelled Islanding. Thirteen towns on one frequency, and a keeper who says the whole job is fifty hertz, on purpose.","displayName":"Cassette: Dam Keeper's Log II","durability":0,"empShielded":f…`
- Row 693 `cassette_dam_keeper_log_3`: `{"contamination":0,"description":"A cassette labelled Gate Two. A leaking hydraulic pack, a seal kit signed out in August, and a four-hour watch kept off the record.","displayName":"Cassette: Dam Keeper's Log III","durability":0,"empShield…`
- Row 694 `cassette_dam_keeper_log_4`: `{"contamination":0,"description":"A cassette labelled Dispatch Is Gone. The ring tone changed to nothing, so the log is the dispatch now.","displayName":"Cassette: Dam Keeper's Log IV","durability":0,"empShielded":false,"equipSlot":"","hea…`
- Row 695 `cassette_dam_keeper_log_5`: `{"contamination":0,"description":"A cassette labelled Manual. Gates closed in sequence, the valley kept on penstock head, and the wheel formally handed to whoever comes after. The machinery is still running, slower.","displayName":"Cassett…`
- Row 696 `cassette_teachers_recordings_1`: `{"contamination":0,"description":"A cassette labelled Geography Lesson. Rivers in the lower course, an atlas open at pages 40 to 44, and a promise that there will be a Thursday.","displayName":"Cassette: Teacher's Recordings I","durability…`
- Row 697 `cassette_teachers_recordings_2`: `{"contamination":0,"description":"A cassette labelled Practical Arithmetic. Rations split by need with the working shown, because the working keeps you honest.","displayName":"Cassette: Teacher's Recordings II","durability":0,"empShielded"…`
- Row 698 `cassette_teachers_recordings_3`: `{"contamination":0,"description":"A cassette labelled Attendance. A register read to stairwells, a blue book left on a desk, and a class dismissed for now.","displayName":"Cassette: Teacher's Recordings III","durability":0,"empShielded":fa…`
- Row 699 `cassette_quarantine_tapes_1`: `{"contamination":0,"description":"A cassette labelled Case Definition. A rash that starts at the wrists, a definition written in pencil, and wristbands at the entrance.","displayName":"Cassette: Quarantine Tapes I","durability":0,"empShiel…`
- Row 700 `cassette_quarantine_tapes_2`: `{"contamination":0,"description":"A cassette labelled Separate Entrance. Arrows on the floor, masks past the second arrow, and a pharmacy's whole stock of pale soap.","displayName":"Cassette: Quarantine Tapes II","durability":0,"empShielde…`
- Row 701 `cassette_quarantine_tapes_3`: `{"contamination":0,"description":"A cassette labelled No Visitors. A rule argued against for ten correct minutes, and messages written small on wristbands to fit more in.","displayName":"Cassette: Quarantine Tapes III","durability":0,"empS…`
- Row 702 `cassette_quarantine_tapes_4`: `{"contamination":0,"description":"A cassette labelled Release Criteria. One test kit, a shelf of probably, and a signature where a reading should be. The definition stays in pencil.","displayName":"Cassette: Quarantine Tapes IV","durabilit…`
- Row 703 `cassette_checkpoint_kilo_1`: `{"contamination":0,"description":"A military-grade cassette tape. The label reads Day 1: The Sealing. A corporal's voice reports twelve personnel, air filtration nominal, waiting it out.","displayName":"Cassette: Checkpoint Kilo I","durabi…`
- Row 704 `cassette_checkpoint_kilo_2`: `{"contamination":0,"description":"A military-grade cassette tape. The label reads Day 12: The First Death. The corporal notes safe dosimeter readings despite unexplained loss.","displayName":"Cassette: Checkpoint Kilo II","durability":0,"e…`
- Row 705 `cassette_checkpoint_kilo_3`: `{"contamination":0,"description":"A military-grade cassette tape. The label reads Day 30: The Ration Split. Background disputes over half rations and a suspect filter.","displayName":"Cassette: Checkpoint Kilo III","durability":0,"empShiel…`
- Row 706 `cassette_checkpoint_kilo_4`: `{"contamination":0,"description":"A military-grade cassette tape. The label reads Day 47: The Final Entry. Corporal Maren warns against trusting deceptive gauges over human lungs.","displayName":"Cassette: Checkpoint Kilo IV","durability":…`
- Row 707 `cassette_saint_maren_1`: `{"contamination":0,"description":"A clinical cassette tape labelled Triage Protocol. Day Zero plus six hours; a doctor announces protocol Omega sorting by survival probability.","displayName":"Cassette: Saint Maren I","durability":0,"empSh…`
- Row 708 `cassette_saint_maren_2`: `{"contamination":0,"description":"A clinical cassette tape labelled The Hardest Decision. Tearful recollections of triage prioritizing the young while comforting the rest.","displayName":"Cassette: Saint Maren II","durability":0,"empShield…`
- Row 709 `cassette_saint_maren_3`: `{"contamination":0,"description":"A clinical cassette tape labelled The Pharmacy Key. Final instructions revealing the key location behind a framed diploma in the doctor's office.","displayName":"Cassette: Saint Maren III","durability":0,"…`
- Row 710 `cassette_family_bunker_1`: `{"contamination":0,"description":"A consumer cassette labelled The First Week. A father's voice over children's laughter describes waiting out the fallout in a basement bunker.","displayName":"Cassette: Martinez Family I","durability":0,"e…`
- Row 711 `cassette_family_bunker_2`: `{"contamination":0,"description":"A consumer cassette labelled The Cough. Hoarse audio documenting fever, depleted iodine pills, and a hidden water cache in a backyard shed.","displayName":"Cassette: Martinez Family II","durability":0,"emp…`
- Row 712 `cassette_family_bunker_3`: `{"contamination":0,"description":"A consumer cassette labelled The Last Game. A small child's solitary voice recounting finishing a board game alone in the silent shelter.","displayName":"Cassette: Martinez Family III","durability":0,"empS…`
- Row 713 `cassette_free_radio_1`: `{"contamination":0,"description":"A broadcast tape labelled The First Broadcast. A young woman urges survivors that they are not alone and to stay sheltered from ash.","displayName":"Cassette: Free Radio I","durability":0,"empShielded":fal…`
- Row 714 `cassette_free_radio_2`: `{"contamination":0,"description":"A broadcast tape labelled The Warning. Urgent advisory warning civilians against roving conscription patrols and masked search parties.","displayName":"Cassette: Free Radio II","durability":0,"empShielded"…`
- Row 715 `cassette_free_radio_3`: `{"contamination":0,"description":"A broadcast tape labelled The Coordinates. Slow recitation of hidden supply coordinates at an old municipal library.","displayName":"Cassette: Free Radio III","durability":0,"empShielded":false,"equipSlot"…`
- Row 716 `cassette_free_radio_4`: `{"contamination":0,"description":"A broadcast tape labelled The Last Transmission. A final sign-off affirming faith in surviving relays and quiet resilience across the wasteland.","displayName":"Cassette: Free Radio IV","durability":0,"emp…`
- Row 717 `medical_scissors`: `{"description":"Stainless bandage shears with the hooked tip that slides under dressing without nicking skin. The pivot is still tight. Field medics mark theirs with colored tape; these are unmarked, which means nobody is missing them yet.…`
- Row 718 `protective_rubber_gloves`: `{"contamination":0,"description":"Long-cuff examination gloves in sealed pairs. The latex has not perished, which says the box sat somewhere dry. They go on before anything else and come off inside-out, one folded into the other.","display…`
- Row 719 `sterilised_bandage`: `{"contamination":0,"description":"A pressure dressing in a boiled-autoclave pouch, the indicator strip still showing sterile. Cleaner than most of what passes for clean now.","displayName":"Sterilised Dressing","durability":0,"empShielded"…`
- Row 720 `sandbags`: `{"contamination":0,"description":"Burlap and whatever the river gives. Filled and stacked, they stop water, then bullets, then light. Every wall in this city is two sandbags deep where it matters.","displayName":"Sandbags","durability":0,"…`
- Row 721 `item_pharmacist_ledger`: `{"contamination":0,"description":"A water-stained ledger listing every name ever filled at the pharmacy counter. The handwriting tightens through the back pages, then stops. Carried for the names, not the doses.","displayName":"Dispensary …`
- Row 722 `item_worn_pet_collar`: `{"contamination":0,"description":"A leather collar, buckle greened with age, tag too tarnished to read. It still smells faintly of the dog that wore it. Kept because the wearing meant something.","displayName":"Worn Pet Collar","durability…`
- Row 723 `item_undertakers_register`: `{"contamination":0,"description":"A heavy cloth-bound book. Each page holds a name, a date, and a single honest line. There are no eulogies here, only records — and the records are the eulogy.","displayName":"Burial Register","durability":…`
- Row 724 `item_grandfathers_soldering_iron`: `{"contamination":0,"description":"A heavy brass iron, tip stained black from decades of use. It still heats true. Three hands learned to splice wire with this iron; it is waiting to teach a fourth.","displayName":"Grandfather's Soldering I…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/holdfast_factions.json`

### `Assets/StreamingAssets/Data/holdfast_factions.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 6557; characters: 6557.
- SHA-256: `e2f13c2291cba31c05b7b0dbca06dc37bd9bf5d692e51d9c2250c8738bdcbd44`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `actions`

#### `actions` — 9 current rows

- Row 001 `faction_the_office`: `{"access_rule":"Access plus named claims. You cannot conquer them. You can lose the Ice Road and gain a file. Threat is tone, not a seventh Power.","alignment":"conditional","badge_asset_id":"","display_name":"The Office","home_region":"th…`
- Row 002 `faction_the_cutters`: `{"access_rule":"Dark and lit are moral words. Will not guide a column onto ice marked dark. Will not blast. Relight-for-a-trap is Ivy's exception, northern: lamps out over eleven days, access gone.","alignment":"conditional","badge_asset_i…`
- Row 003 `faction_the_fleet`: `{"access_rule":"Waiting for a stand-up that uses the same authentication family as a land pad. Some paper only works on land. Blasting is refused. The Ridge is hours and cold.","alignment":"peaceful","badge_asset_id":"","display_name":"The…`
- Row 004 `faction_black_flotilla`: `{"access_rule":"Flag traffic is hailed, boarded, and priced. Marked salvage claims outrange weapons; a claim tag counts as a signature on the water. Unmarked deep work is theirs, not yours.","alignment":"conditional","badge_asset_id":"fact…`
- Row 005 `faction_supply_corps`: `{"access_rule":"Allocation-office discipline: credit against the shelter's next issue, terms read aloud twice, forfeit named before the goods move. Default is remembered in files, not grudges.","alignment":"conditional","badge_asset_id":""…`
- Row 006 `faction_railway_guild`: `{"access_rule":"Keeps the southern rail open for counterparties in good standing. Lends fuel, parts, and capital equipment at fair rates; default closes the track, and the track stays closed.","alignment":"conditional","badge_asset_id":"",…`
- Row 007 `faction_hydro_barons`: `{"access_rule":"Control the deep aquifer and everything that filters it. No negotiation on rates, no second contract while the first is open, and no exceptions to the embargo that follows a default.","alignment":"conditional","badge_asset_…`
- Row 008 `faction_ordnance_foundry`: `{"access_rule":"Production-line counterparty for ammunition, tools, and protective equipment. Rates are honest because capable shelters are repeat customers; defaults are collected by enforcers, not letters.","alignment":"conditional","bad…`
- Row 009 `faction_scavengers`: `{"access_rule":"Move goods nobody else will touch, against collateral they inventory themselves. Payment in full, on the day, or they take what they are owed from what you have.","alignment":"conditional","badge_asset_id":"","display_name"…`


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **Current evidence and safe integration boundary for Plan 99: Hardcore Economy Tuning, Scarcity and Price-Shock Contracts.**.

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
