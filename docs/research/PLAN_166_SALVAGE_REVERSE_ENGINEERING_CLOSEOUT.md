# Plan 166 — Salvage & Reverse Engineering Closeout

`WorkshopReverseEngineeringSystem` remains the sole workshop authority. The implementation adds `PreWarTechDef` catalog loading, deterministic dismantle preview/resolution, authored salvage yields, research-point grants, blueprint progress, equipment quality, structured research notes, and bounded catastrophic failure outcomes.

`ResearchSystem` owns the research-point wallet and `BlueprintProgressState`. Authored recipes use `requiredBlueprintId`, so recipe availability changes through the existing crafting gate rather than runtime recipe fabrication. Existing research completion and breakthrough behavior remain intact.

Changed surfaces:

- `Assets/Ashfall.Core/Research/ResearchSystem.cs`
- `Assets/Ashfall.Core/Research/ResearchState.cs`
- `Assets/Ashfall.Core/Research/TechSalvageCatalog.cs`
- `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs`
- `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`
- `Assets/StreamingAssets/Data/tech_salvage.json`

Focused verification: `Plan166ResearchSalvageTests` passed 6/6. Core build passed. Partial workshop UI and broader lab-facility integration remain follow-up work.
