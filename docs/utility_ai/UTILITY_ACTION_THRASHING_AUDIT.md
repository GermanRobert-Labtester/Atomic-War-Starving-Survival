# Utility Action Thrashing Audit

> **Hysteresis & Commitment:** Analysis of decision frequency, action switching, and anti-thrashing mechanisms.

---

## 1. Action Thrashing Risks

When multiple actions score within 0.01 of each other, survivors might rapidly oscillate every evaluation cycle without completing meaningful work.

---

## 2. Mitigation Mechanisms

1. **Commitment / Minimum Duration:**
   - Once a survivor begins an action (e.g. `action_cook_food` or `action_repair_equipment`), they commit to an atomic work unit (typically 1 shift tick or task duration).
   - Re-evaluation occurs at action completion or upon an emergency override event.
2. **Fatigue & State Shifts:**
   - Performing work increases fatigue, which lowers subsequent scores via `fatigueGate` and curves, naturally transitioning the survivor toward rest (`action_rest`) or socializing.
3. **Deterministic Noise Tie-Breaking:**
   - `NoiseScale = 0.0001d` breaks micro-ties deterministically without creating jumpy flip-flops across identical frames.
