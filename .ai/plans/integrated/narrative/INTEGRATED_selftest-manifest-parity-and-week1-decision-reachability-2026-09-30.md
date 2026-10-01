# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> (executed and verified 2026-09-30; archived immediately per workflow rule 8)

## Selftest manifest parity + week-1 decision reachability (tasks 11 & 13)

**STATUS: APPROVED BY USER** — user-authorized 2026-09-30 ("Start coding till
done these 2 tasks!": task 11 stale debt ledger / parity gap; task 13 week-1
event reachability with at least one decision per day).

### Task 11 — DEBT-HOSTCLI-PROBE-MANIFEST-GAP retirement

- Premise verified: build green (`dotnet build Ashfall.csproj` 0/0); the
  committed manifest already carried the 2026-09-27 descriptor wave (312
  tests, empty shrink-only gate baseline), but 4 dispatched probes were still
  uncataloged (`year_two_chapter_selftest`, `failure_restart_selftest`,
  `food_loop_selftest`, `reasonable_player_selftest` — the last three from
  uncommitted `HostCliRegistry.cs` descriptors).
- Ran `python3 scripts/ci/generate-selftest-manifest.py`: manifest grew
  312→316 (314 headless-compatible); `--check` OK.
- `HostCliActionParityGateTests` 4/4 PASS with the empty baseline; shard-smoke
  dry-run selection sees the previously invisible probes.
- Retired the `KNOWN_DEBT.md` row (ACCEPTED→RETIRED with seal evidence).

### Task 13 — week-1 decision reachability

Forensic finding (verified in source, not assumed):

- `events.json` (240 rows) has **no runtime weighted picker**: only
  id/title/bodyText/minDay are read (`EventsHostSession` log read-model,
  `JournalCatalogData` codex, Phase0 `fireNarrativeEvent` prose dispatch).
  `weight`/`maxDay`/`conditions`/`choices` are dead data at runtime. 37 rows
  have minDay ≤ 7; only 7 of those are referenced by any consumer catalog;
  135 of 240 are referenced by nothing. Wiring a weighted picker for this
  catalog is a new architecture decision (foreman signature required), so it
  was not improvised.
- The real per-day decision stream is `NarrativeArcEventSystem`
  (`narrative_arc_events.json`) with the `EchoSystem` (`echoes.json`) fallback,
  driven by `NarrativeQuestsVerdictDayOwner.TickDay` and presented after the
  daily briefing (`Main.Campaign.OnBriefingAcknowledged`). Before this change:
  all 15 arc events had minDay ≥ 10 and only 2 echoes had minDay ≤ 7 (both 5),
  so days 1–4 surfaced **zero** decisions and days 5–7 at most two one-time
  echoes.
- The 218 registered quests are the `MoralChoiceSystem` catalogs
  (68+100+50, `MoralChoiceIds.TotalQuestCount`); they are player-initiated
  panel content (89 window-eligible in week 1 before chain gates), not a
  day-pushed decision stream.

Data-only fix (minDay gates, matching the task-7 JSON-tuning precedent):

- `narrative_arc_events.json`: `narrative_garrison_defector_intel` 10→1,
  `narrative_militia_council_invitation` 15→3, `narrative_cult_prophet_rumor`
  20→5 (the only three survivor-independent arc events; the four
  character-bound arcs stay out of week 1 because their survivors are not in
  the starting roster).
- `echoes.json`: `echo_the_nameplates` 5→2, `echo_unopened_boots` 5→4,
  `echo_the_frying_pan` 15→6, `echo_school_register` 15→7 (both `minDay` and
  `conditions.MinDay` kept in lockstep).

Result: one new decision becomes eligible each day 1–7 (d1 garrison, d2
nameplates, d3 militia, d4 boots, d5 cult, d6 frying pan, d7 school register);
the pending modal auto-opens after the daily briefing, so a decision reaches
the player every day regardless of resolution timing.

### Verification

- `bin/ashfall-dev validate-json` 714/714, 0 violations.
- New `Ashfall.Core.Tests/Narrative/WeekOneDecisionReachabilityTests.cs`:
  5-seed theory (9001/1337/77/424242/20260930) × days 1–7 resolving each
  decision through the real arc-then-echo owner flow, plus the eligibility
  ladder pin — 6/6 PASS.
- Scoped neighbors green: `NarrativeArcEventSystemTests`, `EchoSystemTests`,
  `EchoCatalogTests` (4 suites, 0 failed).
- Headless: `--data-integrity-selftest` 430/430; `--seven-day-smoke-selftest`
  10/10; `--day1-selftest` PASS; `--real-campaign-journey-selftest` PASS;
  `--reasonable-player-selftest` 17/17.

### Non-goals / untouched

- No new weighted picker for `events.json` (architecture decision pending).
- No changes to `events.json` (the worktree's foreign week-1 tuning diff there
  is preserved untouched), moral-choice catalogs, arc/echo systems' code, or
  any save/UI path.
- No commit; full suite not run.
