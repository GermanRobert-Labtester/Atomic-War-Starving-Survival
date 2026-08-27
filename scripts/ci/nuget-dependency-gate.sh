#!/usr/bin/env bash
# SPDX-License-Identifier: MIT
# nuget-dependency-gate.sh — Central Package Management CI Gate
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "${REPO_ROOT}"

echo "── RUNNING CENTRAL PACKAGE MANAGEMENT GATE ──"
python3 "${SCRIPT_DIR}/nuget-dependency-report.py" --check
