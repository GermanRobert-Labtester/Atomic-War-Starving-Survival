# Plan 33 — Skill Catalog Externalization: Baseline Inventory & Scope

## 1. Executive Summary
- **Mission:** Externalize all hardcoded survivor skill definitions from C# (`SkillProgressionSystem.cs`) into the project's authoritative JSON catalog (`Assets/StreamingAssets/Data/skills.json`) while maintaining 100% behavior parity, cross-host save compatibility, and zero simulation drift.
- **Architectural Authority:** Invariant 6 (Data Authority is JSON) & Invariant 1 (Zero Engine Coupling in Core).
- **Previous Baseline:** `SkillProgressionSystem.cs` registered skills inline via C# methods (`RegisterDefaultSkills()`, `RegisterCombatMilestones()`, etc.).
- **New State:** `Assets/StreamingAssets/Data/skills.json` is the sole production catalog containing 148 verified skills (44 base progression skills + 104 latent expert skills).

---

## 2. Skill Catalog Breakdown

| Category | Count | Source & Behavior |
|---|---|---|
| **Action-Driven Disciplines** | 9 | Tier-1 (50 XP threshold, +10% bonus) & Tier-2 Expert (120 XP threshold, +20% bonus) earned via work actions |
| **Domain Milestones** | 32 | Combat (7), Survival (6), Shelter (6), Medical (5), Expedition (5), Social (3) earned via narrative events and quests |
| **Latent Expert Traits** | 104 | Awakened via high-pressure master tasks or narrative revelation (+20% bonus) |
| **Plan 33 Grounded Extensions** | 3 | `skill_field_surgery` (medical), `skill_water_filtration` (survival), `skill_radio_repair` (science) |
| **Total Authored Skills** | **148** | Pure JSON authority loaded via `SkillCatalogLoader` |

---

## 3. Core Engine Decoupling
- Inline skill definition methods (`RegisterCombatMilestones`, `RegisterSurvivalMilestones`, `RegisterShelterMilestones`, `RegisterMedicalMilestones`, `RegisterExpeditionMilestones`, `RegisterSocialMilestones`, `RegisterLatentExpertTraits`) deleted from `SkillProgressionSystem.cs`.
- `RegisterDefaultSkills()` retained as a zero-op for backwards compatibility with legacy callers.
- `SkillCatalogLoader.LoadAndRegister` wires pure JSON data into `SkillProgressionSystem` instances across all sessions and host surfaces.
