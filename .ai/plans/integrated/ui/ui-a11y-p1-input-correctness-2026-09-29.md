# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# UI A11Y P1 INPUT CORRECTNESS — 2026-09-29

STATUS: FULLY INTEGRATED
Authorized: user session request "some more UI work!" (2026-09-29), implementing
the P1 fixes from `docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md` (§9 items 1, 2
partial, 6). Ownership: user-authorized integrator implementation, mirroring the
`claim-deep-audit-repair-2026-09-26` convention.

> **Editorial polish (prose pass, non-contractual):** the Framing section below is commentary on
> intent and craft only. It changes no scope, no ownership, no fix and no acceptance criterion.
> **MUST NOT** below remains binding, including the deliberately-recorded divergence from the
> audit's proposed fix.

---

## 0. Framing — Whose Keystroke Is It?

> *"Every accessibility defect is a bug where the game accidentally chose a player and excluded
> the rest."*

Three input-correctness defects, all in the host layer, all trivial to describe and all invisible
until they happen to you: raw keys 1–5 consumed twice because two panels believe they are alone; a
**J** that fires in a menu because every other hotkey was gated and this one was not; and a crisis
alert that can be buried under a modal that did not exist when the HUD was written.

None of these are styling. They are *who the game listens to*. A key that does two things at once
does zero things usefully. A hotkey that fires in a menu is a door that opens into a wall. And a
buried crisis alert is a game that told the player something urgent and then talked over itself.

**Tone & register.** Forensic and humane. The vocabulary is the input stack: *collision, gate,
phase, echo, tree-order, topmost, sweep*. Prose should read like someone who has watched a player
press a key four times and then quit.

**The interesting part — the divergence.** The audit proposed adding the crisis HUD to
`OverlayPanelCatalog()`. On revalidation that would have **closed the HUD on every panel switch and
never re-opened it** — losing an active crisis alert entirely. The plan refuses the audit's fix,
records the refusal in writing, and explains why. That is engineering judgement doing its actual
job: the report is an input, not an instruction.

**The second layer.** Listening is a design decision, and this plan audits who the game listens to.
Three defects, three different kinds of exclusion: the player whose keystroke is consumed twice,
the player whose menu opens into a wall, the player whose crisis alert is talked over by the game
itself. And the plan's deepest moment is its refusal — a proposed fix that would have closed the
crisis HUD on every panel switch and never re-opened it is *worse than the defect it treats*.
Competence in this lane is knowing which instructions to decline, in writing.

**Texture (second prose pass — commentary only).**

- "Two panels believe they are alone." The whole collision in one sentence of anthropomorphic blame.
- "A hotkey that fires in a menu is a door that opens into a wall." Architecture as etiquette.
- "A game that told the player something urgent and then talked over itself." The buried alert is a manners failure with a crisis attached.

*Register below unchanged — these fragments are texture, not new recorded items.*

**The polish layer (third prose pass — commentary only).**

*(Non-contractual: craft commentary only. No scope, ownership, acceptance criterion or register
row changes. The register below is unchanged.)*

- Every collision is two certainties meeting: two panels that both believe they are alone. The
  repair is not arbitration — it is making the room's occupancy legible.
- The refusal is the file's moral centre. A report is an input, not an instruction; the plan says
  *no* in writing, with reasons, which is the only form of refusal that outlives its author.
- A key that does two things at once does zero things usefully. Correctness here is not speed; it
  is *one keystroke, one meaning*.

> "The game was already listening to someone — by accident. This plan makes the listening
> deliberate."

---

## Bounded outcome

Three input-correctness defects fixed in the Godot host layer; no Core changes,
no data changes, no save-section changes (all three targets are presentation/
input seams only):

1. **Moral-choice modal key collision (1–5):** `OpenMoralChoiceModal` opens over
   an open CombatPanel without the exclusive-open seam, so raw keys 1–5 are
   double-consumed. Fix: call `CloseAllOverlayPanels()` first, matching
   `OpenFireIncidentPanel` and every `OpenPlayerPanel` route
   (`src/Main.GameFlow.cs:366`).
2. **J hotkey unguarded in menu state:** `AshfallInputActions.IsJournal` branch
   in `Main._UnhandledKeyInput` runs `ToggleJournal()` with no
   `_state == GameState.Playing` gate, unlike every sibling hotkey. Fix: gate
   the branch on Playing (menu-state J becomes a no-op, consistent with
   F/H/T/X/E).
3. **Crisis HUD layering:**
   - `EmergencyResponseHud._Input` (pre-GUI phase) closes the HUD even when a
     later-added modal is stacked above it. Fix: demote to
     `_UnhandledKeyInput` with the standard pressed/echo guard, so tree-order
     (later siblings first) decides, and the HUD still wins over the global
     Esc sweep when it is the only visible overlay.
   - Panels opened after `BuildUserInterface` (lazy panels, modals) draw above
     the crisis HUD. Fix: in `CloseAllOverlayPanels`, `MoveToFront()` the HUD
     when visible, so an active crisis alert is never obscured.

## Divergence from the audit's proposed fix (recorded, deliberate)

The audit proposed adding `_crisisHud` to `OverlayPanelCatalog()`. On
revalidation that would *close the HUD on every panel switch* and never
re-open it (open path is gated on `snap.Severity >= Severe && !Visible`,
`src/Main.UiPanels.cs:1424-1429`) — losing an active crisis alert. The
catalog membership is therefore intentionally NOT done; re-raise is used
instead. Also NOT done (scope): wiring the Core-advertised `Shortcut = "1"`
strings (`CrisisPresentationCoordinator.cs:228` etc.) — the HUD has zero
consumers of `Shortcut`; Space already activates the focused ack button via
engine `ui_accept`; adding digit handlers would create a NEW collision with
CombatPanel 1–5 while both are visible. Core field left dormant; follow-up
noted in the audit report.

## Exact files

- `src/Main.UiHandlers.cs` — Fix 1 (1 line)
- `src/Main.Application.cs` — Fix 2 (1 condition)
- `src/UI/EmergencyResponseHud.cs` — Fix 3a (input phase + guard)
- `src/Main.PanelLifecycle.cs` — Fix 3b (re-raise, ~4 lines)
- `Ashfall.Core.Tests/UI/UiA11yP1InputGateTests.cs` — new static source gate
  (pattern: `MainTriadDriftGateTests`), pinning all three fixes against drift.
- Governance: this plan, `WORKTREE_OWNERSHIP.md` (claim row), `.ai/state.md`.

`src/Main.PanelLifecycle.cs` note: listed in the stale 2026-09-25
PFGL-CODEX-LUNA6-OCTET row, but that phase's files shipped
(`src/Main.PfglOctetBoards.cs`, `src/UI/PfglOctetBoardPanels.cs` exist) and the
file has been edited by multiple later integrations; the change is a 4-line
additive block.

## MUST NOT

- No Core changes (CrisisPresentationCoordinator stays untouched).
- No catalog membership change for the crisis HUD (see divergence).
- No renames, formatting, or cleanup beyond the five files above.

## Verification

1. `dotnet build Ashfall.csproj` — 0 errors.
2. Focused: new gate test + adjacent input/UI gates via `bin/run-scoped-tests`.
3. `godot --headless --path . --quit-after 2` boot check (input-path change).

## Open Items & Deliberate Limits (register — not acceptance criteria)

Drawn from this plan's **MUST NOT** list and the recorded divergence. Not defects — deliberate
non-changes. Any later pass that raises one must re-validate against the reasons recorded here.

| # | Open item | Why it is deliberately open | Who may resolve it (later, separately justified) |
|---|---|---|---|
| UI1-OM-1 | **Core advertises `Shortcut = "1"` strings the HUD never consumes.** | `CrisisPresentationCoordinator.cs:228` etc. advertise digit shortcuts with **zero consumers**. Wiring them would create a NEW collision with CombatPanel 1–5 while both are visible. The Core field is left **dormant**. | A package that resolves the two-panel collision first. |
| UI1-OM-2 | Why does the Core advertise a shortcut nobody implemented? | The strings are real and unread. The plan does not explain the gap and must not invent one. | Never — the artefact reads as intent outlived by refactor. |
| UI1-OM-3 | Should `ModalManager`/`ModalStackController` be wired at all? | Left unwired in P1 and explicitly deferred in P3 to **a separate governance decision**. The trap seam is dead code and stays dead. | Foreman/governance, not a builder. |
| UI1-OM-4 | Why did the audit propose a fix that would have broken a crisis alert? | The divergence is **recorded and deliberate**. The audit was not wrong so much as reasoning from a narrower model of the panel lifecycle. | Never — the record is the value. |
| UI1-OM-5 | What is inside the stale PFGL-CODEX-LUNA6-OCTET ownership row? | The row is stale but the files shipped and were later edited by multiple integrations. Its status is a governance matter, not this package's. | `WORKTREE_OWNERSHIP.md`'s owner. |
