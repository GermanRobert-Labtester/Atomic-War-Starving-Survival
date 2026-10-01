# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# T05 — Expand a11y self-test to first-hour panels (2026-10-01)

STATUS: APPROVED BY USER
(User mandate: "T05 | Expand a11y self-test to first-hour panels | 561-control
focusability gate; verify the 7 stage panels".)

## Goal

Make the accessibility self-tests cover the seven first-hour onboarding stage
panels a new player is routed to, and verify all seven against the focusability
gate. No new authority, no gameplay change.

## Findings

- The seven stage panels are `WaterTreatmentPanel` (water_treatment),
  `PowerGridPanel` (power_grid), `InventoryPanel` (inventory),
  `DutyRosterPanel` (duty_roster), `DoseLedgerPanel` (dose_ledger),
  `ResearchPanel` (research), `ExpeditionPanel` (expeditions).
- **Blind spot:** the 561-control focusability corpus filters on
  `IBindablePanel`; `PowerGridPanel`, `ResearchPanel`, and `ExpeditionPanel` do
  **not** implement it, so three of the seven stage panels were never audited.
- `--ui-accessibility-selftest`'s representative set (18 panels) contained only
  `DutyRosterPanel`, so the smoke gates did not cover the first-hour routes.
- `WaterTreatmentPanel` has no `Open()` method (the host shows it via
  `ShowPanelLifecycle`), so the a11y harness must make the shown state explicit.

## Changes

- `src/Host/UiAccessibilitySelfTest.cs` — all 7 stage panels added to the
  representative set; new **Gate 6** driven by `OnboardingCatalog.FirstHourOrder`
  (`FirstHourStagePanelNames` route→panel map) that verifies each stage panel
  opens with a focusable entry and readable state; `totalGates` 5 → 6.
- `src/Host/HostCli.Command.RunUiLayoutSelfTest.cs` — `FirstHourStagePanelNames`
  added and folded into the `AuditPanelInteractivity` corpus filter so all seven
  stage panels are focusability-audited (561 → 676 controls; 169 → 172 panels).
- `Ashfall.Core.Tests/UI/FirstHourStagePanelAccessibilityGateTests.cs` — new
  3-test guard pinning the route→panel contract and both self-test coverages.
- `docs/ui/KEYBOARD_FIRST_HOUR_WALKTHROUGH.md`,
  `docs/alpha/FIRST_HOUR_PLAYTEST_KIT.md` — current gate figures + T05 note.

## Non-goals

- No gameplay, Core domain, save-schema, or determinism change.
- No change to the onboarding journey machine or stage order.
- No Unity, no full test suite, no snapshot rebaseline.

## Verification

| Check | Result |
|---|---|
| `--ui-accessibility-selftest` | PASS, 6/6 gates; Gate 6 `7/7` |
| `--ui-layout-selftest` | `panels=172 interactive=676 unreachable=0 panelsWithNoFocus=0 blankUnboundPanels=0`, Failures: 0 |
| `FirstHourStagePanelAccessibilityGateTests` | 3/3 (TDD-proven pre-fix failure) |
| `UiAccessibilityGateTests` | 3/3 |
| `OnboardingWiringGateTests` | 4/4 |
| `generate-selftest-manifest.py --check` | in sync (317 tests) |
| `generate-cli-catalog.sh --check` | in sync (358 entries) |
| `dotnet build Ashfall.csproj` | 0 errors |
| `git diff --check` | clean |

## Integration record (2026-10-01)

All seven first-hour stage panels are now audited by both host self-tests. The
focusability corpus grew from 561 to 676 interactive controls across 172 panels
with `unreachable=0`; `--ui-accessibility-selftest` gained Gate 6 and reports
`7/7`. TDD proof: deleting the `expeditions` route map entry and the
`ExpeditionPanel` focusability entry made the new guard fail 2/3; restored →
3/3. No commit; full suite not run; foreign dirty worktree preserved.
