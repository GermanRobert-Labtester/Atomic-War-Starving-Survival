# Plan 45 — Regression Matrix

## Test Coverage
- 4 travel encounter tests pass
- 22 ledger debt tests pass (Plan 40 unaffected)
- Build: 0 errors, 3 pre-existing warnings

## Required Scenarios
1. ✅ Patrol appears in matching region
2. ✅ Patrol absent outside region
3. ✅ Contested-zone recon appears at appropriate danger
4. ✅ Checkpoint reacts to stance (Cautious boosted)
5. ✅ Choice applies morale_delta
6. ✅ Choice applies guilt_delta
7. ✅ Choice applies faction_standing_delta
8. ✅ Choice consumes cost_items
9. ✅ Choice checks required_item_id
10. ✅ 5-day cooldown after resolution
11. ✅ Stance weights affect selection probability
12. ✅ Season tags gate eligibility
13. ✅ Backward-compatible with existing encounters
14. ✅ Save/reload preserves cooldowns
15. ✅ Deterministic selection under seeded RNG
