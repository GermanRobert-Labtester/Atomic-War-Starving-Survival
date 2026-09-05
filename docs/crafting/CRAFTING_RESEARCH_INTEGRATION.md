# Craft Research Integration (Plan 55)

## Status: one authority; no recipe-side research field

Plan 34 is landed: `research_knowledge.json` defines 56 knowledge nodes
(`knowledge_*`), loaded by `ResearchKnowledgeCatalogLoader` into
`ResearchSystem`. Node completion awards the node's `breakthroughItem` into
the shared inventory exactly once (completion-transition event only, never on
save restore).

**Chosen model (§1.4 Model B-variant):** research owns *when a rare item
enters the economy*; crafting owns *conversion*. Recipes never check research
state, and research never unlocks recipes. There is exactly one gate per
recipe, and it is the ingredient bill.

## Live research→crafting material links (pre-existing, preserved)

| Knowledge node | Breakthrough item | Crafting relevance |
|---|---|---|
| `knowledge_water_advanced` | `item_water_filter_advanced` | advanced water component |
| `knowledge_air_filtration` | `item_air_filter_hepa` | air-filtration family |
| `knowledge_cold_canning_preservation` | `item_vacuum_seal_canner` | preservation batch equipment |
| `knowledge_field_trauma_surgery` | `item_surgical_kit` | medical consumer chain |
| `knowledge_radiation_shielding` | `item_radiation_shielding_panel` | shelter shielding |
| `knowledge_micro_dosimeter_blueprint` | `item_dosimeter_calibrated` | dosimeter family (craft_dosimeter exists) |
| `knowledge_chelation_therapy` | — (unlocks pharma domain) | pharma lab authority |

## Plan-55 recipes and research

No Plan-55 recipe requires a `knowledge_*` node. The advanced tier of the
crafting catalog is already occupied by the pharma lab (26 recipes at
`pharma_bench`), research breakthrough items, and the zero-result
infrastructure sinks — authoring additional research-gated Plan-55 recipes
would duplicate those authorities.

## Deferred

The source plan's "ten advanced research-linked recipes" target is
**explicitly deferred** (Risk 2 / §1.6): the repository-native mechanism
(breakthrough items as rare ingredients) is documented above and is the
recommended surface for any future advanced-recipe work. No speculative
`knowledge_*` ID was authored anywhere.
