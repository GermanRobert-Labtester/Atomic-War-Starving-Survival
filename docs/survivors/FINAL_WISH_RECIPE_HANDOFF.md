# Final Wish Recipe & Meal Handoff Integration

**Document:** `docs/survivors/FINAL_WISH_RECIPE_HANDOFF.md`

---

## 1. Authored Meal Wishes (2 Wishes)

Two wishes center around food memories, using ingredients and preparations tied to canonical recipes in `recipes.json`:

| Wish # | Archetype | Title | Ingredients Required | Cooking / Food System Tie-in |
|---|---|---|---|---|
| **22** | `the_chef` | The Simmered Root | `crop_hardy_tuber`, `item_preservation_salt`, `clean_water` | Simmered root potage on the shelter stove; utilizes harvested greenhouse produce and preserved salt |
| **23** | `the_exhausted_father` | Porridge for the Dawn | `canned_food`, `clean_water` | Heating preserved rations with warm water to feed children; kitchen/galley prep |

---

## 2. Resource Consumption Discipline

- Ingredients are consumed atomically when the step resolves.
- No impossible gourmet ingredients are required (e.g. no fresh meat or dairy; strictly preserved rations, tubers, salt, and water).
