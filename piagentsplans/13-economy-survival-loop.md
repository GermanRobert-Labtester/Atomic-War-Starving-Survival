# Plan 13 — Economy, Caravan, Trapping, and Weather-Crisis Integration

> **Rebuild status:** CORE INTEGRATION LARGELY PRESENT — REACHABILITY AND BALANCE AUDIT REMAINS
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

- The economy already has goods, regional prices, commodity baselines, market indices, shocks, embargoes, rationing and caravans. Trapping already owns deterministic yields, bait, equipment wear, disease mappings and persistence.
- The remaining plan is a cross-system legibility and balance package: make supply/demand visible, ensure weather affects real owners once, and prove that trap yields remain a bounded alternative rather than an exploit.
- The plan should treat weather crises as routed consequences, not a new event scheduler. Any new event must attach to existing day-owner and system seams.

**Bounded outcome:** Use `MarketSystem`, `TravelingCaravanSystem`, `WildlifeTrappingSystem`, weather owners, inventory, food safety, disease and shelter systems. Do not add catalog rows, a second price model, a second caravan economy, or a weather-event simulation parallel to existing owners.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- The market state is version 3 with demand, ledger, category indices, trade pressure, shocks, embargo and rationing state.
- Traveling caravans already use goods-derived regional supply, map routes, weather availability and embargo gates.
- The trapping catalog has 10 traps and 15 prey definitions with bait, compatibility and incident links.
- The goods catalog has 51 rows; commodity baselines and regional prices are separate authorities.
- Food preservation, kitchen, disease and greenhouse own the consequences of harvested food.

**Master-authority sections applied to this rebase:**

- Volume 8 harness H-C1/H-C3/H-C8
- Volume 17 C3/C5/C11/C12 roadmaps
- Volume 28 command cookbook

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Audit that crafting outputs and regional supply reach current market/caravan surfaces.
- Create read-only shortage/surplus projections; do not duplicate price mutation.
- Verify trapping choices route through inventory, food safety, disease and equipment condition exactly once.
- Map weather effects to existing power, shelter, medical and economy owners with bounded tests.

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
| prices, demand, indices, shocks and ledger | MarketSystem | `Assets/Ashfall.Core/Economy/MarketSystem.cs` | Sole pricing authority. |
| routes, stock, travel and trade completion | TravelingCaravanSystem | `Assets/Ashfall.Core/TravelingCaravanSystem.cs` | Sole caravan state owner. |
| trap placement, catches, events and state | WildlifeTrappingSystem | `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` | Sole trapping owner. |
| weather kind/effects and forecasts | WeatherSystem | `Assets/Ashfall.Core/World/WeatherSystem.cs` | Weather truth; downstream owners apply effects. |
| safety and contamination consequences | Food/disease owners | `Inventory, KitchenNutritionSystem, DiseaseSystem` | Trapping never writes these states directly. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Economy, Caravan, Trapping, and Weather-Crisis Integration
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ MarketSystem
│   prices, demand, indices, shocks and ledger
│ TravelingCaravanSystem
│   routes, stock, travel and trade completion
│ WildlifeTrappingSystem
│   trap placement, catches, events and state
│ WeatherSystem
│   weather kind/effects and forecasts
│ Food/disease owners
│   safety and contamination consequences
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

1. **Preserve current state ownership.** MarketSystem owns prices, demand, indices, shocks and ledger: Sole pricing authority.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| prices, demand, indices, shocks and ledger | MarketSystem | `Assets/Ashfall.Core/Economy/MarketSystem.cs` | Sole pricing authority. |
| routes, stock, travel and trade completion | TravelingCaravanSystem | `Assets/Ashfall.Core/TravelingCaravanSystem.cs` | Sole caravan state owner. |
| trap placement, catches, events and state | WildlifeTrappingSystem | `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` | Sole trapping owner. |
| weather kind/effects and forecasts | WeatherSystem | `Assets/Ashfall.Core/World/WeatherSystem.cs` | Weather truth; downstream owners apply effects. |
| safety and contamination consequences | Food/disease owners | `Inventory, KitchenNutritionSystem, DiseaseSystem` | Trapping never writes these states directly. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. goods/recipes/trap definitions validate
2. regional supply or trap action produces bounded output
3. owner calculates price/yield/event
4. host applies inventory/need/disease/economy effect
5. daily owner advances market/caravan/trap/weather
6. save captures each owner once

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Market and caravan states persist through their current sections.
- Trapping state persists trap sites, events and harvest history.
- Weather is catalog/current system state, not copied per consumer.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A caravan quote uses MarketSystem or an explicit barter owner, never panel arithmetic.
- A trap catch is granted once and consumes trap durability once.
- Weather applies through the canonical effects table and owner-specific port.
- A food catch cannot bypass preservation, preparation or disease rules.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- No new goods, recipes or prey rows in the audit package.
- Use existing authored regionalSupply and trap fields.
- New event rows require a consumer and owner.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Current market/caravan/trap/weather saves remain authoritative.
- No new aggregate economy save is allowed.
- Restore must preserve active shocks, blocked transitions and trap events.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Market volatility and trap rolls use injected seeded streams.
- Caravan and weather day transitions are deterministic.
- No system consumes another system’s private RNG.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Market raises price/economy/shock events.
- Caravan raises arrival/trade/embargo/weather events.
- Trapping raises catch/bycatch/disease events.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/TravelingCaravanHostSession.cs
- src/Host/WildlifeTrappingHostSession.cs
- src/UI/EconomyMarketPanel.cs
- src/UI/WildlifeTrappingPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Trade/radio/weather prose reflects current owner facts and does not promise guaranteed routes or yields.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A caravan sells an item absent from its inventory. | MarketSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A trap catch is duplicated after reload. | TravelingCaravanSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Weather modifies both dispatch and estimate using different sampled values. | WildlifeTrappingSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A spoiled catch bypasses food safety. | WeatherSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | Regional supply falls back to hidden hardcoded stock when catalog binding exists. | Food/disease owners | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/EconomySystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/TravelingCaravanSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingReplayTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingDiseaseMappingTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan20CConsumerWiringTests.cs`
6. `godot --headless --path . -- --data-integrity-selftest` when a catalog reference changes.

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — current economy census | Inventory current goods, supply provenance, prices and caravan stock paths. | No duplicate price/supply owner. | No production path until the owning implementation package is separately claimed. |
| 1 — crafting-to-market trace | Follow production outputs to market and caravan demand. | Every output has an intended path. | No production path until the owning implementation package is separately claimed. |
| 2 — trapping consequence trace | Verify catch, wear, contamination and food-safety handoffs. | No double charge or bypass. | No production path until the owning implementation package is separately claimed. |
| 3 — weather consequence matrix | Map each relevant weather effect to current owner and event. | One application per effect. | No production path until the owning implementation package is separately claimed. |
| 4 — balance harness | Measure caravan viability, trap yield and weather pressure. | No tuning without evidence. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/Ashfall.Core/Economy/MarketSystem.cs | READ; MODIFY only for proven integration gap | Pricing owner |
| Assets/Ashfall.Core/TravelingCaravanSystem.cs | READ | Caravan owner |
| Assets/Ashfall.Core/WildlifeTrappingSystem.cs | READ | Trapping owner |
| src/UI/TravelingCaravanPanel.cs | READ; MODIFY only for truth gap | Presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Second price formula. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Weather double application. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Catalog supply and hidden fallback stock disagree. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Trap system bypasses food/disease owners. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Balance changes hidden inside documentation work. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new goods/recipes/prey.
- No new currency.
- No economy-wide rewrite.
- No full campaign balance pass without a dedicated window.

# 23. Rollback and Recovery

- Read projections are reversible.
- Any state change is isolated with migration and replay tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- Current row counts and authorities are recorded.
- Craft/market/caravan path is explicit.
- Trapping consequences are single-owner.
- Weather routes through canonical effects.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Audit that crafting outputs and regional supply reach current market/caravan surfaces.
- Create read-only shortage/surplus projections; do not duplicate price mutation.
- Verify trapping choices route through inventory, food safety, disease and equipment condition exactly once.
- Map weather effects to existing power, shelter, medical and economy owners with bounded tests.

## MUST NOT DO

- No new goods/recipes/prey.
- No new currency.
- No economy-wide rewrite.
- No full campaign balance pass without a dedicated window.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/EconomySystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/TravelingCaravanSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingReplayTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingDiseaseMappingTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan20CConsumerWiringTests.cs`
6. `godot --headless --path . -- --data-integrity-selftest` when a catalog reference changes.

## FIRST SAFE IMPLEMENTATION STEP

0 — current economy census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: prices, demand, indices, shocks and ledger → MarketSystem; routes, stock, travel and trade completion → TravelingCaravanSystem; trap placement, catches, events and state → WildlifeTrappingSystem; weather kind/effects and forecasts → WeatherSystem; safety and contamination consequences → Food/disease owners. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 13.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 13 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by MarketSystem or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Economy/MarketSystem.cs`

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


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/TravelingCaravanSystem.cs`

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


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/WildlifeTrappingSystem.cs`

### `Assets/Ashfall.Core/WildlifeTrappingSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1620 lines / 78902 bytes.
- SHA-256: `bc94947453b31bc97de2d069c1a56487a684e7679a736acec13c7f34e30d1c59`.
- Architecture signals: seeded references=13; save/restore symbols=2; typed event declarations=33; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WildlifeTrappingState
public string systemId = WildlifeTrappingSystem.SystemId;
public List<TrapSite> trapSites = new List<TrapSite>();
public int totalCatch;
public int totalToxicRemoved;
public List<string> firstCatchLoggedSpeciesIds = new List<string>();
public List<WildlifeTrappingPendingEvent> pendingEvents = new List<WildlifeTrappingPendingEvent>();
public int eventSequence;
public int nextDeploymentSequence;
public int rngSeed;
public ulong primaryRngState;
public int encounterRngSeed;
public ulong encounterRngState;
public int incidentRngSeed;
public ulong incidentRngState;
public sealed class TrapSite
public string siteId = string.Empty;
public string assignedHunterId = string.Empty;
public string baitType = string.Empty;
public string trapType = "snare"; // snare, deadfall, cage, pit
public string trapId = string.Empty; // Plan 36: catalog link
public int setDay = -1;
public int checkDay = -1;
public int checkIntervalDays = 2;
public int remainingDurability = -1; // -1 = legacy/untracked, >0 = operational, 0 = broken
public bool isBroken; // Plan 36: trap cannot produce catches when true
public bool hasCatch;
public string catchSpecies = string.Empty;
public string bycatchSpecies = string.Empty; // Plan 36 III: bycatch species if occurred
public float bycatchYield; // Plan VI: independently resolved secondary carcass yield
public bool bycatchToxic; // Plan VI: independently resolved secondary toxicity
public float carcassYield;
public bool isToxic;
public bool toxinRemoved;
public bool isMeatProcessed;
public bool hidePreserved;
public string diseaseId = string.Empty; // Tasks 5-8: resolved disease ID from catch
public float contaminationDose; // Tasks 5-8: resolved contamination dose in rads
public string bycatchDiseaseId = string.Empty; // Plan VI: resolved secondary disease ID
public float bycatchContaminationDose; // Plan VI: resolved secondary contamination dose
public string pendingNarrativeEvent = string.Empty;
public int deploymentSequence;
public sealed class BaitProfile
public string baitId = string.Empty;
public string displayName = string.Empty;
public float catchBonusMultiplier = 1.0f; // multiplies base catch chance
public float toxicReduction = 0.0f; // reduces toxic chance by this fraction
public List<string> preferredSpecies = new List<string>();
public int craftCostScrapMeat = 0;
public int craftCostRoots = 0;
public int craftCostChemicals = 0;
public sealed class QuarrySpecies
public string speciesId = string.Empty;
public string displayName = string.Empty;
public float baseYieldKg = 1.0f;
public float toxicChance = 0.2f;
public float hideYield = 0.0f;
public string hideItemId = string.Empty;
public string preferredTrapType = "snare";
public List<string> attractedByBaitIds = new List<string>();
public float minSkillLevel = 0.0f;
public sealed class WildlifeSelectionContext
public static readonly WildlifeSelectionContext Default = new WildlifeSelectionContext();
public string SeasonWindowId { get; set; } = string.Empty;
public WeatherKind CurrentWeather { get; set; } = WeatherKind.Clear;
public HashSet<string> PresentMigrationSpecies { get; set; } = new HashSet<string>(StringComparer.Ordinal);
public Dictionary<string, float> AbundanceFactors { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);
public Dictionary<string, float> HunterSkillLevels { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);
public sealed class WildlifeTrappingSystem
public const string SystemId = "wildlife_trapping";
public WildlifeTrappingState State => _state;
public event Action OnTrappingChanged;
public event Action<string, string, string, bool> OnButcheryCompleted; // siteId, butcherId, species, isToxic
public event Action<ButcheryCompletedEvent>? OnButcheryCompletedDetailed;
public event Action<string, string> OnHidePreserved; // siteId, hideItemId
public event Action<string, string>? OnTrophyReady;
public event Action<string, string, string>? OnNewSpeciesDiscovered;
public event Action<string, string, string, string, int, string>? OnBycatchOccurred;
public event Action<BycatchOccurredEvent>? OnBycatchResolved;
public event Action<TrapLifecycleEvent>? OnTrapDeployed;
public event Action<TrapLifecycleEvent>? OnTrapBroken;
public event Action<TrapLifecycleEvent>? OnTrapRepaired;
public event Action<TrapLifecycleEvent>? OnTrapRemoved;
public event Action<WildlifeTrappingPendingEvent>? OnPendingEventCreated;
public static int DeriveEncounterStreamSeed(int parentSeed) {
public static int DeriveIncidentStreamSeed(int parentSeed) {
public void RegisterBait(BaitProfile bait) {
public void RegisterQuarry(QuarrySpecies species) {
public void RegisterPreyDefinition(PreyDefinition prey) {
public void RegisterTrapDefinition(TrapDefinition trap) {
public void SetHunterSkill(float skillLevel) {
public void SetSelectionContext(WildlifeSelectionContext context) {
public static float SkillMultiplierFor(float skillLevel) {
public static float WeatherPenaltyFor(WeatherKind kind) {
public static float CalculateWeatherMultiplier(float weatherSensitivity, WeatherKind weather) {
public Func<WeatherKind, float>? WeatherPenaltyProvider { get; set; }
public float EffectiveWeatherPenalty(WeatherKind weather) => WeatherPenaltyProvider != null
public static float CalculatePrimaryCatchChance( float densityMultiplier, float hunterSkillLevel, float baitMultiplier, float weatherSensitivity, WeatherKind weather,
public List<WildlifeTrappingPendingEvent> GetPendingEvents() {
public bool MarkEventDelivered(string eventId) {
public int CountPendingEvents(string kind) {
public bool CanSetTrapAtSite(string siteId, out string failureCode) {
public ActionResult SetTrap(string siteId, string baitType, string hunterId, string trapType = "snare", string trapId = "", int checkIntervalDays = -1, int durabilityChecks = -1) {
public const float BaseCatchChance = 0.5f;
public List<string> GetEligibleQuarryIds(string baitType, string trapType, float hunterSkillLevel, string trapId = "") {
public ActionResult CheckTraps(float densityMultiplier = 1f) {
public ActionResult Butcher(string siteId, string butcherId = "") {
public ActionResult PreserveHide(string siteId, out string hideItemId, out float hideQuantity) {
public ActionResult TransferCatchToInventory(string siteId, Inventory.Inventory inventory, string fallbackRawMeatId = "raw_meat") {
public string GetTrophyRecipeForSpecies(string speciesId) {
public IReadOnlyDictionary<string, BaitProfile> GetBaitCatalog() => _baitCatalog;
public IReadOnlyDictionary<string, QuarrySpecies> GetQuarryCatalog() => _quarryCatalog;
public IReadOnlyDictionary<string, TrapDefinition> GetTrapDefinitionCatalog() => _trapDefinitionCatalog;
public IReadOnlyDictionary<string, PreyDefinition> GetPreyDefinitionCatalog() => _preyDefinitionCatalog;
public bool RollDiseaseRisk(float diseaseRisk) {
public bool RollContaminationRisk(float contaminationRisk) {
public ActionResult RepairTrap(string siteId, int restoreDurability) {
public ActionResult RemoveTrap(string siteId) {
public ActionResult RemoveToxin(string siteId) {
public void TickDay(int day, float densityMultiplier = 1f) {
public WildlifeTrappingState CaptureState() {
public void RestoreState(WildlifeTrappingState saved) {
public static string BuildNarrativeIncidentSourceId(string siteId, string eventId) => $"wildlife-trap:{siteId ?? string.Empty}:incident:{eventId ?? string.Empty}";
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/World/WeatherSystem.cs`

### `Assets/Ashfall.Core/World/WeatherSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 532 lines / 22049 bytes.
- SHA-256: `23309eb8803b1759e8d529078baea0677ee6d24f777601e3023fb1c6c12ea341`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class SeasonWindowDef
public string id = string.Empty;
public string displayName = string.Empty;
public int startDay = 0;
public float clearWeight = 1f;
public float rainWeight = 1f;
public float overcastWeight = 1f;
public float ashfallWeight = 1f;
public float falloutStormWeight = 1f;
public float blizzardWeight = 1f;
public float blackRainWeight = 1f;
public class SeasonProfileDef
public string id = "default_winter";
public string displayName = "The Long Winter";
public float weatherCheckIntervalHours = 6f;
public List<SeasonWindowDef> seasons = new List<SeasonWindowDef>();
public class WorldWeatherState
public string systemId = WeatherSystem.SystemId;
public string currentKind = "Clear";
public float totalElapsedHours = 0f;
public float hoursUntilNextCheck = 0f;
public int rollCount = 0;
public bool restrictToNonHazardWeather = false;
public float wind_direction_deg = 0f;
public float wind_speed_kph = 0f;
public class WeatherSystem
public const string SystemId = "world_weather_system";
public const float FalloutStormOutdoorRadModifier = 150f;
public const float BlackRainOutdoorRadModifier = 250f;
public const float BlackRainHazmatMeltMultiplier = 5f;
public const float BlizzardTemperaturePenaltyC = -15f;
public const float FalloutStormTemperaturePenaltyC = -5f;
public const float BlackRainTemperaturePenaltyC = -8f;
public const float BlizzardVisibilityFactor = 0.4f;
public event Action<WeatherKind> OnWeatherChanged;
public event Action<WorldWeatherState> OnStateChanged;
public WorldWeatherState State => _state;
public WeatherKind Current => ParseKind(_state.currentKind);
public void BindWeatherEffects(WeatherEffectsCatalog? catalog) {
public float WindDirectionDeg => _state.wind_direction_deg;
public float WindSpeedKph => _state.wind_speed_kph;
public int Seed => _seed;
public SeasonProfileDef? Profile => _profile;
public void BindProfile(SeasonProfileDef profile, int seed) {
public SeasonWindowDef GetSeasonForDay(int day) {
public void Tick(float gameHours) {
public void ForceWeather(WeatherKind kind) {
public float ForecastRadModifier(WeatherKind kind) {
public bool IsScavengingBlocked(bool hasFullSuit) =>
public float GetTemperaturePenaltyCelsius() {
public float HazmatDegradeMultiplier =>
public static float TemperaturePenaltyForWeather(WeatherKind kind) {
public float TemperaturePenaltyC(WeatherKind kind) {
public float VisibilityModifier(WeatherKind kind) {
public WorldWeatherState CaptureState() {
public void RestoreState(WorldWeatherState saved) {
public void RestrictToNonHazardWeather(bool restrict) {
public List<WeatherForecastEntry> PeekForecast(int daysAhead = 3) {
public class WeatherForecastEntry
public int Day;
public WeatherKind Kind;
public float OutdoorRad;
public float Visibility;
public string Summary = string.Empty;
public float ThermalLoadC;
public float TravelSpeedMultiplier = 1f;
public float TravelEncounterMultiplier = 1f;
public float TrapYieldMultiplier = 1f;
public float CaravanAvailabilityMultiplier = 1f;
public static class WeatherProfileLoader
public const string FileName = "weather_seasons.json";
public static SeasonProfileDef? Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix B.06 — Current Code Architecture: `Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs`

### `Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 326 lines / 12639 bytes.
- SHA-256: `7a8392e5a96ab4af5e738dff8d41c41b01a95a18cf97fad2921d7a4ee60b566e`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FoodBatchCohort
public string CohortId { get; set; } = string.Empty;
public string FoodItemId { get; set; } = string.Empty;
public int Quantity { get; set; }
public string TierId { get; set; } = "preservation_ambient";
public int DayStored { get; set; }
public float FreshnessPercent { get; set; } = 100f;
public bool IsSpoiled { get; set; }
public sealed class ActiveCuringJob
public string JobId { get; set; } = string.Empty;
public string RecipeId { get; set; } = string.Empty;
public string AssignedCookId { get; set; } = string.Empty;
public int DayStarted { get; set; }
public int ProgressTicks { get; set; }
public int TargetTicks { get; set; } = 4;
public bool IsComplete { get; set; }
public sealed class FoodPreservationState
public string SystemId { get; set; } = FoodPreservationSystem.SystemId;
public List<FoodBatchCohort> Cohorts { get; set; } = new List<FoodBatchCohort>();
public List<ActiveCuringJob> ActiveJobs { get; set; } = new List<ActiveCuringJob>();
public bool IsPowerOnline { get; set; } = true;
public int UnpoweredDays { get; set; }
public int TotalSpoiledDiscarded { get; set; }
public int TotalCured { get; set; }
public int TotalConsumed { get; set; }
public int NextCohortSeq { get; set; }
public sealed class FoodPreservationSystem
public const string SystemId = "food_preservation";
public const string DefaultStorageRoomId = "room_storage_bay";
public const float DefaultStorageTemperatureC = 10f;
public FoodPreservationState State => _state;
public bool IsPowerOnline => _state.IsPowerOnline;
public int UnpoweredDays => _state.UnpoweredDays;
public float StorageTemperatureC => _storageTemperatureC;
public event Action<FoodBatchCohort>? OnFoodSpoiled;
public event Action<ActiveCuringJob>? OnCuringCompleted;
public event Action<string, int, bool>? OnFoodConsumed;
public void SetPowerStatus(bool isOnline) {
public void SetStorageTemperatureC(float temperatureC) {
public static float StorageTempShelfLifeFactor(float temperatureC) {
public ActionResult AddCohort(string foodItemId, int quantity, string tierId, int currentDay) {
public ActionResult StartCuringJob(string recipeId, string cookId, int currentDay) {
public void TickDay(int day) {
public int ConsumeFood(string foodItemId, int neededCount, out int spoiledConsumed) {
public int DiscardSpoiled(string? foodItemId = null) {
public int GetTotalFood(string? foodItemId = null) {
public int GetSpoiledFood(string? foodItemId = null) {
public FoodPreservationState CaptureState() {
public void RestoreState(FoodPreservationState saved) {
```


# Appendix B.07 — Current Code Architecture: `src/Host/TravelingCaravanHostSession.cs`

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


# Appendix B.08 — Current Code Architecture: `src/Host/WildlifeTrappingHostSession.cs`

### `src/Host/WildlifeTrappingHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 643 lines / 28260 bytes.
- SHA-256: `9318ab54790453b20a0f62683207ac150c70862300b1fbbdba9ac6fcd06cf8e6`.
- Architecture signals: seeded references=1; save/restore symbols=1; typed event declarations=6; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WildlifeTrappingHostSession
public WildlifeTrappingSystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public WildlifeTrappingCatalog? Catalog { get; set; }
public InventoryHostSession? Inventory { get; set; }
public Action<string, string, int>? ApplyDisease { get; set; }
public Action<string, float>? ApplyContamination { get; set; }
public Func<string, string, string, bool>? DeliverMoralConsequence { get; set; }
public Func<string, string, int, bool>? DeliverTrapEncounter { get; set; }
public Func<string, bool>? DeliverTrappingBroadcast { get; set; }
public Func<string, string, int, string, bool>? DeliverNarrativeIncident { get; set; }
public event Action<BycatchOccurredEvent>? OnBycatchOccurred;
public Func<int, bool>? DeliverButcheryFood { get; set; }
public Action<string, float, string>? ApplyMorale { get; set; }
public Func<PreyDefinition, string>? DiseaseResolver { get; set; }
public event Action<string>? OnTrapCrafted;
public ActionResult SetTrap(string siteId, string baitType, string hunterId) {
public bool CanSetTrapAtSite(string siteId, out string failureCode) => System.CanSetTrapAtSite(siteId, out failureCode);
public void SetSelectionContext(WildlifeSelectionContext context) => System.SetSelectionContext(context);
public ActionResult TrySetTrap(string siteId, string trapId, string baitType, string hunterId) {
public bool TryGetSetupBill(string trapId, out InventoryBill bill, out string reason) {
public bool CanAffordSetup(string trapId, out InventoryBill bill, out string failureReason) {
public float WildlifeDensityMultiplier { get; set; } = 1f;
public ActionResult CheckTraps(float? densityMultiplier = null) {
public void DeliverPendingEvents() {
public string ComposeBroadcastMessage(WildlifeTrappingPendingEvent ev) {
public const float FallbackContaminationDose = PreyDefinition.FallbackContaminationDose;
public const string FallbackDiseaseId = PreyDefinition.FallbackDiseaseId;
public ActionResult Butcher(string siteId, string butcherId = "") {
public static string ResolveDiseaseId(PreyDefinition prey) => PreyDefinition.ResolveDiseaseId(prey);
public ActionResult RemoveToxin(string siteId) {
public ActionResult PreserveHide(string siteId) {
public bool TryGetRepairBill(string siteId, out InventoryBill bill, out string reason) {
public bool CanAffordRepair(string siteId, out InventoryBill bill, out string failureReason) {
public ActionResult TryRepairTrap(string siteId) {
public ActionResult RemoveTrap(string siteId) {
public void ReconcileMapMarkers() {
public void TickDay(int day) {
public event Action<int>? OnCatchPressure;
public override void Save() {
```


# Appendix B.09 — Current Code Architecture: `src/UI/WildlifeTrappingPanel.cs`

### `src/UI/WildlifeTrappingPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 524 lines / 21950 bytes.
- SHA-256: `9cacd807a62a562475a738f5344fae0374a26cce1df74fe7c35c191e8dabcb60`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class WildlifeTrappingPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _host != null;
public string? SelectedRepairSiteId => _selectedRepairSiteId;
public Button? RepairButton => _repairBtn;
public OptionButton? RepairSiteDropdown => _repairSiteDropdown;
public Button? SetTrapButton => _setTrapBtn;
public AshfallStatusRail? StatusRail => _statusRail;
public void Bind(WildlifeTrappingHostSession session) {
public void Unbind() {
public override void _Ready() {
public void SelectRepairSite(string siteId) {
public void RefreshView() {
public override void _ExitTree() {
```


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/economy_goods.json`

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


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/commodity_baselines.json`

### `Assets/StreamingAssets/Data/commodity_baselines.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 2808 bytes / 2806 characters.
- SHA-256: `48463176fa3e326fbc5946304073fe88e23632f9a5c950126ca5cc9ef221a849`.
- Root keys: `categories`, `collection_id`, `description`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
categories: min=12, max=12, observed_paths=1
```

Representative record fields:

- `base_multiplier_permille`
- `category_id`
- `elasticity_class`
- `scarcity_ceiling_permille`
- `scarcity_floor_permille`


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/regional_prices.json`

### `Assets/StreamingAssets/Data/regional_prices.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 3789 bytes / 3787 characters.
- SHA-256: `5dee33fbf091fdaae985b322764f32d6c2693c2a4da2a1486d0491eff4d48860`.
- Root keys: `collection_id`, `description`, `entries`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
entries: min=24, max=24, observed_paths=1
```

Representative record fields:

- `base_price_modifier_permille`
- `category`
- `item_id`
- `region`
- `scarcity_profile`

Representative identifiers (ordered, capped for readability):

```text
item_foundry_brine_pipe
item_desal_membrane
item_ro_membrane
seed_packets
trap_fish
mechanical_parts
electronic_scrap
chemicals
canned_food
cooked_meat
item_frostbite_salve
medical_kit
anti_rad
solar_cell
clean_water
item_taper_kit_opioid
tobacco_pouch
trap_box
```


# Appendix C.13 — Catalog Census: `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json`

### `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 23204 bytes / 23194 characters.
- SHA-256: `ad76e65163efc3860f4ef7d6b209479f27489c0f5cb34a717fe4e64bd5d64d00`.
- Root keys: `baits`, `prey`, `schema_version`, `traps`.

Array-path census (minimum, maximum, observed rows):

```text
baits: min=6, max=6, observed_paths=1
baits[].preferredSpecies: min=3, max=3, observed_paths=2
prey: min=15, max=15, observed_paths=1
prey[].activeSeasons: min=0, max=3, observed_paths=2
prey[].attractedByBaitIds: min=2, max=2, observed_paths=2
traps: min=10, max=10, observed_paths=1
traps[].compatiblePrey: min=4, max=5, observed_paths=2
traps[].narrativeIncidentIds: min=3, max=3, observed_paths=2
traps[].setupCosts: min=1, max=1, observed_paths=2
```

Representative record fields:

- `baseCatchModifier`
- `bycatchChance`
- `bycatchSpecies`
- `checkIntervalDays`
- `compatiblePrey`
- `description`
- `displayName`
- `durabilityChecks`
- `narrativeIncidentChance`
- `narrativeIncidentIds`
- `requiresWater`
- `setupCosts`
- `trapEncounterChance`
- `trapType`
- `trap_id`
- `weatherSensitivity`

Representative identifiers (ordered, capped for readability):

```text
trap_snare
trap_deadfall
trap_pit
trap_net
trap_fish
trap_cage
trap_bird_snare
trap_body_grip
trap_box
trap_improvised_wire
```


# Appendix C.14 — Catalog Census: `Assets/StreamingAssets/Data/weather_effects.json`

### `Assets/StreamingAssets/Data/weather_effects.json`

- Parse: valid strict JSON.
- Schema version: `2`.
- Size: 7615 bytes / 7615 characters.
- SHA-256: `2879d31c294f9f84f274e14b5b266d02d73b4f7bb1abe0a1d8604023af846600`.
- Root keys: `schema_version`, `weather_effects`.

Array-path census (minimum, maximum, observed rows):

```text
weather_effects: min=22, max=22, observed_paths=1
```

Representative record fields:

- `caravan_availability_multiplier`
- `explicitly_neutral`
- `outdoor_rad_modifier`
- `thermal_load_additive_c`
- `trap_yield_multiplier`
- `travel_encounter_multiplier`
- `travel_speed_multiplier`
- `visibility_modifier`
- `weather`


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/EconomySystemTests.cs`

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


# Appendix D.16 — Existing Focused Test Inventory: `Ashfall.Core.Tests/TravelingCaravanSystemTests.cs`

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


# Appendix D.17 — Existing Focused Test Inventory: `Ashfall.Core.Tests/WildlifeTrappingReplayTests.cs`

### `Ashfall.Core.Tests/WildlifeTrappingReplayTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 506; SHA-256: `557b71ad022e24993159476705eb6fca6c1317df56400c700987c39e21b6fb0c`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ButcheryReplay_SaveLoadBoundary_Seed42_PreservesDiseaseAndContaminationOutcome
ButcheryReplay_SaveLoadBoundary_Seed123_PreservesDiseaseAndContaminationOutcome
ButcheryReplay_LowRiskPrey_RemainsDeterministic
ButcheryReplay_FinalHealthStateHash_MatchesAfterRestore
DeterministicReplay_MultiDayCampaignTrace_UninterruptedVsSavedRestored_MatchesExactEventTraceAndHash
DeterministicReplay_ThreeConsecutiveRuns_ProduceIdenticalStateAndEventHash
DeterministicReplay_BreakageBoundary_MatchesBreakDayAndPreventsSubsequentCatches
```


# Appendix D.18 — Existing Focused Test Inventory: `Ashfall.Core.Tests/WildlifeTrappingDiseaseMappingTests.cs`

### `Ashfall.Core.Tests/WildlifeTrappingDiseaseMappingTests.cs`

- Current test declarations: Fact=5, Theory=1, InlineData=0.
- File lines: 136; SHA-256: `b385419bef39f060727cd74da6f3f9287ac2469ef3f1172540e57dda53db692d`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_ContainsExactly15Prey
AuthoritativeCatalogPrey_ResolvesExpectedDiseaseId
ThresholdMicroTest_ExactBoundary_SeparatesNoneFromZoonoticFlu
ExplicitCatalogDiseaseId_AlwaysWinsOverTierFallback
LowRiskPrey_RabbitCottonHareMirrorCarp_NeverResolveFallbackDisease
FallbackPrey_DeerFoxPheasantAshPikeMuskratHedgehogBoar_ResolveZoonoticFlu
```


# Appendix D.19 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Expeditions/Plan20CConsumerWiringTests.cs`

### `Ashfall.Core.Tests/Expeditions/Plan20CConsumerWiringTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 157; SHA-256: `acdf10792343c31cc697b22973cdd1d7bf2f8916f5b2d6e4e21b79156af38fdf`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
WeatherInputs_SlowTravel_AndRaiseEncounter
Runtime_UsesTheDispatchSampledWeatherMultiplier
Trapping_ProviderOverrides_LegacyCurve_UnboundKeepsLegacy
Trapping_ProviderBound_TablePenaltyChangesTheCatchChance
Caravan_WeatherAvailability_CombinesWithEmbargoProgress
```


# Appendix E.20 — Supporting Code Evidence: `src/Main.ShelterSocial.cs`

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


# Appendix E.21 — Supporting Code Evidence: `src/Main.Plans62_65.cs`

### `src/Main.Plans62_65.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 218 lines / 9686 bytes.
- SHA-256: `32d983d505b35066c4366e68303224a5a98a0e88b5ffccc7ef528dbf3b9febb8`.
- Architecture signals: seeded references=2; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public PrewarArchiveDecryptionSystem? ArchiveDecryptionSystem => _archiveDecryption62;
public FoodPreservationSystem? FoodPreservationSystem => _foodPreservation64;
public CampaignEpilogueEngine? CampaignEpilogueEngine => _epilogueEngine65;
public void TickPlans62To65(int day) {
```


# Appendix E.22 — Supporting Code Evidence: `Assets/Ashfall.Core/Disease/DiseaseSystem.cs`

### `Assets/Ashfall.Core/Disease/DiseaseSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1706 lines / 79732 bytes.
- SHA-256: `63336084c564afa0084a69c1bee665d9978f012387d6c26bdb58d0d7901bc8e6`.
- Architecture signals: seeded references=8; save/restore symbols=3; typed event declarations=14; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=1; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DiseaseIds
public const string ExpansionId = "expansion_disease_expansion";
public const string CatalogCollectionId = DiseaseCatalog.CollectionId;
public const string Cholera = "disease_cholera";
public const string ZoonoticFlu = "disease_zoonotic_flu";
public const string BloodFever = "disease_blood_fever";
public const string SporeBlight = "disease_spore_blight";
public const string TyphoidWaterborne = "disease_typhoid_waterborne";
public const string Dysentery = "disease_dysentery";
public const string EventInfection = "disease_infection";
public const string EventQuarantineStarted = "disease_quarantine_started";
public const string EventQuarantineEnded = "disease_quarantine_ended";
public const string EventOutbreakDeclared = "disease_outbreak_declared";
public const string EventOutbreakContained = "disease_outbreak_contained";
public const string EventRecovered = "disease_recovered";
public const string EventDied = "disease_death";
public const string EventProtocolApplied = "disease_protocol_applied";
public const string EventProtocolReset = "disease_protocol_reset";
public const string EventTreatmentApplied = "disease_treatment_applied";
public sealed class DiseaseInfectionState
public string survivor_id = string.Empty;
public int infected_day = 0;
public int days_sick = 0;
public bool quarantined = false;
public string current_stage = DiseaseStageNames.Incubating;
public int stage_entered_day = 0;
public int treatments_applied = 0;
public float lethality_reduction = 0f;
public int last_treatment_day = -1;
public bool is_diagnosed = false;
public sealed class DiseaseImmunityRecord
public string survivor_id = string.Empty;
public string disease_id = string.Empty;
public int immunity_until_day = 0;
public float strength = 1.0f;
public sealed class DiseaseExposureContext
public string SurvivorId { get; set; } = string.Empty;
public string DiseaseId { get; set; } = string.Empty;
public string SourceId { get; set; } = string.Empty;
public float ProbabilityModifier { get; set; } = 1.0f;
public bool BypassImmunity { get; set; } = false;
public int Day { get; set; } = 0;
public sealed class DiseaseExposureResult
public bool Infected { get; set; }
public string Reason { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string DiseaseId { get; set; } = string.Empty;
public float EffectiveProbability { get; set; }
public static DiseaseExposureResult CreateInfected(string survivorId, string diseaseId, float prob) =>
public static DiseaseExposureResult CreateBlocked(string reason, string survivorId, string diseaseId, float prob = 0f) =>
public sealed class DiseaseTreatmentResult
public bool Accepted { get; set; }
public string Reason { get; set; } = string.Empty;
public string Role { get; set; } = string.Empty;
public string ItemId { get; set; } = string.Empty;
public string DiseaseId { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public float LethalityReduction { get; set; }
public bool Cured { get; set; }
public static DiseaseTreatmentResult Refuse(string reason, string itemId, string diseaseId, string survivorId) =>
public static class DiseaseTreatmentRefusals
public const string NotPatient = "not_patient";
public const string UnknownDisease = "unknown_disease";
public const string NoTreatmentAuthorised = "no_treatment_authorised";
public const string ItemNotAuthorised = "item_not_authorised";
public const string OutsideWindow = "outside_window";
public const string AlreadyTreatedToday = "already_treated_today";
public const string NoSupplyChannel = "no_supply_channel";
public const string SupplyUnavailable = "supply_unavailable";
public sealed class DiseaseEntryState
public string disease_id = string.Empty;
public string vector_type = DiseaseVectorNames.Water;
public float spread_timer = 0f;
public bool outbreak_active = false;
public int deaths_during_outbreak = 0;
public int outbreaks_total = 0;
public int outbreaks_prevented = 0;
public int recovered_total = 0;
public int deaths_total = 0;
public int infections_total = 0;
public List<DiseaseInfectionState> infected = new List<DiseaseInfectionState>();
public sealed class DiseaseSystemState
public const int CurrentVersion = 2;
public int stateVersion = CurrentVersion;
public string system_id = DiseaseIds.ExpansionId;
public bool water_purified = false;
public bool vents_sealed = false;
public bool tools_sterilized = false;
public bool air_filtration = false;
public int water_purified_until_day = 0;
public int vents_sealed_until_day = 0;
public int tools_sterilized_until_day = 0;
public int air_filtration_until_day = 0;
public int rngSeed = 0;
public long rngPosition = 0;
public List<DiseaseEntryState> diseases = new List<DiseaseEntryState>();
public List<DiseaseImmunityRecord> immunities = new List<DiseaseImmunityRecord>();
public sealed class DiseasePatientSnapshot
public string survivor_id = string.Empty;
public string disease_id = string.Empty;
public string disease_name = string.Empty;
public int days_sick = 0;
public bool quarantined = false;
public bool contagious = false;          // past incubation, not isolated
public int contagion_risk_percent = 0;   // infectivity * 100
public int treatments_applied = 0;
public float effective_lethality = 0f;
public string current_stage = DiseaseStageNames.Incubating;
public string stage_token = "incubating";
public sealed class DiseaseSnapshot
public int total_infected = 0;
public int total_quarantined = 0;
public int total_contagious = 0;
public int total_outbreaks = 0;
public int total_outbreaks_prevented = 0;
public int total_recovered = 0;
public int total_deaths = 0;
public List<DiseasePatientSnapshot> patients = new List<DiseasePatientSnapshot>();
public sealed class DiseaseSystem
public const int DefaultSeed = 1013;
public const int OutbreakThreshold = 3;
public const float MaxLethalityReduction = 0.9f;
public event Action<string, string> OnInfection;                    // survivorId, diseaseId
public event Action<string, string> OnQuarantineStarted;            // survivorId, diseaseId
public event Action<string, string> OnQuarantineEnded;              // survivorId, diseaseId
public event Action<string> OnOutbreakDeclared;                     // diseaseId
public event Action<string, bool> OnOutbreakContained;              // diseaseId, prevented
public event Action<string, string, bool> OnOutcomeResolved;        // survivorId, diseaseId, recovered
public event Action<string, string, string, string, int>? OnTreatmentApplied;
public event Action<DiseaseSystemState> OnStateChanged;
public event Action<string, string> OnEventRaised;                  // eventId, detail
public Func<string, string, float>? EffectiveLethalityModifier;
public event Action<string, string>? OnStrainMutated;
public Func<string, float>? GetIsolationQuality;
public Func<float>? OnsetProbabilityMultiplier { get; set; }
public ContainmentCapability Containment { get; set; } = ContainmentCapability.None;
public DiseaseSystemState State => _state;
public DiseaseCatalog Catalog => _catalog;
public string SystemId => _state.system_id;
public bool HasImmunity(string survivorId, string diseaseId, int currentDay) {
public DiseaseImmunityRecord? GetImmunity(string survivorId, string diseaseId) {
public void SetImmunity(string survivorId, string diseaseId, int untilDay, float strength = 1.0f) {
public DiseaseExposureResult TryExpose(DiseaseExposureContext context) {
public DiseaseExposureResult TryInfect(string survivorId, string diseaseId, int day, string? sourceId = null) {
public void BindCatalog(DiseaseCatalog catalog) {
public DiseaseDefinition? GetDefinition(string diseaseId) {
public bool RegisterStrain(DiseaseDefinition strainDefinition) {
public void Infect(string survivorId, string diseaseId, int day) {
public event Action<string, string, string, int>? OnOutbreakTriggered;
public DiseaseOutbreakResult TriggerOutbreak( IDiseaseOutbreakSource source, string diseaseId, int day, IReadOnlyList<string>? candidates = null) {
public void TickDaily(int day, IReadOnlyList<string>? candidates = null) {
public void Quarantine(string survivorId, string diseaseId) {
public void EndQuarantine(string survivorId, string diseaseId) {
public bool MutateInfection(string survivorId, string fromDiseaseId, string toDiseaseId) {
public Func<string, int, bool>? TryConsumeItem;
public DiseaseTreatmentResult TryTreat( string survivorId, string diseaseId, string itemId, int day) {
public float GetEffectiveLethality(string survivorId, string diseaseId) {
public bool IsContagious(string survivorId, string diseaseId) {
public bool IsInfected(string survivorId, string diseaseId) {
public bool IsQuarantined(string survivorId, string diseaseId) {
public bool TryGetInfection(string survivorId, string diseaseId, out int daysSick, out bool quarantined) {
public bool Diagnose(string survivorId, string diseaseId) {
public bool IsDiagnosed(string survivorId, string diseaseId) {
public DiseaseClinicalPicture GetClinicalPicture(string survivorId, string diseaseId) {
public string GetTransmissionVector(string diseaseId) {
public bool IsVectorBlocked(string vectorType) {
public void PurifyWater(int day = 0) {
public void ResetWaterPurification() {
public void SealVents(int day = 0) {
public void ResetVentSeal() {
public void SterilizeTools(int day = 0) {
public void ResetToolSterilization() {
public void SetAirFiltration(bool active, int day = 0) {
public void TickProtocolExpiry(int day) {
public int ProtocolDaysRemaining(string vectorType, int today) {
public DiseaseSnapshot GetSnapshot() {
public DiseaseEntryState? GetDiseaseState(string diseaseId) {
public DiseaseSystemState CaptureState() {
public void RestoreState(DiseaseSystemState saved) {
```


# Appendix E.23 — Supporting Code Evidence: `Assets/Ashfall.Core/WildlifeTrappingCatalog.cs`

### `Assets/Ashfall.Core/WildlifeTrappingCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 388 lines / 16243 bytes.
- SHA-256: `a0934ce15a436f9b891633cf235112c96f0a716601ae22d4174e0096ee0dc63c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TrapDefinition
public string trap_id = string.Empty;
public string displayName = string.Empty;
public string description = string.Empty;
public string trapType = "snare";
public List<TrapSetupCost> setupCosts = new List<TrapSetupCost>();
public int checkIntervalDays = 2;
public int durabilityChecks = 8;
public float baseCatchModifier = 1.0f;
public List<string> compatiblePrey = new List<string>();
public bool requiresWater = false;
public float weatherSensitivity = 0.0f;
public float networkPenaltyPerTrap = 0f;
public float bycatchChance = 0f; // Plan 36 III: probability of bycatch on successful catch
public List<BycatchCandidate> bycatchSpecies = new List<BycatchCandidate>(); // Plan 36 III: weighted bycatch pool
public float narrativeIncidentChance = 0f;
public List<string> narrativeIncidentIds = new List<string>();
public float trapEncounterChance = 0f;
public InventoryBill CalculateRepairBill() {
public InventoryBill CalculateSetupBill() {
public bool Validate(out string error) {
public sealed class TrapSetupCost
public string itemId = string.Empty;
public int amount = 1;
public sealed class BycatchCandidate
public string speciesId = string.Empty;
public float weight = 1.0f;
public sealed class PreyDefinition
public string speciesId = string.Empty;
public string displayName = string.Empty;
public string description = string.Empty;
public float baseYieldKg = 1.0f;
public float toxicChance = 0.2f;
public float hideYield = 0.0f;
public string hideItemId = string.Empty;
public string preferredTrapType = "snare";
public List<string> attractedByBaitIds = new List<string>();
public float minSkillLevel = 0.0f;
public string migrationSpeciesId = string.Empty;
public List<string> activeSeasons = new List<string>();
public float diseaseRisk = 0.1f;
public float contaminationRisk = 0.05f;
public string diseaseId = string.Empty; // Plan 36 Closure II: per-species disease mapping
public float contaminationDose = 0f; // Plan 36 Closure II: explicit contamination dose in rads
public float moraleEffect = 0f;
public float moralWeight = 0f;
public bool isRareSpecies = false;
public const string FallbackDiseaseId = "disease_zoonotic_flu";
public const float FallbackContaminationDose = 2.0f;
public bool Validate(out string error) {
public string ResolveDiseaseId() => ResolveDiseaseId(this);
public static string ResolveDiseaseId(PreyDefinition? prey) {
public static string ResolveDiseaseId(float diseaseRisk, string? explicitDiseaseId = null) {
internal sealed class WildlifeTrappingCatalogFileRaw
public int schema_version = 1;
public List<TrapDefinition> traps = new List<TrapDefinition>();
public List<PreyDefinition> prey = new List<PreyDefinition>();
public List<BaitProfile> baits = new List<BaitProfile>();
public static class WildlifeTrappingCatalogLoader
public const string FileName = "wildlife_trapping_catalog.json";
public static WildlifeTrappingCatalog? Load( string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null) {
public sealed class WildlifeTrappingCatalog
public IReadOnlyDictionary<string, TrapDefinition> Traps => _traps;
public IReadOnlyDictionary<string, PreyDefinition> Prey => _prey;
public IReadOnlyDictionary<string, BaitProfile> Baits => _baits;
public void RegisterWith(WildlifeTrappingSystem system) {
```


# Appendix E.24 — Supporting Code Evidence: `Assets/Ashfall.Core/Disease/DiseaseQuarantineCoordinator.cs`

### `Assets/Ashfall.Core/Disease/DiseaseQuarantineCoordinator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 375 lines / 15683 bytes.
- SHA-256: `3a926de1f7f6e5bb05b4f4bd0a57492ceb92dac2142f13ddd6ca4aa3303529fc`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DiseaseQuarantineCoordinator
public event Action<string, string, int>? OnQuarantineAssigned;
public event Action<string, string, int>? OnQuarantineReleased;
public event Action<int, int, float>? OnDailyBurdenProcessed;
public MedicalWardSystem Ward => _medicalWard;
public DiseaseSystem Disease => _diseaseSystem;
public DutyRosterSystem? Roster => _dutyRoster;
public ContainmentCapability Containment =>
public bool IsIsolated(string survivorId) {
public float GetIsolationQuality(string survivorId) {
public MedicalBed? FindAvailableIsolationBed() {
public QuarantineCommandPreview PreviewAssignIsolation(string survivorId) {
public QuarantineCommandResult ExecuteAssignIsolation(string survivorId, int day) {
public QuarantineCommandPreview PreviewReleaseIsolation(string survivorId) {
public QuarantineCommandResult ExecuteReleaseIsolation(string survivorId, int day) {
public void TickDaily(int day) {
public void Rehydrate() {
public sealed class QuarantineCommandPreview
public bool CanExecute { get; set; }
public string Reason { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string TargetBedId { get; set; } = string.Empty;
public string? ConflictingRole { get; set; }
public float ProjectedIsolationQuality { get; set; } = 1.0f;
public Dictionary<string, int> DailySupplyCost { get; set; } = new Dictionary<string, int>();
public static QuarantineCommandPreview Success(string survivorId, string bedId, string? conflictingRole, float projectedQuality, Dictionary<string, int> costs) =>
public static QuarantineCommandPreview Blocked(string reason, string survivorId) =>
public sealed class QuarantineCommandResult
public bool Success { get; set; }
public string Reason { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string BedId { get; set; } = string.Empty;
public int Day { get; set; }
public static QuarantineCommandResult Ok(string survivorId, string bedId, int day) =>
public static QuarantineCommandResult Fail(string reason, string survivorId, string bedId = "", int day = 0) =>
```


# Appendix G.25 — Supporting Regression Evidence: `Ashfall.Core.Tests/WildlifeTrappingIntegrationTests.cs`

### `Ashfall.Core.Tests/WildlifeTrappingIntegrationTests.cs`

- Current test declarations: Fact=52, Theory=0, InlineData=0.
- File lines: 1375; SHA-256: `dbd9b3501f9e0e35166d96a69a821ca6e77baf8bdcc983d4a9329210311ef04b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SetTrap_AndCheck_ResolvesCatch
SaveAndRestore_PreservesTrapSites
WT_WX_001_ZeroSensitivityTrap_IgnoresWeatherPenalty
WT_WX_002_WeatherPenalty_FalloutStormWithSensitivity03_Produces15PercentReduction
WT_WX_003_WeatherPenalty_BlizzardWithSensitivity03_Produces24PercentReduction
WT_WX_004_ClearWeather_ProducesZeroPenalty
WT_WX_005_DeterministicReplay_SameSeedAndWeather_ProducesIdenticalCatch
WT_WX_006_BycatchIsolation_WeatherDoesNotAlterBycatchFormula
WT_WX_007_DurabilityDecrementsOnCheck_RegardlessOfWeather
WT_WX_008_ExhaustiveEnumPolicy_EveryWeatherKindHasExplicitMapping
WT_WX_009_PrimaryCatchChance_ClampsBetween005And095
WT_SK_001_SkillMultiplier_CurveEvaluation
WT_SK_002_SkillMultiplier_ClampsOutOfRangeValues
WT_SK_003_PerSiteHunterSkill_UsesAssignedHunterProgression
WT_SK_004_UnassignedSite_FallsBackToGlobalHunterSkill
WT_SK_005_SkillProgression_GetDisciplineProgress01_NormalizesCorrectly
WT_SK_006_TwoTraps_TwoHunters_EvaluatedIndependently
WT_SK_007_QuarryEligibilityPerHunter_MinSkillLevelGating
WT_SK_008_MidCampaignProgressionUpdate_SeenOnNextCheck
WT_SK_009_BycatchIsolation_SkillDoesNotModifyBycatch
WT_SK_010_DurabilityIsolation_SkillDoesNotAlterDurabilityDecrement
WT_SK_011_SharedAuthorityGuard_TrappingResolvesSharedSkillProgression
WT_JC_001_FirstCatch_FiresOnNewSpeciesDiscovered
WT_JC_002_SecondCatchSameSpecies_DoesNotFireEventAgain
WT_JC_003_DifferentSpecies_SequentialDiscovery_FiresOnceEach
WT_JC_004_FirstCatchLoggedSpeciesIds_RoundTripsThroughSaveRestore
WT_JC_005_LegacySaveWithoutFirstCatch_RestoresAsEmptyList
WT_JC_006_JournalEntry_CreatedOnce_WithValidAuthorAndDedup
WT_JC_007_JournalSystem_UnlockWildlifeCaught_UnlocksCodexKey
WT_JC_008_CodexEntries_ContainsAll15AuthoritativePreySpecies
WT_JC_009_Bycatch_NotCountedAsFirstCatch
WT_CS_001_DataContract_ImprovisedWire_RequiresNoStation
WT_CS_002_DataContract_BoxTrap_RequiresWorkbench
WT_CS_003_DataContract_FishTrap_RequiresWorkbench
WT_CS_004_StationlessRecipe_CraftableWithoutWorkbench
WT_CS_005_BoxTrap_BlockedWithoutWorkbench
WT_CS_006_FishTrap_BlockedWithoutWorkbench
WT_CS_007_BrokenWorkbench_BlocksBoxAndFishTraps
WT_CS_008_OperationalWorkbench_AllowsBoxTrap
WT_CS_009_OperationalWorkbench_AllowsFishTrap
WT_CS_010_ShelterNotBuilt_WorkbenchAbsent
WT_CS_011_ShelterBuilt_WorkbenchSynchronizes
WT_CS_012_StationLosesAvailability_BlocksNewCraft
WT_CS_013_NoUnconditionalProductionSeed_SourceGate
WT_XI_001_DailyWorldRefresh_OccursBeforeTrapCheck
WT_XI_002_FullCheck_UsesWeather_Density_Hunter_And_Bait
WT_XI_003_PostLoadContextRebuild_BeforeCheck
WT_XI_004_DiseaseAndContaminationBridge_Unchanged
WT_XI_005_OverhuntCatchPressure_Unchanged
WT_XI_006_PanelBinding_StillWorks_SourceGate
WildlifeTrapping_EndToEnd_CraftDeployMigrateButcherSaveRestoreBreak_IsDeterministic
WildlifeTrapping_EndToEnd_CatalogIntegrity_Deploy_BaitReach_Durability_SaveRoundTrip
```


# Appendix G.26 — Supporting Regression Evidence: `Ashfall.Core.Tests/WildlifeTrappingCatalogTests.cs`

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


# Appendix G.27 — Supporting Regression Evidence: `Ashfall.Core.Tests/WildlifeTrapping/WildlifeTrappingRuntimeCompletionTests.cs`

### `Ashfall.Core.Tests/WildlifeTrapping/WildlifeTrappingRuntimeCompletionTests.cs`

- Current test declarations: Fact=28, Theory=0, InlineData=0.
- File lines: 995; SHA-256: `c423548dfec379a10a223738751c33a1f1eca7fdd0fe0eaff87bea1f62eccd1a`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Task1_01_SetupCosts_ExactBalance_AtomicallyDeductedAndTrapDeployed
Task1_02_SetupCosts_ShortByOneItem_FailsAtomically_ZeroItemsConsumed
Task1_03_SetupCosts_DuplicateCostEntries_AggregatedCorrectly
Task1_04_SetupCosts_ZeroCostTrap_DeploysWithoutDeductions
Task1_05_SetupCosts_TrapIdentityPreservedAcrossSaveLoad
Task2_01_Durability_DecrementsOnCheck_Catch
Task2_02_Durability_DecrementsOnCheck_NoCatch
Task2_03_Durability_BreaksAtZero
Task2_04_Durability_BrokenTrapProducesNoCatches_SkipsRng
Task2_05_Durability_LegacySave_DefaultsToMinusOne_NeverBreaks
Task2_06_Durability_SnareBreaksEarlierThanCage
Task2_07_Repair_RestoresDefinitionDurability_ClearsBroken
Task2_08_RepairBill_AffordabilityPreflight_WireAndBox
Task3_01_Crafting_ThreeCoreTrapRecipesExistAndResolveCleanly
Task3_02_Crafting_CraftItemThenDeploy_ConsumesItemWithoutDoubleCharging
Task4_01_SeasonGating_PreyExcludedOutOfSeason
Task4_02_SeasonGating_PreyIncludedInSeason
Task4_03_SeasonGating_YearRoundPrey_AlwaysEligible
Task4_04_MigrationGating_AbsentPackExcludesMigrationPrey
Task4_05_MigrationGating_PresentPackIncludesMigrationPrey
Task4_06_SeasonalAbundance_ZeroAbundanceExcludesPrey
Task4_07_DeterministicReplay_SameContextProducesIdenticalCatch
Task5_01_HighRiskPrey_DiseaseAndContaminationRecordedOnCatch
Task5_02_DeterministicMiss_NoDiseaseOrContaminationApplied
Task5_03_Contamination_DoseAppliedToRadiationSystem
Task5_04_Disease_InfectionRecordedInDiseaseSystem
Task5_05_SaveLoad_PreservesCatchRiskFields
Task8_01_Deterministic100DaySimulation_AndBaselineMetrics
```


# Appendix G.28 — Supporting Regression Evidence: `Ashfall.Core.Tests/WildlifeTrappingPersistenceTests.cs`

### `Ashfall.Core.Tests/WildlifeTrappingPersistenceTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 552; SHA-256: `e855f5ca0b876976f340316c20d93b96a296ca5c02c9c911a55fbf3843333aef`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CaptureSerializeDeserialize_PartiallyWornTrap_PreservesNewFields
CaptureSerializeDeserialize_BrokenTrap_PreservesBrokenState
Deserialize_LegacyTrapWithoutDurabilityFields_UsesFunctionalDefaults
RestoreLegacyThenCapture_EmitsCurrentTrapFields
WildlifeTrappingSaveStore_RoundTrip_PreservesDiseaseContaminationAndTrapFields
LegacySaveFixture_PrePlan36_LoadsWithFunctionalDefaultsAndPreservesState
MixedSaveFixture_LoadsAndPreservesAllFourTrapCategories
RestoreState_NullStringFields_NormalizedToEmptyString
RestoreState_NegativeOrInconsistentDurability_NormalizedCorrectly
```


# Appendix H.29 — Supporting Authority Document: `docs/EXPEDITION_BALANCE_BASELINE.md`

### `docs/EXPEDITION_BALANCE_BASELINE.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 36 lines / 1769 bytes.
- SHA-256: `10637966712ee11a19c833ff55127d4faaddf63e98bcddfd61c134fe8153b9fd`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.30 — Supporting Authority Document: `docs/balance/BALANCE_SIM_STARTING_PROFILES.md`

### `docs/balance/BALANCE_SIM_STARTING_PROFILES.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 5169 lines / 265991 bytes.
- SHA-256: `16a5497735174dbace684a14c0a98b77d54cbdcf1971d283c21f9109e7b6f71c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum StartingProfileId
public enum BalancePillarType
public readonly struct StartingProfileBalanceSnapshot : IEquatable<StartingProfileBalanceSnapshot>
public readonly StartingProfileId ProfileId;
public readonly int SustenanceScore;
public readonly int DefenseScore;
public readonly int InfrastructureScore;
public readonly int TotalValueScrap;
public readonly bool IsDominant;
public readonly bool HasProgressionGatedItem;
public readonly long CalculatedTick;
public bool Equals(StartingProfileBalanceSnapshot other) {
public override bool Equals(object obj) => obj is StartingProfileBalanceSnapshot other && Equals(other);
public override int GetHashCode() => (ProfileId, TotalValueScrap, IsDominant).GetHashCode();
public sealed class StartingProfileBalanceAuditor
public IReadOnlyList<StartingProfileBalanceSnapshot> Profiles => _profiles.AsReadOnly();
public StartingProfileBalanceSnapshot AuditProfile( StartingProfileId profileId, int sustenance, int defense, int infrastructure, int totalScrap,
public string ComputeStateDigest() {
public class StartingProfileBalanceTests
public void Test_001_StartingProfile_BalanceAudit_Invariant_1() {
public void Test_002_StartingProfile_BalanceAudit_Invariant_2() {
public void Test_003_StartingProfile_BalanceAudit_Invariant_3() {
public void Test_004_StartingProfile_BalanceAudit_Invariant_4() {
public void Test_005_StartingProfile_BalanceAudit_Invariant_5() {
public void Test_006_StartingProfile_BalanceAudit_Invariant_6() {
public void Test_007_StartingProfile_BalanceAudit_Invariant_7() {
public void Test_008_StartingProfile_BalanceAudit_Invariant_8() {
public void Test_009_StartingProfile_BalanceAudit_Invariant_9() {
public void Test_010_StartingProfile_BalanceAudit_Invariant_10() {
public void Test_011_StartingProfile_BalanceAudit_Invariant_11() {
public void Test_012_StartingProfile_BalanceAudit_Invariant_12() {
public void Test_013_StartingProfile_BalanceAudit_Invariant_13() {
public void Test_014_StartingProfile_BalanceAudit_Invariant_14() {
public void Test_015_StartingProfile_BalanceAudit_Invariant_15() {
public void Test_016_StartingProfile_BalanceAudit_Invariant_16() {
public void Test_017_StartingProfile_BalanceAudit_Invariant_17() {
public void Test_018_StartingProfile_BalanceAudit_Invariant_18() {
public void Test_019_StartingProfile_BalanceAudit_Invariant_19() {
public void Test_020_StartingProfile_BalanceAudit_Invariant_20() {
public void Test_021_StartingProfile_BalanceAudit_Invariant_21() {
public void Test_022_StartingProfile_BalanceAudit_Invariant_22() {
public void Test_023_StartingProfile_BalanceAudit_Invariant_23() {
public void Test_024_StartingProfile_BalanceAudit_Invariant_24() {
public void Test_025_StartingProfile_BalanceAudit_Invariant_25() {
public void Test_026_StartingProfile_BalanceAudit_Invariant_26() {
public void Test_027_StartingProfile_BalanceAudit_Invariant_27() {
public void Test_028_StartingProfile_BalanceAudit_Invariant_28() {
public void Test_029_StartingProfile_BalanceAudit_Invariant_29() {
public void Test_030_StartingProfile_BalanceAudit_Invariant_30() {
public void Test_031_StartingProfile_BalanceAudit_Invariant_31() {
public void Test_032_StartingProfile_BalanceAudit_Invariant_32() {
public void Test_033_StartingProfile_BalanceAudit_Invariant_33() {
public void Test_034_StartingProfile_BalanceAudit_Invariant_34() {
public void Test_035_StartingProfile_BalanceAudit_Invariant_35() {
public void Test_036_StartingProfile_BalanceAudit_Invariant_36() {
public void Test_037_StartingProfile_BalanceAudit_Invariant_37() {
public void Test_038_StartingProfile_BalanceAudit_Invariant_38() {
public void Test_039_StartingProfile_BalanceAudit_Invariant_39() {
public void Test_040_StartingProfile_BalanceAudit_Invariant_40() {
public void Test_041_StartingProfile_BalanceAudit_Invariant_41() {
public void Test_042_StartingProfile_BalanceAudit_Invariant_42() {
public void Test_043_StartingProfile_BalanceAudit_Invariant_43() {
public void Test_044_StartingProfile_BalanceAudit_Invariant_44() {
public void Test_045_StartingProfile_BalanceAudit_Invariant_45() {
public void Test_046_StartingProfile_BalanceAudit_Invariant_46() {
public void Test_047_StartingProfile_BalanceAudit_Invariant_47() {
public void Test_048_StartingProfile_BalanceAudit_Invariant_48() {
public void Test_049_StartingProfile_BalanceAudit_Invariant_49() {
public void Test_050_StartingProfile_BalanceAudit_Invariant_50() {
public void Test_051_StartingProfile_BalanceAudit_Invariant_51() {
public void Test_052_StartingProfile_BalanceAudit_Invariant_52() {
public void Test_053_StartingProfile_BalanceAudit_Invariant_53() {
public void Test_054_StartingProfile_BalanceAudit_Invariant_54() {
public void Test_055_StartingProfile_BalanceAudit_Invariant_55() {
public void Test_056_StartingProfile_BalanceAudit_Invariant_56() {
public void Test_057_StartingProfile_BalanceAudit_Invariant_57() {
public void Test_058_StartingProfile_BalanceAudit_Invariant_58() {
public void Test_059_StartingProfile_BalanceAudit_Invariant_59() {
public void Test_060_StartingProfile_BalanceAudit_Invariant_60() {
public void Test_061_StartingProfile_BalanceAudit_Invariant_61() {
public void Test_062_StartingProfile_BalanceAudit_Invariant_62() {
public void Test_063_StartingProfile_BalanceAudit_Invariant_63() {
public void Test_064_StartingProfile_BalanceAudit_Invariant_64() {
public void Test_065_StartingProfile_BalanceAudit_Invariant_65() {
public void Test_066_StartingProfile_BalanceAudit_Invariant_66() {
public void Test_067_StartingProfile_BalanceAudit_Invariant_67() {
public void Test_068_StartingProfile_BalanceAudit_Invariant_68() {
public void Test_069_StartingProfile_BalanceAudit_Invariant_69() {
public void Test_070_StartingProfile_BalanceAudit_Invariant_70() {
public void Test_071_StartingProfile_BalanceAudit_Invariant_71() {
public void Test_072_StartingProfile_BalanceAudit_Invariant_72() {
public void Test_073_StartingProfile_BalanceAudit_Invariant_73() {
public void Test_074_StartingProfile_BalanceAudit_Invariant_74() {
public void Test_075_StartingProfile_BalanceAudit_Invariant_75() {
public void Test_076_StartingProfile_BalanceAudit_Invariant_76() {
public void Test_077_StartingProfile_BalanceAudit_Invariant_77() {
public void Test_078_StartingProfile_BalanceAudit_Invariant_78() {
public void Test_079_StartingProfile_BalanceAudit_Invariant_79() {
public void Test_080_StartingProfile_BalanceAudit_Invariant_80() {
public void Test_081_StartingProfile_BalanceAudit_Invariant_81() {
public void Test_082_StartingProfile_BalanceAudit_Invariant_82() {
public void Test_083_StartingProfile_BalanceAudit_Invariant_83() {
public void Test_084_StartingProfile_BalanceAudit_Invariant_84() {
public void Test_085_StartingProfile_BalanceAudit_Invariant_85() {
public void Test_086_StartingProfile_BalanceAudit_Invariant_86() {
public void Test_087_StartingProfile_BalanceAudit_Invariant_87() {
public void Test_088_StartingProfile_BalanceAudit_Invariant_88() {
public void Test_089_StartingProfile_BalanceAudit_Invariant_89() {
public void Test_090_StartingProfile_BalanceAudit_Invariant_90() {
public void Test_091_StartingProfile_BalanceAudit_Invariant_91() {
public void Test_092_StartingProfile_BalanceAudit_Invariant_92() {
public void Test_093_StartingProfile_BalanceAudit_Invariant_93() {
public void Test_094_StartingProfile_BalanceAudit_Invariant_94() {
public void Test_095_StartingProfile_BalanceAudit_Invariant_95() {
public void Test_096_StartingProfile_BalanceAudit_Invariant_96() {
public void Test_097_StartingProfile_BalanceAudit_Invariant_97() {
public void Test_098_StartingProfile_BalanceAudit_Invariant_98() {
public void Test_099_StartingProfile_BalanceAudit_Invariant_99() {
public void Test_100_StartingProfile_BalanceAudit_Invariant_100() {
```


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| prices, demand, indices, shocks and ledger | MarketSystem | routes, stock, travel and trade completion | TravelingCaravanSystem | Owner emits/reads a typed fact; no mirror state. |
| prices, demand, indices, shocks and ledger | MarketSystem | trap placement, catches, events and state | WildlifeTrappingSystem | Owner emits/reads a typed fact; no mirror state. |
| prices, demand, indices, shocks and ledger | MarketSystem | weather kind/effects and forecasts | WeatherSystem | Owner emits/reads a typed fact; no mirror state. |
| prices, demand, indices, shocks and ledger | MarketSystem | safety and contamination consequences | Food/disease owners | Owner emits/reads a typed fact; no mirror state. |
| routes, stock, travel and trade completion | TravelingCaravanSystem | prices, demand, indices, shocks and ledger | MarketSystem | Owner emits/reads a typed fact; no mirror state. |
| routes, stock, travel and trade completion | TravelingCaravanSystem | trap placement, catches, events and state | WildlifeTrappingSystem | Owner emits/reads a typed fact; no mirror state. |
| routes, stock, travel and trade completion | TravelingCaravanSystem | weather kind/effects and forecasts | WeatherSystem | Owner emits/reads a typed fact; no mirror state. |
| routes, stock, travel and trade completion | TravelingCaravanSystem | safety and contamination consequences | Food/disease owners | Owner emits/reads a typed fact; no mirror state. |
| trap placement, catches, events and state | WildlifeTrappingSystem | prices, demand, indices, shocks and ledger | MarketSystem | Owner emits/reads a typed fact; no mirror state. |
| trap placement, catches, events and state | WildlifeTrappingSystem | routes, stock, travel and trade completion | TravelingCaravanSystem | Owner emits/reads a typed fact; no mirror state. |
| trap placement, catches, events and state | WildlifeTrappingSystem | weather kind/effects and forecasts | WeatherSystem | Owner emits/reads a typed fact; no mirror state. |
| trap placement, catches, events and state | WildlifeTrappingSystem | safety and contamination consequences | Food/disease owners | Owner emits/reads a typed fact; no mirror state. |
| weather kind/effects and forecasts | WeatherSystem | prices, demand, indices, shocks and ledger | MarketSystem | Owner emits/reads a typed fact; no mirror state. |
| weather kind/effects and forecasts | WeatherSystem | routes, stock, travel and trade completion | TravelingCaravanSystem | Owner emits/reads a typed fact; no mirror state. |
| weather kind/effects and forecasts | WeatherSystem | trap placement, catches, events and state | WildlifeTrappingSystem | Owner emits/reads a typed fact; no mirror state. |
| weather kind/effects and forecasts | WeatherSystem | safety and contamination consequences | Food/disease owners | Owner emits/reads a typed fact; no mirror state. |
| safety and contamination consequences | Food/disease owners | prices, demand, indices, shocks and ledger | MarketSystem | Owner emits/reads a typed fact; no mirror state. |
| safety and contamination consequences | Food/disease owners | routes, stock, travel and trade completion | TravelingCaravanSystem | Owner emits/reads a typed fact; no mirror state. |
| safety and contamination consequences | Food/disease owners | trap placement, catches, events and state | WildlifeTrappingSystem | Owner emits/reads a typed fact; no mirror state. |
| safety and contamination consequences | Food/disease owners | weather kind/effects and forecasts | WeatherSystem | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Audit that crafting outputs and regional supply reach current market/caravan surfaces. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Create read-only shortage/surplus projections; do not duplicate price mutation. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Verify trapping choices route through inventory, food safety, disease and equipment condition exactly once. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Map weather effects to existing power, shelter, medical and economy owners with bounded tests. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.562 — Additional Current Architecture Evidence: `src/Host/WildlifeTrappingSaveStore.cs`

### `src/Host/WildlifeTrappingSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 56 lines / 2823 bytes.
- SHA-256: `e7466c5984ace8f5eda69dc4f37361fdef786ec1a425251bc51342ca416b6518`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class WildlifeTrappingSaveStore
public const string FileName = "wildlife_trapping_save.json";
public const string SectionName = "wildlife_trapping";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static bool TrySave(WildlifeTrappingState state) => s_store.TrySave(state);
public static WildlifeTrappingState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(WildlifeTrappingState state) => s_store.CapturePersisted(state);
public static string TryCaptureDirect(WildlifeTrappingState state) => s_store.CaptureBare(state);
public static WildlifeTrappingState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(WildlifeTrappingState state) => s_store.CaptureBare(state);
public static WildlifeTrappingState? TryRestore(string json) => s_store.RestoreBare(json);
```


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Shelter/FoodPreservationCatalog.cs`

### `Assets/Ashfall.Core/Shelter/FoodPreservationCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 139 lines / 5631 bytes.
- SHA-256: `a9182b9e4d5c8a472abf2ae335c0353d94d53c4f8c1cfea2e8275caa296622da`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PreservationTierDef
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public int shelf_life_days { get; set; } = 5;
public float shelf_life_multiplier { get; set; } = 1.0f;
public int power_draw_watts { get; set; } = 0;
public int flavor_morale_bonus { get; set; } = 0;
public float spoilage_toxin_risk { get; set; } = 0.1f;
public List<string> allowed_food_types { get; set; } = new List<string>();
public sealed class CuringRecipeDef
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string target_tier { get; set; } = string.Empty;
public string input_item_id { get; set; } = string.Empty;
public int input_quantity { get; set; } = 1;
public string preservative_item_id { get; set; } = string.Empty;
public int preservative_quantity { get; set; } = 1;
public string output_item_id { get; set; } = string.Empty;
public int output_quantity { get; set; } = 1;
public int work_ticks_required { get; set; } = 4;
public sealed class FoodPreservationCatalog
public int schema_version { get; set; } = 1;
public List<PreservationTierDef> preservation_tiers { get; set; } = new List<PreservationTierDef>();
public List<CuringRecipeDef> curing_recipes { get; set; } = new List<CuringRecipeDef>();
public Dictionary<string, string> food_type_by_item_id { get; set; } = new Dictionary<string, string>(StringComparer.Ordinal);
public void Index() {
public PreservationTierDef? GetTier(string tierId) {
public CuringRecipeDef? GetRecipe(string recipeId) {
public string ResolveFoodType(string foodItemId) {
public bool IsFoodTypeAllowed(PreservationTierDef? tier, string foodType) {
public static class FoodPreservationCatalogLoader
public static FoodPreservationCatalog Load(string dataDir, IFileIO fileIo) {
```


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/WildlifeTrappingEvents.cs`

### `Assets/Ashfall.Core/WildlifeTrappingEvents.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 163 lines / 6491 bytes.
- SHA-256: `ff6c6808d26d4dc0699047da3c849e54269a72706bac6f6c83c9fddcc33bd8ac`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TrapLifecycleEvent
public string siteId = string.Empty;
public string trapId = string.Empty;
public string trapType = string.Empty;
public bool isBroken;
public int day;
public sealed class ButcheryCompletedEvent
public string actionId = string.Empty;
public string siteId = string.Empty;
public string butcherId = string.Empty;
public string primarySpeciesId = string.Empty;
public string bycatchSpeciesId = string.Empty;
public float primaryYield;
public float bycatchYield;
public float totalYield;
public bool isToxic;
public bool bycatchToxic;
public int setDay;
public sealed class BycatchOccurredEvent
public string siteId = string.Empty;
public string trapId = string.Empty;
public string primarySpeciesId = string.Empty;
public string bycatchSpeciesId = string.Empty;
public float bycatchYield;
public bool bycatchToxic;
public int day;
public string hunterId = string.Empty;
public static class WildlifeTrappingEventStatus
public const string Pending = "pending";
public const string Delivered = "delivered";
public static class WildlifeTrappingEventKinds
public const string MoralConsequence = "moral_consequence";
public const string TrapEncounter = "trap_encounter";
public const string TrappingBroadcast = "trapping_broadcast";
public const string NarrativeIncident = "narrative_incident";
public static class TrapEncounterIds
public const string BaitStolen = "enc_trap_bait_stolen";
public const string Tampered = "enc_trap_tampered";
public const string StrangerDiscovery = "enc_trap_stranger_discovery";
public static class TrapNarrativeIncidentIds
public const string SprungBloodTrail = "trap_sprung_blood_trail";
public const string BaitStolen = "trap_bait_stolen";
public const string HumanBootprints = "trap_human_bootprints";
public static readonly string[] Ordered = {
public static class TrappingBroadcastIds
public const string FirstCatch = "radio_wildlife_first_catch";
public const string TrapBroken = "radio_wildlife_trap_broken";
public const string RareBycatch = "radio_wildlife_rare_bycatch";
public static class TrappingMoralTier
public const string HighQuestId = "quest_moral_trap_prey_high";
public const string MediumQuestId = "quest_moral_trap_prey_medium";
public const string LowQuestId = "quest_moral_trap_prey_low";
public const float HighThreshold = 0.75f;
public const float MediumThreshold = 0.35f;
public static string ResolveQuestId(float moralWeight) {
public sealed class WildlifeTrappingPendingEvent
public string eventId = string.Empty;
public int sequence;
public string kind = string.Empty; // WildlifeTrappingEventKinds
public string sourceTrapSiteId = string.Empty;
public string survivorId = string.Empty;
public string speciesId = string.Empty;
public string payloadId = string.Empty; // quest/encounter/broadcast content ID
public float weight;
public int day;
public string status = WildlifeTrappingEventStatus.Pending;
```


# Appendix Q.566 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Economy/BlackMarketSystem.cs`

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


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/TravelingCaravanHeadlessDemo.cs`

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


# Appendix Q.568 — Additional Current Architecture Evidence: `src/Main.ExpandedShelterSystems.cs`

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


# Appendix Q.569 — Additional Current Architecture Evidence: `src/Host/FoodPreservationSaveStore.cs`

### `src/Host/FoodPreservationSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 26 lines / 1165 bytes.
- SHA-256: `6e85136a9fcd89ec09828b94dccfe3bd3ae88979882907a84dd62e2e849e0c22`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class FoodPreservationSaveStore
public const string FileName = "food_preservation_save.json";
public const string SectionName = "food_preservation";
public static string TryCapturePersisted(FoodPreservationState state) => s_store.CapturePersisted(state);
public static FoodPreservationState? TryLoad() => s_store.TryLoad();
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/ExpansionHubSave.cs`

### `Assets/Ashfall.Core/ExpansionHubSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 507 lines / 28559 bytes.
- SHA-256: `6f9efccbfed1c5101889fb1aeff9b33800d110d217b1f669924bbbfdf2d1e282`.
- Architecture signals: seeded references=0; save/restore symbols=31; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class ExpansionHubSave
public const int CurrentSaveVersion = 6;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public DebtDispatcherState debtDispatcher = new DebtDispatcherState();
public FactionEmbargoLedgerState embargoes = new FactionEmbargoLedgerState();
public DebtConsequenceBridgeState debtBridge = new DebtConsequenceBridgeState();
public SaltMineState saltMine = new SaltMineState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV5
public int saveVersion = 5;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public DebtDispatcherState debtDispatcher = new DebtDispatcherState();
public FactionEmbargoLedgerState embargoes = new FactionEmbargoLedgerState();
public DebtConsequenceBridgeState debtBridge = new DebtConsequenceBridgeState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV1
public int saveVersion = 1;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV2
public int saveVersion = 2;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV3
public int saveVersion = 3;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV4
public int saveVersion = 4;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public string Checksum = string.Empty;
public static class ExpansionHubSaveCodec
public static ExpansionHubSave Capture( int simDay, WaystationSystem waystation, LocationLayoutSystem layouts, LocationMemorySystem memory, SiteEncounterSystem siteEncounters,
public static string Encode(ExpansionHubSave save, IJsonSerializer json) {
public static ExpansionHubSave Decode(string jsonText, IJsonSerializer json) {
public static void Restore( ExpansionHubSave save, WaystationSystem waystation, LocationLayoutSystem layouts, LocationMemorySystem memory, SiteEncounterSystem siteEncounters,
```


# Appendix Q.571 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/WeatherSondeSystem.cs`

### `Assets/Ashfall.Core/World/WeatherSondeSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 691 lines / 31183 bytes.
- SHA-256: `9e662202eb4e737868a9c0b00fda05309187b2987ee2e37453478e3a4a00f274`.
- Architecture signals: seeded references=6; save/restore symbols=2; typed event declarations=17; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class SondeTelemetrySample
public int sampleIndex = 0;
public float altitudeKm = 0f;
public float temperatureC = 0f;
public float radiationMsv = 0f;
public float windSpeedKmh = 0f;
public float windDirectionDeg = 0f;
public float humidityPct = 0f;
public bool isLost = false;       // signal lost at this altitude
public class SondeForecastEntry
public int dayOffset = 0;         // 0 = today, 1 = tomorrow, etc.
public string predictedKind = "Clear";
public float confidence = 0f;     // 0..1
public float uncertaintyRadius = 0f; // weather variability
public string hazardTag = "none"; // Plan 71 §6.9: "ashfall" / "fallout_storm" / "none" …
public class WeatherSondeState
public string systemId = WeatherSondeSystem.SystemId;
public string sondeId = string.Empty;
public bool isLaunched = false;
public bool isRecovered = false;
public bool isFailed = false;
public int launchDay = 0;
public float launchHour = 0f;
public int flightDurationTicks = 0;
public int ticksElapsed = 0;
public float batteryLevel = 1.0f;   // 0..1
public float hydrogenLevel = 1.0f;  // 0..1, inflation gas
public float sensorQuality = 1.0f;  // 0..1, degrades with altitude
public float observationQuality = 0f; // 0..1, cumulative quality
public List<SondeTelemetrySample> samples = new List<SondeTelemetrySample>();
public List<SondeForecastEntry> forecast = new List<SondeForecastEntry>();
public string failureReason = string.Empty;
public float currentAltitudeKm = 0f;      // live altitude (ascent or descent)
public bool isBurst = false;              // envelope burst — descent phase
public float burstAltitudeKm = 0f;
public int positionEastingM = 0;          // quantized world meters (Trap H)
public int positionNorthingM = 0;
public float driftEastKm = 0f;            // unquantized drift accumulators (persisted
public float driftNorthKm = 0f;           //  for exact split-run continuation)
public int landingDay = -1;
public int landingEastingM = 0;
public int landingNorthingM = 0;
public float payloadCondition = 0f;       // 0..1 sensor/package state at landing
public bool recoveryTargetSpawned = false;
public int recoveryExpiryDay = -1;
public bool recoveryClaimed = false;
public string payloadId = string.Empty;
public class WeatherSondeSystem
public const string SystemId = "weather_sonde_system";
public const int DefaultFlightDurationTicks = 4;
public const int MaxFlightDurationTicks = 8;
public const float BatteryDrainPerTick = 0.06f;           // ascent drain
public const float BatteryDrainDescentPerTick = 0.03f;      // parachute descent drain
public const float HydrogenDrainPerTick = 0.05f;
public const float SensorDegradationPerKm = 0.02f;
public const float MaxAltitudeKm = 30f;
public const float AltitudePerTickKm = 7.5f;
public const int BaseForecastHorizonDays = 3;
public const int ExtendedForecastHorizonDays = 5;
public const float HighQualityThreshold = 0.7f;
public const float SignalLossChancePerTick = 0.05f;
public const float HoursPerFlightTick = 1f;
public const float MetersPerKm = 1000f;
public const int MaxTelemetrySamples = 64;         // bounded history (§6.16)
public const int RecoveryTargetExpiryDays = 14;    // authored expiration (§6.11)
public const float BurstPayloadDamage = 0.6f;      // payload condition cap after burst
public const float HydrogenCostPerLaunch = 0.5f;
public const float BatteryCostPerLaunch = 0.3f;
public event Action<string> OnLaunchStarted;           // sondeId
public event Action<SondeTelemetrySample> OnTelemetryReceived;
public event Action<SondeTelemetrySample> OnTelemetryLost;
public event Action<string> OnSondeFailed;             // reason
public event Action<string> OnSondeRecovered;          // sondeId
public event Action OnPayloadLanded;                   // Plan 71 §6.11
public event Action<List<SondeForecastEntry>> OnForecastConfidenceChanged;
public event Action<WeatherSondeState> OnStateChanged;
public WeatherSondeState State => _state;
public bool IsLaunched => _state.isLaunched;
public bool IsComplete => _state.isRecovered || _state.isFailed;
public void ApplySoundingCatalog( IReadOnlyList<SoundingAltitudeBandDef>? bands, IReadOnlyList<SoundingPayloadDef>? payloads) {
public void BindRecoveryInventory(Ashfall.Core.Inventory.Inventory? inventory) {
public bool Launch(string sondeId, int day, float hour, float hydrogenAvailable, float batteryAvailable) {
public bool Tick(ISeededRng rng, int day = -1) {
public ActionResult ClaimRecoveryPayload(int currentDay) {
public int GetSampleCount() => _state.samples.Count;
public int GetLostSampleCount() {
public float GetCurrentAltitude() {
public List<SondeForecastEntry> GetForecast() => new List<SondeForecastEntry>(_state.forecast);
public WeatherSondeState CaptureState() {
public void RestoreState(WeatherSondeState saved) {
```


# Appendix Q.572 — Additional Current Architecture Evidence: `src/Host/PanelBindLifecycleSelfTest.cs`

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


# Appendix Q.573 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Disease/DiseaseHeadlessDemo.cs`

### `Assets/Ashfall.Core/Disease/DiseaseHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 489 lines / 28179 bytes.
- SHA-256: `d985f4110e9c912b9818316dad3b3cf219b2d9b5703fed11e1e5531a51a47b1b`.
- Architecture signals: seeded references=8; save/restore symbols=11; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DiseaseHeadlessDemo
public const int DemoSeed = 1013;
public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null) {
public string id = string.Empty;
public int schema_version;
public List<DiseaseDemoItemRow> items = new List<DiseaseDemoItemRow>();
```


# Appendix Q.574 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Localization/WildlifeTrappingLocalization.cs`

### `Assets/Ashfall.Core/Localization/WildlifeTrappingLocalization.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 33 lines / 1701 bytes.
- SHA-256: `1b9c2b80e02d7bc2200b46d87985be131d521765686231462c2642687c1bed7d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class WildlifeTrappingLocalization
public const string FirstSnareTutorialId = "wildlife.trapping.first_snare";
public const string WearOutTutorialId = "wildlife.trapping.wear_out";
public const string BycatchTutorialId = "wildlife.trapping.bycatch";
public static string TrapNameKey(string trapId) => Key("trap", trapId, "name");
public static string TrapDescriptionKey(string trapId) => Key("trap", trapId, "description");
public static string PreyNameKey(string speciesId) => Key("prey", speciesId, "name");
public static string PreyDescriptionKey(string speciesId) => Key("prey", speciesId, "description");
public static string BaitNameKey(string baitId) => Key("bait", baitId, "name");
public static string TutorialTitleKey(string tutorialId) => $"{tutorialId}.title";
public static string TutorialBodyKey(string tutorialId) => $"{tutorialId}.body";
```


# Appendix Q.575 — Additional Current Architecture Evidence: `src/Host/HostCli.WorldPlaytest.cs`

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


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.
