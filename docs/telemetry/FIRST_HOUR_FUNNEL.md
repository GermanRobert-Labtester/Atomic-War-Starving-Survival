# ASHFALL First-Hour Funnel Report

## Session `local_session`

- Events: 14 · Max day: 4
- **Live first-hour progress: 1/7 (14%)**

| # | Step | Reached | Day |
|---|------|---------|-----|
| 1 | Water treatment started | no | — |
| 2 | Breaker toggled | no | — |
| 3 | Food ration consumed | no | — |
| 4 | Duty assigned | no | — |
| 5 | Dose reading opened | no | — |
| 6 | Research started | no | — |
| 7 | Expedition dispatched | yes | 2 |

- Canonical funnel: 2/13 steps
- Top actions: `session_start`×10, `day_advanced`×3, `sigil`×1

## Session `unknown`

- Events: 1 · Max day: 0
- **Live first-hour progress: 0/7 (0%)**

| # | Step | Reached | Day |
|---|------|---------|-----|
| 1 | Water treatment started | no | — |
| 2 | Breaker toggled | no | — |
| 3 | Food ration consumed | no | — |
| 4 | Duty assigned | no | — |
| 5 | Dose reading opened | no | — |
| 6 | Research started | no | — |
| 7 | Expedition dispatched | no | — |

- Canonical funnel: 0/13 steps

---

---

## Delta — 2026-10-01

P001 instrumentation re-run (`scripts/tools/first_hour_funnel.py`). Per-session live first-hour progress, verb histogram, and hint engagement.

| Session | Events | Max day | Live progress | Top actions | Hint engagement |
|---|---|---|---|---|---|
| `local_session` | 859 | 4 | 6/7 | `session_start`×546, `day_advanced`×174, `sigil`×80, `survivor_perished`×39, `water.start`×3, `power.breaker`×3, `food.consume`×3, `duty.assign`×3 | `research` 1/1/no |
| `probe_session` | 2000 | 405 | 7/7 | `panel_opened`×1983, `sigil`×10, `day_advanced`×3, `water.start`×1, `hint_shown`×1, `hint_dismissed`×1, `expedition.returned`×1 | `water` 1/1/yes |
| `unknown` | 1 | 0 | 0/7 | — | none recorded |
