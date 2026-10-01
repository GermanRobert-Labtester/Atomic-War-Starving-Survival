# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# First-Hour Keyboard-Only Playtest + Triage (T01)

> **STATUS: FULLY INTEGRATED** — authorized by the user's direct request in this
> session: "T01 │ Play keyboard-only first-hour session; triage ≤3 fixes",
> executed against `docs/alpha/FIRST_HOUR_PLAYTEST_KIT.md` §2–§5 as the
> strongest available automated form. Two fixes shipped; both proven by a new
> source gate that fails pre-fix and passes post-fix.

## 1. Goal & Outcome

Run one first-hour session in its strongest keyboard-only-equivalent automated
form (live funnel + focusability/clickability/accessibility/tab-trap gates +
forensic static audit of the 7 stage surfaces and their host routes), then
triage **at most three** evidence-backed fixes. A fix must extend an existing
owner (host wiring, panel, Core contract) — no new authority, no save-schema
or determinism change.

## 2. Non-Goals

- No new gameplay system, save section, or panel.
- No change to `OnboardingCatalog` routes / objectives or to Core.
- No global input-contract change (Plan 37 / C2[15] owns Tab parity; the
  documented `DailyBriefingModal` Tab dead-zone is recorded, not fixed here).
- No full test suite (per `TEST_POLICY.md`).

## 3. Current Evidence (the "play")

- `--playable-metrics-selftest` → 17/17, first-hour funnel 7/7.
- `--ui-accessibility-selftest` → PASS 5/5 (183 controls, 18 panels; 5 gates).
- `--ui-layout-selftest` → `panels=169 buttons=661 inert=0`,
  `interactive=659 unreachable=0 panelsWithNoFocus=0`,
  `blankUnboundPanels=0`, `Failures: 0`.
- Static audit of the 7 stage surfaces (water_treatment, power_grid,
  inventory, duty_roster, dose_ledger, research, expeditions) against the
  sigils each stage requires (`OnboardingWiringGateTests` only asserts the
  sigil string exists in source — it does not prove the live route reaches a
  surface that can produce it). Two confirmed defects, below.

## 4. Defects & Fixes

### Fix 1 — Food stage: the `inventory` route's SELECT does nothing

`_inventoryOverlay` (`Main.UiPanels.cs`) is the panel opened by the first-hour
Food stage's `ShowMeWhereRoute="inventory"`. `InventoryPanel` raises
`OnItemSelected` from every row's `SELECT` button, but only the *legacy hidden*
`_inventoryPanel` instance (added to the developer console's right column)
subscribed it (`Main.Inventory.cs:148`). The player-facing overlay's event has
no subscriber, so SELECT is a dead affordance and the only consume path
(`OnInventoryItemSelected` → `InventoryDetailPanel` → `OnInventoryConsumeClicked`
→ `ObserveSigil("food.ration_consumed")`) is unreachable. The clickability gate
cannot see this because the button *has* a `pressed` connection.

Fix: subscribe `_inventoryOverlay.OnItemSelected += OnInventoryItemSelected;`
at the overlay's creation site. Same handler the legacy widget uses; no new
authority.

### Fix 2 — Duty stage sigil fires on row *selection*, never on assignment

`DutyRosterPanel.HandleRowSelected` raises `OnAssignmentChanged`, and the host's
`HandleDutyRosterAssignmentChanged` records `ObserveSigil("duty.assigned")`
(`Main.UiPanels.cs:241`). The real assignment mutations (`TryAssign`,
`ConfirmPendingAssignment`, `VACATE SHIFT`) raise `OnAssignmentChanged` **not at
all**. So the onboarding Duty stage ("Assign a survivor to a shift") completes
from merely focusing/Tab-ing to a roster row (Enter), and a genuine assignment
produces no signal. `OnboardingWiringGateTests` passes because the string exists;
the automated funnel drives `RecordSigil` directly and never exercises this path.

Fix: `OnAssignmentChanged` is raised by the assignment mutations (on success)
and is no longer raised by row selection. `OnRoleSelected` / `OnDetailsRequested`
keep the selection behavior unchanged.

## 5. Claimed Paths

- `src/Main.UiPanels.cs` — one subscription line at the `_inventoryOverlay`
  creation site only.
- `src/UI/DutyRosterPanel.cs` — `HandleRowSelected`, `TryAssign`,
  `ConfirmPendingAssignment`, VACATE handler.
- `Ashfall.Core.Tests/UI/FirstHourKeyboardPlaytestGateTests.cs` (new) —
  focused source regression gate for both contracts.
- `.ai/plans/first-hour-keyboard-playtest-2026-10-01.md`, `.ai/state.md`.

Foreign dirty hunks already present in `Main.UiPanels.cs` (expedition-loot
toast, dev-session start) and in `Main.Inventory.cs` / `Main.DutyRoster.cs`
(`ClosePanelAnimated`) are untouched.

## 6. Verification (results)

- [x] `dotnet build Ashfall.csproj` — 0 errors (6 pre-existing unrelated warnings).
- [x] New `FirstHourKeyboardPlaytestGateTests` 2/2 green; `OnboardingWiringGateTests` 4/4 green.
- [x] `--onboarding-journey-selftest` PASS; `--playable-metrics-selftest` 17/17;
      `--ui-accessibility-selftest` 5/5; `--player-panels-uitest` 22/22;
      `--duty-roster-uitest` PASS; `--inventory-uitest` PASS;
      `--ui-layout-selftest` `inert=0 unreachable=0 panelsWithNoFocus=0
      blankUnboundPanels=0 Failures: 0`.
- [x] `git diff --check` clean (owned paths).
- [x] TDD proof: the new gate fails 2/2 against the pre-fix source and passes
      2/2 after the fix.

## 7. Limits (honest)

- No physical keyboard/display session exists for this agent, so the
  directional-traversal leg remains the human alpha step named in the kit;
  this pass used the focusability/clickability/tab-trap/accessibility gates
  plus a forensic route↔sigil audit of the 7 stage surfaces.
- The `DailyBriefingModal` `[Tab]` dead-zone and the missing dashboard-rail
  entry for `water_treatment` were recorded, not fixed (Plan 37 owns the
  former; the latter is a product decision).
