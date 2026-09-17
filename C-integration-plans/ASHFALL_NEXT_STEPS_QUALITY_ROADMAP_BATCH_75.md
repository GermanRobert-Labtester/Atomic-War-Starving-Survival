# ASHFALL — Quality Roadmap Batch 75

## Theme: Difficulty Scaling Framework — Dynamic Challenge Adjustment

| Field | Value |
|-------|-------|
| **Batch** | 75 |
| **Priority** | MEDIUM |
| **Risk** | Medium |
| **Estimated Effort** | 7-10 working days |
| **Prerequisite Batches** | None (reads existing system APIs, does not modify them) |
| **Systems Touched** | AdaptiveDifficultySystem (new), IDifficultyProvider (new), NeedsSystem, RadiationSystem, WeatherSystem, DiseaseSystem, MarketSystem, ExpeditionSystem, CombatTraumaSystem |

---

## Motivation

**Correction (see Review Notes):** the game has no difficulty-selection system today, dynamic or otherwise. `StartingLevelSystem` (`Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs`) is the Day-1 Holdfast bunker-setup/tutorial system — it manages starting room shielding, a morning ration-policy choice, a midday maintenance directive, an evening radio-protocol choice, and air-filter/radon degradation. It has no difficulty enum, no multiplier field, and no concept of "difficulty" at all. There is no dynamic-difficulty gap to fill in an existing system — this batch introduces the concept of adaptive difficulty from scratch. Static difficulty (implicitly, whatever fixed constants each system currently uses) creates two failure modes in a 300-day survival game:

1. **Triviality spiral** — An optimizing player stockpiles resources, the game becomes a passive click-through with no tension by day 100.
2. **Death spiral** — A single bad event cascades (radiation exposure -> sickness -> missed work shifts -> resource deficit -> starvation) with no rubber-banding to prevent an unwinnable state.

Both outcomes lead to player abandonment. An adaptive difficulty system reads player performance metrics and adjusts system multipliers with smoothing and hysteresis, keeping the game in the "anxious but survivable" zone that defines the genre.

Core systems that benefit from scaling (verified to exist in `Assets/Ashfall.Core/` under these exact names — see Review Notes for corrections to the original list):
- `NeedsSystem` (`Survivors/NeedsSystem.cs`) — decay rates for hunger/thirst/fatigue
- `RadiationSystem` (`Radiation/RadiationSystem.cs`) — dose multipliers, decontamination effectiveness
- `WeatherSystem` (`World/WeatherSystem.cs`) — storm frequency, severity, duration
- `DiseaseSystem` (`Disease/DiseaseSystem.cs`) — outbreak probability, progression speed
- `MarketSystem` (`Economy/MarketSystem.cs`) — price inflation, trade availability (**not** `DynamicEconomySystem` — that class does not exist in this codebase; see Review Notes)
- `ExpeditionSystem` (`Expeditions/ExpeditionSystem.cs`) — danger levels, loot quality
- `CombatTraumaSystem` (`Survivors/CombatTraumaSystem.cs`) — enemy scaling, encounter frequency (**not** `CombatSystem` — no class with that exact name exists; see Review Notes)

---

## Step 1 — Design IDifficultyProvider Interface

### Goal
Define a clean, engine-agnostic contract that any system can query for its current difficulty multiplier without coupling to the adaptive logic.

### Implementation
- Create `Assets/Ashfall.Core/Difficulty/IDifficultyProvider.cs`:
  ```csharp
  namespace Ashfall.Core.Difficulty;

  /// <summary>
  /// Provides difficulty multipliers for game systems.
  /// Multiplier of 1.0 = baseline difficulty.
  /// Greater than 1.0 = harder. Less than 1.0 = easier.
  /// </summary>
  public interface IDifficultyProvider
  {
      float GetMultiplier(DifficultyDomain domain);
      DifficultySnapshot GetSnapshot(); // Full state for UI/save
  }

  public enum DifficultyDomain
  {
      NeedsDecay,           // How fast hunger/thirst/fatigue increase
      RadiationDose,        // Radiation dose multiplier
      RadiationHealing,     // Decontamination/chelation effectiveness (inverse)
      StormFrequency,       // How often fallout storms occur
      StormSeverity,        // Intensity of storms when they do occur
      DiseaseOutbreak,      // Probability of disease events
      DiseaseProgression,   // Speed of disease progression
      EconomyInflation,     // Price multiplier for trades
      ExpeditionDanger,     // Danger level of expeditions
      ExpeditionLoot,       // Loot quality/quantity (inverse scaling)
      CombatDifficulty,     // Enemy strength/numbers
      CombatLoot,           // Combat reward scaling (inverse)
  }

  public sealed class DifficultySnapshot
  {
      public int Day { get; init; }
      public float OverallMultiplier { get; init; }
      public Dictionary<DifficultyDomain, float> Multipliers { get; init; } = new();
      public PerformanceTier CurrentTier { get; init; }
  }

  public enum PerformanceTier
  {
      Struggling,   // Player is clearly losing
      Challenged,   // Player is under pressure but surviving
      Stable,       // Player is managing comfortably
      Thriving,     // Player has excess resources and safety
      Dominant      // Player has trivialized the game
  }
  ```
- Create a `NullDifficultyProvider` that always returns 1.0f (for tests and opt-out scenarios).
- Ensure the interface has no engine dependencies — pure C# in `Ashfall.Core`.

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

### Done-when
- `IDifficultyProvider` compiles in Core with zero engine references.
- `NullDifficultyProvider` exists and returns 1.0f for all domains.
- `DifficultyDomain` enum covers all planned scaling axes.
- `DifficultySnapshot` is serializable (plain C# DTO) for save/load.

---

## Step 2 — Define Performance Metrics

### Goal
Establish the input signals that the adaptive system uses to assess player performance — objective, measurable, and not gameable.

### Implementation
- Create `Assets/Ashfall.Core/Difficulty/PerformanceMetrics.cs`:
  ```csharp
  namespace Ashfall.Core.Difficulty;

  public sealed class PerformanceMetrics
  {
      // Survival indicators
      public int DaysSurvived { get; set; }
      public int AliveSurvivorCount { get; set; }
      public int SurvivorCountTrend { get; set; }    // +/- over last 30 days
      public int DeathsLast30Days { get; set; }

      // Resource indicators
      public float FoodDaysRemaining { get; set; }   // Days of food at current consumption
      public float WaterDaysRemaining { get; set; }
      public float MedicineSurplus { get; set; }     // Ratio: stock / projected need
      public float FuelDaysRemaining { get; set; }

      // Health indicators
      public float AverageRadiationLevel { get; set; }
      public int ActiveAfflictionCount { get; set; }
      public float AverageHealthPercent { get; set; }

      // Progress indicators
      public int CompletedExpeditions { get; set; }
      public int FailedExpeditions { get; set; }
      public float ExpeditionSuccessRate { get; set; }
      public int TradeSessionsCompleted { get; set; }
  }
  ```
- Create `Assets/Ashfall.Core/Difficulty/PerformanceEvaluator.cs`:
  ```csharp
  public sealed class PerformanceEvaluator
  {
      public PerformanceTier Evaluate(PerformanceMetrics metrics) { ... }
  }
  ```
- Tier thresholds:
  - **Struggling**: deaths > 2 in 30 days OR food < 3 days OR avg health < 30%
  - **Challenged**: deaths > 0 in 30 days OR food < 7 days OR active afflictions > 3
  - **Stable**: food 7-20 days, health 50-75%, manageable radiation
  - **Thriving**: food > 20 days, health > 75%, no deaths in 30 days, expedition success > 80%
  - **Dominant**: food > 40 days, all survivors healthy, no threats active
- Evaluation uses worst-case weighting (any single "Struggling" signal dominates).

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~PerformanceEvaluator"
```

### Done-when
- `PerformanceMetrics` captures all relevant signals without engine coupling.
- `PerformanceEvaluator.Evaluate()` correctly classifies example scenarios into tiers.
- Unit tests cover all tier boundaries and edge cases (e.g., mixed signals).
- Metrics are gatherable from existing Core system state (no new sensors needed).

---

## Step 3 — Implement AdaptiveDifficultySystem

### Goal
Build the central difficulty adjustment engine that translates performance tiers into domain multipliers with smoothing and hysteresis.

### Implementation
- Create `Assets/Ashfall.Core/Difficulty/AdaptiveDifficultySystem.cs`:
  ```csharp
  namespace Ashfall.Core.Difficulty;

  public sealed class AdaptiveDifficultySystem : IDifficultyProvider
  {
      private readonly PerformanceEvaluator _evaluator;
      private readonly DifficultyConfig _config;
      private readonly Dictionary<DifficultyDomain, float> _currentMultipliers;
      private readonly Dictionary<DifficultyDomain, float> _targetMultipliers;
      private PerformanceTier _currentTier;
      private PerformanceTier _previousTier;
      private int _ticksInCurrentTier;

      public void UpdateMetrics(PerformanceMetrics metrics) { ... }
      public float GetMultiplier(DifficultyDomain domain) => _currentMultipliers[domain];
      public DifficultySnapshot GetSnapshot() { ... }

      // Save/load
      public AdaptiveDifficultyState CaptureState() => ...;
      public void RestoreState(AdaptiveDifficultyState state) { ... }
  }
  ```
- **Smoothing**: Multipliers lerp toward targets at 5% per day tick (no sudden jumps).
- **Hysteresis**: Tier must be stable for 7 days before multipliers shift (prevents oscillation from a single bad/good day).
- **Bounds**: No multiplier goes below 0.5 or above 2.0 (prevents absurd extremes).
- **Per-domain curves**: Each `DifficultyDomain` has its own response curve to tier changes:
  - `NeedsDecay`: Struggling=0.7, Stable=1.0, Dominant=1.4
  - `StormFrequency`: Struggling=0.6, Stable=1.0, Dominant=1.5
  - `ExpeditionDanger`: Struggling=0.8, Stable=1.0, Dominant=1.3
- Create `DifficultyConfig` (plain C# class) holding all tuning parameters.
- Implement `CaptureState/RestoreState` with a serializable `AdaptiveDifficultyState` DTO.

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~AdaptiveDifficulty"
```

### Done-when
- System correctly adjusts multipliers based on tier.
- Smoothing prevents jumps larger than 5% per day.
- Hysteresis prevents oscillation (test: alternate good/bad days, multiplier stays near 1.0).
- Save/load round-trips all state including current multipliers and tier history.
- Deterministic: same metric sequence with same config = same multiplier sequence.

---

## Step 4 — Wire NeedsSystem and RadiationSystem

### Goal
Connect `NeedsSystem` decay rates and `RadiationSystem` dose calculations to read multipliers from `IDifficultyProvider`.

### Implementation
- Add `IDifficultyProvider` as a constructor parameter to `NeedsSystem` (with `NullDifficultyProvider` default for backward compatibility):
  ```csharp
  public NeedsSystem(IDifficultyProvider? difficulty = null)
  {
      _difficulty = difficulty ?? NullDifficultyProvider.Instance;
  }
  ```
- In `NeedsSystem.TickDay(...)`, multiply base decay rates:
  ```csharp
  float hungerDecay = BaseHungerDecay * _difficulty.GetMultiplier(DifficultyDomain.NeedsDecay);
  ```
- In `RadiationSystem`, apply `DifficultyDomain.RadiationDose` to incoming dose calculations.
- Apply `DifficultyDomain.RadiationHealing` as a divisor to chelation/decontamination effectiveness.
- Ensure `NullDifficultyProvider` (returns 1.0f) means existing behavior is unchanged.
- Update all test constructions that create these systems directly — pass `NullDifficultyProvider.Instance` or leave null for default.

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Needs"
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Radiation"
dotnet build Ashfall.csproj
```

### Done-when
- `NeedsSystem` reads decay multiplier from `IDifficultyProvider`.
- `RadiationSystem` reads dose and healing multipliers.
- All existing Needs tests (58+) pass unchanged (using null/default provider).
- New tests verify: multiplier > 1 increases decay, multiplier < 1 decreases decay.
- Godot host compiles cleanly.

---

## Step 5 — Wire Weather, Disease, Combat, and Economy Systems

### Goal
Connect remaining challenge-generating systems to the difficulty framework.

### Implementation
- **WeatherSystem** (`Assets/Ashfall.Core/World/WeatherSystem.cs`, current ctor: `WeatherSystem(WorldWeatherState state = null)`): Read `DifficultyDomain.StormFrequency` when rolling for storm occurrence. Read `DifficultyDomain.StormSeverity` when determining storm intensity. Applied as multipliers to the base probability/intensity from weather data. Adding an `IDifficultyProvider` parameter here is a bigger relative change than for `NeedsSystem`/`RadiationSystem`/`DiseaseSystem` since it only has one existing optional parameter today — still consistent with the ports-and-adapters pattern, just flag it as a smaller-precedent change.
- **DiseaseSystem** (`Assets/Ashfall.Core/Disease/DiseaseSystem.cs`, current ctor already takes `ISeededRng`, `Func<int, ISeededRng>`, `ILog` as optional params): Read `DifficultyDomain.DiseaseOutbreak` when rolling outbreak probability. Read `DifficultyDomain.DiseaseProgression` when advancing disease stages. This system already follows the ports-and-adapters DI pattern most closely — lowest-risk integration of the five.
- **`CombatTraumaSystem`** (`Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs`) — **not** `CombatSystem`; no class of that name exists in this codebase. Read `DifficultyDomain.CombatDifficulty` when scaling enemy stats. Read `DifficultyDomain.CombatLoot` when determining rewards. **Caveat:** this system does not follow the constructor-injection pattern used elsewhere — it exposes `Rng` and `ApplyMoraleDelta` as public mutable fields assigned post-construction, and per AGENTS.md Invariant 4 it is a known determinism offender (`public System.Random Rng;`, not yet migrated to `ISeededRng`). Adding an `IDifficultyProvider` field here should follow the same pattern already in use on this class (public settable property) rather than inventing a constructor parameter that doesn't match its existing shape, and should NOT be used as an opportunity to silently "fix" the `Rng` determinism issue in this batch (that is Invariant 4 cleanup, out of scope — track separately, but do not introduce a *new* undeterministic pattern either).
- **ExpeditionSystem** (`Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`, current ctor: `ExpeditionSystem()`, parameterless): Read `DifficultyDomain.ExpeditionDanger` for danger scaling. Read `DifficultyDomain.ExpeditionLoot` for loot table adjustments. This is the largest constructor-shape change of the five systems — going from zero to one optional parameter is fine, but note that adding a first DI seam to a previously-parameterless constructor may require touching more call sites than systems that already take optional params (grep all `new ExpeditionSystem()` sites before assuming a drop-in change).
- **`MarketSystem`** (`Assets/Ashfall.Core/Economy/MarketSystem.cs`) — **not** `DynamicEconomySystem`; that class does not exist anywhere in this codebase (it is referenced only in AGENTS.md's stale description of a legacy `Assets/_Game/Economy/` tree that is not present in this checkout). Read `DifficultyDomain.EconomyInflation` as a price multiplier on all trade calculations.
- Pattern: each system accepts `IDifficultyProvider?` in constructor (or public settable field where the system already uses that pattern, e.g. `CombatTraumaSystem`), defaults to `NullDifficultyProvider.Instance`.
- Multipliers apply as simple scaling factors to existing base values — no structural changes to system logic.

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
```

### Done-when
- All five systems (`WeatherSystem`, `DiseaseSystem`, `CombatTraumaSystem`, `ExpeditionSystem`, `MarketSystem`) accept and use `IDifficultyProvider` via whichever DI shape each already uses (constructor parameter or public field).
- Existing tests pass with default (1.0) multipliers.
- New tests verify each system responds to non-1.0 multipliers correctly (minimum: one test per system per domain it reads, i.e. 2 tests for `WeatherSystem`, 2 for `DiseaseSystem`, 2 for `CombatTraumaSystem`, 2 for `ExpeditionSystem`, 1 for `MarketSystem` — 9 tests minimum, not just "new tests" left unquantified).
- Determinism preserved: same seed + same multiplier sequence = same outcomes (add an explicit determinism test per touched system, not just an assertion in prose).
- Data integrity selftest passes (difficulty doesn't affect catalog validation).
- All call sites of `new ExpeditionSystem()` (parameterless today) are enumerated and updated or confirmed compatible with the new optional parameter before this step is marked done.

---

## Step 6 — Difficulty Curve Visualization Data

### Goal
Expose current difficulty state in a format the Godot UI can display, allowing players to understand why the game is getting easier/harder.

### Implementation
- Add to `DifficultySnapshot`:
  ```csharp
  public sealed class DifficultySnapshot
  {
      // ... existing fields ...
      public List<DifficultyHistoryPoint> RecentHistory { get; init; } = new();
  }

  public sealed class DifficultyHistoryPoint
  {
      public int Day { get; init; }
      public float OverallMultiplier { get; init; }
      public PerformanceTier Tier { get; init; }
  }
  ```
- `AdaptiveDifficultySystem` maintains a rolling window of last 60 days of history points.
- History is included in `CaptureState()` and restored on load.
- Create `Assets/Ashfall.Core/Difficulty/DifficultyDisplayData.cs`:
  ```csharp
  public sealed class DifficultyDisplayData
  {
      public PerformanceTier CurrentTier { get; init; }
      public string TierDescription { get; init; } = "";  // Human-readable
      public float OverallMultiplier { get; init; }
      public DifficultyDomain MostAffectedDomain { get; init; }
      public float MostAffectedMultiplier { get; init; }
      public bool IsAdjusting { get; init; }  // Currently lerping toward new target
  }
  ```
- Provide `GetDisplayData()` method on `AdaptiveDifficultySystem` for the UI host to call.
- Tier descriptions (tone: cold, restrained, per project rules):
  - Struggling: "Your group is barely holding together."
  - Challenged: "Every day is a fight. Resources are thin."
  - Stable: "You're surviving. For now."
  - Thriving: "Supplies are adequate. The wasteland notices."
  - Dominant: "You've carved out safety. The world will test it."

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Difficulty"
dotnet build Ashfall.csproj
```

### Done-when
- `DifficultySnapshot` includes 60-day rolling history.
- `DifficultyDisplayData` provides everything a UI panel needs without engine coupling.
- History survives save/load round-trip.
- Tier descriptions match project tone (no magic, no fantasy, cold/exhausted/human).

---

## Step 7 — Difficulty Adaptation Tests

### Goal
Comprehensive test suite verifying the rubber-banding behavior works correctly across extended simulations.

### Implementation
- Create `Ashfall.Core.Tests/DifficultyAdaptationTests.cs`:
  ```csharp
  public class DifficultyAdaptationTests
  {
      [Fact]
      public void Struggling_Player_Gets_Easier_Over_Time() { ... }

      [Fact]
      public void Thriving_Player_Gets_Harder_Over_Time() { ... }

      [Fact]
      public void Stable_Player_Stays_Near_Baseline() { ... }

      [Fact]
      public void Hysteresis_Prevents_Oscillation() { ... }

      [Fact]
      public void Multipliers_Never_Exceed_Bounds() { ... }

      [Fact]
      public void Save_Load_Preserves_Full_State() { ... }

      [Fact]
      public void Null_Provider_Has_No_Effect_On_Systems() { ... }

      [Fact]
      public void Deterministic_Same_Metrics_Same_Output() { ... }

      [Fact]
      public void Smoothing_Limits_Daily_Change() { ... }

      [Fact]
      public void Death_Spiral_Detection_Eases_Rapidly() { ... }
  }
  ```
- **Death spiral test**: Simulate metrics showing 3 deaths in 10 days + zero food. Verify system drops multipliers toward 0.5 within ~14 days (hysteresis + smoothing).
- **Triviality test**: Simulate metrics showing 40+ days food, all healthy, no threats. Verify multipliers rise toward 1.5+ within ~21 days.
- **Oscillation test**: Alternate between "Struggling" and "Thriving" metrics every 3 days. Verify multipliers stay near 1.0 (hysteresis prevents response to unstable signals).
- **Bounds test**: Feed extreme metrics for 100 days. Verify no multiplier exceeds 2.0 or drops below 0.5.
- Integration test: wire `AdaptiveDifficultySystem` into `NeedsSystem`, simulate 50 days with declining metrics, verify needs decay actually decreases.

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~DifficultyAdaptation"
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj  # Full suite still passes
```

### Done-when
- All 10 difficulty adaptation tests pass.
- Tests cover: easing, hardening, stability, hysteresis, bounds, save/load, determinism, smoothing, death spiral, null provider.
- Full test suite (2120 existing `[Fact]`/`[Theory]` tests as of this writing, verify current count with `grep -rc "\[Fact\]\|\[Theory\]" Ashfall.Core.Tests/ --include="*.cs"` before relying on this figure — it drifts — + new) passes.
- No test depends on timing or threading (all deterministic tick-based).

---

## Summary Table

| Step | Description | Key Deliverable | Risk | Dependencies |
|------|-------------|-----------------|------|--------------|
| 1 | Design IDifficultyProvider interface | `IDifficultyProvider` + `DifficultyDomain` enum | Low | None |
| 2 | Define performance metrics | `PerformanceMetrics` + `PerformanceEvaluator` | Low | None |
| 3 | Implement AdaptiveDifficultySystem | Central difficulty engine with smoothing/hysteresis | Medium | Steps 1-2 |
| 4 | Wire NeedsSystem & RadiationSystem | Decay/dose responds to difficulty | Low-Med | Steps 1-3 |
| 5 | Wire Weather/Disease/Combat/Economy | All challenge systems respond to difficulty | Medium | Steps 1-3 |
| 6 | Difficulty curve visualization data | `DifficultyDisplayData` + history for UI | Low | Step 3 |
| 7 | Difficulty adaptation tests | 10 comprehensive behavior tests | Low | Steps 1-6 |

---

## Risks & Mitigations

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|------------|
| Rubber-banding feels "unfair" (player notices difficulty adjusting) | Medium | High (immersion) | Smoothing + hysteresis make changes gradual and invisible; UI shows tier but not exact multipliers |
| Multiplier interaction between domains causes unintended amplification | Medium | Medium | Each domain is independent; no multiplicative stacking between domains |
| Existing balance broken by multiplier introduction | Low | High | `NullDifficultyProvider` is the default; opt-in via `AdaptiveDifficultySystem` wire-up |
| Determinism violated if metrics collection order varies | Low | High | Metrics are snapshot at start of day tick, evaluation is pure function of snapshot |
| Save file grows from history data | Low | Low | 60 history points = ~2KB; negligible vs existing save size |
| Players abuse the system (intentionally trigger "Struggling" for easier game) | Medium | Low (single-player) | Signals are lagging indicators (30-day trends); short-term manipulation doesn't help |
| `ExpeditionSystem`'s parameterless constructor has unknown call-site count | Medium | Medium | Enumerate all `new ExpeditionSystem()` sites before Step 5; if call sites are numerous, prefer a settable property over a breaking constructor signature change |
| `CombatTraumaSystem` DI wiring gets conflated with its known `System.Random`/determinism cleanup (Invariant 4) | Medium | Medium | Scope this batch strictly to adding the difficulty hook via the existing public-field pattern; do not attempt the `Rng` migration in the same PR — track it as a separate task |

## Rollback Plan

- Every touched system takes `IDifficultyProvider` as an **optional** parameter/field defaulting to `NullDifficultyProvider.Instance`. If the feature needs to be disabled post-merge, wiring `NullDifficultyProvider` everywhere (or simply not constructing/registering `AdaptiveDifficultySystem` in the host) fully reverts behavior to pre-batch baseline with no further code changes — this is the intended kill switch and should be explicitly tested (Step 7, `Null_Provider_Has_No_Effect_On_Systems`).
- Because each of the 5 wiring steps (Steps 4-5) touches independent systems, they can be merged and reverted independently — a regression traced to, say, `WeatherSystem`'s wiring does not require reverting `NeedsSystem`'s wiring.
- If `AdaptiveDifficultySystem.CaptureState()`/`RestoreState()` is found to be incompatible with an in-flight save after partial rollout, the mitigation is to treat missing/malformed difficulty state as "absent" and fall back to `NullDifficultyProvider` for that load rather than failing the whole save load (graceful degradation, consistent with existing save-store patterns).

---

## Design Principles

1. **Invisible to the player by default** — No UI element says "difficulty lowered". The world just becomes slightly more forgiving or demanding.
2. **Opt-in for transparency** — Players who want to see the system can enable a debug/stats panel showing current tier and trends.
3. **Never unwinnable** — At minimum multiplier (0.5), the game is survivable with basic competence.
4. **Never trivial** — At maximum multiplier (2.0), the game is demanding but fair.
5. **Deterministic** — Same inputs produce same outputs regardless of frame rate, platform, or session length.
6. **Saveable** — Full difficulty state round-trips through save/load including history.

---

## Exit Criteria

This batch is COMPLETE when:
1. `IDifficultyProvider` interface exists in Core with `NullDifficultyProvider` default.
2. `PerformanceEvaluator` classifies scenarios into 5 tiers correctly.
3. `AdaptiveDifficultySystem` adjusts multipliers with smoothing and hysteresis.
4. `NeedsSystem`, `RadiationSystem`, `WeatherSystem`, `DiseaseSystem`, `CombatTraumaSystem`, `ExpeditionSystem`, and `MarketSystem` all read from the provider (corrected system names — see Review Notes).
5. All pre-existing tests (current count: 2120 `[Fact]`/`[Theory]` — re-verify at execution time) pass unchanged with `NullDifficultyProvider`.
6. 10 new difficulty adaptation tests pass.
7. Save/load round-trips full difficulty state.
8. Godot host builds cleanly.
9. Data integrity selftest passes.
10. `CombatTraumaSystem`'s existing public-field DI pattern (not constructor injection) is respected rather than papered over, and no new non-deterministic pattern is introduced while wiring it.


---

## Review Notes (Corrected)

This plan was adversarially reviewed against the actual codebase at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` prior to execution. The following
factual errors in the original draft were found and corrected in place above:

1. **`StartingLevelSystem` does not implement "initial difficulty selection."** The original
   Motivation section claimed "the game has `StartingLevelSystem` for initial difficulty selection
   but no dynamic adjustment during play." This is false. `StartingLevelSystem`
   (`Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs`) is the Day-1 Holdfast bunker-setup
   sequence: it manages 5 starting rooms and their shielding, a morning ration-policy choice
   (`RationPolicy`), a midday maintenance directive (`MaintenanceDirective`), an evening
   radio-protocol choice (`RadioProtocol`), and air-filter/radon degradation via `TickDay()`. It
   contains zero references to "difficulty," no difficulty enum, and no multiplier field of any
   kind. There was no dynamic-difficulty gap in an existing system to fill — this batch introduces
   the entire concept of adaptive difficulty from nothing, which changes the batch's actual risk
   profile (net-new subsystem, not an extension of partially-built functionality) even though the
   effort estimate may still be reasonable.

2. **`DynamicEconomySystem` and `CombatSystem` do not exist in this codebase.** The original
   "Systems Touched" list and Step 5 named `DynamicEconomySystem` and `CombatSystem` as integration
   points. Neither class exists anywhere under `Assets/Ashfall.Core/` or `src/`. The real classes
   are `Ashfall.Core.Economy.MarketSystem` (`Assets/Ashfall.Core/Economy/MarketSystem.cs`) and
   `Ashfall.Core.Survivors.CombatTraumaSystem` (`Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs`).
   `DynamicEconomySystem` appears to have been carried over from AGENTS.md's description of a
   legacy `Assets/_Game/Economy/DynamicEconomySystem.cs` — but `Assets/_Game/` does not exist at all
   in this checkout (confirmed: `Assets/` contains only `Ashfall.Core/`, `StreamingAssets/`, `art/`,
   `audio/`, `sprites/`, `ui/`). AGENTS.md's Unity-legacy architecture tables describe a state of
   the repo that predates this checkout; do not cite them for file/class existence without
   independently verifying against the current tree.

3. **`CombatTraumaSystem` does not use constructor-injected ports.** Unlike `NeedsSystem`,
   `RadiationSystem`, and `DiseaseSystem` (which already accept 2-6 optional constructor
   parameters, matching the plan's proposed DI pattern), `CombatTraumaSystem` exposes `Rng` and
   `ApplyMoraleDelta` as public mutable fields assigned after construction. It is also a named
   determinism offender in AGENTS.md Invariant 4 (`public System.Random Rng;`, not yet migrated to
   `ISeededRng`). The plan's blanket instruction to add `IDifficultyProvider` "as a constructor
   parameter" does not fit this class's actual shape and was corrected to use the same public-field
   pattern already in use, with an explicit warning not to conflate this batch with the separate,
   pre-existing `Rng` determinism cleanup.

4. **`ExpeditionSystem` and `WeatherSystem` have fewer existing DI seams than implied.**
   `ExpeditionSystem()` is parameterless today (zero DI hooks); `WeatherSystem(WorldWeatherState
   state = null)` has exactly one. The plan treated all five Step-5 systems as uniformly ready for a
   drop-in optional parameter. `ExpeditionSystem` in particular is a bigger relative change since it
   has no existing optional-parameter convention to extend, and all of its construction call sites
   need to be enumerated before assuming zero-friction integration.

5. **Stale test count: "1941+ existing tests" is outdated.** The actual current count, verified by
   `grep -rc "\[Fact\]" Ashfall.Core.Tests/ --include="*.cs"` → 2116, plus 4 `[Theory]` attributes =
   **2120** total test methods. "1941" traces to a historical entry in `10LOOP_AUDIT_REPORT.md`
   ("Suite: 1941 → 1949 tests (+8)") describing the suite size at some earlier point in the
   project's history, not its current size. Both occurrences in this plan (Step 7's Done-when and
   the Exit Criteria) were corrected to state the actual count and instruct re-verification at
   execution time rather than hardcoding a number that will drift again.

6. **Missing rollback plan.** The original plan had no explicit rollback/kill-switch section. Added
   one: because every system takes `IDifficultyProvider` as an optional parameter defaulting to
   `NullDifficultyProvider`, disabling the feature is a no-code-change operation (don't construct/
   register `AdaptiveDifficultySystem` in the host), and this should be an explicit test
   (`Null_Provider_Has_No_Effect_On_Systems`, already present in Step 7) rather than an implied
   property.

7. **Done-when criteria in Step 5 were vague ("new tests verify each system responds").** Tightened
   to a minimum test count per touched system (9 tests minimum: 2 each for Weather/Disease/Combat/
   Expedition, 1 for Market) and an explicit determinism-test requirement per system, rather than
   leaving "new tests" unquantified.

Everything else in the plan — the `IDifficultyProvider`/`DifficultyDomain`/`PerformanceEvaluator`
design, the tier thresholds, the smoothing/hysteresis approach, the verification commands
(`dotnet build`/`dotnet test`/`godot --headless -- --data-integrity-selftest`, all confirmed to be
real, invocable CLI verbs), and the design principles were verified as sound and left unchanged.
