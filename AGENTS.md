# PROJECT: ASHFALL (working title) — 2D Atomic-War Survival

Original 2D survival-management game set after a nuclear exchange. Inspired by the survival-management genre; do **not** copy any existing game's art, names, characters, UI layout, text, or code.

---

## READ THIS FIRST — NON-NEGOTIABLE RULES

These five rules override anything else in this file. If a later section contradicts them, the rule below wins.

1. **Unity is NOT a target editor.** This project migrated to Godot. Do not invoke the Unity editor, batchmode, headless, playmode, or any Unity build tool — ever — unless the user explicitly asks in that message.
2. **All verification uses `dotnet` + `godot --headless`.** There is no other build/test path. No `unity -batchmode`. No `.unity` scene loading. No Unity Test Framework.
3. **Migration direction is Unity → Godot, always.** Never port code from Godot back into `Assets/_Game/`. Never write new gameplay logic in `Assets/_Game/`. Anything engine-specific belongs in `src/` (Godot) or — if engine-agnostic — `Assets/Ashfall.Core/`.
4. **Unity assets must be migrated to Godot assets.** `.unity` scenes → `.tscn`. `.prefab` → `.tscn` (packed scene) or instanced `.tres`. `.asset` (ScriptableObject) → JSON in `StreamingAssets/Data/` (the authority). `.png`/`.psd`/`.ai` art → import into the root-level Godot `assets/` tree with Godot-native import settings. Do not extend the `Assets/_Game/` asset tree.
5. **Core stays engine-agnostic.** `Assets/Ashfall.Core/` is the single source of truth. No `UnityEngine.*`, no `Godot.*`, no `JsonUtility`. New logic that runs in both hosts goes here.

If a task seems to require violating any of the above, stop and ask the user.

---

## STACK

| Layer            | Engine / Format             | Location                                  | Namespace           | Target          |
|------------------|-----------------------------|-------------------------------------------|---------------------|-----------------|
| **Core** (truth) | Engine-agnostic C#          | `Assets/Ashfall.Core/`                    | `Ashfall.Core.*`    | `netstandard2.1` |
| **Godot host** (active, only editor) | Godot 4.7+ (.NET/C#) | `src/`                                     | `AtomicWar.GodotApp.*` | `net8.0`     |
| **Unity host** (inactive — migrating out) | Unity 6 LTS, 2D, URP | `Assets/_Game/` (read-only legacy)         | `AtomicWar._Game.*` | Unity 6 (do not run) |
| **Tests**        | xUnit                       | `Ashfall.Core.Tests/`                     | `Ashfall.Core.Tests`| `net9.0`        |
| **Data authority**| JSON                       | `Assets/StreamingAssets/Data/`            | —                   | —               |
| **Godot Bridge** (shim, shrinking) | `UnityEngine.*` compat | `src/Bridge/`                              | `UnityEngine.*`     | `net8.0`        |
| **Godot assets** (imported/migrated) | Godot native (`assets/`) | `assets/art/`, `assets/audio/`, `assets/fonts/`, `assets/sprites/`, `assets/ui/` | — | — |

Godot project: `project.godot` at root, `gl_compatibility` renderer, 1920×1080, 60 FPS, `BarlowCondensed` + `ShareTechMono` fonts.

---

## ASSET MIGRATION (Unity → Godot)

Unity assets in `Assets/` are legacy. Every Unity asset has a Godot equivalent — port, do not extend.

| Unity asset                         | Godot equivalent                              | Where it lives after migration         |
|-------------------------------------|-----------------------------------------------|----------------------------------------|
| `.unity` scene                      | `.tscn`                                       | root of repo or `assets/` subfolder    |
| `.prefab`                           | Packed `.tscn` (instanced via `PackedScene`)  | `assets/<system>/`                     |
| `.asset` ScriptableObject           | JSON in `StreamingAssets/Data/` (authority) + generator in Core | `Assets/StreamingAssets/Data/` |
| `.png`/`.jpg`/`.psd`/`.ai` texture  | Re-imported PNG with Godot import preset       | `assets/art/` or `assets/sprites/`     |
| `.wav`/`.ogg`/`.mp3` audio           | Re-imported with Godot `AudioStream` preset    | `assets/audio/`                        |
| `.ttf`/`.otf` font                  | Re-imported with font hinting preset           | `assets/fonts/`                        |
| `.controller` animator              | `AnimationPlayer` / `AnimationTree` in `.tscn`| inside the scene that needs it         |
| `Material` (.mat)                   | Godot `Material` (`.tres`) or StandardMaterial3D/CanvasItemMaterial | `assets/<system>/materials/` |
| `PhysicsMaterial2D`                 | Godot `PhysicsMaterial`                       | inside scene resource                  |
| `TileMap`/palettes                  | Godot `TileSet` + `TileMapLayer`              | `assets/<zone>/`                       |

**Rules for asset work:**
- Never edit `.meta` files by hand for Unity — they will be deleted when the asset is migrated.
- Never create a new `.unity` scene, `.prefab`, or `.asset`.
- When porting art, also port the import settings (filter, mipmaps, compression) into the `.import` file Godot generates.
- ScriptableObject data must be re-encoded as snake_case JSON with `schema_version` — see Data Authority below.

---

## CORE ARCHITECTURE — SIX INVARIANTS

### Invariant 1 — Zero engine coupling in Core
`Assets/Ashfall.Core/` must contain **zero** references to `UnityEngine`, `UnityEditor`, `Godot`, `GodotSharp`, or `JsonUtility`. The `.asmdef` has `noEngineReferences: true`. Holds today: 0 violations.

### Invariant 2 — Ports and Adapters
Host needs are interfaces in `Assets/Ashfall.Core/Ports.cs`:

| Interface       | Purpose                       | Godot adapter          | Unity adapter (legacy, do not extend) |
|-----------------|-------------------------------|------------------------|---------------------------------------|
| `IJsonSerializer` | JSON serialize/deserialize   | core default           | **MISSING** — uses `JsonUtility`      |
| `IFileIO`       | File/directory access         | core default           | **MISSING** — uses `System.IO`        |
| `ILog`          | Info/Warn/Error logging       | `GodotLog`             | `GameLogAdapter` (private nested)     |
| `IClock`        | Day counter                   | core default           | core default                          |
| `ISeededRng`    | Deterministic PRNG            | `CoreSeededRng`        | **MISSING** — uses `System.Random`    |
| `IEventBus`     | String-based pub/sub          | **NOT USED** (direct calls) | **NOT USED** (Unity static bus) |

`ISimClock` (tick-based) duplicates `IClock` — consolidation planned.

### Invariant 3 — Cross-host save compatibility
A save written by one host must load in the other. **Currently violated for the main save** — `Assets/_Game/Core/SaveSystem.*.cs` (967+ lines, 10 partial files) uses `JsonUtility`. Twenty-eight catalog loaders in `Assets/_Game/Data/` use the same anti-pattern.

Fix path: ship a Unity `IJsonSerializer` adapter, then migrate the SaveSystem and all 28 catalog loaders off `JsonUtility`. Until then, do not add new `JsonUtility` call sites.

### Invariant 4 — Determinism
Same seed ⇒ identical simulation in both engines. Use `ISeededRng` (xorshift64*). Never `System.Random`. Never `Guid.NewGuid()`.

Known offenders (fix when touching these):
- `Assets/Ashfall.Core/FinalWishSystem.cs:66` — `public System.Random Rng;`
- `Assets/Ashfall.Core/CombatTraumaSystem.cs:53` — `public System.Random Rng;`
- `Assets/Ashfall.Core/WeatherSystem.cs:144` — `new Random(unchecked(...))`
- `Assets/Ashfall.Core/ProceduralItemInstance.cs:36` — `Guid.NewGuid()`
- `InMemoryFlagLedger` uses `StringComparer.OrdinalIgnoreCase` — case-normalization drift risk across hosts.

### Invariant 5 — No gameplay logic in hosts
Thin MonoBehaviours (Unity) and thin Nodes (Godot) handle only presentation, input, and wiring. Gameplay lives in plain C# systems inside `Ashfall.Core`.

Known offenders (do not grow these; migrate logic into Core instead):
- `Assets/_Game/Quests/PersonalQuestSystem.cs` (4936 lines, 404 methods, 0 core refs)
- `Assets/_Game/Medical/MedicalSystem.cs` (1287 lines, 0 core refs)
- `Assets/_Game/Survivors/SurvivorWorkShiftSystem.cs` (1291 lines, 0 core refs)
- `Assets/_Game/Economy/DynamicEconomySystem.cs` (1797 lines, minimal core refs)
- `src/Host/HoldfastRuntimeSession.cs` duplicates core survival mechanics — refactor into Core.

### Invariant 6 — Data authority is JSON
`Assets/StreamingAssets/Data/` is the authority. ScriptableObjects are a Unity-editor convenience generated from JSON, never the source. Never fork data per engine.

Known data issues:
- 121 ScriptableObject definitions — risk of dual authority
- 56 narrative JSON files are **untracked in git** — missing on fresh clone (`Assets/StreamingAssets/Data/narrative/`)
- Property naming mixes `camelCase` and `snake_case` — migrate to `snake_case`
- Only 35 of ~280 JSON files have `schema_version` — add to all core data files
- ~~`world_history.json:15` references "China"~~ — RESOLVED: replaced with a fictional nation ("the Meridian Compact"); all real-country/alliance terms swept from the data authority and gated by `Ashfall.Core.Tests/DataRuleComplianceTests.cs` (no real countries/wars/people).

---

## BRIDGE SHIM RULES (Godot)

`src/Bridge/` (10 files, 2686 lines, 165+ shimmed types) lets legacy `Assets/_Game/` code compile under Godot by providing a `UnityEngine.*` compatibility layer. The shim is a **migration aid, not the end state**. Goal: shrink it to zero.

- `BridgeGap.Semantic()` — throws on logic-affecting gaps. Never silence. These prevent silent bugs.
- `BridgeGap.Cosmetic()` — logs visual-only gaps. Expected in headless mode.
- `BridgeSelfTest` — run with `godot --headless --path . -- --bridge-selftest`.
- Do not add new shim types without classifying gaps (Semantic / Cosmetic / no-op).
- Every new `Assets/_Game/` file that needs the shim is a migration smell — move the logic into Core instead.

---

## SAVE / LOAD

Every stateful system implements:

```csharp
public SystemState CaptureState() => new SystemState { ... };
public void RestoreState(SystemState state) { ... }
```

DTOs are `[Serializable]` plain C# classes. Use `IJsonSerializer`, not `JsonUtility`.

- `SaveChecksum` (`Assets/Ashfall.Core/SaveChecksum.cs`) — reflection-based integrity hash. Normalizes null/empty, float G9 formatting, culture-invariant, ordinal name order.
- Versioned migration: codecs support V1→V2→V3. Throw on future, migrate on past. Examples: `HoldfastSaveCodec`, `YearOfAshSaveCodec`, `DoseLedgerSaveCodec`.
- Known gaps (5 Godot save stores lack checksum): `ExpeditionSaveStore`, `MedicalSaveStore`, `NarrativeSaveStore`, `WorldSaveStore`, `JournalSaveStore`. All five now ship checksummed envelopes and require a non-empty `Checksum` field in the new format. Pre-checksum bare-state saves still load via the legacy fallback path. Integrity contract pinned by `Ashfall.Core.Tests/SaveStoreChecksumSweepTests.cs` (12 tests, 3 per store: clean round-trip, mutated-state changes hash, null checksum rejected).
- **Known issue — stricter load guard:** `NarrativeSaveStore.TryLoad` and its three sibling stores now reject a new-format envelope whose `Checksum` field is null or empty (`checksum field missing (corrupt save)`). The old guard `!string.IsNullOrEmpty(envelope.Checksum)` silently treated a missing checksum as "legacy", which is wrong: a malformed save in the new format is not legacy. The bare-state fallback only fires for genuinely pre-checksum saves.
- ~~`JournalSaveStore` bypasses core `IJsonSerializer`~~ — RESOLVED: serializes via `SystemTextJsonSerializer` (core adapter, `HostDefaults.cs`), the same path as every other host store; contract pinned by `SaveStoreChecksumSweepTests`.
- `LocationEvolutionSaveable`, `WildlifeSaveable`, `LandmarkSaveable` have empty `CaptureState/RestoreState` — silent data loss; fix when touched.

---

## EVENT SYSTEM

Two parallel buses (architectural debt, merge planned):

| Bus                              | Style                            | Where it's real       |
|----------------------------------|----------------------------------|-----------------------|
| `IEventBus` / `SimpleEventBus`   | String-based, constructor-injected | Defined, **underused** |
| `EventBus` static class          | Type-safe generics, allocation-free, editor profiling | Unity side, the real decoupler |
| Godot                            | No bus — direct method calls on host sessions | — |

Rule: every public system raises C# events on state change (for UI + save). Use whichever bus the host wires. Unity's `EventBus.IsSuppressed` flag suppresses events during save/load restore.

---

## DATA INTEGRITY

`Assets/Ashfall.Core/CatalogIntegrityValidator.cs` (603 lines) — five-tier validation:

1. **REGISTRY** — every definition-position id with file + JSON path.
2. **TIER-1** — strings with a known snake_case prefix (200+ prefixes) must resolve.
3. **TIER-2** — values at known reference keys (`resultItemId`, `requiredItemId`, etc.) must resolve.
4. **RANGES** — `minDay`/`maxDay` pairs must be ordered.
5. **UNIQUENESS** — no duplicate definition ids within one file.

Run with: `godot --headless --path . -- --data-integrity-selftest` (94 catalogs, 0 errors today).

**ID rules:**
- snake_case ids everywhere. Never invent an id outside the master list.
- Known prefixes: `item_`, `loc_`, `faction_`, `trait_`, `quest_`, `recipe_`, `event_`, `npc_`, `affliction_`, `expansion_`, `encounter_`, `radio_`, `echo_`, `flag_`, `skill_`, `knowledge_`, `ending_`, `article_`, `sector_`, `zone_`, etc.
- `CatalogIntegrityValidator` mechanically enforces the rule.

---

## EXPANSION SYSTEM

`ExpansionMasterSession` (in Core) coordinates four expansions: Holdfast (01), Duty Roster (02), Standing Record (03), Nobody's Charter/Crossing (04). Verdict, Year of Ash, Greenhouse, etc. are standalone with their own host wiring.

Implementation pattern (five phases):

1. **Phase 1** — system classes in domain-specific namespaces. Each must implement `CaptureState/RestoreState` with a serializable DTO.
2. **Phase 2** — data: update `items.json`, `locations.json`, `survivors.json`, `recipes.json`.
3. **Phase 3** — new IDs into static classes, trait constants, quest runtime classes.
4. **Phase 4** — wire into `GameBootstrap`: properties, construction, event wiring, init, tick registration, save fields.
5. **Phase 5** — tests: behavior, save round-trips, canonical-IDs, integration smoke.

Known issues:
- `GameBootstrap.Phase0Expansion.cs` — six systems constructed/registered/ticked but key effects are stubs ("wired in Phase 11").
- `GameBootstrap` is a 1225-line god object across 82 partial files.
- ~~588 "DEMOTE ghost" markers across 124 Unity files~~ — RESOLVED (0 markers remain in Core/src; the migration swept them out).

---

## KNOWN ISSUES

### Critical (block release)

| # | Issue                                                                              | Location                                                |
|---|------------------------------------------------------------------------------------|---------------------------------------------------------|
| C1 | `JsonUtility` in Unity SaveSystem blocks cross-host saves                          | `Assets/_Game/Core/SaveSystem.*.cs` (6 call sites in SaveSystem.IO.cs). Wire-format contract is now pinned by `Assets/Ashfall.Core/SaveWireContract.cs` and `Ashfall.Core.Tests/SaveWireContractTests.cs` (7 tests assert Godot + Unity-shape serializers produce identical JSON trees and the same `SaveChecksum` hash for the same state); the adapter task is reduced from "design from scratch" to "implement `IJsonSerializer` over `JsonUtility` and pass the contract tests". Still missing in `Assets/_Game/Core/`. |
| C2 | ~~`System.Random` breaks determinism~~ — RESOLVED                                 | migrated to `ISeededRng`; verified by `Ashfall.Core.Tests` |
| C3 | ~~`Guid.NewGuid()` breaks determinism~~ — RESOLVED                                 | comment at `Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs:48` documents the fix |
| C4 | ~~56 narrative JSON files untracked in git~~ — RESOLVED                            | 196/196 narrative JSON files now tracked |
| C5 | ~~`HoldfastTradeSessionTests.cs` — 10 compile errors, stale API~~ — RESOLVED      | 3/3 tests pass against current API |
| C6 | 28 catalog loaders use `JsonUtility` — blocks Godot data loading                   | 21 remaining in `Assets/_Game/Data/*CatalogLoader.cs` (down from 28) |

### High

| #  | Issue                                                              | Location                                                         |
|----|--------------------------------------------------------------------|------------------------------------------------------------------|
| H1 | `HoldfastRuntimeSession` duplicates core survival mechanics        | `src/Host/HoldfastRuntimeSession.cs`                             |
| H2 | Duplicate `WornGear` class                                         | both in Core (`Inventory/Inventory.cs:22` + `Radiation/RadiationSystem.cs:64`); consolidate to one location. **Bridge exists:** `Radiation.WornGear.FromInventory(Inventory.WornGear)` is the single sanctioned conversion point, wired by the Godot host `SurvivorsHostSession` (equipped gas mask/hazmat now cuts dose; verified by `--survivors-selftest` gear probes + `InventoryGearBridgeTests`). |
| H3 | `SimClock` duplicate                                              | not actually a duplicate: `Ashfall.Core/HostDefaults.cs:67` is `IClock` (day-based), `Ashfall.Core/Clock/ISimClock.cs:15` is `ISimClock` (tick-based); both still used |
| H4 | 13 bare `catch { }` blocks swallow exceptions                      | `YearOfAshCatalogLoader.cs` (7), `VerdictCatalogLoader.cs` (3) — unchanged |
| H5 | Utility AI forked — Unity uses defective version                   | `Assets/_Game/AI/UtilityAI.cs` vs `Assets/Ashfall.Core/UtilityAI/` |
| H6 | Unity has no `IFileIO`, `IJsonSerializer`, `IClock` adapters       | `Assets/_Game/Core/`                                             |
| H7 | `Main.cs` (Godot) — one `partial class Main` in a single ~6.5k-line file, but internally regular: per-subsystem triads of `SetupXxx` (construct + wire system), `SaveXxx` (capture into save; `SaveAll` orchestrates all 24), `FlushXxxIfDirty` (deferred flush) — 31 Setup / 24 Save + `SaveAll` / 17 Flush methods across domains (Expeditions, Combat, Economy, Medical, Narrative, Holdfast, YearOfAsh, Maritime, Muster, …). Risks: triad drift (a Setup without a Save silently drops state) and single-file navigation; end state is one true partial file per domain | `src/Main.cs` |
| H8 | `SettingsManager` uses `PlayerPrefs` (Unity-only)                  | `Assets/_Game/Settings/SettingsManager.cs`                       |
| H9 | ~~124 compiler warnings in tests~~ — RESOLVED                       | test suite builds with 0 errors, 3 minor analyzer warnings (xUnit2013/xUnit2020) — not nullable refs |
| H10 | NeedsSystem & RadiationSystem save/load round-trip tests           | `NeedsRadiationSystemTests.cs` covers tick behaviour (58 tests); save/load round-trip coverage still missing |
| H11 | JournalSystem coverage                                            | 6 Core files; `JournalSaveStore` has integrity tests; `JournalSystem` core behaviour still untested |
| H12 | ScriptableObject definitions                                       | 44 in `Assets/_Game/` (down from 121 as Unity migrates to Godot); risk of dual authority reduces as engines diverge |

---

## NAMESPACE CONVENTIONS

| Layer   | Namespace                                              | Directory match |
|---------|--------------------------------------------------------|-----------------|
| Core    | `Ashfall.Core`, `Ashfall.Core.Economy`, `Ashfall.Core.Journal`, … | ✅ |
| Unity (legacy) | `AtomicWar._Game`, `AtomicWar._Game.Core`, …       | ✅              |
| Godot   | `AtomicWar.GodotApp`, `AtomicWar.GodotApp.Economy`, …  | ✅ (`AtomicWar.Journal` is the one legacy exception) |
| Tests   | `Ashfall.Core.Tests`                                   | ✅ (flat)       |

---

## VERIFICATION CHECKLIST (run after every task)

Report PASS/FAIL for each before claiming done.

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Must compile (currently green: HoldfastTradeSessionTests, 1934/1934 pass)
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # All tests pass
3. dotnet build Ashfall.csproj                                  # Godot host: 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest       # Catalog integrity: 0 errors
5. godot --headless --path . -- --bridge-selftest               # Shim honesty
```

The `dotnet` and `godot` commands are the canonical path. **No Unity commands.**

---

## DOMAIN REFERENCE

**Survival needs:** hunger, thirst, fatigue, warmth, morale, **RADIATION** (accumulates), health, hygiene.

**Hazards:** fallout zones, fallout storms, nuclear-winter cold, irradiated water/food, EMP/electronics failure, mutated flora/fauna, chronic illness (long-term rad), respiratory degeneration.

**Medical:** affliction pipeline, triage, chemical dependency, respiratory degeneration, combat trauma, somatic flashback, guilt/insomnia, dose ledger, chelation, iodine/anti-rad.

**Social:** ideological friction, ration conflict, moral branching, leadership, caregiving, final wishes, trauma bonding, coalition camps, census claims, voluntary registers.

**Economy:** dynamic pricing, trade stances/attitudes, ledger debt, brine water, ice roads, cohort system, sick lists, waystations, traveling caravans, holdfast trade sessions.

**World:** weather, seasonal fallout storms, nuclear winter, visibility, outdoor radiation, geological strata, hydro-geology, industrial ruins, wasteland cartography.

**Shelter:** bunker with radiation shielding + air-filtration (degrades), material shielding, sky-layer armor, blast doors, hatch defense, greenhouse, duty roster, survivor work shifts.

**Key items:** dosimeter, geiger counter, iodine pills, rad-away/anti-rad, gas mask, hazmat suit (degrading), water filter, fuel, air filter (shelter), clean water, potassium iodide, chelation agents, improvised cooking stove, basic water boiler, protective rubber gloves, sewing kit, cigarette lighter, car battery.

---

## REPOSITORY SETUP (first clone / every contributor)

Run once on any fresh checkout **before** staging assets:

```bash
./setup-repo.sh   # idempotent: core.ignorecase=false + git lfs install
```

Why it matters — the repo deliberately keeps two case-distinct trees:

| Path        | Tree                          |
|-------------|-------------------------------|
| `Assets/`   | Unity legacy (`Ashfall.Core`, `StreamingAssets/Data`, `_Game`) |
| `assets/`   | Godot-native assets (`art/ audio/ ui/ sprites/ fonts/`) |

Git's `core.ignorecase` defaults to **true** on macOS/Windows, which aliases
`Assets/` and `assets/` and breaks `git add assets/` (it silently stages the
uppercase tree instead). `setup-repo.sh` pins `core.ignorecase false`.

Binary policy: images/fonts are **Git LFS** pointers (`git lfs ls-files`
lists them); `*.wav/*.mp3/*.ogg` stay **plain binary** by `.gitattributes`.
Never add large PNG/AI assets outside LFS.

Verifying assets from a clean checkout (the one-time import is gitignored):

```bash
dotnet build Ashfall.csproj
./scripts/ci/godot-asset-gate.sh   # import + asset-registry 48/48 + data-integrity + bridge + disease + expansions
```

## GIT RULES

- Commit after each accepted deliverable.
- Keep changes small and reviewable — **one system per task**.
- Binary assets: do **not** add large PNG/AI assets without Git LFS (~565 MB tracked without LFS today).
- `unity-assets-archive-2026-08-14.tar.gz` (140 MB) should be removed from git history.
- `.gitignore` is comprehensive — do not track `_verify_*.csproj`, `.mimocode/`, `Builds/`.
- `scripts/` directory is included in `.csproj` but empty — do not add to it without understanding why.

---

## TASK WORKFLOW

0. Check `REPO_REVIEW_REPORT.md` for known issues in the area you're touching.
1. **Restate the goal in 2 lines.**
2. **List files you'll touch/create.**
3. **Implement** — follow the invariants above, especially: no engine coupling in Core, `ISeededRng` (not `System.Random`), `IJsonSerializer` (not `JsonUtility`), `CaptureState/RestoreState` for any stateful system, Unity assets → Godot assets.
4. **Verify** — run all 5 verification steps above.
5. **Summarize** + give the exact next prompt to run.

---

## CROSS-TOOL QA RULE

Any system introducing ≥2 new coupled variables must be implemented by one tool and reviewed/tested by a **different** tool. The reviewer sees only the diff + the spec — never the implementer's reasoning. It reviews the code, not the story.

---

## TONE & CONTENT RULES

- No magic, no fantasy, no real countries/wars/people, no glorified violence.
- Tone: cold, exhausted, human, restrained. Show, don't preach.
- After writing code, VERIFY: the `Ashfall.Core` test suite must run WITHOUT Unity (plain
  `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`), and Godot host checks via
  `dotnet build Ashfall.csproj` or `godot --headless`. NEVER run Unity batch compile or playmode
  tests unless the user explicitly asks for Unity to be run. Report PASS/FAIL before claiming done.
- Keep changes small and reviewable. One system per task.

ATOMIC-SURVIVAL DOMAIN (the needs/hazards this game is about):
needs: hunger, thirst, fatigue, warmth, morale, RADIATION (accumulates), HEALTH
hazards: fallout zones, fallout storms, nuclear-winter cold, irradiated water/food,
         EMP/electronics failure, mutated flora/fauna, chronic illness (long-term rad)
items: dosimeter, geiger counter, iodine pills, rad-away/anti-rad, gas mask,
       hazmat suit (degrading), water filter, fuel, air filter (shelter), clean water
shelter: bunker with radiation shielding level + air-filtration that degrades

WORKFLOW PER TASK:
1) Restate goal in 2 lines. 2) List files you'll touch/create. 3) Implement.
4) Verify (compile/test). 5) Summarize + give the exact next prompt to run.

CROSS-TOOL QA RULE: any system that introduces >=2 new coupled variables MUST be
implemented by one tool and reviewed/tested by a DIFFERENT tool (see Prompt #26).
The reviewer may NOT see the implementer's reasoning — only the diff + the spec —
so it reviews the CODE, not the story the implementer told itself.

## Cursor Cloud specific instructions

This is a **Unity 6 LTS (`6000.5.5f1`)** project. There is no npm/pip/dotnet layer;
Unity itself compiles the C#, resolves packages (`Packages/manifest.json`), and runs
the tests. The startup update script installs the Unity Editor to
`~/Unity/Hub/Editor/6000.5.5f1/Editor/Unity` (matches the path in `docs/CI.md`) plus
the Linux runtime libs it needs. Refer to `README.md` ("Verify") and `docs/CI.md`
for the canonical compile/test commands; only the non-obvious caveats are below.

### A Unity license is REQUIRED and is not bundled

Every Unity operation (compile, EditMode/PlayMode tests, player build, editor play)
needs an activated license. Without one the editor prints
`No valid Unity Editor license found. Please activate your license.` and exits 198.
Provide these as **secrets** (see `docs/CI.md`; obtain via https://game.ci/docs/github/activation):
- `UNITY_EMAIL`, `UNITY_PASSWORD`
- `UNITY_LICENSE` (full `.ulf` contents, Personal) **or** `UNITY_SERIAL` (Plus/Pro)

Activate once per VM before running anything, e.g. Personal (write the `.ulf`, then
Unity picks it up), or serial-based:
`Unity -batchmode -nographics -quit -serial "$UNITY_SERIAL" -username "$UNITY_EMAIL" -password "$UNITY_PASSWORD" -logFile -`.
Note: a `.alf`/`.ulf` produced by manual activation is tied to the machine that made
it; cloud VMs are ephemeral, so prefer credential/serial activation or a `.ulf` that
your Unity account can re-activate.

### Running compile / tests / build (after activation)

Use `xvfb-run` for anything that may touch graphics; batchmode+nographics is fine for
compile and tests. Canonical commands live in `README.md`/`docs/CI.md`. Gotchas:
- **Do NOT combine `-runTests` with `-quit`** — it kills the editor before results are
  written and exits 0 with an empty result file (already called out in `README.md`).
- EditMode and PlayMode are **separate** `-testPlatform` runs; run both.
- First editor open on a fresh checkout populates `Library/` (long, one-time); it is
  gitignored. A warm `Library/` speeds up subsequent runs.
- `Gameplay.unity` is generated by `Tools/ASHFALL/Build Gameplay Scene`
  (`buildMethod AtomicWar._Game.Editor.GameplaySceneBuilder.BuildGameplayScene`); data
  catalogs by `AtomicWar._Game.Editor.CatalogGenerator.GenerateAll`. Don't hand-edit
  generated assets — regenerate.

### Runnable without a license

- The CI "Data Validation Gate" (StreamingAssets JSON syntax + ProjectVersion pin) is
  pure Python and needs no Unity/license — handy for a quick sanity check.
- **Offline Roslyn compile (limited):** the editor ships a usable compiler at
  `Editor/Data/DotNetSdk/sdk/*/Roslyn/bincore/csc.dll` + BCL under
  `Editor/Data/UnityReferenceAssemblies/unity-4.8-api` + `UnityEngine` modules.
  Leaf assemblies (e.g. `AtomicWar._Game.Utilities`, then `Survivors`) can be
  compiled without activating a license. This does **not** replace `-runTests` or
  a player build — packages are unresolved, and UnityEngine.Object "fake null"
  blocks running ScriptableObject-based systems under plain mono. Use it only as
  a syntax/API smoke check when no license is available.
- Full EditMode/PlayMode suites, scene play, and Linux player builds are **out of
  scope** until Unity license secrets are provided.
- All AI-generated images, videos, audio, and 3D assets must be saved in `generated_AIassets/` at the game root.
- Image generation: `gemini-3-pro-image-preview` (nano banana pro) at 1024×1024. Never `gemini-2.5-flash-image-preview`. Never request 2048×2048.
