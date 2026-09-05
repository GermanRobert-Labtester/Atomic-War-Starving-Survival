#!/usr/bin/env python3
"""
tools/generate_phase2_audio.py

Synthesizes Phase 2 Audio Assets for ASHFALL:
- Living Shelter System Activity:
  - sfx_generator_heavy_strain.wav (strained high-load generator throb)
  - sfx_water_filtration_loop.wav (pump hum + filtration flow)
  - sfx_air_recycler_hiss.wav (pneumatic air circulation hiss)
  - sfx_workshop_lathe_hum.wav (machining motor + metallic rasp)
  - sfx_infirmary_monitor_beep.wav (medical telemetry monitor ping)
- Surface Expeditions & Vehicles:
  - sfx_vehicle_engine_diesel.wav (diesel engine + dirt road travel rumble)
  - sfx_vehicle_breakdown_stall.wav (engine sputter + belt snap + steam hiss)
  - amb_expedition_camp_fire.wav (night wind + crackling wood fire embers)
- Psychological Trauma & Stress:
  - sfx_trauma_tinnitus_ring.wav (3.8 kHz ringing tone)
  - sfx_trauma_heartbeat_rapid.wav (135 BPM rapid visceral heartbeat)
  - sfx_trauma_cabin_fever_whisper.wav (eerie bunker pipe draft + phantom whispers)

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
AMB_DIR = os.path.join(ROOT_DIR, "assets", "audio", "ambience")

import sys
import pathlib
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
from audio_pipeline import AudioExporter, PRESET_SFX, PRESET_LOOP, PRESET_AMBIENCE

def ensure_dir(path):
    os.makedirs(path, exist_ok=True)

def save_wav_with_import(filepath, samples, loop_mode=0, sample_rate=SAMPLE_RATE):
    """Save float samples using shared AudioPipeline with safe headroom. Never writes .import."""
    ensure_dir(os.path.dirname(filepath))
    rel_path = os.path.relpath(filepath, ROOT_DIR).replace('\\', '/')
    if "ambience" in rel_path:
        preset = PRESET_AMBIENCE
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

def gen_generator_heavy_strain():
    dur = 2.4
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    f_noise = bandpass(raw_noise, 400, 2200)

    for i in range(n):
        t = i / SAMPLE_RATE
        # Deep straining fundamental at 44 Hz with heavy 2nd and 3rd harmonics
        throb = (math.sin(2.0 * math.pi * 44.0 * t) * 0.7 +
                 math.sin(2.0 * math.pi * 88.0 * t) * 0.5 +
                 math.sin(2.0 * math.pi * 132.0 * t) * 0.3)
        # Heavy piston thuds (8 Hz cycle)
        piston_pulse = (math.sin(2.0 * math.pi * 8.0 * t) + 1.0) * 0.5
        piston_hit = math.pow(piston_pulse, 4.0) * 0.6
        # Metallic engine block rattle
        rattle = f_noise[i] * (0.2 + 0.4 * piston_hit)
        samples[i] = throb + (throb * piston_hit * 0.8) + rattle
    return samples

def gen_water_filtration():
    dur = 3.0
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    fluid_noise = bandpass(raw_noise, 600, 3200)

    for i in range(n):
        t = i / SAMPLE_RATE
        # Continuous 120 Hz induction motor hum + 60 Hz line buzz
        hum = (math.sin(2.0 * math.pi * 120.0 * t) * 0.4 +
               math.sin(2.0 * math.pi * 60.0 * t) * 0.25)
        # Fluid trickle & pump intake surge (period 1.5s)
        surge = math.sin(2.0 * math.pi * (1.0 / 1.5) * t) * 0.5 + 0.5
        trickle = fluid_noise[i] * (0.25 + 0.35 * surge)
        # Periodic pressure relief hiss
        hiss_env = math.exp(-((t % 1.5 - 0.2) / 0.15) ** 2) * 0.3
        hiss = fluid_noise[i] * hiss_env
        samples[i] = hum + trickle + hiss
    return samples

def gen_air_recycler():
    dur = 3.0
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    rushing_air = bandpass(raw_noise, 350, 2600)
    low_duct = lowpass(raw_noise, 180)

    for i in range(n):
        t = i / SAMPLE_RATE
        # Slow atmospheric pressure cycle (period 3.0s)
        cycle = math.sin(2.0 * math.pi * (1.0 / 3.0) * t) * 0.2 + 0.8
        samples[i] = rushing_air[i] * cycle * 0.6 + low_duct[i] * 0.4
    return samples

def gen_workshop_lathe():
    dur = 2.5
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    tool_friction = bandpass(raw_noise, 900, 4800)

    for i in range(n):
        t = i / SAMPLE_RATE
        # High speed rotation drive hum (285 Hz + 570 Hz)
        drive = (math.sin(2.0 * math.pi * 285.0 * t) * 0.5 +
                 math.sin(2.0 * math.pi * 570.0 * t) * 0.25)
        # Shaving rasp modulation
        rasp_mod = math.sin(2.0 * math.pi * 14.0 * t) * 0.3 + 0.7
        rasp = tool_friction[i] * 0.35 * rasp_mod
        samples[i] = drive + rasp
    return samples

def gen_infirmary_beep():
    dur = 0.35
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    for i in range(n):
        t = i / SAMPLE_RATE
        env = math.exp(-t * 18.0)
        # Clean medical monitor tone: 980 Hz + gentle 2nd harmonic
        tone = (math.sin(2.0 * math.pi * 980.0 * t) * 0.8 +
                math.sin(2.0 * math.pi * 1960.0 * t) * 0.2)
        samples[i] = tone * env
    return samples

def gen_vehicle_diesel():
    dur = 2.5
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    tread_noise = lowpass(raw_noise, 650)
    chatter_noise = bandpass(raw_noise, 1200, 3800)

    for i in range(n):
        t = i / SAMPLE_RATE
        # Diesel engine combustion cylinders firing at 52 Hz
        cyl = (math.sin(2.0 * math.pi * 52.0 * t) * 0.6 +
               math.sin(2.0 * math.pi * 104.0 * t) * 0.4 +
               math.sin(2.0 * math.pi * 156.0 * t) * 0.2)
        # Rhythmic compression pulses (13 Hz)
        pulse = math.pow((math.sin(2.0 * math.pi * 13.0 * t) + 1.0) * 0.5, 3.0)
        # Valve rocker clatter
        valve = chatter_noise[i] * 0.15 * pulse
        # Gravel crunch under tire wheels
        gravel = tread_noise[i] * 0.35
        samples[i] = cyl * (0.8 + 0.4 * pulse) + valve + gravel
    return samples

def gen_vehicle_breakdown():
    dur = 1.9
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    steam_noise = bandpass(raw_noise, 1800, 6000)

    # 1. Stuttering engine deceleration (0..0.7s)
    n_sputter = int(0.7 * SAMPLE_RATE)
    phase = 0.0
    for i in range(n_sputter):
        t = i / SAMPLE_RATE
        freq = 52.0 * (1.0 - (t / 0.7) * 0.7)
        phase += 2.0 * math.pi * freq / SAMPLE_RATE
        # Coughing misfire
        misfire = math.sin(2.0 * math.pi * 9.0 * t)
        if misfire < -0.4:
            amp = 0.05
        else:
            amp = 0.6 * (1.0 - t / 0.7)
        samples[i] += math.sin(phase) * amp

    # 2. Catastrophic belt snap / metal seize crunch at t=0.7s
    t_crunch = int(0.7 * SAMPLE_RATE)
    n_crunch = int(0.25 * SAMPLE_RATE)
    raw_crunch = [random.uniform(-1.0, 1.0) for _ in range(n_crunch)]
    filtered_crunch = bandpass(raw_crunch, 800, 4500)
    for j in range(n_crunch):
        idx = t_crunch + j
        if idx < n:
            t_sub = j / SAMPLE_RATE
            env = math.exp(-t_sub * 25.0)
            metal_ping = (math.sin(2.0 * math.pi * 380.0 * t_sub) * 0.7 +
                          math.sin(2.0 * math.pi * 820.0 * t_sub) * 0.5)
            samples[idx] += (metal_ping + filtered_crunch[j] * 0.7) * env * 0.9

    # 3. Pressurized radiator steam hiss (0.8s..1.9s)
    t_steam = int(0.85 * SAMPLE_RATE)
    for k in range(t_steam, n):
        t_sub = (k - t_steam) / SAMPLE_RATE
        env = math.exp(-t_sub * 2.2)
        samples[k] += steam_noise[k] * env * 0.55

    return samples

def gen_camp_fire():
    dur = 3.5
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    wind_sub = lowpass(raw_noise, 160)
    ember_hiss = bandpass(raw_noise, 1500, 5000)

    for i in range(n):
        t = i / SAMPLE_RATE
        # Gentle desert night wind
        wind = wind_sub[i] * (0.35 + 0.15 * math.sin(2.0 * math.pi * 0.3 * t))
        # Constant warm ember bed hiss
        hiss = ember_hiss[i] * 0.18
        samples[i] = wind + hiss

    # Stochastic wood snap sparks (10-15 per loop)
    num_snaps = 14
    for _ in range(num_snaps):
        pos = random.randint(int(0.1 * SAMPLE_RATE), n - int(0.1 * SAMPLE_RATE))
        snap_len = random.randint(int(0.01 * SAMPLE_RATE), int(0.035 * SAMPLE_RATE))
        snap_freq = random.uniform(2200, 4800)
        for s in range(snap_len):
            idx = pos + s
            if idx < n:
                t_snap = s / SAMPLE_RATE
                snap_env = math.exp(-t_snap * 180.0)
                tone = math.sin(2.0 * math.pi * snap_freq * t_snap) * 0.6
                noise_pop = random.uniform(-1.0, 1.0) * 0.5
                samples[idx] += (tone + noise_pop) * snap_env * 0.75
    return samples

def gen_tinnitus_ring():
    dur = 2.5
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    for i in range(n):
        t = i / SAMPLE_RATE
        # Pure 3820 Hz acoustic trauma ring with gentle 1.2 Hz vestibular amplitude sway
        amp_mod = 0.85 + 0.15 * math.sin(2.0 * math.pi * 1.2 * t)
        tone = math.sin(2.0 * math.pi * 3820.0 * t) * 0.75
        harmonic = math.sin(2.0 * math.pi * 7640.0 * t) * 0.1
        samples[i] = (tone + harmonic) * amp_mod
    return samples

def gen_heartbeat_rapid():
    dur = 1.8  # ~133 BPM loop (4 heartbeats in 1.8s, ~0.45s per cycle)
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    cycle_samples = int(0.45 * SAMPLE_RATE)

    for cycle in range(4):
        base_idx = cycle * cycle_samples
        # Lub (first heart sound: 58 Hz deep ventricle contraction)
        lub_len = int(0.12 * SAMPLE_RATE)
        for i in range(lub_len):
            idx = base_idx + i
            if idx < n:
                t = i / SAMPLE_RATE
                env = math.exp(-t * 28.0)
                thump = (math.sin(2.0 * math.pi * 58.0 * t) * 0.8 +
                         math.sin(2.0 * math.pi * 116.0 * t) * 0.3)
                samples[idx] += thump * env * 0.85

        # Dub (second heart sound: 74 Hz aortic valve closure, 110ms offset)
        dub_offset = int(0.11 * SAMPLE_RATE)
        dub_len = int(0.10 * SAMPLE_RATE)
        for i in range(dub_len):
            idx = base_idx + dub_offset + i
            if idx < n:
                t = i / SAMPLE_RATE
                env = math.exp(-t * 32.0)
                thump = (math.sin(2.0 * math.pi * 74.0 * t) * 0.7 +
                         math.sin(2.0 * math.pi * 148.0 * t) * 0.25)
                samples[idx] += thump * env * 0.70
    return samples

def gen_cabin_fever_whisper():
    dur = 3.0
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    sub_creak = lowpass(raw_noise, 120)
    whisper_air = bandpass(raw_noise, 750, 1900)

    for i in range(n):
        t = i / SAMPLE_RATE
        # Deep structural pipe resonance
        pipe = math.sin(2.0 * math.pi * 92.0 * t) * 0.25 * (math.sin(2.0 * math.pi * 0.4 * t) * 0.5 + 0.5)
        # Phantom auditory breathing wave
        breath_env = math.pow(math.sin(2.0 * math.pi * (1.0 / 3.0) * t) * 0.5 + 0.5, 2.0)
        whisper = whisper_air[i] * breath_env * 0.45
        samples[i] = sub_creak[i] * 0.3 + pipe + whisper
    return samples

def main():
    random.seed(2026)
    print("Synthesizing Phase 2 Audio Assets...")

    # 1. Living Shelter System
    shelter_assets = [
        ("sfx_generator_heavy_strain.wav", gen_generator_heavy_strain, 1, SFX_DIR),
        ("sfx_water_filtration_loop.wav", gen_water_filtration, 1, SFX_DIR),
        ("sfx_air_recycler_hiss.wav", gen_air_recycler, 1, SFX_DIR),
        ("sfx_workshop_lathe_hum.wav", gen_workshop_lathe, 1, SFX_DIR),
        ("sfx_infirmary_monitor_beep.wav", gen_infirmary_beep, 0, SFX_DIR),
    ]

    # 2. Surface Expeditions & Vehicles
    expedition_assets = [
        ("sfx_vehicle_engine_diesel.wav", gen_vehicle_diesel, 1, SFX_DIR),
        ("sfx_vehicle_breakdown_stall.wav", gen_vehicle_breakdown, 0, SFX_DIR),
        ("amb_expedition_camp_fire.wav", gen_camp_fire, 1, AMB_DIR),
    ]

    # 3. Psychological Trauma
    trauma_assets = [
        ("sfx_trauma_tinnitus_ring.wav", gen_tinnitus_ring, 0, SFX_DIR),
        ("sfx_trauma_heartbeat_rapid.wav", gen_heartbeat_rapid, 1, SFX_DIR),
        ("sfx_trauma_cabin_fever_whisper.wav", gen_cabin_fever_whisper, 1, SFX_DIR),
    ]

    for filename, fn, loop, out_dir in shelter_assets + expedition_assets + trauma_assets:
        samples = fn()
        path = os.path.join(out_dir, filename)
        save_wav_with_import(path, samples, loop_mode=loop)

    print("\nPhase 2 audio synthesis complete.")

if __name__ == "__main__":
    main()
