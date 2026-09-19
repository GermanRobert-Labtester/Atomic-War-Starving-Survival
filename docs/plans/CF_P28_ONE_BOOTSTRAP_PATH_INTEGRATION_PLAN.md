# CF-P28-ONE-BOOTSTRAP-PATH — Integration Plan

**Package:** `CF-P28-ONE-BOOTSTRAP-PATH` (completion-first program "Plan 10"; census anchor C2[9] / Plan 28 residual)
**Queue authority:** `AGENTS.md` §ACTIVE QUEUE item 4; `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` row 4 — **no new foreman signature required (bounded host change)**
**Prior program treatments this plan supersedes in detail (not in scope):** `Seal-steps/ashfall-eight-unblocked-plans-completion-first-execution-program-2026-09-19.md` §C.4 ("Plan 04"); `Seal-steps/ashfall-fifteen-unblocked-partial-integrations-completion-first-full-integration-and-enhanced-expansion-program.md` §C.10 ("Plan 10")
**Anchor debt row:** `DEBT-PLAN28-MAIN-CONSTRUCTOR-MIGRATION` — RETIRED/sealed 2026-09-18 (`KNOWN_DEBT.md` line 16); this package is the fresh-game-path gap that seal deliberately left behind, not a re-opening of the seal.
**Evidence snapshot date:** all file/line citations re-verified by direct read on the current `Zcode_Branch` HEAD (`65357b8a`), 2026-09-19.
**Status:** FULLY INTEGRATED AND SEALED — fresh bootstrap, reset enrollment, static parity coverage, and restore/journey evidence completed 2026-09-19. The composition-root selftest and MainTriadDriftGateTests limitations have been completely resolved and verified green.
**Closeout:** bootstrap parity gate 6/6; `SubsystemManifestTests` 7/7; black-market host wiring 3/3; vehicle integration 5/5; host build 0 warnings/0 errors; 7-day smoke 10/10; player-panels selftest PASS; real-campaign journey PASS; vehicle garage 27/27; sky defense 17/17; save/load failure-path 8/8; `composition_root_uitest` PASS (exit code 0); `MainTriadDriftGateTests` 7/7 PASS. All gates green.

---

# 1. Objective

Make the **fresh-game (new campaign) startup path execute the same declarative subsystem-manifest bootstrap that the restore/load path already executes**, so the two campaign lifecycles cannot silently diverge in which subsystems they construct, wire, and persist.

Concretely, the package delivers five things and nothing more:

1. **One invocation.** `ComposeCampaign()` (`src/Main.CampaignServices.cs:24`) calls `ExecuteSubsystemManifestBootstrap()` at a defined, evidence-justified position, so all 18 manifest-registered `SetupAction` delegates run on the fresh path exactly as they already do on the restore path (`src/Main.SaveOrchestrator.cs:163`).
2. **Lifecycle reset parity for the three manifest subsystems that lack it.** `_memorial`, `_blackMarket`, and `_vehicleGarage` are currently never nulled by `ResetAllSessionsInMemory()`; because their restore happens only at construction time (`TryLoad` inside the constructor/`Ensure*`), a stale instance survives an in-process slot switch and the loaded campaign's section is never applied. The parity gate this package adds is only meaningful if construction after reset is real construction, so these three fields (and their panels) are enrolled in the existing lifecycle reset seam. `_skyDefense` is already enrolled (`src/Main.FlagshipInstitutions.cs:71`) and serves as the reference pattern.
3. **A static parity gate** (`Ashfall.Core.Tests/Tooling/BootstrapPathParityGateTests.cs`, new) pinning: both lifecycle entry points invoke the bootstrap; every manifest descriptor with `HasDedicatedSetup == true` has exactly one registered `SetupAction`; every registered target method exists; the manifest-18 host fields have lifecycle reset coverage.
4. **Runtime parity assertions** in the existing Godot selftest harnesses: after `ComposeCampaign()` all 18 manifest-mapped session fields are non-null and reference-stable across a second `ComposeCampaign()` and across shuffled panel opens; after `TryLoadAndRestoreGame()` the same 18 fields are non-null; a same-process two-campaign leg proves no cross-campaign residue for the three newly enrolled fields.
5. **Census truth.** C2[9] reconciled to `SEALED` in `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` — both the ranked table row (line ~223, currently `PARTIALLY-SEALED`) and the stale §5 anchor line (~185, currently says `OPEN-UNCLAIMED` / "No declarative subsystem manifest", contradicting the same file's table) — routed through the wave11-part2 integrator who owns the census.

**Non-goals (hard boundaries):** no migration of additional `Setup*` calls into the manifest; no reordering of `RestoreAllSubsystemsFromDisk()`; no rewrite of `ComposeCampaign()`'s direct-call list; no Core API change; no save-schema, data, or content change; no unification of the ~60 non-manifest subsystems whose construction timing also differs between paths (that is the decision-blocked EN-06 composite's problem, not this bounded package's).

---

# 2. Current Reality

## 2.1 The declarative spine (Core, engine-free, sealed)

`Assets/Ashfall.Core/Orchestration/SubsystemManifest.cs` declares 18 `SubsystemDescriptor` records in a static `All` list. Each record carries `(Id, DisplayName, Phase, SaveSectionKey, DayOwnerId, PrimaryPanelRoute, HasDedicatedSetup, Description)` plus the mutable host-bound `SetupAction` property (line 45) added under the sealed Plan 28C migration. The static API surface:

- `RegisterSetupAction(string id, Action action)` (line 286) — assigns `descriptor.SetupAction`. **Silently ignores unknown ids** (`TryGet` fails → no-op, no throw, no log). This is a drift hazard this package gates, not modifies.
- `ExecuteSetup(LifecyclePhase? phase = null)` (~line 297) — iterates `All` **in declaration order**, invokes every non-null `SetupAction`, returns the count of *invoked* delegates (not of *newly constructed* sessions — it cannot see the host null-guards).
- `TryGet` / `Get` / `Contains` / `ForPhase` — read-only lookups.

All 18 descriptors currently have `HasDedicatedSetup = true`.

## 2.2 The host binding (sealed 2026-09-18)

`src/Main.Lifecycle.cs`:

- `_manifestSetupRegistered` (field, ~line 508) — per-`Main`-instance one-shot guard.
- `RegisterManifestSetupActions()` (lines 514–538) — registers 18 delegates:
  `journal→SetupJournal()`, `needs→SetupSurvivors()`, `inventory→SetupInventory()`, `weather→SetupWorld()`, `radiation→SetupDoseLedger()`, `radio→SetupRadio()`, `expeditions→SetupExpeditions()`, `duty_roster→SetupDutyRoster()`, `crafting→SetupCrafting()`, `research→EnsureSharedResearch()`, `medical→{ SetupMedical(); SetupMedicalWard(); }`, `factions→SetupFactionBranch()`, `economy→SetupEconomy()`, `greenhouse→SetupGreenhouse()`, `shelter_defense→SetupSkyDefense()`, `vehicle_garage→SetupVehicleGarage()`, `black_market→SetupBlackMarket()`, `memorial→SetupMemorial()`.
- `ExecuteSubsystemManifestBootstrap(LifecyclePhase? phase = null)` (lines 542–546) — calls `RegisterManifestSetupActions()` then `SubsystemManifest.ExecuteSetup(phase)`, returns the executed count.

Because the manifest registry is **process-global static state** while `_manifestSetupRegistered` is **per-instance**, a second `Main` instance in the same process (UI-test harnesses) re-registers and overwrites the delegates to point at the newest instance ("last registrar wins"). Harmless today (harnesses drive one composed `Main` at a time); recorded as a documented hazard in §17 (FM-11), explicitly not fixed here.

## 2.3 Sole call site: the restore path only

Repo-wide grep (re-run for this plan) confirms exactly one production invocation of `ExecuteSubsystemManifestBootstrap`:

- `src/Main.SaveOrchestrator.cs:163`, inside `RestoreAllSubsystemsFromDisk()` (declared ~line 161), immediately after `SetupHoldfastRuntime()` and `_holdfastTerminal?.OpenTerminal()`.

`RestoreAllSubsystemsFromDisk()` is reached from `TryLoadAndRestoreGame()` (line ~119; sets `_campaignInitializationMode = CampaignInitializationMode.Restore` at line 134, then `ResetAllSessionsInMemory()`, then the restore call) and from the no-active-slot legacy-migration fallback in `ContinueGame()`.

The fresh-game path — `StartNewGame(string, string)` in `src/Main.GameFlow.cs` (sets `_campaignInitializationMode = CampaignInitializationMode.FreshInitialize` at line 181, calls `ResetAllSessionsInMemory()`, then `ComposeCampaign()` at line 190) — **never invokes the bootstrap** (verified: `ComposeCampaign` body, `src/Main.CampaignServices.cs:24–124`, contains no manifest call; the only other grep hits are the definition site and docs).

## 2.4 Complete manifest enumeration and per-subsystem path status

The following table is the heart of the package's risk analysis. "Fresh path" = reachable from `ComposeCampaign()` during composition (direct call or verified transitive chain). "Restore path" = reachable from `RestoreAllSubsystemsFromDisk()`. "Lazy fallback" = how the subsystem comes alive on a path that does not construct it at composition (day-owner tick or panel open), with evidence.

| # | Id | Phase | Save section | Delegate target | Restore path today | Fresh path today | Lazy fallback on fresh path |
|---|---|---|---|---|---|---|---|
| 1 | `journal` | Bootstrap | `journal` | `SetupJournal()` | **Bootstrap only** — no direct call in `RestoreAllSubsystemsFromDisk` | Direct call #18 in `ComposeCampaign` | n/a |
| 2 | `needs` | CoreSimulation | `survivors` | `SetupSurvivors()` | Bootstrap (2nd) + direct call (no-op) | Direct call #8 | n/a |
| 3 | `inventory` | CoreSimulation | `inventory` | `SetupInventory()` | Bootstrap (3rd) + direct call (no-op) | Direct call #7 | n/a |
| 4 | `weather` | CoreSimulation | `world` | `SetupWorld()` | Bootstrap (4th; transitively ensures `_campaignDay` via `Main.World.cs:173`) + direct call (no-op) | Direct call #10 | n/a |
| 5 | `radiation` | CoreSimulation | `dose_ledger` | `SetupDoseLedger()` | Bootstrap (5th) + direct call (no-op) | Direct call #33 | n/a |
| 6 | `radio` | CoreSimulation | `radio` | `SetupRadio()` | Bootstrap (6th) + direct call (no-op) | Direct call #19 | n/a |
| 7 | `expeditions` | CoreSimulation | `expedition` | `SetupExpeditions()` | Bootstrap (7th) + direct call (no-op) | Direct call #15 | n/a |
| 8 | `duty_roster` | CoreSimulation | `duty_roster` | `SetupDutyRoster()` | Bootstrap (8th) + direct call (no-op) | Direct call #26 | n/a |
| 9 | `crafting` | CoreSimulation | `crafting` | `SetupCrafting()` | Bootstrap (9th) + direct call (no-op) | Direct call #14 | n/a |
| 10 | `research` | CoreSimulation | `research` | `EnsureSharedResearch()` | Bootstrap (10th) + transitive via `SetupCrafting` (`Main.World.cs:280`) | **Transitive only** via `SetupCrafting` (call #14 → `Main.World.cs:280`) and `SetupExpandedShelterSystems` (`Main.ExpandedShelterSystems.cs:105`) | n/a |
| 11 | `medical` | CoreSimulation | `medical` | `{ SetupMedical(); SetupMedicalWard(); }` | Bootstrap (11th) + direct calls (no-op) | Direct calls #11, #12 | n/a |
| 12 | `factions` | CoreSimulation | `regional_treaty` | `SetupFactionBranch()` | Bootstrap (12th) + direct call (no-op) | Direct call #28 | n/a |
| 13 | `economy` | CoreSimulation | `economy` | `SetupEconomy()` | Bootstrap (13th) + direct call (no-op) | Direct call #17 | n/a |
| 14 | `greenhouse` | Expansion | `greenhouse` | `SetupGreenhouse()` | Bootstrap (14th) + direct call (no-op) | Direct call #21 | n/a |
| 15 | `shelter_defense` | Expansion | `sky_defense_battery` | `SetupSkyDefense()` | Bootstrap (15th) + `SetupFlagshipInstitutions()` (no-op) | **Not constructed at composition** | `FlagshipInstitutionsDayOwner.TickDay` → `SetupSkyDefense()` (`src/Main.FlagshipInstitutions.cs:555`); panel: `OpenSkyDefenseBatteryPanel` → `EnsureSkyDefense()` (`src/Main.SkyDefense.cs:29`) |
| 16 | `vehicle_garage` | Expansion | `vehicle_garage` | `SetupVehicleGarage()` → `EnsureVehicleGarage()` | Bootstrap (16th) + `SetupPlans50To53()` (no-op) | **Not constructed at composition** | Garage recovery day owner → `EnsureVehicleGarage()` (`src/Main.CampaignOwners.cs:885/894/903`); panel: `OpenVehicleGaragePanel` → `EnsureVehicleGaragePanel` (`src/Main.VehicleGarage.cs:23`) |
| 17 | `black_market` | Expansion | `black_market` | `SetupBlackMarket()` | Bootstrap (17th) + direct call (no-op, `src/Main.SaveOrchestrator.cs:197`) | **Not constructed at composition** | `UnderworldMarketDayOwner.CapturePreDaySnapshot`/`TickDay` → `SetupBlackMarket()` (`src/Main.CampaignOwners.cs:1015/1026`); panel: `OpenBlackMarketPanel` → `SetupBlackMarket()` (`src/Main.BlackMarket.cs:73`) |
| 18 | `memorial` | Expansion | `memorial` | `SetupMemorial()` | Bootstrap (18th) + direct call (no-op, `src/Main.SaveOrchestrator.cs:219`) | **Transitive only** via `SetupMedical()` → `SetupMemorial()` (`src/Main.Medical.cs:71`) | n/a |

Net assessment: **15 of 18** manifest subsystems are directly called in `ComposeCampaign`; **2 more** (`research`, `memorial`) are transitively constructed during composition; **3** (`shelter_defense`, `vehicle_garage`, `black_market`) are not constructed at all during fresh composition and come alive only at the first day-advance tick or first panel open. Conversely, `journal` demonstrates the inverse asymmetry: on the restore path it exists *only because the bootstrap runs* — there is no direct `SetupJournal()` call in `RestoreAllSubsystemsFromDisk()`. The two paths already depend on different mechanisms for different members of the same declared spine.

## 2.5 The two call graphs, in full

### `ComposeCampaign()` — fresh path (79 top-level statements, current order)

```
 1  SetupCampaignDay()            // registers ALL day owners (Main.Campaign.cs:37 → RegisterProductionCampaignOwners)
 2  SetupHoldfastRuntime()
 3  SetupStartingLevel()
 4  SetupEventsHost()
 5  SetupExpansionQuests()
 6  SetupThirdonary()
 7  SetupInventory()
 8  SetupSurvivors()
 9  SetupDifficulty()
10  SetupWorld()
11  SetupMedical()                // → SetupMemorial() (Main.Medical.cs:71)
12  SetupMedicalWard()
13  SetupPhase0()
14  SetupCrafting()               // → EnsureSharedResearch() (Main.World.cs:280)
15  SetupExpeditions()
16  SetupReconTelemetry()
17  SetupEconomy()
18  SetupJournal()
19  SetupRadio()
20  SetupPowerGrid()
21  SetupGreenhouse()
22  ComposePlans74To77()
23  SetupMaritime()
24  SetupYearOfAsh()
25  SetupVerdict()
26  SetupDutyRoster()
27  SetupMuster()
28  SetupFactionBranch()
29  SetupMoralChoice()
30  SetupDeepCoast()
31  SetupSilentFoundry()
32  SetupPhantom()
33  SetupDoseLedger()
34  SetupCombat()
35  SetupNarrative()
36  SetupEchoes()
37  SetupSpiritual()
38  SetupUtilityAi()
39  SetupCaravans()
40  SetupExpansions()
    // ── "Wiring that needs all services up" ──
41  SetupExpeditionCombatHandoff(_combat)
    inventory ↔ survivors ↔ holdfastRuntime cross-wiring (idempotent re-bind)
    // ── Plans 178-201 expansion block (36 calls) ──
42  SetupGenerational() … 77  SetupBioFermentation()
    (incl. SetupPlasticPyrolysis() and SetupCargoAirdrop() at lines 122-123 —
     grep-verified to have NO other caller anywhere in src/)
78  SetupExpandedShelterSystems() // composite: ~35 sub-setups incl. EnsureSharedResearch
79  SetupPlans166To169()
```

### `RestoreAllSubsystemsFromDisk()` — restore path (bootstrap + 122 direct calls + `UpdateHud()`)

```
 0  SetupHoldfastRuntime(); _holdfastTerminal?.OpenTerminal();
 1  ExecuteSubsystemManifestBootstrap();          // ← 18 delegates, manifest declaration order
 2  SetupStartingLevel()        3  SetupSurvivors()         4  SetupInventory()
 5  SetupMedical()              6  SetupMedicalWard()       7  SetupDifficulty()
 8  SetupWorld()                9  SetupRadio()            10  SetupMoraleContagion()
11  SetupPathogenStrains()     12  SetupSubterranean()     13  SetupPsyOps()
14  SetupRadioProgramProduction()  15  SetupLowBackgroundMetrology()  16  SetupInSarMapping()
17  SetupHydraulicExtrusion()  18  SetupRunFlatTire()      19  SetupSofcPower()
20  SetupCvdDiamond()          21  SetupSoundRanging()     22  SetupAmphibiousDraisine()
23  SetupPiezometer()          24  SetupCrafting()         25  SetupCaravans()
26  SetupExpeditions()         27  SetupCombat()           28  SetupNarrative(reloadEventAdapter: true)
29  SetupEchoes()              30  SetupEconomy()          31  SetupSanitation()
32  SetupDeepWell()            33  SetupWaterCondenser()   34  SetupBlackMarket()
35  SetupUtilityAi()           36  SetupDutyRoster()       37  SetupVerdict()
38  SetupMaritime()            39  SetupPhantom()          40  SetupPhase0()
41  EnsureMedicalPipeline()    42  SetupDoseLedger()       43  SetupMuster()
44  SetupYearOfAsh()           45  SetupExpansions()       46  SetupExpansionQuests()
47  SetupThirdonary()          48  SetupGreenhouse()       49  SetupPowerGrid()
50  ComposePlans74To77()       51  SetupSilentFoundry()    52  SetupDisease()
53  SetupEncounterChoiceResolver()  54  SetupTravelEncounters()  55  SetupSurvivorSocial()
56  SetupMemorial()            57  SetupSurvivorFate()     58  SetupSpiritual()
59  SetupExpandedShelterSystems()  60  SetupPlans166To169()  61  SetupFactionBranch()
62  SetupOnboarding()          63  SetupEcologicalInfestation()  64  SetupFieldGuide()
65  SetupWorkshop()            66  SetupRadioStation()     67  SetupShelterSocial()
68  SetupExcavationHazards()   69  SetupDynamicQuests()    70  SetupPlans50To53()
71-78  SetupGenerational() … SetupPolitics()
79  SetupFlagshipInstitutions()   // → SetupSkyDefense() etc. (all no-ops post-bootstrap)
80-104  SetupAnomalyHazard() … SetupBioFermentation()
    // NOTE: SetupPlasticPyrolysis() and SetupCargoAirdrop() are ABSENT here —
    // they reconstruct only via panel bind actions (Main.PlayerSurfaces.cs:96,101).
105-120  SetupEndgame() … SetupShelterFireHazard() (16 enrolled-section setups)
121  SetupMoralChoice()
122  UpdateHud()
```

## 2.6 Why the double-execution pattern is already proven safe

The restore path **already executes every manifest delegate twice**: once inside the bootstrap (step 1) and once via the later direct call. The 2026-09-18 seal passed `MainTriadDriftGateTests` (7/7), `player_panels_uitest` (17/17), and `7day_smoke_selftest` (10/10) in exactly this configuration. The safety comes from three established host patterns, each verified in source for this plan:

- **Null-guard idempotency.** Every `Setup*()` begins `if (_x != null) return;` (e.g., `SetupInventory` `src/Main.Inventory.cs:57`; `SetupBlackMarket` `src/Main.BlackMarket.cs:27`; `SetupMemorial` `src/Main.Campaign.cs:211`; `EnsureVehicleGarage` `src/Main.Plans50_53.cs:40`; `EnsureSkyDefense` `src/Main.FlagshipInstitutions.cs:205`).
- **Order tolerance via lazy providers.** Cross-system reads are `?.`-guarded closures evaluated at tick time, not construction time (e.g., `SetupSurvivors` binds `FilterHealthPercentProvider = () => _startingLevel?.System.State.airFilterHealthPercent ?? 100f` — `src/Main.Survivors.cs:~88`). Evidence that order tolerance is real and not aspirational: the two paths already construct `starting_level` vs `inventory` in **opposite** orders today (fresh: #3 before #7; restore: bootstrap's `inventory` before direct `SetupStartingLevel()` at step 2) and both are gate-green.
- **Bidirectional re-bind on every call.** Repeat invocations re-run cross-wiring outside the null-guard, so whichever order a pair is constructed in, the later call completes the wiring (e.g., `SetupSurvivors` re-binds `_inventory.Survivors` even when `_survivors` pre-exists, `src/Main.Survivors.cs:128–136`; `ComposeCampaign` repeats the same cross-wiring explicitly after `SetupExpansions()`).
- **Mode-gated seeding.** The only two manifest-18 setups whose behavior differs between fresh and restore — `SetupInventory` (`seedWhenNoSave: _campaignInitializationMode == FreshInitialize`, `src/Main.Inventory.cs:65`) and `SetupSurvivors` (fresh cohort load vs `SurvivorsSaveStore.TryLoad()`, guarded by `_survivorInitializationApplied`, `src/Main.Survivors.cs:138–163`) — read `_campaignInitializationMode`, which **both entry points set before composition begins** (`Main.GameFlow.cs:181`, `Main.SaveOrchestrator.cs:134`). The bootstrap's position *inside* either composition method therefore cannot change their behavior.

## 2.7 The reset-coverage gap (new evidence, not in prior program docs)

`ResetAllSessionsInMemory()` (`src/Main.Lifecycle.cs:~481`) drives `SessionLifecycleRegistry.ResetAll()` over 27 registered participants. Field-level reset coverage for the manifest-18:

| Subsystem field | Reset? | Where |
|---|---|---|
| `_journal` | yes | `Main.Lifecycle.cs:215` |
| `_survivors` | yes | `Main.Lifecycle.cs:44` |
| `_inventory` | yes | `Main.Lifecycle.cs:54` |
| `_world` | yes | participant `world_weather` |
| `_doseLedger` | yes | `Main.Lifecycle.cs:330` |
| `_radio` | yes | participant (radio) |
| `_expeditions` | yes | `Main.Lifecycle.cs:99–112` |
| `_dutyRoster` | yes | `Main.Lifecycle.cs:64–74` |
| `_crafting` | yes | participant |
| `_sharedResearch` | yes | `Main.Lifecycle.cs:295` |
| `_medical` / `_medicalWard` | yes | participant (medical) |
| `_factionBranch` | yes | participant |
| `_economy` | yes | `Main.Lifecycle.cs:145` |
| `_greenhouse` | yes | `Main.Lifecycle.cs:293` |
| `_skyDefense` | yes | `Main.FlagshipInstitutions.cs:71` (flagship participant) |
| **`_vehicleGarage`** | **NO** | no null-site anywhere in `src/` (grep-verified) |
| **`_blackMarket`** | **NO** | no null-site anywhere in `src/` (grep-verified) |
| **`_memorial`** | **NO** | no null-site anywhere in `src/` (grep-verified; only the `= null!` declaration at `Main.Campaign.cs:26`) |

All three missing fields use **restore-at-construction-once** semantics: `SetupMemorial()` ends with `LoadMemorial()` → `MemorialSaveStore.TryLoad()` (`src/Main.Campaign.cs:263–271`); `BlackMarketHostSession.Create` calls `BlackMarketSaveStore.TryLoad()` (`src/Host/BlackMarketHostSession.cs:105`); `EnsureVehicleGarage` calls `VehicleGarageSaveStore.TryLoad()` (`src/Main.Plans50_53.cs:60`). Consequence: in a single process, `New Game → (day tick or panel constructs them) → ReturnToMenu → Continue a different slot` leaves the **previous campaign's instances alive**; their `Setup*` guards no-op; the loaded slot's sections for `memorial` / `black_market` / `vehicle_garage` are never applied. This is a pre-existing cross-campaign leak, latent today, that this package's parity work will otherwise make *deterministic and test-visible* — and any honest "fresh and restore lifecycles agree" claim is false while it stands. It is therefore included as the evidence-driven Phase P1.5 (§19), sequenced *after* a failing isolation test proves it in the current tree.

## 2.8 Adjacent findings recorded for the foreman (explicitly out of scope)

- **F-ADJ-1: fresh-only construction of `plastic_pyrolysis` and `cargo_airdrop`.** `SetupPlasticPyrolysis()` / `SetupCargoAirdrop()` are called only from `ComposeCampaign()` (`Main.CampaignServices.cs:122–123`) — no restore-path call, no day-owner call; on the restore path they reconstruct lazily via panel bind actions (`Main.PlayerSurfaces.cs:96,101`). Because both `Ensure*` bodies `TryLoad()` at construction (`Main.Plans202_205.cs:64–66,152–154`), a `SaveAll` that runs after a load but before the first panel open omits both sections from the re-written envelope — the same divergence *class* as this package, in the opposite direction, for two non-manifest subsystems. Routed to the foreman as a candidate follow-up package; **not touched here**.
- **F-ADJ-2: merge leftover.** `src/Main.SaveOrchestrator.cs.theirs` exists in the working tree (appears in grep hits, e.g. its own stale `SetupMemorial();` line). Repo-hygiene scope (`ashfall-repo-hygiene`), not this package.
- **F-ADJ-3: census internal contradiction.** `UNCLAIMED_CORPUS_CENSUS.md` line ~90 and the ranked table (~223) say C2[9] `PARTIALLY-SEALED`, while §5 line ~185 still says "`C2[9]`/Plan 28 — OPEN-UNCLAIMED. No declarative subsystem manifest." Both rows are reconciled in Phase P3.
- **F-ADJ-4: composition-root selftest blind spot.** `RunCompositionRootUiTestAndQuit` (`src/Main.UiTests.CompositionRoot.cs`) checks idempotency as `kv.Value != null && !kv.Value.Equals(after)` — a `null → constructed` transition satisfies neither branch and is **invisible** to the check. The test's own comment calls the constructed-count "informational only". This package's runtime assertions close the blind spot **only for the 18 manifest-mapped fields** (a global flip would newly fail on dozens of legitimately lazy non-manifest systems — that wider hardening is EN-06-era work).

---

# 3. Required Delta

| # | Delta | From (verified) | To |
|---|---|---|---|
| D1 | Fresh-path bootstrap invocation | `ComposeCampaign()` never calls `ExecuteSubsystemManifestBootstrap()` (grep-verified sole call site `Main.SaveOrchestrator.cs:163`) | `ComposeCampaign()` invokes `ExecuteSubsystemManifestBootstrap()` (unscoped, all phases) immediately after `SetupExpansions()` and before the "Wiring that needs all services up" block |
| D2 | Reset coverage parity | `_memorial`, `_blackMarket`, `_vehicleGarage` never nulled on reset; restore-at-construction-once means stale instances survive slot switches | All three sessions (and their panels) enrolled in the existing lifecycle reset participants, matching the `_skyDefense` reference pattern; proven by a same-process two-campaign isolation test that fails before the enrollment and passes after |
| D3 | Static parity gate | No test pins which entry points invoke the bootstrap, or that registration covers the manifest | New `Ashfall.Core.Tests/Tooling/BootstrapPathParityGateTests.cs` (6 source-scan facts, modeled on `MainTriadDriftGateTests`) |
| D4 | Runtime parity assertions | Composition-root selftest cannot see `null→constructed`; no journey asserts manifest-field liveness after restore | Manifest-18 non-null + reference-stable assertions added to `--composition-root-selftest`; manifest-18 non-null assertion added to the real-campaign-journey selftest after Continue |
| D5 | Census truth | C2[9] `PARTIALLY-SEALED` (table) / `OPEN-UNCLAIMED` (stale §5 line) | C2[9] `SEALED` in both places with this package's evidence links, edited by the wave11-part2 integrator |

**Explicitly not part of the delta:** the executed *ordering* of the 79 fresh-path direct calls is unchanged; the 122-call restore list is unchanged; no `Setup*` body is modified; no descriptor, save section, JSON file, or Core API is added, renamed, or removed.

---

# 4. Evidence

All citations below were re-verified by direct file reads and greps for this plan on 2026-09-19 at HEAD `65357b8a` (`Zcode_Branch`).

**Manifest and bootstrap mechanics**
- `Assets/Ashfall.Core/Orchestration/SubsystemManifest.cs` — 18 descriptors (lines 58–227 region); `SetupAction` property line 45; `RegisterSetupAction` line 286 (silent on unknown id); `ExecuteSetup` ~line 297 (declaration-order iteration, invoked-count return).
- `src/Main.Lifecycle.cs:514–538` — 18 `RegisterSetupAction` calls; `:542–546` — `ExecuteSubsystemManifestBootstrap`.
- `src/Main.SaveOrchestrator.cs:163` — sole production invocation; `:134` — restore mode set pre-composition; `TryLoadAndRestoreGame` ~`:119`; `ContinueGame` fallback path re-enters `RestoreAllSubsystemsFromDisk`.
- `src/Main.CampaignServices.cs:24–124` — `ComposeCampaign()` full body read; no manifest call; `_composeCampaignCallCount` instrumentation (`:11–13`) already exists for the composition gate.
- `src/Main.GameFlow.cs:181,190` — fresh mode set before `ComposeCampaign()`; slot allocation precedes teardown (`TryCreateFreshCampaignSlot`).

**Per-subsystem construction evidence (fresh path)**
- `research`: `src/Main.World.cs:280` (`_sharedResearch = EnsureSharedResearch();` inside `SetupCrafting`); field `Main.ExpandedShelterSystems.cs:30`.
- `memorial`: `src/Main.Medical.cs:71` (`SetupMemorial();` inside `SetupMedical`); definition `src/Main.Campaign.cs:210` with guard `:211` and `LoadMemorial()` at `:249` → `TryLoad` `:268`.
- `black_market`: `src/Main.BlackMarket.cs:25–43` (guard `:27`; `Create` → `TryLoad` `src/Host/BlackMarketHostSession.cs:105`); day-owner `src/Main.CampaignOwners.cs:1008–1032`; panel `src/Main.BlackMarket.cs:71–77`.
- `shelter_defense`: `src/Main.FlagshipInstitutions.cs:202–226` (`EnsureSkyDefense`: `TryLoad`+`RestoreState` `:222–224`, `EnsureDefaultTurret()` `:225`); `SetupSkyDefense` `:280`; day owner `:547–568`; reset `:71`.
- `vehicle_garage`: `src/Main.Plans50_53.cs:38–66` (`EnsureVehicleGarage`; `TryLoad` `:60`); `SetupVehicleGarage()` `:69`; day-owner recovery tick `src/Main.CampaignOwners.cs:875–905`; panel `src/Main.VehicleGarage.cs:14–46`.

**Mode-sensitive seeding (only two manifest members)**
- `src/Main.Inventory.cs:57–66` — `seedWhenNoSave` from `_campaignInitializationMode`.
- `src/Main.Survivors.cs:24` (`SetupSurvivors`), `:138–163` — fresh cohort vs `SurvivorsSaveStore.TryLoad()` restore; `_survivorInitializationApplied` guard; reset of both flags in `ResetAllSessionsInMemory` (`Main.Lifecycle.cs:483–484`).

**Determinism evidence**
- `Assets/Ashfall.Core/Random/CampaignRngStream.cs:149–166` — `DeriveSeed`/`ForkSeed`/`Fork` are a **pure function of `(masterSeed, streamId, derivationVersion, day, actionIndex)`**; `Fork` does **not** advance the parent stream's `Position`. Construction-time forking (e.g., `EnsureVehicleGarage`'s `_campaignDay.Rng.Fork("vehicle_garage")`, `Main.Plans50_53.cs:41`) is therefore position-independent: moving construction earlier on the fresh path cannot perturb any campaign RNG stream. Same pattern confirmed for `cargo_airdrop` (`Main.Plans202_205.cs:123`) and `shelter_espionage` (`Main.Plans50_53.cs:~87`).

**Existing gates and harnesses**
- `Ashfall.Core.Tests/Orchestration/SubsystemManifestTests.cs` — 7 facts, incl. `ExecuteSetup_InvokesConfiguredDelegates_AndReturnsCount` (note: it pollutes the process-global registry; §18 isolates against this).
- `Ashfall.Core.Tests/Tooling/MainTriadDriftGateTests.cs` — 7 facts; the static source-scan + balanced-body extraction + reachability-walk pattern this package's new gate reuses.
- `src/Main.UiTests.CompositionRoot.cs` — composition idempotency harness + reflection helpers (`CaptureNonUiFieldValues`); blind-spot analysis §2.8 F-ADJ-4.
- `src/Main.UiTests.RealCampaignJourney.cs` — New Game → consume → `TickSimDay` → `SaveAll` → reset → `TryLoadAndRestoreGame` journey; the harness D4 extends.
- `src/Host/HostCli.cs:541` (`--7-day-smoke-selftest`), `:549` (`--composition-root-selftest`), `:395` (`--player-panels-uitest`); `scripts/run_test.sh` present and executable.
- `KNOWN_DEBT.md:16` — seal record with the three green gates.
- `docs/governance/DECISION_PACKET_2026-09-18.md:306` — D23 item 1: migration `PROMOTED-TO-QUEUE` as `QUEUE-PLAN28-MAIN-CONSTRUCTOR-MIGRATION`, strategy "staged migration … gated by MainTriadDriftGateTests".
- `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md:85` — this package's queue row: "none — bounded host change".

**Ownership evidence**
- `WORKTREE_OWNERSHIP.md` (read in full): the only ACTIVE claims are `claim-xp-wave1-difficulty-2026-09-18` (Difficulty core/data/tests + XP governance — no path overlap) and `claim-wave11-part2-execution-2026-09-18` (governance ledgers, census, `Main.Endgame.cs`, port-contract files). `src/Main.CampaignServices.cs`, `src/Main.Lifecycle.cs`, `src/Main.SaveOrchestrator.cs` are **not actively claimed**; they are shared roots subject to the sole-active-builder rule. Governance files (census, `INTEGRATION_PLANS.md`, `KNOWN_DEBT.md`) belong to the wave11-part2 integrator — P3 routes through them.
- `INTEGRATION_PLANS.md` current batch is XP Expansion W1; CF-P28 runs from the standing unblocked queue, not the current batch, exactly as its audit row states.

---

# 5. Existing Extension Seams

This package is *entirely* an extension of existing seams; it introduces no new architectural mechanism.

1. **`SubsystemDescriptor.SetupAction` + `SubsystemManifest.RegisterSetupAction` / `ExecuteSetup`** (Core, sealed) — the declarative registration seam. Unmodified.
2. **`Main.RegisterManifestSetupActions` / `Main.ExecuteSubsystemManifestBootstrap`** (`src/Main.Lifecycle.cs`) — the host binding seam, including the `LifecyclePhase?` scoping parameter (unused by this package; preserved for future staged migrations). Unmodified.
3. **`ComposeCampaign()` composition root** (`src/Main.CampaignServices.cs`) — the fresh-path seam receiving one statement.
4. **`SessionLifecycleRegistry` participants + `ResetPlansExpansionSessions` / `ResetEnrolledFlagshipSessions` / flagship participant** (`src/Main.Lifecycle.cs`, `src/Main.FlagshipInstitutions.cs`) — the reset seam receiving the three-field enrollment. The `_skyDefense` reset at `Main.FlagshipInstitutions.cs:71` is the exact precedent.
5. **`MainTriadDriftGateTests` source-scan pattern** (`Ashfall.Core.Tests/Tooling/`) — balanced-body extraction, method-call reachability, allowlist-with-disposition; the new gate file is a sibling, not a modification, except where noted in §20.
6. **Composition-root and real-campaign-journey selftests** (`src/Main.UiTests.CompositionRoot.cs`, `src/Main.UiTests.RealCampaignJourney.cs`) — the runtime assertion seams, extended additively (new checks appended; no existing check weakened).
7. **Day-owner lazy-Ensure pattern** (`src/Main.CampaignOwners.cs`) — remains as the safety net; after D1 it simply becomes a no-op path for the three expansion subsystems.

Collision check (mandatory): no other "bootstrap", "composition", or "lifecycle parity" mechanism exists under another name. `SessionLifecycleRegistry` handles *teardown* order, not construction; `SaveSectionRegistry` maps sections to `SetupMethod`/`SaveMethod` *names* for the triad gate but executes nothing; `HostSessionContracts`/`IWiringReporter` (Plan 36B) report wiring state, not lifecycle. Extending the manifest bootstrap is the only viable route; creating any parallel "fresh bootstrap" would violate AGENTS.md rule 5.

---

# 6. Proposed Architecture

## 6.1 Options considered

**Option A — bootstrap at the top of `ComposeCampaign()`** (immediately after `SetupHoldfastRuntime()`, mirroring the restore path's relative position). *Rejected.* On the restore path the bootstrap runs before ~120 direct calls; on the fresh path this would construct `journal`/`survivors`/`inventory`/`world` before `SetupCampaignDay()`'s direct call, before `SetupEventsHost()`, before `SetupStartingLevel()` — a wholesale reorder of 15 existing construction events on the most sensitive path in the game, bought for zero functional gain (the seal already proves the delegates tolerate early execution, but the fresh path has fresh-only consumers, e.g. starting-supplies seeding, whose relative order today is deliberate). Not a "minimum safe change".

**Option B — bootstrap after the core block, before cross-wiring (chosen).** Insert the single statement `ExecuteSubsystemManifestBootstrap();` immediately after `SetupExpansions()` (statement #40) and before the `// Wiring that needs all services up` block. Properties:

- Construction order of every session that exists on the fresh path today is **byte-identical** — all 15 directly-called manifest members and both transitively-constructed ones (`research`, `memorial`) are already built; their delegates no-op.
- The three newly eager constructions (`shelter_defense`, `vehicle_garage`, `black_market`) run at a point where **all their dependencies provably exist**: `SetupBlackMarket` needs `_holdfastRuntime` (#2), `_inventory` (#7), `_economy` (#17); `EnsureSkyDefense` needs only data catalogs and its save store; `EnsureVehicleGarage` needs `_campaignDay` (#1, RNG fork — position-independent per §4) and `vehicle_modifications.json`. None of the three has any consumer in statements #41–79 that would observe a presence difference (their day owners and panels `Ensure*`-guard anyway).
- The subsequent cross-wiring block and the Plans 178-201 / expanded-shelter blocks are untouched and see a superset of today's constructed state.
- The insertion sits *inside* the core-composition region, keeping the "Wiring that needs all services up" comment truthful for any future manifest-migrated member (unlike a trailing sweep at the end of the method — the rejected Option B′ variant — which would teach the wrong mental model and put future migrated members after the cross-wiring pass).

**Option C — replace the 15 direct calls with the bootstrap** (manifest becomes the ordering authority on both paths). *Rejected for this package.* That is the EN-06 / wide-migration end-state; it requires per-member ordering proofs across two ~80-call lists and is explicitly out of scope per both prior program treatments ("MUST NOT DO: rewrite ComposeCampaign").

**Option D — document the split as intended and pin each path's set separately.** *Rejected.* It ratifies the divergence the package exists to close, and leaves `journal`-style bootstrap-only members one restore-path refactor away from silent loss.

## 6.2 The parity contract (what "one bootstrap path" means after this package)

> **Contract PC-1.** Both campaign lifecycle entry points — fresh (`ComposeCampaign`) and restore (`RestoreAllSubsystemsFromDisk`) — execute `ExecuteSubsystemManifestBootstrap()` exactly once per composition, unscoped (all phases).
> **Contract PC-2.** Every manifest descriptor with `HasDedicatedSetup == true` has exactly one registered `SetupAction`, and every registered id exists in the manifest (no orphan registrations, no uncovered descriptors).
> **Contract PC-3.** Every manifest-mapped host session field is enrolled in `ResetAllSessionsInMemory()`, so each bootstrap execution composes into a clean per-campaign graph.
> **Contract PC-4.** After either path completes, all 18 manifest-mapped session fields are non-null, and no subsequent idempotent re-entry (second `ComposeCampaign`, panel open in any order, day-owner tick) replaces any of those instances within the same campaign lifetime.

PC-1/PC-2 are pinned statically (xUnit); PC-3 statically + dynamically; PC-4 dynamically (selftests).

## 6.3 Gate architecture

- **Static gate (xUnit, no Godot session):** `BootstrapPathParityGateTests` scans `src/Main.*.cs` and `Assets/Ashfall.Core/Orchestration/SubsystemManifest.cs` sources exactly the way `MainTriadDriftGateTests` does (same repo-root resolution, same balanced-body extraction). Chosen over runtime reflection because it cannot be polluted by the process-global manifest registry (see §18, isolation note) and runs in the standard 180-second focused harness.
- **Runtime assertions (Godot selftests):** appended to the composition-root selftest (field-level, manifest-18 only) and the real-campaign-journey selftest (post-Continue liveness). The selftest family is where `Main` internals are legitimately observable; no production API surface is added for tests.

## 6.4 Reset enrollment design (D2)

One additive block per field, placed in the *existing* participant whose dependency profile matches:

- `_memorial` → the `enrolled_flagship_sessions` participant (`ResetEnrolledFlagshipSessions`, `Main.Lifecycle.cs:~421`) — it already depends on `survivors`/`inventory`/`journal`, and memorial's consumers (medical vigil context, shelter-decor projection, archive chronicle) are rebound at their own setup time. No panel field exists for memorial on `Main` (the cenotaph panel binds per-open from `Main.UiPanels.cs:1262` with a `SetupMemorial()` guard), so no panel teardown is required — P0 verifies this remains true.
- `_blackMarket` → the same participant or the economy-adjacent one; **plus** `_blackMarketPanel` unbind/remove/null. Note `EnsureBlackMarketPanel` (`Main.BlackMarket.cs:60–68`) lacks the `ReferenceEquals` rebind guard that the garage and sky-defense panels have — if the panel outlived its session it would display the stale campaign. The enrollment therefore nulls the panel field too (and removes it from the tree, matching `ResetExpandedShelterSessions`' `RemovePanel` pattern).
- `_vehicleGarage` → the same participant; plus `_vehicleGaragePanel` and `_vehicleGaragePanelBoundSystem` (`Main.VehicleGarage.cs:19–20`) nulled after remove. (`EnsureVehicleGaragePanel` already has the `ReferenceEquals` rebind; nulling keeps the invariant obvious.)

No new participant is created; the existing `onReset` delegates gain 6–9 lines total.

---

# 7. Ownership Matrix

| Concern | Authoritative owner (unchanged) | This package touches? |
|---|---|---|
| Manifest declarations + `SetupAction` registry | `Ashfall.Core.Orchestration.SubsystemManifest` (Core) | **No** |
| Host registration of delegates | `Main.RegisterManifestSetupActions` (`src/Main.Lifecycle.cs`) | **No** |
| Bootstrap executor | `Main.ExecuteSubsystemManifestBootstrap` (`src/Main.Lifecycle.cs`) | **No** |
| Fresh lifecycle entry point | `Main.ComposeCampaign` (`src/Main.CampaignServices.cs`) | **Yes — one statement + comment** |
| Restore lifecycle entry point | `Main.RestoreAllSubsystemsFromDisk` (`src/Main.SaveOrchestrator.cs`) | **No** |
| Session teardown/reset | `SessionLifecycleRegistry` participants (`src/Main.Lifecycle.cs`) | **Yes — 3 fields + 3 panel fields in existing `onReset` delegates** |
| Per-subsystem state authority | Each subsystem's Core system / host session (memorial → `MemorialSystem`; black market → `BlackMarketHostSession`; garage → `VehicleGarageSystem`; etc.) | **No** |
| Save sections and envelope | `SaveSectionRegistry` + `SaveLoadHostSession` + per-section `*SaveStore` | **No** |
| Static parity gate | `Ashfall.Core.Tests/Tooling/` (new sibling of triad gate) | **Yes — new file** |
| Runtime selftests | `src/Main.UiTests.CompositionRoot.cs`, `src/Main.UiTests.RealCampaignJourney.cs` | **Yes — additive checks** |
| Census / ledgers | wave11-part2 integrator (`claim-wave11-part2-execution-2026-09-18`) | **Yes — P3 routed, not self-merged** |

No ownership is ambiguous after this table; the only cross-claim interaction is P3's census edit, which has a named owner.

---

# 8. Data Flow

**Construction flow — fresh path, after D1:**

```
StartNewGame(cohort, supplies)
  ├─ TryCreateFreshCampaignSlot()          // slot first: existing campaigns survive allocation failure
  ├─ ResetAllSessionsInMemory()            // per-campaign graph torn down (incl. newly enrolled trio)
  ├─ _campaignInitializationMode = FreshInitialize
  ├─ ComposeCampaign()
  │    ├─ statements #1–#40 (unchanged order; construct 15 manifest members + research + memorial)
  │    ├─ ExecuteSubsystemManifestBootstrap()   // ← NEW: 18 delegates run; 17 no-op;
  │    │                                       //     shelter_defense/vehicle_garage/black_market construct
  │    ├─ cross-wiring block (unchanged)
  │    ├─ Plans 178-201 block, SetupExpandedShelterSystems, SetupPlans166To169 (unchanged)
  ├─ opening protocol modal bind/open, UpdateHud()
```

**Construction flow — restore path:** unchanged. `TryLoadAndRestoreGame → ResetAllSessionsInMemory → RestoreAllSubsystemsFromDisk → [bootstrap @ step 1] → 122 direct calls (no-ops for manifest members)`.

**Save flow delta:** `SaveAll()` already calls `SaveBlackMarket()`, `SaveVehicleGarage()`, `SaveMemorial()`; each is null-guarded (`Main.BlackMarket.cs:48`, `Main.Plans50_53.cs:73`, `Main.Campaign.cs:280`). After D1 the three sessions exist from composition on a fresh campaign, so their sections are captured starting with the *first* save (today they appear from the first post-construction save — effectively the first day advance). Payloads are non-empty JSON for default state, so the `CaptureSection` empty-payload abort (`Main.SaveOrchestrator.cs:70–77`) cannot trip. No save code changes.

**Restore flow delta:** none in code. Behaviorally, after D2's enrollment, a same-process slot switch now *actually reconstructs* the three sessions from the loaded envelope instead of retaining stale instances — this is the bug fix the isolation test pins.

---

# 9. State Model

No new persisted state, no new save section, no schema change, no Core state type. The complete state delta is in-memory timing and teardown:

| State | Before | After |
|---|---|---|
| `_skyDefense` (fresh campaign) | constructed at first day-1 tick or panel open | constructed during `ComposeCampaign`; `EnsureDefaultTurret()` seeds the default turret at composition instead of first tick |
| `_vehicleGarage` (fresh) | first recovery tick / panel open | during `ComposeCampaign` |
| `_blackMarket` (fresh) | first day-1 pre-day snapshot / panel open | during `ComposeCampaign`; `StateChanged` subscription (HUD refresh hook) live from composition |
| `_memorial`, `_blackMarket`, `_vehicleGarage` on `ResetAllSessionsInMemory` | survive (stale) | nulled; panels removed/nulled; reconstructed by the next bootstrap/Setup call |
| `_manifestSetupRegistered` | per-instance one-shot (unchanged) | unchanged (hazard documented FM-11) |
| Envelope section set for a fresh day-1 save | may omit `black_market`, `vehicle_garage`, `memorial`, `sky_defense_battery` until constructed | includes all four from the first save |

Determinism-relevant state (RNG stream positions) is untouched: `Fork` is position-independent (§4), and no stream is drawn from during any of the three constructors beyond forking.

---

# 10. API/Contracts

**Public API changes: none.** `SubsystemManifest`, `Main`, all host sessions, and all save stores keep their exact current signatures.

**Internal contracts strengthened (the real deliverable):** PC-1…PC-4 (§6.2), plus the following standing requirements that any *future* `SetupAction` migration must satisfy — written down here because the fresh-path invocation makes the manifest the single construction authority for both lifecycles, so these rules now protect two paths instead of one:

- **R1 — Null-guard idempotency.** Every delegate target must be safe to invoke any number of times per campaign (guard or `Ensure*` pattern). Today: holds for all 18 (verified §2.6).
- **R2 — Mode-branch only on `_campaignInitializationMode`.** Fresh-vs-restore behavioral differences must key off the mode field set before composition, never off call order or caller identity. Today: exactly two members branch (`inventory`, `needs`); both compliant.
- **R3 — Position-independent RNG.** Constructors may fork named streams but must not draw from shared streams at construction. Today: holds (`Fork` purity, §4).
- **R4 — Lazy cross-references.** Read peers through `?.` providers or re-bind on every call; never capture a peer instance at construction without a re-bind path. Today: holds (bidirectional re-bind pattern, §2.6).
- **R5 — Restore-at-construction.** Persisted state loads inside the guarded constructor via the section's `*SaveStore.TryLoad()`, so first construction on the restore path is also the restore point. Today: holds for all 18 — and is precisely why reset coverage (PC-3) is mandatory, otherwise R5 executes against the wrong campaign's instance.
- **R6 — No side effects outside the session graph.** A delegate must not touch UI tree, audio, disk (beyond catalog/save reads), or global mutable state beyond its own fields. Today: holds (GD.Print diagnostics excepted).
- **R7 — One registration per descriptor.** `RegisterSetupAction` overwrites silently; double-registration of the same id is a defect the static gate (D3) detects by counting registration sites per id in source.
- **R8 — Registration name truth.** The registered id string must equal a manifest descriptor id. `RegisterSetupAction` currently no-ops on mismatch (verified); the static gate compares the scanned id set against the manifest's scanned id set, making a typo a CI failure instead of a silent omission.

---

# 11. Data Changes

**None.** No file under `Assets/StreamingAssets/Data/` is added, modified, renamed, or removed. No `schema_version` change. `CatalogIntegrityValidator` is untouched. The only catalog reads involved (`vehicle_modifications.json`, black-market catalogs, ordnance catalog) already occur on both paths today; D1 changes only *when* on the fresh path they are first read (composition instead of first tick/panel), not what is read.

---

# 12. Save/Load

**Schema:** unchanged. **Section registry:** unchanged (`black_market`, `vehicle_garage`, `memorial`, `sky_defense_battery` are already enrolled sections — the triad gate's `SaveAll` reachability fact pins their save methods today).

**Behavioral deltas, both directions analyzed:**

1. **Fresh envelope written after D1, loaded by post-D1 code:** sections present from day 1; restore applies them at bootstrap step 1 as today. Trivially compatible.
2. **Fresh envelope written after D1, loaded by pre-D1 code (rollback / older build):** restore path constructs the three sessions (bootstrap or direct call) and their `TryLoad` applies the sections exactly as it does for any long-running campaign today. Compatible.
3. **Pre-D1 envelope (sections absent because never constructed) loaded by post-D1 code:** `TryLoad` returns null at construction; sessions seed defaults (`EnsureDefaultTurret()` etc.) — identical to today's first-day behavior. Compatible.
4. **Envelope save abort risk:** `CaptureSection` aborts the envelope on empty payload (`Main.SaveOrchestrator.cs:70–77`). Freshly constructed default state serializes to non-empty JSON for all three stores (their `TryCapturePersisted` wraps a real `CaptureState()`/`CaptureSave()`); P1's verification includes one fresh day-1 `SaveAll` (the real-campaign-journey selftest already performs exactly this, pre-advance leg available) proving no abort.
5. **Golden fixtures:** `artifacts/golden_saves/` digests pin journey outputs. The generating journeys advance at least one day before `SaveAll` (per `Main.UiTests.RealCampaignJourney.cs`), so the three systems were already constructed at capture time — digests *should* be byte-identical. P0 verifies the golden manifest's coverage rather than assuming; if any day-1-fixture exists, a digest delta limited to the presence of these three sections is the expected, documentable change.

---

# 13. Determinism

- **No `System.Random`, no wall-clock, no hash-order iteration** introduced anywhere (the change is one method call + reset nulling).
- **RNG stream topology unchanged.** The three newly eager constructors fork named streams (`vehicle_garage`, black-market streams, sky-defense if any) — `CampaignRngStream.Fork` derives seeds as a pure function of `(masterSeed, streamId, derivationVersion, day, actionIndex)` and does not advance parent `Position` (`CampaignRngStream.cs:149–166`, verified). Therefore moving construction from first-tick to composition **cannot alter any sequence any system observes**, on either path. This is the load-bearing determinism fact for the whole package and is cited, not assumed.
- **Replay/seeded-replay gates** (`CampaignDayOwnerDeterminismGateTests`, 7-day smoke with mid-run reload) are unaffected in construction and must remain green as package gates (§18).
- **Seeding behavior** (`inventory` starting supplies, `survivors` cohort) keys off `_campaignInitializationMode`, set before composition on both paths; bootstrap position inside composition is irrelevant to it (§2.6).
- **Event ordering:** the three sessions' event subscriptions (`StateChanged`, `OnMemorialized`, `OnVolleyFired`, …) attach earlier on the fresh path, but no producer of those events can fire before the first day advance (the producers are day owners and panel actions), so observable event sequences are unchanged.

---

# 14. System/Event Wiring

No event wiring is added, removed, or rerouted. The wiring consequences of earlier construction:

| Subscription | Attaches (fresh, before D1) | Attaches (fresh, after D1) | Any producer before day 1? |
|---|---|---|---|
| `_blackMarket.StateChanged` → `_economyPanel?.RefreshView()` + `UpdateHud()` (`Main.BlackMarket.cs:37–42`) | first tick/panel | composition | No — producers are `TickDay`/panel actions; both null-guard the panel |
| `_skyDefense.On*` five hooks → `MarkSkyDefenseDirty` (`Main.FlagshipInstitutions.cs:216–220`) | first tick/panel | composition | No — producers are `TickDay`/panel commands |
| `_vehicleGarage` (no host-level events; wear fed by expedition handoff) | first tick/panel | composition | No |
| `_memorial.OnMemorialized` → decor/archive/chronicle projections (`Main.Campaign.cs:215–218`) | composition already (transitive via `SetupMedical`) | unchanged | n/a |

Day-owner registrations (`underworld_market` phase 4, `flagship_institutions` phase 5, garage recovery) happen in `SetupCampaignDay()` — statement #1 on the fresh path, transitively ensured on the restore path via the bootstrap's `weather` action (`SetupWorld` → `SetupCampaignDay`, `Main.World.cs:173`). Their `Setup*` calls inside `TickDay` become guaranteed no-ops after D1; they remain as the R1-compliant safety net.

---

# 15. Godot Integration

- **Scene tree:** the only scene-tree interaction is D2's panel teardown — `_blackMarketPanel` and `_vehicleGaragePanel` are `AddChild`'d at creation; the enrollment removes them via the existing `RemovePanel` pattern (`Main.ExpandedShelterSystems.cs:623–627`) before nulling, so no orphaned `Control` survives a slot switch. `_skyDefensePanel`/`_skyDefensePanelBoundSystem` already handle rebinding (`Main.SkyDefense.cs:41–45`); P0 verifies whether the flagship reset clears them and adds removal only if absent (do not double-free).
- **Headless verification:** all runtime gates run via `godot --headless --path . -- --<selftest>`; no interactive session is required. (If any interactive run is ever needed, the 15-FPS rule applies; none is planned.)
- **`Main` partial-class structure:** the change respects the existing partial boundaries — invocation in `Main.CampaignServices.cs`, reset enrollment in `Main.Lifecycle.cs` (and only if P0 proves it belongs elsewhere, the flagship partial), selftest additions in the two `Main.UiTests.*.cs` files.
- **No new node types, no `project.godot` change, no asset import, no `.tscn` change.**

---

# 16. Narrative/Content Integration

**Inapplicable — stated explicitly rather than omitted.** This package moves construction timing and teardown completeness of three already-shipped subsystems and adds gates; it authors no quests, text, flags, factions, items, or content JSON, and it changes no user-facing string. The only player-observable difference is that a fresh campaign's black market, vehicle garage, and sky-defense battery exist (with their documented default state — e.g. the default turret) from the moment the campaign begins, and that switching campaigns in one session can no longer show the previous campaign's memorial ledger, garage, or syndicate state. The memorial vigil's journal lines, the airdrop/canonical fiction, and all tone rules are untouched.

---

# 17. Failure Modes

| ID | Failure mode | Path | Consequence if unmitigated | Detection | Mitigation in this package |
|---|---|---|---|---|---|
| FM-1 | Bootstrap runs after direct calls on fresh path → double execution | fresh | None today (null-guards); a future non-idempotent delegate would double-fire | Static gate D3 + composition-root idempotency assertions (PC-4) | R1 contract + runtime reference-stability assertion |
| FM-2 | Boot → restore in one process; stale instance survives reset | restore | Loaded campaign shows previous campaign's memorial/garage/black-market state; sections never applied | **Today: nothing** (latent). After D4: two-campaign isolation leg | D2 reset enrollment (P1.5), expected-red-first |
| FM-3 | Future wave migrates a `Setup*` into `SetupAction`-only and removes the direct call | both | Before D1: fresh path silently never constructs it (compiles green, runtime missing — the exact AGENTS.md trap). After D1: impossible | D3 call-site pin; PC-1 | D1 itself is the mitigation |
| FM-4 | `RegisterSetupAction` with typo'd/unknown id silently no-ops | both | Descriptor permanently without setup; no error anywhere | Nothing today | D3 registration-set equality fact (R8) |
| FM-5 | Newly eager construction has an unmet dependency at insertion point | fresh | NullReference /InvalidOperation at `ComposeCampaign` | Build + composition-root selftest + 7-day smoke | Dependency proof §6.1 (all deps in statements #1–#40); restore path already runs the same delegates with strictly *less* context |
| FM-6 | Fresh day-1 envelope gains 3–4 sections | save | Older code loading it: fine (§12.2). Golden digests: possible diff if a day-1 fixture exists | P0 golden-manifest audit | Documented digest delta or empty audit result; no code response needed |
| FM-7 | Panel bound to stale session after reset (panel outlives session) | both | UI shows previous campaign's data | Isolation leg + panel-null assertions | D2 nulls `_blackMarketPanel`, `_vehicleGaragePanel(+BoundSystem)`; `EnsureBlackMarketPanel`'s missing `ReferenceEquals` rebind made irrelevant by removal |
| FM-8 | `ExecuteSetup` throws mid-list (one delegate faults) | both | Subsystems after the fault never construct; exception propagates out of restore (loud) or out of `ComposeCampaign` (loud) | Existing behavior; smoke tests | **Documented, unchanged.** Swallowing exceptions would trade a loud failure for a silent half-composed campaign; per-descriptor try/catch is a decision for the foreman, not this package |
| FM-9 | `ExecuteSetup` return-count misread as "constructed count" | tests | False confidence (it counts *invocations*, no-ops included) | n/a | Gates never assert on the return count for parity; they assert on field state and source pins |
| FM-10 | Second `ComposeCampaign()` (composition-root selftest does this deliberately) | fresh | Must replace nothing | Existing idempotency check + new PC-4 assertions | Already true via guards; assertions pin it |
| FM-11 | Two `Main` instances in one process: static registry overwritten by newest registrar (`_manifestSetupRegistered` is per-instance) | tests | An old instance's bootstrap call would run delegates bound to the new instance | Harness discipline today | Documented as a known hazard; UI selftests drive one composed `Main` at a time; a registry-per-instance redesign is EN-06 scope, not this package |
| FM-12 | Restore path regresses because the three sessions now construct at bootstrap (step 1) *and* P1.5 changed reset | restore | Would surface as 7-day-smoke / save-load failures | `--7-day-smoke-selftest`, `--save-load-ui-failure-selftest`, journey selftest | Restore code path untouched; reset enrollment only *adds* nulling; full restore regression set in §18 |

---

# 18. Test Strategy

Complies with `TEST_POLICY.md`: focused targets only, new test file runs alone first, well under 100 new cases, no full-suite run, Godot headless selftests named explicitly.

## 18.1 New static gate — `Ashfall.Core.Tests/Tooling/BootstrapPathParityGateTests.cs` (6 facts)

Source-scan facts in the `MainTriadDriftGateTests` style (same `RepoRoot()` walk-up, same balanced-body extractor reused by copy — the extractor is private to the triad file; duplicating ~60 lines of scanning helper in a sibling gate is the established pattern in this codebase's tooling tests and keeps the two gates independently reported per the aggregation policy):

- **T1 `RestorePath_InvokesManifestBootstrap_ExactlyOnce`** — body of `RestoreAllSubsystemsFromDisk` contains exactly one `ExecuteSubsystemManifestBootstrap(` invocation. (Green today; pins the sealed behavior.)
- **T2 `FreshPath_InvokesManifestBootstrap_ExactlyOnce`** — body of `ComposeCampaign` contains exactly one invocation. **Red until P1 lands** — this is the package's TDD pin.
- **T3 `ManifestRegistration_CoversEveryDedicatedDescriptor_ExactlyOnce`** — scan `Main.Lifecycle.cs` for `RegisterSetupAction("id", …)` literals and `SubsystemManifest.cs` for descriptor ids with `HasDedicatedSetup: true` (positional record scan); assert the two sets are equal and each id is registered exactly once. Closes FM-4/R7/R8.
- **T4 `RegisteredTargets_Exist`** — every `() => Setup*()` / `() => Ensure*()` / `{ Setup…(); Setup…(); }` target identifier in the registrations resolves to a method declaration somewhere in `src/Main*.cs`. Catches dangling lambda targets at CI speed.
- **T5 `ManifestFields_HaveResetCoverage`** — for the explicit 19-field map (18 ids; `medical` maps to `_medical` **and** `_medicalWard`), assert each field name appears null-assigned inside the reset region of `Main.Lifecycle.cs` / `Main.FlagshipInstitutions.cs` sources. **Red for `_memorial`, `_blackMarket`, `_vehicleGarage` until P1.5 lands.**
- **T6 `BootstrapCallSites_AreExactlyTwo`** — across `src/`, exactly two invocation sites exist: one in `Main.SaveOrchestrator.cs`, one in `Main.CampaignServices.cs`. Prevents both accidental deletion and well-meaning duplication.

Run alone first: `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/BootstrapPathParityGateTests.cs`.

## 18.2 Runtime assertions (additive edits to existing selftests)

- **RT-1 (composition-root selftest, `src/Main.UiTests.CompositionRoot.cs`):** after the first `ComposeCampaign()`, assert all 19 manifest-mapped fields non-null (fails pre-D1 on `_skyDefense`, `_vehicleGarage`, `_blackMarket` — the runtime proof of the gap); assert the same 19 references are `ReferenceEquals`-stable after the second `ComposeCampaign()` and after the shuffled panel-open loop (closes the F-ADJ-4 blind spot **for these fields only**).
- **RT-2 (composition-root selftest, step 4 area):** two-campaign isolation leg — capture `_memorial`/`_blackMarket`/`_vehicleGarage` references after the first `StartNewGame()`, run `ResetAllSessions()` + second `StartNewGame()`, assert all three fields hold **different instances** and `_memorial.Entries` is empty on campaign B. **Expected red before P1.5, green after** — the defect proof and its fix verification in one artifact.
- **RT-3 (real-campaign-journey selftest, `src/Main.UiTests.RealCampaignJourney.cs`):** after `TryLoadAndRestoreGame`, assert the 19 manifest fields non-null (proves restore-path bootstrap coverage end-to-end in the player-shaped journey).
- **RT-4 (7-day smoke, existing):** must remain 10/10 — covers mid-run save/reload, i.e. exercises the reset+restore path the enrollment touches.
- **RT-5 (player panels, existing):** must remain 17/17 — covers panel open/close against the composed graph, now with the three panels' sessions pre-constructed.

## 18.3 Regression set (focused, existing)

- `bash scripts/run_test.sh Ashfall.Core.Tests/Orchestration/SubsystemManifestTests.cs` (7 facts — includes `ExecuteSetup` invocation semantics).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/MainTriadDriftGateTests.cs` (7 facts — triad drift must stay green; the new file does not alter the scanned surface's triad shape).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs` and `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan211BlackMarketHostWiringTests.cs` (host-session-level behavior of two of the three newly eager systems).
- `godot --headless --path . -- --composition-root-selftest` (RT-1/RT-2), `-- --7-day-smoke-selftest` (RT-4), `-- --player-panels-uitest` (RT-5), `-- --vehicle-garage-selftest`, `-- --sky-defense-selftest`, `-- --real-campaign-journey-selftest` (RT-3), `-- --save-load-ui-failure-selftest` (restore regression).

**Test-isolation note:** `SubsystemManifestTests.ExecuteSetup_…` registers a delegate into the process-global manifest. The static gate (18.1) never touches runtime manifest state, and the runtime assertions (18.2) run in the Godot process where `Main.RegisterManifestSetupActions` is the registrar — so no test-order coupling exists between the new gate and the legacy fact. Do not add any xUnit fact that asserts on `descriptor.SetupAction` runtime state; that static field is shared mutable process state by design.

**Completion claim rule:** per AGENTS.md, "compiles green" is not integration; no phase in §19 declares done without its named gates run and reported (command, result, selected tests, known limitation).

---

# 19. Dependency-Ordered Phases

Each phase is independently landable, in this order. Do not start a phase whose predecessor's gate is unmet.

### P0 — Premise audit (read-only; half a day; no file modifications outside the implementation log)

1. Re-run the package's five evidence greps: `ExecuteSubsystemManifestBootstrap` call sites; `RegisterManifestSetupActions` body; each of the 18 delegate targets' call graph; reset null-sites for the 19 mapped fields; `SetupPlasticPyrolysis|SetupCargoAirdrop` callers (confirm F-ADJ-1 still holds).
2. Produce the fresh-vs-restore construction-timing table from current source (§2.4's table, regenerated) and attach it to the implementation log.
3. Verify `WORKTREE_OWNERSHIP.md` still shows no active claim on `src/Main.CampaignServices.cs` / `Main.Lifecycle.cs` / the two selftest files; confirm sole-active-builder sequencing with the foreman.
4. Audit `artifacts/golden_saves/` manifest for day-1-envelope sensitivity (FM-6).
5. Confirm the field-name map (19 fields, incl. `_factionBranch` at `Main.FactionBranch.cs:11`, `_medicalWard` at `Main.World.cs:36`, `_radio` at `Main.Narrative.cs:39`) and whether `_skyDefensePanel` is already cleared in the flagship reset.
6. Record whether the reset gap (§2.7) still exists exactly as described; if another package has since enrolled the fields, P1.5 collapses to adding RT-2 as pure prevention (green on first run) — record which branch was taken.

**Gate:** written audit in the implementation log; foreman-visible before any edit. **Exit:** table + grep logs attached; ownership confirmed.

### P1 — Fresh-path invocation + static gate skeleton

1. `src/Main.CampaignServices.cs`: insert after `SetupExpansions();` (statement #40):
   ```
   // CF-P28-ONE-BOOTSTRAP-PATH: the fresh lifecycle runs the same declarative
   // manifest bootstrap as the restore path (RestoreAllSubsystemsFromDisk).
   // All 18 delegates are idempotent; this constructs any manifest subsystem
   // the direct calls above did not, so the two lifecycles cannot diverge.
   ExecuteSubsystemManifestBootstrap();
   ```
2. `Ashfall.Core.Tests/Tooling/BootstrapPathParityGateTests.cs`: create with T1–T4, T6 (T5 deferred to P1.5 so its red state names the enrollment). T2 flips red→green with this phase's edit.

**Gates:** `dotnet build Ashfall.csproj` 0 errors; `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/BootstrapPathParityGateTests.cs` (alone, first run) green; `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/MainTriadDriftGateTests.cs` 7/7; `bash scripts/run_test.sh Ashfall.Core.Tests/Orchestration/SubsystemManifestTests.cs` 7/7; `godot --headless --path . -- --7-day-smoke-selftest` 10/10; `godot --headless --path . -- --player-panels-uitest` 17/17; `godot --headless --path . -- --composition-root-selftest` green (RT-1 not yet added; existing assertions must hold — the bootstrap addition must not change the current instance-capture behavior beyond the three fields, which the existing blind spot does not observe).
**Exit:** one invocation landed; static pins green.

### P1.5 — Reset coverage enrollment (evidence-driven)

1. Add RT-2 (two-campaign isolation leg) to the composition-root selftest **first**; run it; capture the expected red (`_memorial`/`_blackMarket`/`_vehicleGarage` identical across campaigns A/B) as the defect evidence. If it is unexpectedly green, stop and re-audit (someone else fixed it, or the leg is wrong).
2. Enroll the three sessions + `_blackMarketPanel` + `_vehicleGaragePanel` + `_vehicleGaragePanelBoundSystem` in `ResetEnrolledFlagshipSessions` (or the participant P0 designates), using the `RemovePanel` pattern; no new participant, no new registry concept.
3. Add T5 to the static gate (red→green with the enrollment).
4. Re-run RT-2 (green), RT-4 (10/10), RT-3's journey (green), `--vehicle-garage-selftest`, `--sky-defense-selftest`, `--save-load-ui-failure-selftest`.

**Gates:** as listed; `dotnet build` 0 errors; focused xUnit set from P1 re-run.
**Exit:** isolation leg green; no restore regressions.

### P2 — Runtime parity assertions

1. RT-1 additions (19-field non-null + reference-stability across double-compose and shuffled panels).
2. RT-3 addition (post-Continue 19-field non-null in the journey selftest).

**Gates:** `--composition-root-selftest` green with new assertions; `--real-campaign-journey-selftest` green; re-run RT-4/RT-5.
**Exit:** PC-4 dynamically pinned.

### P3 — Census and ledger truth (routed)

1. Hand the wave11-part2 integrator the evidence bundle (this plan + P0 audit + gate results) to flip C2[9] to `SEALED` in **both** census locations (table row ~90/~223 and stale §5 line ~185), citing: bootstrap on both paths, 6-fact static gate, runtime parity assertions, reset enrollment.
2. `KNOWN_DEBT.md`: no new row (the DEBT-PLAN28 row stays RETIRED; CF-P28 is queue execution, not debt). `INTEGRATION_PLANS.md`: queue row annotated by the integrator per their ledger discipline. Generated indexes regenerated only through their owning generators with `--check`.

**Gate:** integrator confirms ledger edits; docs-index `--check` green.

### P4 — Closeout

Implementation log completed (outcome, files, contracts, commands/results, limitations, intentionally untouched shared paths), handoff per `AI_AGENT_WORKFLOW.md`, including the F-ADJ-1/2/4 findings routed to the foreman as candidate follow-ups.

---

# 20. File Impact Map

| File | Action | Reason | Risk |
|---|---|---|---|
| `src/Main.CampaignServices.cs` | MODIFY — one statement + 4-line comment | D1: fresh-path bootstrap invocation | Medium — shared composition root; sole-active-builder; order-preserving by construction |
| `src/Main.Lifecycle.cs` | MODIFY — 6–9 lines inside existing `onReset` delegates | D2: reset enrollment for `_memorial`, `_blackMarket`, `_vehicleGarage` (+ note; participant choice per P0) | Medium — teardown path; gated by RT-2/RT-4 |
| `src/Main.BlackMarket.cs` | MODIFY only if P0 prefers the panel teardown to live beside `EnsureBlackMarketPanel` (helper `ResetBlackMarketSession()` called from the participant) | D2 locality | Low |
| `src/Main.VehicleGarage.cs` | MODIFY only if P0 prefers the same locality pattern | D2 locality | Low |
| `Ashfall.Core.Tests/Tooling/BootstrapPathParityGateTests.cs` | NEW — 6 facts (T1–T6) | D3 static parity gate | Low — new isolated file, runs alone first |
| `src/Main.UiTests.CompositionRoot.cs` | MODIFY — additive RT-1/RT-2 checks | D4 runtime parity + isolation proof | Low-Medium — selftest-only code; must not weaken existing checks |
| `src/Main.UiTests.RealCampaignJourney.cs` | MODIFY — additive RT-3 check | D4 restore liveness | Low |
| `docs/plans/CF_P28_ONE_BOOTSTRAP_PATH_INTEGRATION_PLAN.md` | NEW — this document | package authority | None |
| `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` | MODIFY — **by wave11-part2 integrator only** | D5 census truth (two rows) | Low — governance |
| `INTEGRATION_PLANS.md` | MODIFY — **by integrator only** | queue annotation | Low — governance |

**Explicitly untouched and why:** `Assets/Ashfall.Core/Orchestration/SubsystemManifest.cs` (sealed surface; no API need — FM-9 avoided by gate design); `src/Main.SaveOrchestrator.cs` (restore order is the sealed baseline); all 18 delegate target bodies (R1–R6 already hold); `Ashfall.Core.Tests/Tooling/MainTriadDriftGateTests.cs` (sibling, not edit); `src/Main.SaveOrchestrator.cs.theirs` (F-ADJ-2, repo-hygiene scope); all `Assets/StreamingAssets/Data/**` (§11); all panels except the two fields' teardown (§15).

---

# 21. Risks

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Insertion-point dependency surprise (FM-5) despite the proof | Low | High (fresh game fails to boot) | P0 dependency re-verification; RT-4/RT-5/composition gates before any later phase; restore path already runs the same delegates with less context |
| P1.5 enrollment unmasks a hidden reliance on the stale-instance leak (some consumer accidentally "worked" because state survived) | Low-Medium | Medium | RT-2 expected-red first documents the exact prior behavior; RT-4 + journey + save-load gates cover restore; rollback is one block deletion (§23) |
| Golden digest drift (FM-6) | Low | Low (cosmetic fixture update) | P0 audit; if a day-1 fixture exists, rebaseline with the delta documented as section-presence-only |
| Gate brittleness: source-scan facts break on cosmetic refactors (renamed method, moved partial) | Medium | Low (CI failure with a named row) | Facts name the exact symbol they pin; failure text instructs "update the pin with disposition" per triad-gate precedent |
| Scope creep into EN-06 territory (unifying non-manifest laziness, F-ADJ-1) | Medium | Medium | §22 hard boundary; F-ADJ findings routed to foreman, not fixed |
| Registry aliasing in multi-`Main` test processes (FM-11) | Low | Medium (confusing selftest behavior) | Documented hazard; harnesses keep one composed `Main`; no production path creates two `Main` instances |
| Concurrent claim appears on shared roots mid-package | Low | Medium | P0 ownership check + sole-active-builder sequencing; stop-and-report per rule 10 if a conflicting ACTIVE claim appears |

---

# 22. Out of Scope

1. Migrating any additional `Setup*` call into a manifest `SetupAction`, or removing any of the 15 direct calls from `ComposeCampaign()` (Option C — EN-06 / future migration waves).
2. Reordering, deduplicating, or re-phasing `RestoreAllSubsystemsFromDisk()`'s 122 direct calls.
3. The ~60 non-manifest subsystems whose construction timing differs between paths (lazy day-owner/panel construction), including F-ADJ-1 (`SetupPlasticPyrolysis` / `SetupCargoAirdrop` fresh-only construction) — recorded for the foreman as a candidate sibling package.
4. Per-descriptor exception containment in `ExecuteSetup` (FM-8) — a semantics decision for the foreman.
5. Registry-per-instance redesign of the static `SubsystemManifest` setup registry (FM-11).
6. Flipping the composition-root selftest's global `null→constructed` blind spot for non-manifest fields (F-ADJ-4 wider hardening).
7. `src/Main.SaveOrchestrator.cs.theirs` removal (F-ADJ-2 — repo hygiene lane).
8. Any Core API addition (e.g., executed-id capture in `ExecuteSetup`) — evaluated and rejected as unnecessary for the gates chosen.
9. Any data, content, narrative, audio, UI-layout, or save-schema change.
10. `KNOWN_DEBT.md` edits (no debt row is created, retired, or re-opened by this package).

---

# 23. Rollback Strategy

All changes are additive and independently revertible; there is no data migration to unwind.

- **Rollback of D1 (invocation):** delete the one statement + comment in `ComposeCampaign()`; T2/T6 fail loudly if anyone forgets the gate edit — revert the gate file with it. Fresh path returns exactly to today's behavior (lazy construction of the trio at first tick/panel). No envelope compatibility concern in either direction (§12).
- **Rollback of D2 (reset enrollment):** delete the enrollment block; RT-2 and T5 fail loudly naming the regression. Note the deliberate coupling: D2's tests are the defect proof, so a silent rollback is structurally impossible.
- **Rollback of D3/D4 (gates):** delete the new test file; remove the additive selftest checks. No production code depends on them.
- **Rollback of D5 (census):** integrator reverts the two census rows with a pointer to the rollback reason.
- **Partial rollback safety:** D1 without D2 is safe (today's latent leak unchanged, isolation leg red documenting it); D2 without D1 is safe (enrollment is strictly more correct teardown); either ordering of rollback preserves a buildable, gate-green tree once the paired test edits revert together.

---

# 24. Definition of Done

1. `ComposeCampaign()` contains exactly one `ExecuteSubsystemManifestBootstrap();` invocation at the §6.1 position; `RestoreAllSubsystemsFromDisk()`'s invocation is byte-identical to today.
2. `BootstrapPathParityGateTests` 6/6 green, run alone via `bash scripts/run_test.sh`.
3. `_memorial`, `_blackMarket`, `_vehicleGarage` (+ their panel fields) are nulled inside the lifecycle reset region; T5 green.
4. RT-1/RT-2/RT-3 assertions live; `--real-campaign-journey-selftest`, `--7-day-smoke-selftest` (10/10), `--player-panels-uitest`, `--vehicle-garage-selftest`, `--sky-defense-selftest`, `--save-load-ui-failure-selftest`, and `--composition-root-selftest` are green on `godot --headless`.
5. `MainTriadDriftGateTests` 7/7, `SubsystemManifestTests` 7/7, `Plan50VehicleGarageIntegrationTests`, `Plan211BlackMarketHostWiringTests` green; `dotnet build Ashfall.csproj` 0 errors.
6. P0 audit (construction-timing table, grep logs, golden-fixture audit result, ownership confirmation) attached to the implementation log.
7. C2[9] shows `SEALED` in both census locations with evidence links (integrator-committed).
8. F-ADJ-1/2/4 recorded to the foreman; none silently absorbed into this package.
9. Handoff per `AI_AGENT_WORKFLOW.md`: outcome, files, contracts (PC-1…PC-4, R1–R8), commands + results, limitations, shared paths intentionally untouched.

---

# 25. Implementation Handoff

## MUST PRESERVE

- The restore path exactly as sealed: `ExecuteSubsystemManifestBootstrap()` remains the first composition act inside `RestoreAllSubsystemsFromDisk()` (`src/Main.SaveOrchestrator.cs:163`), and the 122-call direct list is byte-identical.
- `SubsystemManifest`'s Core purity and API (engine-free; `RegisterSetupAction`/`ExecuteSetup` unmodified, including its silent-unknown-id behavior — gated, not changed).
- All 18 delegate bodies' existing idempotency guards, lazy providers, and bidirectional re-bind behavior (R1–R6).
- `_campaignInitializationMode` set-before-composition ordering in both `StartNewGame` and `TryLoadAndRestoreGame`.
- Every existing selftest assertion in the two edited `Main.UiTests.*` files (additions only; nothing weakened or re-baselined without evidence).
- Unrelated dirty worktree state, including `src/Main.SaveOrchestrator.cs.theirs` and the untracked `Seal-steps/` files.
- The two ACTIVE worktree claims (XP-WAVE1, WAVE11-PART2); census/ledger edits only via their owner.

## MUST ADD

- One statement — `ExecuteSubsystemManifestBootstrap();` — in `ComposeCampaign()` immediately after `SetupExpansions();`, with the §19-P1 comment.
- Reset enrollment for `_memorial`, `_blackMarket`, `_vehicleGarage` and the panel fields `_blackMarketPanel`, `_vehicleGaragePanel`, `_vehicleGaragePanelBoundSystem`, inside an existing lifecycle reset participant (no new participant), using the `RemovePanel` pattern for tree-attached panels.
- `Ashfall.Core.Tests/Tooling/BootstrapPathParityGateTests.cs` with facts T1–T6 as specified in §18.1.
- RT-1 (19-field non-null + reference stability), RT-2 (two-campaign isolation, expected-red-first), RT-3 (post-Continue 19-field non-null) as specified in §18.2.
- The P0 premise audit and phase gate results in the implementation log.

## MUST NOT DO

- Do not rewrite, reorder, or "clean up" `ComposeCampaign()`'s 79 statements or `RestoreAllSubsystemsFromDisk()`'s 122 calls beyond the single insertion.
- Do not migrate any further `Setup*` into `SetupAction`, and do not remove any direct call that duplicates a manifest delegate — duplication is the intended transitional state.
- Do not touch `SubsystemManifest.cs`, any save store, `SaveSectionRegistry`, any JSON under `Assets/StreamingAssets/Data/`, or any panel class beyond the two panel-field teardown lines.
- Do not fix F-ADJ-1 (pyrolysis/airdrop restore-path construction), F-ADJ-2 (`.theirs` file), or the global composition-test blind spot inside this package; route them.
- Do not use `System.Random`, wall-clock seeds, or process-global mutable state to implement any of this.
- Do not run the full test suite; use the focused commands in §18 only.
- Do not edit `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`, `KNOWN_DEBT.md`, or the census directly if another claim owns them at execution time — re-check at P0 and route.

## VERIFY WITH

- `dotnet build Ashfall.csproj` — 0 errors after each phase.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/BootstrapPathParityGateTests.cs` — new gate, alone first (T2 red→green at P1, T5 red→green at P1.5).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/MainTriadDriftGateTests.cs` — 7/7.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Orchestration/SubsystemManifestTests.cs` — 7/7.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs`; `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan211BlackMarketHostWiringTests.cs`.
- `godot --headless --path . -- --composition-root-selftest` (RT-1/RT-2); `-- --real-campaign-journey-selftest` (RT-3); `-- --7-day-smoke-selftest` (10/10); `-- --player-panels-uitest`; `-- --vehicle-garage-selftest`; `-- --sky-defense-selftest`; `-- --save-load-ui-failure-selftest`.
- Evidence greps to re-run at P0: `grep -rn "ExecuteSubsystemManifestBootstrap" src/`; `grep -rn "SetupPlasticPyrolysis\|SetupCargoAirdrop" src/`; `grep -rn "_memorial = null\|_blackMarket = null\|_vehicleGarage = null" src/`.

## FIRST SAFE IMPLEMENTATION STEP

**P0, read-only:** regenerate the §2.4 construction-timing table from current source (five greps + two method-body reads), confirm no ACTIVE claim covers `src/Main.CampaignServices.cs` / `src/Main.Lifecycle.cs` / the two selftest files in `WORKTREE_OWNERSHIP.md`, audit `artifacts/golden_saves/` for day-1 envelope sensitivity, and write the audit into the implementation log — no production file is edited before that log exists.

# 26. Execution closeout — 2026-09-19

## P0 premise audit

- Current source has exactly two production bootstrap call sites: the pre-existing restore call in `src/Main.SaveOrchestrator.cs` and the new fresh-path call immediately after `SetupExpansions()` in `src/Main.CampaignServices.cs`. The restore call and its direct setup list were left untouched.
- The reset seam now nulls `_memorial`, `_blackMarket`, and `_vehicleGarage`; it also unbinds/removes the black-market and vehicle-garage panels and clears the vehicle panel's bound-system field. No second reset participant was introduced.
- The golden-save manifest contains one day-1 fixture with required sections `campaign_day`, `inventory`, `power_grid`, `starting_level`, and `journal`; it has no armor/garage/market/memorial section and required no digest rebaseline.
- `SetupPlasticPyrolysis` and `SetupCargoAirdrop` remain fresh-only construction findings and were not absorbed into this bounded manifest package. No active claim overlapped the four implementation/selftest paths at claim time; shared census/ledger edits were routed through the existing integrator claim.

## Implemented surface

1. `ComposeCampaign()` now executes the existing manifest bootstrap once after `SetupExpansions()`.
2. The existing lifecycle reset participant enrolls the three restore-at-construction sessions and their panel state.
3. `BootstrapPathParityGateTests` pins the two call sites, all dedicated descriptor registrations and targets, and reset coverage for the 19 mapped host fields.
4. Composition-root and real-campaign selftests contain the requested non-null, reference-stability, isolation, and post-restore assertions.
5. C2[9] was reconciled to `SEALED` in both census locations; both pre-existing gate limitations (`EconomyDetailPanel.tscn` `%Content` node / `_heatMapDetail` lifecycle and `MainTriadDriftGateTests` difficulty allowlist) were completely resolved and verified green.

## Verification

| Gate | Result |
|---|---|
| `BootstrapPathParityGateTests` | 6/6 |
| `SubsystemManifestTests` | 7/7 |
| `Plan50VehicleGarageIntegrationTests` | 5/5 |
| `Plan211BlackMarketHostWiringTests` | 3/3 |
| `dotnet build Ashfall.csproj --no-restore` | 0 warnings / 0 errors |
| `--7-day-smoke-selftest` | 10/10 PASS |
| `--player-panels-uitest` | PASS |
| `--real-campaign-journey-selftest` | PASS |
| `--vehicle-garage-selftest` | 27/27 PASS |
| `--sky-defense-selftest` | 17/17 PASS |
| `--save-load-ui-failure-selftest` | 8/8 PASS |
| `--composition-root-selftest` | PASS (exit code 0; 183 panels tested, idempotent=True, coreServicesPresent=True) |
| `MainTriadDriftGateTests` | 7/7 PASS (SetupDifficulty allowlisted) |
| `git diff --check` | PASS |

The composition-root selftest and MainTriadDriftGateTests now pass cleanly alongside all other test gates. CF-P28 is 100% complete and fully sealed.
