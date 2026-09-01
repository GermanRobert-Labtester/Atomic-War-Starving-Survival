# Plan 33 — Skill System Hook Matrix

## 1. System Integration Seams

| Subsystem | Hook Method / Seam | Skill Interaction |
|---|---|---|
| **ApprenticeshipSystem** | `ApprenticeshipSystem.TickDay()` | Mentors train apprentices toward target skills (`pair.targetSkillId`). Upon graduation, awards target skill via `SkillProgressionSystem`. |
| **LibraryStudySystem** | `LibraryStudySystem.TickDay()` | Reading manuals grants action XP and awards designated technical discipline skills. |
| **LatentExpertAwakeningSystem** | `LatentExpertAwakeningSystem.RecordProgress()` | High-stress crisis milestones unlock latent traits (`trait_*`) into active skills (`skill_*`). |
| **SkillAtrophySystem** | `SkillAtrophySystem.Tick()` | Prolonged low morale (<10) causes unused discipline skills to become dormant. |
| **NeedsSystem & Medical** | `MedicalSystem` / `Clinic` | `skill_field_dressing`, `skill_steady_hands`, and `skill_field_surgery` boost medical treatment efficiency and reduce contamination risk. |
| **PowerGrid & Workshop** | `PowerGridSystem` / `Workshop` | `skill_rough_repairs`, `skill_workshop_sense`, and `skill_jury_rigger` reduce component breakdown rates and scrap waste. |
| **Radio & Signals** | `RadioSystem` / `Transmitter` | `skill_signal_ear` and `skill_radio_repair` enhance broadcast signal clarity and tuning speed. |
| **Water & Filtration** | `WaterSystem` / `Pump` | `skill_water_filtration` enhances clean water output and filter lifespan. |

---

## 2. Host Wiring & UI Surfaces

| Host Surface | Component | Integration Path |
|---|---|---|
| **SkillMatrixPanel** | `src/UI/SkillMatrixPanel.cs` | Displays active, dormant, and expert skills across all living survivors in the shelter. |
| **SurvivorDetailPanel** | `src/UI/SurvivorDetailPanel.cs` | Shows individual survivor discipline bonuses, active skills, and expert trait status. |
| **ApprenticeshipPanel** | `src/UI/ApprenticeshipPanel.cs` | Selects qualified mentors and displays graduation progress toward target skills. |
| **LibraryStudyPanel** | `src/UI/LibraryStudyPanel.cs` | Tracks study progress and skills learned from technical manuals. |
