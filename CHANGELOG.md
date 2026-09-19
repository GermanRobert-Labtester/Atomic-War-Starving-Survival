# Changelog

All notable changes to ASHFALL. Format: Keep a Changelog.

## [Unreleased]

### Plan 76 series — Expedition destination catalog & scavenging-table migration

- **Plan 76 — destination catalog validation & loot-ref repair** (`docs/expeditions/PLAN76_CLOSEOUT.md`):
  catalog verified at 53 authored destinations / 263-id merged dispatchable surface
  (expansion target already exceeded; quantitative objective superseded per
  repository-truth rule); repaired 3 invalid loot references on 5 destinations
  (`bandages`→`bandage`, `food_rations`→`dried_rations`,
  `copper_wire`→`copper_wire_10m_of_10m`) including the host no-catalog
  fallbacks; new `Plan76DestinationLootReferenceTests` regression gate.
- **Plan 76.1 — full Plan 46 scavenging-table migration** (`docs/expeditions/PLAN76_1_CLOSEOUT.md`):
  all 53 destinations bound to `scavenging_tables.json` tables (11 → 53 of 53);
  29 new tables authored across 7 family passes (medical, mechanical/fuel,
  household/commercial, military, electrical, water/chemical, remainder);
  renewable trade-stock model introduced for living settlements; one-time
  cache model introduced for supply caches; zero new item ids; all codex
  references reuse existing ids. Catalog: 20 → 49 tables.
- **Plan 76.2 — deterministic balance simulation** (`docs/balance/BALANCE_SIM_EXPEDITION_DESTINATIONS.md`):
  seeded harness (200 runs × 53 destinations, byte-identical two-pass
  determinism proof) over real runtime math; flagged and fixed the
  `loc_ordnance_shoulder` economy outlier (E[value] 216.9 → 114.4, ratio to
  next destination 3.5× → 1.86×, best-ammo identity retained) via
  owner-approved quantity-band trim; documented `collapsed_building` yield
  bump and the Denial Cut dominance decision (accepted — warlord-layer
  encounter multiplier and existing narrative hooks are the differentiators).
- **Plan 76.3 — low-priority pass & series closeout**: `collapsed_building`
  bulk-band yield bump (+45% E[value], identity preserved); Denial Cut
  decision recorded; this changelog.

### Plan 76.3 — destination-level seams sealed (GAP-48A / GAP-49B)

- Weather gates: `WeatherRouteGateCatalog` (Core) + 3 destination-targeted
gates + dispatch wiring via reason-carrying `ExtraGateBlock` seam; dispatch bar
now renders the true block source instead of hardcoded crossing text.
- GAP-48B force passage: gates carry `force_stamina_cost`; a forced sortie
starts `MaxStamina − cost` (Core `Start`/`ExecuteStart` `startingStamina`,
clamped); FORCE PASSAGE action in the dispatch bar with the gate's
consequence as tooltip; ice-road/deep-coast blocks remain absolute.
- Micro-locations: `micro_locations.json` merged into the narrative encounter
loader; 3 destination-bound encounters for the Plan 76 §35 targets.
- Host/UI wiring files (`ExpeditionHostSession.cs`, `Main.Maritime.cs`,
`ExpeditionPanel.cs`) intentionally uncommitted with their interleaved
Plan 60 / concurrent changes; full suite 6848/6848 green.

### Notes

- Verification basis for the series: `dotnet build Ashfall.csproj` (0/0),
  scoped `dotnet test` green, `--data-integrity-selftest` (0 findings),
  `--expedition-selftest` (19/19), `--content-utilization-selftest` (gate PASS).
- Two unrelated test failures in `FactionRadioBroadcastExpansionTests` belong
  to a concurrent workstream active in the same tree and are outside the
  scope of this series.
- `src/Host/ExpeditionHostSession.cs` loot-ref fallback fixes are present in
  the working tree but intentionally left uncommitted with that file's other
  in-flight changes.

---

## [1.1.0] — 2026-09-19

> **Tag:** `v1.1.0`
> **Bump Type:** Minor
> **Base:** (de-facto v1.0.0 — no `v1.0.0` tag exists; see PROCESS.md retro note)

<!-- generated:begin -->

### Architecture & Infrastructure

- **Plan 37 / C2[15] — Input, Focus & Controller Reality:**
  22-action canonical input contract (`AshfallInputActions.cs`);
  joypad bindings (B, A, RB, DpadUp/Down/Left/Right, Y, Back) in `project.godot`;
  `AshfallFocusNavigator` spatial navigation with analog stick repeat cadence;
  `ModalManager` rebuilt over Core `ModalStackController`;
  `KeyBindingApplicator` safe-mode boot, conflict detection, reset;
  rebinding UI section in `SettingsPanel`;
  `input_map_contract` CI gate + 6 xUnit tests; `KEYBOARD.md` generated map.
  Save schema `UserSettingsData.SchemaVersion` bumped to 2 with `KeyBindings`
  sanitization codec.

- **Plan 48 / C2[21] — Release Craft (Phase 1–3):**
  `ReleaseVersion.cs` engine-free strict-semver parser (`TryParse`, `ClassifyBump`);
  three-source version agreement enforced (`project.godot`, `Directory.Build.props`,
  `export_presets.cfg`); `version-gate.py` drift CI gate (fast, critical);
  `generate_changelog.py --check` marker-region drift gate (fast);
  `SaveSupportWindowTests` (15 tests, full tier) pinning six codec schema
  versions and validating the historical fixture corpus;
  `VERSIONING.md`, `PROCESS.md`, `TEMPLATE.md` release documentation;
  28 capability claims registered.

### Save Schema

| Codec | Version | Notes |
|---|---|---|
| `holdfast` | v5 | No change from prior state |
| `year_of_ash` | v5 | No change |
| `dose_ledger` | v2 | No change |
| `expansion_hub` | v6 | No change |
| `expansion_quest` | v1 | No change |
| `weight_of_choices` | v2 | No change |
| `user_settings` | v2 | KeyBindings field added; migration: empty map on v1 loads |

### CI Gates

Total gates at v1.1.0: 57 (53 fast + 3 full + 1 performance)
New gates added: `input_map_contract` (fast), `version_gate` (fast),
`changelog_drift` (fast), `save_support_window` (full)

<!-- generated:end -->
