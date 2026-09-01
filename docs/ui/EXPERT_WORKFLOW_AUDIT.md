# ASHFALL — Expert Workflow & Thousandth-Tick Efficiency Audit

**Audit Reference:** Plan 14 Task 14E / 14F
**Target:** High-frequency administrative workflows across long campaigns (100–1000+ ticks).

---

## 1. High-Frequency Workflow Friction Study

| Workflow | Pre-Plan 14 Friction (Clicks / Transitions) | Post-Plan 14 Optimized Path | Avoidable Friction Eliminated |
|---|:---:|:---:|---|
| **1. Triage Endangered Survivor** | 5 clicks (Sidebar -> Survivors -> Find Mikhail -> Medical -> Administer) | **2 actions** (Direct HUD Critical Badge or Hotkey `M` -> Survivor auto-selected -> Administer) | Eliminated manual roster hunting; endangered survivor pre-selected. |
| **2. Check Rations & Water Runway** | 4 clicks (Sidebar -> Inventory -> Filter Food -> Calculate days) | **1 glance / 1 click** (Status Rail shows "Water: 12 (3.3d)" -> Click opens directly to Food/Water tab) | Eliminated mental math; consumption rate & days remaining pre-computed. |
| **3. Reassign Duty Shift** | 4 clicks (Sidebar -> Duty Roster -> Select Slot -> Confirm) | **2 clicks** (Hotkey -> Drag or click quick-swap slot) | Kept last active shift tab in view. |
| **4. Check Weather & Radiation Risk** | 3 clicks (Sidebar -> Weather -> History) | **1 keypress** (`F` for Forecast / `H` for History) | Direct global hotkey navigation. |
| **5. Advance Day** | 3 clicks (Advance -> Confirm -> Briefing dismissal) | **1 keypress** (`Enter` advances and acknowledges) | Typewriter skip on first press, confirm on second press. |
| **6. Reference Survival Rule / Help** | 3 clicks (Open menu -> Help -> Scroll) | **1 keypress** (`F1` or `J` tab 5 opens Field Manual) | Direct field manual glossary. |

---

## 2. Selection & Context Preservation

1. **Remembered Tab & Selection:** Switching between Survivors and Medical retains the actively selected survivor ID.
2. **Filter State Resilience:** Inventory filter category (All / Food / Medical / Materials / Equipment) persists during a session.
3. **Non-Blocking Guidance:** Experienced players with `Tutorial: Disabled` or `Contextual Only` experience zero onboarding popups while retaining full access to the Field Manual via `J` (Journal) or `F1`.
