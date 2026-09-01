#!/usr/bin/env python3
"""
Build legacy asset inventory for Ticket #124.
Generates tools/asset_migration/legacy_asset_inventory.json
"""
import os, hashlib, json, sys
from pathlib import Path

REPO = Path(".").resolve()
LEGACY_ROOTS = [
    REPO / "Assets" / "art",
    REPO / "Assets" / "sprites",
    REPO / "Assets" / "ui",
    REPO / "Assets" / "audio",
]
CANONICAL_ROOT = REPO / "assets"

def sha256(path: Path):
    h = hashlib.sha256()
    try:
        with open(path, "rb") as f:
            for chunk in iter(lambda: f.read(1024*1024), b""):
                h.update(chunk)
        return h.hexdigest()
    except Exception:
        return None

def rel(path: Path):
    return str(path.relative_to(REPO)).replace("\\", "/")

def is_asset_file(name: str):
    lower = name.lower()
    if lower.endswith(".import"):
        return False
    if lower.endswith(".gdignore"):
        return False
    if lower.endswith(".gitkeep"):
        return False
    if lower.endswith(".md"):
        return False
    if lower.endswith(".txt"):
        return False
    if lower.endswith(".json"):
        return False
    return True

# Build canonical index: basename (no ext) -> list of (path, hash)
canonical_index = {}
for root, dirs, files in os.walk(CANONICAL_ROOT):
    dirs.sort()
    for f in sorted(files):
        if not is_asset_file(f):
            continue
        p = Path(root) / f
        basename = p.stem
        h = sha256(p)
        canonical_index.setdefault(basename, []).append({
            "path": rel(p),
            "sha256": h,
            "size": p.stat().st_size,
        })

inventory = []
for legacy_root in LEGACY_ROOTS:
    if not legacy_root.exists():
        continue
    for dirpath, dirs, files in os.walk(legacy_root):
        dirs.sort()
        for f in sorted(files):
            p = Path(dirpath) / f
            if not is_asset_file(f):
                continue
            h = sha256(p)
            basename = p.stem
            matches = canonical_index.get(basename, [])
            is_dup = any(m["sha256"] == h for m in matches)
            # Find LFS status via .gitattributes
            lfs = False
            try:
                import subprocess
                r = subprocess.run(["git", "check-attr", "filter", str(p.relative_to(REPO))], capture_output=True, text=True)
                if "lfs" in r.stdout:
                    lfs = True
            except Exception:
                pass

            entry = {
                "legacy_path": rel(p),
                "normalized_legacy_path": rel(p).lower(),
                "size": p.stat().st_size,
                "sha256": h,
                "extension": p.suffix.lower(),
                "basename": basename,
                "canonical_matches": [m["path"] for m in matches],
                "is_exact_duplicate": is_dup,
                "lfs": lfs,
                "classification": None,
                "migration_status": None,
            }
            inventory.append(entry)

# Sort deterministically
inventory.sort(key=lambda e: e["legacy_path"])

# Auto-classify
for entry in inventory:
    if entry["is_exact_duplicate"]:
        entry["classification"] = "already_migrated"
        entry["migration_status"] = "remove_legacy_copy"
    elif len(entry["canonical_matches"]) == 0:
        entry["classification"] = "migrate"
        entry["migration_status"] = "copy_to_canonical"
    else:
        entry["classification"] = "superseded"
        entry["migration_status"] = "quarantine_legacy_copy"

# Summary
from collections import Counter
counts = Counter(e["classification"] for e in inventory)
summary = {
    "total_legacy_files": len(inventory),
    "migrated": counts.get("migrate", 0),
    "already_migrated": counts.get("already_migrated", 0),
    "superseded": counts.get("superseded", 0),
    "quarantine": 0,
    "archive": 0,
    "unresolved": counts.get("migrate", 0),
    "exact_duplicates": counts.get("already_migrated", 0),
    "content_conflicts": counts.get("superseded", 0),
    "case_conflicts": 0,
}

output = {
    "schema_version": 1,
    "generated_by": "tools/asset_migration/build_inventory.py",
    "ticket": "TICKET_124",
    "summary": summary,
    "entries": inventory,
}

out_path = REPO / "tools" / "asset_migration" / "legacy_asset_inventory.json"
out_path.write_text(json.dumps(output, indent=2), encoding="utf-8")
print(f"Inventory written: {out_path}")
print(f"Total: {summary['total_legacy_files']}")
print(f"  already_migrated (exact duplicates): {summary['exact_duplicates']}")
print(f"  superseded (content conflicts): {summary['content_conflicts']}")
print(f"  migrate (unresolved): {summary['unresolved']}")
