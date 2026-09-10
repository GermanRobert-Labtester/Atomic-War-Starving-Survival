#!/usr/bin/env python3
"""ASHFALL Unity→Godot asset batch migration tool (Plan 48 / Task 14, Phase 14K).

Manifest-driven, copy-first, never deletes originals.

    python3 scripts/tools/migrate-assets-batch.py --manifest scripts/tools/asset-migration-batch-01.json [--dry-run]

Checks performed per entry:
  - source exists;
  - destination is snake_case and inside the destination root;
  - no case-folded collision with the existing destination tree;
  - LFS expectation recorded (verified externally via `git lfs ls-files`).

Deterministic: ordinal ordering in the report. The script never deletes any
file; it only copies and writes a report artifact.
"""

import argparse
import json
import os
import shutil
import sys


def is_snake_case_path(p):
    name = os.path.basename(p)
    stem = os.path.splitext(name)[0]
    return all(c == "_" or c.isdigit() or c.islower() for c in stem.replace("-", "_") if c.isalpha() is False or True) and stem == stem.lower()


def case_folded_exists(dest):
    """Detect case-only collisions against the existing destination tree."""
    d, name = os.path.split(dest)
    if not os.path.isdir(d):
        return False
    return any(f.lower() == name.lower() and f != name for f in os.listdir(d))


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--manifest", required=True)
    ap.add_argument("--repo-root", default=None)
    ap.add_argument("--dry-run", action="store_true")
    ap.add_argument("--report", default="artifacts/asset-migration-batch-01-report.json")
    args = ap.parse_args()

    repo_root = args.repo_root or os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
    manifest_path = os.path.join(repo_root, args.manifest) if not os.path.isabs(args.manifest) else args.manifest
    with open(manifest_path, "r", encoding="utf-8") as fh:
        manifest = json.load(fh)

    dest_root = os.path.join(repo_root, manifest.get("destination_root", "assets/"))
    results = []
    failures = 0

    for entry in manifest.get("entries", []):
        src = entry.get("source")
        dst = entry.get("destination")
        row = {"source": src, "destination": dst, "status": "SKIPPED", "issues": []}

        if dst is None:
            row["status"] = entry.get("status", "ORPHAN_HOLD")
            results.append(row)
            continue

        src_path = os.path.join(repo_root, src)
        dst_path = os.path.join(repo_root, dst)

        if not os.path.isfile(src_path):
            # Idempotence: already migrated (source held or removed in a prior run).
            if os.path.isfile(dst_path):
                row["status"] = "ALREADY_MIGRATED"
            else:
                row["status"] = "FAIL"
                row["issues"].append(f"source missing and destination absent: {src}")
                failures += 1
            results.append(row)
            continue

        if not dst_path.startswith(dest_root):
            row["status"] = "FAIL"
            row["issues"].append(f"destination outside {manifest.get('destination_root')}: {dst}")
            failures += 1
            results.append(row)
            continue

        if not is_snake_case_path(dst_path):
            row["status"] = "FAIL"
            row["issues"].append(f"destination not snake_case: {dst}")
            failures += 1
            results.append(row)
            continue

        if os.path.isfile(dst_path):
            row["status"] = "ALREADY_MIGRATED"
            results.append(row)
            continue

        if case_folded_exists(dst_path):
            row["status"] = "FAIL"
            row["issues"].append(f"case-only collision in destination directory: {dst}")
            failures += 1
            results.append(row)
            continue

        if args.dry_run:
            row["status"] = "DRY_RUN_WOULD_COPY"
        else:
            os.makedirs(os.path.dirname(dst_path), exist_ok=True)
            shutil.copy2(src_path, dst_path)  # copy-first: source is never removed
            row["status"] = "COPIED"
        results.append(row)

    report = {
        "schema_version": 1,
        "manifest": args.manifest,
        "dry_run": args.dry_run,
        "entries": results,
        "failures": failures,
    }
    out = os.path.join(repo_root, args.report)
    os.makedirs(os.path.dirname(out), exist_ok=True)
    with open(out, "w", encoding="utf-8") as fh:
        json.dump(report, fh, indent=2, sort_keys=True)
        fh.write("\n")

    for r in results:
        line = f"{r['status']:>22}  {r['source']}"
        if r["issues"]:
            line += "  -- " + "; ".join(r["issues"])
        print(line)
    print(f"report -> {os.path.relpath(out, repo_root)} (failures={failures})")
    return 1 if failures else 0


if __name__ == "__main__":
    sys.exit(main())
