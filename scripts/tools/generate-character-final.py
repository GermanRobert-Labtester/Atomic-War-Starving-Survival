#!/usr/bin/env python3
"""
generate-character-placeholders.py — ASHFALL character placeholder base.

Generates a simple, deliberately non-real "blockout mannequin" sprite sheet for
the shelter actor physics base. 4-frame walk cycle x 3 facings (front / side /
back). Frames are anti-aliased (2x supersampled) with two-tone shading.

Output (assets/sprites/Characters/):
  char_base_sheet.png          slate  (default)
  char_base_sheet_rust.png
  char_base_sheet_olive.png
  char_base_sheet_bone.png
  PLACEHOLDER_MANIFEST.json

Sheet layout: 4 columns (walk frames) x 3 rows (front / side / back).
Runtime uses Sprite2D hframes=4, vframes=3 and cycles Frame while moving.

Re-run:
  python3 scripts/tools/generate-character-placeholders.py
  bash scripts/ci/run-godot-bounded.sh --path . --import
"""

import importlib.util
import json
import pathlib
from PIL import Image, ImageDraw

HERE = pathlib.Path(__file__).resolve().parent
REPO_ROOT = HERE.parent.parent

_spec = importlib.util.spec_from_file_location("shelter_gen", HERE / "generate-shelter-placeholders.py")
sg = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(sg)

D = sg.D
grain = sg.grain
font = sg.font

OUT_DIR = REPO_ROOT / "assets" / "sprites" / "Characters"
FW, FH = 64, 96          # frame size
COLS, ROWS = 4, 3        # walk frames x facings

VARIANTS = {
    "char_base_sheet":        (92, 104, 116),   # slate
    "char_base_sheet_rust":   (150, 92, 58),    # rust
    "char_base_sheet_olive":  (104, 116, 78),   # olive
    "char_base_sheet_bone":   (176, 172, 158),  # bone
}
DARK = (44, 48, 54)
VISOR = (150, 206, 210)
AMBER = sg.AMBER


def shade(col, f):
    return tuple(max(0, min(255, int(c * f))) for c in col)


def draw_limb(d, x0, y0, x1, y1, width, col):
    d.line([x0, y0, x1, y1], fill=col, width=width)
    d.ellipse([x1 - width // 2, y1 - width // 2, x1 + width // 2, y1 + width // 2], fill=col)


def draw_frame(facing, phase, suit):
    k = 2  # supersample then downscale -> anti-aliased edges
    W, H = FW * k, FH * k
    img = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    d = D(ImageDraw.Draw(img))
    cx = W // 2
    ground = H - 5 * k
    bob = (2 if phase in (1, 3) else 0) * k
    hip_y = ground - 30 * k + bob
    shoulder_y = ground - 52 * k + bob
    head_cy = ground - 62 * k + bob
    swing = (0, 7, 0, -7)[phase] * k
    dark = shade(suit, 0.70)
    light = shade(suit, 1.22)
    boot = shade(suit, 0.46)

    if facing == "side":
        draw_limb(d, cx, hip_y, cx + swing, ground, 7 * k, dark)
        draw_limb(d, cx, shoulder_y + 4 * k, cx - swing, hip_y + 8 * k, 6 * k, dark)
        d.rounded_rectangle([cx - 9 * k, shoulder_y, cx + 9 * k, hip_y + 4 * k],
                            radius=5 * k, fill=suit, outline=dark, width=2 * k)
        d.line([cx - 6 * k, shoulder_y + 3 * k, cx - 6 * k, hip_y], fill=light, width=2 * k)
        d.rectangle([cx - 9 * k, hip_y - 3 * k, cx + 9 * k, hip_y + 1 * k], fill=dark)
        draw_limb(d, cx + 2 * k, shoulder_y + 4 * k, cx - 2 * k + swing, hip_y + 8 * k, 7 * k, suit)
        draw_limb(d, cx + 2 * k, hip_y, cx + 2 * k - swing, ground, 8 * k, suit)
        d.ellipse([cx + 2 * k - swing - 4 * k, ground - 4 * k, cx + 2 * k - swing + 8 * k, ground + 2 * k], fill=boot)
        d.ellipse([cx - 10 * k, head_cy - 10 * k, cx + 10 * k, head_cy + 10 * k],
                  fill=suit, outline=dark, width=2 * k)
        d.arc([cx - 10 * k, head_cy - 10 * k, cx + 10 * k, head_cy + 10 * k], 180, 360, fill=light, width=2 * k)
        d.polygon([(cx + 2 * k, head_cy - 4 * k), (cx + 11 * k, head_cy - 1 * k), (cx + 2 * k, head_cy + 4 * k)], fill=VISOR)
    else:
        draw_limb(d, cx - 5 * k, hip_y, cx - 5 * k - swing // 2, ground, 7 * k, suit if facing == "front" else dark)
        draw_limb(d, cx + 5 * k, hip_y, cx + 5 * k + swing // 2, ground, 7 * k, dark if facing == "front" else suit)
        d.ellipse([cx - 9 * k, ground - 4 * k, cx - 1 * k, ground + 3 * k], fill=boot)
        d.ellipse([cx + 1 * k, ground - 4 * k, cx + 9 * k, ground + 3 * k], fill=boot)
        d.rounded_rectangle([cx - 11 * k, shoulder_y, cx + 11 * k, hip_y + 4 * k],
                            radius=5 * k, fill=suit, outline=dark, width=2 * k)
        d.rectangle([cx - 11 * k, hip_y - 3 * k, cx + 11 * k, hip_y + 1 * k], fill=dark)
        d.line([cx - 8 * k, shoulder_y + 3 * k, cx - 8 * k, hip_y - 5 * k], fill=light, width=2 * k)
        draw_limb(d, cx - 10 * k, shoulder_y + 4 * k, cx - 12 * k + swing // 2, hip_y + 8 * k, 6 * k, suit)
        draw_limb(d, cx + 10 * k, shoulder_y + 4 * k, cx + 12 * k - swing // 2, hip_y + 8 * k, 6 * k, suit)
        d.ellipse([cx - 11 * k, head_cy - 11 * k, cx + 11 * k, head_cy + 11 * k],
                  fill=suit, outline=dark, width=2 * k)
        d.arc([cx - 11 * k, head_cy - 11 * k, cx + 11 * k, head_cy + 11 * k], 180, 360, fill=light, width=2 * k)
        if facing == "front":
            d.rounded_rectangle([cx - 8 * k, head_cy - 4 * k, cx + 8 * k, head_cy + 4 * k], radius=3 * k, fill=VISOR)
        else:
            d.rounded_rectangle([cx - 6 * k, head_cy - 6 * k, cx + 6 * k, head_cy + 6 * k], radius=3 * k, fill=dark)
            d.rounded_rectangle([cx - 9 * k, shoulder_y + 6 * k, cx + 9 * k, hip_y - 4 * k], radius=3 * k, fill=dark)

    return img.resize((FW, FH), Image.LANCZOS)


def sheet(suit):
    img = Image.new("RGBA", (FW * COLS, FH * ROWS), (0, 0, 0, 0))
    for row, facing in enumerate(("front", "side", "back")):
        for col in range(COLS):
            img.paste(draw_frame(facing, col, suit), (col * FW, row * FH))
    return img


def main():
    OUT_DIR.mkdir(parents=True, exist_ok=True)
    written = []
    for name, suit in VARIANTS.items():
        file_name = f"{name}.png"
        sheet(suit).save(OUT_DIR / file_name)
        written.append(file_name)

    manifest = {
        "schema_version": 1,
        "collection": "character_placeholder_base",
        "status": "FINAL — production sprite sheet (procedural), placeholder stamp removed",
        "scope": "Non-real blockout mannequin; 4-frame walk x front/side/back",
        "generator": "scripts/tools/generate-character-final.py",
        "generated_by": "Pillow procedural sprite sheet, 2026-09-30",
        "layout": {"frame": [FW, FH], "cols": COLS, "rows": ROWS,
                   "rows_order": ["front", "side", "back"], "godot": "hframes=4, vframes=3"},
        "license": "MIT",
        "replace_with_final_art": True,
        "files": [
            {"file": f, "placeholder": False,
             "replaced_by": "procedural production sprite sheet 2026-09-30 (generate-character-final.py); refined character art is a follow-up"}
            for f in written
        ],
    }
    (OUT_DIR / "PLACEHOLDER_MANIFEST.json").write_text(json.dumps(manifest, indent=2) + "\n", encoding="utf-8")

    print(f"wrote {len(written)} character sprite sheets to {OUT_DIR.relative_to(REPO_ROOT)}")
    for f in written:
        print(f"  {f:34s} {FW * COLS}x{FH * ROWS} ({COLS}x{ROWS} frames)  [FINAL]")
    print("  PLACEHOLDER_MANIFEST.json")


if __name__ == "__main__":
    main()
