#!/usr/bin/env python3
"""ASHFALL — texture import trimmer (alpha download budget).

Two measured policies, both verified against the snapshot corpus:

  * art JPGs  (`assets/art/*.jpg`)          -> lossy re-encode, FULL dimensions.
    Keeps every layout that sizes art by its native texture unchanged; the
    alpha PCK dropped 294 MB -> 128 MB with only compression noise.
  * faction emblems (`assets/ui/Icons/faction_icon_*.png`) -> VRAM compressed,
    capped at 256 px. Emblems are always drawn at explicit 32-64 px sizes, so
    the cap is invisible; referenced ctex fell 15.2 MB -> 8.1 MB.

Idempotent; safe to run after every art wave (the wave close-out calls it).
Run `godot --headless --path . --import` afterwards to rebuild the ctex files.

Usage:
    python3 scripts/tools/trim_texture_imports.py [--dry-run]
"""
import argparse
import glob
import re
import sys

POLICIES = [
    # (glob, settings)
    ("assets/art/*.jpg", {"compress/mode": "1", "process/size_limit": "0", "mipmaps/generate": "false"}),
    ("assets/ui/Icons/faction_icon_*.png", {"compress/mode": "2", "process/size_limit": "256", "mipmaps/generate": "false"}),
]


def patch_import(import_path: str, settings: dict) -> bool:
    with open(import_path) as f:
        text = f.read()
    out = text
    for key, value in settings.items():
        pattern = re.compile(rf"^{re.escape(key)}=.*$", re.M)
        if pattern.search(out):
            out = pattern.sub(f"{key}={value}", out)
        else:
            out = out.rstrip() + f"\n{key}={value}\n"
    if out != text:
        with open(import_path, "w") as f:
            f.write(out)
        return True
    return False


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--dry-run", action="store_true")
    args = parser.parse_args()

    total_changed = 0
    for pattern, settings in POLICIES:
        changed = considered = 0
        for import_path in sorted(glob.glob(pattern + ".import")):
            considered += 1
            if args.dry_run:
                continue
            if patch_import(import_path, settings):
                changed += 1
        total_changed += changed
        print(f"[trim-textures] {pattern}: {changed}/{considered} updated {settings}")

    if not args.dry_run and total_changed:
        print("[trim-textures] run `godot --headless --path . --import` to rebuild ctex files")
    return 0


if __name__ == "__main__":
    sys.exit(main())
