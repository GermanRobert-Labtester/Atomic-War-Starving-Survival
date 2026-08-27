#!/usr/bin/env bash
# SPDX-License-Identifier: MIT
# generate-architecture-map.sh — Architecture Test Map Generator & CI Gate
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "${REPO_ROOT}"

python3 "${SCRIPT_DIR}/generate-architecture-map.py" "$@"
