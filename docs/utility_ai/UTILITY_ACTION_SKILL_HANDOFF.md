# Utility Action Skill Handoff

> **Skill Seams:** Mapping between skill-sensitive utility actions and Plan 33 skill IDs (`skills.json`).

---

## 1. Three Core Skill-Integrated Actions

1. **`action_repair_equipment` → `skill_rough_repairs` / `skill_workshop_sense`**
   - *Authority:* Plan 33 crafting/mechanic skills.
   - *Mechanic:* Survivor's `CraftingSkill` scales `skillBonusFactor = 0.3`. A skilled mechanic scores repair work significantly higher than an untrained survivor, naturally drawing them to the workshop.

2. **`action_treat_wounded` → `skill_field_dressing` / `skill_steady_hands`**
   - *Authority:* Plan 33 medical skills.
   - *Mechanic:* `skillBonusFactor = 0.2`. Medics with medical training experience a higher baseline drive to administer aid, while traits like `hitman` or `germaphobe` gate inappropriate actors.

3. **`action_conduct_research` → `skill_cold_analysis`**
   - *Authority:* Plan 33 science/analysis skills.
   - *Mechanic:* `skillBonusFactor = 0.25`. Analytical survivors have elevated raw scores for deciphering pre-war engineering schematics and laboratory logs.

---

## 2. Dynamic Training & Teaching Seams

- **`action_train_skill`:** Dynamically targets the survivor's lowest non-maxed skill relevant to their background (e.g. `skill_field_dressing`, `skill_rough_repairs`, `skill_ration_stretcher`).
- **`action_teach_skill`:** Requires the mentor's skill tier to exceed the apprentice's tier, transferring modest XP increments without unbounded farming.
