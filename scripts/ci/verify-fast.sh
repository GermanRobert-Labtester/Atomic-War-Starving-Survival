#!/usr/bin/env bash
# =============================================================================
# ASHFALL — Local Fast-Tier Verification Runner
# =============================================================================
# Executes all fast-tier verification gates from docs/ci/CI_GATE_MANIFEST.json
# using the canonical gate runner scripts/ci/run-gates.py.
#
# Usage:
#   bash scripts/ci/verify-fast.sh              # Runs fast-tier gates
#   bash scripts/ci/verify-fast.sh --list       # Lists all registered gates
#   bash scripts/ci/verify-fast.sh --gate <id>  # Runs specific gate
# =============================================================================

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "${REPO_ROOT}"

python3 "${SCRIPT_DIR}/run-gates.py" --tier fast \
  --report-json "${REPO_ROOT}/build/reports/gate-results.json" \
  --fail-artifact "${REPO_ROOT}/build/reports/CI_GATE_FAILURE_REPORT.md" \
  "$@"
