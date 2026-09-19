#!/usr/bin/env python3
"""
scripts/ci/version-gate.py — Plan 48 / C2[21] Phase 2
=======================================================
Drift-enforcement gate: ensures all three canonical version sources agree
on a single strict-semver X.Y.Z value, that the changelog has been updated
when the version changed since the base ref, and that hotfix branches never
carry save-schema constant changes.

Exit codes
----------
0  All checks passed
1  One or more checks failed
"""
from __future__ import annotations

import argparse
import os
import re
import subprocess
import sys
import tempfile
import textwrap

# ---------------------------------------------------------------------------
# Semver helpers
# ---------------------------------------------------------------------------
SEMVER_RE = re.compile(r"^\d+\.\d+\.\d+$")


def is_strict_semver(value: str) -> bool:
    return bool(SEMVER_RE.match(value))


# ---------------------------------------------------------------------------
# Version source readers
# ---------------------------------------------------------------------------
REPO_ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))


def read_project_godot_version(root: str) -> str | None:
    """Return config/version value from project.godot, or None."""
    path = os.path.join(root, "project.godot")
    if not os.path.exists(path):
        return None
    with open(path, encoding="utf-8") as fh:
        for line in fh:
            m = re.match(r'^config/version="([^"]+)"', line.strip())
            if m:
                return m.group(1)
    return None


def read_build_props_version(root: str) -> str | None:
    """Return VersionPrefix value from Directory.Build.props, or None."""
    path = os.path.join(root, "Directory.Build.props")
    if not os.path.exists(path):
        return None
    with open(path, encoding="utf-8") as fh:
        for line in fh:
            m = re.search(r"<VersionPrefix>([^<]+)</VersionPrefix>", line)
            if m:
                return m.group(1).strip()
    return None


def read_export_presets_version(root: str) -> str | None:
    """Return file_version from the first Windows export preset, or None."""
    path = os.path.join(root, "export_presets.cfg")
    if not os.path.exists(path):
        return None
    with open(path, encoding="utf-8") as fh:
        for line in fh:
            m = re.match(r'^application/file_version="([^"]+)"', line.strip())
            if m:
                return m.group(1)
    return None


# ---------------------------------------------------------------------------
# CHANGELOG check helpers
# ---------------------------------------------------------------------------
def changelog_has_unreleased_or_version(root: str, version: str) -> bool:
    """Return True if CHANGELOG.md contains a heading for `version` or [Unreleased]."""
    path = os.path.join(root, "CHANGELOG.md")
    if not os.path.exists(path):
        return False
    with open(path, encoding="utf-8") as fh:
        content = fh.read()
    pattern = rf"##\s+\[{re.escape(version)}\]|##\s+\[Unreleased\]"
    return bool(re.search(pattern, content, re.IGNORECASE))


def version_changed_since_base(root: str, base_ref: str) -> bool:
    """
    Return True if project.godot config/version line differs between base_ref
    and the working tree.  Returns False on any git error.
    """
    try:
        old = subprocess.check_output(
            ["git", "show", f"{base_ref}:project.godot"],
            cwd=root,
            stderr=subprocess.DEVNULL,
            text=True,
        )
    except subprocess.CalledProcessError:
        return False
    old_ver = None
    for line in old.splitlines():
        m = re.match(r'^config/version="([^"]+)"', line.strip())
        if m:
            old_ver = m.group(1)
            break
    new_ver = read_project_godot_version(root)
    return old_ver != new_ver


# ---------------------------------------------------------------------------
# Hotfix check — no save-schema constant changes allowed
# ---------------------------------------------------------------------------
SCHEMA_CONSTANT_PATTERN = re.compile(
    r"\bSchemaVersion\s*=\s*\d+|SCHEMA_VERSION\s*=\s*\d+", re.IGNORECASE
)


def detect_schema_constant_changes(root: str, base_ref: str) -> list[str]:
    """
    Return list of files where a save-schema constant changed versus base_ref.
    Empty list means clean.
    """
    try:
        diff_output = subprocess.check_output(
            ["git", "diff", base_ref, "--name-only"],
            cwd=root,
            stderr=subprocess.DEVNULL,
            text=True,
        )
    except subprocess.CalledProcessError:
        return []

    changed = []
    for fname in diff_output.splitlines():
        fname = fname.strip()
        if not fname.endswith(".cs"):
            continue
        abs_path = os.path.join(root, fname)
        if not os.path.exists(abs_path):
            continue
        try:
            file_diff = subprocess.check_output(
                ["git", "diff", base_ref, "--", fname],
                cwd=root,
                stderr=subprocess.DEVNULL,
                text=True,
            )
        except subprocess.CalledProcessError:
            continue
        added_lines = [l[1:] for l in file_diff.splitlines() if l.startswith("+") and not l.startswith("+++")]
        for line in added_lines:
            if SCHEMA_CONSTANT_PATTERN.search(line):
                changed.append(fname)
                break
    return changed


# ---------------------------------------------------------------------------
# Self-test mode
# ---------------------------------------------------------------------------
def run_self_test() -> int:
    """
    Prove failure cases via a temporary fixture tree.
    Returns 0 on success (all expected failures were detected), 1 otherwise.
    """
    print("version-gate self-test: verifying failure detection …")
    failures: list[str] = []

    with tempfile.TemporaryDirectory(prefix="vgate_selftest_") as tmpdir:

        def make_files(godot_ver: str, props_ver: str, export_ver: str) -> None:
            with open(os.path.join(tmpdir, "project.godot"), "w") as f:
                f.write(f'[application]\nconfig/version="{godot_ver}"\n')
            with open(os.path.join(tmpdir, "Directory.Build.props"), "w") as f:
                f.write(f"<Project><PropertyGroup><VersionPrefix>{props_ver}</VersionPrefix></PropertyGroup></Project>\n")
            with open(os.path.join(tmpdir, "export_presets.cfg"), "w") as f:
                f.write(f'[preset.0.options]\napplication/file_version="{export_ver}"\napplication/product_version="{export_ver}"\n')

        # Test 1: all agree — expect PASS
        make_files("1.1.0", "1.1.0", "1.1.0")
        godot = read_project_godot_version(tmpdir)
        props = read_build_props_version(tmpdir)
        export = read_export_presets_version(tmpdir)
        if not (godot == props == export == "1.1.0"):
            failures.append("Test 1: agreement case failed unexpectedly")

        # Test 2: godot vs props mismatch — must detect drift
        make_files("1.1.0", "1.0.0", "1.1.0")
        godot = read_project_godot_version(tmpdir)
        props = read_build_props_version(tmpdir)
        if godot == props:
            failures.append("Test 2: drift not detected (godot != props)")

        # Test 3: non-semver value — must reject
        make_files("1.1", "1.1", "1.1")
        godot = read_project_godot_version(tmpdir)
        if is_strict_semver(godot or ""):
            failures.append("Test 3: non-semver '1.1' accepted as strict semver")

        # Test 4: export preset mismatch — must detect drift
        make_files("1.1.0", "1.1.0", "1.0.9")
        godot = read_project_godot_version(tmpdir)
        export = read_export_presets_version(tmpdir)
        if godot == export:
            failures.append("Test 4: drift not detected (godot != export_presets)")

        # Test 5: missing project.godot — reader returns None
        import shutil
        shutil.copy(os.path.join(tmpdir, "project.godot"), os.path.join(tmpdir, "project.godot.bak"))
        os.remove(os.path.join(tmpdir, "project.godot"))
        godot = read_project_godot_version(tmpdir)
        if godot is not None:
            failures.append("Test 5: missing project.godot should return None")
        shutil.copy(os.path.join(tmpdir, "project.godot.bak"), os.path.join(tmpdir, "project.godot"))

    if failures:
        print("version-gate self-test FAIL:")
        for f in failures:
            print(f"  FAIL: {f}")
        return 1

    print("version-gate self-test PASS — all failure cases detected correctly")
    return 0


# ---------------------------------------------------------------------------
# Main gate logic
# ---------------------------------------------------------------------------
def run_gate(args: argparse.Namespace) -> int:
    root = args.repo_root or REPO_ROOT
    errors: list[str] = []

    # --- Check 1: read all three sources ---
    godot_ver = read_project_godot_version(root)
    props_ver = read_build_props_version(root)
    export_ver = read_export_presets_version(root)

    if godot_ver is None:
        errors.append("project.godot: config/version not found")
    if props_ver is None:
        errors.append("Directory.Build.props: <VersionPrefix> not found")
    if export_ver is None:
        errors.append("export_presets.cfg: application/file_version not found")

    if errors:
        for e in errors:
            print(f"  VERSION_GATE FAIL: {e}")
        return 1

    print(f"  project.godot          : {godot_ver}")
    print(f"  Directory.Build.props  : {props_ver}")
    print(f"  export_presets.cfg     : {export_ver}")

    # --- Check 2: strict semver ---
    for label, ver in [
        ("project.godot", godot_ver),
        ("Directory.Build.props", props_ver),
        ("export_presets.cfg", export_ver),
    ]:
        if not is_strict_semver(ver):
            errors.append(f"{label}: '{ver}' is not strict semver X.Y.Z")

    # --- Check 3: all three sources agree ---
    if godot_ver != props_ver:
        errors.append(f"Version mismatch: project.godot='{godot_ver}' vs Directory.Build.props='{props_ver}'")
    if godot_ver != export_ver:
        errors.append(f"Version mismatch: project.godot='{godot_ver}' vs export_presets.cfg='{export_ver}'")

    if errors:
        for e in errors:
            print(f"  VERSION_GATE FAIL: {e}")
        return 1

    canonical_ver = godot_ver

    # --- Check 4 (optional): changelog bump-link ---
    base_ref = args.base_ref
    if base_ref:
        if version_changed_since_base(root, base_ref):
            if not changelog_has_unreleased_or_version(root, canonical_ver):
                errors.append(
                    f"Version changed since {base_ref} but CHANGELOG.md has no "
                    f"[{canonical_ver}] or [Unreleased] section"
                )

    # --- Check 5 (hotfix mode): no schema-constant movement ---
    if args.hotfix and base_ref:
        schema_violators = detect_schema_constant_changes(root, base_ref)
        if schema_violators:
            errors.append(
                "HOTFIX MODE: save-schema constant changes detected — hotfix commits "
                "must not alter save schema. Violating files:\n  "
                + "\n  ".join(schema_violators)
            )

    if errors:
        for e in errors:
            print(f"  VERSION_GATE FAIL: {e}")
        return 1

    print(f"  VERSION_GATE PASS — canonical version: {canonical_ver}")
    return 0


# ---------------------------------------------------------------------------
# Entry point
# ---------------------------------------------------------------------------
def main() -> int:
    parser = argparse.ArgumentParser(
        description="ASHFALL version drift gate (Plan 48 / C2[21])",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog=textwrap.dedent("""\
            Exit 0 = PASS, Exit 1 = FAIL

            Normal usage (fast CI gate):
              python3 scripts/ci/version-gate.py

            Changelog bump-link check:
              python3 scripts/ci/version-gate.py --base-ref origin/main

            Hotfix branch gate (no schema constant changes):
              python3 scripts/ci/version-gate.py --hotfix --base-ref v1.1.0

            Self-test (proves failure-detection works):
              python3 scripts/ci/version-gate.py --self-test
        """),
    )
    parser.add_argument("--self-test", action="store_true", help="Run self-test and exit")
    parser.add_argument("--hotfix", action="store_true", help="Hotfix mode: refuse save-schema constant changes")
    parser.add_argument("--base-ref", dest="base_ref", default=None, help="Git ref for changelog/schema delta check")
    parser.add_argument("--repo-root", dest="repo_root", default=None, help="Override repo root path")
    args = parser.parse_args()

    if args.self_test:
        return run_self_test()

    return run_gate(args)


if __name__ == "__main__":
    sys.exit(main())
