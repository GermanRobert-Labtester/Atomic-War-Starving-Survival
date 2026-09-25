# Plan P1: Whole-Game Playable Vertical Slice — UI Integration

STATUS: APPROVED BY USER

## 1. Goal & Outcome

**Goal (3 concurrent packages):**

1. **PH-A `WHOLEGAME-P1A-CORE-LOOP-FEEDBACK`** — The player's core action (advance day) produces visible feedback; the consequence ledger persists across saves and resets across campaigns; the save-before-commit window is correct; the crisis HUD has working actions; the duty-roster metric leak is sealed; the dev console is guarded from release builds; the boot path fail-closes instead of leaving a blank window.

2. **PH-B `WHOLEGAME-P1B-PANEL-WIRING-CATALOG`** — Every reachable expanded panel is enrolled in the overlay catalog so Esc closes it (instead of falling through to the menu); a `ShowPanelLifecycle` helper replaces every bare `Visible = true` open so all panels get consistent animation + focus; the shelter interior shows real room assignments instead of stacking everyone in the bunks; the barter panel uses the appraisal skill instead of always reporting 0.

3. **PH-C `WHOLEGAME-P1C-UI-QUALITY-POLISH`** — Dashboard shows current selection; focus-visible style on interactive controls; tooltip coverage for disabled controls; F1 hint correct; Phase0Panel Esc works; dead route cleaned up; modal arrow keys use InputMap actions.

**Non-goals (explicit):**
- No gameplay balance, content, catalog, or data-authority changes.
- No performance optimization, y-sort, sprite fixes, orphan-scene decisions (deferred to `WHOLEGAME-P1D-ADVANCED-PLAYABLE`).
- No universal hardcoded-Esc sweep across all ~100 panels (only the panels touched by PH-C; the rest are P1D).
- No new panels, no new routes, no changes to `PanelRegistryBootstrap.RegisterAll()` route set.
- No full test suite runs. Scoped tests only.
- No commit without this plan present.

**Architectural insight:** The existing panel lifecycle architecture is correct. `PanelRegistryBootstrap` (routes), `ConfigureActions` (bind/open/close), `OverlayPanelCatalog` (Esc + close-all), `RegisterOpenMotionRecursive` (VisibilityChanged → AnimateOpen + EnsureInitialFocus), `UiMotion`, and `AshfallFocusPolicy` form a complete contract. 45+ expanded panels were built outside it. The fix extends the contract with one `ShowPanelLifecycle` bridge, not a new system.

## 2. Claimed Paths & Affected Files

### PH-A (disjoint from PH-B)

**New:**
- `Ashfall.Core.Tests/Flags/ConsequenceLedgerSaveTests.cs`
- `Ashfall.Core.Tests/Campaign/DayAdvanceOrderTests.cs`

**Edited:**
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` — add `consequence_ledger` section + filename
- `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs` — swap persist/commit order
- `src/Main.Lifecycle.cs` — register consequence_ledger lifecycle participant
- `src/Main.Holdfast.cs` — route advance messages through FeedbackPanel
- `src/Main.Application.cs` — move ValidateCatalogs inside try; route countdown
- `src/Main.GameFlow.cs` — move duty-roster subscription to build-time; guard dev console
- `src/Main.SaveOrchestrator.cs` — SaveConsequenceLedger / RestoreConsequenceLedger
- `src/UI/EmergencyResponseHud.cs` — wire 4 action buttons + Esc audit
- `src/UI/CrisisPresentationCoordinator.cs` — add ExecuteAction + auto-eval seam

### PH-B (disjoint from PH-A)

**New:**
- `Ashfall.Core.Tests/UI/PanelCatalogCompletenessTests.cs`

**Edited:**
- `src/Main.PanelLifecycle.cs` — add `ShowPanelLifecycle` helper; expand `OverlayPanelCatalog`
- `src/Main.ExpandedShelterSystems.cs` — replace ~30 bare `Visible = true` with helper
- `src/Main.PlayerSurfaces.cs` — wire shelter/barter sessions; update openActions
- `src/UI/ShelterPanel.cs` — pass duty-roster + assignment sessions to `Initialize`
- `src/UI/ShelterBarterPanel.cs` — bind appraisal skill

### PH-C (after PH-A/B land)

**Edited:**
- `src/UI/GameDashboardPanel.cs` — selection state + F1 hint fix
- `src/UI/Phase0Panel.cs` — Esc handler
- `src/UI/DailyBriefingModal.cs` — arrow key actions
- `src/Main.GameFlow.cs` — dead route `plans_110_113` cleanup
- ~20 `src/UI/*Panel.cs` — focus-visible style application
- ~5 `src/UI/*Panel.cs` — tooltip for disabled controls

### Read-only authorities used
`Ashfall.Core/UI/PanelRegistryBootstrap.cs`, `Ashfall.Core/Flags/CampaignConsequenceLedger.cs`,
`src/UI/FeedbackPanel.cs`, `src/UI/FeedbackMessages.cs`, `src/UI/AshfallFocusPolicy.cs`,
`src/UI/AshfallFocusNavigator.cs`, `src/Host/AshfallInputActions.cs`,
`TEST_POLICY.md`, `AI_AGENT_WORKFLOW.md`.

### Not claimed by this integration
`docs/`, `scripts/`, `Assets/StreamingAssets/Data/`, `Assets/Ashfall.Core/` beyond the two
files listed, `bin/`, `.github/`, all `Next-steps-plans/`, `C-integration-plans/`,
`piagentsplans/`. PH-D (perf/y-sort/orphans/Esc-sweep/controller) is deferred entirely.

## 3. Pre-flight Evidence (measured 2026-09-25)

- Audit report produced 2026-09-25 with 11 critical findings (QA-01..QA-11), 14 visual
  findings, 9 warning rows. Root premise (panel lifecycle bypass) verified in source:
  - `src/Main.PanelLifecycle.cs` catalog = 155 entries; audit found 45 real overlay
    panels absent.
  - `src/Main.ExpandedShelterSystems.cs:510-722` — ~30 cases use `Visible = true`,
    0 use `.Open()`.
  - `src/UI/ShelterPanel.cs:137` — one-arg `Initialize(_survivorsHost)` nulls the
    assignment sources every refresh.
  - `src/UI/ShelterBarterPanel.cs:69` — `_playerAppraisalSkillLevel` never assigned.
  - `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` — no `consequence_ledger` key;
    `src/Main.Lifecycle.cs` — no consequence participant; zero production callers of
    `CampaignConsequenceLedger.CaptureState/RestoreState/ClearAll`.
  - `src/Main.GameFlow.cs:686` — `OnAssignmentChanged +=` inside the open `case`.
  - `src/UI/EmergencyResponseHud.cs:209-217` — only `actionId == "ack"` branch.
  - `src/Main.Application.cs:57` — `ValidateRequiredCatalogs()` outside the `try` at `:66`.
- Build baseline: `dotnet build Ashfall.csproj --no-restore` → 0/0 (pre-existing state;
  re-verify after each package).
- Existing equivalent tests checked: `PanelRouteGateTests` (registry↔switch parity),
  `AccessibilitySourceAuditTests` (focus/timer contracts), `CatchPolicyLintGateTests`.
  No existing test asserts catalog completeness or consequence-ledger persistence — the
  two new test files are not duplicates.

## 4. Implementation Steps (Max 60-100 steps)

### PH-A
1. `SaveSectionRegistry.cs`: add `consequence_ledger` to `All` + `SectionFileNames`.
2. `Main.Lifecycle.cs`: register `DelegateSessionParticipant("consequence_ledger", … onReset: ClearAll)`.
3. `Main.SaveOrchestrator.cs`: add `SaveConsequenceLedger` / `RestoreConsequenceLedger`;
   call restore from `RestoreAllSubsystemsFromDisk`; ensure `SaveAll` captures it.
4. New `ConsequenceLedgerSaveTests.cs`: round-trip, ClearAll, lifecycle reset (4 cases).
5. `CampaignDayCoordinator.cs`: commit (`_lastAdvancedDay`, `Calendar.SetDay`) BEFORE
   `PersistBeforeBriefing`; confirm rollback path still restores from pre-day snapshots.
6. New `DayAdvanceOrderTests.cs`: assert commit precedes persist (2 cases).
7. `FeedbackPanel.cs` (read-only verify) → find public enqueue path; if absent, add
   `EnqueueVisible(Message)` public method.
8. `Main.Holdfast.cs` + `Main.Application.cs`: replace `_statusLabel.Text =` advance
   messages with FeedbackPanel enqueue (countdown persistent, cancel/success/fail toast).
9. `EmergencyResponseHud.cs`: add `OnActionRequested` event + dispatch 4 actionIds.
10. `CrisisPresentationCoordinator.cs`: add `ExecuteAction(string)`; wire from
    `Main.PlayerSurfaces.cs` bind site.
11. Day-advance seam: call `EvaluateCrisisState()` after `CampaignDayCoordinator.Advance()`.
12. `Main.GameFlow.cs:686`: remove `+=` from `case "duty_roster"`; add to build-time
    wiring near `Main.UiPanels.cs:364`.
13. `GameDashboardPanel.cs:537` + `Main.UiPanels.cs:1623`: guard with `OS.IsDebugBuild()`.
14. `Main.Application.cs`: move `ValidateRequiredCatalogs()` + lines 839-889 inside the try.
15. Build both targets; run focused tests; host selftests.

### PH-B
16. `Main.PanelLifecycle.cs`: add `ShowPanelLifecycle(Control)` helper.
17. `Main.PanelLifecycle.cs`: expand `OverlayPanelCatalog()` with the ~45 missing panels
    (name list in §2 of the audit; verify each field exists on Main before adding).
18. `Main.ExpandedShelterSystems.cs`: replace every `Visible = true` open with
    `ShowPanelLifecycle(...)` (preserve `RefreshView()` calls).
19. `Main.PlayerSurfaces.cs`: update `ConfigureActions` `openAction:` lambdas that use
    `Visible = true` to use `ShowPanelLifecycle(...)`.
20. `ShelterPanel.cs`: extend `Bind` (or add setters) to receive duty-roster +
    shelter-assignment hosts; pass them to `Initialize(_survivorsHost, dutyRoster, assignments)`.
21. `Main.PlayerSurfaces.cs` / shelter bind site: pass the additional sessions.
22. `ShelterBarterPanel.cs`: bind appraisal skill in `Bind(...)`.
23. Wire barter bind site to pass survivor skill.
24. New `PanelCatalogCompletenessTests.cs`: every `ConfigureActions` route id is either
    in catalog, explicitly whitelisted (dashboard/menu/hud), or documented exclusion.
25. Build both targets; run focused tests; host selftests.

### PH-C
26. `GameDashboardPanel.cs`: `_selectedRouteId` + highlight active nav button; clear on menu.
27. Apply `AshfallFocusPolicy.ApplyFocusVisibleStyle` to dashboard + top-5 panels' buttons.
28. Tooltip sweep for disabled controls (5 priority panels).
29. `GameDashboardPanel.cs:686`: fix F1 hint text.
30. `Phase0Panel.cs`: add `_UnhandledInput` with `IsCloseOrCancel`.
31. `Main.GameFlow.cs`: remove dead `plans_110_113` route (unregister + remove case).
32. `DailyBriefingModal.cs`: replace hardcoded arrows with InputMap actions (or document).
33. Build both targets; run focused tests; accessibility selftest.

## 5. Verification

- [ ] `dotnet build Ashfall.csproj --no-restore` → 0 warnings / 0 errors (after each PH)
- [ ] `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore` → 0/0
- [ ] Scoped tests via `bin/run-scoped-tests` (<30s) per package
- [ ] `bash scripts/run_test.sh Ashfall.Core.Tests/Flags/ConsequenceLedgerSaveTests.cs` → 4/4
- [ ] `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/DayAdvanceOrderTests.cs` → 2/2
- [ ] `bash scripts/run_test.sh Ashfall.Core.Tests/UI/PanelCatalogCompletenessTests.cs` → PASS
- [ ] `bash scripts/run_test.sh Ashfall.Core.Tests/UI/PanelRouteGateTests.cs` → 20/20 (no regression)
- [ ] `bin/ashfall-dev panel-bind-lifecycle-selftest` → PASS
- [ ] `bin/ashfall-dev data-integrity-selftest` → PASS (0 errors)
- [ ] `bin/ashfall-dev ui-accessibility-selftest` → PASS (PH-C)
- [ ] Max 10-15 test-edit steps per failure before auto-flagging in `.ai/state.md`
- [ ] Manual runtime checks listed per package in the plan body (headless cannot verify
      focus/animation/toasts — mark as runtime-required)

## 6. Known Limitations / Debt

- Occupant overflow at ≥9 per room and synchronous `Free()` re-entrancy in hotspot clicks
  are pre-existing and NOT fixed here (bounded by starting roster of 3; tracked as P1D).
- The remaining ~55 panels with hardcoded `Key.Escape` are NOT converted in PH-C; full
  sweep is P1D.
- FeedbackPanel countdown update may need a small API addition (`UpdateToastText`) — if the
  panel API resists, fall back to re-enqueue with dedup suppression.
- `OpenWithFocus`/focus-restoration dead path remains (activation is a P1D design decision).
