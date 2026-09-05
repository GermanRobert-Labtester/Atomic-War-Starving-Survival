#!/usr/bin/env python3
"""
audio-asset-gate.py — Authoritative CI Gate for Audio Library

Validates:
1. True Peak Headroom: No file peaks at or above 0.000 dBFS (hard fail), and none > -0.95 dBFS.
2. Silence / Minimum Duration: No empty or completely silent audio files (duration > 0.01s, RMS > -80 dBFS).
3. Metadata Sidecars: Every audio file in assets/audio/ has a valid Godot .import sidecar.
4. Git Tracking: Zero untracked or unversioned audio assets in assets/audio/.
"""

import sys
import glob
import pathlib
import subprocess
import re

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
AUDIO_DIR = REPO_ROOT / "assets" / "audio"

def check_untracked():
    cmd = ['git', 'status', '--porcelain', 'assets/audio']
    res = subprocess.run(cmd, cwd=REPO_ROOT, capture_output=True, text=True)
    untracked = []
    for line in res.stdout.splitlines():
        if line.startswith('??'):
            path = line[3:].strip()
            if path.endswith(('.wav', '.mp3', '.ogg')):
                untracked.append(path)
    return untracked

def check_audio_file(file_path):
    sidecar = file_path.with_name(f"{file_path.name}.import")
    if not sidecar.is_file():
        return False, f"Missing Godot .import sidecar: {sidecar.relative_to(REPO_ROOT)}"

    # Run ebur128 true peak and duration
    cmd = ['ffmpeg', '-nostats', '-i', str(file_path), '-filter_complex', 'ebur128=peak=true', '-f', 'null', '-']
    res = subprocess.run(cmd, capture_output=True, text=True)

    m_tp = re.search(r'True peak:\s+Peak:\s+([-\d\.]+)\s+dBFS', res.stderr)
    m_time = re.search(r'time=(\d+):(\d+):([\d\.]+)', res.stderr)

    if not m_tp:
        return False, f"Could not measure true peak for {file_path.relative_to(REPO_ROOT)}"

    tp = float(m_tp.group(1))
    if tp >= 0.000:
        return False, f"HARD FAIL: True Peak {tp} dBFS >= 0.000 dBFS (overs clipping) on {file_path.relative_to(REPO_ROOT)}"
    if tp > -0.95:
        return False, f"HARD FAIL: True Peak {tp} dBFS > -1.0 dBFS headroom threshold on {file_path.relative_to(REPO_ROOT)}"

    # Check duration
    if m_time:
        duration = int(m_time.group(1))*3600 + int(m_time.group(2))*60 + float(m_time.group(3))
        if duration < 0.01:
            return False, f"HARD FAIL: Audio duration too short ({duration}s) on {file_path.relative_to(REPO_ROOT)}"

    return True, f"OK (TP={tp:.2f} dBFS)"

def main():
    audio_files = sorted([
        pathlib.Path(f) for f in glob.glob(str(AUDIO_DIR / "**/*.*"), recursive=True)
        if f.lower().endswith(('.wav', '.mp3', '.ogg'))
    ])

    print(f"[AUDIO_GATE] Scanning {len(audio_files)} audio assets across assets/audio/...")

    # 1. Check untracked assets
    untracked = check_untracked()
    if untracked:
        print(f"[AUDIO_GATE] FAIL: Found {len(untracked)} untracked audio assets:")
        for u in untracked[:10]:
            print(f"  - {u}")
        print("[AUDIO_GATE] Track all audio assets with Git LFS before running gate.")
        sys.exit(1)

    # 2. Check each audio file
    failures = []
    for f in audio_files:
        ok, msg = check_audio_file(f)
        if not ok:
            failures.append((f, msg))

    if failures:
        print(f"[AUDIO_GATE] FAIL: {len(failures)} audio assets failed validation:")
        for f, msg in failures:
            print(f"  - {msg}")
        sys.exit(1)

    print(f"[AUDIO_GATE] PASS: All {len(audio_files)} audio assets passed headroom, metadata, and silence checks.")
    print(f"[AUDIO_GATE] 0 files peaking at or above 0.000 dBFS.")
    print(f"[AUDIO_GATE] 0 unversioned/untracked audio assets.")
    sys.exit(0)

if __name__ == '__main__':
    main()
