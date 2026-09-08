# Faction War Rebel Branch Handoff

> **Integration Anchor:** Plan 121 (Independent & Rebel Faction Branch Expansion, 8 → 15 branches)
> **Authority:** `Assets/StreamingAssets/Data/independent_faction_branch.json`

---

## 1. Interaction Between Independent Branches and Temporal Overrides

Plan 121 established 15 distinct Independent survivor branches, representing unaffiliated paths ranging from communal mutual aid to radical sabotage and self-governing enclaves.
Plan 124's temporal location overrides provide the physical, environmental backdrop against which these rebel and independent factions operate.

### 1.1 Non-Intrusive Presentation Layer
- **Decoupled Mechanics:** Overrides in `faction_war_location_overrides.json` are evaluated purely from `(locationId, day)`. They do not forcibly trigger or mutate `FlagLedger` flags, nor do they mutate `branchId` assignments.
- **Narrative Resonance:** When an independent branch directs the player toward a location (e.g. `loc_settlement_iron_siding`, `loc_bridge_seven`, or `loc_ash_militia_deadfall_barrier`), the temporal state of the location accurately mirrors the wider conflict.

---

## 2. Concrete Branch Intersections

### 2.1 The Ash Militia Deadfall (`loc_ash_militia_deadfall_barrier`)
- **Override:** `loc_override_roadblock_liberated` (Days 330–370)
- **Rebel Context:** As independent guerilla cells push state garrison outposts out of Sector 4, the clearing of the deadfall barrier provides diegetic evidence of militia retreat and civilian self-determination.

### 2.2 Iron Siding Abandonment (`loc_settlement_iron_siding`)
- **Override:** `loc_override_village_abandoned` (Days 280–340)
- **Rebel Context:** Shows the human cost of resource starvation on frontier worker communities who refuse allegiance to major militarized factions, reinforcing the urgency of independent mutual aid networks.

### 2.3 Bridge Seven Demolition (`loc_bridge_seven`)
- **Override:** `loc_override_bridge_destroyed` (Days 310–360)
- **Rebel Context:** The tactical demolition of Bridge Seven prevents heavy military armor from crossing into independent safe valleys, illustrating asymmetric scorched-earth strategies.
