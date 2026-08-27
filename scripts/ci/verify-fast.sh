#!/usr/bin/env bash
# =============================================================================
# ASHFALL — Local Fast-Tier Verification Runner
# =============================================================================
# Mirrors the canonical CI pipeline in .github/workflows/ci.yml in exact order.
# Stops immediately on the first gate failure (fail-fast, set -euo pipefail).
#
# Usage:
#   bash scripts/ci/verify-fast.sh
#   ./scripts/ci/verify-fast.sh
#
# Gates executed in order:
#   1. StreamingAssets JSON syntax validation (fast-fail)
#   2. Build Core & Tests (net9.0)
#   3. Execute Core unit test suite (xUnit net9.0)
#   4. Build Godot host (Ashfall.csproj net8.0, 0 errors)
#   5. Data integrity gate (--data-integrity-selftest, 129 catalogs, 0 errors)
#   6. Bridge removal gate (--bridge-selftest)
#   7. Asset registry resolution gate (--asset-registry-selftest)
#   8. Player UI panels lifecycle test (--player-panels-uitest)
#   9. Save stores & failure paths (save-load-ui-failure, holdfast, inventory, journal)
#  10. Campaign smoke (playable shell & day 1 onboarding)
#  11. Expansions completeness gate (--expansions-selftest, 01-10)
#  12. Triad drift gate (scripts/ci/triad-drift-gate.sh)
#  13. CLI catalog drift gate (scripts/ci/generate-cli-catalog.sh --check)
#  14. Compiler warning baseline gate (scripts/ci/warning-baseline-gate.sh)
# =============================================================================

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "${REPO_ROOT}"

START_TIME=$(date +%s)

echo "============================================================================="
echo "  ASHFALL FAST-TIER VERIFICATION RUNNER (Mirroring CI Gates)"
echo "============================================================================="
echo "Started at: $(date -u +"%Y-%m-%dT%H:%M:%SZ")"
echo "Root: ${REPO_ROOT}"
echo "-----------------------------------------------------------------------------"

trap 'echo -e "\n❌ [ABORT] Fast verification failed at gate $? (stopping on first failure)."; exit 1' ERR

# 1. Validate StreamingAssets JSON syntax (fast-fail)
echo -e "\n[1/14] Validating StreamingAssets JSON syntax..."
python3 - <<'PY'
import json
import pathlib
import sys

root = pathlib.Path("Assets/StreamingAssets/Data")
if not root.is_dir():
    print(f"ERROR: authoritative data directory is missing: {root}")
    sys.exit(1)

files = sorted(root.rglob("*.json"))
if not files:
    print(f"ERROR: no JSON catalogs found under {root}")
    sys.exit(1)

errors = []
for path in files:
    try:
        json.loads(path.read_text(encoding="utf-8"))
    except Exception as exc:
        errors.append(f"{path}: {exc}")

if errors:
    print("JSON validation FAILED:")
    print("\n".join(errors))
    sys.exit(1)

print(f"  -> PASS: validated {len(files)} JSON catalogs under {root}")
PY

# 2. Build Core & Tests
echo -e "\n[2/14] Building Ashfall.Core.Tests..."
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --nologo

# 3. Run Core Test Suite
echo -e "\n[3/14] Running Core unit test suite..."
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --nologo

# 4. Build Godot Host
echo -e "\n[4/14] Building Godot host (Ashfall.csproj)..."
dotnet build Ashfall.csproj --nologo

# 5. Data Integrity Gate
echo -e "\n[5/14] Running data integrity gate..."
godot --headless --path . -- --data-integrity-selftest

# 6. Bridge Removal Gate
echo -e "\n[6/14] Running bridge removal confirmation..."
godot --headless --path . -- --bridge-selftest

# 7. Asset Registry Resolution Gate
echo -e "\n[7/14] Running asset registry resolution gate..."
godot --headless --path . -- --asset-registry-selftest

# 8. Player UI Panels Test
echo -e "\n[8/14] Running player panels UI lifecycle test..."
godot --headless --path . -- --player-panels-uitest

# 9. Save Stores & Failure Paths
echo -e "\n[9/14] Running save stores & failure path self-tests..."
godot --headless --path . -- --save-load-ui-failure-selftest
godot --headless --path . -- --holdfast-save-selftest
godot --headless --path . -- --inventory-save-selftest
godot --headless --path . -- --journal-save-selftest

# 10. Campaign Smoke Tests
echo -e "\n[10/14] Running campaign smoke self-tests..."
godot --headless --path . -- --playable-shell-selftest
godot --headless --path . -- --day1-selftest

# 11. Expansions Completeness Gate
echo -e "\n[11/14] Running expansions completeness gate (01–10)..."
godot --headless --path . -- --expansions-selftest

# 12. Triad Drift Gate
echo -e "\n[12/14] Running triad drift gate..."
bash scripts/ci/triad-drift-gate.sh

# 13. CLI Catalog Drift Gate
echo -e "\n[13/14] Running CLI command catalog drift gate..."
bash scripts/ci/generate-cli-catalog.sh --check

# 14. Compiler Warning Baseline Gate
echo -e "\n[14/14] Running compiler warning baseline gate..."
bash scripts/ci/warning-baseline-gate.sh

END_TIME=$(date +%s)
ELAPSED=$((END_TIME - START_TIME))

echo -e "\n============================================================================="
echo "  ✅ ALL 14 FAST-TIER VERIFICATION GATES PASSED (${ELAPSED}s)"
echo "============================================================================="
exit 0
