#!/usr/bin/env python3
"""
scripts/release/generate_changelog.py — Plan 48 / C2[21] Phase 2 (skeleton)
=============================================================================
Changelog generation skeleton with --check mode.

At this phase only the machine-generated region is validated (markers present
and well-formed).  Full generation (git-log mapping, diff sections, save/mod
summaries) is implemented in Phase 4.

Generated region markers (must appear exactly once in CHANGELOG.md):
    <!-- generated:begin -->
    <!-- generated:end -->

Usage
-----
    # Check mode (CI gate — does NOT modify CHANGELOG.md):
    python3 scripts/release/generate_changelog.py --check

    # Generation mode (Phase 4 — full output):
    python3 scripts/release/generate_changelog.py --version 1.2.0 --base v1.1.0

Exit codes
----------
0  PASS
1  FAIL
"""
from __future__ import annotations

import argparse
import os
import re
import sys
import textwrap

REPO_ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

MARKER_BEGIN = "<!-- generated:begin -->"
MARKER_END = "<!-- generated:end -->"


# ---------------------------------------------------------------------------
# Helpers
# ---------------------------------------------------------------------------
def find_changelog(root: str) -> str:
    return os.path.join(root, "CHANGELOG.md")


def read_changelog(root: str) -> str | None:
    path = find_changelog(root)
    if not os.path.exists(path):
        return None
    with open(path, encoding="utf-8") as fh:
        return fh.read()


def validate_markers(content: str) -> list[str]:
    """
    Return a list of error strings describing marker problems.
    Empty list means the marker region is well-formed.
    """
    errors: list[str] = []
    begin_count = content.count(MARKER_BEGIN)
    end_count = content.count(MARKER_END)

    if begin_count == 0 and end_count == 0:
        # No markers at all — that is acceptable for --check on a fresh repo
        # (generators add them on first write).  Warn but do not fail.
        return []
    if begin_count != 1:
        errors.append(f"Expected exactly 1 '{MARKER_BEGIN}', found {begin_count}")
    if end_count != 1:
        errors.append(f"Expected exactly 1 '{MARKER_END}', found {end_count}")
    if begin_count == 1 and end_count == 1:
        begin_pos = content.index(MARKER_BEGIN)
        end_pos = content.index(MARKER_END)
        if begin_pos > end_pos:
            errors.append(f"'{MARKER_BEGIN}' appears after '{MARKER_END}'")
    return errors


def validate_version_section(content: str, version: str | None) -> list[str]:
    """If a version is given, confirm a heading for it or [Unreleased] exists."""
    if version is None:
        return []
    if not re.search(
        rf"##\s+\[{re.escape(version)}\]|##\s+\[Unreleased\]", content, re.IGNORECASE
    ):
        return [
            f"CHANGELOG.md has no ## [{version}] or ## [Unreleased] heading"
        ]
    return []


# ---------------------------------------------------------------------------
# Check mode
# ---------------------------------------------------------------------------
def run_check(args: argparse.Namespace) -> int:
    root = args.repo_root or REPO_ROOT
    content = read_changelog(root)
    if content is None:
        print("CHANGELOG_DRIFT FAIL: CHANGELOG.md not found")
        return 1

    errors: list[str] = []
    errors.extend(validate_markers(content))
    errors.extend(validate_version_section(content, args.version))

    if errors:
        for e in errors:
            print(f"  CHANGELOG_DRIFT FAIL: {e}")
        return 1

    print("  CHANGELOG_DRIFT PASS — marker region well-formed")
    return 0


# ---------------------------------------------------------------------------
# Generation mode (Phase 4 stub — no-op for now)
# ---------------------------------------------------------------------------
def run_generate(args: argparse.Namespace) -> int:
    """
    Full generation is implemented in Phase 4.
    This stub validates that required args are present and that CHANGELOG.md
    is reachable, then exits without modifying anything.
    """
    if not args.version:
        print("generate_changelog: --version is required for generation mode")
        return 1
    root = args.repo_root or REPO_ROOT
    content = read_changelog(root)
    if content is None:
        print(f"generate_changelog: CHANGELOG.md not found in {root}")
        return 1
    print(
        f"generate_changelog: generation mode stub "
        f"(version={args.version}, base={args.base}) — Phase 4 will implement full output"
    )
    return 0


# ---------------------------------------------------------------------------
# Entry point
# ---------------------------------------------------------------------------
def main() -> int:
    parser = argparse.ArgumentParser(
        description="ASHFALL changelog generator (Plan 48 / C2[21])",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog=textwrap.dedent("""\
            Phase 2 scope: --check validates marker region only.
            Phase 4 will add full git-log mapping, diff sections, and region writing.

            Examples:
              python3 scripts/release/generate_changelog.py --check
              python3 scripts/release/generate_changelog.py --version 1.2.0 --base v1.1.0
        """),
    )
    parser.add_argument("--check", action="store_true", help="Drift-check mode (CI gate, read-only)")
    parser.add_argument("--version", default=None, help="Target version string (e.g. 1.2.0)")
    parser.add_argument("--base", default=None, help="Base git ref for range (e.g. v1.1.0)")
    parser.add_argument("--repo-root", dest="repo_root", default=None, help="Override repo root path")
    args = parser.parse_args()

    if args.check:
        return run_check(args)

    return run_generate(args)


if __name__ == "__main__":
    sys.exit(main())
