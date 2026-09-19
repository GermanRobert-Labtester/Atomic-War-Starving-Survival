#!/usr/bin/env bash
# =============================================================================
# input-map-gate.sh — Input Map Contract & Action Parity Gate (Plan 37 / C2[15])
# =============================================================================
# Validates:
#  1. Every action declared in project.godot [input] has a contract row in
#     AshfallInputActions.Contract.
#  2. Every action in AshfallInputActions.Contract is declared in project.godot.
#  3. Every Is* predicate in AshfallInputActions.cs has >=1 call site in src/.
#  4. CanonicalDefaults has no within-map collisions.
#  5. IsConfirmOrAccept alias is removed.
#
# Prevents action-map drift, orphan handlers, and false affordances.
# =============================================================================

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "${REPO_ROOT}"

echo "── Input Map Contract & Action Parity Gate ──"

python3 - <<'PY'
import os
import re
import sys
import glob

repo_root = os.getcwd()
godot_path = os.path.join(repo_root, "project.godot")
actions_path = os.path.join(repo_root, "src", "Host", "AshfallInputActions.cs")

if not os.path.isfile(godot_path):
    print(f"ERROR: {godot_path} not found!", file=sys.stderr)
    sys.exit(1)
if not os.path.isfile(actions_path):
    print(f"ERROR: {actions_path} not found!", file=sys.stderr)
    sys.exit(1)

with open(godot_path, "r", encoding="utf-8") as f:
    godot_content = f.read()

with open(actions_path, "r", encoding="utf-8") as f:
    actions_content = f.read()

# 1. Parse actions from project.godot
input_sec_match = re.search(r'\[input\](.*?)(?=\n\[|\Z)', godot_content, re.DOTALL)
if not input_sec_match:
    print("ERROR: [input] section not found in project.godot", file=sys.stderr)
    sys.exit(1)

godot_actions = set(re.findall(r'(?m)^(ashfall_[a-z0-9_]+)=', input_sec_match.group(1)))

# 2. Parse constants from AshfallInputActions.cs
constants = {}
for m in re.finditer(r'public const string\s+([A-Za-z0-9_]+)\s*=\s*"([^"]+)";', actions_content):
    constants[m.group(1)] = m.group(2)

# 3. Parse Contract entries
contract_match = re.search(r'public static readonly IReadOnlyList<InputActionContract> Contract\s*=\s*new\[\]\s*\{(.*?)\};', actions_content, re.DOTALL)
if not contract_match:
    print("ERROR: Contract array not found in AshfallInputActions.cs", file=sys.stderr)
    sys.exit(1)

contract_actions = set()
for m in re.finditer(r'new\s+InputActionContract\s*\(\s*([A-Za-z0-9_]+)', contract_match.group(1)):
    ident = m.group(1)
    act_name = constants.get(ident, ident.strip('"'))
    contract_actions.add(act_name)

# Verify bidirectional parity
missing_in_contract = godot_actions - contract_actions
if missing_in_contract:
    print(f"ERROR: project.godot actions missing in Contract: {sorted(missing_in_contract)}", file=sys.stderr)
    sys.exit(1)

missing_in_godot = contract_actions - godot_actions
if missing_in_godot:
    print(f"ERROR: Contract actions missing in project.godot: {sorted(missing_in_godot)}", file=sys.stderr)
    sys.exit(1)

# 4. Parse predicates and verify call sites in src/
predicates = re.findall(r'public static bool (Is[A-Za-z0-9_]+)\s*\(', actions_content)
if not predicates:
    print("ERROR: No Is* predicates found in AshfallInputActions.cs", file=sys.stderr)
    sys.exit(1)

src_files = [
    f for f in glob.glob(os.path.join(repo_root, "src", "**", "*.cs"), recursive=True)
    if os.path.basename(f) != "AshfallInputActions.cs"
]
combined_src = ""
for f in src_files:
    with open(f, "r", encoding="utf-8") as sfile:
        combined_src += sfile.read() + "\n"

orphan_predicates = []
for pred in predicates:
    if not re.search(r'\b' + pred + r'\b', combined_src):
        orphan_predicates.append(pred)

if orphan_predicates:
    print(f"ERROR: Predicates in AshfallInputActions.cs with 0 call sites in src/: {orphan_predicates}", file=sys.stderr)
    sys.exit(1)

# 5. Check CanonicalDefaults for collisions
defaults_match = re.search(r'public static readonly IReadOnlyDictionary<string, Key> CanonicalDefaults\s*=\s*new Dictionary<string, Key>\s*\{(.*?)\};', actions_content, re.DOTALL)
if not defaults_match:
    print("ERROR: CanonicalDefaults dictionary not found in AshfallInputActions.cs", file=sys.stderr)
    sys.exit(1)

seen_keys = {}
collisions = []
for m in re.finditer(r'\{\s*([A-Za-z0-9_]+)\s*,\s*Key\.([A-Za-z0-9_]+)\s*\}', defaults_match.group(1)):
    act = m.group(1)
    key = m.group(2)
    if key in seen_keys:
        collisions.append(f"Key.{key} shared by '{seen_keys[key]}' and '{act}'")
    else:
        seen_keys[key] = act

if collisions:
    print(f"ERROR: Within-map collisions in CanonicalDefaults: {collisions}", file=sys.stderr)
    sys.exit(1)

# 6. Verify IsConfirmOrAccept is absent
if "IsConfirmOrAccept" in actions_content:
    print("ERROR: Deprecated alias IsConfirmOrAccept is still present in AshfallInputActions.cs", file=sys.stderr)
    sys.exit(1)

print(f"Verified {len(godot_actions)} canonical actions across project.godot and AshfallInputActions.")
print(f"Verified {len(predicates)} input predicates with active call sites in src/.")
print("input map contract PASS")
PY
