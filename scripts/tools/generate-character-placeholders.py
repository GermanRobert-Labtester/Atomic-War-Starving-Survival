#!/usr/bin/env python3
"""
generate-character-placeholders.py — ASHFALL character placeholder base.

Generates a simple, deliberately non-real "blockout mannequin" sprite sheet for
the shelter actor physics base. 4-frame walk cycle x 3 facings (front / side /
back). Every frame is stamped PLACEHOLDER.

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
    img = Image.new("RGBA", (FW, FH), (0, 0, 0, 0))
    d = D(ImageDraw.Draw(img))
    cx = FW // 2
    ground = FH - 6
    bob = 2 if phase in (1, 3) else 0
    hip_y = ground - 30 + bob
    shoulder_y = ground - 52 + bob
    head_cy = ground - 62 + bob
    swing = (0, 7, 0, -7)[phase]
    dark = shade(suit, 0.72)

    if facing == "side":
        # Far limbs first.
        draw_limb(d, cx, hip_y, cx + swing, ground, 7, dark)
        draw_limb(d, cx, shoulder_y + 4, cx - swing, hip_y + 8, 6, dark)
        # Torso
        d.rounded_rectangle([cx - 9, shoulder_y, cx + 9, hip_y + 4], radius=5, fill=suit, outline=dark, width=2)
        # Near limbs
        draw_limb(d, cx + 2, shoulder_y + 4, cx - 2 + swing, hip_y + 8, 7, suit)
        draw_limb(d, cx + 2, hip_y, cx + 2 - swing, ground, 8, suit)
        # Head + visor (profile)
        d.ellipse([cx - 10, head_cy - 10, cx + 10, head_cy + 10], fill=suit, outline=dark, width=2)
        d.polygon([(cx + 2, head_cy - 4), (cx + 11, head_cy - 1), (cx + 2, head_cy + 4)], fill=VISOR)
    else:
        # Both legs / arms; front shows visor, back shows pack.
        draw_limb(d, cx - 5, hip_y, cx - 5 - swing // 2, ground, 7, suit if facing == "front" else dark)
        draw_limb(d, cx + 5, hip_y, cx + 5 + swing // 2, ground, 7, dark if facing == "front" else suit)
        d.rounded_rectangle([cx - 11, shoulder_y, cx + 11, hip_y + 4], radius=5, fill=suit, outline=dark, width=2)
        draw_limb(d, cx - 10, shoulder_y + 4, cx - 12 + swing // 2, hip_y + 8, 6, suit)
        draw_limb(d, cx + 10, shoulder_y + 4, cx + 12 - swing // 2, hip_y + 8, 6, suit)
        d.ellipse([cx - 11, head_cy - 11, cx + 11, head_cy + 11], fill=suit, outline=dark, width=2)
        if facing == "front":
            d.rounded_rectangle([cx - 8, head_cy - 4, cx + 8, head_cy + 4], radius=3, fill=VISOR)
        else:
            d.rounded_rectangle([cx - 6, head_cy - 6, cx + 6, head_cy + 6], radius=3, fill=dark)

    # PLACEHOLDER stamp readable at source resolution.
    d.rectangle([2, 2, FW - 3, 12], outline=AMBER, width=1)
    d.text((FW // 2, 7), "PLACEHOLDER", font=font(8, bold=True), fill=AMBER, anchor="mm")
    return grain(img, f"{facing}{phase}")


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
        "status": "PLACEHOLDER — NOT FINAL ART",
        "scope": "Non-real blockout mannequin; 4-frame walk x front/side/back",
        "generator": "scripts/tools/generate-character-placeholders.py",
        "generated_by": "Pillow, programmatic line art",
        "layout": {"frame": [FW, FH], "cols": COLS, "rows": ROWS,
                   "rows_order": ["front", "side", "back"], "godot": "hframes=4, vframes=3"},
        "license": "MIT",
        "replace_with_final_art": True,
        "files": [
            {"file": f, "placeholder": True, "label_in_image": "PLACEHOLDER"}
            for f in written
        ],
    }
    (OUT_DIR / "PLACEHOLDER_MANIFEST.json").write_text(json.dumps(manifest, indent=2) + "\n", encoding="utf-8")

    print(f"wrote {len(written)} character placeholder sheets to {OUT_DIR.relative_to(REPO_ROOT)}")
    for f in written:
        print(f"  {f:34s} {FW * COLS}x{FH * ROWS} ({COLS}x{ROWS} frames)  [PLACEHOLDER]")
    print("  PLACEHOLDER_MANIFEST.json")


if __name__ == "__main__":
    main()
