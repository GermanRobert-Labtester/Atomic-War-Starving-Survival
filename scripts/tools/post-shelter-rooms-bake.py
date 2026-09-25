#!/usr/bin/env python3
# SPDX-License-Identifier: MIT
"""ASHFALL — post-process the room pictogram / prop / tile bakes.

Companion to bake-shelter-rooms.py (Blender). Does two jobs:

  1. Sprites (25): crop each 256x256 raw render to its alpha bbox, fit into
     a 116px box, center on a 128x128 transparent canvas — exactly the flow
     that produced the three shipped stage props — and write
     assets/sprites/Shelter/{room_*,prop_ceiling_lamp,prop_pipe_bundle}.png.

  2. Tiles (3): generate tile_{wall_concrete,floor_concrete,floor_grate}.png
     128x128 RGB, seamless by construction (wrapped value noise / periodic
     geometry; no runtime consumer today, so any future tiling is safe).
     Deterministic: random.Random(20260926), no wall-clock or hash order.

--check verifies all 28 finals already on disk (size, mode, transparency,
non-flatness, tile seam deltas) without writing.

Usage:
  python3 scripts/tools/post-shelter-rooms-bake.py [--raw-dir artifacts/shelter-bake-rooms/raw] [--check]
"""

import math
import random
import sys

from PIL import Image

RAW_DIR = "artifacts/shelter-bake-rooms/raw"
FINAL_DIR = "assets/sprites/Shelter"
SEED = 20260926

ROOM_IDS = (
    "room_airlock", "room_armory_munitions", "room_bunker_corridor",
    "room_bunks", "room_bunks_crowded", "room_clinic",
    "room_common_mess_hall", "room_quarters_private", "room_filtration",
    "room_generator", "room_greenhouse_shelter", "room_hope_beacon",
    "room_kitchen", "room_laboratory_research", "room_radio_tuner",
    "room_reading_quiet_room", "room_storage_bay", "room_storage_secure",
    "room_ward_clinical", "room_ward_quarantine", "room_workshop",
    "room_workshop_heavy", "room_workshop_precision",
)
PROP_IDS = ("prop_ceiling_lamp", "prop_pipe_bundle")
TILE_IDS = ("tile_wall_concrete", "tile_floor_concrete", "tile_floor_grate")
SPRITE_IDS = ROOM_IDS + PROP_IDS


def process_sprite(name):
    img = Image.open(f"{RAW_DIR}/{name}_raw.png").convert("RGBA")
    bbox = img.getbbox()
    if bbox:
        img = img.crop(bbox)
    img.thumbnail((116, 116), Image.LANCZOS)
    canvas = Image.new("RGBA", (128, 128), (0, 0, 0, 0))
    canvas.paste(img, ((128 - img.width) // 2, (128 - img.height) // 2), img)
    canvas.save(f"{FINAL_DIR}/{name}.png", optimize=True)
    return canvas.size


# ── seamless tiles ─────────────────────────────────────────────────────────
def _smooth(t):
    return t * t * (3.0 - 2.0 * t)


def wrapped_noise(size, grid, rng):
    """One octave of periodic value noise on size x size, lattice grid x grid."""
    lat = [[rng.random() for _ in range(grid)] for _ in range(grid)]
    out = [[0.0] * size for _ in range(size)]
    step = size / grid
    for y in range(size):
        gy = y / step
        y0 = int(gy) % grid
        y1 = (y0 + 1) % grid
        ty = _smooth(gy - math.floor(gy))
        for x in range(size):
            gx = x / step
            x0 = int(gx) % grid
            x1 = (x0 + 1) % grid
            tx = _smooth(gx - math.floor(gx))
            a = lat[y0][x0] * (1 - tx) + lat[y0][x1] * tx
            b = lat[y1][x0] * (1 - tx) + lat[y1][x1] * tx
            out[y][x] = a * (1 - ty) + b * ty
    return out


def fbm(size, octaves, rng):
    """Periodic fractal noise; every octave lattice divides the period."""
    acc = [[0.0] * size for _ in range(size)]
    amp, total = 1.0, 0.0
    for grid in octaves:
        layer = wrapped_noise(size, grid, rng)
        for y in range(size):
            row = acc[y]
            lay = layer[y]
            for x in range(size):
                row[x] += lay[x] * amp
        total += amp
        amp *= 0.5
    return [[v / total for v in row] for row in acc]


def _wrap_blob(px, size, cx, cy, r, color, alpha):
    """Soft wrapped circular stain — writes on all 9 periodic copies."""
    for dy in (-size, 0, size):
        for dx in (-size, 0, size):
            bx, by = cx + dx, cy + dy
            for y in range(int(by - r) - 1, int(by + r) + 2):
                if y < 0 or y >= size:
                    continue
                for x in range(int(bx - r) - 1, int(bx + r) + 2):
                    if x < 0 or x >= size:
                        continue
                    d = math.hypot(x - bx, y - by) / r
                    if d < 1.0:
                        w = (1.0 - d * d) * alpha
                        r0, g0, b0 = px[x, y]
                        px[x, y] = (int(r0 + (color[0] - r0) * w),
                                    int(g0 + (color[1] - g0) * w),
                                    int(b0 + (color[2] - b0) * w))


def make_tile_wall_concrete():
    rng = random.Random(SEED)
    n = fbm(128, (4, 8, 16), rng)
    img = Image.new("RGB", (128, 128))
    px = img.load()
    base = (0x24, 0x27, 0x2E)          # DESIGN.md charcoal wall family
    for y in range(128):
        for x in range(128):
            d = (n[y][x] - 0.5) * 26
            px[x, y] = tuple(max(0, min(255, int(c + d * k)))
                             for c, k in zip(base, (0.9, 1.0, 1.15)))
    for _ in range(6):                  # moisture stains, wrapped
        _wrap_blob(px, 128, rng.randrange(128), rng.randrange(128),
                   rng.randrange(14, 30), (0x17, 0x19, 0x1E), 0.5)
    for _ in range(40):                 # fine pits
        x, y = rng.randrange(128), rng.randrange(128)
        r0, g0, b0 = px[x, y]
        px[x, y] = (max(0, r0 - 10), max(0, g0 - 10), max(0, b0 - 8))
    return img


def make_tile_floor_concrete():
    rng = random.Random(SEED + 1)
    n = fbm(128, (4, 8, 16, 32), rng)
    img = Image.new("RGB", (128, 128))
    px = img.load()
    base = (0x30, 0x35, 0x3F)          # stage floor family
    for y in range(128):
        for x in range(128):
            d = (n[y][x] - 0.5) * 20
            px[x, y] = tuple(max(0, min(255, int(c + d * k)))
                             for c, k in zip(base, (0.95, 1.0, 1.1)))
    for _ in range(4):                  # worn walk patches (wrapped, faint)
        _wrap_blob(px, 128, rng.randrange(128), rng.randrange(128),
                   rng.randrange(18, 34), (0x3C, 0x42, 0x4D), 0.35)
    for _ in range(70):                 # aggregate specks
        x, y = rng.randrange(128), rng.randrange(128)
        r0, g0, b0 = px[x, y]
        if rng.random() < 0.5:
            px[x, y] = (min(255, r0 + 9), min(255, g0 + 9), min(255, b0 + 8))
        else:
            px[x, y] = (max(0, r0 - 9), max(0, g0 - 9), max(0, b0 - 8))
    return img


def make_tile_floor_grate():
    img = Image.new("RGB", (128, 128))
    px = img.load()
    period, bar = 32, 8                 # 4 cells per side: fully periodic
    hole, bar_rgb, edge = (0x1A, 0x1D, 0x22), (0x3C, 0x42, 0x4C), (0x48, 0x4E, 0x58)
    for y in range(128):
        fy = y % period
        for x in range(128):
            fx = x % period
            # jitter derived only from the in-cell offset so every 32px
            # cell is byte-identical (exact periodicity, provable in --check)
            n = ((fx * 7 + fy * 13) % 7) - 3
            if fx < bar or fy < bar:
                c = edge if (fx == 0 or fy == 0) else bar_rgb
            else:
                c = hole
            px[x, y] = tuple(max(0, min(255, v + n)) for v in c)
    return img


TILE_MAKERS = {
    "tile_wall_concrete": make_tile_wall_concrete,
    "tile_floor_concrete": make_tile_floor_concrete,
    "tile_floor_grate": make_tile_floor_grate,
}


# ── verification ───────────────────────────────────────────────────────────
def _luma(p):
    return 0.299 * p[0] + 0.587 * p[1] + 0.114 * p[2]


def tile_seam_delta(img):
    px = img.load()
    w, h = img.size
    dv = sum(abs(_luma(px[0, y]) - _luma(px[w - 1, y])) for y in range(h)) / h
    dh = sum(abs(_luma(px[x, 0]) - _luma(px[x, h - 1])) for x in range(w)) / w
    return dv, dh


def check():
    ok = True
    for name in SPRITE_IDS:
        p = f"{FINAL_DIR}/{name}.png"
        try:
            img = Image.open(p)
            assert img.size == (128, 128), f"{p}: {img.size}"
            assert img.mode == "RGBA", f"{p}: {img.mode}"
            alpha = img.getchannel("A").getextrema()
            assert alpha[0] < 128, f"{p}: no transparency"
            colors = img.convert("RGB").getcolors(maxcolors=1 << 16)
            assert colors and len(colors) > 32, f"{p}: suspiciously flat"
            print(f"OK  {p} alpha_range={alpha} colors={len(colors)}")
        except Exception as ex:
            ok = False
            print(f"FAIL {p}: {ex}")
    for name in TILE_IDS:
        p = f"{FINAL_DIR}/{name}.png"
        try:
            img = Image.open(p).convert("RGB")
            assert img.size == (128, 128), f"{p}: {img.size}"
            colors = img.getcolors(maxcolors=1 << 16)
            assert colors and len(colors) >= 8, f"{p}: suspiciously flat"
            if name == "tile_floor_grate":
                # exact periodicity proof for the geometric tile: every
                # 32px cell column/row must match the first (bar/hole
                # contrast across the wrap is the texture, not a seam)
                px = img.load()
                for off in (32, 64, 96):
                    for y in range(128):
                        assert px[0, y] == px[off, y], f"{p}: col {off} != col 0"
                        assert px[y, 0] == px[y, off], f"{p}: row {off} != row 0"
                print(f"OK  {p} colors={len(colors)} periodicity=exact(period 32)")
            else:
                dv, dh = tile_seam_delta(img)
                assert dv < 10 and dh < 10, f"{p}: seam deltas dv={dv:.1f} dh={dh:.1f}"
                print(f"OK  {p} colors={len(colors)} seam dv={dv:.2f} dh={dh:.2f}")
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
    for name in SPRITE_IDS:
        print(f"[post] {name}: {process_sprite(name)}")
    for name, maker in TILE_MAKERS.items():
        img = maker()
        img.save(f"{FINAL_DIR}/{name}.png", optimize=True)
        dv, dh = tile_seam_delta(img)
        print(f"[post] {name}: {img.size} seam dv={dv:.2f} dh={dh:.2f}")


main()
