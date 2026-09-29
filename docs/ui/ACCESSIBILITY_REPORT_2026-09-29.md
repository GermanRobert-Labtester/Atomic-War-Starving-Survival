# ASHFALL UI accessibility and usability audit — 2026-09-29

Read-only audit of `src/UI/` (~267 files), `src/Main*.cs` seams, and
`Assets/Ashfall.Core/UI/Theme.cs`. Method: two static read-only sweeps
(colors/typography/overflow; input/focus/lifecycle) plus WCAG contrast
computation from the canonical float tuples. No UI code was edited.

Relationship to prior audits:

- Supersedes [ACCESSIBILITY_REPORT.md](ACCESSIBILITY_REPORT.md)
  (2026-09-05) — several of its HIGH findings are now fixed: the `Dim`
  token was raised to 6.0:1, the class-default theme
  (`AshfallUiTheme.cs`) landed 2026-09-25, the Esc→IsCloseOrCancel sweep
  closed 2026-09-26, and grid rows gained keyboard selection
  (`AshfallDataGrid.cs:281`). The old file is retained for history and
  carries a superseded banner.
- Does not redo the sealed 2026-09-25 visual audit (C1–C48,
  `UI_UX_AUDIT_2026-09-25.md`). This audit covers contrast hygiene,
  typography, input, and lifecycle — dimensions that audit did not rank.

## 1. Theme token contrast (computed, current tuples)

Luminance from the float RGB tuples in `Assets/Ashfall.Core/UI/Theme.cs`
(sRGB linearization, WCAG formula). Thresholds: 4.5:1 body, 3:1 large
text/UI components. Background is opaque Ink (0.035, 0.043, 0.047),
relative luminance 0.0032. Translucent panels composite lighter (see §3),
which only *lowers* these ratios; the table is the best case.

| Foreground (tuple) | On Ink | Verdict |
|---|---:|---|
| Pale (0.902, 0.878, 0.824) | 14.98:1 | Pass |
| Hot (0.957, 0.784, 0.459) | 12.56:1 | Pass |
| Success (0.361, 0.839, 0.439) | 10.62:1 | Pass |
| Warm (0.827, 0.667, 0.384) | 9.11:1 | Pass |
| Info / Lethe (0.431, 0.639, 0.659) | 7.01:1 | Pass |
| Muted (0.576, 0.561, 0.518) | 6.11:1 | Pass |
| Dim (0.557, 0.561, 0.510) | 6.02:1 | Pass (was 3.45:1 in the 2026-09-05 audit; fixed) |
| Critical (1.0, 0.322, 0.322) | 6.19:1 | Pass |
| Warning / Entropy (0.788, 0.482, 0.227) | 5.99:1 | Pass |

**The token palette passes WCAG AA everywhere.** All contrast failures
below come from hardcoded colors that bypass the tokens, or from
translucent stacking.

## 2. Hardcoded colors bypassing tokens

### 2a. One real contrast failure

| Evidence | Value | Use | Ratio | Issue |
|---|---|---|---:|---|
| `src/UI/GameDashboardPanel.cs:327` | `Modulate (1,1,1,0.22)` | deselected travel-route icon | ≈1.9:1 | Fails the 3:1 UI-component minimum so far that selected vs deselected is hard to distinguish for low-vision players |

### 2b. Marginal / tonal drift

| Evidence | Value | Use | Ratio on Ink | Issue |
|---|---|---|---:|---|
| `src/UI/GeigerCalibrationPanel.cs:223,227,235`, `src/UI/SafeCrackModal.cs:190`, `src/UI/BrineExtractionPanel.cs:215-219` | `Colors.Red` (1,0,0) | alarm-state label text | 4.94:1 | Barely passes 4.5:1 and only on opaque Ink; the Critical token (6.19:1) exists and reads better. `Colors.White` alongside it is also a token bypass |
| `src/UI/TriangulationPanel.cs:184,204,212` | `Colors.White` / `Colors.Green` / `Colors.Yellow` | discovery status text | 18.4 / 14.4 / 17.4 | Contrast passes; pure primaries break the restrained ash-palette tone (rule: restrained, human tone) |
| 12+ files, e.g. `src/UI/TimeCapsulePanel.cs:201,226,266`, `src/UI/SurvivorDeathLegacyPanel.cs:126,174`, `src/UI/RelationshipDecayPanel.cs:127,165`, `src/UI/PersonalQuestPanel.cs:130,192` | `(0.6,0.6,0.6)` | empty-state / metadata text | 6.93:1 | Passes but duplicates `Dim` with drift; `Dim` was itself a documented contrast fix — these sites predate it and never moved |
| `src/UI/ShelterPanel.cs:158-161` | `(0.9,0.9,0.9)` / `(0.83,0.67,0.38)` / `(0.43,0.64,0.66)` / `(0.58,0.56,0.52)` | MakeDataRow palette | ~15/9/7/6 | Re-derived Pale/Warm/Info/Muted by hand; will drift when tokens change |

### 2c. Semantic accents re-derived instead of tokens (contrast OK today, drift risk)

`src/UI/EmergencyResponseHud.cs:116-119,167,187,243` (crisis severity
reds/oranges/whites), `src/UI/SaveLoadPanel.cs:76,130,171,292`
(error `(1,0.4,0.4)`, success `(0.53,1,0.67)`, destructive button text
`(1,0.45,0.4)`), `src/UI/SurvivorDeathLegacyPanel.cs:138,147,154,186`,
`src/UI/TimeCapsulePanel.cs:213,278`, `src/UI/RelationshipDecayPanel.cs:140,178`,
`src/UI/PersonalQuestPanel.cs:154,210`, `src/UI/ShelterDecorPanel.cs:422`.
All currently measure ≥6:1 on opaque Ink; the problem is one-palette
authority, not today's ratios.

### 2d. Panel backgrounds: ~59 hand-rolled `Ink` re-derivations

Every dense panel builds its scrim as a literal, e.g.
`(0.04,0.05,0.06,0.88)` in `InventoryPanel.cs:169`,
`SurvivorsPanel.cs:204`, `StatusPanel.cs:293`, ledger/trade panels at
0.90–0.92, `SettingsPanel.cs:171` at `(0.02,0.02,0.03,0.92)` (darker
than Ink), tinted variants (`ExpeditionPanel.cs:1373` amber 0.94,
`BlackProjectsArchivePanel.cs:330` red 0.85,
`EmergencyResponseHud.cs:125-126` crisis red 0.95 vs normal 0.95).
Effective text contrast on these is stack-dependent (§3). Consolidating
on `Theme.InkPanel`/`BackdropOverlay` tokens is the single largest
hygiene win in the tree.

## 3. Text on translucent / no-panel backgrounds (stack-dependent contrast)

- `src/UI/MapDetailPanel.cs:227-229` — scene backdrop scrim alpha
  **0.74** over `BackdropArt`; every label in the panel renders through
  generated art. Lowest-alpha panel scrim in the codebase.
- `src/UI/GameOverPanel.cs:30-90` — title/body/stats/hint labels have
  **no backing panel at all**; they sit on the background carousel's
  0.80 black overlay over photos. The 11px hint (`:87`) is most at risk
  on light photo regions.
- `src/UI/MainMenuPanel.cs:85,141,189` — carousel overlay **0.55** with
  11px status/version labels; same exposure.
- `src/UI/ExpeditionPanel.cs:171,1294,1373` — screen-level art at 0.82,
  encounter scrim 0.85, banner 0.94; contrast depends on art beneath.
- Compound translucent chrome: sidebar rows Ink 0.40
  (`AshfallSidebar.cs:114`) inside rail Ink 0.55 (`AshfallStatusRail.cs:32`)
  inside metric cards Ink 0.72 (`AshfallMetricCard.cs:45`) — effective
  background varies with the host panel, so the §1 "on Ink" table is a
  best case for sidebar hints and metadata.
- `src/UI/GameHudOverlay.cs:155-158` — HUD meters (Ink 0.9/0.92) over
  the live world view.

Acceptance direction: re-derive effective backgrounds per backdrop with
existing snapshot renders before approving any of these as pass/fail;
the fix (raising scrim alpha to ≥0.9 or tokenizing) should be chosen
per-surface, not globally.

## 4. Typography

No font size below 11 exists and no literal <12 override exists
(smallest literal is 12, `WaystationNetworkPanel.cs:79`). The risk is
concentrated in the **11px tier**:

- `Theme.FontSizeLabel = 11` (`Assets/Ashfall.Core/UI/Theme.cs:176`).
- `AshfallUiHelpers.MakeMetadata` (11px, `AshfallUiHelpers.cs:205-216`)
  — **282 call sites** across `src/UI`: empty states, row metadata,
  caravan schedule pills, demand lines, event-log lines.
- `AshfallUiHelpers.MakeLabel` (11px) — `GameDashboardPanel.cs:757`
  gauge names pinned to 74px width, `ShelterDecorPanel.cs:89,93,329`.
- Direct 11px content: main-menu status/version
  (`MainMenuPanel.cs:141,189`), game-over hint (`GameOverPanel.cs:87`),
  every status-rail metric card label (`AshfallMetricCard.cs:67`),
  save-slot name/hint (`SaveLoadPanel.cs:129,369`), footers of
  weather/event panels (`WeatherForecastPanel.cs:343`,
  `WeatherHistoryPanel.cs:185`, `EventsLogPanel.cs:177`).
- Precedent: two 11→12 raises already shipped with comments
  (`AshfallDataGrid.cs:426-430`, `AshfallSidebar.cs:182-184`).

12px (`MakeSmall`, 203 call sites) is the de facto dense-content size;
13px mono carries right-aligned grid numerics. Contrast passes at these
sizes, but 11px BarlowCondensed is at the floor for sustained reading
(ledgers, dose registers). Raising `FontSizeLabel` to 12 would lift 282+
sites with one token edit; the two shipped precedents set the pattern.

## 5. Input, hotkeys, and focus

### 5a. Hotkey collisions

| Key | Consumers | Status |
|---|---|---|
| 1–5 (raw) | CombatPanel actions (`CombatPanel.cs:429-476`), MoralChoiceModal quick-select (`MoralChoiceModal.cs:300-325`), journal tabs (`project.godot:138-162`) | **Live collision:** `OpenMoralChoiceModal` (`src/Main.UiHandlers.cs:227-263`) does not call `CloseAllOverlayPanels()`, unlike every `OpenPlayerPanel` route (`src/Main.GameFlow.cs:362`), so combat panel and moral modal can be visible together; tree order decides the winner |
| Tab | `ashfall_next_tab` (combat target cycle `CombatPanel.cs:419-428`, briefing skip `DailyBriefingModal.cs:173-178`) vs engine `ui_focus_next` | **Dead-zone:** when any control holds focus, the viewport consumes Tab before `_UnhandledInput`, so the panel's own "[Tab] Cycle Target" hint (`CombatPanel.cs:393`) usually never fires |
| J | `ToggleJournal` (`src/Main.Application.cs:1251-1258`) | **Not state-gated** — unlike every sibling hotkey, no `_state == Playing` guard; J opens the journal over the main menu |
| Esc | per-panel `IsCloseOrCancel` + global sweep (`Main.Application.cs:1294-1309`) + raw-Esc panels (`VerdictPanel.cs:43-52`, `SettingsPanel.cs:130`, `ConfirmationModal.cs:134`) + **crisis HUD `_Input`** (`EmergencyResponseHud.cs:294-301`) | The crisis HUD's `_Input` runs before GUI/unhandled phases, so with a modal stacked over the HUD, Esc closes the HUD, not the top modal |
| 1 / Space (advertised) | Crisis shortcut labels from `CrisisPresentationCoordinator.cs:228-236,281-296` | **Dead affordance:** `EmergencyResponseHud.cs` handles only Esc; the advertised keys do nothing |

### 5b. Focus navigation

- Nav mechanism is sound: spatial nearest-neighbor via
  `AshfallFocusNavigator.MoveDirection` (`AshfallFocusNavigator.cs:24-83`)
  with grid-band preference and first-focusable pickup.
- **Modal focus trap is unwired:** `AshfallFocusPolicy.TrapFocus`
  (`AshfallFocusPolicy.cs:143-179`) and `OpenWithFocus` (`:117-137`) are
  called only from `ModalManager.HandleInput` (`ModalManager.cs:78`),
  and `ModalManager` is instantiated nowhere in the live app (only a
  self-test, `src/Audio/AudioSelfTest.cs:1087`); the Core
  `ModalStackController` likewise has no driver. Tab cycling is
  therefore unbounded — it can leave an open overlay into background
  chrome. Same root cause as the 2026-09-05 report's overlay-detection
  finding, now with the dead-code seam identified.
- **Nav scope is the whole Main tree** (`Main.Application.cs:1290-1293`):
  arrow/D-pad navigation is not restricted to the open panel, so focus
  can travel into dashboard controls behind an overlay.
- **Stacked-panel focus restore is broken:** during
  `CloseAllOverlayPanels` (`src/Main.PanelLifecycle.cs:242-263`), panel A
  is hidden before panel B's restore check runs, so B's opener
  (`opener.Visible` guard, `AshfallFocusPolicy.cs:184-190`) fails and
  nothing is restored; multiple panels also enqueue competing deferred
  `GrabFocus` calls (last-enqueued wins). Single-panel close restores
  correctly.
- **Dead code:** `AshfallFocusNavigator.TickStickRepeat`
  (`AshfallFocusNavigator.cs:110-130`) has zero callers — held-stick
  repeat never engages.
- **Not keyboard-reachable at all:** AshfallSidebar nav rows are
  PanelContainer+Label with mouse-only `GuiInput`
  (`AshfallSidebar.cs:167-174`); RichTextLabel meta deep-links in the
  daily briefing (`DailyBriefingModal.cs:224-231`) have no keyboard
  activation path.

### 5c. Focus visibility

Theme class defaults cover Button (+CheckBox/CheckButton/OptionButton
inheritance), LineEdit, TextEdit, ItemList, and Slider-by-grabber
(`AshfallUiTheme.cs:69-181`). Remaining gaps:

- **SpinBox** (13 instantiations, e.g. `BlackMarketPanel.cs:178,241`,
  `ShelterBarterPanel.cs`): no theme entries; only the inner LineEdit
  gets a ring, spin arrows have no focus/hover styling.
- Grid rows and sidebar rows rely on per-control styles
  (`AshfallDataGrid.cs:281`) — present, but sidebar rows are
  unreachable anyway (§5b).

### 5d. Mouse target sizes

- `MakeButton` default height is `FontSizeBody + SpacingMd` = **27px**
  (`AshfallUiHelpers.cs:532`) — 1px under the ~28px comfort threshold,
  width unconstrained.
- ~52 interactive controls sit under 28px height. Smallest:
  ShelterBarter plus/minus **24×22** (`ShelterBarterPanel.cs:745,765,896,916`);
  FeedbackPanel close 24×24 (`:197`); InventoryPanel row buttons
  64×24/82×24 (`:100,108`); PowerGridPanel 56×24/60×24 (`:140-160,269`)
  and 26px action buttons (`:213-244`); six row-embedded selects at
  0×24 (`DecontaminationPanel.cs:244`, `MedicalWardPanel.cs:255`,
  `TravelingCaravanPanel.cs:310`, `ChemicalDependencyPanel.cs:276`,
  `LibraryStudyPanel.cs:240`, `GreenhousePanel.cs:650`); OptionButtons
  0×26 (`FungiCultivationBedPanel.cs:272,274`,
  `RoboticsWorkshopPanel.cs:211`).

## 6. Overflow risks (fixed widths + variable text)

- **AshfallDataGrid cells have no clip, wrap, or ellipsis**
  (`AshfallDataGrid.cs:410-421`) and horizontal scroll is disabled
  (`:128-131`) — a long survivor name or role silently overflows its
  column. Callers pin narrow MinWidths: DutyRosterPanel 180/200/140
  (`DutyRosterPanel.cs:129-134`), DoseLedgerPanel 200/130 (`:336-340`).
- `SurvivorsPanel.cs:149` — localized display names in a 140px
  MakeSmall label, no wrap/clip.
- `src/Economy/TradeScreenGodotPanel.cs:408,814,841` — item names in
  100/120/140px labels, vertical-only scroll.
- `AshfallDashboardShell.cs:79-90` — H2 shell title is ExpandFill with
  no clip; a long title runs into the right-docked close button
  (e.g. fixed 1100×720 shell in `CaravanBarterLedgerPanel.cs:163-167`).
- `AshfallMetricCard.cs:41-42` — mono value text has no clip on cards
  as narrow as 110px.
- `GameDashboardPanel.cs:757-758` — 11px gauge labels pinned to 74px.
- Positive: recipe/description/body text paths use WordSmart autowrap
  (`KitchenNutritionPanel.cs:251-253`, BlackMarket/ShelterBarter/GameOver
  body labels).

## 7. Hover feedback gaps

Buttons are fully covered (theme hover stylebox + motion FX on every
Button). Gaps: AshfallDataGrid rows have zero `MouseEntered` handling
(`AshfallDataGrid.cs` — styling is selection-state only); AshfallSidebar
rows likewise (`AshfallSidebar.cs:178-200`); ItemList has no per-item
hover affordance in the theme; RichTextLabel links fall back to engine
defaults; SpinBox arrows are unstyled. Only 2 files in `src/UI` use
`MouseEntered` at all.

## 8. Modal layering / critical alerts

- The crisis HUD (`EmergencyResponseHud`) is added mid-build
  (`src/Main.UiPanels.cs:1417`), so lazily created panels
  (`src/Main.Onboarding.cs:88-97`, `src/Main.Campaign.cs:113-120`) draw
  above it; there is no z-order mechanism anywhere (`CanvasLayer`/
  `ZIndex`: zero uses in src/).
- The HUD is **absent from `OverlayPanelCatalog()`**
  (`src/Main.PanelLifecycle.cs:17-225`): `AnyOverlayPanelOpen` and
  `CloseAllOverlayPanels` never account for it, and opening a player
  panel does not hide or re-raise it.
- Severity gate: the HUD surfaces only when
  `snap.Severity >= CrisisSeverity.Severity` (`Main.UiPanels.cs:1424-1429`)
  — lower-severity crisis snapshots never render anywhere.
- Core side aggregation is healthy: single prioritized snapshot
  (`CrisisPresentationCoordinator.cs:148-506`).
- Net effect: a critical alert can be obscured by any panel opened
  afterward, and its advertised keyboard shortcuts are inert (§5a).

## 9. Ranked fixes

> **Implementation status (2026-09-29):** items 1, 2, and 6 are implemented
> (plan `.ai/plans/ui-a11y-p1-input-correctness-2026-09-29.md`, gate test
> `Ashfall.Core.Tests/UI/UiA11yP1InputGateTests.cs`). Item 2's catalog
> membership was replaced by a re-raise in `CloseAllOverlayPanels` — adding
> the HUD to the catalog would close (and lose) an active crisis alert on
> every panel switch. The advertised Core `Shortcut` strings were left
> dormant: the HUD never reads them, Space already activates the focused
> ack button, and digit wiring would create a new collision with
> CombatPanel 1–5.

**P1 — correctness of input/alerts (player can lose commands or miss crises):**

1. Route `OpenMoralChoiceModal` through the exclusive-open seam
   (`CloseAllOverlayPanels`) or make MoralChoiceModal consume digits
   while visible — removes the live 1–5 double-consumption with
   CombatPanel. (`src/Main.UiHandlers.cs:227`)
2. Add the crisis HUD to `OverlayPanelCatalog()` and demote its Esc
   handler from `_Input` to the unhandled phase so stacked modals close
   first; decide and implement the advertised 1/Space shortcuts or stop
   advertising them. (`src/Main.PanelLifecycle.cs`,
   `src/UI/EmergencyResponseHud.cs:294`,
   `src/UI/CrisisPresentationCoordinator.cs:228`)
3. Wire the existing `ModalStackController`/`ModalManager` trap seam (or
   scope `AshfallFocusNavigator` to the open panel) so Tab and arrows
   cannot leave an open overlay. (`src/UI/ModalManager.cs:78`,
   `AshfallFocusPolicy.cs:143`, `src/Main.Application.cs:1290`)
4. Fix stacked-panel focus restore: check `opener.Visible` deferred, or
   restore from the panel below instead of the opener. (`AshfallFocusPolicy.cs:184`)

**P2 — reachable and readable:**

5. Make AshfallSidebar nav rows keyboard-reachable (focusable buttons,
   not mouse-only PanelContainers). (`AshfallSidebar.cs:167`)
6. Add the `_state == Playing` guard to the J hotkey. (`src/Main.Application.cs:1251`)
7. Raise `Theme.FontSizeLabel` 11→12 (one-token edit lifting 282+
   sites; two shipped precedents). (`Assets/Ashfall.Core/UI/Theme.cs:176`)
8. Replace the `Colors.Red`/`Colors.White` alarm accents with
   Critical/Pale tokens; sweep the §2b/§2c hardcoded accents onto
   tokens (mechanical, no visual change except Triangulation's pure
   green/yellow). (`GeigerCalibrationPanel.cs:223`, `TriangulationPanel.cs:184`)
9. Add clip/ellipsis + TextOverrunBehavior to AshfallDataGrid cell
   labels and shell titles (overflow becomes invisible truncation
   instead of column bleed). (`AshfallDataGrid.cs:410`,
   `AshfallDashboardShell.cs:79`)
10. Bump the ~52 sub-28px controls to ≥28px height, starting with
    ShelterBarter 24×22 and the six 0×24 row selects. (`ShelterBarterPanel.cs:745` et al.)
11. Lift the deselected-route icon modulate from 0.22 to a ≥3:1 state. (`GameDashboardPanel.cs:327`)

**P3 — hygiene and polish:**

12. Consolidate the ~59 hand-rolled panel scrims onto InkPanel/
    BackdropOverlay tokens; verify MapDetailPanel (0.74), GameOverPanel
    (no panel), and MainMenuPanel (0.55) text against real backdrops
    with snapshot renders before/after.
13. Add hover states to grid rows, sidebar rows, ItemList items, and
    SpinBox; style SpinBox focus in the theme.
14. Wire or delete `TickStickRepeat` and the RichTextLabel link keyboard
    path. (`AshfallFocusNavigator.cs:110`, `DailyBriefingModal.cs:224`)

## 10. Verification notes

- Contrast values computed from source tuples, not hex constants (hex
  constants in `Theme.cs` are known-stale; see the 2026-09-05 report's
  token-drift table — tuples remain the render authority).
- No fresh runtime captures were taken; §3 stack-dependent findings
  need per-backdrop snapshot renders before being promoted to pass/fail.
- Findings cite file:line as of branch `integration/all-latest-2026-09-24`
  on 2026-09-29; paths are unowned (no WORKTREE_OWNERSHIP claims touched).
