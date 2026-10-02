#!/usr/bin/env python3
"""
json-schema-policy-gate.py — JSON Schema Policy Gate for ASHFALL

Enforces that any new, changed, or existing JSON catalog under
Assets/StreamingAssets/Data/ adheres to the canonical schema policy:
  1. Root must be a JSON object ({}), never a bare array ([]).
  2. Must declare a valid, positive integer 'schema_version' (>= 1).
  3. Must parse as valid UTF-8 JSON.
  4. Top-level keys must not contain null/empty schema declarations.

Usage:
  python3 scripts/ci/json-schema-policy-gate.py           # Validates all JSON catalogs (CI mode)
  python3 scripts/ci/json-schema-policy-gate.py --all     # Validates all JSON catalogs
  python3 scripts/ci/json-schema-policy-gate.py --staged  # Validates only git staged JSON files
  python3 scripts/ci/json-schema-policy-gate.py --diff    # Validates only modified/new JSON files vs HEAD
"""

import argparse
import json
import os
import pathlib
import re
import subprocess
import sys

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
DATA_DIR = REPO_ROOT / "Assets" / "StreamingAssets" / "Data"

def get_staged_json_files():
    try:
        res = subprocess.check_output(
            ["git", "diff", "--cached", "--name-only", "--diff-filter=ACMR"],
            cwd=REPO_ROOT, text=True, timeout=60
        )
        files = []
        for line in res.strip().splitlines():
            if line.endswith(".json") and line.startswith("Assets/StreamingAssets/Data/"):
                p = REPO_ROOT / line
                if p.is_file():
                    files.append(p)
        return files
    except Exception as e:
        print(f"Warning: git command failed: {e}", file=sys.stderr)
        return []

def get_diff_json_files():
    try:
        res = subprocess.check_output(
            ["git", "diff", "HEAD", "--name-only", "--diff-filter=ACMR"],
            cwd=REPO_ROOT, text=True, timeout=60
        )
        files = []
        for line in res.strip().splitlines():
            if line.endswith(".json") and line.startswith("Assets/StreamingAssets/Data/"):
                p = REPO_ROOT / line
                if p.is_file():
                    files.append(p)
        return files
    except Exception as e:
        print(f"Warning: git command failed: {e}", file=sys.stderr)
        return []

def get_all_json_files():
    if not DATA_DIR.is_dir():
        return []
    return sorted(DATA_DIR.rglob("*.json"))

def validate_json_file(file_path: pathlib.Path):
    """
    Validates a single JSON file against schema policy.
    Returns (is_valid, list_of_errors, schema_version_found).
    """
    rel_path = file_path.relative_to(REPO_ROOT).as_posix()
    errors = []

    try:
        raw_text = file_path.read_text(encoding="utf-8")
    except UnicodeDecodeError as ex:
        return False, [f"{rel_path}: Invalid UTF-8 encoding ({ex})"], 0
    except Exception as ex:
        return False, [f"{rel_path}: Failed to read file ({ex})"], 0

    if not raw_text.strip():
        return False, [f"{rel_path}: File is empty"], 0

    try:
        data = json.loads(raw_text)
    except json.JSONDecodeError as ex:
        return False, [f"{rel_path}: Invalid JSON syntax at line {ex.lineno}, col {ex.colno}: {ex.msg}"], 0

    # Rule 1: Root must be an object ({})
    if isinstance(data, list):
        errors.append(
            f"{rel_path}: Root is a bare JSON array ([]). Schema policy requires an object root "
            f"declaring 'schema_version'."
        )
        return False, errors, 0

    if not isinstance(data, dict):
        errors.append(
            f"{rel_path}: Root is not a JSON object ({type(data).__name__}). Schema policy requires an object root."
        )
        return False, errors, 0

    # Rule 2: Top-level schema_version must be declared
    if "schema_version" not in data:
        errors.append(
            f"{rel_path}: Missing mandatory 'schema_version' key at root object."
        )
        return False, errors, 0

    sv = data["schema_version"]

    # Rule 3: schema_version must be an integer >= 1
    if isinstance(sv, bool) or not isinstance(sv, int):
        errors.append(
            f"{rel_path}: 'schema_version' must be an integer (found {type(sv).__name__}: {repr(sv)})."
        )
        return False, errors, 0

    if sv < 1:
        errors.append(
            f"{rel_path}: 'schema_version' must be a positive integer >= 1 (found {sv})."
        )
        return False, errors, sv

    return True, [], sv

# Contract files under docs/ci/ are consumed by CI tooling, not the game runtime,
# so they carry a string schema_version rather than the integer catalog version.
# The scan is data-driven over docs/ci/*.json; NON_CONTRACT_JSON is the explicit
# exclusion list for any future raw data dump (none today).
CI_DIR = REPO_ROOT / "docs" / "ci"
NON_CONTRACT_JSON = set()


def validate_ci_contract_files():
    errors = []
    if not CI_DIR.is_dir():
        return errors
    for p in sorted(CI_DIR.glob("*.json")):
        if p.name in NON_CONTRACT_JSON:
            continue
        rel = p.relative_to(REPO_ROOT).as_posix()
        try:
            data = json.loads(p.read_text(encoding="utf-8"))
        except Exception as ex:
            errors.append(f"{rel}: invalid JSON ({ex})")
            continue
        if not isinstance(data, dict):
            errors.append(f"{rel}: root must be a JSON object")
            continue
        sv_key = next((k for k in data if str(k).lower().replace("_", "") == "schemaversion"), None)
        if sv_key is None:
            errors.append(f"{rel}: missing mandatory 'schema_version' key at root object")
            continue
        if not isinstance(data[sv_key], str):
            errors.append(
                f"{rel}: 'schema_version' must be a string for a docs/ci contract file "
                f"(found {type(data[sv_key]).__name__}: {data[sv_key]!r})"
            )
        elif not re.fullmatch(r"\d+(\.\d+)*", data[sv_key]):
            errors.append(f"{rel}: 'schema_version' {data[sv_key]!r} is not a dotted-numeric version")
    existing = {p.name for p in CI_DIR.glob("*.json")}
    for name in sorted(NON_CONTRACT_JSON):
        if name not in existing:
            errors.append(f"docs/ci/{name}: NON_CONTRACT_JSON exclusion points at a missing file (stale exclusion)")
    return errors


def main():
    parser = argparse.ArgumentParser(description="JSON Schema Policy Gate for ASHFALL")
    group = parser.add_mutually_exclusive_group()
    group.add_argument("--all", action="store_true", help="Validate all JSON catalogs in StreamingAssets/Data")
    group.add_argument("--staged", action="store_true", help="Validate only staged JSON files")
    group.add_argument("--diff", action="store_true", help="Validate modified JSON files compared to HEAD")

    args = parser.parse_args()

    if args.staged:
        files = get_staged_json_files()
        mode_label = f"staged JSON files ({len(files)} file(s))"
    elif args.diff:
        files = get_diff_json_files()
        mode_label = f"changed JSON files vs HEAD ({len(files)} file(s))"
    else:
        files = get_all_json_files()
        mode_label = f"all StreamingAssets/Data JSON catalogs ({len(files)} file(s))"

    if not files:
        if args.staged or args.diff:
            print(f"OK: No {mode_label} to check.")
            sys.exit(0)
        else:
            print(f"ERROR: No JSON files found under {DATA_DIR}", file=sys.stderr)
            sys.exit(1)

    print(f"── Validating JSON Schema Policy on {mode_label} ──")

    all_errors = []
    version_counts = {}

    for f in files:
        ok, errs, sv = validate_json_file(f)
        if not ok:
            all_errors.extend(errs)
        else:
            version_counts[sv] = version_counts.get(sv, 0) + 1

    # CI/full mode also validates the docs/ci/ contract files (E10).
    if not (args.staged or args.diff):
        contract_errors = validate_ci_contract_files()
        all_errors.extend(contract_errors)

    if all_errors:
        print("\nJSON SCHEMA POLICY VIOLATIONS DETECTED:", file=sys.stderr)
        for err in all_errors:
            print(f"  • {err}", file=sys.stderr)
        print(f"\nTotal violations: {len(all_errors)} in {len(files)} scanned files.", file=sys.stderr)
        print("Policy rule: All new/modified JSON files must be object-rooted and declare 'schema_version' >= 1.", file=sys.stderr)
        sys.exit(1)

    version_breakdown = ", ".join(f"v{v}: {count}" for v, count in sorted(version_counts.items()))
    print(f"✅ PASS: All {len(files)} JSON file(s) declare valid schema policy ({version_breakdown}).")
    sys.exit(0)

if __name__ == "__main__":
    main()
