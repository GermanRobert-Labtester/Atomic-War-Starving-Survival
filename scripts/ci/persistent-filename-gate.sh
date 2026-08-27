#!/usr/bin/env bash
# SPDX-License-Identifier: MIT
# persistent-filename-gate.sh — Persistent Filename Uniqueness & Save-Section Registry Gate
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "${REPO_ROOT}"

echo "── RUNNING PERSISTENT FILENAME UNIQUENESS & REGISTRY GATE ──"
python3 "${SCRIPT_DIR}/persistent-filename-gate.py"
