#!/usr/bin/env python3
"""
add_p11_methods.py

STATUS:  HISTORICAL / COMPLETED (Phase 1-1 migration artifact)
OWNER:   Godot Host Sessions / Save Infrastructure
PURPOSE: Added P1-1 methods (IsDirty, MarkDirty, Save) to legacy HostSession
         files that previously lacked dirty tracking and save delegation.
NOTE:    Superseded by add_p11_methods_v2.py and recent generic SaveStore<T>
         infrastructure. Retained for historical provenance.
"""
import re
from pathlib import Path

REPO_ROOT = Path(__file__).resolve().parent.parent.parent
HOST_DIR = REPO_ROOT / "src" / "Host"

def get_save_store_info(hostsession_name: str):
    """Map HostSession name to save store file and method signature."""
    # Remove 'HostSession' suffix
    system_name = hostsession_name.replace("HostSession", "")

    # Special cases
    special_cases = {
        "ChemicalDependency": ("ChemicalDependencySaveStore", "TrySave", "ChemicalDependencyLedgerState", "System.CaptureState()"),
        "Journal": ("JournalSaveStore", "Save", "JournalSave", "System.CaptureState()"),
        "MedicalWard": ("MedicalWardSaveStore", "TrySave", "MedicalWardSave", "System.CaptureState()"),
        "Weather": ("WeatherSaveStore", "TrySave", "WorldWeatherState", "System.CaptureState()"),
        "ShelterAssignment": (None, None, None, None),
    }

    if system_name in special_cases:
        return special_cases[system_name]

    # Standard pattern: XxxSaveStore.TrySave(XxxState state)
    save_store = f"{system_name}SaveStore"
    state_type = f"{system_name}State"
    return save_store, "TrySave", state_type, "System.CaptureState()"

def process_hostsession(filepath: Path):
    content = filepath.read_text()
    original = content

    # Skip if already has IsDirty
    if "public bool IsDirty" in content:
        return False, "already has IsDirty"

    # Find class name
    class_match = re.search(r'public sealed class (\w+HostSession)', content)
    if not class_match:
        return False, "could not find class"

    class_name = class_match.group(1)
    system_name = class_name.replace("HostSession", "")

    # Find insertion point (after LastEvent property)
    last_event_match = re.search(r'(public string LastEvent \{ get; private set; \} = string\.Empty;)', content)
    if not last_event_match:
        return False, "could not find LastEvent"

    # Get save store info
    save_store, method, state_type, state_expr = get_save_store_info(system_name)

    # Generate methods
    methods = "\n\n"
    methods += "        public bool IsDirty { get; private set; }\n\n"
    methods += "        public void MarkDirty()\n"
    methods += "        {\n"
    methods += "            IsDirty = true;\n"
    methods += "            StateChanged?.Invoke();\n"
    methods += "        }\n\n"
    methods += "        public void Save()\n"
    methods += "        {\n"
    methods += "            if (!IsDirty) return;\n"
    methods += "            try\n"
    methods += "            {\n"

    if save_store and method == "TrySave":
        methods += f"                if ({save_store}.{method}({state_expr}))\n"
        methods += "                    IsDirty = false;\n"
    elif save_store and method == "Save":
        methods += f"                {save_store}.{method}({state_expr});\n"
        methods += "                IsDirty = false;\n"
    else:
        methods += "                // TODO: implement save for " + class_name + "\n"
        methods += "                IsDirty = false;\n"

    methods += "            }\n"
    methods += "            catch (Exception e)\n"
    methods += "            {\n"
    methods += '                GD.PrintErr("[' + system_name + '] save failed: " + e.Message);\n'
    methods += "            }\n"
    methods += "        }\n"

    # Insert methods after LastEvent
    new_content = content[:last_event_match.end()] + methods + content[last_event_match.end():]

    if new_content != original:
        filepath.write_text(new_content)
        return True, f"added IsDirty/MarkDirty/Save"

    return False, "no changes made"

def main():
    changed = 0
    errors = 0

    for f in sorted(HOST_DIR.glob("*HostSession.cs")):
        if f.name in ["HostCli.cs", "HostCli.SelfTests.cs", "HostCli.PanelTests.cs"]:
            continue
        try:
            ok, msg = process_hostsession(f)
            if ok:
                changed += 1
                print(f"[CHANGED] {f.name}: {msg}")
            else:
                print(f"[SKIP] {f.name}: {msg}")
        except Exception as e:
            errors += 1
            print(f"[ERROR] {f.name}: {e}")

    print(f"\nSummary: {changed} changed, {errors} errors")

if __name__ == "__main__":
    main()
