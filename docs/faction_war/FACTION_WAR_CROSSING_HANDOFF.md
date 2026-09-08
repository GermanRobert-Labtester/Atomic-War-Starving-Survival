# Faction War Crossing Expansion Handoff

> **Integration Anchor:** Plan 115 (The Crossing Expansion)
> **Source Catalog:** `Assets/StreamingAssets/Data/crossing_locations.json`
> **Target Overrides:** Indices 10, 17 in `faction_war_location_overrides.json`

---

## 1. Architectural Alignment

Plan 115 introduced The Crossing: a contentious border zone where refugees, grain syndicates, and border patrols negotiate passage, safe-conduct vouches, and emergency grain distribution.
Plan 124 deepens the Crossing's vulnerability by introducing 2 temporal overrides showing the physical toll of border escalation and refugee displacement during the mid-campaign faction war.

---

## 2. Integrated Crossing Locations

### 2.1 `loc_crossing_granary_pledge` → `loc_override_granary_burned`
- **Active Window:** Days 220–260 (41 days)
- **Override Type:** `post_strike`
- **Display Name Override:** `Crossing Granary Pledge Post (Smoldering Ruins)`
- **Atmosphere Override:** `The nauseating sweetness of scorched grain, hot tin, and wet ash.`
- **Narrative & Gameplay Effect:**
  A punitive raid against the grain syndicate leaves the pledge post and drying sheds in ruins. Charred rye spills through ruptured iron walls, halting formal pledge collections and forcing refugees to scrape ruined grain from mud puddles.

### 2.2 `loc_crossing_petition_tent` → `loc_override_camp_overrun`
- **Active Window:** Days 340–380 (41 days)
- **Override Type:** `abandoned`
- **Display Name Override:** `Crossing Petition Tent (Trampled Grounds)`
- **Atmosphere Override:** `Torn canvas flapping in bitter wind, frozen mud, and cold hearths.`
- **Narrative & Gameplay Effect:**
  As winter arrives and food distribution breaks down, the orderly petition queues disintegrate into panic. The central pavilion is stripped of its canvas and timbers, leaving frozen ruts, discarded petition tokens, and an eerie winter silence.

---

## 3. Preservation of Crossing Systems

- The underlying crossing mechanisms (petition vouchers, safe conduct tokens, treaty concession tracking) remain untouched.
- Presentation layers check `GetActiveLocationOverride` to update location displays and narrative context without breaking underlying trading or quest state machines.
