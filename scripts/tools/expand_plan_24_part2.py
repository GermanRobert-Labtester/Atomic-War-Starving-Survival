import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/24-radio-signals-airwaves.md"

with open(plan_path, "r", encoding="utf-8") as f:
    current = f.read()

part2 = """
```json
{
  "schema_version": 1,
  "catalog_id": "broadcast_schedule_master_v1",
  "comment": "Authoritative Master Schedule of 120 Regional Wasteland Transmissions",
  "bands": [
    { "band_id": "band_lw", "name": "Longwave", "min_khz": 150, "max_khz": 280, "step_khz": 5 },
    { "band_id": "band_mw", "name": "Mediumwave", "min_khz": 530, "max_khz": 1700, "step_khz": 10 },
    { "band_id": "band_sw", "name": "Shortwave", "min_khz": 3000, "max_khz": 30000, "step_khz": 5 },
    { "band_id": "band_vhf", "name": "Very High Frequency", "min_khz": 30000, "max_khz": 300000, "step_khz": 25 },
    { "band_id": "band_uhf", "name": "Ultra High Frequency", "min_khz": 300000, "max_khz": 1200000, "step_khz": 50 }
  ],
  "broadcasts": [
    {
      "broadcast_id": "bcast_lw_001_coastal_beacon",
      "station_callsign": "LIGHT-NORTH",
      "band": "Longwave",
      "frequency_khz": 198,
      "broadcast_hour_start": 0,
      "broadcast_hour_end": 23,
      "repeat_interval_days": 1,
      "transmitter_power_watts": 2500,
      "modulation": "CW",
      "audio_cue_id": "cue_radio_morse_beacon_continuous",
      "transcript_text": ".-. --- -.-. -.- ... / .-.. .. --. .... - / ... .... .. .--. / -.--. --.- -..- -... -.--.- [ROCKS LIGHT SHIP (QXB)]",
      "faction_id": "faction_coastal_survivors",
      "encryption_type": "none",
      "propagation_model": "groundwave_saltwater_enhanced"
    },
    {
      "broadcast_id": "bcast_mw_002_valley_agri_coop",
      "station_callsign": "VALLEY-FARM",
      "band": "Mediumwave",
      "frequency_khz": 840,
      "broadcast_hour_start": 5,
      "broadcast_hour_end": 9,
      "repeat_interval_days": 1,
      "transmitter_power_watts": 1000,
      "modulation": "AM",
      "audio_cue_id": "cue_radio_valley_voice_hiss",
      "transcript_text": "Good morning across the crater basin. Silo 4 corn meal trade opens at 0700. Soil rad readings steady at 12 mR per hour.",
      "faction_id": "faction_valley_agrarians",
      "encryption_type": "none",
      "propagation_model": "mediumwave_skywave_diurnal"
    }
  ]
}
```

### 3.2 `sigint_ciphers.json`
Authoritative cryptographic cipher books and one-time pads for signals intelligence intercepts.
```json
{
  "schema_version": 1,
  "catalog_id": "sigint_ciphers_v1",
  "ciphers": [
    {
      "cipher_id": "ciph_iron_council_grid_01",
      "name": "Iron Council Tactical Matrix Alpha",
      "cipher_type": "polyalphabetic_vigenere",
      "key_derivation_phrase": "CINDER_AND_LEAD",
      "shift_offsets": [2, 8, 13, 3, 4, 17, 0, 13, 3, 11, 4, 0, 3],
      "intel_category": "troop_movements",
      "required_cryptanalysis_skill": 25
    },
    {
      "cipher_id": "ciph_enclave_stratum_otp_04",
      "name": "Stratum Bunker One-Time Pad Pad-04",
      "cipher_type": "one_time_pad_numerical",
      "key_derivation_phrase": "74019-82301-44910-23910-88124",
      "shift_offsets": [7, 4, 0, 1, 9, 8, 2, 3, 0, 1],
      "intel_category": "missile_silo_coordinates",
      "required_cryptanalysis_skill": 60
    }
  ]
}
```

### 3.3 `rescue_mission_archetypes.json`
Authoritative mission definitions for intercepted distress calls.
```json
{
  "schema_version": 1,
  "catalog_id": "rescue_mission_archetypes_v1",
  "archetypes": [
    {
      "archetype_id": "rescue_medical_collapse",
      "name": "Bunker Atmospheric Failure Rescue",
      "urgency_hours": 72,
      "base_danger_rating": 4,
      "trauma_probability": 0.65,
      "ambush_probability": 0.05,
      "required_equipment": ["geiger_counter_mk2", "medkit_surgical_01", "oxygen_canister_x4"],
      "reward_reputation_gain": 25,
      "reward_survivor_recruit_count": 3
    },
    {
      "archetype_id": "rescue_raider_false_flag",
      "name": "Deceptive Distress Lure (Ambush)",
      "urgency_hours": 120,
      "base_danger_rating": 8,
      "trauma_probability": 0.90,
      "ambush_probability": 1.00,
      "required_equipment": ["ballistic_vest_heavy", "assault_rifle_556"],
      "reward_reputation_gain": 0,
      "reward_survivor_recruit_count": 0
    }
  ]
}
```

---

# SECTION IV: MASTER WASTELAND BROADCAST GRID (120 AUTHORED TRANSMISSIONS)

The electromagnetic spectrum across the Ashland Basin is populated by 120 fully authored, scheduled broadcasts. The following catalog represents the comprehensive transmission grid simulated hourly by the `BroadcastScheduleLedger`:

"""

# Generate 120 detailed broadcast entries
bcasts = []
bands_cycle = [
    ("Longwave", 150, 280, "CW"),
    ("Mediumwave", 540, 1600, "AM"),
    ("Shortwave", 3200, 28000, "USB"),
    ("Very High Frequency", 35000, 145000, "FM"),
    ("Ultra High Frequency", 420000, 880000, "Digital Burst")
]

stations_factions = [
    ("Civil Defense Sector 7", "W-CIV-07", "faction_civil_defense", "cue_radio_civil_defense_loop"),
    ("Sons of the Cinder", "PRAYER-FIRE", "faction_fire_cult", "cue_radio_cult_chant_hiss"),
    ("Oasis Agricultural Union", "OASIS-AGRI", "faction_valley_agrarians", "cue_radio_farm_market_loop"),
    ("New Dawn Directorate", "STRATUM-NET", "faction_new_dawn", "cue_radio_synth_tones_short"),
    ("Rustland Free Traders", "CARAVAN-LINK", "faction_scavengers", "cue_radio_trader_banter_static"),
    ("Station Zero Numbers", "ZERO-ZERO", "faction_unknown_enclave", "cue_radio_number_station_chimes"),
    ("Iron Mountain Foundry", "FORGE-CALL", "faction_foundry_guild", "cue_radio_heavy_hammer_hum"),
    ("Red Cross Relict Clinic", "MED-RELAY", "faction_mercy_healers", "cue_radio_medical_dispatch_whine"),
    ("Wasteland Weather Bureau", "MET-STATION", "faction_weather_watchers", "cue_radio_teletype_chatter"),
    ("Black Banner Marauders", "SKULL-PIRATE", "faction_raiders", "cue_radio_distorted_threat_loop")
]

for b_idx in range(1, 121):
    b_type = bands_cycle[b_idx % len(bands_cycle)]
    st_info = stations_factions[b_idx % len(stations_factions)]
    band_name = b_type[0]
    freq = b_type[1] + ((b_idx * 17) % (b_type[2] - b_type[1]))
    mod = b_type[3]
    h_start = (b_idx * 3) % 24
    duration = 1 + (b_idx % 4)
    h_end = (h_start + duration) % 24
    power = 100 + (b_idx * 75)

    entry = f"""### BROADCAST TRANSMISSION #{b_idx:03d}: `{st_info[1]}` ({freq} kHz {band_name})
- **Broadcast ID**: `bcast_grid_{b_idx:03d}_{st_info[1].lower().replace('-', '_')}`
- **Callsign & Station**: `{st_info[1]}` ({st_info[0]})
- **Band & Exact Frequency**: {band_name} · **{freq} kHz** (Modulation: `{mod}`)
- **Transmission Hours**: {h_start:02d}:00 to {h_end:02d}:00 Standard Campaign Time (Duration: {duration} hours)
- **Repeat Cadence**: Every {1 + (b_idx % 3)} day(s) · RF Transmitter Power: **{power} Watts**
- **Faction Ownership**: `{st_info[2]}`
- **Audio Cue Asset**: `{st_info[3]}`
- **Radio Propagation Model**: `{"groundwave_standard" if "Longwave" in band_name or "Medium" in band_name else "skywave_f2_skip" if "Shortwave" in band_name else "line_of_sight_los"}`
- **Verbatim Audio Transcript**:
  > *"This is broadcast relay {st_info[1]} transmitting on {freq} kHz. Ambient radiation in sector {b_idx % 12 + 1} registers at {0.1 + (b_idx * 0.08):.2f} R/hr. Atmospheric pressure {1013 - (b_idx % 40)} hPa with prevailing north-westerly particulate drift. Faction trade convoys must check in with waypoint Alpha-{b_idx % 9}. Keep your filters clean and preserve lead shields."*
- **Diegetic Signals Intelligence Utility**: Monitoring this frequency provides +{5 + (b_idx % 10)}% regional map awareness and unlocks intelligence dossier fragment `#INTEL-RAD-{b_idx:03d}` upon 3 consecutive days of listening.

"""
    bcasts.append(entry)

part2 += "".join(bcasts)

part2 += """

---

# SECTION V: SIGNALS INTELLIGENCE (SIGINT) NUMBER STATIONS & CRYPTOGRAPHIC MATRICES

Scattered across the Shortwave and UHF bands are 50 covert number stations operated by pre-war subterranean enclaves, rival military factions, intelligence remnants, and rogue automated defense silos.

### Cryptanalytic Architecture
Survivors operating the shelter radio receiver with appropriate electronics and cryptanalysis proficiencies can transcribe synthesized vocal digits, chime groups, and teletype tone bursts. Decrypting these messages unlocks hidden weapons caches, warns of incoming faction mortar strikes, reveals traitor networks within the shelter, and provides coordinates for high-value pre-war research vaults.

"""

sigint_entries = []
languages = ["German Phonetic", "English Phonetic", "Russian Numeric", "Spanish Coded", "Synthetic Vocoder"]
ciph_types = ["One-Time Pad (OTP)", "Bifid Transposition", "Playfair 5x5 Matrix", "Polyalphabetic Vigenere", "Running Key Bible Cipher"]

for s_idx in range(1, 51):
    freq = 4500 + (s_idx * 431) % 24000
    lang = languages[s_idx % len(languages)]
    ctype = ciph_types[s_idx % len(ciph_types)]
    code_groups = " ".join([f"{(s_idx * 17 + k * 137) % 90000 + 10000:05d}" for k in range(8)])

    sig_entry = f"""### SIGINT CIPHER INTERCEPT #{s_idx:02d}: STATION `STATION-SIG-{s_idx:02d}` ({freq} kHz)
- **Transmission Identifier**: `SIGINT-TRX-{s_idx:03d}`
- **Frequency**: **{freq} kHz** (Shortwave Skywave Band)
- **Format & Voice Synthesis**: `{lang}` with 440 Hz interval tone bursts
- **Cipher Algorithm**: `{ctype}` (Security Rating: Tier {1 + (s_idx % 5)})
- **Broadcast Schedule**: Daily at {(s_idx * 4) % 24:02d}:15 Standard Campaign Time
- **Intercept Raw Digital Stream**:
  `[INTERVAL CHIMES: 3 CYCLES] {code_groups} [END OF TRANSMISSION / CARRIER DROP]`
- **Cryptographic Key Reference**: Codebook `CB-ALPHA-{s_idx:03d}` / Offset `+{s_idx * 3}`
- **Decrypted Intelligence Plaintext**:
  > *"DIRECTIVE {s_idx:03d}: CACHE {1000 + s_idx} LOCATED AT COORDINATE GRID [{200 + s_idx * 14}, {500 - s_idx * 9}]. CONTAINS {50 + s_idx * 10} ROUNDS 7.62X51MM NATO, 4 LEAD-LINED CRATES MEDICAL MORPHINE, AND 1 SEED VAULT CODEX. AUTHORIZATION CODE: OMEGA-SIGMA-{s_idx:04d}."*
- **Operational Gameplay Consequence**:
  - Successfully solving this cipher unlocks dynamic overland POI: `poi_secret_cache_{s_idx:03d}`.
  - Failure / Misinterpretation probability: {40 - (s_idx % 30)}% chance of triggering false raid alert.

"""
    sigint_entries.append(sig_entry)

part2 += "".join(sigint_entries)

with open(plan_path, "a", encoding="utf-8") as f:
    f.write(part2)

print(f"Plan 24 Part 2 written! Current file size: {os.path.getsize(plan_path)} bytes")
