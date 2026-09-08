# Faction War Existing 9-Override Audit

> **Catalog Authority:** `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
> **Audit Focus:** Verification and preservation of the original 9 baseline overrides (indices 0..8).

---

## 1. Baseline Inventory

Prior to Plan 124, `faction_war_location_overrides.json` defined 9 initial location overrides. This document details each baseline entry and verifies that it remains byte-for-byte identical in the expanded catalog.

### 1.1 Detailed Record Matrix

#### 0. `loc_override_almshouse_pre_strike`
- **Location ID:** `loc_almshouse`
- **Override Type:** `pre_strike`
- **Active Window:** Days 1–100
- **Display Name:** `St. Jude's Almshouse (Pre-Strike)`
- **Atmosphere:** `The smell of boiled turnips and damp masonry.`
- **Summary:** Depicts St. Jude's Almshouse during the initial survival period, providing shelter and meager soup to starving refugees before artillery strikes devastate the parish.

#### 1. `loc_override_almshouse_post_strike`
- **Location ID:** `loc_almshouse`
- **Override Type:** `post_strike`
- **Active Window:** Days 101–300
- **Display Name:** `St. Jude's Almshouse (Shelling Ruin)`
- **Atmosphere:** `Acrid cordite, pulverized lime, and damp cold.`
- **Summary:** The almshouse after an artillery barrage, showing collapsed masonry, shattered stained glass, and scavengers picking through the rubble.

#### 2. `loc_override_plaza_cleared`
- **Location ID:** `loc_ration_queue_plaza`
- **Override Type:** `pre_strike`
- **Active Window:** Days 1–50
- **Display Name:** `Ration Distribution Plaza (Clear)`
- **Atmosphere:** `Cold morning wind carrying the murmur of an orderly crowd.`
- **Summary:** The civic plaza early in the post-exchange period, when ration queues still functioned with civil guard presence and organized tickets.

#### 3. `loc_override_silo_fortified`
- **Location ID:** `loc_grain_silo`
- **Override Type:** `post_strike`
- **Active Window:** Days 200–350
- **Display Name:** `Grain Silo (Hardened Outpost)`
- **Atmosphere:** `Welding stink, diesel exhaust, and the creak of steel plates.`
- **Summary:** The leaning concrete silo fortified by faction militiamen with sandbags, corrugated steel skirts, and elevated sentry platforms.

#### 4. `loc_override_conscription_burned`
- **Location ID:** `loc_conscription_office`
- **Override Type:** `post_strike`
- **Active Window:** Days 220–340
- **Display Name:** `Conscription Office (Gutted Shell)`
- **Atmosphere:** `Wet charcoal, charred ledger paper, and iron-rich rain.`
- **Summary:** The municipal conscription bureau burned out following anti-draft riots or faction shelling; blackened desks and sodden records litter the street.

#### 5. `loc_override_cache_looted`
- **Location ID:** `loc_d9_cache_bunker_delta`
- **Override Type:** `post_strike`
- **Active Window:** Days 300–450
- **Display Name:** `D/9 Cache Delta (Breached Vault)`
- **Atmosphere:** `Stale sump air, hydraulic fluid, and torch-cut slag.`
- **Summary:** A pre-war subterranean bunker whose heavy blast doors have been breached by hydraulic jacks and cutting torches, leaving empty racking and breached crates.

#### 6. `loc_override_weighbridge_barricaded`
- **Location ID:** `loc_weighbridge`
- **Override Type:** `post_strike`
- **Active Window:** Days 250–380
- **Display Name:** `North Weighbridge (Double Barricade)`
- **Atmosphere:** `Creosote timbers, diesel fumes, and the clinking of chain.`
- **Summary:** The freight weigh station turned into a heavily fortified toll point with double layers of timber cribbing, razor wire, and defensive revetments.

#### 7. `loc_override_waystation_refugee`
- **Location ID:** `loc_shrine_switchback_waystation`
- **Override Type:** `ambient_addendum`
- **Active Window:** Days 150–280
- **Display Name:** `Switchback Waystation (Overcrowded)`
- **Atmosphere:** `Wood smoke, wet wool, and low voices trading rumors.`
- **Summary:** The mountain pass waystation filled to capacity with displaced families fleeing low-elevation fighting, pitching blankets and burning pine scrap.

#### 8. `loc_override_understory_transmitter_ambient`
- **Location ID:** `loc_understory_transmitter`
- **Override Type:** `ambient_addendum`
- **Active Window:** Days 480–600
- **Display Name:** `Understory Mast Site (Listening Post)`
- **Atmosphere:** `Low generator hum, static hiss, and cold mountain fog.`
- **Summary:** The ridge radio transmitter reoccupied by signal operators listening for encrypted military communiqués and faction war traffic.

---

## 2. Byte-for-Byte Preservation Verification

A checksum and element comparison of entries 0 through 8 in `Assets/StreamingAssets/Data/faction_war_location_overrides.json` against the baseline confirms:
- Indices 0..8 have identical string values across all fields (`id`, `locationId`, `overrideType`, `activeFromDay`, `activeUntilDay`, `displayNameOverride`, `descriptionOverride`, `atmosphereOverride`).
- Serialization order is strictly maintained.
- Unit test `Ashfall.Core.Tests.FactionWarLocationOverridesExpansionTests.BaselineOverrides_PreservedAtIndices0Through8` verifies this contract programmatically.
