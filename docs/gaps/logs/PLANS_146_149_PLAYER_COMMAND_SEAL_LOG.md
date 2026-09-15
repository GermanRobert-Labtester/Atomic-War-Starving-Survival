# Plans 146–149 Player-Command Seal Log

**Date:** 2026-09-12  
**Scope:** Industrial flagship set (EB-PVD / Mine Flail / Microfluidic / Rail Grinding)  
**Status:** PARTIALLY SEALED → HIGH gaps closed

## Broken chain before

```
Panel OnActionRequested declared
→ handlers OPEN-only
→ Start*/PerformMaintenance unused in production
→ PanelRegistry missing OPEN ids
→ Microfluidic OnRunCompleted → LastEvent only
→ RegisterMinefield/RegisterRailSegment test-only
→ GetHazardModifier/GetTravelModifier unused by expedition estimates
```

## Seal applied

| Gap | Fix |
|---|---|
| Panel actions | Four panels emit `start_*` / `maintain_*` with truthful params |
| Handlers | `Main.Plans146_149` calls host Start*/PerformMaintenance + inventory consume |
| Menu reachability | `PanelRegistry` ids: `ebpvd_coating`, `microfluidic_diagnostic`, `mine_clearing_flail`, `rail_grinding` |
| Diagnosis write | `OnRunCompleted` → `DiagnosisKnowledgeStore` Confirm / Suspect / RuleOut |
| Route bootstrap | Empty route section seeds north corridor minefield + iron-vein rail |
| Travel effect | Estimate applies hazard×travel; live encounter composer multiplies `GetHazardModifier` |
| Corridor UI truth | Mine/rail panels list `Routes.GetAllSegments()` instead of fiction rows |

## Verification

- `dotnet build Ashfall.csproj` → 0/0
- Focused xUnit: Plans146_149Integration (6), RouteInfrastructure (11), EbPvd (10), Microfluidic (11), MineFlail (7), RailGrinding (7) — all pass

## Remaining limitations

- Live `ExpeditionSystem.Start` still uses authored `distanceTicks` (travel modifier is estimate/preview + encounter risk only).
- Coated-part → PowerGrid efficiency still open (MED).
- Assay patient defaults to first living survivor; no patient picker yet.
- Host command path is not covered by xUnit (Godot handler); Core lifecycle/save coverage unchanged.
- Wave_17 docs that reuse plan IDs 146–149 are a name collision only — not this flagship set.

## Files touched

- `src/Main.Plans146_149.cs`
- `src/Main.EvolvingWorld.cs`
- `src/Main.PlayerSurfaces.cs`
- `src/Host/ExpeditionHostSession.cs`
- `src/Host/MineClearingFlailHostSession.cs`
- `src/Host/RailGrindingHostSession.cs`
- `src/UI/EbPvdCoatingPanel.cs`
- `src/UI/MicrofluidicDiagnosticPanel.cs`
- `src/UI/MineFlailPanel.cs`
- `src/UI/RailGrindingPanel.cs`
