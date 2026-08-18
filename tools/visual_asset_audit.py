#!/usr/bin/env python3
"""
ASHFALL Phase A — Visual asset audit scanner.
Iterates every visual file (.png/.jpg/.jpeg/.svg/.webp/.tga/.bmp/.exr/.hdr)
under assets/, computes MD5 + SHA256 + dimensions + visual hash + categories
them by directory and semantic prefix from the AssetRegistry convention.

Output: docs/visual/visual_asset_manifest.json
"""
import os
import sys
import json
import hashlib
import argparse
from pathlib import Path
from collections import defaultdict, defaultdict as _dd

REPO_ROOT = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
ASSETS_DIR = REPO_ROOT / "assets"

VISUAL_EXTS = {".png", ".jpg", ".jpeg", ".svg", ".webp", ".tga", ".bmp", ".exr", ".hdr"}

# ──────────── semantic classification ────────────
# Map from directory → semantic_category. Multi-pass with prefix rules.
DIRECTORY_CATEGORY = {
    "sprites/Items/Medical":      "Inventory_Item_Medical",
    "sprites/Items/Ammo":         "Inventory_Item_Ammo",
    "sprites/Items/Weapons":      "Weapon",
    "sprites/Items/Tools":        "Inventory_Item_Tool",
    "sprites/Items/Materials":    "Crafting_Material",
    "sprites/Items/Devices":      "Inventory_Item_Device",
    "sprites/Items/Containers":   "Inventory_Item_Container",
    "sprites/Items":              "Inventory_Item",
    "sprites/Portraits":          "Character_Portrait",
    "sprites/portraits":          "Character_Portrait",
    "sprites/Survivors":          "Character_Sprite",
    "sprites/NPC":                "NPC_Portrait",
    "sprites/npcs":               "NPC_Portrait",
    "sprites/Enemies":            "Enemy_Sprite",
    "sprites/enemies":            "Enemy_Sprite",
    "sprites/Locations":          "Location_Art",
    "sprites/locations":          "Location_Art",
    "sprites/Factions":           "Faction_Art",
    "sprites/factions":           "Faction_Art",
    "sprites/Environment":        "Environment",
    "sprites/environment":        "Environment",
    "sprites/Weather":            "Weather",
    "sprites/VFX":                "VFX",
    "sprites/Particles":          "Particles",
    "sprites/UI":                 "UI_Art",
    "sprites/ui":                 "UI_Art",
    "sprites/Tiles":              "Tile",
    "sprites/Masks":              "Mask",
    "sprites/WeatherOverlays":    "Overlay_Weather",
    "sprites/StatusEffects":      "Overlay_Status",
    "sprites/Props":              "Prop",
    "sprites/Decorations":        "Decoration",
    "sprites/Backgrounds":        "Background",
    "sprites/AI_Generated":       "Generated_AI",
    "sprites":                    "Sprite_Unknown",
    "ui":                         "UI_BuildingBlock",
    "art":                        "Art_Generic",
}

PREFIX_CATEGORY = [
    ("item_", "Inventory_Item_Generic"),
    ("weapon_", "Weapon"),
    ("ammo_", "Inventory_Item_Ammo"),
    ("loc_", "Location_Art"),
    ("location_", "Location_Art"),
    ("faction_", "Faction_Art"),
    ("survivor_", "Character_Portrait"),
    ("npc_", "NPC_Portrait"),
    ("encounter_", "Encounter_Art"),
    ("weather_", "Weather"),
    ("quest_", "Quest_Art"),
    ("journal_", "Journal_Art"),
    ("trade_", "Trade_Art"),
    ("expedition_", "Expedition_Art"),
    ("holdfast_", "Holdfast_Art"),
    ("medical_", "Inventory_Item_Medical"),
    ("medicine_", "Inventory_Item_Medical"),
    ("pharma_", "Inventory_Item_Medical"),
    ("food_", "Inventory_Item_Food"),
    ("water_", "Inventory_Item_Food"),
    ("drink_", "Inventory_Item_Food"),
    ("scrap_", "Crafting_Material"),
    ("electronic_", "Crafting_Material"),
    ("mechanical_", "Crafting_Material"),
    ("panel_", "UI_PanelChrome"),
    ("frame_", "UI_PanelChrome"),
    ("button_", "UI_Button"),
    ("icon_", "UI_Icon"),
    ("bg_", "UI_Background"),
    ("header_", "UI_Header"),
    ("tab_", "UI_Tab"),
    ("misc_", "Misc"),
    ("placeholder_", "Placeholder"),
]

EXT_CATEGORY = {
    ".png": "PNG_Raster",
    ".jpg": "JPEG_Raster",
    ".jpeg": "JPEG_Raster",
    ".svg": "SVG_Vector",
}

try:
    from PIL import Image
    HAVE_PIL = True
except Exception:
    HAVE_PIL = False


def classify(file_path: Path, rel: str) -> dict:
    """Returns semantic_category, visual_family, intended_usage based on path."""
    parts_lower = [p.lower() for p in file_path.parts]
    stem_lower = file_path.stem.lower()

    # Directory-driven category first
    for key_dir, cat in DIRECTORY_CATEGORY.items():
        rel_key = rel.lower().replace("\\", "/")
        parts = rel_key.split("/")
        # check if path contains this dir
        if any(p == key_dir.lower() for p in parts):
            return {
                "semantic_category": cat,
                "visual_family": cat.split("_")[0],
                "intended_usage": "sprite",
            }

    # Fall back to stem-prefix rules
    for prefix, cat in PREFIX_CATEGORY:
        if stem_lower.startswith(prefix):
            return {
                "semantic_category": cat,
                "visual_family": cat.split("_")[0],
                "intended_usage": "sprite_or_icon",
            }

    # Default to Art_Generic
    return {
        "semantic_category": "Art_Generic",
        "visual_family": "Art",
        "intended_usage": "unknown",
    }


def compute_hashes(file_path: Path) -> dict:
    md5 = hashlib.md5()
    sha256 = hashlib.sha256()
    size = 0
    try:
        with open(file_path, "rb") as fh:
            for chunk in iter(lambda: fh.read(65536), b""):
                md5.update(chunk)
                sha256.update(chunk)
                size += len(chunk)
        return {
            "md5": md5.hexdigest(),
            "sha256": sha256.hexdigest(),
            "file_size": size,
        }
    except Exception as e:
        return {"md5": "UNREADABLE", "sha256": "UNREADABLE", "file_size": 0, "error": str(e)}


def image_dims(file_path: Path) -> dict:
    if not HAVE_PIL:
        return {"width": "UNKNOWN", "height": "UNKNOWN", "alpha_present": "UNKNOWN", "img_error": "PIL not available"}
    try:
        with Image.open(file_path) as im:
            w, h = im.size
            mode = im.mode
            has_alpha = "A" in mode or mode == "PA" or mode == "RGBA" or "transparency" in im.info
            return {
                "width": w,
                "height": h,
                "aspect_ratio": round(w / h, 4) if h else "INVALID",
                "alpha_present": has_alpha,
                "mode": mode,
            }
    except Exception as e:
        return {"width": 0, "height": 0, "aspect_ratio": 0, "alpha_present": False, "img_error": str(e)[:200]}


def quick_visual_signals(file_path: Path) -> dict:
    """Cheap checks for placeholder/debug patterns WITHOUT loading full image."""
    signals = {}
    name = file_path.stem.lower()
    if "placeholder" in name:
        signals["placeholder_naming"] = True
    if "debug" in name:
        signals["debug_naming"] = True
    if "temp" in name or "tmp_" in name or "_tmp" in name:
        signals["temp_naming"] = True
    if name in ("frame_9slice", "frame_9slice.png"):
        signals["canonical_ui_frame"] = True
    if name in ("panel_bg", "panel_bg_9slice", "panel_bg_9slice.png", "panel_bg.png"):
        signals["canonical_panel_bg"] = True
    if name.startswith("stitch_") or "stitch" in name:
        signals["stitch_naming"] = True
    if name.startswith("ai_") or "_ai_" in name:
        signals["ai_naming"] = True
    return signals


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--mode", choices=["scan", "summary"], default="scan")
    ap.add_argument("--out", default=str(REPO_ROOT / "docs/visual/visual_asset_manifest.json"))
    args = ap.parse_args()
    OUT = Path(args.out)
    OUT.parent.mkdir(parents=True, exist_ok=True)

    if args.mode == "scan":
        manifest = []
        count = 0
        for root, dirs, files in os.walk(ASSETS_DIR):
            for fn in files:
                p = Path(root) / fn
                if p.suffix.lower() not in VISUAL_EXTS:
                    continue
                rel = str(p.relative_to(REPO_ROOT))
                h = compute_hashes(p)
                dims = image_dims(p)
                cls = classify(p, rel)
                signals = quick_visual_signals(p)

                rec = {
                    "asset_id": p.stem,
                    "file_path": rel,
                    "file_type": p.suffix.lower(),
                    "width": dims.get("width"),
                    "height": dims.get("height"),
                    "aspect_ratio": dims.get("aspect_ratio"),
                    "file_size": h.get("file_size"),
                    "alpha_present": dims.get("alpha_present"),
                    "mode": dims.get("mode"),
                    "image_load_error": dims.get("img_error"),
                    "md5": h.get("md5"),
                    "sha256": h.get("sha256"),
                    "hash_error": h.get("error"),
                    "semantic_category": cls["semantic_category"],
                    "visual_family": cls["visual_family"],
                    "intended_usage": cls["intended_usage"],
                    "signals": signals,
                    # fields filled by later passes
                    "registry_ids": [],
                    "content_ids": [],
                    "code_references": [],
                    "data_references": [],
                    "scene_references": [],
                    "fallback_usage_count": 0,
                    "duplicate_group": "single",
                    "visual_status": "UNKNOWN",
                    "wiring_status": "UNKNOWN",
                    "severity": "NONE",
                    "notes": [],
                }
                manifest.append(rec)
                count += 1

        # write manifest
        with open(OUT, "w") as fh:
            json.dump(manifest, fh, indent=1)
        print(f"[scan] wrote {count} records → {OUT}")
        # summary by category
        from collections import Counter
        cat_count = Counter(r["semantic_category"] for r in manifest)
        ext_count = Counter(r["file_type"] for r in manifest)
        print(f"[scan] categories: {dict(cat_count)}")
        print(f"[scan] extensions: {dict(ext_count)}")
    else:
        with open(OUT) as fh:
            manifest = json.load(fh)
        from collections import Counter
        cat_count = Counter(r["semantic_category"] for r in manifest)
        ext_count = Counter(r["file_type"] for r in manifest)
        dup_count = Counter(r["duplicate_group"] for r in manifest)
        print(f"records: {len(manifest)}")
        print(f"categories: {dict(cat_count)}")
        print(f"extensions: {dict(ext_count)}")
        print(f"duplicate_groups: {dict(dup_count)}")


if __name__ == "__main__":
    main()
