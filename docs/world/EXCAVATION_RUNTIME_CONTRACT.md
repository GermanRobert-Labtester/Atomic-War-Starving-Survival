# Excavation Runtime Contract — Plan 11

> **Document Class:** Architecture & Runtime Specification
> **Authority:** Core (`Assets/Ashfall.Core/ExcavationSystem.cs`)
> **Host Wiring:** `src/Host/ExcavationHostSession.cs`, `src/UI/ExcavationPanel.cs`
> **Save Key:** `excavation` (via `ExcavationSaveStore.cs` / `CampaignEnvelopeBuilder`)

---

## 1. Executive Summary

ASHFALL's subterranean exploration tier operates through `ExcavationSystem` in `Ashfall.Core`. The excavation system models structural excavation through progressive clearing, worker labor allocation, structural risk assessment, shoring reinforcement, cave-in risk evaluation, and room/cache breaches.

This document establishes the runtime contract for deep-strata excavation expeditions, defining how authored excavation sites interact with depth bands, material costs, labor shifts, environmental hazards, deterministic cave-in rolls, and save persistence.

---

## 2. Excavation System Runtime Contract

### 2.1 State Structure
```csharp
[Serializable]
public sealed class ExcavationState
{
    public string systemId = ExcavationSystem.SystemId;
    public List<ExcavationSite> sites = new List<ExcavationSite>();
}

[Serializable]
public sealed class ExcavationSite
{
    public string siteId = string.Empty;
    public string roomBlueprintId = string.Empty;
    public float progress;
    public float requiredProgress = 100f;
    public int assignedWorkerCount;
    public float structuralRisk; // 0.0 to 1.0
    public bool hasCavedIn;
    public bool isComplete;
    public List<string> requiredTools = new List<string>();
    public bool shoringApplied;
    public List<string> discoveredCaches = new List<string>();

    // Plan 11 Strata Extensions (backward-compatible)
    public float currentDepthMeters;
    public float maxDepthMeters = 100f;
    public int activeDepthBandIndex;
    public string activeHazard = string.Empty;
    public string relicRewardId = string.Empty;
    public bool isDepleted;
}
```

### 2.2 Depth Progression & Daily Tick Mechanics
1. **Daily Progress Formula:**
   $$\Delta \text{Progress} = \text{Workers} \times 5.0 \times (\text{ShoringApplied} ? 1.2 : 1.0)$$
2. **Cave-In Probability:**
   $$P(\text{CaveIn}) = \text{StructuralRisk} \times 0.10$$
   - Rolled deterministically using `ISeededRng` per site on each daily tick.
   - Upon cave-in: `hasCavedIn = true`, progress penalized by 20 points ($\min 0$), and warning logged.
3. **Shoring Effect:**
   - Shoring halves the structural risk ($\text{Risk} \leftarrow \text{Risk} \times 0.5$) and boosts clearing speed by +20%.
4. **Completion & Breach Milestones:**
   - When $\text{Progress} \ge \text{RequiredProgress}$, `isComplete = true`, unlocking the associated room blueprint, relic reward, and first-breach journal entry.

---

## 3. Authored Deep-Strata Sites Profile

| Site ID | Location ID | Name | Max Depth | Strata Bands | Shoring Materials | Primary Hazard | Authored Reward |
|---|---|---|---|---|---|---|---|
| `excavation_command_vault` | `loc_excavation_command_vault` | Collapsed Command Vault | 90m | 3 (20m, 50m, 90m) | `item_scrap_metal`, `item_mechanical_parts` | Toxic Stale Air / Collapse | `item_comm_codebook_alpha`, `item_relic_military_core` |
| `excavation_utility_tunnels` | `loc_excavation_utility_tunnels` | Utility Tunnel Network | 70m | 3 (15m, 40m, 70m) | `item_scrap_wood`, `item_metal_sheet` | `hazard_spore_mold`, Flooding | `item_tools_precision`, `item_wire_copper`, `item_filter_charcoal` |
| `excavation_metro_interchange` | `loc_excavation_metro_interchange` | Buried Metro Interchange | 110m | 3 (25m, 60m, 110m) | `item_steel_columns`, `item_hydraulic_jack` | Structural Collapse, Spore Bloom | `item_civilian_relic_transit_chronometer`, `item_logistics_cipher_sheet` |
| `excavation_mine_shaft` | `loc_excavation_mine_shaft` | Mine Shaft Adit 4 | 140m | 3 (30m, 75m, 140m) | `item_timber_beams`, `item_hydraulic_jack` | Methane Pocket / High Cave-In | `item_heavy_industrial_motor`, `item_scrap_titanium` |
| `excavation_archive_bunker` | `loc_excavation_archive_bunker` | Pre-War Archive Bunker | 80m | 3 (10m, 35m, 80m) | `item_reinforced_arches`, `item_scrap_metal` | Asphyxiation / Sealed Air | `item_archive_index_cylinder`, `item_schematic_filter_rebuild` |

---

## 4. Determinism & Save Contract
- All rolls use `ISeededRng` (xorshift64* / `SeededRng`).
- State is captured and restored through `ExcavationSaveStore` into the atomic `campaign.json` envelope.
- Save versioning defaults missing fields safely without throwing or mutating pre-existing saves.
