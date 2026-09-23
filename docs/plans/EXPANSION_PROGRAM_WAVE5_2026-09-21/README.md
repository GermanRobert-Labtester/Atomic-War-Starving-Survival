# ASHFALL Expansion & Integration Program — Wave 5 (2026-09-21)

Ten more plans (41–50), all **major expansion**. This wave also includes the
**major expansion of Plan 1**: a generated 99-system dossier appendix.

Produced from a read-only audit of HEAD `5be1a30a`. **None is a claim.**

## Plan 1, majorly expanded

| File | Size | Content |
|---|---:|---|
| `../EXPANSION_PROGRAM_2026-09-21/PLAN-ORPHAN-SEAL-01.md` | ~21 KB | original plan (waves, DoD, risks) |
| `../EXPANSION_PROGRAM_2026-09-21/PLAN-ORPHAN-SEAL-01_APPENDIX-A_ORPHAN_DOSSIERS.md` | **~76 KB** | per-system dossiers for **all 99 orphan authorities**: types, known test files, candidate catalogs, suggested host seam, five-step wiring recipe, wave assignment, plus a 99-row summary table |

The appendix is generated from the reachability audit, so the plan now carries
its own work list: each Wave 1–8 package can lift its dossier entries directly.

## The ten expansion plans

| # | Plan | Frontier | Authored base it builds on |
|---|---|---|---|
| 41 | [`PLAN-ESPIONAGE-COUNTERINTEL-41.md`](PLAN-ESPIONAGE-COUNTERINTEL-41.md) | Spy networks, dead drops, double agents, ciphers, EW intercepts | `FactionCovertOpsCoordinator`, `InformantNetworkTradecraftEngine`, `HiddenAgendaSystem`, `SignalTriangulationSystem`, 4 data catalogs |
| 42 | [`PLAN-RADIO-MEDIA-42.md`](PLAN-RADIO-MEDIA-42.md) | Station empire, programs, audiences, jamming, print | `RadioProgramProductionSystem`, `PsyOps`, `PropagandaSystem`, `Distress*`, 11 radio data files |
| 43 | [`PLAN-FAMILY-DYNASTY-43.md`](PLAN-FAMILY-DYNASTY-43.md) | Households, childhood, coming of age, inheritance | `RomanceFamilySystem`, `GenerationalSystem`, `SecondGenerationMilestoneEngine`, `CampaignLegacySystem`, `ChildDevelopmentSystem`, `AgingSystem` |
| 44 | [`PLAN-CRIME-SYNDICATES-44.md`](PLAN-CRIME-SYNDICATES-44.md) | Syndicates, smuggling, heists, enforcement | `BlackMarketSystem`, `LedgerDebtSystem`, 4 host-unreachable engines, contraband matrices |
| 45 | [`PLAN-INDUSTRY-AUTOMATION-45.md`](PLAN-INDUSTRY-AUTOMATION-45.md) | Production queues, quality, shifts, exports | `SilentFoundrySystem` (Plan 213 sealed), 6 host-unreachable industry engines, 7 foundry catalogs |
| 46 | [`PLAN-WATER-AGRICULTURE-46.md`](PLAN-WATER-AGRICULTURE-46.md) | Watersheds, irrigation, soil, closed loops | `WaterTreatmentSystem`, `DeepWellSystem`, `BrineWaterSystem`, `FluidInfrastructure`, `Farming/` incl. 2 orphans |
| 47 | [`PLAN-PANDEMIC-PUBLIC-HEALTH-47.md`](PLAN-PANDEMIC-PUBLIC-HEALTH-47.md) | Surveillance, quarantine, surge, countermeasures | `DiseaseSystem` (20 conditions), `SickListSystem`, wastewater/sanitation/vent, Plan 60 contract |
| 48 | [`PLAN-ENERGY-NUCLEAR-48.md`](PLAN-ENERGY-NUCLEAR-48.md) | Generation portfolio, grid, storage, reactor | `PowerGridSystem`, subgrids, `NuclearCoreLifecycleSystem`, `SofcElectrochemistryEngine`, `CvdDiamondSynthesisEngine` |
| 49 | [`PLAN-SIGNALS-REMOTE-SENSING-49.md`](PLAN-SIGNALS-REMOTE-SENSING-49.md) | Orbital telemetry, InSAR, GPR, metrology, EW | `OrbitalHarrowTelemetrySystem`, `InSarDeformationEngine`, `GeodeticSurveyEngine`, `GroundPenetratingRadarEngine`, 9 catalogs |
| 50 | [`PLAN-RECREATION-MORALE-50.md`](PLAN-RECREATION-MORALE-50.md) | Downtime, hobbies, music, games, venues | `SurvivorDowntimeSystem`, `HobbySystem`, `CassettePlaybackSystem`, `VinylMoraleSystem`, 4 catalogs |

## Programme state (56 plans)

| Wave | Plans | Focus |
|---|---|---|
| 1 | 01–06 (+ Appendix A) | orphan sealing, kit, unblock, culture + body verticals, launch face |
| 2 | 11–20 | governance, saves, determinism, data, UI, perf, tests, narrative, assets, release ops |
| 3 | 21–30 | events, field data, selftests, debt, inputs; ecology, maritime, weather, politics, transport |
| 4 | 31–40 | architecture, lifecycle, time, references, silent failure; belief, justice, science, food, shelter |
| 5 | 41–50 | espionage, radio, dynasty, crime, industry, water, pandemic, energy, sensing, recreation |

## Recommended order for Wave 5

1. **43 family**, **50 recreation**, **47 pandemic** — they extend already-live
   survivor/medical owners with the lowest new-authority risk.
2. **45 industry**, **48 energy**, **46 water** — after Plan 40 (shelter
   architecture) lands its room/grid/fluid seams.
3. **41 espionage**, **42 radio**, **49 sensing** — after Plan 29 (politics) and
   Plan 28 (weather) land; they consume those seams.
4. **44 crime** — after Plan 37 (justice) so enforcement/raids have a legal
   layer to route through.

## Shared rules

- Every plan names the existing authority it extends and forbids a parallel one.
- Every mechanic has a bounded player action and an observable outcome.
- Every new row/field needs a live consumer; every new item resolves to
  `items.json`.
- Seeded RNG only; replay equality across mid-run save/reload.
- Fictional, original content; no real people, nations, media, or hardware.
- Focused verification only; no full-suite defaults.


## Claim readiness

Every plan in this programme carries a **§24 Claim readiness** block (claim
template, verification commands, dependencies, checklist). The cross-wave
index — execution order, hubs, free starts, and residual gaps — is
[`../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md`](../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md).
