# Economy Core Audit — Stage 0
**Date:** 2026-08-15
**Scope:** `Assets/Ashfall.Core/Economy/` (MarketSystem, GoodsCatalog, EconomyHeadlessDemo) + `Ashfall.Core.Tests/EconomySystemTests.cs`
**Baseline:** Build 0/0, 558 tests, --economy-selftest 11/11, expansions 236/236

---

## Executive Summary

The ported economy core is structurally sound: determinism is preserved via caller-owned `ISeededRng`, save/load uses deep-copy snapshots with version guards, and validation is strict but non-throwing. Three **HIGH** findings and one **MEDIUM** finding require fixes before further porting work; several **MEDIUM/LOW** items are logged for the Stage 6 debug loop.

---

## Audit Layers

### 1. Correctness

| # | Severity | File:Line | Finding | Evidence | Status |
|---|---|---|---|---|---|
| C1 | HIGH | MarketSystem.cs:140-145 | `GetPrice` returns `0f` for missing goods | Silent failure indistinguishable from a valid zero-price good; downstream callers may treat 0 as "free" | **FIXED** |
| C2 | MEDIUM | MarketSystem.cs:106-112 vs 124-131 | `GetDemandMultiplier` double-clamps on read; `IsSuppliesShort` sums raw stored values | If any future path writes an unclamped value, the two methods disagree on whether supplies are short | Deferred to debug loop |
| C3 | MEDIUM | MarketSystem.cs:193 | `Barter` assumes `takePrice > 0` | Currently safe because `basePrice > 0` is validated and floor clamp > 0, but no explicit guard | Deferred to debug loop |
| C4 | LOW | MarketSystem.cs:178 | `Barter` give-leg books `unitPrice = exchangedValue / giveQuantity` | This is a derived blended price, not the live market price; could confuse ledger readers | Deferred to debug loop |

### 2. Determinism

| # | Severity | File:Line | Finding | Evidence | Status |
|---|---|---|---|---|---|
| D1 | HIGH | HostDefaults.cs:95-102 | `SeededRng` wraps `System.Random` | `System.Random` algorithm is not guaranteed identical across .NET runtimes/versions; the IceRoad code already works around this with salt modulo, proving the team knows `Random` is not cross-runtime-safe | **FIXED** |
| D2 | LOW | MarketSystem.cs:92 | `TickDay` iterates `_catalog.All()` which returns sorted list | Safe today, but if catalog binding changes to an unsorted source, tick order becomes non-deterministic | Deferred to debug loop |

### 3. Save/Load

| # | Severity | File:Line | Finding | Evidence | Status |
|---|---|---|---|---|---|
| S1 | MEDIUM | MarketState.cs:52-53 | `MarketState.demand` and `ledger` are public mutable `List<>` fields | Callers can bypass `MarketSystem` invariants by mutating the state object directly | Deferred to debug loop |
| S2 | LOW | — | `MarketState` lacks a `Checksum` field unlike `DutyRosterSave`, `HoldfastSave` | Host-side save stores must add their own checksum envelope; not a Core bug, but inconsistent with the project's save pattern | Deferred to debug loop |

### 4. API Surface

| # | Severity | File:Line | Finding | Evidence | Status |
|---|---|---|---|---|---|
| A1 | MEDIUM | MarketSystem.cs:52-53 | Public mutable collection fields on `MarketState` | Exposes internal state by reference; violates encapsulation | Deferred to debug loop |
| A2 | LOW | MarketSystem.cs:99 | `State => _state` exposes the live mutable state object | Combined with A1, allows invariant bypass | Deferred to debug loop |

### 5. Error Handling

| # | Severity | File:Line | Finding | Evidence | Status |
|---|---|---|---|---|---|
| E1 | HIGH | MarketSystem.cs:143 | `GetPrice` returns `0f` for `good == null` instead of throwing or returning a sentinel | See C1; silent failure is also an error-handling issue | **FIXED** (with C1) |
| E2 | LOW | MarketSystem.cs:167 | `Transact` ignores `day <= 0` | Not validated; could book ledger entries with day=0 or negative | Deferred to debug loop |

### 6. Test Quality

| # | Severity | File:Line | Finding | Evidence | Status |
|---|---|---|---|---|---|
| T1 | MEDIUM | EconomySystemTests.cs | No ledger conservation test | No test verifies that total ledger value sums to zero across buy/sell/barter | **FIXED** |
| T2 | LOW | EconomySystemTests.cs | No test for `GetPrice` with missing good | Mutation: flip null-check to always-return-0 would not be caught | Deferred to debug loop |
| T3 | LOW | EconomySystemTests.cs | No cross-process determinism test | Same-seed test runs in one process; doesn't verify across separate runs | Deferred to debug loop |

---

## Fixes Applied (Critical/High Only)

### Fix 1: `GetPrice` missing-good sentinel
- Changed return from `0f` to `float.NaN` for missing goods
- Added XML doc explaining the sentinel
- Barter already guards against null goods, so this is safe

### Fix 2: `SeededRng` determinism
- Replaced `System.Random` with a custom xoshiro256** algorithm
- Same seed produces identical sequence across all .NET runtimes
- Maintains the `ISeededRng` interface contract

### Fix 3: Ledger conservation test
- Added `Ledger_Conservation_AllTransactionTypes` test
- Verifies total signed ledger value is 0 across buy, sell, and barter

---

## Dependency/Capability Map: DynamicEconomySystem

```
DynamicEconomySystem (1816 LOC, Unity-coupled)
├── Trust math (pure calculation)
│   ├── GetEffectiveTrust
│   ├── ModifyTrust / SetTrust
│   └── Trust-inversion (Cult of the Glow)
├── Faction standing (Unity-coupled via FactionSO)
│   ├── RegisterFaction
│   ├── WillTrade / WillShareIntel
│   ├── GetRaidAggression / SetRaidAggression
│   ├── GetStance → TradeStance enum
│   └── Succession / surrender / parley logic
├── Price modifiers (partially portable)
│   ├── GetTradeValue (item + faction + survivor)
│   ├── GetBarterUnitValue
│   ├── EvaluateOffer / IsFairTrade / TryExecuteTrade
│   └── Barter-only mode
├── Demand pressure (partially ported as MarketSystem)
│   ├── GetDemandMultiplier
│   ├── AdjustDemand
│   └── IsSuppliesShort
├── Quest hooks (Unity-coupled via PersonalQuestSystem)
│   └── GetQuestTradeMultiplier
├── Shelter hooks (Unity-coupled via Shelter)
│   └── ScarcityOverride
├── Defense hooks (Unity-coupled via HatchDefenseSystem)
│   └── Consecutive repels / auto-surrender
└── Event hooks (Unity-coupled via EventRunner)
    └── NotifyPhaseChanged
```

**Pure economics (portable now):** Trust math, price modifier formulas, demand pressure, barter-only mode, scarcity override data.

**Engine-coupled glue:** FactionSO, PersonalQuestSystem, Shelter, HatchDefenseSystem, EventRunner.

**Port strategy:** Extract pure economics into `MarketSystem` extensions or a new `FactionEconomy` class behind port interfaces. Leave glue in a thin `DynamicEconomyAdapter` that implements the ports.

---

## Next Steps

1. Commit this audit + fixes
2. Stage 1: Trivial ports (TradeStance, BiologicalTradeItem, HardcoreEconomyTuning)
3. Stage 2: Strangler-fig extraction of DynamicEconomySystem
