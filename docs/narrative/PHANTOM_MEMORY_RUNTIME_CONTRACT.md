# Phantom Memory Runtime Contract & Architecture

> **Author:** ASHFALL Core Architecture Team
> **System:** `Ashfall.Core.PhantomMemoryEngine` & `Ashfall.Core.Phantoms`
> **Authority:** Engine-agnostic C# (`Assets/Ashfall.Core/`)

---

## 1. Overview & System Purpose

The **Phantom Memory Engine** (`PhantomMemoryEngine.cs`) models how ordinary and historical objects carry psychological echoes of pre-war life and the Exchange. When survivors scavenge or inspect objects, the engine evaluates survivor background, traits, and object categories to trigger motivation bursts or psychological breakdowns.

Under **Plan 21**, the phantom layer expands from a simple random trigger check into an integrated world layer spanning:
1. **Phantom Triggers** (`phantom_triggers.json`) — Authorial memory fragments attached to item classes and survivor backgrounds.
2. **Heirlooms** (`phantom_heirlooms.json` & `HeirloomSystem`) — Persistent named objects with generational provenance, holder-specific memories, and inheritance chains.
3. **Confession Secrets** (`confession_secrets.json` & `ConfessionSecretSystem`) — Discoverable truths creating moral leverage.

---

## 2. Runtime Contract: `PhantomMemoryEngine`

### 2.1 Inputs
- **`PhantomSurvivorSnapshot`**:
  - `survivorId`: string (unique identifier)
  - `displayName`: string (survivor name for text interpolation `{name}`)
  - `backgroundId`: string (e.g. `former_soldier`, `nurse`, `teacher`, `farmer`, `engineer`, `generic`)
  - `isAlive`: bool (dead survivors never trigger phantom memories)
- **`itemId`**: string (item identifier, e.g. `item_dog_tags`, `wedding_ring`, `foreman_whistle`)
- **`ISeededRng`**: deterministic random number generator (xorshift64* implementation)

### 2.2 Trigger Chance & Roll Math
- Base trigger chance: `BaseTriggerChance = 0.15` (15%)
- Scaled chance: `effectiveChance = BaseTriggerChance * (1 + triggersExperienced * 0.10)`
- Outcome determination:
  - If `roll < motivationChance * effectiveChance` $\rightarrow$ `TriggerOutcome.Motivation`
  - Else if `roll < effectiveChance` $\rightarrow$ `TriggerOutcome.Breakdown`
  - Else $\rightarrow$ `TriggerOutcome.None`

### 2.3 Psychological & Work Consequences
- **Motivation Outcome**:
  - `MotivationMoraleBoost`: +15 morale (emitted / routed to `NeedsSystem`)
  - `MotivationWorkSpeedBonus`: +20% work efficiency multiplier (`1.20x`)
  - `MotivationBoostDurationHours`: 8.0 in-game hours
  - Breakdown timers cleared to 0
- **Breakdown Outcome**:
  - `BreakdownMoraleDrop`: -20 morale (routed to `NeedsSystem` / `GuiltInsomniaSystem`)
  - `BreakdownWorkRefusalHours`: 4.0 in-game hours of work refusal
  - Motivation boost cleared to 0

### 2.4 Events & Lifecycle
- `OnPhantomTriggered(survivorId, itemId, isMotivation)`
- `OnPhantomBreakdown(survivorId, itemId)`
- `OnStateChanged(PhantomMemoryEngineState)`

### 2.5 State & Persistence
- `PhantomMemoryRecord`:
  - `survivorId`: string
  - `triggersExperienced`: int
  - `motivationBoostHoursRemaining`: float
  - `breakdownRefusalHoursRemaining`: float
  - `triggeredItemIds`: List<string> (tracks experienced item triggers for idempotence and memory log)
- Deterministic serialization: `CaptureState()` sorts records ordinally by `survivorId` to guarantee identical checksums regardless of dictionary iteration order across platforms.

---

## 3. Catalog DTOs (`Ashfall.Core.Phantoms`)

### 3.1 `PhantomTriggerCatalogJson`
Root schema:
```json
{
  "schema_version": 1,
  "items": [
    {
      "background_id": "former_soldier",
      "triggers": [
        {
          "item_category": "military",
          "motivation_chance": 0.2,
          "description": "Dog tags, half-melted, still readable...",
          "motivation_text": "{name} pockets the tags. 'I'll remember them,' they say...",
          "breakdown_text": "{name} reads the name on the tag and goes pale..."
        }
      ]
    }
  ]
}
```

### 3.2 Category Inference & Taxonomy
Category inference maps item IDs to semantic categories:
- `childhood`: `toy_*`, `child_*`, `mitten`, `chalk`, `doll`, `drawing`
- `photograph`: `photo_*`, `album`, `portrait`, `daguerreotype`
- `correspondence`: `letter_*`, `mail_*`, `diary_*`, `notebook`, `ledger`, `chart`
- `personal_item`: `ring_*`, `watch_*`, `heirloom_*`, `lighter`, `mug`, `comb`, `key_*`
- `military`: `dog_tag*`, `military_*`, `medal_*`, `insignia`, `canteen`
- `medical`: `medical_*`, `bandage_*`, `pill_*`, `stethoscope`, `scalpel`, `suture`
- `work_tool`: `caliper`, `micrometer`, `wrench`, `whistle`, `punch`, `rule`, `tester`
- `ordinary_object`: `ticket`, `receipt`, `matchbook`, `keyring`, `comb`, `mug`
- `generic`: fallback for any unclassified object

---

## 4. Integration Boundaries

| Subsystem | Authority | Integration Seam |
|---|---|---|
| Morale & Needs | `NeedsSystem` | Motivation (+15) and Breakdown (-20) deltas |
| Guilt & Insomnia | `GuiltInsomniaSystem` | Guilt source recorded on traumatic/abandonment breakdown |
| Social Relations | `SurvivorRelationsSystem` | Memory sharing, bond reflection, and grief dispersion |
| Succession & Lineage | `GenerationalLineageExtension` | Heirloom transfer on survivor death |
| Memorial Wall | `MemorialSystem` | Keepsakes and heirlooms displayed in memorial space |
| Save Stores | `PhantomMemorySaveStore` & `SaveStoreHub` | Versioned, checksummed atomic persistence |
