#!/usr/bin/env python3
"""Generate a small deterministic ASHFALL visual starter pack with Pillow.

The pack is intentionally engine-neutral source art: icons, transparent decals,
seamless material tiles, and small FX sprites.  It writes into the active
Godot tree and keeps a manifest beside the outputs so the relationship between
source and runtime art stays obvious.

Usage:
    python3 scripts/tools/generate-visual-pack.py
    python3 scripts/tools/generate-visual-pack.py --check

Godot import sidecars are produced by ``launch.sh --generate-visuals`` when
Godot is available.  No network or non-deterministic source is used.
"""

from __future__ import annotations

import argparse
import json
import math
import random
from pathlib import Path
from typing import Iterable

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[2]
OUTPUT_DIR = REPO_ROOT / "assets" / "sprites" / "Generated"
MANIFEST_PATH = OUTPUT_DIR / "ASHFALL_VISUAL_PACK_MANIFEST.json"
SEED = 20260913

INK = (11, 14, 17, 255)
CHARCOAL = (25, 30, 34, 255)
CONCRETE = (79, 86, 87, 255)
CONCRETE_LIGHT = (119, 119, 108, 255)
ASH = (151, 147, 132, 255)
BRASS = (195, 157, 91, 255)
AMBER = (229, 183, 84, 255)
TEAL = (83, 139, 141, 255)
RUST = (143, 67, 49, 255)
PAPER = (198, 188, 157, 255)


def font(size: int, bold: bool = False) -> ImageFont.FreeTypeFont | ImageFont.ImageFont:
    filename = "BarlowCondensed-Bold.ttf" if bold else "ShareTechMono-Regular.ttf"
    path = REPO_ROOT / "assets" / "fonts" / filename
    try:
        return ImageFont.truetype(str(path), size)
    except OSError:
        return ImageFont.load_default()


def clamp(value: float, low: float = 0.0, high: float = 255.0) -> int:
    return int(max(low, min(high, round(value))))


def periodic_noise(x: int, y: int, width: int, height: int, phase: float) -> float:
    """Low-cost tileable noise made from periodic bands, in the range -1..1."""
    # Include both endpoints so the final pixel is identical to the first
    # pixel when adjacent copies of the PNG meet.
    u = 2.0 * math.pi * x / max(1, width - 1)
    v = 2.0 * math.pi * y / max(1, height - 1)
    value = (
        0.52 * math.sin(u * 2.0 + phase)
        + 0.31 * math.sin(v * 3.0 - phase * 0.7)
        + 0.17 * math.sin((u + v) * 5.0 + phase * 1.3)
    )
    return value


def torus_distance(x: int, y: int, cx: int, cy: int, width: int, height: int) -> float:
    dx = min(abs(x - cx), width - abs(x - cx))
    dy = min(abs(y - cy), height - abs(y - cy))
    return math.sqrt(dx * dx + dy * dy)


def add_grain(image: Image.Image, seed: int, count: int = 700, alpha: int = 28, margin: int = 0) -> None:
    rnd = random.Random(seed)
    overlay = Image.new("RGBA", image.size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(overlay)
    width, height = image.size
    x_range = range(margin, max(margin + 1, width - margin))
    y_range = range(margin, max(margin + 1, height - margin))
    for _ in range(count):
        x = rnd.choice(x_range)
        y = rnd.choice(y_range)
        tone = 255 if rnd.random() > 0.5 else 0
        draw.point((x, y), fill=(tone, tone, tone, rnd.randrange(5, alpha + 1)))
    image.alpha_composite(overlay)


def save_png(image: Image.Image, filename: str, kind: str, size: tuple[int, int]) -> dict:
    path = OUTPUT_DIR / filename
    image.save(path, format="PNG", optimize=True)
    return {
        "file": filename,
        "kind": kind,
        "dimensions": list(size),
        "runtime_path": f"res://assets/sprites/Generated/{filename}",
        "source": "scripts/tools/generate-visual-pack.py",
    }


def frame(draw: ImageDraw.ImageDraw, size: int, accent: tuple[int, int, int, int]) -> None:
    draw.rounded_rectangle((3, 3, size - 4, size - 4), radius=10, fill=CHARCOAL, outline=ASH, width=1)
    draw.rounded_rectangle((7, 7, size - 8, size - 8), radius=7, outline=accent, width=2)
    for x, y, sx, sy in ((10, 10, 17, 10), (size - 10, 10, size - 17, 10),
                         (10, size - 10, 17, size - 10), (size - 10, size - 10, size - 17, size - 10)):
        draw.line((x, y, sx, sy), fill=AMBER, width=2)


def draw_radiation(draw: ImageDraw.ImageDraw, cx: int, cy: int, radius: int, color: tuple) -> None:
    draw.ellipse((cx - 8, cy - 8, cx + 8, cy + 8), outline=color, width=3)
    for angle in (-math.pi / 2, math.pi / 6, 5 * math.pi / 6):
        start = angle - math.pi / 6
        end = angle + math.pi / 6
        box = (cx - radius, cy - radius, cx + radius, cy + radius)
        draw.arc(box, math.degrees(start), math.degrees(end), fill=color, width=7)
    draw.ellipse((cx - radius, cy - radius, cx + radius, cy + radius), outline=TEAL, width=1)


def generate_icon(name: str, accent: tuple[int, int, int, int], glyph: str) -> Image.Image:
    size = 96
    image = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    frame(draw, size, accent)
    cx, cy = size // 2, size // 2
    if glyph == "radiation":
        draw_radiation(draw, cx, cy, 27, accent)
    elif glyph == "water":
        draw.polygon([(cx, cy - 28), (cx - 19, cy + 5), (cx - 17, cy + 18),
                      (cx, cy + 28), (cx + 17, cy + 18), (cx + 19, cy + 5)],
                     outline=accent, width=3)
        draw.arc((cx - 10, cy - 2, cx + 10, cy + 18), 200, 350, fill=TEAL, width=3)
    elif glyph == "power":
        draw.polygon([(cx + 7, cy - 30), (cx - 13, cy + 3), (cx - 1, cy + 3),
                      (cx - 8, cy + 31), (cx + 17, cy - 7), (cx + 4, cy - 7)], fill=accent)
        draw.line((cx - 25, cy + 25, cx + 25, cy - 25), fill=TEAL, width=1)
    elif glyph == "route":
        draw.ellipse((cx - 25, cy - 25, cx + 25, cy + 25), outline=accent, width=2)
        draw.line((cx - 19, cy + 19, cx - 4, cy + 4, cx + 5, cy + 11, cx + 20, cy - 17), fill=accent, width=3)
        draw.polygon([(cx + 20, cy - 17), (cx + 11, cy - 14), (cx + 18, cy - 7)], fill=AMBER)
        draw.ellipse((cx - 7, cy - 7, cx + 7, cy + 7), fill=CHARCOAL, outline=TEAL, width=2)
    elif glyph == "medical":
        draw.rounded_rectangle((cx - 10, cy - 28, cx + 10, cy + 28), radius=3, fill=accent)
        draw.rounded_rectangle((cx - 28, cy - 10, cx + 28, cy + 10), radius=3, fill=accent)
        draw.line((cx - 29, cy + 31, cx + 29, cy - 31), fill=TEAL, width=2)
    add_grain(image, SEED + len(name), count=320, alpha=18)
    return image


def generate_warning_decal() -> Image.Image:
    size = 192
    image = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    rnd = random.Random(SEED + 40)
    for _ in range(90):
        x = rnd.randrange(20, size - 20)
        y = rnd.randrange(20, size - 20)
        radius = rnd.randrange(2, 11)
        draw.ellipse((x - radius, y - radius, x + radius, y + radius), fill=(*RUST[:3], rnd.randrange(18, 70)))
    draw.regular_polygon((size // 2, size // 2, 68), 3, rotation=180, fill=(*AMBER[:3], 34), outline=AMBER, width=5)
    draw_radiation(draw, size // 2, size // 2 + 3, 42, AMBER)
    draw.line((22, 153, 172, 39), fill=(*RUST[:3], 120), width=5)
    add_grain(image, SEED + 41, count=500, alpha=45)
    return image


def generate_repair_decal() -> Image.Image:
    size = 192
    image = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    draw.polygon([(36, 49), (140, 31), (166, 69), (146, 151), (53, 164), (25, 117)],
                 fill=(*BRASS[:3], 150), outline=ASH, width=3)
    for x, y in ((48, 63), (145, 50), (45, 135), (143, 135)):
        draw.ellipse((x - 7, y - 7, x + 7, y + 7), fill=CHARCOAL, outline=TEAL, width=2)
    draw.line((62, 110, 128, 90), fill=PAPER, width=5)
    draw.line((95, 72, 94, 130), fill=RUST, width=4)
    add_grain(image, SEED + 42, count=460, alpha=36)
    return image


def generate_material_tile(material: str, seed_offset: int) -> Image.Image:
    width = height = 256
    image = Image.new("RGBA", (width, height), INK)
    pixels = image.load()
    rnd = random.Random(SEED + seed_offset)
    for y in range(height):
        for x in range(width):
            n = periodic_noise(x, y, width, height, seed_offset * 0.41)
            if material == "concrete":
                base = 82 + n * 24
                color = (clamp(base), clamp(base + 3), clamp(base + 1), 255)
            else:
                rust = max(0.0, n * 0.7 + 0.34)
                color = (clamp(45 + rust * 100), clamp(48 + rust * 35), clamp(44 + rust * 16), 255)
            pixels[x, y] = color

    draw = ImageDraw.Draw(image, "RGBA")
    if material == "concrete":
        for _ in range(24):
            radius = rnd.randrange(8, 34)
            cx = rnd.randrange(radius, width - radius + 1)
            cy = rnd.randrange(radius, height - radius + 1)
            alpha = rnd.randrange(12, 34)
            draw.ellipse((cx - radius, cy - radius, cx + radius, cy + radius), fill=(*ASH[:3], alpha))
        for offset in (24, 112, 204):
            draw.line((2, offset, 254, offset + 3), fill=(*INK[:3], 70), width=2)
            draw.line((offset, 2, offset + 2, 254), fill=(*CONCRETE_LIGHT[:3], 32), width=1)
    else:
        for _ in range(21):
            radius = rnd.randrange(10, 40)
            cx = rnd.randrange(radius, width - radius + 1)
            cy = rnd.randrange(radius, height - radius + 1)
            alpha = rnd.randrange(25, 75)
            draw.ellipse((cx - radius, cy - radius, cx + radius, cy + radius), fill=(*RUST[:3], alpha))
        for x in (18, 70, 137, 211):
            draw.line((x, 2, x + rnd.randrange(-8, 9), 254), fill=(*CHARCOAL[:3], 90), width=rnd.randrange(2, 5))
            draw.line((x + 5, 2, x + rnd.randrange(-5, 10), 254), fill=(*BRASS[:3], 42), width=1)

    # Keep a clean one-pixel border so the periodic base and overlays tile
    # without a visible seam when the texture repeats.
    add_grain(image, SEED + seed_offset, count=2200, alpha=34, margin=2)
    return image


def generate_fx(kind: str, seed_offset: int) -> Image.Image:
    size = 128
    image = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    draw = ImageDraw.Draw(image, "RGBA")
    rnd = random.Random(SEED + seed_offset)
    if kind == "ash":
        for _ in range(78):
            x = rnd.randrange(8, 120)
            y = rnd.randrange(8, 120)
            radius = rnd.choice((1, 1, 2, 3))
            draw.ellipse((x - radius, y - radius, x + radius, y + radius), fill=(*ASH[:3], rnd.randrange(40, 170)))
    elif kind == "steam":
        for index in range(10):
            x = 24 + index * 9 + rnd.randrange(-4, 5)
            y = 95 - index * 6 + rnd.randrange(-5, 6)
            radius = 10 + rnd.randrange(-3, 6)
            draw.ellipse((x - radius, y - radius, x + radius, y + radius), fill=(*PAPER[:3], 22), outline=(*ASH[:3], 36), width=2)
    elif kind == "spark":
        cx, cy = 64, 68
        for angle, length in ((-1.5, 43), (-0.7, 32), (0.1, 46), (1.0, 36), (2.2, 30)):
            ex = cx + math.cos(angle) * length
            ey = cy + math.sin(angle) * length
            draw.line((cx, cy, ex, ey), fill=AMBER, width=3)
            draw.line((cx, cy, ex, ey), fill=PAPER, width=1)
        draw.ellipse((cx - 7, cy - 7, cx + 7, cy + 7), fill=(*AMBER[:3], 180))
    add_grain(image, SEED + seed_offset + 100, count=150, alpha=26)
    return image


def generate_preview(entries: list[dict]) -> Image.Image:
    width, height = 960, 600
    image = Image.new("RGBA", (width, height), INK)
    draw = ImageDraw.Draw(image)
    draw.rectangle((18, 18, width - 18, height - 18), outline=BRASS, width=2)
    draw.text((42, 34), "ASHFALL / PROCEDURAL VISUAL STARTER PACK", fill=AMBER, font=font(25, bold=True))
    draw.text((44, 68), "LOCAL PILLOW SOURCE  •  DETERMINISTIC SEED 20260913", fill=ASH, font=font(14))

    by_file = {entry["file"]: entry for entry in entries}
    panels: Iterable[tuple[str, str, int, int, int, int]] = (
        ("icons", "ashfall_icon_radiation.png", 46, 120, 150, 150),
        ("icons", "ashfall_icon_water.png", 216, 120, 150, 150),
        ("icons", "ashfall_icon_power.png", 386, 120, 150, 150),
        ("icons", "ashfall_icon_route.png", 556, 120, 150, 150),
        ("icons", "ashfall_icon_medical.png", 726, 120, 150, 150),
        ("decals", "ashfall_decal_warning_trefoil.png", 74, 325, 150, 150),
        ("decals", "ashfall_decal_repair_patch.png", 244, 325, 150, 150),
        ("materials", "ashfall_texture_concrete_tile.png", 414, 325, 150, 150),
        ("materials", "ashfall_texture_rust_sheet.png", 584, 325, 150, 150),
        ("fx", "ashfall_fx_ash_drift.png", 754, 325, 72, 72),
        ("fx", "ashfall_fx_steam_puff.png", 754, 420, 72, 72),
        ("fx", "ashfall_fx_spark.png", 842, 372, 72, 72),
    )
    for label, filename, x, y, box_w, box_h in panels:
        draw.rectangle((x - 8, y - 8, x + box_w + 8, y + box_h + 24), fill=CHARCOAL, outline=(55, 62, 62, 255), width=1)
        source = Image.open(OUTPUT_DIR / filename).convert("RGBA")
        source.thumbnail((box_w, box_h), Image.Resampling.LANCZOS)
        image.alpha_composite(source, (x + (box_w - source.width) // 2, y + (box_h - source.height) // 2))
        draw.text((x - 2, y + box_h + 2), label.upper(), fill=TEAL, font=font(11, bold=True))
    return image


def build() -> list[dict]:
    OUTPUT_DIR.mkdir(parents=True, exist_ok=True)
    entries: list[dict] = []
    icons = (
        ("ashfall_icon_radiation.png", "radiation", AMBER, "radiation"),
        ("ashfall_icon_water.png", "water", TEAL, "water"),
        ("ashfall_icon_power.png", "power", AMBER, "power"),
        ("ashfall_icon_route.png", "route", BRASS, "route"),
        ("ashfall_icon_medical.png", "medical", PAPER, "medical"),
    )
    for filename, kind, accent, glyph in icons:
        entries.append(save_png(generate_icon(kind, accent, glyph), filename, "icon", (96, 96)))

    entries.append(save_png(generate_warning_decal(), "ashfall_decal_warning_trefoil.png", "decal", (192, 192)))
    entries.append(save_png(generate_repair_decal(), "ashfall_decal_repair_patch.png", "decal", (192, 192)))
    entries.append(save_png(generate_material_tile("concrete", 61), "ashfall_texture_concrete_tile.png", "seamless_material", (256, 256)))
    entries.append(save_png(generate_material_tile("rust", 62), "ashfall_texture_rust_sheet.png", "seamless_material", (256, 256)))
    entries.append(save_png(generate_fx("ash", 71), "ashfall_fx_ash_drift.png", "fx_sprite", (128, 128)))
    entries.append(save_png(generate_fx("steam", 72), "ashfall_fx_steam_puff.png", "fx_sprite", (128, 128)))
    entries.append(save_png(generate_fx("spark", 73), "ashfall_fx_spark.png", "fx_sprite", (128, 128)))
    preview = generate_preview(entries)
    entries.append(save_png(preview, "ashfall_visual_pack_preview.png", "preview_sheet", (960, 600)))
    for entry in entries:
        entry["consumer"] = {
            "icon": "HUD/resource or status indicators",
            "decal": "world props, warning signage, repair overlays",
            "seamless_material": "tileable shelter/surface material layers",
            "fx_sprite": "ash, condensation, and electrical FX",
            "preview_sheet": "art review only",
        }[entry["kind"]]

    manifest = {
        "schema_version": 1,
        "collection": "ashfall_procedural_visual_starter_pack",
        "status": "PROCEDURAL STARTER ART",
        "generated_by": "Pillow, deterministic Python line art and textures",
        "generator": "scripts/tools/generate-visual-pack.py",
        "seed": SEED,
        "license": "MIT",
        "godot_import": "Run ./launch.sh --generate-visuals to refresh PNGs and import sidecars",
        "files": entries,
    }
    MANIFEST_PATH.write_text(json.dumps(manifest, indent=2) + "\n", encoding="utf-8")
    return entries


def check() -> int:
    if not MANIFEST_PATH.is_file():
        print(f"MISSING: {MANIFEST_PATH}")
        return 1
    manifest = json.loads(MANIFEST_PATH.read_text(encoding="utf-8"))
    failures: list[str] = []
    for entry in manifest.get("files", []):
        path = OUTPUT_DIR / entry["file"]
        if not path.is_file():
            failures.append(f"missing {entry['file']}")
            continue
        try:
            with Image.open(path) as image:
                actual = list(image.size)
                expected = entry["dimensions"]
                if actual != expected:
                    failures.append(f"{entry['file']}: expected {expected}, found {actual}")
                image.verify()
        except Exception as exc:  # Pillow raises format-specific exceptions.
            failures.append(f"{entry['file']}: {exc}")
    if failures:
        print("VISUAL_PACK_CHECK FAIL")
        for failure in failures:
            print(f"- {failure}")
        return 1
    print(f"VISUAL_PACK_CHECK PASS ({len(manifest.get('files', []))} files, seed {manifest.get('seed')})")
    return 0


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--check", action="store_true", help="validate the existing pack without writing files")
    args = parser.parse_args()
    if args.check:
        return check()
    entries = build()
    print(f"wrote {len(entries)} procedural visual files to {OUTPUT_DIR.relative_to(REPO_ROOT)}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
