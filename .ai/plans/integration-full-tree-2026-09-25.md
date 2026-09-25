# Feature / Task Plan: Full working-tree integration — WHOLEGAME-P1 + alpha G3/G4/G5 + barter/UI waves + docs expansion (2026-09-25)

STATUS: APPROVED BY USER

Approval basis: the user's explicit session instruction of 2026-09-25 —
"Full immediate integration mandatory … verify all have been fully integrated
not partials" — issued on branch `integration/all-latest-2026-09-24`. The
WHOLEGAME-P0/P1 packages were separately pre-approved in
`.ai/plans/wholegame-p0-build-green.md` and
`.ai/plans/wholegame-p1-playable-ui-integration.md`.

## 1. Goal & Outcome

- **Goal:** Land every uncommitted deliverable on the integration branch in
  verified, non-partial form: (a) WHOLEGAME-P1 core/host/UI code + tests;
  (b) the alpha-balance stream (G3 storm watch, G4 route dose check, G5
  quiet-hours tradeoff, door-encounter barter counter-offers, salvage
  teardown tests, localization ratchet, host-CLI parity gate); (c) the
  UI/UX panel-flow wave (`src/UI/UiPanelFlow.cs` in-panel transitions,
  drag/movable windows, pulse feedback); (d) the plan-document expansion
  program output (≈3,400 docs), new location art (188 assets), and
  generated tooling under `scripts/`; (e) ledger updates.
- **Non-Goals:** No new gameplay features authored here; no full test-suite
  run (scoped-only per TEST_POLICY.md); no quarantine changes; no Unity.

## 2. Claimed Paths & Affected Files

- **Files (production):** `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`,
  `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`,
  `Assets/Ashfall.Core/Shelter/ShelterNoiseSystem.cs`,
  `Assets/Ashfall.Core/UI/CrisisPresentationCoordinator.cs`,
  `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`,
  `Assets/Ashfall.Core/Radiation/RouteDoseCheck.cs` (new),
  `Assets/StreamingAssets/Data/door_encounters.json`,
  `src/Host/{ConsequenceLedgerSaveStore.cs (new), StormWatch.cs (new),
  HostCli.cs, PowerGridHostSession.cs, SalvageHostSession.cs,
  ShelterThermalHostSession.cs}`,
  `src/Main.*.cs` (Lifecycle, Application, ExpandedShelterSystems, GameFlow,
  Holdfast, PanelLifecycle, Plans147, PlayerSurfaces, SaveOrchestrator,
  ShelterInfrastructure, UiPanels),
  `src/UI/{UiPanelFlow.cs (new), AshfallDashboardShell.cs, AshfallMetricCard.cs,
  BrineExtractionPanel.cs, ConfirmationModal.cs, DoseGeographyPanel.cs,
  EmergencyResponseHud.cs, GameDashboardPanel.cs, InventoryPanel.cs,
  NarrativeArcModal.cs, Phase0Panel.cs, PowerGridPanel.cs,
  ShelterAtmospherePanel.cs, ShelterBarterPanel.cs, ShelterPanel.cs,
  ShelterThermalPanel.cs}`.
- **Files (tests):** `Ashfall.Core.Tests/{Flags/ConsequenceLedgerSaveTests.cs (new),
  Campaign/DayAdvanceOrderTests.cs (new), Radiation/RouteDoseCheckTests.cs (new),
  Shelter/QuietHoursTradeoffTests.cs (new), Tooling/HostCliActionParityGateTests.cs (new),
  Tooling/LocalizationRatchetTests.cs (new), UI/PanelCatalogCompletenessTests.cs (new),
  Inventory/SalvageTeardownSystemTests.cs, YearOfAsh/DoorEncounterBarterTests.cs}`.
- **Files (docs/data/ledgers):** `docs/**` (expansion program), `scripts/**`,
  `assets/art/**`, `INTEGRATION_PLANS.md`, `KNOWN_DEBT.md`,
  `WORKTREE_OWNERSHIP.md`, `.ai/state.md`.
- **Restored (unauthorized deletions reverted):** root + archived pre-foreman
  rulebooks (`.clinerules`, `CLAUDE.md`, `CRUSH.md`, `MIMOCODE.md`,
  `.kiro/steering/narrative.md`, `docs/archive/agent-rules/2026-09-12-pre-foreman/*`)
  per AGENTS.md and KNOWN_DEBT `DEBT-RULEBOOK-SNAPSHOT`. Six empty stray
  root files (`Attach`, `Compute`, `Connect`, `Materialize`, `Parse`, `Read`)
  removed as junk.

## 3. Pre-flight Checks

- [x] JSON parse + Godot data-integrity selftest (427 catalogs, 0 errors)
- [x] Existing owners extended (WeatherSystem read model, ShelterNoiseSystem,
  SaveSectionRegistry consequence_ledger, PanelRegistryBootstrap)

## 4. Implementation Steps (Max 60-100 steps)

1. Restore mandated rulebooks; remove junk files. (done)
2. Build Core tests project and Godot host project. (done, 0/0 both)
3. Run scoped tests for every changed/new test file. (done, 12 suites PASS)
4. Run data-integrity selftest headless. (done, PASS)
5. Record ledger entries; commit production set, then docs/assets wave. (this step)

## 5. Verification

- [x] `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` → 0 W / 0 E
- [x] `dotnet build Ashfall.csproj` → 0 W / 0 E
- [x] `bin/run-scoped-tests` → 6/6 + 3/3 + 1/1 + 2/2 files, 0 failed
  (QuietHoursTradeoff 3, HostCliActionParityGate 4, LocalizationRatchet 2,
  PanelCatalogCompleteness 3, ConsequenceLedgerSave 4, DayAdvanceOrder 2,
  RouteDoseCheck 3, SaveSectionRegistry 5, CampaignConsequenceLedger 7,
  CampaignDayCoordinator 19, SalvageTeardown, DoorEncounterBarter)
- [x] `godot --headless --data-integrity-selftest` → PASS, 427/427, 0 errors
- [x] Max 10-15 test-edit steps: 8 testing steps used, 0 failures to resolve
