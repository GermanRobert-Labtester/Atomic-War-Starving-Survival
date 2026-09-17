#!/usr/bin/env bash
# =============================================================================
# ASHFALL — C1 Plan 19: UID Sidecar CI Gate (INV-19.6 / 19C-A.2)
# =============================================================================
# Scans src/ and Assets/ for *.cs.uid files and ensures each has an existing
# sibling *.cs file. Prevents orphaned sidecars from accumulating when source
# files are deleted or moved.
#
# Exit codes:
#   0 - Clean (all .cs.uid files have existing sibling .cs files)
#   1 - Dangling sidecars detected
# =============================================================================

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "${REPO_ROOT}"

echo "=== UID Sidecar Gate ==="
echo "Scanning src/ and Assets/ for dangling *.cs.uid sidecars..."

dangling=()

# Scan src and Assets for *.cs.uid files
while IFS= read -r uid_file; do
    [ -z "$uid_file" ] && continue
    cs_file="${uid_file%.uid}"
    if [ ! -f "$cs_file" ]; then
        dangling+=("$uid_file")
    fi
done < <(find Assets src -type f -name "*.cs.uid" | sort)

count="${#dangling[@]}"

if [ "$count" -gt 0 ]; then
    echo "ERROR: Found $count dangling .cs.uid sidecar(s) without sibling .cs file:"
    for path in "${dangling[@]}"; do
        echo "  - $path (expected ${path%.uid})"
    done
    echo "UID Sidecar Gate: FAIL"
    exit 1
fi

echo "UID Sidecar Gate: PASS (0 dangling .cs.uid sidecars found)"
exit 0
