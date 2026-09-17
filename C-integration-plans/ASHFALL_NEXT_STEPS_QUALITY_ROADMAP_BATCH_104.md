# ASHFALL Quality Roadmap — Batch 104

## Theme: Achievement & Milestone System — Player Progress Recognition

**Priority:** LOW-MEDIUM
**Risk:** **CORRECTED — Low-Medium, not Low.** Step 4 (day-advance wiring) is not purely additive: it requires adding new counters to `HoldfastTradeSession` and `ExpeditionSystem` — existing, tested Core classes — not just reading their state. That is a real (if small) behavior touch to systems this batch doesn't otherwise own, and two of the ten planned stat keys (`items_crafted`, `winters_survived`) have no confirmed source system at all. The condition-evaluator mini-DSL also has more unhandled parsing edge cases than "Low" risk implies (see Step 3 attack notes). Everything else (Steps 1, 2, 5, 6) remains genuinely additive and low-risk.
**Estimated Scope:** **CORRECTED —** ~1,495 lines of Core code + tests (was ~1,100 + ~450 = ~1,550 total in the original combined figure, but the original per-step breakdown undercounted Step 4; see corrected Summary Table at the end of this document for the per-step figures this total is built from).
**Dependencies:** **CORRECTED —** `IClock` (used — confirmed real property is `IClock.Day`, not `CurrentDay`), `InMemoryFlagLedger` (referenced only in the motivation section as a rejected alternative, not an actual dependency of the shipped design), `CaptureState/RestoreState`, JSON data authority, `CatalogIntegrityValidator`. **`ISeededRng` was listed but is never actually used anywhere in this batch** — no step introduces randomness (achievement thresholds are static config values, not randomized), so this dependency is spurious and has been dropped from the list. If a future "hidden achievement surprise reveal" feature (see Future Extensions) needs randomness, that batch should declare `ISeededRng` itself.

---

## Motivation

The game tracks many statistics implicitly — days survived, survivors saved, items crafted, expeditions completed, factions allied, radiation milestones endured — but no structured system recognizes player accomplishments. Today, a player who survives 100 days receives no acknowledgment. A player who saves every survivor from a crisis gets no marker. Progress is invisible beyond the immediate game state.

### What achievements provide:

| Benefit | How it helps ASHFALL specifically |
|---|---|
| **Progress visibility** | Player knows they're advancing even in the slow mid-game grind |
| **Replayability incentives** | "Try to unlock all Radiation Mastery achievements" drives new playthroughs |
| **Implicit tutorials** | "Trade with 3 factions" teaches players the trade system exists |
| **Narrative beats** | "First Winter Survived" marks a meaningful milestone in the story |
| **Social sharing** | Share accomplishments (future: Steam achievements integration) |
| **Design feedback** | Track which achievements unlock rates are too low (content not discovered) |

### Why not just use `InMemoryFlagLedger`?

**CORRECTED — the real `IFlagLedger`/`InMemoryFlagLedger` API, confirmed by direct read of `Assets/Ashfall.Core/Flags/IFlagLedger.cs`, is:**

```csharp
public interface IFlagLedger
{
    bool IsSet(string flagId);
    void Set(string flagId);
    void Clear(string flagId);
    int GetCounter(string counterId);
    void Increment(string counterId, int amount = 1);
    void SetCounter(string counterId, int value);
}
```

There is no `HasFlag`/`SetFlag`/`ClearFlag` anywhere in this codebase — the original draft of this document did not use those names either (it only described flags generically), so no rename was required here. What *was* wrong is the claim that the ledger "lacks progress tracking": `InMemoryFlagLedger` already ships `GetCounter`/`Increment`/`SetCounter` — a working integer counter store with `StringComparer.OrdinalIgnoreCase` keys. That is genuine, if crude, progress tracking (e.g. `Increment("counter_items_crafted")` then compare `GetCounter(...)` against a threshold). The corrected list of what `InMemoryFlagLedger` actually lacks:
- **Percent-of-target progress display** (`GetCounter` returns a raw int; there's no `TargetValue`/`PercentComplete` concept).
- Condition evaluation beyond a bare `>=` on one counter (no expression language at all — not even a simple one).
- Notification integration (fire event on unlock).
- Categorization and display metadata (icon, description, rarity, sort order).
- Prevention of duplicate unlock notifications (nothing stops calling `Set(flagId)` twice and firing two unlock toasts — that has to be built by the caller today).
- Case-normalization risk: `InMemoryFlagLedger` uses `StringComparer.OrdinalIgnoreCase` (documented as a known cross-host drift risk in `AGENTS.md` Invariant 2) — achievement ids inherit this risk if the system is ever built on top of flags instead of its own dictionary.

A dedicated `AchievementSystem` is still justified — the corrected list above is shorter than the original, but "no expression evaluator, no metadata, no notification queue" is enough on its own.

### Design constraints:

- Must live in `Assets/Ashfall.Core/` — zero engine references (`Invariant 1`).
- Achievement definitions in JSON data authority (`Assets/StreamingAssets/Data/`).
- Uses `ISeededRng` if any randomized achievement thresholds exist (determinism).
- Full `CaptureState/RestoreState` for save/load.
- Condition expressions evaluated without `System.Linq.Expressions` compilation (too heavy) — use a simple interpreter.
- Achievement checking must be O(N) where N = incomplete achievements, not O(all achievements * all stats).

---

## Step 1 — Design `AchievementSystem` in Core

### Goal
Define the core `AchievementSystem` class and its supporting types: `AchievementDef` (catalog definition), `AchievementProgress` (per-save tracking), and the unlock lifecycle.

### Implementation

**File:** `Assets/Ashfall.Core/Achievements/AchievementSystem.cs`

```csharp
namespace Ashfall.Core.Achievements
{
    /// <summary>
    /// Tracks player achievement progress and fires unlock notifications.
    /// Evaluated once per day-advance (not per tick — too expensive).
    /// </summary>
    public sealed class AchievementSystem
    {
        private readonly AchievementCatalog _catalog;
        private readonly IConditionEvaluator _evaluator;
        private readonly IClock _clock;
        private readonly Dictionary<string, AchievementProgress> _progress;

        public AchievementSystem(AchievementCatalog catalog, IConditionEvaluator evaluator, IClock clock);

        /// <summary>
        /// Evaluate all incomplete achievements against current game state.
        /// Returns list of newly unlocked achievement IDs this evaluation.
        /// </summary>
        public IReadOnlyList<string> EvaluateAll(IStatProvider stats);

        /// <summary>
        /// Get progress for a specific achievement.
        /// </summary>
        public AchievementProgress GetProgress(string achievementId);

        /// <summary>
        /// Get all achievements with their current status.
        /// </summary>
        public IReadOnlyList<AchievementStatus> GetAll();

        /// <summary>
        /// Check if a specific achievement is unlocked.
        /// </summary>
        public bool IsUnlocked(string achievementId);

        /// <summary>Event fired when an achievement unlocks.</summary>
        public event Action<AchievementUnlockedEvent> OnAchievementUnlocked;

        // Save/Load
        public AchievementSystemState CaptureState();
        public void RestoreState(AchievementSystemState state);
    }
}
```

**File:** `Assets/Ashfall.Core/Achievements/AchievementProgress.cs`

```csharp
namespace Ashfall.Core.Achievements
{
    [Serializable]
    public sealed class AchievementProgress
    {
        public string AchievementId { get; set; }
        public bool IsUnlocked { get; set; }
        public int UnlockedOnDay { get; set; }      // -1 if not unlocked
        public float CurrentValue { get; set; }      // current progress toward threshold
        public float TargetValue { get; set; }       // threshold for unlock
    }

    public sealed class AchievementStatus
    {
        public AchievementDef Definition { get; }
        public AchievementProgress Progress { get; }
        public float PercentComplete => Progress.TargetValue > 0
            ? Math.Clamp(Progress.CurrentValue / Progress.TargetValue, 0f, 1f)
            : 0f;
    }

    public sealed class AchievementUnlockedEvent
    {
        public string AchievementId { get; }
        public string Title { get; }
        public string Description { get; }
        public int UnlockedOnDay { get; }
    }
}
```

**File:** `Assets/Ashfall.Core/Achievements/AchievementSystemState.cs`

```csharp
namespace Ashfall.Core.Achievements
{
    [Serializable]
    public sealed class AchievementSystemState
    {
        public int Version { get; set; } = 1;
        public List<AchievementProgress> Entries { get; set; } = new();
    }
}
```

**Lifecycle:**
1. On game start: `AchievementSystem` loads catalog, initializes empty progress for all defs.
2. On save load: `RestoreState` overwrites progress from save.
3. On day-advance: `EvaluateAll(stats)` checks all incomplete achievements.
4. On unlock: fires `OnAchievementUnlocked` event (UI subscribes for toast notification).
5. On save: `CaptureState` persists all progress.

### Verification
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — compiles.
- No engine references.
- Types are serializable for save/load.

### Done when
- `AchievementSystem`, `AchievementProgress`, `AchievementStatus`, `AchievementUnlockedEvent`, and `AchievementSystemState` compile.
- `CaptureState/RestoreState` pattern implemented.
- Event mechanism defined.

---

## Step 2 — Define Achievement Catalog in JSON Data Authority

### Goal
Create the achievement definition catalog as a JSON file in `Assets/StreamingAssets/Data/`. Each achievement has an ID, display metadata, category, and an unlock condition expression.

### Implementation

**File:** `Assets/StreamingAssets/Data/achievements.json`

```json
{
    "schema_version": 1,
    "achievements": [
        {
            "achievement_id": "achievement_first_dawn",
            "title": "First Dawn",
            "description": "Survive your first full day after the bombs fell.",
            "category": "survival",
            "icon_key": "icon_sunrise",
            "condition": "days_survived >= 1",
            "target_value": 1,
            "stat_key": "days_survived",
            "rarity": "common",
            "sort_order": 1
        },
        {
            "achievement_id": "achievement_week_in_ash",
            "title": "A Week in Ash",
            "description": "Endure seven days in the wasteland.",
            "category": "survival",
            "icon_key": "icon_calendar",
            "condition": "days_survived >= 7",
            "target_value": 7,
            "stat_key": "days_survived",
            "rarity": "common",
            "sort_order": 2
        },
        {
            "achievement_id": "achievement_hundred_days",
            "title": "The Long Haul",
            "description": "One hundred days since the world ended.",
            "category": "survival",
            "icon_key": "icon_hourglass",
            "condition": "days_survived >= 100",
            "target_value": 100,
            "stat_key": "days_survived",
            "rarity": "rare",
            "sort_order": 3
        },
        {
            "achievement_id": "achievement_first_trade",
            "title": "Open for Business",
            "description": "Complete your first trade with another faction.",
            "category": "economy",
            "icon_key": "icon_handshake",
            "condition": "trades_completed >= 1",
            "target_value": 1,
            "stat_key": "trades_completed",
            "rarity": "common",
            "sort_order": 10
        },
        {
            "achievement_id": "achievement_merchant",
            "title": "Merchant of the Wastes",
            "description": "Complete trades with five different factions.",
            "category": "economy",
            "icon_key": "icon_merchant",
            "condition": "unique_trade_partners >= 5",
            "target_value": 5,
            "stat_key": "unique_trade_partners",
            "rarity": "uncommon",
            "sort_order": 11
        },
        {
            "achievement_id": "achievement_first_expedition",
            "title": "Into the Unknown",
            "description": "Send your first expedition into the wasteland.",
            "category": "exploration",
            "icon_key": "icon_compass",
            "condition": "expeditions_launched >= 1",
            "target_value": 1,
            "stat_key": "expeditions_launched",
            "rarity": "common",
            "sort_order": 20
        },
        {
            "achievement_id": "achievement_veteran_explorer",
            "title": "Veteran Explorer",
            "description": "Complete twenty expeditions successfully.",
            "category": "exploration",
            "icon_key": "icon_map_marked",
            "condition": "expeditions_completed >= 20",
            "target_value": 20,
            "stat_key": "expeditions_completed",
            "rarity": "rare",
            "sort_order": 21
        },
        {
            "achievement_id": "achievement_first_craft",
            "title": "Improvisation",
            "description": "Craft your first item from salvaged materials.",
            "category": "crafting",
            "icon_key": "icon_wrench",
            "condition": "items_crafted >= 1",
            "target_value": 1,
            "stat_key": "items_crafted",
            "rarity": "common",
            "sort_order": 30
        },
        {
            "achievement_id": "achievement_water_filter_mastery",
            "title": "Clean Water Initiative",
            "description": "Craft five water filters for the shelter.",
            "category": "crafting",
            "icon_key": "icon_water_drop",
            "condition": "crafted_count_item_water_filter >= 5",
            "target_value": 5,
            "stat_key": "crafted_count_item_water_filter",
            "rarity": "uncommon",
            "sort_order": 31
        },
        {
            "achievement_id": "achievement_full_shelter",
            "title": "Standing Room Only",
            "description": "Have ten survivors living in your shelter simultaneously.",
            "category": "social",
            "icon_key": "icon_people",
            "condition": "current_survivors >= 10",
            "target_value": 10,
            "stat_key": "current_survivors",
            "rarity": "uncommon",
            "sort_order": 40
        },
        {
            "achievement_id": "achievement_radiation_survivor",
            "title": "Walking Hot Zone",
            "description": "Have a survivor accumulate 500 rads and still live.",
            "category": "survival",
            "icon_key": "icon_radiation",
            "condition": "max_survivor_rads >= 500",
            "target_value": 500,
            "stat_key": "max_survivor_rads",
            "rarity": "rare",
            "sort_order": 5
        },
        {
            "achievement_id": "achievement_first_winter",
            "title": "Nuclear Winter",
            "description": "Survive the first winter after the exchange.",
            "category": "survival",
            "icon_key": "icon_snowflake",
            "condition": "winters_survived >= 1",
            "target_value": 1,
            "stat_key": "winters_survived",
            "rarity": "uncommon",
            "sort_order": 4
        }
    ]
}
```

**File:** `Assets/Ashfall.Core/Achievements/AchievementCatalog.cs`

```csharp
namespace Ashfall.Core.Achievements
{
    public sealed class AchievementCatalog
    {
        private readonly Dictionary<string, AchievementDef> _defs;

        public static AchievementCatalog Load(IFileIO fileIO, IJsonSerializer json);
        public AchievementDef GetDef(string achievementId);
        public bool HasDef(string achievementId);
        public IReadOnlyList<AchievementDef> All { get; }
        public IReadOnlyList<AchievementDef> ByCategory(string category);
    }

    [Serializable]
    public sealed class AchievementDef
    {
        public string AchievementId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string IconKey { get; set; }
        public string Condition { get; set; }       // expression string
        public float TargetValue { get; set; }
        public string StatKey { get; set; }         // which stat to track for progress
        public string Rarity { get; set; }          // common, uncommon, rare, legendary
        public int SortOrder { get; set; }
    }
}
```

**Catalog rules:**
- All `achievement_id` values use snake_case with `achievement_` prefix.
- `schema_version` required.
- Categories: `survival`, `economy`, `exploration`, `crafting`, `social`, `medical`, `narrative`.
- Rarity tiers: `common`, `uncommon`, `rare`, `legendary`.

**CORRECTED (was stated as a passive "registered" fact, but this is a mandatory code change, matching the identical error found in sibling Batch 103 for `action_`):**
`achievement_` is **not** currently in `CatalogIntegrityValidator.IdPrefixes` (`Assets/Ashfall.Core/CatalogIntegrityValidator.cs`, confirmed absent by direct read of the full prefix array — it lists `item_`, `loc_`, `location_`, `quest_`, `npc_`, `survivor_`, `faction_`, and roughly 170 more, but no `achievement_`). This step must add the string `"achievement_"` to `IdPrefixes` as part of Step 2, not merely confirm or "register" it as if the entry already exists. Without this addition, `--data-integrity-selftest` will fail once `achievements.json` is registered (TIER-1 check: strings with a known snake_case prefix must resolve) — every `achievement_id` value in the catalog above uses this prefix.

### Verification
- `godot --headless --path . -- --data-integrity-selftest` — new JSON passes (requires the `IdPrefixes` edit above; will fail without it).
- Unit test: load catalog → all 12 defs present, categories correct.
- All `achievement_id` values pass `CatalogIntegrityValidator` prefix check.

### Done when
- `achievements.json` exists with 12+ achievement definitions.
- `AchievementCatalog` loads and indexes by ID and category.
- `"achievement_"` has been added to `CatalogIntegrityValidator.IdPrefixes` (it does not exist there today) in the same commit as `achievements.json`.
- Data integrity selftest passes.

---

## Step 3 — Implement Condition Evaluator (Simple Expression Interpreter)

### Goal
Build a lightweight expression evaluator that can resolve conditions like `"days_survived >= 100"` or `"crafted_count_item_water_filter >= 5"` against a stat provider. No compilation, no `System.Linq.Expressions` — just a simple parser for `{stat_key} {operator} {number}`.

### Implementation

**File:** `Assets/Ashfall.Core/Achievements/IConditionEvaluator.cs`

```csharp
namespace Ashfall.Core.Achievements
{
    public interface IConditionEvaluator
    {
        /// <summary>
        /// Evaluate a condition expression against current stats.
        /// Returns true if the condition is met.
        /// </summary>
        bool Evaluate(string condition, IStatProvider stats);

        /// <summary>
        /// Extract the current progress value from a condition's stat.
        /// Used for progress bar display.
        /// </summary>
        float GetCurrentValue(string statKey, IStatProvider stats);
    }
}
```

**File:** `Assets/Ashfall.Core/Achievements/IStatProvider.cs`

```csharp
namespace Ashfall.Core.Achievements
{
    /// <summary>
    /// Provides named statistics for achievement condition evaluation.
    /// Implemented by the host or a stats aggregator.
    /// </summary>
    public interface IStatProvider
    {
        /// <summary>Get a named statistic value. Returns 0 if unknown.</summary>
        float GetStat(string statKey);

        /// <summary>Check if a stat key is tracked.</summary>
        bool HasStat(string statKey);
    }
}
```

**File:** `Assets/Ashfall.Core/Achievements/SimpleConditionEvaluator.cs`

```csharp
namespace Ashfall.Core.Achievements
{
    /// <summary>
    /// Parses and evaluates simple expressions: "{stat} {op} {value}"
    /// Supported operators: >=, <=, >, <, ==, !=
    /// No boolean combinators (AND/OR) in V1 — one condition per achievement.
    /// </summary>
    public sealed class SimpleConditionEvaluator : IConditionEvaluator
    {
        public bool Evaluate(string condition, IStatProvider stats);
        public float GetCurrentValue(string statKey, IStatProvider stats);

        // Internal parsing
        private static (string statKey, CompareOp op, float threshold) Parse(string condition);
    }

    internal enum CompareOp { GreaterOrEqual, LessOrEqual, Greater, Less, Equal, NotEqual }
}
```

**Grammar (V1 — intentionally simple):**
```
condition := stat_key SPACE operator SPACE number
stat_key  := [a-z_0-9]+
operator  := ">=" | "<=" | ">" | "<" | "==" | "!="
number    := float (invariant culture)
```

**Design decisions:**
- V1 does not support AND/OR combinators. Each achievement has one condition. Complex achievements are split into prerequisites (future enhancement).
- Parsing is done once and cached (condition strings are immutable).
- Unknown stat keys return 0 (safe default — achievement won't unlock prematurely).
- Float comparison uses epsilon (1e-6) for `==` and `!=`.

**ATTACK — is this mini-DSL properly scoped, or is it underestimated complexity?** Verdict: the *scope* (single comparison, no combinators) is correctly kept minimal, but the plan underestimates the parsing edge cases a "simple" grammar still has to handle correctly, and doesn't specify the failure mode for most of them:
- **Operator ambiguity is unaddressed.** `>=` and `<=` share a prefix character with `>` and `<`. A naive split-on-space-then-match-operator-token approach works only if the grammar is tokenized correctly (`>=` must not be misparsed as `>` followed by a stray `=`). The plan's grammar (`stat_key SPACE operator SPACE number`) implies exactly 3 whitespace-delimited tokens, which is fine for `Split(' ')` — but the plan never states what happens with `"days_survived>=100"` (no spaces) or `"days_survived  >=  100"` (multiple spaces). Given every example in `achievements.json` uses exactly one space on each side, a naive `string.Split(' ')` with no `StringSplitOptions.RemoveEmptyEntries` will work for the shipped data but is one typo away from breaking on a future content author's condition string with double spaces — this should be handled explicitly (trim + collapse whitespace, or split on a regex) rather than left as an implicit assumption.
- **Negative thresholds are unaddressed.** `float.TryParse` on the `number` token will happily parse `-5`, but nothing in `achievements.json`'s 12 entries or the grammar spec says whether a negative threshold is a valid design (e.g., is `"some_debt_stat <= -100"` meaningful?). Not a blocker, but the grammar section should say explicitly whether negative numbers are in-scope, since the regex-free `Parse` implementation needs to decide whether `-` is a valid leading character of the `number` token or gets misread as part of an operator.
- **`FormatException` message contract is unspecified.** The Verification step says "malformed condition → throws `FormatException` with helpful message" but never defines what "malformed" covers: wrong operator token (`=>` instead of `>=`), missing number, missing stat_key, or extra tokens (`"a >= b >= c"`). Each is a different failure a content author (writing `achievements.json` by hand) will hit, and a plan this data-driven should enumerate them as explicit test cases rather than one generic "malformed" bucket. **Fix:** the Step 7 test list already has only one `MalformedCondition_ThrowsFormatException` test — this is too coarse; it should be split into at least: unknown operator token, missing number token, non-numeric number token, and extra/missing tokens, so a single passing test can't hide three different unhandled cases.
- **Caching contract is stated but not owned.** "Parse results are cacheable" doesn't say *who* caches — is `SimpleConditionEvaluator` itself expected to memoize by condition string internally, or is caching the caller's (`AchievementSystem`'s) responsibility? Given `Evaluate` and `GetCurrentValue` both re-parse the same `condition` string on every call in the interface as drafted, and `EvaluateAll` is called once per day-advance for every incomplete achievement, an uncached implementation re-parses the same ~12 (eventually more) fixed strings every single day for the life of the save. This is not a performance blocker at 12 achievements, but the plan should say explicitly whether `AchievementSystem` parses once at catalog-load time and stores the parsed `(statKey, op, threshold)` tuple per achievement, or whether `SimpleConditionEvaluator` internally memoizes — currently neither is specified, so two implementers could reasonably build two different (both "correct" per the interface) designs, one 10x more expensive than the other at higher achievement counts.

None of the above require expanding scope (no AND/OR, no nested expressions) — the fix is specifying the parsing edge cases and cache ownership explicitly, not adding grammar features. This keeps the "no `System.Linq.Expressions`, no compilation" constraint intact while closing the ambiguity gaps.

### Verification
- Unit test: `"days_survived >= 100"` with stat=99 → false; stat=100 → true.
- Unit test: `"trades_completed >= 1"` with stat=0 → false; stat=1 → true.
- Unit test: unknown operator token (e.g. `"days_survived => 100"`) → throws `FormatException`.
- Unit test: missing/non-numeric number token (e.g. `"days_survived >="`, `"days_survived >= abc"`) → throws `FormatException`.
- Unit test: extra tokens (e.g. `"a >= b >= c"`) → throws `FormatException`, not silently parses the first two tokens.
- Unit test: double-space or trailing-whitespace condition string parses identically to the single-space form.
- Unit test: unknown stat key → returns 0, condition evaluates safely.

### Done when
- `SimpleConditionEvaluator` correctly parses and evaluates all 6 operators.
- `IStatProvider` interface defines the stat query contract.
- Whitespace variance (double spaces, trailing whitespace) is handled, not just the exact single-space form shown in `achievements.json`.
- Parse-result caching ownership is decided and documented (recommended: `AchievementSystem` parses once per `AchievementDef` at catalog-load time and stores the parsed tuple, since it already owns the def lifecycle — not `SimpleConditionEvaluator` re-parsing per call).
- No engine coupling.

---

## Step 4 — Wire Achievement Checking Into Day-Advance

### Goal
Call `AchievementSystem.EvaluateAll()` once per day-advance. Any newly-unlocked achievements fire the `OnAchievementUnlocked` event for UI notification.

### Implementation

**CORRECTED — the stated integration point is wrong.** `TickSimDay` (`private void TickSimDay(int day)` in `src/Main.cs`) is a **private method on the Godot host's ~6.5k-line `Main` partial class** (confirmed by direct read) — it cannot be called or hooked from `Ashfall.Core`, and "Core-level, or host Main.cs" as stated treats these as interchangeable options when they are not: only the host option is actually reachable, and it couples the achievement wiring to the exact host this project is migrating away from extending (`AGENTS.md` Invariant 5 — no gameplay logic in hosts).

The project already has a purpose-built, engine-agnostic, tested seam for exactly this: `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`, whose own doc comment states it "replaces the historical open-coded `TickSimDay` method." It defines:

```csharp
public interface IDayAdvanceOwner
{
    void CapturePreDaySnapshot(int day);   // idempotent, must not mutate state
    void TickDay(int day, List<DayStateChangeEvent> events);  // deterministic, same day + same state => same events
}
```

Systems register once via `coordinator.Register("achievement_system", owner)`, ordered deterministically by `ownerId` (ordinal sort), and `CampaignDayCoordinator.Advance` ticks every registered owner exactly once per day. This is real, tested infrastructure — `Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs` exists and exercises it with stub owners.

**Corrected integration:** `AchievementSystem` (or a thin adapter around it) should implement `IDayAdvanceOwner` and register with `CampaignDayCoordinator` under a stable id such as `"achievement_system"`, calling `EvaluateAll(stats)` from `TickDay`. This is Core-level, engine-agnostic, and testable without any Godot host — unlike hooking `Main.TickSimDay`, which would require booting the Godot host to test at all.

```csharp
// Core-level, engine-agnostic — no dependency on src/Main.cs:
public sealed class AchievementDayAdvanceOwner : IDayAdvanceOwner
{
    private readonly AchievementSystem _achievements;
    private readonly IStatProvider _stats;

    public AchievementDayAdvanceOwner(AchievementSystem achievements, IStatProvider stats)
    {
        _achievements = achievements;
        _stats = stats;
    }

    public void CapturePreDaySnapshot(int day) { /* no-op: achievements have no pre-day snapshot needs */ }

    public void TickDay(int day, List<DayStateChangeEvent> events)
    {
        var newlyUnlocked = _achievements.EvaluateAll(_stats);
        // newlyUnlocked contains IDs of achievements that just unlocked this day.
        // OnAchievementUnlocked has already fired for each; optionally append
        // entries to `events` here if UI wants them in the daily briefing report.
    }
}

// Host wiring (registration only — no gameplay logic in the host):
coordinator.Register("achievement_system", new AchievementDayAdvanceOwner(achievementSystem, statProvider));
```

**Stat provider implementation:**

**File:** `Assets/Ashfall.Core/Achievements/GameStatProvider.cs`

```csharp
namespace Ashfall.Core.Achievements
{
    /// <summary>
    /// Aggregates stats from all game systems for achievement evaluation.
    /// Implements IStatProvider by querying live system state.
    /// </summary>
    public sealed class GameStatProvider : IStatProvider
    {
        private readonly IClock _clock;
        private readonly Func<int> _survivorCount;
        private readonly Func<int> _tradesCompleted;
        private readonly Func<int> _uniqueTradePartners;
        private readonly Func<int> _expeditionsLaunched;
        private readonly Func<int> _expeditionsCompleted;
        private readonly Func<int> _itemsCrafted;
        private readonly Func<float> _maxSurvivorRads;
        private readonly Func<int> _wintersSurvived;
        // ... additional stat providers

        public float GetStat(string statKey);
        public bool HasStat(string statKey);
    }
}
```

**Stat key registry (known stat keys for V1):**

**CORRECTED — this table is written as if every "Source system" column names a pre-existing counter. None of the counters below exist anywhere in `Ashfall.Core` today** (confirmed by grepping for `tradesCompleted`, `expeditionsCompleted`, `itemsCrafted`, `wintersSurvived`, and equivalents — zero matches across the entire Core tree). Only `days_survived` has a real, already-wired source (`IClock.Day` — note: the property is `Day`, not `CurrentDay`; `IClock` defines `int Day { get; }` in `Assets/Ashfall.Core/Ports.cs`, and the real implementation `SimClock` in `HostDefaults.cs` exposes `Day`). Every other row requires **new counter-tracking code added to an existing system**, which is undisclosed scope not counted in this batch's ~1,100 LOC / 9-10 file estimate:

| Stat key | Source system | Type | Status |
|---|---|---|---|
| `days_survived` | `IClock.Day` (real property, confirmed) | int | Exists — read-only, no new code needed |
| `current_survivors` | Survivor roster count | int | Likely exists as a roster/collection count somewhere in the host or a survivor registry — verify the exact accessor before writing `GameStatProvider`; do not assume a name |
| `trades_completed` | **New counter** — `HoldfastTradeSession` has no trade-count field today (confirmed: only `_held`, `_stock`, `_value` are tracked; every successful `Buy`/`Sell` call updates those but nothing increments a running total) | int | **Must be added to `HoldfastTradeSession`** as part of this step |
| `unique_trade_partners` | **New tracking** — `HoldfastTradeSession` has no per-faction distinct-partner set | int | **Must be added** — requires a `HashSet<string>` of faction ids touched by successful trades |
| `expeditions_launched` | **New counter** — `ExpeditionSystem` has no launch counter; `Start(...)` returning `true` is the only current signal | int | **Must be added to `ExpeditionSystem`** |
| `expeditions_completed` | **New counter** — `ExpeditionSystem.OnExpeditionCompleted` fires per completion but nothing accumulates a total | int | **Must be added**, likely by having `GameStatProvider` subscribe to the existing event and increment its own counter, rather than modifying `ExpeditionSystem` itself |
| `items_crafted` | **Unverified — no crafting system was located by name in this pass.** The plan never names the real crafting class. Before writing this row, locate the actual crafting/production system (search for a `Craft`-named class in `Assets/Ashfall.Core/`) and confirm whether it already counts anything | int | **Blocked on locating the real crafting system** — do not assume one exists with this shape |
| `crafted_count_{item_id}` | Same crafting system as above, per-item | int | Same blocker |
| `max_survivor_rads` | Likely `DoseLedgerSystem.GetCumulative(survivorId)` (confirmed real method) taken as a max across the roster, or `RadiationSystem` if that tracks a comparable per-survivor value — verify which of the two is the intended "rads" source, since this project has two dose-adjacent systems (`DoseLedgerSystem` for the kept-record dose, `RadiationSystem` for the physics) and they are not the same number | float | Needs a design decision on which system is authoritative for this stat, not just a `Func<float>` wired to something unnamed |
| `winters_survived` | **Unverified.** No `wintersSurvived`/`WintersSurvived` symbol exists in Core. `YearOfAshTimelineSystem` tracks `CurrentDay`/`CurrentPhase` (`Phase4_DeepFreeze`, etc., confirmed real) which may be the intended source via phase transitions, but this is a guess, not a confirmed mapping | int | **Must be resolved against the real `YearOfAshTimelineSystem` phase enum before implementation**, or dropped from the V1 catalog if no real source exists yet |

Recommendation: split this step in two — (a) wire `GameStatProvider` against the stats that already exist today (`days_survived` via `IClock.Day`, and `current_survivors` once its real accessor is confirmed), ship those achievements first; (b) treat every "new counter" row as its own small follow-up task per source system, since each one touches a different existing file (`HoldfastTradeSession`, `ExpeditionSystem`, the unlocated crafting system) and mixing "add a counter to system X" with "build the stat aggregator" inside one step violates the project's own "one system per task" rule (`AGENTS.md` → Git Rules / Task Workflow).

**Performance:**
- Only incomplete achievements are evaluated (skip already-unlocked).
- Stat queries are lazy (only fetched if the achievement's stat key matches).
- With 12 achievements and ~10 stats, evaluation is < 1ms.

### Verification
- Integration test: advance days via `CampaignDayCoordinator.Advance` (or a stub `IDayAdvanceOwner` harness in tests — this is fully testable without Godot, unlike the original `Main.TickSimDay` hook) until "First Dawn" condition met → event fires.
- Test: already-unlocked achievement → not re-evaluated, no duplicate event.
- Test: progress updates correctly between evaluations.
- Performance: 100 achievements evaluated in < 5ms.

### Done when
- `AchievementDayAdvanceOwner` (or equivalent) implements `IDayAdvanceOwner` and is registered with `CampaignDayCoordinator` under a stable snake_case owner id — not hooked to the private `Main.TickSimDay`.
- `GameStatProvider` aggregates the stats that have a confirmed real source today (`days_survived` via `IClock.Day`); every stat requiring a new counter (trade counts, expedition counts, crafting counts, winters survived) is either implemented as its own small change to the owning system in this step, or explicitly deferred to a follow-up task — not silently assumed to already exist.
- Newly unlocked achievements fire the event exactly once.
- Progress values update correctly for UI progress bars.

---

## Step 5 — Add Achievement Notification Integration

### Goal
When an achievement unlocks, fire a structured notification event that the UI layer can subscribe to for toast/popup display. Also integrate with the existing notification or event system if one exists.

### Implementation

**File:** `Assets/Ashfall.Core/Achievements/AchievementNotifier.cs`

```csharp
namespace Ashfall.Core.Achievements
{
    /// <summary>
    /// Bridges achievement unlocks to the host notification system.
    /// Subscribes to AchievementSystem.OnAchievementUnlocked and
    /// formats/queues notifications for UI consumption.
    /// </summary>
    public sealed class AchievementNotifier
    {
        private readonly Queue<AchievementNotification> _pending;

        public AchievementNotifier(AchievementSystem achievements);

        /// <summary>Pending notifications for UI to consume and dismiss.</summary>
        public IReadOnlyCollection<AchievementNotification> Pending { get; }

        /// <summary>Dequeue the next notification (FIFO). Returns null if empty.</summary>
        public AchievementNotification Dequeue();

        /// <summary>Clear all pending notifications.</summary>
        public void DismissAll();
    }

    [Serializable]
    public sealed class AchievementNotification
    {
        public string AchievementId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconKey { get; set; }
        public string Rarity { get; set; }
        public int UnlockedOnDay { get; set; }
    }
}
```

**Notification lifecycle:**
1. Achievement unlocks → `OnAchievementUnlocked` fires.
2. `AchievementNotifier` receives event, creates `AchievementNotification`, enqueues.
3. UI polls `Pending` (or subscribes to a UI-layer event) each frame.
4. UI displays toast notification (animated slide-in, auto-dismiss after 5 seconds).
5. Player can also view achievement history in the achievement panel.

**Integration with existing systems:**
- **CORRECTED — `IEventBus` is not a live option for the Godot host.** `AGENTS.md`'s own architecture notes (Event System section) confirm: `IEventBus`/`SimpleEventBus` is "Defined, **underused**", and explicitly "Godot: No bus — direct method calls on host sessions." Presenting "if `IEventBus` is wired" as a coin-flip alongside "if direct calls (Godot pattern)" is misleading — for the actual Godot host in this project, direct calls are the *only* real option today. Do not spend implementation time on an `IEventBus` publish path unless a specific host explicitly wires it; default to: host session subscribes to `AchievementSystem.OnAchievementUnlocked` directly (same pattern as `ExpeditionHostSession.cs:84-85`'s existing subscriptions) and calls into `AchievementNotifier`/UI directly, no string-event bus involved.
- Notification queue is NOT saved — it's transient. If the game saves and reloads mid-notification, the toast is lost (acceptable — player sees it in the achievement panel).

**Notification formatting:**
- Common achievements: simple text toast.
- Rare/legendary: enhanced visual treatment (different color/border — UI responsibility, not Core).
- Rarity information passed to UI for styling decisions.

### Verification
- Test: unlock achievement → notification appears in `Pending`.
- Test: `Dequeue` removes from queue.
- Test: multiple unlocks same day → all queued in order.
- Test: `DismissAll` clears queue.

### Done when
- `AchievementNotifier` queues notifications on unlock.
- Notifications contain all display metadata (title, description, icon, rarity).
- Queue is FIFO and non-persistent (transient).
- No engine coupling in Core notification logic.

---

## Step 6 — Add Achievement Panel UI Contract (Core ViewModel)

### Goal
Define the Core-level view model that the Godot UI panel will bind to. This is the data contract — the actual Godot `.tscn` panel is a host concern, but Core must provide the structured data.

### Implementation

**File:** `Assets/Ashfall.Core/Achievements/AchievementPanelViewModel.cs`

```csharp
namespace Ashfall.Core.Achievements
{
    /// <summary>
    /// View model for the achievement panel UI.
    /// Provides sorted, categorized achievement data for display.
    /// Host UI binds to this; Core owns the data shape.
    /// </summary>
    public sealed class AchievementPanelViewModel
    {
        private readonly AchievementSystem _system;
        private readonly AchievementCatalog _catalog;

        public AchievementPanelViewModel(AchievementSystem system, AchievementCatalog catalog);

        /// <summary>All categories with their achievements.</summary>
        public IReadOnlyList<AchievementCategoryGroup> Categories { get; }

        /// <summary>Total achievements defined.</summary>
        public int TotalCount { get; }

        /// <summary>Total achievements unlocked.</summary>
        public int UnlockedCount { get; }

        /// <summary>Overall completion percentage (0.0 to 1.0).</summary>
        public float CompletionPercent => TotalCount > 0 ? (float)UnlockedCount / TotalCount : 0f;

        /// <summary>Refresh data from current system state.</summary>
        public void Refresh();
    }

    public sealed class AchievementCategoryGroup
    {
        public string CategoryId { get; }           // "survival", "economy", etc.
        public string DisplayName { get; }          // "Survival", "Economy", etc.
        public IReadOnlyList<AchievementDisplayItem> Items { get; }
        public int UnlockedInCategory { get; }
        public int TotalInCategory { get; }
    }

    public sealed class AchievementDisplayItem
    {
        public string AchievementId { get; }
        public string Title { get; }
        public string Description { get; }
        public string IconKey { get; }
        public string Rarity { get; }
        public bool IsUnlocked { get; }
        public int UnlockedOnDay { get; }           // -1 if locked
        public float ProgressPercent { get; }       // 0.0 to 1.0
        public string ProgressText { get; }         // "4 / 5" or "100%" if unlocked
        public int SortOrder { get; }
    }
}
```

**UI binding contract (Godot host will implement):**
```
Achievement Panel (.tscn):
├── Header: "Achievements (7/12 - 58%)"
├── Category Tabs: [Survival] [Economy] [Exploration] [Crafting] [Social]
└── Grid/List of AchievementDisplayItem:
    ├── Icon (locked: greyed out silhouette; unlocked: full color)
    ├── Title + Description
    ├── Progress bar (if not unlocked)
    └── "Unlocked on Day X" (if unlocked)
```

**Sorting:**
- Within each category: unlocked first (by unlock day, newest first), then locked (by sort_order).
- Categories sorted by internal order: survival → economy → exploration → crafting → social → medical → narrative.

**CORRECTED — `medical` and `narrative` categories are declared but unused.** Step 2's design constraints list seven categories (`survival`, `economy`, `exploration`, `crafting`, `social`, `medical`, `narrative`), but the 12 shipped `achievements.json` entries only use five of them — zero achievements exist in `medical` or `narrative`. `AchievementCategoryGroup.TotalInCategory` for those two will always be 0, meaning `Refresh()` must decide whether to render an empty category tab or omit it — this is undefined behavior today. **Fix: either add at least one achievement per declared category so the panel's category tab list matches the catalog (recommended, since the panel's own UI binding contract below shows only 5 tabs — `Survival, Economy, Exploration, Crafting, Social` — silently dropping `medical`/`narrative` from the UI mock while still listing 7 in the design constraints), or explicitly state that `AchievementPanelViewModel.Categories` only includes categories with `TotalInCategory > 0` and drop the two placeholder categories from Step 2 until real content exists for them.**

### Verification
- Unit test: `Refresh()` with mixed unlocked/locked → correct counts and percentages.
- Unit test: progress text formats correctly ("4 / 5", "Unlocked").
- Unit test: categories group correctly.
- Test: empty state (no unlocks) → 0% completion.

### Done when
- `AchievementPanelViewModel` provides complete display data for UI.
- All formatting is culture-invariant.
- No engine references — UI implementation is host responsibility.
- Sorting and grouping logic is tested.

---

## Step 7 — Write Achievement System Tests

### Goal
Comprehensive test coverage for the entire achievement subsystem: catalog loading, condition evaluation, unlock lifecycle, duplicate prevention, progress tracking, save/load round-trips, notification queuing, and view model formatting.

### Implementation

**File:** `Ashfall.Core.Tests/AchievementSystemTests.cs`

```csharp
namespace Ashfall.Core.Tests
{
    public class AchievementConditionEvaluatorTests
    {
        [Fact] public void GreaterOrEqual_Met_ReturnsTrue();
        [Fact] public void GreaterOrEqual_NotMet_ReturnsFalse();
        [Fact] public void LessThan_Met_ReturnsTrue();
        [Fact] public void Equal_Exact_ReturnsTrue();
        [Fact] public void NotEqual_Different_ReturnsTrue();
        [Fact] public void UnknownStat_ReturnsZero_ConditionFalse();
        [Fact] public void MalformedCondition_ThrowsFormatException();
        [Fact] public void FloatThreshold_EpsilonComparison();
        [Fact] public void StatKeyWithUnderscores_ParsedCorrectly();
    }

    public class AchievementSystemCoreTests
    {
        [Fact] public void EvaluateAll_ConditionMet_ReturnsNewlyUnlocked();
        [Fact] public void EvaluateAll_ConditionNotMet_ReturnsEmpty();
        [Fact] public void EvaluateAll_AlreadyUnlocked_NotReEvaluated();
        [Fact] public void EvaluateAll_NoDuplicateUnlockEvents();
        [Fact] public void IsUnlocked_AfterUnlock_ReturnsTrue();
        [Fact] public void GetProgress_TracksCurrentValue();
        [Fact] public void GetProgress_UnlockedShowsFull();
        [Fact] public void MultipleAchievementsUnlockSameDay();
        [Fact] public void OnAchievementUnlocked_EventFires();
        [Fact] public void OnAchievementUnlocked_ContainsCorrectData();
    }

    public class AchievementCatalogTests
    {
        [Fact] public void LoadFromJson_AllDefsPresent();
        [Fact] public void ByCategory_FiltersCorrectly();
        [Fact] public void AllIds_HaveAchievementPrefix();
        [Fact] public void UnknownId_Throws();
        [Fact] public void SchemaVersionPresent();
    }

    public class AchievementSaveLoadTests
    {
        [Fact] public void CaptureState_RestoreState_RoundTrip();
        [Fact] public void RestoreState_PreservesUnlockStatus();
        [Fact] public void RestoreState_PreservesProgress();
        [Fact] public void RestoreState_PreservesUnlockDay();
        [Fact] public void EmptyState_LoadsWithoutError();
        [Fact] public void NewAchievementAdded_OldSaveStillLoads();
    }

    public class AchievementNotifierTests
    {
        [Fact] public void UnlockQueuesNotification();
        [Fact] public void DequeueFIFO();
        [Fact] public void MultipleUnlocks_AllQueued();
        [Fact] public void DismissAll_ClearsQueue();
        [Fact] public void NotificationContainsMetadata();
    }

    public class AchievementPanelViewModelTests
    {
        [Fact] public void Refresh_CorrectTotalCount();
        [Fact] public void Refresh_CorrectUnlockedCount();
        [Fact] public void CompletionPercent_Calculated();
        [Fact] public void Categories_GroupedCorrectly();
        [Fact] public void ProgressText_FormatsCorrectly();
        [Fact] public void SortOrder_UnlockedFirst_ThenBySortOrder();
    }
}
```

**Test infrastructure:**
- `FakeStatProvider` — returns configurable stat values for each test.
- `FakeClock` — controls day counter.
- In-memory JSON for catalog loading (no file I/O in unit tests).
- Event capture via subscriber counting / recording.

**Edge case coverage:**
- Achievement with `target_value: 0` → immediately unlocks (or never — design decision: skip if target is 0).
- All achievements already unlocked → `EvaluateAll` returns empty instantly.
- Stat decreases below threshold after unlock → stays unlocked (achievements are permanent).
- Save from older version missing new achievements → new achievements start at 0 progress.
- Very large stat values (int overflow protection).

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Must compile
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # All pass
dotnet build Ashfall.csproj                                  # Godot host clean
godot --headless --path . -- --data-integrity-selftest       # 0 errors (achievements.json valid)
godot --headless --path . -- --bridge-selftest               # Exits 0
```

### Done when
- All 35+ tests pass.
- Coverage: condition parsing, evaluation, unlock lifecycle, save/load, notification, view model.
- Tests are deterministic and engine-independent.
- No regressions in existing test suite.
- All new code is in `Ashfall.Core` with zero engine references.

---

## Summary Table

| Step | Deliverable | Files | Key Risk | Estimated LOC |
|------|------------|-------|----------|---------------|
| 1 | `AchievementSystem`, progress types, state DTO | `Achievements/AchievementSystem.cs`, `AchievementProgress.cs`, `AchievementSystemState.cs` | None — types only | ~180 |
| 2 | `achievements.json` + `AchievementCatalog` | `StreamingAssets/Data/achievements.json`, `Achievements/AchievementCatalog.cs` | Data integrity, category completeness | ~200 |
| 3 | `SimpleConditionEvaluator` + `IStatProvider` | `Achievements/IConditionEvaluator.cs`, `SimpleConditionEvaluator.cs`, `IStatProvider.cs` | Expression parsing edge cases | ~150 |
| 4 | Day-advance wiring + `GameStatProvider` | `Achievements/GameStatProvider.cs`, host integration | Stat collection from live systems | ~120 |
| 5 | `AchievementNotifier` | `Achievements/AchievementNotifier.cs` | Queue correctness, event lifecycle | ~80 |
| 6 | `AchievementPanelViewModel` | `Achievements/AchievementPanelViewModel.cs` | Sorting/grouping logic | ~150 |
| 7 | Test suite (35+ tests) | `Ashfall.Core.Tests/AchievementSystemTests.cs` | Covering all evaluation paths | ~450 |

**Total estimated:** ~1,330 lines across 9-10 files.

---

## Summary Table

**CORRECTED — this section was missing entirely from the original draft (present in sibling Batch 103, absent here despite the same template).** Added for consistency and because the header's `Estimated Scope`/`Dependencies` fields need a per-step breakdown to be actionable:

| Step | Deliverable | Files | Key Risk | Estimated LOC |
|------|------------|-------|----------|---------------|
| 1 | `AchievementSystem`, progress types, state DTO | `Achievements/AchievementSystem.cs`, `AchievementProgress.cs`, `AchievementSystemState.cs` | None — types only | ~180 |
| 2 | `achievements.json` + `AchievementCatalog` + `"achievement_"` added to `CatalogIntegrityValidator.IdPrefixes` | `StreamingAssets/Data/achievements.json`, `Achievements/AchievementCatalog.cs`, `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | Data integrity; forgetting the `IdPrefixes` edit fails `--data-integrity-selftest`; 2 of 7 declared categories (`medical`, `narrative`) have zero content | ~205 |
| 3 | `SimpleConditionEvaluator` + `IStatProvider` | `Achievements/IConditionEvaluator.cs`, `SimpleConditionEvaluator.cs`, `IStatProvider.cs` | Expression parsing edge cases (operator ambiguity, whitespace variance, malformed-condition failure modes) — see Step 3 attack notes | ~160 |
| 4 | Day-advance wiring via `IDayAdvanceOwner` + `GameStatProvider` + **new counters in `HoldfastTradeSession`, `ExpeditionSystem`, and an as-yet-unlocated crafting system** | `Achievements/GameStatProvider.cs`, `Achievements/AchievementDayAdvanceOwner.cs`, plus edits to `HoldfastTradeSession.cs` (trade count + partner set) and `ExpeditionSystem.cs` (launch/completion counters), plus a located-or-deferred crafting system | Undisclosed scope: 6 of 10 stat keys require new counters in other systems, not just an aggregator reading existing state; `winters_survived` and `items_crafted` have no confirmed source system at all | ~250 (was ~120 in the original draft — underestimated because the new counters were not counted) |
| 5 | `AchievementNotifier` | `Achievements/AchievementNotifier.cs` | Queue correctness, event lifecycle; do not build an `IEventBus` publish path — Godot host uses direct calls only | ~80 |
| 6 | `AchievementPanelViewModel` | `Achievements/AchievementPanelViewModel.cs` | Sorting/grouping logic; decide now whether empty categories (`medical`, `narrative`) render or are hidden | ~150 |
| 7 | Test suite (35+ named tests, enumerated in Step 7) | `Ashfall.Core.Tests/AchievementSystemTests.cs` | Covering all evaluation paths, including the 4 new malformed-condition sub-cases added by the Step 3 correction | ~470 |

**Total estimated:** ~1,495 lines across 10-13 files (higher than the original ~1,330/9-10 estimate once Step 4's real new-counter work and Step 2's `IdPrefixes` edit are counted).

---

## Verification Checklist (per project rules)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Must compile cleanly
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # All tests pass (including new AchievementSystemTests)
3. dotnet build Ashfall.csproj                                  # Godot host: 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest       # Catalog integrity: 0 errors (achievements.json validated)
5. godot --headless --path . -- --bridge-selftest               # Exits 0
```

---

## Exit Criteria

This batch is complete when:
- [ ] `AchievementSystem` tracks progress and fires unlock events.
- [ ] `achievements.json` exists with 12+ definitions; every category declared in Step 2's design constraints either has at least one achievement or has been removed from the declared category list (no silent empty categories).
- [ ] `SimpleConditionEvaluator` parses and evaluates all 6 comparison operators, including the whitespace-variance and multi-failure-mode cases added by the Step 3 correction (not just the single generic "malformed condition" case originally specified).
- [ ] `GameStatProvider` aggregates `days_survived` (via the real `IClock.Day` property) and any other stat with a confirmed real source; every stat requiring a new counter (`trades_completed`, `unique_trade_partners`, `expeditions_launched`, `expeditions_completed`, `items_crafted`, `crafted_count_{item_id}`, `winters_survived`) is either implemented in this batch against its owning system, or explicitly moved to `achievements.json` v2 / a follow-up batch with the affected achievements removed from v1's 12.
- [ ] `AchievementNotifier` queues notifications on unlock (FIFO, transient), wired via direct event subscription (not `IEventBus`, which is unused by the Godot host per `AGENTS.md`).
- [ ] `AchievementPanelViewModel` provides complete categorized display data.
- [ ] `CaptureState/RestoreState` round-trip preserves all achievement progress.
- [ ] No duplicate unlock events (achievements are permanent once earned).
- [ ] Forward-compatible save loading (new achievements added → old saves still load).
- [ ] Every named test enumerated in Step 7 (35 `[Fact]`s, plus the additional malformed-condition sub-cases from the Step 3 correction) exists and passes — "35+" is a floor on top of the named list, not a substitute for it.
- [ ] `"achievement_"` has been added to `CatalogIntegrityValidator.IdPrefixes` in the same commit as `achievements.json` (confirmed absent today — this is a required code change, not a passive registration check).
- [ ] Day-advance wiring uses `IDayAdvanceOwner`/`CampaignDayCoordinator.Register(...)`, not the private `Main.TickSimDay`.
- [ ] Zero engine coupling in all new code.
- [ ] All 5 verification steps pass.

---

## Future Extensions (out of scope for this batch)

- **Hidden achievements** — not shown in panel until unlocked (surprise milestones).
- **Achievement prerequisites** — "Unlock X before Y becomes trackable."
- **Boolean combinators** — `AND`/`OR` in condition expressions for complex multi-stat achievements.
- **Meta-achievements** — "Unlock all achievements in the Survival category."
- **Time-limited achievements** — "Reach 50 days before day 60" (speedrun challenges).
- **Steam/platform integration** — Sync unlocks with platform achievement APIs.
- **Achievement rewards** — Unlocking grants a bonus item, trait, or cosmetic.
- **Statistics panel** — Detailed stat tracking UI (total items crafted, distance traveled, etc.) as prerequisite for richer achievements.
- **Leaderboard seeds** — Achievements tied to specific world seeds for competitive runs.


---

## Review Notes (Corrected)

This was a first-pass adversarial review — the document carried no prior "CORRECTED" annotations before this pass, unlike sibling Batch 103.

### `IFlagLedger` API claim — the specific error the review brief asked to hunt for

The document never actually called `HasFlag`/`SetFlag`/`ClearFlag` anywhere — it only described the flag ledger generically ("tracks boolean flags") without naming its methods, so there was no wrong-method-name bug to fix in that narrow sense. **What was wrong instead:** the motivation section overstated what `InMemoryFlagLedger` lacks. Confirmed by direct read of `Assets/Ashfall.Core/Flags/IFlagLedger.cs`, the real API is:

```csharp
public interface IFlagLedger
{
    bool IsSet(string flagId);
    void Set(string flagId);
    void Clear(string flagId);
    int GetCounter(string counterId);
    void Increment(string counterId, int amount = 1);
    void SetCounter(string counterId, int value);
}
```

The plan claimed the ledger "lacks: Progress tracking (40% toward 'Craft 50 items')" — but `GetCounter`/`Increment`/`SetCounter` already provide a working integer counter store today. Fixed: the motivation section now lists the corrected, narrower set of real gaps (percent-of-target display, expression evaluation, notification dedup, display metadata) instead of the overstated list.

### Factual errors found and fixed

1. **`IClock.CurrentDay` does not exist — the real property is `IClock.Day`.** Confirmed by direct read of `Assets/Ashfall.Core/Ports.cs` (`interface IClock { int Day { get; } ... }`) and the real implementation `SimClock` in `HostDefaults.cs`. Fixed everywhere this appeared (header dependencies, stat key registry).
2. **The Step 4 integration point (`TickSimDay`) is a private method on the Godot host's `Main` class, not a Core-level or dual-option seam.** The plan phrased it as "Core-level, or host `Main.cs`" as if these were interchangeable; only the host option is reachable, and `TickSimDay` is `private`, so nothing outside `Main` can call it. The project has a real, tested, engine-agnostic replacement seam for exactly this purpose — `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs` + `IDayAdvanceOwner`, whose own doc comment states it replaces "the historical open-coded `TickSimDay` method." Fixed: Step 4 now wires `AchievementSystem` through `IDayAdvanceOwner`/`CampaignDayCoordinator.Register(...)`, with a corrected code sample and rationale.
3. **The stat key registry (Step 4) presented 8 of 10 "Source system" columns as if pre-existing counters already exist.** Confirmed by grep across all of `Ashfall.Core` that `tradesCompleted`, `expeditionsCompleted`, `itemsCrafted`, `wintersSurvived`, and their equivalents do not exist anywhere. This is not a minor naming issue — it means Step 4 silently required building new counter-tracking in `HoldfastTradeSession` and `ExpeditionSystem` (both real, existing, tested classes) and locating or building an entirely unnamed crafting system, none of which was counted in the original LOC estimate or flagged as touching other systems. Fixed: the table now marks each row's real status (exists / new counter needed / unverified-blocked) and the step's Done-when and risk level were both updated to reflect this.
4. **`IEventBus` was presented as a coin-flip integration option for notifications, but the Godot host doesn't use it.** `AGENTS.md`'s own Event System section states `IEventBus`/`SimpleEventBus` is "Defined, **underused**" and "Godot: No bus — direct method calls on host sessions." Fixed: Step 5 now states direct event subscription is the only real path for this project's Godot host, following the same pattern as the confirmed real subscribers in `src/Host/ExpeditionHostSession.cs:84-85`.
5. **`achievement_` prefix gap in `CatalogIntegrityValidator` was stated as a passive "registered" fact rather than a required code change.** Confirmed by direct read of the full ~180-entry `IdPrefixes` array: neither `achievement_` nor `action_` (the sibling Batch 103's prefix) appear anywhere in it. Fixed: Step 2's catalog rules and Done-when now state explicitly that adding `"achievement_"` to `IdPrefixes` is a mandatory part of this step, matching the identical fix already applied in Batch 103 for `action_`.
6. **Category/content mismatch:** Step 2's design constraints declare 7 categories (`survival`, `economy`, `exploration`, `crafting`, `social`, `medical`, `narrative`); the shipped 12-achievement catalog only populates 5 of them, and Step 6's own UI mockup shows only 5 tabs. Fixed: flagged as an undefined-behavior gap (empty category rendering) with two concrete resolution options instead of leaving it as a silent inconsistency between two sections of the same document.

### Scope creep / underspecification found and fixed

7. **Mini-DSL condition evaluator (Step 3) — the review brief specifically asked whether this is "properly scoped as real complexity."** Verdict: the chosen *scope* (single comparison, no AND/OR, no compiled expressions) is correctly minimal and appropriately sized — this was not over-engineered. What was underspecified: operator-token ambiguity (`>=` vs `>`), whitespace-variance handling, negative-number support, and — most concretely — the single `MalformedCondition_ThrowsFormatException` test collapsing at least four distinct failure modes (unknown operator, missing number, non-numeric number, extra tokens) into one test that can pass while three of the four cases remain unhandled. Also unspecified: who owns parse-result caching (the interface as drafted re-parses on every `Evaluate`/`GetCurrentValue` call unless a specific class is named as the memoizer). Fixed: added explicit parsing edge-case tests, split the malformed-condition test into four, and assigned caching ownership to `AchievementSystem` at catalog-load time.
8. **No Summary Table existed in this document at all**, despite the sibling Batch 103 having one and this document's own header format (Priority/Risk/Estimated Scope/Dependencies) implying the same level of per-step accounting. Fixed: added a full Summary Table with corrected LOC estimates reflecting the Step 4 new-counter scope.
9. **Vague Done-when ("35+ tests pass covering all subsystem layers").** Same issue as Batch 103's "23+" — a floor with no enumeration requirement lets an implementer under-deliver relative to the plan's own named test list. Fixed: Exit Criteria now requires every named test in Step 7 (plus the new malformed-condition sub-cases from the Step 3 fix) to exist and pass, with "35+" as a floor on top, not a substitute.
10. **`ISeededRng` listed as a dependency in the header but never used anywhere in the seven steps.** No achievement threshold, no notification behavior, and no evaluator logic in this plan involves randomness. Fixed: removed from the dependency list with a note that a future randomized feature (e.g., hidden-achievement reveal order) should declare it itself rather than inheriting an unused dependency from this batch.

### What was already correct and did not need fixing

- The core design shape (`AchievementSystem` + `AchievementCatalog` + `IConditionEvaluator` + `AchievementNotifier` + `AchievementPanelViewModel`, each with a clean single responsibility) is sound and matches the project's existing system-decomposition style seen in `DoseLedgerSystem`/`ChemicalDependencySystem`.
- `CaptureState/RestoreState` DTO shapes (Step 1) follow the house pattern correctly (versioned state, plain serializable DTOs, no engine types).
- The forward-compatible save-loading design decision ("new achievements added → old saves still load, start at 0 progress") is a reasonable and correctly-identified edge case.
- The five canonical verification commands match the project's actual `dotnet`/`godot --headless` pipeline; no Unity commands appear anywhere in this batch.
