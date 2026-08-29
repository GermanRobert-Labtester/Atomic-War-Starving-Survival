#!/usr/bin/env bash
# =============================================================================
# ASHFALL — Legacy Asset Path Gate (Ticket #124)
# =============================================================================
# Fails if active runtime assets exist under prohibited legacy roots:
#   Assets/art
#   Assets/sprites
#   Assets/ui
#   Assets/audio
#
# Allowed exceptions:
#   - .gdignore (Godot ignore markers that protect the empty legacy dirs)
#   - .gitkeep (empty directory placeholders)
#   - Documentation/tooling files outside runtime asset roots
#
# Exit 0 = clean; exit 1 = prohibited runtime assets found.

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "${REPO_ROOT}"

PROHIBITED_ROOTS=(
    "Assets/art"
    "Assets/sprites"
    "Assets/ui"
    "Assets/audio"
)

violations=0

for root in "${PROHIBITED_ROOTS[@]}"; do
    if [[ ! -d "$root" ]]; then
        continue
    fi
    while IFS= read -r -d '' file; do
        rel="${file#"$REPO_ROOT"/}"
        echo "❌ [FAIL] Prohibited runtime asset under legacy root: $rel" >&2
        violations=$((violations + 1))
    done < <(find "$root" -type f \
        ! -name ".gdignore" \
        ! -name ".gitkeep" \
        ! -name "*.md" \
        ! -name "*.txt" \
        ! -name "*.json" \
        -print0 2>/dev/null)
done

if [[ "$violations" -gt 0 ]]; then
    echo "" >&2
    echo "❌ Legacy Asset Path Gate FAILED ($violations prohibited file(s) found)." >&2
    echo "   Remediation: migrate runtime assets to canonical assets/ root." >&2
    exit 1
fi

echo "✅ Legacy Asset Path Gate PASSED (0 prohibited runtime assets under Assets/art, Assets/sprites, Assets/ui, Assets/audio)."
exit 0
