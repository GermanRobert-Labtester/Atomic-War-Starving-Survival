# C1[8] / Plan 31B–31C — Implementation Log

**Task:** Wave 10 Part 1 — C1[8] "The Event Layer Speaks" (31B navigable briefings + 31C replayable diagnostics)
**Status:** 31A sealed previously (Wave 9 B1) · **31B DONE** · **31C DONE**
**Date:** 2026-09-17
**Authorized by:** user ("i authorise C1 then B1!")

## 31B — Navigable briefings

### Core (engine-free)
- **`Assets/Ashfall.Core/Campaign/BriefingRouteMap.cs` (new)** — the single
  kind/category → route authority. Carries `panel:<id>` route strings only;
  never owns Godot objects.
  - `RouteFor(kind)` — concrete-kind map with semantic fallback.
  - `RouteForSemantic(SemanticKind)` — Casualty/Survivor→`survivor_detail`,
    Hazard/Shelter→`shelter`, Production→`inventory`, Expedition→`expeditions`,
    Communication→`radio`, Weather→`weather`, Narrative→`journal`,
    Heartbeat/Unknown→`null` (informational).
  - `RouteForCategory(category)` — routes handled entries without per-case wiring.
  - `ApplyRoutes(report)` — fills `DeepLinkRoute`/`IsActionable`, preserves
    existing routes, idempotent.
- **`DailyBriefingReportBuilder.cs`** — `DailyBriefingEntry` gained `Kind`,
  `CauseId`, `ActorId`, `IsActionable`; the generic-event branch records
  `Kind = evt.Kind`; `ApplyRoutes` runs at the end of `BuildFromDayEvents`,
  `Build`, and `BuildFromBriefingFacts`.

### Host
- **`src/Main.Campaign.cs`** — `HandleBriefingDeepLink` now validates the target
  against `PanelRegistry` (Plan 16 liveness) before `OpenPlayerPanel`; a
  shelved/unregistered target logs a dev warning and stays informational
  (31B.4/31B.8). The modal already renders `>> GOTO` and hides on activation.

### Tests
- **`Ashfall.Core.Tests/Campaign/Plan31BriefingRouteTests.cs` (new, 6)** —
  every produced route resolves to a **live player-navigable** panel; generic
  non-heartbeat event becomes actionable; unknown kind is informational (no dead
  affordance); handled death routes to the live survivor log (memorial panel is
  a Prototype → live-parent fallback); `ApplyRoutes` idempotent + preserves.

## 31C — Replayable diagnostics

- **`docs/telemetry/DAY_RECORD.md` (new)** — schema v1, writing policy
  (dev-only, JSONL, no wall-clock), seed/session/owner-order/timing/failure rules.
- **`Assets/Ashfall.Core/Campaign/DayRecord.cs` (new)** — `DayRecord` +
  `DayRecordOwner` + `DayRecordEvent` + `DayRecordBuilder.FromDay(...)` /
  `ToJsonLine(...)`. Built from the same `DayAdvancedEventArgs` the briefing
  consumes; engine-free, no timestamps.
- **`CampaignDayCoordinator.cs`** — `DayOwnerReport.DurationMs` (monotonic,
  observational) timed per owner; no behaviour/determinism change.
- **`src/Main.DayRecord.cs` (new)** — opt-in writer, gated by
  `ASHFALL_DAY_RECORD=1`, appends JSONL to `user://day-record.jsonl`; off in
  release. Called from `ShowBriefingForDay`.
- **`Ashfall.Core.Tests/Campaign/DayRecordTests.cs` (new, 4)** — order/events/
  failures captured; null args versioned-empty; JSONL field names + round-trip;
  schema field set pinned.

## Verification

| Check | Result |
|---|---|
| `Plan31BriefingRouteTests` | 6/6 |
| `DayRecordTests` | 4/4 |
| `DailyBriefingReportBuilderTests` / `DayEventVocabularyTests` / `DailyBriefingCrisisTests` / `DayEventSemanticKindTests` | 13/13 · 8/8 · 8/8 · 37/37 |
| Full Core suite | **11,716 / 11,716 PASS** |
| Host build | 0 warnings / 0 errors |
| `--panel-bind-lifecycle-selftest` / `--ui-accessibility-selftest` / `--7-day-smoke-selftest` / `--player-panels-uitest` | PASS |

## Remainder / deferrals

- 31B.9 subject focus (passing survivor/item/expedition id into the target
  panel) is not wired — the route opens the panel without a subject query. The
  `?`-query parsing seam already exists in `HandleBriefingDeepLink`.
- 31B snapshots (quiet/normal/crisis) not added.
- 31C writer is proven by compilation + the Core builder tests; a dedicated
  headless selftest that drives the production briefing path is a follow-up.
