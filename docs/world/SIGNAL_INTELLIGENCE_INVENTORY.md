# Signal Intelligence Inventory & Cipher Hunt Architecture — Plan 11

> **Document Class:** Signal Intelligence Catalog & Quest Architecture
> **Authority:** `Assets/Ashfall.Core/Narrative/SignalIntelligenceCatalog.cs`, `Assets/StreamingAssets/Data/narrative/numbers_station_ciphers.json`
> **Radio Tuning Authority:** `Assets/Ashfall.Core/Radio/RadioTuner.cs`, `Assets/StreamingAssets/Data/radio.json`
> **Save Key:** `radio`, `questline_master`

---

## 1. Executive Summary

ASHFALL's signal-intelligence layer models shortwave transmissions, numbers stations, seismic alarms, and bunker wiretaps. Plan 11 activates this layer into multi-stage cipher treasure hunts:
$$\text{Intercept Broadcast} \longrightarrow \text{Acquire Key/Codebook} \longrightarrow \text{Decode Coordinates} \longrightarrow \text{Reveal Hidden Map Node} \longrightarrow \text{Payoff Expeditions}$$

---

## 2. Signal Intelligence Catalog Inventory

| Catalog Entry Type | Source File | Record Count | Runtime Consumer | Status |
|---|---|---|---|---|
| **Numbers Station Ciphers** | `numbers_station_ciphers.json` | 8 | `SignalIntelligenceCatalog`, `RadioHostSession` | Active & Expanded |
| **Seismic Fault Alarms** | `seismic_array_fault_alarms.json` | 6 | `SignalIntelligenceCatalog` | Active |
| **EMP Atmospheric Sniffers** | `emp_atmospheric_sniffer_logs.json` | 6 | `SignalIntelligenceCatalog` | Active |
| **Bunker Wiretap Transcripts** | `bunker_wiretap_transcripts.json` | 6 | `SignalIntelligenceCatalog` | Active |

---

## 3. Authored Cipher Quest Chains

### Chain 1: "The Relay Count"
- **Broadcast:** `radio_broadcast_relay_count` / `cipher_station_relay_count` (104.5 MHz / 14487 kHz)
- **Prose:** Repeating numeric phonetic groups: `8-2-6... 9-0-1... DELTA-7... repeating sequence 4-4-1-9-2`.
- **Required Key:** `item_comm_codebook_alpha` (excavated from Collapsed Command Vault).
- **Decoded Destination:** `loc_hidden_relay_bunker` ("Hidden Relay Bunker 09").
- **Payoff:** Pre-war high-frequency transceiver components, `item_military_radio`, classified telemetry logs.
- **Flags:** `flag_sig_relay_heard` $\rightarrow$ `flag_sig_relay_key_found` $\rightarrow$ `flag_sig_relay_decoded` $\rightarrow$ `flag_sig_relay_location_revealed` $\rightarrow$ `flag_sig_relay_resolved`.

### Chain 2: "Winter Ledger"
- **Broadcast:** `radio_broadcast_winter_ledger` / `cipher_station_winter_ledger` (94.2 MHz / 6890 kHz)
- **Prose:** Automated logistics broadcast reading cold-storage inventory batches and sub-tier access offsets.
- **Required Key:** `item_logistics_cipher_sheet` (recovered from Metro Interchange / logistical depots).
- **Decoded Destination:** `loc_logistics_reserve_cache` ("Sub-Basement Logistics Reserve").
- **Payoff:** Preserved emergency medical crates, water purification tablets, high-grade filters.
- **Flags:** `flag_sig_winter_heard` $\rightarrow$ `flag_sig_winter_key_found` $\rightarrow$ `flag_sig_winter_decoded` $\rightarrow$ `flag_sig_winter_location_revealed` $\rightarrow$ `flag_sig_winter_resolved`.

### Chain 3: "Last Rotation"
- **Broadcast:** `radio_broadcast_last_rotation` / `cipher_station_last_rotation` (107.8 MHz / 8930 kHz)
- **Prose:** Command cadre dead-hand standby prompt: `Waypoint November protocol active. Authenticate with index cylinder.`
- **Required Key:** `item_archive_index_cylinder` (recovered from Pre-War Archive Bunker).
- **Decoded Destination:** `loc_deaddrop_command_shelter` ("Dead-Drop Command Shelter").
- **Payoff:** Specialized cipher hardware, classified directive microfilm, rare technical schematics.
- **Flags:** `flag_sig_rotation_heard` $\rightarrow$ `flag_sig_rotation_key_found` $\rightarrow$ `flag_sig_rotation_decoded` $\rightarrow$ `flag_sig_rotation_location_revealed` $\rightarrow$ `flag_sig_rotation_resolved`.

---

## 4. Map Node Reveal Semantics
- Prior to decoding, the hidden node is marked `discoverable = true`, `startingUnlocked = false`, and remains invisible in `WastelandMapView` and unreachable by route planning.
- Upon decoding, `WastelandMapSystem.Discover(locationId)` executes, placing the marker on the travel atlas and unlocking BFS shortest-path route planning to the destination.
- All state persists through `WastelandMapSaveStore` and `QuestlineSaveStore`.
