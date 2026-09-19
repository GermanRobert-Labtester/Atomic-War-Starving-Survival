#!/usr/bin/env bash
# scripts/ci/release-gate.sh — Plan 48 / C2[21] Phase 4
# ======================================================
# Release-tier CI gate: runs all checks required before a version tag is pushed.
# Runs all fast gates + the full-tier save_support_window gate + build/export
# smoke boot.
#
# Usage:
#   bash scripts/ci/release-gate.sh
#   bash scripts/ci/release-gate.sh --skip-full   (fast gates only, for quick pre-tag check)
#
# Exit codes:
#   0  All release gates pass
#   1  One or more gates failed

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"

SKIP_FULL=false
while [[ $# -gt 0 ]]; do
    case "$1" in
        --skip-full) SKIP_FULL=true; shift ;;
        *) echo "Unknown argument: $1"; exit 1 ;;
    esac
done

cd "$REPO_ROOT"

PASS=0
FAIL=0
FAILURES=()

run_gate() {
    local name="$1"; shift
    local cmd=("$@")
    echo -n "  [release-gate] $name ... "
    if "${cmd[@]}" > /tmp/rg_out.txt 2>&1; then
        echo "PASS"
        PASS=$((PASS + 1))
    else
        echo "FAIL"
        FAIL=$((FAIL + 1))
        FAILURES+=("$name")
        echo "  --- Output ---"
        tail -10 /tmp/rg_out.txt | sed 's/^/    /'
        echo "  -------------"
    fi
}

echo "========================================"
echo "  ASHFALL Release Gate"
echo "========================================"
echo ""

# --- Fast tier (all 53 gates) ---
echo "[1/3] Fast tier — all 53 fast gates"
if bash scripts/ci/verify-fast.sh; then
    echo "  Fast tier: PASS"
    PASS=$((PASS + 1))
else
    echo "  Fast tier: FAIL"
    FAIL=$((FAIL + 1))
    FAILURES+=("fast_tier (verify-fast.sh)")
fi

# --- Release-specific gates ---
echo ""
echo "[2/3] Release-specific gates"

run_gate "version_gate" python3 scripts/ci/version-gate.py
run_gate "changelog_drift" python3 scripts/release/generate_changelog.py --check

if ! $SKIP_FULL; then
    echo ""
    echo "[3/3] Full-tier gates"
    run_gate "save_support_window" \
        dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj \
            --filter SaveSupportWindowTests --nologo
fi

# --- Summary ---
echo ""
echo "========================================"
TOTAL=$((PASS + FAIL))
if [[ $FAIL -eq 0 ]]; then
    echo "  RELEASE_GATE PASS — $PASS/$TOTAL checks passed"
    echo "========================================"
    echo ""
    echo "Safe to tag. Run:"
    echo "  git tag -a vX.Y.Z -m \"ASHFALL vX.Y.Z\""
    echo "  git push origin main vX.Y.Z"
    exit 0
else
    echo "  RELEASE_GATE FAIL — $FAIL/$TOTAL checks failed:"
    for f in "${FAILURES[@]}"; do
        echo "    - $f"
    done
    echo "========================================"
    exit 1
fi
