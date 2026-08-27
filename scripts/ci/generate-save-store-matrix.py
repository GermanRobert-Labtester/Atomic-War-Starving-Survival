#!/usr/bin/env python3
"""
generate-save-store-matrix.py — Save-Store Contract Matrix Generator & Completeness Gate

Catalogs and validates all save store classes, their static persistence methods,
target JSON files, checksum protections, slot root isolation, and unit test coverage.

Usage:
  python3 scripts/ci/generate-save-store-matrix.py           # Regenerates docs/saves/SAVE_STORE_CONTRACT_MATRIX.md
  python3 scripts/ci/generate-save-store-matrix.py --check   # Verifies docs/saves/SAVE_STORE_CONTRACT_MATRIX.md is in sync and compliant
"""

import datetime
import glob
import os
import re
import sys
import pathlib

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
DOC_PATH = REPO_ROOT / "docs" / "saves" / "SAVE_STORE_CONTRACT_MATRIX.md"

def extract_save_stores():
    search_paths = [
        REPO_ROOT / "src",
        REPO_ROOT / "Assets" / "Ashfall.Core"
    ]

    cs_files = []
    for sp in search_paths:
        for p in sp.rglob("*.cs"):
            s = str(p)
            if "/obj/" in s or "/bin/" in s: continue
            if s.endswith("SelfTest.cs") or s.endswith("Tests.cs") or s.endswith("Test.cs"): continue
            cs_files.append(p)

    # Load all unit tests to check test coverage
    test_files = list((REPO_ROOT / "Ashfall.Core.Tests").rglob("*.cs"))
    test_contents = {tf.name: tf.read_text(encoding="utf-8") for tf in test_files}

    stores = []

    for f in sorted(cs_files):
        content = f.read_text(encoding="utf-8")
        rel_file = f.relative_to(REPO_ROOT).as_posix()

        # Look for classes ending with SaveStore or classes with SavePath / SectionName
        class_matches = list(re.finditer(r"public\s+(?:static\s+|sealed\s+)?class\s+([A-Za-z0-9_]*SaveStore[A-Za-z0-9_]*|[A-Za-z0-9_]+)\b", content))
        for i, cm in enumerate(class_matches):
            cname = cm.group(1)
            cstart = cm.start()
            cend = class_matches[i+1].start() if i+1 < len(class_matches) else len(content)
            cbody = content[cstart:cend]

            # Check if this class is a gameplay SaveStore
            is_store_named = cname.endswith("SaveStore")
            has_save_path = "SavePath" in cbody
            has_section = "SectionName" in cbody
            has_static_save = bool(re.search(r"public\s+static\s+[^\n(]+\s+(Save|TryLoad|Load|Exists|Delete|RestoreSave)\s*\(", cbody))

            if not is_store_named and not (has_static_save and (has_save_path and has_section)):
                continue

            # Extract section name
            sec_match = re.search(r'public\s+const\s+string\s+SectionName\s*=\s*"([^"]+)"', cbody)
            sec_name = sec_match.group(1) if sec_match else None

            # Extract save filename / json target
            json_matches = re.findall(r'"([a-z0-9_]+\.json(?:|\.bak))"', cbody)
            json_file = json_matches[0] if json_matches else None

            # Extract static save/load methods
            methods = re.findall(r"public\s+static\s+[^\n(]+\s+(Save[A-Za-z0-9_]*|TryLoad[A-Za-z0-9_]*|Load[A-Za-z0-9_]*|Exists[A-Za-z0-9_]*|Delete[A-Za-z0-9_]*|RestoreSave[A-Za-z0-9_]*|SavePayload|RestoreState|CaptureState)\s*\(", cbody)
            unique_methods = sorted(list(set(methods)))

            # Extract checksum / codec
            has_checksum = "Checksum" in cbody or "SaveChecksum" in cbody or "SaveEnvelopeHelper" in cbody
            has_codec = bool(re.search(r"\w*Codec\s*\.\s*(Encode|Decode|TryDecode)", cbody))

            # Extract slot root isolation
            has_slot_root = "SaveSlotRoot" in cbody or "ResolveSlotFile" in cbody or "ResolveSlotPath" in cbody

            # Match tests
            matched_tests = []
            for tname, tcontent in test_contents.items():
                if cname in tcontent or (sec_name and f'"{sec_name}"' in tcontent):
                    matched_tests.append(tname)

            stores.append({
                "class": cname,
                "file": rel_file,
                "section": sec_name or "—",
                "json_file": json_file or "—",
                "methods": unique_methods,
                "has_checksum": has_checksum or has_codec,
                "has_slot_root": has_slot_root,
                "tests": sorted(list(set(matched_tests)))
            })

    # Deduplicate by class name
    deduped = {}
    for s in stores:
        if s["class"] not in deduped:
            deduped[s["class"]] = s

    return list(deduped.values())

def generate_markdown(stores, verified_date=None):
    if not verified_date:
        verified_date = datetime.date.today().isoformat()

    total_stores = len(stores)
    total_methods = sum(len(s["methods"]) for s in stores)
    total_checksummed = sum(1 for s in stores if s["has_checksum"])
    total_slot_isolated = sum(1 for s in stores if s["has_slot_root"])
    total_tested = sum(1 for s in stores if s["tests"])

    lines = [
        "# ASHFALL — Save-Store Contract Matrix & Completeness Authority",
        "",
        f"**Last Verified:** {verified_date}<br>",
        f"**Total Save Stores:** {total_stores} classes<br>",
        f"**Total Static Persistence Methods:** {total_methods} methods<br>",
        f"**Checksum-Protected Stores:** {total_checksummed}/{total_stores} ({total_checksummed/total_stores*100:.1f}%)<br>",
        f"**Slot-Root Isolated Stores:** {total_slot_isolated}/{total_stores} ({total_slot_isolated/total_stores*100:.1f}%)<br>",
        f"**Tested Stores:** {total_tested}/{total_stores} ({total_tested/total_stores*100:.1f}%)",
        "",
        "> **GENERATED FILE — do not edit by hand.**",
        "> Source of truth: All save store classes under `src/` and `Assets/Ashfall.Core/`.",
        "> Generated via: `bash scripts/ci/generate-save-store-matrix.sh`",
        "> CI Completeness Gate: `bash scripts/ci/generate-save-store-matrix.sh --check`",
        "",
        "---",
        "",
        "## 1. Architectural Save-Store Contract Invariants",
        "",
        "1. **Invariant 3 (Save Envelope Integrity):** Every save store must wrap payload state in a `{ State, Checksum }` envelope stamped by `SaveChecksum` or delegate to a Core save codec (`*Codec.Encode / Decode`). Bare unchecksummed stores are strictly rejected.",
        "2. **Slot-Root Isolation:** All save paths must resolve through `SaveSlotRoot.ResolveSlotFile(...)` or `SaveSlotRoot.ResolveSlotPath(...)` so headless self-tests, slots, and profiles execute in isolated environments without mutating default user data.",
        "3. **Declarative Section Alignment:** Every registered `SectionName` must correspond directly to an entry in `SaveSectionRegistry.cs` (`Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`).",
        "",
        "---",
        "",
        "## 2. Save-Store Inventory & Contract Matrix",
        "",
        "| # | Save Store Class | Source File | Section Key | Target JSON File | Methods | Checksum | Slot Root | Test Fixtures |",
        "|---|---|---|---|---|---|:---:|:---:|---|"
    ]

    for i, s in enumerate(sorted(stores, key=lambda x: x["class"]), start=1):
        methods_str = ", ".join(f"`{m}()`" for m in s["methods"]) if s["methods"] else "—"
        checksum_icon = "✅" if s["has_checksum"] else "❌"
        slot_icon = "✅" if s["has_slot_root"] else "❌"
        tests_str = ", ".join(f"`{t}`" for t in s["tests"][:2]) + (f" *(+{len(s['tests'])-2} more)*" if len(s['tests']) > 2 else "") if s["tests"] else "—"

        lines.append(
            f"| {i} | `{s['class']}` | [`{s['file']}`](file:///{REPO_ROOT.as_posix()}/{s['file']}) | `{s['section']}` | `{s['json_file']}` | {methods_str} | {checksum_icon} | {slot_icon} | {tests_str} |"
        )

    lines.append("")
    return "\n".join(lines)

def main():
    check_mode = "--check" in sys.argv
    stores = extract_save_stores()

    # Validation checks
    errors = []
    for s in stores:
        if not s["has_checksum"]:
            errors.append(f"Store '{s['class']}' in '{s['file']}' is missing checksum protection!")

    if errors:
        print("SAVE-STORE CONTRACT VIOLATIONS DETECTED:", file=sys.stderr)
        for err in errors:
            print(f"  ❌ {err}", file=sys.stderr)
        sys.exit(1)

    verified_date = datetime.date.today().isoformat()

    if check_mode and DOC_PATH.exists():
        current_content = DOC_PATH.read_text(encoding="utf-8")
        date_match = re.search(r"\*\*Last Verified:\*\*\s+(\d{4}-\d{2}-\d{2})", current_content)
        if date_match:
            verified_date = date_match.group(1)

    rendered = generate_markdown(stores, verified_date)

    if check_mode:
        if not DOC_PATH.exists():
            print(f"FAIL: {DOC_PATH} does not exist. Run bash scripts/ci/generate-save-store-matrix.sh", file=sys.stderr)
            sys.exit(1)

        current = DOC_PATH.read_text(encoding="utf-8")
        if current.strip() != rendered.strip():
            print(f"FAIL: {DOC_PATH} is out of sync with current save store implementations.", file=sys.stderr)
            print("Run: bash scripts/ci/generate-save-store-matrix.sh && git commit", file=sys.stderr)
            sys.exit(1)
        else:
            print(f"OK: Save-store matrix is up to date ({len(stores)} store classes verified).")
            sys.exit(0)
    else:
        DOC_PATH.parent.mkdir(parents=True, exist_ok=True)
        DOC_PATH.write_text(rendered, encoding="utf-8")
        print(f"Wrote {DOC_PATH} ({len(stores)} save store classes).")

if __name__ == "__main__":
    main()
