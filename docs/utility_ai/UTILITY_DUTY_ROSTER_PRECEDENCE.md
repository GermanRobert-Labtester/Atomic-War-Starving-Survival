# Utility Duty Roster Precedence

> **Precedence Contract:** Authority relationship between explicit DutyRoster / player assignments and autonomous Utility AI decisions.

---

## 1. Precedence Hierarchy

```
[Level 1: Emergency Override]
  - Acute life-safety override actions (`isOverrideAction == true`)
        │
        ▼
[Level 2: Explicit Player Orders & Duty Roster Assignments]
  - Guard duty at sentry post
  - Assigned shift in kitchen / workshop / greenhouse / clinic
  - Manual player command
        │
        ▼
[Level 3: Critical Autonomous Needs]
  - Exhausted survivor collapses to rest (`action_rest`)
  - Starving survivor seeks sustenance
  - Critical medical triage (`action_treat_wounded`)
        │
        ▼
[Level 4: Discretionary Idle Autonomy]
  - Socializing in mess hall (`action_socialize`)
  - Voluntary skill practice (`action_train_skill`)
  - Maintenance backlog (`action_repair_equipment`)
  - Slack research (`action_conduct_research`)
```

---

## 2. Duty Roster Invariants

1. **Assigned Work Wins Over Discretionary Autonomy:** A survivor assigned to the kitchen by the player will not wander off to socialize or practice tool handling during their shift.
2. **Duty Gaps Enable Autonomy:** When off-shift, unassigned, or when assigned tasks run out of materials (e.g. workshop lacks metal), the survivor transitions to autonomous Utility AI decisions.
3. **Emergency Interruption:** If an emergency override triggers, the survivor interrupts routine duty, resolves the crisis, and returns to their assigned station once danger abates.
