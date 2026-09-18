# DAY_RECORD — Replayable Diagnostics Schema

**Status:** CURRENT · **Plan:** C1[8] / Plan 31C · **Schema version:** 1
**Owner:** Campaign-day coordinator (`Assets/Ashfall.Core/Campaign/`)
**Builder:** `DayRecordBuilder` (`Assets/Ashfall.Core/Campaign/DayRecord.cs`)

A `DayRecord` is one machine-readable line describing a single simulated day,
built from the **same** owner reports and event stream the daily briefing
consumes. A bug report containing seed + day can be investigated from the
record without re-running the game blindly.

## Record shape

```json
{
  "schemaVersion": 1,
  "sessionId": "campaign-slot-1",
  "seed": 12345,
  "day": 42,
  "ownerOrder": ["power", "needs", "radio"],
  "owners": [
    { "ownerId": "power", "durationMs": 0.42, "failed": false, "failureCode": null }
  ],
  "events": [
    { "kind": "power_shed_automatic", "sourceOwnerId": "power", "primaryId": "room_ward",
      "secondaryId": "", "numeric": 0, "causeId": "", "actorId": "" }
  ]
}
```

## Policy

| Rule | Decision |
|---|---|
| Versioning | integer `schemaVersion`; tooling must read older versions backward; field-set changes bump the version (tests pin fields) |
| Writing | **dev/debug only** — enabled by build/setting/CLI flag; **off in release** |
| Output | one JSON object per line (JSONL), append-safe, under the app/user log path (never repo-absolute) |
| Seed/session | campaign seed + day + session/slot id; **never** account/user identity |
| Owner order | actual execution order (a failure can depend on ordering) |
| Timing | monotonic per-owner `durationMs` only — **no wall-clock timestamps**, no determinism impact |
| Failure capture | per-owner `failed` + safe failure code/message |

## Producer contract

- `DayRecordBuilder.FromDay(seed, sessionId, day, DayAdvancedEventArgs)` — pure.
- `DayRecordBuilder.ToJsonLine(record)` — compact JSON line.
- Per-owner duration comes from `DayOwnerReport.DurationMs` (monotonic, observational).

## Consumer contract

- A day record must be reproducible from `(seed, day)` plus the record itself
  (owners ordering + events); it is diagnostics, not a save.
- Diagnostics data is never read back into simulation state.
