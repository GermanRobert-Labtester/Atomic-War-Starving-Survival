#!/usr/bin/env python3
"""
generate_train_screech_crash.py — High-impact railway derailment, flange screech & collision SFX.

Synthesizes:
assets/audio/sfx/sfx_train_screech_crash.wav

Characteristics:
- Steel flange brake screech: FM synthesis on high inharmonic frequencies.
- High-inertia impact: Low-frequency impact transient with waveshaping saturation.
- Structural crumpling & metal tearing.
- Headroom: Peak ceiling strictly <= -1.5 dBFS.
- Target loudness: ~ -14.0 LUFS.
"""

import math
import struct
import wave
import pathlib
import random

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent
TARGET_FILE = REPO_ROOT / "assets" / "audio" / "sfx" / "sfx_train_screech_crash.wav"

SAMPLE_RATE = 44100
DURATION = 2.85
NUM_SAMPLES = int(SAMPLE_RATE * DURATION)

def synthesize():
    rng = random.Random(42)  # Deterministic seed
    samples = [0.0] * NUM_SAMPLES

    # 1. Phase 1: High Flange Screech (0.0 to 1.2s, peaking around 0.6s)
    screech_len = int(SAMPLE_RATE * 1.5)
    p1 = 0.0
    p2 = 0.0
    p3 = 0.0
    for i in range(screech_len):
        t = i / SAMPLE_RATE
        # Screech envelope: rises sharply, maintains, cuts into collision
        if t < 0.6:
            env = (t / 0.6) ** 1.5
        else:
            env = max(0.0, 1.0 - ((t - 0.6) / 0.9) ** 0.8)

        # Frequency modulation for gritty metal screech
        wobble1 = math.sin(2.0 * math.pi * 32.0 * t) * 120.0
        wobble2 = math.sin(2.0 * math.pi * 17.5 * t) * 85.0
        jitter = (rng.random() - 0.5) * 60.0

        f1 = 2150.0 + wobble1 + jitter
        f2 = 2840.0 + wobble2
        f3 = 3490.0 - wobble1 * 0.5

        p1 += 2.0 * math.pi * f1 / SAMPLE_RATE
        p2 += 2.0 * math.pi * f2 / SAMPLE_RATE
        p3 += 2.0 * math.pi * f3 / SAMPLE_RATE

        screech = (
            math.sin(p1) * 0.45 +
            math.sin(p2) * 0.35 +
            math.sin(p3) * 0.20
        )

        # Add friction noise
        friction = (rng.random() - 0.5) * 0.25
        samples[i] += (screech + friction) * env * 0.6

    # 2. Phase 2: Violent Kinetic Impact & Derailment Crash (starts at t=0.55s)
    impact_start = int(SAMPLE_RATE * 0.55)
    impact_len = NUM_SAMPLES - impact_start

    lp_noise_state = 0.0
    for i in range(impact_len):
        idx = impact_start + i
        t = i / SAMPLE_RATE

        # Impact transient envelope
        if t < 0.08:
            impact_env = (t / 0.08) ** 0.5
        else:
            impact_env = math.exp(-2.8 * (t - 0.08))

        # Heavy sub-bass thud (75Hz sweeping down to 35Hz)
        sub_freq = 75.0 * math.exp(-1.5 * t) + 35.0
        sub = math.sin(2.0 * math.pi * sub_freq * t) * impact_env * 0.85

        # Metal crumple & crunch (saturated noise + ring mod)
        white = (rng.random() * 2.0 - 1.0)
        lp_noise_state += 0.25 * (white - lp_noise_state)
        crunch = lp_noise_state * math.exp(-1.8 * t) * 0.75

        # Structural tearing clang (180 Hz & 320 Hz metallic ring)
        clang1 = math.sin(2.0 * math.pi * 184.0 * t) * math.exp(-3.5 * t) * 0.4
        clang2 = math.sin(2.0 * math.pi * 318.0 * t) * math.exp(-4.2 * t) * 0.3

        val = sub + crunch + clang1 + clang2
        # Soft saturation curve
        val = math.tanh(val * 1.4)
        samples[idx] += val * 0.85

    # 3. Phase 3: Debris & Hiss Settling Tail (from t=1.2s to end)
    tail_start = int(SAMPLE_RATE * 1.2)
    tail_len = NUM_SAMPLES - tail_start
    hp_state = 0.0
    for i in range(tail_len):
        idx = tail_start + i
        t = i / SAMPLE_RATE
        tail_env = max(0.0, 1.0 - (t / (DURATION - 1.2))) ** 1.8

        # Escaping steam / settling gravel hiss (high-passed noise)
        raw_noise = (rng.random() * 2.0 - 1.0)
        hp_val = raw_noise - hp_state
        hp_state = raw_noise * 0.7 + hp_state * 0.3

        samples[idx] += hp_val * tail_env * 0.15

    # Export via shared AudioPipeline (enforcing -1.5 dBFS headroom)
    sys.path.insert(0, str(pathlib.Path(__file__).resolve().parent))
    from audio_pipeline import AudioExporter, PRESET_SFX
    sha256 = AudioExporter.export_wav(TARGET_FILE, samples, PRESET_SFX)
    print(f"Generated {TARGET_FILE.relative_to(REPO_ROOT)} ({NUM_SAMPLES} samples, {DURATION}s) [sha256: {sha256[:8]}].")

if __name__ == '__main__':
    synthesize()
