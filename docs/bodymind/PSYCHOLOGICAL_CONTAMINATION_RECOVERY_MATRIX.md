# Psychological Contamination Recovery Matrix

This document defines the recovery mechanisms that allow survivors to process psychological contamination and return to normal duty without permanent crippling debuffs.

| Recovery Method | Execution Context | Mechanism / Rate | System Responsible | Conditions & Modifiers |
| :--- | :--- | :--- | :--- | :--- |
| **Natural Time Decay** | Campaign simulation ticks | `DaysRemaining -= gameDays` | `PsychologicalContaminationSystem.Tick` | Baseline recovery of 1 day per 24 hours spent away from hazard sites. |
| **Safe Shelter Rest** | Resting in bunker bunk / living quarters | Accelerated decay (+0.5 days/day) | `NeedsSystem` / `ShelterRest` | Requires shelter warmth > 10°C, adequate water, and no active shelter crises. |
| **Companion Grounding** | Paired expedition or shift with high-bond survivor | Immediate -1 day on entry duration | `CombatTraumaSystem` / Relations bridge | Requires companion Bond > 50 or companion with `trait_calm` / `trait_empathic`. |
| **Avoidance of Hazard Sites** | Expedition roster management | Prevents restacking of duration | Player / Roster Management | Giving the survivor domestic tasks (workshop, tailoring) allows full recovery within 2–5 days. |
| **Comfort / Music / Vinyl** | Shelter morale recreation | -0.5 days across all active entries | `VinylMoraleSystem` / Archive Desk | Listening to preserved vinyl tracks or family letters provides emotional anchoring. |
