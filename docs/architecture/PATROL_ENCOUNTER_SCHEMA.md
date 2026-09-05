# ASHFALL Patrol Encounter Schema & Transaction Specification
## Technical Reference: Core Narrative, Inventory & Faction Authority

### Overview
This document specifies the data contracts, JSON schema standards, evaluation lifecycles, and transaction semantics for patrol and travel encounters in ASHFALL.

---

### 1. JSON Data Contract (`travel_encounters.json`)

Each encounter definition adheres to the following JSON structure:

```json
{
  "id": "enc_patrol_garrison_checkpoint",
  "title": "Iron Garrison Checkpoint",
  "category": "Human",
  "faction_id": "iron_garrison",
  "territory_state": "Contested",
  "region_tags": ["high_scarp", "the_toll"],
  "min_danger_level": 0.5,
  "max_danger_level": 2.0,
  "base_weight": 1.2,
  "stance_weights": {
    "Cautious": 0.8,
    "Aggressive": 1.5,
    "Diplomatic": 1.4
  },
  "season_tags": ["all"],
  "description": "A fortified roadblock formed from rusted ISO containers...",
  "choices": [
    {
      "choice_id": "choice_pay_garrison_toll",
      "text": "Pay the toll in rations (2 Canned Food)",
      "is_nonviolent": true,
      "is_avoidance": false,
      "morale_delta": 0,
      "guilt_delta": 0,
      "unlocks_field_guide_id": "",
      "advances_chain_stage": 0,
      "faction_id": "iron_garrison",
      "faction_standing_delta": 1,
      "cost_items": ["canned_food", "canned_food"],
      "required_item_id": "",
      "required_item_quantity": 0,
      "required_flag": ""
    },
    {
      "choice_id": "choice_show_garrison_pass",
      "text": "Present sealed government transit pass",
      "is_nonviolent": true,
      "is_avoidance": false,
      "morale_delta": 1,
      "guilt_delta": 0,
      "unlocks_field_guide_id": "",
      "advances_chain_stage": 0,
      "faction_id": "iron_garrison",
      "faction_standing_delta": 2,
      "cost_items": [],
      "required_item_id": "sealed_government_document",
      "required_item_quantity": 1,
      "required_flag": ""
    }
  ]
}
```

---

### 2. Domain Models & Contracts (C#)

#### A. Normalized Item Costs
Cost item lists (`cost_items: ["canned_food", "canned_food"]`) are parsed and aggregated into immutable `ItemCost` pairs:
```csharp
public readonly record struct ItemCost(
    string ItemId,
    int Quantity);
```

#### B. Requirement Failure Categories & Accessibility Diagnostics
Failure diagnostics provide machine-typed categories and user-facing accessibility text:
```csharp
public enum ChoiceRequirementFailureType
{
    MissingRequiredItem,
    MissingCostItem,
    CooldownActive,
    OtherExistingRequirement
}

public sealed record ChoiceRequirementFailure(
    ChoiceRequirementFailureType FailureType,
    string ItemId,
    int RequiredQuantity,
    int AvailableQuantity,
    string Reason);
```

#### C. Choice Availability Evaluation
Availability is non-mutating and dynamically re-evaluated at UI render or choice selection time:
```csharp
public sealed record TravelEncounterChoiceAvailability
{
    public bool IsAvailable { get; init; }
    public IReadOnlyList<ChoiceRequirementFailure> Failures { get; init; }
}
```

#### D. Full Resolution Result
Upon successful commitment, `TravelEncounterResolution` encapsulates the complete atomic outcome:
```csharp
public sealed record TravelEncounterResolution
{
    public string EncounterId { get; init; }
    public string ChoiceId { get; init; }
    public int MoraleDelta { get; init; }
    public int GuiltDelta { get; init; }
    public string? UnlockedFieldGuideId { get; init; }
    public string? FactionId { get; init; }
    public int FactionStandingDelta { get; init; }
    public IReadOnlyList<ItemCost> CostItems { get; init; }
    public int ResolvedDay { get; init; }
    public int CooldownDays { get; init; } // Default: 5
}
```

---

### 3. Execution & Transaction Lifecycle

1. **Pre-Flight Validation**:
   - `EvaluateChoiceAvailability` checks cooldown, required items (`CountById >= RequiredItemQuantity`), and cost items (`CountById >= CostQuantity`).
   - If any condition fails, execution immediately halts with `false` and zero mutations occur.
2. **Atomic Cost Deduction**:
   - Costs are bundled into an `InventoryBill`.
   - `inv.TryExecuteTransaction(bill)` deducts all items in a single atomic operation. If any item is missing or inventory locks, rollback is automatic.
3. **Faction Standing Mutation**:
   - `FactionWarSystem.ModifyStanding(choice.FactionId, choice.FactionStandingDelta)` mutates the authoritative faction ledger.
   - Standing is clamped within `[-100, +100]`.
4. **State & Cooldown Updates**:
   - Encounter cooldown is marked: `_encounterAvailableDay[encounterId] = currentDay + 5`.
   - Chain stage advanced if configured.
5. **Event Emission**:
   - Emits `OnChoiceResolved(encounterId, choiceId)` for backward compatibility.
   - Emits `OnTravelChoiceResolved(resolution)` with rich payload for host telemetry and UI presentation.

---

### 4. Expedition Bridge Integration (Option B)

- `ExpeditionEncounterBridge` binds both `NarrativeEncounterSystem` and `TravelEncounterSystem`.
- `Surface(ExpeditionState)` evaluates narrative and patrol candidates simultaneously.
- When surfacing patrol encounters, `TravelEncounterDefinition` choices are projected onto `EncounterChoiceDefinition`, preserving `costItems`, `requiredItemId`, `requiredItemQuantity`, `factionId`, and `factionStandingDelta`.
- `ExpeditionEncounterBridge.ResolveChoice` intercepts patrol encounters and routes resolution through `TravelEngine.ResolveChoice`, guaranteeing that expeditions and normal travel share the same inventory authority, faction standing consequences, and 5-day cooldown clock.
