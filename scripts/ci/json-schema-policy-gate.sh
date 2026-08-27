#!/usr/bin/env bash
# =============================================================================
# json-schema-policy-gate.sh — JSON Schema Policy Gate
# =============================================================================
# Enforces that all new, changed, or existing JSON data authority files under
# Assets/StreamingAssets/Data/ declare a valid object-root and schema_version >= 1.
#
# Usage:
#   bash scripts/ci/json-schema-policy-gate.sh          # Full CI mode (all 403 files)
#   bash scripts/ci/json-schema-policy-gate.sh --staged # Pre-commit hook mode (staged files only)
#   bash scripts/ci/json-schema-policy-gate.sh --diff   # Working tree changes vs HEAD
# =============================================================================

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "${REPO_ROOT}"

python3 scripts/ci/json-schema-policy-gate.py "$@"
