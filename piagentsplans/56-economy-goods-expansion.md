# Plan 56 — Economy Goods Catalog, Market Determinism and Trade Reachability

> **Rebuild status:** COMPLETE 51-GOOD MARKET AUTHORITY — DETERMINISM AND BALANCE MAINTENANCE
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

- The original goal was sensible—make the market deep enough for trade routes and settlement pressure—but current data has exceeded the target and is already consumed by the canonical market, settlement and caravan paths.
- The live contract is goods catalog → `GoodsCatalogLoader` → `MarketSystem` price/demand state → settlement/caravan trade projections → campaign save. Volatility and elasticity are authored parameters, not UI decoration.
- The rebase focuses on bounded prices, seeded replay, supply/demand semantics, reference integrity and truthful player-facing market information. It does not duplicate market state in a goods cache.

**Bounded outcome:** Retire the old 16→40 data-only premise. Current `economy_goods.json` has 51 unique goods, settlement/caravan references are tested, and `MarketSystem` has seeded price evolution and save state. The next package is a bounded market-quality audit, not another goods batch or a new pricing owner.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `economy_goods.json` is present with 51 unique goods and the current Plan56 tests assert the 51-good breadth target, 16 baseline rows, item resolution, settlement wiring and caravan wiring.
- `GoodsCatalogLoader` validates IDs, positive base prices, volatility bounds and positive elasticity; `MarketSystem` owns deterministic daily price evolution and state capture/restore.
- Market price floors/ceilings and fuzz/determinism tests exist; a fresh run remains required before claiming current pass status.
- Settlement and caravan consumers have distinct owners; goods metadata is not a second settlement or caravan authority.

**Master-authority sections applied to this rebase:**

- Master authority Volume 28 verification cookbook: focused evidence before broad gates.
- Lane D save/state/compatibility guidance: owner DTOs, migration and restore proof.
- Lane E UI/UX/accessibility guidance: truthful projections and keyboard/controller lifecycle.
- Lane G testing guidance: smallest affected target, negative cases and deterministic replay.
- Anti-padding protocol: content exhaustion may end the plan before the character checkpoint.
- Volume 32 data-authority and market fact guidance.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 16→40 target with a 51-row current market census.
- Document price floor/ceiling, demand multiplier, volatility and elasticity semantics.
- Preserve seeded replay and save round-trip as non-negotiable contracts.
- Audit every new good for a current item/settlement/caravan consumer and a non-degenerate economic role.

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
| goods metadata and validation | GoodsCatalogLoader | `Assets/Ashfall.Core/Economy/GoodsCatalog.cs` | Sole goods catalog loader. |
| prices, demand, shocks and market state | MarketSystem | `Assets/Ashfall.Core/Economy/MarketSystem.cs` | Owns market mutation and save state. |
| settlement profiles and route cargo | Settlement/caravan trade owners | `Assets/Ashfall.Core/World/SettlementCatalog.cs; Assets/Ashfall.Core/TravelingCaravanSystem.cs` | Consume canonical goods; do not copy prices. |
| breadth, references and determinism | Market focused tests | `Ashfall.Core.Tests/Plan56EconomyGoodsTests.cs; Ashfall.Core.Tests/MarketPriceDeterminismTests.cs` | Executable current proof. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Economy Goods Catalog, Market Determinism and Trade Reachability
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ GoodsCatalogLoader
│   goods metadata and validation
│ MarketSystem
│   prices, demand, shocks and market state
│ Settlement/caravan trade owners
│   settlement profiles and route cargo
│ Market focused tests
│   breadth, references and determinism
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

1. **Preserve current state ownership.** GoodsCatalogLoader owns goods metadata and validation: Sole goods catalog loader.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| goods metadata and validation | GoodsCatalogLoader | `Assets/Ashfall.Core/Economy/GoodsCatalog.cs` | Sole goods catalog loader. |
| prices, demand, shocks and market state | MarketSystem | `Assets/Ashfall.Core/Economy/MarketSystem.cs` | Owns market mutation and save state. |
| settlement profiles and route cargo | Settlement/caravan trade owners | `Assets/Ashfall.Core/World/SettlementCatalog.cs; Assets/Ashfall.Core/TravelingCaravanSystem.cs` | Consume canonical goods; do not copy prices. |
| breadth, references and determinism | Market focused tests | `Ashfall.Core.Tests/Plan56EconomyGoodsTests.cs; Ashfall.Core.Tests/MarketPriceDeterminismTests.cs` | Executable current proof. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load/validate goods
2. bind the canonical catalog to MarketSystem
3. read campaign demand and authored shocks
4. advance seeded daily prices with bounds
5. project market/settlement/caravan views
6. execute trade through canonical inventory/economy commands
7. capture/restore market state

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Catalog definitions are immutable; current prices, demand, shocks and embargoes are market state.
- Price evolution is bounded by the current floor/ceiling contract.
- Elasticity scales response; volatility controls random-walk amplitude; neither is a hidden difficulty multiplier.
- Restore must preserve current market state without rerunning a price tick or consuming RNG.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A good must resolve to a canonical item when the current contract requires an item-backed trade good.
- Base price, volatility and elasticity are finite and within loader ranges.
- Same seed, catalog and state produce the same price trajectory.
- A failed trade never partially mutates inventory or market state.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `economy_goods.json` is the sole goods authority.
- No new parallel price table or caravan-specific copy.
- A new row needs a category, economic role, item reference where required, and a consumer.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the existing economy save section and market state.
- No new save section is justified by goods content.
- Legacy market state must restore with current version semantics and safe price bounds.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- All stochastic price movement uses the injected seeded RNG.
- Catalog iteration order is stable and not hash-dependent.
- Paired runs compare price trajectories, not only final price.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Market day ticks and price explanations are emitted by the current market owner.
- Settlement/caravan trade commands return through the existing economy owner.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Main.Economy.cs
- src/Economy/EconomyMarketPanel.cs
- src/UI/TravelingCaravanPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Trade notes are brief, grounded and fictional.
- Goods should express scarcity and regional pressure without promising a guaranteed arbitrage.
- Luxury, necessity and salvage profiles must remain legible in the market view.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A good is listed but not resolvable or consumed. | GoodsCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A price leaves the floor/ceiling or becomes NaN. | MarketSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A UI quote differs from the owner quote. | Settlement/caravan trade owners | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A caravan copies market state and diverges after restore. | Market focused tests | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new row duplicates an existing economic role. | GoodsCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Plan56EconomyGoodsTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/MarketPriceDeterminismTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/TravelingCaravanSystemTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — market census | Read current goods, loader, market and consumers. | 51 rows and current owner are proven. | No production path until the owning implementation package is separately claimed. |
| 1 — economic matrix | Audit categories, ranges, references and trade roles. | No orphan or duplicate economic role. | No production path until the owning implementation package is separately claimed. |
| 2 — replay/save proof | Verify seeded trajectories, bounds and restore. | Continuous and restored runs match. | No production path until the owning implementation package is separately claimed. |
| 3 — balance review | Review floors, ceilings, elasticity and player-facing explanations. | No opaque or unbounded market behavior remains. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/economy_goods.json | READ ONLY; MODIFY only for proven gap | 51-row authority |
| Assets/Ashfall.Core/Economy/GoodsCatalog.cs | READ ONLY | Loader/validation |
| Assets/Ashfall.Core/Economy/MarketSystem.cs | READ ONLY | Market owner |
| src/Economy/EconomyMarketPanel.cs | READ ONLY | Presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Copying market prices into settlement or caravan state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Using unseeded randomness in price evolution. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding floating-point values without finite checks. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Treating row count as economic balance proof. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new goods for this rebase.
- No new pricing subsystem.
- No economy save-section change.
- No production edits in this rebase.

# 23. Rollback and Recovery

- Revert the plan file.
- Future data/code changes retain the previous catalog/save fixture and focused market tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 51 current goods and their consumers are named.
- Determinism, bounds, save and trade contracts are explicit.
- No parallel market authority is proposed.
- Focused market and consumer commands are listed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 16→40 target with a 51-row current market census.
- Document price floor/ceiling, demand multiplier, volatility and elasticity semantics.
- Preserve seeded replay and save round-trip as non-negotiable contracts.
- Audit every new good for a current item/settlement/caravan consumer and a non-degenerate economic role.

## MUST NOT DO

- No new goods for this rebase.
- No new pricing subsystem.
- No economy save-section change.
- No production edits in this rebase.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Plan56EconomyGoodsTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/MarketPriceDeterminismTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/TravelingCaravanSystemTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — market census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: goods metadata and validation → GoodsCatalogLoader; prices, demand, shocks and market state → MarketSystem; settlement profiles and route cargo → Settlement/caravan trade owners; breadth, references and determinism → Market focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 56.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 56 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by GoodsCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Economy/GoodsCatalog.cs`

### `Assets/Ashfall.Core/Economy/GoodsCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 355 lines / 15912 bytes.
- SHA-256: `07d30b18b404439dac8d92e4d6ee87e0b1e65e9784374206c0117ef0b3f1836c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class GoodCategories
public static readonly string[] Known = {
public static bool IsKnown(string category) {
public static string DisplayName(string category) {
public static string CategoryDescription(string category) {
public class GoodDefinition
public string id = string.Empty;
public string displayName = string.Empty;
public string category = "misc";
public float basePrice = 1f;
public float volatility = 0.1f;   // 0..1: daily price noise amplitude
public float elasticity = 1f;     // > 0: how strongly demand moves price
public int stackSize = 10;
public float weightKg = 1f;
public string barterNote = string.Empty; // optional barter-relevant metadata
public string regionalSupply = string.Empty; // optional production-source provenance (Plan 56 follow-up)
public string Description =>
public class GoodsCatalogLoadResult
public List<GoodDefinition> Goods { get; } = new List<GoodDefinition>();
public List<string> Errors { get; } = new List<string>();
public bool HasErrors => Errors.Count > 0;
public class GoodsCatalog
public IReadOnlyDictionary<string, GoodDefinition> ById => _byId;
public int Count => _byId.Count;
public GoodDefinition Find(string id) {
public IReadOnlyList<GoodDefinition> All() {
internal void Add(GoodDefinition def) => _byId[def.id] = def;
public static class GoodsCatalogLoader
public const string FileName = "economy_goods.json";
public const int CurrentSchemaVersion = 1;
public static GoodsCatalogLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public int schema_version = 1;
public List<RawGoodDefinition> goods = new List<RawGoodDefinition>();
public string id;
public string displayName;
public string category;
public float? basePrice;
public float? volatility;
public float? elasticity;
public int? stackSize;
public float? weightKg;
public string barterNote;
public string regionalSupply;
public static GoodsCatalog ToCatalog(GoodsCatalogLoadResult load) {
internal static bool IsSnakeCase(string id) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Economy/MarketSystem.cs`

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


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/TravelingCaravanSystem.cs`

### `Assets/Ashfall.Core/TravelingCaravanSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 518 lines / 22717 bytes.
- SHA-256: `9c7a92b23c61fab7d3f7b434eb3a9d78773ce6c37d2e5de21cd3d1723ccbc349`.
- Architecture signals: seeded references=2; save/restore symbols=3; typed event declarations=11; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class CaravanInventoryItem
public string itemId;
public int quantity;
public int priceRations;
public class CaravanEntry
public string caravanId;
public string caravanName;
public string factionId;
public string originRegion; // flotilla, foundry, greenhouse, settlement, traplines
public string currentNodeId;
public List<string> routeNodeIds = new List<string>();
public int routeIndex = 0;
public int daysAtCurrentNode = 0;
public int stayDurationDays = 2;
public int guardCount = 4;
public bool isRobbed = false;
public bool embargoBlocked = false;
public List<CaravanInventoryItem> inventory = new List<CaravanInventoryItem>();
public class TravelingCaravanState
public List<CaravanEntry> activeCaravans = new List<CaravanEntry>();
public int completedTradesCount = 0;
public class TravelingCaravanSystem
public const string SystemId = "traveling_caravan_system";
public GoodsCatalog? Catalog { get; set; }
public Narrative.TravelEncounterSystem? TravelEncounters { get; set; }
public TradeEmbargoSystem? Embargoes { get; set; }
public WastelandMapSystem? Map { get; set; }
public event Action<CaravanEntry, string>? OnCaravanArrivedAtNode;
public event Action<CaravanEntry, string, int>? OnTradeCompleted;
public event Action<CaravanEntry, Narrative.TravelEncounterDefinition>? OnCaravanPatrolEncountered;
public event Action<CaravanEntry, string>? OnCaravanEmbargoed;
public event Action<CaravanEntry>? OnCaravanResumed;
public TravelingCaravanState State => _state;
public int CaravanCount => _state.activeCaravans?.Count ?? 0;
public void SpawnCaravan(string caravanId, string name, string factionId, List<string> route, string originRegion = "settlement") {
public CaravanEntry? GetCaravanAtNode(string nodeId) {
public void DailyTick() => DailyTick(0, null);
public Func<WeatherKind, float>? WeatherAvailabilityProvider { get; set; }
public void DailyTick( int currentDay, ISeededRng? rng = null, string defaultRegion = "the_toll", int dangerLevel = 2, string season = "all",
public Narrative.TravelEncounterDefinition? CheckRouteEncounter( CaravanEntry caravan, string region, int dangerLevel, string season, int currentDay,
public bool ResolveRouteEncounterChoice( string encounterId, string choiceId, int currentDay, out Narrative.TravelEncounterResolutionResult? result) {
public bool TryBuyItem(string caravanId, string itemId, int amount, ref int playerRations) {
public TravelingCaravanState CaptureState() {
public void RestoreState(TravelingCaravanState state) {
```


# Appendix B.05 — Current Code Architecture: `src/Main.Economy.cs`

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


# Appendix B.06 — Current Code Architecture: `src/Economy/EconomyMarketPanel.cs`

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


# Appendix B.07 — Current Code Architecture: `src/UI/TravelingCaravanPanel.cs`

### `src/UI/TravelingCaravanPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 465 lines / 22942 bytes.
- SHA-256: `1ac34a6c364a547c4c51833e595c3ca5985ac49c0b465134cdb044df5ecce8e0`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=3; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class TravelingCaravanPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _host != null;
public string CurrentTraderProfileId { get; private set; } = string.Empty;
public void Bind( TravelingCaravanHostSession session, TradeVoiceResolver? voiceResolver = null, Func<WeatherKind>? weatherProvider = null) {
public override void _Ready() {
public void Open() {
public void RefreshView() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/economy_goods.json`

### `Assets/StreamingAssets/Data/economy_goods.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 21607 bytes / 21605 characters.
- SHA-256: `18c8a68c9479179b68557cc28bc9fbce71a988e1d5c2a130c9bcf47c20a0739b`.
- Root keys: `goods`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
goods: min=51, max=51, observed_paths=1
```

Representative record fields:

- `barterNote`
- `basePrice`
- `category`
- `displayName`
- `elasticity`
- `id`
- `regionalSupply`
- `stackSize`
- `volatility`
- `weightKg`

Representative identifiers (ordered, capped for readability):

```text
clean_water
scrap_metal
bandages
antibiotics
iodine_pills
fuel
9mm_ammo
crowbar
gas_mask
dosimeter
canned_food
diamond
coal
item_foundry_brine_pipe
item_foundry_ice_anchor
item_foundry_winch_drum
cooked_meat
water_filter
air_filter
item_frostbite_salve
seed_packets
chemicals
electronic_scrap
mechanical_parts
solar_cell
medical_kit
anti_rad
item_desal_membrane
item_prussian_blue_chelating_pellets
item_lead_shielded_sample_cask
item_ro_membrane
trap_improvised_wire
trap_box
trap_fish
ammo_556
ammo_12g
diesel_fuel
item_smoked_meat
item_pickled_tubers
tobacco_pouch
item_logistics_cipher_sheet
sealed_government_document
weapon_sidearm
weapon_pipe_shotgun
item_taper_kit_opioid
duct_tape
rope
item_cassette_tape
cloth
item_air_filter_hepa
crop_oilseed
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/items.json`

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


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/caravans.json`

### `Assets/StreamingAssets/Data/caravans.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 4154 bytes / 4154 characters.
- SHA-256: `0cb331e654f547f6f642e6780f3594d933bffd5437724e456f73d37ed42a1fac`.
- Root keys: `caravans`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
caravans: min=4, max=4, observed_paths=1
caravans[].route_node_ids: min=5, max=5, observed_paths=2
caravans[].specialty_goods: min=5, max=5, observed_paths=2
```

Representative record fields:

- `caravan_id`
- `faction_id`
- `guard_count`
- `name`
- `origin_region`
- `route_node_ids`
- `specialty_goods`
- `stay_duration_days`


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Plan56EconomyGoodsTests.cs`

### `Ashfall.Core.Tests/Plan56EconomyGoodsTests.cs`

- Current test declarations: Fact=15, Theory=0, InlineData=0.
- File lines: 311; SHA-256: `aba9b873a4fa9a17f480f310cf890aec8900428ca469141ae3b22a57758673e3`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_reaches_the_51_good_breadth_target
Loader_reports_zero_validation_errors
All_plan56_goods_present_with_canonical_item_ids
All_sixteen_baseline_goods_are_preserved
Plan56_goods_resolve_to_canonical_items
Plan56_goods_use_valid_categories_and_bands
Tobacco_is_the_high_volatility_high_elasticity_luxury_contrast
Ammo_and_diesel_prices_fit_between_existing_anchors
Preserved_foods_are_more_stable_than_fresh_meat
Settlements_wire_all_six_plan56_goods
Settlements_never_export_their_own_needs
Caravans_carry_five_plan56_goods
Seeded_price_trace_is_deterministic_for_all_40_goods
Volatility_and_elasticity_produce_differentiated_behavior
Price_floor_and_ceiling_hold_under_extreme_demand_shocks
```


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/MarketPriceDeterminismTests.cs`

### `Ashfall.Core.Tests/MarketPriceDeterminismTests.cs`

- Current test declarations: Fact=5, Theory=1, InlineData=3.
- File lines: 168; SHA-256: `209355051a87c3687f6a12a5b46f91fc0a663bdab400f1f0f436efa27bdc4b7c`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SameSeed_Determinism_PriceTrajectoryIdentical
DifferentSeed_Divergence_Allowed_ButWithinBounds
NumericalSafety_NoNaNOrInfinityOrNegative
PriceExplosion_BoundedByCeiling
AllGoods_Determinism_And_Bounds
FuzzCatalog_AllGoods_Determinism_And_Bounds
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/TravelingCaravanSystemTests.cs`

### `Ashfall.Core.Tests/TravelingCaravanSystemTests.cs`

- Current test declarations: Fact=10, Theory=1, InlineData=2.
- File lines: 226; SHA-256: `b29870657e2d1f44edb5e1c52d2507e11a9ac78556e0149731fdd6b0c10f369e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SpawnCaravan_AddsCaravanToStateAndFiresEvent
Constructor_CapturedState_DoesNotAliasInput
DailyTick_MalformedRouteAndNullEntry_FailClosed
SpawnCaravan_DuplicateIdOrBlankRoute_DoesNotCreateSecondAuthority
DailyTick_AdvancesWaypointsAndLoopsRoute
TryBuyItem_DeductsStockAndRations_FailsWhenInsufficient
TryBuyItem_NonPositiveAmount_DoesNotMutate
TryBuyItem_OverflowingPrice_FailsClosed
CaptureState_ReturnsDeepCopySnapshot
RestoreState_RestoresCaravansAndTradeCount
RestoreState_DoesNotAliasEnvelopeCollections
```


# Appendix E.14 — Supporting Code Evidence: `src/Host/HostCli.PanelTests.cs`

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


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Economy/CommodityBaselineCatalog.cs`

### `Assets/Ashfall.Core/Economy/CommodityBaselineCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 251 lines / 10410 bytes.
- SHA-256: `8bea652d277f4f0ee0e3a8a70dbd1ad635977207c86dac841b32cdedcf48a19e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CommodityBaselineDefinition
public string category_id = string.Empty;
public int base_multiplier_permille = 1000;
public string elasticity_class = "medium";
public int scarcity_floor_permille = 700;
public int scarcity_ceiling_permille = 2000;
public sealed class CommodityBaselineLoadResult
public List<CommodityBaselineDefinition> Categories { get; } = new List<CommodityBaselineDefinition>();
public List<string> Errors { get; } = new List<string>();
public bool HasErrors => Errors.Count > 0;
public sealed class CommodityBaselineCatalog
public IReadOnlyDictionary<string, CommodityBaselineDefinition> ByCategoryId => _byCategoryId;
public int Count => _byCategoryId.Count;
public CommodityBaselineDefinition? Find(string categoryId) {
public const int PermilleScale = 1000;
internal void Add(CommodityBaselineDefinition def) => _byCategoryId[def.category_id] = def;
public static class CommodityBaselineCatalogLoader
public const string FileName = "commodity_baselines.json";
public const int CurrentSchemaVersion = 1;
public static readonly IReadOnlyList<string> AcceptedElasticityClasses = new[] { "low", "medium", "high" };
public const int MinMultiplierPermille = 200;
public const int MaxMultiplierPermille = 5000;
public static CommodityBaselineLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static bool IsAcceptedElasticity(string elasticity) {
public static CommodityBaselineCatalog ToCatalog(CommodityBaselineLoadResult load) {
public int schema_version = 1;
public List<RawCommodityBaseline> categories = new List<RawCommodityBaseline>();
public string category_id;
public int? base_multiplier_permille;
public string elasticity_class;
public int? scarcity_floor_permille;
public int? scarcity_ceiling_permille;
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/Economy/BlackMarketSystem.cs`

### `Assets/Ashfall.Core/Economy/BlackMarketSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 892 lines / 40636 bytes.
- SHA-256: `e9e7c168248aaad33090197d250a614bbbf2b21410f9727ea101c8812218c877`.
- Architecture signals: seeded references=1; save/restore symbols=4; typed event declarations=18; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class UnderworldLedgerState
public string syndicateId = string.Empty;
public bool discovered = false;
public int discoveredDay = -1;
public float trust = 0f;          // 0..100 (DebtReliability + standing mirror)
public float heat = 0f;           // 0..100 (attention from patrols/collectors)
public int accessTier = 0;        // 0 = unknown, 1..3 after discovery
public int lastStockRefreshDay = -1;
public sealed class UnderworldDebtRecord
public const string StatusActive = "active";
public const string StatusRepaid = "repaid";
public const string StatusDefaulted = "defaulted";
public string debtId = string.Empty;
public string syndicateId = string.Empty;
public float principalUnits = 0f;
public float repaidUnits = 0f;
public int interestBp = 0;        // on top of the outstanding remainder at due day
public int issuedDay = 0;
public int dueDay = 1;
public string status = StatusActive;
public string reason = string.Empty;
public sealed class BlackMarketStockLine
public string entryId = string.Empty;
public int quantity = 0;
public int generatedDay = 0;
public sealed class BlackMarketState
public const int CurrentVersion = 1;
public int schemaVersion = CurrentVersion;
public List<UnderworldLedgerState> syndicates = new List<UnderworldLedgerState>();
public List<UnderworldDebtRecord> debts = new List<UnderworldDebtRecord>();
public List<BlackMarketStockLine> stock = new List<BlackMarketStockLine>();   // flat snapshot; keyed by (syndicate, entry)
public List<string> stockOwners = new List<string>();                        // syndicateId per stock line (parallel list)
public List<string> firedEventKeys = new List<string>();                     // debt-event idempotency
public int lastTickDay = -1;
public Factions.FactionBountySystemState? factionBounties = null;
public readonly struct BlackMarketQuote
public readonly bool Valid;
public readonly string EntryId;
public readonly int Quantity;
public readonly float UnitPrice;
public readonly float CanonicalUnitPrice;
public readonly float TotalValue;
public readonly string RejectReason;
public readonly struct BlackMarketLoanQuote
public readonly bool Valid;
public readonly string SyndicateId;
public readonly float Units;
public readonly int DurationDays;
public readonly int DueDay;
public readonly int InterestBp;
public readonly string RejectReason;
public readonly struct BlackMarketRepayQuote
public readonly bool Valid;
public readonly string DebtId;
public readonly float RequestedUnits;
public readonly float AppliedUnits;
public readonly float OutstandingUnits;
public readonly bool CompletesDebt;
public readonly string RejectReason;
public sealed class BlackMarketSystem
public const string SystemId = "black_market";
public const float TrustDiscountAtMax = 0.20f;
public const int MinEffectivePremiumBp = 12500 - 10000;   // ≥ +25% over canonical
public const float HeatMax = 100f;
public const float TrustMax = 100f;
public event Action<string>? OnContactDiscovered;          // syndicateId
public event Action<string, int, int>? OnStockRefreshed;   // syndicateId, day, lineCount
public event Action<UnderworldDebtRecord>? OnDebtIssued;
public event Action<UnderworldDebtRecord, float>? OnDebtRepaid;   // debt, amountRepaid
public event Action<UnderworldDebtRecord>? OnDebtOverdue;
public event Action<UnderworldDebtRecord>? OnBountyPlaced;
public event Action<BlackMarketState>? OnStateChanged;
public BlackMarketState State => _state;
public void BindCatalog(BlackMarketInventoryCatalog catalog) {
public void BindMarket(MarketSystem market) {
public void BindFactionBountySystem(FactionBountySystem bounties) {
public IReadOnlyList<string> ValidationErrors { get; private set; } = Array.Empty<string>();
public BlackMarketInventoryCatalog Catalog => _catalog;
public UnderworldLedgerState EnsureLedger(string syndicateId) {
public UnderworldLedgerState? FindLedger(string syndicateId) {
public bool DiscoverContact(string syndicateId, int day) {
public bool IsContactDiscovered(string syndicateId) =>
public IReadOnlyList<string> DiscoveredContacts =>
public IReadOnlyList<BlackMarketStockLine> EnsureStockSnapshot(string syndicateId, int day, ISeededRng rng) {
public IReadOnlyList<BlackMarketStockLine> GetStock(string syndicateId) {
public BlackMarketStockLine? GetStockLine(string syndicateId, string entryId) {
public float GetBuyPrice(string syndicateId, BlackMarketEntryDefinition entry) {
public float GetSellPrice(string syndicateId, BlackMarketEntryDefinition entry) {
public BlackMarketQuote PreviewBuy(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketQuote PreviewSell(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketQuote Buy(string syndicateId, string entryId, int quantity, int day) =>
public BlackMarketQuote Buy(string syndicateId, string entryId, int quantity, int day, Func<BlackMarketQuote, bool>? settle) {
public BlackMarketQuote Sell(string syndicateId, string entryId, int quantity, int day) =>
public BlackMarketQuote Sell(string syndicateId, string entryId, int quantity, int day, Func<BlackMarketQuote, bool>? settle) {
public BlackMarketLoanQuote PreviewLoan(string syndicateId, float units, int day, int durationDays) {
public UnderworldDebtRecord? TakeLoan(string syndicateId, float units, int day, int durationDays) =>
public UnderworldDebtRecord? TakeLoan(string syndicateId, float units, int day, int durationDays, Func<BlackMarketLoanQuote, bool>? settle) {
public float OutstandingOnDebt(UnderworldDebtRecord debt) {
public BlackMarketRepayQuote PreviewRepay(string debtId, float amount, int day) {
public bool RepayDebt(string debtId, float amount, int day) =>
public bool RepayDebt(string debtId, float amount, int day, Func<BlackMarketRepayQuote, bool>? settle) {
public void TickDaily(int day) {
public BlackMarketState CaptureState() {
public void RestoreState(BlackMarketState? saved) {
```


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs`

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


# Appendix G.19 — Supporting Regression Evidence: `Ashfall.Core.Tests/EconomyProbeTests.cs`

### `Ashfall.Core.Tests/EconomyProbeTests.cs`

- Current test declarations: Fact=14, Theory=0, InlineData=0.
- File lines: 325; SHA-256: `052abf0700a3d3a78f3980e6516983cfc54efe2064e1ef801552f34f28e0e3ad`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Probe_SeedFuzz_100Seeds200Ticks_BoundsHoldNoExceptions
Probe_SeedFuzz_WithInterleavedTransactions_NoExceptions
Probe_LedgerConservation_NoNegativeOrNaNValues
Probe_TruncatedSave_ThrowsAndHostPathReturnsNull
Probe_MissingFieldsSave_MigratesPredictably
Probe_DayZeroTick_DoesNotCorrupt
Probe_EmptyCatalog_TickAndTransactSafe
Probe_SingleGoodCatalog_StaysBounded
Probe_BarterRemainderBothDirections
Probe_BarterWithUntrackedGoods_Rejected
Probe_ReloadContinuity_HashMatchesUninterruptedRun
Probe_ClampSaturation_BindsExactlyAndStays
Probe_ForeignDemandRows_DoNotCorruptCatalogPrices
Probe_HugeQuantityTransaction_NoOverflow
```


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/Economy/EconomyHostSessionTests.cs`

### `Ashfall.Core.Tests/Economy/EconomyHostSessionTests.cs`

- Current test declarations: Fact=14, Theory=0, InlineData=0.
- File lines: 351; SHA-256: `1dd5f19b286ef1bdc80c7dfe3e5d5917a459f0288ef279323535d4c41b0d7941`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
MarketSystem_TickDays_AdvancesDayAndTickCount
MarketSystem_TickDays_ZeroOrNegativeDays_IsNoOp
MarketSystem_TickDays_DeterministicPricingAcrossInstances
MarketSystem_LoadCatalog_ValidDirectory_BindsKnownGoods
MarketSystem_LoadCatalog_InvalidOrEmpty_DoesNotThrow
EconomyHostSession_TickDemo_AdvancesDayTicksAndRaisesStateChanged
EconomyHostSession_TickDemo_ZeroDays_UpdatesEventWithoutAdvancingDay
EconomyHostSession_LoadData_BindsCatalogFromDataDir
EconomyHostSession_BarterDemo_Accepted_ExchangesGoodsUpdatesLastEventAndRaisesStateChanged
EconomyHostSession_BarterDemo_UnknownGiveItem_ReturnsRejectionMessage
EconomyHostSession_BarterDemo_UnknownTakeItem_ReturnsRejectionMessage
EconomyHostSession_BarterDemo_ZeroOrNegativeQuantity_ReturnsRejectionMessage
EconomyHostSession_BarterDemo_TakeGoodTooValuable_ReturnsRejectionMessage
EconomyHostSession_BarterDemo_WithRealCatalog_ExecutesSuccessfully
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/Economy/Plan212DynamicEconomyTests.cs`

### `Ashfall.Core.Tests/Economy/Plan212DynamicEconomyTests.cs`

- Current test declarations: Fact=18, Theory=0, InlineData=0.
- File lines: 433; SHA-256: `03d48d979e7ce2afaa55bc8489da43a53f1ac8db9ca36ae10e97e5b0e30149a4`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
NoCommodityCatalog_GetPrice_IsIdenticalToLegacyPath
NoCommodityCatalog_IndexAtNeutral_NoFactorRowsEmitted
CategoryIndex_TrackedCategory_ScalesPrice
TickDay_IndexPullsTowardBaselineTarget_WithBoundedSmoothing
TickDay_IndexStaysWithinAuthoredBounds_AndConvergesToBaselineTarget
ApplyShock_Shortage_RaisesPrice_Crash_LowersPrice
ApplyShock_SeverityClamps_AndUnknownCategoryRejected
ApplyShock_IsIdempotentPerCategoryKindSource
Shock_ExpiresDeterministically_Once
Buy_Sell_RecordOpposingPressure_BarterSplitsLegs
BuyPressure_RaisesTarget_SellPressure_LowersTarget
Pressure_DecaysDaily_TowardZero
Arbitrage_RepeatedBuySellLoops_NeverCompound
CaptureRestore_CurrentVersion_RoundTripsExactly
Restore_V1LegacySave_MigratesNeutral_NeutralNotRunaway
Restore_NewerVersion_ThrowsLoudly
PairedRun_SameSeedSameInputs_IdenticalPrices
TickDay_TicksExactlyOnce_PerCall_NoDoubleAdvance
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/EconomySystemTests.cs`

### `Ashfall.Core.Tests/EconomySystemTests.cs`

- Current test declarations: Fact=35, Theory=0, InlineData=0.
- File lines: 531; SHA-256: `dea0edb96080a8c21b8bafc6e5323ff40e0a6ddfc5076e19b2e44a4d1cecd6c0`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ValidGoods_LoadWithoutErrors
DuplicateIds_AreErrors
MissingRequiredFields_AreErrors
InvalidRanges_AreErrors
UnknownCategory_IsError
NonSnakeCaseId_IsError
MalformedJson_IsError
MissingFile_IsError
Catalog_AllSortedOrdinal
Price_BoundsInvariant_AllTicks
Demand_StaysWithinUnityClamps
DeterministicReplay_SameSeedSameTrajectory
DifferentSeeds_Diverge
SaveLoad_RoundTripEquality
SaveLoad_ResumesIdenticalTrajectory
CorruptState_NewerVersionFailsLoudly
CorruptState_OldVersionMigratesPredictably
Transactions_BookAtCurrentPrice
Transactions_UnknownOrZeroRejected
Barter_ExchangesEqualValue
Barter_TooValuableRejected
Barter_NonExactRatio_KeepsEqualLedgerAndReportsRemainder
RestoreState_DeduplicatesDemandRows
TickDay_RaisesEconomyChanged
CaptureState_ReturnsSnapshotNotLiveState
SaveLoad_ChecksumStable
IsSuppliesShort_UnityThresholdParity
Ledger_Conservation_BarterLegsAreEqualValue
Load_ValidJson_ReturnsSuccess
Load_EmptyJson_ReturnsFailure
Load_MalformedJson_ReturnsFailure
Overlay_Default_ReturnsUnityParity
Overlay_AppliedBundle_ReturnsScarcityMultiplier
Overlay_AppliedBundle_FactionPreferenceFound
Overlay_AppliedBundle_PriceShockFoundWithinDuration
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
| goods metadata and validation | GoodsCatalogLoader | prices, demand, shocks and market state | MarketSystem | Owner emits/reads a typed fact; no mirror state. |
| goods metadata and validation | GoodsCatalogLoader | settlement profiles and route cargo | Settlement/caravan trade owners | Owner emits/reads a typed fact; no mirror state. |
| goods metadata and validation | GoodsCatalogLoader | breadth, references and determinism | Market focused tests | Owner emits/reads a typed fact; no mirror state. |
| prices, demand, shocks and market state | MarketSystem | goods metadata and validation | GoodsCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| prices, demand, shocks and market state | MarketSystem | settlement profiles and route cargo | Settlement/caravan trade owners | Owner emits/reads a typed fact; no mirror state. |
| prices, demand, shocks and market state | MarketSystem | breadth, references and determinism | Market focused tests | Owner emits/reads a typed fact; no mirror state. |
| settlement profiles and route cargo | Settlement/caravan trade owners | goods metadata and validation | GoodsCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| settlement profiles and route cargo | Settlement/caravan trade owners | prices, demand, shocks and market state | MarketSystem | Owner emits/reads a typed fact; no mirror state. |
| settlement profiles and route cargo | Settlement/caravan trade owners | breadth, references and determinism | Market focused tests | Owner emits/reads a typed fact; no mirror state. |
| breadth, references and determinism | Market focused tests | goods metadata and validation | GoodsCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| breadth, references and determinism | Market focused tests | prices, demand, shocks and market state | MarketSystem | Owner emits/reads a typed fact; no mirror state. |
| breadth, references and determinism | Market focused tests | settlement profiles and route cargo | Settlement/caravan trade owners | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 16→40 target with a 51-row current market census. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Document price floor/ceiling, demand multiplier, volatility and elasticity semantics. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Preserve seeded replay and save round-trip as non-negotiable contracts. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Audit every new good for a current item/settlement/caravan consumer and a non-degenerate economic role. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.555 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs`

### `Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 669 lines / 28319 bytes.
- SHA-256: `0674cafba15eb65c0bc6d9e87b5573a0c37ed5363e41d5dabe9c8e752ddac63e`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=12; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum CaravanStatus
public enum CaravanHazardOutcome
public sealed class CaravanManifestState
public string manifest_id { get; set; } = string.Empty;
public string route_id { get; set; } = string.Empty;
public string faction_id { get; set; } = string.Empty;
public CaravanStatus status { get; set; } = CaravanStatus.Scheduled;
public int departure_origin_day { get; set; }
public int expected_arrival_day { get; set; }
public int actual_arrival_day { get; set; }
public int departure_day { get; set; }
public int transit_progress_days { get; set; }
public int escort_guard_strength { get; set; }
public bool hazard_resolved { get; set; }
public CaravanHazardOutcome hazard_outcome { get; set; } = CaravanHazardOutcome.None;
public Dictionary<string, int> stocks { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
public int stay_duration_days { get; set; } = 3;
public sealed class CaravanSupplyDisruptionState
public string source_id { get; set; } = string.Empty;
public string faction_id { get; set; } = string.Empty;
public float magnitude { get; set; }
public int start_day { get; set; }
public int end_day { get; set; }
public sealed class CaravanTradeNetworkSave
public string systemId { get; set; } = "caravan_trade_network";
public int schema_version { get; set; } = 1;
public int last_tick_day { get; set; } = 1;
public List<CaravanManifestState> caravans { get; set; } = new List<CaravanManifestState>();
public Dictionary<string, int> faction_profitable_trades { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
public List<string> favored_factions { get; set; } = new List<string>();
public List<CaravanSupplyDisruptionState> supply_disruptions { get; set; } = new List<CaravanSupplyDisruptionState>();
public sealed class BarterTransactionResult
public bool Success { get; set; }
public string FailureReason { get; set; } = string.Empty;
public float OfferedValue { get; set; }
public float RequestedValue { get; set; }
public bool UnlockedFavoredStatus { get; set; }
public static BarterTransactionResult Fail(string reason) =>
public sealed class CaravanTradeNetworkSystem
public const string SystemId = "caravan_trade_network";
public const int TradesRequiredForFavoredStatus = 5;
public const float FavoredTariffDiscount = 0.15f; // 15% tariff reduction
public const float ImportDemandMultiplier = 1.50f; // +50% demand premium
public const float ExportSurplusMultiplier = 0.70f; // -30% surplus discount
public event Action<CaravanManifestState>? OnCaravanScheduled;
public event Action<CaravanManifestState>? OnCaravanArrived;
public event Action<CaravanManifestState, CaravanHazardOutcome>? OnCaravanHazardResolved;
public event Action<CaravanManifestState>? OnCaravanDeparted;
public event Action<string, float, float>? OnTradeCompleted; // faction, offeredVal, requestedVal
public event Action<string>? OnFavoredBarterStatusUnlocked; // factionId
public WastelandMapSystem? Map { get; set; }
public IReadOnlyList<CaravanRouteDefinition> Routes => _routes;
public IReadOnlyList<CaravanManifestState> Caravans => _state.caravans;
public IReadOnlyDictionary<string, int> FactionTrades => _state.faction_profitable_trades;
public IReadOnlyList<string> FavoredFactions => _state.favored_factions;
public bool HasFavoredStatus(string factionId) =>
public bool TryApplySupplyDisruption( string factionId, float magnitude, int startDay, int endDay, string sourceId)
public float GetActiveSupplyDisruptionMagnitude(string factionId, int day) {
public int GetProfitableTradeCount(string factionId) =>
public CaravanManifestState? FindActiveCaravanForRoute(string route_id) {
public CaravanManifestState? FindManifest(string manifestId) {
public CaravanManifestState ScheduleCaravan(string routeId, int currentDay, int escortStrength = 0) {
public float CalculateItemBuyPrice(CaravanManifestState manifest, string itemId, float crisisMultiplier = 1.0f) {
public float CalculateItemSellPrice(CaravanManifestState manifest, string itemId, float crisisMultiplier = 1.0f) {
public BarterTransactionResult ExecuteBarter( string manifestId, Dictionary<string, int> playerOffered, Dictionary<string, int> playerRequested, float crisisMultiplier = 1.0f) {
public void TickDay(int day) {
public void SetItemValueResolver(Func<string, float>? resolver) => _itemValueResolver = resolver;
public void SetTreatyPriceReliefProvider(Func<string, float>? provider) =>
public float GetTreatyPriceRelief(string factionId) =>
public CaravanTradeNetworkSave CaptureState() {
public void RestoreState(CaravanTradeNetworkSave? save) {
```


# Appendix Q.556 — Additional Current Architecture Evidence: `src/Host/EconomySaveStore.cs`

### `src/Host/EconomySaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 117 lines / 5315 bytes.
- SHA-256: `ed20a1698357aae2d945d06cc2363e72afe424e42cf829ad4567eb4d4c1b9412`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class EconomySaveEnvelope
public string Checksum = string.Empty;
public MarketState State;
public static class EconomySaveStore
public const string FileName = "economy_save.json";
public const string SectionName = "economy";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(MarketState state) => s_store.CaptureBare(state);
public static MarketState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(MarketState state) => s_store.CaptureBare(state);
public static MarketState? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(MarketState state) => s_store.TrySave(state);
public static bool TrySave(MarketState state, string path) => s_store.TrySave(state, path);
public static MarketState? TryLoad() => s_store.TryLoad();
public static MarketState? TryLoad(string path) => s_store.TryLoad(path);
public static string TryCapturePersisted(MarketState state) => s_store.CapturePersisted(state);
```


# Appendix Q.557 — Additional Current Architecture Evidence: `src/Host/ContentUtilizationRuntimeCollector.cs`

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


# Appendix Q.558 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`

### `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 3459 lines / 190529 bytes.
- SHA-256: `d79f9fa53bb0e6eed315b2dbb58e4c2a2f32991272ddd44200da26afa1458719`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CatalogIntegrityReport
public readonly List<string> Errors = new List<string>();
public readonly List<string> Warnings = new List<string>();
public int ErrorCount => Errors.Count;
public bool Clean => Errors.Count == 0;
public int AuthoredIds;
public int ReuseCount;
public void Error(string message) {
public void Warn(string message) {
public static class CatalogIntegrityValidator
public static readonly string[] IdPrefixes = {
public static readonly string[] DefinitionKeys = {
public static readonly string[] ReferenceKeys = {
public static readonly string[] RangeKeys = { "minDay", "maxDay", "MinDay", "min_day" };
public static readonly string[] VocabularyKeys = {
public static readonly string[] KnownRuntimeIds = {
public static readonly string[] PrefixPatternKeys = {
public readonly Dictionary<string, List<string>> Registry = new Dictionary<string, List<string>>(StringComparer.Ordinal);
public readonly List<Ref> PendingRefs = new List<Ref>();
public readonly Dictionary<string, RangeMemoEntry> RangeMemo = new Dictionary<string, RangeMemoEntry>(StringComparer.Ordinal);
public CatalogIntegrityReport Report;
public string File;
public int Authored;
public int Reuse;
public string Value;
public string Path;
public bool Strict;
public string? EntityContext;
public static CatalogIntegrityReport Validate(string dataDirectory, IFileIO files) => Validate(dataDirectory, files, SearchOption.TopDirectoryOnly);
public static CatalogIntegrityReport Validate(string dataDirectory, IFileIO files, SearchOption searchOption) {
public static void ValidateNightWatchOperationsCatalog( string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateShelterOperationsCatalogs( string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateDifficultyPresetCatalog( string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateVehicleArmorGradeCatalog( string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public int? Min;
public int? Max;
public static void ValidateDistressSignalStages(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateTradeEmbargoRules(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateRegionalPriceAtlas(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateCommitments(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateWildlifeTrappingCatalog(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
```


# Appendix Q.559 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Crafting/ChemicalSynthesisCatalog.cs`

### `Assets/Ashfall.Core/Crafting/ChemicalSynthesisCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 198 lines / 8091 bytes.
- SHA-256: `40e84ef8c81f8a2ab00b663c1b7e3486bb2b63206c9299952902a0b630bfb21e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ChemicalProcessDefinition
public string id = string.Empty;
public string displayName = string.Empty;
public string description = string.Empty;
public int requiredApparatusTier = 1;
public Dictionary<string, int> inputItems = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
public Dictionary<string, int> outputItems = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
public int processingTicks = 2;
public string heatBand = "Nominal";
public float volatilityRating = 0.2f;
public float scrubberDemand = 1.0f;
public float equipmentWear = 2.0f;
public float corrosionRating; // Plans 90-93 mineral line: normalized apparatus/storage corrosion per tick
public float skillRequirement = 10.0f;
public List<string> tags = new List<string>();
public bool Validate(out string error) {
public sealed class ChemicalSynthesisCatalogDto
public int schema_version { get; set; } = 1;
public List<ChemicalProcessDefinition> processes { get; set; } = new List<ChemicalProcessDefinition>();
public sealed class ChemicalSynthesisCatalog
public IReadOnlyDictionary<string, ChemicalProcessDefinition> Processes => _processes;
public ChemicalProcessDefinition? GetProcess(string processId) {
public static class ChemicalSynthesisCatalogLoader
public const string DefaultFileName = "chemical_syntheses.json";
public const string MineralFileName = "mineral_acid_synthesis_catalog.json";
public static readonly IReadOnlyDictionary<string, string> KeyAliases = new Dictionary<string, string> {
public static ChemicalSynthesisCatalog? Load(string dataDir, IFileIO fileIO, IJsonSerializer jsonSerializer) {
```


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Economy/EconomyHeadlessDemo.cs`

### `Assets/Ashfall.Core/Economy/EconomyHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 102 lines / 4380 bytes.
- SHA-256: `a91843e03d98c17860612f7364308a836f9f85ddc3b3074d68494993afa56369`.
- Architecture signals: seeded references=3; save/restore symbols=3; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class EconomyHeadlessDemo
public static HeadlessReport Run(string dataDirectory, ILog? log = null) {
```


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Economy/RegionalSupplyRouter.cs`

### `Assets/Ashfall.Core/Economy/RegionalSupplyRouter.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 274 lines / 12273 bytes.
- SHA-256: `d46e3a51f89f6677490c4ec72a4856c3b59a543ad417ea9ca6822931ce9676cd`.
- Architecture signals: seeded references=2; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class RegionalSupplyRouter
public static bool IsAcceptedSupplyTag(string regionalSupply) {
public static IReadOnlyList<string> AcceptedSupplyTagValues => AcceptedSupplyTags;
public static IReadOnlyList<string> TagsForOrigin(string originRegion) {
public static bool ProducesGood(GoodsCatalog catalog, string originRegion, string goodId) {
public static List<CaravanCargoEntry> SpecialtyCargoForOrigin( GoodsCatalog catalog, string originRegion, int maxLots = 4, ISeededRng? rng = null) {
public static float WorldShortageDemandScale( GoodsCatalog catalog, string goodId, IEnumerable<string> activeOriginRegions) {
public static float ShortageDemandScale(string regionalSupply, string originRegion) {
public static string ProvenanceLabel(GoodsCatalog catalog, string originRegion, string goodId) {
public static List<string> FilterStockForShortage( IReadOnlyList<string> stockItemIds, GoodsCatalog catalog, string originRegion) {
public static float RationAnchor(GoodsCatalog catalog) {
public sealed class CaravanCargoEntry
public string GoodId = string.Empty;
public int Quantity;
public int PriceRations;
```


# Appendix Q.562 — Additional Current Architecture Evidence: `src/Host/HostCli.WorldPlaytest.cs`

### `src/Host/HostCli.WorldPlaytest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1008 lines / 56167 bytes.
- SHA-256: `96d58936693cbbb9fc62a949e176a97480f7ce3654f83151cec2af518576e3c9`.
- Architecture signals: seeded references=8; save/restore symbols=26; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunWorldPlaytestSelfTest(string dataDirectory, Main? productionMain = null) {
public string id = string.Empty;
public bool passed;
public string evidence = string.Empty;
public int schema_version = 1;
public string source_commit = string.Empty;
public string day_owner = "world_evolution";
public int master_seed;
public int snapshot_count;
public List<WorldPlaytestBand> target_bands = new List<WorldPlaytestBand>();
public int first_migration_day = -1;
public int first_degradation_transition_day = -1;
public float wildlife_density_min;
public float wildlife_density_max;
public float wildlife_density_delta;
public float encounter_multiplier_min;
public float encounter_multiplier_max;
public float canned_food_scarcity_min;
public float canned_food_scarcity_max;
public int briefing_event_count;
public int journal_event_count;
public int radio_event_count;
public int trapping_density_reads;
public int encounter_multiplier_reads;
public int dead_seed_count;
public bool same_seed_byte_equal;
public bool midpoint_save_load_byte_equal;
public bool different_seed_diverged;
public bool duplicate_narrative_events_after_restore;
public List<WorldPlaytestInfluenceRow> influence_map = new List<WorldPlaytestInfluenceRow>();
public List<WorldPlaytestCheck> checks = new List<WorldPlaytestCheck>();
public List<WorldPlaytestSnapshot> snapshots = new List<WorldPlaytestSnapshot>();
public string tuning_decision = string.Empty;
public string measure = string.Empty;
public string band = string.Empty;
public string producer = string.Empty;
public string value = string.Empty;
public string consumer = string.Empty;
public string read_cadence = string.Empty;
public string player_visible_consequence = string.Empty;
public int day;
public float global_wildlife_ratio;
public float total_wildlife_density;
public List<WorldPlaytestLocation> locations = new List<WorldPlaytestLocation>();
public List<WorldPlaytestWildlife> wildlife = new List<WorldPlaytestWildlife>();
public WorldPlaytestEconomy economy = new WorldPlaytestEconomy();
public List<WorldPlaytestSurfaceEvent> surfaced_events = new List<WorldPlaytestSurfaceEvent>();
public string location_id = string.Empty;
public string owner = string.Empty;
public string degradation_tier = string.Empty;
public float contamination;
public float loot_depletion;
public float encounter_multiplier;
public string pack_id = string.Empty;
public string species_id = string.Empty;
public string region_id = string.Empty;
public int density;
public float starvation;
public bool rabid;
public float canned_food_scarcity_proxy;
public List<WorldPlaytestGood> scarcity_goods = new List<WorldPlaytestGood>();
public string item_id = string.Empty;
public float demand_multiplier;
public string channel = string.Empty;
public string kind = string.Empty;
public string primary_id = string.Empty;
public string secondary_id = string.Empty;
public float numeric;
public string expedition_id = string.Empty;
public string location_id = string.Empty;
public int phase;
public int schema_version = 1;
public WorldWeatherState weather = new WorldWeatherState();
public LocationEvolutionSaveState locations = new LocationEvolutionSaveState();
public WildlifeSaveState wildlife = new WildlifeSaveState();
public LandmarkSaveState landmarks = new LandmarkSaveState();
public MarketState market = new MarketState();
public WildlifeTrappingState trapping = new WildlifeTrappingState();
public List<ExpeditionState> expeditions = new List<ExpeditionState>();
public int expedition_completed_count;
public CampaignDaySave coordinator = new CampaignDaySave();
public List<string> previous_sectors = new List<string>();
public List<string> surfaced_event_keys = new List<string>();
public int last_trapping_catch;
public WeatherSystem Weather { get; }
public LocationEvolutionSystem Locations { get; }
public WildlifeMigrationSystem Wildlife { get; }
public LandmarkDegradationSystem Landmarks { get; }
public MarketSystem Market { get; }
public WildlifeTrappingSystem Trapping { get; }
public ExpeditionSystem Expeditions { get; }
public CampaignDayCoordinator Coordinator { get; }
public List<WorldPlaytestSnapshot> Snapshots { get; } = new List<WorldPlaytestSnapshot>();
public int Migrations { get; private set; }
public int DegradationTransitions { get; private set; }
public int TrappingDayTicks { get; private set; }
public int TrappingDensityReads { get; private set; }
public int EncounterMultiplierReads { get; private set; }
public int EncounterEvaluationTicks { get; private set; }
public int SurfacedBriefingEvents { get; private set; }
public int SurfacedJournalEvents { get; private set; }
public int SurfacedRadioEvents { get; private set; }
public bool NoDuplicateNarrativeEvents { get; private set; } = true;
public int DeadSeedCount { get; private set; }
public static WorldPlaytestRun Create(string dataDirectory, int seed) {
public void AdvanceThrough(int firstDay, int lastDay) {
public WorldPlaytestSave CaptureSave() {
public void RestoreSave(WorldPlaytestSave save) {
public float EncounterMultiplierMovement => Snapshots.Count == 0
public float ScarcityMovement => Snapshots.Count < 2
public bool AllSnapshotsSane() {
public bool NoDuplicateSnapshotRecords() {
public bool RuinedStatesAreSticky() {
public WorldPlaytestArtifact BuildArtifact(string dataDirectory, List<WorldPlaytestCheck> checks, bool sameSeedEqual, bool midpointEqual, bool differentSeedDiverged) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundryHeadlessDemo.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundryHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 187 lines / 11583 bytes.
- SHA-256: `d09f8c8d3f60830df15c9028c4cae67540b0c213d1c543dc0bfb4a419fe8022f`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class SilentFoundryHeadlessDemo
public const int DemoSeed = 1009;
public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null) {
```


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs`

### `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 320 lines / 11951 bytes.
- SHA-256: `13a5e861745ddbf7ef9428bb701dc55dcef605d5339a79503f616536eb95bbdd`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=16; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ChemicalRetortState
public string vesselId = string.Empty;
public string activeProcessId = string.Empty;
public int processProgress;
public int processingTicksRequired;
public string heatBand = "Nominal"; // Low, Nominal, High, Runaway
public string pressureBand = "Nominal"; // Vacuum, Nominal, Elevated, Critical
public float catalystCondition = 100.0f; // 0..100
public float scrubberCondition = 100.0f; // 0..100
public bool isSealed = true;
public string assignedOperatorId = string.Empty;
public string failureState = "None"; // None, BatchLoss, VesselDamage, ScrubberFailure, ExposureEvent
public int lastTickDay;
public ChemicalRetortState Clone() {
public sealed class ChemicalSynthesisSave
public List<ChemicalRetortState> vessels = new List<ChemicalRetortState>();
public float scrubberReserve = 100.0f;
public int apparatusTier = 1;
public int lastTickDay;
public ChemicalSynthesisSave Clone() {
public sealed class ChemicalSynthesisSystem
public IReadOnlyList<ChemicalRetortState> Vessels => _vessels;
public float ScrubberReserve => _scrubberReserve;
public int ApparatusTier => _apparatusTier;
public int LastTickDay => _lastTickDay;
public event Action<string, string>? OnProcessStarted;
public event Action<string, string>? OnProcessCompleted;
public event Action<string, string, string>? OnProcessFailed;
public event Action<string, string, float>? OnExposureIncident; // vesselId, operatorId, severity
public event Action? OnStateChanged;
public ChemicalRetortState? GetVessel(string vesselId) {
public bool TryUpgradeApparatus(int targetTier) {
public bool TryStartProcess(string processId, string vesselId, string operatorId = "") {
public bool TryHarvestOutput(string vesselId) {
public bool TryServiceScrubber(string vesselId) {
public bool TryPurgeVessel(string vesselId) {
public void TickDay(int currentDay) {
public ChemicalSynthesisSave CaptureState() {
public void RestoreState(ChemicalSynthesisSave? save) {
```


# Appendix Q.565 — Additional Current Architecture Evidence: `src/Host/BlackMarketSelfTest.cs`

### `src/Host/BlackMarketSelfTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 273 lines / 13565 bytes.
- SHA-256: `ebfd0d33121a65f7976adb09419f1b8a3d8a8a9bc902f5139fe62bd6de9be251`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class BlackMarketSelfTest
public static int RunSelfTest(string? dataDir = null) {
```


# Appendix Q.566 — Additional Current Architecture Evidence: `src/Host/EconomyHostSession.cs`

### `src/Host/EconomyHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 261 lines / 11988 bytes.
- SHA-256: `7e7cc9f92f1137aea3e30ba6f0fee55fc3c35b134de6367d2305881c7dc470e8`.
- Architecture signals: seeded references=1; save/restore symbols=8; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class EconomyHostSession
public const int DemoSeed = 2026;
public MarketSystem Market { get; }
public ResourceRationingSystem Rationing { get; }
public GoodsCatalog Catalog { get; private set; }
public CommodityBaselineCatalog? CommodityCatalog { get; private set; }
public TradeEmbargoSystem? EmbargoSystem { get; private set; }
public RegionalPriceAtlas? RegionalAtlas { get; private set; }
public string LastEvent { get; private set; } = string.Empty;
public Action<RationTarget>? RationTierChangedSeam { get; set; }
public static EconomyHostSession Create(string dataDir) {
public void LoadData(string dataDir) {
public void BindRationingResourceValidator(Func<string, bool>? validator) => Rationing.BindResourceValidator(validator);
public RationTarget SetRationTier(string resourceId, RationingTier tier, int currentDay) => Rationing.SetRationTier(resourceId, tier, currentDay);
public ResourceAllocationDecision AuthorizeAllocation( string resourceId, string consumerId, int demandUnits, int availableUnits, int currentDay)
public string TickDemo(int days) {
public string BarterDemo(string giveItemId, int giveQuantity, string takeItemId) {
public void TickDay(int day, ISeededRng rng) {
public PriceExplanation ExplainPrice( string itemId, MarketTransactionSide side = MarketTransactionSide.Buy) => Market.ExplainPrice(itemId, side);
public PriceExplanation ExplainPrice( string itemId, MarketTransactionSide side, string? region) => Market.ExplainPrice(itemId, side, region);
public string StatusLine() {
public MarketState CaptureSave() {
public void RestoreSave(MarketState state) {
```


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Economy/BlackMarketSettlementService.cs`

### `Assets/Ashfall.Core/Economy/BlackMarketSettlementService.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 372 lines / 19874 bytes.
- SHA-256: `e8914870d671c1b6a02b3ff9abdc2140ef8861e044d437ed5a187262749cf2fe`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public readonly struct BlackMarketActionPreview
public readonly bool IsAvailable;
public readonly string ActionId;
public readonly string SyndicateId;
public readonly string EntryId;
public readonly string DebtId;
public readonly string ItemId;
public readonly int Quantity;
public readonly long SettlementUnits;
public readonly int DueDay;
public readonly string ReasonId;
public readonly string Message;
public readonly struct BlackMarketActionResult
public readonly bool Success;
public readonly string ActionId;
public readonly string SyndicateId;
public readonly string EntryId;
public readonly string DebtId;
public readonly string ItemId;
public readonly int Quantity;
public readonly long WalletDelta;
public readonly int InventoryDelta;
public readonly long SettlementUnits;
public readonly int DueDay;
public readonly string ReasonId;
public readonly string Message;
public sealed class BlackMarketSettlementService
public const string BuyAction = "buy";
public const string SellAction = "sell";
public const string LoanAction = "take_loan";
public const string RepayAction = "repay";
public long WalletValue => _wallet.Value;
public int InventoryCount(string itemId) =>
public BlackMarketActionPreview PreviewBuy(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketActionResult Buy(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketActionPreview PreviewSell(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketActionResult Sell(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketActionPreview PreviewLoan(string syndicateId, long units, int day, int durationDays) {
public BlackMarketActionResult TakeLoan(string syndicateId, long units, int day, int durationDays) {
public BlackMarketActionPreview PreviewRepay(string debtId, long units, int day) {
public BlackMarketActionResult Repay(string debtId, long units, int day) {
public static string ReasonText(string reasonId) {
```


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Economy/HardcoreEconomyTuningLoader.cs`

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


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Economy/TradeEmbargoSystem.cs`

### `Assets/Ashfall.Core/Economy/TradeEmbargoSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 708 lines / 33711 bytes.
- SHA-256: `b244e89a2e33a180124ed8064fddf0e97c3b0bd3b7a46ba07dfb36e574eb4864`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class EmbargoRule
public string RuleId = string.Empty;
public WeatherKind Weather;
public IReadOnlyList<string> AffectedRegions { get; }
public IReadOnlyList<string> AffectedCategories { get; }
public IReadOnlyList<string> AffectedItemIds { get; }
public bool AffectsAllGoods { get; }
public int PriceMultiplierPermille { get; }
public bool CaravanBlocked { get; }
public int RouteSlowPermille { get; }
public int DecayDays { get; }
public sealed class TradeEmbargoLoadResult
public List<EmbargoRule> Rules { get; } = new List<EmbargoRule>();
public List<string> Errors { get; } = new List<string>();
public bool HasErrors => Errors.Count > 0;
public sealed class TradeEmbargoCatalog
public IReadOnlyList<EmbargoRule> Rules => _rules;
public int Count => _rules.Count;
internal void Add(EmbargoRule rule) => _rules.Add(rule);
public sealed class EmbargoShockRuntime
public string ruleId = string.Empty;
public int weatherKind = 0;
public int peakPermille = 1000;
public int currentPermille = 1000;
public int remainingDecayDays = 0;
public int lastAdvancedDay = -1;
public sealed class TradeEmbargoState
public const int Version = 1;
public int version = Version;
public int lastNotifiedDay = -1;
public List<EmbargoShockRuntime> shocks = new List<EmbargoShockRuntime>();
public class TradeEmbargoSystem
public const float CombinedProductFloor = 0.4f;
public const float CombinedProductCeiling = 2.5f;
public const int MinMultiplierPermille = 500;
public const int MaxMultiplierPermille = 5000;
public const int MinRouteSlowPermille = 1;     // below this, author caravan_blocked instead
public const int MaxRouteSlowPermille = 1000;  // slowdowns never speed travel up
public const int MaxDecayDays = 30;
public TradeEmbargoState State => _state;
public int RuleCount => _rules.Count;
public bool RegisterRule(EmbargoRule rule) {
public EmbargoRule? FindRule(string ruleId) =>
public bool IsEmbargoActive(WeatherKind current) {
public List<EmbargoRule> GetActiveRules(WeatherKind current) {
public List<string> GetAffectedRegions(WeatherKind current) {
public bool IsRouteBlocked(string? region, WeatherKind current) {
public float GetRouteProgressMultiplier(string? region, WeatherKind current) {
public int GetWeatherPriceMultiplierPermille(string? region, WeatherKind current, string itemId, string itemCategory) {
public void NotifyWeather(int day, WeatherKind current) {
public int GetCurrentPriceMultiplierPermille(string? region, string itemId, string itemCategory) {
public IReadOnlyList<EmbargoShockRuntime> ActiveShocks => _state.shocks;
public EmbargoSummary GetEmbargoSummary(WeatherKind current) {
public static bool IsRuleValid(EmbargoRule? rule, out List<string> errors) {
public TradeEmbargoState CaptureState() {
public void RestoreState(TradeEmbargoState? saved) {
public sealed class EmbargoSummary
public string weatherKind = string.Empty;
public bool embargoActive;
public bool allGoodsAffected;
public bool anyRouteBlocked;
public int activeRuleCount;
public List<string> affectedRegions = new List<string>();
public List<string> affectedCategories = new List<string>();
public List<string> affectedItemIds = new List<string>();
public static class TradeEmbargoCatalogLoader
public const string FileName = "trade_embargoes.json";
public const int CurrentSchemaVersion = 1;
public static TradeEmbargoLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static TradeEmbargoCatalog ToCatalog(TradeEmbargoLoadResult load) {
public int schema_version = 1;
public string collection_id = string.Empty;
public string description = string.Empty;
public List<RawEmbargoRule> rules = new List<RawEmbargoRule>();
public string rule_id;
public string weather_kind;
public List<string> affected_regions;
public List<string> affected_categories;
public List<string> affected_item_ids;
public bool affects_all_goods;
public int? price_multiplier_permille;
public bool caravan_blocked;
public int? route_slow_permille;
public int? decay_days;
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/InfrastructureHeadlessDemo.cs`

### `Assets/Ashfall.Core/InfrastructureHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 167 lines / 8793 bytes.
- SHA-256: `8e420b1538619fdc512e550f48f2da44469b8d3a5c2f7e124f70fb36d0667b98`.
- Architecture signals: seeded references=5; save/restore symbols=8; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class InfrastructureHeadlessDemo
public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null) {
```


# Appendix Q.571 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/TravelingCaravanHeadlessDemo.cs`

### `Assets/Ashfall.Core/TravelingCaravanHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 71 lines / 3065 bytes.
- SHA-256: `52a9daa0337cc0ebdbe7ab2d66761a7f65c6aab933bf2f9f108c8d1cb9f958dd`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class TravelingCaravanHeadlessDemo
public static int Run() {
```


# Appendix Q.572 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Waystation/WaystationNetworkSystem.cs`

### `Assets/Ashfall.Core/Waystation/WaystationNetworkSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 200 lines / 8161 bytes.
- SHA-256: `51cf876a0bed96e4cc3c89c657bc4ab383ebdc339abc92b4d8a1fed8bc8b1e6e`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WaystationInstanceState
public string stationId { get; set; } = string.Empty;
public bool isUnlocked { get; set; } = true;
public float condition { get; set; } = 100f;
public float filterHealth { get; set; } = 100f;
public bool stoveLit { get; set; } = true;
public int daysSinceResupply { get; set; } = 0;
public List<string> availableStockItemIds { get; set; } = new List<string>();
public List<string> assignedWatchSurvivorIds { get; set; } = new List<string>();
public sealed class WaystationNetworkState
public string systemId = WaystationNetworkSystem.SystemId;
public List<WaystationInstanceState> stations = new List<WaystationInstanceState>();
public int totalMaintenanceActions = 0;
public sealed class WaystationNetworkSystem
public const string SystemId = "waystation_network_system";
public event Action<WaystationInstanceState>? OnWaystationStateChanged;
public void BindShortagePolicy(GoodsCatalog catalog, Func<bool> isSuppliesShort) {
public WaystationNetworkState State => _state;
public IReadOnlyList<WaystationDef> Catalog => _catalog;
public WaystationInstanceState? GetStation(string stationId) {
public static List<string> LapsedImports(WaystationDef def, WaystationInstanceState station) {
public WaystationDef? GetDefinition(string stationId) {
public void TickDay() {
public bool RepairFilter(string stationId) {
public bool AssignWatch(string stationId, IEnumerable<string> survivorIds) {
public WaystationNetworkState CaptureState() {
public void RestoreState(WaystationNetworkState state) {
```


# Appendix Q.573 — Additional Current Architecture Evidence: `src/Host/TravelingCaravanHostSession.cs`

### `src/Host/TravelingCaravanHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 105 lines / 4789 bytes.
- SHA-256: `c7d2f84a1650e46942b0fe301a65cd18fe0c3ded6892b3b0e541cb7e036a79e7`.
- Architecture signals: seeded references=1; save/restore symbols=5; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TravelingCaravanHostSession
public TravelingCaravanSystem Engine { get; }
public string LastEvent { get; private set; } = string.Empty;
public static TravelingCaravanHostSession Create(string dataDir) {
public string SpawnCaravan(string nodeId) {
public string TickRoute(WeatherKind weather = WeatherKind.Clear, int day = 0, ISeededRng? rng = null) {
public string Buy(string caravanId, string itemId, int amount, ref int playerRations) {
public string StatusLine() {
public TravelingCaravanState CaptureSave() => Engine.CaptureState();
public void RestoreSave(TravelingCaravanState state) => Engine.RestoreState(state);
```


# Appendix Q.574 — Additional Current Architecture Evidence: `src/Host/WeatherCascadeHostSession.cs`

### `src/Host/WeatherCascadeHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 518 lines / 24050 bytes.
- SHA-256: `636de4c6947442faa54627ada802045054777d90216b5bc371cc29c388574de7`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WeatherCascadeContribution
public readonly string TargetSystem;
public readonly string OwnerName;
public readonly string Detail;
public sealed class WeatherCascadeOutcome
public readonly string EventId;
public readonly WeatherKind Kind;
public readonly float Severity;
public readonly int EffectCount;
public readonly int Day;
public readonly List<WeatherCascadeContribution> Contributions = new List<WeatherCascadeContribution>();
public string Describe() =>
public sealed class WeatherCascadeHostSession : HostSessionBase
public const string CascadeSystemId = WeatherCascadeSystem.SystemId;
public string EventId = string.Empty;
public string Region = string.Empty;
public int StartDay;
public int DurationDays;
public float Severity;
public float EncounterMultiplier = 1f;
public bool HasEncounter;
public bool HasMorale;
public string MoraleSourceId = string.Empty;
public WeatherCascadeSystem System { get; }
public WeatherEffectsCatalog? EffectsCatalog { get; private set; }
public DisasterResponseSystem? ShelterResilience { get; set; }
public ExpeditionSystem? Expeditions { get; set; }
public MarketSystem? Market { get; set; }
public NeedsSystem? Needs { get; set; }
public bool UsingAuthoredTemplates { get; private set; }
public IReadOnlyList<string> LoadErrors => _loadErrors;
public IReadOnlyList<WeatherCascadeOutcome> History => _history;
public IReadOnlyList<string> AccessibilityReport => _accessibility;
public bool LoadAuthoredTemplates(string dataDirectory, IFileIO files) {
public void BindEffectsCatalog(WeatherEffectsCatalog? catalog) => EffectsCatalog = catalog;
public WeatherCascadeOutcome TriggerFront(WeatherKind kind, int day, IReadOnlyList<string>? regions = null) {
public List<WeatherCascadeOutcome> TickDay(int day) {
public string StatusLine() {
public static class WeatherCascadeSaveStore
public const string FileName = "weather_cascade_save.json";
public const string SectionName = "weather_cascade";
public static bool TrySave(WeatherCascadeState state) => s_store.TrySave(state);
public static WeatherCascadeState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(WeatherCascadeState state) => s_store.CapturePersisted(state);
public static WeatherCascadeState? TryRestore(string json) => s_store.RestoreEnvelope(json);
public static WeatherCascadeState? TryRestoreBare(string json) => s_store.RestoreBare(json);
```


# Appendix Q.575 — Additional Current Architecture Evidence: `src/UI/BlackMarketSnapshotFixture.cs`

### `src/UI/BlackMarketSnapshotFixture.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 81 lines / 3381 bytes.
- SHA-256: `b4bd03a2aa971f6ee937d7573c067f48656c48aa33907a0ef4c63b97c5eeaee9`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
internal static class BlackMarketSnapshotFixture
public static IDisposable? Bind(Node node) {
public void Dispose() {
```


# Appendix Q.576 — Additional Current Architecture Evidence: `src/Host/HostCli.Difficulty.cs`

### `src/Host/HostCli.Difficulty.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 240 lines / 12222 bytes.
- SHA-256: `d6321a4ac1d13f723faab7cf03a5472ce3174e300e05fc8edaf4001e85b4b754`.
- Architecture signals: seeded references=3; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunDifficultySelfTest(string dataDirectory) {
```


# Appendix Q.577 — Additional Current Architecture Evidence: `src/Host/BlackMarketHostSession.cs`

### `src/Host/BlackMarketHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 203 lines / 9381 bytes.
- SHA-256: `0c966540706c1a34c641997d0c65bdf94c5cf2f0a99c1a15d0fba7a68ed68a62`.
- Architecture signals: seeded references=1; save/restore symbols=5; typed event declarations=1; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class BlackMarketHostSession
public BlackMarketSystem System { get; }
public FactionBountySystem Bounties { get; }
public string LastEvent { get; private set; } = string.Empty;
public event Action<BlackMarketActionResult>? ActionCompleted;
public bool ActionsAvailable => _settlement != null && _dayProvider != null;
public long WalletValue => _settlement?.WalletValue ?? 0L;
public static BlackMarketHostSession Create(string dataDir, MarketSystem market) {
public void BindSettlementOwners(HoldfastTradeSession wallet, Ashfall.Core.Inventory.Inventory inventory, ItemCatalog items, Func<int> dayProvider) {
public int InventoryCountForEntry(string entryId) {
public BlackMarketActionPreview PreviewBuy(string syndicateId, string entryId, int quantity) =>
public BlackMarketActionPreview PreviewSell(string syndicateId, string entryId, int quantity) =>
public BlackMarketActionPreview PreviewLoan(string syndicateId, long units, int durationDays) =>
public BlackMarketActionPreview PreviewRepay(string debtId, long units) =>
public BlackMarketActionResult Buy(string syndicateId, string entryId, int quantity) =>
public BlackMarketActionResult Sell(string syndicateId, string entryId, int quantity) =>
public BlackMarketActionResult TakeLoan(string syndicateId, long units, int durationDays) =>
public BlackMarketActionResult Repay(string debtId, long units) =>
public void TickDay(int day, ISeededRng stockRng) {
public string StatusLine() {
public BlackMarketState CaptureSave() => System.CaptureState();
public void RestoreSave(BlackMarketState state) => System.RestoreState(state);
```


# Appendix Q.578 — Additional Current Architecture Evidence: `src/Foundry/SilentFoundryHostSession.cs`

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


# Appendix Q.579 — Additional Current Architecture Evidence: `src/Main.AdvancedShelterSystems.cs`

### `src/Main.AdvancedShelterSystems.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 613 lines / 27033 bytes.
- SHA-256: `88b198ecb0532edcf5924ebe49168fc581cd5e3467c69fae3fa6a603f89c5df4`.
- Architecture signals: seeded references=7; save/restore symbols=14; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public CaravanTradeNetworkSystem EnsureCaravanTrade() {
public AdvancedSurgicalWardSystem EnsureSurgicalWard() {
public PowerDistributionSubgridSystem EnsurePowerSubgrids() {
public void TickPowerSubgrids(int day) {
public PerimeterDefenseSystem EnsurePerimeterDefense() {
public HydroponicBiomeSystem EnsureHydroponicBiomes() {
public NuclearCoreLifecycleSystem EnsureNuclearCore() {
public ArmoredCrawlerExpeditionSystem EnsureArmoredCrawlers() {
public void TickAdvancedShelterSystems(int day) {
```


# Appendix Q.580 — Additional Current Architecture Evidence: `src/Host/HostCli.Cartography.cs`

### `src/Host/HostCli.Cartography.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 128 lines / 5959 bytes.
- SHA-256: `93a67f4d27fa1ba0a6ff1f62f230ff573e15f1e2ad9dec9aa31d33bb6f2f7674`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunCartographySelfTest(string dataDirectory) {
```


# Appendix Q.581 — Additional Current Architecture Evidence: `src/Host/HostCli.StartingSupplies.cs`

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


# Appendix Q.582 — Additional Current Architecture Evidence: `src/Host/HostCli.WeatherCascade.cs`

### `src/Host/HostCli.WeatherCascade.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 326 lines / 18534 bytes.
- SHA-256: `829b77e2656089f4450fa847778140c2fbf29eedc49dbfbb62f4dc7539637036`.
- Architecture signals: seeded references=0; save/restore symbols=3; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunWeatherCascadeSelfTest(string dataDirectory) {
```


# Appendix Q.583 — Additional Current Architecture Evidence: `src/Host/InventoryHostSession.cs`

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


# Appendix Q.584 — Additional Current Architecture Evidence: `src/Main.Inventory.cs`

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


# Appendix Q.585 — Additional Current Architecture Evidence: `src/UI/ChemicalLabPanel.cs`

### `src/UI/ChemicalLabPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 476 lines / 22365 bytes.
- SHA-256: `e19cbe4f71e7e9900e23347e6a70041540911ca541532d0e15fe11cb811f639d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class ChemicalLabPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _host != null;
public override void _Ready() {
public void Bind(ChemicalSynthesisHostSession? host) {
public void Bind(object? session) {
public void Unbind() {
public void Open() {
public void Close() {
public override void _ExitTree() {
public void RefreshView() {
```


# Appendix R.586 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs`

### `Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs`

- Current test declarations: Fact=3, Theory=7, InlineData=0.
- File lines: 793; SHA-256: `5abd429663ea6c6a577aca007601d49cc33be4d4af52a39ecec98e280ac14634`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
FailureRecording_IncludesSeedAndReproductionInstructions
WeatherSystem_SeedSweep_IsDeterministicAcrossReplaysAndSaves
MarketSystem_SeedSweep_IsDeterministicAcrossReplaysAndSaves
PowerGridSystem_SeedSweep_IsDeterministicAcrossReplaysAndSaves
NeedsSystem_SeedSweep_IsDeterministicAcrossReplaysAndSaves
DiseaseSystem_SeedSweep_IsDeterministicAcrossReplaysAndSaves
CombatBallistics_SeedSweep_IsDeterministicAcrossReplays
ProceduralScavenge_SeedSweep_IsDeterministicAcrossReplays
SplitRun_SaveLoad_ProducesIdenticalFinalState
ReplayParity_SameSeed_SameFinalState
```


# Appendix R.587 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Economy/EconomySaveStoreTests.cs`

### `Ashfall.Core.Tests/Economy/EconomySaveStoreTests.cs`

- Current test declarations: Fact=17, Theory=0, InlineData=0.
- File lines: 432; SHA-256: `592055011da541fec365435e86d85aa6cabce0d769a8ddb001385ef55204e7b7`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
TryLoad_NonExistentFile_ReturnsNullWithoutLoggingError
TryLoad_EmptyOrWhitespaceFile_ReturnsNullWithoutLoggingError
TryLoad_MalformedJsonSyntax_CatchesException_LogsErrorAndReturnsNull
TryLoad_TruncatedJson_CatchesException_LogsErrorAndReturnsNull
TryLoad_InvalidJsonTokens_CatchesException_LogsErrorAndReturnsNull
TryLoad_TamperedPayload_ChecksumMismatch_ReturnsNull
TryLoad_TamperedChecksum_ReturnsNull
TryLoad_MissingChecksumField_ReturnsNull
TryLoad_NullStateInEnvelope_ReturnsNull
TryLoad_LegacyBareState_WithValidSystemId_LoadsSuccessfully
TryLoad_LegacyBareState_MissingOrEmptySystemId_ReturnsNull
TryLoad_FileIoThrowsIOException_CatchesException_LogsErrorAndReturnsNull
TryLoad_CleanRoundTrip_PreservesMarketStateAndChecksum
TrySave_NullState_ReturnsFalse
TrySave_FileIoThrowsIOException_CatchesException_LogsErrorAndReturnsFalse
RestoreBare_MalformedJson_LogsErrorAndReturnsNull
CaptureBare_NullState_ReturnsEmptyString
```


# Appendix R.588 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/FlagshipEconomyScenarioTests.cs`

### `Ashfall.Core.Tests/FlagshipEconomyScenarioTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 339; SHA-256: `e07a641af5b2b040a3fa2980723837d97965f02814d53864660f0a7861406e05`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ScenarioA_SanitationCollapse_RaisesMedicalPrices_ThenRecovers
ScenarioB_DebtSpiral_OverdueFiresOnce_BountyPlaced_SaveLoadNoDuplication
ScenarioC_WinterShock_FoodPriceRisesGradually_AndBounded
ScenarioD_MetallurgySupplyResponse_ProductionSoftensPrices_PremiumPersists
ScenarioE_ToxicSpill_DeterministicTrigger_SaveRestoreNotRecreated
ScenarioF_CombinedCampaign_ContinuousEqualsMidReloadReplay
```


# Appendix R.589 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Plan56Phase5Tests.cs`

### `Ashfall.Core.Tests/Plan56Phase5Tests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 233; SHA-256: `4653fd3d8c063e5d16de1b1a0b4f647aa6d33aaa672c786e11ba048a227298a3`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
WorldScale_BestReliefAcrossActiveRegions
WorldScale_NarrowActiveSet_EscalatesUnservicedPools
ScarcityTraces_ProvenanceScalesTheDelta
ScarcityTraces_AreDeterministicAcrossReplays
ExtremeScarcity_ClampsAtCeiling_NeverNaN
LongHorizonAudit_NoRunaway_NoCollapse_AllBounded
LongHorizonAudit_IsDeterministic
```


# Appendix R.590 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Plan56FollowUpTests.cs`

### `Ashfall.Core.Tests/Plan56FollowUpTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 156; SHA-256: `d63957ef3d9bf25a82fa56d739822cb05e234d414f11039391a7f82bb4633742`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RegionalSupply_ParsesAsFirstClassField
RegionalSupply_IsOptional_AndTrimmed
PreviouslyEmptyCategories_AreNowFilled
NewCategoryGoods_AreCanonicalItemIds_AndEconomicallyDistinct
NewGoods_PriceWithinClampBand_AndDeterministic
Baseline40_AreUntouchedByTheCategoryFill
```


# Appendix R.591 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Shelter/Plan210_212CrossPlanIntegrationTests.cs`

### `Ashfall.Core.Tests/Shelter/Plan210_212CrossPlanIntegrationTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 224; SHA-256: `3245ab1c311ab30c0be915dde88e37bb28f7befdfd6bc50335aea44b1cfeb6c3`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Rules_ExposureSweep_RunsOnlyWhenSanitationIsPoorOrSpilled
Rules_MoraleMark_IsReversible_HazardousOnly
Rules_CrisisDemandShock_IsBoundedAndRefreshSafe
SanitationCrisis_RaisesMedicalPrices_ThroughTheCanonicalShock
MedicineShortage_RaisesBothCanonicalAndBlackMarketPrices_OnePipeline
DiseaseSweep_FeedsTheAuthoredSource_AndProbabilityModifierChangesOutcomes
HazardousMark_SetOnCollapse_ClearedOnRecovery
MetallurgySupply_SellingOutputSoftensTheIndex_Structurally
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.


# Appendix S — Quality Assurance Pass Record: Plan 56

This record is part of the planning artifact, not a fresh runtime test result.

## Pass 1 — content and premise accuracy
- Content pass replaced the stale 16→40 target with the current 51-good census.
- The historical baseline is separated from the current source/data/test authority.
- Current row counts and owner boundaries are stated without using count as a quality proxy.

## Pass 2 — integration architecture
- Integration pass traced goods → market → settlement/caravan → inventory/save.
- Core, data, host, UI, save, event and test seams are named with current paths.
- The plan does not authorize a parallel save section, catalog, manager or host cache.

## Final precision and reaccuracy pass
- Precision pass corrected the live SettlementCatalog owner and preserves price determinism.
- Every embedded current-file hash, focused runner command and master-authority reference is rechecked.
- Any proposed future seam is labeled as requiring a separate claim and premise verification.
