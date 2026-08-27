#!/usr/bin/env python3
"""
normalize-doc-links.py — Portable Relative-Link Normalizer and Validator

Converts machine-specific absolute file URIs (e.g. `file:///home/robertsrff/...`, `/home/...`)
into portable, repository-relative markdown links.

Usage:
  python3 scripts/ci/normalize-doc-links.py --check   # Check for machine-specific links and broken relative links
  python3 scripts/ci/normalize-doc-links.py --write   # Normalize all machine-specific links in place
"""

import os
import re
import sys
import pathlib
import urllib.parse

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent

# Patterns for machine-specific links
# Matches:
#   file:////home/...
#   file:///home/...
#   file:///Users/...
#   file:///C:/...
#   /home/robertsrff/...
FILE_URI_PATTERN = re.compile(
    r'\[([^\]]*)\]\((file:///[^)]+|/home/[^)]+)\)'
)

def resolve_target_rel_to_repo(uri_str):
    # Remove file:/// or file:////
    clean = re.sub(r'^file://+', '', uri_str)
    # Unquote URL encoding (%20 -> space)
    clean = urllib.parse.unquote(clean)

    # Split off anchor if present
    anchor = ""
    if "#" in clean:
        parts = clean.split("#", 1)
        clean = parts[0]
        anchor = "#" + parts[1]

    # If it contains repository directory marker
    marker = "Atomic War"
    alt_marker = "GermanRobert-Labtester/Atomic-War-Starving-Survival"
    alt_marker2 = "Atomic_War_Straving_Survival/Atomic War"

    target_path = None
    if alt_marker2 in clean:
        sub = clean.split(alt_marker2, 1)[1].lstrip('/')
        target_path = REPO_ROOT / sub
    elif marker in clean:
        sub = clean.split(marker, 1)[1].lstrip('/')
        target_path = REPO_ROOT / sub
    elif alt_marker in clean:
        sub = clean.split(alt_marker, 1)[1].lstrip('/')
        target_path = REPO_ROOT / sub
    elif clean.startswith(REPO_ROOT.as_posix()):
        sub = clean[len(REPO_ROOT.as_posix()):].lstrip('/')
        target_path = REPO_ROOT / sub
    else:
        # Check if clean path matches something in repo directly
        possible = (REPO_ROOT / clean.lstrip('/')).resolve()
        if possible.exists():
            target_path = possible

    return target_path, anchor

def process_markdown_file(file_path, write_mode=False):
    content = file_path.read_text(encoding="utf-8", errors="ignore")
    modified = False
    violations = []

    def replace_link(match):
        nonlocal modified
        label = match.group(1)
        uri = match.group(2)

        target_path, anchor = resolve_target_rel_to_repo(uri)
        if target_path:
            # Compute relative path from file_path.parent to target_path
            try:
                rel_path = os.path.relpath(target_path, file_path.parent).replace('\\', '/')
                new_link = f"[{label}]({rel_path}{anchor})"
                modified = True
                return new_link
            except ValueError:
                pass

        violations.append((uri, match.group(0)))
        return match.group(0)

    new_content = FILE_URI_PATTERN.sub(replace_link, content)

    if write_mode and modified:
        file_path.write_text(new_content, encoding="utf-8")

    return modified, violations

def main():
    write_mode = "--write" in sys.argv
    check_mode = "--check" in sys.argv or not write_mode

    md_files = []
    for p in REPO_ROOT.rglob("*.md"):
        if any(ignored in p.parts for ignored in [".git", "build", "obj", "bin", "artifacts", "node_modules"]):
            continue
        md_files.append(p)

    total_modified = 0
    total_violations = 0

    for mf in sorted(md_files):
        mod, viols = process_markdown_file(mf, write_mode=write_mode)
        if mod:
            total_modified += 1
            if write_mode:
                print(f"Normalized: {mf.relative_to(REPO_ROOT)}")
        if viols:
            total_violations += len(viols)
            for v_uri, v_full in viols:
                print(f"Machine-specific link in {mf.relative_to(REPO_ROOT)}: {v_uri}")

    if write_mode:
        print(f"\nSuccessfully normalized links across {total_modified} markdown file(s).")
        return 0

    if total_violations > 0 or total_modified > 0:
        print(f"\nFAILED: Found {total_violations} unresolvable machine link(s) and {total_modified} file(s) with absolute links needing normalization.")
        print("Run `python3 scripts/ci/normalize-doc-links.py --write` to normalize in place.")
        return 1

    print(f"OK: All markdown documents use portable relative links ({len(md_files)} files checked).")
    return 0

if __name__ == "__main__":
    sys.exit(main())
