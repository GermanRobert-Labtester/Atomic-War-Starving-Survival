# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# UI A11Y P2.5 + P3 SIDEBAR KEYBOARD ACCESS, HOVER FEEDBACK, OVERFLOW PRECISION — 2026-09-29

STATUS: FULLY INTEGRATED
Authorized: user session request "Continue doing more UI correction and UI
precision work!" (2026-09-29). Sixth package in the audit-fix series: the one
remaining unimplemented P2 item (§9.5 functionality) plus the P3 hover
feedback gaps (§7/§9.13) and the remaining fixed-width overflow sites (§6).

> **Editorial polish (prose pass, non-contractual):** the Framing section below is commentary on
> intent and craft only. It changes no scope, no ownership, no fix and no acceptance criterion.
> **MUST NOT** below remains binding — including the recorded engine limitations.

---

## 0. Framing — The Control That Wasn't There

> *"A row you can see and cannot reach is not a control. It is a picture of a control."*

The sidebar navigation rows are `PanelContainer + Label` with a mouse-only `GuiInput`. They look
exactly like buttons. They respond to a mouse. And to a keyboard or a controller they are **not
merely hard to use — they do not exist**: `FindFocusableControls` collects the Button family only,
so the focus navigator cannot see them at all.

The fix is one architectural move: convert the row to a flat `Button` with per-row styleboxes
mirroring today's look exactly. In a single step that yields keyboard activation, navigator and
Tab-trap eligibility, *and* hover feedback. Three defects, one root cause, one change — which is
what it looks like when the accessibility problem is structural rather than cosmetic.

**Tone & register.** Observant, structural, slightly indignant on the player's behalf. The
vocabulary is the widget tree: *row, container, stylebox, focus ring, navigator, hover, clip*. Prose
should read like someone who has watched a player tab past a menu they can see.

**The interesting honesty.** Three hover gaps are recorded as **engine-limited** and left alone:
per-item `ItemList` hover (Godot has no hovered-item stylebox), `SpinBox` arrow theming, and
`RichTextLabel` link hover. The plan does not fake a workaround and does not pretend the gap
closed. Some things the engine will not give you, and saying so is the work.

**The second layer.** A row that looks like a button and is not one is worse than a missing button:
it is a *picture* of a control, and a player who tabs past it learns that the interface is lying
about what can be reached. The structural fix — one root cause, three defects — is the plan's
argument that accessibility in this lane is engineering, not decoration. And the three recorded
engine-limited gaps are the honesty that buys the rest of the file its credibility.

**Texture (second prose pass — commentary only).**

- "A player tab past a menu they can see." The image the whole lane is written against.
- "One architectural move" yields activation, navigator eligibility, Tab-trap eligibility and hover — four gifts from one decision.
- Engine-limited is recorded, never faked. A workaround that pretends is a defect with a costume on.

*Register below unchanged — these fragments are texture, not new recorded items.*

**The polish layer (third prose pass — commentary only).**

*(Non-contractual: craft commentary only. No scope, ownership, acceptance criterion or register
row changes. The register below is unchanged.)*

- The defect is ontological, not ergonomic: for keyboard and controller players the rows did not
  merely work badly — they were *absent*. A picture of a control is not a control.
- One conversion pays three debts — activation, navigator eligibility, hover feedback — because
  the fix is at the type, not the symptom. The navigator collects Buttons; the plan teaches the
  row to be one, honestly.
- The same defect photographed many times is still one defect. Counting root causes rather than
  sites is how a sweep stays a repair instead of becoming a chore.

> "The keyboard player was not poorly served. The keyboard player was not served at all — and
> could not have complained in a way the bug tracker would have kept."

---

## Bounded outcome

1. **Sidebar nav rows become keyboard-reachable (P2.5).** Rows are
   PanelContainer+Label with mouse-only `GuiInput` (`AshfallSidebar.cs:167-174`)
   — unreachable by keyboard/controller and invisible to the focus navigator
   (`FindFocusableControls` collects Button-family only). Fix: convert the row
   container to a flat `Button` with per-row styleboxes mirroring today's
   look (normal Ink 0.40, hover Warm 0.12, pressed Warm 0.20, focus =
   the shared `MakeFocusVisibleStyleBox` ring), `Pressed` → `Select(item.Id)`.
   This yields keyboard activation (ui_accept), navigator/Tab-trap
   eligibility, and hover feedback in one move. `SetRowHighlight` mutates the
   per-row `normal` stylebox instead of `panel`.
2. **Grid row hover feedback (P3 §7).** Selectable `AshfallDataGrid` rows get
   `MouseEntered`/`MouseExited`: a Warm 0.10 hover fill while not selected,
   restored via the existing `ApplyRowStyle`/selection path on exit. Selection
   styling stays owned by `RefreshRowHighlights`.
   Not done (engine-limited, noted): per-item ItemList hover (no hovered-item
   stylebox in Godot), SpinBox arrow theming, RichTextLabel link hover.
3. **Remaining fixed-width overflow labels (precision §6).** `ClipText` +
   `TextOverrunBehavior.TrimEllipsis` on: TradeScreenGodotPanel item-name
   labels (100/120/140px), AshfallMetricCard `_valueLbl` (mono value on
   110-180px cards), SurvivorsPanel survivor display name (140px),
   GameDashboardPanel gauge row names (74px). Same correction pattern as the
   P3 grid/shell package.

## Exact files

- `src/UI/AshfallSidebar.cs` — AddRow row type + styleboxes + Pressed;
  SetRowHighlight selector
- `src/UI/AshfallDataGrid.cs` — hover handlers in the selectable-row block
- `src/Economy/TradeScreenGodotPanel.cs` — 3 label sites
- `src/UI/AshfallMetricCard.cs`, `src/UI/SurvivorsPanel.cs`,
  `src/UI/GameDashboardPanel.cs` — 1 label site each
- `Ashfall.Core.Tests/UI/UiA11ySidebarHoverOverflowGateTests.cs` — new gate
- Governance: this plan, `WORKTREE_OWNERSHIP.md`, `.ai/state.md`

## MUST NOT

- No Core changes; no ItemList/SpinBox/RichTextLabel theming (engine-limited,
  recorded); no layout/size changes beyond clip behavior; no data/save paths.

## Verification

1. `dotnet build Ashfall.csproj` — 0 errors.
2. Gate tests via `scripts/run_test.sh` + adjacent UI gates.
3. `--ui-layout-selftest` + `--player-panels-uitest` headless runtime probes.

## Open Items & Deliberate Limits (register — not acceptance criteria)

Drawn from this plan's **MUST NOT** list and its engine-limited notes. Not defects — recorded
boundaries. Any later pass that raises one must re-check the row type and the clip sites.

| # | Open item | Why it is deliberately open | Who may resolve it (later, separately verified) |
|---|---|---|---|
| UI4-OM-1 | **Godot has no hovered-item `ItemList` stylebox.** | Engine-limited and recorded. Per-item hover is not faked and the gap is not pretended closed. | An engine change or a custom-drawn replacement list. |
| UI4-OM-2 | `SpinBox` arrow theming and `RichTextLabel` link hover. | Also engine-limited, also recorded. | Same — engine, not plan. |
| UI4-OM-3 | Why were the rows mouse-only in the first place? | `PanelContainer + Label` with `GuiInput` is a plausible way to draw a row. That it reads as a control is exactly the problem; the origin is not recorded. | Never — texture by omission. |
| UI4-OM-4 | Does making the row a `Button` change anything a mouse player sees? | Per-row styleboxes **mirror today's look exactly** (Ink 0.40 / Warm 0.12 / Warm 0.20 / shared focus ring). Visual parity is asserted, not snapshot-verified here. | The visual lane's snapshot goldens. |
| UI4-OM-5 | How many other controls are pictures of controls? | This package fixes the sites the audit enumerated. `FindFocusableControls` collects Button-family only — a structural rule that can hide others. | A sweep of non-Button interactive containers. |
| UI4-OM-6 | Does hover feedback help anyone who cannot hover? | Hover is delivered alongside keyboard activation deliberately. The three of them arrive together or not at all. | Never — a rule, not a gap. |
