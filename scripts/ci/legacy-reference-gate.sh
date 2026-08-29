#!/usr/bin/env bash
# =============================================================================
# ASHFALL — Legacy Reference Gate (Ticket #124)
# =============================================================================
# Fails if active runtime code/resources/catalogs introduce new references
# to prohibited legacy uppercase Assets/ paths.
#
# Allowed exceptions (not flagged):
#   - Assets/StreamingAssets/...  (data authority, intentionally legacy-rooted)
#   - StreamingAssets/Data/...    (data authority references in docs/comments)
#   - Assets/Ashfall.Core/...     (Core library code, not a runtime asset)
#   - res://Assets/... fallbacks  (backward-compatible case normalization)
#   - project://database/Assets/... (Godot editor internal, not runtime)
#   - Test files (Ashfall.Core.Tests/)
#   - CI/maintenance scripts (scripts/, .mimocode/)
#   - Documentation (.md, .txt, .csv, .yaml, .yml)
#   - Build artifacts (build/, .godot/)
#   - Root tooling scripts (setup-repo.sh, export_code.py, generate_master_doc.py)
#
# Scanned extensions (runtime code/resources/catalogs only):
#   .cs .tscn .tres .json .uss .uxml .sh .py
#
# Exit 0 = clean; exit 1 = legacy references found.

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "${REPO_ROOT}"

# Directories to skip entirely
SKIP_DIRS=(
    ".git"
    ".godot"
    "node_modules"
    "tools"
    "docs"
    "deprecated_audits"
    "artifacts"
    "Ashfall.Core.Tests/TestResults"
    "assets/quarantine"
    ".claude"
    "Builds"
    "build"
    "Ashfall.Core.Tests"
    "scripts"
    ".mimocode"
)

# Root tooling files to skip (not active runtime code)
ROOT_TOOLING_FILES=(
    "setup-repo.sh"
    "export_code.py"
    "generate_master_doc.py"
)

# Build find exclusion args using -o between prune clauses
FIND_CMD=( find . )
for d in "${SKIP_DIRS[@]}"; do
    FIND_CMD+=( -path "*/$d/*" -prune -o )
done
FIND_CMD+=( -type f \( \
    -name "*.cs" -o \
    -name "*.tscn" -o \
    -name "*.tres" -o \
    -name "*.json" -o \
    -name "*.uss" -o \
    -name "*.uxml" -o \
    -name "*.sh" -o \
    -name "*.py" \
\) -print0 )

violations=0

while IFS= read -r -d '' file; do
    rel="${file#"$REPO_ROOT"/}"

    # Skip root tooling files
    for tool in "${ROOT_TOOLING_FILES[@]}"; do
        if [[ "$rel" == "./$tool" ]]; then
            continue 2
        fi
    done

    # Allow Core library and data authority paths entirely
    if [[ "$rel" == */Assets/Ashfall.Core/* || "$rel" == */Assets/StreamingAssets/* ]]; then
        continue
    fi

    # Check for Assets/ references
    if ! grep -n "Assets/" "$file" >/dev/null 2>&1; then
        continue
    fi

    # Determine if the reference is allowed
    # Allowed: data authority, Core library, case-normalization fallback, Godot editor paths
    allowed=true
    while IFS= read -r line; do
        # Allow data authority paths (with or without Assets/ prefix)
        if [[ "$line" == *"Assets/StreamingAssets"* || "$line" == *"StreamingAssets/Data"* ]]; then
            continue
        fi
        # Allow Core library references
        if [[ "$line" == *"Ashfall.Core"* ]]; then
            continue
        fi
        # Allow res://Assets/ case-normalization fallbacks
        if [[ "$line" == *"res://Assets/"* ]]; then
            continue
        fi
        # Allow Godot editor internal project://database/Assets/ paths
        if [[ "$line" == *"project://database/Assets/"* ]]; then
            continue
        fi
        # Allow doc comments and empty lines
        stripped="${line#*:}"
        stripped="$(echo "$stripped" | sed 's/^[[:space:]]*//')"
        if [[ "$stripped" == \#* || "$stripped" == //* || "$stripped" == /*\* || "$stripped" == --* ]]; then
            continue
        fi
        # If we reach here, the line contains a disallowed Assets/ reference
        allowed=false
        break
    done < <(grep -n "Assets/" "$file")

    if $allowed; then
        continue
    fi

    echo "❌ [FAIL] Legacy Assets/ reference in: $rel" >&2
    grep -n "Assets/" "$file" | sed "s|^|    |" >&2
    violations=$((violations + 1))
done < <("${FIND_CMD[@]}" 2>/dev/null)

if [[ "$violations" -gt 0 ]]; then
    echo "" >&2
    echo "❌ Legacy Reference Gate FAILED ($violations file(s) with legacy Assets/ references)." >&2
    echo "   Remediation: rewrite references to canonical assets/ root." >&2
    exit 1
fi

echo "✅ Legacy Reference Gate PASSED (0 legacy Assets/ references in active runtime code/resources)."
exit 0
