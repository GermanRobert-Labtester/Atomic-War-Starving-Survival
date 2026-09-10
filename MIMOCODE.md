# ASHFALL PROJECT — MIMOCODE Instructions
# AUTO-GENERATED from AGENTS.md (canonical source). Run sync-agent-rulebooks.py to regenerate.
# Last generated: 2026-09-10

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

The historical Greenhouse handoff below is **not current implementation
evidence**. The 2026-09-05 UI audit found seed selection, water choices,
supply/readiness presentation already present, but all three water payloads
decode incorrectly to clean 50-unit watering (`UI-20`). Amendment,
maintenance and sterilization API claims must be checked against the
current, trimmed host before implementation. Do not repeat the old
“PLANT hardcodes tubers / all eight APIs already exist” claim without
source verification. Agents asked to build/generate missing UI must use
**`google-stitch`**, reconciling the historical handoff with current source:

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
does not exist or is a **stub**: ignored binding, hardcoded operational
state, empty refresh, or missing domain commands. `Bind(object?)` alone is
not proof of a stub: ElectrostaticScrubberPanel casts and subscribes to a
real VentilationHostSession. Stitch output remains a proposal until
reconciled with the runtime theme (`AshfallUiHelpers`/`DesignTheme`), the
`state → blocker → cost → consequence` panel standard, and Core binding
discipline (presentation-only).

Partial historical list, reconciled 2026-09-05. The complete **UI PANELS &
UX AUDIT** register later in this file supersedes old missing/bound claims.
Do not mark a panel complete from file creation or a typed field alone:

> **Plans 78–81 flagship batch (Wave 6, integrated 2026-09-05):**
> `DeconAirlockPanel`, `GeodeticSurveyPanel`, `KineticStoragePanel`,
> `ChemicalReconPanel`. The four Core systems exist
> (`DecontaminationSystem` protocol/effluent extension, `GeodeticSurveyEngine`,
> `KineticStorageSystem`, `ChemicalReconEngine`) with catalogs
> `decontamination_protocol_catalog.json`, `geodetic_survey_catalog.json`,
> `kinetic_flywheel_catalog.json`, `toxic_chemical_catalog.json` (all
> `GAMEPLAY_CONSUMED` in the utilization baseline). Binding + adapter
> integration completed: panels Bind on first OPEN (`Main.World.cs`), action
> adapters pass typed arguments (queue-resolved decon case, monument-derived
> survey triples, class-rate flywheel power, observation-derived sample
> location), and the two missing Core commands now exist —
> `KineticStorageSystem.EngageEmergencyBrake` and
> `ChemicalReconEngine.SelectFilterCategory`. See UI-11/UI-12 resolution
> notes below; re-audit pending per audit policy.
> **Historical Stitch prompts + binding contracts to reconcile:
> `docs/ui/PLANS_78_81_UI_STITCH_SPEC.md`** (Greenhouse-spec format).

| Panel | Status |
|---|---|
| `ElectrostaticScrubberPanel` | exists with real VentilationHostSession binding; duplicate construction and missing normal player route (UI-13) |
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

Real bound implementations include `SlurryDewateringSumpPanel`,
`SumpFloodingPanel` and `WeatherSondePanel`; sharing helpers does not make
them stubs. Remove a missing/stub entry only after its domain state,
actions, normal route, lifecycle and feedback are verified, not merely
when a Bind method is added.

> **Plans 190–193 flagship batch (Wave 0 recon complete, 2026-09-06):**
> `AmputationTriagePanel`, `RailwayTerminalPanel`, `FungiCultivationBedPanel`,
> `JusticeTribunalPanel`. All four Core systems exist (`AmputationSystem`,
> `RailwaySystem`, `FungiCultivationSystem`, `JusticeSystem`) with catalogs
> `surgical_procedures.json`, `rail_network.json`, `underground_flora.json`,
> `wasteland_laws.json` (all `GAMEPLAY_CONSUMED`). Save stores
> (`AmputationSaveStore`, `RailwaySaveStore`, `FungiSaveStore`,
> `JusticeSaveStore`) follow the canonical `SaveStoreHub`/`SchemaVersionedEnvelope`
> pattern. Godot wiring is complete in `Main.Plans190_193.cs`
> (Setup/Save/TickDay + journal events). All four panels are **UI-07 stubs**:
> typed `Bind(XxxSystem)` casts to real Core systems, but `RefreshView` is
> empty and no domain commands, state display, or normal player route exists.
> Do not mark these panels complete until the UI-07 acceptance criteria are met.

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

---

## UI PANELS & UX AUDIT — 2026-09-05 — OPEN FINDINGS

This register records the owner's requested deep audit of missing, stubbed,
relabelled and over-shared panels. It supersedes historical UI maturity
claims above where they conflict with current source. **These findings are
not fixes or authorization to implement them.** Existing/concurrent work
was preserved; only audit documentation was changed.

**Owner requirement: do not copy/relabel one complete panel for unrelated
systems or items.** Shared theme, framing and low-level components are
allowed; the actual task body, selected entity, state, blockers, costs,
consequences and command flow must be domain-specific. A new title, color,
class name, typed field or screenshot does not complete a panel. Do not
replace missing specialist workflows with another generic multi-system
operations console.

Full evidence and ownership analysis:
[UI_PANELS_UX_FORENSIC_REPORT.md](docs/forensics/UI_PANELS_UX_FORENSIC_REPORT.md).
Every inventoried source and route:
[UI_PANELS_UX_INVENTORY.md](docs/forensics/UI_PANELS_UX_INVENTORY.md).
Contrast, overflow, focus and readability:
[ACCESSIBILITY_REPORT.md](docs/ui/ACCESSIBILITY_REPORT.md).

### Audited coverage and interpretation

- 178 panel-source files across src, including four expansion classes named
  AviationUI/ChemUI/LaborUI/PoliticsUI. The initial count was 174; four
  Plans146–149 files appeared/changed concurrently. This is a naming-based
  panel inventory, not the number of all UI components/scenes.
- All 141 descriptors reconciled: 112 declared Live, 29 shelved Prototype.
  Of 134 configured unique IDs, 23 are unregistered; plans_110_113 is Live
  with no action destination. Counts overlap classes; do not sum them as
  an invented total of missing screens.
- 19 near-identical hardcoded three-column prototypes; 10 additional
  fake-success prototypes; 11 typed one-label stubs; 11 incomplete
  expansion readouts; 6 atlases with inert action bars.
- Full static inventory and suspicious-family tracing, representative
  stored screenshots and headless checks—not 178 fresh visual playthroughs.
- **EXISTS ≠ COMPILES ≠ WIRED ≠ EXECUTES ≠ PLAYER-FACING ≠ VERIFIED.**
  Entries without a targeted defect in the inventory are not certified
  complete. Shared style alone is not evidence of a bad copy.

### Complete finding register

#### UI-01 — HIGH — Research, Standing Record and Muster atlases are relabeled faction screens

Affected: ResearchAtlasPanel; StandingRecordAtlasPanel; MusterAtlasPanel.

All three populate the same five fixed faction/trust rows, coalition values and faction dossiers under different domain headings. Binding a host does not replace BuildData. Research still renders Faction/Current/ΔTrust instead of a research dependency/progress workflow; Standing Record and Muster reuse the same body and mostly fixed status metrics. Registered Live and host-configured is not proof of discoverable navigation.

Evidence: src/UI/ResearchAtlasPanel.cs:BuildData/BuildGrids/RefreshDetail (477/289/408); src/UI/StandingRecordAtlasPanel.cs:BuildData/RefreshStatusRail/RefreshDetail (433/232/364); src/UI/MusterAtlasPanel.cs:BuildData/RefreshStatusRail (433/232); src/Main.PlayerSurfaces.cs:485; snapshots/research_atlas_default.png; snapshots/standing_record_atlas_default.png; snapshots/muster_atlas_default.png.

Required follow-up acceptance: Design domain-specific bodies against the existing Research, Standing Record and Muster owners. Share theme primitives, not faction datasets or unrelated workflow structure.

#### UI-02 — HIGH — Six atlas action bars are inert text, not commands

Affected: MapAtlasPanel; MaritimeAtlasPanel; MusterAtlasPanel; QuestsAtlasPanel; ResearchAtlasPanel; StandingRecordAtlasPanel.

BuildActionFixtureRows feeds non-selectable AshfallDataGrid rows even on bound screens. Labels such as Dispatch Sortie, Plot Waypoint, Accept, Abandon, Inspect and Schedule have no corresponding button activation or command dispatch. A visually complete action strip therefore promises unavailable interaction.

Evidence: src/UI/MapAtlasPanel.cs:BuildActionRows/BuildActionFixtureRows (355/449); src/UI/MaritimeAtlasPanel.cs:BuildActionRows/BuildActionFixtureRows (319/423); src/UI/QuestsAtlasPanel.cs:BuildActionFixtureRows (372); src/UI/{ResearchAtlasPanel,StandingRecordAtlasPanel,MusterAtlasPanel}.cs:BuildActionFixtureRows.

Required follow-up acceptance: Give each actual task a real command with selection, blocker, cost, consequence and authoritative result feedback. Do not make fake rows clickable without implementing the underlying contract.

#### UI-03 — HIGH — Quest and maritime atlases substitute authored examples for current progress

Affected: QuestsAtlasPanel; MaritimeAtlasPanel.

Quests hardcodes 3 active/5 available/11 completed/4 locked/0 failed/1 abandoned on the bound path; a fixed five Holdfast keys are labeled Active independently of actual quest status. Maritime buckets catalog sites by index modulo four, reports stage Sealed and decision None, and sums catalog oxygen budgets rather than showing a current expedition's remaining oxygen. These are not reliable operational views.

Evidence: src/UI/QuestsAtlasPanel.cs:RefreshStatusRail/BuildQuestRows (154/185); src/UI/MaritimeAtlasPanel.cs:RefreshStatusRail/RoomRowsFor (255 onward).

Required follow-up acceptance: Project quest progress and active dive-instance state. Distinguish catalog capacity/planning values from live remaining resources; render genuine empty/unknown states.

#### UI-04 — HIGH — Map atlas selection loses quadrant identity

Affected: MapAtlasPanel.

Three quadrant grids emit local row indices into the same selection handler. ResolveVisibleRow and FindLocation walk the complete location list rather than the clicked quadrant's filtered list. East/South row zero can therefore show North/global row zero's detail; null-sector filtering further changes offsets.

Evidence: src/UI/MapAtlasPanel.cs:OnRowSelected wiring/TileRowsFor/ResolveVisibleRow/FindLocation (130 onward/285/338/395).

Required follow-up acceptance: Carry a stable location ID from each rendered row to detail and actions; verify every quadrant, filtered row and empty state.

#### UI-05 — HIGH — Nineteen three-column consoles are near-identical hardcoded shells

Affected: AquiferTreatyConcessionPanel; BasalRadonMigrationPanel; ClandestineInsurgencyPanel; CrossingSafeConductVouchPanel; CryogenicPermafrostCorePanel; FungalProteinFermenterPanel; HeavyMarineDieselGeneratorPanel; InductionCupolaFurnacePanel; IronCenotaphMemorialPanel; LongWalkExpeditionPanel; MagneticDrumArchivePanel; MechanicalProstheticsLathePanel; SonicRuptureDrillPanel; SubterraneanDebtLedgerPanel; SurfaceShrapnelAegisPanel; TraumaBondingCohortPanel; TroposphericRadioRelayPanel; UltrasonicDecontaminationAirlockPanel; VaultDoorBreachingPanel.

These assert IsBound=true, ignore Bind(object? session), render fixed telemetry and expose non-close buttons without Pressed handlers. Normalizing comments, strings, class names, numbers, whitespace and selected color-token names produces two exact structural groups of eight and ten; Magnetic Drum is the nineteenth near-copy with a changed margin accessor. All nineteen are shelved Prototype routes, not completed gameplay. Eighteen still look up Margin at the wrong tree depth and throw during eager construction.

Evidence: src/UI/AquiferTreatyConcessionPanel.cs:IsBound/Bind/RefreshView/BuildLayout/CreatePanelFrame (20/34/45/111/161); corresponding members in all named files; src/UI/MagneticDrumArchivePanel.cs; Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs:137; src/Main.UiPanels.cs:BuildUserInterface; headless decon-airlock-uitest log.

Required follow-up acceptance: Keep them shelved until each has its own domain task design and real owner contract. Fix eager-construction health separately; recoloring, relabeling or assigning a host field does not complete a panel.

#### UI-06 — MEDIUM — Ten additional prototypes simulate success without changing state

Affected: AnaerobicBiogasDigesterPanel; SubterraneanCartographyPanel; UndergroundPrintingPressPanel; SiliconIngotSlicingPanel; GeothermalSteamTurbinePanel; WarDogKennelPanel; IsotopeSeparatorPanel; PlasmaArcSmeltingPanel; BoreholeSeismographPanel; HeavyLogisticsAirlockPanel.

These roughly 126–130-line prototypes have unconditional IsBound, no session Bind, empty RefreshView/Unbind and hardcoded telemetry. Buttons only ShowFeedback, including success-sounding results, without a Core mutation. Biogas reports fixed 38.2°C/96.5% and feeding output with no inventory operation. Prototype gating limits direct player exposure but these are missing workflows, not implemented consoles.

Evidence: src/UI/AnaerobicBiogasDigesterPanel.cs; corresponding IsBound/RefreshView/ShowFeedback members in all ten files; Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs:126.

Required follow-up acceptance: Resolve existing gameplay equivalents and owners before designing each unique workflow; do not promote simulated-success fixtures.

#### UI-07 — HIGH — Eleven typed panels are still a single label with an empty refresh

Affected: AmputationTriagePanel; ArchaeologyExcavationPanel; CeremonyFestivalPanel; ChemWarfareDefensePanel; CommsArrayTransceiverPanel; FungiCultivationBedPanel; JusticeTribunalPanel; RailwayTerminalPanel; RoboticsWorkshopPanel; SurvivorDowntimePanel; WinterFreezePanel.

Each 33-line implementation stores a typed system but renders only AshfallDashboardShell plus one generic label and Close; RefreshView is empty. There is no entity selection, task state, actionable blocker, cost, consequence or domain command. RailwayTerminalPanel is not a completed bound-panel exemption despite the previous AGENTS claim.

**2026-09-06 update — Plans 190–193 quartet:** `AmputationTriagePanel`,
`RailwayTerminalPanel`, `FungiCultivationBedPanel`, and `JusticeTribunalPanel`
now have **real typed Core bindings** (not `Bind(object?)`): each panel's
`Bind()` casts to the actual system type (`AmputationSystem`, `RailwaySystem`,
`FungiCultivationSystem`, `JusticeSystem`). Save stores, catalog loading,
and Main orchestration are complete. The panels remain UI-07 stubs because
`RefreshView` is still empty and no domain commands are wired — but their
backend is fully operational. The remaining seven panels in this list have
no such backend.

**2026-09-06 Plan 204 update — `FungiCultivationBedPanel` UI-07 body
implemented:** the panel now renders the full domain workflow (bed list with
phase labels, strain/growth/flushes/preparation/moisture/contamination/room
spore-load/bioluminescent-light readout) and wires ten real commands to the
Core system — PrepareSubstrate (cold/heated), CultivateSpores, WaterPlot,
HarvestPlot, PurgeToxicBloom, DisposeInfectedSubstrate (discard/burn/quarantine),
and DigNewBed (canonical `room_greenhouse_shelter`). Feedback mirrors Core
`ActionResult` with player-readable blockers; no raw IDs. Core additions
documented in `docs/shelter/PLAN_204_MUSHROOM_CULTIVATION_CLOSEOUT.md`;
test evidence in `Ashfall.Core.Tests/Farming/FungiCultivationPlan204Tests.cs`
(16 tests; full suite 9070/9070 PASS). Its UI-07 gap is closed for the fungi
domain; `AmputationTriagePanel`, `RailwayTerminalPanel` and
`JusticeTribunalPanel` remain empty-refresh stubs with operational backends.

**Plans 198–201 closeout (commit 231595b8, verified):** the four late-game
console stubs are now full domain bodies — `CeremonyFestivalPanel` (schedule,
atomic contribute, faction invite, truce/prep/materials state),
`ChemWarfareDefensePanel` (lane hazards, density tiers, abstract agent
profiles, decon dispatch), `CommsArrayTransceiverPanel` (tier/power/tuning,
lock progress, live orbital-window query, tune/upgrade/strategic-uplink),
`RoboticsWorkshopPanel` (chassis/logic/core/EMP/rogue state, reactivate,
program, repair). All mutations route through the Core systems plus the
canonical inventory (atomic bills), faction-stance trust and journal
feedback; the four panels are now constructed in `BuildUserInterface`
(closing a would-be NPE on their previously null fields). Verification:
`--plans198-201-uitest` (route → bind → visible → command → state delta →
feedback, exception-free, exit 0), 10760/10760 dotnet tests,
data-integrity 0 errors, content-utilization stage-4 for all four catalogs,
scene-binding 25/25. UI-07 is CLOSED for these four; the remaining six
(`AmputationTriagePanel`, `ArchaeologyExcavationPanel`, `JusticeTribunalPanel`,
`RailwayTerminalPanel`, `SurvivorDowntimePanel`, `WinterFreezePanel`) are
still empty-refresh stubs.

Evidence: src/UI/RailwayTerminalPanel.cs:Bind/RefreshView/_Ready (13/19/21); same full-file structure in all eleven named files; src/Main.PlayerSurfaces.cs:45; src/Main.Plans190_193.cs (full orchestration for all four).

Required follow-up acceptance: Treat all eleven as STUB/PARTIAL. For the four Plans 190–193 panels, implement against the existing Core APIs (see `Main.Plans190_193.cs` for the canonical `Ensure*()` constructors). Specify separate domain workflows and required Core projections before implementation for the remaining seven. **Plan 204 closed the fungi body (see update above); the other ten remain STUB/PARTIAL.**

**Plans 198–201 update:** `CeremonyFestivalPanel`, `ChemWarfareDefensePanel`,
`CommsArrayTransceiverPanel` and `RoboticsWorkshopPanel` now have real bodies
and command flows (see closeout note above) — UI-07 is closed for these four;
six panels remain STUB/PARTIAL.

#### UI-08 — HIGH — Eleven expansion readouts lack their management workflows

Affected: AviationUI; ChemUI; LaborUI; PoliticsUI; PrisonerPanel; StealthReadoutPanel; MutationTreePanel; NurseryPanel; FalloutPlumePanel; DesperationCrisisPanel; MercenaryBountyBoardPanel.

Most use the same shell/status rail/single multiline Label and read actual system state when refreshed, but expose no domain commands beyond Close. A readout is not a dispatch, policy, allocation, training or care workflow. Mercenary's board is thinner still: active-count text plus a fixed no-targets message and neutral guild text. These are partial readouts, not evidence that all corresponding Core mechanics are absent.

Evidence: src/UI/{AviationUI,ChemUI,LaborUI,PoliticsUI,PrisonerPanel,StealthReadoutPanel,MutationTreePanel,NurseryPanel,FalloutPlumePanel,DesperationCrisisPanel}.cs:_Ready/RefreshView; src/UI/MercenaryBountyBoardPanel.cs:RefreshView; src/Main.PlayerSurfaces.cs:45/511.

Required follow-up acceptance: Document the actual player decisions per domain and expose the existing command owners with selectable targets, blockers and costs; do not count a shared multiline label as a dedicated workflow.

#### UI-09 — HIGH — Twenty-three configured navigation IDs were never registered

Affected: expansion_fallout_plume; desperation_crisis; mercenary_bounty_board; archaeology_excavation; amputation_surgery; railway_logistics; fungi_cultivation; justice_tribunal; chem_warfare_defense; comms_array_transceiver; ceremony_ritual; robotics_assembly; survivor_downtime; winter_freeze; aviation; narcotics; forced_labor; politics; prisoners; stealth; mutation_tree; nursery; fallout_detail.

ConfigureActions returns false for an unknown descriptor, and the host ignores that return value. These 23 IDs cover 22 classes because Fallout has two IDs. Nine have dashboard AddNavButton entries, so the dashboard advertises routes that OpenPlayerPanel rejects as unknown. Registering them alone would expose the incomplete bodies in UI-07/UI-08.

**2026-09-06 update:** `amputation_surgery`, `railway_logistics`,
`fungi_cultivation`, and `justice_tribunal` now have operational Core
systems, save stores, and Main orchestration behind their stub panels.
Registration remains blocked on UI-07 completion (empty RefreshView).
**Plan 204 update:** `fungi_cultivation`'s UI-07 body is now implemented
(real commands, non-empty RefreshView — see UI-07 note), clearing its
registration blocker; the route registration itself is still a separate
pending host change (UI-09 contract work), and the other three panels
remain blocked.

**Plans 198–201 update (commit 231595b8):** `chem_warfare_defense`,
`comms_array_transceiver`, `ceremony_ritual` and `robotics_assembly` are
now registered as **Live** descriptors in `PanelRegistryBootstrap` and their
UI-07 bodies are implemented (see UI-07 closeout note). Route → bind →
visible → command → state delta → feedback is proven end-to-end by the
`--plans198-201-uitest` headless gate (5/5 segments PASS, exit 0,
exception-free). These four IDs are closed for UI-09; the remaining
nineteen remain unregistered.

Evidence: Assets/Ashfall.Core/UI/PanelRegistry.cs:ConfigureActions/Resolve (204/227); Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs; src/Main.PlayerSurfaces.cs:45–114/511–554; src/UI/GameDashboardPanel.cs:435–448; src/Main.GameFlow.cs:165; src/Main.Plans190_193.cs.

Required follow-up acceptance: Reconcile descriptor, binding, availability, entry point, destination and close path as one contract. Gate unfinished workflows instead of silently adding Live descriptors.

#### UI-10 — HIGH — A Live industrial route has no destination; four domains have no dedicated UI

Affected: plans_110_113; chlor-alkali synthesis; solar concentration; precision optics; ballistic shields.

The bootstrap registers plans_110_113 as Live, but no ConfigureActions destination or matching player panel exists. Main.Plans110_113 constructs, loads, ticks and captures four real host systems with no corresponding typed UI consumers. There is backend capability without an operational player surface.

Evidence: Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs:169; src/Main.PlayerSurfaces.cs; src/Main.Plans110_113.cs; src/Host/{ChlorAlkaliHostSession,SolarConcentratorHostSession,PrecisionOpticsHostSession,BallisticShieldHostSession}.cs; src/Main.GameFlow.cs:217.

Required follow-up acceptance: Plan four distinct task surfaces on the existing owners. Do not fix this by introducing another unrelated multi-system omnibus screen.

#### UI-11 — HIGH — Wave 6 panels exist but are not bound or normally reachable

Affected: DeconAirlockPanel; GeodeticSurveyPanel; KineticStoragePanel; ChemicalReconPanel.

All four now have substantial UI source, construction and OnActionRequested subscriptions. No production Bind call to these panel instances or normal registered navigation route was found. They start hidden; OPEN handlers and tests can set visibility directly, which does not bind state or make a player entry point. The old 'files do not exist' statement is stale.

Evidence: src/Main.UiPanels.cs:396–440; src/Main.World.cs:462–520; src/Main.Plans78_81.cs; src/Main.UiTests.Wave6.cs:10–47; src/UI/{DeconAirlockPanel,GeodeticSurveyPanel,KineticStoragePanel,ChemicalReconPanel}.cs:Bind; Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs.

Required follow-up acceptance: Complete authoritative binding, dependency setup, route, discovery and lifecycle first; retain four domain-specific designs and validate actions before promotion.

> **RESOLVED 2026-09-05 (pending re-audit):** panels now Bind on first OPEN
> via the action handlers (`Main.World.cs` — `_deconAirlockBound` et al.
> guards make Bind idempotent and order-independent of session setup);
> route/discovery/lifecycle unchanged (registry OPEN/CLOSE + OnClose).
> Action-argument correctness resolved under UI-12.

#### UI-12 — HIGH — Wave 6 action adapters contain wrong arguments and explicit no-ops

Affected: DeconAirlockPanel; GeodeticSurveyPanel; KineticStoragePanel; ChemicalReconPanel.

Decon emits a selected case ID but Main passes it to StartProtocolCycle's survivorId slot with empty gear and fixed contamination. Survey Observe supplies an empty target and fixed clear weather/skill; resolve does nothing. Flywheel EMERGENCY_BRAKE does nothing, while charge/discharge hardcode 1000 and 60. Chemical change_filter does nothing and sampling supplies an empty location. Several calls bypass host command wrappers and their LastEvent/StateChanged feedback; decon does explicitly mark its dirty flag, so not all persistence routing is absent.

Evidence: src/Main.World.cs:462–520; src/UI/DeconAirlockPanel.cs:365; Assets/Ashfall.Core/DecontaminationSystem.cs:StartProtocolCycle; Assets/Ashfall.Core/World/GeodeticSurveyEngine.cs:Observe; Assets/Ashfall.Core/Expeditions/ChemicalReconEngine.cs:CollectSample; src/Host/ChemicalReconHostSession.cs.

Required follow-up acceptance: Pin typed command arguments and before/after state tests, especially emergency actions. A callable manual-brake Core command was not found; resolve that seam explicitly instead of inventing UI-owned brake behavior.

> **RESOLVED 2026-09-05 (pending re-audit):** all four adapters now pass
> typed arguments — decon resolves survivor/gear/contamination from the
> selected queue case; survey observe derives the FROM monument from active
> monuments and resolve iterates active-monument triples (engine unlock is
> idempotent); flywheel CHARGE/DISCHARGE use class-rate `max_charge_kw`/
> `max_discharge_kw` (hardcoded 1000 kW removed); chemical scans use the
> engine's `activeSensorBand` and sampling resolves the location from the
> hazard's latest observation. The missing brake seam is now an explicit
> Core command: `KineticStorageSystem.EngageEmergencyBrake(instanceId)`
> (logged, takes rotor offline, released only by PerformMaintenance) exposed
> through `KineticStorageHostSession.EngageEmergencyBrake` with `LastEvent`
> feedback. `ChemicalReconEngine.SelectFilterCategory` resolves the
> change_filter no-op the same way.

#### UI-13 — HIGH — Existing specialist panels are stranded despite useful implementations

Affected: ElectrostaticScrubberPanel; GeothermalAquiferPanel; ChemicalLabPanel.

Electrostatic Scrubber already binds VentilationHostSession and invokes real stage controls; it is constructed twice across Main.UiPanels and SetupElectrostaticScrubberPanel, with no normal registered entry. Geothermal Aquifer now has a Setup-time Bind and real action buttons, but its OnActionRequested is not subscribed in construction and no normal route was found. ChemicalLabPanel has typed chemical-synthesis binding and operations but no Main construction/registration. File absence is the wrong diagnosis for all three.

Evidence: src/UI/ElectrostaticScrubberPanel.cs:Bind; src/Main.UiPanels.cs:BuildUserInterface; src/Main.ExpandedShelterSystems.cs:129–137; src/Main.ShelterInfrastructure.cs:SetupGeothermalAquifer (374–388); src/Main.UiPanels.cs:868–870; src/UI/GeothermalAquiferPanel.cs:105–120; src/UI/ChemicalLabPanel.cs; src/Main.ChemicalSynthesis.cs.

Required follow-up acceptance: Reuse each existing specialist implementation's legitimate domain work, then finish its single construction/binding/route/event/lifecycle contract. PharmaLab does not automatically replace retort chemical synthesis.

#### UI-14 — HIGH — New Plans 146–149 screens are partial readouts, not completed control workflows

Affected: EbPvdCoatingPanel; MicrofluidicDiagnosticPanel; MineFlailPanel; RailGrindingPanel.

These four files appeared/changed concurrently during the audit. At the final inspection they compile, are constructed and bound from BuildPlans146To149Panels, and have domain data projections. However, OnActionRequested is only declared, not emitted by domain controls; Main handlers implement OPEN only, and registry/player entry points are absent. Their shared sidebar/table/detail scaffold, hardcoded example values and raw machine IDs do not fulfill coating, diagnostics, clearing or grinding operations. This is an as-observed WIP finding, not an assertion about a future completed change.

Evidence: src/Main.UiPanels.cs:442; src/Main.Plans146_149.cs:140–224; src/UI/{EbPvdCoatingPanel,MicrofluidicDiagnosticPanel,MineFlailPanel,RailGrindingPanel}.cs; src/UI/EbPvdCoatingPanel.cs:RefreshView; Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs.

Required follow-up acceptance: Require four distinct state→blocker→cost→consequence workflows, real commands and normal routes. Re-audit the concurrent work before accepting any closeout claim.

#### UI-15 — MEDIUM — Three omnibus panels compress unrelated domains into one operations screen

Affected: Phase0Panel; Plans94To97Panel; Plans130To133Panel.

Phase0 explicitly groups ten systems under an internal phase label. Plans94To97 puts grain milling/storage, cryogenic separation and heliograph communications side by side; Plans130To133 does the same for powder metallurgy, NVIS communications, lyophilization and draisine rerailing. Real host-backed commands exist, so these are not all stubs, but the plan-number information architecture and compressed fixed layouts conflict with the owner's rejection of reusing one panel for unrelated tasks. Plans94's IsBound accepts any host while builders assume the others, and its first nonempty event can mask feedback from another subsystem.

Evidence: src/UI/Phase0Panel.cs:class summary/_Ready; src/UI/Plans94To97Panel.cs:IsBound/RefreshView/FirstEvent; src/UI/Plans130To133Panel.cs:_Ready/RefreshView.

Required follow-up acceptance: Give the domains dedicated workflows or explicitly distinct navigable task views while preserving shared styling and existing command owners. No new gameplay systems merely to split presentation.

#### UI-16 — HIGH — Overlay detection, dismissal and navigation maintain different incomplete lists

Affected: Global overlay lifecycle; WorkshopPanel; PharmaLabPanel; Phase0Panel; DeepCoastPanel; WeatherHistoryPanel; expanded shelter panels; Wave 6; specialist/new panels.

AnyOverlayPanelOpen covers far fewer controls than CloseAllOverlayPanels, and CloseAllOverlayPanels itself omits multiple constructed overlays. For example workshop, pharma, phase0, deep coast, weather history, water/kitchen expanded panels and newer panels are missing from the close list. OpenExpandedPanel does not repair this. Global Escape can treat an unlisted overlay as 'no overlay' and return to the menu when the panel does not consume the event; switching panels can leave an older overlay visible. This is source-proven list drift; every possible focus/stack combination was not replayed.

Evidence: src/Main.GameFlow.cs:AnyOverlayPanelOpen/_UnhandledInput (662/693); src/Main.PanelLifecycle.cs:CloseAllOverlayPanels (9–61); src/Main.PlayerSurfaces.cs:OpenExpandedPanel; src/Main.Application.cs:_UnhandledKeyInput.

Required follow-up acceptance: Use one authoritative lifecycle contract for visibility, topmost focus, back/close and navigation. Verify each route with keyboard/controller cancel and multiple-overlay transitions.

#### UI-17 — HIGH — Dashboard navigation exceeds the supported canvas before its extra controls

Affected: GameDashboardPanel navigation rail; atlas/fixed-width console layouts.

The dashboard navigation is a non-scrolling VBox with 40 navigation buttons at a 30-pixel minimum each: 1200 pixels before headings, separation, save/developer controls or header/footer. That already exceeds 1080 pixels. Stored 1280×800 Research/Standing/Muster atlas images also visibly cut off right-side grid content. Plans130To133 has a 1320-pixel minimum before fitting smaller targets. These are concrete overflow defects/risks, not evidence that root anchors solve responsive layout.

Evidence: src/UI/GameDashboardPanel.cs:BuildNavigationRail/AddNavButton (397–460/604); src/UI/Plans130To133Panel.cs:_Ready; snapshots/{research_atlas_default,standing_record_atlas_default,muster_atlas_default}.png.

Required follow-up acceptance: Provide reachable scroll/reflow navigation and test descendant bounds, long labels, focus scrolling and supported scales. Preserve domain distinction while repairing sizing.

#### UI-18 — HIGH — Three detail refreshes access freed labels during UI construction

Affected: ExpeditionRadarPanel; FactionsNarrativePanel; SkillMatrixPanel.

RefreshDetail empties the detail container through immediate child freeing and then accesses a cached label that belonged to it. A headless BuildUserInterface run logged ObjectDisposedException at ExpeditionRadar line 370, FactionsNarrative line 325 and SkillMatrix line 371. These failures occur before the decon UI smoke test announces PASS.

Evidence: src/UI/ExpeditionRadarPanel.cs:RefreshDetail (370); src/UI/FactionsNarrativePanel.cs:RefreshDetail (325); src/UI/SkillMatrixPanel.cs:RefreshDetail (371); src/UI/AshfallUiHelpers.cs:EmptyChildren; /tmp/ashfall-ui-audit.KI94f7/decon-airlock-uitest.log:11/40/69.

Required follow-up acceptance: Repair ownership/lifetime of rebuilt detail children and assert fresh construction plus repeated selection/refresh is exception-free.

#### UI-19 — MEDIUM — Production unbound/empty paths display plausible fixture data

Affected: GreenhousePanel; WeatherPanel; FactionMatrixPanel; FactionsNarrativePanel; SilentFoundryPanel; ExpeditionRadarPanel; DutyRosterPanel; SkillMatrixPanel; DoseLedgerPanel; SurvivalWorkstationPanel; MapAtlasPanel; MaritimeAtlasPanel; QuestsAtlasPanel.

Production Refresh/BuildRows branches include fixture builders on missing hosts or empty collections. A lost binding or genuinely empty campaign can therefore display credible invented rows rather than an explicit unavailable/empty state. This finding is about fallback behavior; it does not claim these panels all use fake data when correctly bound. UI-01/UI-03 document the separate bound-path violations.

Evidence: src/UI/GreenhousePanel.cs:304; src/UI/WeatherPanel.cs:121; src/UI/FactionMatrixPanel.cs:148; src/UI/FactionsNarrativePanel.cs:245; src/UI/SilentFoundryPanel.cs:280; src/UI/ExpeditionRadarPanel.cs:239/291; src/UI/DutyRosterPanel.cs:255; src/UI/SkillMatrixPanel.cs:225; src/UI/DoseLedgerPanel.cs:113; src/UI/SurvivalWorkstationPanel.cs:320; src/UI/MapAtlasPanel.cs:273; src/UI/MaritimeAtlasPanel.cs:281; src/UI/QuestsAtlasPanel.cs:BuildQuestRows.

Required follow-up acceptance: Keep fixtures in explicit test/preview mode. Runtime empty, loading, unavailable and error states must be truthful and must not imply completed actions or inventory.

#### UI-20 — HIGH — Greenhouse water choices all decode to clean 50-unit watering

Affected: GreenhousePanel; Main.HandleGreenhouseAction.

The panel emits water:25:clean, water:50:clean and water:50:tainted. Main splits only the first colon; '25:clean' and '50:tainted' are neither 'tainted' nor valid floats, so all three fall through to clean water with 50 units. The selected quantity/source is lost and the displayed stock gate can disagree with consumption. Seed selection, water choices, supply/readiness UI now exist; the older eight-gap specification must not be treated as current evidence. Amendment/maintenance/sterilization host API claims also need revalidation against the trimmed current host.

Evidence: src/UI/GreenhousePanel.cs:511–593; src/Main.World.cs:HandleGreenhouseAction (69–95); src/Host/GreenhouseHostSession.cs:Plant/Water; docs/ui/GREENHOUSE_UI_GAP_SPEC.md.

Required follow-up acceptance: Pin quantity and source as a typed command contract with inventory-delta tests for all three choices, then reconcile the historical Stitch handoff with current APIs.

#### UI-21 — HIGH — Coverage claims and smoke-test passes overstate player readiness

Affected: PlayerSurfaceManifest; PanelRouteGateTests; UiLayoutSelfTest; UiAccessibilitySelfTest; Wave 6/Plans 146–149 UI tests; snapshot manifest.

PlayerSurfaceManifest manufactures binding strings and marks Live routes ProductionRendered/reachable without observing execution. Route tests miss AddNavButton parameter literals and the inverse 'every configured ID is registered' check. Layout tests set and check root bounds, not descendants. Accessibility tests do not measure contrast or perform a full focus traversal. Wave 6 tests check construction/non-null, and new Plans146–149 tests primarily IsBound. The decon smoke log contains 18 missing-Margin errors, 18 NullReferenceExceptions and three ObjectDisposedExceptions before PASS; layout teardown reports 16634 leaked ObjectDB instances. Scene contracts and golden fixtures are limited evidence, not end-to-end gameplay.

Evidence: Assets/Ashfall.Core/UI/PlayerSurfaceManifest.cs:Generate; Ashfall.Core.Tests/UI/PanelRouteGateTests.cs; src/Host/HostCli.cs:RunUiLayoutSelfTest (2638 onward); src/Host/UiAccessibilitySelfTest.cs; src/Main.UiTests.Wave6.cs; src/Main.Plans146_149.cs:228 onward; docs/ui/snapshot_manifest.json; headless audit logs.

Required follow-up acceptance: Require route→bind→visible→select→command→state delta→feedback→save/reload proof, fail on engine exceptions, and validate descendant bounds/focus. Never report generated 100% as observed UI completion.

#### UI-22 — MEDIUM — UI documentation classifies files and contracts incorrectly

Affected: AGENTS.md missing/stub list and Greenhouse handoff; UI_PANEL_ARCHITECTURE_GUIDE; closeout/coverage claims.

The previous list says Electrostatic and Wave 6 files are missing and exempts RailwayTerminal as bound. Current source proves different states (UI-07/UI-11/UI-13). Bind(object?) alone is not a stub test: Electrostatic performs a real VentilationHostSession cast and subscriptions. **RailwayTerminalPanel** is confirmed as a UI-07 stub with typed `Bind(RailwaySystem)` — the Core system, save store, and Main orchestration are complete, but `RefreshView` is empty and no domain commands are wired. The architecture guide's scene-contract examples and historical Greenhouse API descriptions are not a substitute for current source. Concurrent closeout files likewise cannot override missing command/navigation evidence.

Evidence: AGENTS.md:STITCH UI HANDOFF/Missing UI panels; docs/ui/UI_PANEL_ARCHITECTURE_GUIDE.md; src/UI/RailwayTerminalPanel.cs; src/UI/ElectrostaticScrubberPanel.cs:Bind; src/UI/GreenhousePanel.cs; docs/ui/GREENHOUSE_UI_GAP_SPEC.md; src/Main.Plans190_193.cs.

Required follow-up acceptance: Track EXISTS/COMPILES/WIRED/EXECUTES/PLAYER-FACING/VERIFIED separately. Update status with current source and action-level acceptance evidence, not filename creation or a screenshot.

#### UI-23 — HIGH — Shared small-text tokens and controls undermine readability and keyboard access

Affected: Theme; AshfallDataGrid; AshfallSidebar; AshfallDashboardShell; GameDashboardPanel.

Runtime tuple colors give Dim/Ink contrast 3.45:1 and Dim/SurfaceCard 3.09:1, below the 4.5:1 body-text audit threshold; Dim/SelectedBg is 2.73:1. DataGrid headers and sidebar hints use Dim at 11px. Critical/SurfaceCard is 4.12:1. Several hex tokens disagree with the tuples actually rendered, so a mockup can use different colors from production. DataGrid row activation is mouse-GuiInput-only, without focusable keyboard selection. Close targets are 28px high and nav targets 30px: usability concerns at small scales, not a claimed universal target-size standards violation.

Evidence: Assets/Ashfall.Core/UI/Theme.cs:31–95/FontSizeLabel; src/UI/AshfallDataGrid.cs:BuildHeaderCell/BuildRow; src/UI/AshfallSidebar.cs:67/156; src/UI/AshfallDashboardShell.cs:AttachHeaderCloseButton (150); src/UI/GameDashboardPanel.cs:606; docs/ui/ACCESSIBILITY_REPORT.md.

Required follow-up acceptance: Fix non-disabled readable text contrast and focusable row selection first; reconcile token representations, audit text scaling/long labels, and verify focus visibility/topmost behavior. Keep non-color warning labels.

### Functional equivalents and uncertainty — do not invent duplicate Core systems

- ResearchPanel, StandingRecordPanel, MusterPanel, QuestsPanel, MapPanel,
  MaritimePanel/DeepCoastPanel are existing domain counterparts to the
  atlases. Preserve their owners and legitimate work; do not retain fake
  atlas datasets or simply relabel the same body again.
- SlurryDewateringSumpPanel has real SumpFlooding binding despite a similar
  frame/comment. ElectrostaticScrubberPanel has a real object-to-session
  cast. Neither is a stub merely because of those superficial matches.
- ChemicalLabPanel is not automatically replaced by PharmaLab; retort
  chemical synthesis and pharmaceutical preparation are different tasks.
- ChroniclePanel has an Endgame/Epilogue partial equivalent; its lack of
  direct Main construction does not prove endings are absent.
- WeatherHistoryPanel has a real weather-history/F5 path through
  Main.Application and OpenWeatherHistoryPanel, despite no registry entry.
- Trauma bonding, Long Walk, vouch, radon, cupola, aquifer and memorial have
  existing Core semantic counterparts (TraumaBondSystem, LongWalkSystem,
  VouchAccessSystem, YearOfAshRadonSystem, CupolaFoundryEngine,
  GeothermalAquiferSystem, MemorialSystem). Resolve the exact command/data
  contract before creating any new owner for a shelved prototype.
- WeatherHardeningHostSession, CounterIntelligenceHostSession and
  ReconTelemetryHostSession lack direct typed panel consumers in this
  sweep. This is a candidate exposure review, **not proof that each needs
  an independent screen**: indirect integration/readouts must be considered.
- No save-corruption or deterministic-replay failure was reproduced by
  this UI audit. Missing presentation does not imply missing persistence.
  Use existing campaign sections and seeded Core owners.

### Verification record — do not convert partial passes to “all green”

| Check | Observed audit result |
|---|---|
| Core/tests build | Initial PASS; final recheck **FAIL, 8 errors/5 warnings** in concurrent Plans146–149 test additions |
| Full xUnit run | **INCOMPLETE/UNKNOWN**: output stalled, then old terminal session was lost; no final full-suite verdict — **ROOT CAUSE RESOLVED 2026-09-06 (F9–F12 seal):** the stall was `GeothermalAquiferSystemTests.AdvanceDrilling_BitDestroyed_DeactivatesProject` spinning an unbounded `while` when no strata catalog is loaded (`AdvanceDrilling` fails `no_strata` forever; fix applied in-place to the untracked test file — load a depth-0 stratum; owned by the Flagship XI stream to commit). With the fix, `dotnet test --blame-hang` completes in 69 s: 8328 total, 8315 passed, 13 failed (all other streams' in-flight gate tests; none from the F9–F12 wave) |
| Godot host build | Final recheck **PASS, 0 warnings/0 errors** |
| data-integrity-selftest | Earlier audit snapshot **PASS**, 255 catalogs, 0 errors/0 warnings |
| bridge-selftest | **PASS**, exit 0; shim-removal verb only |
| scene-binding-selftest | **PASS, 22/22** scene contracts; not all panel workflows |
| ui-accessibility-selftest | Prints **PASS, 5 gates**, but limited assertions and teardown leaks/errors |
| ui-layout-selftest | Prints **PASS**, but root-only size checks; 16634 ObjectDB instances leaked at shutdown |
| decon-airlock-uitest | Prints **PASS** after 18 missing-Margin, 18 NullReferenceException and 3 ObjectDisposedException reports: **FAIL for UI health** |
| Targeted PanelRouteGateTests | Approved bounded retry **PASS, 19/19** against earlier --no-build assembly; latest test source still failed compilation at recheck |

The lost terminal no longer had a dotnet process when checked; no process
was killed. A later sandboxed targeted attempt failed because the test
runner could not open a local socket; the approved outside-sandbox retry
passed. These are distinct observations; the cause of the earlier stalled
full run is not established.

**Resolution (2026-09-06, F9–F12 wave seal):** the recurring full-suite
stall was reproduced under `--blame-hang` and pinned to the unbounded drill
loop in `GeothermalAquiferSystemTests.AdvanceDrilling_BitDestroyed_DeactivatesProject`
(no strata loaded ⇒ `AdvanceDrilling` fails `no_strata` every iteration ⇒
infinite loop). A second test in the same file used the same missing-catalog
pattern and was fixed identically. `SaveRoundTrip_PreservesFullState` in that
file still fails for behavioral reasons owned by that stream. The earlier
UI-audit-era stalls are consistent with this test (alphabetically late,
always near the end of a run) but older runs are not reproducible to prove it.
Quarantine note: the F9–F12 wave's four committed test files were swept into
the csproj quarantine while untracked and were unquarantined in commit
620381bd after verifying they compile and pass on trunk.

New test-build errors included unresolved CoreSeededRng,
PowerSupplyContext.Throttled, EbPvdCoatingEngine.ResumeJob and mismatched
context/RegisterRailSegment arguments. These belonged to concurrent work
and were not repaired by the audit. Earlier catalog/headless results
predate some concurrent additions; see the forensic report for exact limits.

### Closure rules for future UI work

1. Keep prototypes unavailable until truthful domain state and real
   commands exist. Never promote merely to satisfy a route-count gate.
2. Use google-stitch via Antigravity for later missing/stub design work,
   following project policy; reconcile proposals with current API contracts
   and runtime color tuples, not stale hex-only mockups/specs.
3. Verify normal entry → descriptor → dependency setup → Bind → visible
   state → stable selected ID → command → Core/inventory delta → returned
   feedback → keyboard/controller Close/back → save/reload where relevant.
4. Fail UI health checks on engine exceptions. Test child bounds, long
   labels, readable contrast and focusable grid selection; root dimensions,
   IsBound and historical screenshots are insufficient.
5. Do not create panel-local gameplay/resource state, a parallel save store
   or another modality manager to make a mockup look operational.
6. Keep LastEvent/result feedback tied to the active domain command.
   No fixture-success messages in production and no unrelated subsystem's
   stale event replacing the action the player just performed.
7. Recheck concurrent changes before closing findings. Update this register
   and the detailed report with evidence, not a blanket completion claim.

Suggested next prompt after accepting this audit: “Plan the UI-01–UI-23
repairs from the forensic report, preserving unique domain workflows and
existing Core ownership. Prioritize runtime health, truthful state/actions
and dead routes. Do not implement or generate external designs yet.”
