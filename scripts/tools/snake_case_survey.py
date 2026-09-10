#!/usr/bin/env python3
"""ASHFALL snake_case migration survey (Plan 47 / Task 13, Phase 13A).

Walks the top-level data catalogs, records every non-snake_case JSON key,
and ranks catalogs into wave candidates by a conservative risk score.

Deterministic: ordinal (byte) ordering everywhere. Offline, read-only.

Output: artifacts/snake-case-survey.json

Usage:
    python3 scripts/tools/snake_case_survey.py [--data-dir PATH] [--out PATH]
"""

import argparse
import collections
import json
import os
import re
import sys

SNAKE_RE = re.compile(r"[a-z0-9_]+")

# Keys that are structural and must never be ranked as violations.
STRUCTURAL_WHITELIST = {"$schema"}

# Rough per-key penalty when a loader reads raw JSON strings instead of typed DTOs.
RAW_JSON_PENALTY = 3.0


def is_snake_case(key: str) -> bool:
    return bool(SNAKE_RE.fullmatch(key)) or key in STRUCTURAL_WHITELIST


def camel_to_snake(key: str) -> str:
    """Spelling-only mapping: displayName -> display_name. Never changes values."""
    out = []
    for i, ch in enumerate(key):
        if ch.isupper():
            if i > 0 and (not key[i - 1].isupper() or (i + 1 < len(key) and key[i + 1].islower())):
                out.append("_")
            out.append(ch.lower())
        else:
            out.append(ch)
    s = "".join(out)
    s = re.sub(r"_+", "_", s).strip("_")
    # numerals: maxStack2 -> max_stack2 stays; keep simple, spelling-only
    return s


def walk(obj, path, depth, violations):
    if depth > 6:
        return
    if isinstance(obj, dict):
        for k, v in obj.items():
            if not is_snake_case(k):
                violations[k]["count"] += 1
                violations[k]["paths"].add(path + "." + k)
            walk(v, path + "." + k, depth + 1, violations)
    elif isinstance(obj, list):
        for v in obj[:200]:
            walk(v, path + "[]", depth + 1, violations)


def find_loader(repo_root, file_name):
    """Locate Core/host source files that reference this catalog by file name."""
    hits = []
    for base in ("Assets/Ashfall.Core", "src"):
        d = os.path.join(repo_root, base)
        for dirpath, _dirs, files in os.walk(d):
            for f in files:
                if not f.endswith(".cs") or f.endswith(".uid"):
                    continue
                p = os.path.join(dirpath, f)
                try:
                    with open(p, "r", encoding="utf-8", errors="ignore") as fh:
                        if file_name in fh.read():
                            hits.append(os.path.relpath(p, repo_root))
                except OSError:
                    continue
    return sorted(hits, key=str)


def consumer_count(repo_root, file_name):
    n = 0
    for base in ("Assets/Ashfall.Core", "src", "Ashfall.Core.Tests", "scripts", "scenes"):
        d = os.path.join(repo_root, base)
        if not os.path.isdir(d):
            continue
        for dirpath, _dirs, files in os.walk(d):
            for f in files:
                if not (f.endswith(".cs") or f.endswith(".tscn")) or f.endswith(".uid"):
                    continue
                p = os.path.join(dirpath, f)
                try:
                    with open(p, "r", encoding="utf-8", errors="ignore") as fh:
                        if file_name in fh.read():
                            n += 1
                except OSError:
                    continue
    return n


def raw_json_consumers(repo_root, old_keys):
    """Count live source files that read any legacy key spelling as a raw string."""
    hits = collections.Counter()
    for key in old_keys:
        needle = '"' + key + '"'
        for base in ("Assets/Ashfall.Core", "src"):
            d = os.path.join(repo_root, base)
            for dirpath, _dirs, files in os.walk(d):
                for f in files:
                    if not f.endswith(".cs") or f.endswith(".uid"):
                        continue
                    p = os.path.join(dirpath, f)
                    try:
                        with open(p, "r", encoding="utf-8", errors="ignore") as fh:
                            if needle in fh.read():
                                hits[key] += 1
                    except OSError:
                        continue
    return hits


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--data-dir", default="Assets/StreamingAssets/Data")
    ap.add_argument("--out", default="artifacts/snake-case-survey.json")
    ap.add_argument("--narrative-subdir", default="narrative")
    args = ap.parse_args()

    repo_root = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
    data_dir = os.path.join(repo_root, args.data_dir)
    narrative_dir = os.path.join(data_dir, args.narrative_subdir)

    rows = []
    for f in sorted(os.listdir(data_dir), key=str):
        if not f.endswith(".json"):
            continue
        path = os.path.join(data_dir, f)
        try:
            with open(path, "r", encoding="utf-8") as fh:
                data = json.load(fh)
        except Exception:
            continue
        violations = collections.defaultdict(lambda: {"count": 0, "paths": set()})
        walk(data, "$", 0, violations)
        if not violations:
            continue
        old_keys = sorted(violations, key=str)
        loaders = find_loader(repo_root, f)
        consumers = consumer_count(repo_root, f)
        raw = raw_json_consumers(repo_root, old_keys)
        raw_penalty = 1.0 + (RAW_JSON_PENALTY if raw else 0.0)
        schema_version = data.get("schema_version") if isinstance(data, dict) else None
        # Risk score: more violations × more consumers × raw-json penalty × key breadth.
        total_violations = sum(violations[k]["count"] for k in old_keys)
        risk = (
            total_violations
            * max(1, consumers)
            * raw_penalty
            * (1.0 + len(old_keys) / 20.0)
        )
        rows.append({
            "file": f,
            "schema_version": schema_version,
            "violation_keys": old_keys,
            "unique_violations": len(old_keys),
            "total_occurrences": total_violations,
            "loaders": loaders,
            "consumer_file_count": consumers,
            "raw_json_key_hits": dict(sorted(raw.items(), key=lambda kv: kv[0])),
            "suggested_canonical": {k: camel_to_snake(k) for k in old_keys},
            "risk_score": round(risk, 2),
        })

    # Narrative corpus: aggregate naming state only (single summary row set).
    narrative_files = 0
    narrative_snake_clean = 0
    narrative_violation_keys = collections.Counter()
    if os.path.isdir(narrative_dir):
        for f in sorted(os.listdir(narrative_dir), key=str):
            if not f.endswith(".json"):
                continue
            narrative_files += 1
            try:
                with open(os.path.join(narrative_dir, f), "r", encoding="utf-8") as fh:
                    data = json.load(fh)
            except Exception:
                continue
            v = collections.defaultdict(lambda: {"count": 0, "paths": set()})
            walk(data, "$", 0, v)
            if not v:
                narrative_snake_clean += 1
            else:
                for k in v:
                    narrative_violation_keys[k] += v[k]["count"]

    report = {
        "schema_version": "1.0.0",
        "tool": "scripts/tools/snake_case_survey.py",
        "ordering": "ordinal",
        "summary": {
            "root_catalogs_scanned": len([f for f in os.listdir(data_dir) if f.endswith(".json")]),
            "root_catalogs_with_violations": len(rows),
            "narrative_files_scanned": narrative_files,
            "narrative_files_snake_clean": narrative_snake_clean,
        },
        "narrative_aggregate_violations": {
            k: narrative_violation_keys[k] for k in sorted(narrative_violation_keys, key=str)
        },
        "catalogs": rows,
    }

    os.makedirs(os.path.dirname(os.path.join(repo_root, args.out)), exist_ok=True)
    out_path = os.path.join(repo_root, args.out)
    with open(out_path, "w", encoding="utf-8") as fh:
        json.dump(report, fh, indent=2, sort_keys=True)
        fh.write("\n")

    print(f"survey: {report['summary']['root_catalogs_with_violations']} catalogs with violations "
          f"-> {os.path.relpath(out_path, repo_root)}")
    for row in sorted(rows, key=lambda r: r["risk_score"]):
        print(f"  {row['risk_score']:>10.2f}  {row['file']}  keys={row['unique_violations']} "
              f"consumers={row['consumer_file_count']}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
