# ASHFALL Integration Plans

This is the sole live integration ledger. It is not a backlog and must not
duplicate prose from historical plan documents.

## Current batch

**Status:** ACTIVE
**Premise:** Flagship Plans 138–141 authorized by user (foreman) 2026-09-12.
Wave A reconnaissance complete: `docs/plans/PLANS_138_141_WAVE_A_RECONNAISSANCE.md`
(verified live authorities; several plan-text names corrected to real owners).
Implementation follows the proven phased pattern (Plan 173/196 precedent):
Core+catalog+tests → host/save → panel.

| Package | Owner | Exact paths | Acceptance | Focused verify |
|---|---|---|---|---|
| `PLANS-138-138-METROLOGY-PHASE1` | Builder (this session) | `Assets/StreamingAssets/Data/low_background_lead_catalog.json`; `Assets/Ashfall.Core/Radiation/LowBackgroundLeadEngine.cs`; `Ashfall.Core.Tests/Radiation/Plan138LowBackgroundLeadEngineTests.cs` | **DONE 2026-09-12:** 4-profile provenance catalog (very_low/low/ordinary, all residual >0bp — no zero-activity claims); effective-background composition clamped ≥0; mass-weighted cross-contamination downgrade (material conserved); assay detection-limit + below-limit indeterminate band; calibration/skill/duration bounded confidence (0.05..0.97); seeded-RNG deterministic; Capture/Restore round-trip + old-save baseline. Focused 16/16; Radiation suites 117/117; build 0/0. **Phase 2 DONE 2026-09-12:** host session + `low_background_metrology_save.json` save store; `SaveSectionRegistry` row (owner `radiation`); SaveOrchestrator setup+save wiring; forked RNG stream `low_background_metrology` (Fork pattern — cannot shift other streams); environmental background via `EnvironmentBackgroundBpProvider` seam (authored default until the Phase 3 ExposureContext bridge); no day-tick needed (assay is command-driven). Wiring tests 3/3; triad gate 7/7; filename-registry gate 4/4; Plan 173 wiring 1/1; Phase 1 16/16; build 0/0. **Phase 3 DONE 2026-09-12:** `LowBackgroundLeadPanel` (state→confidence→blocker→consequence; honest uncertainty language — below-limit ≠ clean); sample truth wired from water-treatment intake (`incomingContaminationLevel`, never invented by UI); live environmental bridge via `GetLastExposureEnvironment` (shelter-interior EffectiveZoneRadLevel → bp, authored-baseline fallback); panel route `low_background_metrology` in expanded-panel switch + GameFlow; ARCHITECTURE_TEST_MAP Plan 138 row. Arch-map gate 5/5; scene lint 30/0; combined gates 30/30; build 0/0. Plan 138 COMPLETE — next wave: Plan 139 (InSAR). | `bash scripts/run_test.sh Ashfall.Core.Tests/Radiation/Plan138LowBackgroundLeadEngineTests.cs` (16/16); `dotnet build Ashfall.csproj` 0/0 |
| `PLANS-138-139-141-LATER-PHASES` | unclaimed | Plan 138 host/save/panel; Plan 139 InSAR; Plan 140 extrusion; Plan 141 run-flat | per-plan phased acceptance | per-plan |

**Sequencing:** Phase 1 (Core) → Phase 2 (host/save) → Phase 3 (panel), one
plan at a time; Plan 139/140/141 start only after the prior plan's phase
accepts. Packages below stay unclaimed until their wave opens.

## Operating rules

- A new batch is created only after this batch is `ACCEPTED` or `BLOCKED`.
- Reference existing plans by path and ID; do not copy their prose here.
- A batch may cite 9-20 plans but must expose no more than three concurrent
  packages with disjoint ownership.
- Every package needs premise evidence, exact paths, acceptance, and focused
  verification before a builder edits.
- Completed batch rows are moved to a short archive table in this file; full
  historical plans remain where they are until separately archived.

## Recent closed batches

| Batch | Closed | Outcome | Evidence |
|---|---|---|---|
| `BATCH-2026-09-12-DEBT-170-199-REMAINING-MAPS` | 2026-09-12 | Ten remaining PARTIAL family maps ACCEPTED (no parallel systems). Index: `docs/foreman/PLAN_170_199_REMAINING_FAMILY_MAPS.md`. | `python3 scripts/ci/generate-docs-index.py --check` |
| `BATCH-2026-09-12-DEBT-189-WATER-SOURCE-MAP` | 2026-09-12 | Plan 189 map ACCEPTED: no WaterSourceSystem; piezometer source IDs + WT intake gate + Fluid pipes remain owners. | `docs/water/PLAN_189_WATER_SOURCE_AUTHORITY_MAP.md` ACCEPTED; `python3 scripts/ci/generate-docs-index.py --check` |
| `BATCH-2026-09-12-DEBT-TEST-QUARANTINE-HOLDFAST-NPC` | 2026-09-12 | Reinstated `HoldfastNpcCatalogTests`; rematched stale `HoldfastCatalog.Npcs` to `HoldfastNpcCatalogLoader` owner; dropped Compile Remove. | `HoldfastNpcCatalogTests` 3/3 |
| `BATCH-2026-09-12-DEBT-TEST-QUARANTINE-ARCHIVEINKS` | 2026-09-12 | Reinstated `ArchiveInksCatalogTests`; `InkMaterialDefinition` JsonPropertyName for snake_case JSON; dropped Compile Remove. | `ArchiveInksCatalogTests` 12/12; `ArchiveDeskSystemTests` 10/10 |
| `BATCH-2026-09-12-DEBT-TEST-QUARANTINE-UTILITYAI` | 2026-09-12 | Reinstated `UtilityAiExpandedCatalogTests` (stale 20-vs-6 note); dropped its Compile Remove; other quarantine removes untouched. | `bash scripts/run_test.sh Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs` (14/14) |
| `BATCH-2026-09-12-DEBT-184-A11Y-COLORBLIND` | 2026-09-12 | Path β: `colorblind_mode` under UserSettings*; Core `ColorblindColorMapper`; `ToColor` maps Critical/Success per mode; SettingsPanel OptionButton; Theme constants unchanged. | `Ashfall.Core.Tests/Settings/` 30/30; `--settings-selftest` PASS (incl. colorblind asserts); `dotnet build Ashfall.csproj` 0/0 |
| `BATCH-2026-09-12-DEBT-184-A11Y-BRIDGE` | 2026-09-12 | Path α: `AccessibilityPresentation` MotionAllowed/scale; dose/radiation refresh on Apply; settings-selftest asserts four-flag effects. | `UserSettingsRecoveryTests` 14/14; `--settings-selftest` PASS; `dotnet build Ashfall.csproj` 0/0 |
| `BATCH-2026-09-12-DEBT-184-A11Y-MAP` | 2026-09-12 | Plan 184 expanded a11y authority map ACCEPTED: sole UserSettings*; Plan 80 floors; colorblind IN after Path α; remap/AT/audio-desc/cognitive OUT. Docs-only. | `docs/ui/PLAN_184_EXPANDED_ACCESSIBILITY_AUTHORITY_MAP.md` ACCEPTED; `git diff --check` |
| `BATCH-2026-09-12-DEBT-173-RADIO-PANEL` | 2026-09-12 | Phase 3: equipment/prep-cost StartPrep gates + RadioPanel PROGRAM PRODUCTION strip (start/cancel/LastEvent) + bind sites; map UI cite. | `Plan173RadioProgramProductionTests` 10/10; host wiring 1/1; `ArchitectureTestMapGateTests` 5/5; `dotnet build Ashfall.csproj` 0/0 |
| `BATCH-2026-09-12-ARCH-TEST-MAP-GATE` | 2026-09-12 | Restored `bio_fermentation` map coverage; retargeted nuclear/crawler fixture cites; marked surgical_ward test GAP (retired fixture). Doc-only. | `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/ArchitectureTestMapGateTests.cs` (5/5) |
| `BATCH-2026-09-12-DEBT-173-RADIO-HOST` | 2026-09-12 | Phase 2 host/save: session + `radio_program_production` section + daily tick + catalog load + PsyOps StartCampaign + schedule Resolve delivery + content-utilization claim. No RadioPanel. | `Plan173RadioProgramProductionTests` 7/7; `Plan173RadioProgramProductionHostWiringTests` 1/1; `MainTriadDriftGateTests` 7/7; `CampaignEnvelopeBuilderTests` 10/10; `PersistentFilenameRegistryGateTests` 4/4; `dotnet build Ashfall.csproj` 0/0 |
| `BATCH-2026-09-12-DEBT-173-RADIO-PRODUCTION` | 2026-09-12 | Phase 1 Core: `RadioProgramProductionSystem` + `radio_programs.json` consume SlotId / ScheduledBroadcastResult / StartCampaign; Capture/Restore; no UI. | `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/Plan173RadioProgramProductionTests.cs` (7/7) |
| `BATCH-2026-09-12-DEBT-173-RADIO-ADAPTER` | 2026-09-12 | Signed Plan 173 radio adapter map: `RadioProgramSlot.SlotId` bind; `ScheduledBroadcastResult` delivery; `PsyOps.StartCampaign` propaganda input; dedicated production save scope; no second schedule/receiver. | `docs/radio/PLAN_173_RADIO_PROGRAM_ADAPTER_MAP.md` ACCEPTED |
| `BATCH-2026-09-12-DEBT-196-FOOD-SEAM` | 2026-09-12 | Sealed Plan 196 type/temp seam: `AddCohort` food-type gate + host-fed `room_storage_bay` °C decay; Kitchen prepared-meal-only untouched. | `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Plan196FoodTypeTempSeamTests.cs` (8/8); `FoodPreservationSystemTests` 8/8; BioFermentation 20/20; `dotnet build Ashfall.csproj` 0/0 |
| `BATCH-2026-09-12-DEBT-196-FOOD-BOUNDARY` | 2026-09-12 | Signed Plan 196 ownership map: FoodPreservation sole stored cohorts; Kitchen prepared meals only; food-type + host thermal seams named; §3 defaults approved. | `docs/shelter/PLAN_196_FOOD_SPOILAGE_AUTHORITY_MAP.md` ACCEPTED |
| `BATCH-2026-09-12-DEBT-166-169-RELOAD-REPLAY` | 2026-09-12 | Sealed continuous-versus-mid-reload equality for Research, Espionage (+fired set), Fluid, ProceduralNarrative+QuestRuntime with day-keyed campaign RNG; adjacent WT/greenhouse/caravan spot-checks. | `bash scripts/run_test.sh Ashfall.Core.Tests/Plan166_169ReloadReplayTests.cs` (2/2) |
| `BATCH-2026-09-12-DEBT-167-168-CLOSEOUT` | 2026-09-12 | Sealed DEBT-167 (EspionageConsequenceRouter → Caravan supply disruption / PsyOps jamming / FactionWar defense pressure; fired-set save) and DEBT-168 (default fluid topology; WT→Fluid transfer; Greenhouse/Disease delivery applicator; host daily cadence). | `bash scripts/run_test.sh Ashfall.Core.Tests/Plan167EspionageConsequenceRoutingTests.cs` (4/4); `bash scripts/run_test.sh Ashfall.Core.Tests/Plan168WaterDeliveryTests.cs` (5/5); `dotnet build Ashfall.csproj` 0/0 |
| `BATCH-2026-09-12-PARTIAL-IMPLEMENTATION-REBASE` | 2026-09-12 | Rebased the historical partial portfolio on direct owner/save/UI evidence. Removed already-live Plan 187 from the queue; found Plan 184 has persisted but inactive preferences; separated Plan 173 and 196 into explicit interface-boundary work rather than speculative feature work. | `docs/foreman/PARTIAL_IMPLEMENTATION_REBASE.md`; `python3 scripts/ci/generate-docs-index.py --check`; `git diff --check` |
| `BATCH-2026-09-12-PLAN-170-199-FORENSICS` | 2026-09-12 | Audited 30 historical plans by actual owner/API/data/host/test evidence. Classified live equivalents, partial adjacent systems, unstarted work, and blocked proposals; recorded three promotion candidates without authorizing speculative implementation. | `docs/foreman/PLAN_170_199_FORENSIC_AUDIT.md`; `python3 scripts/ci/generate-docs-index.py --check`; `git diff --check` |
| `BATCH-2026-09-12-INTEGRATION-HARDENING` | 2026-09-12 | Exposed the Core-owned Plan 167 sabotage action through its host boundary, added one typed-intent contract test, and moved 134 ignored .NET cache files (52 MiB) into Twin with verified hashes. Deferred 166–169 work is promotion-gated in `KNOWN_DEBT.md`. | `bash scripts/run_test.sh Ashfall.Core.Tests/Plan167EspionageTests.cs` (7/7); `dotnet build Ashfall.csproj --no-restore --nologo`; 134/134 SHA-256 checks; triad gate (7/7) |
| `BATCH-2026-09-12-FOREMAN` | 2026-09-12 | Installed five governance authorities, compacted and synchronized 13 client rulebooks, preserved 14 pre-rollout files with hashes, and replaced same-file racing with ownership. No production code changed. | `python3 scripts/ci/sync-agent-rulebooks.py --check`; archive `SHA256SUMS`; `bash -n scripts/run_test.sh` |
