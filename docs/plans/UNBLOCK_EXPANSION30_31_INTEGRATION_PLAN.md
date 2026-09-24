# UNBLOCK-EXPANSION-30-31 — Readiness Plan (The Press + The Kiln)

**Date:** 2026-09-24
**Claim:** `claim-unblock-expansion-30-31-2026-09-24`
**Authority:** user-authorized integrator package (foreman assignment in this turn).
**Shape:** identical to `UNBLOCK-EXPANSION-25-29` completed 2026-09-24 (rail + glass).

## 0. Forensically proved premise

Both engines are signed pure-domain authorities with Core tests, and **both have
zero host references** — their behavior is unreachable in play:

| Expansion | Engine | Core file | Host refs | Save section | Day owner | Probe |
|---|---|---|---|---|---|---|
| 30 — The Press | `PublicBroadsheetPressEngine` | `Assets/Ashfall.Core/Print/PublicBroadsheetPressEngine.cs` (271 L) | **0** | none | none | none |
| 31 — The Kiln | `KilnFiringEngine` | `Assets/Ashfall.Core/Shelter/KilnFiringEngine.cs` (240 L) | **0** | none | none | none |

Evidence command: `grep -rln "\bPublicBroadsheetPressEngine\b" --include=*.cs src/` → 0 files;
same for `KilnFiringEngine`. Existing Core tests (`Print/PublicBroadsheetPressEngineTests.cs`
114 L, `Shelter/KilnFiringEngineTests.cs` 96 L) prove the arithmetic but not reachability.

Both engines are **static/stateless**: they mutate a state object handed to them
(`TypeTrayState`, `KilnBatchState`) and return plain results. Neither owns a
campaign-lifetime ledger, so neither can survive save/load. That is the real gap.

## 1. Authority boundaries (one authority per concern)

### Expansion 30 — The Press
| Concern | Owner | This package |
|---|---|---|
| Rumor facts (strength, truthfulness, propagation) | `Ashfall.Core.InformationFlow.RumorSystem` (host: `RumorNetworkHostSession`, section `wasteland_rumors`) | Press only **reports** the engine's debunk correction; the host raises `Truthfulness` on the canonical rumor record. Never authors or decays a rumor. |
| Shelter morale | `SurvivorsHostSession.Needs` → `NeedKind.Morale` (same route as `Main.Narrative.ApplyNarrativeMorale`) | Press returns `MoraleStabilizationPermille`; the host converts it and applies it to living interior residents through the canonical needs authority. No morale counter. |
| Ink material definitions | `ArchiveInkCatalogLoader` / `ArchiveDeskSystem` (catalog + records) | Press owns only its **press consumables** (ink reservoir, paper stock, type tray) in permille — the same shape as `GlassworksLedger`'s grit stock. Verified there is no canonical consumable stock authority to duplicate (no authored paper/ink/clay/limestone crafting items in `items.json`). |
| Radio broadcast | `RadioProgramProductionSystem` | Untouched. The press prints; the radio broadcasts. |
| Compositor/firer skill | `SkillProgressionSystem` (canonical, `crafting` discipline) | Host resolves skill permille from the canonical owner; no parallel skill state. |

### Expansion 31 — The Kiln
| Concern | Owner | This package |
|---|---|---|
| Metallurgy / refractory consumption | `Ashfall.Core.Shelter.CupolaFoundryEngine` (`refractory_integrity`) | Kiln produces graded refractory tile output and records lining wear; it does **not** mutate the cupola furnace state (no public seam; named deferral). |
| Kiln fuel | none exists (verified: `FuelPermille` appears only in `KilnFiringEngine`) | The kiln ledger owns its own fuel reserve, mirroring `GlassworksLedger`'s grit stock. Not a duplicate of anything. |
| Masonry / shelter upgrades | `RouteInfrastructureSystem`, `WeatherHardeningCatalogLoader` | Kiln records fired-brick output only; upgrade application is a named deferral (no canonical material-application seam). |
| Ceramics/brick narrative catalogs | `CeramicsKilnCatalog`, `MasonryBrickworksCatalog` | Untouched. |

## 2. Implementation (mirrors the Expansion 25/29 package exactly)

### Core (new, engine-free)
- `Assets/Ashfall.Core/Print/BroadsheetPressLedger.cs` — `BroadsheetPressState`
  (schema 1: type tray, pressed-publication archive, cumulative counters),
  `PressedPublication`, `BroadsheetPressCensus`, and `BroadsheetPressLedger`
  (execute run → delegates `PublicBroadsheetPressEngine` and archives the run;
  `RestoreTypeTray`; `CalculateDebunkCorrection`; schema-gated `RestoreState`;
  `GetCensus`; `Clear`). Deterministic publication ids — no RNG.
- `Assets/Ashfall.Core/Shelter/KilnFiringLedger.cs` — `KilnFiringState`
  (schema 1: batches, fuel reserve, lining wear, output tallies),
  `KilnFiringCensus`, `KilnFiringLedger` (add batch, advance one stage consuming
  ledger fuel + accruing lining wear on draw, lime calcination, refuel, reline,
  schema-gated `RestoreState`, `GetCensus`, `Clear`).

### Host
- `src/Host/BroadsheetPressHostSession.cs` (+ `BroadsheetPressSaveStore`, section
  `broadsheet_press`, file `broadsheet_press_save.json`).
- `src/Host/KilnworksHostSession.cs` (+ `KilnworksSaveStore`, section `kilnworks`,
  file `kilnworks_save.json`; host owner name taken from the design bible).
- `src/Main.BroadsheetPress.cs`, `src/Main.Kilnworks.cs`.
- `src/Main.CampaignOwners.cs` — `BroadsheetPressDayOwner` (census heartbeat; the
  press never auto-prints) and `KilnworksDayOwner` (advances the oldest active
  batch one stage at the deterministic optimal temperature, phase 5), both with
  `IPreDaySnapshotRestore`.
- `src/Main.SaveOrchestrator.cs`, `src/Main.Lifecycle.cs`, `src/Main.Application.cs`.
- `src/Host/HostCli.BroadsheetPress.cs`, `src/Host/HostCli.Kilnworks.cs` (12 checks each),
  `src/Host/HostCli.cs`, `Assets/Ashfall.Core/HostCliRegistry.cs`.
- `Assets/Ashfall.Core/Campaign/DayEventVocabulary.cs` — 2 heartbeats.

### Tests
- `Ashfall.Core.Tests/Print/BroadsheetPressLedgerTests.cs` (5).
- `Ashfall.Core.Tests/Shelter/KilnFiringLedgerTests.cs` (5).
- Save-section pin bump in `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`.

### Generated artifacts
- `scripts/ci/generate-architecture-map.py` (+2 nodes) → `ARCHITECTURE_TEST_MAP.md`.
- `generate-save-store-matrix`, `generate-selftest-manifest`, `generate-docs-index`,
  `generate-plan-integration-audit` (adds Expansion 30/31 so these plans can never
  silently regress to partial again).

## 3. Acceptance

1. Host + Core builds 0 errors.
2. Both probes 12/12 (`--broadsheet-press-selftest`, `--kilnworks-selftest`).
3. Both ledger test files pass; engine test files still pass (no engine edit).
4. Save round-trip gate passes with the new section pin.
5. `MainTriadDriftGateTests` 7/7; architecture map 247 subsystems at 100%.
6. Plan-integration audit reports 46/46 (adds both plans, gates the regressions).
7. No Unity, no parallel ledger, no RNG, no Core engine references.

## 4. Named deferrals

- **The Press UI panel** — presentation; the UndergroundPrintingPressPanel already
  renders the clandestine print context, and the host commands + probe are the
  operational surface. Registering a new routed panel needs coordinated edits
  across `PanelRegistryBootstrap`, `OpenPlayerPanel`, `Main.UiPanels`,
  `Main.PlayerSurfaces`, `PlayerSurfaceManifest` (concurrently-owned shared seams).
- **Kiln → CupolaFoundry refractory handoff** and **kiln output → shelter upgrade
  application** — both need a public material-application seam on the canonical
  owners; neither exists today, and inventing one would fork those authorities.
- **Broadsheet/almanac authored content** — publications are archived facts with
  headlines; authored issue content is a narrative-content task.
