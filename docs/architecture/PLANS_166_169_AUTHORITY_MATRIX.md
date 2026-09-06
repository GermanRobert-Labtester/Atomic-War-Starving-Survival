# Plans 166–169 Authority Matrix

Verified against branch `feat/asset-pipeline-flagship` at HEAD `052c7353b40f74df7e3e2df4b40fe7cabc3656a5` during the 2026-09-06 implementation pass.

| Feature | Core authority | Save section | Catalog | Godot composition | Status |
|---|---|---|---|---|---|
| Salvage and blueprint progression | `ResearchSystem` + `WorkshopReverseEngineeringSystem` | `research` and existing `crafting` payload | `tech_salvage.json`, `recipes.json` | `CraftingHostSession`, shared research | Implemented |
| Espionage | `EspionageSystem` | `espionage` | `espionage_missions.json` | `EspionageHostSession` | Implemented |
| Fluid distribution | `FluidLogisticsSystem`; treatment remains `WaterTreatmentSystem` | `fluid_logistics` plus existing `water_treatment` | `fluid_infrastructure.json` | `FluidLogisticsHostSession` | Implemented core seam |
| Shared/procedural quests | `QuestRuntimeCoordinator` + `ProceduralNarrativeSystem` | `procedural_narrative` | `quest_templates.json` | `ProceduralNarrativeHostSession` | Implemented core seam |

All new domain rules are engine-agnostic and use deterministic seeded RNG. The Godot host owns catalog loading, lifecycle enrollment, campaign-day registration, save capture, and presentation-facing event text.

Deferred work is intentionally bounded: production UI panels and full consequence adapters for faction standing, disease, greenhouse delivery, map intel, and quest rewards still need to be connected to their mature authorities. The new fluid bridge provides the atomic WaterTreatment transfer seam without creating a second bulk-water ledger.
