# UNBLOCK — Plan 204: Survivor Recruitment & Defection System Full Host Integration

> **Status:** APPROVED & IN INTEGRATION
> **Package:** `UNBLOCK-PLAN204-RECRUITMENT`
> **Target:** Plan 204 (`RecruitmentSystem` in Core)
> **Blockade Solved:** `B-03 — Recruitment is a true Core orphan` (from `CORE_MECHANICS_PLAYER_FACING_PORTFOLIO_PRECISION_FULL_INTEGRATION_HANDOFF.md`)
> **Role:** Integrator (user-authorized unblock & full host integration)
> **Scope Rule:** No partial closeouts. The Survivor Recruitment & Defection System is promoted to full host integration across domain census, persistence, campaign day owner, diagnostic probe, UI presentation panel, and regression verification.

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

## 2. Architectural Boundaries (Rule 5)

- `SurvivorRoster` and `SurvivorsHostSession` own live living survivors, needs, and roster slots.
- `VisitorIntegrationSystem` owns temporary visitor housing and asylum vetting.
- `RecruitmentSystem` owns active recruitment campaigns, wilderness discovery, defection offers, and candidate intake records.
- Deterministic; zero unseeded RNG.
