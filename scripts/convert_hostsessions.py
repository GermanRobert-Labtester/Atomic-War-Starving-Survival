#!/usr/bin/env python3
"""Convert all HostSession classes to inherit from HostSessionBase."""
import re
from pathlib import Path

HOST_DIR = Path("src/Host")
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
