#!/usr/bin/env python3
"""
tools/audio_pipeline.py — ASHFALL Shared Audio Generation & Mastering Pipeline.

Provides canonical, release-grade audio generation primitives for ASHFALL:
1. Named Delivery Presets per asset class (UI, Ambience, Loop, Radio, SFX, Transient).
2. Prohibited Full-Scale Peak Normalization (Strict peak ceiling <= -1.5 dBFS, max overs = 0).
3. Deterministic Seeded PRNG and DSP synthesis building blocks.
4. Clean WAV Exporter (Never fabricates .import sidecars; delegates to Godot import).
5. EBU R128 Loudness and True Peak Measurement via FFmpeg.
6. Byte-Reproducibility Engine (Verifies identical SHA-256 hashes across seeded runs).
7. Delivery Ledger (Machine-readable JSON and Markdown reports).
"""

import dataclasses
import hashlib
import json
import math
import os
import pathlib
import random
import re
import struct
import subprocess
import wave
from typing import Callable, Dict, List, Optional, Tuple

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent
DEFAULT_SAMPLE_RATE = 44100

@dataclasses.dataclass(frozen=True)
class DeliveryPreset:
    name: str
    target_lufs: float
    max_peak_dbfs: float
    loop_mode: int = 0  # 0: disabled, 1: forward
    sample_rate: int = DEFAULT_SAMPLE_RATE

    @property
    def linear_ceiling(self) -> float:
        """Linear amplitude ceiling corresponding to max_peak_dbfs (never exceeds 0.891)."""
        return 10.0 ** (self.max_peak_dbfs / 20.0)

# Canonical Delivery Presets per ASHFALL Audio Directives
PRESET_UI = DeliveryPreset(
    name="UI",
    target_lufs=-18.0,
    max_peak_dbfs=-1.5,
    loop_mode=0
)

PRESET_AMBIENCE = DeliveryPreset(
    name="Ambience",
    target_lufs=-24.0,
    max_peak_dbfs=-2.0,
    loop_mode=1
)

PRESET_LOOP = DeliveryPreset(
    name="Loop",
    target_lufs=-20.0,
    max_peak_dbfs=-2.0,
    loop_mode=1
)

PRESET_RADIO = DeliveryPreset(
    name="Radio",
    target_lufs=-16.0,
    max_peak_dbfs=-1.5,
    loop_mode=0
)

PRESET_VOICE = DeliveryPreset(
    name="Voice",
    target_lufs=-16.0,
    max_peak_dbfs=-1.5,
    loop_mode=0
)

PRESET_SFX = DeliveryPreset(
    name="SFX",
    target_lufs=-14.0,
    max_peak_dbfs=-1.5,
    loop_mode=0
)

PRESET_MUSIC = DeliveryPreset(
    name="Music",
    target_lufs=-16.0,
    max_peak_dbfs=-1.5,
    loop_mode=1
)

PRESET_TRANSIENT = DeliveryPreset(
    name="Transient",
    target_lufs=-16.0,
    max_peak_dbfs=-1.5,
    loop_mode=0
)

PRESET_MAP: Dict[str, DeliveryPreset] = {
    "ui": PRESET_UI,
    "ambience": PRESET_AMBIENCE,
    "loop": PRESET_LOOP,
    "radio": PRESET_RADIO,
    "voice": PRESET_VOICE,
    "sfx": PRESET_SFX,
    "music": PRESET_MUSIC,
    "transient": PRESET_TRANSIENT
}

class SeededSynthesizer:
    """Deterministic, pure-math synthesis building blocks for procedural audio."""

    def __init__(self, seed: int = 42, sample_rate: int = DEFAULT_SAMPLE_RATE):
        self.seed = seed
        self.sample_rate = sample_rate
        self.rng = random.Random(seed)

    def reset(self):
        self.rng = random.Random(self.seed)

    def sine(self, freq: float, duration: float, phase: float = 0.0) -> List[float]:
        n = int(self.sample_rate * duration)
        omega = 2.0 * math.pi * freq / self.sample_rate
        return [math.sin(omega * i + phase) for i in range(n)]

    def fm_sine(self, carrier_freq: float, mod_freq: float, mod_index: float, duration: float) -> List[float]:
        n = int(self.sample_rate * duration)
        samples = []
        phase_c = 0.0
        phase_m = 0.0
        delta_m = 2.0 * math.pi * mod_freq / self.sample_rate
        for _ in range(n):
            mod_val = math.sin(phase_m) * mod_index
            inst_freq = carrier_freq + mod_val
            delta_c = 2.0 * math.pi * inst_freq / self.sample_rate
            samples.append(math.sin(phase_c))
            phase_c += delta_c
            phase_m += delta_m
        return samples

    def white_noise(self, duration: float) -> List[float]:
        n = int(self.sample_rate * duration)
        return [(self.rng.random() * 2.0 - 1.0) for _ in range(n)]

    def filtered_noise(self, duration: float, filter_type: str = "lp", cutoff_factor: float = 0.1) -> List[float]:
        """Simple deterministic one-pole low-pass or high-pass noise filter."""
        raw = self.white_noise(duration)
        out = []
        state = 0.0
        alpha = max(0.001, min(0.999, cutoff_factor))
        for s in raw:
            state += alpha * (s - state)
            if filter_type == "lp":
                out.append(state)
            else:
                out.append(s - state)
        return out

    @staticmethod
    def apply_adsr(samples: List[float], attack_s: float, decay_s: float, sustain_level: float, release_s: float, sample_rate: int = DEFAULT_SAMPLE_RATE) -> List[float]:
        n = len(samples)
        att_samples = int(attack_s * sample_rate)
        dec_samples = int(decay_s * sample_rate)
        rel_samples = int(release_s * sample_rate)
        sus_samples = max(0, n - att_samples - dec_samples - rel_samples)

        env = []
        # Attack
        for i in range(min(att_samples, n)):
            env.append(i / max(1, att_samples))
        # Decay
        for i in range(min(dec_samples, n - len(env))):
            t = i / max(1, dec_samples)
            env.append(1.0 - (1.0 - sustain_level) * t)
        # Sustain
        for _ in range(min(sus_samples, n - len(env))):
            env.append(sustain_level)
        # Release
        rem = n - len(env)
        for i in range(rem):
            t = i / max(1, rem)
            env.append(sustain_level * (1.0 - t))

        return [samples[i] * env[i] for i in range(n)]

    @staticmethod
    def soft_clip(samples: List[float], drive: float = 1.0) -> List[float]:
        """Apply hyperbolic tangent soft-saturation."""
        return [math.tanh(s * drive) for s in samples]

class AudioMasterer:
    """Enforces mastering limits, true peak headroom, and prevents 0.000 dBFS normalization."""

    @staticmethod
    def master(samples: List[float], preset: DeliveryPreset) -> List[float]:
        """
        Master samples to preset headroom.
        PROHIBITS peak normalization to 1.0 (0 dBFS).
        Clamps peak to preset.linear_ceiling (default -1.5 dBFS ceiling = ~0.8414).
        """
        if not samples:
            return samples

        ceiling = preset.linear_ceiling
        if ceiling > 0.891:  # -1.0 dBFS is 0.8913
            raise ValueError(f"Linear ceiling {ceiling} exceeds safe mastering threshold -1.0 dBFS")

        peak = max(abs(s) for s in samples)
        if peak < 1e-6:
            return samples

        # Scale to fit strictly within linear ceiling
        scale = ceiling / peak
        mastered = [s * scale for s in samples]

        # Final verification: ensure no sample exceeds ceiling
        return [max(-ceiling, min(ceiling, s)) for s in mastered]

class AudioExporter:
    """Exports samples to WAV with byte-level verification. Never writes .import sidecars."""

    @staticmethod
    def export_wav(filepath: pathlib.Path, samples: List[float], preset: DeliveryPreset) -> str:
        """
        Exports 16-bit PCM WAV to disk.
        Returns SHA-256 hex digest of the exported file.
        Explicitly does NOT write any .import file.
        """
        filepath = pathlib.Path(filepath)
        filepath.parent.mkdir(parents=True, exist_ok=True)

        mastered = AudioMasterer.master(samples, preset)

        packed = bytearray()
        for s in mastered:
            clamped = max(-1.0, min(1.0, s))
            val = int(clamped * 32767.0)
            packed.extend(struct.pack('<h', val))

        with wave.open(str(filepath), 'wb') as wf:
            wf.setnchannels(1)
            wf.setsampwidth(2)
            wf.setframerate(preset.sample_rate)
            wf.writeframes(packed)

        # Read back bytes to compute canonical SHA-256 hash
        file_bytes = filepath.read_bytes()
        sha256 = hashlib.sha256(file_bytes).hexdigest()
        return sha256

class AudioMeasurer:
    """EBU R128 and True Peak measurement using FFmpeg."""

    @staticmethod
    def measure(filepath: pathlib.Path) -> Dict[str, float]:
        """
        Runs ffmpeg ebur128 on the audio file and returns measured metrics:
        - integrated_lufs
        - true_peak_dbfs
        - duration_seconds
        """
        filepath = pathlib.Path(filepath)
        if not filepath.exists():
            raise FileNotFoundError(f"File not found: {filepath}")

        cmd = [
            "ffmpeg", "-nostats", "-i", str(filepath),
            "-filter_complex", "ebur128=peak=true",
            "-f", "null", "-"
        ]
        res = subprocess.run(cmd, capture_output=True, text=True, check=False)
        output = res.stderr

        # Parse integrated loudness
        integrated_lufs = -99.0
        m_lufs = re.search(r"Integrated loudness:\s+I:\s+([-\d\.]+)\s+LUFS", output)
        if m_lufs:
            integrated_lufs = float(m_lufs.group(1))

        # Parse true peak
        true_peak_dbfs = 99.0
        m_peak = re.search(r"True peak:\s+Peak:\s+([-\d\.]+)\s+dBFS", output)
        if m_peak:
            true_peak_dbfs = float(m_peak.group(1))

        # Parse duration
        duration = 0.0
        m_dur = re.search(r"time=(\d+):(\d+):([\d\.]+)", output)
        if m_dur:
            h, m, s = float(m_dur.group(1)), float(m_dur.group(2)), float(m_dur.group(3))
            duration = h * 3600.0 + m * 60.0 + s

        return {
            "integrated_lufs": integrated_lufs,
            "true_peak_dbfs": true_peak_dbfs,
            "duration_seconds": duration
        }

class ReproducibilityEngine:
    """Verifies that two identical seeded runs yield byte-identical outputs."""

    @staticmethod
    def verify(generator_fn: Callable[[int], List[float]], seed: int, preset: DeliveryPreset) -> Tuple[bool, str, str]:
        """
        Renders generator_fn(seed) twice and checks SHA-256 equality.
        Returns (is_reproducible, hash_1, hash_2).
        """
        run1_samples = generator_fn(seed)
        mastered1 = AudioMasterer.master(run1_samples, preset)
        bytes1 = bytearray()
        for s in mastered1:
            bytes1.extend(struct.pack('<h', int(max(-1.0, min(1.0, s)) * 32767.0)))
        hash1 = hashlib.sha256(bytes1).hexdigest()

        run2_samples = generator_fn(seed)
        mastered2 = AudioMasterer.master(run2_samples, preset)
        bytes2 = bytearray()
        for s in mastered2:
            bytes2.extend(struct.pack('<h', int(max(-1.0, min(1.0, s)) * 32767.0)))
        hash2 = hashlib.sha256(bytes2).hexdigest()

        return (hash1 == hash2, hash1, hash2)

class DeliveryLedger:
    """Records generated audio deliverables, metadata, measurements, and QA sign-off."""

    def __init__(self):
        self.entries: List[Dict] = []

    def record(self, asset_id: str, filepath: pathlib.Path, preset: DeliveryPreset, sha256: str, metrics: Dict[str, float], status: str = "ACCEPTED"):
        try:
            rel_path = str(filepath.relative_to(REPO_ROOT)).replace("\\", "/")
        except ValueError:
            rel_path = str(filepath).replace("\\", "/")
        self.entries.append({
            "asset_id": asset_id,
            "path": rel_path,
            "preset": preset.name,
            "target_lufs": preset.target_lufs,
            "integrated_lufs": metrics.get("integrated_lufs", -99.0),
            "max_peak_ceiling_dbfs": preset.max_peak_dbfs,
            "measured_true_peak_dbfs": metrics.get("true_peak_dbfs", 99.0),
            "duration_s": round(metrics.get("duration_seconds", 0.0), 3),
            "sha256": sha256,
            "status": status
        })

    def save_json(self, path: pathlib.Path):
        path = pathlib.Path(path)
        path.parent.mkdir(parents=True, exist_ok=True)
        with open(path, "w", encoding="utf-8") as f:
            json.dump({"schema_version": 1, "entries": self.entries}, f, indent=2)

    def save_markdown(self, path: pathlib.Path):
        path = pathlib.Path(path)
        path.parent.mkdir(parents=True, exist_ok=True)
        lines = [
            "# ASHFALL Audio Pipeline Delivery Ledger",
            "",
            "| Asset ID | Path | Preset | Measured LUFS | True Peak | Duration | SHA-256 (first 8) | Status |",
            "|---|---|---|---|---|---|---|---|"
        ]
        for e in self.entries:
            short_hash = e["sha256"][:8]
            lines.append(f"| `{e['asset_id']}` | `{e['path']}` | {e['preset']} | {e['integrated_lufs']:.1f} LUFS | {e['measured_true_peak_dbfs']:.1f} dBFS | {e['duration_s']}s | `{short_hash}` | **{e['status']}** |")
        lines.append("")
        path.write_text("\n".join(lines), encoding="utf-8")

def trigger_godot_import() -> int:
    """Executes headless Godot asset import. Returns process returncode."""
    cmd = ["godot", "--headless", "--path", str(REPO_ROOT), "--import"]
    res = subprocess.run(cmd, capture_output=True, text=True, check=False)
    return res.returncode
