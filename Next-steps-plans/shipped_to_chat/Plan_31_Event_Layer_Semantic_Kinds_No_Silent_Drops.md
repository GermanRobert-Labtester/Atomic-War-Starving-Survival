# Plan 31 — The Event Layer Speaks: Semantic Kinds, No Silent Drops

> **Wave:** Continuity Wave 4 — *The World Beyond the Gate*
> **Supersedes/corrects:** Wave 1's [Plan 17 Task 17A](Plan_17_Legibility_Cause_Effect_Guidance.md).
> 17A claimed "exactly one producer and 18 of 19 owners mute, so the briefing runs on its
> fallback". Re-measured at `ccac926e`, that is **wrong**: all 19 owners emit, and the briefing's
> real path does run. The actual defect is different — and cheaper to fix. See the erratum in
> `Wave4_Continuity_Audit_INDEX.md`.
>
> **Theme:** the typed day-event channel is fully built end to end — producers, transport, consumer
> — and **20 of the 27 event kinds the game emits hit no `case` in the briefing builder, which has
> no `default`.** Meanwhile six richer kinds the builder *does* handle (`survivor_condition`,
> `resource_delta`, `shelter_consequence`, `weather_condition`, `radio_transmission`,
> `crafting_production`) are never emitted by anything. Producer and consumer grew apart, and the
> failure mode is a clean, quiet briefing that reports almost none of what happened.

---

## Evidence Inventory (re-verified @ `ccac926e`)

### The transport is real and everyone uses it

| Fact | Evidence |
|---|---|
| Core contract | `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs:320` `IDayAdvanceOwner.TickDay(int day, List<DayStateChangeEvent> events)`; `DayStateChangeEvent { Kind, SourceOwnerId, PrimaryId, SecondaryId, Numeric }` (`:355–365`); per-owner reports collected with `FailureMessage` (`:377–388`) |
| All 19 owners emit | `grep -c "new DayStateChangeEvent" src/Main.CampaignOwners.cs` → **26 sites**; per-owner class scan → **19/19 owner classes** contain at least one emission (`EvolvingWorldDayOwner` has 7, `SurvivorsNeedsDayOwner` 2, all others 1) |
| The briefing's primary path does run | `src/Main.Campaign.cs:225` — `if (args != null && args.AllEvents().Any()) report = DailyBriefingReportBuilder.BuildFromDayEvents(day, day, args.AllEvents());` — so the hand-rolled fallback below it is the *exception* branch (headless/no-args), not the rule |

### …and 74 % of the vocabulary is thrown away

| Kind emitted | Source | Rendered? |
|---|---|---|
| `consumed_rations`, `crafting_completed`, `expedition_milestone`, `expeditions_caravans_ticked`, `hazard_warning`, `radio_intercept`, `weather_ticked` | `src/Main.CampaignOwners.cs` (+ harness) | **yes — 7 kinds** |
| `duty_roster_ticked`, `events_evaluated`, `expeditions_ticked`, `greenhouse_foundry_ticked`, `holdfast_ticked`, `journal_ticked`, `maritime_ticked`, `market_ticked`, `medical_disease_ticked`, `memorial_checked`, `narrative_ticked`, `needs_ticked`, `phase0_ticked`, `power_ticked`, `shelter_decor_morale`, `shelter_facilities_ticked`, `survivor_social_ticked`, `survivors_ticked`, `world_evolution_ticked`, `world_ticked` | `src/Main.CampaignOwners.cs`, `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs` | **DROPPED — 20 kinds** |

| Fact | Evidence |
|---|---|
| The switch has no default | `Assets/Ashfall.Core/Campaign/DailyBriefingReportBuilder.cs:49–115` — `foreach (evt …) switch (evt.Kind) { case … }` with **14 handled kinds and no `default:`** → unhandled kinds are silently discarded |
| The consumer is ahead of the producers | handled-but-never-emitted: `survivor_condition`, `resource_delta`, `shelter_consequence`, `weather_condition`, `radio_transmission`, `crafting_production` — six semantic shapes the builder already knows how to phrase |
| The vocabulary has no owner | `grep -rn "DayEventKinds"` → **0 hits**: kinds are string literals scattered across the host, a Core harness, and one switch |
| Heartbeats aren't information | `power_ticked`, `needs_ticked`, `market_ticked` carry no cause or delta — even if rendered they would not answer "what did I do and what did it cost" |
| Owner failure is invisible | `DayOwnerReport.FailureMessage` + `HasFailures` exist on the event args, and `src/Main.Campaign.cs` does not surface them (a skipped owner reads as "quiet day") |
| The briefing has a max-entries mechanism | `AddSectionIfNotEmpty(r, …, maxEntriesPerSection)` — volume control already exists to reuse |
| Attribution already has one good example | `EvolvingWorldDayOwner`'s `hazard_warning` + journal write
  (`src/Main.CampaignOwners.cs:501–506`) — transition, ids, numeric, and a journal line: the pattern to copy |

---

## Task 31A — Own the vocabulary: `DayEventKinds` in Core, producers converted to transitions

**Goal:** one authority for event kinds, and replace heartbeat emissions with transition/decision
emissions in the 17 owners that currently only ping.

**Files:** new `Assets/Ashfall.Core/Campaign/DayEventKinds.cs`,
`src/Main.CampaignOwners.cs`, `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs`,
`src/Main.Campaign.cs`, `Ashfall.Core.Tests/DayEventKindContractTests.cs` (new).

### Substeps

1. **Create `DayEventKinds`** as a static Core class: one `const string` per kind, snake_case, each
   with a doc-comment stating producer, meaning of `PrimaryId`/`SecondaryId`/`Numeric`, and section.
   Seed it with the 14 kinds the builder already handles (the consumer defines the vocabulary —
   producers were supposed to fill it).
2. **Replace the six handled-but-never-emitted kinds first** — that alone upgrades the briefing
   without touching the builder: `survivor_condition` (needs transitions), `resource_delta`
   (production/consumption deltas), `shelter_consequence` (power, facilities, thermal),
   `weather_condition` (state transitions), `radio_transmission`, `crafting_production`.
3. **Emit on transitions, not on ticks**: a rule per owner — *a day with no change emits nothing*.
   Heartbeats are for diagnostics, not the briefing; if a heartbeat is genuinely wanted, send it to
   the diagnostics file (W3's 26C step 7), never into the player's report.
4. **Delete or re-point the 20 dropped kinds** — for each, either it becomes a transition kind
   (`power_ticked` → `grid_brownout` / `breaker_tripped` / `fuel_low`), or it disappears. Keep a
   table in the PR so nothing is dropped by omission.
5. **Give every event a cause**: `SourceOwnerId` is already required — add an optional
   `causeId`/`actorId` field only where a player act exists (a ration cut, an assignment, a
   dispatch); leave it empty for ambient change so "you caused this" and "this happened" stay
   distinguishable, and make the briefing visually separate them.
6. **Add the failure surface**: when `DayAdvancedEventArgs` reports owner failure (`HasFailures` +
   `FailureMessage`), the briefing must show a warning line — a silently skipped system currently
   reads as a calm day, which is the worst possible failure mode for a hardening pass.
7. **Contract test**: every string passed as a `Kind` anywhere in the repo must be a
   `DayEventKinds` member (source-scan gate, the `SaveStoreCoverageGateTests` idiom), and every
   `DayEventKinds` member must be either emitted somewhere or explicitly marked reserved.
8. **Aggregation rule** with a stable order: N similar events collapse to one line with a count and
   the affected ids, chosen by severity — ordinal, deterministic, never dictionary-random
   (Invariant 4).
9. **Volume budget**: reuse `maxEntriesPerSection`; assert the briefing fits on screen at 1280×800
   (snapshot test), since richer events can quietly overflow.
10. **Localization from day one** (Wave 3's 25A): briefing lines are keyed
    (`briefing.kind.<kind>`) with `{primary}`, `{secondary}`, `{value}` placeholders — do not add
    new inline English while doing this work.
11. **Tests**: per-kind producer tests, no-transition-no-event test, aggregation determinism,
    failure-surfacing test, contract-gate tests (both directions).
12. **Run the five-step verification checklist** + `verify-fast.sh`.

**DoD:** 0 emitted kinds are dropped, 0 handled kinds are unproduced, and every briefing line names
its cause or is labelled ambient.

---

## Task 31B — Make the briefing navigable: from line to the screen that fixes it

**Goal:** the report becomes an interface, not a printout — every line points at the surface where
the player can act, and the history stays queryable afterwards.

**Files:** `src/Main.Campaign.cs` (`ShowBriefingForDay`),
`Assets/Ashfall.Core/Campaign/DailyBriefingReportBuilder.cs`,
`Assets/Ashfall.Core/UI/PanelRegistry.cs` (+ `PanelRegistryBootstrap.cs`),
`src/UI/DailyBriefingPanel`/modal (locate), `events_log` route, `journal` route,
`Ashfall.Core.Tests/BriefingRouteContractTests.cs` (new).

### Substeps

1. **Add a `route` to the briefing entry** (`DailyBriefingEntry` already carries category +
   primary/secondary ids) — map kind → panel id, resolved in Core against `PanelRegistry` ids so a
   deleted route breaks a test rather than a button.
2. **Only route to *live* panels** (Wave 1's 16A verdict + 15C liveness): an event that would route
   to a shelved console must route to its parent surface or to nothing, with the choice recorded —
   otherwise this plan recreates the false affordance problem in a new place.
3. **Wire the click** using the seam that already exists:
   `src/Main.Onboarding.cs:72` (`OnShowMeWhereRequested += route => OpenPlayerPanel(route)`) proves
   "close overlay, open panel" works; reuse it rather than inventing a second navigation helper.
4. **Preserve history**: write the same events into the events log (`events_log`) and the journal
   daily brief so a dismissed modal isn't the only copy — Wave 1's 17A step 11, now with real data.
5. **Severity ordering with stable ties**: deaths → warnings → your decisions → consequences →
   ambient, and documented tie-breaks so the same day always reads identically.
6. **Group by subject** where useful (all lines about one survivor together), because a per-event
   list makes a bad day look like many small unrelated facts.
7. **Highlight deltas over levels**: show "−2 cans (ration cut)" not "canned_food: 4" — the fallback
   branch in `src/Main.Campaign.cs:229+` is level-shaped; port its useful bits to delta-shaped
   events instead of deleting them.
8. **Keep the fallback branch honest**: it is the headless path (tests + no-campaign). Make it emit
   the same kinds through `DayEventKinds`, so two branches can't disagree about what a day means.
9. **Keyboard + accessibility**: briefing lines are focusable, actionable from the keyboard, and not
   colour-only for severity (`ashfall-ui-access`).
10. **Attribution to the player's act**: where `causeId` names a player decision (Wave 1's 15B
    choices, Wave 2's assignments), clicking the line opens the source (quest, roster, dispatch) —
    closing the loop from consequence back to the decision that bought it.
11. **Snapshot** the briefing at three densities (quiet day, normal, crisis) to prove no overflow.
12. **Tests**: kind→route mapping completeness, "no route to non-live panel" contract, journal
    parity, keyboard activation, stable ordering.
13. **Run the checklist.**

**DoD:** every briefing line either does something when clicked or is explicitly informational — and
the day's record survives closing the modal.

---

## Task 31C — Diagnostics that match the narrative layer

**Goal:** the same event stream must serve the developer: a machine-readable day record so a bug
report becomes a reproducible scenario, not a screenshot.

**Files:** `src/Main.Campaign.cs`, `Assets/Ashfall.Core/Campaign/DailyBriefingSave.cs`,
`src/Host/CampaignDayPersistenceAdapter.cs`, new `docs/telemetry/DAY_RECORD.md`,
`artifacts/` (gitignored), `src/Main.Lifecycle.cs` (session header), Wave 3's 26C steps 7–8.

### Substeps

1. **Emit a per-day JSON record** (`day_YYYYJJ.jsonl` or a rolling `day-record.jsonl`) containing
   seed, day, owner order, per-owner duration, events (kind/source/ids/numeric), failures, and
   final headline state — reuse the existing `artifacts/` convention, `.gdignore`d.
2. **Write it only in dev/debug builds** (or behind a settings toggle) so shipped builds don't grow
   logs and players don't ship their campaigns to forums by accident.
3. **Per-owner timing** from the coordinator's own loop — this is the data the W3 perf budget needs
   (26C step 1) and it currently doesn't exist per owner.
4. **Ring buffer for the last N days in memory**, dumped on fatal error, so a crash carries the
   sequence that produced it (26C step 8), with no bare `catch { }` (H4's routing pattern).
5. **Replay hook**: a CLI verb that reads a day record and reproduces the reported scenario at the
   same seed/day — the difference between "can't reproduce" and a fixed bug
   (`ashfall-time-travel-debugger` / `ashfall-seed-replay` are the existing tools).
6. **Correlate with the briefing**: the player-visible line and the diagnostic record share
   `kind`/`sourceOwnerId`, so a support question maps 1:1 to a sim fact.
7. **Redact**: no absolute paths beyond the resolved data dir, no usernames, no save contents —
   a diagnostics file that leaks a campaign folder is a privacy bug.
8. **Size bound + rotation** so a 360-day campaign cannot fill a disk.
9. **Document the schema** in `docs/telemetry/DAY_RECORD.md` with a sample record and the field
   list, and cite it from `docs/debug/`.
10. **Tests**: schema stability (a named field set), rotation, replay-of-a-record round trip, and a
    guard that release builds don't write it by default.
11. **Run the checklist** + the W3 export boot smoke (the record must not appear in a shipped
    build's first run).

**DoD:** a bug report containing a seed and a day number is reproducible in one command.

---

## Cross-Task Dependencies

```
31A (vocabulary + producers) ──► 31B (routing/reading) ──► 31C (diagnostics/replay)
        │                              ▲
        ├──► 30A step 6 & 30B (world politics needs kinds to be reportable)
        ├──► 32B (travel/route events), 33B (intel events), 34B (milestone events)
        └──► 25A/25C (keyed briefing text; do this before extraction freezes, or re-do it)
   corrects ──► W1 17A's premise; 17C's audio triggers should subscribe to the same kinds
```

**Execution order:** 31A → 31B → 31C, and **31A first in the whole wave** — Plans 30, 32, 33, 34 all
emit into this vocabulary, so defining it late would mean re-doing four producers.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --day1-selftest                  # briefing content
7. godot --headless --path . -- --real-campaign-journey-selftest # multi-day event stream
8. DayEventKinds contract gate (31A step 7) — 0 unhandled kinds
9. snapshot: briefing at quiet / normal / crisis density
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|
| 31A | 1 new + 1 switch | 1 file (19 owners) | 10–14 | Medium | LOW–MED (briefing content changes → snapshots) |
| 31B | 1 | 2 | 8–12 | Medium | LOW |
| 31C | 0–1 | 2 | 5–8 | Low–Med | LOW (dev-only output) |

**Guardrails:** no new event bus (this is the typed channel that already ships; the `IEventBus`
merge stays out of scope per `AGENTS.md`), no new briefing framework, no per-frame emission, no
player-visible heartbeat noise, and **no unhandled kind reaching a silent `switch` end again** — the
`default:` branch is the whole lesson of this plan.
