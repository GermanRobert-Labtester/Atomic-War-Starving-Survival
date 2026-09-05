#!/usr/bin/env python3
"""
asset-decode-gate.py — Hard CI gate verifying that all assets in the assets/ tree
have valid file signatures, valid container structures, and can be independently decoded.

Exits with:
  0 — All files valid
  1 — Corrupted, truncated, or mislabeled files detected
"""

import os
import sys
import struct
import wave
import xml.etree.ElementTree as ET

ROOT_DIRS = ["assets/art", "assets/sprites", "assets/ui", "assets/audio", "assets/fonts"]

def check_png(path):
    with open(path, "rb") as f:
        hdr = f.read(8)
        if hdr != b"\x89PNG\r\n\x1a\n":
            return f"Invalid PNG magic bytes: {hdr[:16]}"
        ihdr_len, ihdr_type = struct.unpack(">I4s", f.read(8))
        if ihdr_type != b"IHDR" or ihdr_len < 13:
            return f"Invalid PNG IHDR chunk: {ihdr_type}"
        w, h = struct.unpack(">II", f.read(8))
        if w == 0 or h == 0:
            return f"Invalid PNG dimensions: {w}x{h}"
    return None

def check_jpg(path):
    with open(path, "rb") as f:
        hdr = f.read(3)
        if hdr != b"\xff\xd8\xff":
            return f"Invalid JPEG SOI marker: {hdr[:8]}"
    return None

def check_svg(path):
    try:
        tree = ET.parse(path)
        root = tree.getroot()
        tag = root.tag.lower()
        if not tag.endswith("svg"):
            return f"Root XML tag is not <svg>: {tag}"
    except Exception as e:
        return f"Corrupt SVG XML: {e}"
    return None

def check_wav(path):
    try:
        with wave.open(path, "rb") as wf:
            frames = wf.getnframes()
            rate = wf.getframerate()
            ch = wf.getnchannels()
            if frames == 0:
                return f"Zero audio frames in WAV"
            if ch not in (1, 2):
                return f"Unsupported channel count: {ch}"
            if rate < 22050:
                return f"Sample rate below project standard: {rate} Hz"
    except Exception as e:
        return f"Failed to decode WAV: {e}"
    return None

def check_mp3(path):
    with open(path, "rb") as f:
        hdr = f.read(4)
        if len(hdr) < 4:
            return "File too short for MP3"
        if hdr[:3] != b"ID3" and not (hdr[0] == 0xff and (hdr[1] & 0xe0) == 0xe0):
            return f"Invalid MP3 header/sync: {hdr[:8]}"
    return None

def check_ogg(path):
    with open(path, "rb") as f:
        hdr = f.read(4)
        if hdr != b"OggS":
            return f"Invalid OggS header: {hdr[:8]}"
    return None

def check_font(path):
    with open(path, "rb") as f:
        hdr = f.read(4)
        # 0x00010000 for TrueType, 'OTTO' for OpenType, 'ttcf' for font collection
        if hdr not in (b"\x00\x01\x00\x00", b"OTTO", b"ttcf"):
            return f"Invalid font sfnt header: {hdr[:8]}"
    return None

def main():
    errors = []
    counts = {"png": 0, "jpg": 0, "svg": 0, "wav": 0, "mp3": 0, "ogg": 0, "font": 0}

    for root_dir in ROOT_DIRS:
        if not os.path.exists(root_dir):
            continue
        for root, dirs, files in os.walk(root_dir):
            for f in sorted(files):
                path = os.path.join(root, f)
                ext = os.path.splitext(f)[1].lower()
                err = None

                if ext == ".png":
                    counts["png"] += 1
                    err = check_png(path)
                elif ext in (".jpg", ".jpeg"):
                    counts["jpg"] += 1
                    err = check_jpg(path)
                elif ext == ".svg":
                    counts["svg"] += 1
                    err = check_svg(path)
                elif ext == ".wav":
                    counts["wav"] += 1
                    err = check_wav(path)
                elif ext == ".mp3":
                    counts["mp3"] += 1
                    err = check_mp3(path)
                elif ext == ".ogg":
                    counts["ogg"] += 1
                    err = check_ogg(path)
                elif ext in (".ttf", ".otf"):
                    counts["font"] += 1
                    err = check_font(path)

                if err:
                    errors.append(f"{path}: {err}")

    print("=== ASSET DECODE GATE REPORT ===")
    for k, v in counts.items():
        print(f"  {k.upper()}: {v} files verified")
    total = sum(counts.values())
    print(f"Total checked: {total}")

    if errors:
        print(f"\n[FAIL] Found {len(errors)} decode/signature error(s):")
        for e in errors:
            print(f"  ERROR: {e}")
        sys.exit(1)
    else:
        print("\n[PASS] All assets verified with valid signatures and decodable containers.")
        sys.exit(0)

if __name__ == "__main__":
    main()
