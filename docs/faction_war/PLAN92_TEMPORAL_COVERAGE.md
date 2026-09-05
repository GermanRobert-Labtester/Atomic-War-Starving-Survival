# Plan 92 — Temporal Coverage & Reachability Analysis

> **Operational Arc:** Faction War Narrative Layer (Days 480–605+)
> **Onset Engine:** `s.minDay <= currentDay`

---

## 1. Temporal Banding & Distribution

The 40 snippets are distributed across the 5 canonical onset bands of the Faction War timeline:

```text
  Day 480                 Day 500              Day 525            Day 560            Day 585           Day 605
    ├─── Opening (13) ─────┼─── Early (8) ──────┼─── Mid (9) ──────┼─── Late (8) ─────┼── Endurance (2) ──┤
    │                      │                    │                  │                  │                   │
  d482 (Garrison)        d502 (Foundry)       d526 (Exchange)    d562 (Garrison)    d584 (D/9 Cell)
  d483 (Exchange)        d505 (Garrison)      d528 (Foundry)     d566 (Independent) d591 (Pilgrim)
  d485 (Exchange)        d508 (Exchange)      d530 (Exchange)    d568 (Exchange)
  d486 (Garrison)        d512 (Exchange)      d534 (Independent) d571 (Forward R.)
  d487 (Civilian)        d516 (Garrison)      d538 (Garrison)    d573 (Forward R.)
  d488 (Understory)      d518 (Understory)    d542 (Garrison)    d574 (Civilian)
  d489 (Exchange)        d520 (Civilian)      d546 (Understory)  d576 (Understory)
  d490 (Civilian)                             d549 (Civilian)    d580 (Shrine)
  d492 (Understory)                           d552 (Garrison)
  d493 (Independent)                          d556 (Foundry)
  d494 (Garrison)
  d497 (Independent)
  d498 (Independent)
```

---

## 2. Checkpoint Pool Sizes Across Campaign Progression

At representative campaign checkpoints, the total available snippet pool expands monotonically:

| Checkpoint Day | Narrative Context | Cumulative Eligible Snippets | % of Total Corpus Unlocked |
|---|---|---|---|
| **Day 480** | Pre-war friction / border skirmishes | 0 | 0% |
| **Day 485** | Opening markets, tally audits | 3 | 7.5% |
| **Day 500** | Cold war onset, checkpoint controls | 13 | 32.5% |
| **Day 525** | Initial skirmishes, Almshouse shelling | 20 | 50.0% |
| **Day 550** | Plaza strike aftermath, grain siege | 29 | 72.5% |
| **Day 575** | Forward Roster road tolling, late war weariness | 36 | 90.0% |
| **Day 600+** | Ceasefire by exhaustion, crater investigations | 40 | 100.0% |

---

## 3. Reachability & Expiry Verification
- **Unreachable Snippets:** Exactly **0**. The maximum `minDay` is 591 (`dlg_d591_switchback_waystation_doubt`), which sits comfortably before the Day 605 narrative climax.
- **Premature Expiry Risk:** Handled via the **Evergreen-After-Onset Rule**. All snippets describe systemic, ongoing procedural friction, craft methods, institutional memory, or permanent aftermath realities rather than transient "yesterday morning" headlines.
