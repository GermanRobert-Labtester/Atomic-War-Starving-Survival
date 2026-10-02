#!/usr/bin/env bash
# =============================================================================
# ASHFALL — Regenerate all generated documentation/catalog artifacts
# =============================================================================
# Every drift gate (pot_template_drift, save_store_matrix_drift,
# ui_design_map_drift, plan_integration_audit_drift, gate_inventory_drift,
# docs_index_drift) fails when its generated artifact is stale. The order below
# matters: the Master Documentation Index must be regenerated LAST, because it
# hashes every document, including the artifacts written by the earlier steps.
# Regenerating the index first and another artifact second leaves the index stale
# again (observed repeatedly during the Oct-2 waves).
#
# Usage:
#   bash scripts/ci/regen-generated-docs.sh          # regenerate everything
#   bash scripts/ci/regen-generated-docs.sh --check   # run each --check instead
# =============================================================================

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "${REPO_ROOT}"

CHECK=0
if [ "${1:-}" = "--check" ]; then
    CHECK=1
fi

run() {
    if [ "${CHECK}" -eq 1 ]; then
        echo "── check: $*"
        "$@" --check
    else
        echo "── regen: $*"
        "$@"
    fi
}

# 1. Localization translator template (assets/l10n).
if [ "${CHECK}" -eq 1 ]; then
    python3 scripts/ci/generate_pot_template.py --check
else
    python3 scripts/ci/generate_pot_template.py
fi

# 2. Save-store contract matrix (docs/saves).
run bash scripts/ci/generate-save-store-matrix.sh

# 3. UI design map (docs/ui).
run python3 scripts/ci/generate-ui-design-map.py

# 4. Recent plan-integration audit (docs/plans).
run python3 scripts/ci/generate-plan-integration-audit.py

# 5. Gate inventory (docs/ci) — derives from CI_GATE_MANIFEST.json.
if [ "${CHECK}" -eq 1 ]; then
    python3 scripts/ci/run-gates.py --check-inventory docs/ci/GATE_INVENTORY.md
else
    python3 scripts/ci/run-gates.py --write-inventory docs/ci/GATE_INVENTORY.md
fi

# 6. Master documentation index — MUST be last.
run python3 scripts/ci/generate-docs-index.py

echo "── done."
