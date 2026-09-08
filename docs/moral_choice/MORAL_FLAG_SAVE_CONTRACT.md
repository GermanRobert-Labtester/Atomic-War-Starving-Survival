# Moral Flag Save Contract

Flags remain inside `MoralChoiceState.activeFlags` and the existing `moral_choice` save section. No new save store or envelope was introduced.

- Representation: `List<string>` treated as a set.
- Write: `MoralChoiceSystem.SetFlag`, after a committed option resolution.
- Read: `HasFlag` / `EvaluateGate` and existing restore logic.
- Duplicate writes: ignored in state; shared `IFlagLedger` is set-based.
- Old saves: preserve existing flags; absent Plan 125 IDs read as false.
- Ordering: existing state list order is preserved; no new unordered serialization was introduced.
- Migration: none required for the additive catalog/option field.

The Plan 125 tests cover one-flag round-trip, old-state defaults, repeated writes, and coexistence of opposing flags from separate incidents.
