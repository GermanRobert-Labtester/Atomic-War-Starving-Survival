#!/usr/bin/env python3
"""
ASHFALL - Automated Lowest-Plans Candidate Generator
Scans docs/plans and docs/expansions for all substantive plans (size >= 500,000),
sorts by character/byte count ascending, outputs corpus statistics,
and writes the lowest N candidates to the specified candidates JSON file.
"""

import os
import sys
import json

BASE = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

def get_candidates(n=685, output_file=None):
    scan_dirs = ["docs/plans", "docs/expansions"]
    records = []

    for sdir in scan_dirs:
        full_dir = os.path.join(BASE, sdir)
        for root, _, files in os.walk(full_dir):
            for f in files:
                if not f.endswith(".md"):
                    continue
                full_path = os.path.join(root, f)
                try:
                    sz = os.path.getsize(full_path)
                    if sz >= 500000:
                        rel_path = os.path.relpath(full_path, BASE)
                        records.append({
                            "path": rel_path,
                            "size": sz,
                            "name": f
                        })
                except Exception as e:
                    pass

    records.sort(key=lambda r: r["size"])

    print(f"Total substantive plans discovered (>= 500k): {len(records):,}")
    if records:
        print(f"Global min: {records[0]['size']:,} bytes ({records[0]['path']})")
        print(f"Global max: {records[-1]['size']:,} bytes ({records[-1]['path']})")
        total_bytes = sum(r["size"] for r in records)
        print(f"Total corpus size: {total_bytes:,} bytes ({total_bytes / 1e9:.3f} Billion bytes)")

    candidates = records[:n]
    if candidates:
        print(f"\nSelected lowest {len(candidates)} candidates:")
        print(f"  Lowest candidate : {candidates[0]['size']:,} bytes ({candidates[0]['path']})")
        print(f"  Highest candidate: {candidates[-1]['size']:,} bytes ({candidates[-1]['path']})")

    if output_file:
        out_path = os.path.join(BASE, output_file) if not os.path.isabs(output_file) else output_file
        os.makedirs(os.path.dirname(out_path), exist_ok=True)
        with open(out_path, "w", encoding="utf-8") as out:
            json.dump(candidates, out, indent=2)
        print(f"Successfully saved {len(candidates)} candidates to {out_path}")

    return records, candidates

if __name__ == "__main__":
    count = int(sys.argv[1]) if len(sys.argv) > 1 else 685
    out_file = sys.argv[2] if len(sys.argv) > 2 else None
    get_candidates(count, out_file)
