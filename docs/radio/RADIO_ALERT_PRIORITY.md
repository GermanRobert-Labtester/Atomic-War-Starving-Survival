# Radio Alert Priority & Anti-Spam Policy

> **Document Status:** Authoritative Broadcast Priority Standard
> **Authority:** Plan 24 (Task 24AS)

---

## 1. Alert Priority Hierarchy

When multiple broadcasts are eligible on a given frequency, the radio system resolves content according to strict priority tiers:

```text
Tier 4: EMERGENCY (Flash Traffic)
  - Severe Fallout Storm / Tornado Alert (Plan 19)
  - Orbital Harrow Kinetic Deorbit Warning (Plan 19)
  - Imminent Perimeter Raid Warning
  --> Overrides all routine programming immediately; sounds emergency tone.

Tier 3: URGENT (Critical Operational Data)
  - Active Distress SOS Intercept (New unlogged distress call)
  - Disease Outbreak / Contaminated Water Notice (Plan 09)
  - Railway Bridge Demolition Countdown (Detachment 9)
  --> Interrupts regular chatter; logged prominently in Signal Log.

Tier 2: IMPORTANT (Appointment Programming & High Lore)
  - Daily Morning Weather & Fallout Forecast
  - Missing Persons / Survivor Message Roll
  - Market & Caravan Arrival Bulletin
  - Verdict Census Call (Day 210+)
  --> Scheduled broadcast windows; airs during appointed time slots.

Tier 1: ROUTINE (Atmosphere & Faction Chatter)
  - Faction ambient intercept chatter
  - Cultural vinyl music shortwave broadcasts
  - Numbers station periodic chimes
  - Automated carrier hum and static events
  --> Default ambient texture when no higher-tier alert is active.
```

---

## 2. Anti-Spam Cadence Rules

1. **Coalescence:** If both an approaching weather front and a disease alert occur on the same day, they are formatted into sequential segments rather than spamming duplicate alerts.
2. **Frequency Deduping:** Once an emergency voice-over cue has played for a specific broadcast, subsequent tuning to that frequency during the same day renders the text transcript without repeating the jarring voice clip.
