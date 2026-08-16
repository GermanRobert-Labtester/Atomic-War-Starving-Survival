#!/usr/bin/env python3
"""
ASHFALL procedural audio asset generator.
Generates atmospheric, industrial, post-nuclear audio assets using numpy.
All output: 16-bit PCM WAV, 44100 Hz, mono (except ambience which is stereo).
"""

import numpy as np
import wave
import os
import struct

SAMPLE_RATE = 44100
OUTPUT_DIR = os.path.join(os.path.dirname(os.path.dirname(os.path.abspath(__file__))), "assets", "audio")

def ensure_dir(path):
    os.makedirs(path, exist_ok=True)

def save_wav(filename, samples, sample_rate=SAMPLE_RATE, loop=False):
    """Save float samples [-1, 1] as 16-bit PCM WAV."""
    samples = np.clip(samples, -1.0, 1.0)
    int_samples = (samples * 32767).astype(np.int16)
    n_channels = 1 if samples.ndim == 1 else 2
    with wave.open(filename, 'w') as wf:
        wf.setnchannels(n_channels)
        wf.setsampwidth(2)
        wf.setframerate(sample_rate)
        wf.writeframes(int_samples.tobytes())
    print(f"  Written: {filename} ({len(samples)/sample_rate:.1f}s, {n_channels}ch)")

def brown_noise(n_samples):
    """Generate brown (Brownian) noise."""
    white = np.random.randn(n_samples)
    brown = np.cumsum(white) * 0.01
    brown = brown / (np.max(np.abs(brown)) + 1e-10)
    return brown

def pink_noise(n_samples):
    """Generate approximate pink noise using Voss-McCartney algorithm."""
    num_rows = 16
    array = np.random.randn(num_rows, n_samples)
    pink = np.zeros(n_samples)
    for i in range(num_rows):
        step = 2 ** i
        for j in range(0, n_samples, step):
            end = min(j + step, n_samples)
            pink[j:end] += array[i, j]
    pink = pink / (np.max(np.abs(pink)) + 1e-10)
    return pink

def lowpass(samples, cutoff_hz, sample_rate=SAMPLE_RATE):
    """Simple one-pole lowpass filter."""
    rc = 1.0 / (2.0 * np.pi * cutoff_hz)
    dt = 1.0 / sample_rate
    alpha = dt / (rc + dt)
    filtered = np.zeros_like(samples)
    filtered[0] = samples[0] * alpha
    for i in range(1, len(samples)):
        filtered[i] = filtered[i-1] + alpha * (samples[i] - filtered[i-1])
    return filtered

def envelope(n_samples, attack=0.01, decay=0.0, sustain=1.0, release=0.05, sample_rate=SAMPLE_RATE):
    """ADSR envelope."""
    env = np.ones(n_samples)
    a_samples = int(attack * sample_rate)
    d_samples = int(decay * sample_rate)
    r_samples = int(release * sample_rate)
    for i in range(min(a_samples, n_samples)):
        env[i] = i / max(a_samples, 1)
    for i in range(a_samples, min(a_samples + d_samples, n_samples)):
        t = (i - a_samples) / max(d_samples, 1)
        env[i] = 1.0 - t * (1.0 - sustain)
    for i in range(max(0, n_samples - r_samples), n_samples):
        t = (n_samples - i) / max(r_samples, 1)
        env[i] = env[i] * t
    return env

def gen_click():
    """Short mechanical click — dry, industrial."""
    dur = 0.08
    n = int(dur * SAMPLE_RATE)
    t = np.arange(n) / SAMPLE_RATE
    noise = np.random.randn(n)
    noise = lowpass(noise, 3000)
    env_sig = envelope(n, attack=0.001, release=0.04)
    click = noise * env_sig * 0.7
    tone = np.sin(2 * np.pi * 800 * t) * envelope(n, attack=0.001, release=0.02) * 0.3
    return click + tone

def gen_confirm():
    """Confirmation tone — two short ascending tones."""
    dur = 0.25
    n = int(dur * SAMPLE_RATE)
    t = np.arange(n) / SAMPLE_RATE
    half = n // 2
    sig = np.zeros(n)
    env1 = envelope(half, attack=0.005, release=0.03)
    sig[:half] = np.sin(2 * np.pi * 440 * t[:half]) * env1 * 0.5
    env2 = envelope(n - half, attack=0.005, release=0.05)
    sig[half:] = np.sin(2 * np.pi * 660 * t[half:]) * env2 * 0.5
    return sig

def gen_warning():
    """Warning alert — harsh dual-tone pulse."""
    dur = 0.6
    n = int(dur * SAMPLE_RATE)
    t = np.arange(n) / SAMPLE_RATE
    pulse_freq = 4.0
    pulse = np.sign(np.sin(2 * np.pi * pulse_freq * t)) * 0.5 + 0.5
    tone_a = np.sin(2 * np.pi * 520 * t) * 0.4
    tone_b = np.sin(2 * np.pi * 680 * t) * 0.3
    env_sig = envelope(n, attack=0.01, release=0.08)
    return (tone_a + tone_b) * pulse * env_sig

def gen_geiger():
    """Geiger counter crackle — random clicks with varying density."""
    dur = 8.0
    n = int(dur * SAMPLE_RATE)
    sig = np.zeros(n)
    num_clicks = np.random.randint(80, 150)
    for _ in range(num_clicks):
        pos = np.random.randint(0, n - 200)
        click_dur = np.random.randint(40, 120)
        click = np.random.randn(click_dur) * 0.6
        click = lowpass(click, 5000)
        end = min(pos + click_dur, n)
        sig[pos:end] += click[:end - pos]
    sig = lowpass(sig, 6000)
    sig = sig / (np.max(np.abs(sig)) + 1e-10) * 0.7
    return sig

def gen_radiation_alert():
    """Radiation alert — pulsing low tone with Geiger overlay."""
    dur = 2.0
    n = int(dur * SAMPLE_RATE)
    t = np.arange(n) / SAMPLE_RATE
    drone = np.sin(2 * np.pi * 110 * t) * 0.3
    pulse = (np.sin(2 * np.pi * 2.5 * t) * 0.5 + 0.5)
    clicks = np.zeros(n)
    for _ in range(40):
        pos = np.random.randint(0, n - 100)
        click = np.random.randn(60) * 0.3
        end = min(pos + 60, n)
        clicks[pos:end] += click[:end - pos]
    clicks = lowpass(clicks, 4000)
    env_sig = envelope(n, attack=0.1, release=0.2)
    return (drone * pulse + clicks * 0.4) * env_sig

def gen_weather_alert():
    """Weather warning — descending tone sweep with wind burst."""
    dur = 1.8
    n = int(dur * SAMPLE_RATE)
    t = np.arange(n) / SAMPLE_RATE
    freq_sweep = np.linspace(600, 200, n)
    phase = np.cumsum(2 * np.pi * freq_sweep / SAMPLE_RATE)
    tone = np.sin(phase) * 0.4
    wind = pink_noise(n) * 0.3
    wind = lowpass(wind, 800)
    env_sig = envelope(n, attack=0.05, release=0.3)
    wind_env = np.concatenate([np.linspace(0, 1, n//4), np.ones(n//2), np.linspace(1, 0, n - n//2 - n//4)])
    wind_env = wind_env[:n]
    return (tone + wind * wind_env) * env_sig

def gen_bunker_ambience():
    """Bunker ambience — ventilation hum, generator drone, pipe resonances. Stereo."""
    dur = 30.0
    n = int(dur * SAMPLE_RATE)
    t = np.arange(n) / SAMPLE_RATE

    ventilation = brown_noise(n)
    ventilation = lowpass(ventilation, 120)
    ventilation = ventilation * 0.4

    gen_freq = 55 + 2 * np.sin(2 * np.pi * 0.05 * t)
    gen_phase = np.cumsum(2 * np.pi * gen_freq / SAMPLE_RATE)
    generator = np.sin(gen_phase) * 0.15
    generator += np.sin(gen_phase * 2) * 0.05
    generator += np.sin(gen_phase * 3) * 0.02

    pipe_freqs = [220, 330, 440, 165]
    pipes = np.zeros(n)
    for f in pipe_freqs:
        amp = 0.02 * (np.sin(2 * np.pi * np.random.uniform(0.01, 0.05) * t) * 0.5 + 0.5)
        pipes += np.sin(2 * np.pi * f * t) * amp
    pipes = lowpass(pipes, 600)

    drip_interval = int(2.5 * SAMPLE_RATE)
    drips = np.zeros(n)
    for i in range(0, n - 2000, drip_interval):
        offset = np.random.randint(-int(0.3 * SAMPLE_RATE), int(0.3 * SAMPLE_RATE))
        pos = min(max(i + offset, 0), n - 1000)
        drip_t = np.arange(800) / SAMPLE_RATE
        drip = np.sin(2 * np.pi * 1200 * drip_t) * np.exp(-drip_t * 15) * 0.08
        end = min(pos + 800, n)
        drips[pos:end] += drip[:end - pos]

    left = ventilation + generator + pipes * 0.7 + drips
    right = ventilation * 0.95 + generator * 0.9 + pipes * 0.5 + drips * 0.8
    right = np.roll(right, int(0.02 * SAMPLE_RATE))

    stereo = np.column_stack([left, right])
    stereo = stereo / (np.max(np.abs(stereo)) + 1e-10) * 0.6
    return stereo

def gen_surface_ambience():
    """Surface ambience — wind, distant debris, faint radiation crackle. Stereo."""
    dur = 30.0
    n = int(dur * SAMPLE_RATE)
    t = np.arange(n) / SAMPLE_RATE

    wind_base = pink_noise(n)
    wind_mod = np.sin(2 * np.pi * 0.08 * t) * 0.3 + 0.7
    wind = lowpass(wind_base * wind_mod, 400) * 0.5

    gust_freq = 0.03
    gust = np.sin(2 * np.pi * gust_freq * t) * 0.5 + 0.5
    gust_noise = pink_noise(n)
    gust_noise = lowpass(gust_noise, 600)
    wind += gust_noise * gust * 0.3

    geiger_bg = np.zeros(n)
    for _ in range(200):
        pos = np.random.randint(0, n - 80)
        click = np.random.randn(40) * 0.15
        end = min(pos + 40, n)
        geiger_bg[pos:end] += click[:end - pos]
    geiger_bg = lowpass(geiger_bg, 5000) * 0.2

    debris = pink_noise(n) * 0.05
    debris_env = np.zeros(n)
    for _ in range(8):
        pos = np.random.randint(0, n - int(3 * SAMPLE_RATE))
        length = np.random.randint(int(0.5 * SAMPLE_RATE), int(2 * SAMPLE_RATE))
        debris_env[pos:pos+length] += np.linspace(0, 0.3, length)
    debris_env = np.clip(debris_env, 0, 0.3)
    debris *= debris_env

    left = wind + geiger_bg + debris
    right = wind * 0.9 + geiger_bg * 0.8 + debris * 1.1
    right = np.roll(right, int(0.015 * SAMPLE_RATE))

    stereo = np.column_stack([left, right])
    stereo = stereo / (np.max(np.abs(stereo)) + 1e-10) * 0.55
    return stereo

def gen_main_menu_music():
    """Main menu drone — somber, minimal, cold. Mono."""
    dur = 60.0
    n = int(dur * SAMPLE_RATE)
    t = np.arange(n) / SAMPLE_RATE

    base_freq = 55
    drone = np.sin(2 * np.pi * base_freq * t) * 0.2
    drone += np.sin(2 * np.pi * base_freq * 1.5 * t) * 0.08
    drone += np.sin(2 * np.pi * base_freq * 2 * t) * 0.04

    lfo = np.sin(2 * np.pi * 0.02 * t) * 0.5 + 0.5
    drone *= (0.6 + lfo * 0.4)

    pad = pink_noise(n) * 0.06
    pad = lowpass(pad, 200)

    high_pad = brown_noise(n) * 0.03
    high_pad = lowpass(high_pad, 800)
    high_lfo = np.sin(2 * np.pi * 0.01 * t) * 0.5 + 0.5
    high_pad *= high_lfo

    fade = envelope(n, attack=3.0, release=5.0)
    sig = (drone + pad + high_pad) * fade
    sig = sig / (np.max(np.abs(sig)) + 1e-10) * 0.45
    return sig

def gen_gameplay_music():
    """Gameplay underscore — tense, quiet, minimal pulses. Mono."""
    dur = 60.0
    n = int(dur * SAMPLE_RATE)
    t = np.arange(n) / SAMPLE_RATE

    base_freq = 73.42
    drone = np.sin(2 * np.pi * base_freq * t) * 0.12
    drone += np.sin(2 * np.pi * base_freq * 3 * t) * 0.03
    drone_lfo = np.sin(2 * np.pi * 0.015 * t) * 0.3 + 0.7
    drone *= drone_lfo

    pulse_interval = int(4.0 * SAMPLE_RATE)
    pulses = np.zeros(n)
    for i in range(0, n - int(0.5 * SAMPLE_RATE), pulse_interval):
        pulse_t = np.arange(int(0.4 * SAMPLE_RATE)) / SAMPLE_RATE
        pulse = np.sin(2 * np.pi * 110 * pulse_t) * np.exp(-pulse_t * 3) * 0.15
        end = min(i + len(pulse), n)
        pulses[i:end] += pulse[:end - i]

    texture = brown_noise(n) * 0.04
    texture = lowpass(texture, 300)

    fade = envelope(n, attack=2.0, release=4.0)
    sig = (drone + pulses + texture) * fade
    sig = sig / (np.max(np.abs(sig)) + 1e-10) * 0.4
    return sig


def main():
    np.random.seed(42)

    categories = {
        "ambience": ["bunker_ambience", "surface_ambience"],
        "ui": ["ui_click", "ui_confirm", "ui_warning"],
        "sfx": ["geiger", "radiation_alert", "weather_alert"],
        "music": ["main_menu", "gameplay_underscore"],
    }

    generators = {
        "bunker_ambience": gen_bunker_ambience,
        "surface_ambience": gen_surface_ambience,
        "ui_click": gen_click,
        "ui_confirm": gen_confirm,
        "ui_warning": gen_warning,
        "geiger": gen_geiger,
        "radiation_alert": gen_radiation_alert,
        "weather_alert": gen_weather_alert,
        "main_menu": gen_main_menu_music,
        "gameplay_underscore": gen_gameplay_music,
    }

    total = 0
    for category, names in categories.items():
        cat_dir = os.path.join(OUTPUT_DIR, category)
        ensure_dir(cat_dir)
        print(f"\n[{category}]")
        for name in names:
            gen_fn = generators[name]
            samples = gen_fn()
            filename = os.path.join(cat_dir, f"{name}.wav")
            save_wav(filename, samples)
            total += 1

    print(f"\nGenerated {total} audio assets in {OUTPUT_DIR}")

if __name__ == "__main__":
    main()
