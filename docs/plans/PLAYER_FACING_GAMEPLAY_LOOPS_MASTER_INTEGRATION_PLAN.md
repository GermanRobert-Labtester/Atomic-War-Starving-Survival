# PLAYER-FACING GAMEPLAY LOOPS — MASTER INTEGRATION PLAN

**Document type:** Implementation-ready master integration plan (planning artifact only)
**Program id:** `PFGL-MASTER-2026-09-25`
**Revision:** R5 — W10 contract verification + precision & honesty seals (2026-09-25)
**Prior revisions:** R1 (~105k W1–W8) · R2 (~200k + W9) · R3 (padding purge + API/port precision) · R4 (PIR gate + live-API hardening + W10 scope)
**Date:** 2026-09-25
**Evidence HEAD:** `1678c0749f49b5e4f9a99231e8e71acf3e47fdb1` — **re-verified at R4 + R5** (Appendix BV)
**User scope:** Missing player-facing gameplay loops (mechanics/features players still cannot meaningfully play)
**Scope expansion (R2 retained):** Seal Plan 188 player-thin routines board + schedule/conflict/satisfaction mechanic
**R3 focus:** Delete budget-padding slots; replace with live-API precision (Romance verbs, NeedsSystem ports, conflict uniqueness)
**R4 focus:** Pre-integration readiness gate (PIR), exact live signatures for all ten waves, seal-quality framework (gap/feature/mechanic), integration-smoothness contract, W10 Sleep Acoustic Recovery scope
**R5 focus:** Live-verify the merged W10 contract end-to-end; close the §10 W10 gap; CLI-flag drift fix; census-assumption + manifest-lag honesty; head-matter W10 completeness
**Production code changed by this document:** none
**Target length:** ~200k characters of evidence-dense prose
**Authorities:** `INTEGRATION_PLANS.md` · `WORKTREE_OWNERSHIP.md` · `TEST_POLICY.md` · `KNOWN_DEBT.md` · `AI_AGENT_WORKFLOW.md`

---

## 0. Executive contract

### What this plan delivers
A dependency-ordered ten-wave program (`PFGL-W1`…`PFGL-W10`) that converts the largest remaining **player-facing** gaps into sealed, playable loops by extending current owners. After the 2026-09-24/25 UNBLOCK seals, most Core islands already have host sessions, save sections, day owners, and CLI probes. The dominant remaining defect class is:

> **Host-complete / player-thin** — the system ticks and persists, but a normal campaign session has no operable board, no truthful command surface, or the live UI binds a *different* authority.

Secondary class:

> **True orphans** — Core authorities with zero `src/` references: `RecruitmentSystem`, `ChronicConditionSystem`, and the intentionally deferred/parallel `EmergencyAlertSystem`.

### Success definition (PLAYER-OPERABLE)
In an ordinary campaign session (CLI probe optional for CI, required for package evidence), a player can:

1. Court / break / form families through UI bound to `RomanceFamilySystem`.
2. Run wilderness/faction recruitment campaigns through a wired `RecruitmentSystem`.
3. Install vehicle modules on the garage seam (`VehicleCustomizationSystem` + `VehicleGarageSystem`).
4. Schedule player trade routes from a routed board (`PlayerTradeRouteSystem`).
5. Found/supply colonies from a board over `ColonySystem` (distinct from apiary “colony”).
6. Operate shelter governance through UI reconciled to one write owner (`ShelterGovernanceEngine` vs `PoliticsSystem` — DEC required).
7. Start wildlife/cooking recipes from the kitchen surface (`CookingSystem` / `CookingHostSession.StartCooking`).
8. Activate disaster mitigation protocols bound to `DisasterResponseSystem`.
9. Select NG+ legacy/meta boons via `CampaignLegacySystem` / `MetaProgressionSystem`.
10. Resolve ideological confrontations and act on NPC grudges/favors.
11. Change difficulty custom lanes / ironman lock mid-run (`DEBT-PLAN181`).
12. Inspect/repair shelter maintenance alerts (`ShelterMaintenanceSystem`).
13. Decision-gated chronic accommodations (`ChronicConditionSystem`) without a parallel medical ledger.
14. Receive authored keepsakes for previously orphaned origin item ids (`DEBT-ENRICHMENT-KEEPSAKE-ORPHANS`).
15. Assign survivor daily routine templates, set chronotype/shift preferences, resolve schedule conflicts, and observe satisfaction feeding morale/needs ports (`SurvivorRoutineSystem` — Plan 188).
16. Improve dormitory sleep recovery by installing sensory-relief kits / soundproofing on the sleep-acoustic ledger, reading quiet-hours compliance from `ShelterNoiseSystem`, and applying Fatigue/Morale deltas through `NeedsSystem` (`SleepAcousticLedger` / Expansion 41 — W10).

### Non-goals (program-wide)
- Greenfield Core for wildland firefront / mobile clinic / public works / scenario library (gap-audit hypotheses stay premise-gated).
- Wiring `WaterSourceSystem` (Plan 189 forbids; Water Sources surface uses deep-well/condenser/piezometer).
- Wiring `FoodTypeSystem` as live owner (Plan 196 sealed on `FoodPreservationSystem`).
- Wiring `FactionDiplomacySystem` parallel to live `RegionalTreatySystem` without custody DEC.
- A second quiet-hours schedule UI that writes only `SleepAcousticLedger` while ignoring live `ShelterNoiseSystem` / `shelter_atmosphere` (W10 must sync or read-through).
- Specialty kiln/press/glass presentation polish ahead of social/economy boards.
- Unity restoration; full-suite-as-default; mass refactors; unclaimed shared-hub edits.

---

# 1. Objective

Make the highest-value already-authored survival, social, economy, and crisis loops **player-operable** inside the Godot host by extending current owners, registering truthful routes where needed, and sealing only true Core orphans that still have zero host reachability.

Primary outcome: a sequenced package train an integrator can claim one wave at a time.

Secondary outcome: retire or promote the two ACCEPTED player-facing debts that block honesty (difficulty runtime panel; keepsake orphans).

# 2. Current Reality (evidence 2026-09-25)

## 2.1 Architecture
| Layer | Path | Role |
|---|---|---|
| Core | `Assets/Ashfall.Core/` | Engine-free gameplay authority |
| Host | `src/` | Thin Godot adapters, panels, CLI |
| Data | `Assets/StreamingAssets/Data/` | JSON catalogs |
| Tests | `Ashfall.Core.Tests/` | Focused xUnit |
| Save pin | `SaveSectionRegistry.All.Count` | **266** |
| Player surfaces | `docs/player_surface_manifest.json` | **219** total; **58** InteractiveCommands / **161** ReadOnlyObservational |
| Port contract | live generated contract | ~307 seams / ~201 HOST_REQUIRED / 0 unbound |

There is no separate `GameBootstrap` type. Host creation is `Main` partials + `HostSessionBase` + lifecycle/save orchestrator.

## 2.2 Recently sealed (do not redo)
Live ledger records full host integration for (non-exhaustive): Plans 143, 151, 155, 159, 162, 165–169, 171–181, 183–185, 187, 192, 198–200, 202, 208, 216; Expansions 25–41; Shelter Operations Board; Water Sources surface; Leadership Succession. Plan-integration audit reports **71/71 INTEGRATED** for its tracked set. That proves host+save+CLI for those rows; it does **not** prove interactive player boards for every authority.

## 2.3 Dominant gap classes
| Class | Meaning | Examples |
|---|---|---|
| TRUE ORPHAN | Core type host_files=0 | `RecruitmentSystem`, `ChronicConditionSystem`, `EmergencyAlertSystem` |
| HOST OK / UI MISSING | Session+save exist; no interactive board | Vehicle customization, trade routes, colony, legacy/meta, shelter maintenance |
| HOST OK / UI MISMATCH | Panel binds a different owner | Politics→`PoliticsSystem` vs governance engine; Kitchen→nutrition vs `CookingSystem` |
| HOST OK / UI THIN | Providers/observational only | Romance on `SurvivorDetailPanel`; ideology labels |
| ACCEPTED DEBT | Known deferred surface/content | `DEBT-PLAN181-DIFFICULTY-RUNTIME-UI`; keepsake orphans |
| FALSE PARALLEL | Do not wire as new live owner | `WaterSourceSystem`, `FoodTypeSystem`, `FactionDiplomacySystem` (until custody) |

## 2.4 Reachability snapshot
| Core type | host files | Save key | Player board |
|---|---:|---|---|
| `RecruitmentSystem` | 0 | none proven for type | Prisoner recruit ≠ Plan 204 |
| `ChronicConditionSystem` | 0 | none proven for type | Afflictions are other owners |
| `EmergencyAlertSystem` | 0 | none | HUD acknowledge-only |
| `RomanceFamilySystem` | 1 (+Main) | `romance_family` | Provider only |
| `VehicleCustomizationSystem` | 1 | `vehicle_customization` | none (garage≠modules) |
| `PlayerTradeRouteSystem` | 1 | `trade_routes` | none |
| `ColonySystem` | 5 | `colony` | none |
| `ShelterGovernanceEngine` | 2 | `shelter_governance` | politics mismatch |
| `CookingSystem` | 3 | `cooking` | kitchen mismatch |
| `DisasterResponseSystem` | 6 | `disaster_response` | HUD thin |
| `CampaignLegacySystem` | 2 | `campaign_legacy` | no board |
| `MetaProgressionSystem` | 1 | `meta_progression` | no board |
| `NpcMemorySystem` | 1 | `npc_memory` | none |
| `IdeologicalFrictionEvents` | 1 | `ideological_friction` | thin |
| `ShelterMaintenanceSystem` | 2 | `shelter_maintenance` | none |
| `DifficultySettingsSystem` | 3 | `difficulty_settings` | start preset only |
| `SurvivorRoutineSystem` | **2** direct type refs (`Main.SurvivorRoutines`, `SurvivorRoutineHostSession`); **9** files touch `SurvivorRoutine*` (adds `HostCli`, `HostCli.SurvivorRoutines`, `Main.Application`, `Main.CampaignOwners`, `Main.Lifecycle`, `Main.SaveOrchestrator`, `SurvivorDetailPanel`) | `survivor_routines` | **ReadOnly provider only** — no assign/resolve board |
| `SleepAcousticLedger` / `SleepAcousticRestEngine` | **8** files touch `SleepAcoustic*` in `src/` (Core `Assets/Ashfall.Core/Needs/`, `SleepAcousticRestHostSession`, `Main.SleepAcousticRest`, `HostCli.SleepAcousticRest`, day owner `CampaignOwners:176`, save orchestrator) | `sleep_acoustic_rest` (in pin 266) | **none** — W10 seals computed-then-discarded sleep results (C12) |

### 2.4.1 Accuracy corrections (R2 polish pass)
| Prior claim / risk | Live evidence (HEAD `1678c074`) | Plan treatment |
|---|---|---|
| Stealth weapon-noise “unbridged” from some older gap notes | `CombatHostSession.ActionFire` already calls `Stealth.ApplyWeaponNoise(...)` after `Engine.PlayerFire` | **Out of scope / already sealed** — do not reopen as PFGL work |
| Plan 188 “no dedicated Core type” in some queue notes | Live type is `SurvivorRoutineSystem` under `Assets/Ashfall.Core/Survivors/` | Treat as host-complete / player-thin; W9 seals the board |
| “Nine packages” framing | R2 expands to nine | All W1–W9 contracts retained; W9 appended |
| Save pin drift | `Assert.Equal(266, SaveSectionRegistry.All.Count)` still live | W9 reuses existing `survivor_routines` section — **no pin bump** unless a new section is proven necessary |
| Interactive surface count | `docs/player_surface_manifest.json`: 219 total / 58 interactive / 161 read-only | W9 promotes routines from observational provider → InteractiveCommands (+1 interactive when routed) |
| Satisfaction→needs coupling | `TickSurvivorRoutines` evaluates scores but does not yet port OverallSatisfaction into morale/needs owners | W9 mechanic seals a **read→port** adapter into existing Needs/morale owners (no parallel ledger) |

### 2.4.2 R4 pre-integration verification pass (2026-09-25, HEAD `1678c074`)
Every program premise was re-probed against live source at R4. Verified-green premises and corrected drift:

| Probe | R4 result | Status |
|---|---|---|
| `git rev-parse HEAD` | `1678c074…` — same as plan stamp | VERIFIED |
| Save pin `Assert.Equal(266, …)` | 266 in `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs:315,317` | VERIFIED |
| All 14 named save keys in `SaveSectionRegistry` | `romance_family`, `vehicle_customization`, `trade_routes`, `colony`, `shelter_governance`, `cooking`, `disaster_response`, `campaign_legacy`, `meta_progression`, `npc_memory`, `ideological_friction`, `shelter_maintenance`, `difficulty_settings`, `survivor_routines` — all present | VERIFIED |
| Orphan counts (`\bType\b` in `src`) | Recruitment=0, Chronic=0, EmergencyAlert=0 | VERIFIED (W2/W8 premises hold) |
| W1 romance verbs | `TryInitiateAttraction` / `ConductCourtshipEvent` / `DissolvePartnership(survivorA, survivorB, reason="mutual_drift")` / `FormFamilyUnit` / `AddChildToFamily` / `CalculateCompatibility` / `AdvanceDay` live on `RomanceFamilyHostSession` | VERIFIED (+`AddChildToFamily` added) |
| W2 recruitment verbs | `StartCampaign` / `DiscoverCandidate` / `MakeDefectionOffer` / `TickDay(day)` live; **`CancelCampaign`, `TickCampaigns`, `AcceptCandidate` do not exist** | DRIFT CORRECTED (§10) |
| W3 vehicle verbs | `InstallModule(vehicleId, moduleId, maxSlots=4)` / `RemoveModule` / `GetInstalledModules` + base-camp/bunk/rest verbs live | VERIFIED (+depth added) |
| W4 trade verbs | Host type is **`TradeRouteHostSession`**; verbs `EstablishContract` / `SuspendContract` / `ResumeContract` / `CancelContract` / `RecordRunOutcome` / `TickDay(day, funds)` | DRIFT CORRECTED (§10) |
| W4 colony verbs | `EstablishColony` / `ConstructBuilding` / `EstablishSupplyLine` / `SetSupplyLineStatus` / `AssignSurvivorToColony` / `TransferSupplies` — **no EstablishColony/TransferSupplies/Assign verb set**; hosted via `OutpostSettlementHostSession` + `OrphanSealWave1HostSessions` | DRIFT CORRECTED (§10) |
| W5 cooking verbs | `StartCooking` / `ProgressCooking` / `CancelCooking` / `TickDay` live on `CookingHostSession` | VERIFIED (+depth added) |
| W5 disaster verbs | `TriggerDisaster` / `ActivateProtocol` / `DeactivateProtocol` / `IsProtocolActive` / `TickDisaster` / `CalculateRoomDamage` / `AdjustResilience` live; **`ActivateDisasterProtocol` / `ResolveIncident` do not exist on this owner** (`ResolveIncident` is `AirlockSecuritySystem`) | DRIFT CORRECTED (§10) |
| W6 verbs | `PrepareNewGameContext` / `ArchiveCampaign`; `SetNgPlusBoonActive`; `ResolveConfrontation` / `AttemptConversion`; `Forgive` / `IsTradeRefused` / `GetTradePriceMultiplier` live | VERIFIED (+`SetNgPlusBoonActive` named) |
| W7 difficulty verbs | Core `SelectPreset` / `SetCustomScalar` / `LockSettings`; Main `SelectDifficultyPreset` / `SetDifficultyCustomScalar` / `LockDifficultySettings` live; **`SetCustomLane` does not exist** | DRIFT CORRECTED (§10) |
| W7 maintenance verbs | `PerformMaintenance(componentId, actionType, skillLevel, day)` / `GetWarningComponents` / `GetFailedComponents` live; **`ScheduleRepair` / `ClearAlert` do not exist** | DRIFT CORRECTED (§10) |
| W8 chronic verbs | `AddCondition` / `AssignAccommodation` / `RemoveAccommodation` / `CalculateCapabilityModifier` / `GetTotalImpairmentScore` live (planning-only; DEC-CHRONIC gate intact) | VERIFIED |
| W9 routines verbs | All nine §10 verbs live on `SurvivorRoutineHostSession`, incl. `GetActiveConflicts()` / `Census` | VERIFIED |
| NeedsSystem port mutators | `Modify(id, NeedKind, delta)` / `ApplyAttributedDelta(...)` / `SetExternalModifier(...)` live on `NeedsSystem` | VERIFIED (AF4 row resolved) |
| Manifest counts | 219 / 58 / 161 in `docs/player_surface_manifest.json` | VERIFIED |
| `routine_templates.json` | schema 1; `routine_early_riser`, `routine_night_owl`, `routine_standard`, `routine_night_shift` | VERIFIED |
| `HostCliAction.SurvivorRoutinesSelfTest` | Registered in `HostCli.cs:243` + `HostCliRegistry.cs:143,967` | VERIFIED |
| Politics mismatch | `src/UI/PoliticsUI.cs:22,26` binds `PoliticsSystem` | VERIFIED (W5 DEC premise holds) |
| Kitchen mismatch | `KitchenNutritionPanel`/`KitchenNutritionPanelContent` bind nutrition; `CookingHostSession.StartCooking` unused by kitchen | VERIFIED |
| Detail thin | `SurvivorDetailPanel.RoutineProvider` is `Func<string, SurvivorRoutineRecord?>` (read-only) | VERIFIED |
| Stealth bridge | `CombatHostSession.ActionFire` → `Stealth.ApplyWeaponNoise` | VERIFIED (out of program) |

**R4 verdict:** the program architecture is sound and fully integrable, but §10 (and the old Appendix AN catalog) originally carried **invented or stale verb names in W2, W4, W5-disaster, and W7** that would have failed at compile time on first implementation. All are now replaced with live signatures (§10 + R4-31 verb cards) and the drift is logged in Appendix BV / the R4-1 correction ledger. Pre-integration smoothness is gated by the new PIR checklist (Appendix BT).

## 2.5 Extension cookbook (reuse exactly)
1. `src/Host/<X>HostSession.cs` : `HostSessionBase`
2. `src/Main.<X>.cs` — Setup/Save/Reset/commands + journal
3. `src/Main.CampaignOwners.cs` — day owner + `IPreDaySnapshotRestore`
4. `SaveSectionRegistry` (+ pin bump) **or** reuse existing section
5. `HostCliRegistry` + Application dispatch + 12-check selftest
6. UI battery when interactive: `PanelRegistryBootstrap`, `Main.GameFlow`, `Main.PlayerSurfaces`, `PlayerSurfaceManifest`, panel class
7. RNG: `_campaignDay.Rng.Fork(CampaignStreamIds.*, …)` — never `System.Random`
8. Port-contract classify new public seams
9. Claim shared hubs in `WORKTREE_OWNERSHIP.md` before edit

Exemplars: Bestiary / Health History (stateful full loop), Leadership Succession (extend existing save section), Shelter Operations Board (facade, no new section).

# 3. Required Delta

| Gap | Broken completion-chain link |
|---|---|
| Romance | UI command → host → Core (providers exist; board missing) |
| Recruitment | Entire host chain (construct/register/save/UI) |
| Vehicle modules | UI on garage seam → `InstallModule` |
| Trade routes | UI → existing host session |
| Colony | UI → `ColonySystem` (disambiguate apiary) |
| Governance | One-authority map + UI bind |
| Cooking | Kitchen bind or honest dual-surface to `StartCooking` |
| Disaster | Bind HUD/board to mitigation APIs |
| NG+ legacy | Start-flow board |
| Ideology / NPC memory | Resolve/forgive/trade-refusal surfaces |
| Difficulty panel | Routed settings panel |
| Maintenance | Board over alerts/repairs |
| Chronic | DEC then wire-or-retire |
| Keepsakes | CONTENT authorship/remap |
| Daily routines | UI assign/prefer/resolve → existing host; satisfaction→morale/needs port |
| Sleep recovery | W10 board + SleepQualityResult→Needs ports (today the result is computed then discarded — C12); quiet-hours one-write via `ShelterNoiseSystem` |

# 4. Evidence Index

| Evidence | Location |
|---|---|
| Live ledger head | `INTEGRATION_PLANS.md` (Water Sources + Shelter Operations 2026-09-25) |
| Difficulty UI debt | `KNOWN_DEBT.md` `DEBT-PLAN181-DIFFICULTY-RUNTIME-UI` |
| Keepsake orphans | `KNOWN_DEBT.md` `DEBT-ENRICHMENT-KEEPSAKE-ORPHANS` |
| Romance providers | `src/Main.RomanceFamily.cs`, `src/UI/SurvivorDetailPanel.cs` |
| Romance host API | `src/Host/RomanceFamilyHostSession.cs` `FormFamilyUnit` |
| Recruitment orphan | `Assets/Ashfall.Core/Survivors/RecruitmentSystem.cs` + 0 `src/` type refs |
| Vehicle install | `VehicleCustomizationSystem.InstallModule`; host session + CLI |
| Cooking start | `CookingHostSession.StartCooking`; kitchen binds nutrition |
| Politics mismatch | `src/UI/PoliticsUI.cs` binds `PoliticsSystem` |
| Prior audits | `docs/gaps/ASHFALL_IMPLEMENTATION_GAP_AUDIT.md`; `docs/plans/OLDEST_PARTIAL_PLANS_AUDIT_20_2026-09-23.md` (historical) |
| Measurement | `rg -l '\bType\b' src --glob '*.cs' \| wc -l` |
| Survivor routines Core | `Assets/Ashfall.Core/Survivors/SurvivorRoutineSystem.cs` |
| Survivor routines host | `src/Host/SurvivorRoutineHostSession.cs` · `src/Main.SurvivorRoutines.cs` |
| Routines save | `survivor_routines` / `survivor_routines_save.json` (registry row present) |
| Routines catalog | `Assets/StreamingAssets/Data/routine_templates.json` (4 templates) |
| Routines CLI | `HostCliAction.SurvivorRoutinesSelfTest` (12-check) |
| Routines day owner | `Main.CampaignOwners` phase 5 `SurvivorRoutinesDayOwner` |
| Routines UI today | `SurvivorDetailPanel.RoutineProvider` read-only projection only |
| Stealth fire bridge | `src/Host/CombatHostSession.cs` `ActionFire` → `Stealth.ApplyWeaponNoise` |

# 5. Existing Extension Seams

Prefer **facade over new authority** when owners exist (Shelter Operations / Water Sources pattern).
Prefer **extend-existing save section** when state already rides one (Leadership in `survivor_social`).
Prefer **bind-existing panel** before new routes (Kitchen, Politics, Garage, Nursery, Emergency HUD, Survivor Detail, Starting Cohort).
True orphan wiring follows Bestiary/Health History template end-to-end.

# 6. Proposed Architecture

## 6.1 Ten packages (W10 added in R4)
| Package | Id | Loops | Class |
|---|---|---|---|
| Wave 1 | `PFGL-W1-ROMANCE-BOARD` | Romance & family operable board | EXPAND |
| Wave 2 | `PFGL-W2-RECRUITMENT-WIRE` | Recruitment host+save+board | WIRE |
| Wave 3 | `PFGL-W3-VEHICLE-MODULES` | Customization on garage seam | EXPAND |
| Wave 4 | `PFGL-W4-TRADE-COLONY` | Trade routes + colony boards | EXPAND |
| Wave 5 | `PFGL-W5-GOVERNANCE-COOK-DISASTER` | Governance DEC+UI; kitchen cook; disaster protocols | EXPAND |
| Wave 6 | `PFGL-W6-LEGACY-IDEOLOGY-NPC` | NG+ board; ideology resolve; NPC memory acts | EXPAND |
| Wave 7 | `PFGL-W7-DIFFICULTY-MAINT-CONTENT` | Difficulty panel; maintenance; keepsakes | EXPAND+CONTENT |
| Wave 8 | `PFGL-W8-CHRONIC-DECISION` | Chronic one-authority DEC → wire/retire | DECISION→WIRE |
| Wave 9 | `PFGL-W9-SURVIVOR-ROUTINES-BOARD` | Daily routines assign + preferences + conflict resolve + satisfaction ports | EXPAND+MECHANIC |
| Wave 10 | `PFGL-W10-SLEEP-ACOUSTIC-RECOVERY` | Dormitory sleep-quality board + Needs Fatigue/Morale recovery ports; quiet-hours read from ShelterNoise | EXPAND+MECHANIC |

## 6.2 Completion chain
`DECLARED → COMPILED → CONSTRUCTED → REGISTERED → CALLED → MUTATES → OBSERVED → PERSISTED → RESTORED → VERIFIED → PLAYER-OPERABLE`

CLI selftest alone stops at VERIFIED. This program requires PLAYER-OPERABLE.

## 6.3 Collision map (resolve before coding)
| Concern | Extend | Forbidden parallel | DEC? |
|---|---|---|---|
| Romance | `RomanceFamilySystem` | Second relationship ledger | No |
| Recruitment | `RecruitmentSystem` | Overloading Prisoner recruit as Plan 204 | Boundary note |
| Vehicle modules | Customization + Garage | Second garage | Seam map |
| Trade routes | `PlayerTradeRouteSystem` | Second market ledger | No |
| Colony | `ColonySystem` | Apiary/outpost confusion | Naming/UI |
| Governance | TBD politics vs governance engine | Dual elections | **YES** |
| Cooking | `CookingSystem` + kitchen | Duplicate meal authority | Bind map |
| Disaster | `DisasterResponseSystem` | Revive `EmergencyAlertSystem` blindly | **If alerts** |
| Legacy/meta | existing | Second NG+ store | No |
| Ideology | `IdeologicalFrictionEvents` | Parallel zealotry writes | Boundary |
| NPC memory | `NpcMemorySystem` | Parallel relations store | Boundary |
| Difficulty UI | `DifficultySettingsSystem` | Second scalar provider | No |
| Maintenance | `ShelterMaintenanceSystem` | Second HP ledger | No |
| Chronic | TBD vs afflictions/amputation/health history | Parallel medical | **YES** |
| Keepsakes | item catalog | Runtime item invention | Content only |
| Daily routines | `SurvivorRoutineSystem` | Second schedule ledger; inventing new sleep HP | No (ports only) |
| Sleep acoustic (W10) | `SleepAcousticLedger`/`SleepAcousticRestEngine` + `ShelterNoiseSystem` quiet hours | A second quiet-hours schedule or a wellbeing store in `SleepAcousticState` | No (one-write rule R4-50) |
| Stealth fire noise | already on `CombatHostSession.ActionFire` | Re-bridging as PFGL work | Out of program |

# 7. Ownership Matrix (program)

| Concern | Owner | Host | Save | UI | RNG | Observability |
|---|---|---|---|---|---|---|
| Romance | `RomanceFamilySystem` | `RomanceFamilyHostSession` | `romance_family` | Detail actions / optional route | Social fork | journal |
| Recruitment | `RecruitmentSystem` | create session | new or justified | muster/recruitment | Recruitment fork | journal |
| Vehicle modules | `VehicleCustomizationSystem` | existing | `vehicle_customization` | garage strip | none/low | journal |
| Trade routes | `PlayerTradeRouteSystem` | existing | `trade_routes` | new board | Economy fork | journal |
| Colony | `ColonySystem` | existing | `colony` | new board | Expedition fork | journal |
| Governance | DEC-chosen write owner | existing hosts | existing | Politics UI reconcile | Social | journal |
| Cooking | `CookingSystem` | `CookingHostSession` | `cooking` | kitchen strip | none/low | journal |
| Disaster | `DisasterResponseSystem` | existing | `disaster_response` | HUD/board | none/low | journal |
| Legacy/meta | existing systems | existing | existing | NG+ board | none/profile | journal |
| Ideology | `IdeologicalFrictionEvents` | existing | `ideological_friction` | resolve board | Social | journal |
| NPC memory | `NpcMemorySystem` | existing | `npc_memory` | barter/detail | none | UI banner |
| Difficulty | `DifficultySettingsSystem` | existing | `difficulty_settings` | runtime panel | none | apply scalars |
| Maintenance | `ShelterMaintenanceSystem` | existing | `shelter_maintenance` | board | none | journal |
| Inventory costs | Inventory authority | via bills | none | n/a | n/a | fail-closed |
| Roster intake | Survivors roster | recruitment success only | survivors | muster | Social | journal |
| External treaties | `RegionalTreatySystem` | leave alone | existing | factions | n/a | n/a |
| Daily routines | `SurvivorRoutineSystem` | `SurvivorRoutineHostSession` | `survivor_routines` | Routines board (+ Detail) | none/low | journal + census |
| Satisfaction ports | Needs / morale owners (existing) | adapter from routine tick | none new | board readout | n/a | fail-closed clamps |
| Sleep acoustic (W10) | `SleepAcousticLedger` + `SleepAcousticRestEngine` | `SleepAcousticRestHostSession` + `Main.SleepAcousticRest` | `sleep_acoustic_rest` | dormitory section (prefer `shelter_atmosphere` expand) | none/low | journal + census |
| Quiet hours (W10 read) | `ShelterNoiseSystem` (sole write) | atmosphere host | existing | atmosphere toggle | n/a | compliance flags into evaluate |

# 8. Data Flow

```
UI control
  → Main command / HostSession method
    → validate (ids, costs, cooldowns, fitness)
      → Core authority mutation (deterministic)
        → domain event / census delta
          → journal fact (+ optional day event)
            → UI refresh from Core read model
              → SaveX / CaptureSection on flush or dirty
```

Panels never compute success chances, costs, or eligibility offline from Core.

# 9. State Model Rules

1. Stateful loops use schema-gated Capture/Restore.
2. Old saves: missing section → defaults; never crash.
3. Derived presentation state is not saved.
4. Facades save nothing of their own.
5. Adding a `SaveSectionRegistry` row bumps pin 266→N and updates `ComprehensiveSaveStoreCorruptionAndMigrationTests`.
6. Filename uniqueness enforced with store `FileName` + registry map + gate tests.

# 10. API / Contracts (R4 — all signatures verified live at HEAD `1678c074`)

> **Binding rule:** verbs below were re-verified against live source at R4 (Appendix BV). `(host)` = named host session, `(core)` = Core owner, `(main)` = live Main wrapper. If live code has drifted by claim time, **live code wins** — reconcile the row in a revision note (Rule 7); never invent a bypass wrapper or a parallel verb.

### W1 Romance — `src/Host/RomanceFamilyHostSession.cs` (verified R3+R4)
| Verb (live) | Signature / role |
|---|---|
| `TryInitiateAttraction(...)` | begin attraction (eligibility-gated) |
| `ConductCourtshipEvent(...)` | courtship progress |
| `DissolvePartnership(survivorA, survivorB, reason = "mutual_drift")` | breakup |
| `FormFamilyUnit(survivorA, survivorB, familyName) → FamilyUnit` | family seal verb |
| `AddChildToFamily(familyId, childId, isAdopted) → bool` | **added R4** — child/adopt join |
| `CalculateCompatibility(ageA, ageB, beliefA, beliefB, sharedTrauma) → float` | read model |
| `GetRelationship` / `GetRomanticPartner` / `GetFamilyForSurvivor` | read models (providers) |
| `AdvanceDay(currentDay)` | day-owner tick |
| `Census` / `CapturePersistedState` / `CaptureCoreState` / `RestoreCoreState` | persistence |

> **R3 precision (retained):** live session does **not** expose `BeginCourtship` / `AdvanceRomance` / `BreakRelationship`. UI and Main wrappers must call the live verbs above.

### W2 Recruitment — `Assets/Ashfall.Core/Survivors/RecruitmentSystem.cs` (host_files=0 → CREATE `src/Host/RecruitmentHostSession.cs`)
| Verb (live, core) | Signature / role |
|---|---|
| `StartCampaign(campaignTypeId, recruiterId, targetFaction, day, recruiterSkill = 50)` | → `(bool Success, string Message, RecruitmentCampaignRecord? Campaign)`; caps at `MaxActiveCampaigns`; chance = `base_success_chance + recruiterSkill/4` clamped 10–95 |
| `DiscoverCandidate(candidateTemplateId, locationId, day, currentFaction = "")` | → `RecruitmentCandidateRecord` (`cand_{NextSequence}`) |
| `MakeDefectionOffer(candidateId, offerType, offerValue, day, diplomacySkill = 50)` | → `(bool, string, DefectionOfferRecord?)`; carries `SuccessChance` + `DiscoveredRisk` |
| `TickDay(day)` | resolves due campaigns (**no rng parameter today**) |
| Reads | `GetActiveCampaigns` / `GetKnownCandidates` / `GetDefectionOffers` / `GetCampaignDef` / `GetCandidateDef` |
| Counts | `ActiveCampaignCount` / `KnownCandidateCount` / `TotalRecruitedCount` |
| Persistence | `CaptureState` / `RestoreState` (`RecruitmentState`) |

> **R4 drift corrections (R3 names that do not exist):** `CancelCampaign`, `TickCampaigns(day, rng)`, `AcceptCandidate`. Two mechanic truths W2 must respect: (a) campaign resolution is currently a **flat threshold** (`SuccessChance >= 40` at due date), not a seeded roll — P1's `ISeededRng` roll is a deliberate mechanic-quality upgrade, not a bug fix; (b) there is **no accept verb** — campaign success sets `IsRecruited` inline. If the board needs first-class cancel/accept verbs (`CancelCampaign`, `ResolveOffer`/`AcceptCandidate`), add them in Core P1 with deterministic semantics + Capture/Restore coverage; roster intake stays with the survivors roster authority in P4; never simulate acceptance in UI.

### W3 Vehicle — `Assets/Ashfall.Core/Vehicles/VehicleCustomizationSystem.cs` (host `src/Host/VehicleCustomizationHostSession.cs`)
`InstallModule(vehicleId, moduleId, maxSlots = 4) → bool` · `RemoveModule(vehicleId, moduleId) → bool` · `GetInstalledModules(vehicleId) → IReadOnlyList<string>` · `IsMobileBaseCapable(vehicleId)` · `GetBunkCapacity(vehicleId) → int` · `DeployBaseCamp(vehicleId, locationId)` / `PackBaseCamp(vehicleId)` / `IsBaseCampDeployed(vehicleId)` · `RestSurvivorsInVehicle(vehicleId, survivorIds) → int` · `TryGetModule` + module catalog · `CaptureState`/`RestoreState` (JSON string).

> Garage remains condition/armor owner. **R4 meaningfulness:** the module strip must surface bunk capacity / mobile-base capability / base-camp deploy / rest outcomes, not only install-remove — those are the mechanics that make modules matter.

### W4 Trade/Colony — verified R4
**Trade** (`src/Host/TradeRouteHostSession.cs` over `PlayerTradeRouteSystem`): `EstablishContract(...)` (host) · `RegisterContract(contract)` · `GetContract(routeId)` · `CancelContract(routeId, currentDay)` · `SuspendContract(routeId)` / `ResumeContract(routeId)` · `RecordRunOutcome(routeId, outcome, currentDay, tariffPaid = 0)` · `TickDay(currentDay, funds = null)` (host) · `GetCensus` / `TotalTariffChitsPaid` · `CaptureState`/`RestoreState`.
> **R4 name fix:** “schedule/pause/resolve” → the exact verbs above; host type is **`TradeRouteHostSession`** (not `PlayerTradeRouteHostSession`).

**Colony** (`Assets/Ashfall.Core/Expeditions/ColonySystem.cs`, hosted through `src/Host/OutpostSettlementHostSession.cs` + `src/Host/OrphanSealWave1HostSessions.cs`): `EstablishColony(...)` · `ConstructBuilding(colonyId, definitionId, currentDay = 1)` · `EstablishSupplyLine(...)` · `SetSupplyLineStatus(lineId, status)` · `AssignSurvivorToColony(colonyId, survivorId)` · `TransferSupplies(colonyId, amount) → float` · `TickDay(currentDay)` · `GetColonies`/`GetColony` · `CaptureState`/`RestoreState`.
> **R4 name fix:** there is **no** `Found`/`Supply`/`Abandon` verb set. “Abandon” is `OutpostSettlementSystem` behavior (different owner) — the W4 board must not present an abandon command unless the outpost owner's verb is deliberately bound and disambiguated from the apiary “colony”.

### W5 Governance/Cook/Disaster — verified R4
**Governance** (post-**DEC-GOV** write-owner candidates on `ShelterGovernanceEngine`): `AssignSurvivorToBloc(survivorId, blocId)` · `GetSurvivorBloc` · `RecalculateBlocWeights()` · `EvaluatePolicyConsent(scope, optionId) → PolicyConsentEvaluation` · `ApplyPolicyEffects(scope, optionId)` · `AdjustBlocGrievance(blocId, deltaBp)` · `OpenDispute(...)` / `ResolveDispute(caseId, resolution, day)` · `CalculateStabilityRating()` · `Tick(gameHours)` · `GetCensus` · `CaptureState`/`RestoreState`. Which verbs become the signed write path is DEC-GOV's decision; `src/UI/PoliticsUI.cs` binds `PoliticsSystem` today (verified R4).

**Cooking** (`src/Host/CookingHostSession.cs`): `StartCooking(...) → ActionResult` · `ProgressCooking(deltaMinutes) → int` · `CancelCooking(operationId) → ActionResult` · `TickDay(currentDay, dayMinutes = 120f)` · `LoadAuthoredRecipes` / `BindInventory` · `CaptureState`/`RestoreState`.
> **R4 meaningfulness:** the kitchen surface must show `ProgressCooking` ticks and offer `CancelCooking`, not only Start.

**Disaster** (`DisasterResponseSystem`, hosted via `OrphanSealWave1HostSessions` / `WeatherCascadeHostSession`): `TriggerDisaster(...)` · `ActivateProtocol(EmergencyProtocolType)` · `DeactivateProtocol(...)` · `IsProtocolActive(...)` · `TickDisaster(...)` · `CalculateRoomDamage(disaster, roomId) → double` · `AdjustResilience(delta)` · `GetDisaster`/`GetAllDisasters` · `CaptureState`/`RestoreState`.
> **R4 name fix:** `ActivateDisasterProtocol` / `ResolveIncident` do not exist on this owner (`ResolveIncident` belongs to `AirlockSecuritySystem` — do not borrow it). HUD/board binds `ActivateProtocol`/`DeactivateProtocol`; incident closure is `TickDisaster` state transitions surfaced as facts; `CalculateRoomDamage`/`AdjustResilience` are the meaningful readouts.

### W6 Legacy/Ideology/NPC — verified R4
- **Legacy** (`src/Host/CampaignLegacyHostSession.cs`): `ArchiveCampaign(legacy, rng)` · `PrepareNewGameContext() → StartingCampaignContext` · `RegisterTrait` / `TryGetTrait` · `GetCensus` · `CaptureState`/`RestoreState`.
- **Meta** (`src/Host/MetaProgressionHostSession.cs`): `SetNgPlusBoonActive(id, active) → bool` · `IsUnlocked` / `IsBoonActive` · `GetGrantsForBoon` / `GetActiveNgPlusGrants` · `EvaluateProgress(...)` · `GetCensus` · `CaptureState`/`RestoreState`. **R4 add:** boon selection verb is `SetNgPlusBoonActive`; the NG+ board toggles boons through it and applies the context via `PrepareNewGameContext`.
- **Ideology** (`src/Host/IdeologicalFrictionHostSession.cs`): `CheckDailyFriction(...)` · `IsPairOnCooldown(a, b, day)` · `ResolveConfrontation(...)` · `AttemptConversion(...)` · `UpdateBunkerFactions(beliefs)` · `GetCensus` · `CaptureState`/`RestoreState`.
- **NPC memory** (`src/Host/NpcMemoryHostSession.cs`): `RecordAction(...)` · `Forgive(npcId, reason, restitutionAmount = 0f) → bool` · `IsTradeRefused(npcId) → bool` · `GetTradePriceMultiplier(npcId) → float` · `GetDialogueTone(npcId)` · `TickDailyDecay(day, decayRatePerDay = 1.0f)` · `GetCensus` · `CaptureState`/`RestoreState`.

### W7 Difficulty/Maintenance — verified R4
**Difficulty** — Core `DifficultySettingsSystem`: `SelectPreset(presetId)` · `SetCustomScalar(scalarName, value)` · `LockSettings()` · `GetEffectiveScalars` / `GetEffectiveProvider` · `GetCensus`. Live Main wrappers (`src/Main.DifficultySettings.cs`): `SelectDifficultyPreset` · `SetDifficultyCustomScalar` · `LockDifficultySettings` · `ApplyDifficultySettingsToScalars` · `GetDifficultySettingsCensus`.
> **R4 name fix:** `SetCustomLane` does not exist — the scalar setter is `SetCustomScalar` (core) / `SetDifficultyCustomScalar` (main).

**Maintenance** (`src/Host/ShelterMaintenanceHostSession.cs`, Main `src/Main.ShelterMaintenance.cs`): `PerformMaintenance(componentId, actionType, skillLevel, day) → bool` · `TickDay(currentDay, weatherStressMult = 1.0f, radiationStressMult = 1.0f)` · `GetFailedComponents()` / `GetWarningComponents()` · `GetComponent` / `GetAverageIntegrity` · `GetCensus` · `CaptureState`/`RestoreState`.
> **R4 name fix:** `ScheduleRepair` / `ClearAlert` do not exist. The repair verb is `PerformMaintenance`; “alerts” are the `GetWarningComponents`/`GetFailedComponents` projections — the board shows them and acts through maintenance, never through a parallel alert store.

### W8 Chronic — API card (DEC-CHRONIC gated; verified live for planning only)
`AddCondition(survivorId, conditionId, day, cause = "injury")` · `GetSurvivorConditions(survivorId)` · `AssignAccommodation(survivorId, accommodationId, conditionId, day)` · `RemoveAccommodation(survivorId, accommodationId)` · `GetSurvivorAccommodations(survivorId)` · `CalculateCapabilityModifier(survivorId, capabilityName) → float` · `GetTotalImpairmentScore(survivorId) → float` · `LoadCatalog` · `CaptureState`/`RestoreState`. No host wire until DEC-CHRONIC signs; never a second medical ledger.

### W9 Survivor daily routines — `src/Host/SurvivorRoutineHostSession.cs` (verified R3 + R4)
`AssignRoutine(survivorId, templateId) → SurvivorRoutineRecord` · `SetPreference(survivorId, chronotype, workShift = "morning", social = "balanced")` · `SetEnforcementLevel(level)` (`strict`|`flexible`|`none`) · `EvaluateDailySatisfaction(survivorId, day, hoursWorked, hoursSlept, mealsHad, socialHours) → RoutineSatisfactionRecord` · `DetectConflicts(day, roomAssignments, workspaceAssignments) → IReadOnlyList<RoutineConflictRecord>` · `ResolveConflict(conflictId) → bool` · `GetActivityAtHour(survivorId, hour) → string` · `GetActiveConflicts()` · `GetRoutine`/`GetPreference`/`GetTemplate`/`GetAllTemplates` · `Census`/`TrackedRoutineCount`/`EnforcementLevel` · `CaptureState`/`RestoreState`/`Reset` · satisfaction→Needs/morale port adapter (new, Appendix BK)

### W10 Sleep Acoustic — `src/Host/SleepAcousticRestHostSession.cs` / `src/Main.SleepAcousticRest.cs` (verified R5)
**Core state owner** `SleepAcousticLedger` (`Assets/Ashfall.Core/Needs/SleepAcousticLedger.cs`): `RegisterOrUpdateQuarter(quarter)` · `GetQuarter(roomId)` · `EvaluateQuarterSleep(roomId, isQuietHours, compliance, hoursSlept) → SleepQualityResult` · `InstallSensoryReliefKit(roomId) → bool` · `RestockSensoryReliefKits(count)` · `UpdateRoomSoundproofing(roomId, wallPermille, doorPermille)` · `SetQuietHoursSchedule(startHour, endHour, active)` · `AdvanceDay(hoursSlept = 8)` · `GetCensus()` · `CaptureState`/`RestoreState`.
**Core math owner** `SleepAcousticRestEngine` (static): `CalculateAcousticAttenuation(...)` · `CalculateCrowdingDensity(occupants, roomAreaSqMetres)` · `EvaluateSleepQuality(quarter, isQuietHours, compliance, hoursSlept)` · `ComputeNetFatigueRecovery(baseFatigueRecovery, fatigueMultiplierPermille)`.
**Main wrappers (live, exact):** `EvaluateSleepingQuarterSleep` · `InstallSensoryReliefKit` · `RestockSensoryReliefKits` · `UpdateSleepingQuarterSoundproofing` · `SetQuietHoursSchedule` · `AdvanceSleepAcousticRestDay(hoursSlept = 8)` · `GetSleepAcousticCensus`. Host session mirrors the ledger verbs one-for-one (verified). Day owner `sleep_acoustic_rest` (phase 5, `CampaignOwners:176`); save row `sleep_acoustic_rest` → `sleep_acoustic_rest_save.json` (already inside pin 266); CLI flag `--sleep-acoustic-selftest` (alias `--the-quiet-selftest`).
> **R5 name fix:** the W10 package text named `--sleep-acoustic-rest-selftest`; the live flag is **`--sleep-acoustic-selftest`** — corrected in all acceptance commands.
> **R5 mechanic honesty (census assumption):** `GetCensus` re-evaluates bands with **hardcoded `Compliant` + 8h**; `AdvanceDay` still discards `EvaluateQuarterSleep` results (C12). The board must either pass live compliance into the readout or label the assumption — an optimistic dormitory score is a silent-lie risk.
> **R5 morale polarity (verified in engine):** `MoraleRestorationBonus` is good-positive (`DeepSanctuary +4` … `Unbearable −6`); under the higher=worse Morale convention the port calls `Modify(Morale, -bonus)`, and `ComputeNetFatigueRecovery` feeds `Modify(Fatigue, -recovery)`.


# 11–25. Wave packages (full integration contracts)
Each wave below is an embeddable single-package plan: objective, reality, delta, seams, architecture, ownership, data flow, state, APIs, data, save, determinism, wiring, Godot, narrative, failures, tests, phases, files, risks, out-of-scope, rollback, DoD, handoff.

## PFGL-W1-ROMANCE-BOARD — Wave 1 — Romance & Family Operable Board

**Priority:** P1
**Core owner:** `RomanceFamilySystem` (`Assets/Ashfall.Core/Survivors/RomanceFamilySystem.cs`, ~747 LOC)
**Host:** `src/Host/RomanceFamilyHostSession.cs`
**Main:** `src/Main.RomanceFamily.cs`
**Save:** `romance_family`

### Why now
Players see partner/stage/soulmate via RomanceProvider, but courtship/breakup/FormFamilyUnit are CLI-capable without a normal operable board. Highest-ROI social EXPAND: Core rich, save exists, host exists, agency missing.

### Player value
Ongoing survivor drama, morale stakes, generational hooks without new Core invention.

### Objective
Seal this loop to PLAYER-OPERABLE using the named owner only.

### Current reality
See §2 reachability table and evidence index for this owner.

### Required delta
Close the broken completion-chain link(s) in §3 for this loop.

### Existing extension seams
Reuse HostSessionBase, Main triad, CampaignOwners, SaveSectionRegistry-or-reuse, HostCliRegistry, panel battery if interactive, CampaignStreamIds.

### Proposed architecture
Extend the live owner; prefer bind-existing UI; new routes only with claimed hubs; facades save nothing.

### Ownership matrix
| Concern | Owner |
|---|---|
| Domain | named Core owner |
| Host/Main | named paths |
| Save | named section strategy |
| Inventory/needs/relations | existing authorities only |

### Data flow / state / determinism
Follow §§8–9 and program determinism: forked `ISeededRng`, schema-gated restore, no presentation saves.

### API/contracts
See §10 for this wave.

### Data changes
Prefer existing catalogs. Add JSON only when P0 proves missing rows. snake_case. Integrity gate on catalog edits.

### Save/load / event wiring / Godot / narrative
Orchestrator Setup/Save; Lifecycle Reset; day owner phase by domain; thin panels; restrained fictional journal prose; no real-world war names.

### Failure modes
- UI mutates local stage without Core
- Bypassed affinity gates
- Duplicate family units
- Wall-clock RNG
- Universal: null catalog, missing ids, dead survivors, duplicate ids, save mid-transition, stale session after slot switch, unbound HOST_REQUIRED.

### Test strategy
Core unit + persistence + host command mutation + UI route gates if new route + CLI 12-check + no full suite default.

### Dependency-ordered phases
#### P0 Premise
Re-rg RomanceFamilySystem; list RomanceFamilyHostSession commands; confirm save section; confirm providers; claim files.

**Gate:** premise evidence recorded; no unrelated edits.

#### P1 Core check
Verify FormFamilyUnit/courtship/Capture/Restore; add Census only if board needs it.

**Gate:** premise evidence recorded; no unrelated edits.

#### P2 Main commands
TryInitiateAttraction/ConductCourtshipEvent/DissolvePartnership/FormFamilyUnit + journal romance_* keys.

**Gate:** premise evidence recorded; no unrelated edits.

#### P3 UI
Preferred: SurvivorDetail action row. Alternate: route romance_family with full panel battery.

**Gate:** premise evidence recorded; no unrelated edits.

#### P4 Observability
Stage-change journal; heartbeats stay informational.

**Gate:** premise evidence recorded; no unrelated edits.

#### P5 Verify
Plan150 tests; --romance-family-selftest; panel gates if new route; manual form+reload.

**Gate:** premise evidence recorded; no unrelated edits.

### File impact map
| Action | File | Reason | Risk |
|---|---|---|---|
| MODIFY | `src/UI/SurvivorDetailPanel.cs` | Actionable romance controls | MED |
| MODIFY | `src/Main.RomanceFamily.cs` | UI callbacks/journal | LOW |
| MODIFY | `src/Main.UiPanels.cs` | Bind action providers | LOW |
| CREATE_OPTIONAL | `src/UI/RomanceFamilyPanel.cs` | Only if detail cannot host actions | MED |
| MODIFY_OPTIONAL | `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs` | New route only if needed | HIGH |
| READ | `src/Host/RomanceFamilyHostSession.cs` | Reuse APIs | LOW |

### Risks
Shared hub contention; one-authority collisions; CLI-complete illusion; naming collisions.

### Out of scope
- No parallel romance ledger
- Do not replace GenerationalSystem nursery ownership
- No System.Random courtship rolls
- No new route if detail actions suffice
- Unrelated debloat; Unity; full-suite campaigns.

### Rollback
Small reversible commits per phase; UI-first revert; keep save sections readable; never reset unrelated dirty files.

### Definition of Done
- bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/Plan150RomanceFamilyIntegrationTests.cs
- godot --headless --path . -- --romance-family-selftest
- Player can form a family from UI; save/load preserves pair; journal shows fact

### Implementation handoff
**MUST PRESERVE:** one authority; Core engine-free; sealed loops; unrelated dirty files.
**MUST ADD:** player-operable path for `RomanceFamilySystem`.
**MUST NOT DO:** parallel ledgers; System.Random; unclaimed hub edits.
**VERIFY WITH:** acceptance commands above.
**FIRST SAFE STEP:** P0 premise + WORKTREE claim `PFGL-W1-ROMANCE-BOARD`.

### Integrator execution checklist
- [ ] 1. Claim exact paths in WORKTREE_OWNERSHIP.md
- [ ] 2. Record Rule-7 rg evidence in claim note
- [ ] 3. List colliding sibling authorities
- [ ] 4. Confirm save strategy (reuse vs new+pin)
- [ ] 5. Confirm UI strategy (bind vs new route)
- [ ] 6. Implement Core gaps only if proven
- [ ] 7. Implement host/Main commands
- [ ] 8. Wire day owner if stateful tick required
- [ ] 9. Register CLI selftest
- [ ] 10. Add/adjust UI
- [ ] 11. Run focused Core tests
- [ ] 12. Run selftest
- [ ] 13. Run save/panel gates if touched
- [ ] 14. Manual player path success+failure
- [ ] 15. Write handoff; mark claim COMPLETE

### Manual player script
1. Launch host (15 FPS if runtime session). 2. Open delivered surface. 3. Success command + failure command. 4. Confirm feedback. 5. Save/load. 6. Check sibling UI non-goals. 7. Attach selftest transcript.

## PFGL-W2-RECRUITMENT-WIRE — Wave 2 — Recruitment & Defection Full Host Wire

**Priority:** P1
**Core owner:** `RecruitmentSystem` (`Assets/Ashfall.Core/Survivors/RecruitmentSystem.cs`, ~366 LOC)
**Host:** `CREATE src/Host/RecruitmentHostSession.cs`
**Main:** `CREATE src/Main.Recruitment.cs`
**Save:** `recruitment (new→pin bump)`

### Why now
TRUE ORPHAN (host_files=0) with campaign/candidate templates and Plan204 tests. PrisonerPanel RECRUIT is a different owner. Population growth campaigns are unplayable.

### Player value
Active roster growth/specialization through costly multi-day campaigns.

### Objective
Seal this loop to PLAYER-OPERABLE using the named owner only.

### Current reality
See §2 reachability table and evidence index for this owner.

### Required delta
Close the broken completion-chain link(s) in §3 for this loop.

### Existing extension seams
Reuse HostSessionBase, Main triad, CampaignOwners, SaveSectionRegistry-or-reuse, HostCliRegistry, panel battery if interactive, CampaignStreamIds.

### Proposed architecture
Extend the live owner; prefer bind-existing UI; new routes only with claimed hubs; facades save nothing.

### Ownership matrix
| Concern | Owner |
|---|---|
| Domain | named Core owner |
| Host/Main | named paths |
| Save | named section strategy |
| Inventory/needs/relations | existing authorities only |

### Data flow / state / determinism
Follow §§8–9 and program determinism: forked `ISeededRng`, schema-gated restore, no presentation saves.

### API/contracts
See §10 for this wave.

### Data changes
Prefer existing catalogs. Add JSON only when P0 proves missing rows. snake_case. Integrity gate on catalog edits.

### Save/load / event wiring / Godot / narrative
Orchestrator Setup/Save; Lifecycle Reset; day owner phase by domain; thin panels; restrained fictional journal prose; no real-world war names.

### Failure modes
- System.Random success
- Duplicate roster ids
- Non-atomic costs
- Prisoner recruit silently wrong system
- Universal: null catalog, missing ids, dead survivors, duplicate ids, save mid-transition, stale session after slot switch, unbound HOST_REQUIRED.

### Test strategy
Core unit + persistence + host command mutation + UI route gates if new route + CLI 12-check + no full suite default.

### Dependency-ordered phases
#### P0 Premise
Confirm 0 src refs; cost item ids resolve; prisoner/visitor boundary written; claim save/cli/lifecycle hubs.

**Gate:** premise evidence recorded; no unrelated edits.

#### P1 Core harden
Capture/Restore, census, schema gate, ISeededRng success rolls.

**Gate:** premise evidence recorded; no unrelated edits.

#### P2 Data
Strict catalog loader; integrity registration if required.

**Gate:** premise evidence recorded; no unrelated edits.

#### P3 Host+store
RecruitmentHostSession + SaveStoreHub.Checksummed.

**Gate:** premise evidence recorded; no unrelated edits.

#### P4 Main+day
Setup/Save/Reset; day owner ticks campaigns; journal; roster intake port on success.

**Gate:** premise evidence recorded; no unrelated edits.

#### P5 Registries
SaveSectionRegistry+pin; HostCliRegistry; Lifecycle ResetLateWave; SaveOrchestrator.

**Gate:** premise evidence recorded; no unrelated edits.

#### P6 UI
Muster section or recruitment route: start campaign (`StartCampaign`) / make defection offer (`MakeDefectionOffer`) / cancel-or-accept only via Core verbs added in P1 (no UI-side simulation).

**Gate:** premise evidence recorded; no unrelated edits.

#### P7 Verify
--recruitment-selftest 12/12; Plan204; save pin; panel gates.

**Gate:** premise evidence recorded; no unrelated edits.

### File impact map
| Action | File | Reason | Risk |
|---|---|---|---|
| CREATE | `src/Host/RecruitmentHostSession.cs` | Host adapter | MED |
| CREATE | `src/Main.Recruitment.cs` | Setup/Save/commands | MED |
| CREATE | `src/Host/RecruitmentSelfTest.cs` | 12-check probe | LOW |
| MODIFY | `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` | Section+filename; pin tests | HIGH |
| MODIFY | `Assets/Ashfall.Core/HostCliRegistry.cs` | Flag+action | MED |
| MODIFY | `src/Main.CampaignOwners.cs` | Day owner | MED |
| MODIFY | `src/Main.Lifecycle.cs` | ResetLateWave | MED |
| MODIFY | `src/Main.SaveOrchestrator.cs` | Setup/Save | MED |
| MODIFY | `src/Main.Application.cs` | CLI dispatch | LOW |
| CREATE_OPTIONAL | `src/UI/RecruitmentPanel.cs` | Player board | MED |
| MODIFY | `Assets/Ashfall.Core/Survivors/RecruitmentSystem.cs` | Only if Capture/Census gaps | MED |

### Risks
Shared hub contention; one-authority collisions; CLI-complete illusion; naming collisions.

### Out of scope
- Do not merge into PrisonerSystem
- No ad-hoc survivor list add outside roster intake
- Atomic inventory bills for food/water/currency
- Unrelated debloat; Unity; full-suite campaigns.

### Rollback
Small reversible commits per phase; UI-first revert; keep save sections readable; never reset unrelated dirty files.

### Definition of Done
- rg -l '\bRecruitmentSystem\b' src --glob '*.cs' | wc -l   # expect >= 3
- bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/Plan204RecruitmentIntegrationTests.cs
- godot --headless --path . -- --recruitment-selftest
- bash scripts/run_test.sh Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs

### Implementation handoff
**MUST PRESERVE:** one authority; Core engine-free; sealed loops; unrelated dirty files.
**MUST ADD:** player-operable path for `RecruitmentSystem`.
**MUST NOT DO:** parallel ledgers; System.Random; unclaimed hub edits.
**VERIFY WITH:** acceptance commands above.
**FIRST SAFE STEP:** P0 premise + WORKTREE claim `PFGL-W2-RECRUITMENT-WIRE`.

### Integrator execution checklist
- [ ] 1. Claim exact paths in WORKTREE_OWNERSHIP.md
- [ ] 2. Record Rule-7 rg evidence in claim note
- [ ] 3. List colliding sibling authorities
- [ ] 4. Confirm save strategy (reuse vs new+pin)
- [ ] 5. Confirm UI strategy (bind vs new route)
- [ ] 6. Implement Core gaps only if proven
- [ ] 7. Implement host/Main commands
- [ ] 8. Wire day owner if stateful tick required
- [ ] 9. Register CLI selftest
- [ ] 10. Add/adjust UI
- [ ] 11. Run focused Core tests
- [ ] 12. Run selftest
- [ ] 13. Run save/panel gates if touched
- [ ] 14. Manual player path success+failure
- [ ] 15. Write handoff; mark claim COMPLETE

### Manual player script
1. Launch host (15 FPS if runtime session). 2. Open delivered surface. 3. Success command + failure command. 4. Confirm feedback. 5. Save/load. 6. Check sibling UI non-goals. 7. Attach selftest transcript.

## PFGL-W3-VEHICLE-MODULES — Wave 3 — Vehicle Customization on Garage Seam

**Priority:** P1
**Core owner:** `VehicleCustomizationSystem` (`Assets/Ashfall.Core/Vehicles/VehicleCustomizationSystem.cs`, ~516 LOC)
**Host:** `src/Host/VehicleCustomizationHostSession.cs`
**Main:** `src/Main.VehicleCustomization.cs`
**Save:** `vehicle_customization`

### Why now
InstallModule exists and is CLI-tested; vehicle_garage UI is Plan 50 wear/armor. Players cannot install bunk/cargo/winch modules from UI.

### Player value
Configurable expedition tools instead of static garage objects.

### Objective
Seal this loop to PLAYER-OPERABLE using the named owner only.

### Current reality
See §2 reachability table and evidence index for this owner.

### Required delta
Close the broken completion-chain link(s) in §3 for this loop.

### Existing extension seams
Reuse HostSessionBase, Main triad, CampaignOwners, SaveSectionRegistry-or-reuse, HostCliRegistry, panel battery if interactive, CampaignStreamIds.

### Proposed architecture
Extend the live owner; prefer bind-existing UI; new routes only with claimed hubs; facades save nothing.

### Ownership matrix
| Concern | Owner |
|---|---|
| Domain | named Core owner |
| Host/Main | named paths |
| Save | named section strategy |
| Inventory/needs/relations | existing authorities only |

### Data flow / state / determinism
Follow §§8–9 and program determinism: forked `ISeededRng`, schema-gated restore, no presentation saves.

### API/contracts
See §10 for this wave.

### Data changes
Prefer existing catalogs. Add JSON only when P0 proves missing rows. snake_case. Integrity gate on catalog edits.

### Save/load / event wiring / Godot / narrative
Orchestrator Setup/Save; Lifecycle Reset; day owner phase by domain; thin panels; restrained fictional journal prose; no real-world war names.

### Failure modes
- Offline invented module list
- maxSlots ignored
- Costs not charged
- Universal: null catalog, missing ids, dead survivors, duplicate ids, save mid-transition, stale session after slot switch, unbound HOST_REQUIRED.

### Test strategy
Core unit + persistence + host command mutation + UI route gates if new route + CLI 12-check + no full suite default.

### Dependency-ordered phases
#### P0 Premise
Map garage panel bind; list customization commands; claim vehicle UI files.

**Gate:** premise evidence recorded; no unrelated edits.

#### P1 Seam map
Garage owns condition/armor; Customization owns modules/slots; shared vehicle ids.

**Gate:** premise evidence recorded; no unrelated edits.

#### P2 Host/Main
Inventory bills for ScrapCost/ComponentsCost; journal module_installed.

**Gate:** premise evidence recorded; no unrelated edits.

#### P3 UI
Garage Modules strip: install/remove, slots, modifiers read model.

**Gate:** premise evidence recorded; no unrelated edits.

#### P4 Verify
Customization selftest; garage uitest slice; save round-trip; armor path unchanged.

**Gate:** premise evidence recorded; no unrelated edits.

### File impact map
| Action | File | Reason | Risk |
|---|---|---|---|
| MODIFY | `src/UI/*Garage*Panel*.cs` | Modules strip (confirm path in P0) | MED |
| MODIFY | `src/Main.VehicleCustomization.cs` | UI bridges | LOW |
| READ | `src/Host/VehicleCustomizationHostSession.cs` | InstallModule | LOW |

### Risks
Shared hub contention; one-authority collisions; CLI-complete illusion; naming collisions.

### Out of scope
- Do not fork VehicleGarageSystem
- No second vehicle wear save
- No aviation/naval graph travel in this wave
- Unrelated debloat; Unity; full-suite campaigns.

### Rollback
Small reversible commits per phase; UI-first revert; keep save sections readable; never reset unrelated dirty files.

### Definition of Done
- godot --headless --path . -- --vehicle-customization-selftest
- UI install increases modules; save/load keeps them; garage armor path unchanged

### Implementation handoff
**MUST PRESERVE:** one authority; Core engine-free; sealed loops; unrelated dirty files.
**MUST ADD:** player-operable path for `VehicleCustomizationSystem`.
**MUST NOT DO:** parallel ledgers; System.Random; unclaimed hub edits.
**VERIFY WITH:** acceptance commands above.
**FIRST SAFE STEP:** P0 premise + WORKTREE claim `PFGL-W3-VEHICLE-MODULES`.

### Integrator execution checklist
- [ ] 1. Claim exact paths in WORKTREE_OWNERSHIP.md
- [ ] 2. Record Rule-7 rg evidence in claim note
- [ ] 3. List colliding sibling authorities
- [ ] 4. Confirm save strategy (reuse vs new+pin)
- [ ] 5. Confirm UI strategy (bind vs new route)
- [ ] 6. Implement Core gaps only if proven
- [ ] 7. Implement host/Main commands
- [ ] 8. Wire day owner if stateful tick required
- [ ] 9. Register CLI selftest
- [ ] 10. Add/adjust UI
- [ ] 11. Run focused Core tests
- [ ] 12. Run selftest
- [ ] 13. Run save/panel gates if touched
- [ ] 14. Manual player path success+failure
- [ ] 15. Write handoff; mark claim COMPLETE

### Manual player script
1. Launch host (15 FPS if runtime session). 2. Open delivered surface. 3. Success command + failure command. 4. Confirm feedback. 5. Save/load. 6. Check sibling UI non-goals. 7. Attach selftest transcript.

## PFGL-W4-TRADE-COLONY — Wave 4 — Trade Routes Board + Colony Founding Board

**Priority:** P1
**Core owner:** `PlayerTradeRouteSystem + ColonySystem` (`Assets/Ashfall.Core/Economy/PlayerTradeRouteSystem.cs ; Assets/Ashfall.Core/Expeditions/ColonySystem.cs`, ~670 LOC)
**Host:** `existing TradeRoute + Colony hosts`
**Main:** `existing Main partials`
**Save:** `trade_routes ; colony`

### Why now
Both host-wired with saves but lack interactive boards. Greenhouse colony means bees. Outposts partially on Shelter Operations; player colonies remain distinct.

### Player value
Long-horizon economy and territorial expansion beyond one-off barter.

### Objective
Seal this loop to PLAYER-OPERABLE using the named owner only.

### Current reality
See §2 reachability table and evidence index for this owner.

### Required delta
Close the broken completion-chain link(s) in §3 for this loop.

### Existing extension seams
Reuse HostSessionBase, Main triad, CampaignOwners, SaveSectionRegistry-or-reuse, HostCliRegistry, panel battery if interactive, CampaignStreamIds.

### Proposed architecture
Extend the live owner; prefer bind-existing UI; new routes only with claimed hubs; facades save nothing.

### Ownership matrix
| Concern | Owner |
|---|---|
| Domain | named Core owner |
| Host/Main | named paths |
| Save | named section strategy |
| Inventory/needs/relations | existing authorities only |

### Data flow / state / determinism
Follow §§8–9 and program determinism: forked `ISeededRng`, schema-gated restore, no presentation saves.

### API/contracts
See §10 for this wave.

### Data changes
Prefer existing catalogs. Add JSON only when P0 proves missing rows. snake_case. Integrity gate on catalog edits.

### Save/load / event wiring / Godot / narrative
Orchestrator Setup/Save; Lifecycle Reset; day owner phase by domain; thin panels; restrained fictional journal prose; no real-world war names.

### Failure modes
- Colony board mutates apiary
- Trade bypasses embargo
- Unregistered briefing link
- Universal: null catalog, missing ids, dead survivors, duplicate ids, save mid-transition, stale session after slot switch, unbound HOST_REQUIRED.

### Test strategy
Core unit + persistence + host command mutation + UI route gates if new route + CLI 12-check + no full suite default.

### Dependency-ordered phases
#### P0 Premise
Locate host sessions/commands; claim UI hubs.

**Gate:** premise evidence recorded; no unrelated edits.

#### P1 Trade UI
Route trade_routes or Economy subpanel: schedule/pause/tariff/status.

**Gate:** premise evidence recorded; no unrelated edits.

#### P2 Colony UI
Settlement Colony board: found/supply/abandon; map node ids; avoid apiary wording.

**Gate:** premise evidence recorded; no unrelated edits.

#### P3 Observability
Journal; briefing deep-links only to registered routes.

**Gate:** premise evidence recorded; no unrelated edits.

#### P4 Verify
Existing selftests; PanelRouteGateTests; PlayerSurfaceCoverageGateTests.

**Gate:** premise evidence recorded; no unrelated edits.

### File impact map
| Action | File | Reason | Risk |
|---|---|---|---|
| CREATE | `src/UI/TradeRoutesPanel.cs` | Player board | MED |
| CREATE | `src/UI/ColonyBoardPanel.cs` | Player board | MED |
| MODIFY | `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs` | Routes | HIGH |
| MODIFY | `src/Main.GameFlow.cs` | Open cases | HIGH |
| MODIFY | `src/Main.PlayerSurfaces.cs` | Surfaces | HIGH |
| MODIFY | `Assets/Ashfall.Core/UI/PlayerSurfaceManifest.cs` | Manifest parity | MED |

### Risks
Shared hub contention; one-authority collisions; CLI-complete illusion; naming collisions.

### Out of scope
- Do not duplicate OutpostSettlementSystem
- No second embargo ledger
- Do not rename apiary APIs
- Unrelated debloat; Unity; full-suite campaigns.

### Rollback
Small reversible commits per phase; UI-first revert; keep save sections readable; never reset unrelated dirty files.

### Definition of Done
- Existing trade-routes and colony selftests still PASS
- PanelRouteGateTests + PlayerSurfaceCoverageGateTests PASS
- Player can schedule a route and found a colony from UI

### Implementation handoff
**MUST PRESERVE:** one authority; Core engine-free; sealed loops; unrelated dirty files.
**MUST ADD:** player-operable path for `PlayerTradeRouteSystem + ColonySystem`.
**MUST NOT DO:** parallel ledgers; System.Random; unclaimed hub edits.
**VERIFY WITH:** acceptance commands above.
**FIRST SAFE STEP:** P0 premise + WORKTREE claim `PFGL-W4-TRADE-COLONY`.

### Integrator execution checklist
- [ ] 1. Claim exact paths in WORKTREE_OWNERSHIP.md
- [ ] 2. Record Rule-7 rg evidence in claim note
- [ ] 3. List colliding sibling authorities
- [ ] 4. Confirm save strategy (reuse vs new+pin)
- [ ] 5. Confirm UI strategy (bind vs new route)
- [ ] 6. Implement Core gaps only if proven
- [ ] 7. Implement host/Main commands
- [ ] 8. Wire day owner if stateful tick required
- [ ] 9. Register CLI selftest
- [ ] 10. Add/adjust UI
- [ ] 11. Run focused Core tests
- [ ] 12. Run selftest
- [ ] 13. Run save/panel gates if touched
- [ ] 14. Manual player path success+failure
- [ ] 15. Write handoff; mark claim COMPLETE

### Manual player script
1. Launch host (15 FPS if runtime session). 2. Open delivered surface. 3. Success command + failure command. 4. Confirm feedback. 5. Save/load. 6. Check sibling UI non-goals. 7. Attach selftest transcript.

## PFGL-W5-GOVERNANCE-COOK-DISASTER — Wave 5 — Governance Reconcile + Kitchen Cooking + Disaster Protocols

**Priority:** P1
**Core owner:** `ShelterGovernanceEngine/PoliticsSystem ; CookingSystem ; DisasterResponseSystem` (`Governance + Cooking + Disaster Core paths`, ~1638 LOC)
**Host:** `existing`
**Main:** `existing`
**Save:** `shelter_governance ; cooking ; disaster_response`

### Why now
Three mismatches: Politics UI vs governance engine; Kitchen nutrition vs StartCooking; DisasterResponse host-rich vs acknowledge-thin HUD.

### Player value
Political agency, trap→cook food pipeline, active crisis response.

### Objective
Seal this loop to PLAYER-OPERABLE using the named owner only.

### Current reality
See §2 reachability table and evidence index for this owner.

### Required delta
Close the broken completion-chain link(s) in §3 for this loop.

### Existing extension seams
Reuse HostSessionBase, Main triad, CampaignOwners, SaveSectionRegistry-or-reuse, HostCliRegistry, panel battery if interactive, CampaignStreamIds.

### Proposed architecture
Extend the live owner; prefer bind-existing UI; new routes only with claimed hubs; facades save nothing.

### Ownership matrix
| Concern | Owner |
|---|---|
| Domain | named Core owner |
| Host/Main | named paths |
| Save | named section strategy |
| Inventory/needs/relations | existing authorities only |

### Data flow / state / determinism
Follow §§8–9 and program determinism: forked `ISeededRng`, schema-gated restore, no presentation saves.

### API/contracts
See §10 for this wave.

### Data changes
Prefer existing catalogs. Add JSON only when P0 proves missing rows. snake_case. Integrity gate on catalog edits.

### Save/load / event wiring / Godot / narrative
Orchestrator Setup/Save; Lifecycle Reset; day owner phase by domain; thin panels; restrained fictional journal prose; no real-world war names.

### Failure modes
- Dual election writes
- Cooking strip serves meals without CookingSystem
- HUD ack without Core mutation
- Universal: null catalog, missing ids, dead survivors, duplicate ids, save mid-transition, stale session after slot switch, unbound HOST_REQUIRED.

### Test strategy
Core unit + persistence + host command mutation + UI route gates if new route + CLI 12-check + no full suite default.

### Dependency-ordered phases
#### P0 DEC-GOV
Sign one write owner before politics code. Stop if unsigned.

**Gate:** premise evidence recorded; no unrelated edits.

#### P1 Governance UI
Implement signed bind; blocs/consent visible.

**Gate:** premise evidence recorded; no unrelated edits.

#### P2 Kitchen bind
Cooking strip or sibling wildlife_cooking route; keep nutrition serve path.

**Gate:** premise evidence recorded; no unrelated edits.

#### P3 Disaster
Bind HUD/ops section to mitigation activate/resolve.

**Gate:** premise evidence recorded; no unrelated edits.

#### P4 Verify
Focused tests + selftests + panel gates.

**Gate:** premise evidence recorded; no unrelated edits.

### File impact map
| Action | File | Reason | Risk |
|---|---|---|---|
| MODIFY | `src/UI/PoliticsUI.cs` | Per DEC bind | HIGH |
| MODIFY | `src/UI/KitchenNutritionPanel.cs` | Cooking strip | MED |
| MODIFY | `src/UI/EmergencyResponseHud.cs` | Bind protocols (verify path P0) | MED |
| MODIFY | `src/Main.Cooking.cs` | UI bridges | LOW |
| DOC | `docs/governance/DECISION_REGISTER.md` | DEC-GOV | LOW |

### Risks
Shared hub contention; one-authority collisions; CLI-complete illusion; naming collisions.

### Out of scope
- No EmergencyAlertSystem without DEC
- Do not replace KitchenNutrition meal serving
- No third politics system
- Unrelated debloat; Unity; full-suite campaigns.

### Rollback
Small reversible commits per phase; UI-first revert; keep save sections readable; never reset unrelated dirty files.

### Definition of Done
- DEC-GOV recorded before politics code lands
- Kitchen can StartCooking a real recipe path
- Disaster protocol activation mutates DisasterResponse and survives save

### Implementation handoff
**MUST PRESERVE:** one authority; Core engine-free; sealed loops; unrelated dirty files.
**MUST ADD:** player-operable path for `ShelterGovernanceEngine/PoliticsSystem ; CookingSystem ; DisasterResponseSystem`.
**MUST NOT DO:** parallel ledgers; System.Random; unclaimed hub edits.
**VERIFY WITH:** acceptance commands above.
**FIRST SAFE STEP:** P0 premise + WORKTREE claim `PFGL-W5-GOVERNANCE-COOK-DISASTER`.

### Integrator execution checklist
- [ ] 1. Claim exact paths in WORKTREE_OWNERSHIP.md
- [ ] 2. Record Rule-7 rg evidence in claim note
- [ ] 3. List colliding sibling authorities
- [ ] 4. Confirm save strategy (reuse vs new+pin)
- [ ] 5. Confirm UI strategy (bind vs new route)
- [ ] 6. Implement Core gaps only if proven
- [ ] 7. Implement host/Main commands
- [ ] 8. Wire day owner if stateful tick required
- [ ] 9. Register CLI selftest
- [ ] 10. Add/adjust UI
- [ ] 11. Run focused Core tests
- [ ] 12. Run selftest
- [ ] 13. Run save/panel gates if touched
- [ ] 14. Manual player path success+failure
- [ ] 15. Write handoff; mark claim COMPLETE

### Manual player script
1. Launch host (15 FPS if runtime session). 2. Open delivered surface. 3. Success command + failure command. 4. Confirm feedback. 5. Save/load. 6. Check sibling UI non-goals. 7. Attach selftest transcript.

## PFGL-W6-LEGACY-IDEOLOGY-NPC — Wave 6 — NG+ Legacy + Ideology Resolve + NPC Memory Acts

**Priority:** P2
**Core owner:** `CampaignLegacySystem ; MetaProgressionSystem ; IdeologicalFrictionEvents ; NpcMemorySystem` (`Legacy/Meta/Ideology/NpcMemory Core`, ~1925 LOC)
**Host:** `existing`
**Main:** `existing`
**Save:** `campaign_legacy ; meta_progression ; ideological_friction ; npc_memory`

### Why now
Cross-run and social-consequence systems are host/CLI sealed but lack operable boards. ResolveConfrontation/AttemptConversion and Forgive/IsTradeRefused exist.

### Player value
Replayability and living social consequences in barter and shelter politics.

### Objective
Seal this loop to PLAYER-OPERABLE using the named owner only.

### Current reality
See §2 reachability table and evidence index for this owner.

### Required delta
Close the broken completion-chain link(s) in §3 for this loop.

### Existing extension seams
Reuse HostSessionBase, Main triad, CampaignOwners, SaveSectionRegistry-or-reuse, HostCliRegistry, panel battery if interactive, CampaignStreamIds.

### Proposed architecture
Extend the live owner; prefer bind-existing UI; new routes only with claimed hubs; facades save nothing.

### Ownership matrix
| Concern | Owner |
|---|---|
| Domain | named Core owner |
| Host/Main | named paths |
| Save | named section strategy |
| Inventory/needs/relations | existing authorities only |

### Data flow / state / determinism
Follow §§8–9 and program determinism: forked `ISeededRng`, schema-gated restore, no presentation saves.

### API/contracts
See §10 for this wave.

### Data changes
Prefer existing catalogs. Add JSON only when P0 proves missing rows. snake_case. Integrity gate on catalog edits.

### Save/load / event wiring / Godot / narrative
Orchestrator Setup/Save; Lifecycle Reset; day owner phase by domain; thin panels; restrained fictional journal prose; no real-world war names.

### Failure modes
- Meta grants outside inventory authority
- Conversion ignores cooldown
- Barter ignores IsTradeRefused
- Universal: null catalog, missing ids, dead survivors, duplicate ids, save mid-transition, stale session after slot switch, unbound HOST_REQUIRED.

### Test strategy
Core unit + persistence + host command mutation + UI route gates if new route + CLI 12-check + no full suite default.

### Dependency-ordered phases
#### P0 Premise
Trace PrepareNewGameContext; list resolve/forgive APIs; find barter refusal consumers.

**Gate:** premise evidence recorded; no unrelated edits.

#### P1 NG+ board
Starting/post-verdict selection within caps.

**Gate:** premise evidence recorded; no unrelated edits.

#### P2 Ideology board
Active events; resolve confrontation/conversion/mediation; cooldown honesty.

**Gate:** premise evidence recorded; no unrelated edits.

#### P3 NPC memory
Barter/detail: trust/grudge, forgive, trade-refused banner.

**Gate:** premise evidence recorded; no unrelated edits.

#### P4 Verify
Selftests + UI gates.

**Gate:** premise evidence recorded; no unrelated edits.

### File impact map
| Action | File | Reason | Risk |
|---|---|---|---|
| CREATE | `src/UI/LegacyMetaPanel.cs` | NG+ selection | MED |
| CREATE | `src/UI/IdeologicalFrictionPanel.cs` | Resolve board | MED |
| MODIFY | `barter or detail panel` | NPC memory acts | MED |
| MODIFY | `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs` | Routes | HIGH |

### Risks
Shared hub contention; one-authority collisions; CLI-complete illusion; naming collisions.

### Out of scope
- Do not second-guess UnifiedEndingResolver
- No free-text medical notes in NPC memory
- Ideology standing only via existing ports
- Unrelated debloat; Unity; full-suite campaigns.

### Rollback
Small reversible commits per phase; UI-first revert; keep save sections readable; never reset unrelated dirty files.

### Definition of Done
- NG+ selection changes starting context as designed
- Ideology resolve updates save state
- Forgive clears trade refusal when rules say so

### Implementation handoff
**MUST PRESERVE:** one authority; Core engine-free; sealed loops; unrelated dirty files.
**MUST ADD:** player-operable path for `CampaignLegacySystem ; MetaProgressionSystem ; IdeologicalFrictionEvents ; NpcMemorySystem`.
**MUST NOT DO:** parallel ledgers; System.Random; unclaimed hub edits.
**VERIFY WITH:** acceptance commands above.
**FIRST SAFE STEP:** P0 premise + WORKTREE claim `PFGL-W6-LEGACY-IDEOLOGY-NPC`.

### Integrator execution checklist
- [ ] 1. Claim exact paths in WORKTREE_OWNERSHIP.md
- [ ] 2. Record Rule-7 rg evidence in claim note
- [ ] 3. List colliding sibling authorities
- [ ] 4. Confirm save strategy (reuse vs new+pin)
- [ ] 5. Confirm UI strategy (bind vs new route)
- [ ] 6. Implement Core gaps only if proven
- [ ] 7. Implement host/Main commands
- [ ] 8. Wire day owner if stateful tick required
- [ ] 9. Register CLI selftest
- [ ] 10. Add/adjust UI
- [ ] 11. Run focused Core tests
- [ ] 12. Run selftest
- [ ] 13. Run save/panel gates if touched
- [ ] 14. Manual player path success+failure
- [ ] 15. Write handoff; mark claim COMPLETE

### Manual player script
1. Launch host (15 FPS if runtime session). 2. Open delivered surface. 3. Success command + failure command. 4. Confirm feedback. 5. Save/load. 6. Check sibling UI non-goals. 7. Attach selftest transcript.

## PFGL-W7-DIFFICULTY-MAINT-CONTENT — Wave 7 — Difficulty Runtime Panel + Maintenance Board + Keepsake Content

**Priority:** P2
**Core owner:** `DifficultySettingsSystem ; ShelterMaintenanceSystem ; items.json` (`Difficulty + Maintenance Core + StreamingAssets items`, ~645 LOC)
**Host:** `existing`
**Main:** `src/Main.DifficultySettings.cs`
**Save:** `difficulty_settings ; shelter_maintenance`

### Why now
DEBT-PLAN181 defers runtime slider/lock panel due to shared panel-seam contention. Maintenance host-wired without board. 65/76 keepsake ids orphaned so origin skips grants.

### Player value
Mid-run challenge agency; visible shelter upkeep; origin keepsakes that appear in inventory.

### Objective
Seal this loop to PLAYER-OPERABLE using the named owner only.

### Current reality
See §2 reachability table and evidence index for this owner.

### Required delta
Close the broken completion-chain link(s) in §3 for this loop.

### Existing extension seams
Reuse HostSessionBase, Main triad, CampaignOwners, SaveSectionRegistry-or-reuse, HostCliRegistry, panel battery if interactive, CampaignStreamIds.

### Proposed architecture
Extend the live owner; prefer bind-existing UI; new routes only with claimed hubs; facades save nothing.

### Ownership matrix
| Concern | Owner |
|---|---|
| Domain | named Core owner |
| Host/Main | named paths |
| Save | named section strategy |
| Inventory/needs/relations | existing authorities only |

### Data flow / state / determinism
Follow §§8–9 and program determinism: forked `ISeededRng`, schema-gated restore, no presentation saves.

### API/contracts
See §10 for this wave.

### Data changes
Prefer existing catalogs. Add JSON only when P0 proves missing rows. snake_case. Integrity gate on catalog edits.

### Save/load / event wiring / Godot / narrative
Orchestrator Setup/Save; Lifecycle Reset; day owner phase by domain; thin panels; restrained fictional journal prose; no real-world war names.

### Failure modes
- Panel bypasses DifficultySettingsSystem
- Ironman lock bypassable
- Keepsake items missing schema/snake_case
- Universal: null catalog, missing ids, dead survivors, duplicate ids, save mid-transition, stale session after slot switch, unbound HOST_REQUIRED.

### Test strategy
Core unit + persistence + host command mutation + UI route gates if new route + CLI 12-check + no full suite default.

### Dependency-ordered phases
#### P0 Claim panel hubs
Exclusive claim on registry/GameFlow/PlayerSurfaces/Manifest.

**Gate:** premise evidence recorded; no unrelated edits.

#### P1 Difficulty panel
Eight lanes, ironman lock, preset sync; ApplyDifficultySettingsToScalars.

**Gate:** premise evidence recorded; no unrelated edits.

#### P2 Maintenance board
Components, alerts, schedule repair, inventory costs.

**Gate:** premise evidence recorded; no unrelated edits.

#### P3 Keepsake content
Author or remap 65 orphan ids; tighten origin probe.

**Gate:** premise evidence recorded; no unrelated edits.

#### P4 Verify
Difficulty selftest; maintenance tests; origin-mechanics orphan count target.

**Gate:** premise evidence recorded; no unrelated edits.

### File impact map
| Action | File | Reason | Risk |
|---|---|---|---|
| CREATE | `src/UI/DifficultySettingsPanel.cs` | Runtime panel | MED |
| MODIFY | `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs` | Route | HIGH |
| MODIFY | `src/Main.DifficultySettings.cs` | UI bridges | LOW |
| CREATE | `src/UI/ShelterMaintenancePanel.cs` | Board | MED |
| MODIFY | `Assets/StreamingAssets/Data/items.json` | Keepsake items | MED |
| MODIFY | `survivor enrichment JSON as needed` | Remaps | MED |

### Risks
Shared hub contention; one-authority collisions; CLI-complete illusion; naming collisions.

### Out of scope
- No second DifficultyScalarsProvider
- No unified HP ledger
- No runtime keepsake invention
- Unrelated debloat; Unity; full-suite campaigns.

### Rollback
Small reversible commits per phase; UI-first revert; keep save sections readable; never reset unrelated dirty files.

### Definition of Done
- DEBT-PLAN181 promotion condition met; foreman retires debt after evidence
- godot --headless --path . -- --difficulty-settings-selftest
- Keepsake orphan count meets DEC target

### Implementation handoff
**MUST PRESERVE:** one authority; Core engine-free; sealed loops; unrelated dirty files.
**MUST ADD:** player-operable path for `DifficultySettingsSystem ; ShelterMaintenanceSystem ; items.json`.
**MUST NOT DO:** parallel ledgers; System.Random; unclaimed hub edits.
**VERIFY WITH:** acceptance commands above.
**FIRST SAFE STEP:** P0 premise + WORKTREE claim `PFGL-W7-DIFFICULTY-MAINT-CONTENT`.

### Integrator execution checklist
- [ ] 1. Claim exact paths in WORKTREE_OWNERSHIP.md
- [ ] 2. Record Rule-7 rg evidence in claim note
- [ ] 3. List colliding sibling authorities
- [ ] 4. Confirm save strategy (reuse vs new+pin)
- [ ] 5. Confirm UI strategy (bind vs new route)
- [ ] 6. Implement Core gaps only if proven
- [ ] 7. Implement host/Main commands
- [ ] 8. Wire day owner if stateful tick required
- [ ] 9. Register CLI selftest
- [ ] 10. Add/adjust UI
- [ ] 11. Run focused Core tests
- [ ] 12. Run selftest
- [ ] 13. Run save/panel gates if touched
- [ ] 14. Manual player path success+failure
- [ ] 15. Write handoff; mark claim COMPLETE

### Manual player script
1. Launch host (15 FPS if runtime session). 2. Open delivered surface. 3. Success command + failure command. 4. Confirm feedback. 5. Save/load. 6. Check sibling UI non-goals. 7. Attach selftest transcript.

## PFGL-W8-CHRONIC-DECISION — Wave 8 — Chronic Conditions DEC + Wire-or-Retire

**Priority:** P2
**Core owner:** `ChronicConditionSystem vs Afflictions/Amputation/HealthHistory/Bionics` (`Assets/Ashfall.Core/Medical/ChronicConditionSystem.cs`, ~345 LOC)
**Host:** `CREATE only if DEC-A`
**Main:** `CREATE only if DEC-A`
**Save:** `decision-dependent`

### Why now
TRUE ORPHAN medical system. Wiring without one-authority decision risks parallel disability ledgers against afflictions, amputation limb gates, and health history.

### Player value
Long-term impairment accommodations — only if custody is clear.

### Objective
Seal this loop to PLAYER-OPERABLE using the named owner only.

### Current reality
See §2 reachability table and evidence index for this owner.

### Required delta
Close the broken completion-chain link(s) in §3 for this loop.

### Existing extension seams
Reuse HostSessionBase, Main triad, CampaignOwners, SaveSectionRegistry-or-reuse, HostCliRegistry, panel battery if interactive, CampaignStreamIds.

### Proposed architecture
Extend the live owner; prefer bind-existing UI; new routes only with claimed hubs; facades save nothing.

### Ownership matrix
| Concern | Owner |
|---|---|
| Domain | named Core owner |
| Host/Main | named paths |
| Save | named section strategy |
| Inventory/needs/relations | existing authorities only |

### Data flow / state / determinism
Follow §§8–9 and program determinism: forked `ISeededRng`, schema-gated restore, no presentation saves.

### API/contracts
See §10 for this wave.

### Data changes
Prefer existing catalogs. Add JSON only when P0 proves missing rows. snake_case. Integrity gate on catalog edits.

### Save/load / event wiring / Godot / narrative
Orchestrator Setup/Save; Lifecycle Reset; day owner phase by domain; thin panels; restrained fictional journal prose; no real-world war names.

### Failure modes
- Silent dual penalties
- Partial wire leaving HOST_REQUIRED unbound
- Universal: null catalog, missing ids, dead survivors, duplicate ids, save mid-transition, stale session after slot switch, unbound HOST_REQUIRED.

### Test strategy
Core unit + persistence + host command mutation + UI route gates if new route + CLI 12-check + no full suite default.

### Dependency-ordered phases
#### P0 Forensic map
Compare capability_penalties vs EquipLimbGate vs affliction bridge vs health history.

**Gate:** premise evidence recorded; no unrelated edits.

#### P1 DEC-CHRONIC
Sign A wire / B retire-fold / C TEST_ONLY.

**Gate:** premise evidence recorded; no unrelated edits.

#### P2 Implement signed path only
A: Bestiary-style host+Afflictions strip. B: retirement plan. C: port-contract TEST_ONLY.

**Gate:** premise evidence recorded; no unrelated edits.

#### P3 Verify
Focused medical tests; no parallel write paths.

**Gate:** premise evidence recorded; no unrelated edits.

### File impact map
| Action | File | Reason | Risk |
|---|---|---|---|
| DOC | `docs/governance/DECISION_REGISTER.md` | DEC-CHRONIC | LOW |
| CONDITIONAL | `src/Host/ChronicConditionHostSession.cs` | Only if A | MED |
| CONDITIONAL | `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` | Only if A | HIGH |

### Risks
Shared hub contention; one-authority collisions; CLI-complete illusion; naming collisions.

### Out of scope
- No wire before DEC
- Do not duplicate amputation multipliers
- No stigmatizing free text
- Unrelated debloat; Unity; full-suite campaigns.

### Rollback
Small reversible commits per phase; UI-first revert; keep save sections readable; never reset unrelated dirty files.

### Definition of Done
- DEC recorded
- Either host_files>=1 with commands/UI, or explicit retirement/TEST_ONLY with no unbound HOST_REQUIRED

### Implementation handoff
**MUST PRESERVE:** one authority; Core engine-free; sealed loops; unrelated dirty files.
**MUST ADD:** player-operable path for `ChronicConditionSystem vs Afflictions/Amputation/HealthHistory/Bionics`.
**MUST NOT DO:** parallel ledgers; System.Random; unclaimed hub edits.
**VERIFY WITH:** acceptance commands above.
**FIRST SAFE STEP:** P0 premise + WORKTREE claim `PFGL-W8-CHRONIC-DECISION`.

### Integrator execution checklist
- [ ] 1. Claim exact paths in WORKTREE_OWNERSHIP.md
- [ ] 2. Record Rule-7 rg evidence in claim note
- [ ] 3. List colliding sibling authorities
- [ ] 4. Confirm save strategy (reuse vs new+pin)
- [ ] 5. Confirm UI strategy (bind vs new route)
- [ ] 6. Implement Core gaps only if proven
- [ ] 7. Implement host/Main commands
- [ ] 8. Wire day owner if stateful tick required
- [ ] 9. Register CLI selftest
- [ ] 10. Add/adjust UI
- [ ] 11. Run focused Core tests
- [ ] 12. Run selftest
- [ ] 13. Run save/panel gates if touched
- [ ] 14. Manual player path success+failure
- [ ] 15. Write handoff; mark claim COMPLETE

### Manual player script
1. Launch host (15 FPS if runtime session). 2. Open delivered surface. 3. Success command + failure command. 4. Confirm feedback. 5. Save/load. 6. Check sibling UI non-goals. 7. Attach selftest transcript.


## PFGL-W9-SURVIVOR-ROUTINES-BOARD — Wave 9 — Daily Routines Board + Schedule Conflict Mechanic

### Why now
Plan 188 already shipped a complete Core authority (`SurvivorRoutineSystem`), Godot host session, save section `survivor_routines`, day owner (phase 5), catalog (`routine_templates.json` with four canonical templates), and a 12-check CLI selftest. The only remaining broken link on the completion chain is **PLAYER-OPERABLE**: `SurvivorDetailPanel` exposes a read-only `RoutineProvider`, and no interactive surface can assign templates, edit preferences, or resolve conflicts. That makes daily life scheduling invisible to the player even though the simulation already evaluates satisfaction and detects roommate/workspace clashes every day advance.

R2 also seals a **game mechanic gap**: satisfaction scores are computed and stored, but `TickSurvivorRoutines` does not yet port `OverallSatisfaction` (and component scores) into the existing Needs / morale owners. Closing that port turns an observational score into a felt loop — bad schedules degrade rest/social outcomes; good schedules stabilize them — without inventing a parallel wellbeing ledger.

### Player value
Players manage shelter chronobiology the way they already manage room and workstation assignments: pick a template (early riser, night owl, standard, night shift), tune preferences, watch conflicts light up when roommates’ sleep windows diverge or workstations overlap at noon, resolve those conflicts, and feel the morale/needs consequences. This is a high-frequency daily loop that compounds with romance (W1 social time), recruitment intake (W2 new survivors need defaults), cooking meal cadence (W5), and maintenance quiet hours (W7).

### Objective
Deliver a routed **Survivor Routines Board** (and/or operable Detail actions) bound exclusively to `SurvivorRoutineHostSession`, plus a thin satisfaction→Needs/morale port adapter owned by the existing wellbeing authorities, verified by focused tests and the existing routines selftest.

### Current reality
| Layer | Evidence |
|---|---|
| Core | `Assets/Ashfall.Core/Survivors/SurvivorRoutineSystem.cs` — templates, routines, preferences, satisfaction, conflicts, Capture/Restore |
| Catalog | `Assets/StreamingAssets/Data/routine_templates.json` — `routine_early_riser`, `routine_night_owl`, `routine_standard`, `routine_night_shift` |
| Host | `src/Host/SurvivorRoutineHostSession.cs` — `AssignRoutine`, `SetPreference`, `EvaluateDailySatisfaction`, `DetectConflicts`, `ResolveConflict`, `GetActivityAtHour`, `SetEnforcementLevel` |
| Main | `src/Main.SurvivorRoutines.cs` — Setup/Tick/Save/Reset; Detail `RoutineProvider` wiring; room/workspace maps from `ShelterAssignmentSystem` |
| Day owner | `Main.CampaignOwners` — `survivor_routines` phase 5 `SurvivorRoutinesDayOwner` calls `TickSurvivorRoutines` |
| Save | `SaveSectionRegistry` row `survivor_routines` → `survivor_routines_save.json`; pin **266** already includes this section |
| CLI | `HostCliAction.SurvivorRoutinesSelfTest` — 12 checks including assign, preference, satisfaction ideal/poor, sleep conflict detect/resolve, roundtrip |
| UI | `SurvivorDetailPanel.RoutineProvider` read-only only; **no** PanelRegistry / GameFlow / PlayerSurfaces interactive route for routines |
| Mechanic gap | Tick evaluates satisfaction from approximate needs thresholds; **no** write-back into Needs/morale owners |

### Required delta
1. Interactive UI surface (new board route **or** Detail command strip) calling Main wrappers → host methods for assign / preference / enforce / resolve.
2. Truthful conflict list + severity badges sourced from `GetActiveConflicts()`.
3. Hourly activity readout via `GetActivityAtHour` for selected survivor (schedule strip).
4. Satisfaction port adapter: on tick (or on `OnSatisfactionEvaluated`), map Overall/Sleep/Social scores into existing Needs/morale mutation APIs with documented clamps — **extend owners**, do not store a second wellbeing number in the routines section beyond what Core already persists.
5. Manifest + panel battery registration so the surface counts as InteractiveCommands.
6. Focused tests for UI-bound command path (host-level) and port adapter determinism; re-run routines selftest.
7. Journal facts for assign / resolve / enforcement changes.

### Existing extension seams
- Prefer **bind/extend SurvivorDetailPanel** for per-survivor assign/prefer actions (already has RoutineProvider).
- Prefer **new thin board** for shelter-wide conflict queue + enforcement level (Shelter Operations Board pattern — facade over existing session, no new save section).
- Reuse `ShelterAssignmentSystem` room/workspace maps already consumed by `TickSurvivorRoutines`.
- Reuse Needs / morale / fatigue owners for ports; never create `RoutineWellbeingSystem`.

### Proposed architecture
```
[Routines Board / Detail strip]
    → Main.AssignSurvivorRoutine / SetSurvivorRoutinePreference / ResolveSurvivorRoutineConflict / SetRoutineEnforcement
        → SurvivorRoutineHostSession.*
            → SurvivorRoutineSystem (sole schedule authority)
                → events OnRoutineAssigned / OnConflictDetected / OnConflictResolved / OnSatisfactionEvaluated
                    → journal adapter
                    → satisfaction port adapter → Needs/morale owners (clamped)
                        → UI refresh from census + GetRoutine + GetActiveConflicts
                            → existing SaveSurvivorRoutines / CaptureSection (no new registry row)
```

### Ownership matrix
| Concern | Owner |
|---|---|
| Schedule templates & blocks | `SurvivorRoutineSystem` |
| Preferences / enforcement | same |
| Conflict detect/resolve | same |
| Persistence | `survivor_routines` section (existing) |
| Room/workspace facts | `ShelterAssignmentSystem` (read) |
| Needs/morale mutation | existing Needs / morale authorities via port adapter |
| UI presentation | new board + Detail strip |
| RNG | none required for assign/resolve; satisfaction inputs come from deterministic day tick approximations unless a future fork is justified |

### Data flow / state / determinism
- Assign/prefer/resolve are deterministic given ids and catalog.
- Daily satisfaction inputs today are derived from roster needs thresholds inside `TickSurvivorRoutines` (hoursWorked fixed 8; sleep/meals approximated). W9 may refine input derivation **only** if it stays deterministic and owned; wall-clock and `System.Random` remain forbidden.
- Conflict ids use Core `NextSequence` — preserve across save/load.
- Port adapter must be idempotent per day per survivor (do not stack morale hits on UI refresh).

### API/contracts
Host (already public — UI must call these, not reimplement):
- `AssignRoutine(survivorId, templateId) → SurvivorRoutineRecord`
- `SetPreference(survivorId, chronotype, workShift, social)`
- `SetEnforcementLevel(level)` — `strict` | `flexible` | `none`
- `EvaluateDailySatisfaction(survivorId, day, hoursWorked, hoursSlept, mealsHad, socialHours)`
- `DetectConflicts(day, roomAssignments, workspaceAssignments)`
- `ResolveConflict(conflictId) → bool`
- `GetActivityAtHour(survivorId, hour) → activity_type`
- `GetActiveConflicts()` / `GetCensus()` / `CaptureState` / `RestoreState`

New Main wrappers (thin):
- `AssignSurvivorRoutine`, `SetSurvivorRoutinePreference`, `ResolveSurvivorRoutineConflict`, `SetRoutineEnforcementLevel`, `GetSurvivorRoutineBoardSnapshot`

New port adapter contract (design — implement under Needs/morale owner files claimed in WORKTREE):
- `ApplyRoutineSatisfactionPorts(RoutineSatisfactionRecord record)` via `NeedsSystem.Modify` / `ApplyAttributedDelta` — SleepSatisfaction→Fatigue; Social/Overall→Morale with **higher=worse** delta signs; MealSatisfaction observational by default; see Appendix P/BK (R3).

### Data changes
- Catalog already valid; optional content pass may add 1–2 templates only if integrity gate stays green.
- No new save section. No parallel JSON wellbeing store.
- Player surface manifest: add interactive routines board entry; keep Detail observational fields accurate.

### Save/load / event wiring / Godot / narrative
- Save path unchanged: `SaveSurvivorRoutines` / `FlushSurvivorRoutinesIfDirty` / registry capture.
- Journal: assign template, resolve conflict, enforcement change, optional low-satisfaction day note (facts only).
- Godot: panel class + bootstrap route + GameFlow open/close + focus/back parity.
- Narrative tone: restrained schedule language (“sleep windows clash”, “night shift template”) — no real-world labor-law pastiche.

### Failure modes
| Failure | Handling |
|---|---|
| Unknown template id | Host/Core reject; UI shows catalog list only |
| Unknown survivor id | Reject; no phantom routine row |
| Resolve already-resolved / missing conflict | `ResolveConflict` false; UI refreshes census |
| Port adapter owner null | Fail-closed skip; routines state still persists |
| Double day tick | Idempotent ports by (survivorId, day) guard |
| Detail provider null before Setup | Setup-on-demand already present; board calls Setup first |

### Test strategy
- Re-run `SurvivorRoutinesSelfTest` (12-check) — must stay green.
- Focused Core tests already covering assign/conflict/satisfaction remain authoritative; add port-adapter unit tests only for the new mapping function.
- Static: rg that no second routines ledger type appears; panel calls Main/host only.
- Manual: assign night owl + early riser to same room → conflict appears → resolve → census ActiveConflicts decrements; satisfaction poor path moves morale readout.

### Dependency-ordered phases
| Phase | Work | Exit |
|---|---|---|
| P0 Premise | Re-rg host files; confirm save key; claim UI hubs + Needs port files in WORKTREE | Claim accepted |
| P1 Port design | Map satisfaction fields → live Needs/morale APIs; write scalar table in package note | One-authority sign-off |
| P2 Main commands | Thin wrappers + journal + board snapshot DTO | CLI + compile |
| P3 UI board | Panel + Detail strip + registry/manifest/GameFlow | Route opens; commands mutate Core |
| P4 Port wire | Adapter on tick / satisfaction event; idempotency guard | Needs/morale move with scores |
| P5 Verify | Selftest + focused tests + manual script | DoD |

### File impact map
| File | Action |
|---|---|
| `src/UI/SurvivorRoutinesBoardPanel.cs` (new) **or** Detail extension | Add |
| `src/UI/SurvivorDetailPanel.cs` | Extend actions beyond RoutineProvider |
| `src/UI/PanelRegistryBootstrap.cs` | Register |
| `src/Main.GameFlow.cs` | Open/close |
| `src/Main.PlayerSurfaces.cs` / manifest | Interactive entry |
| `src/Main.SurvivorRoutines.cs` | Wrappers + port hook |
| Needs/morale owner files (exact paths from P1) | Port adapter only |
| `WORKTREE_OWNERSHIP.md` | Claim |
| Tests under `Ashfall.Core.Tests/` (focused) | Port mapping tests |
| **Do not touch** | `SaveSectionRegistry` pin (reuse section); Combat stealth; Recruitment; Chronic |

### Risks
| Risk | Mitigation |
|---|---|
| Port adapter becomes a second wellbeing authority | Adapter-only; scalars documented; state remains in Needs owners |
| Conflict spam from DetectConflicts re-adding duplicates | Premise: audit Core for duplicate conflict rows; harden required (proven R4) |
| UI invents schedule math | Forbidden — display GetActivityAtHour / census only |
| Overlap with InterpersonalConflictSystem | Routines conflicts are schedule-typed; do not merge ledgers |

### Out of scope
- Replacing ShelterAssignmentSystem.
- Full chronotype simulation with per-minute agent AI.
- Wiring EmergencyAlertSystem.
- Stealth fire rework (already bridged).
- New save section / pin bump.
- Parallel `RoutineWellbeingSystem`.

### Rollback
Revert UI + Main wrappers + port adapter files; leave Core/host/save as they are (already sealed). Routines fall back to read-only provider behavior.

### Definition of Done
- [ ] Player can assign any of the four catalog templates from UI
- [ ] Player can set chronotype / work shift / social preference from UI
- [ ] Active conflicts visible; Resolve clears via Core
- [ ] Enforcement level controllable
- [ ] Satisfaction ports move existing Needs/morale readouts under a deterministic test
- [ ] `SurvivorRoutinesSelfTest` 12/12 pass
- [ ] Manifest interactive count reflects new board
- [ ] No new SaveSectionRegistry row; pin stays 266 unless unrelated work landed
- [ ] WORKTREE claims released / handed off

### Implementation handoff
**Package:** `PFGL-W9-SURVIVOR-ROUTINES-BOARD`
**Class:** EXPAND + MECHANIC (player board + satisfaction ports)
**Depends on:** none hard; softer synergy after W1 (social hours) and shelter assignment stability
**FIRST SAFE STEP:** P0 premise rg + WORKTREE claim for panel hubs and Needs port files.
**Blocked by:** none decision-gated (unlike W5 governance / W8 chronic).

### Integrator execution checklist
1. `rg -l SurvivorRoutineSystem src --glob '*.cs'`
2. Confirm `survivor_routines` in SaveSectionRegistry and pin 266
3. Claim files in WORKTREE_OWNERSHIP.md
4. Implement P2→P5 in order
5. Run HostCli SurvivorRoutinesSelfTest
6. Run focused port tests via `bash scripts/run_test.sh <file>`
7. Manual player script below
8. Handoff per AI_AGENT_WORKFLOW.md

### Manual player script
1. Open Routines board (or Survivor Detail → Routines actions).
2. Assign survivor A `routine_early_riser`, survivor B `routine_night_shift`.
3. Ensure A and B share a room via shelter assignment.
4. Advance day → ActiveConflicts shows sleep_disturbance.
5. Resolve conflict → ActiveConflicts decrements; journal fact appears.
6. Set A preference extrovert; advance with poor social hours → satisfaction/morale port moves.
7. Save / load → routines, preferences, resolved conflict persist.


# Program-level Save/Load strategy

| Wave | Section strategy |
|---|---|
| W1 | Reuse `romance_family` |
| W2 | Likely new `recruitment` → pin bump + tests |
| W3 | Reuse `vehicle_customization` |
| W4 | Reuse `trade_routes`, `colony` |
| W5 | Reuse governance/cooking/disaster |
| W6 | Reuse legacy/meta/ideology/npc_memory |
| W7 | Reuse difficulty_settings / shelter_maintenance; content JSON for keepsakes |
| W8 | New only if DEC-A |

Always: Setup on restore path; Save in SaveAll; Reset on lifecycle; CaptureSection after store write.

# Program-level Determinism strategy

- Master seed from campaign day.
- New stochastic domains register snake_case `CampaignStreamIds`.
- Recruitment success, romance picks, ideology conversion, colony encounters: fork(day, actionIndex).
- UI order must not affect simulation order; Core sorts by id.
- Never `System.Random` for gameplay.

# Program-level Test strategy

1. Smallest Core tests via `bash scripts/run_test.sh …`
2. New selftest alone first
3. Save section added → ComprehensiveSaveStoreCorruptionAndMigrationTests + PersistentFilenameRegistryGateTests + SaveSectionRegistryTests
4. UI route added → PanelRouteGateTests + PlayerSurfaceCoverageGateTests (+ MainTriadDriftGateTests if triad touched)
5. Port contract `--check` when public Core seams change
6. Optional `--7-day-smoke-selftest` / `--player-panels-uitest` when day/UI spine touched
7. Never default full suite

# Program-level Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Shared panel hub race | HIGH | Exclusive claims; one UI wave at a time |
| Governance dual-write | HIGH | DEC before code |
| Chronic dual-penalty | HIGH | DEC before code |
| CLI-complete illusion | MED | DoD requires PLAYER-OPERABLE |
| Colony/apiary naming | MED | UI copy + separate route ids |
| Pin drift | MED | Update assert same PR |
| Keepsake orphans | MED | Author/remap; no runtime invent |
| Scope into wildfire/clinic hypotheses | MED | Stay out of scope until signed |

# Program-level Out of Scope

1. `WaterSourceSystem` wiring
2. `FoodTypeSystem` live owner revival
3. `FactionDiplomacySystem` parallel without custody DEC
4. `EmergencyAlertSystem` unless DEC says DisasterResponse cannot absorb
5. Wildland firefront / mobile medical outreach / public works / scenario library greenfield Core
6. Specialty kiln/press/glass polish before W1–W5
7. Aviation/naval graph travel migration
8. Unity restoration
9. Full-suite mandatory green as package acceptance
10. XP-04 economy legs / XP-06 body-integrity schema (decision-blocked elsewhere)

# Program-level Rollback

One wave per claim; revert claim on failure. Prefer UI-only reverts leaving Core/host intact. Save sections once shipped remain readable no-ops if UI removed. Never force-push; never reset dirty unrelated files.

# Program Definition of Done

1. Waves W1–W7 sealed with evidence in `INTEGRATION_PLANS.md`
2. W8 wired per DEC-A or explicitly retired/classified
3. `DEBT-PLAN181` retired after runtime panel evidence
4. Keepsake orphan debt promoted/retired per content target
5. Player surface interactive count increases for new boards
6. No new HOST_REQUIRED unbound seams
7. No parallel authorities introduced
8. Focused verifications attached per `AI_AGENT_WORKFLOW.md` handoff
9. Every wave passed PIR-1…PIR-10 (Appendix BT) with scrapbook evidence before code landed
10. Every wave declared its seal tier (gap / feature / mechanic) and met that tier's Appendix BU4 checklist

# Implementation Handoff (program)

## MUST PRESERVE
Godot authority; Core engine-free; JSON data authority; one authority per concern; sealed 2026-09-24/25 loops; MasterSeed determinism; save pin discipline.

## MUST ADD
Player-operable boards/commands for ranked loops; Recruitment host chain; signed DECs for governance and chronic; keepsake content.

## MUST NOT DO
Parallel ledgers; unclaimed hub edits; System.Random gameplay; Unity; wiring false-parallel systems; treating CLI PASS as player-facing Done.

## VERIFY WITH
Per-wave acceptance commands; port-contract check; panel gates when UI changes; origin-mechanics selftest for keepsakes.

## FIRST SAFE IMPLEMENTATION STEP
0. Run the Pre-Integration Readiness Gate (Appendix BT: PIR-1…PIR-10) for the wave and paste the record into the acceptance scrapbook — **no wave code before PIR passes**.
1. Foreman opens `PFGL-W1-ROMANCE-BOARD` claim with exact W1 file list.
2. Builder runs P0 `rg` premise and records host/UI evidence (paste live signatures per PIR-3).
3. Prefer SurvivorDetail romance actions without a new route if possible.

# Ranked opportunity table

| Rank | Action | Priority | Impact | Readiness | Risk | Unlocks |
|---:|---|---|---|---|---|---|
| 1 | Romance operable board | P1 | High | High | Low-Med | Family/gen hooks |
| 2 | Recruitment host wire | P1 | High | Med | Med | Population agency |
| 3 | Vehicle modules on garage | P1 | High | High | Low-Med | Expedition loadouts |
| 4 | Trade routes board | P1 | High | High | Med | Long economy |
| 5 | Colony board | P1 | High | High | Med | Territorial play |
| 6 | Governance DEC+UI | P1 | High | Med | High | Political play |
| 7 | Kitchen↔Cooking bind | P1 | High | High | Med | Food pipeline |
| 8 | Disaster protocol surface | P1 | Med-High | High | Med | Crisis agency |
| 9 | NG+ legacy board | P2 | Med-High | High | Med | Replay |
| 10 | Ideology resolve board | P2 | Med | High | Med | Belief play |
| 11 | NPC memory acts | P2 | Med | High | Low-Med | Barter depth |
| 12 | Difficulty runtime panel | P2 | Med | High | Med | Challenge agency |
| 13 | Maintenance board | P2 | Med | High | Low-Med | Shelter upkeep |
| 14 | Keepsake content | P2 | Med | High | Low | Origin honesty |
| 15 | Chronic DEC→wire/retire | P2 | Med | Low until DEC | High | Medical depth |
| 16 | Survivor routines board + satisfaction ports | P1 | High | High | Low-Med | Daily agency + mechanic |

# NOW / NEXT / LATER

### NOW
1. Claim + execute `PFGL-W1-ROMANCE-BOARD`
2. Claim + execute `PFGL-W9-SURVIVOR-ROUTINES-BOARD` (board + satisfaction→Needs/morale ports)
3. Premise-audit `PFGL-W2-RECRUITMENT-WIRE` costs/ids
4. Draft DEC-GOV one-pager (no code)

### NEXT
5. W3 vehicle modules
6. W4 trade+colony
7. W5 cook/disaster (governance only after DEC-GOV)
8. W6 social consequence boards
9. W7 difficulty+maintenance+keepsakes

### LATER
10. W8 chronic decision (DEC-CHRONIC)
11. EmergencyAlertSystem only with separate DEC
12. Only then consider gap-audit wildfire/clinic/public-works/scenario programs

# What NOT to do yet

- Do not start wildland firefront Core
- Do not wire WaterSourceSystem / FoodTypeSystem / FactionDiplomacySystem
- Do not spend the panel-hub claim on kiln/press polish before romance/routines/recruitment/vehicle/trade
- Do not treat `PARTIAL_REMAINING_PLACEHOLDER_2026-09-19.md` “15/15 integrated” as player-facing completeness
- Do not run the full test suite to prove a wave
- Do not reopen stealth weapon-noise bridging (`ActionFire` already calls `ApplyWeaponNoise`)
- Do not invent `RoutineWellbeingSystem` — port into existing Needs/morale owners
- Do not bump the save pin for W9 (`survivor_routines` already registered)

# Creative expansion opportunity (after W1–W6 + W9)

Deepen **household + recruitment + colony + daily routines** into a population strategy tetrad: romance creates dependents, recruitment fills skills, colonies export surplus and import risk, and routines make chronobiology an operable lever that ports into Needs/morale. That tetrad is supported by existing Core and yields more player decisions than adding new hazard engines.

Optional content after boards ship: additional routine templates, romance activities that consume Social blocks, recruitment intake defaulting to `routine_standard`.

# Suggested follow-up skill

After foreman accepts this plan and claims a NOW wave: use **ashfall-implement** / careful integration implementer on `PFGL-W1-ROMANCE-BOARD` or `PFGL-W9-SURVIVOR-ROUTINES-BOARD` only (one package at a time).

# Appendix selection note (R4)

R2/R3 produced a large appendix mountain (E–BH and many duplicate evidence cards). R4 retains the **core wave contracts** plus precision-critical appendices (A–D, BI–BR subset) and the **R4 revision block** containing the fresh gap audit and **W10 Sleep Acoustic recovery** package. Historical R2 essay appendices remain recoverable from git history of this file at R3; they are omitted here to keep the active plan evidence-dense near the 200k character target without empty attestation slots.

---

# Appendix A — Re-measurement commands (Rule 7)

```bash
for n in RecruitmentSystem ChronicConditionSystem EmergencyAlertSystem \
         RomanceFamilySystem VehicleCustomizationSystem PlayerTradeRouteSystem \
         ColonySystem ShelterGovernanceEngine CookingSystem DisasterResponseSystem \
         CampaignLegacySystem MetaProgressionSystem NpcMemorySystem \
         IdeologicalFrictionEvents ShelterMaintenanceSystem DifficultySettingsSystem; do
  echo "$n host=$(rg -l "\\b$n\\b" src --glob '*.cs' | wc -l)"
done

rg -n "RomanceProvider|InstallModule|StartCooking|FormFamilyUnit" src Assets/Ashfall.Core --glob '*.cs'
rg -n "DEBT-PLAN181|DEBT-ENRICHMENT-KEEPSAKE" KNOWN_DEBT.md
```

# Appendix B — Shared hubs to claim

- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`
- `Assets/Ashfall.Core/HostCliRegistry.cs`
- `Assets/Ashfall.Core/Campaign/DayEventVocabulary.cs`
- `src/Main.CampaignOwners.cs`
- `src/Main.Lifecycle.cs`
- `src/Main.SaveOrchestrator.cs`
- `src/Main.Application.cs`
- `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`
- `src/Main.GameFlow.cs`
- `src/Main.PlayerSurfaces.cs`
- `Assets/Ashfall.Core/UI/PlayerSurfaceManifest.cs`

# Appendix C — False positives (do not reopen as missing host)

LeadershipSystem, BestiarySystem, HealthHistorySystem, ChildDevelopmentSystem, MemoryDecaySystem, ExerciseSystem, DreamSystem, InterpersonalConflictSystem, PersonalQuestSystem, BlackMarketSystem, SanitationSystem, CompanionAnimalSystem (Plan 151), AfflictionQuestWorkBridge (Plan 143), NightWatch panel, Shelter Operations Board, Water Sources surface, Radio program production, MutationSystem (Plan 172), SkillCertificationSystem, PsychologicalProfileSystem, CultureCreationSystem.

# Appendix D — Claim & handoff templates

```
claim-pfgl-wN-<slug>-YYYY-MM-DD
Owner: <integrator>
Paths: <exact files>
Non-goals: <from wave>
Acceptance: <commands>
Status: ACTIVE
```

```
Outcome:
Files:
Contract:
Commands/results:
Limitations:
Shared paths intentionally untouched:
```

# Appendix BI — R3 precision corrections log

## BI1. Why R3 exists
R2 reached the ~200k budget but closed with explicit **padding attestation slots** (`Appendix BL199325` etc.) and meta size notes. R3 deletes those and spends the budget on live-API corrections that would otherwise fail P0.

## BI2. Corrections applied this revision
| Area | Stale / weak claim | Live evidence | Plan treatment |
|---|---|---|---|
| Romance verbs | `BeginCourtship` / `AdvanceRomance` / `BreakRelationship` | `RomanceFamilyHostSession`: `TryInitiateAttraction`, `ConductCourtshipEvent`, `DissolvePartnership`, `FormFamilyUnit` | All W1 API lists + acceptance rg updated |
| W9 morale ports | Vague “morale negative” without polarity | `NeedsSystem`: Morale **higher = worse** (0..100) | Scalar table rewritten; deltas signed correctly |
| W9 mutator | “Needs/morale owners” unnamed | `Modify` / `ApplyAttributedDelta` / external modifiers on `NeedsSystem` | Named mutators + attributed source `routine_satisfaction` |
| Day ordering | Unspecified vs needs tick | `survivors_needs` phase 3; `survivor_routines` phase 5 | Ports after needs tick — intentional |
| Conflict retick | Risk noted only | `DetectConflicts` always `_state.Conflicts.Add` with no uniqueness predicate | Elevated to **required P0 harden** if duplicate unresolved rows share `(ConflictType, SurvivorA, SurvivorB, Day)` |
| Romance AdvanceDay | Missing from some lists | `AdvanceDay(int currentDay)` on host | Listed under W1 contracts |
| Padding slots | BL199* / BK size note | n/a | **Deleted** |

## BI3. Romance host method card (paste into W1 P0)
```
TryInitiateAttraction(...)
ConductCourtshipEvent(...)
DissolvePartnership(survivorA, survivorB, reason = "mutual_drift")
FormFamilyUnit(survivorA, survivorB, familyName)
AddChildToFamily(familyId, childId, isAdopted)
CalculateCompatibility(ageA, ageB, beliefA, beliefB, sharedTrauma)
GetRelationship / GetRomanticPartner / GetFamilyForSurvivor
AdvanceDay(currentDay)
CapturePersistedState / Restore paths via RomanceFamilySaveStore
Census via GetCensus()
```
UI labels may say “Begin courtship”; code identifiers must match the host methods above.

## BI4. Conflict uniqueness harden sketch (Core — only if P0 proves spam)
Predicate before Add:
`(ConflictType, SurvivorA, SurvivorB, Day)` with survivor ids ordered canonically (`OrdinalIgnoreCase` min/max) so A↔B and B↔A collapse. Skip insert when an unresolved match exists. Still emit no duplicate `OnConflictDetected` for the skipped row. Claim `SurvivorRoutineSystem.cs` only when this bug is proven by a focused retick test.

---

# Appendix BJ — Immediate next-agent briefing (R3)

If authorized to implement next:

1. Prefer `PFGL-W1-ROMANCE-BOARD` or `PFGL-W9-SURVIVOR-ROUTINES-BOARD`.
2. Run the R4-3 cross-wave precision runbook probes (successor of the old Appendix AS premise script); paste results into handoff.
3. For W1: bind UI to **live** romance verbs (`TryInitiateAttraction` / `ConductCourtshipEvent` / `DissolvePartnership` / `FormFamilyUnit`) — do not invent `BeginCourtship` wrappers unless they are thin aliases over those methods.
4. For W9: implement ports with `NeedsSystem` polarity (Morale higher=worse); prefer `ApplyAttributedDelta` with source `routine_satisfaction`.
5. For W9 P0: write a focused retick test proving whether `DetectConflicts` duplicates; harden before shipping the board if yes.
6. Claim WORKTREE paths with the R4-19 claim template (successor of the old Appendix U matrices); respect the R4-35 shared-hub caution list.
7. Do not start W5 governance writes or W8 without DEC signatures.
8. Do not reopen stealth fire noise (`ActionFire` → `ApplyWeaponNoise` already live).
9. Do not bump save pin for W9.

If remaining in planning mode: this document (R3) is the PFGL authority until a newer revision stamps a new evidence HEAD.

---

# Appendix BK — NeedsSystem port cookbook (W9 implementer sheet)

## BK1. Read model
Per-survivor fields on `SurvivorNeedsState`: Hunger, Thirst, Fatigue, Warmth, Morale, Health, Hygiene, Numbness, RadiationAnxiety.

## BK2. Write API choices
| API | Use when |
|---|---|
| `Modify(id, NeedKind, delta)` | Simple one-shot day port |
| `ApplyAttributedDelta(id, NeedKind, …)` | Preferred — source-tagged for diagnostics |
| `SetExternalModifier(id, sourceId, NeedKind, …)` | Multi-day lingering bias (usually **overkill** for W9 v1) |

## BK3. Sign convention cheat-sheet
| Intent | Fatigue delta | Morale delta |
|---|---|---|
| Make survivor worse | `+` | `+` |
| Make survivor better | `−` | `−` |

## BK4. Idempotency store
Keep a process-local or Core-adjacent set of applied keys `"survivorId:day"` for the adapter. Do not persist a second wellbeing section; clearing on day rollover is enough if the tick runs once per day owner advance. If Main can call Tick twice, the set is mandatory.

## BK5. Wiring site
`src/Main.SurvivorRoutines.cs` inside `TickSurvivorRoutines` after `EvaluateDailySatisfaction(...)` returns. Resolve `NeedsSystem` through the existing survivors/needs host path already used by `SurvivorsNeedsDayOwner` — do not construct a parallel `new NeedsSystem()`.

## BK6. Verification
```bash
rg -n 'ApplyAttributedDelta|NeedKind\\.Morale|NeedKind\\.Fatigue|routine_satisfaction' src -g '*.cs'
bash scripts/run_test.sh Ashfall.Core.Tests/<RoutineSatisfactionPortTests>.cs
# polarity unit: OverallSatisfaction 20 => Morale value increased (worse)
# polarity unit: OverallSatisfaction 90 => Morale value decreased (better) within clamps
```

---

# Appendix BL — DetectConflicts duplicate forensic protocol

## BL1. Minimal repro (xUnit sketch)
1. Assign `routine_early_riser` + `routine_night_shift` to two survivors.
2. Pass identical roomAssignments map for both.
3. Call `DetectConflicts(day=10, rooms, null)` twice.
4. Count unresolved `sleep_disturbance` rows for that pair/day.

**Pass criterion for “needs harden”:** count ≥ 2 after two calls.
**Pass criterion for “clean”:** count == 1.

## BL2. If harden lands in W9
- File: `Assets/Ashfall.Core/Survivors/SurvivorRoutineSystem.cs` only.
- Add focused test file; run alone first.
- Do not change save schema; uniqueness is behavioral.
- Update HostCli selftest only if check 8–10 assumptions break.

## BL3. UI implication
Even after harden, board must list `GetActiveConflicts()` not a client-side reconstruction.

---

# Appendix BM — W1 UI binding precision (Romance)

## BM1. Control → method map
| Player-facing control | Host method |
|---|---|
| Begin courtship / show attraction | `TryInitiateAttraction` |
| Advance / conduct outing or beat | `ConductCourtshipEvent` |
| Break up / dissolve | `DissolvePartnership` |
| Form family | `FormFamilyUnit` |
| Add dependent to family | `AddChildToFamily` |
| Compatibility preview (read) | `CalculateCompatibility` |

## BM2. Fail-closed
`TryInitiateAttraction` / `ConductCourtshipEvent` return `bool` — UI must refresh from GetRelationship on false; no optimistic local stage flags.

## BM3. Day advance
Romance day owner already registered as `romance_family` phase 5 beside `survivor_routines`. Do not double-call `AdvanceDay` from the panel.

## BM4. Acceptance rg
```bash
rg -n 'TryInitiateAttraction|ConductCourtshipEvent|DissolvePartnership|FormFamilyUnit' src/UI -g '*.cs'
rg -n 'BeginCourtship|AdvanceRomance|BreakRelationship' src/UI -g '*.cs'
# second command should return no gameplay bindings (aliases to live verbs OK if they immediately delegate)
```

---

# Appendix BO — Save pin & section quick card (R3)

| Item | Value |
|---|---|
| Pin baseline | **266** (`Assert.Equal(266, SaveSectionRegistry.All.Count)`) |
| W9 section | `survivor_routines` / `survivor_routines_save.json` — **already counted** |
| W1 section | `romance_family` — reuse |
| W2 section | likely **new** → bump pin in same PR as registry + corruption test |
| W8 section | only if DEC chooses wire |
| Facades | Shelter-operations-style boards save nothing of their own |

---

# Appendix BP — R3 quality bar (replaces padding policy)

1. Every new appendix row must cite a path, type, save key, phase, method, or command.
2. Meta commentary about character budgets is limited to the revision banner + this rule.
3. If length must grow, extend forensic cards or runbooks — never empty attestation slots.
4. Stale verb names discovered live **block** DoD until the plan or code side is reconciled (Rule 7).

---

# Appendix BQ — Manual script polarity checks (W9)

1. Assign poor-sleep pair; advance day; note Fatigue before/after port (expect Fatigue **up** if SleepSatisfaction low).
2. Force high OverallSatisfaction path (ideal hours); expect Morale **down** (better) within clamp.
3. Save/load; confirm needs values restored from needs save authority (not from routines section).
4. Confirm routines section still holds satisfaction history records.

---

# Appendix BR — Cross-wave API drift watchlist

Re-rg these before each wave claim — names drift more than save keys:

| Wave | Re-verify |
|---|---|
| W1 | Romance host verbs (R3 baseline above) |
| W2 | `StartCampaign` / `DiscoverCandidate` / `MakeDefectionOffer` / `TickDay(day)` (R4-verified; no `TickCampaigns`/`AcceptCandidate`) |
| W3 | `InstallModule` / garage condition accessors |
| W4 | `TradeRouteHostSession` verbs (`EstablishContract`/`SuspendContract`/`ResumeContract`/`RecordRunOutcome`) + Colony `EstablishColony`/`TransferSupplies` (R4-verified names) |
| W5 | `StartCooking`/`ProgressCooking`/`CancelCooking`; disaster `ActivateProtocol`/`DeactivateProtocol`/`TickDisaster`; governance write owner post-DEC |
| W6 | Legacy prepare path; ideology resolve; NpcMemory forgive |
| W7 | `SelectPreset`/`SetCustomScalar`/`LockSettings`; maintenance `PerformMaintenance`/warning-failure projections |
| W8 | Chronic API only after DEC |
| W9 | Routines host verbs + DetectConflicts uniqueness harden + Needs mutators/polarity |
| W10 | SleepAcoustic SetQuietHoursSchedule sync vs ShelterNoise.SetQuietHours; Needs ports from SleepQualityResult; sensory kits |

---


# Appendix P — Deep exemplar W9 Survivor Routines (implementation-grade)

## P.Objective
Make Plan 188 player-operable and couple satisfaction to existing Needs/morale owners.

## P.Reality (forensic card)
- **Core path:** `Assets/Ashfall.Core/Survivors/SurvivorRoutineSystem.cs`
- **Host path:** `src/Host/SurvivorRoutineHostSession.cs`
- **Main path:** `src/Main.SurvivorRoutines.cs`
- **Save key / file:** `survivor_routines` / `survivor_routines_save.json`
- **Catalog:** `routine_templates.json` schema_version 1; four templates
- **Day phase:** 5 via `SurvivorRoutinesDayOwner`
- **CLI:** `SurvivorRoutinesSelfTest` checks 1–12 (init, templates, assign, preference, satisfaction ideal/poor, conflict detect/resolve, capture/restore)
- **UI:** `RoutineProvider` on `SurvivorDetailPanel` only
- **Broken link:** PLAYER-OPERABLE (+ satisfaction port MECHANIC)

## P.Delta
UI commands + manifest route + satisfaction port adapter + journal facts.

## P.Collisions
| Other system | Relationship |
|---|---|
| `ShelterAssignmentSystem` | Read room/workspace for DetectConflicts |
| Needs / fatigue / hunger / morale | Write targets for ports |
| `InterpersonalConflictSystem` | Separate ledger (social fights ≠ schedule clashes) |
| Romance W1 | Social hours synergy only |
| Holdfast legacy assignment | Main already documents ShelterAssignmentSystem as canonical |

## P.Flow — Assign routine
1. Player selects survivor + template from catalog dropdown (ids from `GetAllTemplates()`).
2. UI → `Main.AssignSurvivorRoutine(id, templateId)`.
3. Host `AssignRoutine` copies wake/sleep/meals/blocks from template.
4. `OnRoutineAssigned` → journal.
5. Panel refreshes schedule strip via `GetActivityAtHour` for hours 0–23.
6. Dirty flag → save section on flush.

## P.Flow — Conflict resolve
1. Day tick builds room/workspace maps; `DetectConflicts` may add `sleep_disturbance` or `workspace_overlap`.
2. Board lists `GetActiveConflicts()`.
3. Player Resolve → `ResolveConflict` sets `IsResolved`; census ActiveConflicts drops.
4. Journal fact; optional morale relief port (only if P1 scalar table includes resolve bonus).

## P.Flow — Satisfaction port
1. `EvaluateDailySatisfaction` computes Sleep/Meal/Work/Social/Overall.
2. Adapter reads record; applies clamped deltas to Needs/morale **once per (survivorId, day)**.
3. UI Needs readouts change; routines section still stores satisfaction history as today.

## P.Invariants
1. One schedule authority: `SurvivorRoutineSystem`.
2. One assignment authority for rooms/workspaces: `ShelterAssignmentSystem`.
3. One wellbeing authority family: existing Needs/morale (ports only).
4. Save section already registered — no pin bump for W9.
5. Templates authored in JSON — UI cannot invent template ids.
6. Deterministic tick — no `System.Random`.

## P.Determinism
Satisfaction inputs currently approximate from needs thresholds inside Main. Any refinement must use campaign day state + seeded forks if randomness is introduced (prefer none). Port adapter pure function of satisfaction record + prior applied-day set.

## P.Failures
See W9 failure table; additionally: catalog load failure leaves host event string; board shows empty templates and disables Assign.

## P.First commit slice
1. WORKTREE claim
2. Main wrappers + snapshot DTO (no UI yet)
3. Port adapter behind feature-local method with unit tests
4. Then UI battery

## P.Scalar table (R3 — live NeedsSystem contract)
**Authority:** `Assets/Ashfall.Core/Survivors/NeedsSystem.cs`
**Mutators:** `Modify(survivorId, NeedKind, delta)` · `ApplyAttributedDelta(survivorId, NeedKind, …)` · optional `SetExternalModifier` / `RemoveExternalModifier`
**Polarity (critical):** Hunger / Thirst / Fatigue / **Morale** / Numbness / RadiationAnxiety are **0..100 where HIGHER = WORSE**. Warmth/Health are lower-worse.
**Exemplar port pattern:** `MoraleContagionPorts.ApplyMoraleDelta` already documents canonical morale writes as higher=worse.

| Satisfaction field | NeedKind | Draft delta sense (HIGHER morale = worse) | Clamp / guard |
|---|---|---|---|
| SleepSatisfaction | `Fatigue` | `<50` → positive Fatigue delta (more tired pressure); `>80` → small negative Fatigue delta (relief) | once per `(survivorId, day)` |
| MealSatisfaction | — | **No hunger write by default** (kitchen/rations already charge Hunger) | observational only unless P1 proves a gap |
| WorkSatisfaction | `Morale` | `<40` → small **positive** Morale delta (worsens); `>80` → small negative Morale delta (improves) | ±N/day cap |
| SocialSatisfaction | `Morale` | poor social fit → positive Morale delta; good fit → negative | ±N/day cap |
| OverallSatisfaction | `Morale` | `<35` → additional positive Morale stress tick; `>85` → small relief | idempotent; do not stack with component writes beyond documented split |

**Day-phase ordering:** `survivors_needs` is phase **3**; `survivor_routines` is phase **5**. Ports running inside the routines tick therefore apply **after** the main needs tick for that day — prefer `ApplyAttributedDelta` / attributed source id `routine_satisfaction` so later diagnostics can see the source.

**Forbidden:** inventing `RoutineWellbeingSystem`; writing Morale as if higher were better; double-charging Hunger from meal satisfaction while cooking/rations already ran.

## P.Template reference (catalog ids)
| template_id | Role |
|---|---|
| `routine_early_riser` | Early wake/sleep; morning chronotype fit |
| `routine_night_owl` | Late schedule |
| `routine_standard` | Default 9–17 work block |
| `routine_night_shift` | Inverted sleep (selftest uses for sleep_disturbance with early riser) |

## P.Enforcement levels
| Level | Player meaning |
|---|---|
| `strict` | Shelter expects adherence; UI copy warns conflicts matter more |
| `flexible` | Default |
| `none` | Informational schedules only |

Core stores the string; UI must not invent other enums without Core change.

## P.Board wireframe (textual)
```
SURVIVOR ROUTINES
Enforcement: [flexible ▾]     Census: routines=N prefs=N conflicts=A/R
─────────────────────────────────────────────────────────
Roster          Template              Chronotype   Shift
Alex            routine_standard ▾    intermediate morning ▾
Blair           routine_night_owl ▾   night_owl    night ▾
─────────────────────────────────────────────────────────
Schedule strip (Alex): 0-6 Sleep | 7-8 Meal | 9-17 Work | ...
─────────────────────────────────────────────────────────
ACTIVE CONFLICTS
conf_12  Alex↔Blair  sleep_disturbance  major  [Resolve]
─────────────────────────────────────────────────────────
Satisfaction (today): Sleep 72  Meal 88  Work 64  Social 51  Overall 69
```

## P.Acceptance commands
```bash
# CLI package evidence
# (invoke via existing HostCli entry for SurvivorRoutinesSelfTest)
rg -n 'AssignRoutine|ResolveConflict' src/UI -g '*.cs'
rg -n 'ApplyRoutineSatisfactionPorts|OverallSatisfaction' src -g '*.cs'
bash scripts/run_test.sh Ashfall.Core.Tests/<RoutinePortTestFile>.cs
```

## P.Handoff blurb
Outcome: routines board player-operable + satisfaction ports. Files: UI panel/Detail, Main wrappers, Needs port touch, manifest. Contract: existing host APIs. Verify: selftest 12/12 + port tests + manual script. Untouched: SaveSectionRegistry pin, stealth, recruitment, chronic.

---


# Appendix O — R2 Accuracy & Polish Changelog

> **R3 addendum:** padding slots removed; Romance verbs + NeedsSystem port polarity corrected — see Appendix BI.

## O1. Purpose
This revision (R2) raises the plan from ~105k to ~200k characters by (a) correcting evidence that drifted or was overstated in prior gap notes, (b) expanding Wave 9 as a new player-facing gap **and** mechanic seal, and (c) deepening integrator-grade runbooks, conflict analyses, acceptance matrices, and player scripts without inventing false Core APIs.

## O2. Corrections applied
1. **Program width:** eight → nine waves; success definition gains routines operable item #15.
2. **Stealth fire noise:** marked already bridged on `CombatHostSession.ActionFire`; removed from any implied PFGL backlog.
3. **Plan 188 Core type:** named `SurvivorRoutineSystem` explicitly; queue notes that said “no dedicated Core type” are historical and superseded for this program.
4. **Reachability table:** added `SurvivorRoutineSystem` row (host-complete / UI thin).
5. **Save strategy:** W9 reuses `survivor_routines` — pin 266 unchanged for this wave.
6. **Surface counts:** restated from live `docs/player_surface_manifest.json` (219 / 58 / 161).
7. **Satisfaction mechanic:** documented as the intentional W9 gameplay expansion (ports into Needs/morale).
8. **False parallels list:** unchanged; routines is a true live owner, not a false parallel.
9. **Collision map:** routines + stealth rows added.
10. **Evidence index:** routines Core/host/save/catalog/CLI/day-owner/UI rows added.

## O3. Quality bar for expansion prose
Every added subsection must include at least one of: live path, live type name, save key, CLI action, acceptance command, failure mode, or ownership boundary. Pure motivational filler is rejected. Duplicate wave text is allowed only when it carries a new checklist, scalar table, or evidence citation.

## O4. What R2 deliberately did not change
- W1–W8 package ids and dependency order.
- DEC requirements for governance (W5) and chronic (W8).
- Non-goals forbidding WaterSourceSystem / FoodTypeSystem / FactionDiplomacySystem false parallels.
- Production code (still planning-only).

## O5. Re-verify commands (run before each wave claim)
```bash
git rev-parse HEAD
rg -n "Assert.Equal\(26[0-9]" Ashfall.Core.Tests/Save -g '*.cs'
rg -l 'SurvivorRoutineSystem' src --glob '*.cs' | wc -l
rg -n 'ApplyWeaponNoise' src/Host/CombatHostSession.cs
python3 -c "import json;m=json.load(open('docs/player_surface_manifest.json'));print(m['totalSurfaces'],m['interactiveSurfaces'],m['readOnlySurfaces'])"
```

## O6. Character-budget accounting (target ~200k)
| Block | Role |
|---|---|
| §§0–10 program contract | Architecture + evidence |
| W1–W9 packages | Retained integration contracts |
| W9 package | New gap + mechanic |
| Program-level strategies | Save/RNG/test/risk/DoD |
| Appendices A–N | Prior exemplars/runbooks |
| Appendices O–Z | R2 polish, W9 depth, matrices, FAQs, scripts |

---


# Appendix G — Deep exemplar W1 Romance (implementation-grade)

## Objective
Enable court/advance/break/form-family through ordinary UI; `RomanceFamilySystem` remains sole authority.

## Reality
Core defines relationships, family units, courtship catalog, scores, soulmate, cohabitation. Host exposes `FormFamilyUnit`. Detail panel shows provider text only.

## Delta
Action controls calling existing host methods; optional routed board only if needed. Persist via `romance_family`.

## Collisions
Relations affinity is baseline; GenerationalSystem owns children; GenealogyBridge may already call FormFamilyUnit — UI must use the same Core method.

## Flow — Form family
UI → `Main.TryFormFamily` → Ensure session → host `FormFamilyUnit` → Core validates → journal `romance_family_formed:*` → refresh provider → dirty save.

## Invariants
Unordered pair keys; score 0..100; living survivors; idempotent reject if already family.

## Determinism
Courtship auto picks use Social stream fork(day, pairHash).

## Failures
Dead survivor reject; low affinity reject; empty name reject/default via Core; mid-courtship save restores scores.

## First commit slice
1. Main wrappers + journal
2. Two SurvivorDetail buttons when provider non-null
3. Focused test + selftest
4. Stop — no new route yet


# R4 REVISION BLOCK — PRECISION + W10 SCOPE EXPANSION (2026-09-25)

This block is the R4 polish, accuracy, and scope-expansion pass over R3. It does not reopen sealed UNBLOCK host wires. It corrects implementer-blocking API fiction, records a fresh reachability audit, and adds wave **PFGL-W10-SLEEP-ACOUSTIC-RECOVERY**.

## R4-0. Fresh gap audit (live evidence)

### R4-0.1 Method
- `rg -l '\bType\b' src --glob '*.cs'` host_files counts for high-value authorities.
- `docs/player_surface_manifest.json`: **219** surfaces · **58** InteractiveCommands · **161** ReadOnlyObservational.
- Save pin: `Assert.Equal(266, SaveSectionRegistry.All.Count)` still live.
- HEAD: `1678c0749f49b5e4f9a99231e8e71acf3e47fdb1`.
- CLI/selftest PASS and plan-integration INTEGRATED are **not** treated as player-operable.

### R4-0.2 True orphans (host_files = 0)
| Authority | Core path | Notes |
|---|---|---|
| `RecruitmentSystem` | `Assets/Ashfall.Core/Survivors/RecruitmentSystem.cs` | PFGL **W2**. Prisoner recruit UI is a different seam. |
| `ChronicConditionSystem` | `Assets/Ashfall.Core/Medical/ChronicConditionSystem.cs` | PFGL **W8** (DEC before wire). |
| `EmergencyAlertSystem` | `Assets/Ashfall.Core/Emergency/EmergencyAlertSystem.cs` | Deferred/parallel; do not revive under W5 without DEC. |
| `GenealogyBridge` | `Assets/Ashfall.Core/Survivors/GenealogyBridge.cs` | Event bridge over `GenerationalLineageExtension`; custody DEC before host wire. |
| `RadiationEconomyBridge` | `Assets/Ashfall.Core/Radiation/RadiationEconomyBridge.cs` | Trade contamination evaluator; wire only onto live trade evaluation seams. |

### R4-0.3 Missing Core types (plan titles without live class)
| Plan-facing name | Live substitute / status |
|---|---|
| `InformalBarterSystem` | Live interactive `shelter_barter` / `ShelterBarterSystem` |
| `CrisisRationingSystem` | Closest: `ResourceRationingSystem` (thin host) |
| `PhobiaSystem` | Psychology profiles host covers phobia fields |
| `InformationFlowSystem` | Live `RumorSystem` under InformationFlow |

### R4-0.4 Dominant player-facing defect class
**Host-complete / player-thin** remains dominant after 2026-09-24/25 UNBLOCK seals: tick + save + CLI exist, but InteractiveCommands boards are missing or bound to a different owner.

High-impact thin set already claimed by W1–W9: Romance, Recruitment (orphan), Vehicle modules, Trade/Colony, Governance/Cook/Disaster, Legacy/Ideology/NPC, Difficulty/Maintenance/Keepsakes, Chronic (DEC), Routines.

### R4-0.5 Beyond W1–W9 candidates ranked
| Rank | Authority | host shape | Player surface | Why ranked |
|---|---|---|---|---|
| 1 | `SleepAcousticLedger` + `SleepAcousticRestEngine` | Main+HostSession+CLI+day+save | **none** (quiet hours already on `shelter_atmosphere` via `ShelterNoiseSystem`) | Sleep quality results are **computed then discarded**; Needs ports missing — silent mechanic failure |
| 2 | `ExerciseSystem` | Main+Host+CLI+day+save | **none** | `GetFatigueResistanceMultiplier` only probed in CLI; not composed into DutyRoster/NeedsPerformance |
| 3 | `DreamSystem` | host-rich | none | `InterpretSurvivorDream` unused by UI |
| 4 | `MemoryDecaySystem` | host-rich | none (`phantom_memory` ≠ this) | Reinforce verb live, no desk |
| 5 | `InterpersonalConflictSystem` | host-rich | mediates via **other** owner (`SurvivorRelationsSystem`) | Dual-ledger DEC required before board |
| 6 | `ClinicalWardTriageEngine` | host | `medical_ward` binds MedicalWard | dual-authority trap |
| 7 | `DependencyTaperWithdrawalEngine` | host | `chemical_dependency` binds ChemicalDependency | dual-authority trap |
| 8 | `CartographySystem` | thin projection | observational maps | lower play-loop impact |

**R4 selects #1 as W10** because it seals a proven silent failure (discarded `SleepQualityResult`) and a missing recovery mechanic while reusing the live quiet-hours write path on `ShelterNoiseSystem`.

### R4-0.6 Note on AGENTS.md C3 HOLD text
Live ledger/`KNOWN_DEBT.md` show C3 HOLDs 174/175/192/199 **lifted & sealed** (DEC-313/314/318/319). Treat AGENTS.md “still decision-blocked” lines for those four as **stale instruction residue** until the rulebook sync regenerates. W4 trade-route work is no longer blocked by C3-192.

---

## R4-1. Precision correction ledger (claim → live truth)

| ID | Severity | Stale claim (R3 or earlier) | Live truth | Plan action |
|---|---|---|---|---|
| C1 | **P0** | W2: `CancelCampaign` / `TickCampaigns(day,rng)` / `AcceptCandidate` | Live: `DiscoverCandidate`, `StartCampaign`, `MakeDefectionOffer`, `TickDay(int)`, Capture/Restore. Threshold success (`SuccessChance >= 40`), no RNG. | Rewrite W2 contracts to live verbs; roster bridge on recruited flag |
| C2 | **P0** | W4 Colony `Abandon` | No Abandon/Disband. Use `EstablishColony`, `TransferSupplies`, `AssignSurvivorToColony`, `SetSupplyLineStatus`. | Remove Abandon from DoD/mockups; wrap thin host |
| C3 | **P0** | DetectConflicts duplicate = optional forensic | **Proven:** always appends new conflict ids; day owner reticks. | W9 required uniqueness harden on `(ConflictType, SurvivorA, SurvivorB, Day)` or unresolved pair |
| C4 | **P1** | Recruitment “Capture/Census gaps” | `CaptureState` exists; Census API missing | Add Census if CI architecture scanner requires; else document ActiveCampaignCount getters |
| C5 | **P1** | Needs Morale higher=worse universally applied | XML + contagion ports agree; `ApplyCriticalNeedConsequences` does `Modify(Morale, -loss)` under critical needs (sign inconsistent with higher=worse) | W9/W10 ports use documented polarity; file separate Needs polarity bug if touching critical path |
| C6 | **P2** | W9 Core `Reset` | Reset is host-only on `SurvivorRoutineHostSession` | Say host Reset |
| C7 | **OK** | Romance verbs TryInitiateAttraction / ConductCourtshipEvent / DissolvePartnership / FormFamilyUnit | Confirmed on host+Core | Keep |
| C8 | **OK** | Needs mutators Modify / ApplyAttributedDelta; survivors_needs phase 3 before survivor_routines phase 5 | Confirmed | Keep |
| C9 | **OK** | Kitchen UI ≠ CookingSystem.StartCooking | KitchenNutritionPanel binds KitchenNutritionHostSession | W5 still must bind StartCooking |
| C10 | **OK** | VehicleGarage InstallModification ≠ VehicleCustomization InstallModule | Confirmed | W3 keep seam split |
| C11 | **OK** | Save pin 266 / HEAD 1678c074 | Confirmed | Keep |
| C12 | **R4 NEW** | SleepAcoustic “extends NeedsSystem” comments | Comments claim extension; `AdvanceDay` evaluates then **discards** `SleepQualityResult`; no `NeedsSystem.Modify` call sites in sleep-acoustic host/Core | W10 seals the port |

---

## R4-2. Program table updates

| Wave | Package | Delta vs R3 |
|---|---|---|
| W1–W8 | unchanged intent | API precision only where listed in R4-1 |
| W9 | routines board | DetectConflicts uniqueness harden **required** |
| **W10** | **NEW** sleep acoustic recovery | Board + Needs ports + quiet-hours sync |

Dependency sketch:

```
W1 Romance ─────────────────────────────────────┐
W2 Recruitment (orphan wire) ───────────────────┤
W3 Vehicle modules ─────────────────────────────┤
W4 Trade + Colony boards ───────────────────────┼─→ player-operable midgame loop set
W5 Governance DEC + Cook + Disaster ────────────┤
W6 Legacy / Ideology / NPC ─────────────────────┤
W7 Difficulty panel + Maintenance + Keepsakes ──┤
W8 Chronic DEC ─────────────────────────────────┤
W9 Routines + conflict harden + satisfaction ports ┤
W10 Sleep recovery ports + dormitory board ─────┘  (soft after W9 sleep hours / assignment stability)
```

W10 has **no hard DEC** (unlike W5 governance / W8 chronic). Soft synergy: W9 chronotype/sleep hours and shelter room assignments feed quarter occupancy; `shelter_atmosphere` already toggles quiet hours.

---

# PFGL-W10-SLEEP-ACOUSTIC-RECOVERY — Wave 10 — Dormitory Sleep Quality Recovery Seal

**Package id:** `PFGL-W10-SLEEP-ACOUSTIC-RECOVERY`
**Kind:** EXPAND existing host + MECHANIC seal (Needs ports) + thin UI expand
**Core calc owner:** `SleepAcousticRestEngine` (`Assets/Ashfall.Core/Needs/SleepAcousticRestEngine.cs`)
**Core state owner:** `SleepAcousticLedger` / `SleepAcousticState` (`Assets/Ashfall.Core/Needs/SleepAcousticLedger.cs`)
**Host:** `src/Host/SleepAcousticRestHostSession.cs` · `src/Main.SleepAcousticRest.cs` · day owner `sleep_acoustic_rest` phase 5
**Save:** section `sleep_acoustic_rest` / `sleep_acoustic_rest_save.json` — **already in pin 266** (no bump)
**Adjacent write owner for quiet hours:** `ShelterNoiseSystem` via `ShelterAtmosphereHostSession` / Interactive `shelter_atmosphere` panel
**CLI:** `--sleep-acoustic-selftest` (alias `--the-quiet-selftest`; live flag verified R5 — earlier `--sleep-acoustic-rest-selftest` name was fiction)

## W10.1 Objective

Make nightly sleep recovery **player-operable and mechanically real**:

1. Player can see per-quarter sleep quality band, net bunk decibels, crowding penalty, fatigue restoration multiplier, and morale restoration bonus.
2. Player can install/restock sensory-relief kits and update dormitory soundproofing on the **SleepAcoustic** ledger (permille scale).
3. Quiet hours schedule remains owned by **`ShelterNoiseSystem`** (already toggled on `ShelterAtmospherePanel`); SleepAcoustic **reads or syncs** that schedule instead of presenting a divergent second toggle as sole authority.
4. On day advance, evaluated `SleepQualityResult` values are **ported** into `NeedsSystem` Fatigue/Morale for assigned occupants (higher=worse polarity), using `Modify` or `ApplyAttributedDelta`.
5. Discarded-result silent failure in `SleepAcousticLedger.AdvanceDay` is closed.

## W10.2 Current Reality (evidence)

### Host-complete today
- Setup/save/reset/census/day owner/CLI exist.
- Public Main verbs: `EvaluateSleepingQuarterSleep`, `InstallSensoryReliefKit`, `RestockSensoryReliefKits`, `UpdateSleepingQuarterSoundproofing`, `SetQuietHoursSchedule`, `AdvanceSleepAcousticRestDay`, `GetSleepAcousticCensus`.
- Engine APIs: `CalculateAcousticAttenuation`, `CalculateCrowdingDensity`, `EvaluateSleepQuality`, `ComputeNetFatigueRecovery`.

### Player-thin / silent gaps today
- **No** `src/UI/*SleepAcoustic*` panel; manifest has zero sleep-acoustic InteractiveCommands surface.
- `ShelterAtmospherePanel` already offers Quiet Hours toggle + workshop soundproofing against **`ShelterNoiseSystem`** (percent scale 0–100), **not** SleepAcoustic permille quarters / sensory kits / sleep-quality cards.
- `SleepAcousticLedger.AdvanceDay` loops quarters, calls `EvaluateQuarterSleep`, then **drops** the `SleepQualityResult` on the floor — only increments `TotalNightsSlept`.
- No `NeedsSystem.Modify` / `ApplyAttributedDelta` call sites under sleep-acoustic host/Core despite “extends NeedsSystem” comments.
- Dual quiet-hours fields: `ShelterNoiseSystem` (`QuietHoursActive/Start/End`) **and** `SleepAcousticState` (`QuietHoursActive/StartHour/EndHour`) can diverge if UI writes only one.

### Collision map (must resolve in P0)
| Concern | Owner A | Owner B | W10 rule |
|---|---|---|---|
| Quiet hours schedule | `ShelterNoiseSystem` (player-facing today) | `SleepAcousticState` | **A writes; B syncs/reads.** Board must not invent a third schedule. Prefer `SetQuietHours` on atmosphere host, then copy into SleepAcoustic before evaluate — or SleepAcoustic evaluate reads Noise schedule directly via host adapter. |
| Room soundproofing | Noise profiles (% wall/door) | SleepAcoustic quarters (‰ wall/door) | Keep both scales; define a **deterministic projection** Noise%→Sleep‰ when atmosphere soundproofs a dormitory room id, **or** only edit SleepAcoustic ‰ from the dormitory board and leave workshop Noise% on atmosphere. Document chosen projection in the claim. |
| Fatigue/Morale | `NeedsSystem` | SleepAcoustic multipliers | SleepAcoustic computes; **NeedsSystem stores**. Never store shadow need values in SleepAcousticState. |
| Occupancy | `ShelterAssignmentSystem` | `SleepingQuarterState.AssignedOccupants` | Refresh occupants from assignment before evaluate; do not invent a third roster. |
| Ambient noise input | `ShelterNoiseSystem.GetRoomNoise` | quarter `AmbientNoiseLevelDecibels` | Host adapter may project room noise → quarter ambient each tick; avoid stale authored defaults forever. |

## W10.3 Required Delta

| Gap | Delta |
|---|---|
| Silent discard of sleep results | Persist last-night results for UI **or** recompute deterministically on snapshot; **must** port Needs on tick |
| Missing Needs ports | After evaluate, for each assigned survivor: Fatigue delta from `ComputeNetFatigueRecovery`; Morale delta from `MoraleRestorationBonus` with higher=worse sign |
| Missing operable dormitory controls | Expand `ShelterAtmospherePanel` **or** add `sleep_acoustic_rest` InteractiveCommands board for kits + dormitory ‰ soundproofing + quality cards |
| Divergent quiet hours | Sync path Noise → SleepAcoustic (or read-through) on tick and on atmosphere toggle |
| No attribution | Prefer `ApplyAttributedDelta(..., attribute: "sleep_acoustic")` when available so journals/telemetry can explain the change |
| Census optimism (R5) | `GetCensus` re-evaluates bands with hardcoded `Compliant` + 8h — the board must pass live quiet-hours compliance into the readout or visibly label the assumption |

## W10.4 Non-goals
- New save section / pin bump.
- Reviving `EmergencyAlertSystem`.
- Replacing `ShelterNoiseSystem` or deleting atmosphere quiet-hours UI.
- Parallel need stores, sleep HP ledgers, or dream interpretation (DreamSystem remains a runner-up wave).
- Exercise fatigue-resistance composition (runner-up W11).
- Clinical ward / dependency taper UI under this package.
- Changing IdealSleepDecibels/MaxTolerable constants for balance before correctness.

## W10.5 Ownership Matrix

| Concern | Owner |
|---|---|
| Sleep quality math | `SleepAcousticRestEngine` (static pure) |
| Quarters, kits reserve, last evaluate inputs | `SleepAcousticLedger` |
| Quiet hours schedule write | `ShelterNoiseSystem` |
| Need values | `NeedsSystem` |
| Room assignment / occupants | `ShelterAssignmentSystem` |
| Presentation | Godot panel (expand atmosphere or new routed board) |
| Persistence of acoustic ledger | existing `sleep_acoustic_rest` section |
| Persistence of noise | existing `shelter_noise` section |

## W10.6 State / API contract

### Live ledger fields (reuse)
`SleepAcousticState`: Quarters[], SensoryReliefKitsReserve, QuietHoursActive, QuietHoursStartHour, QuietHoursEndHour, QuietHoursViolationsCount, SleepDisturbanceAlertCount, TotalNightsSlept, SchemaVersion.

`SleepingQuarterState`: RoomId, RoomAreaSquareMetres, AssignedOccupants, WallSoundproofingPermille, DoorSoundproofingPermille, AmbientNoiseLevelDecibels, DarknessQualityPermille, HasSensoryReliefKit.

`SleepQualityResult`: SleepQualityIndexPermille, EnvironmentBand, NetDecibelsAtBunk, CrowdingPenaltyPermille, FatigueRestorationMultiplierPermille, MoraleRestorationBonus.

### Proposed host mechanic (minimal)
```
On SleepAcousticRestDayOwner.TickDay / AdvanceDay:
  1. SetupSleepAcousticRest + SetupShelterAtmosphere (noise)
  2. SyncQuietHoursFromNoise()  // copy Noise schedule into ledger OR pass Noise flags into Evaluate
  3. RefreshQuarterOccupancyFromAssignments()
  4. Optionally project GetRoomNoise(room) → AmbientNoiseLevelDecibels
  5. For each quarter:
       result = EvaluateQuarterSleep(...)
       recovery = ComputeNetFatigueRecovery(baseRecovery, result.FatigueRestorationMultiplierPermille)
       for each occupant survivorId in quarter:
           Needs.Modify(id, Fatigue, -recovery)           // higher=worse → negative improves
           Needs.Modify(id, Morale, -result.MoraleRestorationBonus)  // positive bonus improves (lowers) morale value
           // If bonus negative (Disturbed/Unbearable), -(-2)=+2 worsens morale — correct under higher=worse
  6. Emit sleep_acoustic_rest_ticked with census
```

**Base recovery constant:** choose a single authored or host-constant base (e.g. 10–20 fatigue points per full night) documented in the claim; do not scatter magic numbers across UI.

**Determinism:** no `System.Random`; occupant iteration order must be stable (Ordinal sort of survivor ids).

### UI verbs to expose (bind existing Main methods)
- `InstallSensoryReliefKit(roomId)`
- `RestockSensoryReliefKits(n)` (inventory bill if an item id exists; otherwise reserve-only with honesty label)
- `UpdateSleepingQuarterSoundproofing(roomId, wall‰, door‰)`
- Readouts: census + last/per-quarter `SleepQualityResult`
- Quiet hours: call atmosphere/`ShelterNoiseSystem.SetQuietHours` then sync — **do not** only call `SetQuietHoursSchedule` on SleepAcoustic from a second unlabeled toggle

## W10.7 Data flow

```
Player (atmosphere / dormitory board)
  → SetQuietHours / Soundproof / InstallSensoryReliefKit / Restock
  → ShelterNoiseSystem and/or SleepAcousticLedger (per collision rules)
  → Day tick SleepAcousticRestDayOwner
  → EvaluateSleepQuality + ComputeNetFatigueRecovery
  → NeedsSystem.Modify / ApplyAttributedDelta (Fatigue, Morale)
  → SurvivorDetail / needs HUD refresh
  → Save: sleep_acoustic_rest + needs section + shelter_noise (existing)
```

## W10.8 Phased implementation

### Phase 0 — Premise lock
- Re-rg live verbs; confirm CLI flag name; confirm pin 266; claim paths in `WORKTREE_OWNERSHIP.md` under `PFGL-W10-SLEEP-ACOUSTIC-RECOVERY`.
- Write collision decision note: quiet-hours write owner = ShelterNoise; sync strategy; soundproofing projection choice.
- Baseline: focused Exercise/Sleep tests + `--sleep-acoustic-selftest` current PASS count.
- Gate: written collision note + claim rows.

### Phase 1 — Core/host mechanic (Needs ports)
- Change `SleepAcousticLedger.AdvanceDay` **or** host day owner to apply ports (prefer host adapter if Core must stay free of NeedsSystem reference — **Rule check:** Core already says it extends NeedsSystem in comments; if Core must not reference NeedsSystem types, put ports in `Main.SleepAcousticRest` / host session with injected `Action`/`Needs` accessors).
- **Architectural preference:** keep `SleepAcousticRestEngine` pure; apply Needs mutations in host (`Main` / HostSession) to avoid Core→Needs circular ownership if not already coupled.
- Add deterministic occupant enumeration helper.
- Gate: unit/host test proving Fatigue decreases after Restful night and increases path for Unbearable (via worse multiplier / negative morale bonus).

### Phase 2 — Quiet hours sync
- Implement `SyncQuietHoursFromNoise` called from day owner and from atmosphere toggle path (event or explicit Main glue).
- Test: toggle atmosphere quiet hours → next sleep evaluate sees matching compliance inputs.
- Gate: focused test for sync.

### Phase 3 — UI expand
- Prefer expanding `ShelterAtmospherePanel` with a “Dormitory Sleep” section bound to `SleepAcousticRestHostSession` **or** register `sleep_acoustic_rest` route + panel if shared panel claim contention requires isolation.
- Show quality band, ‰ multipliers, kits reserve, install/restock/soundproof dormitory controls.
- Update `docs/player_surface_manifest.json` / PlayerSurfaces if new route; if expanding atmosphere, bump action coverage honesty if needed.
- Gate: `--player-panels-uitest` subset or panel bind smoke; headless instantiate.

### Phase 4 — Verification
- `bash scripts/run_test.sh` on new/focused sleep acoustic + needs port tests only.
- Godot headless selftest for sleep acoustic.
- Confirm SaveSectionRegistry pin still 266.
- Manual script: enforce quiet hours, install kit, advance day, observe Fatigue/Morale via survivor needs snapshot.

## W10.9 Failure modes

| Case | Expected |
|---|---|
| No quarters | EnsureDefaultQuarter; still tick safely |
| No occupants | Evaluate for census; skip Needs ports |
| Missing NeedsSystem | Fail visible in selftest; do not swallow |
| Kits reserve 0 | InstallSensoryReliefKit returns false; UI shows reason |
| Divergent quiet hours before sync | Sync at start of tick; selftest asserts equality after sync helper |
| Old saves | Default quarter + existing schema_version; no migration unless fields added |
| Double-port on retick | Day owner once-per-day; pre-day snapshot restore remains valid |
| Polarity inversion | Tests lock Restful → Fatigue value down; Unbearable → Fatigue recovery weak / Morale value up |

## W10.10 Test strategy

| Layer | Cases |
|---|---|
| Engine unit | EvaluateSleepQuality bands; ComputeNetFatigueRecovery clamp; attenuation monotonic in soundproofing |
| Ledger | Install kit consumes reserve; AdvanceDay increments nights; **new:** ports invoked via injectable sink in test |
| Host | Sync quiet hours; day owner emits `sleep_acoustic_rest_ticked` |
| Needs integration | Round-trip Fatigue/Morale after simulated night |
| UI | Bind/open/visible; kit button disabled at 0 reserve |
| Regression | ShelterAtmosphere quiet toggle still works; shelter_noise save round-trip |

## W10.11 File impact map

| File/area | Action | Risk |
|---|---|---|
| `Assets/Ashfall.Core/Needs/SleepAcousticLedger.cs` | MODIFY AdvanceDay only if ports stay Core-local; else READ ONLY | MED |
| `Assets/Ashfall.Core/Needs/SleepAcousticRestEngine.cs` | READ ONLY unless tiny helper | LOW |
| `src/Host/SleepAcousticRestHostSession.cs` | MODIFY — port hooks, sync helper | MED |
| `src/Main.SleepAcousticRest.cs` | MODIFY — Needs accessors glue | MED |
| `src/Main.CampaignOwners.cs` | MODIFY SleepAcousticRestDayOwner tick | MED |
| `src/Host/ShelterAtmosphereHostSession.cs` / `src/UI/ShelterAtmospherePanel.cs` | MODIFY — sync on toggle; optional dormitory section | MED |
| `src/UI/SleepAcousticPanel.cs` (+ tscn) | CREATE optional if not expanding atmosphere | MED |
| `src/Main.PlayerSurfaces.cs` / PanelRegistryBootstrap / GameFlow | MODIFY only if new route | HIGH (shared) |
| `Ashfall.Core.Tests/Needs/*SleepAcoustic*` | CREATE/MODIFY focused tests | LOW |
| `WORKTREE_OWNERSHIP.md` | MODIFY claim rows | LOW |
| `SaveSectionRegistry.cs` | READ ONLY (no new section) | — |

Shared hubs (`PanelRegistryBootstrap`, `OpenPlayerPanel`, `Main.UiPanels`, `Main.PlayerSurfaces`, `PlayerSurfaceManifest`) require integrator ownership if a **new** route is chosen — same contention class as `DEBT-PLAN181`. Expanding `ShelterAtmospherePanel` avoids that contention and is the **recommended** default.

## W10.12 Risks
- Dual soundproofing scales confuse players — mitigate with UI labels (“Noise insulation %” vs “Bunk lining ‰”) and one projection rule.
- Needs polarity bugs (C5) — do not “fix” critical-path morale sign inside W10; only use documented higher=worse for sleep ports.
- Over-coupling DreamSystem — out of scope.
- Shared panel hub contention — prefer atmosphere expand.

## W10.13 Rollback
- Mechanic ports behind a host feature flag or revert day-owner diff first.
- UI section revert leaves Core/host math intact.
- No save migration to roll back.

## W10.14 Definition of Done
- [ ] Quiet hours: atmosphere toggle and sleep evaluate agree after sync helper.
- [ ] AdvanceDay/day owner ports Fatigue+Morale with tested polarity.
- [ ] Player can install sensory kit and see quality band without CLI.
- [ ] No new save section; pin 266 holds.
- [ ] Focused tests + sleep-acoustic selftest PASS.
- [ ] Collision note filed in claim / handoff.
- [ ] No EmergencyAlert / ClinicalWard / Exercise scope creep.

## W10.15 Implementation handoff

### MUST PRESERVE
- `ShelterNoiseSystem` as quiet-hours write owner for the existing atmosphere panel.
- Existing `sleep_acoustic_rest` and `shelter_noise` save sections.
- Pure engine math; determinism; NeedsSystem as sole need store.

### MUST ADD
- Needs ports from `SleepQualityResult`.
- Quiet-hours sync/read-through.
- Player-visible dormitory sleep quality + kit controls (atmosphere expand preferred).

### MUST NOT DO
- Second conflicting quiet-hours-only panel that writes SleepAcoustic alone.
- New save pin.
- Parallel fatigue shadow state.
- Greenfield dream/exercise boards in the same claim.

### VERIFY WITH
- Focused xUnit for ports + sync.
- `godot --headless --path . -- --sleep-acoustic-selftest` (verified flag name).
- Panel bind smoke / player-panels subset if UI touched.
- `rg` proving Needs Modify/ApplyAttributedDelta call sites exist on the sleep tick path.

### FIRST SAFE IMPLEMENTATION STEP
P0 premise + WORKTREE claim `PFGL-W10-SLEEP-ACOUSTIC-RECOVERY` + written quiet-hours collision note; then host-side Needs port with tests before any UI.

---

## R4-3. Cross-wave precision runbook (implementer)

Before claiming any wave, re-verify:

```bash
git rev-parse HEAD
rg -n "Assert.Equal\(266, SaveSectionRegistry.All.Count\)" Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs
rg -n "public bool TryInitiateAttraction|ConductCourtshipEvent|DissolvePartnership|FormFamilyUnit" src/Host/RomanceFamilyHostSession.cs
rg -n "public .*StartCampaign|MakeDefectionOffer|TickDay|DiscoverCandidate|CancelCampaign|AcceptCandidate" Assets/Ashfall.Core/Survivors/RecruitmentSystem.cs
rg -n "EstablishColony|Abandon|TransferSupplies|AssignSurvivorToColony" Assets/Ashfall.Core/Expeditions/ColonySystem.cs
rg -n "DetectConflicts|Conflicts.Add" Assets/Ashfall.Core/Survivors/SurvivorRoutineSystem.cs
rg -n "Modify\(|ApplyAttributedDelta" Assets/Ashfall.Core/Survivors/NeedsSystem.cs
rg -n "AdvanceDay|EvaluateQuarterSleep|SetQuietHours" Assets/Ashfall.Core/Needs/SleepAcousticLedger.cs src/Main.SleepAcousticRest.cs src/Host/ShelterAtmosphereHostSession.cs
```

Any stale verb in a wave DoD **blocks** that wave’s seal (Rule 7).

---

## R4-4. W2 Recruitment contract patch (authoritative override)

Replace any earlier W2 API lists with:

| Verb | Role |
|---|---|
| `LoadCatalog` / getters | Authored campaigns + candidates |
| `DiscoverCandidate(template, location, day, faction)` | Adds known candidate |
| `StartCampaign(typeId, faction, recruiterId, day)` | Returns `(Success, Message, Campaign?)`; respects MaxActiveCampaigns |
| `MakeDefectionOffer(...)` | Pending offer with discovery risk |
| `TickDay(day)` | Completes due campaigns by threshold; resolves offers; may set `IsRecruited` |
| `CaptureState` / `RestoreState` | Persistence DTO `RecruitmentState` |

Host must still: construct session, register save section (likely new → pin bump), day owner, CLI 12-check, journal seams, **roster intake bridge**, and player muster board. Adding `CancelCampaign` or seeded rolls is a **Core DEC**, not a silent host fake.

---

## R4-5. W4 Colony contract patch (authoritative override)

| Verb | Role |
|---|---|
| `EstablishColony(locationId, name, type, garrison, supplies, day)` | Found |
| `ConstructBuilding(colonyId, definitionId, day)` | Build |
| `EstablishSupplyLine(...)` / `SetSupplyLineStatus` | Logistics |
| `AssignSurvivorToColony` / `TransferSupplies` | Population & stock |
| `TickDay` | Daily sim |
| `CaptureState` / `RestoreState` | Save |

UI mockups showing **Abandon** must use `SetSupplyLineStatus` / deactivate `IsActive` only if such a Core path is added under DEC — not assumed present.

---

## R4-6. W9 DetectConflicts harden patch (authoritative override)

Required algorithm sketch:

```
before add sleep_disturbance/workspace conflict for (a,b,type,day):
  if unresolved exists for same unordered pair + type (+ optional day window): skip
  else add with new ConflictId
```

Also consider clearing resolved-or-stale conflicts when schedules change. Tests must retick DetectConflicts twice and assert conflict count does not grow unbounded.

---

## R4-7. W10 synergy with W9 routines

- W9 owns chronotype / sleep hour templates and schedule conflicts.
- W10 owns acoustic environment quality and Needs recovery magnitude.
- Shared input: who sleeps in which room (`ShelterAssignmentSystem`) and whether quiet hours are enforced (`ShelterNoiseSystem`).
- Do not merge ledgers; optional read-only: routines SleepHour vs quiet-hours window for compliance classification upgrades (Compliant vs ViolatedMajor) — phase this only after basic ports work.

---

## R4-8. Runner-up wave card (not in R4 DoD) — Exercise fatigue resistance

`ExerciseSystem` is host-complete with **zero** player surfaces. Mechanic gap: `GetFatigueResistanceMultiplier` is CLI-only. Future `PFGL-W11-EXERCISE-BOARD` should compose the multiplier into `DutyRosterSystem.WorkSpeedMultiplierLookup` and/or needs fatigue accrual rate providers without creating a second fitness need store. Listed so R4 scope stays honest.

---

## R4-9. Acceptance matrix addendum

| Wave | Player-operable proof | Pin impact |
|---|---|---|
| W1 | Romance board actions call live Romance verbs | reuse `romance_family` |
| W2 | Muster board starts campaign; tick recruits; roster gains survivor | likely +1 section |
| W3 | Garage/modules InstallModule path | reuse customization |
| W4 | Trade schedule + EstablishColony/Transfer from UI | reuse trade_routes/colony |
| W5 | Governance writes per DEC; StartCooking; disaster protocols | reuse |
| W6 | Legacy select; ideology resolve; NPC forgive/call-in | reuse |
| W7 | Difficulty panel; maintenance repair; keepsake orphan count target | reuse + content |
| W8 | DEC then wire/retire chronic | conditional +1 |
| W9 | Routines board + unique conflicts + satisfaction→Needs | reuse survivor_routines |
| W10 | Sleep quality visible; kits; Needs ports; quiet hours synced | reuse sleep_acoustic_rest |

---

## R4-10. Program DoD delta

R3 DoD plus:

- [ ] All R4-1 P0 rows corrected in wave contracts (this document).
- [ ] W9 uniqueness harden treated as required in implementation claims.
- [ ] W10 claimable as independent package with collision note.
- [ ] Character budget met with evidence-dense appendices (no empty attestation slots).

---

## R4-11. Forensic cards — SleepAcoustic silent failure

**Finding:** `SleepAcousticLedger.AdvanceDay` evaluates sleep quality per quarter then discards the result.
**Impact:** Players and systems never receive Fatigue/Morale recovery from authored acoustic math; Expansion 41 looks “integrated” by CLI while the survival loop is inert.
**Class:** Silent failure / unwired mechanic (host-complete, effect-thin).
**Seal:** W10 Phases 1–4.
**Evidence paths:** `Assets/Ashfall.Core/Needs/SleepAcousticLedger.cs` (`AdvanceDay`), `SleepAcousticRestEngine.ComputeNetFatigueRecovery`, absence of Needs mutator call sites on the sleep tick path, Interactive `shelter_atmosphere` vs missing sleep-acoustic surface.

---

## R4-12. Forensic cards — Dual quiet hours

**Finding:** `ShelterNoiseSystem.SetQuietHours` is player-reachable; `SleepAcousticLedger.SetQuietHoursSchedule` is a parallel schedule field.
**Impact:** Sleep evaluate can use stale compliance while atmosphere shows different quiet hours.
**Class:** Ownership ambiguity / divergent mutable state.
**Seal:** W10 Phase 2 sync/read-through; UI rule forbids SleepAcoustic-only schedule ownership.
**Evidence:** `src/UI/ShelterAtmospherePanel.cs` toggle; both state objects’ QuietHours* fields; save sections `shelter_noise` + `sleep_acoustic_rest`.

---

## R4-13. Forensic cards — Recruitment API fiction

**Finding:** Plan text referenced non-existent Cancel/Accept/TickCampaigns(rng).
**Impact:** Implementer would block on missing Core methods or invent illegal host behavior.
**Class:** Stale contract / plan drift.
**Seal:** R4-4 authoritative override; W2 P0 premise must rg live API.

---

## R4-14. Forensic cards — Colony Abandon fiction

**Finding:** Abandon listed without Core method.
**Seal:** R4-5 override; UI copy uses supply-line status / honesty labels.

---

## R4-15. Forensic cards — DetectConflicts spam

**Finding:** Proven unbounded append on retick.
**Seal:** W9 required harden (R4-6).

---

## R4-16. Extended ownership matrix (program + W10)

| Concern | Core owner | Host session | Save section | UI seam | Day phase |
|---|---|---|---|---|---|
| Romance/family | RomanceFamilySystem | RomanceFamilyHostSession | romance_family | W1 board/expand | existing |
| Recruitment | RecruitmentSystem | CREATE | likely recruitment | muster board | new owner |
| Vehicle modules | VehicleCustomizationSystem | VehicleCustomizationHostSession | vehicle_customization | garage expand | existing |
| Trade routes | PlayerTradeRouteSystem | TradeRouteHostSession | trade_routes | new/expand trade | existing |
| Colonies | ColonySystem | ColonyHostSession (thicken) | colony | colony board | existing |
| Governance | ShelterGovernanceEngine vs Politics | both live | shelter_governance / politics | DEC | existing |
| Cooking recipes | CookingSystem | CookingHostSession | cooking | kitchen bind StartCooking | existing |
| Disaster protocols | DisasterResponseSystem | existing | disaster_response | emergency HUD | existing |
| Routines | SurvivorRoutineSystem | SurvivorRoutineHostSession | survivor_routines | W9 board | phase 5 |
| Sleep recovery | SleepAcousticRestEngine+Ledger | SleepAcousticRestHostSession | sleep_acoustic_rest | atmosphere expand / W10 | phase 5 |
| Quiet hours schedule | ShelterNoiseSystem | ShelterAtmosphereHostSession | shelter_noise | shelter_atmosphere | atmosphere tick |
| Needs values | NeedsSystem | survivors needs host | needs / survivors_needs | HUD/detail | phase 3 |

---

## R4-17. Detailed W10 player script (acceptance narrative)

1. Open Shelter Atmosphere; confirm Quiet Hours state.
2. Open Dormitory Sleep section/board; note quality band for `primary_shelter_dormitory`.
3. Restock/install sensory-relief kit; soundproof dormitory walls/doors on ‰ controls.
4. Advance one campaign day.
5. Reopen survivor needs: Fatigue lower than pre-night baseline for Restful path; Morale improved under higher=worse.
6. Save/load; kits reserve, quarter ‰, needs values persist via their owners.
7. Lift quiet hours on atmosphere; advance day; sleep compliance path worsens recovery vs enforced night (deterministic compare under test harness).

---

## R4-18. Needs polarity quick card (W9 + W10)

| Need | Range | Worse direction | Sleep port sign |
|---|---|---|---|
| Fatigue | 0..100 | higher worse | apply `-recovery` to improve |
| Morale | 0..100 | higher worse | apply `-MoraleRestorationBonus` (bonus may be negative) |
| Hunger/Thirst/Numbness/RadiationAnxiety | 0..100 | higher worse | not W10 |
| Warmth/Health | 0..100 | lower worse | not W10 |

Mutators: `NeedsSystem.Modify`, `NeedsSystem.ApplyAttributedDelta`.
Phase ordering: needs phase 3, sleep_acoustic_rest phase 5 — ports run in phase 5 after needs accrual; document that sleep recovery applies post-accrual same day (acceptable) or schedule explicit post-phase hook if double-counting appears.

---

## R4-19. Claim template (W10)

```
claim-id: claim-pfgl-w10-sleep-acoustic-recovery-2026-09-25
package: PFGL-W10-SLEEP-ACOUSTIC-RECOVERY
owner: <integrator>
paths:
  - src/Host/SleepAcousticRestHostSession.cs
  - src/Main.SleepAcousticRest.cs
  - src/Main.CampaignOwners.cs (SleepAcousticRestDayOwner only)
  - src/UI/ShelterAtmospherePanel.cs (dormitory section) OR src/UI/SleepAcousticPanel.cs
  - Ashfall.Core.Tests/Needs/<new port tests>
non-goals: EmergencyAlert, Exercise board, Dream UI, new save section
collision-note: quiet-hours-write=ShelterNoiseSystem; SleepAcoustic syncs
```

---

## R4-20. Expanded gap catalogue (player-facing, evidence-ranked)

### Tier A — Program waves (do these)
W1 Romance board; W2 Recruitment orphan wire; W3 Vehicle modules; W4 Trade+Colony; W5 Governance DEC+Cook+Disaster; W6 Legacy/Ideology/NPC; W7 Difficulty+Maint+Keepsakes; W8 Chronic DEC; W9 Routines+conflicts; W10 Sleep recovery ports.

### Tier B — Next after program
Exercise board + fatigue resistance composition; Dream interpretation; Memory reinforce desk; InterpersonalConflict one-authority DEC vs SurvivorRelations; GenealogyBridge custody; RadiationEconomyBridge on trade evaluate.

### Tier C — Manifest honesty / thin polish
Night watch + personal belongings manifest ReadOnly while buttons work — classify as manifest lag, not missing loops. Cartography projection polish.

### Tier D — False parallels (do not wire as live owners)
WaterSourceSystem; FoodTypeSystem; FactionDiplomacySystem without DEC.

---

## R4-21. Save pin strategy (updated)

| Wave | Section | Pin |
|---|---|---|
| W1 | romance_family | no bump |
| W2 | recruitment (likely new) | +1 with registry+corruption test |
| W3–W7, W9–W10 | reuse | no bump |
| W8 | conditional chronic | +1 only if wire |

Baseline **266**.

---

## R4-22. Determinism rules (program reminder)

- No `System.Random` in Core gameplay.
- Recruitment live tick is threshold-based; if seeded rolls are desired later, take MasterSeed stream explicitly.
- Sleep ports iterate survivor ids with Ordinal ordered sequences.
- DetectConflicts uniqueness must not depend on dictionary iteration order.

---

## R4-23. UI extension cookbook (W10 preferred path)

1. Bind optional `SleepAcousticRestHostSession` on `ShelterAtmospherePanel` via Main accessor.
2. Add “Dormitory Sleep” collapsible: census labels + kit buttons + ‰ sliders.
3. Quiet hours button remains Noise-backed; after toggle call SleepAcoustic sync.
4. Do not compute sleep scores in the panel — call Main evaluate/census.
5. Accessibility: keyboard focus order, contrast, close/back unchanged.

---

## R4-24. Content / inventory note for sensory kits

If restock should consume inventory items, locate authored item ids before billing; if none exist, keep reserve-only counter with UI honesty (“kit reserve” not “crafted item”) or author a single schema-valid item in a follow-up content microclaim. Do not invent silent item spawns.

---

## R4-25. End-to-end verification commands (focused)

```bash
# Adjust test paths to the files created by the wave claim
bash scripts/run_test.sh Ashfall.Core.Tests/Needs/
godot --headless --path . -- --sleep-acoustic-selftest
godot --headless --path . -- --shelter-atmosphere-selftest  # if present
rg -n "ApplyAttributedDelta|NeedKind.Fatigue|NeedKind.Morale" src/Main.SleepAcousticRest.cs src/Host/SleepAcousticRestHostSession.cs src/Main.CampaignOwners.cs
```

Do not run full suite by default (`TEST_POLICY.md`).

---

## R4-26. Handoff summary for foreman

**Planning artifact only.** No production code changed by R4 authorship.
**Biggest gaps:** player-thin boards + three true orphans + SleepAcoustic silent Needs gap.
**R4 delivers:** corrected W2/W4/W9 contracts + new W10 sleep recovery package.
**First build package still:** `PFGL-W1-ROMANCE-BOARD` unless foreman reprioritizes orphan Recruitment or W10 silent-failure seal earlier (W10 is justified as reliability seal and may be scheduled ahead of cosmetic boards).

---

## R4-27. Scheduling recommendation

Two valid trains:

**Train Social-first (default R3):** W1 → W2 → W3 → W4 → W5 → W6 → W7 → W8 → W9 → W10

**Train Reliability-first (R4 option):** W10 (silent failure) → W9 (conflict spam) → W1 → W2 → …

Foreman picks train; builders keep packages disjoint in WORKTREE_OWNERSHIP.

---

## R4-28. Long-form W10 failure injection checklist

1. Null session → Setup guards.
2. Unknown roomId soundproof → no-op/false with LastEvent.
3. Negative restock count → clamp/reject.
4. Occupant id missing in NeedsSystem → skip with counter in selftest.
5. Extremely high ambient dB → band Unbearable; recovery multiplier floor.
6. Quiet hours overnight wrap (22→06) → Noise `IsHourWithinQuietHours` semantics remain authoritative for schedule; SleepAcoustic compliance enum remains evaluate input.
7. Save during day owner → pre-day snapshot restore still works.
8. UI unmounted → Core tick still ports.

---

## R4-29. Relationship to UNBLOCK Expansion 41

Expansion 41 sealed host reachability for SleepAcoustic (session/save/day/CLI). R4 W10 does **not** redo that seal. It closes the **player-facing + Needs effect** gap left after host integration — the exact defect class called out in post-UNBLOCK gap audits: host-complete / player-thin / effect-thin.

---

## R4-30. Document maintenance (R4)

- Revise on HEAD move across listed rewires, pin changes, or completed wave addenda.
- Keep session `plan.md` synced when this file is the active planning surface.
- Production code stays out of this document’s authorship.
- Empty attestation slots remain forbidden; grow via forensic cards and runbooks only.

---



---

## R4-31. Wave-by-wave live verb cards (compact)

### W1 RomanceFamilyHostSession
`CalculateCompatibility` · `TryInitiateAttraction` · `ConductCourtshipEvent` · `DissolvePartnership` · `FormFamilyUnit` · `AddChildToFamily` · `AdvanceDay` · Capture/Restore · Census.
Forbidden aliases: `BeginCourtship`, `AdvanceRomance`, `BreakRelationship`.

### W2 RecruitmentSystem
`DiscoverCandidate` · `StartCampaign` · `MakeDefectionOffer` · `TickDay(int day)` · `CaptureState`/`RestoreState` · count getters (`ActiveCampaignCount`, `KnownCandidateCount`, `TotalRecruitedCount`).
Forbidden fiction: `CancelCampaign`, `TickCampaigns(day,rng)`, `AcceptCandidate`. Roster intake is a **host bridge** when `IsRecruited` becomes true.

### W3 VehicleCustomizationHostSession
`InstallModule` / `RemoveModule` (+ camp helpers). Distinct from garage `InstallModification` / `InstallArmorGrade`.

### W4 Trade + Colony
Trade: live schedule/pause/resolve host verbs on `TradeRouteHostSession`.
Colony: `EstablishColony` · `ConstructBuilding` · `EstablishSupplyLine` · `SetSupplyLineStatus` · `AssignSurvivorToColony` · `TransferSupplies` · `TickDay` · Capture/Restore. No `Abandon`.

### W5 Governance / Cook / Disaster
Governance writes only after DEC choosing `ShelterGovernanceEngine` vs `PoliticsSystem`.
Cook: `CookingHostSession.StartCooking` (kitchen panel today binds `KitchenNutritionHostSession` — rebind or dual-surface honesty).
Disaster: `DisasterResponseSystem` protocols on emergency HUD. Never `EmergencyAlertSystem` without DEC.

### W6 Legacy / Ideology / NPC
`CampaignLegacySystem` / `MetaProgressionSystem` select path; `IdeologicalFrictionEvents` resolve/mediate; `NpcMemorySystem` forgive/call-in acts via live host verbs (re-rg on claim).

### W7 Difficulty / Maintenance / Keepsakes
Difficulty runtime panel (shared hub claim; debt DEBT-PLAN181); `ShelterMaintenanceSystem` repair/clear board; author/remap keepsake item orphans (65/76).

### W8 Chronic
DEC then `AddCondition` / `AssignAccommodation` / capability modifiers — or retire. Collision set: MedicalPipeline affliction handlers, Amputation, HealthHistory, Bionics.

### W9 Routines
`AssignRoutine` · `SetPreference` · `EvaluateDailySatisfaction` · `DetectConflicts` (**unique**) · `Resolve` · satisfaction → Needs ports. Host Reset only.

### W10 Sleep Acoustic
`EvaluateQuarterSleep` · `InstallSensoryReliefKit` · `RestockSensoryReliefKits` · `UpdateRoomSoundproofing`/`UpdateSleepingQuarterSoundproofing` · quiet-hours **sync from** `ShelterNoiseSystem.SetQuietHours` · Needs ports from `SleepQualityResult` via `ComputeNetFatigueRecovery`.

---

## R4-32. Reachability snapshot (re-measured 2026-09-25)

| Type | HOST | MAIN | Program role |
|---|---:|---:|---|
| RecruitmentSystem | 0 | 0 | W2 orphan wire |
| ChronicConditionSystem | 0 | 0 | W8 DEC |
| EmergencyAlertSystem | 0 | 0 | deferred parallel |
| GenealogyBridge | 0 | 0 | Tier B |
| RadiationEconomyBridge | 0 | 0 | Tier B |
| RomanceFamilySystem | 1 | 0 | W1 |
| VehicleCustomizationSystem | 1 | 0 | W3 |
| PlayerTradeRouteSystem | 1 | 0 | W4 |
| ColonySystem | 5 | 2 | W4 thicken host+UI |
| SurvivorRoutineSystem | 2 | 1 | W9 |
| ExerciseSystem | ≥2 | 1 | Tier B / future W11 |
| InterpersonalConflictSystem | 4 | 2 | Tier B DEC vs relations |
| DreamSystem | ≥4 | 2 | Tier B |
| MemoryDecaySystem | 4 | 2 | Tier B |
| SleepAcoustic host chain | live | 1 | W10 effect-thin seal |

Save pin **266**. Surfaces **219** / Interactive **58** / ReadOnly **161**. Port policy seams **307**. HEAD `1678c074`.

### Port contract pins (retained from first R4 pass)

- Port contract policy: `docs/ci/port_contract_policy.json` total_seams **307**.
- Architecture map subsystem count tracked near save pin era (**266** subsystems in recent seals).
- W10 must not invent unbound HOST_REQUIRED ports without updating policy through integrator.


---

## R4-33. Player-surface honesty notes

- W9/W10 should increase interactive honesty when commands become reachable without CLI.
- Night watch & personal belongings: operable buttons exist while manifest says ReadOnly — **manifest lag**, not missing loops.
- `vehicle_garage` ReadOnlyObservational supports W3.
- `politics` ReadOnlyObservational supports W5 UI reconcile.
- `kitchen_nutrition` Interactive but wrong cooking owner — W5.
- `shelter_atmosphere` Interactive already owns quiet-hours write — W10 extends, does not replace.

---

## R4-34. Worked examples (precision)

### Restful night port arithmetic
baseRecovery=12, multiplierPermille=1200, moraleBonus=+2:
`ComputeNetFatigueRecovery(12,1200)=14` → `Modify(Fatigue,-14)` → `Modify(Morale,-2)`.

### Unbearable morale
moraleBonus=-6 → `Modify(Morale,-(-6))` = `Modify(Morale,+6)` (worsens under higher=worse).

### DetectConflicts uniqueness
First detect for sleep_disturbance(A,B) creates one unresolved row. Second detect same day without schedule change must **not** append another (R4 required harden).

### Recruitment threshold
SuccessChance 39 → fail; 40 → succeed + `DiscoverCandidate` with `IsRecruited=true`. Host intakes once into survivors roster.

### Colony founding
`EstablishColony(locationId, name, ColonyType.Outpost, garrison, supplies, day)` returns `ColonyOutpost`. Resupply via `TransferSupplies`. Soft-close logistics via `SetSupplyLineStatus` — not Abandon.

### Quiet hours sync
Atmosphere toggle calls `ShelterAtmosphereHostSession.SetQuietHours` → Noise state. Before sleep evaluate, copy into SleepAcoustic ledger **or** pass Noise flags into `EvaluateQuarterSleep` so compliance cannot drift.

---

## R4-35. Shared-hub caution list

Claim explicitly before editing:

- `src/Main.PlayerSurfaces.cs`
- `src/Main.GameFlow.cs` panel routes
- PanelRegistryBootstrap / `Main.UiPanels.cs`
- `docs/player_surface_manifest.json`
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`
- `Assets/Ashfall.Core/HostCliRegistry.cs`
- `src/Main.CampaignOwners.cs`
- `src/Main.Lifecycle.cs` / `src/Main.SaveOrchestrator.cs`

W10 default = expand `ShelterAtmospherePanel` to minimize hub edits (same contention class as DEBT-PLAN181).

---

## R4-36. Tier B runner-ups (post-program)

1. **Exercise board + fatigue-resistance composition** — `GetFatigueResistanceMultiplier` into DutyRoster / needs fatigue accrual; zero surfaces today.
2. **Dream interpretation board** — `InterpretSurvivorDream` unused by UI.
3. **Memory reinforce desk** — keep PhantomMemory separate.
4. **InterpersonalConflict DEC** — Plan 202 session vs SurvivorRelations mediate UI.
5. **GenealogyBridge custody** — event bridge over GenerationalLineage.
6. **RadiationEconomyBridge** — hook EvaluateTrade onto live trade path.

---

## R4-37. W10 inventory / sensory kit honesty

If restock should consume inventory, find schema-valid item ids first. If none, keep reserve counter with honest UI label (“sensory kit reserve”) or author one item in a tiny follow-up content claim. Never spawn undeclared items.

Ambient noise projection: optional host map from `ShelterNoiseSystem.GetRoomNoise(roomId)` into `SleepingQuarterState.AmbientNoiseLevelDecibels` each tick so dormitory cards track workshop noise after atmosphere soundproofing.

Occupancy projection: refresh `AssignedOccupants` from `ShelterAssignmentSystem` before evaluate; Ordinal-sort survivor ids when porting Needs.

---

## R4-38. Scheduling trains

**Social-first (default):** W1→W2→W3→W4→W5→W6→W7→W8→W9→W10

**Reliability-first (R4 option):** W10 (silent Needs gap) → W9 (conflict spam) → W1 → W2 → …

Foreman selects; WORKTREE claims stay disjoint.

---

## R4-39. W10 failure injection checklist

1. Null sleep session → Setup guard.
2. Unknown roomId → false/no-op + LastEvent.
3. Kits reserve 0 → install false; UI disabled.
4. Occupant missing in NeedsSystem → skip + selftest counter.
5. Extreme ambient dB → Unbearable band; multiplier floor.
6. Quiet hours wrap 22→06 → Noise schedule authoritative.
7. Save mid-tick → pre-day snapshot restore.
8. UI unmounted → tick still ports.
9. Double AdvanceDay same day → ports not double-applied (guard).
10. Divergent quiet hours before sync → sync at tick start; assert equality in test.

---

## R4-40. Relationship to Expansion 41 UNBLOCK

Expansion 41 sealed SleepAcoustic host reachability (session/save/day/CLI). W10 does **not** redo that seal. It closes the remaining **player-facing + Needs effect** gap: host-complete / player-thin / effect-thin after UNBLOCK.

---

## R4-41. Deep W10 phase narratives (integrator)

### P0 narrative
Lock collision note: quiet-hours write=`ShelterNoiseSystem`; SleepAcoustic syncs. Confirm section `sleep_acoustic_rest` already registered. Claim atmosphere panel path XOR new panel path. Capture baseline selftest PASS count. Reject any design that adds `sleep_hp` shadow fields.

### P1 narrative
Implement Needs ports in host adapter with injectable needs sink for tests. Prefer `ApplyAttributedDelta` with attribute `sleep_acoustic` when signature allows; else `Modify`. Prove Restful vs Unbearable polarity. Do not edit engine band thresholds.

### P2 narrative
Wire atmosphere toggle → sync helper. Add test: Noise quiet hours false → SleepAcoustic evaluate compliance path uses ViolatedMinor (or explicit passed compliance). Document chosen compliance mapping when QuietHoursActive is false.

### P3 narrative
UI: dormitory section lists quarters, band enum, ‰ multipliers, kit reserve, install/restock, ‰ soundproof controls. All buttons call Main verbs. No local sleep score math. If new route required, claim shared hubs first.

### P4 narrative
Focused tests only; headless selftest; rg for Needs mutator on tick path; pin 266 unchanged; player script R4-17.

---

## R4-42. Cross-wave dependency notes for W10

- After W9: chronotype sleep hours can refine compliance classification (optional later).
- After W7 maintenance: generator noise may feed ambient dB projection.
- After W1: family co-sleeping rooms share quarter crowding — occupancy refresh handles it.
- Independent of W2/W3/W4 economically; soft narrative only.

---

## R4-43. Program risk register addendum (R4)

| Risk | Wave | Mitigation |
|---|---|---|
| Implementer codes CancelCampaign | W2 | R4-4 override + P0 rg gate |
| UI offers Abandon | W4 | R4-5 + copy review |
| Conflict spam softlocks board | W9 | required uniqueness harden |
| Dual quiet hours confuse recovery | W10 | Noise write + sync |
| Needs polarity sign bug spreads | W9/W10 | use XML higher=worse; don't “fix” critical path in these waves |
| Shared hub races | W7/W10 | claim rows; prefer expand existing panels |
| Recruitment without roster bridge | W2 | DoD requires intake |

---

## R4-44. Definition of Done crosswalk (R4)

| Success line (§0) | Wave | Evidence |
|---|---|---|
| Court/break/families | W1 | UI→Romance verbs |
| Recruitment campaigns | W2 | host+save+board+roster |
| Vehicle modules | W3 | InstallModule path |
| Trade routes | W4 | schedule UI |
| Colonies | W4 | EstablishColony UI |
| Governance | W5 | DEC+writes |
| Cooking start | W5 | StartCooking bound |
| Disaster protocols | W5 | HUD |
| NG+ legacy | W6 | board |
| Ideology/NPC | W6 | resolve/acts |
| Difficulty runtime | W7 | panel |
| Maintenance | W7 | board |
| Keepsakes | W7 | content |
| Chronic | W8 | DEC path |
| Routines | W9 | board+ports+unique conflicts |
| Sleep recovery | W10 | kits+ports+sync |

---

## R4-45. Final R4 quality gates

- [x] Fresh orphan/thin audit performed on HEAD `1678c074`
- [x] P0 API fiction corrected (Recruitment, Colony, DetectConflicts)
- [x] Romance/Needs confirmed accurate
- [x] Additional gap+mechanic added (W10 Sleep Acoustic recovery ports)
- [x] Quiet-hours dual-state collision documented
- [x] Phases, tests, files, DoD, handoff present for W10
- [x] Active plan trimmed to evidence-dense target; pre-R4 appendix essay mountain was condensed **in place** (this file is untracked — no git copy exists; load-bearing recipes live in R4-3…R4-57 and the retained appendices)
- [x] No production code modified by planning authorship
- [x] Session `plan.md` synced

### Out-of-scope reminder (program, retained from first R4 pass)

Greenfield wildfire / mobile clinic / public works / scenario library; Unity; full-suite default; mass format; WaterSourceSystem/FoodTypeSystem/FactionDiplomacySystem false parallels; EmergencyAlert revival; C3 stale HOLD confusion for 174/175/192/199.

---


---



---

## R4-46. Deep exemplar notes — W2 Recruitment (corrected)

**Core file:** `Assets/Ashfall.Core/Survivors/RecruitmentSystem.cs` (~360 LOC)
**Tests:** `Ashfall.Core.Tests/Survivors/Plan204RecruitmentIntegrationTests.cs`
**Catalog expectation:** recruitment templates JSON (re-rg path on claim; load via `LoadCatalog`).

**Minimal host wire (mirrors Bestiary/HealthHistory pattern):**
1. `RecruitmentHostSession` + checksummed `RecruitmentSaveStore` (`recruitment_save.json`, section `recruitment`).
2. `Main.Recruitment.cs` Setup/Save/Reset + wrappers for StartCampaign/MakeDefectionOffer/DiscoverCandidate/TickDay/census-like getters.
3. Day owner phase 5 calling `TickDay`.
4. SaveSectionRegistry + pin bump 266→267 with corruption test update.
5. CLI 12-check selftest.
6. Roster bridge on recruited candidates (single intake).
7. Player muster board (new panel or expand survivors/faction ops surface).

**Do not:** call prisoner recruit APIs as Plan 204; invent Cancel/Accept; use System.Random for success.

---

## R4-47. Deep exemplar notes — W9 conflicts (required harden)

Pseudocode for uniqueness:

```
bool ExistsUnresolved(type, a, b):
  return Conflicts.Any(c => !c.IsResolved && c.ConflictType==type && unordered_pair(c.A,c.B)==unordered_pair(a,b))

on detect candidate:
  if ExistsUnresolved(...): skip
  else append new ConflictId
```

Tests: detect twice → count +0 second time; resolve → IsResolved; optional re-open policy documented.

Satisfaction ports: map OverallSatisfaction into Morale/Fatigue with higher=worse; run after phase-3 needs; attribute `survivor_routines`.

---

## R4-48. Deep exemplar notes — W1 Romance UI bind

Provider-thin today: expand Nursery/Survivor Detail/dedicated romance board. Buttons must call:

- Initiate → `TryInitiateAttraction(...)` with live signature (re-rg args: affinities, ideology tags, rng, day, force)
- Courtship event → `ConductCourtshipEvent(...)`
- Dissolve → `DissolvePartnership`
- Form household → `FormFamilyUnit`

Journal on seams `OnPartnershipDissolvedSeam` etc. Save reuse `romance_family`.


---

## R4-49. Architectural rationale — why W10 seals a silent failure

ASHFALL’s post-UNBLOCK defect class is no longer “Core missing.” It is “Core + host + save + CLI exist while the campaign still cannot feel the system.” SleepAcoustic is the textbook case:

1. **Authored math exists** — attenuation, crowding, quiet-hours compliance, band classification, fatigue multiplier permille, morale bonus.
2. **Host lifecycle exists** — setup, checksummed save, phase-5 day owner, census, CLI probe.
3. **Comments claim Needs extension** — multiple files state the engine extends NeedsSystem.
4. **AdvanceDay discards results** — the evaluate call’s return value is unused; TotalNightsSlept increments alone.
5. **Players already toggle quiet hours** on `shelter_atmosphere` against a *different* state object.

Therefore W10 is not a greenfield feature. It is a **reliability seal** that makes Expansion 41’s integration honest at the survival layer. Prefer scheduling it on the reliability-first train when silent failures outrank new social boards.

---

## R4-50. Architectural rationale — quiet hours one-write rule

Two mutable quiet-hours schedules are a Rule 5 violation waiting to happen. R4 chooses:

- **Write authority:** `ShelterNoiseSystem` (already Interactive via atmosphere).
- **Read/sync authority:** SleepAcoustic evaluate inputs.
- **UI rule:** Dormitory board may *display* quiet hours and deep-link/toggle through atmosphere/Noise APIs; it must not present an unlabeled second schedule that only writes SleepAcousticState.

Soundproofing may remain dual-scale (Noise % vs Sleep ‰) if labeled and optionally projected; quiet hours may not remain dual-write.

---

## R4-51. Needs port attribution cookbook (W10)

When `ApplyAttributedDelta` is available:

```
ApplyAttributedDelta(survivorId, NeedKind.Fatigue, -recovery, attribute: "sleep_acoustic")
ApplyAttributedDelta(survivorId, NeedKind.Morale, -moraleBonus, attribute: "sleep_acoustic")
```

When only `Modify` exists, call Modify and emit a journal/day event noting sleep band. Never store Fatigue inside SleepAcousticState.

Base recovery constant: document in claim (recommended 10–20). Clamp through NeedsSystem. Sort occupant ids Ordinal before apply.

Phase note: `survivors_needs` phase 3 accrues fatigue; `sleep_acoustic_rest` phase 5 recovers. Same-day recovery after accrual is intended. If double-counting appears versus routines sleep satisfaction ports (W9), reduce one port’s magnitude rather than merging ledgers.

---

## R4-52. Panel expand sketch — ShelterAtmosphere dormitory section

Planning wireframe (not production):

```
SHELTER ATMOSPHERE
[Noise overview…]
Quiet Hours: ACTIVE 22:00–06:00  [Lift / Enforce]   ← Noise-backed

DORMITORY SLEEP (SleepAcoustic)
Quarter: primary_shelter_dormitory
Occupants: 4   Ambient: 55 dB → bunk 41 dB
Band: RESTFUL   Quality: 720‰   Fatigue mult: 1200‰   Morale Δ: +2
Kits reserve: 7
[Install sensory kit] [Restock +1]
Wall lining: [====|----] 600‰   Door seal: [===|-----] 500‰
[Apply soundproofing]
Last night: TotalNights=12  Disturbed quarters=0
```

All actions call Main verbs; labels show ‰ vs Noise % clearly.

---

## R4-53. Premise-failure exits (W10)

Stop and report to foreman if:

- `sleep_acoustic_rest` section missing from registry (unexpected)
- NeedsSystem polarity docs contradict required tests and cannot be interpreted
- ShelterAtmospherePanel claim held by another package and new route hubs also held
- SleepAcoustic Core is mid-rewrite on another claim
- Evidence shows Needs ports already landed (re-verify; don’t duplicate)

---

## R4-54. Mapping to ashfall-plan §1–25 checklist

| Plan § | W10 coverage |
|---|---|
| 1 Objective | W10.1 |
| 2 Current Reality | W10.2 |
| 3 Delta | W10.3 |
| 4 Evidence | R4-0, R4-11/12 |
| 5 Extension seams | atmosphere + Needs + assignment |
| 6 Architecture | collision map W10.2 |
| 7 Ownership | W10.5 |
| 8 Data flow | W10.7 |
| 9 State | W10.6 |
| 10 API | W10.6 / R4-31 |
| 11 Data | R4-37 kits |
| 12 Save/Load | reuse section |
| 13 Determinism | sorted ids; no Random |
| 14 Wiring | day owner ports |
| 15 Godot | atmosphere expand |
| 16 Narrative | journal attribute |
| 17 Failures | W10.9 / R4-39 |
| 18 Tests | W10.10 |
| 19 Phases | W10.8 / R4-41 |
| 20 Files | W10.11 |
| 21 Risks | W10.12 / R4-43 |
| 22 Out of scope | W10.4 |
| 23 Rollback | W10.13 |
| 24 DoD | W10.14 |
| 25 Handoff | W10.15 |

---




---

## R4-55. Precision pass certificate (2026-09-25)

Re-verified against live tree at HEAD `1678c0749f49b5e4f9a99231e8e71acf3e47fdb1`:

| Check | Result |
|---|---|
| Romance host verbs | PASS — TryInitiateAttraction / ConductCourtshipEvent / DissolvePartnership / FormFamilyUnit |
| Needs polarity docs | PASS — Hunger/Thirst/Fatigue/Morale higher=worse; Modify/ApplyAttributedDelta |
| Recruitment live API | PASS — DiscoverCandidate / StartCampaign / MakeDefectionOffer / TickDay; CaptureState exists |
| Colony live API | PASS — EstablishColony / TransferSupplies / Assign; no Abandon |
| DetectConflicts retick | PASS proven spam — W9 harden required |
| SleepAcoustic discard | PASS proven — AdvanceDay drops SleepQualityResult; W10 ports required |
| Quiet hours dual-state | PASS — ShelterNoise write + SleepAcoustic fields; W10 sync required |
| Save pin | PASS — 266 |
| Surfaces | PASS — 219 / 58 interactive / 161 read-only |
| Production code touched by this plan | PASS — none |

Workflow completed this session: fresh gap audit → ~100k core wave contracts retained → accuracy polish (P0 API ledger) → expand with W10 gap+mechanic → precision certificate → sync `plan.md`.



---

## R4-56. Immediate next-agent briefing

**Do not implement until the user/foreman authorizes a wave claim.** This document is planning-only.

When authorized, preferred first claims (pick one train):

1. **Reliability:** `PFGL-W10-SLEEP-ACOUSTIC-RECOVERY` — close silent Needs discard; sync quiet hours; dormitory UI on atmosphere.
2. **Social:** `PFGL-W1-ROMANCE-BOARD` — bind live Romance verbs to an operable board.
3. **Orphan:** `PFGL-W2-RECRUITMENT-WIRE` — full host wire using **live** Recruitment API only.

Before coding: read `WORKTREE_OWNERSHIP.md`, claim exact paths, re-rg APIs (R4-3), run focused baselines only (`TEST_POLICY.md`). Shared hubs remain integrator-owned. Pin stays **266** unless W2/W8 adds a section.

Success for any wave: player-operable proof in an ordinary campaign session, focused tests green, no parallel authority, handoff per `AI_AGENT_WORKFLOW.md`.


- Reconfirm HEAD with `git rev-parse HEAD` at claim time.

- Reconfirm HEAD with `git rev-parse HEAD` at claim time.

- Reconfirm HEAD with `git rev-parse HEAD` at claim time.

- Reconfirm HEAD with `git rev-parse HEAD` at claim time.

---

## R4-57. Mini glossary

| Term | Meaning |
|---|---|
| Player-operable | Ordinary campaign UI/command path exercises the live owner end-to-end |
| Host-complete / player-thin | Setup/save/CLI exist; InteractiveCommands board missing or wrong-bound |
| Effect-thin | Host ticks but authored outcomes never reach Needs/inventory/roster |
| True orphan | Core type with zero `src/` references |
| One authority | Rule 5 — single mutable owner per concern |
| Pin 266 | `SaveSectionRegistry.All.Count` test baseline at evidence HEAD |
| W10 sync | Copy/read ShelterNoise quiet hours into SleepAcoustic evaluate inputs |

# Appendix BT — Pre-Integration Readiness Gate (PIR) — smooth, precise integration before code

## BT1. Purpose
PIR is the program's **pre-integration verification**: proof, before a single line of wave code lands, that the integration will be smooth, precise, and architecturally clean. A wave that fails any PIR row stops before code — no “fix while integrating”. Evidence for each row is pasted into the wave's acceptance scrapbook at claim time. Cross-checks: R4-45 quality gates (doc-level) and R4-3 runbook (probe commands).

## BT2. The ten gates
| # | Gate | Evidence required | On fail |
|---|---|---|---|
| PIR-1 | **Evidence freshness** | `git rev-parse HEAD` recorded. If HEAD moves past `1678c074`, re-run the R4-3 probes + the wave's BR watchlist row before claiming | Re-verify; do not trust this plan's verb tables blindly |
| PIR-2 | **Ownership** | Exact paths claimed in `WORKTREE_OWNERSHIP.md`; R4-35 shared-hub caution list free or mutex-held | Wait or hand to integrator |
| PIR-3 | **API copy** | Wave implementer pastes live signatures from the owner source file into the claim note and diffs against §10/R4-31. Any divergence → revision note here first (Rule 7) | Reconcile plan vs code before writing code |
| PIR-4 | **Collision map** | The wave's §6.3 row resolved; DECs signed if required (DEC-GOV, DEC-CHRONIC); false parallels (Appendix C / Q2 list) untouched | Stop for signature (AM2-equivalent request) |
| PIR-5 | **Save plan** | Reuse vs new section decided. New section ⇒ pin bump + `ComprehensiveSaveStoreCorruptionAndMigrationTests` edit planned in the same change; §9.6 filename uniqueness checked | Redesign to reuse if possible |
| PIR-6 | **Determinism plan** | RNG stream named (`_campaignDay.Rng.Fork(CampaignStreamIds.*, …)` or “none required”); `System.Random`/wall-clock banned; idempotency guard named for every day-ported effect | Redesign port |
| PIR-7 | **Test plan** | Focused target files named; new test files run alone first; `scripts/run_test.sh` targets under TEST_POLICY caps; no full suite | Narrow scope |
| PIR-8 | **UI plan** | Bind-vs-new-route decided. New route ⇒ panel class + `PanelRegistryBootstrap` + `Main.GameFlow` + `Main.PlayerSurfaces` + manifest all in the file impact map; accessibility rows included | Split wave or bind existing surface |
| PIR-9 | **Baseline green** | Pre-change compile + the wave's existing focused tests/selftest pass **before** edits, so post-change failures are attributable | Establish baseline first |
| PIR-10 | **Rollback slice** | First commit slice identified — small, reversible; the wave rollback section is executable as written | Shrink the slice |

## BT3. Per-wave PIR hot spots
| Wave | Highest-risk premise | PIR row that catches it |
|---|---|---|
| W1 Romance | UI binding stale romance verbs | PIR-3 (live verbs are `TryInitiateAttraction` et al.) |
| W2 Recruitment | Inventing `CancelCampaign`/`AcceptCandidate` wrappers instead of Core verbs | PIR-3 + PIR-6 (seeded roll design; R4-4 patch) |
| W3 Vehicle | Garage (`InstallModification`/`InstallArmorGrade`) vs customization authority bleed | PIR-4 (seam split, R4-31 W3) |
| W4 Trade/Colony | Apiary “colony” collision; abandon verb that doesn't exist | PIR-4 + PIR-3 (R4-5 patch) |
| W5 Governance | Dual elections if DEC-GOV unsigned | PIR-4 (hard stop) |
| W5 Cook/Disaster | Kitchen dual authority; borrowing `ResolveIncident` from airlock owner | PIR-3 + PIR-4 |
| W6 Legacy/Ideology/NPC | Parallel NG+ store; zealotry writes | PIR-4 + PIR-5 |
| W7 Difficulty/Maint/Content | Panel-hub race (DEBT-PLAN181 lists the shared seams) | PIR-2 (hub mutex) |
| W8 Chronic | Wiring before DEC-CHRONIC | PIR-4 (hard stop) |
| W9 Routines | Port adapter becoming a second wellbeing ledger; double tick; DetectConflicts duplicate rows | PIR-5 + PIR-6 + R4-6 harden |
| W10 Sleep Acoustic | Quiet-hours dual-write; Needs polarity sign errors | PIR-4 (one-write rule R4-50) + PIR-6 (R4-51 cookbook) |

## BT4. Pre-integration dry run (last PIR step for waves with UI)
Execute in order; all green before declaring the wave integrated (§6.2 PLAYER-OPERABLE, not CLI-complete):
1. Compile Core + host (`dotnet build` targets per TEST_POLICY).
2. Focused Core tests for the wave (`scripts/run_test.sh <files>`).
3. CLI selftest (`godot --headless --path . -- --<wave>-selftest`) — evidence only, not the goal.
4. Panel route gates (`PanelRouteGateTests`, `PlayerSurfaceCoverageGateTests`) if UI touched.
5. Save round-trip: mutate → save → load → assert (focused persistence test per §9).
6. Manual player script at 15 FPS.

## BT5. Integration smoothness contract
1. **Landing order inside a wave:** Core harden → host/store → Main/day owner → registries → UI → verify. Never UI before the command exists.
2. **One seam per commit slice;** compile + focused tests green between slices.
3. **Shared hubs** (R4-35 list) are edited in a single owned burst per wave and released in the handoff; no two waves hold `PanelRegistryBootstrap`/`GameFlow`/`SaveSectionRegistry` simultaneously.
4. **Port contract:** classify every new public seam in the same change that creates it; 0 unbound HOST_REQUIRED at exit (policy file pins in R4-32).
5. **Fail-closed everywhere:** unknown ids reject; missing owners skip with a journal fact; no exceptions swallowed into silent success.
6. **One authority per concern** re-asserted at exit: `rg` shows no second ledger type for the wave's domain.

---

# Appendix BU — Seal quality framework: gap / feature / mechanic

## BU1. Three seal tiers
| Tier | Meaning | Required evidence |
|---|---|---|
| **GAP SEAL** | A Core authority that was host-invisible becomes reachable: host session + save + day owner + CLI probe exist and agree | host_files ≥ 1; save round-trip; selftest green; completion chain §6.2 to VERIFIED |
| **FEATURE SEAL** | The loop is **player-operable**: a normal session has a truthful command surface bound to the right owner | all GAP evidence + interactive surface registered + UI-bound command mutation test + manifest entry |
| **MECHANIC SEAL** | The loop **changes decisions**: pressure/trade-off, observable consequence in existing owners, and counterplay | all FEATURE evidence + consequence port or event + focused determinism/polarity test + player script proving cause→effect |

A wave is only “integrated” at its declared tier. CLI-complete is never FEATURE; provider-only is never FEATURE; a board over dead verbs is never MECHANIC; discarded computation (W10's `SleepQualityResult`) is never a mechanic.

## BU2. Meaningfulness pentad (all five for FEATURE; add counterplay for MECHANIC)
| Criterion | Question | Failing example |
|---|---|---|
| Agency | Does the player initiate the verb? | auto-only tick with no input |
| Legibility | Can the player read the relevant state in UI? | hidden satisfaction scores |
| Consequence | Does Core state change observably? | journal-only “success” |
| Persistence | Does it survive save/load? | session-only effect |
| Feedback | Is the result shown (journal, readout, badge)? | silent mutation |
| Counterplay (MECHANIC) | Can the player act against the pressure? | unavoidable morale drain |

## BU3. Declared tier per wave
| Wave | Tier | Mechanic payload that justifies the tier |
|---|---|---|
| W1 Romance | FEATURE (MECHANIC-hook) | courtship timing vs schedule (W9 social blocks); family consequences feed existing morale/needs readouts |
| W2 Recruitment | FEATURE → MECHANIC | cost/risk campaigns (food/water bills + `DiscoveredRisk`); P1 seeded roll adds variance with counterplay via recruiter skill |
| W3 Vehicle | FEATURE | modules alter bunk/cargo/defense and enable base-camp/rest — surfaced, not silent |
| W4 Trade/Colony | FEATURE | tariffs (`TotalTariffChitsPaid`) + supply lines create upkeep pressure readable on the board |
| W5 Governance | MECHANIC (post-DEC) | consent/grievance/stability loop with dispute counterplay |
| W5 Cook/Disaster | FEATURE | cooking progress/cancel + protocol activation with `CalculateRoomDamage`/`AdjustResilience` readouts |
| W6 Legacy/Ideology/NPC | FEATURE | NG+ boon trade-offs; confrontation/conversion; grudge→price/refusal consequences |
| W7 Difficulty/Maint/Content | FEATURE | mid-run scalar changes + ironman lock (genuine risk choice); maintenance integrity pressure |
| W8 Chronic | DECISION→(FEATURE or RETIRE) | accommodations modulate capability — declared only after DEC-CHRONIC |
| W9 Routines | **MECHANIC** | schedule conflicts + satisfaction→Needs/morale ports with polarity tests and counterplay (reassign/resolve) |
| W10 Sleep Acoustic | **MECHANIC** | quiet-hours compliance / soundproofing / sensory kits trade-offs → `SleepQualityResult` ports into Fatigue/Morale (R4-51) |

## BU4. Seal checklists (paste into wave DoD)
**Gap seal:** host session exists and is constructed in Main setup · save section captured/restored (or justified reuse) · day owner wired if stateful · CLI selftest registered and green · port contract classified.

**Feature seal:** interactive surface registered (panel battery complete) · every control maps to a live §10/R4-31 verb · UI shows Core truth only (no offline math) · manifest count updated · manual player script passes.

**Mechanic seal:** consequence lands in an **existing** owner (port/event) · idempotency + clamps documented · polarity/determinism focused test (higher=worse Morale convention — but verify each call site per R4-1 C5) · counterplay verified in the manual script · no parallel ledger (`rg` assertion).

## BU5. Anti-patterns that fail the seal
1. CLI-complete declared as player-operable.
2. Read-only provider counted as a board.
3. A panel that computes success chance/cost offline.
4. A “temporary” shadow store for wellbeing/schedule/economy state.
5. A board whose verbs don't match live API names (the R4-1 C1/C2 drift class — caught by PIR-3).
6. Journal prose as the only proof of effect.
7. Authored math computed and discarded (W10 class) advertised as a working mechanic.

---

# Appendix BV — R4 live verification log (dual-pass merge)

## BV1. Header
Evidence HEAD: `1678c0749f49b5e4f9a99231e8e71acf3e47fdb1` (unchanged from R2/R3 stamp — `git rev-parse HEAD` re-run at R4)
Pass type: pre-integration verification + API contract hardening (two independent R4 probe passes merged; see R4-0/R4-1 for the second pass's gap audit and C1–C12 correction ledger)
Production code changed: none

## BV2. Probes executed (independent pass, all against live source)
| Command family | Result |
|---|---|
| `rg -l '\b<Type>\b' src` for 17 named authorities | match §2.4 table (Recruitment/Chronic/EmergencyAlert = 0; SurvivorRoutineSystem = 2 direct / 9 seam files) |
| `Assert.Equal(266, …)` grep | pin live at `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs:315,317` |
| save-key existence grep on `SaveSectionRegistry.cs` | all 14 named keys present |
| method-signature sweeps on 16 owner files + 10 host sessions | §10 + R4-31 verb cards built from these |
| `NeedsSystem.cs` mutator sweep | `Modify` / `ApplyAttributedDelta` / `SetExternalModifier` confirmed |
| `routine_templates.json` parse | schema 1; `routine_early_riser`, `routine_night_owl`, `routine_standard`, `routine_night_shift` |
| `player_surface_manifest.json` parse | 219 / 58 / 161 |
| `HostCliAction.SurvivorRoutinesSelfTest` grep | registered `HostCli.cs:243`, `HostCliRegistry.cs:143,967` |
| `PoliticsUI.cs` / kitchen panels / `SurvivorDetailPanel.RoutineProvider` reads | mismatch & thinness premises hold |

## BV3. Drift ledger
The authoritative correction ledger is **R4-1 (C1–C12)**. This independent pass confirmed the same drift set — `CancelCampaign`/`TickCampaigns`/`AcceptCandidate` (W2), Found/Supply/Abandon (W4), `ActivateDisasterProtocol`/`ResolveIncident` (W5 disaster), `SetCustomLane`/`ScheduleRepair`/`ClearAlert` (W7) — and additionally recorded the W9 host-file-count precision fix (§2.4) and `AddChildToFamily`/`SetNgPlusBoonActive` additions.

## BV4. What R4 deliberately did not change
Wave sequencing (except W10 addition), DEC gates, ownership strategy, save-pin policy, W9/W10 mechanic designs, and all R2/R3 historical logs (BI, AF, AZ1/AZ1b) remain as recorded. No production code, no ledger edits, no claims.

## BV5. Residual uncertainties after R4
| Item | Uncertainty | Path |
|---|---|---|
| W2 cancel/accept verb design | first-class Core verbs vs inline status changes | P1 design note under PIR-3 / R4-4 |
| Governance write owner | DEC-GOV unsigned | decision request at W5 P0 |
| Chronic wire-or-retire | DEC-CHRONIC unsigned | decision request at W8 P0 |
| DetectConflicts duplicate-on-retick | R4-1 C3 marks proven; harden scope fixed in R4-6 | W9 P0 implements |
| Needs polarity at critical-path call site | R4-1 C5 sign inconsistency | file separate bug unless W9/W10 touch it |
| Port-contract seam counts (~307) | not re-measured in this pass | re-measure at first wave claim (PIR-1) |

---

## BV6. R5 re-verification addendum (W10 contract + honesty seals)

Pass type: live verification of the merged R4/W10 content; production code changed: none.

| Probe | Result | Plan action |
|---|---|---|
| `SleepAcousticLedger` public API sweep | `RegisterOrUpdateQuarter` / `GetQuarter` / `EvaluateQuarterSleep(roomId, isQuietHours, compliance, hoursSlept)` / `InstallSensoryReliefKit(roomId)` / `RestockSensoryReliefKits(count)` / `UpdateRoomSoundproofing(roomId, wallPermille, doorPermille)` / `SetQuietHoursSchedule(startHour, endHour, active)` / `AdvanceDay(hoursSlept = 8)` / `GetCensus` / `CaptureState` / `RestoreState` | §10 W10 block added with exact signatures |
| `SleepAcousticRestEngine` statics | `CalculateAcousticAttenuation` / `CalculateCrowdingDensity` / `EvaluateSleepQuality` / `ComputeNetFatigueRecovery` confirmed | §10 W10 |
| Main wrappers | `EvaluateSleepingQuarterSleep` / `InstallSensoryReliefKit` / `RestockSensoryReliefKits` / `UpdateSleepingQuarterSoundproofing` / `SetQuietHoursSchedule` / `AdvanceSleepAcousticRestDay(hoursSlept = 8)` / `GetSleepAcousticCensus` confirmed | §10 W10 |
| W10 wiring | day owner `sleep_acoustic_rest` phase 5 (`Main.CampaignOwners.cs:176`); save row + filename (`SaveSectionRegistry.cs:312,602`); CLI action `SleepAcousticRestSelfTest` (`HostCliRegistry.cs:138,937`) | W10 host-complete premise holds; save pin untouched |
| CLI flag name | live `--sleep-acoustic-selftest` (alias `--the-quiet-selftest`); `--sleep-acoustic-rest-selftest` **does not exist** | DRIFT CORRECTED in all 4 command rows |
| C12 discard claim | `AdvanceDay` computes `EvaluateQuarterSleep` result and never reads it — only `TotalNightsSlept++` | CONFIRMED — the W10 Needs port is the seal |
| Census assumption (new finding) | `GetCensus` re-evaluates bands with hardcoded `Compliant` + 8h | Honest-readout requirement added to W10.3 + §10 |
| C3 DetectConflicts duplicate | `DetectConflicts` appends fresh `ConflictId` + `_state.Conflicts.Add` unconditionally — duplicates on retick | CONFIRMED — W9 harden (R4-6) is required, not optional |
| C5 Needs polarity | `ApplyCriticalNeedConsequences` calls `Modify(Morale, -loss)` under critical needs — sign contradicts higher=worse | CONFIRMED — separate bug unless W9/W10 touch that path |
| W10 morale polarity | engine `MoraleRestorationBonus` is good-positive (`DeepSanctuary +4` … `Unbearable −6`) | R4-35 port arithmetic correct as written |
| Garage verbs | `VehicleGarageSystem.InstallModification` / `InstallArmorGrade` (with `CanInstall*` gates + `IPlayerInventoryPort`) distinct from `VehicleCustomizationSystem.InstallModule` | CONFIRMED seam split (W3) |
| Tier B orphans | `GenealogyBridge` / `RadiationEconomyBridge` = 0 `src/` refs | CONFIRMED |
| Port policy | `docs/ci/port_contract_policy.json` `total_seams: 307` | CONFIRMED |
| Manifest honesty | `vehicle_garage` / `politics` = ReadOnlyObservational; `kitchen_nutrition` = InteractiveCommands; `night_watch` (9 button factories) + `personal_belongings` (claim/grant/favorite buttons) are classified ReadOnlyObservational | CONFIRMED manifest-lag (R4-33) — hygiene microclaim, not feature waves |

## BV6b. R5 deliverables checklist
- [x] §10 W10 contract added with verified signatures (state owner, math owner, Main wrappers)
- [x] Head-matter W10 completeness: §2.4 reachability row, §3 delta row, §7 ownership rows
- [x] CLI flag drift fixed in all acceptance commands
- [x] Census-assumption honesty item added to W10.3 + §10
- [x] Manifest-lag claims verified with concrete button evidence
- [x] C3/C5/C12 forensic claims independently confirmed against live source
- [x] Production code still unchanged

# END OF PFGL-MASTER-2026-09-25 R5
