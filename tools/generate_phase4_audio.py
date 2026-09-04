#!/usr/bin/env python3
"""
tools/generate_phase4_audio.py

Synthesizes Phase 4 Audio Assets for ASHFALL:
- Vehicle Audio & Logistics:
  - sfx_vehicle_engine_dirtbike.wav (high-RPM 2-stroke motorcycle buzz loop)
  - sfx_vehicle_engine_truck.wav (heavy turbo diesel rumble & spool loop)
  - sfx_vehicle_refuel.wav (jerrycan clank, fuel glug & cap click)
  - sfx_vehicle_repair.wav (socket ratchet clicks & chassis hammer strike)
- Tactical Ballistics & Cover Impact Foley:
  - sfx_impact_wood_splinter.wav (bullet shattering dry timber / splinter crack)
  - sfx_impact_concrete_crack.wav (lead slug pulverizing concrete rubble & gravel)
  - sfx_impact_metal_ricochet.wav (sheet metal deflection clang & whiz)
  - sfx_weapon_rebar_spear_thud.wav (heavy rebar spear impact & penetration thud)
  - sfx_weapon_molotov_burst.wav (glass shatter & roaring kerosene fireball)
- Extreme Dosimetry:
  - sfx_geiger_intense_crackling.wav (dense blistering avalanche radiation clicks loop)

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
    preset = PRESET_LOOP if loop_mode == 1 else PRESET_SFX
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

def gen_dirtbike_engine():
    dur = 2.0
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    chain_noise = bandpass(raw_noise, 2200, 5500)

    for i in range(n):
        t = i / SAMPLE_RATE
        # 2-stroke motorcycle high-RPM engine (145 Hz combustion fundamental)
        combust = (math.sin(2.0 * math.pi * 145.0 * t) * 0.5 +
                   math.sin(2.0 * math.pi * 290.0 * t) * 0.35 +
                   math.sin(2.0 * math.pi * 435.0 * t) * 0.2)
        # Expansion chamber pipe rasp
        rasp = (math.sin(2.0 * math.pi * 870.0 * t) * 0.15 *
                (0.8 + 0.2 * math.sin(2.0 * math.pi * 22.0 * t)))
        samples[i] = combust + rasp + chain_noise[i] * 0.22
    return samples

def gen_truck_engine():
    dur = 2.4
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    turbo_air = bandpass(raw_noise, 1800, 6000)

    for i in range(n):
        t = i / SAMPLE_RATE
        # Heavy inline-6 diesel strokes (38 Hz + 76 Hz)
        throb = (math.sin(2.0 * math.pi * 38.0 * t) * 0.6 +
                 math.sin(2.0 * math.pi * 76.0 * t) * 0.35 +
                 math.sin(2.0 * math.pi * 114.0 * t) * 0.2)
        # Turbocharger high-frequency whine (2,650 Hz)
        turbo = math.sin(2.0 * math.pi * 2650.0 * t) * 0.12
        samples[i] = throb + turbo + turbo_air[i] * 0.18
    return samples

def gen_vehicle_refuel():
    dur = 1.4
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    fluid_noise = bandpass(raw_noise, 400, 1800)

    # 1. Jerrycan metal neck insert clank (0..0.15s)
    for i in range(int(0.15 * SAMPLE_RATE)):
        t = i / SAMPLE_RATE
        env = math.exp(-t * 50.0)
        clank = (math.sin(2.0 * math.pi * 1100.0 * t) * 0.7 +
                 math.sin(2.0 * math.pi * 2400.0 * t) * 0.4)
        samples[i] += clank * env

    # 2. Resonant fluid glugs (at 0.25s, 0.60s, 0.95s)
    for glug_t in [0.25, 0.60, 0.95]:
        start = int(glug_t * SAMPLE_RATE)
        for j in range(int(0.18 * SAMPLE_RATE)):
            idx = start + j
            if idx < n:
                t = j / SAMPLE_RATE
                env = math.sin(math.pi * (t / 0.18))
                glug = (math.sin(2.0 * math.pi * (280.0 + 90.0 * (t / 0.18)) * t) * 0.75 +
                        fluid_noise[idx] * 0.35)
                samples[idx] += glug * env * 0.8

    # 3. Ratchet cap twist click at end (1.15s..1.35s)
    t_cap = int(1.15 * SAMPLE_RATE)
    for k in range(t_cap, n):
        t_sub = (k - t_cap) / SAMPLE_RATE
        env_cap = math.exp(-t_sub * 60.0)
        click = math.sin(2.0 * math.pi * 3200.0 * t_sub) * 0.6
        samples[k] += click * env_cap
    return samples

def gen_vehicle_repair():
    dur = 1.2
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    click_noise = bandpass(raw_noise, 1800, 6500)

    # 1. Rapid socket ratchet clicks (0..0.6s)
    for click_idx in range(7):
        start = int((0.08 * click_idx) * SAMPLE_RATE)
        for i in range(int(0.04 * SAMPLE_RATE)):
            idx = start + i
            if idx < n:
                t = i / SAMPLE_RATE
                env = math.exp(-t * 90.0)
                click = math.sin(2.0 * math.pi * 2800.0 * t) * 0.7 + click_noise[idx] * 0.5
                samples[idx] += click * env

    # 2. Heavy steel hammer strike on chassis (0.75s..1.2s)
    t_hammer = int(0.75 * SAMPLE_RATE)
    for j in range(t_hammer, n):
        t_sub = (j - t_hammer) / SAMPLE_RATE
        env_h = math.exp(-t_sub * 18.0)
        thud = (math.sin(2.0 * math.pi * 95.0 * t_sub) * 0.8 +
                math.sin(2.0 * math.pi * 1450.0 * t_sub) * 0.5)
        samples[j] += thud * env_h
    return samples

def gen_impact_wood():
    dur = 0.35
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    splinter = bandpass(raw_noise, 1100, 5200)

    for i in range(n):
        t = i / SAMPLE_RATE
        env_crack = math.exp(-t * 45.0)
        env_wood = math.exp(-t * 18.0)
        thump = math.sin(2.0 * math.pi * 160.0 * t) * 0.7
        snap = math.sin(2.0 * math.pi * 1850.0 * t) * 0.5
        samples[i] = (thump + snap) * env_crack + splinter[i] * env_wood * 0.85
    return samples

def gen_impact_concrete():
    dur = 0.40
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    grit = bandpass(raw_noise, 900, 3800)

    for i in range(n):
        t = i / SAMPLE_RATE
        env_pop = math.exp(-t * 50.0)
        env_grit = math.exp(-t * 12.0)
        blast = math.sin(2.0 * math.pi * 88.0 * t) * 0.9
        samples[i] = blast * env_pop + grit[i] * env_grit * 0.75
    return samples

def gen_impact_metal():
    dur = 0.55
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    for i in range(n):
        t = i / SAMPLE_RATE
        env_clang = math.exp(-t * 14.0)
        # Resonant steel plate ringing (2850 Hz + 4400 Hz harmonic)
        clang = (math.sin(2.0 * math.pi * 2850.0 * t) * 0.7 +
                 math.sin(2.0 * math.pi * 4400.0 * t) * 0.4)
        # Rising whiz chirp
        whiz_freq = 1200.0 + 3200.0 * (t / dur)
        whiz = math.sin(2.0 * math.pi * whiz_freq * t) * 0.25 * math.exp(-t * 22.0)
        samples[i] = clang * env_clang + whiz
    return samples

def gen_rebar_spear():
    dur = 0.60
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    flesh_crunch = bandpass(raw_noise, 400, 2200)

    for i in range(n):
        t = i / SAMPLE_RATE
        env_thud = math.exp(-t * 25.0)
        sub_thud = math.sin(2.0 * math.pi * 62.0 * t) * 0.9
        pipe_vibe = math.sin(2.0 * math.pi * 480.0 * t) * 0.4 * math.exp(-t * 35.0)
        samples[i] = (sub_thud + pipe_vibe + flesh_crunch[i] * 0.6) * env_thud
    return samples

def gen_molotov_burst():
    dur = 1.6
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    fire_roar = bandpass(raw_noise, 220, 1600)
    glass_tink = bandpass(raw_noise, 2400, 8000)

    # 1. Glass shatter (0..0.25s)
    for i in range(int(0.25 * SAMPLE_RATE)):
        t = i / SAMPLE_RATE
        env = math.exp(-t * 30.0)
        tink = (math.sin(2.0 * math.pi * 3100.0 * t) * 0.6 +
                math.sin(2.0 * math.pi * 5200.0 * t) * 0.4)
        samples[i] += (tink + glass_tink[i] * 0.7) * env

    # 2. Explosive fire whoosh & roar (0.05s..1.6s)
    t_fire = int(0.05 * SAMPLE_RATE)
    for j in range(t_fire, n):
        t_sub = (j - t_fire) / SAMPLE_RATE
        env_fire = math.sin(math.pi * (t_sub / 1.55)) * math.exp(-t_sub * 1.8)
        sub_whump = math.sin(2.0 * math.pi * 75.0 * t_sub) * math.exp(-t_sub * 8.0) * 0.7
        samples[j] += (sub_whump + fire_roar[j] * 0.85) * env_fire
    return samples

def gen_geiger_intense():
    dur = 2.5
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    # Generate ~220 high-density ionizing discharge clicks
    rng = random.Random(999)
    click_positions = sorted([rng.uniform(0.0, dur) for _ in range(220)])

    for pos in click_positions:
        start = int(pos * SAMPLE_RATE)
        for i in range(int(0.003 * SAMPLE_RATE)): # 3ms micro-click
            idx = start + i
            if idx < n:
                t = i / SAMPLE_RATE
                env = math.exp(-t * 1200.0)
                click = math.sin(2.0 * math.pi * 3600.0 * t) * env
                samples[idx] += click * 0.65
    return samples

def main():
    random.seed(2026_04)
    print("Synthesizing Phase 4 Audio Assets...")

    assets = [
        # Vehicles & Logistics
        ("sfx_vehicle_engine_dirtbike.wav", gen_dirtbike_engine, 1),
        ("sfx_vehicle_engine_truck.wav", gen_truck_engine, 1),
        ("sfx_vehicle_refuel.wav", gen_vehicle_refuel, 0),
        ("sfx_vehicle_repair.wav", gen_vehicle_repair, 0),
        # Ballistics & Cover Impacts
        ("sfx_impact_wood_splinter.wav", gen_impact_wood, 0),
        ("sfx_impact_concrete_crack.wav", gen_impact_concrete, 0),
        ("sfx_impact_metal_ricochet.wav", gen_impact_metal, 0),
        ("sfx_weapon_rebar_spear_thud.wav", gen_rebar_spear, 0),
        ("sfx_weapon_molotov_burst.wav", gen_molotov_burst, 0),
        # Intense Dosimetry
        ("sfx_geiger_intense_crackling.wav", gen_geiger_intense, 1),
    ]

    for filename, fn, loop in assets:
        samples = fn()
        path = os.path.join(SFX_DIR, filename)
        save_wav_with_import(path, samples, loop_mode=loop)

    print("\nPhase 4 audio synthesis complete.")

if __name__ == "__main__":
    main()
