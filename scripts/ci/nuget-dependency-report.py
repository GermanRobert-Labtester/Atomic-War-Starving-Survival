#!/usr/bin/env python3
"""
nuget-dependency-report.py — Central Package Management (CPM) Validator & Dependency Reporter

Validates that:
  1. Directory.Packages.props exists and ManagePackageVersionsCentrally is true.
  2. Zero .csproj files specify inline Version="..." on <PackageReference>.
  3. Every <PackageReference> is mapped to a declared <PackageVersion>.
  4. Generates a clear dependency inventory and status report.

Usage:
  python3 scripts/ci/nuget-dependency-report.py --check   # CI validation gate
  python3 scripts/ci/nuget-dependency-report.py --report  # Print dependency inventory table
"""

import os
import re
import sys
import pathlib
import xml.etree.ElementTree as ET

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
PROPS_FILE = REPO_ROOT / "Directory.Packages.props"

def load_cpm_packages():
    if not PROPS_FILE.exists():
        return None, {}

    try:
        tree = ET.parse(PROPS_FILE)
        root = tree.getroot()
    except Exception as e:
        print(f"Error parsing {PROPS_FILE}: {e}", file=sys.stderr)
        return None, {}

    # Check ManagePackageVersionsCentrally
    manage_cpm = False
    for elem in root.iter():
        if elem.tag == "ManagePackageVersionsCentrally" and elem.text and elem.text.strip().lower() == "true":
            manage_cpm = True

    # Find PackageVersion items
    versions = {}
    for elem in root.iter():
        if elem.tag == "PackageVersion":
            pkg = elem.attrib.get("Include")
            ver = elem.attrib.get("Version")
            if pkg and ver:
                versions[pkg] = ver

    return manage_cpm, versions

def scan_csproj_files():
    csproj_files = list(REPO_ROOT.rglob("*.csproj"))
    results = []

    for csproj in sorted(csproj_files):
        if "/obj/" in csproj.as_posix() or "/bin/" in csproj.as_posix():
            continue

        try:
            tree = ET.parse(csproj)
            root = tree.getroot()
        except Exception as e:
            print(f"Error parsing {csproj}: {e}", file=sys.stderr)
            continue

        tf = "unknown"
        for elem in root.iter():
            if elem.tag == "TargetFramework":
                tf = elem.text.strip() if elem.text else "unknown"

        pkg_refs = []
        inline_versions = []

        for elem in root.iter():
            if elem.tag == "PackageReference":
                pkg = elem.attrib.get("Include")
                ver = elem.attrib.get("Version")
                if pkg:
                    pkg_refs.append(pkg)
                if ver:
                    inline_versions.append((pkg, ver))

        results.append({
            "path": csproj.relative_to(REPO_ROOT).as_posix(),
            "target_framework": tf,
            "packages": pkg_refs,
            "inline_versions": inline_versions
        })

    return results

def main():
    check_mode = "--check" in sys.argv
    report_mode = "--report" in sys.argv or not check_mode

    cpm_enabled, cpm_versions = load_cpm_packages()

    if not PROPS_FILE.exists() or not cpm_enabled:
        print(f"FAILED: Central Package Management not enabled in {PROPS_FILE.name}")
        return 1

    projects = scan_csproj_files()
    errors = []

    # Map package -> consumers
    consumers = {pkg: [] for pkg in cpm_versions}
    for proj in projects:
        if proj["inline_versions"]:
            for pkg, ver in proj["inline_versions"]:
                errors.append(f"Inline version '{ver}' found on PackageReference '{pkg}' in {proj['path']} (must be centralized).")

        for pkg in proj["packages"]:
            if pkg not in cpm_versions:
                errors.append(f"PackageReference '{pkg}' in {proj['path']} has no matching <PackageVersion> in Directory.Packages.props.")
            else:
                consumers[pkg].append(proj["path"])

    if report_mode or check_mode:
        print("=================================================================================")
        print("  ASHFALL — CENTRAL PACKAGE MANAGEMENT & DEPENDENCY REPORT")
        print("=================================================================================")
        print(f"CPM Configuration: Directory.Packages.props (Active, {len(cpm_versions)} packages defined)")
        print(f"Scanned Projects:  {len(projects)} .csproj files")
        print("---------------------------------------------------------------------------------")
        print(f"{'Package Name':<30} {'Version':<12} {'Consumers':<40}")
        print("---------------------------------------------------------------------------------")
        for pkg, ver in sorted(cpm_versions.items()):
            cons = ", ".join(consumers.get(pkg, [])) or "(unused)"
            print(f"{pkg:<30} {ver:<12} {cons:<40}")
        print("=================================================================================")

    if errors:
        print("\n❌ CPM POLICY VIOLATIONS DETECTED:")
        for err in errors:
            print(f"  - {err}")
        return 1

    print(f"\n✅ Central Package Management Gate Passed: {len(cpm_versions)} package(s) centralized across {len(projects)} project(s).")
    return 0

if __name__ == "__main__":
    sys.exit(main())
