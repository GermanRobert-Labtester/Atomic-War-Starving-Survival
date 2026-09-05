# Foundry Treaty Dialogue & Narrative Handoff Contract

**Target Files:** `Assets/StreamingAssets/Data/dialogue/`, `Assets/StreamingAssets/Data/narrative/`
**Host Hook:** `src/UI/DialoguePanel.cs` / `src/UI/FactionsPanel.cs`

---

## 1. Dialogue Bark & Node Conditions

Treaty consequence states provide conditions for branching dialogue trees with faction representatives and survivors:

1. **State Flags Exposed:**
   - `treaty.<treaty_id>.met` (boolean)
   - `treaty.<treaty_id>.missed` (boolean)
   - `treaty.<treaty_id>.violated` (boolean)
   - `treaty.foundry.standing` (float)

2. **Dialogue Tone Anchors:**
   - When `met`: Faction envoys express professional, institutional respect ("The shipment arrived dry and stamped; your name is cleared in the weigh-house.").
   - When `missed`: Faction envoys express weariness and commercial pressure ("We're holding your berth for three days, no more. The Cutters don't run empty barges for charity.").
   - When `violated`: Faction envoys deliver grim ultimatums or refuse audience ("Step back from the balance. After what happened at Checkpoint Gamma, you're lucky you walked up the road unescorted.").

---

## 2. Diegetic Radio & Journal Reflections

Each applied consequence generates a diegetic journal or radio line:
- `jrnl_treaty_flotilla_met`: "Lock Gate Four cycled on high tide this morning. Three barges cleared through without delay."
- `jrnl_treaty_flotilla_missed`: "Lock Gate Four stayed shut. Flotilla cutters are idling in the channel, burning precious diesel."
- `jrnl_treaty_aquifer_violated`: "Black bilge in the marsh intake. Rebuilder sentries at Pump Station Nine have turned away all coastal traffic."
- `jrnl_treaty_garrison_violated`: "Eastern Road closed. Central Garrison has thrown up wire across Checkpoint Gamma."
