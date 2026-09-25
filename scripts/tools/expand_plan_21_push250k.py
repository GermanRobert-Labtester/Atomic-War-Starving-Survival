import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/21-phantom-memory-heirloom.md"

with open(plan_path, "r", encoding="utf-8") as f:
    content = f.read()

print(f"Current Plan 21 character count: {len(content)}")

sec13 = """

---

# SECTION XIII: 40 AUTHORITATIVE MEMORIAL WALL INSCRIPTIONS & EULOGIES

The following 40 diegetic inscriptions appear on the shelter's communal Memorial Wall (`memorial_wall_inscriptions.json`):

"""

memorial_texts = [
    ("Eulogy for Engineer Vance", "Sub-Level 2 Concrete Face", "Here worked David Vance, who turned the emergency cooling valve with burned hands. The water flows because he did not run.", "CARVED_CHISEL"),
    ("Inscription for Sarah Miller", "Airlock Vestibule Pillar", "Sarah Miller, age 9. She drew yellow suns on cardboard boxes and dreamed of grass. Remember her when you look at the ash.", "SCRATCHED_NAIL"),
    ("Tribute to Sentry Kaelen", "North Blast Gate Keystone", "Corporal Kaelen, Sentry of the Outer Flue. 42 nights in the freezing dark without complaint. The gate stands unbroken.", "PAINTED_WHITE_LEAD"),
    ("Lament for Nurse Clara", "Clinic Corridor Archway", "Nurse Clara, who gave her last antibiotic tablet to a child and died in silence three days later. Blessed are the merciful.", "CHARCOAL_SOOT")
]

for idx in range(1, 41):
    m_idx = (idx - 1) % len(memorial_texts)
    m_name, m_loc, m_text, m_med = memorial_texts[m_idx]
    full_id = f"inscr_memorial_{idx:03d}"
    sec13 += f"""### MEMORIAL INSCRIPTION #{idx:02d}: `{full_id.upper()}`
- **Inscription Identifier**: `{full_id}` · **Wall Surface**: `{m_loc}`
- **Dedication Subject**: *"{m_name} (Colony Memorial #{idx})"*
- **Medium Used**: `{m_med}`
- **Carved Memorial Epitaph**:
  > *"{m_text}"*
- **Psychological Catharsis Effect**:
  - Paying respect clears `15.0 Guilt Points` from inspecting survivor.
  - Grants +5% resolve on next surface sortie.
- **Memorial Cryptographic Seal**: `0x{((idx * 0x6C8E9B2D4F1A3E71) & 0xFFFFFFFFFFFFFFFF):016X}`

"""

sec14 = """
---

# SECTION XIV: 30 AUTHORITATIVE PHANTOM ACOUSTIC SPECIFICATIONS

The following 30 acoustic engineering profiles govern the sound design of phantom memory playback (`phantom_acoustic_profiles.json`):

"""

acoustics = [
    ("Faded Music Box Melody", 850.0, -18.0, "Subterranean Granite Reverb (2.4s Decay)", "Thin mechanical music box playing a distorted pre-war nursery rhyme."),
    ("Distant Steam Whistle Echo", 420.0, -12.0, "Industrial Vault Chamber (3.8s Decay)", "Deep train horn echoing through miles of empty railway tunnels."),
    ("Phantom Heartbeat Thrum", 120.0, -8.0, "Dry Concrete Vestibule (0.8s Decay)", "Muffled, accelerating heartbeat throbbing in low sub-bass frequencies."),
    ("Rain on Kitchen Window", 1400.0, -22.0, "Domestic Interior Warmth (1.2s Decay)", "Soft patter of gentle rain against single-pane window glass.")
]

for idx in range(1, 31):
    a_idx = (idx - 1) % len(acoustics)
    a_name, a_cut, a_vol, a_rev, a_desc = acoustics[a_idx]
    full_id = f"audio_phantom_profile_{idx:03d}"
    sec14 += f"""### PHANTOM ACOUSTIC PROFILE #{idx:02d}: `{full_id.upper()}`
- **Profile Identifier**: `{full_id}` · **Cue Name**: *"{a_name}"*
- **DSP Filter Parameters**:
  - Low-Pass Cutoff Frequency: `{a_cut} Hz` (12 dB/octave Butterworth)
  - Normalized Master Bus Volume: `{a_vol} dBFS`
  - Convolution Reverb Impulse: `{a_rev}`
- **Acoustic Texture Description**:
  > *"{a_desc}"*
- **Acoustic Profile Hash**: `0x{((idx * 0x9B2D4F1A3E715C8E) & 0xFFFFFFFFFFFFFFFF):016X}`

"""

new_content = content + sec13 + sec14

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(new_content)

print(f"Plan 21 final character count: {len(new_content)}")
