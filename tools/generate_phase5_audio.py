#!/usr/bin/env python3
"""
tools/generate_phase5_audio.py

Synthesizes Phase 5 Audio Assets for ASHFALL:
- Diegetic Radio & Broadcasts:
  - assets/audio/radio/radio_numbers_station.wav (chime preamble + formant cipher digits through shortwave QSB flutter)
  - assets/audio/radio/radio_ebs_alert.wav (canonical 853+960 Hz dual-tone attention signal + undulating air raid siren)
  - assets/audio/radio/radio_dead_hand_pulse.wav (automated ICBM telemetry pulse loop)
  - assets/audio/radio/radio_distress_beacon.wav (emergency locator transmitter beacon loop)
- Cassette Audio Log Transport:
  - assets/audio/sfx/sfx_tape_rewind.wav (high-speed spool motor whir & tape head flutter)
  - assets/audio/sfx/sfx_tape_stop.wav (mechanical leaf spring stop clack & brake felt thud)
- Tactile Inventory & Loot Category Foley:
  - assets/audio/sfx/sfx_item_ammo_box.wav (metal ammo box lid clank & brass cartridge rattle)
  - assets/audio/sfx/sfx_item_med_vial.wav (pharmaceutical glass ampoule clink & liquid slosh)
  - assets/audio/sfx/sfx_item_ration_pack.wav (heavy MRE foil packet crinkle & canvas rustle)

Uses standard Python library (wave, math, struct, random, hashlib) with zero external dependencies.
Outputs 16-bit PCM WAV, 44100 Hz, mono with valid Godot .import sidecars.
"""

import hashlib
import math
import os
import random
import struct
import wave

SAMPLE_RATE = 44100
ROOT_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
SFX_DIR = os.path.join(ROOT_DIR, "assets", "audio", "sfx")
RADIO_DIR = os.path.join(ROOT_DIR, "assets", "audio", "radio")

import sys
import pathlib
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
from audio_pipeline import AudioExporter, PRESET_SFX, PRESET_LOOP, PRESET_RADIO

def ensure_dir(path):
    os.makedirs(path, exist_ok=True)

def save_wav_with_import(filepath, samples, loop_mode=0, sample_rate=SAMPLE_RATE):
    """Save float samples using shared AudioPipeline with safe headroom. Never writes .import."""
    ensure_dir(os.path.dirname(filepath))
    rel_path = os.path.relpath(filepath, ROOT_DIR).replace('\\', '/')
    if "radio" in rel_path:
        preset = PRESET_RADIO
    elif loop_mode == 1:
        preset = PRESET_LOOP
    else:
        preset = PRESET_SFX
    sha256 = AudioExporter.export_wav(pathlib.Path(filepath), samples, preset)
    print(f"  Written: {rel_path} ({len(samples)/sample_rate:.2f}s, loop={loop_mode}) [sha256: {sha256[:8]}]")

def lowpass(samples, cutoff_hz, sample_rate=SAMPLE_RATE):
    rc = 1.0 / (2.0 * math.pi * cutoff_hz)
    dt = 1.0 / sample_rate
    alpha = dt / (rc + dt)
    out = [0.0] * len(samples)
    if not samples:
        return out
    out[0] = samples[0] * alpha
    for i in range(1, len(samples)):
        out[i] = out[i-1] + alpha * (samples[i] - out[i-1])
    return out

def highpass(samples, cutoff_hz, sample_rate=SAMPLE_RATE):
    rc = 1.0 / (2.0 * math.pi * cutoff_hz)
    dt = 1.0 / sample_rate
    alpha = rc / (rc + dt)
    out = [0.0] * len(samples)
    if not samples:
        return out
    out[0] = samples[0]
    for i in range(1, len(samples)):
        out[i] = alpha * (out[i-1] + samples[i] - samples[i-1])
    return out

def bandpass(samples, low_cutoff, high_cutoff, sample_rate=SAMPLE_RATE):
    return lowpass(highpass(samples, low_cutoff, sample_rate), high_cutoff, sample_rate)

# ── Synthesizers ──────────────────────────────────────────────────────────

def gen_numbers_station():
    """Chime interval motif followed by synthesized robotic NATO digits through shortwave flutter."""
    dur = 4.2
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    sw_static = bandpass(raw_noise, 400, 2600)

    # 1. Triplet Chime Motif (A4 440 Hz, C#5 554 Hz, E5 659 Hz)
    chimes = [(0.0, 440.0), (0.28, 554.37), (0.56, 659.25)]
    for start_t, freq in chimes:
        s_idx = int(start_t * SAMPLE_RATE)
        for i in range(int(0.35 * SAMPLE_RATE)):
            idx = s_idx + i
            if idx < n:
                t = i / SAMPLE_RATE
                env = math.exp(-t * 12.0)
                sine = math.sin(2.0 * math.pi * freq * t) + 0.3 * math.sin(2.0 * math.pi * freq * 2.0 * t)
                samples[idx] += sine * env * 0.7

    # 2. Formant-modeled numbers ("7", "4", "0", "9")
    # Digits spaced across 1.1s to 3.8s
    # Simple dual-formant phonemes
    digits = [
        (1.10, 0.45, 180.0, [(400.0, 1800.0), (300.0, 1200.0)]),  # "seven"
        (1.80, 0.40, 175.0, [(450.0, 800.0), (600.0, 1000.0)]),    # "four"
        (2.45, 0.50, 170.0, [(300.0, 2200.0), (450.0, 900.0)]),    # "zero"
        (3.15, 0.48, 172.0, [(650.0, 1900.0), (300.0, 1100.0)]),   # "niner"
    ]

    for start_t, word_dur, f0, formants in digits:
        s_idx = int(start_t * SAMPLE_RATE)
        w_len = int(word_dur * SAMPLE_RATE)
        for i in range(w_len):
            idx = s_idx + i
            if idx < n:
                t = i / SAMPLE_RATE
                env = math.sin(math.pi * (t / word_dur))
                # Buzz excitation
                buzz = math.sin(2.0 * math.pi * f0 * t) + 0.5 * math.sin(2.0 * math.pi * f0 * 2.0 * t)
                # Apply vowel formants
                f1, f2 = formants[0] if (t / word_dur) < 0.5 else formants[1]
                vowel = (math.sin(2.0 * math.pi * f1 * t) * 0.6 +
                         math.sin(2.0 * math.pi * f2 * t) * 0.4) * buzz
                samples[idx] += vowel * env * 0.85

    # 3. Shortwave ionospheric modulation (QSB flutter) + carrier hiss
    for i in range(n):
        t = i / SAMPLE_RATE
        # Ionospheric fading (0.4 Hz sine flutter)
        qsb = 0.75 + 0.25 * math.sin(2.0 * math.pi * 0.45 * t)
        # Carrier heterodine whistle (1,150 Hz faint)
        carrier = math.sin(2.0 * math.pi * 1150.0 * t) * 0.04
        samples[i] = (samples[i] * qsb) + carrier + sw_static[i] * 0.15

    return samples

def gen_ebs_alert():
    """853 Hz + 960 Hz canonical attention signal + undulating air raid siren."""
    dur = 3.2
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n

    # 1. EBS Attention Signal (0..1.4s): 853 Hz + 960 Hz sines
    t_ebs_end = int(1.4 * SAMPLE_RATE)
    for i in range(t_ebs_end):
        t = i / SAMPLE_RATE
        env = min(1.0, t * 25.0) # smooth onset
        tone = (math.sin(2.0 * math.pi * 853.0 * t) + math.sin(2.0 * math.pi * 960.0 * t)) * 0.5
        samples[i] = tone * env * 0.8

    # 2. Undulating Air-Raid Civil Evacuation Siren (1.4s..3.2s)
    phase = 0.0
    for j in range(t_ebs_end, n):
        t_siren = (j - t_ebs_end) / SAMPLE_RATE
        # Siren frequency sweeps between 450 Hz and 780 Hz
        freq = 615.0 + 165.0 * math.sin(2.0 * math.pi * 0.85 * t_siren)
        phase += 2.0 * math.pi * freq / SAMPLE_RATE
        env_siren = min(1.0, t_siren * 8.0) * math.exp(-t_siren * 0.2)
        siren = math.sin(phase) + 0.3 * math.sin(phase * 2.0)
        samples[j] = siren * env_siren * 0.75

    return samples

def gen_dead_hand_pulse():
    """Automated ICBM telemetry telemetry ping with sub-carrier hum."""
    dur = 2.0
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n

    # Pulse 1 at 0.1s, Pulse 2 at 1.0s
    for p_t in [0.1, 1.0]:
        s_idx = int(p_t * SAMPLE_RATE)
        for i in range(int(0.28 * SAMPLE_RATE)):
            idx = s_idx + i
            if idx < n:
                t = i / SAMPLE_RATE
                env = math.exp(-t * 16.0)
                ping = math.sin(2.0 * math.pi * 680.0 * t) + 0.4 * math.sin(2.0 * math.pi * 1360.0 * t)
                samples[idx] += ping * env * 0.8

    # Background electrical sub-carrier hum (60 Hz + 120 Hz)
    for k in range(n):
        t = k / SAMPLE_RATE
        hum = math.sin(2.0 * math.pi * 60.0 * t) * 0.2 + math.sin(2.0 * math.pi * 120.0 * t) * 0.1
        samples[k] += hum * 0.35

    return samples

def gen_distress_beacon():
    """Emergency locator transmitter beacon pulse loop."""
    dur = 1.8
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n

    # Double chirp pulse at 0.15s and 0.45s
    for p_t in [0.15, 0.45]:
        s_idx = int(p_t * SAMPLE_RATE)
        for i in range(int(0.12 * SAMPLE_RATE)):
            idx = s_idx + i
            if idx < n:
                t = i / SAMPLE_RATE
                freq = 1350.0 - 400.0 * (t / 0.12) # downward chirp
                env = math.sin(math.pi * (t / 0.12))
                chirp = math.sin(2.0 * math.pi * freq * t)
                samples[idx] += chirp * env * 0.85

    return samples

def gen_tape_rewind():
    """High-speed cassette transport rewind whir with accelerating spool whine."""
    dur = 1.2
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    tape_flutter = bandpass(raw_noise, 800, 3500)

    phase = 0.0
    for i in range(n):
        t = i / SAMPLE_RATE
        # Motor speed ramping up from 320 Hz to 1150 Hz
        motor_freq = 320.0 + 830.0 * (t / dur)**0.7
        phase += 2.0 * math.pi * motor_freq / SAMPLE_RATE
        env = math.sin(math.pi * (t / dur))
        motor = math.sin(phase) * 0.6 + math.sin(phase * 2.0) * 0.25
        samples[i] = (motor + tape_flutter[i] * 0.35) * env

    return samples

def gen_tape_stop():
    """Mechanical cassette leaf-spring stop clack & felt brake pad thud."""
    dur = 0.25
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n

    for i in range(n):
        t = i / SAMPLE_RATE
        env_clack = math.exp(-t * 90.0)
        env_thud = math.exp(-t * 35.0)
        clack = (math.sin(2.0 * math.pi * 2900.0 * t) * 0.7 +
                 math.sin(2.0 * math.pi * 4800.0 * t) * 0.4)
        thud = math.sin(2.0 * math.pi * 140.0 * t) * 0.8
        samples[i] = clack * env_clack + thud * env_thud

    return samples

def gen_item_ammo_box():
    """Metal ammunition tin clank and rattling cartridges."""
    dur = 0.35
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    brass_rattle = bandpass(raw_noise, 2200, 6800)

    for i in range(n):
        t = i / SAMPLE_RATE
        env_tin = math.exp(-t * 22.0)
        env_rattle = math.exp(-t * 15.0)
        tin_clank = (math.sin(2.0 * math.pi * 850.0 * t) * 0.7 +
                     math.sin(2.0 * math.pi * 1750.0 * t) * 0.4)
        samples[i] = tin_clank * env_tin + brass_rattle[i] * env_rattle * 0.75

    return samples

def gen_item_med_vial():
    """Pharmaceutical glass ampoule clink and liquid droplet slosh."""
    dur = 0.30
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n

    for i in range(n):
        t = i / SAMPLE_RATE
        env_glass = math.exp(-t * 25.0)
        env_drop = math.sin(math.pi * min(1.0, t / 0.15)) * math.exp(-t * 18.0)
        glass = (math.sin(2.0 * math.pi * 3200.0 * t) * 0.7 +
                 math.sin(2.0 * math.pi * 5100.0 * t) * 0.35)
        slosh = math.sin(2.0 * math.pi * 420.0 * t) * 0.4
        samples[i] = glass * env_glass + slosh * env_drop

    return samples

def gen_item_ration_pack():
    """Thick MRE foil packet crinkling and canvas ration rustle."""
    dur = 0.45
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    foil = bandpass(raw_noise, 1500, 5800)
    canvas = bandpass(raw_noise, 300, 1400)

    for i in range(n):
        t = i / SAMPLE_RATE
        env = math.sin(math.pi * (t / dur))
        micro_flutter = 0.7 + 0.3 * math.sin(2.0 * math.pi * 28.0 * t)
        samples[i] = (foil[i] * 0.7 + canvas[i] * 0.5) * env * micro_flutter

    return samples

def main():
    random.seed(2026_05)
    print("Synthesizing Phase 5 Audio Assets...")

    # Radio & Broadcast Assets
    radio_assets = [
        ("radio_numbers_station.wav", gen_numbers_station, 1),
        ("radio_ebs_alert.wav", gen_ebs_alert, 0),
        ("radio_dead_hand_pulse.wav", gen_dead_hand_pulse, 1),
        ("radio_distress_beacon.wav", gen_distress_beacon, 1),
    ]

    for filename, fn, loop in radio_assets:
        samples = fn()
        path = os.path.join(RADIO_DIR, filename)
        save_wav_with_import(path, samples, loop_mode=loop)

    # SFX & Foley Assets
    sfx_assets = [
        ("sfx_tape_rewind.wav", gen_tape_rewind, 0),
        ("sfx_tape_stop.wav", gen_tape_stop, 0),
        ("sfx_item_ammo_box.wav", gen_item_ammo_box, 0),
        ("sfx_item_med_vial.wav", gen_item_med_vial, 0),
        ("sfx_item_ration_pack.wav", gen_item_ration_pack, 0),
    ]

    for filename, fn, loop in sfx_assets:
        samples = fn()
        path = os.path.join(SFX_DIR, filename)
        save_wav_with_import(path, samples, loop_mode=loop)

    print("\nPhase 5 audio synthesis complete.")

if __name__ == "__main__":
    main()
