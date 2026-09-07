# Plans B86–B89 — Implementation Log

## Phase 0 — Docs + baseline

Status: PASS (with known-accepted baseline failure)

Changed:

- `docs/PLANS_86_89_AUTHORITY_MAP.md` (CREATE)
- `docs/plans/PLANS_86_89_INTEGRATION_PLAN.md` (CREATE)
- `docs/combat/PLAN_86_AUTHORITY_MAP.md` (CREATE)

Tests / gates:

| Check | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests` | PASS (earlier) |
| `dotnet test Ashfall.Core.Tests` | PARTIAL — 8971 passed, 1 failed |
| `dotnet build Ashfall.csproj` | PASS (0 warnings / 0 errors) |
| `--data-integrity-selftest` | PASS — 288 catalogs, 0 errors |
| `--bridge-selftest` | PASS |

Known-accepted baseline failure (pre-existing, unrelated to B86–B89):

- `CatchPolicyLintGateTests.CatchPolicy_ZeroUndocumentedEmptyCatches`
- Evidence: `src/Host/HostCli.Mods.cs:77` empty catch without `/* cleanup: … */`
- Owned by concurrent mods/l10n stream; do not fix inside this tranche unless it blocks Core work

Divergences: none

Remaining: Phase 1 B89 Precision Metrology

---

## Phase 1 — B89 Precision Metrology

Status: PASS

Changed:

- `Assets/StreamingAssets/Data/metrology_standards_catalog.json` (CREATE)
- `Assets/Ashfall.Core/Shelter/PrecisionMetrologySystem.cs` (CREATE — grades, certificates, calibrate/certify/disturbance/TickDay/ProjectToWorkshop)
- `Assets/Ashfall.Core/Random/CampaignRngStream.cs` — `MetrologyCalibrationDrift` / `MetrologyMeasurementNoise`
- `Assets/StreamingAssets/Data/items.json` — gauge block / optical flat / surface plate / micrometer set
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` — authoritative + loader/consumer maps
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` — `precision_metrology` section
- `Assets/Ashfall.Core/HostCliRegistry.cs` + `src/Host/HostCli.cs` — `--precision-metrology-selftest`
- `src/Host/PrecisionMetrologySaveStore.cs` (CREATE)
- `src/Host/HostCli.PlansB86_B89.cs` (CREATE)
- `src/Main.PlansB86_B89.cs` (CREATE — Setup/Save/Tick/seismic wire/ResolveBallisticsToolingCalibration)
- `src/Main.Plans74_77.cs` — ballistics uses live tooling; SetupPrecisionMetrology dependency
- `src/Main.CampaignOwners.cs` — PrecisionMetrologyDayOwner phase 2
- `src/Main.SaveOrchestrator.cs` / `src/Main.Application.cs` — save + CLI case
- `Ashfall.Core.Tests/Shelter/PrecisionMetrologySystemTests.cs` (CREATE)

Tests / gates:

| Check | Result |
|---|---|
| `dotnet test --filter PrecisionMetrology` | PASS — 13/13 |
| `dotnet test --filter MainTriadDriftGate\|SaveStoreCoverage\|PrecisionMetrology` | PASS — 19/19 |
| `dotnet build Ashfall.csproj` | PASS — 0 warnings / 0 errors |
| `godot -- --precision-metrology-selftest` | PASS — 12/12 |

Divergences:

- Host keeps a duplicate `HostCliAction` in `src/Host/HostCli.cs` (in addition to Core `HostCliRegistry`); both were updated.
- Ballistics soft-fallback treats workshop Calibration `1.0` as unset (GetOrCreate default) so it does not invent a benefit.

Remaining: Phase 2 B88 HF/DF

---

## Phase 1 — B89 Precision Metrology (earlier WIP note)

Status: SUPERSEDED by PASS above

---

## Phase 2 — B88 HF/DF Direction Finding

Status: PASS

Changed:

- `Assets/StreamingAssets/Data/direction_finding_catalog.json` (CREATE)
- `Assets/Ashfall.Core/Radio/DirectionFindingCatalog.cs` (CREATE)
- `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` — baselines, skywave, fingerprint identity ≠ coordinates
- `Assets/Ashfall.Core/Radio/RadioSave.cs` — V3 triangulation nest
- `Assets/Ashfall.Core/Random/CampaignRngStream.cs` — `df_skywave_jitter` / `df_false_signature`
- Host: RadioHostSession Capture/Restore, DiscoverRumor bridge, `--direction-finding-selftest`

Tests / gates:

| Check | Result |
|---|---|
| `dotnet test --filter DirectionFinding\|RadioSaveMigration` | PASS |
| `godot -- --direction-finding-selftest` | PASS — 11/11 |

Divergences: none material

Remaining: Phase 3 B87 Aquaponics

---

## Phase 3 — B87 Closed-Loop Aquaponics

Status: PASS

Changed:

- `Assets/StreamingAssets/Data/aquaponics_system_catalog.json` (CREATE)
- `Assets/Ashfall.Core/Shelter/AquaponicsSystem.cs` (CREATE — tanks, biomass/feed/DO/N/biofilter/disease, harvest, nutrient export, Capture/Restore)
- `Assets/StreamingAssets/Data/items.json` — `item_insect_larvae_meal`, `item_biofilter_media`, `item_aquaponic_fish`
- `Assets/StreamingAssets/Data/nutrition_profiles.json` — `item_aquaponic_fish`
- `Assets/Ashfall.Core/Random/CampaignRngStream.cs` — `aquaponics_disease` / `aquaponics_fry_survival`
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` — `aquaponics` → `aquaponics_save.json`
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` — catalog allowlist + loader/consumer maps
- `src/Host/AquaponicsSaveStore.cs` (CREATE)
- `src/Main.PlansB86_B89.cs` — Setup/Save/Tick + power query + nutrient export projection
- `src/Main.CampaignOwners.cs` — AquaponicsDayOwner phase 2
- `src/Main.SaveOrchestrator.cs` / `src/Main.Application.cs` / HostCli dual enums — `--aquaponics-selftest`
- `Ashfall.Core.Tests/Shelter/AquaponicsSystemTests.cs` (CREATE — 12 tests incl. 60/120d soak)

Tests / gates:

| Check | Result |
|---|---|
| `dotnet test --filter Aquaponics` | PASS — 12/12 |
| `dotnet build Ashfall.csproj` | PASS — 0 warnings / 0 errors |
| `godot -- --aquaponics-selftest` | PASS — 13/13 |
| `godot -- --data-integrity-selftest` | PASS — 292 catalogs, 0 errors |

Divergences:

- Single injected RNG stream (`AquaponicsDisease`) drives both disease rolls and fry survival for host wiring; both `CampaignStreamIds` exist for future split.
- UI Prototype deferred (Core+save+day tick only).
- Greenhouse coupling is export-only via `AquaponicNutrientSource` (no plot mutation).

## Phase 4 — B86 Combat Breaching

Status: PASS (Core + catalog + combat action wiring + selftest; UI deferred)

Changed:

- `Assets/StreamingAssets/Data/breaching_equipment_catalog.json` — 12 obstacle profiles + 5 tools (fictional linear breach consumable; no real construction fields)
- `Assets/Ashfall.Core/Combat/CombatBreachingEngine.cs` — Evaluate/Begin/Advance/Abandon + catalog loader
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Breaching.cs` — player actions, logistics bind, barrier ensure, noise/wear side-effect ports
- `Assets/Ashfall.Core/Combat/CombatTypes.cs` — BarrierState breach fields + `BreachPhaseIds`
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Persistence.cs` — `CloneBarriers` copies B86 fields
- `Assets/Ashfall.Core/Combat/WeaponConditionSystem.cs` — optional `EmitBreachNoise` / `ApplyBreachToolWear`
- `Assets/StreamingAssets/Data/items.json` — five breach tool/consumable items
- ContentUtilizationScanner + dual HostCli enums + `--combat-breaching-selftest`
- `Ashfall.Core.Tests/Combat/CombatBreachingEngineTests.cs` (8 tests)

Tests / gates:

| Check | Result |
|---|---|
| `dotnet test --filter CombatBreaching` | PASS — 8/8 |
| `dotnet build Ashfall.csproj` | PASS — 0 warnings / 0 errors |
| `godot -- --combat-breaching-selftest` | PASS — 13/13 |
| `godot -- --data-integrity-selftest` | PASS — 293 catalogs, 0 errors |

Divergences:

- Encounter-local barriers only (no world persistence of cleared props); route mines remain `RouteInfrastructure` / flail.
- Noise/wear projected through optional ports (event-logged when unbound).
- UI Prototype deferred (`VaultDoorBreachingPanel` remains shelved).

Remaining: Phase 5 Cross-tranche CI / utilization / combined scenario

---

## Phase 5 — Cross-tranche CI / utilization / combined scenario

Status: PASS (UI deferred for all four plans)

Changed:

- B86 review HIGH absorption: fail-closed logistics, Abandoned resume keeps tool id, Interrupted blocks Begin, `CombatHostPorts` ctor sinks, catalog-authored balance multipliers (`breaching_equipment_catalog.json` `balance` + per-tool vehicle/cover factors)
- `src/Host/CombatHostSession.cs` — loads breaching catalog; `ConfigureBreachingLogistics` from `Main.Expeditions.SetupCombat`
- `Ashfall.Core.Tests/PlansB86ToB89ContinuityTests.cs` — seed `8689`, 45-day combined scenario, save day 40 → reload days 41–45
- Contract sync surfaces: `HostCli.PrintHelp` four `--*-selftest` lines; `SaveSectionRegistry` 162; VersionReport envelopes 156; CatchPolicy annotation on `HostCli.Mods.cs`
- Regenerated: `docs/cli/HOST_CLI_COMMAND_CATALOG.md`, `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`, `docs/architecture/ARCHITECTURE_TEST_MAP.md` (via `ARCHITECTURE_GRAPH` +8 keys), `docs/INDEX.md`, `docs/data/CATALOG_REGISTRY.md`, `docs/audio/AUDIO_CUE_CATALOG.md`, `docs/agents/AGENT_SKILLS_INDEX.md`
- `scripts/ci/generate-architecture-map.py` — added `seismic_dynamics`, `cryo_vault`, `geothermal_orc`, `ballistics_workbench`, `aeroponics`, `pneumatic_dispatch`, `precision_metrology`, `aquaponics`

Tests / gates:

| Check | Result |
|---|---|
| `dotnet test --filter PlansB86ToB89Continuity\|CombatBreaching` | PASS — continuity 2/2 (+ prior B86 unit coverage) |
| `godot -- --combat-breaching-selftest` | PASS — 13/13 |
| `godot -- --aquaponics-selftest` | PASS — 13/13 |
| `godot -- --direction-finding-selftest` | PASS — 11/11 |
| `godot -- --precision-metrology-selftest` | PASS — 12/12 |
| `godot -- --data-integrity-selftest` | PASS — 293 catalogs (earlier Phase 5 evidence) |
| content-utilization CI gate | PASS — four catalogs `classification:0` (GAMEPLAY_CONSUMED) |
| `--scene-binding-selftest` / scene-lint | PASS — 25/25 and 30/0 (earlier Phase 5 evidence) |
| `bash scripts/ci/verify-fast.sh` | PASS — **47/47** in 188.78s |

Known-accepted deviations:

- Four new catalogs remain at `achievedRung:1` / `requiredRung:5` (scanner name-maps exist; runtime evidence instrumentation for rung climb still open — CI utilization gate still passes overall).
- UI for B86–B89 remains deferred (Prototype / no player route). Dedicated panels are out of Phase 5 scope.
- Architecture graph also backfilled six concurrent-stream sections already in `SaveSectionRegistry` so count matches 162.

Divergences: none blocking.

Remaining: optional follow-ups only — UI panels via `google-stitch`, utilization rung climb instrumentation, balance soak tables beyond the 45-day continuity hash.
