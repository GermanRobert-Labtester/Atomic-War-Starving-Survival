# Task #112 — Campaign Calendar Authority: Residual Gap Closure

**Situation:** The core delivery already landed (`8029aa69`): `ICampaignCalendar` + `CalendarSimClockAdapter` (ISimClock preserved, substep 2), read-only `_simDay`, the five clock adapters, journal-day derivation gone, a `CampaignCalendarReconciler`, source gates, and `docs/architecture/CAMPAIGN_CALENDAR_AUTHORITY.md`. The audit found one undecided architecture fork plus five residuals:

1. **The calendar is a follower, not the authority**: `CommitAdvance` derives `targetDay = _core.Clock.Day + 1`; three external `Calendar.SetDay(_core.Clock.Day)` writes exist (Main.GameFlow.cs:687, Main.Holdfast.cs:54,112).
2. **`CampaignCalendarReconciler` is dead in production** (substeps 7/8): zero host callers — legacy conflicting section days load unreconciled and `[CALENDAR_MISMATCH]` never fires.
3. **Fallback expressions persist** (substep 11): Main.cs:52 (`?? _core?.Clock?.Day ?? 1`), radio reads core clock preferentially (Main.Narrative.cs:181), Phase0 (`?? _simDay`, Main.Phase0.cs:172).
4. **Gate blind spots** (substep 10): `_simDay` writes unscanned in src/UI; `.AdvanceDays(`/`Clock.SetDay(` unscanned in Main/UI.
5. **Duty roster off-by-one**: `DutyRosterHostSession.TickDay` self-advances its clock while the owner also `SyncDay(day)`s — correctness rests on per-tick re-clamp.
(Plus: no end-to-end projection-agreement test across a real advance + save/load — substep 9 is Core-stub-only.)

**Decisions taken (user answers did not arrive; recommendations, reversible):** ① the **calendar becomes the leader** — `CommitAdvance` derives the target day from `Calendar.CurrentDay + 1` and the Core clock becomes a projection synced by the holdfast owner; ② gate extends to **Main + UI only** (src/Host stays unscanned — engine sessions legitimately own internal ISimClock tick clocks, substep 2).

## Phase A — Calendar leads (the inversion)
- `Main.Holdfast.cs CommitAdvance`: `targetDay = _campaignDay.Calendar.CurrentDay + 1` (SetupCampaignDay is already called first in both advance paths).
- `HoldfastCoreDayOwner.TickDay`: after `_core.TickDay()`, land the clock exactly on the campaign day (`Clock.SetDay(day)` — self-healing projection); keep the existing snapshot/rollback (still needed for fail-closed retry).
- The three stray `Calendar.SetDay(_core.Clock.Day)` writes: replace with clock←calendar sync (`_core.Clock.SetDay(Calendar.CurrentDay)`) where a sync is genuinely needed (restore/new-game paths), drop where redundant — read each site at implementation.
- Load path converges: restore pipeline sets the calendar (`RestoreState`), then the holdfast clock re-syncs from it. Verify `TickSimDay`'s UiTests day sequence (277→280) still passes under calendar-led idempotency.

## Phase B — Reconciler wired + mismatches reported (substeps 7/8)
- In `SetupCampaignDay` (Main.Campaign.cs): after `CampaignDaySaveStore.TryLoad()`, collect the persisted section days (campaign_day, holdfast `simDay`, duty roster, economy `Market.Day`, year-of-ash timeline), call `CampaignCalendarReconciler.Reconcile` (its existing campaign_day > holdfast > max priority), emit each `MismatchRecord.FormatLogMessage()` through GodotLog (`[CALENDAR_MISMATCH] …`), and adopt the reconciled day before `RestoreState`. Read the reconciler's exact signature first and match it.

## Phase C — Fallback removal (substep 11)
- `Main.cs:52`: `_simDay => _campaignDay?.Calendar?.CurrentDay ?? 1` (calendar-or-uninitialized only).
- Radio (Main.Narrative.cs:181): `_radio.SetDay(_simDay)`.
- Phase0 (Main.Phase0.cs:172): calendar projection primary — read context and simplify.
- UI bindings keep reading `_simDay` (the projection) — no panel reads raw subsystem clocks.

## Phase D — Duty roster off-by-one
- Move roster day ownership fully into `SyncDay`: `TickDay` stops self-advancing (`Clock.AdvanceDays(1)` removed) and ticks for the day it was synced to. Audit internal `Clock` uses inside DutyRosterHostSession first (schedule math) so nothing silently shifts by one.

## Phase E — Gate widening (substep 10, Main+UI scope)
- Extend `CampaignCalendarSourceGateTests`: scan `src/UI/*.cs` for `_simDay` writes and `.AdvanceDays(`; scan `src/Main*.cs` for `.AdvanceDays(` and `Clock.SetDay(`/`Clock.Day =` with an explicit allowlist (Main.CampaignOwners.cs snapshot rollback, Main.Holdfast.cs calendar-led sync). Leave src/Host unscanned (ISimClock granularity preserved). Fix any real violations the widened gate surfaces.

## Phase F — End-to-end projection agreement (substep 9)
- Extend the existing `RunIceRoadTickDemo` headless selftest (HostCli.PanelTests.cs:1170 — drives a real advance through CommitAdvance): after advance, assert `Calendar.CurrentDay == _core.Clock.Day == Market.Day == roster day`; then save/reload through the real stores and assert all projections agree again (and that a seeded mismatch in a legacy section logs `[CALENDAR_MISMATCH]` and reconciles). If that selftest lacks access to the needed state, use the SilentFoundry UiTests advance path instead — decide at implementation.

## Phase G — Verification & docs
Per-phase commits with the canonical checklist; final `verify-fast`; update `docs/architecture/CAMPAIGN_CALENDAR_AUTHORITY.md` (authority direction), AGENTS.md, REPO_REVIEW_REPORT.md; cross-tool review handoff (≥2 coupled variables: authority direction + reconciler + gate).

## Out of scope
Scanning src/Host; changing ISimClock semantics; refactoring panel-local display fields named `_simDay` in UI panels (host-pushed, never incremented — verified); TouchSimDay/UiTests day values.

## Risks
- The inversion touches the advance hot path → all 25 coordinator + calendar tests must pass unmodified; save/reload covered by Phase F's e2e gates.
- Reconciler adoption changes which day wins on conflicting legacy saves → matches the documented priority (campaign_day > holdfast > max) already pinned by Core tests.
- Roster off-by-one fix shifts schedule math by one day for in-flight rosters → audited at implementation; the owner's SyncDay already re-clamps.
- Concurrent session hazard: stage only task files; commit after each green phase.
- **FIRST SAFE STEP:** Phase B (wire the reconciler — additive, no behavior change on agree-state saves) before the Phase A inversion.