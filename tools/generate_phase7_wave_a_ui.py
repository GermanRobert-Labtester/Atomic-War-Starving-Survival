#!/usr/bin/env python3
"""
tools/generate_phase7_wave_a_ui.py — Flagship Audio Wave A: Dedicated UI Cues

Synthesizes dedicated tactile audio assets for previously shared UI cues:
- ui_save_success.wav (save_success)
- ui_invalid_action.wav (ui_invalid_action)
- ui_modal_open.wav (ui_modal_open)
- ui_modal_close.wav (ui_modal_close)
- ui_tab_change.wav (ui_tab_change)
- ui_cancel.wav (ui_cancel)

Uses shared audio_pipeline.py with PRESET_UI (-18.0 LUFS, -1.5 dBFS ceiling).
NEVER writes .import sidecars.
"""

import math
import os
import pathlib
import random
import sys

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent
sys.path.insert(0, str(REPO_ROOT / "tools"))

from audio_pipeline import (
    AudioExporter,
    AudioMeasurer,
    PRESET_UI,
    SeededSynthesizer,
    ReproducibilityEngine,
    DeliveryLedger
)

UI_DIR = REPO_ROOT / "assets" / "audio" / "ui"
SAMPLE_RATE = 44100

def synthesize_save_success(seed=101):
    """Dual-tone resonant mechanical chime + ledger punch solenoid (0.55s)."""
    rng = random.Random(seed)
    n = int(SAMPLE_RATE * 0.55)
    samples = [0.0] * n

    # 1. Mechanical strike transient (first 25ms)
    for i in range(int(SAMPLE_RATE * 0.025)):
        t = i / SAMPLE_RATE
        env = (1.0 - t / 0.025) ** 2
        click = (rng.random() * 2.0 - 1.0) * env * 0.4
        thump = math.sin(2.0 * math.pi * 125.0 * t) * env * 0.5
        samples[i] += click + thump

    # 2. Harmonically consonant resonant bell tones (D5 = 587.33 Hz, A5 = 880.0 Hz, F#6 = 1479.98 Hz)
    for i in range(n):
        t = i / SAMPLE_RATE
        env1 = math.exp(-7.5 * t)
        env2 = math.exp(-8.2 * t)
        env3 = math.exp(-11.0 * t)
        tone1 = math.sin(2.0 * math.pi * 587.33 * t) * env1 * 0.45
        tone2 = math.sin(2.0 * math.pi * 880.00 * t) * env2 * 0.35
        tone3 = math.sin(2.0 * math.pi * 1479.98 * t) * env3 * 0.15
        samples[i] += tone1 + tone2 + tone3

    return [math.tanh(s * 1.2) for s in samples]

def synthesize_invalid_action(seed=102):
    """Dull mechanical lockout latch strike / blocked lever thud (0.16s)."""
    rng = random.Random(seed)
    n = int(SAMPLE_RATE * 0.16)
    samples = [0.0] * n

    lp_state = 0.0
    for i in range(n):
        t = i / SAMPLE_RATE
        env = math.exp(-28.0 * t)
        # Two dissonant low frequencies (115 Hz and 148 Hz) for "blocked/wrong" feel
        f1 = math.sin(2.0 * math.pi * 115.0 * t) * 0.5
        f2 = math.sin(2.0 * math.pi * 148.0 * t) * 0.4
        # Plastic/metal latch click
        noise = (rng.random() * 2.0 - 1.0)
        lp_state += 0.35 * (noise - lp_state)
        click = lp_state * math.exp(-45.0 * t) * 0.45
        samples[i] = (f1 + f2 + click) * env

    return [math.tanh(s * 1.3) for s in samples]

def synthesize_modal_open(seed=103):
    """Heavy mechanical drawer / panel slide-out with soft latch lock (0.24s)."""
    rng = random.Random(seed)
    n = int(SAMPLE_RATE * 0.24)
    samples = [0.0] * n

    # Slide friction noise (0 to 0.18s)
    slide_samples = int(SAMPLE_RATE * 0.18)
    bp1, bp2 = 0.0, 0.0
    for i in range(slide_samples):
        t = i / SAMPLE_RATE
        env = math.sin(math.pi * (t / 0.18)) ** 1.5
        noise = (rng.random() * 2.0 - 1.0)
        # Bandpass around 1.4 kHz
        bp1 += 0.22 * (noise - bp1)
        bp2 += 0.22 * (bp1 - bp2)
        samples[i] += bp2 * env * 0.55

    # Latch dock click (0.18s to end)
    dock_start = int(SAMPLE_RATE * 0.17)
    for i in range(dock_start, n):
        t = (i - dock_start) / SAMPLE_RATE
        env = math.exp(-42.0 * t)
        metal = math.sin(2.0 * math.pi * 2850.0 * t) * 0.35
        thud = math.sin(2.0 * math.pi * 210.0 * t) * 0.55
        samples[i] += (metal + thud) * env * 0.65

    return [math.tanh(s * 1.2) for s in samples]

def synthesize_modal_close(seed=104):
    """Crisp solid panel latch dock and magnetic catch (0.19s)."""
    rng = random.Random(seed)
    n = int(SAMPLE_RATE * 0.19)
    samples = [0.0] * n

    # Impact transient
    for i in range(n):
        t = i / SAMPLE_RATE
        env = math.exp(-32.0 * t)
        body = math.sin(2.0 * math.pi * 165.0 * t) * 0.6
        clack = math.sin(2.0 * math.pi * 1820.0 * t) * 0.35 * math.exp(-60.0 * t)
        noise = (rng.random() * 2.0 - 1.0) * 0.25 * math.exp(-70.0 * t)
        samples[i] = (body + clack + noise) * env

    return [math.tanh(s * 1.2) for s in samples]

def synthesize_tab_change(seed=105):
    """Crisp mechanical rotary notch detent / card divider flip (0.09s)."""
    rng = random.Random(seed)
    n = int(SAMPLE_RATE * 0.09)
    samples = [0.0] * n

    for i in range(n):
        t = i / SAMPLE_RATE
        env = math.exp(-55.0 * t)
        notch = math.sin(2.0 * math.pi * 2240.0 * t) * 0.55
        snap = math.sin(2.0 * math.pi * 560.0 * t) * 0.45
        noise = (rng.random() * 2.0 - 1.0) * 0.3 * math.exp(-80.0 * t)
        samples[i] = (notch + snap + noise) * env

    return [math.tanh(s * 1.2) for s in samples]

def synthesize_cancel(seed=106):
    """Spring toggle return release click (0.12s)."""
    rng = random.Random(seed)
    n = int(SAMPLE_RATE * 0.12)
    samples = [0.0] * n

    for i in range(n):
        t = i / SAMPLE_RATE
        env = math.exp(-38.0 * t)
        # Chirp down from 1200 to 680 Hz
        freq = 1200.0 - 520.0 * (t / 0.12)
        chirp = math.sin(2.0 * math.pi * freq * t) * 0.5
        spring = math.sin(2.0 * math.pi * 3400.0 * t) * 0.25 * math.exp(-50.0 * t)
        thump = math.sin(2.0 * math.pi * 190.0 * t) * 0.35
        samples[i] = (chirp + spring + thump) * env

    return [math.tanh(s * 1.2) for s in samples]

GENERATORS = [
    ("save_success", "ui_save_success.wav", synthesize_save_success, 101),
    ("ui_invalid_action", "ui_invalid_action.wav", synthesize_invalid_action, 102),
    ("ui_modal_open", "ui_modal_open.wav", synthesize_modal_open, 103),
    ("ui_modal_close", "ui_modal_close.wav", synthesize_modal_close, 104),
    ("ui_tab_change", "ui_tab_change.wav", synthesize_tab_change, 105),
    ("ui_cancel", "ui_cancel.wav", synthesize_cancel, 106),
]

def generate_all():
    print("=== Generating Wave A Dedicated UI Assets ===")
    ledger = DeliveryLedger()
    failures = 0

    for cue_id, filename, gen_func, seed in GENERATORS:
        out_path = UI_DIR / filename
        # 1. Test reproducibility
        is_repro, h1, h2 = ReproducibilityEngine.verify(gen_func, seed=seed, preset=PRESET_UI)
        if not is_repro:
            print(f"  [FAIL] {filename} failed reproducibility check: {h1} != {h2}")
            failures += 1
            continue

        # 2. Synthesize & export
        samples = gen_func(seed=seed)
        sha = AudioExporter.export_wav(out_path, samples, PRESET_UI)
        metrics = AudioMeasurer.measure(out_path)
        ledger.record(cue_id, out_path, PRESET_UI, sha, metrics, "ACCEPTED")
        print(f"  [GENERATED] {filename}: {metrics['integrated_lufs']:.1f} LUFS, Peak {metrics['true_peak_dbfs']:.2f} dBFS, Dur {metrics['duration_seconds']:.3f}s [sha256: {sha[:8]}]")

    print("\nGeneration complete. Run Godot headless import to generate .import sidecars.")
    return failures

if __name__ == '__main__':
    sys.exit(generate_all())
