# Recipe Unlock Authority (Plan 55)

## Finding

The repository has **no recipe discovery/unlock registry**. Every recipe in
`recipes.json` is known to the player as soon as the catalog loads. There is
no save field for "known recipes", no `ResearchSystem` recipe-unlock payload,
and no recipe-side `skill_prerequisite`/`research_prerequisite` field consumed
by any runtime code.

## Model decision

**Model: catalog membership = knowledge; qualification = station + ingredients.**

- **Data owner:** `recipes.json` (the only recipe authority).
- **Runtime evaluator:** `CraftingSystem.CanCraft` (station operational →
  craft-result gate → ingredient bill → output capacity).
- **UI visibility:** all catalog recipes listed; ineligibility is explained by
  existing `CommandPreview` lock reasons (`station_unavailable`,
  `insufficient_ingredients`, `inventory_full`, `result_restricted`,
  `moonshine_restricted`) — no new UI surface required.
- **Save owner:** only `ActiveCraftSave` (in-progress jobs). Recipe knowledge
  needs no save state because it is static.
- **Old-save default:** new recipes appear automatically; no fabricated
  unlocks; in-progress jobs referencing legacy IDs restore unchanged.

## Research integration (the live surface)

`ResearchSystem.OnResearchCompleted` awards the node's `breakthroughItem`
into the shared inventory exactly once (completion-transition only; never on
save restore — see `CraftingHostSession`). Example: `knowledge_water_advanced`
→ `item_water_filter_advanced`.

Plan 55 does **not** add research fields to recipes. Advanced crafting is
gated *economically* by rare breakthrough items where the data already links
them. This is the one-authority rule: research owns when an item enters the
economy; crafting owns conversion; neither duplicates the other.

## Skill integration (the live surface)

Crafters modify cost/time through `SetCrafterCostMultiplier` /
`SetCrafterCraftTimeMultiplier`; the pharma lab binds chemist skill through
its own evaluator (`skill_medical_doctor`, `skill_chemistry_specialist`); the
workshop binds `skill_crafting_expert` / `skill_scavenge_efficiency`. Skills
qualify **crafters**, not recipes. See `CRAFTING_SKILL_INTEGRATION.md`.

## Explicitly rejected

- Model A (recipe-side prerequisite strings): no consumer reads them.
- Model C (dual discovery/qualification): the architecture does not model it.
- Adding a new unlock registry: violates §12 non-goals.
