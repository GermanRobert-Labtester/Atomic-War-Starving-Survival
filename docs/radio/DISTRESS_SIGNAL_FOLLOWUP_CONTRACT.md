// SPDX-License-Identifier: MIT
# DISTRESS SIGNAL FOLLOW-UP CONTRACT (Tasks 9–12 Wave 3)

> Status: **SEALED — Wave 3.** Live authority for distress follow-up
> transmissions. Baseline evidence:
> `docs/radio/DISTRESS_SIGNAL_TASKS_9_12_BASELINE.md` (PC-4).

## 1. Stage vs follow-up (normative distinction)

- **Stage** — same signal ID, same lifecycle, time-based presentation/
  intelligence progression (`message_fragments`, absolute-day anchor).
  Reaching a later stage is not an event and schedules nothing.
- **Follow-up** — a distinct authored transmission caused by a qualifying
  parent-signal OUTCOME (player action or authoritative resolution), with
  its own campaign-day delay, content, and identity.

## 2. Schema (additive `follow_up_signals` on distress broadcasts)

```json
"follow_up_signals": [
  {
    "id": "fu_stable_unique_id",
    "trigger_condition": "rescue_success",
    "delay_days": 5,
    "text": "...",
    "clarity": 0.95,
    "outcome_hint": "optional clue"
  }
]
```

Wave 3 uses **self-contained payloads, NOT catalog-backed signal
references** — follow-up chains/cycles (A→B→A) are therefore structurally
impossible. If catalog-backed references are added later, cycle validation
becomes mandatory before any authored data ships (plan §11F).

## 3. Trigger grammar (closed, validated)

| Trigger | Producer (exactly-once lifecycle transition) |
|---|---|
| `answered` | `RecordExpeditionDispatched` → Dispatched |
| `rescue_success` | `RecordDestinationReached` → TerminalRescued |
| `rescue_failed` | `RecordDestinationReached` → TerminalFailed with `ArrivalResolved` (late arrival) |
| `expired` | Deadline expiry of a discovered, actionable signal — legacy `TerminalFailed` without arrival OR `OnIgnoreConsequence` |
| `ambush_encountered` | `RecordDestinationReached` → TerminalAmbush |

Anything else is validator-rejected. Authored design facts preserved:
ignoring a trap is wisdom (no trust/trigger consequence); a genuine-path
(`rescue_success`) follow-up can never fire from a trap resolution; trap
aftermay content may legitimately use `ambush_encountered`.

## 4. Scheduling contract (`DistressFollowUpScheduler`)

- due day = **event-day + delay_days** (campaign days only, never wall clock);
- the scheduler's `CurrentDay` is set by the host BEFORE the mission tick
  (lifecycle events carry no day parameter);
- scheduling is exactly-once per `(parentSignalId, followUpId)` — pending
  set blocks duplicates, fired ledger blocks re-scheduling;
- firing is exactly-once via the persisted fired-key ledger (ClaimedReceipts
  pattern); loading on/after the due day fires on the next tick;
- same-day order is deterministic: `(dueDay, parentSignalId, followUpId)`
  ordinal sort;
- a pending entry whose parent definition was removed expires silently.

## 5. Save contract (V6)

`RadioSaveState.signalFollowUps` (`SignalFollowUpSaveState`): pending list
(parent, followUpId, dueDay — sorted) + fired keys (sorted). V5→V6
frozen-shape migration (`RadioSaveStateFrozenV5`) → empty scheduler default.
Restore fires no events, so a restored pending follow-up fires exactly once.

## 6. Validation (data-integrity gate)

Inside `ValidateDistressSignalStages`: unique follow-up IDs across the
corpus, closed trigger grammar, `delay_days >= 0`, non-empty text, clarity
in [0,1] when present, present-or-empty outcome hints. Errors name catalog,
signal, follow-up path, follow-up id, field, value, and rule.

## 7. Host wiring

`RadioHostSession` owns the scheduler, binds it to the mission manager's
events, sets its day before the mission tick, ticks it after, surfaces fired
transmissions as the session event line, and captures/restores the state in
the radio save section.

## 8. Tests

`Ashfall.Core.Tests/Radio/DistressFollowUpTests.cs` (19 cases): qualifying
triggers, exact timing, distinct content, determinism, save/load (pending +
fired), ignored/trap suppression, trap aftermath, duplicate
scheduling/firing prevention, same-day ordering, terminal-parent
non-rescheduling, validator rules, DTO binding, V6 round-trip, V5→V6
migration, and the stage-vs-follow-up distinction.
