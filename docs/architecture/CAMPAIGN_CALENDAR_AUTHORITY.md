# ASHFALL Campaign Calendar Authority & Time Invariants

This document specifies the authoritative campaign calendar hierarchy, time domains, clock projections, and reconciliation rules in **ASHFALL**.

---

## 1. Time Domains & Distinctions

ASHFALL separates time into three distinct, non-conflated domains:

| Time Domain | Authority Type | Type / Interface | Resolution / Unit | Persistence & Ownership |
|---|---|---|---|---|
| **Campaign Day** | Discrete, monotonic integer | [`ICampaignCalendar`](../../Assets/Ashfall.Core/Campaign/CampaignCalendar.cs) | In-game Days (1, 2, 3, …) | Persisted in `campaign.json` via `campaign_day` section; owned exclusively by `CampaignDayCoordinator`. |
| **Simulation Ticks** | Continuous intraday sub-clock | [`ISimClock`](../../Assets/Ashfall.Core/Clock/ISimClock.cs) | Discrete Ticks (60 ticks/hour, 1,440 ticks/day) | Ephemeral / intraday state for machines, radio pulses, radiation drift, and day-night cycles. |
| **Wall-Clock Time** | Real-world wall time | `DateTime.UtcNow` | Milliseconds / UTC timestamps | Diagnostics, session logs, UI cooldown throttling, and save metadata. Never drives gameplay rules. |

---

## 2. Single Writer Architecture

```
                    ┌─────────────────────────┐
                    │   User 'End Day' Click  │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │   Main.CommitAdvance    │
                    └────────────┬────────────┘
                                 │
                                 ▼
                  ┌──────────────────────────────┐
                  │   CampaignDayCoordinator     │
                  │   - Re-entrancy guard        │
                  │   - 17 Phase-ordered owners  │
                  │   - Fail-closed atomicity    │
                  └──────────────┬───────────────┘
                                 │ On All Owners Succeed
                                 ▼
                  ┌──────────────────────────────┐
                  │   ICampaignCalendar (Core)   │
                  │   - Authoritative day setter │
                  │   - Raises OnDayChanged      │
                  └──────────────┬───────────────┘
                                 │
       ┌─────────────────────────┼─────────────────────────┐
       ▼                         ▼                         ▼
┌──────────────┐         ┌───────────────┐         ┌───────────────┐
│ Main._simDay │         │ Clock Adapter │         │ SimClock Adpt │
│ (Read-Only)  │         │ (IClock)      │         │ (ISimClock)   │
└──────────────┘         └───────────────┘         └───────────────┘
```

### Invariants:
1. **Single Writer**: Only [`CampaignDayCoordinator`](../../Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs) can advance [`ICampaignCalendar`](../../Assets/Ashfall.Core/Campaign/CampaignCalendar.cs).
2. **Projections are Read-Only**: `_simDay` in `Main` is a read-only property (`_campaignDay.Calendar.CurrentDay`).
3. **No Secondary Authority**: Subsystems (Journal, Verdict, Duty Roster, Year of Ash) never mutate or derive the master day.
4. **Fail-Closed Protection**: If any daily advance owner fails or throws, the calendar day is not advanced, persistence is aborted, and the pre-day snapshot remains uncommitted.

---

## 3. Save Reconciliation Rules

When loading saves from disk, conflicting legacy section day timestamps are reconciled via [`CampaignCalendarReconciler`](../../Assets/Ashfall.Core/Campaign/CampaignCalendar.cs):

1. If `campaign_day` exists and `lastAdvancedDay > 0`, it is authoritative.
2. If `campaign_day` is absent (legacy save), `holdfast.day` is the primary fallback, followed by `max(section_days)`.
3. Any section whose recorded day diverges from the authoritative day is logged with structured diagnostic format:
   `[CALENDAR_MISMATCH] section='{section}' section_day={sectionDay} authoritative_day={authDay}`
