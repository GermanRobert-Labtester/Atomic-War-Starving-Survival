# ASHFALL — First-Hour Onboarding & Tutorial Causality Audit

**Document Reference:** Plan 14 Task 14A Closeout Audit
**Authority:** `OnboardingJourney.cs`, `OnboardingCatalog.cs`, `TutorialPanel.cs`, `OnboardingHintPanel.cs`, `Main.Onboarding.cs`.

---

## 1. Executive Summary

ASHFALL's first hour presents a deep, high-stakes post-nuclear survival simulation. The onboarding architecture uses a deterministic state machine (`OnboardingJourney`) that records genuine player actions without fabricating simulation state. However, telemetry and forensic review identified 3 key teach-before-demand gaps where vital mechanics caused lethal pressure before the player understood the remedy.

---

## 2. Onboarding Surface Audit

### 2.1 Surfaces Inspected:
- `TutorialPanel.cs`: Static reference showing real controls and honest survival basics. Accessible via `F1` or sidebar help.
- `OnboardingHintPanel.cs`: Contextual persistent hint panel displaying current objective, stage checklist, "Show me where" navigation link, skip stage, replay, and assistance levels (Standard, Minimal, Detailed).
- `DailyBriefingModal.cs`: Typewriter-driven opening narrative and daily briefing modal.
- `Main.Onboarding.cs`: Orchestrates sigil observation from runtime actions and saves/restores onboarding progress in the campaign envelope.

### 2.2 Onboarding Stages & Flow:
1. **Protocol (Stage 0):** Resolve Day 1 opening protocol directives (Ration, Maintenance, Radio).
2. **Inspect (Stage 1):** Inspect 3 bunker rooms in Shelter panel.
3. **Rationing (Stage 2):** Open inventory and review food/water stores.
4. **Assignment (Stage 3):** Assign survivor to duty roster shift.
5. **Weather (Stage 4):** Read weather forecast for pending radiation/storms.
6. **InventoryUse (Stage 5):** Use an item from stores (e.g. pharmaceutical or equipment).
7. **DayAdvance (Stage 6):** Advance Day 1 to complete opening cycle.

---

## 3. Key Enhancements Implemented in Plan 14

1. **Contextual Radiation & Triage Hint:**
   - Detects when Gunner Mikhail or any survivor has active acute radiation poisoning on Day 1.
   - Triggers non-modal contextual cue with direct link to MedicalPanel and explanation of Rad-Away usage.

2. **Ration Policy & Depletion Causality:**
   - In Inventory and Day Advance preview, displays exact daily consumption rates and remaining runway in days.

3. **Power & Filtration Warning:**
   - Integrates battery/generator status in HUD status rail with explicit indicator when filtration drops below safe threshold.

4. **Tutorial Persistence & Settings Controls:**
   - Tutorial verbosity configurable in Settings: `All Tutorials` / `Contextual Only` / `Disabled`.
   - Dedicated "Reset Tutorials" button in Settings.
   - Completion and dismissal flags survive save/load seamlessly.

5. **Field Manual Recoverability:**
   - All tutorial basics and tips are permanently accessible in the Journal / Field Manual tab with localized entries and dynamic control prompts.
