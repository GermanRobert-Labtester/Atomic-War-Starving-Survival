# Verdict Save Contract & Migration Policy

> **Authority:** `Assets/Ashfall.Core/Verdict/VerdictSave.cs`
> **Campaign Integration:** Section `verdict` in `campaign.json` envelope.

---

## 1. Persisted State Architecture

The Verdict save section persists player investigation progress using the following canonical structure:
- `visited_locations`: `HashSet<string>` tracking discovered and explored location IDs.
- `enrolled_evidence`: List of enrolled evidence fragment IDs (`ev_*`).
- `resolved_witnesses`: List of witness encounter states (`npc_*`).
- `unlocked_radio_broadcasts`: List of triggered radio events.

---

## 2. Backward & Forward Compatibility

1. **Old Saves (4-Site Era):**
   - When loading a save generated before Plan 82, existing entries in `visited_locations` (e.g. `loc_geophone_pit_1`) load without error.
   - The 11 newly authored locations default to unvisited (`false`) and undiscovered.
   - Campaign progress along Arc 1 is completely preserved.
2. **New Saves (15-Site Era):**
   - Any newly visited location ID is added to `visited_locations`.
   - The list serializes into the standard JSON string array.
   - Round-trip save and reload preserves the exact visited set without duplication or dropped entries.
3. **No Save Schema Alterations:**
   - Plan 82 is pure data. Zero modifications were made to `VerdictSave.cs` or the campaign envelope builder.
