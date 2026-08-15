#!/usr/bin/env python3
"""ASHFALL text-merge tool: applies authored text batches to master JSON data.

Usage: python3 tools/merge_item_text.py
Validates ids against the master list (no invented ids, no missing ids),
preserves all other fields, and reports word-count stats per file.
"""
import json
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
DATA = ROOT / "Assets" / "StreamingAssets" / "Data"
BATCHES = Path(__file__).resolve().parent / "item_text_batches"

PLANS = [
    # (source json, target json, target field, label)
    ("items_out_batch_1.json", "items.json", "description", "items b1"),
    ("items_out_batch_2.json", "items.json", "description", "items b2"),
    ("items_out_batch_3.json", "items.json", "description", "items b3"),
    ("items_out_batch_4.json", "items.json", "description", "items b4"),
    ("survivors_out.json", "survivors.json", "bio", "survivors"),
    ("locations_out.json", "locations.json", "description", "locations"),
    ("events_out.json", "events.json", "bodyText", "events"),
]

ASCII_RE = re.compile(r"^[\x20-\x7e]*$")


def fail(msg):
    print(f"FAIL: {msg}")
    sys.exit(1)


def load(path):
    with open(path, "r", encoding="utf-8") as f:
        return json.load(f)


def wordcount(s):
    return len(s.split())


def main():
    errors = []

    # Aggregate per target file
    by_target = {}
    for batch_file, target_file, field, label in PLANS:
        by_target.setdefault(target_file, []).append((batch_file, field, label))

    for target_file, batches in by_target.items():
        target = load(DATA / target_file)
        by_id = {e["id"]: e for e in target}
        seen = {}
        total_words = 0
        short = []

        for batch_file, field, label in batches:
            batch_path = BATCHES / batch_file
            if not batch_path.exists():
                fail(f"missing batch file: {batch_path}")
            batch = load(batch_path)

            # Batch entries: id uniqueness (within and across batches)
            for entry in batch:
                eid = entry.get("id")
                if eid in seen:
                    errors.append(f"[{label}] duplicate id '{eid}'")
                seen[eid] = (entry, field)

            # Text quality checks
            for entry in batch:
                eid = entry.get("id")
                text = entry.get(field) or ""
                total_words += wordcount(text)
                if not text.strip():
                    errors.append(f"[{label}] empty {field} for '{eid}'")
                if not ASCII_RE.match(text):
                    errors.append(f"[{label}] non-ASCII chars in '{eid}'")
                if "\u2014" in text or "\u2013" in text or "\u2018" in text or "\u2019" in text or "\u201c" in text or "\u201d" in text:
                    errors.append(f"[{label}] smart punctuation in '{eid}'")
                if wordcount(text) < 25:
                    short.append(eid)
            print(f"[{label}] ok: {len(batch)} entries")

        target_ids = set(by_id.keys())
        batch_ids = set(seen.keys())

        missing_in_batch = target_ids - batch_ids
        if missing_in_batch:
            errors.append(f"[{target_file}] target ids missing from batches ({len(missing_in_batch)}): "
                          + ", ".join(sorted(missing_in_batch)[:10]))

        invented = batch_ids - target_ids
        if invented:
            errors.append(f"[{target_file}] invented ids not in master list: "
                          + ", ".join(sorted(invented)[:10]))

        if not errors:
            avg = total_words // max(len(target), 1)
            print(f"  coverage: {len(batch_ids)}/{len(target_ids)} ids, avg {avg} words")
            if short:
                print(f"  note: {len(short)} entries under 25 words: {', '.join(short[:8])}")

            # Apply (preserve ordering of the target file; use each batch's field name)
            for e in target:
                if e["id"] in seen:
                    entry, field = seen[e["id"]]
                    e[field] = entry[field]

            with open(DATA / target_file, "w", encoding="utf-8") as f:
                json.dump(target, f, indent=2, ensure_ascii=False)
                f.write("\n")
            print(f"  wrote {target_file}")

    if errors:
        print("\n".join(errors))
        sys.exit(1)

    # Final id sanity: snake_case, no duplicates
    for fname in ["items.json", "survivors.json", "locations.json", "events.json"]:
        data = load(DATA / fname)
        ids = [e["id"] for e in data]
        if len(set(ids)) != len(ids):
            fail(f"{fname}: duplicate ids")
        bad = [i for i in ids if not re.match(r"^[a-z][a-z0-9_]*$", i)]
        if bad:
            fail(f"{fname}: non-snake ids {bad[:10]}")
        print(f"PASS {fname}: {len(ids)} ids, all snake_case, no duplicates")

    print("ALL MERGES PASS")


if __name__ == "__main__":
    main()
