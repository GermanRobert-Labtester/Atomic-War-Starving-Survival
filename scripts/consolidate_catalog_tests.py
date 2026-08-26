#!/usr/bin/env python3
"""
Batch-consolidate ASHFALL catalog test files to inherit CatalogTestBase.

Handles two patterns (first match only):
1. Class with existing base: `class Foo : Bar` → `class Foo : CatalogTestBase, Bar`
2. Class without base: `class Foo {` → `class Foo : CatalogTestBase {`
"""

import argparse
import re
import sys
from pathlib import Path

CATALOG_TESTS_DIR = Path("Ashfall.Core.Tests")


def inherits_catalog_test_base(content: str) -> bool:
    return ": CatalogTestBase" in content


def consolidate_file(filepath: Path, dry_run: bool = True) -> tuple[bool, str]:
    """Consolidate a single catalog test file."""
    content = filepath.read_text()
    original = content

    if inherits_catalog_test_base(content):
        return False, "already inherits CatalogTestBase"

    # Pattern 1: class with existing base class (first match only)
    new_content, count1 = re.subn(
        r'(public\s+(?:sealed\s+)?class\s+\w+CatalogTests\s*:\s*)(?!CatalogTestBase)',
        r'\1CatalogTestBase, ',
        content,
        count=1
    )

    # Pattern 2: class without base class (first match only)
    if count1 == 0:
        new_content, count2 = re.subn(
            r'(public\s+(?:sealed\s+)?class\s+\w+CatalogTests\s*)(\s*\{)',
            r'\1: CatalogTestBase\2',
            content,
            count=1
        )
    else:
        count2 = 0

    if new_content != original:
        if not dry_run:
            filepath.write_text(new_content)
        return True, "added CatalogTestBase inheritance"

    return False, "no changes needed"


def main():
    parser = argparse.ArgumentParser(description="Consolidate catalog test files")
    parser.add_argument("--check", action="store_true", help="Dry run")
    parser.add_argument("--write", action="store_true", help="Apply changes")
    args = parser.parse_args()

    dry_run = not args.write
    mode = "CHECK" if dry_run else "WRITE"

    print(f"Catalog test consolidation — mode: {mode}")
    print()

    files = [f for f in CATALOG_TESTS_DIR.glob("*CatalogTests.cs") if not inherits_catalog_test_base(f.read_text())]
    print(f"Found {len(files)} files to potentially consolidate")

    changed = 0
    skipped = 0
    errors = 0

    for f in sorted(files):
        try:
            ok, msg = consolidate_file(f, dry_run=dry_run)
            if ok:
                changed += 1
                print(f"[CHANGED] {f.name}: {msg}")
            else:
                skipped += 1
                if "skipped" not in msg and "already" not in msg:
                    print(f"[SKIP] {f.name}: {msg}")
        except Exception as e:
            errors += 1
            print(f"[ERROR] {f.name}: {e}")

    print()
    print(f"Summary: {changed} changed, {skipped} skipped, {errors} errors")

    if errors > 0:
        sys.exit(1)


if __name__ == "__main__":
    main()
