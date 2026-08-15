# ASHFALL Repository Deep Review Report
**Date:** 2026-08-16 | **Branch:** `cursor/phase11-expansion-ui-integration` | **Commits:** 1002
**Method:** 6 parallel deep-review agents + direct investigation. All findings verified against source.

---

## Executive Summary

Ashfall is a post-nuclear 2D survival-management game with an ambitious dual-engine architecture: an engine-agnostic C# core (`Assets/Ashfall.Core/`, 234 files) shared between a Unity 6 LTS host (`Assets/_Game/`, 1337 files) and a Godot 4.7+ host (`src/`, 84 files). The project is 1002 commits in, with 280 JSON data files and 143 test files.

**Overall health: Strong core foundations, significant architectural debt in the hosts.** The engine-agnostic core is clean — zero engine coupling, proper ports, versioned save codecs, sophisticated data validation. But critical issues exist: the main Unity save system uses `JsonUtility` and **cannot cross hosts** (violating the core cross-host invariant), 28 catalog loaders also use `JsonUtility` blocking Godot data loading, `System.Random` in 3 core systems breaks determinism, 56 narrative JSON files are untracked in git (missing on fresh clone), 588 "demoted ghost" markers indicate massive dead code, and the Unity host contains thousands of lines of game logic that bypass the core entirely.

---

## Build Status

| Target | Result | Details |
|--------|--------|---------|
| Godot (`Ashfall.csproj`) | ✅ **Clean** | 0 errors, 0 warnings |
| Tests (`Ashfall.Core.Tests.csproj`) | ❌ **10 errors** | `HoldfastTradeSessionTests.cs` — stale API |
| Tests (cached DLL) | ✅ 1514 pass | Previous build: 1514 passed, 0 failed |
| Compiler warnings (tests) | ⚠️ 124 | Mostly nullable reference types |

---

## CRITICAL Findings (8)

### C1. Main Game Save Cannot Cross Hosts — `JsonUtility` Violation ⭐

**The AGENTS.md invariant states:** *"A save written by one host MUST load in the other."*

**Reality:** Unity's main `SaveSystem` (967+ lines across 10 partial files) uses `JsonUtility.ToJson`/`FromJson` — a Unity-only API. There are 10+ call sites in `Assets/_Game/Core/`. The Unity host has **no `IJsonSerializer` adapter**. Only the newer per-subsystem expansion saves (Verdict, Holdfast, Dose, etc.) use the portable `IJsonSerializer` + `SaveChecksum` pattern.

**Impact:** The main game save (20+ subsystems: weather, needs, radiation, shelter, survivors, inventory, etc.) **cannot load in Godot**. The cross-host save invariant is violated for the primary save path.

**Fix:** Create a Unity `IJsonSerializer` adapter using `System.Text.Json` and migrate `SaveSystem` to use it.

---

### C2. `System.Random` in 3 Core Systems Breaks Determinism

The AGENTS.md rule: *"Same seed => same simulation in both engines."* The core provides `SeededRng` (xorshift64*) for this. Three systems bypass it:

| File | Line | Usage |
|------|------|-------|
| `FinalWishSystem.cs` | 66 | `public System.Random Rng;` — prognosis day calculation |
| `CombatTraumaSystem.cs` | 53 | `public System.Random Rng;` — false-alarm rolls |
| `WeatherSystem.cs` | 144 | `new Random(unchecked(_seed * 397 + _state.rollCount))` — weather rolls |

Additionally, `ProceduralItemInstance.cs:36` uses `Guid.NewGuid()` for instance IDs — non-deterministic.

**Impact:** Saves from Unity and Godot will diverge for these systems. `WeatherSystem` is the most dangerous because it ticks automatically and silently accumulates divergence.

**Fix:** Replace `System.Random` with `ISeededRng` in all three. Replace `Guid.NewGuid()` with seeded generation.

---

### C3. Test Project Doesn't Compile — `HoldfastTradeSessionTests.cs`

**File:** `Ashfall.Core.Tests/HoldfastTradeSessionTests.cs`
**Errors:** 10 (CS0120, CS1061)

The test calls APIs that no longer exist:
- `HoldfastCatalogLoader.Load(path)` — was static, now requires instance with `(IFileIO, IJsonSerializer, ILog)` constructor
- `session.PurchaseItem(...)` / `session.SellItem(...)` — removed from `HoldfastTradeSession`

**Impact:** The entire test project fails to compile. The 1514 passing tests are from a stale cached DLL.

---

### C4. 56 Narrative JSON Files Untracked in Git

`git ls-files --others --exclude-standard` confirms **56 JSON files** in `Assets/StreamingAssets/Data/narrative/` exist on disk but are NOT tracked in git. A fresh clone would be missing these files, causing runtime data integrity failures.

---

### C5. 28 Runtime Catalog Loaders Use `JsonUtility.FromJson` — Blocks Godot Data Loading

Beyond `SaveSystem`, **28 expansion catalog loaders** in `Assets/_Game/Data/` use `JsonUtility.FromJson` with the array-wrap hack (`JsonUtility.FromJson<Container>("{\"entries\":" + json + "}")`). These run at game start and cache results in static fields. This couples ALL expansion data loading to Unity's serializer. The Godot host cannot load the same JSON catalogs through these paths.

Representative files: `HoldfastItemsCatalogLoader.cs`, `CrossingItemsCatalogLoader.cs`, `LoreDiscoveryIndex.cs`, `EchoCatalogLoader.cs`, + 24 more.

---

### C6. Massive Logic Leakage in Unity Host

The architecture mandates: *"thin MonoBehaviours; no gameplay rules in either host."* Reality:

| File | Lines | Methods | Core Refs | Issue |
|------|-------|---------|-----------|-------|
| `PersonalQuestSystem.cs` | **4936** | 404 | **0** | Entire quest engine in Unity |
| `DynamicEconomySystem.cs` | 1797 | 76 | 6 | Economy logic with minimal core use |
| `SurvivorWorkShiftSystem.cs` | 1291 | 38 | **0** | Work shift logic |
| `MedicalSystem.cs` | 1287 | 45 | **0** | Medical triage pipeline |

`PersonalQuestSystem.cs` is described in its own comments as *"Plain C#, save/load safe"* — the author knew it should be engine-agnostic but placed it in `_Game` with `using UnityEngine`.

---

### C7. Two Parallel Event Buses — Core Bus Unused

- **Core bus** (`IEventBus`/`SimpleEventBus`): string-based, constructor-injected — but **no core simulation systems publish through it**
- **Unity bus** (`EventBus` static class): type-safe generics, allocation-free — the *real* decoupler, but Unity-only
- **Godot**: no event bus integration at all — uses direct method calls

The AGENTS.md rule says *"Every public system raises C# events on state change"* but the mechanism is the Unity static `EventBus`, not the core `IEventBus`.

---

### C8. Utility AI Forked — Unity Uses Defective Version

- **Unity** (`Assets/_Game/AI/UtilityAI.cs`): uses `UnityEngine.Mathf`, `System.Random`, has a latent defect where vetoed actions (score=0) CAN win because `bestScore` starts at -1
- **Core** (`Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs`): uses `ISeededRng` (deterministic), fixes the veto defect (`if (score > 0f && score > bestScore)`), data-driven

Unity has not adopted the core version. This violates the one-source-of-truth rule.

---

## HIGH Findings (12)

### H1. 124 Files Contain 588 "DEMOTE ghost" Markers — Massive Dead Code

124 files across `_Game/` contain 588 total "demoted (ghost)" markers. These are systems that were constructed, save-wired, then demoted to dormant no-ops. Their classes still compile and consume build time but do nothing. `BootActionFamily()` constructs nothing — it just logs 37 "demoted" messages.

### H2. Phase 0 Expansion Callbacks Are No-Op Placeholders

`GameBootstrap.Phase0Expansion.cs` has multiple callbacks documented as *"Placeholder -- wired in Phase 11"*:
- `PhantomMemorySystem.SetWorkEfficiencyMultiplier`
- `SomaticFlashbackSystem.SetWorkEfficiencyPenalty`
- `ChemicalDependencySystem.ApplyCraftingPenalty`
- `FinalWishSystem.ApplyPermanentShelterMoraleBuff`
- `TradeSpecialtySystem.FireNarrativeEvent`
- `RespiratoryDegenerationSystem.ApplyStaminaPenalty`

These systems are constructed, registered for save/load, and ticked every frame — but their key gameplay effects are no-ops.

### H3. Some ISaveable Adapters Have Empty CaptureState/RestoreState

`LocationEvolutionSaveable`, `WildlifeSaveable`, `LandmarkSaveable` all return `null` from `CaptureState()` and do nothing in `RestoreState()`. Any runtime state they accumulate is silently lost on save/load.

### H4. `HoldfastRuntimeSession` Duplicates Core Survival Mechanics

`src/Host/HoldfastRuntimeSession.cs` defines its own hunger/thirst/radiation/health simulation with hardcoded thresholds — while `NeedsSystem` and `RadiationSystem` already exist in the core and are hosted via `SurvivorsHostSession`. Two sources of truth for survival rules.

### H5. Duplicate `WornGear` Class in Two Namespaces

- `Ashfall.Core.Inventory.WornGear` (bare DTO)
- `Ashfall.Core.Radiation.WornGear` (has methods: `DurabilityFraction()`, `EffectiveProtection()`, `Degrade()`)

Same fields, different types. Any code passing between Inventory and Radiation requires manual conversion.

### H6. Duplicate `SimClock` with Incompatible Interfaces

- `IClock`/`SimClock` in `HostDefaults.cs` — day-based (`int Day`, `AdvanceDays`)
- `ISimClock`/`SimClock` in `Clock/ISimClock.cs` — tick-based (`long CurrentTick`, `AdvanceTicks`, `AdvanceHours`)

Different namespaces, different granularities, both actively used.

### H7. Silent Exception Swallowing — 13 Bare `catch { }` Blocks

`YearOfAshCatalogLoader.cs` has 7 bare catches. `VerdictCatalogLoader.cs` has 3. `VerdictSave.cs`, `VerdictNpcSystem.cs`, `DeepLoreLocationCatalogLoader.cs` each have 1. All swallow exceptions (including OOM, StackOverflow) with no logging.

### H8. 140 MB Archive + 311 MB AI Assets + 114 MB PNGs Tracked in Git

Total binary assets tracked: **~565 MB**. Includes `unity-assets-archive-2026-08-14.tar.gz` (140 MB), `generated_AIassets/` (311 MB), and large PNG art (114 MB). No Git LFS configured.

### H9. Target Framework Inconsistency

| Project | Target |
|---------|--------|
| Godot host (`Ashfall.csproj`) | `net8.0` |
| Core (`Ashfall.Core.csproj`) | `netstandard2.1` |
| Tests (`Ashfall.Core.Tests.csproj`) | `net9.0` + `RollForward: LatestMajor` |

Tests require .NET 9 SDK. A developer with only .NET 8 (matching Godot) cannot run tests.

### H10. Real Country Name in Data — Rule Violation

`world_history.json:15` contains: *"Microchip fabrication halted across three continents when **China** suspended rare earth mineral exports."* AGENTS.md states: *"No real countries/wars/people."*

### H11. Inconsistent JSON Property Naming

JSON property names mix `camelCase` (`displayName`, `basePrice`, `minDay`) and `snake_case` (`display_name`, `target_location_id`, `min_day`) across files. The deserialization layer must handle both conventions.

### H12. GameBootstrap Is a 1,225-Line God Object Across 82 Partial Files

`GameBootstrap` declares hundreds of system properties, constructs all systems, wires all event handlers, and drives the game loop. The `RegisterExpansionSaveables()` method alone has 636 lines. While the partial-class split provides organizational relief, a single MonoBehaviour owns references to every system in the game.

---

## MEDIUM Findings (10)

| # | Finding | Details |
|---|---------|---------|
| M1 | 124 compiler warnings in tests | CS8600 ×144, CS8602 ×12, CS8618 ×28 (nullable refs) |
| M2 | 121 ScriptableObject definitions | Risk of dual data authority vs JSON |
| M3 | Godot migration at ~4% | 10K LOC Godot vs 232K LOC Unity |
| M4 | Port adapters incomplete in Unity | No `IFileIO`, `IJsonSerializer`, or `IClock` adapters |
| M5 | Godot compiles all 1337 Unity files | Via shim — massive dead weight in Godot build |
| M6 | 5 Godot save stores have no checksum | `ExpeditionSaveStore`, `MedicalSaveStore`, `NarrativeSaveStore`, `WorldSaveStore`, `JournalSaveStore` |
| M7 | `JournalSaveStore` bypasses core serializer | Uses `System.Text.Json` directly |
| M8 | `Main.cs` (Godot) is ~3000 lines | Monolithic entry point |
| M9 | NeedsSystem & RadiationSystem lack save/load round-trip tests | Most fundamental survival systems untested for serialization |
| M10 | JournalSystem has zero tests | 6 files, core narrative progression, no coverage |

---

## LOW Findings (6)

| # | Finding |
|---|---------|
| L1 | Zero TODO/FIXME/HACK comments in entire codebase |
| L2 | Git hygiene good — `_verify_`, `.mimocode/`, `Builds/` properly gitignored |
| L3 | No engine coupling in Core (zero `using UnityEngine/Godot/Editor`) |
| L4 | `InMemoryFlagLedger` uses `OrdinalIgnoreCase` — subtle determinism risk |
| L5 | Only 35 of ~280 JSON files have `schema_version` field |
| L6 | `scripts/` directory included in .csproj but empty |

---

## What's Working Well

### ✅ Engine-Agnostic Core
- 234 files, zero engine references, clean port interfaces
- `IJsonSerializer`, `IFileIO`, `ILog`, `IClock`, `ISeededRng` — well-designed ports
- Systems are self-contained with serializable state

### ✅ Save/Load Architecture (per-subsystem)
- Versioned save codecs (V1 → V2 → V3) with migration
- `SaveChecksum` — reflection-based, serializer-independent integrity hash
- Normalizes null-vs-empty and float formatting for cross-host compatibility
- Per-system save DTOs with proper codecs

### ✅ Data Integrity
- `CatalogIntegrityValidator` — 5-tier validation (registry, prefix, reference, range, uniqueness)
- 56-catalog cross-reference gate wired as Godot CLI self-test
- Mechanically enforces "never invent an id" rule

### ✅ Test Suite (when it compiles)
- ~1,491 tests across 143 files
- Exceptional save/load integrity testing (checksum, tamper rejection, multi-version migration, aliasing regression)
- Determinism testing (same-seed-same-output)
- Catalog tests as data integrity gates (not just "does it load")
- Headless demo smoke tests

### ✅ Godot Host Quality
- All Nodes properly thin (presentation only)
- Consistent design system tokens (`AshfallUiHelpers` + `Theme`)
- Proper port/adapter implementations (`GodotLog`, `CoreSeededRng`, etc.)
- Invariant culture enforced everywhere
- `BridgeSelfTest` validates shim honesty

### ✅ Bridge Shim Honesty
- `BridgeGap.Semantic()` throws on logic-affecting gaps
- `BridgeGap.Cosmetic()` logs visual-only gaps
- `BridgeSelfTest` prevents silent regression of gap classifications

### ✅ Expansion Architecture
- `ExpansionMasterSession` orchestrates 4 core expansions
- 4+ standalone expansions with own host wiring
- Each has catalog, save state, headless demo

---

## Recommended Priority Actions

### Immediate (blocks development)
1. **Track the 56 untracked narrative JSON files** — `git add Assets/StreamingAssets/Data/narrative/`
2. **Fix or delete `HoldfastTradeSessionTests.cs`** — test project must compile
3. **Replace "China" in `world_history.json`** with fictional country name

### Short-term (determinism & save integrity)
4. **Replace `System.Random` with `ISeededRng`** in `FinalWishSystem`, `CombatTraumaSystem`, `WeatherSystem`
5. **Replace `Guid.NewGuid()`** in `ProceduralItemInstance` with seeded generation
6. **Create Unity `IJsonSerializer` adapter** and migrate `SaveSystem` off `JsonUtility`
7. **Consolidate duplicate `WornGear`** and `SimClock` implementations

### Medium-term (architecture debt)
8. **Move `PersonalQuestSystem` to Core** — 4936 lines, the biggest migration blocker
9. **Move `MedicalSystem`, `SurvivorWorkShiftSystem` to Core**
10. **Adopt core Utility AI in Unity** — replace defective fork
11. **Unify event buses** — make core `IEventBus` the real bus, or document the split
12. **Add save/load round-trip tests** for NeedsSystem and RadiationSystem
13. **Add tests for JournalSystem**

### Long-term (migration & quality)
14. **Set up Git LFS** for ~565 MB of binary assets
15. **Align target frameworks** — tests should match Godot host (`net8.0`)
16. **Add `schema_version`** to all core data files
17. **Standardize JSON property naming** — pick camelCase or snake_case, migrate all
18. **Fix 13 bare `catch { }` blocks** — add logging via `ILog.Error()`
19. **Shrink the Unity surface** — only 7% of Unity files reference core

---

## File Statistics

| Category | Count | Notes |
|----------|-------|-------|
| Core C# files | 234 | Engine-agnostic, zero coupling |
| Unity C# files | 1337 | 233K total lines |
| Godot C# files | 84 | Including 10 Bridge shim files |
| Test files | 143 | xUnit, ~1,491 test methods |
| JSON data files | 280 | ~3 MB total, 56 untracked |
| Binary assets (tracked) | — | ~565 MB (PNG + AI + archive) |
| ScriptableObject defs | 121 | Unity editor convenience |
| Total git commits | 1002 | Active development |
| Uncommitted changes | 56 files | +713 / -220 lines |
| Largest Unity file | 4936 lines | `PersonalQuestSystem.cs` |
| Largest Godot file | ~3000 lines | `Main.cs` |
| TODO/FIXME/HACK | 0 | Entire codebase |
| Bare `catch {}` blocks | 13 | Core only |
