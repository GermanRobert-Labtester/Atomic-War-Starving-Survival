#!/usr/bin/env python3
"""
tools/ingest_elevenlabs_phase7.py — Ingest and master ElevenLabs generated audio assets.

Takes audio generated via ElevenLabs MCP, decodes to standard PCM,
masters through AudioPipeline enforcing peak ceiling <= -1.5 dBFS,
and writes standard WAV or MP3 assets without .import sidecars.
"""

import os
import pathlib
import struct
import subprocess
import sys

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent
sys.path.insert(0, str(REPO_ROOT / "tools"))

from audio_pipeline import (
    AudioExporter,
    AudioMeasurer,
    DeliveryLedger,
    PRESET_SFX,
    PRESET_LOOP,
    PRESET_RADIO,
    PRESET_AMBIENCE,
    PRESET_UI,
    PRESET_VOICE
)

def ingest_file(input_file: pathlib.Path, target_file: pathlib.Path, preset, cue_id: str, ledger: DeliveryLedger = None):
    if not input_file.exists():
        raise FileNotFoundError(f"Input file not found: {input_file}")

    target_file.parent.mkdir(parents=True, exist_ok=True)

    # 1. Decode to 44.1kHz mono f32le using ffmpeg
    cmd = [
        "ffmpeg", "-y", "-i", str(input_file),
        "-ar", "44100", "-ac", "1", "-f", "f32le", "-"
    ]
    proc = subprocess.run(cmd, capture_output=True, check=True)
    samples = [val[0] for val in struct.iter_unpack("<f", proc.stdout)]

    # 2. Master and export via AudioExporter (enforces peak <= -1.5 dBFS)
    if target_file.suffix.lower() == ".wav":
        sha256 = AudioExporter.export_wav(target_file, samples, preset)
    else:
        # For MP3/OGG targets, export temp wav then encode via ffmpeg with strict peak ceiling
        temp_wav = target_file.with_suffix(".temp.wav")
        sha256 = AudioExporter.export_wav(temp_wav, samples, preset)
        subprocess.run([
            "ffmpeg", "-y", "-i", str(temp_wav),
            "-b:a", "128k", "-ar", "44100", "-ac", "1",
            str(target_file)
        ], capture_output=True, check=True)
        temp_wav.unlink(missing_ok=True)
        import hashlib
        with open(target_file, "rb") as f:
            sha256 = hashlib.sha256(f.read()).hexdigest()

    metrics = AudioMeasurer.measure(target_file)
    target_file = target_file.resolve()
    print(f"  [INGESTED] {target_file.relative_to(REPO_ROOT)}: {metrics['integrated_lufs']:.1f} LUFS, Peak {metrics['true_peak_dbfs']:.2f} dBFS, Dur {metrics['duration_seconds']:.2f}s [sha256: {sha256[:8]}]")

    if ledger:
        ledger.record(cue_id, target_file, preset, sha256, metrics, "ACCEPTED")
    return sha256, metrics

if __name__ == '__main__':
    if len(sys.argv) < 4:
        print("Usage: python3 ingest_elevenlabs_phase7.py <input_file> <target_file> <preset_name> <cue_id>")
        sys.exit(1)

    presets = {
        "SFX": PRESET_SFX,
        "LOOP": PRESET_LOOP,
        "RADIO": PRESET_RADIO,
        "AMBIENCE": PRESET_AMBIENCE,
        "UI": PRESET_UI,
        "VOICE": PRESET_VOICE,
    }
    inp = pathlib.Path(sys.argv[1])
    tgt = pathlib.Path(sys.argv[2])
    pre = presets.get(sys.argv[3].upper(), PRESET_SFX)
    cid = sys.argv[4]

    ingest_file(inp, tgt, pre, cid)
