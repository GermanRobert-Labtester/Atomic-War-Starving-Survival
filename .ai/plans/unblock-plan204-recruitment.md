# UNBLOCK PLAN 204 — SURVIVOR RECRUITMENT & DEFECTION SYSTEM FULL INTEGRATION

> **STATUS: APPROVED BY USER**
> **Package:** `UNBLOCK-PLAN204-RECRUITMENT`
> **Target:** Plan 204 — Survivor Recruitment & Defection System (`RecruitmentSystem` in Core)
> **Blockade Solved:** `B-03 — Recruitment is a true Core orphan` (from `CORE_MECHANICS_PLAYER_FACING_PORTFOLIO_PRECISION_FULL_INTEGRATION_HANDOFF.md`)
> **Role:** Integrator (user-authorized unblock & full host integration)
> **Zero Partials Directive:** 100% full host integration: Core census, save store, host session, Main partial, SaveOrchestrator, lifecycle reset, campaign day owner, diagnostic probe, UI presentation panel, route registration, and scoped verification.

---

## 1. Executive Summary & Objective

Promote `RecruitmentSystem` from a true Core orphan (0 references in `src/`) to a fully integrated, save-safe, deterministic, and player-operable host feature in ASHFALL.

1. **Domain Authority (`Assets/Ashfall.Core/Survivors/RecruitmentSystem.cs`):**
   - Add `RecruitmentCensus` read model (`ActiveCampaigns`, `KnownCandidates`, `TotalRecruited`, `PendingOffers`, `TotalEvents`).
   - Expose `GetCensus()`.
2. **Save Persistence:**
   - Register section `recruitment` in `SaveSectionRegistry.cs` (file: `recruitment_save.json`, domain: `survivors`, lifecycle group: `ExpandedShelterLifecycleGroup`).
   - Implement `RecruitmentSaveStore` using `SaveStoreHub.Checksummed<RecruitmentState>`.
3. **Host Session & Main Partial:**
   - Implement `RecruitmentHostSession` in `src/Host/RecruitmentHostSession.cs`, loading `recruitment_templates.json`.
   - Implement `src/Main.Recruitment.cs` with setup, save, reset, flush, and operation methods.
   - Wire into `src/Main.SaveOrchestrator.cs` (`RestoreAllSubsystemsFromDisk`, `SaveAllDirect`) and `src/Main.Lifecycle.cs` (`ResetLateWaveIntegrationSessions`).
4. **Campaign Day Owner:**
   - Implement `RecruitmentDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore` in `src/Main.CampaignOwners.cs` (Phase 5).
   - Register in `RegisterProductionCampaignOwners()`.
   - Classify `recruitment_ticked` as `SemanticKind.Heartbeat` in `DayEventVocabulary.cs`.
5. **CLI Diagnostic Probe:**
   - Add `HostCliAction.RecruitmentSelfTest` and flag `--recruitment-selftest` in `HostCliRegistry.cs` and `src/Host/HostCli.cs`.
   - Implement 12-check diagnostic probe in `src/Host/RecruitmentSelfTest.cs`.
   - Update `docs/ci/SELFTEST_MANIFEST.json`.
6. **UI Presentation Panel:**
   - Implement `src/UI/RecruitmentPanel.cs` with active campaigns, candidate roster, defection offers, and controller/keyboard support.
   - Register route `recruitment` in `PanelRegistryBootstrap.cs` and `Main.PanelLifecycle.cs`.
7. **Verification & Quality Gates:**
   - Run scoped tests via `bin/run-scoped-tests`.
   - Verify `--recruitment-selftest` passes 12/12.
   - Verify host compiles cleanly with 0 warnings / 0 errors.

---

## 2. Implementation Steps

1. Update `RecruitmentSystem.cs` with `RecruitmentCensus` and `GetCensus()`.
2. Update `SaveSectionRegistry.cs` with `recruitment` section.
3. Update `DayEventVocabulary.cs` with `recruitment_ticked`.
4. Create `src/Host/RecruitmentSaveStore.cs` and `src/Host/RecruitmentHostSession.cs`.
5. Create `src/Main.Recruitment.cs`.
6. Wire `src/Main.SaveOrchestrator.cs`, `src/Main.Lifecycle.cs`, and `src/Main.CampaignOwners.cs`.
7. Create `src/Host/RecruitmentSelfTest.cs` and wire `HostCli.cs` / `HostCliRegistry.cs` / `Main.Application.cs`.
8. Create `src/UI/RecruitmentPanel.cs` and wire route in `Main.PanelLifecycle.cs` / `PanelRegistryBootstrap.cs`.
9. Update `scripts/ci/generate-architecture-map.py` (including missing `consequence_ledger` and new `recruitment`) and regenerate.
10. Update tests in `Ashfall.Core.Tests/` and run `bin/run-scoped-tests`.
11. Run `--recruitment-selftest` via headless Godot.
12. Mark both plan files `FULLY INTEGRATED` multiple times and move them immediately to `integrated/survivors/`.
