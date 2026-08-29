#!/usr/bin/env bash
# =============================================================================
# ASHFALL CI — Compiler Warning Baseline Gate
# =============================================================================
# Enforces that dotnet builds of the Core library, Godot host, and Test suite
# emit 0 errors and do not exceed the established warning baseline.
#
# Baseline:
#   - Ashfall.Core.Tests: 0 warnings
#   - Ashfall.csproj (Godot host): 0 warnings under standard build settings
#
# Determinism: every build below uses -t:Rebuild. Plain `dotnet build` lets
# MSBuild skip compilation when a project is up to date, in which case the
# compiler emits no warnings at all and this gate passed regardless of the
# source. Same source + same toolchain must give the same verdict no matter
# what was built before.
#
# Usage:
#   bash scripts/ci/warning-baseline-gate.sh
#   Exit code: 0 = PASS, 1 = FAIL (new warnings detected)
# =============================================================================

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "${REPO_ROOT}"

echo "── RUNNING COMPILER WARNING BASELINE GATE ──"

TMP_CORE_LOG="/tmp/ashfall_warn_core.log"
TMP_HOST_LOG="/tmp/ashfall_warn_host.log"
TMP_TEST_LOG="/tmp/ashfall_warn_test.log"

FAILED=0

# 1. Build Ashfall.Core.Tests (net9.0)
echo "[1/3] Building Ashfall.Core.Tests..."
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --nologo -t:Rebuild > "${TMP_TEST_LOG}" 2>&1 || {
    echo "ERROR: Ashfall.Core.Tests failed to build!"
    cat "${TMP_TEST_LOG}"
    exit 1
}

TEST_WARNS=$(grep -o "warning CS[0-9]*:.*" "${TMP_TEST_LOG}" | sort -u | wc -l || true)
if [ "${TEST_WARNS}" -gt 0 ]; then
    echo "WARNING GATE FAILED: ${TEST_WARNS} warning(s) in Ashfall.Core.Tests:"
    grep "warning CS" "${TMP_TEST_LOG}" | sort -u
    FAILED=1
else
    echo "  -> Ashfall.Core.Tests: 0 warnings (PASS)"
fi

# 2. Build Ashfall.Core (net8.0)
echo "[2/3] Building Ashfall.Core..."
dotnet build Ashfall.Core/Ashfall.Core.csproj --nologo -t:Rebuild > "${TMP_CORE_LOG}" 2>&1 || {
    echo "ERROR: Ashfall.Core failed to build!"
    cat "${TMP_CORE_LOG}"
    exit 1
}

CORE_WARNS=$(grep -o "warning CS[0-9]*:.*" "${TMP_CORE_LOG}" | sort -u | wc -l || true)
if [ "${CORE_WARNS}" -gt 0 ]; then
    echo "WARNING GATE FAILED: ${CORE_WARNS} warning(s) in Ashfall.Core:"
    grep "warning CS" "${TMP_CORE_LOG}" | sort -u
    FAILED=1
else
    echo "  -> Ashfall.Core: 0 warnings (PASS)"
fi

# 3. Build Godot host (Ashfall.csproj, net8.0)
echo "[3/3] Building Ashfall.csproj (Godot host)..."
dotnet build Ashfall.csproj --nologo -t:Rebuild > "${TMP_HOST_LOG}" 2>&1 || {
    echo "ERROR: Ashfall.csproj failed to build!"
    cat "${TMP_HOST_LOG}"
    exit 1
}

HOST_WARNS=$(grep -o "warning CS[0-9]*:.*" "${TMP_HOST_LOG}" | sort -u | wc -l || true)
if [ "${HOST_WARNS}" -gt 0 ]; then
    echo "WARNING GATE FAILED: ${HOST_WARNS} warning(s) in Ashfall.csproj:"
    grep "warning CS" "${TMP_HOST_LOG}" | sort -u
    FAILED=1
else
    echo "  -> Ashfall.csproj: 0 warnings (PASS)"
fi

if [ "${FAILED}" -ne 0 ]; then
    echo "=== COMPILER WARNING BASELINE GATE FAILED ==="
    exit 1
fi

echo "=== COMPILER WARNING BASELINE GATE PASSED (0 warnings across all targets) ==="
exit 0
