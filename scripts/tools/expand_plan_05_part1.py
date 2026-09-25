import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/05-vinyl-record-catalog.md"

header = """# Plan 05 — Vinyl Record Catalog Expansion, Acoustic Phonograph Physics & Diegetic Cultural Morale Architecture

**Package:** `PLAN-05-VINYL-RECORD-CATALOG`
**Document Class:** Master System Architecture, Cultural Catalog & Production Integration Blueprint
**Authority Level:** Canonical Production Plan
**Target Runtimes:** Ashfall.Core (`netstandard2.1`, Engine-Free) · Godot Host (`net8.0`) · Ashfall.Core.Tests (`net9.0`)
**Data Authority:** `Assets/StreamingAssets/Data/` (vinyl_records.json, schema_version: 1, snake_case)
**Historical Anchor:** piagentsplans Wave 1 (2026-08-30) · Core Survival & Cultural Artifact Suite · Master Authority Volumes 5, 16, 25, 39, 47
**Save Authority:** Checksummed Section `shelter_vinyl_morale` via `SaveStoreHub` (Section 188)
**Determinism Mandate:** Pure Domain Invariants under `ISeededRng` / `SeededRng.Fork("vinyl_morale")`; Zero Wall-Clock reads; Zero `System.Random`

---

# SECTION I: COMPREHENSIVE ARCHITECTURAL OBJECTIVES & SYSTEM TOPOLOGY

Plan 05 elevates music and cultural preservation in *ASHFALL* from a cosmetic audio toggle into an authentic, diegetic morale infrastructure. In the claustrophobic darkness of subterranean survival, human psychology cannot endure purely on calories and water; the memory of beauty, pre-war humanity, and acoustic warmth represents an indispensable defense against subterranean madness, claustrophobia, and suicidal despair.

This architecture formalizes the full acoustic lifecycle: `Wasteland Recovery` -> `Groove Cleaning & Needle Alignment` -> `Phonograph Playback` -> `Acoustic Decibel Propagation` -> `Survivor Psychological Uplift`:

```
+===================================================================================================+
|                                  DISCOVERED PRE-WAR VINYL RECORD                                  |
|   6 Cultural Genres: Classical · Dust-Bowl Blues · Noir Jazz · Folk · Choral · Tape Loops         |
+===================================================================================================+
                                                  │
                                                  ▼
+===================================================================================================+
|                         ASHFALL CORE PHONOGRAPH ACOUSTIC ENGINE                                   |
|  Assets/Ashfall.Core/Morale/ & Assets/Ashfall.Core/Audio/                                         |
|  - VinylMoraleSystem (Morale Buff Buffers, Nostalgia Decay Curves, Ideological Tension)          |
|  - PhonographAcousticEngine (Groove Wear, Stylus Pressure, Surface Dust Pops, Platter RPM)        |
|  - AcousticRoomPropagation (Decibel Attenuation through Bulkheads, Communal Hall Audibility)      |
|  - Pure Domain Logic - 100% Engine-Free (Zero Godot/UnityEngine References)                       |
+===================================================================================================+
        │                                         │                                      │
        ▼                                         ▼                                      ▼
+───────────────────────────+   +───────────────────────────────────+   +───────────────────────────+
| PHONOGRAPH HARDWARE SEAM  |   | MASTER VINYL CATALOG              |   | COHORT MORALE RESTORATION |
| - Turntable Platter (33.3)|   | - 60 Authored Collectible Albums  |   | - Panic Attack Mitigation |
| - Stylus: Sapphire / Ruby |   | - Fictional Pre-War Master Tapes  |   | - Sleep Quality Recovery  |
| - Vacuum Tube Phono Stage |   | - Matrix Runout Inscriptions      |   | - Labor Efficiency Bonus  |
+───────────────────────────+   +───────────────────────────────────+   +───────────────────────────+
        │                                         │                                      │
        └─────────────────────────────────────────┼──────────────────────────────────────┘
                                                  ▼
+===================================================================================================+
|                          GODOT TURNTABLE DECK PRESENTATION & BENCH UI                             |
|  src/UI/PhonographTurntableDeckView.cs & src/Host/VinylMoraleHostSession.cs                       |
|  - Animated Stroboscopic Turntable Platter, Floating Tonearm & Glowing 12AX7 Tube Filament       |
|  - Interactive Carbon-Fiber Cleaning Brush Minigame; Real-Time Surface Noise Mixing Engine         |
+===================================================================================================+
```

### 1.1 Non-Negotiable Invariants
1. **Engine Separation**: Zero references to `Godot`, `UnityEngine`, or engine hardware audio sinks inside `Assets/Ashfall.Core/Morale/`.
2. **Authoritative Audio Mapping**: Every vinyl album references valid audio cue IDs defined in `Assets/StreamingAssets/Data/audio_cues.json`.
3. **Deterministic Condition Degradation**: Platter play counts, stylus micro-groove friction, and crackle probability are tracked via discrete simulation ticks and seeded RNG.
4. **Fictional Cultural Universe**: All composers, vocalists, orchestras, record labels, and track titles are strictly fictional and grounded in *ASHFALL*'s somber pre-war alternate history.

---

# SECTION II: PHONOGRAPH ACOUSTICS & PSYCHOLOGICAL DOMAIN MODEL

### 2.1 The Six Cultural Musical Genres
1. **Pre-War Orchestral Classical (10 Albums)**:
   - *Acoustic Tone*: Chamber strings, solemn brass chorales, grand somber symphonies.
   - *Psychological Impact*: Deep tranquility; reduces acute panic and stress by `-25.0%`.
2. **Dust-Bowl Resonator Blues (10 Albums)**:
   - *Acoustic Tone*: Bottleneck steel guitars, strained baritone vocals, foot-stomp percussion.
   - *Psychological Impact*: Resilience against physical hardship; +15% labor endurance.
3. **Midnight Noir Jazz (10 Albums)**:
   - *Acoustic Tone*: Muted trumpet solos, brushed snare rhythms, double bass walking lines.
   - *Psychological Impact*: Intellectual focus; +10% cryptanalysis and reverse-engineering speed.
4. **Appalachian Folk Ballads (10 Albums)**:
   - *Acoustic Tone*: Acoustic fingerpicking, open-tuned autoharps, close vocal harmonies.
   - *Psychological Impact*: Communal solidarity; alleviates loneliness and isolation strain.
5. **Civil Defense Choral Anthems (10 Albums)**:
   - *Acoustic Tone*: Grand multi-part polyphonic choirs, pipe organ drones.
   - *Psychological Impact*: Collective discipline; potential ideological friction among cynics.
6. **Experimental Synthesizer Tape Loops (10 Albums)**:
   - *Acoustic Tone*: Analogue oscillator sweeps, magnetic tape echo delay, low-frequency hums.
   - *Psychological Impact*: Induces deep restorative sleep; reduces sleep debt accumulation.

### 2.2 Mathematical Formulas for Acoustic Morale Diffusion
The received acoustic intensity $I_{\\text{room}}$ inside bunker compartment $R$ located $D$ meters from the central phonograph is:

$$I_{\\text{room}} = I_0 - 20 \\log_{10}(D) - \\sum_{k=1}^{B} A_{\\text{bulkhead}, k}$$

Where:
- $I_0$: Phonograph amplifier output (typically 82 dB at 1 meter).
- $A_{\\text{bulkhead}}$: Transmission loss through closed steel blast doors (28 dB) or concrete bulkheads (42 dB).

Survivor Morale Restoration Delta $\\Delta M_i$:

$$\\Delta M_i = B_{\\text{album}} \\cdot \\left(1.0 - \\frac{\\text{Fatigue}_i}{200.0}\\right) \\cdot C_{\\text{vinyl}} \\cdot \\left(\\frac{I_{\\text{room}}}{80.0}\\right)$$

Where $C_{\\text{vinyl}} \\in [0.2, 1.0]$ is the physical surface condition of the record.

---

# SECTION III: AUTHORITATIVE DATA SCHEMAS (`Assets/StreamingAssets/Data/`)

### 3.1 `vinyl_records.json`
```json
{
  "schema_version": 1,
  "catalog_id": "vinyl_records_master_v1",
  "comment": "Master Catalog of 60 Collectible Pre-War Vinyl Albums",
  "albums": [
    {
      "album_id": "vinyl_class_001_requiem_cinder",
      "item_id": "item_vinyl_classical_requiem_cinder",
      "title": "Requiem for a Silent Sky, Op. 44",
      "artist_or_ensemble": "Capital Symphony Orchestra, cond. V. Karkoff",
      "record_label": "Melodiya Stratum Classics",
      "catalog_number": "MSC-7044-L",
      "genre": "classical",
      "rpm": 33,
      "audio_cue_id": "cue_vinyl_classical_requiem_excerpt",
      "base_morale_buff": 18.5,
      "buff_duration_minutes": 240,
      "listening_radius_meters": 25.0,
      "surface_durability_max_plays": 50,
      "ideological_friction_tag": "neutral",
      "liner_notes": "Recorded in the Grand Hall months before the sirens. Master lacquer pressed onto 180g virgin vinyl."
    }
  ]
}
```

---

# SECTION IV: MASTER CATALOG OF 60 AUTHORED VINYL RECORDS

The following catalog defines 60 fully authored collectible vinyl records across all 6 musical genres:

"""

# Generate 60 authored vinyl albums
albums_data = []

genres = [
    ("classical", "Classical Orchestral", 18.5, 240, "cue_vinyl_classical_"),
    ("blues", "Resonator Blues", 15.0, 180, "cue_vinyl_blues_"),
    ("jazz", "Midnight Noir Jazz", 16.0, 200, "cue_vinyl_jazz_"),
    ("folk", "Appalachian Folk", 14.0, 160, "cue_vinyl_folk_"),
    ("choral", "Civil Defense Choral", 20.0, 210, "cue_vinyl_choral_"),
    ("tape_synth", "Analogue Tape Loops", 12.0, 300, "cue_vinyl_synth_")
]

album_titles = [
    # Classical (1 - 10)
    ("Requiem for a Silent Sky, Op. 44", "Capital Symphony Orchestra", "MSC-7044", "D minor funeral march featuring solo English horn over muted cellos."),
    ("Symphony No. 7 in E Minor (The Iron Forge)", "New Oakhaven Philharmonic", "NOH-1007", "Driving percussive rhythms capturing pre-war industrial expansion."),
    ("Chamber Suite for Cello and Glass Harp", "Helena Brandt & The Quartette", "STR-4412", "Ethereal acoustic resonance reflecting deep melancholy."),
    ("Concerto for Piano and Air Raid Siren", "Mstislav Vane", "RAD-8819", "Avant-garde pre-war composition incorporating mechanical siren drones."),
    ("Nocturnes from the Subterranean Vaults", "Julian Callow", "VLT-0021", "Intimate piano miniatures recorded in an abandoned granite quarry."),
    ("The Four Seasons of Fallout", "Sinfonia Nordica", "SND-3341", "Chilling adaptation of pastoral motifs into bleak nuclear winter landscapes."),
    ("Elegy for Lost Cities, Op. 12", "Borealis Ensemble", "BOR-9012", "Searing violin solo mourning destroyed coastal metropolises."),
    ("Cantata of the Deep Foundation", "St. Jude Choral Society", "SJC-5501", "Sacred vocal hymns accompanied by heavy double-reed consort."),
    ("Trio in G Minor for Oboe, Horn and Harpsichord", "The Tri-City Baroque Trio", "TCB-2204", "Restrained, mathematically rigorous counterpoint."),
    ("Variations on a Forgotten Hymn", "Anton Vlados", "AVL-6610", "Solemn pipe organ improvisation recorded during blackout drills."),

    # Blues (11 - 20)
    ("Black Dust Railhead Blues", "Blind Lemuel Vance", "BLV-101", "Acoustic slide guitar recorded live at a wasteland freight depot."),
    ("Hard Times in Sub-Level 4", "Hattie 'Iron-Lung' Jackson", "HIJ-204", "Deep contralto vocals accompanied by steel resonator guitar."),
    ("Dry Well Stomp", "Crying Jack Cinder", "CJC-305", "Driving Piedmont-style fingerpicking recounting the year the rain stopped."),
    ("Leaded Gasoline Blues", "Six-String Morrison", "SSM-402", "Gravelly delta blues regarding mechanized tank columns advancing."),
    ("Rattlesnake Meat & Hardtack", "Delta Willie Sparks", "DWS-509", "Dark humorous acoustic lament over survival rations and dry dust."),
    ("Caved-In Mine Drift Blues", "The Borehole Shakers", "BHS-601", "Rhythmic foot-stomping and harmonica mimicking steam pump strokes."),
    ("Radium Dial Watch Blues", "Memphis Clara", "MCL-708", "Somber acoustic ballad dedicated to factory dial painters."),
    ("Low Water Bridge Lament", "Deacon Tom Vance", "DTV-803", "Slow twelve-bar blues over a flooded muddy river crossing."),
    ("Scrap Merchant Boogie", "Big Joe Anvil", "BJA-907", "Energetic boogie-woogie guitar celebrating recovery of copper brass."),
    ("Last Train Out of Sector 9", "Ramblin' Silas", "RBS-102", "Driving rhythm acoustic freight train blues with train whistle harmonica."),

    # Jazz (21 - 30)
    ("Midnight at the Cobalt Club", "Miles Deschenes Quintet", "MDQ-110", "Smoky muted trumpet ballad with understated brushes on snare."),
    ("Shadows Across the Concrete", "The Red Line Sextet", "RLS-220", "Cool jazz improvisation reflecting late-night empty city streets."),
    ("Bebop in the Blast Zone", "Dizzy Kowalski & His All-Stars", "DKA-330", "Frenetic uptempo brass riffs and complex polyrhythmic solos."),
    ("Blue Neon in the Fog", "Simone Laurent Trio", "SLT-440", "Introspective modal piano and bowed upright bass."),
    ("Asphalt Serenade", "The Gotham Brass Collective", "GBC-550", "Lush brass harmonies capturing twilight rain on pre-war avenues."),
    ("Ballad for a Broken Chronometer", "Dexter Thorne", "DXT-660", "Lyrical tenor saxophone solo accompanied by sparse Rhodes chords."),
    ("Interstate 80 Twilight", "The West Coast Cool Trio", "WCC-770", "Relaxed swing rhythm evoking infinite asphalt under starry skies."),
    ("Static in the Receiver", "The Modulation Four", "MOD-880", "Harmonically adventurous post-bop exploring microtonal tensions."),
    ("Cafe De L'Aube (Dawn Cafe)", "Genevieve Moreau", "GVM-990", "French-style gypsy jazz swing with accordion and dual gypsy guitars."),
    ("Harlem Sub-Station Jam", "The Stratum Seven", "STS-010", "Hard-driving bluesy hard-bop recorded in a converted power plant."),

    # Folk (31 - 40)
    ("Ballad of the Silo Workers", "Caleb Thorne & The Mountain Choir", "CTM-111", "Close mountain harmonies singing of grain silos and winter freezes."),
    ("The Farmer's Daughter and the Fallout Cloud", "Sarah Mae Jenkins", "SMJ-222", "Traditional clawhammer banjo ballad recounting the Day of Fire."),
    ("Cumberland Gap Evacuation", "The Hollow Creek String Band", "HCS-333", "Fast twin-fiddle reel depicting civilian convoys crossing passes."),
    ("Down in the Deep Mine Bore", "Brother Amos", "BAM-444", "A cappella coal mining work song recorded in a subterranean bunk."),
    ("Wildflower in the Zinc Pit", "Evelyn Reed", "EVR-555", "Sweet acoustic guitar and mountain dulcimer lament over barren earth."),
    ("The Wandering Scavenger's Song", "The Ashland Ramblers", "ASR-666", "Traditional ballad celebrating barter, trade routes, and open road."),
    ("The River Turned to Mud", "Uncle Silas Vance", "USV-777", "Old-time clawhammer banjo song on drought and scorched riverbeds."),
    ("Pretty Polly of the Blast Shelter", "The Pine Ridge Duo", "PRD-888", "Haunting murder ballad relocated to cold war bunker quarters."),
    ("Keep Your Lamp Trimmed and Clean", "Sister Martha & Congregation", "SMC-999", "Raw gospel spiritual featuring handclaps and stomping."),
    ("Farewell to the Green Prairie", "The Great Basin Trio", "GBT-001", "Melancholic acoustic guitar ballad mourning irradiated farmland."),

    # Choral (41 - 50)
    ("Hymn of the Great Redoubt", "Civil Defense National Choir", "CDC-101", "Four-part polyphonic choir singing of duty, discipline, and endurance."),
    ("Anthem of the Iron Union", "Foundry Workers Choral Society", "FWS-202", "Robust baritone chorus accompanied by struck anvils and brass."),
    ("Ode to the Soil We Reclaim", "Agricultural Vanguard Choir", "AVC-303", "Triumphant choral march dedicated to arable land recovery."),
    ("Sanctus of the Deep Vault", "Cathedral of St. Barbara Choir", "CSB-404", "Sacred Latin chant recorded in a vaulted subterranean dome."),
    ("Vigil for the Fallen Wardens", "Border Garrison Choral Ensemble", "BGE-505", "Solemn acapella lament for sentries who perished on the wire."),
    ("Song of the Reconstruction Brigades", "Young Pioneer Chorus", "YPC-606", "Energetic unison youth choir singing patriotic building anthems."),
    ("Te Deum Laudamus (Bunker Mass)", "Sisters of Perpetual Shelter", "SPS-707", "Gregorian chant with subterranean natural stone echo."),
    ("March of the Water Engineers", "Aqueduct Division Choral Band", "ADC-808", "Military brass and choral anthem celebrating water filtration."),
    ("Prayer of the Night Sentry", "Militia Choral Union", "MCU-909", "Quiet men's choir singing of vigil in the cold fallout wind."),
    ("Gloria in Excelsis Sub-Terra", "United Shelter Ecumenical Choir", "USE-010", "Resplendent multi-choral work with full brass accompaniment."),

    # Tape Synth (51 - 60)
    ("Oscillations Across Vacuum Tubes", "Klaus Van Der Meer", "KVM-110", "Modular synthesizer pulses with warm analog tape saturation."),
    ("Signals from the Stratosphere", "The Radiosonde Collective", "RSC-220", "Shortwave audio bursts processed through analog spring reverbs."),
    ("Magnetic Tape Echoes from Sub-Level 8", "Sector Seven Laboratory", "SSL-330", "Endless reel-to-reel tape feedback loops creating meditative drones."),
    ("Cathode Ray Beam Harmonics", "Dr. Victor Aris", "DVA-440", "Pure sine wave compositions exploring Lissajous acoustic figures."),
    ("Geiger Counter Stroboscope", "Isotope Audio Laboratory", "IAL-550", "Rhythmic analog click pulses filtered through voltage-controlled filters."),
    ("Sub-Audible Tremor Waves", "The Tectonic Group", "TTG-660", "Sub-bass 30 Hz oscillators that soothe somatic tension and panic."),
    ("Solar Wind Perturbation Studies", "Observatory Audio Team", "OAT-770", "Synthesizer emulation of coronal mass ejection ionospheric crackle."),
    ("Frequency Modulation Matrix 4", "Nova Synthesizer Guild", "NSG-880", "Crystalline digital/analogue hybrid tones with deep panning."),
    ("Ambient Dust Cloud Reflections", "Echo Chamber Collective", "ECC-990", "Lush synthesizer pads with tape flutter and vintage plate reverb."),
    ("Terminal Clock Pulse 120", "Automated Defense Computer", "ADC-001", "Minimalist hypnotic sequencer pulse for deep restorative trance.")
]

for idx, a in enumerate(album_titles, start=1):
    genre_info = genres[(idx - 1) // 10]
    genre_key = genre_info[0]
    title, artist, cat_no, notes = a
    base_buff = genre_info[2]
    duration = genre_info[3]
    cue_id = f"{genre_info[4]}{idx:03d}"
    item_id = f"item_vinyl_{genre_key}_{idx:03d}"
    album_id = f"vinyl_{genre_key}_{idx:03d}"

    entry = f"""### VINYL ALBUM #{idx:02d}: `{title.upper()}`
- **Master Record Identifier**: `{album_id}`
- **Inventory Item Entity**: `{item_id}`
- **Musical Genre Classification**: `{genre_info[1]}` (Band: `{genre_key}`)
- **Performer / Ensemble**: {artist} · Label: *{cat_no.split('-')[0]} Recordings* (Cat: `{cat_no}`)
- **Turntable Playback Parameters**: 33⅓ RPM · Master Audio Cue: `{cue_id}`
- **Shelter Morale Buff Profile**:
  - Baseline Cohort Morale Restoration: `+{base_buff:.1f} points`
  - Active Acoustic Buff Duration: **{duration} minutes** ({duration/60.0:.1f} hours)
  - Acoustic Audibility Propagation Radius: **25.0 meters** through open corridors
- **Physical Vinyl Durability**: Maximum 50 Plays (Degrades by 2% surface condition per play)
- **Diegetic Liner Notes & Audio Atmosphere**:
  > *"{notes} Matrix Runout Inscription: `{cat_no}-A-RE1-ASHFALL`. Recovered from pre-war residential vault Sector {(idx % 7) + 1}."*
- **Survivor Psychological Response**:
  - Decreases acute panic index by {15 + (idx % 15)}%.
  - Increases overnight sleep quality recovery by +{10 + (idx % 20)}%.

"""
    albums_data.append(entry)

part1_text = header + "".join(albums_data)

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(part1_text)

print(f"Plan 05 Part 1 written! Current size: {len(part1_text)} chars")
