# Batch 12 — Unblock Oldest Partial Plans: Plans 150 + 152

**Package:** `UNBLOCK-OLDEST-BATCH12-PLANS`
**Claim:** `claim-unblock-oldest-batch12-plans-2026-09-23`
**Date:** 2026-09-23
**Status:** COMPLETE
**Predecessor:** `UNBLOCK-OLDEST-BATCH11-PLANS` (Plans 147 + 148), which completed per-NPC memory and ideological friction.

---

## 1. Scope and Selection

The two remaining `Ready (Core-only)` entries from the oldest-partials queue. Both
had a complete, engine-free Core authority and **zero** host references, zero save
section, and zero CLI probe — the classic partially-integrated shape this queue
exists to close.

| Rank | Plan | Core authority | State before this batch |
|---|---|---|---|
| 9 | 150 — Romance & Family Dynamics | `Survivors/RomanceFamilySystem.cs` (654 lines) | 0 `src/` refs, no save section, no probe |
| 10 | 152 — Vehicle Customization & Mobile Base | `Vehicles/VehicleCustomizationSystem.cs` (434 lines) | 0 `src/` refs, no save section, no probe |

## 2. Premise Audit — Current Evidence

Verified in source before any change (rule 7 — a plan's status is not evidence):

- Both Core authorities already had `CaptureState`/`RestoreState`, but neither
  **gated on schema version** — a future payload would be half-applied silently.
- Neither exposed a **census**, which the architecture scanner consumes across
  every other integrated subsystem.
- `RomanceCourtshipCatalog.LoadFromJson` and `VehicleCustomizationCatalog.LoadFromJson`
  are **lenient hand-rolled parsers**: they default every malformed field instead of
  rejecting it, so an authored typo would become a live courtship event or vehicle
  build. Neither had a strict loader.
- **Latent data-loss defect in Plan 150:** `CaptureState` writes
  `CohabitationQuarters`, but `RestoreState` never read it back — the assignment was
  dropped on every save/load cycle.
- **No authored data existed** for either (`romance_courtship.json`, `vehicle_modules.json`).
- The pre-existing Core tests (`Plan150RomanceFamilyIntegrationTests`,
  `Plan152VehicleCustomizationIntegrationTests`) encode the **intended data
  contract** — specific ids such as `shared_meal`, `quiet_walk`, `reinforced_hull`,
  `bunk_beds_module`, and exact values (`shared_meal` +5, `quiet_walk` +8,
  `reinforced_hull` defence 20 / speed −0.05). This is why they were red the moment
  an authored file appeared with different ids: the contract is real, not decorative.

## 3. Architecture & Boundary Rules

- **One authority per concern.** The Core systems own relationship/family and
  vehicle-module state. The host only projects and persists; no parallel store,
  no second affinity, no second condition/fuel drain (rule 5).
- **Validated-bind seam.** The host loads the authored table through a strict
  loader and hands the Core **pre-validated** rows via a new `BindValidated*`
  seam, making the lenient parser unreachable from the host.
- **Daily mutation stays with its owner.** The vehicle tick deliberately does
  not degrade condition or burn fuel — that is the expedition vehicle
  authority's job, and doing it here would be a second drain on the same resource.
- **Romance is driven by canonical affinity.** New attraction reads the affinity
  `SurvivorRelationsSystem` already tracks and draws from the campaign's own
  `social` RNG stream, so a replay is identical.

## 4. Phased Execution

1. **Core** — `RomanceCourtshipCatalogLoader` and `VehicleModuleCatalogLoader`
   (strict snake_case projection, collected per-row errors, empty-table hard
   error); `BindValidatedEvents` / `BindValidatedModules`; `RomanceFamilyCensus` /
   `VehicleCustomizationCensus` + `GetCensus()`; schema-gated `RestoreState`; and
   the `CohabitationQuarters` restore fix.
2. **Data** — authored `romance_courtship.json` (20 courtship events, including
   the five contract ids) and `vehicle_modules.json` (20 modules across armour,
   cargo, living, weapon, utility, including every contract id and value).
3. **Host** — `RomanceFamilyHostSession` (+`RomanceFamilySaveStore`),
   `VehicleCustomizationHostSession` (+`VehicleCustomizationSaveStore`),
   `Main.RomanceFamily.cs`, `Main.VehicleCustomization.cs`,
   `HostCli.RomanceFamily.cs`, `HostCli.VehicleCustomization.cs`.
4. **Seams** — `SaveSectionRegistry` (`romance_family`, `vehicle_customization`),
   `HostCliRegistry` (2 actions + descriptors), `DayEventVocabulary`
   (`romance_family_ticked`, `vehicle_customization_ticked` as internal
   heartbeats), `Main.CampaignOwners` (2 phase-5 day owners with
   `IPreDaySnapshotRestore`), `Main.Application`, `Main.SaveOrchestrator`,
   `HostCli`.
5. **Tests** — 4 new suites, plus the section-count pin 232 → 234.
6. **Generated** — architecture map (234 subsystems), port contract (291 seams),
   catalog registry, selftest manifest, save-store matrix, plan register, docs index.
7. **Stale gates closed** — three host files still carried hand-rolled
   `Assets/StreamingAssets/Data` resolvers and `HostCli.CampaignLegacy.cs` held an
   undocumented empty catch; all routed through `CatalogPath.ResolveDataDir()` and
   documented, returning `Tooling` to 122/122.

## 5. Evidence

Host and Core test builds 0 errors / 0 warnings.

- `--romance-family-selftest` **12/12**; `--vehicle-customization-selftest` **12/12**
- `Plan150RomanceFamilyHostIntegrationTests` **10/10**;
  `Plan152VehicleCustomizationHostIntegrationTests` **10/10**
- `RomanceCourtshipCatalogLoaderTests` **10/10**; `VehicleModuleCatalogLoaderTests` **11/11**
- `ComprehensiveSaveStoreCorruptionAndMigrationTests` **1406/1406** (section pin 234)
- Vehicles **16/16**, Survivors **511/511**, Records **34/34**, Save **1519/1519**,
  Campaign **257/257**, Tooling **122/122**, Weather **27/27**, Governance **48/48**,
  Settlements **12/12**, Expeditions **331/331**, Content **27/27**,
  Presentation **12/12**, Combat **93/93**, Narrative **306/306**, Endgame **123/123**
- `--data-integrity-selftest` PASS (0 errors, 421 catalogs);
  `--port-contract-selftest` PASS (291 seams, 186 host-required bound);
  `--save-store-checksum-selftest`, `--7-day-smoke-selftest`,
  `--save-load-ui-failure-selftest`, `--selftest-manifest` all PASS
- `agent-fast-verify` **10/10**; all eight generators in sync

## 6. Deferred With Named Reasons

- Romance/family UI panel and family-tree view: presentation only; the plan's
  DoD for this batch is the authority + host seam + persistence + probe.
- Vehicle combat rules, vehicle racing/trading, and famous-vehicle reputation:
  separate mechanics with their own owners; not part of the integration seam.
- The two deferred developer-facing items are recorded rather than silently
  dropped: vehicle-condition/fuel degradation remains with the expedition
  authority, and the romance quest hooks (`The Matchmaker` et al.) belong to the
  quest owner.

## 7. Next Batch Head

The oldest-partials queue is now empty for Plans 59, 135, 136, 140, 141, 145,
147, 148, 150, and 152. Queue authority remains `INTEGRATION_PLANS.md`.
