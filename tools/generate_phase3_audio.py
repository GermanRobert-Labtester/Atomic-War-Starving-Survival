#!/usr/bin/env python3
"""
tools/generate_phase3_audio.py

Synthesizes Phase 3 Audio Assets for ASHFALL:
- Somatic Flashbacks & Trauma:
  - sfx_flashback_distortion.wav (disorienting pitch-sweep, beating cluster & reverse woosh)
  - sfx_flashback_grounded.wav (warm grounding exhale & harmonic resolution chime)
- Tactical Combat Foley:
  - sfx_weapon_burst_rupture.wav (explosive barrel breach, metal shred & propellant hiss)
  - sfx_weapon_dry_fire_click.wav (hardened firing pin striking empty chamber)
  - sfx_shell_casing_drop_01.wav (brass casing ping & multi-bounce 1)
  - sfx_shell_casing_drop_02.wav (brass casing ping & multi-bounce 2)
  - sfx_combat_last_stand.wav (adrenaline sub-bass thud & rising tension drone)
  - sfx_combat_decon_spray.wav (pressurized aerosol valve & chemical mist hiss)
- Diegetic Audio Logs & Narrative Echoes:
  - sfx_tape_deck_insert.wav (cassette shell slide & mechanical spring latch)
  - sfx_tape_deck_button.wav (heavy mechanical transport key latch)
  - sfx_tape_hiss_loop.wav (vintage magnetic tape background hiss loop)
  - sfx_echo_memory_shimmer.wav (ethereal harmonic memory discovery shimmer)

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

def gen_flashback_distortion():
    dur = 2.2
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    air_noise = bandpass(raw_noise, 400, 1800)

    for i in range(n):
        t = i / SAMPLE_RATE
        # Disorienting sub sweep: 110 Hz down to 38 Hz
        sub_freq = 110.0 - (72.0 * (t / dur))
        sub = math.sin(2.0 * math.pi * sub_freq * t) * 0.7

        # Dissonant beating cluster (310 Hz & 326 Hz)
        cluster = (math.sin(2.0 * math.pi * 310.0 * t) +
                   math.sin(2.0 * math.pi * 326.0 * t)) * 0.35

        # Reverse envelope whoosh swelling in the middle
        env = math.exp(-((t - 1.1) / 0.6) ** 2)
        whoosh = air_noise[i] * env * 0.8
        samples[i] = (sub + cluster) * (0.4 + 0.6 * env) + whoosh
    return samples

def gen_flashback_grounded():
    dur = 1.5
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    breath = bandpass(raw_noise, 320, 850)

    for i in range(n):
        t = i / SAMPLE_RATE
        env_exhale = math.exp(-t * 2.2)
        env_chime = math.exp(-t * 1.8)

        # Calming harmonic resolution fifth (220 Hz + 330 Hz + 440 Hz)
        chord = (math.sin(2.0 * math.pi * 220.0 * t) * 0.5 +
                 math.sin(2.0 * math.pi * 330.0 * t) * 0.35 +
                 math.sin(2.0 * math.pi * 440.0 * t) * 0.2) * env_chime

        # Soft grounding breath
        air = breath[i] * env_exhale * 0.4
        samples[i] = chord + air
    return samples

def gen_weapon_burst_rupture():
    dur = 1.4
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    shred_noise = bandpass(raw_noise, 1200, 4800)
    steam_noise = bandpass(raw_noise, 2200, 7000)

    # 1. Catastrophic metal breach crack (t=0)
    for i in range(int(0.25 * SAMPLE_RATE)):
        t = i / SAMPLE_RATE
        env = math.exp(-t * 35.0)
        pop = math.sin(2.0 * math.pi * 125.0 * t) * 0.9
        shear = (math.sin(2.0 * math.pi * 1850.0 * t) * 0.6 +
                 math.sin(2.0 * math.pi * 3400.0 * t) * 0.4)
        samples[i] += (pop + shear + shred_noise[i] * 0.8) * env

    # 2. Escaping hot gas discharge (0.05s .. 1.4s)
    t_gas = int(0.05 * SAMPLE_RATE)
    for j in range(t_gas, n):
        t_sub = (j - t_gas) / SAMPLE_RATE
        env_gas = math.exp(-t_sub * 4.0)
        samples[j] += steam_noise[j] * env_gas * 0.6
    return samples

def gen_weapon_dry_fire():
    dur = 0.16
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    click_noise = bandpass(raw_noise, 1500, 6000)

    for i in range(n):
        t = i / SAMPLE_RATE
        env = math.exp(-t * 55.0)
        # Sharp metal firing pin strike (2450 Hz + 4900 Hz resonance)
        pin = (math.sin(2.0 * math.pi * 2450.0 * t) * 0.75 +
               math.sin(2.0 * math.pi * 4900.0 * t) * 0.35)
        samples[i] = (pin + click_noise[i] * 0.6) * env
    return samples

def gen_shell_casing(bounce_delays):
    dur = 0.50
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    tink_noise = bandpass(raw_noise, 2800, 7500)

    for bounce_idx, delay_s in enumerate(bounce_delays):
        start = int(delay_s * SAMPLE_RATE)
        amp = 1.0 / (bounce_idx + 1)
        decay = 40.0 + bounce_idx * 15.0
        base_freq = 3200.0 + random.uniform(-150, 150)
        for i in range(int(0.12 * SAMPLE_RATE)):
            idx = start + i
            if idx < n:
                t = i / SAMPLE_RATE
                env = math.exp(-t * decay)
                tink = (math.sin(2.0 * math.pi * base_freq * t) * 0.8 +
                        math.sin(2.0 * math.pi * (base_freq * 1.5) * t) * 0.4)
                samples[idx] += (tink + tink_noise[idx] * 0.4) * env * amp
    return samples

def gen_combat_last_stand():
    dur = 2.0
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    for i in range(n):
        t = i / SAMPLE_RATE
        # Adrenaline surge low thud + rising pitch drone
        drone_freq = 70.0 + 35.0 * (t / dur)
        drone = (math.sin(2.0 * math.pi * drone_freq * t) * 0.6 +
                 math.sin(2.0 * math.pi * (drone_freq * 2.0) * t) * 0.3)
        # Heavy heartbeat throb
        throb_env = math.exp(-((t % 0.6 - 0.1) / 0.08) ** 2) * 0.8
        throb = math.sin(2.0 * math.pi * 50.0 * t) * throb_env
        samples[i] = drone * (0.5 + 0.5 * (t / dur)) + throb
    return samples

def gen_combat_decon():
    dur = 1.2
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    spray_noise = highpass(raw_noise, 1800)

    for i in range(n):
        t = i / SAMPLE_RATE
        # Pneumatic trigger click (0..0.05s)
        click = 0.0
        if t < 0.05:
            click = math.sin(2.0 * math.pi * 1400.0 * t) * math.exp(-t * 80.0) * 0.5
        # Pressurized aerosol mist burst
        spray_env = math.exp(-t * 2.8) * 0.85
        samples[i] = click + spray_noise[i] * spray_env
    return samples

def gen_tape_deck_insert():
    dur = 0.45
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    fric_noise = bandpass(raw_noise, 1200, 5000)

    # 1. Cassette shell sliding into plastic slot (0..0.2s)
    for i in range(int(0.2 * SAMPLE_RATE)):
        t = i / SAMPLE_RATE
        samples[i] = fric_noise[i] * 0.35 * (t / 0.2)

    # 2. Dual mechanical spring latch snap (0.2s..0.45s)
    t_latch = int(0.2 * SAMPLE_RATE)
    for j in range(t_latch, n):
        t_sub = (j - t_latch) / SAMPLE_RATE
        env = math.exp(-t_sub * 40.0)
        snap = (math.sin(2.0 * math.pi * 650.0 * t_sub) * 0.7 +
                math.sin(2.0 * math.pi * 1950.0 * t_sub) * 0.4)
        samples[j] += (snap + fric_noise[j] * 0.5) * env
    return samples

def gen_tape_deck_button():
    dur = 0.25
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    click_noise = bandpass(raw_noise, 800, 3500)

    for i in range(n):
        t = i / SAMPLE_RATE
        env = math.exp(-t * 45.0)
        thunk = (math.sin(2.0 * math.pi * 320.0 * t) * 0.8 +
                 math.sin(2.0 * math.pi * 960.0 * t) * 0.4)
        samples[i] = (thunk + click_noise[i] * 0.6) * env
    return samples

def gen_tape_hiss():
    dur = 3.0
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    hiss = bandpass(raw_noise, 300, 6800)

    for i in range(n):
        t = i / SAMPLE_RATE
        # Vintage tape wow/flutter amplitude fluctuation
        flutter = 0.90 + 0.10 * math.sin(2.0 * math.pi * 0.8 * t)
        samples[i] = hiss[i] * 0.35 * flutter
    return samples

def gen_echo_memory():
    dur = 2.5
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    for i in range(n):
        t = i / SAMPLE_RATE
        env = math.exp(-t * 1.2) * (1.0 - math.exp(-t * 8.0))
        # Ethereal memory chord (520, 780, 1040, 1560 Hz)
        tremolo = 0.85 + 0.15 * math.sin(2.0 * math.pi * 4.5 * t)
        shimmer = (math.sin(2.0 * math.pi * 520.0 * t) * 0.5 +
                   math.sin(2.0 * math.pi * 780.0 * t) * 0.35 +
                   math.sin(2.0 * math.pi * 1040.0 * t) * 0.25 +
                   math.sin(2.0 * math.pi * 1560.0 * t) * 0.15) * tremolo
        samples[i] = shimmer * env
    return samples

def main():
    random.seed(2026_03)
    print("Synthesizing Phase 3 Audio Assets...")

    assets = [
        # Somatic Flashbacks & Trauma
        ("sfx_flashback_distortion.wav", gen_flashback_distortion, 0),
        ("sfx_flashback_grounded.wav", gen_flashback_grounded, 0),
        # Tactical Combat Foley
        ("sfx_weapon_burst_rupture.wav", gen_weapon_burst_rupture, 0),
        ("sfx_weapon_dry_fire_click.wav", gen_weapon_dry_fire, 0),
        ("sfx_shell_casing_drop_01.wav", lambda: gen_shell_casing([0.0, 0.08, 0.15, 0.21]), 0),
        ("sfx_shell_casing_drop_02.wav", lambda: gen_shell_casing([0.0, 0.10, 0.18, 0.24]), 0),
        ("sfx_combat_last_stand.wav", gen_combat_last_stand, 0),
        ("sfx_combat_decon_spray.wav", gen_combat_decon, 0),
        # Diegetic Audio Logs & Echoes
        ("sfx_tape_deck_insert.wav", gen_tape_deck_insert, 0),
        ("sfx_tape_deck_button.wav", gen_tape_deck_button, 0),
        ("sfx_tape_hiss_loop.wav", gen_tape_hiss, 1),
        ("sfx_echo_memory_shimmer.wav", gen_echo_memory, 0),
    ]

    for filename, fn, loop in assets:
        samples = fn()
        path = os.path.join(SFX_DIR, filename)
        save_wav_with_import(path, samples, loop_mode=loop)

    print("\nPhase 3 audio synthesis complete.")

if __name__ == "__main__":
    main()
