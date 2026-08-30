#!/usr/bin/env bash
# =============================================================================
# triad-drift-gate.sh — Declarative Save-Section Triad Gate
# =============================================================================
# Validates that every save section defined in the declarative authority
# (Assets/Ashfall.Core/Save/SaveSectionRegistry.cs) has matching SaveXxx and
# SetupXxx methods in src/Main.*.cs, and that no unregistered Save methods exist.
#
# Prevents AGENTS.md Invariant / H7 triad drift (Setup without Save or Save
# without registered section).
#
# Documentation: docs/architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md
# =============================================================================

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "${REPO_ROOT}"

echo "── Declarative Triad Drift Gate ──"

python3 - <<'PY'
import re
import glob
import sys
import pathlib

registry_path = pathlib.Path("Assets/Ashfall.Core/Save/SaveSectionRegistry.cs")
if not registry_path.is_file():
    print(f"ERROR: {registry_path} not found!", file=sys.stderr)
    sys.exit(1)

# 1. Parse declarative SaveSectionRegistry
registry_content = registry_path.read_text(encoding="utf-8")
pattern = re.compile(
    r'new\s*\(\s*"([^"]+)"\s*,\s*"([^"]+)"\s*,\s*("[^"]+"|\bnull\b)\s*,\s*"([^"]+)"\s*,\s*"([^"]+)"'
    r'(?:\s*,\s*RequiresSetup:\s*(true|false))?'
    r'(?:\s*,\s*LifecycleGroup:\s*[A-Za-z_][A-Za-z0-9_]*)?'
    r'\s*\)'
)

entries = []
for m in pattern.finditer(registry_content):
    sec_key = m.group(1)
    save_method = m.group(2)
    setup_raw = m.group(3)
    setup_method = setup_raw.strip('"') if setup_raw != "null" else None
    owner = m.group(4)
    desc = m.group(5)
    req_setup_str = m.group(6)
    requires_setup = (req_setup_str != "false") if req_setup_str else (setup_method is not None)

    entries.append({
        "key": sec_key,
        "save": save_method,
        "setup": setup_method,
        "owner": owner,
        "desc": desc,
        "requires_setup": requires_setup
    })

if not entries:
    print("ERROR: Failed to parse any save section definitions from SaveSectionRegistry.cs", file=sys.stderr)
    sys.exit(1)

# 2. Extract methods from src/Main*.cs
main_files = glob.glob("src/Main*.cs")
all_main_code = "\n".join(pathlib.Path(f).read_text(encoding="utf-8") for f in main_files)

found_saves = set(re.findall(r"private void (Save[A-Za-z0-9_]+)\(", all_main_code))
found_saves.discard("SaveAll")
found_saves.discard("SaveAllExpandedShelterSystems")

found_setups = set(re.findall(r"private void (Setup[A-Za-z0-9_]+)\(", all_main_code))

registered_saves = {e["save"] for e in entries}
registered_setups = {e["setup"] for e in entries if e["setup"]}

errors = []

print(f"Registered Save Sections: {len(entries)}")
print(f"Found in host: {len(found_saves)} Save methods, {len(found_setups)} Setup methods")
print()

# Check every registered section has its Save method and Setup method (if required)
for e in entries:
    sec_key = e["key"]
    save_m = e["save"]
    setup_m = e["setup"]
    req_setup = e["requires_setup"]

    if save_m not in found_saves:
        errors.append(f"[FAIL] Registered section '{sec_key}' expects method '{save_m}()' in src/Main*.cs but it was not found.")

    if req_setup:
        if not setup_m:
            errors.append(f"[FAIL] Registered section '{sec_key}' requires setup but has null SetupMethod.")
        elif setup_m not in found_setups:
            errors.append(f"[FAIL] Registered section '{sec_key}' expects setup method '{setup_m}()' in src/Main*.cs but it was not found.")
    elif setup_m is None:
        print(f"[OK]   Section '{sec_key}' ({save_m}) — setup exemption documented ({e['owner']}: {e['desc']})")

# Check if there are any Save methods in Main*.cs not declared in the registry
for save_m in sorted(found_saves):
    if save_m not in registered_saves:
        # Check special alias for MoralChoice / EventAdapter
        if save_m == "SaveMoralChoice" and "SaveEventAdapter" in registered_saves:
            continue
        errors.append(f"[FAIL] Host defines '{save_m}()' in src/Main*.cs which is not declared in SaveSectionRegistry.cs.")

if errors:
    print("\nTRIAD DRIFT ERRORS DETECTED:")
    for err in errors:
        print(err, file=sys.stderr)
    sys.exit(1)

print("\nGATE PASS: triad drift — SaveSectionRegistry matches host Setup and Save implementations cleanly.")
PY

exit 0
