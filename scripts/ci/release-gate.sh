#!/usr/bin/env bash
# scripts/ci/release-gate.sh — Plan 48 / C2[21] Phase 4
# ======================================================
# Release-tier CI gate: runs all checks required before a version tag is
# pushed. Runs the full fast tier (docs/ci/CI_GATE_MANIFEST.json) plus the
# release-required full-tier gates through the canonical runner. Do not add
# direct gate commands here — every gate this script needs must come from
# the manifest via verify-fast.sh / run-gates.py, or the release policy
# monitor (tools/gotools/cmd/releasepolicy) will flag drift.
#
# Usage:
#   bash scripts/ci/release-gate.sh
#   bash scripts/ci/release-gate.sh --skip-full   (fast gates only, for a
#                                                   developer's quick local
#                                                   pre-tag sanity check —
#                                                   the release workflow must
#                                                   never pass this flag)
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

echo "========================================"
echo "  ASHFALL Release Gate"
echo "========================================"
echo ""

# --- Fast tier (every gate in CI_GATE_MANIFEST.json classified "fast") ---
echo "[1/2] Fast tier — all fast gates (scripts/ci/verify-fast.sh)"
if bash scripts/ci/verify-fast.sh; then
    echo "  Fast tier: PASS"
    PASS=$((PASS + 1))
else
    echo "  Fast tier: FAIL"
    FAIL=$((FAIL + 1))
    FAILURES+=("fast_tier (verify-fast.sh)")
fi

# --- Full-tier release-required gates (canonical runner, not duplicated
#     direct commands) ---
if ! $SKIP_FULL; then
    echo ""
    echo "[2/2] Full-tier release gates — test_core_suite + save_support_window"
    if python3 scripts/ci/run-gates.py \
        --gate test_core_suite,save_support_window \
        --report-json build/reports/release-full-results.json; then
        echo "  Full-tier release gates: PASS"
        PASS=$((PASS + 1))
    else
        echo "  Full-tier release gates: FAIL"
        FAIL=$((FAIL + 1))
        FAILURES+=("full_tier (test_core_suite,save_support_window)")
    fi
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
