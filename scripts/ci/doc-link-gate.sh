#!/usr/bin/env bash
# SPDX-License-Identifier: MIT
# doc-link-gate.sh — Validates portable relative links across all documentation.
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "${REPO_ROOT}"

echo "── RUNNING PORTABLE DOC LINK GATE ──"
python3 "${SCRIPT_DIR}/normalize-doc-links.py" --check
echo "=== PORTABLE DOC LINK GATE PASSED ==="
