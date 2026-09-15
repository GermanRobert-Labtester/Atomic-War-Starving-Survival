#!/usr/bin/env python3
"""
generate-shelter-placeholders.py — ASHFALL shelter placeholder art pack.

Generates a coherent, deterministic placeholder set for the first-week
(days 1-7) intact Holdfast shelter: a cutaway interior background plus one
prop pictogram per authored room in `shelter_rooms.json`. Drawn
programmatically with Pillow — no AI generation, no external downloads.

Output (all under assets/sprites/Shelter/):
  shelter_interior_day1_7.png   760x420   intact interior cutaway
  room_<id>.png                 128x128   per-room prop pictogram
  prop_<name>.png               128x128   common environment props

The room list is read from the data authority so the pack stays in sync with
`shelter_rooms.json`. Re-run after room changes:
  python3 scripts/tools/generate-shelter-placeholders.py
Then import sidecars:
  bash scripts/ci/run-godot-bounded.sh --path . --import
"""

import json
import math
import pathlib
import random
from PIL import Image, ImageDraw, ImageEnhance, ImageFilter, ImageFont

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
DATA = REPO_ROOT / "Assets" / "StreamingAssets" / "Data" / "shelter_rooms.json"
OUT_DIR = REPO_ROOT / "assets" / "sprites" / "Shelter"
FONT_DIR = REPO_ROOT / "assets" / "fonts"

BG_W, BG_H = 760, 420
ICON = 128

# Palette — restrained, industrial, intact-shelter (not bombed).
CONCRETE = (28, 32, 38)
CONCRETE_DARK = (20, 23, 27)
CONCRETE_LIGHT = (46, 52, 60)
AMBER = (214, 158, 74)
TEAL = (96, 158, 156)
OFFWHITE = (206, 210, 214)
STEEL = (118, 128, 138)
RUST = (150, 92, 58)

# function -> (accent colour, pictogram kind)
FUNCTION_STYLE = {
    "Corridor":        (STEEL, "corridor"),
    "Dormitory":       (AMBER, "bunk"),
    "Workshop":        (RUST, "gear"),
    "MedicalBay":      (TEAL, "cross"),
    "Kitchen":         (AMBER, "pot"),
    "Storage":         (STEEL, "crate"),
    "Greenhouse":      (96, "plant"),
    "RadioRoom":       (TEAL, "antenna"),
    "Armory":          (RUST, "shield"),
    "Laboratory":      (TEAL, "flask"),
    "CommonArea":      (AMBER, "table"),
    "Airlock":         (STEEL, "airlock"),
    "GeneratorRoom":   (AMBER, "generator"),
    "FiltrationStack": (TEAL, "filter"),
}

PROPS = {
    "prop_supply_crate":  "crate",
    "prop_water_barrel":  "barrel",
    "prop_ceiling_lamp":  "lamp",
    "prop_pipe_bundle":   "pipes",
    "prop_hatch_door":    "airlock",
}


def font(size, bold=False):
    name = "BarlowCondensed-SemiBold.ttf" if bold else "BarlowCondensed-Regular.ttf"
    path = FONT_DIR / name
    try:
        return ImageFont.truetype(str(path), size)
    except Exception:
        return ImageFont.load_default()


def grain(img, seed, amount=9):
    """Deterministic subtle film grain so flat placeholders read as concrete."""
    rnd = random.Random(seed)
    px = img.load()
    w, h = img.size
    for _ in range((w * h) // 26):
        x = rnd.randrange(w)
        y = rnd.randrange(h)
        r, g, b = px[x, y][:3]
        d = rnd.randint(-amount, amount)
        px[x, y] = (max(0, min(255, r + d)), max(0, min(255, g + d)), max(0, min(255, b + d)))
    return img


def rounded(draw, box, radius, fill=None, outline=None, width=1):
    draw.rounded_rectangle(box, radius=radius, fill=fill, outline=outline, width=width)


class D:
    """ImageDraw wrapper that rounds float coordinates to ints (Pillow 11+)."""

    def __init__(self, draw):
        self.draw = draw

    @staticmethod
    def _xy(seq):
        if not seq:
            return seq
        if isinstance(seq[0], (int, float)):
            return [int(round(v)) for v in seq]
        return [(int(round(x)), int(round(y))) for x, y in seq]

    def line(self, xy, **kw):
        self.draw.line(self._xy(xy), **kw)

    def rectangle(self, xy, **kw):
        self.draw.rectangle(self._xy(xy), **kw)

    def ellipse(self, xy, **kw):
        self.draw.ellipse(self._xy(xy), **kw)

    def polygon(self, xy, **kw):
        self.draw.polygon(self._xy(xy), **kw)

    def rounded_rectangle(self, xy, **kw):
        self.draw.rounded_rectangle(self._xy(xy), **kw)

    def arc(self, xy, *a, **kw):
        self.draw.arc(self._xy(xy), *a, **kw)

    def text(self, xy, *a, **kw):
        self.draw.text((int(round(xy[0])), int(round(xy[1]))), *a, **kw)


# ── pictograms (centred on cx,cy within a size s box) ───────────────────────

def pictogram(draw, kind, cx, cy, s, col):
    s = int(round(s))
    lw = max(2, s // 22)
    if kind == "bunk":
        for i in (0, 1):
            y = cy - s * 0.22 + i * s * 0.34
            draw.rectangle([cx - s * 0.34, y, cx + s * 0.34, y + s * 0.16], outline=col, width=lw)
            draw.line([cx - s * 0.34, y + s * 0.16, cx - s * 0.40, y + s * 0.30], fill=col, width=lw)
            draw.rectangle([cx - s * 0.30, y + s * 0.03, cx - s * 0.18, y + s * 0.11], fill=col)
    elif kind == "gear":
        draw.ellipse([cx - s * 0.28, cy - s * 0.28, cx + s * 0.28, cy + s * 0.28], outline=col, width=lw)
        draw.ellipse([cx - s * 0.09, cy - s * 0.09, cx + s * 0.09, cy + s * 0.09], outline=col, width=lw)
        for i in range(8):
            a = i * math.pi / 4
            draw.line([cx + math.cos(a) * s * 0.28, cy + math.sin(a) * s * 0.28,
                       cx + math.cos(a) * s * 0.38, cy + math.sin(a) * s * 0.38], fill=col, width=lw)
    elif kind == "cross":
        draw.rectangle([cx - s * 0.10, cy - s * 0.34, cx + s * 0.10, cy + s * 0.34], fill=col)
        draw.rectangle([cx - s * 0.34, cy - s * 0.10, cx + s * 0.34, cy + s * 0.10], fill=col)
    elif kind == "pot":
        draw.arc([cx - s * 0.32, cy - s * 0.30, cx + s * 0.32, cy + s * 0.18], 0, 180, fill=col, width=lw)
        draw.rectangle([cx - s * 0.30, cy - s * 0.06, cx + s * 0.30, cy + s * 0.24], outline=col, width=lw)
        draw.line([cx - s * 0.40, cy + s * 0.24, cx + s * 0.40, cy + s * 0.24], fill=col, width=lw)
    elif kind == "crate":
        draw.rectangle([cx - s * 0.34, cy - s * 0.26, cx + s * 0.34, cy + s * 0.30], outline=col, width=lw)
        draw.line([cx - s * 0.34, cy - s * 0.26, cx + s * 0.34, cy + s * 0.30], fill=col, width=lw)
        draw.line([cx + s * 0.34, cy - s * 0.26, cx - s * 0.34, cy + s * 0.30], fill=col, width=lw)
    elif kind == "plant":
        draw.line([cx, cy + s * 0.32, cx, cy - s * 0.05], fill=col, width=lw)
        draw.arc([cx, cy - s * 0.30, cx + s * 0.36, cy + s * 0.04], 90, 260, fill=col, width=lw)
        draw.arc([cx - s * 0.36, cy - s * 0.20, cx, cy + s * 0.10], 280, 90, fill=col, width=lw)
    elif kind == "antenna":
        draw.line([cx, cy + s * 0.34, cx, cy - s * 0.10], fill=col, width=lw)
        draw.ellipse([cx - s * 0.06, cy - s * 0.16, cx + s * 0.06, cy - s * 0.04], fill=col)
        for r in (0.18, 0.30, 0.42):
            draw.arc([cx - s * r, cy - s * (r + 0.10), cx + s * r, cy + s * (r - 0.02)], 200, 340, fill=col, width=lw)
    elif kind == "shield":
        draw.polygon([(cx, cy - s * 0.34), (cx + s * 0.30, cy - s * 0.18),
                      (cx + s * 0.22, cy + s * 0.26), (cx, cy + s * 0.36),
                      (cx - s * 0.22, cy + s * 0.26), (cx - s * 0.30, cy - s * 0.18)], outline=col, width=lw)
        draw.line([cx - s * 0.14, cy, cx - s * 0.02, cy + s * 0.14], fill=col, width=lw)
        draw.line([cx - s * 0.02, cy + s * 0.14, cx + s * 0.18, cy - s * 0.14], fill=col, width=lw)
    elif kind == "flask":
        draw.polygon([(cx - s * 0.12, cy - s * 0.30), (cx + s * 0.12, cy - s * 0.30),
                      (cx + s * 0.30, cy + s * 0.30), (cx - s * 0.30, cy + s * 0.30)], outline=col, width=lw)
        draw.line([cx - s * 0.20, cy + s * 0.14, cx + s * 0.20, cy + s * 0.14], fill=col, width=lw)
    elif kind == "table":
        draw.rectangle([cx - s * 0.36, cy - s * 0.06, cx + s * 0.36, cy + s * 0.06], fill=col)
        for x in (-0.30, 0.30):
            draw.line([cx + s * x, cy + s * 0.06, cx + s * x, cy + s * 0.32], fill=col, width=lw)
        draw.rectangle([cx - s * 0.30, cy - s * 0.28, cx - s * 0.16, cy - s * 0.06], outline=col, width=lw)
        draw.rectangle([cx + s * 0.16, cy - s * 0.28, cx + s * 0.30, cy - s * 0.06], outline=col, width=lw)
    elif kind == "airlock":
        draw.rectangle([cx - s * 0.30, cy - s * 0.34, cx + s * 0.30, cy + s * 0.34], outline=col, width=lw)
        draw.ellipse([cx - s * 0.16, cy - s * 0.16, cx + s * 0.16, cy + s * 0.16], outline=col, width=lw)
        draw.line([cx - s * 0.16, cy, cx + s * 0.16, cy], fill=col, width=lw)
        draw.line([cx, cy - s * 0.16, cx, cy + s * 0.16], fill=col, width=lw)
    elif kind == "generator":
        draw.rectangle([cx - s * 0.36, cy - s * 0.24, cx + s * 0.36, cy + s * 0.24], outline=col, width=lw)
        draw.ellipse([cx - s * 0.20, cy - s * 0.20, cx + s * 0.20, cy + s * 0.20], outline=col, width=lw)
        draw.polygon([(cx + s * 0.02, cy - s * 0.16), (cx - s * 0.08, cy + s * 0.02),
                      (cx + s * 0.02, cy + s * 0.02), (cx - s * 0.04, cy + s * 0.18),
                      (cx + s * 0.12, cy - s * 0.04), (cx + s * 0.02, cy - s * 0.04)], fill=col)
    elif kind == "filter":
        draw.rectangle([cx - s * 0.28, cy - s * 0.34, cx + s * 0.28, cy + s * 0.34], outline=col, width=lw)
        for y in (-0.16, 0.0, 0.16):
            draw.line([cx - s * 0.28, cy + s * y, cx + s * 0.28, cy + s * y], fill=col, width=lw)
    elif kind == "corridor":
        for i in (-1, 0, 1):
            draw.line([cx + i * s * 0.18, cy + s * 0.30, cx + i * s * 0.08, cy - s * 0.30], fill=col, width=lw)
        draw.line([cx - s * 0.30, cy - s * 0.30, cx - s * 0.10, cy - s * 0.30], fill=col, width=lw)
        draw.line([cx + s * 0.10, cy - s * 0.30, cx + s * 0.30, cy - s * 0.30], fill=col, width=lw)
    elif kind == "barrel":
        draw.rectangle([cx - s * 0.24, cy - s * 0.32, cx + s * 0.24, cy + s * 0.32], outline=col, width=lw)
        for y in (-0.16, 0.16):
            draw.line([cx - s * 0.24, cy + s * y, cx + s * 0.24, cy + s * y], fill=col, width=lw)
    elif kind == "lamp":
        draw.polygon([(cx, cy - s * 0.30), (cx - s * 0.26, cy + s * 0.02), (cx + s * 0.26, cy + s * 0.02)], fill=col)
        for i in (-1, 1):
            draw.line([cx + i * s * 0.06, cy + s * 0.02, cx + i * s * 0.24, cy + s * 0.32], fill=col, width=lw)
    elif kind == "pipes":
        for i, off in enumerate((-0.22, 0.0, 0.22)):
            draw.line([cx - s * 0.38, cy + s * off, cx + s * 0.38, cy + s * off], fill=col, width=lw + i)
        draw.arc([cx - s * 0.30, cy - s * 0.34, cx + s * 0.30, cy - s * 0.06], 180, 360, fill=col, width=lw)


def room_icon(room):
    fn = room.get("function", "")
    accent, kind = FUNCTION_STYLE.get(fn, (STEEL, "crate"))
    img = Image.new("RGBA", (ICON, ICON), (0, 0, 0, 0))
    d = D(ImageDraw.Draw(img))
    rounded(d, [4, 4, ICON - 5, ICON - 5], 10, fill=CONCRETE, outline=CONCRETE_LIGHT, width=2)
    d.rectangle([4, 4, ICON - 5, 26], fill=(0, 0, 0, 60))
    rounded(d, [10, 10, ICON - 11, ICON - 11], 8, outline=accent, width=2)
    d.text((ICON / 2, 17), "PLACEHOLDER", font=font(9, bold=True), fill=AMBER, anchor="mm")
    pictogram(d, kind, ICON / 2, ICON * 0.46, ICON * 0.72, accent)
    label = room.get("display_name", room["id"]).upper()
    if len(label) > 16:
        label = label[:15] + "…"
    d.text((ICON / 2, ICON - 20), label, font=font(15, bold=True), fill=OFFWHITE, anchor="mm")
    d.text((ICON / 2, ICON - 7), fn.upper(), font=font(10), fill=accent, anchor="mm")
    return grain(img, room["id"])


def prop_icon(kind, accent=RUST):
    img = Image.new("RGBA", (ICON, ICON), (0, 0, 0, 0))
    d = D(ImageDraw.Draw(img))
    rounded(d, [4, 4, ICON - 5, ICON - 5], 10, fill=CONCRETE_DARK, outline=CONCRETE_LIGHT, width=2)
    pictogram(d, kind, ICON / 2, ICON / 2, ICON * 0.78, accent)
    d.text((ICON / 2, ICON - 13), "PLACEHOLDER", font=font(10, bold=True), fill=AMBER, anchor="mm")
    return grain(img, kind)


# ── interior cutaway background ────────────────────────────────────────────

def interior_background(rooms):
    img = Image.new("RGBA", (BG_W, BG_H), CONCRETE)
    d = D(ImageDraw.Draw(img))

    # ceiling + hanging lamps
    d.rectangle([0, 0, BG_W, 74], fill=CONCRETE_DARK)
    for x in range(60, BG_W, 110):
        d.line([x, 0, x, 30], fill=STEEL, width=2)
        d.polygon([(x, 30), (x - 18, 54), (x + 18, 54)], fill=(58, 54, 40))
        d.ellipse([x - 16, 52, x + 16, 60], fill=AMBER)
    # pipes
    for i, y in enumerate((14, 24, 34)):
        d.line([0, y, BG_W, y], fill=(70, 78, 88), width=3 + (i % 2))
    d.line([0, 44, BG_W, 44], fill=RUST, width=4)

    # floor with perspective
    d.rectangle([0, 336, BG_W, BG_H], fill=(36, 40, 46))
    for y in range(340, BG_H, 12):
        d.line([0, y, BG_W, y], fill=(30, 34, 40), width=1)
    d.line([0, 336, BG_W, 336], fill=STEEL, width=3)

    # corridor band
    d.rectangle([0, 250, BG_W, 336], fill=(24, 27, 32))
    d.line([0, 250, BG_W, 250], fill=CONCRETE_LIGHT, width=2)

    # room alcoves, evenly spaced across the viewport
    order = [
        "room_storage_bay", "room_workshop", "room_bunker_corridor", "room_kitchen",
        "room_bunks", "room_clinic", "room_filtration", "room_airlock",
    ]
    by_id = {r["id"]: r for r in rooms}
    margin, gap = 22, 6
    slot = (BG_W - margin * 2) / len(order)
    room_w = slot - gap
    for i, room_id in enumerate(order):
        room = by_id.get(room_id, {"id": room_id, "display_name": room_id, "function": ""})
        accent, kind = FUNCTION_STYLE.get(room.get("function", ""), (STEEL, "crate"))
        x0 = margin + i * slot + gap / 2
        x1 = x0 + room_w
        rounded(d, [x0, 150, x1, 322], 7, fill=(33, 38, 45), outline=accent, width=2)
        d.rectangle([x0 + 3, 150, x1 - 3, 172], fill=(0, 0, 0, 80))
        pictogram(d, kind, (x0 + x1) / 2, 218, 54, accent)
        name = room.get("display_name", room_id).upper()
        if len(name) > 11:
            name = name[:10] + "…"
        d.text(((x0 + x1) / 2, 300), name, font=font(11, bold=True), fill=OFFWHITE, anchor="mm")
        d.text(((x0 + x1) / 2, 163), "PLACEHOLDER", font=font(7, bold=True), fill=AMBER, anchor="mm")

    # wall stencil — intact, first week
    d.text((16, 92), "HOLDFAST // SHELTER INTERIOR", font=font(20, bold=True), fill=OFFWHITE)
    d.text((16, 116), "DAY 1–7 · INTACT · ALL SECTIONS SEALED",
           font=font(14), fill=AMBER)
    d.rectangle([16, 136, 176, 158], outline=AMBER, width=2)
    d.text((96, 147), "PLACEHOLDER ASSET", font=font(13, bold=True), fill=AMBER, anchor="mm")
    d.text((BG_W - 16, 92), "PLACEHOLDER", font=font(13, bold=True), fill=STEEL, anchor="ra")
    d.text((BG_W - 16, 112), "not final art", font=font(11), fill=STEEL, anchor="ra")

    # right-side hatch detail
    d.rectangle([BG_W - 58, 200, BG_W - 14, 320], outline=STEEL, width=3)
    d.ellipse([BG_W - 46, 244, BG_W - 26, 264], outline=STEEL, width=3)

    # vignette
    vig = Image.new("L", (BG_W, BG_H), 0)
    vd = D(ImageDraw.Draw(vig))
    vd.ellipse([-120, -120, BG_W + 120, BG_H + 120], fill=255)
    vig = vig.filter(ImageFilter.GaussianBlur(70))
    dark = Image.new("RGBA", (BG_W, BG_H), (0, 0, 0, 96))
    img = Image.composite(img, Image.alpha_composite(img, dark), vig)
    return grain(img.convert("RGBA"), 139141)


# Phase grades applied to the day cutaway so one drawing produces the set.
PHASE_GRADES = {
    "day":   {"tint": (255, 255, 255), "alpha": 0,   "bright": 1.00},
    "dawn":  {"tint": (255, 178, 104), "alpha": 42,  "bright": 0.90},
    "dusk":  {"tint": (208, 118, 64),  "alpha": 74,  "bright": 0.72},
    "night": {"tint": (58, 88, 168),   "alpha": 100, "bright": 0.52},
}


def apply_phase_grade(img, phase):
    g = PHASE_GRADES.get(phase, PHASE_GRADES["day"])
    out = img
    if g["bright"] != 1.0:
        out = ImageEnhance.Brightness(out).enhance(g["bright"])
    if g["alpha"] > 0:
        out = Image.alpha_composite(out, Image.new("RGBA", out.size, g["tint"] + (g["alpha"],)))
    return out


# ── reusable 128x128 shelter tiles ─────────────────────────────────────────

def tile_wall_concrete():
    img = Image.new("RGBA", (ICON, ICON), (52, 56, 62))
    d = D(ImageDraw.Draw(img))
    for y in (0, 64):
        d.line([0, y, ICON, y], fill=(38, 42, 48), width=3)
    d.line([64, 0, 64, 64], fill=(38, 42, 48), width=2)
    d.line([32, 64, 32, ICON], fill=(38, 42, 48), width=2)
    for x in range(8, ICON, 16):
        for y in (6, 58, 70, 122):
            d.ellipse([x - 1, y - 1, x + 1, y + 1], fill=(92, 98, 106))
    rnd = random.Random(7)
    for _ in range(40):
        x = rnd.randrange(ICON)
        y = rnd.randrange(0, 40)
        d.line([x, y, x, y + rnd.randint(6, 30)], fill=(44, 48, 54), width=1)
    return grain(img, 701)


def tile_floor_concrete():
    img = Image.new("RGBA", (ICON, ICON), (46, 46, 48))
    d = D(ImageDraw.Draw(img))
    d.line([0, 64, ICON, 64], fill=(34, 34, 36), width=4)
    d.line([64, 0, 64, ICON], fill=(34, 34, 36), width=4)
    d.rectangle([0, 0, ICON - 1, ICON - 1], outline=(38, 38, 40), width=2)
    rnd = random.Random(11)
    for _ in range(80):
        x = rnd.randrange(ICON)
        y = rnd.randrange(ICON)
        d.ellipse([x - 2, y - 1, x + 2, y + 1], fill=(54, 54, 56))
    return grain(img, 702)


def tile_floor_grate():
    img = Image.new("RGBA", (ICON, ICON), (34, 36, 40))
    d = D(ImageDraw.Draw(img))
    for x in range(8, ICON, 16):
        d.line([x, 0, x, ICON], fill=(64, 70, 78), width=5)
    for y in (0, 64, 127):
        d.line([0, y, ICON, y], fill=(74, 80, 88), width=4)
    return grain(img, 703)


def main():
    OUT_DIR.mkdir(parents=True, exist_ok=True)
    rooms = json.loads(DATA.read_text(encoding="utf-8"))["rooms"]

    bg = interior_background(rooms)
    written = []
    for phase, file_name in (
        ("day", "shelter_interior_day1_7.png"),
        ("dawn", "shelter_interior_dawn.png"),
        ("dusk", "shelter_interior_dusk.png"),
        ("night", "shelter_interior_night.png"),
    ):
        apply_phase_grade(bg, phase).save(OUT_DIR / file_name)
        written.append(file_name)

    for room in rooms:
        name = f"{room['id']}.png"
        room_icon(room).save(OUT_DIR / name)
        written.append(name)

    for name, kind in PROPS.items():
        file_name = f"{name}.png"
        prop_icon(kind).save(OUT_DIR / file_name)
        written.append(file_name)

    for tile_name, drawer in (
        ("tile_wall_concrete.png", tile_wall_concrete),
        ("tile_floor_concrete.png", tile_floor_concrete),
        ("tile_floor_grate.png", tile_floor_grate),
    ):
        drawer().save(OUT_DIR / tile_name)
        written.append(tile_name)

    # Every generated file is explicitly labelled as a placeholder, both inside
    # the pixels and in this manifest, so a later art pass can find and replace
    # the whole set deterministically.
    manifest = {
        "schema_version": 1,
        "collection": "holdfast_shelter_placeholders",
        "status": "PLACEHOLDER — NOT FINAL ART",
        "scope": "Intact Holdfast shelter, days 1-7 (never bombed/damaged)",
        "generator": "scripts/tools/generate-shelter-placeholders.py",
        "generated_by": "Pillow, programmatic line art",
        "license": "MIT",
        "replace_with_final_art": True,
        "files": [
            {"file": f, "placeholder": True, "label_in_image": "PLACEHOLDER"}
            for f in written
        ],
    }
    (OUT_DIR / "PLACEHOLDER_MANIFEST.json").write_text(
        json.dumps(manifest, indent=2) + "\n", encoding="utf-8")

    print(f"wrote {len(written)} placeholder files to {OUT_DIR.relative_to(REPO_ROOT)}")
    print("  4 interior lighting phases (day/dawn/dusk/night)  [PLACEHOLDER]")
    print(f"  {len(rooms)} room_<id>.png             {ICON}x{ICON}  [PLACEHOLDER]")
    print(f"  {len(PROPS)} prop_<name>.png           {ICON}x{ICON}  [PLACEHOLDER]")
    print("  3 tile_*.png                 128x128  [PLACEHOLDER]")
    print("  PLACEHOLDER_MANIFEST.json    labels every file")


if __name__ == "__main__":
    main()
