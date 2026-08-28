# ASHFALL Repository Deep Review Report [HISTORICAL ARCHIVE]

> [!CAUTION]
> **SUPERSEDED HISTORICAL DOCUMENT — DO NOT USE FOR CURRENT ARCHITECTURE OR IMPLEMENTATION**
>
> This audit was conducted on **2026-08-16** during the early dual-engine bridge era.
> **Current Status (August 2026):**
> 1. **Unity Host Fully Deleted**: The legacy Unity host (`Assets/_Game/`) and compatibility bridge (`src/Bridge/`) have been completely deleted. Godot 4.7+ .NET is the authoritative and sole game host.
> 2. **All Critical & High Findings Resolved**:
>    - **C1 (Cross-Host Saves / JsonUtility)**: Resolved. All saves now use portable `IJsonSerializer` and `SaveChecksum` envelopes.
>    - **C2 (Determinism / System.Random)**: Resolved. Migrated to `ISeededRng` / `SeededRng`.
>    - **C3 (HoldfastTradeSessionTests)**: Resolved. 100% passing tests (3344+ tests green).
>    - **C4 (Untracked Narrative JSON)**: Resolved. All 196+ narrative JSON files are fully tracked in Git.
>    - **C5 (JsonUtility in Catalog Loaders)**: Resolved. Migrated to Core `SystemTextJsonSerializer`.
>    - **C6 (Unity Host Logic)**: Resolved. Deleted with `Assets/_Game/`; Core is single source of truth.
>    - **C7 (Demoted Ghost Markers)**: Resolved. 0 markers remain.
>    - **C8 (Broken CI Pipeline)**: Resolved. Canonical `dotnet` + `godot --headless` CI gate enforced.
>
> For active project architecture, consult:
> - [`AGENTS.md`](AGENTS.md) — Canonical rules and invariants.
> - [`docs/CURRENT_AUTHORITY.md`](docs/CURRENT_AUTHORITY.md) — System index and current authority map.
> - [`docs/ASHFALL_CODE_INDEX.md`](docs/ASHFALL_CODE_INDEX.md) — Architecture and subsystem code paths.

**Date:** 2026-08-16 | **Branch:** `cursor/phase11-expansion-ui-integration` (historical) | **Commits:** 1002
**Method:** 6 parallel deep-review agents + direct investigation. All findings verified against historical source.

---

## Historical Executive Summary (2026-08-16)

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
| ~~M1~~ | ~~124 compiler warnings in tests~~ — RESOLVED | was CS8600 ×144, CS8602 ×12, CS8618 ×28 (nullable refs); now 0 nullable warnings, 3 minor xUnit analyzer hints. See AGENTS.md §H9. |
| M2 | 121 ScriptableObject definitions | Risk of dual data authority vs JSON |
| M3 | Godot migration at ~4% | 10K LOC Godot vs 232K LOC Unity |
| M4 | Port adapters incomplete in Unity | No `IFileIO`, `IJsonSerializer`, or `IClock` adapters |
| M5 | Godot compiles all 1337 Unity files | Via shim — massive dead weight in Godot build |
| ~~M6~~ | ~~5 Godot save stores have no checksum~~ — RESOLVED | `ExpeditionSaveStore`, `MedicalSaveStore`, `NarrativeSaveStore`, `WorldSaveStore`, `JournalSaveStore` now ship checksummed envelopes and require a non-empty `Checksum` field in the new format. Integrity contract pinned by `Ashfall.Core.Tests/SaveStoreChecksumSweepTests.cs` (12 tests, 3 per store). See AGENTS.md §SAVE/LOAD. |
| ~~M7~~ | ~~`JournalSaveStore` bypasses core serializer~~ — RESOLVED | Serializes via `SystemTextJsonSerializer` (core `IJsonSerializer` adapter, `Ashfall.Core/HostDefaults.cs`), the same path as every other host store; checksummed envelope + legacy fallback verified by `JournalSaveChecksumTests` in `SaveStoreChecksumSweepTests.cs` and the `--journal-selftest` headless run. See AGENTS.md §SAVE/LOAD. |
| M8 | `Main.cs` (Godot) is 6546 lines in one file | Not a shapeless monolith: single `partial class Main` organized as per-subsystem triads — `SetupXxx` (construct + wire), `SaveXxx` (capture; `SaveAll` orchestrates all 24), `FlushXxxIfDirty` (deferred flush); 31/24/17 methods. Risks: triad drift (a Setup without a Save silently drops state) and single-file navigation. See AGENTS.md §H7 |
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
- **1,941 tests across 173 files** (was ~1,491 / 143 — gained 450+ methods and 30 files from the save-stores + pending-list + wire-contract work)
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
| Test files | 173 | xUnit, 1,941 test methods |
| JSON data files | 280 | ~3 MB total, 56 untracked |
| Binary assets (tracked) | — | ~565 MB (PNG + AI + archive) |
| ScriptableObject defs | 121 | Unity editor convenience |
| Total git commits | 1002 | Active development |
| Uncommitted changes | 56 files | +713 / -220 lines |
| Largest Unity file | 4936 lines | `PersonalQuestSystem.cs` |
| Largest Godot file | 6546 lines | `Main.cs` — one file, structured as per-subsystem `Setup`/`Save`/`FlushIfDirty` triads (AGENTS.md §H7) |
| TODO/FIXME/HACK | 0 | Entire codebase |
| Bare `catch {}` blocks | 13 | Core only |

---

## Initiative #41 — Generic Injected Persistence Service (2026-08-27) — COMPLETE

Replaced the per-store persistence boilerplate in every host save store (51
`*SaveStore*.cs` files + 10 stores embedded in `*HostSession.cs` files) with
one generic, port-injected service:

- **Core:** `Assets/Ashfall.Core/Save/SaveStore.cs` (`SaveStore<T>`, checksummed
  + codec flavors) built on `SaveEnvelopeHelper`; `SchemaVersionedEnvelope<T>`
  legacy adapter preserves the 12 shelter-batch property envelopes byte-for-byte.
- **Host:** `src/Host/SaveStoreHub.cs` is the single injection point
  (FileSystemIO / SystemTextJsonSerializer / GodotLog / SaveSlotRoot per-call
  base-dir routing).
- **Façades:** all stores keep their class names, consts, and public static
  signatures; ~4,900 lines of duplicated logic removed across batches.
- **Behavior:** on-disk JSON byte-identical; checksum targets, fallback
  strictness, and per-section quirks (Economy state-hash, HoldfastTrade
  quarantine/backup, World multi-field envelope, Greenhouse indented JSON,
  StartingLevel surface) preserved. One deliberate change: writes are now
  atomic (temp+rename) with optional `.bak`.
- **Gates hardened:** coverage gate + checksum selftest Gate A + matrix
  generator now REQUIRE SaveStoreHub/SaveEnvelopeHelper/Core-codec delegation.
- **Pinned by:** `Ashfall.Core.Tests/Save/SaveStoreServiceTests.cs` (incl.
  byte-identity vs the hand-rolled pattern) plus the pre-existing sweep/seal/
  wire suites, all passing unmodified.
- **Discovered follow-up (out of scope):** the 12 shelter-batch sections carry
  a degenerate checksum (SaveChecksum walks public fields only; their
  property-only envelopes hash to a constant) — integrity is not real there;
  fixing it needs a save-evolution initiative with legacy dual-read.

---

## Initiative #42 — Single Versioned Atomic Campaign Envelope (2026-08-27) — COMPLETE

The per-slot `campaign.json` envelope is now the single authoritative save:

- **Save:** every `SaveXxx` captures its section bytes in memory
  (`SaveStore<T>.CapturePersisted`, byte-identical to the old file format);
  `CampaignEnvelopeBuilder` packs the registry-ordered, whitelisted payload
  map into ONE atomic write. Failed capture aborts the whole save — mixed-
  generation partial saves are structurally impossible. Section files are no
  longer written at save time.
- **Format V2:** sections keyed by `SaveSectionRegistry` SectionKey with real
  schema versions; `SaveSectionRegistry.SectionFileNames` is the file-name
  authority (whitelist + V1 filename→key migration + registry-derived reset
  lists, closing 12+ hardcoded-delete gaps).
- **Load:** validate → migrate V1 in memory (reserved `legacy` import section
  preserved; strays dropped; disk rewritten V2 on next save) → explode to
  registry file names → unchanged `SetupXxx` flows.
- **Legacy:** Continue with no slots auto-migrates pre-slot global section
  files verbatim into a fresh `migrated_N` slot (corrupt sections skipped
  with warning; originals untouched).
- **Pinned by:** `CampaignEnvelopeBuilderTests` (9 tests) + the 7-gate
  `--save-load-ui-failure-selftest`; all pre-existing suites pass.
- **Known follow-up (out of scope):** in-memory restore without file
  explosion (`ICampaignSaveSection` remains the seam); multi-generation
  retention beyond the single `.bak`.

---

## Task #101 — Expedition Vehicle & Weapon-Condition Logistics (2026-08-28) — COMPLETE

- Vehicles change real outcomes: dispatch preparation gates on exact fuel
  need (depleted tank refuses with a refuel message), burns fuel + wear via
  `PrepareForExpedition`, builds a per-tick profile from vehicle condition
  (worn ⇒ mid-route breakdown risk); driven sorties travel faster and haul
  more; a seeded breakdown reverts the remainder to foot.
- Weapon condition flows through ONE authority: equipment-condition
  instances (Weapon family) project into combat loadouts, engagement wear
  writes back at encounter end, and readiness/jam risk feed the expedition
  estimate (degraded weapon raises effective encounter risk up to +50%).
- UI: DISPATCH PREPARATION block (vehicle/weapon selectors, refuel top-up,
  live estimate line); dispatch routed through the host (gates kept).
- Persistence: expedition section = `ExpeditionAggregateState` (sorties +
  garage), legacy shapes migrate, `vehicles.json` is authoritative.
- Verification: Core 4395/4399 (4 failures = concurrent session's
  uncommitted HostCli onboarding files, pre-existing); `--expedition-selftest`
  10/10 demo + 9/9 vehicle gates; data-integrity + bridge selftests PASS;
  host builds 0 errors. Cross-tool review required (≥2 coupled variables:
  fuel/condition/readiness).

---

## Task #111 — Campaign-Day Coordinator Migration: Residual Gap Closure (2026-08-28) — COMPLETE

The core migration landed earlier (commit 5c0f9046: coordinator, 17 phase
owners, fail-closed, tests). This pass closed the four audited residuals:

- **Retry-restore (substep 7 made real):** a failed fail-closed advance arms
  a pending restore; a retry of the same day rolls owners implementing the
  new `IPreDaySnapshotRestore` back (reverse order) before ticking — no
  double-applied days. Pinned by 4 new coordinator tests (rollback works,
  failed rollback stays fail-closed, success clears, stale day dropped).
- **Real snapshots on the 5 stateful owners:** holdfast_core (clock — the
  double-day hazard), survivors_needs, weather_world, expeditions_caravans
  (aggregate + caravans), economy_market.
- **economy_market owner (phase 2):** the market now advances daily through
  the coordinator via `EconomyHostSession.TickDay(day, rng)` on the economy
  RNG stream. INTENTIONAL BEHAVIOR CHANGE: the market previously only moved
  through a dead demo button; economy pacing may shift — flag for balance.
- **Demo cleanup (substep 5):** nine dead On*Clicked handlers and their
  orphaned Demo host methods deleted (Economy/Caravan/World/Medical/
  Maritime/Expedition incl. TickDemoHours); Maritime's live menu button
  calls StartDive; the census-levy menu button kept, promoted to
  `HonourCensusLevy`.
- **Source gate widened (substep 12):** every src/Main*.cs partial now
  scanned — TickSimDay only in Main.Holdfast.cs + Main.UiTests.* drivers;
  `_campaignDay.Advance` only in Main.Holdfast.cs.

Verification: Core 4400/4404 (4 failures = concurrent session's uncommitted
HostCli onboarding files, pre-existing); host 0 errors (2 warnings in the
same concurrent files); expedition/data-integrity/save-load selftests PASS;
gate + coordinator suites green.

---

## Task #112 — Campaign Calendar Authority: Residual Gap Closure (2026-08-28) — COMPLETE

Core delivery landed earlier (8029aa69: ICampaignCalendar, adapters,
reconciler, gates, doc). This pass closed the audited residuals:

- **Calendar now LEADS**: CommitAdvance derives targetDay from
  `Calendar.CurrentDay + 1`; the Core holdfast clock is a projection
  landed by the holdfast_core owner and re-synced FROM the calendar on
  restore/new-game/reconciliation (three stray calendar writes removed).
- **Reconciler wired** into SetupCampaignDay (was dead code): legacy
  section days (campaign_day/holdfast/duty_roster/economy/year_of_ash)
  reconcile before the envelope is adopted; disagreements emit
  [CALENDAR_MISMATCH]; a later section upgrades an older envelope.
- **Fallbacks removed**: _simDay projects the calendar only; radio and
  phase0 read the projection, not the holdfast clock.
- **Duty roster off-by-one fixed**: TickDay no longer self-advances; the
  roster clock equals the campaign day after every advance.
- **Gate widened**: Calendar.SetDay / AdvanceDays / sim-day self-mutation
  forbidden across Main partials + UI (allowlist: Holdfast sync sites,
  UiTests drivers); engine-internal ISimClock ticks in src/Host stay
  legitimate (substep 2).
- **Live projection-agreement gates**: silent_foundry_uitest now asserts
  holdfast clock == market day == roster clock == calendar adapters after
  real advances, plus calendar save/reload round-trip.
- BEHAVIOR NOTE: reconciliation may now advance a campaign whose
  campaign_day section lagged a subsystem section (documented priority
  campaign_day > holdfast > max). Verification: coordinator/calendar
  suites 34/34; Core 4418/4422 (4 = concurrent session's uncommitted
  HostCli onboarding files); silent_foundry_uitest projection gates 8/8
  (its factions-panel failure pre-exists, reproduced on a stashed tree).
