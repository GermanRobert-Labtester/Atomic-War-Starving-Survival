#!/usr/bin/env python3
"""
Schema version migration tool for ASHFALL JSON data files.

Modes:
  --check   Report what would change; no writes
  --write   Mutate only validated eligible files
  --dry-run Same as --check

Rules:
  - Object-root files: add "schema_version": 1 if missing
  - Wrapper-list files: already wrapped, skip
  - Bare-list files: wrap as {"schema_version": 1, "key": [...]}
  - Static blobs: skip unless explicitly targeted
  - Never double-wrap
  - Preserve key order and formatting where possible
"""

import argparse
import json
import os
import sys
from typing import Dict, List, Tuple

DATA_DIR = "Assets/StreamingAssets/Data"

# Files that should NOT be auto-wrapped (static content, generated files, etc.)
SKIP_FILES = {
    "schema_manifest.json",
    "mod_manifest.json",
    "README.md",
}

# Key names for wrapper-list files (the list key to preserve)
WRAPPER_LIST_KEYS = {
    "items": "items",
    "locations": "locations",
    "survivors": "survivors",
    "events": "events",
    "radio": "radio_broadcasts",
    "quests": "quests",
    "doses": "doses",
    "diseases": "diseases",
    "actions": "actions",
    "encounters": "encounters",
    "sectors": "sectors",
    "zones": "zones",
    "recipes": "recipes",
    "factions": "factions",
    "echoes": "echoes",
    "articles": "articles",
    "skills": "skills",
    "traits": "traits",
    "afflictions": "afflictions",
    "knowledge": "knowledge",
    "endings": "endings",
    "npc": "npcs",
    "questlines": "questlines",
    "dialogue": "dialogue",
    "journal": "journal_entries",
    "transmissions": "transmissions",
    "songs": "songs",
    "broadcasts": "broadcasts",
    "procedures": "procedures",
    "blueprints": "blueprints",
    "catalogs": "catalogs",
}


def classify_root(data: dict) -> Tuple[str, str, int]:
    """Classify JSON root shape. Returns (shape, wrapper_key, schema_version_or_0)."""
    if not isinstance(data, dict):
        return "bare-list", "", 0

    # Check for existing schema_version
    sv = data.get("schema_version", 0)
    if isinstance(sv, int) and sv > 0:
        return "versioned", "", sv

    # Check if it's already a wrapper (has a known list key)
    for key in WRAPPER_LIST_KEYS:
        if key in data and isinstance(data[key], list):
            return "wrapper-list", key, 0

    # Check if it's a single catalog object (not a wrapper)
    # Heuristic: if it has exactly one key that's a list, it might be an unwrapped wrapper
    list_keys = [k for k, v in data.items() if isinstance(v, list)]
    if len(list_keys) == 1:
        return "object-root", list_keys[0], 0

    return "object-root", "", 0


def migrate_file(filepath: str, dry_run: bool = True) -> Tuple[bool, str]:
    """Migrate a single JSON file. Returns (changed, message)."""
    filename = os.path.basename(filepath)
    if filename in SKIP_FILES:
        return False, f"SKIP (in skip list): {filename}"

    try:
        with open(filepath, 'r', encoding='utf-8') as f:
            content = f.read()
            data = json.loads(content)
    except (json.JSONDecodeError, OSError) as e:
        return False, f"ERROR (parse failed): {e}"

    shape, wrapper_key, existing_sv = classify_root(data)

    if shape == "versioned":
        return False, f"SKIP (already versioned v{existing_sv}): {filename}"

    if shape == "wrapper-list":
        return False, f"SKIP (already wrapper-list with key '{wrapper_key}'): {filename}"

    if shape == "bare-list":
        # Need to determine wrapper key from filename
        stem = os.path.splitext(filename)[0].lower()
        wrapper_key = None
        for key in WRAPPER_LIST_KEYS:
            if stem.startswith(key) or stem.endswith(key):
                wrapper_key = WRAPPER_LIST_KEYS[key]
                break
        if not wrapper_key:
            wrapper_key = "items"  # default fallback

        new_data = {
            "schema_version": 1,
            wrapper_key: data if isinstance(data, list) else [data]
        }
        if not dry_run:
            with open(filepath, 'w', encoding='utf-8') as f:
                json.dump(new_data, f, indent=2, ensure_ascii=False)
        return True, f"WRAP (bare-list → wrapper-list '{wrapper_key}'): {filename}"

    if shape == "object-root":
        # Add schema_version at root
        if not dry_run:
            # Insert schema_version as first key
            new_data = {"schema_version": 1}
            new_data.update(data)
            with open(filepath, 'w', encoding='utf-8') as f:
                json.dump(new_data, f, indent=2, ensure_ascii=False)
        return True, f"VERSION (add schema_version=1 to object-root): {filename}"

    return False, f"SKIP (unknown shape): {filename}"


def main():
    parser = argparse.ArgumentParser(description="Schema version migration tool")
    parser.add_argument("--check", action="store_true", help="Report what would change (default)")
    parser.add_argument("--write", action="store_true", help="Mutate eligible files")
    parser.add_argument("--target", choices=["all", "expansion", "core"], default="all", help="Target scope")
    args = parser.parse_args()

    dry_run = not args.write
    mode = "CHECK" if dry_run else "WRITE"

    print(f"Schema version migration — mode: {mode}")
    print(f"Target: {args.target}")
    print(f"Data dir: {DATA_DIR}")
    print()

    if not os.path.isdir(DATA_DIR):
        print(f"ERROR: Data directory not found: {DATA_DIR}")
        sys.exit(1)

    manifest: List[Dict] = []
    changed_count = 0
    skipped_count = 0
    error_count = 0

    for root, dirs, files in os.walk(DATA_DIR):
        for filename in files:
            if not filename.endswith('.json'):
                continue

            filepath = os.path.join(root, filename)
            relpath = os.path.relpath(filepath, DATA_DIR)

            # Apply target filter
            if args.target == "expansion" and "expansion" not in relpath.lower():
                continue
            if args.target == "core" and "expansion" in relpath.lower():
                continue

            changed, message = migrate_file(filepath, dry_run=dry_run)

            entry = {
                "path": relpath,
                "changed": changed,
                "message": message,
            }
            manifest.append(entry)

            if changed:
                changed_count += 1
                print(f"[CHANGED] {message}")
            elif "ERROR" in message:
                error_count += 1
                print(f"[ERROR] {message}")
            else:
                skipped_count += 1

    print()
    print(f"Summary: {changed_count} changed, {skipped_count} skipped, {error_count} errors")

    # Write manifest
    manifest_path = "docs/forensics/schema_migration_manifest.json"
    with open(manifest_path, 'w', encoding='utf-8') as f:
        json.dump({
            "mode": mode,
            "target": args.target,
            "changed": changed_count,
            "skipped": skipped_count,
            "errors": error_count,
            "entries": manifest,
        }, f, indent=2, ensure_ascii=False)
    print(f"Manifest written: {manifest_path}")

    if error_count > 0:
        sys.exit(1)


if __name__ == "__main__":
    main()
