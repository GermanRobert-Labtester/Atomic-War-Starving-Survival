#!/usr/bin/env python3
"""
ASHFALL ElevenLabs SFX generator.
Generates game sound effects via the ElevenLabs Sound Generation API.
All outputs: 0.5-8 seconds, saved as WAV in assets/audio/sfx/.
"""

import os
import sys
import time
import json

try:
    from elevenlabs import ElevenLabs
except ImportError:
    print("ERROR: elevenlabs SDK not installed. Run: pip3 install --user elevenlabs")
    sys.exit(1)

API_KEY = os.environ.get("ELEVENLABS_API_KEY", "")
OUTPUT_DIR = os.path.join(os.path.dirname(os.path.dirname(os.path.abspath(__file__))), "assets", "audio", "sfx")

# ASHFALL SFX catalog — post-nuclear survival aesthetic.
# Each entry: (filename_stem, prompt, duration_seconds)
SFX_CATALOG = [
    # ── Shelter / Bunker ────────────────────────────────
    ("sfx_bunker_door_open",
     "Heavy steel bunker door grinding open with hydraulic hiss, echoing in concrete corridor, industrial post-apocalyptic",
     3.0),
    ("sfx_bunker_door_seal",
     "Airtight steel door sealing shut with pneumatic clank and rubber gasket compression, muffled echo, cold industrial",
     2.0),
    ("sfx_ventilation_fan",
     "Constant low industrial ventilation fan humming in enclosed concrete space, slight metallic rattle, nuclear bunker ambience",
     5.0),
    ("sfx_generator_cough",
     "Diesel generator struggling to start, coughing and sputtering, then settling into uneven rumble, underground bunker",
     4.0),
    ("sfx_pipe_clang",
     "Single metallic pipe clang echoing through empty concrete bunker corridor, industrial reverb, cold and lonely",
     1.5),
    ("sfx_water_drip_cave",
     "Slow water droplets falling into shallow puddle in underground concrete chamber, echoing, cold and damp",
     4.0),

    # ── Radiation / Hazards ─────────────────────────────
    ("sfx_geiger_burst",
     "Intense geiger counter crackling burst, rapid radioactive clicks escalating then fading, scientific instrument",
     2.5),
    ("sfx_radiation_alarm",
     "Harsh electronic radiation alarm pulsing with urgent beeps, industrial warning siren, post-apocalyptic bunker",
     3.0),
    ("sfx_contamination_warning",
     "Low ominous electronic hum building into急促 warning tone, hazmat contamination alert, cold industrial facility",
     2.5),
    ("sfx_air_filter_degrade",
     "Air filtration system struggling, fan motor whining higher, air pressure dropping, mechanical distress in enclosed space",
     3.5),

    # ── Weather / Surface ───────────────────────────────
    ("sfx_wind_gust_harsh",
     "Harsh nuclear winter wind gust howling past concrete structures, carrying grit and debris, desolate surface",
     4.0),
    ("sfx_fallout_storm_approach",
     "Distant rumble of approaching fallout storm, low thunder mixed with wind, ominous atmospheric pressure change",
     5.0),
    ("sfx_debris_impact",
     "Heavy debris impact on concrete surface, chunks of rubble falling, dust cloud, post-explosion aftermath",
     1.5),

    # ── Actions / Inventory ─────────────────────────────
    ("sfx_item_pickup_metal",
     "Small metal item being picked up and placed in inventory, metallic clink, tactical equipment handling",
     0.8),
    ("sfx_crafting_assemble",
     "Hands assembling improvised device, small tools clicking, metal parts fitting together, careful mechanical work",
     3.0),
    ("sfx_repair_wrench",
     "Wrench turning bolt, metal on metal repair work, mechanical tightening, industrial maintenance",
     2.0),
    ("sfx_trade_exchange",
     "Heavy objects being exchanged between two people, goods changing hands, barter transaction, post-apocalyptic trade",
     1.5),
    ("sfx_water_pour",
     "Clean water being poured from metal container into cup, liquid splashing, precious resource being measured",
     2.0),
    ("sfx_pill_bottle",
     "Plastic pill bottle being opened and tablets rattling, medical supplies, medicine dosage, survival medicine",
     1.5),

    # ── Combat / Danger ─────────────────────────────────
    ("sfx_distant_explosion",
     "Very distant nuclear explosion muffled by thick concrete walls, low bass thump, barely audible but felt",
     3.0),
    ("sfx_alarm_klaxon",
     "Emergency klaxon alarm blaring in underground bunker, rotating siren tone, urgent military-style alert",
     4.0),
    ("sfx_glass_break_small",
     "Small glass vial breaking on concrete floor, sharp crystalline shatter, medical supplies accident",
     1.0),

    # ── Radio / Communication ───────────────────────────
    ("sfx_radio_tune",
     "Old radio being tuned through static, frequencies sweeping, crackling between stations, analog dial turning",
     3.0),
    ("sfx_radio_signal_lock",
     "Radio finding clear signal through static, audio sharpening into focus, Morse code or voice emerging from noise",
     2.5),
    ("sfx_morse_key",
     "Morse code telegraph key being pressed rapidly, electrical clicking pattern, old communication equipment",
     2.0),

    # ── Medical ─────────────────────────────────────────
    ("sfx_heartbeat_slow",
     "Slow heavy heartbeat, low frequency thumping, medical monitoring, exhausted survival, human body struggling",
     3.0),
    ("sfx_coughing_fit",
     "Person coughing violently, wet productive cough, respiratory illness, radiation sickness symptom",
     2.5),
    ("sfx_injection",
     "Medical syringe injection, small needle puncture, plunger pressing, liquid being administered",
     1.5),
]


def generate_sfx(client, name, prompt, duration, output_dir):
    """Generate a single SFX via ElevenLabs text_to_sound_effects API."""
    output_path = os.path.join(output_dir, f"{name}.mp3")

    print(f"  Generating: {name} ({duration}s)...")
    try:
        audio_chunks = client.text_to_sound_effects.convert(
            text=prompt,
            duration_seconds=duration,
            output_format="mp3_44100_128",
            model_id="eleven_text_to_sound_v2",
        )

        # The API returns an iterator of bytes chunks
        audio_data = b"".join(audio_chunks)

        with open(output_path, 'wb') as f:
            f.write(audio_data)

        print(f"    ✓ Saved: {output_path} ({len(audio_data)} bytes)")
        return True

    except Exception as e:
        print(f"    ✗ FAILED: {e}")
        return False


def main():
    if not API_KEY:
        print("ERROR: Set ELEVENLABS_API_KEY environment variable")
        print("  export ELEVENLABS_API_KEY=sk_...")
        sys.exit(1)

    os.makedirs(OUTPUT_DIR, exist_ok=True)

    client = ElevenLabs(api_key=API_KEY)

    print(f"[ElevenLabs SFX Generator]")
    print(f"  Output: {OUTPUT_DIR}")
    print(f"  Assets to generate: {len(SFX_CATALOG)}")
    print()

    success = 0
    failed = 0
    results = []

    for i, (name, prompt, duration) in enumerate(SFX_CATALOG):
        print(f"[{i+1}/{len(SFX_CATALOG)}]")
        ok = generate_sfx(client, name, prompt, duration, OUTPUT_DIR)
        if ok:
            success += 1
            results.append({"name": name, "status": "ok"})
        else:
            failed += 1
            results.append({"name": name, "status": "failed"})

        # Rate limit: pause between requests
        if i < len(SFX_CATALOG) - 1:
            time.sleep(1.0)

    print(f"\n{'='*50}")
    print(f"Results: {success} generated, {failed} failed, {len(SFX_CATALOG)} total")

    # Write manifest
    manifest_path = os.path.join(OUTPUT_DIR, "sfx_manifest.json")
    manifest = {
        "generated_by": "elevenlabs_sound_generation",
        "total": len(SFX_CATALOG),
        "success": success,
        "failed": failed,
        "assets": []
    }
    for name, prompt, duration in SFX_CATALOG:
        manifest["assets"].append({
            "id": name,
            "prompt": prompt,
            "duration_target": duration,
            "file": f"{name}.mp3"
        })
    with open(manifest_path, 'w') as f:
        json.dump(manifest, f, indent=2)
    print(f"Manifest: {manifest_path}")


if __name__ == "__main__":
    main()
