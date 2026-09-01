# Combat Encounter Coverage & Composition Matrix

**Document:** `docs/combat/COMBAT_ENCOUNTER_COVERAGE.md`
**Catalog Authority:** `Assets/StreamingAssets/Data/combat_catalog.json`
**Runtime System:** [`Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs`](../../Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs)

---

## 1. Encounter Composition Pools

| Encounter Archetype | Primary Combatants | Tactical Signature | Player Decision & Strategic Counter |
|---|---|---|---|
| **Flank Pressure Fauna Group** | `combatant_burrower_mite`, `combatant_feral_mutt` | Fast multi-lane flanking attack; forces quick target switching. | Close-range volume fire (`weapon_smg`, `weapon_scrap_shotgun`) and lane re-centering. |
| **Center Armor Fauna Anchor** | `combatant_armored_boar`, `combatant_spore_hound` | Heavy armored charging beast behind spore cloud cover. | Armor-piercing loads (`ammo_762x54r`, `weapon_marksman_rifle`, `weapon_rebar_spear`). |
| **Subway Ruin Stalker Pack** | `combatant_pale_crawler`, `combatant_chrome_loper` | High-damage sprint ambush in dark confined corridors. | Suppressive fire and high-readiness sidearms (`weapon_sidearm`, `weapon_service_rifle`). |
| **Sector Checkpoint Conscript Levy** | `combatant_conscript_levy`, `combatant_desperate_scavenger` | Low-morale human checkpoint guards with high surrender potential. | Intimidation, bribery, food trade, or warning shots to trigger early surrender without blood. |
| **Warlord Veteran Choke Strike** | `combatant_warlord_veteran`, `combatant_conscript_levy` | Disciplined military entrenchment with suppressive fire and barricades. | Precision counter-sniping, tactical retreat, or formal tribute negotiation. |
| **Black Flotilla Coastal Picket** | `combatant_flotilla_marine` | Tight noise discipline and maritime rifle fire along coastal wharves. | Flotilla faction standing, barter tokens, or smoke/subsonic infiltration. |
