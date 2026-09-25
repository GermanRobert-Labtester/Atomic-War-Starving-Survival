#!/usr/bin/env python3
"""
Scans all plans across docs/ and Twin_ASHFall/ (excluding README, templates, etc.)
and outputs the N lowest character-count plans to a JSON file.
"""
import os, sys, json

BASE = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

def scan_plans(n=685, output_file=None):
    scan_dirs = [
        os.path.join(BASE, "docs"),
        os.path.join(BASE, "Twin_ASHFall")
    ]

    records = []
    for sdir in scan_dirs:
        for root, dirs, files in os.walk(sdir):
            for f in files:
                if not f.endswith(".md"):
                    continue
                if f.upper().startswith("README"):
                    continue
                full_path = os.path.join(root, f)
                try:
                    with open(full_path, "r", encoding="utf-8") as fp:
                        content = fp.read()
                    char_count = len(content)
                    records.append({
                        "path": full_path,
                        "rel_path": os.path.relpath(full_path, BASE),
                        "chars": char_count
                    })
                except Exception as e:
                    pass

    records.sort(key=lambda r: r["chars"])

    print(f"Total plans discovered: {len(records):,}")
    if records:
        print(f"Global min: {records[0]['chars']:,} ({records[0]['rel_path']})")
        print(f"Global max: {records[-1]['chars']:,} ({records[-1]['rel_path']})")
        total_chars = sum(r["chars"] for r in records)
        print(f"Total corpus chars: {total_chars:,}")

    candidates = records[:n]
    if candidates:
        print(f"\nTop {len(candidates)} lowest plans:")
        print(f"Lowest candidate: {candidates[0]['chars']:,} ({candidates[0]['rel_path']})")
        print(f"Highest candidate: {candidates[-1]['chars']:,} ({candidates[-1]['rel_path']})")

    if output_file:
        with open(output_file, "w", encoding="utf-8") as out:
            json.dump(candidates, out, indent=2)
        print(f"Wrote {len(candidates)} candidates to {output_file}")

    return records, candidates

if __name__ == "__main__":
    n = int(sys.argv[1]) if len(sys.argv) > 1 else 685
    out = sys.argv[2] if len(sys.argv) > 2 else os.path.join(BASE, "scripts/tools/batch_candidates.json")
    scan_plans(n, out)
