#!/usr/bin/env python3
"""
tools/generate_tactile_ui.py

Synthesizes tactile, analog, retro-futuristic UI sound effects for ASHFALL.
Uses standard Python library (wave, struct, math, random) with zero external dependencies.
Outputs 16-bit PCM WAV, 44100 Hz, mono to assets/audio/ui/.

Includes:
- ui_switch_toggle.wav (heavy industrial metal toggle switch)
- ui_rotary_click.wav (stepped mechanical dial detent)
- ui_crt_power_on.wav (CRT monitor degauss coil pulse + flyback hum)
- ui_paper_rustle.wav (tactile dossier/paper handling)
- ui_stamp_heavy.wav (bureaucratic rubber ink-stamp thud)
- ui_drawer_slide.wav (metal cabinet drawer slide and latch)
- ui_click_01..04.wav (multi-take mechanical keycap click variations)
"""

import math
import os
import random
import struct
import wave

SAMPLE_RATE = 44100
OUTPUT_DIR = os.path.join(os.path.dirname(os.path.dirname(os.path.abspath(__file__))), "assets", "audio", "ui")

import sys
import pathlib
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
from audio_pipeline import AudioExporter, PRESET_UI

def ensure_dir(path):
    os.makedirs(path, exist_ok=True)

def save_wav(filename, samples, sample_rate=SAMPLE_RATE):
    """Save float samples using shared AudioPipeline with UI preset. Never writes .import."""
    ensure_dir(os.path.dirname(filename))
    sha256 = AudioExporter.export_wav(pathlib.Path(filename), samples, PRESET_UI)
    base = os.path.basename(filename)
    print(f"  Written: {base} ({len(samples)/sample_rate:.3f}s) [sha256: {sha256[:8]}]")

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

def gen_click_variant(freq_center, decay_ms, noise_mix=0.3):
    dur = 0.045
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    filtered_noise = bandpass(raw_noise, freq_center * 0.7, freq_center * 1.5)

    decay_rate = 4.0 / (decay_ms / 1000.0)
    for i in range(n):
        t = i / SAMPLE_RATE
        env = math.exp(-t * decay_rate)
        tone = math.sin(2.0 * math.pi * freq_center * t) * 0.7
        thump = math.sin(2.0 * math.pi * 180.0 * t) * math.exp(-t * 80.0) * 0.4
        samples[i] = ((1.0 - noise_mix) * tone + noise_mix * filtered_noise[i] + thump) * env * 0.75
    return samples

def gen_switch_toggle():
    dur = 0.09
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n

    # Phase 1: lever scrape at t=0..12ms
    n_scrape = int(0.012 * SAMPLE_RATE)
    raw_scrape = [random.uniform(-1.0, 1.0) * (1.0 - (i / n_scrape)) for i in range(n_scrape)]
    filtered_scrape = bandpass(raw_scrape, 1800, 5000)
    for i in range(n_scrape):
        samples[i] += filtered_scrape[i] * 0.4

    # Phase 2: heavy mechanical spring latch impact at t=18ms
    t_latch = int(0.018 * SAMPLE_RATE)
    raw_snap = [random.uniform(-1.0, 1.0) for _ in range(n - t_latch)]
    filtered_snap = bandpass(raw_snap, 1200, 4500)

    for i in range(t_latch, n):
        t_sub = (i - t_latch) / SAMPLE_RATE
        env = math.exp(-t_sub * 70.0)
        tone = math.sin(2.0 * math.pi * 420.0 * t_sub) * 0.6
        snap = filtered_snap[i - t_latch] * math.exp(-t_sub * 250.0) * 0.5
        body = math.sin(2.0 * math.pi * 220.0 * t_sub) * math.exp(-t_sub * 50.0) * 0.5
        samples[i] += (tone + snap + body) * env * 0.82
    return samples

def gen_rotary_click():
    dur = 0.05
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    filtered_noise = bandpass(raw_noise, 1000, 3200)

    for i in range(n):
        t = i / SAMPLE_RATE
        env = math.exp(-t * 95.0)
        tone = math.sin(2.0 * math.pi * 1450.0 * t) * 0.6 + math.sin(2.0 * math.pi * 880.0 * t) * 0.3
        samples[i] = (tone + filtered_noise[i] * 0.4) * env * 0.78
    return samples

def gen_crt_power_on():
    dur = 0.55
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n

    # 1. Degauss coil chirp: 60Hz -> 180Hz over 80ms
    n_chirp = int(0.08 * SAMPLE_RATE)
    chirp_phase = 0.0
    for i in range(n):
        t = i / SAMPLE_RATE
        if i < n_chirp:
            freq = 60.0 + (120.0 * (i / n_chirp))
            chirp_phase += 2.0 * math.pi * freq / SAMPLE_RATE
            amp = 1.0 - (0.95 * (i / n_chirp))
            samples[i] += math.sin(chirp_phase) * amp * 0.75
        elif t < 0.20:
            t_tail = t - 0.08
            samples[i] += math.sin(2.0 * math.pi * 120.0 * t_tail) * math.exp(-t_tail * 25.0) * 0.15

        # 2. Flyback transformer capacitor high whine (12.5 kHz)
        flyback = math.sin(2.0 * math.pi * 12500.0 * t) * math.exp(-t * 5.0) * 0.12
        samples[i] += flyback

    # 3. Relay switch click at start (8ms)
    n_click = int(0.008 * SAMPLE_RATE)
    raw_click = [random.uniform(-1.0, 1.0) * (1.0 - (i / n_click)) for i in range(n_click)]
    filtered_click = bandpass(raw_click, 1500, 4000)
    for i in range(n_click):
        samples[i] += filtered_click[i] * 0.4
    return samples

def gen_paper_rustle():
    dur = 0.28
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    filtered_noise = bandpass(raw_noise, 650, 3200)

    for i in range(n):
        t = i / SAMPLE_RATE
        env1 = math.exp(-((t - 0.06) / 0.04) ** 2) * 0.8
        env2 = math.exp(-((t - 0.16) / 0.06) ** 2) * 0.6
        env = env1 + env2
        flutter = math.sin(2.0 * math.pi * 95.0 * t) * env * 0.2
        samples[i] = (filtered_noise[i] * env + flutter) * 0.65
    return samples

def gen_stamp_heavy():
    dur = 0.20
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    raw_noise = [random.uniform(-1.0, 1.0) for _ in range(n)]
    filtered_noise = bandpass(raw_noise, 800, 2400)

    for i in range(n):
        t = i / SAMPLE_RATE
        wood_thud = math.sin(2.0 * math.pi * 115.0 * t) * math.exp(-t * 40.0) * 0.8
        wood_sub = math.sin(2.0 * math.pi * 65.0 * t) * math.exp(-t * 25.0) * 0.5
        ink_noise = filtered_noise[i] * math.exp(-t * 120.0) * 0.4
        samples[i] = (wood_thud + wood_sub + ink_noise) * 0.85
    return samples

def gen_drawer_slide():
    dur = 0.26
    n = int(dur * SAMPLE_RATE)
    samples = [0.0] * n
    scrape_len = int(0.16 * SAMPLE_RATE)
    raw_scrape = [random.uniform(-1.0, 1.0) * (0.25 + 0.30 * (i / scrape_len)) for i in range(scrape_len)]
    filtered_scrape = bandpass(raw_scrape, 850, 2800)
    for i in range(scrape_len):
        samples[i] += filtered_scrape[i] * 0.45

    latch_start = scrape_len
    raw_snap = [random.uniform(-1.0, 1.0) for _ in range(n - latch_start)]
    filtered_snap = bandpass(raw_snap, 1500, 5000)
    for i in range(latch_start, n):
        t_latch = (i - latch_start) / SAMPLE_RATE
        latch = (math.sin(2.0 * math.pi * 320.0 * t_latch) * 0.7 + math.sin(2.0 * math.pi * 780.0 * t_latch) * 0.4) * math.exp(-t_latch * 60.0)
        snap = filtered_snap[i - latch_start] * math.exp(-t_latch * 200.0) * 0.5
        samples[i] += (latch + snap) * 0.75
    return samples

def main():
    random.seed(1984)
    ensure_dir(OUTPUT_DIR)
    print("Generating Tactile UI Sound Set (Zero-dependency PCM WAV)...")

    # 1. Multi-take click variations
    click_variations = [
        ("ui_click_01.wav", 2400, 18, 0.35),
        ("ui_click_02.wav", 2750, 16, 0.30),
        ("ui_click_03.wav", 2200, 22, 0.40),
        ("ui_click_04.wav", 3100, 14, 0.28),
    ]
    for filename, freq, decay, mix in click_variations:
        samples = gen_click_variant(freq, decay, mix)
        save_wav(os.path.join(OUTPUT_DIR, filename), samples)

    # 2. Tactile Foley instruments
    tactile_generators = [
        ("ui_switch_toggle.wav", gen_switch_toggle),
        ("ui_rotary_click.wav", gen_rotary_click),
        ("ui_crt_power_on.wav", gen_crt_power_on),
        ("ui_paper_rustle.wav", gen_paper_rustle),
        ("ui_stamp_heavy.wav", gen_stamp_heavy),
        ("ui_drawer_slide.wav", gen_drawer_slide),
    ]
    for filename, fn in tactile_generators:
        samples = fn()
        save_wav(os.path.join(OUTPUT_DIR, filename), samples)

    print("\nTactile UI synthesis complete.")

if __name__ == "__main__":
    main()
