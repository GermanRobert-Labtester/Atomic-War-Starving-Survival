# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# Plan 194 — Emergency Alert & Warning System — Full Host Integration

**STATUS: APPROVED BY USER**
**Authorized by:** user directive in session ("instead of committing anything move on to the next plan!") — 2026-09-26.
**Claim:** `claim-plan194-emergency-alert-integration-2026-09-26`
**Signed design authority:** `DEC-184` (Emergency Alert prioritization / evacuation protocol, `SIGNED`), anchor `C2[?]`.

## Bounded outcome

Convert the Core `EmergencyAlertSystem` (committed, tested, but a true host
orphan) into a fully wired host feature: authored catalog + checksummed save
section + phase-5 campaign day clock + lifecycle + diagnostic probe + a
truthful read-only dashboard surface. No second threat authority: the alert
system reports on facts other owners produce (weather, defense, disease, power,
water); it only prioritizes, tracks response windows, records evacuation
protocol state, and logs resolution.

## Premise audit (evidence-first)

- `EmergencyAlertSystem` (`Assets/Ashfall.Core/Emergency/EmergencyAlertSystem.cs`)
  has **0 references in `src/`**.
- `Assets/StreamingAssets/Data/emergency_alerts.json` exists (8 alert types,
  schema_version 1) but no host consumer. `LoadCatalog(string json)` is lenient
  and already tested by `Ashfall.Core.Tests/Emergency/Plan194EmergencyAlertIntegrationTests.cs`.
- No `emergency_alert` save section, no CLI probe, no UI readout, no day owner.
- The nearby `BroadcastGenre.EmergencyAlert` (radio) is an unrelated enum member,
  not this system. The `EmergencyResponseHud` binds `CrisisPresentationSnapshot`
  and is a separate crisis presenter — not reused or overloaded.
- Authority boundaries (Rule 5): threat owners remain sovereign; this system
  owns only active alert records, response windows, escalation, and evacuation
  protocol state.

## Files changed

- Core: `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (`emergency_alert`
  section + `emergency_alert_save.json`), `Assets/Ashfall.Core/HostCliRegistry.cs`
  (`EmergencyAlertSelfTest` + descriptor), `Assets/Ashfall.Core/Campaign/DayEventVocabulary.cs`
  (`emergency_alert_ticked` heartbeat).
- Host: `src/Host/EmergencyAlertHostSession.cs` (new; `EmergencyAlertSaveStore` +
  `SaveStoreHub.Checksummed<EmergencyAlertState>`, catalog loader),
  `src/Main.EmergencyAlerts.cs` (new; setup/save/flush/reset, raise/ack/resolve,
  evacuation protocols, day tick, readout), `src/Host/HostCli.EmergencyAlert.cs`
  (new, 12 checks), `src/Host/HostCli.cs` (enum/parse/help),
  `src/Main.Application.cs` (dispatch), `src/Main.SaveOrchestrator.cs`
  (setup/save), `src/Main.Lifecycle.cs` (reset), `src/Main.CampaignOwners.cs`
  (phase-5 `EmergencyAlertDayOwner`), `src/UI/GameDashboardPanel.cs` +
  `src/Main.GameFlow.cs` (read-only alert card on the dashboard).
- Tests: `Ashfall.Core.Tests/Emergency/Plan194EmergencyAlertHostIntegrationTests.cs`
  (new), `Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`
  (section pin 272→273).
- Generated/docs: `scripts/ci/generate-architecture-map.py` (+1 node),
  `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md` (+1 row), and the owning
  generators' outputs.
- Governance: this plan, `docs/plans/UNBLOCK_PLAN194_EMERGENCY_ALERT_INTEGRATION_PLAN.md`,
  `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `docs/governance/DECISION_REGISTER.md`.

## Acceptance

1. Core/tests/host builds 0 errors / 0 new warnings (concurrent-package warnings excluded/attributed).
2. `Plan194EmergencyAlertHostIntegrationTests` 8/8 + existing
   `Plan194EmergencyAlertIntegrationTests` green.
3. `--emergency-alert-selftest` 12/12 headless.
4. Save round-trip preserves active alerts, history, and protocols; section pin updated.
5. `emergency_alert_ticked` is classified and present in the semantic parity matrix.
6. Generated `--check` gates pass (architecture map, save-store matrix, CLI
   catalog, selftest manifest, catalog registry, plan-integration audit).
7. Plan marked FULLY INTEGRATED and archived.
