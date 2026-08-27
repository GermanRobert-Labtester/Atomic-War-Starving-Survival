#!/usr/bin/env python3
"""
ASHFALL Approved Asset Importer
Imports approved assets from generated_AIassets/ into Godot's assets/ tree
and updates generated_AIassets/_manifest.json with approval metadata and import targets.
"""

import os
import sys
import json
import shutil
import datetime

ROOT_DIR = os.path.abspath(os.path.join(os.path.dirname(__file__), "../.."))
OUTPUT_DIR = os.path.join(ROOT_DIR, "generated_AIassets")
MANIFEST_PATH = os.path.join(OUTPUT_DIR, "_manifest.json")

def get_import_target(asset):
    aid = asset["id"]
    path = asset["path"]

    if "vector/" in path:
        filename = os.path.basename(path)
        return f"assets/ui/{filename}"
    elif "items/" in path:
        return f"assets/art/{aid}.png"
    elif "badges/" in path:
        return f"assets/ui/Icons/{aid}.png"
    elif "backgrounds/" in path:
        return f"assets/art/{aid}.png"
    else:
        return f"assets/art/{aid}.png"

def main():
    if not os.path.exists(MANIFEST_PATH):
        print(f"[Error] Manifest not found at {MANIFEST_PATH}")
        sys.exit(1)

    with open(MANIFEST_PATH, "r") as f:
        manifest = json.load(f)

    now_iso = datetime.datetime.now(datetime.timezone.utc).isoformat()
    imported_count = 0

    for asset in manifest["assets"]:
        src_path = os.path.join(ROOT_DIR, asset["path"])
        target_rel = get_import_target(asset)
        target_abs = os.path.join(ROOT_DIR, target_rel)

        # Ensure target dir exists
        os.makedirs(os.path.dirname(target_abs), exist_ok=True)

        # Copy file if source exists
        if os.path.exists(src_path):
            shutil.copy2(src_path, target_abs)
            imported_count += 1

        # If vector asset, also copy rendered png to assets/ui/Textures/
        if "vector/" in asset["path"]:
            vid = asset["id"]
            vpng_src = os.path.join(OUTPUT_DIR, "vector", "_png", f"{vid}.png")
            if os.path.exists(vpng_src):
                vpng_target = os.path.join(ROOT_DIR, "assets/ui/Textures", f"{vid}.png")
                os.makedirs(os.path.dirname(vpng_target), exist_ok=True)
                shutil.copy2(vpng_src, vpng_target)

        # Update manifest record
        asset["status"] = "approved"
        asset["approved_at"] = now_iso
        asset["import_target"] = target_rel

    manifest["updated_at"] = now_iso

    with open(MANIFEST_PATH, "w") as f:
        json.dump(manifest, f, indent=2)

    print(f"[Import] Successfully approved and imported {imported_count} assets into Godot assets/ tree.")
    print(f"[Import] Manifest updated at {MANIFEST_PATH}.")

if __name__ == "__main__":
    main()
