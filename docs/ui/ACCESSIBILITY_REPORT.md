# ASHFALL UI accessibility and usability audit — 2026-09-05

Read-only companion to [UI panels forensic report](../forensics/UI_PANELS_UX_FORENSIC_REPORT.md) and [178-panel/141-route inventory](../forensics/UI_PANELS_UX_INVENTORY.md). Scope: shared theme/components, navigation/lifecycle, high-density screens and representative stored renders. This is not a full assistive-technology or fresh visual certification.

## Ranked findings

| Priority | Finding | Evidence / acceptance direction |
|---|---|---|
| HIGH | Grid rows cannot be selected through their own keyboard/controller path | AshfallDataGrid.BuildRowContainer (src/UI/AshfallDataGrid.cs:226 onward) creates PanelContainer rows, accepts only left InputEventMouseButton, and assigns no focus mode/navigation action. Add a focusable selection contract with visible focus and scrolling; exercise every grid consumer. A programmatic SetSelected method is not keyboard input. |
| HIGH | Dashboard navigation physically exceeds the nominal canvas | GameDashboardPanel.BuildNavigationRail (397–460): 40 buttons × minimum 30px = 1200px before section headings/gaps/save/dev/header/footer, inside a VBox without scrolling. Make all entries reachable at supported sizes and with focus scrolling. |
| HIGH | Missing/incorrect overlay detection makes Escape unpredictable | Main.GameFlow.AnyOverlayPanelOpen (662) and Main.PanelLifecycle.CloseAllOverlayPanels (9) maintain different incomplete lists. Global cancel can return to menu if an unlisted panel does not consume input. Validate topmost cancel/focus restoration and transitions, not only the existence of OnClose. |
| HIGH | Non-disabled metadata uses insufficient contrast and 11px type | DataGrid.MakeHeaderLabel (343–354), Sidebar header/hints (67–68/156–157), Theme.FontSizeLabel=11. Dim is below 4.5:1 on all tested opaque backgrounds; headers and navigational hints are not merely disabled controls. |
| HIGH | Errors occur while creating high-density detail screens | ExpeditionRadar, FactionsNarrative, SkillMatrix free cached detail labels and then use them; three ObjectDisposedExceptions observed. A broken information panel is not usable regardless of contrast. See UI-18. |
| MEDIUM | Stored atlas renders crop right-side data | Research/StandingRecord/Muster default PNGs at 1280×800 visibly lose right-side grid content. Existing snapshots are fixture evidence, not fresh bound renders. Validate child bounds and long values before approval. |
| MEDIUM | Small text/targets become harder to use at reduced display scale | Theme uses labels 11px, small 12px, mono 13px; shell Close minimum height 28px; dashboard nav 30px. Reported as usability risks, not a blanket claim of a target-size standard violation. |
| MEDIUM | Theme hex values and runtime tuples disagree | PaleHex, SurfaceHex, SurfaceCardHex and WarningHex do not match their corresponding tuple values. Stitch handoffs/screenshots that use hex may not match Godot controls using tuples. Reconcile the existing authority rather than adding a second palette. |
| MEDIUM | Misleading labels and fake action affordances | Six atlases show non-selectable command text; multiple domains show unrelated or fixed state. Readability cannot compensate for incorrect meaning. See UI-01–UI-03/UI-19. |

## Measured token contrast

Evidence: Assets/Ashfall.Core/UI/Theme.cs. Ratios computed from the **float RGB tuples used by ToColor**, not the inconsistent Hex constants. For each sRGB channel c: c/12.92 when c≤0.04045, otherwise ((c+0.055)/1.055)^2.4; relative luminance = 0.2126R + 0.7152G + 0.0722B; ratio = (lighter+0.05)/(darker+0.05).

Audit thresholds supplied by the UI-access skill: 4.5:1 for body text; 3:1 for large text. The table is opaque-token analysis, not a blanket claim that every listed pair occurs in every rendered state. Semitransparent panels/row fills require compositing against the actual scene and can change the result.

| Foreground | On Ink (0.035, 0.043, 0.047) | On SurfaceCard (0.078, 0.098, 0.118) | On SelectedBg (0.157, 0.137, 0.098) | Body-text verdict |
|---|---:|---:|---:|---|
| Dim (0.400, 0.404, 0.373) | 3.45:1 | 3.09:1 | 2.73:1 | Fails all three; selected pair also below large-text threshold |
| Muted (0.576, 0.561, 0.518) | 6.11:1 | 5.48:1 | 4.84:1 | Passes these opaque pairs |
| Pale (0.902, 0.878, 0.824) | 14.98:1 | 13.43:1 | 11.87:1 | Passes |
| Warm (0.827, 0.667, 0.384) | 9.11:1 | 8.17:1 | 7.22:1 | Passes |
| Critical (0.902, 0.200, 0.200) | 4.59:1 | 4.12:1 | 3.64:1 | Fails on the latter two opaque pairs |
| Warning (0.788, 0.482, 0.227) | 5.99:1 | 5.37:1 | 4.74:1 | Passes these opaque pairs |

Token drift, values rounded to nearest byte for explanation:

| Token | Hex constant | Tuple approximately renders |
|---|---|---|
| Pale | #C7DCD0 | #E6E0D2 |
| Surface | #050709 | #0E1114 |
| SurfaceCard | #090B0D | #14191E |
| Warning | #FF6B35 | #C97B3A |

## Density, scaling and long-content risks

The reviewed dense surfaces include DutyRoster, DoseLedger, CaravanBarterLedger, Medical, Journal and the atlases. Shared DataGrid column minimums, fixed shell minimums and monospaced metadata are the primary cross-cutting seams. This audit does not claim all those domain screens are broken.

- DataGrid headers use 11px Dim text. Sidebar hints use the same size/color, making the information needed to distinguish tasks harder to read.
- Plans94To97 sets minimum 1120×620; Plans130To133 sets 1320×700. These are lower bounds, not responsive fitting rules.
- Each new Plans146–149 screen uses a sizable grid plus detail pane and sidebar. Its normal route is currently missing, so the report classifies both workflow and layout acceptance as incomplete, not a visually tested production pass.
- Raw IDs and long entity names can exceed minimum-width assumptions. New EB-PVD rows explicitly print job/coating IDs. No localized long-string fixture was injected during this documentation-only audit; ellipsis, wrapping, scroll and full-name detail remain acceptance checks rather than invented reproductions.
- Dashboard JOURNAL appears in different entry contexts with journal versus journal_detail IDs; players need clear distinctions if destinations differ (GameDashboardPanel.BuildHeader/BuildNavigationRail).

## Interaction and focus scope

Normal Godot buttons may have default focus behavior; absence of an explicit FocusMode assignment alone is not treated as a defect. The DataGrid finding is narrower and confirmed: row PanelContainers have no focusable/input-action path, and the handler only accepts mouse-button events.

The host supplies global cancel handling, so a panel lacking its own Escape method is not automatically a defect. The confirmed problem is that global detection/dismissal lists omit panels and disagree. Header text saying CLOSE [Esc] does not independently guarantee correct cancellation from every focus state.

ModalManager and modal-specific handlers are partial existing equivalents. This audit does not recommend another parallel modal stack. Follow-up must test topmost input ownership, hidden-overlay input, cancel/back, restoration to the triggering control, critical alert visibility and controller equivalents against the actual active composition.

## Visual evidence inspected

| Stored image | What it shows | Limit |
|---|---|---|
| [Research default](../../snapshots/research_atlas_default.png) | Faction/trust table in a research-labelled screen, repeated dossier/action layout and right clipping | Historical default/unbound fixture |
| [Standing Record default](../../snapshots/standing_record_atlas_default.png) | Same faction rows/body under different headings and clipped right content | Historical default/unbound fixture |
| [Muster default](../../snapshots/muster_atlas_default.png) | Same five-faction body and fixed metric layout | Historical default/unbound fixture |

No images were edited, regenerated, annotated destructively or accepted as new baselines. Current source was used to distinguish bound-path fixed data from fixture-only behavior.

## Verification limits and next acceptance gates

--ui-accessibility-selftest printed five passing gates, but it does not compute these contrast ratios or traverse all row controls by keyboard. Its text gate checks placeholder strings, not visual readability. --ui-layout-selftest sets/checks root sizes rather than proving child fit; its pass log also reported 16634 leaked ObjectDB instances. The decon smoke check printed PASS after 18 missing-node, 18 null-reference and three disposed-object reports.

A later approved repair needs: exception-free construction/rebinding; all domain controls reachable from normal navigation; tab/directional traversal with visible focus; correct topmost back/close; descendant bounds at supported sizes; long/empty/error text; readable actual composited colors; textual hazard states; command correctness and feedback. Use dotnet and godot --headless, preserve baselines, and do not equate a screenshot or IsBound flag with a working player workflow.
