#!/usr/bin/env python3
"""
Execute legacy asset migration for Ticket #124.
Reads tools/asset_migration/legacy_asset_inventory.json and performs:
  - remove_legacy_copy: delete legacy asset + orphaned .import sidecar
  - quarantine_legacy_copy: move legacy asset + .import sidecar to assets/quarantine/legacy_assets/
  - copy_to_canonical: copy missing asset to canonical tree (none expected)
"""
import json, shutil, os, sys
from pathlib import Path

REPO = Path(".").resolve()
INVENTORY = REPO / "tools" / "asset_migration" / "legacy_asset_inventory.json"
QUARANTINE_ROOT = REPO / "assets" / "quarantine" / "legacy_assets"

def migrate():
    with open(INVENTORY, encoding="utf-8") as f:
        data = json.load(f)

    entries = data["entries"]
    QUARANTINE_ROOT.mkdir(parents=True, exist_ok=True)

    removed = 0
    quarantined = 0
    copied = 0
    errors = []

    for entry in entries:
        legacy = REPO / entry["legacy_path"]
        if not legacy.exists():
            continue

        status = entry.get("migration_status")
        if status == "remove_legacy_copy":
            try:
                legacy.unlink()
                # Remove orphaned .import sidecar
                sidecar = legacy.with_suffix(legacy.suffix + ".import")
                if sidecar.exists():
                    sidecar.unlink()
                removed += 1
            except Exception as e:
                errors.append(f"REMOVE failed {legacy}: {e}")

        elif status == "quarantine_legacy_copy":
            try:
                target = QUARANTINE_ROOT / entry["legacy_path"]
                target.parent.mkdir(parents=True, exist_ok=True)
                shutil.move(str(legacy), str(target))
                # Move .import sidecar if present
                sidecar = (REPO / entry["legacy_path"]).with_suffix(legacy.suffix + ".import")
                if sidecar.exists():
                    sidecar_target = target.with_suffix(target.suffix + ".import")
                    sidecar_target.parent.mkdir(parents=True, exist_ok=True)
                    shutil.move(str(sidecar), str(sidecar_target))
                quarantined += 1
            except Exception as e:
                errors.append(f"QUARANTINE failed {legacy}: {e}")

        elif status == "copy_to_canonical":
            # Compute target path in canonical tree (preserve relative under Assets/)
            rel = legacy.relative_to(REPO / "Assets")
            target = REPO / "assets" / rel
            target.parent.mkdir(parents=True, exist_ok=True)
            shutil.copy2(str(legacy), str(target))
            # Copy .import sidecar
            sidecar = legacy.with_suffix(legacy.suffix + ".import")
            if sidecar.exists():
                sidecar_target = target.with_suffix(target.suffix + ".import")
                shutil.copy2(str(sidecar), str(sidecar_target))
            copied += 1

    # Remove empty legacy directories (bottom-up)
    for root_dir in [REPO / "Assets" / "art", REPO / "Assets" / "sprites", REPO / "Assets" / "ui", REPO / "Assets" / "audio"]:
        if root_dir.exists():
            for dirpath, dirnames, filenames in os.walk(root_dir, topdown=False):
                if dirpath == str(root_dir):
                    continue
                # Try to remove empty dirs
                try:
                    Path(dirpath).rmdir()
                except OSError:
                    pass
            try:
                root_dir.rmdir()
            except OSError:
                pass

    # Update inventory summary
    data["summary"]["removed"] = removed
    data["summary"]["quarantined"] = quarantined
    data["summary"]["copied"] = copied
    data["summary"]["errors"] = len(errors)
    with open(INVENTORY, "w", encoding="utf-8") as f:
        json.dump(data, f, indent=2)

    print(f"Migration complete:")
    print(f"  Removed (exact duplicates): {removed}")
    print(f"  Quarantined (content conflicts): {quarantined}")
    print(f"  Copied (missing assets): {copied}")
    print(f"  Errors: {len(errors)}")
    for e in errors:
        print(f"    {e}")

if __name__ == "__main__":
    migrate()
