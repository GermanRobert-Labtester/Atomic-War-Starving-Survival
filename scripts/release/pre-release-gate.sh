#!/usr/bin/env bash
# scripts/release/pre-release-gate.sh — Plan 48 / C2[21] single pre-release gate
# =============================================================================
# One command for the whole pre-tag ceremony:
#
#   1. scripts/ci/release-gate.sh   — fast tier + full release-required gates
#   2. scripts/ci/export-build.sh   — export the shipping build + runtime smoke
#
# This is a thin composition only. It owns no gate logic and adds no direct
# gate command: release policy (tools/rstools ashfall-dev releasepolicy) still reads the
# canonical `bash scripts/ci/release-gate.sh` and `scripts/ci/export-build.sh`
# as the authorities. The tagged CI workflow runs the same two steps in two
# jobs; this wrapper exists so a local pre-tag run is one command.
#
# Usage:
#   bash scripts/release/pre-release-gate.sh
#   bash scripts/release/pre-release-gate.sh --skip-export   (fast local sanity)
#
# Exit codes:
#   0  both stages passed
#   1  a stage failed

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "$REPO_ROOT"

SKIP_EXPORT=false
while [[ $# -gt 0 ]]; do
    case "$1" in
        --skip-export) SKIP_EXPORT=true; shift ;;
        *) echo "Unknown argument: $1" >&2; exit 1 ;;
    esac
done

FAILURES=()

echo "========================================"
echo "  ASHFALL Pre-Release Gate"
echo "========================================"

echo ""
echo "[1/2] Release gate — fast tier + full release gates"
if ! bash scripts/ci/release-gate.sh; then
    FAILURES+=("release-gate.sh")
fi

if ! $SKIP_EXPORT; then
    echo ""
    echo "[2/2] Export smoke — shipping build + packaged runtime smoke"
    if ! bash scripts/ci/export-build.sh; then
        FAILURES+=("export-build.sh")
    fi
else
    echo ""
    echo "[2/2] Export smoke — SKIPPED (--skip-export)"
fi

echo ""
echo "========================================"
if [[ ${#FAILURES[@]} -eq 0 ]]; then
    echo "  PRE_RELEASE_GATE PASS"
    echo "========================================"
    exit 0
fi

echo "  PRE_RELEASE_GATE FAIL — failing stages:"
for f in "${FAILURES[@]}"; do
    echo "    - $f"
done
echo "========================================"
exit 1
