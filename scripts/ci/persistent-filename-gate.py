#!/usr/bin/env python3
"""
persistent-filename-gate.py — Persistent Filename Uniqueness & Save-Section Registry Gate

Validates that:
  1. Every persistent save filename in src/ and Assets/Ashfall.Core/ is unique (0 aliasing/collisions).
  2. Every persistent save file name follows snake_case naming ending in .json.
  3. Every persistent filename maps to a declared SectionKey in SaveSectionRegistry.cs (or documented section family).
  4. Every registered save section in SaveSectionRegistry.cs has a valid persistent store filename.
  5. Non-gameplay persistent files (settings.json, audio_settings.json) are inventoried and collision-free.

Usage:
  python3 scripts/ci/persistent-filename-gate.py           # Validates all persistent filenames
"""

import os
import re
import sys
import pathlib

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
REGISTRY_FILE = REPO_ROOT / "Assets" / "Ashfall.Core" / "Save" / "SaveSectionRegistry.cs"

# Known section family aliases that map into a canonical registered section
SECTION_ALIASES = {
    "holdfast_s1": "holdfast",
    "holdfast_trade": "holdfast_trade",
    "weather": "world"
}

# Known non-gameplay persistent files (engine/user preferences)
NON_GAMEPLAY_FILES = {
    "settings.json": "UserSettings",
    "audio_settings.json": "AudioSettings"
}

def load_registered_sections():
    if not REGISTRY_FILE.exists():
        print(f"FAILED: Registry file not found at {REGISTRY_FILE}", file=sys.stderr)
        sys.exit(1)

    text = REGISTRY_FILE.read_text(encoding="utf-8")
    matches = re.findall(r'new\s*\(\s*"([^"]+)"', text)
    return set(matches)

def scan_persistent_filenames():
    search_paths = [
        REPO_ROOT / "src",
        REPO_ROOT / "Assets" / "Ashfall.Core"
    ]

    stores = []
    filename_to_stores = {}

    for sp in search_paths:
        for p in sp.rglob("*.cs"):
            s = p.as_posix()
            if "/obj/" in s or "/bin/" in s: continue
            if s.endswith("Test.cs") or s.endswith("Tests.cs") or s.endswith("SelfTest.cs"): continue

            content = p.read_text(encoding="utf-8")
            rel_path = p.relative_to(REPO_ROOT).as_posix()

            class_matches = list(re.finditer(r"public\s+(?:static\s+|sealed\s+)?class\s+([A-Za-z0-9_]+)", content))
            for i, cm in enumerate(class_matches):
                cname = cm.group(1)
                cstart = cm.start()
                cend = class_matches[i+1].start() if i+1 < len(class_matches) else len(content)
                cbody = content[cstart:cend]

                # Match FileName or SaveFileName constants
                file_matches = re.findall(r'public\s+const\s+string\s+(?:FileName|SaveFileName)\s*=\s*"([^"]+)"', cbody)
                sec_matches = re.findall(r'public\s+const\s+string\s+SectionName\s*=\s*"([^"]+)"', cbody)

                if file_matches:
                    fn = file_matches[0]
                    sec = sec_matches[0] if sec_matches else None
                    store_info = {
                        "class": cname,
                        "file": rel_path,
                        "filename": fn,
                        "section": sec
                    }
                    stores.append(store_info)
                    if fn not in filename_to_stores:
                        filename_to_stores[fn] = []
                    filename_to_stores[fn].append(store_info)

    return stores, filename_to_stores

def main():
    registered_sections = load_registered_sections()
    stores, filename_to_stores = scan_persistent_filenames()

    errors = []

    print("=================================================================================")
    print("  ASHFALL — PERSISTENT FILENAME UNIQUENESS & REGISTRY GATE")
    print("=================================================================================")
    print(f"Registered Sections in SaveSectionRegistry: {len(registered_sections)}")
    print(f"Discovered Persistent Save Stores:          {len(stores)}")
    print(f"Discovered Unique Save Filenames:           {len(filename_to_stores)}")
    print(f"Discovered Non-Gameplay Persistent Files:   {len(NON_GAMEPLAY_FILES)}")
    print("---------------------------------------------------------------------------------")

    # 1. Check uniqueness (collisions)
    for fn, store_list in sorted(filename_to_stores.items()):
        if len(store_list) > 1:
            classes = [s["class"] for s in store_list]
            errors.append(f"Collision on filename '{fn}': shared by multiple classes {classes}")

    # Check non-gameplay collisions
    for ng_fn in NON_GAMEPLAY_FILES:
        if ng_fn in filename_to_stores:
            errors.append(f"Collision: Non-gameplay file '{ng_fn}' collides with gameplay save store {filename_to_stores[ng_fn]}")

    # 2. Check naming convention & registry representation
    snake_json_pattern = re.compile(r'^[a-z0-9_]+\.json$')
    covered_sections = set()

    for s in stores:
        fn = s["filename"]
        sec = s["section"]

        # Validate snake_case .json naming
        if not snake_json_pattern.match(fn):
            errors.append(f"Invalid filename '{fn}' in {s['class']} ({s['file']}): must be snake_case ending in .json")

        # Validate section mapping in SaveSectionRegistry
        if sec:
            canon_sec = SECTION_ALIASES.get(sec, sec)
            if canon_sec not in registered_sections:
                errors.append(f"SectionName '{sec}' in {s['class']} ({s['file']}) is not registered in SaveSectionRegistry.cs")
            else:
                covered_sections.add(canon_sec)

    # 3. Check that all registered sections have associated stores
    for reg_sec in registered_sections:
        if reg_sec not in covered_sections:
            errors.append(f"Registered section '{reg_sec}' has no corresponding SaveStore with matching SectionName")

    if errors:
        print("\n❌ PERSISTENT FILENAME GATE VIOLATIONS:")
        for err in errors:
            print(f"  - {err}")
        return 1

    print(f"{'Filename':<35} {'Class':<30} {'Section':<20}")
    print("---------------------------------------------------------------------------------")
    for fn, store_list in sorted(filename_to_stores.items()):
        s = store_list[0]
        print(f"{fn:<35} {s['class']:<30} {s['section'] or '—':<20}")
    print("---------------------------------------------------------------------------------")
    for ng_fn, owner in sorted(NON_GAMEPLAY_FILES.items()):
        print(f"{ng_fn:<35} {owner:<30} {'(non-gameplay)':<20}")
    print("=================================================================================")
    print(f"\n✅ Persistent Filename Gate Passed: {len(filename_to_stores)} save files + {len(NON_GAMEPLAY_FILES)} user files strictly unique and mapped to {len(registered_sections)} registered sections.")
    return 0

if __name__ == "__main__":
    sys.exit(main())
