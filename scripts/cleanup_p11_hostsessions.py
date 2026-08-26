#!/usr/bin/env python3
"""Clean up the 4 new P1-1 HostSession files to properly inherit from HostSessionBase."""
import re
from pathlib import Path

FILES = [
    "src/Host/ChemicalDependencyHostSession.cs",
    "src/Host/JournalHostSession.cs",
    "src/Host/MedicalWardHostSession.cs",
    "src/Host/WeatherHostSession.cs",
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
