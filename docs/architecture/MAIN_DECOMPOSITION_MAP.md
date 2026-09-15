# Main decomposition map

Status: wave-1 evidence, 2026-09-10.

`Main` is already a partial composition root in the Godot host. The current
root is 82 lines, with 103 `Main.<Domain>.cs` partials (104 files including
the root). The domain split is therefore recorded and gated here rather than
introducing another cosmetic move wave.

## Mechanical inventory

The source-of-truth persistence inventory is
`Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`. It currently contains 173
registered campaign sections. The source scan observes 1,355 Setup tokens,
969 Save tokens, and 78 Flush tokens across the partial set; the xUnit gate
counts unique method declarations and verifies the registry methods directly.

`MainTriadDriftGateTests` enforces:

- every registry `SaveMethod` and required `SetupMethod` exists in a `Main*.cs`
  partial;
- every registered save method is reachable from `SaveAll`, including the
  composite `SaveAllExpandedShelterSystems` and `PersistPlans*` helpers;
- expression-bodied triad methods are included in the scan;
- flush methods have a dirty/save guard or an explicit transient/composite
  disposition;
- the lifecycle/orchestration files and architecture citations remain present.

## Representative ownership matrix

| Domain | Partial owner | Setup | Save | Flush | Registry sections | Shared dependencies |
|---|---|---|---|---|---|---|
| Application/lifecycle | `Main.Application.cs`, `Main.Lifecycle.cs` | composition/bootstrap | canonical `SaveAll` | `FlushDirtyStoresForDayAdvance` | all sections through orchestration | Godot tree, clock, save hub |
| Expeditions | `Main.Expeditions.cs` | `SetupExpeditions` | `SaveExpeditions` | `FlushExpeditionIfDirty` | `expedition`, `recon_telemetry`, `railway`, cargo/vehicle sections | campaign day, inventory, weather |
| Economy | `Main.Economy.cs` | `SetupEconomy` | `SaveEconomy` | `FlushEconomyIfDirty` | `economy`, caravan and barter sections | inventory, campaign day |
| Medical | `Main.Medical.cs`, `Main.MedicalTriage.cs` | `SetupMedical` | `SaveMedical`, `SaveMedicalPipeline` | `FlushMedicalIfDirty` | `medical`, `medical_pipeline`, ward/disease sections | survivors, inventory, journal |
| World/evolution | `Main.World.cs`, `Main.EvolvingWorld.cs` | `SetupWorld` | `SaveWorld` | `FlushWorldIfDirty` | `world`, infestation, field-guide sections | campaign day, weather, radio |
| Narrative/radio | `Main.Narrative.cs`, `Main.Quests.cs`, `Main.NarrativeQuestlines.cs` | `SetupNarrative`, `SetupRadio` | `SaveNarrative`, `SaveRadio` | `FlushNarrativeIfDirty`, `FlushEventAdapterIfDirty`, `FlushJournalIfDirty` | narrative, radio, journal, event sections | event bus, journal, flags |
| Survivors/gear | `Main.Survivors.cs`, `Main.Inventory.cs`, `Main.SurvivorFate.cs` | survivor/inventory setup | survivor/inventory/fate saves | domain-specific dirty flushes | `survivors`, `inventory`, `dose_ledger`, `survivor_fate` | needs, radiation, save hub |
| Expanded shelter | `Main.ExpandedShelterSystems.cs` and Plan partials | composite setup | composite save delegates | documented child flushes | registry lifecycle group `expanded_shelter` | save hub, shared campaign day |

The complete matrix is intentionally registry-derived: adding a new persisted
section changes the authority file and causes the default test gate to report
the missing host method or orchestration edge. This avoids a second manually
maintained list drifting away from the campaign envelope.

## Ordering and ownership rules

`Main.Lifecycle.cs` remains the lifecycle authority. The decomposition does
not move construction order, rename public methods, duplicate shared roots, or
create another lazy singleton site. `Main.cs` retains composition fields and
shared services; domain behavior belongs in the owning partial or Core.

The root's 82 lines are materially below the historical ~6.5k-line god-object
baseline. Remaining work is a wave-2 ownership cleanup for the many small Plan
partials, not a reason to combine behavior back into `Main.cs`.

## Wave-2 backlog

1. Keep new Plan partials grouped by domain instead of by feature batch where
   that can be done as a move-only change.
2. Move any remaining panel-only closures only when their session ownership is
   unambiguous.
3. Keep `MainTriadDriftGateTests` green after each persisted-section addition.
4. Preserve `Main.Lifecycle.cs` as the only place that can reorder startup,
   reset, save, and shutdown participants.
