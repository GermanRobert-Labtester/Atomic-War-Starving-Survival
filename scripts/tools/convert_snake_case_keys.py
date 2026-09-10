#!/usr/bin/env python3
"""ASHFALL snake_case catalog conversion (Plan 47 / Task 13, Phase 13E).

Spelling-only JSON key conversion for migrating catalogs:
  - renames keys per an explicit legacy→canonical alias map;
  - never touches values, ID strings, or schema_version;
  - fails loudly on dual-key conflicts (same rule as the Core normalizer);
  - preserves key order, indentation and trailing newline.

Dual-read compatibility is provided by the Core loaders (CatalogKeyNormalizer);
this tool converts the canonical authority files themselves.

Usage:
    python3 scripts/tools/convert_snake_case_keys.py --alias-file scripts/tools/snake_case_aliases.json \
        FILE [FILE ...] [--dry-run]
"""

import argparse
import json
import sys


def load_alias_map(path):
    with open(path, "r", encoding="utf-8") as fh:
        raw = json.load(fh)
    # { "catalog-file-or-*": { legacy: canonical } }
    return raw


def aliases_for(alias_doc, file_name):
    merged = {}
    merged.update(alias_doc.get("*", {}))
    merged.update(alias_doc.get(file_name, {}))
    return merged


def canonical_text(node):
    return json.dumps(node, sort_keys=True, ensure_ascii=False)


def convert(obj, alias_map, path, conflicts, renamed):
    if isinstance(obj, dict):
        out = {}
        for k, v in obj.items():
            if k in alias_map:
                canon = alias_map[k]
                if canon in out and canonical_text(out[canon]) != canonical_text(v):
                    conflicts.append(f"{path}.{k}: '{k}' and '{canon}' conflict")
                else:
                    if k != canon:
                        renamed.append((path + "." + k, k, canon))
                    k = canon
            out[k] = convert(v, alias_map, path + "." + k, conflicts, renamed)
        return out
    if isinstance(obj, list):
        return [convert(v, alias_map, path + "[]", conflicts, renamed) for v in obj]
    return obj


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("files", nargs="+")
    ap.add_argument("--alias-file", required=True)
    ap.add_argument("--dry-run", action="store_true")
    args = ap.parse_args()

    alias_doc = load_alias_map(args.alias_file)
    failures = 0
    for file_path in args.files:
        file_name = file_path.rsplit("/", 1)[-1]
        alias_map = aliases_for(alias_doc, file_name)
        if not alias_map:
            print(f"SKIP {file_path}: no aliases configured")
            continue
        with open(file_path, "r", encoding="utf-8") as fh:
            raw = fh.read()
        data = json.loads(raw)
        conflicts, renamed = [], []
        converted = convert(data, alias_map, "$", conflicts, renamed)
        if conflicts:
            failures += 1
            for c in conflicts:
                print(f"CONFLICT {file_path}: {c}")
            continue
        if not renamed:
            print(f"OK    {file_path}: already canonical")
            continue
        if args.dry_run:
            print(f"DRY   {file_path}: would rename {len(renamed)} keys")
            continue
        text = json.dumps(converted, indent=2, ensure_ascii=False)
        if raw.endswith("\n"):
            text += "\n"
        with open(file_path, "w", encoding="utf-8") as fh:
            fh.write(text)
        print(f"WROTE {file_path}: renamed {len(renamed)} keys")

    return 1 if failures else 0


if __name__ == "__main__":
    sys.exit(main())
