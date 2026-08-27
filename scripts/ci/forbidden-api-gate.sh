#!/usr/bin/env bash
# =============================================================================
# ASHFALL — Forbidden Core APIs Source Gate
# =============================================================================
# Mechanically scans Assets/Ashfall.Core/ to ensure zero forbidden APIs:
#   1. Zero engine namespaces (UnityEngine, UnityEditor, Godot, GodotSharp)
#   2. Zero nondeterministic RNG (System.Random, new Random(), UnityEngine.Random)
#   3. Zero Guid.NewGuid()
#   4. Zero serializer bypasses (JsonUtility, Newtonsoft.Json, BinaryFormatter)
#   5. Zero wall-clock drift (DateTime.Now, DateTime.UtcNow)
#   6. Zero Thread.Sleep
# =============================================================================

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
CORE_DIR="${REPO_ROOT}/Assets/Ashfall.Core"

echo "── Running Forbidden Core API Source Gate ──"
echo "Target: ${CORE_DIR}"

violations=0

check_pattern() {
    local label="$1"
    local pattern="$2"

    # Search non-comment lines
    local matches
    matches=$(grep -rnE "$pattern" "$CORE_DIR" --include="*.cs" || true)

    # Filter out pure comment lines (leading whitespace followed by // or *)
    local real_matches=""
    if [ -n "$matches" ]; then
        while IFS= read -r line; do
            local code_part
            code_part=$(echo "$line" | cut -d: -f3- | sed 's/^[[:space:]]*//')
            if [[ ! "$code_part" =~ ^// ]] && [[ ! "$code_part" =~ ^\* ]] && [[ ! "$code_part" =~ ^/\* ]]; then
                real_matches+="$line"$'\n'
            fi
        done <<< "$matches"
    fi

    if [ -n "$real_matches" ]; then
        echo -e "\n❌ [FAIL] ${label}:"
        echo "$real_matches"
        violations=$((violations + 1))
    else
        echo "[OK] ${label}"
    fi
}

check_pattern "Zero Engine Namespaces" "using[[:space:]]+(UnityEngine|UnityEditor|Godot|GodotSharp)"
check_pattern "Zero System.Random / Nondeterministic RNG" "(new[[:space:]]+System\.Random|new[[:space:]]+Random\(|System\.Random|UnityEngine\.Random)"
check_pattern "Zero Guid.NewGuid()" "Guid\.NewGuid\(\)"
check_pattern "Zero Legacy Serializer Bypasses" "(JsonUtility|Newtonsoft|BinaryFormatter)"
check_pattern "Zero Wall-Clock Simulation Drift" "(DateTime\.Now|DateTime\.UtcNow)"
check_pattern "Zero Thread.Sleep" "Thread\.Sleep\("

if [ "$violations" -gt 0 ]; then
    echo -e "\n❌ Forbidden Core API Gate FAILED ($violations violation categories detected)."
    exit 1
fi

echo -e "\n✅ Forbidden Core API Gate PASSED (0 violations)."
exit 0
