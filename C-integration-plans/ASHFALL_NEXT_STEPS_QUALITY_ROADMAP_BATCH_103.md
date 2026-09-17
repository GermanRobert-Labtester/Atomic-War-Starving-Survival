# ASHFALL Quality Roadmap — Batch 103

## Theme: Rate Limiting & Cooldown System — Prevent Exploitation of Repeated Actions

**Priority:** MEDIUM
**Risk:** Low — additive constraint layer, no existing behavior mutated
**Estimated Scope:** ~900 lines of Core code + ~350 lines of tests
**Dependencies:** `IClock` (day counter), `ISeededRng` (deterministic jitter), `CaptureState/RestoreState` pattern, JSON data authority

---

## Motivation

Many game actions can be repeated without limit today. No cooldown or rate-limiting infrastructure exists anywhere in `Ashfall.Core`. Each system that needs throttling would have to implement its own ad-hoc timer, leading to inconsistent behavior, duplicated code, and untestable edge cases.

### Exploitable patterns (current state):

| Action | Exploit | Consequence |
|---|---|---|
| Trading same goods back and forth | Price arbitrage — buy low, sell high, repeat same day | Infinite credits |
| Sending expeditions every day | No recovery time — survivors never rest | Trivializes resource gathering |
| Crafting same item endlessly | If resources allow, no throughput limit | Stockpile bypass |
| Consuming medical items rapidly | Anti-rad every tick instead of daily | Negates radiation threat |
| Reassigning duty roster every tick | Min-max assignments without cost | Removes meaningful scheduling |
| Repeated faction negotiations | Spam diplomacy attempts | Trivializes alliance building |

### Design goals:

1. **Centralized** — one system manages all cooldowns; individual systems query it.
2. **Data-driven** — cooldown durations defined in JSON (data authority), not hardcoded.
3. **Scoped** — supports per-survivor cooldowns (recovery), per-global cooldowns (market reset), and per-pair cooldowns (trade with specific faction).
4. **Deterministic** — jitter uses `ISeededRng`, not `System.Random`.
5. **Saveable** — full `CaptureState/RestoreState` with versioned codec.
6. **Queryable** — systems can check "is this action available?" and "how many days until available?"

---

## Step 1 — Design `ICooldownService` Interface in Core

### Goal
Define the port (interface) that all systems use to check and register cooldowns. This establishes the API contract before any implementation.

### Implementation

**File:** `Assets/Ashfall.Core/Cooldowns/ICooldownService.cs`

```csharp
namespace Ashfall.Core.Cooldowns
{
    /// <summary>
    /// Central service for tracking action cooldowns.
    /// Thread-safe within a single simulation tick.
    /// </summary>
    public interface ICooldownService
    {
        /// <summary>
        /// Register that an action was performed. Starts the cooldown timer.
        /// </summary>
        void RecordAction(CooldownKey key);

        /// <summary>
        /// Check if an action is currently on cooldown.
        /// </summary>
        bool IsOnCooldown(CooldownKey key);

        /// <summary>
        /// Days remaining until the action is available. 0 = available now.
        /// </summary>
        int DaysRemaining(CooldownKey key);

        /// <summary>
        /// The day the action will next be available. Returns current day if available now.
        /// </summary>
        int AvailableOnDay(CooldownKey key);

        /// <summary>
        /// Force-clear a cooldown (admin/debug, or narrative event override).
        /// </summary>
        void ClearCooldown(CooldownKey key);

        /// <summary>
        /// Advance all cooldowns by one day. Called during TickSimDay.
        /// </summary>
        void AdvanceDay(int currentDay);
    }

    /// <summary>
    /// Composite key identifying a specific cooldown instance.
    /// </summary>
    public readonly struct CooldownKey : IEquatable<CooldownKey>
    {
        public string ActionId { get; }        // e.g. "action_trade", "action_expedition"
        public string Scope { get; }           // e.g. "global", survivor_id, faction_id
        public string Target { get; }          // optional: e.g. trade partner id

        public CooldownKey(string actionId, string scope, string target = null);
    }

    public enum CooldownScope
    {
        Global,         // one cooldown shared by all (e.g., market reset)
        PerSurvivor,    // each survivor has independent cooldown
        PerPair         // specific pair (e.g., trade between survivor X and faction Y)
    }
}
```

**Design decisions:**
- `CooldownKey` is a value type (struct) for allocation-free lookups in hot paths.
- Three-part key (`ActionId` + `Scope` + `Target`) covers all scoping patterns without polymorphism.
- `AdvanceDay` is explicit (not automatic) — the simulation controls when time passes.
- `ClearCooldown` exists for narrative overrides ("a messenger arrives, trade embargo lifted").

### Verification
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — compiles.
- No engine references in new file.
- Interface is minimal and testable.

### Done when
- `ICooldownService`, `CooldownKey`, and `CooldownScope` compile in `Ashfall.Core/Cooldowns/`.
- No implementation yet — just the contract.

---

## Step 2 — Implement `CooldownRegistry` with Save/Load

### Goal
Build the concrete implementation of `ICooldownService` that stores active cooldowns, respects the `IClock` day counter, applies `ISeededRng` jitter, and implements `CaptureState/RestoreState`.

### Implementation

**File:** `Assets/Ashfall.Core/Cooldowns/CooldownRegistry.cs`

```csharp
namespace Ashfall.Core.Cooldowns
{
    public sealed class CooldownRegistry : ICooldownService
    {
        private readonly IClock _clock;
        private readonly ISeededRng _rng;
        private readonly CooldownCatalog _catalog;
        private readonly Dictionary<CooldownKey, CooldownEntry> _active;

        public CooldownRegistry(IClock clock, ISeededRng rng, CooldownCatalog catalog);

        // ICooldownService implementation
        public void RecordAction(CooldownKey key);
        public bool IsOnCooldown(CooldownKey key);
        public int DaysRemaining(CooldownKey key);
        public int AvailableOnDay(CooldownKey key);
        public void ClearCooldown(CooldownKey key);
        public void AdvanceDay(int currentDay);

        // Save/Load (Invariant: CaptureState/RestoreState)
        public CooldownRegistryState CaptureState();
        public void RestoreState(CooldownRegistryState state);
    }

    [Serializable]
    public sealed class CooldownEntry
    {
        public int StartedOnDay { get; set; }
        public int ExpiresOnDay { get; set; }   // StartedOnDay + duration + jitter
        public string ActionId { get; set; }
    }

    [Serializable]
    public sealed class CooldownRegistryState
    {
        public List<CooldownEntryDto> ActiveCooldowns { get; set; } = new();
    }

    [Serializable]
    public sealed class CooldownEntryDto
    {
        public string ActionId { get; set; }
        public string Scope { get; set; }
        public string Target { get; set; }
        public int StartedOnDay { get; set; }
        public int ExpiresOnDay { get; set; }
    }
}
```

**Jitter logic:**
```csharp
// Deterministic jitter: +-1 day for cooldowns >= 3 days
int jitter = 0;
if (baseDuration >= 3)
{
    jitter = _rng.Next(-1, 2); // -1, 0, or +1
}
int finalDuration = Math.Max(1, baseDuration + jitter);
```

**Expiration cleanup:**
- `AdvanceDay` removes entries where `currentDay >= ExpiresOnDay`.
- Lazy cleanup: expired entries are also pruned on any `IsOnCooldown` / `DaysRemaining` query.

**Save/Load:**
- `CaptureState` serializes all active (non-expired) entries.
- `RestoreState` rebuilds the dictionary from DTOs.
- Versioned: `CooldownRegistryState` gets a `Version` field (starts at 1).

### Verification
- Unit test: `RecordAction` → `IsOnCooldown` returns true.
- Unit test: advance past expiry → `IsOnCooldown` returns false.
- Unit test: `CaptureState` → `RestoreState` → state is identical.
- `dotnet test` passes.

### Done when
- `CooldownRegistry` fully implements `ICooldownService`.
- Save/load round-trip preserves all active cooldowns.
- Jitter is deterministic (same seed → same jitter).
- No engine coupling.

---

## Step 3 — Define Cooldown Catalog in JSON Data Authority

### Goal
Define all game cooldowns in a JSON file under `Assets/StreamingAssets/Data/` (the single source of truth). Systems reference cooldowns by `action_id`, never by hardcoded magic numbers.

### Implementation

**File:** `Assets/StreamingAssets/Data/cooldowns.json`

```json
{
    "schema_version": 1,
    "cooldowns": [
        {
            "action_id": "action_trade_same_partner",
            "display_name": "Trade (same partner)",
            "description": "Cannot trade with the same faction again for several days.",
            "scope": "per_pair",
            "base_duration_days": 3,
            "jitter_enabled": true,
            "min_duration_days": 2,
            "max_duration_days": 4
        },
        {
            "action_id": "action_expedition_recovery",
            "display_name": "Expedition Recovery",
            "description": "Survivor must rest after returning from an expedition.",
            "scope": "per_survivor",
            "base_duration_days": 2,
            "jitter_enabled": false,
            "min_duration_days": 2,
            "max_duration_days": 2
        },
        {
            "action_id": "action_anti_rad_dose",
            "display_name": "Anti-Radiation Medication",
            "description": "Body needs time to process anti-radiation treatment.",
            "scope": "per_survivor",
            "base_duration_days": 1,
            "jitter_enabled": false,
            "min_duration_days": 1,
            "max_duration_days": 1
        },
        {
            "action_id": "action_duty_reassign",
            "display_name": "Duty Roster Reassignment",
            "description": "Workers need time to adjust to new assignments.",
            "scope": "global",
            "base_duration_days": 1,
            "jitter_enabled": false,
            "min_duration_days": 1,
            "max_duration_days": 1
        },
        {
            "action_id": "action_faction_negotiate",
            "display_name": "Faction Negotiation",
            "description": "Diplomacy requires patience between attempts.",
            "scope": "per_pair",
            "base_duration_days": 5,
            "jitter_enabled": true,
            "min_duration_days": 4,
            "max_duration_days": 7
        },
        {
            "action_id": "action_craft_batch",
            "display_name": "Crafting Batch",
            "description": "Workshop needs time to reset between production runs.",
            "scope": "global",
            "base_duration_days": 1,
            "jitter_enabled": false,
            "min_duration_days": 1,
            "max_duration_days": 1
        }
    ]
}
```

**File:** `Assets/Ashfall.Core/Cooldowns/CooldownCatalog.cs`

```csharp
namespace Ashfall.Core.Cooldowns
{
    public sealed class CooldownCatalog
    {
        private readonly Dictionary<string, CooldownDef> _defs;

        public static CooldownCatalog Load(IFileIO fileIO, IJsonSerializer json);
        public CooldownDef GetDef(string actionId);
        public bool HasDef(string actionId);
        public IReadOnlyCollection<CooldownDef> All { get; }
    }

    [Serializable]
    public sealed class CooldownDef
    {
        public string ActionId { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public CooldownScope Scope { get; set; }
        public int BaseDurationDays { get; set; }
        public bool JitterEnabled { get; set; }
        public int MinDurationDays { get; set; }
        public int MaxDurationDays { get; set; }
    }
}
```

**Catalog rules:**
- All `action_id` values use snake_case with `action_` prefix.
- `min_duration_days` / `max_duration_days` bound jitter (never go below min or above max).
- `schema_version` required (project convention).
- File registered in `CatalogIntegrityValidator` prefix list.

### Verification
- `godot --headless --path . -- --data-integrity-selftest` — new JSON passes validation.
- Unit test: load catalog from JSON string → all 6 defs parsed correctly.
- Confirm `action_` prefix was added to `CatalogIntegrityValidator.IdPrefixes` (see correction below).

**CORRECTED (was stated as a passive "verify" check, but this is a mandatory code change):**
`action_` is **not** currently in `CatalogIntegrityValidator.IdPrefixes`
(`Assets/Ashfall.Core/CatalogIntegrityValidator.cs`, ~180-entry array, confirmed absent by direct read). Every `cooldowns.json` entry uses `action_id` values with the `action_` prefix, so **this step must add the string `"action_"` to `IdPrefixes` as part of Step 3**, not merely confirm it's already there. Without this addition, `--data-integrity-selftest` will fail once `cooldowns.json` is registered (TIER-1 check: "strings with a known snake_case prefix must resolve").

### Done when
- `cooldowns.json` exists in data authority with `schema_version`.
- `CooldownCatalog` loads and indexes all definitions.
- `"action_"` has been added to `CatalogIntegrityValidator.IdPrefixes` (it does not exist there today).
- Data integrity selftest passes.

---

## Step 4 — Wire Trading Cooldown (Same Trade Partner: 3-Day Cooldown)

### Goal
Integrate the cooldown system into the existing trade flow. After completing a trade with a faction, the same faction cannot be traded with again for 3 days (with jitter).

### Implementation

**CORRECTED — integration point:** There is no `Economy/TradeSystem.cs` and no `Assets/Ashfall.Core/Economy/` folder in Core. The real, verified Core trading class is `Assets/Ashfall.Core/HoldfastTradeSession.cs` (293 lines), with `Buy(string itemId, int quantity, string factionId)` and `Sell(...)` methods. Neither method currently takes a day/`IClock` parameter — the cooldown check must be threaded through as a new dependency injected into `HoldfastTradeSession`'s constructor (or an explicit `day` parameter added to `Buy`/`Sell`), not bolted onto a nonexistent `TradeSystem`. Confirm the exact call surface by reading `HoldfastTradeSession.cs` before writing this step's code — do not assume method signatures.

**File:** Modify `Assets/Ashfall.Core/HoldfastTradeSession.cs` (verified real path — was previously misnamed).

```csharp
// Before executing a trade:
var key = new CooldownKey("action_trade_same_partner", tradingPartnerId, survivorId);
if (_cooldowns.IsOnCooldown(key))
{
    int remaining = _cooldowns.DaysRemaining(key);
    return TradeResult.OnCooldown(remaining);
}

// After successful trade:
_cooldowns.RecordAction(key);
```

**Trade result extension:**
```csharp
public sealed class TradeResult
{
    public bool Success { get; }
    public string FailureReason { get; }
    public int CooldownDaysRemaining { get; }

    public static TradeResult OnCooldown(int daysRemaining)
        => new() { Success = false, FailureReason = "trade_cooldown", CooldownDaysRemaining = daysRemaining };
}
```

**UI contract:**
- When trade is blocked by cooldown, UI shows: "Cannot trade with {faction} for {N} more days."
- Trade button is greyed out with tooltip showing remaining days.

### Verification
- Test: execute trade → immediately attempt same partner → blocked.
- Test: advance 3 days → same partner trade succeeds.
- Test: different partner → not blocked (cooldown is per-pair).
- Existing tests in the confirmed real file `Ashfall.Core.Tests/HoldfastTradeSessionTests.cs` (`dotnet test --filter HoldfastTradeSessionTests`) still pass after the signature change.

**CORRECTED — risk/rollback (previously missing):** Because `HoldfastTradeSession.Buy`/`Sell` currently have no day/time dimension at all, wiring in a cooldown check changes their call surface (new constructor dependency or new parameter). This is a breaking signature change, not purely additive — any existing caller (host wiring, other Core systems, or tests) constructing `HoldfastTradeSession` or calling `Buy`/`Sell` directly must be updated in the same commit. Rollback plan: keep the cooldown check behind a constructor-injected `ICooldownService` that defaults to a no-op/null-object implementation (always returns "not on cooldown") so existing callers that don't pass one keep working unmodified; this also lets the feature be disabled by injecting the no-op service without reverting code.

### Done when
- Trading with the same partner is blocked for the configured cooldown period.
- Different partners are unaffected.
- Cooldown state persists across save/load.
- All existing `HoldfastTradeSession` callers (found via `find_references`/grep on `Buy(` and `Sell(` before starting) are updated to compile against the new signature.

---

## Step 5 — Wire Expedition Cooldown (Survivor Recovery: 2 Days After Return)

### Goal
After a survivor returns from an expedition, they cannot be sent on another expedition for 2 days (no jitter — fixed recovery period).

### Implementation

**CORRECTED — current behavior confirmed by reading `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`:** `Start(...)` only blocks a *second concurrent* expedition for a survivor already in `_active` (`if (_active.ContainsKey(survivorId)) return false;`). The moment an expedition resolves (`Completed` or `Failed`), the handler removes the survivor from `_active` immediately, so today a survivor can be re-dispatched on the very next call with zero rest. This confirms the plan's motivating claim ("no recovery time") is accurate.

**CORRECTED — `Fail` is a private method, not a hookable integration point:** `Fail(ExpeditionState exp, string reason)` (line ~289) is `private`. It is called internally from `TickHours` on stamina collapse; nothing outside `ExpeditionSystem` can call or override it. The only externally observable signal for a failed expedition is the **public event** `OnExpeditionFailed` (fired inside `Fail` immediately before `_active.Remove`). Re-stated integration point:
- **Success path:** subscribe to the public event `OnExpeditionCompleted` (fires inside the `TickHours` loop when phase transitions to `Completed`, immediately before `_active.Remove(exp.survivorId)`).
- **Failure path:** subscribe to the public event `OnExpeditionFailed` (fires inside the private `Fail(...)` method, immediately before `_active.Remove(exp.survivorId)`).
- Both events pass the `ExpeditionState`, which has `survivorId` — record the cooldown for that id in both handlers (recovery should apply whether the expedition succeeded or the survivor collapsed).
- **Dispatch guard:** `Start(...)` is where the new `CanDispatch`-equivalent check must be added, before the existing `_active.ContainsKey(survivorId)` check — do not replace that check, cooldown and concurrent-dispatch are two independent guards.

**CORRECTED — code sample below was wrong: `ExpeditionState` has no `Participants` collection.** `ExpeditionState.survivorId` is a single string (one survivor per expedition instance — confirmed by direct read of `ExpeditionState` in `ExpeditionSystem.cs`). Corrected sample:

```csharp
// Subscribe once, in host wiring (or ExpeditionSystem's own constructor if the
// cooldown service is injected there):
expeditionSystem.OnExpeditionCompleted += exp =>
    _cooldowns.RecordAction(new CooldownKey("action_expedition_recovery", exp.survivorId));
expeditionSystem.OnExpeditionFailed += (exp, reason) =>
    _cooldowns.RecordAction(new CooldownKey("action_expedition_recovery", exp.survivorId));

// New query used by the host before calling Start(...) — does not change Start's signature:
public bool CanDispatch(string survivorId)
{
    var key = new CooldownKey("action_expedition_recovery", survivorId);
    return !_cooldowns.IsOnCooldown(key);
}
```

**Design notes:**
- Recovery period is per-survivor (different survivors can have different cooldown states).
- Cooldown starts on return, not on dispatch.
- If a survivor is injured, a separate medical cooldown may stack (future enhancement).
- Narrative justification: "exhaustion from the wasteland requires rest."

### Verification
- Test: survivor returns → cannot dispatch same survivor next day.
- Test: advance 2 days → survivor is available.
- Test: different survivor → not affected.
- Test: save/load during cooldown → cooldown state preserved.

**CORRECTED — risk/rollback (previously missing):** `OnExpeditionCompleted` and `OnExpeditionFailed` are pre-existing public events with real subscribers today (`src/Host/ExpeditionHostSession.cs:84-85`, confirmed by direct read). Adding a new subscriber inside `ExpeditionSystem` itself (or wiring `_cooldowns.RecordAction` from the host session alongside the existing subscribers) is additive and does not change either event's signature, so this step is lower-risk than Step 4's trade change. The only new risk: `Start(...)` gains an additional rejection path (cooldown), so any caller that assumed "returns false only when a survivor already has an active expedition" must be updated to also handle the new false-return reason — recommend returning a richer result (or an `out` reason) instead of overloading the existing `bool`, OR keep `bool Start(...)` unchanged and add a separate `CanDispatch(string survivorId)` query the host UI calls *before* invoking `Start`, so `Start`'s contract does not change. Rollback: inject a no-op `ICooldownService` (see Step 4's null-object plan) to disable without reverting code.

### Done when
- Returned survivors have a 2-day mandatory recovery period, recorded via `OnExpeditionCompleted` and `OnExpeditionFailed` (not by modifying the private `Fail` method).
- `Start(...)`'s existing `bool` contract and its existing concurrent-dispatch guard (`_active.ContainsKey`) are both preserved; the cooldown check is an additional, independent guard.
- UI can query `DaysRemaining` to show "Available in N days."
- No regression in existing expedition tests, including the two confirmed real subscribers in `src/Host/ExpeditionHostSession.cs:84-85`.

---

## Step 6 — Wire Medical Item Cooldown (Anti-Rad: 1 Day Between Doses)

### Goal
Prevent survivors from consuming anti-radiation medication (or other medical items) faster than the intended rate. Anti-rad has a 1-day cooldown per survivor.

### Implementation

**CORRECTED — integration point verified:** The real call site is `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs`, `OnSubstanceConsumed(survivorId, itemId, kind)` — this method currently applies dependency effects unconditionally with no day parameter and no interval check. Separately, `Assets/Ashfall.Core/DoseLedgerSystem.cs` already has a field `lastAntiRadDay` on its entry type, but it is written only for dose-reduction math and is **never read as a gate** — it does not currently block repeat anti-rad use. Do not assume `lastAntiRadDay` already enforces a cooldown; it doesn't. This step must either (a) wire the new `ICooldownService` alongside `OnSubstanceConsumed`, independent of `lastAntiRadDay`, or (b) repurpose `lastAntiRadDay` as the actual gate and drop the parallel `ICooldownService` check for this one case — pick one approach explicitly to avoid two competing sources of truth for the same 1-day anti-rad limit.

```csharp
// Before consuming anti-rad:
var key = new CooldownKey("action_anti_rad_dose", survivorId);
if (_cooldowns.IsOnCooldown(key))
{
    return MedicalResult.TooSoon(_cooldowns.DaysRemaining(key));
}

// After consuming:
_cooldowns.RecordAction(key);
// Apply anti-rad effect...
```

**Extensibility:**
- Other medical items can have their own cooldown entries in `cooldowns.json`:
  - `action_iodine_dose` — potassium iodide (1 day).
  - `action_chelation_treatment` — chelation agents (2 days).
  - `action_stimulant_dose` — fatigue stimulant (1 day).
- The pattern is the same: check before consume, record after consume.

**CORRECTED — the two-competing-sources-of-truth question must be decided now, not left open:** Recommended resolution: **(a)** — wire `ICooldownService` independently and leave `DoseLedgerSystem.lastAntiRadDay` exactly as-is (it currently only feeds dose-reduction math in `BookReading`, unrelated to gating). Reason: `lastAntiRadDay` lives on `DoseEntry` inside `DoseLedgerSystem`, which has no reference to `ICooldownService` or any cooldown catalog, and repurposing it as a gate would require `DoseLedgerSystem` to take a new dependency it doesn't otherwise need, coupling two systems that are currently independent. Keeping the cooldown check in `ChemicalDependencySystem.OnSubstanceConsumed` (the actual real call site, confirmed by direct read — it currently applies dependency effects unconditionally with no day parameter and no interval check) is the smaller, more localized change. Do not implement both gates; if a future task discovers `lastAntiRadDay` should also gate, that is a separate, explicit follow-up batch, not a silent side effect of this one.

**CORRECTED — risk/rollback (previously missing):** `OnSubstanceConsumed(string survivorId, string itemId, ChemicalDependencyKind kind)` is the real, confirmed signature — it has no `day` parameter today. Adding a cooldown check inside it (or immediately before/after the call) does not require changing this signature, since the day comes from `IClock`/`ICooldownService` injected at construction, not from the caller. This keeps the change additive at the call-site level. Risk: `OnSubstanceConsumed` is presumably called by other Core or host code performing dependency-formation side effects unrelated to anti-rad (e.g. alcohol, stimulants) — confirm via `find_references` on `OnSubstanceConsumed` before adding the anti-rad-specific cooldown check, and gate the check to `itemId`/`kind` values that are actually anti-rad, not all substances. Rollback: same no-op `ICooldownService` pattern as Steps 4 and 5.

### Verification
- Test: consume anti-rad → attempt again same day → blocked.
- Test: advance 1 day → can consume again.
- Test: different medical item → independent cooldown.
- Test: different survivor → independent cooldown.
- Test: confirm `lastAntiRadDay` is unchanged by this step (still write-only for dose-reduction math) to catch accidental dual-gating regressions.

### Done when
- Anti-rad consumption is limited to once per day per survivor, gated exclusively through `ICooldownService` (not `lastAntiRadDay`).
- Cooldown catalog entry is the source of truth for duration.
- Save/load round-trip preserves medical cooldowns.
- `find_references` on `OnSubstanceConsumed` has been run and all call sites reviewed so the new check only applies to anti-rad-kind consumption, not every substance.

---

## Step 7 — Write Cooldown System Tests

### Goal
Comprehensive test coverage for the entire cooldown subsystem: registry behavior, catalog loading, jitter determinism, integration with game systems, and save/load round-trips.

### Implementation

**File:** `Ashfall.Core.Tests/CooldownSystemTests.cs`

```csharp
namespace Ashfall.Core.Tests
{
    public class CooldownRegistryTests
    {
        [Fact] public void RecordAction_IsOnCooldown_ReturnsTrue();
        [Fact] public void AdvancePastExpiry_IsOnCooldown_ReturnsFalse();
        [Fact] public void DaysRemaining_DecrementsCorrectly();
        [Fact] public void ClearCooldown_ImmediatelyAvailable();
        [Fact] public void DifferentScope_IndependentCooldowns();
        [Fact] public void PerPairKey_DifferentTargets_Independent();
        [Fact] public void GlobalScope_SharedAcrossAllEntities();
        [Fact] public void JitterDeterministic_SameSeed_SameResult();
        [Fact] public void JitterBounded_NeverExceedsMinMax();
        [Fact] public void ExpiredEntries_CleanedOnAdvance();
    }

    public class CooldownCatalogTests
    {
        [Fact] public void LoadFromJson_AllDefsPresent();
        [Fact] public void UnknownActionId_Throws();
        [Fact] public void AllActionIds_HaveActionPrefix();
        [Fact] public void MinMaxDuration_Ordered();
    }

    public class CooldownSaveLoadTests
    {
        [Fact] public void CaptureState_RestoreState_RoundTrip();
        [Fact] public void RestoreState_PreservesExpiryDays();
        [Fact] public void EmptyRegistry_CaptureState_EmptyList();
        [Fact] public void MidCooldown_SaveLoad_StillBlocked();
    }

    public class CooldownIntegrationTests
    {
        [Fact] public void TradeCooldown_BlocksSamePartner();
        [Fact] public void TradeCooldown_AllowsDifferentPartner();
        [Fact] public void ExpeditionRecovery_BlocksDispatch();
        [Fact] public void MedicalCooldown_BlocksConsumption();
        [Fact] public void MultipleActions_IndependentTracking();
    }
}
```

**Test infrastructure:**

**CORRECTED — both referenced helpers were unverified/wrong:**
- ~~`FakeClock` (already exists in test project)~~ — **does not exist.** No `FakeClock` type was found anywhere in `Ashfall.Core.Tests/`. The closest precedent is a private `ManualClock : IClock` class nested inside `Ashfall.Core.Tests/YearOfAshTests.cs:589`. This batch must either extract a shared `ManualClock`/`FakeClock` test helper (recommended, since multiple test classes below need day control) or define its own private one inside `CooldownSystemTests.cs` — do not assume a reusable fake already exists.
- ~~`CoreSeededRng` with fixed seed~~ — **wrong class name.** The real concrete implementation is `SeededRng : ISeededRng` in `Assets/Ashfall.Core/HostDefaults.cs:96`, constructed as `new SeededRng(seed)`. `CoreSeededRng` does not exist anywhere in the codebase.
- In-memory JSON strings for catalog loading (no file system dependency in unit tests) — unverified against existing patterns but consistent with other catalog loaders; fine as stated.

**Edge case coverage:**
- Record action when already on cooldown → extends? resets? (Design decision: no-op, original cooldown stands).
- Negative days remaining → clamped to 0.
- Very large cooldown (365 days) → handles correctly.
- Empty catalog → no cooldowns enforced (graceful fallback).

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Must compile
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # All pass
dotnet build Ashfall.csproj                                  # Godot host clean
godot --headless --path . -- --data-integrity-selftest       # 0 errors (new JSON valid)
godot --headless --path . -- --bridge-selftest               # Exits 0
```

### Done when
- All 23+ tests pass.
- Coverage: registry behavior, catalog loading, jitter, save/load, integration.
- Tests are deterministic and engine-independent.
- No regressions in existing test suite.

---

## Summary Table

| Step | Deliverable | Files | Key Risk | Estimated LOC |
|------|------------|-------|----------|---------------|
| 1 | `ICooldownService`, `CooldownKey`, `CooldownScope` | `Cooldowns/ICooldownService.cs` | None — interface only | ~70 |
| 2 | `CooldownRegistry` (impl + save/load) | `Cooldowns/CooldownRegistry.cs` | Jitter correctness, expiry edge cases | ~250 |
| 3 | `cooldowns.json` + `CooldownCatalog` loader + `"action_"` added to `CatalogIntegrityValidator.IdPrefixes` | `StreamingAssets/Data/cooldowns.json`, `Cooldowns/CooldownCatalog.cs`, `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | Data integrity validation; forgetting the `IdPrefixes` edit fails `--data-integrity-selftest` | ~185 |
| 4 | Trade cooldown wiring | `Assets/Ashfall.Core/HoldfastTradeSession.cs` (constructor + `Buy`/`Sell`); update all call sites in `Ashfall.Core.Tests/HoldfastTradeSessionTests.cs`, `src/Host/HoldfastRuntimeSession.cs`, `src/Host/HoldfastTerminalPanel.cs` | **Breaking signature change** — real callers confirmed by grep, must be updated in the same commit | ~40 core + call-site updates |
| 5 | Expedition recovery wiring | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` (`Start`, new `CanDispatch`); subscribers wired in `src/Host/ExpeditionHostSession.cs` (existing `OnExpeditionCompleted`/`OnExpeditionFailed` subscribers at lines 84-85) | Additive via existing public events; low regression risk if `Start`'s bool contract is preserved | ~40 |
| 6 | Medical item cooldown wiring | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` (`OnSubstanceConsumed`) — explicitly NOT `DoseLedgerSystem.lastAntiRadDay` | Must scope the check to anti-rad kind only; verify via `find_references` before editing | ~40 |
| 7 | Test suite (23+ tests) | `Ashfall.Core.Tests/CooldownSystemTests.cs` (+ extracting a shared `ManualClock`/`FakeClock` helper if not already present) | Covering all scoping combinations; no reusable clock fake exists yet | ~350 |

**Total estimated:** ~975 lines across 7-9 files (higher than the original ~970/6-7 estimate once the three real call-site files in Steps 4-6 are counted individually rather than as one-line "system modification" entries).

---

## Verification Checklist (per project rules)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Must compile cleanly
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # All tests pass (including new CooldownSystemTests)
3. dotnet build Ashfall.csproj                                  # Godot host: 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest       # Catalog integrity: 0 errors (cooldowns.json validated)
5. godot --headless --path . -- --bridge-selftest               # Exits 0
```

---

## Exit Criteria

This batch is complete when:
- [ ] `ICooldownService` interface defines the full cooldown API contract.
- [ ] `CooldownRegistry` implements the service with deterministic jitter via `ISeededRng` (the real class, `Ashfall.Core.SeededRng` from `HostDefaults.cs` — not `CoreSeededRng`, which does not exist in Core).
- [ ] `cooldowns.json` exists in data authority with `schema_version` and 6+ action definitions.
- [ ] `CooldownCatalog` loads and validates all definitions.
- [ ] `"action_"` has been added to `CatalogIntegrityValidator.IdPrefixes` in the same commit as `cooldowns.json` (verified absent today; `--data-integrity-selftest` fails without it).
- [ ] `HoldfastTradeSession.Buy`/`Sell` check cooldown before allowing a same-partner trade, AND all three confirmed real call sites (`Ashfall.Core.Tests/HoldfastTradeSessionTests.cs`, `src/Host/HoldfastRuntimeSession.cs`, `src/Host/HoldfastTerminalPanel.cs`) compile against any signature change.
- [ ] `ExpeditionSystem` enforces 2-day recovery per survivor via the public `OnExpeditionCompleted`/`OnExpeditionFailed` events (not the private `Fail` method), and `Start(...)`'s existing `bool` contract is unchanged.
- [ ] `ChemicalDependencySystem.OnSubstanceConsumed` enforces a 1-day cooldown on anti-rad-kind consumption specifically (not all substances), with `DoseLedgerSystem.lastAntiRadDay` left untouched as a separate, unrelated field.
- [ ] `CaptureState/RestoreState` round-trip preserves all active cooldowns.
- [ ] All tests listed in Step 7 (23 named `[Fact]`s enumerated above) pass — "23+" is not a target to hit approximately; every named test in the plan must exist and pass, and any additional tests added during implementation are a bonus, not a substitute.
- [ ] Data integrity selftest passes with new JSON file.
- [ ] Zero engine coupling in all new code.
- [ ] All 5 verification steps pass.

---

## Future Extensions (out of scope for this batch)

- **Cooldown reduction traits** — survivor traits that reduce cooldown duration (e.g., "Quick Trader" = -1 day trade cooldown).
- **Narrative cooldown overrides** — quest events that clear or extend cooldowns ("embargo lifted" / "faction angered").
- **UI cooldown timers** — visual countdown indicators on action buttons.
- **Stacking cooldowns** — multiple actions on the same key extend rather than no-op.
- **Conditional cooldowns** — cooldown only applies if certain conditions are met (e.g., trade value exceeds threshold).


---

## Review Notes (Corrected)

This batch already carried a first round of "CORRECTED" annotations before this pass. This pass verified those against the real codebase and fixed the remaining errors found. Summary of everything wrong that has now been fixed, in one place:

### Factual errors found and fixed

1. **`ExpeditionSystem.Fail` is `private`, not a hookable integration point (Step 5).** The plan told the implementer to "hook `OnExpeditionCompleted`/`Fail`" as if both were addressable the same way. `Fail(ExpeditionState, string)` is a private method called only from inside `TickHours`; the only public surface for the failure path is the `OnExpeditionFailed` event. Fixed: Step 5 now names the two real public events (`OnExpeditionCompleted`, `OnExpeditionFailed`) as the only integration points, and confirms (via direct read of `src/Host/ExpeditionHostSession.cs:84-85`) that both already have live subscribers today, which is a real regression risk the original plan didn't call out.
2. **Step 5's code sample referenced `expedition.Participants`, which does not exist.** `ExpeditionState` has a single `survivorId : string`, not a participant collection — this project's expeditions are one-survivor-per-instance. Fixed: replaced the sample with code against the real event signatures and the real single-survivor field.
3. **`cooldowns.json`'s `action_` prefix gap (Step 3) was correctly identified in the prior pass** — confirmed by direct read of the full ~180-entry `IdPrefixes` array in `CatalogIntegrityValidator.cs`: neither `action_` nor `achievement_` (relevant to the sibling Batch 104) appear anywhere in it. The prior correction was accurate and is retained.
4. **`HoldfastTradeSession` real API (Step 4) was correctly identified in the prior pass** — confirmed exact signatures `Buy(string itemId, int quantity, string factionId)` / `Sell(string itemId, int quantity, string factionId)`, no day parameter, and confirmed the three real call sites that would break on a signature change: `Ashfall.Core.Tests/HoldfastTradeSessionTests.cs`, `src/Host/HoldfastRuntimeSession.cs`, `src/Host/HoldfastTerminalPanel.cs`. This pass replaced the vague "locate the actual test file" instruction in the verification section with the confirmed filename, since it is now known.
5. **`DoseLedgerSystem.lastAntiRadDay` / `ChemicalDependencySystem.OnSubstanceConsumed` (Step 6) — the prior pass correctly found the dual-gating risk but left the decision open ("pick one approach explicitly").** A plan that says "decide later" is not actionable. Fixed: this pass makes the decision (wire `ICooldownService` independently in `ChemicalDependencySystem`, leave `lastAntiRadDay` untouched) and states the reasoning, so the implementer isn't left re-deriving the same tradeoff analysis.
6. **`SeededRng` vs `CoreSeededRng` (Step 7 test infra) — the prior pass's correction was right for Core code** (`Ashfall.Core.SeededRng` in `HostDefaults.cs:96`) but the Exit Criteria and Summary Table still hadn't been updated to match. Fixed: propagated the correct class name into the Exit Criteria.

### Scope creep / underspecification found and fixed

7. **Steps 5 and 6 had no Risk/Rollback subsection while Step 4 did.** Added matching Risk/Rollback notes to both, following the same "inject a no-op `ICooldownService`" pattern established in Step 4, so all three integration steps are disable-able the same way without a revert.
8. **Summary Table's Files column said "Trade system modification" / "Expedition system modification" / "Medical system modification"** — vague placeholders that don't name a single real file, despite the step bodies having since been corrected to name exact files. Fixed: Summary Table now lists the exact confirmed file paths per step, matching the step bodies.
9. **Vague Done-when ("23+ tests pass covering all scenarios").** "23+" as a floor with no enumeration lets an implementer under-deliver relative to the plan's own Step 7 test list (which names exactly 23 `[Fact]`s). Fixed: Exit Criteria now states every named test in Step 7 must exist and pass; "23+" is a floor on top of the named list, not a substitute for it.
10. **Step 5's Done-when didn't mention preserving `Start`'s existing contract**, which is a real risk once a new rejection path (cooldown) is added to a method whose callers currently only handle one false-return reason. Fixed: Done-when now requires either an `out`/richer result or a separate `CanDispatch` query so `Start`'s `bool` signature and meaning don't silently change.

### What was already correct and did not need fixing

- `ICooldownService`/`CooldownKey`/`CooldownScope` design (Step 1) — no existing cooldown/timer infrastructure was found anywhere in `MarketSystem`, `HoldfastTradeSession`, or `ExpeditionSystem` (confirmed by direct read of all three), so the "no existing cooldown/timer logic" motivating claim is accurate and the interface design is sound.
- Jitter bounding logic, `CaptureState/RestoreState` versioning pattern, and the `ManualClock`/`FakeClock` test-helper gap (Step 7) — all previously verified accurately in the prior pass; retained as-is.
- The five canonical verification commands match the project's actual `dotnet`/`godot --headless` pipeline (`AGENTS.md`); no Unity commands appear anywhere in this batch.
