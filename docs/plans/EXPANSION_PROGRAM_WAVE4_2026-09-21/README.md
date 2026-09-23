# ASHFALL Expansion & Integration Program — Wave 4 (2026-09-21)

Ten more proposed plans (31–40): **five gap-sealing and five expansion**.
Produced from a read-only audit of HEAD `5be1a30a`. **None is a claim.**

## Gap sealing (31–35)

| # | Plan | Gap | Evidence found |
|---|---|---|---|
| 31 | [`PLAN-ARCHITECTURE-BOUNDARY-31.md`](PLAN-ARCHITECTURE-BOUNDARY-31.md) | Core purity / IO boundary | **0 real engine refs** in Core (26 hits are comments), but **260 Core files import IO/JSON** and **487 direct `File.*`/`Path.*` sites** vs 62 files using the `IFileSystem` port |
| 32 | [`PLAN-LIFECYCLE-SEALING-32.md`](PLAN-LIFECYCLE-SEALING-32.md) | Lifetime / disposal | **517 UI files**, 229 with `_Process`/`_Ready`/`_ExitTree`, only **115** with teardown hooks; 780 host files; 100-orphan precedent |
| 33 | [`PLAN-TEMPORAL-AUTHORITY-33.md`](PLAN-TEMPORAL-AUTHORITY-33.md) | Time sources / tick order | `SimClock` + `CampaignDayCoordinator`, **104 day-owner refs**, 45 hour/tick-hour consumers, **12 wall-clock reads in Core**, `IWallClock` port exists |
| 34 | [`PLAN-REFERENCE-INTEGRITY-34.md`](PLAN-REFERENCE-INTEGRITY-34.md) | Cross-catalog & save references | Validator + 318 catalogs, migration precedents (`DoseQuestMigration`, `VerdictQuestMigration`, section aliases), **46 `OrdinalIgnoreCase` sites** |
| 35 | [`PLAN-SILENT-FAILURE-35.md`](PLAN-SILENT-FAILURE-35.md) | Silent failure / observability | **13 fully bare catches + 40 narrow catches**; `ActionResult`, `ILog`, catch-policy gate, local-only telemetry |

## Expansion (36–40)

| # | Plan | Frontier | Authored base |
|---|---|---|---|
| 36 | [`PLAN-BELIEF-IDEOLOGY-36.md`](PLAN-BELIEF-IDEOLOGY-36.md) | Faith, ritual, pilgrimage, schism | `ZealotrySystem` (sealed), `SpiritualRitualCalendarEngine`, `IdeologicalFrictionSystem`, 4 belief data catalogs |
| 37 | [`PLAN-JUSTICE-LAW-37.md`](PLAN-JUSTICE-LAW-37.md) | Law codes, trials, sentencing, bounty | `Verdict/` (15 files), `wasteland_laws.json`, 7 `verdict_*` catalogs, `CrossingArbitrationSystem`, `FactionBountySystem` |
| 38 | [`PLAN-SCIENCE-EDUCATION-38.md`](PLAN-SCIENCE-EDUCATION-38.md) | Schoolroom, archive, salvage science, pharma | `ResearchSystem` (56 nodes), `SurvivorEducationSystem`, `LibraryStudySystem`, `PrewarArchiveDecryptionSystem`, `PharmaLabSystem` |
| 39 | [`PLAN-FOOD-CUISINE-39.md`](PLAN-FOOD-CUISINE-39.md) | Kitchen, preservation chains, seed bank, common table | `CookingSystem`, `FoodTypeSystem`, `CommonTableRationingEngine` host-unreachable; 7 food data catalogs |
| 40 | [`PLAN-SHELTER-ARCHITECTURE-40.md`](PLAN-SHELTER-ARCHITECTURE-40.md) | Rooms, air, heat, water, structure | `ShelterExpansionSystem`/`ShelterMaintenanceSystem` host-unreachable; 10+ shelter data catalogs |

## Recommended order

```
31 BOUNDARY ─┐
32 LIFECYCLE ┼─ structural seals, before the wiring waves add surface
33 TEMPORAL  ┤
34 REFERENCE ┤
35 SILENT ───┘
36–40 expansions ── after PLAN-ORPHAN-SEAL-01 waves and Wave 2/3 enablers
```

## Full programme state

| Wave | Plans | Focus |
|---|---|---|
| 1 | 01–06 (6) | orphan sealing, kit, unblock, two verticals, launch face |
| 2 | 11–20 (10) | governance, saves, determinism, data, UI, perf, tests, narrative, assets, release ops |
| 3 | 21–30 (10) | events, field data, selftests, debt, inputs; ecology, maritime, weather, politics, transport |
| 4 | 31–40 (10) | architecture, lifecycle, time, references, silent failure; belief, justice, science, food, shelter |

36 plans total across all waves. Each plan names its existing authority,
packages with acceptance criteria, focused verification, and its non-goals.

## Shared rules

- Core engine-free; IO through the port; extend the named owner.
- One clock, one tick order, one reference policy.
- Typed errors; player-visible failure; no silent defaults.
- JSON is authority; every row/field consumer-bound.
- Seeded RNG only; focused verification only; fictional original content.


## Claim readiness

Every plan in this programme carries a **§24 Claim readiness** block (claim
template, verification commands, dependencies, checklist). The cross-wave
index — execution order, hubs, free starts, and residual gaps — is
[`../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md`](../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md).
