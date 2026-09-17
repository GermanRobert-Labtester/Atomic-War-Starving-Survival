# DECISION MEMO: `water_sample_contaminated` Equipability Quirk

**Document ID:** `MEMO-2026-09-17-WATER-SAMPLE-EQUIPABILITY`
**Author:** AGY (Antigravity Agent)
**Target:** AI Foreman / Systems Integrator
**Status:** AWAITING FOREMAN SIGNATURE (Preserving Option B pending review)

---

## 1. Current Item Row
In `Assets/StreamingAssets/Data/items.json`:
```json
{
  "id": "water_sample_contaminated",
  "displayName": "Contaminated Water Sample",
  "description": "A labelled water sample in a sealed glass vial, drawn from a known contaminated source. It is evidence, not drinking water, and the difference matters. Worth six, a tenth of a kilo, five to a stack. Scientists, medics, and the cautious all recognise the same colour: the colour of a warning nobody heeded.",
  "type": "Container",
  "stackMax": 5,
  "weight": 0.1,
  "radProtection": 20,
  "durability": 60,
  "degradeRate": 0.5,
  "contamination": 5,
  "hungerRestore": 0,
  "thirstRestore": 0,
  "healthEffect": 0,
  "radCleanse": 0,
  "moraleEffect": 0,
  "isEquipable": true,
  "equipSlot": "Body",
  "tradeValue": 6,
  "empShielded": false
}
```

## 2. Current Category and Equipability
- **Item Type:** `Container`
- **Equipable:** `true`
- **Equip Slot:** `Body`
- **Radiation Protection:** `20.0`
- **Durability:** `60.0`
- **Degrade Rate:** `0.5`

## 3. All Current Consumers
1. `Assets/StreamingAssets/Data/dynamic_questlines.json`: Objective delivery/collection item for investigation quests.
2. `artifacts/asset_registry.json`, `assets/art/water_sample_contaminated.jpg`: Visual icon asset.
3. `Inventory.DegradeEquippedGear`: Degrades if worn in the `Body` slot during radiation exposure.

## 4. Why It Is Flagged
The item is conceptually a small glass vial containing a sample of toxic water. However, it is flagged with `isEquipable: true` in the `Body` slot and grants 20 points of radiation protection, effectively behaving like protective radiation armor if a survivor equips it. This is widely regarded as a legacy data authoring quirk from an early prototype where sample vials may have been tested as detector badges or protective amulets.

## 5. Decision Options

### Option A: Correct Classification (Removes Quirk)
- Set `"isEquipable": false`, `"equipSlot": "None"`, `"radProtection": 0`, `"durability": 0`, `"degradeRate": 0`.
- **Pros:** Logical physical consistency; a water vial cannot be worn on the torso as rad shielding.
- **Cons:** Any existing save file where a survivor has `water_sample_contaminated` in their `EquippedGear` body slot would need migration handling on load (unequipping to inventory or dropping).

### Option B: Intentionally Retain (Diegetic Lore Reinterpretation)
- Retain current fields as intentional. Reinterpret diegetically as a specialized "Radiation Dosimeter / Chemical Indicator Flask" worn around the chest strap that provides early ambient warning and minor shielding through leaded vial glass.
- **Pros:** Zero risk of save deserialization regression or unequip failure; requires zero test changes.
- **Cons:** Minor category anomaly persists in the item catalog.

## 6. Save and Content Impact
- **Save Impact under Option A:** Slight risk if legacy saves contain this item in the `EquippedGear[Body]` slot.
- **Save Impact under Option B:** Exactly zero save impact; perfectly backwards-compatible.

## 7. Tests Impacted
- `Ashfall.Core.Tests/Inventory/Plan21ProtectiveWearTests.cs` (currently audits all equipable rad-protective gear including this item).
- Changing to Option A would require adjusting the count of equipable protective body items from 6 to 5.

## 8. Recommendation & Status
**Recommendation:** Retain Option B (or sign Option A in a dedicated save-migration wave). Per Wave 9 Master Plan instructions (§5.15), Option B is intentionally retained and code is untouched pending foreman signature.
