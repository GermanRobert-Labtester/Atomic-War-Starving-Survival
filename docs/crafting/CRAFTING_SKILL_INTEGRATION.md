# Craft Skill Integration (Plan 55)

## Status: efficiency-only; recipe-side skill gating is NOT supported

The recipe DTO has no `skill_prerequisite` field and no runtime code reads
one. The live skill surfaces are:

| Surface | Mechanic | Where |
|---|---|---|
| Crafting cost/time | `CraftingSystem.SetCrafterCostMultiplier` / `SetCrafterCraftTimeMultiplier` (crafterId-scoped) | `CraftingSystem.cs` |
| Workshop evaluator | `skill_crafting_expert` (+0.5), `skill_scavenge_efficiency` (+0.3) trait bindings | `Main.World.cs` `SetupCrafting` |
| Pharma chemist evaluator | `skill_medical_doctor` (+0.5), `skill_chemistry_specialist` (+0.4) | `Main.World.cs` |
| Survivor skills catalog | 148 skill defs (`skills.json`), e.g. `skill_wasteland_brewer`, `skill_butcher`, `skill_pharmacologist` | `SkillDef` / `SkillCatalogLoader` |

## Plan-55 recipes and skills

All 8 Plan-55 recipes are authored **without** skill gates (unsupported).
Skill differentiation still applies at runtime through the crafter
multipliers above: a `skill_crafting_expert` survivor crafts any Plan-55
recipe cheaper and faster; pharma work is chemist-gated by the existing lab.

## Per-recipe skill affinity (future wiring, NOT in data)

If a recipe-side gate is ever added generally, the natural affinity mapping is:

| Recipe | Conceptual skill (from `skills.json`) |
|---|---|
| `craft_flatbread`, `craft_boiled_roots`, `craft_vegetable_soup` | `skill_iron_stomach` / ration-stretcher family |
| `craft_pemmican`, `craft_travel_ration` | `skill_butcher` / preservation |
| `craft_splint` | `skill_field_dressing` |
| `reload_556`, `reload_762` | `skill_cold_bore` / armorer family |

No `skill_*` ID was authored into any data file; nothing in this document
weakens integrity validation. The source plan's "five skill-gated intermediate
recipes" target is **explicitly deferred** pending a general recipe-side skill
gate (Risk 1 / §1.5).
