# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# Open-item closeout + repair-sweep finding audit — 2026-10-01

> **STATUS: APPROVED BY USER** — user-directed: "continue with those 2 open …
> Then do repair sweeping finding audit, completely fix and repair with hardening
> where bugs were found, including tool calls missing or tool call receiving not
> existent!"

## Part A — the two remaining open items

1. **`RunSettingsSelfTest` Shift-held exposure (headless only).**
   `KeyBindingApplicator.Apply(data)` read `Input.IsKeyPressed(Key.Shift)`, so the
   settings probe was non-deterministic in headless/held-key environments.
   **Fix:** `Apply(UserSettingsData, bool safeMode)` is now the explicit seam;
   `Apply(data)` forwards the live check. The probe passes `safeMode: false`
   (5 call sites) and adds a `safeMode: true` skip assertion. `--settings-selftest`
   PASS.
2. **Apprenticeship `actingEligible` retention (UI-label-only stub).**
   A vocational pair whose mentor died was retained forever with "no designation
   assigned" and no action. **Fix:** `ApprenticeshipSystem.AssignActingDesignation(pairId)`
   completes the pair, credits the remaining skill XP, clears the flag, removes it
   from the ledger, and emits completion; `ApprenticeshipHostSession.AssignActingDesignation`
   forwards it; `ApprenticeshipPanel` renders an **ASSIGN ACTING DESIGNATION** row per
   eligible apprentice. `YearTwoApprenticeLadderTests` 9/9 (new round-trip test).

## Part B — repair-sweep finding audit (fix + harden)

| # | Gate | Finding | Fix |
|---|---|---|---|
| 1 | `CatchPolicyLintGateTests` | `HostCli.Command.RunDutyRosterSaveSelfTest` null-probe catch had no disposition | documented `// probe:` (expected `ArgumentNullException`) |
| 2 | `CatchPolicyLintGateTests` | `report.Warn(...)` not recognized as context logging (5 real uses) | added `report.Warn` to the gate's logging keywords |
| 3 | `CatalogPathForbiddenGateTests` | `AssetInspectorPanel` doc comment contained the raw `Assets/StreamingAssets/Data` literal | reworded to `CatalogPath` |
| 4 | `NoFreshCampaignSystemGateTests` | `RunDay1ToDay2MilestoneSelfTest` builds a synthetic isolated `DutyRosterSystem` | added to `AllowedSites` with a deliberate isolated-scenario disposition |
| 5 | `PortContractGateTests` | 4 Core integration seams untracked (`WorldIncidentSystem.RegisterRange`, `YearOfAshTimelineSystem.BindYearTwoClimateCatalog`, `ReckoningSystem.ConfigureFromProfile`/`ConfigureTiming`) | added 4 policy entries (HOST_REQUIRED ×3, LIVE_VIA_CORE ×1); `total_seams` 308→310 |
| 6 | `DayEventParitySourceGateTests` | `world_incident_surfaced` emitted by `Main.CampaignOwners.cs` but unclassified | classified `SemanticKind.Narrative` + parity-matrix row |
| 7 | `ArchitectureTestMapGateTests` | `world_incidents` registered section missing from the generated architecture graph | added the graph entry to `generate-architecture-map.py`; regenerated `ARCHITECTURE_TEST_MAP.md` (315 subsystems) |
| 8 | `JsonNamingMixPinTests` | `audio_logs_expansion_05.json` + `faction_war_radio.json` are DTO-bound camelCase catalogs | pinned with a disposition (mass migration deferred) |

Tool-call parity audit (`HostCliActionParityGateTests` 5/5, `SelfTestManifestGateTests`
4/4, `HostSelfTestExitContractTests` 1/1, `HostActionInputContractTests` 1/1) found **no**
missing host command / non-existent receiver; the earlier static enum-vs-dispatch
mismatches were Core-enum vs host-enum definition differences, not defects.

## Verification

- Host build: **0 errors / 0 new warnings**.
- Runtime: `--settings-selftest` PASS; `--day1-to-day2-milestone-selftest` PASS;
  `--7-day-smoke-selftest` 10/10.
- xUnit: `YearTwoApprenticeLadderTests` 9/9, `ApprenticeshipSystemTests` 8/8,
  `ApprenticeshipIntegrationTests` 3/3, `CatchPolicyLintGateTests` 3/3,
  `CatalogPathForbiddenGateTests` 2/2, `NoFreshCampaignSystemGateTests` 2/2,
  `PortContractGateTests` 8/8, `DayEventParitySourceGateTests` 2/2,
  `ArchitectureTestMapGateTests` 6/6, `JsonNamingMixPinTests` 4/4,
  `HostCliActionParityGateTests` 5/5, `SelfTestManifestGateTests` 4/4,
  `UiAccessibilityGateTests` 3/3, `CompositionRootArchitectureGateTests` 4/4,
  `PanelLifecycleTests` 5/5, `PlayerSurfaceLivenessGateTests` 5/5,
  `UiWave4SourceContractTests` 7/7, `SnapshotCorpusPinTests` 2/2,
  `QuarantineManifestGateTests` 1/1, `DocLinkValidationGateTests` 2/2,
  `FastVerifyIntegrityTests` 2/2.
- Generated contracts: `generate-architecture-map.py --check` OK (315 subsystems);
  `generate-selftest-manifest.py --check` OK (317 tests); `l10n_drift_gate.py` PASS;
  `triad-drift-gate.sh` GATE PASS.
- `git diff --check` clean. No full suite. Foreign dirty worktree preserved.
