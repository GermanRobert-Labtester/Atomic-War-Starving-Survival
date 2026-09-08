# Plan 142 Timestamp Policy

## Rules

1. Canonical-shaped records preserve their authored `day`, `hour`, and
   `timestamp` after validation.
2. The loader accepts hours in the existing journal range `[0, 24)`.
3. The explicit canonical timestamp must equal
   `JournalVoice.FormatTimestamp(day, hour)`. A mismatch is invalid data, not
   a reason to fabricate a replacement.
4. Ambient records have day only. Their adapter hour is `-1`, and the
   canonical display is exactly `Day N`. No hour, minute, wall-clock, or
   timezone is invented.
5. Existing runtime producers continue to pass their actual simulation day and
   optional simulation hour. The adapter never uses `DateTime`, process time,
   or a random value.

## Ordering

`JournalSystem` remains newest-first in insertion order. Authored activation
does not replay a whole corpus on startup, so it cannot reorder a save or
flood the 64-entry ring.

When two real producers activate records in one simulation step, the order is
the order of producer calls. The authored day/hour are display provenance, not
a second sorting state. Save and restore preserve the existing entry array
order exactly.
