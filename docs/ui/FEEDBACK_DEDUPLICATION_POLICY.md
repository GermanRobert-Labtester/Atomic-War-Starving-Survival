# Feedback Deduplication & Spam Suppression Policy

## 1. Objective

Simulation systems that tick continuously (every simulation tick or campaign day) must not overwhelm the player with redundant alerts. Warnings must communicate actionable changes in state without degrading into repetitive visual noise.

---

## 2. Policy Principles

1. **State Transition Triggers:**
   - Warnings fire upon **crossing a threshold** into an alert state (e.g. from Food > 30% to Food <= 30%), not every tick while Food remains <= 30%.
   - When the resource recovers above the threshold, the warning state resets; if it subsequently drops again, a new warning may fire.
2. **Presentation Time Cooldown:**
   - Repeated events with the same `DedupeKey` are suppressed if emitted within `PresentationCooldownSeconds` (default: 5.0 seconds).
   - Cooldown operates strictly on presentation time and does not alter campaign or simulation timing.
3. **Severity Escalation Bypass:**
   - If an event has higher severity than a recently emitted event with the same dedupe key (e.g. escalating from `warning` to `critical`), deduplication suppression is bypassed to alert the player immediately.
4. **Campaign Day Debounce:**
   - Daily survival warnings (e.g. low rations) debounce within the same campaign day so that advancing a single day fires at most one notification per condition.
5. **No Save Persistence:**
   - Deduplication tracking state is ephemeral presentation memory. On game save or UI reload, deduplication state resets cleanly without polluting save envelopes.

---

## 3. Producer Deduplication Rules

| Producer | Dedupe Key Pattern | Trigger Rule | Escalation Path |
|---|---|---|---|
| Hunger / Food | `survival_food_{level}` | Hunger >= 70% (`warning`), >= 90% (`critical`) | Warning -> Critical bypasses cooldown |
| Thirst / Water | `survival_water_{level}` | Thirst >= 70% (`warning`), >= 90% (`critical`) | Warning -> Critical bypasses cooldown |
| Power Grid | `power_grid_{state}` | Normal -> Brownout -> Blackout | Blackout immediately supersedes Brownout |
| Radiation | `radiation_hazard` | Dose rate >= 50 mSv | Fires once upon entering hazard zone |
| Medical Outbreak | `disease_outbreak_{disease}` | Fires once per declared outbreak | Reset only when outbreak resolved |
| Trade Result | `trade_{timestamp}` | Unique per committed transaction | No deduplication needed |
