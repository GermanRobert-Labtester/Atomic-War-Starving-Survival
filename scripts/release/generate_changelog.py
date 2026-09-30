#!/usr/bin/env python3
"""
scripts/release/generate_changelog.py — Plan 48 / C2[21]
=============================================================================
Deterministic git-range changelog generation with read-only marker checks.

Generation replaces only the machine-generated region. Save, data and mod
paths are review prompts, never inferred compatibility or migration claims.

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
import subprocess
import sys
import tempfile
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
    with open(path, encoding="utf-8", newline="") as fh:
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
# Generation mode
# ---------------------------------------------------------------------------
def run_generate(args: argparse.Namespace) -> int:
    if not args.version or not re.fullmatch(r"(?:0|[1-9][0-9]*)\.(?:0|[1-9][0-9]*)\.(?:0|[1-9][0-9]*)", args.version):
        print("generate_changelog: --version must be strict semver X.Y.Z")
        return 1
    if not args.base:
        print("generate_changelog: --base is required for generation mode")
        return 1
    root = args.repo_root or REPO_ROOT
    content = read_changelog(root)
    if content is None:
        print(f"generate_changelog: CHANGELOG.md not found in {root}")
        return 1
    errors = validate_markers(content)
    if errors:
        print("generate_changelog: " + "; ".join(errors))
        return 1

    def git(*arguments: str) -> str:
        return subprocess.run(
            ["git", "-c", "core.fsmonitor=false", "-C", root, *arguments],
            check=True, capture_output=True, text=True, encoding="utf-8",
        ).stdout.rstrip("\r\n")

    try:
        base = git("rev-parse", "--verify", "--end-of-options", args.base + "^{commit}")
        head = git("rev-parse", "--verify", "HEAD^{commit}")
        git("merge-base", "--is-ancestor", base, head)
        commit_range = base + ".." + head
        rows = git("log", "--no-show-signature", "--reverse", "--topo-order", "--format=%H%x09%s", commit_range)
        paths = git("diff", "--name-only", "--diff-filter=ACDMRT", base, head).splitlines()
    except (subprocess.CalledProcessError, OSError):
        print("generate_changelog: cannot resolve an ancestor base..HEAD git range")
        return 1

    groups: dict[str, list[str]] = {"Added": [], "Fixed": [], "Changed": []}
    for row in rows.splitlines():
        sha, subject = row.split("\t", 1)
        subject = subject or "(empty commit subject)"
        match = re.match(r"^(feat|fix|[^: ]+)(?:\([^)]*\))?!?:\s*(.*)$", subject)
        category = "Added" if match and match[1] == "feat" else "Fixed" if match and match[1] == "fix" else "Changed"
        # Commit messages are data, not Markdown markup or HTML.
        escaped = subject.replace("&", "&amp;").replace("<", "&lt;").replace(">", "&gt;")
        escaped = re.sub(r"([\\`*_\[\]])", r"\\\1", escaped)
        groups[category].append(f"- {escaped} (`{sha[:12]}`)")

    lines = [f"### Release {args.version}", "", f"Commit range: `{base}` → `{head}`."]
    for category, entries in groups.items():
        if entries:
            lines.extend(["", f"#### {category}", "", *entries])
    if not rows:
        lines.extend(["", "No commits in this range."])
    for label, prefixes in (
        ("Save compatibility review", ("Assets/Ashfall.Core/Save", "Assets/Ashfall.Core/Serialization", "artifacts/golden_saves/")),
        ("Authored data review", ("Assets/StreamingAssets/Data/",)),
        ("Mod compatibility review", ("Assets/Ashfall.Core/Mod", "docs/mod", "mods/")),
    ):
        relevant = sorted(p for p in paths if p.startswith(prefixes))
        if relevant:
            lines.extend(["", f"#### {label}", "", "Changed paths; review required before making compatibility claims.", ""])
            lines.extend("- " + p.replace("&", "&amp;").replace("<", "&lt;").replace(">", "&gt;").replace("`", "\\`") for p in relevant)
    newline = "\r\n" if "\r\n" in content else "\n"
    body = newline * 2 + newline.join(lines) + newline * 2
    if MARKER_BEGIN in content:
        start = content.index(MARKER_BEGIN) + len(MARKER_BEGIN)
        end = content.index(MARKER_END)
        updated = content[:start] + body + content[end:]
    else:
        headings = list(re.finditer(rf"(?m)^## \[{re.escape(args.version)}\][^\n]*(?:\n|$)", content))
        if not headings:
            headings = list(re.finditer(r"(?mi)^## \[Unreleased\][^\n]*(?:\n|$)", content))
        if len(headings) != 1:
            print("generate_changelog: expected one unambiguous target version or Unreleased heading")
            return 1
        start = headings[0].end()
        updated = content[:start] + newline + MARKER_BEGIN + body + MARKER_END + newline + content[start:]
    path = find_changelog(root)
    if updated != content:
        temporary = None
        try:
            with tempfile.NamedTemporaryFile(mode="w", encoding="utf-8", newline="", dir=root, delete=False) as fh:
                temporary = fh.name
                fh.write(updated)
            os.chmod(temporary, os.stat(path).st_mode & 0o777)
            os.replace(temporary, path)
        finally:
            if temporary and os.path.exists(temporary):
                os.unlink(temporary)
    print(f"generate_changelog: wrote release {args.version} ({len(rows.splitlines())} commits)")
    return 0


# ---------------------------------------------------------------------------
# Entry point
# ---------------------------------------------------------------------------
def main() -> int:
    parser = argparse.ArgumentParser(
        description="ASHFALL changelog generator (Plan 48 / C2[21])",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog=textwrap.dedent("""\
            --check validates marker structure without changing the file.
            Generation uses an ancestor base..HEAD range and replaces the marker body.

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
