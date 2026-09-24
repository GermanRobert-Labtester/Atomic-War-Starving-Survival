# UNBLOCK — Plan 181: Difficulty Settings System Host Integration

**Status:** HOST INTEGRATION COMPLETE 2026-09-24 (integrator, user-authorized).
**Claim:** `claim-unblock-plan-181-difficulty-settings-2026-09-24`.
**Evidence:** `--difficulty-settings-selftest` 12/12; host + Core builds 0 errors.

## Premise (verified in source before editing)

The audit row "Plan 181 — Partial, Core-only (overlaps XP difficulty binding)" is
half right and needs the overlap stated precisely:

- **Already integrated by XP-01 (`XP-WAVE1-DIFFICULTY-AUTHORITY`, DEC-40/XP-01):**
  `DifficultyPresetCatalog` + `DifficultyPresetCatalogLoader`, `DifficultyDirector`,
  `DifficultyScalarsProvider`, `difficulty_presets.json` (4 authored presets), the
  campaign-header identity preset (`manifest.difficultyPresetId`), and the eight
  scalar consumers (hunger, thirst, radiation, disease, hostile, market, equipment,
  crisis deadline) via `Main.Difficulty.cs` / `Main.EvolvingWorld.cs` /
  `Main.BriefingCrisis.cs`.
- **Not integrated:** the signed pure-domain `DifficultySettingsSystem` (DEC-174,
  Plan 181) had **0 host references**. It is the settings authority that owns
  preset selection, the eight custom slider lanes, and the ironman/campaign lock,
  plus `CaptureState`/`RestoreState`. Nothing in the host consumed it, so custom
  difficulty and the lock were unreachable in play.
- **Also present but out of scope:** `DifficultyConsequenceWeave` (DEC-40, EN-01) is
  a signed read model with 0 host consumer sites. Wiring its four world consumers
  (war stage severity, crisis deadline scaling, shock weight, monotonicity) changes
  war/crisis behaviour and belongs to EN-01, not Plan 181. Left untouched.

## Integration (single authority, no second catalog)

**Core (additive)**
- `DifficultyScalarsProvider.FromScalars(presetId, scalars)` — validated, cloned
  factory so a custom configuration can be expressed as the canonical typed view.
  `HasSameScalarsAs` for value comparison.
- `DifficultySettingsSystem`: `BindCatalog(catalog)` (share the campaign's single
  catalog instance), `GetEffectiveProvider()`, `GetCensus()` +
  `DifficultySettingsCensus`, and a schema-gated `RestoreState` (a newer schema is
  refused; a legacy v0 payload normalizes to v1).

**Host**
- `DifficultySettingsSaveStore` — section `difficulty_settings`
  (`difficulty_settings_save.json`), checksummed.
- `DifficultySettingsHostSession` — binds the shared catalog, routes
  `SelectPreset` / `SetCustomScalar` / `Lock`, exposes the census and the effective
  provider.
- `Main.DifficultySettings.cs` — setup/save/reset/census and the runtime commands.
- `Main.Difficulty.cs` (one additive hook in `SetupDifficulty`, one in
  `SelectDifficultyForNewCampaign`): the settings authority overrides the effective
  `_difficultyScalars` only; the campaign identity preset and header are unchanged.
  Consumers already read `_difficultyScalars` live, so a change takes effect without
  rebinding any system.
- CLI probe `--difficulty-settings-selftest` (12 checks).

**Authority boundary:** the header remains the immutable campaign-identity preset
(endings/telemetry/replay). The settings section is the only mutable runtime
settings store when unlocked; the default (unlocked, non-custom, header preset)
makes the two agree exactly.

## Deferred with named reasons

- **Runtime slider/lock panel**: registering a new routed panel requires
  coordinated edits to `PanelRegistryBootstrap`, `OpenPlayerPanel`,
  `Main.UiPanels`, `Main.PlayerSurfaces`, and `PlayerSurfaceManifest` — all
  concurrently-owned shared seams (Plans 186/188, prose waves). The campaign-start
  preset selector already exists (`StartingCohortSetupPanel`); the host commands
  and probe provide the operational surface now. Promotion condition: claim the
  panel-route seams once no other package holds them.
- **`DifficultyConsequenceWeave` consumer wiring**: EN-01, not Plan 181 (see above).
- **Custom preset naming/sharing, preset comparison view, difficulty journal,
  "The Choice/Adjustment/Lock" narrative events, and completion quest hooks**:
  presentation/narrative over the settings authority; the events would need an
  emitted day-event kind and a parity-matrix row.
- **HUD difficulty indicator**: the read-only display exists via
  `Main.DifficultyActiveDisplayName()`; placing it in the HUD dashboard touches the
  same concurrently-owned UI seams.

## Verification

```
host + Core builds: 0 errors / 0 warnings
--difficulty-settings-selftest 12/12
Difficulty suite + Plan181DifficultySettingsIntegrationTests (existing 6/6)
--data-integrity-selftest PASS   --port-contract-selftest PASS
save section #240 registered; architecture map node added
```
