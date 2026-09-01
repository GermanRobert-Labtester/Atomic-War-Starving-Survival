# Location Memory & Revisitation Audit — Plan 11

> **Document Class:** Gazetteer & Location Memory Specification
> **Authority:** `Assets/Ashfall.Core/StandingRecord/LocationMemorySystem.cs`, `Assets/StreamingAssets/Data/standing_record_memory.json`
> **Host Wiring:** `src/World/MapLocationMarkerView.cs`, `src/UI/MapDetailPanel.cs`
> **Save Key:** `location_memory`

---

## 1. Executive Summary

`LocationMemorySystem` maintains the historical strata and diegetic recast descriptions of world locations across three temporal/narrative layers:
- **`pre`**: Initial pre-war / pre-collapse observation state.
- **`after`**: Lived, scavenged, or palimpsest state following major world mutations.
- **`now`**: Active, current state reflecting current owner, degradation, and hazard blooms.

---

## 2. Discovery vs. Visitation Invariant

1. **`Revealed` State:**
   - Generated when coordinates are decoded via radio ciphers, archive maps, or rumor transcripts.
   - Node appears on the map as an available destination, but the location history does not record a physical visit.
2. **`Visited` State:**
   - Triggered when an expedition physically arrives at the node.
   - Sets `lastVisitedDay`, captures initial scavenging state, and records first-breach entries.
3. **`Changed-Since-Last-Visit` State:**
   - Evaluated dynamically if `LocationEvolutionRecord.lastVisitedDay < LocationEvolutionRecord.lastMutatedDay` or if a world evolution event fired at that node.
   - Surfaces a distinct state indicator on `MapLocationMarkerView` and updates descriptive prose in `MapDetailPanel`.

---

## 3. Diegetic Revisitation Prose Examples

| Location | Prior State | Mutated State | Revisit Prose |
|---|---|---|---|
| `loc_excavation_command_vault` | Sealed military blast door | Breached & depleted | "The blast hatch hangs askew on torn hinges. Inside, water drips onto emptied electronics racks." |
| `loc_cut_abandoned_depot` | Quiet roadside ruin | Faction Checkpoint | "The depot has been fortified with sandbags and barbed wire. Armed sentries watch the highway from the roof." |
| `loc_excavation_metro_interchange` | Dusty dry concourse | Spore Bloom | "Thick bioluminescent mold mats cover the platform edge. The air tastes bitter and burns the throat." |
| `loc_water_station` | Intact cistern | Collapsed landmark | "The concrete reservoir tower came down in the storm. Grey water pools in the crater." |

---

## 4. Save & Restoration Guarantees
- Memory strata and active recast flags persist in `LocationMemoryState`.
- Loading a save preserves exact recast histories without resetting visited locations to pristine text.
