#!/usr/bin/env python3
# SPDX-License-Identifier: MIT
"""ASHFALL — post-process the shelter stage bakes (Pillow, deterministic).

Companion to bake-shelter-stage.py (Blender). Blender 5.2 in this environment
falls back to "color management disabled" (broken OCIO config), so raw PNGs
carry scene-linear values. This script:

  1. converts linear -> sRGB properly,
  2. applies a per-phase levels grade toward the DESIGN.md charcoal palette,
  3. slightly desaturates (restrained, cold look),
  4. writes the final 760x420 backdrops and 128x128 prop sprites into
     assets/sprites/Shelter/ using the exact filenames the runtime consumes.

Usage:
  python3 scripts/tools/post-shelter-bake.py [--raw-dir artifacts/shelter-bake/raw]
                                             [--check]
--check verifies the finals already on disk (size + not placeholder-flat)
without writing.
"""

import sys

from PIL import Image

RAW_DIR = "artifacts/shelter-bake/raw"
FINAL_DIR = "assets/sprites/Shelter"

# Per-phase gentle levels (black/white in 0..1). The raw PNGs from this
# Blender build already land in the display-referred range (wall ~ #3C4046),
# so the grade only crushes slightly toward the DESIGN.md charcoal family —
# no linear->sRGB LUT is applied (it would double-brighten).
BACKDROPS = {
    "shelter_interior_day1_7": dict(black=0.030, white=0.97, gamma=1.0),
    "shelter_interior_dawn":   dict(black=0.028, white=0.95, gamma=1.0),
    "shelter_interior_dusk":   dict(black=0.025, white=0.93, gamma=1.0),
    "shelter_interior_night":  dict(black=0.020, white=0.88, gamma=1.0),
}

PROPS = ("prop_supply_crate", "prop_water_barrel", "prop_hatch_door")
DESAT = 0.14  # fraction of luma mixed in


def lin_to_srgb_lut():
    lut = []
    for i in range(256):
        c = i / 255.0
        s = c / 12.92 if c <= 0.0031308 else 1.055 * (c ** (1 / 2.4)) - 0.055
        lut.append(max(0, min(255, round(s * 255))))
    return lut


LUT = lin_to_srgb_lut()


def grade_levels(l, black, white, gamma):
    v = (l - black) / max(1e-6, white - black)
    v = max(0.0, min(1.0, v)) ** gamma
    return v


def process_backdrop(name, cfg):
    img = Image.open(f"{RAW_DIR}/{name}_raw.png").convert("RGB")
    px = img.load()
    w, h = img.size
    for y in range(h):
        for x in range(w):
            r, g, b = px[x, y]
            lr = grade_levels(r / 255.0, cfg["black"], cfg["white"], cfg["gamma"])
            lg = grade_levels(g / 255.0, cfg["black"], cfg["white"], cfg["gamma"])
            lb = grade_levels(b / 255.0, cfg["black"], cfg["white"], cfg["gamma"])
            luma = 0.299 * lr + 0.587 * lg + 0.114 * lb
            mix = lambda c: c + (luma - c) * DESAT
            px[x, y] = (int(mix(lr) * 255 + 0.5), int(mix(lg) * 255 + 0.5),
                        int(mix(lb) * 255 + 0.5))
    img = img.resize((760, 420), Image.LANCZOS)
    img.save(f"{FINAL_DIR}/{name}.png", optimize=True)
    return img.size


def process_prop(name):
    img = Image.open(f"{RAW_DIR}/{name}_raw.png").convert("RGBA")
    bbox = img.getbbox()
    if bbox:
        img = img.crop(bbox)
    # fit into a 116px box on a 128x128 transparent canvas
    img.thumbnail((116, 116), Image.LANCZOS)
    canvas = Image.new("RGBA", (128, 128), (0, 0, 0, 0))
    canvas.paste(img, ((128 - img.width) // 2, (128 - img.height) // 2), img)
    canvas.save(f"{FINAL_DIR}/{name}.png", optimize=True)
    return canvas.size


def check():
    ok = True
    for name in BACKDROPS:
        p = f"{FINAL_DIR}/{name}.png"
        try:
            img = Image.open(p)
            assert img.size == (760, 420), f"{p}: {img.size}"
            colors = img.convert("RGB").getcolors(maxcolors=1 << 16)
            assert colors and len(colors) > 64, f"{p}: suspiciously flat"
            print(f"OK  {p} {img.size} {len(colors)} colors")
        except Exception as ex:
            ok = False
            print(f"FAIL {p}: {ex}")
    for name in PROPS:
        p = f"{FINAL_DIR}/{name}.png"
        try:
            img = Image.open(p)
            assert img.size == (128, 128), f"{p}: {img.size}"
            assert img.mode == "RGBA", f"{p}: {img.mode}"
            alpha = img.getchannel("A").getextrema()
            assert alpha[0] < 128, f"{p}: no transparency"
            print(f"OK  {p} {img.size} alpha_range={alpha}")
        except Exception as ex:
            ok = False
            print(f"FAIL {p}: {ex}")
    return 0 if ok else 1


def main():
    args = sys.argv[1:]
    if "--check" in args:
        sys.exit(check())
    if "--raw-dir" in args:
        global RAW_DIR
        RAW_DIR = args[args.index("--raw-dir") + 1]
    for name, cfg in BACKDROPS.items():
        print(f"[post] {name}: {process_backdrop(name, cfg)}")
    for name in PROPS:
        print(f"[post] {name}: {process_prop(name)}")


main()
