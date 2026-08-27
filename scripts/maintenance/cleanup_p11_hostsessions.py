#!/usr/bin/env python3
"""
cleanup_p11_hostsessions.py

STATUS:  HISTORICAL / COMPLETED (Phase 1-1 migration artifact)
OWNER:   Godot Host Sessions / Architecture
PURPOSE: Cleaned up 4 newly created Phase 1-1 HostSession files to inherit
         from HostSessionBase and removed duplicate StateChanged events.
"""
import re
from pathlib import Path

REPO_ROOT = Path(__file__).resolve().parent.parent.parent
FILES = [
    REPO_ROOT / "src" / "Host" / "ChemicalDependencyHostSession.cs",
    REPO_ROOT / "src" / "Host" / "JournalHostSession.cs",
    REPO_ROOT / "src" / "Host" / "MedicalWardHostSession.cs",
    REPO_ROOT / "src" / "Host" / "WeatherHostSession.cs",
]

def process_file(filepath: Path):
    content = filepath.read_text()

    # Change sealed to inherit base (already done by previous script, but ensure)
    content = re.sub(
        r'public sealed class (\w+HostSession)',
        r'public class \1 : HostSessionBase',
        content
    )

    # Remove 'public event Action? StateChanged;' (in base class)
    content = re.sub(r'\s*public event Action\? StateChanged;\n', '', content)

    # Remove 'public bool IsDirty { get; private set; }' (in base class)
    content = re.sub(r'\s*public bool IsDirty \{ get; private set; \};\n', '', content)

    # Remove 'public void MarkDirty() {...}' blocks (in base class)
    content = re.sub(
        r'\n\s*public void MarkDirty\(\)\n\s*\{\n\s*IsDirty = true;\n\s*StateChanged\?\.Invoke\(\);\n\s*\}\n',
        '\n',
        content
    )

    # Change 'public void Save()' to 'public override void Save()'
    content = re.sub(
        r'(\n\s*)(public void Save\(\)\n)',
        r'\1public override void Save()\n',
        content
    )

    filepath.write_text(content)
    return True

def main():
    for f in FILES:
        process_file(Path(f))
        print(f"[CLEANED] {f}")

if __name__ == "__main__":
    main()
