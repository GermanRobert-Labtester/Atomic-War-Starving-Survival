#!/usr/bin/env python3
"""
generate-surface-placeholders.py — ASHFALL surface/exterior placeholder pack.

Companion to generate-shelter-placeholders.py. Generates deterministic,
clearly-labelled placeholder backdrops for the first-week (days 1-7) wasteland
surface: the hatch approach, the wide wasteland sky/horizon, and the expedition
departure threshold. Intact world only — no craters, no mushroom clouds.

Output (all under assets/sprites/Surface/):
  surface_hatch_approach_day1_7.png   1280x720
  wasteland_sky_day1_7.png            1920x1080
  expedition_departure_day1_7.png     1280x720
  PLACEHOLDER_MANIFEST.json

Re-run:
  python3 scripts/tools/generate-surface-placeholders.py
  bash scripts/ci/run-godot-bounded.sh --path . --import
"""

import importlib.util
import json
import math
import pathlib
import random
from PIL import Image, ImageDraw, ImageEnhance, ImageFilter

HERE = pathlib.Path(__file__).resolve().parent
REPO_ROOT = HERE.parent.parent

# Reuse the shelter pack's drawing helpers (the shelter script guards main()).
_spec = importlib.util.spec_from_file_location("shelter_gen", HERE / "generate-shelter-placeholders.py")
sg = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(sg)

D = sg.D
grain = sg.grain
font = sg.font
pictogram = sg.pictogram

OUT_DIR = REPO_ROOT / "assets" / "sprites" / "Surface"

OFFWHITE = sg.OFFWHITE
AMBER = sg.AMBER
STEEL = sg.STEEL

SKY_TOP = (54, 64, 78)
SKY_HORIZON = (156, 156, 148)
ASH = (168, 162, 150)
GROUND = (58, 56, 52)
GROUND_DARK = (40, 39, 37)
RIDGE_FAR = (108, 114, 120)
RIDGE_MID = (78, 82, 86)
RIDGE_NEAR = (54, 56, 58)


def stamp(img, d, label, w):
    """Bottom-left PLACEHOLDER stamp + scope line, consistent across the pack."""
    d.rectangle([24, 24, 246, 50], outline=AMBER, width=2)
    d.text((135, 37), "PLACEHOLDER ASSET", font=font(15, bold=True), fill=AMBER, anchor="mm")
    d.text((24, 58), label, font=font(14), fill=OFFWHITE, anchor="la")
    d.text((w - 24, 30), "PLACEHOLDER", font=font(15, bold=True), fill=STEEL, anchor="ra")
    d.text((w - 24, 50), "not final art", font=font(12), fill=STEEL, anchor="ra")


def sky(img, horizon_y, seed):
    d = D(ImageDraw.Draw(img))
    w, h = img.size
    for y in range(horizon_y):
        t = y / max(1, horizon_y)
        c = tuple(int(SKY_TOP[i] + (SKY_HORIZON[i] - SKY_TOP[i]) * t) for i in range(3))
        d.line([0, y, w, y], fill=c)
    # low sun
    sx, sy = int(w * 0.74), int(horizon_y * 0.42)
    d.ellipse([sx - 46, sy - 46, sx + 46, sy + 46], fill=(214, 206, 180))
    d.ellipse([sx - 70, sy - 70, sx + 70, sy + 70], outline=(188, 184, 170), width=2)
    # haze bands
    rnd = random.Random(seed)
    for _ in range(14):
        y = rnd.randint(int(horizon_y * 0.35), horizon_y - 4)
        d.line([0, y, w, y], fill=ASH, width=rnd.choice([1, 1, 2]))
    return d


def ridgeline(d, w, base_y, height, colour, seed, jitter=70):
    rnd = random.Random(seed)
    pts = [(0, base_y)]
    x = 0
    while x < w:
        x += rnd.randint(60, 140)
        y = base_y - rnd.randint(10, height)
        pts.append((x, y))
    pts.append((w, base_y))
    pts.append((w, base_y + jitter))
    pts.append((0, base_y + jitter))
    d.polygon(pts, fill=colour)


def wasteland_sky():
    w, h = 1920, 1080
    img = Image.new("RGBA", (w, h), SKY_HORIZON)
    horizon = int(h * 0.66)
    d = sky(img, horizon, 2027)
    ridgeline(d, w, horizon, 150, RIDGE_FAR, 11)
    ridgeline(d, w, horizon + 40, 190, RIDGE_MID, 22)
    ridgeline(d, w, horizon + 90, 120, RIDGE_NEAR, 33, jitter=90)

    # distant intact structures
    tx = 1480
    d.polygon([(tx, horizon - 150), (tx - 16, horizon - 10), (tx + 16, horizon - 10)], outline=RIDGE_NEAR, width=4)
    for i in range(1, 6):
        y = horizon - 20 - i * 22
        d.line([tx - (16 - i * 2), y, tx + (16 - i * 2), y], fill=RIDGE_NEAR, width=2)
    # water tower
    wx = 360
    d.rectangle([wx - 40, horizon - 190, wx + 40, horizon - 120], outline=RIDGE_NEAR, width=4)
    for lx in (wx - 26, wx + 26):
        d.line([lx, horizon - 120, wx + (lx - wx) * 3, horizon], fill=RIDGE_NEAR, width=3)

    # ground
    d.rectangle([0, horizon + 60, w, h], fill=GROUND)
    rnd = random.Random(44)
    for _ in range(600):
        x = rnd.randrange(w)
        y = rnd.randint(horizon + 70, h - 4)
        d.line([x, y, x, y - rnd.randint(2, 7)], fill=GROUND_DARK, width=1)
    for _ in range(60):
        x = rnd.randrange(w)
        y = rnd.randint(horizon + 80, h - 10)
        r = rnd.randint(3, 9)
        d.ellipse([x - r, y - r // 2, x + r, y + r // 2], fill=(70, 68, 62))

    stamp(img, d, "WASTELAND SURFACE // DAY 1–7 · HORIZON CLEAR · NO IMPACT SITES", w)
    return grain(_vignette(img, 120), 2027)


def hatch_approach():
    w, h = 1280, 720
    img = Image.new("RGBA", (w, h), SKY_HORIZON)
    horizon = int(h * 0.52)
    d = sky(img, horizon, 311)
    ridgeline(d, w, horizon, 90, RIDGE_FAR, 7)
    d.rectangle([0, horizon, w, h], fill=GROUND)

    # earth berm with concrete retaining wall
    cx = w // 2
    d.polygon([(cx - 360, horizon + 180), (cx + 360, horizon + 180), (cx + 430, h), (cx - 430, h)], fill=(64, 60, 54))
    d.rounded_rectangle([cx - 230, horizon - 10, cx + 230, horizon + 250], radius=10, fill=(96, 94, 88), outline=(70, 68, 63), width=4)
    # steel hatch
    d.rounded_rectangle([cx - 120, horizon + 30, cx + 120, horizon + 220], radius=8, fill=(74, 82, 90), outline=(46, 52, 58), width=5)
    d.line([cx, horizon + 30, cx, horizon + 220], fill=(46, 52, 58), width=4)
    d.ellipse([cx - 26, horizon + 106, cx + 26, horizon + 158], outline=(150, 156, 160), width=6)
    for a in range(0, 360, 60):
        rad = math.radians(a)
        d.line([cx + math.cos(rad) * 26, horizon + 132 + math.sin(rad) * 26,
                cx + math.cos(rad) * 42, horizon + 132 + math.sin(rad) * 42], fill=(150, 156, 160), width=4)
    # lamp on a pole
    lx = cx + 300
    d.line([lx, horizon + 40, lx, horizon - 90], fill=STEEL, width=5)
    d.polygon([(lx, horizon - 90), (lx - 18, horizon - 70), (lx + 18, horizon - 70)], fill=(120, 112, 86))
    # handrail
    for rx in (cx - 190, cx + 190):
        d.line([rx, horizon + 250, rx, horizon + 180], fill=STEEL, width=4)
    d.line([cx - 200, horizon + 180, cx + 200, horizon + 180], fill=STEEL, width=4)

    # paved apron
    d.polygon([(cx - 430, h), (cx + 430, h), (cx + 330, horizon + 250), (cx - 330, horizon + 250)], fill=(50, 50, 50))
    for i in range(1, 6):
        y = horizon + 250 + i * 38
        d.line([cx - 430 + i * 12, y, cx + 430 - i * 12, y], fill=(42, 42, 42), width=2)
    # footprints
    rnd = random.Random(99)
    for _ in range(26):
        fx = cx + rnd.randint(-180, 180)
        fy = horizon + 280 + rnd.randint(0, 180)
        d.ellipse([fx, fy, fx + 7, fy + 14], outline=(84, 82, 76), width=1)

    d.text((24, horizon + 300), "SURFACE ACCESS // REINFORCED HATCH · INTACT", font=font(16, bold=True), fill=OFFWHITE)
    stamp(img, d, "HATCH APPROACH // DAY 1–7 · SEALED · NO BREACH", w)
    return grain(_vignette(img, 110), 311)


def expedition_departure():
    w, h = 1280, 720
    img = Image.new("RGBA", (w, h), (26, 28, 32))
    d = D(ImageDraw.Draw(img))

    # exterior seen through the shelter mouth
    ox0, ox1, oy1 = int(w * 0.24), int(w * 0.80), int(h * 0.72)
    exc = Image.new("RGBA", (ox1 - ox0, oy1 - 40), SKY_HORIZON)
    ed = sky(exc, int((oy1 - 40) * 0.62), 501)
    ridgeline(ed, exc.width, int((oy1 - 40) * 0.62), 80, RIDGE_MID, 13)
    ed.rectangle([0, int((oy1 - 40) * 0.62), exc.width, exc.height], fill=GROUND)
    rnd = random.Random(77)
    for _ in range(220):
        x = rnd.randrange(exc.width)
        y = rnd.randint(int((oy1 - 40) * 0.64), exc.height - 2)
        ed.line([x, y, x, y - rnd.randint(2, 6)], fill=GROUND_DARK, width=1)
    img.paste(exc, (ox0, 40))

    # shelter mouth frame (interior concrete)
    d.rectangle([0, 0, w, 40], fill=(24, 26, 30))
    d.polygon([(0, 0), (ox0, 40), (ox0, oy1), (0, h)], fill=(38, 40, 44))
    d.polygon([(w, 0), (ox1, 40), (ox1, oy1), (w, h)], fill=(38, 40, 44))
    d.rectangle([0, oy1, w, h], fill=(34, 36, 40))
    d.line([ox0, 40, ox0, oy1], fill=STEEL, width=4)
    d.line([ox1, 40, ox1, oy1], fill=STEEL, width=4)

    # morning light shaft — drawn on its own layer so it blends over the exterior
    shaft = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    sd = D(ImageDraw.Draw(shaft))
    sd.polygon([(ox0, 40), (ox1, 40), (ox1 - 130, oy1 + 120), (ox0 + 70, oy1 + 120)],
               fill=(220, 200, 160, 46))
    img = Image.alpha_composite(img, shaft)
    d = D(ImageDraw.Draw(img))

    # staged kit in the foreground interior
    pictogram(d, "crate", 150, 560, 150, sg.RUST)
    pictogram(d, "barrel", 300, 580, 130, sg.AMBER)
    pictogram(d, "crate", 1120, 560, 140, sg.RUST)
    pictogram(d, "pipes", 1040, 600, 130, STEEL)
    # footprints leading out
    rnd2 = random.Random(15)
    for i in range(30):
        fx = w // 2 + rnd2.randint(-90, 90)
        fy = oy1 - 30 + i * 8
        d.ellipse([fx, fy, fx + 8, fy + 15], outline=(58, 56, 52), width=1)

    d.text((24, h - 96), "EXPEDITION DEPARTURE // MORNING LIGHT · ROUTE CLEAR", font=font(16, bold=True), fill=OFFWHITE)
    stamp(img, d, "DEPARTURE THRESHOLD // DAY 1–7 · INTACT SHELTER MOUTH", w)
    return grain(_vignette(img, 110), 501)


def _vignette(img, alpha):
    w, h = img.size
    vig = Image.new("L", (w, h), 0)
    ImageDraw.Draw(vig).ellipse([-w // 6, -h // 6, w + w // 6, h + h // 6], fill=255)
    vig = vig.filter(ImageFilter.GaussianBlur(90))
    dark = Image.new("RGBA", (w, h), (0, 0, 0, alpha))
    inv = vig.point(lambda v: 255 - v)  # edges opaque, centre transparent
    out = img.copy()
    out.paste(dark, (0, 0), inv)
    return out


def apply_phase_grade(img, phase):
    grades = {
        "day":   ((255, 255, 255), 0, 1.00),
        "dawn":  ((255, 176, 104), 44, 0.90),
        "dusk":  ((206, 116, 66), 78, 0.72),
        "night": ((56, 86, 166), 110, 0.50),
    }
    tint, alpha, bright = grades.get(phase, grades["day"])
    out = img if bright == 1.0 else ImageEnhance.Brightness(img).enhance(bright)
    if alpha > 0:
        out = Image.alpha_composite(out, Image.new("RGBA", out.size, tint + (alpha,)))
    return out


def main():
    OUT_DIR.mkdir(parents=True, exist_ok=True)
    bases = {
        "wasteland_sky": wasteland_sky(),
        "surface_hatch_approach": hatch_approach(),
        "expedition_departure": expedition_departure(),
    }

    written = []
    phases = (("day", "day1_7"), ("dawn", "dawn"), ("dusk", "dusk"), ("night", "night"))
    for stem, image in bases.items():
        for phase, suffix in phases:
            name = f"{stem}_{suffix}.png"
            apply_phase_grade(image, phase).save(OUT_DIR / name)
            written.append(name)

    manifest = {
        "schema_version": 1,
        "collection": "wasteland_surface_placeholders",
        "status": "PLACEHOLDER — NOT FINAL ART",
        "scope": "Intact wasteland surface, days 1-7 (no impact sites, craters, or mushroom clouds)",
        "generator": "scripts/tools/generate-surface-placeholders.py",
        "generated_by": "Pillow, programmatic line art",
        "lighting_phases": ["day1_7", "dawn", "dusk", "night"],
        "license": "MIT",
        "replace_with_final_art": True,
        "files": [
            {"file": f, "placeholder": True, "label_in_image": "PLACEHOLDER"}
            for f in written
        ],
    }
    (OUT_DIR / "PLACEHOLDER_MANIFEST.json").write_text(json.dumps(manifest, indent=2) + "\n", encoding="utf-8")

    print(f"wrote {len(written)} placeholder backdrops to {OUT_DIR.relative_to(REPO_ROOT)}")
    for stem, image in bases.items():
        print(f"  {stem}_<phase>.png  {image.size[0]}x{image.size[1]}  x4 phases  [PLACEHOLDER]")
    print("  PLACEHOLDER_MANIFEST.json               labels every file")


if __name__ == "__main__":
    main()
