#!/usr/bin/env python3
"""
add_p11_methods_v2.py

STATUS:  HISTORICAL / COMPLETED (Phase 1-1 migration artifact)
OWNER:   Godot Host Sessions / Save Infrastructure
PURPOSE: Batch-added P1-1 dirty tracking and Save methods to all 28+ HostSession
         files, handling per-store save store signatures and DTO types.
NOTE:    Retained for reference on host session save mappings.
"""
import re
from pathlib import Path

REPO_ROOT = Path(__file__).resolve().parent.parent.parent
HOST_DIR = REPO_ROOT / "src" / "Host"

SPECIAL_SAVE_STORES = {
    "ChemicalDependency": ("ChemicalDependencySaveStore", "TrySave", "ChemicalDependencyLedgerState", "System.CaptureState()"),
    "Journal": ("JournalSaveStore", "TrySave", "JournalSave", "System.CaptureState()"),
    "MedicalWard": ("MedicalWardSaveStore", "TrySave", "MedicalWardSave", "System.CaptureState()"),
    "Weather": ("WeatherSaveStore", "TrySave", "WorldWeatherState", "System.CaptureState()"),
    "ShelterAssignment": (None, None, None, None),
    "Inventory": ("InventorySaveStore", "TrySave", "InventorySaveState", "System.CaptureState()"),
    "DoseLedger": ("DoseLedgerSaveStore", "TrySave", "DoseLedgerSave", "System.CaptureState()"),
    "DutyRoster": ("DutyRosterSaveStore", "TrySave", "DutyRosterSave", "System.CaptureState()"),
    "ExpansionHub": ("ExpansionHubSaveStore", "TrySave", "ExpansionHubSave", "System.CaptureState()"),
    "Holdfast": ("HoldfastSaveStore", "TrySave", "HoldfastSave", "System.CaptureState()"),
    "Radio": ("RadioSaveStore", "TrySave", "RadioSaveState", "System.CaptureState()"),
    "HoldfastTrade": ("HoldfastTradeSaveStore", "TrySave", "HoldfastTradeSaveState", "System.CaptureState()"),
    "Maritime": ("MaritimeSaveStore", "TrySave", "MaritimeHostSave", "System.CaptureState()"),
    "Memorial": ("MemorialSaveStore", "TrySave", "MemorialSave", "System.CaptureState()"),
    "Muster": ("MusterSaveStore", "TrySave", "MusterHostSave", "System.CaptureState()"),
    "Narrative": ("NarrativeSaveStore", "TrySave", "NarrativeEncounterState", "System.CaptureState()"),
    "PowerGrid": ("PowerGridSaveStore", "TrySave", "PowerGridSave", "System.CaptureState()"),
    "PhantomMemory": ("PhantomMemorySaveStore", "TrySave", "PhantomMemoryEngineState", "System.CaptureState()"),
    "Phase0": ("Phase0SaveStore", "TrySave", "Phase0EffectsSaveState", "System.CaptureState()"),
    "SilentFoundry": ("SilentFoundrySaveStore", "TrySave", "SilentFoundrySave", "System.CaptureState()"),
    "WastelandMap": ("WastelandMapSaveStore", "TrySave", "WastelandMapSave", "System.CaptureState()"),
    "World": ("WorldSaveStore", "TrySave", "WorldSave", "System.CaptureState()"),
    "Survivors": ("SurvivorsSaveStore", "TrySave", "SurvivorsSave", "System.CaptureState()"),
    "Research": ("ResearchSaveStore", "TrySave", "ResearchSave", "System.CaptureState()"),
    "StartingLevel": ("StartingLevelSaveStore", "TrySave", "StartingLevelSave", "System.CaptureState()"),
    "StandingRecord": ("StandingRecordSaveStore", "TrySave", "StandingRecordSave", "System.CaptureState()"),
    "Verdict": ("VerdictSaveStore", "TrySave", "VerdictSave", "System.CaptureState()"),
    "Caravan": ("CaravanSaveStore", "TrySave", "TravelingCaravanState", "System.CaptureState()"),
    "DailyBriefing": ("DailyBriefingSaveStore", "TrySave", "DailyBriefingSave", "System.CaptureState()"),
    "Disease": ("DiseaseSaveStore", "TrySave", "DiseaseSystemState", "System.CaptureState()"),
    "Ecomomy": ("EconomySaveStore", "TrySave", "MarketState", "System.CaptureState()"),
}

# These classes don't have LastEvent but DO have public properties we can anchor on
NO_LAST_EVENT_CLASSES = {"DoseLedgerHostSession", "ExpansionHostSession", "PowerGridHostSession", "ShelterAssignmentHostSession"}

def get_save_info(class_name: str):
    system_name = class_name.replace("HostSession", "")
    if system_name in SPECIAL_SAVE_STORES:
        return SPECIAL_SAVE_STORES[system_name]
    # Standard pattern
    save_store = f"{system_name}SaveStore"
    state_type = f"{system_name}State"
    return save_store, "TrySave", state_type, "System.CaptureState()"

def find_insertion_point(content: str, class_name: str):
    """Find where to insert IsDirty/MarkDirty/Save methods."""
    # Try LastEvent first
    last_event_match = re.search(r'(public string LastEvent \{ get; private set; \} = string\.Empty;)', content)
    if last_event_match:
        return last_event_match.end()

    # Try last public property
    prop_match = re.search(r'(\n    public (sealed )?class \w+\n    \{[^}]*?)(\n    public \w+[^=]*?\{)', content, re.DOTALL)
    if prop_match:
        return prop_match.start(3)

    # Try after opening brace of class
    class_open = re.search(r'public (sealed )?class \w+\n    \{', content)
    if class_open:
        return class_open.end()

    return None

def process_hostsession(filepath: Path):
    content = filepath.read_text()
    original = content

    if "public bool IsDirty" in content:
        return False, "already has IsDirty"

    class_match = re.search(r'public sealed class (\w+HostSession)', content)
    if not class_match:
        return False, "could not find class"

    class_name = class_match.group(1)
    system_name = class_name.replace("HostSession", "")

    insert_pos = find_insertion_point(content, class_name)
    if insert_pos is None:
        return False, "could not find insertion point"

    save_store, method, state_type, state_expr = get_save_info(class_name)

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

    new_content = content[:insert_pos] + methods + content[insert_pos:]

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
