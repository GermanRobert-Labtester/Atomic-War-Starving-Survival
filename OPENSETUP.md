# ASHFALL PROJECT — OPENSETUP Instructions
# AUTO-GENERATED from AGENTS.md (canonical source). Run sync-agent-rulebooks.py to regenerate.
# Last generated: 2026-09-05

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

## MCP CONNECTION REGISTRY — DO NOT REDISCOVER

The project owner already maintains the following MCP connections. Treat this section as the canonical routing registry for every AI client/agent working on ASHFALL. **Do not waste task time rediscovering, reinstalling, or re-authenticating these MCPs before first use.**

| Canonical MCP alias | Connection | Primary use in ASHFALL | Do not use for |
|---|---|---|---|
| `composio` | **Composio MCP** | Connected SaaS/tool actions, external workspace operations, integration workflows, and app-specific automation exposed through Composio | Core gameplay logic, local code/search when native repo tools are available, or storing credentials in the repo |
| `google-stitch` | **Google Stitch MCP** | UI/UX ideation, screen/layout generation, interaction mockups, visual variants, and design handoff for Godot UI work | Authoritative gameplay/data decisions, direct edits to `Ashfall.Core`, or replacing the project design system without review |

### MCP operating rules

1. **Assume both connections are preconfigured and authorized by the project owner.** Start with the requested MCP action/tool call; do not begin by searching the web, shell, dotfiles, or repository for connection details.
2. **Tool enumeration is allowed only when the current client requires it.** If an agent must call `list_tools`, `list_resources`, or equivalent to learn the exact exposed function names, do that once and proceed. That is capability discovery, not connection rediscovery.
3. **Never request, print, persist, or commit MCP credentials/tokens.** Secrets belong to the user's MCP/client configuration, never `AGENTS.md`, source files, logs, prompts, or JSON catalogs.
4. **Do not install duplicate MCP servers.** If `composio` or `google-stitch` is unavailable in a particular client, report the unavailable connection clearly. Only troubleshoot/reconnect after an actual failed invocation or explicit user request.
5. **Prefer the MCP over manual browser work when its domain matches the task.** Example: use Google Stitch for UI mockup/design generation instead of manually reconstructing a design service; use Composio for a connected external app workflow instead of asking the user to copy data between services.
6. **MCP output is not project authority.** Stitch designs are proposals until reconciled with the existing Godot theme, responsive layout rules, accessibility, and runtime state. Composio-returned external data must not silently override `Assets/StreamingAssets/Data/`.
7. **Respect task mode.** A READ-ONLY/audit task stays read-only even if an MCP can write. Never use an MCP write action to bypass repository or user constraints.
8. **Use the canonical aliases in plans and handoffs.** Refer to these connections as `composio` and `google-stitch` so downstream agents know which MCP is intended even if their client exposes a different internal tool prefix.

### MCP routing shorthand

- **UI concept / new screen / layout exploration** → `google-stitch` first, then implement approved output in Godot `src/UI/` / `.tscn` using existing theme/components.
- **External app / connected service / automation workflow** → `composio` first.
- **Repository code, tests, JSON authority, Godot scenes** → native repo/editor tools first; MCP only when it adds a specific external capability.
- **Verification** → always the canonical `dotnet` + `godot --headless` pipeline below; MCP output never substitutes for tests.

### STITCH UI HANDOFF — Greenhouse supply actions (Plan 22) — DO NOT REDISCOVER

The Greenhouse panel (`src/UI/GreenhousePanel.cs`) has **live host APIs with
no UI**: seed selection (PLANT hardcodes tubers), soil AMEND, drip-chain
install/filter, pest-protection deploy, shade cloth, STERILIZE (grow-medium
clear), water quantity/tainted choice, a supply-stock strip, and
readiness columns. All backing methods exist and are tested (Plan 22,
`--greenhouse-selftest` 89/89). Agents asked to build/generate this UI must
use **`google-stitch`** with the ready-made handoff spec:

- **Spec:** `docs/ui/GREENHOUSE_UI_GAP_SPEC.md` — 8 gap register entries,
  each with the exact host API, state bindings, action routes, theme tokens
  (hex), component helpers, tone-anchored state copy, and paste-ready Stitch
  prompt skeletons.
- **Rules:** Stitch output is a layout proposal only — implement through
  `AshfallUiHelpers`/`AshfallStatusRail`/`AshfallDataGrid`, extend
  `Main.HandleGreenhouseAction`'s switch with the gap's action string, keep
  `LastEvent` as the single feedback strip, and never render raw item IDs.
  Reconcile with the runtime theme before merging; verify with
  `--greenhouse-selftest` + `ashfall-godot-scene-lint`.

### Missing UI panels — Google Stitch authority (via Antigravity)

**`google-stitch` is the design authority for every missing/stub UI panel.**
The Antigravity client holds the live MCP connection to Google Stitch —
route all screen/layout/interaction design for the panels below through it
(ANTIGRAVITY.md is the client bootstrap). A panel is "missing" when it either
does not exist or is a **stub**: untyped `Bind(object?)`, hardcoded flavor
text, no Core session binding. Stitch output remains a proposal until
reconciled with the runtime theme (`AshfallUiHelpers`/`DesignTheme`), the
`state → blocker → cost → consequence` panel standard, and Core binding
discipline (presentation-only).

Currently missing/stub (verified 2026-09-03 — replace entries as they are
bound to Core):

| Panel | Status |
|---|---|
| `ElectrostaticScrubberPanel` | missing (Plan 72 — create bound to VentilationSystem stage) |
| `AquiferTreatyConcessionPanel` | stub — hardcoded text, no Core binding |
| `BasalRadonMigrationPanel` | stub |
| `ClandestineInsurgencyPanel` | stub |
| `CrossingSafeConductVouchPanel` | stub |
| `CryogenicPermafrostCorePanel` | stub |
| `FungalProteinFermenterPanel` | stub |
| `HeavyMarineDieselGeneratorPanel` | stub |
| `InductionCupolaFurnacePanel` | stub |
| `IronCenotaphMemorialPanel` | stub |
| `LongWalkExpeditionPanel` | stub |
| `MagneticDrumArchivePanel` | stub |
| `MechanicalProstheticsLathePanel` | stub |
| `SonicRuptureDrillPanel` | stub |
| `SubterraneanDebtLedgerPanel` | stub |
| `SurfaceShrapnelAegisPanel` | stub |
| `TraumaBondingCohortPanel` | stub |
| `TroposphericRadioRelayPanel` | stub |
| `UltrasonicDecontaminationAirlockPanel` | stub |
| `VaultDoorBreachingPanel` | stub |

Bound panels are exempt (e.g. `SlurryDewateringSumpPanel`, `SumpFloodingPanel`,
`WeatherSondePanel`, `RailwayTerminalPanel`). When a stub gains a Core
binding, remove it from this table in the same commit.

### Failure policy

If an MCP invocation fails because the server/tool is missing, disconnected, or auth-expired:

1. record the exact failure concisely;
2. do not repeatedly probe alternate endpoints;
3. continue with non-MCP work if possible;
4. ask for reconnection only when that MCP capability is actually required to finish the task.

---

## STACK

| Layer            | Engine / Format             | Location                                  | Namespace           | Target          |
|------------------|-----------------------------|-------------------------------------------|---------------------|-----------------|
| **Core** (truth) | Engine-agnostic C#          | `Assets/Ashfall.Core/`                    | `Ashfall.Core.*`    | `netstandard2.1` |
| **Godot host** (active, only editor) | Godot 4.7+ (.NET/C#) | `src/`                                     | `AtomicWar.GodotApp.*` | `net8.0`     |
| **Unity host** (removed — migration complete) | Unity 6 LTS, 2D, URP | `Assets/_Game/` (deleted)         | `AtomicWar._Game.*` | — (do not run) |
| **Tests**        | xUnit                       | `Ashfall.Core.Tests/`                     | `Ashfall.Core.Tests`| `net9.0`        |
| **Data authority**| JSON                       | `Assets/StreamingAssets/Data/`            | —                   | —               |
| **Godot Bridge** (shim) | **REMOVED** — migration complete | `src/Bridge/` (deleted) | — | — |
| **Godot assets** (imported/migrated) | Godot native (`assets/`) | `assets/art/`, `assets/audio/`, `assets/fonts/`, `assets/sprites/`, `assets/ui/` | — | — |

Godot project: `project.godot` at root, `gl_compatibility` renderer, 1920×1080, 60 FPS, `BarlowCondensed` + `ShareTechMono` fonts.

### SDK & Target Framework Requirements (.NET 8 Host / .NET 9 Tests)
- **`global.json`**: Root workspace configuration pins baseline SDK `8.0.100` with `rollForward: latestMajor` and `allowPrerelease: false`.
- **Godot Host (`Ashfall.csproj`) & Core Library (`Ashfall.Core.csproj`)**: Targets **`net8.0`** (and `netstandard2.1` compatibility), guaranteeing full compatibility with Godot 4.7+ .NET Mono runtime without runtime version mismatch.
- **Unit & Determinism Tests (`Ashfall.Core.Tests.csproj`)**: Targets **`net9.0`** (with `RollForward: LatestMajor`), required for high-throughput xUnit execution and modern runtime determinism in simulation suites.
- **Environment Prerequisites**: Development environments require .NET 9+ SDK (which builds both `net8.0` host and `net9.0` test assemblies via `global.json`'s `latestMajor` roll-forward) or side-by-side .NET 8 + .NET 9 SDK installations as configured in CI.

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

**Remaining debt:** The Unity legacy asset tree (`Assets/art/` ~2080 files, `Assets/sprites/`, `Assets/ui/`, `Assets/audio/radio/`) still lives under the Unity-style `Assets/` tree instead of the Godot root `assets/` tree. Migration direction remains Unity → Godot, but the work is now asset porting (not scene/prefab porting).

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

| Interface       | Purpose                       | Godot adapter          |
|-----------------|-------------------------------|------------------------|
| `IJsonSerializer` | JSON serialize/deserialize   | core default           |
| `IFileIO`       | File/directory access         | core default           |
| `ILog`          | Info/Warn/Error logging       | `GodotLog`             |
| `IClock`        | Day counter                   | core default           |
| `ISeededRng`    | Deterministic PRNG            | `CoreSeededRng`        |
| `IEventBus`     | Bounded radio/intercept bus   | `SimpleEventBus` (strictly bounded; see `docs/architecture/EVENT_SURFACE.md`) |

`IClock` (day-level whole days) and `ISimClock` (tick-based intraday resolution) are intentionally decoupled by architectural directive: DO NOT MERGE. Governed by `docs/architecture/CLOCK_POLICY.md` and verified by `ClockPolicyTests`.

### Invariant 3 — Cross-host save compatibility
A save written by one host must load in the other. **Unity host removed** — save compatibility is now Godot-only. The `SaveWireContract` tests (7 tests) pin the JSON shape and `SaveChecksum` hash for the Godot host. All save stores ship checksummed envelopes; the legacy `JsonUtility` path is gone with `_Game/`.

### Invariant 4 — Determinism
Same seed ⇒ identical simulation in both engines. Use `ISeededRng` (xorshift64*). Never `System.Random`. Never `Guid.NewGuid()`. Strongly typed C# events (`event Action<T>`) are the canonical event default (see `docs/architecture/EVENT_SURFACE.md`).

Known offenders (fix when touching these):
- ~~`Assets/Ashfall.Core/FinalWishSystem.cs:66` — `public System.Random Rng;`~~ — **RESOLVED** (now uses `ISeededRng`)
- ~~`Assets/Ashfall.Core/CombatTraumaSystem.cs:53` — `public System.Random Rng;`~~ — **RESOLVED** (now uses `ISeededRng`)
- ~~`Assets/Ashfall.Core/WeatherSystem.cs:144` — `new Random(unchecked(...))`~~ — **RESOLVED** (now uses `SeededRng`)
- ~~`Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs` — `Guid.NewGuid()`~~ — **RESOLVED** (deterministic id path; documented at line ~48; sequence state pinned by `ProceduralItemInstanceDeterminismTests`)
- ~~`InMemoryFlagLedger` OrdinalIgnoreCase drift~~ — **RESOLVED** (`Normalize` + `StringComparer.Ordinal` in `Flags/IFlagLedger.cs`; pinned by `FlagLedgerDeterminismTests`)
- ~~Unsanctioned `System.Random` / `new Random(` / `Random.Shared` / `Guid.NewGuid` / `DateTime.Now` / `DateTime.UtcNow`~~ — **RESOLVED & MECHANICALLY GATED** (Plan 44: gated by `Ashfall.Core.Tests/DeterminismGuardTests.cs`; non-gameplay call sites documented via `DETERMINISM_ALLOWLIST:`; verified across Core and src)

### Invariant 5 — No gameplay logic in hosts
Thin MonoBehaviours (Unity) and thin Nodes (Godot) handle only presentation, input, and wiring. Gameplay lives in plain C# systems inside `Ashfall.Core`.

Known offenders (do not grow these; migrate logic into Core instead):
- ~~`Assets/_Game/Quests/PersonalQuestSystem.cs` (4936 lines)~~ — **RESOLVED** (deleted with `_Game/`)
- ~~`Assets/_Game/Medical/MedicalSystem.cs` (1287 lines)~~ — **RESOLVED** (deleted with `_Game/`)
- ~~`Assets/_Game/Survivors/SurvivorWorkShiftSystem.cs` (1291 lines)~~ — **RESOLVED** (deleted with `_Game/`)
- ~~`Assets/_Game/Economy/DynamicEconomySystem.cs` (1797 lines)~~ — **RESOLVED** (deleted with `_Game/`)
- ~~`src/Host/HoldfastRuntimeSession.cs` duplicates core survival mechanics~~ — **RESOLVED** (thin projection: `Health/Hunger/Thirst/Radiation` read from `SurvivorsHostSession` via `NeedsSystem`/`RadiationSystem` at `src/Host/HoldfastRuntimeSession.cs:44`; `TickDay:164` fallback decay only when `Survivors==null` for headless tests)

### Invariant 6 — Data authority is JSON
`Assets/StreamingAssets/Data/` is the authority. ScriptableObjects are a Unity-editor convenience generated from JSON, never the source. Never fork data per engine.

Known data issues:
- ~~121 ScriptableObject definitions — risk of dual authority~~ — RESOLVED (0 ScriptableObjects remain; see H12)
- 56 narrative JSON files are **untracked in git** — missing on fresh clone (`Assets/StreamingAssets/Data/narrative/`)
- Property naming mixes `camelCase` and `snake_case` — migrate to `snake_case` (migration notes filed per file; rename deferred to a follow-up task — see A11 parity audit)
- ~~Only 35 of ~280 JSON files have `schema_version`~~ — RESOLVED: all 411 data JSON files (137 root catalogs + 272 narrative + whitelists/documents) carry a top-level `schema_version`; presence is now enforced by `CatalogIntegrityValidator` (a root-object catalog missing it fails `--data-integrity-selftest`), gated by `CatalogIntegrityValidatorTests` (missing→error, present→pass, bare-array root exempt).
- ~~`world_history.json:15` references "China"~~ — RESOLVED: replaced with a fictional nation ("the Meridian Compact"); all real-country/alliance terms swept from the data authority and gated by `Ashfall.Core.Tests/DataRuleComplianceTests.cs` (no real countries/wars/people).

---

## BRIDGE SHIM — REMOVED

The `UnityEngine.*` compatibility shim (`src/Bridge/`) and the legacy `Assets/_Game/` host have been **fully deleted**. Migration to Godot is complete; there is nothing left to shim. `--bridge-selftest` is retained as a stable CI verb: it prints the removal notice and exits 0 rather than booting into the app loop. Do not reintroduce a `UnityEngine.*` shim layer.

`--expedition-encounter-bridge-selftest` is unrelated to the old shim: it smoke-tests the live `ExpeditionEncounterBridge` domain class (bare-notice + resolved surface paths).

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
- ~~Known gaps (5 Godot save stores lack checksum)~~ — RESOLVED: `ExpeditionSaveStore`, `MedicalSaveStore`, `NarrativeSaveStore`, `WorldSaveStore`, `JournalSaveStore` all ship checksummed envelopes and require a non-empty `Checksum` field in the new format. Pre-checksum bare-state saves still load via the legacy fallback path. Integrity contract pinned by `Ashfall.Core.Tests/SaveStoreChecksumSweepTests.cs` (12 tests, 3 per store: clean round-trip, mutated-state changes hash, null checksum rejected).
- ~~3 further bare stores (Weather, HostEvent, ChemicalDependency)~~ — RESOLVED: sealed with the same `ExpeditionSaveStore` envelope pattern (`{ State, Checksum }` + legacy bare-state fallback). Contracts pinned by `Ashfall.Core.Tests/BareSaveStoreSealTests.cs` (12 tests, 4 per store incl. legacy bare-state load). Regression-proofed by `Ashfall.Core.Tests/SaveStoreCoverageGateTests.cs`, which source-scans every `src/**/*SaveStore*.cs` and fails CI if any store has neither a checksum envelope nor Core-codec delegation — a bare store can no longer ship silently.
- **Known issue — stricter load guard:** `NarrativeSaveStore.TryLoad` and its three sibling stores now reject a new-format envelope whose `Checksum` field is null or empty (`checksum field missing (corrupt save)`). The old guard `!string.IsNullOrEmpty(envelope.Checksum)` silently treated a missing checksum as "legacy", which is wrong: a malformed save in the new format is not legacy. The bare-state fallback only fires for genuinely pre-checksum saves.
- ~~`JournalSaveStore` bypasses core `IJsonSerializer`~~ — RESOLVED: serializes via `SystemTextJsonSerializer` (core adapter, `HostDefaults.cs`), the same path as every other host store; contract pinned by `SaveStoreChecksumSweepTests`.
- **Initiative #41 — generic persistence service (complete):** every host save store (all `*SaveStore*.cs` files plus the stores embedded in `*HostSession.cs` files) is now a thin static façade over the injected Core `SaveStore<T>` service (`Assets/Ashfall.Core/Save/SaveStore.cs`, built on `SaveEnvelopeHelper`) via `SaveStoreHub` (`src/Host/SaveStoreHub.cs`, which injects `FileSystemIO`/`SystemTextJsonSerializer`/`GodotLog` + the `SaveSlotRoot` base-dir router, re-resolved per operation). The service owns the checksum envelope, **atomic writes** (temp+rename; the one deliberate behavior change), optional `.bak` rotation, path overrides, and per-section legacy-bare-state fallback (`allowLegacyBareState:false` for sections that dropped their pre-envelope format). Core codecs plug in via `FromCodec` encode/decode delegates; the 12 shelter-batch sections with legacy `{SchemaVersion, State, Checksum}` property envelopes keep their exact on-disk shape via `SchemaVersionedEnvelope<T>`. On-disk formats are byte-preserved (pinned by `Ashfall.Core.Tests/Save/SaveStoreServiceTests.cs`). `SaveStoreCoverageGateTests`, the `--save-store-checksum-selftest` Gate A, and the save-store matrix generator now **require** delegation (SaveStoreHub / SaveEnvelopeHelper / Core codec) — hand-rolled envelope boilerplate fails CI.
- **Initiative #42 — single versioned atomic campaign envelope (complete):** `SaveAll` no longer writes ~61 section files. Every `SaveXxx` captures its section's persisted bytes in memory (`SaveStore<T>.CapturePersisted` — byte-identical to the old file format) into a payload map; `CampaignEnvelopeBuilder` (`Assets/Ashfall.Core/Save/`) packs it into ONE registry-keyed, checksummed, atomic `campaign.json` per slot (`manifestVersion` 2). A failed capture aborts the whole save — cross-system partial saves are structurally impossible. Loads validate the envelope, migrate V1 (filename-keyed) envelopes in memory via the registry filename→key map (reserved `legacy` import section preserved; strays dropped), and explode sections to their registry file names so the `SetupXxx` flows are unchanged. Continue with no slots auto-migrates pre-slot global section files verbatim into a fresh `migrated_N` slot. `SaveSectionRegistry.SectionFileNames` is the single authority for section file names (whitelist, migration, registry-derived reset lists). Envelope contract pinned by `Ashfall.Core.Tests/Save/CampaignEnvelopeBuilderTests.cs` and the 7-gate `--save-load-ui-failure-selftest`.
- **Task #101 — expedition vehicle & weapon-condition logistics (complete):** `ExpeditionSystem` accepts an `ExpeditionVehicleProfile` at `Start` (speed multiplier, cargo capacity, per-travel-tick breakdown chance, fuel-per-tick); travel steps multiply while the vehicle runs and a seeded per-tick roll can break it down mid-route (reverts to foot speed/capacity, `OnVehicleBreakdown`). Pure `ExpeditionSystem.Estimate` mirrors the tick math for the UI (ticks, fuel, capacity, breakdown + readiness-adjusted encounter risk). `WeaponEquipmentBridge` (`Ashfall.Core.Combat`) projects the persisted `EquipmentConditionSystem` weapon instances into combat `WeaponInstanceState` tokens (0–100 ↔ 0–1) and writes engagement wear back at encounter end — one persisted condition per weapon, no new durability authority (canon rule respected). `ExpeditionHostSession` owns the garage (`ExpeditionVehicleSystem`, deterministic seed, starter quad, fuel gate + prep on dispatch, refuel/repair); the expedition save section is now the aggregate `ExpeditionAggregateState` (sorties + garage) with legacy envelope/bare-list migration; `vehicles.json` is the data authority. Pinned by `ExpeditionVehicleLogisticsTests` (13) + the 9 vehicle gates in `--expedition-selftest`.
- ~~`LocationEvolutionSaveable`, `WildlifeSaveable`, `LandmarkSaveable` have empty `CaptureState/RestoreState`~~ — **CORRECTED (2026-08-27 audit):** no such classes exist; the real systems (`LocationEvolutionSystem`, `WildlifeMigrationSystem`, `LandmarkDegradationSystem`) have functional capture/restore and persist as sub-fields of `WorldHostSave` inside the world section.

---

## EVENT SYSTEM

Two parallel buses (architectural debt; full merge deferred — audit #32 disposition):

| Bus                              | Style                            | Where it's real       |
|----------------------------------|----------------------------------|-----------------------|
| `IEventBus` / `SimpleEventBus`   | String-based, constructor-injected | Verdict radio/census + DiveInstanceRunner (+ HostEventAdapter). Not a general gameplay bus. |
| ~~`EventBus` static class~~ — **REMOVED** (Unity host deleted) | Type-safe generics, allocation-free, editor profiling | Unity host deleted with `_Game/` |
| Godot                            | No bus — direct method calls / C# events on host sessions | Primary pattern |

Rule: every public system raises C# events on state change (for UI + save). Prefer typed C# events; do not expand `IEventBus` string topics without an explicit migration plan.

---

## MICRO-LOCATION INTEGRATION CONTRACT (F17–F20)

Micro-locations (`Assets/StreamingAssets/Data/micro_locations.json`) resolve through `NarrativeEncounterSystem.TryResolve`, which returns a consequence payload the host applies **in this fixed order**: item delta → journal unlock → location discovery → world flag → hazard routing. Effects must flow into the subsystem that already owns the mechanic — never a parallel `MicroLocationXxxSystem`.

- **Hazard flags** (e.g. `micro_contamination_exposure` → `disease_zoonotic_flu`) route through `MicroLocationHazardRegistry` (`Assets/Ashfall.Core/Narrative/`) into the owning disease authority. Every authored `setWorldFlag` must have a registered hazard consumer or sit on the reviewed-inert list in `MicroLocationIntegrationDeterminismTests` — a flag with no consumer fails the suite.
- **`seed_packets` is plantable** through the canonical `GreenhouseExpansionCatalog.CropCatalog` (13 crops; mixed packet → tuber profile). Never add a micro-location-specific planting exception.
- **One-shot discipline:** depleting choices exhaust the site (Core F1); hazard consequences fire only on the flag's unset→set transition plus `DiseaseSystem.Infect`'s own already-infected no-op. Deterministic: no new RNG in the resolution or hazard path.

Authoritative per-task docs: `docs/discovery/MICRO_LOCATION_HAZARDS.md`, `MICRO_LOCATION_GREENHOUSE.md`, `MICRO_LOCATION_RADIO.md`, `MICRO_LOCATION_WATER.md`. Known pending extension: discovery selection has no season/drought/skill context yet — see `docs/plans/PLAN_F21_DISCOVERY_SELECTION_CONTEXT_EXTENSION.md`.

---

## DATA INTEGRITY

`Assets/Ashfall.Core/CatalogIntegrityValidator.cs` (603 lines) — five-tier validation:

1. **REGISTRY** — every definition-position id with file + JSON path.
2. **TIER-1** — strings with a known snake_case prefix (200+ prefixes) must resolve.
3. **TIER-2** — values at known reference keys (`resultItemId`, `requiredItemId`, etc.) must resolve.
4. **RANGES** — `minDay`/`maxDay` pairs must be ordered.
5. **UNIQUENESS** — no duplicate definition ids within one file.

Run with: `godot --headless --path . -- --data-integrity-selftest` (must report 0 errors).

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
| C1 | ~~`JsonUtility` in Unity SaveSystem blocks cross-host saves~~ — **RESOLVED** (Unity host removed) | Unity SaveSystem deleted with `_Game/`; `SaveWireContract` tests confirm Godot-only save compatibility |
| C2 | ~~`System.Random` breaks determinism~~ — RESOLVED                                 | migrated to `ISeededRng`; verified by `Ashfall.Core.Tests` |
| C3 | ~~`Guid.NewGuid()` breaks determinism~~ — RESOLVED                                 | comment at `Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs:48` documents the fix |
| C4 | ~~56 narrative JSON files untracked in git~~ — RESOLVED                            | 196/196 narrative JSON files now tracked |
| C5 | ~~`HoldfastTradeSessionTests.cs` — 10 compile errors, stale API~~ — RESOLVED      | 3/3 tests pass against current API |
| C6 | ~~28 catalog loaders use `JsonUtility`~~ — **RESOLVED** (migrated to Core) | 10 `*CatalogLoader.cs` files now in `Assets/Ashfall.Core/`; all use `SystemTextJsonSerializer` (core default) |

### High

| #  | Issue                                                              | Location                                                         |
|----|--------------------------------------------------------------------|------------------------------------------------------------------|
| H1 | ~~`HoldfastRuntimeSession` duplicates core survival mechanics~~ — **RESOLVED** (thin projection onto `NeedsSystem`/`RadiationSystem` via `SurvivorsHostSession`; fallback `_fallback*` only for headless tests) | `src/Host/HoldfastRuntimeSession.cs:44` (`Health`/`Hunger`/`Thirst`/`Radiation` project via `Survivors?.Find()`; `TickDay:164` fallback decay only when `Survivors==null`) |
| H2 | ~~Duplicate `WornGear` class~~ — **RESOLVED** (consolidated)         | Radiation uses `Inventory.WornGear` directly (`using InventoryWornGear = Ashfall.Core.Inventory.WornGear` in `RadiationSystem.cs`); `FromInventory` removed. Host path is `Inventory.FillWornGear` via `SurvivorsHostSession` (gas mask/hazmat cuts dose; `--survivors-selftest` + `InventoryGearBridgeTests`). |
| H3 | ~~`SimClock` duplicate~~ — **RESOLVED & POLICY-ENFORCED** (Plan 45) | Non-merge directive in `docs/architecture/CLOCK_POLICY.md`: `IClock` (day-based) and `ISimClock` (tick-based) serve decoupled horizons. Verified by `ClockPolicyTests` (Verdict 7-day 03:00 cadence, Warlord daily idempotency, tick-day conversion, and replay). |
| H4 | ~~13 bare `catch { }` blocks swallow exceptions~~ — **RESOLVED** | `YearOfAshCatalogLoader.cs` (7) + `VerdictCatalogLoader.cs` (3) — zero bare `catch { }`; every parse failure routes through `CatalogDiagnostics.Warn(path, shape, ex)` → an injectable `ILog` sink (default `ConsoleLog`, overridable via `RegisterLog`), carrying file path + attempted JSON shape. Missing optional files stay silent-empty by design (not a failure). Regression coverage in `Ashfall.Core.Tests/CatalogLoaderHardeningTests.cs` (6 tests: malformed→logged for both loaders, valid→baseline, missing→silent-empty). |
| H5 | Utility AI forked — **Core vs Godot host** (not Unity)             | `Assets/Ashfall.Core/UtilityAI/` vs `src/UtilityAI/` (Godot host) |
| H6 | ~~Unity has no `IFileIO`, `IJsonSerializer`, `IClock` adapters~~ — **RESOLVED** (Unity host removed) | Unity host deleted with `_Game/` |
| H7 | `Main` host sprawl — ~19.8k lines across **74** `Main*.cs` partials | Triad pattern holds (`SetupXxx` / `SaveXxx` / optional `FlushXxxIfDirty`); SaveAll enrolls registered sections. Drift gated by `MainTriadDriftGateTests` (audit #28/#29). Full decomposition remains deferred (`ashfall-decompose-godot`). |
| H8 | ~~`SettingsManager` uses `PlayerPrefs` (Unity-only)~~ — **RESOLVED** | Unity `SettingsManager.cs` deleted with `_Game/` |
| H9 | ~~124 compiler warnings in tests~~ — RESOLVED                       | test suite builds with 0 errors, 3 minor analyzer warnings (xUnit2013/xUnit2020) — not nullable refs |
| H10 | ~~NeedsSystem & RadiationSystem save/load round-trip tests~~ — **RESOLVED** | `NeedsRadiationSystemTests.cs` covers tick behaviour (58 tests); `NeedsRadiationSaveRoundTripTests.cs` now adds save/load round-trip coverage (17 tests): all-fields round-trip + restored-state-drives-tick, capture→tick→differs no-op guard, default/empty restore with documented defaults, checksum stability, paired capture/restore determinism for both systems, and the Core no-projection half of the `HoldfastRuntimeSession` `Survivors==null` fallback (`HoldfastRuntimeSession.cs:177`). Fallback-decay math itself lives in the Godot host (Godot.NET.Sdk/net8.0), not referenceable from the net9.0 test project, and is covered by host integration tests. |
| H11 | ~~JournalSystem coverage~~ — **RESOLVED** (Plan 43)                 | Full coverage closed by Plan 43 across all 6 Core journal files (`JournalSystemTests`, `JournalSystemCoreBehaviorTests`, `JournalProducerIntegrationTests`): lifecycle, newest-first, dedup, dual discovery contract, eviction at 64, tabs, object recycling, event suppression on restore, trait-driven `JournalVoice`, `ProceduralEulogyEngine`, and producer integrations (Autopsy, LibraryStudy, MoralChoice). |
| H12 | ~~ScriptableObject definitions~~ — **RESOLVED** (migrated to JSON) | 0 ScriptableObjects remain; all data authority now in `Assets/StreamingAssets/Data/` JSON files |

---

## NAMESPACE CONVENTIONS

| Layer   | Namespace                                              | Directory match |
|---------|--------------------------------------------------------|-----------------|
| Core    | `Ashfall.Core`, `Ashfall.Core.Economy`, `Ashfall.Core.Journal`, … | ✅ |
| Unity (removed) | — | ✅ (deleted with `_Game/`) |
| Godot   | `AtomicWar.GodotApp`, `AtomicWar.GodotApp.Economy`, …  | ✅ (`AtomicWar.Journal` is the one legacy exception) |
| Tests   | `Ashfall.Core.Tests`                                   | ✅ (flat)       |

---

## VERIFICATION CHECKLIST (run after every task)

Report PASS/FAIL for each before claiming done.

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Must compile cleanly
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # All tests pass
3. dotnet build Ashfall.csproj                                  # Godot host: 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest       # Catalog integrity: 0 errors
5. godot --headless --path . -- --bridge-selftest               # Exits 0 (shim removed; kept as stable CI verb)
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
| `Assets/`   | Unity legacy (migrated: `Ashfall.Core`, `StreamingAssets/Data`; `_Game` deleted) |
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

---

## AI CLIENT / CLOUD RUNNER RULE

All AI clients and cloud runners — Cursor, Claude, Gemini, Codex, local agents, or others — follow the same project authority:

- active engine: **Godot 4.7+ C#**;
- verification: **`dotnet` + `godot --headless`**;
- Core truth: **`Assets/Ashfall.Core/`**;
- data truth: **`Assets/StreamingAssets/Data/`**;
- legacy Unity tree: **read-only unless the user explicitly requests Unity work in that message**;
- MCP routing: use the canonical `composio` / `google-stitch` registry above instead of rediscovering connections.

Client-specific bootstrap instructions must never override the non-negotiable project rules in this file.
