#!/usr/bin/env python3
"""
convert_hostsessions.py

STATUS:  HISTORICAL / COMPLETED (Phase 1-1 migration artifact)
OWNER:   Godot Host Sessions / Architecture
PURPOSE: Batch-migrated sealed HostSession classes across src/Host/ to inherit
         from HostSessionBase.
"""
import re
from pathlib import Path

REPO_ROOT = Path(__file__).resolve().parent.parent.parent
HOST_DIR = REPO_ROOT / "src" / "Host"
SKIP_FILES = {"HostCli.cs", "HostCli.SelfTests.cs", "HostCli.PanelTests.cs", "HostSessionBase.cs", "Phase0HostSession.cs"}

def process_file(filepath: Path):
    content = filepath.read_text()
    original = content

    # Skip if already inherits from HostSessionBase
    if "HostSessionBase" in content:
        return False, "already inherits HostSessionBase"

    # Change sealed class to inherit from HostSessionBase
    content = re.sub(
        r'public sealed class (\w+HostSession)',
        r'public class \1 : HostSessionBase',
        content
    )

    if content == original:
        return False, "no sealed class found"

    filepath.write_text(content)
    return True, "converted to inherit HostSessionBase"

def main():
    changed = 0
    for f in sorted(HOST_DIR.glob("*HostSession.cs")):
        if f.name in SKIP_FILES:
            continue
        ok, msg = process_file(f)
        if ok:
            changed += 1
            print(f"[CHANGED] {f.name}: {msg}")

    print(f"\nSummary: {changed} files converted")

if __name__ == "__main__":
    main()
